using System;

namespace MAXIMUS.Services.PDMS.Contracts
{
    public class Response
    {
        public int ResponseCode { get; set; }
        
        public DateTime TimeStamp { get; set; }

        public int ErrorCode { get; set; }
        
        public string ErrorDescription { get; set; }

        public string TransactionId { get; set; }
    }
}