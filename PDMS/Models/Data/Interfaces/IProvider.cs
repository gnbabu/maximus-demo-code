using System;

namespace MAXIMUS.Models.Data.Interfaces.PDMS
{
    public interface IProvider
    {
        int PartyID { get; set; }
        int PartyTypeID { get; set; }
        string PartyType { get; set; }
        int RoleTypeID { get; set; }
        string RoleType { get; set; }
        int OrganizationTypeID { get; set; }
        string OrganizationName { get; set; }
        string DBA { get; set; }
        string OrganizationType { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string Suffix { get; set; }
        string Gender { get; set; }
        DateTime BirthDate { get; set; }
        int ProviderTypeID { get; set; }
        string ProviderType { get; set; }
        string ProviderTypeName { get; set; }
        string ProviderID { get; set; }
        string CAQHID { get; set; }
        string NPI { get; set; }
        string BaseMedicaidID { get; set; }
        string TaxID { get; set; }
        string EIN { get; set; }
        string UPIN { get; set; }
        string CredentialAddress1 { get; set; }
        string CredentialAddress2 { get; set; }
        string CredentialCity { get; set; }
        string CredentialState { get; set; }
        string CredentialZip { get; set; }
        string CredentialExtendedZip { get; set; }
        string CredentialCounty { get; set; }
        string CredentialAddressName { get; set; }
        string CredentialPhoneAreaCode { get; set; }
        string CredentialPhoneNumber { get; set; }
        string CredentialFaxAreaCode { get; set; }
        string CredentialFaxNumber { get; set; }
        string CredentialEmailAddress { get; set; }
        string PracticeState { get; set; }
        int ProgramStatusTypeID { get; set; }
        string ContactTitle { get; set; }
        DateTime LastAttestDate { get; set; }
        DateTime TermDate { get; set; }
        string TermEnrollmentStatus { get; set; }
        DateTime RequestedEffectiveDate { get; set; }
        DateTime ChangeEffectiveDate { get; set; }
        int PDMSStatusID { get; set; }
        string PDMSStatus { get; set; }
        DateTime PDMSStatusDate { get; set; }
        int CAQHStatusID { get; set; }
        string CAQHStatus { get; set; }
        DateTime CAQHStatusDate { get; set; }
        string RosterStatus { get; set; }
        DateTime LastModifiedDate { get; set; }
        Guid LastModifiedUser { get; set; }
    }
}
