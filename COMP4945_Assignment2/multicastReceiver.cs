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
        public static bool ContinueRunning = true;
        public MulticastReceiver(GameArea f)
        {
            form = f;
        }
        public void run()
        {
            IPEndPoint multiep = new IPEndPoint(IPAddress.Parse("239.50.50.51"), Program.PORT);
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                sock.Bind(new IPEndPoint(IPAddress.Any, Program.PORT));
                sock.MulticastLoopback = false;
                Debug.WriteLine("Listening on port " + Program.PORT);
                sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse("239.50.50.51")));
                EndPoint ep = (EndPoint)multiep;
                byte[] data = new byte[1024];
                string stringData;
                int recv;
                while (ContinueRunning)
                {
                    recv = sock.ReceiveFrom(data, ref ep);
                    stringData = Encoding.ASCII.GetString(data, 0, recv);
                    StringReader reader = new StringReader(stringData);
                    if (!reader.ReadLine().Equals(MulticastSender.HEADER))
                        continue;
                    string msg = reader.ReadLine();
                    handleMsg(msg);
                    Debug.WriteLine("received: {0}  from: {1}", msg, ep.ToString());
                }
            }
            catch (SocketException e)
            {
                Debug.WriteLine("SocketExceptionError: " + e.Message);
                sock.Close();
            }
        }

        private void handleMsg(string msg)
        {
            string[] pos = msg.Split(',');
            int x = int.Parse(pos[0]);
            int y = int.Parse(pos[1]);
            int dir = int.Parse(pos[2]);
            form.draw(x, y, dir);
        }
    }
}
