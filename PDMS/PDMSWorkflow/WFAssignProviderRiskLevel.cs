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
    public class WFAssignProviderRiskLevel : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        public WFAssignProviderRiskLevel(int processID, int stepID)
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

            bool toReturn = false;

            int registrationId = 0;
            try
            {
                Process p = new Process(ProcessID);
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out registrationId))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, CON.appAdminUserId, true));
                DataAccess.ExecuteStoredProcedure("usp_AssignProviderRiskLevel", parameters);

                string RevertSuspension = p.GetProcessParameter(Constants.ProcessParameter.IsRevertSuspension);
                bool isRevertSuspension = string.IsNullOrEmpty(RevertSuspension) ? false : Convert.ToBoolean(RevertSuspension);

                if (isRevertSuspension)
                    nextStep = "Screening";
                else
                    nextStep = "Next";
                toReturn = true;

            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, "Registration Id: " + registrationId.ToString() + " - " + logMsg);
            }
            return toReturn;
        }

        override public string NextStep()
        {
            return nextStep;
        }
    }
}
