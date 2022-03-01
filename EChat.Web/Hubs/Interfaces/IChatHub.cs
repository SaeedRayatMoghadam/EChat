using System.Threading.Tasks;

namespace EChat.Web.Hubs.Interfaces
{
    public interface IChatHub
    {
        Task JoinGroup(string token);
    }
}