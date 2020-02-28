using Microsoft.AspNet.SignalR;

namespace SiganlRGameHub
{
    public class GameHub : Hub
    {
        public void Send(string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Others.broadcastMessage(message);
        }
    }
}