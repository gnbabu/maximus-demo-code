namespace PDMSRestServices.Models
{
    public class UIControlVisibility
    {
        public int UIControlId { get; set; }
        public string PageName { get; set; }
        public string ControlID { get; set; }
        public int Reg_Id { get; set; }
        public bool? IsVisible { get; set; }
        public string IsActive { get; set; }

        public string Record_Status { get; set; }
        public DateTime? Approval_Date { get; set; }
        public DateTime? Review_Date { get; set; }
    }

    public class UpdateUIControls
    {
        public List<int> Ids { get; set; }
        public string Action { get; set; }
  
    }

    public class DeleteUI
    {
        public List<int> Ids { get; set; }
    }
}
