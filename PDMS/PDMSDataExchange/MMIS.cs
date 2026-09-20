using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using mt = MAXIMUS.DataExchange.PDMS.TN_MMIS_Service_Test;

namespace MAXIMUS.DataExchange.PDMS
{
    public class MMIS : BaseJob, IJob
    {
#region "Constructors"

        public MMIS(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

#endregion


#region "Class Level Declarations"

        const string dbtProvs = "PROVIDER_STAGING";
        const string dbtAddr = "ADDRESS_STAGING";
        const string dbrProv2Addrs = dbtProvs + "2" + dbtAddr;
        const string dbfPK = "PROVIDER_STAGING_PK";

        #endregion

        #region "Public Methods"


        override public void ExecuteJob()
        {
            // Default Job - MMIS - Retrieve Providers
            this.ExecuteJob(Guid.Parse("5EA0C60A-78BE-4BC3-9692-602104AB0E58"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                // MMIS - Retrieve Providers
                case "5EA0C60A-78BE-4BC3-9692-602104AB0E58":
                    this.RetrieveProviders();
                    break;

                // MMIS - Submit Providers
                case "BC64B545-5EEE-4F24-8814-D43EDD2EF876":
                    this.PopulateStagingData();
                    this.SubmitProviders();
                    break;
            }
        }

        public void LoadXmlFromMMISExchange()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string exportFolder = DateTime.Now.ToString("yyyyMMddHH") + "-" + MethodBase.GetCurrentMethod().Name;
            string basePath = AppSettings.Get("MMIS-SentProvidersXmlPath");
            List<string> listImportedFolderNames = GetImportedFolderNames().ToList();
            DirectoryInfo di = new DirectoryInfo(basePath);
            DirectoryInfo[] folders = di.GetDirectories("*", SearchOption.AllDirectories);

            foreach(DirectoryInfo folder in folders)
            {
                if(listImportedFolderNames.Contains(folder.Name))
                {
                    continue;
                }

                if (folder.Name.Contains("Submit"))
                {
                    ProcessSubmitFolder(folder);
                    InsertImportedFolder(folder.Name, DateTime.Now);
                }
                else if (folder.Name.Contains("Retrieve"))
                {
                    ProcessRetrieveFolder(folder);
                    InsertImportedFolder(folder.Name, DateTime.Now);
                }
                else
                {
                    continue;
                }
            }

            // "Request"
            // "Response"
            try
            {
                //// add records to staging tables
                //mt.ProviderCaqhGetRequest caqhGetRequest = new mt.ProviderCaqhGetRequest();
                //mt.ProviderCaqhGetResponse caqhGetResponse = new mt.ProviderCaqhGetResponse();

                //// create the object to submit and the response
                //mt.ProviderCaqhSubmitRequest caqhSubmitRequest = new mt.ProviderCaqhSubmitRequest();
                //mt.ProviderCaqhSubmitResponse caqhSubmitResponse = new mt.ProviderCaqhSubmitResponse();
            
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private void ProcessSubmitFolder(DirectoryInfo folder)
        {
            FileInfo[] files = folder.GetFiles("*.xml");

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains("Request"))
                {
                    StoreSubmitProvidersRequest(file);
                }
                else if (file.Name.Contains("Response"))
                {
                    StoreSubmitProvidersResponse(file);
                }
                else
                {
                    continue;
                }
            }
        }

        private void ProcessRetrieveFolder(DirectoryInfo folder)
        {
            FileInfo[] files = folder.GetFiles("*.xml");

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains("Request"))
                {
                    StoreRetrieveProvidersRequest(file);
                }
                else if (file.Name.Contains("Response"))
                {
                    StoreRetrieveProvidersResponse(file);
                }
                else
                {
                    continue;
                }
            }
        }

        private void StoreSubmitProvidersRequest(FileInfo file)
        {
            Stream reader = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ProviderCaqhSubmitRequest));
                reader = new FileStream(file.FullName, FileMode.Open);
                ProviderCaqhSubmitRequest submitRequest = (ProviderCaqhSubmitRequest)serializer.Deserialize(reader);
                Dictionary<string, int> addressTypes = SelectAddressType();
                Dictionary<string, int> phoneTypes = SelectPhoneType();
                Dictionary<string, int> transmissionTypes = SelectTransmissionType();
                
                foreach (ProviderCaqhSubmitRequestProvider provider in submitRequest.Items)
                {
                    #region SubmitProviders_Request
                    int id = InsertSubmitProviders_Request(
                        ToNullableInt32(provider.providerId),
                        ToNullableInt32(provider.caqhId),
                        provider.deaId,
                        ToNullableInt32(provider.ssnId),
                        ToNullableInt32(provider.npiId),
                        ToNullableInt32(provider.medicareId),
                        ToNullableInt32(provider.taxId),
                        provider.action,
                        provider.lastName,
                        provider.firstName,
                        provider.middleName,
                        provider.suffixName,
                        provider.country,
                        ToNullableDateTime(provider.birthDateTime),
                        ToNullableDateTime(provider.attestDateTime),
                        provider.providerTypeCode,
                        provider.providerSpecialtyCode,
                        provider.taxonomyCode,
                        provider.disclosureRec,
                        ToNullableDateTime(provider.disclosureDateTime),
                        provider.enrollmentStatusCode,
                        ToNullableDateTime(provider.termDateTime),
                        provider.statusCode);
                    #endregion

                    #region PrimaryAddress
                    foreach (ProviderCaqhSubmitRequestProviderPrimaryAddress address in provider.primaryAddress)
                    {
                        InsertAddress(
                            address.street1,
                            address.street2,
                            address.city,
                            address.state,
                            address.zip,
                            address.email,
                            string.Empty,
                            addressTypes["Primary"],
                            string.Empty,
                            id);
                    }
                    #endregion

                    #region PayToAddress
                    foreach (ProviderCaqhSubmitRequestProviderPayToAddress address in provider.payToAddress)
                    {
                        InsertAddress(
                            address.street1,
                            address.street2,
                            address.city,
                            address.state,
                            address.zip,
                            address.email,
                            provider.payToName,
                            addressTypes["PayTo"],
                            string.Empty,
                            id);
                    }
                    #endregion

                    #region MailingAddress
                    foreach (ProviderCaqhSubmitRequestProviderMailingAddress address in provider.mailingAddress)
                    {
                        InsertAddress(
                            address.street1,
                            address.street2,
                            address.city,
                            address.state,
                            address.zip,
                            address.email,
                            provider.mailToName,
                            addressTypes["Mailing"],
                            string.Empty,
                            id);
                    }
                    #endregion

                    #region ServicingAddress
                    foreach (ProviderCaqhSubmitRequestProviderServicingAddress address in provider.servicingAddress)
                    {
                        InsertAddress(
                            address.street1,
                            address.street2,
                            address.city,
                            address.state,
                            address.zip,
                            address.email,
                            provider.servicingName,
                            addressTypes["Servicing"],
                            string.Empty,
                            id);
                    }
                    #endregion

                    #region DEA
                    InsertDEA(
                        ToNullableDateTime(provider.deaEffectiveDateTime),
                        ToNullableDateTime(provider.deaEndDateTime),
                        id);
                    #endregion

                    #region License
                    InsertLicense(
                        ToNullableInt32(provider.license),
                        provider.stateLicenceCode,
                        provider.licenseTypeCode,
                        ToNullableDateTime(provider.licenseEffectiveDateTime),
                        ToNullableDateTime(provider.licenseEndDateTime),
                        id);
                    #endregion

                    #region Phone Voice
                    InsertPhone(
                        phoneTypes["Voice"],
                        provider.phoneVoice,
                        id);
                    #endregion

                    #region Phone Fax
                    InsertPhone(
                        phoneTypes["Fax"],
                        provider.phoneFax,
                        id);
                    #endregion

                    #region Groups
                    foreach (ProviderCaqhSubmitRequestProviderGroupsProviderGroup group in provider.groups)
                    {
                        int groupId = InsertProviderGroup(
                                        group.serviceName,
                                        group.medicareId,
                                        group.npiId,
                                        group.taxId,
                                        ToNullableDateTime(group.groupEndDateTime),
                                        string.Empty,
                                        null,
                                        null,
                                        id);


                        foreach (ProviderCaqhSubmitRequestProviderGroupsProviderGroupServiceAddress address in group.serviceAddress)
                        {
                            InsertProviderGroupAddress(
                                address.street1,
                                address.street2,
                                address.city,
                                address.state,
                                address.zip,
                                address.email,
                                addressTypes["ProviderGroup"],
                                string.Empty,
                                groupId);
                        }
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    
                }
            }
        }

        private void StoreSubmitProvidersResponse(FileInfo file)
        {
            Stream reader = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ProviderCaqhSubmitResponse));
                reader = new FileStream(file.FullName, FileMode.Open);
                ProviderCaqhSubmitResponse submitResponse = (ProviderCaqhSubmitResponse)serializer.Deserialize(reader);

                InsertSubmitProviders_Response(
                    submitResponse.ExtensionData,
                    submitResponse.correlationId,
                    submitResponse.errors,
                    submitResponse.transactionId);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private void StoreRetrieveProvidersRequest(FileInfo file)
        {
            Stream reader = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ProviderCaqhGetRequest));
                reader = new FileStream(file.FullName, FileMode.Open);
                ProviderCaqhGetRequest retrieveRequest = (ProviderCaqhGetRequest)serializer.Deserialize(reader);

                InsertRetrieveProviders_Request(retrieveRequest.sak_trans);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private void StoreRetrieveProvidersResponse(FileInfo file)
        {
            Stream reader = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ProviderCaqhGetResponse));
                reader = new FileStream(file.FullName, FileMode.Open);
                ProviderCaqhGetResponse retrieveResponse = (ProviderCaqhGetResponse)serializer.Deserialize(reader);
                Dictionary<string, int> addressTypes = SelectAddressType();

                int id = InsertRetrieveProviders_Response(
                    retrieveResponse.ExtensionData,
                    ToNullableInt32(retrieveResponse.correlationId),
                    retrieveResponse.errors,
                    ToNullableInt32(retrieveResponse.providerId),
                    retrieveResponse.countyCode,
                    ToNullableInt32(retrieveResponse.medicareId),
                    retrieveResponse.errorCode,
                    retrieveResponse.statusCode
                    );

                foreach (ProviderCaqhGetResponseGroupsProviderGroup group in retrieveResponse.groups)
                {
                    int groupId = InsertProviderGroup(
                                        group.serviceName,
                                        group.medicareId,
                                        group.npiId,
                                        group.taxId,
                                        null,
                                        group.ExtensionData,//string ExtensionData,
                                        null,
                                        null,
                                        id);

                    foreach (ProviderCaqhGetResponseGroupsProviderGroupServiceAddress address in group.serviceAddress)
                    {
                        InsertProviderGroupAddress(
                            address.street1,
                            address.street2,
                            address.city,
                            address.state,
                            address.zip,
                            null,
                            addressTypes["ProviderGroup"],
                            address.ExtensionData,
                            groupId);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void PopulateStagingData()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // add records to staging tables
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                DataAccess.ExecuteStoredProcedure("usp_AddPDMSToMMISExportRecords", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        public void RetrieveProviders()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            string basePath = AppSettings.Get("MMIS-SentProvidersXmlPath");
            string exportFolder = DateTime.Now.ToString("yyyyMMddHH") + "-" + MethodBase.GetCurrentMethod().Name;
            string location = string.Empty;
            DateTime now = DateTime.Now;
            string obj = string.Empty;
            DataSet providersDS;
            string providerPK = string.Empty;
            string path = string.Empty;
            int countRow = 0;
            int countTestRows = Convert.ToInt32(AppSettings.Get("MMIS-RetrieveRecordCount","10"));

            try
            {
                MMISShared ms = new MMISShared(this.ThreadId);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());
                path = basePath + exportFolder + @"\";

                // Select all providers which have a Submitted status
                providersDS = GetProviders(Constants.PDMSStatusType.SubmittedtoMMIS);

                // log entry
                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart, providersDS.Tables[0].Rows.Count, ""));

                // loop over provider records in dataset
                foreach (DataRow providerRow in providersDS.Tables[0].Rows)
                {
                    // create the object to submit and the response
                    mt.ProviderCaqhGetRequest request = new mt.ProviderCaqhGetRequest();
                    mt.ProviderCaqhGetResponse response = new mt.ProviderCaqhGetResponse();

                    try
                    {
                        // populate the object credentials and provider object
                        int pdmsStatus = Constants.PDMSStatusType.Processed;
                        string sakTrans = Methods.GetStringValue(providerRow["SAK_TRANS"]);
                        request.sak_trans = sakTrans;
                        int medicaidPk = Methods.GetIntValue(providerRow["MEDICAID_PK"].ToString());
                        int transType = Convert.ToInt32(providerRow["TRANSACTION_TYPE_ID"].ToString());
                        int transQueueId = Convert.ToInt32(providerRow["TRANSACTION_QUEUE_ID"].ToString());
                        string medicareId = Methods.GetStringValue(providerRow["MEDICARE_ID"].ToString());
                        int partyId = Convert.ToInt32(providerRow["PARTY_ID"]);
                        providerPK = Methods.GetStringValue(providerRow[dbfPK]);

                        // serialize the request to xml and save to filesystem
                        obj = Methods.SerializeObjectToXml(request);
                        Methods.WriteStringToFile(path, obj, providerPK + "Request");

                        request.userId = ms.uid;
                        request.password = ms.pwd;

                        if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                        {
                            // create the web service object and retrieve from the MMIS
                            mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
                            response = mmis.GetProvider(request);
                        }
                        else
                        {
                            countRow += 1;
                            bool process = false;
                            if (countRow <= countTestRows) process = true;
                            string testValue = DateTime.Now.ToString("ssfff");
                            List<mt.Error> errorList = new List<mt.Error>();
                            response.errors = errorList.ToArray<mt.Error>();
                            response.statusCode = string.Empty;
                            int medicaidLength = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));

                            if (process)
                            {
                                response.countyCode = "00";
                                response.errorCode = string.Empty;
                                response.providerId = "222" + testValue;
                                response.statusCode = "201";
                                response.correlationId = Methods.GetStringValue(providerRow["CORRELATION_ID"]);
                                if (string.IsNullOrWhiteSpace(medicareId))
                                {
                                    string medicaidPrefix = new string('1', medicaidLength - 5);
                                    response.medicareId = medicaidPrefix + testValue;
                                }
                                else
                                {
                                    response.medicareId = medicareId;
                                }
                            }
                        }

                        // serialize the object to xml and save to filesystem
                        obj = Methods.SerializeObjectToXml(response);
                        location = Methods.WriteStringToFile(path, obj, providerPK + "Response");

                        log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, providerPK, sakTrans));

                        // Bug 1694 -- Do not process records where the status code is empty
                        if (!string.IsNullOrWhiteSpace(response.statusCode))
                        {
                            // if MMIS errors are returned
                            string errDelimiter = AppSettings.Get("MMIS-ErrorDelimiter");
                            char delimiter = Convert.ToChar(errDelimiter);
                            string errCodes = response.errorCode;
                            if (!string.IsNullOrWhiteSpace(errCodes))
                            {
                                errCodes = errCodes.Trim();

                                // set status and populate error cross reference
                                pdmsStatus = Constants.PDMSStatusType.Errors;

                                string[] errors = errCodes.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string errorId in errors)
                                {
                                    // if the error is a number
                                    if (Methods.IsNumeric(errorId.Trim()))
                                    {
                                        RecordError(partyId, Convert.ToInt32(errorId.Trim()));
                                    }
                                    // else, log the issue
                                    else
                                    {
                                        log.CreateLogEntry(String.Format(Constants.LogString.ExceptionInvalidMMISError, errorId));
                                    }
                                }
                            }

                            // Given the proper response code (201), load the missing Medicaid Ids for the Party Id
                            List<MMISMedicaidId> medicaidIds = new List<MMISMedicaidId>();
                            ProviderNotification providerNotify = new ProviderNotification(this.ThreadId);
                            if (AppSettings.Get("MMIS-ProviderEnrollmentNotification", bool.FalseString).ToLower() ==
                                bool.TrueString.ToLower())
                            {
                                if (response.statusCode == Constants.MMISStatusType.Processed.ToString())
                                    providerNotify.LoadMissingMedicaidIds(partyId, response, ref medicaidIds);
                            }

                            // get properties
                            string medicaidId = string.Empty;
                            if (!string.IsNullOrWhiteSpace(response.medicareId))
                            {
                                medicaidId = response.medicareId.Trim();
                            }
                            string statusCode = string.Empty;
                            if (!string.IsNullOrWhiteSpace(response.statusCode))
                            {
                                statusCode = response.statusCode.Trim();
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

                            // Update TRANSACTION table with Submit Date information
                            // -----------------------------------------------------
                            TransactionController.UpdateTransactionQueue(
                                transQueueId
                                , null
                                , now
                                , now
                                , Constants.appPDMSDataExchangeUserId);

                            // if transaction type is 7 (Base Medi)
                            if (transType == Constants.TransactionType.RequestBaseMedicaidIDfromMMIS)
                            {

                                InsertAffilTxns(Convert.ToInt32(providerPK)
                                    , statusCode
                                    , medicaidId);
                            }

                            UpdateProvider(Convert.ToInt32(providerPK), pdmsStatus, response.countyCode, response.medicareId
                                , response.providerId, response.statusCode, response.correlationId);

                            // If there are missing Medicaid Ids then send out a welcome letter
                            if (medicaidIds.Count > 0)
                            {
                                if (!providerNotify.SendWelcomeLetterEmail(partyId, medicaidIds))
                                {
                                    log.CreateLogEntry("Error encountered during MMIS provider processing [pk-" +
                                        providerPK + "]: " + "Send Welcome Letter failed", Logging.LogPriority.Error);
                                }
                            }
                        }
                        else
                        {
                            // log entry
                            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotProcessed, providerPK, sakTrans));
                        }
                    }
                    catch (Exception ex)
                    {
                        log.CreateLogEntry("Error encountered during MMIS provider processing [pk-" + providerPK + "]: " + ex.Message, Logging.LogPriority.Error);
                    }
                    ResponseErrors(providerPK, response.errors);
                }
                // Per Weston, the below error description will be the only error that is returned except for credentialing issues with username and password
                //transId - null
                //corrId - "2039"
                //code - ""
                //desc - "There was an internal error.  Please call the help desk."

                // log entry
                log.CreateLogEntry(Constants.LogString.RetrievingMMISRecordsEnd);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        public void SubmitProviders()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            DataSet providersDS;
            string providerPK = string.Empty;
            string exportFolder = DateTime.Now.ToString("yyyyMMddHH") + "-" + MethodBase.GetCurrentMethod().Name;
            string location = string.Empty;
            string obj = string.Empty;
            string basePath = AppSettings.Get("MMIS-SentProvidersXmlPath");
            string path = string.Empty;
            DateTime now = DateTime.Now;

            try
            {
                MMISShared ms = new MMISShared(this.ThreadId);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());

                // retrieve added records
                providersDS = GetProviders(Constants.PDMSStatusType.PendingMMISSubmission);

                // log entry
                log.CreateLogEntry(String.Format(Constants.LogString.SubmittingMMISRecordsStart, providersDS.Tables[dbtProvs].Rows.Count, -1));

                path = basePath + exportFolder + @"\";

                // loop over provider records in dataset
                foreach (DataRow providerRow in providersDS.Tables[dbtProvs].Rows)
                {
                    try
                    {
                        // create the object to submit and the response
                        mt.ProviderCaqhSubmitRequest request = new mt.ProviderCaqhSubmitRequest();
                        mt.ProviderCaqhSubmitResponse response = new mt.ProviderCaqhSubmitResponse();
                        
                        providerPK = providerRow[dbfPK].ToString();
                        mt.Provider provider = new mt.Provider();
                        int partyId = Convert.ToInt32(providerRow["PARTY_ID"].ToString());
                        int transType = Convert.ToInt32(providerRow["TRANSACTION_TYPE_ID"].ToString());
                        int transQueueId = Convert.ToInt32(providerRow["TRANSACTION_QUEUE_ID"].ToString());
                        provider.practiceType = providerRow["MMIS_PRACTICE_INDICATOR"].ToString();
                        provider.action = providerRow["ACTION"].ToString();
                        provider.lastName = providerRow["LAST_NAME"].ToString();
                        provider.firstName = providerRow["FIRST_NAME"].ToString();
                        provider.middleName = providerRow["MIDDLE_NAME"].ToString();
                        provider.suffixName = providerRow["SUFFIX_NAME"].ToString();
                        provider.phoneVoice = providerRow["PHONE_VOICE"].ToString();
                        provider.phoneFax = providerRow["PHONE_FAX"].ToString();
                        provider.country = providerRow["COUNTRY"].ToString();
                        provider.license = providerRow["LICENSE"].ToString();
                        provider.stateLicenceCode = providerRow["STATE_LICENSE_CODE"].ToString();
                        provider.licenseTypeCode = providerRow["LICENSE_TYPE_CODE"].ToString();
                        provider.deaId = providerRow["DEA_NUMBER"].ToString();
                        provider.providerTypeCode = providerRow["PROVIDER_TYPE_CODE"].ToString();
                        provider.providerSpecialtyCode = providerRow["PROVIDER_SPECIALTY_CODE"].ToString();
                        provider.taxonomyCode = providerRow["TAXONOMY_CODE"].ToString();
                        provider.disclosureRec = providerRow["DISCLOSURE_REC"].ToString();
                        provider.enrollmentStatusCode = providerRow["ENROLLMENT_STATUS_CODE"].ToString();
                        provider.providerId = providerRow["PROVIDER_ID"].ToString();
                        provider.caqhId = providerRow["CAQH_ID"].ToString();
                        provider.ssnId = providerRow["SSN_ID"].ToString();
                        provider.npiId = providerRow["NPI_ID"].ToString();
                        provider.medicareId = providerRow["MEDICARE_ID"].ToString().Trim();
                        provider.taxId = providerRow["TAX_ID"].ToString();
                        provider.statusCode = providerRow["STATUS_CODE"].ToString();
						provider.programEligibilityCode = providerRow["MMIS_ELIGIBILITY_CODE"].ToString();
						provider.eight35indicator = providerRow["EIGHT_THIRTY_FIVE_INDICATOR"].ToString();

						
                        // check all dates for null and set if not null
                        if (!String.IsNullOrWhiteSpace(providerRow["BIRTH_DATE_TIME"].ToString()))
                        {
                            provider.birthDateTime = Methods.GetDateValue(providerRow["BIRTH_DATE_TIME"]);
                        }
                        if (!String.IsNullOrWhiteSpace(providerRow["ATTEST_DATE_TIME"].ToString()))
                        {
                            provider.attestDateTime = Methods.GetDateValue(providerRow["ATTEST_DATE_TIME"]);
                        }
                        if (!String.IsNullOrWhiteSpace(providerRow["LICENSE_EFFECTIVE_DATE_TIME"].ToString()))
                        {
                            provider.licenseEffectiveDateTime = Methods.GetDateValue(providerRow["LICENSE_EFFECTIVE_DATE_TIME"]);
                        }
                        if (!String.IsNullOrWhiteSpace(providerRow["LICENSE_END_DATE_TIME"].ToString()))
                        {
                            provider.licenseEndDateTime = Methods.GetDateValue(providerRow["LICENSE_END_DATE_TIME"]);
                        }
                        if (!String.IsNullOrWhiteSpace(providerRow["DEA_EFFECTIVE_DATE_TIME"].ToString()))
                        {
                            provider.deaEffectiveDateTime = Methods.GetDateValue(providerRow["DEA_EFFECTIVE_DATE_TIME"]);
                        }
                        if (!String.IsNullOrWhiteSpace(providerRow["DEA_END_DATE_TIME"].ToString()))
                        {
                            provider.deaEndDateTime = Methods.GetDateValue(providerRow["DEA_END_DATE_TIME"]);
                        }
                        if (!String.IsNullOrWhiteSpace(providerRow["DISCLOSURE_DATE_TIME"].ToString()))
                        {
                            provider.disclosureDateTime = Methods.GetDateValue(providerRow["DISCLOSURE_DATE_TIME"]);
                        }
                        if (!String.IsNullOrWhiteSpace(providerRow["TERM_DATE_TIME"].ToString()))
                        {
                            provider.termDateTime = Methods.GetDateValue(providerRow["TERM_DATE_TIME"]);
                        }

                        foreach (DataRow address in providerRow.GetChildRows(dbrProv2Addrs))
                        {

                            mt.Address addr = new mt.Address();
                            int addressType = Convert.ToInt32(address["ADDRESS_STAGING_TYPE_ID"]);

                            addr.street1 = Methods.GetStringValue(address["ADDRESS_STREET1"], false).ToString();
                            addr.street2 = Methods.GetStringValue(address["ADDRESS_STREET2"], false).ToString();
                            addr.city = Methods.GetStringValue(address["ADDRESS_CITY"], false).ToString();
                            addr.state = Methods.GetStringValue(address["ADDRESS_STATE"], false).ToString();
                            addr.zip = Methods.GetStringValue(address["ADDRESS_ZIP"], false).ToString();
                            addr.email = Methods.GetStringValue(address["ADDRESS_EMAIL"], false).ToString();
                            string caqhPracticeId = Methods.GetStringValue(address["CAQH_PRACTICE_ID"], false).ToString().Trim();

                            switch (addressType)
                            {
                                case (int)Enumerations.ContactMechanismRoleTypeId.MailTo:
                                    provider.mailingAddress = addr;
                                    provider.mailToName = Methods.GetStringValue(address["NAME"], false).ToString();
                                    break;

                                case (int)Enumerations.ContactMechanismRoleTypeId.Credentialing:
                                    provider.primaryAddress = addr;
                                    break;

                                case (int)Enumerations.ContactMechanismRoleTypeId.PayTo:
                                    provider.payToAddress = addr;
                                    provider.payToName = Methods.GetStringValue(address["NAME"], false).ToString();
                                    break;

                                case (int)Enumerations.ContactMechanismRoleTypeId.Servicing:
                                    provider.servicingAddress = addr;
                                    provider.servicingName = Methods.GetStringValue(address["NAME"], false).ToString();
                                    break;
                            }
                        }

                        // populate the provider object
                        request.provider = provider;

                        // serialize the object to xml and save to filesystem
                        obj = Methods.SerializeObjectToXml(request);
                        Methods.WriteStringToFile(path, obj, providerPK + "Request");

                        // populate the object credentials 
                        request.userId = ms.uid;
                        request.password = ms.pwd;

                        if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                        {
                            // submit to the MMIS
                            mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient(); 
                            response = mmis.SubmitProvider(request);
                        }
                        else
                        {
                            string testValue = DateTime.Now.ToString("ssfff");
                            response.transactionId = "888" + testValue;
                            response.correlationId = "999" + testValue;
                            List<mt.Error> errorList = new List<mt.Error>();
                            response.errors = errorList.ToArray<mt.Error>();
                        }

                        // serialize the object to xml and save to filesystem
                        obj = Methods.SerializeObjectToXml(response);
                        location = Methods.WriteStringToFile(path, obj, providerPK + "Response");

                        // provider submitted log
                        log.CreateLogEntry(String.Format(Constants.LogString.SubmittingMMISRecord, providerPK, response.correlationId, location));

                        // extract response errors
                        ResponseErrors(providerPK, response.errors);

                        // Update TRANSACTION table with Submit Date information
                        // -----------------------------------------------------
                        TransactionController.UpdateTransactionQueue(
                            transQueueId
                            , now
                            , null
                            , now
                            , Constants.appPDMSDataExchangeUserId);

                        // generated by sp_Admin_StoredProcBuilder on Aug 20 2012  1:44PM
                        // create parameters objects and fill with values
                        List<SqlParameter> parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("PROVIDER_STAGING_PK", DbType.Int32, providerPK, true));
                        parameters.Add(SqlParms.CreateParameter("PDMS_STATUS_TYPE_ID", DbType.Int32, Constants.PDMSStatusType.SubmittedtoMMIS, true));
                        parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_TYPE_ID", DbType.Int32, Enumerations.ProviderStatusChangeTypeId.MMIS, true));
                        parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, response.transactionId, true));
                        parameters.Add(SqlParms.CreateParameter("CORRELATION_ID", DbType.String, response.correlationId, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                        DataAccess.ExecuteStoredProcedure("usp_UpdatePDMSToMMISExportRecords", parameters);
                    }
                    catch (Exception ex)
                    {
                        LogErrorRecord(log, "MMIS Submit Exception", dbtProvs, providerPK, ex.Message);
                    }
                }

                // log entry
                log.CreateLogEntry(Constants.LogString.SubmittingMMISRecordsEnd);

                // Per Weston, the below error description will be the only error that is returned except for credentialing issues with username and password
                //transId - null
                //corrId - "2039"
                //code - ""
                //desc - "There was an internal error.  Please call the help desk."
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        public void UpdateProvider(int providerStagingPk, int pdmsStatusTypeId, string countyCode, string medicaidId
            , string providerId, string statusCode, string coorelationId = "")
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Aug 14 2012  9:56AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PROVIDER_STAGING_PK", DbType.Int32, providerStagingPk, false));
                parameters.Add(SqlParms.CreateParameter("PDMS_STATUS_TYPE_ID", DbType.Int32, pdmsStatusTypeId, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_TYPE_ID", DbType.Int32, Enumerations.ProviderStatusChangeTypeId.MMIS, false));
                // parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, DataRowName, true));
                parameters.Add(SqlParms.CreateParameter("CORRELATION_ID", DbType.String, coorelationId.Trim(), true));
                parameters.Add(SqlParms.CreateParameter("COUNTY_CODE", DbType.String, countyCode, true));
                parameters.Add(SqlParms.CreateParameter("MEDICARE_ID", DbType.String, medicaidId.Trim(), true));
                parameters.Add(SqlParms.CreateParameter("STATUS_CODE", DbType.String, statusCode.Trim(), true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_ID", DbType.String, providerId.Trim(), true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_UpdatePDMSToMMISExportedRecords", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private void InsertAffilTxns(int providerStagingPk, string statusCode, string baseMedicaidId)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Nov 26 2013 11:59AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PROVIDER_STAGING_PK", DbType.Int32, providerStagingPk, true));
                parameters.Add(SqlParms.CreateParameter("STATUS_CODE", DbType.String, statusCode, true));
                parameters.Add(SqlParms.CreateParameter("BASE_MEDICAID_ID", DbType.String, baseMedicaidId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_InsertAffiliationTransactions", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        #endregion

        #region "Private Methods"

        private DataSet GetProviders(int pdmsStatusType)
        {

            DataSet ds = new DataSet();

            if (pdmsStatusType == Constants.PDMSStatusType.PendingMMISSubmission)
            {
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectPDMSToMMISExportRecords", (dbtProvs + Constants.pluralEnding));

                ds.Tables[0].TableName = dbtProvs;
                ds.Tables[1].TableName = dbtAddr;

                DataRelation relation;
                relation = new DataRelation(dbrProv2Addrs, ds.Tables[dbtProvs].Columns[dbfPK]
                    , ds.Tables[dbtAddr].Columns[dbfPK]);
                ds.Relations.Add(relation);
            }
            if (pdmsStatusType == Constants.PDMSStatusType.SubmittedtoMMIS)
            {
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectPDMSToMMISExportedRecords", (dbtProvs + Constants.pluralEnding));

                ds.Tables[0].TableName = dbtProvs;
            }
            return ds;
        }

        private void LogErrorRecord(Logging log, string actionType, string recordType, string recordValue, string exceptionMessage, string additionRecordInformation = "")
        {
            if (additionRecordInformation.Length > 0)
            {
                additionRecordInformation = ", " + additionRecordInformation;
            }
            string message = String.Format(Constants.LogString.ExtractErrorRecordFailure, actionType, (recordType + ": " + recordValue), additionRecordInformation);
            message += Environment.NewLine + exceptionMessage;
            log.CreateLogEntry(message, Logging.LogPriority.DataLoadIssues);
        }

        private void RecordError(int partyId, int mmisErrorId, string medicaidPK = "")
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            DateTime now = DateTime.Now;
            List<SqlParameter> parameters;

            try
            {
                // format the error Id to match the current error numbering convention
                int paddedLength = 3;
                string errorCode = "M" + mmisErrorId.ToString("D" + paddedLength.ToString());

                // generated by sp_Admin_StoredProcBuilder on Aug 14 2012 11:32AM
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, errorCode, true));
                string errorTypeId = DataAccess.ExecuteScalar("sp_SelectErrorTypeIdByErrorCode", parameters);

                // generated by sp_Admin_StoredProcBuilder on Dec 26 2012  9:20AM
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_TYPE_ID", DbType.Int32, errorTypeId, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_SavePDMSError", parameters);            
            }
            catch
            {
                // create log object
                log.CreateLogEntry(String.Format("Could not record PDMS error based on number provided by MMIS [{0}]", mmisErrorId), Logging.LogPriority.Error);
            }
        }

        private bool ResponseErrors(string providerPK, mt.Error[] errors)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            bool returnVal = false;

            // loop over errors and record information
            foreach (mt.Error err in errors)
            {
                string code;
                string desc;
                code = err.errorCode.Trim();
                desc = err.errorDescription.Trim();

                // generated by sp_Admin_StoredProcBuilder on Jul 16 2012 12:30PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PROVIDER_STAGING_PK", DbType.Int32, providerPK, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, code, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_DESCRIPTION", DbType.String, desc, true));

                DataAccess.ExecuteStoredProcedure("insertSTAGING_ERROR", parameters);

                LogErrorRecord(log, "MMIS Response Exception", dbtProvs, providerPK, code + " " + desc); 
            }

            // if errors encountered, return true
            if (errors.Length > 0)
            {
                returnVal = true;
            }

            return returnVal;
        }

        private int? ToNullableInt32(string toParse)
        {
            int toReturn;

            if (Int32.TryParse(toParse, out toReturn))
            {
                return toReturn;
            }
            else
            {
                return null;
            }
        }

        private DateTime? ToNullableDateTime(string toParse)
        {
            DateTime toReturn;
            DateTime sqlMinDateTime = new DateTime(1753, 1, 1);
            DateTime sqlMaxDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997);

            if (DateTime.TryParse(toParse, out toReturn))
            {
                if (sqlMinDateTime <= toReturn && toReturn <= sqlMaxDateTime)
                {
                    return toReturn;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region MMIS XML
            private void InsertAddress(
                                string Street1,
                                string Street2,
                                string City,
                                string State,
                                string Zip,
                                string Email,
                                string Name,
                                int AddressType_ID,
                                string ExtensionData,
                                int XREF_ID)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013  3:16PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("Street1", DbType.String, Street1, true));
                    parameters.Add(SqlParms.CreateParameter("Street2", DbType.String, Street2, true));
                    parameters.Add(SqlParms.CreateParameter("City", DbType.String, City, true));
                    parameters.Add(SqlParms.CreateParameter("State", DbType.String, State, true));
                    parameters.Add(SqlParms.CreateParameter("Zip", DbType.String, Zip, true));
                    parameters.Add(SqlParms.CreateParameter("Email", DbType.String, Email, true));
                    parameters.Add(SqlParms.CreateParameter("Name", DbType.String, Name, true));
                    parameters.Add(SqlParms.CreateParameter("AddressType_ID", DbType.Int32, AddressType_ID, true));
                    parameters.Add(SqlParms.CreateParameter("ExtensionData", DbType.String, ExtensionData, true));
                    parameters.Add(SqlParms.CreateParameter("XREF_ID", DbType.Int32, XREF_ID, true));

                    DataAccess.ExecuteStoredProcedure("usp_MMISXml_InsertAddress", parameters);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private void InsertProviderGroupAddress(
                                        string Street1,
                                        string Street2,
                                        string City,
                                        string State,
                                        string Zip,
                                        string Email,
                                        int AddressType_ID,
                                        string ExtensionData,
                                        int ProviderGroupId)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013  3:22PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("Street1", DbType.String, Street1, true));
                    parameters.Add(SqlParms.CreateParameter("Street2", DbType.String, Street2, true));
                    parameters.Add(SqlParms.CreateParameter("City", DbType.String, City, true));
                    parameters.Add(SqlParms.CreateParameter("State", DbType.String, State, true));
                    parameters.Add(SqlParms.CreateParameter("Zip", DbType.String, Zip, true));
                    parameters.Add(SqlParms.CreateParameter("Email", DbType.String, Email, true));
                    parameters.Add(SqlParms.CreateParameter("AddressType_ID", DbType.Int32, AddressType_ID, true));
                    parameters.Add(SqlParms.CreateParameter("ExtensionData", DbType.String, ExtensionData, true));
                    parameters.Add(SqlParms.CreateParameter("ProviderGroupId", DbType.Int32, ProviderGroupId, true));

                    DataAccess.ExecuteStoredProcedure("usp_MMISXml_InsertProviderGroupAddress", parameters);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private void InsertDEA(
                                    DateTime? EffectiveDateTime,
                                    DateTime? EndDateTime,
                                    int XREF_ID)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013 10:19AM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("EffectiveDateTime", DbType.DateTime, EffectiveDateTime, true));
                    parameters.Add(SqlParms.CreateParameter("EndDateTime", DbType.DateTime, EndDateTime, true));
                    parameters.Add(SqlParms.CreateParameter("XREF_ID", DbType.Int32, XREF_ID, true));

                    DataAccess.ExecuteStoredProcedure("usp_MMISXml_InsertDEA", parameters);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private void InsertLicense(
                                        int? License,
                                        string StateCode,
                                        string TypeCode,
                                        DateTime? EffectiveDateTime,
                                        DateTime? EndDateTime,
                                        int XREF_ID)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013  3:26PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("License", DbType.Int32, License, true));
                    parameters.Add(SqlParms.CreateParameter("StateCode", DbType.String, StateCode, true));
                    parameters.Add(SqlParms.CreateParameter("TypeCode", DbType.String, TypeCode, true));
                    parameters.Add(SqlParms.CreateParameter("EffectiveDateTime", DbType.DateTime, EffectiveDateTime, true));
                    parameters.Add(SqlParms.CreateParameter("EndDateTime", DbType.DateTime, EndDateTime, true));
                    parameters.Add(SqlParms.CreateParameter("XREF_ID", DbType.Int32, XREF_ID, true));

                    DataAccess.ExecuteStoredProcedure("usp_MMISXml_InsertLicense", parameters);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private void InsertPhone(
                                        int PhoneType_ID,
                                        string Number,
                                        int XREF_ID)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013  3:27PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("PhoneType_ID", DbType.Int32, PhoneType_ID, true));
                    parameters.Add(SqlParms.CreateParameter("Number", DbType.String, Number, true));
                    parameters.Add(SqlParms.CreateParameter("XREF_ID", DbType.Int32, XREF_ID, true));

                    DataAccess.ExecuteStoredProcedure("usp_MMISXml_InsertPhone", parameters);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private void InsertImportedFolder(
                                        string FolderName,
                                        DateTime ImportDate)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013  3:29PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("FolderName", DbType.String, FolderName, true));
                    parameters.Add(SqlParms.CreateParameter("ImportDate", DbType.DateTime, ImportDate, true));

                    DataAccess.ExecuteStoredProcedure("usp_MMISXml_InsertImportedFolder", parameters);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private List<string> GetImportedFolderNames()
            {
                try
                {
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_MMISXml_GetFoldersImported");
                    DataTable dt = ds.Tables[0];
                    List<string> folderNameList = dt.AsEnumerable().Select(dr => dr.Field<string>("FolderName")).ToList();

                    return folderNameList;
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private int InsertProviderGroup(
                                            string ServiceName,
                                            string MedicareId,
                                            string NpiId,
                                            string TaxId,
                                            DateTime? GroupEndDateTime,
                                            string ExtensionData,
                                            int? Response_ID,
                                            int? Request_ID,
                                            int XREF_ID)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013  3:37PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("ServiceName", DbType.String, ServiceName, true));
                    parameters.Add(SqlParms.CreateParameter("MedicareId", DbType.String, MedicareId, true));
                    parameters.Add(SqlParms.CreateParameter("NpiId", DbType.String, NpiId, true));
                    parameters.Add(SqlParms.CreateParameter("TaxId", DbType.String, TaxId, true));
                    parameters.Add(SqlParms.CreateParameter("GroupEndDateTime", DbType.DateTime, GroupEndDateTime, true));
                    parameters.Add(SqlParms.CreateParameter("ExtensionData", DbType.String, ExtensionData, true));
                    parameters.Add(SqlParms.CreateParameter("Response_ID", DbType.Int32, Response_ID, true));
                    parameters.Add(SqlParms.CreateParameter("Request_ID", DbType.Int32, Request_ID, true));
                    parameters.Add(SqlParms.CreateParameter("XREF_ID", DbType.Int32, XREF_ID, true));

                    string strID = DataAccess.ExecuteScalar("usp_MMISXml_InsertProviderGroup", parameters);

                    return Convert.ToInt32(strID);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private int InsertSubmitProviders_Request(
                                            int? ProviderId,
                                            int? CaqhId,
                                            string DeaId,
                                            int? SsnId,
                                            int? NpiId,
                                            int? MedicareId,
                                            int? TaxId,
                                            string Action,
                                            string LastName,
                                            string FirstName,
                                            string MiddleName,
                                            string SuffixName,
                                            string Country,
                                            DateTime? BirthDateTime,
                                            DateTime? AttestDateTime,
                                            string ProviderTypeCode,
                                            string ProviderSpecialtyCode,
                                            string TaxonomyCode,
                                            string DisclosureRec,
                                            DateTime? DisclosureDateTime,
                                            string EnrollmentStatusCode,
                                            DateTime? TermDateTime,
                                            string StatusCode)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013  3:57PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("ProviderId", DbType.Int32, ProviderId, true));
                    parameters.Add(SqlParms.CreateParameter("CaqhId", DbType.Int32, CaqhId, true));
                    parameters.Add(SqlParms.CreateParameter("DeaId", DbType.String, DeaId, true));
                    parameters.Add(SqlParms.CreateParameter("SsnId", DbType.Int32, SsnId, true));
                    parameters.Add(SqlParms.CreateParameter("NpiId", DbType.Int32, NpiId, true));
                    parameters.Add(SqlParms.CreateParameter("MedicareId", DbType.Int32, MedicareId, true));
                    parameters.Add(SqlParms.CreateParameter("TaxId", DbType.Int32, TaxId, true));
                    parameters.Add(SqlParms.CreateParameter("Action", DbType.String, Action, true));
                    parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, LastName, true));
                    parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, FirstName, true));
                    parameters.Add(SqlParms.CreateParameter("MiddleName", DbType.String, MiddleName, true));
                    parameters.Add(SqlParms.CreateParameter("SuffixName", DbType.String, SuffixName, true));
                    parameters.Add(SqlParms.CreateParameter("Country", DbType.String, Country, true));
                    parameters.Add(SqlParms.CreateParameter("BirthDateTime", DbType.DateTime, BirthDateTime, true));
                    parameters.Add(SqlParms.CreateParameter("AttestDateTime", DbType.DateTime, AttestDateTime, true));
                    parameters.Add(SqlParms.CreateParameter("ProviderTypeCode", DbType.String, ProviderTypeCode, true));
                    parameters.Add(SqlParms.CreateParameter("ProviderSpecialtyCode", DbType.String, ProviderSpecialtyCode, true));
                    parameters.Add(SqlParms.CreateParameter("TaxonomyCode", DbType.String, TaxonomyCode, true));
                    parameters.Add(SqlParms.CreateParameter("DisclosureRec", DbType.String, DisclosureRec, true));
                    parameters.Add(SqlParms.CreateParameter("DisclosureDateTime", DbType.DateTime, DisclosureDateTime, true));
                    parameters.Add(SqlParms.CreateParameter("EnrollmentStatusCode", DbType.String, EnrollmentStatusCode, true));
                    parameters.Add(SqlParms.CreateParameter("TermDateTime", DbType.DateTime, TermDateTime, true));
                    parameters.Add(SqlParms.CreateParameter("StatusCode", DbType.String, StatusCode, true));


                    string strId = DataAccess.ExecuteScalar("usp_MMISXml_InsertSubmitProviders_Request", parameters);

                    return Convert.ToInt32(strId);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private void InsertSubmitProviders_Response(
                                            string ExtensionData,
                                            string CorrelationId,
                                            string Errors,
                                            string TransactionId)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013  3:58PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("ExtensionData", DbType.String, ExtensionData, true));
                    parameters.Add(SqlParms.CreateParameter("CorrelationId", DbType.String, CorrelationId, true));
                    parameters.Add(SqlParms.CreateParameter("Errors", DbType.String, Errors, true));
                    parameters.Add(SqlParms.CreateParameter("TransactionId", DbType.String, TransactionId, true));

                    DataAccess.ExecuteStoredProcedure("usp_MMISXml_InsertSubmitProviders_Response", parameters);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private void InsertRetrieveProviders_Request(string Sak_Trans)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013  4:02PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("Sak_Trans", DbType.Int32, Sak_Trans, true));

                    DataAccess.ExecuteStoredProcedure("usp_MMISXml_InsertRetrieveProviders_Request", parameters);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private int InsertRetrieveProviders_Response(
                                            string ExtensionData,
                                            int? CorrelationId,
                                            string Errors,
                                            int? ProviderId,
                                            string CountryCode,
                                            int? MedicareId,
                                            string ErrorCode,
                                            string StatusCode)
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Mar 25 2013  4:02PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("ExtensionData", DbType.String, ExtensionData, true));
                    parameters.Add(SqlParms.CreateParameter("CorrelationId", DbType.Int32, CorrelationId, true));
                    parameters.Add(SqlParms.CreateParameter("Errors", DbType.String, Errors, true));
                    parameters.Add(SqlParms.CreateParameter("ProviderId", DbType.Int32, ProviderId, true));
                    parameters.Add(SqlParms.CreateParameter("CountryCode", DbType.String, CountryCode, true));
                    parameters.Add(SqlParms.CreateParameter("MedicareId", DbType.Int32, MedicareId, true));
                    parameters.Add(SqlParms.CreateParameter("ErrorCode", DbType.String, ErrorCode, true));
                    parameters.Add(SqlParms.CreateParameter("StatusCode", DbType.String, StatusCode, true));

                    string strId = DataAccess.ExecuteScalar("usp_MMISXml_InsertRetrieveProviders_Response", parameters);

                    return Convert.ToInt32(strId);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }

            private Dictionary<string, int> SelectAddressType()
            {
                Dictionary<string, int> dict = new Dictionary<string, int>();

                try
                {
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_MMISXml_SelectAddressType");
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow row in dt.Rows)
                    {
                        dict.Add(row["TypeName"].ToString().Trim(), Convert.ToInt32(row["AddressType_ID"]));
                    }
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }

                return dict;
            }

            private Dictionary<string, int> SelectPhoneType()
            {
                Dictionary<string, int> dict = new Dictionary<string, int>();

                try
                {
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_MMISXml_SelectPhoneType");
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow row in dt.Rows)
                    {
                        dict.Add(row["TypeName"].ToString().Trim(), Convert.ToInt32(row["PhoneType_ID"]));
                    }
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }

                return dict;
            }

            private Dictionary<string, int> SelectTransmissionType()
            {
                Dictionary<string, int> dict = new Dictionary<string, int>();

                try
                {
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_MMISXml_SelectTransmissionType");
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow row in dt.Rows)
                    {
                        dict.Add(row["TypeName"].ToString().Trim(), Convert.ToInt32(row["TransmissionType_ID"]));
                    }
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }

                return dict;
            }

        #endregion
    }
}
