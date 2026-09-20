using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MAXIMUS.Core.Libraries;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Data.SqlClient;
using System.Xml.Linq;
using Corp.Core.Libraries;
using System.Xml;

namespace MAXIMUS.DataExchange.PDMS.RemittanceAdviceMCE
{
    public class RemittanceAdviceProcessor : BaseJob, IJob
    {
        public RemittanceAdviceProcessor(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private const string thisGuidString = "7948AA0F-D41D-407E-B142-F5F3448BA229";
        private Guid thisGuid = new Guid(thisGuidString);

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(thisGuid);
        }

        override public void ExecuteJob(Guid jobId)
        {
            ProcessReadmittancePackage(jobId);
        }

        public static string RemoveTroublesomeCharacters(string inString)
        {
            if (inString == null) return null;

            StringBuilder newString = new StringBuilder();
            char ch;

            for (int i = 0; i < inString.Length; i++)
            {

                ch = inString[i];
                if (XmlConvert.IsXmlChar(ch))
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();

        }

        private void ProcessReadmittancePackage(Guid jobId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            RemittanceAdviceHelper.CreateLogEntry("ProcessReadmittancePackage", "Initiating Remittance Advice Processor v2", CON.RemittanceAdviceMCE.LogPriorityInfo);

            string localPath = AppSettings.Get("RALocalPath");
            if (!(Directory.Exists(localPath)))
            {
                Directory.CreateDirectory(localPath);
            }
            string localPathOutput = AppSettings.Get("RALocalPathOutput");
            if (!(Directory.Exists(localPathOutput)))
            {
                Directory.CreateDirectory(localPathOutput);
            }
            string localPathArchive = AppSettings.Get("RALocalArchivePath");
            if (!(Directory.Exists(localPathArchive)))
            {
                Directory.CreateDirectory(localPathArchive);
            }
            string localPathReturn = AppSettings.Get("RALocalReturnPath");
            if (!(Directory.Exists(localPathReturn)))
            {
                Directory.CreateDirectory(localPathReturn);
            }
            string localReProcess = AppSettings.Get("RALocalReProcessPath");
            if (!(Directory.Exists(localReProcess)))
            {
                Directory.CreateDirectory(localReProcess);
            }
            string localProcessing = AppSettings.Get("RALocalProcessingPath");
            if (!(Directory.Exists(localProcessing)))
            {
                Directory.CreateDirectory(localProcessing);
            }

            DirectoryInfo localDirectory = new DirectoryInfo(localPath);
            DirectoryInfo localProcessingDirectory = new DirectoryInfo(localProcessing);
            FileCompression fc = new FileCompression(this.ThreadId);

            int recordID = 0;
            string errorFileName = string.Empty;

            foreach (FileInfo file in localDirectory.GetFiles())
            {
                
                try
                {
                    recordID = RemittanceAdviceHelper.InsertINBOUND_RA_MCE_DTLS(file.Name, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseInProcess, CON.RemittanceAdviceMCE.RemittanceAdviceMCEInsert);
                    
                    string extractFile = Regex.Replace(file.Name, ".zip", string.Empty, RegexOptions.IgnoreCase);
                    string extractPath = Path.Combine(localProcessingDirectory.FullName, extractFile);

                    fc.UnzipFile(file.FullName, extractPath);
                    DirectoryInfo extractedDirectory = new DirectoryInfo(extractPath);
                    XDocument controlFile = new XDocument();

                    String errorCode = "";
                    String errorDescription = "";
                    string msg = "";

                    int fileCount = 0;
                    bool fileStatus = false;
                    List<String> dataFileList = new List<String>();

                    foreach (FileInfo extractedFile in extractedDirectory.GetFiles("*.xml"))
                    {
                        try
                        {
                            string archiveSenderID = extractedDirectory.Name.Split('.')[0];
                            string controlSenderID = extractedFile.Name.Split('.')[0];
                            if (!(archiveSenderID.Equals(controlSenderID)) && extractedFile.Name.Contains('-'))
                            {
                                controlSenderID = extractedFile.Name.Split('-')[0];
                            }
                            RemittanceAdviceHelper.UpdateControlFileNameINBOUND_RA_MCE_DTLS(recordID, extractedFile.Name, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);

                            if (!(archiveSenderID.Equals(controlSenderID)))
                            {
                                String senderID = extractedDirectory.Name.Split('.')[0];
                                String MCEDirectoryName = String.Format("{0}.{1}.RA.Errors", senderID, DateTime.Now.ToString("yyyyMMdd"));
                                if (!(Directory.Exists(Path.Combine(localPathReturn, MCEDirectoryName))))
                                {
                                    Directory.CreateDirectory(Path.Combine(localPathReturn, MCEDirectoryName));
                                }
                                errorFileName = extractedFile.Name.Replace("RA.xml", "RA.Errors.xml");
                                XDocument errorfile = new XDocument(
                                        new XDeclaration("1.0", "utf-8", "yes"));

                                XElement Envelope = new XElement("RAErrorControlFile");
                                Envelope.Add(new XElement("EDITransaction_Type", "RA"));
                                XElement error = new XElement("Error");
                                error.Add(new XElement("ErrorCode", "-1"));
                                error.Add(new XElement("ErrorDescription", "Invalid Control Filename"));
                                Envelope.Add(error);
                                Envelope.Save(Path.Combine(localPathReturn, MCEDirectoryName, errorFileName));
                                string responseXML = Envelope.ToString();
                                RemittanceAdviceHelper.UpdateResponseXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, errorFileName, responseXML, "Invalid Control Filename", CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                            }
                            else
                            {
                                fileCount++;
                                string text = File.ReadAllText(extractedFile.FullName);
                                text = RemoveTroublesomeCharacters(text);
                                text = text.Substring(text.IndexOf("<Remitt"), text.Length - text.IndexOf("<Remitt"));
                                
                                File.WriteAllText(extractedFile.FullName, text, Encoding.Unicode);
                                try
                                {
                                    controlFile = XDocument.Load(extractedFile.FullName); 
                                    RemittanceAdviceHelper.UpdateRequestXMLINBOUND_RA_MCE_DTLS(recordID, controlFile.ToString(), CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                                }
                                catch (Exception ex)
                                {
                                    fileStatus = false;
                                    RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, "Failed to parse XML document. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                                    errorCode = "06";
                                    errorDescription = "Invalid Control File Format";
                                }

                                if (msg != "") 
                                {
                                    errorCode = "06";
                                    errorDescription = "Invalid Control File Format";
                                }

                                int regId = 0;
                                int documentId = 0;
                                int onBaseDocumentID = 0;

                                try
                                {
                                    List<SqlParameter> parameters = new List<SqlParameter>();
                                    if (controlFile.Root.Element("Provider_ID") != null)
                                    {
                                        parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, controlFile.Root.Element("Provider_ID").Value.ToString(), false));
                                    }
                                    else
                                    {
                                        parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, "", false));
                                    }

                                    if (controlFile.Root.Element("Provider_NPI") != null)
                                    {
                                        parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, controlFile.Root.Element("Provider_NPI").Value.ToString(), false));
                                    }
                                    else
                                    {
                                        parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, "", false));
                                    }
                                    regId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_getRegIDFromMedAndOrNPI", parameters));

                                }
                                catch (Exception ex)
                                {
                                    RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, "Failed to validate if Provider Exists. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                                }

                                if (regId == 0)
                                {
                                    errorCode = "01";
                                    errorDescription = "Provider Not Found in Database";
                                }

                                if (errorCode.Equals(""))
                                {
                                    if ((controlFile.Root.Element("RA_number") == null) && (controlFile.Root.Element("RA_Number") == null))
                                    {
                                        errorCode = "08";
                                        errorDescription = "Invalid RA Number";
                                    }

                                    if ((controlFile.Root.Element("Provider_NPI") == null) && (controlFile.Root.Element("Provider_ID") == null))
                                    {
                                        errorCode = "11";
                                        errorDescription = "No ProviderID or ProviderNPI provided";
                                    }

                                    if ((controlFile.Root.Element("SenderID") == null) || (controlFile.Root.Element("SenderID").Value == ""))
                                    {
                                        errorCode = "12";
                                        errorDescription = "Invalid Sender ID";
                                    }

                                    string dataFileName = "";
                                    foreach (XElement documentElement in controlFile.Root.Elements())
                                    {
                                        if (documentElement.Name == "DocumentName")
                                        {
                                            dataFileName = documentElement.Value;
                                            if (!(dataFileName.EndsWith(".pdf")))
                                            {
                                                dataFileName = dataFileName + ".pdf";
                                            }
                                            dataFileList.Add(dataFileName);

                                            // Valiate Control File / PDF Agreement
                                            if ((controlFile.Root.Element("DocumentName") == null) || (!(File.Exists(Path.Combine(extractedDirectory.FullName, dataFileName)))))
                                            {
                                                errorCode = "02";
                                                errorDescription = "Invalid data file reference from control file";
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    long fileSize = new System.IO.FileInfo(Path.Combine(extractedDirectory.FullName, dataFileName)).Length / 1024 / 1024;
                                                    if (fileSize > 100)
                                                    {
                                                        errorCode = "05";
                                                        errorDescription = String.Format("Data file {0} exceeds 1000MB in size", dataFileName);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    fileStatus = false;
                                                    RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, "Error calculating file size. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                                                }
                                            }
                                        }
                                    }
                                    // Validations complete, process file
                                }
                                XDocument errorfile = new XDocument(
                                        new XDeclaration("1.0", "utf-8", "yes"));

                                XElement Envelope = new XElement("RAErrorControlFile");
                                Envelope.Add(new XElement("EDITransaction_Type", "RA"));
                                if (controlFile.Root != null)
                                {
                                    if (controlFile.Root.Element("DocumentType") != null)
                                    {
                                        Envelope.Add(new XElement("DocumentType", controlFile.Root.Element("DocumentType").Value));
                                    }
                                    foreach (String dataFileListMember in dataFileList)
                                    {
                                        Envelope.Add(new XElement("DocumentName", dataFileListMember));
                                    }
                                    if (controlFile.Root.Element("RA_number") != null)
                                    {
                                        Envelope.Add(new XElement("RA_number", controlFile.Root.Element("RA_number").Value));
                                    }
                                    if (controlFile.Root.Element("RA_Number") != null)
                                    {
                                        Envelope.Add(new XElement("RA_Number", controlFile.Root.Element("RA_Number").Value));
                                    }
                                    if (controlFile.Root.Element("RADate") != null)
                                    {
                                        Envelope.Add(new XElement("RADate", controlFile.Root.Element("RADate").Value));
                                    }
                                    if (controlFile.Root.Element("PaymentIdentifier") != null)
                                    {
                                        Envelope.Add(new XElement("PaymentIdentifier", controlFile.Root.Element("PaymentIdentifier").Value));
                                    }
                                    // Envelope.Add(new XElement("Payer_Name", controlfile.Root.Element("Payer_Name").Value));
                                    if (controlFile.Root.Element("Provider_ID") != null)
                                    {
                                        Envelope.Add(new XElement("Provider_ID", controlFile.Root.Element("Provider_ID").Value));
                                    }
                                    if (controlFile.Root.Element("Provider_NPI") != null)
                                    {
                                        Envelope.Add(new XElement("Provider_NPI", controlFile.Root.Element("Provider_NPI").Value));
                                    }
                                    Envelope.Add(new XElement("Status"));
                                    Envelope.Add(new XElement("AttachmentControlNumber"));
                                    Envelope.Add(new XElement("BatchDate", DateTime.Now.ToString("yyyyMMdd HH:mm")));
                                    if (controlFile.Root.Element("UUID") != null)
                                    {
                                        Envelope.Add(new XElement("UUID", controlFile.Root.Element("UUID").Value));
                                    }
                                }
                                XElement error = new XElement("Error");
                                // no error, load files
                                if (errorCode == "")
                                {
                                    error.Add(new XElement("ErrorCode"));
                                    error.Add(new XElement("ErrorDescription"));
                                    try
                                    {
                                        foreach (String dataFileListElement in dataFileList)
                                        {
                                            documentId = RemittanceAdviceHelper.InsertREG_DOCUMENT(regId, dataFileListElement);

                                            RemittanceAdviceHelper.UpdateProcessDocDtlsINBOUND_RA_MCE_DTLS(recordID, documentId, dataFileListElement, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);

                                            int attachmentID = 0;
                                            if (documentId != 0)
                                            {
                                                int documentType = 69;
                                                try
                                                {
                                                    attachmentID = Convert.ToInt32(RemittanceAdviceHelper.InsertDOCUMENT_ATTACHMENT_XREF(documentType, documentId));

                                                    int controlID = 0;
                                                    if (attachmentID != 0)
                                                    {
                                                        try
                                                        {
                                                            List<SqlParameter> controlParameters = new List<SqlParameter>();
                                                            // JLB -- don't trust document passed validation, so enter empty strings if necessary
                                                            if (controlFile.Root.Element("EDITransaction_Type") != null)
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("edi_transaction_type", DbType.String, controlFile.Root.Element("EDITransaction_Type").Value, false));
                                                            }
                                                            else
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("edi_transaction_type", DbType.String, "", false));
                                                            }

                                                            if (controlFile.Root.Element("Tax_ID") != null)
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("tax_id", DbType.String, controlFile.Root.Element("Tax_ID").Value, false));
                                                            }
                                                            else
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("tax_id", DbType.String, "", false));
                                                            }
                                                            if (controlFile.Root.Element("RA_number") != null)
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("ra_number", DbType.String, controlFile.Root.Element("RA_number").Value, false));
                                                            }
                                                            else if (controlFile.Root.Element("RA_Number") != null)
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("ra_number", DbType.String, controlFile.Root.Element("RA_Number").Value, false));
                                                            }
                                                            else
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("ra_number", DbType.String, "", false));
                                                            }
                                                            if (controlFile.Root.Element("PaymentIdentifier") != null)
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("payment_identifier", DbType.String, controlFile.Root.Element("PaymentIdentifier").Value, false));
                                                            }
                                                            else
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("payment_identifier", DbType.String, "", false));
                                                            }

                                                            List<SqlParameter> providerIdParameters = new List<SqlParameter>();
                                                            providerIdParameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId.ToString(), false));
                                                            DataSet dsProviderInfo = DataAccess.ExecuteStoredProcedure("usp_SelectProviderByRegId", providerIdParameters, "ProviderInfo");
                                                            controlParameters.Add(SqlParms.CreateParameter("provider_id", DbType.String, dsProviderInfo.Tables[0].Rows[0]["MEDICAID_ID"], false));
                                                            
                                                            providerIdParameters = new List<SqlParameter>();
                                                            providerIdParameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId.ToString(), false));
                                                            dsProviderInfo = DataAccess.ExecuteStoredProcedure("usp_SelectProviderByRegId", providerIdParameters, "ProviderInfo");
                                                            controlParameters.Add(SqlParms.CreateParameter("provider_npi", DbType.String, dsProviderInfo.Tables[0].Rows[0]["NPI"], false));

                                                            if (controlFile.Root.Element("SenderID") != null)
                                                            {
                                                                // TO DO - replace with DB lookup
                                                                String senderID = controlFile.Root.Element("SenderID").Value;
                                                                if (senderID.Equals("0021914"))
                                                                {
                                                                    senderID = "4";
                                                                }
                                                                if (senderID.Equals("0021920") || senderID.Equals("341858379"))
                                                                {
                                                                    senderID = "2";
                                                                }
                                                                if (senderID.Equals("0002937") || senderID.Equals("ANTHEM"))
                                                                {
                                                                    senderID = "3";
                                                                }
                                                                if (senderID.Equals("0004202") || senderID.Equals("421406317") || senderID.Equals("D004202"))
                                                                {
                                                                    senderID = "5";
                                                                }
                                                                if (senderID.Equals("0003150"))
                                                                {
                                                                    senderID = "6";
                                                                }
                                                                if (senderID.Equals("0021919") || senderID.Equals("0499441430000"))
                                                                {
                                                                    senderID = "7";
                                                                }
                                                                if (senderID.Equals("0007316"))
                                                                {
                                                                    senderID = "8";
                                                                }
                                                                if (senderID.Equals("0007610"))
                                                                {
                                                                    senderID = "9";
                                                                }
                                                                controlParameters.Add(SqlParms.CreateParameter("sender_id", DbType.String, senderID, false));
                                                            }
                                                            else
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("sender_id", DbType.String, "", false));
                                                            }

                                                            if (controlFile.Root.Element("ReceiverID") != null)
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("receiver_id", DbType.String, controlFile.Root.Element("ReceiverID").Value, false));
                                                            }
                                                            else
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("receiver_id", DbType.String, "", false));
                                                            }

                                                            if (controlFile.Root.Element("DocumentType") != null)
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("document_type", DbType.String, controlFile.Root.Element("DocumentType").Value, false));
                                                            }
                                                            else
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("document_type", DbType.String, "", false));
                                                            }

                                                            if (controlFile.Root.Element("RADate") != null)
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("ra_date", DbType.String, controlFile.Root.Element("RADate").Value, false));
                                                            }
                                                            else
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("ra_date", DbType.DateTime, "", false));
                                                            }

                                                            if (controlFile.Root.Element("TimeStamp") != null)
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("time_stamp", DbType.String, controlFile.Root.Element("TimeStamp").Value, false));
                                                            }
                                                            else
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("time_stamp", DbType.DateTime, "", false));
                                                            }

                                                            controlParameters.Add(SqlParms.CreateParameter("document_name", DbType.String, dataFileListElement, false));

                                                            if (controlFile.Root.Element("UUID") != null)
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("uuid", DbType.String, controlFile.Root.Element("UUID").Value, false));
                                                            }
                                                            else
                                                            {
                                                                controlParameters.Add(SqlParms.CreateParameter("uuid", DbType.String, "", false));
                                                            }
                                                            controlParameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentId, false));
                                                            controlParameters.Add(SqlParms.CreateParameter("DOCUMENT_ID_PDF", DbType.Int32, documentId, false));
                                                            controlID = Convert.ToInt32(DataAccess.ExecuteScalar("insertREMITTANCE_ADVICE_CONTROL", controlParameters));

                                                            int detailID = 0;
                                                            if (controlID != 0)
                                                            {
                                                                try
                                                                {
                                                                    if (controlFile.Root.Element("Detail") != null)
                                                                    {
                                                                        foreach (XElement icn in controlFile.Root.Descendants("ICN"))
                                                                        {
                                                                            detailID = RemittanceAdviceHelper.InsertREMITTANCE_ADVICE_Detail(controlID, icn.Value);
                                                                        }
                                                                    }
                                                                    // Move files from input to output, upload to onbase
                                                                    OnBaseInterface onBaseInterface = new OnBaseInterface();
                                                                    File.Copy(Path.Combine(extractPath, dataFileListElement), Path.Combine(localPathOutput, dataFileListElement), true);

                                                                    byte[] fileBytes = File.ReadAllBytes(Path.Combine(localPathOutput, dataFileListElement));
                                                                    onBaseDocumentID = onBaseInterface.GetSubmitedFileID(regId, documentId, fileBytes, dataFileListElement);

                                                                    if (onBaseDocumentID == 0)
                                                                    {
                                                                        fileStatus = false;
                                                                        RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, "Failed to upload to onBase", CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                                                                    }
                                                                    else
                                                                    {
                                                                        fileStatus = true;
                                                                    }

                                                                    File.Delete(Path.Combine(localPathOutput, dataFileListElement));
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    fileStatus = false;
                                                                    RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, "Failed to insert Detail Record. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            fileStatus = false;
                                                            RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, "Failed to insert Control Record. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    fileStatus = false;
                                                    RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, "Failed to insert Document Record. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        fileStatus = false;
                                        RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, "Failed to insert Document Record. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                                    }
                                }
                                else
                                {
                                    fileStatus = false;
                                    RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, String.Format("Error Found in Control File - not processing.  Error Description: {0}", errorDescription), CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                                    // Control File Validation Failed - log reason, archive ZIP file
                                    error.Add(new XElement("ErrorCode", errorCode.ToString()));
                                    error.Add(new XElement("ErrorDescription", errorDescription));
                                }

                                // Save Return File
                                Envelope.Add(error);
                                //errorfile.Add(Envelope);

                                errorFileName = extractedFile.Name.Replace("RA.xml", "RA.Errors.xml"); 
                                Envelope.Save(Path.Combine(localPathReturn, errorFileName)); //, ".PDF", "XML", RegexOptions.IgnoreCase));

                                string responseXML = Envelope.ToString();
                                RemittanceAdviceHelper.UpdateResponseXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseSuccess, errorFileName, responseXML, null, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                            }
                        }
                        catch (Exception ex)
                        {
                            fileStatus = false;
                            RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, "Error in ZIP Contents handling. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                        }
                    }

                    // clean up
                    foreach (FileInfo extractedFile in extractedDirectory.GetFiles())
                    {
                        extractedFile.Delete();
                    }
                    extractedDirectory.Delete();
                    if (fileStatus == true)
                    {
                        file.CopyTo(Path.Combine(localPathArchive, file.Name), true);
                    }
                    else
                    {
                        file.CopyTo(Path.Combine(localReProcess, file.Name), true);
                    }
                    file.Delete();
                }
                catch (Exception ex)
                {
                    RemittanceAdviceHelper.UpdateStatusXMLINBOUND_RA_MCE_DTLS(recordID, CON.RemittanceAdviceMCE.RemittanceAdviceMCEResponseFailure, "Error in ZIP Contents handling. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, CON.RemittanceAdviceMCE.RemittanceAdviceMCEUpdate);
                    RemittanceAdviceHelper.CreateLogEntry("ProcessReadmittancePackage", String.Format("Error in ZIP file handling: {0}, {1}", ex.Message, ex.StackTrace), CON.RemittanceAdviceMCE.LogPriorityInfo);

                    foreach (FileInfo fileProcess in localProcessingDirectory.GetFiles())
                    {
                        fileProcess.Delete();
                    }
                    foreach (DirectoryInfo dirProcess in localProcessingDirectory.GetDirectories())
                    {
                        dirProcess.Delete(true);
                    }
                    file.CopyTo(Path.Combine(localReProcess, file.Name), true);
                    file.Delete();

                }
            }
        }
    }
}
