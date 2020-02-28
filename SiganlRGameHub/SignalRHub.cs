using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SignalRGameHub
{
    class SignalRHub : Hub
    {
        public void Send(string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Others.broadcastMessage(message);
        }
    }
}