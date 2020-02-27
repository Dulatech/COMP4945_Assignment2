using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.IO;
using COMP4945_Assignment2;

namespace NetworkComm
{
    class NetworkController
    {
        public delegate void MessageReceivedHandler(string msg);
        public event MessageReceivedHandler MessageReceived;
        GameArea form;
        public Receiver rcvr;
        public bool IsHost { get; set; }
        public NetworkController(GameArea f)
        {
            form = f;
            IsHost = false;
            MessageReceived += new MessageReceivedHandler(MsgReceivedHandler);
            rcvr = new MulticastReceiver(this);
        }
        public void OnMessageReceived(string msg)
        {
            MessageReceived?.Invoke(msg);
        }
        public void MsgReceivedHandler(string msg)
        {
            StringReader reader = new StringReader(msg);
            if (!reader.ReadLine().Equals(SenderAPI.HEADER))
                return;
            string secondLine = reader.ReadLine();
            if (secondLine.Length == 1)
            {
                form.PrintGameStateToDebug();
                Debug.WriteLine("{0}\n", msg);
                if (IsHost && int.TryParse(secondLine, out int type) && type == 1)
                    HandleJoinReq(reader.ReadLine());
                else
                    return;
            }
            else
            {
                if (Guid.Parse(secondLine) == GameArea.gameID)
                    HandleGameMsg(reader.ReadLine());
            }
        }
        private void HandleJoinReq(string msg)
        {
            string[] ar = msg.Split(',');
            if (Guid.Parse(ar[0]) == GameArea.gameID) // don't respond to other game id requests
            {
                int n = int.Parse(ar[2]);
                SenderAPI.SendJoinResp(ar[1], (n == GameArea.nextPlayer && n <= GameArea.MAX_PLAYERS));
            }
        }
        private void HandleGameMsg(string msg)
        {
            string[] ar = msg.Split(',');
            int type = int.Parse(ar[0]);
            Guid playerID, bulletID, bombID;
            int playerNum, x, y, dir, scoreType, score;
            switch (type)
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

        public void EnterGame()
        {
            rcvr.EnterGame();
        }

        public void CreateNewGame()
        {
            form.CreateNewGame();
        }
    }
}
