using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UdpPlugWebsocket
{
    public partial class MessageForm : Form
    {
        private MessageForm()
        {
            //打开添加用户面板
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            InitializeComponent();
            Instance = this;
            this.FormClosed += Log_FormClosed;

        }
        private void Log_FormClosed(object sender, FormClosedEventArgs e)
        {
            Instance = null;
        }

        private static MessageForm _instance;

        public static MessageForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MessageForm();
                return _instance;
            }
            private set { _instance = value; }
        }

        String s_output = "";
        public void SetOutput(string text)
        {
            //决定是否屏显
            if (SetupForm.cfg.EnableScreenLog == false) return;

            text = DateTime.Now.ToLongDateString() +" "+DateTime.Now.ToLongTimeString()+ " " + text;
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

        public static void Log(string text)
        {
            if (_instance != null)
            {
                _instance.SetOutput(text);
            }
        }

        private void Log_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
