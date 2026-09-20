using MAXIMUS.Core.Libraries;

namespace PDMSWorkflow.Workflows
{
    public class AdminUpdate : Workflow.BaseWorkflow
    {
        public override int WorkflowID
        {
            get { return Constants.WorkflowType.AdminUpdate; }
        }
    }
}
