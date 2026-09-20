using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class PriorAuthAttachment
    {
        public int PRIOR_AUTH_SUB_DOCUMENT_TYPE_ID { get; set; }
        public string PRIOR_AUTH_SUB_TRACKING_NUMBER { get; set; }
        public string PRIOR_AUTH_SUB_DOCUMENT_ID { get; set; }
        public string PRIOR_AUTH_SUB_Note { get; set; }
        public int PRIOR_AUTH_SUB_ATTACHMENT_AUTH_TYPE { get; set; }
        public string PRIOR_AUTH_SUB_DOCUMENT_TYPE_DESC { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string PRIOR_AUTH_SUB_ATTACHMENT_STATUS { get; set; }
        public string MedicaidId { get; set; }
        public int PRIOR_AUTH_SUB_ATTACHMENT_ID { get; set; }
        public string DOCUMENT_NAME { get; set; }
        public string ORIGINAL_DOCUMENT_NAME { get; set; }
    }
}
