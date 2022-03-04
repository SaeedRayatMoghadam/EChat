using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EChat.CoreLayer.Services.Chats;
using EChat.CoreLayer.Services.Chats.ChatGroups;
using EChat.CoreLayer.Services.Users;
using EChat.CoreLayer.Services.Users.UserGroups;
using EChat.CoreLayer.Utilities;
using EChat.CoreLayer.ViewModels.Chats;
using EChat.DataLayer.Entities.Chats;
using EChat.Web.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace EChat.Web.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly IChatGroupService _chatGroupService;
        private readonly IUserGroupService _userGroupService;
        private readonly IChatService _chatService;
        private readonly IUserService _userService;

        public ChatHub(IChatGroupService chatGroupService, IUserGroupService userGroupService, IChatService chatService, IUserService userService)
        {
            _chatGroupService = chatGroupService;
            _userGroupService = userGroupService;
            _chatService = chatService;
            _userService = userService;
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Welcome", Context.User.GetUserId());
            return base.OnConnectedAsync();
        }

        public async Task Greeting()
        {
            await Clients.All.SendAsync("Welcome", "Hello");
        }

        //public async Task SendMessage(string message)
        //{
        //    var user = Context.User.FindFirstValue(ClaimTypes.Name);
        //    await Clients.All.SendAsync("SendMessage", $"{user} : {message}");
        //}


        public async Task JoinGroup(string token, long currentGroupId)
        {
            var group = await _chatGroupService.Get(token);
            var groupDto = FixGroupModel(group);
            if (group == null)
                await Clients.Caller.SendAsync("Error", "Group Not Found");
            else
            {
                var chats = await _chatService.GetAll(group.Id);
                if (!await _userGroupService.IsUserInGroup(Context.User.GetUserId(), token))
                {
                    await _userGroupService.JoinGroup(Context.User.GetUserId(), group.Id);
                    await Clients.Caller.SendAsync("NewGroup", groupDto.Title, groupDto.Token, groupDto.ImageUrl);
                }
                if (currentGroupId > 0)
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentGroupId.ToString());

                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                await Clients.Caller.SendAsync("JoinGroup", groupDto, chats);
            }
        }

        public async Task SendMessage(string text, long groupId)
        {
            var group = await _chatGroupService.Get(groupId);
            if (group == null)
                return;

            var chat = new Chat()
            {
                Body = text,
                GroupId = groupId,
                CreateDate = DateTime.Now,
                UserId = Context.User.GetUserId()
            };

            await _chatService.SendMessage(chat);
            chat.CreateDate = DateTime.Now.Date;

            var chatModel = new ChatViewModel()
            {
                Body = text,
                UserName = Context.User.GetUserName(),
                CreateDate = $"{chat.CreateDate.Hour}:{chat.CreateDate.Minute}",
                UserId = Context.User.GetUserId(),
                GroupName = group.Title,
                GroupId = groupId
            };

            var userIDs = await _userService.GetUsersIDs(groupId);

            await Clients.Users(userIDs).SendAsync("SendNotification", chatModel);

            await Clients.Groups(groupId.ToString()).SendAsync("SendMessage", chatModel);
        }

        public async Task JoinPrivateGroup(long receiverId, long currentGroupId)
        {
            if (currentGroupId > 0)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentGroupId.ToString());

            var group = await _chatGroupService.CreatePrivateGroup(Context.User.GetUserId(), receiverId);
            var groupDto = FixGroupModel(group);

            if (!await _userGroupService.IsUserInGroup(Context.User.GetUserId(), group.Token))
            {
                await _userGroupService.JoinGroup(new List<long>()
                    { groupDto.ReceiverId ?? 0, group.OwnerId }, group.Id);

                await Clients.Caller.SendAsync("NewGroup", groupDto.Title, groupDto.Token, groupDto.ImageUrl);
                await Clients.User(groupDto.ReceiverId.ToString()).SendAsync("NewGroup", Context.User.GetUserName(), groupDto.Token, groupDto.ImageUrl);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());


            var chats = await _chatService.GetAll(group.Id);


            await Clients.Caller.SendAsync("JoinGroup", groupDto, chats);
        }

        #region Utils

        private ChatGroup FixGroupModel(ChatGroup model)
        {
            if (model.IsPrivate)
            {
                if (model.OwnerId == Context.User.GetUserId())
                {
                    return new ChatGroup()
                    {
                        Id = model.Id,
                        Title = model.Receiver.UserName,
                        Token = model.Token,
                        CreateDate = model.CreateDate,
                        ImageUrl = model.Receiver.Avatar,
                        IsPrivate = false,
                        OwnerId = model.OwnerId,
                        ReceiverId = model.ReceiverId,

                    };
                }
                return new ChatGroup()
                {
                    Id = model.Id,
                    Title = model.User.UserName,
                    Token = model.Token,
                    CreateDate = model.CreateDate,
                    ImageUrl = model.User.Avatar,
                    IsPrivate = false,
                    OwnerId = model.OwnerId,
                    ReceiverId = model.ReceiverId,

                };
            }

            return new ChatGroup()
            {
                Id = model.Id,
                Token = model.Token,
                Title = model.Title,
                CreateDate = model.CreateDate,
                ImageUrl = model.ImageUrl,
                IsPrivate = false,
                OwnerId = model.OwnerId,
                ReceiverId = model.ReceiverId,
            };
        }

        #endregion
    }
}
