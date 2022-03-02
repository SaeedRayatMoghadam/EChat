using System;

namespace EChat.CoreLayer.ViewModels.Chats
{
    public class ChatViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long GroupId { get; set; }
        public string GroupName { get; set; }
        public string Body { get; set; }
        public string CreateDate { get; set; }
    }
}