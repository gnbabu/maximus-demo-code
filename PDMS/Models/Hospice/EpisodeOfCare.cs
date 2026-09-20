using System;

namespace Models.Hospice
{
   public class EpisodeOfCare
    {
        public DateTime PriceChangeDate { get; set; }
        public int Sequence { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NoBenefitDays { get; set; }
        public int NoCalendarDays { get; set; }

    }
}
