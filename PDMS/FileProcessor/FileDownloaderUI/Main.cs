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
using FileDownloader;
using FileProcessorCore;

namespace FileDownloaderUI
{
	public partial class Main : Form
	{
		private LogFile _log;
		

		public Uri DownloadUri
		{
			get
			{
				if (Uri.IsWellFormedUriString(txtUri.Text, UriKind.Absolute))
				{
					return new Uri(txtUri.Text);
				}
				else
				{
					return null; 
				}
			}
		}

		public string DownloadDestinationPath
		{
			get
			{
				if (DownloadUri != null && !string.IsNullOrEmpty(txtDestination.Text))
				{
					return Path.Combine(txtDestination.Text, DownloadUri.Segments[DownloadUri.Segments.Length - 1]);
				}
				else
				{
					return null;
				}
			}
		}

		public DownloadTaskConfig TaskConfig { get; set; }

		private DownloadTask _selectedTask;
		public DownloadTask SelectedTask
		{
			get
			{
				return _selectedTask;
			}
			set
			{
				_selectedTask = value;
				LoadTaskDetails(_selectedTask);
			}
		}

		public Main()
		{
			InitializeComponent();

			_log = new LogFile("FileDownloader", false);
			_log.LogMessagedAdded += _log_LogMessagedAdded;

			/// Create default task config
			TaskConfig = new DownloadTaskConfig();
		}

		void _log_LogMessagedAdded(LogMessageAddedEventArgs args)
		{
			txtLog.Text = string.Format("{0}{1}{2}", args.Message, Environment.NewLine, txtLog.Text);
		}

		private void btnBrowseFile_Click(object sender, EventArgs e)
		{
			DialogResult result = dlgSaveFolder.ShowDialog();

			if (result == DialogResult.OK && !string.IsNullOrEmpty(dlgSaveFolder.SelectedPath))
			{
				txtDestination.Text = dlgSaveFolder.SelectedPath;
			}
		}
		private void btnDownload_Click(object sender, EventArgs e)
		{
			DownloadFile();
		}

		private void DownloadFile()
		{
			if (DownloadUri == null)
			{
				lblProgressDetails.Text = "Enter URL.";
			}
			else if(string.IsNullOrEmpty(DownloadDestinationPath))
			{
				lblProgressDetails.Text = "Enter destination path.";
			}
			else
			{
				if (File.Exists(DownloadDestinationPath))
				{
					DialogResult result = MessageBox.Show("Overwrite existing file?", "Confirm", MessageBoxButtons.YesNo);
					if (result != DialogResult.Yes)
					{
						return;
					}
				}

				btnDownload.Enabled = false;

				Downloader downloader = new Downloader(_log);
				downloader.DownloadFileCompleted += downloader_DownloadFileCompleted;
				downloader.DownloadProgressChanged += downloader_DownloadProgressChanged;

				downloader.Fetch(DownloadUri, DownloadDestinationPath, "");
			}
		}

		void downloader_DownloadProgressChanged(System.Net.DownloadProgressChangedEventArgs args)
		{
 			pbDownload.Value = args.ProgressPercentage;

			lblProgressDetails.Text = string.Format("{0} of {1} bytes...", args.BytesReceived, args.TotalBytesToReceive);
		}

		void downloader_DownloadFileCompleted(AsyncCompletedEventArgs args)
		{
 			btnDownload.Enabled = true;
			if(args.Error == null)
			{

				lblProgressDetails.Text = "Download complete.";
				
			}
			else
			{
				lblProgressDetails.Text = "Error downloading file. See Log below.";
				_log.WriteError(string.Format("Error downloading file: {0}", args.Error.ToString()));
			}
		}

		private void btnAddTask_Click(object sender, EventArgs e)
		{
			DownloadTask task = new DownloadTask();

			task.Name = "<new task>";

			TaskConfig.Tasks.Add(task);
			
			LoadTasks();

			/// Select correct one.
			lbDownloadTasks.SelectedIndex = TaskConfig.Tasks.Count - 1;

			txtTaskName.Focus();
			txtTaskName.SelectAll();
		}

		private void btnDeleteTask_Click(object sender, EventArgs e)
		{
			if (TaskConfig.Tasks.Count > lbDownloadTasks.SelectedIndex)
			{
				TaskConfig.Tasks.RemoveAt(lbDownloadTasks.SelectedIndex);

				LoadTasks();
			}
		}

		private void LoadTasks()
		{
			lbDownloadTasks.DataSource = null;
			lbDownloadTasks.DataSource = TaskConfig.Tasks;
			
		}

		private void lbDownloadTasks_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbDownloadTasks.SelectedIndex > -1)
			{
				/// Or Selected Item, casted??
				SelectedTask = (DownloadTask)lbDownloadTasks.SelectedItem;

				btnDeleteTask.Enabled = true;
			}
			else
			{
				SelectedTask = null;
				btnDeleteTask.Enabled = false;
			}
		}

		private void LoadTaskDetails(DownloadTask task)
		{
			if (task == null)
			{
				txtTaskName.Text = string.Empty;
				txtTaskUrlPattern.Text = string.Empty;
				txtTestUrlValue.Text = string.Empty;
				txtSeedIncrement.Text = string.Empty;
				txtSeedStartValue.Text = string.Empty;
				txtYearOffset.Text = string.Empty;
				txtMonthOffset.Text = string.Empty;
				txtDayOffset.Text = string.Empty;
				dtSeedStart.Value = DateTime.Today;
			}
			else
			{
				txtTaskName.Text = task.Name;
				txtTaskUrlPattern.Text = task.UrlPattern;
				txtTestUrlValue.Text = task.FormatUrl(dtTestDate.Value);
				txtSeedIncrement.Text = task.SeedIncrement.ToString();
				txtSeedStartValue.Text = task.SeedStartValue.ToString();
				txtYearOffset.Text = task.YearOffset.ToString();
				txtMonthOffset.Text = task.MonthOffset.ToString();
				txtDayOffset.Text = task.DayOffset.ToString();
				dtSeedStart.Value = task.SeedStartDate.HasValue ? task.SeedStartDate.Value : DateTime.Today;
			}
			
		}

		private void txtTaskName_Leave(object sender, EventArgs e)
		{
			SelectedTask.Name = txtTaskName.Text.Trim();
			LoadTasks();
		}

		private void txtTaskUrlPattern_Leave(object sender, EventArgs e)
		{
			SelectedTask.UrlPattern = txtTaskUrlPattern.Text.Trim();
			LoadTasks();
		}
		private void dtSeedStart_Leave(object sender, EventArgs e)
		{
			SelectedTask.SeedStartDate = dtSeedStart.Value;
			LoadTasks();
		}

		private void txtYearOffset_Leave(object sender, EventArgs e)
		{
			int temp;

			if (int.TryParse(txtYearOffset.Text.Trim(), out temp))
			{
				SelectedTask.YearOffset = temp;
			}
			else
			{
				SelectedTask.YearOffset = null;
			}
			
			LoadTasks();
		}

		private void txtMonthOffset_Leave(object sender, EventArgs e)
		{
			int temp;

			if (int.TryParse(txtMonthOffset.Text.Trim(), out temp))
			{
				SelectedTask.MonthOffset = temp;
			}
			else
			{
				SelectedTask.MonthOffset = null;
			}

			LoadTasks();
		}

		private void txtDayOffset_Leave(object sender, EventArgs e)
		{
			int temp;

			if (int.TryParse(txtDayOffset.Text.Trim(), out temp))
			{
				SelectedTask.DayOffset = temp;
			}
			else
			{
				SelectedTask.DayOffset = null;
			}

			LoadTasks();
		}

		private void txtSeedStartValue_Leave(object sender, EventArgs e)
		{
			try
			{
				SelectedTask.SeedStartValue = int.Parse(txtSeedStartValue.Text.Trim());
				LoadTasks();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Seed Start Value must be an integer.");
			}
		}

		private void txtSeedIncrement_Leave(object sender, EventArgs e)
		{
			try
			{
				SelectedTask.SeedIncrement = int.Parse(txtSeedIncrement.Text.Trim());
				LoadTasks();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Seed Increment must be an integer.");
			}
		}

		private void btnSaveConfig_Click(object sender, EventArgs e)
		{
			if (TaskConfig != null)
			{
				DialogResult result = dlgSaveFile.ShowDialog();

				if (result == DialogResult.OK)
				{
					TaskConfig.Save(dlgSaveFile.FileName);
				}
			}
		}

		private void btnOpenConfig_Click(object sender, EventArgs e)
		{
			DialogResult result = dlgOpenFile.ShowDialog();

			if (result == DialogResult.OK)
			{
				try
				{
					TaskConfig = DownloadTaskConfig.ParseConfig(dlgOpenFile.FileName);
					LoadTasks();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error opening file.");
				}
			}
		}

		private void dtTestDate_ValueChanged(object sender, EventArgs e)
		{
			LoadTaskDetails(SelectedTask);
		}

		private void btnDownloadTestUrl_Click(object sender, EventArgs e)
		{
			txtUri.Text = txtTestUrlValue.Text;
			tabConfig.SelectedTab = tabDownloadFile;
			DownloadFile();
		}

		

		

		

		

	
	}
}
