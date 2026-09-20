namespace PDMSRestServices.Models
{
    public class ProviderFeedVM
    {
        public int RegID { get; set; }
        public int ProviderFeedID { get; set; }
        public string InitiatedBy { get; set; }
        public string PersonReviewedBy { get; set; }
        public string EnrollmentType { get; set; }
        public string FinalDispossion { get; set; }
        public string NotesDate { get; set; }
        public string CreatedBy { get; set; }
        public List<string> Notes { get; set; }
        public int ProcessID { get; set; }
    }
    public class ProviderFeedHistVM
    {
        public int ProviderFeedID { get; set; }
        public string NOTE_DATE_TIME { get; set; }
        public string Type { get; set; }
        public string UserName { get; set; }
        public string TASK_NAME { get; set; }
        public string REG_PAGE_NAME { get; set; }
        public List<string> NOTE_TEXT { get; set; }
    }

    public class ProviderFeedNotes
    {
        public int ProviderFeedID { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
    }
    public class HttpResponseMessage
    {
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }

    }

    public class ProviderFeedAddNotes
    {
        public string Note_Date { get; set; }
        public string Initiator_OHID { get; set; }
        public string Lasted_Reviewed_By { get; set; }
        public string Enrollment_Type { get; set; }
        public string Final_Disposition { get; set; }
    }
}
