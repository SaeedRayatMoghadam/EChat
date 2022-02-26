using System.Threading.Tasks;
using EChat.CoreLayer.ViewModels;
using EChat.DataLayer.Entities.Users;

namespace EChat.CoreLayer.Services.Users
{
    public interface IUserService
    {
        Task<bool> IsUserExist(string userName);
        Task<bool> IsUserExist(long id);
        Task<bool> Register(RegisterViewModel model);
        Task<User> Login(LoginViewModel model); 
    }
}