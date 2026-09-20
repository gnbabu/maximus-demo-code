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

using FileProcessorCore;

namespace FileProcessorTestFileGenerator
{
	public partial class Main : Form
	{
		private TestFileProcessor _testFileProcessor;

		private bool WriteLogToConsole
		{
			get
			{
				return !string.IsNullOrWhiteSpace(EWSConfiguration.AppSettings("WriteLogToConsole")) &&
					bool.Parse(EWSConfiguration.AppSettings("WriteLogToConsole"));
			}
		}

		public Main()
		{
			InitializeComponent();
		}

		private void btnBrowseTemplateFolder_Click(object sender, EventArgs e)
		{
			dlgFolderBrowser.SelectedPath = txtTemplateFolder.Text;
			DialogResult result = dlgFolderBrowser.ShowDialog();

			if (result == DialogResult.OK)
			{
				txtTemplateFolder.Text = dlgFolderBrowser.SelectedPath;
				
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			// Restore saved settings
			txtTemplateFolder.Text = Properties.Settings.Default.TemplateFolder;
			txtDropFolder.Text = Properties.Settings.Default.DropFolder;
			txtNumTestFiles.Text = Properties.Settings.Default.NumFiles.ToString();
			txtNumRecordsPerFile.Text = Properties.Settings.Default.NumRecordsPerFile.ToString();

			if (!string.IsNullOrEmpty(txtTemplateFolder.Text) && Directory.Exists(txtTemplateFolder.Text))
			{
				LoadTemplateFiles();
			}

			base.OnLoad(e);
		}

		private void LoadTemplateFiles()
		{
			var files = Directory.GetFiles(txtTemplateFolder.Text, "*.xml", SearchOption.TopDirectoryOnly);
			var fVals = files.Select(f => new KeyValuePair<string, string>(Path.GetFileName(f), f)).ToList();
			lstTemplateFiles.DataSource = fVals;
			lstTemplateFiles.ValueMember = "Value";
			lstTemplateFiles.DisplayMember = "Key";

			if (!string.IsNullOrEmpty(Properties.Settings.Default.TemplateName) &&
				files.Contains(Properties.Settings.Default.TemplateName))
			{
				lstTemplateFiles.SelectedValue = Properties.Settings.Default.TemplateName;
			}
			else
			{
				lstTemplateFiles.SelectedIndex = -1;
			}
		}

		private void btnBrowseDropFolder_Click(object sender, EventArgs e)
		{
			dlgFolderBrowser.SelectedPath = txtDropFolder.Text;
			DialogResult result = dlgFolderBrowser.ShowDialog();

			if (result == DialogResult.OK)
			{
				txtDropFolder.Text = dlgFolderBrowser.SelectedPath;
			}
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			if (lstTemplateFiles.SelectedIndex < 0)
			{
				MessageBox.Show("You must select a template first.");
				return;
			}

			if (!Directory.Exists(txtDropFolder.Text))
			{
				MessageBox.Show("Drop Folder does not exist!");
				return;
			}

			int numFiles, numRecordsPerFile;

			if (!int.TryParse(txtNumTestFiles.Text, out numFiles) || numFiles <= 0)
			{
				MessageBox.Show("# of Files must be a number greater than 0.");
				return;
			}

			if (!int.TryParse(txtNumRecordsPerFile.Text, out numRecordsPerFile) || numRecordsPerFile <= 0)
			{
				MessageBox.Show("# Records per File must be a number greater than 0.");
				return;
			}

			string templateFileName = ((KeyValuePair<string, string>)lstTemplateFiles.SelectedItem).Value;

			Properties.Settings.Default.TemplateFolder = txtTemplateFolder.Text;
			Properties.Settings.Default.DropFolder = txtDropFolder.Text;
			Properties.Settings.Default.NumFiles = numFiles;
			Properties.Settings.Default.NumRecordsPerFile = numRecordsPerFile;
			Properties.Settings.Default.TemplateName = templateFileName;

			Properties.Settings.Default.Save();
			_testFileProcessor = new TestFileProcessor();

			
			LogFile log = new LogFile("Test File Generator", WriteLogToConsole);
			TestLineProcessor processor = new TestLineProcessor(log, templateFileName);
			for (int i = 0; i < numFiles; i++)
			{
				_testFileProcessor.GenerateTestFiles(processor, txtDropFolder.Text, numRecordsPerFile);
			}
		}
	}
}
