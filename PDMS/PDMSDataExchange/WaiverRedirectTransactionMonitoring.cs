using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using static MAXIMUS.Core.Libraries.Constants;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS
{
    public class WaiverRedirectTransactionMonitoring :BaseJob,IJob
    {
        private Logging log = null;
        private readonly string appID = "FC023C37-86F1-48AA-94A5-6F473BBF2A5D";
        public WaiverRedirectTransactionMonitoring(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }
        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(Guid.Parse("FC023C37-86F1-48AA-94A5-6F473BBF2A5D"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                case "FC023C37-86F1-48AA-94A5-6F473BBF2A5D":
                    ProcessTransactionMonitoring();
                    break;
            }
        }

        public void ProcessTransactionMonitoring()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(appID), logMsg);
            try
            {
                log.CreateLogEntry(Constants.LogString.TransactionMonitoring_Processing);
                DataAccess.ExecuteStoredProcedure("usp_ResendWaiverTransaction", "ResendWaiverTransaction");
                log.CreateLogEntry(Constants.LogString.TransactionMonitoring_Ended);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("TransactionMonitoring Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);
        }



    }
}
