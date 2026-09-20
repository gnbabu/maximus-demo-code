using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;


namespace MAXIMUS.DataExchange.PDMS
{
    public class MassEmailProcessor : BaseJob, IJob
    {
        #region "Constructors"
        public MassEmailProcessor(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }
        #endregion

        #region "Logging Objects"

        private int logCnt = 0;

        #endregion

        #region "Public Metods"

        public override void ExecuteJob()
        {
            // Default Job - MMIS Submit New Groups
            this.ExecuteJob(Guid.Parse("72777AAB-5FFE-4974-BFB3-10A2C750D617"));
        }

        public override void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();

            // create log object
            string logProcessName = string.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logProcessName);
			
            try
            {

                List<EmailBatch> batchRecordsToProcess = EmailBatchJobsToProcess();
                if (!batchRecordsToProcess.Any())
                {
                    log.CreateLogEntry("No bulk emails to process.");
                    return;
                }


                foreach (var batch in batchRecordsToProcess)
                {
                    // Get EmailQueue items to process by batch

                    List<EmailQueueItem> recordsToProcess = GetQueueItemsToProcess(batch.EmailBatchId);
                    if (!recordsToProcess.Any())
                    {
                        log.CreateLogEntry("No emails to process for batch id: "+ batch.EmailBatchId);
                        continue;
                    }

                    Notification notify = new Notification();
                    int emailSentCount = 0;
                    int paperMailSentCount = 0;
                    foreach (EmailQueueItem item in recordsToProcess)
                    {
                        try
                        {
                            string regId=GetRegIDFromKeyValueFieldsPair(item.EmailValues);

                            // send topartyid as adminpartyid to fix communication not generated when party id is not available. //DCPDMS-2774

                            //if Paper Email Template then send both Paper mail and Email
                            string paperTemplates = AppSettings.Get("PaperEmailTemplates", string.Empty);
                            if (paperTemplates.Contains(item.TemplateId.ToString()))
                            {
                                CreateCommunicationEvent(
                                   item.EmailValues
                                   , new Guid()
                                   , item.RegId.ToString()
                                   , "Email Template Id from EmailTemplates: " + item.TemplateId.ToString()
                                   , item.EmailSubject
                                   , item.ParsedEmailBody);

                                paperMailSentCount++;

                                if (!string.IsNullOrEmpty(item.EmailAddresses))
                                {
                                    notify.SendNotification(item.EmailAddresses, item.EmailSubject, item.ParsedEmailBody, true);

                                    CreateCommunicationEvent(
                                        AppSettings.Get("SmtpFromEmailAddress", string.Empty)
                                        , item.EmailAddresses
                                        , item.EmailSubject
                                        , item.ParsedEmailBody
                                        , item.EmailValues
                                        , new Guid()
                                        , item.RegId.ToString()
                                        , "Email Template Id: " + item.TemplateId, notify.isEmailSent, notify.log_message);
                                    emailSentCount++;
                                }

                            }
                            else
                            {
                                //add logic for paper mails
                                if (Methods.RequirePaperNotice(item.EmailAddresses, AppSettings.Get("PSEmailTypes", string.Empty).Split(',')))
                                {
                                    //for paper mails create mail event.
                                    //create a communication event for paper mails   
                                    // private void CreateCommunicationEvent(string fields, Guid userId, string regId, string templateName, string subject, string body)

                                    CreateCommunicationEvent(
                                        item.EmailValues
                                        , new Guid()
                                        , item.RegId.ToString()
                                        , "Email Template Id from EmailTemplates: " + item.TemplateId.ToString()
                                        , item.EmailSubject
                                        , item.ParsedEmailBody);

                                    paperMailSentCount++;
                                }
                                else
                                {
                                    notify.SendNotification(item.EmailAddresses, item.EmailSubject, item.ParsedEmailBody, true);

                                    CreateCommunicationEvent(
                                        AppSettings.Get("SmtpFromEmailAddress", string.Empty)
                                        , item.EmailAddresses
                                        , item.EmailSubject
                                        , item.ParsedEmailBody
                                        , item.EmailValues
                                        , new Guid()
                                        , item.RegId.ToString()
                                        , "Email Template Id: " + item.TemplateId, notify.isEmailSent, notify.log_message);
                                    emailSentCount++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            log.CreateLogEntry("Problem sending email EmailQueueId "+ item.EmailQueueId + "[Exception: " + ex.Message);
                            // Log the message and go
                            try
                            {
                            }
                            catch (Exception ex2)
                            {
                                // do nothing
                                log.CreateLogEntry("Problem updating EmailQueueId " + item.EmailQueueId + "[Exception: " + ex2.Message);
                            }
                        }
                    }// End of email queue loop


                    var TnEmailAddressList = AppSettings.Get("Jobs-Email-72777AAB-5FFE-4974-BFB3-10A2C750D617");
                    //Send report to TNCare
                    string subject = "Mass email summary recap.";
                    string body = string.Format(@"<P>Total Records requested to be emailed: {0}<br/>Total email's successfully sent: {1}<br/>Total paper mail's successfully sent: {2}<br/>Batch Job: {3}</P>", recordsToProcess.Count, emailSentCount, paperMailSentCount, batch.EmailBatchId);
                    notify.SendNotification(TnEmailAddressList, subject, body, true);


                    // Update the batch record (updates with the EmailSendDateTime and JobComplete = true
                    BatchJobUpdate(batch.EmailBatchId);

                }// End of batch record loop
            }
            catch (Exception ex)
            {
				throw ex;
            }
        }


        #endregion

        #region PRIVATE METHODS

        private void AddCommunicationEvent(Enumerations.CommunicationEventTypeId eventType, int fromParty, int toParty
            , DateTime eventDate, string eventValue)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(eventType), true));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATED_FROM_PARTY_ID", DbType.Int32, fromParty, true));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATED_TO_PARTY_ID", DbType.Int32, toParty, true));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, eventDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, eventValue, true));

                DataAccess.ExecuteStoredProcedure("insertCOMMUNICATION_EVENT", parameters);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Communication Event", eventType.ToString(), ex.Message, "Event Value: " + eventValue);
            }
        }

        private void CreateCommunicationEvent(
            string emailFrom
            , string emailTo
            , string subject
            , string body
            , string keyValuePairsAsString
            , Guid userId
            , string regId
            , string templateName
            , bool isEmailSent
            , string log_message)
        {
            // Create the communication event and email
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CommunicationEventType", "PROVIDER EMAIL OUT"));
            string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

            // create parameters objects and fill with values
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_TYPE_ID", Convert.ToInt32(comTypeID)));
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_VALUE", string.Empty));
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("EMAIL_FROM", emailFrom));
            parameters.Add(new SqlParameter("EMAIL_TO", emailTo));
            parameters.Add(new SqlParameter("SUBJECT", subject));
            parameters.Add(new SqlParameter("BODY", body));
            parameters.Add(new SqlParameter("TEMPLATE_NAME", templateName));
            parameters.Add(new SqlParameter("KEY_VALUE_PAIR", keyValuePairsAsString));
            parameters.Add(new SqlParameter("REG_ID", regId));
            parameters.Add(new SqlParameter("USER_ID", userId));
            parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId));
            parameters.Add(new SqlParameter("isEmailSent", isEmailSent));
            parameters.Add(new SqlParameter("log_message", log_message));
            DataAccess.ExecuteStoredProcedure("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);
      
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId));
            string comEventID = DataAccess.ExecuteScalar("sp_SelectLastCommunicationEventID", parameters);

            // generated by sp_Admin_StoredProcBuilder on Oct  3 2012  2:44PM
            // create parameters objects and fill with values
            parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", DateTime.Now));
            parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Methods.GetCurrentUserId()));
            parameters.Add(new SqlParameter("COMMUNICATION_EVENT_ID", comEventID));
            parameters.Add(new SqlParameter("ERROR_TYPE_ID",Constants.errortypeid));
            parameters.Add(new SqlParameter("CREATED_BY_USER",Methods.GetCurrentUserId()));
            parameters.Add(new SqlParameter("CREATED_ON_DATE_TIME", DateTime.Now));
          

            DataAccess.ExecuteStoredProcedure("usp_InsertCommunicationEventIdsErrorHistory", parameters);
        }

        private void LogErrorRecord(Logging log, string recordType, string recordValue, string exceptionMessage, string additionRecordInformation = "")
        {
            if (additionRecordInformation.Length > 0)
            {
                additionRecordInformation = ", " + additionRecordInformation;
            }
            string message = String.Format(Constants.LogString.ExtractErrorRecordFailure,null, recordType + ": " + recordValue, additionRecordInformation);
            message += Environment.NewLine + exceptionMessage;
            log.CreateLogEntry(message, Logging.LogPriority.DataLoadIssues);
        }

        //private int GetAdminPartyId()
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>();
        //    string rtn = DataAccess.ExecuteScalar("sp_SelectAdminPartyID", parameters);
        //    if (string.IsNullOrEmpty(rtn))
        //    {
        //        return 0;
        //    }
        //    return Convert.ToInt32(rtn);
        //}

        private List<EmailBatch> EmailBatchJobsToProcess()
        {
            List<EmailBatch> toProcess = new List<EmailBatch>();
            try
            {
                DataTable dt = null;
                var records = DataAccess.ExecuteStoredProcedure("usp_EmailBatchJobsToProcess", "RecordsToProcess");

                if (records.Tables.Count < 1)
                {
                    dt = records.Tables[0];
                    if (dt.Rows.Count < 1)
                        return toProcess;
                }
                dt = records.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    EmailBatch batch = new EmailBatch();
                    batch.EmailBatchId = Convert.ToInt32(row["EmailBatchId"]);
                    batch.EmailSubject = Convert.ToString(row["EmailSubject"]);
                    batch.TemplateId = Convert.ToInt32(row["TemplateId"]);
                    batch.JobComplete = Convert.ToBoolean(row["JobComplete"]);

                    if (row["CreateDateTime"] != null)
                    {
                        batch.CreateDateTime = Convert.ToDateTime(row["CreateDateTime"]);
                    }

                    toProcess.Add(batch);
                }

                return toProcess;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        private List<EmailQueueItem> GetQueueItemsToProcess(int batchId)
        {
            List<EmailQueueItem> queue = new List<EmailQueueItem>();
            DataTable dt = null;


            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("BatchId", batchId));
            var records = DataAccess.ExecuteStoredProcedure("usp_GetEmailQueueToProcess", sqlParams, "RecordsToProcess");

            if (records.Tables.Count < 1)
            {
                dt = records.Tables[0];
                if (dt.Rows.Count < 1)
                    return null;
            }

            dt = records.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                EmailQueueItem item = new EmailQueueItem();
                item.EmailQueueId = Convert.ToInt32(row["EmailQueueId"]);
                item.BatchId = Convert.ToInt32(row["BatchId"]);
                item.TemplateId = Convert.ToInt32(row["TemplateId"]);
                item.NPI = Convert.ToString(row["NPI"]);
                item.TaxId = Convert.ToString(row["TaxId"]);
                item.RegId = Convert.ToInt32(row["Reg_Id"]);
                item.Name = Convert.ToString(row["Name"]);
                item.EmailAddresses = Convert.ToString(row["EmailAddress"]);
                item.IsSent = Convert.ToBoolean(row["IsSent"]);
                item.TemplateBody = Convert.ToString(row["TemplateBody"]);
                item.ParsedEmailBody = Convert.ToString(row["ParsedEmailBody"]);
                item.EmailFields = Convert.ToString(row["EmailFields"]);
                item.EmailValues = Convert.ToString(row["EmailValues"]);
                item.EmailSubject = Convert.ToString(row["EmailSubject"]);

                if(row["CreateDateTime"] != null)
                {
                    item.CreateDateTime = Convert.ToDateTime(row["CreateDateTime"]);
                }

                queue.Add(item);
            }
            return queue;
        }

        private void BatchJobUpdate(int batchJobId)
        {
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("EmailBatchId", batchJobId));

                DataAccess.ExecuteStoredProcedure("usp_EmailJobUpdate", sqlParams);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }


        //Used for paper notifications

        private void CreateCommunicationEvent(string fields, Guid userId, string regId, string templateName, string subject, string body)
	    //public override void CreateCommunicationEvent(int fromPartyId, int toPartyId, Dictionary<string, object> fields, Guid userId, string regId, string templateName)
        {
	        // Create the communication event and email
	        var dtNow = DateTime.Now;
	        var parameters = new List<SqlParameter>();
	        parameters.Add(new SqlParameter("CommunicationEventType", "Print"));
	        var comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

	        // create parameters objects and fill with values
	        parameters = new List<SqlParameter>();
	        parameters.Add(new SqlParameter("COMMUNICATION_EVENT_TYPE_ID", Convert.ToInt32(comTypeID)));
	        parameters.Add(new SqlParameter("COMMUNICATION_EVENT_VALUE", string.Empty));
	        parameters.Add(new SqlParameter("COMMUNICATION_EVENT_DATE_TIME", dtNow));
	        parameters.Add(new SqlParameter("SUBJECT", subject));
	        parameters.Add(new SqlParameter("BODY", body));
	        parameters.Add(new SqlParameter("TEMPLATE_NAME", templateName));
	        parameters.Add(new SqlParameter("KEY_VALUE_PAIR", fields)); //fields already contains key value pairs
	        parameters.Add(new SqlParameter("REG_ID", regId));
	        parameters.Add(new SqlParameter("USER_ID", userId));
	        parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", DateTime.Now));
	        parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId));
	        DataAccess.ExecuteStoredProcedure("sp_insertCOMMUNICATIONEVENT_AND_MAIL", parameters);

	        var logMsg = string.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
	        var log = new Logging(ThreadId, logMsg);
	        var logMessage = "Paper notice created successfully [Subject: {0} :: Reg ID: {1}]";
	        logMessage = string.Format(logMessage, subject, regId);
	        log.CreateLogEntry(logMessage, +logCnt);


	        //Add UpdateCommunicationEventForParty functionality
	        parameters = new List<SqlParameter>();
	        parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", dtNow));
	        parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId));
	        var comEventID = DataAccess.ExecuteScalar("sp_SelectLastCommunicationEventID", parameters);

	        // generated by sp_Admin_StoredProcBuilder on Oct  3 2012  2:44PM
	        // create parameters objects and fill with values
	        parameters = new List<SqlParameter>();

            //parameters.Add(new SqlParameter("PARTY_ID", toPartyId));
            //PARTY_ERROR table not existed in Database. So I just commented this code
            //   parameters.Add(new SqlParameter("REG_ID", regId));
            //   parameters.Add(new SqlParameter("COMMUNICATION_EVENT_ID", comEventID));
            //parameters.Add(new SqlParameter("RESOLVING_ACTION_TYPE_ID",
            // Constants.ResolvingActionType.SystemGeneratedEmail));
            //parameters.Add(new SqlParameter("LAST_MODIFIED_DATE_TIME", dtNow));
            //parameters.Add(new SqlParameter("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId));

            //DataAccess.ExecuteStoredProcedure("sp_UpdateCommunicationEventIdsForPartyErrorHistory", parameters);
        }

        private string GetRegIDFromKeyValueFieldsPair(string KeyValuePair)
        {
            string regId=string.Empty;
            
            string logProcessName = string.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logProcessName);

            try
            {
                if(!string.IsNullOrEmpty(KeyValuePair))
                {
                    string[] items = KeyValuePair.TrimEnd(';').Split(';');
                    foreach (string item in items)
                    {
                          string[] keyValue = item.Split(':');
                                  
                            if(keyValue[0].ToString().ToUpper().Contains("REGID"))
                            {
                                //Then get the regid
                                regId=keyValue[1].ToString().Trim();
                                break;
                            }     
                    }
                }

            }
            catch(Exception ex)
            {
                log.CreateLogEntry("Failed to Get RegID from KeyValue Pairs. "
                                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                                 + ex.StackTrace, Logging.LogPriority.Error);
                regId =string.Empty;
            }
            return regId;
        }

    }
        #endregion

    }

