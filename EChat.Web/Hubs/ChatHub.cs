using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EChat.CoreLayer.Services.Chats;
using EChat.CoreLayer.Services.Chats.ChatGroups;
using EChat.CoreLayer.Services.Users.UserGroups;
using EChat.CoreLayer.Utilities;
using EChat.CoreLayer.ViewModels.Chats;
using EChat.Web.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace EChat.Web.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly IChatGroupService _chatGroupService;
        private readonly IUserGroupService _userGroupService;
        private readonly IChatService _chatService;

        public ChatHub(IChatGroupService chatGroupService, IUserGroupService userGroupService, IChatService chatService)
        {
            _chatGroupService = chatGroupService;
            _userGroupService = userGroupService;
            _chatService = chatService;
        }

        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("Welcome", "Hello");
            return base.OnConnectedAsync();
        }

        public async Task Greeting()
        {
            await Clients.All.SendAsync("Welcome", "Hello");
        }

        public async Task SendMessage(string message)
        {
            var user = Context.User.FindFirstValue(ClaimTypes.Name);
            await Clients.All.SendAsync("SendMessage", $"{user} : {message}");
        }


        public async Task JoinGroup(string token)
        {
            var group = await _chatGroupService.Get(token);
            if (group == null)
                await Clients.Caller.SendAsync("Error", "Group NOT Found");
            else
            {
                if (!await _userGroupService.IsUserInGroup(Context.User.GetUserId(), token))
                {
                    await _userGroupService.JoinGroup(Context.User.GetUserId(), group.Id);
                    await Clients.Caller.SendAsync("NewGroup", group.Title,group.Token,group.ImageUrl);
                }
            
                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                await Clients.Group(group.Id.ToString()).SendAsync("JoinGroup", group, group.Chats);
            }
        }

    }
}
