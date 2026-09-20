using MAXIMUS.Core.Libraries;

namespace PDMSWorkflow.Workflows
{
    public class RegistrationDIDDReferral : Workflow.BaseWorkflow
    {
        public override int WorkflowID
        {
            get { return Constants.WorkflowType.RegistrationDIDDReferral; }
        }
    }
}
