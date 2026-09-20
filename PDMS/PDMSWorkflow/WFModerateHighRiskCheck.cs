using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    class WFModerateHighRiskCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFModerateHighRiskCheck(int processID, int stepID)
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
                //SAM505  
                string showRTPBCICheckBox = p.GetProcessParameter(Constants.ProcessParameter.ShowRTPBCICheckBox);
                bool isWaiverProvAddODMSpec = string.IsNullOrEmpty(showRTPBCICheckBox) ? false : Convert.ToBoolean(showRTPBCICheckBox);

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                int riskLevel = 0;
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_CheckRiskLevelByRegID", sqlParms, "RegDataHR");
                dsReg.Tables[0].TableName = "RegDataHR";
                if (dsReg.Tables[0].Rows.Count > 0)
                {
                    DataRow drRiskLevel = dsReg.Tables[0].Rows[0];
                    if (!drRiskLevel.IsNull("PROVIDER_RISK_LEVEL_ID"))
                    {
                        riskLevel = ObjectControllerHelper.GetInt("PROVIDER_RISK_LEVEL_ID", drRiskLevel);
                    }
                }
                if (riskLevel > 1 || isWaiverProvAddODMSpec)
                {
                    //Need to be High Or Moderate Risk Level
                    nextStep = "Yes";
                    toReturn = true;
                }
                else
                {
                    nextStep = "No";
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
