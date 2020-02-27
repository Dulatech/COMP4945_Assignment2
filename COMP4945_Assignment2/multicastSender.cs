using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkComm
{
    public class MulticastSender
    {
        static UdpClient sock;
        public static readonly IPEndPoint iep = new IPEndPoint(IPAddress.Parse("239.50.50.51"), MulticastReceiver.PORT);
        static MulticastSender()
        {
            sock = new UdpClient();
        }
        public static void SendMsg(string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg);
            sock.Send(data, data.Length, iep);
        }
    }
}
