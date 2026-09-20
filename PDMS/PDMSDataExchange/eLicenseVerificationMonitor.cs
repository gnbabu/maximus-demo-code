using Corp.Core.Libraries.Helper;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
   public class eLicenseVerificationMonitor : BaseJob, IJob
   {
        public eLicenseVerificationMonitor(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private Logging log = null;

        override public void ExecuteJob()
        {
           this.ExecuteJob(Guid.Parse("B368A88E-932F-4B06-884A-CA7185CA905A"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            if (jobGuid == "B368A88E-932F-4B06-884A-CA7185CA905A")
            {
                UpdateOhioeLicense();
            }
        }

        public void UpdateOhioeLicense()
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            
            string enrollmentStatusID = string.Empty;
            string enrollmentStatusReasonID = string.Empty;
            Guid userid = new Guid(Constants.appPDMSDataExchangeUserId);
            try
            {
                log.CreateLogEntry("Checking for providers that need eLicense verification");
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet eLicenseds = DataAccess.ExecuteStoredProcedure("usp_SelectRegforELicenseVerification", parameters, "eLicenseRegIds");
                if (ObjectControllerHelper.HasRows(eLicenseds))
                {
                    foreach (DataRow row in eLicenseds.Tables[0].Rows)
                    {
                        int regID = ObjectControllerHelper.GetInt("REG_ID", row);
                        int regLicensureID = ObjectControllerHelper.GetInt("REG_LICENSURE_ID", row);
                        int regAddressID = ObjectControllerHelper.GetInt("REG_ADDRESSID", row);
                        string licenseNum = ObjectControllerHelper.GetString("LICENSE_NUMBER", row);
                        string licenseTypeId = ObjectControllerHelper.GetString("LICENSE_TYPE_ID", row);
                        string lname = ObjectControllerHelper.GetString("LastName", row);
                        string dob = ObjectControllerHelper.GetString("BirthDate", row);
                        string boardName = ObjectControllerHelper.GetString("LICENSE_BOARD_NAME", row);
                        string taxID = ObjectControllerHelper.GetString("TaxID", row);
                        string last4 = taxID.Substring(5);
                        if(!string.IsNullOrEmpty(lname) && !string.IsNullOrEmpty(dob) && !string.IsNullOrEmpty(taxID) && !string.IsNullOrEmpty(boardName) && !string.IsNullOrEmpty(last4))
                        {
                            ELicenseVerificationRequest eRequest = new ELicenseVerificationRequest(lname, dob, last4, boardName);
                            Object response = GetLicenseVerificationResult(eRequest, regID, licenseTypeId);
                            if (response != null)
                            {
                                if (!response.ToString().Contains("No license found"))
                                {
                                    ELicenseVerificationResponse responseObj = (ELicenseVerificationResponse)response;
                                    Dictionary<string, string> ValidLicenseStatus = ActiveElicenseStatus();
                                    if (ValidLicenseStatus.ContainsKey(responseObj.license_sub_status) && ValidLicenseStatus.ContainsValue(responseObj.license_status))
                                    {
                                        log.CreateLogEntry("Begin Update data from response for RegID:" + regID.ToString());
                                        // Update the Name (First and Last Name), License Type, Address, Current Status, License Status Code, Effective Date and End Date to match
                                        List<SqlParameter> param = new List<SqlParameter>();
                                        param.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                                        param.Add(SqlParms.CreateParameter("REG_LICENSURE_ID", DbType.Int32, regLicensureID, true));
                                        param.Add(SqlParms.CreateParameter("REG_ADDRESSID", DbType.Int32, regAddressID, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_TYPE_ID", DbType.String, licenseTypeId, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_NUMBER", DbType.String, responseObj.license_number, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_EFF_DATE", DbType.String, responseObj.begin_date, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_END_DATE", DbType.String, responseObj.end_date, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_BOARD_NAME", DbType.String, responseObj.board_type, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_STATUS", DbType.String, responseObj.license_status, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_SUBSTATUS", DbType.String, responseObj.license_sub_status, true));
                                        param.Add(SqlParms.CreateParameter("ELICENSE_VERIFIED", DbType.Boolean, true, true));
                                        param.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, responseObj.first_name, true));
                                        param.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, responseObj.last_name, true));
                                        param.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, Constants.AddressType.ProfessionalLicenseAddress, true));
                                        param.Add(SqlParms.CreateParameter("ADDRESS1", DbType.String, responseObj.address_1.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("ADDRESS2", DbType.String, responseObj.address_2.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("CITY", DbType.String, responseObj.city.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("STATE", DbType.String, responseObj.state.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("ZIP", DbType.String, responseObj.zip.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("COUNTY", DbType.String, responseObj.county.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.String, Constants.RegistrationModifiedStatusType.Changed.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                                        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, true));
                                        DataAccess.ExecuteStoredProcedure("usp_UpdateBy_eLicenseVerificationResult", param);
                                        log.CreateLogEntry("End Update data from response for RegID:" + regID.ToString());

                                    }
                                    Dictionary<string, string> InValidLicenseStatus = InActiveElicenseStatus();
                                    if (InValidLicenseStatus.ContainsKey(responseObj.license_sub_status) && 
                                        (InValidLicenseStatus.ContainsValue(responseObj.license_status) || responseObj.license_status.ToString() == "Closed" || responseObj.license_status.ToString() == "Relinquished"))
                                    {
                                        log.CreateLogEntry("Begin Update data from response for RegID:" + regID.ToString());
                                        List<SqlParameter> param = new List<SqlParameter>();
                                        param.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                                        param.Add(SqlParms.CreateParameter("REG_LICENSURE_ID", DbType.Int32, regLicensureID, true));
                                        param.Add(SqlParms.CreateParameter("REG_ADDRESSID", DbType.Int32, regAddressID, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_TYPE_ID", DbType.String, licenseTypeId, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_NUMBER", DbType.String, responseObj.license_number, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_EFF_DATE", DbType.String, responseObj.begin_date, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_END_DATE", DbType.String, responseObj.end_date, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_BOARD_NAME", DbType.String, responseObj.board_type, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_STATUS", DbType.String, responseObj.license_status, true));
                                        param.Add(SqlParms.CreateParameter("LICENSE_SUBSTATUS", DbType.String, responseObj.license_sub_status, true));
                                        param.Add(SqlParms.CreateParameter("ELICENSE_VERIFIED", DbType.Boolean, true, true));
                                        param.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, responseObj.first_name, true));
                                        param.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, responseObj.last_name, true));
                                        param.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, Constants.AddressType.ProfessionalLicenseAddress, true));
                                        param.Add(SqlParms.CreateParameter("ADDRESS1", DbType.String, responseObj.address_1.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("ADDRESS2", DbType.String, responseObj.address_2.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("CITY", DbType.String, responseObj.city.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("STATE", DbType.String, responseObj.state.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("ZIP", DbType.String, responseObj.zip.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("COUNTY", DbType.String, responseObj.county.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.String, Constants.RegistrationModifiedStatusType.Changed.ToString(), true));
                                        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                                        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, true));
                                        DataAccess.ExecuteStoredProcedure("usp_UpdateBy_eLicenseVerificationResult", param);
                                        log.CreateLogEntry("End Update data from response for RegID:" + regID.ToString());

                                        // Check if we can Terminate the Provider and then Terminate it and Send email
                                        bool canTerminate = false;
                                        parameters = new List<SqlParameter>();
                                        parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                                        parameters.Add(SqlParms.CreateParameter("REG_LICENSURE_ID", DbType.Int32, regLicensureID, true));
                                        parameters.Add(SqlParms.CreateParameter("LICENSE_NUM", DbType.String, responseObj.license_number, true));
                                        canTerminate = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CanTerminateProvider", parameters));

                                        if (canTerminate)
                                        {
                                            //Send Email
                                            bool isEmailSent = SendEmail(regID);
                                            if (isEmailSent)
                                            {
                                                enrollmentStatusID = Constants.EnrollStatus.INACTIVE.ToString();
                                                enrollmentStatusReasonID = "70"; // IN	- LICENSE/CERTIFICATION NOT ACTIVE 
                                                if (responseObj.license_sub_status == "Abandoned" && (responseObj.license_status == "Inactive" || responseObj.license_status == "Closed"))
                                                    enrollmentStatusReasonID = "41"; //39	-LICENSE ABANDONED       
                                                else if ((responseObj.license_sub_status == "Expired" || responseObj.license_sub_status == "Lapsed") && responseObj.license_status == "Inactive")
                                                    enrollmentStatusReasonID = "31"; //3 -	LICENSE/CERTIFICATION NOT RENEWED  
                                                else if (responseObj.license_sub_status == "Revocation" && responseObj.license_status == "Inactive")
                                                    enrollmentStatusReasonID = "20"; //2 -	LICENSE/CERTIFICATION REVOKED  
                                                else if (responseObj.license_sub_status == "Suspended" && responseObj.license_status == "Inactive")
                                                    enrollmentStatusReasonID = "29"; // 28 - LICENSE SUSPEND - LICENSE BRD
                                                else if (responseObj.license_sub_status == "Retired" && responseObj.license_status == "Inactive")
                                                    enrollmentStatusReasonID = "40"; // 38-	RETIRED  
                                                else if (responseObj.license_sub_status == "Deceased" && responseObj.license_status == "Closed")
                                                    enrollmentStatusReasonID = "19"; // 19-	DECEASED 
                                                else if (responseObj.license_sub_status == "Permanent Revocation" && responseObj.license_status == "Closed")
                                                    enrollmentStatusReasonID = "20"; // 2- LICENSE/CERTIFICATION REVOKED                                              

                                                log.CreateLogEntry("Begin Terminate RegID:" + regID.ToString());
                                                //List<SqlParameter> parameters = new List<SqlParameter>();
                                                parameters = new List<SqlParameter>();
                                                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                                                parameters.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, DateTime.Now, false));
                                                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                                                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, false));
                                                parameters.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, true, false));
                                                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, enrollmentStatusID, false));
                                                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASON_CODE", DbType.String, enrollmentStatusReasonID, false));
                                                DataAccess.ExecuteStoredProcedure("usp_TerminateProvider", parameters);
                                                log.CreateLogEntry("End Terminate RegID:" + regID.ToString());

                                                InsertProviderFeedNotes(regID, " Sent Email -  Termination Due to Inactive License", 0, "Job/eLicenseVerificationMonitor/UpdateOhioeLicense");
                                               
                                            }        
                                        }
                                    }
                                }
                            }

                        }
                        else
                            log.CreateLogEntry("Reg Id: " + regID + " - Cannot Verify as the fields LastName,BirthDate,BoardName or TaxID is null.",
                                    Logging.LogPriority.Error);
                    }
                }
            }

            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }

        }

        private void InsertProviderFeedNotes(int regID, string subject, int processID, string method)
        {
            try
            {
                DataSet ds = RegistrationController.CheckProviderJobNotesExists(regID, Constants.EnrollmentType.Job,
                    Constants.FinalDisposition.Terminated, subject);

                bool noteExists = ObjectControllerHelper.HasRows(ds) &&
                          ds.Tables[0].Rows[0]["ISEXISTS"]?.ToString() == "1";

                if (noteExists) return;

                RegistrationController.InsertProviderFeedNotes(
                       regID, 0, null, subject, null,
                       Constants.EnrollmentType.Job,
                       Constants.FinalDisposition.Terminated,
                       processID
                   );

            }
            catch (Exception ex)
            {
                string errorMessage = $"{method} Reg Id: {regID} - An error occurred while inserting Job Provider Notes. Message: {ex.Message ?? string.Empty}";
                log.CreateLogEntry(errorMessage, Logging.LogPriority.Error);
            }

        }
        private bool SendEmail(int regID)
        {
            bool emailSent = false;
            try
            {                
                string subject = "Termination Due to Inactive License";
                DataSet dsEmails = RegistrationController.SelectRegistrationEmails(regID, subject);
                string Template_Name = "EMAIL_TEMPLATE_LICENSE_INACTIVE_TERMINATE";
                if (!ObjectControllerHelper.HasRows(dsEmails))
                {
                    string bccList = "";
                    string recipients = "";
                    GetRecipients(regID, out recipients, out bccList);

                    EMailNotification notify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId, bccList);
                    if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                        emailSent = true;
                    else
                    {
                        log.CreateLogEntry("Reg Id: " + regID + " - problems sending Revalidation email",
                        Logging.LogPriority.Error);
                    }
                }
                else
                {
                    log.CreateLogEntry("Skipping sending Term Inactive License notification for reg Id = " + regID + " as we found an email");
                }
            }
            catch(Exception ex)
            {
                log.CreateLogEntry("Failed to send Term Inactive License notification for Reg Id: " + regID + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
            }
            return emailSent;


        }

        private string AddEmail(string addressList, DataSet ds, string colName)
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

        private void GetRecipients(int regId, out string recList, out string bccList)
        {
            string rtn = string.Empty;
            recList = string.Empty;
            bccList = string.Empty;
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parameters, "EmailRecipients");
                recList = AddEmail(recList, ds, "CONTACT_EMAIL_ADDRESS");
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_Users", parameters, "EmailRecipients");
                recList = AddEmail(recList, ds, "Email");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Reg Id: " + regId.ToString() + ", GetRecipients - " + ex.Message, Logging.LogPriority.Error);
            }

            //return rtn;
        }
        public Object GetLicenseVerificationResult(ELicenseVerificationRequest eRequest, int regID, string licenseTypeId)
        {
            ELicenseVerificationResponse eResponse = new ELicenseVerificationResponse();
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<Object> lstResponseObject = new List<object>();
            string strResponse = string.Empty;
            string strRequest = string.Empty;
            try
            {
                if (eRequest != null)
                {
                    string Uri = AppSettings.Get("eLicenseVerificationWebserviceURL");
                    string testFlag = AppSettings.Get("eLicenseVerificationTestEnabled");
                    strRequest = JsonConvert.SerializeObject(eRequest);


                    if (!string.IsNullOrEmpty(testFlag) && testFlag == "true")
                    {
                        string testFile = AppSettings.Get("eLicenseVerificationTestResponseFile");
                        string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                       // string templateActualPath = @"C:\Projects\OHPDMS\PDMS\ProviderDataManagementSystemService\Documents\";
                        string fileComplete = templateActualPath + testFile;

                        //FileTransferServiceClient client = new FileTransferServiceClient();
                        string contents = File.ReadAllText(fileComplete);
                        if (!string.IsNullOrEmpty(contents))
                        {
                            if (contents.Contains("No license found"))
                            {
                                //No License Found
                                var response = JsonConvert.DeserializeObject(contents);
                                strResponse = response.ToString();
                                lstResponseObject.Add(response);
                            }
                            else
                            {
                                var response = JsonConvert.DeserializeObject<List<ELicenseVerificationResponse>>(contents);
                                strResponse = JsonConvert.SerializeObject(response).ToString();
                                lstResponseObject.Add(response[0]);

                            }
                        }
                    }
                    else
                    {

                        //Make service call
                        //To do AFTER We Receive Service information from State.

                    }
                }
            }
            catch (Exception ex)
            {
                lstResponseObject = null;
                strResponse = MAXIMUS.Core.Libraries.CoreException.FormatException(ex) + " Current Base Directory:" + System.AppDomain.CurrentDomain.BaseDirectory;
                log.CreateLogEntry("Error getting service response for reg Id = " + regID + ".");
            }

            //Before Returning store Erequest n Response
            Guid userid = new Guid(Constants.appPDMSDataExchangeUserId);

            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
            parameters.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, eRequest.last_name, false));
            parameters.Add(SqlParms.CreateParameter("LAST4SSN", DbType.String, eRequest.last_4_ssn, false));
            parameters.Add(SqlParms.CreateParameter("DOB", DbType.String, eRequest.dob, false));
            parameters.Add(SqlParms.CreateParameter("LICENSE_TYPE_ID", DbType.String, licenseTypeId, false));
            parameters.Add(SqlParms.CreateParameter("ELICENSE_REQUEST", DbType.String, strRequest, false));
            parameters.Add(SqlParms.CreateParameter("ELICENSE_RESPONSE", DbType.String, strResponse, false));
            parameters.Add(SqlParms.CreateParameter("ELICENSE_VERIFIED_DATE", DbType.DateTime, DateTime.Now, false));
            parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, userid, false));
            parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, false));
            int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar("insertREG_ELICENSE_VERIFICATION", parameters));
            return lstResponseObject[0];
        }

        public static Dictionary<string, string> ActiveElicenseStatus()
        {
            Dictionary<string, string> licneseStats = new Dictionary<string, string>();
            licneseStats.Add("Board Action", "Active");
            licneseStats.Add("Probation", "Active");
            licneseStats.Add("Restricted", "Active");
            licneseStats.Add("Active", "Active");
            licneseStats.Add("Escrow", "Active");
            licneseStats.Add("Not Renewing", "Active");
            licneseStats.Add("Additional Endorsement Pending", "Active");
            licneseStats.Add("Pending Receipt of Plan", "Active");
            licneseStats.Add("Plan Under Review", "Active");

            return licneseStats;
        }

        public static Dictionary<string, string> InActiveElicenseStatus()
        {
            Dictionary<string, string> licneseStats1 = new Dictionary<string, string>();
            licneseStats1.Add("Inactive", "Inactive");
            licneseStats1.Add("Abandoned", "Inactive");
            licneseStats1.Add("Additional Endorsement Pending", "Inactive");
            licneseStats1.Add("Board Action", "Inactive");
            licneseStats1.Add("Emeritus", "Inactive");
            licneseStats1.Add("Expired", "Inactive");
            licneseStats1.Add("Failed Exam", "Inactive");
            licneseStats1.Add("Revocation", "Inactive");
            licneseStats1.Add("Surrendered", "Inactive");
            licneseStats1.Add("Suspended", "Inactive");
            licneseStats1.Add("Withdrawn", "Inactive");
            licneseStats1.Add("Escrow", "Inactive");
            licneseStats1.Add("Collections", "Inactive");
            licneseStats1.Add("Out of Business", "Inactive");
            licneseStats1.Add("Lapsed", "Inactive");
            licneseStats1.Add("Retired", "Inactive");
            licneseStats1.Add("Limited Temp Finished", "Inactive");
            licneseStats1.Add("Exempt", "Inactive");
            licneseStats1.Add("Forfeited", "Inactive");
            licneseStats1.Add("No Sub-Status Values", "Closed");
            //licneseStats1.Add("Abandoned", "Closed");
            licneseStats1.Add("Advanced", "Closed");
            licneseStats1.Add("Deceased", "Closed");
            //licneseStats1.Add("Failed Exam", "Closed");
            licneseStats1.Add("Permanent Revocation", "Closed");
            licneseStats1.Add("Permanent Denial", "Closed");
            //licneseStats1.Add("Withdrawn", "Closed");
            licneseStats1.Add("Permanent Withdrawal", "Closed");
            //licneseStats1.Add("Inactive", "Relinquished");
            return licneseStats1;
        }
    }
}
