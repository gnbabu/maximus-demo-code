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
    public class WFCheckSendPayloadCPCConvener : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        public WFCheckSendPayloadCPCConvener(int processID, int stepID)
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
                int ApplicationTypeID = 0;
                int workflowEventTypeID = 0;
                string CPCPracticeTypeID = string.Empty;

                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    ApplicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                    workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                    CPCPracticeTypeID = ObjectControllerHelper.GetString("MMISCPCPracticeTypeID", drReg);
                }
                if (ApplicationTypeID == CON.ApplicationType.CPC && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                {
                    nextStep = "No";
                }
                else
                {
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    sqlParms.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                    sqlParms.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, AppSettings.Get("CPCProgramYear"), false));
                    sqlParms.Add(SqlParms.CreateParameter("CPC_PRACTICE_TYPE", DbType.String, CPCPracticeTypeID, false));
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckCPCConvenerEndDated", sqlParms,"ConvenerData");

                    nextStep = ObjectControllerHelper.HasRows(ds) && ObjectControllerHelper.GetBool("isSendConvenerPayload", ds.Tables[0].Rows[0]) ? "Yes" : "No";      
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
