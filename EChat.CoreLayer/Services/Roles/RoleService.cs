using EChat.DataLayer.Context;

namespace EChat.CoreLayer.Services.Roles
{
    public class RoleService : BaseService,IRoleService
    {
        public RoleService(EChatContext context) : base(context)
        {
        }
    }
}