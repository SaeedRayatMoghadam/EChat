using System.Collections.Generic;
using System.Threading.Tasks;
using EChat.CoreLayer.ViewModels.Chats;
using EChat.DataLayer.Entities.Chats;

namespace EChat.CoreLayer.Services.Chats.ChatGroups
{
    public interface IChatGroupService
    {
        Task<List<ChatGroup>> GetAll(long userId);
        Task<ChatGroup> Create(CreateGroupViewModel model);
        Task<List<SearchResultViewModel>> Search(string searchText);
    }
}