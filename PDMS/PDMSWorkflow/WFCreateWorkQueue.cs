using MAXIMUS.Core.Libraries;
using System;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFCreateWorkQueue : BaseWorkflowTask, IWorkflowTask
    {
        public WFCreateWorkQueue(int processID, int stepID)
            : base(processID, stepID)
        {
            //
            // TODO: Add constructor logic here
            //
        }
        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            //DLF TODO: Add stored procedure. Emailed Teresa to ask what it does.
            return true;
        }
        override public string NextStep()
        {
            return null;
        }
    }
}

