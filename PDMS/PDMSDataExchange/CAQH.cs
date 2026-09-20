using FileHelpers;
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

namespace MAXIMUS.DataExchange.PDMS
{
    public class CAQH : BaseJob, IJob
    {
#region "Constructors"

        public CAQH(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

#endregion
#region "Logging Objects"

        private int logCnt = 0;

 #endregion

        #region "Class Level Declarations"

            private enum RetrieveColumn
            {
                New = 0,
                Old = 1,
                ChangeType = 2,
                BasedOnChangeType = 3
            }

            string xmlProviderId = string.Empty;    //   the unique provider id from CAQH
            const string dbrSep = "2";

            // db table constants
            const string dboProviders = "Providers";
            const string dboProviderRecords = "ProviderRecords";
            const string valPrev = "PREVIOUS_VALUE";
            const string valCurr = "CURRENT_VALUE";
            const string valChangeType = "CHANGE_TYPE";
            const string valKey = "CAQH_UPDATE_PK";
            const string valMap = "MAPPED_ATTRIBUTE";

            // db relationship constants
            const string dbrProv2Records = dboProviders + "2" + dboProviderRecords;

            // db field constants
            const string dbfProviderId = "PROVIDER_ID";

            // CAQHS_ tables db constants
            // tables
            const string dbtProvider = "CAQHS_Provider";
            const string dbtProviderAssociate = "CAQHS_ProviderAssociate";
            const string dbtProviderDEA = "CAQHS_ProviderDEA";
            const string dbtProviderDisclosure = "CAQHS_ProviderDisclosure";
            const string dbtProviderHospital = "CAQHS_ProviderHospital";
            const string dbtProviderLicense = "CAQHS_ProviderLicense";
            const string dbtProviderSpecialty = "CAQHS_ProviderSpecialty";
            const string dbtProviderPractice = "CAQHS_ProviderPractice";
            const string dbtProviderPracticeAssociate = "CAQHS_ProviderPracticeAssociate";
            const string dbtProviderPracticeTax = "CAQHS_ProviderPracticeTax";
            // relationships
            const string dbrProvider2Associate = dbtProvider + dbrSep + dbtProviderAssociate;
            const string dbrProvider2DEA = dbtProvider + dbrSep + dbtProviderDEA;
            const string dbrProvider2Disclosure = dbtProvider + dbrSep + dbtProviderDisclosure;
            const string dbrProvider2Hospital = dbtProvider + dbrSep + dbtProviderHospital;
            const string dbrProvider2License = dbtProvider + dbrSep + dbtProviderLicense;
            const string dbrProvider2Specialty = dbtProvider + dbrSep + dbtProviderSpecialty;
            const string dbrProvider2Practice = dbtProvider + dbrSep + dbtProviderPractice;
            const string dbrPractice2Associates = dbtProviderPractice + dbrSep + dbtProviderPracticeAssociate;
            const string dbrPractice2Tax = dbtProviderPractice + dbrSep + dbtProviderPracticeTax;

            // default values
            const string defaultAppSetting = "ZZ";

            /// <summary>
            ///     The unique record value used when logging record handling exceptions during processing
            ///         of standard extract import. If a requirement asks that all log entries provide
            ///         values for identifying which record is tied to the exception, add to the
            ///         return value from this property.
            /// </summary>
            private string importRecordKey
            {
                get
                {
                    return "CAQH Provider Id: " + this.xmlProviderId;
                }
            }
        #endregion

        #region "Properties"

        
        // tracks errors at the provider level during imports from CAQH
        private bool pdmsProviderErrors;
        private bool PDMSProviderErrors
        {
            get
            {
                return this.pdmsProviderErrors;
            }
            set
            {
                this.pdmsProviderErrors = value;
            }
        }

        // tracks errors at the service location/practice level during imports from CAQH
        private bool pdmsPracticeErrors;
        private bool PDMSPracticeErrors
        {
            get
            {
                return this.pdmsPracticeErrors;
            }
            set
            {
                this.pdmsPracticeErrors = value;
            }
        }


        #endregion

        #region "Public Methods"

        override public void ExecuteJob()
        {
            // Default Job - CAQH Retrieve Return Roster
            this.ExecuteJob(Guid.Parse("72400B92-CEF0-41F8-B9E1-1AB7CBF3C599"));
        }

        public void LoadPDMSFromCAQHStaging(string extractName)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string caqhProviderId = string.Empty;
            string standardExtractId = string.Empty;

            try
            {
                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.LoadingExtractRecords, extractName));

                // generated by sp_Admin_StoredProcBuilder on Oct 22 2012  1:16PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ExtractFile", DbType.String, extractName, true));

                DataSet stagingProviders = DataAccess.ExecuteStoredProcedure("sp_SelectCAQHDataSummaryRecords", parameters, dbtProvider + Constants.pluralEnding);

                // load staging tables into primary tables
                foreach (DataTable table in stagingProviders.Tables)
                {
                    // iterate all records in table
                    foreach (DataRow row in table.Rows)
                    {
                        caqhProviderId = Methods.GetStringValue(row["CAQHProviderID"]);
                        string action = Methods.GetStringValue(row["Action"]);
                        string providerId = Methods.GetStringValue(row["ProviderId"]);

                        switch (action)
                        {
                            case Constants.CAQHSummaryFileStatus.New:
                                StandardExtractProviderAdd(caqhProviderId, providerId);
                                break;

                            case Constants.CAQHSummaryFileStatus.ReattestChanges:
                                break;

                            case Constants.CAQHSummaryFileStatus.ReattestNoChanges:

                                // select the existing provider record and update only the INTERCHANGE_STAGING.Attest_dte_prov m_CAQHProvider with 
                                //      detailRow["LOAD_FILE_DATE_TIME"] and insert into staging table

                                break;

                            default:
                                break;
                        }
                    }
                }

                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.LoadingExtractRecordsComplete, extractName));
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        public void ProviderRefresh()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            int timeout = Methods.GetIntValue(AppSettings.Get("SQL-CommandTimeoutInSeconds","1200"));

            // create log entry
            log.CreateLogEntry("Refresh started");

            // generated by sp_Admin_StoredProcBuilder on Feb 20 2014  1:57PM
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("LOG_THREAD_NUMBER", DbType.Guid, this.ThreadId, true));

            DataAccess.ExecuteStoredProcedure("usp_UberRefresh_Part1", parameters, timeout);

            LoadPDMSFromCAQHStaging("%");

            // generated by sp_Admin_StoredProcBuilder on Feb 20 2014  1:58PM
            // create parameters objects and fill with values
            parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("LOG_THREAD_NUMBER", DbType.Guid, this.ThreadId, true));

            DataAccess.ExecuteStoredProcedure("usp_UberRefresh_Part3", parameters, timeout);

            // create log entry
            log.CreateLogEntry("Refresh complete");
        }

        public void RetrieveStandardExtractFilesOnly()
        {
            try
            {
                RetrieveStandardExtractFilesOnly(DateTime.Now.AddDays(-1));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        public void RetrieveStandardExtractFilesOnly(DateTime extractDate)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            List<string> extractFiles = new List<string>();

            try
            {
                extractFiles = LoadCAQHStagingDataFilesOnly(extractDate);
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete, +logCnt);
        }

        #endregion

        #region "Private Methods"

        private void AddAddress(int partyId, Enumerations.ContactMechanismRoleTypeId addressType, string addressName, int caqhPracticeId, string address, string address2
            , string city, string state, string county, string zip, string extZip, string phone, string fax, string email, string country, int personPartyId = 0
            , bool raiseErrors = true)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            DateTime now = DateTime.Now;
            int? practiceId = null;
            string cmId = string.Empty;
            List<SqlParameter> parameters;

            try
            {
                // if the caqhPracticeId 
                if (caqhPracticeId != -1)
                {
                    practiceId = caqhPracticeId;
                }

                //////////////////////////////////////
                // Addresses               
                //////////////////////////////////////
                // if an address was provided
                if (!string.IsNullOrWhiteSpace(address) && !string.IsNullOrWhiteSpace(city) && !string.IsNullOrWhiteSpace(state))
                {
                    // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:33PM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_NAME", DbType.String, addressName, true));
                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_TYPE_ID", DbType.Int32, Enumerations.ContactMechanismTypeId.PostalAddress, true));
                    parameters.Add(SqlParms.CreateParameter("CAQH_PRACTICE_ID", DbType.Int32, practiceId, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    cmId = DataAccess.ExecuteScalar("insertCONTACT_MECHANISM", parameters);

                    // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:36PM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ROLE_TYPE_ID", DbType.Int32, addressType, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    DataAccess.ExecuteStoredProcedure("insertCONTACT_MECHANISM_ROLE", parameters);

                    // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:37PM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    DataAccess.ExecuteStoredProcedure("insertPARTY_CONTACT_MECHANISMCustom", parameters);

                    // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:38PM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                    parameters.Add(SqlParms.CreateParameter("ADDRESS", DbType.String, address, true));
                    parameters.Add(SqlParms.CreateParameter("ADDRESS2", DbType.String, address2, true));
                    parameters.Add(SqlParms.CreateParameter("CITY", DbType.String, city, true));
                    parameters.Add(SqlParms.CreateParameter("COUNTY", DbType.String, county, true));
                    parameters.Add(SqlParms.CreateParameter("STATE_ABBREV", DbType.String, state, true));
                    parameters.Add(SqlParms.CreateParameter("ZIP_CODE", DbType.String, zip, true));
                    parameters.Add(SqlParms.CreateParameter("EXTENDED_ZIP", DbType.String, extZip, true));
                    parameters.Add(SqlParms.CreateParameter("COUNTRY", DbType.String, string.Empty, true));
                    parameters.Add(SqlParms.CreateParameter("POSTAL_ADDRESS_NAME", DbType.String, addressName, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    DataAccess.ExecuteStoredProcedure("insertPOSTAL_ADDRESSCustom", parameters);
                }
                //////////////////////////////////////
                // Phone Number
                //////////////////////////////////////
                // if a phone number was provided
                if (!string.IsNullOrWhiteSpace(phone))
                {

                    // if the phone is a number
                    if (Methods.IsNumeric(phone, 10))
                    {
                        int id = partyId;
                        if (personPartyId != 0) id = personPartyId;
                        // No auto-closing on ADDs: CloseError(id, Enumerations.ErrorType.P040);

                        // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:33PM
                        // create parameters objects and fill with values
                        parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_NAME", DbType.String, addressName, true));
                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_TYPE_ID", DbType.Int32, Enumerations.ContactMechanismTypeId.TelecomNumber, true));
                        parameters.Add(SqlParms.CreateParameter("CAQH_PRACTICE_ID", DbType.Int32, practiceId, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                        cmId = DataAccess.ExecuteScalar("insertCONTACT_MECHANISM", parameters);

                        // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:36PM
                        // create parameters objects and fill with values
                        parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ROLE_TYPE_ID", DbType.Int32, addressType, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                        DataAccess.ExecuteStoredProcedure("insertCONTACT_MECHANISM_ROLE", parameters);

                        // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:37PM
                        // create parameters objects and fill with values
                        parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                        DataAccess.ExecuteStoredProcedure("insertPARTY_CONTACT_MECHANISMCustom", parameters);

                        // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:43PM
                        // create parameters objects and fill with values
                        parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                        parameters.Add(SqlParms.CreateParameter("AREA_CODE", DbType.Decimal, Methods.Left(phone, 3), true));
                        parameters.Add(SqlParms.CreateParameter("TELECOMMUNICATION_NUMBER", DbType.Decimal, Methods.Right(phone, 7), true));
                        parameters.Add(SqlParms.CreateParameter("COUNTRY_CODE", DbType.Decimal, string.Empty, true));
                        parameters.Add(SqlParms.CreateParameter("TELECOMMUNICATION_NUMBER_TYPE_ID", DbType.Int32, Enumerations.TelecommNumberTypeId.Phone, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                        DataAccess.ExecuteStoredProcedure("insertTELECOMMUNICATIONS_NUMBERCustom", parameters);
                    }
                    else
                    {
                        int id = partyId;
                        if (personPartyId != 0) id = personPartyId;
                        if (raiseErrors) RecordError(id, Enumerations.ErrorType.P040);
                    }

                }

                //////////////////////////////////////
                // Fax Number
                //////////////////////////////////////
                // if a fax number was provided
                if (!string.IsNullOrWhiteSpace(fax))
                {

                    // if the fax is a number
                    if (Methods.IsNumeric(fax, 10))
                    {
                        int id = partyId;
                        if (personPartyId != 0) id = personPartyId;
                        // No auto-closing on ADDs: CloseError(id, Enumerations.ErrorType.P039);

                        // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:33PM
                        // create parameters objects and fill with values
                        parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_NAME", DbType.String, addressName, true));
                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_TYPE_ID", DbType.Int32, Enumerations.ContactMechanismTypeId.TelecomNumber, true));
                        parameters.Add(SqlParms.CreateParameter("CAQH_PRACTICE_ID", DbType.Int32, practiceId, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                        cmId = DataAccess.ExecuteScalar("insertCONTACT_MECHANISM", parameters);

                        // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:36PM
                        // create parameters objects and fill with values
                        parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ROLE_TYPE_ID", DbType.Int32, addressType, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                        DataAccess.ExecuteStoredProcedure("insertCONTACT_MECHANISM_ROLE", parameters);

                        // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:37PM
                        // create parameters objects and fill with values
                        parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                        DataAccess.ExecuteStoredProcedure("insertPARTY_CONTACT_MECHANISMCustom", parameters);

                        // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:43PM
                        // create parameters objects and fill with values
                        parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                        parameters.Add(SqlParms.CreateParameter("AREA_CODE", DbType.Decimal, Methods.Left(fax, 3), true));
                        parameters.Add(SqlParms.CreateParameter("TELECOMMUNICATION_NUMBER", DbType.Decimal, Methods.Right(fax, 7), true));
                        parameters.Add(SqlParms.CreateParameter("COUNTRY_CODE", DbType.Decimal, string.Empty, true));
                        parameters.Add(SqlParms.CreateParameter("TELECOMMUNICATION_NUMBER_TYPE_ID", DbType.Int32, Enumerations.TelecommNumberTypeId.Fax, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                        DataAccess.ExecuteStoredProcedure("insertTELECOMMUNICATIONS_NUMBERCustom", parameters);
                    }
                    else
                    {
                        int id = partyId;
                        if (personPartyId != 0) id = personPartyId;
                        if (raiseErrors) RecordError(id, Enumerations.ErrorType.P039);
                    }
                }

                //////////////////////////////////////
                // Email 
                //////////////////////////////////////
                // if an email was provided
                if (!string.IsNullOrWhiteSpace(email))
                {
                    // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:33PM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_NAME", DbType.String, addressName, true));
                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_TYPE_ID", DbType.Int32, Enumerations.ContactMechanismTypeId.EmailAddress, true));
                    parameters.Add(SqlParms.CreateParameter("CAQH_PRACTICE_ID", DbType.Int32, practiceId, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    cmId = DataAccess.ExecuteScalar("insertCONTACT_MECHANISM", parameters);

                    // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:36PM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ROLE_TYPE_ID", DbType.Int32, addressType, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    DataAccess.ExecuteStoredProcedure("insertCONTACT_MECHANISM_ROLE", parameters);

                    // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:37PM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    DataAccess.ExecuteStoredProcedure("insertPARTY_CONTACT_MECHANISMCustom", parameters);

                    // generated by sp_Admin_StoredProcBuilder on Jul 19 2012  4:48PM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ID", DbType.Int32, cmId, true));
                    parameters.Add(SqlParms.CreateParameter("ELECTRONIC_ADDRESS_STRING", DbType.String, email, true));
                    parameters.Add(SqlParms.CreateParameter("ELECTRONIC_ADDRESS_TYPE_ID", DbType.Int32, 15, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    DataAccess.ExecuteStoredProcedure("insertELECTRONIC_ADDRESSCustom", parameters);
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Address", address, ex.Message);
            }
        }

        private int AddCAQHPracticeId(int partyId, int idNumber)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            int practiceId = idNumber;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jun 26 2012 10:53AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("CAQH_PRACTICE_ID_VALUE", DbType.Int32, idNumber, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                practiceId = Convert.ToInt32(DataAccess.ExecuteStoredProcedure("sp_SaveCAQHPracticeId", parameters, SqlDbType.Int));
                return practiceId;
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "CAQH Practice Id", idNumber.ToString(), ex.Message);
                return practiceId;
            }
        }

        private void AddCommunicationEvent(int RegId, Enumerations.CommunicationEventTypeId eventType, DateTime eventDate, string eventValue)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jun 26 2012  2:07PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(eventType), true));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, eventDate, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, true));

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

        private void AddDisclosure(int RegId, string disclosureText, DateTime eventDate, string eventValue)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // if the event date is valid
                if (eventDate <= DateTime.Now)
                {
                    // generated by sp_Admin_StoredProcBuilder on Aug 28 2012  3:56PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TEXT", DbType.String, disclosureText, true));
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, true));
                    parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, eventValue, true));
                    parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, eventDate, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    DataAccess.ExecuteStoredProcedure("sp_InsertCOMMUNICATION_EVENT", parameters);
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Communication Event", disclosureText, ex.Message, "Event Value: " + eventValue);
            }
        }

        private int AddLicense(int partyId, string licenseTypeAbbrev, string licenseNumber, DateTime issueDate
            , DateTime expirationDate, string state, int licenseId = 0)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            int newId = 0;

            try
            {
                // if a null type description, record error
                if (string.IsNullOrWhiteSpace(licenseTypeAbbrev))
                {
                    RecordError(partyId, Enumerations.ErrorType.P050);
                }
                else
                {
                    // No auto-closing on ADDs: CloseError(partyId, Enumerations.ErrorType.P050);

                    string licenseTypeId = SelectLicenseTypeId(licenseTypeAbbrev);

                    // if a valid license type
                    if (licenseTypeId.Length > 0)
                    {
                        // No auto-closing on ADDs: CloseError(partyId, Enumerations.ErrorType.P018);

                        // generated by sp_Admin_StoredProcBuilder on Aug  1 2012 11:33AM
                        // create parameters objects and fill with values
                        List<SqlParameter> parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("LICENSE_TYPE_ID", DbType.Int32, licenseTypeId, true));
                        parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                        parameters.Add(SqlParms.CreateParameter("LICENSE_NUMBER", DbType.String, licenseNumber, true));
                        parameters.Add(SqlParms.CreateParameter("ISSUE_DATE", DbType.DateTime, issueDate, true));
                        parameters.Add(SqlParms.CreateParameter("EXPIRATION_DATE", DbType.DateTime, expirationDate, true));
                        parameters.Add(SqlParms.CreateParameter("STATE_ABBREV", DbType.String, state, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                        // TODO: if it does not return a single count, generate log entry, reset counter
                        newId = Convert.ToInt32(DataAccess.ExecuteScalar("insertLICENSURE", parameters));
                    }
                    else // invalid license type
                    {
                        RecordError(partyId, Enumerations.ErrorType.P018);
                    }
                }
            }
            catch (Exception ex)
            {
                string extraInfo = "LicenseTypeDesc: " + licenseTypeAbbrev;
                LogErrorRecord(log, "License", licenseNumber, ex.Message, extraInfo);
            }
            return newId;
        }

        private void AddLicenseToPractices(int partyId, int licensureId)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            int recCnt;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Aug 27 2012 12:03PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("LICENSURE_ID", DbType.Int32, licensureId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                recCnt = Convert.ToInt32(DataAccess.ExecuteStoredProcedure("sp_InsertLicensesForAllPractices", parameters, SqlDbType.Int));

            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "License Id", licensureId.ToString(), ex.Message);
            }
        }

        private string AddMedicaidId(int partyId, int practiceId = -1, string medicaidId = "")
        {
            DateTime now = DateTime.Now;
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string medicaidPk;
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul 23 2012  9:16AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (medicaidId.Length > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, true));
                }
                // parameters.Add(SqlParms.CreateParameter("PARTY_IDENTIFICATION_NUMBER_ID", DbType.Int32, , true));
                parameters.Add(SqlParms.CreateParameter("party_id", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                medicaidPk = DataAccess.ExecuteScalar("insertMEDICAID_ID", parameters);

                if (practiceId != -1)
                {
                    // generated by sp_Admin_StoredProcBuilder on Jul 23 2012  9:10AM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPk, true));
                    parameters.Add(SqlParms.CreateParameter("CAQH_PRACTICE_ID", DbType.Int32, practiceId, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    DataAccess.ExecuteStoredProcedure("insertMEDICAID_ID_PRACTICE_XREFCustom", parameters);
                }
            }
            catch (Exception ex)
            {
                // LogErrorRecord(log, "Medicaid Id", name, ex.Message);
                throw ex;
            }
            return medicaidPk;
        }

        private int AddPerson(string firstName, string lastName, string middleName, string gender, string suffix, DateTime? birthDate, Enumerations.PartyRoleTypeId roleType)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            int returnVal = 0;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jun 26 2012  3:25PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, firstName, true));
                parameters.Add(SqlParms.CreateParameter("MIDDLE_NAME", DbType.String, middleName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, lastName, true));
                parameters.Add(SqlParms.CreateParameter("GENDER", DbType.String, Methods.Left(gender, 1), true));
                parameters.Add(SqlParms.CreateParameter("SUFFIX", DbType.String, suffix, true));
                parameters.Add(SqlParms.CreateParameter("BIRTH_DATE", DbType.DateTime, birthDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                string returnString = Convert.ToString(DataAccess.ExecuteStoredProcedure("sp_InsertPerson", parameters, SqlDbType.Int));
                returnVal = Convert.ToInt32(returnString);

                if (returnVal > 0)
                {
                    // generated by sp_Admin_StoredProcBuilder on Sep  6 2012 10:51AM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, returnVal, true));
                    parameters.Add(SqlParms.CreateParameter("ROLE_TYPE_ID", DbType.Int32, roleType, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    DataAccess.ExecuteStoredProcedure("insertPARTY_ROLE", parameters);
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Person Name", firstName + " " + lastName, ex.Message);
            }
            return returnVal;
        }

        private int AddPersonOrganization(int partyId, Enumerations.OrganizationTypeId typeId, string name)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            int returnVal = 0;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jun 26 2012  3:25PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("ORGANIZATION_NAME", DbType.String, name, true));
                parameters.Add(SqlParms.CreateParameter("ORGANIZATION_TYPE_ID", DbType.Int32, Convert.ToInt32(typeId), true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                string returnString = Convert.ToString(DataAccess.ExecuteStoredProcedure("sp_SavePersonOrganization", parameters, SqlDbType.Int));
                returnVal = Convert.ToInt32(returnString);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Organization Name", name, ex.Message);
            }
            return returnVal;
        }

        private void AddPartyIdentification(int partyId, string idNumberValue, Enumerations.PartyIdentificationNumberTypeId typeId)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jun 25 2012 10:04AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_IDENTIFICATION_NUMBER_TYPE_ID", DbType.Int32, Convert.ToInt32(typeId), false));
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("PARTY_IDENTIFICATION_NUMBER", DbType.String, idNumberValue, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("insertPARTY_IDENTIFICATION_NUMBER", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Identification Number", idNumberValue, ex.Message);
            }
        }

        private int AddPartyProviderType(int partyId, string typeAbbrevation)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string returnVal = "0";

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jun 26 2012 11:52AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ABBREVIATION", DbType.String, typeAbbrevation, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                returnVal = DataAccess.ExecuteScalar("sp_SavePartyProviderType", parameters);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Party Type", typeAbbrevation, ex.Message);
            }
            return Convert.ToInt32(returnVal);
        }

        private void AddPartySpecialty(int partyId, int specialtyTypeId, string boardCertified, DateTime certificationDate, string specialtyBoardName)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul 10 2012  5:11PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE_ID", DbType.Int32, specialtyTypeId, true));
                parameters.Add(SqlParms.CreateParameter("BOARD_CERTIFIED", DbType.String, boardCertified, true));
                parameters.Add(SqlParms.CreateParameter("CERTIFICATION_DATE", DbType.Date, certificationDate, true));
                parameters.Add(SqlParms.CreateParameter("SPECIALTY_BOARD_NAME", DbType.String, specialtyBoardName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("sp_SavePartySpecialty", parameters);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Party Specialty", specialtyTypeId.ToString(), ex.Message);
            }
        }

        private int AddPartyTax(int partyId, string taxValue, Enumerations.TaxIdTypeId taxType)
        {
            int returnVal = 0;
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jun 22 2012  9:36AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("TAX_ID_VALUE", DbType.String, taxValue, true));
                parameters.Add(SqlParms.CreateParameter("TAX_ID_TYPE_ID", DbType.Int32, Convert.ToInt32(taxType), true));
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                string returnString = Convert.ToString(DataAccess.ExecuteScalar("insertTAX_ID", parameters));
                returnVal = Convert.ToInt32(returnString);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Party Tax Number", taxValue, ex.Message);
            }
            return returnVal;
        }

        private void AddPracticeAssociate(string medicaidPk, Enumerations.PartyRoleTypeId partyRoleType, string firstName, string lastName)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Aug 31 2012 10:59AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ROLE_TYPE_ID", DbType.Int32, partyRoleType, true));
                parameters.Add(SqlParms.CreateParameter("medicaid_PK", DbType.Int32, medicaidPk, true));
                parameters.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, firstName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, lastName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("sp_AddPracticeAssociate", parameters);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Practice Associate", medicaidPk, ex.Message);
            }
        }

        private void AddPracticeOrganization(int partyId, string medicaidPk, Enumerations.PartyRoleTypeId roleType, string name)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep  3 2012 10:16AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("ROLE_TYPE_ID", DbType.Int32, roleType, true));
                parameters.Add(SqlParms.CreateParameter("medicaid_PK", DbType.Int32, medicaidPk, true));
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("sp_AddPracticeOrganization", parameters);
            }
            catch (Exception ex)
            {
                string msg = String.Format(Constants.LogString.PracticeOrganization, partyId.ToString(), medicaidPk, roleType.ToString());
                LogErrorRecord(log, msg, string.Empty, ex.Message);
            }
        }

        private void AddPracticeTax(int practiceId, int taxId)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul 11 2012  5:41PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("CAQH_PRACTICE_ID", DbType.Int32, practiceId, true));
                parameters.Add(SqlParms.CreateParameter("TAX_ID", DbType.Int32, taxId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("insertTAXID_PRACTICEID_XREFCustom", parameters);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "CAQH Practice to Tax Number", practiceId.ToString(), ex.Message);
            }
        }

            private void AddProviderPropertyType(int partyId, Enumerations.ProviderPropertyTypeId typeId, string propertyValue)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jun 26 2012  2:19PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_PROPERTY_TYPE_ID", DbType.Int32, Convert.ToInt32(typeId), true));
                parameters.Add(SqlParms.CreateParameter("PROPERTY_VALUE", DbType.String, propertyValue, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("insertPROVIDER_PROPERTY", parameters);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Provider Property", typeId.ToString(), ex.Message, "Value: " + propertyValue);
            }
        }

        private void ApplyBusinessRules(int partyId)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Oct 22 2012  1:49PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_SaveBusinessRuleErrors", parameters);
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break(); 
                LogErrorRecord(log, "Business Rules Party Id", partyId.ToString(), ex.Message);
            }
        }

        private bool IsLicenseUsedInMMIS(string licenseTypeAbbrev)
        {
            bool returnVal = false;
            string results = string.Empty;
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Aug 28 2012  3:23PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("LICENSE_TYPE_ABBREV", DbType.String, licenseTypeAbbrev, true));

                results = DataAccess.ExecuteScalar("sp_SelectLicenseUsedInMMIS", parameters);
                returnVal = Methods.GetBoolFromDataModelAffirmed(results);
            }
            catch (Exception ex)
            {
                // no action
                string err = ex.Message;
            }
            return returnVal;
        }

        private List<string> LoadCAQHStagingDataFilesOnly(DateTime extractDate)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            List<string> returnVal = new List<string>();

            try
            {
                // create log entry
                log.CreateLogEntry(Constants.LogString.LoadingConfig, +logCnt);

                // retrieve testing setting
                DirectoryInfo localDirectory;
                string localPath = AppSettings.Get("CAQH-StandardExtractLocalPath");
                string summaryFileName = AppSettings.Get("CAQH-DataSummaryFile");
                string summaryFullFileName;
                string testingEnabled = AppSettings.Get("InterfaceTestingCAQH").ToLower();

                // if testing enabled
                if (testingEnabled == bool.TrueString.ToLower())
                {
                    localPath = localPath + @"Test\";
                }
                    
                if (Directory.Exists(localPath) == false)
                {
                    log.CreateLogEntry(Constants.LogString.TestDirectoryNotFound, Logging.LogPriority.Important, (logCnt += 1));
                }

                log.CreateLogEntry(String.Format(Constants.LogString.TestDirectoryLoad, localPath), +logCnt);

                localDirectory = new DirectoryInfo(localPath);

                // log the number of files retreived
                log.CreateLogEntry(String.Format(Constants.LogString.FileCountToProcess, localDirectory.GetFiles().Length));

                // loop over the test documents and attempting to import 
                foreach (FileInfo file in localDirectory.GetFiles())
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.LoadingFile, file.Name));

                    //  unzip files
                    FileCompression fc = new FileCompression(this.ThreadId);
                    string extractFile = file.Name.Replace(".zip", string.Empty);
                    string extractDayPath = extractFile.Substring(26, 4) + extractFile.Substring(22, 4);

                    string extractPath = localDirectory.FullName + extractFile + @"\";


                        fc.UnzipFile(file.FullName, extractPath);

                        // load the summary file
                        summaryFullFileName = LoadSummaryFile(extractPath + summaryFileName, extractFile);

                        DataSet providers = SelectDataSummaryUniqueProviders(summaryFullFileName);


                        log.CreateLogEntry(String.Format(Constants.LogString.ProcessingFilesInDir, extractPath));

                        // load xml files into staging tables
                        foreach (DataTable table in providers.Tables)
                        {
                            // iterate all records in table
                            foreach (DataRow row in table.Rows)
                            {
                                string caqhProviderId = Methods.GetStringValue(row["CAQH_PIN"]);
                                string action = Methods.GetStringValue(row["STATUS"]);
                                StandardExtractStagingExtractFilesOnly(extractPath, extractFile, caqhProviderId, action, extractDayPath);
                            }
                        }

                        // create log entry
                        log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileComplete, summaryFileName), +logCnt);

                        // append extract file to returned list of files
                        returnVal.Add(extractFile);

                        // delete the extracted folder
                        DirectoryInfo di = new DirectoryInfo(extractPath);
                        di.Delete(true);

                    string archiveDirectory = localPath + @"Archive\";
                    Directory.CreateDirectory(archiveDirectory);
                    if (File.Exists(archiveDirectory + file.Name))
                    {
                        File.Delete(archiveDirectory + file.Name);
                    }
                    file.MoveTo(archiveDirectory + file.Name);
                }
                log.CreateLogEntry(Constants.LogString.ProcessingComplete, +logCnt);
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            return returnVal;
        }

        private void LoadReturnRosterStatuses(string fileName, string caqhProviderId = "")
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            DateTime now = DateTime.Now;
            List<SqlParameter> parameters;

            try
            {
                // Bug 4489: Prevent deleting return roster records if only updating statuses on a single provider
                if (string.IsNullOrWhiteSpace(caqhProviderId))
                {
                    // generated by sp_Admin_StoredProcBuilder on Dec 12 2012 11:58AM
                    // create parameters objects and fill with values
                    parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("LOAD_FILE_NAME", DbType.String, fileName, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                    DataAccess.ExecuteStoredProcedure("usp_DeleteOldReturnRosters", parameters);
                }

                // generated by sp_Admin_StoredProcBuilder on Sep  3 2012 11:03AM
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("LOAD_FILE_NAME", DbType.String, fileName, true));
                if (!string.IsNullOrWhiteSpace(caqhProviderId))
                {
                    parameters.Add(SqlParms.CreateParameter("CAQH_PIN", DbType.String, caqhProviderId, true));
                }
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("sp_InsertRosterStatuses", parameters);

                // generated by sp_Admin_StoredProcBuilder on Sep  3 2012 12:23PM
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("LOAD_FILE_NAME", DbType.String, fileName, true));
                if (!string.IsNullOrWhiteSpace(caqhProviderId))
                {
                    parameters.Add(SqlParms.CreateParameter("CAQH_PIN", DbType.String, caqhProviderId, true));
                }
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("sp_UpdateRosterStatuses", parameters);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                log.CreateLogEntry("Failure when attempting to insert PDMS Error. Details: " + msg, Logging.LogPriority.Error);
            }
        }

        private string LoadSummaryFile(string fullFileName, string fileName)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            // verify the data summary file has not been loaded before.
            if (SelectDataSummaryUniqueProviders(fileName).Tables[0].Rows.Count == 0)
            {

                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.LoadingFile, fileName));

                // open the file with FileHelper class and set internal variables
                CAQHSummaryFileRecord[] summaryRecords;
                FileHelperEngine engine = new FileHelperEngine(typeof(CAQHSummaryFileRecord));

                try
                {
                    engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                    summaryRecords = engine.ReadFile(fullFileName) as CAQHSummaryFileRecord[];
                    DateTime loadDateTime = DateTime.Now;

                    // create log entry
                    log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString()
                        , engine.ErrorManager.ErrorCount.ToString()), +logCnt);

                    // loop over all records and write to database
                    foreach (CAQHSummaryFileRecord summaryRecord in summaryRecords)
                    {
                        try
                        {
                            // create parameters objects and fill with values
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            SqlParameter parameter;
                            parameter = new SqlParameter("@LOAD_FILE_NAME", fileName);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@LOAD_FILE_DATE_TIME", loadDateTime);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@CAQH_PIN", summaryRecord.pin);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@PLAN_PROVIDER_ID", Methods.GetDecimalValue(summaryRecord.planProviderId, true));
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@LICENSE_NUMBER", summaryRecord.licenseNumber);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@LAST_NAME", summaryRecord.lastName);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@FIRST_NAME", summaryRecord.firstName);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@MIDDLE_NAME", summaryRecord.middleName);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@PRIMARY_PRACTICE_STATE", summaryRecord.primaryPracticeState);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@STATUS", summaryRecord.status);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@XPATH", summaryRecord.xPath);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@CAQH_TABLE", summaryRecord.table);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@ELEMENT", summaryRecord.element);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@OLD_DATA", summaryRecord.oldData);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@NEW_DATA", summaryRecord.newData);
                            parameters.Add(parameter);
                            parameter = new SqlParameter("@ANNIVERSARY_DATE", Methods.GetDateTimeValue(summaryRecord.anniversaryDate, true));
                            parameters.Add(parameter);
                            parameter = new SqlParameter(Constants.dbpModDate, DateTime.Now);
                            parameters.Add(parameter);
                            parameter = new SqlParameter(Constants.dbpModUser, Constants.appPDMSDataExchangeUserId);
                            parameters.Add(parameter);

                            // execute the update stored procedure with values
                            DataAccess.ExecuteStoredProcedure("sp_AddCAQHDataSummaryRecord", parameters);
                        }
                        catch (Exception ex)
                        {
                            LogFileRecordError(log, engine.LineNumber.ToString(), fileName, ex.Message);
                            // do not rethrow exception so the next record is processed
                        }
                    }

                    // record all bad errors
                    if (engine.ErrorManager.HasErrors)
                    {
                        foreach (ErrorInfo err in engine.ErrorManager.Errors)
                        {
                            LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFileRecordError(log, engine.LineNumber.ToString(), fileName, Constants.LogString.FileLoadFailure + ex.Message);
                    // do not rethrow exception so the next file is processed
                }

                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.LoadingFileComplete, fileName));
            }
            else
            {
                // create log entry
                log.CreateLogEntry(String.Format("Skipping load of summary file [{0}], loaded previously", fileName), +logCnt);
            }
            return fileName;
        }

        private void LogErrorRecord(Logging log, string recordType, string recordValue, string exceptionMessage, string additionRecordInformation = "")
        {
            if (additionRecordInformation.Length > 0)
            {
                additionRecordInformation = ", " + additionRecordInformation;
            }
            string message = String.Format(Constants.LogString.ExtractErrorRecordFailure, this.importRecordKey, recordType + ": " + recordValue, additionRecordInformation);
            message += Environment.NewLine + exceptionMessage;
            log.CreateLogEntry(message, Logging.LogPriority.DataLoadIssues);
        }

        private void LogFileRecordError(Logging log, string lineNumber, string fileName, string errorMessage)
        {
            string fullMessage = "Record failed. Line No: {0}; File: {1}; Reason: {2}";
            log.CreateLogEntry(String.Format(fullMessage, lineNumber, fileName, errorMessage), Logging.LogPriority.DataLoadIssues, +logCnt);
        }

        private void RecordError(int partyId, Enumerations.ErrorType errorType, string medicaidPK = "")
        {
            DateTime now = DateTime.Now;
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Dec 26 2012  9:20AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_TYPE_ID", DbType.Int32, errorType, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_SavePDMSError", parameters);
            }
            catch (Exception ex)
            {
                string msg = ex.Message + " [Error: " + errorType.ToString() + "]";
                log.CreateLogEntry("Failure when attempting to insert PDMS Error. Details: " + msg, Logging.LogPriority.Error);
            }
        }

        private void CloseError(int partyId, Enumerations.ErrorType errorType, string medicaidPK = "")
        {
            DateTime now = DateTime.Now;
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Dec 26 2012  9:20AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_TYPE_ID", DbType.Int32, errorType, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_ClosePDMSError", parameters);
            }
            catch (Exception ex)
            {
                string msg = ex.Message + " [Error: " + errorType.ToString() + "]";
                log.CreateLogEntry("Failure when attempting to insert PDMS Error. Details: " + msg, Logging.LogPriority.Error);
            }
        }

        private void RecordPDMSStatus(int partyId, int pdmsStatusType, Enumerations.RecordStatusEnum action
            , string medicaidPK = "", string caqhProviderId = "")
        {
            DateTime now = DateTime.Now;
            string providerStatusId;
            List<SqlParameter> parameters;

            if (action == Enumerations.RecordStatusEnum.Create)
            {
                // generated by sp_Admin_StoredProcBuilder on Dec 26 2012  9:33AM
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_DATE", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_TYPE_ID", DbType.Int32, Enumerations.ProviderStatusChangeTypeId.PDMS, true));
                parameters.Add(SqlParms.CreateParameter("PDMS_STATUS_TYPE_ID", DbType.Int32, pdmsStatusType, true));
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_SavePartyStatus", parameters);

                // do not load for service locations
                if (medicaidPK.Length == 0)
                {
                    // load the return roster (in case there are CAQH errors for this provider)
                    LoadReturnRosterStatuses("%", caqhProviderId);
                }
            }

            if (action == Enumerations.RecordStatusEnum.Update)
            {
                providerStatusId = SelectProviderStatusId(partyId, medicaidPK);

                // generated by sp_Admin_StoredProcBuilder on Dec 26 2012  9:33AM
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_DATE", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_TYPE_ID", DbType.Int32, Enumerations.ProviderStatusChangeTypeId.PDMS, true));
                parameters.Add(SqlParms.CreateParameter("PDMS_STATUS_TYPE_ID", DbType.Int32, pdmsStatusType, true));
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_SavePartyStatus", parameters);
            }
        }

        private void SaveProviderTransactions(int partyId)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Oct 22 2013  8:10AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_SaveProviderTransactions", parameters);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Save Transaction", partyId.ToString(), ex.Message);
            }
        }

        private void SaveXmlIds(string caqhId)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {

            // generated by sp_Admin_StoredProcBuilder on Mar  4 2014 12:29PM
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("CAQH_ID", DbType.String, caqhId, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

            DataAccess.ExecuteStoredProcedure("usp_SaveXmlIds", parameters);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "XmlIds", caqhId, ex.Message);
            }
        }

        private DataSet SelectDataSummaryUniqueProviders(string loadFileName)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul 23 2012 11:07AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("LOAD_FILE_NAME", DbType.String, loadFileName, true));

                return DataAccess.ExecuteStoredProcedure("sp_SelectCAQHUniqueProviders", parameters, "SummaryFile");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string SelectInitialRosterProviderId(int partyId)
        {
            string returnVal = string.Empty;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jan 22 2013  4:31PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                returnVal = Convert.ToString(DataAccess.ExecuteStoredProcedure("usp_SaveMMISProviderMatchingRuleErrors", parameters,"PROVIDER_ID", SqlDbType.VarChar, 100));
            }
            catch
            {
                // no action
            }
            return returnVal;
        }

        private string SelectLicenseTypeId(string licenseTypeAbbrevation)
        {
            string returnVal = string.Empty;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Aug  1 2012 10:21AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("LICENSE_TYPE_ABBREV", DbType.String, licenseTypeAbbrevation, true));

                returnVal = DataAccess.ExecuteScalar("sp_SelectLicenseTypeByAbbrev", parameters);
            }
            catch
            {
                // no action
            }
            return returnVal;
        }

        private string SelectProviderStatusId(int partyId, string MedicaidPk = "")
        {
            int returnVal = 0;
            string results = string.Empty;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep  4 2012 12:52PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                if (MedicaidPk.Length > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, MedicaidPk, true));
                }

                results = DataAccess.ExecuteScalar("sp_SelectProviderStatusId", parameters);
                if (Methods.IsNumeric(results))
                {
                    returnVal = Convert.ToInt32(results);
                }
            }
            catch
            {
                // no action
            }
            return returnVal.ToString();
        }

        private int SelectProviderTypeId(string providerTypeAbbrev)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            int returnVal = 0;
            string results = string.Empty;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul 12 2012  3:03PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ABBREVIATION", DbType.String, providerTypeAbbrev, false));

                results = DataAccess.ExecuteScalar("sp_SelectProviderType", parameters);
                if (Methods.IsNumeric(results))
                {
                    returnVal = Convert.ToInt32(results);
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Provider Type", providerTypeAbbrev, ex.Message);
            }
            return returnVal;
        }

        private int SelectSpecialtyTypeId(string specialtyType, string providerType)
        {
            int returnVal = 0;
            string results = string.Empty;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul  9 2012 12:36PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TAXONOMY_NAME", DbType.String, specialtyType, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ABBREVIATION", DbType.String, providerType, true));

                results = DataAccess.ExecuteScalar("sp_SelectSpecialtyType", parameters);
                if (Methods.IsNumeric(results))
                {
                    returnVal = Convert.ToInt32(results);
                }
            }
            catch
            {
                // no action
            }
            return returnVal;
        }

        private DataSet SelectStagingProvider(string stagingProviderId)
        {

            DataSet returnVal = new DataSet();
            const string Id = "ProviderId";
            const string PracticeId = "ProviderPracticeId";

            try
            {

                // generated by sp_Admin_StoredProcBuilder on Oct 16 2012 12:32PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ProviderId", DbType.Int32, stagingProviderId, true));

                returnVal = DataAccess.ExecuteStoredProcedure("usp_SelectCAQHStagingProvider"
                    , parameters, (dbtProvider + Constants.pluralEnding));

                returnVal.Tables[0].TableName = dbtProvider;
                returnVal.Tables[1].TableName = dbtProviderAssociate;
                returnVal.Tables[2].TableName = dbtProviderDEA;
                returnVal.Tables[3].TableName = dbtProviderDisclosure;
                returnVal.Tables[4].TableName = dbtProviderHospital;
                returnVal.Tables[5].TableName = dbtProviderLicense;
                returnVal.Tables[6].TableName = dbtProviderSpecialty;
                returnVal.Tables[7].TableName = dbtProviderPractice;
                returnVal.Tables[8].TableName = dbtProviderPracticeAssociate;
                returnVal.Tables[9].TableName = dbtProviderPracticeTax;

                returnVal.Relations.Add(new DataRelation(dbrProvider2Associate
                    , returnVal.Tables[dbtProvider].Columns[Id]
                    , returnVal.Tables[dbtProviderAssociate].Columns[Id]));

                returnVal.Relations.Add(new DataRelation(dbrProvider2DEA
                    , returnVal.Tables[dbtProvider].Columns[Id]
                    , returnVal.Tables[dbtProviderDEA].Columns[Id]));

                returnVal.Relations.Add(new DataRelation(dbrProvider2Disclosure
                    , returnVal.Tables[dbtProvider].Columns[Id]
                    , returnVal.Tables[dbtProviderDisclosure].Columns[Id]));

                returnVal.Relations.Add(new DataRelation(dbrProvider2Hospital
                    , returnVal.Tables[dbtProvider].Columns[Id]
                    , returnVal.Tables[dbtProviderHospital].Columns[Id]));

                returnVal.Relations.Add(new DataRelation(dbrProvider2License
                    , returnVal.Tables[dbtProvider].Columns[Id]
                    , returnVal.Tables[dbtProviderLicense].Columns[Id]));

                returnVal.Relations.Add(new DataRelation(dbrProvider2Specialty
                    , returnVal.Tables[dbtProvider].Columns[Id]
                    , returnVal.Tables[dbtProviderSpecialty].Columns[Id]));

                returnVal.Relations.Add(new DataRelation(dbrProvider2Practice
                    , returnVal.Tables[dbtProvider].Columns[Id]
                    , returnVal.Tables[dbtProviderPractice].Columns[Id]));

                returnVal.Relations.Add(new DataRelation(dbrPractice2Associates
                    , returnVal.Tables[dbtProviderPractice].Columns[PracticeId]
                    , returnVal.Tables[dbtProviderPracticeAssociate].Columns[PracticeId]));

                returnVal.Relations.Add(new DataRelation(dbrPractice2Tax
                    , returnVal.Tables[dbtProviderPractice].Columns[PracticeId]
                    , returnVal.Tables[dbtProviderPracticeTax].Columns[PracticeId]));

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                // no action
            }
            return returnVal;

        }

        private void EmailErrorsNotification(int RegId)
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            string body = string.Empty;
            string emailFrom = string.Empty;
            string emailTo = string.Empty;
            string subject = string.Empty;
            string templateName = string.Empty;
            string templatesDirectory = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;
            string NPI = string.Empty;
            int fromPartyId = 0;
            DateTime now = DateTime.Now;

            try
            {
                List<SqlParameter> parameters;

                fromPartyId = ProviderController.GetAdminPartyId();
                templatesDirectory = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                emailFrom = AppSettings.Get("SmtpFromEmailAddress", string.Empty);

                // generated by sp_Admin_StoredProcBuilder on Sep  5 2012  3:10PM
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, true));
                emailTo = DataAccess.ExecuteScalar("sp_SelectProviderContactEmail", parameters);

                // generated by sp_Admin_StoredProcBuilder on Sep  5 2012  1:53PM
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_NAME", DbType.String, "Additional Info Existing", true));
                DataSet document = DataAccess.ExecuteStoredProcedure("sp_SelectDocumentTypes", parameters, "DOCUMENTS");
                if (ObjectControllerHelper.HasRows(document))
                {
                    subject = ObjectControllerHelper.GetString("DOCUMENT_SUBJECT", document.Tables[0].Rows[0]);
                    templateName = ObjectControllerHelper.GetString("DOCUMENT_FILE_NAME", document.Tables[0].Rows[0]);
                }

                // get the list of error for the provider
                // generated by sp_Admin_StoredProcBuilder on Sep  5 2012  1:28PM
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, true));
                DataSet errors = DataAccess.ExecuteStoredProcedure("usp_SelectEmailErrorText", parameters, "ERRORS");

                // loop over all errors and append to error string
                string errorText = string.Empty;
                foreach (DataRow error in errors.Tables[0].Rows)
                {
                    firstName = Methods.GetStringValue(error["FirstName"]);
                    lastName = Methods.GetStringValue(error["LastName"]);
                    NPI = Methods.GetStringValue(error["NPI"]);
                    string thisErrorText = Methods.GetStringValue(error["EMAIL_ERROR_TEXT"]);
                    
                    // HACK: To prevent emails from being sent for errors that have not had their associate resolving action strings set
                    //          to not add the new line
                    if (!string.IsNullOrWhiteSpace(thisErrorText))
                    {
                        errorText += thisErrorText.Trim() + System.Environment.NewLine;
                    }
                }

                // if any error exist
                if (errorText.Length > 0)
                {
                    fields.Add("PROVIDERNAME", firstName + " " + lastName);
                    fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    fields.Add("NPI", NPI);
                    fields.Add("REASON", errorText);

                    // If enabled, send the email
                    string emailDisabled = AppSettings.Get("CAQH-DisableImportEmails", bool.TrueString).ToLower();
                    EMailNotification notify = new EMailNotification(string.Empty, subject, emailTo);
                    if (emailDisabled == bool.FalseString.ToLower())
                    {
                        
                        body = notify.SendNotification(templatesDirectory + "\\" + templateName, fields, false);
                        
                    }

                    // Create the communicaton event and email
                    parameters = new List<SqlParameter>();
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
                    parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, new Guid("00000000-0000-0000-0000-000000000000"), false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                    parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean, notify.isEmailSent, true));
                    parameters.Add(SqlParms.CreateParameter("log_message", DbType.String, notify.log_message, true));
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
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private void NPIComparision(string caqhProviderId, bool addFlag)
        {
            try
            {
                // Pull the PDMS and Submit Roster data using the caqhProviderID
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CAQH_PIN", DbType.String, caqhProviderId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectProviderNPIs", parameters, "ProviderNPIs");
                if (!ObjectControllerHelper.HasRows(ds)) return;
                DataRow row = ds.Tables[0].Rows[0];
                string pdmsNPI = Methods.GetStringValue(row["PdmsNPI"]);
                string submitRosterNPI = Methods.GetStringValue(row["SubmitRosterNPI"]);
                if (string.IsNullOrEmpty(pdmsNPI) || string.IsNullOrEmpty(submitRosterNPI)) return;
                if (pdmsNPI == submitRosterNPI)
                {
                    if (!addFlag)
                    {
                        CloseError(Methods.GetIntValue(row["PARTY_ID"]), Enumerations.ErrorType.P053);
                    }
                    return;
                }
                // They are not equal generate an error
                RecordError(Methods.GetIntValue(row["PARTY_ID"]), Enumerations.ErrorType.P053);
            }
            catch (Exception ex)
            {
                // Create log object
                string logMsg = String.Format(Constants.LogString.MethodSignature,
                    MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Logging log = new Logging(this.ThreadId, logMsg);
                LogErrorRecord(log, "CAQH Id", caqhProviderId, ex.Message);
            }
        }

        private void StandardExtractProviderAdd(string caqhProviderId, string stagingProviderId)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            int partyId;
            DataSet provider = new DataSet();
            string providerType = string.Empty;

            try
            {
                // reset pdms error variable
                this.PDMSProviderErrors = false;
                string defaultCountry = AppSettings.Get("DefaultCountryForCAQHImport",defaultAppSetting);
                string defaultBoardName = AppSettings.Get("CAQH-DefaultSpecialtyBoardName",defaultAppSetting);
                string defaultState = AppSettings.Get("CAQH-DefaultState", defaultAppSetting);
                string defaultPracticeName = AppSettings.Get("CAQH-DefaultPracticeName", defaultAppSetting);
                string fName = string.Empty;
                string lName = string.Empty;
                string npi = string.Empty;
                string servicingEmail = string.Empty;
                string practiceState = string.Empty;
                string mmisProviderId = string.Empty;

                // verify if provivder exists
                int existingPartyId = VerifyProviderExists(caqhProviderId);

                // if the provider did not exist previously
                if (existingPartyId == 0)
                { 
                    // retrieve the provider detail
                    provider = SelectStagingProvider(stagingProviderId);

                    // log provider load
                    log.CreateLogEntry(string.Format(Constants.LogString.ExtractLoadPDMSProvider, Constants.CAQHSummaryFileStatus.New, caqhProviderId));

                    // load the provider
                    foreach (DataRow providerInfo in provider.Tables[dbtProvider].Rows)
                    {
                        //////////////////////////////////////
                        // Person information               //
                        //////////////////////////////////////
                        fName = Methods.GetStringValue(providerInfo["FirstName"]);
                        lName = Methods.GetStringValue(providerInfo["LastName"]);

                        partyId = AddPerson(fName, lName, Methods.GetStringValue(providerInfo["MiddleName"])
                            , Methods.GetStringValue(providerInfo["Gender"])
                            , Methods.GetStringValue(providerInfo["Suffix"])
                            , Convert.ToDateTime(Methods.GetDateTimeValue(providerInfo["BirthDate"],false))
                            , Enumerations.PartyRoleTypeId.Provider);

                        if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.Print("caqhProviderId: " + caqhProviderId.ToString());

                        //////////////////////////////////////
                        // Person NPI
                        //////////////////////////////////////
                        npi = Methods.GetStringValue(providerInfo["NPI"]);
                        AddPartyIdentification(partyId, npi, Enumerations.PartyIdentificationNumberTypeId.NPI);

                        //////////////////////////////////////
                        // Person SSN
                        //////////////////////////////////////
                        AddPartyTax(partyId, Methods.GetStringValue(providerInfo["SSN"])
                            , Enumerations.TaxIdTypeId.SSN);

                        //////////////////////////////////////
                        // Person Attest Id
                        //////////////////////////////////////
                        AddPartyIdentification(partyId, Methods.GetStringValue(providerInfo["ProviderAttestID"])
                            , Enumerations.PartyIdentificationNumberTypeId.AttestId);

                        //////////////////////////////////////
                        // Person UPIN
                        //////////////////////////////////////
                        AddPartyIdentification(partyId, Methods.GetStringValue(providerInfo["UPIN"])
                            , Enumerations.PartyIdentificationNumberTypeId.UPIN);

                        //////////////////////////////////////
                        // CAQH Provider Id
                        //////////////////////////////////////
                        caqhProviderId = Methods.GetStringValue(providerInfo["CAQHProviderID"]);
                        AddPartyIdentification(partyId, caqhProviderId
                            , Enumerations.PartyIdentificationNumberTypeId.CAQHId);

                        //////////////////////////////////////
                        // Person Attest Date
                        //////////////////////////////////////
                        DateTime attestDate = Convert.ToDateTime(Methods.GetDateTimeValue(providerInfo["AttestDate"], false));
                        AddCommunicationEvent(partyId, Enumerations.CommunicationEventTypeId.AttestDate, attestDate, attestDate.ToString());

                        //////////////////////////////////////
                        // MMIS Provider Id
                        //////////////////////////////////////
                        mmisProviderId = SelectInitialRosterProviderId(partyId);

                        //////////////////////////////////////
                        // Base Medicaid Id
                        //////////////////////////////////////
                        string baseMedi = string.Empty;
                        try
                        {
                            // generated by sp_Admin_StoredProcBuilder on Nov  6 2013 10:50AM
                            // create parameters objects and fill with values
                            List<SqlParameter> parameters = new List<SqlParameter>();

                            parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));

                            baseMedi = DataAccess.ExecuteScalar("usp_SelectMMIS_GROUP_BASE_MEDICAID_ID", parameters);

                            if (baseMedi.Length > 0) AddMedicaidId(partyId, -1, baseMedi);
                        }
                        catch (Exception ex)
                        {
                            CoreException.ThrowException(ex);
                        }

                        //////////////////////////////////////
                        // Misc Provider Flags              //
                        //////////////////////////////////////
                        practiceState = Methods.GetStringValue(providerInfo["PrimaryPracticeState"]);
                        AddProviderPropertyType(partyId, Enumerations.ProviderPropertyTypeId.PrimaryPracticeState
                            , practiceState);
                        AddProviderPropertyType(partyId, Enumerations.ProviderPropertyTypeId.HospitalBasedFlag
                            , Methods.GetBoolFromCAQHAffirmed(Methods.GetStringValue(providerInfo["HospitalBasedFlag"])).ToString());
                        AddProviderPropertyType(partyId, Enumerations.ProviderPropertyTypeId.HospitalPrivilegeFlag
                            , Methods.GetBoolFromCAQHAffirmed(Methods.GetStringValue(providerInfo["HospitalPrivilegeFlag"])).ToString());
                        AddProviderPropertyType(partyId, Enumerations.ProviderPropertyTypeId.MedicareProviderFlag
                            , Methods.GetBoolFromCAQHAffirmed(Methods.GetStringValue(providerInfo["MedicaidProviderFlag"])).ToString());

                        // get the provider type
                        providerType = Methods.GetStringValue(providerInfo["ProviderTypeAbbreviation"]);

                        //////////////////////////////////////
                        // Disclosure Flag                  //
                        //////////////////////////////////////
                        foreach (DataRow disclosure in providerInfo.GetChildRows(dbrProvider2Disclosure))
                        {
                            string affirmed = Methods.GetBoolFromCAQHAffirmed(Methods.GetStringValue(disclosure["DisclosureAnswerFlag"])).ToString().ToLower();

                            // if the disclosure is affirmed
                            if (affirmed == Boolean.TrueString.ToLower())
                            {
                                // insert the disclosure as a communication event
                                AddDisclosure(partyId, Methods.GetStringValue(disclosure["DisclosureSummary"]), attestDate, affirmed.ToString().ToLower());
                            }
                        }

                        //////////////////////////////////////
                        // Provider Hospital                //
                        //////////////////////////////////////
                        foreach (DataRow hospital in providerInfo.GetChildRows(dbrProvider2Hospital))
                        {
                            string affiliationType = Methods.GetStringValue(hospital["HospitalAffiliationTypeDescription"]);
                           
                            // if the hospital is the primary
                            if (affiliationType == Constants.CAQHPrimary)
                            {
                                string hospitalName = Methods.GetStringValue(hospital["HospitalName"]);

                                // if the hospital name is blank
                                if (hospitalName.Trim().Length == 0)
                                {
                                    hospitalName = defaultBoardName;
                                }

                                // save the hospital affilation to person
                                int hospitalPartyid = AddPersonOrganization(partyId, Enumerations.OrganizationTypeId.Hospital
                                    , hospitalName);

                                // save hospital detail information
                                AddAddress(hospitalPartyid, Enumerations.ContactMechanismRoleTypeId.MailTo, hospitalName
                                    , -1, Methods.GetStringValue(hospital["Address"])
                                    , Methods.GetStringValue(hospital["Address2"])
                                    , Methods.GetStringValue(hospital["City"])
                                    , Methods.GetStringValue(hospital["State"])
                                    , string.Empty
                                    , Methods.GetStringValue(hospital["ZipCode"])
                                    , string.Empty
                                    , Methods.GetStringValue(hospital["PhoneNumber"])
                                    , Methods.GetStringValue(hospital["FaxNumber"])
                                    , Methods.GetStringValue(hospital["EmailAddress"])
                                    , defaultCountry
                                    , partyId
                                    , false);
                            }
                        }   // provider hospital loop

                        //////////////////////////////////////
                        // Provider Credentialing Address   //
                        //////////////////////////////////////
                        foreach (DataRow pa in providerInfo.GetChildRows(dbrProvider2Associate))
                        {
                            string associateType = Methods.GetStringValue(pa["AssociateTypeDescription"]);

                            // if the credentialling associate
                            if (associateType == Constants.CAQHAssociateType.Credentialing)
                            {
                                // save credentialing information
                                AddAddress(partyId, Enumerations.ContactMechanismRoleTypeId.Credentialing
                                    , Methods.GetStringValue(pa["AssociateFirstName"]) + " " + Methods.GetStringValue(pa["AssociateLastName"])
                                    , -1, Methods.GetStringValue(pa["Address"])
                                    , Methods.GetStringValue(pa["Address2"])
                                    , Methods.GetStringValue(pa["City"])
                                    , Methods.GetStringValue(pa["State"])
                                    , string.Empty
                                    , Methods.GetStringValue(pa["PostalCode"])
                                    , string.Empty
                                    , Methods.GetStringValue(pa["PhoneNumber"])
                                    , Methods.GetStringValue(pa["FaxNumber"])
                                    , Methods.GetStringValue(pa["EmailAddress"])
                                    , defaultCountry);
                            }
                        }   // provider credentialing loop
                    
                        //////////////////////////////////////
                        // Provider Practice                //
                        //////////////////////////////////////
                        foreach (DataRow pp in providerInfo.GetChildRows(dbrProvider2Practice))
                        {
                            // save the caqh practice id
                            int practiceId = AddCAQHPracticeId(partyId, Methods.GetIntValue(pp["XmlId"]));

                            //////////////////////////////////////
                            // Medicaid Id                      //
                            //////////////////////////////////////
                            string medicaidPk = AddMedicaidId(partyId, practiceId);
                            string practiceName = string.IsNullOrEmpty(Methods.GetStringValue(pp["PracticeName"])) ? defaultPracticeName : 
                                Methods.GetStringValue(pp["PracticeName"]);
                            // save servicing information
                            AddAddress(partyId, Enumerations.ContactMechanismRoleTypeId.Servicing
                                , practiceName
                                , practiceId, Methods.GetStringValue(pp["Address"])
                                , Methods.GetStringValue(pp["Address2"])
                                , Methods.GetStringValue(pp["City"])
                                , Methods.GetStringValue(pp["State"])
                                , Methods.GetStringValue(pp["County"])
                                , Methods.GetStringValue(pp["Zip"])
                                , Methods.GetStringValue(pp["ExtZip"])
                                , Methods.GetStringValue(pp["PhoneNumber"])
                                , Methods.GetStringValue(pp["FaxNumber"])
                                , Methods.GetStringValue(pp["EmailAddress"])
                                , defaultCountry
                                , 0
                                , false);

                            // loop over all associate addresses which contain all needed addresses
                            foreach (DataRow ppa in pp.GetChildRows(dbrPractice2Associates))
                            {
                               
                                string firstName = Methods.GetStringValue(ppa["AssociateFirstName"]);
                                string lastName = Methods.GetStringValue(ppa["AssociateLastName"]);
                                string practiceAssocType = Methods.GetStringValue(ppa["AssociateTypeDescription"]);

                                //////////////////////////////////////
                                //  Office Manager                  //
                                //////////////////////////////////////
                                if (practiceAssocType == Constants.CAQHAssociateType.OfficeMgr)
                                {
                                    // save the associate name
                                    if (firstName.Length > 0 && lastName.Length > 0)
                                    {
                                        AddPracticeAssociate(medicaidPk, Enumerations.PartyRoleTypeId.OfficeMgr, firstName, lastName);
                                    }
                                }

                                //////////////////////////////////////
                                // Person Address                   //
                                //  PDMS - Pay To                   //
                                //  CAQH - Payment Contact          //
                                //      If empty, Practice Address  //
                                //////////////////////////////////////
                                if (practiceAssocType == Constants.CAQHAssociateType.PayTo)
                                {
                                    //////////////////////////////////////
                                    //  Check Payable To                //
                                    //////////////////////////////////////
                                    string name = Methods.GetStringValue(ppa["CheckPayableTo"]).Trim();
                                    if (name.Length > 0)
                                    {
                                        AddPracticeOrganization(partyId, medicaidPk
                                            , Enumerations.PartyRoleTypeId.CheckPayableTo, name);
                                    }
                                    else
                                    {
                                        name = Methods.GetStringValue(pp["PracticeName"]);
                                    }

                                    // save Pay To information
                                    AddAddress(partyId, Enumerations.ContactMechanismRoleTypeId.PayTo
                                        , name
                                        , practiceId, Methods.GetStringValue(ppa["Address"])
                                        , Methods.GetStringValue(ppa["Address2"])
                                        , Methods.GetStringValue(ppa["City"])
                                        , Methods.GetStringValue(ppa["State"])
                                        , Methods.GetStringValue(ppa["County"])
                                        , Methods.GetStringValue(ppa["Zip"])
                                        , string.Empty
                                        , Methods.GetStringValue(ppa["PhoneNumber"])
                                        , Methods.GetStringValue(ppa["FaxNumber"])
                                        , Methods.GetStringValue(ppa["EmailAddress"])
                                        , defaultCountry
                                        , 0
                                        , false);
                                }

                                //////////////////////////////////////
                                // Person Address                   //
                                //  PDMS - Mail To                  //
                                //  CAQH - Billing Contact          //
                                //      If empty, Practice Address  //
                                //////////////////////////////////////
                                if (practiceAssocType == Constants.CAQHAssociateType.MailTo)
                                {
                                    // save the associate name
                                    if (firstName.Length > 0 && lastName.Length > 0)
                                    {
                                        AddPracticeAssociate(medicaidPk, Enumerations.PartyRoleTypeId.BillingContact, firstName, lastName);
                                    }

                                    // save Pay To information
                                    AddAddress(partyId, Enumerations.ContactMechanismRoleTypeId.MailTo
                                        , firstName + " " + lastName
                                        , practiceId, Methods.GetStringValue(ppa["Address"])
                                        , Methods.GetStringValue(ppa["Address2"])
                                        , Methods.GetStringValue(ppa["City"])
                                        , Methods.GetStringValue(ppa["State"])
                                        , Methods.GetStringValue(ppa["County"])
                                        , Methods.GetStringValue(ppa["Zip"])
                                        , string.Empty
                                        , Methods.GetStringValue(ppa["PhoneNumber"])
                                        , Methods.GetStringValue(ppa["FaxNumber"])
                                        , Methods.GetStringValue(ppa["EmailAddress"])
                                        , defaultCountry
                                        , 0
                                        , false);
                                }
                            }   // practice associates loop


                            // loop over all tax elements
                            foreach (DataRow tax in pp.GetChildRows(dbrPractice2Tax))
                            {
                                bool isGroup = true;
                                string taxType = Methods.GetStringValue(tax["TaxTypeDescription"]);
                                string groupName = Methods.GetStringValue(tax["GroupName"]);
                                string taxId = Methods.GetStringValue(tax["TaxID"]);

                                // if tax type exists
                                if (taxType.Length > 0)
                                {
                                    if (taxType == Constants.CAQHTaxTypeIndividual)
                                    {
                                        isGroup = false;
                                    }
                                }
                                else
                                {
                                    if (groupName.Length == 0)
                                    {
                                        isGroup = false;
                                    }
                                }

                                if (isGroup)
                                {
                                    // if the hospital name is blank
                                    if (groupName.Trim().Length == 0)
                                    {
                                        groupName = defaultBoardName;
                                    }

                                    // save the practice as a group
                                    int orgPartyId = AddPersonOrganization(partyId, Enumerations.OrganizationTypeId.ProviderPracticeGroup, groupName);

                                    // associate the tax id with the group organization
                                    int orgTaxId = AddPartyTax(orgPartyId, taxId, Enumerations.TaxIdTypeId.Group);

                                    // associate the tax id to the practice
                                    AddPracticeTax(practiceId, orgTaxId);
                                }
                                else
                                {
                                    // associate the tax id with the provider (person)
                                    int personTaxId = AddPartyTax(partyId, taxId, Enumerations.TaxIdTypeId.Individual);

                                    // associate the tax id to the practice
                                    AddPracticeTax(practiceId, personTaxId);
                                }
                            }   // pract tax loop

                            UpdateAddressName(partyId, practiceId, Enumerations.ContactMechanismRoleTypeId.PayTo);
                        }      // practice loop


                        //////////////////////////////////////
                        // Provider Type                    //
                        //////////////////////////////////////
                        // if a provider type was provided
                        int providerTypeId = 0;
                        string typeAbbrev = string.Empty;

                        providerTypeId = SelectProviderTypeId(providerType);
                        if (providerTypeId != 0)
                        {
                            // attempt to get provider type
                            AddPartyProviderType(partyId, providerType);
                            typeAbbrev = providerType;
                        }

                        int licenseStateCount = 0;
                        // hack due to state defaulting, yes I know...iterations over same records multiple times
                        // loop over provider license numbers
                        foreach (DataRow license in providerInfo.GetChildRows(dbrProvider2License))
                        {
                            bool currentPracticing = Methods.GetBoolean(license["CurrentlyPracticingFlag"]);

                            // if the active license
                            if (currentPracticing == true)
                            {
                                licenseStateCount += 1;
                            }
                        }

                        string validLicenseType = string.Empty;
                        int invalidlicenseCount = 0;
                        int licenseCount = 0;   // Bug 1245 - count all licenses, code changes to not exclude non-TN licenses
                        int licenseId = 0;
                        string licenseTypeId = string.Empty;
                        string licenseNumber = string.Empty;
                        // loop over provider licenses
                        foreach (DataRow license in providerInfo.GetChildRows(dbrProvider2License))
                        {
                            bool currentPracticing = Methods.GetBoolean(license["CurrentlyPracticingFlag"]);

                            // if the active license
                            if (currentPracticing == true || /* For bug 2331, condition 4 */ (currentPracticing == false && (Methods.GetStringValue(license["LicenseStatusDescription"]) == "Active")))
                            {
                                licenseNumber = Methods.GetStringValue(license["LicenseNumber"]).Trim();

                                // if TN license
                                if (!string.IsNullOrWhiteSpace(licenseNumber))
                                {
                                    // No auto-closing on ADDs: CloseError(partyId, Enumerations.ErrorType.P017);

                                    licenseCount += 1;

                                    string state = license["State"].ToString();
                                    if (licenseStateCount == 1 && string.IsNullOrWhiteSpace(state))
                                    {
                                        license["State"] = defaultState;
                                    }

                                    // get the license type id
                                    validLicenseType = Methods.GetStringValue(license["LicenseType"]);

                                    licenseTypeId = SelectLicenseTypeId(validLicenseType);

                                    // if the licensetype is invalid
                                    if (string.IsNullOrWhiteSpace(licenseTypeId))
                                    {
                                        // set to provider type
                                        licenseTypeId = SelectLicenseTypeId(typeAbbrev);
                                        validLicenseType = typeAbbrev;
                                        RecordError(partyId, Enumerations.ErrorType.P045);
                                        invalidlicenseCount += 1;
                                    }
                                    else
                                    {
                                        // No auto-closing on ADDs: CloseError(partyId, Enumerations.ErrorType.P045);
                                    }

                                    // if license not used in the MMIS
                                    bool boolUsed = IsLicenseUsedInMMIS(validLicenseType);
                                    if (boolUsed == false)
                                    {
                                        licenseCount -= 1;
                                        invalidlicenseCount += 1;
                                    }

                                    // if this is valid license and used in the MMIS
                                    if (licenseTypeId.Length > 0 && boolUsed == true)
                                    {
                                        licenseId = AddLicense(partyId, validLicenseType
                                            , licenseNumber
                                            , Methods.GetDateValue(license["IssueDate"])
                                            , Methods.GetDateValue(license["ExpirationDate"])
                                            , Methods.GetStringValue(license["State"]));
                                    }
                                }
                                else
                                {
                                    RecordError(partyId, Enumerations.ErrorType.P017);
                                }
                            }
                        }

                        // if the invalid license type count exist
                        if (invalidlicenseCount > 0 && licenseCount != 1)
                        {
                            RecordError(partyId, Enumerations.ErrorType.P018);
                        }
                        else
                        {
                            // No auto-closing on ADDs: CloseError(partyId, Enumerations.ErrorType.P018);
                        }

                        // perform action based on number of license(s)
                        switch (licenseCount)
                        {
                            case 1: // one license, add to practices
                                AddLicenseToPractices(partyId, licenseId);
                                break;
                            default:
                                break;  // no action
                        }

                        int deaCount = 0;
                        // hack due to state defaulting, yes I know...iterations over same records multiple times
                        // loop over provider DEA numbers
                        foreach (DataRow dea in providerInfo.GetChildRows(dbrProvider2DEA))
                        {
                            deaCount += 1;
                        }
                                
                        int deaId = 0;
                        // loop over provider DEA numbers
                        foreach (DataRow dea in providerInfo.GetChildRows(dbrProvider2DEA))
                        {
                            string deaState = dea["State"].ToString();
                            if (deaCount == 1 && !string.IsNullOrWhiteSpace(deaState))
                            {
                                deaState = defaultState;
                            }
                            string deaNumber = dea["DEANumber"].ToString().Trim();

                            // if the dea number exists
                            if (!string.IsNullOrWhiteSpace(deaNumber))
                            {
                                deaId = AddLicense(partyId, Constants.CAQHLicenseTypeDEA
                                    , deaNumber
                                    , Methods.GetDateValue(dea["IssueDate"])
                                    , Methods.GetDateValue(dea["ExpirationDate"])
                                    , deaState);
                            }
                        }   // dea loop

                        // perform action based on number of TN license(s)
                        switch (deaCount)
                        {
                            case 1: // one license, add to practices
                                // all dea to all service locations/practices
                                AddLicenseToPractices(partyId, deaId);
                                break;
                            default:
                                break;  // no action
                        }
                        //}

                        //////////////////////////////////////
                        // Provider Specialty               //
                        //////////////////////////////////////
                        int specialtyCount = 0;
                        //int partyTaxonomyId = 0;
                        int specialtyTypeId = 0;
                        foreach (DataRow specialty in providerInfo.GetChildRows(dbrProvider2Specialty))
                        {
                            // if the primary specialty
                            if (Methods.GetStringValue(specialty["SpecialtyTypeDescription"]) == Constants.CAQHPrimary)
                            {

                                specialtyTypeId = SelectSpecialtyTypeId(Methods.GetStringValue(specialty["SpecialtyName"])
                                    , typeAbbrev);

                                // if a match found
                                if (specialtyTypeId > 0)
                                {
                                    // No auto-closing on ADDs: CloseError(partyId, Enumerations.ErrorType.P026);

                                    // if a specialty board is not provided
                                    string boardName = Methods.GetStringValue(specialty["SpecialtyBoardName"]);
                                    if (string.IsNullOrWhiteSpace(boardName))
                                    {
                                        boardName = defaultBoardName;
                                    }

                                    AddPartySpecialty(partyId
                                        , Methods.GetIntValue(specialtyTypeId)
                                        , Methods.GetBoolean(specialty["BoardCertifiedFlag"]).ToString()
                                        , Methods.GetDateValue(specialty["CertificationDate"])
                                        , boardName);

                                    //////////////////////////////////////
                                    // Taxonomy                         //
                                    //////////////////////////////////////
                                    // partyTaxonomyId = AddPartyTaxonomy(partyId, specialtyTypeId, providerTypeId);
                                    specialtyCount += 1;
                                }
                                else // else, log no match found
                                {
                                    RecordError(partyId, Enumerations.ErrorType.P026);
                                }
                            }
                        } // specialty loop

                        // perform action based on number of provider specialties
                        switch (specialtyCount)
                        {
                            case 1: // add one specialty and taxonomy to all practices
                                // AddPartySpecialtyToPractices(partyId, specialtyTypeId);
                                //  AddPartyTaxonomyToPractices(partyId, partyTaxonomyId);
                                // No auto-closing on ADDs: CloseError(partyId, Enumerations.ErrorType.P010);
                                break;

                            default: // else (more than one), review required error
                                // RecordError(partyId, Enumerations.ErrorType.P010);
                                break;
                        }

                        // apply the business rules proc
                        ApplyBusinessRules(partyId);

                        // map xml ids
                        SaveXmlIds(caqhProviderId);

                        // Generate an Error if the SUBMIT_ROSTER NPI is not equal to PDMS NPI - ADD
                        NPIComparision(caqhProviderId, true);

                        // apply the MMIS matching
                        UpdateMMISMatching(partyId);

                        // apply the elgibility matching 
                        UpdateEligibilityMatching(partyId);

                        // attempt to resave status in the event of P011,P012 or P013 errors
                        //  bug 2364
                        RecordPDMSStatus(partyId, Constants.PDMSStatusType.PendingMMISSubmission
                            , Enumerations.RecordStatusEnum.Create,"",caqhProviderId);

                        // generate transactions for the providers service locations
                        SaveProviderTransactions(partyId);

                        EmailErrorsNotification(partyId);

                    } // provider loop 
                }   
                else
                {
                    // record the duplicate party id error
                    partyId = Convert.ToInt32(existingPartyId);
                    RecordError(partyId, Enumerations.ErrorType.P038);
                }

            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Record import failure, only a portion of the data for this record was imported", string.Empty, ex.Message, ex.StackTrace);
            }
        }

        private void StandardExtractStagingExtractFilesOnly(string extractDirectory, string extractfile, string caqhProviderId, string action, string extractDayPath)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            
            List<SqlParameter> parameters;
            string providerId = string.Empty;
            string providerPracticeId = string.Empty;
            string pathXML = AppSettings.Get("CAQH-StandardExtractPhysicalPath") + extractDayPath + @"\";

            try
            {
                // set the filename wildcard
                string fileWildcard = "00" + caqhProviderId + "*.xml";

                // load the extract directory object
                DirectoryInfo di = new DirectoryInfo(extractDirectory);

                // iterate over all matching files, this should only be one file
                foreach (FileInfo fi in di.GetFiles(fileWildcard))
                {
                    try
                    {
                        string msg = String.Format(Constants.LogString.ExtractLoadProvider, action, caqhProviderId, fi.Name);
                        log.CreateLogEntry(msg);

                        //StreamReader sr = fi.OpenText();
                        //string providerXml = sr.ReadToEnd();
                        //sr.Dispose();
                        //provider = LoadProviderXml(providerXml);


                        /// Copy XML file to UI physical folder  ///
                        String newFile = pathXML + fi.Name;
                        if (Directory.Exists(pathXML) == false)
                        {
                            Directory.CreateDirectory(pathXML);
                        }
                        if (File.Exists(newFile) == false)
                        {
                            File.Copy(fi.FullName, newFile);
                        }
                        String xmlFileName = extractDayPath + @"/" + fi.Name;



                        //////////////////////////////////////
                        // Person information               //
                        //////////////////////////////////////
                        // generated by sp_Admin_StoredProcBuilder on Oct 15 2012  3:49PM
                        // create parameters objects and fill with values
                        parameters = new List<SqlParameter>();

                        //UNDONE: get the StandardExtractId pk from the 'new' Standard Extract table which lists only the files
                        parameters.Add(SqlParms.CreateParameter("CAQHProviderID", DbType.Int32, caqhProviderId, true));
                        parameters.Add(SqlParms.CreateParameter("ExtractFileName", DbType.String, extractfile, true));
                        parameters.Add(SqlParms.CreateParameter("XMLFileName", DbType.String, xmlFileName, true));
                        providerId = DataAccess.ExecuteScalar("usp_UpdateCAQHS_ProviderXMLFile", parameters);

                    }
                    catch (Exception ex)
                    {
                        if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                        LogErrorRecord(log, "Record import failure, only a portion of the data for this record was imported", caqhProviderId, ex.Message, ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw ex;
            }
        }
        
        private void UpdateEligibilityMatching(int partyId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jan 24 2013  5:15PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_SaveEligibilityMatching", parameters);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "Eligibility Matching", partyId.ToString(), ex.Message);
            }
        }

        private void UpdateMMISMatching(int partyId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jan 24 2013  5:15PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_SavePDMSServiceLocationToMMISMatching", parameters);
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "MMIS Matching", partyId.ToString(), ex.Message);
            }
        }

        private void UpdateAddressName(int partyId, int practiceId, Enumerations.ContactMechanismRoleTypeId cmRoleType)
        {
            // generated by sp_Admin_StoredProcBuilder on Sep  5 2012  5:31PM
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
            parameters.Add(SqlParms.CreateParameter("CAQH_PRACTICE_ID", DbType.Int32, practiceId, true));
            parameters.Add(SqlParms.CreateParameter("CONTACT_MECHANISM_ROLE_TYPE_ID", DbType.Int32, cmRoleType, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

            DataAccess.ExecuteStoredProcedure("sp_UpdateAddressName", parameters);
        }

        private int VerifyProviderExists(string caqhId)
        {
            string returnVal = string.Empty;
            int returnInt = 0;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Jul 20 2012 11:51AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_IDENTIFICATION_NUMBER_TYPE_ID", DbType.Int32, Enumerations.PartyIdentificationNumberTypeId.CAQHId, true));
                parameters.Add(SqlParms.CreateParameter("PARTY_IDENTIFICATION_NUMBER", DbType.String, caqhId, false));

                returnVal = DataAccess.ExecuteStoredProcedure("sp_SelectPartyIdByPartyIdentificationNumber", parameters, SqlDbType.Int).ToString();
                returnInt = Convert.ToInt32(returnVal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnInt;
        }

        #endregion
    }
}
