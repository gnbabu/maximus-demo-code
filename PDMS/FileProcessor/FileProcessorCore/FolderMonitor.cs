using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileProcessorCore
{
	public class FolderMonitor
	{
		private FileSystemWatcher _watcher;

		public bool IsInteractive { get; set; }
		public LineRecordImportFileProcessor FileProcessor { get; set; }
		public LineRecordImportLineProcessor LineProcessor { get; set; }
		public string ArchiveFolder { get; set; }
		public string ErrorFolder { get; set; }

		public FolderMonitor(LineRecordImportFileProcessor fileProcessor, LineRecordImportLineProcessor lineProcessor, string path, string fileExtensionFilter, 
			bool isInteractive, string archiveFolder, string errorFolder)
		{
			FileProcessor = fileProcessor;
			LineProcessor = lineProcessor;

			IsInteractive = isInteractive;

			ArchiveFolder = archiveFolder;
			ErrorFolder = errorFolder;

			_watcher = new FileSystemWatcher(path);

			_watcher.Created += new FileSystemEventHandler(watcher_Created);
			_watcher.EnableRaisingEvents = true;

			Console.WriteLine("Started monitoring folder " + _watcher.Path);
		}

		private void watcher_Created(object sender, FileSystemEventArgs e)
		{
			if(e.ChangeType == WatcherChangeTypes.Created)
			{
				Console.WriteLine(string.Format("New file detected: {0}.", e.FullPath));

				bool proccessFile = false;
				if (IsInteractive)
				{
					bool hasAnswer = false;
					/// Get input
					while (!hasAnswer)
					{
						Console.WriteLine("Process file? Y/N");
						ConsoleKeyInfo answer = Console.ReadKey(false);
						Console.WriteLine();
						
						if (answer.KeyChar == 'y' || answer.KeyChar == 'Y')
						{
							proccessFile = true;
							hasAnswer = true;
						}
						else if (answer.KeyChar == 'n' || answer.KeyChar == 'N')
						{
							proccessFile = false;
							hasAnswer = true;
						}
					}
				}
				else
				{
					proccessFile = true;
				}

				if (proccessFile)
				{
					/// NOTE Change to LogFile instead of Console!
					Console.WriteLine(string.Format("Running processor type {0}.", LineProcessor.GetType().Name));

					FileProcessor.ProcessImportFile(FileProcessor.Log, LineProcessor, e.FullPath, ArchiveFolder, ErrorFolder);
				}
			}
		}
	}
}
