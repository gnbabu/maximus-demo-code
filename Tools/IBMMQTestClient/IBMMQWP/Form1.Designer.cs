namespace IBMMQWP
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
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblErrorMsg = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblReadMsg = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblMsgRead = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblReadRestuls = new System.Windows.Forms.Label();
            this.btnReadMsg = new System.Windows.Forms.Button();
            this.lblQueueToRecieveName = new System.Windows.Forms.Label();
            this.txtGETQueueName = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblWriteResults = new System.Windows.Forms.Label();
            this.btnWriteMsg = new System.Windows.Forms.Button();
            this.txtPutMsg = new System.Windows.Forms.TextBox();
            this.txtPUTQueueName = new System.Windows.Forms.TextBox();
            this.lblPUTQueueName = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label24 = new System.Windows.Forms.Label();
            this.cbTransportProperty = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtCipherName = new System.Windows.Forms.TextBox();
            this.txtCertificateName = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtChannelName = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtQueueName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.lblConnect = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtQueueManagerName = new System.Windows.Forms.TextBox();
            this.lblQueueManagerName = new System.Windows.Forms.Label();
            this.cbCertificateStore = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox1.Controls.Add(this.lblErrorMsg);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(-16384, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.groupBox1.Size = new System.Drawing.Size(0, 357);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Properties";
            // 
            // lblErrorMsg
            // 
            this.lblErrorMsg.AutoSize = true;
            this.lblErrorMsg.Location = new System.Drawing.Point(-16, 300);
            this.lblErrorMsg.Margin = new System.Windows.Forms.Padding(0);
            this.lblErrorMsg.Name = "lblErrorMsg";
            this.lblErrorMsg.Size = new System.Drawing.Size(0, 17);
            this.lblErrorMsg.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(-16, 266);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(140, 17);
            this.label9.TabIndex = 19;
            this.label9.Text = "Connection Message";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-16, 146);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 17);
            this.label6.TabIndex = 18;
            this.label6.Text = "Certificate Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-16, 146);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 17);
            this.label7.TabIndex = 17;
            this.label7.Text = "Password";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(-16, 146);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "User Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-16, 32);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-16, 85);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Server Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-16, 85);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Channel Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-16, 85);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Queue Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-16, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Queue Manager Name";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(-16384, 380);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.groupBox2.Size = new System.Drawing.Size(0, 170);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Put Message";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(-16, 231);
            this.button2.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(0, 28);
            this.button2.TabIndex = 8;
            this.button2.Text = "Put Message";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(-16, 84);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(0, 17);
            this.label12.TabIndex = 6;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(-16, 84);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 17);
            this.label11.TabIndex = 5;
            this.label11.Text = "Put Results";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(-16, 39);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 17);
            this.label10.TabIndex = 3;
            this.label10.Text = "Message to Send";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox3.Controls.Add(this.lblReadMsg);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Location = new System.Drawing.Point(-16384, 578);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.groupBox3.Size = new System.Drawing.Size(0, 170);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Read Message";
            // 
            // lblReadMsg
            // 
            this.lblReadMsg.AutoSize = true;
            this.lblReadMsg.Location = new System.Drawing.Point(-16, 139);
            this.lblReadMsg.Margin = new System.Windows.Forms.Padding(0);
            this.lblReadMsg.Name = "lblReadMsg";
            this.lblReadMsg.Size = new System.Drawing.Size(103, 17);
            this.lblReadMsg.TabIndex = 11;
            this.lblReadMsg.Text = "Message Read";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(-16, 82);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 17);
            this.label13.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(-16, 82);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(93, 17);
            this.label14.TabIndex = 9;
            this.label14.Text = "Read Results";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox4.Controls.Add(this.lblMsgRead);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.lblReadRestuls);
            this.groupBox4.Controls.Add(this.btnReadMsg);
            this.groupBox4.Controls.Add(this.lblQueueToRecieveName);
            this.groupBox4.Controls.Add(this.txtGETQueueName);
            this.groupBox4.Location = new System.Drawing.Point(49, 572);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(603, 196);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Read Message";
            // 
            // lblMsgRead
            // 
            this.lblMsgRead.AutoSize = true;
            this.lblMsgRead.Location = new System.Drawing.Point(197, 71);
            this.lblMsgRead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMsgRead.Name = "lblMsgRead";
            this.lblMsgRead.Size = new System.Drawing.Size(65, 17);
            this.lblMsgRead.TabIndex = 27;
            this.lblMsgRead.Text = "Message";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(24, 71);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(103, 17);
            this.label15.TabIndex = 26;
            this.label15.Text = "Message Read";
            // 
            // lblReadRestuls
            // 
            this.lblReadRestuls.AutoSize = true;
            this.lblReadRestuls.Location = new System.Drawing.Point(24, 151);
            this.lblReadRestuls.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReadRestuls.Name = "lblReadRestuls";
            this.lblReadRestuls.Size = new System.Drawing.Size(93, 17);
            this.lblReadRestuls.TabIndex = 25;
            this.lblReadRestuls.Text = "Read Results";
            // 
            // btnReadMsg
            // 
            this.btnReadMsg.Enabled = false;
            this.btnReadMsg.Location = new System.Drawing.Point(11, 108);
            this.btnReadMsg.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadMsg.Name = "btnReadMsg";
            this.btnReadMsg.Size = new System.Drawing.Size(584, 28);
            this.btnReadMsg.TabIndex = 24;
            this.btnReadMsg.Text = "Read Message";
            this.btnReadMsg.UseVisualStyleBackColor = true;
            this.btnReadMsg.Click += new System.EventHandler(this.btnReadMsg_Click);
            // 
            // lblQueueToRecieveName
            // 
            this.lblQueueToRecieveName.AutoSize = true;
            this.lblQueueToRecieveName.Location = new System.Drawing.Point(24, 34);
            this.lblQueueToRecieveName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQueueToRecieveName.Name = "lblQueueToRecieveName";
            this.lblQueueToRecieveName.Size = new System.Drawing.Size(168, 17);
            this.lblQueueToRecieveName.TabIndex = 21;
            this.lblQueueToRecieveName.Text = "Queue To Recieve Name";
            // 
            // txtGETQueueName
            // 
            this.txtGETQueueName.Location = new System.Drawing.Point(200, 28);
            this.txtGETQueueName.Margin = new System.Windows.Forms.Padding(4);
            this.txtGETQueueName.Name = "txtGETQueueName";
            this.txtGETQueueName.Size = new System.Drawing.Size(393, 22);
            this.txtGETQueueName.TabIndex = 22;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox5.Controls.Add(this.lblWriteResults);
            this.groupBox5.Controls.Add(this.btnWriteMsg);
            this.groupBox5.Controls.Add(this.txtPutMsg);
            this.groupBox5.Controls.Add(this.txtPUTQueueName);
            this.groupBox5.Controls.Add(this.lblPUTQueueName);
            this.groupBox5.Location = new System.Drawing.Point(48, 324);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(603, 222);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Put Message";
            // 
            // lblWriteResults
            // 
            this.lblWriteResults.AutoSize = true;
            this.lblWriteResults.Location = new System.Drawing.Point(27, 181);
            this.lblWriteResults.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWriteResults.Name = "lblWriteResults";
            this.lblWriteResults.Size = new System.Drawing.Size(92, 17);
            this.lblWriteResults.TabIndex = 20;
            this.lblWriteResults.Text = "Write Results";
            // 
            // btnWriteMsg
            // 
            this.btnWriteMsg.Enabled = false;
            this.btnWriteMsg.Location = new System.Drawing.Point(11, 130);
            this.btnWriteMsg.Margin = new System.Windows.Forms.Padding(4);
            this.btnWriteMsg.Name = "btnWriteMsg";
            this.btnWriteMsg.Size = new System.Drawing.Size(584, 28);
            this.btnWriteMsg.TabIndex = 19;
            this.btnWriteMsg.Text = "Write Message";
            this.btnWriteMsg.UseVisualStyleBackColor = true;
            this.btnWriteMsg.Click += new System.EventHandler(this.btnPutMsg_Click);
            // 
            // txtPutMsg
            // 
            this.txtPutMsg.Location = new System.Drawing.Point(3, 78);
            this.txtPutMsg.Margin = new System.Windows.Forms.Padding(4);
            this.txtPutMsg.Name = "txtPutMsg";
            this.txtPutMsg.Size = new System.Drawing.Size(593, 22);
            this.txtPutMsg.TabIndex = 18;
            // 
            // txtPUTQueueName
            // 
            this.txtPUTQueueName.Location = new System.Drawing.Point(211, 30);
            this.txtPUTQueueName.Margin = new System.Windows.Forms.Padding(4);
            this.txtPUTQueueName.Name = "txtPUTQueueName";
            this.txtPUTQueueName.Size = new System.Drawing.Size(384, 22);
            this.txtPUTQueueName.TabIndex = 17;
            // 
            // lblPUTQueueName
            // 
            this.lblPUTQueueName.AutoSize = true;
            this.lblPUTQueueName.Location = new System.Drawing.Point(17, 36);
            this.lblPUTQueueName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPUTQueueName.Name = "lblPUTQueueName";
            this.lblPUTQueueName.Size = new System.Drawing.Size(138, 17);
            this.lblPUTQueueName.TabIndex = 16;
            this.lblPUTQueueName.Text = "QueueToSendName";
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox6.Controls.Add(this.label25);
            this.groupBox6.Controls.Add(this.cbCertificateStore);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Controls.Add(this.cbTransportProperty);
            this.groupBox6.Controls.Add(this.label23);
            this.groupBox6.Controls.Add(this.txtCipherName);
            this.groupBox6.Controls.Add(this.txtCertificateName);
            this.groupBox6.Controls.Add(this.label22);
            this.groupBox6.Controls.Add(this.txtChannelName);
            this.groupBox6.Controls.Add(this.label21);
            this.groupBox6.Controls.Add(this.txtPort);
            this.groupBox6.Controls.Add(this.label19);
            this.groupBox6.Controls.Add(this.txtServerName);
            this.groupBox6.Controls.Add(this.label20);
            this.groupBox6.Controls.Add(this.txtPassword);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.txtUserName);
            this.groupBox6.Controls.Add(this.label18);
            this.groupBox6.Controls.Add(this.txtQueueName);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.lblConnect);
            this.groupBox6.Controls.Add(this.btnConnect);
            this.groupBox6.Controls.Add(this.txtQueueManagerName);
            this.groupBox6.Controls.Add(this.lblQueueManagerName);
            this.groupBox6.Location = new System.Drawing.Point(47, 13);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox6.Size = new System.Drawing.Size(605, 303);
            this.groupBox6.TabIndex = 22;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Connect";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(10, 162);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(128, 17);
            this.label24.TabIndex = 24;
            this.label24.Text = "Transport Property";
            // 
            // cbTransportProperty
            // 
            this.cbTransportProperty.FormattingEnabled = true;
            this.cbTransportProperty.Items.AddRange(new object[] {
            "Managed Client",
            "MQSeries"});
            this.cbTransportProperty.Location = new System.Drawing.Point(11, 181);
            this.cbTransportProperty.Margin = new System.Windows.Forms.Padding(1);
            this.cbTransportProperty.Name = "cbTransportProperty";
            this.cbTransportProperty.Size = new System.Drawing.Size(189, 24);
            this.cbTransportProperty.TabIndex = 23;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(235, 113);
            this.label23.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(90, 17);
            this.label23.TabIndex = 21;
            this.label23.Text = "Cipher Name";
            // 
            // txtCipherName
            // 
            this.txtCipherName.Location = new System.Drawing.Point(237, 131);
            this.txtCipherName.Margin = new System.Windows.Forms.Padding(1);
            this.txtCipherName.Name = "txtCipherName";
            this.txtCipherName.Size = new System.Drawing.Size(358, 22);
            this.txtCipherName.TabIndex = 20;
            // 
            // txtCertificateName
            // 
            this.txtCertificateName.Location = new System.Drawing.Point(237, 204);
            this.txtCertificateName.Margin = new System.Windows.Forms.Padding(4);
            this.txtCertificateName.Name = "txtCertificateName";
            this.txtCertificateName.Size = new System.Drawing.Size(360, 22);
            this.txtCertificateName.TabIndex = 19;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(234, 184);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(112, 17);
            this.label22.TabIndex = 18;
            this.label22.Text = "Certificate Name";
            // 
            // txtChannelName
            // 
            this.txtChannelName.Location = new System.Drawing.Point(9, 137);
            this.txtChannelName.Margin = new System.Windows.Forms.Padding(4);
            this.txtChannelName.Name = "txtChannelName";
            this.txtChannelName.Size = new System.Drawing.Size(192, 22);
            this.txtChannelName.TabIndex = 17;
            this.txtChannelName.TextChanged += new System.EventHandler(this.txtChannelName_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(8, 116);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(101, 17);
            this.label21.TabIndex = 16;
            this.label21.Text = "Channel Name";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(413, 87);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(185, 22);
            this.txtPort.TabIndex = 15;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(419, 68);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(88, 17);
            this.label19.TabIndex = 14;
            this.label19.Text = "Port Number";
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(412, 41);
            this.txtServerName.Margin = new System.Windows.Forms.Padding(4);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(185, 22);
            this.txtServerName.TabIndex = 13;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(419, 20);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(91, 17);
            this.label20.TabIndex = 12;
            this.label20.Text = "Server Name";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(237, 87);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(168, 22);
            this.txtPassword.TabIndex = 11;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(235, 68);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(110, 17);
            this.label17.TabIndex = 10;
            this.label17.Text = "Password Name";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(237, 41);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(167, 22);
            this.txtUserName.TabIndex = 9;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(239, 20);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(79, 17);
            this.label18.TabIndex = 8;
            this.label18.Text = "User Name";
            // 
            // txtQueueName
            // 
            this.txtQueueName.Location = new System.Drawing.Point(9, 88);
            this.txtQueueName.Margin = new System.Windows.Forms.Padding(4);
            this.txtQueueName.Name = "txtQueueName";
            this.txtQueueName.Size = new System.Drawing.Size(197, 22);
            this.txtQueueName.TabIndex = 7;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(8, 67);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(92, 17);
            this.label16.TabIndex = 6;
            this.label16.Text = "Queue Name";
            // 
            // lblConnect
            // 
            this.lblConnect.AutoSize = true;
            this.lblConnect.Location = new System.Drawing.Point(10, 266);
            this.lblConnect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnect.Name = "lblConnect";
            this.lblConnect.Size = new System.Drawing.Size(111, 17);
            this.lblConnect.TabIndex = 5;
            this.lblConnect.Text = "Connect Results";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(9, 234);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(589, 28);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtQueueManagerName
            // 
            this.txtQueueManagerName.Location = new System.Drawing.Point(9, 41);
            this.txtQueueManagerName.Margin = new System.Windows.Forms.Padding(4);
            this.txtQueueManagerName.Name = "txtQueueManagerName";
            this.txtQueueManagerName.Size = new System.Drawing.Size(214, 22);
            this.txtQueueManagerName.TabIndex = 1;
            // 
            // lblQueueManagerName
            // 
            this.lblQueueManagerName.AutoSize = true;
            this.lblQueueManagerName.Location = new System.Drawing.Point(8, 20);
            this.lblQueueManagerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQueueManagerName.Name = "lblQueueManagerName";
            this.lblQueueManagerName.Size = new System.Drawing.Size(152, 17);
            this.lblQueueManagerName.TabIndex = 0;
            this.lblQueueManagerName.Text = "Queue Manager Name";
            // 
            // cbCertificateStore
            // 
            this.cbCertificateStore.FormattingEnabled = true;
            this.cbCertificateStore.Items.AddRange(new object[] {
            "USER",
            "SYSTEM"});
            this.cbCertificateStore.Location = new System.Drawing.Point(352, 157);
            this.cbCertificateStore.Name = "cbCertificateStore";
            this.cbCertificateStore.Size = new System.Drawing.Size(172, 24);
            this.cbCertificateStore.TabIndex = 25;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(234, 162);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(109, 17);
            this.label25.TabIndex = 26;
            this.label25.Text = "Certificate Store";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 782);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.Name = "Form1";
            this.Text = "IBM Websphere Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblErrorMsg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblReadMsg;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblReadRestuls;
        private System.Windows.Forms.Button btnReadMsg;
        private System.Windows.Forms.Label lblQueueToRecieveName;
        private System.Windows.Forms.TextBox txtGETQueueName;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lblConnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtQueueManagerName;
        private System.Windows.Forms.Label lblQueueManagerName;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblWriteResults;
        private System.Windows.Forms.Button btnWriteMsg;
        private System.Windows.Forms.TextBox txtPutMsg;
        private System.Windows.Forms.TextBox txtPUTQueueName;
        private System.Windows.Forms.Label lblPUTQueueName;
        private System.Windows.Forms.Label lblMsgRead;
        private System.Windows.Forms.TextBox txtQueueName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtChannelName;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtCertificateName;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtCipherName;
        private System.Windows.Forms.ComboBox cbTransportProperty;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.ComboBox cbCertificateStore;
    }
}

