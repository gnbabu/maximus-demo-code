using System;

namespace MAXIMUS.Core.Libraries
{
    public class EmailBatch
    {
        public int EmailBatchId { get; set; }
        public int TotalRecords { get; set; }
        public int TemplateId { get; set; }
        public string EmailSubject { get; set; }
        public DateTime? EmailSendDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool JobComplete { get; set; }
    }
}
