using MAXIMUS.Core.Libraries;


namespace PDMSWorkflow.Workflows
{
    public class RegistrationUpdateProvider : Workflow.BaseWorkflow
    {
        public override int WorkflowID
        {
            get { return Constants.WorkflowType.RegistrationUpdateProvider; }
        }
    }
}
