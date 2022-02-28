using EChat.DataLayer.Entities.Chats;

namespace EChat.CoreLayer.ViewModels.Chats
{
    public class UserGroupViewModel
    {
        public string GroupName { get; set; }
        public string ImageUrl { get; set; }
        public Chat LastChat { get; set; }
        public string Token { get; set; }
    }
}