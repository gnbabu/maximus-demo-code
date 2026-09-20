using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.Controllers.PDMS
{
    public static class RosterController
    {
        // db table constants
        public const string dbtSubmitRoster = "SubmitRoster";
        public const string dbtSubmitRosterGroup = "SubmitRosterGroup";
        public const string dbtReturnRoster = "ReturnRoster";
        public const string dboReturnRosterStatus = "ReturnRosterStatus";
        public const string dboReturnRosterProviderStatus = "ReturnRosterProviderStatus";
        public const string dboReturnRejectReason = "ReturnRejectReason";

        // db relationship constantsd
        public const string dbrSubmitRoster2Group = dbtSubmitRoster + "2" + dbtSubmitRosterGroup;

        // next roster id
        static int dbkSubmitRosterId = 0;

        // db field constants
        public const string dbfSubmitRosterId = dbtSubmitRoster + Constants.id;
        private const string dbfSubmitRosterGroupId = dbtSubmitRosterGroup + Constants.id;
        private const string dbfSRGSubmittedDate = "SubmittedDate";
        private const string dbfReturnRosterId = dbtReturnRoster + Constants.id;

        // db parameter constants
        private const string dbpSubmitRosterId = "@" + dbfSubmitRosterId;
        private const string dbpSubmitRosterGroupId = "@" + dbfSubmitRosterGroupId;
        private const string dbpSRGSubmittedDate = "@" + dbfSRGSubmittedDate;
        private const string dbpReturnRosterId = "@" + dbfReturnRosterId;

        public static DataSet CheckSubmitRoster(string providerNPI, string ssn)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, providerNPI, true));
                parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, ssn, true));
                return DataAccess.ExecuteStoredProcedure("usp_CheckSubmitRoster", parameters, "CheckSubmitRoster");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRosterExceptions()
        {
            try
            {
                const string RosterException = "ROSTER_EXCEPTION";
                const string OnlineForm = "SUBMIT_ROSTER_ONLINE_FORM";
                DataRelation relation;

                DataSet exceptionRoster = new DataSet();
                exceptionRoster = DataAccess.ExecuteStoredProcedure("sp_SelectRosterExceptions", (RosterException + Constants.pluralEnding));

                exceptionRoster.Tables[0].TableName = RosterException;
                exceptionRoster.Tables[1].TableName = OnlineForm;

                relation = new DataRelation(RosterException + "2_1" + OnlineForm, exceptionRoster.Tables[RosterException].Columns[Constants.fFirstName]
                    , exceptionRoster.Tables[OnlineForm].Columns[Constants.fFirstName]);
                exceptionRoster.Relations.Add(relation);

                relation = new DataRelation(RosterException + "2_2" + OnlineForm, exceptionRoster.Tables[RosterException].Columns[Constants.fLastName]
                    , exceptionRoster.Tables[OnlineForm].Columns[Constants.fLastName]);
                exceptionRoster.Relations.Add(relation);

                return exceptionRoster;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectResolvingReasonTypes(int resolvingActionTypeId, int errorTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("resolving_action_type_id", DbType.Int32 , resolvingActionTypeId, false));
                parameters.Add(SqlParms.CreateParameter("error_type_id", DbType.Int32, errorTypeId, false));

                DataSet returnResolvingReasonTypes = new DataSet();
                returnResolvingReasonTypes = DataAccess.ExecuteStoredProcedure("sp_SelectResolvingReasonTypes", parameters, "ResolvingReasonTypes");
                returnResolvingReasonTypes.Tables[0].TableName = "ResolvingReasonTypes";

                return returnResolvingReasonTypes;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectResolvingActionTypes(bool documentsOnly, int partyId, int errorTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DocumentsOnly", DbType.Boolean, documentsOnly, false));
                if (partyId > 0) parameters.Add(SqlParms.CreateParameter("PartyId", DbType.Int32, partyId, false));
                if (errorTypeId > 0) parameters.Add(SqlParms.CreateParameter("ErrorTypeId", DbType.Int32, errorTypeId, false));

                DataSet returnResolvingActionTypes = new DataSet();
                returnResolvingActionTypes = DataAccess.ExecuteStoredProcedure("sp_SelectResolvingActionTypes", parameters, "ResolvingActionTypes");
                returnResolvingActionTypes.Tables[0].TableName = "ResolvingActionTypes";

                return returnResolvingActionTypes;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDocumentTypes()
        {
            try
            {
                DataSet returnDocumentTypes = new DataSet();
                returnDocumentTypes = DataAccess.ExecuteStoredProcedure("sp_SelectDocumentTypes", (dboReturnRejectReason + Constants.pluralEnding));

                returnDocumentTypes.Tables[0].TableName = "DocumentTypes";

                return returnDocumentTypes;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMMISEnrollmentStatuses()
        {
            try
            {
                DataSet returnMMISEnrollmentStatuses = new DataSet();
                returnMMISEnrollmentStatuses = DataAccess.ExecuteStoredProcedure("sp_selectMmisEnrollmentStatusType", 
                    (dboReturnRejectReason + Constants.pluralEnding));
                returnMMISEnrollmentStatuses.Tables[0].TableName = "MMISEnrollmentStatuses";
                return returnMMISEnrollmentStatuses;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectErrorTypes()
        {
            try
            {
                DataSet returnErrorTypes = new DataSet();
                returnErrorTypes = DataAccess.ExecuteStoredProcedure("sp_SelectErrorTypes", (dboReturnRejectReason + Constants.pluralEnding));

                returnErrorTypes.Tables[0].TableName = "ErrorTypes";

                return returnErrorTypes;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectErrorResolvingActions(int errorTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ERROR_TYPE_ID", DbType.Int32, errorTypeId, false));

                DataSet returnResolvingActions = new DataSet();
                returnResolvingActions = DataAccess.ExecuteStoredProcedure("sp_SelectErrorResolvingActions", parameters, (dbtSubmitRoster + Constants.pluralEnding));
                returnResolvingActions.Tables[0].TableName = "ResolvingActions";

                return returnResolvingActions;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectErrorStatusTypes()
        {
            try
            {
                DataSet returnErrorStatusTypes = new DataSet();
                returnErrorStatusTypes = DataAccess.ExecuteStoredProcedure("sp_SelectErrorStatusTypes", (dboReturnRejectReason + Constants.pluralEnding));

                returnErrorStatusTypes.Tables[0].TableName = "ErrorStatusTypes";

                return returnErrorStatusTypes;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectErrorCategoryTypes()
        {
            try
            {
                DataSet returnErrorCategoryTypes = new DataSet();
                returnErrorCategoryTypes = DataAccess.ExecuteStoredProcedure("sp_SelectErrorCategoryTypes", (dboReturnRejectReason + Constants.pluralEnding));

                returnErrorCategoryTypes.Tables[0].TableName = "ErrorCategoryTypes";

                return returnErrorCategoryTypes;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSubmitRoster(int submitRosterId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter(dbfSubmitRosterId, DbType.Int32, submitRosterId, false));

                DataSet submitRosters = new DataSet();
                submitRosters = DataAccess.ExecuteStoredProcedure("usp_SelectSubmitRosterById", parameters, (dbtSubmitRoster + Constants.pluralEnding));

                submitRosters.Tables[0].TableName = dbtSubmitRoster;
                submitRosters.Tables[1].TableName = dbtSubmitRosterGroup;

                DataRelation relation = new DataRelation(dbrSubmitRoster2Group, submitRosters.Tables[dbtSubmitRoster].Columns[dbfSubmitRosterId]
                    , submitRosters.Tables[dbtSubmitRosterGroup].Columns[dbfSubmitRosterId]);

                submitRosters.Relations.Add(relation);

                return submitRosters;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectServiceAddresses(int submitRosterId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter(dbfSubmitRosterId, DbType.Int32, submitRosterId, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectServiceAddresses", parameters, (dbtSubmitRoster + Constants.pluralEnding));
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSubmitRosterByNPI(string providerNPI)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, providerNPI, true));

                DataSet submitRosters = new DataSet();
                submitRosters = DataAccess.ExecuteStoredProcedure("sp_SelectSubmitRosterByNPI", parameters, 
                    (dbtSubmitRoster + Constants.pluralEnding));
                return submitRosters;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMMISMatch(string providerNPI, string ssn)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, providerNPI, true));
                parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, ssn, true));
                return DataAccess.ExecuteStoredProcedure("usp_SelectMMISMatch", parameters, "MMISMatch");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMMISServiceAddressByNPI(string providerNPI)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, providerNPI, true));

                DataSet submitRosters = new DataSet();
                submitRosters = DataAccess.ExecuteStoredProcedure("usp_SelectMMISServiceAddressByNPI", parameters,
                    (dbtSubmitRoster + Constants.pluralEnding));
                return submitRosters;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMMISServiceAddressByValues(string providerID, string npi, string ssn)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (providerID != null) parameters.Add(SqlParms.CreateParameter("ProviderID", DbType.String, providerID, true));
                if (npi != null) parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                if (ssn != null) parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, ssn, true));
                DataSet submitRosters = new DataSet();
                submitRosters = DataAccess.ExecuteStoredProcedure("usp_SelectMMISServiceAddressByValues", parameters,
                    (dbtSubmitRoster + Constants.pluralEnding));
                return submitRosters;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectMMISServiceValues(string providerID, string npi, string ssn)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(providerID)) 
                    parameters.Add(SqlParms.CreateParameter("ProviderID", DbType.String, providerID, true));
                if (!string.IsNullOrEmpty(npi)) 
                    parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                if (!string.IsNullOrEmpty(ssn)) 
                    parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, ssn, true));
                DataSet dt = DataAccess.ExecuteStoredProcedure("usp_SelectMMISServiceValues", parameters,
                    "MMISServiceValues");
                return dt;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectServiceAddressById(int submitRosterId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SubmitRosterId", DbType.Int32, submitRosterId, true));

                DataSet submitRosters = new DataSet();
                submitRosters = DataAccess.ExecuteStoredProcedure("usp_SelectServiceAddressById", parameters,
                    (dbtSubmitRoster + Constants.pluralEnding));
                return submitRosters;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateReturnRosters(DataSet returnRosters)
        {
            string tableName = dbtReturnRoster;

            try
            {
                // loop over all returnRosters
                foreach (DataRow returnRoster in returnRosters.Tables[tableName].Rows)
                {

                    // perform action based on the record status
                    switch ((int)returnRoster[Constants.dbfRecordStatus])
                    {

                        case (int)Enumerations.RecordStatusEnum.Create:

                            // TODO: Add method to create return roster
                            // AddSubmitRoster(returnRoster);
                            break;

                        case (int)Enumerations.RecordStatusEnum.Delete:

                            throw new Exception(String.Format(Constants.CustomExceptionMessages.RecordDeleteException, tableName));

                        case (int)Enumerations.RecordStatusEnum.Update:

                            throw new Exception(String.Format(Constants.CustomExceptionMessages.RecordUpdateException, tableName));

                        default:

                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        private static bool ValueIsNull(DataRow row, string columnName)
        {
            bool rtn = false;
            try
            {
                if (row.IsNull(columnName)) return true;
                if (string.IsNullOrEmpty(row[columnName].ToString())) return true;
                return false;
            }
            catch { }
            return rtn;
        }

        // Save the SubmitRoster Service Addresses
        public static void SubmitServiceAddress(int submitRosterId, DataSet ds)
        {
            try
            {
                DataRow row = ds.Tables[0].Rows[0];
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SUBMIT_ROSTER_PK", DbType.Int32, row, false));
                parameters.Add(SqlParms.CreateParameter("ADDRESS", DbType.String, row, false));
                parameters.Add(SqlParms.CreateParameter("ADDRESS2", DbType.String, row, true));
                parameters.Add(SqlParms.CreateParameter("CITY", DbType.String, row, false));
                parameters.Add(SqlParms.CreateParameter("COUNTY", DbType.String, row, true));
                parameters.Add(SqlParms.CreateParameter("STATE", DbType.String, row, false));
                parameters.Add(SqlParms.CreateParameter("ZIP", DbType.String, row, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, row, true));
                parameters.Add(SqlParms.CreateParameter("GROUP_NAME", DbType.String, row, true));
                parameters.Add(SqlParms.CreateParameter("GROUP_MEDICAID_ID", DbType.String, row, true));
                parameters.Add(SqlParms.CreateParameter("GROUP_NPI", DbType.String, row, true));
                parameters.Add(SqlParms.CreateParameter("GROUP_TAX_ID", DbType.String, row, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, row, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, row, false));
                parameters.Add(SqlParms.CreateParameter("SERVICE_ADDRESS_POP_ID", DbType.Int32, row, true));
                parameters.Add(SqlParms.CreateParameter("ACTION_CODE_TYPE_ID", DbType.Int32, row, false));
                parameters.Add(SqlParms.CreateParameter("EIN", DbType.String, row, true));
                if (!ValueIsNull(row, "SERVICE_ADDRESS_ID"))
                {
                    parameters.Add(SqlParms.CreateParameter("SERVICE_ADDRESS_ID", DbType.Int32, row, true));
                    DataAccess.ExecuteStoredProcedure("updateSERVICE_ADDRESSCustom", parameters);
                }
                else DataAccess.ExecuteStoredProcedure("insertSERVICE_ADDRESS", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int UpdateSubmitRosters(DataSet submitRosters, int pdmsStatusId)
        {
            int rtn = 0;
            try
            {
                // loop over all submitRosters
                foreach (DataRow submitRoster in submitRosters.Tables[dbtSubmitRoster].Rows)
                {

                    // perform action based on the record status
                    switch ((int)submitRoster[Constants.dbfRecordStatus])
                    {

                        case (int)Enumerations.RecordStatusEnum.Create:

                            rtn = AddSubmitRoster(submitRoster, pdmsStatusId);
                            break;

                        case (int)Enumerations.RecordStatusEnum.Delete:

                            throw new Exception(String.Format(Constants.CustomExceptionMessages.RecordDeleteException, dbtSubmitRoster));

                        case (int)Enumerations.RecordStatusEnum.Update:
                            UpdateSubmitRoster(submitRoster, pdmsStatusId);
                            break;

                        default:

                            break;

                    }
                }

                // loop over all submitRosterGroups
                foreach (DataRow submitRosterGroup in submitRosters.Tables[dbtSubmitRosterGroup].Rows)
                {
                    switch ((int)submitRosterGroup[Constants.dbfRecordStatus])
                    {

                        case (int)Enumerations.RecordStatusEnum.Create:

                            AddSubmitRosterGroup(submitRosterGroup);
                            break;

                        case (int)Enumerations.RecordStatusEnum.Delete:

                            throw new Exception(String.Format(Constants.CustomExceptionMessages.RecordDeleteException, dbtSubmitRosterGroup));

                        case (int)Enumerations.RecordStatusEnum.Update:

                            throw new Exception(String.Format(Constants.CustomExceptionMessages.RecordUpdateException, dbtSubmitRosterGroup));

                        default:

                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return rtn;
        }

        public static void AddSubmitRosterExisting(DataRow submitRoster)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, submitRoster, false));
                parameters.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("MIDDLE_NAME", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("SUFFIX", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("BIRTHDATE", DbType.Date, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("PRIMARY_PRACTICE_STATE", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_NPI", DbType.Decimal, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("ADDRESS", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("ADDRESS2", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("CITY", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("STATE", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("ZIP", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("EXT_ZIP", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("PHONE", DbType.Decimal, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("PHONE_EXT", DbType.Decimal, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("EMAIL_ADDRESS", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("LICENSE_NO", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("LICENSE_STATE", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("DEA", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("UPIN", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("CAQH_ID", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("SUBMITTED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, submitRoster, false));
                DataAccess.ExecuteStoredProcedure("insertSUBMIT_ROSTER_MMIS_EXISTING", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        internal static void AddReturnRoster(DataRow returnRoster)
        {

            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("FileName", DbType.String, returnRoster, false));
            parameters.Add(SqlParms.CreateParameter("FileDateTime", DbType.DateTime, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("PlanId", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("AuthorizationFlag", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("StatusId", DbType.Int32, returnRoster, false));
            parameters.Add(SqlParms.CreateParameter("ProviderId", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("MiddleName", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("ProviderType", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("Street1", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("Street2", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("City", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("State", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("Zip5", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("BirthDate", DbType.Date, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("LicenseNumber", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("Upin", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("DEANumber", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("RosterStatus", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("NonResponderFlag", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("PracticeState", DbType.String, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter("AnniversaryDate", DbType.Date, returnRoster, true));
            parameters.Add(SqlParms.CreateParameter(Constants.dbfModDate, DbType.DateTime, returnRoster, false));
            parameters.Add(SqlParms.CreateParameter(Constants.dbfModUser, DbType.Guid, returnRoster, false));

            // execute the update stored procedure with values
            DataAccess.ExecuteStoredProcedure("sp_AddReturnRoster", parameters);
        }

        internal static int AddSubmitRoster(DataRow submitRoster, int pdmsStatusId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, submitRoster, false));
            parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("MiddleName", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("Suffix", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("BirthDate", DbType.Date, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("ProviderType", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("PracticeState", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("NPI", DbType.Decimal, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("Street1", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("Street2", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("City", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("State", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("Zip5", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("Zip4", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("Phone", DbType.Decimal, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("PhoneExt", DbType.Decimal, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("Email", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("LicenseNumber", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("LicenseState", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("DEAId", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("UPIN", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("CAQHId", DbType.String, submitRoster, true));
            parameters.Add(SqlParms.CreateParameter("SubmittedDate", DbType.DateTime, DateTime.Now, true)); // HACK: 2012.07.09 - Added per request of mham 
            parameters.Add(SqlParms.CreateParameter(Constants.dbfModDate, DbType.DateTime, submitRoster, false));
            parameters.Add(SqlParms.CreateParameter(Constants.dbfModUser, DbType.Guid, submitRoster, false));
            dbkSubmitRosterId = Convert.ToInt32(DataAccess.ExecuteStoredProcedure("sp_AddSubmitRoster", parameters, SqlDbType.Int));

            // Provider Status
            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_DATE", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_TYPE_ID", DbType.Int32, Constants.ProviderStatusChangeType.CAQH, true));
            parameters.Add(SqlParms.CreateParameter("SUBMIT_ROSTER_PK", DbType.Int32, dbkSubmitRosterId, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, true));
            int providerStatusId = Convert.ToInt32(DataAccess.ExecuteScalar("insertPROVIDER_STATUS", parameters));

            // PDMS Status
            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_ID", DbType.Int32, providerStatusId, true));
            parameters.Add(SqlParms.CreateParameter("PDMS_STATUS_TYPE_ID", DbType.Int32, pdmsStatusId, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, true));
            DataAccess.ExecuteStoredProcedure("insertPDMS_STATUSCustom", parameters);

            // CAQH Status
            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_ID", DbType.Int32, providerStatusId, true));
            parameters.Add(SqlParms.CreateParameter("CAQH_STATUS_TYPE_ID", DbType.Int32, Constants.CAQHStatusType.NoCAQHResponse, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, true));
            DataAccess.ExecuteStoredProcedure("insertCAQH_STATUSCustom", parameters);

            return dbkSubmitRosterId;
        }

        internal static void AddSubmitRosterGroup(DataRow submitRosterGroup)
        {
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("SubmitRosterId", DbType.Int32, dbkSubmitRosterId, false));
            parameters.Add(SqlParms.CreateParameter("Name", DbType.String, submitRosterGroup, true));
            parameters.Add(SqlParms.CreateParameter("MedicaidId", DbType.Decimal, submitRosterGroup, true));
            parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, submitRosterGroup, true));
            parameters.Add(SqlParms.CreateParameter("TaxId", DbType.Decimal, submitRosterGroup, true));
            parameters.Add(SqlParms.CreateParameter("ReceivedDate", DbType.DateTime, submitRosterGroup, true));
            parameters.Add(SqlParms.CreateParameter("NPPESResult", DbType.String, submitRosterGroup, true));
            parameters.Add(SqlParms.CreateParameter("NPPESCheckDate", DbType.Date, submitRosterGroup, true));
            parameters.Add(SqlParms.CreateParameter(Constants.dbfModDate, DbType.DateTime, submitRosterGroup, false));
            parameters.Add(SqlParms.CreateParameter(Constants.dbfModUser, DbType.Guid, submitRosterGroup, false));

            DataAccess.ExecuteStoredProcedure("sp_AddSubmitRosterGroup", parameters);
        }

        internal static void UpdateSubmitRoster(DataRow submitRoster, int pdmsStatusId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SubmitRosterId", DbType.Int32, submitRoster, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, submitRoster, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("MiddleName", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("Suffix", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("BirthDate", DbType.Date, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("ProviderType", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("PracticeState", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.Decimal, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("Street1", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("Street2", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("City", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("State", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("Zip5", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("Zip4", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("Phone", DbType.Decimal, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("PhoneExt", DbType.Decimal, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("Email", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("LicenseNumber", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("LicenseState", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("DEAId", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("UPIN", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter("CAQHId", DbType.String, submitRoster, true));
                parameters.Add(SqlParms.CreateParameter(Constants.dbfModDate, DbType.DateTime, submitRoster, false));
                parameters.Add(SqlParms.CreateParameter(Constants.dbfModUser, DbType.Guid, submitRoster, false));
                DataAccess.ExecuteStoredProcedure("sp_UpdateSubmitRoster", parameters, SqlDbType.Int);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
