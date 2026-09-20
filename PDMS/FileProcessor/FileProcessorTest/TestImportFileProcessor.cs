using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileProcessorCore;

namespace FileProcessorTest
{
	public class TestImportFileProcessor : LineRecordImportFileProcessor
	{
		public override LineRecordImportLineProcessor GetLineProcessor(string filename)
		{
			throw new NotImplementedException();
		}

		public override string BuildImportSummary(List<Record> lstRecords, LineRecordImportLineProcessor processor, string filename)
		{
			return string.Format("Processed file: {0}", filename);
		}

		public override int Compare(string filePathX, string filePathY)
		{
			return filePathX.CompareTo(filePathY);
		}
	}
}
