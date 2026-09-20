using System;

namespace Models.Hospice
{
   public class ServiceCountyState
    {
        public string County { get; set; }
        public DateTime CountyEffDate { get; set; }
        public DateTime CountyEndDate { get; set; }
        public string State { get; set; }
    }
}
