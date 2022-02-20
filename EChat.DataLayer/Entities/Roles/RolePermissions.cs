using System.ComponentModel.DataAnnotations.Schema;
using EChat.DataLayer.Enums;

namespace EChat.DataLayer.Entities.Roles
{
    public class RolePermissions : BaseEntity
    {
        public long RoleId { get; set; }
        public Permissions Permissions { get; set; }

        #region Relations

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        #endregion
    }
}