using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Configuration;

using FileProcessorCore;

namespace FileProcessorTest
{
	class Program
	{
		#region Configuration Items

		private static bool? _runInteractively;

		public static bool RunInteractively
		{
			get
			{
				if (!_runInteractively.HasValue)
				{
					bool temp = false;
					bool.TryParse(ConfigurationManager.AppSettings["runInteractively"], out temp);

					_runInteractively = temp;
				}

				return _runInteractively.Value;
			}
		}

		


		#endregion

		static void Main(string[] args)
		{
			string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Configs", ConfigurationManager.AppSettings["configFile"]);
			FileProcessorConfig config = FileProcessorConfig.ParseConfig(configPath);

			TestImportFileProcessor fileProcessor = new TestImportFileProcessor();

			TestFixedLengthImportLineProcessor lineProcessor = new TestFixedLengthImportLineProcessor(fileProcessor.Log);

			foreach(FileProcessorConfig.Folder folder in config.Folders )
			{
				new FolderMonitor(fileProcessor, lineProcessor, folder.Path, "", RunInteractively, folder.ArchiveFolder, folder.ErrorFolder);
			}

			while (true)
			{

			}
		}
	}
}
