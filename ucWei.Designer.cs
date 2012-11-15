namespace Weiddler
{
    partial class ucWei
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorDialogUrl = new System.Windows.Forms.ColorDialog();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtColor = new System.Windows.Forms.TextBox();
            this.btnSaveUrlColor = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgUrlColors = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer_NU = new System.Windows.Forms.SplitContainer();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnTelnet = new System.Windows.Forms.Button();
            this.btnTracert = new System.Windows.Forms.Button();
            this.btnNSLookup = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnPing = new System.Windows.Forms.Button();
            this.txtHostOrIP = new System.Windows.Forms.TextBox();
            this.rtbNU = new System.Windows.Forms.RichTextBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgUrlColors)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.splitContainer_NU.Panel1.SuspendLayout();
            this.splitContainer_NU.Panel2.SuspendLayout();
            this.splitContainer_NU.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(31, 8);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(199, 20);
            this.txtUrl.TabIndex = 0;
            // 
            // txtColor
            // 
            this.txtColor.Location = new System.Drawing.Point(288, 8);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(22, 20);
            this.txtColor.TabIndex = 1;
            this.txtColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtColor_MouseClick);
            // 
            // btnSaveUrlColor
            // 
            this.btnSaveUrlColor.Location = new System.Drawing.Point(397, 6);
            this.btnSaveUrlColor.Name = "btnSaveUrlColor";
            this.btnSaveUrlColor.Size = new System.Drawing.Size(75, 23);
            this.btnSaveUrlColor.TabIndex = 2;
            this.btnSaveUrlColor.Text = "SaveColor";
            this.btnSaveUrlColor.UseVisualStyleBackColor = true;
            this.btnSaveUrlColor.Click += new System.EventHandler(this.btnSaveUrlColor_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Url:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Color";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(589, 477);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnRemove);
            this.tabPage1.Controls.Add(this.dgUrlColors);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btnSaveUrlColor);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtUrl);
            this.tabPage1.Controls.Add(this.txtColor);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(581, 451);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "ColorYourUrl";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgUrlColors
            // 
            this.dgUrlColors.AllowUserToAddRows = false;
            this.dgUrlColors.AllowUserToDeleteRows = false;
            this.dgUrlColors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUrlColors.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgUrlColors.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgUrlColors.Location = new System.Drawing.Point(3, 35);
            this.dgUrlColors.Name = "dgUrlColors";
            this.dgUrlColors.ReadOnly = true;
            this.dgUrlColors.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgUrlColors.Size = new System.Drawing.Size(575, 413);
            this.dgUrlColors.TabIndex = 6;
            this.dgUrlColors.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgUrlColors_CellFormatting);
            this.dgUrlColors.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUrlColors_RowEnter);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer_NU);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(581, 451);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "NetworkUtility";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer_NU
            // 
            this.splitContainer_NU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_NU.Location = new System.Drawing.Point(3, 3);
            this.splitContainer_NU.Name = "splitContainer_NU";
            this.splitContainer_NU.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_NU.Panel1
            // 
            this.splitContainer_NU.Panel1.Controls.Add(this.label4);
            this.splitContainer_NU.Panel1.Controls.Add(this.txtPort);
            this.splitContainer_NU.Panel1.Controls.Add(this.btnTelnet);
            this.splitContainer_NU.Panel1.Controls.Add(this.btnTracert);
            this.splitContainer_NU.Panel1.Controls.Add(this.btnNSLookup);
            this.splitContainer_NU.Panel1.Controls.Add(this.label3);
            this.splitContainer_NU.Panel1.Controls.Add(this.btnPing);
            this.splitContainer_NU.Panel1.Controls.Add(this.txtHostOrIP);
            // 
            // splitContainer_NU.Panel2
            // 
            this.splitContainer_NU.Panel2.Controls.Add(this.rtbNU);
            this.splitContainer_NU.Size = new System.Drawing.Size(575, 445);
            this.splitContainer_NU.SplitterDistance = 39;
            this.splitContainer_NU.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(461, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Port:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(496, 6);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(47, 20);
            this.txtPort.TabIndex = 6;
            // 
            // btnTelnet
            // 
            this.btnTelnet.Location = new System.Drawing.Point(405, 3);
            this.btnTelnet.Name = "btnTelnet";
            this.btnTelnet.Size = new System.Drawing.Size(50, 23);
            this.btnTelnet.TabIndex = 5;
            this.btnTelnet.Text = "Telnet";
            this.btnTelnet.UseVisualStyleBackColor = true;
            this.btnTelnet.Click += new System.EventHandler(this.btnTelnet_Click);
            // 
            // btnTracert
            // 
            this.btnTracert.Location = new System.Drawing.Point(347, 3);
            this.btnTracert.Name = "btnTracert";
            this.btnTracert.Size = new System.Drawing.Size(52, 23);
            this.btnTracert.TabIndex = 4;
            this.btnTracert.Text = "Tracert";
            this.btnTracert.UseVisualStyleBackColor = true;
            this.btnTracert.Click += new System.EventHandler(this.btnTracert_Click);
            // 
            // btnNSLookup
            // 
            this.btnNSLookup.Location = new System.Drawing.Point(275, 3);
            this.btnNSLookup.Name = "btnNSLookup";
            this.btnNSLookup.Size = new System.Drawing.Size(66, 23);
            this.btnNSLookup.TabIndex = 3;
            this.btnNSLookup.Text = "NSLookup";
            this.btnNSLookup.UseVisualStyleBackColor = true;
            this.btnNSLookup.Click += new System.EventHandler(this.btnNSLookup_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "IP:";
            // 
            // btnPing
            // 
            this.btnPing.Location = new System.Drawing.Point(216, 3);
            this.btnPing.Name = "btnPing";
            this.btnPing.Size = new System.Drawing.Size(53, 23);
            this.btnPing.TabIndex = 2;
            this.btnPing.Text = "Ping";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // txtHostOrIP
            // 
            this.txtHostOrIP.Location = new System.Drawing.Point(51, 4);
            this.txtHostOrIP.Name = "txtHostOrIP";
            this.txtHostOrIP.Size = new System.Drawing.Size(159, 20);
            this.txtHostOrIP.TabIndex = 1;
            this.txtHostOrIP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPing_KeyPress);
            // 
            // rtbNU
            // 
            this.rtbNU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbNU.Location = new System.Drawing.Point(0, 0);
            this.rtbNU.Name = "rtbNU";
            this.rtbNU.Size = new System.Drawing.Size(575, 402);
            this.rtbNU.TabIndex = 0;
            this.rtbNU.Text = "";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(478, 6);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 7;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // ucWei
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ucWei";
            this.Size = new System.Drawing.Size(589, 477);
            this.Load += new System.EventHandler(this.ucWei_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgUrlColors)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer_NU.Panel1.ResumeLayout(false);
            this.splitContainer_NU.Panel1.PerformLayout();
            this.splitContainer_NU.Panel2.ResumeLayout(false);
            this.splitContainer_NU.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialogUrl;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtColor;
        private System.Windows.Forms.Button btnSaveUrlColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgUrlColors;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnPing;
        private System.Windows.Forms.TextBox txtHostOrIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer_NU;
        private System.Windows.Forms.RichTextBox rtbNU;
        private System.Windows.Forms.Button btnNSLookup;
        private System.Windows.Forms.Button btnTracert;
        private System.Windows.Forms.Button btnTelnet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnRemove;

    }
}
