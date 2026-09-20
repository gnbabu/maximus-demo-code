using System;
using Workflow;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System.Reflection;
using System.Data;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;


namespace PDMSWorkflow
{
    public class WFCheckForRapBack: BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckForRapBack(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            try
            {
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_CheckForRapBack");
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
