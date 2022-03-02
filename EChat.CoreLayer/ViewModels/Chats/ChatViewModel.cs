using System;

namespace EChat.CoreLayer.ViewModels.Chats
{
    public class ChatViewModel
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }
    }
}