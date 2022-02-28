using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EChat.DataLayer.Entities.Users;

namespace EChat.DataLayer.Entities.Chats
{
    public class ChatGroup : BaseEntity
    {
        [MaxLength(80)]
        public string Title { get; set; }

        [MaxLength(110)]
        public string Token { get; set; }

        [MaxLength(100)]
        public string ImageUrl { get; set; }

        public long OwnerId { get; set; }

        #region Relations

        [ForeignKey("OwnerId")]
        public User User { get; set; }

        public ICollection<Chat> Chats { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }


        #endregion
    }
}