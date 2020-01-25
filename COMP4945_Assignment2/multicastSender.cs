using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace COMP4945_Assignment2
{
    class MulticastSender
    {
        // just in case other applications use this port for multicast
        public static readonly string HEADER = "SOMETHING UNIQUE";
        UdpClient sock;
        IPEndPoint iep = new IPEndPoint(IPAddress.Parse("239.50.50.51"), Program.PORT);
        public MulticastSender()
        {
            sock = new UdpClient();
        }
        public void SendMsg(string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(HEADER + "\n" + msg);
            sock.Send(data, data.Length, iep);
        }
    }
}
