using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using FileProcessorCore;

namespace MIPFileProcessor
{
	public static class MMISProviderEligibilityProcessor
	{
		private static LogFile _log;
		private const string IMPORT_FILE_ARGUMENT = "/ImportFileToStaging";
		private const string PROCESS_STAGING_RECORDS_ARGUMENT = "/ProcessStagingRecords";

		private static string ApplicationName
		{
			get
			{
				return ConfigurationManager.AppSettings["ApplicationName"];
			}
		}

		public static void ImportAndProcessFile(string[] args)
		{
			_log = new LogFile(ApplicationName, true);

			if (args.Contains(IMPORT_FILE_ARGUMENT))
			{
				/// First run the file processor that loads up the staging table.
				MMISImportFileProcessor fileProcessor = new MMISImportFileProcessor(_log);

				fileProcessor.Run();

				_log.Clear();
			}

			if (args.Contains(PROCESS_STAGING_RECORDS_ARGUMENT))
			{
				/// Next run the database processor that processes the staging records
				MMISProviderUpdateProcessor databaseProcessor = new MMISProviderUpdateProcessor(_log);

				databaseProcessor.Run();
			}
		}
	}
}
