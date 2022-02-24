using System.Threading.Tasks;
using EChat.DataLayer.Context;
using EChat.DataLayer.Entities.Chats;

namespace EChat.CoreLayer.Services.Chats
{
    public class ChatService : BaseService,IChatService
    {
        public ChatService(EChatContext context) : base(context)
        {
        }

        public async Task SendMessage(Chat chat)
        {
            Insert(chat);
            await Save();
        }
    }
}