using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace udp
{
    public interface IUdp : IDisposable
    {
        event UdpEventHandler Received;

        void Send(byte[] bt, EndPoint ep);

        Socket UdpSocket { get; }
    }
}
