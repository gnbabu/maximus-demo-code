using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.Controllers.PDMS
{
    public static class DIDDController
    {
        public static DataSet GetRegistrationStatusForDIDDReferral(int referralId)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, referralId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetRegistrationByReferral", parameters, "Registration");

                lookup.Tables[0].TableName = "Registration";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchDIDDReferral(string name, string applicationNo, string taxId, string medicaidId, string npi, string emailAddress, string createdBy, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            DataSet ds = new DataSet();
            string outValue;
            totalResultCount = 0;

            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Name", DbType.String, name, false));
                parameters.Add(SqlParms.CreateParameter("ApplicationNo", DbType.String, applicationNo, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxId, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidId, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("EmailAddress", DbType.String, emailAddress, false));
                parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.String, createdBy, false));
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SearchDDIDReferral", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                ds.Tables[0].TableName = "ReferralSearchResults";
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

        public static DataSet SearchDIDDReferralByRegIDs(string RegIDs, int pageSize, int startRowIndex, int tableId, int statusId, int ordinal, bool isAssigned, out int totalResultCount)
        {
            try
            {
                string outValue = "";
                totalResultCount = 0;
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegIDs", DbType.String, RegIDs, false));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.Int32, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, false));
                if (tableId > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("TableId", DbType.Int32, tableId, false));
                    parameters.Add(SqlParms.CreateParameter("StatusId", DbType.Int32, statusId, false));
                    parameters.Add(SqlParms.CreateParameter("Ordinal", DbType.Int32, ordinal, false));
                }
                parameters.Add(SqlParms.CreateParameter("IsAssigned", DbType.Boolean, isAssigned, false));  
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SearchDIDDReferralByRegIDs", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }
                lookup.Tables[0].TableName = "ReferralSearchResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchDIDDReferralByReferralIDs(string ReferralIDs, int pageSize, int startRowIndex, int tableId, int statusId, int ordinal, out int totalResultCount)
        {
            try
            {
                string outValue = "";
                totalResultCount = 0;
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ReferralIDs", DbType.String, ReferralIDs, false));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.Int32, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, false));
                if (tableId > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("TableId", DbType.Int32, tableId, false));
                    parameters.Add(SqlParms.CreateParameter("StatusId", DbType.Int32, statusId, false));
                    parameters.Add(SqlParms.CreateParameter("Ordinal", DbType.Int32, ordinal, false));
                }
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_REFERRAL_ByReferralIDs", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }
                lookup.Tables[0].TableName = "ReferralSearchResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateDIDDReferral(int diddReferralID, string firstName, string lastName,
            string groupIdentityName, string taxId, string NPI, string email, DateTime? contractFromDate, DateTime? contractToDate, string zip, string zipExt, 
            int submitTypeID, int locationTypeID, DateTime modifiedOn, Guid modifiedBy, int referralType)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralID, false));
                parameters.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, firstName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, lastName, true));
                parameters.Add(SqlParms.CreateParameter("GROUP_ENTITY_NAME", DbType.String, groupIdentityName, true));
                parameters.Add(SqlParms.CreateParameter("TAX_ID", DbType.String, taxId, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
                parameters.Add(SqlParms.CreateParameter("email", DbType.String, email, true));
                if (contractFromDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("DIDDCONTRACT_FROMDATE", DbType.Date, contractFromDate.Value, false));
                }
                if (contractToDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("DIDDCONTRACT_TODATE", DbType.Date, contractToDate.Value, false));
                }
                parameters.Add(SqlParms.CreateParameter("ZIP", DbType.String, zip, false));
                parameters.Add(SqlParms.CreateParameter("ZIP_EXT", DbType.String, zipExt, false));
                parameters.Add(SqlParms.CreateParameter("SUBMIT_TYPE_ID", DbType.Int32, submitTypeID, false));
                parameters.Add(SqlParms.CreateParameter("MODIFIED_ON", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("MODIFIED_BY", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_TYPE_ID", DbType.Int32, referralType, false));
                parameters.Add(SqlParms.CreateParameter("LOCATION_TYPE_ID", DbType.Int32, locationTypeID, false));

                DataAccess.ExecuteScalar("updateDIDD_REFERRALCustom", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateDIDDReferralSuffix(int diddReferralId, string regionSuffix)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralId, false));
                parameters.Add(SqlParms.CreateParameter("RegionSuffix", DbType.String, regionSuffix, false));

                DataAccess.ExecuteScalar("usp_UpdateDIDD_REFERRALSuffix", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDIDDReferral(int referralId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, referralId, false));

                DataSet referral = new DataSet();
                referral = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_REFERRAL", parameters, "DIDDRefferral" + referralId);
                referral.Tables[0].TableName = "DIDDRefferral" + referralId;

                return referral;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectDIDDReferralByID(int referralId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ReferralID", DbType.Int32, referralId, false));

                DataSet referral = new DataSet();
                referral = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_REFERRAL_ByID", parameters, "DIDDRefferral" + referralId);
                referral.Tables[0].TableName = "DIDDRefferral" + referralId;

                return referral;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchProviderAllTaxID(string TaxID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                
                parameters.Add(SqlParms.CreateParameter("Taxid", DbType.String, TaxID, false));

                DataSet referral = new DataSet();
                referral = DataAccess.ExecuteStoredProcedure("usp_SearchAllNFOCUSProviderByTaxID", parameters, "DuplicateDIDDRefferral");
                referral.Tables[0].TableName = "DuplicateDIDDRefferral";

                return referral;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDuplicateReferrals(int Referral_ID, string TaxID, string ZipCode, string ZipExt)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Referral_ID", DbType.Int32, Referral_ID, false));
                parameters.Add(SqlParms.CreateParameter("Tax_id", DbType.String, TaxID, false));
                parameters.Add(SqlParms.CreateParameter("Zip", DbType.String, ZipCode, false));
                parameters.Add(SqlParms.CreateParameter("Zip_Ext", DbType.String, ZipExt, false));

                DataSet referral = new DataSet();
                referral = DataAccess.ExecuteStoredProcedure("usp_SelectDuplicate_DIDD_REFERRAL", parameters, "DuplicateDIDDRefferral");
                referral.Tables[0].TableName = "DuplicateDIDDRefferral";

                return referral;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        
        public static DataSet SelectDIDDReferralByApplicationNo(string applicationNo, string taxID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(applicationNo))
                    parameters.Add(SqlParms.CreateParameter("ApplicationNo", DbType.String, applicationNo, false));
                if (!string.IsNullOrEmpty(taxID))
                    parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));

                DataSet referral = new DataSet();
                referral = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_REFERRAL_ByApplicationNo", parameters, "DIDDRefferral");
                referral.Tables[0].TableName = "DIDDRefferral";

                return referral;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet InsertDIDDReferral(string firstName, string lastName, string groupIdentityName, string applicationNo, string taxId, string NPI,
            string email, DateTime? contractFromDate, DateTime? contractToDate, bool isActive, string zip, string zipExt, int submitTypeID, int locationTypeID, 
            DateTime createdOn, Guid createdBy, int referralType)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, firstName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, lastName, true));
                parameters.Add(SqlParms.CreateParameter("GROUP_ENTITY_NAME", DbType.String, groupIdentityName, true));
                parameters.Add(SqlParms.CreateParameter("APPLICATION_NO", DbType.String, applicationNo, false));
                parameters.Add(SqlParms.CreateParameter("TAX_ID", DbType.String, taxId, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
                parameters.Add(SqlParms.CreateParameter("email", DbType.String, email, true));
                if (contractFromDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("CONTRACTFROMDATE", DbType.DateTime, contractFromDate.Value, true));
                }
                if (contractToDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("CONTRACTToDATE", DbType.DateTime, contractToDate.Value, true));
                }
                parameters.Add(SqlParms.CreateParameter("IS_ACTIVE", DbType.Boolean, isActive, false));
                parameters.Add(SqlParms.CreateParameter("ZIP", DbType.String, zip, false));
                parameters.Add(SqlParms.CreateParameter("ZIP_EXT", DbType.String, zipExt, false));
                parameters.Add(SqlParms.CreateParameter("SUBMIT_TYPE_ID", DbType.Int32, submitTypeID, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON", DbType.DateTime, createdOn, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY", DbType.Guid, createdBy, false));
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_TYPE_ID", DbType.Int32, referralType, false));
                parameters.Add(SqlParms.CreateParameter("LOCATION_TYPE_ID", DbType.Int32, locationTypeID, false));

                return DataAccess.ExecuteStoredProcedure("insertDIDD_REFERRAL", parameters, "DIDDRefferral");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int UpdateDIDDReferral(int referralId, string firstName, string lastName, string groupIdentityName,
            string applicationNo, string taxId, string NPI, string email, int DiddWaiverId, int DiddRegionId, bool? isActive, DateTime modifiedOn, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, referralId, true));

                if(!string.IsNullOrEmpty(firstName))
                    parameters.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, firstName, true));
                
                if (!string.IsNullOrEmpty(lastName))
                    parameters.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, lastName, true));
                
                if (!string.IsNullOrEmpty(groupIdentityName))
                    parameters.Add(SqlParms.CreateParameter("GROUP_ENTITY_NAME", DbType.String, groupIdentityName, true));

                if (!string.IsNullOrEmpty(applicationNo))
                    parameters.Add(SqlParms.CreateParameter("APPLICATION_NO", DbType.String, applicationNo, false));
                
                if (!string.IsNullOrEmpty(taxId))
                    parameters.Add(SqlParms.CreateParameter("TAX_ID", DbType.String, taxId, false));
                
                if (!string.IsNullOrEmpty(NPI))
                    parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
                
                if (!string.IsNullOrEmpty(email))
                    parameters.Add(SqlParms.CreateParameter("email", DbType.String, email, true));
                
                if (DiddWaiverId != 0 )
                    parameters.Add(SqlParms.CreateParameter("DIDD_WAIVER_ID", DbType.Int32, DiddWaiverId, true));
                
                if (DiddRegionId != 0)
                    parameters.Add(SqlParms.CreateParameter("DIDD_REGION_ID", DbType.Int32, DiddRegionId, true));

                if (isActive.HasValue)
                    parameters.Add(SqlParms.CreateParameter("IS_ACTIVE", DbType.Boolean, isActive, false));

                parameters.Add(SqlParms.CreateParameter("MODIFIED_ON", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("MODIFIED_BY", DbType.Guid, modifiedBy, false));

                return Convert.ToInt32(DataAccess.ExecuteScalar("updateDIDD_REFERRAL", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDIDDReferralService(int referralID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, referralID, false));

                DataSet referral = new DataSet();
                referral = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_REFERRAL_SERVICE", parameters, "DIDDServiceRefferral" + referralID);
                referral.Tables[0].TableName = "DIDDServiceRefferral" + referralID;

                return referral;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectDIDDReferralServiceByReferralServiceID(int referralServiceID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_SERVICE_ID", DbType.Int32, referralServiceID, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_REFERRAL_SERVICEByReferralServiceID", parameters, "serviceData");
                ds.Tables[0].TableName = "serviceData";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertDIDDReferralService(int diddReferralId, int diddServiceId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralId, false));
                parameters.Add(SqlParms.CreateParameter("DIDD_SERVICE_ID", DbType.Int32, diddServiceId, false));

                return Convert.ToInt32(DataAccess.ExecuteScalar("insertDIDD_REFERRAL_SERVICE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int InsertDIDDREFERRALWAIVER(int diddReferralId, int diddWaiverId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralId, false));
                parameters.Add(SqlParms.CreateParameter("DIDD_WAIVER_ID", DbType.Int32, diddWaiverId, false));

                return Convert.ToInt32(DataAccess.ExecuteScalar("insertDIDD_REFERRAL_WAIVER", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int InsertDIDDREFERRALREGION(int diddReferralId, int diddRegionId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralId, false));
                parameters.Add(SqlParms.CreateParameter("DIDD_REGION_ID", DbType.Int32, diddRegionId, false));

                return Convert.ToInt32(DataAccess.ExecuteScalar("insertDIDD_REFERRAL_REGION", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static int UpdateDIDDReferralService(int diddReferralServiceId, int diddReferralId, int diddServiceId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_SERVICE_ID", DbType.Int32, diddReferralServiceId, false));
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralId, false));
                parameters.Add(SqlParms.CreateParameter("DIDD_SERVICE_ID", DbType.Int32, diddServiceId, false));

                return Convert.ToInt32(DataAccess.ExecuteScalar("updateDIDD_REFERRAL_SERVICE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int UpdateDIDDReferral(int referralId, string firstName, string lastName, string groupIdentityName,
            string applicationNo, string taxId, string NPI, string email, string DiddWaiverId, string DiddRegionId, int DiddReferralTypeId, bool? isActive, DateTime modifiedOn, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, referralId, true));

                if (!string.IsNullOrEmpty(firstName))
                    parameters.Add(SqlParms.CreateParameter("FIRST_NAME", DbType.String, firstName, true));

                if (!string.IsNullOrEmpty(lastName))
                    parameters.Add(SqlParms.CreateParameter("LAST_NAME", DbType.String, lastName, true));

                if (!string.IsNullOrEmpty(groupIdentityName))
                    parameters.Add(SqlParms.CreateParameter("GROUP_ENTITY_NAME", DbType.String, groupIdentityName, true));

                if (!string.IsNullOrEmpty(applicationNo))
                    parameters.Add(SqlParms.CreateParameter("APPLICATION_NO", DbType.String, applicationNo, false));

                if (!string.IsNullOrEmpty(taxId))
                    parameters.Add(SqlParms.CreateParameter("TAX_ID", DbType.String, taxId, false));

                if (!string.IsNullOrEmpty(NPI))
                    parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));

                if (!string.IsNullOrEmpty(email))
                    parameters.Add(SqlParms.CreateParameter("email", DbType.String, email, true));

                if (!string.IsNullOrEmpty(DiddWaiverId))
                    parameters.Add(SqlParms.CreateParameter("DIDD_WAIVER_ID", DbType.String, DiddWaiverId, true));

                if (!string.IsNullOrEmpty(DiddRegionId))
                    parameters.Add(SqlParms.CreateParameter("DIDD_REGION_ID", DbType.String, DiddRegionId, true));

                if (isActive.HasValue)
                    parameters.Add(SqlParms.CreateParameter("IS_ACTIVE", DbType.Boolean, isActive, false));

                parameters.Add(SqlParms.CreateParameter("MODIFIED_ON", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("MODIFIED_BY", DbType.Guid, modifiedBy, false));

                return Convert.ToInt32(DataAccess.ExecuteScalar("updateDIDD_REFERRAL", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int DeleteDIDDREFERRALSERVICE(int diddReferralId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralId, false));


                return Convert.ToInt32(DataAccess.ExecuteScalar("DeleteDIDD_REFERRAL_SERVICE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int DeleteDIDDREFERRALREGION(int diddReferralId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralId, false));


                return Convert.ToInt32(DataAccess.ExecuteScalar("DeleteDIDD_REFERRAL_REGION", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int DeleteDIDDREFERRALWAIVER(int diddReferralId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_ID", DbType.Int32, diddReferralId, false));


                return Convert.ToInt32(DataAccess.ExecuteScalar("DeleteDIDD_REFERRAL_WAIVER", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectDIDDReferralTypes()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_REFERRAL_TYPE");

                lookup.Tables[0].TableName = "DIDD_REFERRAL_TYPE";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDIDDRegions()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_REGION");

                lookup.Tables[0].TableName = "DIDDRegions";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDIDDWaivers()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_WAIVER");

                lookup.Tables[0].TableName = "DIDDWaiver";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDIDDServices()
        {
            try
            {
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_SERVICE");

                lookup.Tables[0].TableName = "DIDDService";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDIDDServices(int waiverID)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("WaiverID", DbType.Int32, waiverID, false));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_SERVICE_ByWaiverID", parameters, "DIDDService");
                lookup.Tables[0].TableName = "DIDDService";
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectDIDDServicesByDIDD_Service_ID(int DIDD_Service_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_SERVICE_ID", DbType.Int32, DIDD_Service_ID, false));
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_ServiceByDIDD_SERVICE_ID", parameters, "DIDDservices");

                lookup.Tables[0].TableName = "DIDDService";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectDIDDContractDetails(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDIDD_REG_Contract_Details", parameters, "DIDDDetails");

                lookup.Tables[0].TableName = "DIDDDetails";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectCurrentContractInfoByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_Contract_CurrentByRegID", parameters, "CurrentContract");

                lookup.Tables[0].TableName = "CurrentContract";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectContractHistoryByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_Contract_HistoryByRegID", parameters, "ContractHistory");

                lookup.Tables[0].TableName = "ContractHistory";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectSignatureHistoryByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectREG_Contract_SignaturesByRegID", parameters, "History");

                lookup.Tables[0].TableName = "History";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectDIDDReferralByRegID(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectDIDDReferralByRegID", parameters, "ReferralData");

                lookup.Tables[0].TableName = "ReferralData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertUpdateContractByContractTypeID(int regID, int contractTypeID, string adminName, DateTime? contractStartDate, DateTime? contractEndDate, string signedBy, DateTime changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("ContractTypeID", DbType.Int32, contractTypeID, false));
                if (!string.IsNullOrEmpty(adminName))
                {
                    parameters.Add(SqlParms.CreateParameter("Administrator", DbType.String, adminName, false));
                }
                if (contractStartDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("ContractStartDate", DbType.Date, contractStartDate, false));
                }
                if (contractEndDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("ContractEndDate", DbType.Date, contractEndDate, false));
                }
                parameters.Add(SqlParms.CreateParameter("SignedBy", DbType.String, signedBy, false));
                parameters.Add(SqlParms.CreateParameter("ModifiedDateTime", DbType.DateTime, changedDate, false));
                parameters.Add(SqlParms.CreateParameter("ModifiedByUser", DbType.Guid, changedBy, false));

                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertReg_ContractCustom", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void ResetContractSignatures(int regID, DateTime changedDate, Guid changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("ModifiedDateTime", DbType.DateTime, changedDate, false));
                parameters.Add(SqlParms.CreateParameter("ModifiedByUser", DbType.Guid, changedBy, false));

                DataAccess.ExecuteScalar("usp_REG_CONTRACT_ResetSignatures", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateDIDDReferralStartDate(int regID, DateTime startDate)
        {
            //Updates the start date on the referral and the associated services, where the dates are currently null.
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("DIDDCONTRACT_FROMDATE", DbType.DateTime, startDate, false));

                DataAccess.ExecuteScalar("usp_UpdateDIDD_REFERRALStartDate", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static void UpdateDIDDReferralServiceDates(int referralServiceID, DateTime startDate, DateTime? endDate, DateTime modifiedOn, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DIDD_REFERRAL_SERVICE_ID", DbType.Int32, referralServiceID, false));
                parameters.Add(SqlParms.CreateParameter("CONTRACT_FROMDATE", DbType.DateTime, startDate, false));
                if (endDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("CONTRACT_TODATE", DbType.DateTime, endDate.Value, false));
                }
                parameters.Add(SqlParms.CreateParameter("MODIFIED_DATE", DbType.DateTime, modifiedOn, false));
                parameters.Add(SqlParms.CreateParameter("MODIFIED_BY", DbType.Guid, modifiedBy, false));

                DataAccess.ExecuteScalar("usp_UpdateDIDD_REFERRAL_SERVICEDates", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
