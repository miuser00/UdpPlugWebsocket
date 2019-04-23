namespace DoorControl
{
    partial class WebSocketForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.rtb_Server = new System.Windows.Forms.RichTextBox();
            this.btn_Send = new System.Windows.Forms.Button();
            this.txt_server = new System.Windows.Forms.TextBox();
            this.txt_msg_serversend = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lis_sessions = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtb_Server
            // 
            this.rtb_Server.Location = new System.Drawing.Point(377, 66);
            this.rtb_Server.Name = "rtb_Server";
            this.rtb_Server.Size = new System.Drawing.Size(415, 199);
            this.rtb_Server.TabIndex = 0;
            this.rtb_Server.Text = "";
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(706, 23);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(70, 21);
            this.btn_Send.TabIndex = 1;
            this.btn_Send.Text = "发送";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // txt_server
            // 
            this.txt_server.Location = new System.Drawing.Point(101, 27);
            this.txt_server.Name = "txt_server";
            this.txt_server.Size = new System.Drawing.Size(183, 21);
            this.txt_server.TabIndex = 2;
            this.txt_server.Text = "0.0.0.0:6666";
            // 
            // txt_msg_serversend
            // 
            this.txt_msg_serversend.Location = new System.Drawing.Point(477, 23);
            this.txt_msg_serversend.Name = "txt_msg_serversend";
            this.txt_msg_serversend.Size = new System.Drawing.Size(210, 21);
            this.txt_msg_serversend.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(424, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Message";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(375, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Log";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lis_sessions);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rtb_Server);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btn_Send);
            this.groupBox1.Controls.Add(this.txt_server);
            this.groupBox1.Controls.Add(this.txt_msg_serversend);
            this.groupBox1.Location = new System.Drawing.Point(46, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(821, 367);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(290, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 22);
            this.button1.TabIndex = 9;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "Server IP";
            // 
            // lis_sessions
            // 
            this.lis_sessions.FormattingEnabled = true;
            this.lis_sessions.ItemHeight = 12;
            this.lis_sessions.Location = new System.Drawing.Point(38, 66);
            this.lis_sessions.Name = "lis_sessions";
            this.lis_sessions.Size = new System.Drawing.Size(304, 196);
            this.lis_sessions.TabIndex = 7;
            // 
            // WebSocketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 693);
            this.Controls.Add(this.groupBox1);
            this.Name = "WebSocketForm";
            this.Text = "WebSocketServer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_Server;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.TextBox txt_server;
        private System.Windows.Forms.TextBox txt_msg_serversend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lis_sessions;
        private System.Windows.Forms.Button button1;
    }
}

