using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace FileProcessorCore
{
	public abstract class LineRecordImportFileProcessor : BatchProcessor, IComparer<string>
	{
		#region Properties

		public static string DropFolder
		{
			get
			{
				return EWSConfiguration.AppSettings("DropFolder");
			}
		}

		public static string WorkingFolder
		{
			get
			{
				return EWSConfiguration.AppSettings("WorkingFolder");
			}
		}

		public static string ArchiveFolder
		{
			get
			{
				return EWSConfiguration.AppSettings("ArchiveFolder");
			}
		}

		public static string ErrorFolder
		{
			get
			{
				return EWSConfiguration.AppSettings("ErrorFolder");
			}
		}

		#endregion

		#region Constructors

		public LineRecordImportFileProcessor()
			: base()
		{

		}

		public LineRecordImportFileProcessor(LogFile log)
			: base(log)
		{

		}

		#endregion

		public abstract LineRecordImportLineProcessor GetLineProcessor(string filename);

		public abstract string BuildImportSummary(List<Record> lstRecords, LineRecordImportLineProcessor processor, string filename);

		public override void Execute()
		{
			Log.WriteVerbose("Begin scanning Drop Folder...");

			IEnumerable<string> lstDropFiles = Directory.EnumerateFiles(DropFolder).OrderBy(f => f, this);
			List<string> lstWorkingFiles = new List<string>();
			List<Record> lstImports = new List<Record>();

			if (!Directory.Exists(WorkingFolder))
			{
				Log.WriteVerbose(string.Format("Begin creating Working Folder: {0}.", WorkingFolder));

				Directory.CreateDirectory(WorkingFolder);

				Log.WriteVerbose("End creating Working Folder.");
			}

			Log.WriteVerbose(string.Format("End scanning Drop Folder. {0} files found.", lstDropFiles.Count()));


			Log.WriteVerbose("Begin moving files from Drop folder to Working folder...");

			foreach (string dropFilePath in lstDropFiles)
			{
				string filename = Path.GetFileName(dropFilePath);
				string workingFilePath = Path.Combine(WorkingFolder, filename);

				LineRecordImportLineProcessor lineProcessor = GetLineProcessor(filename);

				/// If a processor is found, it means a regular expression was matched and we should move this
				/// file out of the drop folder into the working folder and then work it.
				if (lineProcessor != null)
				{

					Log.WriteVerbose(string.Format("\tMoving {0} from {1} to {2}", filename, dropFilePath, workingFilePath));

					File.Move(dropFilePath, workingFilePath);

					lstWorkingFiles.Add(workingFilePath);
				}
			}

			Log.WriteVerbose("End moving files from Drop folder to Working folder...");

			string archiveDirectory = Path.Combine(ArchiveFolder, EWSConfiguration.CurrentDateTime.ToString("yyyyMMdd_HHmm_ss"));
			string errorDirectory = Path.Combine(ErrorFolder, EWSConfiguration.CurrentDateTime.ToString("yyyyMMdd_HHmm_ss"));

			Log.WriteVerbose("Begin creating archive directory...");

			Directory.CreateDirectory(archiveDirectory);

			Log.WriteVerbose("End creating archive directory.");


			foreach (string workingFile in lstWorkingFiles)
			{
				var importRecords = ProcessImportFile(Log, GetLineProcessor(workingFile), workingFile, archiveDirectory, errorDirectory);

				/// Note importRecords may be null if the file does not match any of the processor filename patterns.
				if (importRecords != null)
				{
					lstImports = lstImports.Concat(importRecords).ToList();
				}
				else
				{
					Log.WriteError(string.Format("No records found in file {0}", workingFile));
					HasErrors = true;
				}
			}

			if (lstImports != null)
			{
				HasErrors = HasErrors || lstImports.Any(p => !p.Success && !p.RecordFilteredOut);
			}
		}

		public List<Record> ProcessImportFile(LogFile log, LineRecordImportLineProcessor processor, string workingFile, string archiveDirectory, string errorDirectory)
		{
			List<Record> lstRecords = new List<Record>();

			string filename = Path.GetFileName(workingFile);
			
			if (processor == null)
			{
				return null;
			}

			log.WriteVerbose(string.Format("Begin processing file {0}...", workingFile));

			int lineNumber = 0;

			string errorFilePath = Path.Combine(errorDirectory, string.Format("{0}.errors.dat", filename));
			bool hasWrittenErrors = false;

			if (!Directory.Exists(errorDirectory))
			{
				log.WriteVerbose("Begin creating error directory...");

				Directory.CreateDirectory(errorDirectory);

				log.WriteVerbose("End creating error directory.");
			}

			
			
			using (StreamWriter errorFile = new StreamWriter(errorFilePath))
			{
				using (GenericFileReader reader = new GenericFileReader(workingFile, processor.Format))
				{
					if (!reader.EndOfStream && processor.IgnoreHeader)
					{
						if (processor.Format == FileFormat.FixedLength)
						{
							reader.ReadLine();
						}
						else if (processor.Format == FileFormat.CSV)
						{
							reader.ReadFields();
						}
					}

					while (!reader.EndOfStream)
					{
						lineNumber++;

						string text = null;
						string[] textArray = null;

						if (processor.Format == FileFormat.FixedLength)
						{
							text = reader.ReadLine();
                            if (text.Length != processor.LineLength)
                            {
                                string spaces = new string(' ', processor.LineLength - text.Length);
                                Console.WriteLine("Line Number = " + lineNumber + " Length = " + text.Length);
                                text += spaces;
                            }
						}
						else if (processor.Format == FileFormat.CSV)
						{
							textArray = reader.ReadFields();
						}
						log.WriteVerbose(string.Format("\tLine #{0} Contents: {1}", lineNumber, text));

						if (!string.IsNullOrEmpty(text) || textArray != null)
						{
							Record record = null;
							try
							{
								record = processor.ProcessLine(workingFile, lineNumber, text, textArray);

								if (record.IsValid)
								{
									record.Success = true;
								}
								else
								{
									if (!record.RecordFilteredOut)
									{
										hasWrittenErrors = true;
										errorFile.WriteLine(text);
									}
								}

								log.WriteVerbose(string.Format("End processing line #{0} of {1}...", lineNumber, workingFile));
							}
							catch (Exception ex)
							{
								if (record != null)
								{
									record.Success = false;
									record.Errors.Add(string.Format("Error at line #{0}: {1}", lineNumber, ex.Message));
								}
							}

							lstRecords.Add(record);
						}
					}
				}
			}

			log.WriteVerbose(string.Format("End processing file {0}...", workingFile));


			if (!string.IsNullOrEmpty(archiveDirectory))
			{

				if (!Directory.Exists(archiveDirectory))
				{
					log.WriteVerbose("Begin creating archive directory...");

					Directory.CreateDirectory(archiveDirectory);

					log.WriteVerbose("End creating archive directory.");
				}

				string archiveFile = Path.Combine(archiveDirectory, filename);

				log.WriteVerbose(string.Format("Begin moving file {0} to {1}...", workingFile, archiveFile));

				File.Move(workingFile, archiveFile);

				log.WriteVerbose(string.Format("End moving file {0} to {1}.", workingFile, archiveFile));
			}

			if (!hasWrittenErrors)
			{
				log.WriteVerbose(string.Format("No errors encountered, deleting empty error file: {0}", errorFilePath));
				File.Delete(errorFilePath);
			}

			string importSummary = BuildImportSummary(lstRecords, processor, filename);

			Log.Write(importSummary);

			return lstRecords;
		}

		private List<string> _fakeNames;

		public List<string> FakeNames
		{
			get
			{
				if (_fakeNames == null)
				{
					_fakeNames = new List<string>();

					using (StreamReader reader = new StreamReader("Resources\\FakeNames.txt"))
					{
						while (!reader.EndOfStream)
						{
							_fakeNames.Add(reader.ReadLine());
						}
					}
				}

				return _fakeNames;
			}
		}

		private List<string> _fakePhoneNumbers;

		public List<string> FakePhoneNumbers
		{
			get
			{
				if (_fakePhoneNumbers == null)
				{
					_fakePhoneNumbers = new List<string>();

					using (StreamReader reader = new StreamReader("Resources\\FakePhoneNumbers.txt"))
					{
						while (!reader.EndOfStream)
						{
							_fakePhoneNumbers.Add(reader.ReadLine());
						}
					}
				}

				return _fakePhoneNumbers;
			}
		}

		private List<string> _fakeEmails;

		public List<string> FakeEmails
		{
			get
			{
				if (_fakeEmails == null)
				{
					_fakeEmails = new List<string>();

					using (StreamReader reader = new StreamReader("Resources\\FakeEmails.txt"))
					{
						while (!reader.EndOfStream)
						{
							_fakeEmails.Add(reader.ReadLine());
						}
					}
				}

				return _fakeEmails;
			}
		}

		private List<string> _fakeAddresses;

		public List<string> FakeAddresses
		{
			get
			{
				if (_fakeAddresses == null)
				{
					_fakeAddresses = new List<string>();

					using (StreamReader reader = new StreamReader("Resources\\FakeAddresses.txt"))
					{
						while (!reader.EndOfStream)
						{
							_fakeAddresses.Add(reader.ReadLine());
						}
					}
				}

				return _fakeAddresses;
			}
		}


		public string GetFakeFullName(Random r)
		{
			return FakeNames[r.Next(0, FakeNames.Count)].Replace(",", " ").ToUpper();
		}

		public string GetFakeFirstName(Random r)
		{
			return FakeNames[r.Next(0, FakeNames.Count)].Split(',')[0].ToUpper();
		}

		public string GetFakeMiddleName(Random r)
		{
			return FakeNames[r.Next(0, FakeNames.Count)].Split(',')[1].ToUpper();
		}

		public string GetFakePhoneNumber(Random r)
		{
			return FakePhoneNumbers[r.Next(0, FakePhoneNumbers.Count)].ToUpper();
		}

		public string GetFakeLastName(Random r)
		{
			return FakeNames[r.Next(0, FakeNames.Count)].Split(',')[2].ToUpper();
		}

		public bool GenerateTestFiles(LineRecordImportLineProcessor lineProcessor, string dropFolder, int numberOfRecords)
		{
			try
			{
				Random r = new Random();

				string filepath = Path.Combine(dropFolder, lineProcessor.TestFilenameTemplate.Replace("{Rand}", r.Next(10000, 9999999).ToString()));

				using (StreamWriter file = new StreamWriter(filepath, false))
				{
					

					/// EmpVer test records.
					for (int i = 0; i < numberOfRecords; i++)
					{

						foreach (var field in lineProcessor.TemplateFields)
						{
							if (field.Type == DataType.Filler)
							{
								file.Write(new string(' ', field.Length));
							}
							else
							{
								string testValue = field.TestValue;


								if (testValue == "{FullName}")
								{
									testValue = GetFakeFullName(r);
								}
								else if (testValue == "{FirstName}")
								{
									testValue = GetFakeFirstName(r);
								}
								else if (testValue == "{MiddleName}")
								{
									testValue = GetFakeMiddleName(r);
								}
								else if (testValue == "{LastName}")
								{
									testValue = GetFakeLastName(r);
								}
								else if (testValue == "{Date}")
								{
									testValue = GetFakeDate(r, field.Type);
								}
								else if (testValue == "{PhoneNumber}")
								{
									testValue = GetFakePhoneNumber(r);
								}
								else if (testValue == "{Address}")
								{
									testValue = GetFakeAddress(r);
								}
								else if (testValue == "{City}")
								{
									testValue = GetFakeCity(r);
								}
								else if (testValue == "{StateInitials}")
								{
									testValue = GetFakeStateInitials(r);
								}
								else if (testValue == "{PostalCodeFull}")
								{
									testValue = GetFakePostalCodeFull(r);
								}
								else if (testValue == "{ValidValue}" && field.ValidValues != null && field.ValidValues.Count() > 0)
								{
									testValue = field.ValidValues.ElementAt(r.Next(0, field.ValidValues.Count()));
								}
								else if (testValue == "{ValidValueOrBlank}" && field.ValidValues != null && field.ValidValues.Count() > 0)
								{
									if (r.Next(0, 2) == 1)
									{
										testValue = field.ValidValues.ElementAt(r.Next(0, field.ValidValues.Count()));
									}
									else
									{
										testValue = string.Empty;
									}
								}
								else if (field.RandomNumberDigits > 0)
								{

									testValue = r.Next((int)Math.Pow(10, field.RandomNumberDigits - 1), (int)Math.Pow(10, field.RandomNumberDigits) - 1).ToString();
								}
								else if (!string.IsNullOrEmpty(field.TestRandomNumberFormat))
								{
									testValue = field.TestRandomNumberFormat;

									for (int j = 0; j < testValue.Length; j++)
									{
										if (testValue[j] == 'd')
										{
											testValue = testValue.Remove(j, 1).Insert(j, r.Next(j == 0 ? 1 : 0, 10).ToString());
										}
									}
								}


								if (testValue.Length > field.Length)
								{
									testValue = testValue.Substring(0, field.Length);
									// throw new Exception(string.Format("File '{0}', Field '{1}', test value is longer than length.", filepath, field.FieldName));
								}

								if (field.Type == DataType.Currency)
								{
									file.Write(testValue.PadRight(field.Length, '0'));
								}
								else
								{
									file.Write(testValue.PadRight(field.Length));
								}
							}
						}

						file.WriteLine();
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				Log.WriteError(string.Format("Error generating Test Files: {0}", ex));
				return false;
			}
			
		}

		private string GetFakePostalCodeFull(Random r)
		{
			string postalCode = FakeAddresses[r.Next(0, FakeAddresses.Count)].Split(',')[3];
			if (postalCode.Length < 5)
			{
				postalCode = new string('0', 5 - postalCode.Length) + postalCode;
			}
			if (r.Next(0, 2) == 1)
			{
				postalCode += r.Next(1000, 10000).ToString();
			}

			return postalCode;
		}

		private string GetFakeStateInitials(Random r)
		{
			return FakeAddresses[r.Next(0, FakeAddresses.Count)].Split(',')[2].ToUpper();
		}

		private string GetFakeCity(Random r)
		{
			return FakeAddresses[r.Next(0, FakeAddresses.Count)].Split(',')[1].ToUpper();
		}

		private string GetFakeAddress(Random r)
		{
			return FakeAddresses[r.Next(0, FakeAddresses.Count)].Split(',')[0].ToUpper();
		}

		private string GetFakeDate(Random r, DataType dataType)
		{
			bool validDate = false;

			DateTime? randDate = null;

			while (!validDate)
			{
				try
				{
					randDate = new DateTime(r.Next(1990, 2014), r.Next(1, 13), r.Next(1, 32));

					validDate = true;
				}
				catch (Exception)
				{

				}
			}
			if (randDate.HasValue)
			{
				switch (dataType)
				{
					case DataType.DateMMDDYYYY:
						return randDate.Value.ToString("MMddyyyy");
					case DataType.DateYYYYMMDD:
						return randDate.Value.ToString("yyyyMMdd");
					case DataType.DateDDMMYY:
						return randDate.Value.ToString("dd/MM/yy");
					case DataType.DateMMDDYYYYSlash:
						return randDate.Value.ToString("MM/dd/yyyy");
					case DataType.DateMMDDYYSlash:
						return randDate.Value.ToString("MM/dd/yy");
					case DataType.DateYYYYMMDDDash:
						return randDate.Value.ToString("yyyy-MM-dd");
					default:
						throw new ArgumentException(string.Format("Invalid DateTime datatype: {0}", dataType));
				}
			}
			else
			{
				throw new Exception("No date generated.");
			}							
		}

		public abstract int Compare(string filePathX, string filePathY);
	}
}
