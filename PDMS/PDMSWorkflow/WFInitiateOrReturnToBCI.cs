using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    class WFInitiateOrReturnToBCI : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFInitiateOrReturnToBCI(int processID, int stepID)
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

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regID));
                DataSet dsBGReg = DataAccess.ExecuteStoredProcedure("usp_SelectREG_BACKGROUND_CHECK", sqlParams, "BackgroundCheckData");

                bool hasValidBackgroundCheck = dsBGReg != null &&
                     dsBGReg.Tables.Count > 0 &&
                     dsBGReg.Tables[0].Rows.Count > 0 &&
                     dsBGReg.Tables[0].Rows[0]["REG_ID"] != DBNull.Value &&
                     int.TryParse(dsBGReg.Tables[0].Rows[0]["REG_ID"].ToString(), out int returnedRegId) &&
                     returnedRegId == regID;

                nextStep = hasValidBackgroundCheck ? "Return To BCI Step" : "Initiate BCI";
                toReturn = true;
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
