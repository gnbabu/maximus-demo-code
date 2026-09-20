using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.Controllers.PDMS
{
    public static class DocumentUploadController
    {
        //public static void InsertOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested,
        //    string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId,
        //    string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier)
        //{
        //    try
        //    {
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(SqlParms.CreateParameter("EDITransaction_Type_ID", DbType.Int32, editTransactionTypeId, false));
        //        parameters.Add(SqlParms.CreateParameter("PayerRequested", DbType.String, payerRequested, false));
        //        parameters.Add(SqlParms.CreateParameter("Member_ID", DbType.String, memberId, false));
        //        parameters.Add(SqlParms.CreateParameter("Claim_Type_ID", DbType.Int32, claimTypeId, false));
        //        parameters.Add(SqlParms.CreateParameter("Claim_number", DbType.String, claimNumber, false));
        //        parameters.Add(SqlParms.CreateParameter("PA_NUMBER", DbType.String, paNumber, false));
        //        parameters.Add(SqlParms.CreateParameter("Provider_ID", DbType.String, providerId, false));
        //        parameters.Add(SqlParms.CreateParameter("Provider_NPI", DbType.String, providerNPI, false));
        //        parameters.Add(SqlParms.CreateParameter("Sender_ID", DbType.String, senderId, false));
        //        parameters.Add(SqlParms.CreateParameter("Receiver_ID", DbType.String, receiverId, false));
        //        parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_id", DbType.Int32, documentTypeId, false));
        //        parameters.Add(SqlParms.CreateParameter("DocumentName", DbType.String, documentName, false));
        //        parameters.Add(SqlParms.CreateParameter("UUID", DbType.Guid, uuid, false));
        //        parameters.Add(SqlParms.CreateParameter("TO_SEND", DbType.Boolean, toSend, false));
        //        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appAdminUserId, false));
        //        parameters.Add(SqlParms.CreateParameter("OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER", DbType.String,documentIdentifier, false));
        //        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
        //        parameters.Add(SqlParms.CreateParameter("CREATE_DATE_TIME", DbType.DateTime, DateTime.Now, false));
        //        DataAccess.ExecuteStoredProcedure("insertOutBound_Document_Uploads", parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw CoreException.ThrowException(ex);
        //    }
        //}

        public static void InsertClaimsOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested,
                string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId,
                string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string OrgFileName, string documentID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("EDITransaction_Type_ID", DbType.Int32, editTransactionTypeId, false));
                parameters.Add(SqlParms.CreateParameter("PayerRequested", DbType.String, payerRequested, false));
                parameters.Add(SqlParms.CreateParameter("Member_ID", DbType.String, memberId, false));
                parameters.Add(SqlParms.CreateParameter("Claim_Type_ID", DbType.Int32, claimTypeId, false));
                parameters.Add(SqlParms.CreateParameter("Claim_number", DbType.String, claimNumber, false));
                parameters.Add(SqlParms.CreateParameter("PA_NUMBER", DbType.String, paNumber, false));
                parameters.Add(SqlParms.CreateParameter("Provider_ID", DbType.String, providerId, false));
                parameters.Add(SqlParms.CreateParameter("Provider_NPI", DbType.String, providerNPI, false));
                parameters.Add(SqlParms.CreateParameter("Sender_ID", DbType.String, senderId, false));
                parameters.Add(SqlParms.CreateParameter("Receiver_ID", DbType.String, receiverId, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_id", DbType.Int32, documentTypeId, false));
                parameters.Add(SqlParms.CreateParameter("DocumentName", DbType.String, documentName, false));
                parameters.Add(SqlParms.CreateParameter("UUID", DbType.Guid, uuid, false));
                parameters.Add(SqlParms.CreateParameter("TO_SEND", DbType.Boolean, toSend, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appAdminUserId, false));
                parameters.Add(SqlParms.CreateParameter("OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER", DbType.String, documentIdentifier, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATE_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("OriginalDocumentName", DbType.String, OrgFileName, false));
                parameters.Add(SqlParms.CreateParameter("Document_ID", DbType.String, documentID, false));
                DataAccess.ExecuteStoredProcedure("Claims_insertOutBound_Document_Uploads", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested,
                string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId,
                string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string orginalDocumentName, string providerComments)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("EDITransaction_Type_ID", DbType.Int32, editTransactionTypeId, false));
                parameters.Add(SqlParms.CreateParameter("PayerRequested", DbType.String, payerRequested, false));
                parameters.Add(SqlParms.CreateParameter("Member_ID", DbType.String, memberId, false));
                parameters.Add(SqlParms.CreateParameter("Claim_Type_ID", DbType.Int32, claimTypeId, false));
                parameters.Add(SqlParms.CreateParameter("Claim_number", DbType.String, claimNumber, false));
                parameters.Add(SqlParms.CreateParameter("PA_NUMBER", DbType.String, paNumber, false));
                parameters.Add(SqlParms.CreateParameter("Provider_ID", DbType.String, providerId, false));
                parameters.Add(SqlParms.CreateParameter("Provider_NPI", DbType.String, providerNPI, false));
                parameters.Add(SqlParms.CreateParameter("Sender_ID", DbType.String, senderId, false));
                parameters.Add(SqlParms.CreateParameter("Receiver_ID", DbType.String, receiverId, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_id", DbType.Int32, documentTypeId, false));
                parameters.Add(SqlParms.CreateParameter("DocumentName", DbType.String, documentName, false));
                parameters.Add(SqlParms.CreateParameter("UUID", DbType.Guid, uuid, false));
                parameters.Add(SqlParms.CreateParameter("TO_SEND", DbType.Boolean, toSend, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appAdminUserId, false));
                parameters.Add(SqlParms.CreateParameter("OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER", DbType.String, documentIdentifier, false));
                parameters.Add(SqlParms.CreateParameter("OriginalDocumentName", DbType.String, orginalDocumentName, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATE_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("providerComments", DbType.String, providerComments, false));
                DataAccess.ExecuteStoredProcedure("insertOutBound_Document_Uploads", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void ArchiveDocumentToGenericUploadControl(int regID, int documentID, string regPageSection, string wfTaskName, string docUploadPage)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, regPageSection, true));
                parameters.Add(SqlParms.CreateParameter("WF_TASK_NAME", DbType.String, wfTaskName, true));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_UPLOAD_PAGE", DbType.String, docUploadPage, true));
                DataAccess.ExecuteStoredProcedure("usp_ArchiveDocumentToGenericUploadControl", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

    }

    
}
