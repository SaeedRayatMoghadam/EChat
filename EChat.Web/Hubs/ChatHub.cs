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
            if (group == null)
                await Clients.Caller.SendAsync("Error", "Group NOT Found");
            else
            {
                var chats = await _chatService.GetAll(group.Id);
                if (!await _userGroupService.IsUserInGroup(Context.User.GetUserId(), token))
                {
                    await _userGroupService.JoinGroup(Context.User.GetUserId(), group.Id);
                    await Clients.Caller.SendAsync("NewGroup", group.Title,group.Token,group.ImageUrl);
                }

                if(currentGroupId > 0)
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId,currentGroupId.ToString());
            
                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                await Clients.Caller.SendAsync("JoinGroup", group, chats);
            }
        }

        public async Task SendMessage(string text, long groupId)
        {
            var group = await _chatGroupService.Get(groupId);
            if(group == null)
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
                CreateDate = $"{chat.CreateDate.Hour} : {chat.CreateDate.Minute}",
                UserId = Context.User.GetUserId(),
                GroupName = group.Title,
                GroupId = groupId
            };

            var userIDs = await _userService.GetUsersIDs(groupId);

            await Clients.Users(userIDs).SendAsync("SendNotification", chatModel);

            await Clients.Groups(groupId.ToString()).SendAsync("SendMessage", chatModel);
        }
    }
}
