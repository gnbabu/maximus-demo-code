using Workflow;

namespace PDMSWorkflow
{
    /// <summary>
    /// Summary description for WFLoadFromActive
    /// </summary>
    public class WFLoadFromActive : BaseWorkflowTask, IWorkflowTask
    {
        public WFLoadFromActive(int processID, int stepID)
            : base(processID, stepID)
        {
            //
            // TODO: Add constructor logic here
            //
        }
        override public bool ProcessTask()
        {
            //DLF TODO: Joe/Humayoon stored proc
            return true;
        }
        override public string NextStep()
        {
            return null;
        }
    }
}
