namespace PDMSRestServices.Models
{
    public class DynamicControls
    {
        public int ID { get; set; }
        public string RegID { get; set; }
        public string ProviderType { get; set; }
        public string SectionType { get; set; }
        public string FieldName { get; set; }
        public string DataType { get; set; }
        public string ControlType { get; set; }
        public string ControlLevel { get; set; }
        public string IsActive { get; set; }
        public string SelectedValues { get; set; }
        public string ControlId { get; set; }
        public string ModifiedUser { get; set; }
        public string ModifiedDate { get; set; }
        public string Record_Status { get; set; }
        public DateTime? Approval_Date { get; set; }
        public DateTime? Review_Date { get; set; }
    }

    public class UpdateDynamicControls
    {
        public List<int> Ids { get; set; }
        public string Status { get; set; }
    }
    public class DeleteDynamicFields
    {
        public List<int> Ids { get; set; }
    }
}
