using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessorCore
{
	public class ImportRecord : Record
	{
		public string Filename { get; protected set; }
		public int LineNumber { get; protected set; }

		public ImportRecord(LogFile log, string filename, int lineNumber) : base(log)
		{
			Filename = filename;
			LineNumber = lineNumber;
		}
	}
}
