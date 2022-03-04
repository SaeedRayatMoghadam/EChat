using System.Threading.Tasks;

namespace EChat.Web.Hubs.Interfaces
{
    public interface IChatHub
    {
        Task JoinGroup(string token, long currentGroupId);
        Task SendMessage(string text, long groupId);
        Task JoinPrivateGroup(long receiverId, long currentGroupId);
    }
}