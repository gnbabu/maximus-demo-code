using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace FileProcessorCore
{
	public class Record
	{
		public List<string> Errors { get; set; }
		public bool FailedValidation { get; set; }
		public bool Success { get; set; }
		public bool RecordFilteredOut { get; set; }
		public bool IsValid { get; set; }
		public Dictionary<string, string> Data { get; set; }
		public LogFile Log { get; private set; }
		
		public string AlertDocumentTemplate { get; set; }

		public virtual string ID { get; set; }

		public Record(LogFile log)
		{
			Log = log;
			Data = new Dictionary<string, string>();
			Errors = new List<string>();
		}
	}

	

	
}
