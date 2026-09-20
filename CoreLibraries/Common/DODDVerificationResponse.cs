using System;

namespace MAXIMUS.Core.Libraries
{
    public class DODDVerificationResponse
    {
        public int ABRID { get; set; }

        public string ArbitrationFlag { get; set; }
        public string CriminalFlag { get; set; }

        public string DateValue { get; set; }

        public string DOB { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Registrydate { get; set; }

        public string OtherDesc { get; set; }

        public string SurName { get; set; }

        public string inccatdesc { get; set; }

        public string ResponseMessage { get; set; }

        public bool Success { get; set; }
    }
}
