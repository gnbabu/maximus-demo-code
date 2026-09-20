using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;


namespace PDMSWorkflow
{
    public class WFReturnToInitializingWF : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFReturnToInitializingWF(int processID, int stepID)
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
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectWorkflowStepsRTIByRegID", sqlParms, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int taskID = ObjectControllerHelper.GetInt("TASK_ID", drReg);
                switch (taskID)
                {
                    case 4:
                        nextStep = "Return To Manual Screening";
                        toReturn = true;
                        break;
                    case 5:
                        nextStep = "Return To Provider Review";
                        toReturn = true;
                        break;
                    case 14:
                        nextStep = "Return To Background Check Entry";
                        toReturn = true;
                        break;
                    case 15:
                        nextStep = "Return To Background Check Complete Entry";
                        toReturn = true;
                        break;
                    case 39:
                        nextStep = "Return To Background Check ReSubmit Entry";
                        toReturn = true;
                        break;
                    case 211:
                        nextStep = "Return To Site Visit Review";
                        toReturn = true;
                        break;
                    default:
                        break;
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
