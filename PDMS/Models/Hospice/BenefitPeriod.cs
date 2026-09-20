using System;

namespace Models.Hospice
{
    public class BenefitPeriod
    {
        public int BenPeriod { get; set; }
        public DateTime BenPeriodEffDate { get; set; }
        public DateTime BenPeriodEndDate { get; set; }
        public string BenUpdateReason { get; set; }
        public string Status { get; set; }
    }
}
