using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.IO;
namespace UdpPlugWebsocket
{
    public partial class Browser : Form
    {
        string s_output;
        //本Websocket服务对象
        public WebSocketServer server;
        //人工选中的连接对象
        public static IWebSocketSession curr;
        private static Browser _instance;

        public Browser()
        {
            InitializeComponent();

            server = new WebSocketServer("ws://0.0.0.0:"+SetupForm.cfg.BrowserPort);
            server.AddWebSocketService<Laputa>("/");
            server.Start();
        }
        private void Browser_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 发送字节
        /// </summary>
        public void Send(string EndpointString,byte[] bytes)
        {
            try
            {
                IWebSocketSession session=server.WebSocketServices["/"].Sessions.Sessions.Where(x => { return x.Context.UserEndPoint.ToString() == EndpointString; }).FirstOrDefault();
                server.WebSocketServices["/"].Sessions.SendToAsync(System.Text.Encoding.Default.GetString(bytes), session.ID, null);
            }
            catch(Exception ex)
            {
                HandleError?.Invoke(ex.StackTrace);
            }
        }
        public void RefreshDataGrid(WebSocketSessionManager wsm)
        {
            DataTable dt = GetConnectionTable(wsm);
            {
                this.Invoke(new Action(() =>
                {
                    lock (dt.Rows.SyncRoot)
                    {
                        dgv_browsers.DataSource = dt.Copy();
                    }
                    dgv_browsers.Columns[0].Width = 400;
                    dgv_browsers.Columns[1].Width = 200;
                }));
            }
        }
        private DataTable GetConnectionTable(WebSocketSessionManager wsm)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("EndPoint");
            dt.Columns.Add("State");

            lock (wsm.Sessions)
            {
                foreach (IWebSocketSession session in wsm.Sessions.ToArray())
                {
                    
                    DataRow dr = dt.NewRow();
                    dr[0] = session.ID;
                    dr[1] = session.Context.UserEndPoint;
                    dr[2] = session.State;
                    dt.Rows.Add(dr);
                }
            }
            return dt;

        }
        public static Browser Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Browser();
                return _instance;
            }
            private set { _instance = value; }
        }
        public void LogMessage(string message)
        {
            HandleMessage?.Invoke(message);
        }
        public void LogError(string message)
        {
            HandleError?.Invoke(message);
        }
        #region 消息和错误事件
        /// <summary>
        /// 消息处理程序
        /// </summary>
        public Action<string> HandleMessage { get; set; }
        /// <summary>
        /// 异常处理程序
        /// </summary>
        public Action<string> HandleError { get; set; }


        #endregion
        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                server.WebSocketServices["/"].Sessions.SendToAsync(txt_Send.Text, curr.ID, null);
            }catch (Exception ex)
            {
                HandleError?.Invoke(ex.StackTrace);
            }
        }
        private void dgv_browsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_browsers.SelectedRows.Count > 0)
            {
                string tag = dgv_browsers.SelectedRows[0].Cells["EndPoint"].Value.ToString();
                curr = server.WebSocketServices["/"].Sessions.Sessions.Where(x =>
                {
                    var Id = (string)x.Context.UserEndPoint.ToString();
                    return Id == tag;
                }).FirstOrDefault();
            }
            else
            {
                curr = null;
            }
        }
        public void SetOutput(string text)
        {
            //决定是否屏显
            if (SetupForm.cfg.EnableScreenLog == false) return;

            text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " |" + text;
            this.Invoke(new Action(() =>
            {
                s_output = s_output + text.Replace("\0", "") + "\r";

                if ((s_output.Length) > 5000)
                {
                    s_output = s_output.Substring(s_output.Length - 5000, 5000);
                }
                //滚到最后
                this.richTextBox1.Text = s_output;
                this.richTextBox1.Select(richTextBox1.TextLength, 0);
                //this.richTextBox1.Focus();
                this.richTextBox1.ScrollToCaret();

            }));
        }
        #region 客户端连接事件
        /// <summary>
        /// 当新客户端连接后执行
        /// </summary>
        public Action<WebSocketSharp.Net.WebSockets.WebSocketContext> HandleNewWebSocketClientConnected { get; set; }

        /// <summary>
        /// 客户端连接接受新的消息后调用
        /// </summary>
        public Action<byte[], WebSocketSharp.Net.WebSockets.WebSocketContext> HandleWebSocketRecMsg { get; set; }

        /// <summary>
        /// 客户端连接发送消息后回调
        /// </summary>
        public Action<byte[], WebSocketSharp.Net.WebSockets.WebSocketContext> HandleWebSocketSendMsg { get; set; }

        /// <summary>
        /// 客户端连接关闭后回调
        /// </summary>
        public Action<WebSocketSharp.Net.WebSockets.WebSocketContext> HandleWebSocketClientClose { get; set; }

        #endregion
    }
    public class Laputa : WebSocketBehavior
    {
        
        protected override void OnOpen()
        {
            
            Browser.Instance. LogMessage( Context.UserEndPoint.ToString()  + " was accepted" + " Websocket ID is " + ID);
            Browser.Instance.RefreshDataGrid(Sessions);
            Browser.Instance.HandleNewWebSocketClientConnected?.Invoke(Context);
            base.OnOpen();
        }
        protected override void OnClose(CloseEventArgs e)
        {
            Browser.Instance.LogMessage("Websocket "+this.ID + " was closed");
            Browser.Instance.RefreshDataGrid(Sessions);
            Browser.Instance.HandleWebSocketClientClose?.Invoke(Context);
            base.OnClose(e);

        }
        protected override void OnMessage(MessageEventArgs e)
        {
            string s_data = e.Data;
            Logger log = new Logger();
            //Browser.Instance.LogMessage(Context.UserEndPoint.ToString() + " is Saying: " + e.Data);
            Browser.Instance.HandleWebSocketRecMsg?.Invoke(System.Text.Encoding.Default.GetBytes(e.Data), Context);
            //Console.Write(e.Data);
            Browser.Instance.SetOutput(Context.UserEndPoint.ToString()+" received " +e.Data);
        }

    }


}
