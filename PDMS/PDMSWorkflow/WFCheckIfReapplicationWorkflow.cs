using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Workflow;
using static MAXIMUS.Core.Libraries.Constants;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFCheckIfReapplicationWorkflow : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        //Logging log = new Logging(LogThreadID, logMsg);

        public WFCheckIfReapplicationWorkflow(int processID, int stepID)
           : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
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
                DataSet dsReg;
                DataRow drReg;
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;

                if (drReg != null)
                {
                    int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                    
                    if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                    {
                        nextStep = "Update WF";
                        toReturn = true;
                    }
                    else
                    {
                        nextStep = "Not Update WF";
                        toReturn = true;
                    }
                    
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
        }
        override public string NextStep()
        {
            return nextStep;
        }
    }
}
