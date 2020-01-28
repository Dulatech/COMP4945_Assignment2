using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace COMP4945_Assignment2
{
    class MulticastSender
    {
        public static readonly Guid ID = Guid.NewGuid();
        // just in case other applications use this port for multicast
        public static readonly string HEADER = "SOMETHING UNIQUE";
        static UdpClient sock;
        static readonly IPEndPoint iep = new IPEndPoint(IPAddress.Parse("239.50.50.51"), Program.PORT);
        static MulticastSender()
        {
            sock = new UdpClient();
        }
        private static void Send(int type, string msg)
        {
            if (type == -1) // game msg
            {
                msg = HEADER + "\n" + GameArea.gameID + "\n" + msg;
            } else
            {
                msg = HEADER + "\n" + type + "\n" + msg;
            }
            byte[] data = Encoding.ASCII.GetBytes(msg);
            sock.Send(data, data.Length, iep);
            if (type != 0)
                System.Diagnostics.Debug.WriteLine("Sent:\n" + msg + "\n");
        }
        public static void SendGameMsg(int type, string msg)
        {
            Send(-1, type + "," + ID + "," + GameArea.playerNum + "," + msg);
        }
        // multicast invitations every half a second
        // this method should be passed on to a new background thread only if the user is a host of a new game
        // i.e. playerNum == 0
        public static void SendInvitations()
        {
            while (true)
            {
                if (GameArea.currentNumOfPlayers < 4)
                    Send(0, GameArea.gameID + "," + GameArea.currentNumOfPlayers);
                Thread.Sleep(500);
            }
        }
        public static void SendJoinReq(Guid gameID, Guid reqID, int playerNum)
        {
            Send(1, gameID + "," + reqID + "," + playerNum);
        }
        public static void SendJoinResp(string reqId, bool accepted)
        {
            int type = accepted ? 2 : 3;
            Send(type, GameArea.gameID + "," + reqId);
        }
    }
}
