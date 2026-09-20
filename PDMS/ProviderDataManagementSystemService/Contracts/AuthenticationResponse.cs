using System;
using System.Collections.Generic;

namespace MAXIMUS.Services.PDMS.Contracts
{
    public class AuthenticateUserResponse : Response
    {
        public AuthenticateUserResponse()
        {
            MedicaidIds = new List<string>();
        }

        public bool Authenticated { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoffTime { get; set; }

        public string ProviderName { get; set; }

        public string NPI { get; set; }
        public string TaxId { get; set; }
        public List<string> MedicaidIds { get; set; }
        public string ProviderID { get; set; }
    }
}