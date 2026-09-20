using FileHelpers;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MMISInterface;
using System;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    public interface IMMISFilePath<TRequest>
	{
        string GetFilePath();
	}
    public interface IMMISSendFile
    {
        void SendFile(string fileName, Guid threadID);
    }
    public interface IMMISReadFile<TRequest>
    {
        TRequest[] ReadRecords(FileInfo file);
        FileInfo[] GetFiles();
    }
}

namespace MAXIMUS.DataExchange.PDMS.InterfaceFile
{
    public class SubmitFilePath : MMISTransactionStep, IMMISFilePath<SubmitWaiverProvider>
    {
        #region Constructors

        public SubmitFilePath(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

        public string GetFilePath()
        {
            // set the sFTP variables prior to retrieving file from the MMIS
            string localPath = AppSettings.Get("MMIS-SubmitWaiverProviderLocalPath");
            string fileName = AppSettings.Get("MMIS-SubmitWaiverProviderFile");
            string dtmWildcard = AppSettings.Get("MMIS-SubmitWaiverProviderFileWildcardDTM");

            fileName = fileName.Replace(dtmWildcard, DateTime.Now.ToString(dtmWildcard));

            // Create local path if it does not exist and append filename
            Directory.CreateDirectory(localPath);
            string fullFileName = localPath + fileName;

            return fullFileName;
        }
    }

    public class RetrieveIndividualProviderRecords : RetrieveProviderRecords
    {
        #region Constructors

        public RetrieveIndividualProviderRecords(Guid threadID)
            : base(threadID)
        {

        }

        #endregion

        protected override void MoveFileToArchive(string sourceFilePath, string destinationFilePath)
        {
            // Do nothing for individuals, because the same file will be needed by the group
        }
    }
    public class RetrieveProviderRecords : MMISTransactionStep, IMMISReadFile<RetrieveWaiverProvider>
    {
        #region Constructors

        public RetrieveProviderRecords(Guid threadID)
            : base(threadID)
        {

        }

        #endregion
        public FileInfo[] GetFiles()
        {
            DateTime now = DateTime.Now;
            string testingEnabled = AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower();
            FileInfo[] files = null;

            // do nothing if testing enabled
            if (testingEnabled == bool.FalseString.ToLower())
            {
                // retrieve and set required variables
                string localPath = AppSettings.Get("MMIS-ReturnWaiverProviderLocalPath");
                string fileName = AppSettings.Get("MMIS-ReturnWaiverProviderFile");
                string dtmWildcard = AppSettings.Get("MMIS-ReturnWaiverProviderFileWildcardDTM");
                DirectoryInfo localDirectory;
                string archiveFolder = localPath + AppSettings.Get("MMIS-ArchinveFolderName");

                // create archive path if it does not exist
                if (!Directory.Exists(archiveFolder))
                {
                    Directory.CreateDirectory(archiveFolder);
                }

                // Get file name
                fileName = fileName.Replace(dtmWildcard, now.ToString(dtmWildcard));

                // create local path if it does not exist
                Directory.CreateDirectory(localPath);
                localDirectory = new DirectoryInfo(localPath);
                files = localDirectory.GetFiles();
            }
            return files;
        }

        public RetrieveWaiverProvider[] ReadRecords(FileInfo file)
        {
            RetrieveWaiverProvider[] records = null;
            string testingEnabled = AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower();
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadID, logMsg);

            // do nothing if testing enabled
            if (testingEnabled == bool.FalseString.ToLower() && file != null)
            {
                // Get file name
                string fileName = file.FullName;
                string archiveFolder = AppSettings.Get("MMIS-ReturnWaiverProviderLocalPath") + AppSettings.Get("MMIS-ArchinveFolderName");

                // create log entry
                log.CreateLogEntry(String.Format("Loading return file {0}", fileName));

                // open the file with FileHelper class and set internal variables
                FileHelperEngine engine = new FileHelperEngine(typeof(RetrieveWaiverProvider));

                try
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart));

                    // Read records from file
                    engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                    records = engine.ReadFile(file.FullName) as RetrieveWaiverProvider[];

                    // create log entry
                    log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString()
                        , engine.ErrorManager.ErrorCount.ToString()));

                    // record all bad errors
                    if (engine.ErrorManager.HasErrors)
                    {
                        foreach (ErrorInfo err in engine.ErrorManager.Errors)
                        {
                            LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                        }
                    }

                    this.MoveFileToArchive(file.FullName, archiveFolder + "\\" + file.Name);
                }
                catch (Exception ex)
                {
                    LogFileRecordError(log, engine.LineNumber.ToString(), fileName, Constants.LogString.FileLoadFailure + ex.Message);
                    CoreException.ThrowException(this.ThreadID, ex, log.ProcessName);
                }

                // create log entry
                log.CreateLogEntry(String.Format("Return provider {0} file load complete", fileName));
            }
            return records;
        }

        protected virtual void MoveFileToArchive(string sourceFilePath, string destinationFilePath)
        {
            File.Move(sourceFilePath, destinationFilePath);
        }

        private void LogFileRecordError(Logging log, string lineNumber, string fileName, string errorMessage)
        {
            string fullMessage = "Record failed. Line No: {0}; File: {1}; Reason: {2}";
            log.CreateLogEntry(String.Format(fullMessage, lineNumber, fileName, errorMessage), Logging.LogPriority.DataLoadIssues);
        }
    }

    public class SubmitProviderFile : MMISTransactionStep, IMMISSendFile
    {
        
        #region Constructors

        public SubmitProviderFile(Guid threadID)
			: base(threadID)
		{

		}
#endregion
        public void SendFile(string fileName, Guid threadID)
        {
            // set the sFTP variables prior to retrieving file from the MMIS
            string hostName = AppSettings.Get("MMIS-HostName");
            string remotePath = AppSettings.Get("MMIS-SendRemotePath");
            string userName = AppSettings.Get("MMIS-UserName");
            string password = AppSettings.Get("MMIS-Pwd");
            string localPath = AppSettings.Get("MMIS-SubmitLocalPath");
            string hostKey = AppSettings.Get("MMIS-sFTPHostKey");

            // Always send file to MMIS even though its empty
            // if testing is enabled
            if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
            {
                //  send the file to MMIS
                //FileTransport ft = new FileTransport(threadId);
                //ft.SetFileViaSFTP(localPath, fileName, hostName, remotePath, userName, password, hostKey);
            }
            else
            {
                // no action
            }
        }
    }

    public class SubmitWaiverServiceFilePath : MMISTransactionStep, IMMISFilePath<SubmitWaiverServices>
    {
        #region Constructors

        public SubmitWaiverServiceFilePath(Guid threadID)
            : base(threadID)
        {

        }

        #endregion

        public string GetFilePath()
        {
            // set the sFTP variables prior to retrieving file from the MMIS
            string localPath = AppSettings.Get("MMIS-SubmitWaiverServicesLocalPath");
            string fileName = AppSettings.Get("MMIS-SubmitWaiverServicesFile");
            string dtmWildcard = AppSettings.Get("MMIS-SubmitWaiverServicesFileWildcardDTM");

            fileName = fileName.Replace(dtmWildcard, DateTime.Now.ToString(dtmWildcard));

            // Create local path if it does not exist and append filename
            Directory.CreateDirectory(localPath);
            string fullFileName = localPath + fileName;

            return fullFileName;
        }
    }

    public class RetrieveWaiverServiceRecords : MMISTransactionStep, IMMISReadFile<RetrieveWaiverServices>
    {
        #region Constructors

        public RetrieveWaiverServiceRecords(Guid threadID)
            : base(threadID)
        {

        }

        #endregion
        public FileInfo[] GetFiles()
        {
            string testingEnabled = AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower();
            FileInfo[] files = null;

            // do nothing if testing enabled
            if (testingEnabled == bool.FalseString.ToLower())
            {
                // retrieve and set required variables
                string localPath = AppSettings.Get("MMIS-ReturnWaiverServicesLocalPath");
                string fileName = AppSettings.Get("MMIS-ReturnWaiverServicesFile");
                string dtmWildcard = AppSettings.Get("MMIS-ReturnWaiverServicesFileWildcardDTM");
                DirectoryInfo localDirectory;
                string archiveFolder = localPath + AppSettings.Get("MMIS-ArchinveFolderName");

                // create archive path if it does not exist
                if (!Directory.Exists(archiveFolder))
                {
                    Directory.CreateDirectory(archiveFolder);
                }

                // create local path if it does not exist
                Directory.CreateDirectory(localPath);
                localDirectory = new DirectoryInfo(localPath);
                files = localDirectory.GetFiles();
            }
            return files;
        }

        public RetrieveWaiverServices[] ReadRecords(FileInfo file)
        {
            RetrieveWaiverServices[] records = null;
            string testingEnabled = AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower();
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadID, logMsg);
            string fileName = file.FullName;

            // do nothing if testing enabled
            if (testingEnabled == bool.FalseString.ToLower() && file != null)
            {
                // create log entry
                log.CreateLogEntry(String.Format("Loading return file {0}", fileName));

                string archiveFolder = AppSettings.Get("MMIS-ReturnWaiverServicesLocalPath") + AppSettings.Get("MMIS-ArchinveFolderName");

                // open the file with FileHelper class and set internal variables
                FileHelperEngine engine = new FileHelperEngine(typeof(RetrieveWaiverServices));

                try
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart));

                    // Read records from file
                    engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                    records = engine.ReadFile(file.FullName) as RetrieveWaiverServices[];

                    // create log entry
                    log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString()
                        , engine.ErrorManager.ErrorCount.ToString()));

                    // record all bad errors
                    if (engine.ErrorManager.HasErrors)
                    {
                        foreach (ErrorInfo err in engine.ErrorManager.Errors)
                        {
                            LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFileRecordError(log, engine.LineNumber.ToString(), fileName, Constants.LogString.FileLoadFailure + ex.Message);
                    CoreException.ThrowException(this.ThreadID, ex, log.ProcessName);
                }

                // create log entry
                log.CreateLogEntry(String.Format("Return provider {0} file load complete", fileName));
                File.Move(file.FullName, archiveFolder + "\\" + file.Name);
            }
            return records;
        }

        private void LogFileRecordError(Logging log, string lineNumber, string fileName, string errorMessage)
        {
            string fullMessage = "Record failed. Line No: {0}; File: {1}; Reason: {2}";
            log.CreateLogEntry(String.Format(fullMessage, lineNumber, fileName, errorMessage), Logging.LogPriority.DataLoadIssues);
        }
    }
}
