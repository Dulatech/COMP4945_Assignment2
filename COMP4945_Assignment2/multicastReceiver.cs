﻿using System;
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
        EndPoint ep = (EndPoint)(MulticastSender.iep);
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
        public void SetMulticastLoopback(bool b)
        {
            sock.MulticastLoopback = b;
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
                    string secondLine = reader.ReadLine();
                    if (secondLine.Length == 1)
                    {
                        form.PrintGameStateToDebug();
                        Debug.WriteLine("{0}\n{1}\n", ep.ToString(), stringData);
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
                MulticastSender.SendJoinResp(ar[1], (n == GameArea.nextPlayer && n <= GameArea.MAX_PLAYERS));
            }
        }
        private void HandleGameMsg(string msg)
        {
            string[] ar = msg.Split(',');
            int type = int.Parse(ar[0]);
            Guid playerID, bulletID, bombID;
            int playerNum, x, y, dir,scoreType, score;
            switch(type)
            {
                case 0: // movement
                    playerID = Guid.Parse(ar[1]);
                    playerNum = int.Parse(ar[2]);
                    x = int.Parse(ar[3]);
                    y = int.Parse(ar[4]);
                    dir = int.Parse(ar[5]);
                    form.MovePlayer(playerID, playerNum, x, y, dir);
                    break;
                case 1: // bullet made
                    x = int.Parse(ar[3]);
                    y = int.Parse(ar[4]);
                    bulletID = Guid.Parse(ar[6]);
                    form.CreateBullet(bulletID, x, y);
                    break;
                case 2: // bullet hit
                    playerID = Guid.Parse(ar[1]);
                    playerNum = int.Parse(ar[2]);
                    bulletID = Guid.Parse(ar[3]);
                    form.PlayerIsDead(playerID, playerNum);
                    form.RemoveProjectile(bulletID, true);
                    break;
                case 3: // bomb made
                    x = int.Parse(ar[3]);
                    y = int.Parse(ar[4]);
                    bombID = Guid.Parse(ar[6]);
                    form.CreateBomb(bombID, x, y);
                    break;
                case 4: // bomb hit
                    playerID = Guid.Parse(ar[1]);
                    playerNum = int.Parse(ar[2]);
                    bombID = Guid.Parse(ar[3]);
                    form.PlayerIsDead(playerID, playerNum);
                    form.RemoveProjectile(bombID, false);
                    break;
                case 5: // score update
                    scoreType = int.Parse(ar[3]);
                    score = int.Parse(ar[4]);
                    form.ChangeScore(scoreType, score);
                    break;
                case -1: // disconnect
                    playerID = Guid.Parse(ar[1]);
                    playerNum = int.Parse(ar[2]);
                    form.RemovePlayer(playerID, playerNum);
                    break;
            }
        }

        // joins or creates new game
        public void EnterGame()
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
            long until = DateTime.Now.Ticks + TimeSpan.TicksPerMillisecond * 500;
            byte[] data = new byte[1024];
            MulticastSender.SendJoinReq(gameToJoin, MulticastSender.ID, playerNum);
            while (DateTime.Now.Ticks < until)
            {
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
