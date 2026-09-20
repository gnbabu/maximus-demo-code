using Corp.Core.Libraries.ServiceAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PDMSRestServices.Models
{
    public class StreamlinedAppDropDowns
    {
        public List<ProviderType> dtProviderTypes { get; set; }
        public List<Gender> dtGender { get; set; }
        public List<States> dtState { get; set; }

    }

    public class ProviderType
    {
        public string PROVIDER_TYPE_NAME { get; set; }
        public string MMIS_PROVIDER_TYPE_ID { get; set; }
    }

    public class Gender
    {
        public string PROVIDER_GENDER_NAME { get; set; }
        public string PROVIDER_GENDER_INITIAL { get; set; }
    }

    public class States
    {
        public string STATE_ABBREV { get; set; }
        public string STATE_ID { get; set; }
    }

    public class TaxonomyTypes
    {
        public string TAXONOMY_CODE { get; set; }
        public string TAXONOMY_DESCRIPTION { get; set; }
    }

    public class SpecialtyTypes
    {
        public string SPECIALTY_TYPE_NAME { get; set; }
        public int SPECIALTY_TYPE_ID { get; set; }
    }

    public class County
    {
        public string County_Value { get; set; }
        public string County_Text { get; set; }
    }

    public class ProviderRegistrationData
    {
        public Guid UserID { get; set; }
        public Guid SelectedProvAdminUserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string NPI { get; set; }
        public string TaxID { get; set; }
        public int TaxIDTypeID { get; set; }
        public string MMISProviderTypeID { get; set; }
        public DateTime? RequestedEffectiveDate { get; set; }
        public string TaxonomyCode { get; set; }
        public string TaxonomyName { get; set; }
        public string SpecialtyTypeID { get; set; }
        public string AddressContactName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }   
        public string ZipCode { get; set; }
        public string CountyName { get; set; }
        public string CountyCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ZipExt { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int OverrideAddress { get; set; }

    }
    //public class AddressVerificationReq
    //{
    //    public AddressVerificationRequest Address { get; set; }
    //    public int AddressTypeId { get; set; }

    //}
}
