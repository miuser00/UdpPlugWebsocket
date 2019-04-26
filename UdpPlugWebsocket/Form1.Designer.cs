namespace UdpPlugWebsocket
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.配置ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.系统配置ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示调试信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.硬件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pan_Left = new System.Windows.Forms.Panel();
            this.btn_Panel = new System.Windows.Forms.Button();
            this.btn_Browser = new System.Windows.Forms.Button();
            this.btn_Device = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slab_bottom = new System.Windows.Forms.ToolStripStatusLabel();
            this.pan_Fill = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_Message = new System.Windows.Forms.TabPage();
            this.tab_Error = new System.Windows.Forms.TabPage();
            this.pan_Top = new System.Windows.Forms.Panel();
            this.tm_HearBeat = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.pan_Left.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pan_Fill.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.配置ToolStripMenuItem,
            this.配置ToolStripMenuItem1,
            this.硬件ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1121, 29);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "mnu_Main";
            // 
            // 配置ToolStripMenuItem
            // 
            this.配置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统配置ToolStripMenuItem});
            this.配置ToolStripMenuItem.Name = "配置ToolStripMenuItem";
            this.配置ToolStripMenuItem.Size = new System.Drawing.Size(12, 25);
            // 
            // 系统配置ToolStripMenuItem
            // 
            this.系统配置ToolStripMenuItem.Name = "系统配置ToolStripMenuItem";
            this.系统配置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.系统配置ToolStripMenuItem.Text = "系统配置";
            // 
            // 配置ToolStripMenuItem1
            // 
            this.配置ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统配置ToolStripMenuItem1,
            this.exitToolStripMenuItem,
            this.显示调试信息ToolStripMenuItem});
            this.配置ToolStripMenuItem1.Font = new System.Drawing.Font("新宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.配置ToolStripMenuItem1.Name = "配置ToolStripMenuItem1";
            this.配置ToolStripMenuItem1.Size = new System.Drawing.Size(66, 25);
            this.配置ToolStripMenuItem1.Text = "配置";
            // 
            // 系统配置ToolStripMenuItem1
            // 
            this.系统配置ToolStripMenuItem1.Name = "系统配置ToolStripMenuItem1";
            this.系统配置ToolStripMenuItem1.Size = new System.Drawing.Size(212, 26);
            this.系统配置ToolStripMenuItem1.Text = "系统配置";
            this.系统配置ToolStripMenuItem1.Click += new System.EventHandler(this.系统配置ToolStripMenuItem1_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.exitToolStripMenuItem.Text = "退出";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // 显示调试信息ToolStripMenuItem
            // 
            this.显示调试信息ToolStripMenuItem.Name = "显示调试信息ToolStripMenuItem";
            this.显示调试信息ToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.显示调试信息ToolStripMenuItem.Text = "显示调试信息";
            this.显示调试信息ToolStripMenuItem.Click += new System.EventHandler(this.显示调试信息ToolStripMenuItem_Click);
            // 
            // 硬件ToolStripMenuItem
            // 
            this.硬件ToolStripMenuItem.Font = new System.Drawing.Font("新宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.硬件ToolStripMenuItem.Name = "硬件ToolStripMenuItem";
            this.硬件ToolStripMenuItem.Size = new System.Drawing.Size(66, 25);
            this.硬件ToolStripMenuItem.Text = "帮助";
            this.硬件ToolStripMenuItem.Click += new System.EventHandler(this.硬件ToolStripMenuItem_Click);
            // 
            // pan_Left
            // 
            this.pan_Left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pan_Left.Controls.Add(this.btn_Panel);
            this.pan_Left.Controls.Add(this.btn_Browser);
            this.pan_Left.Controls.Add(this.btn_Device);
            this.pan_Left.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_Left.Location = new System.Drawing.Point(0, 29);
            this.pan_Left.Name = "pan_Left";
            this.pan_Left.Size = new System.Drawing.Size(263, 613);
            this.pan_Left.TabIndex = 24;
            // 
            // btn_Panel
            // 
            this.btn_Panel.BackColor = System.Drawing.Color.White;
            this.btn_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_Panel.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Panel.Location = new System.Drawing.Point(0, 100);
            this.btn_Panel.Name = "btn_Panel";
            this.btn_Panel.Size = new System.Drawing.Size(261, 50);
            this.btn_Panel.TabIndex = 8;
            this.btn_Panel.Text = "交换面板";
            this.btn_Panel.UseVisualStyleBackColor = false;
            this.btn_Panel.Click += new System.EventHandler(this.btn_Panel_Click);
            // 
            // btn_Browser
            // 
            this.btn_Browser.BackColor = System.Drawing.Color.White;
            this.btn_Browser.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_Browser.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Browser.Location = new System.Drawing.Point(0, 50);
            this.btn_Browser.Name = "btn_Browser";
            this.btn_Browser.Size = new System.Drawing.Size(261, 50);
            this.btn_Browser.TabIndex = 6;
            this.btn_Browser.Text = "控制页面";
            this.btn_Browser.UseVisualStyleBackColor = false;
            this.btn_Browser.Click += new System.EventHandler(this.btn_Browser_Click);
            // 
            // btn_Device
            // 
            this.btn_Device.BackColor = System.Drawing.Color.White;
            this.btn_Device.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_Device.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Device.Location = new System.Drawing.Point(0, 0);
            this.btn_Device.Name = "btn_Device";
            this.btn_Device.Size = new System.Drawing.Size(261, 50);
            this.btn_Device.TabIndex = 5;
            this.btn_Device.Text = "终端设备";
            this.btn_Device.UseVisualStyleBackColor = false;
            this.btn_Device.Click += new System.EventHandler(this.btn_Device_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slab_bottom});
            this.statusStrip1.Location = new System.Drawing.Point(0, 642);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1121, 22);
            this.statusStrip1.TabIndex = 60;
            this.statusStrip1.Text = "状态:";
            // 
            // slab_bottom
            // 
            this.slab_bottom.AutoSize = false;
            this.slab_bottom.BackColor = System.Drawing.SystemColors.Control;
            this.slab_bottom.Name = "slab_bottom";
            this.slab_bottom.Size = new System.Drawing.Size(300, 17);
            this.slab_bottom.Text = "状态：";
            this.slab_bottom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pan_Fill
            // 
            this.pan_Fill.BackColor = System.Drawing.Color.White;
            this.pan_Fill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pan_Fill.Controls.Add(this.splitter1);
            this.pan_Fill.Controls.Add(this.tabControl1);
            this.pan_Fill.Controls.Add(this.pan_Top);
            this.pan_Fill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan_Fill.Location = new System.Drawing.Point(263, 29);
            this.pan_Fill.Name = "pan_Fill";
            this.pan_Fill.Size = new System.Drawing.Size(858, 613);
            this.pan_Fill.TabIndex = 61;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 507);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(856, 2);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tab_Message);
            this.tabControl1.Controls.Add(this.tab_Error);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 507);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(856, 104);
            this.tabControl1.TabIndex = 7;
            // 
            // tab_Message
            // 
            this.tab_Message.Location = new System.Drawing.Point(4, 4);
            this.tab_Message.Name = "tab_Message";
            this.tab_Message.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Message.Size = new System.Drawing.Size(848, 78);
            this.tab_Message.TabIndex = 1;
            this.tab_Message.Text = "Message";
            this.tab_Message.UseVisualStyleBackColor = true;
            // 
            // tab_Error
            // 
            this.tab_Error.Location = new System.Drawing.Point(4, 4);
            this.tab_Error.Name = "tab_Error";
            this.tab_Error.Size = new System.Drawing.Size(848, 78);
            this.tab_Error.TabIndex = 2;
            this.tab_Error.Text = "Error";
            this.tab_Error.UseVisualStyleBackColor = true;
            // 
            // pan_Top
            // 
            this.pan_Top.BackColor = System.Drawing.Color.White;
            this.pan_Top.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pan_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan_Top.Location = new System.Drawing.Point(0, 0);
            this.pan_Top.Name = "pan_Top";
            this.pan_Top.Size = new System.Drawing.Size(856, 507);
            this.pan_Top.TabIndex = 4;
            // 
            // tm_HearBeat
            // 
            this.tm_HearBeat.Enabled = true;
            this.tm_HearBeat.Interval = 10000;
            this.tm_HearBeat.Tick += new System.EventHandler(this.tm_HearBeat_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 664);
            this.Controls.Add(this.pan_Fill);
            this.Controls.Add(this.pan_Left);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "UdpPlugWebsocket 0.11";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pan_Left.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pan_Fill.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 配置ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 系统配置ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 硬件ToolStripMenuItem;
        private System.Windows.Forms.Panel pan_Left;
        private System.Windows.Forms.Button btn_Panel;
        private System.Windows.Forms.Button btn_Browser;
        private System.Windows.Forms.Button btn_Device;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slab_bottom;
        private System.Windows.Forms.Panel pan_Fill;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_Error;
        private System.Windows.Forms.TabPage tab_Message;
        public System.Windows.Forms.Panel pan_Top;
        private System.Windows.Forms.ToolStripMenuItem 显示调试信息ToolStripMenuItem;
        private System.Windows.Forms.Timer tm_HearBeat;
    }
}

