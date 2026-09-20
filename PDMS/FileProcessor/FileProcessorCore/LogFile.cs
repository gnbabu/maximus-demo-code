using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FileProcessorCore
{
	public class LogMessageAddedEventArgs : EventArgs
	{
		public string Message { get; set; }

		public LogMessageAddedEventArgs(string message) : base()
		{
			Message = message;
		}
	}

	public class LogFile
	{
		public string ApplicationName { get; set; }
		public bool LogToConsole { get; set; }
		private StringBuilder log;
		private DateTime? _start;
		private DateTime? _end;

		public bool WriteVerboseLog
		{
			get
			{
				return !string.IsNullOrWhiteSpace(EWSConfiguration.AppSettings("WriteVerboseLog")) &&
					bool.Parse(EWSConfiguration.AppSettings("WriteVerboseLog"));
			}
		}

		public delegate void LogMessagedAddedHandler(LogMessageAddedEventArgs args);
		public event LogMessagedAddedHandler LogMessagedAdded;


		public LogFile(string applicationName, bool logToConsole)
		{
			log = new StringBuilder();
			ApplicationName = applicationName;
			LogToConsole = logToConsole;
		}

		public void Start()
		{
			_start = DateTime.Now;
		}

		public void End()
		{
			_end = DateTime.Now;
		}

		public TimeSpan? TotalTime
		{
			get
			{
				if (!_start.HasValue || !_end.HasValue)
				{
					return null;
				}
				else
				{
					return _end - _start;
				}
			} 
		}

		public void WriteError(string message)
		{
			string error = string.Format("ERROR: {0}", message);
			log.AppendLine(error);

			if (LogToConsole)
			{
				Console.WriteLine(error);
			}

			if(LogMessagedAdded != null)
			{
				LogMessagedAdded(new LogMessageAddedEventArgs(message));
			}
		}

		public void Write(string message)
		{
			log.AppendLine(message);

			if (LogToConsole)
			{
				Console.WriteLine(message);
			}

			if(LogMessagedAdded != null)
			{
				LogMessagedAdded(new LogMessageAddedEventArgs(message));
			}
		}

		public void WriteVerbose(string message)
		{
			if (WriteVerboseLog)
			{
				log.AppendLine(message);

				if (LogToConsole)
				{
					Console.WriteLine(message);
				}

				if(LogMessagedAdded != null)
				{
					LogMessagedAdded(new LogMessageAddedEventArgs(message));
				}
			}
		}

		public string GetLog()
		{
			return log.ToString();
		}

		public void SendStatusEmail()
		{
			/// Save to file or Email
			EmailHelper.SendSuccessLogEmail(this, ApplicationName, log.ToString());
		}

		public void Clear()
		{
			log.Clear();
		}
	}
}
