using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace FileProcessorCore
{
	public abstract class LineRecordExportFileProcessor : BatchProcessor
	{
		public static string PickupFolder
		{
			get
			{
				return EWSConfiguration.AppSettings("PickupFolder");
			}
		}

		#region Constructors

		public LineRecordExportFileProcessor()
			: base()
		{

		}

		public LineRecordExportFileProcessor(LogFile log)
			: base(log)
		{

		}

		#endregion

		protected abstract IEnumerable<Record> GetRecords(LogFile log);

		protected abstract Dictionary<LineRecordExportLineProcessor, IEnumerable<Record>> SplitRecordsIntoProcessorGroups(LogFile log, string pickupFolderPath, IEnumerable<Record> lstRecords);

		public override void Execute()
		{
			Log.WriteVerbose("Begin loading records to be exported...");

			IEnumerable<Record> lstExportRecords = GetRecords(Log);

			Log.Write(string.Format("Found {0} Export Records.", lstExportRecords.Count()));

			Log.WriteVerbose("End loading records .");


			Log.WriteVerbose("Begin processing export records...");

			if (!string.IsNullOrEmpty(PickupFolder) && !Directory.Exists(PickupFolder))
			{
				Directory.CreateDirectory(PickupFolder);
			}

			var processorGroups = SplitRecordsIntoProcessorGroups(Log, PickupFolder, lstExportRecords);

			foreach (var processorGroup in processorGroups)
			{
				LineRecordExportLineProcessor processor = processorGroup.Key;

				if (processorGroup.Value != null && processorGroup.Value.Count() > 0)
				{

					string filePath = processor.GenerateFilename(PickupFolder);

					Log.WriteVerbose(string.Format("Writing export file {0}", filePath));

					using (StreamWriter writer = new StreamWriter(filePath, true))
					{
						if (!string.IsNullOrEmpty(processor.HeaderLine))
						{
							writer.WriteLine(processor.HeaderLine);
						}

						foreach (var record in processorGroup.Value)
						{
							Log.WriteVerbose(string.Format("Begin writing output record. ID: {0}. File: {1}.", record.ID, filePath));

							processor.PreProcessRecord(record);

							processor.WriteRecordToFile(writer, record);

							writer.WriteLine();

							Log.WriteVerbose(string.Format("End writing output file. ID: {0}.", record.ID));
						}

						writer.Flush();
						writer.Close();
					}
				}
			}

			Log.WriteVerbose("End processing export records.");

		}
	}
}
