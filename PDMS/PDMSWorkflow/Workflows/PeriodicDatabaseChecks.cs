using MAXIMUS.Core.Libraries;

namespace PDMSWorkflow.Workflows
{
    public class PeriodicDatabaseChecks : Workflow.BaseWorkflow
    {
        public override int WorkflowID
        {
            get { return Constants.WorkflowType.PeriodicDatabaseChecks; }
        }
    }
}
