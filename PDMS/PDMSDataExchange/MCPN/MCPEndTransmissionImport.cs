using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.MCPN
{
    public class MCPEndTransmissionImport : BaseJob, IJob
    {

        private const string thisGuidString = "78D3F083-8B0E-4A12-8B19-D8055D20CB7E";
        private Guid thisGuid = new Guid(thisGuidString);

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

        string _planId;

        String PlanId
        {
            get
            {
                return _planId;
            }
            set
            {
                _planId = value;
            }
        }

        #region "Constructors"

        public MCPEndTransmissionImport(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        #endregion

        #region "Logging Objects"

        private int logCnt = 0;

        #endregion

        #region "Public Methods"

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(thisGuid);
        }

        override public void ExecuteJob(Guid jobId)
        {
            switch (jobId.ToString().ToUpper())
            {
                default:
                    this.ImportMCPEndTransmissionImport();
                    break;
            }
        }

        public IList<FileInfo> ImportMCPEndTransmissionImport()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            int filePrimaryKey;
            IList<FileInfo> returnVal = new List<FileInfo>();

            try
            {
                log.CreateLogEntry(Constants.LogString.LoadingConfig, +logCnt);

                string fileWildcard = AppSettings.Get("MCP-EndTransmissionImportFileWildcard");
                DirectoryInfo localDirectory;
                string localPath = AppSettings.Get("MCP-ImportLocalPath");
                Console.Write(localPath);
                localDirectory = new DirectoryInfo(localPath);

                log.CreateLogEntry(String.Format(Constants.LogString.LoadingDirectory, localDirectory.FullName));
                log.CreateLogEntry(String.Format(Constants.LogString.FileCountToProcess, localDirectory.GetFiles(fileWildcard).Length));

                // interate over the downloaded files
                foreach (FileInfo file in localDirectory.GetFiles(fileWildcard))
                {
                    this.CurrentFile = file;
                    this.PlanId = MCPShared.GetPlanIdFromFileName(this.ThreadId, file);

                    filePrimaryKey = MCPShared.FileImported(this.ThreadId, file);

                    if (filePrimaryKey != 0)
                    {
                        log.CreateLogEntry(String.Format(Constants.LogString.LoadingFile, file.Name));
                        //ExportMCPEndTransmission();
                        returnVal.Add(file);
                    }
                    else
                    {
                        log.CreateLogEntry(String.Format(Constants.LogString.LoadingSkipped, file.Name), +logCnt);
                    }

                    // move the file 
                    string planId = MCPShared.GetPlanIdFromFileName(this.ThreadId, file);
                    string archiveDirectory = localPath + @"Archive\" + String.Format(MCPShared.directoryFormat, planId);
                    Directory.CreateDirectory(archiveDirectory);
                    if (File.Exists(archiveDirectory + file.Name))
                    {
                        File.Delete(archiveDirectory + file.Name);
                    }
                    file.MoveTo(archiveDirectory + file.Name);
                }
            }
            catch (Exception ex)
            {
                //string fullMessage = "File action failed. File Name: {0} Reason: {1}";
                //log.CreateLogEntry(String.Format(fullMessage, fileName, ex.Message), Logging.LogPriority.Error, +logCnt);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete, +logCnt);

            return returnVal;
        }
        #endregion
    }
}
