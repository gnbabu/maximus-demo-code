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
    public class WFCheckSendPayloadCPCMembers : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        public WFCheckSendPayloadCPCMembers(int processID, int stepID)
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
                int WorkflowID = 0;
                int workflowEventTypeID = 0;

                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    WorkflowID = ObjectControllerHelper.GetInt("WorkflowID", drReg); 
                    workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                }
                if (WorkflowID == CON.WorkflowType.CPC && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                {
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckifSendCPCMemberPayload", sqlParms, "MemberData");

                    nextStep = ObjectControllerHelper.HasRows(ds) && ObjectControllerHelper.GetBool("isSendPayloadMember", ds.Tables[0].Rows[0]) ? "Yes" : "No";

                    if (nextStep == "No" && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                        nextStep = "Update Event";

                }
                if (WorkflowID == CON.WorkflowType.CPC && (workflowEventTypeID == CON.WorkflowEventType.NewReg || workflowEventTypeID == CON.WorkflowEventType.CPCReattest))
                {
                    nextStep = "Yes";
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
