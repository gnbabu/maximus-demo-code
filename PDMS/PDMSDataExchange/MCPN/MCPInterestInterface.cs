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

namespace MAXIMUS.DataExchange.PDMS.MCPN
{
    class MCPInterestInterface : BaseJob, IJob
    {

        private Logging log = null;
        private readonly string appID = "9090DF90-DEA0-44EA-816C-8B14FC569D47";
        private string[] plans = new string[] { "145", "315", "325", "420", "731", "761" };

        public MCPInterestInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        override public void ExecuteJob()
        {
            // Default Job - MCP Interest File Generation
            this.ExecuteJob(Guid.Parse(appID.ToString()));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    GenerateInterestFiles();
                    break;
            }
        }

        public void GenerateInterestFiles()
        {
            try
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Guid threadID = new Guid(appID);
                log = new Logging(threadID, logMsg);
                DataTable dt = MCPShared.SelectMCP_PLANS_SUBMITTER_DETAILS(log, threadID).Tables[0];
                
                foreach (DataRow row in dt.Rows)
                {
                    string planNumber = row["SUBMITTER_ID"].ToString();
                    int submitterID = Convert.ToInt32(row["MCP_PLANS_SUBMITTER_DETAILS_ID"].ToString());
                    ProcessMCPInterestFile(log, threadID, planNumber, submitterID);                    
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("MCP Interest File Exception: Error while generating the file {0}", ex.ToString()));
            }
        }

        public void ProcessMCPInterestFile(Logging log, Guid threadID, string planNumber, int submitterID)
        {
            try
            {
                DirectoryInfo localDirectory;
                string extractType = "IN";
                string localPath = AppSettings.Get("MCP-ExportLocalPath");
                localDirectory = new DirectoryInfo(String.Format(localPath, planNumber));
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    localDirectory = new DirectoryInfo(String.Format(@"C:\Projects\Deployments\PDMS\PNM_DEV\MCPInterestFile\{0}", planNumber));
                }
                if (!localDirectory.Exists)
                {
                    localDirectory.Create();
                }
                
                string exportFileName = extractType + planNumber + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                
                //stage the data
                int fileID = MCPShared.ProcessSTG_MCPInterestFile(log, threadID, exportFileName, localDirectory.ToString(), submitterID);
                
                if (fileID != 0)
                {
                    DataTable dt = MCPShared.SelectMCPInterestFileDataByFileID(log, threadID, fileID).Tables[0];
                    MCPShared.CreateCSV(dt, localDirectory.ToString(), exportFileName);
                }
                else
                {
                    log.CreateLogEntry(String.Format("MCP Interest File, no data was staged"), Logging.LogPriority.Information);
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("MCP Interest File Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(ex);
            }
        }

    }
}
