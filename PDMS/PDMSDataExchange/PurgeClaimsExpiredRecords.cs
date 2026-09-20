using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS
{
    class PurgeClaimsExpiredRecords : BaseJob, IJob
    {

        public PurgeClaimsExpiredRecords(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse("42998BA3-B9AC-4015-8D8F-A3F6ED905261"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                case "42998BA3-B9AC-4015-8D8F-A3F6ED905261":                    
                    DailyRecordsUpdate();
                    DailyRecordsDelete();
                    break;
            }

        }
        public static void DailyRecordsDelete()
        {
            int logCnt = 0;
            Guid ThreadId = Guid.NewGuid();
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(ThreadId, logMsg);

            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet dsMembers = new DataSet();
                dsMembers = DataAccess.ExecuteStoredProcedure("Usp_Claims_Delete_Records_Service");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("MAXIMUS.Core.Libraries.Scheduler.Main failed. Reason: " + ex.Message, Logging.LogPriority.Error, +logCnt);
            }

        }
        public static void DailyRecordsUpdate()
        {
            int logCnt = 0;
            Guid ThreadId = Guid.NewGuid();
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(ThreadId, logMsg);
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet dsMembers = new DataSet();
                dsMembers = DataAccess.ExecuteStoredProcedure("Usp_Claims_Update_Records_Service");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("MAXIMUS.Core.Libraries.Scheduler.Main failed. Reason: " + ex.Message, Logging.LogPriority.Error, +logCnt);

            }

        }
    }
}
