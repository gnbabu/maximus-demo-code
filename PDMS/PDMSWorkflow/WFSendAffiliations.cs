using MAXIMUS.Core.Libraries;
using System;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFSendAffiliations : BaseWorkflowTask, IWorkflowTask
    {
        public WFSendAffiliations(int processID, int stepID)
            : base(processID, stepID)
        {
            //
            // TODO: Add constructor logic here
            //
        }
        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            //bool toReturn = false;

            string returnVal = string.Empty;
            Process p = new Process(ProcessID);
            //TODO REGTOLIVE
            //try
            //{
            //    if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out regID))
            //    {
            //        throw new Exception(string.Format(
            //            Constants.LogString.WorkflowParameterNotNumeric,
            //            Assembly.GetExecutingAssembly().GetName().Name,
            //            Constants.ProcessParameter.RegistrationID,
            //            GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
            //    }

            //    List<SqlParameter> sqlParms = new List<SqlParameter>();
            //    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
            //    sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
            //    sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, false));
            //    DataAccess.ExecuteStoredProcedure("usp_UpdateGroupAffiliations", sqlParms);

            //}
            //catch (Exception ex)
            //{
            //    throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            //}
            return true;
        }
        override public string NextStep()
        {
            return null;
        }
    }
}

