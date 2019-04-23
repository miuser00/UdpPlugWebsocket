using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Miuser.NUDP.Sockets;


namespace UdpPlugWebsocket
{
    public partial class Device : Form
    {
        string s_output;

        //本UDP服务对象
        public SocketServer server;
        //人工选中的连接对象
        public SocketConnection curr;


        public Device()
        {
            InitializeComponent();
            server = new SocketServer(SetupForm.cfg.UDPPort);
            //处理从客户端收到的消息
            server.HandleRecMsg = new Action<byte[], SocketConnection, SocketServer>((bytes, client, theServer) =>
            {
                string msg = Encoding.UTF8.GetString(bytes);
                SetOutput(client.Tag+$" 收到消息:{msg}");
            });

            //处理服务器启动后事件
            server.HandleServerStarted = new Action<SocketServer>(theServer =>
            {
                LogMessage("UDP Server 服务已启动************");
            });

            //处理新的客户端连接后的事件
            server.HandleNewClientConnected = new Action<SocketServer, SocketConnection>((theServer, theCon) =>
            {
                LogMessage(theCon.Tag+ $@" UDP客户端接入，当前连接数：{theServer.GetConnectionCount()}");
                RefreshDataGrid(theServer);
                
            });

            //处理客户端连接关闭后的事件
            server.HandleClientClose = new Action<SocketConnection, SocketServer>((theCon, theServer) =>
            {
                LogMessage(theCon.Tag + $@" UDP客户端关闭，当前连接数为：{theServer.GetConnectionCount()}");
                RefreshDataGrid(theServer);

            });

            //处理异常
            server.HandleException = new Action<Exception>(ex =>
            {
                LogError(ex.Message+" "+ex.StackTrace);
            });
            server.StartServer();

        }
        private void Device_Load(object sender, EventArgs e)
        {

        }

        private void RefreshDataGrid(SocketServer theServer)
        {
            DataTable dt = GetConnectionTable(theServer);
            lock (dt)
            {
                this.Invoke(new Action(() =>
                {
                    dgv_devices.DataSource = dt;
                    dgv_devices.Columns[0].Width = 200;
                }));
            }
        }
        private DataTable GetConnectionTable(SocketServer server)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Tag");
            dt.Columns.Add("IP");
            dt.Columns.Add("UDP");
            dt.Columns.Add("Lifetime");

            IEnumerable<SocketConnection> connections = server.GetConnectionList();
            lock (connections)
            {
                foreach (SocketConnection conn in server.GetConnectionList().ToArray())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = conn.Tag;
                    dr[1] = conn._endpoint.Address;
                    dr[2] = conn._endpoint.Port;
                    dr[3] = conn._lifetime;
                    dt.Rows.Add(dr);
                }
            }
            
            return dt;
        }

        private static Device _instance;
        public static Device Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Device();
                return _instance;
            }
            private set { _instance = value; }
        }


        private void btn_Send_Click(object sender, EventArgs e)
        {
            curr?.Send(txt_Send.Text);
        }

        private void dgv_devices_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_devices.SelectedRows.Count > 0)
            {
                string tag = dgv_devices.SelectedRows[0].Cells["Tag"].Value.ToString();
                curr = server.GetTheConnection(x =>
                {
                    var Id = (string)x.Tag;
                    return Id == tag;
                });
            }else
            {
                curr = null;
            }
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
        public void SetOutput(string text)
        {
            //决定是否屏显
            if (SetupForm.cfg.EnableScreenLog==false) return;

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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
