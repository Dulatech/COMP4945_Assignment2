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
            bool invitationRcvd = false;
            Guid gameToJoin = Guid.Empty;
            int playerNumToJoin = -1;
            var subscription = NetworkController.myHub.On<string>("broadcastMessage", (msg1) =>
            {
                if (!invitationRcvd)
                {
                    StringReader reader = new StringReader(msg1);
                    // continue unless first line is HEADER and also the second line is 0
                    if (!reader.ReadLine().Equals(Sender.HEADER) || !reader.ReadLine().Equals("0"))
                        return;
                    string msg = reader.ReadLine();
                    System.Diagnostics.Debug.WriteLine(msg + "\n");
                    string[] payload = msg.Split(',');
                    gameToJoin = Guid.Parse(payload[0]);
                    playerNumToJoin = int.Parse(payload[1]);
                    invitationRcvd = true;
                    Sender.SendJoinReq(gameToJoin, NetworkController.ID, playerNumToJoin);
                }
                else
                {
                    if (joining) return;
                    StringReader reader = new StringReader(msg1);
                    if (!reader.ReadLine().Equals(Sender.HEADER))
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
                                GameArea.currentNumOfPlayers = playerNumToJoin;
                                GameArea.playerNum = playerNumToJoin;
                                GameArea.gameID = gameToJoin;
                                joining = true;
                            }
                            else // type == 3
                            {
                                joining = false;
                            }
                        }
                    }
                }
            });
            Thread.Sleep(500);
            subscription.Dispose();
        }
    }
}
