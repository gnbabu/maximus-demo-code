using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibcoDocImportNetCore
{

    public class Enumerations
    {
        public enum LogAccessType
        {
            PageLoad = 1,
            SearchClick = 2,
            Download = 3
        }

        public enum RecordStatusEnum
        {
            Read = 1,
            Create = 2,
            Update = 3,
            Delete = 4,
            Inactive = 5
        }

        public enum CAQHChangeType
        {
            Added,
            Changed,
            Removed,
            Unchanged
        }

        public enum ContactMechanismRoleTypeId
        {
            MailTo = 1,
            PayTo = 2,
            Servicing = 3,
            Credentialing = 4,
            Satellite = 5,
            OfficeMgr = 8,
            Remittance = 9,
            Other = 10,
            W9 = 11
        }

        public enum ContactMechanismTypeId
        {
            PostalAddress = 22,
            TelecomNumber = 23,
            EmailAddress = 24
        }

        public enum CommunicationEventTypeId
        {
            AttestDate = 36,
            RecredentialDate = 37,
            PracticeEndDate = 38,
            TermDate = 39,
            DisclosureAnswerFlag = 40
        }

        public enum ElectronicAddressTypeId
        {
            EmailAddress = 15,
            URL = 16
        }

        public enum OrganizationTypeId
        {
            Hospital = 36,
            ProviderPracticeGroup = 37,
            EducationalInstitution = 38,
            SpecialtyBoard = 39,
            Other = 40
        }

        public enum PartyRoleTypeId
        {
            Provider = 43,
            Hospital = 44,
            SpecialtyBoard = 45,
            EducationalInstitution = 46,
            ProviderGroupPractice = 47,
            Other = 48,
            OfficeMgr = 49,
            BillingContact = 50,
            CredentialingContact = 51,
            CheckPayableTo = 52
        }

        public enum PartyIdentificationNumberTypeId
        {
            AttestId = 38,
            CAQHId = 37,
            MMISProviderId = 40,
            NPI = 36,
            UPIN = 39
        }

        public enum PartyTypeId
        {
            Person = 15,
            Organization = 16,
            Associate = 17
        }

        #region "Errors"

        /// <summary>
        /// This enumeration is associated with the following PDMS table:
        ///     PDMS_ERROR_CATEGORY_TYPE
        /// </summary>
        public enum ErrorCategoryType
        {
            PDMS = 1,
            CAQH = 2,
            MMIS = 3
        }

        /// <summary>
        /// This enumeration is associated with the following PDMS table:
        ///     ERROR_TYPE
        /// </summary>
        /// <remarks>
        ///     To regeneration list of the database run the following query
        ///     SELECT ERROR_CODE + ' = ' + CONVERT(varchar(4),ERROR_TYPE_ID) + ',' + 
        ///       REPLICATE(' ', 15-(LEN(ERROR_CODE)+LEN(CONVERT(varchar(4),ERROR_TYPE_ID)))) + '// ' + 
        ///       REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(ERROR_NAME, '= N' , ''),'/',''),' ',''),'–',''),'=Y',''),'-','')
        ///     FROM ERROR_TYPE
        ///     WHERE ERROR_CODE LIKE 'P%'
        ///     ORDER BY ERROR_CODE
        /// </remarks>
        public enum ErrorType
        {
            P001 = 1,          // DisclosureFlag
            P002 = 2,          // CountryNotUnitedStates
            P003 = 3,          // ExpiredAttestation
            P004 = 4,          // Undeliverable
            P005 = 5,          // ReturnedMail
            P006 = 6,          // DeceasedRetired
            P007 = 7,          // OptOut
            P008 = 8,          // NonResponderFlag
            P009 = 9,          // DEANumberReviewRequired
            P010 = 10,         // ProviderSpecialtyReviewRequired
            P011 = 11,         // MMISServiceLocationReviewRequired
            P012 = 12,         // MMISProviderReviewRequired
            P013 = 13,         // MMISBaseMedicaidIDReviewRequired
            P014 = 14,         // MissingCredentialingContactAddressDefaultedtoPracticeLocationAddress
            P015 = 15,         // MissingDEANumber
            P016 = 16,         // MissingHospitaldata
            P017 = 17,         // MissingLicenseNumber(s)
            P018 = 18,         // InvalidLicenseType
            P019 = 19,         // InvalidLicenseTypeDefaultedtoProviderType
            P020 = 20,         // MissingLicenseIssueDate
            P021 = 21,         // MissingPracticeLocation(s)
            P022 = 22,         // TaxonomyNotFound
            P023 = 23,         // InvalidProviderType
            P024 = 24,         // MissingProviderType
            P025 = 25,         // MissingProviderTypeDefaultedtoLicenseType
            P026 = 26,         // InvalidSpecialtyType
            P027 = 27,         // MissingSpecialties
            P029 = 29,         // MissingTaxIDs
            P030 = 30,         // MissingCredentialingContactEmailAddress
            P031 = 31,         // MissingCredentialingContactEmailAddressDefaultedtoPracticeLocationEmail
            P032 = 32,         // MissingPracticeLocationEmailAddress
            P033 = 33,         // MissingBillingContactEmailAddress
            P034 = 34,         // MissingBillingContactEmailAddressDefaultedtoPracticeLocationEmail
            P035 = 35,         // MissingPaymentContactEmailAddress
            P036 = 36,         // MissingPaymentContactEmailAddressDefaultedtoPracticeLocationEmail
            P037 = 37,         // MissingDEAIssueDateDefaultedtoImportDate
            P038 = 141,        // DuplicateProvider
            P039 = 142,        // InvalidFaxNumber
            P040 = 143,        // InvalidPhoneNumber
            P041 = 145,        // ServiceLocationRemovedfromCAQH
            P042 = 146,        // LicenseReviewRequired
            P043 = 147,        // MissingPracticeLocationEmailAddressDefaultedtoCredentialingContactEmail
            P044 = 148,        // NPIMismatch
            P045 = 149,        // MissingLicenseTypeDefaultedtoProviderType
            P047 = 151,        // MissingCredentialingContactAddress
            P048 = 152,        // MissingLicenseExpirationDateDefaultedto12312299
            P049 = 153,        // MissingDEAExpirationDateDefaultedto12312299
            P050 = 154,        // MissingLicenseType
            P051 = 155,        // InvalidEmailAddress
            P052 = 158,        // MissingCredentialingContactEmailAddressDefaultedtoRegistrationEmail
            P053 = 161,        // CAQHNPIMismatch
            P054 = 156,        // PrimaryPracticeStateisnotTN
            P056 = 157,        // CurrentlyPracticingFlagchangedtoNo
            P057 = 159,        // BillingContactAddressMissingDefaultedtoPracticeAddress
            P058 = 160,        // PaymentContactAddressMissingDefaultedtoPracticeAddress
            P059 = 162,        // CAQHUpdateIdMismatch
            P060 = 163,        // DirectoryMaintenancemissingStandardExtract
            P061 = 212,        // CAQHRefreshReviewRequired
            P063 = 267,        // MissingTaxTypeDescriptiondefaultedtoIndividual
            P070 = 281         // Failed NPI Check Digit
        }

        #endregion

        public enum ProviderPropertyTypeId
        {
            MedicareProviderFlag = 36,
            HospitalPrivilegeFlag = 37,
            HospitalBasedFlag = 38,
            PrimaryPracticeState = 39,
            Other = 40
        }

        public enum ProviderStatusChangeTypeId
        {
            CAQH = 1,
            MMIS = 2,
            PDMS = 3
        }

        public enum TaxIdTypeId
        {
            SSN = 15,
            Group = 16,
            Individual = 21
        }

        public enum TelecommNumberTypeId
        {
            Phone = 19,
            Fax = 20
        }

        public enum FieldTypeEnum { TextBox, Calendar, Label, StackedLabel, LinkButton, ComboBox, YesNo, RadioButtonList, Checkbox, NumericBox };

        public enum SupplementalRebateStatusType
        {
            Pending = 1,
            ManufacturerReview = 2,
            StateReview = 3,
            Approved = 4
        };

        public enum ScreeningEntityType
        {
            None = 0,
            Provider = 1,
            Affiliation = 2,
            Owner = 3,
            HouseholdMember = 4
        }


        public enum ScreeningActivityType
        {
            ApplicationReceipt = 1,
            FeeCollection = 2,
            OIGLEIEVerification = 3,
            SAMVerification = 4,
            SSDMFVerification = 5,
            NPIVerification = 6,
            MCSISVerification = 7,
            PECOSVerfication = 8,
            SAVEVerification = 9,
            NDENVerification = 10,
            LicenseVerification = 11,
            APSCPSVerification = 12,
            SexOffenderVerification = 13,
            NEMEPLVerification = 14,
            SiteVisit = 15,
            MedicaidIDAssigned = 16,
            WelcomeLetter = 17,
            NPDBVerification = 18,
            DEAVerification = 19,
            ControlledSubstanceVerification = 20,
            CriminalBackgroundCheck = 21,
            SiteVisitVerification = 22,
            MEDVerification = 23,
            //5yearworkhistory = 26,            
            UnDefined = 99,
            DODDAbuserRegistry = 31
        }
        public enum CredentialActivityType
        {
            OIGVerification = 3,
            SAMVerification = 4,
            NPPESVerification = 6,
            MedicaidExclusionVerification = 7,
            LicenseVerification = 11,
            NPDBVerification = 18,
            DEAVerification = 19,
            ControlledSubstanceVerification = 20,
            MedicareExclusionVerification = 23,
            ProviderAttestation = 24,
            MalPracticeInsurance = 25,
            FiveYearWorkHistory = 26,
            Education = 29,
            MedicareOptOut = 32,
            BoardVerification = 39,
            FinancialAttestation = 36,
            OhioDepartmentofInsurance = 35,
            MemberComplaintsrecredOnly = 34,
            QualityofCarerecredOnly = 33,
            MaternityLicense = 38,
            FacilityLicenseVerification = 40,
            SiteVisitAndAccreditation = 37,
            UnDefined = 99
        }

        //NE Demo
        public enum ProviderCategoryType
        {
            IndividualSolo = 1,
            Group = 2,
            FacilityInstitution = 3,
            NonMedical = 4,
            Pharmacy = 5,
            GroupMemberProfile = 6
        }

        //NE Demo
        public enum ApplicationFeeWaiverReason
        {
            MedicareEnrolled = 1,
            FeePaidInAnotherState = 2
        }

        //NE Demo
        public enum ApplicationFeeStatus
        {
            Pending = 1,
            Paid = 2,
            Waived = 3
        }

        public enum ResultChangeType
        {
            None,
            ToNegativeResult,
            ToPositiveResult
        }

        public enum MoratoriaMatchResult
        {
            NoMatch = 0,
            MatchFound = 1
        }


    }
}
