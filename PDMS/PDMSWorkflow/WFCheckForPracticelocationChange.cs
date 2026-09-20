using MAXIMUS.Controllers.PDMS;
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
    public class WFCheckForPracticelocationChange : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckForPracticelocationChange(int processID, int stepID)
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
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int applicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                
                if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                {
                    if (!RegistrationController.HasPrimaryPracticeLocationChange(regID))
                    {
                        nextStep = "No Change";
                        toReturn = true;
                    }
                    else
                    {
                        // OHPNM-2614 - primary practice location has changed; check risk level to see if they need to go through screening/review
                        List<SqlParameter> riskSqlParms = new List<SqlParameter>();
                        int riskLevel = 0; // stored procedure doesn't return anything if risk level is 1 or 0
                        riskSqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                        DataSet riskDataSet = DataAccess.ExecuteStoredProcedure("usp_CheckRiskLevelByRegID", riskSqlParms, "RegDataHR");
                        riskDataSet.Tables[0].TableName = "RegDataHR";
                        if (riskDataSet.Tables[0].Rows.Count > 0)
                        {
                            DataRow drRiskLevel = riskDataSet.Tables[0].Rows[0];
                            if (!drRiskLevel.IsNull("PROVIDER_RISK_LEVEL_ID"))
                            {
                                riskLevel = ObjectControllerHelper.GetInt("PROVIDER_RISK_LEVEL_ID", drRiskLevel);
                            }
                        }
                        // else -- stored procedure doesn't return anything if risk level is 1 or 0, so risk level of 1 will actually be a zero at this point

                        if (riskLevel > 1)
                        {
                            // High Or Moderate Risk Level. send them to screening/review
                            nextStep = "Check For Site Visit";
                        }
                        else
                        {
                            // they don't need screening/review since they are low risk; send them on their way
                            nextStep = "No Change";
                        }
                        
                        toReturn = true;
                    }
                }
                else
                {
                    nextStep = "Check For Site Visit";
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
