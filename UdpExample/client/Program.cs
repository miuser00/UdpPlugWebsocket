using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using udp;
namespace client
{
    class Program
    { 
        private const string _serverIp = "127.0.0.1";
        private const int _serverPort = 7101;

        static void Main(string[] args)
        {
            Client client = new Client();
            client.ep = new IPEndPoint(IPAddress.Parse(_serverIp), _serverPort);
            client.Listening();
            client.Received += new UdpEventHandler(client_Received);
            while (true)
            {
                string tmp = Console.ReadLine();

                    byte[] bt = Encoding.Default.GetBytes(tmp);
                    System.Threading.Thread t = new System.Threading.Thread(() =>
                    {
                        client.Send(bt, client.ep);
                    });
                    t.Start();
               
            }
        }

        static void client_Received(object sender, UdpEventArgs e)
        {
            IPEndPoint ep = e.Remote as IPEndPoint;
            string tmpReceived = Encoding.Default.GetString(e.Received);
            Console.WriteLine(ep.Address.ToString() + ":" + ep.Port + "--> " + tmpReceived);
        }
    }

    public class Client : Udp
    {
        public EndPoint ep;
        public Client()
        {

        }
    }
}
