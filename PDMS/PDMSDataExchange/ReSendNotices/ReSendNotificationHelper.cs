using DocumentFormat.OpenXml.Wordprocessing;
using MAXIMUS.Core.Libraries;
using NPOI.XWPF.UserModel;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using static NPOI.HSSF.UserModel.HeaderFooter;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS.ReSendNotices
{
    public class ReSendNotificationHelper
    {
        public static bool SendNotification(int regId, string bodyTemplate, string subject, string recipients)
        {
            bool rtn = false;
            try
            {
                switch (bodyTemplate)
                {
                    case "EMAIL_TEMPLATE_REVALIDATION_REQUEST":
                        rtn = RevalidationRequest(regId, subject, recipients);
                        break;
                    default:
                        ReSendNoticesHelper.CreateLogEntry("SendNotification", "Default Block", CON.ReSendNotices.LogPriorityError);
                        break;
                }
            }
            catch (Exception ex)
            {
                ReSendNoticesHelper.CreateLogEntry("UpdateReSendEmailNoticesStatus", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
                return false;
            }
            return rtn;
        }

        private static bool RevalidationRequest(int regId, string subject, string recipients)
        {
            try
            {
                string templateActualPath = AppSettings.Get("ReSendNoticesProcessFolder", string.Empty) + @"\Templates";

                Dictionary<string, object> fields1 = new Dictionary<string, object>();
                DataSet dsProv = ReSendNoticesHelper.SelectREG_PROVIDER(regId);
                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;
                if (hasRows)
                {
                    fields1.Add("LEGALNAME", dsProv.Tables[0].Rows[0]["NAME"].ToString());
                    fields1.Add("NPI", dsProv.Tables[0].Rows[0]["NPI"].ToString());
                    fields1.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields1.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                    SendEmailNotification(templateActualPath + @"/RevalidationRequest.txt", fields1, true, regId, subject, recipients);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ReSendNoticesHelper.CreateLogEntry("RevalidationRequest", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
                return false;
            }
        }

     
        public static void SendEmailNotification(string templateActualPath, Dictionary<string, object> fields, bool isHTML, int regId, string subject, string recipients)
        {
            string GenericTemplatePath = AppSettings.Get("ReSendNoticesProcessFolder", string.Empty) + @"\Templates";
            GenericTemplatePath = GenericTemplatePath + "//GenericNoticeTemplate.txt";
            string genericBody = SendGenericNotification(GenericTemplatePath, regId, isHTML, subject, recipients);
            if (regId != null)
            {
                string smsMsg = AppSettings.Get("SmsDefaultMessage", "You have a new message from OHIO Medicaid.  Please login to your account to view it.");
                SendSMS(smsMsg, regId, subject);
            }
            Dictionary<string, object> emailFields = fields;
            Dictionary<string, object> paperFields = fields;

            string emailBody = SendNotification(templateActualPath, string.Empty, emailFields, isHTML, new List<string>());


            CreateEmailCommunicationEvent(emailFields, new Guid(CON.ReSendNotices.ReSendNoticesAppID), regId, "RevalidationRequest.txt",
                emailBody, subject, recipients, true, "", genericBody);

        }

        public static void SendPaperNotification(string templateActualPath, Dictionary<string, object> fields, bool isHTML, int regId, string subject, string recipients)
        {
            Dictionary<string, object> paperFields = fields;
            string paperBody = SendPaperNotification(templateActualPath, paperFields, isHTML, regId, subject);

            CreatePaperCommunicationEvent(paperFields, new Guid(CON.ReSendNotices.ReSendNoticesAppID), regId, "RevalidationRequest.txt",
                paperBody, subject, recipients, true, "");
        }

        public static string SendNotification(string templateActualPath, string emailTemplateConfigSetting, 
            Dictionary<string, object> fields, bool isHTML, List<string> attachmentFileNames)
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
                emailMessage = null;
                return body;
            }
            catch (Exception ex)
            {
                ReSendNoticesHelper.CreateLogEntry("RevalidationRequest", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
                return null;
            }
        }


        public static string SendSMS(string Body, int regId, string subject)
        {
            string resultMsg = "";
            try
            {
                bool CanSMS = false;
                DataSet ds = ReSendNoticesHelper.SelectProviderAddressInfo(regId);
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
                            sms.mobile_number = AppSettings.Get("TestSMSMobileNumber", "8178211257");
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
                        if (updateUser == Guid.Empty) updateUser = Guid.Parse(CON.ReSendNotices.ReSendNoticesAppID);
                        param.Add(new SqlParameter("UpdateUser", updateUser));
                        ReSendNoticesHelper.InsertSMS("sp_insertSMS", param);
                    }
                }
            }
            catch (Exception ex)
            {
                ReSendNoticesHelper.CreateLogEntry("RevalidationRequest", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
                return null;
            }
            return resultMsg;
        }

        public static string SendGenericNotification(string templateActualPath, int reg_id, bool isHTML, string subject, string recipients)
        {
            try
            {
                TemplateEvaluator template = new TemplateEvaluator();
                template.Load(templateActualPath);
                MailMessage emailMessage = new MailMessage();

                //0HPNM - 13418 : Add MED ID and Provider Name to the Provider Email Notification  
                string providerName = string.Empty;
                string medicaidId = string.Empty;
                DataSet ds = ReSendNoticesHelper.SelectREG_PROVIDER(reg_id);
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
                string body1 = template.Eval(TemplateFieldAccessors.DictionaryAccessor(fields));
                emailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body1, null, (isHTML ? "text/html" : "text/plain")));
                SendMessage(emailMessage, isHTML, false, false, "", subject, recipients);
                
                return body1;
            }
            catch (Exception ex)
            {
                ReSendNoticesHelper.CreateLogEntry("RevalidationRequest", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
                return null;
            }
        }

        public static void CreateEmailCommunicationEvent(Dictionary<string, object> fields, Guid userId, int regId, 
            string templateName, string body, string subject, string recipients, bool isEmailSent, string log_message, string genericBody)
        {
            // Create the communicaton event and email
            string comTypeID = "41";
            string emailFrom = AppSettings.Get("SmtpFromEmailAddress", string.Empty);

            // create parameters objects and fill with values
            List<SqlParameter>  parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_TYPE_ID", Convert.ToInt32(comTypeID)));
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_VALUE", string.Empty));
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("EMAIL_FROM", emailFrom));
            parameters.Add(new SqlParameter("EMAIL_TO", recipients));
            parameters.Add(new SqlParameter("SUBJECT", subject));
            parameters.Add(new SqlParameter("BODY", body));
            parameters.Add(new SqlParameter("TEMPLATE_NAME", templateName));
            parameters.Add(new SqlParameter("KEY_VALUE_PAIR", ReSendNoticesHelper.GetKeyValueString(fields)));
            parameters.Add(new SqlParameter("REG_ID", regId));
            parameters.Add(new SqlParameter("USER_ID", userId));
            parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId));
            parameters.Add(new SqlParameter("isEmailSent", isEmailSent));
            parameters.Add(new SqlParameter("log_message", log_message));
            parameters.Add(new SqlParameter("GENERIC_BODY", genericBody));
            ReSendNoticesHelper.InsertCOMMUNICATIONEVENT_AND_EMAIL("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);

        }

        public static string SendPaperNotification(string templateActualPath, Dictionary<string, object> fields, bool isHTML, int regID, string subject)
        {
            string body = string.Empty;
            try
            {
                fields.Add("FONTSIZE", "14pt");
                fields.Add("DISPLAYOMR", "<br />");

                TemplateEvaluator template = new TemplateEvaluator();
                string path;
                if (!string.IsNullOrEmpty(templateActualPath)) path = templateActualPath;
                else
                {
                    path = System.AppDomain.CurrentDomain.BaseDirectory;     //Done: Joe - this is to Get the base directory that the assembly resolver uses to probe for assemblies.
                }
                template.Load(path);
                body = template.Eval(TemplateFieldAccessors.DictionaryAccessor(fields));

                string smsMsg = AppSettings.Get("SmsDefaultMessage",
                    "You have a new message from OHIO Medicaid.  Please login to your account to view it.");
                SendSMS(smsMsg, regID, subject);

                return body;
            }
            catch (Exception ex)
            {
                ReSendNoticesHelper.CreateLogEntry("CreatePaperCommunicationEvent", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
                return null;
            }
        }

        public static void CreatePaperCommunicationEvent(Dictionary<string, object> fields, Guid userId, int regId, string templateName, 
            string body, string subject, string recipients, bool isEmailSent, string log_message)
        {

            try
            {
                //code to add bookmark
                object mailpiece;
                if (!fields.TryGetValue("MAILPIECE", out mailpiece))
                {
                    
                }

                if (!string.IsNullOrEmpty(mailpiece.ToString()))
                {
                    body = mailpiece.ToString() + body;
                }

                int comTypeID = ReSendNoticesHelper.SelectCommunicationEventTypes("Print");
                ReSendNoticesHelper.InsertCOMMUNICATIONEVENT_AND_MAIL(comTypeID, subject, body, templateName, ReSendNoticesHelper.GetKeyValueString(fields), regId);
            }
            catch(Exception ex)
            {
                ReSendNoticesHelper.CreateLogEntry("CreatePaperCommunicationEvent", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
            }
        }


        public static string ConstructSubject(string subject)
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

        public static void SendMessage(MailMessage emailMessage, bool isHTML, bool sendSSL, bool bypassTestEmail, string bccList, string subject, string recipients)
        {

            const char delimiter1 = ',';
            const char delimiter2 = ';';
            string testing = bool.TrueString.ToLower();

            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(new Guid(CON.ReSendNotices.ReSendNoticesAppID), logMsg);

            try
            {
                string emailSubject = ConstructSubject(subject);

                bool testEmail = false;
                // if testing enabled
                testing = AppSettings.Get("TestEmailEnabled", bool.TrueString.ToLower());
                if (testing.ToLower() == bool.TrueString.ToLower() && bypassTestEmail == false)
                {
                    recipients = AppSettings.Get("TestEmailAddress", "rohitnagvenkar@maximus.com");
                    testEmail = true;
                }
                else
                {
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                }

                // add the To email address(es)
                char delimiter = delimiter1;
                if (recipients.IndexOf(delimiter1) == -1) delimiter = delimiter2;
                string[] Addrs = recipients.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
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

                // send it
                smtpclient.Send(emailMessage);
            }
            catch (SmtpFailedRecipientException ex)
            {
                
            }
            catch (SmtpException ex)
            {
                
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
