using System;

namespace MAXIMUS.Core.Libraries
{
    public class EmailQueueItem
    {
        public int EmailQueueId { get; set; }
        public int BatchId { get; set; }
        public int TemplateId { get; set; }
        public string NPI { get; set; }
        public string TaxId { get; set; }
        public int RegId { get; set; }
        public string Name { get; set; }
        public string EmailAddresses { get; set; }
        public bool IsSent { get; set; }
        public string TemplateBody { get; set; }
        public string ParsedEmailBody { get; set; }
        public string EmailFields { get; set; }
        public string EmailValues { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? LastErrorDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string EmailSubject { get; set; }
    }
}
