using System.Collections.Generic;
using System.Threading.Tasks;
using EChat.CoreLayer.ViewModels.Chats;
using EChat.DataLayer.Entities.Users;

namespace EChat.CoreLayer.Services.Users.UserGroups
{
    public interface IUserGroupService
    {
        Task<List<UserGroupViewModel>> GetAll(long userId);
        Task JoinGroup(UserGroup model);
    }
}