using System.Collections.Generic;
using System.Threading.Tasks;
using EChat.DataLayer.Entities.Chats;

namespace EChat.CoreLayer.Services.Chats
{
    public interface IChatService
    {
        Task SendMessage(Chat chat);
        Task<List<Chat>> GetAll(long groupId);
    }
}