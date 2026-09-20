using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Linq;
using Corp.Core.Libraries;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MAXIMUS.Controllers.PDMS
{
    public static class RegistrationController
    {
        public static int InsertRegistration(string formCompletionName, string formCompletionPhone,
            int registrationStatusTypeId, int diddReferralId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FORM_COMPLETION_NAME", DbType.String, formCompletionName, true));
                parameters.Add(SqlParms.CreateParameter("FORM_COMPLETION_PHONE", DbType.String, formCompletionPhone, true));
                parameters.Add(SqlParms.CreateParameter("REGISTRATION_STATUS_TYPE_ID", DbType.Int32, registrationStatusTypeId, true));
                if (diddReferralId > 0)
                    parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                int regId = Convert.ToInt32(DataAccess.ExecuteScalar("insertREGISTRATION", parameters));

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, changedBy, false));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, changedDate, false));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, changedBy, false));
                DataAccess.ExecuteStoredProcedure("usp_InsertRegistrationUserXref", parameters);
                return regId;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertRegistrationUserXref(string regId, string userId, DateTime createdDateTime, string createdBy)
        {
            /* In this situation, an external provider user is creating an account.
             * The registration exists from conversion data. 
             * So, the registration is not tied to a user. 
             */
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, false));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, userId, false));
                DataAccess.ExecuteStoredProcedure("usp_InsertRegistrationUserXref", parameters);
                return Convert.ToInt32(regId);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int OnlyInsertRegistrationUserXref(string regId, string userId, DateTime createdDateTime, string createdBy)
        {
            /* In this situation, an external provider user is creating an account.
             * The registration exists from conversion data. 
             * So, the registration is not tied to a user. 
             */
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, false));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, userId, false));
                DataAccess.ExecuteStoredProcedure("usp_Insert_RegistrationUserXref", parameters);
                return Convert.ToInt32(regId);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateRegistrationUserXref(string regId, string userId, DateTime createdDateTime, string createdBy)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, false));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, userId, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateRegistrationUserXref", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateRegistrationStatusType(int regID, int statusTypeID, DateTime changedOn, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("REGISTRATION_STATUS_TYPE_ID", DbType.Int32, statusTypeID, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedOn, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, false));
                DataAccess.ExecuteStoredProcedure("updateREGISTRATION", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateRegistration(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                DataAccess.ExecuteStoredProcedure("updateREGISTRATION", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateRegistrationCustom(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                DataAccess.ExecuteStoredProcedure("updateREGISTRATIONCustom", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateCMCLinksVisibility(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                DataAccess.ExecuteStoredProcedure("usp_UpdateCMCLinksVisibility", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveRegistrationProgramStatus(int regId, int regProgramStatusTypeId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("REG_PROGRAM_STATUS_TYPE_ID", DbType.Int32, regProgramStatusTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_SaveRegistrationProgramStatus", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteRegistrationUserXref(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataAccess.ExecuteStoredProcedure("usp_DeleteREGISTRATION_USER_XREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void RemoveReceiveACHData(int regAchRequestId, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ACH_REQUEST_ID", DbType.Int32, regAchRequestId, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, false));
                DataAccess.ExecuteStoredProcedure("usp_Remove_Receive_ACH_Data", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static string CheckRegistrationExistsByRegId(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regId, true));
                string strMsg = DataAccess.ExecuteScalar("usp_CheckRegistrationExistsByRegId", parameters);
                return strMsg;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static void RemoveQuestionData(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataAccess.ExecuteStoredProcedure("usp_Remove_Question_Data", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegistration(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATION", parameters, "Registration_" + regId);
                ds.Tables[0].TableName = "Registration";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectReg_License_Document(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATION", parameters, "LicDocument_" + regId);
                ds.Tables[0].TableName = "LicDocument";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void DeleteLicense_Document(int documentID, int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataAccess.ExecuteStoredProcedure("usp_DeleteLicense_Document", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertDocumentIndex(int documentId, string docXrefType, string indexId, string userName)
        {
            int documentIndexId = -1;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentId, true));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_XREF_TYPE_ID", DbType.String, docXrefType, true));
                parameters.Add(SqlParms.CreateParameter("INDEXID", DbType.String, indexId.Trim(), true));
                parameters.Add(SqlParms.CreateParameter("CREATED_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_MODIFIED_USER", DbType.Guid, userName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userName, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("insertDOCUMENT_INDEX", parameters, "DocumentIndex");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    documentIndexId = Convert.ToInt32(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Documentid: " + documentId.ToString() +
                    ", InedId: " + indexId + " - " + ex.Message + " - " + ex.StackTrace));
            }
            return documentIndexId;
        }

        public static DataSet GetDocumentIndex(string indexIds)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("IndexIds", DbType.String, indexIds, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetDocumentIndexByIndexIds", parameters, "DocumentIndex");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegistrationByUserId(string userId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByUserId", parameters, "Registration_" + userId);
                ds.Tables[0].TableName = "Registration";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchRegistration(string taxId, string npi, string ssn, string userId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(taxId)) parameters.Add(SqlParms.CreateParameter("TAX_ID", DbType.String, taxId, true));
                if (!string.IsNullOrEmpty(npi)) parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                if (!string.IsNullOrEmpty(ssn)) parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, ssn, true));
                if (!string.IsNullOrEmpty(userId)) parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SearchRegistration", parameters, "Registration_" + userId);
                ds.Tables[0].TableName = "Registration";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Search the Registrations for a specific NPI, TaxID (and specialty if applicable) and UserId combination
        public static DataSet FindRegistrationNPIDuplicate(string npi, string taxID, string userId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, true));
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_FindRegistrationNPIDuplicate", parameters, "Registration_" + userId);
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetDeARecordRegistrationDetails(string DEANumber)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DEANumber", DbType.String, DEANumber, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetDEARecordRegistrationDetails", parameters, "DEARecordRegistration");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static string GetEnrollmenDateforNPI(string npiNumber, string state)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npiNumber, false));
                parameters.Add(SqlParms.CreateParameter("STATE", DbType.String, state, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("USP_GETENROLLMENTDATEFORNPI", parameters, "EnrollmenDateforNPI");
                ds.Tables[0].TableName = "EnrollmenDateforNPI";
                if (ObjectControllerHelper.HasRows(ds))
                {
                    return ds.Tables["EnrollmenDateforNPI"].Rows[0]["STATUS_EFF_DT"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("NPI: " + npiNumber.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet SelectRegSectionStatusCMC(int regId, int workFlowId, string tableName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("WORKFLOW_ID", DbType.Int32, workFlowId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SECTION_STATUS_CMC", parameters, "section_status");
                ds.Tables[0].TableName = "section_status";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet SelectRegistrationApplicationVersion(int regID,int versionID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("VERSION_ID", DbType.Int32, versionID, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_APPLICATION_VersionTables", parameters, "application_version");
                ds.Tables[0].TableName = "application_version";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regID.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectRegistrationData(int regId, string tableName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Only add the RegistrationId if it is greater than -1
                if (regId > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                if (tableName == "Application")
                {

                    parameters.Add(SqlParms.CreateParameter("Primay_AddressTypeId", DbType.Int32, CON.AddressType.PrimaryContact, true));
                    parameters.Add(SqlParms.CreateParameter("Primay_ServiceTypeId", DbType.Int32, CON.AddressType.PrimaryPractice, true));
                    parameters.Add(SqlParms.CreateParameter("BillingTypeId", DbType.Int32, CON.AddressType.Billing, true));
                    parameters.Add(SqlParms.CreateParameter("CorrespondenceTypeId", DbType.Int32, CON.AddressType.Correspondence, true));
                    parameters.Add(SqlParms.CreateParameter("OtherTypeId", DbType.Int32, CON.AddressType.Other, true));
                    parameters.Add(SqlParms.CreateParameter("W9TypeId", DbType.Int32, CON.AddressType.FaxFormW9, true));
                    parameters.Add(SqlParms.CreateParameter("RemittanceTypeId", DbType.Int32, CON.AddressType.Remittance, true));
                    parameters.Add(SqlParms.CreateParameter("Home_Office_AddressId", DbType.Int32, CON.AddressType.HomeOffice, true));
                    parameters.Add(SqlParms.CreateParameter("One_AddressId", DbType.Int32, CON.AddressType.TaxForm1099, true));
                    parameters.Add(SqlParms.CreateParameter("ProfessionalAddressId", DbType.Int32, CON.AddressType.ProfessionalLicenseAddress, true));
                    parameters.Add(SqlParms.CreateParameter("LongTermCareAddressId", DbType.Int32, CON.AddressType.NursingFacility, true));
                    parameters.Add(SqlParms.CreateParameter("BHQuestions", DbType.String, "BH", true));
                }
                string storedProc = "";
                switch (tableName.ToLower())
                {
                    case "provider": storedProc = "usp_SelectREG_PROVIDER"; break;
                    case "application": storedProc = "usp_SelectREG_Application"; break;
                    case "question": storedProc = "usp_SelectREG_QUESTION"; break;
                    case "owner_xref_history": storedProc = "usp_SelectREG_OWNER_XREF_History"; break;
                    case "owner_other": storedProc = "usp_SelectREG_OWNER_OTHER"; break;
                    case "owner_conviction": storedProc = "usp_SelectREG_OWNER_CONVICTION"; break;
                    case "appeal": storedProc = "usp_SelectREG_APPEAL"; break;
                    case "application_fee": storedProc = "usp_SelectREG_APPLICATION_FEE"; break;
                    case "enrollment": storedProc = "usp_SelectREG_ENROLLMENT"; break;
                    case "ach_request_contact_custom": storedProc = "usp_SelectREG_ACH_REQUEST_CONTACT_CUSTOM"; break;
                    case "ach_request": storedProc = "usp_SelectREG_ACH_REQUEST"; break;
                    case "ach_request_history": storedProc = "usp_SelectREG_ACH_REQUEST_History"; break;
                    case "service_location": storedProc = "usp_SelectREG_SERVICE_LOCATION"; break;
                    case "section_status": storedProc = "usp_SelectREG_SECTION_STATUS"; break;
                    case "page_status": storedProc = "usp_SelectREG_PAGE_STATUS"; break;
                    case "specialty": storedProc = "usp_SelectREG_SPECIALTY"; break;
                    case "specialty_active": storedProc = "usp_SelectREG_SPECIALTY_Active"; break;
                    case "contracts": storedProc = "usp_SelectREG_CONTRACTS"; break;
                    case "insurance": storedProc = "usp_SelectREG_INSURANCE"; break;
                    case "affiliation": storedProc = "usp_SelectREG_affiliation"; break;
                    case "incident_compliance_case_xref": storedProc = "usp_SelectREG_INCIDENT_COMPLIANCE_CASE_XREF"; break;
                    case "hearing_rights": storedProc = "usp_SelectREG_hearing_rights"; break;
                    case "ach_reliacard": storedProc = "usp_SelectREG_ACH_RELIACARD"; break;
                    case "ach_fee_information": storedProc = "usp_SelectREG_ACH_FEE_INFORMATION"; break;
                    case "ach_contact": storedProc = "usp_SelectREG_ACH_CONTACTCustom"; break;
                    case "ach_contact_history": storedProc = "usp_SelectREG_ACH_CONTACT_History"; break;
                    case "ach_edison": storedProc = "usp_SelectREG_ACH_EDISON"; break;
                    case "addtl_specialty": storedProc = "usp_SelectREG_ADDTL_SPECIALTY"; break;
                    case "specialty_history": storedProc = "usp_SelectREG_SPECIALTY_History"; break;
                    case "addtl_taxonomy": storedProc = "usp_SelectREG_ADDTL_TAXONOMY"; break;
                    case "taxonomy_history": storedProc = "usp_SelectREG_TAXONOMY_History"; break;
                    case "appeal_notice": storedProc = "usp_SelectREG_APPEAL_NOTICE"; break;
                    case "background_check": storedProc = "usp_SelectREG_BACKGROUND_CHECK"; break;
                    case "behavioralhealthinfo": storedProc = "usp_SelectREG_BehavioralHealthInfo"; break;
                    case "service_location_history": storedProc = "usp_SelectREG_SERVICE_LOCATION_History"; break;
                    case "board_certification": storedProc = "usp_SelectREG_BOARD_CERTIFICATION"; break;
                    case "board_certification_history": storedProc = "usp_SelectREG_BOARD_CERTIFICATION_History"; break;
                    case "buildinghistory": storedProc = "usp_SelectREG_BuildingHistory"; break;
                    case "category_of_service_info": storedProc = "usp_SelectREG_CATEGORY_OF_SERVICE_INFO"; break;
                    case "state_cds_number": storedProc = "usp_SelectREG_STATE_CDS_NUMBER"; break;
                    case "state_cds_number_history": storedProc = "usp_SelectREG_STATE_CDS_NUMBER_HISTORY"; break;
                    case "certification": storedProc = "usp_SelectREG_CERTIFICATION"; break;
                    case "provider_history": storedProc = "usp_SelectREG_PROVIDER_History"; break;
                    case "dea": storedProc = "usp_SelectREG_DEA"; break;
                    case "deahistory": storedProc = "usp_SelectREG_DEAHistory"; break;
                    case "certification_history": storedProc = "usp_SelectREG_CERTIFICATION_History"; break;
                    case "clia": storedProc = "usp_SelectREG_CLIA"; break;
                    case "cliahistory": storedProc = "usp_SelectREG_CLIAHistory"; break;
                    case "chop_parent": storedProc = "usp_SelectREG_CHOP_PARENT"; break;
                    case "submitdate": storedProc = "usp_SelectREG_SubmitDate"; break;
                    case "cpr_certification": storedProc = "usp_SelectREG_CPR_Certification"; break;
                    case "firstaid_certification": storedProc = "usp_SelectREG_FIRSTAID_CERTIFICATION"; break;
                    case "credentialing": storedProc = "usp_SelectREG_CREDENTIALING"; break;
                    case "licenses": storedProc = "usp_SelectREG_LICENSES"; break;
                    case "dme_accreditation_agency": storedProc = "usp_SelectREG_DME_ACCREDITATION_AGENCY"; break;
                    case "dme_personnel_info": storedProc = "usp_SelectREG_DME_PERSONNEL_INFO"; break;
                    case "dme_background_chk_professional_info": storedProc = "usp_SelectREG_DME_BACKGROUND_CHK_PROFESSIONAL_INFO"; break;
                    case "dme_product_category_info": storedProc = "usp_SelectREG_DME_PRODUCT_CATEGORY_INFO"; break;
                    case "dme_registered_agent": storedProc = "usp_SelectREG_DME_REGISTERED_AGENT"; break;
                    case "dme_registered_agent_history": storedProc = "usp_SelectREG_DME_REGISTERED_AGENT_History"; break;
                    case "dme_product_category_info_history": storedProc = "usp_SelectREG_DME_PRODUCT_CATEGORY_INFO_History"; break;
                    case "educationhistory": storedProc = "usp_SelectREG_EDUCATIONHistory"; break;
                    case "enrollmenthistory": storedProc = "usp_SelectREG_EnrollmentHistory"; break;
                    case "delegate_credentialing": storedProc = "usp_SelectREG_DELEGATE_CREDENTIALING"; break;
                    case "household_member": storedProc = "usp_SelectREG_HOUSEHOLD_MEMBER"; break;
                    case "licenseshistory": storedProc = "usp_SelectREG_LICENSESHistory"; break;
                    case "malpractice_claim_history": storedProc = "usp_SelectREG_MALPRACTICE_CLAIM_History"; break;
                    case "mcp_affiliation_careplan": storedProc = "usp_SelectREG_MCP_AFFILIATION_CAREPLAN"; break;
                    case "mco_affiliation": storedProc = "usp_SelectREG_MCO_AFFILIATION"; break;
                    case "medicaid": storedProc = "usp_SelectREG_MEDICAID"; break;
                    case "medicaidhistory": storedProc = "usp_SelectREG_MEDICAIDHistory"; break;
                    case "medicare": storedProc = "usp_SelectREG_MEDICARE"; break;
                    case "medicarehistory": storedProc = "usp_SelectREG_MEDICAREHistory"; break;
                    case "mspcostreportregxref": storedProc = "usp_SelectREG_MSPCostReportREGXref"; break;
                    case "provider_notecustom": storedProc = "usp_SelectREG_PROVIDER_NOTEcustom"; break;
                    case "nursing_professional_certification": storedProc = "usp_SelectREG_Nursing_Professional_Certification"; break;
                    case "owner_paper_provider": storedProc = "usp_SelectREG_owner_paper_provider"; break;
                    case "orientation": storedProc = "usp_SelectREG_ORIENTATION"; break;
                    case "evv_training": storedProc = "usp_SelectREG_EVV_TRAINING"; break;
                    case "owner_type": storedProc = "usp_SelectREG_OWNER_TYPE"; break;
                    case "owner": storedProc = "usp_SelectREG_OWNER"; break;
                    case "owner_history": storedProc = "usp_SelectREG_OWNER_History"; break;
                    case "subcontractor": storedProc = "usp_SelectREG_SUBCONTRACTOR"; break;
                    case "pharmacy_provider_info": storedProc = "usp_SelectREG_PHARMACY_PROVIDER_INFO"; break;
                    case "pharmacist_info": storedProc = "usp_SelectREG_PHARMACIST_INFO"; break;
                    case "npi_history": storedProc = "usp_SelectREG_NPI_HISTORY"; break;
                    case "event_info": storedProc = "usp_SelectREG_EVENT_INFO"; break;
                    case "reconsideration": storedProc = "usp_SelectREG_RECONSIDERATION"; break;
                    case "groupmemberrequiresretroreview": storedProc = "usp_SelectREG_GroupMemberRequiresRetroReview"; break;
                    case "reimbursement": storedProc = "usp_SelectREG_REIMBURSEMENT"; break;
                    case "restriction": storedProc = "usp_SelectREG_RESTRICTION"; break;
                    case "satellite_location_history": storedProc = "usp_SelectREG_SATELLITE_LOCATION_History"; break;
                    case "background_verification": storedProc = "usp_SelectREG_BACKGROUND_VERIFICATION"; break;
                    case "costreportregxref": storedProc = "usp_SelectREG_CostReportREGXref"; break;
                    case "useraccountinfo": storedProc = "usp_SelectREG_UserAccountInfo"; break;
                    case "dds": storedProc = "usp_SelectREG_DDS"; break;
                    case "specialtyservices": storedProc = "usp_SelectREG_SPECIALTYServices"; break;
                    case "providerworkhistory": storedProc = "usp_SelectREG_ProviderWorkHistory"; break;
                    case "ltc_risk_alert": storedProc = "usp_SelectREG_LTC_RISK_ALERT"; break;
                    case "owner_xref": storedProc = "usp_SelectREG_OWNER_XREF"; break;
                    case "owner_conviction_on_behalf": storedProc = "usp_SelectREG_owner_conviction_on_behalf"; break;
                    case "owner_sanction": storedProc = "usp_SelectREG_owner_sanction"; break;
                    case "original_owner": storedProc = "usp_SelectREG_original_owner"; break;
                    case "subcontractor5yrs": storedProc = "usp_SelectREG_SUBCONTRACTOR5yrs"; break;
                    case "owner_residency": storedProc = "usp_SelectREG_owner_residency"; break;
                    case "owner_transaction": storedProc = "usp_SelectREG_owner_transaction"; break;
                    case "subcontractor_history": storedProc = "usp_SelectREG_subcontractor_history"; break;
                    case "owner_sanction_history": storedProc = "usp_SelectREG_owner_sanction_history"; break;
                    case "owner_residency_history": storedProc = "usp_SelectREG_owner_residency_history"; break;
                    case "owner_conviction_history": storedProc = "usp_SelectREG_owner_conviction_history"; break;
                    case "owner_other_history": storedProc = "usp_SelectREG_owner_other_history"; break;
                    case "pharmacy_provider_history": storedProc = "usp_SelectREG_PharmacyProviderHistory"; break;

                    default: throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                DataSet ds = new DataSet();
                Logging log = new Logging();
                //log.CreateLogEntry("Stored proc being called is " + storedProc, Logging.LogPriority.Error);
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "RegistrationData_" + tableName);
                ds.Tables[0].TableName = "RegistrationData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }



        public static DataSet SelectAddressCustomData(int regId, int addresstype, string tableName)
        {
            try
            {
                DataSet ds = new DataSet();
                if (regId > -1)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                    parameters.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, addresstype, true));
                    string storedProc = "";
                    switch (tableName.ToLower())
                    {
                        case "addresscustom": storedProc = "usp_SelectREG_ADDRESSCustom"; break;
                        case "addresscustomnew": storedProc = "usp_SelectREG_ADDRESSCustomnew"; break;
                        default:
                            throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                    }
                    Logging log = new Logging();
                    //log.CreateLogEntry("Stored proc being called is " + storedProc, Logging.LogPriority.Error);
                    ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "RegistrationData_" + tableName);
                    ds.Tables[0].TableName = "RegistrationData";

                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectAddressCustomDataPagingNew(int regId, int addresstype, int PageNumber, int RowsPerPage)
        {
            try
            {
                DataSet ds = new DataSet();
                if (regId > -1)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                    parameters.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, addresstype, true));
                    parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, PageNumber, false));
                    parameters.Add(SqlParms.CreateParameter("RowsPerPage", DbType.Int32, RowsPerPage, false));
                    ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDRESSCustomPagingNew", parameters, "RegistrationData_" + "REG_ADDRESSCustomPagingNew");
                    ds.Tables[0].TableName = "RegistrationData";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAddressADDRESSCustomByID(int regId, int addresstype, int regAddrID)
        {
            try
            {
                DataSet ds = new DataSet();
                if (regId > -1)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                    parameters.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, addresstype, true));
                    parameters.Add(SqlParms.CreateParameter("REG_ADDRESS_ID", DbType.Int32, regAddrID, false));
                    ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDRESSCustomByID", parameters, "RegistrationData_" + "REG_ADDRESSCustomPagingNew");
                    ds.Tables[0].TableName = "RegistrationAddressDataByID";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAddressCustomDataSortedNew(int regId, int addresstype, int PageNumber, int RowsPerPage, string sortOrder, string sortColumn)
        {
            try
            {
                DataSet ds = new DataSet();
                if (regId > -1)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                    parameters.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, addresstype, true));
                    parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, PageNumber, false));
                    parameters.Add(SqlParms.CreateParameter("RowsPerPage", DbType.Int32, RowsPerPage, false));
                    parameters.Add(SqlParms.CreateParameter("SortOrder", DbType.String, sortOrder, false));
                    parameters.Add(SqlParms.CreateParameter("SortColumn", DbType.String, sortColumn, false));
                    ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDRESSCustomSortedNew", parameters, "RegistrationData_" + "REG_ADDRESSCustomSortedNew");
                    ds.Tables[0].TableName = "RegistrationData";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        //check if enrollment is active or not 
        public static bool IsEnrollmentActive(string npi, string providerType)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_NAME", DbType.String, providerType, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_Check_Enrollment_Status";
                return Convert.ToBoolean(DataAccess.ExecuteScalar(storedProc, parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Get the Registration Audit data
        public static DataSet SelectRegistrationAuditData(int regId, string tableName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("TableName", DbType.String, tableName, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectREG_AUDIT_History";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "RegistrationData_" + tableName);
                ds.Tables[0].TableName = "RegistrationData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertRegSpecialityByMMIS(int regId, string MMIS_Code, string LAST_MODIFIED_USER)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("MMIS_SPECIALITY_ID", DbType.String, MMIS_Code, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, LAST_MODIFIED_USER, true));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASONS_ID", DbType.Int32, CON.EnrollStatusReasonID.Active, true));
                parameters.Add(SqlParms.CreateParameter("ENROLL_STATUS_ID", DbType.Int32, CON.EnrollStatus.ACTIVE, true));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_INSERT_REG_SPECIALTY", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", MMIS_Code: " + MMIS_Code.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectRegistrationDataWithParams(string storedProc, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value)) parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    else parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }
                string storedprocname = "";
                DataSet ds = new DataSet();
                switch (storedProc.ToLower())
                {
                    case "usp_selectdsterminated": storedprocname = "usp_selectdsterminated"; break;
                    case "usp_selectregaffilationduplicate": storedprocname = "usp_selectregaffilationduplicate"; break;
                    case "usp_selectreg_form_1099_info": storedprocname = "usp_selectreg_form_1099_info"; break;
                    case "usp_selectreg_page_section_setting": storedprocname = "usp_selectreg_page_section_setting"; break;
                    case "usp_selectreg_page_setting": storedprocname = "usp_selectreg_page_setting"; break;
                    case "usp_selectwf_task_page_action": storedprocname = "usp_selectwf_task_page_action"; break;
                    case "usp_selectregistrationheader": storedprocname = "usp_selectregistrationheader"; break;
                    case "usp_searchallproviderbynpi": storedprocname = "usp_searchallproviderbynpi"; break;
                    case "usp_selectreg_affiliationbyaffiliationid": storedprocname = "usp_selectreg_affiliationbyaffiliationid"; break;
                    case "usp_selectreg_owner": storedprocname = "usp_selectreg_owner"; break;
                    case "usp_selectfederaldealicenseidbylicenseidregid": storedprocname = "usp_selectfederaldealicenseidbylicenseidregid"; break;
                    case "usp_selectcertification_accreditation_status_type": storedprocname = "usp_selectcertification_accreditation_status_type"; break;
                    case "usp_selectcertification_action_type": storedprocname = "usp_selectcertification_action_type"; break;
                    case "usp_selectcertification_eligibility_type": storedprocname = "usp_selectcertification_eligibility_type"; break;
                    case "usp_selectcertification_ltc_bed_breakdown_type": storedprocname = "usp_selectcertification_ltc_bed_breakdown_type"; break;
                    case "usp_selectclialicenseidbylicenseidregid": storedprocname = "usp_selectclialicenseidbylicenseidregid"; break;
                    case "usp_selectclia_certificate_infobyclianumber": storedprocname = "usp_selectclia_certificate_infobyclianumber"; break;
                    case "usp_selectreg_address_history": storedprocname = "usp_selectreg_address_history"; break;
                    case "usp_selectreg_address_section_history": storedprocname = "usp_selectreg_address_section_history"; break;
                    case "usp_selectreg_specialty": storedprocname = "usp_selectreg_specialty"; break;
                    case "usp_selectcpcprovidereligibility": storedprocname = "usp_selectcpcprovidereligibility"; break;
                    case "usp_selectreg_credentialing_contact": storedprocname = "usp_selectreg_credentialing_contact"; break;
                    case "usp_selectreg_credentialing_contact_history": storedprocname = "usp_selectreg_credentialing_contact_history"; break;
                    case "usp_selectreg_board_certification": storedprocname = "usp_selectreg_board_certification"; break;
                    case "usp_selectreg_workhistory": storedprocname = "usp_selectreg_workhistory"; break;
                    case "usp_selectreg_workhistory_history": storedprocname = "usp_selectreg_workhistory_history"; break;
                    case "usp_selectreg_workgaps": storedprocname = "usp_selectreg_workgaps"; break;
                    case "usp_selectreg_education": storedprocname = "usp_selectreg_education"; break;
                    case "usp_selectreg_dme_registered_agent": storedprocname = "usp_selectreg_dme_registered_agent"; break;
                    case "usp_selectreg_dme_product_category_info_byid": storedprocname = "usp_selectreg_dme_product_category_info_byid"; break;
                    case "usp_selectreg_program_status_type": storedprocname = "usp_selectreg_program_status_type"; break;
                    case "usp_wf_selecttasknames": storedprocname = "usp_wf_selecttasknames"; break;
                    case "usp_selectreg_household_member_byid": storedprocname = "usp_selectreg_household_member_byid"; break;
                    case "usp_selectregaffilationfororp": storedprocname = "usp_selectregaffilationfororp"; break;
                    case "usp_selectreg_household_member_address_history": storedprocname = "usp_selectreg_household_member_address_history"; break;
                    case "usp_selectreg_household_member_address_history_byid": storedprocname = "usp_selectreg_household_member_address_history_byid"; break;
                    case "usp_selectreg_household_member_criminal_history": storedprocname = "usp_selectreg_household_member_criminal_history"; break;
                    case "usp_selectreg_household_member_criminal_history_byid": storedprocname = "usp_selectreg_household_member_criminal_history_byid"; break;
                    case "usp_selectreg_insurance_history": storedprocname = "usp_selectreg_insurance_history"; break;
                    case "usp_selectreg_licenseidbylicenseidregid": storedprocname = "usp_selectreg_licenseidbylicenseidregid"; break;
                    case "usp_selectreg_endorsement_specialty_focus": storedprocname = "usp_selectreg_endorsement_specialty_focus"; break;
                    case "usp_selectreg_mco_affiliationbymcoaffiliationid": storedprocname = "usp_selectreg_mco_affiliationbymcoaffiliationid"; break;
                    case "usp_selectreg_medicarebyid": storedprocname = "usp_selectreg_medicarebyid"; break;
                    case "usp_selectreg_subcontractor_history": storedprocname = "usp_selectreg_subcontractor_history"; break;
                    case "usp_selectowneraddressinfo": storedprocname = "usp_selectowneraddressinfo"; break;
                    case "usp_selectrealestateowneraddressinfo": storedprocname = "usp_selectrealestateowneraddressinfo"; break;
                    case "usp_selectadditionaldisclosureaddressinfo": storedprocname = "usp_selectadditionaldisclosureaddressinfo"; break;
                    case "usp_selectreg_pharmacist_infobylicenseidregid": storedprocname = "usp_selectreg_pharmacist_infobylicenseidregid"; break;
                    case "usp_selectreg_background_check": storedprocname = "usp_selectreg_background_check"; break;
                    case "usp_selectreg_waiver_services": storedprocname = "usp_selectreg_waiver_services"; break;
                    case "usp_selectreg_specialty_approval": storedprocname = "usp_selectreg_specialty_approval"; break;
                    case "usp_select_retrievereport_type": storedprocname = "usp_select_retrievereport_type"; break;
                    case "usp_selectreg_hrsa340b": storedprocname = "usp_selectreg_hrsa340b"; break;
                    case "usp_selectreg_specialtyhistory": storedprocname = "usp_selectreg_specialtyhistory"; break;
                    case "usp_selectreg_taxonomy": storedprocname = "usp_selectreg_taxonomy"; break;
                    case "usp_selectreg_taxonomyhistory": storedprocname = "usp_selectreg_taxonomyhistory"; break;
                    case "usp_selectreg_specialty_taxonomyhistory": storedprocname = "usp_selectreg_specialty_taxonomyhistory"; break;
                    case "usp_selectreg_specialtyservices_dds": storedprocname = "usp_selectreg_specialtyservices_dds"; break;
                    case "usp_selectreg_page_section_setting_getall": storedprocname = "usp_selectreg_page_section_setting_getall"; break;
                    case "usp_select_referral_by_taxid_zip": storedprocname = "usp_select_referral_by_taxid_zip"; break;
                    case "usp_selectfinancial_pages_getall": storedprocname = "usp_selectfinancial_pages_getall"; break;
                    case "usp_checkcpcindividualmeetkidscriteria": storedprocname = "usp_checkcpcindividualmeetkidscriteria"; break;
                    case "usp_selectreg_addresscustom": storedprocname = "usp_selectreg_addresscustom"; break;
                    case "usp_selectreg_addresssectioncustom": storedprocname = "usp_selectreg_addresssectioncustom"; break;
                    case "usp_selectcmcqualifyingenrollmentcount": storedprocname = "usp_selectcmcqualifyingenrollmentcount"; break;
                    case "usp_selectreg_mco_pg_affiliationbymcoaffiliationid": storedprocname = "usp_selectreg_mco_pg_affiliationbymcoaffiliationid"; break;
                    case "usp_selectreg_closurenotice": storedprocname = "usp_selectreg_closurenotice"; break;
                    case "usp_selectreg_daysnotice": storedprocname = "usp_selectreg_daysnotice"; break;
                    case "usp_select_regenrollmentsforprovider": storedprocname = "usp_select_regenrollmentsforprovider"; break;
                    case "usp_selectreg_credentialing_id": storedprocname = "usp_selectreg_credentialing_id"; break;
                    case "usp_selectreg_1099_address_history": storedprocname = "usp_selectreg_1099_address_history"; break;
                    case "usp_getrestrictedserviceshistory": storedprocname = "usp_getrestrictedserviceshistory"; break;
                    case "usp_selectreg_bhhistory": storedprocname = "usp_selectreg_bhhistory"; break;
                    case "usp_selectreg_ownerhistory": storedprocname = "usp_selectreg_ownerhistory"; break;
                    case "usp_selectreg_orginfo_history": storedprocname = "usp_selectreg_orginfo_history"; break;
                    case "usp_selectreg_cpcpractice_history": storedprocname = "usp_selectreg_cpcpractice_history"; break;
                    case "usp_selectreg_contact_history": storedprocname = "usp_selectreg_contact_history"; break;
                    case "usp_selectreg_pharmacy_provider_info_history": storedprocname = "usp_selectreg_pharmacy_provider_info_history"; break;
                    case "usp_selectmcp_affiliation_history": storedprocname = "usp_selectmcp_affiliation_history"; break;
                    case "usp_selectreg_medicare_history": storedprocname = "usp_selectreg_medicare_history"; break;
                    case "usp_selectreg_medicaid_history": storedprocname = "usp_selectreg_medicaid_history"; break;
                    case "usp_selectreg_pharmacist_info_history": storedprocname = "usp_selectreg_pharmacist_info_history"; break;
                    case "usp_getregspecialtyadditionalfields": storedprocname = "usp_GetRegSpecialtyAdditionalFields"; break;
                    default:
                        throw new ArgumentOutOfRangeException(String.Format("Invalid value provided for: tableName {1}", storedProc.ToLower()));
                }
                ds = DataAccess.ExecuteStoredProcedure(storedprocname, parameters, "RegistrationData_" + Guid.NewGuid().ToString());
                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "RegistrationData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRenderingLocationsByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regID, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectRenderingLocationsByRegID", parameters, "RenderingLocationsByRegID");
                ds.Tables[0].TableName = "RenderingLocationsByRegID";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectQuestionType(string questionTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(questionTypeId))
                    parameters.Add(SqlParms.CreateParameter("QUESTION_TYPE_ID", DbType.String, questionTypeId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectQUESTION_TYPE", parameters, "QuestionType_" + questionTypeId);
                ds.Tables[0].TableName = "QuestionType";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegPageStatus(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PAGE_STATUS", parameters, "RegistrationData_" + Guid.NewGuid().ToString());
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRelationshipTypes()
        {
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectRELATIONSHIP_TYPE");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectRegNotes(int regId, int regPageTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                if (regPageTypeId > 0) parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER_NOTE", parameters, "RegistrationData_" + regId + "_Notes");
                ds.Tables[0].TableName = "RegistrationData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegDocuments(int regId, int regPageTypeId, string regPageSection, string regPageTypeExclusions, int? screeningActivityID, string roleName = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));

                if (regPageTypeId > 0)
                    parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeId, true));

                if (!string.IsNullOrEmpty(regPageTypeExclusions))
                    parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_EXCLUSIONS", DbType.String, regPageTypeExclusions, true));

                if (!string.IsNullOrEmpty(regPageSection))
                    parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, regPageSection, true));

                if (screeningActivityID.HasValue)
                    parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_ID", DbType.String, screeningActivityID.Value, true));

                if (roleName.Length > 0)
                    parameters.Add(SqlParms.CreateParameter("USER_ROLE", DbType.String, roleName, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_DOCUMENT", parameters, "RegistrationData_" + regId + "_Documents");
                ds.Tables[0].TableName = "RegistrationData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegSubcontractors(int regId, int subcontractorTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                if (subcontractorTypeId > 0) parameters.Add(SqlParms.CreateParameter("SUBCONTRACTOR_TYPE_ID", DbType.Int32, subcontractorTypeId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SUBCONTRACTOR", parameters, "RegistrationData_" + regId + "_SubContractors");
                ds.Tables[0].TableName = "RegistrationData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Reg Id: " + regId.ToString() +
                    ", Subcontractor Type Id: " + subcontractorTypeId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static bool VerifyDuplicateTaxonomySpecialtyForReg(int regId, int taxonomyTypeId, int specialtyTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("taxonomy_Type_Id", DbType.Int32, taxonomyTypeId, true));
                parameters.Add(SqlParms.CreateParameter("specialty_Type_Id", DbType.Int32, specialtyTypeId, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_VerifyDuplicate_PrimaryTaxonomySpecialtyForReg", parameters, "RegistrationData_" + regId + "_VerifyDuplicateTaxonomySpecialtyForReg");
                ds.Tables[0].TableName = "RegistrationData";

                return Convert.ToBoolean(int.Parse(ds.Tables[0].Rows[0][0].ToString()));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("VerifyDuplicateTaxonomySpecialtyForReg: " + regId.ToString() +
                    ", taxonomyType Id: " + taxonomyTypeId + ", specialtyType Id: " + specialtyTypeId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static bool VerifyDuplicateTaxonomyForReg(int regId, string taxonomyCode, int regTaxonomyId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("TAXONOMY_CODE", DbType.String, taxonomyCode, true));
                parameters.Add(SqlParms.CreateParameter("REG_TAXONOMY_ID", DbType.Int32, regTaxonomyId, true));

                string returnValue = DataAccess.ExecuteScalar("usp_VerifyDuplicate_TaxonomyForReg", parameters);

                return Convert.ToBoolean(returnValue);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("VerifyDuplicateTaxonomyForReg: " + regId.ToString() +
                    ", taxonomyType Id: " + taxonomyCode + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static bool VerifyActiveSpecialtyForRegBySpecialtyTypeID(int regId, int specialtyTypeId, DateTime startDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("specialty_Type_Id", DbType.Int32, specialtyTypeId, true));
                parameters.Add(SqlParms.CreateParameter("Start_Date", DbType.DateTime, startDate, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_VerifyActive_SpecialtyForRegBySpecialtyTypeID", parameters, "RegistrationData_" + regId + "_VerifyActiveSpecialtyForRegBySpecialtyTypeID");
                ds.Tables[0].TableName = "RegistrationData";

                return Convert.ToBoolean(int.Parse(ds.Tables[0].Rows[0][0].ToString()));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("VerifyActiveSpecialtyForRegBySpecialtyTypeID: " + regId.ToString() +
                    ", specialtyType Id: " + specialtyTypeId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static bool VerifyDuplicateSpecialtyForReg(int regId, int specialtyTypeId, int regspecialtyid, DateTime startDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("specialty_Type_Id", DbType.Int32, specialtyTypeId, true));
                parameters.Add(SqlParms.CreateParameter("Reg_Specialty_Id", DbType.Int32, regspecialtyid, true));
                parameters.Add(SqlParms.CreateParameter("Start_Date", DbType.DateTime, startDate, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_VerifyDuplicate_SpecialtyForReg", parameters, "RegistrationData_" + regId + "_VerifyDuplicateSpecialtyForReg");
                ds.Tables[0].TableName = "RegistrationData";

                return Convert.ToBoolean(int.Parse(ds.Tables[0].Rows[0][0].ToString()));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("VerifyDuplicateSpecialtyForReg: " + regId.ToString() +
                    ", specialtyType Id: " + specialtyTypeId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }
        public static bool VerifyFutureDatedSpecialtyEnrollmentForReg(int regId, int specialtyTypeId, int regspecialtyid, DateTime startDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("specialty_Type_Id", DbType.Int32, specialtyTypeId, true));
                parameters.Add(SqlParms.CreateParameter("Reg_Specialty_Id", DbType.Int32, regspecialtyid, true));
                parameters.Add(SqlParms.CreateParameter("Start_Date", DbType.DateTime, startDate, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_VerifyFutureDated_SpecialtyForReg", parameters, "RegistrationData_" + regId + "_VerifyFutureDatedSpecialtyEnrollmentForReg");
                ds.Tables[0].TableName = "RegistrationData";

                return Convert.ToBoolean(int.Parse(ds.Tables[0].Rows[0][0].ToString()));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("VerifyFutureDatedSpecialtyEnrollmentForReg: " + regId.ToString() +
                    ", specialtyType Id: " + specialtyTypeId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet VerifyIfCreatesInvalidSpecialtySpanForReg(int regId, int specialtyTypeId, int regspecialtyid, DateTime startDate, DateTime endDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE_ID", DbType.Int32, specialtyTypeId, true));
                parameters.Add(SqlParms.CreateParameter("REG_SPECIALTY_ID", DbType.Int32, regspecialtyid, true));
                parameters.Add(SqlParms.CreateParameter("START_DATE", DbType.DateTime, startDate, true));
                parameters.Add(SqlParms.CreateParameter("END_DATE", DbType.DateTime, endDate, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_VerifyIfCreatesInvalidSpecialtySpanForReg", parameters, "RegistrationData_" + regId + "_VerifyIfCreatesInvalidSpecialtySpanForReg");
                ds.Tables[0].TableName = "RegistrationData";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("VerifyFutureDatedSpecialtyEnrollmentForReg: " + regId.ToString() +
                    ", specialtyType Id: " + specialtyTypeId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectRegErrorTypes(int regPageTypeId, string userId, string userRole = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeId, true));
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("UserRole", DbType.String, userRole, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationErrorTypes", parameters, "RegistrationData_" + userId + "_RegErrorTypes");
                ds.Tables[0].TableName = "RegistrationData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Reg Page Type Id: " + regPageTypeId.ToString() +
                    ", User Id: " + userId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int InsertRegServicesType(int regId, int ServiceTypeID, int LicenseNo, int PrimaryLocation, bool IsParticipate, DateTime LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("ServiceTypeID", DbType.Int32, ServiceTypeID, true));
                parameters.Add(SqlParms.CreateParameter("LicenseNo", DbType.Int32, LicenseNo, true));
                parameters.Add(SqlParms.CreateParameter("PrimaryLocation", DbType.Int32, PrimaryLocation, true));
                parameters.Add(SqlParms.CreateParameter("IsParticipate", DbType.Boolean, IsParticipate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, LAST_MODIFIED_DATE_TIME, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, LAST_MODIFIED_USER, true));
                return Convert.ToInt32(DataAccess.ExecuteScalar("insertReg_ServicesType", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Service Type ID: " + ServiceTypeID.ToString() + ", License No: " + LicenseNo +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int UpdateRegServicesType(int regId, int ServiceTypeID, int LicenseNo, int PrimaryLocation, bool IsParticipate,
            DateTime LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("ServiceTypeID", DbType.Int32, ServiceTypeID, true));
                parameters.Add(SqlParms.CreateParameter("LicenseNo", DbType.Int32, LicenseNo, true));
                parameters.Add(SqlParms.CreateParameter("PrimaryLocation", DbType.Int32, PrimaryLocation, true));
                parameters.Add(SqlParms.CreateParameter("IsParticipate", DbType.Boolean, IsParticipate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, LAST_MODIFIED_DATE_TIME, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, LAST_MODIFIED_USER, true));
                return Convert.ToInt32(DataAccess.ExecuteScalar("updateReg_ServicesType", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Service Type ID: " + ServiceTypeID.ToString() + ", License No: " + LicenseNo +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }



        public static DataSet SelectLicTotNumOfBeds(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_selectRegTotalNumBeds", parameters, "TotBeds_" + regId);
                ds.Tables[0].TableName = "TotBeds";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectReg_Dental_Licenses(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_DENTAL_LICENSES", parameters, "DentalLicenseRecords_" + regId);
                ds.Tables[0].TableName = "DentalLicenseRecords";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectReg_Dental_LicenseType(int regDentalId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_DENTAL_LICENSE_ID", DbType.Int32, regDentalId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("[selectREG_DENTAL_LICENSETYPE_LICENSEID]", parameters, "DentalLicenseRecords_" + regDentalId);
                ds.Tables[0].TableName = "DentalLicenseRecords";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regDentalId.ToString() + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectVisionProvidersByID(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_VISION_PROVIDERS_DETAILS_ByID", parameters, "VisionData_" + regId);

                ds.Tables[0].TableName = "VisionData";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPharmacyProvidersByID(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PHARMACY_PROVIDERS_DETAILS_ByID", parameters, "PharmacyData_" + regId);

                ds.Tables[0].TableName = "PharmacyProviderData";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectACH_Fee(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectACH_FEE_Info", parameters, "FeeInfo_" + regId);
                ds.Tables[0].TableName = "FeeInfo";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectAgreementInitials(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_AGREEMENT_INITIALS", parameters, "Agreements_" + regId);
                ds.Tables[0].TableName = "AgreementInitials";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int InsertRegistrationData(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                string storedproc = string.Empty;
                switch (tableName.ToLower())
                {
                    case "application_fee": storedproc = "insertreg_application_fee"; break;
                    case "ach_request": storedproc = "insertreg_ach_request"; break;
                    case "ach_contact": storedproc = "insertreg_ach_contact"; break;
                    case "ach_reliacard": storedproc = "insertreg_ach_reliacard"; break;
                    case "ach_edison": storedproc = "insertreg_ach_edison"; break;
                    case "background_checkcustom": storedproc = "insertreg_background_checkcustom"; break;
                    case "category_of_service_info": storedproc = "insertreg_category_of_service_info"; break;
                    case "state_cds_number": storedproc = "insertreg_state_cds_number"; break;
                    case "certification": storedproc = "insertreg_certification"; break;
                    case "credentialing_comments": storedproc = "insertreg_credentialing_comments"; break;
                    case "error": storedproc = "insertreg_error"; break;
                    case "dental_licenses": storedproc = "insertreg_dental_licenses"; break;
                    case "dme_personnel_info": storedproc = "insertreg_dme_personnel_info"; break;
                    case "dme_background_chk_professional_info": storedproc = "insertreg_dme_background_chk_professional_info"; break;
                    case "dme_product_category_info": storedproc = "insertreg_dme_product_category_info"; break;
                    case "dme_registered_agent": storedproc = "insertreg_dme_registered_agent"; break;
                    case "ach_fee_information": storedproc = "insertreg_ach_fee_information"; break;
                    case "household_member_address_history": storedproc = "insertreg_household_member_address_history"; break;
                    case "household_member_criminal_history": storedproc = "insertreg_household_member_criminal_history"; break;
                    case "mcp_affiliation_careplancustom": storedproc = "insertreg_mcp_affiliation_careplancustom"; break;
                    case "npi_medicare": storedproc = "insertreg_npi_medicare"; break;
                    case "owner_paper_provider": storedproc = "insertreg_owner_paper_provider"; break;
                    case "providercustom": storedproc = "insertreg_providercustom"; break;
                    case "evv_training": storedproc = "insertreg_evv_training"; break;
                    case "owner_conviction": storedproc = "insertreg_owner_conviction"; break;
                    case "owner_conviction_on_behalf": storedproc = "insertreg_owner_conviction_on_behalf"; break;
                    case "owner_debarred": storedproc = "insertreg_owner_debarred"; break;
                    case "owner_excluded": storedproc = "insertreg_owner_excluded"; break;
                    case "owner": storedproc = "insertreg_owner"; break;
                    case "original_owner": storedproc = "insertreg_original_owner"; break;
                    case "owner_other": storedproc = "insertreg_owner_other"; break;
                    case "owner_sanction": storedproc = "insertreg_owner_sanction"; break;
                    case "subcontractor": storedproc = "insertreg_subcontractor"; break;
                    case "subcontractor_owner": storedproc = "insertreg_subcontractor_owner"; break;
                    case "supplier": storedproc = "insertreg_supplier"; break;
                    case "owner_terminated": storedproc = "insertreg_owner_terminated"; break;
                    case "owner_transaction": storedproc = "insertreg_owner_transaction"; break;
                    case "npi_history": storedproc = "insertreg_npi_history"; break;
                    case "event_info": storedproc = "insertreg_event_info"; break;
                    case "appeal": storedproc = "insertreg_appeal"; break;
                    case "dea": storedproc = "insertreg_dea"; break;
                    case "background_verification": storedproc = "insertreg_background_verification"; break;
                    case "dds": storedproc = "insertreg_dds"; break;
                    case "specialty": storedproc = "insertreg_specialty"; break;
                    case "specialty_category": storedproc = "insertreg_specialty_category"; break;
                    case "specialty_employee": storedproc = "insertreg_specialty_employee"; break;
                    case "specialty_approval": storedproc = "insertreg_specialty_approval"; break;
                    case "board_certification": storedproc = "insertreg_board_certification"; break;
                    case "cpr_certification": storedproc = "insertreg_cpr_certification"; break;
                    case "firstaid_certification": storedproc = "insertreg_firstaid_certification"; break;
                    case "health_care_facility_affiliationcustom": storedproc = "insertreg_health_care_facility_affiliationcustom"; break;
                    case "malpractice_claim": storedproc = "insertreg_malpractice_claim"; break;
                    case "nursing_professional_certification": storedproc = "insertreg_nursing_professional_certification"; break;
                    case "pending_affiliation": storedproc = "insertreg_pending_affiliation"; break;
                    case "service_locationcustom": storedproc = "insertreg_service_locationcustom"; break;
                    case "address": storedproc = "insertREG_ADDRESS"; break;
                    case "office_timingcustom": storedproc = "insertREG_OFFICE_TIMINGcustom"; break;
                    case "credentialing_contact": storedproc = "insertREG_credentialing_contact"; break;
                    case "specialtycustom2": storedproc = "insertREG_specialtycustom2"; break;
                    case "dme_accreditation_agency": storedproc = "insertreg_dme_accreditation_agency"; break;
                    case "insurance": storedproc = "insertreg_insurance"; break;
                    case "document": storedproc = "usp_InsertREG_DOCUMENT"; break;
                    case "education": storedproc = "insertreg_education"; break;
                    case "form_1099_info": storedproc = "insertreg_form_1099_info"; break;
                    case "tax_info": storedproc = "insertreg_tax_info"; break;
                    case "dodd_verification": storedproc = "insertreg_dodd_verification"; break;
                    case "elicense_verification": storedproc = "insertreg_elicense_verification"; break;
                    case "taxonomycustom": storedproc = "insertreg_taxonomycustom"; break;
                    case "specialtycustom": storedproc = "insertreg_specialtycustom"; break;
                    case "affiliationcustom": storedproc = "insertreg_affiliationcustom"; break;
                    case "hearing_rights": storedproc = "insertreg_hearing_rights"; break;
                    case "additional_addresses": storedproc = "insertreg_additional_addresses"; break;
                    case "taxonomy": storedproc = "insertreg_taxonomy"; break;
                    case "appeal_notice": storedproc = "insertreg_appeal_notice"; break;
                    case "behavioralhealthinfo": storedproc = "insertreg_BehavioralHealthInfo"; break;
                    case "clia": storedproc = "insertreg_clia"; break;
                    case "chop_parent": storedproc = "insertreg_chop_parent"; break;
                    case "reg_ltc_risk_alert": storedproc = "insertreg_reg_ltc_risk_alert"; break;
                    case "dme": storedproc = "insertreg_dme"; break;
                    case "delegate_credentialingcustom": storedproc = "insertreg_delegate_credentialingcustom"; break;
                    case "reconsideration": storedproc = "insertreg_reconsideration"; break;
                    case "hospitalcostreport": storedproc = "insertreg_hospitalcostreport"; break;
                    case "mspcostreport": storedproc = "insertreg_mspcostreport"; break;
                    case "household_member": storedproc = "insertreg_household_member"; break;
                    case "medicaidcustom": storedproc = "insertreg_medicaidcustom"; break;
                    case "alternate_id": storedproc = "insertreg_alternate_id"; break;
                    case "medicare": storedproc = "insertreg_medicare"; break;
                    case "cost_report_document": storedproc = "insertreg_cost_report_document"; break;
                    case "mspcostreportdocuments": storedproc = "insertreg_mspcostreportdocuments"; break;
                    case "orientation": storedproc = "insertreg_orientation"; break;
                    case "pharmacist_info": storedproc = "insertreg_pharmacist_info"; break;
                    case "pharmacy_provider_info": storedproc = "insertreg_pharmacy_provider_info"; break;
                    case "reimbursement": storedproc = "insertreg_reimbursement"; break;
                    case "restriction": storedproc = "insertreg_restriction"; break;
                    case "satellite_location": storedproc = "insertreg_satellite_location"; break;
                    case "hrsa340b": storedproc = "insertreg_hrsa340b"; break;
                    case "costreportdocuments": storedproc = "insertreg_costreportdocuments"; break;
                    case "costreport": storedproc = "insertreg_costreport"; break;
                    case "workhistory": storedproc = "insertreg_workhistory"; break;
                    case "workgaps": storedproc = "insertreg_workgaps"; break;
                    case "enrollment": storedproc = "insertreg_enrollment"; break;
                    case "tax_history": storedproc = "insertreg_tax_history"; break;
                    case "licensecustom": storedproc = "insertreg_licensecustom"; break;
                    case "endorsement_specialty_focus": storedproc = "insertreg_endorsement_specialty_focus"; break;
                    case "ama_profile_downloads": storedproc = "insertreg_ama_profile_downloads"; break;
                    case "ach_requestcustom": storedproc = "insertreg_ach_requestcustom"; break;
                    case "provider_feed": storedproc = "insertreg_provider_feed"; break;
                    case "specialty_additional_fields": storedproc = "usp_InsertREG_SPECIALTY_ADDITIONAL_FIELDS"; break;

                    default: throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(storedproc, parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int InsertCPCAttestationData(Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, object> pair in parms)
                {
                    if (pair.Value.GetType() == typeof(DataTable))
                    {
                        SqlParameter Parameter = new SqlParameter();
                        Parameter.ParameterName = "@" + pair.Key;
                        Parameter.SqlDbType = SqlDbType.Structured;
                        Parameter.Value = pair.Value;

                        parameters.Add(Parameter);

                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                DataAccess.ExecuteStoredProcedure("usp_InsertUpdate_CPCATTESTATION", parameters);
                return 1;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int InsertUploadFile(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                DataAccess.ExecuteScalar("usp_InsertDocument_Files", parameters);
                return 1;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void SetAddBCITextRTPEmailFlag(int regId, int specialtyTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE_ID", DbType.Int32, specialtyTypeId, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SetAddBCITextRTPEmailFlag", parameters, "Document");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("In SetAddBCITextRTPEmailFlag REG_ID: " + regId.ToString() +
                   " - " + ex.Message + " - " + ex.StackTrace));
            }

        }

        public static void InsertDentalRegistrationData(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                DataAccess.ExecuteStoredProcedure("insertREG_" + tableName, parameters);


            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }


        public static void UpdateRegistrationDataWithParams(string storedProc, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    /// Bug 3621:  CSR_Check all Address 2 Fields
                    /// When converting empty strings to NULL in parameters, stored procedures are updating to
                    /// old value instead of to empty string. So now when we pass an empty string, it should
                    /// be passed to the stored proc as an empty string, and not as a NULL value.
                    if (pair.Value == null)
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                string storedprocname = "";
                switch (storedProc.ToLower())
                {
                    case "updatereg_address": storedprocname = "updatereg_address"; break;
                    case "updatereg_form_1099_info": storedprocname = "updatereg_form_1099_info"; break;
                    case "updatereg_owner": storedprocname = "updatereg_owner"; break;
                    case "updatereg_tax_info": storedprocname = "updatereg_tax_info"; break;
                    case "updatereg_dodd_verification": storedprocname = "updatereg_dodd_verification"; break;
                    case "updatereg_elicense_verification": storedprocname = "updatereg_elicense_verification"; break;
                    case "updatereg_service_location": storedprocname = "updatereg_service_location"; break;
                    case "updatereg_affiliationcustom": storedprocname = "updatereg_affiliationcustom"; break;
                    case "updatereg_hearing_rights": storedprocname = "updatereg_hearing_rights"; break;
                    case "updatereg_incident_compliance_case_xref": storedprocname = "updatereg_incident_compliance_case_xref"; break;
                    case "updatereg_specialty": storedprocname = "updatereg_specialty"; break;
                    case "updatereg_taxonomy": storedprocname = "updatereg_taxonomy"; break;
                    case "updatereg_appeal_notice": storedprocname = "updatereg_appeal_notice"; break;
                    case "updatereg_providercustom": storedprocname = "updatereg_providercustom"; break;
                    case "updatereg_appeal": storedprocname = "updatereg_appeal"; break;
                    case "updatereg_dea": storedprocname = "updatereg_dea"; break;
                    case "updatereg_clia": storedprocname = "updatereg_clia"; break;
                    case "updatereg_chop_parent": storedprocname = "updatereg_chop_parent"; break;
                    case "updatereg_credentialing_contact": storedprocname = "updatereg_credentialing_contact"; break;
                    case "updatereg_ama_profile_downloads": storedprocname = "updatereg_ama_profile_downloads"; break;
                    case "updatereg_education": storedprocname = "updatereg_education"; break;
                    case "updatereg_household_member": storedprocname = "updatereg_household_member"; break;
                    case "updatereg_incident_compliance_case_detail": storedprocname = "updatereg_incident_compliance_case_detail"; break;

                    case "updatereg_insurance": storedprocname = "updatereg_insurance"; break;
                    case "updatereg_medicaidcustom": storedprocname = "updatereg_medicaidcustom"; break;
                    case "updatereg_medicare": storedprocname = "updatereg_medicare"; break;
                    case "updatereg_alternate_id": storedprocname = "updatereg_alternate_id"; break;
                    case "updatereg_office_timingcustom": storedprocname = "updatereg_office_timingcustom"; break;
                    case "updatereg_orientation": storedprocname = "updatereg_orientation"; break;
                    case "updatereg_document": storedprocname = "updatereg_document"; break;
                    case "updatereg_reconsideration": storedprocname = "updatereg_reconsideration"; break;
                    case "updatereg_service_locationcustom": storedprocname = "updatereg_service_locationcustom"; break;
                    case "updatereg_restriction": storedprocname = "updatereg_restriction"; break;
                    case "updatereg_satellite_location": storedprocname = "updatereg_satellite_location"; break;
                    case "updatereg_application": storedprocname = "updatereg_application"; break;
                    case "updatereg_specialtycustom": storedprocname = "updatereg_specialtycustom"; break;
                    case "updatereg_dds": storedprocname = "updatereg_dds"; break;
                    case "updatereg_workhistory": storedprocname = "updatereg_workhistory"; break;
                    case "updatereg_workgaps": storedprocname = "updatereg_workgaps"; break;
                    case "updatereg_application_fee": storedprocname = "updatereg_application_fee"; break;
                    case "updatereg_ach_request": storedprocname = "updatereg_ach_request"; break;
                    case "updatereg_enrollment": storedprocname = "updatereg_enrollment"; break;
                    case "updatereg_tax_history": storedprocname = "updatereg_tax_history"; break;
                    case "updatereg_licensecustom": storedprocname = "updatereg_licensecustom"; break;
                    case "updatereg_application_feecustom": storedprocname = "updatereg_application_feecustom"; break;
                    case "updatereg_ach_contact": storedprocname = "updatereg_ach_contact"; break;
                    case "updatereg_ach_reliacard": storedprocname = "updatereg_ach_reliacard"; break;
                    case "updatereg_ach_edison": storedprocname = "updatereg_ach_edison"; break;
                    case "updatereg_additional_addresses": storedprocname = "updatereg_additional_addresses"; break;
                    case "updatereg_background_checkcustom": storedprocname = "updatereg_background_checkcustom"; break;
                    case "updatereg_reg_category_of_service_info": storedprocname = "updatereg_reg_category_of_service_info"; break;
                    case "updatereg_state_cds_number": storedprocname = "updatereg_state_cds_number"; break;
                    case "updatereg_certification": storedprocname = "updatereg_certification"; break;
                    case "updatereg_dental_licenses": storedprocname = "updatereg_dental_licenses"; break;
                    case "updatereg_dme_personnel_info": storedprocname = "updatereg_dme_personnel_info"; break;
                    case "updatereg_dme_background_chk_professional_info": storedprocname = "updatereg_dme_background_chk_professional_info"; break;
                    case "updatereg_dme_product_category_info": storedprocname = "updatereg_dme_product_category_info"; break;
                    case "updatereg_dme_registered_agent": storedprocname = "updatereg_dme_registered_agent"; break;
                    case "updatereg_ach_fee_information": storedprocname = "updatereg_ach_fee_information"; break;
                    case "updatereg_household_member_address_history": storedprocname = "updatereg_household_member_address_history"; break;
                    case "updatereg_household_member_criminal_history": storedprocname = "updatereg_household_member_criminal_history"; break;
                    case "updatereg_mcp_affiliation_careplancustom": storedprocname = "updatereg_mcp_affiliation_careplancustom"; break;
                    case "updatereg_owner_paper_provider": storedprocname = "updatereg_owner_paper_provider"; break;
                    case "updatereg_evv_training": storedprocname = "updatereg_evv_training"; break;
                    case "updatereg_owner_conviction": storedprocname = "updatereg_owner_conviction"; break;
                    case "updatereg_owner_conviction_on_behalf": storedprocname = "updatereg_owner_conviction_on_behalf"; break;
                    case "updatereg_owner_debarred": storedprocname = "updatereg_owner_debarred"; break;
                    case "updatereg_owner_excluded": storedprocname = "updatereg_owner_excluded"; break;
                    case "updatereg_original_owner": storedprocname = "updatereg_original_owner"; break;
                    case "updatereg_owner_other": storedprocname = "updatereg_owner_other"; break;
                    case "updatereg_owner_sanction": storedprocname = "updatereg_owner_sanction"; break;
                    case "updatereg_owner_residency": storedprocname = "updatereg_owner_residency"; break;
                    case "updatereg_subcontractor": storedprocname = "updatereg_subcontractor"; break;
                    case "updatereg_subcontractor_owner": storedprocname = "updatereg_subcontractor_owner"; break;
                    case "updatereg_supplier": storedprocname = "updatereg_supplier"; break;
                    case "updatereg_owner_terminated": storedprocname = "updatereg_owner_terminated"; break;
                    case "updatereg_owner_transaction": storedprocname = "updatereg_owner_transaction"; break;
                    case "updatereg_pharmacist_info": storedprocname = "updatereg_pharmacist_info"; break;
                    case "updatereg_pharmacy_provider_info": storedprocname = "updatereg_pharmacy_provider_info"; break;
                    case "updatereg_event_info": storedprocname = "updatereg_event_info"; break;
                    case "updatereg_reimbursement": storedprocname = "updatereg_reimbursement"; break;
                    case "updatereg_background_verfication": storedprocname = "updatereg_background_verfication"; break;
                    case "updatereg_number_of_bedscustom": storedprocname = "updatereg_number_of_bedscustom"; break;
                    case "updatereg_vision_provider_details": storedprocname = "updatereg_vision_provider_details"; break;
                    case "updatereg_specialty_category": storedprocname = "updatereg_specialty_category"; break;
                    case "updatereg_specialty_employee": storedprocname = "updatereg_specialty_employee"; break;
                    case "updatereg_specialty_approval": storedprocname = "updatereg_specialty_approval"; break;
                    case "updatereg_credentialing": storedprocname = "updatereg_credentialing"; break;
                    case "updatereg_board_certificationcustom": storedprocname = "updatereg_board_certificationcustom"; break;
                    case "updatereg_category_of_service_info": storedprocname = "updatereg_category_of_service_info"; break;
                    case "updatereg_cpr_certification": storedprocname = "updatereg_cpr_certification"; break;
                    case "updatereg_firstaid_certification": storedprocname = "updatereg_firstaid_certification"; break;
                    case "updatereg_health_care_facility_affiliationcustom": storedprocname = "updatereg_health_care_facility_affiliationcustom"; break;
                    case "updatereg_malpractice_claim": storedprocname = "updatereg_malpractice_claim"; break;
                    case "updatereg_nursing_professional_certification": storedprocname = "updatereg_nursing_professional_certification"; break;
                    case "updatereg_pending_affiliation": storedprocname = "updatereg_pending_affiliation"; break;
                    case "updatereg_cpc_enrollmentcustom": storedprocname = "updatereg_cpc_enrollmentcustom"; break;
                    case "usp_transferregtolive_ach_edison": storedprocname = "usp_transferregtolive_ach_edison"; break;
                    case "usp_transferregtolive_ach_fee_information": storedprocname = "usp_transferregtolive_ach_fee_information"; break;
                    case "updatereg_ownercustom": storedprocname = "updatereg_ownercustom"; break;
                    case "updatereg_reimbursementcustom": storedprocname = "updatereg_reimbursementcustom"; break;
                    case "usp_savesite_visit_screening": storedprocname = "usp_savesite_visit_screening"; break;
                    case "usp_updatereg_taxonomy_primary": storedprocname = "usp_updatereg_taxonomy_primary"; break;
                    case "usp_updatecpcaffiliation_onreattest": storedprocname = "usp_updatecpcaffiliation_onreattest"; break;
                    case "updatereg_cliacustom": storedprocname = "updatereg_cliacustom"; break;
                    case "usp_updatereg_specialty_enroll_status": storedprocname = "usp_updatereg_specialty_enroll_status"; break;
                    case "updatereg_ach_requestcustom": storedprocname = "updatereg_ach_requestcustom"; break;
                    case "usp_update_reg_provider_odm_credentialing_delegates_checked": storedprocname = "usp_update_reg_provider_odm_credentialing_delegates_checked"; break;
                    case "updatereg_chop_parentcustom": storedprocname = "updateREG_CHOP_PARENTCustom"; break;
                    case "updatereg_npi_medid_enrollment_span": storedprocname = "usp_updateREG_NPI_MEDID_ENROLLMENT_SPANCustom"; break;
                    case "updatereg_provider_feed": storedprocname = "updatereg_provider_feed"; break;
                    case "usp_update_reg_section_status": storedprocname = "usp_Update_REG_Section_Status"; break;
                    default:
                        throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                DataAccess.ExecuteStoredProcedure(storedprocname, parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Stored Procedure: " + storedProc + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void UpdateRegistrationData(string tableName, Dictionary<string, string> parms)
        {
            string storedproc = "";
            switch (tableName.ToLower())
            {
                case "address": storedproc = "updatereg_address"; break;
                case "form_1099_info": storedproc = "updatereg_form_1099_info"; break;
                case "owner": storedproc = "updatereg_owner"; break;
                case "tax_info": storedproc = "updatereg_tax_info"; break;
                case "dodd_verification": storedproc = "updatereg_dodd_verification"; break;
                case "elicense_verification": storedproc = "updatereg_elicense_verification"; break;
                case "service_location": storedproc = "updatereg_service_location"; break;
                case "affiliationcustom": storedproc = "updatereg_affiliationcustom"; break;
                case "hearing_rights": storedproc = "updatereg_hearing_rights"; break;
                case "incident_compliance_case_xref": storedproc = "updatereg_incident_compliance_case_xref"; break;
                case "specialty": storedproc = "updatereg_specialty"; break;
                case "taxonomy": storedproc = "updatereg_taxonomy"; break;
                case "appeal_notice": storedproc = "updatereg_appeal_notice"; break;
                case "providercustom": storedproc = "updatereg_providercustom"; break;
                case "appeal": storedproc = "updatereg_appeal"; break;
                case "dea": storedproc = "updatereg_dea"; break;
                case "clia": storedproc = "updatereg_clia"; break;
                case "chop_parent": storedproc = "updatereg_chop_parent"; break;
                case "credentialing_contact": storedproc = "updatereg_credentialing_contact"; break;
                case "ama_profile_downloads": storedproc = "updatereg_ama_profile_downloads"; break;
                case "education": storedproc = "updatereg_education"; break;
                case "household_member": storedproc = "updatereg_household_member"; break;
                case "incident_compliance_case_detail": storedproc = "updatereg_incident_compliance_case_detail"; break;

                case "insurance": storedproc = "updatereg_insurance"; break;
                case "medicaidcustom": storedproc = "updatereg_medicaidcustom"; break;
                case "medicare": storedproc = "updatereg_medicare"; break;
                case "alternate_id": storedproc = "updatereg_alternate_id"; break;
                case "office_timingcustom": storedproc = "updatereg_office_timingcustom"; break;
                case "orientation": storedproc = "updatereg_orientation"; break;
                case "document": storedproc = "updatereg_document"; break;
                case "reconsideration": storedproc = "updatereg_reconsideration"; break;
                case "service_locationcustom": storedproc = "updatereg_service_locationcustom"; break;
                case "restriction": storedproc = "updatereg_restriction"; break;
                case "satellite_location": storedproc = "updatereg_satellite_location"; break;
                case "application": storedproc = "updatereg_application"; break;
                case "specialtycustom": storedproc = "updatereg_specialtycustom"; break;
                case "dds": storedproc = "updatereg_dds"; break;
                case "workhistory": storedproc = "updatereg_workhistory"; break;
                case "workgaps": storedproc = "updatereg_workgaps"; break;
                case "application_fee": storedproc = "updatereg_application_fee"; break;
                case "ach_request": storedproc = "updatereg_ach_request"; break;
                case "enrollment": storedproc = "updatereg_enrollment"; break;
                case "tax_history": storedproc = "updatereg_tax_history"; break;
                case "licensecustom": storedproc = "updatereg_licensecustom"; break;
                case "application_feecustom": storedproc = "updatereg_application_feecustom"; break;
                case "ach_contact": storedproc = "updatereg_ach_contact"; break;
                case "ach_reliacard": storedproc = "updatereg_ach_reliacard"; break;
                case "ach_edison": storedproc = "updatereg_ach_edison"; break;
                case "additional_addresses": storedproc = "updatereg_additional_addresses"; break;
                case "background_checkcustom": storedproc = "updatereg_background_checkcustom"; break;
                case "reg_category_of_service_info": storedproc = "updatereg_reg_category_of_service_info"; break;
                case "state_cds_number": storedproc = "updatereg_state_cds_number"; break;
                case "certification": storedproc = "updatereg_certification"; break;
                case "dental_licenses": storedproc = "updatereg_dental_licenses"; break;
                case "dme_personnel_info": storedproc = "updatereg_dme_personnel_info"; break;
                case "dme_background_chk_professional_info": storedproc = "updatereg_dme_background_chk_professional_info"; break;
                case "dme_product_category_info": storedproc = "updatereg_dme_product_category_info"; break;
                case "dme_registered_agent": storedproc = "updatereg_dme_registered_agent"; break;
                case "ach_fee_information": storedproc = "updatereg_ach_fee_information"; break;
                case "household_member_address_history": storedproc = "updatereg_household_member_address_history"; break;
                case "household_member_criminal_history": storedproc = "updatereg_household_member_criminal_history"; break;
                case "mcp_affiliation_careplancustom": storedproc = "updatereg_mcp_affiliation_careplancustom"; break;
                case "owner_paper_provider": storedproc = "updatereg_owner_paper_provider"; break;
                case "evv_training": storedproc = "updatereg_evv_training"; break;
                case "owner_conviction": storedproc = "updatereg_owner_conviction"; break;
                case "owner_conviction_on_behalf": storedproc = "updatereg_owner_conviction_on_behalf"; break;
                case "owner_debarred": storedproc = "updatereg_owner_debarred"; break;
                case "owner_excluded": storedproc = "updatereg_owner_excluded"; break;
                case "original_owner": storedproc = "updatereg_original_owner"; break;
                case "owner_other": storedproc = "updatereg_owner_other"; break;
                case "owner_sanction": storedproc = "updatereg_owner_sanction"; break;
                case "owner_residency": storedproc = "updatereg_owner_residency"; break;
                case "subcontractor": storedproc = "updatereg_subcontractor"; break;
                case "subcontractor_owner": storedproc = "updatereg_subcontractor_owner"; break;
                case "supplier": storedproc = "updatereg_supplier"; break;
                case "owner_terminated": storedproc = "updatereg_owner_terminated"; break;
                case "owner_transaction": storedproc = "updatereg_owner_transaction"; break;
                case "pharmacist_info": storedproc = "updatereg_pharmacist_info"; break;
                case "pharmacy_provider_info": storedproc = "updatereg_pharmacy_provider_info"; break;
                case "event_info": storedproc = "updatereg_event_info"; break;
                case "reimbursement": storedproc = "updatereg_reimbursement"; break;
                case "background_verfication": storedproc = "updatereg_background_verfication"; break;
                case "number_of_bedscustom": storedproc = "updatereg_number_of_bedscustom"; break;
                case "vision_provider_details": storedproc = "updatereg_vision_provider_details"; break;
                case "specialty_category": storedproc = "updatereg_specialty_category"; break;
                case "specialty_employee": storedproc = "updatereg_specialty_employee"; break;
                case "specialty_approval": storedproc = "updatereg_specialty_approval"; break;
                case "credentialing": storedproc = "updatereg_credentialing"; break;
                case "board_certificationcustom": storedproc = "updatereg_board_certificationcustom"; break;
                case "category_of_service_info": storedproc = "updatereg_category_of_service_info"; break;
                case "cpr_certification": storedproc = "updatereg_cpr_certification"; break;
                case "firstaid_certification": storedproc = "updatereg_firstaid_certification"; break;
                case "health_care_facility_affiliationcustom": storedproc = "updatereg_health_care_facility_affiliationcustom"; break;
                case "malpractice_claim": storedproc = "updatereg_malpractice_claim"; break;
                case "nursing_professional_certification": storedproc = "updatereg_nursing_professional_certification"; break;
                case "pending_affiliation": storedproc = "updatereg_pending_affiliation"; break;
                case "cpc_enrollmentcustom": storedproc = "updatereg_cpc_enrollmentcustom"; break;
                case "cliacustom": storedproc = "updateREG_CLIACustom"; break;
                case "specialty_enroll_status": storedproc = "usp_UpdateREG_Specialty_Enroll_Status"; break;
                case "ach_requestcustom": storedproc = "updatereg_ach_requestcustom"; break;
                case "chop_parentcustom": storedproc = "updateREG_CHOP_PARENTCustom"; break;
                case "provider_feed": storedproc = "updatereg_provider_feed"; break;

                default:
                    throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
            }
            UpdateRegistrationDataWithParams(storedproc, parms);
        }

        public static void UpateRegDocumentXref(int regId, int documentId, int rowId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("document_Id", DbType.Int32, documentId, true));
                parameters.Add(SqlParms.CreateParameter("row_Id", DbType.Int32, rowId, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_UpdateREG_DOCUMENT_XREF", parameters, "Document");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", documentId: " + documentId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }

        }

        public static void UpdateIncidentAlertFlag(int regId, DateTime changedDate, Guid changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_UpdateIncidentAlertFlag", parameters, "RegIncident");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                     " - " + ex.Message + " - " + ex.StackTrace));
            }

        }

        public static void UpdateORPFlag(int regId, bool flag)
        {
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("Reg_ID", regId));
                sqlParams.Add(new SqlParameter("@Flag", flag));
                DataAccess.ExecuteStoredProcedure("usp_UpdateFlag_ConvertORP", sqlParams, "UpdateORPApplicationFlag");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                     " - " + ex.Message + " - " + ex.StackTrace));
            }

        }

        public static string GetORPFlagforRegID(int regId)
        {
            try
            {
                List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                sqlParms1.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));
                return DataAccess.ExecuteScalar("usp_GetFlag_ConvertORP", sqlParms1);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                     " - " + ex.Message + " - " + ex.StackTrace));
            }
        }


        public static void UpdateIncidentReviewStatus(int regId, int caseStatus, DateTime changedDate, Guid changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("CASE_STATUS", DbType.Int32, caseStatus, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_UpdateIncidentReviewStatus", parameters, "IncidentCaseReview");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                     " - " + ex.Message + " - " + ex.StackTrace));
            }

        }


        public static int InsertRegDocument(int regId, int regPageTypeId, string regPageSection, string name, string description,
            string fileName, DateTime? changedDate, string changedBy, int? screeningActivityID, string wftaskname, string documentUploadPage)
        {
            int docID = -1;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeId, true));
                if (!string.IsNullOrEmpty(regPageSection))
                    parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, regPageSection, true));
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, true));
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, description, true));
                parameters.Add(SqlParms.CreateParameter("WF_TASKNAME", DbType.String, wftaskname, true));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, fileName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                if (screeningActivityID.HasValue)
                    parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_ID", DbType.Int32, screeningActivityID.Value, true));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_UPLOAD_PAGE", DbType.String, documentUploadPage, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_InsertREG_DOCUMENT", parameters, "Document");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    docID = Convert.ToInt32(row[0].ToString());
                }
                return docID;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Reg Page Type Id: " + regPageTypeId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void ProcessGlobalAdminChangeRequest(string currentOHID, string newOHID, int docID, int onBaseDocID, DateTime createdDateTime, string lastModifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CURRENT_ADMIN_OHID", DbType.String, currentOHID, true));
                parameters.Add(SqlParms.CreateParameter("NEW_ADMIN_OHID", DbType.String, newOHID, true));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docID, true));
                parameters.Add(SqlParms.CreateParameter("ONBASE_DOC_ID", DbType.Int32, onBaseDocID, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_DATE_TIME", DbType.DateTime, createdDateTime, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_ProcessGlobalAdminChangeRequest", parameters, "Document");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("ProcessGlobalAdminChangeRequest Document ID: " + docID.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int GetRegistrationIdByUserId(string userId, string currentAdminOHID)
        {
            int regId = -1;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, false));
                parameters.Add(SqlParms.CreateParameter("CURRENT_ADMIN_OHID", DbType.String, currentAdminOHID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetRegistrationIdByUserId", parameters, "RegistrationUser");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    regId = Convert.ToInt32(row["REG_ID"].ToString());
                }

                return regId;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("UserId: " + userId +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void InsertProviderDisenrollment(int regId, string providerDisenrollmentId, DateTime disenrollDate, string modifiedBy, DateTime changedDate, bool IsInsert = true)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_DISENROLLMENT_XREF_ID", DbType.Int32, Convert.ToInt32(providerDisenrollmentId), true));
                parameters.Add(SqlParms.CreateParameter("DISENROLLMENT_DATE", DbType.DateTime, disenrollDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, modifiedBy, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("ISINSERT", DbType.Boolean, IsInsert, true));
                DataAccess.ExecuteStoredProcedure("insertREG_PROVIDER_DISENROLLMENTCustom", parameters, "Disenrollment");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() + ex.StackTrace));
            }
        }



        public static void InsertRegProviderNote(int regId, int regPageTypeId, int regSectionTypeId, int noteTypeId, string noteText, DateTime noteDate,
            int stepId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeId, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_NOTE_TYPE_ID", DbType.Int32, noteTypeId, true));
                parameters.Add(SqlParms.CreateParameter("NOTE_TEXT", DbType.String, noteText, true));
                parameters.Add(SqlParms.CreateParameter("NOTE_DATE_TIME", DbType.DateTime, noteDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                int providerNoteId = Convert.ToInt32(DataAccess.ExecuteScalar("insertPROVIDER_NOTECustom", parameters));

                //providerNoteId is 0 whenever there is an update. dont call SP insertREG_PROVIDER_NOTE_XREFCustom when its an update
                if (providerNoteId != 0)
                {
                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                    parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeId, true));
                    parameters.Add(SqlParms.CreateParameter("REG_SECTION_TYPE_ID", DbType.Int32, regSectionTypeId, true));
                    parameters.Add(SqlParms.CreateParameter("PROVIDER_NOTE_ID", DbType.Int32, providerNoteId, true));
                    parameters.Add(SqlParms.CreateParameter("STEP_ID", DbType.Int32, stepId, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                    DataAccess.ExecuteStoredProcedure("insertREG_PROVIDER_NOTE_XREFCustom", parameters);
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Reg Page Type Id: " + regPageTypeId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static string GetProviderTypeByRegId(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetProviderTypeByRegId", parameters, "ProviderType");
                ds.Tables[0].TableName = "ProviderType";
                if (ObjectControllerHelper.HasRows(ds))
                {
                    return ds.Tables["ProviderType"].Rows[0]["PROVIDER_TYPE_NAME"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static string GetProviderTypeIdByRegId(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetProviderTypeIdByRegId", parameters, "ProviderType");
                ds.Tables[0].TableName = "ProviderType";
                if (ObjectControllerHelper.HasRows(ds))
                {
                    return ds.Tables["ProviderType"].Rows[0]["MMIS_PROVIDER_TYPE_ID"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void SaveRegistrationSectionStatus(int regId, int regPageTypeId, int regSectionTypeId, int? regProviderStatusTypeId, int? regProviderServicesStatusTypeId,
            DateTime changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeId, false));
                parameters.Add(SqlParms.CreateParameter("REG_SECTION_TYPE_ID", DbType.Int32, regSectionTypeId, false));
                if (regProviderStatusTypeId != null)
                    parameters.Add(SqlParms.CreateParameter("REG_PROVIDER_STATUS_TYPE_ID", DbType.Int32, regProviderStatusTypeId, false));
                if (regProviderServicesStatusTypeId != null)
                    parameters.Add(SqlParms.CreateParameter("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", DbType.Int32, regProviderServicesStatusTypeId, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_SaveRegistrationSectionStatus", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Reg Page Type Id: " + regPageTypeId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void SaveRegistrationPageStatus(int regId, int regPageTypeId, int? regProviderStatusTypeId, int? regProviderServicesStatusTypeId,
            DateTime changedDate, string changedBy, int? FinancialReviewStatus, int? LTCReviewStatus, int? StateReviewStatus, int? DBHReviewStatus,
            int? DDSReviewStatus, int? DHCFReviewStatus, int? DHCFApplicationFeeReviewStatus, int? StateApplicationReviewStatus, int? DBHReviewerReviewStatus, int? DDSReviewerReviewStatus)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeId, false));
                if (regProviderStatusTypeId != null)
                    parameters.Add(SqlParms.CreateParameter("REG_PROVIDER_STATUS_TYPE_ID", DbType.Int32, regProviderStatusTypeId, false));
                if (regProviderServicesStatusTypeId != null)
                    parameters.Add(SqlParms.CreateParameter("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", DbType.Int32, regProviderServicesStatusTypeId, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                if (FinancialReviewStatus != null)
                    parameters.Add(SqlParms.CreateParameter("Financial_REVIEW_STATUS_ID", DbType.Int32, FinancialReviewStatus, false));
                if (LTCReviewStatus != null)
                    parameters.Add(SqlParms.CreateParameter("LTC_REVIEW_STATUS_ID", DbType.Int32, LTCReviewStatus, false));
                if (StateReviewStatus != null)
                    parameters.Add(SqlParms.CreateParameter("STATE_REVIEW_STATUS_ID", DbType.Int32, StateReviewStatus, false));
                if (DBHReviewStatus != null)
                    parameters.Add(SqlParms.CreateParameter("DBH_REVIEW_STATUS_ID", DbType.Int32, DBHReviewStatus, false));
                if (DDSReviewStatus != null)
                    parameters.Add(SqlParms.CreateParameter("DDS_REVIEW_STATUS_ID", DbType.Int32, DDSReviewStatus, false));
                if (DHCFReviewStatus != null)
                    parameters.Add(SqlParms.CreateParameter("DHCF_REVIEW_STATUS_ID", DbType.Int32, DHCFReviewStatus, false));
                if (DHCFApplicationFeeReviewStatus != null)
                    parameters.Add(SqlParms.CreateParameter("DHCF_APPLICATION_FEE_REVIEW_STATUS_ID", DbType.Int32, DHCFApplicationFeeReviewStatus, false));
                if (StateApplicationReviewStatus != null)
                    parameters.Add(SqlParms.CreateParameter("STATE_APPLICATION_FEE_REVIEW_STATUS_ID", DbType.Int32, StateApplicationReviewStatus, false));
                if (DBHReviewerReviewStatus != null)
                    parameters.Add(SqlParms.CreateParameter("DBH_REVIEWER_REVIEW_STATUS_ID", DbType.Int32, DBHReviewerReviewStatus, false));
                if (DDSReviewerReviewStatus != null)
                    parameters.Add(SqlParms.CreateParameter("DDS_REVIEWER_REVIEW_STATUS_ID", DbType.Int32, DDSReviewerReviewStatus, false));
                DataAccess.ExecuteStoredProcedure("usp_SaveRegistrationPageStatus", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Reg Page Type Id: " + regPageTypeId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void SyncRegistrationPageStatus(int regId, int regPageTypeId, DateTime changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeId, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_SyncRegistrationPageStatus", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Reg Page Type Id: " + regPageTypeId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void SaveRegistrationQuestion(int regId, string questionTypeId, int response, int modifiedStatusTypeId,
            DateTime changedDate, string changedBy, string responseComment)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("QUESTION_TYPE_ID", DbType.String, questionTypeId, false));
                parameters.Add(SqlParms.CreateParameter("RESPONSE", DbType.Int32, response, false));
                parameters.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, modifiedStatusTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                parameters.Add(SqlParms.CreateParameter("RESPONSE_COMMENT", DbType.String, responseComment, true));
                DataAccess.ExecuteStoredProcedure("usp_SaveRegistrationQuestion", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Question Type Id: " + questionTypeId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void SaveRegistrationAgreementInitials(int regId, string questionTypeId, int response, int modifiedStatusTypeId,
        DateTime changedDate, string changedBy, string responseComment, string Initials, string signature)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("QUESTION_TYPE_ID", DbType.String, questionTypeId, false));
                parameters.Add(SqlParms.CreateParameter("RESPONSE", DbType.Int32, response, false));
                parameters.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, modifiedStatusTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                parameters.Add(SqlParms.CreateParameter("RESPONSE_COMMENT", DbType.String, responseComment, true));
                parameters.Add(SqlParms.CreateParameter("Initials", DbType.String, Initials, true));
                parameters.Add(SqlParms.CreateParameter("REG_SIGNATURE", DbType.String, signature, true));

                DataAccess.ExecuteStoredProcedure("usp_SaveAgreementInitials", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Question Type Id: " + questionTypeId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void SaveRegistrationOwnerXref(int regId, int regOwner1Id, int regOwner2Id, int relationshipTypeId, int modifiedStatusTypeId,
            DateTime changedDate, string changedBy, int relationshipTypeId2)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("REG_OWNER1_ID", DbType.Int32, regOwner1Id, false));
                parameters.Add(SqlParms.CreateParameter("REG_OWNER2_ID", DbType.Int32, regOwner2Id, false));
                parameters.Add(SqlParms.CreateParameter("RELATIONSHIP_TYPE_ID", DbType.Int32, relationshipTypeId, false));
                if (relationshipTypeId2 > 0)
                    parameters.Add(SqlParms.CreateParameter("RELATIONSHIP_TYPE_ID2", DbType.Int32, relationshipTypeId2, false));
                parameters.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, modifiedStatusTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_SaveREG_OWNER_XREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Reg Owner1 Id: " + regOwner1Id.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectREG_ACH_REQUEST(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ACH_REQUEST", parameters, "ACH");
                ds.Tables[0].TableName = "ACH";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet CheckRegAccountingReview(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckRegAccountingReview", parameters, "AccountingReview_" + regId);
                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "AccountingReview";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectREG_ERRORcustom(int regId, int pageTypeId, int sectionTypeId, bool includeClosed, bool includePartyErrors)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, pageTypeId, true));
                parameters.Add(SqlParms.CreateParameter("REG_SECTION_TYPE_ID", DbType.Int32, sectionTypeId, true));
                parameters.Add(SqlParms.CreateParameter("IncludeClosed", DbType.Boolean, includeClosed, true));
                parameters.Add(SqlParms.CreateParameter("IncludePartyErrors", DbType.Boolean, includePartyErrors, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ERRORcustom", parameters, "ReturnReasons_" + regId);
                ds.Tables[0].TableName = "ReturnReasons";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Select the Registration emails given the REG_ID and Subject (optional)
        public static DataSet SelectRegistrationEmails(int regId, string subject)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                if (!string.IsNullOrEmpty(subject))
                    parameters.Add(SqlParms.CreateParameter("Subject", DbType.String, subject, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationEmails", parameters, "RegistrationEmails_" + regId);
                ds.Tables[0].TableName = "RegistrationEmails";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Permanently delete the Registration Data
        public static void DeleteRegistrationData(string tableName, string idColumnName, int id)
        {
            try
            {
                string storedproc = "";
                switch (tableName.ToLower())
                {

                    case "clia": storedproc = "usp_deletereg_clia"; break;
                    case "dea": storedproc = "usp_deletereg_dea"; break;
                    case "medicare": storedproc = "usp_deletereg_medicare"; break;
                    case "specialty": storedproc = "usp_deletereg_specialty"; break;
                    case "cpr_certification": storedproc = "usp_deletereg_cpr_certification"; break;
                    case "credentialing_contact": storedproc = "usp_deletereg_credentialing_contact"; break;
                    case "firstaid_certification": storedproc = "usp_deletereg_firstaid_certification"; break;
                    case "dental_licensetype_licenseid": storedproc = "usp_deletereg_dental_licensetype_licenseid"; break;
                    case "dental_license": storedproc = "usp_deletereg_dental_license"; break;
                    case "dme_accreditation_agency": storedproc = "usp_deletereg_dme_accreditation_agency"; break;
                    case "household_member_address_history": storedproc = "usp_deletereg_household_member_address_history"; break;
                    case "household_member_criminal_history": storedproc = "usp_deletereg_household_member_criminal_history"; break;
                    case "household_membercustom": storedproc = "usp_deletereg_household_membercustom"; break;
                    case "license": storedproc = "usp_deletereg_license"; break;
                    case "address": storedproc = "usp_deletereg_address"; break;
                    case "owner": storedproc = "usp_deletereg_owner"; break;
                    case "pharmacist_info": storedproc = "usp_deletereg_pharmacist_info"; break;
                    case "taxonomy": storedproc = "usp_deletereg_taxonomy"; break;
                    case "specialty_category": storedproc = "usp_deletereg_specialty_category"; break;
                    case "specialty_employee": storedproc = "usp_deletereg_specialty_employee"; break;
                    case "specialty_approval": storedproc = "usp_deletereg_specialty_approval"; break;
                    case "endorsement_specialty_focus": storedproc = "usp_deletereg_endorsement_specialty_focus"; break;
                    case "boardcertification": storedproc = "USP_DELETEREG_BOARD_CERTIFICATION"; break;

                    default:
                        throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter(idColumnName, DbType.Int32, id, true));
                DataAccess.ExecuteStoredProcedure(storedproc, parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName +
                    ", Id Column Name: " + idColumnName + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void DeleteRegistrationDataWithParams(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value)) parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    else parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }
                string storedproc = string.Empty;
                switch (tableName.ToLower())
                {
                    case "specialtycustom2": storedproc = "usp_DeleteREG_specialtycustom2"; break;
                    case "specialtycustom": storedproc = "usp_DeleteREG_specialtycustom"; break;
                    case "specialtycustom_cpc": storedproc = "usp_DeleteREG_SPECIALTYCustom_CPC"; break;
                    case "usp_deletereg_ltc_risk_alert": storedproc = "usp_DeleteREG_LTC_RISK_ALERT"; break;

                    default: throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                DataAccess.ExecuteStoredProcedure(storedproc, parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteRegNPIEnrollment(int id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_NPI_MEDID_ENROLLMENT_SPAN_ID", DbType.Int32, id, true));
                DataAccess.ExecuteStoredProcedure("usp_DeleteREG_NPI_MEDID_ENROLLMENT_SPAN", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("usp_DeleteREG_NPI_MEDID_ENROLLMENT_SPAN: " + id.ToString() +
                    ", " + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void UpdateErrorHistory(int regErrorId, int errorStatusTypeId, int? communicationEventId,
            int? resolvingActionTypeId, int? resolvingReasonTypeId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ERROR_ID", DbType.Int32, regErrorId, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_STATUS_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_STATUS_TYPE_ID", DbType.Int32, errorStatusTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_ID", DbType.Int32, communicationEventId, true));
                parameters.Add(SqlParms.CreateParameter("RESOLVING_ACTION_TYPE_ID", DbType.Int32, resolvingActionTypeId, true));
                parameters.Add(SqlParms.CreateParameter("RESOLVING_REASON_TYPE_ID", DbType.Int32, resolvingReasonTypeId, true));
                DataAccess.ExecuteStoredProcedure("insertREG_ERROR_HISTORYCustom", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Reg Error Id: " + regErrorId.ToString() +
                    ", Error Status Type Id: " + errorStatusTypeId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void InsertUserWorkflowPermission(string USERID, int WORKFLOW_ID, string LAST_MODIFIED_USER)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("WORKFLOW_ID", DbType.Int32, WORKFLOW_ID, true));
                parameters.Add(SqlParms.CreateParameter("USERID", DbType.Guid, USERID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, LAST_MODIFIED_USER, true));
                DataAccess.ExecuteStoredProcedure("insertWF_WORKFLOW_PERMISSIONS", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("User Id: " + USERID +
                    ", Workflow Id: " + WORKFLOW_ID.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void DeleteUserWorkflowPermission(string USERID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("USERID", DbType.Guid, USERID, true));
                DataAccess.ExecuteStoredProcedure("deleteWF_WORKFLOW_PERMISSIONS", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("User Id: " + USERID +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectUserWorkflowPermission(string USERID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("USERID", DbType.Guid, USERID, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_selectWF_WORKFLOW_PERMISSIONS", parameters, "wp");
                ds.Tables[0].TableName = "ReturnReasons";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("User Id: " + USERID +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet TransferPDMStoRegistration(bool ownershipChanged, int partyId, string userId,
            DateTime requestedEffectiveDate, DateTime changeEffectiveDate, string formCompletionName,
            string formCompletionPhone)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, true));

                string storedProc = "usp_TransferLiveToReg_OwnershipChanged";
                if (!ownershipChanged) storedProc = "usp_TransferLiveToReg";
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("RequestedEffectiveDate", DbType.DateTime, requestedEffectiveDate, true));
                parameters.Add(SqlParms.CreateParameter("ChangeEffectiveDate", DbType.DateTime, changeEffectiveDate, true));
                parameters.Add(SqlParms.CreateParameter("FormCompletionName", DbType.String, formCompletionName, true));
                parameters.Add(SqlParms.CreateParameter("FormCompletionPhone", DbType.String, formCompletionPhone, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "TransferPDMStoReg");
                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "TransferPDMStoReg";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Party Id: " + partyId.ToString() +
                    ", User Id: " + userId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet TransferPDMStoRegistrationIndividual(int partyId, string userId, string firstLastName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("FirstLastName", DbType.String, firstLastName, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_TransferLiveToRegIndividual", parameters, "TransferPDMStoReg");
                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "TransferPDMStoReg";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Party Id: " + partyId.ToString() +
                    ", User Id: " + userId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet GetGroupNames(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetGroupNames", parameters, "GroupNames_" + regId);
                ds.Tables[0].TableName = "GroupNames";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetWFProcessByRegId(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetWFProcessByRegId", parameters, "Process_" + regId);
                ds.Tables[0].TableName = "Process";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetGroupAffiliationHistory(int regId, int pageSize, int pageNumber)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("pageSize", DbType.Int32, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("pageNumber", DbType.Int32, pageNumber, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetGroupAffiliationHistory", parameters, "GAPage_" + regId);
                ds.Tables[0].TableName = "GAPage";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool RegistrationNeverSubmitted(int regId)
        {
            try
            {
                bool toReturn = true;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationSubmissionHistory", parameters, "Process_" + regId);
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                        toReturn = false;

                return toReturn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckAttestToFeePaymentRequired(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regId, true));
                return Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIsAttestToFeePaymentRequired", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckCDSNumberSectionRequired(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regId, true));
                return Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckCDSRequired", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRevalidationProviders(int regId = 0)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (regId > 0) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectRevalidationProviders", parameters, "Revalidations_" + regId);
                ds.Tables[0].TableName = "Revalidations";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectICF_IIDContractDetails(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectICF_IIDContract_Details", parameters, "ContractDetails_" + regID);

                lookup.Tables[0].TableName = "ContractDetails";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void ResetApplicationFee(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));

                DataAccess.ExecuteStoredProcedure("usp_ResetREG_APPLICATION_FEE", parameters, "Reg_Application_Fee");

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegistrationsByAgentUserIDAndSubRole(Guid userID, string subroleName)
        {
            try
            {
                DataSet lookup = new DataSet();
                //string outValue;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("ROLE_NAME", DbType.String, subroleName, true));

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllREGISTRATIONsByAgentUserIDAndSubRole", parameters, "ds");
                lookup.Tables[0].TableName = "MyAgentProviders";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegistrationsByUserID(Guid userID, int onlyProviderCategoryTypeID, int excludeProviderCategoryTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet lookup = new DataSet();
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("OnlyProviderCategoryTypeID", DbType.Int32, onlyProviderCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("ExcludeProviderCategoryTypeID", DbType.Int32, excludeProviderCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationsByUserID", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                lookup.Tables[0].TableName = "MyProviders";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectAllRegistrationsByUserIDAndTaxID(Guid userID, string taxID, int onlyProviderCategoryTypeID, int excludeProviderCategoryTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet lookup = new DataSet();
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("OnlyProviderCategoryTypeID", DbType.Int32, onlyProviderCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("ExcludeProviderCategoryTypeID", DbType.Int32, excludeProviderCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllREGISTRATIONsByUserIDAndTaxID", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                lookup.Tables[0].TableName = "MyProviders";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAllRegistrationsByUserID(Guid userID, int onlyProviderCategoryTypeID, int excludeProviderCategoryTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, string loggedinUserID, out int totalResultCount)
        {
            try
            {
                DataSet lookup = new DataSet();
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("OnlyProviderCategoryTypeID", DbType.Int32, onlyProviderCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("ExcludeProviderCategoryTypeID", DbType.Int32, excludeProviderCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("LoggedinUserID", DbType.String, loggedinUserID, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllREGISTRATIONsByUserID", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                lookup.Tables[0].TableName = "MyProviders";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }



        public static bool IsDeemedEligble(string userName)
        {
            try
            {
                bool IsDeemedEligibleAccount = false;

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, !string.IsNullOrEmpty(userName) ? userName.ToLower() : "", false));

                IsDeemedEligibleAccount = Convert.ToBoolean(DataAccess.ExecuteStoredProcedure("usp_CheckIfDeemedUser", parameters, "IsDeemedUser", SqlDbType.Bit, 100));

                return IsDeemedEligibleAccount;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegistrationsPendingConvertedByTaxID(string taxID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet lookup = new DataSet();
                string outValue;
                totalResultCount = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationsPendingConvertedByTaxID", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                lookup.Tables[0].TableName = "RegData";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectRegistrationsWithSameNPIOverlapEffectiveDate(int reg_id, DateTime effective_date, string npi)
        {
            try
            {
                DataSet lookup = new DataSet();


                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_id, false));
                parameters.Add(SqlParms.CreateParameter("effective_date", DbType.DateTime, effective_date, false));
                parameters.Add(SqlParms.CreateParameter("npi", DbType.String, npi, true));

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationsWithSameNPIOverlapEffectiveDate", parameters, "ds");
                lookup.Tables[0].TableName = "RegData";


                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectAllRegistrationsByTaxID(string taxID, Guid userID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet lookup = new DataSet();
                string outValue;
                totalResultCount = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllRegistrationsByTaxID", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                lookup.Tables[0].TableName = "RegData";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectRegistrationByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectUserNameByUserID(string userID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("userId", DbType.String, userID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetLoggedInUserNameByUserId", parameters, "UserName_" + userID);

                lookup.Tables[0].TableName = "UserName";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectConvertedRegistrationByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectConvertedRegistrationByRegID", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectConfirmedAffiliationByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_CONFIRMED_AFFILIATION", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPracticePartnership(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetPracticePartnership", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetProviderNameByMedicaidID(string medicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Medicaid_ID", DbType.String, medicaidId, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderNameByMedicaidID", parameters, "RegData_" + medicaidId);
                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPendingAffiliationByID(int pendingAffiliationID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("pendingAffiliationID", DbType.Int32, pendingAffiliationID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PENDING_AFFILIATION_ByID", parameters, "RegData_" + pendingAffiliationID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMalpracticeClaimByID(int MalpracticeClaimID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MalpracticeClaimID", DbType.Int32, MalpracticeClaimID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_MALPRACTICE_CLAIM_ByID", parameters, "RegData_" + MalpracticeClaimID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet DeleteMalpracticeClaimByID(int MalpracticeClaimID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MalpracticeClaimID", DbType.Int32, MalpracticeClaimID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_DeleteREG_MALPRACTICE_CLAIM_ByID", parameters, "RegData_" + MalpracticeClaimID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderPaymentInfoByRegID(int RegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER_PAYMENT_INFO_ByRegID", parameters, "RegData_" + RegID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAffiliationByMedicaidID(string medicaid_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("medicaidID", DbType.String, medicaid_ID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_AFFILIATION_ByMedicaidID", parameters, "RegData_" + medicaid_ID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectPendingAffiliationByMedicaidID(string medicaid_ID, int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("medicaidID", DbType.String, medicaid_ID, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PENDING_AFFILIATION_ByMedicaidID", parameters, "RegData_" + medicaid_ID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAffiliationByStatusMedicaidID(string medicaid_ID, int regID, int grpAffiliationStatusId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Medicaid_ID", DbType.String, medicaid_ID, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("Affilation_Status_ID", DbType.Int32, grpAffiliationStatusId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_AFFILIATION_ByStatusMedicaidID", parameters, "RegData_" + medicaid_ID);

                lookup.Tables[0].TableName = "RegAffiliation";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectHealthCareAffiliationByID(int healthCareAffiliationID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("healthCareAffiliationID", DbType.Int32, healthCareAffiliationID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_HEALTH_CARE_AFFILIATION_ByID", parameters, "RegData_" + healthCareAffiliationID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteRegDMEProductServiceCategoryByRegID(int regID, int DME_PRODUCT_SERVICE_CATEGORY_Type_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("DME_PRODUCT_SERVICE_CATEGORY_Type_ID", DbType.Int32, DME_PRODUCT_SERVICE_CATEGORY_Type_ID, false));

                DataAccess.ExecuteStoredProcedure("usp_DeleteREG_DME_PRODUCT_SERVICE_CATEGORY_By_RegID", parameters, "RegData_" + regID);




            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteRegDMEProductServiceSubCategoryByRegID(int regID, int DME_PRODUCT_SERVICE_CATEGORY_Type_ID, int DME_PRODUCT_SERVICE_SUB_CATEGORY_Type_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("DME_PRODUCT_SERVICE_CATEGORY_Type_ID", DbType.Int32, DME_PRODUCT_SERVICE_CATEGORY_Type_ID, false));
                parameters.Add(SqlParms.CreateParameter("DME_PRODUCT_SERVICE_SUB_CATEGORY_Type_ID", DbType.Int32, DME_PRODUCT_SERVICE_SUB_CATEGORY_Type_ID, false));

                DataAccess.ExecuteStoredProcedure("usp_DeleteREG_DME_PRODUCT_SERVICE_SUB_CATEGORY_By_RegID", parameters, "RegData_" + regID);




            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteRegDMEProductServiceQualificationByRegID(int regID, int DME_PRODUCT_SERVICE_CATEGORY_Type_ID, int DME_QUALIFICATION_Type_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("DME_PRODUCT_SERVICE_CATEGORY_Type_ID", DbType.Int32, DME_PRODUCT_SERVICE_CATEGORY_Type_ID, false));
                parameters.Add(SqlParms.CreateParameter("DME_QUALIFICATION_Type_ID", DbType.Int32, DME_QUALIFICATION_Type_ID, false));

                DataAccess.ExecuteStoredProcedure("usp_DeleteREG_DME_PRODUCT_SERVICE_QUALIFICATION_By_RegID", parameters, "RegData_" + regID);




            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeletePendingAffiliationByID(int pendingAffiliationID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("pendingAffiliationID", DbType.Int32, pendingAffiliationID, false));

                DataAccess.ExecuteStoredProcedure("usp_DeleteREG_PENDING_AFFILIATION_ByID", parameters, "RegData");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteHealthCareFacilityAffiliationByID(int healthCareAffiliationID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("healthCareAffiliationID", DbType.Int32, healthCareAffiliationID, false));

                DataAccess.ExecuteStoredProcedure("usp_DeleteREG_HEALTH_CARE_FACILITY_AFFILIATION_ByID", parameters, "RegData");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteAssignedDelegateByID(int regAssignedDelegateID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_DELEGATE_CREDENTIALING_ID", DbType.Int32, regAssignedDelegateID, false));

                DataAccess.ExecuteStoredProcedure("usp_DeleteREG_DELEGATE_CREDENTIALING_ByID", parameters, "RegData");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectNursingProfessionalCertificationByID(int nursingProfessionalID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("nursingProfessionalID", DbType.Int32, nursingProfessionalID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_NURSING_PROFESSIONAL_CERTIFICATION_ByID", parameters, "RegData_" + nursingProfessionalID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectBoardCertificationByID(int CPRID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("BoardCertificationID", DbType.Int32, CPRID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_Board_CERTIFICATION_ByID", parameters, "RegData_" + CPRID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectCPRCertificationByID(int CPRID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CPRCertificationID", DbType.Int32, CPRID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_CPR_CERTIFICATION_ByID", parameters, "RegData_" + CPRID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int IsUserInSubRole(int regId, string userId, string roleName)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.String, userId, false));
                parameters.Add(SqlParms.CreateParameter("RoleName", DbType.String, roleName, false));
                int result = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectREG_UserSubRoles", parameters));
                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderByGRPMedicaidID(string GRPMedicaid_ID)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GRPMedicaid_ID", DbType.String, GRPMedicaid_ID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderByGRPMedicaidID", parameters, "RegData_" + GRPMedicaid_ID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }



        public static DataSet SelectProviderByRegID(string Reg_ID)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_ID", DbType.String, Reg_ID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderByRegId", parameters, "RegData_" + Reg_ID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectHospitalFacilityByMedicaidID(string GRPMedicaid_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GRPMedicaid_ID", DbType.String, GRPMedicaid_ID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectHospitalFacilityByMedicaidID", parameters, "RegData_" + GRPMedicaid_ID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAffiliationByGRPMedicaidID(int reg_ID, string GRPMedicaid_ID, string GRPTaxID, string GRPNPI)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("reg_ID", DbType.Int32, reg_ID, false));
                parameters.Add(SqlParms.CreateParameter("GRPMedicaid_ID", DbType.String, GRPMedicaid_ID, false));
                parameters.Add(SqlParms.CreateParameter("GRPTaxID", DbType.String, GRPTaxID, false));
                parameters.Add(SqlParms.CreateParameter("GRPNPI", DbType.String, GRPNPI, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAffiliationByGRPMedicaidID", parameters, "RegData_" + reg_ID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPendingAffiliationByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PENDING_AFFILIATION", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCredentialingDelegatesByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_DELEGATE_CREDENTIALING", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMalpracticeClaimByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_MALPRACTICE_CLAIM", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectHealthCareFacilityAffiliationByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_HEALTH_CARE_FACILITY_AFFILIATION", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectRegistrationsByTaxID(string taxID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONsByTaxID", parameters, "RegData_" + taxID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegistrationsByNPI(string NPI)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONsByNPI", parameters, "RegData_" + NPI);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetIndividualStandardSpanStartDate(string medicaidID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetIndividualStandardSpanStartDate", parameters, "RegData_" + medicaidID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectRegistrationsByAdminUpdateKeyFields(string NPI, string TaxonomyCode, int ProviderTypeID,
           string MMISProviderTypeID, string ZipCode, string ZipExt, bool IsFilterOtherWaivers, string ServicesProviderTypeName, int RegID, string taxid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Boolean, ProviderTypeID, true));
                parameters.Add(SqlParms.CreateParameter("TaxonomyCode", DbType.String, TaxonomyCode, true));
                parameters.Add(SqlParms.CreateParameter("MMISProviderTypeID", DbType.String, MMISProviderTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ZipCode", DbType.String, ZipCode, true));
                parameters.Add(SqlParms.CreateParameter("ZipExt", DbType.String, ZipExt, true));
                parameters.Add(SqlParms.CreateParameter("IsFilterOtherWaivers", DbType.Boolean, IsFilterOtherWaivers, true));
                parameters.Add(SqlParms.CreateParameter("ServicesProviderTypeName", DbType.String, ServicesProviderTypeName, true));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegID, true));
                parameters.Add(SqlParms.CreateParameter("taxid", DbType.String, taxid, true));


                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONsByAdminUpdateKeyFields", parameters, "RegData");

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void DeleteDocumentMailById(int priorAuthDocumentMailId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DOCUMENT_MAIL_ID", DbType.Int32, priorAuthDocumentMailId, true));
                DataAccess.ExecuteStoredProcedure("usp_DeleteDocumentByMailId", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegistrationsByKeyFields(string NPI, string TaxonomyCode, int ProviderTypeID,
           string MMISProviderTypeID, string ZipCode, string ZipExt, bool IsFilterOtherWaivers, string ServicesProviderTypeName, int RegID, string taxId, int applicationType)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Boolean, ProviderTypeID, true));
                parameters.Add(SqlParms.CreateParameter("TaxonomyCode", DbType.String, TaxonomyCode, true));
                parameters.Add(SqlParms.CreateParameter("MMISProviderTypeID", DbType.String, MMISProviderTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ZipCode", DbType.String, ZipCode, true));
                parameters.Add(SqlParms.CreateParameter("ZipExt", DbType.String, ZipExt, true));
                parameters.Add(SqlParms.CreateParameter("IsFilterOtherWaivers", DbType.Boolean, IsFilterOtherWaivers, true));
                parameters.Add(SqlParms.CreateParameter("ServicesProviderTypeName", DbType.String, ServicesProviderTypeName, true));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegID, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeId", DbType.Int32, applicationType, true));
                parameters.Add(SqlParms.CreateParameter("TaxId", DbType.String, taxId, true));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONsByKeyFields", parameters, "RegData_" + NPI);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetCPCLinkVisibility(int RegID, int CurrentStepID, string MMISProviderTypeID, int EntityTypeID, string CPC_Program_Year, string CPC_Practice_Type)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegID, true));
                parameters.Add(SqlParms.CreateParameter("CURRENT_STEP_ID", DbType.Int32, CurrentStepID, true));
                parameters.Add(SqlParms.CreateParameter("MMIS_PROVIDER_TYPE_ID", DbType.String, MMISProviderTypeID, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, EntityTypeID, true));
                parameters.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, CPC_Program_Year, true));
                parameters.Add(SqlParms.CreateParameter("CPC_PRACTICE_TYPE", DbType.String, CPC_Practice_Type, true));
                DataSet result = new DataSet();
                result = DataAccess.ExecuteStoredProcedure("usp_GetCPCLinks_Visibility", parameters, "CPCData_" + RegID);

                result.Tables[0].TableName = "CPCData";

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CanReEnableCPCLinks(int RegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegID, true));

                string CanReenable = Convert.ToString(DataAccess.ExecuteStoredProcedure("usp_CanReEnableCPCLinks", parameters, "CanReenable", SqlDbType.VarChar, 100));

                bool result = CanReenable == "True" ? true : false;

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool CanReEnableCMCLinks(int RegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegID, true));

                string CanReenable = Convert.ToString(DataAccess.ExecuteStoredProcedure("usp_CanReEnableCMCLinks", parameters, "CanReenable", SqlDbType.VarChar, 100));

                bool result = CanReenable == "True" ? true : false;

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool CanStartCredentialReconsideration(int RegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegID, true));

                string CanShow = Convert.ToString(DataAccess.ExecuteStoredProcedure("usp_CanStartCredentialReconsideration", parameters, "CanShow", SqlDbType.VarChar, 100));

                bool result = CanShow == "True" ? true : false;

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet CheckCMCLinkEnabledByRegID(int RegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegID, true));
                DataSet result = new DataSet();
                result = DataAccess.ExecuteStoredProcedure("usp_CheckCMCLinkEnabledByRegID", parameters, "CMCDataLink_" + RegID);

                result.Tables[0].TableName = "CMCDataLink";

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet CheckCMCEnrollmentPeriod()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_CheckCMCEnrollmentPeriod", "CheckCMCEnrollmentPeriod");
                lookup.Tables[0].TableName = "CheckCMCEnrollmentPeriod";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet CheckCMCInvited(int RegId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, true));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetCMCInvite", parameters, "CheckCMCInvite" + RegId);
                lookup.Tables[0].TableName = "CheckCMCInvited";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet IsCMCEnrolled(int RegId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, true));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetCMCEnrollmentDetails", parameters, "EnrolledCMC" + RegId);
                lookup.Tables[0].TableName = "EnrolledCMC";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetCMCLinkVisibility(int RegID, int CurrentStepID, string MMISProviderTypeID, int EntityTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegID, true));
                parameters.Add(SqlParms.CreateParameter("CURRENT_STEP_ID", DbType.Int32, CurrentStepID, true));
                parameters.Add(SqlParms.CreateParameter("MMIS_PROVIDER_TYPE_ID", DbType.String, MMISProviderTypeID, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, EntityTypeID, true));
                DataSet result = new DataSet();
                result = DataAccess.ExecuteStoredProcedure("usp_GetCMCLinks_Visibility", parameters, "CMCData_" + RegID);

                result.Tables[0].TableName = "CMCData";

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertNewCPCProviderRegistration(int regID, string cpcType, string CPC_Program_Year, Guid userID, string CPC_Practice_Type)
        {
            int cpcRegID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("CPC_LINK_TYPE", DbType.String, cpcType, false));
                parameters.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, CPC_Program_Year, true));
                parameters.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("User", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("CPC_PRACTICE_TYPE", DbType.String, CPC_Practice_Type, true));
                cpcRegID = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertNew_CPCProviderRegistration", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return cpcRegID;
        }
        //public static void InsertCPCContract(int regID, string cpc_program_year, string userID)
        //{
        //    try
        //    {
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
        //        parameters.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, cpc_program_year, false));
        //        parameters.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, true));
        //        parameters.Add(SqlParms.CreateParameter("User", DbType.Guid, userID, false));
        //        DataAccess.ExecuteStoredProcedure("usp_InsertREG_CONTRACT_CPC", parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw CoreException.ThrowException(ex);
        //    }
        //}

        // JIRA 3041 - Include DDContractNumber
        public static int InsertNewProviderRegistration(Guid userID, int applicationTypeID, string providerName, string dba, string firstName, string middleName, string lastName, DateTime? birthDate,
            string gender, int taxIDTypeID, string taxID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int registrationStatusTypeId, int referralID, int specialtyTypeID, int taxonomyTypeID, string practiceName, string zipCode, string zipExt, bool isPaperApplication,
             int paperRequestQueueID, DateTime createdDate, Guid createdBy, int workflowID, DateTime? RegCreateDateTime, int workflowEventTypeID, int waiverTypeID, bool retroEffectiveDate, string ddFacilityNumber, string DDContractNumber)
        {
            int regId = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderName", DbType.String, providerName, false));
                parameters.Add(SqlParms.CreateParameter("DBA", DbType.String, dba, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("MiddleInitial", DbType.String, middleName, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("Gender", DbType.String, gender, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("TaxIDTypeID", DbType.Int32, taxIDTypeID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderCategoryTypeID", DbType.Int32, providerCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("TypeofPracticeID", DbType.Int32, practiceTypeID, true));
                parameters.Add(SqlParms.CreateParameter("RegistrationStatusTypeID", DbType.Int32, registrationStatusTypeId, true));
                if (birthDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("BirthDate", DbType.Date, birthDate, true));
                if (requestedEffectiveDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("RequestedEffectiveDate", DbType.Date, requestedEffectiveDate, true));

                /***************************************************************************************************
                 * SpecialtyID is not required, so use 0 for null specialty 
                 ***************************************************************************************************/
                //if (specialtyTypeID > 0)
                //    parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, specialtyTypeID, true));
                parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, specialtyTypeID, true));

                if (taxonomyTypeID > 0)
                    parameters.Add(SqlParms.CreateParameter("TaxonomyTypeID", DbType.Int32, taxonomyTypeID, true));

                if (referralID > 0)
                    parameters.Add(SqlParms.CreateParameter("ReferralID", DbType.Int32, referralID, true));

                if (paperRequestQueueID > 0)
                    parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, paperRequestQueueID, true));

                parameters.Add(SqlParms.CreateParameter("PracticeName", DbType.String, practiceName, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipCode", DbType.String, zipCode, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipExt", DbType.String, zipExt, true));
                parameters.Add(SqlParms.CreateParameter("Is_Paper_Application", DbType.Boolean, isPaperApplication == true ? 1 : 0, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, createdDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, createdBy, true));
                parameters.Add(SqlParms.CreateParameter("WorkflowID", DbType.Int32, workflowID, true));
                parameters.Add(SqlParms.CreateParameter("Reg_Create_Date_Time", DbType.DateTime, RegCreateDateTime, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("WorkflowEventTypeID", DbType.Int32, workflowEventTypeID, true));
                parameters.Add(SqlParms.CreateParameter("WaiverTypeID", DbType.Int32, waiverTypeID, true));
                parameters.Add(SqlParms.CreateParameter("RetroEffectiveDate", DbType.Boolean, retroEffectiveDate, true));
                parameters.Add(SqlParms.CreateParameter("DDFacilityNumber", DbType.String, ddFacilityNumber, true));

                regId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertNewProviderRegistration", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return regId;
        }

        public static int InsertNewLinkedProviderRegistration(Guid userID, int applicationTypeID, string providerName, string dba, string firstName, string middleName, string lastName, DateTime? birthDate,
           string gender, int taxIDTypeID, string taxID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
           int registrationStatusTypeId, int referralID, int specialtyTypeID, int taxonomyTypeID, string practiceName, string zipCode, string zipExt, bool isPaperApplication,
            int paperRequestQueueID, DateTime createdDate, Guid createdBy, int workflowID, DateTime? RegCreateDateTime, int workflowEventTypeID, int FromRegID)
        {
            int regId = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, FromRegID, false));
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderName", DbType.String, providerName, false));
                parameters.Add(SqlParms.CreateParameter("DBA", DbType.String, dba, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("MiddleInitial", DbType.String, middleName, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("Gender", DbType.String, gender, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("TaxIDTypeID", DbType.Int32, taxIDTypeID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderCategoryTypeID", DbType.Int32, providerCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("TypeofPracticeID", DbType.Int32, practiceTypeID, true));
                parameters.Add(SqlParms.CreateParameter("RegistrationStatusTypeID", DbType.Int32, registrationStatusTypeId, true));
                if (birthDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("BirthDate", DbType.Date, birthDate, true));
                if (requestedEffectiveDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("RequestedEffectiveDate", DbType.Date, requestedEffectiveDate, true));

                /***************************************************************************************************
                 * SpecialtyID is not required, so use 0 for null specialty 
                 ***************************************************************************************************/
                //if (specialtyTypeID > 0)
                //    parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, specialtyTypeID, true));
                parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, specialtyTypeID, true));

                if (taxonomyTypeID > 0)
                    parameters.Add(SqlParms.CreateParameter("TaxonomyTypeID", DbType.Int32, taxonomyTypeID, true));

                if (referralID > 0)
                    parameters.Add(SqlParms.CreateParameter("ReferralID", DbType.Int32, referralID, true));

                if (paperRequestQueueID > 0)
                    parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, paperRequestQueueID, true));

                parameters.Add(SqlParms.CreateParameter("PracticeName", DbType.String, practiceName, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipCode", DbType.String, zipCode, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipExt", DbType.String, zipExt, true));
                parameters.Add(SqlParms.CreateParameter("Is_Paper_Application", DbType.Boolean, isPaperApplication == true ? 1 : 0, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, createdDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, createdBy, true));
                parameters.Add(SqlParms.CreateParameter("WorkflowID", DbType.Int32, workflowID, true));
                parameters.Add(SqlParms.CreateParameter("Reg_Create_Date_Time", DbType.DateTime, RegCreateDateTime, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("WorkflowEventTypeID", DbType.Int32, workflowEventTypeID, true));

                regId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertNewLinkedProviderRegistration", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return regId;
        }

        public static void UpdateConvertedProviderRegistration(int regID, Guid userID, string providerName, string dba, string firstName, string middleName, string lastName, DateTime? birthDate,
            string gender, int taxIDTypeID, string taxID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int referralID, int specialtyTypeID, int taxonomyTypeID, string practiceName, string zipCode, string zipExt, bool isPaperApplication,
            int paperRequestQueueID, DateTime createdDate, Guid createdBy, int workflowID, DateTime npiStartDate, DateTime? npiEndDate, DateTime? endDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderName", DbType.String, providerName, false));
                parameters.Add(SqlParms.CreateParameter("DBA", DbType.String, dba, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("MiddleInitial", DbType.String, middleName, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("Gender", DbType.String, gender, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("TaxIDTypeID", DbType.Int32, taxIDTypeID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderCategoryTypeID", DbType.Int32, providerCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("TypeofPracticeID", DbType.Int32, practiceTypeID, true));

                if (birthDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("BirthDate", DbType.Date, birthDate, true));
                if (requestedEffectiveDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("RequestedEffectiveDate", DbType.Date, requestedEffectiveDate, true));

                if (specialtyTypeID > 0)
                    parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, specialtyTypeID, true));
                if (taxonomyTypeID > 0)
                    parameters.Add(SqlParms.CreateParameter("TaxonomyTypeID", DbType.Int32, taxonomyTypeID, true));

                if (referralID > 0)
                    parameters.Add(SqlParms.CreateParameter("ReferralID", DbType.Int32, referralID, true));
                parameters.Add(SqlParms.CreateParameter("PracticeName", DbType.String, practiceName, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipCode", DbType.String, zipCode, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipExt", DbType.String, zipExt, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, createdDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, createdBy, true));
                if (paperRequestQueueID > 0)
                    parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, paperRequestQueueID, true));
                parameters.Add(SqlParms.CreateParameter("Is_Paper_Application", DbType.Boolean, isPaperApplication == true ? 1 : 0, true));
                parameters.Add(SqlParms.CreateParameter("WorkflowID", DbType.Int32, workflowID, true));
                parameters.Add(SqlParms.CreateParameter("NPIStartDate", DbType.DateTime, npiStartDate, true));
                parameters.Add(SqlParms.CreateParameter("NPIEndDate", DbType.DateTime, npiEndDate, true));
                parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, endDate, true));

                DataAccess.ExecuteStoredProcedure("usp_UpdateConvertedProviderRegistration", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectRegAffiliationData(int regAffiliationID, string tableName)
        {
            if (regAffiliationID <= 0)
                return null;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_AFFILIATION_ID", DbType.Int32, regAffiliationID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectREG_AFFILIATION_" + tableName;
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "RegAffiliationData_" + regAffiliationID);
                ds.Tables[0].TableName = "RegAffiliationData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetSpecialtiesByMMISId(string mmisSpecialtyID)
        {


            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("mmis_specialty_Id", DbType.String, mmisSpecialtyID, false));
                DataSet ds = new DataSet();

                ds = DataAccess.ExecuteStoredProcedure("usp_SelectCPCSpecialties", parameters, "CPCSpecialties");
                ds.Tables[0].TableName = "CPCSpecialties";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static void InsertUpdateRegAffiliationQuestions(int regAffiliationID, Dictionary<string, string> parms)
        {
            if (regAffiliationID <= 0)
                return;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (pair.Value == null)
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                DataAccess.ExecuteStoredProcedure("insertUpdateREG_AFFILIATION_QUESTIONS", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteRegAffiliation(int regAffiliationID)
        {
            if (regAffiliationID <= 0)
                return;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_AFFILIATION_ID", DbType.Int32, regAffiliationID, true));

                DataAccess.ExecuteStoredProcedure("usp_DeleteREG_AFFILIATION", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static void UpdateGroupMemberProfileAffiliationStatus(int regID, DateTime changedOn, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedOn, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateGROUP_MEMBER_PROFILE_AFFILIATION_STATUS", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet DisenrollProvider(int regID, DateTime termDate, string comments, DateTime modifiedOn, Guid modifiedBy, bool insertTransaction, string enrollmentStatusCode = "", bool insertPreviousEnrollmentSpan = false, bool isFromJob = true)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, termDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, insertTransaction, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, enrollmentStatusCode, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASON_CODE", DbType.String, CON.EnrollmentStatusCode.VOLUNTARYWITHDRAWAL, false));

                parameters.Add(SqlParms.CreateParameter("IS_FRM_JOB", DbType.Boolean, isFromJob, false));
                parameters.Add(SqlParms.CreateParameter("PROCESS_TYPE", DbType.String, "DISENROLL", false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_TerminateProvider", parameters, "TransactionIds");
                return ds;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SuspendProvider(int regID, DateTime termDate, string comments, DateTime modifiedOn, Guid modifiedBy, bool insertTransaction, string enrollmentStatusCode = "", bool insertPreviousEnrollmentSpan = false, string enrollmentStatusReason = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, termDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, insertTransaction, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASON_CODE", DbType.String, enrollmentStatusReason, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, enrollmentStatusCode, false));
                parameters.Add(SqlParms.CreateParameter("IS_FRM_JOB", DbType.Boolean, true, false));
                parameters.Add(SqlParms.CreateParameter("PROCESS_TYPE", DbType.String, "SUSPEND", false));
                DataSet dsTransID = DataAccess.ExecuteStoredProcedure("usp_TerminateProvider", parameters, "TransactionIds");


                return dsTransID;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void ReactivateProvider(int regID, DateTime newEffectiveDate, DateTime revalidationDate, string comments, string enrollmentStatusCode, DateTime modifiedOn, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("EFFECTIVE_DATE", DbType.DateTime, newEffectiveDate, false));
                parameters.Add(SqlParms.CreateParameter("REVALIDATION_DATE", DbType.DateTime, revalidationDate, false));
                parameters.Add(SqlParms.CreateParameter("COMMENTS", DbType.String, comments, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, enrollmentStatusCode, false));
                //parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, enrollmentStatusCode, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));

                DataAccess.ExecuteStoredProcedure("usp_ReactivateProvider", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void ReactivateReconsideredProvider(int regID, DateTime newEffectiveDate, DateTime revalidationDate, string comments, DateTime modifiedOn, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("EFFECTIVE_DATE", DbType.DateTime, newEffectiveDate, false));
                parameters.Add(SqlParms.CreateParameter("COMMENTS", DbType.String, comments, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                DataAccess.ExecuteStoredProcedure("usp_ReactivateReconsiderationProvider", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void RetroEffectiveDateProvider(int regID, DateTime effectiveDate, string comments, DateTime modifiedOn, Guid modifiedBy, bool insertTransaction, DateTime newRevalDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("EFFECTIVE_DATE", DbType.DateTime, effectiveDate, false));
                parameters.Add(SqlParms.CreateParameter("COMMENTS", DbType.String, comments, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, insertTransaction, false));
                parameters.Add(SqlParms.CreateParameter("NEW_REVAL_DATE", DbType.DateTime, newRevalDate, false));
                DataAccess.ExecuteStoredProcedure("usp_RetroEffectiveDateProvider", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet ChangeProviderTerminationDate(int regID, DateTime newTermDate, string comments, DateTime modifiedOn, Guid modifiedBy, bool insertTransaction)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("NEW_TERM_DATE", DbType.DateTime, newTermDate, false));
                parameters.Add(SqlParms.CreateParameter("COMMENTS", DbType.String, comments, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, insertTransaction, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_ChangeProviderTerminationDate", parameters, "TermTransactionIds");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet InsertExpressTerminatationWorkflow(int regID, DateTime createdOn, Guid createdBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("CreatedOn", DbType.DateTime, createdOn, false));
                parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, createdBy, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_InsertEXPRESS_TERMINATEWorkflow", parameters, "ProcessData_" + regID);
                ds.Tables[0].TableName = "ProcessData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet CheckSpanOnProvwithSameNPIonEffectiveDateChange(int regID, DateTime NewEffectiveDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("NEW_EFFECTIVE_DATE", DbType.DateTime, NewEffectiveDate, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_ChkSpanOnProvwithSameNPI_EffectiveDateChange", parameters, "SameNPIData_" + regID);
                ds.Tables[0].TableName = "SameNPIData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet PerformMoratoriaRematch(int regID, DateTime modifiedOn, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("ModifiedOn", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("ModifiedBy", DbType.Guid, modifiedBy, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_PerformMoratoria_RematchByRegID", parameters, "RegData");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Permanently delete all the Registration Data
        public static void DeleteRegistration(int regID, bool allowPostApplicationCompleteDelete)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("AllowPostApplicationCompleteDelete", DbType.Boolean, allowPostApplicationCompleteDelete, true));
                DataAccess.ExecuteStoredProcedure("usp_DeleteRegistration", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Delete of Registration failed for Reg ID: " + regID.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void AdminUpdateRegistrationKeyFields(int regID, string providerName, string firstName, string middleName, string lastName,
             string taxid, int taxIDTypeID, string npi,
            int taxonomyTypeID, string zipCode, string zipExt, String gender, DateTime modifiedOn, Guid modifiedBy, DateTime? birthDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderName", DbType.String, providerName, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("MiddleInitial", DbType.String, middleName, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("TaxIDTypeID", DbType.Int32, taxIDTypeID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("taxid", DbType.String, taxid, true));
                parameters.Add(SqlParms.CreateParameter("Gender", DbType.String, gender, true));

                if (taxonomyTypeID > 0)
                    parameters.Add(SqlParms.CreateParameter("TaxonomyTypeID", DbType.Int32, taxonomyTypeID, true));

                parameters.Add(SqlParms.CreateParameter("LocationZipCode", DbType.String, zipCode, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipExt", DbType.String, zipExt, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, modifiedOn, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, modifiedBy, true));
                parameters.Add(SqlParms.CreateParameter("BirthDate", DbType.DateTime, birthDate, true));

                DataAccess.ExecuteStoredProcedure("usp_UpdateByAdminRegistrationKeyFields", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateRegistrationKeyFields(int regID, string providerName, string firstName, string middleName, string lastName,
             int taxIDTypeID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, int specialtyTypeID,
            int taxonomyTypeID, string practiceName, string zipCode, string zipExt, string gender, DateTime modifiedOn, Guid modifiedBy, DateTime npiStartDate,
            DateTime? npiEndDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderName", DbType.String, providerName, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("MiddleInitial", DbType.String, middleName, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("TaxIDTypeID", DbType.Int32, taxIDTypeID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderCategoryTypeID", DbType.Int32, providerCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("TypeofPracticeID", DbType.Int32, practiceTypeID, true));

                if (specialtyTypeID > 0)
                    parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, specialtyTypeID, true));
                if (taxonomyTypeID > 0)
                    parameters.Add(SqlParms.CreateParameter("TaxonomyTypeID", DbType.Int32, taxonomyTypeID, true));

                parameters.Add(SqlParms.CreateParameter("PracticeName", DbType.String, practiceName, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipCode", DbType.String, zipCode, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipExt", DbType.String, zipExt, true));
                parameters.Add(SqlParms.CreateParameter("Gender", DbType.String, gender, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, modifiedOn, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, modifiedBy, true));
                parameters.Add(SqlParms.CreateParameter("NPIStartDate", DbType.DateTime, npiStartDate, true));
                parameters.Add(SqlParms.CreateParameter("NPIEndDate", DbType.DateTime, npiEndDate, true));

                DataAccess.ExecuteStoredProcedure("usp_UpdateRegistrationKeyFields", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet InsertIntoVersionTables(int RegID, string userId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegID, false));
                parameters.Add(SqlParms.CreateParameter("User", DbType.Guid, userId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_InsertIntoVersionTableByRegID", parameters, "VersionTables_" + RegID);

                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "VersionTables";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG Id: " + RegID.ToString() +
                    ", User Id: " + userId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void CancelRegistration(int regID, DateTime modifiedDate, Guid modifiedBy, int processID, string commandName = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, modifiedDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, modifiedBy, true));
                parameters.Add(SqlParms.CreateParameter("CommandName", DbType.String, commandName, true));
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, false));

                DataAccess.ExecuteStoredProcedure("usp_CancelProviderRegistration", parameters);
            }
            catch (Exception ex)
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);

                Logging log = new Logging(new Guid(), logMsg);
                log.CreateLogEntry("Error on usp_CancelProviderRegistration :" + ex.ToString(), Logging.LogPriority.Error);
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    log.CreateLogEntry("Error on usp_CancelProviderRegistration(Inner Ex) :" + ex.InnerException.Message, Logging.LogPriority.Error);
                }
                throw CoreException.ThrowException(ex);
            }
        }

        public static void CancelRegistrationCPC(int regID, DateTime modifiedDate, Guid modifiedBy, int processID, int workflowEventTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, processID, false));
                parameters.Add(SqlParms.CreateParameter("WORKFLOW_EVENT_TYPE_ID", DbType.Int32, workflowEventTypeID, false));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, modifiedDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, modifiedBy, true));

                DataAccess.ExecuteStoredProcedure("usp_CancelWorkflow_CPC", parameters);
            }
            catch (Exception ex)
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);

                Logging log = new Logging(new Guid(), logMsg);
                log.CreateLogEntry("Error on usp_CancelProviderRegistration :" + ex.ToString(), Logging.LogPriority.Error);
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    log.CreateLogEntry("Error on usp_CancelProviderRegistration(Inner Ex) :" + ex.InnerException.Message, Logging.LogPriority.Error);
                }
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectRegSectionUploadControl(int regPageTypeId, int? applicationTypeID, int? providerTypeId, int? providerCategoryTypeId, string regPageSection, int? reg_id)
        {


            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Application_Type_Id", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("Provider_Category_Type_Id", DbType.Int32, providerCategoryTypeId, true));
                parameters.Add(SqlParms.CreateParameter("Provider_Type_Id", DbType.Int32, providerTypeId, true));
                parameters.Add(SqlParms.CreateParameter("Reg_Page_Type_Id", DbType.Int32, regPageTypeId, false));
                parameters.Add(SqlParms.CreateParameter("Reg_Page_Section", DbType.String, regPageSection, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_id, true));

                DataSet ds = new DataSet();
                // string storedProc = "usp_SelectREG_SECTIONUPLOADCONTROL";
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SECTION_UPLOAD_CONTROL", parameters, "RegSectionUploadControl_" + regPageTypeId);
                ds.Tables[0].TableName = "RegSectionUploadControl";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteRegistrationDocument(int documentId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("document_Id", DbType.Int32, documentId, false));

                DataAccess.ExecuteStoredProcedure("usp_DeleteProviderDocument", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }

        public static DataSet SelectRegSectionUploadDocument(int regPageTypeId, int regID, int? providerTypeId, string regPageSection, int rowId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_Page_Type_Id", DbType.Int32, regPageTypeId, false));
                parameters.Add(SqlParms.CreateParameter("Reg_Id", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("Reg_Page_Section", DbType.String, regPageSection, false));
                parameters.Add(SqlParms.CreateParameter("Provider_Type_Id", DbType.Int32, providerTypeId, true));
                parameters.Add(SqlParms.CreateParameter("row_Id", DbType.Int32, rowId, true));

                DataSet ds = new DataSet();
                string storedProc = "usp_SelectREG_SECTION_UPLOAD_DOCUMENT";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "RegSectionUploadDocument_" + regID);
                ds.Tables[0].TableName = "RegSectionUploadDocument";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegSectionUploadControlandDocument(int regPageTypeId, int? applicationTypeID, int? providerTypeId, int? providerCategoryTypeId, string regPageSection, int? reg_id, int rowId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Application_Type_Id", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("Provider_Category_Type_Id", DbType.Int32, providerCategoryTypeId, true));
                parameters.Add(SqlParms.CreateParameter("Provider_Type_Id", DbType.Int32, providerTypeId, true));
                parameters.Add(SqlParms.CreateParameter("Reg_Page_Type_Id", DbType.Int32, regPageTypeId, false));
                parameters.Add(SqlParms.CreateParameter("Reg_Page_Section", DbType.String, regPageSection, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_id, true));
                parameters.Add(SqlParms.CreateParameter("ROW_ID", DbType.Int32, rowId, true));

                DataSet ds = new DataSet();
                string storedProc = "usp_SelectREG_SECTION_UPLOAD_CONTROL_DOCUMENT";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "RegSectionUploadControlDocument_" + reg_id);
                ds.Tables[0].TableName = "RegSectionUploadControlDocument";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelecIncidentComplianceDocument(int regPageTypeId, int? applicationTypeID, int? providerTypeId, int? providerCategoryTypeId, string regPageSection, int? reg_id, int rowId, string IncidentCaseNum)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Application_Type_Id", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("Provider_Category_Type_Id", DbType.Int32, providerCategoryTypeId, true));
                parameters.Add(SqlParms.CreateParameter("Provider_Type_Id", DbType.Int32, providerTypeId, true));
                parameters.Add(SqlParms.CreateParameter("Reg_Page_Type_Id", DbType.Int32, regPageTypeId, false));
                parameters.Add(SqlParms.CreateParameter("Reg_Page_Section", DbType.String, regPageSection, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_id, true));
                parameters.Add(SqlParms.CreateParameter("ROW_ID", DbType.Int32, rowId, true));
                parameters.Add(SqlParms.CreateParameter("INCIDENT_CASE_NUMBER", DbType.String, IncidentCaseNum, false));

                DataSet ds = new DataSet();
                string storedProc = "usp_Select_IncidentCompliance_Documents";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "IncidentComplianceDocument_" + reg_id);
                ds.Tables[0].TableName = "IncidentComplianceDocument";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPaperDocuments(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet ds = new DataSet();
                string storedProc = "usp_SelectPaperDocuments";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "PaperDocuments_" + regID);
                ds.Tables[0].TableName = "PaperDocuments";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectConvertedDocuments(int regID, int pageNumber, int pageSize)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageNumber, false));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.Int32, pageSize, false));

                DataSet ds = new DataSet();
                string storedProc = "usp_SelectConvertedDocuments";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "ConvertedDocuments_" + regID);
                ds.Tables[0].TableName = "ConvertedDocuments";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectWorkflowEventTypeByRegId(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = new DataSet();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regId, true));
                string storedProc = "usp_SelectWorkflowEventTypeByRegId";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "WorkflowEventType_" + regId);
                ds.Tables[0].TableName = "WorkflowEventType";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAllAffiliationsByRegId(int regId, int affiliationStatus, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet lookup = new DataSet();
                string outValue;
                totalResultCount = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();
                if (regId > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("AffliationStatus", DbType.Boolean, affiliationStatus, true));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAllAffiliationsByRegId", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                lookup.Tables[0].TableName = "RegData";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectSubmittedAgreementPDFs(int regId, int regPageTypeID, string regPageSection, int pageSize, int startRowIndexSub, int startRowIndexUpd)
        {
            try
            {
                DataSet lookup = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();
                if (regId > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, regPageSection, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndexSub", DbType.String, startRowIndexSub, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndexUpd", DbType.String, startRowIndexUpd, false));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SELECT_SUBMITTED_AGREEMENT_PDFS", parameters, "RegPdfData");
                lookup.Tables[0].TableName = "RegPdfData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectMCPAffiliationsByRegId(int regId, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet lookup = new DataSet();
                string outValue;
                totalResultCount = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();
                if (regId > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));
                /*parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, true));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("AffliationStatus", DbType.String, ssn, true));*/

                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectMCPAffiliationsByRegId", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                lookup.Tables[0].TableName = "RegData";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SearchAffiliationsByRegId(int regId, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, string name, string npi, string ssn, out int totalResultCount)
        {
            try
            {
                DataSet lookup = new DataSet();
                string outValue;
                totalResultCount = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();
                if (regId > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, true));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("AffliationStatus", DbType.String, ssn, true));

                lookup = DataAccess.ExecuteStoredProcedure("usp_SearchAffiliationsByRegId", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                lookup.Tables[0].TableName = "RegData";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectWFStepInfo(int stepId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = new DataSet();
                parameters.Add(SqlParms.CreateParameter("stepId", DbType.Int32, stepId, true));
                string storedProc = "usp_Select_WFStepInfo";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "WFStepInfo");
                ds.Tables[0].TableName = "WFStepInfo";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet VerifyCurrentAssignedUserForStep(Guid userId, int stepId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = new DataSet();
                parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("STEP_ID", DbType.Int32, stepId, true));
                string storedProc = "usp_VerifyCurrentAssignedUserForStep";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "WFAssignedStepInfo");
                ds.Tables[0].TableName = "WFAssignedStepInfo";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetAffiliateSpecialtyExportData(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = new DataSet();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_GetAffiliateSpecialtyExportData", parameters, "AffSpecData");
                ds.Tables[0].TableName = "AffSpecData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetAffiliateLicenseExportData(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = new DataSet();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_GetAffiliateLicenseExportData", parameters, "AffSpecData");
                ds.Tables[0].TableName = "AffLicenseData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectCurrentWFTaskInfo(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = new DataSet();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regId, true));
                string storedProc = "usp_SelectREG_CurrentWFTaskInfo";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "WFTaskInfo_" + regId);
                ds.Tables[0].TableName = "WFTaskInfo";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetWorkflowStepsForRegID(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("regId", DbType.Guid, regId, false));

                DataSet getWorkflowStepsForRegID = new DataSet();
                getWorkflowStepsForRegID = DataAccess.ExecuteStoredProcedure("usp_GetWorkflowStepsForRegID", parameters, "GetWorkflowStepsForRegID_" + regId);
                getWorkflowStepsForRegID.Tables[0].TableName = "getWorkflowStepsForRegID";

                return getWorkflowStepsForRegID;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet VerifyCPCKidsAttestationSelected(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_VerifyCPCKidsAttestationSelected", parameters, "CPCKidsAttestationSelected_" + regID);
                ds.Tables[0].TableName = "CPCKidsAttestationSelected";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectRegEnrollmentByRegID(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_ID", DbType.Guid, regId, false));
                //DataSet enrollmentds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ENROLLMENT", sqlParms1, "regData");
                DataSet enrollmentds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ENROLLMENT", parameters, "usp_SelectREG_ENROLLMENT");
                enrollmentds.Tables[0].TableName = "usp_SelectREG_ENROLLMENT";

                return enrollmentds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetMMISTransactionsForRegistration(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("regId", DbType.Int32, regId, false));

                return DataAccess.ExecuteStoredProcedure("usp_GetMMISTransactions", parameters, "TransactionList");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetWorkflowStepsForProcessID(int processId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, processId, false));

                DataSet getWorkflowStepsForRegID = new DataSet();
                getWorkflowStepsForRegID = DataAccess.ExecuteStoredProcedure("usp_GetWorkflowStepsForProcessID", parameters, "GetWorkflowStepsForProcessID_" + processId);
                getWorkflowStepsForRegID.Tables[0].TableName = "getWorkflowStepsForProcessID";

                return getWorkflowStepsForRegID;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetApplicationDetailsbyProcessID(int ProcessId, int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessId, false));

                DataSet getAppDtlsForRegID = new DataSet();
                getAppDtlsForRegID = DataAccess.ExecuteStoredProcedure("usp_GetApplicationDtlsByProcessIDandRegID", parameters, "GetAppDtlsForProcessID_" + ProcessId);
                getAppDtlsForRegID.Tables[0].TableName = "getAppDtlsForProcessID";

                return getAppDtlsForRegID;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetWorkflowProcessesForRegID(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                DataSet getWorkflowProcessesForRegID = new DataSet();
                getWorkflowProcessesForRegID = DataAccess.ExecuteStoredProcedure("usp_GetWorkflowProcessesForRegID", parameters, "GetWorkflowProcessesForRegID_" + regId);
                getWorkflowProcessesForRegID.Tables[0].TableName = "getWorkflowStepsForProcessID";

                return getWorkflowProcessesForRegID;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectTimesApplicationReturnedToProviderFromLTC(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectLTCReturnToProvider", parameters, "RegData");

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void DeleteRegMCOAffiliation(int regMCOAffiliationID)
        {
            if (regMCOAffiliationID <= 0)
                return;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_MCO_AFFILIATION_ID", DbType.Int32, regMCOAffiliationID, true));

                DataAccess.ExecuteStoredProcedure("usp_DeleteREG_MCO_AFFILIATION", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateToConvertToFeeForService(int currentRegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("currentRegID", DbType.Int32, currentRegID, false));

                DataAccess.ExecuteStoredProcedure("usp_UpdateToConvertToFeeForService", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool HasNewOwnersOrChangeInOwnerInformation(int regID)
        {
            try
            {
                bool rtn = false;

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_OWNER", sqlParms, "RegData");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (ObjectControllerHelper.HasRows(ds) && ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", dr) == CON.RegistrationModifiedStatusType.Changed || ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", dr) == CON.RegistrationModifiedStatusType.Inserted)
                        rtn = true;
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool HasPrimaryPracticeLocationChange(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                //OHPNM-859 pschwarz 11/4/2020: added insert to trigger change as well         
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                sqlParms.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, Constants.AddressType.PrimaryPractice, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDRESSCustom", sqlParms, "RegData");
                if (ObjectControllerHelper.HasRows(ds) &&
                    (ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", ds.Tables[0].Rows[0]) == CON.RegistrationModifiedStatusType.Changed ||
                     ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", ds.Tables[0].Rows[0]) == CON.RegistrationModifiedStatusType.Inserted))
                    rtn = true;
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool HasDelegateChanged(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                //OHPNM-859 pschwarz 11/4/2020: added insert to trigger change as well         
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ProviderDelegateChangedByRegID", sqlParms, "RegData");
                if (ObjectControllerHelper.HasRows(ds) &&
                    (ObjectControllerHelper.GetBool("ODM_Credentialing_Delegates_Changed", ds.Tables[0].Rows[0])))
                    rtn = true;
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        //ap
        public static DataSet SelectServiceID(string Id, bool regIDFlag = false)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                DataSet ds = null;
                if (regIDFlag)
                {
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.String, Id, false));
                    ds = DataAccess.ExecuteStoredProcedure("usp_SelectFinancialProviderInfo_ByRegID", sqlParms, "ServiceLocation");
                }
                else
                {
                    sqlParms.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, Id, false));
                    ds = DataAccess.ExecuteStoredProcedure("usp_SelectFinancialProviderInformation", sqlParms, "ServiceLocation");
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectProviderScreeningByProcessID(int processID)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("process_ID", DbType.Int32, processID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectProviderScreeningByProcessID", sqlParms, "RegData");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet IsRegistrationWentThroughAppeals(int RegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.String, RegID, false));

                return DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationByRegIDWentThroughAppeals", parameters, "RegData");


            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetRegistrationAutoApproveSections(int RegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_ID", DbType.String, RegID, false));

                return DataAccess.ExecuteStoredProcedure("usp_Select_REG_PAGE_SETTING_AutoApprove", parameters, "RegData");


            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet UpdateRegForReactivateByProvider(int regId, DateTime modifiedDate, string modifiedBy, int workflowID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, modifiedBy, true));
                parameters.Add(SqlParms.CreateParameter("WorkflowID", DbType.Int32, workflowID, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_UpdateRegForReactivateByProvider", parameters, "usp_UpdateRegForReactivateByProvider");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAddressInfoData(int regId, int addressTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, addressTypeId, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectAddressInfo";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "AddressInfo");
                ds.Tables[0].TableName = "AddressInfo";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet CheckOwnerAddressExists(int regId, int addressTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, addressTypeId, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_CheckForOwnerInfo";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "AddressInfo");
                ds.Tables[0].TableName = "AddressInfo";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int GetElapsedTimeLimitByApplicationType(int applicationTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeId, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ElapsedTimeByApplicationType", parameters, "ElapsedTimeLimit");
                ds.Tables[0].TableName = "ElapsedTimeLimit";
                if (ObjectControllerHelper.HasRows(ds))
                {
                    return Convert.ToInt32(ds.Tables["ElapsedTimeLimit"].Rows[0]["ELAPSED_EVENT_TIME_HOURS"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("APPLICATION_TYPE_ID: " + applicationTypeId.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int GetElapsedDaysByApplicationType(int applicationTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeId, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ElapsedDaysApplicationType", parameters, "ElapsedDayLimit");
                ds.Tables[0].TableName = "ElapsedDayLimit";
                if (ObjectControllerHelper.HasRows(ds))
                {
                    return Convert.ToInt32(ds.Tables["ElapsedDayLimit"].Rows[0]["ELAPSED_EVENT_TIME_DAYS"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("APPLICATION_TYPE_ID: " + applicationTypeId.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectTaxInfo(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_TAX_INFO", parameters, "TaxInfo_" + regId);
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderDisenrollmentxref()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_DISENROLLMENT_XREF");
                ds.Tables[0].TableName = "ProviderDisenrollmentxref";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderDisenrollment(int regId)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER_DISENROLLMENT", sqlParms, "REG_ID");
                ds.Tables[0].TableName = "RegProviderDisenrollment";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectTaxClass()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectTAX_CLASS", "TaxClass_");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateTaxInfo(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                DataAccess.ExecuteStoredProcedure("updateREG_TAX_INFO", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static void InsertTaxInfo(int regid, Dictionary<string, string> parms)
        {
            if (regid <= 0)
                return;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (pair.Value == null)
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                DataAccess.ExecuteStoredProcedure("insertREG_TAX_INFO", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet VerifyZipCodeIsOH(string zipCode) //akash
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("ZipCode", DbType.Int32, zipCode, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("USP_Verify_Zip_Code_Is_OH", sqlParms, "zipCode");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPublicProviderLblDataByRegID(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectPublicProviderLblDataByRegID", parameters, "PublicProviderData_" + regId);
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPublicProviderLblLocDataByRegID(int regId, int regAddressId = 0)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId, true));
                if (regAddressId > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("REG_ADDRESS_ID", DbType.String, regAddressId, true));
                }
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectPublicProviderLblLocDataByRegID", parameters, "PublicProviderLocData_" + regId);
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegOwnerByOwnerID(int OwnerID)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Owner_ID", DbType.Int32, OwnerID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectRegOwnerByOwnerID", sqlParms, "OwnerData");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCurrentAndPreviousApplicationsByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetCurrentandPreviousApplicationsForRegID", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "ApplicationData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegSectionUploadControlByRegIDAndPageSection(int regID, string regPageSection)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, regPageSection, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetRegSectionUploadControlByRegIDAndPageSection", parameters, "RegSectionData");

                lookup.Tables[0].TableName = "ApplicationData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectRegistrationsByMedicaidID(string medicaid_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GRPMedicaid_ID", DbType.String, medicaid_ID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationsByMedicaidID", parameters, "ExitingProvider");

                lookup.Tables[0].TableName = "ApplicationData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectExitingProviderByMedicaidID(string medicaid_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GRPMedicaid_ID", DbType.String, medicaid_ID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectExistingProviderByMedicaidID", parameters, "ExitingProvider");

                lookup.Tables[0].TableName = "ApplicationData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertRegChopParent(Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, object> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                DataAccess.ExecuteStoredProcedure("insertREG_CHOP_PARENT", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertRegChopPrimaryServiceAddress(Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, object> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                DataAccess.ExecuteStoredProcedure("usp_InsertCHOPParentPrimaryServiceAddress", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetRegWaiverServices(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetRegWaiverServicesForRegID", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "ApplicationData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }



        public static void UpdateMedicaidIDByRegID(int regID, string newMediciadID, DateTime createdDateTime, string createdBy)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                sqlParms.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, newMediciadID, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, createdDateTime, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("sp_UpdateMedicaidIDByRegID", sqlParms);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateHospitalNoByRegID(int regID, string newHOSPITAL_NO, DateTime createdDateTime, string createdBy)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                sqlParms.Add(SqlParms.CreateParameter("HOSPITAL_NO", DbType.String, newHOSPITAL_NO, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, createdDateTime, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateHospitalNoByRegID", sqlParms);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateHealthCenterNoByRegID(int regID, string newHealth_Center_No, DateTime createdDateTime, string createdBy)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                sqlParms.Add(SqlParms.CreateParameter("Health_Center_No", DbType.String, newHealth_Center_No, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, createdDateTime, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateHealthCenterNoByRegID", sqlParms);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateNursingFacilityNoByRegID(int regID, string newNursing_Facility_No, DateTime createdDateTime, string createdBy)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                sqlParms.Add(SqlParms.CreateParameter("Nursing_Facility_No", DbType.String, newNursing_Facility_No, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, createdDateTime, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateNursingFacilityNoByRegID", sqlParms);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateFacilityNumber(int opRegID, int facilityRegID, DateTime createdDateTime, string createdBy)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("OperatorREG_ID", DbType.Int32, opRegID, false));
                sqlParms.Add(SqlParms.CreateParameter("FacilityREG_ID", DbType.Int32, facilityRegID, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, createdDateTime, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateFacilityNumber", sqlParms);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool HasNameOwnerPracticeChange(int regID)
        {
            try
            {
                bool rtn = false;
                bool practiceLocationModified = false;
                bool ownerModified = false;
                bool nameModified = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                DataSet ds;
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ModifiedStatusTypeByRegID", sqlParms, "RegDataMST");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    // go through each name/owner/location change for all rows returned and keep track of all changes
                    foreach (DataRow dataRow in ds.Tables[0].Rows)
                    {
                        // OHPNM-2724 - changed from looking for type "Change" to looking for any type that's not "NoChange" (deleted, inserted, or changed) (and not null == 0) 
                        if (!nameModified)
                        {
                            nameModified = (ObjectControllerHelper.GetInt("NAME_MODIFIED_STATUS_TYPE_ID", dataRow) != CON.RegistrationModifiedStatusType.NoChange && ObjectControllerHelper.GetInt("NAME_MODIFIED_STATUS_TYPE_ID", dataRow) != 0);
                        }
                        if (!ownerModified)
                        {
                            ownerModified = (ObjectControllerHelper.GetInt("OWNER_MODIFIED_STATUS_TYPE_ID", dataRow) != CON.RegistrationModifiedStatusType.NoChange && ObjectControllerHelper.GetInt("OWNER_MODIFIED_STATUS_TYPE_ID", dataRow) != 0);
                        }
                        if (!practiceLocationModified)
                        {
                            practiceLocationModified = (ObjectControllerHelper.GetInt("PRACTICE_MODIFIED_STATUS_TYPE_ID", dataRow) != CON.RegistrationModifiedStatusType.NoChange && ObjectControllerHelper.GetInt("PRACTICE_MODIFIED_STATUS_TYPE_ID", dataRow) != 0);
                        }
                    }
                }
                if (ownerModified || nameModified)
                {
                    rtn = true;
                }
                else if (practiceLocationModified)
                {
                    rtn = true;
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool HasOwnerChange(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                DataSet ds;
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_OWNER", sqlParms, "RegDataMST");
                if (ObjectControllerHelper.HasRows(ds) &&
                    (ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", ds.Tables[0].Rows[0])
                   == CON.RegistrationModifiedStatusType.Changed ||
                     ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", ds.Tables[0].Rows[0]) == CON.RegistrationModifiedStatusType.Inserted))
                    rtn = true;
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool HasSpecialtyChange(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SPECIALTYByRegID", sqlParms, "RegDataRS");
                DataSet dsWF = GetWorkflowStepsForRegID(regID);
                DateTime dtProcessStartDate = new DateTime();
                if (ObjectControllerHelper.HasRows(dsWF.Tables[1]))
                    dtProcessStartDate = ObjectControllerHelper.GetDateTime("START_DATE_TIME", dsWF.Tables[1].Rows[0]);
                //OHPNM-859 pschwarz 11/4/2020: added insert to trigger change as well 
                if (ObjectControllerHelper.HasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", dr) == CON.RegistrationModifiedStatusType.Changed
                            || ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", dr) == CON.RegistrationModifiedStatusType.Inserted)
                        {
                            DateTime dtLastModifiedDate = ObjectControllerHelper.GetDateTime("LAST_MODIFIED_DATE_TIME", dr);
                            if (dtLastModifiedDate != null && dtProcessStartDate != null && dtLastModifiedDate >= dtProcessStartDate)
                            {
                                rtn = true;
                                break;
                            }
                        }
                    }


                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool HasLicenseChange(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_LICENSES", sqlParms, "RegDataRS");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", dr) == CON.RegistrationModifiedStatusType.Changed)
                        {
                            rtn = true;
                            break;
                        }
                    }
                }

                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool HasOwnerChangedOrInsertedForHighRisk(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectOwnerChangeForHighRiskByRegID", sqlParms, "RegDataRS");
                //OHPNM-859 pschwarz 11/7/2020: added insert to trigger change as well 
                if (ObjectControllerHelper.HasRows(ds))
                {
                    bool isOwnerInsertedOrChanged = ds.Tables[0].AsEnumerable()
                                  .Where(r => r.Field<int>("MODIFIED_STATUS_TYPE_ID") == CON.RegistrationModifiedStatusType.Inserted ||
                                             r.Field<int>("MODIFIED_STATUS_TYPE_ID") == CON.RegistrationModifiedStatusType.Changed)

                                  .Any();

                    rtn = isOwnerInsertedOrChanged;
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool HasOwnerInsertedForHighRisk(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectOwnerChangeForHighRiskByRegID", sqlParms, "RegDataRS");
                //OHPNM-859 pschwarz 11/7/2020: added insert to trigger change as well 
                if (ObjectControllerHelper.HasRows(ds))
                {
                    bool isOwnerInserted = ds.Tables[0].AsEnumerable()
                                   .Where(r => r.Field<int>("MODIFIED_STATUS_TYPE_ID") == CON.RegistrationModifiedStatusType.Inserted)
                                   .Any();

                    rtn = isOwnerInserted;
                }

                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool HasPracticeChangeForHighRisk(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectPracticeChangeForHighRiskByRegID", sqlParms, "RegDataRS");
                //OHPNM-859 pschwarz 11/7/2020: added insert to trigger change as well 
                if (ObjectControllerHelper.HasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", dr) == CON.RegistrationModifiedStatusType.Changed ||
                            ObjectControllerHelper.GetInt("MODIFIED_STATUS_TYPE_ID", dr) == CON.RegistrationModifiedStatusType.Inserted)
                        {
                            rtn = true;
                            break;
                        }
                    }
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckIfEnrolledProvider(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckIfEnrolledProviderByRegID", sqlParms, "RegDataRS");
                if (ObjectControllerHelper.HasRows(ds) && !string.IsNullOrEmpty(ObjectControllerHelper.GetString("MEDICAID_ID", ds.Tables[0].Rows[0])))
                    rtn = true;
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckHRTermFieldsByRegID(int regID)
        {
            try
            {
                bool rtn = false;
                string termDate = string.Empty;
                string termReason = string.Empty;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckHRTermFieldsByRegID", sqlParms, "RegDataHRF");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    termDate = ObjectControllerHelper.GetString("DATE_TERMINATION", ds.Tables[0].Rows[0]);
                    termReason = ObjectControllerHelper.GetString("TERMINATION_REASON", ds.Tables[0].Rows[0]);
                }
                if (!string.IsNullOrEmpty(termDate) && !string.IsNullOrWhiteSpace(termDate) && !termReason.Equals("0"))
                {
                    rtn = true;
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetHRTermFieldsByRegID(int regID)
        {
            try
            {
                string termDate = string.Empty;
                string termReason = string.Empty;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckHRTermFieldsByRegID", sqlParms, "RegDataHRF");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetRRTermFieldsByRegID(int regID)
        {
            try
            {
                string termDate = string.Empty;
                string termReason = string.Empty;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckRRTermFieldsByRegID", sqlParms, "RegDataHRF");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckIfHearingRightsEligible(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckIfHearingRightsByRegID", sqlParms, "RegDataCHR");
                if (ObjectControllerHelper.HasRows(ds))
                    rtn = true;
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckIfReconsiderationRequested(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckReconsiderationReceivedByRegID", sqlParms, "RegDataCRR");
                if (ObjectControllerHelper.HasRows(ds))
                    rtn = true;
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateRegProviderEnrollmentStatusCode(int RegID, string EnrollmentStatusCode, DateTime changedDate, Guid modifiedBy, DateTime enddate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegID, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, EnrollmentStatusCode, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("END_DATE", DbType.DateTime, enddate, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateEnrollmentStatusByRegID", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateRSLFacilityHomeNumber(int regID, DateTime LTEffectiveDate, int homeNumber, string comment, DateTime changedDate, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("LT_EFFECTIVE_DATE", DbType.DateTime, LTEffectiveDate, false));
                parameters.Add(SqlParms.CreateParameter("LT_HOME_NUMBER", DbType.Int32, homeNumber, false));
                parameters.Add(SqlParms.CreateParameter("LT_COMMENTS", DbType.String, comment, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateRSLFacilityHomeNumberByRegID", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertUpdateODHFacilityHomeNumber(int regID, DateTime LTEffectiveDate, int homeNumber, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LT_HOME_NUMBER", DbType.Int32, homeNumber, false));
                parameters.Add(SqlParms.CreateParameter("LT_EFFECTIVE_DATE", DbType.DateTime, LTEffectiveDate, false));
                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateODHFacilityHomeNumberByRegID", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet VerifyTaxIdExistsAsEINForSSN(string taxID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, taxID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_VerifyTaxIdExistsAsEINForSSN", parameters, "ExitingProvider");

                lookup.Tables[0].TableName = "ApplicationData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectIncidentCaseDetails(string caseNumber)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("INCIDENT_CASE_NUMBER", DbType.String, caseNumber, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_INCIDENT_COMPLIANCE_CASE_DETAIL", parameters, "IncidentCaseDtl");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet InsertIncidentFlags(int regId, bool POCBYMAIL, string lastmodifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                //parameters.Add(SqlParms.CreateParameter("CASE_NUMBER", DbType.String, caseNumber, false));
                parameters.Add(SqlParms.CreateParameter("POCBYMAIL", DbType.Boolean, POCBYMAIL, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, lastmodifiedUser, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_insert_INCIDENTFLAGS", parameters, "NODIDdt");

                lookup.Tables[0].TableName = "NODIDdt";


                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet UpdateIncidentData(string caseNumber, bool NODNeeded, DateTime NODIssuedDate, string csNotes, string Incident_id, string lastmodifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CASE_NUMBER", DbType.String, caseNumber, false));
                parameters.Add(SqlParms.CreateParameter("NOD_NEEDED", DbType.Boolean, NODNeeded, false));
                parameters.Add(SqlParms.CreateParameter("NOD_ISSUED_DATE", DbType.DateTime, NODIssuedDate, false));
                parameters.Add(SqlParms.CreateParameter("CS_NOTES", DbType.String, csNotes, false));
                parameters.Add(SqlParms.CreateParameter("INCIDENT_ID", DbType.String, Incident_id, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, lastmodifiedUser, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_update_REG_INCIDENT_COMPLIANCE_CASE_DETAIL", parameters, "IncidentDtls");

                lookup.Tables[0].TableName = "IncidentDtls";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdatePOCMail(int regId, bool pocBymail, string lastmodifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("POCBYMAIL", DbType.Boolean, pocBymail, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, lastmodifiedUser, false));

                //DataSet lookup = new DataSet();
                DataAccess.ExecuteStoredProcedure("sp_update_POCBYMAIL", parameters, "IncidentDtls");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static int LinkLTToLTCProvider(int facilityLTRegID, string firstName, string lastName, string providerName, string npi, string MedicaidID,
            string startDate, string endDate, string lastmodifiedDate, string lastmodifiedUser, int affiliationStatus, string modifiedStatus, string createdDate,
            string createdBy)
        {
            try
            {
                List<SqlParameter> affParams = new List<SqlParameter>();
                affParams.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, facilityLTRegID, false));
                affParams.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, firstName, false));
                affParams.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, lastName, false));
                affParams.Add(SqlParms.CreateParameter("NAME", DbType.String, providerName, false));
                affParams.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                affParams.Add(SqlParms.CreateParameter("Medicaid_ID", DbType.String, MedicaidID, false));
                affParams.Add(SqlParms.CreateParameter("START_DATE", DbType.DateTime, startDate, false));
                affParams.Add(SqlParms.CreateParameter("END_DATE", DbType.DateTime, endDate, false));
                affParams.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastmodifiedDate, false));
                affParams.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastmodifiedUser, false));
                affParams.Add(SqlParms.CreateParameter("GROUP_AFFILIATION_STATUS_ID", DbType.Int32, affiliationStatus, false));
                affParams.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, modifiedStatus, false));
                affParams.Add(SqlParms.CreateParameter("Created_On_Date_Time", DbType.DateTime, createdDate, false));
                affParams.Add(SqlParms.CreateParameter("Created_By_User", DbType.Guid, createdBy, false));
                int regAffiliationID = Convert.ToInt32(DataAccess.ExecuteScalar("insertREG_AFFILIATIONcustom", affParams));
                return regAffiliationID;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetMaxNursingFacilityNo()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetMaxNursingFacilityNoFromRegServiceLocation", "checkMedicaidID");
                lookup.Tables[0].TableName = "checkMedicaidID";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetMaxHospitalNo()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetMaxHospitalNoFromRegServiceLocation", "checkMedicaidID");
                lookup.Tables[0].TableName = "checkMedicaidID";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetMaxHealthCenterNo()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetMaxHealthCenterNoFromRegServiceLocation", "checkMedicaidID");
                lookup.Tables[0].TableName = "checkMedicaidID";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GenerateMedicaidID()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GenerateMedicaidID");
                lookup.Tables[0].TableName = "checkMedicaidID";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void DeleteRegRestriction(int regRestrictionID)
        {
            if (regRestrictionID <= 0)
                return;


            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_RESTRICTION_ID", DbType.Int32, regRestrictionID, true));

                DataAccess.ExecuteStoredProcedure("usp_DeleteREG_RESTRICTION", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetRestrictedServicesHistory(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetRestrictedServicesHistory", parameters, "GAPage_" + regId);
                ds.Tables[0].TableName = "RSHistory";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetCHOPFacilityAffiliationRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetCHOPFacilityRegID", parameters, "chopFacilityID");
                lookup.Tables[0].TableName = "chopFacilityID";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckIfBCIIorFBINeeded(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_CheckIfOhioResidentByRegID", sqlParms, "RegDataCOR");
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (ObjectControllerHelper.HasRows(dsReg))
                {
                    bool isOHResident = ObjectControllerHelper.GetBool("IS_OHIO_RESIDENT", drReg);
                    if (isOHResident)
                    {
                        rtn = true;
                    }
                }

                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void SetUserActiveStatus(Guid agentuserId, Guid adminuserId, bool status, int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("AgentUserId", DbType.Guid, agentuserId, false));
                parameters.Add(SqlParms.CreateParameter("AdminUserId", DbType.Guid, adminuserId, false));
                parameters.Add(SqlParms.CreateParameter("Status", DbType.Guid, status, false));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Guid, regID, false));
                DataAccess.ExecuteStoredProcedure("usp_SetUserActiveStatus", parameters);

                return;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void ChangeStatusDeniedProvider(DateTime changedDate, Guid changedBy, int enrollStatusReason, int enrollStatus, int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASON_ID", DbType.Int32, enrollStatusReason, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.Int32, enrollStatus, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                DataAccess.ExecuteStoredProcedure("updateREG_PROVIDERCustom", parameters);

                return;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateBuildingEnrollmentSpan(int regAffiliaitonID, int facility_regID, string providerName, string NPI, string nextMED_ID, int providerTypeID,
            DateTime start_date, DateTime end_date, DateTime lastmodifiedByDate, Guid lastmodifiedByUser, int affiliationStatus, int modifiedStatus,
            DateTime changedDate, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_AFFILIATION_ID", DbType.Int32, regAffiliaitonID, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, facility_regID, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_NAME", DbType.String, providerName, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, false));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, nextMED_ID, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("GROUP_AFFILIATION_STATUS_ID", DbType.Int32, affiliationStatus, false));
                parameters.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, modifiedStatus, false));
                parameters.Add(SqlParms.CreateParameter("START_DATE", DbType.DateTime, start_date, false));
                parameters.Add(SqlParms.CreateParameter("END_DATE", DbType.DateTime, end_date, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastmodifiedByDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastmodifiedByUser, false));
                parameters.Add(SqlParms.CreateParameter("Created_On_Date_Time", DbType.DateTime, changedDate, false));
                parameters.Add(SqlParms.CreateParameter("Created_By_User", DbType.Guid, modifiedBy, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateBuildingEnrollmentSpan", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool ScreeningComplete(int regID)
        {
            try
            {
                bool rtn = false;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_CheckIfOhioResidentByRegID", sqlParms, "RegDataCOR");
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (ObjectControllerHelper.HasRows(dsReg))
                {
                    bool isOHResident = ObjectControllerHelper.GetBool("IS_OHIO_RESIDENT", drReg);
                    if (isOHResident)
                    {
                        rtn = true;
                    }
                }

                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateInitialBCNoticeDate(int regId, DateTime initialNoticeDate, DateTime createdDateTime, string createdBy)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("DateOfInitialBCNotice", DbType.DateTime, initialNoticeDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateDateOfBCNotices", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateIssueFBILetterOnDate(int regId, DateTime IssueFBILetterOnDate, DateTime createdDateTime, string createdBy)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("IssueFBILetterOnDate", DbType.DateTime, IssueFBILetterOnDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateDateOfBCNotices", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateReminderBCNoticeDate(int regId, bool IssueBCReminderNotice, DateTime reminderNoticeDate, DateTime createdDateTime, string createdBy)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("IssueBCReminderNotice", DbType.Boolean, IssueBCReminderNotice, false));
                parameters.Add(SqlParms.CreateParameter("DateOfReminderBCNotice", DbType.DateTime, reminderNoticeDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateDateOfBCNotices", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateLTCRiskAlertCustom(int regId, int ProcessID)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateREG_LTC_RISK_ALERTCustom", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int SelectBuildingRegistration(int regID)
        {
            DataSet ds = null;
            int buildingRegID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectBuildingRegistration_ByRegID", parameters, "buildingRegID");
                ds.Tables[0].TableName = "buildingRegID";
                if (ObjectControllerHelper.HasRows(ds))
                {
                    buildingRegID = ObjectControllerHelper.GetInt("BUILDING_REG_ID", ds.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return buildingRegID;
        }

        public static void PopulateStagingData(int transactionID, string transactionType, string Result)
        {

            try
            {
                Logging log = new Logging(Guid.NewGuid(), "PopulateStagingData");
                PreProcessingDataFixes.PreProcessingDataFixesMain(log, transactionID);
            }
            catch (Exception ex)
            {
                Logging log = new Logging(Guid.NewGuid(), "PopulateStagingData");
                log.CreateLogEntry(string.Format("Error in PreProcessingDataFixesMain: {0} ", ex.Message + " " + ex.StackTrace));
            }

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep 11 2013  5:21PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("transaction_queue_id", DbType.Int32, transactionID, true));
                parameters.Add(SqlParms.CreateParameter("transactionType", DbType.String, transactionType, true));
                parameters.Add(SqlParms.CreateParameter("Result", DbType.String, Result, true));
                DataAccess.ExecuteStoredProcedure("usp_TransferPNMToStaging", parameters);

                //Write a post cleanup script that is configurable within try catch and no error throw
                //Used to cleanup duplicate specialties/affiliations etc..
                try
                {
                    PostProcessStageDataCleanup(transactionID);
                }
                catch (Exception ex)
                {
                    Logging log = new Logging(Guid.NewGuid(), "PopulateStagingData");
                    log.CreateLogEntry(string.Format("Error in PostProcessStageDataCleanup: {0} ", ex.Message + " " + ex.StackTrace));
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void PostProcessStageDataCleanup(int transactionID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("transaction_queue_id", DbType.Int32, transactionID, true));
                DataAccess.ExecuteStoredProcedure("usp_PostStageCleanupData", parameters);
            }
            catch (Exception Ex)
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Logging log = new Logging(Guid.NewGuid(), logMsg);
                log.CreateLogEntry(string.Format("Error in PostStageCleanupData: {0} ", Ex));
            }
        }

        public static DataSet SelectSvcLocIDTranType(int regID)
        {
            //string returnVal = string.Empty;
            DataSet ds = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectSvcLocIDTranType_ByRegID", parameters, "ServiceLocations");
                ds.Tables[0].TableName = "ServiceLocations";
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return ds;
        }

        public static void InsertRegAddressesForFacility(int facilityRegID, int opRegID, DateTime createdDateTime, string createdBy)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("OperatorREG_ID", DbType.Int32, opRegID, false));
                sqlParms.Add(SqlParms.CreateParameter("FacilityREG_ID", DbType.Int32, facilityRegID, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, createdDateTime, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("usp_InsertRegAddressesForFacility", sqlParms);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMedicaidId(int regID)
        {
            //string returnVal = string.Empty;
            DataSet ds = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));

                ds = DataAccess.ExecuteStoredProcedure("usp_SelectServiceLocationID_ByRegID", parameters, "ServiceLocations");
                ds.Tables[0].TableName = "ServiceLocations";
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return ds;
        }

        public static int InsertNewProviderRegistration(Guid userID, int applicationTypeID, string providerName, string dba, string firstName, string middleName, string lastName, DateTime? birthDate,
            string gender, int taxIDTypeID, string taxID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int registrationStatusTypeId, int referralID, int specialtyTypeID, int taxonomyTypeID, string practiceName, string zipCode, string zipExt, bool isPaperApplication,
             int paperRequestQueueID, DateTime createdDate, Guid createdBy, int workflowID, DateTime? RegCreateDateTime, int workflowEventTypeID, int waiverTypeID, bool retroEffectiveDate,
             int practiceTypeCodeID, int ownershipTypeCodeID)
        {
            int regId = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderName", DbType.String, providerName, false));
                parameters.Add(SqlParms.CreateParameter("DBA", DbType.String, dba, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("MiddleInitial", DbType.String, middleName, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("Gender", DbType.String, gender, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("TaxIDTypeID", DbType.Int32, taxIDTypeID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderCategoryTypeID", DbType.Int32, providerCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("TypeofPracticeID", DbType.Int32, practiceTypeID, true));
                parameters.Add(SqlParms.CreateParameter("RegistrationStatusTypeID", DbType.Int32, registrationStatusTypeId, true));
                if (birthDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("BirthDate", DbType.Date, birthDate, true));
                if (requestedEffectiveDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("RequestedEffectiveDate", DbType.Date, requestedEffectiveDate, true));

                /***************************************************************************************************
                 * SpecialtyID is not required, so use 0 for null specialty 
                 ***************************************************************************************************/
                //if (specialtyTypeID > 0)
                //    parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, specialtyTypeID, true));
                parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, specialtyTypeID, true));

                if (taxonomyTypeID > 0)
                    parameters.Add(SqlParms.CreateParameter("TaxonomyTypeID", DbType.Int32, taxonomyTypeID, true));

                if (referralID > 0)
                    parameters.Add(SqlParms.CreateParameter("ReferralID", DbType.Int32, referralID, true));

                if (paperRequestQueueID > 0)
                    parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, paperRequestQueueID, true));

                parameters.Add(SqlParms.CreateParameter("PracticeName", DbType.String, practiceName, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipCode", DbType.String, zipCode, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipExt", DbType.String, zipExt, true));
                parameters.Add(SqlParms.CreateParameter("Is_Paper_Application", DbType.Boolean, isPaperApplication == true ? 1 : 0, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, createdDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, createdBy, true));
                parameters.Add(SqlParms.CreateParameter("WorkflowID", DbType.Int32, workflowID, true));
                parameters.Add(SqlParms.CreateParameter("Reg_Create_Date_Time", DbType.DateTime, RegCreateDateTime, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("WorkflowEventTypeID", DbType.Int32, workflowEventTypeID, true));
                parameters.Add(SqlParms.CreateParameter("WaiverTypeID", DbType.Int32, waiverTypeID, true));
                parameters.Add(SqlParms.CreateParameter("RetroEffectiveDate", DbType.Boolean, retroEffectiveDate, true));

                parameters.Add(SqlParms.CreateParameter("PRACTICE_TYPE_ID", DbType.Int32, practiceTypeCodeID, true));
                parameters.Add(SqlParms.CreateParameter("OWNERSHIP_TYPE_ID", DbType.Int32, ownershipTypeCodeID, true));


                regId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertNewProviderRegistration", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return regId;
        }

        public static DataSet SelectBuildingMedicaidIDByRegID(int regID)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectBuildingMedicaidID_ByRegID", sqlParms, "regBuildingMedID");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int GetCHOPSellerRegID(int regIDParent)
        {
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regIDParent, false));
            DataSet dsCHOPParent = DataAccess.ExecuteStoredProcedure("usp_SelectREG_CHOP_PARENT", sqlParms, "regCHOPParent");
            int reg_ID_SELL = 0;
            if (ObjectControllerHelper.HasRows(dsCHOPParent))
            {
                DataRow drCHOPParent = dsCHOPParent.Tables[0].Rows[0];
                reg_ID_SELL = ObjectControllerHelper.GetInt("REG_ID_SELL", drCHOPParent);
            }

            return reg_ID_SELL;
        }

        public static void UpdateBuildingRegistrationToActive(int facilityRegID, int buildingRegID, int groupAffiliationStatus, DateTime lastmodifiedDate, Guid lastmodifiedUser)
        {
            try
            {
                List<SqlParameter> affParams = new List<SqlParameter>();
                affParams.Add(SqlParms.CreateParameter("FacilityREG_ID", DbType.Int32, facilityRegID, false));
                affParams.Add(SqlParms.CreateParameter("BuildingREG_ID", DbType.String, buildingRegID, false));
                affParams.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastmodifiedDate, false));
                affParams.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastmodifiedUser, false));
                affParams.Add(SqlParms.CreateParameter("GROUP_AFFILIATION_STATUS_ID", DbType.Int32, groupAffiliationStatus, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateGroupAffiliationStatusBldgByRegID", affParams);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateRegistrationAffiliationStatus(int regID, int groupAffiliationStatus, DateTime lastmodifiedDate, Guid lastmodifiedUser)
        {
            try
            {
                List<SqlParameter> affParams = new List<SqlParameter>();
                affParams.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                affParams.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastmodifiedDate, false));
                affParams.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastmodifiedUser, false));
                affParams.Add(SqlParms.CreateParameter("GROUP_AFFILIATION_STATUS_ID", DbType.Int32, groupAffiliationStatus, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateRegistrationAffiliationStatusByRegID", affParams);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdatePNMApplicationStatus(int regID, int applicationStatus, DateTime lastmodifiedDate, Guid lastmodifiedUser, int processID)
        {
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                sqlParams.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastmodifiedDate, false));
                sqlParams.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastmodifiedUser, false));
                sqlParams.Add(SqlParms.CreateParameter("PNM_APPLICATION_STATUS", DbType.Int32, applicationStatus, false));
                sqlParams.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, processID, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdatePNM_APPLICATION_STATUSByRegID", sqlParams);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertRegApplicationRecord(int regId, DateTime regCreadtedDate, DateTime lastModifiedDate, Guid lastModifiedUser, int processId)
        {
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));
                sqlParams.Add(SqlParms.CreateParameter("Reg_Create_Date_Time", DbType.DateTime, regCreadtedDate, false));
                sqlParams.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, lastModifiedDate, false));
                sqlParams.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, lastModifiedUser, false));
                sqlParams.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processId, false));
                DataAccess.ExecuteStoredProcedure("usp_insertREG_APPLICATIONCustom", sqlParams);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void PopulateStagingData(int transactionID, string transactionType, string Result, string StageDocument)
        {
            try
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Logging log = new Logging(Guid.NewGuid(), logMsg);
                PreProcessingDataFixes.PreProcessingDataFixesMain(log, transactionID);
            }
            catch (Exception ex)
            {
                //nothing to do here since logs are already being captured inside the method
            }

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("transaction_queue_id", DbType.Int32, transactionID, true));
                parameters.Add(SqlParms.CreateParameter("transactionType", DbType.String, transactionType, true));
                parameters.Add(SqlParms.CreateParameter("StageDocument", DbType.String, StageDocument, true));
                parameters.Add(SqlParms.CreateParameter("Result", DbType.String, Result, true));
                DataAccess.ExecuteStoredProcedure("usp_TransferPNMToStaging", parameters);

                //Write a post cleanup script that is configurable within try catch and no error throw
                //Used to cleanup duplicate specialties/affiliations etc..
                try
                {
                    PostProcessStageDataCleanup(transactionID);
                }
                catch
                {
                    //do nothing here as this is failure of cleanup script
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int GetRegSectionUploadControlIDByRegID(int regID, string regPageSection, string title)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, regPageSection, false));
                parameters.Add(SqlParms.CreateParameter("TITLE", DbType.String, title, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetRegSectionUploadControlIDByRegID", parameters, "RegSectionUploadControlID");
                ds.Tables[0].TableName = "RegSectionUploadControlID";
                if (ObjectControllerHelper.HasRows(ds))
                {
                    return Convert.ToInt32(ds.Tables["RegSectionUploadControlID"].Rows[0]["REG_SECTION_UPLOAD_CONTROL_ID"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckRRTermFieldsByRegID(int regID)
        {
            try
            {
                bool rtn = false;
                string termDate = string.Empty;
                string termReason = string.Empty;
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckRRTermFieldsByRegID", sqlParms, "RegDataHRF");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    termDate = ObjectControllerHelper.GetString("EFFECTIVE_DATE", ds.Tables[0].Rows[0]);
                    termReason = ObjectControllerHelper.GetString("TERMINATION_REASON", ds.Tables[0].Rows[0]);
                }
                if (!string.IsNullOrEmpty(termDate) && !string.IsNullOrWhiteSpace(termDate) && !termReason.Equals("0"))
                {
                    rtn = true;
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectHospiceDocumentByMail(string hospiceTrackingNo)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("HOSPICE_TRACKING_NUMBER", DbType.String, hospiceTrackingNo, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectHOSPICEDOCUMENTMAIL", parameters, "HospiceDocumentMail" + hospiceTrackingNo);

                lookup.Tables[0].TableName = "HospiceDocumentMail";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int InsertHospiceDocumentByMail(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar("Insert_Hospic_Document_Mail", parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Insert_Hospic_Document_Mail - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectSiteVisitDataByRegID(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectSiteVisitDataByRegID", parameters, "RegistrationData_" + Guid.NewGuid().ToString());
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateGroupAffiliationStatusByNPI(int regID, int groupaffiliationstatusid, DateTime modifiedDate, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("group_affiliation_status", DbType.Int32, groupaffiliationstatusid, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, true));


                DataAccess.ExecuteStoredProcedure("updateREG_AFFILIATION_BYNPI", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateBCIIResultByID(int bcID, int regID, int resultID, DateTime modifiedDate, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_BACKGROUND_CHECK_ID", DbType.Int32, bcID, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("BACKGROUND_RESULT_TYPE_ID", DbType.Int32, resultID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, true));

                DataAccess.ExecuteStoredProcedure("updateREG_BACKGROUND_CHECKcustom", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void CreateContractForSpecialty386(int regId, DateTime startDate, Guid changedBy)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("START_DATE", DbType.DateTime, startDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                //OHPNM-4208 Comment out changes for now per Mamta 	
                //DataAccess.ExecuteStoredProcedure("usp_CreateContractForSpecialty386", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegApplicationByRegID(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_APPLICATIONCustom", parameters, "RegistrationData_" + Guid.NewGuid().ToString());
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectApplicationStatusFromAudit(int regId, string appStatusID, int processID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationStatus", DbType.String, appStatusID, true));
                parameters.Add(SqlParms.CreateParameter("Process_ID", DbType.Int32, processID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectApplicationStatusFromAudit", parameters, "RegistrationData_" + Guid.NewGuid().ToString());
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }



        public static DataSet SelectBuildingRegistrationByOperatorRegID(int regID)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectBuildingRegistration_ByOperatorRegID", sqlParms, "regBuildingMedID");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }




        public static DataSet SelectCredentialingProviderType(int regId)

        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_Id", DbType.Int32, regId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectCREDENTIALING_PROVIDER_TYPE", parameters, "RegistrationData_" + Guid.NewGuid().ToString());
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectCLIACertificateDatesByCLIANumber(int regId, string cliaNumber)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_Id", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("CLIA_Number", DbType.String, cliaNumber, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectCLIA_Certificate_DatesByCLIANumber", parameters, "RegistrationDataClia");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectOfficeInformationData(int regId, int regAddressId, string tableName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("REG_ADDRESS_ID", DbType.Int32, regAddressId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_OFFICE_TIMING_BY_REG_ADDRESS_ID", parameters, "OfficeInformationData");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateCMCRegistrationData(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                DataAccess.ExecuteStoredProcedure("usp_UpdateREG_CMC_ENROLLMENTByRegID", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckCMCProgramExistsByRegID(int regID)
        {
            try
            {
                bool rtn = false;

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_CMC_ENROLLMENTByRegID", sqlParms, "RegData");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (ObjectControllerHelper.HasRows(ds))
                    {
                        rtn = true;
                    }
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool CheckIfDODDInitialApplication(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regId, true));
                return Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfDODDInitialApplication", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool CheckIfODAInitialApplication(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regId, true));
                return Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfODAInitialApplication", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdatetoMedicaidProviderRegistration(int regID, Guid userID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int taxonomyTypeID, string zipCode, string zipExt, DateTime createdDate, Guid createdBy, int workflowID, DateTime npiStartDate, DateTime? npiEndDate, DateTime? endDate,
            int applicationTypeID, int waiverTypeID, int workflowEventTypeID, int waiverServiceUpdateTypeID, string gender)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, false));
                parameters.Add(SqlParms.CreateParameter("ProviderCategoryTypeID", DbType.Int32, providerCategoryTypeID, false));
                parameters.Add(SqlParms.CreateParameter("TypeofPracticeID", DbType.Int32, practiceTypeID, true));

                if (requestedEffectiveDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("RequestedEffectiveDate", DbType.Date, requestedEffectiveDate, true));

                if (taxonomyTypeID > 0)
                    parameters.Add(SqlParms.CreateParameter("TaxonomyTypeID", DbType.Int32, taxonomyTypeID, true));

                parameters.Add(SqlParms.CreateParameter("LocationZipCode", DbType.String, zipCode, true));
                parameters.Add(SqlParms.CreateParameter("LocationZipExt", DbType.String, zipExt, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, createdDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, createdBy, true));
                parameters.Add(SqlParms.CreateParameter("WorkflowID", DbType.Int32, workflowID, true));
                parameters.Add(SqlParms.CreateParameter("NPIStartDate", DbType.DateTime, npiStartDate, true));
                parameters.Add(SqlParms.CreateParameter("NPIEndDate", DbType.DateTime, npiEndDate, true));
                parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, endDate, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("WaiverTypeID", DbType.Int32, waiverTypeID, true));
                parameters.Add(SqlParms.CreateParameter("WorkflowEventTypeID", DbType.Int32, workflowEventTypeID, true));
                parameters.Add(SqlParms.CreateParameter("WaiverServiceUpdateTypeID", DbType.Int32, waiverServiceUpdateTypeID, true));
                parameters.Add(SqlParms.CreateParameter("Gender", DbType.String, gender, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdatetoMedicaidProviderRegistration", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int SelectClaimAttachmentTypeId(string typename, string claim)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("DocumentType", DbType.String, typename, true),
                    SqlParms.CreateParameter("Claim", DbType.String, claim, true)
                };

                object scalarResult = DataAccess.ExecuteScalar("usp_SelectClaimAttachmentTypeId", parameters);

                if (scalarResult == null || string.IsNullOrWhiteSpace(scalarResult.ToString()))
                {
                    return 0;
                }
                if (int.TryParse(scalarResult.ToString(), out int result))
                {
                    return result;
                }
                throw new FormatException("Returned value could not be parsed to an integer.");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteAttachmentsForMedicaidID(string Medicaid_ID)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Medicaid_ID", DbType.String, Medicaid_ID, false));

                DataAccess.ExecuteStoredProcedure("usp_DeleteAttachmentsForMedicaidID", parameters);

                return;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateRegMedicareEnrollmentStatus(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value)) parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    else parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                DataAccess.ExecuteStoredProcedure("usp_REG_MEDICARE_ENROLLMENT_STATUS_UPDATE", parameters);
                return;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static string GetNPIOrMedicaidIdByRegId(int regID)
        {
            string NPIOrMedicaidId = string.Empty;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                NPIOrMedicaidId = DataAccess.ExecuteScalar("usp_Select_NPI_Or_MedicaidId_By_RegId", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

            return NPIOrMedicaidId;
        }

        public static DataSet GetBehaviourHealthAffRequired(int regID)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetBehvAffPageRequired", parms, "behvpageRequired");
                lookup.Tables[0].TableName = "behvpageRequired";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool CheckRestrictedServicesExistsByRegID(int regID)
        {
            try
            {
                bool rtn = false;

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectRestricted_Services", sqlParms, "RegData");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (ObjectControllerHelper.HasRows(ds))
                    {
                        rtn = true;
                    }
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveRegistrationSectionStatusRTP(int regId, int regPageTypeId, int regSectionTypeId, int? regProviderStatusTypeId, int? regProviderServicesStatusTypeId,
            DateTime changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, regPageTypeId, false));
                parameters.Add(SqlParms.CreateParameter("REG_SECTION_TYPE_ID", DbType.Int32, regSectionTypeId, false));
                if (regProviderStatusTypeId != null)
                    parameters.Add(SqlParms.CreateParameter("REG_PROVIDER_STATUS_TYPE_ID", DbType.Int32, regProviderStatusTypeId, false));
                if (regProviderServicesStatusTypeId != null)
                    parameters.Add(SqlParms.CreateParameter("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", DbType.Int32, regProviderServicesStatusTypeId, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_SaveRegistrationSectionStatusRTP", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("REG_ID: " + regId.ToString() +
                    ", Reg Page Type Id: " + regPageTypeId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void InsertAddressHospitalNoByRegID(int regID, int regAddressID, string newHOSPITAL_NO, DateTime createdDateTime, string createdBy)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                sqlParms.Add(SqlParms.CreateParameter("REG_ADDRESS_ID", DbType.Int32, regAddressID, false));
                sqlParms.Add(SqlParms.CreateParameter("HOSPITAL_NO", DbType.String, newHOSPITAL_NO, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, createdDateTime, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("insertREG_ADDRESS_HospitalNo", sqlParms);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void InsertAddressHealthCenterNoByRegID(int regID, int regAddressID, string newHealth_Center_No, DateTime createdDateTime, string createdBy)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                sqlParms.Add(SqlParms.CreateParameter("REG_ADDRESS_ID", DbType.Int32, regAddressID, false));
                sqlParms.Add(SqlParms.CreateParameter("Health_Center_No", DbType.String, newHealth_Center_No, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, createdDateTime, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("insertREG_ADDRESS_HealthCenterNo", sqlParms);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void InsertAddressNursingFacilityNoByRegID(int regID, int regAddressID, string newNursing_Facility_No, DateTime createdDateTime, string createdBy)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                sqlParms.Add(SqlParms.CreateParameter("REG_ADDRESS_ID", DbType.Int32, regAddressID, false));
                sqlParms.Add(SqlParms.CreateParameter("Nursing_Facility_No", DbType.String, newNursing_Facility_No, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, createdDateTime, false));
                sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));
                DataAccess.ExecuteStoredProcedure("insertREG_ADDRESS_NursingFacilityNo", sqlParms);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetMaxNursingFacilityNoRegAddress()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetMaxNursingFacilityNoFromRegAddressHospitalNo", "checkMedicaidID");
                lookup.Tables[0].TableName = "checkMedicaidID";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetMaxHospitalNoRegAddress()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetMaxHospitalNoFromRegAddressHospitalNo", "checkMedicaidID");
                lookup.Tables[0].TableName = "checkMedicaidID";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetMaxHealthCenterNoRegAddress()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetMaxHealthCenterNoFromRegAddressHospitalNo", "checkMedicaidID");
                lookup.Tables[0].TableName = "checkMedicaidID";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Gets the data if any previous registrations have same NPI  
        public static DataSet GetExistingRegDataForNPI(string NPI, int regId, string mmisProviderTypeID, bool isKeyFieldEditRequest)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, false));
                sqlParms.Add(SqlParms.CreateParameter("REGID", DbType.Int32, regId, true));
                sqlParms.Add(SqlParms.CreateParameter("MMIS_PROVIDER_TYPE_ID", DbType.String, mmisProviderTypeID, false));
                sqlParms.Add(SqlParms.CreateParameter("isKeyFieldEditRequest", DbType.Int32, isKeyFieldEditRequest, true));

                DataSet dsNPIAlreadyPresent = DataAccess.ExecuteStoredProcedure("usp_SelectNPIWithProviderType", sqlParms, "NPIProviderTypeIDCheck");
                return dsNPIAlreadyPresent;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void CheckEligibletoConvertFromORP(int regId, out bool isEligible)
        {
            string outValue;
            isEligible = false;
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataAccess.ExecuteStoredProcedure("usp_CheckEligibletoConvertFromORP", sqlParms, "IsEligible", SqlDbType.Bit, out outValue, 0);

                if (!string.IsNullOrEmpty(outValue))
                {
                    isEligible = Convert.ToBoolean(outValue);
                }

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetCHOPHistory(int regId, int pageSize, int pageNumber)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("pageSize", DbType.Int32, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("pageNumber", DbType.Int32, pageNumber, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetCHOPHistory", parameters, "GAPage_" + regId);
                ds.Tables[0].TableName = "GCPage";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetClosureHistory(int regId, int pageSize, int pageNumber)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("pageSize", DbType.Int32, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("pageNumber", DbType.Int32, pageNumber, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetClosureHistory", parameters, "GAPage_" + regId);
                ds.Tables[0].TableName = "GCPage";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectProviderTypeChangeRequestByOldReg(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_selectProviderTypeChangeRequestByMainReg", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int ValidateNPIandMEDidData(int regID, int ProcessID, string currentAction)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, false));
                parameters.Add(SqlParms.CreateParameter("CURRENT_ACTION", DbType.String, currentAction, false));
                int result = Convert.ToInt32(DataAccess.ExecuteStoredProcedure("usp_ValidateREG_NPI_ENROLLMENT_SPAN", parameters, "RESULT", SqlDbType.Int));

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectProviderTypeChangeRequestByNewReg(int regID, int ProcessID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_selectProviderTypeChangeRequestByNewReg", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveProviderTypeChangeRequest(int oldRegId, int newRegId, int processId, int status,
        DateTime createDate, Guid createdBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OLD_REG_ID", DbType.Int32, oldRegId, false));
                parameters.Add(SqlParms.CreateParameter("NEW_REG_ID", DbType.Int32, newRegId, false));
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, processId, false));
                parameters.Add(SqlParms.CreateParameter("STATUS", DbType.Int32, status, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, createDate, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, createdBy, true));
                DataAccess.ExecuteStoredProcedure("usp_insertProviderTypeChangeRequest", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Old REG_ID: " + oldRegId.ToString() +
                    ", New REG_ID: " + newRegId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void UpdateProviderTypeChangeStatus(int newRegId, int status, DateTime UpdDate, string UpdBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NEW_REG_ID", DbType.Int32, newRegId, false));
                parameters.Add(SqlParms.CreateParameter("STATUS", DbType.Int32, status, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, UpdDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, UpdBy, true));
                DataAccess.ExecuteStoredProcedure("usp_updateProviderTypeChangeRequestStatus", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(", New REG_ID: " + newRegId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void InsertRegNPIEnrollment(int regId, int processId, string providerEffectiveDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, processId, false));
                if (providerEffectiveDate != null)
                {
                    parameters.Add(SqlParms.CreateParameter("PROVIDER_EFFECTIVE_DATE", DbType.String, providerEffectiveDate, false));
                }

                DataAccess.ExecuteStoredProcedure("usp_insertREG_NPI_MEDID_ENROLLMENT_SPANcustom", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(", REG_ID: " + regId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet SelectCMCProviderDetails(string medicaidId, Guid userID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.Int32, medicaidId, false));
                parameters.Add(SqlParms.CreateParameter("USERID", DbType.Guid, userID, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectCMCProviderDetails", parameters, "RegData_" + medicaidId);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet CheckIfActiveCMCProvider(int reg_id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_id, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_CheckIfActiveCMCProvider", parameters, "CMCData_" + reg_id);

                lookup.Tables[0].TableName = "CMCData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetProviderSpecialtySearchResults(string npi, string medicaidID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidID, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetProviderSpecialtySearchResults", parameters, "RegData_" + npi);

                lookup.Tables[0].TableName = "RegData_" + npi;

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet CheckIfCPCPrimary(int reg_id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_id, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_CheckifPrimary_CPC_Provider", parameters, "RegData_" + reg_id);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertProviderFeedNotes(int regId, int regProviderFeedId, string initiatedBy, string note,
            string personReviewedBy = null, string enrollmentType = null, string finalDisposition = null, int processID = 0)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true),
                    SqlParms.CreateParameter("REG_PROVIDER_FEED_ID", DbType.Int32, regProviderFeedId, true),
                    SqlParms.CreateParameter("INITIATED_BY", DbType.String, initiatedBy, true),
                    SqlParms.CreateParameter("PERSON_REVIEWED_BY", DbType.String, personReviewedBy, true),
                    SqlParms.CreateParameter("ENROLLMENT_TYPE", DbType.String, enrollmentType, true),
                    SqlParms.CreateParameter("FINAL_DISPOSITION", DbType.String, finalDisposition, true),
                    SqlParms.CreateParameter("NOTES", DbType.String, note, true),
                    SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, processID, true)
                };
                DataAccess.ExecuteStoredProcedure("usp_InsertRegProviderFeedNotes", parameters);
                return regId;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(", REG ID: " + regId.ToString() + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet CheckProviderJobNotesExists(int regId, string enrollmentType, string finalDisposition, string note)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_TYPE", DbType.String, enrollmentType, true));
                parameters.Add(SqlParms.CreateParameter("FINAL_DISPOSITION", DbType.String, finalDisposition, true));
                parameters.Add(SqlParms.CreateParameter("NOTES", DbType.String, note, true));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_CheckProviderJobNotesExists", parameters, "RegProviderNotesData");

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateHidePreviousNotesFlag(string regId, string credentialingId, bool hidePreviousNotesFlag, string userId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("CREDENTIALING_ID", DbType.Int32, credentialingId, false));
                parameters.Add(SqlParms.CreateParameter("HIDE_PREVIOUS_NOTES_FLAG", DbType.Boolean, hidePreviousNotesFlag, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userId, false));

                DataAccess.ExecuteStoredProcedure("usp_UpdateHidePreviousNotesFlag", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet Reports_SelectProviderReCredentialingDueReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("StartDate", DbType.DateTime, startDate, true));
                parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, endDate, true));

                ds = DataAccess.ExecuteStoredProcedure(
                        "usp_Reports_ProviderReCredentialingDueReport",
                        parameters,
                        "ReCredentialingDue");

                ds.Tables[0].TableName = "ProviderReCredentialingDue";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet Reports_SelectProviderCredentialingApproved(
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("StartDate", DbType.DateTime, startDate, true),
                    SqlParms.CreateParameter("EndDate", DbType.DateTime, endDate, true)
                };

                ds = DataAccess.ExecuteStoredProcedure(
                    "usp_Reports_CredentialingCleanFile",
                    parameters,
                    "ProviderCredentialingApproved");

                ds.Tables[0].TableName = "ProviderCredentialingCleanFile";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet Reports_SelectProviderCredentialingCommitteeDecision(
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                DataSet ds = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("StartDate", DbType.DateTime, startDate, true),
                    SqlParms.CreateParameter("EndDate", DbType.DateTime, endDate, true)
                };

                ds = DataAccess.ExecuteStoredProcedure(
                        "usp_Reports_CredentialingFlaggedFiles",
                        parameters,
                        "ProviderCredentialingCommitteeDecision");

                ds.Tables[0].TableName = "ProviderCredentialingFlaggedFiles";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet Reports_SelectProviderReCredentialingOverdue(
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                DataSet ds = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("StartDate", DbType.DateTime, startDate, true),
                    SqlParms.CreateParameter("EndDate", DbType.DateTime, endDate, true)
                };

                ds = DataAccess.ExecuteStoredProcedure(
                    "usp_Reports_ProviderCredentialinggraterthan36MonthsReport",
                    parameters,
                    "ProviderReCredentialingOverdue");

                ds.Tables[0].TableName = "ProviderCredentialinggraterthan36MonthsReport";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet Reports_SelectCredentialingFileProcessingSummary(
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                DataSet ds = new DataSet();

                var parameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("StartDate", DbType.DateTime, startDate, true),
                    SqlParms.CreateParameter("EndDate", DbType.DateTime, endDate, true)
                };

                ds = DataAccess.ExecuteStoredProcedure(
                    "usp_Reports_CredentialngCleanandFlaggedfiles",
                    parameters,
                    "CredentialingFileProcessingSummary");

                ds.Tables[0].TableName = "CredentialngCleanandFlaggedfiles";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectActiveOwnerStepsProvider(string ownerID, bool includeOtherSteps, string ownerRole)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OwnerID", DbType.String, ownerID, true));
                parameters.Add(SqlParms.CreateParameter("IncludeOtherSteps", DbType.Boolean, includeOtherSteps, true));
                parameters.Add(SqlParms.CreateParameter("OwnerRole", DbType.String, ownerRole, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectActiveOwnerStepsProvider", parameters, "Assigned");

                ds.Tables[0].TableName = "Assigned";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet WF_SelectActiveOwnerStepsCredentialProvider(string ownerID, bool includeOtherSteps, string ownerRole)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OwnerID", DbType.String, ownerID, true));
                parameters.Add(SqlParms.CreateParameter("IncludeOtherSteps", DbType.Boolean, includeOtherSteps, true));
                parameters.Add(SqlParms.CreateParameter("OwnerRole", DbType.String, ownerRole, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectActiveOwnerStepsCredentialProvider", parameters, "Assigned");

                ds.Tables[0].TableName = "CredentialChairAssigned";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCredentialingDocuments(int regId, string section)
        {
            try
            {
                DataSet ds = new DataSet();
                if (regId > -1)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                    parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, section, true));
                    ds = DataAccess.ExecuteStoredProcedure("usp_SelectCredentialingDocuments", parameters, "RegistrationData_" + "REG_ADDRESSCustomPagingNew");
                    ds.Tables[0].TableName = "RegistrationData";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectGroupAffiliationByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectGroupAffiliationByRegID", parameters, "RegData_" + regID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

    }
}
