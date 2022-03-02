using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EChat.CoreLayer.ViewModels.Chats;
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

        public async Task<List<ChatViewModel>> GetAll(long groupId)
        {
            return await Table<Chat>().Where(c => c.GroupId == groupId)
                .Include(c => c.ChatGroup)
                .Include(c => c.User)
                .Select(c => new ChatViewModel()
                {
                    UserName = c.User.UserName,
                    CreateDate = $"{c.CreateDate.Hour} : {c.CreateDate.Minute}",
                    Body = c.Body,
                    GroupName = c.ChatGroup.Title,
                    GroupId = c.GroupId,
                    UserId = c.UserId
                })
                .ToListAsync();
        }
        
    }
}