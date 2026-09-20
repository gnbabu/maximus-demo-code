using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.GroupAffiliations
{
    public class GroupAffiliations : BaseJob, IJob
    {

        private Logging log = null;
        private readonly string appID = "495336C2-A0E8-44B4-9522-BD9E74AF1D98";

        public GroupAffiliations(Guid threadId) : base(threadId)
        {
            this.ThreadId = Guid.Parse("495336C2-A0E8-44B4-9522-BD9E74AF1D98");
        }

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(Guid.Parse("495336C2-A0E8-44B4-9522-BD9E74AF1D98"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                case "495336C2-A0E8-44B4-9522-BD9E74AF1D98":
                    ProcessGroupffiliations();
                    break;
            }
        }

        public void ProcessGroupffiliations()
        {
            //Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(appID), logMsg);
            try
            {
                log.CreateLogEntry(Constants.LogString.GroupAffiliation_Processing);
                //OHPNM - 14496 Update the group Aff Status from Ind Required Validation to Confirmed
                GroupAffiliationsHelper.UpdateGroupAffiliationStatus(log, new Guid(appID));
                //OHPNM-18268 Update group aff status to pending removal if Ind or group is termed but the aff is in active/confirmed status
                GroupAffiliationsHelper.UpdateGroupAffiliationtoPendingRemovalStatus(log, new Guid(appID));
                GroupAffiliationsHelper.ProcessGroupAffiliationsRecords(log, new Guid(appID));
                log.CreateLogEntry(Constants.LogString.GrroupAffiliation_Ended);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Group Affiliations Exception: {0}", ex.StackTrace), Logging.LogPriority.Error); 
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);
        }
    }
}
