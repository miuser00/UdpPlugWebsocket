using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpPlugWebsocket
{
    public class WSConnection
    {
        //Websocket连接生存时间
        private static int LIFELIMIT = SetupForm.cfg.NODETTL;
        public string EndpointString;
        public int ttl;
        //定义Timer类
        private System.Timers.Timer timer;
        public WSConnection()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            ttl = LIFELIMIT;
            InitTimer();
            IsActive = true;
        }
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
            ttl -= (int)((System.Timers.Timer)(sender)).Interval;
            if (ttl <= 0)
            {
                IsActive = false;
                HandleWebsocketClosed?.Invoke(this);
                timer.Dispose();
            }
        }
        public bool IsActive { get; set; }

        public void Reactive()
        {
            ttl = LIFELIMIT;
        }
        #endregion
        #region 事件处理
        /// <summary>
        /// NODE关闭后回调
        /// </summary>
        public Action<WSConnection> HandleWebsocketClosed { get; set; }
        #endregion
    }
    public class UDPConnection
    {
        //UDP连接生存时间
        private static int LIFELIMIT = SetupForm.cfg.NODETTL;
        public string EndpointString;
        public int ttl;
        //定义Timer类
        private System.Timers.Timer timer;
        public UDPConnection()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            ttl = LIFELIMIT;
            InitTimer();
            IsActive = true;
        }
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
            ttl -= (int)((System.Timers.Timer)(sender)).Interval;
            if (ttl <= 0)
            {
                IsActive = false;
                HandleUDPClosed?.Invoke(this);
                timer.Dispose();
            }
        }
        public bool IsActive { get; set; }

        public void Reactive()
        {
            ttl = LIFELIMIT;
        }
        #endregion
        #region 事件处理

        /// <summary>
        /// NODE关闭后回调
        /// </summary>
        public Action<UDPConnection> HandleUDPClosed { get; set; }
        #endregion
    }

    //交换模块的群组
    public class NODE
    {
        //NODE连接生存时间
        private static int LIFELIMIT = SetupForm.cfg.NODETTL;
        //剩余生存时间
        public int ttl;
        //节点的UID
        public string ID;

        public List<WSConnection> WebsocketConnections;
        public List<UDPConnection> UDPConnections;

        //定义Timer类
        private System.Timers.Timer timer;


        public NODE(string ID)
        {
            WebsocketConnections = new List<WSConnection>();
            UDPConnections = new List<UDPConnection>();
            System.Timers.Timer timer = new System.Timers.Timer();
            ttl = LIFELIMIT;
            InitTimer();
            IsActive = true;
            HandleNodeCreated?.Invoke(this);

        }
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
            ttl -= (int)((System.Timers.Timer)(sender)).Interval;
            if (ttl <= 0)
            {
                IsActive = false;
                HandleNodeClosed?.Invoke(this);
                timer.Dispose();
            }
        }
        #endregion

        # region 外部函数
        public void AddSource(string endpoint, Switch.Source source)
        {
            if (source == Switch.Source.UDPPort) AddUDPConnection(endpoint);
            if (source == Switch.Source.WebSocket) AddWebsocketConnection(endpoint);
        }

        /// <summary>
        /// 添加一个新的UDP连接
        /// </summary>
        /// <param name="endpoint"></param>
        public void AddUDPConnection(string endpoint)
        {
            UDPConnection udp = new UDPConnection();
            udp.EndpointString = endpoint;
            //节点超时则自动从连接中移除
            udp.HandleUDPClosed = new Action<UDPConnection>((conn => {
                UDPConnections.Remove(conn); HandleUDPRemoved?.Invoke(conn.EndpointString);
            }));
            UDPConnections.Add(udp);
            HandleUDPAdded?.Invoke(endpoint);
        }
        /// <summary>
        /// 添加一个新的Websocket连接
        /// </summary>
        /// <param name="endpoint"></param>
        public void AddWebsocketConnection(string endpoint)
        {
            WSConnection web = new WSConnection();
            web.EndpointString = endpoint;
            //节点超时则自动从连接中移除
            web.HandleWebsocketClosed = new Action<WSConnection>((conn => {
                WebsocketConnections.Remove(conn); HandleWebSocketRemoved?.Invoke(conn.EndpointString);
            }));
            WebsocketConnections.Add(web);
            HandleWebSocketAdded?.Invoke(endpoint);
        }
        public bool IsActive { get; set; }

        public void Reactive()
        {
            ttl = LIFELIMIT;
        }
        #endregion

        #region 事件处理
        /// <summary>
        /// NODE关闭后回调
        /// </summary>
        public Action<NODE> HandleNodeClosed { get; set; }
        /// <summary>
        /// NODE建立后回调
        /// </summary>
        public Action<NODE> HandleNodeCreated { get; set; }
        /// <summary>
        /// 新的Websocket连接建立后回调
        /// </summary>
        public Action<string> HandleWebSocketAdded { get; set; }
        /// <summary>
        /// 新的Websocket连接断开后回调
        /// </summary>
        public Action<string> HandleWebSocketRemoved { get; set; }
        /// <summary>
        /// 新的UDP连接建立后回调
        /// </summary>
        public Action<string> HandleUDPAdded { get; set; }
        /// <summary>
        /// 新的UDP连接断开后回调
        /// </summary>
        public Action<string> HandleUDPRemoved { get; set; }
        /// <summary>
        /// 异常处理程序
        /// </summary>
        public Action<Exception> HandleException { get; set; }
        #endregion
    }
}
