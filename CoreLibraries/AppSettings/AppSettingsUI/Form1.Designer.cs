namespace AppSettingsUI.csproj
{
    partial class frmAppSettings
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
            this.btnGetAppSetting = new System.Windows.Forms.Button();
            this.btnConnectString = new System.Windows.Forms.Button();
            this.txtGetSetting = new System.Windows.Forms.TextBox();
            this.txtReturnValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSendEmail = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGetAppSetting
            // 
            this.btnGetAppSetting.Location = new System.Drawing.Point(85, 38);
            this.btnGetAppSetting.Name = "btnGetAppSetting";
            this.btnGetAppSetting.Size = new System.Drawing.Size(120, 23);
            this.btnGetAppSetting.TabIndex = 0;
            this.btnGetAppSetting.Text = "Get App Setting";
            this.btnGetAppSetting.UseVisualStyleBackColor = true;
            this.btnGetAppSetting.Click += new System.EventHandler(this.btnGetAppSetting_Click);
            // 
            // btnConnectString
            // 
            this.btnConnectString.Location = new System.Drawing.Point(85, 100);
            this.btnConnectString.Name = "btnConnectString";
            this.btnConnectString.Size = new System.Drawing.Size(120, 23);
            this.btnConnectString.TabIndex = 1;
            this.btnConnectString.Text = "Get Connection String";
            this.btnConnectString.UseVisualStyleBackColor = true;
            this.btnConnectString.Click += new System.EventHandler(this.btnConnectString_Click);
            // 
            // txtGetSetting
            // 
            this.txtGetSetting.Location = new System.Drawing.Point(11, 12);
            this.txtGetSetting.Name = "txtGetSetting";
            this.txtGetSetting.Size = new System.Drawing.Size(268, 20);
            this.txtGetSetting.TabIndex = 2;
            // 
            // txtReturnValue
            // 
            this.txtReturnValue.Location = new System.Drawing.Point(12, 168);
            this.txtReturnValue.Multiline = true;
            this.txtReturnValue.Name = "txtReturnValue";
            this.txtReturnValue.Size = new System.Drawing.Size(268, 166);
            this.txtReturnValue.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(83, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "App Settings Return";
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.Location = new System.Drawing.Point(86, 70);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(120, 23);
            this.btnSendEmail.TabIndex = 5;
            this.btnSendEmail.Text = "Send Test Email";
            this.btnSendEmail.UseVisualStyleBackColor = true;
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            // 
            // frmAppSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 346);
            this.Controls.Add(this.btnSendEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtReturnValue);
            this.Controls.Add(this.txtGetSetting);
            this.Controls.Add(this.btnConnectString);
            this.Controls.Add(this.btnGetAppSetting);
            this.Name = "frmAppSettings";
            this.Text = "App Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetAppSetting;
        private System.Windows.Forms.Button btnConnectString;
        private System.Windows.Forms.TextBox txtGetSetting;
        private System.Windows.Forms.TextBox txtReturnValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSendEmail;
    }
}

