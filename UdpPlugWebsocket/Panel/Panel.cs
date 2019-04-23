using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Miuser.NUDP.Sockets;

namespace UdpPlugWebsocket
{
    public partial class Panel : Form
    {
        //交换机的实例
        public Switch sw;
        public Panel()
        {
            InitializeComponent();
            sw = new Switch();
            //处理新的结点连接后的事件
            sw.HandleNodeCreated = new Action<Switch, NODE>((theSW, theNode) =>
            {
                //RefreshDataGrid(theSW);
            });
            //处理节点关闭后的事件
            sw.HandleNodeClosed = new Action<Switch, NODE>((theSW, theNode) =>
            {
                //RefreshDataGrid(theSW);
            });
            //处理节点状态变化后的事件
            sw.HandleNodeChanged = new Action<Switch, NODE>((theSW, theNode) =>
            {
                RefreshDataGrid(theSW);
            });
            //节点接收UDP消息后的事件
            sw.HandleUDPRsvMsg = new Action<byte[], string>((bytes, endpointString) =>
            {
                SetOutput("(UDP " + endpointString + ")<=" + System.Text.Encoding.Default.GetString(bytes));
            });
            //节点收到Websocket消息后的事件
            sw.HandleWebsocketRsvMsg = new Action<byte[], string>((bytes, endpointString) =>
            {
                SetOutput("(WebSocket " + endpointString + ")<=" + System.Text.Encoding.Default.GetString(bytes));
            });

            //节点发送UDP消息后的事件
            sw.HandleUDPSendMsg = new Action<byte[], string>((bytes, endpointString) =>
            {
                SetOutput("(UDP " + endpointString + ")=>" + System.Text.Encoding.Default.GetString(bytes));
            });
            //节点发送Websocket消息后的事件
            sw.HandleWebSocketSendMsg = new Action<byte[], string>((bytes, endpointString) =>
            {
                SetOutput("(WebSocket " + endpointString + ")=>" + System.Text.Encoding.Default.GetString(bytes));
            });


        }
        private static Panel _instance;
        public static Panel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Panel();
                return _instance;
            }
            private set { _instance = value; }
        }

        private void Panel_Load(object sender, EventArgs e)
        {



        }
        private void RefreshDataGrid(Switch theSw)
        {
            DataTable dt = GetNodeTable(theSw);
            lock (dt)
            {
                this.Invoke(new Action(() =>
                {
                    dgv_nodes.DataSource = dt;
                    dgv_nodes.Columns[0].Width = 150;
                    dgv_nodes.Columns[1].Width = 100;
                    dgv_nodes.Columns[2].Width = 100;
                }));
            }
        }
        private DataTable GetNodeTable(Switch theSw)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("UDP ");
            dt.Columns.Add("Websocket");
            dt.Columns.Add("Lifetime");

            IEnumerable<NODE> nodes = theSw.GetNodeList();
            lock (nodes)
            {
                foreach (NODE node in theSw.GetNodeList().ToArray())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = node.ID;
                    dr[1] = node.UDPConnections==null? 0:node.UDPConnections.Count;
                    dr[2] = node.WebsocketConnections == null ? 0 : node.WebsocketConnections.Count;
                    dr[3] = node.ttl;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        String s_output = "";
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

    }

}
