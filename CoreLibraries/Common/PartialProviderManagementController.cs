using Corp.Core.Libraries.PartialProviderManagementReference;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSWebControls;

namespace Corp.Core.Libraries
{
    public class PartialProviderManagementController   // WHY IS THIS IN THIS PROJECT? THE ONLY REASON WE NEED AN EXTRA COPY OF ONBASE FUNCTIONS IS THIS SINGLE CALL ON LINE 23
    {
        public static string GetDocumentBase64Binary(string fileName, int documentID)
        {
            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            string filePath = Path.Combine(InfoAccess.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName);  /// WHY ???????
            
            OnBaseComponents onBaseInterface = new OnBaseComponents();
            byte[] encryptedFile = onBaseInterface.RetrieveFile(filePath, documentID);

            // this could be decrypting things that are not encrypted - moving to central location
            EncryptorRIJ encryptObj = new EncryptorRIJ();
            byte[] decryptedFile = encryptObj.DecryptRijndael(encryptedFile);

            string pdfBase64 = Convert.ToBase64String(decryptedFile);
            return pdfBase64;
        }

        public static PartialProviderDocumentsList[] fillPartialProviderDocumentsList(DataTable dt)
        {
            int countRow = 0;
            if (dt != null)
            {
                countRow = dt.Rows.Count;
            }
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderDocumentsList[] enPartialProviderDocumentsList = new PartialProviderManagementReference.PartialProviderDocumentsList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enPartialProviderDocumentsList[i] = new PartialProviderManagementReference.PartialProviderDocumentsList();
                enPartialProviderDocumentsList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enPartialProviderDocumentsList[i].DocName = ProviderManagementHelper.GetString("DocName", dr);
                enPartialProviderDocumentsList[i].DocType = ProviderManagementHelper.GetString("DocType", dr);
                enPartialProviderDocumentsList[i].DocExt = ProviderManagementHelper.GetString("DocExt", dr);
                enPartialProviderDocumentsList[i].DocDescription = ProviderManagementHelper.GetString("DocDescription", dr);
                enPartialProviderDocumentsList[i].ContentBase64binary = GetDocumentBase64Binary(ProviderManagementHelper.GetString("DocName", dr), ProviderManagementHelper.GetInt("DocumentID", dr));
                enPartialProviderDocumentsList[i].Provdocsrckey = ProviderManagementHelper.GetString("Provdocsrckey", dr);
            }
            return enPartialProviderDocumentsList;
        }

        public static PartialProviderDemographics fillProviderDemographic(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderDemographics enPartialProviderDemographics = new PartialProviderManagementReference.PartialProviderDemographics();
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enPartialProviderDemographics.RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enPartialProviderDemographics.ProviderId = ProviderManagementHelper.GetString("ProviderId", dr);
                enPartialProviderDemographics.TaxId = ProviderManagementHelper.GetString("TaxId", dr);
                enPartialProviderDemographics.ProviderRoleCode = ProviderManagementHelper.GetString("ProviderRoleCode", dr);
                enPartialProviderDemographics.NationalProviderIdentifier = ProviderManagementHelper.GetString("NationalProviderIdentifier", dr);
                enPartialProviderDemographics.FirstName = ProviderManagementHelper.GetString("FirstName", dr);
                enPartialProviderDemographics.MiddleName = ProviderManagementHelper.GetString("MiddleName", dr);
                enPartialProviderDemographics.LastName = ProviderManagementHelper.GetString("LastName", dr);
                enPartialProviderDemographics.Prefix = ProviderManagementHelper.GetString("Prefix", dr);
                enPartialProviderDemographics.Suffix = ProviderManagementHelper.GetString("Suffix", dr);
                enPartialProviderDemographics.SSN = ProviderManagementHelper.GetString("SSN", dr);
                enPartialProviderDemographics.Gender = ProviderManagementHelper.GetString("Gender", dr);
                enPartialProviderDemographics.EntityType = ProviderManagementHelper.GetString("EntityType", dr);
                enPartialProviderDemographics.OrganizationBusinessName = ProviderManagementHelper.GetString("ProviderName", dr);
                enPartialProviderDemographics.LegalProviderName = ProviderManagementHelper.GetString("LegalProviderName", dr);
                enPartialProviderDemographics.ProviderBusinessName = ProviderManagementHelper.GetString("ProviderBusinessName", dr);
                enPartialProviderDemographics.ProviderTaxName = ProviderManagementHelper.GetString("ProviderTaxName", dr);
                enPartialProviderDemographics.TeachingIndicator = ProviderManagementHelper.GetString("TeachingIndicator", dr);
                enPartialProviderDemographics.OwnershipCode = ProviderManagementHelper.GetString("OwnershipCode", dr);
                enPartialProviderDemographics.PracticeTypeCode = ProviderManagementHelper.GetString("PracticeTypeCode", dr);
                enPartialProviderDemographics.ProviderProfitStatusCode = ProviderManagementHelper.GetString("ProviderProfitStatusCode", dr);
                enPartialProviderDemographics.NewPatientIndicator = ProviderManagementHelper.GetString("NewPatientIndicator", dr);
                enPartialProviderDemographics.EnrollmentType = ProviderManagementHelper.GetString("EnrollmentType", dr);
                enPartialProviderDemographics.EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enPartialProviderDemographics.EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);

                if (!ProviderManagementHelper.GetStringDateTime("DateOfBirth", dr).Equals("1900-01-01T00:00:00"))
                {
                    enPartialProviderDemographics.DateOfBirth = ProviderManagementHelper.GetStringDateTime("DateOfBirth", dr);
                }
                else
                {
                    enPartialProviderDemographics.DateOfBirth = string.Empty;
                }
                enPartialProviderDemographics.DateOfDeath = ProviderManagementHelper.GetStringDateTime("DateOfDeath", dr);
                enPartialProviderDemographics.Race = ProviderManagementHelper.GetString("Race", dr);
                enPartialProviderDemographics.MedicareInd = ProviderManagementHelper.GetString("MedicareInd", dr);
                enPartialProviderDemographics.PCPStatusCode = ProviderManagementHelper.GetString("PCPStatusCode", dr);
                enPartialProviderDemographics.PCPTermDate = ProviderManagementHelper.GetStringDateTime("PCPTermDate", dr);
                enPartialProviderDemographics.BirthCountry = ProviderManagementHelper.GetString("BirthCountry", dr);
                enPartialProviderDemographics.BirthCity = ProviderManagementHelper.GetString("BirthCity", dr);
                enPartialProviderDemographics.BirthState = ProviderManagementHelper.GetString("BirthState", dr);
                enPartialProviderDemographics.ProvRiskLevel = ProviderManagementHelper.GetString("ProvRiskLevel", dr);
                enPartialProviderDemographics.ProvDirectorySearchIndicator = ProviderManagementHelper.GetString("ProvDirectorySearchIndicator", dr);
                enPartialProviderDemographics.PaymentTypeCode = ProviderManagementHelper.GetString("PaymentTypeCode", dr);
                enPartialProviderDemographics.ProvIHSAssoCode = ProviderManagementHelper.GetString("ProvIHSAssoCode", dr);
                enPartialProviderDemographics.ProvDemographicsSrcKey = ProviderManagementHelper.GetString("ProvDemographicsSrcKey", dr);
            }
            return enPartialProviderDemographics;
        }

        public static SubmitPartialProviderInformationAddress[] fillProviderAddress(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.SubmitPartialProviderInformationAddress[] enSubmitPartialProviderInformationAddress = new PartialProviderManagementReference.SubmitPartialProviderInformationAddress[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enSubmitPartialProviderInformationAddress[i] = new PartialProviderManagementReference.SubmitPartialProviderInformationAddress();
                enSubmitPartialProviderInformationAddress[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enSubmitPartialProviderInformationAddress[i].AddressType = ProviderManagementHelper.GetString("AddressType", dr);
                enSubmitPartialProviderInformationAddress[i].AddressLine1 = ProviderManagementHelper.GetString("AddressLine1", dr);
                enSubmitPartialProviderInformationAddress[i].AddressLine2 = ProviderManagementHelper.GetString("AddressLine2", dr);
                enSubmitPartialProviderInformationAddress[i].City = ProviderManagementHelper.GetString("City", dr);
                enSubmitPartialProviderInformationAddress[i].State = ProviderManagementHelper.GetString("State", dr);
                enSubmitPartialProviderInformationAddress[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enSubmitPartialProviderInformationAddress[i].ZipCode5 = ProviderManagementHelper.GetString("ZipCode5", dr);
                enSubmitPartialProviderInformationAddress[i].ZipCode4 = ProviderManagementHelper.GetString("ZipCode4", dr);
                enSubmitPartialProviderInformationAddress[i].Country = ProviderManagementHelper.GetString("Country", dr);
                enSubmitPartialProviderInformationAddress[i].BorderStateIndicator = ProviderManagementHelper.GetString("BorderStateIndicator", dr);
                enSubmitPartialProviderInformationAddress[i].AddressNameTypeIndicator = ProviderManagementHelper.GetString("AddressNameTypeIndicator", dr);
                enSubmitPartialProviderInformationAddress[i].ProvOrgAddressName = ProviderManagementHelper.GetString("ProvOrgAddressName", dr);
                enSubmitPartialProviderInformationAddress[i].ProvOrgAddressFirstName = ProviderManagementHelper.GetString("ProvOrgAddressFirstName", dr);
                enSubmitPartialProviderInformationAddress[i].ProvOrgAddressMiddleName = ProviderManagementHelper.GetString("ProvOrgAddressMiddleName", dr);
                enSubmitPartialProviderInformationAddress[i].ProvOrgAddressLastName = ProviderManagementHelper.GetString("ProvOrgAddressLastName", dr);
                enSubmitPartialProviderInformationAddress[i].ProvOrgPhoneNumber1 = ProviderManagementHelper.GetString("ProvOrgPhoneNumber1", dr);
                enSubmitPartialProviderInformationAddress[i].ProvOrgPhoneNumberExt1 = ProviderManagementHelper.GetString("ProvOrgPhoneNumberExt1", dr);
                enSubmitPartialProviderInformationAddress[i].ProvOrgPhoneNumber2 = ProviderManagementHelper.GetString("ProvOrgPhoneNumber2", dr);
                enSubmitPartialProviderInformationAddress[i].ProvOrgPhoneNumberExt2 = ProviderManagementHelper.GetString("ProvOrgPhoneNumberExt2", dr);
                enSubmitPartialProviderInformationAddress[i].ProvOrgFaxNumber1 = ProviderManagementHelper.GetString("ProvOrgFaxNumber1", dr);
                enSubmitPartialProviderInformationAddress[i].ProvOrgFaxNumber2 = ProviderManagementHelper.GetString("ProvOrgFaxNumber2", dr);
                enSubmitPartialProviderInformationAddress[i].ProvAddressContactName = ProviderManagementHelper.GetString("ProvAddressContactName", dr);
                enSubmitPartialProviderInformationAddress[i].ProvEmail = ProviderManagementHelper.GetString("ProvEmail", dr);
                enSubmitPartialProviderInformationAddress[i].Longitude = ProviderManagementHelper.GetString("ProvAddressLongitude", dr);
                enSubmitPartialProviderInformationAddress[i].Latitude = ProviderManagementHelper.GetString("ProvAddressLatitude", dr);
                enSubmitPartialProviderInformationAddress[i].HandicapAccessibilityIndicator = ProviderManagementHelper.GetString("HandicapAccessibilityIndicator", dr);
                enSubmitPartialProviderInformationAddress[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enSubmitPartialProviderInformationAddress[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enSubmitPartialProviderInformationAddress[i].ProvAddressSrcKey = ProviderManagementHelper.GetString("ProvAddressSrcKey", dr);
            }
            return enSubmitPartialProviderInformationAddress;
        }

        public static PartialProviderTypeList[] fillProviderType(DataTable dt)
        {
            int countRow = 0;
            if (dt != null)
            {
                countRow = dt.Rows.Count;
            }
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderTypeList[] enPartialProviderTypeList = new PartialProviderManagementReference.PartialProviderTypeList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enPartialProviderTypeList[i] = new PartialProviderManagementReference.PartialProviderTypeList();
                enPartialProviderTypeList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enPartialProviderTypeList[i].ProviderTypeCode = ProviderManagementHelper.GetString("ProviderTypeCode", dr);
                enPartialProviderTypeList[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enPartialProviderTypeList[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enPartialProviderTypeList[i].ProvTypeSrcKey = ProviderManagementHelper.GetString("ProvTypeSrcKey", dr);
            }
            return enPartialProviderTypeList;
        }

        public static PartialTaxonomyClassificationList[] fillTaxonomyClassification(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialTaxonomyClassificationList[] enTaxonomyClassifications = new PartialProviderManagementReference.PartialTaxonomyClassificationList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enTaxonomyClassifications[i] = new PartialProviderManagementReference.PartialTaxonomyClassificationList();
                enTaxonomyClassifications[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enTaxonomyClassifications[i].ClassificationTypeCode = ProviderManagementHelper.GetString("ClassificationTypeCode", dr);
                enTaxonomyClassifications[i].ClassificationCode = ProviderManagementHelper.GetString("ClassificationCode", dr);
                enTaxonomyClassifications[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enTaxonomyClassifications[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enTaxonomyClassifications[i].ProvTxnmyClsfctnSrcKey = ProviderManagementHelper.GetString("ProvTxnmyClsfctnSrcKey", dr);
            }
            return enTaxonomyClassifications;
        }







        public static PartialOwnerRelationshipList[] fillOwnerRelationshipLists(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialOwnerRelationshipList[] enPartialOwnerRelationshipList = new PartialProviderManagementReference.PartialOwnerRelationshipList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enPartialOwnerRelationshipList[i] = new PartialProviderManagementReference.PartialOwnerRelationshipList();
                enPartialOwnerRelationshipList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enPartialOwnerRelationshipList[i].Item = ProviderManagementHelper.GetString("Owner1SSN", dr);
                enPartialOwnerRelationshipList[i].ItemElementName = ItemChoiceType.Owner1SSN;
                enPartialOwnerRelationshipList[i].Item1 = ProviderManagementHelper.GetString("Owner2SSN", dr);
                enPartialOwnerRelationshipList[i].Item1ElementName = Item1ChoiceType.Owner2SSN;
                enPartialOwnerRelationshipList[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enPartialOwnerRelationshipList[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enPartialOwnerRelationshipList[i].RelationType = ProviderManagementHelper.GetString("RelationType", dr);
                enPartialOwnerRelationshipList[i].OwnrRltnshpSrcKey = ProviderManagementHelper.GetString("OwnrRltnshpSrcKey", dr);

            }
            return enPartialOwnerRelationshipList;
        }

        public static PartialProviderAffiliationList[] fillAffiliationList(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderAffiliationList[] enPartialProviderAffiliationList = new PartialProviderManagementReference.PartialProviderAffiliationList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enPartialProviderAffiliationList[i] = new PartialProviderManagementReference.PartialProviderAffiliationList();
                enPartialProviderAffiliationList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enPartialProviderAffiliationList[i].ProvAffiliationSrcKey = ProviderManagementHelper.GetString("ProviderId", dr);
                enPartialProviderAffiliationList[i].NPI = ProviderManagementHelper.GetString("NPI", dr);
                enPartialProviderAffiliationList[i].ProviderAffiliationTypeCode = ProviderManagementHelper.GetString("ProviderAffiliationTypeCode", dr);
                enPartialProviderAffiliationList[i].AdditionalInfo1 = ProviderManagementHelper.GetString("AdditionalInfo1", dr);
                enPartialProviderAffiliationList[i].AdditionalInfo2 = ProviderManagementHelper.GetString("AdditionalInfo2", dr);
                enPartialProviderAffiliationList[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enPartialProviderAffiliationList[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enPartialProviderAffiliationList[i].ProvAffiliationSrcKey = ProviderManagementHelper.GetString("ProvAffiliationSrcKey", dr);
            }
            return enPartialProviderAffiliationList;
        }

        public static PartialProviderAlternateIdList[] fillAlternateIdList(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderAlternateIdList[] enPartialProviderAlternateIdList = new PartialProviderManagementReference.PartialProviderAlternateIdList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enPartialProviderAlternateIdList[i] = new PartialProviderManagementReference.PartialProviderAlternateIdList();
                enPartialProviderAlternateIdList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enPartialProviderAlternateIdList[i].AlternateIdType = ProviderManagementHelper.GetString("AlternateIdType", dr);
                enPartialProviderAlternateIdList[i].AlternateId = ProviderManagementHelper.GetString("AlternateId", dr);
                enPartialProviderAlternateIdList[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enPartialProviderAlternateIdList[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enPartialProviderAlternateIdList[i].ProvAlternateIdSrcKey = ProviderManagementHelper.GetString("ProvAlternateIdSrcKey", dr);
            }
            return enPartialProviderAlternateIdList;
        }

        public static PartialEFTEnrollmentList[] fillEFTEnrollmentList(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialEFTEnrollmentList[] enEFTEnrollmentList = new PartialProviderManagementReference.PartialEFTEnrollmentList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEFTEnrollmentList[i] = new PartialProviderManagementReference.PartialEFTEnrollmentList();
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

        public static PartialProviderLanguageList[] fillLanguageList(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderLanguageList[] enLanguageList = new PartialProviderManagementReference.PartialProviderLanguageList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                string lang = "";
                dr = dt.Rows[i];
                enLanguageList[i] = new PartialProviderManagementReference.PartialProviderLanguageList();
                enLanguageList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enLanguageList[i].Language = ProviderManagementHelper.GetString("LanguageCode", dr);

                //Fixed issue OHPNM-10294
                //if (i > 0)
                //{
                //    for (int j = 0; j < (i - 1); j++)
                //    {
                //        if (!string.IsNullOrEmpty(enLanguageList[j].Language.ToString()))
                //        {
                //            lang = "Yes";
                //            break;
                //        }
                //    }
                //}
                //if (lang == "")
                //{
                //    enLanguageList[i].Language = ProviderManagementHelper.GetString("LanguageCode", dr);
                //}
                enLanguageList[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enLanguageList[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enLanguageList[i].Provlngsrccode = ProviderManagementHelper.GetString("Provlngsrccode", dr);
            }
            return enLanguageList;
        }

        public static PartialProviderApplication[] fillenrollProviderApplication(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderApplication[] enPartialProviderApplication = new PartialProviderManagementReference.PartialProviderApplication[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enPartialProviderApplication[i] = new PartialProviderManagementReference.PartialProviderApplication();
                enPartialProviderApplication[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enPartialProviderApplication[i].ApplicationId = ProviderManagementHelper.GetString("ApplicationId", dr);
                enPartialProviderApplication[i].AdditionalUserID = ProviderManagementHelper.GetString("AdditionalApplicationId", dr);
                enPartialProviderApplication[i].ApplicationCreatedDate = ProviderManagementHelper.GetStringDateTime("ApplicationCreatedDate", dr);
                enPartialProviderApplication[i].ApprovalDate = ProviderManagementHelper.GetStringDateTime("ApprovalDate", dr);
                enPartialProviderApplication[i].StatePlanEnrollCode = ProviderManagementHelper.GetString("StatePlanEnrollCode", dr);
                enPartialProviderApplication[i].EnrollMethodCode = ProviderManagementHelper.GetString("EnrollMethodCode", dr);
                enPartialProviderApplication[i].ProvEnrollStatusCode = ProviderManagementHelper.GetString("ProvEnrollStatusCode", dr);
                enPartialProviderApplication[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enPartialProviderApplication[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enPartialProviderApplication[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enPartialProviderApplication[i].NonMedicaidApplicationProviderType = ProviderManagementHelper.GetString("Non_MedicaidApplicationProviderType", dr);
                enPartialProviderApplication[i].AdditionalApplicationId = ProviderManagementHelper.GetString("AdditionalApplicationId", dr);
                enPartialProviderApplication[i].ApplicationType = ProviderManagementHelper.GetString("ApplicationType", dr);
                enPartialProviderApplication[i].ApplicationStatus = ProviderManagementHelper.GetString("ApplicationStatus", dr);
                enPartialProviderApplication[i].ApplicationLegalStatus = ProviderManagementHelper.GetString("ApplicationLegalStatus", dr);
                enPartialProviderApplication[i].ApplicationFeeConfirmation = ProviderManagementHelper.GetString("ApplicationFeeConfirmation", dr);
                enPartialProviderApplication[i].SingleSignOnAssignedID = ProviderManagementHelper.GetString("SingleSignOnAssignedID", dr);
                enPartialProviderApplication[i].SingleSignOnSelectedID = ProviderManagementHelper.GetString("SingleSignOnSelectedID", dr);
                enPartialProviderApplication[i].AdditionalUserID = ProviderManagementHelper.GetString("AdditionalUserID", dr);
                enPartialProviderApplication[i].SingleSignOnRole = ProviderManagementHelper.GetString("SingleSignOnRole", dr);
                enPartialProviderApplication[i].EmailAddress = ProviderManagementHelper.GetString("EmailAddress", dr);
                enPartialProviderApplication[i].ProvApplicationSrcKey = ProviderManagementHelper.GetString("ProvApplicationSrcKey", dr);
            }
            return enPartialProviderApplication;
        }

        public static PartialProviderAttestationsList[] fillProviderAttestationsListAttestations(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderAttestationsList[] enProviderAttestationsListAttestations = new PartialProviderManagementReference.PartialProviderAttestationsList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderAttestationsListAttestations[i] = new PartialProviderManagementReference.PartialProviderAttestationsList();
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
        public static PartialProviderBusinessStatusList[] fillProviderBusinessStatusListBusinessStatus(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderBusinessStatusList[] enProviderBusinessStatusListBusinessStatus = new PartialProviderManagementReference.PartialProviderBusinessStatusList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderBusinessStatusListBusinessStatus[i] = new PartialProviderManagementReference.PartialProviderBusinessStatusList();
                enProviderBusinessStatusListBusinessStatus[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderBusinessStatusListBusinessStatus[i].BusinessStatusCode = ProviderManagementHelper.GetString("BusinessStatusCode", dr);
                enProviderBusinessStatusListBusinessStatus[i].EnrollmentBusinessStatusReasonCode = ProviderManagementHelper.GetString("EnrollmentBusinessStatusReasonCode", dr);
                enProviderBusinessStatusListBusinessStatus[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderBusinessStatusListBusinessStatus[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderBusinessStatusListBusinessStatus[i].ProvBsnsStatusSrcKey = ProviderManagementHelper.GetString("ProvBsnsStatusSrcKey", dr);
            }
            return enProviderBusinessStatusListBusinessStatus;
        }
        public static PartialProviderCHOPList[] fillProviderCHOPListCHOP(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderCHOPList[] enProviderCHOPListCHOP = new PartialProviderManagementReference.PartialProviderCHOPList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderCHOPListCHOP[i] = new PartialProviderManagementReference.PartialProviderCHOPList();
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
        public static SubmitPartialProviderInformationContact[] fillProviderContactListContact(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.SubmitPartialProviderInformationContact[] enPartialProviderContactList = new PartialProviderManagementReference.SubmitPartialProviderInformationContact[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enPartialProviderContactList[i] = new PartialProviderManagementReference.SubmitPartialProviderInformationContact();
                enPartialProviderContactList[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enPartialProviderContactList[i].ContactType = ProviderManagementHelper.GetString("ContactType", dr);
                enPartialProviderContactList[i].ContactDetail = ProviderManagementHelper.GetString("ContactDetail", dr);
                enPartialProviderContactList[i].ContactInstruction = ProviderManagementHelper.GetString("ContactInstruction", dr);
                enPartialProviderContactList[i].PreferredContactIndicator = ProviderManagementHelper.GetString("PreferredContactIndicator", dr);
                //enPartialProviderContactList[i].ProvContactSrcKey = ProviderManagementHelper.GetString("ProvContactSrcKey", dr);
            }
            return enPartialProviderContactList;
        }


        public static PartialProviderManagedEmployeesListProviderManagedEmployee[] fillProviderManagedEmployeesListManagedEmployee(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderManagedEmployeesListProviderManagedEmployee[] enProviderManagedEmployeesListManagedEmployee = new PartialProviderManagementReference.PartialProviderManagedEmployeesListProviderManagedEmployee[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderManagedEmployeesListManagedEmployee[i] = new PartialProviderManagementReference.PartialProviderManagedEmployeesListProviderManagedEmployee();
                enProviderManagedEmployeesListManagedEmployee[i].ManagedEmployee = fillProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee(dr);
                enProviderManagedEmployeesListManagedEmployee[i].ManagedEmployeeAddress = fillProviderManagedEmployeesListProviderManagedEmployeeAddress(dr);
            }
            return enProviderManagedEmployeesListManagedEmployee;
        }

        public static PartialProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee fillProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee(DataRow dr)
        {
            //int countRow = dt.Rows.Count;
            //DataRow dr = null;
            PartialProviderManagementReference.PartialProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee = new PartialProviderManagementReference.PartialProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee();
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee = new PartialProviderManagementReference.PartialProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee();
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.FirstName = ProviderManagementHelper.GetString("FirstName", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.MiddleName = ProviderManagementHelper.GetString("MiddleName", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.LastName = ProviderManagementHelper.GetString("LastName", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.ManageEmployeesType = ProviderManagementHelper.GetString("ManageEmployeesType", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.OwnerAffiliationType = ProviderManagementHelper.GetString("OwnerAffiliationType", dr);
            enProviderManagedEmployeesListProviderManagedEmployeeManagedEmployee.ManageEmployeesInd = ProviderManagementHelper.GetString("ManageEmployeesInd", dr); ;
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

        public static PartialProviderManagedEmployeesListProviderManagedEmployeeAddress[] fillProviderManagedEmployeesListProviderManagedEmployeeAddress(DataRow dr)
        {
            int countRow = 1;
            if (string.IsNullOrEmpty(ProviderManagementHelper.GetString("AddressLine1", dr).Trim()))
            {
                dr = null;
                countRow = 0;
            }
            PartialProviderManagementReference.PartialProviderManagedEmployeesListProviderManagedEmployeeAddress[] enProviderManagedEmployeesListProviderManagedEmployeeAddress = new PartialProviderManagementReference.PartialProviderManagedEmployeesListProviderManagedEmployeeAddress[countRow];
            for (int i = 0; i < countRow; i++)
            {
                enProviderManagedEmployeesListProviderManagedEmployeeAddress[i] = new PartialProviderManagementReference.PartialProviderManagedEmployeesListProviderManagedEmployeeAddress();
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

        public static PartialProviderOwnershipList[] fillProviderOwnershipList(DataTable dt)
        {

            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderOwnershipList[] enPartialProviderOwnershipList = new PartialProviderManagementReference.PartialProviderOwnershipList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enPartialProviderOwnershipList[i] = new PartialProviderManagementReference.PartialProviderOwnershipList();
                enPartialProviderOwnershipList[i].OwnerAddress = fillProviderOwnershipListAddress(dr);
                enPartialProviderOwnershipList[i].OwnershipDetails = fillProviderOwnershipListOwnershipDetails(dr);
            }
            return enPartialProviderOwnershipList;
        }

        public static PartialProviderOwnershipListAddress[] fillProviderOwnershipListAddress(DataRow dr)
        {
            int countRow = 1;
            if (string.IsNullOrEmpty(ProviderManagementHelper.GetString("AddressLine1", dr).Trim()))
            {
                dr = null;
                countRow = 0;
            }
            PartialProviderManagementReference.PartialProviderOwnershipListAddress[] enPartialOwnerAddress = new PartialProviderManagementReference.PartialProviderOwnershipListAddress[countRow];
            for (int i = 0; i < countRow; i++)
            {
                enPartialOwnerAddress[i] = new PartialProviderManagementReference.PartialProviderOwnershipListAddress();
                enPartialOwnerAddress[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enPartialOwnerAddress[i].AddressType = ProviderManagementHelper.GetString("AddressType", dr);
                enPartialOwnerAddress[i].AddressLine1 = ProviderManagementHelper.GetString("AddressLine1", dr);
                enPartialOwnerAddress[i].AddressLine2 = ProviderManagementHelper.GetString("AddressLine2", dr);
                enPartialOwnerAddress[i].City = ProviderManagementHelper.GetString("City", dr);
                enPartialOwnerAddress[i].State = ProviderManagementHelper.GetString("State", dr);
                enPartialOwnerAddress[i].CountyCode = ProviderManagementHelper.GetString("CountyCode", dr);
                enPartialOwnerAddress[i].ZipCode4 = ProviderManagementHelper.GetString("ZipCode4", dr);
                enPartialOwnerAddress[i].ZipCode5 = ProviderManagementHelper.GetString("ZipCode5", dr);
                enPartialOwnerAddress[i].Country = ProviderManagementHelper.GetString("Country", dr);
                enPartialOwnerAddress[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("AddrEffectiveDate", dr);
                enPartialOwnerAddress[i].EndDate = ProviderManagementHelper.GetStringDateTime("AddrEndDate", dr);
                //enProviderOwnershipListAddress[i].ProvOwnrAdrsSrcKey = ProviderManagementHelper.GetString("ProvOwnrAdrsSrcKey", dr);
            }
            return enPartialOwnerAddress;
        }

        public static PartialProviderOwnershipListOwnershipDetails fillProviderOwnershipListOwnershipDetails(DataRow dr)
        {
            int countRow = 1;
            PartialProviderManagementReference.PartialProviderOwnershipListOwnershipDetails enPartialProviderOwnershipListOwnershipDetails = new PartialProviderManagementReference.PartialProviderOwnershipListOwnershipDetails();
            for (int i = 0; i < countRow; i++)
            {
                enPartialProviderOwnershipListOwnershipDetails.OwnerType = ProviderManagementHelper.GetString("OwnerType", dr);
                //enPartialProviderOwnershipListOwnershipDetails.OwnerAffiliationType = ProviderManagementHelper.GetString("OwnerAffiliationType", dr);
                enPartialProviderOwnershipListOwnershipDetails.OwnPercentage = ProviderManagementHelper.GetDecimalString("OwnPercentage", dr);
                enPartialProviderOwnershipListOwnershipDetails.SanctionInd = ProviderManagementHelper.GetString("SanctionInd", dr);
                enPartialProviderOwnershipListOwnershipDetails.SanctionExplain = ProviderManagementHelper.GetString("SanctionExplain", dr);
                enPartialProviderOwnershipListOwnershipDetails.RelationshipInd = ProviderManagementHelper.GetString("RelationshipInd", dr);
                enPartialProviderOwnershipListOwnershipDetails.CriminalOffenseInd = ProviderManagementHelper.GetString("CriminalOffenseInd", dr);
                enPartialProviderOwnershipListOwnershipDetails.CriminalOffenseExplain = ProviderManagementHelper.GetString("CriminalOffenseExplain", dr);

                enPartialProviderOwnershipListOwnershipDetails.LicenseSuspendInd = ProviderManagementHelper.GetString("LicenseSuspendInd", dr);
                enPartialProviderOwnershipListOwnershipDetails.LicenseSuspendExplain = ProviderManagementHelper.GetString("LicenseSuspendExplain", dr);
                enPartialProviderOwnershipListOwnershipDetails.PenaltyInd = ProviderManagementHelper.GetString("PenaltyInd", dr);
                enPartialProviderOwnershipListOwnershipDetails.PenaltyExplain = ProviderManagementHelper.GetString("PenaltyExplain", dr);
                enPartialProviderOwnershipListOwnershipDetails.CntrlIntrstOthrProvEntityInd = ProviderManagementHelper.GetString("CntrlIntrstOthrProvEntityInd", dr);
                enPartialProviderOwnershipListOwnershipDetails.CntrlIntrstOthrProvEntityExplain = ProviderManagementHelper.GetString("CntrlIntrstOthrProvEntityExplain", dr);
                enPartialProviderOwnershipListOwnershipDetails.OwnrSubcntrctrTrnxInd = ProviderManagementHelper.GetString("OwnrSubcntrctrTrnxInd", dr);
                enPartialProviderOwnershipListOwnershipDetails.OwnrSubcntrctrDetails = ProviderManagementHelper.GetString("OwnrSubcntrctrDetails", dr);
                enPartialProviderOwnershipListOwnershipDetails.OwnrSubcntrctrBsnssTrnxInd = ProviderManagementHelper.GetString("OwnrSubcntrctrBsnssTrnxInd", dr);
                enPartialProviderOwnershipListOwnershipDetails.OwnrSubcntrctrBsnssTrnxDetails = ProviderManagementHelper.GetString("OwnrSubcntrctrBsnssTrnxDetails", dr);
                enPartialProviderOwnershipListOwnershipDetails.TransactionAmount = ProviderManagementHelper.GetString("TransactionAmount", dr);
                enPartialProviderOwnershipListOwnershipDetails.TransactionDate = ProviderManagementHelper.GetStringDateTime("TransactionDate", dr);
                enPartialProviderOwnershipListOwnershipDetails.OwnerOutOfStateResidentIndicator = ProviderManagementHelper.GetString("OwnerOutOfStateResidentIndicator", dr);
                enPartialProviderOwnershipListOwnershipDetails.OwnerOutOfStateResidentExplain = ProviderManagementHelper.GetString("OwnerOutOfStateResidentExplain", dr);
                enPartialProviderOwnershipListOwnershipDetails.OwnerStateFederalOffenseInd = ProviderManagementHelper.GetString("OwnerStateFederalOffenseInd", dr);
                enPartialProviderOwnershipListOwnershipDetails.OwnerStateFederalOffenseExplain = ProviderManagementHelper.GetString("OwnerStateFederalOffenseExplain", dr);

                enPartialProviderOwnershipListOwnershipDetails.FingerprintStatusIndicator = ProviderManagementHelper.GetString("FingerprintStatusIndicator", dr);
                enPartialProviderOwnershipListOwnershipDetails.RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enPartialProviderOwnershipListOwnershipDetails.EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enPartialProviderOwnershipListOwnershipDetails.EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);

                //OHPNM -14007
                if (ProviderManagementHelper.GetString("OwnerType", dr).Equals("I")
                    || ProviderManagementHelper.GetString("OwnerType", dr).Equals("RI")
                    || ProviderManagementHelper.GetString("OwnerType", dr).Equals("SI")
                    || ProviderManagementHelper.GetString("OwnerType", dr).Equals("SUI")
                    )
                {
                    PartialProviderManagementReference.PartialOwnerOwnerIndividual enooIndiv = new PartialProviderManagementReference.PartialOwnerOwnerIndividual();
                    enooIndiv.FirstName = ProviderManagementHelper.GetString("FirstName", dr);
                    enooIndiv.MiddleName = ProviderManagementHelper.GetString("MiddleName", dr);
                    enooIndiv.LastName = ProviderManagementHelper.GetString("LastName", dr);
                    enooIndiv.DateOfBirth = ProviderManagementHelper.GetStringDateTime("DateOfBirth", dr);
                    enooIndiv.SSN = ProviderManagementHelper.GetString("SSN", dr);
                    enooIndiv.Title = ProviderManagementHelper.GetString("Title", dr);
                    enooIndiv.OwnerIndvSrcKey = ProviderManagementHelper.GetString("OwnerIndvSrcKey", dr);
                    enPartialProviderOwnershipListOwnershipDetails.Item = enooIndiv;
                }
                else
                {
                    PartialProviderManagementReference.PartialOwnerOwnerOrganization enooOrg = new PartialProviderManagementReference.PartialOwnerOwnerOrganization();
                    enooOrg.OwnerOrgName = ProviderManagementHelper.GetString("OwnerOrgName", dr);
                    enooOrg.OrgTaxIdNumber = ProviderManagementHelper.GetString("OrgTaxIdNumber", dr);
                    enooOrg.Title = ProviderManagementHelper.GetString("Title", dr);
                    enooOrg.OwnerOrgSrcKey = ProviderManagementHelper.GetString("OwnerOrgSrcKey", dr);
                    enPartialProviderOwnershipListOwnershipDetails.Item = enooOrg;
                }

            }
            return enPartialProviderOwnershipListOwnershipDetails;
        }

        public static PartialProgramAffiliationsList[] fillProgramAffiliationsListProgramAffiliation(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProgramAffiliationsList[] enProgramAffiliationsListProgramAffiliation = new PartialProviderManagementReference.PartialProgramAffiliationsList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProgramAffiliationsListProgramAffiliation[i] = new PartialProviderManagementReference.PartialProgramAffiliationsList();
                enProgramAffiliationsListProgramAffiliation[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProgramAffiliationsListProgramAffiliation[i].AffiliatedProgramCode = ProviderManagementHelper.GetString("AffiliatedProgramCode", dr);
                enProgramAffiliationsListProgramAffiliation[i].API = ProviderManagementHelper.GetString("API", dr);
                enProgramAffiliationsListProgramAffiliation[i].AffiliatedProgramId = ProviderManagementHelper.GetString("AffiliatedProgramId", dr);
                enProgramAffiliationsListProgramAffiliation[i].ProgramAffiationStatusCode = ProviderManagementHelper.GetString("ProgramAffiationStatusCode", dr);
                enProgramAffiliationsListProgramAffiliation[i].ProgramAffiliationStatusReason = ProviderManagementHelper.GetString("ProgramAffiliationStatusReason", dr);
                enProgramAffiliationsListProgramAffiliation[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProgramAffiliationsListProgramAffiliation[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProgramAffiliationsListProgramAffiliation[i].PgmAffltnSrcKey = ProviderManagementHelper.GetString("PgmAffltnSrcKey", dr);
            }
            return enProgramAffiliationsListProgramAffiliation;
        }
        public static PartialProviderReviewsList[] fillProviderReviewsListReview(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderReviewsList[] enProviderReviewsListReview = new PartialProviderManagementReference.PartialProviderReviewsList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderReviewsListReview[i] = new PartialProviderManagementReference.PartialProviderReviewsList();
                enProviderReviewsListReview[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderReviewsListReview[i].ProviderReviewTypeCode = ProviderManagementHelper.GetString("ProviderReviewTypeCode", dr);
                enProviderReviewsListReview[i].ProposedEffectiveDate = ProviderManagementHelper.GetStringDateTime("ProposedEffectiveDate", dr);
                enProviderReviewsListReview[i].ProviderReviewID = ProviderManagementHelper.GetString("ProviderReviewID", dr);
                enProviderReviewsListReview[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderReviewsListReview[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderReviewsListReview[i].Provrvwsrccode = ProviderManagementHelper.GetString("Provrvwsrccode", dr);
            }
            return enProviderReviewsListReview;
        }
        public static PartialProviderServiceLocation[] fillEnrollProviderServiceLocation(DataSet ds)
        {
            DataTable dt = ds.Tables[17];
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderServiceLocation[] enEnrollProviderServiceLocation = new PartialProviderManagementReference.PartialProviderServiceLocation[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderServiceLocation[i] = new PartialProviderManagementReference.PartialProviderServiceLocation();
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



        public static PartialProviderServiceLocationContact[] fillEnrollProviderServiceLocationContact(DataTable dt)
        {
            int countRow = 0;
            if (dt != null)
            {
                countRow = dt.Rows.Count;
            }

            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderServiceLocationContact[] enEnrollProviderServiceLocationContact = new PartialProviderManagementReference.PartialProviderServiceLocationContact[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderServiceLocationContact[i] = new PartialProviderManagementReference.PartialProviderServiceLocationContact();
                enEnrollProviderServiceLocationContact[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enEnrollProviderServiceLocationContact[i].ContactType = ProviderManagementHelper.GetString("ContactType", dr);
                enEnrollProviderServiceLocationContact[i].ContactDetail = ProviderManagementHelper.GetString("ContactDetail", dr);
                enEnrollProviderServiceLocationContact[i].ContactInstruction = ProviderManagementHelper.GetString("ContactInstruction", dr);
                enEnrollProviderServiceLocationContact[i].PreferredContactIndicator = ProviderManagementHelper.GetString("PreferredContactIndicator", dr);
                enEnrollProviderServiceLocationContact[i].SvcLctnCntctSrcKey = ProviderManagementHelper.GetString("SvcLctnCntctSrcKey", dr);
            }
            return enEnrollProviderServiceLocationContact;
        }
        public static PartialProviderServiceLocationLicense[] fillEnrollProviderServiceLocationLicense(DataSet ds)
        {
            DataTable dt = ds.Tables[23];
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            int StgProviderLicenseId = 0;

            PartialProviderManagementReference.PartialProviderServiceLocationLicense[] enEnrollProviderServiceLocationLicense = new PartialProviderManagementReference.PartialProviderServiceLocationLicense[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                StgProviderLicenseId = ProviderManagementHelper.GetInt("StgProviderLicenseId", dr);
                enEnrollProviderServiceLocationLicense[i] = new PartialProviderManagementReference.PartialProviderServiceLocationLicense();
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
        public static PartialLicenseCertificationsList[] fillLicenseCertificationsListCertifications(DataSet ds, int StgProviderLicenseId)
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
            PartialProviderManagementReference.PartialLicenseCertificationsList[] enLicenseCertificationsListCertifications = new PartialProviderManagementReference.PartialLicenseCertificationsList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enLicenseCertificationsListCertifications[i] = new PartialProviderManagementReference.PartialLicenseCertificationsList();
                enLicenseCertificationsListCertifications[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enLicenseCertificationsListCertifications[i].CertificationId = ProviderManagementHelper.GetString("CertificationId", dr);
                enLicenseCertificationsListCertifications[i].CertifyingOrg = ProviderManagementHelper.GetString("CertifyingOrg", dr);
                enLicenseCertificationsListCertifications[i].CertificationFocus = ProviderManagementHelper.GetString("CertificationFocus", dr);
                enLicenseCertificationsListCertifications[i].CertificationSpeciality = ProviderManagementHelper.GetString("CertificationSpeciality", dr);
                enLicenseCertificationsListCertifications[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enLicenseCertificationsListCertifications[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enLicenseCertificationsListCertifications[i].ProvLicenseCertSrcKey = ProviderManagementHelper.GetString("ProvLicenseCertSrcKey", dr);
            }
            return enLicenseCertificationsListCertifications;
        }
        public static PartialProviderCertificationList[] fillProviderCertificationListCertification(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderCertificationList[] enProviderCertificationListCertification = new PartialProviderManagementReference.PartialProviderCertificationList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderCertificationListCertification[i] = new PartialProviderManagementReference.PartialProviderCertificationList();
                enProviderCertificationListCertification[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderCertificationListCertification[i].CertificateNumber = ProviderManagementHelper.GetString("CertificateNumber", dr);
                enProviderCertificationListCertification[i].CertificateType = ProviderManagementHelper.GetString("CertificateType", dr);
                enProviderCertificationListCertification[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderCertificationListCertification[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderCertificationListCertification[i].ProvCrtfctnSrcKey = ProviderManagementHelper.GetString("ProvCrtfctnSrcKey", dr);
            }
            return enProviderCertificationListCertification;
        }
        public static PartialProviderIdentifierList[] fillProviderIdentifierListIdentifier(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderIdentifierList[] enProviderIdentifierListIdentifier = new PartialProviderManagementReference.PartialProviderIdentifierList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderIdentifierListIdentifier[i] = new PartialProviderManagementReference.PartialProviderIdentifierList();
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
        public static PartialProviderFacilityList[] fillProviderFacilityListFacility(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderFacilityList[] enProviderFacilityListFacility = new PartialProviderManagementReference.PartialProviderFacilityList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderFacilityListFacility[i] = new PartialProviderManagementReference.PartialProviderFacilityList();
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
        public static PartialProviderTaxonomyList[] fillProviderTaxonomyListTaxonomy(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderTaxonomyList[] enProviderTaxonomyListTaxonomy = new PartialProviderManagementReference.PartialProviderTaxonomyList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderTaxonomyListTaxonomy[i] = new PartialProviderManagementReference.PartialProviderTaxonomyList();
                enProviderTaxonomyListTaxonomy[i].RecordStatusCode = ProviderManagementHelper.GetString("RecordStatusCode", dr);
                enProviderTaxonomyListTaxonomy[i].TaxonomyCode = ProviderManagementHelper.GetString("TaxonomyCode", dr);
                enProviderTaxonomyListTaxonomy[i].PrimaryTaxonomyIndicator = ProviderManagementHelper.GetString("PrimaryTaxonomyIndicator", dr);
                enProviderTaxonomyListTaxonomy[i].EffectiveDate = ProviderManagementHelper.GetStringDateTime("EffectiveDate", dr);
                enProviderTaxonomyListTaxonomy[i].EndDate = ProviderManagementHelper.GetStringDateTime("EndDate", dr);
                enProviderTaxonomyListTaxonomy[i].ProvTxnmySrcKey = ProviderManagementHelper.GetString("ProvTxnmySrcKey", dr);
            }
            return enProviderTaxonomyListTaxonomy;
        }
        public static PartialProviderSpecialtyList[] fillProviderSpecialtyListSpecialty(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderSpecialtyList[] enProviderSpecialtyListSpecialty = new PartialProviderManagementReference.PartialProviderSpecialtyList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderSpecialtyListSpecialty[i] = new PartialProviderManagementReference.PartialProviderSpecialtyList();
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
        public static PartialProviderRestrictionList[] fillEnrollProviderRestrictionListRestriction(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderRestrictionList[] enEnrollProviderRestrictionListRestriction = new PartialProviderManagementReference.PartialProviderRestrictionList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enEnrollProviderRestrictionListRestriction[i] = new PartialProviderManagementReference.PartialProviderRestrictionList();
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






        public static PartialProviderServiceLocationAddress[] fillEnrollProviderServiceLocationAddress(DataRow dr)
        {
            int countRow = 0;
            if (dr != null)
            {
                countRow = 1;
            }

            PartialProviderManagementReference.PartialProviderServiceLocationAddress[] enEnrollProviderServiceLocationAddress = new PartialProviderManagementReference.PartialProviderServiceLocationAddress[countRow];
            for (int i = 0; i < countRow; i++)
            {
                //dr = dt.Rows[i];
                enEnrollProviderServiceLocationAddress[i] = new PartialProviderManagementReference.PartialProviderServiceLocationAddress();
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

        public static PartialVisitList[] fillVisitListVisit(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialVisitList[] enVisitListVisit = new PartialProviderManagementReference.PartialVisitList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enVisitListVisit[i] = new PartialProviderManagementReference.PartialVisitList();
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

        public static PartialProviderTrainingList[] fillProviderTrainingListTraining(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderTrainingList[] enProviderTrainingListTraining = new PartialProviderManagementReference.PartialProviderTrainingList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderTrainingListTraining[i] = new PartialProviderManagementReference.PartialProviderTrainingList();
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

        public static PartialProviderServicesList[] fillProviderServicesListServices(DataTable dt)
        {
            int countRow = dt.Rows.Count;
            DataRow dr = null;
            PartialProviderManagementReference.PartialProviderServicesList[] enProviderServicesListServices = new PartialProviderManagementReference.PartialProviderServicesList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                enProviderServicesListServices[i] = new PartialProviderManagementReference.PartialProviderServicesList();
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