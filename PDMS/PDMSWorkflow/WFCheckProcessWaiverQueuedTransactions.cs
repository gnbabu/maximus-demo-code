using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFCheckProcessWaiverQueuedTransactions : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckProcessWaiverQueuedTransactions(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int regID;
            Process p = new Process(ProcessID);
            try
            {
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out regID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                sqlParms.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, false));
                sqlParms.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, false));
                sqlParms.Add(SqlParms.CreateParameter("USER", DbType.Guid, CON.appAdminUserId, false));
                sqlParms.Add(SqlParms.CreateParameter("LOG_THREAD_NUMBER", DbType.Guid, LogThreadID, false));
                string isQueued = Convert.ToString(DataAccess.ExecuteStoredProcedure("usp_CheckAndProcessWaiverQueuedTrans", sqlParms, "IsQueued", SqlDbType.VarChar, 100));
                
                if (isQueued == "Processed")
                {
                    nextStep = "Processed";
                    toReturn = true;
                }
                else if (isQueued == "Complete Workflow")
                {
                    nextStep = "Complete Workflow";
                    toReturn = true;
                }
                else if (isQueued == "Wait")
                {
                    nextStep = "";
                    toReturn = false;
                }
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return toReturn;
        }
        override public string NextStep()
        {
            return nextStep;
        }
    }
}

