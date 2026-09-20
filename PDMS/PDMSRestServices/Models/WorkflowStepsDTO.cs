namespace PDMSRestServices.Models
{
    public class WorkflowStepsDTO
    {
        public string TaskName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string StepID { get; set; }
    }
}
