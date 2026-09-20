using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO; // Jira 2636
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Web;

namespace MAXIMUS.Controllers.PDMS
{
    public static class UserController
    {

        public static DataSet GetAllUserRoles()
        {
            DataSet retUserRoles = null;
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectAllUserRoles");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    retUserRoles = ds;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return retUserRoles;
        }

        public static DataSet GetUserRolesByUser(string userName)
        {
            DataSet retUserRoles = null;
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("user_name", DbType.String, userName, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetUserRolesByUserName", parameters, "UserRoles");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    retUserRoles = ds;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return retUserRoles;
        }

        public static DataSet GetLoweredUserNameByUserName(string userName, string userId)
        {
            DataSet retUserRoles = null;
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("user_name", DbType.String, userName, true));
                parameters.Add(SqlParms.CreateParameter("user_id", DbType.String, userId, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetLoweredUserNameByUserName", parameters, "UserRoles");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    retUserRoles = ds;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return retUserRoles;
        }

        public static DataSet GetUserRolesByOHID(string ohid)
        {
            DataSet retUserRoles = null;
            try
            {
                
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("@OHID", DbType.String, ohid, true));

                // Execute the correct stored procedure.
                DataSet ds = DataAccess.ExecuteStoredProcedure("dbo.GetUserRolesByOHID", parameters, "UserRoles");

                // Check if the DataSet has rows before assigning it.
                if (ObjectControllerHelper.HasRows(ds))
                {
                    retUserRoles = ds;
                }
            }
            catch (Exception ex)
            {
                
                throw CoreException.ThrowException(ex);
            }
            return retUserRoles;
        }

        public static DataSet SearchForExistingProvider(string taxId, string NPI)
        {
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxId, true));
            parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));


            DataSet ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure("usp_SearchForExistingProvider", parameters, "Providers");
            ds.Tables[0].TableName = "Providers";

            return ds;
        }

        public static DataSet SearchProviderForUserName(string email, string taxId, string NPI,
            string medicaidId, string zip)
        {
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("EMail", DbType.String, email, false));
            parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxId, true));
            parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
            parameters.Add(SqlParms.CreateParameter("BaseMedicaidID", DbType.String, medicaidId, true));
            parameters.Add(SqlParms.CreateParameter("Zip", DbType.String, zip, true));


            DataSet ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure("usp_SearchProviderForUserName", parameters, "ProviderUserID");
            ds.Tables[0].TableName = "ProviderUserID";

            return ds;
        }
        public static DataSet SelectRegCostReport(string UserId, int REG_COST_REPORT_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, UserId, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Guid, REG_COST_REPORT_ID, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_Select_LTCCostReport", parameters, "CostReport");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[0].TableName = "CostReport";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectLTCCostReportById(string UserId, int REG_COST_REPORT_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, UserId, true));
                parameters.Add(SqlParms.CreateParameter("REG_COST_REPORT_ID", DbType.Guid, REG_COST_REPORT_ID, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_Select_LTCCostReportById", parameters, "CostReport");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[0].TableName = "CostReport";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectMSPCostReportById(string UserId, int REG_COST_REPORT_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, UserId, true));
                parameters.Add(SqlParms.CreateParameter("REG_MSP_COST_REPORT_ID", DbType.Guid, REG_COST_REPORT_ID, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_Select_MSPCostReportById", parameters, "CostReport");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[0].TableName = "CostReport";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectRegMSPCostReport(string UserId, int REG_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, UserId, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Guid, REG_ID, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_Select_MSPCostReport", parameters, "MSPCostReport");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[0].TableName = "CostReport";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectUserSecurityQuestionsByNameEmail(string userName, string email)
        {
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, userName, false));
            parameters.Add(SqlParms.CreateParameter("EMail", DbType.String, email, false));

            DataSet ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectUserSecurityQuestionsbyNameEmail", parameters, "UserSecurityQuestions");
            ds.Tables[0].TableName = "UserSecurityQuestions";
            return ds;
        }

        public static Boolean CheckUserSecurityAnswer(string userName, int questionNumber, string answer)
        {
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, userName, false));
            parameters.Add(SqlParms.CreateParameter("QuestionNumber", DbType.Int32, questionNumber, false));
            parameters.Add(SqlParms.CreateParameter("Answer", DbType.String, answer, false));

            string s = DataAccess.ExecuteScalar("usp_CheckUserSecurityAnswer", parameters);

            if (s == "1")
                return true;
            else
                return false;
        }

        public static DataSet SelectUserSecurityQuestions(string userId)
        {
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, false));

            DataSet ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectUserSecurityQuestions", parameters, "UserSecurityQuestions");
            ds.Tables[0].TableName = "UserSecurityQuestions";
            return ds;
        }

        public static void InsertUserSecurityQuestions(string userId, int? question1Id, string answer1,
            int? question2Id, string answer2, int? question3Id, string answer3,
            DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("Question1ID", DbType.Int32, question1Id, true));
                parameters.Add(SqlParms.CreateParameter("Answer1", DbType.String, answer1, true));
                parameters.Add(SqlParms.CreateParameter("Question2ID", DbType.Int32, question2Id, true));
                parameters.Add(SqlParms.CreateParameter("Answer2", DbType.String, answer2, true));
                parameters.Add(SqlParms.CreateParameter("Question3ID", DbType.Int32, question3Id, true));
                parameters.Add(SqlParms.CreateParameter("Answer3", DbType.String, answer3, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_InsertUserSecurityQuestions", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateUserSecurityQuestions(string userId, int? question1Id, string answer1,
            int? question2Id, string answer2, int? question3Id, string answer3,
            DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("Question1ID", DbType.Int32, question1Id, true));
                parameters.Add(SqlParms.CreateParameter("Answer1", DbType.String, answer1, true));
                parameters.Add(SqlParms.CreateParameter("Question2ID", DbType.Int32, question2Id, true));
                parameters.Add(SqlParms.CreateParameter("Answer2", DbType.String, answer2, true));
                parameters.Add(SqlParms.CreateParameter("Question3ID", DbType.Int32, question3Id, true));
                parameters.Add(SqlParms.CreateParameter("Answer3", DbType.String, answer3, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateUserSecurityQuestions", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateDateSigned(string regID, DateTime Datesigned, Guid User)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regID, true));
                parameters.Add(SqlParms.CreateParameter("DateSigned", DbType.DateTime, Datesigned, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, User, true));
                DataAccess.ExecuteStoredProcedure("Usp_UpdateDateSigned", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectUserAccountInformation(string userId)
        {
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, false));

            DataSet ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectUserAccountInformation", parameters, "UserAccountInformation");
            ds.Tables[0].TableName = "UserAccountInformation";
            return ds;
        }

        public static void InsertUserAccountInformation(string userId, string contactName, string title, string phone, string phoneExt,
            string medId, int provCat, int provType, string taxId, int taxIDTypeID, string npi, string groupName, int zip, int diddReferralId, int? taxonomyTypeId,
            int? specialtyTypeId, DateTime? changedDate, string changedBy, bool is_OHID, string OHID, string UserType, string IOPUserName, int? regId = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("Name", DbType.String, contactName, true));
                parameters.Add(SqlParms.CreateParameter("Title", DbType.String, title, true));
                parameters.Add(SqlParms.CreateParameter("Phone", DbType.String, phone, true));
                parameters.Add(SqlParms.CreateParameter("PhoneExt", DbType.String, phoneExt, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medId, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, provCat, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, provType, true));
                parameters.Add(SqlParms.CreateParameter("TAX_ID", DbType.String, taxId, true));
                parameters.Add(SqlParms.CreateParameter("TAX_ID_TYPE_ID", DbType.String, taxIDTypeID, true));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.Double, npi, true));
                parameters.Add(SqlParms.CreateParameter("GROUP_NAME", DbType.String, groupName, true));
                parameters.Add(SqlParms.CreateParameter("SERVICE_LOCATION_ZIP", DbType.Int32, zip, true));
                if (diddReferralId > 0) parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralId, true));
                if (taxonomyTypeId.HasValue && taxonomyTypeId.Value > 0) parameters.Add(SqlParms.CreateParameter("TAXONOMY_TYPE_ID", DbType.Int32, taxonomyTypeId, true));
                if (specialtyTypeId.HasValue && specialtyTypeId.Value > 0) parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE_ID", DbType.Int32, specialtyTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, changedBy, true));
                parameters.Add(SqlParms.CreateParameter("Is_OHID", DbType.Boolean, is_OHID, true));
                parameters.Add(SqlParms.CreateParameter("OHID", DbType.String, OHID, true));
                parameters.Add(SqlParms.CreateParameter("USER_TYPE", DbType.String, UserType, true));
                parameters.Add(SqlParms.CreateParameter("IOP_USERNAME", DbType.String, IOPUserName, true));
                if (regId.HasValue) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));

                DataAccess.ExecuteStoredProcedure("usp_InsertUserAccountInformation", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteUserApplicationType(string userId, int applicationTypeId)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  2:30PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeId, true));

                DataAccess.ExecuteStoredProcedure("usp_DeleteUSER_APPLICATION_XREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteUserProviderType(string userId, int providerTypeId)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  2:30PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeId, true));

                DataAccess.ExecuteStoredProcedure("usp_DeleteUSER_PROVIDER_TYPE_XREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetUserRoleCompatibility()
        {
            DataSet retCompatibility = null;
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectUserRoleCompatibility");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    retCompatibility = ds;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return retCompatibility;
        }

        public static void DeleteUserStatusType(string userId, int statusTypeId)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  2:30PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("STATUS_TYPE_ID", DbType.Int32, statusTypeId, true));

                DataAccess.ExecuteStoredProcedure("usp_DeleteUSER_STATUS_TYPE_XREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertUserApplicationType(string userId, int applicationTypeId, DateTime? changedDate, string changedBy)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  2:30PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationId", DbType.Int32, applicationTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));

                DataAccess.ExecuteStoredProcedure("insertUSER_APPLICATION_XREFCustom", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertUserProviderType(string userId, int providerTypeId, DateTime? changedDate, string changedBy)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  2:32PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, providerTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));

                DataAccess.ExecuteStoredProcedure("insertUSER_PROVIDER_TYPE_XREFCustom", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertUserStatusType(string userId, int statusTypeId, DateTime? changedDate, string changedBy)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  2:33PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("STATUS_TYPE_ID", DbType.Int32, statusTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));

                DataAccess.ExecuteStoredProcedure("insertUSER_STATUS_TYPE_XREFCustom", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateUserAccountInformation(string userId, string contactName, string title,
            string phone, string phoneExt, DateTime? changedDate, string changedBy, bool FORCE_PASSWORD_RESET, string userType, bool isTempPwdSent = false)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("Name", DbType.String, contactName, true));
                parameters.Add(SqlParms.CreateParameter("Title", DbType.String, title, true));
                parameters.Add(SqlParms.CreateParameter("Phone", DbType.String, phone, true));
                parameters.Add(SqlParms.CreateParameter("PhoneExt", DbType.String, phoneExt, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, changedBy, true));
                parameters.Add(SqlParms.CreateParameter("FORCE_PASSWORD_RESET", DbType.Boolean, FORCE_PASSWORD_RESET, true));
                parameters.Add(SqlParms.CreateParameter("UserType", DbType.String, userType, true));
                parameters.Add(SqlParms.CreateParameter("IS_TEMP_PWD_SENT", DbType.String, isTempPwdSent, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateUserAccountInformation", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveUserIOPToken(Guid userId, string tokenResp, Guid changedBy, string userClaimResp, bool updateLastLoginDt, string IOPaccessTkn, string IOPrefreshTkn, string IOPidTkn)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, true));
                parameters.Add(SqlParms.CreateParameter("Token_Response", DbType.String, tokenResp, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                parameters.Add(SqlParms.CreateParameter("User_Claim_Response", DbType.Guid, userClaimResp, true));
                parameters.Add(SqlParms.CreateParameter("UpdateLastLoginDt", DbType.Boolean, updateLastLoginDt, true));
                parameters.Add(SqlParms.CreateParameter("IOP_ACCESS_TOKEN", DbType.String, IOPaccessTkn, true));
                parameters.Add(SqlParms.CreateParameter("IOP_REFRESH_TOKEN", DbType.String, IOPrefreshTkn, true));
                parameters.Add(SqlParms.CreateParameter("IOP_ID_TOKEN", DbType.String, IOPidTkn, true));
                DataAccess.ExecuteStoredProcedure("usp_SaveUserIOPToken", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveUserPNMToken(Guid userId, string tokenResp)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId.ToString(), true));
                parameters.Add(SqlParms.CreateParameter("PNMToken", DbType.String, tokenResp, true));
                DataAccess.ExecuteStoredProcedure("usp_SaveUserPNMToken", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static void NotifyPasswordReset(string templateActualPath, string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid(), string emailFrom = null)
        {
            Notification n = new Notification();
            Dictionary<string, object> fields = new Dictionary<string, object>();
            //commented for bug 4440
            //fields.Add("CURRENTDATE", DateTime.Now.ToShortDateString());
            fields.Add("USERNAME", username);
            fields.Add("PDMSURL", pdmsUrl);
            string subject = "Password Reset";
            string body = string.Empty;
            EMailNotification notify = new EMailNotification(body, subject, recipients);

            body = notify.SendActualNotification(templateActualPath + "//PasswordReset.txt", fields, true);

            #region COMMUNICATION EVENT
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
            string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
            parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, recipients, false));
            parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
            parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
            parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, "PasswordReset.txt", false));
            parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, ObjectControllerHelper.GetKeyValueString(fields), false));
            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
            parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, userid, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, true));
            parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean, notify.isEmailSent, true));
            parameters.Add(SqlParms.CreateParameter("log_message", DbType.String, notify.log_message, true));
            string comEventID = DataAccess.ExecuteScalar("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);
            #endregion
        }



        public static void NotifyPasswordResetEmail(string templateActualPath, string recipients, string username, string pdmsUrl, int regId = 0, int partyId = 0, Guid userid = new Guid(), string emailFrom = null)
        {
            Notification n = new Notification();
            Dictionary<string, object> fields = new Dictionary<string, object>();
            //commented for bug 4440
            //fields.Add("CURRENTDATE", DateTime.Now.ToShortDateString());
            fields.Add("USERNAME", username);
            fields.Add("PDMSURL", pdmsUrl);
            string subject = "Password Reset Email";
            string body = string.Empty;
            EMailNotification notify = new EMailNotification(body, subject, recipients);

            body = notify.SendActualNotification(templateActualPath + "PasswordResetEmail.txt", fields, true);

            #region COMMUNICATION EVENT
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
            string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
            parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, recipients, false));
            parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
            parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
            parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, "PasswordResetEmail.txt", false));
            parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, ObjectControllerHelper.GetKeyValueString(fields), false));
            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
            parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, userid, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, true));
            parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean, notify.isEmailSent, true));
            parameters.Add(SqlParms.CreateParameter("log_message", DbType.String, notify.log_message, true));
            string comEventID = DataAccess.ExecuteScalar("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);
            #endregion
        }

        public static void NotifyProviderAccountCreation(string templateActualPath, string recipients, string LEGAL_BUSINESS_NAME, string NPI, string USER_NAME, string LOGIN_URL, int regId = 0, Guid userid = new Guid(), string emailFrom = null)
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("LEGAL_BUSINESS_NAME", LEGAL_BUSINESS_NAME);
            fields.Add("NPI", NPI);
            fields.Add("USER_NAME", USER_NAME);
            fields.Add("LOGIN_URL", LOGIN_URL);
            //Bug NE branding
            string subject = DataAccess.GetAppSetting("BrandName") + " Provider Account Created";

            string body = string.Empty;
            EMailNotification notify = new EMailNotification(body, subject, recipients);

            body = notify.SendActualNotification(templateActualPath + "//ProviderAccountCreationConfirmation.txt", fields, true);

            #region COMMUNICATION EVENT
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
            string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
            parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, recipients, false));
            parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
            parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
            parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, "ProviderAccountCreationConfirmation.txt", false));
            parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, ObjectControllerHelper.GetKeyValueString(fields), false));
            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
            parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, userid, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, true));
            parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean, notify.isEmailSent, true));
            parameters.Add(SqlParms.CreateParameter("log_message", DbType.String, notify.log_message, true));
            DataAccess.ExecuteStoredProcedure("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);
            #endregion
        }

        public static void NotifyProviderOfDIDDReferral(string templateActualPath, string recipients, string providerFirstName,
            string providerLastName, string appNumber, DateTime? contractFromDate, DateTime? contractToDate, string emailFrom = null, string inputSubject = null)
        {

            Notification n = new Notification();
            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("FIRST_NAME", providerFirstName);
            fields.Add("LAST_NAME", providerLastName);
            fields.Add("APPLICATION_NUMBER", appNumber);
            fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
            fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
            fields.Add("PROVIDERNAME", providerFirstName + " " + providerLastName);
            if (contractFromDate.HasValue)
            {
                fields.Add("CONTRACT_FROM_DATE", contractFromDate.Value.ToShortDateString());
            }
            if (contractToDate.HasValue)
            {
                fields.Add("CONTRACT_TO_DATE", contractToDate.Value.ToShortDateString());
            }
            string subject = "District of Columbia Medicaid Provider Screening and Enrollment Services Provider - Referral Created";//commented for bug 4440 "Referral Created"
            if (!string.IsNullOrEmpty(inputSubject)) subject = inputSubject;

            EMailNotification notify = new EMailNotification(string.Empty, subject, recipients);
            notify.SendNotification(templateActualPath + "//DIDDReferralEmail.txt", fields, true);
            notify.CreateCommunicationEvent(fields, new Guid(), "0", "DIDDReferralEmail.txt");



        }
        public static int GetAdminPartyId()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string rtn = DataAccess.ExecuteScalar("sp_SelectAdminPartyID", parameters);
            if (string.IsNullOrEmpty(rtn)) return 0;
            return Convert.ToInt32(rtn);
        }
        public static bool NotifyPINnumber(string templateActualPath, string email, string pinNumber, string username, string userId)
        {
            Notification n = new Notification();
            Dictionary<string, object> fields = new Dictionary<string, object>();
            string name = string.Empty;
            if (!string.IsNullOrEmpty(userId))
            {
                DataSet dsUser = SelectUserAccountInformation(userId);
                if (ObjectControllerHelper.HasRows(dsUser))
                {
                    name = ObjectControllerHelper.GetString("Name", dsUser.Tables[0].Rows[0]);
                }
            }

            //commented for bug4440
            //fields.Add("CURRENTDATE", DateTime.Now.ToShortDateString());
            fields.Add("PIN", pinNumber);
            fields.Add("USERNAME", username);
            fields.Add("NAME", name);
            string subject = "PIN Authentication";
            string body = string.Empty;
            EMailNotification notify = new EMailNotification(body, subject, email);
            body = notify.SendActualNotification(templateActualPath + "//PINInformationEmail.txt", fields, true);
            //do not create communication event for this.
            return true;
        }


        public static bool NotifyUserName(string templateActualPath, string email, string mcaid, string pdmsurl)
        {

            string username = GetUserName(email, mcaid);
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            Notification n = new Notification();
            Dictionary<string, object> fields = new Dictionary<string, object>();
            //commented for bug4440
            //fields.Add("CURRENTDATE", DateTime.Now.ToShortDateString());
            fields.Add("USERNAME", username);
            fields.Add("PDMSURL", pdmsurl);
            string subject = "User Name Recovery";
            string body = string.Empty;
            EMailNotification notify = new EMailNotification(body, subject, email);

            body = notify.SendActualNotification(templateActualPath + "//UserNameRecovery.txt", fields, true);

            //do not create communication event for this.
            return true;
        }

        private static string GetUserName(string email, string mcaid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("email", DbType.String, email, false));
                parameters.Add(SqlParms.CreateParameter("mcaid", DbType.String, mcaid, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_RecoverUserName", parameters, "usp_RecoverUserName");
                return ObjectControllerHelper.HasRows(ds) ? ds.Tables[0].Rows[0]["UserName"].ToString() : null;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectUserApplicationTypes(string userId)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:10PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectUserApplicationTypes", parameters, "UserApplicationTypes");
                ds.Tables[0].TableName = "UserApplicationType";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectUserProviderTypes(string userId)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:13PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectUserProviderTypes", parameters, "UserProviderTypes");
                ds.Tables[0].TableName = "UserProviderType";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectUserStatusTypes(string userId)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:13PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, userId, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectUserStatusTypes", parameters, "UserStatusTypes");
                ds.Tables[0].TableName = "UserStatusType";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectActiveUsersByRole(string role)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ROLE", DbType.String, role, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectActiveUsersByRole", parameters, "ActiveUsers");
                ds.Tables[0].TableName = "ActiveUsers";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectUsersInRoles(string roles)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("GroupNameList", DbType.String, roles, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectUsersInRoles", parameters, "ActiveUsersInRoles");
                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "ActiveUsersInRoles";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectUsersInProviderAdminRole(string role, int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("GroupName", DbType.String, role, true));
                parameters.Add(SqlParms.CreateParameter("regID", DbType.Int32, regID, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectUsersInProviderAdminRole", parameters, "ActiveUsersInRoles");
                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "ActiveUsersInRoles";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectUsersInRolesByTaxID(string roles, string taxID, string excludeUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("GroupNameList", DbType.String, roles, true));
                parameters.Add(SqlParms.CreateParameter("taxId", DbType.String, taxID, true));
                parameters.Add(SqlParms.CreateParameter("excludeUser", DbType.String, excludeUser, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectUsersInRolesByTaxID", parameters, "ActiveUsersInRolesByTaxID");
                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "ActiveUsersInRolesByTaxID";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectEmailNotifications(Guid userId, int regId, int pageSize, int pageNumber, string subject, string npi, string columnName = null, string sortDirection = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, userId.ToString(), true));

                if (regId > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                }


                if (!string.IsNullOrEmpty(subject))
                {
                    parameters.Add(SqlParms.CreateParameter("subject", DbType.String, subject, false));
                }

                if (!string.IsNullOrEmpty(npi))
                {
                    parameters.Add(SqlParms.CreateParameter("npi", DbType.String, npi, false));
                }

                if (!string.IsNullOrEmpty(columnName))
                {
                    parameters.Add(SqlParms.CreateParameter("columnName", DbType.String, columnName, false));
                }

                if (!string.IsNullOrEmpty(columnName))
                {
                    parameters.Add(SqlParms.CreateParameter("sortDirection", DbType.String, sortDirection, false));
                }

                parameters.Add(SqlParms.CreateParameter("pageSize", DbType.Int32, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("pageNumber", DbType.Int32, pageNumber, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailNotifications_PAGED", parameters, "EmailNotifications");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[1].TableName = "EmailNotifications";
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectEmailNotificationsByRegID(Guid userId, int regId, int pageSize, int pageNumber, string subject, string npi, string sortBy, string sortDirection)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId.ToString(), true));

                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, true));

                if (!string.IsNullOrEmpty(subject))
                {
                    parameters.Add(SqlParms.CreateParameter("Subject", DbType.String, subject, false));
                }

                if (!string.IsNullOrEmpty(npi))
                {
                    parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    parameters.Add(SqlParms.CreateParameter("SortBy", DbType.String, sortBy, false));
                }

                if (!string.IsNullOrEmpty(sortDirection))
                {
                    parameters.Add(SqlParms.CreateParameter("SortDirection", DbType.String, sortDirection, false));
                }

                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.Int32, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageNumber, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailNotificationsByRegID_PAGED", parameters, "EmailNotifications");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[1].TableName = "EmailNotifications";
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetEmailAttachments(string COMMUNICATION_EVENT_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_ID", DbType.Guid, COMMUNICATION_EVENT_ID, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectATTACHMENTS", parameters, "Attachments");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[0].TableName = "Attachments";
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegistrationStatuses(string UserId, int REG_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, UserId, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Guid, REG_ID, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationStatuses", parameters, "Status");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[0].TableName = "Status";
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegistrationXREF(int REG_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Guid, REG_ID, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_USER_XREFcustom", parameters, "XREF");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[0].TableName = "XREF";
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void TransferRegistrationToNewUser(int REG_ID, string fromUserId, string toUserId, bool isFromUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("TO_USER", DbType.Guid, toUserId, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, REG_ID, false));
                parameters.Add(SqlParms.CreateParameter("FROM_USER", DbType.Guid, fromUserId, false));
                parameters.Add(SqlParms.CreateParameter("IS_FROM_USER", DbType.Boolean, isFromUser, false));

                DataAccess.ExecuteStoredProcedure("usp_TransferRegistrationToNewUser", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet PasswordExistsInPast(int months, Guid userid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("months", DbType.Int32, months, true));
                parameters.Add(SqlParms.CreateParameter("userid", DbType.Guid, userid, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_PasswdExistsInPast", parameters, "PASSHISTORY");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[0].TableName = "PASSHISTORY";
                }


                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void CopyPasswordToPwdHistory(Guid userid, Guid createdby)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("userid", DbType.Guid, userid, true));
                parameters.Add(SqlParms.CreateParameter("createdby", DbType.Guid, createdby, true));
                DataAccess.ExecuteStoredProcedure("usp_CopyPasswordToPwdHistory", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void SetForcePasswordReset(Guid userid, bool FORCE_PASSWORD_RESET, bool isTempPwdSent)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("userid", DbType.Guid, userid, true));
                parameters.Add(SqlParms.CreateParameter("FORCE_PASSWORD_RESET", DbType.Boolean, FORCE_PASSWORD_RESET, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("IS_TEMP_PWD_SENT", DbType.Boolean, isTempPwdSent, true));
                DataAccess.ExecuteStoredProcedure("usp_SetForce_Password_Reset", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet IsActiveReset(string recordid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("recordid", DbType.Guid, recordid, true));               // DataAccess.ExecuteStoredProcedure("usp_CheckActivePasswordReset", parameters);



                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_CheckActivePasswordReset", parameters, "ACTIVEPASSRESET");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[0].TableName = "ACTIVEPASSRESET";
                }


                return ds;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static Guid CreatePasswordRequest(Guid userId)
        {
            //create stored procedure to create a record in password request table
            Guid recordid;
            recordid = Guid.NewGuid();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("recordid", DbType.Guid, recordid, true));
                parameters.Add(SqlParms.CreateParameter("userid", DbType.Guid, userId, true));
                DataAccess.ExecuteStoredProcedure("usp_InsertPasswordRequest", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

            return recordid;
        }

        public static DataSet GetUsermembershipInfoByUserName(string userName)
        {
            DataSet userMembershipInfo = null;
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("user_name", DbType.String, userName, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectPasswordExpiryInfoByUsername", parameters, "MembershipInfo");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    userMembershipInfo = ds;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return userMembershipInfo;
        }

        public static DataSet GetAllAgentsByProviderAdmin(string ProvAdminUserID)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:10PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ProvAdminUserID", DbType.Guid, ProvAdminUserID, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetAllAgentsByProviderAdmin", parameters, "AgentsByProviderAdmin");
                ds.Tables[0].TableName = "AgentsByProviderAdmin";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAgentRolesByProviderAdmin(string ProvAdminUserID, int RegID, int Offset, int PageSize, string ProvAgentUserName, bool isCostRptMgmtAgent)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:10PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ProvAdminUserID", DbType.Guid, ProvAdminUserID, true));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegID, true));
                parameters.Add(SqlParms.CreateParameter("Offset", DbType.Int32, Offset, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.Int32, PageSize, true));
                parameters.Add(SqlParms.CreateParameter("ProvAgentUserName", DbType.String, ProvAgentUserName, true));
                parameters.Add(SqlParms.CreateParameter("isCostRptMgmtAgent", DbType.Boolean, isCostRptMgmtAgent, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectAgentrolesByProviderAdmin", parameters, "AgentRolesByProviderAdmin");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "AgentUserStatusByProviderAdmin";
                    ds.Tables[1].TableName = "AgentRolesByProviderAdmin";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetAgentRolesByProviderAdminCount(string ProvAdminUserID, int RegID, string ProvAgentUserName)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ProvAdminUserID", DbType.Guid, ProvAdminUserID, true));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegID, true));
				parameters.Add(SqlParms.CreateParameter("ProvAgentUserName", DbType.String, ProvAgentUserName, true));

				DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetAgentrolesByProviderAdminCount", parameters, "AgentRolesByProviderAdmin");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectAgentFacilities_ContractsByCEOUser(string CEOUserID)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:10PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("CEOUserID", DbType.Guid, CEOUserID, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectDoDDFacilities_ContractsByCEOUser", parameters, "AgentRolesByProviderAdmin");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "SecondaryUserFacilityStatusByCEOUser";
                    ds.Tables[1].TableName = "FacilityStatusByCEOUser";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveAgentRolesByProviderAdmin(Dictionary<string, object> parms)
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

                DataAccess.ExecuteStoredProcedure("usp_SaveAgentRoleChangedByProvAdmin", parameters);                
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void SaveSecondaryUserFacility_ContractByCEOUser(Dictionary<string, object> parms)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:10PM
                // create parameters objects and fill with values
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
                DataAccess.ExecuteStoredProcedure("usp_SaveSecUserFacility_ContractsByCEOUsr", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void DeleteProviderAgentMapping(int RegId, string ProvAdminID, Guid AgentUserID )
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, false));
                parameters.Add(SqlParms.CreateParameter("PROV_ADMIN_ID", DbType.Guid,ProvAdminID, false));
                parameters.Add(SqlParms.CreateParameter("AGENT_USERID", DbType.Guid,AgentUserID, false));
                DataAccess.ExecuteStoredProcedure("usp_DeleteProviderAgentRoleMapping", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
           
        }

        public static void InsertNewProviderAgent(string AgentUserID, string ProvAdminUserID, int RegID, bool IsActive, string ChangedBy)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:10PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("AgentUserID", DbType.Guid, AgentUserID, true));
                parameters.Add(SqlParms.CreateParameter("ProvAdminUserID", DbType.Guid, ProvAdminUserID, true));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegID, true));
                parameters.Add(SqlParms.CreateParameter("IsActive", DbType.Boolean, IsActive, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, ChangedBy, true));


                DataAccess.ExecuteStoredProcedure("usp_Insert_USER_AGENT_REG_XREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet CheckAgentRolesByProviderAdmin(string ProvAdminUserID, int RegID)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:10PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ProvAdminUserID", DbType.Guid, ProvAdminUserID, true));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegID, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_CheckAgentRoleExistByProvAdmin", parameters, "CheckAgentRolesByProviderAdmin");
                ds.Tables[0].TableName = "CheckAgentRolesByProviderAdmin";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectPendingAgentsByProviderAdmin(string ProvAdminUserID)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:10PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ProvAdminUserID", DbType.Guid, ProvAdminUserID, true));
                //parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegID, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectPendingAgentsByProviderAdmin", parameters, "PendingAgentsByProviderAdmin");
                ds.Tables[0].TableName = "PendingAgentsByProviderAdmin";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        private static string AddEmail(string addressList, DataSet ds, string colName)
        {
            string rtn = addressList;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string email = dr[colName].ToString();
                    if (!string.IsNullOrEmpty(email) && rtn.IndexOf(email) == -1)
                    {
                        if (!string.IsNullOrEmpty(rtn)) rtn += ",";
                        rtn += email;
                    }
                }
            }
            return rtn;
        }

        private static string GetRecipients(int regId)
        {
            string rtn = string.Empty;
            try
            {
                
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parameters, "EmailRecipients");
                rtn = AddEmail(rtn, ds, "CONTACT_EMAIL_ADDRESS");
                //add additional emails
                //rtn = AddEmail(rtn, ds, "CONTACT_EMAIL_ADDRESS1");
                //rtn = AddEmail(rtn, ds, "CONTACT_EMAIL_ADDRESS2");
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_Users", parameters, "EmailRecipients");
                rtn = AddEmail(rtn, ds, "Email");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

            return rtn;
        }

        public static void NotifyBackgroundCheckWithoutPrints(string subject,string owner, string templateActualPath, string file, string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid(), string emailFrom = null)
        {
            Notification n = new Notification();
            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("PDMSURL", pdmsUrl);
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("REG_ID", regId));
            //DataSet ds1 = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms, "reginfo");
            //string providerName = Methods.GetStringValue(ds1.Tables[0].Rows[0]["Name"]).ToLower();
            //fields.Add("PROVIDERNAME", providerName);
            recipients = GetRecipients(regId);
            string pdmsEmail = DataAccess.GetAppSetting("PDMSEMAIL");
            fields.Add("PDMSEMAIL", pdmsEmail);
            fields.Add("OWNERNAME", owner);
            //string subject = "Second Background Check Notification";
            string body = string.Empty;
            EMailNotification notify = new EMailNotification(body, subject, recipients);
			notify.Reg_ID = regId.ToString();
			fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
            //OHPNM-16344-Populating required fields in 2nd BCI notice
            // Set the Expire Date
            AddBookmark(fields, "EXPIREDATE", DateTime.Now.AddDays(30).ToString("MM/dd/yyyy"));

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            
            // Get the names of people needing FCBC and add any found to an HTML table row <tr>
            string providerListHtml = owner;
            AddBookmark(fields, "PROVIDER_LIST", providerListHtml);
             
            sqlParams.Add(new SqlParameter("REG_ID", regId));
            DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER", sqlParams,"reginfo");

            sqlParams.Clear();
            sqlParams.Add(new SqlParameter("REG_ID", regId));
            DataSet dsServiceLoc = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", sqlParams,"regsvcinfo");
            string medicaidID = string.Empty;
            if (Methods.HasRows(dsServiceLoc))
            {
                DataRow row = dsServiceLoc.Tables[0].Rows[0];
                medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                GetFromAddress(fields);

                string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                    row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                    row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                    (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                        : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId.ToString(), Methods.GetStringValue(row, "DBA"));
            }
                       
            body = notify.SendNotification(templateActualPath + "\\" + file, fields, true);
            
            #region COMMUNICATION EVENT
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
            string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
            parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, recipients, false));
            parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
            parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
            parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, file, false));
            parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, ObjectControllerHelper.GetKeyValueString(fields), false));
            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
            parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, userid, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, true));
            parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean, notify.isEmailSent, true));
            parameters.Add(SqlParms.CreateParameter("log_message", DbType.String, notify.log_message, true));
            string comEventID = DataAccess.ExecuteScalar("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);
            #endregion

        }

        private static void AddBookmark(Dictionary<string, object> fields, string bookmarkName, string value)
        {
            if (string.IsNullOrEmpty(value)) fields.Add(bookmarkName, string.Empty);
            else fields.Add(bookmarkName, value);
        }

        private static void GetFromAddress(Dictionary<string, object> fields)
        {
            string strFromAddress = AppSettings.Get("Mail-FromAddress", string.Empty);
            AddBookmark(fields, "FROMADDRESS", strFromAddress);
        }

        private static void GetToAddress(Dictionary<string, object> fields, string ContactName, string Quadrant, string Address1, string Address2, string CityStateZip, string ProviderName = "", string NPI = "", string MedicaidID = "", string Taxonomy = "", string EFFECTIVEDATE = "", string DateType = "")
        {
            string strToaddress = string.Empty;

            if (!string.IsNullOrEmpty(MedicaidID.ToString()))

            {
                MedicaidID = "Medicaid ID: " + MedicaidID;
            }
            else
            {
                MedicaidID = "";
            }

            bool Medicaidflag = false;
            bool Taxonomyflag = false;
            bool Effectivedateflag = false;

            if (!string.IsNullOrEmpty(Taxonomy.ToString()))
            {
                Taxonomy = "Taxonomy: " + Taxonomy;
            }
            else
            {
                Taxonomy = "";
            }

            if (DateType == "Re-Enrollment Due Date")
            {
                DateType = "Re-Enrollment Due Date: ";
            }
            else
            {
                DateType = "Effective Date: ";

            }


            if (!string.IsNullOrEmpty(EFFECTIVEDATE.ToString()))
            {
                EFFECTIVEDATE = DateType + EFFECTIVEDATE;
            }
            else
            {
                EFFECTIVEDATE = "";
            }
            if (!string.IsNullOrEmpty(NPI.ToString()))
            {
                NPI = "NPI: " + NPI;
            }
            else
            {
                NPI = "";
            }

            if (!string.IsNullOrEmpty(Quadrant.ToString()))
            {
                Address1 = Address1 + " " + Quadrant;
            }

            if (ProviderName == "")
            {
                strToaddress += "<p>" + ContactName + "<p/>";
                strToaddress += "<p>" + Address1 + "<p/>";
                if (Address2 != "")
                    strToaddress += "<p>" + Address2 + "<p/>";

                strToaddress += "<p>" + CityStateZip + "<p/>";
            }
            else
            {
                ProviderName = "Provider Name: " + ProviderName;
                //  strToaddress += "<table style=\"font-family:'Times New Roman';font-size:12pt;width:100%;margin:0px;cellpadding: 0px;cellspacing: 0px\">";
                strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Times New Roman';font-size:12pt\">" + ContactName + "</td>";
                strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + ProviderName + "</td></tr>";
                strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Times New Roman';font-size:12pt\">" + Address1 + "</td>";
                strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + NPI + "</td></tr>";
                if (Address2.Trim() != "")
                {
                    strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Times New Roman';font-size:12pt\">" + Address2 + "</td>";
                    if (MedicaidID != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + MedicaidID + "</td></tr>";
                        Medicaidflag = true;
                    }
                    else if (Taxonomy != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + Taxonomy + "</td></tr>";
                        Taxonomyflag = true;

                    }
                    else if (EFFECTIVEDATE != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + EFFECTIVEDATE + "</td></tr>";
                        Effectivedateflag = true;

                    }
                    else
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\"></td></tr>";
                    }

                }

                strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Times New Roman';font-size:12pt\">" + CityStateZip + "</td>";
                if (Address2.Trim() == "")

                    if (MedicaidID != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + MedicaidID + "</td></tr>";
                        Medicaidflag = true;

                    }
                    else if (Taxonomy != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + Taxonomy + "</td></tr>";

                        Taxonomyflag = true;

                    }
                    else if (EFFECTIVEDATE != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + EFFECTIVEDATE + "</td></tr>";
                        Effectivedateflag = true;
                    }
                    else
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\"></td></tr>";
                    }
                else
                    if (MedicaidID != "" && !(Medicaidflag))
                {
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + MedicaidID + "</td></tr>";
                }
                else if (Taxonomy != "" && !(Taxonomyflag))
                {
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + Taxonomy + "</td></tr>";
                    Taxonomyflag = true;
                }
                else if (EFFECTIVEDATE != "" && !(Effectivedateflag))
                {
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + EFFECTIVEDATE + "</td></tr>";
                    Effectivedateflag = true;

                }
                else
                {
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\"></td></tr>";
                }



                {
                    if (Taxonomy != "" && !(Taxonomyflag))
                    {
                        strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Times New Roman';font-size:12pt\"></td>";
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + Taxonomy + "</td></tr>";
                    }

                    if (EFFECTIVEDATE != "" && !(Effectivedateflag))
                    {
                        strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Times New Roman';font-size:12pt\"></td>";
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Times New Roman';font-size:12pt\">" + EFFECTIVEDATE + "</td></tr>";
                    }
                }

                //   strToaddress += "</table>";
            }

            AddBookmark(fields, "TOADDRESS", strToaddress);
        }

        public static void NotifyRiskLevelBumpUp(string templateActualPath, string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid(), string emailFrom = null)
        {
            Notification n = new Notification();
            Dictionary<string, object> fields = new Dictionary<string, object>();

            fields.Add("PDMSURL", pdmsUrl);

            List<SqlParameter> parms1 = new List<SqlParameter>();
            parms1.Add(new SqlParameter("REG_ID", regId));
            parms1.Add(new SqlParameter("UserID", Constants.appAdminUserId));
            DataSet ds2 = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationHeader", parms1, "regHEader");
            string bumpupReason = Methods.GetStringValue(ds2.Tables[0].Rows[0]["BUMP_UP_REASON_DESC"]);
            fields.Add("BUMPUPREASON", bumpupReason);

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("REG_ID", regId));
            DataSet ds1 = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms, "reginfo");

            string providerName = Methods.GetStringValue(ds1.Tables[0].Rows[0]["Name"]).ToLower();

            fields.Add("PROVIDERNAME", providerName);
            recipients = GetRecipients(regId);
            string pdmsEmail = DataAccess.GetAppSetting("PDMSEMAIL");
            fields.Add("PDMSEMAIL", pdmsEmail);
          //string subject = "Notification of Increased Risk Level During Re-enrollment in the District of Columbia Medicaid Program";
            string subject = "Notification of Increased Risk Level for Ohio Medicaid";   // Jira 2194
            string body = string.Empty;
            EMailNotification notify = new EMailNotification(body, subject, recipients);

            fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));



            parms.Clear();
            parms.Add(new SqlParameter("REG_ID", regId));
            DataSet dsServiceLoc = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms, "reg");
            string medicaidID = string.Empty;
            if (Methods.HasRows(dsServiceLoc))
            {
                DataRow row = dsServiceLoc.Tables[0].Rows[0];
                medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
            }
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds1.Tables[0].Rows[0];
                GetFromAddress(fields);
            }
            if (Methods.HasRows(ds1))
            {
                DataRow row = ds1.Tables[0].Rows[0];
                string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();


                GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                                  row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                                  row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                                  (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                      : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", "");
            }
            body = notify.SendNotification(templateActualPath + "//RiskLevelBumpUp.txt", fields, true);


            #region COMMUNICATION EVENT
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
            string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
            parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, recipients, false));
            parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
            parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
            parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, "RiskLevelBumpUp.txt", false));
            parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, ObjectControllerHelper.GetKeyValueString(fields), false));
            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
            parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, userid, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, true));
            parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean, notify.isEmailSent, true));
            parameters.Add(SqlParms.CreateParameter("log_message", DbType.String, notify.log_message, true));
            string comEventID = DataAccess.ExecuteScalar("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);
            #endregion

        }

        public static void SavePendingAgentsByProviderAdmin(string AgentUserID, string ProvAdminUserID)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:10PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("AgentUserID", DbType.String, AgentUserID, true));
                parameters.Add(SqlParms.CreateParameter("ProvAdminUserID", DbType.Guid, ProvAdminUserID, true));


                DataAccess.ExecuteStoredProcedure("usp_SavePendingAgentsByProviderAdmin", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderByProviderAdmin(string ProvAdminUserID, string eMail)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2013  3:10PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ProvAdminUserID", DbType.Guid, ProvAdminUserID, true));
                parameters.Add(SqlParms.CreateParameter("email", DbType.String, eMail, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectProviderByProviderAdmin", parameters, "SelectProviderByProviderAdmin");
                ds.Tables[0].TableName = "SelectProviderByProviderAdmin";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectCostReportDocument(int documentId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_DocumentById", parameters, "CostReportDocument");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    ds.Tables[0].TableName = "CostReportDocument";
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        
        public static DataSet GetMMISTrackingDetails()
        {
            try
            {
                DataSet ds = new DataSet();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
               ds.ReadXml(templateActualPath + @"/MITS.xml");
                //ds.ReadXml(@"C:\PDMS\PDMS\ProviderDataManagementSystemService\Documents\MITS.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetMSPDuedateDetails()
        {
            try
            {
                DataSet ds = new DataSet();
                string templateActualPath = @"C:\PDMS\PDMS\ProviderDataManagementSystemService\Documents";
                if (!File.Exists(templateActualPath + @"/DUEDATERES.xml")) // Jira 2636
                {
                    templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty); ;
                }
                ds.ReadXml(templateActualPath + @"/DUEDATERES.xml");
                //ds.ReadXml(@"C:\PDMS\PDMS\ProviderDataManagementSystemService\Documents\DUEDATERES.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SearchCostReportDetails()
        {
            try
            {
                DataSet ds = new DataSet();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                 //////////////ds.ReadXml(templateActualPath + @"/SearchResponse.xml");
                  ds.ReadXml(@"C:\PDMS\PDMS\ProviderDataManagementSystemService\Documents\SearchResponse.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        
        public static DataSet MSPSearchCostReportDetails()
        {
            try
            {
                DataSet ds = new DataSet();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                ///////////////////////ds.ReadXml(templateActualPath + @"/MSPSearchResponse.xml");
                //ds.ReadXml(templateActualPath + @"/RecipientEligibilityRequestResponse.xml");
                 ds.ReadXml(@"C:\PDMS\PDMS\ProviderDataManagementSystemService\Documents\MSPSearchResponse.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet HospitalSearchCostReportDetails()
        {
            try
            {
                DataSet ds = new DataSet();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                //ds.ReadXml(templateActualPath + @"/HospitalSearchResponse.xml");
                //ds.ReadXml(templateActualPath + @"/RecipientEligibilityRequestResponse.xml");
                //ds.ReadXml(@"C:\PDMS\PDMS\ProviderDataManagementSystemService\Documents\HospitalSearchResponse.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void NotifyProviderTempPassword(string templateActualPath, string recipients, string USER_NAME, string LOGIN_URL, string tempPswd, int regId = 0, Guid userid = new Guid(), string emailFrom = null)
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("USER_NAME", USER_NAME);
            fields.Add("LOGIN_URL", LOGIN_URL);
            fields.Add("TEMP_PSWD", tempPswd);
            //Bug NE branding
            string subject = DataAccess.GetAppSetting("BrandName") + " Temporary Password";

            string body = string.Empty;
            EMailNotification notify = new EMailNotification(body, subject, recipients);
            //templateActualPath = @"C:\Projects\OHPDMS\PDMS\ProviderDataManagementSystemService\Documents\";
            body = notify.SendActualNotification(templateActualPath + "//TemporaryPasswordEmail.txt", fields, true);

            #region COMMUNICATION EVENT
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
            string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
            parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, recipients, false));
            parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
            parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
            parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, "TemporaryPasswordEmail.txt", false));
            parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, ObjectControllerHelper.GetKeyValueString(fields), false));
            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
            parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, userid, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, true));
            parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean, notify.isEmailSent, true));
            parameters.Add(SqlParms.CreateParameter("log_message", DbType.String, notify.log_message, true));
            DataAccess.ExecuteStoredProcedure("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);
            #endregion
        }
		
        public static bool UserCanAccessReg(string userId, int regId)
        {
            try
            {
                int results;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("userId", DbType.String, userId, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                results = Convert.ToInt32(DataAccess.ExecuteScalar("usp_ValidateUserCanAccessReg", parameters, CommandType.StoredProcedure));

                if (results == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet CheckIfUserExists(string userid)
        {
            DataSet retUserRoles = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("USERID", DbType.Guid, userid, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckIfUserExists", parameters, "CheckIfUserExists");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    retUserRoles = ds;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return retUserRoles;
        }
        public static DataSet GetPowerAgentInformation(string loggedinUserID, string provAdminUserID, int regID)
        {
            DataSet ds = null;
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("POWER_AGENT_USER_ID", DbType.Guid, loggedinUserID, false));
                sqlParms.Add(SqlParms.CreateParameter("PROVIDER_ADMIN_USER_ID", DbType.Guid, provAdminUserID, true));
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_GetPowerAgentInformation", sqlParms, "PowerAgentInformation");
                return ds;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet ValidateGlobalAdminChange(string currentOHID, string newOHID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CurrentOHID", DbType.String, currentOHID ?? string.Empty, false));
                parameters.Add(SqlParms.CreateParameter("NewOHID", DbType.String, newOHID ?? string.Empty, false));

                DataSet result = DataAccess.ExecuteStoredProcedure("usp_ValidateGlobalAdminChange", parameters, "validationResult");
                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

    }
}
