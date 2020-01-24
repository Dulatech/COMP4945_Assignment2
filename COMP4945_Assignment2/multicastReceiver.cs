using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace COMP4945_Assignment2
{
    class MulticastReceiver
    {
        GameArea form;
        public MulticastReceiver(GameArea f)
        {
            form = f;
        }
        public void run()
        {
            IPEndPoint multiep = new IPEndPoint(IPAddress.Parse("239.50.50.51"), 49452);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                sock.Bind(new IPEndPoint(IPAddress.Any, 49452));
                sock.MulticastLoopback = false;
                Debug.WriteLine("Listening on port 49455");
                sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse("239.50.50.51")));
                EndPoint ep = (EndPoint)multiep;
                byte[] data = new byte[1024];
                string stringData;
                int recv;
                while (true)
                {
                    recv = sock.ReceiveFrom(data, ref ep);
                    stringData = Encoding.ASCII.GetString(data, 0, recv);
                    handleMsg(stringData);
                    Debug.WriteLine("received: {0}  from: {1}", stringData, ep.ToString());
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
