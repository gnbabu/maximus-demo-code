using System;

namespace Workflow
{
    [Serializable]
    public abstract class BaseWorkflow
    {

        public abstract int WorkflowID { get;}

        public string WorkflowName { get; set; }

        //Seems not to be used anymore
        //public string WorkflowShortName { get; set; }

        //ToDo in future? Make code more OO and get some of the logic out of the BaseWorkflowTask that is more workflow related than task related.                    
        //public BaseWorkflowTask CurrentTask { get; set; }
        //public BaseWorkflowTask NextTask { get; set; }
        //protected BaseWorkflowTask GetNextTask { get; set;}

        //TODO in future? The methods below is where the derived classes could come in handy 
        //public virtual void StartWorkflow ()
        //public virtual void EndWorkflow ()
        //public virtual void MoveToNextTask ()
    }
}
