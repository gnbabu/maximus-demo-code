using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using mt = MAXIMUS.DataExchange.PDMS.TN_MMIS_Service_Test;

namespace MAXIMUS.DataExchange.PDMS
{
    public class MMISRetrieveGroups : BaseJob, IJob
    {
        #region "Constructors"

        public MMISRetrieveGroups(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        #endregion
        
        #region "Class Level Declarations"
        #endregion

        #region "Public Methods"

        override public void ExecuteJob()
        {
            // Default Job - MMIS - Retrieve Groups (Retrieve all group information)
            this.ExecuteJob(Guid.Parse("1F5C48DF-11F1-490B-B8B9-787598594258"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                // MMIS - Retrieve Groups
                case "1F5C48DF-11F1-490B-B8B9-787598594258":
                    this.RetrieveGroups(Constants.TransactionType.RetrieveAllGroupsfromMMIS);
                    this.RetrieveAffiliations();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Resends affiliations returned by the procedure usp_ReprocessAffiliationEmails
        /// </summary>
        public void ResendAffiliationNotifications()
        {
            // create log object
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logProcessName);

            string location = string.Empty;
            string obj = string.Empty;
            DataSet group;
            DateTime now = DateTime.Now;

            try
            {
                MMISShared ms = new MMISShared(this.ThreadId);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());

                // get the staging records
                int transactionType = Constants.TransactionType.ResendAffiliationNotifications;
                group = ms.GetSubmittedStagingRecords(transactionType);

                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart, group.Tables[0].Rows.Count, transactionType));

                // loop over records in dataset
                foreach (DataRow groupRow in group.Tables[0].Rows)
                {
                    int partyId = Convert.ToInt32(groupRow["PARTY_ID"]);
                    string sakTrans = Methods.GetStringValue(groupRow["SAK_TRANS"]);

                    foreach (DataRow affilRow in groupRow.GetChildRows(MMISShared.dbrGrps2dbtAffs))
                    {
                        string affilPDMSId = Methods.GetStringValue(affilRow["MMIS_STAGING_AFFILIATE_PK"]);

                        try
                        {
                            DateTime termDate = Convert.ToDateTime(Methods.GetDateTimeValue(affilRow["TERM_DATE"], false));
                            string affilAction = Methods.GetStringValue(affilRow["ACTION"]);

                            if (affilAction == "AM" && termDate.CompareTo(DateTime.MinValue) == 0)
                            {
                                // Send welcome letter to provider
                                Notification n = new Notification(this.ThreadId);
                                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                                string subject = "Affiliation Confirmation";
                                string recipients = string.Empty;
                                Dictionary<string, object> fields = new Dictionary<string, object>();
                                fields.Add("CURRENTDATE", Methods.GetShortDate(DateTime.Now.ToString()));
                                fields.Add("LEGALNAME", ObjectControllerHelper.GetString("NAME", groupRow));
                                fields.Add("PRIMARYCONTACTNAME", ObjectControllerHelper.GetString("ServicingAddressName", groupRow));
                                fields.Add("GROUPNPI", ObjectControllerHelper.GetString("NPI", groupRow));
                                fields.Add("PRIMARYCONTACTADDRESS1", ObjectControllerHelper.GetString("ServicingAddress1", groupRow));
                                fields.Add("GROUPMEDICAID", ObjectControllerHelper.GetString("MEDICAID_ID", groupRow));
                                fields.Add("PRIMARYCONTACTADDRESS2", ObjectControllerHelper.GetString("ServicingAddress2", groupRow));
                                fields.Add("EFFECTIVEDATE", Methods.GetShortDate(ObjectControllerHelper.GetString("EFFECTIVE_DATE", groupRow)));
                                fields.Add("GROUPCITYSTATEZIP", ObjectControllerHelper.GetString("ServicingCity", groupRow) + ", " + ObjectControllerHelper.GetString("ServicingState", groupRow) + " " + ObjectControllerHelper.GetString("ServicingZip", groupRow));
                                fields.Add("PDMSURL", AppSettings.Get("PDMSURL"));
                                fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL",string.Empty));
                                string entityTypeId = "";
                                entityTypeId = ObjectControllerHelper.GetString("ProviderCategoryTypeID", groupRow);

                                if (entityTypeId == "2")
                                {
                                    fields.Add("NPILabel", "Type 2 NPI");
                                    fields.Add("GroupMedicaidLabel", "Group ");
                                    fields.Add("grouplabel", AppSettings.Get("group "));
                                    fields.Add("groupOrPracticeLabel", "group");
                                }
                                else
                                {
                                    fields.Add("NPILabel", "NPI");
                                    fields.Add("GroupMedicaidLabel", "");
                                    fields.Add("grouplabel", AppSettings.Get("group "));
                                    fields.Add("groupOrPracticeLabel", "group");
                                }

                                string userEmail = ObjectControllerHelper.GetString("Email", groupRow);
                                string contactEmail = ObjectControllerHelper.GetString("CONTACT_EMAIL_ADDRESS", groupRow);
                                bool hasTwo = (!string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(contactEmail));
                                bool isDuplicate = userEmail.Equals(contactEmail, StringComparison.InvariantCultureIgnoreCase);
                                recipients = userEmail + (!isDuplicate && hasTwo ? "," : string.Empty) + (isDuplicate ? string.Empty : contactEmail);

                                //Get individual level data by part_id
                                //set field value if it exits. otherwise, add it
                                if (fields.ContainsKey("INDIVIDUALPROVIDERNAME"))
                                {
                                    fields["INDIVIDUALPROVIDERNAME"] = ObjectControllerHelper.GetString("ProviderName", affilRow);
                                    fields["INDIVIDUALNPI"] = ObjectControllerHelper.GetString("NPI", affilRow);
                                    fields["INDIVIDUALMEDICAIDID"] = ObjectControllerHelper.GetString("BaseMedicaidID", affilRow);
                                    fields["AFFILIATIONDATE"] = Methods.GetShortDate(ObjectControllerHelper.GetString("AffiliationDate", affilRow));
                                }
                                else
                                {
                                    fields.Add("INDIVIDUALPROVIDERNAME", ObjectControllerHelper.GetString("ProviderName", affilRow));
                                    fields.Add("INDIVIDUALNPI", ObjectControllerHelper.GetString("NPI", affilRow));
                                    fields.Add("INDIVIDUALMEDICAIDID", ObjectControllerHelper.GetString("BaseMedicaidID", affilRow));
                                    fields.Add("AFFILIATIONDATE", Methods.GetShortDate(ObjectControllerHelper.GetString("AffiliationDate", affilRow)));
                                    //recipients = ObjectControllerHelper.GetString("Recipient", affilRow);
                                }


                                Notification notify = null;
                                string body = String.Empty;
                                if (Methods.RequirePaperNotice(recipients, AppSettings.Get("PSEmailTypes", string.Empty).Split(',')))
                                {
                                    notify = new PaperNotification(subject, new Guid());
                                }
                                else
                                {
                                    notify = new EMailNotification(body, subject, recipients, new Guid());
                                }
                                body = notify.SendNotification(templateActualPath + @"/AffiliationConfirmationLetter.txt", fields);

                                notify.CreateCommunicationEvent(fields, new Guid(), "0", "AffiliationConfirmationLetter.txt");
                                                                 
                            }   // affiliation add if statement
                        }
                        catch (Exception ex)
                        {
                            CoreException.ThrowException(this.ThreadId, ex, logProcessName);
                            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, affilPDMSId, sakTrans));
                        }
                    } // affiliation loop
                }   // group loop
            }   // try statement
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
        }

        /// <summary>
        ///     Retrieves the submitted groups and/or affiliates from the MMIS.
        /// </summary>
        /// <param name="transactionType">Common.Constants.TransactionType</param>
        public void RetrieveGroups(int transactionType)
        {
            // create log object
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logProcessName);

            try
            {
                switch (transactionType)
                {
                    case (int)Constants.TransactionType.RetrieveAllGroupsfromMMIS:
                        RetrieveGroupsByTransaction(Constants.TransactionType.RequestMedicaidIDfromMMIS);
                        RetrieveGroupsByTransaction(Constants.TransactionType.SendProviderUpdatestoMMIS);
                        RetrieveGroupsByTransaction(Constants.TransactionType.SendProviderStatusChangetoMMIS);
                        RetrieveGroupsByTransaction(Constants.TransactionType.SendPaymentInfotoMMIS);
                        break;

                    case (int)Constants.TransactionType.RetrieveGroupAffiliationsfromMMIS:
                        // RetrieveGroupsByTransaction(Constants.TransactionType.SendGroupAffiliationstoMMIS);
                        RetrieveAffiliations();
                        break;

                    default:
                        string msg = "RetrieveGroups has not implement this Transaction Type Id";
                        throw new NotImplementedException(msg);
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
        }

        #endregion

        #region "Private Methods"

        private void CreateCommunicationEvent(int RegId, string emailFrom, string templateName, string emailTo, string subject, string body, Dictionary<string, object> fields, Guid userId, bool isEmailSent, string log_message)
        {
            // Create the communicaton event and email
            DateTime now = DateTime.Now;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
            string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

            // create parameters objects and fill with values
            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, now, true));
            parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
            parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, emailTo, false));
            parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
            parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
            parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, templateName, false));
            parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, ObjectControllerHelper.GetKeyValueString(fields), false));
            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, false));
            parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, userId, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
            parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean,isEmailSent,true));
            parameters.Add(SqlParms.CreateParameter("log_message", DbType.String,log_message,true));

            DataAccess.ExecuteStoredProcedure("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
            string comEventID = DataAccess.ExecuteScalar("sp_SelectLastCommunicationEventID", parameters);

            //// generated by sp_Admin_StoredProcBuilder on Oct  3 2012  2:44PM
            //// create parameters objects and fill with values
            parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, true));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_ID", DbType.Int32, comEventID, true));
            parameters.Add(SqlParms.CreateParameter("RESOLVING_ACTION_TYPE_ID", DbType.Int32, Constants.ResolvingActionType.SystemGeneratedEmail, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

            DataAccess.ExecuteStoredProcedure("sp_UpdateCommunicationEventIdsForPartyErrorHistory", parameters);
        }

        private bool HasRows(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasRows(DataTable dt)
        {
            return dt.Rows.Count > 0;
        }

        private void RetrieveAffiliations()
        {
            // create log object
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logProcessName);

            string location = string.Empty;
            string obj = string.Empty;
            DataSet group;
            DateTime now = DateTime.Now;
            int countRow = 0;

            try
            {
                MMISShared ms = new MMISShared(this.ThreadId);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());

                // get the staging records
                int transactionType = Constants.TransactionType.SendGroupAffiliationstoMMIS;
                group = ms.GetSubmittedStagingRecords(transactionType);

                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart, group.Tables[0].Rows.Count, transactionType));

                // loop over records in dataset
                foreach (DataRow groupRow in group.Tables[0].Rows)
                {
                    // create the object to submit and the response
                    mt.ProviderGroupAffiliationsCaqhGetRequest request = new mt.ProviderGroupAffiliationsCaqhGetRequest();
                    mt.ProviderGroupAffiliationsCaqhGetResponse response = new mt.ProviderGroupAffiliationsCaqhGetResponse();

                    string groupPk = string.Empty;
                    string medicaidPk = string.Empty;
                    string sakTrans = string.Empty;

                    try
                    {
                        // populate the object credentials and provider object
                        int partyId = Convert.ToInt32(groupRow["PARTY_ID"]);
                        medicaidPk = Methods.GetStringValue(groupRow["MEDICAID_PK"].ToString());
                        groupPk = Methods.GetStringValue(groupRow[MMISShared.groupStagingPk].ToString());
                        sakTrans = Methods.GetStringValue(groupRow["SAK_TRANS"]);
                        request.sak_trans = sakTrans;

                        // serialize the request to xml and save to filesystem
                        obj = Methods.SerializeObjectToXml(request);
                        Methods.WriteStringToFile(ms.path, obj, groupPk + "Request");

                        request.userId = ms.uid;
                        request.password = ms.pwd;

                        if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                        {
                            // create the web service object and retrieve from the MMIS
                            mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
                            response = mmis.GetProviderGroupAffiliations(request);
                        }
                        else
                        {
                            // get test values
                            string groupStatus = AppSettings.Get("MMIS-TestGroupStatus", Constants.MMISStatusType.Processed.ToString());
                            string groupError = AppSettings.Get("MMIS-TestGroupErrors", string.Empty);
                            string affilStatus = AppSettings.Get("MMIS-TestAffilStatus", Constants.MMISStatusType.Processed.ToString());
                            string affilError = AppSettings.Get("MMIS-TestAffilErrors", string.Empty);

                            countRow += 1;
                            bool process = false;
                            if (countRow <= ms.testRowsCount) process = true;
                            int medicaidLength = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));
                            string medicaidPrefix = new string('2', medicaidLength - 5);
                            string testValue = DateTime.Now.ToString("ssfff");
                            List<mt.Error> errorList = new List<mt.Error>();
                            // List<mt.ProviderMembers> providerList = new List<mt.ProviderMembers>();
                            response.correlationId = "444" + testValue;
                            string medicareId = Methods.GetStringValue(groupRow["MEDICAID_ID"]);
                            response.errorCode = groupError;

                            if (process)
                            {
                                response.statusCode = groupStatus;
                                if (string.IsNullOrWhiteSpace(medicareId))
                                {
                                    response.medicareId = medicaidPrefix + testValue;
                                }
                                else
                                {
                                    response.medicareId = medicareId;
                                }
                                response.pdmsId = Methods.GetStringValue(groupRow[MMISShared.groupStagingPk].ToString());

                                // add affiliates to response
                                // group = ms.GetSubmittedStagingRecords(transactionType);
                                // DataRow groupRecord = group.Tables[0].Rows[0];
                                List<mt.Affiliates> affilList = new List<mt.Affiliates>();

                                // update all transactions for each affiliation
                                foreach (DataRow affiliate in groupRow.GetChildRows(MMISShared.dbrGrps2dbtAffs))
                                {
                                    mt.Affiliates affil = new mt.Affiliates();
                                    affil.action = Methods.GetStringValue(affiliate["ACTION"], false).ToString();
                                    affil.medicareId = Methods.GetStringValue(affiliate["BASE_MEDICAID_ID"], false).ToString();
                                    affil.memberEffectiveDate = Methods.GetDateValue(affiliate["EFFECTIVE_DATE"]);
                                    affil.memberTermDate = Methods.GetDateValue(affiliate["TERM_DATE"]);
                                    affil.npiId = Methods.GetStringValue(affiliate["NPI"], false).ToString();
                                    affil.pdmsId = Methods.GetStringValue(affiliate["MMIS_STAGING_AFFILIATE_PK"], false).ToString();
                                    affil.statusCode = affilStatus;
                                    affil.errorCode = affilError;
                                    affilList.Add(affil);
                                }
                                response.affiliates = affilList.ToArray<mt.Affiliates>();
                            }
                        }

                        // serialize the object to xml and save to filesystem
                        obj = Methods.SerializeObjectToXml(response);
                        location = Methods.WriteStringToFile(ms.path, obj, groupPk + "Response");

                        // log entry
                        log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, groupPk, sakTrans));

                        string statusCode = string.Empty;
                        if (!string.IsNullOrWhiteSpace(response.statusCode))
                        {
                            statusCode = response.statusCode.Trim();

                            // get properties
                            string medicaidId = string.Empty;
                            if (!string.IsNullOrWhiteSpace(response.medicareId))
                            {
                                medicaidId = response.medicareId.Trim();
                            }
                            string errorCodes = string.Empty;
                            if (!string.IsNullOrWhiteSpace(response.errorCode))
                            {
                                errorCodes = response.errorCode.Trim();
                            }
                            string correlationId = response.correlationId.Trim();
                            if (!string.IsNullOrWhiteSpace(response.correlationId))
                            {
                                correlationId = response.correlationId.Trim();
                            }

                            // if MMIS errors are returned
                            ms.ProcessDataErrors(errorCodes, partyId.ToString(), medicaidPk);

                            // generated by sp_Admin_StoredProcBuilder on Sep 12 2013 11:07AM
                            // create parameters objects and fill with values
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_GROUP_PK", DbType.Int32, groupPk, true));
                            parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, sakTrans, true));
                            parameters.Add(SqlParms.CreateParameter("CORRELATION_ID", DbType.String, correlationId, true));
                            parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, errorCodes, true));
                            parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, true));
                            parameters.Add(SqlParms.CreateParameter("STATUS_CODE", DbType.Int32, statusCode, true));
                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                            DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingGroup", parameters);

                            // loop over affiliates and save error codes
                            foreach (mt.Affiliates affiliate in response.affiliates)
                            {
                                string mmisErrors = string.Empty;
                                if (!string.IsNullOrWhiteSpace(affiliate.errorCode))
                                {
                                    mmisErrors = affiliate.errorCode.Trim();
                                }
                                string affilStatus = string.Empty;
                                if (!string.IsNullOrWhiteSpace(affiliate.statusCode))
                                {
                                    affilStatus = affiliate.statusCode.Trim();
                                }
                                string affilPDMSId = string.Empty;
                                if (!string.IsNullOrWhiteSpace(affiliate.pdmsId))
                                {
                                    affilPDMSId = affiliate.pdmsId.Trim();
                                }
                                string affilAction = ms.GetAffiliateAction(affilPDMSId);

                                // generated by sp_Admin_StoredProcBuilder on Sep 24 2013  5:04PM
                                // create parameters objects and fill with values
                                parameters = new List<SqlParameter>();
                                parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_AFFILIATE_PK", DbType.Int32, affilPDMSId, true));
                                parameters.Add(SqlParms.CreateParameter("MMIS_STATUS_TYPE_ID", DbType.Int32, affilStatus, true));
                                parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, mmisErrors, true));
                                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                                DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingAffiliate", parameters);

                                // Bug 3005 -- if affiliation status is empty and group status is not empty
                                if (affilStatus.Length == 0 && statusCode.Length > 0)
                                {
                                    affilStatus = statusCode;
                                }

                                // Bug 3005 -- if affiliation error code(s) is empty and group error code(s) is not empty
                                if (mmisErrors.Length == 0 && errorCodes.Length > 0)
                                {
                                    mmisErrors = errorCodes;
                                    affilStatus = Constants.MMISStatusType.Errors.ToString();
                                }
                                
                                if (affilStatus.Length > 0)
                                {
                                    int affilPartyId = ms.GetAffiliatePartyId(affilPDMSId);

                                    // Update TRANSACTION table with Submit Date information
                                    // -----------------------------------------------------
                                    TransactionController.UpdateTransactionQueue(
                                        Convert.ToInt32(transactionType)
                                        , affilPartyId
                                        , Convert.ToInt32(medicaidPk)
                                        , null
                                        , now
                                        , now
                                        , Constants.appPDMSDataExchangeUserId);

                                    // generated by sp_Admin_StoredProcBuilder on Nov 18 2013  8:40PM
                                    // create parameters objects and fill with values
                                    parameters = new List<SqlParameter>();

                                    parameters.Add(SqlParms.CreateParameter("IndProvPartyID", DbType.Int32, affilPartyId, true));
                                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                                    DataAccess.ExecuteStoredProcedure("usp_UpdateAffiliations", parameters);

                                    if (affilStatus == Constants.MMISStatusType.Processed.ToString())
                                    {
                                        if (AffiliationLetterCount(partyId, affilPartyId) == 0 && affiliate.memberTermDate.CompareTo(DateTime.MinValue) == 0)
                                        {
                                                // Send welcome letter to provider
                                                Notification n = new Notification(this.ThreadId);
                                                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                                                string subject = "Affiliation Confirmation";
                                                string recipients = string.Empty;
                                                Dictionary<string, object> fields = new Dictionary<string, object>();
                                                fields.Add("CURRENTDATE", Methods.GetShortDate(DateTime.Now.ToString()));
                                                fields.Add("LEGALNAME", ObjectControllerHelper.GetString("NAME", groupRow));
                                                fields.Add("PRIMARYCONTACTNAME", ObjectControllerHelper.GetString("ServicingAddressName", groupRow));
                                                fields.Add("GROUPNPI", ObjectControllerHelper.GetString("NPI", groupRow));
                                                fields.Add("PRIMARYCONTACTADDRESS1", ObjectControllerHelper.GetString("ServicingAddress1", groupRow));
                                                fields.Add("GROUPMEDICAID", ObjectControllerHelper.GetString("MEDICAID_ID", groupRow));
                                                fields.Add("PRIMARYCONTACTADDRESS2", ObjectControllerHelper.GetString("ServicingAddress2", groupRow));
                                                fields.Add("EFFECTIVEDATE", Methods.GetShortDate(ObjectControllerHelper.GetString("EFFECTIVE_DATE", groupRow)));
                                                fields.Add("GROUPCITYSTATEZIP", ObjectControllerHelper.GetString("ServicingCity", groupRow) + ", " + ObjectControllerHelper.GetString("ServicingState", groupRow) + " " + ObjectControllerHelper.GetString("ServicingZip", groupRow));
                                                fields.Add("PDMSURL", AppSettings.Get("PDMSURL"));
                                            fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL",string.Empty));
                                            string entityTypeId = "";
                                            entityTypeId = ObjectControllerHelper.GetString("ProviderCategoryTypeID", groupRow);

                                            if (entityTypeId == "2")
                                            {
                                                fields.Add("NPILabel", "Type 2 NPI");
                                                fields.Add("GroupMedicaidLabel", "Group ");
                                                fields.Add("grouplabel", AppSettings.Get("group "));
                                                fields.Add("groupOrPracticeLabel", "group");
                                            }
                                            else
                                            {
                                                fields.Add("NPILabel", "NPI");
                                                fields.Add("GroupMedicaidLabel", "");
                                                fields.Add("grouplabel", AppSettings.Get("group "));
                                                fields.Add("groupOrPracticeLabel", "practice");
                                            }
                                            string userEmail = ObjectControllerHelper.GetString("Email", groupRow);
                                                string contactEmail = ObjectControllerHelper.GetString("CONTACT_EMAIL_ADDRESS", groupRow);
                                                bool hasTwo = (!string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(contactEmail));
                                                bool isDuplicate = userEmail.Equals(contactEmail, StringComparison.InvariantCultureIgnoreCase);
                                                recipients = userEmail + (!isDuplicate && hasTwo ? "," : string.Empty) + (isDuplicate ? string.Empty : contactEmail);

                                                //Send email for each affiliated individual
                                                if (HasRows(group.Tables[2]) && !string.IsNullOrEmpty(affilPartyId.ToString()))
                                                {
                                                    //Get individual level data by part_id
                                                    // DataRow[] rows = group.Tables[2].Select("PARTY_ID = " + affilPartyId.ToString());
                                                    DataRow[] rows = groupRow.GetChildRows(MMISShared.dbrGrps2dbtAffs);
                                                    foreach (DataRow row in rows)
                                                    {
                                                        if (affilPartyId.ToString() == ObjectControllerHelper.GetString("PARTY_ID", row))
                                                        {
                                                            //set field value if it exits. otherwise, add it
                                                            if (fields.ContainsKey("INDIVIDUALPROVIDERNAME"))
                                                            {
                                                                fields["INDIVIDUALPROVIDERNAME"] = ObjectControllerHelper.GetString("ProviderName", row);
                                                                fields["INDIVIDUALNPI"] = ObjectControllerHelper.GetString("NPI", row);
                                                                fields["INDIVIDUALMEDICAIDID"] = ObjectControllerHelper.GetString("BaseMedicaidID", row);
                                                                fields["AFFILIATIONDATE"] = Methods.GetShortDate(ObjectControllerHelper.GetString("AffiliationDate", row));
                                                            }
                                                            else
                                                            {
                                                                fields.Add("INDIVIDUALPROVIDERNAME", ObjectControllerHelper.GetString("ProviderName", row));
                                                                fields.Add("INDIVIDUALNPI", ObjectControllerHelper.GetString("NPI", row));
                                                                fields.Add("INDIVIDUALMEDICAIDID", ObjectControllerHelper.GetString("BaseMedicaidID", row));
                                                                fields.Add("AFFILIATIONDATE", Methods.GetShortDate(ObjectControllerHelper.GetString("AffiliationDate", row)));
                                                                //recipients = ObjectControllerHelper.GetString("Recipient", row);
                                                            }

                                                            Notification notify = null;
                                                            string body = String.Empty;
                                                            if (Methods.RequirePaperNotice(recipients, AppSettings.Get("PSEmailTypes", string.Empty).Split(',')))
                                                            {
                                                                notify = new PaperNotification(subject, new Guid());
                                                            }
                                                            else
                                                            {
                                                                notify = new EMailNotification(body, subject, recipients, new Guid());
                                                            }
                                                            body = notify.SendNotification(templateActualPath + @"/AffiliationConfirmationLetter.txt", fields);

                                                            notify.CreateCommunicationEvent(fields, new Guid(), "0", "AffiliationConfirmationLetter.txt");
                                                                                                                       
                                                        }
                                                    }
                                                }
                                            }
                                    }
                                    if (affilStatus == Constants.MMISStatusType.Errors.ToString())
                                    {
                                        // if MMIS errors are returned
                                        ms.ProcessDataErrors(mmisErrors, partyId.ToString(), medicaidPk);
                                    }
                                } // status code loop
                            }
                        }
                        else
                        {
                            // log entry
                            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotProcessed, groupPk, sakTrans));
                        }
                    }
                    catch (Exception ex)
                    {
                        CoreException.ThrowException(this.ThreadId, ex, logProcessName);
                        log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, groupPk, sakTrans));
                    }
                    if (!object.Equals(response.errors,null))
                    {
                        ms.ProcessServiceErrors(groupPk, response.errors);
                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
        }

        private void RetrieveGroupsByTransaction(int transactionType)
        {
            // create log object
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logProcessName);

            string location = string.Empty;
            string obj = string.Empty;
            DataSet ds;
            DateTime now = DateTime.Now;
            int countRow = 0;

            try
            {
                MMISShared ms = new MMISShared(this.ThreadId);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());

                // get the staging records
                ds = ms.GetSubmittedStagingRecords(transactionType);

                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart, ds.Tables[0].Rows.Count, transactionType));

                // loop over records in dataset
                foreach (DataRow groupRow in ds.Tables[0].Rows)
                {
                    string groupPk = string.Empty;
                    string medicaidPk = string.Empty;
                    string sakTrans = string.Empty;

                    // create the object to submit and the response
                    mt.ProviderGroupCaqhGetRequest request = new mt.ProviderGroupCaqhGetRequest();
                    mt.ProviderGroupCaqhGetResponse response = new mt.ProviderGroupCaqhGetResponse();
    
                    try
                    {
                        // populate the object credentials and provider object
                        int partyId = Convert.ToInt32(groupRow["PARTY_ID"]);
                        string medicaidId = Methods.GetStringValue(groupRow["MEDICAID_ID"]);
                        int tqId = Convert.ToInt32(groupRow["TRANSACTION_QUEUE_ID"].ToString());
                        medicaidPk = Methods.GetStringValue(groupRow["MEDICAID_PK"].ToString());
                        groupPk = Methods.GetStringValue(groupRow[MMISShared.groupStagingPk].ToString());
                        sakTrans = Methods.GetStringValue(groupRow["SAK_TRANS"]);
                        request.sak_trans = sakTrans;

                        // serialize the request to xml and save to filesystem
                        obj = Methods.SerializeObjectToXml(request);
                        Methods.WriteStringToFile(ms.path, obj, groupPk + "Request");

                        request.userId = ms.uid;
                        request.password = ms.pwd;

                        if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                        {
                            // create the web service object and retrieve from the MMIS
                            mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
                            response = mmis.GetProviderGroup(request);
                        }
                        else
                        {

                            // get test values
                            string groupStatus = AppSettings.Get("MMIS-TestGroupStatus", Constants.MMISStatusType.Processed.ToString());
                            string groupError = AppSettings.Get("MMIS-TestGroupErrors", string.Empty);

                            countRow += 1;
                            bool process = false;
                            if (countRow <= ms.testRowsCount) process = true;
                            
                            if (process)
                            {
                                int medicaidLength = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));
                                string medicaidPrefix = new string('1', medicaidLength - 5);
                                string testValue = DateTime.Now.ToString("ssfff");
                                response.correlationId = "555" + testValue;
                                List<mt.Error> errorList = new List<mt.Error>();
                                response.errors = errorList.ToArray<mt.Error>();
                                response.errorCode = groupError;
                                response.statusCode = groupStatus;
                                response.pdmsId = Methods.GetStringValue(groupRow[MMISShared.groupStagingPk].ToString());
                                if (string.IsNullOrWhiteSpace(medicaidId)) 
                                {
                                    response.medicareId = medicaidPrefix + testValue;
                                }
                                else
                                {
                                    response.medicareId = medicaidId;
                                }
                            }
                        }

                        // serialize the object to xml and save to filesystem
                        obj = Methods.SerializeObjectToXml(response);
                        location = Methods.WriteStringToFile(ms.path, obj, groupPk + "Response");

                        // get properties
                        medicaidId = string.Empty;
                        if (!string.IsNullOrWhiteSpace(response.medicareId))
                        {
                            medicaidId = response.medicareId.Trim();
                        }
                        string errorCodes = string.Empty;
                        if (!string.IsNullOrWhiteSpace(response.errorCode))
                        {
                            errorCodes = response.errorCode.Trim();
                        }
                        string correlationId = response.correlationId.Trim();
                        if (!string.IsNullOrWhiteSpace(response.correlationId))
                        {
                            correlationId = response.correlationId.Trim();
                        }
                        string statusCode = response.statusCode.Trim();
                        if (!string.IsNullOrWhiteSpace(response.statusCode))
                        {
                            statusCode = response.statusCode.Trim();
                        }
                        groupPk = string.Empty;
                        if (!string.IsNullOrWhiteSpace(response.pdmsId))
                        {
                            groupPk = response.pdmsId.Trim();
                        }
                        
                        // log entry
                        log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, groupPk, sakTrans));

                        if (!string.IsNullOrWhiteSpace(statusCode))
                        {
                            // if MMIS errors are returned
                            ms.ProcessDataErrors(errorCodes, partyId.ToString(), medicaidPk);

                            // Update TRANSACTION table with Process Date information
                            // -----------------------------------------------------
                            TransactionController.UpdateTransactionQueue(
                                tqId
                                , null
                                , now
                                , now
                                , Constants.appPDMSDataExchangeUserId);

                            // generated by sp_Admin_StoredProcBuilder on Sep 12 2013 11:07AM
                            // create parameters objects and fill with values
                            List<SqlParameter> parameters = new List<SqlParameter>();

                            parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_GROUP_PK", DbType.Int32, groupPk, true));
                            parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, sakTrans, true));
                            parameters.Add(SqlParms.CreateParameter("CORRELATION_ID", DbType.String, correlationId, true));
                            parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, errorCodes, true));
                            parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, true));
                            parameters.Add(SqlParms.CreateParameter("STATUS_CODE", DbType.Int32, statusCode, true));
                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, false));
                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                            DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingGroup", parameters);
                        }
                        else
                        {
                            // log entry
                            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotProcessed, groupPk, sakTrans));
                        }
                    }
                    catch (Exception ex)
                    {
                        CoreException.ThrowException(this.ThreadId, ex, logProcessName);
                        log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, groupPk, sakTrans));
                    }
                    ms.ProcessServiceErrors(groupPk, response.errors);
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
        }

        private int AffiliationLetterCount(int groupPartyId, int affilPartyId)
        {

            int emailCount = 0;

            // generated by sp_Admin_StoredProcBuilder on Jul 15 2014  8:52AM
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("GROUP_PARTY_ID", DbType.Int32, groupPartyId, true));
            parameters.Add(SqlParms.CreateParameter("AFFIL_PARTY_ID", DbType.Int32, affilPartyId, true));

            emailCount = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_ValidateAffiliationEmail", parameters));

            return emailCount;
        }

        #endregion
    }
}
