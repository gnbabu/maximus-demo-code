using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.Controllers.PDMS
{
    public static class ACHController
    {
        public static DataSet SearchACHFeeInformation(string name, string taxID, string npi, string medicaidID, DateTime? feePaidDateFrom, 
            DateTime? feePaidDateTo, DateTime? feeDueDateFrom, DateTime? feeDueDateTo, int sortBy, int pageNumber, int rowsPerPage, 
            bool sortAsc)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Name", DbType.String, name, false));
                parameters.Add(SqlParms.CreateParameter("BaseMedicaidID", DbType.String, medicaidID, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                if (feePaidDateFrom.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("PaidFrom", DbType.Date, feePaidDateFrom, false));
                }
                if (feePaidDateTo.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("PaidTo", DbType.Date, feePaidDateTo, false));
                }
                if (feeDueDateFrom.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("DueFrom", DbType.Date, feeDueDateFrom, false));
                }
                if (feeDueDateTo.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("DueTo", DbType.Date, feeDueDateTo, false));
                }
                parameters.Add(SqlParms.CreateParameter("SortBy", DbType.Int32, sortBy, false));
                parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageNumber, false));
                parameters.Add(SqlParms.CreateParameter("RowsPerPage", DbType.Int32, rowsPerPage, false));
                parameters.Add(SqlParms.CreateParameter("ASC", DbType.Boolean, sortAsc, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SearchAch_Fee_Information", parameters, "FeeSearchResults");

                lookup.Tables[0].TableName = "FeeSearchResults";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectACHFeeInformation(string name, string taxID, string npi, string medicaidID, DateTime? feePaidDateFrom, 
            DateTime? feePaidDateTo, DateTime? feeDueDateFrom, DateTime? feeDueDateTo)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Name", DbType.String, name, false));
                parameters.Add(SqlParms.CreateParameter("BaseMedicaidID", DbType.String, medicaidID, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                if (feePaidDateFrom.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("PaidFrom", DbType.Date, feePaidDateFrom, false));
                }
                if (feePaidDateTo.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("PaidTo", DbType.Date, feePaidDateTo, false));
                }
                if (feeDueDateFrom.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("DueFrom", DbType.Date, feeDueDateFrom, false));
                }
                if (feeDueDateTo.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("DueTo", DbType.Date, feeDueDateTo, false));
                }

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectAch_Fee_Results", parameters, "FeeInformation");

                lookup.Tables[0].TableName = "FeeInformation";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectACHFeeInformationByPartyID(int partyID)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("partyID", DbType.Int32, partyID, false));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectAch_Fee_InformationByPartyID", parameters, "FeeInformation");

                ds.Tables[0].TableName = "FeeInformation";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertAchFeeInformation(int partyID, DateTime paymentDate, string edisonID, string paymentNumber, DateTime createdDateTime, Guid createdByUserID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyID, true));
                parameters.Add(SqlParms.CreateParameter("PaymentDate", DbType.Date, paymentDate.Date, true));
                parameters.Add(SqlParms.CreateParameter("EdisonID", DbType.String, edisonID, true));
                parameters.Add(SqlParms.CreateParameter("PaymentNumber", DbType.String, paymentNumber, true));
                parameters.Add(SqlParms.CreateParameter("CreatedDateTime", DbType.DateTime, createdDateTime, true));
                parameters.Add(SqlParms.CreateParameter("CreatedByUser", DbType.Guid, createdByUserID, true));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertAchFeeInformation", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }


    }
}
