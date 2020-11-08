namespace ValisApplicationService.GuiElements
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageManual = new System.Windows.Forms.TabPage();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.chkInfo = new System.Windows.Forms.CheckBox();
            this.chkWarning = new System.Windows.Forms.CheckBox();
            this.chkError = new System.Windows.Forms.CheckBox();
            this.traceListView = new System.Windows.Forms.ListView();
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnThread = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnLogger = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnException = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripManual = new System.Windows.Forms.ToolStrip();
            this.btnStart = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.manualTimerEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEmulateTimerPulse = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.tabControl1.SuspendLayout();
            this.tabPageManual.SuspendLayout();
            this.toolStripManual.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPageManual);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1173, 600);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageManual
            // 
            this.tabPageManual.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageManual.Controls.Add(this.chkDebug);
            this.tabPageManual.Controls.Add(this.chkInfo);
            this.tabPageManual.Controls.Add(this.chkWarning);
            this.tabPageManual.Controls.Add(this.chkError);
            this.tabPageManual.Controls.Add(this.traceListView);
            this.tabPageManual.Controls.Add(this.toolStripManual);
            this.tabPageManual.Location = new System.Drawing.Point(4, 25);
            this.tabPageManual.Name = "tabPageManual";
            this.tabPageManual.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageManual.Size = new System.Drawing.Size(1165, 571);
            this.tabPageManual.TabIndex = 0;
            this.tabPageManual.Text = "Service - Manual Control";
            this.tabPageManual.UseVisualStyleBackColor = true;
            // 
            // chkDebug
            // 
            this.chkDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDebug.AutoSize = true;
            this.chkDebug.Checked = true;
            this.chkDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDebug.Location = new System.Drawing.Point(193, 544);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(58, 17);
            this.chkDebug.TabIndex = 5;
            this.chkDebug.Text = "Debug";
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // chkInfo
            // 
            this.chkInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkInfo.AutoSize = true;
            this.chkInfo.Checked = true;
            this.chkInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInfo.Location = new System.Drawing.Point(143, 544);
            this.chkInfo.Name = "chkInfo";
            this.chkInfo.Size = new System.Drawing.Size(44, 17);
            this.chkInfo.TabIndex = 4;
            this.chkInfo.Text = "Info";
            this.chkInfo.UseVisualStyleBackColor = true;
            // 
            // chkWarning
            // 
            this.chkWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkWarning.AutoSize = true;
            this.chkWarning.Checked = true;
            this.chkWarning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWarning.Location = new System.Drawing.Point(66, 546);
            this.chkWarning.Name = "chkWarning";
            this.chkWarning.Size = new System.Drawing.Size(71, 17);
            this.chkWarning.TabIndex = 3;
            this.chkWarning.Text = "Warnings";
            this.chkWarning.UseVisualStyleBackColor = true;
            // 
            // chkError
            // 
            this.chkError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkError.AutoSize = true;
            this.chkError.Checked = true;
            this.chkError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkError.Enabled = false;
            this.chkError.Location = new System.Drawing.Point(7, 546);
            this.chkError.Name = "chkError";
            this.chkError.Size = new System.Drawing.Size(53, 17);
            this.chkError.TabIndex = 2;
            this.chkError.Text = "Errors";
            this.chkError.UseVisualStyleBackColor = true;
            // 
            // traceListView
            // 
            this.traceListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.traceListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.traceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnDate,
            this.columnThread,
            this.columnLevel,
            this.columnLogger,
            this.columnMessage,
            this.columnException});
            this.traceListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(161)));
            this.traceListView.FullRowSelect = true;
            this.traceListView.GridLines = true;
            this.traceListView.Location = new System.Drawing.Point(5, 31);
            this.traceListView.Name = "traceListView";
            this.traceListView.Size = new System.Drawing.Size(1154, 503);
            this.traceListView.TabIndex = 1;
            this.traceListView.UseCompatibleStateImageBehavior = false;
            this.traceListView.View = System.Windows.Forms.View.Details;
            // 
            // columnDate
            // 
            this.columnDate.Text = "Date";
            this.columnDate.Width = 138;
            // 
            // columnThread
            // 
            this.columnThread.Text = "Thread";
            this.columnThread.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnThread.Width = 64;
            // 
            // columnLevel
            // 
            this.columnLevel.Text = "Level";
            this.columnLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnLevel.Width = 70;
            // 
            // columnLogger
            // 
            this.columnLogger.Text = "Logger";
            this.columnLogger.Width = 208;
            // 
            // columnMessage
            // 
            this.columnMessage.Text = "Message";
            this.columnMessage.Width = 321;
            // 
            // columnException
            // 
            this.columnException.Text = "Exception";
            this.columnException.Width = 351;
            // 
            // toolStripManual
            // 
            this.toolStripManual.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripManual.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStart,
            this.btnStop,
            this.toolStripSeparator1,
            this.toolStripDropDownButton1,
            this.btnEmulateTimerPulse,
            this.toolStripSeparator2,
            this.btnClear});
            this.toolStripManual.Location = new System.Drawing.Point(3, 3);
            this.toolStripManual.Name = "toolStripManual";
            this.toolStripManual.Size = new System.Drawing.Size(1157, 25);
            this.toolStripManual.TabIndex = 0;
            this.toolStripManual.Text = "toolStrip1";
            // 
            // btnStart
            // 
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(51, 22);
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.OnBtnStart);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(51, 22);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.OnBtnStop);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualTimerEventToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // manualTimerEventToolStripMenuItem
            // 
            this.manualTimerEventToolStripMenuItem.Name = "manualTimerEventToolStripMenuItem";
            this.manualTimerEventToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.manualTimerEventToolStripMenuItem.Text = "Manual Timer Event";
            // 
            // btnEmulateTimerPulse
            // 
            this.btnEmulateTimerPulse.Enabled = false;
            this.btnEmulateTimerPulse.Image = ((System.Drawing.Image)(resources.GetObject("btnEmulateTimerPulse.Image")));
            this.btnEmulateTimerPulse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEmulateTimerPulse.Name = "btnEmulateTimerPulse";
            this.btnEmulateTimerPulse.Size = new System.Drawing.Size(136, 22);
            this.btnEmulateTimerPulse.Text = "Emulate Timer Event";
            this.btnEmulateTimerPulse.CheckedChanged += new System.EventHandler(this.OnManualTimerEventCheckedChanged);
            this.btnEmulateTimerPulse.Click += new System.EventHandler(this.btnEmulateTimerPulse_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClear
            // 
            this.btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(38, 22);
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.OnbtnClear);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 600);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainWindow";
            this.Text = "Valis Application Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageManual.ResumeLayout(false);
            this.tabPageManual.PerformLayout();
            this.toolStripManual.ResumeLayout(false);
            this.toolStripManual.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        internal System.Windows.Forms.TabPage tabPageManual;
        internal System.Windows.Forms.ToolStrip toolStripManual;
        private System.Windows.Forms.ToolStripButton btnStart;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem manualTimerEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnEmulateTimerPulse;
        private System.Windows.Forms.ToolStripButton btnClear;
        internal System.Windows.Forms.ListView traceListView;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.ColumnHeader columnThread;
        private System.Windows.Forms.ColumnHeader columnLevel;
        private System.Windows.Forms.ColumnHeader columnLogger;
        private System.Windows.Forms.ColumnHeader columnMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.CheckBox chkDebug;
        public System.Windows.Forms.CheckBox chkInfo;
        public System.Windows.Forms.CheckBox chkWarning;
        public System.Windows.Forms.CheckBox chkError;
        private System.Windows.Forms.ColumnHeader columnException;
    }
}