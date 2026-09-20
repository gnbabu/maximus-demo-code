using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFCredentialCheckRiskLevel : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        public WFCredentialCheckRiskLevel(int processID, int stepID)
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
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                DataSet dsRegRisk = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_Credentialing", sqlParms, "RegDataRisk");

                dsRegRisk.Tables[0].TableName = "RegDataRisk";
                DataRow drRegRisk = ObjectControllerHelper.HasRows(dsRegRisk) ? dsRegRisk.Tables[0].Rows[0] : null;
                int credentialRiskLevel = ObjectControllerHelper.GetInt("RISK_LEVEL_ID", drRegRisk);


                sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                string mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);

                if (credentialRiskLevel == 1 && (!string.IsNullOrWhiteSpace(mmisProviderTypeID) && (mmisProviderTypeID.Equals("86") || mmisProviderTypeID.Equals("88") || mmisProviderTypeID.Equals("89"))))
                {
                    nextStep = "Send to Credential Committee";
                }
                else
                {
                    nextStep = "Send to Credential Quality";

                }

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
