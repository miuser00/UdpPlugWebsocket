using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace udp
{
    internal class UdpState
    {
        public byte[] Buffer;
        public EndPoint Remote;
        public Socket Socket;

        public UdpState()
        {
            Buffer = new byte[65536];
        }
    }
}
