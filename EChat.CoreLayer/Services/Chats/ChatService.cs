using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EChat.DataLayer.Context;
using EChat.DataLayer.Entities.Chats;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Chat>> GetAll(long groupId)
        {
            return await Table<Chat>().Where(c => c.GroupId == groupId).ToListAsync();
        }
    }
}