using System.ComponentModel.DataAnnotations.Schema;
using EChat.DataLayer.Entities.Users;

namespace EChat.DataLayer.Entities.Chats
{
    public class Chat : BaseEntity
    {
        public long GroupId { get; set; }
        public string Body { get; set; }
        public long UserId { get; set; }

        #region Relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("GroupId")]
        public ChatGroup ChatGroup { get; set; }

        #endregion
    }
}