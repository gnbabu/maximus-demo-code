using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class NPPESAPIRequest
    {
        public string NPI { get; set; }
        public string ProviderType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string SSN { get; set; }
        public string UserID { get; set; }
    }

    public class NPPESAPIResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }
        public List<TaxonomyTypes> TaxnomyTypes { get; set; }

        public List<SpecialtyTypes> SpecialtyTypes { get; set; }

    }    
}
