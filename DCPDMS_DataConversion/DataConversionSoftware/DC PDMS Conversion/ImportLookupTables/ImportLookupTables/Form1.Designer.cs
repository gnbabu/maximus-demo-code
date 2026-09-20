namespace ImportLookupTables
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
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnGO = new System.Windows.Forms.Button();
            this.dlgGetFilePath = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtImportFileExt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCreateObjects = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnDataFileFolder = new System.Windows.Forms.Button();
            this.txtDataFileFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMetaDataFile = new System.Windows.Forms.Button();
            this.txtMetaDataFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnLayoutSheetGO = new System.Windows.Forms.Button();
            this.btnBrowseForPDMS = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPDMSPath = new System.Windows.Forms.TextBox();
            this.btnBrowseForMMIS = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMMISPath = new System.Windows.Forms.TextBox();
            this.tpErrorReport = new System.Windows.Forms.TabPage();
            this.btnCreateErrorReport = new System.Windows.Forms.Button();
            this.btnBrowseErrReportOutput = new System.Windows.Forms.Button();
            this.txtOutputFileName = new System.Windows.Forms.TextBox();
            this.txtRunID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dlgFolderPicker = new System.Windows.Forms.FolderBrowserDialog();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSourceDatabase = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tpErrorReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInputFile
            // 
            this.txtInputFile.Location = new System.Drawing.Point(12, 38);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(691, 22);
            this.txtInputFile.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(709, 37);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnGO
            // 
            this.btnGO.Location = new System.Drawing.Point(358, 95);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(98, 23);
            this.btnGO.TabIndex = 2;
            this.btnGO.Text = "GO";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.btnGO_Click);
            // 
            // dlgGetFilePath
            // 
            this.dlgGetFilePath.FileName = "openFileDialog1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tpErrorReport);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(804, 379);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtImportFileExt);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.btnCreateObjects);
            this.tabPage2.Controls.Add(this.btnImport);
            this.tabPage2.Controls.Add(this.btnDataFileFolder);
            this.tabPage2.Controls.Add(this.txtDataFileFolder);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.btnMetaDataFile);
            this.tabPage2.Controls.Add(this.txtMetaDataFile);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(796, 350);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Import Data to Initial Staging Tables";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtImportFileExt
            // 
            this.txtImportFileExt.Location = new System.Drawing.Point(150, 95);
            this.txtImportFileExt.Name = "txtImportFileExt";
            this.txtImportFileExt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtImportFileExt.Size = new System.Drawing.Size(108, 22);
            this.txtImportFileExt.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Import File Extension";
            // 
            // btnCreateObjects
            // 
            this.btnCreateObjects.Location = new System.Drawing.Point(192, 317);
            this.btnCreateObjects.Name = "btnCreateObjects";
            this.btnCreateObjects.Size = new System.Drawing.Size(203, 23);
            this.btnCreateObjects.TabIndex = 7;
            this.btnCreateObjects.Text = "Create Objects";
            this.btnCreateObjects.UseVisualStyleBackColor = true;
            this.btnCreateObjects.Click += new System.EventHandler(this.btnCreateObjects_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(401, 317);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(203, 23);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import Data";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnDataFileFolder
            // 
            this.btnDataFileFolder.Location = new System.Drawing.Point(706, 53);
            this.btnDataFileFolder.Name = "btnDataFileFolder";
            this.btnDataFileFolder.Size = new System.Drawing.Size(75, 23);
            this.btnDataFileFolder.TabIndex = 5;
            this.btnDataFileFolder.Text = "Browse...";
            this.btnDataFileFolder.UseVisualStyleBackColor = true;
            this.btnDataFileFolder.Click += new System.EventHandler(this.btnDataFileFolder_Click);
            // 
            // txtDataFileFolder
            // 
            this.txtDataFileFolder.Location = new System.Drawing.Point(120, 56);
            this.txtDataFileFolder.Name = "txtDataFileFolder";
            this.txtDataFileFolder.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDataFileFolder.Size = new System.Drawing.Size(581, 22);
            this.txtDataFileFolder.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Data File Folder";
            // 
            // btnMetaDataFile
            // 
            this.btnMetaDataFile.Location = new System.Drawing.Point(706, 10);
            this.btnMetaDataFile.Name = "btnMetaDataFile";
            this.btnMetaDataFile.Size = new System.Drawing.Size(75, 23);
            this.btnMetaDataFile.TabIndex = 2;
            this.btnMetaDataFile.Text = "Browse...";
            this.btnMetaDataFile.UseVisualStyleBackColor = true;
            this.btnMetaDataFile.Click += new System.EventHandler(this.btnMetaDataFile_Click);
            // 
            // txtMetaDataFile
            // 
            this.txtMetaDataFile.Location = new System.Drawing.Point(119, 13);
            this.txtMetaDataFile.Name = "txtMetaDataFile";
            this.txtMetaDataFile.Size = new System.Drawing.Size(581, 22);
            this.txtMetaDataFile.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Meta data File";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnGO);
            this.tabPage1.Controls.Add(this.txtInputFile);
            this.tabPage1.Controls.Add(this.btnBrowse);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(796, 350);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Import Reference Values";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnLayoutSheetGO);
            this.tabPage3.Controls.Add(this.btnBrowseForPDMS);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.txtPDMSPath);
            this.tabPage3.Controls.Add(this.btnBrowseForMMIS);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.txtMMISPath);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(796, 350);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Compare MMIS/PDMS Layout Sheets";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnLayoutSheetGO
            // 
            this.btnLayoutSheetGO.Location = new System.Drawing.Point(353, 120);
            this.btnLayoutSheetGO.Name = "btnLayoutSheetGO";
            this.btnLayoutSheetGO.Size = new System.Drawing.Size(75, 23);
            this.btnLayoutSheetGO.TabIndex = 6;
            this.btnLayoutSheetGO.Text = "GO";
            this.btnLayoutSheetGO.UseVisualStyleBackColor = true;
            this.btnLayoutSheetGO.Click += new System.EventHandler(this.btnLayoutSheetGO_Click);
            // 
            // btnBrowseForPDMS
            // 
            this.btnBrowseForPDMS.Location = new System.Drawing.Point(698, 68);
            this.btnBrowseForPDMS.Name = "btnBrowseForPDMS";
            this.btnBrowseForPDMS.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseForPDMS.TabIndex = 5;
            this.btnBrowseForPDMS.Text = "Browse...";
            this.btnBrowseForPDMS.UseVisualStyleBackColor = true;
            this.btnBrowseForPDMS.Click += new System.EventHandler(this.btnBrowseForPDMS_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "PDMS Mapping File";
            // 
            // txtPDMSPath
            // 
            this.txtPDMSPath.Location = new System.Drawing.Point(140, 68);
            this.txtPDMSPath.Name = "txtPDMSPath";
            this.txtPDMSPath.Size = new System.Drawing.Size(552, 22);
            this.txtPDMSPath.TabIndex = 3;
            // 
            // btnBrowseForMMIS
            // 
            this.btnBrowseForMMIS.Location = new System.Drawing.Point(698, 23);
            this.btnBrowseForMMIS.Name = "btnBrowseForMMIS";
            this.btnBrowseForMMIS.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseForMMIS.TabIndex = 2;
            this.btnBrowseForMMIS.Text = "Browse...";
            this.btnBrowseForMMIS.UseVisualStyleBackColor = true;
            this.btnBrowseForMMIS.Click += new System.EventHandler(this.btnBrowseForMMIS_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "MMIS Layout File";
            // 
            // txtMMISPath
            // 
            this.txtMMISPath.Location = new System.Drawing.Point(140, 23);
            this.txtMMISPath.Name = "txtMMISPath";
            this.txtMMISPath.Size = new System.Drawing.Size(552, 22);
            this.txtMMISPath.TabIndex = 0;
            // 
            // tpErrorReport
            // 
            this.tpErrorReport.Controls.Add(this.txtSourceDatabase);
            this.tpErrorReport.Controls.Add(this.label8);
            this.tpErrorReport.Controls.Add(this.btnCreateErrorReport);
            this.tpErrorReport.Controls.Add(this.btnBrowseErrReportOutput);
            this.tpErrorReport.Controls.Add(this.txtOutputFileName);
            this.tpErrorReport.Controls.Add(this.txtRunID);
            this.tpErrorReport.Controls.Add(this.label7);
            this.tpErrorReport.Controls.Add(this.label6);
            this.tpErrorReport.Location = new System.Drawing.Point(4, 25);
            this.tpErrorReport.Name = "tpErrorReport";
            this.tpErrorReport.Padding = new System.Windows.Forms.Padding(3);
            this.tpErrorReport.Size = new System.Drawing.Size(796, 350);
            this.tpErrorReport.TabIndex = 3;
            this.tpErrorReport.Text = "Make Conversion Error Report";
            this.tpErrorReport.UseVisualStyleBackColor = true;
            // 
            // btnCreateErrorReport
            // 
            this.btnCreateErrorReport.Location = new System.Drawing.Point(273, 321);
            this.btnCreateErrorReport.Name = "btnCreateErrorReport";
            this.btnCreateErrorReport.Size = new System.Drawing.Size(250, 23);
            this.btnCreateErrorReport.TabIndex = 5;
            this.btnCreateErrorReport.Text = "Create Error Report";
            this.btnCreateErrorReport.UseVisualStyleBackColor = true;
            this.btnCreateErrorReport.Click += new System.EventHandler(this.btnCreateErrorReport_Click);
            // 
            // btnBrowseErrReportOutput
            // 
            this.btnBrowseErrReportOutput.Location = new System.Drawing.Point(685, 65);
            this.btnBrowseErrReportOutput.Name = "btnBrowseErrReportOutput";
            this.btnBrowseErrReportOutput.Size = new System.Drawing.Size(80, 23);
            this.btnBrowseErrReportOutput.TabIndex = 4;
            this.btnBrowseErrReportOutput.Text = "Browse...";
            this.btnBrowseErrReportOutput.UseVisualStyleBackColor = true;
            this.btnBrowseErrReportOutput.Click += new System.EventHandler(this.btnBrowseErrReportOutput_Click);
            // 
            // txtOutputFileName
            // 
            this.txtOutputFileName.Location = new System.Drawing.Point(90, 65);
            this.txtOutputFileName.Name = "txtOutputFileName";
            this.txtOutputFileName.Size = new System.Drawing.Size(589, 22);
            this.txtOutputFileName.TabIndex = 3;
            // 
            // txtRunID
            // 
            this.txtRunID.Location = new System.Drawing.Point(91, 7);
            this.txtRunID.Name = "txtRunID";
            this.txtRunID.Size = new System.Drawing.Size(307, 22);
            this.txtRunID.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "Output File";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Run ID";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 17);
            this.label8.TabIndex = 6;
            this.label8.Text = "Source Database";
            // 
            // txtSourceDatabase
            // 
            this.txtSourceDatabase.Location = new System.Drawing.Point(127, 39);
            this.txtSourceDatabase.Name = "txtSourceDatabase";
            this.txtSourceDatabase.Size = new System.Drawing.Size(552, 22);
            this.txtSourceDatabase.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 403);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "DC PDMS Data Conversion Utilities";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tpErrorReport.ResumeLayout(false);
            this.tpErrorReport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.OpenFileDialog dlgGetFilePath;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtMetaDataFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMetaDataFile;
        private System.Windows.Forms.Button btnDataFileFolder;
        private System.Windows.Forms.TextBox txtDataFileFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCreateObjects;
        private System.Windows.Forms.FolderBrowserDialog dlgFolderPicker;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnLayoutSheetGO;
        private System.Windows.Forms.Button btnBrowseForPDMS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPDMSPath;
        private System.Windows.Forms.Button btnBrowseForMMIS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMMISPath;
        private System.Windows.Forms.TextBox txtImportFileExt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tpErrorReport;
        private System.Windows.Forms.Button btnCreateErrorReport;
        private System.Windows.Forms.Button btnBrowseErrReportOutput;
        private System.Windows.Forms.TextBox txtOutputFileName;
        private System.Windows.Forms.TextBox txtRunID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSourceDatabase;
        private System.Windows.Forms.Label label8;
    }
}

