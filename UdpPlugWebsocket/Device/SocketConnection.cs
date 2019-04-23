using System;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Net;
using UdpPlugWebsocket;

namespace Miuser.NUDP.Sockets
{
    /// <summary>
    /// Socket连接,双向通信
    /// </summary>
    public class SocketConnection 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socket">维护的Socket对象</param>
        /// <param name="server">维护此连接的服务对象</param>
        public SocketConnection(IPEndPoint endpoint,SocketServer server)
        {
            _endpoint = endpoint;
            _server = server;
            InitTimer();
            IsActive = true;
        }

        #region 私有成员
        //UDP连接生存时间
        private static int LIFELIMIT =SetupForm.cfg.UDPTTL;
        //定义Timer类
        private System.Timers.Timer timer;
        #endregion
        #region 内部函数
        /// <summary>
        /// 初始化Timer控件
        /// </summary>
        private void InitTimer()
        {
            //设置定时间隔(毫秒为单位)
            int interval = 1000;
            timer = new System.Timers.Timer(interval);
            //设置执行一次（false）还是一直执行(true)
            timer.AutoReset = true;
            //设置是否执行System.Timers.Timer.Elapsed事件
            timer.Enabled = true;
            //绑定Elapsed事件
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerUp);
        }

        /// <summary>
        /// Timer类执行定时到点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            _lifetime -= (int)((System.Timers.Timer)(sender)).Interval;
            if (_lifetime <= 0)
            {
                IsActive = false;
                HandleClientClose?.Invoke(this, _server);
                timer.Dispose();
            }
        }
        #endregion

        #region 公有成员
        //连接字符
        public readonly IPEndPoint _endpoint;
        //服务端
        public SocketServer _server = null;
        //剩余生存时间
        public int _lifetime = LIFELIMIT;
        #endregion

        #region 外部接口

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bytes">数据字节</param>
        public void Send(byte[] bytes)
        {
            if (_server == null) return;
            try
            {
                _server._socket.SendTo(bytes, _endpoint);
                HandleSendMsg?.Invoke(bytes,this, _server);
            }
            catch (SocketException ex)
            {
                HandleException?.BeginInvoke(ex, null, null);
            }
        }

        /// <summary>
        /// 发送字符串（默认使用UTF-8编码）
        /// </summary>
        /// <param name="msgStr">字符串</param>
        public void Send(string msgStr)
        {
            Send(Encoding.UTF8.GetBytes(msgStr));
        }

        /// <summary>
        /// 发送字符串（使用自定义编码）
        /// </summary>
        /// <param name="msgStr">字符串消息</param>
        /// <param name="encoding">使用的编码</param>
        public void Send(string msgStr,Encoding encoding)
        {
            Send(encoding.GetBytes(msgStr));
        }

        /// <summary>
        /// 传入自定义属性
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 连接是否仍然活着
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 重新激活连接，连接生命加满
        /// </summary>
        public void Reactive()
        {
            _lifetime = LIFELIMIT;
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 客户端连接发送消息后回调
        /// </summary>
        public Action<byte[], SocketConnection, SocketServer> HandleSendMsg { get; set; }
        /// <summary>
        /// 异常处理程序
        /// </summary>
        public Action<Exception> HandleException { get; set; }

        /// <summary>
        /// 客户端连接关闭后回调
        /// </summary>
        public Action<SocketConnection, SocketServer> HandleClientClose { get; set; }

        #endregion
    }
}
