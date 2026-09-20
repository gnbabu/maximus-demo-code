using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.Core.Libraries
{
    /// <summary>
    ///     This class provides all external notification functionality such as email.
    /// </summary>
    public class Notification
    {
        public string body { get; set; }
        protected string body1;
		public string Reg_ID { get; set; }
		protected string subject;
        public bool isEmailSent { get; set; }
        public string log_message { get; set; }
        public Dictionary<string, object> Fields = new Dictionary<string, object>();
        private Logging log = null;

        #region "Constructors"
        /// <summary>
        ///     The default parameterless constructor. Generates a new GUID for the Logging threadId
        /// </summary>
        public Notification()
        {
            // generate a new thread id GUID
            ThreadId = Guid.NewGuid();
        }

        /// <summary>
        ///     The parameterized constructor which provides the Logging threadId GUID.
        /// </summary>
        /// <param name="threadId">The GUID which is used for tying all log entires across all classes.</param>
        public Notification(Guid threadId)
        {
            ThreadId = threadId;
        }

        /// <summary>
        ///     The parameterized constructor which provides the subject.
        /// </summary>
        /// <param name="subject">subject.</param>
        public Notification(string subject)
        {
            this.subject = subject;
        }

        #endregion

        #region "Logging Objects"

        protected int logCnt = 0;
        private static Guid threadId;
        protected static Guid ThreadId
        {
            get
            {
                return threadId;
            }
            set
            {
                threadId = value;
            }
        }

        #endregion

        #region "Public Methods"

        public virtual string SendMQNotification(string templateActualPath, Dictionary<string, object> fields, bool isHTML = true)
        {
            return string.Empty;
        }
        public virtual string SendNotification(string templateActualPath, Dictionary<string, object> fields, bool isHTML = true)
        {
            return string.Empty;
        }

        /// <summary>
        ///     Sends an HTML or Non-HTML message
        /// </summary>
        /// <param name="recipients">One or more email addresses with a comma delimiter, no delimiter necessary on a single email address</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The body of the email</param>
        /// <param name="isHTML"> bool value to set if email to be sent as html format</param>
        public void SendNotification(string recipients, string subject, string body, bool isHTML)
        {
            try
            {

                MailMessage emailMessage = new MailMessage();
                emailMessage.Body = body;
                SendMessage(emailMessage, recipients, subject, isHTML, false);
                emailMessage = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        ///     Find the "tag" in the "input" text
        /// </summary>
        /// <param name="input">String input text</param>
        /// <param name="tag">String tag to search for replacement</param>
        /// RETURNS the new string with the "tag" replaced
        public bool FindTag(string input, string tag)
        {
            return (input.IndexOf("[[" + tag + "]]") != -1);
        }

        /// <summary>
        ///     Replace the "tag" in the "input" text with the "replace" data
        /// </summary>
        /// <param name="input">String input for modification</param>
        /// <param name="tag">String tag to search for replacement</param>
        /// <param name="replace">String data to substitute in place of the tag</param>
        /// RETURNS the new string with the "tag" replaced
        public string ReplaceTag(string input, string tag, string replace)
        {
            string rtn = input.Replace("[[" + tag + "]]", replace);
            return rtn;
        }

        public virtual void GetMissingEmailRelatedFeilds(Dictionary<string, object> fields)
        {
            if (!fields.ContainsKey("FROMADDRESS"))
            {
                string strFromAddress = AppSettings.Get("Mail-FromAddress", string.Empty);
                AddBookmark(fields, "FROMADDRESS", strFromAddress);
            }
            if (!fields.ContainsKey("TOADDRESS"))
            {
                AddBookmark(fields, "TOADDRESS", string.Empty);
            }

        }

        public bool SendWorkFlowEngineNotification(string regId, string bodyTemplate, string EFTFailureReason = "")
        {
            bool rtn = false;
            Reg_ID = regId;
            try
            {
                switch (bodyTemplate)
                {
                    case "EMAIL_TEMPLATE_ASSIGN_SITE_VISIT":
                        rtn = SendAssignSiteVisit(regId);
                        break;
                    case "EMAIL_TEMPLATE_PROVIDER_ERROR":
                        rtn = SendProviderError(bodyTemplate, regId);
                        break;
                    case "EMAIL_TEMPLATE_RTP_REMINDER":
                        rtn = SendProviderError(bodyTemplate, regId);
                        break;
                    case "EMAIL_TEMPLATE_RTP_REMINDER_REVAL":
                        rtn = SendProviderError(bodyTemplate, regId);
                        break;
                    case "EMAIL_TEMPLATE_ACCOUNT_ERROR":
                        rtn = SendAccountError(regId);
                        break;
                    case "EMAIL_TEMPLATE_WELCOME_PROVIDER":
                        rtn = SendProviderWelcome(regId,1);
                        break;
                    case "EMAIL_TEMPLATE_WELCOME_PROVIDER_REAPPLICATION":
                    rtn = SendProviderWelcome(regId,2);
                        break;
                    case "EMAIL_TEMPLATE_REVALIDATION_REQUEST":
                        rtn = RevalidationRequest(regId);
                        break;
                    case "EMAIL_TEMPLATE_REFER_TO_STATE":
                        rtn = SendReturnToStateNotification(regId);
                        break;
                    case "EMAIL_TEMPLATE_DENY_PROVIDER":
                        rtn = SendDenyProviderNotification(regId);
                        break;
                    //case "EMAIL_TEMPLATE_ICF_SUCCESSFUL_REVALIDATION":
                    //    SendICFSuccessfulRevalidation(regId);
                    //    rtn = true;
                    //    break;
                    case "EMAIL_TEMPLATE_ICF_INITIAL_ENROLLMENT":
                        rtn = SendICFInitialEnrollment(regId);
                        break;
                    //case "EMAIL_TEMPLATE_NF_SUCCESSFUL_REVALIDATION":
                    //    SendNFSuccessfulRevalidation(regId);
                    //    rtn = true;
                    //    break;
                    case "EMAIL_TEMPLATE_NF_INITIAL_ENROLLMENT":
                        rtn = SendICFInitialEnrollment(regId);
                        break;
                    case "EMAIL_TEMPLATE_REJECTION_OF_SELF_SUPPLIED_BACKGROUND":
                        rtn = RejectionofSelfSuppliedBackground(regId);
                        break;
                    case "EMAIL_TEMPLATE_CONFIRM_GROUP_MEMBER":
                        rtn = SendConfirmGroupMemberNotification(regId);
                        break;
                    case "EMAIL_TEMPLATE_REFER_TO_STATE_RETROEFFECTIVE_DATE":
                        rtn = SendRetroEffectiveDateNotification(regId);
                        break;
                    case "EMAIL_TEMPLATE_REFER_TO_STATE_GROUPMEMBER_RETROEFFECTIVE_DATE":
                        rtn = SendGroupMemberRetroEffectiveDateNotification(regId);
                        break;
                    case "EMAIL_TEMPLATE_PROVIDER_MORATORIA_EXCLUSION":
                        rtn = SendProviderMoratoriaExclusionNotification(regId);
                        break;
                    case "EMAIL_TEMPLATE_EXCLUSION_LETTER":
                        rtn = SendExclusionLetter(regId);
                        break;
                    case "EMAIL_TEMPLATE_FEDERAL_EXCLUSION_INTERFACE":
                        rtn = FederalExclusionInterfaces(regId);
                        break;
                    case "EMAIL_TEMPLATE_APPLICATION_FEE_WAIVER_HARDSHIP":
                        rtn = SendApplicationFeeWaiverHardshipNotification(regId);
                        break;
                    case "EMAIL_TEMPLATE_SUCCESSFUL_REVALIDATION":
                        rtn = SuccessfullRevalidation(regId);
                        break;
                    case "EMAIL_TEMPLATE_REVALIDATION_INITIAL_NOTICE":
                        rtn = RevalidationInitialNotice(regId);
                        break;

                    case "EMAIL_TEMPLATE_REVALIDATION_SECOND_NOTICE":
                        rtn = RevalidationSecondNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_REVALIDATION_THIRD_NOTICE":
                        rtn = RevalidationThirdNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_REVALIDATION_FINAL_NOTICE":
                        rtn = RevalidationFinalNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_REVALIDATION_OVERDUE_TERMINATE_NOTICE":
                        rtn = RevalidationOverDueTerminateProviderNotice(regId);
                        break;
                    case "EMAIL_Suspension_Upon_Indictment_Letter":
                        rtn = SuspensionUponIndictmentLetter(regId);
                        break;
                    case "EMAIL_TEMPLATE_REGISTRATION_DUE_NOTICE":
                        rtn = AutoClosureNotice(regId, false);
                        break;
                    case "EMAIL_TEMPLATE_REGISTRATION_RTPDUE_NOTICE":
                        rtn = AutoClosureNotice(regId, true);
                        break;
                    case "EMAIL_TEMPLATE_PROVIDER_BACKGROUND":
                        //Email template for issuing BCI Letter
                        rtn = SendProviderBackgroundCheckBCI(regId);
                        break;
                    case "EMAIL_TEMPLATE_PROVIDER_SECOND_BACKGROUND":
                        rtn = SendProviderBackgroundSecondNoticeCheck(regId);
                        break;
                    case "EMAIL_TEMPLATE_PROVIDER_BACKGROUND_10DAYS_NOTICE":
                        rtn = SendProvider10DAYBackgroundCheck(regId);
                        break;

                    case "EMAIL_TEMPLATE_PROVIDER_APPROVE_NOTIFICATION_LTC":
                        rtn = SendProviderApprovalNoticeToLTC(regId);
                        break;
                    case "EMAIL_TEMPLATE_ORIENTATION":
                        rtn = SendProviderOrientation(bodyTemplate, regId);
                        break;
                    case "EMAIL_DME_TEMPLATE_ORIENTATION":
                        rtn = SendProviderOrientation(bodyTemplate, regId);
                        break;
                    case "EMAIL_TEMPLATE_PHARMA_ORIENTATION":
                        rtn = SendProviderOrientation(bodyTemplate, regId);
                        break;
                    case "EMAIL_TEMPLATE_REFER_TO_DBH":
                        SendDBHNotification(regId);
                        rtn = true;
                        break;
                    case "EMAIL_TEMPLATE_REFER_TO_ORFA":
                        rtn = SendORFANotification(regId);
                        break;
                    case "EMAIL_TEMPLATE_LTC_NOTIFICATION":
                        rtn = SendNoticeToLTC(regId);
                        break;
                    case "EMAIL_TEMPLATE_STATE_ADMIN_SITE_VISIT_PASSED_NOTIFICATION":
                        rtn = SendDHCPSiteVisitPassedNotification(regId);
                        break;
                    case "EMAIL_TEMPLATE_PCG_SITE_VISIT":
                        rtn = SendPCGSiteVisit(regId);
                        break;
                    case "EMAIL_AWARENESS_NOTIFICATION":
                        rtn = SendAwarenessNotification(regId);
                        break;
                    case "EMAIL_RISKLEVELBUMPUP_NOTIFICATION":
                        rtn = SendAwarenessNotification(regId, "RiskLevelBumpUp.txt"); // jira 2194
                        break;
                    case "PROVIDER_10DAYS_NOTIFICATION":
                        rtn = SendProvider10DayNotice(regId);
                        break;
                    case "EMAIL_REENROLLMENT_SUCCESS_NOTIFICATION":
                        rtn = SendReEnrollmentSuccessNotice(regId);
                        break;
                    case "CHANGE_APPROVED_SERVICES_NOTIFICATION":
                        rtn = SendChangeApprovedServicesNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_LICENSE_REVALIDATION_INITIAL_NOTICE":
                        rtn = LicenseRevalidationInitialNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_LICENSE_REVALIDATION_SECOND_NOTICE":
                        rtn = LicenseRevalidationSecondNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_LICENSE_INACTIVE_TERMINATE":
                        rtn = LicenseInactiveTerminationNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_ELICENSE_TERMINATION_NOTICE":
                        rtn = LicenseTerminationNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_LICENSE_DENIAL_NOTICE":
                        rtn = LicenseDenialNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_CERTIFICATION_TERMINATION_NOTICE":
                        rtn = CertificationTerminationNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_INACTIVE_LAPSED_LICENSE_TERMINATION_NOTICE":
                        rtn = InactiveOrLapsedLicenseTerminationNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_SUBMIT_CR_NOTICE":
                        rtn = SubmitCRNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_INFORMATION_RECIEVED_NOTICE":
                        rtn = InformationRecievedNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_REPORT_SUBMISSION_NOTICE":
                        rtn = ReportSubmissionNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_REPORT_REJECTION_NOTICE":
                        rtn = ReportRejectionNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_CPC_REPORT_NOTICE":
                        rtn = CPCReportNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_NOTIFY_DDS":
                        rtn = SendNotificationToDDS(regId);
                        break;
                    case "EMAIL_APPROVED_SERVICES_NOTIFICATION":
                        rtn = SendApprovedServices(regId);
                        break;
                    //case "EMAIL_TEMPLATE_LTC_OR_DDS_NOTIFICATION":
                    //    SendNoticeToLTCOrDDS(regId);
                    //    rtn = true;
                    //    break;
                    case "EMAIL_TEMPLATE_CREDENTIAL_FAIL_ADMIN":
                        rtn = SendCredentialFailNotice(regId, 1);
                        break;
                    case "EMAIL_TEMPLATE_CREDENTIAL_FAIL_COMMITTEE":
                        rtn = SendCredentialFailNotice(regId, 2);
                        break;
                    case "EMAIL_TEMPLATE_CREDENTIAL_FAIL_RTP":
                        SendCredentialFailNotice(regId, 3);
                        rtn = true;
                        break;
                    case "EMAIL_TEMPLATE_SCREENING_AUTO_TERMINATION":
                        rtn = SendScreeningAutoTerminationNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_RISKALERT_CHOP_ACKNOWLEDGE":
                        rtn = SendRiskAlertChopAcknowledgeNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_WELCOME_CHOP_PROVIDER":
                        rtn = SendChopWelcome(regId);
                        break;
                    case "EMAIL_TEMPLATE_RISKALERT_CHOP_45DAY":
                        rtn = SendRiskAlertChopNotice(regId, "RISKALERT");
                        break;
                    case "EMAIL_TEMPLATE_RISKALERT_CHOP_DATECHANGE":
                        rtn = SendRiskAlertChopNotice(regId, "CHOPEFFECIVE");
                        break;
                    case "EMAIL_TEMPLATE_RISKALERT_CHOP_WITHDRAWN":
                        rtn = SendRiskAlertChopNotice(regId, "CHOPWITHDRAW");
                        break;
                    case "EMAIL_TEMPLATE_FREE_FORM":
                        rtn = SuccessfullRevalidation(regId);
                        break;
                    case "EMAIL_TEMPLATE_NOTIFY_AGENT":
                        rtn = SendLTCCostReportNotification(regId, "123123");
                        break;
                    case "EMAIL_TEMPLATE_SCREENING_DODD_ABUSER_REGISTRY":
                        rtn = DODDAbuserRegistryInterfaces(regId);
                        break;
                    case "EMAIL_TEMPLATE_INACTIVE_PROVIDER_TERMINATE":
                        rtn = SendInactiveProviderTerminationNotice(regId);
                        break;
                    case "CPC_INVITATION_LETTER":
                        rtn = SendCPCInvitation(regId);
                        break;

                    case "CPC_REMINDER_LETTER":
                    case "CPC_FINAL_REMINDER_LETTER":
                        rtn = SendCPCReminder(regId, bodyTemplate);
                        break;

                    case "CPC_NON_RENEWAL_NOTIFICATION_LETTER":
                        rtn = SendNonRenewalNotificationLetter(regId);
                        break;
                    case "EMAIL_TEMPLATE_WELCOME_CPCPROVIDER":
                        rtn = SendCPCWelcomeLetter(regId);
                        break;
                    case "EMAIL_TEMPLATE_CPC_ATTESTATION_ENROLLMENT_UPDATE_LETTER":
                        rtn = SendAttestationEnrollmentUpdateLetter(regId);
                        break;
                    case "EMAIL_TEMPLATE_ODA_TERMINATION":
                        rtn = SendODATerminationNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_CREDENTIAL_RTP_FIRST_REQUEST":
                        rtn = SendCredentialRTPReminderNotice(regId, 1);
                        break;
                    case "EMAIL_TEMPLATE_CREDENTIAL_RTP_SECOND_REMINDER":
                        rtn = SendCredentialRTPReminderNotice(regId, 2);
                        break;
                    case "EMAIL_TEMPLATE_CREDENTIAL_RTP_FINAL_REMINDER":
                        rtn = SendCredentialRTPReminderNotice(regId, 3);
                        break;
                    case "EMAIL_TEMPLATE_DODD_TERMINATION":
                        rtn = SendDODDTerminationNotice(regId);
                        break;

                    case "EMAIL_TEMPLATE_SCREENING_NPPES_TERMINATION":
                        rtn = NPPESInactiveTerminationLetter(regId);
                        break;

                    case "EMAIL_TEMPLATE_EFT_ERROR":
                        rtn = SendEFTError(regId, EFTFailureReason);
                        break;
                    case "EMAIL_TEMPLATE_CMC_WELCOME_PROVIDER":
                        rtn = SendCMCWelcomeLetter(regId);
                        break;
                    case "EMAIL_TEMPLATE_CMC_INVITATION":
                        rtn = SendCMCInvitational(regId);
                        break;
                    case "EMAIL_TEMPLATE_CMC_NON_RENEWAL":
                        rtn = SendCMCNonRenevalLetter(regId);
                        break;
                    case "EMAIL_TEMPLATE_CMC_REMINDER":
                        rtn = SendCMCReminder(regId);
                        break;
                    case "EMAIL_TEMPLATE_EFT_UPDATE_NOTICE":
                        rtn = SendEFTUpdateNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_NOT_PROCESSED_NOTICE":
                        rtn = NotProcessedNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_PASRR":
                        rtn = PASRRNotice(regId);
                        break;
                    case "EMAIL_TEMPLATE_PROVIDER_BACKGROUND_AND_FINGERPRINT":
                        //Email template for issuing combined BCI/FBI Letter for owners
                        rtn = SendProviderBackgroundCheckCombinedBCIAndFBI(regId);
                        break;
                    case "EMAIL_TEMPLATE_PROVIDER_FBI":
                        //Email template for issuing FBI Letter
                        rtn = SendProviderBackgroundCheck(regId);
                        break;
                    default:
                        throw new Exception(string.Format(Constants.LogString.WorkflowError,
                            "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
                            "Registration Id: " + regId.ToString() +
                            " - Email template \"" + bodyTemplate + "\" not found"));
                }
                GetMissingEmailRelatedFeilds(Fields);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ThreadId, ex);
            }
            return rtn;
        }
        private bool SendEFTError(string regId, string EFTFailureReason)
        {
            try
            {
                string templateActualPath = AppSettings.Get("SI_MQ_TemplatesPath", string.Empty);

                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;

                AddBookmark(fields, "REG_ID", regId);
                AddBookmark(fields, "PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                AddBookmark(fields, "PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                AddBookmark(fields, "ReturnCodeDescription", EFTFailureReason);
                

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    string EffectiveDate = "";
                    if (!string.IsNullOrEmpty(row["CHANGE_EFFECTIVE_DATE"].ToString()))
                    {
                        DateTime eff = Convert.ToDateTime(row["CHANGE_EFFECTIVE_DATE"].ToString());
                        EffectiveDate = eff.ToString("MM/dd/yyyy");
                    }
                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                                                                 row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                                                                 row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                                                                 (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                                                     : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), ds.Tables[0].Rows[0]["MEDICAID_ID"].ToString(), ds.Tables[0].Rows[0]["TAXONOMY_CODE"].ToString(), regId, ds.Tables[0].Rows[0]["DBA"].ToString(), EffectiveDate);
                }
                    this.Reg_ID = regId;
                body = SendMQNotification(templateActualPath + @"/EFTError.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "EFTError.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SendCostReportRejectionNotification(string regId, string comments, string TrackingNumber)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                //int toPartyId = GetAdminPartyId();
                List<SqlParameter> parms1 = new List<SqlParameter>();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms1);
                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms1);
                string medicaidID = string.Empty;
                string providertype = string.Empty;
                string name = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    providertype = Methods.GetStringValue(row, "PROVIDER_TYPE_NAME");
                    name = Methods.GetStringValue(row, "NAME");
                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), name, Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                    fields.Add("PNMURL", AppSettings.Get("PDMS-URL", string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("OhioMedicaidID", medicaidID);
                    fields.Add("TrackingNumber", TrackingNumber);
                    fields.Add("Commentsfromagent", comments);
                    body = SendNotification(templateActualPath + @"/RejectionNotice.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "RejectionNotice.txt");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, object> GetFieldValuesByRegid(int regId)
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            List<SqlParameter> parms1 = new List<SqlParameter>();
            parms1.Add(new SqlParameter("REG_ID", regId));
            DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms1);
            parms1.Clear();
            parms1.Add(new SqlParameter("REG_ID", regId));
            DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms1);
            string medicaidID = string.Empty;
            string providertype = string.Empty;
            string name = string.Empty;
            if (Methods.HasRows(dsServiceLoc))
            {
                DataRow row = dsServiceLoc.Tables[0].Rows[0];
                medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                providertype = Methods.GetStringValue(row, "PROVIDER_TYPE_NAME");
                name = Methods.GetStringValue(row, "NAME");
                GetFromAddress(fields);
                string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                    row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                    row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                    (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                        : string.Empty), name, Methods.GetStringValue(row, "NPI"), medicaidID, "", regId.ToString(), Methods.GetStringValue(row, "DBA"));
                fields.Add("PNMURL", AppSettings.Get("PDMS-URL", string.Empty));
                fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                fields.Add("OhioMedicaidID", medicaidID);
                fields.Add("TAXID", Methods.GetStringValue(row, "TAX_ID"));

                var enrollmentStatusReason = Methods.GetStringValue(row, "ENROLLMENT_STATUS_REASON");
                var effectiveDate = Methods.GetStringValue(row, "EFFECTIVE_DATE");

                fields.Add("ProviderName", ToName);
                fields.Add("CredentialRule", "5160-1-42");
                fields.Add("CredentialEmail", "Credentialing@medicaid.ohio.gov");
                fields.Add("EffectiveDate", effectiveDate);
                fields.Add("DenialReason", enrollmentStatusReason);

                DateTime endDate;
                string endDateString = "";
                if (DateTime.TryParse(Methods.GetStringValue(row, "END_DATE"), out endDate))
                    endDateString = endDate.ToString("MM-dd-yyyy");
                else
                    endDateString = "";
                fields.Add("REVALIDATIONDUE", endDateString);

                fields.Add("LEGALNAME", Methods.GetStringValue(row, "NAME"));
                fields.Add("NPI", Methods.GetStringValue(row, "NPI"));
            }
            List<SqlParameter> parms = new List<SqlParameter> { new SqlParameter("REG_ID", regId) };
            var dsLic = ExecuteStoredProcedure("usp_SelectREG_LICENSES", parms);
            if (Methods.HasRows(dsLic))
            {
                var row = dsLic.Tables[0].Rows[0];
                fields.Add("LICENSESTATE", Methods.GetStringValue(row, "LICENSE_STATE"));
                fields.Add("LICENSENUMBER", Methods.GetStringValue(row, "LICENSE_NUMBER"));
                fields.Add("LICENSEENDDATE", Methods.GetStringValue(row, "LICENSE_END_DATE"));
            }
            var emailAddress = string.Empty;
            parms = new List<SqlParameter>
            {
                new SqlParameter("SendToTypeID", 1),
                new SqlParameter("RegId", regId)
            };
            var dsEmail = ExecuteStoredProcedure("usp_SelectEmailRecipients", parms);
            if (Methods.HasRows(dsEmail))
            {
                foreach (DataRow row in dsEmail.Tables[0].Rows)
                {
                    emailAddress = Methods.GetStringValue(row, "EmailAddress");
                    if (!String.IsNullOrEmpty(emailAddress)) break;
                }
                fields.Add("EMAILADDRESS", emailAddress);
            }
            return fields;
        }
        private bool SendRiskAlertChopAcknowledgeNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();


                //   int toPartyId = GetAdminPartyId();

                List<SqlParameter> parms1 = new List<SqlParameter>();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms1);

                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms1);
                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsrisk = ExecuteStoredProcedure("usp_SelectREG_LTC_RISK_ALERT", parms1);
                string medicaidID = string.Empty;
                string enteringprovider = string.Empty;
                string ownercostreport = string.Empty;
                string chopeffective = string.Empty;
                string providertype = string.Empty;
                string name = string.Empty;
                string riskAlertDate = string.Empty;
                GetFromAddress(fields);
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    providertype = Methods.GetStringValue(row, "PROVIDER_TYPE_NAME");
                    name = Methods.GetStringValue(row, "NAME");
                    if (Methods.HasRows(dsrisk))
                    {
                        DataRow drAlert = dsrisk.Tables[0].Rows[0];
                        enteringprovider = Methods.GetStringValue(drAlert, "Entering_Provider_Name");
                        ownercostreport = Methods.GetStringValue(drAlert, "owner_name_from_cost_report");
                        chopeffective = Methods.GetStringValue(drAlert, "Chop_effective_date");
                        riskAlertDate = Methods.GetStringValue(drAlert, "Risk_Alert_Date");

                        DateTime dDate;
                        DateTime rDate;

                        if (!string.IsNullOrEmpty(chopeffective) && DateTime.TryParse(chopeffective, out dDate))
                        {
                            chopeffective = Convert.ToDateTime(chopeffective).ToString("MM/dd/yyyy");
                        }
                        if (!string.IsNullOrEmpty(riskAlertDate) && DateTime.TryParse(riskAlertDate, out rDate))
                        {
                            riskAlertDate = Convert.ToDateTime(riskAlertDate).ToString("MM/dd/yyyy");
                        }
                    }


                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    string Dba = row["DBA"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), name, Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));


                    fields.Add("PNMURL", AppSettings.Get("PDMS-URL", string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("EXITINGMEDICAIDID", medicaidID);
                    fields.Add("ProviderName", ToName);
                    fields.Add("EXITINGNPI", Methods.GetStringValue(row, "NPI"));
                    fields.Add("EXITINGFACILITYNAME", name);
                    fields.Add("FACILITYNAME", Dba);
                    fields.Add("EXITINGOPERATOR", name);
                    fields.Add("ENTERINGOPERATORNAME", enteringprovider);
                    fields.Add("OWNERFROMCOSTREPORT", ownercostreport);
                    fields.Add("CHOPEFFECTIVEDATE", chopeffective);
                    fields.Add("RISKALERTDATE", riskAlertDate);

                    string recipients = GetRecipients(Constants.SendToTypeID.CHOPEmailGroupAndEnteringProvider, Convert.ToInt32(regId));
                    subject = subject.Replace("[[Provider Type]]", providertype.Trim());

                    EMailNotification enotify = new EMailNotification(body, subject, recipients, ThreadId);
                    if (providertype.Trim() == CON.ProviderType.NURSING_FACILITY)
                    {
                        body = SendNotification(templateActualPath + @"/NFCHOPAcknowledgment.txt", fields, true);
                        //body = enotify.SendActualNotification(templateActualPath + "//NFCHOPAcknowledgment.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "NFCHOPAcknowledgment.txt");
                    }

                    else if (providertype.Trim() == CON.ProviderType.NON_STATE_OPERATED_ICF_MR)
                    {

                        body = SendNotification(templateActualPath + @"/ICFCHOPAcknowledgment.txt", fields, true);
                        //body = enotify.SendActualNotification(templateActualPath + "//ICFCHOPAcknowledgment.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "ICFCHOPAcknowledgment.txt");
                    }

                }
                else
                {
                    return false;
                }
                Fields = fields;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendChopWelcome(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms1 = new List<SqlParameter>();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms1);

                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsChop = ExecuteStoredProcedure("usp_SelectREG_CHOP_DATA", parms1);
                GetFromAddress(fields);
                string enteringMedicaidID = string.Empty;
                string enteringName = string.Empty;
                string enteringDBA = string.Empty;
                string exitingName = string.Empty;
                string exitingMedicaidID = string.Empty;
                string facilityName = string.Empty;
                string chopEffectiveDate = string.Empty;
                string providerTypeName = string.Empty;

                if (dsChop != null && dsChop.Tables.Count > 0 && dsChop.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsChop.Tables[0].Rows[0];

                    enteringMedicaidID = Methods.GetStringValue(row, "ENTERINGMEDICAIDID");
                    enteringName = Methods.GetStringValue(row, "ENDTERINGNAME");
                    enteringDBA = Methods.GetStringValue(row, "ENTERINGDBA");
                    exitingName = Methods.GetStringValue(row, "EXITINGNAME");
                    exitingMedicaidID = Methods.GetStringValue(row, "EXITINGMEDICAIDID");
                    facilityName = Methods.GetStringValue(row, "FACILITYNAME");
                    chopEffectiveDate = Methods.GetStringValue(row, "Chop_Effective_Date");
                    DateTime CEdate;
                    providerTypeName = Methods.GetStringValue(row, "PROVIDER_TYPE_NAME");
                    if (!string.IsNullOrEmpty(chopEffectiveDate) && DateTime.TryParse(chopEffectiveDate, out CEdate))
                    {
                        chopEffectiveDate = Convert.ToDateTime(chopEffectiveDate).ToString("MM/dd/yyyy");
                    }
                    fields.Add("EXITINGFACILITYNAME", exitingName);
                    fields.Add("EXITINGMEDICAIDID", exitingMedicaidID);
                    fields.Add("ENTERINGFACILITYNAME", enteringName);
                    fields.Add("ENTERINMEDICAIDID", enteringMedicaidID);
                    fields.Add("ENTERINGDBANAME", enteringDBA);
                    fields.Add("CHOPEFFECTIVEDATE", chopEffectiveDate);
                }
                else
                {
                    return false;
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    string name = Methods.GetStringValue(row, "NAME");

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                    row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                    row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                    (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                        : string.Empty), name, Methods.GetStringValue(row, "NPI"), enteringMedicaidID, "", regId, Methods.GetStringValue(row, "DBA"));

                    fields.Add("PNMURL", AppSettings.Get("PDMS-URL", string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("ProviderName", ToName);

                    if (providerTypeName.Trim() == CON.ProviderType.NURSING_FACILITY)
                    {
                        body = SendNotification(templateActualPath + "//NFCHOPEnrollment.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "NFCHOPEnrollment.txt");
                    }

                    else if (providerTypeName.Trim() == CON.ProviderType.NON_STATE_OPERATED_ICF_MR)
                    {
                        body = SendNotification(templateActualPath + "//ICFCHOPEnrollment.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "ICFCHOPEnrollment.txt");
                    }
                }
                else
                {
                    return false;
                }
                Fields = fields;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendRiskAlertChopNotice(string regId, string NoticeType)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                // int toPartyId = GetAdminPartyId();

                List<SqlParameter> parms1 = new List<SqlParameter>();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms1);

                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms1);
                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsrisk = ExecuteStoredProcedure("usp_SelectREG_LTC_RISK_ALERT", parms1);
                string medicaidID = string.Empty;
                string enteringprovider = string.Empty;
                string ownercostreport = string.Empty;
                string chopeffective = string.Empty;
                string chopWithdrawn = string.Empty;
                string providertype = string.Empty;
                string name = string.Empty;

                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    providertype = Methods.GetStringValue(row, "PROVIDER_TYPE_NAME");
                    name = Methods.GetStringValue(row, "NAME");
                    if (Methods.HasRows(dsrisk))
                    {
                        DataRow drAlert = dsrisk.Tables[0].Rows[0];

                        DateTime chopeffective1 = Convert.ToDateTime(Methods.GetStringValue(drAlert, "Chop_Effective_Date"));
                        chopeffective = chopeffective1.ToString("MM/dd/yyyy");
                        DateTime chopWithdrawn1 = Convert.ToDateTime(Methods.GetStringValue(drAlert, "Risk_Alert_Status_End_Date"));
                        chopWithdrawn = chopWithdrawn1.ToString("MM/dd/yyyy");
                    }

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), name, Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));

                    fields.Add("PNMURL", AppSettings.Get("PDMS-URL", string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("MEDICAID", medicaidID);
                    fields.Add("Name", name);
                    fields.Add("NPI", Methods.GetStringValue(row, "NPI"));


                    if (NoticeType == "CHOPWITHDRAW")
                    {
                        fields.Add("DATECHOPWITHDRAWN", chopWithdrawn);
                        body = SendNotification(templateActualPath + @"/CHOPWithdrawn.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "CHOPWithdrawn.txt");
                    }

                    else if (NoticeType == "CHOPEFFECIVE")
                    {
                        fields.Add("CHOPEFFECTIVEDATE", chopeffective);
                        string recipients = GetRecipients(Constants.SendToTypeID.CHOPEmailGroup, Convert.ToInt32(regId));
                        EMailNotification enotify = new EMailNotification(body, subject, recipients, ThreadId);
                        body = enotify.SendActualNotification(templateActualPath + @"/CHOPDateChange.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "CHOPDateChange.txt");
                    }
                    else if (NoticeType == "RISKALERT")
                    {
                        fields.Add("CHOPEFFECTIVEDATE", chopeffective);
                        string recipients = GetRecipients(Constants.SendToTypeID.CHOPEmailGroup, Convert.ToInt32(regId));

                        EMailNotification enotify = new EMailNotification(body, subject, recipients, ThreadId);
                        //body = SendNotification(templateActualPath + @"/RiskAlertNotice.txt", fields, true);
                        body = enotify.SendActualNotification(templateActualPath + "//RiskAlertNotice.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "RiskAlertNotice.txt");
                    }
                    return true;
                }
                Fields = fields;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SendLTCCostReportNotification(string regId, string TrackingNumber)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                //int toPartyId = GetAdminPartyId();
                List<SqlParameter> parms1 = new List<SqlParameter>();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms1);
                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms1);
                string medicaidID = string.Empty;
                string providertype = string.Empty;
                string name = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    providertype = Methods.GetStringValue(row, "PROVIDER_TYPE_NAME");
                    name = Methods.GetStringValue(row, "NAME");
                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), name, Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                    fields.Add("PNMURL", AppSettings.Get("PDMS-URL", string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("OhioMedicaidID", medicaidID);
                    fields.Add("TrackingNumber", TrackingNumber);
                    body = SendNotification(templateActualPath + @"/LTCCOSTREPORT.txt", fields, true);
                    string recipients = GetRecipients(Constants.SendToTypeID.Provider, Convert.ToInt32(regId));

                    subject = "";
                    EMailNotification enotify = new EMailNotification(body, subject, recipients, ThreadId);
                    body = enotify.SendActualNotification(templateActualPath + "//LTCCOSTREPORT.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "LTCCOSTREPORT.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendCredentialRTPReminderNotice(string regId, int NoticeType)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                //int toPartyId = GetAdminPartyId();
                if (hasRows)
                {
                    //int partyId = Methods.GetIntValue(dsProv.Tables[0].Rows[0]["PARTY_ID"]);
                    //if (partyId > 0) toPartyId = partyId;
                    DataRow row = dsProv.Tables[0].Rows[0];
                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), "", "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }

                string reqNumber = NoticeType == 1 ? "Initial Request" : NoticeType == 2 ? "Second Request" : "Final Request";
                fields.Add("RETURNREASONS", GetReturnReasonsHTML(regId, true));
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                fields.Add("RequestNumber", reqNumber);

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(new SqlParameter("RegID", regId));
                DataSet dsReg = ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms);
                DataRow drReg = dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0 ? dsReg.Tables[0].Rows[0] : null;
                int WorkflowID = Convert.ToInt32(drReg["WorkflowID"]);
                int workFlowEventId = Convert.ToInt32(drReg["WORKFLOW_EVENT_TYPE_ID"]);

                sqlParms = new List<SqlParameter>();
                sqlParms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsCred = ExecuteStoredProcedure("usp_SelectPROVIDER_Credentialing", sqlParms);

                string credName = dsCred.Tables[0].Rows.Count > 1 ? "Recredentialing" : "Credentialing";
                fields.Add("CredName", credName);

                if (NoticeType == 3)
                {
                    string credFinalText = credName == "Recredentialing" ? "Failure to do so within 3 business days of this communication will result in the termination of your provider agreement." : "Failure to do so within 3 business days of this communication will result in the closure of the application.";
                    fields.Add("CredFinalText", credFinalText);
                }

                body = SendNotification(templateActualPath + @"/CredentialRTPReminderNotice.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "CredentialRTPReminderNotice.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendCredentialFailNotice(string regId, int NoticeType)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                //int toPartyId = GetAdminPartyId();

                List<SqlParameter> parms1 = new List<SqlParameter>();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms1);

                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms1);
                string medicaidID = string.Empty;
                string effectiveDate = string.Empty;
                string enrollmentStatusReason = string.Empty;

                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (dsProv != null && dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsProv.Tables[0].Rows[0];

                    enrollmentStatusReason = Methods.GetStringValue(row, "ENROLLMENT_STATUS_REASON");
                    effectiveDate = Methods.GetStringValue(row, "EFFECTIVE_DATE");

                    DateTime dDate;

                    if (!string.IsNullOrEmpty(effectiveDate) && DateTime.TryParse(effectiveDate, out dDate))
                    {
                        effectiveDate = Convert.ToDateTime(effectiveDate).ToString("MM/dd/yyyy");
                    }

                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, "", regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"));


                    fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("MedicaidID", medicaidID);
                    fields.Add("ProviderName", ToName);
                    fields.Add("CredentialRule", "5160-1-42");
                    fields.Add("CredentialRuleK4", "5160-1-42 (K)(4)");
                    fields.Add("CredentialEmail", "Credentialing@medicaid.ohio.gov");
                    fields.Add("EffectiveDate", effectiveDate);
                    //fields.Add("DenialReason", enrollmentStatusReason);

                    if (NoticeType == 1)
                    {
                        body = SendNotification(templateActualPath + @"/CredentialReviewFailNotice.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "CredentialReviewFailNotice.txt");
                    }
                    else if (NoticeType == 2)
                    {
                        parms1.Clear();
                        parms1.Add(new SqlParameter("REG_ID", regId));
                        DataSet dsCredential = ExecuteStoredProcedure("usp_SelectREG_Credentialing", parms1);
                        string committeeSummary = string.Empty;
                        string denialReason = string.Empty;
                        if (Methods.HasRows(dsCredential))
                        {
                            DataRow row1 = dsCredential.Tables[0].Rows[0];
                            committeeSummary = Methods.GetStringValue(row1, "COMMITTEE_SUMMARY");
                            denialReason = Methods.GetStringValue(row1, "COMMITTEE_DENIAL_TERM_REASON");
                        }

                        fields.Add("DenialReason", denialReason);
                        fields.Add("COMM_SUMMARY", committeeSummary);
                        body = SendNotification(templateActualPath + @"/CredentialCommitteeTermNotice.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "CredentialCommitteeTermNotice.txt");
                    }
                    else if (NoticeType == 3)
                    {
                        fields.Add("CredentialingRTPReasons", GetReturnReasonsHTML(regId));
                        body = SendNotification(templateActualPath + @"/CredentailRTPAutoTerminateNotice.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "CredentailRTPAutoTerminateNotice.txt");
                    }
                    return true;
                }
                Fields = fields;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
        private bool SendEFTUpdateNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                string ToName = string.Empty;
                if (hasRows)
                {
                    DataRow row = dsProv.Tables[0].Rows[0];
                    GetFromAddress(fields);
                    ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), "", "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }
                
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                fields.Add("ProviderName", ToName);

                body = SendNotification(templateActualPath + @"/EFTInformationUpdate.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "EFTInformationUpdate.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region "Private Methods"

        /// <summary>
        ///     This internal method contains the core functionality for sending emails
        /// </summary>
        /// <param name="emailMessage">The email message to use for sending email</param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="isHTML"></param>
        /// <param name="sendSSL"></param>
        /// <param name="bypassTestEmail">This parameter should never be sent as true. Whenever true is passed it must have approval 
        ///     of the Technical Manager (as of 9/27/2012 - Akhilesh Singh)</param>
        private void SendMessage(MailMessage emailMessage, string to, string subject, bool isHTML, bool sendSSL
            , bool bypassTestEmail = false, string cc = "", string bcc = "")
        {

            const char delimiter1 = ',';
            const char delimiter2 = ';';
            string testing = bool.TrueString.ToLower();

            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(threadId, logMsg);

            try
            {
                string emailSubject = ConstructSubject(subject);

                bool testEmail = false;
                // if testing enabled
                testing = AppSettings.Get("TestEmailEnabled", bool.TrueString.ToLower());
                if (testing.ToLower() == bool.TrueString.ToLower() && bypassTestEmail == false)
                {
                    string logTestingMessage = "Test email address(es) being substituted for:" + to;
                    logTestingMessage = String.Format(logTestingMessage, emailSubject, to);
                    log.CreateLogEntry(logTestingMessage, +logCnt);
                    to = AppSettings.Get("TestEmailAddress", "OHPNMCodeJunkies@maximus.com");
                    testEmail = true;
                }
                else
                {
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                }

                // add the To email address(es)
                char delimiter = delimiter1;
                if (to.IndexOf(delimiter1) == -1)
                {
                    delimiter = delimiter2;
                }
                string[] Addrs = to.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string adr in Addrs)
                {
                    emailMessage.To.Add(new MailAddress(adr));
                }

                if (!testEmail)                                         // Only add them if we are NOT testing
                {

                    // add the CC email address(es)
                    if (!string.IsNullOrWhiteSpace(cc))
                    {
                        delimiter = delimiter1;
                        if (cc.IndexOf(delimiter1) == -1)
                        {
                            delimiter = delimiter2;
                        }
                        string[] bccAddrs = cc.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string adr in bccAddrs)
                        {
                            emailMessage.CC.Add(new MailAddress(adr));
                        }
                    }

                    // add the BCC email address(es)
                    string bccEmail = AppSettings.Get("SmtpBCC", string.Empty);
                    bccEmail = bccEmail + bcc;
                    if (!string.IsNullOrWhiteSpace(bccEmail))
                    {
                        delimiter = delimiter1;
                        if (bccEmail.IndexOf(delimiter1) == -1)
                        {
                            delimiter = delimiter2;
                        }
                        string[] bccAddrs = bccEmail.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string adr in bccAddrs)
                        {
                            emailMessage.Bcc.Add(new MailAddress(adr));
                        }
                    }
                }

                string replyToEmail = AppSettings.Get("SmtpReplyTo", string.Empty);
                string emailfromAddress = AppSettings.Get("SmtpFromEmailAddress", string.Empty);

                if (!string.IsNullOrEmpty(replyToEmail))
                {
                    emailMessage.ReplyToList.Add(replyToEmail);
                }
                if (!string.IsNullOrEmpty(emailfromAddress))
                {
                    emailMessage.From = new MailAddress(emailfromAddress);
                }

                emailMessage.Subject = emailSubject;
                emailMessage.IsBodyHtml = isHTML;

                SmtpClient smtpclient;
                smtpclient = new SmtpClient();

                smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (!string.IsNullOrEmpty(AppSettings.Get("SmtpClientPortNumber")))
                {
                    smtpclient.Port = (int)Methods.GetIntValue(AppSettings.Get("SmtpClientPortNumber"), false);
                }

                smtpclient.Host = AppSettings.Get("SmtpClientHost");
                smtpclient.UseDefaultCredentials = Convert.ToBoolean(AppSettings.Get("SmtpUseDefaultCredentials", bool.FalseString.ToLower()));
                smtpclient.EnableSsl = sendSSL;

                // send it
                smtpclient.Send(emailMessage);

                string logMessage = "Email sent successfully [Subject: {0} :: To: {1}]";
                logMessage = String.Format(logMessage, emailSubject, to);
                log.CreateLogEntry(logMessage, +logCnt);
            }
            catch (Exception ex)
            {
                string msg = String.Format("Email not sent, Reason [{0}]", ex.Message);
                log.CreateLogEntry(msg, Logging.LogPriority.Error, +logCnt);
            }
        }

        /// <summary>
        ///     Given the subject, checks if the "SubjectWithEnvironment" settings is TRUE.  If it is, appends the "Environment" setting (if it exists)
        ///     to the end of the subject.
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        private string ConstructSubject(string subject)
        {
            string rtn = subject;
            string testing = AppSettings.Get("SubjectWithEnvironment", bool.TrueString.ToLower());
            if (testing.ToLower() == bool.TrueString.ToLower() &&
                !string.IsNullOrEmpty(AppSettings.Get("Environment", string.Empty)))
            {
                // If not already in subject add it
                string append = "[" + AppSettings.Get("Environment", string.Empty) + "]";
                if (subject.IndexOf(append) == -1)
                {
                    rtn += " " + append;
                }
            }
            return rtn;
        }

        public string SendSMS(string Body, string regId)
        {
            string resultMsg = "";
            try
            {
                bool CanSMS = false;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("REG_ID", regId));
                parameters.Add(new SqlParameter("ADDRESS_TYPE_ID", "8"));
                DataSet ds;
                ds = ExecuteStoredProcedure("usp_SelectProviderAddressInfo", parameters);
                SmsInfo sms = new SmsInfo();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Boolean canText = false;
                    Boolean.TryParse(ds.Tables[0].Rows[i]["CAN_TEXT1"].ToString(), out canText);
                    if ((canText && ds.Tables[0].Rows[i]["PHONE1"] != null))
                    {
                        sms.mobile_number = ds.Tables[0].Rows[i]["PHONE1"].ToString();
                        CanSMS = true;
                    }
                    else
                    {
                        Boolean.TryParse(ds.Tables[0].Rows[i]["CAN_TEXT2"].ToString(), out canText);
                        if ((canText && ds.Tables[0].Rows[i]["PHONE2"] != null))
                        {
                            sms.mobile_number = ds.Tables[0].Rows[i]["PHONE2"].ToString();
                            CanSMS = true;
                        }
                    }
                }

                if (CanSMS)
                {
                    bool testSMSenabled = true;
                    try
                    {
                        if (string.IsNullOrEmpty(subject)) subject = "Text Message";
                        sms.message = Body;

                        //get appsettings test mobile number in test environment.
                        testSMSenabled = Convert.ToBoolean(AppSettings.Get("TestSMSEnabled", bool.TrueString.ToLower()));
                        if (testSMSenabled)
                        {
                            sms.mobile_number = AppSettings.Get("TestSMSMobileNumber", "6145357488");
                            testSMSenabled = true;
                        }


                        SmsResult result = new SmsResult();
                        result = Methods.SMSAsync(sms).Result;
                        if (result.success)
                        {
                            resultMsg = "SUCCESS: ";
                        }
                        else
                        {
                            resultMsg = "ERROR: ";

                        }
                        resultMsg += result.message;
                    }
                    catch (Exception ex)
                    {
                        resultMsg = "ERROR: " + Methods.FullExceptionMessage(ex);
                    }
                    finally
                    {
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(new SqlParameter("REG_ID", regId));
                        param.Add(new SqlParameter("BODY", sms.message));
                        param.Add(new SqlParameter("SUBJECT", subject));
                        param.Add(new SqlParameter("PHONE", sms.mobile_number));
                        param.Add(new SqlParameter("RESULT", resultMsg));
                        Guid updateUser = Methods.GetCurrentUserId();
                        if (updateUser == Guid.Empty) updateUser = Guid.Parse(Constants.appPDMSDataExchangeUserId);
                        param.Add(new SqlParameter("UpdateUser", updateUser));
                        ExecuteStoredProcedure("sp_insertSMS", param);
                    }
                }

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ThreadId, ex);
            }
            return resultMsg;
        }

        private bool SendAccountError(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    fields.Add("RETURNREASONs", GetReturnReasonsHTML(regId));
                    fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                    body = SendNotification(templateActualPath + @"/AccountError.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "AccountError.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void CreateCommunicationEvent(Dictionary<string, object> fields, Guid userId, string regId, string templateName)
        {
            Fields = fields;
        }


        protected void UpdateCommunicationEventForParty(int RegID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId));
            string comEventID = ExecuteScalar("sp_SelectLastCommunicationEventID", parameters);

            // generated by sp_Admin_StoredProcBuilder on Oct  3 2012  2:44PM
            // create parameters objects and fill with values
            parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("REG_ID", RegID));
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_ID", comEventID));
            parameters.Add(new SqlParameter("RESOLVING_ACTION_TYPE_ID", Constants.ResolvingActionType.SystemGeneratedEmail));
            parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId));

            ExecuteStoredProcedure("sp_UpdateCommunicationEventIdsForPartyErrorHistory", parameters);
        }

        private bool SendProviderError(string bodyTemplate, string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);               
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);               

                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                //int toPartyId = GetAdminPartyId();
                if (hasRows)
                {
                    //int partyId = Methods.GetIntValue(dsProv.Tables[0].Rows[0]["PARTY_ID"]);
                    //if (partyId > 0) toPartyId = partyId;
                    DataRow row = dsProv.Tables[0].Rows[0];
                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), "", "", regId, Methods.GetStringValue(row, "DBA"));


                    // SAM505
                    int processID = 0;
                    bool isAddBCIText = false;
                    parms = new List<SqlParameter>();
                    parms.Add(new SqlParameter("REG_ID", regId));
                    DataSet dsBCI = ExecuteStoredProcedure("usp_CheckIfAddBCIText_RTPEmail", parms);
                    if (dsBCI.Tables.Count > 0 && dsBCI.Tables[0].Rows.Count > 0)
                    {
                        processID = Convert.ToInt32(dsBCI.Tables[0].Rows[0]["PROCESS_ID"]);
                        isAddBCIText = dsBCI.Tables[0].Rows[0]["ADD_BCI_TEXT"].ToString() == CON.BCITextRTPEmail.ToSend;
                    }                   

                    if (isAddBCIText)
                    {
                        string textBCI = "<p>A BCI check is required if you have been a resident of Ohio for at least 5 years. If you have not been a resident for the last 5 years, a BCI and FBI is required. Visit http://www.ohioattorneygeneral.gov to find a list of webcheck location. Inform the webcheck vendor that the Bureau of Criminal Identification and Investigation (BCI&I) must mail your background results via United States Postal Service directly to The Ohio Department of Medicaid, Attn: BCI Coordinator, P.O. Box 183017, Columbus, Ohio 43218. Inform the webcheck vendor that the reason you are being finger printed is reason code 5164.341 for BCI and 3701.881 for FBI checks.</p><br />";
                        fields.Add("BCIText", textBCI);

                        parms = new List<SqlParameter>();
                        parms.Add(new SqlParameter("ProcessID", processID));
                        parms.Add(new SqlParameter("ParameterName", CON.ProcessParameter.IsAddBCIRTPEmail));
                        parms.Add(new SqlParameter("ParameterValue", CON.BCITextRTPEmail.Sent));  // 1 - Send BCI TEXT in this WF, 2 - BCI Text already sent in this workflow                      
                        ExecuteStoredProcedure("usp_WF_SaveProcessParameter", parms);
                    }
                }
                else
                {
                    return false;
                }
                fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                fields.Add("RETURNREASONS", GetReturnReasonsHTML(regId));
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                AddBookmark(fields, "PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));

                if (bodyTemplate == "EMAIL_TEMPLATE_RTP_REMINDER")
                {
                    body = SendNotification(templateActualPath + @"/ReturnToProviderReminder.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ReturnToProviderReminder.txt");
                }
                else if (bodyTemplate == "EMAIL_TEMPLATE_RTP_REMINDER_REVAL") 
                {
                    body = SendNotification(templateActualPath + @"/ReturnToProviderReminderReval.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ReturnToProviderReminderReval.txt");
                }
                else
                {
                    body = SendNotification(templateActualPath + @"/ProviderError.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ProviderError.txt");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Add the notes that belong to the specific Page Type Id
        private void AddNotesHTML(int regPageTypeId, DataTable dtNotes, ref StringBuilder sb)
        {
            if (dtNotes == null) return;

            foreach (DataRow row in dtNotes.Rows)
            {
                if (regPageTypeId == Convert.ToInt32(row["REG_PAGE_TYPE_ID"]))
                {
                    sb.Append("<br>");
                    sb.Append("&nbsp;&nbsp;&nbsp;-&nbsp;" + row["NOTE_TEXT"].ToString());
                }
            }
        }

        private string GetReturnReasonsHTML(string regId, bool forCredentialingRTP = false)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("REG_ID", regId));
            parameters.Add(new SqlParameter("REG_PAGE_TYPE_ID", -999));
            DataSet ds = new DataSet();
            ds = ExecuteStoredProcedure("usp_SelectREG_ERRORcustom", parameters);
            if (ds.Tables.Count == 0)
                return "There are no return reasons saved.";
            else if (ds.Tables[0].Rows.Count == 0)
                return "There are no return reasons saved.";
            else
            {
                int regPageTypeId = 0;
                StringBuilder sb = new StringBuilder();
                DataTable dtInput = ds.Tables[0];
                DataTable dt = dtInput.DefaultView.ToTable(true, new string[3] { "ERROR_CODE", "ERROR_NAME", "REG_PAGE_TYPE_ID" });
                // Go get the notes
                parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("REG_ID", regId));
                ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER_NOTECustom", parameters);
                DataTable dtNotes = null;
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    int ProviderNoteTypeId = forCredentialingRTP ? Constants.ProviderNoteTypeId.ReturnToProviderCredentialing : Constants.ProviderNoteTypeId.RegistrationRejection;
                    StringBuilder selectPart = new StringBuilder();
                    selectPart.Append(string.Format("PROVIDER_NOTE_TYPE_ID = '{0}'", ProviderNoteTypeId));

                    if (ds.Tables[1].Select(selectPart.ToString()).Count() > 0)
                    {
                        dtNotes = ds.Tables[1].Select(selectPart.ToString()).CopyToDataTable();
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (regPageTypeId != Convert.ToInt32(dt.Rows[i]["REG_PAGE_TYPE_ID"]))
                    {
                        regPageTypeId = Convert.ToInt32(dt.Rows[i]["REG_PAGE_TYPE_ID"]);
                        if (dtNotes != null)
                            AddNotesHTML(regPageTypeId, dtNotes, ref sb);                       
                    }

                    if (i > 0) sb.Append("<br>");
                    sb.Append(dt.Rows[i]["ERROR_CODE"].ToString() + " - " + dt.Rows[i]["ERROR_NAME"].ToString());
                }

                if (dtNotes != null)
                    AddNotesHTML(regPageTypeId, dtNotes, ref sb);

                // return sb.ToString();

                //insert it into array
                string input = sb.ToString();
                input = input.Replace("<br>", "*");
                string[] inputArray = input.Split('*');
                //Select distinct array
                string[] distinctInputArray = inputArray.Distinct().ToArray();
                StringBuilder Result = new StringBuilder();
                foreach (string s in distinctInputArray)
                {
                    Result.Append(s);
                    Result.Append("*");
                }
                //and this line to remove extra ","
                Result.Remove(Result.Length - 1, 1);
                Result = Result.Replace("*", "<br>");

                return Result.ToString();

            }
        }

        private void AddBookmark(Dictionary<string, object> fields, string bookmarkName, string value)
        {
            if (string.IsNullOrEmpty(value)) fields.Add(bookmarkName, string.Empty);
            else fields.Add(bookmarkName, value);
        }

        public virtual void GetFromAddress(Dictionary<string, object> fields)
        {
            string strFromAddress = AppSettings.Get("Mail-FromAddress", string.Empty);
            AddBookmark(fields, "FROMADDRESS", strFromAddress);
        }

        public void GetToAddress(Dictionary<string, object> fields, string ContactName, string Quadrant, string Address1, string Address2, string CityStateZip, string ProviderName = "", string NPI = "", string MedicaidID = "", string Taxonomy = "", string RegID = "", string DBA = "", string EFFECTIVEDATE = "", string DateType = "")
        {
            string strToaddress = string.Empty;
            string mailpiece = string.Empty;

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
            bool RegistrationIDflag = false;
            bool DBAflag = false;

            if (!string.IsNullOrEmpty(Taxonomy.ToString()))
            {
                Taxonomy = "Taxonomy: " + Taxonomy;
            }
            else
            {
                Taxonomy = "";
            }
            if (!string.IsNullOrWhiteSpace(RegID.ToString()))
            {
                RegID = "Registration ID: " + RegID;
            }
            else
            {
                RegID = "";
            }
            if (!string.IsNullOrWhiteSpace(DBA.ToString()))
            {
                DBA = "DBA: " + DBA;
            }
            else
            {
                DBA = "";
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
                //  strToaddress += "<table style=\"font-family:'Arial Narrow';font-size:14pt;width:100%;margin:0px;cellpadding: 0px;cellspacing: 0px\">";
                strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.65in;\">" + ContactName + "</td>";
                strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt;padding-left:0.65in;\">" + ProviderName + "</td></tr>";
                strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.65in;\">" + Address1 + "</td>";
                strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + NPI + "</td></tr>";
                if (Address2.Trim() != "")
                {
                    strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.65in;\">" + Address2 + "</td>";
                    if (DBA != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + DBA + "</td></tr>";
                        DBAflag = true;
                    }
                    else if (MedicaidID != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + MedicaidID + "</td></tr>";
                        Medicaidflag = true;
                    }
                    else if (Taxonomy != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + Taxonomy + "</td></tr>";
                        Taxonomyflag = true;

                    }
                    else if (EFFECTIVEDATE != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + EFFECTIVEDATE + "</td></tr>";
                        Effectivedateflag = true;

                    }
                    else if (RegID != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + RegID + "</td></tr>";
                        RegistrationIDflag = true;
                    }
                    else
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\"></td></tr>";
                    }

                }

                strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.65in;\">" + CityStateZip + "</td>";
                if (Address2.Trim() == "")
                {
                    if (DBA != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + DBA + "</td></tr>";
                        DBAflag = true;
                    }
                    else if (MedicaidID != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + MedicaidID + "</td></tr>";
                        Medicaidflag = true;

                    }
                    else if (Taxonomy != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + Taxonomy + "</td></tr>";

                        Taxonomyflag = true;

                    }
                    else if (EFFECTIVEDATE != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + EFFECTIVEDATE + "</td></tr>";
                        Effectivedateflag = true;
                    }
                    else if (RegID != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + RegID + "</td></tr>";
                        RegistrationIDflag = true;
                    }
                    else
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\"></td></tr>";
                    }
                }
                else
                {
                    if (DBA != "" && !(DBAflag))
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + DBA + "</td></tr>";
                        DBAflag = true;
                    }
                    else if (MedicaidID != "" && !(Medicaidflag))
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + MedicaidID + "</td></tr>";
                    }
                    else if (Taxonomy != "" && !(Taxonomyflag))
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + Taxonomy + "</td></tr>";
                        Taxonomyflag = true;
                    }
                    else if (EFFECTIVEDATE != "" && !(Effectivedateflag))
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + EFFECTIVEDATE + "</td></tr>";
                        Effectivedateflag = true;

                    }
                    else if (RegID != "" && !(RegistrationIDflag))
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + RegID + "</td></tr>";
                        RegistrationIDflag = true;
                    }
                    else
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\"></td></tr>";
                    }
                }

                if (DBA != "" && !(DBAflag))
                {
                    strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt\"></td>";
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + DBA + "</td></tr>";
                }

                if (Taxonomy != "" && !(Taxonomyflag))
                {
                    strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt\"></td>";
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + Taxonomy + "</td></tr>";
                }

                if (EFFECTIVEDATE != "" && !(Effectivedateflag))
                {
                    strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt\"></td>";
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + EFFECTIVEDATE + "</td></tr>";
                }

                if (RegID != "" && !(RegistrationIDflag))
                {
                    strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt\"></td>";
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + RegID + "</td></tr>";
                }

                //   strToaddress += "</table>";
            }

            AddBookmark(fields, "TOADDRESS", strToaddress);

            //crawford logic
            mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add0>$$MAILPIECE$$</add0></h1>";
            mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add1>Add1:" + ContactName + "</add1></h1>";
            mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add2>Add2:" + Address1 + "</add2></h1>";
            if (Address2 != "" && !string.IsNullOrEmpty(Address2) && !string.IsNullOrWhiteSpace(Address2))
            {
                mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add3>Add3:" + Address2 + "</add3></h1>";
                mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add4>Add4:" + CityStateZip + "</add4></h1>";
            }
            else
            {
                mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add3>Add3:" + CityStateZip + "</add3></h1>";
            }
            AddBookmark(fields, "MAILPIECE", mailpiece);

        }

        private bool SendProviderWelcome(string regId, int noticeTypeID)
        {
            try
            {
                bool fnd = false;
                string EffectiveDate = "";
                string AggrementEndDate = string.Empty;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);              
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                string medicaidID = string.Empty;

                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    // From Service Location
                    medicaidID = row["MEDICAID_ID"].ToString();
                }

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                int workfloweventTypeID = 0;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    workfloweventTypeID = Methods.GetIntValue(row, "WORKFLOW_EVENT_TYPE_ID");
                    AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    AddBookmark(fields, "LEGALNAME", row["NAME"].ToString());
                    AddBookmark(fields, "PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));

                    AddBookmark(fields, "NPI", row["NPI"].ToString());
                    AddBookmark(fields, "TAXONOMY", row["TAXONOMY_CODE"].ToString());
                    AddBookmark(fields, "MEDICAID_ID", row["MEDICAID_ID"].ToString());
                    AddBookmark(fields, "PROVIDER_TYPE_NAME", row["PROVIDER_TYPE_NAME"].ToString()); // JIRA 3246
                    // JIRA 3246 - Get Specialties  
                    List<SqlParameter> sqlParams = new List<SqlParameter>();
                    sqlParams.Add(new SqlParameter("@REG_ID", regId));
                    DataSet dsServices = ExecuteStoredProcedure("usp_SelectREG_SPECIALTYServices", sqlParams);
                    string SpecialtyList = string.Empty;
                    if (Methods.HasRows(dsServices))
                    {
                        foreach (DataRow myRow in dsServices.Tables[0].Rows)
                        {
                            if (SpecialtyList != string.Empty) { SpecialtyList += ", "; }
                            SpecialtyList += Methods.GetStringValue(myRow, "SPECIALTY_TYPE_NAME");
                        }
                    }
                    AddBookmark(fields, "PROVIDER_SPECIALTIES", SpecialtyList); // JIRA 3246
                    bool isCredentialingProvider = isCredentialingRequired(regId);
                    if (isCredentialingProvider)
                    {
                        string committeeResultId = GetCommitteeResultByRegID(regId);

                        string credentialingContent = string.Empty;
                        //committeeResultId=5, is representing the "Pass 1 Year"
                        if (!string.IsNullOrWhiteSpace(committeeResultId) && committeeResultId == "5")
                        {
                            credentialingContent = "<p>You are required to complete the recredentialing process in 12 months. Please complete this during the revalidation process.</p>";
                        }
                        else
                        {
                            credentialingContent = "<p>You are required to complete the recredentialing process every 36 months. Please complete this during the revalidation process.</p>";
                        }

                        AddBookmark(fields, "CredentialingResults", credentialingContent);
                    }
                    else
                    {
                        AddBookmark(fields, "CredentialingResults", string.Empty);
                    }
                    if (!string.IsNullOrEmpty(row["CHANGE_EFFECTIVE_DATE"].ToString()))
                    {
                        DateTime eff = Convert.ToDateTime(row["CHANGE_EFFECTIVE_DATE"].ToString());
                        EffectiveDate = eff.ToString("MM/dd/yyyy");
                    }
                    if (!string.IsNullOrEmpty(row["END_DATE"].ToString()))
                    {
                        DateTime eff = Convert.ToDateTime(row["END_DATE"].ToString());
                        AggrementEndDate = eff.ToString("MM/dd/yyyy");
                    }
                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                                                                 row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                                                                 row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                                                                 (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                                                     : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), ds.Tables[0].Rows[0]["MEDICAID_ID"].ToString(), ds.Tables[0].Rows[0]["TAXONOMY_CODE"].ToString(), regId, ds.Tables[0].Rows[0]["DBA"].ToString(), EffectiveDate);

                    fnd = true;
                    AddBookmark(fields, "ProviderName", ToName);

                    // SAM751
                    if (noticeTypeID == 1)
                    {
                        AddBookmark(fields, "subject", "Welcome Ohio Medicaid Provider");
                    }
                    if (noticeTypeID == 2)
                    {
                        AddBookmark(fields, "subject", "Welcome Ohio Medicaid Provider (Reapplication/Reactivation)");
                    }
                }
                
                AddBookmark(fields, "EFFECTIVEDATE", EffectiveDate);
                AddBookmark(fields, "AGREEMENTENDDATE", AggrementEndDate);
                AddBookmark(fields, "Survey_Monkey", AppSettings.Get("Survey_Monkey", string.Empty));
                AddBookmark(fields, "PDMS-URL", AppSettings.Get("PDMS-URL", string.Empty));
                AddBookmark(fields, "Medicaid_URL", AppSettings.Get("Medicaid_URL", string.Empty));
                if (workfloweventTypeID == CON.WorkflowEventType.RevalReg)
                    AddBookmark(fields, "EVV_Reval_Welcome_Message", AppSettings.Get("EVV_Reval_Welcome_Message", string.Empty));
                else if (workfloweventTypeID == CON.WorkflowEventType.NewReg)
                    AddBookmark(fields, "EVV_NewReg_Welcome_Message", AppSettings.Get("EVV_NewReg_Welcome_Message", string.Empty));

                if (fnd)            // Need basic information in order to send email
                {

                    body = SendNotification(templateActualPath + @"/WelcomeLetterTemplateGroups.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "WelcomeLetterTemplateGroups.txt");
                }
                Fields = fields;
                return fnd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendCPCInvitation(string regId)
        {
            try
            {
                string emptyString = string.Empty;
                string toAddress = string.Empty;
                string medicaidID = string.Empty;
                string conditionalParagraph = string.Empty;
                bool meetCPCIndividual = false;
                bool meetCPCConvener = false;
                bool meetCPCKids = false;
                bool basicInformation = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                string cpc_AdditionalInfo_Link = AppSettings.Get("CPC_AdditionalInfo_Link", string.Empty);
                string cmc_ODM_Subscribe_Form_Link = AppSettings.Get("CMC_ODM_Subscribe_Form_Link", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsCPCEligibility = ExecuteStoredProcedure("usp_SelectCPCProviderEligibility", parms);
                if (dsCPCEligibility != null && dsCPCEligibility.Tables.Count > 0 && dsCPCEligibility.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsCPCEligibility.Tables[0].Rows[0];
                    if (row != null)
                    {
                        meetCPCConvener = (bool)row["Meet_CPC_Partnership_Criteria_File"];
                        meetCPCIndividual = (bool)row["Meet_CPC_Individual_Criteria"];
                        meetCPCKids = (bool)row["Meet_CPC_For_Kids_Criteria"];
                    }
                }
                if (meetCPCIndividual)
                {
                    conditionalParagraph = "<ul><li>A practice with 500+ Medicaid claims-based attributed members for individual participation in CPC</li></ul>";
                }
                if (meetCPCConvener)
                {
                    conditionalParagraph += "<ul><li>A practice with 150+ Medicaid claims-based attributed members for participating in a practice partnership in CPC</li></ul>";

                }
                if (meetCPCKids)
                {
                    conditionalParagraph += "<ul><li>A practice with 150+ Medicaid claims-based attributed members under age 21 in CPC for Kids.</li></ul>";
                }
                AddBookmark(fields, "conditional_paragraph", conditionalParagraph);
                AddBookmark(fields, "CPC_AdditionalInfo_Link", cpc_AdditionalInfo_Link);
                AddBookmark(fields, "CMC_ODM_Subscribe_Form_Link", cmc_ODM_Subscribe_Form_Link);
                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLocation = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                if (dsServiceLocation != null && dsServiceLocation.Tables.Count > 0 && dsServiceLocation.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsServiceLocation.Tables[0].Rows[0];
                    if (row != null)
                    {
                        medicaidID = row["MEDICAID_ID"].ToString();
                        AddBookmark(fields, "MedicaidID", row["MEDICAID_ID"].ToString());
                        string enrollmentStartDate = AppSettings.Get("CPCEnrollmentPeriodStartDate") + "/" + DateTime.Now.Year;
                        string enrollmentEndDate = AppSettings.Get("CPCEnrollmentPeriodEndDate") + "/" + DateTime.Now.Year;
                        string enrollmentDate = enrollmentStartDate + "-" + enrollmentEndDate;
                        toAddress = GetCPCToAddress(regId, CON.AddressType.PrimaryPractice);
                        AddBookmark(fields, "TOADDRESS", toAddress);
                        AddBookmark(fields, "upcoming_program_year", AppSettings.Get("CPCProgramYear"));
                        AddBookmark(fields, "current_year", DateTime.Now.Year.ToString());
                        AddBookmark(fields, "enrollment_date", enrollmentDate);
                        AddBookmark(fields, "last_full_date", enrollmentEndDate);
                        GetFromAddress(fields);
                        basicInformation = true;
                    }
                }
                if (basicInformation)            // Need basic information in order to send email
                {
                    body = SendNotification(templateActualPath + @"/CPCInvitationLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "CPCInvitationLetter.txt");
                }
                Fields = fields;
                return basicInformation;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to send CPC Invitation letter to" + regId
                                     + " Exception Message " + ex.Message + " Exception Stack = "
                                     + ex.StackTrace, Logging.LogPriority.Error);
                return false;
            }
        }
        private bool SendCPCReminder(string regId, string bodyTemplate)
        {
            try
            {
                string toAddress = string.Empty;
                string medicaidID = string.Empty;
                bool basicInformation = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                AddBookmark(fields, "final_enrollment_date", AppSettings.Get("CPCEnrollmentPeriodEndDate") + "/" + DateTime.Now.Year);
                AddBookmark(fields, "upcoming_program_year", AppSettings.Get("CPCProgramYear"));
                GetFromAddress(fields);
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    if (row != null)
                    {
                        // From Service Location
                        medicaidID = row["MEDICAID_ID"].ToString();
                        AddBookmark(fields, "MedicaidId", row["MEDICAID_ID"].ToString());
                        toAddress = GetCPCToAddress(regId, CON.AddressType.PrimaryPractice);
                        AddBookmark(fields, "TOADDRESS", toAddress);
                        basicInformation = true;
                    }
                }
                if (basicInformation)            // Need basic information in order to send email
                {
                    if (bodyTemplate == "CPC_REMINDER_LETTER")
                    {
                        body = SendNotification(templateActualPath + @"/CPCReminderInvitationLetter.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "CPCReminderInvitationLetter.txt");
                    }
                    else if (bodyTemplate == "CPC_FINAL_REMINDER_LETTER")
                    {
                        body = SendNotification(templateActualPath + @"/CPCFinalReminderInvitationLetter.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "CPCFinalReminderInvitationLetter.txt");
                    }

                }
                Fields = fields;
                return basicInformation;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to send " + bodyTemplate + " to" + regId
                                    + " Exception Message " + ex.Message + " Exception Stack = "
                                    + ex.StackTrace, Logging.LogPriority.Error);
                return false;
            }
        }
        private bool SendNonRenewalNotificationLetter(string regId)
        {
            try
            {
                string toAddress = string.Empty;
                string medicaidID = string.Empty;
                bool basicInformation = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                string cpc_AdditionalInfo_Link = AppSettings.Get("CPC_AdditionalInfo_Link", string.Empty);

                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                AddBookmark(fields, "upcoming_program_year", AppSettings.Get("CPCProgramYear"));
                AddBookmark(fields, "full_date_program_year", AppSettings.Get("CPCProgramStartDate").ToString() + "/" + AppSettings.Get("CPCProgramYear"));
                AddBookmark(fields, "CPC_AdditionalInfo_Link", cpc_AdditionalInfo_Link);
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsMedicaids = ExecuteStoredProcedure("usp_SelectCPC_MedicaidID", parms);
                if (dsMedicaids != null && dsMedicaids.Tables.Count > 0 && dsMedicaids.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsMedicaids.Tables[0].Rows[0];
                    if (row != null)
                    {

                        AddBookmark(fields, "MedicaidId", row["PRIMARY_MED_ID"].ToString());
                        AddBookmark(fields, "MEDICAID_CPC_ID", row["CPC_ID"].ToString());
                        toAddress = GetCPCToAddress(regId, CON.AddressType.PrimaryPractice);
                        AddBookmark(fields, "TOADDRESS", toAddress);
                        GetFromAddress(fields);
                        basicInformation = true;
                    }
                }
                if (basicInformation)            // Need basic information in order to send email
                {
                    body = SendNotification(templateActualPath + @"/CPCNonRenewalNotificationLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "CPCNonRenewalNotificationLetter.txt");
                }
                Fields = fields;
                return basicInformation;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to send CPC NonRenewal Notification Letter to" + regId
                                    + " Exception Message " + ex.Message + " Exception Stack = "
                                    + ex.StackTrace, Logging.LogPriority.Error);
                return false;
            }
        }

        private bool SendAttestationEnrollmentUpdateLetter(string regId)
        {
            try
            {
                bool basicInformation = false;
                bool isPPAdded = false;
                bool isPPRemoved = false;
                bool isCPCContactChanged = false;
                bool isKidsAdded = false;
                bool isKidsRemoved = false;
                string toAddress = string.Empty;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                string conditionalParagraph = string.Empty;
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsChangesOnEnrollment = ExecuteStoredProcedure("usp_GetCPCChangesOnEnrollment", parms);

                if (dsChangesOnEnrollment != null && dsChangesOnEnrollment.Tables.Count > 0 && dsChangesOnEnrollment.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsChangesOnEnrollment.Tables[0].Rows[0];
                    if (row != null)
                    {
                        isPPAdded = (bool)row["IS_PP_ADDED"];
                        isPPRemoved = (bool)row["IS_PP_REMOVED"];
                        isCPCContactChanged = (bool)row["IS_CPC_CONTACT_CHANGED"];
                        isKidsAdded = (bool)row["IS_KIDS_ADDED"];
                        isKidsRemoved = (bool)row["IS_KIDS_REMOVED"];
                    }
                    if (isPPAdded)
                    {
                        conditionalParagraph += "New Member added to your practice partnership <br> ";
                    }
                    if (isPPRemoved)
                    {
                        conditionalParagraph += "Member removed from your practice partnership <br>";
                    }
                    if (isCPCContactChanged)
                    {
                        conditionalParagraph += "Change to CPC contact information <br> ";
                    }
                    if (isKidsAdded)
                    {
                        conditionalParagraph += "Kids specialty added to your CPC enrollment <br> ";
                    }
                    if (isKidsRemoved)
                    {
                        conditionalParagraph += "Kids specialty removed from your CPC enrollment <br>";
                    }
                    AddBookmark(fields, "cpcProgramYear", AppSettings.Get("CPCProgramYear"));
                    AddBookmark(fields, "conditionalParagraph", conditionalParagraph);
                    GetFromAddress(fields);
                    toAddress = GetCPCToAddress(regId, CON.AddressType.CPCAddressType);
                    AddBookmark(fields, "TOADDRESS", toAddress);
                    basicInformation = true;
                }
                if (basicInformation)
                {
                    body = SendNotification(templateActualPath + @"/CPCAttestationEnrollmentUpdateLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "CPCAttestationEnrollmentUpdateLetter.txt");
                }
                Fields = fields;
                return basicInformation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool SendCPCWelcomeLetter(string regId)
        {
            try
            {
                bool basicInformation = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                string cpc_AdditionalInfo_Link = AppSettings.Get("CPC_AdditionalInfo_Link", string.Empty);
                string cmc_ODM_Subscribe_Form_Link = AppSettings.Get("CMC_ODM_Subscribe_Form_Link", string.Empty);
                string CpcId = string.Empty;
                string convenerCPCId = string.Empty;
                string kidsSpecialtyParagraph = string.Empty;
                string toAddress = string.Empty;
                bool kidsSpecialtyPresent = false;
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                string medicaidID = string.Empty;
                string CPC_Program_Year = AppSettings.Get("CPCProgramYear");
                AddBookmark(fields, "CPC_AdditionalInfo_Link", cpc_AdditionalInfo_Link);
                AddBookmark(fields, "CMC_ODM_Subscribe_Form_Link", cmc_ODM_Subscribe_Form_Link);
                AddBookmark(fields, "CPCUpcomingProgramYear", CPC_Program_Year);//CPCProgramyear
                string programYearStartDate = AppSettings.Get("CPCProgramStartDate") + "/" + CPC_Program_Year;
                string programYearEndDate = AppSettings.Get("CPCProgramEndDate") + "/" + CPC_Program_Year;
                string programYearFullDate = programYearStartDate + "-" + programYearEndDate;//CPC Program Year Full Date
                AddBookmark(fields, "CPCProgramStart_EndDates", programYearFullDate);
                GetFromAddress(fields);
                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsSpecialty = ExecuteStoredProcedure("usp_SelectREG_SPECIALTY", parms); // Get the kids Specialty
                if (dsSpecialty != null && dsSpecialty.Tables.Count > 0 && dsSpecialty.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsSpecialty.Tables[0].Rows[0];
                    if (row != null)
                    {
                        DateTime programYear = Convert.ToDateTime(programYearStartDate);
                        DateTime EndDate = new DateTime(2299, 12, 31);
                       
                        kidsSpecialtyPresent = dsSpecialty.Tables[0].AsEnumerable()
                                          .Where(r => r.Field<string>("MMIS_SPECIALTY_TYPE_ID") == CON.MMISSpecialtyType.CPCPEDIATRICS &&
                                                      r.Field<DateTime>("START_DATE") <= programYear &&
                                                      r.Field<DateTime>("END_DATE") > DateTime.Now &&
                                                      r.Field<Int32>("ENROLL_STATUS_ID") == CON.EnrollmentStatusTypeID.Active &&
                                                      (r["SENT_TO_SI"] != DBNull.Value ? Convert.ToBoolean(r["SENT_TO_SI"]) : true) == true)
                                          .Any() || dsSpecialty.Tables[0].AsEnumerable()
                                          .Where(r => r.Field<string>("MMIS_SPECIALTY_TYPE_ID") == CON.MMISSpecialtyType.CPCPEDIATRICS &&
                                                      r.Field<DateTime>("START_DATE") == programYear &&
                                                      r.Field<DateTime>("END_DATE") == EndDate &&
                                                      r.Field<Int32>("ENROLL_STATUS_ID") == CON.EnrollmentStatusTypeID.InActive &&
                                                      (r["SENT_TO_SI"] != DBNull.Value ? Convert.ToBoolean(r["SENT_TO_SI"]) : false) == false)
                                          .Any();

                        if (kidsSpecialtyPresent)
                        {
                            kidsSpecialtyParagraph = "and the Ohio CPC for Kids Program";
                        }
                        else
                        {
                            kidsSpecialtyParagraph = string.Empty;
                        }
                        AddBookmark(fields, "kidsSpecialtyParagraph", kidsSpecialtyParagraph);
                    }
                }
                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsLocation = ExecuteStoredProcedure("[dbo].[usp_SelectCPC_MedicaidID]", parms); // Get the kids Specialty
                if (dsLocation != null && dsLocation.Tables.Count > 0 && dsLocation.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsLocation.Tables[0].Rows[0];
                    if (row != null)
                    {
                        medicaidID = row["PRIMARY_MED_ID"].ToString();
                        CpcId = row["CPC_ID"].ToString();
                    }
                }
                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProvider = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                if (dsProvider != null && dsProvider.Tables.Count > 0 && dsProvider.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsProvider.Tables[0].Rows[0];
                    if (row != null)
                    {
                        string conditionalParagraph = string.Empty;
                        if (((int)row["APPLICATION_TYPE_ID"] == CON.ApplicationType.CPC) && ((int)row["ENTITY_TYPE_ID"] == CON.ProviderCategoryTypeID.Individual))
                        {
                            //Based on the application type and entity type
                            conditionalParagraph = "Your Ohio Medicaid provider number " + medicaidID + " will be participating individually, with CPC ID " + CpcId + ".";
                        }
                        else
                        {
                            //Based on the application type and entity type
                            conditionalParagraph = "Your Ohio Medicaid provider number " + medicaidID + " will be participating as a practice partnership with CPC Convener ID " + CpcId + ".";
                        }
                        AddBookmark(fields, "Enrollment_text", conditionalParagraph);
                        toAddress = GetCPCToAddress(regId, CON.AddressType.CPCAddressType);
                        AddBookmark(fields, "TOADDRESS", toAddress);
                        basicInformation = true;
                    }
                }
                if (basicInformation)
                {
                    body = SendNotification(templateActualPath + @"/CPCWelcomeLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "CPCWelcomeLetter.txt");
                }
                Fields = fields;
                return basicInformation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetCPCToAddress(string regId, int addressType)
        {
            string toAddress = string.Empty;
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("REG_ID", regId));
            parms.Add(new SqlParameter("ADDRESS_TYPE_ID", addressType));
            DataSet dsAddressInfo = ExecuteStoredProcedure("usp_SelectProviderAddressInfo", parms);
            if (dsAddressInfo != null && dsAddressInfo.Tables.Count > 0 && dsAddressInfo.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsAddressInfo.Tables[0].Rows[0];
                if (row != null)
                {
                    string toName = row["CONTACT_NAME"].ToString() == "" ? row["PRACTICE_NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    string address1 = string.IsNullOrWhiteSpace(row["ADDRESS1"].ToString()) ? "" : row["ADDRESS1"].ToString().TrimEnd();
                    string address2 = string.IsNullOrWhiteSpace(row["ADDRESS2"].ToString()) ? "" : "<br>" + row["ADDRESS2"].ToString().TrimEnd();
                    string address = address1 + address2;
                    toAddress = toName + "<br>" + address + "<br>" +
                                           row["CITY"].ToString().TrimEnd() + "," + row["STATE"].ToString().TrimEnd() + " " +
                                           row["ZIP"].ToString().TrimEnd();
                    return toAddress;
                }
            }
            return toAddress;
        }

        private string GetToAddressByAddressType(string regId, int addressType)
        {
            string toAddress = string.Empty;
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("REG_ID", regId));
            parms.Add(new SqlParameter("ADDRESS_TYPE_ID", addressType));
            DataSet dsAddressInfo = ExecuteStoredProcedure("usp_SelectProviderAddressInfo", parms);
            if (dsAddressInfo != null && dsAddressInfo.Tables.Count > 0 && dsAddressInfo.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsAddressInfo.Tables[0].Rows[0];
                if (row != null)
                {
                    string toName = row["CONTACT_NAME"].ToString() == "" ? row["PRACTICE_NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    string address1 = string.IsNullOrWhiteSpace(row["ADDRESS1"].ToString()) ? "" : row["ADDRESS1"].ToString().TrimEnd();
                    string address2 = string.IsNullOrWhiteSpace(row["ADDRESS2"].ToString()) ? "" : "<br>" + row["ADDRESS2"].ToString().TrimEnd();
                    string address = address1 + address2;
                    toAddress = toName + "<br>" + address + "<br>" +
                                           row["CITY"].ToString().TrimEnd() + "," + row["STATE"].ToString().TrimEnd() + " " +
                                           row["ZIP"].ToString().TrimEnd();
                    return toAddress;
                }
            }
            return toAddress;
        }

        private bool SendCMCWelcomeLetter(string regId)
        {
            try
            {
                bool basicInformation = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                string toAddress = string.Empty;
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                string medicaidID = string.Empty;
               
                string cmcODMWebsite = AppSettings.Get("CMC_ODM_WebSite");
                string CMCProgramWelcomeLetterStartDate = AppSettings.Get("CMCProgramWelcomeLetterStartDate");
                string CMCProgramWelcomeLetterEndDate = AppSettings.Get("CMCProgramWelcomeLetterEndDate");
                string cmc_ODM_Subscribe_Form_Link = AppSettings.Get("CMC_ODM_Subscribe_Form_Link", string.Empty);

                // OHPNM-18899 - Send correct program year in Welcome letters in Late Attestation for CMC
                string CMC_Program_Year = Convert.ToDateTime(CMCProgramWelcomeLetterStartDate).Year.ToString();
                AddBookmark(fields, "Upcoming_Prog_Year", CMC_Program_Year);

                AddBookmark(fields, "Start_Full_Date", CMCProgramWelcomeLetterStartDate);
                AddBookmark(fields, "Last_Full_Date", CMCProgramWelcomeLetterEndDate);
                AddBookmark(fields, "CMC_ODM_WebSite", cmcODMWebsite);
                AddBookmark(fields, "CMC_ODM_Subscribe_Form_Link", cmc_ODM_Subscribe_Form_Link);

                GetFromAddress(fields);
                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProvider = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                if (dsProvider != null && dsProvider.Tables.Count > 0 && dsProvider.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsProvider.Tables[0].Rows[0];
                    if (row != null)
                    {
                        toAddress = GetToAddressByAddressType(regId, CON.AddressType.PrimaryPractice);
                        AddBookmark(fields, "TOADDRESS", toAddress);
                        string medID = row["MEDICAID_ID"].ToString();
                        AddBookmark(fields, "MedicaidID", medID);
                        parms = new List<SqlParameter>();
                        parms.Add(new SqlParameter("REG_ID", regId));
                        parms.Add(new SqlParameter("ADDRESS_TYPE_ID", CON.AddressType.PrimaryPractice));
                        DataSet dsAddressInfo = ExecuteStoredProcedure("usp_SelectProviderAddressInfo", parms);
                        if (dsAddressInfo != null && dsAddressInfo.Tables.Count > 0 && dsAddressInfo.Tables[0].Rows.Count > 0)
                        {
                            DataRow row2 = dsAddressInfo.Tables[0].Rows[0];
                            if (row2 != null)
                            {
                                string practiceName = row2["PRACTICE_NAME"].ToString();
                                AddBookmark(fields, "Practice_Name", practiceName);
                            }
                        }
                        basicInformation = true;
                    }
                }
                if (basicInformation)
                {
                    body = SendNotification(templateActualPath + @"/CMCWelcomeLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "CMCWelcomeLetter.txt");
                }
                Fields = fields;
                return basicInformation;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to send Email Exception Message " + ex.Message + " Exception Stack = "
                                    + ex.StackTrace, Logging.LogPriority.Error);
                return false;
            }
        }

        private bool SendCMCReminder(string regId)
        {
            try
            {
                bool basicInformation = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                string portalLink = AppSettings.Get("PDMS-URL", string.Empty);
                string toAddress = string.Empty;
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                string medicaidID = string.Empty;
                string CMC_Program_Month = AppSettings.Get("CMCUpcomingProgramMonth");
                AddBookmark(fields, "Upcoming_Prog_Month", CMC_Program_Month);
                string CMC_Program_Year = AppSettings.Get("CMCUpcomingProgramYear");
                AddBookmark(fields, "Upcoming_Prog_Year", CMC_Program_Year);
                string programYearStartDate = AppSettings.Get("CMCProgramStartDate");
                string programYearEndDate = AppSettings.Get("CMCProgramEndDate");
                string cmcODMWebsite = AppSettings.Get("CMC_ODM_WebSite");
                string cmc_ODM_Subscribe_Form_Link = AppSettings.Get("CMC_ODM_Subscribe_Form_Link", string.Empty);

                DateTime myDate;
                if (!DateTime.TryParse(programYearStartDate, out myDate))
                {

                }
                string programMonth = myDate.ToString("MMMM");
                string programYear = myDate.Year.ToString();
                AddBookmark(fields, "Start_Full_Date", programYearStartDate);
                AddBookmark(fields, "Last_Full_Date", programYearEndDate);
                AddBookmark(fields, "Prog_Month", programMonth);
                AddBookmark(fields, "Prog_Year", programYear);
                AddBookmark(fields, "Portal_Link", portalLink);
                AddBookmark(fields, "CMC_ODM_Subscribe_Form_Link", cmc_ODM_Subscribe_Form_Link);
                AddBookmark(fields, "CMC_ODM_WebSite", cmcODMWebsite);
                GetFromAddress(fields);
                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProvider = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                if (dsProvider != null && dsProvider.Tables.Count > 0 && dsProvider.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsProvider.Tables[0].Rows[0];
                    if (row != null)
                    {
                        toAddress = GetToAddressByAddressType(regId, CON.AddressType.PrimaryPractice);
                        AddBookmark(fields, "TOADDRESS", toAddress);
                        string medID = row["MEDICAID_ID"].ToString();
                        AddBookmark(fields, "MedicaidID", medID);
                        parms = new List<SqlParameter>();
                        parms.Add(new SqlParameter("REG_ID", regId));
                        parms.Add(new SqlParameter("ADDRESS_TYPE_ID", CON.AddressType.PrimaryPractice));
                        DataSet dsAddressInfo = ExecuteStoredProcedure("usp_SelectProviderAddressInfo", parms);
                        if (dsAddressInfo != null && dsAddressInfo.Tables.Count > 0 && dsAddressInfo.Tables[0].Rows.Count > 0)
                        {
                            DataRow row2 = dsAddressInfo.Tables[0].Rows[0];
                            if (row2 != null)
                            {
                                string practiceName = row2["PRACTICE_NAME"].ToString();
                                AddBookmark(fields, "Practice_Name", practiceName);
                            }
                        }

                        string taxid = row["TAX_ID"].ToString();
                        AddBookmark(fields, "Last_4_TIN", taxid.Substring(5, 4));

                        List<SqlParameter> parms1 = new List<SqlParameter>();
                        parms1.Add(new SqlParameter("REG_ID", regId));
                        parms1.Add(new SqlParameter("CMC_PROGRAM_YEAR", programYear));
                        DataSet dsCMCMedID = ExecuteStoredProcedure("usp_SelectCMCMedicaidIDsbyTaxID", parms1);
                        if (dsCMCMedID != null && dsCMCMedID.Tables.Count > 0 && dsCMCMedID.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = dsCMCMedID.Tables[0].Rows[0];
                            string medID_tbl = dr["MED_ID_TABLE"].ToString();
                            AddBookmark(fields, "MED_ID_TABLE", medID_tbl);
                        }
                        basicInformation = true;
                    }
                }
                if (basicInformation)
                {
                    body = SendNotification(templateActualPath + @"/CMCReminderInvitationalLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "CMCReminderInvitationalLetter.txt");
                }
                Fields = fields;
                return basicInformation;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to send Email Exception Message " + ex.Message + " Exception Stack = "
                                    + ex.StackTrace, Logging.LogPriority.Error);
                return false;
            }
        }

        private bool SendCMCInvitational(string regId)
        {
            try
            {
                bool basicInformation = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                string portalLink = AppSettings.Get("PDMS-URL", string.Empty);
                string cmcODM_Link = AppSettings.Get("CMC_ODM_WebSite", string.Empty);
                string cmc_Enrollment_Weblink = AppSettings.Get("CMC_Enrollment_Weblink", string.Empty);
                string cmc_Learning_Management_System_Link = AppSettings.Get("CMC_Learning_Management_System_Link", string.Empty);
                string cmc_ODM_Subscribe_Form_Link = AppSettings.Get("CMC_ODM_Subscribe_Form_Link", string.Empty);
                string toAddress = string.Empty;
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                string medicaidID = string.Empty;
                string CMC_Program_Month = AppSettings.Get("CMCUpcomingProgramMonth");
                AddBookmark(fields, "Upcoming_Prog_Month", CMC_Program_Month);
                string CMC_Program_Year = AppSettings.Get("CMCUpcomingProgramYear");
                AddBookmark(fields, "Upcoming_Prog_Year", CMC_Program_Year);
                string programYearStartDate = AppSettings.Get("CMCProgramStartDate");
                string programYearEndDate = AppSettings.Get("CMCProgramEndDate");
                string cmc_ODM_Website = AppSettings.Get("CMC_ODM_WebSite");
                DateTime myDate;
                if (!DateTime.TryParse(programYearStartDate, out myDate))
                {

                }
                string programMonth = myDate.ToString("MMMM");
                string programYear = myDate.Year.ToString();
                AddBookmark(fields, "Start_Full_Date", programYearStartDate);
                AddBookmark(fields, "Last_Full_Date", programYearEndDate);
                AddBookmark(fields, "Prog_Month", programMonth);
                AddBookmark(fields, "Prog_Year", programYear);
                AddBookmark(fields, "Portal_Link", portalLink);
                AddBookmark(fields, "CMC_ODM_WebSite",cmc_ODM_Website);
                AddBookmark(fields, "CMC_Enrollment_Weblink", cmc_Enrollment_Weblink);
                AddBookmark(fields, "CMC_Learning_Management_System_Link", cmc_Learning_Management_System_Link);
                AddBookmark(fields, "CMC_ODM_Subscribe_Form_Link", cmc_ODM_Subscribe_Form_Link);
                GetFromAddress(fields);
                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProvider = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                if (dsProvider != null && dsProvider.Tables.Count > 0 && dsProvider.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsProvider.Tables[0].Rows[0];
                    if (row != null)
                    {
                        toAddress = GetToAddressByAddressType(regId, CON.AddressType.PrimaryPractice);
                        AddBookmark(fields, "TOADDRESS", toAddress);
                        string medID = row["MEDICAID_ID"].ToString();
                        AddBookmark(fields, "MedicaidID", medID);
                        parms = new List<SqlParameter>();
                        parms.Add(new SqlParameter("REG_ID", regId));
                        parms.Add(new SqlParameter("ADDRESS_TYPE_ID", CON.AddressType.PrimaryPractice));
                        DataSet dsAddressInfo = ExecuteStoredProcedure("usp_SelectProviderAddressInfo", parms);
                        if (dsAddressInfo != null && dsAddressInfo.Tables.Count > 0 && dsAddressInfo.Tables[0].Rows.Count > 0)
                        {
                            DataRow row2 = dsAddressInfo.Tables[0].Rows[0];
                            if (row2 != null)
                            {
                                string practiceName = row2["PRACTICE_NAME"].ToString();
                                AddBookmark(fields, "Practice_Name", practiceName);
                            }
                        }
                        string taxid = row["TAX_ID"].ToString();
                        AddBookmark(fields, "Last_4_TIN", taxid.Substring(5,4));

                        List<SqlParameter> parms1 = new List<SqlParameter>();
                        parms1.Add(new SqlParameter("REG_ID", regId));
                        parms1.Add(new SqlParameter("CMC_PROGRAM_YEAR", programYear));
                        DataSet dsCMCMedID = ExecuteStoredProcedure("usp_SelectCMCMedicaidIDsbyTaxID", parms1);
                        if (dsCMCMedID != null && dsCMCMedID.Tables.Count > 0 && dsCMCMedID.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = dsCMCMedID.Tables[0].Rows[0];
                            string medID_tbl = dr["MED_ID_TABLE"].ToString();
                            AddBookmark(fields, "MED_ID_TABLE", medID_tbl);
                        }
                        basicInformation = true;
                    }
                }
                if (basicInformation)
                {
                    body = SendNotification(templateActualPath + @"/CMCInvitationLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "CMCInvitationLetter.txt");
                }
                Fields = fields;
                return basicInformation;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to send Email Exception Message " + ex.Message + " Exception Stack = "
                                    + ex.StackTrace, Logging.LogPriority.Error);
                return false;
            }
        }

        private bool SendCMCNonRenevalLetter(string regId)
        {
            try
            {
                bool basicInformation = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                string portalLink = AppSettings.Get("PDMS-URL", string.Empty);
                string toAddress = string.Empty;
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                string medicaidID = string.Empty;
                string CMC_Program_Year = Convert.ToString(DateTime.Now.Year + 1);
                AddBookmark(fields, "Upcoming_Prog_Year", CMC_Program_Year);
                string programYearStartDate = AppSettings.Get("CMCProgramStartDate");
                string programYearEndDate = AppSettings.Get("CMCProgramEndDate");
                string cmcODMWebsite = AppSettings.Get("CMC_ODM_WebSite");
                AddBookmark(fields, "CMC_ODM_WebSite", cmcODMWebsite);
                DateTime myDate;
                if (!DateTime.TryParse(programYearStartDate, out myDate))
                {

                }
                string programMonth = myDate.ToString("MMMM");
                string programYear = myDate.Year.ToString();
                AddBookmark(fields, "FullDateProgYear", programYearStartDate);
                AddBookmark(fields, "Last_Full_Date", programYearEndDate);
                AddBookmark(fields, "Prog_Month", programMonth);
                AddBookmark(fields, "Prog_Year", programYear);
                AddBookmark(fields, "Portal_Link", portalLink);
                GetFromAddress(fields);
                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProvider = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                if (dsProvider != null && dsProvider.Tables.Count > 0 && dsProvider.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsProvider.Tables[0].Rows[0];
                    if (row != null)
                    {
                        toAddress = GetToAddressByAddressType(regId, CON.AddressType.PrimaryPractice);
                        AddBookmark(fields, "TOADDRESS", toAddress);
                        string medID = row["MEDICAID_ID"].ToString();
                        AddBookmark(fields, "MedicaidID", medID);
                        parms = new List<SqlParameter>();
                        parms.Add(new SqlParameter("REG_ID", regId));
                        parms.Add(new SqlParameter("ADDRESS_TYPE_ID", CON.AddressType.PrimaryPractice));
                        DataSet dsAddressInfo = ExecuteStoredProcedure("usp_SelectProviderAddressInfo", parms);
                        if (dsAddressInfo != null && dsAddressInfo.Tables.Count > 0 && dsAddressInfo.Tables[0].Rows.Count > 0)
                        {
                            DataRow row2 = dsAddressInfo.Tables[0].Rows[0];
                            if (row2 != null)
                            {
                                string practiceName = row2["PRACTICE_NAME"].ToString();
                                AddBookmark(fields, "Practice_Name", practiceName);
                            }
                        }
                        basicInformation = true;
                    }
                }
                if (basicInformation)
                {
                    body = SendNotification(templateActualPath + @"/CMCNonRenevalLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "CMCNonRenevalLetter.txt");
                }
                Fields = fields;
                return basicInformation;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to send Email Exception Message " + ex.Message + " Exception Stack = "
                                    + ex.StackTrace, Logging.LogPriority.Error);
                return false;
            }
        }
        private string GetCommitteeResultByRegID(string regId)
        {
            string committeeResult = string.Empty;
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("REG_ID", regId)
            };
            DataSet dsCredential = ExecuteStoredProcedure("usp_SelectREG_Credentialing", sqlParameters);

            if (Methods.HasRows(dsCredential))
            {
                DataRow row1 = dsCredential.Tables[0].Rows[0];
                committeeResult = Methods.GetStringValue(row1, "COMMITTEE_RESULT_ID");
            }

            return committeeResult;
        }

        private bool SendAssignSiteVisit(string regId)
        {
            try
            {
                bool fnd = false;
                string EffectiveDate = "";
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                List<SqlParameter> parms1 = new List<SqlParameter>();
                string medicaidID = string.Empty;

                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    // From Service Location
                    medicaidID = row["MEDICAID_ID"].ToString();

                }

                parms = new List<SqlParameter>();
                parms1 = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                parms1.Add(new SqlParameter("REG_ID", regId));
                ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                                                                 row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                                                                 row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                                                                 (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                                                     : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), ds.Tables[0].Rows[0]["MEDICAID_ID"].ToString(), ds.Tables[0].Rows[0]["TAXONOMY_CODE"].ToString(), regId, Methods.GetStringValue(row, "DBA"), EffectiveDate);

                    AddBookmark(fields, "NAME", ToName);
                    AddBookmark(fields, "REG_ID", regId);
                    fnd = true;
                }

                if (fnd)            // Need basic information in order to send email
                {
                    body = SendNotification(templateActualPath + @"/SiteVistAssignment.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "SiteVistAssignment.txt");
                    Fields = fields;
                }

                return fnd;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SendProvider10DAYBackgroundCheck(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                // Set the Expire Date
                AddBookmark(fields, "EXPIREDATE", DateTime.Now.AddDays(30).ToString("MM/dd/yyyy"));

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_BACKGROUND_CHECK", sqlParams);

                // Get the names of people needing FCBC and add any found to an HTML table row <tr>
                string providerListHtml = string.Empty;
                string code = string.Empty;
                string issueFBILetter = string.Empty;

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        bool includeProvider = false;
                        int backgroundStatusTypeID = 0;
                        string backgroundStatusTypeIDString = Methods.GetStringValue(row, "BACKGROUND_STATUS_TYPE_ID");
                        DateTime backgroundDate = Methods.GetDateValue(row, "DateOfInitialBCNotice");
                        AddBookmark(fields, "BACKGROUNDDATE", backgroundDate.ToString("MM/dd/yyyy"));
                        if (backgroundStatusTypeIDString == string.Empty) // Null status
                            includeProvider = true;
                        else
                        {
                            if (int.TryParse(backgroundStatusTypeIDString, out backgroundStatusTypeID))
                            {
                                includeProvider = (backgroundStatusTypeID != Constants.BackgroundStatusType.Yes); //Other than Yes, then not complete
                            }
                            else
                                includeProvider = true; //Invalid status
                        }

                        // Background not complete, so add the name to the list
                        if (includeProvider)
                        {
                            string providerName = Methods.GetStringValue(row, "NAME");
                            providerListHtml += string.Format("<li>{0}</li>", providerName);
                        }

                        issueFBILetter = Methods.GetStringValue(row, "IssueFBILetter");
                    }

                    code = !string.IsNullOrWhiteSpace((issueFBILetter)) && issueFBILetter == "True" ? "3701.881" : "5164.34";
                    AddBookmark(fields, "PROVIDER_LIST", providerListHtml);
                }
                else
                {
                    return false;
                }

                AddBookmark(fields, "CODE", code);
                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", sqlParams);
                sqlParams.Clear();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", sqlParams);
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
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }
                // Url to our application
                string pdmsUrl = string.Empty;
                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("key", "PDMS-URL"));
                ds = ExecuteStoredProcedure("sp_AppSettingsGetValue", sqlParams);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    pdmsUrl = ds.Tables[0].Rows[0]["AppSettingsValue"].ToString();
                AddBookmark(fields, "PDMSURL", pdmsUrl);
                AddBookmark(fields, "PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));

                body = SendNotification(templateActualPath + @"/BackgroundReportRequestFingerprint10DayNotice.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "BackgroundReportRequestFingerprint10DayNotice.txt");
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendProviderBackgroundCheck(string regId)
        {
            try
            {
                bool isReval = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                Dictionary<string, object> fields = new Dictionary<string, object>();

                // Set the Expire Date
                AddBookmark(fields, "EXPIREDATE", DateTime.Now.AddDays(30).ToString("MM/dd/yyyy"));

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_BACKGROUND_CHECK", sqlParams);

                // Get the names of people needing FCBC and add any found to an HTML table row <tr>
                string providerListHtml = string.Empty;
                string code = string.Empty;

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string issueFBILetter = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        bool includeProvider = false;
                        int backgroundStatusTypeID = 0;
                        string backgroundStatusTypeIDString = Methods.GetStringValue(row, "BACKGROUND_STATUS_TYPE_ID");
                        if (backgroundStatusTypeIDString == string.Empty) // Null status
                            includeProvider = true;
                        else
                        {
                            if (int.TryParse(backgroundStatusTypeIDString, out backgroundStatusTypeID))
                            {
                                includeProvider = (backgroundStatusTypeID != Constants.BackgroundStatusType.Yes); //Other than Yes, then not complete
                            }
                            else
                                includeProvider = true; //Invalid status
                        }

                        // Background not complete, so add the name to the list
                        if (includeProvider)
                        {
                            string providerName = Methods.GetStringValue(row, "NAME");
                            providerListHtml += string.Format("<li>{0}</li>", providerName);
                        }

                        issueFBILetter = Methods.GetStringValue(row, "IssueFBILetter");
                    }

                    code = !string.IsNullOrWhiteSpace((issueFBILetter)) && issueFBILetter == "True" ? "3701.881" : "5164.34";

                    AddBookmark(fields, "PROVIDER_LIST", providerListHtml);
                }
                else
                {
                    return false;
                }
                AddBookmark(fields, "CODE", code);

                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", sqlParams);

                sqlParams.Clear();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", sqlParams);
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

                    isReval = Methods.GetIntValue(row, "WORKFLOW_EVENT_TYPE_ID") == CON.WorkflowEventType.RevalReg;
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }


                // Url to our application
                string pdmsUrl = string.Empty;
                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("key", "PDMS-URL"));
                ds = ExecuteStoredProcedure("sp_AppSettingsGetValue", sqlParams);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    pdmsUrl = ds.Tables[0].Rows[0]["AppSettingsValue"].ToString();
                AddBookmark(fields, "PDMSURL", pdmsUrl);
                AddBookmark(fields, "PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));

                if (isReval)
                {
                    body = SendNotification(templateActualPath + @"/BackgroundLetterTemplateGroupsReval.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "BackgroundLetterTemplateGroupsReval.txt");
                }
                else
                {
                    body = SendNotification(templateActualPath + @"/BackgroundLetterTemplateGroups.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "BackgroundLetterTemplateGroups.txt");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendProviderBackgroundCheckBCI(string regId)
        {
            try
            {
                bool isReval = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                Dictionary<string, object> fields = new Dictionary<string, object>();

                // Set Reg ID
                AddBookmark(fields, "REGID", regId);

                // Set the Expire Date
                AddBookmark(fields, "EXPIREDATE", DateTime.Now.AddDays(30).ToString("MM/dd/yyyy"));

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_BACKGROUND_CHECK", sqlParams);

                // Get the names of people needing FCBC and add any found to an HTML table row <tr>
                string providerListHtml = string.Empty;
                string code = string.Empty;

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string issueFBILetter = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        bool includeProvider = false;
                        int backgroundStatusTypeID = 0;
                        string backgroundStatusTypeIDString = Methods.GetStringValue(row, "BACKGROUND_STATUS_TYPE_ID");
                        if (backgroundStatusTypeIDString == string.Empty) // Null status
                            includeProvider = true;
                        else
                        {
                            if (int.TryParse(backgroundStatusTypeIDString, out backgroundStatusTypeID))
                            {
                                includeProvider = (backgroundStatusTypeID != Constants.BackgroundStatusType.Yes); //Other than Yes, then not complete
                            }
                            else
                                includeProvider = true; //Invalid status
                        }

                        // Background not complete, so add the name to the list
                        if (includeProvider)
                        {
                            string providerName = Methods.GetStringValue(row, "NAME");
                            providerListHtml += string.Format("<li>{0}</li>", providerName);
                        }

                        issueFBILetter = Methods.GetStringValue(row, "IssueFBILetter");
                    }

                    code = !string.IsNullOrWhiteSpace((issueFBILetter)) && issueFBILetter == "True" ? "3701.881" : "5164.34";

                    AddBookmark(fields, "PROVIDER_LIST", providerListHtml);
                }
                else
                {
                    return false;
                }
                AddBookmark(fields, "CODE", code);

                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", sqlParams);

                sqlParams.Clear();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", sqlParams);
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

                    isReval = Methods.GetIntValue(row, "WORKFLOW_EVENT_TYPE_ID") == CON.WorkflowEventType.RevalReg;
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }


                // Url to our application
                string pdmsUrl = string.Empty;
                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("key", "PDMS-URL"));
                ds = ExecuteStoredProcedure("sp_AppSettingsGetValue", sqlParams);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    pdmsUrl = ds.Tables[0].Rows[0]["AppSettingsValue"].ToString();
                AddBookmark(fields, "PDMSURL", pdmsUrl);
                AddBookmark(fields, "PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));

                //if (isReval)
                //{
                //    body = SendNotification(templateActualPath + @"/BackgroundLetterTemplateGroupsReval.txt", fields, true);
                //    CreateCommunicationEvent(fields, new Guid(), regId, "BackgroundLetterTemplateGroupsReval.txt");
                //}
                //else
                //{
                    body = SendNotification(templateActualPath + @"/BackgroundLetterTemplateBCINotice.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "BackgroundLetterTemplateBCINotice.txt");
                //}
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendProviderBackgroundCheckCombinedBCIAndFBI(string regId)
        {
            try
            {
                bool isReval = false;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                Dictionary<string, object> fields = new Dictionary<string, object>();

                // Set the Expire Date
                AddBookmark(fields, "EXPIREDATE", DateTime.Now.AddDays(30).ToString("MM/dd/yyyy"));

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_BACKGROUND_CHECK", sqlParams);

                // Get the names of people needing FCBC and add any found to an HTML table row <tr>
                string providerListHtml = string.Empty;
                string code = string.Empty;

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string issueFBILetter = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        bool includeProvider = false;
                        int backgroundStatusTypeID = 0;
                        string backgroundStatusTypeIDString = Methods.GetStringValue(row, "BACKGROUND_STATUS_TYPE_ID");
                        if (backgroundStatusTypeIDString == string.Empty) // Null status
                            includeProvider = true;
                        else
                        {
                            if (int.TryParse(backgroundStatusTypeIDString, out backgroundStatusTypeID))
                            {
                                includeProvider = (backgroundStatusTypeID != Constants.BackgroundStatusType.Yes); //Other than Yes, then not complete
                            }
                            else
                                includeProvider = true; //Invalid status
                        }

                        // Background not complete, so add the name to the list
                        if (includeProvider)
                        {
                            string providerName = Methods.GetStringValue(row, "NAME");
                            providerListHtml += string.Format("<li>{0}</li>", providerName);
                        }

                        issueFBILetter = Methods.GetStringValue(row, "IssueFBILetter");
                    }

                    code = !string.IsNullOrWhiteSpace((issueFBILetter)) && issueFBILetter == "True" ? "3701.881" : "5164.34";

                    AddBookmark(fields, "PROVIDER_LIST", providerListHtml);
                }
                else
                {
                    return false;
                }
                AddBookmark(fields, "CODE", code);
                AddBookmark(fields, "REGID", regId);

                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", sqlParams);

                sqlParams.Clear();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", sqlParams);
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

                    isReval = Methods.GetIntValue(row, "WORKFLOW_EVENT_TYPE_ID") == CON.WorkflowEventType.RevalReg;
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));

                    AddBookmark(fields, "PROVIDERNAME", Methods.GetStringValue(row, "NAME"));
                    AddBookmark(fields, "NPI", Methods.GetStringValue(row, "NPI"));
                }
                else
                {
                    return false;
                }


                // Url to our application
                string pdmsUrl = string.Empty;
                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("key", "PDMS-URL"));
                ds = ExecuteStoredProcedure("sp_AppSettingsGetValue", sqlParams);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    pdmsUrl = ds.Tables[0].Rows[0]["AppSettingsValue"].ToString();
                AddBookmark(fields, "PDMSURL", pdmsUrl);
                AddBookmark(fields, "PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));

                //if (isReval)
                //{
                //    body = SendNotification(templateActualPath + @"/BackgroundLetterTemplateGroupsReval.txt", fields, true);
                //    CreateCommunicationEvent(fields, new Guid(), regId, "BackgroundLetterTemplateGroupsReval.txt");
                //}
                //else
                //{
                    body = SendNotification(templateActualPath + @"/BackgroundLetterTemplateCombinedFBIAndBCI.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "BackgroundLetterTemplateCombinedFBIAndBCI.txt");
                //}
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendProviderBackgroundSecondNoticeCheck(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                Dictionary<string, object> fields = new Dictionary<string, object>();

                // Set the Expire Date
                AddBookmark(fields, "EXPIREDATE", DateTime.Now.AddDays(30).ToString("MM/dd/yyyy"));

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_BACKGROUND_CHECK", sqlParams);

                // Get the names of people needing FCBC and add any found to an HTML table row <tr>
                string providerListHtml = string.Empty;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        bool includeProvider = false;
                        int backgroundStatusTypeID = 0;
                        string backgroundStatusTypeIDString = Methods.GetStringValue(row, "BACKGROUND_STATUS_TYPE_ID");
                        if (backgroundStatusTypeIDString == string.Empty) // Null status
                            includeProvider = true;
                        else
                        {
                            if (int.TryParse(backgroundStatusTypeIDString, out backgroundStatusTypeID))
                            {
                                includeProvider = (backgroundStatusTypeID != Constants.BackgroundStatusType.Yes); //Other than Yes, then not complete
                            }
                            else
                                includeProvider = true; //Invalid status
                        }

                        // Background not complete, so add the name to the list
                        if (includeProvider)
                        {
                            string providerName = Methods.GetStringValue(row, "NAME");
                            providerListHtml += string.Format("<li>{0}</li>", providerName);
                        }
                    }

                    AddBookmark(fields, "PROVIDER_LIST", providerListHtml);
                }
                else
                {
                    return false;
                }
                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", sqlParams);

                sqlParams.Clear();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", sqlParams);
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
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }

                // Url to our application
                string pdmsUrl = string.Empty;
                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("key", "PDMS-URL"));
                ds = ExecuteStoredProcedure("sp_AppSettingsGetValue", sqlParams);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    pdmsUrl = ds.Tables[0].Rows[0]["AppSettingsValue"].ToString();
                AddBookmark(fields, "PDMSURL", pdmsUrl);
                AddBookmark(fields, "PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));

                body = SendNotification(templateActualPath + @"/BackgroundLetterSecondNotice.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "BackgroundLetterSecondNotice.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetOrientationTrainingDetails(int RiskLevelID, bool IsDME)
        {
            string strTrainingDts = "";

            strTrainingDts = @"<br /><span style=""text-align: center; width: 100%;""><strong>Training Time: 10:00 am - 12:00 pm <br /> <br />Training Dates<br /></strong></span>";

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("RiskLevelID", RiskLevelID));
            sqlParams.Add(new SqlParameter("IsDME", IsDME));
            DataSet ds = ExecuteStoredProcedure("usp_SelectOrientation_Dates", sqlParams);

            if (MAXIMUS.Core.Libraries.Methods.HasRows(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strTrainingDts += Convert.ToDateTime(row["ORIENTATION_DATE"].ToString()).ToString("MMMM dd, yyyy") + @"<br />";
                }
            }

            return strTrainingDts;
        }

        private string GetFiduciarySignTemplate()
        {
            string strReturn = "";
            strReturn += @"<tr>";
            strReturn += @"<td style=""width: 50%;"">";
            strReturn += @"<p>";
            strReturn += @"  ____________________________________________<br />";
            strReturn += @"  Name/Title of Individual with Fiduciary Authority";
            strReturn += @"</p>";
            strReturn += @"</td>";
            strReturn += @"<td style=""width: 50%;"">";
            strReturn += @" <p>";
            strReturn += @"    __________________________________________<br />";
            strReturn += @"   Email address of Individual with Fiduciary Authority";
            strReturn += @"  </p>";
            strReturn += @"</td>";
            strReturn += @"</tr>";
            return strReturn;
        }

        private bool SendProviderOrientation(string bodyTemplate, string regId)
        {
            try
            {
                bool fnd = false;
                //int toPartyId = GetAdminPartyId();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                string riskLevel = "";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    AddBookmark(fields, "LEGALNAME", row["NAME"].ToString());

                    //int partyId = Methods.GetIntValue(ds.Tables[0].Rows[0]["PARTY_ID"]);
                    //if (partyId > 0) toPartyId = partyId;

                    GetFromAddress(fields);

                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));

                    if (bodyTemplate == "EMAIL_DME_TEMPLATE_ORIENTATION")
                    {
                        AddBookmark(fields, "RISKLEVEL", "DME/POS");
                        AddBookmark(fields, "TRAININGDETAILS", GetOrientationTrainingDetails(0, true));

                        AddBookmark(fields, "FIDUCIARY", GetFiduciarySignTemplate());
                        AddBookmark(fields, "OUTSIDEREP", "<li>A person with fiduciary authority.</li><li>A person with responsibility for accepting and processing Medicaid requests for goods and/or services at the service location. </li>");
                    }
                    else if (bodyTemplate == "EMAIL_TEMPLATE_PHARMA_ORIENTATION") //currently will not be used
                    {
                        AddBookmark(fields, "RISKLEVEL", "Moderate Risk");
                        AddBookmark(fields, "TRAININGDETAILS", GetOrientationTrainingDetails(2, false));

                        AddBookmark(fields, "FIDUCIARY", "");
                        AddBookmark(fields, "OUTSIDEREP", "");
                    }
                    else
                    {
                        riskLevel = row["PROVIDER_RISK_LEVEL_NAME"].ToString();
                        AddBookmark(fields, "RISKLEVEL", riskLevel + " Risk");

                        subject = riskLevel + " " + subject;
                        if (riskLevel.ToLower() == "high")
                        {
                            AddBookmark(fields, "TRAININGDETAILS", GetOrientationTrainingDetails(Convert.ToInt32(row["PROVIDER_RISK_LEVEL_ID"].ToString()), false));

                            AddBookmark(fields, "FIDUCIARY", GetFiduciarySignTemplate());
                            AddBookmark(fields, "OUTSIDEREP", "<li>A person with fiduciary authority.</li>");
                        }
                        else if (riskLevel.ToLower() == "moderate")
                        {
                            AddBookmark(fields, "TRAININGDETAILS", GetOrientationTrainingDetails(Convert.ToInt32(row["PROVIDER_RISK_LEVEL_ID"].ToString()), false));

                            AddBookmark(fields, "FIDUCIARY", "");
                            AddBookmark(fields, "OUTSIDEREP", "");
                        }
                        else
                        {
                            AddBookmark(fields, "TRAININGDETAILS", "");
                            AddBookmark(fields, "FIDUCIARY", "");
                            AddBookmark(fields, "OUTSIDEREP", "");
                        }
                    }
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    fnd = true;
                }

                if (fnd)            // Need basic information in order to send email
                {
                    // Pharma orientation uses a different template
                    body = SendNotification(templateActualPath + @"/OrientationLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "OrientationLetter.txt");
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendProvider10DayNotice(string regId)
        {
            try
            {
                bool fnd = false;
                //int toPartyId = GetAdminPartyId();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fnd = true;
                }

                if (fnd)            // Need basic information in order to send email
                {
                    body = SendNotification(templateActualPath + @"/ProviderApplication10DayNotice.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ProviderApplication10DayNotice.txt");
                }
                return fnd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendReEnrollmentSuccessNotice(string regId)
        {
            try
            {
                bool fnd = false;
                //int toPartyId = GetAdminPartyId();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                string EffectiveDate = "";
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    AddBookmark(fields, "LEGALNAME", row["NAME"].ToString());

                    fields.Add("NPI", ds.Tables[0].Rows[0]["NPI"].ToString());



                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CHANGE_EFFECTIVE_DATE"].ToString()))
                    {
                        DateTime eff = Convert.ToDateTime(ds.Tables[0].Rows[0]["CHANGE_EFFECTIVE_DATE"].ToString());
                        EffectiveDate = eff.ToString("MM/dd/yyyy");
                    }

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();



                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                                         row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                                         row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                                         (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                             : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), ds.Tables[0].Rows[0]["MEDICAID_ID"].ToString(), "", regId, Methods.GetStringValue(row, "DBA"), EffectiveDate);


                    fields.Add("MEDICAID", ds.Tables[0].Rows[0]["MEDICAID_ID"].ToString());


                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                    fnd = true;
                }

                if (fnd)            // Need basic information in order to send email
                {
                    body = SendNotification(templateActualPath + @"/ReEnrollmentSuccessNotice.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ReEnrollmentSuccessNotice.txt");
                }
                return fnd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendChangeApprovedServicesNotice(string regId)
        {
            try
            {
                bool fnd = false;
                //int toPartyId = GetAdminPartyId();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    AddBookmark(fields, "CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    AddBookmark(fields, "LEGALNAME", row["NAME"].ToString());

                    fields.Add("NPI", ds.Tables[0].Rows[0]["NPI"].ToString());
                    if (!string.IsNullOrEmpty(row["CHANGE_EFFECTIVE_DATE"].ToString()))
                    {
                        DateTime eff = Convert.ToDateTime(row["CHANGE_EFFECTIVE_DATE"].ToString());
                        AddBookmark(fields, "EFFECTIVEDATE", eff.ToString("MM/dd/yyyy"));
                    }

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), "", "", regId, Methods.GetStringValue(row, "DBA"));


                    fields.Add("MEDICAID", ds.Tables[0].Rows[0]["MEDICAID_ID"].ToString());


                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                    fnd = true;
                }

                if (fnd)            // Need basic information in order to send email
                {
                    body = SendNotification(templateActualPath + @"/ReEnrollmentSuccessNotice.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ReEnrollmentSuccessNotice.txt");
                }
                return fnd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool RevalidationRequest(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    templateActualPath = @"C:\Users\CA_OHPNM_DEV_11\source\repos\p3\ohpnm-src-p3\PDMS\ProviderDataManagementSystemService\Documents";
                }
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                    body = SendNotification(templateActualPath + @"/RevalidationRequest.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "RevalidationRequest.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SuccessfullRevalidation(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                // Get Provider information
                List<SqlParameter> parms = new List<SqlParameter> { new SqlParameter("REG_ID", regId) };
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    fields.Add("MEDICAIDID", medicaidID);
                    DateTime endDate;
                    string endDateString = "";
                    endDateString = DateTime.TryParse(dsProv.Tables[0].Rows[0]["END_DATE"].ToString(), out endDate) ? endDate.ToString("MM-dd-yyyy") : "(END DATE not found)";
                    

                    string ToName = dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString() == ""
                        ? dsProv.Tables[0].Rows[0]["NAME"].ToString()
                        : dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString();

                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(),
                        dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                        dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(),
                        dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                        dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " +
                        dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString())
                            ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"),
                        Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, "", regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"), endDateString, "Re-Enrollment Due Date");

                    fields.Add("TAXID", dsProv.Tables[0].Rows[0]["TAX_ID"].ToString());
                    //fields.Add("FONTSIZE", "14pt");
                    //fields.Add("DISPLAYOMR", string.Empty);
                }
                else
                {
                    return false;
                }
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));

                parms.Clear();
                parms.Add(new SqlParameter("RegID", regId));
                DataSet dsReg = ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegID", parms);
                DataRow drReg = Methods.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                string mmisProviderType = Methods.GetStringValue(drReg, "MMISProviderTypeID");
                bool isCredentialingProvider = isCredentialingRequired(regId);

                if (mmisProviderType == CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR)
                {
                    body = SendNotification(templateActualPath + @"/ICFSuccessfulRevalidation.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ICFSuccessfulRevalidation.txt");
                }
                else if (mmisProviderType == CON.LTCProviderTypes.NursingFacility)
                {
                    body = SendNotification(templateActualPath + @"/NFSuccessfulRevalidation.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "NFSuccessfulRevalidation.txt");
                }
                else
                {
                    if (isCredentialingProvider)
                    {
                        fields.Add("REVALYEARS", "three");
                        fields.Add("CREDENTIALTEXT", "You are required to complete the recredentialing process every 36 months. Please complete this during the revalidation process.");
                    }
                    else
                    {
                        fields.Add("REVALYEARS", "five");
                        fields.Add("CREDENTIALTEXT", "");
                    }
                    body = SendNotification(templateActualPath + @"/SuccessfullRevalidation.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "SuccessfullRevalidation.txt");
                }

                AddLicenseInformationForProvider(regId);
                AddEmailAddressForProvider(regId);
                Fields = fields;
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool RevalidationInitialNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                // Get Provider information
                List<SqlParameter> parms = new List<SqlParameter> { new SqlParameter("REG_ID", regId) };
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsTaxonomy = ExecuteStoredProcedure("usp_SelectREG_TAXONOMY", parms);
                string taxonomy = string.Empty;
                if (dsTaxonomy.Tables.Count > 0 && dsTaxonomy.Tables[0].Rows.Count > 0)
                {
                    taxonomy = dsTaxonomy.Tables[0].Rows[0]["TAXONOMY_NAME"].ToString();
                }
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    fields.Add("MEDICAIDID", medicaidID);
                    DateTime endDate;
                    string endDateString = "";
                    endDateString = DateTime.TryParse(dsProv.Tables[0].Rows[0]["END_DATE"].ToString(), out endDate) ? endDate.ToString("MM-dd-yyyy") : "(END DATE not found)";
                    fields.Add("REVALIDATIONDUE", endDateString);
                    fields.Add("LICENSEREVALIDATIONDUE", endDateString);

                    string ToName = dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString() == ""
                        ? dsProv.Tables[0].Rows[0]["NAME"].ToString()
                        : dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString();

                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(),
                        dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                        dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(),
                        dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                        dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " +
                        dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString())
                            ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"),
                        Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, taxonomy, regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"), endDateString, "Effective Date");

                    fields.Add("TAXID", dsProv.Tables[0].Rows[0]["TAX_ID"].ToString());
                    //fields.Add("FONTSIZE", "14pt");
                    //fields.Add("DISPLAYOMR", string.Empty);

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("PROVIDER_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "PROVIDER_TYPE_ID")));
                    parameters.Add(new SqlParameter("ENTITY_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "ENTITY_TYPE_ID")));
                    parameters.Add(new SqlParameter("APPLICATION_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "APPLICATION_TYPE_ID")));
                    DataSet dsAppFees = ExecuteStoredProcedure("usp_SelectProvider_Type_Fee", parameters);
                    if (Methods.HasRows(dsAppFees))
                    {
                        string appfeeDetail = "<p>Additionally, your provider type (organizational/agency) must pay an application fee at the time of revalidation as required by 42 CFR 455.460 Application fee. If your organization is enrolled with Medicare and/or another state’s Medicaid agency, you will not pay the fee again but will be required to submit proof of payment. You will be prompted through these questions as you begin the revalidation process on-line.</p>";
                        fields.Add("ShowApplicationFeeDetail", appfeeDetail);
                    }
                    else
                        fields.Add("ShowApplicationFeeDetail", string.Empty);

                    if (isCredentialingRequired(regId))
                    {
                        string credentialDetail = "<p>During the revalidation process, recredentialing is also required. In order to complete the revalidation and recredentialing process, please submit your information at least 30 days prior to your revalidation due date. Late submission of information may result in termination and require the enrollment/credentialing process to be restarted.</p>";
                        fields.Add("ShowCredentialDetail", credentialDetail);
                    }
                    else
                    {
                        fields.Remove("ShowCredentialDetail");
                    }
                }
                else
                {
                    return false;
                }
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                AddLicenseInformationForProvider(regId);
                AddEmailAddressForProvider(regId);
                body = SendNotification(templateActualPath + @"/RevalidationInitialNotice.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "RevalidationInitialNotice.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool RevalidationSecondNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsTaxonomy = ExecuteStoredProcedure("usp_SelectREG_TAXONOMY", parms);
                string taxonomy = string.Empty;
                if (dsTaxonomy.Tables.Count > 0 && dsTaxonomy.Tables[0].Rows.Count > 0)
                {
                    taxonomy = dsTaxonomy.Tables[0].Rows[0]["TAXONOMY_NAME"].ToString();
                }
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    DateTime endDate;
                    string endDateString = "";
                    if (DateTime.TryParse(dsProv.Tables[0].Rows[0]["END_DATE"].ToString(), out endDate))
                        endDateString = endDate.ToString("MM-dd-yyyy");
                    else
                        endDateString = "";
                    fields.Add("REVALIDATIONDUE", endDateString);
                    

                    string ToName = dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString() == "" ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString();


                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, taxonomy, regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"), endDateString, "Re-Enrollment Due Date");

                    fields.Add("TAXID", dsProv.Tables[0].Rows[0]["TAX_ID"].ToString());
                    //fields.Add("FONTSIZE", "14pt");
                    //fields.Add("DISPLAYOMR", string.Empty);

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("PROVIDER_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "PROVIDER_TYPE_ID")));
                    parameters.Add(new SqlParameter("ENTITY_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "ENTITY_TYPE_ID")));
                    parameters.Add(new SqlParameter("APPLICATION_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "APPLICATION_TYPE_ID")));
                    DataSet dsAppFees = ExecuteStoredProcedure("usp_SelectProvider_Type_Fee", parameters);
                    if (Methods.HasRows(dsAppFees))
                    {
                        string appfeeDetail = "<p>Additionally, your provider type (organizational/agency) must pay an application fee at the time of revalidation as required by 42 CFR 455.460 Application fee. If your organization is enrolled with Medicare and/or another state’s Medicaid agency, you will not pay the fee again but will be required to submit proof of payment. You will be prompted through these questions as you begin the revalidation process on-line.</p>";
                        fields.Add("ShowApplicationFeeDetail", appfeeDetail);
                    }
                    else
                        fields.Add("ShowApplicationFeeDetail", string.Empty);

                    if (isCredentialingRequired(regId))
                    {
                        string credentialDetail = "<p>During the revalidation process, recredentialing is also required. In order to complete the revalidation and recredentialing process, please submit your information at least 30 days prior to your revalidation due date. Late submission of information may result in termination and require the enrollment/credentialing process to be restarted.</p>";
                        fields.Add("ShowCredentialDetail", credentialDetail);
                    }
                    else
                    {
                        fields.Remove("ShowCredentialDetail");
                    }
                }
                else
                {
                    return false;
                }
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                // fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                AddLicenseInformationForProvider(regId);
                AddEmailAddressForProvider(regId);
                body = SendNotification(templateActualPath + @"/RevalidationSecondNotice.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "RevalidationSecondNotice.txt");
                Fields = fields;
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool RevalidationThirdNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsTaxonomy = ExecuteStoredProcedure("usp_SelectREG_TAXONOMY", parms);
                string taxonomy = string.Empty;
                if (dsTaxonomy.Tables.Count > 0 && dsTaxonomy.Tables[0].Rows.Count > 0)
                {
                    taxonomy = dsTaxonomy.Tables[0].Rows[0]["TAXONOMY_NAME"].ToString();
                }
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    DateTime endDate;
                    string endDateString = "";
                    if (DateTime.TryParse(dsProv.Tables[0].Rows[0]["END_DATE"].ToString(), out endDate))
                        endDateString = endDate.ToString("MM-dd-yyyy");
                    else
                        endDateString = "";
                    fields.Add("REVALIDATIONDUE", endDateString);
                    

                    string ToName = dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString() == "" ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString();


                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, taxonomy, regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"), endDateString, "Re-Enrollment Due Date");

                    fields.Add("TAXID", dsProv.Tables[0].Rows[0]["TAX_ID"].ToString());
                    //fields.Add("FONTSIZE", "14pt");
                    //fields.Add("DISPLAYOMR", string.Empty);

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("PROVIDER_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "PROVIDER_TYPE_ID")));
                    parameters.Add(new SqlParameter("ENTITY_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "ENTITY_TYPE_ID")));
                    parameters.Add(new SqlParameter("APPLICATION_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "APPLICATION_TYPE_ID")));
                    DataSet dsAppFees = ExecuteStoredProcedure("usp_SelectProvider_Type_Fee", parameters);
                    if (Methods.HasRows(dsAppFees))
                    {
                        string appfeeDetail = "<p>Additionally, your provider type (organizational/agency) must pay an application fee at the time of revalidation as required by 42 CFR 455.460 Application fee. If your organization is enrolled with Medicare and/or another state’s Medicaid agency, you will not pay the fee again but will be required to submit proof of payment. You will be prompted through these questions as you begin the revalidation process on-line.</p>";
                        fields.Add("ShowApplicationFeeDetail", appfeeDetail);
                    }
                    else
                        fields.Add("ShowApplicationFeeDetail", string.Empty);

                    if (isCredentialingRequired(regId))
                    {
                        string credentialDetail = "<p>During the revalidation process, recredentialing is also required. In order to complete the revalidation and recredentialing process, please submit your information at least 30 days prior to your revalidation due date. Late submission of information may result in termination and require the enrollment/credentialing process to be restarted.</p>";
                        fields.Add("ShowCredentialDetail", credentialDetail);
                    }
                    else
                    {
                        fields.Remove("ShowCredentialDetail");
                    }
                }
                else
                {
                    return false;
                }
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                //fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));

                AddLicenseInformationForProvider(regId);
                AddEmailAddressForProvider(regId);
                body = SendNotification(templateActualPath + @"/RevalidationThirdNotice.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "RevalidationThirdNotice.txt");
                Fields = fields;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool RevalidationFinalNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                string MMISproviderTypeID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsTaxonomy = ExecuteStoredProcedure("usp_SelectREG_TAXONOMY", parms);
                string taxonomy = string.Empty;
                if (dsTaxonomy.Tables.Count > 0 && dsTaxonomy.Tables[0].Rows.Count > 0)
                {
                    taxonomy = dsTaxonomy.Tables[0].Rows[0]["TAXONOMY_NAME"].ToString();
                }
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                //int toPartyId = GetAdminPartyId();
                if (hasRows)
                {
                    //int partyId = Methods.GetIntValue(dsProv.Tables[0].Rows[0]["PARTY_ID"]);
                    //if (partyId > 0) toPartyId = partyId;
                    fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    DateTime endDate;
                    string endDateString = "";
                    bool isValidEndDate = false;
                    if (DateTime.TryParse(dsProv.Tables[0].Rows[0]["END_DATE"].ToString(), out endDate))
                    {
                        isValidEndDate = true;
                        endDateString = endDate.ToString("MM-dd-yyyy");
                    }
                    else
                    {
                        isValidEndDate = false;
                        endDateString = "";
                    }
                    fields.Add("LICENSEREVALIDATIONDUE", endDateString);


                    string ToName = dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString() == "" ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString();


                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, taxonomy, regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"), endDateString, "Re-Enrollment Due Date");


                    fields.Add("REINSTATEDATE", isValidEndDate ? endDate.AddYears(2).ToString("MM-dd-yyyy") : "(END DATE not found)");

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("PROVIDER_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "PROVIDER_TYPE_ID")));
                    parameters.Add(new SqlParameter("ENTITY_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "ENTITY_TYPE_ID")));
                    parameters.Add(new SqlParameter("APPLICATION_TYPE_ID", Methods.GetIntValue(dsProv.Tables[0].Rows[0], "APPLICATION_TYPE_ID")));
                    DataSet dsAppFees = ExecuteStoredProcedure("usp_SelectProvider_Type_Fee", parameters);
                    if (Methods.HasRows(dsAppFees))
                    {
                        string appfeeDetail = "<p>Additionally, your provider type (organizational/agency) must pay an application fee at the time of revalidation as required by 42 CFR 455.460 Application fee. If your organization is enrolled with Medicare and/or another state’s Medicaid agency, you will not pay the fee again but will be required to submit proof of payment. You will be prompted through these questions as you begin the revalidation process on-line.</p>";
                        fields.Add("ShowApplicationFeeDetail", appfeeDetail);
                    }
                    else
                        fields.Add("ShowApplicationFeeDetail", string.Empty);

                    if (isCredentialingRequired(regId))
                    {
                        string credentialDetail = "<p>During the revalidation process, recredentialing is also required. In order to complete the revalidation and recredentialing process, please submit your information at least 30 days prior to your revalidation due date. Late submission of information may result in termination and require the enrollment/credentialing process to be restarted.</p>";
                        fields.Add("ShowCredentialDetail", credentialDetail);
                    }
                    else
                    {
                        fields.Remove("ShowCredentialDetail");
                    }

                    MMISproviderTypeID = dsProv.Tables[0].Rows[0]["MMIS_PROVIDER_TYPE_ID"].ToString();

                }
                else
                {
                    return false;
                }
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                //fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                // OHPNM-15336,15337 - Send a different 30 day notice for PT types 01, 02, 03, 28, 86, 88, 89 // SAM635 ADD 59,74 AND 10 TO THIS LIST
                if (MMISproviderTypeID == "01" || MMISproviderTypeID == "02" || MMISproviderTypeID == "03" || MMISproviderTypeID == "28" || MMISproviderTypeID == "86" ||
                     MMISproviderTypeID == "88" || MMISproviderTypeID == "89" || MMISproviderTypeID == "59" || MMISproviderTypeID == "74" || MMISproviderTypeID == "10")
                    body = SendNotification(templateActualPath + @"/Revalidation30DaysSuspendedProviders.txt", fields, true);
                else
                    body = SendNotification(templateActualPath + @"/RevalidationFinalNotice.txt", fields, true);

                CreateCommunicationEvent(fields, new Guid(), regId, "RevalidationFinalNotice.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool RevalidationOverDueTerminateProviderNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    templateActualPath = @"C:\inetpub\wwwroot\PDMS\OH_PNM_SVC_DEV\Documents";
                }

                string templateName = "RevalidationOverDueTerminateProviderNotice.txt";
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsTaxonomy = ExecuteStoredProcedure("usp_SelectREG_TAXONOMY", parms);
                string taxonomy = string.Empty;
                if (dsTaxonomy.Tables.Count > 0 && dsTaxonomy.Tables[0].Rows.Count > 0)
                {
                    taxonomy = dsTaxonomy.Tables[0].Rows[0]["TAXONOMY_NAME"].ToString();
                }
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                bool isCredentialingProvider = isCredentialingRequired(regId);

                if (hasRows)
                {
                    DataRow provider = dsProv.Tables[0].Rows[0];
                    fields.Add("LEGALNAME", provider["NAME"].ToString());
                    fields.Add("NPI", provider["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));

                    DateTime endDate;
                    string endDateString = "";
                    bool isValidEndDate = false;
                    if (DateTime.TryParse(provider["END_DATE"].ToString(), out endDate))
                    {
                        isValidEndDate = true;
                        endDateString = endDate.ToString("MM-dd-yyyy");
                    }
                    else
                    {
                        isValidEndDate = false;
                        endDateString = "";
                    }
                    fields.Add("REVALIDATIONDUE", endDateString);


                    string ToName = provider["CONTACT_NAME"].ToString() == "" ? provider["NAME"].ToString() : provider["CONTACT_NAME"].ToString();


                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                                         dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                                         dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                                         (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                                             : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, taxonomy, regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"), endDateString, "Re-Enrollment Due Date");


                    fields.Add("REINSTATEDATE", isValidEndDate ? endDate.AddYears(2).ToString("MM-dd-yyyy") : "(END DATE not found)");

                }
                else
                {
                    return false;
                }
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                if (isCredentialingProvider)
                {
                    templateName = "RevalidationOverDueTerminateProviderNoticeCredentialed.txt";
                }
                templateActualPath = $@"{templateActualPath}\{templateName}";

                body = SendNotification(templateActualPath, fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, templateName);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendInactiveProviderTerminationNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsTaxonomy = ExecuteStoredProcedure("usp_SelectREG_TAXONOMY", parms);
                string taxonomy = string.Empty;
                if (dsTaxonomy.Tables.Count > 0 && dsTaxonomy.Tables[0].Rows.Count > 0)
                {
                    taxonomy = dsTaxonomy.Tables[0].Rows[0]["TAXONOMY_NAME"].ToString();
                }
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    DateTime endDate;
                    string endDateString = "";
                    if (DateTime.TryParse(dsProv.Tables[0].Rows[0]["END_DATE"].ToString(), out endDate))
                    {
                        endDateString = endDate.ToString("MM-dd-yyyy");
                    }
                    else
                    {
                        endDateString = "";
                    }
                    fields.Add("ENDDATE", endDateString);

                    string ToName = dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString() == "" ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString();


                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, taxonomy, regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"), endDateString, "Re-Enrollment Due Date");


                }
                else
                {
                    return false;
                }
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                body = SendNotification(templateActualPath + @"/InactivtyNoClaimTerminationNotice.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "InactivtyNoClaimTerminationNotice.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SuspensionUponIndictmentLetter(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                //int toPartyId = GetAdminPartyId();
                if (hasRows)
                {
                    //int partyId = Methods.GetIntValue(dsProv.Tables[0].Rows[0]["PARTY_ID"]);
                    //if (partyId > 0) toPartyId = partyId;
                    fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("CountyName", dsProv.Tables[0].Rows[0]["CONTACT_COUNTY"].ToString());
                    GetFromAddress(fields);
                    string ToName = dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString() == "" ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                    dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                    dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                    (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                    : string.Empty), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, "", regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"));

                }
                else
                {
                    return false;
                }
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                body = SendNotification(templateActualPath + @"/SuspensionUponIndictmentLetter.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "SuspensionUponIndictmentLetter.txt");
                Fields = fields;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SubmitCRNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (hasRows)
                {
                    //DataRow row = ds.Tables[0].Rows[0];
                    foreach (DataRow row in dsProv.Tables[0].Rows)
                    {
                        GetFromAddress(fields);
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                            row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                            row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                            (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                        fields.Add("Medicaid_ID", medicaidID);
                        fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                        body = SendNotification(templateActualPath + @"/SubmitCRNotice.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "SubmitCRNotice.txt");

                        //fields.Clear();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool NotProcessedNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    templateActualPath = @"C:\inetpub\wwwroot\PDMS\OH_PNM_SVC_DEV\Documents";
                }
                Dictionary<string, object> fields = new Dictionary<string, object>();
                string templateName = @"\NotProcessedNotice.txt";
                templateActualPath = $"{templateActualPath}{templateName}";

                fields.Add("REGID", regId);
                fields.Add("DATE", DateTime.Now);
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(new SqlParameter("RegID", regId));
                DataSet dsReg = ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms);
                DataRow drReg = dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0 ? dsReg.Tables[0].Rows[0] : null;
                int WorkflowID = Convert.ToInt32(drReg["WorkflowID"]);
                int workFlowEventId = Convert.ToInt32(drReg["WORKFLOW_EVENT_TYPE_ID"]);
                bool IsProviderReactivation = Methods.GetIntValue(drReg, "IsProviderReactivation") == 1 ? true : false;
                bool IsReapplication = Methods.GetIntValue(drReg, "isreapplication") == 1 ? true : false;

                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                string endDateString = "";
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    DateTime endDate;

                    if (DateTime.TryParse(dsProv.Tables[0].Rows[0]["END_DATE"].ToString(), out endDate))
                    {
                        endDateString = endDate.ToString("MM-dd-yyyy");
                    }
                    else
                    {
                        endDateString = "";
                    }
                    fields.Add("REVALIDATIONDUE", endDateString);
                }
                if (AppSettings.Get("SAM615Enabled") == "true")
                {
                    fields.Add("returnedOrCancelled", "cancelled");
                }
                else
                {
                    fields.Add("returnedOrCancelled", "returned");
                }
                if (AppSettings.Get("SAM615Enabled") == "true" && workFlowEventId == CON.WorkflowEventType.RevalReg
                    && !IsReapplication && !IsReapplication)
                {
                    fields.Add("revalidationdate", " by " + endDateString);
                }
                else
                {
                    fields.Add("revalidationdate", "");
                }
                subject = "Not Processed";
                body = SendNotification(templateActualPath, fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, templateName);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool InformationRecievedNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (hasRows)
                {
                    //DataRow row = ds.Tables[0].Rows[0];
                    foreach (DataRow row in dsProv.Tables[0].Rows)
                    {
                        GetFromAddress(fields);
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                            row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                            row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                            (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                        fields.Add("Medicaid_ID", medicaidID);
                        fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                        body = SendNotification(templateActualPath + @"/PerseusInformationReceivedNotice.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "PerseusInformationReceivedNotice.txt");

                        //fields.Clear();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool ReportRejectionNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (hasRows)
                {
                    //DataRow row = ds.Tables[0].Rows[0];
                    foreach (DataRow row in dsProv.Tables[0].Rows)
                    {
                        GetFromAddress(fields);
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                            row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                            row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                            (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                        fields.Add("Medicaid_ID", medicaidID);
                        fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                        body = SendNotification(templateActualPath + @"/ReportRejectionNotice.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "ReportRejectionNotice.txt");

                        //fields.Clear();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool ReportSubmissionNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (hasRows)
                {
                    //DataRow row = ds.Tables[0].Rows[0];
                    foreach (DataRow row in dsProv.Tables[0].Rows)
                    {
                        GetFromAddress(fields);
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                            row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                            row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                            (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                        fields.Add("Medicaid_ID", medicaidID);
                        fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                        body = SendNotification(templateActualPath + @"/ReportSubmissionNotice.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "ReportSubmissionNotice.txt");

                        //fields.Clear();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool CPCReportNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (hasRows)
                {
                    //DataRow row = ds.Tables[0].Rows[0];
                    foreach (DataRow row in dsProv.Tables[0].Rows)
                    {
                        GetFromAddress(fields);
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                            row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                            row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                            (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                        fields.Add("Medicaid_ID", medicaidID);
                        fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                        body = SendNotification(templateActualPath + @"/CPCReportNotice.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "CPCReportNotice.txt");

                        //fields.Clear();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool LicenseRevalidationInitialNotice(string regId)
        {
            try
            {
                var templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                var fields = new Dictionary<string, object>();
                var parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                var dsLic = ExecuteStoredProcedure("usp_SelectREG_LICENSES", parms);
                var hasRows = dsLic.Tables.Count > 0 && dsLic.Tables[0].Rows.Count > 0;
                var parms1 = new List<SqlParameter>();
                parms1.Add(new SqlParameter("REG_ID", regId));
                var ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms1);
                fields.Add("LEGALNAME", ds.Tables[0].Rows[0]["NAME"].ToString());
                fields.Add("NPI", ds.Tables[0].Rows[0]["NPI"].ToString());
                fields.Add("TAXID", ds.Tables[0].Rows[0]["TAX_ID"].ToString());
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                fields.Add("REVALIDATIONDUE", string.Empty);
                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                var dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms1);
                var medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    var row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];

                    var ToName = row["CONTACT_NAME"].ToString() == ""
                        ? row["NAME"].ToString()
                        : row["CONTACT_NAME"].ToString();

                    setPaperProviderInformation(ToName, ds.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), ds.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     ds.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), ds.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     ds.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + ds.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + ds.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(ds.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(ds.Tables[0].Rows[0], "NPI"), medicaidID, "", regId, Methods.GetStringValue(ds.Tables[0].Rows[0], "DBA"));

                }
                else
                {
                    return false;
                }

                if (hasRows)
                {
                    foreach (DataRow dr in dsLic.Tables[0].Rows)
                    {
                        DateTime endDate;
                        var endDateString = "";
                        if (DateTime.TryParse(dr["LICENSE_END_DATE"].ToString(), out endDate))
                            endDateString = endDate.ToString("MM-dd-yyyy");
                        else
                            endDateString = "(END DATE not found)";
                        //if (endDate != null && endDate ==
                        //    DateTime.Now.Date.AddDays(Convert.ToInt32(AppSettings.Get("LicenseDueInitialNoticeDays"))))
                        //{
                        if (Methods.GetStringValue(dr, "LICENSE_STATE") != "OH" || (Methods.GetStringValue(dr, "LICENSE_STATE") == "OH" && Methods.GetIntValue(dr, "LICENSE_TYPE_ID") == 6))
                        {
                            fields.Add("LICENSEENDDATE", endDateString);
                            fields.Add("LICENSEREVALIDATIONDUE", string.Empty);
                            body = SendNotification(templateActualPath + @"/LicenseRevalidationInitialNotice.txt", fields, true);
                            CreateCommunicationEvent(fields, new Guid(), regId, "LicenseRevalidationInitialNotice.txt");
                            fields.Remove("LICENSEENDDATE");
                            fields.Remove("CURRENTDATE");
                            fields.Remove("FONTSIZE");
                            fields.Remove("DISPLAYOMR");
                        }
                        //}
                    }
                }
                else
                {
                    return false;
                }
                Fields = fields;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool LicenseRevalidationSecondNotice(string regId)
        {

            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_LICENSES", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                //int toPartyId = GetAdminPartyId();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                GetFromAddress(fields);
                if (hasRows)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    foreach (DataRow dr in dsProv.Tables[0].Rows)
                    {
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                            row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                            row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                            (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));

                        DateTime endDate;
                        string endDateString = "";
                        if (DateTime.TryParse(dr["LICENSE_END_DATE"].ToString(), out endDate))
                            endDateString = endDate.ToString("MM-dd-yyyy");
                        else
                            endDateString = "(END DATE not found)";
                        if (endDate != null && endDate == DateTime.Now.Date.AddDays(Convert.ToInt32(AppSettings.Get("LicenseDue2ndNoticeDays"))))
                        {
                            fields.Add("LICENSEREVALIDATIONDUE", endDateString);
                            fields.Add("LICENSEENDDATE", endDate.AddDays(1).ToShortDateString());
                            //fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                            fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                            fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                            fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                            body = SendNotification(templateActualPath + @"/LicenseRevalidationSecondNotice.txt", fields, true);
                            CreateCommunicationEvent(fields, new Guid(), regId, "LicenseRevalidationSecondNotice.txt");
                        }
                        //fields.Clear();
                    }
                }
                else
                {
                    return false;
                }
                Fields = fields;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool LicenseInactiveTerminationNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (hasRows)
                {
                    //DataRow row = ds.Tables[0].Rows[0];
                    foreach (DataRow row in dsProv.Tables[0].Rows)
                    {

                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                        setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, "", regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"));

                        fields.Add("Medicaid_ID", medicaidID);
                        fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                        body = SendNotification(templateActualPath + @"/LicenseInactiveTerminationLetter.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "LicenseInactiveTerminationLetter.txt");

                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool LicenseTerminationNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (hasRows)
                {
                    DataRow row = dsProv.Tables[0].Rows[0];
                    

                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, "", regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"));


                    fields.Add("Medicaid_ID", medicaidID);
                    fields.Add("ProviderName", ToName);
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    body = SendNotification(templateActualPath + @"/LicenseTermination.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "LicenseTermination.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool LicenseDenialNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (hasRows)
                {
                    DataRow row = dsProv.Tables[0].Rows[0];

                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, "", regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"));

                    fields.Add("Medicaid_ID", medicaidID);
                    fields.Add("ProviderName", ToName);
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    body = SendNotification(templateActualPath + @"/DenialLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "DenialLetter.txt");
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool PASRRNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    templateActualPath = @"C:\OHPNM\ohpnm-src-p3\PDMS\ProviderDataManagementSystemService\Documents";
                }
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                bool hasRows1 = dsServiceLoc.Tables.Count > 0 && dsServiceLoc.Tables[0].Rows.Count > 0;
                if (hasRows1)
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");

                    string ToName = "";
                    string contactName = "";
                    if (hasRows)
                    {
                        DataRow dr = dsProv.Tables[0].Rows[0];
                        contactName = dr["CONTACT_NAME"].ToString();
                        if (!string.IsNullOrEmpty(contactName))
                            contactName = contactName.Trim();
                        ToName = contactName == "" ? dr["NAME"].ToString() : contactName;
                    }

                    fields.Add("MedicaidProviderID", ToName + " (" + medicaidID + ")");
                    fields.Add("DATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    body = SendNotification(templateActualPath + @"\PreAdmissionScreeningAndResidentReview.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "PreAdmissionScreeningAndResidentReview.txt");
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool InactiveOrLapsedLicenseTerminationNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (hasRows)
                {
                    DataRow row = dsProv.Tables[0].Rows[0];

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                    fields.Add("Medicaid_ID", medicaidID);
                    fields.Add("MEDICAID", medicaidID);
                    fields.Add("Name", ToName);
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    body = SendNotification(templateActualPath + @"/LapsedLicenseTermination.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "LapsedLicenseTermination.txt");

                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool CertificationTerminationNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (hasRows)
                {
                    DataRow row = dsProv.Tables[0].Rows[0];

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                    fields.Add("Medicaid_ID", medicaidID);
                    fields.Add("MEDICAID", medicaidID);
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    body = SendNotification(templateActualPath + @"/CertificationTermination.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "CertificationTermination.txt");
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendScreeningAutoTerminationNotice(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);               
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsTaxonomy = ExecuteStoredProcedure("usp_SelectREG_TAXONOMY", parms);
                string taxonomy = string.Empty;
                if (dsTaxonomy.Tables.Count > 0 && dsTaxonomy.Tables[0].Rows.Count > 0)
                {
                    taxonomy = dsTaxonomy.Tables[0].Rows[0]["TAXONOMY_NAME"].ToString();
                }

                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("Name", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));

                    DateTime endDate;
                    string endDateString = "";
                    if (DateTime.TryParse(dsProv.Tables[0].Rows[0]["END_DATE"].ToString(), out endDate))
                    {
                        endDateString = endDate.ToString("MM-dd-yyyy");
                    }
                    else
                    {
                        endDateString = "";
                    }
                    fields.Add("REVALIDATIONDUE", endDateString);

                    string ToName = dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString() == "" ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString();


                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, taxonomy, regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"), endDateString, "Re-Enrollment Due Date");

                    fields.Add("TAXID", dsProv.Tables[0].Rows[0]["TAX_ID"].ToString());

                }
                else
                {
                    return false;
                }
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                //AddLicenseInformationForProvider(regId);
                AddEmailAddressForProvider(regId);
                body = SendNotification(templateActualPath + @"/FederalExclusionInterfaces.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "FederalExclusionInterfaces.txt");
                Fields = fields;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool AutoClosureNotice(string regId, bool isRTP = false)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                int daysElapsed = 0;
                int applicationTypeId = 0;
                int elapsedHours = 0;
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                //int toPartyId = GetAdminPartyId();
                if (hasRows)
                {
                    DataRow row = dsProv.Tables[0].Rows[0];
                    fields.Add("CLOSUREDATE", DateTime.Now.ToString("MM-dd-yyyy"));

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    fields.Add("CONTACT_STATE", row["CONTACT_STATE"].ToString());
                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                    //CR -59 //Get application type and then get elapsed time for auto termination
                    applicationTypeId = Methods.GetIntValue(row, "APPLICATION_TYPE_ID");
                    parms.Clear();
                    if (!isRTP)
                    {
                        parms.Add(new SqlParameter("APPLICATION_TYPE_ID", applicationTypeId));
                        DataSet dsElapsedTime = new DataSet();
                        dsElapsedTime = ExecuteStoredProcedure("usp_SelectREG_ElapsedTimeByApplicationType", parms);

                        if (dsElapsedTime != null && dsElapsedTime.Tables.Count > 0 && dsElapsedTime.Tables[0].Rows.Count > 0)
                        {
                            elapsedHours = Convert.ToInt32(dsElapsedTime.Tables[0].Rows[0]["ELAPSED_EVENT_TIME_HOURS"]);
                            if (elapsedHours > 0)
                            {
                                daysElapsed = Decimal.ToInt32(elapsedHours / 24);
                            }
                        }
                    }
                    else
                    {
                        string RTPdueDays = AppSettings.Get("RTPDueWindow");
                        daysElapsed = Convert.ToInt32(RTPdueDays);
                    }
                    fields.Add("ProviderName", ToName);
                    fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("TIME_ELAPSE_DAYS", daysElapsed.ToString());
                    body = SendNotification(templateActualPath + @"/ClosureNotice.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ClosureNotice.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool SendPCGSiteVisit(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                string RiskLevel = "";
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;

                string medicaidID = GetMedicaidID(regId);

                if (Methods.HasRows(dsProv))
                {
                    DataRow row = dsProv.Tables[0].Rows[0];
                    RiskLevel = row["PROVIDER_RISK_LEVEL_NAME"].ToString();


                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }

                fields.Add("NAME", hasRows ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : "(Provider Name not found)");
                fields.Add("REG_ID", regId);
                              
                subject = "Registration requiring site visit assignment";

                body = SendNotification(templateActualPath + @"/SiteVistAssignment.txt", fields, true);
                string recipients = GetRecipients(Constants.SendToTypeID.PCGNotificationEmailGroup, Convert.ToInt32(regId));
                EMailNotification enotify = new EMailNotification(body, subject, recipients, ThreadId);
                CreateCommunicationEvent(fields, new Guid(), regId, "SiteVistAssignment.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendDHCPSiteVisitPassedNotification(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;

                string medicaidID = GetMedicaidID(regId);

                if (Methods.HasRows(dsProv))
                {
                    DataRow row = dsProv.Tables[0].Rows[0];

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }

                fields.Add("LEGALNAME", hasRows ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : "(Provider Name not found)");
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                fields.Add("LOGIN_URL", AppSettings.Get("PDMS-URL", string.Empty));

                body = SendNotification(templateActualPath + @"/DHCFSitevisitPassedNotification.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "DHCFSitevisitPassedNotification.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendNoticeToLTC(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                //int toPartyId = GetAdminPartyId();

                string medicaidID = GetMedicaidID(regId);

                if (Methods.HasRows(dsProv))
                {
                    DataRow row = dsProv.Tables[0].Rows[0];

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }

                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                fields.Add("LOGIN_URL", AppSettings.Get("PDMS-URL", string.Empty));

                body = SendNotification(templateActualPath + @"/SendNoticeToLTC.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "SendNoticeToLTC.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendProviderApprovalNoticeToLTC(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                //int toPartyId = GetAdminPartyId();

                string medicaidID = GetMedicaidID(regId);

                if (Methods.HasRows(dsProv))
                {
                    DataRow row = dsProv.Tables[0].Rows[0];

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }
                fields.Add("LEGALNAME", hasRows ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : "(Provider Name not found)");
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                fields.Add("LOGIN_URL", AppSettings.Get("PDMS-URL", string.Empty));

                body = SendNotification(templateActualPath + @"/SendProviderApprovalNoticeToLTC.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "SendProviderApprovalNoticeToLTC.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool SendAwarenessNotification(string regId, string txtName = "")
        {
            try
            {
                bool fnd = false;
                string RiskLevel = "";
                string ProviderRiskLevel = string.Empty;
                string PROVIDERNAME = string.Empty; // jira 2194
                string BUMPUPREASON = string.Empty; // jira 2194
                string EffectiveDate = "";

                //int toPartyId = GetAdminPartyId();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
              

                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    //int partyId = Methods.GetIntValue(ds.Tables[0].Rows[0]["PARTY_ID"]);
                    //if (partyId > 0) toPartyId = partyId;                    
                    ProviderRiskLevel = row["PROVIDER_RISK_LEVEL_ID"].ToString();
                    if (ProviderRiskLevel == CON.ProviderRiskLevel.RiskUndefined.ToString())
                    {
                        RiskLevel = "Undefined";
                    }
                    else if (ProviderRiskLevel == CON.ProviderRiskLevel.RiskLimited.ToString())
                    {
                        RiskLevel = "Limited";
                    }
                    else if (ProviderRiskLevel == CON.ProviderRiskLevel.RiskModerate.ToString())
                    {
                        RiskLevel = "Moderate";
                    }
                    else if (ProviderRiskLevel == CON.ProviderRiskLevel.RiskHigh.ToString())
                    {
                        RiskLevel = "High";
                    }
                    PROVIDERNAME = row["NAME"].ToString();                // jira 2194
                    BUMPUPREASON = row["BUMP_UP_REASON_DESC"].ToString(); // jira 2194

                    subject = subject.Replace("Risk Level", RiskLevel + " Risk Level"); //to change to Moderate or High Risk Level

                    GetFromAddress(fields);

                    fnd = true;
                }
                else
                {
                    return false;
                }
                if (fnd && (RiskLevel == "Limited"))
                {
                    subject = AppSettings.Get("StateName", "(State)") + " Medicaid Provider Application Received";
                    bool hasRows = ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CHANGE_EFFECTIVE_DATE"].ToString()))
                    {
                        DateTime eff = Convert.ToDateTime(ds.Tables[0].Rows[0]["CHANGE_EFFECTIVE_DATE"].ToString());
                        EffectiveDate = eff.ToString("MM/dd/yyyy");
                    }


                    if (Methods.HasRows(ds))
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();


                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                                          row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                                          row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                                          (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                                              : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, ds.Tables[0].Rows[0]["TAXONOMY_CODE"].ToString(), regId, Methods.GetStringValue(row, "DBA"), EffectiveDate);
                    }

                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));

                    fields.Add("PROVIDERNAME", PROVIDERNAME); // jira 2194
                    fields.Add("BUMPUPREASON", BUMPUPREASON); // jira 2194

                    AddBookmark(fields, "TAXONOMY", ds.Tables[0].Rows[0]["TAXONOMY_CODE"].ToString());

                    // jira 2194
                    if (txtName == string.Empty)
                    {
                        body = SendNotification(templateActualPath + @"/AwarenessLetter" + RiskLevel + "Risk.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "AwarenessLetter" + RiskLevel + "Risk.txt");
                    }
                    else
                    {
                        subject = "Notification of Increased Risk Level for Ohio Medicaid";
                        body = SendNotification(templateActualPath + @"/" + txtName, fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, txtName);
                    }

                }
                else if (fnd && (RiskLevel == "Moderate" || RiskLevel == "High"))            // Need basic information in order to send email
                {
                    if (Methods.HasRows(ds))
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                           row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                           row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                           (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                               : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                    }
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));

                    fields.Add("PROVIDERNAME", PROVIDERNAME); // jira 2194
                    fields.Add("BUMPUPREASON", BUMPUPREASON); // jira 2194

                    // jira 2194
                    if (txtName == string.Empty)
                    {
                        body = SendNotification(templateActualPath + @"/AwarenessLetter" + RiskLevel + "Risk.txt", fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, "AwarenessLetter" + RiskLevel + "Risk.txt");
                    }
                    else
                    {
                        subject = "Notification of Increased Risk Level for Ohio Medicaid";
                        body = SendNotification(templateActualPath + @"/" + txtName, fields, true);
                        CreateCommunicationEvent(fields, new Guid(), regId, txtName);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEmailAddressForProvider(string regId)
        {
            if (!Fields.ContainsKey("EMAILADDRESS"))
            {
                var emailAddress = string.Empty;
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("SendToTypeID", 1),
                    new SqlParameter("RegId", regId)
                };
                var dsEmail = ExecuteStoredProcedure("usp_SelectEmailRecipients", parms);
                if (!Methods.HasRows(dsEmail)) return;
                foreach (DataRow row in dsEmail.Tables[0].Rows)
                {
                    emailAddress = Methods.GetStringValue(row, "EmailAddress");
                    if (!String.IsNullOrEmpty(emailAddress)) break;
                }

                Fields.Add("EMAILADDRESS", emailAddress);
            }
        }

        private void AddLicenseInformationForProvider(string regId)
        {
            List<SqlParameter> parms = new List<SqlParameter> { new SqlParameter("REG_ID", regId) };
            var dsLic = ExecuteStoredProcedure("usp_SelectREG_LICENSES", parms);
            if (!Methods.HasRows(dsLic)) return;
            var row = dsLic.Tables[0].Rows[0];
            Fields.Add("LICENSESTATE", Methods.GetStringValue(row, "LICENSE_STATE"));
            Fields.Add("LICENSENUMBER", Methods.GetStringValue(row, "LICENSE_NUMBER"));
            Fields.Add("LICENSEENDDATE", Methods.GetStringValue(row, "LICENSE_END_DATE"));
        }

        protected DataSet ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters)
        {
            DataSet returnData = new DataSet();
            Exception thrownException = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(AppSettings.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    foreach (SqlParameter prm in parameters)
                    {
                        cmd.Parameters.Add(prm);
                    }

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(returnData);
                }
            }
            catch (Exception ex)
            {
                thrownException = ex;
                throw ex;
            }
            finally
            {
            }
            return (returnData);
        }

        public static string ExecuteScalar(string storedProcedureName, List<SqlParameter> inputParameters)
        {
            string returnData = string.Empty;
            Exception thrownException = null;

            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    foreach (SqlParameter prm in inputParameters)
                    {
                        cmd.Parameters.Add(prm);
                    }
                    returnData = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                thrownException = ex;
                throw ex;
            }
            finally
            {
            }
            return (returnData);
        }

        public static string GetKeyValueString(Dictionary<string, object> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, object> entry in dict)
            {
                sb.Append(entry.Key + ":" + entry.Value.ToString() + ",");
            }

            string keyValue = sb.ToString();

            if (!string.IsNullOrEmpty(keyValue))
            {
                keyValue = keyValue.Remove(keyValue.Length - 1); //remove the final comma
            }

            return keyValue;
        }

        // Add the notes that belong to the specific Page Type Id
        private void AddNotes(int regPageTypeId, DataTable dtNotes, ref StringBuilder sb)
        {
            if (dtNotes == null) return;

            foreach (DataRow row in dtNotes.Rows)
            {
                if (regPageTypeId == Convert.ToInt32(row["REG_PAGE_TYPE_ID"]))
                {
                    sb.AppendLine("&nbsp;&nbsp;&nbsp;-&nbsp;" + row["NOTE_TEXT"].ToString());
                }
            }
        }

        private string GetReturnReasons(string regId)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("REG_ID", regId));
            parms.Add(new SqlParameter("REG_PAGE_TYPE_ID", -999));      // Capture all
            parms.Add(new SqlParameter("IncludePartyErrors", true));
            DataSet dsErr = ExecuteStoredProcedure("usp_SelectREG_ERRORcustom", parms);
            if (dsErr.Tables.Count == 0) return string.Empty;
            if (dsErr.Tables[0].Rows.Count == 0) return string.Empty;

            int regPageTypeId = 0;
            StringBuilder sb = new StringBuilder();
            // Go get the notes
            parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("REG_ID", regId));
            DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER_NOTECustom", parms);
            DataTable dtNotes = null;
            if (ds.Tables.Count > 0) dtNotes = ds.Tables[0].Select("PROVIDER_NOTE_TYPE_ID = " +
                Constants.ProviderNoteTypeId.RegistrationRejection.ToString()).CopyToDataTable();

            foreach (DataRow row in dsErr.Tables[0].Rows)
            {
                if (regPageTypeId != Convert.ToInt32(row["REG_PAGE_TYPE_ID"]))
                {
                    AddNotes(regPageTypeId, dtNotes, ref sb);
                    regPageTypeId = Convert.ToInt32(row["REG_PAGE_TYPE_ID"]);
                }
                sb.AppendLine(row["ERROR_CODE"].ToString() + " - " + row["ERROR_NAME"].ToString());
            }
            AddNotes(regPageTypeId, dtNotes, ref sb);

            if (dsErr.Tables.Count > 1)
            {
                foreach (DataRow row in dsErr.Tables[1].Rows)
                    sb.AppendLine(row["ERROR_CODE"].ToString() + " - " + row["ERROR_NAME"].ToString());
            }
            return sb.ToString();
        }

        private string CONNSTR
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["mainDB"].ToString();
            }
        }


        private bool SendNotificationToDDS(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                string medicaidID = GetMedicaidID(regId);

                if (Methods.HasRows(dsProv))
                {
                    DataRow row = dsProv.Tables[0].Rows[0];

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                fields.Add("LOGIN_URL", AppSettings.Get("PDMS-URL", string.Empty));
                body = SendNotification(templateActualPath + @"/DDSApplicationPendingReviewNotification.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "DDSApplicationPendingReviewNotification.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool SendReturnToStateNotification(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                List<SqlParameter> parms2 = new List<SqlParameter>();
                parms2.Add(new SqlParameter("RegID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                DataSet dsReg = ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegID", parms2);

                string medicaidID = GetMedicaidID(regId);

                if (Methods.HasRows(dsProv))
                {
                    DataRow row = dsProv.Tables[0].Rows[0];

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                fields.Add("LOGIN_URL", AppSettings.Get("PDMS-URL", string.Empty));
                body = SendNotification(templateActualPath + @"/ReturnToStateNotification.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "ReturnToStateNotification.txt");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetMedicaidID(string regID)
        {
            string rtn = string.Empty;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("REG_ID", regID));

            DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
            string medicaidID = string.Empty;
            if (Methods.HasRows(dsServiceLoc))
            {
                DataRow row = dsServiceLoc.Tables[0].Rows[0];
                medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
            }

            return rtn;
        }

        private bool SendDBHNotification(string regId)
        {
            try
            {

                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                string medicaidID = GetMedicaidID(regId);

                if (Methods.HasRows(dsProv))
                {
                    DataRow row = dsProv.Tables[0].Rows[0];

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                }
                else
                {
                    return false;
                }
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                fields.Add("LOGIN_URL", AppSettings.Get("PDMS-URL", string.Empty));

                body = SendNotification(templateActualPath + @"/DBHApplicationPendingReviewNotification.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "DBHApplicationPendingReviewNotification.txt");
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendORFANotification(string regId)
        {
            try
            {

                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));

                body = SendNotification(templateActualPath + @"/ORFANotification.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "ORFANotification.txt");
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendRetroEffectiveDateNotification(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                bool hasRows = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    fields.Add("LEGAL_NAME", dr["NAME"].ToString());
                    fields.Add("NPI", dr["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    DateTime tmp;
                    if ((DateTime.TryParse(dr["REQUESTED_EFFECTIVE_DATE"].ToString(), out tmp)))
                    {
                        fields.Add("REQUESTED_EFFECTIVE_DATE", tmp.ToString("MM-dd-yyyy"));
                    }
                    else
                    {
                        fields.Add("REQUESTED_EFFECTIVE_DATE", string.Empty);
                    }

                    if ((DateTime.TryParse(dr["SUBMIT_DATE_TIME"].ToString(), out tmp)))
                    {
                        fields.Add("SUBMIT_DATE_TIME", tmp.ToString("MM-dd-yyyy"));
                    }
                    else
                    {
                        fields.Add("SUBMIT_DATE_TIME", string.Empty);
                    }

                    fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                    body = SendNotification(templateActualPath + @"/ReturnToStateRetroEffectiveDate.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ReturnToStateRetroEffectiveDate.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendGroupMemberRetroEffectiveDateNotification(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                bool hasRows = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    fields.Add("LEGAL_NAME", dr["NAME"].ToString());
                    fields.Add("NPI", dr["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    //add member details
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(new SqlParameter("REG_ID", regId));
                    DataSet dsReg = ExecuteStoredProcedure("usp_SelectREG_AFFILIATION", sqlParms);

                    dsReg.Tables[0].TableName = "RegData";
                    DataTable dtAffiliations = null;
                    string filter = "";

                    if (dsReg.Tables[0].Rows != null && dsReg.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] rows = dsReg.Tables[0].Select(filter, "NAME ASC");
                        if (rows.Length > 0)
                        {
                            dtAffiliations = rows.CopyToDataTable();
                        }
                    }
                    DataRow[] drReg = (dsReg.Tables[0].Rows != null && dsReg.Tables[0].Rows.Count > 0) ? dtAffiliations.Select("RETRO_REVIEW_REQUIRED_ID = '" + CON.GroupMemberRetroStatusID.RetroReviewRequired + "'") : null;
                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow dr1 in drReg)
                    {
                        string memberDetails = "";
                        memberDetails = dr1["Name"].ToString() + ":       " + "Requested Effective Date :" + Convert.ToDateTime(dr1["start_date"]).ToShortDateString() + "</br>";
                        sb.AppendLine(memberDetails);
                    }
                    fields.Add("MemberDetails", sb);
                    fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                    body = SendNotification(templateActualPath + @"/ReturnToStateMemberRetroEffectiveDate.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ReturnToStateMemberRetroEffectiveDate.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendApplicationFeeWaiverHardshipNotification(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectApplication_FeeWaiverHardship_MATCHByRegID", parms);

                bool hasRows = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    fields.Add("LEGAL_NAME", dr["NAME"].ToString());
                    fields.Add("NPI", dr["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));

                    body = SendNotification(templateActualPath + @"/ReturnToStateApplicationFeeWaiverHardship.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ReturnToStateApplicationFeeWaiverHardship.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendExclusionLetter(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectSCR_MORATORIA_EXCLUSION_MATCHByRegID", parms);

                bool hasRows = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    fields.Add("LEGAL_NAME", dr["NAME"].ToString());
                    fields.Add("NPI", dr["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("STATECONTACTPHONE", AppSettings.Get(Constants.AppSettingsKeyName.StateContactPhone, string.Empty));
                    fields.Add("PDMSURL", AppSettings.Get(Constants.AppSettingsKeyName.PDMSURL, string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));

                    parms = new List<SqlParameter>();
                    parms.Add(new SqlParameter("REG_ID", regId));

                    DataSet dscontact = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                    if (dscontact != null && dscontact.Tables.Count > 0 && dscontact.Tables[0].Rows.Count > 0)
                    {

                        DataRow row = dscontact.Tables[0].Rows[0];
                        GetFromAddress(fields);
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                       row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                       row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                       (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                           : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), "", "", regId, Methods.GetStringValue(row, "DBA"));
                    }
                    else
                    {
                        return false;
                    }
                    body = SendNotification(templateActualPath + @"/ExclusionLetter.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ExclusionLetter.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool FederalExclusionInterfaces(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);                
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectSCR_MORATORIA_EXCLUSION_MATCHByRegID", parms);

                bool hasRows = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    fields.Add("Name", dr["NAME"].ToString());
                    fields.Add("NPI", dr["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("STATECONTACTPHONE", AppSettings.Get(Constants.AppSettingsKeyName.StateContactPhone, string.Empty));
                    fields.Add("PDMSURL", AppSettings.Get(Constants.AppSettingsKeyName.PDMSURL, string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                    parms = new List<SqlParameter>();
                    parms.Add(new SqlParameter("REG_ID", regId));
                    DataSet dscontact = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                    if (dscontact != null && dscontact.Tables.Count > 0 && dscontact.Tables[0].Rows.Count > 0)
                    {

                        DataRow row = dscontact.Tables[0].Rows[0];
                        GetFromAddress(fields);
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                       row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                       row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                       (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                           : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), "", "", regId, Methods.GetStringValue(row, "DBA"));
                    }
                    else
                    {
                        return false;
                    }
                    body = SendNotification(templateActualPath + @"/FederalExclusionInterfaces.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "FederalExclusionInterfaces.txt");

                    //#region OHPNM-4561
                    //Dictionary<string, string> dictParms = new Dictionary<string, string>();
                    //dictParms.Add("REG_ID", regId.ToString());
                    //dictParms.Add("END_DATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    //dictParms.Add("ENROLLMENT_STATUS_CODE", CON.EnrollmentStatusTypeID.InActive.ToString());
                    //dictParms.Add("ENROLLMENT_STATUS_REASONS_ID", CON.EnrollStatusReasonID.TerminatedFederalExclusion.ToString());
                    //dictParms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
                    //dictParms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    //dictParms.Add("LAST_MODIFIED_USER", Guid.Parse(CON.appAdminUserId).ToString());

                    //ExecuteStoredProcedure("usp_UpdateREG_Specialty_Enroll_Status", parms);
                    //#endregion

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool DODDAbuserRegistryInterfaces(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }
                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsTaxonomy = ExecuteStoredProcedure("usp_SelectREG_TAXONOMY", parms);
                string taxonomy = string.Empty;
                if (dsTaxonomy.Tables.Count > 0 && dsTaxonomy.Tables[0].Rows.Count > 0)
                {
                    taxonomy = dsTaxonomy.Tables[0].Rows[0]["TAXONOMY_NAME"].ToString();
                }

                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("Name", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));

                    DateTime endDate;
                    string endDateString = "";
                    if (DateTime.TryParse(dsProv.Tables[0].Rows[0]["END_DATE"].ToString(), out endDate))
                    {
                        endDateString = endDate.ToString("MM-dd-yyyy");
                    }
                    else
                    {
                        endDateString = "";
                    }
                    fields.Add("REVALIDATIONDUE", endDateString);

                    string ToName = dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString() == "" ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString();

                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, taxonomy, regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"), endDateString, "Re-Enrollment Due Date");

                    // Added for Mass Email 
                    fields.Add("TAXID", dsProv.Tables[0].Rows[0]["TAX_ID"].ToString());

                }
                else
                {
                    return false;
                }

                //AddLicenseInformationForProvider(regId);
                AddEmailAddressForProvider(regId);
                body = SendNotification(templateActualPath + @"/DoddAbuserRegistryNotice.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "DoddAbuserRegistryNotice.txt");
                Fields = fields;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendProviderMoratoriaExclusionNotification(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectSCR_MORATORIA_EXCLUSION_MATCHByRegID", parms);

                bool hasRows = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    fields.Add("LEGAL_NAME", dr["NAME"].ToString());
                    fields.Add("NPI", dr["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("STATECONTACTPHONE", AppSettings.Get(Constants.AppSettingsKeyName.StateContactPhone, string.Empty));
                    fields.Add("PDMSURL", AppSettings.Get(Constants.AppSettingsKeyName.PDMSURL, string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));

                    parms = new List<SqlParameter>();
                    parms.Add(new SqlParameter("REG_ID", regId));
                    DataSet dscontact = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                    if (dscontact != null && dscontact.Tables.Count > 0 && dscontact.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = dscontact.Tables[0].Rows[0];
                        GetFromAddress(fields);
                        string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                        GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                       row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                       row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                       (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                           : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), "", "", regId, Methods.GetStringValue(row, "DBA"));

                    }
                    else
                    {
                        return false;
                    }

                    StringBuilder sb = new StringBuilder();
                    DateTime tmp;
                    DateTime? beginDate = null;
                    DateTime? endDate = null;
                    if (hasRows)
                    {
                        DataTable dt = ds.Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.Rows[i];
                            if (DateTime.TryParse(dr["EFFECTIVE_BEGIN_DATE"].ToString(), out tmp))
                            {
                                beginDate = tmp;
                            }
                            if (DateTime.TryParse(dr["EFFECTIVE_END_DATE"].ToString(), out tmp))
                            {
                                endDate = tmp;
                            }
                            sb.Append(string.Format("<b>STATE:</b> {0}   <b>CITY:</b> {1}   <b>COUNTY:</b> {2}   <b>EXCLUSION START DATE:</b> {3}    <b>EXCLUSION END DATE:</b> {4}",
                                row["STATE_ABBREVIATION"].ToString(), row["CITY"].ToString(), row["COUNTY"].ToString(), beginDate.Value.ToString("MM-dd-yyyy"), endDate.Value.ToString("MM-dd-yyyy")));

                            if (i < dt.Rows.Count - 1)
                                sb.Append("<br>");
                        }
                    }

                    fields.Add("MORATORIAMATCHES", sb.ToString());

                    body = SendNotification(templateActualPath + @"/ReturnToProviderMoratoriaExclusion.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "ReturnToProviderMoratoriaExclusion.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool RejectionofSelfSuppliedBackground(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("LEGAL_NAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    GetFromAddress(fields);
                    body = SendNotification(templateActualPath + @"/RejectionofSelfSuppliedBackground.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "RejectionofSelfSuppliedBackground.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendDenyProviderNotification(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("CONTACTNAME", dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString());
                    fields.Add("LEGAL_NAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    fields.Add("SCREENINGMATCHES", this.GetScreeningMatchesHTML(regId));
                    fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                    body = SendNotification(templateActualPath + @"/DenyProviderNotification.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "DenyProviderNotification.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendICFInitialEnrollment(string regId)
        {
            try
            {

                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();

                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsICFInitialEnrollment = ExecuteStoredProcedure("usp_Select_ICFInitialEnrollment", parms);
                parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                fields.Add("ProviderName", hasRows ? dsProv.Tables[0].Rows[0]["NAME"].ToString() : "(Provider Name not found)");

                string ENDTERINGNAME = string.Empty;
                string ENTERINGDBA = string.Empty;
                string ENTERINGMEDICAIDID = string.Empty;
                string Chop_Effective_Date = string.Empty;
                string EXITINGMEDICAIDID = string.Empty;
                string EffectivedateofExitingOperator = string.Empty;
                string FACILITYNAME = string.Empty;
                string EXITINGNAME = string.Empty;

                if (dsICFInitialEnrollment != null && dsICFInitialEnrollment.Tables.Count > 0 && dsICFInitialEnrollment.Tables[0].Rows.Count > 0)
                {

                    DataRow row = dsICFInitialEnrollment.Tables[0].Rows[0];
                    ENDTERINGNAME = Methods.GetStringValue(row, "ENDTERINGNAME");
                    ENTERINGDBA = Methods.GetStringValue(row, "ENTERINGDBA");
                    ENTERINGMEDICAIDID = Methods.GetStringValue(row, "ENTERINGMEDICAIDID");
                    Chop_Effective_Date = Methods.GetStringValue(row, "Chop_Effective_Date");
                    EXITINGMEDICAIDID = Methods.GetStringValue(row, "EXITINGMEDICAIDID");
                    EffectivedateofExitingOperator = Methods.GetStringValue(row, "EffectivedateofExitingOperator");
                    FACILITYNAME = Methods.GetStringValue(row, "FACILITYNAME");
                    EXITINGNAME = Methods.GetStringValue(row, "EXITINGNAME");
                    //ENDDATE = Methods.GetStringValue(row, "ENDDATE");
                    ////ownercostreport = Methods.GetStringValue(row, "owner_name_from_cost_report");
                    ////chopeffective = Methods.GetStringValue(row, "Chop_effective_date");
                    Chop_Effective_Date = string.Format("MM/dd/yyyy", Chop_Effective_Date);
                    EffectivedateofExitingOperator = string.Format("MM/dd/yyyy", EffectivedateofExitingOperator);
                    //string ProviderName = !string.IsNullOrEmpty(row["ProviderName"].ToString())? row["ProviderName"].ToString() : enteringprovider;
                    string toAddress = GetToAddress(ENDTERINGNAME, Methods.GetStringValue(row, "CONTACT_QUADRANT"),
                       Methods.GetStringValue(row, "CONTACT_ADDRESS1"),
                       Methods.GetStringValue(row, "CONTACT_ADDRESS2"),
                       Methods.GetStringValue(row, "CONTACT_CITY") + ", " +
                       Methods.GetStringValue(row, "CONTACT_STATE") + " " +
                       Methods.GetStringValue(row, "CONTACT_ZIP") +
                      (!string.IsNullOrEmpty(Methods.GetStringValue(row, "CONTACT_EXT_ZIP")) ? "-" + Methods.GetStringValue(row, "CONTACT_EXT_ZIP") : ""), fields,
                      ENDTERINGNAME,
                      Methods.GetStringValue(row, "NPI"), ENTERINGMEDICAIDID, "", regId, Methods.GetStringValue(row, "DBA"), EffectivedateofExitingOperator, "");
                    string strFromAddress = AppSettings.Get("Mail-FromAddress", string.Empty);
                    AddBookmark(fields, "FROMADDRESS", strFromAddress);
                    // Added for Mass Email 
                    fields.Add("TOADDRESS", toAddress);
                    fields.Add("ENDTERINGNAME", ENDTERINGNAME);
                    fields.Add("ENTERINGDBA", ENTERINGDBA);
                    fields.Add("ENTERINGMEDICAIDID", ENTERINGMEDICAIDID);
                    fields.Add("Chop_Effective_Date", Chop_Effective_Date);
                    fields.Add("CHOPEFFECTIVEDATE", Chop_Effective_Date);
                    fields.Add("EXITINGMEDICAIDID", EXITINGMEDICAIDID);
                    fields.Add("EffectivedateofExitingOperator", EffectivedateofExitingOperator);
                    fields.Add("FACILITYNAME", FACILITYNAME);
                    fields.Add("EXITINGNAME", EXITINGNAME);
                    fields.Add("EXITINGFACILITYNAME", EXITINGNAME);
                    fields.Add("ENTERINGOPERATORNAME", ENDTERINGNAME);

                }
                else
                {
                    return false;
                }
                body = SendNotification(templateActualPath + @"/ICFInitialEnrollment.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "ICFInitialEnrollment.txt");
                Fields = fields;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendNFInitialEnrollment(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("NAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    body = SendNotification(templateActualPath + @"/NFInitialEnrollment.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "NFInitialEnrollment.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendConfirmGroupMemberNotification(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_GROUP_BY_GROUPMEMBERPROFILE_REGID", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                int groupRegID = 0;
                Notification notify = null;
                //recipients will contain the group member email addresses, but for these emails, we need to get the group's emails. 
                //send only if the group member profile affiliated to a group
                GetFromAddress(fields);
                if (hasRows)
                {
                    foreach (DataRow groupRow in dsProv.Tables[0].Rows)
                    {
                        fields.Clear();
                        fields.Add("LEGAL_NAME", groupRow["PROVIDERNAME"].ToString());
                        fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                        fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                        groupRegID = Methods.GetIntValue(groupRow["RegID"]);

                        List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                        sqlParms1.Add(new SqlParameter("REG_ID", groupRegID));

                        DataSet dsProvDetail = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", sqlParms1);
                        //DataRow datarow = dsProv.Tables[0].Rows[0];
                        fields.Add("PRIMARYCONTACTNAME", dsProvDetail.Tables[0].Rows[0]["CONTACT_NAME"].ToString());
                        fields.Add("GROUPNPI", dsProvDetail.Tables[0].Rows[0]["NPI"].ToString());
                        fields.Add("GROUPMEDICAID", dsProvDetail.Tables[0].Rows[0]["MEDICAID_ID"].ToString());
                        fields.Add("PRIMARYCONTACTADDRESS1", dsProvDetail.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString());
                        fields.Add("PRIMARYCONTACTADDRESS2", dsProvDetail.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString());
                        if (!string.IsNullOrEmpty(dsProvDetail.Tables[0].Rows[0]["CHANGE_EFFECTIVE_DATE"].ToString()))
                        {
                            DateTime eff = Convert.ToDateTime(dsProvDetail.Tables[0].Rows[0]["CHANGE_EFFECTIVE_DATE"].ToString());
                            fields.Add("EFFECTIVEDATE", eff.ToString("MM/dd/yyyy"));
                        }
                        if (!string.IsNullOrEmpty(dsProvDetail.Tables[0].Rows[0]["END_DATE"].ToString()))
                        {
                            DateTime revalDate = Convert.ToDateTime(dsProvDetail.Tables[0].Rows[0]["END_DATE"].ToString());
                            fields.Add("REVALDATE", revalDate.ToString("MM/dd/yyyy"));
                        }
                        fields.Add("GROUPCITYSTATEZIP", dsProvDetail.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                            dsProvDetail.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProvDetail.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                            (!string.IsNullOrEmpty(dsProvDetail.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProvDetail.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                                : string.Empty));

                        //group member details
                        fields.Add("INDIVIDUALPROVIDERNAME", groupRow["MBR_NAME"].ToString());
                        fields.Add("INDIVIDUALNPI", groupRow["MBR_NPI"].ToString());
                        if (!string.IsNullOrEmpty(groupRow["MBR_EFFECTIVE_DATE"].ToString()))
                        {
                            DateTime eff = Convert.ToDateTime(groupRow["MBR_EFFECTIVE_DATE"].ToString());
                            fields.Add("AFFILIATIONDATE", eff.ToString("MM/dd/yyyy"));
                        }
                        fields.Add("PROVIDERTYPE", groupRow["PROVIDER_TYPE"].ToString());
                        //Get recipients for this group provider - provider user email, provider primary contact email
                        string grpRecipients = GetRecipients(Constants.SendToTypeID.Provider, groupRegID);



                        //if Paper Email Template then send both Paper mail and Email
                        string paperTemplates = AppSettings.Get("PaperEmailTemplates");
                        if (paperTemplates.Contains("GroupAffiliationConfirmNotification.txt"))
                        {
                            notify = new PaperNotification(subject, ThreadId);
                            body = notify.SendNotification(templateActualPath + @"/GroupAffiliationConfirmNotification.txt", fields);
                            if (!string.IsNullOrEmpty(grpRecipients))
                            {
                                notify = new EMailNotification(body, subject, grpRecipients, ThreadId);
                                body = notify.SendNotification(templateActualPath + @"/GroupAffiliationConfirmNotification.txt", fields);
                            }

                        }
                        else
                        {
                            if (Methods.RequirePaperNotice(grpRecipients, AppSettings.Get("PSEmailTypes", string.Empty).Split(',')))
                            {
                                notify = new PaperNotification(subject, ThreadId);
                            }
                            else
                            {
                                notify = new EMailNotification(body, subject, grpRecipients, ThreadId);
                            }
                            body = notify.SendNotification(templateActualPath + @"/GroupAffiliationConfirmNotification.txt", fields);
                        }

                    }
                }
                else
                {
                    return false;
                }
                Fields = fields;
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendApprovedServices(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    templateActualPath = @"C:\Projects\OHPDMS\PDMS\ProviderDataManagementSystemService\Documents";
                }
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@REG_ID", regId));
                DataSet dsServices = ExecuteStoredProcedure("usp_SelectREG_SPECIALTYServices", sqlParams);

                //SAM751 display to include all active specialties in a grid format
                string approvedHTML = "<table border='1' cellpadding='2' cellspacing='5' style=\"padding: 3px; width: 100 % \"><tr><td><b>Specialty</b></td><td><b>Effective Date</b></td></tr>";

                if (Methods.HasRows(dsServices))
                {
                    foreach (DataRow row in dsServices.Tables[0].Rows)
                    {
                        if(!approvedHTML.Contains("<td>" + Methods.GetStringValue(row, "MMIS_SPECIALTY_TYPE_NAME") + "</td>"))
                        approvedHTML += "<tr><td>" + Methods.GetStringValue(row, "MMIS_SPECIALTY_TYPE_NAME") + "</td><td>" + Methods.GetStringValue(row, "START_DATE") + "</td></tr>";
                    }
                    approvedHTML += "</table>";
                }
                else
                {
                    approvedHTML += "<tr>No Active Specialty</tr></table>";
                }                    

                fields.Add("SERVICES_ADDED_ROWS", approvedHTML);

                fields.Add("SVCS_EFFECTIVE_DATE", DateTime.Now.ToString("MM-dd-yyyy"));

                fields.Add("LOGIN_URL", AppSettings.Get("PDMS-URL", string.Empty));

                sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regId));
                string medicaidID = GetMedicaidID(regId);

                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", sqlParams);
                string EffectiveDate = "";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CHANGE_EFFECTIVE_DATE"].ToString()))
                    {
                        DateTime eff = Convert.ToDateTime(ds.Tables[0].Rows[0]["CHANGE_EFFECTIVE_DATE"].ToString());
                        EffectiveDate = eff.ToString("MM/dd/yyyy");
                    }
                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();
                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                    row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                  row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                  (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                      : string.Empty), Methods.GetStringValue(row, "NAME"), Methods.GetStringValue(row, "NPI"), medicaidID, ds.Tables[0].Rows[0]["TAXONOMY_CODE"].ToString(), regId);
                }
                else
                {
                    return false;
                }
                fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                body = SendNotification(templateActualPath + @"/ChangeApprovedServices.txt", fields, true);
                CreateCommunicationEvent(fields, new Guid(), regId, "ChangeApprovedServices.txt");
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string GetRecipients(int sendToTypeID, int regId)
        {
            StringBuilder addrList = new StringBuilder();
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("SendToTypeID", sendToTypeID));
                parameters.Add(new SqlParameter("RegID", regId));

                DataSet ds = ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters);
                if (Methods.HasRows(ds))
                {
                    addrList = Methods.AddEmail(ds.Tables[0], "EmailAddress");
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

            return addrList.ToString();
        }

        private void SendNoticeToLTCOrDDS(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                int toPartyId = GetAdminPartyId();

                if (Methods.HasRows(dsProv))
                {
                    DataRow row = dsProv.Tables[0].Rows[0];


                    string ToName = row["NAME"].ToString() == "" ? row["CONTACT_NAME"].ToString() : row["NAME"].ToString();
                    fields.Add("provider", ToName);


                }
                body = SendNotification(templateActualPath + @"/DDSOrLTCEmailOnPracticeLocationUpdate.txt", fields, true);
                CreateCommunicationEvent(GetAdminPartyId(), toPartyId, fields, new Guid(), regId, "DDSOrLTCEmailOnPracticeLocationUpdate.txt");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool FreeFormEmail(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();

                // Get Provider information
                List<SqlParameter> parms = new List<SqlParameter> { new SqlParameter("REG_ID", regId) };
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                parms.Clear();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms);
                string medicaidID = string.Empty;
                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                }

                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM-dd-yyyy"));
                    fields.Add("MEDICAIDID", medicaidID);
                    DateTime endDate;
                    string endDateString = "";
                    endDateString = DateTime.TryParse(dsProv.Tables[0].Rows[0]["END_DATE"].ToString(), out endDate) ? endDate.ToString("MM-dd-yyyy") : "(END DATE not found)";
                    GetFromAddress(fields);
                    string ToName = dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString() == ""
                        ? dsProv.Tables[0].Rows[0]["NAME"].ToString()
                        : dsProv.Tables[0].Rows[0]["CONTACT_NAME"].ToString();

                    string toAddress = GetToAddress(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(),
                        dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                        dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(),
                        dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                        dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " +
                        dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString())
                            ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"),
                        Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), medicaidID, "", regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"), endDateString, "Re-Enrollment Due Date");
                    // Added for Mass Email 
                    fields.Add("TOADDRESS", toAddress);
                    fields.Add("TAXID", dsProv.Tables[0].Rows[0]["TAX_ID"].ToString());
                    fields.Add("FONTSIZE", "14pt");
                    fields.Add("DISPLAYOMR", string.Empty);
                }
                else
                {
                    return false;
                }
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));

                body = SendNotification(templateActualPath + @"/SuccessfullRevalidation.txt", fields, true);
                AddEmailAddressForProvider(regId);
                CreateCommunicationEvent(fields, new Guid(), regId, "SuccessfullRevalidation.txt");
                Fields = fields;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendODATerminationNotice(string regId)
        {
            try
            {
                string customSubject = string.Empty;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms1 = new List<SqlParameter>();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms1);

                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms1);
                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));

                string medicaidID = string.Empty;
                DateTime termDate;
                string name = string.Empty;

                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                    customSubject = String.Format("Termination of Medicaid Provider Agreement Number {0}", medicaidID);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    name = Methods.GetStringValue(row, "NAME");

                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), name, Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));


                    fields.Add("PROVIDER_NAME", name);
                    fields.Add("PROVIDER_NPI", Methods.GetStringValue(row, "NPI"));
                    fields.Add("MEDICAID_ID", medicaidID);
                    fields.Add("REG_ID", regId);
                    if (DateTime.TryParse(Methods.GetStringValue(row, "TERM_DATE"), out termDate))
                    {
                        fields.Add("TERM_DATE", termDate.ToString("MM-dd-yyyy"));
                    }
                    else
                    {
                        fields.Add("TERM_DATE", string.Empty);
                    }
                    string recipients = GetRecipients(Constants.SendToTypeID.ODANotificationEmailGroup, Convert.ToInt32(regId));
                    EMailNotification enotify = new EMailNotification(body, customSubject, recipients, ThreadId);
                    body = enotify.SendActualNotification(templateActualPath + @"/ODATerminationNotification.txt", fields, true);

                    return true;
                }
                Fields = fields;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendDODDTerminationNotice(string regId)
        {
            try
            {
                string customSubject = string.Empty;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms1 = new List<SqlParameter>();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms1);

                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));
                DataSet dsServiceLoc = ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parms1);
                parms1.Clear();
                parms1.Add(new SqlParameter("REG_ID", regId));

                string medicaidID = string.Empty;
                string name = string.Empty;
                DateTime termDate;
                string ddContractNumber = string.Empty;

                if (Methods.HasRows(dsServiceLoc))
                {
                    DataRow row = dsServiceLoc.Tables[0].Rows[0];
                    medicaidID = Methods.GetStringValue(row, "MEDICAID_ID");
                    ddContractNumber = Methods.GetStringValue(row, "DD_Contract_Number");
                    customSubject = String.Format("Termination of Medicaid Provider Agreement Number {0}", medicaidID);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    name = Methods.GetStringValue(row, "NAME");
                    GetFromAddress(fields);
                    string ToName = row["CONTACT_NAME"].ToString() == "" ? row["NAME"].ToString() : row["CONTACT_NAME"].ToString();

                    GetToAddress(fields, ToName, row["CONTACT_QUADRANT"].ToString(), row["CONTACT_ADDRESS1"].ToString(),
                        row["CONTACT_ADDRESS2"].ToString(), row["CONTACT_CITY"].ToString() + ", " +
                        row["CONTACT_STATE"].ToString() + " " + row["CONTACT_ZIP"].ToString() +
                        (!string.IsNullOrEmpty(row["CONTACT_EXT_ZIP"].ToString()) ? "-" + row["CONTACT_EXT_ZIP"].ToString()
                            : string.Empty), name, Methods.GetStringValue(row, "NPI"), medicaidID, "", regId, Methods.GetStringValue(row, "DBA"));
                    fields.Add("PROVIDER_NAME", name);
                    fields.Add("PROVIDER_NPI", Methods.GetStringValue(row, "NPI"));
                    fields.Add("MEDICAID_ID", medicaidID);
                    fields.Add("REG_ID", regId);
                    if (DateTime.TryParse(Methods.GetStringValue(row, "TERM_DATE"), out termDate))
                    {
                        fields.Add("TERM_DATE", termDate.ToString("MM-dd-yyyy"));
                    }
                    else
                    {
                        fields.Add("TERM_DATE", string.Empty);
                    }
                    fields.Add("DODD_CONTRACT_NUMBER", ddContractNumber);
                    string recipients = GetRecipients(Constants.SendToTypeID.DODDNotificationEmailGroup, Convert.ToInt32(regId));
                    EMailNotification enotify = new EMailNotification(body, customSubject, recipients, ThreadId);
                    body = enotify.SendActualNotification(templateActualPath + @"/DODDTerminationNotification.txt", fields, true);

                    return true;
                }
                Fields = fields;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetAdminPartyId()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string rtn = ExecuteScalar("sp_SelectAdminPartyID", parameters);
            if (string.IsNullOrEmpty(rtn)) return 0;
            return Convert.ToInt32(rtn);
        }

        public virtual void CreateCommunicationEvent(int fromPartyId, int toPartyId, Dictionary<string, object> fields, Guid userId, string regId, string templateName)
        {
            // Do nothing
        }
        private string GetScreeningMatchesHTML(string regId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("REG_ID", regId));
            DataSet ds = new DataSet();
            ds = ExecuteStoredProcedure("usp_SelectREG_SCREENING_MATCH", parameters);
            if (ds.Tables.Count == 0)
                return "There are no screening exception matches.";
            else if (ds.Tables[0].Rows.Count == 0)
                return "There are no screening exception matches.";
            else
            {
                StringBuilder sb = new StringBuilder();
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append(string.Format("{0}: NPI - {1}", dt.Rows[i]["SCREENING_ACTIVITY_TYPE_NAME"].ToString(), dt.Rows[i]["NPI"].ToString()));

                    if (i < dt.Rows.Count - 1)
                        sb.Append("<br>");
                }

                return sb.ToString();
            }
        }
        #endregion
        public string ParseEmailBody(string templateBody, Dictionary<string, object> fieldValues, bool throwIfNotFound = true)
        {
            TemplateEvaluator eval = new TemplateEvaluator(templateBody, Guid.NewGuid());
            string content = eval.Eval(TemplateFieldAccessors.DictionaryAccessor(fieldValues, throwIfNotFound));
            return content;
        }

        //Get ToAddress 
        //for MassEmail notification
        //use existing logic but return formatted address to calling program
        public string GetToAddress(string ContactName, string Quadrant, string Address1, string Address2, string CityStateZip, Dictionary<string, object> fields,
            string ProviderName = "", string NPI = "", string MedicaidID = "", string Taxonomy = "", string RegID = "", string DBA = "", string EFFECTIVEDATE = "", string DateType = "")
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
            bool NPIflag = false;
            bool Taxonomyflag = false;
            bool Effectivedateflag = false;
            bool RegistrationIDflag = false;
            bool DBAflag = false;

            if (!string.IsNullOrEmpty(Taxonomy.ToString()))
            {
                Taxonomy = "Taxonomy: " + Taxonomy;
            }
            else
            {
                Taxonomy = "";
            }
            if (!string.IsNullOrWhiteSpace(RegID.ToString()))
            {
                RegID = "Registration ID: " + RegID;
            }
            else
            {
                RegID = "";
            }
            if (!string.IsNullOrWhiteSpace(DBA.ToString()))
            {
                DBA = "DBA: " + DBA;
            }
            else
            {
                DBA = "";
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
                //  strToaddress += "<table style=\"font-family:'Arial Narrow';font-size:14pt;width:100%;margin:0px;cellpadding: 0px;cellspacing: 0px\">";
                strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.65in;\">" + ContactName + "</td>";
                strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt;padding-left:0.65in;\">" + ProviderName + "</td></tr>";
                strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.65in;\">" + Address1 + "</td>";
                if (DBA != "")
                {
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + DBA + "</td></tr>";
                    DBAflag = true;
                }
                else
                {
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + NPI + "</td></tr>";
                    NPIflag = true;
                }
                if (Address2.Trim() != "")
                {
                    strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.65in;\">" + Address2 + "</td>";
                    if (!NPIflag)
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + NPI + "</td></tr>";
                        NPIflag = true;
                    }
                    else if (MedicaidID != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + MedicaidID + "</td></tr>";
                        Medicaidflag = true;
                    }
                    else if (Taxonomy != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + Taxonomy + "</td></tr>";
                        Taxonomyflag = true;

                    }
                    else if (EFFECTIVEDATE != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + EFFECTIVEDATE + "</td></tr>";
                        Effectivedateflag = true;

                    }
                    else
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\"></td></tr>";
                    }

                }

                strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.65in;\">" + CityStateZip + "</td>";
                if (Address2.Trim() == "")
                {

                    if (!NPIflag)
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + NPI + "</td></tr>";
                        NPIflag = true;
                    }
                    else if (MedicaidID != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + MedicaidID + "</td></tr>";
                        Medicaidflag = true;

                    }
                    else if (Taxonomy != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + Taxonomy + "</td></tr>";

                        Taxonomyflag = true;

                    }
                    else if (EFFECTIVEDATE != "")
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + EFFECTIVEDATE + "</td></tr>";
                        Effectivedateflag = true;
                    }
                    else
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\"></td></tr>";
                    }
                }
                else
                {
                    if (!NPIflag)
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + NPI + "</td></tr>";
                        NPIflag = true;
                    }
                    else if (MedicaidID != "" && !(Medicaidflag))
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + MedicaidID + "</td></tr>";
                    }
                    else if (Taxonomy != "" && !(Taxonomyflag))
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + Taxonomy + "</td></tr>";
                        Taxonomyflag = true;
                    }
                    else if (EFFECTIVEDATE != "" && !(Effectivedateflag))
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + EFFECTIVEDATE + "</td></tr>";
                        Effectivedateflag = true;
                    }
                    else
                    {
                        strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\"></td></tr>";
                    }

                }


                if (!NPIflag)
                {
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + NPI + "</td></tr>";
                    NPIflag = true;
                }

                if (Taxonomy != "" && !(Taxonomyflag))
                {
                    strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt\"></td>";
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + Taxonomy + "</td></tr>";
                }

                if (EFFECTIVEDATE != "" && !(Effectivedateflag))
                {
                    strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt\"></td>";
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + EFFECTIVEDATE + "</td></tr>";
                }
                if (RegID != "" && !(RegistrationIDflag))
                {
                    strToaddress += "<tr><td style=\"width:50%;text-align:left;font-family:'Arial Narrow';font-size:14pt\"></td>";
                    strToaddress += "<td style=\"width:50%;text-align:right;font-family:'Arial Narrow';font-size:14pt\">" + RegID + "</td></tr>";
                }

                //   strToaddress += "</table>";
            }

            string mailpiece = string.Empty;
            //crawford logic
            mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add0>$$MAILPIECE$$</add0></h1>";
            mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add1>Add1:" + ContactName + "</add1></h1>";
            mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add2>Add2:" + Address1 + "</add2></h1>";
            if (Address2.Trim() != "" && !string.IsNullOrEmpty(Address2) && !string.IsNullOrWhiteSpace(Address2))
            {
                mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add3>Add3:" + Address2 + "</add3></h1>";
                mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add4>Add4:" + CityStateZip + "</add4></h1>";
            }
            else
            {
                mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add3>Add3:" + CityStateZip + "</add3></h1>";
            }
            AddBookmark(fields, "MAILPIECE", mailpiece);

            return strToaddress;
        }

        private bool isCredentialingRequired(string regId)
        {
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(new SqlParameter("RegID", Convert.ToInt32(regId)));
            DataSet dsReg = ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegID", sqlParms);

            dsReg.Tables[0].TableName = "RegData";
            DataRow drReg = Methods.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
            bool requiresCredentialing = Methods.GetBool("CredentialingRequired", drReg);
            bool isHospitalBasedProvider = IsHospitalBasedProvider(regId);
            //HospitalBasedProviders shouldn't go to credentialing
            requiresCredentialing = isHospitalBasedProvider ? false : requiresCredentialing;

            bool credentialedByDelegate = Methods.GetBool("DelegateCredentialingRequired", drReg);
            if (credentialedByDelegate)
            {
                // if they are credentialed by a delegate, don't require credentialing since the delegate takes care of it
                requiresCredentialing = false;
            }

            return requiresCredentialing;
        }

        private bool IsHospitalBasedProvider(string regId)
        {
            bool hospitalProvider = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("REG_ID", Convert.ToInt32(regId)));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_HEALTH_CARE_FACILITY_AFFILIATION", parameters);

                if (Methods.HasRows(ds))
                {
                    int rowCount = ds.Tables[0].Rows.Count;
                    int inPatientSettingCount = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        bool IsInpatientSetting = Methods.GetBool("IsInpatientSetting", dr);

                        if (IsInpatientSetting)
                            inPatientSettingCount = inPatientSettingCount + 1;
                    }

                    if (rowCount == inPatientSettingCount)
                        hospitalProvider = true;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return hospitalProvider;
        }
        private bool NPPESInactiveTerminationLetter(string regId)
        {
            try
            {
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                
                Dictionary<string, object> fields = new Dictionary<string, object>();
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", regId));
                DataSet dsProv = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);

                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    DataRow dr = dsProv.Tables[0].Rows[0];

                    fields.Add("Name", dr["NAME"].ToString());
                    fields.Add("NPI", dr["NPI"].ToString());
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("STATECONTACTPHONE", AppSettings.Get(Constants.AppSettingsKeyName.StateContactPhone, string.Empty));
                    fields.Add("PDMSURL", AppSettings.Get(Constants.AppSettingsKeyName.PDMSURL, string.Empty));
                    fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));                  
                    
                    string ToName = dr["CONTACT_NAME"].ToString() == "" ? dr["NAME"].ToString() : dr["CONTACT_NAME"].ToString();

                    setPaperProviderInformation(ToName, dsProv.Tables[0].Rows[0]["CONTACT_QUADRANT"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS1"].ToString(),
                     dsProv.Tables[0].Rows[0]["CONTACT_ADDRESS2"].ToString(), dsProv.Tables[0].Rows[0]["CONTACT_CITY"].ToString() + ", " +
                     dsProv.Tables[0].Rows[0]["CONTACT_STATE"].ToString() + " " + dsProv.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() +
                     (!string.IsNullOrEmpty(dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()) ? "-" + dsProv.Tables[0].Rows[0]["CONTACT_EXT_ZIP"].ToString()
                         : string.Empty), fields, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NAME"), Methods.GetStringValue(dsProv.Tables[0].Rows[0], "NPI"), "", "", regId, Methods.GetStringValue(dsProv.Tables[0].Rows[0], "DBA"));



                    body = SendNotification(templateActualPath + @"/NPPESInactiveTerminationNotification.txt", fields, true);
                    CreateCommunicationEvent(fields, new Guid(), regId, "NPPESInactiveTerminationNotification.txt");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void setPaperReturnAddress(Dictionary<string, object> fields)
        {
            string strPaperReturnAddress = AppSettings.Get("Mail-ReturnAddress", string.Empty); ;

            AddBookmark(fields, "RETURNADDRESS", strPaperReturnAddress);
        }

        public void setPaperProviderInformation(string ContactName, string Quadrant, string Address1, string Address2, string CityStateZip, Dictionary<string, object> fields,
            string ProviderName = "", string NPI = "", string MedicaidID = "", string Taxonomy = "", string RegID = "", string DBA = "", string EFFECTIVEDATE = "", string DateType = "")
        {
            string strPaperMailAddress = string.Empty;
            string printWindow = AppSettings.Get("PrintWindow", CON.PrintCenterHeader.Double).ToLower();

            if (printWindow.Equals(CON.PrintCenterHeader.Double))
            {
                setPaperReturnAddress(fields);
                strPaperMailAddress += "<tr><td style=\"text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:4.15in;padding-top:0.32in;\">" + ContactName + "</td></tr>";
                strPaperMailAddress += "<tr><td style=\"text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:4.15in;\">" + Address1 + "</td></tr>";
                if (Address2.Trim() != "" && !string.IsNullOrEmpty(Address2) && !string.IsNullOrWhiteSpace(Address2))
                {
                    strPaperMailAddress += "<tr><td style=\"text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:4.15in;\">" + Address2 + "</td></tr>";
                    strPaperMailAddress += "<tr><td style=\"text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:4.15in;\">" + CityStateZip + "</td></tr>";
                }
                else
                {
                    strPaperMailAddress += "<tr><td style=\"text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:4.15in;\">" + CityStateZip + "</td></tr>";
                }
            }
            else
            {
                strPaperMailAddress += "<tr><td style=\"text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.75in;padding-top:1.85in;\">" + ContactName + "</td></tr>";
                strPaperMailAddress += "<tr><td style=\"text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.75in;\">" + Address1 + "</td></tr>";
                if (Address2.Trim() != "" && !string.IsNullOrEmpty(Address2) && !string.IsNullOrWhiteSpace(Address2))
                {
                    strPaperMailAddress += "<tr><td style=\"text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.75in;\">" + Address2 + "</td></tr>";
                    strPaperMailAddress += "<tr><td style=\"text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.75in;\">" + CityStateZip + "</td></tr>";
                }
                else
                {
                    strPaperMailAddress += "<tr><td style=\"text-align:left;font-family:'Arial Narrow';font-size:14pt;padding-left:0.75in;\">" + CityStateZip + "</td></tr>";
                }
            }

            AddBookmark(fields, "MAILADDRESS", strPaperMailAddress);

            string mailpiece = string.Empty;
            //crawford logic
            mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add0>$$MAILPIECE$$</add0></h1>";
            mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add1>Add1:" + ContactName + "</add1></h1>";
            mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add2>Add2:" + Address1 + "</add2></h1>";
            if (Address2.Trim() != "" && !string.IsNullOrEmpty(Address2) && !string.IsNullOrWhiteSpace(Address2))
            {
                mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add3>Add3:" + Address2 + "</add3></h1>";
                mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add4>Add4:" + CityStateZip + "</add4></h1>";
            }
            else
            {
                mailpiece += "<h1 style=\"font-size: 1px; color: white; display: inline;\"><add3>Add3:" + CityStateZip + "</add3></h1>";
            }
            AddBookmark(fields, "MAILPIECE", mailpiece);
            AddBookmark(fields, "PRIMARYCONTACTNAME", ContactName);

            string strToaddress = string.Empty;
            if (!string.IsNullOrEmpty(MedicaidID.ToString()))
            {
                MedicaidID = "Medicaid ID: " + MedicaidID;
            }
            else
            {
                MedicaidID = "";
            }

            if (!string.IsNullOrEmpty(Taxonomy.ToString()))
            {
                Taxonomy = "Taxonomy: " + Taxonomy;
            }
            else
            {
                Taxonomy = "";
            }
            if (!string.IsNullOrWhiteSpace(RegID.ToString()))
            {
                RegID = "Registration ID: " + RegID;
            }
            else
            {
                RegID = "";
            }
            if (!string.IsNullOrWhiteSpace(DBA.ToString()))
            {
                DBA = "DBA: " + DBA;
            }
            else
            {
                DBA = "";
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

            ProviderName = "Provider Name: " + ProviderName;
            strToaddress += "<tr><td style=\"font-family:'Arial Narrow';font-size:14pt;\">" + ProviderName + "</td></tr>";

            if (DBA != "")
            {
                strToaddress += "<tr><td style=\"font-family:'Arial Narrow';font-size:14pt\">" + DBA + "</td></tr>";
            }
            if (NPI != "")
            {
                strToaddress += "<tr><td style=\"font-family:'Arial Narrow';font-size:14pt\">" + NPI + "</td></tr>";
            }
            if (MedicaidID != "")
            {
                strToaddress += "<tr><td style=\"font-family:'Arial Narrow';font-size:14pt\">" + MedicaidID + "</td></tr>";
            }
            if (Taxonomy != "")
            {
                strToaddress += "<tr><td style=\"font-family:'Arial Narrow';font-size:14pt\">" + Taxonomy + "</td></tr>";
            }
            if (EFFECTIVEDATE != "")
            {
                strToaddress += "<tr><td style=\"font-family:'Arial Narrow';font-size:14pt\">" + EFFECTIVEDATE + "</td></tr>";
            }
            if (RegID != "")
            {
                strToaddress += "<tr><td style=\"font-family:'Arial Narrow';font-size:14pt\">" + RegID + "</td></tr>";
            }

            AddBookmark(fields, "PROVINFO", strToaddress);
        }

    }
}