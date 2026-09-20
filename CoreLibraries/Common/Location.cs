using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Core.Libraries
{
    [DataContract(Name = "Location")]
    public class Location
    {
        [DataMember(Name = "VerificationConfidence")]
        public int VerificationConfidence { get; set; }

        [DataMember(Name = "AddressLine")]
        public string AddressLine { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "State")]
        public string State { get; set; }

        [DataMember(Name = "PostalCode")]
        public string PostalCode { get; set; }

        [DataMember(Name = "ZipAddon")]
        public string ZipAddon { get; set; }

        [DataMember(Name = "County")]
        public string County { get; set; }

        [DataMember(Name = "CountyName")]
        public string CountyName { get; set; }

        [DataMember(Name = "CountyNumber")]
        public string CountyNumber { get; set; }

        [DataMember(Name = "Latitude")]
        public string Latitude { get; set; }

        [DataMember(Name = "Longitude")]
        public string Longitude { get; set; }

        [DataMember(Name = "ReturnCodes")]
        public string ReturnCodes { get; set; }

        [DataMember(Name = "ErrorCodes")]
        public string ErrorCodes { get; set; }

        [DataMember(Name = "ErrorDesc")]
        public string ErrorDesc { get; set; }

        [DataMember(Name = "SearchesLeft")]
        public int SearchesLeft { get; set; }
    }
}
