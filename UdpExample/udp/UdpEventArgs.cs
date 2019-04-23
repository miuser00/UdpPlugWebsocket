using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace udp
{
    public class UdpEventArgs : EventArgs
    {
        private EndPoint _remote;

        private byte[] _rec;

        public byte[] Received
        {
            get { return _rec; }
        }
        public EndPoint Remote
        {
            get { return _remote; }
        }

        public UdpEventArgs(byte[] data, EndPoint remote)
        {
            this._remote = remote;
            this._rec = data;
        }
    }
}
