using MAXIMUS.Core.Libraries;
using PDMSWebAPI.DocumentService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;


using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReceiveDocumentsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReceiveDocumentsService.svc or ReceiveDocumentsService.svc.cs at the Solution Explorer and start debugging.

    public class AttachmentService : IAttachmentService
    {
        Logging Log;
        Guid DocumentServiceUIID = new Guid(CON.WebApiServiceGuid.DocumentService);

        public AttachmentService()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                Log = new Logging(DocumentServiceUIID, "PDMSWebAPI:AttachmentService");
            }
            else
            {
                Log = new Logging(user.UserThreadId, "PDMSWebAPI:AttachmentService");
            }
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header.Replace("\"", "").Trim());
                }
                while (!sr.EndOfStream)
                {
                    string[] dataElements = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = dataElements[i].Replace("\"", "").Trim();
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public SendAttachmentResponseMsg SendAttachment(SendAttachmentRequestMsg request)
        {
            Guid requestID = Guid.NewGuid();
            Log.CreateLogEntry("Calling SendAttachment", Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:AttachmentService", "WebAPI:AttachmentService-SendAttachment");
            PDMSWebAPI.DocumentService.DocumentService obj = new PDMSWebAPI.DocumentService.DocumentService();
            SendAttachmentResponseMsg robj = new SendAttachmentResponseMsg();
            List<AttachmentResponse> objSendRes = new List<AttachmentResponse>();
            try
            {
                int SSAAppID = -1;
                int pnmAppID = -1;
                int regID = -1;
                string PID = string.Empty;
                string NPI = string.Empty;
                string RAN = string.Empty;
                int moduleTransactionID = 0;
                int documentTypeId = 0;
                bool validPSM = true;
                int messageHeaderId = 0;
                messageHeaderId = obj.SaveMessageHeader(requestID, request.MessageHeader, request.Payload.AttachmentInfo.SourceId, request);
                robj.SITransactionKey = (request == null) ? string.Empty : ((request.MessageHeader != null) ? request.MessageHeader.SITransactionKey : string.Empty);
                robj.ModuleTransactionId = (request == null) ? string.Empty : ((request.MessageHeader != null) ? request.MessageHeader.ModuleTransactionId : string.Empty);
                if (Int32.TryParse(robj.ModuleTransactionId, out moduleTransactionID)) { }
                if (request != null &&
                    (ValidateRequiredMessagePayloadFields(request) &&
                    isValidBusinessFlow(request.MessageHeader.BusinessFlow) &&
                    isValidSystem(request.MessageHeader.RequestorSystem, true) &&
                    isValidSubscriberSystem(request.MessageHeader.SubscriberSystem, false)) && request.MessageHeader.StateCode.ToString().ToUpper() == "OH")
                {

                    if (isValidFileSize(request))
                    {
                        int Counter = 1;
                        foreach (SendAttachment saObj in request.Payload.AttachmentInfo.AttachmentData)
                        {
                            AttachmentResponse saResponse = new AttachmentResponse();

                            if (ValidateExtension(saObj.DocumentExtension))
                            {
                                if (request.MessageHeader.RequestorSystem.ToUpper().Equals("PSM") || request.MessageHeader.RequestorSystem.ToUpper().Equals("PCW"))
                                {
                                    validPSM = true;
                                    foreach (SendAttachmentData saId in saObj.Identifiers)
                                    {
                                        if (saId.DocXrefType.ToUpper().Equals("DODDAPPID") && request.MessageHeader.RequestorSystem.ToUpper().Equals("PSM"))
                                        {
                                            if (Int32.TryParse(saId.IndexId, out SSAAppID)) { }
                                        }
                                        if (saId.DocXrefType.ToUpper().Equals("PCWAPPID") && request.MessageHeader.RequestorSystem.ToUpper().Equals("PCW"))
                                        {
                                            if (Int32.TryParse(saId.IndexId, out SSAAppID)) { }
                                        }
                                        if (saId.DocXrefType.ToUpper().Equals("PNMAPPID"))
                                        {
                                            if (Int32.TryParse(saId.IndexId, out pnmAppID)) { }
                                        }
                                        if (saId.DocXrefType.ToUpper().Equals("REGID"))
                                        {
                                            if (Int32.TryParse(saId.IndexId, out regID)) { }
                                        }
                                    }
                                    bool retVal = obj.CheckValidSSAIdentifiers(moduleTransactionID, SSAAppID, pnmAppID, regID);
                                    if (!retVal)
                                    {
                                        validPSM = false;
                                        saResponse.ResponseType = "Failure";
                                        saResponse.ResponseCode = "E11";
                                        saResponse.ResponseMessage = DocumentErrorCodes.E11.ToString();
                                    }
                                    if (string.IsNullOrEmpty(saObj.DocumentType) || string.IsNullOrEmpty(saObj.DocumentName) || string.IsNullOrEmpty(saObj.DocumentExtension)
                                        || saObj.AttachmentData64Binary == null)
                                    {
                                        validPSM = false;
                                        saResponse.ResponseType = "Failure";
                                        saResponse.ResponseCode = "E11";
                                        saResponse.ResponseMessage = DocumentErrorCodes.E12.ToString();
                                    }
                                    documentTypeId = obj.GetPSMDocumentType(saObj.DocumentType);
                                    if (documentTypeId == 0)
                                    {
                                        validPSM = false;
                                        saResponse.ResponseType = "Failure";
                                        saResponse.ResponseCode = "E11";
                                        saResponse.ResponseMessage = DocumentErrorCodes.E13.ToString();
                                    }
                                    if (validPSM)
                                    {
                                        saResponse = obj.UploadDocumentToDB(saObj, request.MessageHeader.SITransactionKey, messageHeaderId);
                                    }
                                }
                                else
                                {
                                    bool isPriorAuthNotification = false;
                                    bool isHospiceNotification = false;
                                    foreach (SendAttachmentData saId in saObj.Identifiers)
                                    {
                                        if (saId.DocXrefType.ToUpper().Equals("IDL"))
                                        {
                                            if (!isPriorAuthNotification)
                                            {
                                                if (isPriorAuthNotificationCheck(saId.IndexId))
                                                {
                                                    isPriorAuthNotification = true;
                                                }
                                            }
                                            if (!isHospiceNotification)
                                            {
                                                if (isHospiceNotificationCheck(saId.IndexId))
                                                {
                                                    isHospiceNotification = true;
                                                }
                                            }
                                        }
                                        if (saId.DocXrefType.ToUpper().Equals("PID"))
                                        {
                                            PID = saId.IndexId;
                                        }
                                        if (saId.DocXrefType.ToUpper().Equals("NPI"))
                                        {
                                            NPI = saId.IndexId;                                            
                                        }
                                        if (saId.DocXrefType.ToUpper().Equals("RAN"))
                                        {
                                            RAN = saId.IndexId;
                                        }
                                    }

                                    int regId = GetRegIdByMedicaidIdOrNPI(PID, NPI);
                                    if (!(regID > 0) && AppSettings.Get("AttachmentServiceCrossReferenceEnabled", string.Empty).Equals("true") )
                                    {
                                        string crossReferencePath = AppSettings.Get("AttachmentServiceCrossReferenceLocation", string.Empty);
                                        if (!string.IsNullOrEmpty(crossReferencePath))
                                        {
                                            if (File.Exists(crossReferencePath))
                                            {
                                                DataTable dt = ConvertCSVtoDataTable(crossReferencePath);
                                                foreach (DataRow dr in dt.Rows)
                                                {
                                                    if (dr["PID"].Equals(PID)) {
                                                        PID = dr["ID_PROVIDER"].ToString();
                                                        regId = GetRegIdByMedicaidIdOrNPI(PID, NPI);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (regId > 0)
                                    {
                                        if (!string.IsNullOrEmpty(RAN) && request.MessageHeader.RequestorSystem.ToString().Equals("MITS") && saObj.DocumentType.ToString().Equals("050"))
                                        {
                                            if (saObj.DocumentExtension.ToString().ToUpper().Equals("PDF") || saObj.DocumentExtension.ToString().ToUpper().Equals("TXT"))
                                            {
                                                saResponse = obj.UploadDocumentToDB(saObj, request.MessageHeader.SITransactionKey, messageHeaderId);

                                                if (GetNotifyProvider(saObj.DocumentType))
                                                {
                                                    string GenericTemplatePath = AppSettings.Get("TemplateFilePath", string.Empty);
                                                    string body = string.Empty;
                                                    GenericTemplatePath += "//GenericNoticeTemplate.txt";
                                                    string recipients = GetRecipients(1, regId);

                                                    EMailNotification notification = new EMailNotification(body, "Provider Notice", recipients);

                                                    notification.SendGenericNotification(GenericTemplatePath, regId.ToString(), true);
                                                }
                                            }
                                            else
                                            {
                                                saResponse.ResponseType = "Failure";
                                                saResponse.ResponseCode = "E10";
                                                saResponse.ResponseMessage = DocumentErrorCodes.E10.ToString();
                                                Log.CreateLogEntry("SendAttachment failure: " + saResponse.ResponseMessage, Logging.LogPriority.Error);
                                            }
                                        }
                                        else
                                        {
                                            //Prior Auth Notifications
                                            if (isPriorAuthNotification)
                                            {
                                                saResponse = obj.UploadPriorAuthNotificationToDB(saObj, request.MessageHeader.SITransactionKey, messageHeaderId);
                                            }//Hospice Notifications
                                            else if (isHospiceNotification)
                                            {
                                                saResponse = obj.UploadHospiceNotificationToDB(saObj, request.MessageHeader.SITransactionKey, messageHeaderId);
                                            }
                                            else
                                            {
                                                saResponse = obj.UploadDocumentToDB(saObj, request.MessageHeader.SITransactionKey, messageHeaderId);
                                            }

                                            if (GetNotifyProvider(saObj.DocumentType))
                                            {
                                                string GenericTemplatePath = AppSettings.Get("TemplateFilePath", string.Empty);
                                                string body = string.Empty;
                                                GenericTemplatePath += "//GenericNoticeTemplate.txt";
                                                string recipients = GetRecipients(1, regId);

                                                EMailNotification notification = new EMailNotification(body, "Provider Notice", recipients);

                                                notification.SendGenericNotification(GenericTemplatePath, regId.ToString(), true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        saResponse.ResponseType = "Failure";
                                        saResponse.ResponseCode = "E14";
                                        saResponse.ResponseMessage = DocumentErrorCodes.E14.ToString();
                                        Log.CreateLogEntry("SendAttachment failure: " + saResponse.ResponseMessage, Logging.LogPriority.Error);
                                    }
                                }
                            }
                            else
                            {
                                saResponse.ResponseType = "Failure";
                                saResponse.ResponseCode = "E01";
                                saResponse.ResponseMessage = DocumentErrorCodes.E01.ToString();

                            }
                            saResponse.IndexId = Counter.ToString();
                            objSendRes.Add(saResponse);
                            Counter++;
                        }
                        Log.CreateLogEntry("SendAttachment successful", Logging.LogPriority.Information);
                    }
                    else
                    {
                        AttachmentResponse saResponse = new AttachmentResponse();
                        saResponse.IndexId = "";
                        saResponse.ResponseType = "Failure";
                        saResponse.ResponseCode = "E09";
                        saResponse.ResponseMessage = DocumentErrorCodes.E09.ToString();
                        objSendRes.Add(saResponse);

                        Log.CreateLogEntry("SendAttachment failure: " + saResponse.ResponseMessage, Logging.LogPriority.Error);
                    }

                }
                else
                {
                    AttachmentResponse saResponse = new AttachmentResponse();
                    saResponse.IndexId = "";
                    saResponse.ResponseType = "Failure";
                    saResponse.ResponseCode = "E08";
                    saResponse.ResponseMessage = DocumentErrorCodes.E08.ToString();
                    objSendRes.Add(saResponse);

                    Log.CreateLogEntry("SendAttachment failure: " + saResponse.ResponseMessage, Logging.LogPriority.Error);
                }
                Log.CreateLogEntry("SendAttachment call end", Logging.LogPriority.Information);
                robj.Response = objSendRes.ToArray();
                obj.saveDocumentServiceResponse(requestID, robj);
                return robj;
            }
            catch (Exception ex)
            {
                Log.CreateLogEntry(string.Format("Exception in SendAttachment  {0}: {1}", ex.Message, ex.ToString()), Logging.LogPriority.Error);

                Log.CreateLogEntry("SendAttachment call end", Logging.LogPriority.Information);
                AttachmentResponse saResponse = new AttachmentResponse();
                saResponse.IndexId = "";
                saResponse.ResponseType = "Failure";
                saResponse.ResponseCode = "E02";
                saResponse.ResponseMessage = DocumentErrorCodes.E02.ToString();
                objSendRes.Add(saResponse);
                robj.Response = objSendRes.ToArray();
                obj.saveDocumentServiceResponse(requestID, robj);
                return robj;
            }

        }

        private string GetRecipients(int sendToTypeID, int regId)
        {
            StringBuilder addrList = new StringBuilder();
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SendToTypeID", DbType.Int32, sendToTypeID, false));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters, "EmailRecipients");
                if (Methods.HasRows(ds))
                {
                    addrList = Methods.AddEmail(ds.Tables[0], "EmailAddress");
                }
            }
            catch (Exception ex)
            {
                Log.CreateLogEntry("Error in Attachment service in sending email to Generic notification recipients: " + ex.Message + ex.StackTrace.ToString(), Logging.LogPriority.Error);
            }

            return addrList.ToString();
        }

        private static int GetRegIdByMedicaidIdOrNPI(string medicaidId, string NPI)
        {
            try
            {
                int regId = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidId.Trim(), false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI.Trim(), false));
                DataSet ds= DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONsByMedicaidIdAndNPI", parameters, "RegData");
                if (Methods.HasRows(ds))
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {     
                       regId = row["RegID"] != null ? Convert.ToInt32(row["RegID"]) : 0;   
                    }
                }
                return regId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveBinaryContentToFiles(SendAttachmentRequestMsg request, string totalFilePath, string SITransactionKey)
        {
            try
            {
                string DestinationPath = AppSettings.Get("FileStorePathWeb", string.Empty);

                foreach (SendAttachment sa in request.Payload.AttachmentInfo.AttachmentData)
                {
                    if (sa != null && !string.IsNullOrEmpty(DestinationPath))
                    {

                        string fileName = sa.DocumentName;
                        string fileExtension = sa.DocumentExtension;
                        string fileNameFinal = SITransactionKey + "_" + fileName + "." + fileExtension;

                        string decodedInput = DecodeBase64string(sa.AttachmentData64Binary);

                        byte[] fileBytes = Encoding.ASCII.GetBytes(decodedInput);


                        //using (var fs = new FileStream(fileNameFinal, FileMode.Create, FileAccess.Write))
                        //{
                        //    fs.Write(fileBytes, 0, fileBytes.Length);

                        //}
                        PDMSWebAPI.DocumentService.DocumentService obj = new PDMSWebAPI.DocumentService.DocumentService();


                        byte[] bytesEncrypted = obj.EncryptedFileBytes(fileBytes);
                        File.WriteAllBytes(Path.Combine(DestinationPath, fileNameFinal), bytesEncrypted);

                    }


                }


            }
            catch (Exception ex)
            {
                //here possible will get more errors based on types need to add Logic TBD

                Log.CreateLogEntry("Failed to Save Binary content to files "
                                         + " Exception Message " + ex.Message + " Exception Stack = "
                                         + ex.StackTrace, Logging.LogPriority.Error);
            }
        }

        private string DecodeBase64string(byte[] fileBytes)
        {
            string strDecoded = string.Empty;
            if (fileBytes != null)
            {

                //var base64EncodedBytes = System.Convert.FromBase64String(encodeBase64String);
                strDecoded = System.Text.Encoding.UTF8.GetString(fileBytes);


            }

            return strDecoded;
        }

        private bool ValidateExtension(string extension)
        {
            bool rValue = false;
            string[] extensions = new string[] { "doc", "docx", "xls", "XLS", "xlsx", "xlsm", "XLSX", "ppt", "pptx", "mdi", "jpe", "jpeg", "png", "bmp", "tif", "tiff", "pdf", "pi", "ec", "zip", "csv", "XLSM", "Arcbak", "txt", "TXT" };

            if (!string.IsNullOrEmpty(extension))
            {

                if (extensions.Contains(extension.Trim(), StringComparer.OrdinalIgnoreCase))
                {
                    rValue = true;
                }

            }

            return rValue;
        }
        private int GetFileSizeInBytes(string base64FileString)
        {
            int fileSizeBytes = 0;
            int n = 0;
            int y = 1;


            // x = (n * (3 / 4)) - y
            // x is the size of a file in bytes
            // n is the length of the Base64 String
            //y will be 2 if Base64 ends with '==' and 1 if Base64 ends with '='.
            RolesAuthentication.Allow("WebAPI:AttachmentService", "WebAPI:AttachmentService-SaveBinaryContentToFiles");
            try
            {


                if (!string.IsNullOrEmpty(base64FileString))
                {
                    if (base64FileString.EndsWith("=="))
                    {
                        y = 2;
                    }
                    n = base64FileString.Length;

                    fileSizeBytes = (n * (3 / 4)) - y;

                }

            }
            catch (Exception ex)
            {
                Log.CreateLogEntry("Failed to Get File Size in Bytes "
                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
            }

            return fileSizeBytes;
        }

        private bool isValidFileSize(SendAttachmentRequestMsg request)
        {
            bool retVal = true;
            int maxBytes = 1310720000; //1250 MB in binary bytes
            if (request != null && request.Payload != null && request.Payload.AttachmentInfo != null && request.Payload.AttachmentInfo.AttachmentData != null)
            {
                int totalFileSize = 0;
                int fileSize = 0;
                foreach (SendAttachment sa in request.Payload.AttachmentInfo.AttachmentData)
                {
                    if (sa.AttachmentData64Binary != null)
                    {
                        fileSize = sa.AttachmentData64Binary.Length;   //GetFileSizeInBytes(Encoding.UTF8.GetString(sa.AttachmentData64Binary, 0, sa.AttachmentData64Binary.Length));
                        totalFileSize = totalFileSize + fileSize;
                        fileSize = 0;
                    }
                }
                if (totalFileSize > maxBytes)
                {
                    retVal = false;
                }
            }


            return retVal;
        }


        private bool ValidateRequiredMessagePayloadFields(SendAttachmentRequestMsg request)
        {
            bool retVal = false;
            //first Check the Message header parameters

            if (request != null && request.MessageHeader != null)
            {
                if (request.MessageHeader.StateCode != null && request.MessageHeader.RequestorSystem != null && request.MessageHeader.BusinessFlow != null && request.MessageHeader.RequestTimestamp != null)
                {
                    //so had required header info, now for payload
                    if (request.Payload != null && request.Payload.AttachmentInfo != null && request.Payload.AttachmentInfo.SourceId != null && request.Payload.AttachmentInfo.AttachmentData != null)
                    {
                        retVal = ValidateSendAttachmentRequiredFields(request.Payload.AttachmentInfo.AttachmentData);

                    }
                }
            }

            return retVal;
        }

        private bool ValidateSendAttachmentRequiredFields(SendAttachment[] sendAttachments)
        {
            bool retVal = false;
            bool hasfailedIdentiferscheck = false;

            foreach (SendAttachment sa in sendAttachments)
            {
                if (sa.Identifiers != null && sa.AttachmentData64Binary != null && sa.DocumentExtension != null && sa.DocumentName != null && sa.DocumentType != null)
                {
                    foreach (SendAttachmentData sda in sa.Identifiers)
                    {
                        if (sda.IndexId != null && sda.DocXrefType != null)
                        {
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                            hasfailedIdentiferscheck = true;
                            break;
                        }
                    }
                }
                else
                {
                    retVal = false;
                    break;
                }
                if (hasfailedIdentiferscheck)
                    break;
            }

            return retVal;
        }

        private bool isValidSystem(string systemObj, bool required)
        {
            bool retVal = false;

            if (!string.IsNullOrEmpty(systemObj))
            {
                systemObj = systemObj.ToUpper().Trim();
                if (systemObj == "PCW" || systemObj == "MITS" || systemObj == "PSM" || systemObj == "HAVEN" || systemObj == "PNM" || systemObj == "FI")
                {
                    retVal = true;
                }
            }
            else
            {
                retVal = !required;
            }

            return retVal;
        }

        private bool isValidSubscriberSystem(string[] systemObj, bool required)
        {
            bool retVal = false;

            if (systemObj.Contains("PNM"))
            {
                retVal = true;
            }
            else
            {
                retVal = !required;
            }

            return retVal;
        }

        private bool isValidBusinessFlow(string businessFlow)
        {
            bool retVal = false;

            if (!string.IsNullOrEmpty(businessFlow))
            {
                businessFlow = businessFlow.ToUpper().Trim();
                if (businessFlow == "SENDATTACHMENT")
                {
                    retVal = true;
                }
            }


            return retVal;
        }


        private bool isPriorAuthNotificationCheck(string letterID)
        {

            bool retVal = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LETTER_ID", DbType.String, letterID, false));
                DataSet dsType = DataAccess.ExecuteStoredProcedure("usp_IsPriorAuthNotificationCheck", parameters, "IsPriorAuthNotificationCheck");
                if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsType.Tables[0].Rows[0];
                    if (!dr.IsNull("IS_PRIOR_AUTH"))
                    {
                        retVal = Convert.ToBoolean(dr["IS_PRIOR_AUTH"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;


            }

            return retVal;
        }

        private bool isHospiceNotificationCheck(string letterID)
        {

            bool retVal = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LETTER_ID", DbType.String, letterID, false));
                DataSet dsType = DataAccess.ExecuteStoredProcedure("usp_IsHospiceNotificationCheck", parameters, "IsHospiceNotificationCheck");
                if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsType.Tables[0].Rows[0];
                    if (!dr.IsNull("IS_HOSPICE"))
                    {
                        retVal = Convert.ToBoolean(dr["IS_HOSPICE"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;


            }

            return retVal;
        }

        private bool GetNotifyProvider(string documentType)
        {

            bool retVal = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_CODE", DbType.String, documentType, false));
                DataSet dsType = DataAccess.ExecuteStoredProcedure("usp_SelectDOCUMENT_TYPE_ByCode", parameters, "DocumentTypes");
                if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsType.Tables[0].Rows[0];
                    if (!dr.IsNull("NOTIFY_PROVIDER"))
                    {
                        retVal = Convert.ToBoolean(dr["NOTIFY_PROVIDER"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;


            }

            return retVal;
        }


    }
}
