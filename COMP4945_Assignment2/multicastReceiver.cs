using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.IO;
using COMP4945_Assignment2;

namespace NetworkComm
{
    class MulticastReceiver : Receiver
    {
        public static int PORT = 49452;
        NetworkController controller;
        Socket sock;
        EndPoint ep = (EndPoint)(MulticastSender.iep);
        //private static readonly object syncLock = new object();
        public bool IsHost { get; set; }
        public MulticastReceiver(NetworkController con)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.Bind(new IPEndPoint(IPAddress.Any, PORT));
            sock.MulticastLoopback = false;
            Debug.WriteLine("Listening on port " + PORT);
            sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse("239.50.50.51")));
            controller = con;
            IsHost = false;
        }
        public override void Run()
        {
            sock.MulticastLoopback = false;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024];
                    int recv = sock.ReceiveFrom(data, ref ep);
                    string stringData = Encoding.ASCII.GetString(data, 0, recv);
                    controller.OnMessageReceived(stringData);
                }
            }
            catch (SocketException e)
            {
                Debug.WriteLine("SocketException: " + e.Message);
                sock.Close();
            }
        }

        // joins or creates new game
        public override void EnterGame()
        {
            Debug.WriteLine("inside EnterGame()");
            sock.MulticastLoopback = true;
            bool joining = false;
            long until = DateTime.Now.Ticks + TimeSpan.TicksPerMillisecond * 1000;
            while (DateTime.Now.Ticks < until)
            {
                try
                {
                    byte[] data = new byte[1024];
                    int recv = sock.ReceiveFrom(data, ref ep);
                    string stringData = Encoding.ASCII.GetString(data, 0, recv);
                    StringReader reader = new StringReader(stringData);
                    // continue unless first line is HEADER and also the second line is 0
                    if (!reader.ReadLine().Equals(SenderAPI.HEADER) || !reader.ReadLine().Equals("0"))
                        continue;
                    string msg = reader.ReadLine();
                    Debug.WriteLine("{0}\n{1}\n", ep.ToString(), msg);
                    string[] payload = msg.Split(',');
                    if (joining = TryJoin(Guid.Parse(payload[0]), int.Parse(payload[1]))) // successfully joined game
                    {
                        break;
                    }
                }
                catch (SocketException e)
                {
                    Debug.WriteLine("SocketException: " + e.Message);
                }
            }
            if (!joining)
                controller.CreateNewGame();
        }
        // returns true for connected, false for failure
        private bool TryJoin(Guid gameToJoin, int playerNum)
        {
            // Send Join Request
            long until = DateTime.Now.Ticks + TimeSpan.TicksPerMillisecond * 500;
            byte[] data = new byte[1024];
            SenderAPI.SendJoinReq(gameToJoin, NetworkController.ID, playerNum);
            while (DateTime.Now.Ticks < until)
            {
                int recv = sock.ReceiveFrom(data, ref ep);
                string stringData = Encoding.ASCII.GetString(data, 0, recv);
                StringReader reader = new StringReader(stringData);
                if (!reader.ReadLine().Equals(SenderAPI.HEADER))
                    continue;
                string secondLine = reader.ReadLine();
                if (int.TryParse(secondLine, out int type) && (type == 2 || type == 3))
                {
                    string s = reader.ReadLine();
                    string[] ar = s.Split(',');
                    if (Guid.Parse(ar[0]) == gameToJoin && Guid.Parse(ar[1]) == NetworkController.ID)
                    {
                        if (type == 2)
                        {
                            Debug.WriteLine("joining game");
                            GameArea.currentNumOfPlayers = playerNum;
                            GameArea.playerNum = playerNum;
                            GameArea.gameID = gameToJoin;
                            return true;
                        } else // type == 3
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
    }
}
