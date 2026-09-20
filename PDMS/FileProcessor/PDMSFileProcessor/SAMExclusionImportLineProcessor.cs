using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileProcessorCore;

namespace PDMSFileProcessorUI
{
	public class SAMExclusionImportLineProcessor : LineRecordImportLineProcessor
	{
		#region Constructors

		public SAMExclusionImportLineProcessor(LogFile log)
			: base(log, null, FileFormat.CSV)
		{
			
		}

		#endregion

		public override Record NewRecord(string workingFilePath, int lineNumber)
		{
			return new Record(Log);
		}

		protected override bool IsInsertRecord(Record record)
		{
			return true;
		}

		protected override bool InsertRecord(Record record)
		{
			return true;
		}

		protected override bool IsUpdateRecord(Record record)
		{
			return false;
		}

		protected override bool UpdateRecord(Record record)
		{
			return false;
		}

		protected override bool IsDeleteRecord(Record record)
		{
			return false;
		}

		protected override bool DeleteRecord(Record record)
		{
			return false;
		}

		protected override string TemplateName
		{
			get { return @"Templates\SAMExclusionFormatFile.xml"; }
		}
	}
}
