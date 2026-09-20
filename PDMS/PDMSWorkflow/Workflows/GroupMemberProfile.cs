using MAXIMUS.Core.Libraries;

namespace PDMSWorkflow.Workflows
{
    public class GroupMemberProfile : Workflow.BaseWorkflow
    {
        public override int WorkflowID
        {
            get { return Constants.WorkflowType.GroupMemberProfile; }
        }
    }
}
