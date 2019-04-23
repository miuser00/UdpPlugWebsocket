using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data;
namespace Miuser.NUDP.Sockets
{
    /// <summary>
    /// Socket服务端
    /// </summary>
    public class SocketServer
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">监听的IP地址</param>
        /// <param name="port">监听的端口</param>
        public SocketServer(string ip, int port)
        {
            _ip = ip;
            _port = port;

        }
        /// <summary>
        /// 构造函数,监听IP地址默认为本机0.0.0.0
        /// </summary>
        /// <param name="port">监听的端口</param>
        public SocketServer(int port)
        {
            _ip = "0.0.0.0";
            _port = port;
            //处理客户端连接关闭后的事件
            Instance = this;

        }
        #region 内部成员

        private string _ip { get; set; } = "";
        private int _port { get; set; } = 0;
        private bool _isListen { get; set; } = true;

        private LinkedList<SocketConnection> _clientList { get; } = new LinkedList<SocketConnection>();

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
        private bool IsListening { get; set; }
        #endregion
        #region 外部成员
        /// <summary>
        /// 指向本类的静态对象，对象为最近生成的类实例
        /// </summary>
        /// <param name="port">监听的端口</param>     
        public static SocketServer Instance;
        public Socket _socket { get; set; } = null;
        #endregion

        #region 内部函数
        /// <summary>
        /// 删除指定的客户端连接
        /// </summary>
        /// <param name="theConnection">指定的客户端连接</param>
        private void RemoveConnection(SocketConnection theConnection)
        {
            RWLock_ClientList.EnterWriteLock();
            try
            {
                _clientList.Remove(theConnection);
                HandleClientClose?.Invoke(theConnection, this);
            }
            finally
            {
                RWLock_ClientList.ExitWriteLock();
            }
        }
        /// <summary>
        /// 删除不活动的客户端连接
        /// </summary>
        /// <param name="theConnection">指定的客户端连接</param>
        private void RemoveInactiveConnections()
        {
            List<SocketConnection> conns = new List<SocketConnection>();
            foreach (SocketConnection connect in _clientList.ToArray())
            {
                if (connect.IsActive == false)
                {
                    conns.Add(connect);
                }
            }
            foreach (SocketConnection conn in conns.ToArray())
            {
                RemoveInactiveConnection(conn);
            }
        }
        /// <summary>
        /// 检测该客户端，如果不活动则删除连接
        /// </summary>
        /// <param name="theConnection">指定的客户端连接</param>
        private void RemoveInactiveConnection(SocketConnection connect)
        {
            if (connect.IsActive == false)
            {
                RemoveConnection(connect);
            }
        }
        /// <summary>
        /// 当某连接不活动超时激活该函数
        /// </summary>
        private void HandleConnClientClose(SocketConnection conn, SocketServer server)
        {
            RemoveConnection(conn);
        }
        /// <summary>
        /// 维护客户端列表的读写锁
        /// </summary>
        private ReaderWriterLockSlim RWLock_ClientList { get; } = new ReaderWriterLockSlim();
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

                        //查询是否UDP连接已经存在
                        SocketConnection connection = GetTheConnection(x =>
                        {
                            var Id = (string)x.Tag;
                            return Id == state.Remote.ToString();
                        });
                        //如果不存在则新建一个UDP连接对象
                        if (connection == null)
                        {
                            connection = new SocketConnection(state.Remote as IPEndPoint, this)
                            {
                                HandleSendMsg = HandleSendMsg == null ? null : new Action<byte[], SocketConnection, SocketServer>(HandleSendMsg),
                                HandleClientClose = new Action<SocketConnection, SocketServer>(HandleConnClientClose),
                                HandleException = HandleException == null ? null : new Action<Exception>(HandleException)
                            };

                            //connection.HandleClientClose += HandleClientClose == null ? null : new Action<SocketConnection, SocketServer>(HandleClientClose);

                            connection.Tag = state.Remote.ToString();
                            AddConnection(connection);

                        }
                        connection.Reactive();
                        HandleRecMsg?.Invoke(btReceived, connection, this);
                    }
                }
                catch (Exception ex)
                {
                    //System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString() + "\t" + ex.Message + ex.Source);
                    HandleException?.Invoke(ex);
                }
                finally
                {
                    state.Socket.BeginReceiveFrom(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ref state.Remote, new AsyncCallback(EndReceiveFrom), state);
                }
            }
        }
        /// <summary>
        /// 关闭指定客户端连接
        /// </summary>
        /// <param name="theConnection">指定的客户端连接</param>
        #endregion


        #region 外部接口
        /// <summary>
        /// 开始服务，监听客户端
        /// </summary>
        public void StartServer()
        {
            try
            {
                //实例化套接字（ip4寻址协议，流式传输，TCP协议）
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //开始侦听
                Listening();
                HandleServerStarted?.BeginInvoke(this, null, null);
            }
            catch (Exception ex)
            {
                HandleException?.BeginInvoke(ex, null, null);
            }
        }
        ///<summary>
        ///按照IP地址和端口发送字符串
        ///</summary>
        public void Send(string EndpointString, byte[] bytes)
        {
            try
            {
                SocketConnection conn = GetTheConnection(x =>
                  {
                      var Id = (string)x.Tag;
                      return Id == EndpointString;
                  });
                conn.Send(bytes);
            }
            catch (Exception ex)
            {
                HandleException?.BeginInvoke(ex, null, null);
            }
        }
        ///<summary>
        ///开始异步监听端口
        ///</summary>
        public void Listening()
        {
            IPAddress ip = IPAddress.Any;
            try
            {
                if (this._ip != null)
                    if (!IPAddress.TryParse(this._ip, out ip))
                        throw new ArgumentException("IP地址错误", "Ip");
                _socket.Bind(new IPEndPoint(ip, this._port));

                UdpState state = new UdpState();
                state.Socket = _socket;
                state.Remote = new IPEndPoint(IPAddress.Any, 0);
                _socket.BeginReceiveFrom(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ref state.Remote, new AsyncCallback(EndReceiveFrom), state);

                IsListening = true;
            }
            catch (Exception ex)
            {
                IsListening = false;
                HandleException?.BeginInvoke(ex, null, null);
            }

        }
        /// <summary>
        /// 异步处理收到的数据包
        /// </summary>

        public void CloseConnection(SocketConnection theConnection)
        {
            RemoveConnection(theConnection);
            //调用外部回调函数通知连接被关闭

        }
        /// <summary>
        /// 添加客户端连接
        /// </summary>
        /// <param name="theConnection">需要添加的客户端连接</param>
        public void AddConnection(SocketConnection theConnection)
        {
            RWLock_ClientList.EnterWriteLock();
            try
            {
                _clientList.AddLast(theConnection);

            }
            finally
            {
                RWLock_ClientList.ExitWriteLock();
                //调用外部回调函数通知新连接建立
                HandleNewClientConnected(this, theConnection);
            }
        }
        /// <summary>
        /// 通过条件获取客户端连接列表
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns></returns>
        public IEnumerable<SocketConnection> GetConnectionList(Func<SocketConnection, bool> predicate)
        {

            RemoveInactiveConnections();
            IEnumerable<SocketConnection> ret;
            lock (_clientList)
            {
                ret = _clientList.Where(predicate);
            }
            return ret;
        }
        /// <summary>
        /// 获取所有客户端连接列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SocketConnection> GetConnectionList()
        {
            RemoveInactiveConnections();
            return _clientList;
        }
        /// <summary>
        /// 寻找特定条件的客户端连接
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns></returns>
        public SocketConnection GetTheConnection(Func<SocketConnection, bool> predicate)
        {
            SocketConnection conn;
            lock (_clientList)
            {
                conn = _clientList.Where(predicate).FirstOrDefault();
            }
            if (conn == null) return null;

            RemoveInactiveConnection(conn);

            if (conn.IsActive) return conn;
            return null;
        }
        /// <summary>
        /// 获取客户端连接数
        /// </summary>
        /// <returns></returns>
        public int GetConnectionCount()
        {
            int ret;
            lock (_clientList)
            {
                ret = _clientList.Count;
            }
            return ret;

        }
        #endregion

        #region 服务端事件
        /// <summary>
        /// 服务启动后执行
        /// </summary>
        public Action<SocketServer> HandleServerStarted { get; set; }
        #endregion
        #region 客户端连接事件
        /// <summary>
        /// 当新客户端连接后执行
        /// </summary>
        public Action<SocketServer, SocketConnection> HandleNewClientConnected { get; set; }
        /// <summary>
        /// 客户端连接接受新的消息后调用
        /// </summary>
        public Action<byte[], SocketConnection, SocketServer> HandleRecMsg { get; set; }
        /// <summary>
        /// 客户端连接发送消息后回调
        /// </summary>
        public Action<byte[], SocketConnection, SocketServer> HandleSendMsg { get; set; }
        /// <summary>
        /// 客户端连接关闭后回调
        /// </summary>
        public Action<SocketConnection, SocketServer> HandleClientClose { get; set; }
        #endregion
        #region 公共事件
        /// <summary>
        /// 异常处理程序
        /// </summary>
        public Action<Exception> HandleException { get; set; }
        #endregion
    }
}
