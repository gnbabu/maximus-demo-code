namespace PDMSRestServices.Models
{
    public class PdfRequest
    {
        public string Name { get; set; }
        public string Std { get; set; }
        public string date { get; set; }
        public FeesStructure fees { get; set; }
    }
    public class PdfRequestJSON
    {
        public string MEun { get; set; }
    }
    public class PdfResponseJSON
    {
        public string? response { get; set; }
        public byte[]? fileBytes { get; set; }
    }

    public class FeesStructure
    {
        public int id { get; set; }
        public string FeesDescription { get; set; }
        public string Amount { get; set; }
    }
}
