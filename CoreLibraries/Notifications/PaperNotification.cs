using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace MAXIMUS.Core.Libraries
{
    public class PaperNotification : Notification
    {
        #region "Constructors"
        /// <summary>
        ///     The default parameterless constructor. Generates a new GUID for the Logging threadId
        /// </summary>
        public PaperNotification() : base()
        {
        }

        /// <summary>
        ///     The parameterized constructor which provides the Logging threadId GUID.
        /// </summary>
        /// <param name="threadId">The GUID which is used for tying all log entires across all classes.</param>
        public PaperNotification(string subject, Guid threadId = new Guid())
        {
            this.subject = subject;
            ThreadId = threadId == Guid.Empty ? Guid.NewGuid() : threadId;
        }

        #endregion

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
        public override string SendNotification(string templateActualPath, Dictionary<string, object> fields, bool isHTML = true)
        {
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
                SendSMS(smsMsg, Reg_ID);

                return body;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ThreadId, ex);
            }
         }
        public override void GetFromAddress(Dictionary<string, object> fields)
        {
            fields.Add("FROMADDRESS", "");
        }
        public override void CreateCommunicationEvent(Dictionary<string, object> fields, Guid userId, string regId, string templateName)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            string logMessage = string.Empty;
            Logging log = new Logging(ThreadId, logMsg);

            //code to add bookmark
            object mailpiece;
            if (!fields.TryGetValue("MAILPIECE", out mailpiece))
            {
                logMessage = "MailPiece Not Found [Subject: {0} :: Reg ID: {1}]";
                logMessage = String.Format(logMessage, subject, regId);
                log.CreateLogEntry(logMessage, +logCnt);
            }

            if (!string.IsNullOrEmpty(mailpiece.ToString()))
            {
                body = mailpiece.ToString() + body;
            }

            // Create the communicaton event and email
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CommunicationEventType", "Print"));
            string comTypeID = ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

            // create parameters objects and fill with values
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_TYPE_ID", Convert.ToInt32(comTypeID)));
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_VALUE", string.Empty));
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("SUBJECT", subject));
            parameters.Add(new SqlParameter("BODY", body));
            parameters.Add(new SqlParameter("TEMPLATE_NAME", templateName));
            parameters.Add(new SqlParameter("KEY_VALUE_PAIR", GetKeyValueString(fields)));
            parameters.Add(new SqlParameter("REG_ID", regId));
            parameters.Add(new SqlParameter("USER_ID", userId));
            parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId));
            ExecuteStoredProcedure("sp_insertCOMMUNICATIONEVENT_AND_MAIL", parameters);

        }
    }
}
