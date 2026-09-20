using System;

namespace MAXIMUS.Core.Libraries
{
    public class EmailTemplate
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateBody { get; set; }
        public string ParsedBody { get; set; }
        public string TemplateNotes { get; set; }
        public bool IsActive { get; set; }
		public string BodyTemplateName { get; set; }
        public string LastModifiedUser { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
