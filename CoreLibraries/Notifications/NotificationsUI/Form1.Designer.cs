namespace NotificationsUI
{
    partial class Form1
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
            this.txtFromEmail = new System.Windows.Forms.TextBox();
            this.lblFromEmail = new System.Windows.Forms.Label();
            this.lblReplyTo = new System.Windows.Forms.Label();
            this.txtReplyToEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBccEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPortNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDefaultCredentials = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtClientHost = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTestEmailAddr = new System.Windows.Forms.TextBox();
            this.btnSendEmail = new System.Windows.Forms.Button();
            this.btnSmsTestForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtFromEmail
            // 
            this.txtFromEmail.Location = new System.Drawing.Point(3, 30);
            this.txtFromEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFromEmail.Name = "txtFromEmail";
            this.txtFromEmail.ReadOnly = true;
            this.txtFromEmail.Size = new System.Drawing.Size(384, 22);
            this.txtFromEmail.TabIndex = 0;
            // 
            // lblFromEmail
            // 
            this.lblFromEmail.AutoSize = true;
            this.lblFromEmail.Location = new System.Drawing.Point(-1, 9);
            this.lblFromEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFromEmail.Name = "lblFromEmail";
            this.lblFromEmail.Size = new System.Drawing.Size(78, 17);
            this.lblFromEmail.TabIndex = 1;
            this.lblFromEmail.Text = "From Email";
            // 
            // lblReplyTo
            // 
            this.lblReplyTo.AutoSize = true;
            this.lblReplyTo.Location = new System.Drawing.Point(0, 59);
            this.lblReplyTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReplyTo.Name = "lblReplyTo";
            this.lblReplyTo.Size = new System.Drawing.Size(103, 17);
            this.lblReplyTo.TabIndex = 3;
            this.lblReplyTo.Text = "Reply To Email";
            // 
            // txtReplyToEmail
            // 
            this.txtReplyToEmail.Location = new System.Drawing.Point(3, 80);
            this.txtReplyToEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtReplyToEmail.Name = "txtReplyToEmail";
            this.txtReplyToEmail.ReadOnly = true;
            this.txtReplyToEmail.Size = new System.Drawing.Size(384, 22);
            this.txtReplyToEmail.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 110);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "BCC Email";
            // 
            // txtBccEmail
            // 
            this.txtBccEmail.Location = new System.Drawing.Point(3, 130);
            this.txtBccEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBccEmail.Name = "txtBccEmail";
            this.txtBccEmail.ReadOnly = true;
            this.txtBccEmail.Size = new System.Drawing.Size(384, 22);
            this.txtBccEmail.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 160);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Port Number";
            // 
            // txtPortNumber
            // 
            this.txtPortNumber.Location = new System.Drawing.Point(3, 181);
            this.txtPortNumber.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPortNumber.Name = "txtPortNumber";
            this.txtPortNumber.ReadOnly = true;
            this.txtPortNumber.Size = new System.Drawing.Size(384, 22);
            this.txtPortNumber.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 210);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Use Default Credentials";
            // 
            // txtDefaultCredentials
            // 
            this.txtDefaultCredentials.Location = new System.Drawing.Point(3, 231);
            this.txtDefaultCredentials.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDefaultCredentials.Name = "txtDefaultCredentials";
            this.txtDefaultCredentials.ReadOnly = true;
            this.txtDefaultCredentials.Size = new System.Drawing.Size(384, 22);
            this.txtDefaultCredentials.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 261);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Client Host";
            // 
            // txtClientHost
            // 
            this.txtClientHost.Location = new System.Drawing.Point(3, 282);
            this.txtClientHost.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtClientHost.Name = "txtClientHost";
            this.txtClientHost.ReadOnly = true;
            this.txtClientHost.Size = new System.Drawing.Size(384, 22);
            this.txtClientHost.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-1, 311);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(365, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "To Email [AppSettings: Jobs-NotificationEmailAddresses]";
            // 
            // txtTestEmailAddr
            // 
            this.txtTestEmailAddr.Location = new System.Drawing.Point(3, 332);
            this.txtTestEmailAddr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTestEmailAddr.Name = "txtTestEmailAddr";
            this.txtTestEmailAddr.ReadOnly = true;
            this.txtTestEmailAddr.Size = new System.Drawing.Size(384, 22);
            this.txtTestEmailAddr.TabIndex = 12;
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.Location = new System.Drawing.Point(4, 359);
            this.btnSendEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(100, 28);
            this.btnSendEmail.TabIndex = 14;
            this.btnSendEmail.Text = "Send Email";
            this.btnSendEmail.UseVisualStyleBackColor = true;
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            // 
            // btnSmsTestForm
            // 
            this.btnSmsTestForm.Location = new System.Drawing.Point(244, 362);
            this.btnSmsTestForm.Name = "btnSmsTestForm";
            this.btnSmsTestForm.Size = new System.Drawing.Size(143, 26);
            this.btnSmsTestForm.TabIndex = 15;
            this.btnSmsTestForm.Text = "SMS Test Form";
            this.btnSmsTestForm.UseVisualStyleBackColor = true;
            this.btnSmsTestForm.Click += new System.EventHandler(this.btnSmsTestForm_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 400);
            this.Controls.Add(this.btnSmsTestForm);
            this.Controls.Add(this.btnSendEmail);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTestEmailAddr);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtClientHost);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDefaultCredentials);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPortNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBccEmail);
            this.Controls.Add(this.lblReplyTo);
            this.Controls.Add(this.txtReplyToEmail);
            this.Controls.Add(this.lblFromEmail);
            this.Controls.Add(this.txtFromEmail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Notifications Core Library UI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFromEmail;
        private System.Windows.Forms.Label lblFromEmail;
        private System.Windows.Forms.Label lblReplyTo;
        private System.Windows.Forms.TextBox txtReplyToEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBccEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPortNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDefaultCredentials;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtClientHost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTestEmailAddr;
        private System.Windows.Forms.Button btnSendEmail;
        private System.Windows.Forms.Button btnSmsTestForm;
    }
}

