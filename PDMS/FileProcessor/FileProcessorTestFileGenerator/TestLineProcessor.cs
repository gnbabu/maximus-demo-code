using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileProcessorCore;

namespace FileProcessorTestFileGenerator
{
	public class TestLineProcessor : LineRecordImportLineProcessor
	{
		private string _templateName;

		protected override string TemplateName
		{
			get { return _templateName; }
		}
		public TestLineProcessor(LogFile log, string templatePath) : base(log, templatePath, FileFormat.FixedLength)
		{
			_templateName = templatePath;
		}

		public override Record NewRecord(string workingFilePath, int lineNumber)
		{
			Log.Write("Test Processor: NewRecord() called.");

			return new Record(Log);
		}

		protected override bool IsInsertRecord(Record record)
		{
			Log.Write("Test Processor: IsInsertRecord() called.");

			return true;
		}

		protected override bool InsertRecord(Record record)
		{
			Log.Write("Test Processor: InsertRecord() called.");

			return true;
		}

		protected override bool IsUpdateRecord(Record record)
		{
			Log.Write("Test Processor: IsUpdateRecord() called.");

			return true;
		}

		protected override bool UpdateRecord(Record record)
		{
			Log.Write("Test Processor: UpdateRecord() called.");

			return true;
		}

		protected override bool IsDeleteRecord(Record record)
		{
			Log.Write("Test Processor: IsDeleteRecord() called.");

			return true;
		}

		protected override bool DeleteRecord(Record record)
		{
			Log.Write("Test Processor: DeleteRecord() called.");

			return true;
		}
	}
}
