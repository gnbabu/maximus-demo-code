using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using FileProcessorCore;


namespace PDMSFileProcessorUI
{
    public class SAMExclusionExportFileProcessor : LineRecordExportFileProcessor
	{
		#region Private Members

		private IEnumerable<Record> _records;

		#endregion

		#region Properties

		public string ExportFileName { get; set; }

		#endregion

		#region Constructors

		public SAMExclusionExportFileProcessor(LogFile log, IEnumerable<Record> records, string exportFileName)
			: base(log)
		{
			_records = records;
			ExportFileName = exportFileName;
		}

		#endregion

		protected override IEnumerable<Record> GetRecords(LogFile log)
		{
			return _records;
		}

		protected override Dictionary<LineRecordExportLineProcessor, IEnumerable<Record>> SplitRecordsIntoProcessorGroups(LogFile log, string pickupFolderPath, IEnumerable<Record> lstRecords)
		{
			Dictionary<LineRecordExportLineProcessor, IEnumerable<Record>> dictProcessors = new Dictionary<LineRecordExportLineProcessor, IEnumerable<Record>>();

			/// All records go into same processor.
			SAMExclusionExportLineProcessor lineProcessor = new SAMExclusionExportLineProcessor(ExportFileName, log);

			dictProcessors[lineProcessor] = lstRecords;

			return dictProcessors;
		}
	}
}
