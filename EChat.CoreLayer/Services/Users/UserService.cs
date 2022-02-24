using EChat.DataLayer.Context;

namespace EChat.CoreLayer.Services.Users
{
    public class UserService : BaseService,IUserService
    {
        public UserService(EChatContext context) : base(context)
        {
        }
    }
}