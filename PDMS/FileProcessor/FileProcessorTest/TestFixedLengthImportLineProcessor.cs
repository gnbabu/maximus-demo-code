using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileProcessorCore;

namespace FileProcessorTest
{
	public class TestFixedLengthImportLineProcessor : LineRecordImportLineProcessor
	{
		public TestFixedLengthImportLineProcessor(LogFile log)
			: base(log, null, FileFormat.FixedLength)
		{

		}

		protected override string TemplateName
		{
			get { return "Templates\\Test_FixedLength.xml"; }
		}

		public override Record NewRecord(string workingFilePath, int lineNumber)
		{
			Log.Write("Test Processor: NewRecord() called.");

			return new Record(Log);
		}

		protected override bool IsInsertRecord(Record record)
		{
			return record.Data["Operation"] == "A" ||
				record.Data["Operation"] == "C" ||
				record.Data["Operation"] == string.Empty;
		}

		protected override bool InsertRecord(Record record)
		{
			Log.Write("Test Processor: InsertRecord() called.");

			return true;
		}

		protected override bool IsUpdateRecord(Record record)
		{
			return false;
		}

		protected override bool UpdateRecord(Record record)
		{
			Log.Write("Test Processor: UpdateRecord() called.");

			return true;
		}

		protected override bool IsDeleteRecord(Record record)
		{
			return record.Data["Operation"] == "D";
		}

		protected override bool DeleteRecord(Record record)
		{
			Log.Write("Test Processor: DeleteRecord() called.");

			return true;
		}
	}
}
