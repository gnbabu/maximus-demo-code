using MAXIMUS.Core.Libraries;

namespace PDMSWorkflow.Workflows
{
    public class RegistrationRevalidation : Workflow.BaseWorkflow
    {
        public override int WorkflowID
        {
            get { return Constants.WorkflowType.RegistrationRevalidation; }
        }
    }
}
