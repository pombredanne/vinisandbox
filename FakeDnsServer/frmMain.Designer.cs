namespace FakeDnsServer
{
    partial class frmMain
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
            this.lstDomains = new System.Windows.Forms.ListBox();
            this.lblIp = new System.Windows.Forms.Label();
            this.lblLog = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnFdlg = new System.Windows.Forms.Button();
            this.sfdLog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // lstDomains
            // 
            this.lstDomains.FormattingEnabled = true;
            this.lstDomains.Location = new System.Drawing.Point(12, 12);
            this.lstDomains.Name = "lstDomains";
            this.lstDomains.Size = new System.Drawing.Size(139, 238);
            this.lstDomains.TabIndex = 0;
            // 
            // lblIp
            // 
            this.lblIp.AutoSize = true;
            this.lblIp.Location = new System.Drawing.Point(163, 96);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new System.Drawing.Size(20, 13);
            this.lblIp.TabIndex = 1;
            this.lblIp.Text = "IP:";
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(163, 121);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(25, 13);
            this.lblLog.TabIndex = 2;
            this.lblLog.Text = "Log";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(204, 93);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(100, 20);
            this.txtIp.TabIndex = 3;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(204, 118);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(100, 20);
            this.txtLog.TabIndex = 4;
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(321, 91);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(75, 23);
            this.btnIniciar.TabIndex = 5;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnFdlg
            // 
            this.btnFdlg.Location = new System.Drawing.Point(321, 116);
            this.btnFdlg.Name = "btnFdlg";
            this.btnFdlg.Size = new System.Drawing.Size(27, 23);
            this.btnFdlg.TabIndex = 7;
            this.btnFdlg.Text = "...";
            this.btnFdlg.UseVisualStyleBackColor = true;
            this.btnFdlg.Click += new System.EventHandler(this.btnFdlg_Click);
            // 
            // sfdLog
            // 
            this.sfdLog.DefaultExt = "txt";
            this.sfdLog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 266);
            this.Controls.Add(this.btnFdlg);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lblIp);
            this.Controls.Add(this.lstDomains);
            this.Name = "frmMain";
            this.Text = "FakeDnsServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstDomains;
        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnFdlg;
        private System.Windows.Forms.SaveFileDialog sfdLog;
    }
}