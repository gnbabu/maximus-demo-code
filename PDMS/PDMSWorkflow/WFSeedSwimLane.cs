using MAXIMUS.Core.Libraries;
using System;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    /// <summary>
    /// Inserts a seed step in the "Provider" swim lane, so future visits to tasks in that swim lane will be auto-assigned to the correct owner id.
    /// </summary>
    public class WFSeedSwimLane : BaseWorkflowTask, IWorkflowTask
    {
        public WFSeedSwimLane(int processID, int stepID)
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

            try
            {
                // Get the ownerID for this provider record.
                string ownerID = string.Empty;

                // Get the name of he task that will be seeded with the ownerID.
                string taskName = GetProcessParameter(Constants.TaskParameter.SeedTaskName);

                // Call the workflow method to insert the placeholder step in this process.
                WorkflowController.InsertSeedStep(ProcessID, taskName, ownerID);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return true;
        }
        override public string NextStep()
        {
            return null;
        }
    }
}
