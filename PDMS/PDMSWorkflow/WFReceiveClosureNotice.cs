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
   public class WFReceiveClosureNotice : BaseWorkflowTask, IWorkflowTask
    {

        string nextStep = null;

        public WFReceiveClosureNotice(int processID, int stepID)
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
                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                    sqlParmsTR.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_APPLICATIONCustom", sqlParmsTR, "RApplication");
                    dsRegTR.Tables[0].TableName = "RApplication";
                    DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                    string AppStatus = ObjectControllerHelper.GetString("Application_Status", drRegTR);
                    if (AppStatus.Equals(CON.ApplicationStatus.Pending_Review_ID) || AppStatus.Equals(CON.ApplicationStatus.Capital_Funds_Review)
                        || AppStatus.Equals(CON.ApplicationStatus.In_Review)
                        || AppStatus.Equals(CON.ApplicationStatus.Sanction_Review)
                        || AppStatus.Equals(CON.ApplicationStatus.Waiting_on_Agreement)
                        || AppStatus.Equals(CON.ApplicationStatus.Pending_Effective_Date))
                    {
                        nextStep = "Next";
                        toReturn = true;
                    }
                    else
                    {
                        nextStep = "";
                        toReturn = false;
                    }
                }
                else
                {
                    nextStep = "Next";
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
