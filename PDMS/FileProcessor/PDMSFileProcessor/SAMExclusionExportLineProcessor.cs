using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using FileProcessorCore;

namespace PDMSFileProcessorUI
{
	public class SAMExclusionExportLineProcessor : LineRecordExportLineProcessor
	{

		#region Properties

		public string FileName { get; set; }

		#endregion

		#region Constructors

		public SAMExclusionExportLineProcessor(string fileName, LogFile log) : base(log)
		{
			FileName = fileName;
		}

		#endregion

		public override string GenerateFilename(string pickupFolderPath)
		{
			return FileName;
		}

		protected override void PostProcessRecord(Record record, bool hasError)
		{
			
		}

		protected override string TemplateName
		{
			get { return @"Templates\SAMExclusionFormatFile.xml"; }
		}
	}
}
