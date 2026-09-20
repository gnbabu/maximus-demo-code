using MAXIMUS.Core.Libraries;


namespace PDMSWorkflow.Workflows
{
    public class UpdateAffiliates : Workflow.BaseWorkflow
    {
        public override int WorkflowID
        {
            get { return Constants.WorkflowType.UpdateAffiliates; }
        }
    }
}
