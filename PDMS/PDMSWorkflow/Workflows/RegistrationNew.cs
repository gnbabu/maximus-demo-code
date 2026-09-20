using MAXIMUS.Core.Libraries;

namespace PDMSWorkflow.Workflows
{
    public class RegistrationNew : Workflow.BaseWorkflow
    {
        public override int WorkflowID
        {
            get { return Constants.WorkflowType.RegistrationNew; }
        }
    }
}
