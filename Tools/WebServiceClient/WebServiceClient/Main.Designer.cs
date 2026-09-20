namespace WebServiceClient
{
    partial class Main
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
            this.txtEndPoint = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnInvoke = new System.Windows.Forms.Button();
            this.chkCert = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lbEndPoints = new System.Windows.Forms.ListBox();
            this.cbCerts = new System.Windows.Forms.ComboBox();
            this.lbService = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.cbStoreName = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbStoreLocation = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtEndPoint
            // 
            this.txtEndPoint.Location = new System.Drawing.Point(140, 71);
            this.txtEndPoint.Name = "txtEndPoint";
            this.txtEndPoint.Size = new System.Drawing.Size(808, 22);
            this.txtEndPoint.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Endpoint";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(246, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(436, 46);
            this.label2.TabIndex = 2;
            this.label2.Text = "Web Service Test App";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(140, 316);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(157, 22);
            this.txtUserName.TabIndex = 3;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(26, 319);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(79, 17);
            this.lblUser.TabIndex = 4;
            this.lblUser.Text = "User Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(696, 321);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(781, 316);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(167, 22);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 491);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Result";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(140, 491);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(807, 286);
            this.txtResult.TabIndex = 8;
            // 
            // btnInvoke
            // 
            this.btnInvoke.Location = new System.Drawing.Point(139, 795);
            this.btnInvoke.Name = "btnInvoke";
            this.btnInvoke.Size = new System.Drawing.Size(188, 36);
            this.btnInvoke.TabIndex = 9;
            this.btnInvoke.Text = "Call Web Service";
            this.btnInvoke.UseVisualStyleBackColor = true;
            this.btnInvoke.Click += new System.EventHandler(this.btnInvoke_Click);
            // 
            // chkCert
            // 
            this.chkCert.AutoSize = true;
            this.chkCert.Checked = true;
            this.chkCert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCert.Location = new System.Drawing.Point(140, 357);
            this.chkCert.Name = "chkCert";
            this.chkCert.Size = new System.Drawing.Size(161, 21);
            this.chkCert.TabIndex = 10;
            this.chkCert.Text = "Use Client Certificate";
            this.chkCert.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(136, 396);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Client Certificate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 451);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Status";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(139, 451);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(808, 22);
            this.txtStatus.TabIndex = 14;
            // 
            // lbEndPoints
            // 
            this.lbEndPoints.FormattingEnabled = true;
            this.lbEndPoints.ItemHeight = 16;
            this.lbEndPoints.Location = new System.Drawing.Point(141, 99);
            this.lbEndPoints.Name = "lbEndPoints";
            this.lbEndPoints.Size = new System.Drawing.Size(807, 84);
            this.lbEndPoints.TabIndex = 15;
            this.lbEndPoints.SelectedIndexChanged += new System.EventHandler(this.lbEndPoints_SelectedIndexChanged);
            // 
            // cbCerts
            // 
            this.cbCerts.FormattingEnabled = true;
            this.cbCerts.Location = new System.Drawing.Point(297, 396);
            this.cbCerts.Name = "cbCerts";
            this.cbCerts.Size = new System.Drawing.Size(651, 24);
            this.cbCerts.TabIndex = 16;
            // 
            // lbService
            // 
            this.lbService.FormattingEnabled = true;
            this.lbService.ItemHeight = 16;
            this.lbService.Location = new System.Drawing.Point(140, 193);
            this.lbService.Name = "lbService";
            this.lbService.Size = new System.Drawing.Size(808, 100);
            this.lbService.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 193);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 17);
            this.label7.TabIndex = 4;
            this.label7.Text = "Service/Method";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(813, 795);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(134, 36);
            this.btnClear.TabIndex = 18;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cbStoreName
            // 
            this.cbStoreName.FormattingEnabled = true;
            this.cbStoreName.Location = new System.Drawing.Point(781, 357);
            this.cbStoreName.Name = "cbStoreName";
            this.cbStoreName.Size = new System.Drawing.Size(166, 24);
            this.cbStoreName.TabIndex = 19;
            this.cbStoreName.SelectedIndexChanged += new System.EventHandler(this.cbStoreName_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(692, 358);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 17);
            this.label8.TabIndex = 5;
            this.label8.Text = "Store Name";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(348, 357);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 17);
            this.label9.TabIndex = 5;
            this.label9.Text = "Store Location";
            // 
            // cbStoreLocation
            // 
            this.cbStoreLocation.FormattingEnabled = true;
            this.cbStoreLocation.Location = new System.Drawing.Point(454, 357);
            this.cbStoreLocation.Name = "cbStoreLocation";
            this.cbStoreLocation.Size = new System.Drawing.Size(166, 24);
            this.cbStoreLocation.TabIndex = 19;
            this.cbStoreLocation.SelectedIndexChanged += new System.EventHandler(this.cbStoreLocation_SelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 843);
            this.Controls.Add(this.cbStoreLocation);
            this.Controls.Add(this.cbStoreName);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lbService);
            this.Controls.Add(this.cbCerts);
            this.Controls.Add(this.lbEndPoints);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkCert);
            this.Controls.Add(this.btnInvoke);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEndPoint);
            this.Name = "Main";
            this.Text = "Main";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEndPoint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnInvoke;
        private System.Windows.Forms.CheckBox chkCert;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.ListBox lbEndPoints;
        private System.Windows.Forms.ComboBox cbCerts;
        private System.Windows.Forms.ListBox lbService;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox cbStoreName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbStoreLocation;
    }
}

