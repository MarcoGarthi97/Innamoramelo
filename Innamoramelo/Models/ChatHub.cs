using Microsoft.AspNetCore.SignalR;

namespace Innamoramelo.Models
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string receiverId, string senderId)
        {
            await Clients.User(receiverId).SendAsync("GetNewMassege", senderId);
        }
    }
}
