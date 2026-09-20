using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Reflection;

namespace MAXIMUS.Core.Libraries

{
    public class EMailNotification : Notification
    {
        string recipients;
        string bccList;
        public string realRecipients;
        #region "Constructors"
        /// <summary>
        ///     The default parameterless constructor. Generates a new GUID for the Logging threadId
        /// </summary>
        public EMailNotification() : base()
        {
        }

        /// <summary>
        ///     The parameterized constructor which provides the Logging threadId GUID.
        /// </summary>
        /// <param name="threadId">The GUID which is used for tying all log entires across all classes.</param>
        public EMailNotification(Guid threadId)
            : base(threadId)
        {
        }
        
        /// <summary>
        ///     The parameterized constructor which provides the Logging threadId GUID.
        /// </summary>
        /// <param name="threadId">The GUID which is used for tying all log entires across all classes.</param>
        public EMailNotification(string body, string subject, string recipients = "", Guid threadId = new Guid(), string bccList = "")
        {
            this.recipients = recipients;
            this.body = body;
            this.subject = subject;
            ThreadId = threadId == Guid.Empty ? Guid.NewGuid() : threadId;
            this.bccList = bccList;
        }
        public EMailNotification(string body, string subject, string recipients, string reg_id)
        {
            this.recipients = recipients;
            this.body = body;
            this.subject = subject;
            ThreadId = new Guid();
            this.bccList = "";
            this.Reg_ID = reg_id;
        }
        /// <summary>
        ///     Sends an HTML or Non-HTML message to specific parties based on AppSettings values. This method will bypass
        ///         the TestEmail settings and send directly to the parties identified in AppSettings
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The body of the email</param>
        /// <param name="isHTML"> bool value to set if email to be sent as html format</param>
        public void SendGenericJobNotification(bool isHTML)
        {
            try
            {
                this.recipients = AppSettings.Get("Jobs-NotificationEmailAddresses", "OHPNMCodeJunkies@maximus.com");
                MailMessage emailMessage = new MailMessage();
                emailMessage.Body = body;
                SendMessage(emailMessage, isHTML, false, true);
                emailMessage = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Sends an HTML or Non-HTML message to specific parties based on AppSettings values. This method will bypass
        ///         the TestEmail settings and send directly to the parties identified in AppSettings
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The body of the email</param>
        /// <param name="isHTML"> bool value to set if email to be sent as html format</param>
        public void SendJobNotification(bool isHTML, Guid jobId)
        {
            try
            {
                this.recipients = AppSettings.Get("Jobs-Email-" + jobId.ToString().ToUpper(), "OHPNMCodeJunkies@maximus.com");
                MailMessage emailMessage = new MailMessage();
                emailMessage.Body = body;
                SendMessage(emailMessage, isHTML, false, true);
                emailMessage = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        ///     Sends a non-HTML message
        /// </summary>
        /// <param name="recipients">One or more email addresses with a comma delimiter, no delimiter necessary on a single email address</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The non-HTML body of the email</param>
        public void SendNotification()
        {
            try
            {
                MailMessage email = new MailMessage();
                email.Body = body;
                SendMessage(email, false, false);
                email = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Sends an HTML or Non-HTML message
        /// </summary>
        /// <param name="recipients">One or more email addresses with a comma delimiter, no delimiter necessary on a single email address</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The body of the email</param>
        /// <param name="isHTML"> bool value to set if email to be sent as html format</param>
        public void SendNotification(bool isHTML)
        {
            try
            {
                MailMessage emailMessage = new MailMessage();
                emailMessage.Body = body;
                SendMessage(emailMessage, isHTML, false);
                emailMessage = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string SendMQNotification(string templateActualPath, Dictionary<string, object> fields, bool isHTML = true)
        {
            string GenericTemplatePath = AppSettings.Get("SI_MQ_TemplatesPath", string.Empty);

            GenericTemplatePath = GenericTemplatePath + "//GenericNoticeTemplate.txt";
            body1 = SendGenericNotification(GenericTemplatePath, Reg_ID, isHTML);
            if (Reg_ID != null)
            {
                string smsMsg = AppSettings.Get("SmsDefaultMessage",
                    "You have a new message from OHIO Medicaid.  Please login to your account to view it.");
                SendSMS(smsMsg, Reg_ID);
            }
            return this.SendNotification(templateActualPath, string.Empty, fields, isHTML, new List<string>());
        }
        public void SendNotificationBody(string body)
        {
            body1 = body;
        }

        public override string SendNotification(string templateActualPath, Dictionary<string, object> fields, bool isHTML = true)
        {
            string GenericTemplatePath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                GenericTemplatePath = @"C:\Users\CA_OHPNM_DEVLEADS_5\source\repos\ohpnm-src-p3\PDMS\ProviderDataManagementSystemService\Documents";
            }
            GenericTemplatePath = GenericTemplatePath + "//GenericNoticeTemplate.txt";
            body1 = SendGenericNotification(GenericTemplatePath, Reg_ID, isHTML);
            if (Reg_ID!=null)
            {
                string smsMsg = AppSettings.Get("SmsDefaultMessage", 
                    "You have a new message from OHIO Medicaid.  Please login to your account to view it.");
                SendSMS(smsMsg, Reg_ID);
            }
            return this.SendNotification(templateActualPath, string.Empty, fields, isHTML, new List<string>());
        }
        public string SendGenericNotification(string templateActualPath, string reg_id,bool isHTML)
        {
            try
            {                
                TemplateEvaluator template = new TemplateEvaluator();
                template.Load(templateActualPath);
                MailMessage emailMessage = new MailMessage();

                //0HPNM - 13418 : Add MED ID and Provider Name to the Provider Email Notification  
                string providerName = string.Empty;
                string medicaidId = string.Empty;
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("REG_ID", reg_id));
                DataSet ds = ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    providerName = row["NAME"].ToString();
                    medicaidId = ds.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
                }
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("REG_ID", reg_id);
				fields.Add("PDMS_URL", AppSettings.Get("PDMS-URL", string.Empty));
                fields.Add("PDMS_RESOURCE_URL", AppSettings.Get("PDMS-RESOURCE-URL", string.Empty));
                fields.Add("PROVIDER_NAME", providerName);
                fields.Add("MEDICAID_ID", medicaidId);
                body1 = template.Eval(TemplateFieldAccessors.DictionaryAccessor(fields));
                emailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body1, null, (isHTML ? "text/html" : "text/plain"))); 
                SendMessage(emailMessage, isHTML, false);
                emailMessage = null;
                return body1;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ThreadId, ex);
            }
        }

        public string SendActualNotification(string templateActualPath, Dictionary<string, object> fields, bool isHTML)
        {
            try
            {
                TemplateEvaluator template = new TemplateEvaluator();
                template.Load(templateActualPath);
                MailMessage emailMessage = new MailMessage();
                body1 = template.Eval(TemplateFieldAccessors.DictionaryAccessor(fields));
                emailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body1, null, (isHTML ? "text/html" : "text/plain")));
                SendMessage(emailMessage, isHTML, false);
                emailMessage = null;
                return body1;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ThreadId, ex);
            }
        }


        public override void CreateCommunicationEvent(Dictionary<string, object> fields, Guid userId, string regId, string templateName)
        {
             // Create the communicaton event and email
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CommunicationEventType", "Email"));
            string comTypeID = ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);
            string emailFrom = AppSettings.Get("SmtpFromEmailAddress", string.Empty);

            // create parameters objects and fill with values
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_TYPE_ID", Convert.ToInt32(comTypeID)));
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_VALUE", string.Empty));
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("EMAIL_FROM", emailFrom));
            parameters.Add(new SqlParameter("EMAIL_TO", recipients));
            parameters.Add(new SqlParameter("SUBJECT", subject));
            parameters.Add(new SqlParameter("BODY", body));
            parameters.Add(new SqlParameter("TEMPLATE_NAME", templateName));
            parameters.Add(new SqlParameter("KEY_VALUE_PAIR", GetKeyValueString(fields)));
            parameters.Add(new SqlParameter("REG_ID", regId));
            parameters.Add(new SqlParameter("USER_ID", userId));
            parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId));
            parameters.Add(new SqlParameter("isEmailSent", isEmailSent));
            parameters.Add(new SqlParameter("log_message", log_message));
            parameters.Add(new SqlParameter("GENERIC_BODY", body1));
            ExecuteStoredProcedure("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);

            //this.UpdateCommunicationEventForParty(fromPartyId, toPartyId);
        }


        public string ResendNotification(bool isHTML, List<string> attachmentFileNames)
        {
            try
            {
                MailMessage emailMessage = new MailMessage();
                emailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, (isHTML ? "text/html" : "text/plain")));
                foreach (string attachmentFileName in attachmentFileNames)
                {
                    if (!string.IsNullOrEmpty(attachmentFileName))
                    {
                        System.IO.FileStream stream = System.IO.File.OpenRead(attachmentFileName);
                        string friendlyName;
                        int idx = attachmentFileName.LastIndexOf("\\");
                        if (idx == -1) friendlyName = attachmentFileName;
                        else friendlyName = attachmentFileName.Substring(idx + 1);
                        Attachment attachment = new Attachment(stream, friendlyName);
                        emailMessage.Attachments.Add(attachment);
                    }
                }

                SendMessage(emailMessage, isHTML, false);
                emailMessage = null;
                return body;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ThreadId, ex);
            }
        }

        /// <summary>
        ///     Send Email with Template file to build the email body message.
        /// </summary>
        /// <param name="templateActualPath">Actual path where the template exists, use this first. If it is empty use emailtemplateConfigSetting</param>
        /// <param name="emailTemplateConfigSetting">Email Template Configuration setting</param>
        /// <param name="Subject">The subject line of the email</param>
        /// <param name="recipients">One or more email addresses with a comma delimiter, no delimiter necessary on a single email address</param>
        /// <param name="fields"> Dictionary fields: string/object pair to add dynamic fields to the email body with the Template file</param>
        /// <param name="isHTML"> bool value to set if email to be sent as html format</param>
        /// <param name="attachmentFileNames"> Physical file names with path of Attachment Files</param>
        /// RETURNS the body of the email
        public string SendNotification(string templateActualPath,string emailTemplateConfigSetting, Dictionary<string, object> fields, bool isHTML, List<string> attachmentFileNames)
        {
            try
            {
                fields.Add("FONTSIZE", "10pt");
                fields.Add("DISPLAYOMR", "");

                TemplateEvaluator template = new TemplateEvaluator();                
                string path;
                if (!string.IsNullOrEmpty(templateActualPath)) path = templateActualPath;
                else
                {
                    path = System.AppDomain.CurrentDomain.BaseDirectory;     //Done: Joe - this is to Get the base directory that the assembly resolver uses to probe for assemblies.
                    path += AppSettings.Get(emailTemplateConfigSetting).ToString();
                }
                template.Load(path);                
                MailMessage emailMessage = new MailMessage();
                string body = template.Eval(TemplateFieldAccessors.DictionaryAccessor(fields));
                emailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, (isHTML ? "text/html" : "text/plain")));        
                foreach (string attachmentFileName in attachmentFileNames)
                {
                    if (!string.IsNullOrEmpty(attachmentFileName))
                    {
                        System.IO.FileStream stream = System.IO.File.OpenRead(attachmentFileName);
                        string friendlyName;
                        int idx = attachmentFileName.LastIndexOf("\\");
                        if (idx == -1) friendlyName = attachmentFileName;
                        else friendlyName = attachmentFileName.Substring(idx + 1);
                        Attachment attachment = new Attachment(stream, friendlyName);
                        emailMessage.Attachments.Add(attachment);
                    }
                }
                //SendMessage(emailMessage, isHTML, false, false);
                emailMessage = null;
                return body;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ThreadId, ex);
            }
        }

        /// <summary>
        ///     Given the subject, checks if the "SubjectWithEnvironment" settings is TRUE.  If it is, appends the "Environment" setting (if it exists)
        ///     to the end of the subject.
        /// </summary>
        private string ConstructSubject()
        {
            string rtn = this.subject;
            string testing = AppSettings.Get("SubjectWithEnvironment", bool.TrueString.ToLower());
            if (testing.ToLower() == bool.TrueString.ToLower() &&
                !string.IsNullOrEmpty(AppSettings.Get("Environment", string.Empty)))
            {
                // If not already in subject add it
                string append = "[" + AppSettings.Get("Environment", string.Empty) + "]";
                if (this.subject.IndexOf(append) == -1) rtn += " " + append;
            }
            return rtn;
        }

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
        private void SendMessage(MailMessage emailMessage, bool isHTML, bool sendSSL, bool bypassTestEmail = false, string bccList = "")
        {

            const char delimiter1 = ',';
            const char delimiter2 = ';';
            string testing = bool.TrueString.ToLower();

            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(ThreadId, logMsg);

            try
            {
                string emailSubject = ConstructSubject();

                bool testEmail = false;
                // if testing enabled
                testing = AppSettings.Get("TestEmailEnabled", bool.TrueString.ToLower());
                if (testing.ToLower() == bool.TrueString.ToLower() && bypassTestEmail == false)
                {
                    this.recipients =  AppSettings.Get("TestEmailAddress", "dhivyalakshmidevarajulu@maximus.com");
                    testEmail = true;
                }
                else
                {
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break(); 
                }

                // add the To email address(es)
                char delimiter = delimiter1;
                if (this.recipients.IndexOf(delimiter1) == -1) delimiter = delimiter2;
                string[] Addrs = this.recipients.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string adr in Addrs)
                {
                    emailMessage.To.Add(new MailAddress(adr));
                }

                if (!testEmail)                                         // Only add them if we are NOT testing
                {
                    // add the BCC email address(es)
                    string bccEmail = AppSettings.Get("SmtpBCC", string.Empty);
                    if (!string.IsNullOrWhiteSpace(bccEmail))
                    {
                        delimiter = delimiter1;
                        if (bccEmail.IndexOf(delimiter1) == -1) delimiter = delimiter2;
                        string[] bccAddrs = bccEmail.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string adr in bccAddrs)
                        {
                            emailMessage.Bcc.Add(new MailAddress(adr));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(bccList))
                    {
                        delimiter = delimiter1;
                        if (bccList.IndexOf(delimiter1) == -1) delimiter = delimiter2;
                        string[] bccAddrs1 = bccList.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string adr1 in bccAddrs1)
                        {
                            emailMessage.Bcc.Add(new MailAddress(adr1));
                        }
                    }
                }

                string replyToEmail = AppSettings.Get("SmtpReplyTo", string.Empty);
                string emailfromAddress = AppSettings.Get("SmtpFromEmailAddress", string.Empty);

                if (!string.IsNullOrEmpty(replyToEmail)) emailMessage.ReplyToList.Add(replyToEmail);
                if (!string.IsNullOrEmpty(emailfromAddress)) emailMessage.From = new MailAddress(emailfromAddress);
               
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

//#if DEBUG
//smtpclient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
//smtpclient.PickupDirectoryLocation = @"C:\TestEmails";
//#endif

                // send it
                smtpclient.Send(emailMessage);
                isEmailSent = true;
                
            }
            catch (SmtpFailedRecipientException ex)
            {
                string msg = String.Format("Email not sent, Reason [{0}]", ex.Message);
                log.CreateLogEntry(msg, Logging.LogPriority.Error, +logCnt);
                isEmailSent = false;
                log_message = msg;
            }
            catch (SmtpException ex)
            {
                string msg = String.Format("Email not sent, Reason [{0}]", ex.Message);
                log.CreateLogEntry(msg, Logging.LogPriority.Error, +logCnt);
                isEmailSent = false;
                log_message = msg;
            }
            catch (Exception ex)
            {
                string msg = String.Format("Email not sent, Reason [{0}]", ex.Message);
                log.CreateLogEntry(msg, Logging.LogPriority.Error, +logCnt);
                isEmailSent = false;
                log_message = msg;
            }
        }
        #endregion
    }
}
