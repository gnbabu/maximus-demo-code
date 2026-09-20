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
    public class WFAddNursingSpecialty : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFAddNursingSpecialty(int processID, int stepID)
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
                sqlParms.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, false));
                sqlParms.Add(SqlParms.CreateParameter("USER", DbType.Guid, CON.appAdminUserId, false));
                DataAccess.ExecuteStoredProcedure("usp_AddNursingSpecialty", sqlParms);

                // OHPNM-10266
                // see if we need to disenroll old provider before passing off to next task
                List<SqlParameter> sqlParmsForOldProvider = new List<SqlParameter>();
                sqlParmsForOldProvider.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParmsForOldProvider, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);

                if (workflowEventTypeID == CON.WorkflowEventType.ChangeProviderType)
                {
                    // they finished adding nursing services but they are changing provider types, so send them to that detour instead of task #11
                    nextStep = "Disenroll Old Provider";
                }
                else
                {
                    // go to task #11
                    nextStep = "Next";
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
