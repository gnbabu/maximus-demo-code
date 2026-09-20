using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.Controllers.PDMS
{
    public static class PaperRequestController
    {

        public static int InsertPaperRequestQueue(int requestTypeID, int documentTypeID, int documentHandle, int requestStatusTypeID, int applicationTypeID, int providerTypeID, int specialtyTypeID,
                int taxonomyTypeID, string taxonomyTypeCode, string taxID, string NPI, string medicaidID, string zip, string zipExt, string comments, DateTime createdOn, Guid createdBy)
        {
            int newID = 0; 
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PaperRequestTypeID", DbType.Int32, requestTypeID, true));
                parameters.Add(SqlParms.CreateParameter("PaperRequestDocumentTypeID", DbType.Int32, documentTypeID, true));
                parameters.Add(SqlParms.CreateParameter("DocumentHandleID", DbType.Int32, documentHandle, true));
                parameters.Add(SqlParms.CreateParameter("PaperRequestStatusTypeID", DbType.Int32, requestStatusTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeID, true));
                if (providerTypeID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, true));
                }
                if (specialtyTypeID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, specialtyTypeID, true));
                }
                if (taxonomyTypeID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("TaxonomyTypeID", DbType.Int32, taxonomyTypeID, true));
                }
                parameters.Add(SqlParms.CreateParameter("TaxonomyTypeCode", DbType.String, taxonomyTypeCode, true));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, true));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidID, true));
                parameters.Add(SqlParms.CreateParameter("Zip", DbType.String, zip, true));
                parameters.Add(SqlParms.CreateParameter("Ext_Zip ", DbType.String, zipExt, true));
                parameters.Add(SqlParms.CreateParameter("Comments ", DbType.String, comments, true));
                parameters.Add(SqlParms.CreateParameter("CreatedOnDateTime ", DbType.DateTime, createdOn, true));
                parameters.Add(SqlParms.CreateParameter("CreatedByUser ", DbType.Guid, createdBy, true));

                newID = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertPAPER_REQUEST_QUEUE", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return newID;
        }

        public static void UpdatePaperRequestQueue(int paperRequestID, int requestTypeID, int documentTypeID, int requestStatusTypeID, int applicationTypeID, int providerTypeID, int specialtyTypeID,
        int taxonomyTypeID, string taxonomyTypeCode, string taxID, string NPI, string medicaidID, string zip, string zipExt, string comments, DateTime modifiedOn, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, paperRequestID, true));
                parameters.Add(SqlParms.CreateParameter("PaperRequestTypeID", DbType.Int32, requestTypeID, true));
                parameters.Add(SqlParms.CreateParameter("PaperRequestDocumentTypeID", DbType.Int32, documentTypeID, true));
                parameters.Add(SqlParms.CreateParameter("PaperRequestStatusTypeID", DbType.Int32, requestStatusTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeID, true));
                if (providerTypeID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, true));
                }
                parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, specialtyTypeID, true));
                if (taxonomyTypeID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("TaxonomyTypeID", DbType.Int32, taxonomyTypeID, true));
                }
                parameters.Add(SqlParms.CreateParameter("TaxonomyTypeCode", DbType.String, taxonomyTypeCode, true));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, taxID, true));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidID, true));
                parameters.Add(SqlParms.CreateParameter("Zip", DbType.String, zip, true));
                parameters.Add(SqlParms.CreateParameter("Ext_Zip ", DbType.String, zipExt, true));
                parameters.Add(SqlParms.CreateParameter("Comments ", DbType.String, comments, true));
                parameters.Add(SqlParms.CreateParameter("ModifiedOnDateTime ", DbType.DateTime, modifiedOn, true));
                parameters.Add(SqlParms.CreateParameter("ModifiedByUser ", DbType.Guid, modifiedBy, true));

                DataAccess.ExecuteStoredProcedure("usp_UpdatePAPER_REQUEST_QUEUE", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void LinkPaperRequestQueueToReg(int paperRequestID, int regID, DateTime modifiedOn, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, paperRequestID, true));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("ModifiedOnDateTime ", DbType.DateTime, modifiedOn, true));
                parameters.Add(SqlParms.CreateParameter("ModifiedByUser ", DbType.Guid, modifiedBy, true));

                DataAccess.ExecuteStoredProcedure("usp_LinkPAPER_REQUEST_QUEUEToReg", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPaperRequestQueueByUserID(Guid userID, int paperRequestTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet ds = new DataSet();
                string outValue;
                totalResultCount = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, true));
                if (paperRequestTypeID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("RequestTypeID", DbType.Int32, paperRequestTypeID, true));
                }
                parameters.Add(SqlParms.CreateParameter("SortExpression", DbType.String, sortColumn, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.String, pageSize, false));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.String, startRowIndex, false));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.String, getTotalRowCount, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectPAPER_REQUEST_QUEUEByUserID", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);
                ds.Tables[0].TableName = "MyQueue";
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

        public static DataSet SelectProviderOperatorDashboardByUserID(Guid userID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, true));
                
                DataSet ds = new DataSet();
                string storedProc = "usp_PaperRequestDashboards";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "MyDashboard");
                ds.Tables[0].TableName = "MyDashboard";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPaperRequestQueueByQueueID(int queueID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, queueID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectPAPER_REQUEST_QUEUEByQueueID";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "PaperData");
                ds.Tables[0].TableName = "PaperData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertPaperRequestError(int requestID, int errorTypeID, int errorStatusTypeID, DateTime createdOn, Guid createdBy)
        {
            int newID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PaperRequestID", DbType.Int32, requestID, true));
                parameters.Add(SqlParms.CreateParameter("ErrorTypeID", DbType.Int32, errorTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ErrorStatusTypeID", DbType.Int32, errorStatusTypeID, true));
                parameters.Add(SqlParms.CreateParameter("CreatedOnDateTime ", DbType.DateTime, createdOn, true));
                parameters.Add(SqlParms.CreateParameter("CreatedByUser ", DbType.Guid, createdBy, true));

                newID = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertPAPER_REQUEST_ERROR", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return newID;
        }

        public static void ValidatePaperRequest(int paperRequestID, DateTime createdOn, Guid createdBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, paperRequestID, true));
                parameters.Add(SqlParms.CreateParameter("ValidatedOnDateTime ", DbType.DateTime, createdOn, true));
                parameters.Add(SqlParms.CreateParameter("ValidatedByUser ", DbType.Guid, createdBy, true));

                DataAccess.ExecuteStoredProcedure("usp_ValidatePAPER_REQUEST", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPaperRequestErrorsByQueueID(int queueID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, queueID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectPAPER_REQUEST_ERROROutstandingByQueueID";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "QueueErrors");
                ds.Tables[0].TableName = "QueueErrors";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPaperRequestMatchData(int queueID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, queueID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectPAPER_REQUEST_QUEUEMatchData";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "QueueErrors");
                ds.Tables[0].TableName = "QueueErrors";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPaperRequestByDocumentHandle(int documentHandleID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DocumentHandleID", DbType.Int32, documentHandleID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectPaperRequestByDocumentHandle";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "PaperRequest");
                ds.Tables[0].TableName = "PaperRequest";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int SelectNextUnassignedPaperRequest(Guid userID, DateTime requestedOn)
        {
            int paperRequestID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID ", DbType.Guid, userID, true));
                parameters.Add(SqlParms.CreateParameter("RequestedDateTime ", DbType.DateTime, requestedOn, true));

                paperRequestID = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectNextUnassignedPaperRequest", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return paperRequestID;
        }

        public static DataSet SelectPaperRequestDocumentsByQueueID(int paperRequestQueueID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PaperRequestQueueID", DbType.Int32, paperRequestQueueID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectPAPER_REQUEST_DOCUMENTByQueueID";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "MyDocs");
                ds.Tables[0].TableName = "MyDocs";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static int InsertPaperRequestDocument(int requestID, string name, string description, string fileName, DateTime createdOn, Guid createdBy)
        {
            int newID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PAPER_REQUEST_QUEUE_ID", DbType.Int32, requestID, true));
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, true));
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, description, true));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, fileName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME ", DbType.DateTime, createdOn, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER ", DbType.Guid, createdBy, true));

                newID = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertPAPER_REQUEST_DOCUMENT", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return newID;
        }

    }
}
