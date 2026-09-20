using System.Runtime.Serialization;

namespace Corp.Core.Libraries.ServiceAgent
{
    [DataContract(Name = "AddressVerificationRequest")]
    public class AddressVerificationRequest
    {
        /// <summary>
        /// street : 
        /// </summary>
        [DataMember]
        public string AddressLine { get; set; }

        /// <summary>
        /// street : 
        /// </summary>
        [DataMember]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// city : 
        /// </summary>
        [DataMember]
        public string City { get; set; }

        /// <summary>
        /// state : 
        /// </summary>
        [DataMember]
        public string State { get; set; }

        /// <summary>
        /// postal_code : 
        /// </summary>
        [DataMember]
        public string PostalCode { get; set; }

    }
}