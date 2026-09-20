using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    class WFDisEnrollmentCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFDisEnrollmentCheck(int processID, int stepID)
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

                bool isProviderDisenrolling = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.IsProviderDisenrolling))
                      ? false : Convert.ToBoolean(p.GetProcessParameter(Constants.ProcessParameter.IsProviderDisenrolling));

                if (isProviderDisenrolling)
                {
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_CheckDisEnrollmentByRegID", sqlParms, "RegDataRPD");
                    dsReg.Tables[0].TableName = "RegDataRPD";
                    if (dsReg.Tables[0].Rows.Count > 0)
                    {
                        nextStep = "Yes"; // GoTo Provider Review
                        toReturn = true;
                    }
                    else
                    {
                        nextStep = "No"; // Check Practice Location Update
                        toReturn = true;
                    }
                }
                else
                {
                    nextStep = "No"; // Check Practice Location Update
                    toReturn = true;
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
