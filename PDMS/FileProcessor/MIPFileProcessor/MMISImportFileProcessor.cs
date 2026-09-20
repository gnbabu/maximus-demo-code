using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using FileProcessorCore;

namespace MIPFileProcessor
{
    public class MMISImportFileProcessor : LineRecordImportFileProcessor
	{
		#region Static Members

		private Regex FILENAME_DATE_REGEX = new Regex(@"NE_MIP_PROV_ELIG_(?<date>\d{8}).TXT", RegexOptions.Compiled);

		#endregion

		#region Private Members

		private MMISImportLineProcessor _lineProcessor;

		#endregion

		#region Constructors

		public MMISImportFileProcessor(LogFile log)
			: base(log)
		{
			_lineProcessor = new MMISImportLineProcessor(log);
		}

		#endregion

		public override LineRecordImportLineProcessor GetLineProcessor(string filename)
		{
			return _lineProcessor;
		}

		public override string BuildImportSummary(List<Record> lstRecords, LineRecordImportLineProcessor processor, string filename)
		{
			StringBuilder sb = new StringBuilder();

			
			foreach (var record in lstRecords)
			{
				if (Log.WriteVerboseLog)
				{
					foreach (var item in record.Data)
					{
						sb.AppendFormat("{0} = {1}{2}", item.Key, item.Value, Environment.NewLine);
					}

					sb.AppendLine();
				}

				/// Write lines that failed or were invalid.
				if (!record.IsValid)
				{
					if(record is ImportRecord)
					{
						sb.AppendLine(string.Format("Record at Line {0} with ID '{1}' is invalid:", ((ImportRecord)record).LineNumber, record.ID));
					}
					else
					{
						sb.AppendLine(string.Format("Record with ID '{0}' is invalid:", record.ID));
					}

					/// Specify which errors were thrown for the record
					foreach (var error in record.Errors)
					{
						sb.AppendLine(string.Format("	VALIDATION ERROR: {0}", error));
					}

					sb.AppendLine();
				}
			}

			/// Total summary
			sb.AppendFormat("Processed {0} records from {1}.  {2} records were invalid.", lstRecords.Count, filename, lstRecords.Where(p => !p.IsValid).Count());
			return sb.ToString();
		}

		/// <summary>
		/// Override of Compare method to sort files based on date inside the filename, so the earlier files are processed first.
		/// If dates not found, go by LastModifiedDateTime.
		/// </summary>
		/// <param name="filePathX"></param>
		/// <param name="filePathY"></param>
		/// <returns></returns>
		public override int Compare(string filePathX, string filePathY)
		{
			Match matchX = FILENAME_DATE_REGEX.Match(filePathX);
			Match matchY = FILENAME_DATE_REGEX.Match(filePathY);
			DateTime dateTimeX;
			DateTime dateTimeY;

			if (matchX.Success && matchY.Success)
			{
				dateTimeX = DateTime.ParseExact(matchX.Groups["date"].Value, "MMddyyyy", null);
				dateTimeY = DateTime.ParseExact(matchY.Groups["date"].Value, "MMddyyyy", null);

				if (dateTimeX != dateTimeY)
				{
					return dateTimeX.CompareTo(dateTimeY);
				}
			}

			/// Could not determine by file date, so use file last accessed date to compare.
			dateTimeX = Directory.GetLastWriteTimeUtc(filePathX);
			dateTimeY = Directory.GetLastWriteTimeUtc(filePathY);

			return dateTimeX.CompareTo(dateTimeY);
		}
	}
}
