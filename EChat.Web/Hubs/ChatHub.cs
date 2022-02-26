using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace EChat.Web.Hubs
{
    public class ChatHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("Welcome", "Hello");
            return base.OnConnectedAsync();
        }

        public async Task Greeting()
        {
            await Clients.All.SendAsync("Welcome", "Hello");
        }
    }
}