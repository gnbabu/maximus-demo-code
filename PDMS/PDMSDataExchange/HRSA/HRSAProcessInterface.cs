

using MAXIMUS.Core.Libraries;
using System;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.HRSA
{
    public class HRSAProcessInterface : BaseJob, IJob
    {
        private int logCnt = 0;
        private Logging log = null;

        FileInfo _currentFile;

        FileInfo CurrentFile
        {
            get
            {
                return _currentFile;
            }
            set
            {
                _currentFile = value;
            }
        }

        public HRSAProcessInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "583908F2-723D-4659-BD4C-02BAD588E143";

        public override void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse("583908F2-723D-4659-BD4C-02BAD588E143"));
        }

        public override void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    this.ThreadId = new Guid(appID);
                    ImportHRSAFile();
                    break;
            }
        }

        private void ImportHRSAFile()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(ThreadId, logMsg);

            try
            {
                log.CreateLogEntry(Constants.LogString.LoadingConfig, +logCnt);

                string fileWildcard = AppSettings.Get("HRSA-ImportFileWildcard");
                string localPath = AppSettings.Get("HRSA-ImportFileLocalPath");

                DirectoryInfo localDirectory;
                localDirectory = new DirectoryInfo(localPath);

                log.CreateLogEntry(String.Format(Constants.LogString.FileCountToProcess, localDirectory.GetFiles(fileWildcard).Length));

                //Clear HRSA staging data before importing new files. 
                HRSAProcessHelper.ClearHRSAStagingData();

                // interate over the downloaded files
                foreach (FileInfo file in localDirectory.GetFiles(fileWildcard))
                {
                    HRSAProcessHelper.ReadExcelAndStageData(log, ThreadId, file);

                    // Archive the file
                    string archiveDirectory = localPath + @"Archive\";
                    Directory.CreateDirectory(archiveDirectory);
                    if (File.Exists(archiveDirectory + file.Name))
                    {
                        string newFileName = HRSAProcessHelper.RenameFileMethod(archiveDirectory, file.Name);
                        file.MoveTo(archiveDirectory + newFileName);
                    }
                    else
                    {
                        file.MoveTo(archiveDirectory + file.Name);
                    }
                }
                log.CreateLogEntry(Constants.LogString.ProcessingComplete, +logCnt);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("ExecuteJob failed. Reason: " + ex.Message, Logging.LogPriority.Error, +logCnt);
            }
        }
    }
}
