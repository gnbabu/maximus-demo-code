using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public class AchFeeInformation
    {
        public int AchFeeInformationID { get; set; }
        public int? PartyID { get; set; }
        public int PartyTypeID { get; set; }
        public int? RegID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganizationName { get; set; }
        public string ProviderName { get; set; }
        public int ProviderTypeID { get; set; }
        public string BaseMedicaidID { get; set; }
        public string NPI { get; set; }
        public string TaxID { get; set; }
        public string EdisonID { get; set; }
        public string PaymentNumber { get; set; }
        public DateTime? FeePaidDate { get; set; }
        public DateTime FeeDueDate { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public Guid CreateUser { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public Guid LastModifiedUser { get; set; }

    }
}
