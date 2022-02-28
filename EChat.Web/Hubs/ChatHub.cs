using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EChat.CoreLayer.Services.Chats.ChatGroups;
using EChat.CoreLayer.Utilities;
using EChat.Web.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace EChat.Web.Hubs
{
    public class ChatHub:Hub,IChatHub
    {
        private readonly IChatGroupService _chatGroupService;

        public ChatHub(IChatGroupService chatGroupService)
        {
            _chatGroupService = chatGroupService;
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

        public async Task CreateGroup(string groupName)
        {
            try
            {
                var group = await _chatGroupService.Create(groupName, Context.User.GetUserId());
                await Clients.Caller.SendAsync("NewGroup", groupName, group.Token);
            }
            catch
            {
                await Clients.Caller.SendAsync("NewGroup", "Error");
            }
        }
    }
}