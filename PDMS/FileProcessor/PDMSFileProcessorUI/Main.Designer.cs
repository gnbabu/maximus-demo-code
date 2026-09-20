namespace PDMSFileProcessorUI
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
			this.lblTestCSVFile = new System.Windows.Forms.Label();
			this.txtTestCSVFile = new System.Windows.Forms.TextBox();
			this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.btnBrowseTestCSVFile = new System.Windows.Forms.Button();
			this.btnImportTestCSVFile = new System.Windows.Forms.Button();
			this.txtLog = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lblTestCSVFile
			// 
			this.lblTestCSVFile.AutoSize = true;
			this.lblTestCSVFile.Location = new System.Drawing.Point(12, 32);
			this.lblTestCSVFile.Name = "lblTestCSVFile";
			this.lblTestCSVFile.Size = new System.Drawing.Size(74, 13);
			this.lblTestCSVFile.TabIndex = 0;
			this.lblTestCSVFile.Text = "Test CSV File:";
			// 
			// txtTestCSVFile
			// 
			this.txtTestCSVFile.Location = new System.Drawing.Point(92, 29);
			this.txtTestCSVFile.Name = "txtTestCSVFile";
			this.txtTestCSVFile.ReadOnly = true;
			this.txtTestCSVFile.Size = new System.Drawing.Size(349, 20);
			this.txtTestCSVFile.TabIndex = 1;
			// 
			// dlgOpenFile
			// 
			this.dlgOpenFile.DefaultExt = "csv";
			this.dlgOpenFile.FileName = "openFileDialog1";
			// 
			// btnBrowseTestCSVFile
			// 
			this.btnBrowseTestCSVFile.Location = new System.Drawing.Point(447, 27);
			this.btnBrowseTestCSVFile.Name = "btnBrowseTestCSVFile";
			this.btnBrowseTestCSVFile.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseTestCSVFile.TabIndex = 2;
			this.btnBrowseTestCSVFile.Text = "Browse...";
			this.btnBrowseTestCSVFile.UseVisualStyleBackColor = true;
			this.btnBrowseTestCSVFile.Click += new System.EventHandler(this.btnBrowseTestCSVFile_Click);
			// 
			// btnImportTestCSVFile
			// 
			this.btnImportTestCSVFile.Location = new System.Drawing.Point(15, 55);
			this.btnImportTestCSVFile.Name = "btnImportTestCSVFile";
			this.btnImportTestCSVFile.Size = new System.Drawing.Size(75, 23);
			this.btnImportTestCSVFile.TabIndex = 3;
			this.btnImportTestCSVFile.Text = "Import";
			this.btnImportTestCSVFile.UseVisualStyleBackColor = true;
			this.btnImportTestCSVFile.Click += new System.EventHandler(this.btnImportTestCSVFile_Click);
			// 
			// txtLog
			// 
			this.txtLog.Location = new System.Drawing.Point(15, 213);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtLog.Size = new System.Drawing.Size(786, 174);
			this.txtLog.TabIndex = 4;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(889, 399);
			this.Controls.Add(this.txtLog);
			this.Controls.Add(this.btnImportTestCSVFile);
			this.Controls.Add(this.btnBrowseTestCSVFile);
			this.Controls.Add(this.txtTestCSVFile);
			this.Controls.Add(this.lblTestCSVFile);
			this.Name = "Main";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblTestCSVFile;
		private System.Windows.Forms.TextBox txtTestCSVFile;
		private System.Windows.Forms.OpenFileDialog dlgOpenFile;
		private System.Windows.Forms.Button btnBrowseTestCSVFile;
		private System.Windows.Forms.Button btnImportTestCSVFile;
		private System.Windows.Forms.TextBox txtLog;
	}
}

