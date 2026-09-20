using MAXIMUS.Core.Libraries;

namespace PDMSWorkflow.Workflows
{
    public class EPD : Workflow.BaseWorkflow
    {
        public override int WorkflowID
        {
            get { return Constants.WorkflowType.EPD; }
        }
    }
}
