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
    public class WFSiteVisitCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFSiteVisitCheck(int processID, int stepID)
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
                int workflowID = ObjectControllerHelper.GetInt("WorkflowID", drReg);

                //If needs a site visit, insert site visit record.
                int requiresSiteVisit = 0;

                if (workflowID == CON.WorkflowType.BumpUp)
                    requiresSiteVisit = 1;
                else   
                    requiresSiteVisit = ScreeningController.RegistrationRequiresSiteVisit(regID);

                if (requiresSiteVisit == 1)
                {
                    ScreeningController.InsertProviderSiteVisitActivity(regID, DateTime.Now, Constants.appAdminUserId);
                    RegistrationController.UpdateRegistrationStatusType(regID, Constants.RegistrationStatusTypeId.SiteVisit, DateTime.Now, Constants.appAdminUserId);
                    nextStep = "Needs Site Visit";
                    toReturn = true;
                }               
                else
                {                    
                    nextStep = "Site Visit Not Needed";
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
