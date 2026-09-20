using MAXIMUS.Core.Libraries;

namespace PDMSWorkflow.Workflows
{
    public class ADHP : Workflow.BaseWorkflow
    {
        public override int WorkflowID
        {
            get { return Constants.WorkflowType.ADHP; }
        }
    }
}
