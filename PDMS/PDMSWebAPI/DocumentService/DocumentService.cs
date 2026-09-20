using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;
using Microsoft.Owin;
using System.Security.Cryptography;
using static MAXIMUS.Core.Libraries.Constants;

namespace PDMSWebAPI.DocumentService
{
    public class DocumentService
    {
        string adminUser = CON.WebApiServiceGuid.DocumentService;
        static Guid DocumentServiceUIID = new Guid(CON.WebApiServiceGuid.DocumentService);

        Logging log = new Logging(DocumentServiceUIID, "PDMSWebAPI:AttachmentService");


        public string documentServiceParseXMLToStringReq(Logging log, SendAttachmentRequestMsg req)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SendAttachmentRequestMsg));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, req, emptyNs);
                string xml = stream2.ToString();

                var doc = XDocument.Parse(xml);
                var nodeToRemove = doc.Descendants().Where(o => o.Name.LocalName == "AttachmentData64Binary");
                nodeToRemove.Remove();
                xml = doc.ToString();

                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static string documentServiceParseXMLToStringRes(Logging log, SendAttachmentResponseMsg res)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SendAttachmentResponseMsg));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, res, emptyNs);
                string xml = stream2.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public int SaveMessageHeader(Guid reqID, SendAttachmentMessageHeader msgHeader, string sourceId, SendAttachmentRequestMsg reqRaw)
        {
            int retVal;
            try
            {
                string requestXML = documentServiceParseXMLToStringReq(log, reqRaw);
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REQUEST_ID", DbType.Guid, reqID, false));
                parameters.Add(SqlParms.CreateParameter("BusinessFlowName", DbType.String, msgHeader.BusinessFlow, false));
                parameters.Add(SqlParms.CreateParameter("StateCode", DbType.String, msgHeader.StateCode, false));
                parameters.Add(SqlParms.CreateParameter("ModuleTransactionId", DbType.String, msgHeader.ModuleTransactionId, false));
                parameters.Add(SqlParms.CreateParameter("AdditionalModuleTransactionId", DbType.String, msgHeader.AdditionalModuleTransactionId, true));
                parameters.Add(SqlParms.CreateParameter("RequestorSystem", DbType.String, msgHeader.RequestorSystem, false));
                parameters.Add(SqlParms.CreateParameter("SubscriberSystem", DbType.String, "PNM", false));
                parameters.Add(SqlParms.CreateParameter("RequestTimestamp", DbType.String, msgHeader.RequestTimestamp, false));
                parameters.Add(SqlParms.CreateParameter("SITransactionKey", DbType.String, msgHeader.SITransactionKey, false));
                parameters.Add(SqlParms.CreateParameter("REQUEST_PAYLOAD", DbType.String, requestXML, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("SourceId", DbType.String, sourceId, false));
                DataSet ds = new DataSet();
                retVal = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertUpdateStg_Message_Header", parameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public void saveDocumentServiceResponse(Guid reqID, SendAttachmentResponseMsg res)
        {
            try
            {
                string str = documentServiceParseXMLToStringRes(log, res);
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REQUEST_ID", DbType.Guid, reqID, false));
                parameters.Add(SqlParms.CreateParameter("RESPONSE_PAYLOAD", DbType.String, str, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, adminUser, false));
                DataAccess.ExecuteScalar("usp_InsertUpdateStg_Message_Header", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void saveInsertINBOUND_PA_NOTIF_DTLS(string idl, string lgn, string pid, string npi, string pan,
            string rid, string docName, int docID, string siKey)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("IDL", DbType.String, idl, false));
                parameters.Add(SqlParms.CreateParameter("LGN", DbType.String, lgn, false));
                parameters.Add(SqlParms.CreateParameter("PID", DbType.String, pid, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("PAN", DbType.String, pan, false));
                parameters.Add(SqlParms.CreateParameter("RID", DbType.String, rid, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_NAME", DbType.String, docName, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docID, false));
                parameters.Add(SqlParms.CreateParameter("SIKey", DbType.String, siKey, false));

                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, 1, false));
                DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_PA_NOTIF_DTLS", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void saveInsertINBOUND_HOSPICE_NOTIF_DTLS(string idl, string lgn, string pid, string npi, string htn,
            string rid, string docName, int docID, string siKey)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("IDL", DbType.String, idl, false));
                parameters.Add(SqlParms.CreateParameter("LGN", DbType.String, lgn, false));
                parameters.Add(SqlParms.CreateParameter("PID", DbType.String, pid, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("HTN", DbType.String, htn, false));
                parameters.Add(SqlParms.CreateParameter("RID", DbType.String, rid, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_NAME", DbType.String, docName, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docID, false));
                parameters.Add(SqlParms.CreateParameter("SIKey", DbType.String, siKey, false));

                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, 1, false));
                DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_HOSPICE_NOTIF_DTLS", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public AttachmentResponse UploadPriorAuthNotificationToDB(SendAttachment req, string SITransactionKey, int messageHeaderID)
        {
            AttachmentResponse saRes = new AttachmentResponse();
            string IDL = string.Empty;
            string LGN = string.Empty;
            string PAN = string.Empty;
            string RID = string.Empty;
            string PID = string.Empty;
            string NPI = string.Empty;
            try
            {
                foreach (SendAttachmentData saId in req.Identifiers)
                {
                    if (saId.DocXrefType.ToUpper().Equals("IDL"))
                    {
                        IDL = saId.IndexId;
                    }
                    if (saId.DocXrefType.ToUpper().Equals("LGN"))
                    {
                        LGN = saId.IndexId;
                    }
                    if (saId.DocXrefType.ToUpper().Equals("RID"))
                    {
                        RID = saId.IndexId;
                    }
                    if (saId.DocXrefType.ToUpper().Equals("PAN"))
                    {
                        PAN = saId.IndexId;
                    }
                    if (saId.DocXrefType.ToUpper().Equals("PID"))
                    {
                        PID = saId.IndexId;
                    }
                    if (saId.DocXrefType.ToUpper().Equals("NPI"))
                    {
                        NPI = saId.IndexId;
                    }
                }

                string fileName = System.IO.Path.GetFileNameWithoutExtension(req.DocumentName) + "." + req.DocumentExtension;
                int documentID = StoreDocumentRecord(fileName, fileName, string.Empty);

                if (documentID > 0)
                {
                    //UpdateOnbase.
                    string disableOnbaseServerLoad = AppSettings.Get("AttachmentServiceDisableOnBaseServerLoad", "false");
                    if (disableOnbaseServerLoad.ToLower() != "true")
                    {
                        SendUpdateToOnbase(fileName, documentID, req.AttachmentData64Binary);
                    }
                    SaveFileToFileStore(fileName, documentID, req.AttachmentData64Binary);

                    saveInsertINBOUND_PA_NOTIF_DTLS(IDL, LGN, PID, NPI, PAN, RID, fileName, documentID, SITransactionKey);

                    saRes.ResponseType = "Success";
                    saRes.ResponseCode = "1001";
                    saRes.ResponseMessage = "Prior Auth Letter Successfully Uploaded.";
                }
                else
                {
                    saRes.ResponseType = "Failure";
                    saRes.ResponseCode = "E06";
                    saRes.ResponseMessage = DocumentErrorCodes.E06;
                }
            }
            catch (Exception ex)
            {
                saRes.ResponseType = "Failure";
                saRes.ResponseCode = "E07";
                saRes.ResponseMessage = DocumentErrorCodes.E07 + " " + ex.Message;
            }
            return saRes;
        }

        public AttachmentResponse UploadHospiceNotificationToDB(SendAttachment req, string SITransactionKey, int messageHeaderID)
        {
            AttachmentResponse saRes = new AttachmentResponse();
            string IDL = string.Empty;
            string LGN = string.Empty;
            string HTN = string.Empty;
            string RID = string.Empty;
            string PID = string.Empty;
            string NPI = string.Empty;
            try
            {
                foreach (SendAttachmentData saId in req.Identifiers)
                {
                    if (saId.DocXrefType.ToUpper().Equals("IDL"))
                    {
                        IDL = saId.IndexId;
                    }
                    if (saId.DocXrefType.ToUpper().Equals("LGN"))
                    {
                        LGN = saId.IndexId;
                    }
                    if (saId.DocXrefType.ToUpper().Equals("RID"))
                    {
                        RID = saId.IndexId;
                    }
                    if (saId.DocXrefType.ToUpper().Equals("HTN"))
                    {
                        HTN = saId.IndexId;
                    }
                    if (saId.DocXrefType.ToUpper().Equals("PID"))
                    {
                        PID = saId.IndexId;
                    }
                    if (saId.DocXrefType.ToUpper().Equals("NPI"))
                    {
                        NPI = saId.IndexId;
                    }
                }

                string fileName = System.IO.Path.GetFileNameWithoutExtension(req.DocumentName) + "." + req.DocumentExtension;
                int documentID = StoreDocumentRecord(fileName, fileName, string.Empty);

                if (documentID > 0)
                {
                    //UpdateOnbase.
                    string disableOnbaseServerLoad = AppSettings.Get("AttachmentServiceDisableOnBaseServerLoad", "false");
                    if (disableOnbaseServerLoad.ToLower() != "true")
                    {
                        SendUpdateToOnbase(fileName, documentID, req.AttachmentData64Binary);
                    }
                    SaveFileToFileStore(fileName, documentID, req.AttachmentData64Binary);

                    saveInsertINBOUND_HOSPICE_NOTIF_DTLS(IDL, LGN, PID, NPI, HTN, RID, fileName, documentID, SITransactionKey);

                    saRes.ResponseType = "Success";
                    saRes.ResponseCode = "1001";
                    saRes.ResponseMessage = "Hospice Letter Successfully Uploaded.";
                }
                else
                {
                    saRes.ResponseType = "Failure";
                    saRes.ResponseCode = "E06";
                    saRes.ResponseMessage = DocumentErrorCodes.E06;
                }
            }
            catch (Exception ex)
            {
                saRes.ResponseType = "Failure";
                saRes.ResponseCode = "E07";
                saRes.ResponseMessage = DocumentErrorCodes.E07 + " " + ex.Message;
            }
            return saRes;
        }

        public AttachmentResponse UploadDocumentToDB(SendAttachment req, string SITransactionKey, int messageHeaderID)
        {
            AttachmentResponse saRes = new AttachmentResponse();
            string request = string.Empty;
            try
            {
                if(req != null)
                {
                    string requestStringJson = JsonConvert.SerializeObject(req);
                   //Limiting the request length to DB field Length                  
                    request = requestStringJson.Length>1450? requestStringJson.Substring(0, 1450): requestStringJson;                   
                }
                int documentTypeId = 0;
                foreach (SendAttachmentData saId in req.Identifiers)
                {
                    if (saId.DocXrefType.ToUpper().Equals("RAN"))
                    {
                        documentTypeId = GetDocumentType(req.DocumentType, "-1");
                    }
                    if (saId.DocXrefType.ToUpper().Equals("DODDAPPID"))
                    {
                        documentTypeId = GetDocumentType(req.DocumentType, "-1");
                    }
                    if (saId.DocXrefType.ToUpper().Equals("PCWAPPID"))
                    {
                        documentTypeId = GetDocumentType(req.DocumentType, "-1");
                    }
                    if (saId.DocXrefType.ToUpper().Equals("IDL"))
                    {
                        documentTypeId = GetDocumentType(req.DocumentType, saId.IndexId);
                    }
                }
                if (documentTypeId == 0)
                {
                    documentTypeId = GetDocumentType(req.DocumentType, req.Identifiers[0].IndexId);
                }

                if (documentTypeId > 0)
                {
                    Dictionary<string, int> docXrefTypeWithId = GetDocumentXrefTypeByCode(req);

                    if (docXrefTypeWithId.Count > 0)
                    {
                        
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(req.DocumentName) + "." + req.DocumentExtension;

                        int documentID = StoreDocumentRecord(fileName, fileName, string.Empty);


                        if (documentID > 0)
                        {
                            string lgnDateStr = "";
                            string ran = "";
                            string pid = "";
                            string npi = "";
                            DateTime lgnDate = DateTime.Now;
                            DateTime ra_date = System.DateTime.Now;
                            foreach (SendAttachmentData saId in req.Identifiers)
                            {
                                int docXrefId = 0;
                                docXrefTypeWithId.TryGetValue(saId.DocXrefType, out docXrefId);
                                
                                bool retVal = StoreDocumentAttachmentIdentifiers(docXrefId, saId.IndexId, documentID);
                                
                                if (saId.DocXrefType.ToUpper().Equals("LGN"))
                                {
                                    lgnDateStr = saId.IndexId;
                                    // read LGN into Datetime, if we cannot, use load time
                                    if (!DateTime.TryParse(saId.IndexId, out ra_date))
                                    {
                                        ra_date = System.DateTime.Now;
                                    }
                                    if (!DateTime.TryParse(lgnDateStr, out lgnDate))
                                    {
                                        lgnDate = DateTime.Now;
                                    }
                                }    
                                if (saId.DocXrefType.ToUpper().Equals("PID"))
                                {
                                    pid = saId.IndexId;
                                }
                                if (saId.DocXrefType.ToUpper().Equals("NPI"))
                                {
                                    npi = saId.IndexId;
                                }
                                if (saId.DocXrefType.ToUpper().Equals("RAN"))
                                {
                                    ran = saId.IndexId;
                                }
                            }
                            int attachmnetXrefId = StoreDocumentAttachmnetXref(req, lgnDate, documentID, SITransactionKey, documentTypeId, 0, messageHeaderID);
                            if (!string.IsNullOrEmpty(ran))
                            {
                                var racSaved = SaveRemittanceAdviceControl(ran, pid, npi, ra_date, req.DocumentExtension, fileName, documentID, SITransactionKey);
                            }

                            saRes.ResponseType = "Success";
                            saRes.ResponseCode = string.Empty;
                            saRes.ResponseMessage = "Successfully Updated Document";

                            //UpdateOnbase.
                            string disableOnbaseServerLoad = AppSettings.Get("AttachmentServiceDisableOnBaseServerLoad", "false");
                            if (disableOnbaseServerLoad.ToLower() != "true")
                            {
                                SendUpdateToOnbase(fileName, documentID, req.AttachmentData64Binary);
                            }
                            SaveFileToFileStore(fileName, documentID, req.AttachmentData64Binary);
                        }
                        else
                        {
                            saRes.ResponseType = "Failure";
                            saRes.ResponseCode = "E06";
                            saRes.ResponseMessage = DocumentErrorCodes.E06;
                        }
                    }
                    else
                    {
                        saRes.ResponseType = "Failure";
                        saRes.ResponseCode = "E05";
                        saRes.ResponseMessage = DocumentErrorCodes.E05;
                    }
                }
                else
                {
                    saRes.ResponseType = "Failure";
                    saRes.ResponseCode = "E10";
                    saRes.ResponseMessage = DocumentErrorCodes.E10;
                }
            }
            catch (Exception ex)
            {
                string logmessage = string.Format("Exception while storing the Data in SendAttachment  {0}", ex.ToString());
                string logRequest = string.Format("SendAttachment request : {0}", request);

                log.CreateLogEntry(logmessage, Logging.LogPriority.Error);
                log.CreateLogEntry(logRequest, Logging.LogPriority.Error);
                saRes.ResponseType = "Failure";
                saRes.ResponseCode = "E07";
                saRes.ResponseMessage = DocumentErrorCodes.E07;
            }


            return saRes;
        }

        public bool CheckValidSSAIdentifiers(int moduleTransactionID, int SSAAppID, int pnmAppID, int regID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("ModuleTransactionID", DbType.Int32, moduleTransactionID, true));
            parameters.Add(SqlParms.CreateParameter("SSAAppID", DbType.Int32, SSAAppID, true));
            parameters.Add(SqlParms.CreateParameter("PNMAppID", DbType.Int32, pnmAppID, true));
            parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
            DataSet dsType = DataAccess.ExecuteStoredProcedure("usp_CheckSSAIdentifiersAttachment", parameters, "CheckSSAIdentifiersAttachment");
            if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public int GetPSMDocumentType(string documentType)
        {

            int retVal = 0;
            DataSet dsType = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_CODE", DbType.String, documentType, false));
                dsType = DataAccess.ExecuteStoredProcedure("usp_SelectDOCUMENT_TYPE_ByCode", parameters, "DocumentTypes");
                if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                    retVal = Convert.ToInt32(dsType.Tables[0].Rows[0]["DOCUMENT_TYPE_ID"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;


            }

            return retVal;
        }

        private void SaveFileToFileStore(string fileName, int documentID, byte[] attachmentData64Binary)
        {
            try                
            {
                string DestinationPath = AppSettings.Get("OnBaseDocUpload-ImportLocalPath", string.Empty);
                fileName = documentID + "_" + fileName;
                string totalFileName = Path.Combine(DestinationPath + fileName);
                File.WriteAllBytes(totalFileName, attachmentData64Binary);
            }
            catch(Exception ex)
            {
                string logmessage = string.Format("Exception while storing the file in SendAttachment file:{0},{1},{2}", fileName, documentID, ex.ToString());
                log.CreateLogEntry(logmessage, Logging.LogPriority.Error);
            }
        }

        private int StoreDocumentRecord(string name, string filename, string desc)
        {

            int retVal = 0;
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, false));
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, desc, false));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, filename, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, adminUser, false));

                DataSet ds = new DataSet();
                retVal = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT", parameters));


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }
        private int StoreDocumentAttachmnetXref(SendAttachment sa, DateTime dteReceived, int documentID, string SITransactionKey, int documentTypeId, int RetrieveReportTypeId = 0,int msgID=0)
        {
            string headerId = string.Empty;
            int retVal = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, false));
                parameters.Add(SqlParms.CreateParameter("NOTES", DbType.String, string.Empty, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_RECEIVED_DATE", DbType.DateTime, dteReceived, false));
                parameters.Add(SqlParms.CreateParameter("SITRANSACTIONKEY", DbType.String, SITransactionKey, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_ID", DbType.Int32, documentTypeId, false));
                parameters.Add(SqlParms.CreateParameter("RetrieveReport_Type_ID", DbType.Int32, RetrieveReportTypeId, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_MODIFIED_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("STG_MESSAGE_HEADER_ID", DbType.Int32, msgID, false));
                DataSet ds = new DataSet();
                retVal = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT_ATTACHMENT_XREF", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }
        private bool StoreDocumentAttachmentIdentifiers(int xrefTypeID, string IndexId, int documentID)
        {
            string headerId = string.Empty;
            int retVal = 0;
            bool bretval = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_XREF_TYPE_ID", DbType.Int32, xrefTypeID, false));
                parameters.Add(SqlParms.CreateParameter("INDEXID", DbType.String, IndexId.Trim(), false));
                parameters.Add(SqlParms.CreateParameter("CREATED_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_MODIFIED_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, adminUser, false));


                retVal = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT_INDEX", parameters));

                if (retVal > 0) bretval = true;

            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to store Document Attachment Identifiers "
                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
                bretval = false;
            }

            return bretval;
        }

        private bool SaveRemittanceAdviceControl(string raNumber, string providerId, string npi, DateTime ra_date, string docType, string docName, int documentId, string siTransactionKey)
        {
            bool bretval = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("edi_transaction_type", DbType.String, "RA", false));
                parameters.Add(SqlParms.CreateParameter("tax_id", DbType.String, "1", false));
                parameters.Add(SqlParms.CreateParameter("ra_number", DbType.String, raNumber.Trim(), false));
                parameters.Add(SqlParms.CreateParameter("payment_identifier", DbType.String, "1", false));
                parameters.Add(SqlParms.CreateParameter("provider_id", DbType.String, providerId.Trim(), false));
                parameters.Add(SqlParms.CreateParameter("provider_npi", DbType.String, npi.Trim(), false));
                parameters.Add(SqlParms.CreateParameter("sender_id", DbType.String, "1", false));
                parameters.Add(SqlParms.CreateParameter("receiver_id", DbType.String, "1", false));
                parameters.Add(SqlParms.CreateParameter("document_type", DbType.String, docType.Trim(), false));
                parameters.Add(SqlParms.CreateParameter("ra_date", DbType.DateTime, ra_date, false));
                parameters.Add(SqlParms.CreateParameter("time_stamp", DbType.DateTime, System.DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("document_name", DbType.String, docName.Trim(), false));
                parameters.Add(SqlParms.CreateParameter("uuid", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("document_id", DbType.Int32, documentId, false));
                parameters.Add(SqlParms.CreateParameter("si_transaction_key", DbType.String, siTransactionKey, true));
                var controlID = Convert.ToInt32(DataAccess.ExecuteScalar("insertREMITTANCE_ADVICE_CONTROL", parameters));

                if (controlID > 0) bretval = true;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to save Remittance Advice Control record "
                                 + " Exception Message: " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
                bretval = false;
            }

            return bretval;
        }

        private int GetDocumentType(string documentType, string letterId)
        {

            int retVal = 0;
            DataSet dsType = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_CODE", DbType.String, documentType, false));
                parameters.Add(SqlParms.CreateParameter("LETTER_ID", DbType.String, letterId, false));
                dsType = DataAccess.ExecuteStoredProcedure("usp_SelectDOCUMENT_TYPE_ByCode", parameters, "DocumentTypes");
                if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                    retVal = Convert.ToInt32(dsType.Tables[0].Rows[0]["DOCUMENT_TYPE_ID"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;


            }

            return retVal;
        }

        private int GetDocumentXrefType(string documentXrefType)
        {

            int retVal = 0;
            DataSet dsType = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_XREF_TYPE", DbType.String, documentXrefType, false));
                dsType = DataAccess.ExecuteStoredProcedure("usp_SelecDOCUMENT_XREF_TYPE_ByType", parameters, "DocumentXrefTypes");
                if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                    retVal = Convert.ToInt32(dsType.Tables[0].Rows[0]["DOCUMENT_XREF_TYPE_ID"].ToString());
            }
            catch (Exception ex)
            {
                 throw ex;


            }

            return retVal;
        }

        private Dictionary<string, int> GetDocumentXrefTypeByCode(SendAttachment sa)
        {
            Dictionary<string, int> dXrefTypeIdCode = new Dictionary<string, int>();

            foreach (SendAttachmentData sda in sa.Identifiers)
            {
                string docXrefType = sda.DocXrefType;
                int docXrefTypeId = GetDocumentXrefType(docXrefType);
                if (docXrefTypeId > 0)
                {
                    if (!dXrefTypeIdCode.ContainsKey(docXrefType))
                        dXrefTypeIdCode.Add(docXrefType, docXrefTypeId);
                }
                else
                {
                    dXrefTypeIdCode.Clear(); //clearing the entries to check for invalid
                    break;
                }
            }


            return dXrefTypeIdCode;
        }
        private void SendUpdateToOnbase(string fileName, int documentId, byte[] attachmentBytes)
        {
            try
            {
                OnBaseInterface onBaseInterface = new OnBaseInterface();
                onBaseInterface.SubmitFile(documentId, attachmentBytes, fileName);
            }
            catch(Exception ex)
            {
                string logmessage = string.Format("Exception while sending SendAttachment file to OnBase:{0},{1},{2}", fileName, documentId, ex.ToString());
                log.CreateLogEntry(logmessage, Logging.LogPriority.Error);
            }
        }
        private void SendUpdateToOnbase(string fileName, int documentId)
        {
            //We are not sending any failure to service clients if onbase upload fails.
            //As BATCH JOB will pick which are not updated to Onbase.
            string DestinationPath = AppSettings.Get("FileStorePathWeb", string.Empty);

            try
            {
                if (!string.IsNullOrEmpty(DestinationPath))
                {
                    string fullFileName = Path.Combine(DestinationPath, fileName);
                    if (File.Exists(fullFileName))
                    {
                        byte[] bytes = File.ReadAllBytes(fullFileName);                      
                        //byte[] bytesEncrypted = EncryptedFileBytes(bytes);

                        OnBaseInterface onBaseInterface = new OnBaseInterface();
                        onBaseInterface.SubmitFile(documentId, bytes, fileName);

                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public byte[] EncryptedFileBytes(byte[] bytesInput)
        {
            Encryption encryption = new Encryption();
            byte[] bytesEncrypted = encryption.EncryptRijndael(bytesInput);

            return bytesEncrypted;
        }
    }
}