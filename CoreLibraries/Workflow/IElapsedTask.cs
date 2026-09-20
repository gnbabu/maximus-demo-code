namespace Workflow
{
    /// <summary>
    /// Summary description for IWorkflowTask
    /// </summary>
    public interface IElapsedTask
    {

        bool ProcessElapsedTask();

        string NextStep();
    }
}