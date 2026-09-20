using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class PaperRequestDocumentData
    {
        public PaperRequestDocumentData() { }

        public int PaperRequestQueueID { get; set; }

        public int DocumentTypeID { get; set; }

        public int DocumentID { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public Guid LastModifiedUser { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
