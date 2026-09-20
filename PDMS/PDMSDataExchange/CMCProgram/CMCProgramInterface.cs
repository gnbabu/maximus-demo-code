using Corp.Core.Libraries;
using FileHelpers;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace MAXIMUS.DataExchange.PDMS.CMCProgram
{

    public class CMCProgramInterface : BaseJob, IJob
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

        public CMCProgramInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "33C07935-7909-4E1B-B640-0487FF86B3E4";

        override public void ExecuteJob()
        {
            // Default Job - ICD Process ODA Waiver Services File
            this.ExecuteJob(Guid.Parse("33C07935-7909-4E1B-B640-0487FF86B3E4"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    this.ThreadId = new Guid(appID);
                    ImportCMCProgramFile();

                    break;
            }
        }

        public bool ImportCMCProgramFile()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            int filePrimaryKey;
            bool returnVal = false;

            try
            {
                log.CreateLogEntry(Constants.LogString.LoadingConfig, +logCnt);

                string fileWildcard = AppSettings.Get("CMC-ProgramImportFileWildcard");
                DirectoryInfo localDirectory;
                string localPath = AppSettings.Get("CMC-ImportLocalPath");
                localDirectory = new DirectoryInfo(localPath);

                log.CreateLogEntry(String.Format(Constants.LogString.FileCountToProcess, localDirectory.GetFiles(fileWildcard).Length));

                // interate over the downloaded files
                foreach (FileInfo file in localDirectory.GetFiles(fileWildcard))
                {
                    this.CurrentFile = file;
                    filePrimaryKey = CMCProgramHelper.CheckImportFileToStaging(log, this.ThreadId, file);

                    if (filePrimaryKey != 0)
                    {
                        log.CreateLogEntry(String.Format(Constants.LogString.LoadingFile, file.Name));
                        LoadCMCProgramFile(log, file, filePrimaryKey, this.ThreadId);
                        returnVal = true;
                    }
                    else
                    {
                        log.CreateLogEntry(String.Format(Constants.LogString.LoadingSkipped, file.Name), +logCnt);
                    }

                    //Send Invitation Letters


                    // move the file
                    string archiveDirectory = localPath + @"Archive\";
                    Directory.CreateDirectory(archiveDirectory);
                    if (File.Exists(archiveDirectory + file.Name))
                    {
                        string newFileName = CMCProgramHelper.RenameFileMethod(archiveDirectory, file.Name);
                        file.MoveTo(archiveDirectory + newFileName);                        
                    }
                    else
                    {
                        file.MoveTo(archiveDirectory + file.Name);
                    }
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

        private void LoadCMCProgramFile(Logging log, FileInfo file, int fileID, Guid threadID)
        {
            string fileName = file.Name;

            // create log entry
            log.CreateLogEntry(String.Format("Loading return file {0}", fileName));

            // open the file with FileHelper class and set internal variables
            
            CMCProgramServiceRecords[] records;            
            List<CMCProgramServiceRecords> lstRecords = new List<CMCProgramServiceRecords>();
            FileHelperEngine engine = new FileHelperEngine(typeof(CMCProgramServiceRecords));

            try
            {
                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart));
                DataTable dt = new DataTable();
                string[] strfileLines = File.ReadAllLines(file.FullName);

                int fileRecordsCount = 0;
                if (strfileLines.Length > 0)
                {
                    string strfirstLine = strfileLines[0];
                    string[] headerLabels = strfirstLine.Split('|');
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }
                    for (int i = 1; i < strfileLines.Length; i++)
                    {
                        fileRecordsCount++;
                        string[] dataWords = strfileLines[i].Split('|');
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }
                        dt.Rows.Add(dr);

                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstRecords.Add(new CMCProgramServiceRecords
                    {
                        PracticeMedicaidID = dt.Rows[i][0].ToString(),
                        FederalTaxID = dt.Rows[i][1].ToString(),
                        PracticeServiceAddr1 = dt.Rows[i][2].ToString(),
                        PracticeServiceAddr2 = dt.Rows[i][3].ToString(),
                        PracticeServiceCity = dt.Rows[i][4].ToString(),
                        PracticeServiceState = dt.Rows[i][5].ToString(),
                        PracticeServiceZip = dt.Rows[i][6].ToString(),
                        QualEnrollCount = dt.Rows[i][7].ToString()
                    });
                }



                // Read records from file
                engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                records = engine.ReadFile(file.FullName) as CMCProgramServiceRecords[];

                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, fileRecordsCount.ToString()
                    , engine.ErrorManager.ErrorCount.ToString()));

              CMCProgramHelper.SaveCMCProgramRecords(lstRecords, fileID, threadID, log);

                // record all bad errors
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        CMCProgramHelper.LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                CMCProgramHelper.LogFileRecordError(log, engine.LineNumber.ToString(), fileName, Constants.LogString.FileLoadFailure + ex.Message);
                // do not rethrow exception so the next file is processed
            }

            // create log entry
            log.CreateLogEntry(String.Format("Return provider {0} file load complete", fileName));
        }
    }
}
