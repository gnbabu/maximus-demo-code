using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Services.PDMS;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mime;
using System.ServiceModel;
using System.Web.UI.WebControls;

namespace MAXIMUS.Controllers.PDMS
{
    public static class LookupTableController
    {
        public static DataSet GetAppSetting(string key)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("key", DbType.String, key, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("sp_AppSettingsGetValue", parameters, "AppSettingResults");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectReferTocomplianceReasons()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectReferToComplianceReasons");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetEnrollStatusReasons()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_Enroll_Reasons");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetActivityTypes()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_ProviderFinancialActivityTypes");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetCertificationTypes()      //akash
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_CertificationTypes");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectRDMCodeSets()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectRDM_INCOMING_CODESET_TABLE_LIST", "RDMCodesets");

                lookup.Tables[0].TableName = "RDMCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectRDMCodeSetsByName(string codeSetName)
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_ReviewAndUpdateRDMCodeSets", parameters, "RDMCodesets");

                lookup.Tables[0].TableName = "RDMCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        #region WPC Codeset methods
        public static DataSet SelectWPCCodeSets()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_STG_RFD_CODESET_TABLE_LIST", "WPCCodesets");

                lookup.Tables[0].TableName = "WPCCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectWPCCodeSetsByName(string codeSetName)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_ReviewAndUpdate_WPC_CodeSets", parameters, "WPCCodesets");

                lookup.Tables[0].TableName = "WPCCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        #endregion

        #region Context4 Codeset methods
        public static DataSet SelectC4CodeSets()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_STG_C4_RFD_CODESET_TABLE_LIST", "C4Codesets");

                lookup.Tables[0].TableName = "C4CodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectC4CodeSetsByName(string codeSetName)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_ReviewAndUpdate_C4_CodeSets", parameters, "C4Codesets");

                lookup.Tables[0].TableName = "C4CodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        #endregion

        #region CMS Codeset methods
        public static DataSet SelectCMSCodeSets()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_STG_CMS_RFD_CODESET_TABLE_LIST", "CMSCodesets");

                lookup.Tables[0].TableName = "CMSCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCMSCodeSetsByName(string codeSetName)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_ReviewAndUpdate_CMS_CodeSets", parameters, "CMSCodesets");

                lookup.Tables[0].TableName = "CMSCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        #endregion

        #region NUBC Codeset methods
        public static DataSet SelectNUBCCodeSets()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_STG_NUBC_RFD_CODESET_TABLE_LIST", "NUBCCodesets");

                lookup.Tables[0].TableName = "NUBCCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectNUBCCodeSetsByName(string codeSetName)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_ReviewAndUpdate_NUBC_CodeSets", parameters, "NUBCCodesets");

                lookup.Tables[0].TableName = "NUBCCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        #endregion

        public static DataSet SelectApplicationTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAPPLICATION_TYPE", "ApplicationTypes");

                lookup.Tables[0].TableName = "ApplicationType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectWaiverTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectWaiver_TYPE", "WaiverTypes");

                lookup.Tables[0].TableName = "WaiverType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectCAQHErrors()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectCAQHErrors");

                lookup.Tables[0].TableName = "CAQHError";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCAQHStatuses()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectCAQHStatusType");

                lookup.Tables[0].TableName = "CAQHStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectContentType(int contentTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (contentTypeID > 0) parameters.Add(SqlParms.CreateParameter("CONTENT_TYPE_ID", DbType.Int32, contentTypeID, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCONTENT_TYPE", parameters, "ContentType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMMISErrors()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectMMISErrors");

                lookup.Tables[0].TableName = "MMISError";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMMISStatuses()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectMMISStatuses");

                lookup.Tables[0].TableName = "MMISStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderTypeById(int providerTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (providerTypeId > 0) parameters.Add(SqlParms.CreateParameter("ProviderTypeId", DbType.Int32, providerTypeId, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderType", parameters, "ProviderType");

                lookup.Tables[0].TableName = "ProviderType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderTypes(int applicationTypeId, int providerCategorytypeid, string roleName)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("providerCategorytypeid", DbType.Int32, providerCategorytypeid, true));
                parameters.Add(SqlParms.CreateParameter("applicationTypeId", DbType.Int32, applicationTypeId, true));
                parameters.Add(SqlParms.CreateParameter("roleName", DbType.String, roleName, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderTypes", parameters, "ProviderType");

                lookup.Tables[0].TableName = "ProviderType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectProviderTypes");

                lookup.Tables[0].TableName = "ProviderType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetCategoryFeeScheduleTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCategoryFeeSchedule");
                lookup.Tables[0].TableName = "CategoryName";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetAffiliateFilesByUser(Guid userID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("userID", DbType.Guid, userID, true));
                return DataAccess.ExecuteStoredProcedure("usp_GetAffiliateFilesbyUserID", parameters, "Affiliates");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetBulkAgentFilesByUser(Guid userID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("userID", DbType.Guid, userID, true));
                return DataAccess.ExecuteStoredProcedure("usp_GetBulkAgentFilesbyUserID", parameters, "Affiliates");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderTypes2()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetProviderType2");

                lookup.Tables[0].TableName = "ProviderType2";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectCredentialingProviderTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCredentialProviderTypes");

                lookup.Tables[0].TableName = "CredProviderTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderTypeByProviderCategory(string CategoryID)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CategoryTypeID", DbType.String, CategoryID, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetProviderTypeByProviderCategory", parameters, "ProviderTypeByCategoryType");

                lookup.Tables[0].TableName = "ProviderTypeByCategoryType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }

        public static DataSet SelectOwnerCategoryType()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectOwnerCategoryType");

                lookup.Tables[0].TableName = "OwnerCategoryType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTypeOfCoverage()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectType_OF_Coverage");

                lookup.Tables[0].TableName = "OwnerCategoryType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectOwnershipTypes()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectOwnership_Type");
                lookup.Tables[0].TableName = "OwnershipTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectPracticeTypes()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPracticeTypes");
                lookup.Tables[0].TableName = "PracticeTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectDefendentType()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDEFENDENT_Type");

                lookup.Tables[0].TableName = "DEFENDENT_Type";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectClaimStatus()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectClaim_Status");

                lookup.Tables[0].TableName = "SelectClaimStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderPaymentType()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProvider_Payment_Type");

                lookup.Tables[0].TableName = "SelectProviderPaymentType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDMEQualificationType(int CategoryID)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CategoryTypeID", DbType.Int32, CategoryID, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDME_QUALIFICATION_TYPE", parameters, "ProductServiceCategoryType");

                lookup.Tables[0].TableName = "ProductServiceCategoryType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }

        public static DataSet SelectProductServiceSubCategoryType(int CategoryID)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CategoryTypeID", DbType.Int32, CategoryID, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDME_PRODUCT_SERVICE_SUB_CATEGORY_TYPE", parameters, "ProductServiceCategoryType");

                lookup.Tables[0].TableName = "ProductServiceCategoryType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static DataSet SelectProductServiceCategoryType()
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDME_PRODUCT_SERVICE_CATEGORY_TYPE");

                lookup.Tables[0].TableName = "ProductServiceCategoryType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static DataSet SelectCategoryOfServiceType(int ProviderTypeID)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, ProviderTypeID, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCATEGORY_OF_SERVICE_ByProviderTYPE", parameters, "CategoryOfService");

                lookup.Tables[0].TableName = "CategoryOfService";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static DataSet SelectRegistrationProviderTypesByCategory(int applicationTypeId, int categoryTypeID, int waiverTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CategoryTypeID", DbType.Int32, categoryTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeId, true));
                parameters.Add(SqlParms.CreateParameter("WaiverTypeID", DbType.Int32, waiverTypeID, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderTypesForRegistration", parameters, "ProviderType");
                lookup.Tables[0].TableName = "ProviderType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderCategories(bool includeGroupMemberProfile)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("IncludeGroupMemberProfile", DbType.Boolean, includeGroupMemberProfile, true));
                return DataAccess.ExecuteStoredProcedure("usp_GetProviderCategory", parameters, "ProviderCategory");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderCategoriesByApplication(int applicationTypeID, int waiverTypeID)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeID, false));
                parameters.Add(SqlParms.CreateParameter("WaiverTypeID", DbType.Int32, waiverTypeID, false));
                return DataAccess.ExecuteStoredProcedure("usp_GetProviderCategoryByApplicationType", parameters, "ProviderCategory");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPAAssignmentGroups()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_GetPAAssignmentTypes", "PAAssignmentTypes");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPAAssignmentProcCodes()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_GetPAAssignmentProcCodes", "PAAssignmentProcCodes");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTypeofPractice()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectTYPE_OF_PRACTICE");
                lookup.Tables[0].TableName = "TypeofPractice";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectEnrollmentStatusType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectENROLL_STATUS");
                lookup.Tables[0].TableName = "EnrollmentStatusType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTerminationStatuses()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectTerminationStatuses");
                lookup.Tables[0].TableName = "TerminationStatusType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMedicareEnrollmentStatusType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectMEDICARE_ENROLLMENT_STATUS_TYPE");
                lookup.Tables[0].TableName = "StatusType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMedicaidEnrollmentStatusType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectMEDICAID_ENROLLMENT_STATUS_TYPE");
                lookup.Tables[0].TableName = "StatusType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectTermReason()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectTERM_REASON");
                lookup.Tables[0].TableName = "TermReason";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTaxIdType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectTAX_ID_TYPE");
                lookup.Tables[0].TableName = "TaxIdType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetMaritalStatus()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectMaritalStatus");

                lookup.Tables[0].TableName = "MaritalStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetAppealStatus()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAppealStatus");

                lookup.Tables[0].TableName = "AppealStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetGovernmentType()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectGovernment_Type");

                lookup.Tables[0].TableName = "GovernmentType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPROFITType()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROFIT");

                lookup.Tables[0].TableName = "PROFIT_Type";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetPROFITStatus()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROFIT_STATUS");

                lookup.Tables[0].TableName = "PROFIT_STATUS";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static DataSet GetTYPE_OF_OWNERSHIP()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectTYPE_OF_OWNERSHIP");

                lookup.Tables[0].TableName = "TYPE_OF_OWNERSHIP";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetAccountTypeEntity()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectACCOUNT_TYPE_ENTITY");

                lookup.Tables[0].TableName = "ACCOUNT_TYPE_ENTITY";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetEFT_TYPE()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectEFT_TYPE");

                lookup.Tables[0].TableName = "EFT_TYPE";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectPDMSStatuses()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectPDMSStatusType");

                lookup.Tables[0].TableName = "PDMSStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPDMSErrors()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectPDMSErrors");

                lookup.Tables[0].TableName = "PDMSError";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectEnrollmentRejectReasons()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectEnrollmentRejectReasons");

                lookup.Tables[0].TableName = "EnrollmentRejectReason";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectErrorDispositions()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectErrorDispositions");

                lookup.Tables[0].TableName = "ErrorDisposition";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderNoteTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_NOTE_TYPE");
                lookup.Tables[0].TableName = "ProviderNoteTypes";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPermissionStatusTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPermissionStatusType");

                lookup.Tables[0].TableName = "PermissionStatusType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPermissionApplicationTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPermissionApplicationType");

                lookup.Tables[0].TableName = "PermissionApplicationType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSecurityQuestionTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectSecurityQuestions");

                lookup.Tables[0].TableName = "SecurityQuestionType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSpecialtiesByProviderType(int providerTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, true));
                return DataAccess.ExecuteStoredProcedure("usp_SelectSpecialtiesByProviderType", parameters, "SpecialtyTypes");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectGroupSpecialtiesByProviderType(int providerTypeID)
        {
            //Filters for provider types that have a mmis code on the proivder type table
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, true));
                return DataAccess.ExecuteStoredProcedure("usp_SelectGroupSpecialtiesByProviderType", parameters, "SpecialtyTypes");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectGroupSpecialtiesByProviderTypeRole(int providerTypeID, bool isInternalUser, int regid)
        {
            //Filters for provider types that have a mmis code on the proivder type table
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, true));
                parameters.Add(SqlParms.CreateParameter("IsInternalUser", DbType.Boolean, isInternalUser, true));
                parameters.Add(SqlParms.CreateParameter("reg_id", DbType.Int32, regid, true));
                return DataAccess.ExecuteStoredProcedure("usp_SelectGroupSpecialtiesByProviderType", parameters, "SpecialtyTypes");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectAllSpecialtiesByProviderType(int providerTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, true));
                return DataAccess.ExecuteStoredProcedure("usp_SelectALLSpecialtiesByProviderTypeID", parameters, "SpecialtyTypes");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSpecialtyTypes(string providerTypeID = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.String, providerTypeID, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllSpecialtyTypes", parameters, "SpecialtyTypes");

                lookup.Tables[0].TableName = "SpecialtyTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectODMCredentialingIsChecked(int REG_ID)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, REG_ID, false));
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_ODM_Credentialing_Delegates_IsChecked", parms, "REG_PROVIDER");

                lookup.Tables[0].TableName = "REG_PROVIDER";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDelegates()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllDelegates");

                lookup.Tables[0].TableName = "Delegates";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetIndvAffPageRequired(int regID)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetIndvAffPageRequired", parms, "pageRequired");
                lookup.Tables[0].TableName = "pageRequired";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderTypesWithAbbrev(string roleName)
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(SqlParms.CreateParameter("RoleName", DbType.String, roleName, false));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderTypesWithAbbrev", parms, "ProviderTypes");

                lookup.Tables[0].TableName = "ProviderTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderTypesPublicSearch()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderTypesPublicSearch", "ProviderTypesPublicSearch");

                lookup.Tables[0].TableName = "ProviderTypesPublicSearch";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTaxonomyTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllTaxonomyTypes");

                lookup.Tables[0].TableName = "TaxonomyTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTaxonomyTypesWithProviderSpecialty()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllTaxonomyTypesWithProviderSpecialty");
                lookup.Tables[0].TableName = "TaxonomyTypes";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAllSpecialtyTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllSpecialtyTypes");
                lookup.Tables[0].TableName = "SpecialtyTypes";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTaxonomyTypes(int speciltyTypeId, int providerTypeId)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE_ID", DbType.Int32, speciltyTypeId, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeId, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectTaxonomyType", parameters, "TaxonomyTypes");
                lookup.Tables[0].TableName = "TaxonomyTypes";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAllLicenseTypes(bool isUsedInMMIS)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (isUsedInMMIS) parameters.Add(SqlParms.CreateParameter("IS_USED_IN_MMIS", DbType.Boolean, isUsedInMMIS, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllLicenseTypes", parameters, "LicenseTypes");
                lookup.Tables[0].TableName = "LicenseTypes";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTaxEntityTypes()
        {

            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectTAX_ENTITY_TYPE");

                lookup.Tables[0].TableName = "TaxEntType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPracticeTypeW9()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPracticeTypeW9");

                lookup.Tables[0].TableName = "PracticeTypeW9";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAllTransactionTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllTransactionTypes");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectReportDocumentTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectReportDocumentTypes");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDocumentTypeByName(string documentName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_NAME", DbType.String, documentName, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("sp_SelectDocumentTypes", parameters, "DocumentType");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderRiskLevels()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_RISK_LEVEL");

                lookup.Tables[0].TableName = "ProviderRiskLevel";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectProviderContractType(string typeBorR)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Billing_Or_Referring", DbType.String, typeBorR, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectCONTRACT_TYPE", parameters, "ProviderContractType");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectProviderArea(string programCode)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROGRAM_CODE", DbType.Int32, programCode, true));
                DataSet lookup = lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_AREA", parameters, "ProviderArea");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectReferenceDataWithoutParam(string storedProc)
        {
            try
            {
                string storedprocname = "";
                switch (storedProc.ToLower())
                {
                    case "usp_selectspecialty_type": storedprocname = "usp_selectspecialty_type"; break;
                    case "usp_selecttype_of_coverage": storedprocname = "usp_selecttype_of_coverage"; break;
                    case "usp_selectmethodresolution": storedprocname = "usp_selectmethodresolution"; break;
                    case "usp_selectoffice_accommodations": storedprocname = "usp_selectoffice_accommodations"; break;
                    case "usp_selectlanguages_spoken": storedprocname = "usp_selectlanguages_spoken"; break;
                    case "usp_selectspecialized_training": storedprocname = "usp_selectspecialized_training"; break;
                    case "usp_selectprovidertitles": storedprocname = "usp_selectprovidertitles"; break;
                    case "usp_selectprovider_gender": storedprocname = "usp_selectprovider_gender"; break;
                    case "usp_selectcultural_competencies": storedprocname = "usp_selectcultural_competencies"; break;
                    case "usp_selectmcp": storedprocname = "usp_selectmcp"; break;
                    case "usp_selectdme_product_service_category_type": storedprocname = "usp_selectdme_product_service_category_type"; break;
                    case "usp_selectprovider_directory_radius": storedprocname = "usp_selectprovider_directory_radius"; break;
                    case "usp_selectreg_health_care_facility_affiliation": storedprocname = "usp_selectreg_health_care_facility_affiliation"; break;
                    case "usp_selectboard_certifications": storedprocname = "usp_selectboard_certifications"; break;
                    case "usp_selectfacility_type": storedprocname = "usp_selectfacility_type"; break;


                    default:
                        throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure(storedprocname, "ReferenceData_" + Guid.NewGuid().ToString());
                if (lookup.Tables.Count > 0) lookup.Tables[0].TableName = "ReferenceData";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectMCP()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectMCP");
                lookup.Tables[0].TableName = "ProviderMCP";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectProgramType()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProgramType");
                lookup.Tables[0].TableName = "ProviderProgramType";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectProviderCPCAccreditation()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_CPC_ACCREDITATION");
                lookup.Tables[0].TableName = "ProviderCPCAccreditation";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectEnrollmentStatusReasons()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectENROLL_STATUS_REASONS");
                lookup.Tables[0].TableName = "ProviderEnrollmentStatusReasons";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectReconsiderEnrollmentStatusReasons()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectReconsiderStatuses");
                lookup.Tables[0].TableName = "ProviderReconsiderEnrollmentStatusReasons";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool IsNotProcessedEligible(int regId)
        {
            try
            {
                string result = string.Empty;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@RegId", regId));
                result = DataAccess.ExecuteScalar("usp_Reg_NotProcessed_Eligible", parameters, CommandType.StoredProcedure);
                return bool.Parse(result);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectProviderContractStatus()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_CONTRACT_STATUS");
                lookup.Tables[0].TableName = "ProviderContractStatus";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectAllTaxonomyCodes()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectTAXONOMY_CODE", parameters, "ProviderTaxonomyCodes");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectBumpUpReasons()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectBump_Up_Reason");

                lookup.Tables[0].TableName = "ProviderRiskLevel";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectPageConfigurationsSectionsByPageName(string PageName)
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("PageName", PageName));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPage_ConfigurationSectionsBy_PageName", parms, "PageConfig");

                lookup.Tables[0].TableName = "PageConfig";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPageConfigurationsByPageName(string PageName)
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("PageName", PageName));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPage_ContentBy_PageName", parms, "PageConfig");

                lookup.Tables[0].TableName = "PageConfig";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPageConfigurationsByPageName(string PageName, Guid userID)
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(SqlParms.CreateParameter("PageName", DbType.String, PageName, false));
                parms.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPage_ContentBy_PageName", parms, "PageConfig");

                lookup.Tables[0].TableName = "PageConfig";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPageConfigurations()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPage_Configuration");

                lookup.Tables[0].TableName = "PageConfig";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeletePageConfiguration(int PageConfigurationID)
        {
            if (PageConfigurationID <= 0)
                return;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Page_Configuration_ID", DbType.Int32, PageConfigurationID, true));

                DataAccess.ExecuteStoredProcedure("usp_DeletePage_Configuration", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSiteVisitRecommendations()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectSITE_VISIT_RECOMMENDATIONS");

                lookup.Tables[0].TableName = "SiteVistRecommendations";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSiteVisitFindings()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectSITE_VISIT_FINDINGS");

                lookup.Tables[0].TableName = "SiteVisitFindings";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSiteVisitResults()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("[usp_SelectSITE_VISIT_RESULTS]");

                lookup.Tables[0].TableName = "SiteVistResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectApplicationFeeWaiverReasons()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAPPLICATION_FEE_WAIVER_REASON");

                lookup.Tables[0].TableName = "ApplicationFeeWaiverReason";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectApplicationFeePaymentType()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAPPLICATION_FEE_PAYMENT_TYPE");

                lookup.Tables[0].TableName = "ApplicationFeePaymentType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectApplicationFeeStatuses()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAPPLICATION_FEE_STATUS");

                lookup.Tables[0].TableName = "ApplicationFeeStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTaxonomyInfoByCode(string taxonomyCode, int providerTypeID)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TaxonomyCode", DbType.String, taxonomyCode, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectTaxonomyByCode", parameters, "TaxonomyInfo");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectTaxonomyTypeByID(int taxonomyTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TaxonomyTypeID", DbType.Int32, taxonomyTypeID, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectTaxonomyTypeByID", parameters, "TaxonomyInfo");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectReferral_UnusedByTaxID(string taxID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet ds = new DataSet();
                string outValue;
                totalResultCount = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, true));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));

                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREFERRAL_UnusedByTaxID", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectGroupAffiliationStatuses()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectGROUP_AFFILIATION_STATUS");

                lookup.Tables[0].TableName = "AffiliationStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectIndividualAffiliationPrivilegesStatus()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectINDIVIDUAL_AFFILIATION_PRIVILEGES_STATUS");

                lookup.Tables[0].TableName = "PrivilegesStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectIndividualAffiliationStaffCategoryStatus()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectINDIVIDUAL_AFFILIATION_STAFF_STATUS");

                lookup.Tables[0].TableName = "StaffCategoryStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectQuestionTypesByIDBeginsWith(string beginsWith)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("BeginsWith", DbType.String, beginsWith, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectQUESTION_TYPE_IDBeginsWith", parameters, "Questions");

                ds.Tables[0].TableName = "Questions";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectScreeningActivityStatusByActivityTypeID(int activityTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ScreeningActivityTypeID", DbType.Int32, activityTypeID, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectSCREENING_ACTIVITY_STATUSByActivityType", parameters, "ActivityStatus");

                ds.Tables[0].TableName = "ActivityStatus";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectImmigrationStatuses()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectIMMIGRATION_STATUS");

                lookup.Tables[0].TableName = "ImmigrationStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCitizenshipTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCITIZENSHIP_TYPE");

                lookup.Tables[0].TableName = "CitizenshipType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCertifiedBeds()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCERTIFIED_BEDS");

                lookup.Tables[0].TableName = "CertifiedBeds";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPaperRequestTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPAPER_REQUEST_TYPE");

                lookup.Tables[0].TableName = "PaperTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPaperDocumentTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPAPER_REQUEST_DOCUMENT_TYPE");

                lookup.Tables[0].TableName = "DocTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPaperRequestStatusTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPAPER_REQUEST_STATUS_TYPE");

                lookup.Tables[0].TableName = "StatusTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCountiesByStateAbbreviation(string stateAbbreviation)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("StateAbbreviation", DbType.String, stateAbbreviation, true));
                parameters.Add(SqlParms.CreateParameter("includeMetaData", DbType.Boolean, false, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectCOUNTY_ByStateAbbreviation", parameters, "CountyList");

                ds.Tables[0].TableName = "CountyList";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        //AP Jira OHPNM-5607 -- Hospice page
        public static DataSet SelectHospiceCountiesByStateAbbreviation(string stateAbbreviation)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("StateAbbreviation", DbType.String, stateAbbreviation, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("[usp_SelectHOSPICECOUNTY_ByStateAbbreviation]", parameters, "CountyList");

                ds.Tables[0].TableName = "CountyList";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectReports()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                DataSet ds = DataAccess.ExecuteStoredProcedure("[usp_SelectReports]", parameters, "ReportList");

                ds.Tables[0].TableName = "ReportList";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectDiddReferralLocationTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_REFERRAL_LOCATION_TYPE");

                lookup.Tables[0].TableName = "LocationTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectBackgroundStatusTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectBackgroundStatusTypes");

                lookup.Tables[0].TableName = "BackgroundStatusTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectBackgroundVerificationStatusTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectBackgroundVerificationStatusTypes");

                lookup.Tables[0].TableName = "BackgroundVerificationStatusTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectBackgroundResultTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectBackgroundResultTypes");

                lookup.Tables[0].TableName = "BackgroundResultTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectBackgroundPerformedByTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectBackgroundPerformedByTypes");

                lookup.Tables[0].TableName = "BackgroundPerformedByTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectOrientationStatusTypes()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectOrientationStatusTypes");

                lookup.Tables[0].TableName = "OrientationStatusTypes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSiteVisitMethods()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectSITE_VISIT_METHODS");

                lookup.Tables[0].TableName = "SiteVistMethods";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSiteVisitScreeningStatuses(int? providerTypeID, int? isInitialStatus)
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                if (providerTypeID.HasValue)
                    sqlParams.Add(new SqlParameter("@PROVIDER_TYPE_ID", providerTypeID));
                if (isInitialStatus.HasValue)
                    sqlParams.Add(new SqlParameter("@IS_INITIAL_STATUS", isInitialStatus));
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectSiteVisitScreeningStatuses", sqlParams, "SiteVisitScreeningStatuses");

                lookup.Tables[0].TableName = "SiteVisitScreeningStatuses";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        #region CallTracking
        public static DataSet SelectCallTrackingReasons()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCALLTRACKING_SUBJECT");

                lookup.Tables[0].TableName = "Reasons";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCallTrackingSources()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCALLTRACKING_SOURCE");

                lookup.Tables[0].TableName = "Sources";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCallTrackingNextActions()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCALLTRACKING_NEXTACTION");

                lookup.Tables[0].TableName = "NextActions";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCallTrackingResolutions()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCALLTRACKING_RESOLUTION");

                lookup.Tables[0].TableName = "Resolutions";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        #endregion

        public static DataSet SelectLicenseRestrictionCodes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectLicenseRestrictionCodes");
                lookup.Tables[0].TableName = "LicenseRestrictionCodes";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCLIACertificateTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCLIACertificateTypes");
                lookup.Tables[0].TableName = "CLIACertificateTypes";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertTaxonomyType(int specialtyTypeID, int providerTypeID, string taxonomyCode, string taxonomyName,
            DateTime expirationDate, DateTime lastModifiedDateTime, string lastModifiedUser, string mmisSpecialtyTypeID,
            bool npiRequired)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE_ID", DbType.Int32, specialtyTypeID, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("TAXONOMY_CODE", DbType.String, taxonomyCode, false));
                parameters.Add(SqlParms.CreateParameter("TAXONOMY_NAME", DbType.String, taxonomyName, false));
                parameters.Add(SqlParms.CreateParameter("EXPIRATION_DATE", DbType.DateTime, expirationDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("MMIS_SPECIALTY_TYPE_ID", DbType.String, mmisSpecialtyTypeID, false));
                parameters.Add(SqlParms.CreateParameter("NPI_REQUIRED", DbType.Boolean, npiRequired, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertTAXONOMY_TYPE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateTaxonomyType(int taxonomyTypeID, int specialtyTypeID, int providerTypeID, string taxonomyCode,
            string taxonomyName, DateTime expirationDate, DateTime lastModifiedDateTime, string lastModifiedUser,
            string mmisSpecialtyTypeID, bool npiRequired)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TAXONOMY_TYPE_ID", DbType.Int32, taxonomyTypeID, false));
                parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE_ID", DbType.Int32, specialtyTypeID, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("TAXONOMY_CODE", DbType.String, taxonomyCode, false));
                parameters.Add(SqlParms.CreateParameter("TAXONOMY_NAME", DbType.String, taxonomyName, false));
                parameters.Add(SqlParms.CreateParameter("EXPIRATION_DATE", DbType.DateTime, expirationDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("MMIS_SPECIALTY_TYPE_ID", DbType.String, mmisSpecialtyTypeID, false));
                parameters.Add(SqlParms.CreateParameter("NPI_REQUIRED", DbType.Boolean, npiRequired, true));
                DataAccess.ExecuteStoredProcedure("updateTAXONOMY_TYPE", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }

        public static DataSet SelectAllProviderTypes()
        {
            try
            {
                return DataAccess
                    .ExecuteSelectSql(
                    "SELECT [PROVIDER_TYPE_ID],[PROVIDER_TYPE_ABBREVIATION],[PROVIDER_TYPE_NAME],pt.[LAST_MODIFIED_DATE_TIME],pt.[LAST_MODIFIED_USER],"
                    + "pt.[IS_USED_IN_MMIS],pt.[PROVIDER_CATEGORY_TYPE_ID],pct.[PROVIDER_CATEGORY_TYPE_NAME],[MMIS_PROVIDER_TYPE_ID],"
                    + "[REQUIRE_NPI],pt.[PROVIDER_RISK_LEVEL_ID],prl.[PROVIDER_RISK_LEVEL_NAME],pt.[APPLICATION_TYPE_ID],at.[APPLICATION_TYPE_NAME] "
                    + "FROM [dbo].[PROVIDER_TYPE] pt "
                    + "inner join [dbo].[PROVIDER_CATEGORY_TYPE] pct on pt.[PROVIDER_CATEGORY_TYPE_ID] = pct.[PROVIDER_CATEGORY_TYPE_ID] "
                    + "left join [dbo].[APPLICATION_TYPE] at on at.[APPLICATION_TYPE_ID] = pt.[APPLICATION_TYPE_ID] "
                    + "left join [dbo].[PROVIDER_RISK_LEVEL] prl on prl.[PROVIDER_RISK_LEVEL_ID] = pt.[PROVIDER_RISK_LEVEL_ID] "
                );
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAllPAAssignProcedureGroups()
        {
            try
            {
                return DataAccess
                    .ExecuteSelectSql(
                    "SELECT [ID],[CDE_PA_ASSIGN],[DSC_50],Convert(date,[DTE_EFFECTIVE]) DTE_EFFECTIVE, Convert(date, [DTE_END]) DTE_END,"
                    + "[PROC FROM] PROC_FROM, [PROC TO] PROC_TO, [PROC_FROM_ORDER], [PROC_TO_ORDER], [LAST_MODIFIED_DATE_TIME]"
                    + "FROM[dbo].[PA_ASSIGN_PROCEDURE_GRP]"
                    + "ORDER BY [ID] DESC"
                );
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAllSpecialtyTypesUnfiltered()
        {
            try
            {
                return DataAccess
                    .ExecuteSelectSql("SELECT [SPECIALTY_TYPE_ID],[SPECIALTY_TYPE_NAME],[LAST_MODIFIED_DATE_TIME],[LAST_MODIFIED_USER],[MMIS_SPECIALTY_TYPE_ID],[EXTERNAL_SPECIALTY_TYPE_NAME],[IsVisible] FROM [dbo].[SPECIALTY_TYPE]");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAllApplicationTypes()
        {
            try
            {
                return DataAccess
                    .ExecuteSelectSql("SELECT [APPLICATION_TYPE_ID] ,[APPLICATION_TYPE_NAME] ,[APPLICATION_TYPE_DESC] ,Convert(bit,[IS_USED_IN_MMIS]) as IS_USED_IN_MMIS ,[MMIS_APPLICATION_TYPE_ID] ,[LAST_MODIFIED_DATE_TIME] ,[LAST_MODIFIED_USER] ,[IsVisible] FROM [dbo].[APPLICATION_TYPE]");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertSpecialtyType(string specialtyTypeName, DateTime lastModifiedDateTime,
            string lastModifiedUser, string mmisSpecialtyTypeID, string externalSpecialtyTypeName, bool isVisible)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE_NAME", DbType.String, specialtyTypeName, false));
                parameters.Add(SqlParms.CreateParameter("EXTERNAL_SPECIALTY_TYPE_NAME", DbType.String, externalSpecialtyTypeName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("MMIS_SPECIALTY_TYPE_ID", DbType.String, mmisSpecialtyTypeID, false));
                parameters.Add(SqlParms.CreateParameter("IsVisible", DbType.Boolean, isVisible, false));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertSPECIALTY_TYPE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateSpecialtyType(int specialtyTypeID, string specialtyTypeName, DateTime lastModifiedDateTime,
            string lastModifiedUser, string mmisSpecialtyTypeID, string externalSpecialtyTypeName, bool isVisible)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE_ID", DbType.Int32, specialtyTypeID, false));
                parameters.Add(SqlParms.CreateParameter("EXTERNAL_SPECIALTY_TYPE_NAME", DbType.String, externalSpecialtyTypeName, false));
                parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE_NAME", DbType.String, specialtyTypeName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("MMIS_SPECIALTY_TYPE_ID", DbType.String, mmisSpecialtyTypeID, false));
                parameters.Add(SqlParms.CreateParameter("IsVisible", DbType.Boolean, isVisible, false));
                DataAccess.ExecuteStoredProcedure("updateSPECIALTY_TYPE", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }


        public static int InsertProviderType(string providerTypeAbbreviation, string providerTypeName, DateTime lastModifiedDateTime, string lastModifiedUser,
           string isUsedInMMIS, int providerCategoryTypeID, string mmisProviderTypeID, bool requireNPI, int providerRiskLevelID, int applicationTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ABBREVIATION", DbType.String, providerTypeAbbreviation, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_NAME", DbType.String, providerTypeName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("IS_USED_IN_MMIS", DbType.String, isUsedInMMIS, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, providerCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("MMIS_PROVIDER_TYPE_ID", DbType.String, mmisProviderTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REQUIRE_NPI", DbType.Boolean, requireNPI, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_RISK_LEVEL_ID", DbType.Int32, providerRiskLevelID, true));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeID, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertPROVIDER_TYPE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateProviderType(int providerTypeID, string providerTypeAbbreviation, string providerTypeName, DateTime lastModifiedDateTime, string lastModifiedUser,
           string isUsedInMMIS, int providerCategoryTypeID, string mmisProviderTypeID, bool requireNPI, int providerRiskLevelID, int applicationTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.String, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ABBREVIATION", DbType.String, providerTypeAbbreviation, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_NAME", DbType.String, providerTypeName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("IS_USED_IN_MMIS", DbType.String, isUsedInMMIS, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, providerCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("MMIS_PROVIDER_TYPE_ID", DbType.String, mmisProviderTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REQUIRE_NPI", DbType.Boolean, requireNPI, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_RISK_LEVEL_ID", DbType.Int32, providerRiskLevelID, true));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeID, true));
                DataAccess.ExecuteStoredProcedure("updateProvider_TYPE", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertPAAssignProcedureGrp(string cdePAAssign, string dsc50, string procFrom, string procTo, int procFromOrder, int procToOrder, DateTime dteEffective, DateTime dteEnd, string createdByUser, DateTime createdOnDate, DateTime lastModifiedDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CDE_PA_ASSIGN", DbType.String, cdePAAssign, false));
                parameters.Add(SqlParms.CreateParameter("DSC_50", DbType.String, dsc50, false));
                parameters.Add(SqlParms.CreateParameter("DTE_EFFECTIVE", DbType.DateTime, dteEffective, false));
                parameters.Add(SqlParms.CreateParameter("DTE_END", DbType.DateTime, dteEnd, false));
                parameters.Add(SqlParms.CreateParameter("PROC_FROM", DbType.String, procFrom, false));
                parameters.Add(SqlParms.CreateParameter("PROC_TO", DbType.String, procTo, true));
                parameters.Add(SqlParms.CreateParameter("PROC_FROM_ORDER", DbType.Int32, procFromOrder, true));
                parameters.Add(SqlParms.CreateParameter("PROC_TO_ORDER", DbType.Int32, procToOrder, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, createdByUser, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, lastModifiedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("usp_InsertPAAssignProcedureGrp", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdatePAAssignProcedureGrp(int id, string cdePAAssign, string dsc50, string procFrom, string procTo, int procFromOrder, int procToOrder, DateTime dteEffective, DateTime dteEnd)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, id, false));
                parameters.Add(SqlParms.CreateParameter("CDE_PA_ASSIGN", DbType.String, cdePAAssign, false));
                parameters.Add(SqlParms.CreateParameter("DSC_50", DbType.String, dsc50, false));
                parameters.Add(SqlParms.CreateParameter("DTE_EFFECTIVE", DbType.DateTime, dteEffective, false));
                parameters.Add(SqlParms.CreateParameter("DTE_END", DbType.DateTime, dteEnd, false));
                parameters.Add(SqlParms.CreateParameter("PROC_FROM", DbType.String, procFrom, false));
                parameters.Add(SqlParms.CreateParameter("PROC_TO", DbType.String, procTo, true));
                parameters.Add(SqlParms.CreateParameter("PROC_FROM_ORDER", DbType.Int32, procFromOrder, true));
                parameters.Add(SqlParms.CreateParameter("PROC_TO_ORDER", DbType.Int32, procToOrder, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdatePAAssignProcedureGrp", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetClaimProcEffectiveDate(string procCode)
        {
            try
            {
                return DataAccess.ExecuteSelectSql("select CLAIMS_HCPCS_PROCEDURE_CODE, EFFECTIVE_DATE, END_DATE, CLAIMS_HCPCS_PROCEDURE_CODE_ORDER, RECORD_STATUS from CLAIMS_HCPCS_PROCEDURE_CODE where claims_hcpcs_procedure_code = '" + procCode + "'");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertProviderCategoryType(string providerCategoryTypeName, DateTime lastModifiedDateTime, string lastModifiedUser,
            bool isActivePhase2, string mmisProviderCategoryTypeID, string imageSource)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_NAME", DbType.String, providerCategoryTypeName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("IsActive_PHASEII", DbType.Boolean, isActivePhase2, false));
                parameters.Add(SqlParms.CreateParameter("MMIS_PROVIDER_CATEGORY_TYPE_ID", DbType.String, mmisProviderCategoryTypeID, true));
                parameters.Add(SqlParms.CreateParameter("IMAGE_SRC", DbType.String, imageSource, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertPROVIDER_CATEGORY_TYPE_CUSTOM", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateProviderCategoryType(int providerCategoryTypeID, string providerCategoryTypeName,
            DateTime lastModifiedDateTime, string lastModifiedUser, bool isActivePhase2, string mmisProviderCategoryTypeID, string imageSource)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.String, providerCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_NAME", DbType.String, providerCategoryTypeName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("IsActive_PHASEII", DbType.Boolean, isActivePhase2, false));
                parameters.Add(SqlParms.CreateParameter("MMIS_PROVIDER_CATEGORY_TYPE_ID", DbType.String, mmisProviderCategoryTypeID, true));
                parameters.Add(SqlParms.CreateParameter("IMAGE_SRC", DbType.String, imageSource, true));
                DataAccess.ExecuteStoredProcedure("updateProvider_CATEGORY_TYPE", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertApplicationType(string applicationTypeName, string applicationTypeDescription,
            bool isUsedInMMIS, string mmisApplicationTypeID, DateTime lastModifiedDateTime, string lastModifiedUser, bool isVisible)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_NAME", DbType.String, applicationTypeName, false));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_DESC", DbType.String, applicationTypeDescription, true));
                parameters.Add(SqlParms.CreateParameter("IS_USED_IN_MMIS", DbType.Boolean, isUsedInMMIS, true));
                parameters.Add(SqlParms.CreateParameter("MMIS_APPLICATION_TYPE_ID", DbType.String, mmisApplicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("IsVisible", DbType.Boolean, isVisible, false));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertAPPLICATION_TYPE_CUSTOM", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateApplicationType(int applicationTypeID, string applicationTypeName, string applicationTypeDescription,
            bool isUsedInMMIS, string mmisApplicationTypeID, DateTime lastModifiedDateTime, string lastModifiedUser, bool isVisible)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.String, applicationTypeID, false));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_NAME", DbType.String, applicationTypeName, false));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_DESC", DbType.String, applicationTypeDescription, true));
                parameters.Add(SqlParms.CreateParameter("IS_USED_IN_MMIS", DbType.Boolean, isUsedInMMIS, true));
                parameters.Add(SqlParms.CreateParameter("MMIS_APPLICATION_TYPE_ID", DbType.String, mmisApplicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("IsVisible", DbType.Boolean, isVisible, false));
                DataAccess.ExecuteStoredProcedure("updateAPPLICATION_TYPE_CUSTOM", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int Insert_Claims_Diagnosis_Information(string sequence, string diagnosisCode, string icdVersions, string diagnosisDescription, string Last_Modified_user, string Created_by_user)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Sequence", DbType.String, sequence, false));
                parameters.Add(SqlParms.CreateParameter("Diagnosis_Code", DbType.String, diagnosisCode, true));
                parameters.Add(SqlParms.CreateParameter("ICD_Version", DbType.String, icdVersions, true));
                parameters.Add(SqlParms.CreateParameter("Diagnosis_Description", DbType.String, diagnosisDescription, true));
                //parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimID, true));
                parameters.Add(SqlParms.CreateParameter("Last_modified_date", DbType.DateTime, DateTime.Now.ToString(), true));
                parameters.Add(SqlParms.CreateParameter("Last_Modified_user", DbType.String, Last_Modified_user, true));
                parameters.Add(SqlParms.CreateParameter("Created_date_time", DbType.DateTime, DateTime.Now.ToString(), true));
                parameters.Add(SqlParms.CreateParameter("Created_by_user", DbType.String, Created_by_user, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertClaims_Diagnosis_Information", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegPageSettingActionAll()
        {
            string sql = "SELECT [REG_PAGE_SETTING_ACTION_ID],[REG_PAGE_NAME],[ROLE_NAME],[TAKE_ACTION_ALLOWED],[LAST_MODIFIED_DATE_TIME],[LAST_MODIFIED_USER] FROM [dbo].[REG_PAGE_SETTING_ACTION]";

            try
            {
                return DataAccess
                    .ExecuteSelectSql(sql);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegPageTypeAll()
        {
            string sql = "SELECT [REG_PAGE_TYPE_ID],[REG_PAGE_NAME],[LAST_MODIFIED_DATE_TIME],[LAST_MODIFIED_USER] ,[SEQUENCE_ID] FROM [dbo].[REG_PAGE_TYPE]";

            try
            {
                return DataAccess
                    .ExecuteSelectSql(sql);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegPageSettingAll(string filterExpression)
        {
            string sql = "SELECT rps.[REG_PAGE_SETTING_ID] ,rps.[ENTITY_TYPE_ID] , pct.[PROVIDER_CATEGORY_TYPE_NAME] as 'ENTITY_TYPE_NAME' ,rps.[PROVIDER_TYPE_ID] ,pt.[PROVIDER_TYPE_NAME] ,rps.[REG_PAGE_NAME] ,rps.[REG_PAGE_SECTION] ,rps.[IS_VISIBLE] ,rps.[IS_EDITABLE] ,rps.[LAST_MODIFIED_DATE_TIME] ,rps.[LAST_MODIFIED_USER] ,rps.[TASK_NAME] ,rps.[IS_REQUIRED] ,rps.[APPLICATION_TYPE_ID] ,at.[APPLICATION_TYPE_NAME] ,rps.[REG_PAGE_TYPE_ID] ,rpt.[REG_PAGE_NAME] ,rps.[REG_SECTION_TYPE_ID] ,rst.[REG_SECTION_TYPE_NAME],ISNULL(rps.[IS_REVIEW_REQUIRED],1) as IS_REVIEW_REQUIRED FROM [dbo].[REG_PAGE_SETTING] rps inner join provider_category_type pct on rps.ENTITY_TYPE_ID = pct.PROVIDER_CATEGORY_TYPE_ID left join provider_type pt on rps.PROVIDER_TYPE_ID = pt.PROVIDER_TYPE_ID left join application_type at on at.APPLICATION_TYPE_ID = rps.APPLICATION_TYPE_ID left join reg_page_type rpt on rpt.REG_PAGE_TYPE_ID = rps.REG_PAGE_TYPE_ID left join reg_section_type rst on rst.REG_SECTION_TYPE_ID = rps.REG_SECTION_TYPE_ID";

            if (!string.IsNullOrEmpty(filterExpression))
                sql += " WHERE " + filterExpression;

            try
            {
                return DataAccess
                    .ExecuteSelectSql(sql);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegSectionUploadControlAll()
        {
            string sql = "SELECT rsuc.[REG_SECTION_UPLOAD_CONTROL_ID] ,rsuc.[APPLICATION_TYPE_ID] ,at.[APPLICATION_TYPE_NAME] ,rsuc.[PROVIDER_CATEGORY_TYPE_ID] ,pct.[PROVIDER_CATEGORY_TYPE_NAME] ,rsuc.[PROVIDER_TYPE_ID] ,pt.[PROVIDER_TYPE_NAME] ,rsuc.[REG_PAGE_TYPE_ID] ,rpt.[REG_PAGE_NAME] ,rsuc.[TITLE] ,rsuc.[DESCRIPTION] ,rsuc.[IS_REQUIRED] ,rsuc.[LAST_MODIFIED_DATE_TIME] ,rsuc.[LAST_MODIFIED_USER] ,rsuc.[REG_PAGE_SECTION] ,rsuc.[REG_PAGE_NAME] FROM [dbo].[REG_SECTION_UPLOAD_CONTROL] rsuc left join APPLICATION_TYPE at on rsuc.APPLICATION_TYPE_ID = at.APPLICATION_TYPE_ID left join PROVIDER_CATEGORY_TYPE pct on pct.PROVIDER_CATEGORY_TYPE_ID = rsuc.PROVIDER_CATEGORY_TYPE_ID left join PROVIDER_TYPE pt on pt.PROVIDER_TYPE_ID = rsuc.PROVIDER_TYPE_ID inner join REG_PAGE_TYPE rpt on rpt.REG_PAGE_TYPE_ID = rsuc.REG_PAGE_TYPE_ID";

            try
            {
                return DataAccess
                    .ExecuteSelectSql(sql);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderTypeFeeAll()
        {
            string sql = "SELECT ptf.[PROVIDER_TYPE_FEE_ID] ,ptf.[PROVIDER_TYPE_ID] ,pt.PROVIDER_TYPE_NAME ,ptf.[IS_FEE_REQUIRED] ,ptf.[FEE_AMOUNT] ,ptf.[LAST_MODIFIED_DATE_TIME] ,ptf.[LAST_MODIFIED_USER] ,ptf.[ENTITY_TYPE_ID] ,pct.PROVIDER_CATEGORY_TYPE_NAME as 'ENTITY_TYPE_NAME' ,ptf.[APPLICATION_TYPE_ID] ,at.APPLICATION_TYPE_NAME FROM [dbo].[PROVIDER_TYPE_FEE] ptf inner join Provider_type pt on pt.PROVIDER_TYPE_ID = ptf.PROVIDER_TYPE_ID inner join provider_category_type pct on pct.PROVIDER_CATEGORY_TYPE_ID = ptf.ENTITY_TYPE_ID left join APPLICATION_TYPE at on at.APPLICATION_TYPE_ID = ptf.APPLICATION_TYPE_ID";
            try
            {
                return DataAccess
                    .ExecuteSelectSql(sql);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectApplicationFeePaymentTypeAll()
        {
            string sql = "SELECT [PAYMENT_TYPE_ID] ,[PAYMENT_TYPE_NAME] ,[LAST_MODIFIED_DATE_TIME] ,[LAST_MODIFIED_USER] FROM [dbo].[APPLICATION_FEE_PAYMENT_TYPE]";
            try
            {
                return DataAccess
                    .ExecuteSelectSql(sql);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPaperRequestDocumentTypeAll()
        {
            string sql = "SELECT [PAPER_REQUEST_DOCUMENT_TYPE_ID] ,[PAPER_REQUEST_DOCUMENT_TYPE_NAME] ,[PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION] ,[LAST_MODIFIED_DATE_TIME] ,[LAST_MODIFIED_USER] ,[PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE] FROM [dbo].[PAPER_REQUEST_DOCUMENT_TYPE]";
            try
            {
                return DataAccess
                    .ExecuteSelectSql(sql);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegSectionTypeAll()
        {
            string sql = "SELECT [REG_SECTION_TYPE_ID] ,[REG_SECTION_TYPE_NAME] ,[REG_PAGE_TYPE_ID] ,[LAST_MODIFIED_DATE_TIME] ,[LAST_MODIFIED_USER] ,[SEQUENCE_ID] ,[HELPER_HEADING_MAIN] ,[HELPER_HEADING_SUB] ,[HELPER_TEXT], [IS_UPDATABLE_SECTION],[SECTION_DISPLAY_NAME],[IS_COMMON_UPDATE] FROM [dbo].[REG_SECTION_TYPE]";
            try
            {
                return DataAccess
                    .ExecuteSelectSql(sql);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertApplicationFeePaymentType(string paymentTypeName, DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PAYMENT_TYPE_NAME", DbType.String, paymentTypeName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertAPPLICATION_FEE_PAYMENT_TYPE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertPaperRequestDocumentType(string paperRequestDocumentTypeName,
            DateTime lastModifiedDate, Guid lastModifiedUser, string paperRequestDocumentTypeDescription,
            string paperRequestDocumentTypeOnbaseCode)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PAPER_REQUEST_DOCUMENT_TYPE_NAME", DbType.String, paperRequestDocumentTypeName, false));
                parameters.Add(SqlParms.CreateParameter("PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION", DbType.String, paperRequestDocumentTypeDescription, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE", DbType.String, paperRequestDocumentTypeOnbaseCode, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertPAPER_REQUEST_DOCUMENT_TYPE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertProviderTypeFee(int providerTypeID, bool isFeeRequired, decimal feeAmount, DateTime lastModifiedDate,
            Guid lastModifiedUser, int entityTypeID, int applicationTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("IS_FEE_REQUIRED", DbType.Boolean, isFeeRequired, true));
                parameters.Add(SqlParms.CreateParameter("FEE_AMOUNT", DbType.Decimal, feeAmount, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("ENTITY_TYPE_ID", DbType.Int32, entityTypeID, false));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeID, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertPROVIDER_TYPE_FEE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertRegPageSettingAction(string regPageName, string roleName, bool takeActionAllowed,
            DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_NAME", DbType.String, regPageName, false));
                parameters.Add(SqlParms.CreateParameter("ROLE_NAME", DbType.String, roleName, false));
                parameters.Add(SqlParms.CreateParameter("TAKE_ACTION_ALLOWED", DbType.String, takeActionAllowed, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertREG_PAGE_SETTING_ACTION", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertRegPageSetting(int entityTypeID, int providerTypeID, string regPageName,
            string regPageSection, bool isVisible, bool isEditable, DateTime lastModifiedDate, Guid lastModifiedUser,
            string taskName, bool isRequired, int applicationTypeID, int regPageTypeID, int regSectionTypeID, bool isReviewRequired)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ENTITY_TYPE_ID", DbType.Int32, entityTypeID, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_NAME", DbType.String, regPageName, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, regPageSection, true));
                parameters.Add(SqlParms.CreateParameter("IS_VISIBLE", DbType.Boolean, isVisible, false));
                parameters.Add(SqlParms.CreateParameter("IS_EDITABLE", DbType.Boolean, isEditable, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("TASK_NAME", DbType.String, taskName, true));
                parameters.Add(SqlParms.CreateParameter("IS_REQUIRED", DbType.Boolean, isRequired, true));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REG_SECTION_TYPE_ID", DbType.Int32, regSectionTypeID, true));
                parameters.Add(SqlParms.CreateParameter("IS_REVIEW_REQUIRED", DbType.Boolean, isReviewRequired, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertREG_PAGE_SETTING", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertRegPageType(string regPageName, DateTime lastModifiedDate, Guid lastModifiedUser, int? sequenceID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_NAME", DbType.String, regPageName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("SEQUENCE_ID", DbType.Int32, sequenceID, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertREG_PAGE_TYPE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertRegSectionUploadControl(int applicationTypeID, int providerCategoryTypeID, int providerTypeID,
            int regPageTypeID, string title, string description, bool isRequired, DateTime lastModifiedDate, Guid lastModifiedUser,
                string regPageSection, string regPageName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, providerCategoryTypeID, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeID, false));
                parameters.Add(SqlParms.CreateParameter("TITLE", DbType.String, title, false));
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, description, true));
                parameters.Add(SqlParms.CreateParameter("IS_REQUIRED", DbType.Boolean, isRequired, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, regPageSection, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_NAME", DbType.String, regPageName, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertREG_SECTION_UPLOAD_CONTROL", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static void UpdateApplicationFeePaymentType(int applicationFeePaymentTypeID, string paymentTypeName, DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PAYMENT_TYPE_ID", DbType.String, applicationFeePaymentTypeID, false));
                parameters.Add(SqlParms.CreateParameter("PAYMENT_TYPE_NAME", DbType.String, paymentTypeName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                DataAccess.ExecuteStoredProcedure("updateAPPLICATION_FEE_PAYMENT_TYPE", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdatePaperRequestDocumentType(int paperRequestDocumentTypeID, string paperRequestDocumentTypeName,
            DateTime lastModifiedDate, Guid lastModifiedUser, string paperRequestDocumentTypeDescription,
            string paperRequestDocumentTypeOnbaseCode)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PAPER_REQUEST_DOCUMENT_TYPE_ID", DbType.String, paperRequestDocumentTypeID, false));
                parameters.Add(SqlParms.CreateParameter("PAPER_REQUEST_DOCUMENT_TYPE_NAME", DbType.String, paperRequestDocumentTypeName, false));
                parameters.Add(SqlParms.CreateParameter("PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION", DbType.String, paperRequestDocumentTypeDescription, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));
                parameters.Add(SqlParms.CreateParameter("PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE", DbType.String, paperRequestDocumentTypeOnbaseCode, true));
                DataAccess.ExecuteStoredProcedure("updatePAPER_REQUEST_DOCUMENT_TYPE", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateProviderTypeFee(int providerTypeFeeID, int providerTypeID, bool isFeeRequired, decimal feeAmount, DateTime lastModifiedDate,
            Guid lastModifiedUser, int entityTypeID, int applicationTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_FEE_ID", DbType.Int32, providerTypeFeeID, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("IS_FEE_REQUIRED", DbType.Boolean, isFeeRequired, true));
                parameters.Add(SqlParms.CreateParameter("FEE_AMOUNT", DbType.Decimal, feeAmount, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("ENTITY_TYPE_ID", DbType.Int32, entityTypeID, false));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeID, true));
                DataAccess.ExecuteStoredProcedure("updatePROVIDER_TYPE_FEE", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateRegPageSettingAction(int regPageSettingActionID, string regPageName, string roleName, bool takeActionAllowed,
            DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_SETTING_ACTION_ID", DbType.Int32, regPageSettingActionID, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_NAME", DbType.String, regPageName, false));
                parameters.Add(SqlParms.CreateParameter("ROLE_NAME", DbType.String, roleName, false));
                parameters.Add(SqlParms.CreateParameter("TAKE_ACTION_ALLOWED", DbType.String, takeActionAllowed, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                DataAccess.ExecuteStoredProcedure("updateREG_PAGE_SETTING_ACTION", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateRegPageSetting(int regPageSettingID, int entityTypeID, int providerTypeID, string regPageName,
            string regPageSection, bool isVisible, bool isEditable, DateTime lastModifiedDate, Guid lastModifiedUser,
            string taskName, bool isRequired, int applicationTypeID, int regPageTypeID, int regSectionTypeID, bool isReviewRequired)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_SETTING_ID", DbType.Int32, regPageSettingID, false));
                parameters.Add(SqlParms.CreateParameter("ENTITY_TYPE_ID", DbType.Int32, entityTypeID, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_NAME", DbType.String, regPageName, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, regPageSection, true));
                parameters.Add(SqlParms.CreateParameter("IS_VISIBLE", DbType.Boolean, isVisible, false));
                parameters.Add(SqlParms.CreateParameter("IS_EDITABLE", DbType.Boolean, isEditable, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("TASK_NAME", DbType.String, taskName, true));
                parameters.Add(SqlParms.CreateParameter("IS_REQUIRED", DbType.Boolean, isRequired, true));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeID, true));
                if (regSectionTypeID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("REG_SECTION_TYPE_ID", DbType.Int32, regSectionTypeID, true));
                }
                parameters.Add(SqlParms.CreateParameter("IS_REVIEW_REQUIRED", DbType.Boolean, isReviewRequired, true));
                DataAccess.ExecuteStoredProcedure("updateREG_PAGE_SETTING", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateRegPageType(int regPageTypeID, string regPageName, DateTime lastModifiedDate, Guid lastModifiedUser,
            int? sequenceID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.String, regPageTypeID, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_NAME", DbType.String, regPageName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("SEQUENCE_ID", DbType.Int32, sequenceID, true));

                DataAccess.ExecuteStoredProcedure("updateREG_PAGE_TYPE", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateRegSectionUploadControl(int regSectionUploadControlID, int applicationTypeID, int providerCategoryTypeID, int providerTypeID,
            int regPageTypeID, string title, string description, bool isRequired, DateTime lastModifiedDate, Guid lastModifiedUser,
                string regPageSection, string regPageName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_SECTION_UPLOAD_CONTROL_ID", DbType.Int32, regSectionUploadControlID, true));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, providerCategoryTypeID, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeID, false));
                parameters.Add(SqlParms.CreateParameter("TITLE", DbType.String, title, false));
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, description, true));
                parameters.Add(SqlParms.CreateParameter("IS_REQUIRED", DbType.Boolean, isRequired, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, regPageSection, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_NAME", DbType.String, regPageName, true));
                DataAccess.ExecuteStoredProcedure("updateREG_SECTION_UPLOAD_CONTROL", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetAllSections()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_SelectREG_SECTION_TYPES");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAppSettingsAll()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_SelectAppSettings");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAllAutomatedReports()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_SelectAllAutomatedReports");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDataFixTablesAll()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_SelectDataFixTablesAll");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectWebAPITestingAll()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_SelectWebAPITestingAll");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateAppSettings(string appSettingsKey, string appSettingsValue, bool appSettingsReadOnly, DateTime lastUpdatedDate, Guid lastActivityUserId, string appSettingsNotes, bool appSettingsEnvironSpecific, bool CanOverwrite)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("AppSettingsKey", DbType.String, appSettingsKey, false));
                parameters.Add(SqlParms.CreateParameter("AppSettingsValue", DbType.String, appSettingsValue, true));
                parameters.Add(SqlParms.CreateParameter("AppSettingsReadOnly", DbType.Boolean, appSettingsReadOnly, false));
                parameters.Add(SqlParms.CreateParameter("LastUpdatedDate", DbType.DateTime, lastUpdatedDate, false));
                parameters.Add(SqlParms.CreateParameter("LastActivityUserId", DbType.Guid, lastActivityUserId, false));
                parameters.Add(SqlParms.CreateParameter("AppSettingsNotes", DbType.String, appSettingsNotes, true));
                parameters.Add(SqlParms.CreateParameter("AppSettingsEnvironSpecific", DbType.Boolean, appSettingsEnvironSpecific, false));
                parameters.Add(SqlParms.CreateParameter("CanOverwrite", DbType.Boolean, CanOverwrite, false));
                DataAccess.ExecuteStoredProcedure("updateAPPSETTINGS_CUSTOM", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertAppSettings(string appSettingsKey, string appSettingsValue, bool appSettingsReadOnly, DateTime lastUpdatedDate, Guid lastActivityUserId, string appSettingsNotes, bool appSettingsEnvironSpecific, bool CanOverwrite)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("AppSettingsKey", DbType.String, appSettingsKey, false));
                parameters.Add(SqlParms.CreateParameter("AppSettingsValue", DbType.String, appSettingsValue, true));
                parameters.Add(SqlParms.CreateParameter("AppSettingsReadOnly", DbType.Boolean, appSettingsReadOnly, false));
                parameters.Add(SqlParms.CreateParameter("LastUpdatedDate", DbType.DateTime, lastUpdatedDate, false));
                parameters.Add(SqlParms.CreateParameter("LastActivityUserId", DbType.Guid, lastActivityUserId, false));
                parameters.Add(SqlParms.CreateParameter("AppSettingsNotes", DbType.String, appSettingsNotes, true));
                parameters.Add(SqlParms.CreateParameter("AppSettingsEnvironSpecific", DbType.Boolean, appSettingsEnvironSpecific, false));
                parameters.Add(SqlParms.CreateParameter("CanOverwrite", DbType.Boolean, CanOverwrite, false));
                DataAccess.ExecuteStoredProcedure("insertAPPSETTINGS_CUSTOM", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertUpdateAutomatedReportsMain(Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, object> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateAutomatedReportsMain", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void InsertDynamicFieldConfiguration(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, string> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                DataAccess.ExecuteStoredProcedure("usp_INSERT_DYNAMIC_FIELD_CONFIGURATION", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void InsertUIControlVisibilityConfig(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, string> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                DataAccess.ExecuteStoredProcedure("usp_INSERT_UIControlVisibilityConfig", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDynamicFieldDisplayName(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, string> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_Select_DYNAMIC_FIELD_FIELDNAME", parameters, "DYNAMIC_FIELD_FIELDNAME");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDynamicFieldConfiguration(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, string> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectDYNAMIC_FIELD_CONFIGURATION", parameters, "SelectDYNAMIC_FIELD_CONFIGURATION");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDynamicFieldChildControls(int? configId = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                // Handle both null and not null scenarios
                if (configId.HasValue)
                {
                    parameters.Add(new SqlParameter("@DYNAMIC_FIELD_CONFIGURATION_ID", (object)configId ?? DBNull.Value));
                }
                else
                {
                    parameters.Add(new SqlParameter("@DYNAMIC_FIELD_CONFIGURATION_ID", DBNull.Value));
                }
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetDynamicChildControls", parameters, "SelectDYNAMIC_FIELD_CONFIGURATION");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectUIControlVisibility(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, string> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectUIControlVisibilityConfig", parameters, "SelectUIControlVisibilityConfig");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetLookupDataUpdateTrace(string tablename)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TABLE_NAME", DbType.String, tablename, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_LookupDataUpdateTrace", parameters, "C4Codesets");

                lookup.Tables[0].TableName = "LookupDataTrace";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveLookupDataUpdateTrace(string tablename, string approvalStatus, string userId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TABLE_NAME", DbType.String, tablename, true));
                parameters.Add(SqlParms.CreateParameter("APPROVAL_STATUS", DbType.String, approvalStatus, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, userId, true));
                DataAccess.ExecuteStoredProcedure("usp_Insert_LookupDataUpdateTrace", parameters, "LookupDataUpdateSet");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAutomatedReportsSub(Guid ReportID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REPORT_ID", DbType.Guid, ReportID, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAutomatedReportsSub", parameters, "SelectAutomatedReportsSub");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertDataFixTables(string tableName, string tableValue, string tableType,
            bool isVisible, string pkColumns, string columsHide, string readonlyColumns, DateTime lastmodifiedDate, Guid lastmodifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TABLE_NAME", DbType.String, tableName, false));
                parameters.Add(SqlParms.CreateParameter("TABLE_VALUE", DbType.String, tableValue, true));
                parameters.Add(SqlParms.CreateParameter("TABLE_TYPE", DbType.String, tableType, false));
                parameters.Add(SqlParms.CreateParameter("IS_VISIBLE", DbType.Boolean, isVisible, false));
                parameters.Add(SqlParms.CreateParameter("PK_COLUMN", DbType.String, pkColumns, false));
                parameters.Add(SqlParms.CreateParameter("COLUMNS_HIDE", DbType.String, columsHide, true));
                parameters.Add(SqlParms.CreateParameter("READONLY_COLUMNS", DbType.String, readonlyColumns, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastmodifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastmodifiedUser, false));
                DataAccess.ExecuteStoredProcedure("usp_InsertDataFixTables", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertWebAPITesting(string APIName, string APIDescp, string APIXML, bool APIEnabled)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("APIName", DbType.String, APIName, false));
                parameters.Add(SqlParms.CreateParameter("APIDescp", DbType.String, APIDescp, true));
                parameters.Add(SqlParms.CreateParameter("APIXML", DbType.String, APIXML, true));
                parameters.Add(SqlParms.CreateParameter("APIEnabled", DbType.Boolean, APIEnabled, false));
                DataAccess.ExecuteStoredProcedure("usp_InsertWebAPITesting", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateWebAPITesting(int id, string APIName, string APIDescp, string APIXML, bool APIEnabled)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, id, false));
                parameters.Add(SqlParms.CreateParameter("APIName", DbType.String, APIName, false));
                parameters.Add(SqlParms.CreateParameter("APIDescp", DbType.String, APIDescp, true));
                parameters.Add(SqlParms.CreateParameter("APIXML", DbType.String, APIXML, true));
                parameters.Add(SqlParms.CreateParameter("APIEnabled", DbType.Boolean, APIEnabled, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateWebAPITesting", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateDataFixTables(int id, string tableName, string tableValue, string tableType,
            bool isVisible, string pkColumns, string columsHide, string readonlyColumns, DateTime lastmodifiedDate, Guid lastmodifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, id, false));
                parameters.Add(SqlParms.CreateParameter("TABLE_NAME", DbType.String, tableName, false));
                parameters.Add(SqlParms.CreateParameter("TABLE_VALUE", DbType.String, tableValue, true));
                parameters.Add(SqlParms.CreateParameter("TABLE_TYPE", DbType.String, tableType, false));
                parameters.Add(SqlParms.CreateParameter("IS_VISIBLE", DbType.Boolean, isVisible, false));
                parameters.Add(SqlParms.CreateParameter("PK_COLUMN", DbType.String, pkColumns, false));
                parameters.Add(SqlParms.CreateParameter("COLUMNS_HIDE", DbType.String, columsHide, true));
                parameters.Add(SqlParms.CreateParameter("READONLY_COLUMNS", DbType.String, readonlyColumns, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastmodifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastmodifiedUser, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateDataFixTables", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteDataFixTable(int id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, id, false));
                DataAccess.ExecuteStoredProcedure("usp_DeleteDataFixTable", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMCOAffiliationStatuses()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectMCO_AFFILIATION_STATUS");

                lookup.Tables[0].TableName = "AffiliationStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectMCOs()
        {
            try
            {

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_MCO");

                lookup.Tables[0].TableName = "MCO";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectEducationTypes()
        {

            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectEDUCATION_TYPE");

                lookup.Tables[0].TableName = "EducationType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDataRankTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDATARANK_TYPE");
                lookup.Tables[0].TableName = "DatarankType";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectVerificationSource(int activityTypeID, int IsIndividual, int includeInactive)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SCREENING_TYPE_ID", DbType.Int32, activityTypeID, true));
                parameters.Add(SqlParms.CreateParameter("IS_INDIVIDUAL", DbType.Int32, IsIndividual, true));
                parameters.Add(SqlParms.CreateParameter("INCLUDE_INACTIVE", DbType.Int32, includeInactive, true));
                return DataAccess.ExecuteStoredProcedure("usp_SelectVerificationSourceDropdown", parameters, "VerificationSource");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectDegreeAwardTypes()
        {

            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDEGREE_AWARD_TYPE");

                lookup.Tables[0].TableName = "DegreeAwardType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTaskGroupByTaskName(string taskName)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("taskName", DbType.String, taskName, true));
                return DataAccess.ExecuteStoredProcedure("usp_SelectTASK_GROUP_By_TaskName", parameters, "TaskGroup");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectLicenseTypeSpecialtyTypeByProviderType(string providertypeId)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderType_Id", DbType.String, providertypeId, true));
                return DataAccess.ExecuteStoredProcedure("usp_SelectSPECIALTY_TYPE_LICENSE_TYPE_ByProviderType", parameters, "BoardInfo");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectLicenseStatuses()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectLICENSE_STATUS");
                lookup.Tables[0].TableName = "LicenseStatues";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectLicenseCurrentStatuses()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectLICENSE_CURRENT_STATUS");
                lookup.Tables[0].TableName = "LicenseCurrentStatues";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectLocationTypes(int sectionTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("SECTION_TYPE_ID", DbType.Int32, sectionTypeId, false)
                };
                return DataAccess.ExecuteStoredProcedure("usp_SelectLocationTypes", parameters, "LocationTypes");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static string SelectLicenseTypeIdByAbbrev(string licenseTypeAbbrevation)
        {
            string returnVal = string.Empty;

            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("LICENSE_TYPE_ABBREV", DbType.String, licenseTypeAbbrevation, true));

                returnVal = DataAccess.ExecuteScalar("sp_SelectLicenseTypeByAbbrev", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return returnVal;
        }

        public static DataSet SelectCLIALabCodesByNumber(string cliaNumber)
        {
            try
            {
                //labcodes are stored in clia speacilties table
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CLIA_Number", DbType.String, cliaNumber, true));
                return DataAccess.ExecuteStoredProcedure("usp_SelectCLIA_SpecialtiesByCLIANumber", parameters, "CLIALabCodes");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCLIACertificateInfoByNumber(string cliaNumber)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CLIA_Number", DbType.String, cliaNumber, true));
                return DataAccess.ExecuteStoredProcedure("usp_SelectCLIA_Certificate_InfoByCLIANumber", parameters, "CLIACerttificateInfo");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectBoardCertificationTypeByProviderTypeID(int ProviderTypeID)
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, ProviderTypeID, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectBoardCertificationTypesByProviderTypeID", parameters, "BoardCertificationType");



                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectAdditionalApplications(int RegId)
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, RegId, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDITIONAL_APPLICATION", parameters, "AdditionalApplications");


                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectBoardSpecialtyTypeByBoardCertificationID(int BoardCertificationID)
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("BoardCertificationID", DbType.Int32, BoardCertificationID, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectBoardSpecialtyByBoardCertificationTypeID", parameters, "BoardSpecialtyType");



                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectHearingStatus()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectHEARING_STATUS");
                lookup.Tables[0].TableName = "HearingStatuses";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTerminationReasons()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectTerminationReasons");
                lookup.Tables[0].TableName = "TermReasons";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetChopTypes()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_CHOP_TYPE");
                lookup.Tables[0].TableName = "ChopType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetContractNames(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectListOfContracts", parameters, "Contracts");
                lookup.Tables[0].TableName = "ContractName";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectReasonBackgroundCheckNotRequired()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectReasonBackgroundCheckNotRequired");
                lookup.Tables[0].TableName = "ReasonBackgroundCheckNotRequired";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectHospiceApplicationActionType()
        {

            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Get_HOSPICE_APPLICATION_ACTION_TYPE");
                lookup.Tables[0].TableName = "HOSPICE_APPLICATION_ACTION_TYPE";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectHospiceBenifitSegmentIndicatorType()
        {

            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Get_HOSPICE_BENEFIT_SEGMENT_INDICATOR_TYPE");
                lookup.Tables[0].TableName = "HOSPICE_BENEFIT_SEGMENT_INDICATOR_TYPE";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectHospiceReasonUpdateType()
        {

            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Get_HOSPICE_REASON_UPDATE_TYPE");
                lookup.Tables[0].TableName = "HOSPICE_REASON_UPDATE_TYPE";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectHospicePayerType()
        {

            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("[usp_Get_HOSPICE_PAYER_TYPE]");
                lookup.Tables[0].TableName = "HOSPICE_PAYER_TYPE";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectHospiceDocumentType()
        {

            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Get_HOSPICE_DOCUMENT_TYPE");
                lookup.Tables[0].TableName = "HOSPICE_DOCUMENT_TYPE";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectCounty(string state)
        {

            try
            {
                var lookup = LookupTableController.SelectHospiceCountiesByStateAbbreviation(state);
                lookup.Tables[0].TableName = "COUNTY";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectState()
        {

            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteSelectSql("SELECT STATE_ABBREV,STATE_NAME FROM STATE_NAME");
                lookup.Tables[0].TableName = "STATE";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetICDDiagnosis(string code, string icdVersion, string diagnosisDes, bool isDynamic = false)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet lookup = new DataSet();
                if (!isDynamic)
                {
                    parameters.Add(SqlParms.CreateParameter("Code", DbType.String, code, false));
                    parameters.Add(SqlParms.CreateParameter("IcdVersion", DbType.String, icdVersion, false));
                    parameters.Add(SqlParms.CreateParameter("DiagnosisDes", DbType.String, diagnosisDes, false));

                    lookup = DataAccess.ExecuteStoredProcedure("usp_Search_ICD10DIAGNOSIS", parameters, "DIAGNOSISSearch");
                    lookup.Tables[0].TableName = "DIAGNOSIS";
                }
                else
                {
                    parameters.Add(SqlParms.CreateParameter("Code", DbType.String, code, false));
                    parameters.Add(SqlParms.CreateParameter("IcdVersion", DbType.String, icdVersion, false));


                    lookup = DataAccess.ExecuteStoredProcedure("usp_Search_ICD10DIAGNOSIS_Dynamic", parameters, "DIAGNOSISSearch");
                    lookup.Tables[0].TableName = "DIAGNOSIS";
                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // for submit claims
        public static DataSet GetICDDiagnosisCode(string code, string icdVersion, string diagnosisDes, string diagnosisVersionCode = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Code", DbType.String, code, false));
                parameters.Add(SqlParms.CreateParameter("IcdVersion", DbType.String, icdVersion, false));
                parameters.Add(SqlParms.CreateParameter("DiagnosisDes", DbType.String, diagnosisDes, false));
                if (!string.IsNullOrEmpty(diagnosisVersionCode))
                    parameters.Add(SqlParms.CreateParameter("DiagnosisVersionCode", DbType.String, diagnosisVersionCode, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Search_ICD_DIAGNOSISCODE", parameters, "DIAGNOSISSearch");
                lookup.Tables[0].TableName = "DIAGNOSIS";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetICDDiagnosisCodeSearchResults(string code, string icdVersion, string diagnosisDes, string diagnosisVersionCode = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Code", DbType.String, code, false));
                parameters.Add(SqlParms.CreateParameter("IcdVersion", DbType.String, icdVersion, false));
                parameters.Add(SqlParms.CreateParameter("DiagnosisDes", DbType.String, diagnosisDes, false));
                if (!string.IsNullOrEmpty(diagnosisVersionCode))
                    parameters.Add(SqlParms.CreateParameter("DiagnosisVersionCode", DbType.String, diagnosisVersionCode, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Search_DiagnosisCode", parameters, "DIAGNOSISSearch");
                lookup.Tables[0].TableName = "DIAGNOSIS";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // MS 2021.01.12
        public static DataSet GetInstitutionalTypeOfBill(string code = "", string desc = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("billCode", DbType.String, code, false));
                parameters.Add(SqlParms.CreateParameter("billDESC", DbType.String, desc, false));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Type_Of_Bill", parameters, "CLAIMS_TYPE_OF_BILL");

                lookup.Tables[0].TableName = "TypeOfBill";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetLTCReviewType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_GET_Review_TYPE");
                lookup.Tables[0].TableName = "ReviewType";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }





        public static DataSet GetOwnerTitle()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectManaging_Employee_Title");

                lookup.Tables[0].TableName = "Managing_Employee_Title";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetAffliationType()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAFFILIATION_TYPE");

                lookup.Tables[0].TableName = "AFFILIATION_TYPE";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        //ap
        public static DataSet GetServiceCodeType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SERVICE_TYPE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetServiceTypeCode()//Ap 3/1/2022
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SERVICE_CODE_TYPE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetServiceCodeTypeForPriorAuth()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_MASTER_PROCEDURE_CODE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetProcedureCodeType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SERVICE_CODE_TYPE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet LoadQueueNames()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_MQ_queues");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet LoadDestinationPayer(bool loadAll = false)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("loadAll", DbType.Boolean, loadAll, false));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_DESTINATIONPAYER", parameters, "DESTINATION_PAYERS");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet LoadDestinationPayerID(int destinationPayerMapID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DestinationPayerMapID", DbType.Int32, destinationPayerMapID, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_DESTINATIONPAYERID", parameters, null);
                return lookup;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetPricingFormula()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIORAUTHSERVICEPRICINGFORMULA");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetReviewReasons()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREVIEW_REASON");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetRestrictedServiceStatuses()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectRESTRICT_SERVICE_STATUS");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetIncludeExclude()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectRESTRICT_INCLUDE_EXCLUDE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        //Akta Package 5 start
        public static DataSet GetAssignements()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_Assignment_Type");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetAssignement(string priorAuthType)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PrioAuthorizationType", DbType.String, priorAuthType, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_Assignment_Type", parameters, "AssignmentType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPayerNames()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DESTINATION_PAYER");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetClaimDestinationPayer()
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDESTINATION_PAYER", "DESTINATION_PAYER");
                lookup.Tables[0].TableName = "DESTINATION_PAYER";

                return lookup;
            }
            catch (Exception ex)
            {

                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetDestinationPayer()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DESTINATION_PAYER");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetAuthorization()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_Authorization_TYPE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetPlanName()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_PLAN_NAME");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetManagedcareplan()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_COVERED_BY_MEDICAID_MANAGED_CAREPLAN");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetSpecialIndicator()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SPECIAL_INDICATOR");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetDiagnosisCodeType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DIAGNOSIS_CODE_TYPE");
                //  DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_ICD10DIAGNOSIS");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetDiagnoisCode()
        {
            try
            {
                // DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DIAGNOSIS_CODE_TYPE");
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SERVICE_CODE_TYPE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetICDVersions()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectICDVersions");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetICDCode()
        {
            try
            {
                // DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DIAGNOSIS_CODE_TYPE");
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_ICD10DIAGNOSIS");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }



        //public static DataSet GetServiceCodeType()
        //{
        //    try
        //    {
        //        DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIORAUTHSERVICECODETYPE");
        //        return lookup;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw CoreException.ThrowException(ex);
        //    }
        //}
        public static DataSet GetClaimStatus()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_CLAIM_STATUS");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorPlacement()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DENTAL_PRIOR_PLACEMENT");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetPriorNewPlacement()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DENTAL_NEW_PLACEMENT");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorDocumentType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DOCUMENT_TYPE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorClaimsDocumentType()
        {
            try
            {

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DOCUMENT_TYPE_ClAIM");
                if (lookup.Tables[0].Rows.Count == 0)
                {
                    lookup.Tables[0].Rows.Add(999, "DB ROWS: " + lookup.Tables[0].Rows.Count.ToString());
                }

                return lookup;
            }
            catch (Exception ex)
            {
                // capture ex.message as dropdown single id/description values.
                DataSet DataSet_ex = new DataSet();
                DataTable Table1 = DataSet_ex.Tables.Add();
                DataRow row1 = Table1.NewRow();
                DataColumn Column0 = new DataColumn("PRIOR_AUTH_CLAIM_REPORT_TYPE_ID", typeof(int));
                DataColumn Column1 = new DataColumn("PRIOR_AUTH_CLAIM_REPORT_TYPE_DESC", typeof(string));
                Table1.Columns.Add(Column0);
                Table1.Columns.Add(Column1);
                Table1.Rows.Add(0, ex.Message);
                return DataSet_ex;

                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetStatus()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_STATUS");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetStatusByDetails(string providerNPI, string medicalID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, providerNPI, true));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicalID, true));
                DataSet dataSet = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_STATUS", parameters, "PAStatus");
                return dataSet;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetSequenceType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_CLAIM_SEQUENCE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetDischargeStatus()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_DISCHARGESTATUS");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetAdmitSource()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_ADMISSIONSOURCE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetEPSDTCondition()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_CLAIM_EPSDT");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetAdmissionType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_ADMISSIONTYPE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetAccidentrelatedto()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_ACCIDENTRELATEDTO");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetAccidentState()
        {
            try
            {
                //  DataSet lookup = DataAccess.ExecuteStoredProcedure("dbo.STATE_NAME");
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_STATE_NAME");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetAccidentcounty()
        {
            try
            {
                //  DataSet lookup = DataAccess.ExecuteStoredProcedure("dbo.STATE_NAME");
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_County");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetReason()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_REASON");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetOtherPhysician()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_OTHERPHYSICIAN");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetPatientRelationship()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_PATIENTRELATIONSHIP");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetInsuranceTypes()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_INSURANCE_TYPE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetClaimFilingIndicator()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetClaimAdjudicationLevel()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_ADJUDICATION_LEVEL");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetAdjustmentGroup()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetReasonCodesOPP()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_REASON_CODE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetSubmiclaimtProviderType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_CLAIM_PROVIDERTYPE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetCorrespondenceType()//OHPNM-3814
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_CORRESPONDENCE_TYPE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }



        public static DataSet GetPayerSequence()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetToothSurface()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_SUBMIT_CLAIM_TOOTHSURFACE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPresentAddimission()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_CLAIM_PresentAddimission");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetUnitsOfMeasure()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_CLAIM_UNITSOFMEASURE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetCRProviderType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_CR_ProviderType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetFacilityProgrameTypes(string mmisProviderTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MMIS_ProviderType_Id", DbType.String, mmisProviderTypeId, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_FacilityProgramType", parameters, "FacilityProgramType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetSettelementTypes()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_Settlement_Type", parameters, "SettlementTypeType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetLetterTypes()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_LETTER", parameters, "SettlementTypeType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetReportTypes()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_REPORT", parameters, "SettlementTypeType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetPeriodTypes()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_PERIODTYPE", parameters, "SettlementTypeType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetCostReportTypes(string mmisProviderTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MMIS_ProviderType_Id", DbType.String, mmisProviderTypeId, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_ReportType", parameters, "ReportType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetCountryCodes()
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCOUNTRY_CODE", "CountryCodes");

                lookup.Tables[0].TableName = "CountryCodes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }

        public static DataSet SelectAllApplicationStatus(string sourceSystem)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("Source_System", DbType.String, sourceSystem, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllApplicationStatus", parameters, "ApplicationStatusType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectAllRegistrationStatus()
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATION_STATUS_TYPE", "RegistrationType");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }

        public static DataSet SelectCPCAttestationData(int RegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("RegID", DbType.String, RegID, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_CPCProvider_Agreement", parameters, "Attestation");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCMCAttestationData(int RegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("RegID", DbType.String, RegID, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_CMCProvider_Agreement", parameters, "Attestation");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool CheckDDFacilityNumberExists(string facilityNumber)
        {
            try
            {
                bool isExists = false;
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("DD_Facility_Number", DbType.String, facilityNumber, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_CheckDDFacilityNumberExists", parameters, "facilityNumber");
                if (ObjectControllerHelper.HasRows(lookup))
                {
                    isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetLTRegFromDDFacilityNumberByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regID, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_GetLTRegFromDDFacilityNumberByRegID", parameters, "facilityNumber");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertDelegateUsers(Guid delegateUserID, bool activeDelegate, bool phoneMatch, Guid lastModifiedUser, DateTime lastModifiedDate,
            Guid createdBy, DateTime createdDate, string contactName, string Emailaddr)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DELEGATES_USER_TRACKING_USERID", DbType.Guid, delegateUserID, false));
                parameters.Add(SqlParms.CreateParameter("CONTACT_NAME", DbType.String, contactName, false));
                parameters.Add(SqlParms.CreateParameter("EMAIL", DbType.String, Emailaddr, false));
                parameters.Add(SqlParms.CreateParameter("ACTIVE_DELEGATE", DbType.Boolean, activeDelegate, false));
                parameters.Add(SqlParms.CreateParameter("PHONE_MATCH", DbType.Boolean, phoneMatch, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, createdBy, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, createdDate, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("usp_InsertUpdateDELEGATES_USER_TRACKING", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectUserIDDelegatesData()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectUserIDDelegatesData");
                lookup.Tables[0].TableName = "DelegatesDetails";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckDelegateUserIDisActive(string userID)
        {
            try
            {
                bool isExists = false;
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, userID, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_CheckDelegateUserIDisActive", parameters, "DelegateUserID");
                if (ObjectControllerHelper.HasRows(lookup))
                {
                    isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetOccurreneceCode(string code, string desc)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OCCURRENCE_CODE", DbType.String, code, true));
                parameters.Add(SqlParms.CreateParameter("OCCURRENCE_CODE_DESC", DbType.String, desc, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Occurrence_code", parameters, "OccurrenceData");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int InsertDelegateDocument(Guid userID, string fileName, string newfilename, string fileDescription, int status, Guid lastModifiedUser, DateTime lastModifiedDate,
                Guid createdBy, DateTime createdDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("USERID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, fileName, false));
                parameters.Add(SqlParms.CreateParameter("DELEGATE_FILE_NAME", DbType.String, newfilename, false));
                parameters.Add(SqlParms.CreateParameter("DELEGATE_FILE_DESCRIPTION", DbType.String, fileDescription, false));
                parameters.Add(SqlParms.CreateParameter("STATUS", DbType.Int32, status, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, createdBy, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, createdDate, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("usp_InsertDELEGATE_DOCUMENT_UPLOAD", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertBulkAgentDocument(Guid userID, string fileName, string newfilename, string fileDescription, int status, Guid lastModifiedUser, DateTime lastModifiedDate,
                Guid createdBy, DateTime createdDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("USERID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, fileName, false));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, newfilename, false));
                parameters.Add(SqlParms.CreateParameter("FILE_DESCRIPTION", DbType.String, fileDescription, false));
                parameters.Add(SqlParms.CreateParameter("STATUS", DbType.Int32, status, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, createdBy, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, createdDate, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("usp_InsertAgent_Bulk_Document_Upload", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        #region PriorAuth Service Details
        public static DataSet GetProcedureCodePopupSearch(string procCode = "", string procCodeDesc = "", bool isDynamic = false)
        {
            try
            {
                DataSet lookup = null;
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                if (!isDynamic)
                {
                    sqlParams.Add(new SqlParameter("ProcedureMMISCode", procCode));
                    sqlParams.Add(new SqlParameter("ProcedureDesc", procCodeDesc));
                    lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_PROCEDURE_CODE", sqlParams, "ProcedureCodePopupData");
                    return lookup;
                }
                else
                {
                    sqlParams.Add(new SqlParameter("ProcedureMMISCode", procCode));
                    lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_PROCEDURE_CODE_Dynamic", sqlParams, "ProcedureCodePopupData");
                    return lookup;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetOralCavity()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_FI_TOOTH_QUAD"); return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetRevenueCodePopupSearch(string revenueCode = "", string revenueCodeDesc = "", bool isdynamic = false)
        {
            DataSet lookup = null;
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                if (!isdynamic)
                {
                    sqlParams.Add(new SqlParameter("REVENUE_CODE_MMIS", revenueCode));
                    sqlParams.Add(new SqlParameter("REVENUE_CODE_DESC", revenueCodeDesc));
                    lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_REVENUE_CODE", sqlParams, "RevenueCodePopupData");

                }
                else
                {
                    sqlParams.Add(new SqlParameter("REVENUE_CODE_MMIS", revenueCode));
                    lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_REVENUE_CODE_Dynamic", sqlParams, "RevenueCodePopupData");
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return lookup;
        }

        public static DataTable GetProcedureCodeServiceDetail(string procedureCode = "", string procedureCodeDesc = "", bool isDynamic = false, bool includeICDCodes = true)
        {
            try
            {
                DataTable lookup = new DataTable();
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                if (!isDynamic)
                {
                    sqlParams.Add(new SqlParameter("CLAIMS_HCPCS_PROCEDURE_CODE", procedureCode));
                    sqlParams.Add(new SqlParameter("SHORT_DESC", procedureCodeDesc));
                    if (includeICDCodes)
                    {
                        sqlParams.Add(new SqlParameter("INCLUDE_ICD_CODES", "1"));
                    }
                    else
                    {
                        sqlParams.Add(new SqlParameter("INCLUDE_ICD_CODES", "0"));
                    }
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectProcedureCode", sqlParams, "CLAIMS_HCPCS_PROCEDURE_CODE");
                    if (ds != null)
                    {
                        lookup = ds.Tables[0];
                    }
                }
                else
                {
                    sqlParams.Add(new SqlParameter("CLAIMS_HCPCS_PROCEDURE_CODE", procedureCode));

                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectProcedureCode_Dynamic", sqlParams, "CLAIMS_HCPCS_PROCEDURE_CODE");
                    if (ds != null)
                    {
                        lookup = ds.Tables[0];
                    }
                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        private static bool IsEmpty(DataSet dataSet)
        {
            return !dataSet.Tables.Cast<DataTable>().Any(x => x.DefaultView.Count > 0);
        }

        public static DataTable GetProcedureCodeServiceDetail_InstitutionalDynamic(string procedureCode = "", string procedureCodetype = "")
        {
            try
            {
                DataTable lookup = new DataTable();
                List<SqlParameter> sqlParams = new List<SqlParameter>();

                sqlParams.Add(new SqlParameter("PROCEDURE_CODE", procedureCode));
                sqlParams.Add(new SqlParameter("PROCEDURE_CODE_TYPE", procedureCodetype));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectProcedureCode_Institutional_Dynamic", sqlParams, "CLAIMS_HCPCS_PROCEDURE_CODE");

                bool isEmpty = IsEmpty(ds);

                if (isEmpty)
                {
                    return null;
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    lookup = ds.Tables[0];
                }

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataTable GetProcedureModifierDetail(string procedureCode = "", string procedureCodeDesc = "")
        {
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("PROCEDURE_MODIFIER_CODE", procedureCode));
                sqlParams.Add(new SqlParameter("PROCEDURE_MODIFIER_DESC", procedureCodeDesc));
                DataTable lookup = new DataTable();
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CLAIMS_PROCEDURE_MODIFIER", sqlParams, "PROCEDURE_MODIFIER");
                if (ds != null)
                {
                    lookup = ds.Tables[0];
                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetLevelOfCare()
        {
            try
            {
                DataTable lookup = new DataTable();
                var ds = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_LEVEL_CARE");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataTable GetPlaceofServiceDetail(string placeOfSrviceCode = "", string placeOfServiceName = "", bool getAllData = false)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PlaceServiceCODE", DbType.String, placeOfSrviceCode, true));
                parameters.Add(SqlParms.CreateParameter("PlaceService_Desc", DbType.String, placeOfServiceName, true));
                parameters.Add(SqlParms.CreateParameter("GetAllData", DbType.Boolean, getAllData, true));
                DataTable lookup = new DataTable();
                var ds = DataAccess.ExecuteStoredProcedure("usp_CLAIMS_PLACE_OF_SERVICE", parameters, "PRIOR_AUTH_PLACE_OF_SERVICE");

                if (ds != null)
                {
                    lookup = ds.Tables[0];

                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetRequestedUnitMeasures()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_REQUESTED_UNITS");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataTable GetReasonCode()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataTable lookup = new DataTable();
                var ds = DataAccess.ExecuteStoredProcedure("usp_PRIOR_AUTH_REASON_CODE");
                if (ds != null)
                {
                    lookup = ds.Tables[0];
                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataTable GetICDProcedureCode(string icdCode, string icdDescrption, string icdVersion)
        {
            try
            {
                if (icdVersion == "ICD 10")
                { icdVersion = "J"; }
                else if (icdVersion == "ICD 9")
                {
                    icdVersion = "I";
                }

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("CLAIMS_ICD_PROCEDURE_CODE", icdCode));
                sqlParams.Add(new SqlParameter("ICD_VERSION", icdVersion));
                sqlParams.Add(new SqlParameter("LONG_DESC", icdDescrption));

                DataTable lookup = new DataTable();
                var ds = DataAccess.ExecuteStoredProcedure("usp_SelectCLAIMS_ICD_PROCEDURE_CODE", sqlParams, "CLAIMS_ICD_PROCEDURE_CODE");
                if (ds != null)
                {
                    lookup = ds.Tables[0];
                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        #endregion


        public static DataSet GetDelayReason()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DELAY_REASON");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        #region Upload Attachment


        public static DataSet GetClaimDocumentTypeByClaimTransactionType(int transType)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("BILLING_SERVICE_TRANSACTION_TYPE", DbType.Int32, transType, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectBilling_Service_DOCUMENT_TYPE", parameters, "DocumentTypes");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet GetClaimTransactionType()
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectBILLING_SERVICE_TRANSACTION_TYPE", "TRANSACTION_TYPE");
                lookup.Tables[0].TableName = "TRANSACTION_TYPE";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet GetCPCAttachmentsDocType()
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetCPCAttachmentsDocType", parameters, "DocAttachments");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet GetUploadAttachmentsByMedicaid(string medicaid, string icn, string pa_number)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaid, true));
                parameters.Add(SqlParms.CreateParameter("icn", DbType.String, icn, true));
                parameters.Add(SqlParms.CreateParameter("pa_number", DbType.String, pa_number, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllAttachementsByMedicaid_id", parameters, "DocAttachments");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }



        #endregion
        public static DataSet GetPatientStatus()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_Claims_Patient_Status");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet GetClaimsAdmissionType()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_Claims_Admission_Type");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet GetClaimAdmitSource()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_Claims_Admit_Source");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataTable GetRevenueCode(string revenueCode = "", string shortDesc = "")
        {
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("CLAIMS_REVENUE_CODE", revenueCode));
                sqlParams.Add(new SqlParameter("CLAIMS_REVENUE_CODE_DESC", shortDesc));
                DataTable lookup = new DataTable();

                var ds = DataAccess.ExecuteStoredProcedure("usp_CLAIMS_REVENUE_CODE", sqlParams, "CLAIMS_HCPCS_REVENUE_CODE");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    lookup = ds.Tables[0];
                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataTable GetSpecialtyTypes(string medId = "", string npi = "")
        {
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("MEDID", medId));
                sqlParams.Add(new SqlParameter("NPI", npi));
                DataTable lookup = new DataTable();

                var ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SPECIALTY_Active_ByMedIdNPI", sqlParams, "REG_SPEACIALTIES");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    lookup = ds.Tables[0];
                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataTable GetCodeSetRule(string providerTypeId = "", string specialtyTypeId = "", string code = "", string codeType = "", string claimsorPA = "Claims", string claimORPAType = "")
        {
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("PROVIDER_TYPE_ID", providerTypeId));
                sqlParams.Add(new SqlParameter("SPECIALTY_TYPE_ID", specialtyTypeId));
                sqlParams.Add(new SqlParameter("CODE", code));
                sqlParams.Add(new SqlParameter("CODE_TYPE", codeType));
                sqlParams.Add(new SqlParameter("CLAIMS_OR_PA", claimsorPA));
                sqlParams.Add(new SqlParameter("CLAIM_OR_PA_TYPE", claimORPAType));
                DataTable lookup = new DataTable();

                var ds = DataAccess.ExecuteStoredProcedure("usp_Select_CODESET_RULES_CONFIG", sqlParams, "CODESET_RULES_CONFIG");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    lookup = ds.Tables[0];
                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataTable GetPlaceOfService()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataTable lookup = new DataTable();
                var ds = DataAccess.ExecuteStoredProcedure("usp_CLAIMS_PLACE_OF_SERVICE");
                if (ds != null)
                {
                    lookup = ds.Tables[0];
                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetSpecialProgramIndicator()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Special_Program_Indicator");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataTable GetCrcReasoneCode(string reasonCode, string reasonDesc)
        {
            try
            {

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("CLAIMS_REASON_CODE", reasonCode));
                sqlParams.Add(new SqlParameter("CLAIMS_REASON_CODE_DESC", reasonDesc));

                DataTable lookup = new DataTable();
                var ds = DataAccess.ExecuteStoredProcedure("usp_CLAIMS_CARC", sqlParams, "CLAIMS_CARC");
                if (ds != null)
                {
                    lookup = ds.Tables[0];
                }
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetActiveSpecialities(string medicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_Active_Service_Provider", parameters, "medicaidId");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetServiceActiveEnrollSpan(string medicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_ActiveEnrollSpan", parameters, "medicaidIdEnrollSpan");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPlaceOfServiceDinfo(int claimid, int PlaceofService)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
                parameters.Add(SqlParms.CreateParameter("Place_of_Service", DbType.Int32, PlaceofService, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Place_Of_Service", parameters, "Claims_Service_Details");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetMQLastExecutionDT(string MQAppID)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MQ_JOB_ID", DbType.Guid, new Guid(MQAppID), true));
                ds = DataAccess.ExecuteStoredProcedure("usp_GetMQLastExecutionDT", parameters, "GetMQLastExecutionDT");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetSubCapitaPayerIDs(int destPayerID)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DEST_PAYER_ID", DbType.Int32, destPayerID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_GetSubCapitaPayerIDs", parameters, "GetSubCapitaPayerIDs");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetSubCapitaPayerIDsByMCE_ID(string mce_Id)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MCE_ID", DbType.String, mce_Id, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_GetSubCapitaPayerIDsByMCEId", parameters, "usp_GetSubCapitaPayerIDsByMCEId");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertPAOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested,
         string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId,
         string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string orginalDocumentName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("EDITransaction_Type_ID", DbType.Int32, editTransactionTypeId, false));
                parameters.Add(SqlParms.CreateParameter("PayerRequested", DbType.String, payerRequested, false));
                parameters.Add(SqlParms.CreateParameter("Member_ID", DbType.String, memberId, false));
                parameters.Add(SqlParms.CreateParameter("Claim_Type_ID", DbType.Int32, claimTypeId, false));
                parameters.Add(SqlParms.CreateParameter("Claim_number", DbType.String, claimNumber, false));
                parameters.Add(SqlParms.CreateParameter("PA_NUMBER", DbType.String, paNumber, false));
                parameters.Add(SqlParms.CreateParameter("Provider_ID", DbType.String, providerId, false));
                parameters.Add(SqlParms.CreateParameter("Provider_NPI", DbType.String, providerNPI, false));
                parameters.Add(SqlParms.CreateParameter("Sender_ID", DbType.String, senderId, false));
                parameters.Add(SqlParms.CreateParameter("Receiver_ID", DbType.String, receiverId, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_id", DbType.Int32, documentTypeId, false));
                parameters.Add(SqlParms.CreateParameter("DocumentName", DbType.String, documentName, false));
                parameters.Add(SqlParms.CreateParameter("UUID", DbType.Guid, uuid, false));
                parameters.Add(SqlParms.CreateParameter("TO_SEND", DbType.Boolean, toSend, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appAdminUserId, false));
                parameters.Add(SqlParms.CreateParameter("OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER", DbType.String, documentIdentifier, false));
                parameters.Add(SqlParms.CreateParameter("OriginalDocumentName", DbType.String, orginalDocumentName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATE_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("usp_PAinsertOutBound_Document_Uploads", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorAuthStatusCodes(string priorAuthStatus)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcedureMMISCode", DbType.String, priorAuthStatus, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_STATUS_CODE", parameters, null);
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPayerMCEID(int DestinationPayer)

        {

            try

            {

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("DestinationPayer", DbType.Int32, DestinationPayer, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DESTINATION_PAYER_MCEID", parameters, "Contracts");

                return lookup;



            }

            catch (Exception ex)

            {

                throw CoreException.ThrowException(ex);

            }

        }


        //public static DataSet GetPriorAuthStatusCodes(string priorAuthStatus)

        //     {

        //         try

        //         {

        //             List<SqlParameter> parameters = new List<SqlParameter>();

        //             parameters.Add(SqlParms.CreateParameter("ProcedureMMISCode", DbType.String, priorAuthStatus, true));



        //             DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_STATUS_CODE", parameters, null);

        //             return lookup;

        //         }

        //         catch (Exception ex)

        //         {

        //             throw CoreException.ThrowException(ex);

        //         }

        //     }



        //public static int InsertPAOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested,

        //            string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId,

        //            string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string orginalDocumentName)

        //        {

        //            try

        //            {

        //                List<SqlParameter> parameters = new List<SqlParameter>();

        //                parameters.Add(SqlParms.CreateParameter("EDITransaction_Type_ID", DbType.Int32, editTransactionTypeId, false));

        //                parameters.Add(SqlParms.CreateParameter("PayerRequested", DbType.String, payerRequested, false));

        //                parameters.Add(SqlParms.CreateParameter("Member_ID", DbType.String, memberId, false));

        //                parameters.Add(SqlParms.CreateParameter("Claim_Type_ID", DbType.Int32, claimTypeId, false));

        //                parameters.Add(SqlParms.CreateParameter("Claim_number", DbType.String, claimNumber, false));

        //                parameters.Add(SqlParms.CreateParameter("PA_NUMBER", DbType.String, paNumber, false));

        //                parameters.Add(SqlParms.CreateParameter("Provider_ID", DbType.String, providerId, false));

        //                parameters.Add(SqlParms.CreateParameter("Provider_NPI", DbType.String, providerNPI, false));

        //                parameters.Add(SqlParms.CreateParameter("Sender_ID", DbType.String, senderId, false));

        //                parameters.Add(SqlParms.CreateParameter("Receiver_ID", DbType.String, receiverId, false));

        //                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_id", DbType.Int32, documentTypeId, false));

        //                parameters.Add(SqlParms.CreateParameter("DocumentName", DbType.String, documentName, false));

        //                parameters.Add(SqlParms.CreateParameter("UUID", DbType.Guid, uuid, false));

        //                parameters.Add(SqlParms.CreateParameter("TO_SEND", DbType.Boolean, toSend, false));

        //                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appAdminUserId, false));

        //                parameters.Add(SqlParms.CreateParameter("OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER", DbType.String, documentIdentifier, false));

        //                parameters.Add(SqlParms.CreateParameter("OriginalDocumentName", DbType.String, orginalDocumentName, false));

        //                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));

        //                parameters.Add(SqlParms.CreateParameter("CREATE_DATE_TIME", DbType.DateTime, DateTime.Now, false));

        //                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("usp_PAinsertOutBound_Document_Uploads", parameters));

        //            }

        //            catch (Exception ex)

        //            {

        //                throw CoreException.ThrowException(ex);

        //            }

        //        }


        public static DataSet GetClaimsUploadAttachmentsByMedicaid(string medicaid, string claimID, string claimType)

        {

            try

            {

                DataSet lookup = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaid, true));

                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.String, claimID, true));

                parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.String, claimType, true));

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllClaimAttachementsByMedicaid_id", parameters, "DocAttachments");

                return lookup;

            }

            catch (Exception ex)

            {

                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));

            }

        }
        public static DataSet GetNpiMedIDEnrollmentSpanActions()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_NPI_MEDID_ENROLLMENT_SPAN_ACTIONS");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet GetDataFixActions(int regid = 0, string medicaid_id = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("regid", DbType.String, regid, true));
                parameters.Add(SqlParms.CreateParameter("medicaid_id", DbType.String, medicaid_id, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDATA_FIX_ACTIONS", parameters, "DataFixActions");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetRegIdByMedId(string Medid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, Medid, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectFinancialProviderInformation", parameters, "ServiceLocation");
                return lookup;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetDataFixTablesDetails(string tabletype)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("TABLE_TYPE", DbType.String, tabletype, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDataFixTablesDetails", parameters, "GetDataFixTablesDetails");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetDataFixTableSpecificDetails(string tableName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("TABLE_NAME", DbType.String, tableName, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDataFixTableSpecificDetails", parameters, "GetDataFixTableSpecificDetails");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetTableColumnsByTableName(string tableName, string tableOperation)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("TABLE_NAME", DbType.String, tableName, true));
                parameters.Add(SqlParms.CreateParameter("TABLE_OPS", DbType.String, tableOperation, true));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_GetTableColumnsByTableName", parameters, "GetTableColumnsByTableName");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static string GetUserIOPRefreshTokenInfoByGuid(Guid userID)
        {
            string returnVal = string.Empty;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("USERID", DbType.Guid, userID, true));
                returnVal = DataAccess.ExecuteScalar("usp_GetUserIOPRefreshTokenInfoByGuid", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return returnVal;
        }

        public static string GetUserIOPAccessTokenInfoByGuid(Guid userID)
        {
            string returnVal = string.Empty;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("USERID", DbType.Guid, userID, true));
                returnVal = DataAccess.ExecuteScalar("usp_GetUserIOPAccessTokenInfoByGuid", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return returnVal;
        }

        public static DataSet GetReSendNoticesDropDownValues()
        {
            try
            {
                DataSet ds;
                List<SqlParameter> parameters = new List<SqlParameter>();
                ds = InfoAccess.ExecuteStoredProcedure("usp_GetReSendNoticesDropDownValues", parameters, "GetReSendNoticesDropDownValues");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetReSendNoticesToGenerateData(int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet ds;
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PageSize", DbType.Int32, pageSize, true));
                parameters.Add(SqlHelper.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, true));
                parameters.Add(SqlHelper.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, true));
                parameters.Add(SqlHelper.CreateParameter("SOPS", DbType.Int32, 0, true));
                ds = InfoAccess.ExecuteStoredProcedure("usp_SelectInsertUpdateDeleteSTG_ReSendEmailNotices", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);

                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertReSendNotices(string RegIDs, string NoticeTypeId, bool paperNotice)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, RegIDs, false));
                parameters.Add(SqlParms.CreateParameter("NOTICES_TYPE_ID", DbType.Int32, Convert.ToInt32(NoticeTypeId), false));
                parameters.Add(SqlParms.CreateParameter("PAPER_NOTICE", DbType.Boolean, paperNotice, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, new Guid("F956B20F-E61F-43DD-AE2F-4322D4A2E75D"), false));
                parameters.Add(SqlHelper.CreateParameter("SOPS", DbType.Int32, 1, true));
                DataAccess.ExecuteScalar("usp_SelectInsertUpdateDeleteSTG_ReSendEmailNotices", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteAllReSendNotices(string IDs)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("IDs", DbType.String, IDs, false));
                parameters.Add(SqlHelper.CreateParameter("SOPS", DbType.Int32, 4, true));
                DataAccess.ExecuteScalar("usp_SelectInsertUpdateDeleteSTG_ReSendEmailNotices", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteReSendNotices(int Index)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, Index, false));
                parameters.Add(SqlHelper.CreateParameter("SOPS", DbType.Int32, 3, true));
                DataAccess.ExecuteScalar("usp_SelectInsertUpdateDeleteSTG_ReSendEmailNotices", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool DoesStateAbbrevExist(string stateAbbrev)
        {
            bool exists = false;

            using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString()))
            using (SqlCommand cmd = new SqlCommand("usp_Check_STATE_ABBREV_Exists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Input parameter
                cmd.Parameters.Add(new SqlParameter("@STATE_ABBREV", SqlDbType.NVarChar, 10)
                {
                    Value = stateAbbrev
                });

                // Output parameter
                SqlParameter outputParam = new SqlParameter("@Exists", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                // Read output value
                exists = Convert.ToBoolean(outputParam.Value);
            }

            return exists;
        }
    }
}
