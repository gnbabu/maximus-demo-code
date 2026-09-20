using FileHelpers;
using System;

namespace MAXIMUS.DataExchange.PDMS.CredRoster
{
    [DelimitedRecord(",")]
    public class CredentialRosterRecord
    {
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String practiceName;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providersLastName;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providersFirstName;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providersMiddleName;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providersTitle;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providersGender;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providersDOB;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providerWebsiteAddress;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providersSocialSecurity;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String mITSProviderType;//Yes
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String existingGroup;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String newGroup;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String hospitalist;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String officeutilizePhysicianExtenders;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String ifyesnumberofNPs;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String ifyesnumberofPAs;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String credentialingContactname;//0	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String credentialingContactphone;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String credentialingContactFax;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String credentialingContactemail;//2	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providerCredentialDate;//Yes
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providerRecredentialDueDate;//Yes
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providerStatustype;//	Yes
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryMITSSpecialty;//Yes
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryBoardCertified_YorN;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryBoardCertificationName;//5	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryBoardInitialCertificationDate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryBoardExpirationDate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String otherSpecialties1;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String otherSpecialties2;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String otherSpecialties3;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String otherSpecialties4;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String otherBoardCertified_YorN;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String boardCertificationName;//5	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String boardCertificationInitialCertificationDate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String boardCertexpirationdate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String malpracticeCoverage_YorN;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String malpracticecoveragestartdate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String malpracticecoverageexpirationdate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String malpracticeCoverageLimitOccurrence;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String malpracticeCoverageLimitAggregate;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String malpracticeCarrier;//0	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String fTCA_YorN;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String policyNumber;//0	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String acceptingNewPatients;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String acceptnewpatientsfromreferralonly;//No
        //[FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        //public String maximumnumberofmembersaccepted;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String genderofpatientsaccepted;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String youngestpatientaccepted;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String oldestpatientaccepted;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String groupOrBillingNPI;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String individualNPI;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String groupMedicaidNumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String groupMedicaidEffectiveDate;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String individualMedicaidNumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String individualMedicaidEffectiveDate;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String medicareNumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String medicareEffectiveDate;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String medicareOptout;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String threeFortyB;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryTaxonomy;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalTaxonomy1;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalTaxonomy2;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalTaxonomy3;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dEANumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dEAIssueDate;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dEAExpirationDate;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dEAData2000Waiver;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalDEANumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalDEAIssueDate;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalDEAExpirationDate;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String stateLicenseNumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String licenseState;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dateStateLicenseIssued;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dateStateLicenseExpires;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalStateLicenseNumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String licenseState1;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dateStateLicenseIssued1;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dateStateLicenseExpires1;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalStateLicenseNumber1;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String licenseState2;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dateStateLicenseIssued2;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dateStateLicenseExpires2;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalStateLicenseNumber2;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String licenseState3;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dateStateLicenseIssued3;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String dateStateLicenseExpires3;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String cAQHNumber;//0	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryHospitalAffiliationMedicaidID;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryHospitalAffiliationStatus;//6	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryHospitalName;//6	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalHospitalAffiliationMedicaidID;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalHospitalAffiliationStatus;//6	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalHospitalName;//6	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String delegatedCredentialing1;//6	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String delegatedCredentialing2;//6	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String delegatedCredentialing3;//6	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String providersDegree;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String medicalSchoolName;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String medicalSchoolStartDate;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String medicalSchoolEndDate;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String residencyLocation;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String residencyCity;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String residencyState;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String residencyStartDate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String residencyEndDate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String residencySpecialty;//5	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String internshipLocation;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String internshipCity;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String internshipState;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String internshipStartDate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String internshipEndDate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String internshipSpecialty;//5	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String fellowshipLocation;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String fellowshipCity;//	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String fellowshipState;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String fellowshipStartDate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String fellowshipEndDate;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String fellowshipSpecialty;//5	Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String developmentallyDisabledSpecailizedTraining;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String physicallyDisabledSpecailizedTraining;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String chronicIllnessSpecialTraining;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String hIVAIDSSpecialTraining;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String mentalIllnessSpecialTraining;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String substanceAbuseSpecialTraining;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String homelessnessSpecialTraining;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String deafnessorhardofhearingSpecialTraining;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String blindnessorvisualimpairmentSpecialTraining;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String coOccuringDisordersSpecialTraining;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String languagesspokenbyProvider;//6	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String cuturalCompetancyTrainingAfricanAmerican;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String cuturalCompetancyTrainingAlaskanNative;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String cuturalCompetancyTrainingAmericanIndian;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String cuturalCompetancyTrainingAsian;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String cuturalCompetancyTrainingHispanicLatino;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String cuturalCompetancyTrainingPacificIslander;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String cuturalCompetancyTrainingLBGTQ;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String businessContactName;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String businessContactPhone;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String businessContactFax;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String businessContactEmailAddress;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryAddressLine1;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryAddressLine2;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryAddressCity;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryAddressState;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryAddressZip;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryAddressCounty;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryAddressPhoneNumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryAddressFaxNumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryAddressEmailAddress;//2	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String primaryAddressCountry;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String officeHoursMonday;//8	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String officeHoursTuesday;//8	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String officeHoursWednesday;//8	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String officeHoursThursday;//8	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String officeHoursFriday;//8	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String officeHoursSaturday;//8	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String officeHoursSunday;//8	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String publictransportationaccess;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String twentyFourHourPhoneCoverage;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String buildingAccess;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String examRoom;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String electronicClaimSubmission;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String languagesspokenbyOfficeStaff;//6	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String languageLine;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String translationServices;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String tDDTTY;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String aSLOffered;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String locationintheproviderdirectory;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String cLIANumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String cLIANumberExpirationDate;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressOrganizationName;//0	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressFirstName;//0	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressLastName;//0	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressLine1;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressLine2;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressGroupCity;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressGroupState;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressGroupZip;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressGroupPhoneNumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressGroupFaxNumber;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressBillingContactName;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String paytoAddressBillingEmailAddress;//2	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String groupTaxIDNumber;//Conditional
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressName_1;//0	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressLine1_1;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressLine2_1;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCity_1;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressState_1;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressZip_1;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCounty_1;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressPhoneNumber_1;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressFaxNumber_1;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressEmailAddress_1;//2	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCountry_1;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressName_2;//0	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressLine1_2;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressLine2_2;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCity_2;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressState_2;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressZip_2;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCounty_2;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressPhoneNumber_2;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressFaxNumber_2;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressEmailAddress_2;//2	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCountry_2;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressName_3;//0	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressLine1_3;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressLine2_3;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCity_3;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressState_3;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressZip_3;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCounty_3;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressPhoneNumber_3;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressFaxNumber_3;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressEmailAddress_3;//2	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCountry_3;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressName_4;//0	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressLine1_4;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressLine2_4;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCity_4;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressState_4;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressZip_4;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCounty_4;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressPhoneNumber_4;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressFaxNumber_4;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressEmailAddress_4;//2	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCountry_4;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressName_5;//0	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressLine1_5;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressLine2_5;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCity_5;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressState_5;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressZip_5;//No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCounty_5;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressPhoneNumber_5;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressFaxNumber_5;//	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressEmailAddress_5;//2	No
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public String additionalAddressCountry_5;

        public string PracticeName { get => practiceName; set => practiceName = value; }
        public string ProvidersLastName { get => providersLastName; set => providersLastName = value; }
        public string ProvidersFirstName { get => providersFirstName; set => providersFirstName = value; }
        public string ProvidersMiddleName { get => providersMiddleName; set => providersMiddleName = value; }
        public string ProvidersTitle { get => providersTitle; set => providersTitle = value; }
        public String ProvidersGender { get => providersGender; set => providersGender = value; }
        public String ProvidersDOB { get => providersDOB; set => providersDOB = value; }
        public String ProviderWebsiteAddress { get => providerWebsiteAddress; set => providerWebsiteAddress = value; }
        public String ProvidersSocialSecurity { get => providersSocialSecurity; set => providersSocialSecurity = value; }
        public String MITSProviderType { get => mITSProviderType; set => mITSProviderType = value; }
        public String ExistingGroup { get => existingGroup; set => existingGroup = value; }
        public String NewGroup { get => newGroup; set => newGroup = value; }
        public String Hospitalist { get => hospitalist; set => hospitalist = value; }
        public String OfficeutilizePhysicianExtenders { get => officeutilizePhysicianExtenders; set => officeutilizePhysicianExtenders = value; }
        public String IfyesnumberofNPs { get => ifyesnumberofNPs; set => ifyesnumberofNPs = value; }
        public String IfyesnumberofPAs { get => ifyesnumberofPAs; set => ifyesnumberofPAs = value; }
        public String CredentialingContactname { get => credentialingContactname; set => credentialingContactname = value; }
        public String CredentialingContactphone { get => credentialingContactphone; set => credentialingContactphone = value; }
        public String CredentialingContactFax { get => credentialingContactFax; set => credentialingContactFax = value; }
        public String CredentialingContactemail { get => credentialingContactemail; set => credentialingContactemail = value; }
        public String ProviderCredentialDate { get => providerCredentialDate; set => providerCredentialDate = value; }
        public String ProviderRecredentialDueDate { get => providerRecredentialDueDate; set => providerRecredentialDueDate = value; }
        public String ProviderStatustype { get => providerStatustype; set => providerStatustype = value; }
        public String PrimaryMITSSpecialty { get => primaryMITSSpecialty; set => primaryMITSSpecialty = value; }
        public String PrimaryBoardCertified_YorN { get => primaryBoardCertified_YorN; set => primaryBoardCertified_YorN = value; }
        public String PrimaryBoardCertificationName { get => primaryBoardCertificationName; set => primaryBoardCertificationName = value; }
        public String PrimaryBoardInitialCertificationDate { get => primaryBoardInitialCertificationDate; set => primaryBoardInitialCertificationDate = value; }
        public String PrimaryBoardExpirationDate { get => primaryBoardExpirationDate; set => primaryBoardExpirationDate = value; }
        public String OtherSpecialties1 { get => otherSpecialties1; set => otherSpecialties1 = value; }
        public String OtherSpecialties2 { get => otherSpecialties2; set => otherSpecialties2 = value; }
        public String OtherSpecialties3 { get => otherSpecialties3; set => otherSpecialties3 = value; }
        public String OtherSpecialties4 { get => otherSpecialties4; set => otherSpecialties4 = value; }
        public String OtherBoardCertified_YorN { get => otherBoardCertified_YorN; set => otherBoardCertified_YorN = value; }
        public String BoardCertificationName { get => boardCertificationName; set => boardCertificationName = value; }
        public String BoardCertificationInitialCertificationDate { get => boardCertificationInitialCertificationDate; set => boardCertificationInitialCertificationDate = value; }
        public String BoardCertexpirationdate { get => boardCertexpirationdate; set => boardCertexpirationdate = value; }
        public String MalpracticeCoverage_YorN { get => malpracticeCoverage_YorN; set => malpracticeCoverage_YorN = value; }
        public String Malpracticecoveragestartdate { get => malpracticecoveragestartdate; set => malpracticecoveragestartdate = value; }
        public String Malpracticecoverageexpirationdate { get => malpracticecoverageexpirationdate; set => malpracticecoverageexpirationdate = value; }
        public String MalpracticeCoverageLimitOccurrence { get => malpracticeCoverageLimitOccurrence; set => malpracticeCoverageLimitOccurrence = value; }
        public String MalpracticeCoverageLimitAggregate { get => malpracticeCoverageLimitAggregate; set => malpracticeCoverageLimitAggregate = value; }
        public String MalpracticeCarrier { get => malpracticeCarrier; set => malpracticeCarrier = value; }
        public String FTCA_YorN { get => fTCA_YorN; set => fTCA_YorN = value; }
        public String PolicyNumber { get => policyNumber; set => policyNumber = value; }
        public String AcceptingNewPatients { get => acceptingNewPatients; set => acceptingNewPatients = value; }
        public String Acceptnewpatientsfromreferralonly { get => acceptnewpatientsfromreferralonly; set => acceptnewpatientsfromreferralonly = value; }
        //public String Maximumnumberofmembersaccepted { get => maximumnumberofmembersaccepted; set => maximumnumberofmembersaccepted = value; }
        public String Genderofpatientsaccepted { get => genderofpatientsaccepted; set => genderofpatientsaccepted = value; }
        public String Youngestpatientaccepted { get => youngestpatientaccepted; set => youngestpatientaccepted = value; }
        public String Oldestpatientaccepted { get => oldestpatientaccepted; set => oldestpatientaccepted = value; }
        public String GroupOrBillingNPI { get => groupOrBillingNPI; set => groupOrBillingNPI = value; }
        public String IndividualNPI { get => individualNPI; set => individualNPI = value; }
        public String GroupMedicaidNumber { get => groupMedicaidNumber; set => groupMedicaidNumber = value; }
        public String GroupMedicaidEffectiveDate { get => groupMedicaidEffectiveDate; set => groupMedicaidEffectiveDate = value; }
        public String IndividualMedicaidNumber { get => individualMedicaidNumber; set => individualMedicaidNumber = value; }
        public String IndividualMedicaidEffectiveDate { get => individualMedicaidEffectiveDate; set => individualMedicaidEffectiveDate = value; }
        public String MedicareNumber { get => medicareNumber; set => medicareNumber = value; }
        public String MedicareEffectiveDate { get => medicareEffectiveDate; set => medicareEffectiveDate = value; }
        public String MedicareOptout { get => medicareOptout; set => medicareOptout = value; }
        public String ThreeFortyB { get => threeFortyB; set => threeFortyB = value; }
        public String PrimaryTaxonomy { get => primaryTaxonomy; set => primaryTaxonomy = value; }
        public String AdditionalTaxonomy1 { get => additionalTaxonomy1; set => additionalTaxonomy1 = value; }
        public String AdditionalTaxonomy2 { get => additionalTaxonomy2; set => additionalTaxonomy2 = value; }
        public String AdditionalTaxonomy3 { get => additionalTaxonomy3; set => additionalTaxonomy3 = value; }
        public String DEANumber { get => dEANumber; set => dEANumber = value; }
        public String DEAIssueDate { get => dEAIssueDate; set => dEAIssueDate = value; }
        public String DEAExpirationDate { get => dEAExpirationDate; set => dEAExpirationDate = value; }
        public String DEAData2000Waiver { get => dEAData2000Waiver; set => dEAData2000Waiver = value; }
        public String AdditionalDEANumber { get => additionalDEANumber; set => additionalDEANumber = value; }
        public String AdditionalDEAIssueDate { get => additionalDEAIssueDate; set => additionalDEAIssueDate = value; }
        public String AdditionalDEAExpirationDate { get => additionalDEAExpirationDate; set => additionalDEAExpirationDate = value; }
        public String StateLicenseNumber { get => stateLicenseNumber; set => stateLicenseNumber = value; }
        public String LicenseState { get => licenseState; set => licenseState = value; }
        public String DateStateLicenseIssued { get => dateStateLicenseIssued; set => dateStateLicenseIssued = value; }
        public String DateStateLicenseExpires { get => dateStateLicenseExpires; set => dateStateLicenseExpires = value; }
        public String AdditionalStateLicenseNumber { get => additionalStateLicenseNumber; set => additionalStateLicenseNumber = value; }
        public String LicenseState1 { get => licenseState1; set => licenseState1 = value; }
        public String DateStateLicenseIssued1 { get => dateStateLicenseIssued1; set => dateStateLicenseIssued1 = value; }
        public String DateStateLicenseExpires1 { get => dateStateLicenseExpires1; set => dateStateLicenseExpires1 = value; }
        public String AdditionalStateLicenseNumber1 { get => additionalStateLicenseNumber1; set => additionalStateLicenseNumber1 = value; }
        public String LicenseState2 { get => licenseState2; set => licenseState2 = value; }
        public String DateStateLicenseIssued2 { get => dateStateLicenseIssued2; set => dateStateLicenseIssued2 = value; }
        public String DateStateLicenseExpires2 { get => dateStateLicenseExpires2; set => dateStateLicenseExpires2 = value; }
        public String AdditionalStateLicenseNumber2 { get => additionalStateLicenseNumber2; set => additionalStateLicenseNumber2 = value; }
        public String LicenseState3 { get => licenseState3; set => licenseState3 = value; }
        public String DateStateLicenseIssued3 { get => dateStateLicenseIssued3; set => dateStateLicenseIssued3 = value; }
        public String DateStateLicenseExpires3 { get => dateStateLicenseExpires3; set => dateStateLicenseExpires3 = value; }
        public String CAQHNumber { get => cAQHNumber; set => cAQHNumber = value; }
        public String PrimaryHospitalAffiliationMedicaidID { get => primaryHospitalAffiliationMedicaidID; set => primaryHospitalAffiliationMedicaidID = value; }
        public String PrimaryHospitalAffiliationStatus { get => primaryHospitalAffiliationStatus; set => primaryHospitalAffiliationStatus = value; }
        public String PrimaryHospitalName { get => primaryHospitalName; set => primaryHospitalName = value; }
        public String AdditionalHospitalAffiliationMedicaidID { get => additionalHospitalAffiliationMedicaidID; set => additionalHospitalAffiliationMedicaidID = value; }
        public String AdditionalHospitalAffiliationStatus { get => additionalHospitalAffiliationStatus; set => additionalHospitalAffiliationStatus = value; }
        public String AdditionalHospitalName{ get => additionalHospitalName; set => additionalHospitalName = value; }
        public String DelegatedCredentialing1 { get => delegatedCredentialing1; set => delegatedCredentialing1 = value; }
        public String DelegatedCredentialing2 { get => delegatedCredentialing2; set => delegatedCredentialing2 = value; }
        public String DelegatedCredentialing3 { get => delegatedCredentialing3; set => delegatedCredentialing3 = value; }
        public String ProvidersDegree { get => providersDegree; set => providersDegree = value; }
        public String MedicalSchoolName { get => medicalSchoolName; set => medicalSchoolName = value; }
        public String MedicalSchoolStartDate { get => medicalSchoolStartDate; set => medicalSchoolStartDate = value; }
        public String MedicalSchoolEndDate { get => medicalSchoolEndDate; set => medicalSchoolEndDate = value; }
        public String ResidencyLocation { get => residencyLocation; set => residencyLocation = value; }
        public String ResidencyCity { get => residencyCity; set => residencyCity = value; }
        public String ResidencyState { get => residencyState; set => residencyState = value; }
        public String ResidencyStartDate { get => residencyStartDate; set => residencyStartDate = value; }
        public String ResidencyEndDate { get => residencyEndDate; set => residencyEndDate = value; }
        public String ResidencySpecialty { get => residencySpecialty; set => residencySpecialty = value; }
        public String InternshipLocation { get => internshipLocation; set => internshipLocation = value; }
        public String InternshipCity { get => internshipCity; set => internshipCity = value; }
        public String InternshipState { get => internshipState; set => internshipState = value; }
        public String InternshipStartDate { get => internshipStartDate; set => internshipStartDate = value; }
        public String InternshipEndDate { get => internshipEndDate; set => internshipEndDate = value; }
        public String InternshipSpecialty { get => internshipSpecialty; set => internshipSpecialty = value; }
        public String FellowshipLocation { get => fellowshipLocation; set => fellowshipLocation = value; }
        public String FellowshipCity { get => fellowshipCity; set => fellowshipCity = value; }
        public String FellowshipState { get => fellowshipState; set => fellowshipState = value; }
        public String FellowshipStartDate { get => fellowshipStartDate; set => fellowshipStartDate = value; }
        public String FellowshipEndDate { get => fellowshipEndDate; set => fellowshipEndDate = value; }
        public String FellowshipSpecialty { get => fellowshipSpecialty; set => fellowshipSpecialty = value; }
        public String DevelopmentallyDisabledSpecailizedTraining { get => developmentallyDisabledSpecailizedTraining; set => developmentallyDisabledSpecailizedTraining = value; }
        public String PhysicallyDisabledSpecailizedTraining { get => physicallyDisabledSpecailizedTraining; set => physicallyDisabledSpecailizedTraining = value; }
        public String ChronicIllnessSpecialTraining { get => chronicIllnessSpecialTraining; set => chronicIllnessSpecialTraining = value; }
        public String HIVAIDSSpecialTraining { get => hIVAIDSSpecialTraining; set => hIVAIDSSpecialTraining = value; }
        public String MentalIllnessSpecialTraining { get => mentalIllnessSpecialTraining; set => mentalIllnessSpecialTraining = value; }
        public String SubstanceAbuseSpecialTraining { get => substanceAbuseSpecialTraining; set => substanceAbuseSpecialTraining = value; }
        public String HomelessnessSpecialTraining { get => homelessnessSpecialTraining; set => homelessnessSpecialTraining = value; }
        public String DeafnessorhardofhearingSpecialTraining { get => deafnessorhardofhearingSpecialTraining; set => deafnessorhardofhearingSpecialTraining = value; }
        public String BlindnessorvisualimpairmentSpecialTraining { get => blindnessorvisualimpairmentSpecialTraining; set => blindnessorvisualimpairmentSpecialTraining = value; }
        public String CoOccuringDisordersSpecialTraining { get => coOccuringDisordersSpecialTraining; set => coOccuringDisordersSpecialTraining = value; }
        public String LanguagesspokenbyProvider { get => languagesspokenbyProvider; set => languagesspokenbyProvider = value; }
        public String CuturalCompetancyTrainingAfricanAmerican { get => cuturalCompetancyTrainingAfricanAmerican; set => cuturalCompetancyTrainingAfricanAmerican = value; }
        public String CuturalCompetancyTrainingAlaskanNative { get => cuturalCompetancyTrainingAlaskanNative; set => cuturalCompetancyTrainingAlaskanNative = value; }
        public String CuturalCompetancyTrainingAmericanIndian { get => cuturalCompetancyTrainingAmericanIndian; set => cuturalCompetancyTrainingAmericanIndian = value; }
        public String CuturalCompetancyTrainingAsian { get => cuturalCompetancyTrainingAsian; set => cuturalCompetancyTrainingAsian = value; }
        public String CuturalCompetancyTrainingHispanicLatino { get => cuturalCompetancyTrainingHispanicLatino; set => cuturalCompetancyTrainingHispanicLatino = value; }
        public String CuturalCompetancyTrainingPacificIslander { get => cuturalCompetancyTrainingPacificIslander; set => cuturalCompetancyTrainingPacificIslander = value; }
        public String CuturalCompetancyTrainingLBGTQ { get => cuturalCompetancyTrainingLBGTQ; set => cuturalCompetancyTrainingLBGTQ = value; }
        public String BusinessContactName { get => businessContactName; set => businessContactName = value; }
        public String BusinessContactPhone { get => businessContactPhone; set => businessContactPhone = value; }
        public String BusinessContactFax { get => businessContactFax; set => businessContactFax = value; }
        public String BusinessContactEmailAddress { get => businessContactEmailAddress; set => businessContactEmailAddress = value; }
        public String PrimaryAddressLine1 { get => primaryAddressLine1; set => primaryAddressLine1 = value; }
        public String PrimaryAddressLine2 { get => primaryAddressLine2; set => primaryAddressLine2 = value; }
        public String PrimaryAddressCity { get => primaryAddressCity; set => primaryAddressCity = value; }
        public String PrimaryAddressState { get => primaryAddressState; set => primaryAddressState = value; }
        public String PrimaryAddressZip { get => primaryAddressZip; set => primaryAddressZip = value; }
        public String PrimaryAddressCounty { get => primaryAddressCounty; set => primaryAddressCounty = value; }
        public String PrimaryAddressPhoneNumber { get => primaryAddressPhoneNumber; set => primaryAddressPhoneNumber = value; }
        public String PrimaryAddressFaxNumber { get => primaryAddressFaxNumber; set => primaryAddressFaxNumber = value; }
        public String PrimaryAddressEmailAddress { get => primaryAddressEmailAddress; set => primaryAddressEmailAddress = value; }
        public String PrimaryAddressCountry { get => primaryAddressCountry; set => primaryAddressCountry = value; }
        public String OfficeHoursMonday { get => officeHoursMonday; set => officeHoursMonday = value; }
        public String OfficeHoursTuesday { get => officeHoursTuesday; set => officeHoursTuesday = value; }
        public String OfficeHoursWednesday { get => officeHoursWednesday; set => officeHoursWednesday = value; }
        public String OfficeHoursThursday { get => officeHoursThursday; set => officeHoursThursday = value; }
        public String OfficeHoursFriday { get => officeHoursFriday; set => officeHoursFriday = value; }
        public String OfficeHoursSaturday { get => officeHoursSaturday; set => officeHoursSaturday = value; }
        public String OfficeHoursSunday { get => officeHoursSunday; set => officeHoursSunday = value; }
        public String Publictransportationaccess { get => publictransportationaccess; set => publictransportationaccess = value; }
        public String TwentyFourHourPhoneCoverage { get => twentyFourHourPhoneCoverage; set => twentyFourHourPhoneCoverage = value; }
        public String BuildingAccess { get => buildingAccess; set => buildingAccess = value; }
        public String ExamRoom { get => examRoom; set => examRoom = value; }
        public String ElectronicClaimSubmission { get => electronicClaimSubmission; set => electronicClaimSubmission = value; }
        public String LanguagesspokenbyOfficeStaff { get => languagesspokenbyOfficeStaff; set => languagesspokenbyOfficeStaff = value; }
        public String LanguageLine { get => languageLine; set => languageLine = value; }
        public String TranslationServices { get => translationServices; set => translationServices = value; }
        public String TDDTTY { get => tDDTTY; set => tDDTTY = value; }
        public String ASLOffered { get => aSLOffered; set => aSLOffered = value; }
        public String Locationintheproviderdirectory { get => locationintheproviderdirectory; set => locationintheproviderdirectory = value; }
        public String CLIANumber { get => cLIANumber; set => cLIANumber = value; }
        public String CLIANumberExpirationDate { get => cLIANumberExpirationDate; set => cLIANumberExpirationDate = value; }
        public String PaytoAddressOrganizationName { get => paytoAddressOrganizationName; set => paytoAddressOrganizationName = value; }
        public String PaytoAddressFirstName { get => paytoAddressFirstName; set => paytoAddressFirstName = value; }
        public String PaytoAddressLastName { get => paytoAddressLastName; set => paytoAddressLastName = value; }
        public String PaytoAddressLine1 { get => paytoAddressLine1; set => paytoAddressLine1 = value; }
        public String PaytoAddressLine2 { get => paytoAddressLine2; set => paytoAddressLine2 = value; }
        public String PaytoAddressGroupCity { get => paytoAddressGroupCity; set => paytoAddressGroupCity = value; }
        public String PaytoAddressGroupState { get => paytoAddressGroupState; set => paytoAddressGroupState = value; }
        public String PaytoAddressGroupZip { get => paytoAddressGroupZip; set => paytoAddressGroupZip = value; }
        public String PaytoAddressGroupPhoneNumber { get => paytoAddressGroupPhoneNumber; set => paytoAddressGroupPhoneNumber = value; }
        public String PaytoAddressGroupFaxNumber { get => paytoAddressGroupFaxNumber; set => paytoAddressGroupFaxNumber = value; }
        public String PaytoAddressBillingContactName { get => paytoAddressBillingContactName; set => paytoAddressBillingContactName = value; }
        public String PaytoAddressBillingEmailAddress { get => paytoAddressBillingEmailAddress; set => paytoAddressBillingEmailAddress = value; }
        public String GroupTaxIDNumber { get => groupTaxIDNumber; set => groupTaxIDNumber = value; }
        public String AdditionalAddressName_1 { get => additionalAddressName_1; set => additionalAddressName_1 = value; }
        public String AdditionalAddressLine1_1 { get => additionalAddressLine1_1; set => additionalAddressLine1_1 = value; }
        public String AdditionalAddressLine2_1 { get => additionalAddressLine2_1; set => additionalAddressLine2_1 = value; }
        public String AdditionalAddressCity_1 { get => additionalAddressCity_1; set => additionalAddressCity_1 = value; }
        public String AdditionalAddressState_1 { get => additionalAddressState_1; set => additionalAddressState_1 = value; }
        public String AdditionalAddressZip_1 { get => additionalAddressZip_1; set => additionalAddressZip_1 = value; }
        public String AdditionalAddressCounty_1 { get => additionalAddressCounty_1; set => additionalAddressCounty_1 = value; }
        public String AdditionalAddressPhoneNumber_1 { get => additionalAddressPhoneNumber_1; set => additionalAddressPhoneNumber_1 = value; }
        public String AdditionalAddressFaxNumber_1 { get => additionalAddressFaxNumber_1; set => additionalAddressFaxNumber_1 = value; }
        public String AdditionalAddressEmailAddress_1 { get => additionalAddressEmailAddress_1; set => additionalAddressEmailAddress_1 = value; }
        public String AdditionalAddressCountry_1 { get => additionalAddressCountry_1; set => additionalAddressCountry_1 = value; }
        public String AdditionalAddressName_2 { get => additionalAddressName_2; set => additionalAddressName_2 = value; }
        public String AdditionalAddressLine1_2 { get => additionalAddressLine1_2; set => additionalAddressLine1_2 = value; }
        public String AdditionalAddressLine2_2 { get => additionalAddressLine2_2; set => additionalAddressLine2_2 = value; }
        public String AdditionalAddressCity_2 { get => additionalAddressCity_2; set => additionalAddressCity_2 = value; }
        public String AdditionalAddressState_2 { get => additionalAddressState_2; set => additionalAddressState_2 = value; }
        public String AdditionalAddressZip_2 { get => additionalAddressZip_2; set => additionalAddressZip_2 = value; }
        public String AdditionalAddressCounty_2 { get => additionalAddressCounty_2; set => additionalAddressCounty_2 = value; }
        public String AdditionalAddressPhoneNumber_2 { get => additionalAddressPhoneNumber_2; set => additionalAddressPhoneNumber_2 = value; }
        public String AdditionalAddressFaxNumber_2 { get => additionalAddressFaxNumber_2; set => additionalAddressFaxNumber_2 = value; }
        public String AdditionalAddressEmailAddress_2 { get => additionalAddressEmailAddress_2; set => additionalAddressEmailAddress_2 = value; }
        public String AdditionalAddressCountry_2 { get => additionalAddressCountry_2; set => additionalAddressCountry_2 = value; }
        public String AdditionalAddressName_3 { get => additionalAddressName_3; set => additionalAddressName_3 = value; }
        public String AdditionalAddressLine1_3 { get => additionalAddressLine1_3; set => additionalAddressLine1_3 = value; }
        public String AdditionalAddressLine2_3 { get => additionalAddressLine2_3; set => additionalAddressLine2_3 = value; }
        public String AdditionalAddressCity_3 { get => additionalAddressCity_3; set => additionalAddressCity_3 = value; }
        public String AdditionalAddressState_3 { get => additionalAddressState_3; set => additionalAddressState_3 = value; }
        public String AdditionalAddressZip_3 { get => additionalAddressZip_3; set => additionalAddressZip_3 = value; }
        public String AdditionalAddressCounty_3 { get => additionalAddressCounty_3; set => additionalAddressCounty_3 = value; }
        public String AdditionalAddressPhoneNumber_3 { get => additionalAddressPhoneNumber_3; set => additionalAddressPhoneNumber_3 = value; }
        public String AdditionalAddressFaxNumber_3 { get => additionalAddressFaxNumber_3; set => additionalAddressFaxNumber_3 = value; }
        public String AdditionalAddressEmailAddress_3 { get => additionalAddressEmailAddress_3; set => additionalAddressEmailAddress_3 = value; }
        public String AdditionalAddressCountry_3 { get => additionalAddressCountry_3; set => additionalAddressCountry_3 = value; }
        public String AdditionalAddressName_4 { get => additionalAddressName_4; set => additionalAddressName_4 = value; }
        public String AdditionalAddressLine1_4 { get => additionalAddressLine1_4; set => additionalAddressLine1_4 = value; }
        public String AdditionalAddressLine2_4 { get => additionalAddressLine2_4; set => additionalAddressLine2_4 = value; }
        public String AdditionalAddressCity_4 { get => additionalAddressCity_4; set => additionalAddressCity_4 = value; }
        public String AdditionalAddressState_4 { get => additionalAddressState_4; set => additionalAddressState_4 = value; }
        public String AdditionalAddressZip_4 { get => additionalAddressZip_4; set => additionalAddressZip_4 = value; }
        public String AdditionalAddressCounty_4 { get => additionalAddressCounty_4; set => additionalAddressCounty_4 = value; }
        public String AdditionalAddressPhoneNumber_4 { get => additionalAddressPhoneNumber_4; set => additionalAddressPhoneNumber_4 = value; }
        public String AdditionalAddressFaxNumber_4 { get => additionalAddressFaxNumber_4; set => additionalAddressFaxNumber_4 = value; }
        public String AdditionalAddressEmailAddress_4 { get => additionalAddressEmailAddress_4; set => additionalAddressEmailAddress_4 = value; }
        public String AdditionalAddressCountry_4 { get => additionalAddressCountry_4; set => additionalAddressCountry_4 = value; }
        public String AdditionalAddressName_5 { get => additionalAddressName_5; set => additionalAddressName_5 = value; }
        public String AdditionalAddressLine1_5 { get => additionalAddressLine1_5; set => additionalAddressLine1_5 = value; }
        public String AdditionalAddressLine2_5 { get => additionalAddressLine2_5; set => additionalAddressLine2_5 = value; }
        public String AdditionalAddressCity_5 { get => additionalAddressCity_5; set => additionalAddressCity_5 = value; }
        public String AdditionalAddressState_5 { get => additionalAddressState_5; set => additionalAddressState_5 = value; }
        public String AdditionalAddressZip_5 { get => additionalAddressZip_5; set => additionalAddressZip_5 = value; }
        public String AdditionalAddressCounty_5 { get => additionalAddressCounty_5; set => additionalAddressCounty_5 = value; }
        public String AdditionalAddressPhoneNumber_5 { get => additionalAddressPhoneNumber_5; set => additionalAddressPhoneNumber_5 = value; }
        public String AdditionalAddressFaxNumber_5 { get => additionalAddressFaxNumber_5; set => additionalAddressFaxNumber_5 = value; }
        public String AdditionalAddressEmailAddress_5 { get => additionalAddressEmailAddress_5; set => additionalAddressEmailAddress_5 = value; }
        public String AdditionalAddressCountry_5 { get => additionalAddressCountry_5; set => additionalAddressCountry_5 = value; }
      

    }
}
