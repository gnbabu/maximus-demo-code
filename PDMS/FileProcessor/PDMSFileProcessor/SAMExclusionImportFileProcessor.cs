using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileProcessorCore;

namespace PDMSFileProcessorUI
{
	public class SAMExclusionImportFileProcessor : LineRecordImportFileProcessor
	{

		#region Constructors

		public SAMExclusionImportFileProcessor(LogFile log)
			: base(log)
		{

		}

		#endregion

		public override LineRecordImportLineProcessor GetLineProcessor(string filename)
		{
			/// Always return SAMExclusionImportLineProcessor
			return new SAMExclusionImportLineProcessor(Log);
		}

		public override string BuildImportSummary(List<Record> lstRecords, LineRecordImportLineProcessor processor, string filename)
		{
			return "Summary: TOOD.";
		}

		public override int Compare(string filePathX, string filePathY)
		{
			return filePathX.CompareTo(filePathY);
		}
	}
}
