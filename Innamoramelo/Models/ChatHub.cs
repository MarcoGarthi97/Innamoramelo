using Microsoft.AspNetCore.SignalR;

namespace Innamoramelo.Models
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string userId)
        {
            await Clients.User(userId).SendAsync("GetNewMassege");
            //await Clients.All.SendAsync("ReceiveMessage", userId, message);
        }
    }
}
