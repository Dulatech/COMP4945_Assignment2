using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace COMP4945_Assignment2
{
    class MulticastReceiver
    {
        GameArea form;
        Socket sock;
        EndPoint ep = (EndPoint)(new IPEndPoint(IPAddress.Parse("239.50.50.51"), Program.PORT));
        //private static readonly object syncLock = new object();
        public bool IsHost { get; set; }
        public MulticastReceiver(GameArea f)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.Bind(new IPEndPoint(IPAddress.Any, Program.PORT));
            sock.MulticastLoopback = false;
            Debug.WriteLine("Listening on port " + Program.PORT);
            sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse("239.50.50.51")));
            form = f;
            IsHost = false;
        }
        public void run()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024];
                    int recv = sock.ReceiveFrom(data, ref ep);
                    string stringData = Encoding.ASCII.GetString(data, 0, recv);
                    StringReader reader = new StringReader(stringData);
                    if (!reader.ReadLine().Equals(MulticastSender.HEADER))
                        continue;
                    Debug.WriteLine("{0}\n{1}\n", ep.ToString(), stringData);
                    string secondLine = reader.ReadLine();
                    if (secondLine.Length == 1)
                    {
                        if (IsHost && int.TryParse(secondLine, out int type) && type == 1)
                            HandleJoinReq(reader.ReadLine());
                        else
                            continue;
                    } else
                    {
                        if (Guid.Parse(secondLine) == GameArea.gameID)
                            HandleGameMsg(reader.ReadLine());
                    }
                }
            }
            catch (SocketException e)
            {
                Debug.WriteLine("SocketException: " + e.Message);
                sock.Close();
            }
        }
        private void HandleJoinReq(string msg)
        {
            string[] ar = msg.Split(',');
            if (Guid.Parse(ar[0]) == GameArea.gameID) // don't respond to other game id requests
            {
                int n = int.Parse(ar[2]);
                MulticastSender.SendJoinResp(ar[1], (n == GameArea.currentNumOfPlayers && n <= GameArea.MAX_PLAYERS));
            }
        }
        private void HandleGameMsg(string msg)
        {
            string[] ar = msg.Split(',');
            //Guid id = Guid.Parse(ar[0]);
            //if (!form.players.Contains(id))
            //{
            //    lock (syncLock)
            //    {
            //        if (!form.players.Contains(id))
            //            form.players.Add(id);
            //    }
            //}
            int type = int.Parse(ar[0]);
            switch(type)
            {
                case 0: // movement
                    Guid id = Guid.Parse(ar[1]);
                    int playerNum = int.Parse(ar[2]);
                    int x = int.Parse(ar[3]);
                    int y = int.Parse(ar[4]);
                    int dir = int.Parse(ar[5]);
                    form.MovePlayer(id, playerNum, x, y, dir);
                    break;
                case 1: // bullet made
                    Guid id_b = Guid.Parse(ar[1]);
                    int playerNum_b = int.Parse(ar[2]);
                    int x_b = int.Parse(ar[3]);
                    int y_b = int.Parse(ar[4]);
                    int dir_b = int.Parse(ar[5]);
                    form.MoveBullet(id_b, 0, x_b, y_b, playerNum_b);
                    break;
                case 2: // bullet hit
                    break;
                case 3: // bomb made
                    break;
                case 4: // bomb hit
                    break;
            }
        }

        // joins or creates new game
        public void EnterGame()
        {
            Debug.WriteLine("inside EnterGame()");
            bool joining = false;
            long until = DateTime.Now.Ticks + TimeSpan.TicksPerSecond * 1;
            while (DateTime.Now.Ticks < until)
            {
                try
                {
                    byte[] data = new byte[1024];
                    int recv = sock.ReceiveFrom(data, ref ep);
                    string stringData = Encoding.ASCII.GetString(data, 0, recv);
                    StringReader reader = new StringReader(stringData);
                    // continue unless first line is HEADER and also the second line is 0
                    if (!reader.ReadLine().Equals(MulticastSender.HEADER) || !reader.ReadLine().Equals("0"))
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
                form.CreateNewGame();
        }
        // returns true for connected, false for failure
        private bool TryJoin(Guid gameToJoin, int playerNum)
        {
            // Send Join Request
            MulticastSender.SendJoinReq(gameToJoin, MulticastSender.ID, playerNum);
            long until = DateTime.Now.Ticks + TimeSpan.TicksPerMillisecond * 500;
            while (DateTime.Now.Ticks < until)
            {
                byte[] data = new byte[1024];
                int recv = sock.ReceiveFrom(data, ref ep);
                string stringData = Encoding.ASCII.GetString(data, 0, recv);
                StringReader reader = new StringReader(stringData);
                if (!reader.ReadLine().Equals(MulticastSender.HEADER))
                    continue;
                string secondLine = reader.ReadLine();
                if (int.TryParse(secondLine, out int type) && (type == 2 || type == 3))
                {
                    string s = reader.ReadLine();
                    string[] ar = s.Split(',');
                    if (Guid.Parse(ar[0]) == gameToJoin && Guid.Parse(ar[1]) == MulticastSender.ID)
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
