using System;
using COMP4945_Assignment2;
using Microsoft.AspNet.SignalR.Client;
using System.IO;
using System.Threading;

namespace NetworkComm
{
    class WebSocketReceiver : Receiver
    {
        NetworkController controller;
        public WebSocketReceiver(NetworkController con)
        {
            controller = con;
        }
        public override void Run()
        {
            NetworkController.myHub.On<string>("broadcastMessage", (msg) => {
                controller.OnMessageReceived(msg);
            });
        }
        public override void EnterGame()
        {
            System.Diagnostics.Debug.WriteLine("inside EnterGame()");
            bool joining = false;
            var subscription = NetworkController.myHub.On<string>("broadcastMessage", (msg1) =>
            {
                StringReader reader = new StringReader(msg1);
                // continue unless first line is HEADER and also the second line is 0
                if (!reader.ReadLine().Equals(SenderAPI.HEADER) || !reader.ReadLine().Equals("0"))
                    return;
                string msg = reader.ReadLine();
                System.Diagnostics.Debug.WriteLine("{0}\n", msg);
                string[] payload = msg.Split(',');
                if (joining = TryJoin(Guid.Parse(payload[0]), int.Parse(payload[1]))) // successfully joined game
                {
                    return;
                }
            });
            Thread.Sleep(1000);
            subscription.Dispose();
            if (!joining)
                controller.CreateNewGame();
        }

        // returns true for connected, false for failure
        private bool TryJoin(Guid gameToJoin, int playerNum)
        {
            bool joined = false;
            var subscription = NetworkController.myHub.On<string>("broadcastMessage", (msg) =>
            {
                StringReader reader = new StringReader(msg);
                if (!reader.ReadLine().Equals(SenderAPI.HEADER))
                    return;
                string secondLine = reader.ReadLine();
                if (int.TryParse(secondLine, out int type) && (type == 2 || type == 3))
                {
                    string s = reader.ReadLine();
                    string[] ar = s.Split(',');
                    if (Guid.Parse(ar[0]) == gameToJoin && Guid.Parse(ar[1]) == NetworkController.ID)
                    {
                        if (type == 2)
                        {
                            System.Diagnostics.Debug.WriteLine("joining game");
                            GameArea.currentNumOfPlayers = playerNum;
                            GameArea.playerNum = playerNum;
                            GameArea.gameID = gameToJoin;
                            joined = true;
                        }
                        else // type == 3
                        {
                            joined = false;
                        }
                    }
                }
            });
            SenderAPI.SendJoinReq(gameToJoin, NetworkController.ID, playerNum);
            Thread.Sleep(500);
            subscription.Dispose();
            return joined;
        }
    }
}
