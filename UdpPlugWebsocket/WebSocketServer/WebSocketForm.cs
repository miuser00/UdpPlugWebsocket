using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace DoorControl
{
    public partial class WebSocketForm : Form
    {
        WebSocketServer wssv;


        private delegate void DelegateStringFun(string log);//代理
    
        public static WebSocketForm frm;
        public string text;
        public string textclient;

        //自身的句柄
        public static void SendMessage(String msg)
        {
            frm.Log(msg);
        }

        public static void ConnectionAccepted(String id)
        {
            frm.AddList(id);
        }

        public static void ConnectionDismiss(String id)
        {
            frm.RemoveList(id);
        }
        public static String logtxt = "";

        public WebSocketForm()
        {
            InitializeComponent();
            frm = this;


        }
        public string Getlog()
        {
            string ret = logtxt;
            logtxt = "";
            return ret;
        }

        public class MyEventArg : EventArgs
        {
            //传递主窗体的数据信息
            public string Text { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             wssv = new WebSocketServer("ws://" +txt_server.Text);

        }
        public class Laputa : WebSocketBehavior
        {
            protected override void OnOpen()
            {
                base.OnOpen();
                SendMessage("Connect from "+Context.Host + " was accepted");
                ConnectionAccepted(ID+"|"+ Context.Host.ToString());

            }
            protected override void OnClose(CloseEventArgs e)
            {
                SendMessage("Connect from " + Context.Host + " was lost");
                ConnectionDismiss(ID + "|" + Context.Host.ToString());
                base.OnClose(e);

            }
            protected override void OnMessage(MessageEventArgs e)
            {

                string s_data = e.Data;
                Logger log=new Logger();
                SendMessage("Server Received: "+e.Data);
                Console.Write(e.Data);
                //Loopback
                //Send(e.Data);
                
                
            }
        }


        public void Log(string log)
        {
            if (this.rtb_Server.InvokeRequired)
            {
                DelegateStringFun d = new DelegateStringFun(Log);
                this.Invoke(d, new object[] { log });
            }
            else
            {
                text= text + log.Replace("\0","") + "\n";
                rtb_Server.Text = text;
            }
        }

        public void AddList(string id)
        {
            if (this.rtb_Server.InvokeRequired)
            {
                DelegateStringFun d = new DelegateStringFun(AddList);
                this.Invoke(d, new object[] { id });
            }
            else
            {
                lis_sessions.Items.Add(id);
                
            }
        }

        public void RemoveList(string id)
        {
            if (this.rtb_Server.InvokeRequired)
            {
                DelegateStringFun d = new DelegateStringFun(RemoveList);
                this.Invoke(d, new object[] { id });
            }
            else
            {
                lis_sessions.Items.Remove(id);     
            }
        }


        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                string s_mixid = lis_sessions.SelectedItem.ToString();
                string s_id = s_mixid.Substring(0, s_mixid.IndexOf("|"));
                wssv.WebSocketServices["/"].Sessions.SendToAsync(txt_msg_serversend.Text, s_id, null);
            }catch
            {
                MessageBox.Show("请先选择session");
            }

        }


        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
