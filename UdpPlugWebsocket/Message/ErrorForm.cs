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
    public partial class ErrorForm : Form
    {
        private ErrorForm()
        {
            //窗体子窗口化
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            InitializeComponent();
            Instance = this;
            this.FormClosed += Message_FormClosed;


        }

        private void Message_FormClosed(object sender, FormClosedEventArgs e)
        {
            Instance = null;
        }

        private static ErrorForm _instance;

        public static ErrorForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ErrorForm();
                return _instance;
            }
            private set { _instance = value; }
        }

        String s_output = "";
        public void SetOutput(string text)
        {
            text = DateTime.Now.ToLongDateString() +" "+DateTime.Now.ToLongTimeString()+ " " + text;
            Action action = () =>
            {
                s_output = s_output + text + "\r";
                
                if ((s_output.Length)>5000)
                {
                    s_output = s_output.Substring(s_output.Length - 5000, 5000);
                }
                //滚到最后
                this.richTextBox1.Text = s_output;
                this.richTextBox1.Select(richTextBox1.TextLength, 0);
                //this.richTextBox1.Focus();
                this.richTextBox1.ScrollToCaret();

            };

            this.richTextBox1.Invoke(action);
        }

        public static void Log(string text)
        {
            if (_instance != null)
            {
                _instance.SetOutput(text);
            }
        }

        private void Message_Load(object sender, EventArgs e)
        {

        }
    }
}
