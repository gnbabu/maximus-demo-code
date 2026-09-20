namespace FileDownloaderUI
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
			this.tabConfig = new System.Windows.Forms.TabControl();
			this.tabDownloadFile = new System.Windows.Forms.TabPage();
			this.txtLog = new System.Windows.Forms.TextBox();
			this.lblProgressDetails = new System.Windows.Forms.Label();
			this.pbDownload = new System.Windows.Forms.ProgressBar();
			this.btnDownload = new System.Windows.Forms.Button();
			this.btnBrowseFile = new System.Windows.Forms.Button();
			this.txtDestination = new System.Windows.Forms.TextBox();
			this.lblDestinationFilename = new System.Windows.Forms.Label();
			this.txtUri = new System.Windows.Forms.TextBox();
			this.lblUrl = new System.Windows.Forms.Label();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.lblTestDate = new System.Windows.Forms.Label();
			this.dtTestDate = new System.Windows.Forms.DateTimePicker();
			this.btnDeleteTask = new System.Windows.Forms.Button();
			this.btnAddTask = new System.Windows.Forms.Button();
			this.gbTask = new System.Windows.Forms.GroupBox();
			this.txtSeedStartValue = new System.Windows.Forms.TextBox();
			this.lblSeedStartValue = new System.Windows.Forms.Label();
			this.txtSeedIncrement = new System.Windows.Forms.TextBox();
			this.lblSeedIncrement = new System.Windows.Forms.Label();
			this.dtSeedStart = new System.Windows.Forms.DateTimePicker();
			this.lblSeedIndex = new System.Windows.Forms.Label();
			this.btnDownloadTestUrl = new System.Windows.Forms.Button();
			this.txtTestUrlValue = new System.Windows.Forms.TextBox();
			this.lblTestUrlValue = new System.Windows.Forms.Label();
			this.txtTaskUrlPattern = new System.Windows.Forms.TextBox();
			this.txtTaskName = new System.Windows.Forms.TextBox();
			this.lblTaskUrlPattern = new System.Windows.Forms.Label();
			this.lblTaskName = new System.Windows.Forms.Label();
			this.lblTasks = new System.Windows.Forms.Label();
			this.lbDownloadTasks = new System.Windows.Forms.ListBox();
			this.btnSaveConfig = new System.Windows.Forms.Button();
			this.btnOpenConfig = new System.Windows.Forms.Button();
			this.dlgSaveFolder = new System.Windows.Forms.FolderBrowserDialog();
			this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
			this.txtYearOffset = new System.Windows.Forms.TextBox();
			this.txtMonthOffset = new System.Windows.Forms.TextBox();
			this.txtDayOffset = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tabConfig.SuspendLayout();
			this.tabDownloadFile.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.gbTask.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabConfig
			// 
			this.tabConfig.Controls.Add(this.tabDownloadFile);
			this.tabConfig.Controls.Add(this.tabPage1);
			this.tabConfig.Location = new System.Drawing.Point(12, 12);
			this.tabConfig.Name = "tabConfig";
			this.tabConfig.SelectedIndex = 0;
			this.tabConfig.Size = new System.Drawing.Size(882, 333);
			this.tabConfig.TabIndex = 0;
			// 
			// tabDownloadFile
			// 
			this.tabDownloadFile.Controls.Add(this.txtLog);
			this.tabDownloadFile.Controls.Add(this.lblProgressDetails);
			this.tabDownloadFile.Controls.Add(this.pbDownload);
			this.tabDownloadFile.Controls.Add(this.btnDownload);
			this.tabDownloadFile.Controls.Add(this.btnBrowseFile);
			this.tabDownloadFile.Controls.Add(this.txtDestination);
			this.tabDownloadFile.Controls.Add(this.lblDestinationFilename);
			this.tabDownloadFile.Controls.Add(this.txtUri);
			this.tabDownloadFile.Controls.Add(this.lblUrl);
			this.tabDownloadFile.Location = new System.Drawing.Point(4, 22);
			this.tabDownloadFile.Name = "tabDownloadFile";
			this.tabDownloadFile.Padding = new System.Windows.Forms.Padding(3);
			this.tabDownloadFile.Size = new System.Drawing.Size(874, 307);
			this.tabDownloadFile.TabIndex = 0;
			this.tabDownloadFile.Text = "Download File";
			this.tabDownloadFile.UseVisualStyleBackColor = true;
			// 
			// txtLog
			// 
			this.txtLog.Location = new System.Drawing.Point(6, 181);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtLog.Size = new System.Drawing.Size(755, 110);
			this.txtLog.TabIndex = 8;
			// 
			// lblProgressDetails
			// 
			this.lblProgressDetails.AutoSize = true;
			this.lblProgressDetails.Location = new System.Drawing.Point(129, 99);
			this.lblProgressDetails.Name = "lblProgressDetails";
			this.lblProgressDetails.Size = new System.Drawing.Size(0, 13);
			this.lblProgressDetails.TabIndex = 7;
			// 
			// pbDownload
			// 
			this.pbDownload.Location = new System.Drawing.Point(6, 142);
			this.pbDownload.Name = "pbDownload";
			this.pbDownload.Size = new System.Drawing.Size(755, 23);
			this.pbDownload.TabIndex = 6;
			// 
			// btnDownload
			// 
			this.btnDownload.Location = new System.Drawing.Point(12, 87);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new System.Drawing.Size(99, 30);
			this.btnDownload.TabIndex = 5;
			this.btnDownload.Text = "Download";
			this.btnDownload.UseVisualStyleBackColor = true;
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// btnBrowseFile
			// 
			this.btnBrowseFile.Location = new System.Drawing.Point(682, 41);
			this.btnBrowseFile.Name = "btnBrowseFile";
			this.btnBrowseFile.Size = new System.Drawing.Size(79, 24);
			this.btnBrowseFile.TabIndex = 4;
			this.btnBrowseFile.Text = "Browse...";
			this.btnBrowseFile.UseVisualStyleBackColor = true;
			this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
			// 
			// txtDestination
			// 
			this.txtDestination.Location = new System.Drawing.Point(75, 44);
			this.txtDestination.Name = "txtDestination";
			this.txtDestination.ReadOnly = true;
			this.txtDestination.Size = new System.Drawing.Size(595, 20);
			this.txtDestination.TabIndex = 3;
			// 
			// lblDestinationFilename
			// 
			this.lblDestinationFilename.AutoSize = true;
			this.lblDestinationFilename.Location = new System.Drawing.Point(3, 47);
			this.lblDestinationFilename.Name = "lblDestinationFilename";
			this.lblDestinationFilename.Size = new System.Drawing.Size(66, 13);
			this.lblDestinationFilename.TabIndex = 2;
			this.lblDestinationFilename.Text = "Destination: ";
			// 
			// txtUri
			// 
			this.txtUri.Location = new System.Drawing.Point(41, 14);
			this.txtUri.Name = "txtUri";
			this.txtUri.Size = new System.Drawing.Size(720, 20);
			this.txtUri.TabIndex = 1;
			this.txtUri.Text = "http://oig.hhs.gov/exclusions/downloadables/2014/sanc1404.zip";
			// 
			// lblUrl
			// 
			this.lblUrl.AutoSize = true;
			this.lblUrl.Location = new System.Drawing.Point(3, 17);
			this.lblUrl.Name = "lblUrl";
			this.lblUrl.Size = new System.Drawing.Size(32, 13);
			this.lblUrl.TabIndex = 0;
			this.lblUrl.Text = "URL:";
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.lblTestDate);
			this.tabPage1.Controls.Add(this.dtTestDate);
			this.tabPage1.Controls.Add(this.btnDeleteTask);
			this.tabPage1.Controls.Add(this.btnAddTask);
			this.tabPage1.Controls.Add(this.gbTask);
			this.tabPage1.Controls.Add(this.lblTasks);
			this.tabPage1.Controls.Add(this.lbDownloadTasks);
			this.tabPage1.Controls.Add(this.btnSaveConfig);
			this.tabPage1.Controls.Add(this.btnOpenConfig);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(874, 307);
			this.tabPage1.TabIndex = 1;
			this.tabPage1.Text = "Configuration";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// lblTestDate
			// 
			this.lblTestDate.AutoSize = true;
			this.lblTestDate.Location = new System.Drawing.Point(52, 216);
			this.lblTestDate.Name = "lblTestDate";
			this.lblTestDate.Size = new System.Drawing.Size(57, 13);
			this.lblTestDate.TabIndex = 10;
			this.lblTestDate.Text = "Test Date:";
			// 
			// dtTestDate
			// 
			this.dtTestDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtTestDate.Location = new System.Drawing.Point(115, 213);
			this.dtTestDate.Name = "dtTestDate";
			this.dtTestDate.Size = new System.Drawing.Size(100, 20);
			this.dtTestDate.TabIndex = 9;
			// 
			// btnDeleteTask
			// 
			this.btnDeleteTask.Enabled = false;
			this.btnDeleteTask.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDeleteTask.Location = new System.Drawing.Point(235, 205);
			this.btnDeleteTask.Name = "btnDeleteTask";
			this.btnDeleteTask.Size = new System.Drawing.Size(30, 34);
			this.btnDeleteTask.TabIndex = 6;
			this.btnDeleteTask.Text = "-";
			this.btnDeleteTask.UseVisualStyleBackColor = true;
			this.btnDeleteTask.Click += new System.EventHandler(this.btnDeleteTask_Click);
			// 
			// btnAddTask
			// 
			this.btnAddTask.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAddTask.Location = new System.Drawing.Point(6, 205);
			this.btnAddTask.Name = "btnAddTask";
			this.btnAddTask.Size = new System.Drawing.Size(30, 34);
			this.btnAddTask.TabIndex = 5;
			this.btnAddTask.Text = "+";
			this.btnAddTask.UseVisualStyleBackColor = true;
			this.btnAddTask.Click += new System.EventHandler(this.btnAddTask_Click);
			// 
			// gbTask
			// 
			this.gbTask.Controls.Add(this.label3);
			this.gbTask.Controls.Add(this.label2);
			this.gbTask.Controls.Add(this.label1);
			this.gbTask.Controls.Add(this.txtDayOffset);
			this.gbTask.Controls.Add(this.txtMonthOffset);
			this.gbTask.Controls.Add(this.txtYearOffset);
			this.gbTask.Controls.Add(this.txtSeedStartValue);
			this.gbTask.Controls.Add(this.lblSeedStartValue);
			this.gbTask.Controls.Add(this.txtSeedIncrement);
			this.gbTask.Controls.Add(this.lblSeedIncrement);
			this.gbTask.Controls.Add(this.dtSeedStart);
			this.gbTask.Controls.Add(this.lblSeedIndex);
			this.gbTask.Controls.Add(this.btnDownloadTestUrl);
			this.gbTask.Controls.Add(this.txtTestUrlValue);
			this.gbTask.Controls.Add(this.lblTestUrlValue);
			this.gbTask.Controls.Add(this.txtTaskUrlPattern);
			this.gbTask.Controls.Add(this.txtTaskName);
			this.gbTask.Controls.Add(this.lblTaskUrlPattern);
			this.gbTask.Controls.Add(this.lblTaskName);
			this.gbTask.Location = new System.Drawing.Point(271, 44);
			this.gbTask.Name = "gbTask";
			this.gbTask.Size = new System.Drawing.Size(588, 209);
			this.gbTask.TabIndex = 4;
			this.gbTask.TabStop = false;
			this.gbTask.Text = "Task Details";
			// 
			// txtSeedStartValue
			// 
			this.txtSeedStartValue.Location = new System.Drawing.Point(269, 84);
			this.txtSeedStartValue.Name = "txtSeedStartValue";
			this.txtSeedStartValue.Size = new System.Drawing.Size(68, 20);
			this.txtSeedStartValue.TabIndex = 12;
			this.txtSeedStartValue.Leave += new System.EventHandler(this.txtSeedStartValue_Leave);
			// 
			// lblSeedStartValue
			// 
			this.lblSeedStartValue.AutoSize = true;
			this.lblSeedStartValue.Location = new System.Drawing.Point(201, 87);
			this.lblSeedStartValue.Name = "lblSeedStartValue";
			this.lblSeedStartValue.Size = new System.Drawing.Size(62, 13);
			this.lblSeedStartValue.TabIndex = 11;
			this.lblSeedStartValue.Text = "Start Value:";
			// 
			// txtSeedIncrement
			// 
			this.txtSeedIncrement.Location = new System.Drawing.Point(404, 84);
			this.txtSeedIncrement.Name = "txtSeedIncrement";
			this.txtSeedIncrement.Size = new System.Drawing.Size(54, 20);
			this.txtSeedIncrement.TabIndex = 10;
			this.txtSeedIncrement.Leave += new System.EventHandler(this.txtSeedIncrement_Leave);
			// 
			// lblSeedIncrement
			// 
			this.lblSeedIncrement.AutoSize = true;
			this.lblSeedIncrement.Location = new System.Drawing.Point(345, 87);
			this.lblSeedIncrement.Name = "lblSeedIncrement";
			this.lblSeedIncrement.Size = new System.Drawing.Size(57, 13);
			this.lblSeedIncrement.TabIndex = 9;
			this.lblSeedIncrement.Text = "Increment:";
			// 
			// dtSeedStart
			// 
			this.dtSeedStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtSeedStart.Location = new System.Drawing.Point(102, 84);
			this.dtSeedStart.Name = "dtSeedStart";
			this.dtSeedStart.Size = new System.Drawing.Size(87, 20);
			this.dtSeedStart.TabIndex = 8;
			this.dtSeedStart.Leave += new System.EventHandler(this.dtSeedStart_Leave);
			// 
			// lblSeedIndex
			// 
			this.lblSeedIndex.AutoSize = true;
			this.lblSeedIndex.Location = new System.Drawing.Point(10, 87);
			this.lblSeedIndex.Name = "lblSeedIndex";
			this.lblSeedIndex.Size = new System.Drawing.Size(86, 13);
			this.lblSeedIndex.TabIndex = 7;
			this.lblSeedIndex.Text = "Seed Start Date:";
			// 
			// btnDownloadTestUrl
			// 
			this.btnDownloadTestUrl.Location = new System.Drawing.Point(73, 170);
			this.btnDownloadTestUrl.Name = "btnDownloadTestUrl";
			this.btnDownloadTestUrl.Size = new System.Drawing.Size(116, 30);
			this.btnDownloadTestUrl.TabIndex = 6;
			this.btnDownloadTestUrl.Text = "Download";
			this.btnDownloadTestUrl.UseVisualStyleBackColor = true;
			this.btnDownloadTestUrl.Click += new System.EventHandler(this.btnDownloadTestUrl_Click);
			// 
			// txtTestUrlValue
			// 
			this.txtTestUrlValue.Location = new System.Drawing.Point(73, 144);
			this.txtTestUrlValue.Name = "txtTestUrlValue";
			this.txtTestUrlValue.ReadOnly = true;
			this.txtTestUrlValue.Size = new System.Drawing.Size(499, 20);
			this.txtTestUrlValue.TabIndex = 5;
			// 
			// lblTestUrlValue
			// 
			this.lblTestUrlValue.AutoSize = true;
			this.lblTestUrlValue.Location = new System.Drawing.Point(6, 147);
			this.lblTestUrlValue.Name = "lblTestUrlValue";
			this.lblTestUrlValue.Size = new System.Drawing.Size(61, 13);
			this.lblTestUrlValue.TabIndex = 4;
			this.lblTestUrlValue.Text = "Test Value:";
			// 
			// txtTaskUrlPattern
			// 
			this.txtTaskUrlPattern.Location = new System.Drawing.Point(73, 58);
			this.txtTaskUrlPattern.Name = "txtTaskUrlPattern";
			this.txtTaskUrlPattern.Size = new System.Drawing.Size(499, 20);
			this.txtTaskUrlPattern.TabIndex = 3;
			this.txtTaskUrlPattern.Leave += new System.EventHandler(this.txtTaskUrlPattern_Leave);
			// 
			// txtTaskName
			// 
			this.txtTaskName.Location = new System.Drawing.Point(73, 31);
			this.txtTaskName.Name = "txtTaskName";
			this.txtTaskName.Size = new System.Drawing.Size(499, 20);
			this.txtTaskName.TabIndex = 2;
			this.txtTaskName.Leave += new System.EventHandler(this.txtTaskName_Leave);
			// 
			// lblTaskUrlPattern
			// 
			this.lblTaskUrlPattern.AutoSize = true;
			this.lblTaskUrlPattern.Location = new System.Drawing.Point(7, 61);
			this.lblTaskUrlPattern.Name = "lblTaskUrlPattern";
			this.lblTaskUrlPattern.Size = new System.Drawing.Size(60, 13);
			this.lblTaskUrlPattern.TabIndex = 1;
			this.lblTaskUrlPattern.Text = "Url Pattern:";
			// 
			// lblTaskName
			// 
			this.lblTaskName.AutoSize = true;
			this.lblTaskName.Location = new System.Drawing.Point(6, 34);
			this.lblTaskName.Name = "lblTaskName";
			this.lblTaskName.Size = new System.Drawing.Size(38, 13);
			this.lblTaskName.TabIndex = 0;
			this.lblTaskName.Text = "Name:";
			// 
			// lblTasks
			// 
			this.lblTasks.AutoSize = true;
			this.lblTasks.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTasks.Location = new System.Drawing.Point(6, 49);
			this.lblTasks.Name = "lblTasks";
			this.lblTasks.Size = new System.Drawing.Size(87, 13);
			this.lblTasks.TabIndex = 3;
			this.lblTasks.Text = "Download Tasks";
			// 
			// lbDownloadTasks
			// 
			this.lbDownloadTasks.DisplayMember = "UrlPattern";
			this.lbDownloadTasks.FormattingEnabled = true;
			this.lbDownloadTasks.Location = new System.Drawing.Point(6, 65);
			this.lbDownloadTasks.Name = "lbDownloadTasks";
			this.lbDownloadTasks.Size = new System.Drawing.Size(259, 134);
			this.lbDownloadTasks.TabIndex = 2;
			this.lbDownloadTasks.SelectedIndexChanged += new System.EventHandler(this.lbDownloadTasks_SelectedIndexChanged);
			// 
			// btnSaveConfig
			// 
			this.btnSaveConfig.Location = new System.Drawing.Point(744, 9);
			this.btnSaveConfig.Name = "btnSaveConfig";
			this.btnSaveConfig.Size = new System.Drawing.Size(115, 29);
			this.btnSaveConfig.TabIndex = 1;
			this.btnSaveConfig.Text = "Save";
			this.btnSaveConfig.UseVisualStyleBackColor = true;
			this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
			// 
			// btnOpenConfig
			// 
			this.btnOpenConfig.Location = new System.Drawing.Point(6, 9);
			this.btnOpenConfig.Name = "btnOpenConfig";
			this.btnOpenConfig.Size = new System.Drawing.Size(115, 29);
			this.btnOpenConfig.TabIndex = 0;
			this.btnOpenConfig.Text = "Open";
			this.btnOpenConfig.UseVisualStyleBackColor = true;
			this.btnOpenConfig.Click += new System.EventHandler(this.btnOpenConfig_Click);
			// 
			// dlgOpenFile
			// 
			this.dlgOpenFile.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
			// 
			// dlgSaveFile
			// 
			this.dlgSaveFile.DefaultExt = "xml";
			this.dlgSaveFile.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
			// 
			// txtYearOffset
			// 
			this.txtYearOffset.Location = new System.Drawing.Point(102, 118);
			this.txtYearOffset.Name = "txtYearOffset";
			this.txtYearOffset.Size = new System.Drawing.Size(68, 20);
			this.txtYearOffset.TabIndex = 13;
			this.txtYearOffset.Leave += new System.EventHandler(this.txtYearOffset_Leave);
			// 
			// txtMonthOffset
			// 
			this.txtMonthOffset.Location = new System.Drawing.Point(269, 118);
			this.txtMonthOffset.Name = "txtMonthOffset";
			this.txtMonthOffset.Size = new System.Drawing.Size(68, 20);
			this.txtMonthOffset.TabIndex = 14;
			this.txtMonthOffset.Leave += new System.EventHandler(this.txtMonthOffset_Leave);
			// 
			// txtDayOffset
			// 
			this.txtDayOffset.Location = new System.Drawing.Point(404, 118);
			this.txtDayOffset.Name = "txtDayOffset";
			this.txtDayOffset.Size = new System.Drawing.Size(68, 20);
			this.txtDayOffset.TabIndex = 15;
			this.txtDayOffset.Leave += new System.EventHandler(this.txtDayOffset_Leave);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 118);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Year Offset:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(192, 121);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 13);
			this.label2.TabIndex = 17;
			this.label2.Text = "Month Offset:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(338, 121);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 13);
			this.label3.TabIndex = 18;
			this.label3.Text = "Day Offset:";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(906, 357);
			this.Controls.Add(this.tabConfig);
			this.Name = "Main";
			this.Text = "Form1";
			this.tabConfig.ResumeLayout(false);
			this.tabDownloadFile.ResumeLayout(false);
			this.tabDownloadFile.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.gbTask.ResumeLayout(false);
			this.gbTask.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabConfig;
		private System.Windows.Forms.TabPage tabDownloadFile;
		private System.Windows.Forms.Button btnBrowseFile;
		private System.Windows.Forms.TextBox txtDestination;
		private System.Windows.Forms.Label lblDestinationFilename;
		private System.Windows.Forms.TextBox txtUri;
		private System.Windows.Forms.Label lblUrl;
		private System.Windows.Forms.FolderBrowserDialog dlgSaveFolder;
		private System.Windows.Forms.ProgressBar pbDownload;
		private System.Windows.Forms.Button btnDownload;
		private System.Windows.Forms.Label lblProgressDetails;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button btnDeleteTask;
		private System.Windows.Forms.Button btnAddTask;
		private System.Windows.Forms.GroupBox gbTask;
		private System.Windows.Forms.Label lblTasks;
		private System.Windows.Forms.ListBox lbDownloadTasks;
		private System.Windows.Forms.Button btnSaveConfig;
		private System.Windows.Forms.Button btnOpenConfig;
		private System.Windows.Forms.OpenFileDialog dlgOpenFile;
		private System.Windows.Forms.SaveFileDialog dlgSaveFile;
		private System.Windows.Forms.TextBox txtTaskName;
		private System.Windows.Forms.Label lblTaskUrlPattern;
		private System.Windows.Forms.Label lblTaskName;
		private System.Windows.Forms.TextBox txtTestUrlValue;
		private System.Windows.Forms.Label lblTestUrlValue;
		private System.Windows.Forms.TextBox txtTaskUrlPattern;
		private System.Windows.Forms.Button btnDownloadTestUrl;
		private System.Windows.Forms.TextBox txtSeedStartValue;
		private System.Windows.Forms.Label lblSeedStartValue;
		private System.Windows.Forms.TextBox txtSeedIncrement;
		private System.Windows.Forms.Label lblSeedIncrement;
		private System.Windows.Forms.DateTimePicker dtSeedStart;
		private System.Windows.Forms.Label lblSeedIndex;
		private System.Windows.Forms.Label lblTestDate;
		private System.Windows.Forms.DateTimePicker dtTestDate;
		private System.Windows.Forms.TextBox txtLog;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDayOffset;
		private System.Windows.Forms.TextBox txtMonthOffset;
		private System.Windows.Forms.TextBox txtYearOffset;
	}
}

