using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using PDMSFileProcessorUI;
using FileProcessorCore;

namespace PDMSFileProcessorUI
{
	public partial class Main : Form
	{
		private LogFile _log;

		
		public Main()
		{
			InitializeComponent();

			_log = new LogFile("PDMSFileProcessorUI", false);
			_log.LogMessagedAdded += _log_LogMessagedAdded;
		}

		void _log_LogMessagedAdded(LogMessageAddedEventArgs args)
		{
			txtLog.Text = string.Format("{0}{1}{2}", args.Message, Environment.NewLine, txtLog.Text);
		}

		private void btnBrowseTestCSVFile_Click(object sender, EventArgs e)
		{
			DialogResult result = dlgOpenFile.ShowDialog();

			if (result == DialogResult.OK) 
			{
				txtTestCSVFile.Text = dlgOpenFile.FileName;
			}
		}

		private void btnImportTestCSVFile_Click(object sender, EventArgs e)
		{
			string workingFile = txtTestCSVFile.Text;
			if (!string.IsNullOrEmpty(workingFile) && File.Exists(workingFile))
			{
				SAMExclusionImportFileProcessor importProcessor = new SAMExclusionImportFileProcessor(_log);
				

				string dir = Path.GetDirectoryName(workingFile);
				string archiveDirectory = Path.Combine(dir, "Archive");
				string errorDirectory = Path.Combine(dir, "Error");
				var importRecords = importProcessor.ProcessImportFile(_log, importProcessor.GetLineProcessor(workingFile), workingFile, null, errorDirectory);

				SAMExclusionExportFileProcessor exportProcessor = new SAMExclusionExportFileProcessor(_log, importRecords, workingFile + ".dat");

				exportProcessor.Execute();
			}
		}
	}
}
