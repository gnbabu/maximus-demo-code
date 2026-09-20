using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS
{
    [DelimitedRecord("|")]
    [IgnoreEmptyLines]
    public class DelegateAffiliationsRecords
    {

        [FieldQuoted]
        private string updateType;

        [FieldQuoted]
        private string groupMedID;

        [FieldQuoted]
        private string affiliateMEDID;

        [FieldQuoted]
        private string affiliateNPI;

        [FieldQuoted]
        private string affiliateStartDate;

        [FieldQuoted]
        private string renderingLocationAddrLine1;

        [FieldQuoted]
        private string renderingLocationAddrLine2;

        [FieldQuoted]
        private string renderingLocationAddrCity;

        [FieldQuoted]
        private string renderingLocationAddrState;

        [FieldQuoted]
        private string renderingLocationAddrZip;

        [FieldQuoted]
        private string response;

        public string UpdateType { get => updateType; set => updateType = value; }
        public string GroupMedID { get => groupMedID; set => groupMedID = value; }
        public string AffiliateMEDID { get => affiliateMEDID; set => affiliateMEDID = value; }
        public string AffiliateNPI { get => affiliateNPI; set => affiliateNPI = value; }
        public string AffiliateStartDate { get => affiliateStartDate; set => affiliateStartDate = value; }
        public string RenderingLocationAddrLine1 { get => renderingLocationAddrLine1; set => renderingLocationAddrLine1 = value; }
        public string RenderingLocationAddrLine2 { get => renderingLocationAddrLine2; set => renderingLocationAddrLine2 = value; }
        public string RenderingLocationAddrCity { get => renderingLocationAddrCity; set => renderingLocationAddrCity = value; }
        public string RenderingLocationAddrState { get => renderingLocationAddrState; set => renderingLocationAddrState = value; }
        public string RenderingLocationAddrZip { get => renderingLocationAddrZip; set => renderingLocationAddrZip = value; }
        public string Response { get => response; set => response = value; }

    }
}
