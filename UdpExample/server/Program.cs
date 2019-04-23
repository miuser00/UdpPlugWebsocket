using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.Net;
using udp;

namespace server
{
    class Program
    { 
        static Server server = new Server();
        static void Main(string[] args)
        {
            server.Port = 8000;
            server.Listening();
            if (server.IsListening)
            {
                server.Received += new UdpEventHandler(server_Received);
            }
            Console.ReadKey();
        }

        static void server_Received(object sender, UdpEventArgs e)
        {
            IPEndPoint ep = e.Remote as IPEndPoint;
            string tmpReceived = Encoding.Default.GetString(e.Received);
            Console.WriteLine(ep.Address.ToString() + ":" + ep.Port + "--> " + tmpReceived);
            ///自动回复
            server.Send(Encoding.Default.GetBytes("服务器已收到数据:'" + tmpReceived + "',来自:‘" + ep.Address.ToString() + ":" + ep.Port + "’"), ep);
        }

    }
    public class Server : Udp
    {
        private EndPoint ep;
        public Server()
        {
        }
    }
}
