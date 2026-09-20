using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace MAXIMUS.DataExchange.PDMS.CredRoster
{
    public class CredentialRosterXMLReference
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Providers
        {
            private CredRosterProviderInformation[] providerInformationField;

            /// <remarks/>
            //[System.Xml.Serialization.XmlArrayAttribute]
            //[System.Xml.Serialization.XmlArrayItemAttribute("ProviderInformation", IsNullable = false)]
            [System.Xml.Serialization.XmlElementAttribute("ProviderInformation", Order = 1)]
            public CredRosterProviderInformation[] ProviderInformation
            {
                get
                {
                    return this.providerInformationField;
                }
                set
                {
                    this.providerInformationField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderInformation
        {

            private CredRosterProviderDemographic providerDemographicField;

            private CredRosterProviderAlternateIdentifiersList[] providerAlternateIdentifiersField;

            private CredRosterProviderSpecialtiesList[] providerSpecialtiesField;

            private CredRosterProviderLicensesList[] providerLicensesField;

            private CredRosterProviderCertificationsList[] providerCertificationsField;

            private CredRosterProviderTaxonomiesList[] providerTaxonomiesField;

            private CredRosterProviderHospitalAffiliationsList[] providerHospitalAffiliationsField;

            private CredRosterProviderAffiliationsList[] providerAffiliationsField;

            private CredRosterProviderEducationList[] providerEducationField;

            private CredRosterProviderInsurance providerInsuranceField;

            private CredRosterProviderAddressList[] providerAddressField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute]
            public CredRosterProviderDemographic ProviderDemographic
            {
                get
                {
                    return this.providerDemographicField;
                }
                set
                {
                    this.providerDemographicField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("AlternateIdentifier", IsNullable = false)]
            public CredRosterProviderAlternateIdentifiersList[] ProviderAlternateIdentifiers
            {
                get
                {
                    return this.providerAlternateIdentifiersField;
                }
                set
                {
                    this.providerAlternateIdentifiersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("Specialty", IsNullable = false)]
            public CredRosterProviderSpecialtiesList[] ProviderSpecialties
            {
                get
                {
                    return this.providerSpecialtiesField;
                }
                set
                {
                    this.providerSpecialtiesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("License", IsNullable = false)]
            public CredRosterProviderLicensesList[] ProviderLicenses
            {
                get
                {
                    return this.providerLicensesField;
                }
                set
                {
                    this.providerLicensesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("Certification", IsNullable = false)]
            public CredRosterProviderCertificationsList[] ProviderCertifications
            {
                get
                {
                    return this.providerCertificationsField;
                }
                set
                {
                    this.providerCertificationsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("Taxonomy", IsNullable = false)]
            public CredRosterProviderTaxonomiesList[] ProviderTaxonomies
            {
                get
                {
                    return this.providerTaxonomiesField;
                }
                set
                {
                    this.providerTaxonomiesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("HospitalAffiliation", IsNullable = false)]
            public CredRosterProviderHospitalAffiliationsList[] ProviderHospitalAffiliations
            {
                get
                {
                    return this.providerHospitalAffiliationsField;
                }
                set
                {
                    this.providerHospitalAffiliationsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("Affiliation", IsNullable = false)]
            public CredRosterProviderAffiliationsList[] ProviderAffiliations
            {
                get
                {
                    return this.providerAffiliationsField;
                }
                set
                {
                    this.providerAffiliationsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("Education", IsNullable = false)]
            public CredRosterProviderEducationList[] ProviderEducation
            {
                get
                {
                    return this.providerEducationField;
                }
                set
                {
                    this.providerEducationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute]
            public CredRosterProviderInsurance ProviderInsurance
            {
                get
                {
                    return this.providerInsuranceField;
                }
                set
                {
                    this.providerInsuranceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("Address", IsNullable = false)]
            public CredRosterProviderAddressList[] ProviderAddress
            {
                get
                {
                    return this.providerAddressField;
                }
                set
                {
                    this.providerAddressField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderDemographic
        {

            private string recordTypeField;

            private string providerStatusTypeField;

            private string medicaidIDField;

            private string practiceNameField;

            private string lastNameField;

            private string firstNameField;

            private string middleNameField;

            private string titleField;

            private string genderField;            

            private string taxIDField;

            private string dateOfBirthField;

            private string nPIField;

            private string mITSProviderTypeField;

            private string effectiveDateField;

            private string providerCredentialDateField;

            private string providerRecredentialDueDateField;

            private string credentialingNotRequiredDateField;

            private string threeFortyBField;

            private string hospitalistField;

            private string officeUtilizePhysicianExtendersField;

            private string numberOfNPsField;

            private string numberOfPAsField;

            private string cAQHNumberField;

            private string credentialingContactNameField;

            private string credentialingContactPhoneField;

            private string credentialingContactFaxField;

            private string credentialingContactEmailField;

            private string businessContactNameField;

            private string businessContactPhoneField;

            private string businessContactFaxField;

            private string businessContactEmailAddressField;

            private string medicareOptOutField;

            /// <remarks/>
            public string RecordType
            {
                get
                {
                    return this.recordTypeField;
                }
                set
                {
                    this.recordTypeField = value;
                }
            }

            /// <remarks/>
            public string ProviderStatusType
            {
                get
                {
                    return this.providerStatusTypeField;
                }
                set
                {
                    this.providerStatusTypeField = value;
                }
            }

            /// <remarks/>
            public string MedicaidID
            {
                get
                {
                    return this.medicaidIDField;
                }
                set
                {
                    this.medicaidIDField = value;
                }
            }

            /// <remarks/>
            public string PracticeName
            {
                get
                {
                    return this.practiceNameField;
                }
                set
                {
                    this.practiceNameField = value;
                }
            }

            /// <remarks/>
            public string LastName
            {
                get
                {
                    return this.lastNameField;
                }
                set
                {
                    this.lastNameField = value;
                }
            }

            /// <remarks/>
            public string FirstName
            {
                get
                {
                    return this.firstNameField;
                }
                set
                {
                    this.firstNameField = value;
                }
            }

            /// <remarks/>
            public string MiddleName
            {
                get
                {
                    return this.middleNameField;
                }
                set
                {
                    this.middleNameField = value;
                }
            }

            /// <remarks/>
            public string Title
            {
                get
                {
                    return this.titleField;
                }
                set
                {
                    this.titleField = value;
                }
            }

            /// <remarks/>
            public string Gender
            {
                get
                {
                    return this.genderField;
                }
                set
                {
                    this.genderField = value;
                }
            }            

            /// <remarks/>
            public string TaxID
            {
                get
                {
                    return this.taxIDField;
                }
                set
                {
                    this.taxIDField = value;
                }
            }

            /// <remarks/>
            public string DateOfBirth
            {
                get
                {
                    return this.dateOfBirthField;
                }
                set
                {
                    this.dateOfBirthField = value;
                }
            }

            /// <remarks/>
            public string NPI
            {
                get
                {
                    return this.nPIField;
                }
                set
                {
                    this.nPIField = value;
                }
            }

            /// <remarks/>
            public string MITSProviderType
            {
                get
                {
                    return this.mITSProviderTypeField;
                }
                set
                {
                    this.mITSProviderTypeField = value;
                }
            }

            /// <remarks/>
            public string EffectiveDate
            {
                get
                {
                    return this.effectiveDateField;
                }
                set
                {
                    this.effectiveDateField = value;
                }
            }

            /// <remarks/>
            public string ProviderCredentialDate
            {
                get
                {
                    return this.providerCredentialDateField;
                }
                set
                {
                    this.providerCredentialDateField = value;
                }
            }

            /// <remarks/>
            public string ProviderRecredentialDueDate
            {
                get
                {
                    return this.providerRecredentialDueDateField;
                }
                set
                {
                    this.providerRecredentialDueDateField = value;
                }
            }

            /// <remarks/>
            public string CredentialingNotRequiredDate
            {
                get
                {
                    return this.credentialingNotRequiredDateField;
                }
                set
                {
                    this.credentialingNotRequiredDateField = value;
                }
            }

            /// <remarks/>
            public string ThreeFortyB
            {
                get
                {
                    return this.threeFortyBField;
                }
                set
                {
                    this.threeFortyBField = value;
                }
            }

            /// <remarks/>
            public string Hospitalist
            {
                get
                {
                    return this.hospitalistField;
                }
                set
                {
                    this.hospitalistField = value;
                }
            }

            /// <remarks/>
            public string OfficeUtilizePhysicianExtenders
            {
                get
                {
                    return this.officeUtilizePhysicianExtendersField;
                }
                set
                {
                    this.officeUtilizePhysicianExtendersField = value;
                }
            }

            /// <remarks/>
            public string NumberOfNPs
            {
                get
                {
                    return this.numberOfNPsField;
                }
                set
                {
                    this.numberOfNPsField = value;
                }
            }

            /// <remarks/>
            public string NumberOfPAs
            {
                get
                {
                    return this.numberOfPAsField;
                }
                set
                {
                    this.numberOfPAsField = value;
                }
            }

            /// <remarks/>
            public string CAQHNumber
            {
                get
                {
                    return this.cAQHNumberField;
                }
                set
                {
                    this.cAQHNumberField = value;
                }
            }

            /// <remarks/>
            public string CredentialingContactName
            {
                get
                {
                    return this.credentialingContactNameField;
                }
                set
                {
                    this.credentialingContactNameField = value;
                }
            }

            /// <remarks/>
            public string CredentialingContactPhone
            {
                get
                {
                    return this.credentialingContactPhoneField;
                }
                set
                {
                    this.credentialingContactPhoneField = value;
                }
            }

            /// <remarks/>
            public string CredentialingContactFax
            {
                get
                {
                    return this.credentialingContactFaxField;
                }
                set
                {
                    this.credentialingContactFaxField = value;
                }
            }

            /// <remarks/>
            public string CredentialingContactEmail
            {
                get
                {
                    return this.credentialingContactEmailField;
                }
                set
                {
                    this.credentialingContactEmailField = value;
                }
            }

            /// <remarks/>
            public string BusinessContactName
            {
                get
                {
                    return this.businessContactNameField;
                }
                set
                {
                    this.businessContactNameField = value;
                }
            }

            /// <remarks/>
            public string BusinessContactPhone
            {
                get
                {
                    return this.businessContactPhoneField;
                }
                set
                {
                    this.businessContactPhoneField = value;
                }
            }

            /// <remarks/>
            public string BusinessContactFax
            {
                get
                {
                    return this.businessContactFaxField;
                }
                set
                {
                    this.businessContactFaxField = value;
                }
            }

            /// <remarks/>
            public string BusinessContactEmailAddress
            {
                get
                {
                    return this.businessContactEmailAddressField;
                }
                set
                {
                    this.businessContactEmailAddressField = value;
                }
            }

            /// <remarks/>
            public string MedicareOptOut
            {
                get
                {
                    return this.medicareOptOutField;
                }
                set
                {
                    this.medicareOptOutField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderAlternateIdentifiers
        //{

        //    private ProvidersProviderInformationProviderAlternateIdentifiersAlternateIdentifier alternateIdentifierField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderAlternateIdentifiersAlternateIdentifier AlternateIdentifier
        //    {
        //        get
        //        {
        //            return this.alternateIdentifierField;
        //        }
        //        set
        //        {
        //            this.alternateIdentifierField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderAlternateIdentifiersList
        {

            private string alternateIDTypeField;

            private string alternateIdField;

            private string effectiveDateField;

            private string endDateField;

            private string additionalInformationField;

            /// <remarks/>
            public string AlternateIDType
            {
                get
                {
                    return this.alternateIDTypeField;
                }
                set
                {
                    this.alternateIDTypeField = value;
                }
            }

            /// <remarks/>
            public string AlternateId
            {
                get
                {
                    return this.alternateIdField;
                }
                set
                {
                    this.alternateIdField = value;
                }
            }

            /// <remarks/>
            public string EffectiveDate
            {
                get
                {
                    return this.effectiveDateField;
                }
                set
                {
                    this.effectiveDateField = value;
                }
            }

            /// <remarks/>
            public string EndDate
            {
                get
                {
                    return this.endDateField;
                }
                set
                {
                    this.endDateField = value;
                }
            }

            /// <remarks/>
            public string AdditionalInformation
            {
                get
                {
                    return this.additionalInformationField;
                }
                set
                {
                    this.additionalInformationField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderSpecialties
        //{

        //    private ProvidersProviderInformationProviderSpecialtiesSpecialty specialtyField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderSpecialtiesSpecialty Specialty
        //    {
        //        get
        //        {
        //            return this.specialtyField;
        //        }
        //        set
        //        {
        //            this.specialtyField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderSpecialtiesList
        {

            private string primarySpecialtyIndicatorField;

            private string specialtyCodeField;

            private string effectiveDateField;

            private string endDateField;

            /// <remarks/>
            public string PrimarySpecialtyIndicator
            {
                get
                {
                    return this.primarySpecialtyIndicatorField;
                }
                set
                {
                    this.primarySpecialtyIndicatorField = value;
                }
            }

            /// <remarks/>
            public string SpecialtyCode
            {
                get
                {
                    return this.specialtyCodeField;
                }
                set
                {
                    this.specialtyCodeField = value;
                }
            }

            /// <remarks/>
            public string EffectiveDate
            {
                get
                {
                    return this.effectiveDateField;
                }
                set
                {
                    this.effectiveDateField = value;
                }
            }

            /// <remarks/>
            public string EndDate
            {
                get
                {
                    return this.endDateField;
                }
                set
                {
                    this.endDateField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderLicenses
        //{

        //    private ProvidersProviderInformationProviderLicensesLicense licenseField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderLicensesLicense License
        //    {
        //        get
        //        {
        //            return this.licenseField;
        //        }
        //        set
        //        {
        //            this.licenseField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderLicensesList
        {

            private string licenseNumberField;

            private string licenseeStateCodeField;

            private string effectiveDateField;

            private string endDateField;

            /// <remarks/>
            public string LicenseNumber
            {
                get
                {
                    return this.licenseNumberField;
                }
                set
                {
                    this.licenseNumberField = value;
                }
            }

            /// <remarks/>
            public string LicenseeStateCode
            {
                get
                {
                    return this.licenseeStateCodeField;
                }
                set
                {
                    this.licenseeStateCodeField = value;
                }
            }

            /// <remarks/>
            public string EffectiveDate
            {
                get
                {
                    return this.effectiveDateField;
                }
                set
                {
                    this.effectiveDateField = value;
                }
            }

            /// <remarks/>
            public string EndDate
            {
                get
                {
                    return this.endDateField;
                }
                set
                {
                    this.endDateField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderCertifications
        //{

        //    private ProvidersProviderInformationProviderCertificationsCertification certificationField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderCertificationsCertification Certification
        //    {
        //        get
        //        {
        //            return this.certificationField;
        //        }
        //        set
        //        {
        //            this.certificationField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderCertificationsList
        {

            private string primaryBoardCertifiedIndicatorField;

            private string certificateNameField;

            private string effectiveDateField;

            private string endDateField;

            /// <remarks/>
            public string PrimaryBoardCertifiedIndicator
            {
                get
                {
                    return this.primaryBoardCertifiedIndicatorField;
                }
                set
                {
                    this.primaryBoardCertifiedIndicatorField = value;
                }
            }

            /// <remarks/>
            public string CertificateName
            {
                get
                {
                    return this.certificateNameField;
                }
                set
                {
                    this.certificateNameField = value;
                }
            }

            /// <remarks/>
            public string EffectiveDate
            {
                get
                {
                    return this.effectiveDateField;
                }
                set
                {
                    this.effectiveDateField = value;
                }
            }

            /// <remarks/>
            public string EndDate
            {
                get
                {
                    return this.endDateField;
                }
                set
                {
                    this.endDateField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderTaxonomies
        //{

        //    private ProvidersProviderInformationProviderTaxonomiesTaxonomy taxonomyField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderTaxonomiesTaxonomy Taxonomy
        //    {
        //        get
        //        {
        //            return this.taxonomyField;
        //        }
        //        set
        //        {
        //            this.taxonomyField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderTaxonomiesList
        {

            private string primaryTaxonomyIndicatorField;

            private string taxonomyCodeField;

            /// <remarks/>
            public string PrimaryTaxonomyIndicator
            {
                get
                {
                    return this.primaryTaxonomyIndicatorField;
                }
                set
                {
                    this.primaryTaxonomyIndicatorField = value;
                }
            }

            /// <remarks/>
            public string TaxonomyCode
            {
                get
                {
                    return this.taxonomyCodeField;
                }
                set
                {
                    this.taxonomyCodeField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderHospitalAffiliations
        //{

        //    private ProvidersProviderInformationProviderHospitalAffiliationsHospitalAffiliation hospitalAffiliationField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderHospitalAffiliationsHospitalAffiliation HospitalAffiliation
        //    {
        //        get
        //        {
        //            return this.hospitalAffiliationField;
        //        }
        //        set
        //        {
        //            this.hospitalAffiliationField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderHospitalAffiliationsList
        {

            private string primaryIndicatorField;

            private string hospitalMedicaidIDField;

            private string hospitalNameField;

            private string statusofPrivilegesField;

            private string staffCategoryField;

            private string effectiveDateField;

            private string endDateField;

            /// <remarks/>
            public string PrimaryIndicator
            {
                get
                {
                    return this.primaryIndicatorField;
                }
                set
                {
                    this.primaryIndicatorField = value;
                }
            }

            /// <remarks/>
            public string HospitalMedicaidID
            {
                get
                {
                    return this.hospitalMedicaidIDField;
                }
                set
                {
                    this.hospitalMedicaidIDField = value;
                }
            }

            /// <remarks/>
            public string HospitalName
            {
                get
                {
                    return this.hospitalNameField;
                }
                set
                {
                    this.hospitalNameField = value;
                }
            }

            /// <remarks/>
            public string StatusofPrivileges
            {
                get
                {
                    return this.statusofPrivilegesField;
                }
                set
                {
                    this.statusofPrivilegesField = value;
                }
            }

            /// <remarks/>
            public string StaffCategory
            {
                get
                {
                    return this.staffCategoryField;
                }
                set
                {
                    this.staffCategoryField = value;
                }
            }

            /// <remarks/>
            public string EffectiveDate
            {
                get
                {
                    return this.effectiveDateField;
                }
                set
                {
                    this.effectiveDateField = value;
                }
            }

            /// <remarks/>
            public string EndDate
            {
                get
                {
                    return this.endDateField;
                }
                set
                {
                    this.endDateField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderAffiliations
        //{

        //    private ProvidersProviderInformationProviderAffiliationsAffiliation affiliationField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderAffiliationsAffiliation Affiliation
        //    {
        //        get
        //        {
        //            return this.affiliationField;
        //        }
        //        set
        //        {
        //            this.affiliationField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderAffiliationsList
        {

            private string groupMedicaidIDField;

            private string uniqueLocationIDField;

            private string optOutField;

            private string effectiveDateField;

            private string endDateField;

            /// <remarks/>
            public string GroupMedicaidID
            {
                get
                {
                    return this.groupMedicaidIDField;
                }
                set
                {
                    this.groupMedicaidIDField = value;
                }
            }

            /// <remarks/>
            public string UniqueLocationID
            {
                get
                {
                    return this.uniqueLocationIDField;
                }
                set
                {
                    this.uniqueLocationIDField = value;
                }
            }

            /// <remarks/>
            public string OptOut
            {
                get
                {
                    return this.optOutField;
                }
                set
                {
                    this.optOutField = value;
                }
            }

            /// <remarks/>
            public string EffectiveDate
            {
                get
                {
                    return this.effectiveDateField;
                }
                set
                {
                    this.effectiveDateField = value;
                }
            }

            /// <remarks/>
            public string EndDate
            {
                get
                {
                    return this.endDateField;
                }
                set
                {
                    this.endDateField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderEducation
        //{

        //    private ProvidersProviderInformationProviderEducationEducation educationField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderEducationEducation Education
        //    {
        //        get
        //        {
        //            return this.educationField;
        //        }
        //        set
        //        {
        //            this.educationField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderEducationList
        {

            private string educationTypeField;

            private string providerDegreeField;

            private string schoolNameField;

            private string locationField;

            private string cityField;

            private string stateField;

            private string zipCodeField;

            private string specialtyField;

            private string effectiveDateField;

            private string endDateField;

            /// <remarks/>
            public string EducationType
            {
                get
                {
                    return this.educationTypeField;
                }
                set
                {
                    this.educationTypeField = value;
                }
            }

            /// <remarks/>
            public string ProviderDegree
            {
                get
                {
                    return this.providerDegreeField;
                }
                set
                {
                    this.providerDegreeField = value;
                }
            }

            /// <remarks/>
            public string SchoolName
            {
                get
                {
                    return this.schoolNameField;
                }
                set
                {
                    this.schoolNameField = value;
                }
            }

            /// <remarks/>
            public string Location
            {
                get
                {
                    return this.locationField;
                }
                set
                {
                    this.locationField = value;
                }
            }

            /// <remarks/>
            public string City
            {
                get
                {
                    return this.cityField;
                }
                set
                {
                    this.cityField = value;
                }
            }

            /// <remarks/>
            public string State
            {
                get
                {
                    return this.stateField;
                }
                set
                {
                    this.stateField = value;
                }
            }

            /// <remarks/>
            public string ZipCode
            {
                get
                {
                    return this.zipCodeField;
                }
                set
                {
                    this.zipCodeField = value;
                }
            }

            /// <remarks/>
            public string Specialty
            {
                get
                {
                    return this.specialtyField;
                }
                set
                {
                    this.specialtyField = value;
                }
            }

            /// <remarks/>
            public string EffectiveDate
            {
                get
                {
                    return this.effectiveDateField;
                }
                set
                {
                    this.effectiveDateField = value;
                }
            }

            /// <remarks/>
            public string EndDate
            {
                get
                {
                    return this.endDateField;
                }
                set
                {
                    this.endDateField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderInsurance
        {

            private string malpracticeCoverageIndicatorField;

            private string startDateField;

            private string expirationDateField;

            private string limitOccuranceField;

            private string limitAggregateField;

            private string carrierField;

            private string fTCAIndicatorField;

            private string policyNumberField;

            /// <remarks/>
            public string MalpracticeCoverageIndicator
            {
                get
                {
                    return this.malpracticeCoverageIndicatorField;
                }
                set
                {
                    this.malpracticeCoverageIndicatorField = value;
                }
            }

            /// <remarks/>
            public string StartDate
            {
                get
                {
                    return this.startDateField;
                }
                set
                {
                    this.startDateField = value;
                }
            }

            /// <remarks/>
            public string ExpirationDate
            {
                get
                {
                    return this.expirationDateField;
                }
                set
                {
                    this.expirationDateField = value;
                }
            }

            /// <remarks/>
            public string LimitOccurance
            {
                get
                {
                    return this.limitOccuranceField;
                }
                set
                {
                    this.limitOccuranceField = value;
                }
            }

            /// <remarks/>
            public string LimitAggregate
            {
                get
                {
                    return this.limitAggregateField;
                }
                set
                {
                    this.limitAggregateField = value;
                }
            }

            /// <remarks/>
            public string Carrier
            {
                get
                {
                    return this.carrierField;
                }
                set
                {
                    this.carrierField = value;
                }
            }

            /// <remarks/>
            public string FTCAIndicator
            {
                get
                {
                    return this.fTCAIndicatorField;
                }
                set
                {
                    this.fTCAIndicatorField = value;
                }
            }

            /// <remarks/>
            public string PolicyNumber
            {
                get
                {
                    return this.policyNumberField;
                }
                set
                {
                    this.policyNumberField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderAddress
        //{

        //    private ProvidersProviderInformationProviderAddressAddress addressField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderAddressAddress Address
        //    {
        //        get
        //        {
        //            return this.addressField;
        //        }
        //        set
        //        {
        //            this.addressField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterProviderAddressList
        {

            private string addressIDField;

            private string addressTypeField;

            private string addressLine1Field;

            private string addressLine2Field;

            private string cityField;

            private string stateField;

            private string countyCodeField;

            private string zipCodeField;

            private string countryField;

            private string addressNameField;

            private string addressFirstNameField;

            private string addressMiddleNameField;

            private string addressLastNameField;

            private string phoneNumberField;

            private string faxNumberField;

            private string emailField;

            private string publicTransportationAccessField;

            private string twentyFourHourPhoneCoverageField;

            private string buildingAccessField;

            private string examRoomField;

            private string electronicClaimSubmissionField;

            private string tDDTTYField;

            private string aSLOfferedField;

            private string locationinProviderDirectoryField;

            private string languageLineField;

            private string translationServicesField;

            private string acceptingNewPatientsField;

            private string acceptNewPatientsFromReferralOnlyField;

            private string genderOfPatientsAcceptedField;

            private string youngestPatientAcceptedField;

            private string oldestPatientAcceptedField;

            private string websiteAddressField;

            private CredRosterServiceLocationLanguagesList[] serviceLocationLanguagesField;

            private CredRosterServiceLocationTrainingsList[] serviceLocationTrainingsField;

            private CredRosterServiceLocationOfficeHoursList[] serviceLocationOfficeHoursField;

            /// <remarks/>
            public string AddressID
            {
                get
                {
                    return this.addressIDField;
                }
                set
                {
                    this.addressIDField = value;
                }
            }

            /// <remarks/>
            public string AddressType
            {
                get
                {
                    return this.addressTypeField;
                }
                set
                {
                    this.addressTypeField = value;
                }
            }

            /// <remarks/>
            public string AddressLine1
            {
                get
                {
                    return this.addressLine1Field;
                }
                set
                {
                    this.addressLine1Field = value;
                }
            }

            /// <remarks/>
            public string AddressLine2
            {
                get
                {
                    return this.addressLine2Field;
                }
                set
                {
                    this.addressLine2Field = value;
                }
            }

            /// <remarks/>
            public string City
            {
                get
                {
                    return this.cityField;
                }
                set
                {
                    this.cityField = value;
                }
            }

            /// <remarks/>
            public string State
            {
                get
                {
                    return this.stateField;
                }
                set
                {
                    this.stateField = value;
                }
            }

            /// <remarks/>
            public string CountyCode
            {
                get
                {
                    return this.countyCodeField;
                }
                set
                {
                    this.countyCodeField = value;
                }
            }

            /// <remarks/>
            public string ZipCode
            {
                get
                {
                    return this.zipCodeField;
                }
                set
                {
                    this.zipCodeField = value;
                }
            }

            /// <remarks/>
            public string Country
            {
                get
                {
                    return this.countryField;
                }
                set
                {
                    this.countryField = value;
                }
            }

            /// <remarks/>
            public string AddressName
            {
                get
                {
                    return this.addressNameField;
                }
                set
                {
                    this.addressNameField = value;
                }
            }

            /// <remarks/>
            public string AddressFirstName
            {
                get
                {
                    return this.addressFirstNameField;
                }
                set
                {
                    this.addressFirstNameField = value;
                }
            }

            /// <remarks/>
            public string AddressMiddleName
            {
                get
                {
                    return this.addressMiddleNameField;
                }
                set
                {
                    this.addressMiddleNameField = value;
                }
            }

            /// <remarks/>
            public string AddressLastName
            {
                get
                {
                    return this.addressLastNameField;
                }
                set
                {
                    this.addressLastNameField = value;
                }
            }

            /// <remarks/>
            public string PhoneNumber
            {
                get
                {
                    return this.phoneNumberField;
                }
                set
                {
                    this.phoneNumberField = value;
                }
            }

            /// <remarks/>
            public string FaxNumber
            {
                get
                {
                    return this.faxNumberField;
                }
                set
                {
                    this.faxNumberField = value;
                }
            }

            /// <remarks/>
            public string Email
            {
                get
                {
                    return this.emailField;
                }
                set
                {
                    this.emailField = value;
                }
            }

            /// <remarks/>
            public string PublicTransportationAccess
            {
                get
                {
                    return this.publicTransportationAccessField;
                }
                set
                {
                    this.publicTransportationAccessField = value;
                }
            }

            /// <remarks/>
            public string TwentyFourHourPhoneCoverage
            {
                get
                {
                    return this.twentyFourHourPhoneCoverageField;
                }
                set
                {
                    this.twentyFourHourPhoneCoverageField = value;
                }
            }

            /// <remarks/>
            public string BuildingAccess
            {
                get
                {
                    return this.buildingAccessField;
                }
                set
                {
                    this.buildingAccessField = value;
                }
            }

            /// <remarks/>
            public string ExamRoom
            {
                get
                {
                    return this.examRoomField;
                }
                set
                {
                    this.examRoomField = value;
                }
            }

            /// <remarks/>
            public string ElectronicClaimSubmission
            {
                get
                {
                    return this.electronicClaimSubmissionField;
                }
                set
                {
                    this.electronicClaimSubmissionField = value;
                }
            }

            /// <remarks/>
            public string TDDTTY
            {
                get
                {
                    return this.tDDTTYField;
                }
                set
                {
                    this.tDDTTYField = value;
                }
            }

            /// <remarks/>
            public string ASLOffered
            {
                get
                {
                    return this.aSLOfferedField;
                }
                set
                {
                    this.aSLOfferedField = value;
                }
            }

            /// <remarks/>
            public string LocationinProviderDirectory
            {
                get
                {
                    return this.locationinProviderDirectoryField;
                }
                set
                {
                    this.locationinProviderDirectoryField = value;
                }
            }

            /// <remarks/>
            public string LanguageLine
            {
                get
                {
                    return this.languageLineField;
                }
                set
                {
                    this.languageLineField = value;
                }
            }

            /// <remarks/>
            public string TranslationServices
            {
                get
                {
                    return this.translationServicesField;
                }
                set
                {
                    this.translationServicesField = value;
                }
            }

            /// <remarks/>
            public string AcceptingNewPatients
            {
                get
                {
                    return this.acceptingNewPatientsField;
                }
                set
                {
                    this.acceptingNewPatientsField = value;
                }
            }

            /// <remarks/>
            public string AcceptNewPatientsFromReferralOnly
            {
                get
                {
                    return this.acceptNewPatientsFromReferralOnlyField;
                }
                set
                {
                    this.acceptNewPatientsFromReferralOnlyField = value;
                }
            }

            /// <remarks/>
            public string GenderOfPatientsAccepted
            {
                get
                {
                    return this.genderOfPatientsAcceptedField;
                }
                set
                {
                    this.genderOfPatientsAcceptedField = value;
                }
            }

            /// <remarks/>
            public string YoungestPatientAccepted
            {
                get
                {
                    return this.youngestPatientAcceptedField;
                }
                set
                {
                    this.youngestPatientAcceptedField = value;
                }
            }

            /// <remarks/>
            public string OldestPatientAccepted
            {
                get
                {
                    return this.oldestPatientAcceptedField;
                }
                set
                {
                    this.oldestPatientAcceptedField = value;
                }
            }

            /// <remarks/>
            public string WebsiteAddress
            {
                get
                {
                    return this.websiteAddressField;
                }
                set
                {
                    this.websiteAddressField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("Language", IsNullable = false)]
            public CredRosterServiceLocationLanguagesList[] ServiceLocationLanguages
            {
                get
                {
                    return this.serviceLocationLanguagesField;
                }
                set
                {
                    this.serviceLocationLanguagesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("Training", IsNullable = false)]
            public CredRosterServiceLocationTrainingsList[] ServiceLocationTrainings
            {
                get
                {
                    return this.serviceLocationTrainingsField;
                }
                set
                {
                    this.serviceLocationTrainingsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute]
            [System.Xml.Serialization.XmlArrayItemAttribute("OfficeHours", IsNullable = false)]
            public CredRosterServiceLocationOfficeHoursList[] ServiceLocationOfficeHours
            {
                get
                {
                    return this.serviceLocationOfficeHoursField;
                }
                set
                {
                    this.serviceLocationOfficeHoursField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderAddressAddressServiceLocationLanguages
        //{

        //    private ProvidersProviderInformationProviderAddressAddressServiceLocationLanguagesLanguage languageField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderAddressAddressServiceLocationLanguagesLanguage Language
        //    {
        //        get
        //        {
        //            return this.languageField;
        //        }
        //        set
        //        {
        //            this.languageField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterServiceLocationLanguagesList
        {

            private string languageSpokenByField;

            private string languageCodeField;

            /// <remarks/>
            public string LanguageSpokenBy
            {
                get
                {
                    return this.languageSpokenByField;
                }
                set
                {
                    this.languageSpokenByField = value;
                }
            }

            /// <remarks/>
            public string LanguageCode
            {
                get
                {
                    return this.languageCodeField;
                }
                set
                {
                    this.languageCodeField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderAddressAddressServiceLocationTrainings
        //{

        //    private ProvidersProviderInformationProviderAddressAddressServiceLocationTrainingsTraining trainingField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderAddressAddressServiceLocationTrainingsTraining Training
        //    {
        //        get
        //        {
        //            return this.trainingField;
        //        }
        //        set
        //        {
        //            this.trainingField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterServiceLocationTrainingsList
        {
            private string trainingByField;

            private string trainingTypeField;

            private string trainingIndicatorField;

            /// <remarks/>
            public string TrainingBy
            {
                get
                {
                    return this.trainingByField;
                }
                set
                {
                    this.trainingByField = value;
                }
            }

            /// <remarks/>
            public string TrainingType
            {
                get
                {
                    return this.trainingTypeField;
                }
                set
                {
                    this.trainingTypeField = value;
                }
            }

            /// <remarks/>
            public string TrainingIndicator
            {
                get
                {
                    return this.trainingIndicatorField;
                }
                set
                {
                    this.trainingIndicatorField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class ProvidersProviderInformationProviderAddressAddressServiceLocationOfficeHours
        //{

        //    private ProvidersProviderInformationProviderAddressAddressServiceLocationOfficeHoursOfficeHours officeHoursField;

        //    /// <remarks/>
        //    public ProvidersProviderInformationProviderAddressAddressServiceLocationOfficeHoursOfficeHours OfficeHours
        //    {
        //        get
        //        {
        //            return this.officeHoursField;
        //        }
        //        set
        //        {
        //            this.officeHoursField = value;
        //        }
        //    }
        //}

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class CredRosterServiceLocationOfficeHoursList
        {

            private string dayField;

            private string startTimeField;

            private string endTimeField;

            private string open24HoursIndicatorField;

            /// <remarks/>
            public string Day
            {
                get
                {
                    return this.dayField;
                }
                set
                {
                    this.dayField = value;
                }
            }

            /// <remarks/>
            public string StartTime
            {
                get
                {
                    return this.startTimeField;
                }
                set
                {
                    this.startTimeField = value;
                }
            }

            /// <remarks/>
            public string EndTime
            {
                get
                {
                    return this.endTimeField;
                }
                set
                {
                    this.endTimeField = value;
                }
            }

            /// <remarks/>
            public string Open24HoursIndicator
            {
                get
                {
                    return this.open24HoursIndicatorField;
                }
                set
                {
                    this.open24HoursIndicatorField = value;
                }
            }
        }


    }
}
