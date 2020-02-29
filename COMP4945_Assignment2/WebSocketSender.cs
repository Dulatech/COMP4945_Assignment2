using Microsoft.AspNet.SignalR.Client;
using System.Diagnostics;
using COMP4945_Assignment2;

namespace NetworkComm
{
    class WebSocketSender
    {
        public static void SendMsg(string msg)
        {
            NetworkController.myHub.Invoke<string>("Send", msg).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Debug.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }
            });
        }

        // needs to be synchronous since application will exit after calling this function
        public static void SendDisconnect(string msg)
        {
            NetworkController.myHub.Invoke<string>("Send", msg).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Debug.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }
            }).Wait();
        }
    }
}
