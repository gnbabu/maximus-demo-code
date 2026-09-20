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
    public class WFCheckWavierAppReqReview : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckWavierAppReqReview(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int regID;
            int additionalApplicationID;
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
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID), out additionalApplicationID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID)));
                }
                int workflowEventTypeID = -1;
                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                }

                List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                sqlParmsTR.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, false));
                sqlParmsTR.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, false));
                sqlParmsTR.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, false));
                DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDITIONAL_APPLICATIONCustom", sqlParmsTR, "RegAddApplication");
                dsRegTR.Tables[0].TableName = "RegAddApplication";
                DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                bool isRequireReview = ObjectControllerHelper.GetBool("IS_REQUIRE_REVIEW", drRegTR);

                if (isRequireReview)
                {
                    nextStep = workflowEventTypeID == CON.WorkflowEventType.NewReg ? "Waiver New Application" : "Waiver Update Application";
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
