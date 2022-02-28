using System.ComponentModel.DataAnnotations.Schema;
using EChat.DataLayer.Entities.Chats;

namespace EChat.DataLayer.Entities.Users
{
    public class UserGroup : BaseEntity
    {
        public long UserId { get; set; }
        public long GroupId { get; set; }

        #region Relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("GroupId")]
        public ChatGroup ChatGroup { get; set; }

        #endregion
    }
}