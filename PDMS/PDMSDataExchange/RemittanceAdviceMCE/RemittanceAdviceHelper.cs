using CON = MAXIMUS.Core.Libraries.Constants;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using MAXIMUS.Core.Libraries;
using System.Net.Mail;
using System.Reflection;
using Corp.Core.Libraries;
using System.Dynamic;
using System.Linq;
using ClosedXML.Report;
using System.IO;
using System.Collections;
using System.Text;
using Amazon.Runtime.Internal.Transform;
using DocumentFormat.OpenXml.Office2010.Word;
using Microsoft.Office.Interop.Excel;

namespace MAXIMUS.DataExchange.PDMS.RemittanceAdviceMCE
{
    public static class RemittanceAdviceHelper
    {
        public static void CreateLogEntry(string processName, string message, int priority)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ThreadId", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), true));
                parameters.Add(SqlParms.CreateParameter("Message", DbType.String, message, true));
                parameters.Add(SqlParms.CreateParameter("ProcessName", DbType.String, processName, true));
                parameters.Add(SqlParms.CreateParameter("Machine", DbType.String, Environment.MachineName, true));
                parameters.Add(SqlParms.CreateParameter("User", DbType.String, Environment.UserDomainName + @"\" + Environment.UserName, true));
                parameters.Add(SqlParms.CreateParameter("Priority", DbType.Int32, priority, true));

                DataAccess.ExecuteScalar("usp_InsertLogRemittanceAdviceMCE", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), ex);
            }
        }

        public static int InsertREG_DOCUMENT(int regId, string dataFileListElement)
        {
            int documentId = 0;
            try
            {
                
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, CON.RemittanceAdviceMCE.RemittanceAdviceMCEPageTypeID, false));
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, dataFileListElement, false));
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, CON.RemittanceAdviceMCE.RemittanceAdviceMCEDescp, false));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, dataFileListElement, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), false));

                documentId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertREG_DOCUMENT", parameters));
            }
            catch (Exception ex)
            {
                CreateLogEntry("InsertREG_DOCUMENT", String.Format("RemittanceAdviceHelper Exception: {0} {1}", ex.Message, ex.StackTrace), CON.RemittanceAdviceMCE.LogPriorityError);
            }
            return documentId;
        }

        public static int InsertDOCUMENT_ATTACHMENT_XREF(int documentType, int documentId)
        {
            int attachmentID = 0;
            try
            {

                List<SqlParameter> attachmentXrefParameters = new List<SqlParameter>();
                attachmentXrefParameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_ID", DbType.Int32, documentType, false)); // is this in constants?
                attachmentXrefParameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentId, false));
                attachmentXrefParameters.Add(SqlParms.CreateParameter("CREATED_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                attachmentXrefParameters.Add(SqlParms.CreateParameter("CREATED_BY_MODIFIED_USER", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), false));
                attachmentXrefParameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), false));

                attachmentID = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT_ATTACHMENT_XREF", attachmentXrefParameters));
            }
            catch (Exception ex)
            {
                CreateLogEntry("InsertDOCUMENT_ATTACHMENT_XREF", String.Format("RemittanceAdviceHelper Exception: {0} {1}", ex.Message, ex.StackTrace), CON.RemittanceAdviceMCE.LogPriorityError);
            }
            return attachmentID;
        }

        public static int InsertREMITTANCE_ADVICE_Detail(int controlID, string icnValue)
        {
            int detailID = 0;
            try
            {

                List<SqlParameter> detailParameters = new List<SqlParameter>();
                detailParameters.Add(SqlParms.CreateParameter("remittance_advice_id", DbType.Int32, controlID, false));
                detailParameters.Add(SqlParms.CreateParameter("ICN", DbType.String, icnValue, false));
                detailID = Convert.ToInt32(DataAccess.ExecuteScalar("insertREMITTANCE_ADVICE_Detail", detailParameters));
            }
            catch (Exception ex)
            {
                CreateLogEntry("InsertREMITTANCE_ADVICE_Detail", String.Format("RemittanceAdviceHelper Exception: {0} {1}", ex.Message, ex.StackTrace), CON.RemittanceAdviceMCE.LogPriorityError);
            }
            return detailID;
        }

        public static int InsertINBOUND_RA_MCE_DTLS(string incomingFileName, string status, int operation)
        {
            int recordID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("INCOMING_RA_FILE_NAME", DbType.String, incomingFileName, true));
                parameters.Add(SqlParms.CreateParameter("STATUS", DbType.String, status, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, operation, true));

                recordID = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_RA_MCE_DTLS", parameters));
            }
            catch (Exception ex)
            {
                CreateLogEntry("InsertINBOUND_RA_MCE_DTLS", String.Format("RemittanceAdviceHelper Exception: {0} {1}", ex.Message, ex.StackTrace), CON.RemittanceAdviceMCE.LogPriorityError);
            }
            return recordID;
        }

        public static void UpdateResponseXMLINBOUND_RA_MCE_DTLS(int fileID, string status, string errorFileName, string responseXML, string errorMsg, int operation)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("INBOUND_RA_MCE_DTLS_ID", DbType.Int32, fileID, true));
                parameters.Add(SqlParms.CreateParameter("STATUS", DbType.String, status, true));
                parameters.Add(SqlParms.CreateParameter("OUTGOING_RA_CONTROL_FILE_NAME", DbType.String, errorFileName, true));
                parameters.Add(SqlParms.CreateParameter("OUTGOING_RA_CONTROL_FILE", DbType.String, responseXML, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_DETAILS", DbType.String, errorMsg, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, operation, true));

                DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_RA_MCE_DTLS", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateINBOUND_RA_MCE_DTLS", String.Format("RemittanceAdviceHelper Exception: {0} {1}", ex.Message, ex.StackTrace), CON.RemittanceAdviceMCE.LogPriorityError);
            }
        }

        public static void UpdateRequestXMLINBOUND_RA_MCE_DTLS(int fileID, string requestXML, int operation)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("INBOUND_RA_MCE_DTLS_ID", DbType.Int32, fileID, true));
                parameters.Add(SqlParms.CreateParameter("INCOMING_RA_CONTROL_FILE", DbType.String, requestXML, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, operation, true));

                DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_RA_MCE_DTLS", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateINBOUND_RA_MCE_DTLS", String.Format("RemittanceAdviceHelper Exception: {0} {1}", ex.Message, ex.StackTrace), CON.RemittanceAdviceMCE.LogPriorityError);
            }
        }

        public static void UpdateStatusXMLINBOUND_RA_MCE_DTLS(int fileID, string status, string errorDetails, int operation)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("INBOUND_RA_MCE_DTLS_ID", DbType.Int32, fileID, true));
                parameters.Add(SqlParms.CreateParameter("STATUS", DbType.String, status, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_DETAILS", DbType.String, errorDetails, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, operation, true));

                DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_RA_MCE_DTLS", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateINBOUND_RA_MCE_DTLS", String.Format("RemittanceAdviceHelper Exception: {0} {1}", ex.Message, ex.StackTrace), CON.RemittanceAdviceMCE.LogPriorityError);
            }
        }

        public static void UpdateControlFileNameINBOUND_RA_MCE_DTLS(int fileID, string controlFileName, int operation)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("INBOUND_RA_MCE_DTLS_ID", DbType.Int32, fileID, true));
                parameters.Add(SqlParms.CreateParameter("INCOMING_RA_CONTROL_FILE_NAME", DbType.String, controlFileName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, operation, true));

                DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_RA_MCE_DTLS", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateINBOUND_RA_MCE_DTLS", String.Format("RemittanceAdviceHelper Exception: {0} {1}", ex.Message, ex.StackTrace), CON.RemittanceAdviceMCE.LogPriorityError);
            }
        }

        public static void UpdateProcessDocDtlsINBOUND_RA_MCE_DTLS(int fileID, int processDoc, string processFileName, int operation)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("INBOUND_RA_MCE_DTLS_ID", DbType.Int32, fileID, true));
                parameters.Add(SqlParms.CreateParameter("INCOMING_RA_PROCESS_DOCUMENT_ID", DbType.Int32, processDoc, true));
                parameters.Add(SqlParms.CreateParameter("INCOMING_RA_PROCESS_FILE_NAME", DbType.String, processFileName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.RemittanceAdviceMCE.RemittanceAdviceMCEAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, operation, true));

                DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_RA_MCE_DTLS", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateINBOUND_RA_MCE_DTLS", String.Format("RemittanceAdviceHelper Exception: {0} {1}", ex.Message, ex.StackTrace), CON.RemittanceAdviceMCE.LogPriorityError);
            }
        }
    }
}
