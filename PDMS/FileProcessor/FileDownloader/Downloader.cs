using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ComponentModel;
using FileProcessorCore;

namespace FileDownloader
{
    public class Downloader
	{
		private LogFile _log;


		public Downloader(LogFile log)
		{
			_log = log;
		}


		#region Events

		public delegate void DownloadProgressChangedHandler(DownloadProgressChangedEventArgs args);
		public event DownloadProgressChangedHandler DownloadProgressChanged;

		public delegate void DownloadFileCompletedHandler(AsyncCompletedEventArgs args);
		public event DownloadFileCompletedHandler DownloadFileCompleted;

		#endregion

		public bool Fetch(Uri uri, string fileName, string id)
		{
			try
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadFileCompleted += client_DownloadFileCompleted;
					client.DownloadProgressChanged += client_DownloadProgressChanged;
					client.DownloadFileAsync(uri, fileName, id);
				}

			}
			catch (Exception ex)
			{
				_log.WriteError(string.Format("Error downloading file from Uri '{0}' to location '{1}': {2}", uri, fileName, ex.ToString()));

				return false;
			}

			return true;
		}

		void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			if (DownloadProgressChanged != null)
			{
				DownloadProgressChanged(e);
			}
		}

		void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (DownloadFileCompleted != null)
			{
				DownloadFileCompleted(e);
			}
		}
    }
}
