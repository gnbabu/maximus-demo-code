namespace PDMSRestServices.Models
{
    public class HelpData
    {
        public string Mode { get; set; }      // popup / pdf
        public string Title { get; set; }   // HTML from RadEditor
        public string Content { get; set; }   // HTML from RadEditor
        public string PdfUrl { get; set; }    // PDF path
    }
}
