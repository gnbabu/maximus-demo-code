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
    public class MMISSubmitGroups : BaseJob, IJob
    {
#region "Constructors"

        public MMISSubmitGroups(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

#endregion

#region "Class Level Declarations"
#endregion

#region "Public Methods"

    override public void ExecuteJob()
    {
        // Default Job - MMIS Submit New Groups
        this.ExecuteJob(Guid.Parse("88CBE38A-171B-4A20-949F-DE8D0641F903"));
    }

    override public void ExecuteJob(Guid jobId)
    {
        string jobGuid = jobId.ToString().ToUpper();
        int transactionType = 0;

        switch (jobGuid)
        {
            // MMIS Submit Groups - New
            case "88CBE38A-171B-4A20-949F-DE8D0641F903":
                transactionType = Constants.TransactionType.RequestMedicaidIDfromMMIS;
                this.InsertStagingRecords(transactionType);
                this.SubmitGroups(transactionType);

                break;

            // MMIS Submit Groups - Updated
            case "4EC56235-7CEC-439B-9D6A-3FD36D16CB5F":
                transactionType = Constants.TransactionType.SendProviderUpdatestoMMIS;
                this.InsertStagingRecords(transactionType);
                this.SubmitGroups(transactionType);
                break;

            // MMIS Submit Groups - Payment Info
            case "83E95989-6B84-4907-8EF1-2C3C41E25D68":
                transactionType = Constants.TransactionType.SendPaymentInfotoMMIS;
                this.InsertStagingRecords(transactionType);
                this.SubmitGroups(transactionType);
                break;

            // UNDONE: Create Job
            // MMIS Submit Group Affiliations - New/Updated
            case "616B0BC0-FCCC-47F6-9C05-A1C9AD4A55A1":
                transactionType = Constants.TransactionType.SendGroupAffiliationstoMMIS;
                this.InsertStagingRecords(transactionType);
                this.SubmitGroupAffiliations();
                break;

            default:
                throw new NotImplementedException();
        }
    }

    /// <summary>
    ///     Insert records into MMIS_STAGING tables based on values in
    ///         the TRANSACTION_QUEUE table
    /// </summary>
    /// <param name="transactionType">Common.Constants.TransactionType</param>
    public void InsertStagingRecords(int transactionType)
    {
        try
        {
            PopulateMMISStaging(transactionType);
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(this.ThreadId, ex);
        }
    }

    /// <summary>
    ///     Submits groups to the MMIS based on records in the
    ///         MMIS_STAGING tables
    /// </summary>
    /// <param name="transactionType">Common.Constants.TransactionType</param>
    public void SubmitGroups(int transactionType)
    {
        // create log object
        string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        Logging log = new Logging(this.ThreadId, logProcessName);

        string location = string.Empty;
        string obj = string.Empty;
        DataSet ds;
        DateTime now = DateTime.Now;

        try
        {
            MMISShared ms = new MMISShared(this.ThreadId);
            ms.SetInternalProperties(MethodBase.GetCurrentMethod());
            
            // get the staging records
            ds = ms.GetUnsubmittedStagingRecords(transactionType);

            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart, ds.Tables[0].Rows.Count, transactionType));
            
            // loop over records in dataset
            foreach (DataRow groupRow in ds.Tables[0].Rows)
            {
                try
                {
                    string groupPk = string.Empty;
                    string medicaidPk = string.Empty;

                    // create the object to submit and the response
                    mt.ProviderGroupCaqhSubmitRequest request = new mt.ProviderGroupCaqhSubmitRequest();
                    mt.ProviderGroupCaqhSubmitResponse response = new mt.ProviderGroupCaqhSubmitResponse();
                
                    mt.ProviderMembers group = new mt.ProviderMembers();
                    groupPk = Methods.GetStringValue(groupRow[MMISShared.groupStagingPk].ToString());
                    medicaidPk = groupRow["MEDICAID_PK"].ToString();
                    int partyId = Convert.ToInt32(groupRow["PARTY_ID"].ToString());
                    int tqId = Convert.ToInt32(groupRow["TRANSACTION_QUEUE_ID"].ToString());
                    group.pdmsId = groupPk;
                    group.practiceType = Utility.ConvertPartyCategoryTypeToMMISCode(Convert.ToInt32(groupRow["ProviderCategoryTypeID"].ToString()));
                    group.action = groupRow["ACTION"].ToString();
                    group.groupName = groupRow["NAME"].ToString();
                    group.npiId = groupRow["NPI"].ToString();
                    group.taxId = groupRow["TAX_ID_VALUE"].ToString();
                    group.programEligibilityCode = groupRow["MMIS_ELIGIBILITY_CODE"].ToString();
                    group.organizationCode = groupRow["ORGANIZATION_CODE"].ToString();
                    group.providerTypeCode = groupRow["PROVIDER_TYPE"].ToString();
                    group.providerSpecialtyCode = groupRow["SPECIALTY"].ToString();
                    group.outOfState = groupRow["OUT_OF_STATE"].ToString();
                    group.enrollmentStatusCode = groupRow["ENROLLMENT_STATUS_CODE"].ToString();
                    group.taxonomyCode = groupRow["TAXONOMY"].ToString();
                    group.license = groupRow["LICENSE_NUMBER"].ToString();
                    group.licenseTypeCode = groupRow["LICENSE_TYPE"].ToString();
					group.stateLicenceCode = groupRow["LICENSE_STATE"].ToString();
                    group.deaId = groupRow["DEA_NUMBER"].ToString();
                    group.eft = groupRow["EFT_INDICATOR"].ToString();
                    group.edisonVendorNumber = groupRow["EDISON_NUMBER"].ToString();
                    group.edisonVendorLocation = groupRow["EDISON_LOCATION"].ToString();
                    group.edisonVendorSequenceNumber = groupRow["EDISON_SEQUENCE_NUMBER"].ToString();
                    group.medicareId = groupRow["MEDICAID_ID"].ToString();
					group.eight35indicator = groupRow["EIGHT_THIRTY_FIVE_INDICATOR"].ToString();

                    // check all dates for null and set if not null
                    if (!String.IsNullOrWhiteSpace(groupRow["EFFECTIVE_DATE"].ToString()))
                    {
                        group.groupEffectiveDate = Methods.GetDateValue(groupRow["EFFECTIVE_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["TERM_DATE"].ToString()))
                    {
                        group.termDateTime = Methods.GetDateValue(groupRow["TERM_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["LICENSE_EFFECTIVE_DATE"].ToString()))
                    {
                        group.licenseEffectiveDateTime = Methods.GetDateValue(groupRow["LICENSE_EFFECTIVE_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["LICENSE_TERM_DATE"].ToString()))
                    {
                        group.licenseEndDateTime = Methods.GetDateValue(groupRow["LICENSE_TERM_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["DEA_EFFECTIVE_DATE"].ToString()))
                    {
                        group.deaEffectiveDateTime = Methods.GetDateValue(groupRow["DEA_EFFECTIVE_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["DEA_TERM_DATE"].ToString()))
                    {
                        group.deaEndDateTime = Methods.GetDateValue(groupRow["DEA_TERM_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["EDISON_EFFECTIVE_DATE"].ToString()))
                    {
                        group.edisonVendorEffectiveDate = Methods.GetDateValue(groupRow["EDISON_EFFECTIVE_DATE"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["EDISON_TERM_DATE"].ToString()))
                    {
                        group.edisonVendorEndDate = Methods.GetDateValue(groupRow["EDISON_TERM_DATE"].ToString());
                    }
					
                    // populate addresses
                    List<mt.Addresses> addresses = ms.PopulateAddresses(groupRow.GetChildRows(MMISShared.dbrGrps2Addrs));
                    group.addresses = addresses.ToArray<mt.Addresses>();

                    // populate the provider object
                    request.providerGroup = group;

                    // serialize the object to xml and save to filesystem
                    obj = Methods.SerializeObjectToXml(request);
                    Methods.WriteStringToFile(ms.path, obj, groupPk + "Request");

                    // populate the object credentials 
                    request.userId = ms.uid;
                    request.password = ms.pwd;

                    if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                    {
                        // submit to the MMIS
                        mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
                        response = mmis.SubmitProviderGroup(request);
                    
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
                    location = Methods.WriteStringToFile(ms.path, obj, groupPk + "Response");

                    // provider submitted log entry
                    log.CreateLogEntry(String.Format(Constants.LogString.SubmittingMMISRecord, groupPk, response.correlationId, location));

                    // Update TRANSACTION table with Submit Date information
                    // -----------------------------------------------------
                    TransactionController.UpdateTransactionQueue(
                        tqId
                        , now
                        , null
                        , now
                        , Constants.appPDMSDataExchangeUserId);

                    // generated by sp_Admin_StoredProcBuilder on Sep 12 2013 11:07AM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_GROUP_PK", DbType.Int32, groupPk, true));
                    parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, response.transactionId, true));
                    parameters.Add(SqlParms.CreateParameter("CORRELATION_ID", DbType.String, response.correlationId, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                    DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingGroup", parameters);

                    // extract response errors
                    ms.ProcessServiceErrors(groupPk, response.errors);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
                }
            }
            log.CreateLogEntry(Constants.LogString.SubmittingMMISRecordsEnd);
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
        }
    }

    /// <summary>
    ///     Submits groups to the MMIS based on records in the
    ///         MMIS_STAGING tables
    /// </summary>
    /// <param name="transactionType">Common.Constants.TransactionType</param>
    public void SubmitGroupAffiliations()
    {
        // create log object
        string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        Logging log = new Logging(this.ThreadId, logProcessName);

        string location = string.Empty;
        string obj = string.Empty;
        DataSet ds;
        string groupPk = string.Empty;
        string medicaidPk = string.Empty;
        DateTime now = DateTime.Now;

        try
        {
            MMISShared ms = new MMISShared(this.ThreadId);
            ms.SetInternalProperties(MethodBase.GetCurrentMethod());

            // get the staging records
            int transactionType = Constants.TransactionType.SendGroupAffiliationstoMMIS;
            ds = ms.GetUnsubmittedStagingRecords(transactionType);

            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart, ds.Tables[0].Rows.Count, transactionType));

            // loop over records in dataset
            foreach (DataRow groupRow in ds.Tables[0].Rows)
            {
                try
                {
                    // create the object to submit and the response
                    mt.ProviderGroupAffiliationsCaqhSubmitRequest request = new mt.ProviderGroupAffiliationsCaqhSubmitRequest();
                    mt.ProviderGroupAffiliationsCaqhSubmitResponse response = new mt.ProviderGroupAffiliationsCaqhSubmitResponse();

                    mt.ProviderGroups group = new mt.ProviderGroups();
                    // mt.ProviderMembers group = new mt.ProviderMembers();
                    groupPk = Methods.GetStringValue(groupRow[MMISShared.groupStagingPk].ToString());
                    medicaidPk = groupRow["MEDICAID_PK"].ToString();
                    int partyId = Convert.ToInt32(groupRow["PARTY_ID"].ToString());
                    group.pdmsId = groupPk;
                    group.practiceType = Utility.ConvertPartyCategoryTypeToMMISCode(Convert.ToInt32(groupRow["ProviderCategoryTypeID"].ToString()));
                    group.action = groupRow["ACTION"].ToString();
                    group.medicareId = groupRow["MEDICAID_ID"].ToString();
                    group.npiId = groupRow["NPI"].ToString();
                    //  group.groupName = groupRow["NAME"].ToString();
                    //  group.taxId = groupRow["TAX_ID_VALUE"].ToString();
                    //  group.programEligibilityCode = groupRow["MMIS_ELIGIBILITY_CODE"].ToString();
                    //  group.organizationCode = groupRow["ORGANIZATION_CODE"].ToString();
                    //  group.providerTypeCode = groupRow["PROVIDER_TYPE"].ToString();
                    //  group.providerSpecialtyCode = groupRow["SPECIALTY"].ToString();
                    //  group.outOfState = groupRow["OUT_OF_STATE"].ToString();
                    //  group.enrollmentStatusCode = groupRow["ENROLLMENT_STATUS_CODE"].ToString();
                    //  group.taxonomyCode = groupRow["TAXONOMY"].ToString();
                    //  group.license = groupRow["LICENSE_NUMBER"].ToString();
                    //  group.licenseTypeCode = groupRow["LICENSE_TYPE"].ToString();
                    //  group.deaId = groupRow["DEA_NUMBER"].ToString();
                    //  group.eft = groupRow["EFT_INDICATOR"].ToString();
                    //  group.edisonVendorNumber = groupRow["EDISON_NUMBER"].ToString();
                    //  group.edisonVendorLocation = groupRow["EDISON_LOCATION"].ToString();
                    //  group.edisonVendorSequenceNumber = groupRow["EDISON_SEQUENCE_NUMBER"].ToString();

                    /*
                    // check all dates for null and set if not null
                    if (!String.IsNullOrWhiteSpace(groupRow["EFFECTIVE_DATE"].ToString()))
                    {
                        group.groupEffectiveDate = Methods.GetDateValue(groupRow["EFFECTIVE_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["TERM_DATE"].ToString()))
                    {
                        group.termDateTime = Methods.GetDateValue(groupRow["TERM_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["LICENSE_EFFECTIVE_DATE"].ToString()))
                    {
                        group.licenseEffectiveDateTime = Methods.GetDateValue(groupRow["LICENSE_EFFECTIVE_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["LICENSE_TERM_DATE"].ToString()))
                    {
                        group.licenseEndDateTime = Methods.GetDateValue(groupRow["LICENSE_TERM_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["DEA_EFFECTIVE_DATE"].ToString()))
                    {
                        group.deaEffectiveDateTime = Methods.GetDateValue(groupRow["DEA_EFFECTIVE_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["DEA_TERM_DATE"].ToString()))
                    {
                        group.deaEndDateTime = Methods.GetDateValue(groupRow["DEA_TERM_DATE"]);
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["EDISON_EFFECTIVE_DATE"].ToString()))
                    {
                        group.edisonVendorEffectiveDate = Methods.GetDateValue(groupRow["EDISON_EFFECTIVE_DATE"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(groupRow["EDISON_TERM_DATE"].ToString()))
                    {
                        group.edisonVendorEndDate = Methods.GetDateValue(groupRow["EDISON_TERM_DATE"].ToString());
                    }
                    */
                    // populate addresses
                    // List<mt.Addresses> addresses = ms.PopulateAddresses(groupRow.GetChildRows(MMISShared.dbrGrps2Addrs));
                    // group.addresses = addresses.ToArray<mt.Addresses>();

                    // populate affiliations
                    List<mt.Affiliates> affiliates = ms.PopulateAffiliates(groupRow.GetChildRows(MMISShared.dbrGrps2dbtAffs));
                    group.affiliates = affiliates.ToArray<mt.Affiliates>();

                    // populate the provider object
                    request.providerGroupAffiliations = group;

                    // serialize the object to xml and save to filesystem
                    obj = Methods.SerializeObjectToXml(request);
                    Methods.WriteStringToFile(ms.path, obj, groupPk + "Request");

                    // populate the object credentials 
                    request.userId = ms.uid;
                    request.password = ms.pwd;

                    if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                    {
                        // submit to the MMIS
                        mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
                        response = mmis.SubmitProviderGroupAffiliations(request);
                    }
                    else
                    {
                        string testValue = DateTime.Now.ToString("ssfff");
                        response.transactionId = "666" + testValue;
                        response.correlationId = "777" + testValue;
                        List<mt.Error> errorList = new List<mt.Error>();
                        response.errors = errorList.ToArray<mt.Error>();
                    }

                    // serialize the object to xml and save to filesystem
                    obj = Methods.SerializeObjectToXml(response);
                    location = Methods.WriteStringToFile(ms.path, obj, groupPk + "Response");

                    // provider submitted log entry
                    log.CreateLogEntry(String.Format(Constants.LogString.SubmittingMMISRecord, groupPk, response.correlationId, location));

                    // update all transactions for each affiliation
                    foreach (DataRow affiliate in groupRow.GetChildRows(MMISShared.dbrGrps2dbtAffs))
                    {
                        // Update TRANSACTION table with Submit Date information
                        // -----------------------------------------------------
                        TransactionController.UpdateTransactionQueue(
                            Convert.ToInt32(transactionType)
                            , Methods.GetIntValue(affiliate["PARTY_ID"])
                            , Convert.ToInt32(medicaidPk)
                            , now
                            , null
                            , now
                            , Constants.appPDMSDataExchangeUserId);
                    }

                    // generated by sp_Admin_StoredProcBuilder on Sep 12 2013 11:07AM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_GROUP_PK", DbType.Int32, groupPk, true));
                    parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, response.transactionId, true));
                    parameters.Add(SqlParms.CreateParameter("CORRELATION_ID", DbType.String, response.correlationId, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                    DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingGroup", parameters);

                    ms.ProcessServiceErrors(groupPk, response.errors);
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
                }
            }
            log.CreateLogEntry(Constants.LogString.SubmittingMMISRecordsEnd);
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
        }
    }
#endregion

#region "Private Methods"

    private void PopulateMMISStaging(int transactionType)
    {
        string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
            , MethodBase.GetCurrentMethod().Name);

        try
        {
            // generated by sp_Admin_StoredProcBuilder on Sep 11 2013  5:21PM
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, transactionType, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

            DataAccess.ExecuteStoredProcedure("usp_InsertMMISStagingGroups", parameters);
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
        }
    }

#endregion

    }
}
