using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using Workflow;
using System.Web.UI.WebControls;

namespace PDMSWorkflow
{
    public class WFCheckIfSendNotProcessedNotice : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckIfSendNotProcessedNotice(int processID, int stepID)
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

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                bool isDoDDIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfDODDInitialApplication", parameters));

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                bool isODAIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfODAInitialApplication", parameters));

                nextStep = isDoDDIntialApp ? "DODD" : isODAIntialApp ? "ODA" : "Yes";

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
