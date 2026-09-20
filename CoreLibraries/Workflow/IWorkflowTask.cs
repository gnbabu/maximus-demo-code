namespace Workflow
{
    /// <summary>
    /// Summary description for IWorkflowTask
    /// </summary>
    public interface IWorkflowTask
    {

        bool ProcessTask();

        string NextStep();
    }
}