using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Security.Authentication;
using System.Runtime.CompilerServices;
using Corp.Core.Libraries.Helper;
using MMSWebControls;

namespace Corp.Core.Libraries
{
    public class SubmitOnBaseDocument
    {
        public string RequestID;
        public string FileTypeID;
        public string Tax_ID;
        public string Request_Type;
        public string Provider_ID;
        public string NPI;
        public string Taxonomy;
        public string Zip5;
        public string Extended_Zip;
        public string PDMS_Reg_ID;
        public string Document_Type;
        public string Application_Type;
        public byte[] Image_Blob;
    }

    public class SubmitOnBaseDocumentOnly
    {
        public string RequestID;
        public string FileTypeID;
        public byte[] Image_Blob;
    }

    public class OnBaseInterface
    {
        public string DownloadedMimeType = "";
        public string DownloadedOriginalFileName = "";


        public int GetSubmitedFileID(int docID, byte[] fileBytes, string fileName, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            int onBaseDocumentID = 0;

            try
            {
                if (AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString).ToLower() == bool.TrueString.ToLower())
                {
                    // Do nothing
                }
                else
                {
                    SubmitOnBaseDocumentOnly request = new SubmitOnBaseDocumentOnly()
                    {
                        RequestID = docID.ToString(),
                        FileTypeID = "16",
                        Image_Blob = fileBytes
                    };

                    // serialize the object to xml 
                    //string obj = Methods.SerializeObjectToXml(request);
                    string obj = ToXMLDoc(request);
                    // Get XML file path and name
                    string localPath = AppSettings.Get("OnBase-UploadDocumentXMLLocalPath");
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        localPath = @"C:\Projects\Deployments\UploadOnBaseDocuments\";
                    }
                    fileName = Path.GetFileNameWithoutExtension(fileName) + ".xml";

                    // Write the object to file
                    Methods.WriteStringToFile(localPath, fileName, obj, false);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    string fullFilePath = localPath + fileName;
                    // Send to OnBase and get response back
                    string uRL = AppSettings.Get("OnBase-UploadHandlerURL");
                    WebClient webClient = new WebClient();
                    byte[] responseBytes = webClient.UploadFile(uRL, fullFilePath);

                    // Return value example                 
                    // <RequestID>98756432</RequestID>
                    // <Result>Success</Result>
                    // <ErrorMessage> </ErrorMessage>
                    // <OnBaseDocumentID>1444230</ OnBaseDocumentID >
                    // Get the XML tags
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(Encoding.UTF8.GetString(responseBytes));

                    string result = xmlDoc.GetElementsByTagName("Result")[0].InnerText;
                    string errorMessage = xmlDoc.GetElementsByTagName("ErrorMessage")[0].InnerText;

                    if (result.ToUpper() == Constants.OnBaseSuccess)
                    {
                        onBaseDocumentID = Convert.ToInt32(xmlDoc.GetElementsByTagName("OnBaseDocumentID")[0].InnerText);

                        this.WriteLog(fileName + "uploaded to onbase ok from " + memberName + " : " + sourceFilePath + " : " + sourceLineNumber + " pnm docid: " + docID.ToString() + " onbaseid: " + onBaseDocumentID);
                    }
                    else
                    {
                        string eMsg = "Failed to write to onBase, error code: " + result + " ERROR MSG: " + errorMessage + " Called From: " + memberName + " : " + sourceFilePath + " : " + sourceLineNumber;
                        this.WriteLog(eMsg);
                        throw new Exception(eMsg);
                    }

                    webClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.WriteLog("ERROR submitting to OnBase: " + ex.Message + " STACK TRACE:" + ex.StackTrace);
                throw ex;
            }

            return onBaseDocumentID;
        }

        public void SubmitFile(int docID, byte[] fileBytes, string fileName, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                if (AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString).ToLower() == bool.TrueString.ToLower())
                {
                    // Do nothing
                }
                else
                {
                    SubmitOnBaseDocumentOnly request = new SubmitOnBaseDocumentOnly()
                    {
                        RequestID = docID.ToString(),
                        FileTypeID = "16",
                        Image_Blob = fileBytes
                    };

                    // serialize the object to xml 
                    //string obj = Methods.SerializeObjectToXml(request);
                    string obj = ToXMLDoc(request);
                    // Get XML file path and name
                    string localPath = AppSettings.Get("OnBase-UploadDocumentXMLLocalPath");
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        localPath = @"C:\Projects\Deployments\UploadOnBaseDocuments\";
                    }
                    fileName = Path.GetFileNameWithoutExtension(fileName) + ".xml";

                    // Write the object to file
                    Methods.WriteStringToFile(localPath, fileName, obj, false);
                    byte[] responseBytes;
                    WebClient webClient = new WebClient();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    try
                    {
                        string fullFilePath = localPath + fileName;
                        // Send to OnBase and get response back
                        string uRL = AppSettings.Get("OnBase-UploadHandlerURL");

                        responseBytes = webClient.UploadFile(uRL, fullFilePath);
                    }
                    catch (WebException ex)
                    {
                        this.WriteLog("Failed to get response from onBase, error code: " + ex.Message + "::" + ex.StackTrace);
                        throw ex;
                    }
                    // Delete the xml file written to the local path
                    File.Delete(Path.Combine(localPath, fileName));

                    // Return value example                 
                    // <RequestID>98756432</RequestID>
                    // <Result>Success</Result>
                    // <ErrorMessage> </ErrorMessage>
                    // <OnBaseDocumentID>1444230</ OnBaseDocumentID >
                    // Get the XML tags
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(Encoding.UTF8.GetString(responseBytes));

                    string result = xmlDoc.GetElementsByTagName("Result")[0].InnerText;
                    string errorMessage = xmlDoc.GetElementsByTagName("ErrorMessage")[0].InnerText;

                    if (result.ToUpper() == Constants.OnBaseSuccess)
                    {
                        int documentID = Convert.ToInt32(xmlDoc.GetElementsByTagName("RequestID")[0].InnerText);
                        int onBaseDocumentID = Convert.ToInt32(xmlDoc.GetElementsByTagName("OnBaseDocumentID")[0].InnerText);

                        // Ingest the OnBase document id
                        // generated by sp_Admin_StoredProcBuilder on Jul 21 2015  2:35PM
                        // create parameters objects and fill with values
                        List<SqlParameter> sqlParams = new List<SqlParameter>();

                        sqlParams.Add(SqlHelper.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, true));
                        sqlParams.Add(SqlHelper.CreateParameter("ONBASE_DOCUMENT_ID", DbType.Int32, onBaseDocumentID, true));

                        InfoAccess.ExecuteStoredProcedure("usp_SetOnBaseDocumentID", sqlParams);
                        this.WriteLog(fileName + "uploaded to onbase ok from " + memberName + " : " + sourceFilePath + " : " + sourceLineNumber + " pnm docid: " + documentID.ToString() + " onbaseid: " + onBaseDocumentID);
                    }
                    else
                    {
                        string eMsg = "Failed to write to onBase, error code: " + result + " ERROR MSG: " + errorMessage + " Called From: " + memberName + " : " + sourceFilePath + " : " + sourceLineNumber;
                        this.WriteLog(eMsg);
                        throw new Exception(eMsg);
                    }

                    webClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.WriteLog("ERROR submitting to OnBase: " + ex.Message + " STACK TRACE:" + ex.StackTrace);
                throw ex;
            }
        }

        public void SubmitFile(int regID, int docID, byte[] fileBytes, string fileName)
        {
            if (AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString).ToLower() == bool.TrueString.ToLower())
            {
                // Do nothing
            }
            else
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Jul 21 2015  9:40AM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, regID, true));
                    parameters.Add(SqlHelper.CreateParameter("DOCUMENT_ID", DbType.Int32, docID, true));

                    DataSet ds = InfoAccess.ExecuteStoredProcedure("usp_GetOnBaseDocumentDetail", parameters, "OnBaseDocument");

                    if (ds.Tables[0].Rows.Count < 1)
                        return;

                    DataRow row = ds.Tables[0].Rows[0];

                    int wfEventTypeID = Methods.GetIntValue(row, "WORKFLOW_EVENT_TYPE_ID");
                    string requestType = string.Empty;
                    switch (wfEventTypeID)
                    {
                        case Constants.WorkflowEventType.NewReg:
                            requestType = "Initial Enrollment";
                            break;
                        case Constants.WorkflowEventType.RevalReg:
                            requestType = "Re-Enrollment";
                            break;
                        case Constants.WorkflowEventType.UpdateReg:
                            requestType = "Update Enrollment";
                            break;
                        default:
                            requestType = "Not Recognized";
                            break;
                    }

                    SubmitOnBaseDocument request = new SubmitOnBaseDocument()
                    {
                        RequestID = row["DOCUMENT_ID"].ToString(),
                        FileTypeID = "16",
                        Tax_ID = row["TAX_ID"].ToString(),
                        Request_Type = requestType,
                        Provider_ID = row["PROVIDER_ID"].ToString(),
                        NPI = row["NPI"].ToString(),
                        Taxonomy = row["TAXONOMY"].ToString(),
                        Zip5 = row["ZIP5"].ToString(),
                        Extended_Zip = row["EXTENDED_ZIP"].ToString(),
                        PDMS_Reg_ID = regID.ToString(),
                        Document_Type = row["DOCUMENT_TYPE"].ToString(),
                        Application_Type = row["APPLICATION_TYPE_ID"].ToString(),
                        Image_Blob = fileBytes
                    };

                    // serialize the object to xml 
                    //string obj = Methods.SerializeObjectToXml(request);
                    string obj = ToXML(request);

                    // Get XML file path and name
                    string localPath = AppSettings.Get("OnBase-UploadDocumentXMLLocalPath");
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        localPath = @"C:\Temp\D\Interfaces\PDMS\PDMS_DEV\PDMSDataExchange\UploadOnBaseDocuments\";
                    }
                    fileName = Path.GetFileNameWithoutExtension(fileName) + ".xml";

                    // Write the object to file
                    Methods.WriteStringToFile(localPath, fileName, obj, false);
                    byte[] responseBytes;
                    WebClient webClient = new WebClient();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    try
                    {
                        string fullFilePath = localPath + fileName;
                        // Send to OnBase and get response back
                        string uRL = AppSettings.Get("OnBase-UploadHandlerURL");

                        responseBytes = webClient.UploadFile(uRL, fullFilePath);
                    }
                    catch (WebException ex)
                    {
                        this.WriteLog("Failed to get response from onBase, error code: " + ex.Message + "::" + ex.StackTrace);
                        throw ex;
                    }
                    // Delete the xml file written to the local path
                    File.Delete(Path.Combine(localPath, fileName));

                    // Return value example                 
                    // <RequestID>98756432</RequestID>
                    // <Result>Success</Result>
                    // <ErrorMessage> </ErrorMessage>
                    // <OnBaseDocumentID>1444230</ OnBaseDocumentID >
                    // Get the XML tags
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(Encoding.UTF8.GetString(responseBytes));

                   
                    string result = xmlDoc.GetElementsByTagName("Result")[0].InnerText;
                    string errorMessage = xmlDoc.GetElementsByTagName("ErrorMessage")[0].InnerText;
                   

                    if (result.ToUpper() == Constants.OnBaseSuccess)
                    {
                        int documentID = Convert.ToInt32(xmlDoc.GetElementsByTagName("RequestID")[0].InnerText);
                        int onBaseDocumentID = string.IsNullOrEmpty(xmlDoc.GetElementsByTagName("OnBaseDocumentID")[0].InnerText) ? 0 : Convert.ToInt32(xmlDoc.GetElementsByTagName("OnBaseDocumentID")[0].InnerText);

                        // Ingest the OnBase document id
                        // generated by sp_Admin_StoredProcBuilder on Jul 21 2015  2:35PM
                        // create parameters objects and fill with values
                        List<SqlParameter> sqlParams = new List<SqlParameter>();

                        sqlParams.Add(SqlHelper.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, true));
                        sqlParams.Add(SqlHelper.CreateParameter("ONBASE_DOCUMENT_ID", DbType.Int32, onBaseDocumentID, true));

                        InfoAccess.ExecuteStoredProcedure("usp_SetOnBaseDocumentID", sqlParams);
                    }
                    else
                    {
                        this.WriteLog("Failed to write to onBase, error code: " + result);
                    }
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    this.WriteLog("Error writing to onBase, error code: " + ex.Message + "::" + ex.StackTrace);
                    throw ex;
                }
            }
        }

        public int GetSubmitedFileID(int regID, int docID, byte[] fileBytes, string fileName, bool isUploadAgreement = false)
        {
            int onBaseDocumentID = 0;
            if (AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString).ToLower() == bool.TrueString.ToLower())
            {
                // Do nothing
            }
            else
            {
                try
                {
                    // generated by sp_Admin_StoredProcBuilder on Jul 21 2015  9:40AM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, regID, true));
                    parameters.Add(SqlHelper.CreateParameter("DOCUMENT_ID", DbType.Int32, docID, true));

                    DataSet ds = InfoAccess.ExecuteStoredProcedure("usp_GetOnBaseDocumentDetail", parameters, "OnBaseDocument");

                    if (ds.Tables[0].Rows.Count < 1)
                        return onBaseDocumentID;

                    DataRow row = ds.Tables[0].Rows[0];

                    int wfEventTypeID = Methods.GetIntValue(row, "WORKFLOW_EVENT_TYPE_ID");
                    string requestType = string.Empty;
                    switch (wfEventTypeID)
                    {
                        case Constants.WorkflowEventType.NewReg:
                            requestType = "Initial Enrollment";
                            break;
                        case Constants.WorkflowEventType.RevalReg:
                            requestType = "Re-Enrollment";
                            break;
                        case Constants.WorkflowEventType.UpdateReg:
                            requestType = "Update Enrollment";
                            break;
                        default:
                            requestType = "Not Recognized";
                            break;
                    }

                    SubmitOnBaseDocument request = new SubmitOnBaseDocument()
                    {
                        RequestID = row["DOCUMENT_ID"].ToString(),
                        FileTypeID = "16",
                        Tax_ID = row["TAX_ID"].ToString(),
                        Request_Type = requestType,
                        Provider_ID = row["PROVIDER_ID"].ToString(),
                        NPI = row["NPI"].ToString(),
                        Taxonomy = row["TAXONOMY"].ToString(),
                        Zip5 = row["ZIP5"].ToString(),
                        Extended_Zip = row["EXTENDED_ZIP"].ToString(),
                        PDMS_Reg_ID = regID.ToString(),
                        Document_Type = row["DOCUMENT_TYPE"].ToString(),
                        Application_Type = row["APPLICATION_TYPE_ID"].ToString(),
                        Image_Blob = fileBytes
                    };

                    // serialize the object to xml 
                    //string obj = Methods.SerializeObjectToXml(request);
                    string obj = ToXML(request);

                    // Get XML file path and name
                    string localPath = string.Empty;
                    if (isUploadAgreement)
                    {
                        // path on the job server
                        localPath = AppSettings.Get("OnBase-UploadAgreementXMLLocalPath");
                    }
                    else 
                        localPath = AppSettings.Get("OnBase-UploadDocumentXMLLocalPath");

                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        localPath = @"C:\Temp\D\Interfaces\PDMS\PDMS_DEV\PDMSDataExchange\UploadOnBaseDocuments\";
                    }
                    fileName = Path.GetFileNameWithoutExtension(fileName) + ".xml";

                    // Write the object to file
                    Methods.WriteStringToFile(localPath, fileName, obj, false);
                    byte[] responseBytes;
                    WebClient webClient = new WebClient();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    try
                    {
                        string fullFilePath = localPath + fileName;
                        // Send to OnBase and get response back
                        string uRL = AppSettings.Get("OnBase-UploadHandlerURL");

                        responseBytes = webClient.UploadFile(uRL, fullFilePath);
                    }
                    catch (WebException ex)
                    {
                        this.WriteLog("Failed to get response from onBase, error code: " + ex.Message + "::" + ex.StackTrace);
                        throw ex;
                    }
                    // Delete the xml file written to the local path
                    File.Delete(Path.Combine(localPath, fileName));

                    // Return value example                 
                    // <RequestID>98756432</RequestID>
                    // <Result>Success</Result>
                    // <ErrorMessage> </ErrorMessage>
                    // <OnBaseDocumentID>1444230</ OnBaseDocumentID >
                    // Get the XML tags
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(Encoding.UTF8.GetString(responseBytes));


                    string result = xmlDoc.GetElementsByTagName("Result")[0].InnerText;
                    string errorMessage = xmlDoc.GetElementsByTagName("ErrorMessage")[0].InnerText;


                    if (result.ToUpper() == Constants.OnBaseSuccess)
                    {
                        int documentID = Convert.ToInt32(xmlDoc.GetElementsByTagName("RequestID")[0].InnerText);
                        onBaseDocumentID = string.IsNullOrEmpty(xmlDoc.GetElementsByTagName("OnBaseDocumentID")[0].InnerText) ? 0 : Convert.ToInt32(xmlDoc.GetElementsByTagName("OnBaseDocumentID")[0].InnerText);

                        // Ingest the OnBase document id
                        // generated by sp_Admin_StoredProcBuilder on Jul 21 2015  2:35PM
                        // create parameters objects and fill with values
                        List<SqlParameter> sqlParams = new List<SqlParameter>();

                        sqlParams.Add(SqlHelper.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, true));
                        sqlParams.Add(SqlHelper.CreateParameter("ONBASE_DOCUMENT_ID", DbType.Int32, onBaseDocumentID, true));

                        InfoAccess.ExecuteStoredProcedure("usp_SetOnBaseDocumentID", sqlParams);
                    }
                    else
                    {
                        this.WriteLog("Failed to write to onBase, error code: " + result);
                    }
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    this.WriteLog("Error writing to onBase, error code: " + ex.Message + "::" + ex.StackTrace);
                    throw ex;
                }
            }
            return onBaseDocumentID;
        }
        public int GetSubmitedFileIDFromJobServer(int docID, byte[] fileBytes, string fileName, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            int onBaseDocumentID = 0;

            try
            {
                if (AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString).ToLower() == bool.TrueString.ToLower())
                {
                    // Do nothing
                }
                else
                {
                    SubmitOnBaseDocumentOnly request = new SubmitOnBaseDocumentOnly()
                    {
                        RequestID = docID.ToString(),
                        FileTypeID = "16",
                        Image_Blob = fileBytes
                    };

                    // serialize the object to xml 
                    //string obj = Methods.SerializeObjectToXml(request);
                    string obj = ToXMLDoc(request);
                    // Get XML file path and name
                    string localPath = AppSettings.Get("OnBase-UploadAgreementXMLLocalPath");
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        localPath = @"C:\Projects\Deployments\UploadOnBaseDocuments\";
                    }
                    fileName = Path.GetFileNameWithoutExtension(fileName) + ".xml";

                    // Write the object to file
                    Methods.WriteStringToFile(localPath, fileName, obj, false);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    string fullFilePath = localPath + fileName;
                    // Send to OnBase and get response back
                    string uRL = AppSettings.Get("OnBase-UploadHandlerURL");
                    WebClient webClient = new WebClient();
                    byte[] responseBytes = webClient.UploadFile(uRL, fullFilePath);

                    // Return value example                 
                    // <RequestID>98756432</RequestID>
                    // <Result>Success</Result>
                    // <ErrorMessage> </ErrorMessage>
                    // <OnBaseDocumentID>1444230</ OnBaseDocumentID >
                    // Get the XML tags
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(Encoding.UTF8.GetString(responseBytes));

                    string result = xmlDoc.GetElementsByTagName("Result")[0].InnerText;
                    string errorMessage = xmlDoc.GetElementsByTagName("ErrorMessage")[0].InnerText;

                    if (result.ToUpper() == Constants.OnBaseSuccess)
                    {
                        onBaseDocumentID = Convert.ToInt32(xmlDoc.GetElementsByTagName("OnBaseDocumentID")[0].InnerText);

                        this.WriteLog(fileName + "uploaded to onbase ok from " + memberName + " : " + sourceFilePath + " : " + sourceLineNumber + " pnm docid: " + docID.ToString() + " onbaseid: " + onBaseDocumentID);
                    }
                    else
                    {
                        string eMsg = "Failed to write to onBase, error code: " + result + " ERROR MSG: " + errorMessage + " Called From: " + memberName + " : " + sourceFilePath + " : " + sourceLineNumber;
                        this.WriteLog(eMsg);
                        throw new Exception(eMsg);
                    }

                    webClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.WriteLog("ERROR submitting to OnBase: " + ex.Message + " STACK TRACE:" + ex.StackTrace);
                throw ex;
            }

            return onBaseDocumentID;
        }

        private void WriteLog(string msg)
        {
            // create log object
            string logProcessName = "OnBase Write";
            Logging log = new Logging(new Guid("97D4C3AB-7798-4A9D-BC7E-8F041D5DA9F8"), logProcessName);
            log.CreateLogEntry(msg);
        }

        public byte[] RetrieveFile(string filePath)  // THIS NEEDS TO GO AWAY!  (ACCESS SHOULD BE BY OBFUSCATED ID ONLY)
        {
            byte[] fileBytes = null;
            string onBaseDocID = string.Empty;

            DataSet ds = GetOnbaseIDByFile(filePath);

            if (ds.Tables[0].Rows.Count > 0)
            {
                onBaseDocID = ds.Tables[0].Rows[0]["ONBASE_DOCUMENT_ID"].ToString();
            }
            if ((onBaseDocID == "0" || string.IsNullOrEmpty(onBaseDocID)) && filePath.Contains("\\"))
            {
                //get the documentid by filename.
                string fileName = Path.GetFileName(filePath);
                DataSet dsOnbase = GetOnbaseIDByFile(fileName);
                if (dsOnbase.Tables[0].Rows.Count > 0)
                {
                    onBaseDocID = dsOnbase.Tables[0].Rows[0]["ONBASE_DOCUMENT_ID"].ToString();
                }
            }
            // If the onbase document id is null or empty, we go get the local file. If not, we go to onbase to get the file.
            if (string.IsNullOrEmpty(onBaseDocID))
            {
                /// Read in file, decrypt, then send to the user.
                fileBytes = File.ReadAllBytes(filePath);  // THIS NEEDS TO GO AWAY
            }
            else
            {
                fileBytes = DownloadOnbaseFile(onBaseDocID);
            }
            return fileBytes;
        }

        public byte[] RetrieveFile(string filePath, int documentID, string isEncrypted = null)
        {

            byte[] fileBytes = null;
            string onBaseDocID = string.Empty;

            // Read file from OnBase.
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, true));
            DataSet ds = InfoAccess.ExecuteStoredProcedure("usp_GetOnBaseDocumentId", parameters, "OnBaseDocument");

            if (ds.Tables[0].Rows.Count > 0)
            {
                onBaseDocID = ds.Tables[0].Rows[0]["ONBASE_DOCUMENT_ID"].ToString();
            }

            // If the onbase document id is null or empty, we go get the local file. If not, we go to onbase to get the file.
            if (string.IsNullOrEmpty(onBaseDocID) || onBaseDocID == "0")
            {
                /// Read in file, decrypt, then send to the user.
                fileBytes = File.ReadAllBytes(filePath);   // THIS NEEDS TO GO AWAY
            }
            else
            {
                fileBytes = DownloadOnbaseFile(onBaseDocID, isEncrypted);
            }
            return fileBytes;
        }
        public byte[] RetrieveFilebyOnBaseDocID(string onBaseDocID, string isEncrypted = null)
        {

            byte[] fileBytes = null;
            fileBytes = DownloadOnbaseFile(onBaseDocID, isEncrypted);
            return fileBytes;
        }

        public byte[] DownloadOnbaseFile(string onBaseDocID, string encrypted = null)
        {
            byte[] rawFileBytes = null;
            byte[] decryptedFileBytes = null;

            // get encryption status
            string IsEncrypted = "Y";
            if (!string.IsNullOrEmpty(encrypted))
                IsEncrypted = encrypted.Substring(0,1).ToUpper(); // GetEncryptionStatusForOnbaseId(int.Parse(onBaseDocID));

            // set filename and mime type for calling process

            // get original file name
            this.DownloadedOriginalFileName = GetFilenameForOnbaseId(int.Parse(onBaseDocID));

            this.DownloadedMimeType = MimeHelper.GetMimeTypeForFileName(DownloadedOriginalFileName);

            string uRL = AppSettings.Get("OnBase-DownloadHandlerURL") + "?dh=" + onBaseDocID;

            if (uRL.Contains("https"))
            {
                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;
            }

            WebClient webClient = new WebClient();
            try
            {
                rawFileBytes = webClient.DownloadData(uRL);

                // if override of != Y (N or No) was passed, just return (THIS MAY CHANGE LATER WHEN WE FIX THE MISSING ENCRYPTION)
                if (IsEncrypted != "Y") { return rawFileBytes; }

                // check if file is encrypted (checks to see if file signature matches extension)
                if (this.IsFileEncrypted(this.DownloadedOriginalFileName, rawFileBytes) != "NO") { 
                    IsEncrypted = "Y"; 
                } else
				{
                    return rawFileBytes;
				}

                // decrypt if needed
                if (IsEncrypted == "Y")
                {
                    try  // will throw error if not encrypted
                    {
                        Encryption enc = new Encryption();
                        decryptedFileBytes = enc.DecryptRijndael(rawFileBytes);

                        // there is a small possibility that the file could have been encrypted twice, so check for known file types
                        string finalCheck = this.IsFileEncrypted(this.DownloadedOriginalFileName, decryptedFileBytes);

                        if (finalCheck == "NO") { return decryptedFileBytes; }
                        if (finalCheck == "UNKNOWN") { return decryptedFileBytes; }

                        if (finalCheck == "YES")  // try decrypting one more time - will throw error if not encrypted
						{
                            try
							{
                                rawFileBytes = decryptedFileBytes;  // file was decrypted sucessfully once, so this is now the raw bytes
                                decryptedFileBytes = enc.DecryptRijndael(rawFileBytes);
                                return decryptedFileBytes;
                            } 
                            catch (Exception x)  
							{
                                return rawFileBytes;
							}
						}                        
                    }
                    catch (Exception ex)  // error on decryption return raw bytes
					{
                        return rawFileBytes;
					}

                }                
            }
            catch (Exception ex)  // surface error getting file from onbase
            {
                throw ex;                
            }
            finally
            {
                webClient.Dispose();
            }

            // if we get here something went wrong return empty array (should never happen)
            return rawFileBytes;

        }

        public string IsFileEncrypted(string filename, byte[] fb)
        {
            // check magic numbers from byte array for file extension
            // can return YES, NO, UNKNOWN or ERROR

            try
            {
                string xt = Path.GetExtension(filename).ToLower();

                byte[] mx = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                // get first 16 bytes to check file signature (if not encrypted, signature should match)
                Buffer.BlockCopy(fb, 0, mx, 0, 16);

                string dta = BitConverter.ToString(mx);

                switch (xt)
                {
                    case ".gif":
                        if (dta.StartsWith("47-49-46-38-37-61")) { return "NO"; }
                        if (dta.StartsWith("47-49-46-38-39-61")) { return "NO"; }
                        break;

                    case ".jpg":
                    case ".jpeg":
                        if (dta.StartsWith("FF-D8-FF")) { return "NO"; }
                        break;

                    case ".pdf":
                        if (dta.StartsWith("25-50-44-46-2D")) { return "NO"; }
                        break;

                    case ".tif":
                    case ".tiff":
                        if (dta.StartsWith("49-49-2A-00")) { return "NO"; }
                        if (dta.StartsWith("4D-4D-00-2A")) { return "NO"; }
                        break;

                    case ".xml":
                        if (dta.Contains("3C-3F-78-6D-6C-20")) { return "NO"; }
                        if (dta.Contains("3C-00-3F-00-78-00-6D-00-6C-00-20")) { return "NO"; }
                        break;

                    case ".doc":  // all old microsoft office files have a similar header
                    case ".msg":
                    case ".ppt":
                    case ".xls":
                        if (dta.StartsWith("D0-CF-11-E0-A1-B1-1A-E1")) { return "NO"; }
                        break;

                    case ".htm":
                    case ".html":
                        if (dta.Contains("3C-68-74-6D-6C-20")) { return "NO"; }
                        if (dta.Contains("3C-00-68-00-74-00-6D-00-6C-00-20")) { return "NO"; }
                        break;

                    case ".docx":  // all based on zip format
                    case ".epub":
                    case ".msix":
                    case ".odp":
                    case ".ods":
                    case ".odt":
                    case ".pptx":
                    case ".xlsx":
                    case ".zip":                    
                        if (dta.StartsWith("50-4B-03-04")) { return "NO"; }
                        if (dta.StartsWith("50-4B-05-06")) { return "NO"; }
                        if (dta.StartsWith("50-4B-07-08")) { return "NO"; }
                        break;

                    case ".7z":
                        if (dta.StartsWith("37-7A-BC-AF-27-1C")) { return "NO"; }
                        break;

                    case ".gz":
                        if (dta.StartsWith("1F-8B")) { return "NO"; }
                        break;

                    case ".rtf":
                        if (dta.StartsWith("7B-5C-72-74-66-31")) { return "NO"; }
                        break;

                    case ".png":
                        if (dta.StartsWith("89-50-4E-47-0D-0A-1A-0A")) { return "NO"; }
                        break;

                    case ".bmp":
                        if (dta.StartsWith("42-4D")) { return "NO"; }
                        break;

                    default: // if we get here file extension is not matched so we cannot check
                        return "UNKNOWN";
                }

                return "YES";

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            return "ERROR";

        }


    public string ToXMLDoc(SubmitOnBaseDocumentOnly request)
        {
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "utf-8", null));

            XElement docs = new XElement("Documents");
            docs.Add(new XAttribute("Count", "1"));

            XElement doc = new XElement("Document");
            doc.Add(new XElement("RequestID", request.RequestID.ToString()));
            doc.Add(new XElement("FileTypeID", request.FileTypeID.ToString()));


            doc.Add(new XElement("Image_Blob", Convert.ToBase64String(request.Image_Blob)));
            docs.Add(doc);
            xDoc.Add(docs);

            return xDoc.Declaration.ToString() + xDoc.ToString(SaveOptions.None);
        }

        private DataSet GetOnbaseIDByFile(string file)
        {
            try
            {
                // Read file from OnBase.
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("FILE_NAME", DbType.String, file, true));
                DataSet ds = InfoAccess.ExecuteStoredProcedure("usp_GetOnBaseDocumentId", parameters, "OnBaseDocument");
                return ds;
            }
            catch (SqlException ex)
            {
                // db error occurred
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ToXML(SubmitOnBaseDocument request)
        {
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "utf-8", null));

            XElement docs = new XElement("Documents");
            docs.Add(new XAttribute("Count", "1"));

            XElement doc = new XElement("Document");
            doc.Add(new XElement("RequestID", request.RequestID.ToString()));
            doc.Add(new XElement("FileTypeID", request.FileTypeID.ToString()));

            XElement kwList = new XElement("KWList");
            XElement kwTaxID = new XElement("keyword", request.Tax_ID.ToString());
            kwTaxID.Add(new XAttribute("ID", "Tax_ID-NP"));
            kwList.Add(kwTaxID);

            XElement kwRequestType = new XElement("keyword", request.Request_Type.ToString());
            kwRequestType.Add(new XAttribute("ID", "Request_Type-NP"));
            kwList.Add(kwRequestType);

            XElement kwProviderID = new XElement("keyword", request.Provider_ID.ToString());
            kwProviderID.Add(new XAttribute("ID", "Provider_ID-NP"));
            kwList.Add(kwProviderID);

            XElement kwNPI = new XElement("keyword", request.NPI.ToString());
            kwNPI.Add(new XAttribute("ID", "NPI-NP"));
            kwList.Add(kwNPI);

            XElement kwTaxonomy = new XElement("keyword", request.Taxonomy.ToString());
            kwTaxonomy.Add(new XAttribute("ID", "Taxonomy-NP"));
            kwList.Add(kwTaxonomy);

            XElement kwZip5 = new XElement("keyword", request.Zip5.ToString());
            kwZip5.Add(new XAttribute("ID", "Zip5-NP"));
            kwList.Add(kwZip5);

            XElement kwExtZip = new XElement("keyword", request.Extended_Zip.ToString());
            kwExtZip.Add(new XAttribute("ID", "Extended_Zip-NP"));
            kwList.Add(kwExtZip);

            XElement kwRegID = new XElement("keyword", request.PDMS_Reg_ID.ToString());
            kwRegID.Add(new XAttribute("ID", "PDMS_Reg_ID-NP"));
            kwList.Add(kwRegID);

            XElement kwDocType = new XElement("keyword", request.Document_Type.ToString());
            kwDocType.Add(new XAttribute("ID", "Document_Type-NP"));
            kwList.Add(kwDocType);

            XElement kwAppType = new XElement("keyword", request.Application_Type.ToString());
            kwAppType.Add(new XAttribute("ID", "Application_Type-NP"));
            kwList.Add(kwAppType);

            doc.Add(kwList);
            doc.Add(new XElement("Image_Blob", Convert.ToBase64String(request.Image_Blob)));
            docs.Add(doc);
            xDoc.Add(docs);

            return xDoc.Declaration.ToString() + xDoc.ToString(SaveOptions.None);
        }


        public string ObfuscateId(int id)
        {
            if (id == 0) { return ""; }

            // if you need to concatenate you can use any of the following characters  i,l,m,n,o,t,x
            try
            {
                string rtn;
                Int64 sid = id + 197;
                Int64 xm;
                xm = sid * 3;
                int xl = xm.ToString().Length + 10;
                string sc = xl.ToString().Substring(1, 1);
                string fc = xl.ToString().Substring(0, 1);
                string csum = sid.ToString().Substring(sid.ToString().Length - 2);
                rtn = Guid.NewGuid().ToString().Replace("-", "").ToLower();
                rtn = sc + rtn.Substring(2, 30) + fc;
                rtn = rtn.Remove(4, 2).Insert(4, csum);
                rtn = rtn.Remove(xl, (xl - 10)).Insert(xl, xm.ToString());
                rtn = rtn.Replace('1', 'g');
                rtn = rtn.Replace('2', 'h');
                rtn = rtn.Replace('3', 'r');
                rtn = rtn.Replace('4', 'w');
                rtn = rtn.Replace('5', 'y');
                rtn = rtn.Replace('6', 'u');
                rtn = rtn.Replace('7', 'z');
                rtn = rtn.Replace('8', 'k');
                rtn = rtn.Replace('9', 'v');
                rtn = rtn.Replace('0', 's');
                rtn = rtn.Replace('c', 'j');
                rtn = rtn.Replace('d', 'p');
                rtn = rtn.Replace('f', 'q');
                return rtn.ToUpper();
            }
            catch
            {
                return "ERROR BAD ID";
            }
        }

        public int DeObfuscateId(string obtxt)
        {
            if (obtxt.Trim() == "") { return 0; }
            try
            {
                string rtn;
                Int64 id;
                rtn = obtxt.ToLower().Trim();
                rtn = rtn.Replace('g', '1');
                rtn = rtn.Replace('h', '2');
                rtn = rtn.Replace('r', '3');
                rtn = rtn.Replace('w', '4');
                rtn = rtn.Replace('y', '5');
                rtn = rtn.Replace('u', '6');
                rtn = rtn.Replace('z', '7');
                rtn = rtn.Replace('k', '8');
                rtn = rtn.Replace('v', '9');
                rtn = rtn.Replace('s', '0');
                rtn = rtn.Replace('j', 'c');
                rtn = rtn.Replace('p', 'd');
                rtn = rtn.Replace('q', 'f');
                string sc = rtn.Substring(0, 1);
                string fc = rtn.Substring(rtn.Length - 1);
                int xl = int.Parse(fc + sc);
                Int64 mv = Int64.Parse(rtn.Substring(xl, (xl - 10)));
                Int64 sid = mv / 3;
                id = sid - 197;
                string csum = sid.ToString().Substring(sid.ToString().Length - 2);
                string xsum = rtn.Substring(4, 2);
                if (csum != xsum)
                {
                    return -99999;
                }
                return (int)id;
            }
            catch
            {
                return -99999;
            }
        }


        public string GetFilenameForOnbaseId(int OnBaseID)
        {
            string fileName = "NOT_FOUND";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("ONBASE_ID", DbType.Int32, OnBaseID, true));
            DataSet ds = InfoAccess.ExecuteStoredProcedure("usp_GetDocumentFilenameForOnbaseId", parameters, "OnBaseDocument");

            if (ds.Tables[0].Rows.Count > 0)
            {
                fileName = ds.Tables[0].Rows[0]["FILE_NAME"].ToString();
            }

            return fileName;
        }

        public string GetEncryptionStatusForOnbaseId(int OnBaseID)
        {
            string isEncrypted = "N";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("ONBASE_ID", DbType.Int32, OnBaseID, true));
            DataSet ds = InfoAccess.ExecuteStoredProcedure("usp_GetEncryptionStatusForOnbaseId", parameters, "OnBaseDocument");

            // note: this currently inspects the IS_COVERTED value (as only converted documents are currently encrypted, in the future it will move to a IS_ENCRYPTED flag in the document table)

            if (ds.Tables[0].Rows.Count > 0)
            {
                isEncrypted = ds.Tables[0].Rows[0]["isEncrypted"].ToString();
            }

            return isEncrypted;
        }



    }
}