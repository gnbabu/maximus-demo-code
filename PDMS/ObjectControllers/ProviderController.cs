using MAXIMUS.Core.Libraries;
using MAXIMUS.Services.PDMS;
using Newtonsoft.Json;
using MAXIMUS.Services.PDMS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using static MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.Controllers.PDMS
{
    public static class ProviderController
    {
        public static DataSet SearchProviders(string IdList, string submitRosterIdList, int tableId, int statusID, int ordinal,
            int sortBy, int pageNumber, int rowsPerPage, bool asc)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("IdList", DbType.String, IdList, false));
                if (tableId > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("TableId", DbType.Int32, tableId, false));
                    parameters.Add(SqlParms.CreateParameter("StatusID", DbType.Int32, statusID, false));
                    parameters.Add(SqlParms.CreateParameter("Ordinal", DbType.Int32, ordinal, false));
                }
                parameters.Add(SqlParms.CreateParameter("SubmitRosterIdList", DbType.String, submitRosterIdList, false));
                parameters.Add(SqlParms.CreateParameter("SortBy", DbType.Int32, sortBy, false));
                parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageNumber, false));
                parameters.Add(SqlParms.CreateParameter("RowsPerPage", DbType.Int32, rowsPerPage, false));
                parameters.Add(SqlParms.CreateParameter("ASC", DbType.Boolean, asc, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SearchProvider", parameters, "ProviderSearchResults");

                lookup.Tables[0].TableName = "ProviderSearchResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchProviders(
            string firstName
            , string lastName
            , string groupName
            , string providerId
            , string medicaidId
            , string caqhId
            , string npi
            , string taxId
            , string pdmsStatusId
            , string caqhStatusId
            , string emailAddress
            , string pdmsStatusDate
            , int riskLevelId
            , int sortBy
            , int pageNumber
            , int rowsPerPage
            , bool asc)
        {
            try
            {

                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("GroupName", DbType.String, groupName, false));
                parameters.Add(SqlParms.CreateParameter("ProviderID", DbType.String, providerId, false));
                parameters.Add(SqlParms.CreateParameter("BaseMedicaidID", DbType.String, medicaidId, false));
                parameters.Add(SqlParms.CreateParameter("CAQHID", DbType.String, caqhId, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxId, false));
                parameters.Add(SqlParms.CreateParameter("PDMSStatus", DbType.String, pdmsStatusId, false));
                parameters.Add(SqlParms.CreateParameter("CAQHStatus", DbType.String, caqhStatusId, false));
                parameters.Add(SqlParms.CreateParameter("EmailAddress", DbType.String, emailAddress, false));
                if (!string.IsNullOrEmpty(pdmsStatusDate))
                    parameters.Add(SqlParms.CreateParameter("PDMSStatusDate", DbType.String, pdmsStatusDate, false));
                parameters.Add(SqlParms.CreateParameter("RiskLevelId", DbType.Int32, riskLevelId, false));
                parameters.Add(SqlParms.CreateParameter("SortBy", DbType.Int32, sortBy, false));
                parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageNumber, false));
                parameters.Add(SqlParms.CreateParameter("RowsPerPage", DbType.Int32, rowsPerPage, false));
                parameters.Add(SqlParms.CreateParameter("ASC", DbType.Boolean, asc, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SearchProvider", parameters, "ProviderSearchResults");

                lookup.Tables[0].TableName = "ProviderSearchResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }



        public static DataSet SearchGroupProviders(string registrationIdList, int tableId, int statusId, int ordinal, bool isAssigned,
            string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            DataSet ds = new DataSet();
            string outValue;
            totalResultCount = 0;

            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegistrationIdList", DbType.String, registrationIdList, false));
                if (tableId > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("TableId", DbType.Int32, tableId, false));
                    parameters.Add(SqlParms.CreateParameter("StatusId", DbType.Int32, statusId, false));
                    parameters.Add(SqlParms.CreateParameter("Ordinal", DbType.Int32, ordinal, false));
                    parameters.Add(SqlParms.CreateParameter("IsAssigned", DbType.Boolean, isAssigned, false));
                }
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, false));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.Int32, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SearchGroupProvider", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                ds.Tables[0].TableName = "ProviderSearchResults";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchGroupProviders(Dictionary<string, object> parms, out int totalResultCount)
        {
            DataSet ds = new DataSet();
            string outValue;
            totalResultCount = 0;

            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, object> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                ds = DataAccess.ExecuteStoredProcedure("usp_SearchGroupProvider", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                ds.Tables[0].TableName = "ProviderSearchResults";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        //     public static DataSet SelectRegProvidersFromListOfIds(List<int> Ids)
        //     {
        //DataSet ds = new DataSet();

        //     }

        public static DataSet SearchOwnerProviders(string groupName, string firstName, string lastName, string taxId,
            string roleName, string IsUserAssigned, string userName, string sortColumn, int pageSize, int startRowIndex,
            bool getTotalRowCount, out int totalResultCount)
        {
            DataSet ds = new DataSet();
            string outValue;
            totalResultCount = 0;

            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GroupName", DbType.String, groupName, false));
                parameters.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxId, false));
                parameters.Add(SqlParms.CreateParameter("RoleName", DbType.String, roleName, false));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, false));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.Int32, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SearchOwnerProvider", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                ds.Tables[0].TableName = "ProviderSearchResults";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchProvidersPublic(string providerTypeAbbrevList, string specialtyTypeIDList, string lastName, string firstName, string middleInitial,
            string city, string state, string zip, string OrgName, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            DataSet ds = new DataSet();
            string outValue;
            totalResultCount = 0;

            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderTypeIDsDelimited", DbType.String, providerTypeAbbrevList, false));
                parameters.Add(SqlParms.CreateParameter("SpecialtyTypeIDsDelimited", DbType.String, specialtyTypeIDList, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("MiddleInitial", DbType.String, middleInitial, false));
                parameters.Add(SqlParms.CreateParameter("City", DbType.String, city, false));
                parameters.Add(SqlParms.CreateParameter("State", DbType.String, state, false));
                parameters.Add(SqlParms.CreateParameter("Zip", DbType.String, zip, false));
                //parameters.Add(SqlParms.CreateParameter("OrgName", DbType.String, OrgName, false));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, false));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.Int32, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SearchProviderPublic2", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                ds.Tables[0].TableName = "ProviderSearchResults";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchProvidersPublicNew(Dictionary<string, object> parms, out int totalResultCount)
        {
            DataSet ds = new DataSet();
            string outValue;
            totalResultCount = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, object> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                // create parameters objects and fill with values
                ds = DataAccess.ExecuteStoredProcedure("usp_SearchProviderPublicNew", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                ds.Tables[0].TableName = "ProviderSearchResults";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchProvidersGIS(string providerTypeAbbr, int pageSize)
        {
            DataSet ds = new DataSet();

            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MMIS_PROVIDER_TYPE_ID", DbType.String, providerTypeAbbr, false));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.Int32, pageSize, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SearchProviderGIS", parameters, "ProviderGISSearch");
                ds.Tables[0].TableName = "ProviderSearch";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchServiceLocations(string firstName, string lastName, string providerId, string medicaidId, string caqhId,
            string npi, string taxId, int pageNumber, int rowsPerPage)
        {
            try
            {

                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("ProviderID", DbType.String, providerId, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidId, false));
                parameters.Add(SqlParms.CreateParameter("CAQHID", DbType.String, caqhId, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxId, false));
                parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageNumber, false));
                parameters.Add(SqlParms.CreateParameter("RowsPerPage", DbType.Int32, rowsPerPage, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SearchServiceLocation", parameters, "LocationSearchResults");
                if (lookup.Tables.Count > 0) lookup.Tables[0].TableName = "LocationSearchResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProvider_NPI_TaxID(string npi, string taxId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxId, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProvider_NPI_TaxID", parameters, "ProviderResults");
                lookup.Tables[0].TableName = "ProviderResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        private static DataTable GetSample(DataTable dtOriginal)
        {
            int idx = 0;
            DataTable dt = dtOriginal.Clone();
            foreach (DataRow row in dtOriginal.Rows)
            {
                idx += 1;
                if (idx > 25) break;
                DataRow newRow = dt.NewRow();
                newRow.ItemArray = row.ItemArray;
                dt.Rows.Add(newRow);
            }
            return dt;
        }

        public static DataSet SearchErrors(string firstName, string lastName, string providerId, string medicaidId, string caqhId,
            string npi, string errorCategoryId, string errorStatusId, string errorTypeId, string hardSoft, string errorDate, int sortBy,
            int pageNumber, int rowsPerPage, bool asc)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("ProviderID", DbType.String, providerId, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidId, false));
                parameters.Add(SqlParms.CreateParameter("CAQHID", DbType.String, caqhId, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("ErrorCategoryId", DbType.String, errorCategoryId, false));
                parameters.Add(SqlParms.CreateParameter("ErrorStatusId", DbType.String, errorStatusId, false));
                parameters.Add(SqlParms.CreateParameter("ErrorTypeId", DbType.String, errorTypeId, false));
                parameters.Add(SqlParms.CreateParameter("HardSoft", DbType.String, hardSoft, false));
                if (!string.IsNullOrEmpty(errorDate))
                    parameters.Add(SqlParms.CreateParameter("ErrorDate", DbType.String, errorDate, false));
                parameters.Add(SqlParms.CreateParameter("SortBy", DbType.Int32, sortBy, false));
                parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageNumber, false));
                parameters.Add(SqlParms.CreateParameter("RowsPerPage", DbType.Int32, rowsPerPage, false));
                parameters.Add(SqlParms.CreateParameter("ASC", DbType.Boolean, asc, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SearchErrors", parameters, "ErrorSearchResults");
                if (lookup.Tables.Count > 0) lookup.Tables[0].TableName = "ErrorSearchResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchErrors(string IdList, string submitRosterIdList, int tableId, int statusID, int ordinal,
            int sortBy, int pageNumber, int rowsPerPage, bool asc)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("IdList", DbType.String, IdList, false));
                if (tableId > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("TableId", DbType.Int32, tableId, false));
                    parameters.Add(SqlParms.CreateParameter("StatusID", DbType.Int32, statusID, false));
                    parameters.Add(SqlParms.CreateParameter("Ordinal", DbType.Int32, ordinal, false));
                }
                parameters.Add(SqlParms.CreateParameter("SubmitRosterIdList", DbType.String, submitRosterIdList, false));
                parameters.Add(SqlParms.CreateParameter("SortBy", DbType.Int32, sortBy, false));
                parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageNumber, false));
                parameters.Add(SqlParms.CreateParameter("RowsPerPage", DbType.Int32, rowsPerPage, false));
                parameters.Add(SqlParms.CreateParameter("ASC", DbType.Boolean, asc, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SearchErrors", parameters, "ErrorSearchResults");
                if (lookup.Tables.Count > 0) lookup.Tables[0].TableName = "ErrorSearchResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDEAByPartyId(int partyId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectDEAByPartyId", parameters, "ProviderResults");
                lookup.Tables[0].TableName = "ProviderResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDocumentsByPartyId(int partyId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDOCUMENT", parameters, "DocumentResults");
                lookup.Tables[0].TableName = "DocumentResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectLicenseByPartyId(int partyId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectLicenseByPartyId", parameters, "ProviderResults");
                lookup.Tables[0].TableName = "ProviderResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSpecialtyByPartyId(int partyId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectSpecialtyByPartyId", parameters, "ProviderResults");
                lookup.Tables[0].TableName = "ProviderResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderFull(int partyId)
        {
            try
            {

                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderFull", parameters, "ProviderResults");

                lookup.Tables[0].TableName = "ProviderResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderContactEmail(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderContactEmail", parameters, "Email");

                lookup.Tables[0].TableName = "Email";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static string SelectProviderContactEmailAddress(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderContactEmail", parameters, "Email");

                lookup.Tables[0].TableName = "Email";

                return lookup.Tables[0].Rows[0].ItemArray[0].ToString().ToLower().Trim();
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetMatchingTaxNpi(string taxId, string npi, int specialtyTypeID)
        {
            try
            {

                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (npi != null)
                {
                    parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                }

                if (taxId != null)
                {
                    parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxId, true));
                }

                if (specialtyTypeID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.String, specialtyTypeID, true));
                }


                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetMatchingTaxNpi", parameters, "MatchingTaxNpi");

                if (lookup.Tables.Count > 0)
                {
                    lookup.Tables[0].TableName = "MatchingTaxNpi";
                }
                else
                {
                    return null;
                }

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetMatchingProvider(string NPI, string MedicaidID, string DD_Facility_num, string DD_Contract_num)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, MedicaidID, false));
                parameters.Add(SqlParms.CreateParameter("DD_Facility_Num", DbType.String, DD_Facility_num, false));
                parameters.Add(SqlParms.CreateParameter("DD_Contract_Num", DbType.String, DD_Contract_num, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetMatchingProvider", parameters, "MatchingData");

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "MatchingProv";
                }
                else
                {
                    return null;
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetAvailableProvider(string NPI, string MedicaidID, string TaxID, string DD_Facility_num, string DD_Contract_num)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, MedicaidID, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, TaxID, false));
                parameters.Add(SqlParms.CreateParameter("DD_Facility_Num", DbType.String, DD_Facility_num, false));
                parameters.Add(SqlParms.CreateParameter("DD_Contract_Num", DbType.String, DD_Contract_num, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetAvailableProviderDetails", parameters, "MatchingData");

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "MatchingProv";
                }
                else
                {
                    return null;
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetMatchingProviderByNPI(string NPI, bool includeWaiverServices)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, false));

                parameters.Add(SqlParms.CreateParameter("IncludeWaiverServices", DbType.Boolean, includeWaiverServices, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetMatchingProviderByNPI", parameters, "MatchingData");

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "MatchingData";
                }
                else
                {
                    return null;
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetMatchingProviderByTaxID(string taxID, bool includeWaiverServices)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("IncludeWaiverServices", DbType.Boolean, includeWaiverServices, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetMatchingProviderByTaxID", parameters, "MatchingData");

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "MatchingData";
                }
                else
                {
                    return null;
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void TerminateProviderGroup(string medId, string taxId, string npi)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medId, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxId, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                DataAccess.ExecuteStoredProcedure("usp_TerminateProviderGroup", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderNotes(int RegId)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_NOTE", parameters, "ProviderNotes");
                lookup.Tables[0].TableName = "ProviderNotes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetCountiesList()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetCounties", "ContiguousZipcode");
                lookup.Tables[0].TableName = "Counties";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectContiguousZipcode(string zipcode)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("zip", DbType.String, zipcode, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectContiguousZipcode", parameters, "ContiguousZipcode");
                lookup.Tables[0].TableName = "ContiguousZipcode";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderNotesByNoteID(int providerNoteID)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNoteID", DbType.Int32, providerNoteID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_NOTE_ByNoteID", parameters, "ProviderNotes");
                lookup.Tables[0].TableName = "ProviderNotes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        //public static DataSet GetContractMaintenance()
        //{
        //    try
        //    {
        //        DataSet lookup = new DataSet();
        //        lookup = DataAccess.ExecuteStoredProcedure("USP_GET_CONTRACT_MAINTENANCE");
        //        lookup.Tables[0].TableName = "REG_Contract";
        //        return lookup;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw CoreException.ThrowException(ex);
        //    }
        //}

        public static DataSet GetContractMaintenanceById(int contractMaintenanceId)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_CONTRACT_ID", DbType.Int32, contractMaintenanceId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ContractsByID", parameters, "ReplicaApps");
                lookup.Tables[0].TableName = "Reg_Contract";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderReplicaApps(int partyId)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_ReplicaApps", parameters, "ReplicaApps");
                lookup.Tables[0].TableName = "ReplicaApps";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderStandardExtracts(int partyId)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_StandardExtracts", parameters, "StandardExtracts");
                lookup.Tables[0].TableName = "StandardExtracts";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Given the PartyID return the MMIS Provider ID
        public static string SelectMMISProviderID(int partyId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("sp_selectMMISProviderID", parameters, "ProviderResults");
                if (!ObjectControllerHelper.HasRows(ds)) return string.Empty;

                return ds.Tables[0].Rows[0]["PARTY_IDENTIFICATION_NUMBER"].ToString();          // 1st Row data
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        private static DataSet SelectProviderCAQHStatus(int partyId)
        {
            try
            {

                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_selectCaqhStatus", parameters, "ProviderResults");
                lookup.Tables[0].TableName = "ProviderResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static NPPESAPIResult ValidNPIinNPPESApi(long npi)
        {
            NPPESAPIResult result = new NPPESAPIResult();
            int outbound_PROV_NPPES_REQ_RES_ID = 0;
            int operation_Type = 1; //insert
            int npiAttemptCount = 0;
            int npiMaxAttemptCount = 2; // only try twice before we go to the back-up mechanism
            bool retrievedNpiData = false;
            const string API_Success_Status = "succeeded";
            const string API_ERROR_Status = "failed";
            string  requestUriString = string.Empty;

            while (npiAttemptCount < npiMaxAttemptCount)
            {
                try
                {
                    npiAttemptCount++;
                    // https://npiregistry.cms.hhs.gov/api/resultsDemo2/?number={0}&enumeration_type={4}&taxonomy_description=&first_name={1}&last_name={2}&organization_name={3}&address_purpose=&city=&state=&postal_code=&country_code=&limit=&skip=&pretty=on
                    string nppesUri = AppSettings.Get("NPI-Registry-URL");
                    requestUriString = string.Format(nppesUri, npi.ToString(), "", "", "", "");
                    int ResponseTimeOut = Convert.ToInt32(AppSettings.Get("IOPResponseTimeOut"));

                    // Make API Call
                    const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                    const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                    ServicePointManager.SecurityProtocol = Tls12;
                    WebRequest request = WebRequest.Create(requestUriString);
                    request.ContentType = "text/json";
                    request.Timeout = ResponseTimeOut;

                    if (outbound_PROV_NPPES_REQ_RES_ID != 0)
                    {
                        operation_Type = 2; //update
                    }
                    else
                    {
                        outbound_PROV_NPPES_REQ_RES_ID = Post_Provider_NPPES_REQ_RES_StatusDetails(outbound_PROV_NPPES_REQ_RES_ID, operation_Type, npi, requestUriString, string.Empty, string.Empty, 0, string.Empty, string.Empty);
                        operation_Type = 2; //update
                    }

                        using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string content = reader.ReadToEnd();
                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(content)))
                            {
                                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(NPPESAPIResult));
                                result = (NPPESAPIResult)serializer.ReadObject(ms);

                                if (content != null)
                                {
                                    retrievedNpiData = true;

                                    Post_Provider_NPPES_REQ_RES_StatusDetails(outbound_PROV_NPPES_REQ_RES_ID, operation_Type, npi, requestUriString, API_Success_Status, content, 0, string.Empty, string.Empty);
                                    break;
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    // we aren't throwing exceptions anymore; we are going to grab the data from our back-up NPI table instead
                    // throw CoreException.ThrowException(ex);
                    int ID = Post_Provider_NPPES_REQ_RES_StatusDetails(outbound_PROV_NPPES_REQ_RES_ID, operation_Type, npi, requestUriString, API_ERROR_Status, string.Empty, 0, string.Empty, ex.ToString());
                    if (outbound_PROV_NPPES_REQ_RES_ID == 0 && ID>0)
                    {
                        outbound_PROV_NPPES_REQ_RES_ID = ID;
                        operation_Type = 2; //update
                    }
                }
            }

            if (!retrievedNpiData)
            {
               // NPI call didn't work; let's go grab it from our local DB

                try
                {
                    string dbResults = string.Empty;
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.Int64, npi, false));

                    DataSet lookup = new DataSet();
                    lookup = DataAccess.ExecuteStoredProcedure("usp_Search_NPI_by_NPI", parameters, "ProviderNPISearchResults");

                    if (lookup.Tables.Count != 0)
                    {
                        // something got returned; need to see if there are actual rows next

                        // right now we only care about a few fields; all fields are returned from PROVIDER_NPI though  in case we want to build up more
                        /*
                        AccountCreation
                            result.result_count
                            result.results[0].enumeration_type (NPI-)
                        NewProvider
                            result.results[0].taxonomies -> desc, code, taxonomy_group
                        OrgInfo
                            result.result_count
                            result.results[0].basic.enumeration_date
                        ProviderAddPresenter - same as accountCreation plus
                           basic.first_name
                           basic.last_name
                           basic.gender 
                        Taxonomies
                            same as newprovider 
                         */

                        List <Result> tempList = new List<Result>(); // use "List" so we can push onto it; convert to array at the end

                        if (ObjectControllerHelper.HasRows(lookup))
                        {
                            // manually create a NPPESAPIResult object to store the SP data lookup
                            result = new NPPESAPIResult();
                            result.result_count = lookup.Tables[0].Rows.Count;

                            foreach (DataRow dr in lookup.Tables[0].Rows)
                            {
                                Result providerResult = new Result();
                                providerResult.enumeration_type = "NPI-" + ObjectControllerHelper.GetString("Entity_Type", dr);

                                providerResult.basic = new Basic();
                                providerResult.basic.first_name = ObjectControllerHelper.GetString("First_Name", dr);
                                providerResult.basic.last_name = ObjectControllerHelper.GetString("Last_Name", dr);
                                providerResult.basic.gender = ObjectControllerHelper.GetString("Gender", dr);
                                providerResult.basic.enumeration_date = Convert.ToDateTime(ObjectControllerHelper.GetString("Enum_Date", dr)).ToString("yyyy-MM-dd");

                                Taxonomy taxonomy = new Taxonomy();
                                taxonomy.desc = ""; // I don't see where to get this; should I join to one of our internal look-up tables?
                                taxonomy.code = ObjectControllerHelper.GetString("Taxonomy_Code", dr);
                                taxonomy.taxonomy_group = ""; // I don't see where to get this; should I join to one of our internal look-up tables?

                                providerResult.taxonomies = new Taxonomy[] { taxonomy };

                                tempList.Add(providerResult);
                            }

                            result.results = tempList.ToArray();
                            dbResults = JsonConvert.SerializeObject(result);
                        }
                        else
                        {
                            dbResults = "No records found";
                        }
                        Post_Provider_NPPES_REQ_RES_StatusDetails(outbound_PROV_NPPES_REQ_RES_ID, operation_Type, npi, null, API_ERROR_Status, string.Empty, 1, dbResults, null);
                    }
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(ex);
                }

                // notify the team via email that the NPI API is having issues
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                string recipientList = AppSettings.Get("NpiApiIsDownNotificationList", string.Empty);
                NotifyThatNpiApiIsDown(templateActualPath, recipientList, npi);
            }
            return result;
        }

        private static int Post_Provider_NPPES_REQ_RES_StatusDetails(int outbound_PROV_NPPES_REQ_RES_ID, int operation_Type, long npi, string request_Payload, string api_Status
            , string api_Response, int fallback_Used, string dbResults, string error_Details)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("OUTBOUND_PROV_NPPES_REQ_RES_ID", DbType.Int32, outbound_PROV_NPPES_REQ_RES_ID, true));
            parameters.Add(SqlParms.CreateParameter("NPI", DbType.Int64, npi, false));
            parameters.Add(SqlParms.CreateParameter("REQUEST_PAYLOAD", DbType.String, request_Payload, true));
            parameters.Add(SqlParms.CreateParameter("API_STATUS", DbType.String, api_Status, true));
            parameters.Add(SqlParms.CreateParameter("API_RESPONSE", DbType.String, api_Response, true));
            parameters.Add(SqlParms.CreateParameter("FALLBACK_USED", DbType.Int32, fallback_Used, true));
            parameters.Add(SqlParms.CreateParameter("SQL_RESPONSE_DATA", DbType.String, dbResults, true));
            parameters.Add(SqlParms.CreateParameter("ERROR_DETAILS", DbType.String, error_Details, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(appAdminUserId), true));
            parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, operation_Type, false));
            string result = DataAccess.ExecuteScalar("usp_InsertUpdateOUTBOUND_PROV_NPPES_REQ_RES", parameters);

            return string.IsNullOrWhiteSpace(result)?0 :int.Parse(result);
        }

        public static void NotifyThatNpiApiIsDown(string templateActualPath, string recipients, long npi)
        {
            Notification n = new Notification();
            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("CURRENTDATE", DateTime.Now.ToString());
            fields.Add("NPI", npi);
            string subject = "NPI Registry Failure Detected";
            string body = string.Empty;
            EMailNotification notify = new EMailNotification(body, subject, recipients);

            body = notify.SendActualNotification(templateActualPath + "//NotifyThatNpiApiIsDown.txt", fields, true);
        }

        // Return TRUE if the NPI is valid in the NPI database
        public static bool ValidNPI(Int64 npi, string lastName, string firstName, string middleName, string state)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.Int64, npi, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName, false));
                parameters.Add(SqlParms.CreateParameter("MiddleName", DbType.String, middleName, false));
                parameters.Add(SqlParms.CreateParameter("StateName", DbType.String, state, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("NPI", "sp_Verify_NPI", parameters, "ValidNPISet");
                if (lookup.Tables.Count == 0) return false;
                if (lookup.Tables[0].Rows.Count == 0) return false;
                if (lookup.Tables[0].Rows[0][0].ToString() == "1") return true;
                return false;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Return TRUE if the NPI is valid in the NPI database
        public static bool VerifyProviderNPI(Int64 npi)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.Int64, npi, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Verify_NPI", parameters, "ValidNPISet");
                if (lookup.Tables.Count == 0) return false;
                if (lookup.Tables[0].Rows.Count == 0) return false;
                if (Convert.ToInt32(lookup.Tables[0].Rows[0][0].ToString()) >= 1) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchProviderNPI(string npi, string medicaidID, string lastName, string firstName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, npi != "" ? npi : null, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidID!=""?medicaidID : null, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName!=""?lastName:null, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName!=""?firstName:null, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_PRIOR_AUTH_Search_NPI", parameters, "NpiSearch");
                lookup.Tables[0].TableName = "NPI";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchHospiceProviderNPI(string npi, string medicaidID, string lastName, string firstName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, npi != "" ? npi : null, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidID != "" ? medicaidID : null, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName != "" ? lastName : null, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName != "" ? firstName : null, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_HOSPICE_Search_NPI", parameters, "NpiSearch");
                lookup.Tables[0].TableName = "NPI";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        /// <summary>
        /// Used for Claims NPI search as it is different from Prior_Auth NPI Search since claims needs expired NPIs in the search
        /// </summary>
        /// <param name="npi"></param>
        /// <param name="medicaidID"></param>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public static DataSet SearchClaimProviderNPI(string npi, string medicaidID, string lastName, string firstName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, npi != "" ? npi : null, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidID != "" ? medicaidID : null, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastName != "" ? lastName : null, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstName != "" ? firstName : null, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Claim_Search_NPI", parameters, "NpiSearch");
                lookup.Tables[0].TableName = "NPI";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetFacilityTypes(string facilityCode, string facilityDesc)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FacilitytypeCode", DbType.String, facilityCode, false));
                parameters.Add(SqlParms.CreateParameter("FacilityCodeDesc", DbType.String, facilityDesc, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_FACILITY_TYPE_CODE", parameters, "FacilityType");
                lookup.Tables[0].TableName = "NPI";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetFacilityTypeByFacilityCode(string facilityCode, string facilityDesc)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FacilitytypeCode", DbType.String, facilityCode, false));
                parameters.Add(SqlParms.CreateParameter("FacilityCodeDesc", DbType.String, facilityDesc, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_FACILITY_TYPE_CODE", parameters, "FacilityType");
                lookup.Tables[0].TableName = "NPI";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void ValidateServiceLocations(string taxId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GROUP_TAX_ID", DbType.String, taxId, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_ValidateServiceLocations", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet ValidateCPCProviderEligibility(int memberRegID, int convenerRegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("member_reg_id", DbType.Int64, memberRegID, false));
                parameters.Add(SqlParms.CreateParameter("Convener_reg_id", DbType.Int64, convenerRegID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_ValidateCPCMemberEligibility", parameters, "ValidateCPCProvider");
                return lookup;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static DataSet ValidateCPCPracticePartnersUploadControl(int regID, int pageTypeID, string sectionName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Reg_Page_Type_Id", DbType.Int64, pageTypeID, false));
                parameters.Add(SqlParms.CreateParameter("Reg_Page_Section", DbType.String, sectionName, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int16, regID, true));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Validate_CPC_UploadControlDocument", parameters, "ValidateCPCUploadControl");
                return lookup;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static DataSet GetCPCandCMCdtlsForRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int16, regID, true));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetProgramData_WorkflowSteps", parameters, "ValidateCPCUploadControl");
                return lookup;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool ValidateIsPrimarySpeciality(int regID, int specialityID)
        {
            //bool status = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("regID", DbType.Int64, regID, true));
                parameters.Add(SqlParms.CreateParameter("specialityID", DbType.Int64, specialityID, true));

                string result = DataAccess.ExecuteScalar("usp_ValidateIsPrimarySpeciality", parameters);
                return result == "True";
            }

            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static DataSet Search_NPPES_EntityType(string NPI)
        {
            try
            {
                int npiValue;
                int.TryParse(NPI.Trim(), out npiValue);

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.Int64, npiValue, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("NPI", "usp_Search_NPPES_EntityType", parameters, "NPI");

                lookup.Tables[0].TableName = "NPI";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool GetValidateNPI(string npi, int regID = 0)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("npi", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                string result = DataAccess.ExecuteScalar("usp_ValidateNPIExist", parameters);

                return result == "True";
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int GetOverrideReasonTypeId(int resolvingReasonTypeId)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ResolvingReasonTypeID", DbType.Int32, resolvingReasonTypeId, true));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("sp_SelectOverrideReasonTypeID", parameters, "ReasonTypeIdResults");
                return Convert.ToInt32(lookup.Tables[0].Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int GetAdminPartyId()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string rtn = DataAccess.ExecuteScalar("sp_SelectAdminPartyID", parameters);
            if (string.IsNullOrEmpty(rtn)) return 0;
            return Convert.ToInt32(rtn);
        }

        public static void SaveMMISData(bool isGroup, int medicaidPK, string medicaidId, string groupMedicaidId, string groupNPI, DateTime? changedDate,
            string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, false));
                parameters.Add(SqlParms.CreateParameter("GROUP_MEDICAID_ID", DbType.String, groupMedicaidId, false));
                parameters.Add(SqlParms.CreateParameter("GROUP_NPI", DbType.String, groupNPI, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_SaveMMISData", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveMMISDataOptional(bool isGroup, int medicaidPK, string medicaidId, string groupMedicaidId, string groupNPI,
            DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("IsGroup", DbType.Boolean, isGroup, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                if (!string.IsNullOrEmpty(medicaidId)) parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, true));
                if (!string.IsNullOrEmpty(groupMedicaidId))
                    parameters.Add(SqlParms.CreateParameter("GROUP_MEDICAID_ID", DbType.String, groupMedicaidId, false));
                if (!string.IsNullOrEmpty(groupNPI)) parameters.Add(SqlParms.CreateParameter("GROUP_NPI", DbType.String, groupNPI, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_SaveMMISData", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SavePartyStatus(int partyId, int providerStatusChangeTypeId, int pdmsStatusTypeId, int? medicaidPK, DateTime? changedDate,
            string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_DATE", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_TYPE_ID", DbType.Int32, providerStatusChangeTypeId, true));
                parameters.Add(SqlParms.CreateParameter("PDMS_STATUS_TYPE_ID", DbType.Int32, pdmsStatusTypeId, true));
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                if (medicaidPK != null) parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_SavePartyStatus", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateCAQHStatus(int partyId, int caqhStatusId, string rosterStatus, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("NEW_STATUS_ID", DbType.Int32, caqhStatusId, true));
                parameters.Add(SqlParms.CreateParameter("new_roster_status", DbType.String, rosterStatus, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_DATE", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_updateCaqhStatus", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateMMISStatus(int partyId, int mmisStatusId, int medicaidPK, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("NEW_STATUS_ID", DbType.Int32, mmisStatusId, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_DATE", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_updateMmisStatus", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateMMISServiceLocation(int partyId, int serviceAddressPopID, int? medicaidPK, DateTime? terminated,
            DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("SERVICE_ADDRESS_POP_ID", DbType.Int32, serviceAddressPopID, true));
                if (medicaidPK != null) parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                if (terminated != null) parameters.Add(SqlParms.CreateParameter("TERMINATED", DbType.DateTime, terminated, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateMMISServiceLocation", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // serviceLevel = TRUE if update the status at the Service Location level, otherwise update at the Provider level
        public static void UpdatePDMSStatus(bool serviceLevel, int errorId, int medicaidPK, int partyId, int pdmsStatusId, DateTime? changedDate, string changedBy)
        {
            if (serviceLevel && medicaidPK == 0) return;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ERROR_ID", DbType.Int32, errorId, true));
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("NEW_STATUS_ID", DbType.Int32, pdmsStatusId, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_DATE", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                if (medicaidPK > 0) parameters.Add(SqlParms.CreateParameter("MedicaidPK", DbType.Int32, medicaidPK, true));
                DataAccess.ExecuteStoredProcedure("sp_updatePdmsStatus", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Update the Provider level status
        public static void UpdatePDMSStatus(int errorId, int partyId, int pdmsStatusId, DateTime? changedDate, string changedBy)
        {
            UpdatePDMSStatus(false, errorId, 0, partyId, pdmsStatusId, changedDate, changedBy);
        }

        private static void AddParameter(ref List<SqlParameter> parms, string columnName, string columnValue)
        {
            if (string.IsNullOrEmpty(columnValue)) return;

            parms.Add(SqlParms.CreateParameter(columnName, DbType.String, columnValue, true));
        }

        // Update the PDMS Data for the Administrator
        public static void UpdatePDMSDataAdmin(int partyId, string providerID,
            string licenseNumber, string licenseState, string licenseEffDate, string licenseEndDate,
            string deaNumber, string deaState, string deaEffDate, string deaEndDate,
            string providerTypeID, string providerSpecialtyTypeID, string baseMedicaidID,
            string emailAddress, string credAddress1, string credAddress2, string credCity, string credState,
            string credZip, string credExtendedZip, string credPhoneAreaCode, string credPhoneNumber,
            string credFaxAreaCode, string credFaxNumber, string licenseTypeID,
            DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                if (providerID != null) parameters.Add(SqlParms.CreateParameter("ProviderID", DbType.String, providerID, true));
                if (licenseNumber != null)
                {
                    parameters.Add(SqlParms.CreateParameter("LicenseNumber", DbType.String, licenseNumber, true));
                    parameters.Add(SqlParms.CreateParameter("LicenseState", DbType.String, licenseState, true));
                    parameters.Add(SqlParms.CreateParameter("LicenseEffDate", DbType.String, licenseEffDate, true));
                    parameters.Add(SqlParms.CreateParameter("LicenseEndDate", DbType.String, licenseEndDate, true));
                }
                if (deaNumber != null)
                {
                    parameters.Add(SqlParms.CreateParameter("DEANumber", DbType.String, deaNumber, true));
                    parameters.Add(SqlParms.CreateParameter("DEAState", DbType.String, deaState, true));
                    parameters.Add(SqlParms.CreateParameter("DEAEffDate", DbType.String, deaEffDate, true));
                    parameters.Add(SqlParms.CreateParameter("DEAEndDate", DbType.String, deaEndDate, true));
                }
                if (providerTypeID != null) parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.String, providerTypeID, true));
                if (providerSpecialtyTypeID != null) parameters.Add(SqlParms.CreateParameter("ProviderSpecialtyTypeID", DbType.String,
                    providerSpecialtyTypeID, true));
                if (baseMedicaidID != null) parameters.Add(SqlParms.CreateParameter("BaseMedicaidID", DbType.String, baseMedicaidID, true));
                AddParameter(ref parameters, "LicenseTypeID", licenseTypeID);
                AddParameter(ref parameters, "CredentialAddress1", credAddress1);
                AddParameter(ref parameters, "CredentialAddress2", credAddress2);
                AddParameter(ref parameters, "CredentialCity", credCity);
                AddParameter(ref parameters, "CredentialState", credState);
                AddParameter(ref parameters, "CredentialZip", credZip);
                AddParameter(ref parameters, "CredentialExtendedZip", credExtendedZip);
                AddParameter(ref parameters, "CredentialPhoneAreaCode", credPhoneAreaCode);
                AddParameter(ref parameters, "CredentialPhoneNumber", credPhoneNumber);
                AddParameter(ref parameters, "CredentialFaxAreaCode", credFaxAreaCode);
                AddParameter(ref parameters, "CredentialFaxNumber", credFaxNumber);
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdatePDMSDataAdmin", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Update the PDMS Data for the Administrator Service Location
        public static void UpdatePDMSDataAdminSvcLoc(int partyId, string serviceLocationID, string medicaidID,
            string servicingAddressName, string servicingAddress1, string servicingAddress2,
            string servicingCity, string servicingState, string servicingZip, string servicingExtendedZip,
            string servicingCounty, string servicingPhoneAreaCode, string servicingPhoneNumber, string servicingEmailAddress,
            string mailtoAddressName, string mailtoAddress1, string mailtoAddress2,
            string mailtoCity, string mailtoState, string mailtoZip, string mailtoExtendedZip,
            string mailtoPhoneAreaCode, string mailtoPhoneNumber, string mailtoEmailAddress,
            string paytoAddressName, string paytoAddress1, string paytoAddress2,
            string paytoCity, string paytoState, string paytoZip, string paytoExtendedZip,
            string paytoPhoneAreaCode, string paytoPhoneNumber, string paytoEmailAddress,
            DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("ServiceLocationID", DbType.String, serviceLocationID, true));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidID, true));
                AddParameter(ref parameters, "ServicingAddressName", servicingAddressName);
                AddParameter(ref parameters, "ServicingAddress1", servicingAddress1);
                AddParameter(ref parameters, "ServicingAddress2", servicingAddress2);
                AddParameter(ref parameters, "ServicingCity", servicingCity);
                AddParameter(ref parameters, "ServicingState", servicingState);
                AddParameter(ref parameters, "ServicingZip", servicingZip);
                AddParameter(ref parameters, "ServicingExtendedZip", servicingExtendedZip);
                AddParameter(ref parameters, "ServicingCounty", servicingCounty);
                AddParameter(ref parameters, "ServicingPhoneAreaCode", servicingPhoneAreaCode);
                AddParameter(ref parameters, "ServicingPhoneNumber", servicingPhoneNumber);
                AddParameter(ref parameters, "ServicingEmailAddress", servicingEmailAddress);
                AddParameter(ref parameters, "MailToAddressName", mailtoAddressName);
                AddParameter(ref parameters, "MailToAddress1", mailtoAddress1);
                AddParameter(ref parameters, "MailToAddress2", mailtoAddress2);
                AddParameter(ref parameters, "MailToCity", mailtoCity);
                AddParameter(ref parameters, "MailToState", mailtoState);
                AddParameter(ref parameters, "MailToZip", mailtoZip);
                AddParameter(ref parameters, "MailToExtendedZip", mailtoExtendedZip);
                AddParameter(ref parameters, "MailToPhoneAreaCode", mailtoPhoneAreaCode);
                AddParameter(ref parameters, "MailToPhoneNumber", mailtoPhoneNumber);
                AddParameter(ref parameters, "MailToEmailAddress", mailtoEmailAddress);
                AddParameter(ref parameters, "PayToAddressName", paytoAddressName);
                AddParameter(ref parameters, "PayToAddress1", paytoAddress1);
                AddParameter(ref parameters, "PayToAddress2", paytoAddress2);
                AddParameter(ref parameters, "PayToCity", paytoCity);
                AddParameter(ref parameters, "PayToState", paytoState);
                AddParameter(ref parameters, "PayToZip", paytoZip);
                AddParameter(ref parameters, "PayToExtendedZip", paytoExtendedZip);
                AddParameter(ref parameters, "PayToPhoneAreaCode", paytoPhoneAreaCode);
                AddParameter(ref parameters, "PayToPhoneNumber", paytoPhoneNumber);
                AddParameter(ref parameters, "PayToEmailAddress", paytoEmailAddress);
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdatePDMSDataAdmin", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Update the PDMS Data for the Administrator - Registration and PDMS data
        public static void UpdatePDMSDataAdminReg(int regId, string medicaidID, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdatePDMSDataAdminReg", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        // Updates all the PDMS statuses at the Provider level and all Service Locations
        // It DOES NOT check if there are Errors still outstanding.  It sets all statuses to the pdmsStatusId provided.
        public static void UpdatePDMSStatusAll(int partyId, int pdmsStatusId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("NEW_STATUS_ID", DbType.Int32, pdmsStatusId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_updatePdmsStatusAll", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdatePDMSStatusSubmitRoster(int submitRosterId, int pdmsStatusId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SubmitRosterId", DbType.Int32, submitRosterId, true));
                parameters.Add(SqlParms.CreateParameter("NEW_STATUS_ID", DbType.Int32, pdmsStatusId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_UpdatePDMSStatusSubmitRoster", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        private static void SetEmailInfo(string templatesDirectory, ref Dictionary<string, object> fields, ref string templateName,
            ref string subject, DateTime eventDate, string providerName, string npi, int resolvingActionTypeId, int resolvingReasonTypeId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("ResolvingActionTypeId", DbType.Int32, resolvingActionTypeId, true));
            DataSet ds = DataAccess.ExecuteStoredProcedure("sp_SelectResolvingActionDocument", parameters, "Document");
            if (!ObjectControllerHelper.HasRows(ds)) return;
            subject = ObjectControllerHelper.GetString("DOCUMENT_SUBJECT", ds.Tables[0].Rows[0]);
            templateName = ObjectControllerHelper.GetString("DOCUMENT_FILE_NAME", ds.Tables[0].Rows[0]);

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("resolving_action_type_id", DbType.Int32, 0, true));
            parameters.Add(SqlParms.CreateParameter("resolving_reason_type_id", DbType.Int32, resolvingReasonTypeId, true));
            ds = DataAccess.ExecuteStoredProcedure("sp_SelectResolvingReasonTypes", parameters, "Reason");
            if (!ObjectControllerHelper.HasRows(ds)) return;
            string reason = ObjectControllerHelper.GetString("EMAIL_VERBIAGE", ds.Tables[0].Rows[0]);
            if (string.IsNullOrEmpty(reason)) reason = ObjectControllerHelper.GetString("RESOLVING_REASON_TYPE", ds.Tables[0].Rows[0]);
            fields.Add("EVENTDATE", eventDate.ToString("MM/dd/yyyy"));
            fields.Add("REASON", reason);
            fields.Add("PROVIDERNAME", providerName);
            fields.Add("NPI", npi);
            fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
        }

        public static void EmailNotification(string templatesDirectory, string emailFrom, string emailTo,
            int resolvingActionTypeId, int resolvingReasonTypeId, DateTime eventDate, string providerName, string npi, DateTime? changedDate,
            string changedBy, ref int communicationEventId, int regId)
        {
            try
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                string subject = string.Empty;
                string templateName = string.Empty;

                // Send the email
                SetEmailInfo(templatesDirectory, ref fields, ref templateName, ref subject, eventDate, providerName, npi, resolvingActionTypeId,
                    resolvingReasonTypeId);
                EMailNotification notify = new EMailNotification(string.Empty, subject, emailTo);
                string body = notify.SendNotification(templatesDirectory + "\\" + templateName, fields, false);

                // Create the communicaton event and email
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
                string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
                parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, emailTo, false));
                parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
                parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
                parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, templateName, false));
                parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, GetKeyValueString(fields), false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean, notify.isEmailSent, true));
                parameters.Add(SqlParms.CreateParameter("log_message", DbType.String, notify.log_message, true));
                DataAccess.ExecuteStoredProcedure("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                string comEventID = DataAccess.ExecuteScalar("sp_SelectLastCommunicationEventID", parameters);
                if (!string.IsNullOrEmpty(comEventID)) communicationEventId = Convert.ToInt32(comEventID);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        private static string GetKeyValueString(Dictionary<string, object> dict)
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

        public static void UpdateErrorHistory(int errorId, int errorStatusTypeId, int? communicationEventId,
            int resolvingActionTypeId, int? resolvingReasonTypeId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ERROR_ID", DbType.Int32, errorId, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_STATUS_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_STATUS_TYPE_ID", DbType.Int32, errorStatusTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_ID", DbType.Int32, communicationEventId, true));
                parameters.Add(SqlParms.CreateParameter("RESOLVING_ACTION_TYPE_ID", DbType.Int32, resolvingActionTypeId, true));
                parameters.Add(SqlParms.CreateParameter("RESOLVING_REASON_TYPE_ID", DbType.Int32, resolvingReasonTypeId, true));
                DataAccess.ExecuteStoredProcedure("insertPARTY_ERROR_HISTORYCustom", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void TerminateAllServiceLocations(int partyId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("UPDATE_DATE", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_UpdateAllServiceLocationTermDates", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void TerminateOneServiceLocation(int partyId, int medicaidPK, string medicaidId, string cdeEnrollStatus,
            DateTime? changedDate, string changedBy, bool referralProvider)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (medicaidPK > 0 && !string.IsNullOrEmpty(medicaidId))
                {
                    int transactionType = referralProvider ? Constants.TransactionType.SendDIDDAddProviderUpdatetoMMIS : Constants.TransactionType.SendProviderUpdatestoMMIS;

                    // If the MedicaidPK exists we need to write a transaction for MMIS.
                    parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                    parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPK, true));
                    parameters.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, transactionType, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                    DataAccess.ExecuteStoredProcedure("usp_SaveTransaction", parameters);
                }

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("medicaid_pk", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("UPDATE_DATE", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                parameters.Add(SqlParms.CreateParameter("CDE_ENROLL_STATUS", DbType.String, cdeEnrollStatus, true));
                DataAccess.ExecuteStoredProcedure("sp_UpdateOneServiceLocationTermDate", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int GetUpdateRecordOption(int errorTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ErrorTypeId", DbType.Int32, errorTypeId, true));
                return Convert.ToInt32(DataAccess.ExecuteScalar("sp_SelectUpdateRecordOption", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateDEA(int partyId, int medicaidPK, string dea, string changedBy)
        {
            try
            {
                string[] theDEA = dea.Split(',');
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("party_id", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("license_number", DbType.String, theDEA[1], true));
                if (!string.IsNullOrEmpty(theDEA[0]))
                    parameters.Add(SqlParms.CreateParameter("state_abbrev", DbType.String, theDEA[0], true));
                parameters.Add(SqlParms.CreateParameter("medicaid_pk", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("last_modified_user", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_updateDEA_XREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateLicense(int partyId, int medicaidPK, string license, string changedBy)
        {
            try
            {
                string[] theLicense = license.Split(',');
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("party_id", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("license_number", DbType.String, theLicense[1], true));
                if (!string.IsNullOrEmpty(theLicense[0]))
                    parameters.Add(SqlParms.CreateParameter("state_abbrev", DbType.String, theLicense[0], true));
                parameters.Add(SqlParms.CreateParameter("medicaid_pk", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("last_modified_user", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_updateLicenseXREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateProviderID(int partyId, string providerID, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("ProviderID", DbType.String, providerID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_UpdateProviderID", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateProvider(int partyId, string medicaidId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, false));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateProvider", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateSpecialty(int partyId, int medicaidPK, int specialtyId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("party_id", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("medicaid_pk", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("specialty_type_id", DbType.Int32, specialtyId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_SavePartyServiceLocationXrefs", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateUPIN(int partyId, int medicaidPK, string upin, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("party_id", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("upin_number", DbType.String, upin, true));
                parameters.Add(SqlParms.CreateParameter("medicaid_pk", DbType.Int32, medicaidPK, true));
                parameters.Add(SqlParms.CreateParameter("last_modified_user", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("sp_updateUPIN_XREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertPartyError(string errorCode, int errorStatusTypeId, int? partyId, int? submitRosterId, int? rosterExceptionId,
            DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, errorCode, true));
                int errorTypeId = Convert.ToInt32(DataAccess.ExecuteScalar("sp_SelectErrorTypeIdByErrorCode", parameters));

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ERROR_TYPE_ID", DbType.Int32, errorTypeId, true));
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("SUBMIT_ROSTER_PK", DbType.Int32, submitRosterId, true));
                parameters.Add(SqlParms.CreateParameter("ROSTER_EXCEPTION_PK", DbType.Int32, rosterExceptionId, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                int errorId = Convert.ToInt32(DataAccess.ExecuteScalar("insertPARTY_ERROR", parameters));

                // Add error history event
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ERROR_ID", DbType.Int32, errorId, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_STATUS_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_STATUS_TYPE_ID", DbType.Int32, errorStatusTypeId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, true));
                DataAccess.ExecuteStoredProcedure("insertPARTY_ERROR_HISTORYCustom", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertDocument(int partyId, string name, string description, string fileName, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, true));
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, description, true));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, fileName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_InsertDOCUMENT", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertProviderNote(int regID, int noteTypeId, string noteText, DateTime noteDate,
            DateTime? changedDate, string changedBy, int errorID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_NOTE_TYPE_ID", DbType.Int32, noteTypeId, true));
                parameters.Add(SqlParms.CreateParameter("NOTE_TEXT", DbType.String, noteText, true));
                parameters.Add(SqlParms.CreateParameter("NOTE_DATE_TIME", DbType.DateTime, noteDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                int providerNoteId = Convert.ToInt32(DataAccess.ExecuteScalar("insertPROVIDER_NOTE", parameters));
                //parameters = new List<SqlParameter>();
                //parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                //parameters.Add(SqlParms.CreateParameter("PROVIDER_NOTE_ID", DbType.Int32, providerNoteId, true));
                //DataAccess.ExecuteStoredProcedure("insertPROVIDER_NOTE_PARTY_XREF", parameters);

                //TODO REGTOLIVE change PARTY_ERROR_HISTORY reference
                if (errorID > 0)
                {
                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("ERROR_ID", DbType.Int32, errorID, true));
                    parameters.Add(SqlParms.CreateParameter("PROVIDER_NOTE_ID", DbType.Int32, providerNoteId, true));
                    DataAccess.ExecuteStoredProcedure("usp_InsertPROVIDER_NOTE_PARTY_ERROR_HISTORY_XREF", parameters);
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertTransactionMonitorNote(int regID, int noteTypeId, string noteText, DateTime noteDate,
    DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_NOTE_TYPE_ID", DbType.Int32, noteTypeId, true));
                parameters.Add(SqlParms.CreateParameter("NOTE_TEXT", DbType.String, noteText, true));
                parameters.Add(SqlParms.CreateParameter("NOTE_DATE_TIME", DbType.DateTime, noteDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("insertTransactionMonitor_NOTE", parameters);
                

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertUserProvGrp_XREF(string userID, string taxID, string npi)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));

                DataAccess.ExecuteStoredProcedure("usp_InsertUserProvGrp_XREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteDocument(int documentID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, false));
                DataAccess.ExecuteStoredProcedure("usp_DeleteDOCUMENT", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetStgProviderById(string regId)
        {
            try
            {

                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetStgProviderById", parameters, "ProviderResults");

                lookup.Tables[0].TableName = "ProviderResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet CheckHistoryChanges_REG_SERVICE_LOCATION(int regId)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_CheckHistoryChanges_REG_SERVICE_LOCATION", parameters, "Changes");
                lookup.Tables[0].TableName = "Changes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet CheckHistoryChanges_REG_PROVIDER(int regId)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_CheckHistoryChanges_REG_PROVIDER", parameters, "Changes");
                lookup.Tables[0].TableName = "Changes";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet CheckProviderRevalidated(int regId)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_IsProviderRevalidationDateRequired", parameters, "RevalidationStatus");
                lookup.Tables[0].TableName = "RevalidationStatus";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectForProviderFile(string npi, string ssn)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, ssn, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectForProviderFile", parameters, "Provider");
                lookup.Tables[0].TableName = "Provider";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectConversionProvider(string userId = null, string userName = null, int regId = 0, string taxId = null, string npi = null)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(userId))
                    parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.String, userId, false));
                if (!string.IsNullOrEmpty(userName))
                    parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, userName, false));
                if (regId != 0)
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId, false));
                if (!string.IsNullOrEmpty(taxId))
                    parameters.Add(SqlParms.CreateParameter("TAX_ID", DbType.String, taxId, false));
                if (!string.IsNullOrEmpty(npi))
                    parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectConversionProvider", parameters, "Provider");
                lookup.Tables[0].TableName = "Provider";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectConversionAffiliation(string regAffiliationId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_AFFILIATION_ID", DbType.Int32, regAffiliationId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectConversionAffiliation", parameters, "ConversionAffiliation");
                lookup.Tables[0].TableName = "ConversionAffiliation";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }

        public static DataSet SelectActionHistoryData(string name, string npi, string taxID, string actionType, string actionStatus, string taskName, string startDate,
     string endDate, string userName, string roleName, int pageNumber, int rowsPerPage, int sortBy, bool isASC)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Name", DbType.String, name, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("ActionType", DbType.String, actionType, false));
                parameters.Add(SqlParms.CreateParameter("ActionStatus", DbType.String, actionStatus, false));
                parameters.Add(SqlParms.CreateParameter("TaskName", DbType.String, taskName, false));
                parameters.Add(SqlParms.CreateParameter("StartDate", DbType.String, startDate, false));
                parameters.Add(SqlParms.CreateParameter("EndDate", DbType.String, endDate, false));
                parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, userName, false));
                parameters.Add(SqlParms.CreateParameter("RoleName", DbType.String, roleName, false));
                parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageNumber, false));
                parameters.Add(SqlParms.CreateParameter("SortBy", DbType.Int32, sortBy, false));
                parameters.Add(SqlParms.CreateParameter("ASC", DbType.Boolean, isASC, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SearchProviderAction", parameters, "ActionHistory");
                ds.Tables[0].TableName = "ActionHistory";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectProviderLicenseByPartyID(int partyID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyID, true));
                DataSet provider = DataAccess.ExecuteStoredProcedure("usp_SelectProvider_LICENSE", parameters, "Licenses");

                return provider;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }

        public static DataSet SelectProviderSpecialtyByPartyID(int partyID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyID, true));
                DataSet provider = DataAccess.ExecuteStoredProcedure("usp_SelectProvider_SPECIALTY", parameters, "Specialties");

                return provider;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectGroupMemberProvidersByAffiliationID(int affiliationID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_AFFILIATION_ID", DbType.Int32, affiliationID, true));
                DataSet provider = DataAccess.ExecuteStoredProcedure("usp_SelectGroupMemberProvidersByAffiliationID", parameters, "Providers");

                return provider;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderTypesByMMISProviderTypeID(string mmisProviderTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MMIS_PROVIDER_TYPE_ID", DbType.String, mmisProviderTypeID, true));
                DataSet providerTypes = DataAccess.ExecuteStoredProcedure("usp_SelectProviderTypesByMMISID", parameters, "ProviderTypes");

                return providerTypes;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetAlertsByTaxID(string taxID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetAlertsByTaxID", parameters, "Alerts");

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "AddlInfo";
                    ds.Tables[1].TableName = "ReEnrollment";
                    ds.Tables[2].TableName = "Renewal";
                }
                else
                {
                    return null;
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectProviderAddressInfo(int regId, int addressTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, addressTypeId, false));
                DataSet addressInfo = DataAccess.ExecuteStoredProcedure("usp_SelectProviderAddressInfo", parameters, "AddressInfoDS");
                addressInfo.Tables[0].TableName = "AddressInfo";
                return addressInfo;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderReturnStatus(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));                
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckRegistrationReturnedBySitevisit", parameters, "StatusInfoDS");             
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProviderAddressLocationInfo(int regId, int sectionTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("SECTION_TYPE_ID", DbType.Int32, sectionTypeId, false));
                DataSet addressInfo = DataAccess.ExecuteStoredProcedure("usp_SelectProviderAddressLocationInfo", parameters, "AddressInfoDS");
                addressInfo.Tables[0].TableName = "AddressInfo";
                return addressInfo;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPowerAgentByUserId(string loggedinUserID, int pageSize, int startRowIndex, string providerAdminUserID,out int totalResultCount)
        {
            try
            {
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PowerAgentUserID", DbType.String, loggedinUserID, false));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("ProviderAdminUserID", DbType.String, providerAdminUserID, false));
                DataSet powerAgents = DataAccess.ExecuteStoredProcedure("usp_GetPowerAgentsByProviderAdmin", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                powerAgents.Tables[0].TableName = "PowerAgents";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }
                return powerAgents;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetProviderAdminsForPowerAgent(string powerAgentUserId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                // Input parameters only (no output parameters)
                parameters.Add(SqlParms.CreateParameter("PowerAgentUserId", DbType.String, powerAgentUserId, false));
                DataSet result = DataAccess.ExecuteStoredProcedure("usp_GetProviderAdminsForPowerAgent", parameters, "getProviderAdminForPowerAgentResult");

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }





        public static DataSet AddPowerAgent(string providerAdminUserId, string OHID, bool accessManagement, string createdByUserId, bool overrideValidation)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                // Input parameters only (no output parameters)
                parameters.Add(SqlParms.CreateParameter("ProviderAdminUserId", DbType.String, providerAdminUserId, false));
                parameters.Add(SqlParms.CreateParameter("OHID", DbType.String, OHID, false));
                parameters.Add(SqlParms.CreateParameter("AccessManagement", DbType.Boolean, accessManagement, false));
                parameters.Add(SqlParms.CreateParameter("CreatedByUser", DbType.String, createdByUserId, false));
                parameters.Add(SqlParms.CreateParameter("OverrideValidation", DbType.Boolean, overrideValidation, false));
                DataSet result = DataAccess.ExecuteStoredProcedure("usp_AddPowerAgent", parameters, "addPowerAgentResult");

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet DeactivatePowerAgent(string createdByUserId, string id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                // Input parameters only (no output parameters)               
                parameters.Add(SqlParms.CreateParameter("CreatedByUser", DbType.String, createdByUserId, false));
                parameters.Add(SqlParms.CreateParameter("Id", DbType.Int32, id, false));

                DataSet result = DataAccess.ExecuteStoredProcedure("usp_DeactivatePowerAgent", parameters, "deactivatePowerAgentResult");

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet UpdateAccessManagementForPowerAgent(string createdByUserId, string id, bool accessManagement)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                // Input parameters only (no output parameters)               
                parameters.Add(SqlParms.CreateParameter("CreatedByUser", DbType.String, createdByUserId, false));
                parameters.Add(SqlParms.CreateParameter("Id", DbType.Int32, id, false));
                parameters.Add(SqlParms.CreateParameter("AccessManagement", DbType.Boolean, accessManagement, false));

                DataSet result = DataAccess.ExecuteStoredProcedure("usp_UpdateAccessManagementForPowerAgent", parameters, "updateAccessManagementPowerAgentResult");

                return result;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegProviderInfo(int regId, int addressTypeId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, addressTypeId, false)); //ADDRESS_TYPE_ID is not used in Stored Proc
                DataSet addressInfo = DataAccess.ExecuteStoredProcedure("usp_SelectRegProviderInfo", parameters, "AddressInfoDS");
                addressInfo.Tables[0].TableName = "AddressInfo";
                return addressInfo;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet TerminateProvider(int regID, DateTime termDate, string changedBy, string enrollmentStatusCode, string enrollmentStatusReason, bool createTrans, bool is_frm_job = false)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, termDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, false));
                parameters.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, createTrans, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, enrollmentStatusCode, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASON_CODE", DbType.String, enrollmentStatusReason, false));
                parameters.Add(SqlParms.CreateParameter("IS_FRM_JOB", DbType.Boolean, is_frm_job, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_TerminateProvider", parameters, "TransactionIds");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void DenyCPCFromProviderReview(int regID, string CPCProgramYear, int ProcessID, DateTime LastModifiedDate, Guid ModifiedBy, int WorkflowEventTypeID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, CPCProgramYear, false));
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, true));
                parameters.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, LastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("USER", DbType.Guid, ModifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("WORKFLOW_EVENT_TYPE_ID", DbType.Int32, WorkflowEventTypeID, false));

                DataAccess.ExecuteStoredProcedure("usp_DenyCPC_FRM_ProviderReview", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet CheckDoDDorODAProvider(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckDODD_ODA_Provider", parameters, "DODDorODA");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchRetrieveReports(Dictionary<string, object> parms, string reportType, out int totalResultCount)
        {
            DataSet ds = new DataSet();
            string outValue;
            totalResultCount = 0;

            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, object> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                if (reportType == "277 Claim Submission Response")
                {
                    ds = DataAccess.ExecuteStoredProcedure("usp_SearchRetrieveReports_277", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                }
                else if (reportType == "278 Prior Auth Submission Response")
                {
                    ds = DataAccess.ExecuteStoredProcedure("usp_SearchRetrieveReports_278", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                }
                else if (reportType == "PASRR Reports")
                {
                    ds = DataAccess.ExecuteStoredProcedure("usp_SearchRetrieveReports_PASRR_Reports", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                }
                else
                {
                    ds = DataAccess.ExecuteStoredProcedure("usp_SearchRetrieveReports", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                }

                ds.Tables[0].TableName = "RetrieveReportsSearchResults";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SearchRAReports(Dictionary<string, object> parms, out int totalResultCount)
        {
            DataSet ds = new DataSet();
            string outValue;
            totalResultCount = 0;

            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (KeyValuePair<string, object> pair in parms)
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                ds = DataAccess.ExecuteStoredProcedure("usp_SearchRAReports", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                ds.Tables[0].TableName = "RetrieveRAResults";
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        #region "File Processing Methods"

        // TODO: Joe for each Service Location send an MMIS record with the "terminateEnrollment" selection
        //4)	Send an MMIS record for each Service Location with the “Enrollment Status” code selected above
        public static void MMISTerminate(int partyId, string terminateEnrollment)
        {
            try
            {
                // terminateEnrollment = MMIS_ENROLLMENT_STATUS.CDE_ENROLL_STATUS
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        #endregion

        public static void InsertCommunicationEventHistoryEvent(int communicationEventId, DateTime downloadDate, Guid lastModifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_ID", DbType.Int32, communicationEventId, false));
                parameters.Add(SqlParms.CreateParameter("DOWNLOADED_DATE", DbType.DateTime, downloadDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));

                DataAccess.ExecuteStoredProcedure("usp_Insert_COMMUNICATION_EVENT_DOWNLOAD_HISTORY", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static void UpdateRetrieveReports(int index, bool isDownloaded, DateTime downloadDate, DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, index, false));
                parameters.Add(SqlParms.CreateParameter("ISDOWNLOADED", DbType.Boolean, isDownloaded, false));
                parameters.Add(SqlParms.CreateParameter("DOWNLOADED_DATE", DbType.DateTime, downloadDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedUser, false));

                DataAccess.ExecuteStoredProcedure("updateDOCUMENT_ATTACHMENT_XREF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveFaultCodeException(string faultString, string faultCode, int transactionID, DateTime modifiedDate, Guid modifiedBy, string rawResponse, string pnmTransactionKey = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ModuleTransactionId", DbType.Int32, transactionID, false));
                parameters.Add(SqlParms.CreateParameter("FaultString", DbType.String, faultString, false));
                parameters.Add(SqlParms.CreateParameter("FaultCode", DbType.String, faultCode, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("RAW_RESPONSE", DbType.String, rawResponse, false));
                parameters.Add(SqlParms.CreateParameter("PNM_TRANSACTION_KEY", DbType.String, pnmTransactionKey, false));
                DataAccess.ExecuteStoredProcedure("updateTransaction_Queue", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void saveSoapResponseCodeException(string ModuleTransactionId, string SITransactionKey, string ResponseCode, string ResponseDetails,
            string ResponseMessage, string ResponseType, DateTime modifiedDate, Guid modifiedBy, string rawResponse, string pnmTransactionKey = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ModuleTransactionId", DbType.Int32, ModuleTransactionId, false));
                parameters.Add(SqlParms.CreateParameter("SITransactionKey", DbType.String, SITransactionKey, false));
                parameters.Add(SqlParms.CreateParameter("ResponseCode", DbType.String, ResponseCode, false));
                parameters.Add(SqlParms.CreateParameter("ResponseDetails", DbType.String, ResponseDetails, false));
                parameters.Add(SqlParms.CreateParameter("ResponseMessage", DbType.String, ResponseMessage, false));
                parameters.Add(SqlParms.CreateParameter("ResponseType", DbType.String, ResponseType, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("RAW_RESPONSE", DbType.String, rawResponse, false));
                parameters.Add(SqlParms.CreateParameter("PNM_TRANSACTION_KEY", DbType.String, pnmTransactionKey, false));
                DataAccess.ExecuteStoredProcedure("updateTransaction_Queue", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetStagingProviderEnrollmentData(int TransactionID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TransactionID", DbType.Int32, TransactionID, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_GetStagingProviderEnrollmentByRegId_New", parameters, "StagingProviderEnrollment");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPECOSRevalidationData(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_selectSCR_PECOS_REVALIDATION_DATA", parameters, "ProviderFee");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetKidsSpecialtyRegistrations()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_CPC_EnrollmentNotEligibleForKidsSpecialty", parameters, "ProviderKidsCPC");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetNotReattestedCPCRegistrations()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectEligible_NotReattestedCPCProviders", parameters, "ProviderNotRettestedCPC");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetPPOldCPCMembers()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectOldCPC_PPMembers", parameters, "ProviderOldCPC");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet EndDateCPCProvider(int regID, DateTime termDate, string changedBy, string enrollmentStatusCode, string enrollmentStatusReason, bool createTrans = false, bool is_frm_job = false, bool endKidsSpecialty = false, bool endOldPP = false, int affiliationId = 0)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, termDate, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, false));
                parameters.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, createTrans, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, enrollmentStatusCode, false));
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASON_CODE", DbType.String, enrollmentStatusReason, false));
                parameters.Add(SqlParms.CreateParameter("savePriorRevalidationDate", DbType.Boolean, true, false));
                parameters.Add(SqlParms.CreateParameter("isKidsSpecialtyEndDate", DbType.Boolean, endKidsSpecialty, false));
                parameters.Add(SqlParms.CreateParameter("OldPPEndDate", DbType.Boolean, endOldPP, false));
                parameters.Add(SqlParms.CreateParameter("REG_AFFILIATION_ID", DbType.Int32, affiliationId, false));
                parameters.Add(SqlParms.CreateParameter("IS_FRM_JOB", DbType.Boolean, is_frm_job, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_EndDateCPCProvider", parameters, "CPCProvidersYearend");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPlaceOfserviceByCodeDynamic(string PlaceOfserviceCode)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PlaceOfServiceCode", DbType.String, PlaceOfserviceCode, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_PLACE_OF_SERVICE_BY_CODE_DYNAMIC", parameters, "PlaceOfService");
                lookup.Tables[0].TableName = "PlaceOfService";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetPlaceOfserviceByCode(string PlaceOfserviceCode, string placeofserviceDesc)
        {
            try
            {
                if (PlaceOfserviceCode.Length == 1)
                    PlaceOfserviceCode = PlaceOfserviceCode.PadLeft(2, '0');
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PlaceOfServiceCode", DbType.String, PlaceOfserviceCode, false));
                parameters.Add(SqlParms.CreateParameter("PlaceOfServiceDesc", DbType.String, placeofserviceDesc, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_PLACE_OF_SERVICE_BY_CODE", parameters, "PlaceOfService");
                lookup.Tables[0].TableName = "PlaceOfService";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}