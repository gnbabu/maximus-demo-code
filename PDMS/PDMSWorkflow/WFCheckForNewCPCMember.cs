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
    public class WFCheckForNewCPCMember : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckForNewCPCMember(int processID, int stepID)
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
                string mmisProviderTypeID = string.Empty;
                int ProviderCategoryTypeID = 0;

                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    ApplicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                    workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                    mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
                    ProviderCategoryTypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID", drReg);
                }
                // SAM818 OHPNM - 19389 - Specialty update on CPC Ind should also go to Provider Review
                if (ApplicationTypeID == CON.ApplicationType.CPC && workflowEventTypeID == CON.WorkflowEventType.UpdateReg && mmisProviderTypeID == "99")
                {
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckifUpdateCPCMember", sqlParms, "ConvenerData");

                    nextStep = ObjectControllerHelper.HasRows(ds) && ObjectControllerHelper.GetBool("isCPCMemberAdded", ds.Tables[0].Rows[0]) ? "Yes" : "No";
                }
                else
                {
                    nextStep = "No";
                }
                toReturn = true;
            }
            catch(Exception ex)
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
