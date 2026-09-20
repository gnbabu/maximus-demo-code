using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class Provider : IProvider
    {

        public Provider()
		{
		}

        public int PartyID {get; set;}

        public int PartyTypeID { get; set; }

        public string PartyType { get; set; }

        public int RoleTypeID { get; set; }

        public string RoleType { get; set; }

        public int OrganizationTypeID { get; set; }

        public string OrganizationName { get; set; }

        public string DBA { get; set; }

        public string OrganizationType { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public int ProviderTypeID { get; set; }

        public string ProviderType { get; set; }

        public string ProviderTypeName { get; set; }

        public int ProviderCategoryTypeID { get; set; }

        public string ProviderCategoryTypeName { get; set; }

        public string ProviderID { get; set; }

        public string CAQHID { get; set; }

        public string NPI { get; set; }

        public string BaseMedicaidID { get; set; }

        public string TaxID { get; set; }

        public string EIN { get; set; }

        public string UPIN { get; set; }

        public string CredentialAddress1 { get; set; }

        public string CredentialAddress2 { get; set; }

        public string CredentialCity { get; set; }

        public string CredentialState { get; set; }

        public string CredentialZip { get; set; }

        public string CredentialExtendedZip { get; set; }

        public string CredentialCounty { get; set; }

        public string CredentialAddressName { get; set; }

        public string CredentialPhoneAreaCode { get; set; }

        public string CredentialPhoneNumber { get; set; }

        public string CredentialFaxAreaCode { get; set; }

        public string CredentialFaxNumber { get; set; }

        public string CredentialEmailAddress { get; set; }

        public string PracticeState{ get; set; }

        public int ProgramStatusTypeID { get; set; }

        public string ContactTitle { get; set; }

        public DateTime LastAttestDate { get; set; }

        public DateTime TermDate { get; set; }

        public string TermEnrollmentStatus { get; set; }

        public DateTime RequestedEffectiveDate { get; set; }

        public DateTime ChangeEffectiveDate { get; set; }

        public int PDMSStatusID { get; set; }

        public string PDMSStatus { get; set; }

        public DateTime PDMSStatusDate { get; set; }

        public int CAQHStatusID { get; set; }

        public string CAQHStatus { get; set; }

        public DateTime CAQHStatusDate { get; set; }

        public string RosterStatus { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public Guid LastModifiedUser { get; set; }

        public string LastModifiedUserName { get; set; }

        public string ProviderName
        {
            get
            {
                return string.IsNullOrWhiteSpace(OrganizationName) ? string.Concat(FirstName, " ", LastName) : OrganizationName;
            }
        }

        public string ProviderTypeNameWithSuffix
        {
            get
            {
                return string.Concat(ProviderTypeName, string.IsNullOrWhiteSpace(ProviderType) ? string.Empty : string.Concat("(", ProviderType, ")"));
            }
        }

        #region Public Methods
        public void LoadProviderObjectFromDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];
            PartyID = Methods.GetIntValue(dr["PartyID"]);
            PartyTypeID = Methods.GetIntValue(dr["PartyTypeID"]);
            PartyType = Methods.GetStringValue(dr["PartyType"]);
            RoleTypeID = Methods.GetIntValue(dr["RoleTypeID"]);
            RoleType = Methods.GetStringValue(dr["RoleType"]);
            OrganizationTypeID = Methods.GetIntValue(dr["OrganizationTypeID"]);
            OrganizationName = Methods.GetStringValue(dr["OrganizationName"]);
            DBA = Methods.GetStringValue(dr["DBA"]);
            OrganizationType = Methods.GetStringValue(dr["OrganizationType"]);
            FirstName = Methods.GetStringValue(dr["FirstName"]);
            MiddleName = Methods.GetStringValue(dr["MiddleName"]);
            LastName = Methods.GetStringValue(dr["LastName"]);
            Suffix = Methods.GetStringValue(dr["Suffix"]);
            Gender = Methods.GetStringValue(dr["Gender"]);
            BirthDate = Methods.GetDateValue(dr["BirthDate"]);
            ProviderTypeID = Methods.GetIntValue(dr["ProviderTypeID"]);
            ProviderTypeName = Methods.GetStringValue(dr["ProviderTypeName"]);
            ProviderType = Methods.GetStringValue(dr["ProviderType"]);
            ProviderCategoryTypeID = Methods.GetIntValue(dr["ProviderCategoryTypeID"]);
            ProviderCategoryTypeName = Methods.GetStringValue(dr["ProviderCategoryTypeName"]);
            ProviderID = Methods.GetStringValue(dr["ProviderID"]);
            CAQHID = Methods.GetStringValue(dr["CAQHID"]);
            NPI = Methods.GetStringValue(dr["NPI"]);
            BaseMedicaidID = Methods.GetStringValue(dr["BaseMedicaidID"]);
            TaxID = Methods.GetStringValue(dr["TaxID"]);
            EIN = Methods.GetStringValue(dr["EIN"]);
            UPIN = Methods.GetStringValue(dr["UPIN"]);
            CredentialAddress1 = Methods.GetStringValue(dr["CredentialAddress1"]);
            CredentialAddress2 = Methods.GetStringValue(dr["CredentialAddress2"]);
            CredentialCity = Methods.GetStringValue(dr["CredentialCity"]);
            CredentialCounty = Methods.GetStringValue(dr["CredentialCounty"]);
            CredentialState = Methods.GetStringValue(dr["CredentialState"]);
            CredentialZip = Methods.GetStringValue(dr["CredentialZip"]);
            CredentialExtendedZip = Methods.GetStringValue(dr["CredentialExtendedZip"]);
            CredentialAddressName = Methods.GetStringValue(dr["CredentialAddressName"]);
            CredentialPhoneAreaCode = Methods.GetStringValue(dr["CredentialPhoneAreaCode"]);
            CredentialPhoneNumber = Methods.GetStringValue(dr["CredentialPhoneNumber"]);
            CredentialFaxAreaCode = Methods.GetStringValue(dr["CredentialFaxAreaCode"]);
            CredentialFaxNumber = Methods.GetStringValue(dr["CredentialFaxNumber"]);
            CredentialEmailAddress = Methods.GetStringValue(dr["CredentialEmailAddress"]);
            PracticeState = Methods.GetStringValue(dr["PracticeState"]);
            ProgramStatusTypeID = Methods.GetIntValue(dr["ProgramStatusTypeID"]);
            ContactTitle = Methods.GetStringValue(dr["ContactTitle"]);
            LastAttestDate = Methods.GetDateValue(dr["LastAttestDate"]);
            TermDate = Methods.GetDateValue(dr["TermDate"]);
            TermEnrollmentStatus = Methods.GetStringValue(dr["TermEnrollmentStatus"]);
            RequestedEffectiveDate = Methods.GetDateValue(dr["RequestedEffectiveDate"]);
            ChangeEffectiveDate = Methods.GetDateValue(dr["ChangeEffectiveDate"]);
            PDMSStatusID = Methods.GetIntValue(dr["PDMSStatusID"]);
            PDMSStatus = Methods.GetStringValue(dr["PDMSStatus"]);
            PDMSStatusDate = Methods.GetDateValue(dr["PDMSStatusDate"]);
            CAQHStatusID = Methods.GetIntValue(dr["CAQHStatusID"]);
            CAQHStatus = Methods.GetStringValue(dr["CAQHStatus"]);
            CAQHStatusDate = Methods.GetDateValue(dr["CAQHStatusDate"]);
            RosterStatus = Methods.GetStringValue(dr["RosterStatus"]);
            LastModifiedDate = Methods.GetDateValue(dr["LastModifiedDateTime"]);
        }
        #endregion

    }
}
