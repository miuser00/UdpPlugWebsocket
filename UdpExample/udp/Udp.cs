using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace udp
{
    public delegate void UdpEventHandler(object sender, UdpEventArgs e);

    public abstract class Udp : IUdp
    {
        public event UdpEventHandler Received;

        private int _port;
        private string _ip;
        public bool IsListening { get; private set; }
        private Socket _sck;

        public Socket UdpSocket
        {
            get { return _sck; }
        }
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        public int Port
        {
            get { return _port; }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 65536)
                    value = 65536;
                _port = value;
            }
        }

        public Udp()
        {
            _sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _sck.ReceiveBufferSize = UInt16.MaxValue * 8;
            //log
            System.Diagnostics.Trace.Listeners.Clear();
            System.Diagnostics.Trace.AutoFlush = true;
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener("log.txt"));
        }


        public void Listening()
        {

            IPAddress ip = IPAddress.Any;
            try
            {
                if (this._ip != null)
                    if (!IPAddress.TryParse(this._ip, out ip))
                        throw new ArgumentException("IP地址错误", "Ip");
                _sck.Bind(new IPEndPoint(ip, this._port));

                UdpState state = new UdpState();
                state.Socket = _sck;
                state.Remote = new IPEndPoint(IPAddress.Any, 0);
                _sck.BeginReceiveFrom(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ref state.Remote, new AsyncCallback(EndReceiveFrom), state);
                
                IsListening = true;
            }
            catch (ArgumentException ex)
            {
                IsListening = false;
                System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString() + "\t" + ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                IsListening = false;
                System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString() + "\t" + ex.Message);
                throw ex;
            }
        }
        private void EndReceiveFrom(IAsyncResult ir)
        {
            if (IsListening)
            {
                UdpState state = ir.AsyncState as UdpState;
                try
                {
                    if (ir.IsCompleted)
                    {
                        int length = state.Socket.EndReceiveFrom(ir, ref state.Remote);
                        byte[] btReceived = new byte[length];
                        Buffer.BlockCopy(state.Buffer, 0, btReceived, 0, length);
                        OnReceived(new UdpEventArgs(btReceived, state.Remote));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString() + "\t" + ex.Message+ex.Source);
                }
                finally
                {
                    state.Socket.BeginReceiveFrom(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ref state.Remote, new AsyncCallback(EndReceiveFrom), state);
                }
            }
        }

        private void OnReceived(UdpEventArgs e)
        {
            if (this.Received != null)
            {
                Received(this, e);
            }
        }

        public void Send(byte[] bt, EndPoint ep)
        {
            if (_sck == null) return;
            try
            {
                this._sck.SendTo(bt, ep);
            }
            catch (SocketException ex)
            {
                System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString() + "\t" + ex.Message);
                throw ex;
            }
        }

        public void Dispose()
        {
            if (_sck == null) return;

              using (_sck) ;
            //this.IsListening = false;
            //this._sck.Blocking = false;
            //this._sck.Shutdown(SocketShutdown.Both);
            //this._sck.Close();
            //this._sck = null;
        }

    }
}
