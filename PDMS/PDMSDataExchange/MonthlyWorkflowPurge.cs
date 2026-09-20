using MAXIMUS.Core.Libraries;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    public class MonthlyWorkflowPurge : BaseJob, IJob
    {
        private Logging log = null;

        public MonthlyWorkflowPurge(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "3260478F-74F3-469B-B3B0-3B4A450453F9";

        override public void ExecuteJob()
        {
            // Default Job - Delegate Affiliation File Processing
            this.ExecuteJob(Guid.Parse(appID));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    PurgeWorkflowApplications();
                    break;
            }
        }

        private void PurgeWorkflowApplications()
        {
            string logMsg = string.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(appID), logMsg);

            try
            {
                log.CreateLogEntry("Starting workflow purge");

                bool enablePurgeWorkflow = Convert.ToBoolean(AppSettings.Get("Workflow-Purge-Enable", "false"));

                if (enablePurgeWorkflow)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    DataAccess.ExecuteStoredProcedure("usp_PurgeWorkflowApplications", parameters);
                }
                else
                {
                    log.CreateLogEntry("Workflow purging not enabled");
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }

            log.CreateLogEntry(Constants.LogString.ProcessingComplete);
        }
    }
}
