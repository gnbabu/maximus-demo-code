using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.MCPN
{
    public class MCPMainImport : BaseJob, IJob
    {
        private const string thisGuidString = "054D9A25-C8DF-4CA1-AAD6-283FECD19E82";
        private const string weeklyGuidString = "2D80C32C-D971-49D7-90F7-69319A71E815";
        private Guid thisGuid;//= new Guid(thisGuidString);
        private string[] plans = new string[] { "145", "192", "293", "420", "315", "191", "731", "761" };
        Guid threadId = Guid.NewGuid();
       
        #region "Constructors"
        public MCPMainImport(Guid threadId): base(threadId)
        {
            this.ThreadId = threadId;
            this.thisGuid = new Guid(MCPMainImport.thisGuidString);
        }

        public MCPMainImport(Guid threadId, string guid = thisGuidString)
            : base(threadId)
        {
            this.ThreadId = threadId;
            this.thisGuid = new Guid(guid);
        }

        #endregion

        #region "Logging Objects"

        private int logCnt = 0;

        #endregion

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(thisGuid);
        }

        override public void ExecuteJob(Guid jobId)
        {
            switch (jobId.ToString().ToUpper())
            {
                case weeklyGuidString:
                    this.RunWeekly();
                    break;
                default:
                    this.RunJobs();
                    break;
            }
        }
        public void RunWeekly()
        {
            MCPAffiliationImport maFile = new MCPAffiliationImport(threadId, weeklyGuidString);
            maFile.ExecuteJob();
            MCPFacilityAffiliationImport faFile = new MCPFacilityAffiliationImport(threadId, weeklyGuidString);
            faFile.ExecuteJob();
            MCPProviderGroupImport pgFile = new MCPProviderGroupImport(threadId, weeklyGuidString);
            pgFile.ExecuteJob();
            MCPServiceLocationImport slFile = new MCPServiceLocationImport(threadId, weeklyGuidString);
            slFile.ExecuteJob();
            ExportWeeklySummaries();
        }

        public void ExportWeeklySummaries()
        {
            string exportType = "HP";
            foreach (string plan in plans)
            {
                MCPShared.ExportWeeklyRecords(plan, exportType);
            }

            exportType = "HC";
            foreach (string plan in plans)
            {
                MCPShared.ExportWeeklyRecords(plan, exportType);
            }

            exportType = "NF";
            foreach (string plan in plans)
            {
                MCPShared.ExportWeeklyRecords(plan, exportType);
            }

            exportType = "EN";
            foreach(string plan in plans)
            {
                MCPShared.ExportWeeklyRecords(plan, exportType);
            }
			
			exportType = "IN";
            foreach (string plan in plans)
            {
                MCPShared.ExportWeeklyRecords(plan, exportType);
            }
        }

        private void RunJobs()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(threadId, logMsg);

            log.CreateLogEntry(String.Format("Checking for MCPEndTransmission file"));
            MCPEndTransmissionImport mcpeti = new MCPEndTransmissionImport(threadId);
            IList<FileInfo> files = mcpeti.ImportMCPEndTransmissionImport();

            foreach (FileInfo file in files)
            {
                string planId = MCPShared.GetPlanIdFromFileName(threadId, file);
                string fileDate = MCPShared.GetFileDateFromName(threadId, file).ToString("yyyyMMdd");
                string fileMask = planId + fileDate;

                log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileComplete, "MCPEndTransmission [fileMask=" + fileMask + "]"));

                /*MCPAffiliationImport mcpai = new MCPAffiliationImport(threadId);
                if (mcpai.ImportMCPAffiliation(fileMask))
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileComplete, "MCPAffiliation [fileMask=" + fileMask + "]"));
                }
                else
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileNotFound, "MCPAffiliation [fileMask=" + fileMask + "]"), Logging.LogPriority.DataLoadIssues);
                }*/

                MCPFacilityAffiliationImport mcpfai = new MCPFacilityAffiliationImport(threadId);
                if (mcpfai.ImportMCPFacilityAffiliation(fileMask))
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileComplete, "MCPFacilityAffiliation [fileMask=" + fileMask + "]"));
                }
                else
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileNotFound, "MCPFacilityAffiliation [fileMask=" + fileMask + "]"), Logging.LogPriority.DataLoadIssues);
                }

                MCPProviderGroupImport mcppgi = new MCPProviderGroupImport(threadId);
                if (mcppgi.ImportMCPProviderGroupImport(fileMask))
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileComplete, "MCPProviderGroup [fileMask=" + fileMask + "]"));

                    MCPServiceLocationImport mcpsli = new MCPServiceLocationImport(threadId);
                    if (mcpsli.ImportMCPServiceLocation(fileMask))
                    {
                        log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileComplete, "MCPServiceLocation [fileMask=" + fileMask + "]"));

                        this.ExportMCPEndTransmission(file);
                    }
                    else
                    {
                        log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileNotFound, "MCPServiceLocation [fileMask=" + fileMask + "]"), Logging.LogPriority.DataLoadIssues);
                    }
                }
                else
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileNotFound, "MCPProviderGroup [fileMask=" + fileMask + "]"), Logging.LogPriority.DataLoadIssues);
                }
            }

            if (files.Count == 0)
            {
                log.CreateLogEntry(String.Format("No MCPEndTransmission files found, MCP import process stopped"), Logging.LogPriority.DataLoadIssues);
            }

            log.CreateLogEntry(Constants.LogString.ProcessingComplete, +logCnt);
        }

        private void ExportMCPEndTransmission(FileInfo file)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                log.CreateLogEntry(Constants.LogString.LoadingConfig, +logCnt);

                string planId = MCPShared.GetPlanIdFromFileName(this.ThreadId, file);

                DirectoryInfo localDirectory;
                string localPath = AppSettings.Get("MCP-ExportLocalPath");
                localDirectory = new DirectoryInfo(String.Format(localPath, planId));

                if (!localDirectory.Exists)
                {
                    localDirectory.Create();
                }

                string exportFile = "";
                if (file.Extension == MCPShared.defaultMCPExtension)
                {
                    exportFile = file.Name.Replace(MCPShared.defaultMCPExtension, MCPShared.defaultResponseExtension);
                }
                else if (file.Extension == MCPShared.defaultMCPExtensionUpperCase)
                {
                    exportFile = file.Name.Replace(MCPShared.defaultMCPExtensionUpperCase, MCPShared.defaultResponseExtension);
                }

                string fullExportFileName = Path.Combine(localDirectory.FullName, exportFile);

                FileInfo fi = new FileInfo(fullExportFileName);
                fi.Create();
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }
    }
}
