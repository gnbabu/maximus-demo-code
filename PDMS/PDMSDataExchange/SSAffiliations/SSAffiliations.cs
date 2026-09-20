using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.SSAffiliations
{
    public class SSAffiliations : BaseJob, IJob
    {

        private Logging log = null;
        private readonly string appID = "8B705DBD-5422-46E3-BFF5-D707B144C97E";

        public SSAffiliations(Guid threadId) : base(threadId)
        {
            this.ThreadId = Guid.Parse("8B705DBD-5422-46E3-BFF5-D707B144C97E");
        }

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(Guid.Parse("8B705DBD-5422-46E3-BFF5-D707B144C97E"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                case "8B705DBD-5422-46E3-BFF5-D707B144C97E":
                    ProcessSSAffiliations();
                    break;
            }
        }

        public void ProcessSSAffiliations()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(appID), logMsg);
            try
            {
                log.CreateLogEntry(Constants.LogString.SSAffiliation_Processing);
                SSAffiliationsHelper.ProcessSSAffiliationsRecords(log, new Guid(appID));
                log.CreateLogEntry(Constants.LogString.SSAffiliation_Ended);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("SSAffiliations Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);
        }

    }
}
