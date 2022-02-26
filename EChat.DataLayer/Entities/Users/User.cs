using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EChat.DataLayer.Entities.Chats;

namespace EChat.DataLayer.Entities.Users
{
    public class User : BaseEntity
    {
        [MaxLength(30)]
        public string UserName { get; set; }

        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(60)]
        public string Avatar { get; set; }

        #region Relations

        public ICollection<ChatGroup> ChatGroups { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }


        #endregion
    }
}