using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace COMP4945_Assignment2
{
    class MulticastSender
    {
        UdpClient sock;
        IPEndPoint iep = new IPEndPoint(IPAddress.Parse("239.50.50.51"), 49452);
        public MulticastSender()
        {
            sock = new UdpClient();
        }
        public void SendMsg(string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg);
            sock.Send(data, data.Length, iep);
        }
    }
}
