using System;

namespace Models.Hospice
{
   public class DiagnosisCodes
    {
        public DateTime DiagEffDate { get; set; } 
        public DateTime DiagEndDate { get; set; }
        public string PrimeTermDiag { get; set; }
        public string TermDiag2 { get; set; }
        public string TermDiag3 { get; set; }
    }
}
