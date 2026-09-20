using Corp.Core.Libraries.ProviderManagementReference;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Corp.Core.Libraries
{
    public class ProviderManagementController
    {
        public const string Yes = "Y";
        public const string No = "N";

        public static UpdateProviderInformationProviderDemographics fillUpdateProviderDemographic(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.UpdateProviderInformationProviderDemographics enEnrollProviderDemographics = new ProviderManagementReference.UpdateProviderInformationProviderDemographics();
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderDemographics.RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderDemographics.ProviderId = ProviderManagementHelper.GetString("ProviderId", dr);
                enEnrollProviderDemographics.TaxId = ProviderManagementHelper.GetString("TaxId", dr);
                enEnrollProviderDemographics.ProviderRoleCode = ProviderManagementHelper.GetString("ProviderRoleCode", dr);
                enEnrollProviderDemographics.NationalProviderIdentifier = ProviderManagementHelper.GetString("NationalProviderIdentifier", dr);
                enEnrollProviderDemographics.FirstName = ProviderManagementHelper.GetString("FirstName", dr);
                enEnrollProviderDemographics.MiddleName = ProviderManagementHelper.GetString("MiddleName", dr);
                enEnrollProviderDemographics.LastName = ProviderManagementHelper.GetString("LastName", dr);
                enEnrollProviderDemographics.Prefix = ProviderManagementHelper.GetString("Prefix", dr);
                enEnrollProviderDemographics.Suffix = ProviderManagementHelper.GetString("Suffix", dr);
                enEnrollProviderDemographics.SSN = ProviderManagementHelper.GetString("SSN", dr);
                enEnrollProviderDemographics.Gender = ProviderManagementHelper.GetString("Gender", dr);
                enEnrollProviderDemographics.EntityType = ProviderManagementHelper.GetString("EntityType", dr);
                enEnrollProviderDemographics.OrganizationBusinessName = ProviderManagementHelper.GetString("ProviderName", dr);
                enEnrollProviderDemographics.LegalProviderName = ProviderManagementHelper.GetString("LegalProviderName", dr);
                enEnrollProviderDemographics.ProviderBusinessName = ProviderManagementHelper.GetString("ProviderBusinessName", dr);
                enEnrollProviderDemographics.ProviderTaxName = ProviderManagementHelper.GetString("ProviderTaxName", dr);
                enEnrollProviderDemographics.TeachingIndicator = ProviderManagementHelper.GetString("TeachingIndicator", dr);
                enEnrollProviderDemographics.OwnershipCode = ProviderManagementHelper.GetString("OwnershipCode", dr);
                enEnrollProviderDemographics.PracticeTypeCode = ProviderManagementHelper.GetString("PracticeTypeCode", dr);
                enEnrollProviderDemographics.ProviderProfitStatusCode = ProviderManagementHelper.GetString("ProviderProfitStatusCode", dr);
                enEnrollProviderDemographics.NewPatientIndicator = ProviderManagementHelper.GetString("NewPatientIndicator", dr);
                enEnrollProviderDemographics.EnrollmentType = ProviderManagementHelper.GetString("EnrollmentType", dr);
                enEnrollProviderDemographics.EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderDemographics.EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);

                if (!ProviderManagementHelper.GetStringDateTime("DateOfBirth", dr).Equals("1900-01-01T00:00:00"))
                {
                    enEnrollProviderDemographics.DateOfBirth = ProviderManagementHelper.GetStringDateTime("DateOfBirth", dr);
                }
                else
                {
                    enEnrollProviderDemographics.DateOfBirth = string.Empty;
                }
                enEnrollProviderDemographics.DateOfDeath = ProviderManagementHelper.GetStringDateTime("DateOfDeath", dr);
                enEnrollProviderDemographics.Race = ProviderManagementHelper.GetString("Race", dr);
                enEnrollProviderDemographics.MedicareInd = ProviderManagementHelper.GetString("MedicareInd", dr);
                enEnrollProviderDemographics.PCPStatusCode = ProviderManagementHelper.GetString("PCPStatusCode", dr);
                enEnrollProviderDemographics.PCPTermDate = ProviderManagementHelper.GetStringDateTime("PCPTermDate", dr);
                enEnrollProviderDemographics.BirthCountry = ProviderManagementHelper.GetString("BirthCountry", dr);
                enEnrollProviderDemographics.BirthCity = ProviderManagementHelper.GetString("BirthCity", dr);
                enEnrollProviderDemographics.BirthState = ProviderManagementHelper.GetString("BirthState", dr);
                enEnrollProviderDemographics.ProvRiskLevel = ProviderManagementHelper.GetString("ProvRiskLevel", dr);
                enEnrollProviderDemographics.ProvDirectorySearchIndicator = ProviderManagementHelper.GetString("ProvDirectorySearchIndicator", dr);
                enEnrollProviderDemographics.PaymentTypeCode = ProviderManagementHelper.GetString("PaymentTypeCode", dr);
                enEnrollProviderDemographics.ProvIHSAssoCode = ProviderManagementHelper.GetString("ProvIHSAssoCode", dr);
                enEnrollProviderDemographics.ProvDemographicsSrcKey = ProviderManagementHelper.GetString("ProvDemographicsSrcKey", dr);
            }
            return enEnrollProviderDemographics;
        }

        public static ProviderAddressListAddress[] fillUpdateProviderAddress(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderAddressListAddress[] enEnrollProviderInformationAddress = new ProviderManagementReference.ProviderAddressListAddress[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderInformationAddress[i] = new ProviderManagementReference.ProviderAddressListAddress();
                enEnrollProviderInformationAddress[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderInformationAddress[i].AddressType = ProviderManagementHelper.GetString("AddressType", dr);
                enEnrollProviderInformationAddress[i].AddressLine1 = ProviderManagementHelper.GetString("AddressLine1", dr);
                enEnrollProviderInformationAddress[i].AddressLine2 = ProviderManagementHelper.GetString("AddressLine2", dr);
                enEnrollProviderInformationAddress[i].City = ProviderManagementHelper.GetString("City", dr);
                enEnrollProviderInformationAddress[i].State = ProviderManagementHelper.GetString("State", dr);
                enEnrollProviderInformationAddress[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enEnrollProviderInformationAddress[i].ZipCode5 = ProviderManagementHelper.GetString("ZipCode5", dr);
                enEnrollProviderInformationAddress[i].ZipCode4 = ProviderManagementHelper.GetString("ZipCode4", dr);
                enEnrollProviderInformationAddress[i].Country = ProviderManagementHelper.GetString("Country", dr);
                enEnrollProviderInformationAddress[i].BorderStateIndicator = ProviderManagementHelper.GetString("BorderStateIndicator", dr);
                enEnrollProviderInformationAddress[i].AddressNameTypeIndicator = ProviderManagementHelper.GetString("AddressNameTypeIndicator", dr);
                enEnrollProviderInformationAddress[i].ProvOrgAddressName = ProviderManagementHelper.GetString("ProvOrgAddressName", dr); 
                enEnrollProviderInformationAddress[i].ProvOrgAddressFirstName = ProviderManagementHelper.GetString("ProvOrgAddressFirstName", dr); 
                enEnrollProviderInformationAddress[i].ProvOrgAddressMiddleName = ProviderManagementHelper.GetString("ProvOrgAddressMiddleName", dr); 
                enEnrollProviderInformationAddress[i].ProvOrgAddressLastName = ProviderManagementHelper.GetString("ProvOrgAddressLastName", dr); 
                enEnrollProviderInformationAddress[i].ProvOrgPhoneNumber1 = ProviderManagementHelper.GetString("ProvOrgPhoneNumber1", dr);
                enEnrollProviderInformationAddress[i].ProvOrgPhoneNumberExt1 = ProviderManagementHelper.GetString("ProvOrgPhoneNumberExt1", dr);
                enEnrollProviderInformationAddress[i].ProvOrgPhoneNumber2 = ProviderManagementHelper.GetString("ProvOrgPhoneNumber2", dr);
                enEnrollProviderInformationAddress[i].ProvOrgPhoneNumberExt2 = ProviderManagementHelper.GetString("ProvOrgPhoneNumberExt2", dr);
                enEnrollProviderInformationAddress[i].ProvOrgFaxNumber1 = ProviderManagementHelper.GetString("ProvOrgFaxNumber1", dr);
                enEnrollProviderInformationAddress[i].ProvOrgFaxNumber2 = ProviderManagementHelper.GetString("ProvOrgFaxNumber2", dr);
                enEnrollProviderInformationAddress[i].ProvAddressContactName = ProviderManagementHelper.GetString("ProvAddressContactName", dr);
                enEnrollProviderInformationAddress[i].ProvEmail = ProviderManagementHelper.GetString("ProvEmail", dr);
                enEnrollProviderInformationAddress[i].Longitude = ProviderManagementHelper.GetString("ProvAddressLongitude", dr);
                enEnrollProviderInformationAddress[i].Latitude = ProviderManagementHelper.GetString("ProvAddressLatitude", dr);
                enEnrollProviderInformationAddress[i].HandicapAccessibilityIndicator = ProviderManagementHelper.GetString("HandicapAccessibilityIndicator", dr);
                enEnrollProviderInformationAddress[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderInformationAddress[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enEnrollProviderInformationAddress[i].ProvAddressSrcKey = ProviderManagementHelper.GetString("ProvAddressSrcKey", dr);
            }
            return enEnrollProviderInformationAddress;
        }

        public static updateProviderApplication[] fillUpdateProviderApplication(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.updateProviderApplication[] enenrollProviderApplication = new ProviderManagementReference.updateProviderApplication[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enenrollProviderApplication[i] = new ProviderManagementReference.updateProviderApplication();
                enenrollProviderApplication[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enenrollProviderApplication[i].ApplicationId = ProviderManagementHelper.GetString("ApplicationId", dr);
                enenrollProviderApplication[i].AdditionalUserID = ProviderManagementHelper.GetString("AdditionalApplicationId", dr);
                enenrollProviderApplication[i].ApplicationCreatedDate = ProviderManagementHelper.GetStringDateTime("ApplicationCreatedDate", dr);
                enenrollProviderApplication[i].ApprovalDate = ProviderManagementHelper.GetStringDateTime("ApprovalDate", dr);
                enenrollProviderApplication[i].StatePlanEnrollCode = ProviderManagementHelper.GetString("StatePlanEnrollCode", dr);
                enenrollProviderApplication[i].EnrollMethodCode = ProviderManagementHelper.GetString("EnrollMethodCode", dr);
                enenrollProviderApplication[i].ProvEnrollStatusCode = ProviderManagementHelper.GetString("ProvEnrollStatusCode", dr);
                enenrollProviderApplication[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enenrollProviderApplication[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enenrollProviderApplication[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enenrollProviderApplication[i].NonMedicaidApplicationProviderType = ProviderManagementHelper.GetString("Non_MedicaidApplicationProviderType", dr);
                enenrollProviderApplication[i].AdditionalApplicationId = ProviderManagementHelper.GetString("AdditionalApplicationId", dr);
                enenrollProviderApplication[i].ApplicationType = ProviderManagementHelper.GetString("ApplicationType", dr);
                enenrollProviderApplication[i].ApplicationStatus = ProviderManagementHelper.GetString("ApplicationStatus", dr);
                enenrollProviderApplication[i].ApplicationLegalStatus = ProviderManagementHelper.GetString("ApplicationLegalStatus", dr);
                enenrollProviderApplication[i].ApplicationFeeConfirmation = ProviderManagementHelper.GetString("ApplicationFeeConfirmation", dr);
                enenrollProviderApplication[i].SingleSignOnAssignedID = ProviderManagementHelper.GetString("SingleSignOnAssignedID", dr);
                enenrollProviderApplication[i].SingleSignOnSelectedID = ProviderManagementHelper.GetString("SingleSignOnSelectedID", dr);
                enenrollProviderApplication[i].AdditionalUserID = ProviderManagementHelper.GetString("AdditionalUserID", dr);
                enenrollProviderApplication[i].SingleSignOnRole = ProviderManagementHelper.GetString("SingleSignOnRole", dr);
                enenrollProviderApplication[i].EmailAddress = ProviderManagementHelper.GetString("EmailAddress", dr);
                enenrollProviderApplication[i].ProvApplicationSrcKey = ProviderManagementHelper.GetString("ProvApplicationSrcKey", dr);
            }
            return enenrollProviderApplication;
        }

        public static UpdateProviderOwnershipList[] fillUpdateProviderOwnershipList(DataTable dt)
        {

            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.UpdateProviderOwnershipList[] enProviderOwnershipList = new ProviderManagementReference.UpdateProviderOwnershipList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderOwnershipList[i] = new ProviderManagementReference.UpdateProviderOwnershipList();
                enProviderOwnershipList[i].OwnerAddress = fillUpdateProviderOwnershipListAddress(dr);
                enProviderOwnershipList[i].OwnershipDetails = fillUpdateProviderOwnershipListOwnershipDetails(dr);
            }
            return enProviderOwnershipList;
        }

        public static UpdateProviderOwnershipListAddress[] fillUpdateProviderOwnershipListAddress(DataRow dr)
        {
            int countRow = 1;
            if (string.IsNullOrEmpty(ProviderManagementHelper.GetString("AddressLine1", dr).Trim()))
            {
                return null;
            }
            else
            {
                ProviderManagementReference.UpdateProviderOwnershipListAddress[] enProviderOwnershipListAddress = new ProviderManagementReference.UpdateProviderOwnershipListAddress[countRow];
                for (int i = 0; i < countRow; i++)
                {
                    enProviderOwnershipListAddress[i] = new ProviderManagementReference.UpdateProviderOwnershipListAddress();
                    enProviderOwnershipListAddress[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                    enProviderOwnershipListAddress[i].AddressType = ProviderManagementHelper.GetString("AddressType", dr);
                    enProviderOwnershipListAddress[i].AddressLine1 = ProviderManagementHelper.GetString("AddressLine1", dr);
                    enProviderOwnershipListAddress[i].AddressLine2 = ProviderManagementHelper.GetString("AddressLine2", dr);
                    enProviderOwnershipListAddress[i].City = ProviderManagementHelper.GetString("City", dr);
                    enProviderOwnershipListAddress[i].State = ProviderManagementHelper.GetString("State", dr);
                    enProviderOwnershipListAddress[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                    enProviderOwnershipListAddress[i].ZipCode4 = ProviderManagementHelper.GetString("ZipCode4", dr);
                    enProviderOwnershipListAddress[i].ZipCode5 = ProviderManagementHelper.GetString("ZipCode5", dr);
                    enProviderOwnershipListAddress[i].Country = ProviderManagementHelper.GetString("Country", dr);
                    enProviderOwnershipListAddress[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("AddrEffectiveDate", dr);
                    enProviderOwnershipListAddress[i].EndDate = ProviderManagementHelper.GetStringDateTime("AddrEndDate", dr);
                    //enProviderOwnershipListAddress[i].ProvOwnrAdrsSrcKey = DatatableHelper.GetString("ProvOwnrAdrsSrcKey", dr);
                }
                return enProviderOwnershipListAddress;
            }

        }

        public static UpdateProviderOwnershipListOwnershipDetails fillUpdateProviderOwnershipListOwnershipDetails(DataRow dr)
        {
            int countRow = 1;
            ProviderManagementReference.UpdateProviderOwnershipListOwnershipDetails enProviderOwnershipListOwnershipDetails = new ProviderManagementReference.UpdateProviderOwnershipListOwnershipDetails();
            for (int i = 0; i < countRow; i++)
            {
                enProviderOwnershipListOwnershipDetails.OwnerType = ProviderManagementHelper.GetString("OwnerType", dr);
                enProviderOwnershipListOwnershipDetails.OwnerAffiliationType = ProviderManagementHelper.GetString("OwnerAffiliationType", dr);
                enProviderOwnershipListOwnershipDetails.OwnPercentage = ProviderManagementHelper.GetDecimalString("OwnPercentage", dr);
                enProviderOwnershipListOwnershipDetails.SanctionInd = ProviderManagementHelper.GetString("SanctionInd", dr);
                enProviderOwnershipListOwnershipDetails.SanctionExplain = ProviderManagementHelper.GetString("SanctionExplain", dr);
                enProviderOwnershipListOwnershipDetails.RelationshipInd = ProviderManagementHelper.GetString("RelationshipInd", dr);
                enProviderOwnershipListOwnershipDetails.CriminalOffenseInd = ProviderManagementHelper.GetString("CriminalOffenseInd", dr);
                enProviderOwnershipListOwnershipDetails.CriminalOffenseExplain = ProviderManagementHelper.GetString("CriminalOffenseExplain", dr);

                enProviderOwnershipListOwnershipDetails.LicenseSuspendInd = ProviderManagementHelper.GetString("LicenseSuspendInd", dr);
                enProviderOwnershipListOwnershipDetails.LicenseSuspendExplain = ProviderManagementHelper.GetString("LicenseSuspendExplain", dr);
                enProviderOwnershipListOwnershipDetails.PenaltyInd = ProviderManagementHelper.GetString("PenaltyInd", dr);
                enProviderOwnershipListOwnershipDetails.PenaltyExplain = ProviderManagementHelper.GetString("PenaltyExplain", dr);
                enProviderOwnershipListOwnershipDetails.CntrlIntrstOthrProvEntityInd = ProviderManagementHelper.GetString("CntrlIntrstOthrProvEntityInd", dr);
                enProviderOwnershipListOwnershipDetails.CntrlIntrstOthrProvEntityExplain = ProviderManagementHelper.GetString("CntrlIntrstOthrProvEntityExplain", dr);
                enProviderOwnershipListOwnershipDetails.OwnrSubcntrctrTrnxInd = ProviderManagementHelper.GetString("OwnrSubcntrctrTrnxInd", dr);
                enProviderOwnershipListOwnershipDetails.OwnrSubcntrctrDetails = ProviderManagementHelper.GetString("OwnrSubcntrctrDetails", dr);
                enProviderOwnershipListOwnershipDetails.OwnrSubcntrctrBsnssTrnxInd = ProviderManagementHelper.GetString("OwnrSubcntrctrBsnssTrnxInd", dr);
                enProviderOwnershipListOwnershipDetails.OwnrSubcntrctrBsnssTrnxDetails = ProviderManagementHelper.GetString("OwnrSubcntrctrBsnssTrnxDetails", dr);
                enProviderOwnershipListOwnershipDetails.TransactionAmount = ProviderManagementHelper.GetString("TransactionAmount", dr);
                enProviderOwnershipListOwnershipDetails.TransactionDate = ProviderManagementHelper.GetStringDateTime("TransactionDate", dr);
                enProviderOwnershipListOwnershipDetails.OwnerOutOfStateResidentIndicator = ProviderManagementHelper.GetString("OwnerOutOfStateResidentIndicator", dr);
                enProviderOwnershipListOwnershipDetails.OwnerOutOfStateResidentExplain = ProviderManagementHelper.GetString("OwnerOutOfStateResidentExplain", dr);
                enProviderOwnershipListOwnershipDetails.OwnerStateFederalOffenseInd = ProviderManagementHelper.GetString("OwnerStateFederalOffenseInd", dr);
                enProviderOwnershipListOwnershipDetails.OwnerStateFederalOffenseExplain = ProviderManagementHelper.GetString("OwnerStateFederalOffenseExplain", dr);

                enProviderOwnershipListOwnershipDetails.FingerprintStatusIndicator = ProviderManagementHelper.GetString("FingerprintStatusIndicator", dr);
                enProviderOwnershipListOwnershipDetails.RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderOwnershipListOwnershipDetails.EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderOwnershipListOwnershipDetails.EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);

                //OHPNM -14007
                if (ProviderManagementHelper.GetString("OwnerType", dr).Equals("I")
                    || ProviderManagementHelper.GetString("OwnerType", dr).Equals("RI")
                    || ProviderManagementHelper.GetString("OwnerType", dr).Equals("SI")
                    || ProviderManagementHelper.GetString("OwnerType", dr).Equals("SUI")
                    )
                {
                    ProviderManagementReference.OwnerOwnerIndividual enooIndiv = new ProviderManagementReference.OwnerOwnerIndividual();
                    enooIndiv.FirstName = ProviderManagementHelper.GetString("FirstName", dr);
                    enooIndiv.MiddleName = ProviderManagementHelper.GetString("MiddleName", dr);
                    enooIndiv.LastName = ProviderManagementHelper.GetString("LastName", dr);
                    enooIndiv.DateOfBirth = ProviderManagementHelper.GetStringDateTime("DateOfBirth", dr);
                    enooIndiv.SSN = ProviderManagementHelper.GetString("SSN", dr);
                    enooIndiv.Title = ProviderManagementHelper.GetString("Title", dr);
                    enooIndiv.OwnerIndvSrcKey = ProviderManagementHelper.GetString("OwnerIndvSrcKey", dr);
                    enProviderOwnershipListOwnershipDetails.Item = enooIndiv;
                }
                else
                {
                    if (dr != null)
                    {
                        ProviderManagementReference.OwnerOwnerOrganization enooOrg = new ProviderManagementReference.OwnerOwnerOrganization();
                        enooOrg.OwnerOrgName = ProviderManagementHelper.GetString("OwnerOrgName", dr);
                        enooOrg.OrgTaxIdNumber = ProviderManagementHelper.GetString("OrgTaxIdNumber", dr);
                        enooOrg.Title = ProviderManagementHelper.GetString("Title", dr);
                        enooOrg.OwnerOrgSrcKey = ProviderManagementHelper.GetString("OwnerOrgSrcKey", dr);
                        enProviderOwnershipListOwnershipDetails.Item = enooOrg;
                    }
                }

            }
            return enProviderOwnershipListOwnershipDetails;
        }

        public static ProviderServiceLocation[] fillUpdateProviderServiceLocation(DataSet ds)
        {
            DataTable dt = ds.Tables[17];
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderServiceLocation[] enEnrollProviderServiceLocation = new ProviderManagementReference.ProviderServiceLocation[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderServiceLocation[i] = new ProviderManagementReference.ProviderServiceLocation();
                enEnrollProviderServiceLocation[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderServiceLocation[i].LocationName = ProviderManagementHelper.GetString("LocationName", dr);
                enEnrollProviderServiceLocation[i].LocationCode = ProviderManagementHelper.GetString("LocationCode", dr);
                enEnrollProviderServiceLocation[i].OrgStateOwnedIndicator = ProviderManagementHelper.GetString("OrgStateOwnedIndicator", dr);
                enEnrollProviderServiceLocation[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderServiceLocation[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);

                if (!string.IsNullOrEmpty(ProviderManagementHelper.GetString("AddressType", dr)))
                {
                    enEnrollProviderServiceLocation[i].ServiceLocationAddress = fillUpdateProviderServiceLocationAddress(dr);
                }
                else
                {
                    enEnrollProviderServiceLocation[i].ServiceLocationAddress = fillUpdateProviderServiceLocationAddress(null);
                }
                if (i == 0)
                {


                    enEnrollProviderServiceLocation[i].ServiceLocationContact = fillUpdateProviderServiceLocationContact(null);

                    enEnrollProviderServiceLocation[i].ProviderLicense = fillUpdateProviderServiceLocationLicense(ds);
                    enEnrollProviderServiceLocation[i].ProviderCertification = fillProviderCertificationListCertification(ds.Tables[22]);
                    enEnrollProviderServiceLocation[i].ProviderIdentifier = fillProviderIdentifierListIdentifier(ds.Tables[24]);
                    enEnrollProviderServiceLocation[i].ProviderFacility = fillProviderFacilityListFacility(ds.Tables[25]);
                    enEnrollProviderServiceLocation[i].ProviderTaxonomy = fillProviderTaxonomyListTaxonomy(ds.Tables[26]);
                    enEnrollProviderServiceLocation[i].ProviderSpecialty = fillProviderSpecialtyListSpecialty(ds.Tables[27]);
                    // enEnrollProviderServiceLocation[i].ProviderType = fillProviderType(null);
                    enEnrollProviderServiceLocation[i].ProviderRestriction = fillUpdateProviderRestrictionListRestriction(ds.Tables[28]);

                    enEnrollProviderServiceLocation[i].SiteVisits = fillVisitListVisit(ds.Tables[29]);
                    enEnrollProviderServiceLocation[i].ProviderTraining = fillProviderTrainingListTraining(ds.Tables[30]);
                }
                enEnrollProviderServiceLocation[i].ProvSvcLocationSrcKey = ProviderManagementHelper.GetString("ProvSvcLocationSrcKey", dr);
            }
            return enEnrollProviderServiceLocation;
        }

        public static ProviderRestrictionListRestriction[] fillUpdateProviderRestrictionListRestriction(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderRestrictionListRestriction[] enEnrollProviderRestrictionListRestriction = new ProviderManagementReference.ProviderRestrictionListRestriction[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderRestrictionListRestriction[i] = new ProviderManagementReference.ProviderRestrictionListRestriction();
                enEnrollProviderRestrictionListRestriction[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderRestrictionListRestriction[i].ReviewReason = ProviderManagementHelper.GetString("ReviewReason", dr);
                enEnrollProviderRestrictionListRestriction[i].ReviewType = ProviderManagementHelper.GetString("ReviewType", dr);
                enEnrollProviderRestrictionListRestriction[i].RestrictionClass = ProviderManagementHelper.GetString("RestrictionClass", dr);
                enEnrollProviderRestrictionListRestriction[i].ClaimType = ProviderManagementHelper.GetString("ClaimType", dr);
                enEnrollProviderRestrictionListRestriction[i].IncludeExcludeIndicator = ProviderManagementHelper.GetString("IncludeExcludeIndicator", dr);

                enEnrollProviderRestrictionListRestriction[i].LowCode = ProviderManagementHelper.GetString("LowCode", dr);
                enEnrollProviderRestrictionListRestriction[i].HighCode = ProviderManagementHelper.GetString("HighCode", dr);
                enEnrollProviderRestrictionListRestriction[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderRestrictionListRestriction[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enEnrollProviderRestrictionListRestriction[i].ProvRestrictionSrcKey = ProviderManagementHelper.GetString("ProvRestrictionSrcKey", dr);
            }
            return enEnrollProviderRestrictionListRestriction;
        }

        public static ServiceLocationContactListContact[] fillUpdateProviderServiceLocationContact(DataTable dt)
        {
            int countRow = 0;
            if (dt != null)
            {
                countRow = dt.Rows.Count;
            }

            DataRow dr = null;
            ProviderManagementReference.ServiceLocationContactListContact[] enEnrollProviderServiceLocationContact = new ProviderManagementReference.ServiceLocationContactListContact[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderServiceLocationContact[i] = new ProviderManagementReference.ServiceLocationContactListContact();
                enEnrollProviderServiceLocationContact[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderServiceLocationContact[i].ContactType = ProviderManagementHelper.GetString("ContactType", dr);
                enEnrollProviderServiceLocationContact[i].ContactDetail = ProviderManagementHelper.GetString("ContactDetail", dr);
                enEnrollProviderServiceLocationContact[i].ContactInstruction = ProviderManagementHelper.GetString("ContactInstruction", dr);
                enEnrollProviderServiceLocationContact[i].PreferredContactIndicator = ProviderManagementHelper.GetString("PreferredContactIndicator", dr);
                enEnrollProviderServiceLocationContact[i].SvcLctnCntctSrcKey = ProviderManagementHelper.GetString("SvcLctnCntctSrcKey", dr);
            }
            return enEnrollProviderServiceLocationContact;
        }
        public static ProviderLicenseListLicense[] fillUpdateProviderServiceLocationLicense(DataSet ds)
        {
            DataTable dt = ds.Tables[23];
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            int StgProviderLicenseId = 0;

            ProviderManagementReference.ProviderLicenseListLicense[] enEnrollProviderServiceLocationLicense = new ProviderManagementReference.ProviderLicenseListLicense[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                StgProviderLicenseId = ProviderManagementHelper.GetInt("StgProviderLicenseId", dr);
                enEnrollProviderServiceLocationLicense[i] = new ProviderManagementReference.ProviderLicenseListLicense();
                enEnrollProviderServiceLocationLicense[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderServiceLocationLicense[i].LicenseType = ProviderManagementHelper.GetString("LicenseType", dr);
                enEnrollProviderServiceLocationLicense[i].LicenseNumber = ProviderManagementHelper.GetString("LicenseNumber", dr);
                enEnrollProviderServiceLocationLicense[i].LicenseeStateCode = ProviderManagementHelper.GetString("LicenseeStateCode", dr);
                enEnrollProviderServiceLocationLicense[i].LicenseIssueBoardNotes = ProviderManagementHelper.GetString("LicenseIssueBoardNotes", dr);
                enEnrollProviderServiceLocationLicense[i].Status = ProviderManagementHelper.GetString("Status", dr);
                enEnrollProviderServiceLocationLicense[i].StatusReason = ProviderManagementHelper.GetString("StatusReason", dr);
                enEnrollProviderServiceLocationLicense[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enEnrollProviderServiceLocationLicense[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderServiceLocationLicense[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enEnrollProviderServiceLocationLicense[i].ProvLicenseSrcKey = ProviderManagementHelper.GetString("ProvLicenseSrcKey", dr);
                if (!(ProviderManagementHelper.GetString("LicenseType", dr).Equals("DEA")))
                {
                    enEnrollProviderServiceLocationLicense[i].LicenseCertifications = fillLicenseCertificationsListCertifications(ds, StgProviderLicenseId);
                }
                else
                {
                    enEnrollProviderServiceLocationLicense[i].LicenseCertifications = fillLicenseCertificationsListCertifications(null, StgProviderLicenseId);
                }
            }
            return enEnrollProviderServiceLocationLicense;
        }


        public static EnrollProviderDemographics fillProviderDemographic(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.EnrollProviderDemographics enEnrollProviderDemographics = new ProviderManagementReference.EnrollProviderDemographics();
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderDemographics.RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderDemographics.ProviderId = ProviderManagementHelper.GetString("ProviderId", dr);
                enEnrollProviderDemographics.TaxId = ProviderManagementHelper.GetString("TaxId", dr);
                enEnrollProviderDemographics.ProviderRoleCode = ProviderManagementHelper.GetString("ProviderRoleCode", dr);
                enEnrollProviderDemographics.NationalProviderIdentifier = ProviderManagementHelper.GetString("NationalProviderIdentifier", dr);
                enEnrollProviderDemographics.FirstName = ProviderManagementHelper.GetString("FirstName", dr);
                enEnrollProviderDemographics.MiddleName = ProviderManagementHelper.GetString("MiddleName", dr);
                enEnrollProviderDemographics.LastName = ProviderManagementHelper.GetString("LastName", dr);
                enEnrollProviderDemographics.Prefix = ProviderManagementHelper.GetString("Prefix", dr);
                enEnrollProviderDemographics.Suffix = ProviderManagementHelper.GetString("Suffix", dr);
                enEnrollProviderDemographics.SSN = ProviderManagementHelper.GetString("SSN", dr);
                enEnrollProviderDemographics.Gender = ProviderManagementHelper.GetString("Gender", dr);
                enEnrollProviderDemographics.EntityType = ProviderManagementHelper.GetString("EntityType", dr);
                enEnrollProviderDemographics.OrganizationBusinessName = ProviderManagementHelper.GetString("ProviderName", dr);
                enEnrollProviderDemographics.LegalProviderName = ProviderManagementHelper.GetString("LegalProviderName", dr);
                enEnrollProviderDemographics.ProviderBusinessName = ProviderManagementHelper.GetString("ProviderBusinessName", dr);
                enEnrollProviderDemographics.ProviderTaxName = ProviderManagementHelper.GetString("ProviderTaxName", dr);
                enEnrollProviderDemographics.TeachingIndicator = ProviderManagementHelper.GetString("TeachingIndicator", dr);
                enEnrollProviderDemographics.OwnershipCode = ProviderManagementHelper.GetString("OwnershipCode", dr);
                enEnrollProviderDemographics.PracticeTypeCode = ProviderManagementHelper.GetString("PracticeTypeCode", dr);
                enEnrollProviderDemographics.ProviderProfitStatusCode = ProviderManagementHelper.GetString("ProviderProfitStatusCode", dr);
                enEnrollProviderDemographics.NewPatientIndicator = ProviderManagementHelper.GetString("NewPatientIndicator", dr);
                enEnrollProviderDemographics.EnrollmentType = ProviderManagementHelper.GetString("EnrollmentType", dr);
                enEnrollProviderDemographics.EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderDemographics.EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);

                if (!ProviderManagementHelper.GetStringDateTime("DateOfBirth", dr).Equals("1900-01-01T00:00:00"))
                {
                    enEnrollProviderDemographics.DateOfBirth = ProviderManagementHelper.GetStringDateTime("DateOfBirth", dr);
                }
                else
                {
                    enEnrollProviderDemographics.DateOfBirth = string.Empty;
                }
                enEnrollProviderDemographics.DateOfDeath = ProviderManagementHelper.GetStringDateTime("DateOfDeath", dr);
                enEnrollProviderDemographics.Race = ProviderManagementHelper.GetString("Race", dr);
                enEnrollProviderDemographics.MedicareInd = ProviderManagementHelper.GetString("MedicareInd", dr);
                enEnrollProviderDemographics.PCPStatusCode = ProviderManagementHelper.GetString("PCPStatusCode", dr);
                enEnrollProviderDemographics.PCPTermDate = ProviderManagementHelper.GetStringDateTime("PCPTermDate", dr);
                enEnrollProviderDemographics.BirthCountry = ProviderManagementHelper.GetString("BirthCountry", dr);
                enEnrollProviderDemographics.BirthCity = ProviderManagementHelper.GetString("BirthCity", dr);
                enEnrollProviderDemographics.BirthState = ProviderManagementHelper.GetString("BirthState", dr);
                enEnrollProviderDemographics.ProvRiskLevel = ProviderManagementHelper.GetString("ProvRiskLevel", dr);
                enEnrollProviderDemographics.ProvDirectorySearchIndicator = ProviderManagementHelper.GetString("ProvDirectorySearchIndicator", dr);
                enEnrollProviderDemographics.PaymentTypeCode = ProviderManagementHelper.GetString("PaymentTypeCode", dr);
                enEnrollProviderDemographics.ProvIHSAssoCode = ProviderManagementHelper.GetString("ProvIHSAssoCode", dr);
                enEnrollProviderDemographics.ProvDemographicsSrcKey = ProviderManagementHelper.GetString("ProvDemographicsSrcKey", dr);
            }
            return enEnrollProviderDemographics;
        }

        public static EnrollProviderInformationAddress[] fillProviderAddress(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.EnrollProviderInformationAddress[] enEnrollProviderInformationAddress = new ProviderManagementReference.EnrollProviderInformationAddress[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderInformationAddress[i] = new ProviderManagementReference.EnrollProviderInformationAddress();
                enEnrollProviderInformationAddress[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderInformationAddress[i].AddressType = ProviderManagementHelper.GetString("AddressType", dr);
                enEnrollProviderInformationAddress[i].AddressLine1 = ProviderManagementHelper.GetString("AddressLine1", dr);
                enEnrollProviderInformationAddress[i].AddressLine2 = ProviderManagementHelper.GetString("AddressLine2", dr);
                enEnrollProviderInformationAddress[i].City = ProviderManagementHelper.GetString("City", dr);
                enEnrollProviderInformationAddress[i].State = ProviderManagementHelper.GetString("State", dr);
                enEnrollProviderInformationAddress[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enEnrollProviderInformationAddress[i].ZipCode5 = ProviderManagementHelper.GetString("ZipCode5", dr);
                enEnrollProviderInformationAddress[i].ZipCode4 = ProviderManagementHelper.GetString("ZipCode4", dr);
                enEnrollProviderInformationAddress[i].Country = ProviderManagementHelper.GetString("Country", dr);
                enEnrollProviderInformationAddress[i].BorderStateIndicator = ProviderManagementHelper.GetString("BorderStateIndicator", dr);
                enEnrollProviderInformationAddress[i].AddressNameTypeIndicator = ProviderManagementHelper.GetString("AddressNameTypeIndicator", dr);
                enEnrollProviderInformationAddress[i].ProvOrgAddressName = ProviderManagementHelper.GetString("ProvOrgAddressName", dr); 
                enEnrollProviderInformationAddress[i].ProvOrgAddressFirstName = ProviderManagementHelper.GetString("ProvOrgAddressFirstName", dr); 
                enEnrollProviderInformationAddress[i].ProvOrgAddressMiddleName = ProviderManagementHelper.GetString("ProvOrgAddressMiddleName", dr); 
                enEnrollProviderInformationAddress[i].ProvOrgAddressLastName = ProviderManagementHelper.GetString("ProvOrgAddressLastName", dr); 

                enEnrollProviderInformationAddress[i].ProvOrgPhoneNumber1 = ProviderManagementHelper.GetString("ProvOrgPhoneNumber1", dr);
                enEnrollProviderInformationAddress[i].ProvOrgPhoneNumberExt1 = ProviderManagementHelper.GetString("ProvOrgPhoneNumberExt1", dr);
                enEnrollProviderInformationAddress[i].ProvOrgPhoneNumber2 = ProviderManagementHelper.GetString("ProvOrgPhoneNumber2", dr);
                enEnrollProviderInformationAddress[i].ProvOrgPhoneNumberExt2 = ProviderManagementHelper.GetString("ProvOrgPhoneNumberExt2", dr);
                enEnrollProviderInformationAddress[i].ProvOrgFaxNumber1 = ProviderManagementHelper.GetString("ProvOrgFaxNumber1", dr);
                enEnrollProviderInformationAddress[i].ProvOrgFaxNumber2 = ProviderManagementHelper.GetString("ProvOrgFaxNumber2", dr);
                enEnrollProviderInformationAddress[i].ProvAddressContactName = ProviderManagementHelper.GetString("ProvAddressContactName", dr);
                enEnrollProviderInformationAddress[i].ProvEmail = ProviderManagementHelper.GetString("ProvEmail", dr);
                enEnrollProviderInformationAddress[i].Longitude = ProviderManagementHelper.GetString("ProvAddressLongitude", dr);
                enEnrollProviderInformationAddress[i].Latitude = ProviderManagementHelper.GetString("ProvAddressLatitude", dr);
                enEnrollProviderInformationAddress[i].HandicapAccessibilityIndicator = ProviderManagementHelper.GetString("HandicapAccessibilityIndicator", dr);
                enEnrollProviderInformationAddress[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderInformationAddress[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enEnrollProviderInformationAddress[i].ProvAddressSrcKey = ProviderManagementHelper.GetString("ProvAddressSrcKey", dr);
            }
            return enEnrollProviderInformationAddress;
        }

        public static ProviderTypeListType[] fillProviderType(DataTable dt)
        {
            int countRow = 0;
            if (dt != null)
            {
                countRow = dt.Rows.Count;
            }
            DataRow dr = null;
            ProviderManagementReference.ProviderTypeListType[] enProviderType = new ProviderManagementReference.ProviderTypeListType[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderType[i] = new ProviderManagementReference.ProviderTypeListType();
                enProviderType[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderType[i].ProviderTypeCode = ProviderManagementHelper.GetString("ProviderTypeCode", dr);
                enProviderType[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderType[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderType[i].ProvTypeSrcKey = ProviderManagementHelper.GetString("ProvTypeSrcKey", dr);
            }
            return enProviderType;
        }

        public static TaxonomyClassificationListTaxonomyClassification[] fillTaxonomyClassification(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.TaxonomyClassificationListTaxonomyClassification[] enTaxonomyClassifications = new ProviderManagementReference.TaxonomyClassificationListTaxonomyClassification[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enTaxonomyClassifications[i] = new ProviderManagementReference.TaxonomyClassificationListTaxonomyClassification();
                enTaxonomyClassifications[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enTaxonomyClassifications[i].ClassificationTypeCode = ProviderManagementHelper.GetString("ClassificationTypeCode", dr);
                enTaxonomyClassifications[i].ClassificationCode = ProviderManagementHelper.GetString("ClassificationCode", dr);
                enTaxonomyClassifications[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enTaxonomyClassifications[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enTaxonomyClassifications[i].ProvTxnmyClsfctnSrcKey = ProviderManagementHelper.GetString("ProvTxnmyClsfctnSrcKey", dr);
            }
            return enTaxonomyClassifications;
        }







        public static OwnerRelationshipListRelationship[] fillOwnerRelationshipLists(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.OwnerRelationshipListRelationship[] enOwnerRelationshipLists = new ProviderManagementReference.OwnerRelationshipListRelationship[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enOwnerRelationshipLists[i] = new ProviderManagementReference.OwnerRelationshipListRelationship();
                enOwnerRelationshipLists[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enOwnerRelationshipLists[i].Item = ProviderManagementHelper.GetString("Owner1SSN", dr);
                enOwnerRelationshipLists[i].ItemElementName = ItemChoiceType.Owner1SSN;
                enOwnerRelationshipLists[i].Item1 = ProviderManagementHelper.GetString("Owner2SSN", dr);
                enOwnerRelationshipLists[i].Item1ElementName = Item1ChoiceType.Owner2SSN;
                enOwnerRelationshipLists[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enOwnerRelationshipLists[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enOwnerRelationshipLists[i].RelationType = ProviderManagementHelper.GetString("RelationType", dr);
                enOwnerRelationshipLists[i].OwnrRltnshpSrcKey = ProviderManagementHelper.GetString("OwnrRltnshpSrcKey", dr);

            }
            return enOwnerRelationshipLists;
        }

        public static ProviderAffiliationListAffiliations[] fillAffiliationList(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderAffiliationListAffiliations[] enAffiliationList = new ProviderManagementReference.ProviderAffiliationListAffiliations[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enAffiliationList[i] = new ProviderManagementReference.ProviderAffiliationListAffiliations();
                enAffiliationList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enAffiliationList[i].AffiliatedProviderId = ProviderManagementHelper.GetString("ProviderId", dr);
                enAffiliationList[i].NPI = ProviderManagementHelper.GetString("NPI", dr);
                enAffiliationList[i].ProviderAffiliationTypeCode = ProviderManagementHelper.GetString("ProviderAffiliationTypeCode", dr);
                enAffiliationList[i].AdditionalInfo1 = ProviderManagementHelper.GetString("AdditionalInfo1", dr);
                enAffiliationList[i].AdditionalInfo2 = ProviderManagementHelper.GetString("AdditionalInfo2", dr);
                enAffiliationList[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enAffiliationList[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enAffiliationList[i].ProvAffiliationSrcKey = ProviderManagementHelper.GetString("ProvAffiliationSrcKey", dr);
            }
            return enAffiliationList;
        }

        public static ProviderAlternateIdListAlternateIdentifier[] fillAlternateIdList(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderAlternateIdListAlternateIdentifier[] enAlternateIdList = new ProviderManagementReference.ProviderAlternateIdListAlternateIdentifier[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enAlternateIdList[i] = new ProviderManagementReference.ProviderAlternateIdListAlternateIdentifier();
                enAlternateIdList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enAlternateIdList[i].AlternateIdType = ProviderManagementHelper.GetString("AlternateIdType", dr);
                enAlternateIdList[i].AlternateId = ProviderManagementHelper.GetString("AlternateId", dr);
                enAlternateIdList[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enAlternateIdList[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enAlternateIdList[i].ProvAlternateIdSrcKey = ProviderManagementHelper.GetString("ProvAlternateIdSrcKey", dr);
            }
            return enAlternateIdList;
        }

        public static EFTEnrollmentListEFTEnrollment[] fillEFTEnrollmentList(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.EFTEnrollmentListEFTEnrollment[] enEFTEnrollmentList = new ProviderManagementReference.EFTEnrollmentListEFTEnrollment[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEFTEnrollmentList[i] = new ProviderManagementReference.EFTEnrollmentListEFTEnrollment();
                enEFTEnrollmentList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEFTEnrollmentList[i].EFTEnrollmentId = ProviderManagementHelper.GetString("EFTEnrollmentId", dr);
                enEFTEnrollmentList[i].FinancialInstitutionAccountNumber = ProviderManagementHelper.GetString("FinancialInstitutionAccountNumber", dr);
                enEFTEnrollmentList[i].FinancialInstitutionAccountType = ProviderManagementHelper.GetString("FinancialInstitutionAccountType", dr);
                enEFTEnrollmentList[i].FinancialInstitutionName = ProviderManagementHelper.GetString("FinancialInstitutionName", dr);
                enEFTEnrollmentList[i].FinancialInstitutionRoutingNumber = ProviderManagementHelper.GetString("FinancialInstitutionRoutingNumber", dr);
                enEFTEnrollmentList[i].ProviderContactFirstName = ProviderManagementHelper.GetString("ProviderContactFirstName", dr);
                enEFTEnrollmentList[i].ProviderContactLastName = ProviderManagementHelper.GetString("ProviderContactLastName", dr);
                enEFTEnrollmentList[i].ProviderContactMiddleName = ProviderManagementHelper.GetString("ProviderContactMiddleName", dr);
                enEFTEnrollmentList[i].ProviderContactPhoneNumber = ProviderManagementHelper.GetString("ProviderContactPhoneNumber", dr);
                enEFTEnrollmentList[i].ProviderContactPhoneNumberExtension = ProviderManagementHelper.GetString("ProviderContactPhoneNumberExtension", dr);
                enEFTEnrollmentList[i].ProviderContactTitle = ProviderManagementHelper.GetString("ProviderContactTitle", dr);
                enEFTEnrollmentList[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEFTEnrollmentList[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enEFTEnrollmentList[i].ProvEFTEnrollmentSrcKey = ProviderManagementHelper.GetString("ProvEFTEnrollmentSrcKey", dr);
            }
            return enEFTEnrollmentList;
        }

        public static ProviderLanguageListLanguages[] fillLanguageList(DataTable dt,int transactionID = 0)
        {

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("TransactionQueueID", DbType.Int32, transactionID, false));

            DataSet dsresult = InfoAccess.ExecuteStoredProcedure("usp_Select_From_REG_LANGUAGE", parameters, "GetAlreadySelectedLanguage");
            DataTable dtresult = dsresult.Tables[0];
            int countRow = dtresult.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderLanguageListLanguages[] enLanguageList = new ProviderManagementReference.ProviderLanguageListLanguages[countRow];


            for (int i = 0; i < countRow; i++)
            {
                dr = dtresult.Rows[i];
                
                    enLanguageList[i] = new ProviderManagementReference.ProviderLanguageListLanguages();
                    enLanguageList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                    enLanguageList[i].Language = ProviderManagementHelper.GetString("LanguageCode", dr);
                    enLanguageList[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                    enLanguageList[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                    enLanguageList[i].Provlngsrccode = ProviderManagementHelper.GetString("Provlngsrccode", dr);
                
            }
            return enLanguageList;
        }

        public static enrollProviderApplication[] fillenrollProviderApplication(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.enrollProviderApplication[] enenrollProviderApplication = new ProviderManagementReference.enrollProviderApplication[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enenrollProviderApplication[i] = new ProviderManagementReference.enrollProviderApplication();
                enenrollProviderApplication[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enenrollProviderApplication[i].ApplicationId = ProviderManagementHelper.GetString("ApplicationId", dr);
                enenrollProviderApplication[i].AdditionalUserID = ProviderManagementHelper.GetString("AdditionalApplicationId", dr);
                enenrollProviderApplication[i].ApplicationCreatedDate = ProviderManagementHelper.GetStringDateTime("ApplicationCreatedDate", dr);
                enenrollProviderApplication[i].ApprovalDate = ProviderManagementHelper.GetStringDateTime("ApprovalDate", dr);
                enenrollProviderApplication[i].StatePlanEnrollCode = ProviderManagementHelper.GetString("StatePlanEnrollCode", dr);
                enenrollProviderApplication[i].EnrollMethodCode = ProviderManagementHelper.GetString("EnrollMethodCode", dr);
                enenrollProviderApplication[i].ProvEnrollStatusCode = ProviderManagementHelper.GetString("ProvEnrollStatusCode", dr);
                enenrollProviderApplication[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enenrollProviderApplication[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enenrollProviderApplication[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enenrollProviderApplication[i].NonMedicaidApplicationProviderType = ProviderManagementHelper.GetString("Non_MedicaidApplicationProviderType", dr);
                enenrollProviderApplication[i].AdditionalApplicationId = ProviderManagementHelper.GetString("AdditionalApplicationId", dr);
                enenrollProviderApplication[i].ApplicationType = ProviderManagementHelper.GetString("ApplicationType", dr);
                enenrollProviderApplication[i].ApplicationStatus = ProviderManagementHelper.GetString("ApplicationStatus", dr);
                enenrollProviderApplication[i].ApplicationLegalStatus = ProviderManagementHelper.GetString("ApplicationLegalStatus", dr);
                enenrollProviderApplication[i].ApplicationFeeConfirmation = ProviderManagementHelper.GetString("ApplicationFeeConfirmation", dr);
                enenrollProviderApplication[i].SingleSignOnAssignedID = ProviderManagementHelper.GetString("SingleSignOnAssignedID", dr);
                enenrollProviderApplication[i].SingleSignOnSelectedID = ProviderManagementHelper.GetString("SingleSignOnSelectedID", dr);
                enenrollProviderApplication[i].AdditionalUserID = ProviderManagementHelper.GetString("AdditionalUserID", dr);
                enenrollProviderApplication[i].SingleSignOnRole = ProviderManagementHelper.GetString("SingleSignOnRole", dr);
                enenrollProviderApplication[i].EmailAddress = ProviderManagementHelper.GetString("EmailAddress", dr);
                enenrollProviderApplication[i].ProvApplicationSrcKey = ProviderManagementHelper.GetString("ProvApplicationSrcKey", dr);
            }
            return enenrollProviderApplication;
        }

        public static ProviderAttestationsListAttestations[] fillProviderAttestationsListAttestations(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderAttestationsListAttestations[] enProviderAttestationsListAttestations = new ProviderManagementReference.ProviderAttestationsListAttestations[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderAttestationsListAttestations[i] = new ProviderManagementReference.ProviderAttestationsListAttestations();
                enProviderAttestationsListAttestations[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderAttestationsListAttestations[i].ProvAttestationTypeIndicator = ProviderManagementHelper.GetString("ProvAttestationTypeIndicator", dr);
                enProviderAttestationsListAttestations[i].ProvAttestationCode = ProviderManagementHelper.GetString("ProvAttestationCode", dr);
                enProviderAttestationsListAttestations[i].ProvResponseIndicator = ProviderManagementHelper.GetString("ProvResponseIndicator", dr);
                enProviderAttestationsListAttestations[i].ProvDateSigned = ProviderManagementHelper.GetStringDateTime("ProvDateSigned", dr);
                enProviderAttestationsListAttestations[i].Provattssrckey = ProviderManagementHelper.GetString("Provattssrckey", dr);
                enProviderAttestationsListAttestations[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
            }
            return enProviderAttestationsListAttestations;
        }
        public static ProviderBusinessStatusListBusinessStatus[] fillProviderBusinessStatusListBusinessStatus(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderBusinessStatusListBusinessStatus[] enProviderBusinessStatusListBusinessStatus = new ProviderManagementReference.ProviderBusinessStatusListBusinessStatus[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderBusinessStatusListBusinessStatus[i] = new ProviderManagementReference.ProviderBusinessStatusListBusinessStatus();
                enProviderBusinessStatusListBusinessStatus[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderBusinessStatusListBusinessStatus[i].BusinessStatusCode = ProviderManagementHelper.GetString("BusinessStatusCode", dr);
                enProviderBusinessStatusListBusinessStatus[i].EnrollmentBusinessStatusReasonCode = ProviderManagementHelper.GetString("EnrollmentBusinessStatusReasonCode", dr);
                enProviderBusinessStatusListBusinessStatus[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderBusinessStatusListBusinessStatus[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderBusinessStatusListBusinessStatus[i].ProvBsnsStatusSrcKey = ProviderManagementHelper.GetString("ProvBsnsStatusSrcKey", dr);
            }
            return enProviderBusinessStatusListBusinessStatus;
        }
        public static ProviderCHOPListCHOP[] fillProviderCHOPListCHOP(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderCHOPListCHOP[] enProviderCHOPListCHOP = new ProviderManagementReference.ProviderCHOPListCHOP[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderCHOPListCHOP[i] = new ProviderManagementReference.ProviderCHOPListCHOP();
                enProviderCHOPListCHOP[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderCHOPListCHOP[i].MedicaidProviderSellerId = ProviderManagementHelper.GetString("MedicaidProviderSellerId", dr);
                enProviderCHOPListCHOP[i].CHOPAmount = ProviderManagementHelper.GetString("CHOPAmount", dr);
                enProviderCHOPListCHOP[i].CHOPTypeCode = ProviderManagementHelper.GetString("CHOPTypeCode", dr);
                enProviderCHOPListCHOP[i].MasterLeaseAmount = ProviderManagementHelper.GetString("MasterLeaseAmount", dr);
                enProviderCHOPListCHOP[i].SubLeaseAmount = ProviderManagementHelper.GetString("SubLeaseAmount", dr);
                enProviderCHOPListCHOP[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderCHOPListCHOP[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderCHOPListCHOP[i].Provchopsrckey = ProviderManagementHelper.GetString("Provchopsrckey", dr);
            }
            return enProviderCHOPListCHOP;
        }
        public static ProviderContactListContact[] fillProviderContactListContact(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderContactListContact[] enProviderContactListContact = new ProviderManagementReference.ProviderContactListContact[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderContactListContact[i] = new ProviderManagementReference.ProviderContactListContact();
                enProviderContactListContact[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderContactListContact[i].ContactType = ProviderManagementHelper.GetString("ContactType", dr);
                enProviderContactListContact[i].ContactDetail = ProviderManagementHelper.GetString("ContactDetail", dr);
                enProviderContactListContact[i].ContactInstruction = ProviderManagementHelper.GetString("ContactInstruction", dr);
                enProviderContactListContact[i].PreferredContactIndicator = ProviderManagementHelper.GetString("PreferredContactIndicator", dr);
                enProviderContactListContact[i].ProvContactSrcKey = ProviderManagementHelper.GetString("ProvContactSrcKey", dr);
            }
            return enProviderContactListContact;
        }


        public static ProviderManagedEmployeesListProviderManagedEmployee[] fillProviderManagedEmployeesListManagedEmployee(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderManagedEmployeesListProviderManagedEmployee[] enProviderManagedEmployeesListManagedEmployee = new ProviderManagementReference.ProviderManagedEmployeesListProviderManagedEmployee[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderManagedEmployeesListManagedEmployee[i] = new ProviderManagementReference.ProviderManagedEmployeesListProviderManagedEmployee();
                enProviderManagedEmployeesListManagedEmployee[i].ManagedEmployee = fillProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee(dr);
                enProviderManagedEmployeesListManagedEmployee[i].ManagedEmployeeAddress = fillProviderManagedEmployeesListProviderManagedEmployeeAddress(dr);
            }
            return enProviderManagedEmployeesListManagedEmployee;
        }

        public static ProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee fillProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee(DataRow dr)
        {
            //int countRow = dt.Rows.Count;
            //DataRow dr = null;
            ProviderManagementReference.ProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee = new ProviderManagementReference.ProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee();
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee = new ProviderManagementReference.ProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee();
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.FirstName = ProviderManagementHelper.GetString("FirstName", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.MiddleName = ProviderManagementHelper.GetString("MiddleName", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.LastName = ProviderManagementHelper.GetString("LastName", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.ManageEmployeesType = ProviderManagementHelper.GetString("ManageEmployeesType", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.OwnerAffiliationType = ProviderManagementHelper.GetString("OwnerAffiliationType", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.ManageEmployeesInd = ProviderManagementHelper.GetString("ManageEmployeesInd", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.DateOfBirth = ProviderManagementHelper.GetStringDateTime("DateOfBirth", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.SSN = ProviderManagementHelper.GetString("SSN", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.SanctionInd = ProviderManagementHelper.GetString("SanctionInd", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.SanctionExplain = ProviderManagementHelper.GetString("SanctionExplain", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.RelationshipInd = ProviderManagementHelper.GetString("RelationshipInd", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.CriminalOffenseInd = ProviderManagementHelper.GetString("CriminalOffenseInd", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.CriminalOffenseExplain = ProviderManagementHelper.GetString("CriminalOffenseExplain", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.LicenseSuspendInd = ProviderManagementHelper.GetString("LicenseSuspendInd", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.LicenseSuspendExplain = ProviderManagementHelper.GetString("LicenseSuspendExplain", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.PenaltyInd = ProviderManagementHelper.GetString("PenaltyInd", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.PenaltyExplain = ProviderManagementHelper.GetString("PenaltyExplain", dr);

            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.CntrlIntrstOthrProvEntityInd = ProviderManagementHelper.GetString("CntrlIntrstOthrProvEntityInd", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.CntrlIntrstOthrProvEntityExplain = ProviderManagementHelper.GetString("CntrlIntrstOthrProvEntityExplain", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.OwnrSubcntrctrBsnssTrnxInd = ProviderManagementHelper.GetString("OwnrSubcntrctrBsnssTrnxInd", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.OwnrSubcntrctrBsnssTrnxDetails = ProviderManagementHelper.GetString("OwnrSubcntrctrBsnssTrnxDetails", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.TransactionAmount = ProviderManagementHelper.GetString("TransactionAmount", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.TransactionDate = ProviderManagementHelper.GetStringDateTime("TransactionDate", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.OwnerOutOfStateResidentIndicator = ProviderManagementHelper.GetString("OwnerOutOfStateResidentIndicator", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.OwnerOutOfStateResidentExplain = ProviderManagementHelper.GetString("OwnerOutOfStateResidentExplain", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.OwnerStateFederalOffenseInd = ProviderManagementHelper.GetString("OwnerStateFederalOffenseInd", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.OwnerStateFederalOffenseExplain = ProviderManagementHelper.GetString("OwnerStateFederalOffenseExplain", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.FingerprintStatusIndicator = ProviderManagementHelper.GetString("FingerprintStatusIndicator", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.ProvMngdEmplySrcKey = ProviderManagementHelper.GetString("ProvMngdEmplySrcKey", dr);

            return enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee;
        }

        public static ProviderManagedEmployeesListProviderManagedEmployeeAddress[] fillProviderManagedEmployeesListProviderManagedEmployeeAddress(DataRow dr)
        {
            int countRow = 1;
            if (string.IsNullOrEmpty(ProviderManagementHelper.GetString("AddressLine1", dr).Trim()))
            {
                dr = null;
                countRow = 0;
            }
            ProviderManagementReference.ProviderManagedEmployeesListProviderManagedEmployeeAddress[] enProviderManagedEmployeesListProviderManagedEmployeeAddress = new ProviderManagementReference.ProviderManagedEmployeesListProviderManagedEmployeeAddress[countRow];
            for (int i = 0; i < countRow; i++)
            {
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i] = new ProviderManagementReference.ProviderManagedEmployeesListProviderManagedEmployeeAddress();
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].AddressType = ProviderManagementHelper.GetString("AddressType", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].AddressLine1 = ProviderManagementHelper.GetString("AddressLine1", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].AddressLine2 = ProviderManagementHelper.GetString("AddressLine2", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].City = ProviderManagementHelper.GetString("City", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].State = ProviderManagementHelper.GetString("State", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].ZipCode5 = ProviderManagementHelper.GetString("ZipCode5", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].ZipCode4 = ProviderManagementHelper.GetString("ZipCode4", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].Country = ProviderManagementHelper.GetString("Country", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("AddrEffectiveDate", dr);
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].EndDate = ProviderManagementHelper.GetStringDateTime("AddrEndDate", dr);
                //enProviderManagedEmployeesListProviderManagedEmployeeAddress[i].ProvOwnrAdrsSrcKey = ProviderManagementHelper.GetString("ProvOwnrAdrsSrcKey", dr);
            }
            return enProviderManagedEmployeesListProviderManagedEmployeeAddress;

        }

        public static ProviderOwnershipList[] fillProviderOwnershipList(DataTable dt)
        {

            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderOwnershipList[] enProviderOwnershipList = new ProviderManagementReference.ProviderOwnershipList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderOwnershipList[i] = new ProviderManagementReference.ProviderOwnershipList();
                enProviderOwnershipList[i].OwnerAddress = fillProviderOwnershipListAddress(dr);
                enProviderOwnershipList[i].OwnershipDetails = fillProviderOwnershipListOwnershipDetails(dr);
            }
            return enProviderOwnershipList;
        }

        public static ProviderOwnershipListAddress[] fillProviderOwnershipListAddress(DataRow dr)
        {
            int countRow = 1;
            if (string.IsNullOrEmpty(ProviderManagementHelper.GetString("AddressLine1", dr).Trim()))
            {
                dr = null;
                countRow = 0;
            }
            ProviderManagementReference.ProviderOwnershipListAddress[] enProviderOwnershipListAddress = new ProviderManagementReference.ProviderOwnershipListAddress[countRow];
            for (int i = 0; i < countRow; i++)
            {
                enProviderOwnershipListAddress[i] = new ProviderManagementReference.ProviderOwnershipListAddress();
                enProviderOwnershipListAddress[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderOwnershipListAddress[i].AddressType = ProviderManagementHelper.GetString("AddressType", dr);
                enProviderOwnershipListAddress[i].AddressLine1 = ProviderManagementHelper.GetString("AddressLine1", dr);
                enProviderOwnershipListAddress[i].AddressLine2 = ProviderManagementHelper.GetString("AddressLine2", dr);
                enProviderOwnershipListAddress[i].City = ProviderManagementHelper.GetString("City", dr);
                enProviderOwnershipListAddress[i].State = ProviderManagementHelper.GetString("State", dr);
                enProviderOwnershipListAddress[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enProviderOwnershipListAddress[i].ZipCode4 = ProviderManagementHelper.GetString("ZipCode4", dr);
                enProviderOwnershipListAddress[i].ZipCode5 = ProviderManagementHelper.GetString("ZipCode5", dr);
                enProviderOwnershipListAddress[i].Country = ProviderManagementHelper.GetString("Country", dr);
                enProviderOwnershipListAddress[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("AddrEffectiveDate", dr);
                enProviderOwnershipListAddress[i].EndDate = ProviderManagementHelper.GetStringDateTime("AddrEndDate", dr);
                //enProviderOwnershipListAddress[i].ProvOwnrAdrsSrcKey = ProviderManagementHelper.GetString("ProvOwnrAdrsSrcKey", dr);
            }
            return enProviderOwnershipListAddress;
        }

        public static ProviderOwnershipListOwnershipDetails fillProviderOwnershipListOwnershipDetails(DataRow dr)
        {
            int countRow = 1;
            ProviderManagementReference.ProviderOwnershipListOwnershipDetails enProviderOwnershipListOwnershipDetails = new ProviderManagementReference.ProviderOwnershipListOwnershipDetails();
            for (int i = 0; i < countRow; i++)
            {
                enProviderOwnershipListOwnershipDetails.OwnerType = ProviderManagementHelper.GetString("OwnerType", dr);
                enProviderOwnershipListOwnershipDetails.OwnerAffiliationType = ProviderManagementHelper.GetString("OwnerAffiliationType", dr);
                enProviderOwnershipListOwnershipDetails.OwnPercentage = ProviderManagementHelper.GetDecimalString("OwnPercentage", dr);
                enProviderOwnershipListOwnershipDetails.SanctionInd = ProviderManagementHelper.GetString("SanctionInd", dr);
                enProviderOwnershipListOwnershipDetails.SanctionExplain = ProviderManagementHelper.GetString("SanctionExplain", dr);
                enProviderOwnershipListOwnershipDetails.RelationshipInd = ProviderManagementHelper.GetString("RelationshipInd", dr);
                enProviderOwnershipListOwnershipDetails.CriminalOffenseInd = ProviderManagementHelper.GetString("CriminalOffenseInd", dr);
                enProviderOwnershipListOwnershipDetails.CriminalOffenseExplain = ProviderManagementHelper.GetString("CriminalOffenseExplain", dr);

                enProviderOwnershipListOwnershipDetails.LicenseSuspendInd = ProviderManagementHelper.GetString("LicenseSuspendInd", dr);
                enProviderOwnershipListOwnershipDetails.LicenseSuspendExplain = ProviderManagementHelper.GetString("LicenseSuspendExplain", dr);
                enProviderOwnershipListOwnershipDetails.PenaltyInd = ProviderManagementHelper.GetString("PenaltyInd", dr);
                enProviderOwnershipListOwnershipDetails.PenaltyExplain = ProviderManagementHelper.GetString("PenaltyExplain", dr);
                enProviderOwnershipListOwnershipDetails.CntrlIntrstOthrProvEntityInd = ProviderManagementHelper.GetString("CntrlIntrstOthrProvEntityInd", dr);
                enProviderOwnershipListOwnershipDetails.CntrlIntrstOthrProvEntityExplain = ProviderManagementHelper.GetString("CntrlIntrstOthrProvEntityExplain", dr);
                enProviderOwnershipListOwnershipDetails.OwnrSubcntrctrTrnxInd = ProviderManagementHelper.GetString("OwnrSubcntrctrTrnxInd", dr);
                enProviderOwnershipListOwnershipDetails.OwnrSubcntrctrDetails = ProviderManagementHelper.GetString("OwnrSubcntrctrDetails", dr);
                enProviderOwnershipListOwnershipDetails.OwnrSubcntrctrBsnssTrnxInd = ProviderManagementHelper.GetString("OwnrSubcntrctrBsnssTrnxInd", dr);
                enProviderOwnershipListOwnershipDetails.OwnrSubcntrctrBsnssTrnxDetails = ProviderManagementHelper.GetString("OwnrSubcntrctrBsnssTrnxDetails", dr);
                enProviderOwnershipListOwnershipDetails.TransactionAmount = ProviderManagementHelper.GetString("TransactionAmount", dr);
                enProviderOwnershipListOwnershipDetails.TransactionDate = ProviderManagementHelper.GetStringDateTime("TransactionDate", dr);
                enProviderOwnershipListOwnershipDetails.OwnerOutOfStateResidentIndicator = ProviderManagementHelper.GetString("OwnerOutOfStateResidentIndicator", dr);
                enProviderOwnershipListOwnershipDetails.OwnerOutOfStateResidentExplain = ProviderManagementHelper.GetString("OwnerOutOfStateResidentExplain", dr);
                enProviderOwnershipListOwnershipDetails.OwnerStateFederalOffenseInd = ProviderManagementHelper.GetString("OwnerStateFederalOffenseInd", dr);
                enProviderOwnershipListOwnershipDetails.OwnerStateFederalOffenseExplain = ProviderManagementHelper.GetString("OwnerStateFederalOffenseExplain", dr);

                enProviderOwnershipListOwnershipDetails.FingerprintStatusIndicator = ProviderManagementHelper.GetString("FingerprintStatusIndicator", dr);
                enProviderOwnershipListOwnershipDetails.RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderOwnershipListOwnershipDetails.EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderOwnershipListOwnershipDetails.EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);

                //OHPNM -14007
                if (ProviderManagementHelper.GetString("OwnerType", dr).Equals("I")
                    || ProviderManagementHelper.GetString("OwnerType", dr).Equals("RI")
                    || ProviderManagementHelper.GetString("OwnerType", dr).Equals("SI")
                    || ProviderManagementHelper.GetString("OwnerType", dr).Equals("SUI")
                    )
                {
                    ProviderManagementReference.OwnerOwnerIndividual enooIndiv = new ProviderManagementReference.OwnerOwnerIndividual();
                    enooIndiv.FirstName = ProviderManagementHelper.GetString("FirstName", dr);
                    enooIndiv.MiddleName = ProviderManagementHelper.GetString("MiddleName", dr);
                    enooIndiv.LastName = ProviderManagementHelper.GetString("LastName", dr);
                    enooIndiv.DateOfBirth = ProviderManagementHelper.GetStringDateTime("DateOfBirth", dr);
                    enooIndiv.SSN = ProviderManagementHelper.GetString("SSN", dr);
                    enooIndiv.Title = ProviderManagementHelper.GetString("Title", dr);
                    enooIndiv.OwnerIndvSrcKey = ProviderManagementHelper.GetString("OwnerIndvSrcKey", dr);
                    enProviderOwnershipListOwnershipDetails.Item = enooIndiv;
                }
                else
                {
                    if(dr != null)
                    {                    
                    ProviderManagementReference.OwnerOwnerOrganization enooOrg = new ProviderManagementReference.OwnerOwnerOrganization();
                    enooOrg.OwnerOrgName = ProviderManagementHelper.GetString("OwnerOrgName", dr);
                    enooOrg.OrgTaxIdNumber = ProviderManagementHelper.GetString("OrgTaxIdNumber", dr);
                    enooOrg.Title = ProviderManagementHelper.GetString("Title", dr);
                    enooOrg.OwnerOrgSrcKey = ProviderManagementHelper.GetString("OwnerOrgSrcKey", dr);
                    enProviderOwnershipListOwnershipDetails.Item = enooOrg;
                    }
                }

            }
            return enProviderOwnershipListOwnershipDetails;
        }

        public static ProgramAffiliationsListProgramAffiliation[] fillProgramAffiliationsListProgramAffiliation(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProgramAffiliationsListProgramAffiliation[] enProgramAffiliationsListProgramAffiliation = new ProviderManagementReference.ProgramAffiliationsListProgramAffiliation[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProgramAffiliationsListProgramAffiliation[i] = new ProviderManagementReference.ProgramAffiliationsListProgramAffiliation();
                enProgramAffiliationsListProgramAffiliation[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProgramAffiliationsListProgramAffiliation[i].AffiliatedProgramCode = ProviderManagementHelper.GetString("AffiliatedProgramCode", dr);
                enProgramAffiliationsListProgramAffiliation[i].API = ProviderManagementHelper.GetString("API", dr);
                enProgramAffiliationsListProgramAffiliation[i].AffiliatedProgramId = ProviderManagementHelper.GetString("AffiliatedProgramId", dr);
                enProgramAffiliationsListProgramAffiliation[i].ProgramAffiliationStatusCode = ProviderManagementHelper.GetString("ProgramAffiationStatusCode", dr);
                enProgramAffiliationsListProgramAffiliation[i].ProgramAffiliationStatusReason = ProviderManagementHelper.GetString("ProgramAffiliationStatusReason", dr);
                enProgramAffiliationsListProgramAffiliation[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProgramAffiliationsListProgramAffiliation[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProgramAffiliationsListProgramAffiliation[i].PgmAffltnSrcKey = ProviderManagementHelper.GetString("PgmAffltnSrcKey", dr);
            }
            return enProgramAffiliationsListProgramAffiliation;
        }
        public static ProviderReviewsListReview[] fillProviderReviewsListReview(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderReviewsListReview[] enProviderReviewsListReview = new ProviderManagementReference.ProviderReviewsListReview[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderReviewsListReview[i] = new ProviderManagementReference.ProviderReviewsListReview();
                enProviderReviewsListReview[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderReviewsListReview[i].ProviderReviewTypeCode = ProviderManagementHelper.GetString("ProviderReviewTypeCode", dr);

                if (!string.IsNullOrEmpty(ProviderManagementHelper.GetString("ProposedEffectiveDate", dr)))
                {
                    enProviderReviewsListReview[i].ProposedEffectiveDate = ProviderManagementHelper.GetStringDateTime("ProposedEffectiveDate", dr);
                }

                
                enProviderReviewsListReview[i].ProviderReviewID = ProviderManagementHelper.GetString("ProviderReviewID", dr);
                enProviderReviewsListReview[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderReviewsListReview[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderReviewsListReview[i].Provrvwsrccode = ProviderManagementHelper.GetString("Provrvwsrccode", dr);
            }
            return enProviderReviewsListReview;
        }
        public static EnrollProviderServiceLocation[] fillEnrollProviderServiceLocation(DataSet ds)
        {
            DataTable dt = ds.Tables[17];
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.EnrollProviderServiceLocation[] enEnrollProviderServiceLocation = new ProviderManagementReference.EnrollProviderServiceLocation[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderServiceLocation[i] = new ProviderManagementReference.EnrollProviderServiceLocation();
                enEnrollProviderServiceLocation[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderServiceLocation[i].LocationName = ProviderManagementHelper.GetString("LocationName", dr);
                enEnrollProviderServiceLocation[i].LocationCode = ProviderManagementHelper.GetString("LocationCode", dr);
                enEnrollProviderServiceLocation[i].OrgStateOwnedIndicator = ProviderManagementHelper.GetString("OrgStateOwnedIndicator", dr);
                enEnrollProviderServiceLocation[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderServiceLocation[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);

                if (!string.IsNullOrEmpty(ProviderManagementHelper.GetString("AddressType", dr)))
                {
                    enEnrollProviderServiceLocation[i].ServiceLocationAddress = fillEnrollProviderServiceLocationAddress(dr);
                }
                else
                {
                    enEnrollProviderServiceLocation[i].ServiceLocationAddress = fillEnrollProviderServiceLocationAddress(null);
                }
                if (i == 0)
                {


                    enEnrollProviderServiceLocation[i].ServiceLocationContact = fillEnrollProviderServiceLocationContact(null);

                    enEnrollProviderServiceLocation[i].ProviderLicense = fillEnrollProviderServiceLocationLicense(ds);
                    enEnrollProviderServiceLocation[i].ProviderCertification = fillProviderCertificationListCertification(ds.Tables[22]);
                    enEnrollProviderServiceLocation[i].ProviderIdentifier = fillProviderIdentifierListIdentifier(ds.Tables[24]);
                    enEnrollProviderServiceLocation[i].ProviderFacility = fillProviderFacilityListFacility(ds.Tables[25]);
                    enEnrollProviderServiceLocation[i].ProviderTaxonomy = fillProviderTaxonomyListTaxonomy(ds.Tables[26]);
                    enEnrollProviderServiceLocation[i].ProviderSpecialty = fillProviderSpecialtyListSpecialty(ds.Tables[27]);
                    // enEnrollProviderServiceLocation[i].ProviderType = fillProviderType(null);
                    enEnrollProviderServiceLocation[i].ProviderRestriction = fillEnrollProviderRestrictionListRestriction(ds.Tables[28]);

                    enEnrollProviderServiceLocation[i].SiteVisits = fillVisitListVisit(ds.Tables[29]);
                    enEnrollProviderServiceLocation[i].ProviderTraining = fillProviderTrainingListTraining(ds.Tables[30]);
                }
                enEnrollProviderServiceLocation[i].ProvSvcLocationSrcKey = ProviderManagementHelper.GetString("ProvSvcLocationSrcKey", dr);
            }
            return enEnrollProviderServiceLocation;
        }



        public static EnrollProviderServiceLocationContact[] fillEnrollProviderServiceLocationContact(DataTable dt)
        {
            int countRow = 0;
            if (dt != null)
            {
                countRow = dt.Rows.Count;
            }

            DataRow dr = null;
            ProviderManagementReference.EnrollProviderServiceLocationContact[] enEnrollProviderServiceLocationContact = new ProviderManagementReference.EnrollProviderServiceLocationContact[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderServiceLocationContact[i] = new ProviderManagementReference.EnrollProviderServiceLocationContact();
                enEnrollProviderServiceLocationContact[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderServiceLocationContact[i].ContactType = ProviderManagementHelper.GetString("ContactType", dr);
                enEnrollProviderServiceLocationContact[i].ContactDetail = ProviderManagementHelper.GetString("ContactDetail", dr);
                enEnrollProviderServiceLocationContact[i].ContactInstruction = ProviderManagementHelper.GetString("ContactInstruction", dr);
                enEnrollProviderServiceLocationContact[i].PreferredContactIndicator = ProviderManagementHelper.GetString("PreferredContactIndicator", dr);
                enEnrollProviderServiceLocationContact[i].SvcLctnCntctSrcKey = ProviderManagementHelper.GetString("SvcLctnCntctSrcKey", dr);
            }
            return enEnrollProviderServiceLocationContact;
        }
        public static EnrollProviderServiceLocationLicense[] fillEnrollProviderServiceLocationLicense(DataSet ds)
        {
            DataTable dt = ds.Tables[23];
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            int StgProviderLicenseId = 0;
            ProviderManagementReference.EnrollProviderServiceLocationLicense[] enEnrollProviderServiceLocationLicense = new ProviderManagementReference.EnrollProviderServiceLocationLicense[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                StgProviderLicenseId = ProviderManagementHelper.GetInt("StgProviderLicenseId", dr);
                enEnrollProviderServiceLocationLicense[i] = new ProviderManagementReference.EnrollProviderServiceLocationLicense();
                //--enEnrollProviderServiceLocationLicense[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderServiceLocationLicense[i].LicenseType = ProviderManagementHelper.GetString("LicenseType", dr);
                enEnrollProviderServiceLocationLicense[i].LicenseNumber = ProviderManagementHelper.GetString("LicenseNumber", dr);
                enEnrollProviderServiceLocationLicense[i].LicenseeStateCode = ProviderManagementHelper.GetString("LicenseeStateCode", dr);
                enEnrollProviderServiceLocationLicense[i].LicenseIssueBoardNotes = ProviderManagementHelper.GetString("LicenseIssueBoardNotes", dr);
                enEnrollProviderServiceLocationLicense[i].Status = ProviderManagementHelper.GetString("Status", dr);
                enEnrollProviderServiceLocationLicense[i].StatusReason = ProviderManagementHelper.GetString("StatusReason", dr);
                enEnrollProviderServiceLocationLicense[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enEnrollProviderServiceLocationLicense[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderServiceLocationLicense[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enEnrollProviderServiceLocationLicense[i].ProvLicenseSrcKey = ProviderManagementHelper.GetString("ProvLicenseSrcKey", dr);
                if (!(ProviderManagementHelper.GetString("LicenseType", dr).Equals("DEA")))
                {
                    enEnrollProviderServiceLocationLicense[i].LicenseCertifications = fillLicenseCertificationsListCertifications(ds, StgProviderLicenseId);
                }
                else
                {
                    enEnrollProviderServiceLocationLicense[i].LicenseCertifications = fillLicenseCertificationsListCertifications(null, StgProviderLicenseId);
                }
            }
            return enEnrollProviderServiceLocationLicense;
        }
        public static LicenseCertificationsListCertifications[] fillLicenseCertificationsListCertifications(DataSet ds, int StgProviderLicenseId)
        {
            DataTable dt = new DataTable();
            int countRow = 0;
            if (ds != null)
            {
                DataView dvSections = ds.Tables[33].DefaultView;
                dvSections.RowFilter = "StgProviderLicenseId = " + StgProviderLicenseId.ToString();
                dt = dvSections.ToTable();
                //dt = ds.Tables[33];
                countRow = dt.Rows.Count;
            }
            
            DataRow dr = null;
            ProviderManagementReference.LicenseCertificationsListCertifications[] enLicenseCertificationsListCertifications = new ProviderManagementReference.LicenseCertificationsListCertifications[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enLicenseCertificationsListCertifications[i] = new ProviderManagementReference.LicenseCertificationsListCertifications();
                enLicenseCertificationsListCertifications[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enLicenseCertificationsListCertifications[i].CertificationId = ProviderManagementHelper.GetString("CertificationId", dr);
                enLicenseCertificationsListCertifications[i].CertifiyingOrg = ProviderManagementHelper.GetString("CertifyingOrg", dr);
                enLicenseCertificationsListCertifications[i].CertificationFocus = ProviderManagementHelper.GetString("CertificationFocus", dr);
                enLicenseCertificationsListCertifications[i].CertificationSpeciality = ProviderManagementHelper.GetString("CertificationSpeciality", dr);
                enLicenseCertificationsListCertifications[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enLicenseCertificationsListCertifications[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enLicenseCertificationsListCertifications[i].ProvLicenseCertSrcKey = ProviderManagementHelper.GetString("ProvLicenseCertSrcKey", dr);
            }
            return enLicenseCertificationsListCertifications;
        }
        public static ProviderCertificationListCertification[] fillProviderCertificationListCertification(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderCertificationListCertification[] enProviderCertificationListCertification = new ProviderManagementReference.ProviderCertificationListCertification[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderCertificationListCertification[i] = new ProviderManagementReference.ProviderCertificationListCertification();
                enProviderCertificationListCertification[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderCertificationListCertification[i].CertificateNumber = ProviderManagementHelper.GetString("CertificateNumber", dr);
                enProviderCertificationListCertification[i].CertificateType = ProviderManagementHelper.GetString("CertificateType", dr);
                enProviderCertificationListCertification[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderCertificationListCertification[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderCertificationListCertification[i].ProvCrtfctnSrcKey = ProviderManagementHelper.GetString("ProvCrtfctnSrcKey", dr);
            }
            return enProviderCertificationListCertification;
        }
        public static ProviderIdentifierListIdentifier[] fillProviderIdentifierListIdentifier(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderIdentifierListIdentifier[] enProviderIdentifierListIdentifier = new ProviderManagementReference.ProviderIdentifierListIdentifier[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderIdentifierListIdentifier[i] = new ProviderManagementReference.ProviderIdentifierListIdentifier();
                enProviderIdentifierListIdentifier[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderIdentifierListIdentifier[i].IdentifierTypeCode = ProviderManagementHelper.GetString("IdentifierTypeCode", dr);
                enProviderIdentifierListIdentifier[i].IssuingEntityID = ProviderManagementHelper.GetString("IssuingEntityID", dr);
                enProviderIdentifierListIdentifier[i].IdentifierID = ProviderManagementHelper.GetString("IdentifierID", dr);
                enProviderIdentifierListIdentifier[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderIdentifierListIdentifier[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderIdentifierListIdentifier[i].ProvIdentifierSrcKey = ProviderManagementHelper.GetString("ProvIdentifierSrcKey", dr);
            }
            return enProviderIdentifierListIdentifier;
        }
        public static ProviderFacilityListFacility[] fillProviderFacilityListFacility(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderFacilityListFacility[] enProviderFacilityListFacility = new ProviderManagementReference.ProviderFacilityListFacility[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderFacilityListFacility[i] = new ProviderManagementReference.ProviderFacilityListFacility();
                enProviderFacilityListFacility[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderFacilityListFacility[i].FacilityId = ProviderManagementHelper.GetString("FacilityId", dr);
                enProviderFacilityListFacility[i].BedCount = ProviderManagementHelper.GetString("BedCount", dr);
                enProviderFacilityListFacility[i].BedTypeCode = ProviderManagementHelper.GetString("BedTypeCode", dr);
                enProviderFacilityListFacility[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderFacilityListFacility[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderFacilityListFacility[i].ProvFacilitySrcKey = ProviderManagementHelper.GetString("ProvFacilitySrcKey", dr);
            }
            return enProviderFacilityListFacility;
        }
        public static ProviderTaxonomyListTaxonomy[] fillProviderTaxonomyListTaxonomy(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderTaxonomyListTaxonomy[] enProviderTaxonomyListTaxonomy = new ProviderManagementReference.ProviderTaxonomyListTaxonomy[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderTaxonomyListTaxonomy[i] = new ProviderManagementReference.ProviderTaxonomyListTaxonomy();
                enProviderTaxonomyListTaxonomy[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderTaxonomyListTaxonomy[i].TaxonomyCode = ProviderManagementHelper.GetString("TaxonomyCode", dr);
                enProviderTaxonomyListTaxonomy[i].PrimaryTaxonomyIndicator = ProviderManagementHelper.GetString("PrimaryTaxonomyIndicator", dr);
                enProviderTaxonomyListTaxonomy[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderTaxonomyListTaxonomy[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderTaxonomyListTaxonomy[i].ProvTxnmySrcKey = ProviderManagementHelper.GetString("ProvTxnmySrcKey", dr);
            }
            return enProviderTaxonomyListTaxonomy;
        }
        public static ProviderSpecialtyListSpecialty[] fillProviderSpecialtyListSpecialty(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderSpecialtyListSpecialty[] enProviderSpecialtyListSpecialty = new ProviderManagementReference.ProviderSpecialtyListSpecialty[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderSpecialtyListSpecialty[i] = new ProviderManagementReference.ProviderSpecialtyListSpecialty();
                enProviderSpecialtyListSpecialty[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderSpecialtyListSpecialty[i].PrimarySpecialityIndicator = ProviderManagementHelper.GetString("PrimarySpecialityIndicator", dr);
                enProviderSpecialtyListSpecialty[i].SpecialtyCode = ProviderManagementHelper.GetString("SpecialtyCode", dr);
                enProviderSpecialtyListSpecialty[i].SpecialityStatusCode = ProviderManagementHelper.GetString("SpecialityStatusCode", dr);
                enProviderSpecialtyListSpecialty[i].SpecialityStatusReasonCode = ProviderManagementHelper.GetString("SpecialityStatusReasonCode", dr);
                enProviderSpecialtyListSpecialty[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderSpecialtyListSpecialty[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderSpecialtyListSpecialty[i].ProvSpcltySrcKey = ProviderManagementHelper.GetString("ProvSpcltySrcKey", dr);
            }
            return enProviderSpecialtyListSpecialty;
        }
        public static EnrollProviderRestrictionListRestriction[] fillEnrollProviderRestrictionListRestriction(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.EnrollProviderRestrictionListRestriction[] enEnrollProviderRestrictionListRestriction = new ProviderManagementReference.EnrollProviderRestrictionListRestriction[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderRestrictionListRestriction[i] = new ProviderManagementReference.EnrollProviderRestrictionListRestriction();
                enEnrollProviderRestrictionListRestriction[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderRestrictionListRestriction[i].ReviewReason = ProviderManagementHelper.GetString("ReviewReason", dr);
                enEnrollProviderRestrictionListRestriction[i].ReviewType = ProviderManagementHelper.GetString("ReviewType", dr);
                enEnrollProviderRestrictionListRestriction[i].RestrictionClass = ProviderManagementHelper.GetString("RestrictionClass", dr);
                enEnrollProviderRestrictionListRestriction[i].ClaimType = ProviderManagementHelper.GetString("ClaimType", dr);
                enEnrollProviderRestrictionListRestriction[i].IncludeExcludeIndicator = ProviderManagementHelper.GetString("IncludeExcludeIndicator", dr);

                enEnrollProviderRestrictionListRestriction[i].LowCode = ProviderManagementHelper.GetString("LowCode", dr);
                enEnrollProviderRestrictionListRestriction[i].HighCode = ProviderManagementHelper.GetString("HighCode", dr);
                enEnrollProviderRestrictionListRestriction[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderRestrictionListRestriction[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enEnrollProviderRestrictionListRestriction[i].ProvRestrictionSrcKey = ProviderManagementHelper.GetString("ProvRestrictionSrcKey", dr);
            }
            return enEnrollProviderRestrictionListRestriction;
        }


        public static ServiceLocationAddressListAddress[] fillUpdateProviderServiceLocationAddress(DataRow dr)
        {
            int countRow = 0;
            if (dr != null)
            {
                countRow = 1;
            }

            ProviderManagementReference.ServiceLocationAddressListAddress[] enEnrollProviderServiceLocationAddress = new ProviderManagementReference.ServiceLocationAddressListAddress[countRow];
            for (int i = 0; i < countRow; i++)
            {
                //dr = dt.Rows[i];
                enEnrollProviderServiceLocationAddress[i] = new ProviderManagementReference.ServiceLocationAddressListAddress();
                enEnrollProviderServiceLocationAddress[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderServiceLocationAddress[i].AddressType = ProviderManagementHelper.GetString("AddressType", dr);
                enEnrollProviderServiceLocationAddress[i].AddressLine1 = ProviderManagementHelper.GetString("AddressLine1", dr);
                enEnrollProviderServiceLocationAddress[i].AddressLine2 = ProviderManagementHelper.GetString("AddressLine2", dr);
                enEnrollProviderServiceLocationAddress[i].City = ProviderManagementHelper.GetString("City", dr);
                enEnrollProviderServiceLocationAddress[i].State = ProviderManagementHelper.GetString("State", dr);

                enEnrollProviderServiceLocationAddress[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enEnrollProviderServiceLocationAddress[i].ZipCode5 = ProviderManagementHelper.GetString("ZipCode5", dr);
                enEnrollProviderServiceLocationAddress[i].ZipCode4 = ProviderManagementHelper.GetString("ZipCode4", dr);
                enEnrollProviderServiceLocationAddress[i].Country = ProviderManagementHelper.GetString("Country", dr);
                enEnrollProviderServiceLocationAddress[i].BorderStateIndicator = ProviderManagementHelper.GetString("BorderStateIndicator", dr);
                enEnrollProviderServiceLocationAddress[i].AddressNameTypeIndicator = ProviderManagementHelper.GetString("AddressNameTypeIndicator", dr);

                enEnrollProviderServiceLocationAddress[i].ProvOrgAddressName = ProviderManagementHelper.GetString("ProvOrgAddressName", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgAddressFirstName = ProviderManagementHelper.GetString("ProvOrgAddressFirstName", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgAddressMiddleName = ProviderManagementHelper.GetString("ProvOrgAddressMiddleName", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgAddressLastName = ProviderManagementHelper.GetString("ProvOrgAddressLastName", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgPhoneNumber1 = ProviderManagementHelper.GetString("ProvOrgPhoneNumber1", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgPhoneNumberExt1 = ProviderManagementHelper.GetString("ProvOrgPhoneNumberExt1", dr);

                enEnrollProviderServiceLocationAddress[i].ProvOrgPhoneNumber2 = ProviderManagementHelper.GetString("ProvOrgPhoneNumber2", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgPhoneNumberExt2 = ProviderManagementHelper.GetString("ProvOrgPhoneNumberExt2", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgFaxNumber1 = ProviderManagementHelper.GetString("ProvOrgFaxNumber1", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgFaxNumber2 = ProviderManagementHelper.GetString("ProvOrgFaxNumber2", dr);
                enEnrollProviderServiceLocationAddress[i].ProvAddressContactName = ProviderManagementHelper.GetString("ProvAddressContactName", dr);
                enEnrollProviderServiceLocationAddress[i].ProvEmail = ProviderManagementHelper.GetString("ProvEmail", dr);

                enEnrollProviderServiceLocationAddress[i].Longitude = ProviderManagementHelper.GetString("ProvAddressLongitude", dr);
                enEnrollProviderServiceLocationAddress[i].Latitude = ProviderManagementHelper.GetString("ProvAddressLatitude", dr);
                enEnrollProviderServiceLocationAddress[i].HandicapAccessibilityIndicator = ProviderManagementHelper.GetString("HandicapAccessibilityIndicator", dr);
                enEnrollProviderServiceLocationAddress[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderServiceLocationAddress[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enEnrollProviderServiceLocationAddress[i].SvcLctnAdrsSrcKey = ProviderManagementHelper.GetString("ProvAddressSrcKey", dr);
            }
            return enEnrollProviderServiceLocationAddress;
        }



        public static EnrollProviderServiceLocationAddress[] fillEnrollProviderServiceLocationAddress(DataRow dr)
        {
            int countRow = 0;
            if (dr != null)
            {
                countRow = 1;
            }

            ProviderManagementReference.EnrollProviderServiceLocationAddress[] enEnrollProviderServiceLocationAddress = new ProviderManagementReference.EnrollProviderServiceLocationAddress[countRow];
            for (int i = 0; i < countRow; i++)
            {
                //dr = dt.Rows[i];
                enEnrollProviderServiceLocationAddress[i] = new ProviderManagementReference.EnrollProviderServiceLocationAddress();
                enEnrollProviderServiceLocationAddress[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderServiceLocationAddress[i].AddressType = ProviderManagementHelper.GetString("AddressType", dr);
                enEnrollProviderServiceLocationAddress[i].AddressLine1 = ProviderManagementHelper.GetString("AddressLine1", dr);
                enEnrollProviderServiceLocationAddress[i].AddressLine2 = ProviderManagementHelper.GetString("AddressLine2", dr);
                enEnrollProviderServiceLocationAddress[i].City = ProviderManagementHelper.GetString("City", dr);
                enEnrollProviderServiceLocationAddress[i].State = ProviderManagementHelper.GetString("State", dr);

                enEnrollProviderServiceLocationAddress[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enEnrollProviderServiceLocationAddress[i].ZipCode5 = ProviderManagementHelper.GetString("ZipCode5", dr);
                enEnrollProviderServiceLocationAddress[i].ZipCode4 = ProviderManagementHelper.GetString("ZipCode4", dr);
                enEnrollProviderServiceLocationAddress[i].Country = ProviderManagementHelper.GetString("Country", dr);
                enEnrollProviderServiceLocationAddress[i].BorderStateIndicator = ProviderManagementHelper.GetString("BorderStateIndicator", dr);
                enEnrollProviderServiceLocationAddress[i].AddressNameTypeIndicator = ProviderManagementHelper.GetString("AddressNameTypeIndicator", dr);

                enEnrollProviderServiceLocationAddress[i].ProvOrgAddressName = ProviderManagementHelper.GetString("ProvOrgAddressName", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgAddressFirstName = ProviderManagementHelper.GetString("ProvOrgAddressFirstName", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgAddressMiddleName = ProviderManagementHelper.GetString("ProvOrgAddressMiddleName", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgAddressLastName = ProviderManagementHelper.GetString("ProvOrgAddressLastName", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgPhoneNumber1 = ProviderManagementHelper.GetString("ProvOrgPhoneNumber1", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgPhoneNumberExt1 = ProviderManagementHelper.GetString("ProvOrgPhoneNumberExt1", dr);

                enEnrollProviderServiceLocationAddress[i].ProvOrgPhoneNumber2 = ProviderManagementHelper.GetString("ProvOrgPhoneNumber2", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgPhoneNumberExt2 = ProviderManagementHelper.GetString("ProvOrgPhoneNumberExt2", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgFaxNumber1 = ProviderManagementHelper.GetString("ProvOrgFaxNumber1", dr);
                enEnrollProviderServiceLocationAddress[i].ProvOrgFaxNumber2 = ProviderManagementHelper.GetString("ProvOrgFaxNumber2", dr);
                enEnrollProviderServiceLocationAddress[i].ProvAddressContactName = ProviderManagementHelper.GetString("ProvAddressContactName", dr);
                enEnrollProviderServiceLocationAddress[i].ProvEmail = ProviderManagementHelper.GetString("ProvEmail", dr);

                enEnrollProviderServiceLocationAddress[i].Longitude = ProviderManagementHelper.GetString("ProvAddressLongitude", dr);
                enEnrollProviderServiceLocationAddress[i].Latitude = ProviderManagementHelper.GetString("ProvAddressLatitude", dr);
                enEnrollProviderServiceLocationAddress[i].HandicapAccessibilityIndicator = ProviderManagementHelper.GetString("HandicapAccessibilityIndicator", dr);
                enEnrollProviderServiceLocationAddress[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enEnrollProviderServiceLocationAddress[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enEnrollProviderServiceLocationAddress[i].SvcLctnAdrsSrcKey = ProviderManagementHelper.GetString("ProvAddressSrcKey", dr);
            }
            return enEnrollProviderServiceLocationAddress;
        }

        public static VisitListVisit[] fillVisitListVisit(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.VisitListVisit[] enVisitListVisit = new ProviderManagementReference.VisitListVisit[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enVisitListVisit[i] = new ProviderManagementReference.VisitListVisit();
                enVisitListVisit[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enVisitListVisit[i].SiteVisitID = ProviderManagementHelper.GetString("SiteVisitID", dr);
                enVisitListVisit[i].SiteVisitResult = ProviderManagementHelper.GetString("SiteVisitResult", dr);
                enVisitListVisit[i].SiteVisitText = ProviderManagementHelper.GetString("SiteVisitText", dr);
                enVisitListVisit[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enVisitListVisit[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enVisitListVisit[i].Provvstsrckey = ProviderManagementHelper.GetString("Provvstsrckey", dr);
            }
            return enVisitListVisit;
        }

        public static ProviderTrainingListTraining[] fillProviderTrainingListTraining(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderTrainingListTraining[] enProviderTrainingListTraining = new ProviderManagementReference.ProviderTrainingListTraining[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderTrainingListTraining[i] = new ProviderManagementReference.ProviderTrainingListTraining();
                enProviderTrainingListTraining[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderTrainingListTraining[i].TrainingType = ProviderManagementHelper.GetString("TrainingType", dr);
                enProviderTrainingListTraining[i].TrainingId = ProviderManagementHelper.GetString("TrainingId", dr);
                enProviderTrainingListTraining[i].TrainingCompleteIndicator = ProviderManagementHelper.GetString("TrainingCompleteIndicator", dr);
                enProviderTrainingListTraining[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderTrainingListTraining[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderTrainingListTraining[i].Provtrnsrckey = ProviderManagementHelper.GetString("Provtrnsrckey", dr);
            }
            return enProviderTrainingListTraining;
        }

        public static ProviderServicesListServices[] fillProviderServicesListServices(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            ProviderManagementReference.ProviderServicesListServices[] enProviderServicesListServices = new ProviderManagementReference.ProviderServicesListServices[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderServicesListServices[i] = new ProviderManagementReference.ProviderServicesListServices();
                enProviderServicesListServices[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderServicesListServices[i].ProviderServiceID = ProviderManagementHelper.GetString("ProviderserviceID", dr);
                enProviderServicesListServices[i].ProviderService = ProviderManagementHelper.GetString("ProviderService", dr);
                enProviderServicesListServices[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderServicesListServices[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderServicesListServices[i].Provsvcsrckey = ProviderManagementHelper.GetString("Provsvcsrckey", dr);
            }
            return enProviderServicesListServices;
        }

    }
}
