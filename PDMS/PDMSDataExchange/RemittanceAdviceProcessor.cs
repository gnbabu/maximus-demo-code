using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml.Schema;
using Corp.Core.Libraries;
using System.Xml;

namespace MAXIMUS.DataExchange.PDMS
{
    public class RemittanceAdviceProcessor : BaseJob, IJob
    {
        public RemittanceAdviceProcessor(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private const string thisGuidString = "EEC1A2A3-6AC2-446D-9F07-DB64946A7014";
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
                // remove any characters outside the valid UTF-8 range as well as all control characters
                // except tabs and new lines
                //if ((ch < 0x00FD && ch > 0x001F) || ch == '\t' || ch == '\n' || ch == '\r')
                //if using .NET version prior to 4, use above logic
                if (XmlConvert.IsXmlChar(ch)) //this method is new in .NET 4
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();

        }

        private void ProcessReadmittancePackage(Guid jobId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            log.CreateLogEntry(String.Format("Initiating RemittanceAdviceProcessor, JobID: {0}", jobId), Logging.LogPriority.Information);

            DirectoryInfo localDirectory;
            string localPath = AppSettings.Get("RALocalPath");

            string localPathOutput = AppSettings.Get("RALocalPathOutput"); // AppSettings.Get("RALocalPath");
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

            localDirectory = new DirectoryInfo(localPath);
            FileCompression fc = new FileCompression(this.ThreadId);


            foreach (FileInfo file in localDirectory.GetFiles())
            {
                try
                {
                    log.CreateLogEntry(String.Format("Loading File: {0}", file.Name), Logging.LogPriority.Information);

                    string extractFile = Regex.Replace(file.Name, ".zip", string.Empty, RegexOptions.IgnoreCase);
                    string extractPath = Path.Combine(localDirectory.FullName, extractFile);

                    fc.UnzipFile(file.FullName, extractPath);

                    DirectoryInfo extractedDirectory;
                    extractedDirectory = new DirectoryInfo(extractPath);
                    XDocument controlFile = new XDocument();
                    // FileInfo dataFile = null; //  new FileInfo(@".\MCERemittanceAdviceProcessorSchema.xsd");

                    String errorCode = "";
                    String errorDescription = "";
                    string msg = "";

                    int fileCount = 0;

                    //                String controlFileValidationSchema = @"<xs:schema attributeFormDefault=""unqualified"" elementFormDefault=""qualified"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
                    //	<xs:element name=""Remittance_Advice_ControlFile"">
                    //		<xs:complexType>
                    //			<xs:sequence>
                    //				<xs:element type=""xs:string"" name=""EDITransaction_Type"" maxOccurs=""1"" minOccurs=""1""/>
                    //				<xs:element type=""xs:string"" name=""Tax_ID"" maxOccurs=""1"" minOccurs=""1""/>
                    //				<xs:element type=""xs:string"" name=""RA_number"" maxOccurs=""1"" minOccurs=""1""/>
                    //				<xs:element type=""xs:string"" name=""PaymentIdentifier"" maxOccurs=""1"" minOccurs=""1""/>

                    //				<xs:choice minOccurs=""1"" maxOccurs=""2"">
                    //					<xs:element type=""xs:string"" name=""Provider_ID""/>
                    //					<xs:element type=""xs:string"" name=""Provider_NPI""/>
                    //				</xs:choice>
                    //				<xs:element type=""xs:string"" name=""SenderID"" maxOccurs=""1"" minOccurs=""1""/>
                    //				<xs:element type=""xs:string"" name=""ReceiverID"" maxOccurs=""1"" minOccurs=""1""/>
                    //				<xs:element type=""xs:string"" name=""DocumentType"" maxOccurs=""1"" minOccurs=""1""/>
                    //				<xs:element type=""xs:string"" name=""RADate"" maxOccurs=""1"" minOccurs=""1""/>
                    //				<xs:element type=""xs:string"" name=""TimeStamp"" maxOccurs=""1"" minOccurs=""1""/>
                    //				<xs:element type=""xs:string"" name=""DocumentName"" maxOccurs=""1"" minOccurs=""1""/>
                    //				<xs:element type=""xs:string"" name=""UUID"" maxOccurs=""1"" minOccurs=""1""/>
                    //				<xs:element name=""Detail"" maxOccurs=""1"" minOccurs=""1"">
                    //					<xs:complexType>
                    //						<xs:sequence>
                    //							<xs:element type=""xs:string"" name=""ICN"" maxOccurs=""unbounded"" minOccurs=""1""/>
                    //						</xs:sequence>
                    //					</xs:complexType>
                    //				</xs:element>
                    //			</xs:sequence>
                    //		</xs:complexType>
                    //	</xs:element>
                    //</xs:schema>";

                    List<String> dataFileList = new List<String>();
                    // List<String> dataFileDocIDList = new List<String>();

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

                            if (!(archiveSenderID.Equals(controlSenderID)))
                            {
                                String senderID = extractedDirectory.Name.Split('.')[0];
                                String MCEDirectoryName = String.Format("{0}.{1}.RA.Errors", senderID, DateTime.Now.ToString("yyyyMMdd"));
                                if (!(Directory.Exists(Path.Combine(localPathReturn, MCEDirectoryName))))
                                {
                                    Directory.CreateDirectory(Path.Combine(localPathReturn, MCEDirectoryName));
                                }
                                XDocument errorfile = new XDocument(
                                        new XDeclaration("1.0", "utf-8", "yes"));

                                XElement Envelope = new XElement("RAErrorControlFile");
                                Envelope.Add(new XElement("EDITransaction_Type", "RA"));
                                XElement error = new XElement("Error");
                                error.Add(new XElement("ErrorCode", "-1"));
                                error.Add(new XElement("ErrorDescription", "Invalid Control Filename"));
                                Envelope.Add(error);
                                Envelope.Save(Path.Combine(localPathReturn, MCEDirectoryName, extractedFile.Name.Replace("RA.xml", "RA.Errors.xml")));
                            }
                            else
                            {
                                log.CreateLogEntry(String.Format("Loading Control File: {0}", extractedFile.Name));
                                fileCount++;
                                string text = File.ReadAllText(extractedFile.FullName);
                                text = RemoveTroublesomeCharacters(text);
                                text = text.Substring(text.IndexOf("<Remitt"), text.Length - text.IndexOf("<Remitt"));
                                // Console.WriteLine(text);
                                File.WriteAllText(extractedFile.FullName, text, Encoding.Unicode);
                                // 
                                // XmlSchemaSet schemas = new XmlSchemaSet();
                                // schemas.Add(null, controlFileValidationSchema);
                                try
                                {
                                    controlFile = XDocument.Load(extractedFile.FullName);
                                }
                                catch (Exception ex)
                                {
                                    log.CreateLogEntry(String.Format("Could not parse XML document {0}.  Message: {1}", extractedFile.Name, ex.Message));
                                    errorCode = "06";
                                    errorDescription = "Invalid Control File Format";
                                }
                                // JLB as long as required fields are presented, do not validate file against schema

                                //controlFile.Validate(schemas, (o, e) =>
                                //{
                                //    msg += e.Message + Environment.NewLine;
                                //});

                                if (msg != "") // control file is valid XML -- this is clunky, do better
                                {
                                    errorCode = "06";
                                    errorDescription = "Invalid Control File Format";
                                }

                                int regId = 0;
                                int documentId = 0;
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
                                    log.CreateLogEntry("Failed to validate if Provider Exists. Exception Message: " + ex.Message + " Exception Stack = "
                                             + ex.StackTrace, Logging.LogPriority.Error);
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
                                                    log.CreateLogEntry(String.Format("Error calculating file size: {0}", ex.Message));
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
                                            List<SqlParameter> parameters = new List<SqlParameter>();

                                            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                                            parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, 8, false));
                                            parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, dataFileListElement, false));
                                            parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, "Remittance Advice", false));
                                            parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, dataFileListElement, false));
                                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, AppSettings.Get("GetCurrentUser"), false));
                                            log.CreateLogEntry(String.Format("Executing {0} with {1}", "usp_InsertREG_DOCUMENT", parameters));
                                            documentId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertREG_DOCUMENT", parameters));

                                            int attachmentID = 0;
                                            if (documentId != 0)
                                            {
                                                int documentType = 69;
                                                try
                                                {
                                                    List<SqlParameter> attachmentXrefParameters = new List<SqlParameter>();
                                                    attachmentXrefParameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_ID", DbType.Int32, documentType, false)); // is this in constants?
                                                    attachmentXrefParameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentId, false));
                                                    attachmentXrefParameters.Add(SqlParms.CreateParameter("CREATED_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                                                    attachmentXrefParameters.Add(SqlParms.CreateParameter("CREATED_BY_MODIFIED_USER", DbType.Guid, AppSettings.Get("GetCurrentUser"), false));
                                                    attachmentXrefParameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, AppSettings.Get("GetCurrentUser"), false));
                                                    log.CreateLogEntry(String.Format("Executing {0} with {1}, {2}, {3}, {4}", "insertDOCUMENT_ATTACHMENT_XREF", 69, documentId, AppSettings.Get("GetCurrentUser"), AppSettings.Get("GetCurrentUser")));
                                                    attachmentID = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT_ATTACHMENT_XREF", attachmentXrefParameters));

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
                                                            log.CreateLogEntry(String.Format("Executing {0} with {1}", "usp_SelectProviderByRegId", regId));
                                                            DataSet dsProviderInfo = DataAccess.ExecuteStoredProcedure("usp_SelectProviderByRegId", providerIdParameters, "ProviderInfo");
                                                            controlParameters.Add(SqlParms.CreateParameter("provider_id", DbType.String, dsProviderInfo.Tables[0].Rows[0]["MEDICAID_ID"], false));


                                                            providerIdParameters = new List<SqlParameter>();
                                                            providerIdParameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId.ToString(), false));
                                                            log.CreateLogEntry(String.Format("Executing {0} with {1}", "usp_SelectProviderByRegId", regId));
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
                                                            log.CreateLogEntry(String.Format("Executing {0} with {1}", "insertREMITTANCE_ADVICE_CONTROL", controlParameters));
                                                            controlID = Convert.ToInt32(DataAccess.ExecuteScalar("insertREMITTANCE_ADVICE_CONTROL", controlParameters));

                                                            int detailID = 0;
                                                            if (controlID != 0)
                                                            {
                                                                try
                                                                {
                                                                    // ICN is not required, but log it if we have it

                                                                    if (controlFile.Root.Element("Detail") != null)
                                                                    {
                                                                        foreach (XElement icn in controlFile.Root.Descendants("ICN"))
                                                                        {
                                                                            List<SqlParameter> detailParameters = new List<SqlParameter>();

                                                                            detailParameters.Add(SqlParms.CreateParameter("remittance_advice_id", DbType.Int32, controlID, false));
                                                                            detailParameters.Add(SqlParms.CreateParameter("ICN", DbType.String, icn.Value, false));
                                                                            log.CreateLogEntry(String.Format("Executing {0} with {1}", "insertREMITTANCE_ADVICE_Detail", detailParameters));
                                                                            detailID = Convert.ToInt32(DataAccess.ExecuteScalar("insertREMITTANCE_ADVICE_Detail", detailParameters));
                                                                        }
                                                                    }
                                                                    // Move files from input to output, upload to onbase
                                                                    log.CreateLogEntry("Completed Remittance Advice file processing - copying data file", Logging.LogPriority.Information);

                                                                    OnBaseInterface onBaseInterface = new OnBaseInterface();
                                                                    File.Copy(Path.Combine(extractPath, dataFileListElement), Path.Combine(localPathOutput, dataFileListElement), true);

                                                                    byte[] fileBytes = File.ReadAllBytes(Path.Combine(localPathOutput, dataFileListElement));
                                                                    onBaseInterface.SubmitFile(regId, documentId, fileBytes, dataFileListElement);
                                                                    File.Delete(Path.Combine(localPathOutput, dataFileListElement));
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    log.CreateLogEntry("Failed to insert Detail Record. Exception Message: " + ex.Message + " Exception Stack = "
                                                                                + ex.StackTrace, Logging.LogPriority.Error);
                                                                }

                                                            }


                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            log.CreateLogEntry("Failed to insert Control Record. Exception Message: " + ex.Message + " Exception Stack = "
                                                                        + ex.StackTrace, Logging.LogPriority.Error);
                                                        }

                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    log.CreateLogEntry("Failed to insert record into the Document. Exception Message: " + ex.Message + " Exception Stack = "
                                                                + ex.StackTrace, Logging.LogPriority.Error);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.CreateLogEntry("Failed to insert record into the Document_XREF_Attachment table. Exception Message: " + ex.Message + " Exception Stack = "
                                                    + ex.StackTrace, Logging.LogPriority.Error);

                                    }
                                }
                                else
                                {
                                    // Control File Validation Failed - log reason, archive ZIP file
                                    log.CreateLogEntry(String.Format("Error Found in Control File - not processing.  Error Description: {0}", errorDescription), Logging.LogPriority.Information);

                                    error.Add(new XElement("ErrorCode", errorCode.ToString()));
                                    error.Add(new XElement("ErrorDescription", errorDescription));
                                }

                                // Save Return File
                                Envelope.Add(error);
                                //errorfile.Add(Envelope);
                                log.CreateLogEntry(String.Format("Saving Return File Extracted File {0}", extractedFile.Name), Logging.LogPriority.Information);
                                Envelope.Save(Path.Combine(localPathReturn, extractedFile.Name.Replace("RA.xml", "RA.Errors.xml"))); //, ".PDF", "XML", RegexOptions.IgnoreCase));
                            }
                        }
                        catch (Exception ex)
                        {
                            log.CreateLogEntry(String.Format("Error in ZIP Contents handling: {0}, {1}", ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                        }
                    }

                    // clean up
                    foreach (FileInfo extractedFile in extractedDirectory.GetFiles())
                    {
                        log.CreateLogEntry(String.Format("Deleting Extracted File {0}", extractedFile.Name), Logging.LogPriority.Information);
                        extractedFile.Delete();
                    }
                    log.CreateLogEntry(String.Format("Deleting Archive Directory {0}", extractedDirectory.Name), Logging.LogPriority.Information);
                    extractedDirectory.Delete();
                    file.CopyTo(Path.Combine(localPathArchive, file.Name), true);
                    log.CreateLogEntry(String.Format("Deleting Archive File {0}", file.Name), Logging.LogPriority.Information);
                    file.Delete();
                } catch (Exception ex)
                {
                    log.CreateLogEntry(String.Format("Error in ZIP file handling: {0}, {1}", ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                }
            }
            // zip return files
            //localDirectory = new DirectoryInfo(localPathReturn);
            //string previousMCE = "0";
            //string directoryName = "";
            //foreach (FileInfo file in localDirectory.GetFiles("*.xml"))
            //{
            //    string fileName = file.Name;
            //    string currentMCE = fileName.Split('.')[0];
            //    if ((currentMCE.Length > 10) && currentMCE.Contains("-"))
            //    {
            //        currentMCE = fileName.Split('-')[0];
            //    }

            //    if (currentMCE != previousMCE)
            //    {
            //        previousMCE = currentMCE;
            //        directoryName = String.Format("{0}{1}{2}.{3}.RA.Errors", localPathReturn, Path.DirectorySeparatorChar, currentMCE, DateTime.Now.ToString("yyyyMMdd"));
            //        if (!(Directory.Exists(directoryName)))
            //        {
            //            Directory.CreateDirectory(directoryName);
            //        }
            //    }
            //    log.CreateLogEntry(String.Format("Moving Return File {0} to {1}", file.FullName, directoryName), Logging.LogPriority.Information);
            //    File.Copy(file.FullName, Path.Combine(directoryName, file.Name.Replace(".xml", ".Errors.xml").Replace(".Errors.Errors", ".Errors")), true);
            //    File.Delete(file.FullName);
            //}

            // create ZIP files for delivery
            //foreach (DirectoryInfo directory in localDirectory.GetDirectories())
            //{
            //    if (directory.FullName.Contains(".RA.")) {
            //        log.CreateLogEntry(String.Format("attempting to Zip / Delete {0}", directory.FullName), Logging.LogPriority.Information);
            //        fc.ZipDirectory(directory.FullName, String.Format("{0}.zip", directory.FullName));
            //        log.CreateLogEntry(String.Format("attempting to delete directory {0}", directory.FullName), Logging.LogPriority.Information);
            //        directory.Delete(true);
            //    }
            //}
        }
    }
}
