using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class DestinationPayerIDs
    {
        public string MCE_ID { get; set; }
        public string PRIOR_AUTH_SUB_DESTINATION_PAYER_DESC_LRG { get; set; }
    }
    public class destinationPayerID
    {
        public destinationPayerID(string payer_code, string payer_desc)
        {
            this.Payer_code = payer_code;
            this.Payer_desc = payer_desc;
        }
        private string _payercode;
        private string _payerdesc;
        public string Payer_code
        {
            get { return _payercode; }
            set { _payercode = value; }
        }
        public string Payer_desc
        {
            get { return _payerdesc; }
            set { _payerdesc = value; }
        }
    }
}