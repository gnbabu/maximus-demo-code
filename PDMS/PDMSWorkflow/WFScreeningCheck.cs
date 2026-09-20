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
    public class WFScreeningCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFScreeningCheck(int processID, int stepID)
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
                int providerRiskLevel = ObjectControllerHelper.GetInt("PROVIDER_RISK_LEVEL_ID", drReg);
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int applicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                bool requiresScreening = true;
                if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                {

                        if (!RegistrationController.HasNewOwnersOrChangeInOwnerInformation(regID) && !RegistrationController.HasPrimaryPracticeLocationChange(regID))
                            requiresScreening = false;
                }
                // TX Demo case to show we screen providers if they change address or add owners
                if(workflowEventTypeID == CON.WorkflowEventType.UpdateReg && (providerRiskLevel == 2 || providerRiskLevel ==3))
                    requiresScreening = true;

                string termStatus = ObjectControllerHelper.GetString("EnrollmentStatusCode", drReg);

                if (requiresScreening)
                {
                    nextStep = "Needs Screening";
                    toReturn = true;
                }
                else
                {

                    if (termStatus == CON.EnrollmentStatusCode.Active)
                        nextStep = "Skip for Active Updates";
                    else
                        nextStep = "Screening Not Needed";

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
