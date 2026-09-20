using System;

namespace Models.Hospice
{
   public class OtherPayerInfo
    {
        public DateTime PayerEffDate { get; set; } 
        public DateTime PayerEndDate { get; set; } 
        public string PayerName { get; set; } 
        public string PayerType { get; set; }
    }
}
