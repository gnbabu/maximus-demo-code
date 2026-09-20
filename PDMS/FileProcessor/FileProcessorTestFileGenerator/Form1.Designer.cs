namespace FileProcessorTestFileGenerator
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
			this.lblTemplateFolder = new System.Windows.Forms.Label();
			this.dlgFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.txtTemplateFolder = new System.Windows.Forms.TextBox();
			this.btnBrowseTemplateFolder = new System.Windows.Forms.Button();
			this.lstTemplateFiles = new System.Windows.Forms.ListBox();
			this.txtNumTestFiles = new System.Windows.Forms.TextBox();
			this.lblNumFiles = new System.Windows.Forms.Label();
			this.lblNumRecoreds = new System.Windows.Forms.Label();
			this.txtNumRecordsPerFile = new System.Windows.Forms.TextBox();
			this.btnBrowseDropFolder = new System.Windows.Forms.Button();
			this.txtDropFolder = new System.Windows.Forms.TextBox();
			this.lblDropFolder = new System.Windows.Forms.Label();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblTemplateFolder
			// 
			this.lblTemplateFolder.AutoSize = true;
			this.lblTemplateFolder.Location = new System.Drawing.Point(13, 18);
			this.lblTemplateFolder.Name = "lblTemplateFolder";
			this.lblTemplateFolder.Size = new System.Drawing.Size(83, 13);
			this.lblTemplateFolder.TabIndex = 0;
			this.lblTemplateFolder.Text = "Template Folder";
			// 
			// txtTemplateFolder
			// 
			this.txtTemplateFolder.Location = new System.Drawing.Point(102, 15);
			this.txtTemplateFolder.Name = "txtTemplateFolder";
			this.txtTemplateFolder.ReadOnly = true;
			this.txtTemplateFolder.Size = new System.Drawing.Size(378, 20);
			this.txtTemplateFolder.TabIndex = 1;
			// 
			// btnBrowseTemplateFolder
			// 
			this.btnBrowseTemplateFolder.Location = new System.Drawing.Point(503, 10);
			this.btnBrowseTemplateFolder.Name = "btnBrowseTemplateFolder";
			this.btnBrowseTemplateFolder.Size = new System.Drawing.Size(100, 29);
			this.btnBrowseTemplateFolder.TabIndex = 2;
			this.btnBrowseTemplateFolder.Text = "Browse...";
			this.btnBrowseTemplateFolder.UseVisualStyleBackColor = true;
			this.btnBrowseTemplateFolder.Click += new System.EventHandler(this.btnBrowseTemplateFolder_Click);
			// 
			// lstTemplateFiles
			// 
			this.lstTemplateFiles.FormattingEnabled = true;
			this.lstTemplateFiles.Location = new System.Drawing.Point(16, 87);
			this.lstTemplateFiles.Name = "lstTemplateFiles";
			this.lstTemplateFiles.Size = new System.Drawing.Size(242, 173);
			this.lstTemplateFiles.TabIndex = 4;
			// 
			// txtNumTestFiles
			// 
			this.txtNumTestFiles.Location = new System.Drawing.Point(367, 91);
			this.txtNumTestFiles.Name = "txtNumTestFiles";
			this.txtNumTestFiles.Size = new System.Drawing.Size(80, 20);
			this.txtNumTestFiles.TabIndex = 5;
			this.txtNumTestFiles.Text = "1";
			// 
			// lblNumFiles
			// 
			this.lblNumFiles.AutoSize = true;
			this.lblNumFiles.Location = new System.Drawing.Point(264, 94);
			this.lblNumFiles.Name = "lblNumFiles";
			this.lblNumFiles.Size = new System.Drawing.Size(50, 13);
			this.lblNumFiles.TabIndex = 6;
			this.lblNumFiles.Text = "# of Files";
			// 
			// lblNumRecoreds
			// 
			this.lblNumRecoreds.AutoSize = true;
			this.lblNumRecoreds.Location = new System.Drawing.Point(264, 122);
			this.lblNumRecoreds.Name = "lblNumRecoreds";
			this.lblNumRecoreds.Size = new System.Drawing.Size(94, 13);
			this.lblNumRecoreds.TabIndex = 7;
			this.lblNumRecoreds.Text = "# Records per File";
			// 
			// txtNumRecordsPerFile
			// 
			this.txtNumRecordsPerFile.Location = new System.Drawing.Point(367, 119);
			this.txtNumRecordsPerFile.Name = "txtNumRecordsPerFile";
			this.txtNumRecordsPerFile.Size = new System.Drawing.Size(80, 20);
			this.txtNumRecordsPerFile.TabIndex = 8;
			this.txtNumRecordsPerFile.Text = "1";
			// 
			// btnBrowseDropFolder
			// 
			this.btnBrowseDropFolder.Location = new System.Drawing.Point(503, 45);
			this.btnBrowseDropFolder.Name = "btnBrowseDropFolder";
			this.btnBrowseDropFolder.Size = new System.Drawing.Size(100, 29);
			this.btnBrowseDropFolder.TabIndex = 11;
			this.btnBrowseDropFolder.Text = "Browse...";
			this.btnBrowseDropFolder.UseVisualStyleBackColor = true;
			this.btnBrowseDropFolder.Click += new System.EventHandler(this.btnBrowseDropFolder_Click);
			// 
			// txtDropFolder
			// 
			this.txtDropFolder.Location = new System.Drawing.Point(102, 50);
			this.txtDropFolder.Name = "txtDropFolder";
			this.txtDropFolder.ReadOnly = true;
			this.txtDropFolder.Size = new System.Drawing.Size(378, 20);
			this.txtDropFolder.TabIndex = 10;
			// 
			// lblDropFolder
			// 
			this.lblDropFolder.AutoSize = true;
			this.lblDropFolder.Location = new System.Drawing.Point(13, 53);
			this.lblDropFolder.Name = "lblDropFolder";
			this.lblDropFolder.Size = new System.Drawing.Size(62, 13);
			this.lblDropFolder.TabIndex = 9;
			this.lblDropFolder.Text = "Drop Folder";
			// 
			// btnGenerate
			// 
			this.btnGenerate.Location = new System.Drawing.Point(354, 175);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(138, 34);
			this.btnGenerate.TabIndex = 12;
			this.btnGenerate.Text = "Generate";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(647, 301);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.btnBrowseDropFolder);
			this.Controls.Add(this.txtDropFolder);
			this.Controls.Add(this.lblDropFolder);
			this.Controls.Add(this.txtNumRecordsPerFile);
			this.Controls.Add(this.lblNumRecoreds);
			this.Controls.Add(this.lblNumFiles);
			this.Controls.Add(this.txtNumTestFiles);
			this.Controls.Add(this.lstTemplateFiles);
			this.Controls.Add(this.btnBrowseTemplateFolder);
			this.Controls.Add(this.txtTemplateFolder);
			this.Controls.Add(this.lblTemplateFolder);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblTemplateFolder;
		private System.Windows.Forms.FolderBrowserDialog dlgFolderBrowser;
		private System.Windows.Forms.TextBox txtTemplateFolder;
		private System.Windows.Forms.Button btnBrowseTemplateFolder;
		private System.Windows.Forms.ListBox lstTemplateFiles;
		private System.Windows.Forms.TextBox txtNumTestFiles;
		private System.Windows.Forms.Label lblNumFiles;
		private System.Windows.Forms.Label lblNumRecoreds;
		private System.Windows.Forms.TextBox txtNumRecordsPerFile;
		private System.Windows.Forms.Button btnBrowseDropFolder;
		private System.Windows.Forms.TextBox txtDropFolder;
		private System.Windows.Forms.Label lblDropFolder;
		private System.Windows.Forms.Button btnGenerate;
	}
}

