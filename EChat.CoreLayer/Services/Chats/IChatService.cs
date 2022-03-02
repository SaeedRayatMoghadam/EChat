using System.Collections.Generic;
using System.Threading.Tasks;
using EChat.CoreLayer.ViewModels.Chats;
using EChat.DataLayer.Entities.Chats;

namespace EChat.CoreLayer.Services.Chats
{
    public interface IChatService
    {
        Task SendMessage(Chat chat);
        Task<List<ChatViewModel>> GetAll(long groupId);
    }
}