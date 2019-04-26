using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UdpPlugWebsocket
{
    public partial class Form1 : Form
    {
        //所有窗体的容器
        List<Form> forms = new List<Form>();

        //初始化配置窗体
        SetupForm setup = new SetupForm();

        //初始化所有窗体对象，每个窗体对应一个功能模组
        private void Init_Forms()
        {
            try
            {
                //添加终端设备窗体
                forms.Add(Device.Instance);
                //添加浏览器窗体
                forms.Add(Browser.Instance);
                //添加交换面板窗体
                forms.Add(Panel.Instance);

                //配置窗体为子窗体，并添加到主窗体上
                foreach (Form frm in forms)
                {
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    pan_Top.Controls.Add(frm);
                }

                tab_Message.Controls.Add(MessageForm.Instance);
                MessageForm.Instance.Show();

                tab_Error.Controls.Add(ErrorForm.Instance);
                ErrorForm.Instance.Show();

                //连接模块与系统终端窗口
                Device.Instance.HandleMessage += new Action<string>(MessageForm.Log);
                Device.Instance.HandleError += new Action<string>(ErrorForm.Log);

                Browser.Instance.HandleMessage += new Action<string>(MessageForm.Log);
                Browser.Instance.HandleError += new Action<string>(ErrorForm.Log);
                //连接Pannel数据输入端   Device->Panel
                Device.Instance.server.HandleRecMsg += new Action<byte[], Miuser.NUDP.Sockets.SocketConnection, Miuser.NUDP.Sockets.SocketServer>((bytes, conn, server) =>
                {
                    Panel.Instance.sw.SendFromUDP(bytes, conn.Tag.ToString());
                });
                //连接Pannel数据输入端   Browser->Panel
                Browser.Instance.HandleWebSocketRecMsg += new Action<byte[], WebSocketSharp.Net.WebSockets.WebSocketContext>((bytes, context) =>
                {
                    Panel.Instance.sw.SendFromWebSocket(bytes, context.UserEndPoint.ToString());
                });
                //连接Pannel数据输出端   Panel->Device
                Panel.Instance.sw.HandleUDPSendMsg += new Action<byte[], string>((bytes, endpointString) =>
                {
                    Device.Instance.server.Send(endpointString, bytes);
                });
                //连接Panel数据输出端  Panel->Browser
                Panel.Instance.sw.HandleWebSocketSendMsg += new Action<byte[], string>((bytes, endpointString)=>
                {
                    Browser.Instance.Send(endpointString, bytes);
                });

                //刷新菜单状态
                显示调试信息ToolStripMenuItem.Checked = SetupForm.cfg.EnableScreenLog;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace,"系统模块初始化错误");
                
            }
        }

        //使所有子窗体不可见
        private void Invislble_Forms()
        {
            foreach (Form frm in forms)
            {
                frm.Visible = false;
            }
        }

        //根据类名获得窗体对象
        private Form Get_Form(string className)
        {
            foreach (Form frm in forms)
            {
                if (frm.GetType().Name == className)
                {
                    return frm;
                }
            }
            return null;
        }
        private void ShowModule(string moduleName)
        {
            try
            {
                if (Get_Form(moduleName) != null) Get_Form(moduleName).Visible = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace, "系统模块初始化错误");
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            Init_Forms();
            ShowModule("Device");
        }

        private void btn_Device_Click(object sender, EventArgs e)
        {
            Invislble_Forms();
            ShowModule("Device");

        }

        private void btn_Browser_Click(object sender, EventArgs e)
        {
            {
                Invislble_Forms();
                ShowModule("Browser");
            }
        }

        private void btn_Panel_Click(object sender, EventArgs e)
        {
            Invislble_Forms();
            ShowModule("Panel");
        }

        private void 系统配置ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            setup.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void 硬件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("UPW 0.1 \n Tks Song");
        }

        private void btn_LogScreen_Click(object sender, EventArgs e)
        {
        }

        private void 显示调试信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupForm.cfg.EnableScreenLog = !SetupForm.cfg.EnableScreenLog;
            显示调试信息ToolStripMenuItem.Checked = SetupForm.cfg.EnableScreenLog;
        }

        private void tm_HearBeat_Tick(object sender, EventArgs e)
        {
            Console.Write("Heatbeating " + DateTime.Now.ToShortDateString());
        }
    }
}
