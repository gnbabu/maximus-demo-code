using Codaxy.WkHtmlToPdf;
using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.ProcessDocuments.PDMS
{

    /// <summary>
    ///     This class provides for the creation of PDMS documents.
    /// </summary>
    public class ProcessDocuments
    {
        private const string dateFormat = "MM/dd/yyyy";
        private Guid m_threadId;
        private int _addressRegId = -1;
        public string userName = "";
        private Guid ThreadId
        {
            get
            {
                return this.m_threadId;
            }
            set
            {
                this.m_threadId = value;
            }
        }

        #region Constructors
        public ProcessDocuments()
        {
            // generate a new thread id GUID
            ThreadId = Guid.NewGuid();
        }

        public ProcessDocuments(Guid threadId)
        {
            this.ThreadId = threadId;
        }

        #endregion

        #region Logging Objects

        //private int logCnt = 0;

        #endregion

        #region Public Methods

        public bool GenerateDIDDContract(int regID, out string pdfFileName)
        {
            pdfFileName = string.Empty;
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string logHeader = string.Format("Error generating DIDD Contract. RegID- {0}:", regID.ToString());
            string templatePath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

            if (string.IsNullOrEmpty(templatePath))
            {
                log.CreateLogEntry(string.Format("{0} template path (PDMS_SVC_TemplatesPath) not found in AppSettings", logHeader), Logging.LogPriority.Error);
                return false;
            }

            string appNbr, appSuffix, providerName, effectiveDate, psSignDate, diddSignDate, providerSignDate, diddCommissonerName, psCommissonerName,
                            taxID, addrLine1, addrLine2, signatory, addrCity, addrState, addrZip, addrPhone, addrFax, legalName, contractStartDate, contractEndDate = string.Empty;
            int contractID;

            try
            {
                DataSet ds = DIDDController.SelectDIDDContractDetails(regID);
                if (!ObjectControllerHelper.HasRows(ds))
                {
                    log.CreateLogEntry(string.Format("{0} contract details were not found for this DIDD registration", logHeader), Logging.LogPriority.Error);
                    return false;
                }

                DataRow drOne = ds.Tables[0].Rows[0];
                contractID = ObjectControllerHelper.GetInt("REG_CONTRACT_ID", drOne);
                appNbr = ObjectControllerHelper.GetString("ContractNumber", drOne);
                appSuffix = ObjectControllerHelper.GetString("ContractSufix", drOne);
                providerName = ObjectControllerHelper.GetString("ProviderName", drOne);
                effectiveDate = drOne.IsNull("EffectiveDate") ? string.Empty : ObjectControllerHelper.GetDateTime("EffectiveDate", drOne).ToShortDateString();
                psSignDate = drOne.IsNull("PSSignDate") ? string.Empty : ObjectControllerHelper.GetDateTime("PSSignDate", drOne).ToShortDateString();
                diddSignDate = drOne.IsNull("DIDDSignDate") ? string.Empty : ObjectControllerHelper.GetDateTime("DIDDSignDate", drOne).ToShortDateString();
                providerSignDate = drOne.IsNull("ProviderSignDate") ? string.Empty : ObjectControllerHelper.GetDateTime("ProviderSignDate", drOne).ToShortDateString();
                signatory = ObjectControllerHelper.GetString("Signatory", drOne);
                diddCommissonerName = ObjectControllerHelper.GetString("DIDDCommissioner", drOne);
                psCommissonerName = ObjectControllerHelper.GetString("PSCommissioner", drOne);
                taxID = ObjectControllerHelper.GetString("TaxID", drOne);
                addrLine1 = ObjectControllerHelper.GetString("PrimaryContactAddressLine1", drOne);
                addrLine2 = ObjectControllerHelper.GetString("PrimaryContactAddressLine2", drOne);
                addrCity = ObjectControllerHelper.GetString("PrimaryContactCity", drOne);
                addrState = ObjectControllerHelper.GetString("PrimaryContactState", drOne);
                addrZip = ObjectControllerHelper.GetString("PrimaryContactZip", drOne);
                addrPhone = ObjectControllerHelper.FormatPhone(ObjectControllerHelper.GetString("PrimaryContactPhone", drOne));
                addrFax = ObjectControllerHelper.FormatPhone(ObjectControllerHelper.GetString("PrimaryContactFax", drOne));
                legalName = ObjectControllerHelper.GetString("LegalName", drOne);
                contractStartDate = drOne.IsNull("ContractStartDate") ? string.Empty : ObjectControllerHelper.GetDateTime("ContractStartDate", drOne).ToShortDateString();
                contractEndDate = drOne.IsNull("ContractEndDate") ? string.Empty : ObjectControllerHelper.GetDateTime("ContractEndDate", drOne).ToShortDateString();
                List<string> services = GetServices(ds.Tables[0]);

                bool isAmendment = string.IsNullOrEmpty(appSuffix) || appSuffix == "0" ? false : true;
                string documentName = isAmendment ? "DIDD Contract Amendment" : "DIDD Contract";
                string templateName = GetTemplateName(documentName);
                if (string.IsNullOrEmpty(templateName))
                {
                    log.CreateLogEntry(string.Format("{0} Document Type \"{1}\" not found", logHeader, documentName), Logging.LogPriority.Error);
                    return false;
                }

                if (templatePath.LastIndexOf("\\") != templatePath.Length - 1) templatePath += "\\";

                string[] finalDoc = MergeDIDDTemplateWithRegData(regID, appNbr, appSuffix, providerName, effectiveDate, psSignDate, diddSignDate,
                                            providerSignDate, diddCommissonerName, psCommissonerName, taxID, contractStartDate, contractEndDate,
                                            signatory, legalName, addrLine1, addrLine2, addrCity, addrState, addrZip, addrPhone, addrFax, services, templatePath, templateName);

                if (finalDoc.Length == 0)
                {
                    log.CreateLogEntry(string.Format("{0} Creation of PDF failed", logHeader), Logging.LogPriority.Error);
                    return false;
                }


                string storagePath = AppSettings.Get("DIDDContract_Path", string.Empty);
                string fileName = SaveAsHtml(regID, templateName, storagePath, finalDoc);
                if (string.IsNullOrEmpty(fileName))
                {
                    return false;
                }
                pdfFileName = ConvertHTMLToPDF(regID, storagePath, fileName, string.Empty, string.Empty);
                if (string.IsNullOrEmpty(pdfFileName))
                {
                    return false;
                }

                //insert DOCUMENT record and REG_DOCUMENT_XREF
                if (contractID > 0)
                {
                    string signedBy = !string.IsNullOrEmpty(psSignDate) ? Constants.UserRoleType.ProviderServicesCommissioner : !string.IsNullOrEmpty(diddSignDate)
                        ? Constants.UserRoleType.DIDDCommissioner : !string.IsNullOrEmpty(providerSignDate) ? Constants.UserRoleType.ProviderAdministrator : string.Empty;
                    string contractName = isAmendment ? string.Concat(appNbr, "-", appSuffix) : appNbr;
                    string name = string.Format("{0}: {1}", documentName, contractName);
                    string description = string.Format("Auto generated DIDD contract on signature/approval of: {0}", signedBy);

                    if (InsertDocumentRecord(regID, pdfFileName, signedBy, name, description) == false)
                    {
                        log.CreateLogEntry(string.Format("{0} Insert of Document record failed", logHeader), Logging.LogPriority.Error);
                        return false;
                    }
                }

                return true;

            }
            catch (PdfConvertTimeoutException ex1)
            {
                log.CreateLogEntry("PdfConvertTimeoutException RegID-" + regID.ToString() + ": " + ex1.Message, Logging.LogPriority.Error);
            }
            catch (PdfConvertException ex2)
            {
                log.CreateLogEntry("PdfConvertException RegID-" + regID.ToString() + ": " + ex2.Message, Logging.LogPriority.Error);
            }
            catch (Exception ex3)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex3.Message), Logging.LogPriority.Error);
            }
            return false;
        }

        public bool GenerateICF_IDDContract(int regID, out string pdfFileName)
        {
            pdfFileName = string.Empty;
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string logHeader = string.Format("Error generating ICF IID Contract. RegID- {0}:", regID.ToString());

            string templatePath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

            if (string.IsNullOrEmpty(templatePath))
            {
                log.CreateLogEntry(string.Format("{0} Creation of PDF failed", logHeader), Logging.LogPriority.Error);
                return false;
            }

            string submitDate, day, month, year, contractStartDate, contractEndDate, providerName, providerNumber, providerSignDate,
                psCommissioner, psSignDate, addrLine1, addrLine2, addrCityStateZip, administrator = string.Empty;
            int contractID;
            try
            {
                DataSet ds = RegistrationController.SelectICF_IIDContractDetails(regID);
                if (!ObjectControllerHelper.HasRows(ds))
                {
                    log.CreateLogEntry(string.Format("{0} contract details were not found for this DCS registration", logHeader), Logging.LogPriority.Error);
                    return false;
                }

                DataRow drOne = ds.Tables[0].Rows[0];
                contractID = ObjectControllerHelper.GetInt("REG_CONTRACT_ID", drOne);
                submitDate = ObjectControllerHelper.GetString("SubmitDate", drOne);
                day = ObjectControllerHelper.GetString("DayPart", drOne);
                month = ObjectControllerHelper.GetString("MonthPart", drOne);
                year = ObjectControllerHelper.GetString("YearPart", drOne);
                contractStartDate = drOne.IsNull("ContractStartDate") ? string.Empty : ObjectControllerHelper.GetDateTime("ContractStartDate", drOne).ToShortDateString();
                contractEndDate = drOne.IsNull("ContractEndDate") ? string.Empty : ObjectControllerHelper.GetDateTime("ContractEndDate", drOne).ToShortDateString();
                providerName = ObjectControllerHelper.GetString("ProviderName", drOne);
                providerNumber = ObjectControllerHelper.GetString("ProviderNumber", drOne);
                providerSignDate = drOne.IsNull("ProviderSignDate") ? string.Empty : ObjectControllerHelper.GetDateTime("ProviderSignDate", drOne).ToShortDateString();
                psCommissioner = ObjectControllerHelper.GetString("PSCommissioner", drOne);
                psSignDate = drOne.IsNull("PSSignDate") ? string.Empty : ObjectControllerHelper.GetDateTime("PSSignDate", drOne).ToShortDateString();
                administrator = ObjectControllerHelper.GetString("Administrator", drOne);
                addrLine1 = ObjectControllerHelper.GetString("PrimaryContactAddressLine1", drOne);
                addrLine2 = ObjectControllerHelper.GetString("PrimaryContactAddressLine2", drOne);
                addrCityStateZip = ObjectControllerHelper.GetString("PrimaryContactCityStateZip", drOne);

                string documentName = "ICF/IID Contract";
                string templateName = GetTemplateName(documentName);
                if (string.IsNullOrEmpty(templateName))
                {
                    log.CreateLogEntry(string.Format("{0} Document Type \"{1}\" not found", logHeader, documentName), Logging.LogPriority.Error);
                    return false;
                }

                if (templatePath.LastIndexOf("\\") != templatePath.Length - 1) templatePath += "\\";

                string[] finalDoc = MergeICFIIDTemplateWithRegData(regID, submitDate, day, month, year, providerSignDate, psSignDate, providerNumber,
                                   providerName, psCommissioner, contractStartDate, contractEndDate, administrator, addrLine1, addrLine2, addrCityStateZip, templatePath, templateName);

                if (finalDoc.Length == 0)
                {
                    log.CreateLogEntry(string.Format("{0} Creation of PDF failed", logHeader), Logging.LogPriority.Error);
                    return false;
                }

                string storagePath = AppSettings.Get("ICFIIDContract_Path", string.Empty);
                string fileName = SaveAsHtml(regID, templateName, storagePath, finalDoc);
                if (string.IsNullOrEmpty(fileName))
                {
                    return false;
                }
                bool rtn = SaveHeaderImageToTempLocation(templatePath, storagePath, "StateSeal.png");
                string headerUrl = string.Empty;
                pdfFileName = ConvertHTMLToPDF(regID, storagePath, fileName, headerUrl, string.Empty);
                if (string.IsNullOrEmpty(pdfFileName))
                {
                    return false;
                }

                //insert DOCUMENT record and REG_DOCUMENT_XREF
                if (contractID > 0)
                {
                    string signedBy = !string.IsNullOrEmpty(psSignDate) ? Constants.UserRoleType.ProviderServicesCommissioner : Constants.UserRoleType.ProviderAdministrator;
                    string name = documentName;
                    string description = string.Format("Auto generated ICF/IDD contract on signature/approval of: {0}", signedBy);
                    if (InsertDocumentRecord(regID, pdfFileName, signedBy, name, description) == false)
                    {
                        log.CreateLogEntry(string.Format("{0} Insert of Document record failed", logHeader), Logging.LogPriority.Error);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.Message), Logging.LogPriority.Error);
            }
            return false;
        }
        public bool GenerateApplication(int regID, string userName, bool saveDocument, out string pdfFileName, int versionID = 0)
        {
            pdfFileName = string.Empty;
            this.userName = userName;
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string logHeader = string.Format("Error generating Application PDF. RegID- {0}:", regID.ToString());
            //PDF E2E
            //string templatePath = "C:\\inetpub\\wwwroot\\OH_PNM_SVC_DEV\\Documents\\";
            string templatePath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
            log.CreateLogEntry("Getting templatePath" + templatePath, Logging.LogPriority.Information);
#if DEBUG
            templatePath = @"C:\Users\CA_OHPNM_DEV_7\source\repos\GIT\DEV_SRC_P3\PDMS\ProviderDataManagementSystemService\Documents\";
#endif
            if (string.IsNullOrEmpty(templatePath))
            {
                log.CreateLogEntry(string.Format("{0} Creation of PDF failed", logHeader), Logging.LogPriority.Error);
                return false;
            }

            try
            {
                string documentName = "Registration Application";
                string templateName = GetTemplateName(documentName);
                if (string.IsNullOrEmpty(templateName))
                {
                    log.CreateLogEntry(string.Format("{0} Document Type \"{1}\" not found", logHeader, documentName), Logging.LogPriority.Error);
                    return false;
                }

                if (templatePath.LastIndexOf("\\") != templatePath.Length - 1) templatePath += "\\";
                log.CreateLogEntry("Getting MergeApplicationTemplateWithRegData with Regid:" + regID + "with templatePath" + templatePath + "with templateName:" + templateName, Logging.LogPriority.Information);

                string[] finalDoc = MergeApplicationTemplateWithRegData(regID, templatePath, templateName, saveDocument, versionID);

                if (finalDoc.Length == 0)
                {
                    log.CreateLogEntry(string.Format("{0} Creation of PDF failed", logHeader), Logging.LogPriority.Error);
                    return false;
                }
                string storagePath = string.Empty;
                //PDF E2E
                if (saveDocument)
                    // called from wfcompletewf step save file on Job Server
                    storagePath = AppSettings.Get("RegistrationApplication_Path_PDF", string.Empty);
                else
                    // called from Generate PDF button save file on App Server
                    storagePath = AppSettings.Get("RegistrationApplication_Path", string.Empty);
#if DEBUG
                storagePath = @"C:\Temp\OHPNM\";
#endif
                //string storagePath = "C:\\";
                string fileName = SaveAsHtml(regID, templateName, storagePath, finalDoc);
                if (string.IsNullOrEmpty(fileName))
                {
                    return false;
                }
                //PDF E2E
                //bool rtn = SaveHeaderImageToTempLocation(templatePath, storagePath, "Ohio_Header.jpg");
                bool rtn = SaveHeaderImageToTempLocation(templatePath, storagePath, "Ohio_Header.jpg");
                string headerUrl = string.Empty;
                log.CreateLogEntry("Calling ConvertHTMLToPDFmethod for  regid " + regID, Logging.LogPriority.Information);
                pdfFileName = ConvertHTMLToPDF(regID, storagePath, fileName, headerUrl, string.Empty);
                log.CreateLogEntry("Out of ConvertHTMLToPDFmethod for  regid " + regID, Logging.LogPriority.Information);
                if (string.IsNullOrEmpty(pdfFileName))
                {
                    return false;
                }

                // Insert DOCUMENT record and REG_DOCUMENT_XREF
                if (saveDocument)
                {
                    string name = documentName;
                    string description = string.Format("Filled registration application for Reg ID: {0}", regID);
                    int docID = RegistrationController.InsertRegDocument(regID, Constants.RegistrationPageType.Contracts, Constants.ApplicationPDF,
                        name, description, pdfFileName, DateTime.Now, userName, null, null, null);

                    int onbaseDocID = 0;
                    if (docID == -1)
                    {
                        log.CreateLogEntry(string.Format("{0} Insert of Document record failed", logHeader), Logging.LogPriority.Error);
                        return false;
                    }
                    else
                    {
                        try
                        {
                            // Send document to Onbase
                            string pdfDirectory = string.Concat(storagePath, "Temporary_Files\\");
                            byte[] fileBytes = File.ReadAllBytes(pdfDirectory + pdfFileName);

                            OnBaseInterface onBaseInterface = new OnBaseInterface();
                            onbaseDocID = onBaseInterface.GetSubmitedFileID(regID, docID, fileBytes, pdfFileName, true);

                            if (onbaseDocID > 0)
                            {
                                // Delete the pdf and html files written to the local path 
                                File.Delete(Path.Combine(pdfDirectory, pdfFileName));

                                string htmlDirectory = string.Concat(storagePath, "Temporary_HtmlFiles\\");
                                File.Delete(Path.Combine(htmlDirectory, fileName));
                            }
                        }
                        catch (Exception ex)
                        {
                            log.CreateLogEntry(string.Format("{0} - Onbase upload of document failed - {1} ", logHeader, ex.Message), Logging.LogPriority.Error);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.Message), Logging.LogPriority.Error);
            }
            return false;
        }

        #endregion

        #region Private Methods
        private string GetTemplateName(string documentName)
        {
            string templateName = string.Empty;

            DataSet ds = LookupTableController.SelectDocumentTypeByName(documentName);
            if (ObjectControllerHelper.HasRows(ds))
            {
                templateName = ObjectControllerHelper.GetString("DOCUMENT_FILE_NAME", ds.Tables[0].Rows[0]);
            }

            return templateName;
        }

        private List<string> GetServices(DataTable dt)
        {
            List<string> services = new List<string>();
            string currRegion = string.Empty;
            string thisRegion = string.Empty;
            services.Add("<ul>");
            foreach (DataRow dr in dt.Rows)
            {
                thisRegion = ObjectControllerHelper.GetString("Region", dr);
                if (currRegion != thisRegion)
                {
                    if (currRegion != string.Empty)
                    {
                        services.Add("</ul>");
                    }
                    services.Add(string.Format("<li><b>{0}</b></li>", thisRegion));
                    services.Add("<ul>");
                    currRegion = thisRegion;
                }
                services.Add(string.Format("<li>{0} - {1}</li>", ObjectControllerHelper.GetString("ServiceName", dr), ObjectControllerHelper.GetString("ServiceLocation", dr)));
            }
            services.Add("</ul>");  //for service
            services.Add("</ul>"); //for region

            return services;
        }

        public string ConvertHTMLToPDF(string storagePath, string fileName, bool IsRetrievReports = false)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            try
            {
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Url = string.Concat(storagePath, fileName);
                pdfDoc.HeaderUrl = string.Empty;
                pdfDoc.FooterUrl = string.Empty;

                PdfOutput pdfOut = new PdfOutput();
                string pdfFilename = fileName.Replace(".html", ".pdf");
                pdfOut.OutputFilePath = string.Concat(storagePath, pdfFilename);

                PdfConvert.ConvertHtmlToPdf(pdfDoc, pdfOut, IsRetrievReports);
                return pdfFilename;
            }
            catch (PdfConvertTimeoutException ex1)
            {
                log.CreateLogEntry("PdfConvertTimeoutException printing job-" + ex1.Message, Logging.LogPriority.Error);
            }
            catch (PdfConvertException ex2)
            {
                log.CreateLogEntry("PdfConvertException printing job-" + ex2.Message, Logging.LogPriority.Error);
            }
            catch (Exception ex3)
            {
                log.CreateLogEntry(string.Format("{0} {1}", "Error converting HTML to PDF", ex3.Message), Logging.LogPriority.Error);
            }
            return string.Empty;
        }

        public string ConvertHTMLToPDFRemittance(string storagePath, string fileName)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            try
            {
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Url = string.Concat(storagePath, fileName);
                pdfDoc.HeaderUrl = string.Empty;
                pdfDoc.FooterUrl = string.Empty;

                PdfOutput pdfOut = new PdfOutput();
                string pdfFilename = fileName.Replace(".html", ".pdf");
                pdfOut.OutputFilePath = string.Concat(storagePath, pdfFilename);

                PdfConvert.ConvertHtmlToPdfRemittance(pdfDoc, pdfOut);
                return pdfFilename;
            }
            catch (PdfConvertTimeoutException ex1)
            {
                log.CreateLogEntry("PdfConvertTimeoutException printing job-" + ex1.Message, Logging.LogPriority.Error);
            }
            catch (PdfConvertException ex2)
            {
                log.CreateLogEntry("PdfConvertException printing job-" + ex2.Message, Logging.LogPriority.Error);
            }
            catch (Exception ex3)
            {
                log.CreateLogEntry(string.Format("{0} {1}", "Error converting HTML to PDF", ex3.Message), Logging.LogPriority.Error);
            }
            return string.Empty;
        }

        public string ConvertHTMLToPDF(string storagePath, string fileName, string destinationPath)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            try
            {



                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Url = string.Concat(storagePath, fileName);
                PdfOutput pdfOut = new PdfOutput();
                string pdfFilename = fileName.Replace(".html", ".pdf");
                pdfOut.OutputFilePath = string.Concat(destinationPath, pdfFilename);
                PdfConvert.ConvertHtmlToPdf(pdfDoc, pdfOut);
                return pdfFilename;
            }
            catch (PdfConvertTimeoutException ex1)
            {
                log.CreateLogEntry("PdfConvertTimeoutException printing job-" + ex1.Message, Logging.LogPriority.Error);
                throw;
            }
            catch (PdfConvertException ex2)
            {
                log.CreateLogEntry("PdfConvertException printing job-" + ex2.Message, Logging.LogPriority.Error);
                throw;
            }
            catch (Exception ex3)
            {
                log.CreateLogEntry(string.Format("{0} {1}", "Error converting HTML to PDF", ex3.Message), Logging.LogPriority.Error);

                throw;
            }
            return string.Empty;
        }
        private string ConvertHTMLToPDF(int regID, string storagePath, string fileName, string headerUrl, string footerUrl)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string htmlDirectory = string.Concat(storagePath + "Temporary_HtmlFiles\\");
            string pdfDirectory = string.Concat(storagePath, "Temporary_Files\\");
            log.CreateLogEntry("Inside ConvertHTMLToPDFmethod and get Temporary_HtmlFiles also Temporary_Files path  for  regid " + regID, Logging.LogPriority.Information);

            try
            {
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Url = string.Concat(htmlDirectory, fileName);
                log.CreateLogEntry("URL of Pdf  for  regid " + regID, Logging.LogPriority.Information);
                pdfDoc.HeaderUrl = headerUrl;
                pdfDoc.FooterUrl = footerUrl;

                if (!Directory.Exists(pdfDirectory))
                {
                    Directory.CreateDirectory(pdfDirectory);
                }

                PdfOutput pdfOut = new PdfOutput();
                string pdfFilename = fileName.Replace(".html", ".pdf");
                pdfOut.OutputFilePath = string.Concat(pdfDirectory, pdfFilename);
                log.CreateLogEntry("Calling for ConvertHtmlToPdf regid " + regID, Logging.LogPriority.Information);
                PdfConvert.ConvertHtmlToPdf(pdfDoc, pdfOut);
                log.CreateLogEntry("Out  for ConvertHtmlToPdf regid with File name " + regID + pdfFilename, Logging.LogPriority.Information);

                return pdfFilename;
            }
            catch (PdfConvertTimeoutException ex1)
            {
                log.CreateLogEntry("PdfConvertTimeoutException RegID-" + regID.ToString() + ": " + ex1.Message, Logging.LogPriority.Error);
            }
            catch (PdfConvertException ex2)
            {
                log.CreateLogEntry("PdfConvertException RegID-" + regID.ToString() + ": " + ex2.Message, Logging.LogPriority.Error);
            }
            catch (Exception ex3)
            {
                log.CreateLogEntry(string.Format("{0} {1}", "Error converting HTML to PDF", ex3.Message), Logging.LogPriority.Error);
            }
            return string.Empty;
        }
        public string FormatPhone(string phone)
        {
            string outPhone = "";
            if (phone.Length == 10)
            {
                outPhone = "(" + phone.Substring(0, 3) + ") " + phone.Substring(3, 3) + "-" + phone.Substring(6, 4);
            }
            else
            {
                outPhone = phone;
            }
            return outPhone;
        }
        public static DataRow GetProviderInfo(int regID)
        {
            DataRow rtn = null;

            DataSet ds = RegistrationController.SelectRegistrationData(regID, "PROVIDER");
            if (ObjectControllerHelper.HasRows(ds)) rtn = ds.Tables[0].Rows[0];
            return rtn;
        }
        private string[] MergeApplicationTemplateWithRegData(int regID, string templatePath, string templateName, bool saveDocument, int versionID = 0)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry("Inside MergeApplicationTemplateWithRegData method with Regid:" + regID + "with templatePath" + templatePath + "with templateName:" + templateName, Logging.LogPriority.Information);

            string[] finalDoc = new string[0];
            try
            {
                List<StringBuilder> sbDocPages = new List<StringBuilder>();
                sbDocPages.Add(new StringBuilder());

                int lineNbr = 0, pageNbr = 0;

                string templateNameWithPath = templatePath + templateName;
                log.CreateLogEntry("TemplateNameWith path:" + templateNameWithPath, Logging.LogPriority.Information);

                //string templateNameWithPath = @"C:\inetpub\wwwroot\OH_PNM_SVC_DEV\Documents\" + templateName;

                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet dsQuestion = new DataSet();
                DataSet agreementSubm = new DataSet();
                DataSet dsProvider = new DataSet();
                DataTable dtQuestion;
                int wfEventTypeID = 0;
                bool isReapplication = false;
                bool isReactivation = false;
                string IsDirectOrReliaCard = string.Empty;
                string achAttest = string.Empty;
                string submitDateTime = string.Empty;

                if (versionID == 0)
                {
                    ds = RegistrationController.SelectRegistrationData(regID, "Application");
                    log.CreateLogEntry("Called SelectRegistrationData  method with regid and Application table:" + regID, Logging.LogPriority.Information);

                    if (!ObjectControllerHelper.HasRows(ds))
                    {
                        log.CreateLogEntry(string.Format("{0} No application found with this registration id", logMsg), Logging.LogPriority.Error);
                        return finalDoc;
                    }

                    ds1 = RegistrationController.SelectRegistration(regID);
                    log.CreateLogEntry("Called SelectRegistrationData  method with regid " + regID, Logging.LogPriority.Information);
                    wfEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", ds1.Tables[0].Rows[0]);
                    isReapplication = ObjectControllerHelper.GetBool("ISReapplication", ds1.Tables[0].Rows[0]);
                    isReactivation = ObjectControllerHelper.GetBool("ISReactivation", ds1.Tables[0].Rows[0]);
                    IsDirectOrReliaCard = ObjectControllerHelper.GetString("MAX_FIN_DEBIT_IND", ds1.Tables[0].Rows[0]);
                    achAttest = ObjectControllerHelper.GetString("ACH_ATTEST", ds1.Tables[0].Rows[0]);
                    submitDateTime = ObjectControllerHelper.GetString("SUBMIT_DATE_TIME", ds1.Tables[0].Rows[0]);

                    dsQuestion = RegistrationController.SelectRegistrationData(regID, "QUESTION");
                    dtQuestion = ObjectControllerHelper.HasRows(dsQuestion.Tables[0]) ? dsQuestion.Tables[0] : null;

                    dsProvider = RegistrationController.SelectRegistrationData(regID, "PROVIDER");
                    DataRow dataRowProvider = dsProvider.Tables[0].Rows[0];
                    wfEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", dsProvider.Tables[0].Rows[0]);


                }
                else
                {
                    //get data from version tables
                    ds = RegistrationController.SelectRegistrationApplicationVersion(regID, versionID);
                    log.CreateLogEntry("Called SelectRegistrationData  method with regid and Application Version table:" + regID, Logging.LogPriority.Information);

                    if (!ObjectControllerHelper.HasRows(ds))
                    {
                        log.CreateLogEntry(string.Format("{0} No application found with this registration id", logMsg), Logging.LogPriority.Error);
                        return finalDoc;
                    }

                    dtQuestion = ds.Tables[77];
                    wfEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", ds.Tables[84].Rows[0]);
                    isReapplication = ObjectControllerHelper.GetBool("IS_REAPPLICATION", ds.Tables[84].Rows[0]);
                    isReactivation = ObjectControllerHelper.GetBool("IS_REACTIVATION", ds.Tables[84].Rows[0]);
                    IsDirectOrReliaCard = ObjectControllerHelper.GetString("MAX_FIN_DEBIT_IND", ds.Tables[0].Rows[0]);
                    achAttest = ObjectControllerHelper.GetString("ACH_ATTEST", ds.Tables[0].Rows[0]);
                    submitDateTime = ObjectControllerHelper.GetString("SUBMIT_DATE_TIME", ds.Tables[0].Rows[0]);
                }
                //      DataSet agreementSubm = RegistrationController.SelectRegDocuments(regID, MAXIMUS.Core.Libraries.Constants.RegistrationPageType.Contracts,
                //MAXIMUS.Core.Libraries.Constants.ApplicationPDF, string.Empty, null);
                agreementSubm = RegistrationController.SelectSubmittedAgreementPDFs(regID, MAXIMUS.Core.Libraries.Constants.RegistrationPageType.Contracts,
            MAXIMUS.Core.Libraries.Constants.ApplicationPDF, 100, 0, 0);
                log.CreateLogEntry("Called SelectRegDocuments  method with regid " + regID, Logging.LogPriority.Information);

                DataRow dataRow = ds.Tables[0].Rows[0];



                List<string> services = GetRegistrationServices(ds.Tables[1]);
                string formattedServices = this.FormatList(services);

                string affiliations = this.GetAffiliations(ds);


                List<string> affiliationsGrid = this.GetAffiliationsGrid(ds.Tables[2]);
                string formattedaffiliations = this.FormatList(affiliationsGrid);


                string householdMembers = this.GetHouseholdMembers(ds);



                List<string> specialties = this.GetSpecialties(ds.Tables[4]);
                string formattedSpecialties = this.FormatList(specialties);

                List<string> addlSpecialties = this.GetSpecialties(ds.Tables[5]);
                string formattedAddlSpecialties = this.FormatList(addlSpecialties);

                List<string> taxonomies = this.GetTaxonomies(ds.Tables[13]);
                string formattedTaxonomies = this.FormatList(taxonomies);

                List<string> addlTaxonomies = this.GetTaxonomies(ds.Tables[14]);
                string formattedAddlTaxonomies = this.FormatList(addlTaxonomies);

                List<string> deaNumbers = this.GetDEANumbers(ds.Tables[15]);
                string formattedDEANumbers = this.FormatList(deaNumbers);

                List<string> cliaNumbers = this.GetCLIANumbers(ds.Tables[16]);
                string formattedCLIANumbers = this.FormatList(cliaNumbers);

                DataTable dtInsurances = ds.Tables[17];
                List<string> insurances = this.GetInsurance(dtInsurances);
                string formattedInsurances = this.FormatList(insurances);

                List<string> dentalLicenses = this.GetDentalLicenses(ds.Tables[18]);
                string formattedDentalLicenses = this.FormatList(dentalLicenses);

                List<string> visionCerts = this.GetVisionCertifications(ds.Tables[19]);
                string formattedVisionCerts = this.FormatList(visionCerts);

                List<string> pharmacyDetails = this.GetPharmacyProviderInfo(ds.Tables[20]);
                string formattedPharmacyDetails = this.FormatList(pharmacyDetails);
                DataTable dtPhrmaDetails = ds.Tables[20];

                List<string> pharmacists = this.GetPharmacistInfo(ds.Tables[21]);
                string formattedPharmacists = this.FormatList(pharmacists);

                List<string> stateCDSNumbers = this.GetStateCDSNumber(ds.Tables[22]);
                string formattedStateCDSNumbers = this.FormatList(stateCDSNumbers);

                List<string> cprCerts = this.GetCPRCertifications(ds.Tables[23]);
                string formattedCPRCerts = this.FormatList(cprCerts);

                List<string> nursingCerts = this.GetNursingCertifications(ds.Tables[24]);
                string formattedNursingCerts = this.FormatList(nursingCerts);

                List<string> categoryOfSvcList = this.GetCategoriesOfService(ds.Tables[25]);
                string formattedCategoryOfSvcList = this.FormatList(categoryOfSvcList);

                List<string> providerIdentifiers = this.GetProviderIdentifiers(ds.Tables[31]);
                string formattedProviderIdentifiers = this.FormatList(providerIdentifiers);
                string formattedRealOwners = "";

                string formattedOwners = "";
                if (ds.Tables[6].Rows.Count > 0)
                {
                    List<string> owners = this.GetOwners(ds.Tables[6]);
                    formattedOwners = this.FormatList(owners);
                }
                else
                    formattedOwners = "No information found.";
                if (ds.Tables.Count > 75)
                {
                    List<string> realestateowners = this.GetRealestateOwners(ds.Tables[75]);
                    formattedRealOwners = this.FormatList(realestateowners);
                }
                else
                    formattedRealOwners = "No information found.";
                string formattedadditonaldiscluser = "";
                if (ds.Tables.Count > 76)
                {
                    List<string> additonaldiscluser = this.GetAdditionalClouser(ds.Tables[76]);
                    formattedadditonaldiscluser = this.FormatList(additonaldiscluser);
                }
                else
                    formattedadditonaldiscluser = "No information found.";
                List<string> addlAddresses = this.GetAdditionalAddresses(ds.Tables[7]);
                string formattedAddlAddresses = this.FormatList(addlAddresses);

                List<string> behavioralHealthInfo = this.GetBehavioralHealthInfo(ds.Tables[51]);
                string formattedbehavioralHealth = this.FormatList(behavioralHealthInfo);

                List<string> requiredDocuments = this.GetRequiredDocuments();
                string formattedRequriedDocuments = this.FormatList(requiredDocuments);
                DataTable dtBHQuestions = ds.Tables[52];
                DataTable dtNVQuestions = ds.Tables[56];
                DataTable dtNWQuestions = ds.Tables[57];
                List<string> PracticePartnershipInfo = GetPracticePartnershipDetails(regID);
                List<string> MedicaidDetailsInfo = GetMedicaidDetails(ds.Tables[65]);
                DataTable GENDER = ds.Tables[59];
                DataTable Competency = ds.Tables[60];
                DataTable spoken = ds.Tables[61];
                DataTable training = ds.Tables[62];

                DataTable medicare = ds.Tables[63];
                string formattedPracticePartnershipInfo = this.FormatList(PracticePartnershipInfo);
                string formattedMedicaidDetailsInfo = this.FormatList(MedicaidDetailsInfo);
                DataTable dtRegPageSettings = ds.Tables[32];
                DataTable dtApplicationFee = new DataTable();
                DataTable dtApplicationFeeAmount = new DataTable();
                dtApplicationFee = ds.Tables[12];
                dtApplicationFeeAmount = ds.Tables.Count > 69 ? ds.Tables[69] : new DataTable();
                DataTable dtApplicationPaymentType = ds.Tables.Count > 70 ? ds.Tables[70] : new DataTable();
                DataTable dtChopSelect = ds.Tables.Count > 71 ? ds.Tables[71] : new DataTable();
                DataTable dtChopType = ds.Tables.Count > 72 ? ds.Tables[72] : new DataTable();
                DataTable dtOwnerQuestion = ds.Tables.Count > 77 ? ds.Tables[77] : new DataTable();
                List<string> applicationFee = this.GetApplicationFee(dtApplicationFee);
                string formattedApplicationFee = this.FormatList(applicationFee);

                int entitytypeID = 0;
                int providertypeID = 0;
                int diddreferralID = 0;
                int taxIDtypeID = 0;
                int applicationTypeID = 0;
                entitytypeID = ObjectControllerHelper.GetInt("ENTITY_TYPE_ID", dataRow);
                providertypeID = ObjectControllerHelper.GetInt("PROVIDER_TYPE_ID", dataRow);
                diddreferralID = ObjectControllerHelper.GetInt("DIDD_REFERRAL_ID", dataRow);
                taxIDtypeID = ObjectControllerHelper.GetInt("TAX_ID_TYPE_ID", dataRow);
                applicationTypeID = ObjectControllerHelper.GetInt("APPLICATION_TYPE_ID", dataRow);
                //bool isW9visible = SectionIsVisible(entitytypeID, providertypeID, diddreferralID, "Substitute W9 Form", taxIDtypeID);
                bool isW9visible = SectionIsVisible("W9Form", dtRegPageSettings);
                //bool isW4visible = SectionIsVisible(entitytypeID, providertypeID, diddreferralID, "Substitute W4 Form", taxIDtypeID);
                bool isAgreementsVisible = SectionIsVisible(applicationTypeID, entitytypeID, providertypeID, diddreferralID, "Agreements", "Agreements");
                bool isMedicaidProviderAgreementVisible = SectionIsVisible(applicationTypeID, entitytypeID, providertypeID, diddreferralID, "Agreements", "AuthorizationReleaseInformation");
                bool isOwnershipAckVisible = SectionIsVisible(applicationTypeID, entitytypeID, providertypeID, diddreferralID, "Agreements", "OwnershipAck");
                bool isFelonyOrMisdemeanorVisible = SectionIsVisible(applicationTypeID, entitytypeID, providertypeID, diddreferralID, "Agreements", "FelonyOrMisdemeanor");
                bool isAuthorizationReleaseInformationVisible = SectionIsVisible(applicationTypeID, entitytypeID, providertypeID, diddreferralID, "Agreements", "AuthorizationReleaseInformation");
                //bool isQuestionnaireVisible = SectionIsVisible(applicationTypeID, entitytypeID, providertypeID, diddreferralID, "Agreements", "AdditionalQuestions");
                //bool isQuestionnaireDMEVisible = SectionIsVisible(applicationTypeID, entitytypeID, providertypeID, diddreferralID, "Agreements", "AdditionalQuestionsDME");
                bool isAffiliationVisible = SectionIsVisible("Affiliations", dtRegPageSettings);
                bool isEducationVisible = SectionIsVisible("Education", dtRegPageSettings);
                bool isWorkflowStepsVisible = SectionIsVisible("WorkflowSteps", dtRegPageSettings);
                bool isSpecialtiesVisible = SectionIsVisible("Specialties", dtRegPageSettings);
                bool isSpecialtiesTaxonomyVisible = SectionIsVisible("SpecialtiesTaxonomies", dtRegPageSettings);
                bool isLicenseVisible = SectionIsVisible("Licenses", dtRegPageSettings);
                bool isFederalDEAVisible = SectionIsVisible("FederalDEA", dtRegPageSettings);
                bool isCLIAVisible = SectionIsVisible("CertificationsCLIA", dtRegPageSettings);
                bool isBHInfoVisible = SectionIsVisible("BehaviouralHealthInformation", dtRegPageSettings);
                bool isInsuranceVisible = SectionIsVisible("Insurance", dtRegPageSettings);
                bool isDentalLicenseisible = SectionIsVisible("DentalLicense", dtRegPageSettings);
                bool isVisionVisible = SectionIsVisible("VisionProviders", dtRegPageSettings);
                bool isPharmacyVisible = SectionIsVisible("PharmacyProviders", dtRegPageSettings);
                bool isStateCDSNumberVisible = SectionIsVisible("StateCDSNumber", dtRegPageSettings);
                bool isCPRVisible = SectionIsVisible("CPRCertification", dtRegPageSettings);
                bool isNursingCertVisible = SectionIsVisible("NursingProfessionalCertification", dtRegPageSettings);
                bool isCategoryVisible = SectionIsVisible("CategoryOfService", dtRegPageSettings);
                bool isOwnerVisible = SectionIsVisible("OwnerInformation", dtRegPageSettings);//TODO
                bool isOwnerScreenVisible = SectionIsVisible("OwnerScreening", dtRegPageSettings);//TODO
                bool isWavierScreenVisible = SectionIsVisible("WaiverServiceDisplay", dtRegPageSettings);//TODO
                bool ishouseHoldVisible = SectionIsVisible("HouseholdMemberScreening", dtRegPageSettings);//TODO
                bool isApplicationVisible = SectionIsVisible("ApplicationFee", dtRegPageSettings);
                bool isOtherAddressVisible = SectionIsVisible("OtherAddress", dtRegPageSettings);
                bool isRemittanceAddressVisible = SectionIsVisible("RemittanceAddress", dtRegPageSettings);
                bool isCorrespondenceVisible = SectionIsVisible("CorrespondenceInformation", dtRegPageSettings);
                bool isPrimaryServiceVisible = SectionIsVisible("PrimaryServiceAddress", dtRegPageSettings);
                bool isBillingVisible = SectionIsVisible("BillingPaymentContactInformation", dtRegPageSettings);
                bool isOfficeVisible = isPrimaryServiceVisible; // changed because office information is now on the primary service address screen
                bool isBoardCertificateVisible = SectionIsVisible("BoardCertification", dtRegPageSettings);
                bool isSatelliteLocationsVisible = SectionIsVisible("SatelliteLocations", dtRegPageSettings);
                //TODO
                bool isGroupAndFacilityAffiliationsVisible = SectionIsVisible("GroupAndFacilityAffiliations", dtRegPageSettings);
                bool isMCOAffiliationVisible = SectionIsVisible("MCOAffiliation", dtRegPageSettings);
                bool isLongTermCareAddressVisible = SectionIsVisible("NursingFacilityAddress", dtRegPageSettings);
                bool isAddress1099FormVisible = SectionIsVisible("Address1099Form", dtRegPageSettings);

                bool isCredentialingContactVisible = SectionIsVisible("CredentialingContact", dtRegPageSettings);
                bool isContractMaintenanceVisible = SectionIsVisible("Contract Maintenance", dtRegPageSettings);
                bool isMalpracticeClaimsHistoryVisible = SectionIsVisible("MalpracticeClaimsHistory", dtRegPageSettings);
                bool isWaiverServicesVisible = SectionIsVisible("WaiverServices", dtRegPageSettings);
                bool isOtherDocumentsVisible = SectionIsVisible("OtherDocuments", dtRegPageSettings); //RequiredDocument

                //Adding new got to check
                bool isEmploymentHistoryVisible = SectionIsVisible("EmploymentHistory", dtRegPageSettings);
                bool isNursingFacilityVentilatorVisible = SectionIsVisible("NursingFacilityVentilator", dtRegPageSettings);
                bool isACHAuthorizationVisible = SectionIsVisible("ACHAuthorization", dtRegPageSettings);
                bool isChangeOperatorInformationVisible = SectionIsVisible("ChangeOperatorInformation", dtRegPageSettings);
                bool isAgreementQuestionsVisible = SectionIsVisible("AgreementQuestions", dtRegPageSettings);
                bool isAppealsVisible = SectionIsVisible("Appeals", dtRegPageSettings);
                bool isHearingRightsVisible = SectionIsVisible("HearingRights", dtRegPageSettings);
                bool isIncidentComplianceReviewVisible = SectionIsVisible("IncidentComplianceReview", dtRegPageSettings);
                bool is45DaysNoticeVisible = SectionIsVisible("45DaysNotice", dtRegPageSettings);
                bool isClosureNoticeVisible = SectionIsVisible("ClosureNotice", dtRegPageSettings);
                bool isBuildingHistoryVisible = SectionIsVisible("BuildingHistory", dtRegPageSettings);
                bool isProviderScreeningVisible = SectionIsVisible("ProviderScreening", dtRegPageSettings);
                bool isIndividualMemberScreeningVisible = SectionIsVisible("IndividualMemberScreening", dtRegPageSettings);
                bool isSiteVisitScreeningVisible = SectionIsVisible("SiteVisitScreening", dtRegPageSettings);
                bool isPrimaryContactInfoVisible = SectionIsVisible("PrimaryContactInfo", dtRegPageSettings);
                bool isHomeOfficeAddressVisible = SectionIsVisible("HomeOfficeAddress", dtRegPageSettings);
                bool isHospitalAddressVisible = SectionIsVisible("HospitalAddress", dtRegPageSettings);
                bool isCPCContactInformationVisible = SectionIsVisible("CPCContactInformation", dtRegPageSettings);
                bool isPracticePartnershipVisible = SectionIsVisible("PracticePartnership", dtRegPageSettings);
                bool isCPCSpecialtiesVisible = SectionIsVisible("CPCSpecialties", dtRegPageSettings);
                bool isMiscellaneousVisible = SectionIsVisible("Miscellaneous", dtRegPageSettings);

                DataSet dset_OwnerXREF = new DataSet(); ;
                dset_OwnerXREF = RegistrationController.SelectRegistrationData(regID, "OWNER_XREF_History");
                DataSet dset_OwnerOTHER = new DataSet(); ;
                dset_OwnerOTHER = RegistrationController.SelectRegistrationData(regID, "OWNER_OTHER");
                DataSet dset_OwnerCONVICTION = new DataSet(); ;
                dset_OwnerCONVICTION = RegistrationController.SelectRegistrationData(regID, "OWNER_CONVICTION");

                List<string> ownerSubcontractors = this.GetOwnerSubcontractors(ds.Tables[28]);
                string formattedOwnerSubcontractors = this.FormatList(ownerSubcontractors);

                List<string> ownerTransactions = this.GetOwnerTransactions(ds.Tables[29]);
                string formattedOwnerTransactions = this.FormatList(ownerTransactions);

                List<string> providerConvictions = this.GetProviderConvictions(ds.Tables[30]);
                string formattedProviderConvictions = this.FormatList(providerConvictions);

                List<string> malpracticeClaims = this.GetMalpracticeClaims(ds.Tables[26]);
                string formattedmalpracticeClaims = this.FormatList(malpracticeClaims);

                //DME
                bool isDMEvisible = SectionIsVisible("DME", dtRegPageSettings);
                // DME Personnel Info
                DataTable dtDMEPersonnelInfo = ds.Tables[33];

                // DME Professionals List
                List<string> dmeProfessionals = this.GetDMEProfessionals(ds.Tables[34]);
                string formattedDMEProfessionals = this.FormatList(dmeProfessionals);

                // DME Products List
                List<string> dmeProducts = this.GetDMEProducts(ds.Tables[35]);
                string formattedDMEProducts = this.FormatList(dmeProducts);

                // DME Accreditations List
                List<string> dmeAccreditations = this.GetDMEAccreditations(ds.Tables[36]);
                string formattedDMEAccreditations = this.FormatList(dmeAccreditations);

                DataTable dtAgreementInits = ds.Tables[27];

                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                DataTable address = new DataTable();
                DataTable longTermAddress = new DataTable();
                DataTable billingAddress = new DataTable();
                DataTable correspondenceAddress = new DataTable();
                DataTable otherAddress = new DataTable();
                DataTable w9Address = new DataTable();
                DataTable physicalAddress = new DataTable();
                DataTable remittanceAddress = new DataTable();
                DataTable officeAddress = new DataTable();
                DataTable homeAddress = new DataTable();
                DataTable hospitalAddress = new DataTable();
                DataTable oneAddress = new DataTable();
                DataTable OneForm = new DataTable();
                DataTable MCQ = new DataTable();
                DataTable MCQAffiliation = new DataTable();
                DataTable boardCert = new DataTable();
                DataTable officeTiming = new DataTable();
                DataTable languages = new DataTable();
                DataTable licenseAddress = new DataTable();
                DataTable accomodations = new DataTable();
                DataTable workHistory = new DataTable();
                DataTable workGapsHistory = new DataTable();
                DataTable siteVisitScreening = new DataTable();
                DataTable PracticePartnership = new DataTable();
                DataTable Endrosment = new DataTable();
                DataTable W9TaxInfo = new DataTable();
                DataTable W9TaxInfoSelect = new DataTable();
                DataTable officeTimingGrp = new DataTable();

                if (ds.Tables.Count > 37)
                {
                    var longTermAddressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.NursingFacility);
                    if (longTermAddressTbl.Any())
                        longTermAddress = longTermAddressTbl.AsEnumerable().CopyToDataTable();
                    var addressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.PrimaryContact);
                    if (addressTbl.Any())
                        address = addressTbl.AsEnumerable().CopyToDataTable();
                    var billingAddressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.Billing);
                    if (billingAddressTbl.Any())
                        billingAddress = billingAddressTbl.AsEnumerable().CopyToDataTable();
                    var correspondenceAddressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.Correspondence);
                    if (correspondenceAddressTbl.Any())
                        correspondenceAddress = correspondenceAddressTbl.AsEnumerable().CopyToDataTable();
                    var otherAddressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.Other);
                    if (otherAddressTbl.Any())
                        otherAddress = otherAddressTbl.AsEnumerable().CopyToDataTable();
                    var w9AddressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.FaxFormW9);
                    if (w9AddressTbl.Any())
                        w9Address = w9AddressTbl.AsEnumerable().CopyToDataTable();
                    var physicalAddressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.PrimaryPractice);
                    if (physicalAddressTbl.Any())
                        physicalAddress = physicalAddressTbl.AsEnumerable().CopyToDataTable();

                    var remittanceAddressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.Remittance);
                    if (remittanceAddressTbl.Any())
                        remittanceAddress = remittanceAddressTbl.AsEnumerable().CopyToDataTable();

                    var homeOfficeAddressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.HomeOffice);
                    if (homeOfficeAddressTbl.Any())
                        homeAddress = homeOfficeAddressTbl.AsEnumerable().CopyToDataTable();

                    var hospitalAddressTbl = ds.Tables[37].AsEnumerable()
                        .Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.Hospital
                            || r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.FranchiseFeeAddress
                            || r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.HcapAddress);
                    if (hospitalAddressTbl.Any())
                        hospitalAddress = hospitalAddressTbl.AsEnumerable().CopyToDataTable();

                    var professionalAddressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.ProfessionalLicenseAddress);
                    if (professionalAddressTbl.Any())
                        licenseAddress = professionalAddressTbl.AsEnumerable().CopyToDataTable();


                    var oneAddressTbl = ds.Tables[37].AsEnumerable().Where(r => r.Field<int>("ADDRESS_TYPE_ID") == CON.AddressType.TaxForm1099);
                    if (oneAddressTbl.Any())
                        oneAddress = oneAddressTbl.AsEnumerable().CopyToDataTable();
                    if (oneAddress.Rows.Count > 0)
                    {
                        _addressRegId = ObjectControllerHelper.GetInt("REG_ADDRESS_ID", oneAddress.Rows[0]);

                        if (_addressRegId > 0)
                            OneForm = Get1099FormDataByAddressId(regID);
                    }
                }
                if (ds.Tables.Count > 66)
                {
                    Endrosment = ds.Tables[66];
                }
                if (ds.Tables.Count > 67)
                {
                    W9TaxInfo = ds.Tables[67];
                }
                if (ds.Tables.Count > 68)
                {
                    W9TaxInfoSelect = ds.Tables[68];
                }

                List<string> licenses = this.GetLicenses(ds.Tables[3], licenseAddress != null && licenseAddress.Rows.Count > 0 ?
                    licenseAddress.Rows[0] : null, licenseAddress != null && licenseAddress.Rows.Count > 0 ?
                    licenseAddress : null, Endrosment != null && Endrosment.Rows.Count > 0 ?
                    Endrosment : null);
                string formattedLicenses = this.FormatList(licenses);
                if (ds.Tables.Count > 38)
                {
                    officeAddress = ds.Tables[38];
                }
                string educations = "";
                string submittedAgreements = "";
                string submittedUpdateAgreements = "";
                string workflows = "";
                string confirmedMCQ = "";
                string boardCertifications = "";
                string contracts = "";
                string contacts = "";
                string otherservices = "";
                string workHistoryDetails = string.Empty;
                string workGapDetails = string.Empty;
                string siteVisitDetails = string.Empty;
                string PracticePartnershipDetails = string.Empty;
                string pendingGrpAffl = "";
                string confGrpAffl = "";
                string delegateGrpAffl = "";
                string hospitalGrpAffl = "";
                string DODD_LIST = "";
                string ODA_LIST = "";
                string ODAPassport_LIST = "";
                string ODATransportation_LIST = "";
                string ODAOther_LIST = "";
                submittedAgreements = this.FormatList(GetSubmittedagreement(agreementSubm.Tables[0], saveDocument, regID, ds.Tables[84]));
                submittedUpdateAgreements = this.FormatList(GetSubmittedUpdateWFagreement(agreementSubm.Tables[1], saveDocument, regID));

                if (ds.Tables.Count > 39)
                {
                    educations = GetEducations(ds.Tables[39]);
                }
                if (ds.Tables.Count > 40)
                {
                    workflows = GeWorkFlows(ds.Tables[40]);
                }
                if (ds.Tables.Count > 42)
                {
                    MCQ = ds.Tables[42];
                }
                if (ds.Tables.Count > 43)
                {
                    MCQAffiliation = ds.Tables[43];
                    confirmedMCQ = GetMCQs(MCQAffiliation);
                }
                if (ds.Tables.Count > 44)
                {
                    boardCert = ds.Tables[44];
                    boardCertifications = GetBoardCertificates(boardCert);
                }
                if (ds.Tables.Count > 45)
                {
                    contracts = GetContracts(ds.Tables[45]);
                }
                if (ds.Tables.Count > 46)
                {
                    contacts = GetCredential(ds.Tables[46]);
                }
                if (ds.Tables.Count > 47)
                {
                    otherservices = GetOtherService(ds.Tables[47]);
                }
                if (ds.Tables.Count > 48)
                {
                    officeTiming = ds.Tables[48];
                }
                if (ds.Tables.Count > 86)
                {
                    officeTimingGrp = ds.Tables[86];
                }
                if (ds.Tables.Count > 49)
                {
                    languages = ds.Tables[49];
                }
                if (ds.Tables.Count > 50)
                {
                    accomodations = ds.Tables[50];
                }
                if (ds.Tables.Count > 53)
                {
                    workHistory = ds.Tables[53];
                    workHistoryDetails = GetWorkHistory(workHistory);
                }

                if (ds.Tables.Count > 54)
                {
                    workGapsHistory = ds.Tables[54];
                    workGapDetails = GetWorkGapDetails(workGapsHistory);
                }
                if (ds.Tables.Count > 78)
                {
                    pendingGrpAffl = this.FormatList(GetPendingAffilation(ds.Tables[78], true));
                }
                if (ds.Tables.Count > 79)
                {
                    confGrpAffl = this.FormatList(GetPendingAffilation(ds.Tables[79], false));
                }
                if (ds.Tables.Count > 80)
                {
                    hospitalGrpAffl = this.FormatList(GethospitalAff(ds.Tables[80]));
                }
                if (ds.Tables.Count > 81)
                {
                    delegateGrpAffl = this.FormatList(GetDelegateAff(ds.Tables[81]));
                }
                if (ds.Tables.Count > 83)
                {
                    DataTable DODD_LISTbl = ds.Tables[83];
                    DataTable ODA_LISTbl = ds.Tables[83];
                    DataTable ODAPassport_LISTbl = ds.Tables[83];
                    DataTable ODATransportation_LISTbl = ds.Tables[83];
                    DataTable ODAOther_LISTbl = ds.Tables[83];
                    var tablDODD_LISTbl = DODD_LISTbl.AsEnumerable().Where(row => row.Field<int?>("SERVICE_CATEGORY_ID") == null);
                    if (tablDODD_LISTbl.Any())
                        DODD_LISTbl = tablDODD_LISTbl.CopyToDataTable();
                    else
                        DODD_LISTbl = new DataTable();
                    DODD_LIST = this.FormatList(GetWavierServiceDisplay(DODD_LISTbl, "No Contracts details available."));
                    var tablODA_LISTbl = ODA_LISTbl.AsEnumerable().Where(row => row.Field<int?>("SERVICE_CATEGORY_ID") == CON.WaiverServiceCategoryType.AssistedLiving);
                    if (tablODA_LISTbl.Any())
                        ODA_LISTbl = tablODA_LISTbl.CopyToDataTable();
                    else
                        ODA_LISTbl = new DataTable();
                    ODA_LIST = this.FormatList(GetWavierServiceDisplay(ODA_LISTbl, "No Contracts details available."));

                    var tablOODAPassport_LISTbl = ODAPassport_LISTbl.AsEnumerable().Where(row => row.Field<int?>("SERVICE_CATEGORY_ID") == CON.WaiverServiceCategoryType.PASSPORT);
                    if (tablOODAPassport_LISTbl.Any())
                        ODAPassport_LISTbl = tablOODAPassport_LISTbl.CopyToDataTable();
                    else
                        ODAPassport_LISTbl = new DataTable();
                    ODAPassport_LIST = this.FormatList(GetWavierServiceDisplay(ODAPassport_LISTbl, "No Contracts details available."));

                    var tablODATransportation_LISTbl = ODATransportation_LISTbl.AsEnumerable().Where(row => row.Field<int?>("SERVICE_CATEGORY_ID") == CON.WaiverServiceCategoryType.Choices);
                    if (tablODATransportation_LISTbl.Any())
                        ODATransportation_LISTbl = tablODATransportation_LISTbl.CopyToDataTable();
                    else
                        ODATransportation_LISTbl = new DataTable();
                    ODATransportation_LIST = this.FormatList(GetWavierServiceDisplay(ODATransportation_LISTbl, "No Contracts details available."));

                    var tablODAOther_LISTbl = ODAOther_LISTbl.AsEnumerable().Where(row => row.Field<int?>("SERVICE_CATEGORY_ID") == CON.WaiverServiceCategoryType.Other);
                    if (tablODAOther_LISTbl.Any())
                        ODAOther_LISTbl = tablODAOther_LISTbl.CopyToDataTable();
                    else
                        ODAOther_LISTbl = new DataTable();
                    ODAOther_LIST = this.FormatList(GetWavierServiceDisplay(ODAOther_LISTbl, "No Contracts details available."));
                }
                if (ds.Tables.Count > 55)
                {
                    siteVisitScreening = ds.Tables[55];


                    siteVisitDetails = GetSiteVisitScreeningDetails(siteVisitScreening);
                }
                DataTable dtUserName = ds.Tables.Count > 73 ? ds.Tables[73] : new DataTable();

                DataTable dtDivUserName = ds.Tables.Count > 74 ? ds.Tables[74] : new DataTable();
                DataTable dtInitials = ds.Tables.Count > 87 ? ds.Tables[87] : new DataTable();
                DataTable dtPDFlogo = LookupTableController.GetAppSetting("RegistrationPDFLogo").Tables[0];
                log.CreateLogEntry("Called LookupTableController for logo path with regid " + regID + "& key logo RegistrationPDFLogo", Logging.LogPriority.Information);
                using (StreamReader sr = new StreamReader(templateNameWithPath))
                {
                    string lineOfData;
                    int lineNumber = 0;
                    log.CreateLogEntry(" Read and display lines from the file until the end of the file is reached. " + regID + "& key logo RegistrationPDFLogo", Logging.LogPriority.Information);

                    // Read and display lines from the file until the end of the file is reached.
                    while ((lineOfData = sr.ReadLine()) != null)
                    {
                        lineOfData = ReplaceTag(lineOfData, "LogoURL", ObjectControllerHelper.GetString("AppSettingsValue", dtPDFlogo.Rows[0]));
                        log.CreateLogEntry(" ReplaceTag  LogoURL to AppSettingsValue value " + regID, Logging.LogPriority.Information);

                        // System.Diagnostics.Debug.WriteLine("Line " + lineNumber.ToString());
                        lineNumber++;
                        DataRow provRow = GetProviderInfo(regID);
                        if (provRow != null)
                        {
                            var usr = provRow["NAME"].ToString();
                            lineOfData = ReplaceTag(lineOfData, "Provider_Name", usr);
                            log.CreateLogEntry(" ReplaceTag  Provider_Name " + regID, Logging.LogPriority.Information);

                        }
                        if (dtUserName.Rows.Count > 0)
                            lineOfData = ReplaceTag(lineOfData, "User_ID", ObjectControllerHelper.GetString("UserName", dtUserName.Rows[0]));
                        log.CreateLogEntry(" ReplaceTag User_ID to UserName " + regID, Logging.LogPriority.Information);

                        if (dtInitials.Rows.Count > 0)
                        {
                            log.CreateLogEntry(string.Format("ReplaceTag INITIALS_ID to NameOfPersonAttesting {0}", regID), Logging.LogPriority.Information);
                            lineOfData = ReplaceTag(lineOfData, "INITIALS_ID", ObjectControllerHelper.GetString("INITIALS", dtInitials.Rows[0]));

                            log.CreateLogEntry(string.Format("ReplaceTag REG_SIGNATURE to Signature {0}", regID), Logging.LogPriority.Information);
                            
                            if (dtInitials.Columns.Contains("REG_SIGNATURE"))
                            {
                                string signatureImage = signatureImage = ObjectControllerHelper.GetString("REG_SIGNATURE", dtInitials.Rows[0]);

                                if (!string.IsNullOrEmpty(signatureImage))
                                {
                                    lineOfData = ReplaceTag(lineOfData, "REG_SIGNATURE", signatureImage);
                                }
                            }
                        }


                        lineOfData = ReplaceTag(lineOfData, "Org_Title", ObjectControllerHelper.GetString("Title", dataRow));
                        log.CreateLogEntry(" ReplaceTag Org_Title to Title " + regID, Logging.LogPriority.Information);

                        lineOfData = ReplaceTag(lineOfData, "ORG_FIRSTNAME", ObjectControllerHelper.GetString("FIRST_NAME", dataRow));
                        log.CreateLogEntry(" ReplaceTag ORG_FIRSTNAME to FIRST_NAME " + regID, Logging.LogPriority.Information);

                        lineOfData = ReplaceTag(lineOfData, "ORG_MIDDLENAME", ObjectControllerHelper.GetString("MIDDLE_INITIAL", dataRow));
                        log.CreateLogEntry(" ReplaceTag ORG_MIDDLENAME to MIDDLE_INITIAL " + regID, Logging.LogPriority.Information);

                        lineOfData = ReplaceTag(lineOfData, "ORG_LASTNAME", ObjectControllerHelper.GetString("LAST_NAME", dataRow)); //npi
                        log.CreateLogEntry(" ReplaceTag ORG_LASTNAME  " + regID, Logging.LogPriority.Information);

                        lineOfData = ReplaceTag(lineOfData, "NPI", ObjectControllerHelper.GetString("NPI", dataRow));
                        log.CreateLogEntry(" ReplaceTag NPI  " + regID, Logging.LogPriority.Information);

                        lineOfData = ReplaceTag(lineOfData, "NAME", ObjectControllerHelper.GetString("NAME", dataRow));
                        log.CreateLogEntry(" ReplaceTag NAME  " + regID, Logging.LogPriority.Information);
                        var gender = string.IsNullOrEmpty(ObjectControllerHelper.GetString("GENDER", dataRow)) ? "" : ObjectControllerHelper.GetString("GENDER", dataRow);
                        lineOfData = ReplaceTag(lineOfData, "NPI_Gender", gender == "" ? "" : gender.ToLower() == "m" ? "Male" : (gender.ToLower() == "f" ? "FeMale" : "Other"));
                        log.CreateLogEntry(" ReplaceTag NPI_Gender  " + regID, Logging.LogPriority.Information);
                        lineOfData = ReplaceTag(lineOfData, "NPI_Gender_Display", (entitytypeID != CON.ProviderCategoryTypeID.Individual) ? "display:none;" : "");
                        log.CreateLogEntry(" ReplaceTag NPI_Gender_Display  " + regID, Logging.LogPriority.Information);
                        lineOfData = ReplaceTag(lineOfData, "ENROLLMENT_STATUS_REASON", string.IsNullOrEmpty(ObjectControllerHelper.GetString("ENROLLMENT_STATUS_REASONS_DESC", dataRow)) ? "Not Set Yet" : ObjectControllerHelper.GetString("ENROLLMENT_STATUS_REASONS_DESC", dataRow));
                        log.CreateLogEntry(" ReplaceTag ENROLLMENT_STATUS_REASON  " + regID, Logging.LogPriority.Information);
                        lineOfData = ReplaceTag(lineOfData, "DBA", ObjectControllerHelper.GetString("DBA", dataRow));
                        log.CreateLogEntry(" ReplaceTag DBA  " + regID, Logging.LogPriority.Information);
                        lineOfData = ReplaceTag(lineOfData, "PRACTICE_TYPE_NAME", ObjectControllerHelper.GetString("PRACTICE_TYPE_DESC", dataRow));
                        //DataSet DSOwner = RegistrationController.SelectRegOwnerByOwnerID(ObjectControllerHelper.GetInt("OWNERSHIP_TYPE_ID1", dataRowProvider));
                        lineOfData = ReplaceTag(lineOfData, "OWNERSHIP_TYPE_ID", ObjectControllerHelper.GetString("Ownership_Type_Name", dataRow));

                        lineOfData = ReplaceTag(lineOfData, "TAX_ID", ObjectControllerHelper.GetString("TAX_ID", dataRow));


                        lineOfData = string.IsNullOrEmpty(ObjectControllerHelper.GetString("BIRTH_DATE", dataRow))
                        ? ReplaceTag(lineOfData, "DATE_OF_BIRTH", "")
                        : ReplaceTag(lineOfData, "DATE_OF_BIRTH", ObjectControllerHelper.GetDateTime("BIRTH_DATE", dataRow).ToString("MM/dd/yyyy"));
                        lineOfData = ReplaceTag(lineOfData, "BIRTH_COUNTRY", ObjectControllerHelper.GetString("BIRTH_COUNTRY", dataRow));
                        lineOfData = ReplaceTag(lineOfData, "BIRTH_STATE", ObjectControllerHelper.GetString("BIRTH_STATE", dataRow));
                        lineOfData = ReplaceTag(lineOfData, "BIRTH_CITY", ObjectControllerHelper.GetString("BIRTH_CITY", dataRow));

                        lineOfData = ReplaceTag(lineOfData, "DATE_OF_BIRTH_Display", (entitytypeID != CON.ProviderCategoryTypeID.Individual) ? "display:none;" : "");

                        lineOfData = string.IsNullOrEmpty(ObjectControllerHelper.GetString("NPI_START_DATE", dataRow))
                            ? ReplaceTag(lineOfData, "NPI_START_DATE", "")
                            : ReplaceTag(lineOfData, "NPI_START_DATE", ObjectControllerHelper.GetDateTime("NPI_START_DATE", dataRow).ToString("MM/dd/yyyy"));

                        lineOfData = ReplaceTag(lineOfData, "PROVIDER_TYPE_NAME", ObjectControllerHelper.GetString("PROVIDER_TYPE_NAME", dataRow));
                        log.CreateLogEntry(" ReplaceTag PROVIDER_TYPE_NAME  " + regID, Logging.LogPriority.Information);
                        lineOfData = !string.IsNullOrEmpty(ObjectControllerHelper.GetString("END_DATE", dataRow))
                            ? ReplaceTag(lineOfData, "END_DATE", ObjectControllerHelper.GetDateTime("END_DATE", dataRow).ToString("MM/dd/yyyy"))
                            : ReplaceTag(lineOfData, "END_DATE", "");
                        log.CreateLogEntry(" ReplaceTag END_DATE  " + regID, Logging.LogPriority.Information);
                        string statusCode = ObjectControllerHelper.GetString("ENROLLMENT_STATUS_CODE", dataRow);
                        if (string.IsNullOrEmpty(statusCode))
                        {
                            lineOfData = ReplaceTag(lineOfData, "ENROLLMENT_STATUS_CODE_DESCRIPTION", "Not Set Yet");
                        }
                        else
                        {
                            if (statusCode == "1")
                                lineOfData = ReplaceTag(lineOfData, "ENROLLMENT_STATUS_CODE_DESCRIPTION", "ACTIVE");
                            if (statusCode == "2")
                                lineOfData = ReplaceTag(lineOfData, "ENROLLMENT_STATUS_CODE_DESCRIPTION", "INACTIVE");
                            if (statusCode == "3")
                                lineOfData = ReplaceTag(lineOfData, "ENROLLMENT_STATUS_CODE_DESCRIPTION", "REPORTING ONLY");
                        }
                        log.CreateLogEntry(" ReplaceTag ENROLLMENT_STATUS_CODE_DESCRIPTION  " + regID, Logging.LogPriority.Information);

                        //string statusReason = ObjectControllerHelper.GetString("ENROLLMENT_STATUS_REASONS_DESC", dataRow);
                        //lineOfData = ReplaceTag(lineOfData, "ENROLLMENT_STATUS_CODE_DESCRIPTION", string.IsNullOrEmpty(statusCode) ? "Not Set Yet" : statusCode);

                        lineOfData = ReplaceTag(lineOfData, "CAQH", ObjectControllerHelper.GetString("CAQH", dataRow));
                        if (dataRow != null && Convert.ToString(dataRow["IS_OHIO_RESIDENT"]) == "True")
                        {
                            lineOfData = ReplaceTag(lineOfData, "IS_OHIO_RESIDENT", "Yes");
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "IS_OHIO_RESIDENT", "No");
                        }
                        if (entitytypeID != CON.ProviderCategoryTypeID.Individual)
                        {
                            lineOfData = ReplaceTag(lineOfData, "disIndOrgInfo", "none");
                        }
                        if (ds.Tables.Count > 65 && ds.Tables[65].Rows.Count == 0)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYMEDICAREGRIDIn", "none");

                        }
                        log.CreateLogEntry(" ReplaceTag DISPLAYMEDICAREGRIDIn  " + regID, Logging.LogPriority.Information);

                        if (!isPrimaryContactInfoVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYPRIMARY", "none");
                        }
                        else
                        {
                            if (address.Rows.Count > 0)
                            {
                                // Primary Contact Information
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYPRIMARY", "block");
                                StringBuilder sb_typeaddress = new StringBuilder();

                                string response = ObjectControllerHelper.GetString("CONTACT_TYPE", address.Rows[0]);
                                if (response == "P")
                                {
                                    sb_typeaddress.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                    sb_typeaddress.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");

                                }
                                else
                                {
                                    sb_typeaddress.AppendLine(string.Format("<input type=\"radio\" />"));
                                    sb_typeaddress.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");

                                }
                                log.CreateLogEntry(" ReplaceTag CONTACT_TYPE of response type P  " + regID, Logging.LogPriority.Information);

                                if (response == "B")
                                {
                                    sb_typeaddress.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                    sb_typeaddress.AppendLine("<span   class=\"tooltipNew\">Organization</span>");

                                }
                                else
                                {
                                    sb_typeaddress.AppendLine(string.Format("<input type=\"radio\"  />"));
                                    sb_typeaddress.AppendLine("<span   class=\"tooltipNew\">Organization</span>");

                                }
                                log.CreateLogEntry(" ReplaceTag CONTACT_TYPE of response type B  " + regID, Logging.LogPriority.Information);

                                lineOfData = ReplaceTag(lineOfData, "PRIMARYAddress_Type", sb_typeaddress.ToString());
                                string orgname = ObjectControllerHelper.GetString("PRACTICE_NAME", address.Rows[0]);
                                if (response == "P")
                                {

                                    lineOfData = ReplaceTag(lineOfData, "displayorgname", "none");
                                    lineOfData = ReplaceTag(lineOfData, "displayIndgname", "table-row");
                                    lineOfData = ReplaceTag(lineOfData, "Pcfname", ObjectControllerHelper.GetString("FIRST_NAME", address.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "Pcmname", ObjectControllerHelper.GetString("MIDDLE_NAME", address.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "Pclname", ObjectControllerHelper.GetString("LAST_NAME", address.Rows[0]));
                                    log.CreateLogEntry(" ReplaceTag PRACTICE_NAME of response type P  " + regID, Logging.LogPriority.Information);

                                }
                                else
                                {
                                    lineOfData = ReplaceTag(lineOfData, "displayorgname", "table-row");
                                    lineOfData = ReplaceTag(lineOfData, "displayIndgname", "none");
                                    lineOfData = ReplaceTag(lineOfData, "orgname", orgname);
                                    log.CreateLogEntry(" ReplaceTag PRACTICE_NAME of response type other  " + regID, Logging.LogPriority.Information);

                                }

                                lineOfData = ReplaceTag(lineOfData, "CONTACT_NAME", ObjectControllerHelper.GetString("CONTACT_NAME", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_TITLE", ObjectControllerHelper.GetString("TITLE", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_ADDRESS1", ObjectControllerHelper.GetString("ADDRESS1", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_ADDRESS2", ObjectControllerHelper.GetString("ADDRESS2", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_CITY", ObjectControllerHelper.GetString("CITY", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_STATE", ObjectControllerHelper.GetString("STATE", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_County", ObjectControllerHelper.GetString("CountyName", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_ZIP", ObjectControllerHelper.GetString("ZIP", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "EXT_ZIP", ObjectControllerHelper.GetString("EXT_ZIP", address.Rows[0]));
                                log.CreateLogEntry(" ReplaceTag CONTACT_ZIP,CONTACT_STATE,CONTACT_CITY " + regID, Logging.LogPriority.Information);
                                string phone = System.Text.RegularExpressions.Regex.Replace(ObjectControllerHelper.GetString("PHONE1", address.Rows[0]), "\\D", string.Empty);

                                lineOfData = ReplaceTag(lineOfData, "CONTACT_PHONE_NUMBER1", FormatPhone(phone));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_PHONE_EXT1", ObjectControllerHelper.GetString("PHONE1_EXT", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "EXT1YES", ObjectControllerHelper.GetBool("CAN_TEXT1", address.Rows[0]) ? "Checked" : "Unchecked");
                                lineOfData = ReplaceTag(lineOfData, "EXT1NO", ObjectControllerHelper.GetBool("CAN_TEXT1", address.Rows[0]) ? "Unchecked" : "Checked");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_PHONE_NUMBER2", ObjectControllerHelper.GetString("PHONE2", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_PHONE_EXT2", ObjectControllerHelper.GetString("PHONE2_EXT", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "EXT2YES", ObjectControllerHelper.GetBool("CAN_TEXT2", address.Rows[0]) ? "Checked" : "Unchecked");
                                lineOfData = ReplaceTag(lineOfData, "EXT2NO", ObjectControllerHelper.GetBool("CAN_TEXT2", address.Rows[0]) ? "Unchecked" : "Checked");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_FAX_NUMBER1", ObjectControllerHelper.GetString("FAX1", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_FAX_NUMBER2", ObjectControllerHelper.GetString("FAX2", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_EMAIL_ADDRESS1", ObjectControllerHelper.GetString("EMAIL1", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_EMAIL_ADDRESS2", ObjectControllerHelper.GetString("EMAIL2", address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_MANAGER", ObjectControllerHelper.GetString("OFFICE_MANAGER", address.Rows[0]));
                                //lineOfData = ReplaceTag(lineOfData, "OFFICE_MANAGER", "");
                                log.CreateLogEntry(" ReplaceTag OFFICE_MANAGER " + regID, Logging.LogPriority.Information);

                            }
                            else
                            {
                                // Primary Contact Information
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYPRIMARY", "block");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_NAME", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_TITLE", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_ADDRESS1", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_CITY", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_STATE", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_PHONE_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_PHONE_EXT1", "");
                                lineOfData = ReplaceTag(lineOfData, "EXT1NO", "Checked");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_PHONE_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_PHONE_EXT2", "");
                                lineOfData = ReplaceTag(lineOfData, "EXT2NO", "Checked");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_FAX_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_FAX_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_EMAIL_ADDRESS1", "");
                                lineOfData = ReplaceTag(lineOfData, "CONTACT_EMAIL_ADDRESS2", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_MANAGER", "");
                                log.CreateLogEntry(" ReplaceTag Primary Contact Information " + regID, Logging.LogPriority.Information);

                            }
                        }

                        // default order s here, if it is dynamic then it will be replacfrom datatable
                        log.CreateLogEntry("Default order s here, if it is dynamic then it will be replacfrom datatable " + regID, Logging.LogPriority.Information);

                        if (dtDivUserName.Rows.Count > 0)
                        {

                            int seq = 0;
                            log.CreateLogEntry("DtDivUserName  foreach loop" + regID, Logging.LogPriority.Information);

                            foreach (DataRow row in dtDivUserName.Rows)
                            {

                                string displayName = row["REG_SECTION_NAME"].ToString();
                                displayName = displayName.Replace(" ", "");
                                if (row["IS_VISIBLE"].ToString() == "True")
                                {
                                    seq++;
                                    lineOfData = ReplaceTag(lineOfData, "ord" + displayName, seq + "");
                                }
                                else
                                {

                                    lineOfData = ReplaceTag(lineOfData, "dis" + displayName, "none");
                                }

                            }
                            log.CreateLogEntry("DtDivUserName end   foreach loop" + regID, Logging.LogPriority.Information);

                        }
                        /*lineOfData = ReplaceTag(lineOfData, "saqproviderinfo", "1");
                        lineOfData = ReplaceTag(lineOfData, "saqprimarycontactinfo", "2");
                        lineOfData = ReplaceTag(lineOfData, "saqcrdentailcontact", "3");
                        lineOfData = ReplaceTag(lineOfData, "saqcpccontactinfo", "4");
                        lineOfData = ReplaceTag(lineOfData, "saqofficeinfo", "5");
                        lineOfData = ReplaceTag(lineOfData, "saqofficeind", "6");
                        lineOfData = ReplaceTag(lineOfData, "saqPhysicalAddr", "7");
                        lineOfData = ReplaceTag(lineOfData, "saqCorrespAddr", "8");
                        lineOfData = ReplaceTag(lineOfData, "saqBillingAddr", "9");
                        lineOfData = ReplaceTag(lineOfData, "saqOTHERSERVICE", "10");
                        lineOfData = ReplaceTag(lineOfData, "saqWorkHistory", "11");
                        lineOfData = ReplaceTag(lineOfData, "saqSiteVisit", "12");
                        lineOfData = ReplaceTag(lineOfData, "saqNursingFacility", "13");
                        lineOfData = ReplaceTag(lineOfData, "saqOtherAddr", "14");
                        lineOfData = ReplaceTag(lineOfData, "saqHomeOffice", "15");
                        lineOfData = ReplaceTag(lineOfData, "saq1099Add", "16");
                        lineOfData = ReplaceTag(lineOfData, "saqLongTermAddress", "17");
                        lineOfData = ReplaceTag(lineOfData, "saqSPECIALTY", "18");
                        lineOfData = ReplaceTag(lineOfData, "saqADDLSPECIALTY", "19");
                        lineOfData = ReplaceTag(lineOfData, "saqTAXONOMY", "20");
                        lineOfData = ReplaceTag(lineOfData, "saqADDLTAXONOMY", "21");
                        lineOfData = ReplaceTag(lineOfData, "saqLICENSE", "22");
                        lineOfData = ReplaceTag(lineOfData, "saqBOARD", "23");
                        lineOfData = ReplaceTag(lineOfData, "saqCLIA", "24");

                        lineOfData = ReplaceTag(lineOfData, "saqMCQ", "25");
                        lineOfData = ReplaceTag(lineOfData, "saqCDS", "26");
                        lineOfData = ReplaceTag(lineOfData, "saqDEA", "27");
                        lineOfData = ReplaceTag(lineOfData, "saqBHINFO", "28");
                        lineOfData = ReplaceTag(lineOfData, "saqMEDICAREDETAIL", "29");
                        lineOfData = ReplaceTag(lineOfData, "saqMEDICAID", "30");
                        lineOfData = ReplaceTag(lineOfData, "saqINSURANCE", "31");
                        lineOfData = ReplaceTag(lineOfData, "saqDNDENTALLICENSE", "32");
                        lineOfData = ReplaceTag(lineOfData, "saqDENTALLICENSE", "33");
                        lineOfData = ReplaceTag(lineOfData, "saqVISIONCERT", "34");
                        lineOfData = ReplaceTag(lineOfData, "saqPHARMACY", "35");
                        lineOfData = ReplaceTag(lineOfData, "saqCPRCERT", "36");
                        lineOfData = ReplaceTag(lineOfData, "saqNURSINGCERT", "37");
                        lineOfData = ReplaceTag(lineOfData, "saqCATEGORYOFSVC", "38");
                        lineOfData = ReplaceTag(lineOfData, "saqAFFILIATIONS", "39");
                        lineOfData = ReplaceTag(lineOfData, "saqEDUCATION", "40");
                        lineOfData = ReplaceTag(lineOfData, "saqMALPRACTICE", "41");
                        lineOfData = ReplaceTag(lineOfData, "saqCONTRACT", "42");
                        lineOfData = ReplaceTag(lineOfData, "saqWORKFLOW", "43");
                        lineOfData = ReplaceTag(lineOfData, "saqOwnerInfo", "44");
                        lineOfData = ReplaceTag(lineOfData, "saqW9", "45");
                        lineOfData = ReplaceTag(lineOfData, "saqWAIVERSVC", "46");
                        lineOfData = ReplaceTag(lineOfData, "saqHOUSEHOLDMEMBERS", "47");
                        lineOfData = ReplaceTag(lineOfData, "saqApplicationFee", "48");
                        lineOfData = ReplaceTag(lineOfData, "saqidOprator", "49");
                        lineOfData = ReplaceTag(lineOfData, "saqDME", "50");
                        lineOfData = ReplaceTag(lineOfData, "saqREQUIREDDOCUMENTS", "51");
                        lineOfData = ReplaceTag(lineOfData, "saqPracticePartnership", "52");
                        lineOfData = ReplaceTag(lineOfData, "saqohoAgreement", "53");*/
                        //MCOAffiliation 
                        if (!isMCOAffiliationVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYMCQ", "none");
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYMCQ", "block");
                            log.CreateLogEntry("ReplaceTag DISPLAYMCQ" + regID, Logging.LogPriority.Information);

                            if (MCQ.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(confirmedMCQ))
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYCMCQ", "block");
                                    lineOfData = ReplaceTag(lineOfData, "ConfirmedMCPList", confirmedMCQ);
                                    log.CreateLogEntry("ReplaceTag ConfirmedMCPList if part" + regID, Logging.LogPriority.Information);

                                }
                                else
                                {
                                    lineOfData = ReplaceTag(lineOfData, "ConfirmedMCPList", "");

                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYCMCQ", "none");
                                    log.CreateLogEntry("ReplaceTag ConfirmedMCPList else part" + regID, Logging.LogPriority.Information);

                                }
                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("IS_CAREPLAN_INTRESTED", MCQ.Rows[0])))
                                    if (ObjectControllerHelper.GetBool("IS_CAREPLAN_INTRESTED", MCQ.Rows[0]))
                                    {
                                        lineOfData = ReplaceTag(lineOfData, "MCQYES", "Checked");
                                        lineOfData = ReplaceTag(lineOfData, "MCQNO", "Unchecked");

                                    }
                                    else
                                    {

                                        lineOfData = ReplaceTag(lineOfData, "MCQNO", "Checked");
                                        lineOfData = ReplaceTag(lineOfData, "MCQYES", "Unchecked");

                                    }
                                log.CreateLogEntry("ReplaceTag IS_CAREPLAN_INTRESTED else part" + regID, Logging.LogPriority.Information);

                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CAREPLAN_IDS", MCQ.Rows[0])))
                                {
                                    string[] CAREPLAN_IDS = ObjectControllerHelper.GetString("CAREPLAN_IDS", MCQ.Rows[0]).Split(',');
                                    log.CreateLogEntry("ReplaceTag CAREPLAN_IDS started foreach loop" + regID, Logging.LogPriority.Information);
                                    foreach (var CAREPLAN_ID in CAREPLAN_IDS)
                                    {
                                        var chkItem = CAREPLAN_ID.ToString().Trim();
                                        switch (chkItem)
                                        {
                                            case "1":
                                                lineOfData = ReplaceTag(lineOfData, "Aetna", "Checked");
                                                break;
                                            case "2":
                                                lineOfData = ReplaceTag(lineOfData, "Buckeye", "Checked");
                                                break;
                                            case "3":
                                                lineOfData = ReplaceTag(lineOfData, "CareSource", "Checked");
                                                break;
                                            case "4":
                                                lineOfData = ReplaceTag(lineOfData, "Molina", "Checked");
                                                break;
                                            case "5":
                                                lineOfData = ReplaceTag(lineOfData, "United", "Checked");
                                                break;
                                            case "7":
                                                lineOfData = ReplaceTag(lineOfData, "AmeriHealth", "Checked");
                                                break;
                                            case "8":
                                                lineOfData = ReplaceTag(lineOfData, "Anthem", "Checked");
                                                break;
                                            case "9":
                                                lineOfData = ReplaceTag(lineOfData, "Humana", "Checked");
                                                break;
                                        }
                                    }
                                    log.CreateLogEntry("ReplaceTag CAREPLAN_IDS End foreach loop" + regID, Logging.LogPriority.Information);
                                }
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "ConfirmedMCPList", confirmedMCQ);
                                log.CreateLogEntry("ReplaceTag ConfirmedMCPList " + regID, Logging.LogPriority.Information);

                            }
                        }

                        //Board Certification
                        if (isBoardCertificateVisible && !string.IsNullOrEmpty(boardCertifications))
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYBOARD", "block");
                            lineOfData = ReplaceTag(lineOfData, "BOARD_LIST", boardCertifications);
                        }
                        else
                        {
                            //lineOfData = ReplaceTag(lineOfData, "BOARD_LIST", "");
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYBOARD", "none");
                        }
                        log.CreateLogEntry("ReplaceTag Board Certification " + regID, Logging.LogPriority.Information);

                        if (isContractMaintenanceVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCONTRACT", "block");

                            lineOfData = !string.IsNullOrEmpty(contracts)
                                    ? ReplaceTag(lineOfData, "CONTRACT_LIST", contracts)
                                    : ReplaceTag(lineOfData, "CONTRACT_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Contract Maintenance records found.")));
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCONTRACT", "none");
                        }
                        log.CreateLogEntry("ReplaceTag DISPLAYCONTRACT " + regID, Logging.LogPriority.Information);

                        if (isCredentialingContactVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCREDENTIAL", "block");
                            lineOfData = !string.IsNullOrEmpty(contacts)
                                ? ReplaceTag(lineOfData, "CREDENTIAL_LIST", contacts)
                                : ReplaceTag(lineOfData, "CREDENTIAL_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Credential Contact records found.")));
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCREDENTIAL", "none");
                            //lineOfData = ReplaceTag(lineOfData, "CREDENTIALL_LIST", "");
                        }
                        log.CreateLogEntry("ReplaceTag DISPLAYCREDENTIAL " + regID, Logging.LogPriority.Information);

                        //SatelliteLocations
                        if (isSatelliteLocationsVisible && !string.IsNullOrEmpty(otherservices))
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOTHERSERVICE", "block");
                            lineOfData = !string.IsNullOrEmpty(otherservices)
                                    ? ReplaceTag(lineOfData, "OTHERSERVICE_LIST", otherservices)
                                    : ReplaceTag(lineOfData, "OTHERSERVICE_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Other Service Locations records found.")));
                        }
                        else
                        {
                            //lineOfData = ReplaceTag(lineOfData, "OTHERSERVICE_LIST", "");
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOTHERSERVICE", "none");
                        }
                        log.CreateLogEntry("ReplaceTag SatelliteLocations " + regID, Logging.LogPriority.Information);

                        if (isNursingFacilityVentilatorVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYNursingFacility", "block");
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYNursingFacility", "none");
                        log.CreateLogEntry("ReplaceTag DISPLAYNursingFacility " + regID, Logging.LogPriority.Information);

                        //Home Office Addresss
                        if (!isHomeOfficeAddressVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYHomeOffice", "none");
                        }
                        else
                        {
                            if (homeAddress.Rows.Count > 0)
                            {
                                log.CreateLogEntry("ReplaceTag DISPLAYHomeOffice " + regID, Logging.LogPriority.Information);

                                // Home Address
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYHomeOffice", "block");
                                StringBuilder sb_type = new StringBuilder();

                                string response = ObjectControllerHelper.GetString("CONTACT_TYPE", homeAddress.Rows[0]);
                                if (response == "P")
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");
                                    log.CreateLogEntry("ReplaceTag DISPLAYHomeOffice with type P " + regID, Logging.LogPriority.Information);

                                }
                                else
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\"  />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");
                                    log.CreateLogEntry("ReplaceTag DISPLAYHomeOffice with type other " + regID, Logging.LogPriority.Information);

                                }
                                if (response == "B")
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");
                                    log.CreateLogEntry("ReplaceTag DISPLAYHomeOffice with type B " + regID, Logging.LogPriority.Information);

                                }
                                else
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\"  />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");
                                    log.CreateLogEntry("ReplaceTag DISPLAYHomeOffice Organization with type other " + regID, Logging.LogPriority.Information);

                                }
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_Type", sb_type.ToString());
                                string orgname = ObjectControllerHelper.GetString("PRACTICE_NAME", homeAddress.Rows[0]);
                                if (response == "P")
                                {

                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYHomeOfficeOrg", "none");
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYHomeOfficeInd", "table-row");
                                    lineOfData = ReplaceTag(lineOfData, "HomeOffice_FN", ObjectControllerHelper.GetString("FIRST_NAME", homeAddress.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "HomeOffice_MN", ObjectControllerHelper.GetString("MIDDLE_NAME", homeAddress.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "HomeOffice_LN", ObjectControllerHelper.GetString("LAST_NAME", homeAddress.Rows[0]));
                                    log.CreateLogEntry("ReplaceTag PRACTICE_NAME  with type P " + regID, Logging.LogPriority.Information);

                                }
                                else
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYHomeOfficeOrg", "table-row");
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYHomeOfficeInd", "none");
                                    lineOfData = ReplaceTag(lineOfData, "HomeOffice_OrgName", orgname);
                                    log.CreateLogEntry("ReplaceTag PRACTICE_NAME  with type Other " + regID, Logging.LogPriority.Information);

                                }

                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_TITLE", ObjectControllerHelper.GetString("TITLE", homeAddress.Rows[0]));

                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_ADDRESS1", ObjectControllerHelper.GetString("ADDRESS1", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_ADDRESS2", ObjectControllerHelper.GetString("ADDRESS2", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_CITY", ObjectControllerHelper.GetString("CITY", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_STATE", ObjectControllerHelper.GetString("STATE", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_COUNTY", ObjectControllerHelper.GetString("CountyName", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_ZIP", ObjectControllerHelper.GetString("ZIP", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_EXT_ZIP", ObjectControllerHelper.GetString("EXT_ZIP", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_PHONE_NUMBER1", FormatPhone(ObjectControllerHelper.GetString("PHONE1", homeAddress.Rows[0])));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_PHONE_EXT1", ObjectControllerHelper.GetString("PHONE1_EXT", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_PHONE_NUMBER2", ObjectControllerHelper.GetString("PHONE2", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_PHONE_EXT2", ObjectControllerHelper.GetString("PHONE2_EXT", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_FAX_NUMBER1", FormatPhone(ObjectControllerHelper.GetString("FAX1", homeAddress.Rows[0])));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_FAX_NUMBER2", ObjectControllerHelper.GetString("FAX2", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_Contact", ObjectControllerHelper.GetString("CONTACT_NAME", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_EMAIL_ADDRESS", ObjectControllerHelper.GetString("EMAIL1", homeAddress.Rows[0]));
                                log.CreateLogEntry("ReplaceTag HomeOffice_TITLE  with all details " + regID, Logging.LogPriority.Information);
                            }
                            else
                            {
                                // Home Address
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYHomeOffice", "block");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_Type", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_TITLE", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_FName", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_MName", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_LName", "");

                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_ADDRESS1", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_ADDRESS2", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_CITY", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_STATE", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_COUNTY", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_EXT_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_PHONE_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_PHONE_EXT1", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_PHONE_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_PHONE_EXT2", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_FAX_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_FAX_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_Contact", "");
                                lineOfData = ReplaceTag(lineOfData, "HomeOffice_EMAIL_ADDRESS", "");
                                log.CreateLogEntry("ReplaceTag DISPLAYHomeOffice  with all details " + regID, Logging.LogPriority.Information);

                            }
                        }

                        //Hospital Addresss
                        if (!isHospitalAddressVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYHospitalAddress", "none");
                        }
                        else
                        {
                            if (hospitalAddress.Rows.Count > 0)
                            {
                                log.CreateLogEntry("ReplaceTag DISPLAYHospitalAddress " + regID, Logging.LogPriority.Information);
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYHospitalAddress", "block");
                                StringBuilder sb_type = new StringBuilder();

                                string response = ObjectControllerHelper.GetString("CONTACT_TYPE", hospitalAddress.Rows[0]);
                                if (response == "P")
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");
                                    log.CreateLogEntry("ReplaceTag DISPLAYHospitalAddress with type P " + regID, Logging.LogPriority.Information);

                                }
                                else
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\"  />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");
                                    log.CreateLogEntry("ReplaceTag DISPLAYHospitalAddress with type other " + regID, Logging.LogPriority.Information);

                                }
                                if (response == "B")
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");
                                    log.CreateLogEntry("ReplaceTag DISPLAYHospitalAddress with type B " + regID, Logging.LogPriority.Information);

                                }
                                else
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\"  />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");
                                    log.CreateLogEntry("ReplaceTag DISPLAYHospitalAddress Organization with type other " + regID, Logging.LogPriority.Information);

                                }
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_Type", sb_type.ToString());
                                string orgname = ObjectControllerHelper.GetString("PRACTICE_NAME", hospitalAddress.Rows[0]);
                                if (response == "P")
                                {

                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYHospitalAddressOrg", "none");
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYHospitalAddressInd", "table-row");
                                    lineOfData = ReplaceTag(lineOfData, "HospitalAddress_FN", ObjectControllerHelper.GetString("FIRST_NAME", hospitalAddress.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "HospitalAddress_MN", ObjectControllerHelper.GetString("MIDDLE_NAME", hospitalAddress.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "HospitalAddress_LN", ObjectControllerHelper.GetString("LAST_NAME", hospitalAddress.Rows[0]));
                                    log.CreateLogEntry("ReplaceTag PRACTICE_NAME  with type P " + regID, Logging.LogPriority.Information);

                                }
                                else
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYHospitalAddressOrg", "table-row");
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYHospitalAddressInd", "none");
                                    lineOfData = ReplaceTag(lineOfData, "HospitalAddress_OrgName", orgname);
                                    log.CreateLogEntry("ReplaceTag PRACTICE_NAME  with type Other " + regID, Logging.LogPriority.Information);

                                }

                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_TITLE", ObjectControllerHelper.GetString("TITLE", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_Location_Type", ObjectControllerHelper.GetString("LOCATION_TYPE_NAME", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_ADDRESS1", ObjectControllerHelper.GetString("ADDRESS1", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_ADDRESS2", ObjectControllerHelper.GetString("ADDRESS2", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_CITY", ObjectControllerHelper.GetString("CITY", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_STATE", ObjectControllerHelper.GetString("STATE", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_ZIP", ObjectControllerHelper.GetString("ZIP", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_COUNTY", ObjectControllerHelper.GetString("CountyName", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_EXT_ZIP", ObjectControllerHelper.GetString("EXT_ZIP", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_PHONE_NUMBER1", FormatPhone(ObjectControllerHelper.GetString("PHONE1", hospitalAddress.Rows[0])));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_PHONE_EXT1", ObjectControllerHelper.GetString("PHONE1_EXT", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_PHONE_NUMBER2", ObjectControllerHelper.GetString("PHONE2", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_PHONE_EXT2", ObjectControllerHelper.GetString("PHONE2_EXT", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_FAX_NUMBER1", FormatPhone(ObjectControllerHelper.GetString("FAX1", hospitalAddress.Rows[0])));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_FAX_NUMBER2", ObjectControllerHelper.GetString("FAX2", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_Contact", ObjectControllerHelper.GetString("CONTACT_NAME", hospitalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_EMAIL_ADDRESS", ObjectControllerHelper.GetString("EMAIL1", hospitalAddress.Rows[0]));
                                log.CreateLogEntry("ReplaceTag HospitalAddress_TITLE  with all details " + regID, Logging.LogPriority.Information);
                            }
                            else
                            {
                                // Hospital Addresss
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYHospitalAddress", "block");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_Type", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_TITLE", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_Location_Type", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_FName", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_MName", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_LName", "");

                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_ADDRESS1", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_ADDRESS2", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_CITY", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_STATE", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_COUNTY", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_EXT_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_PHONE_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_PHONE_EXT1", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_PHONE_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_PHONE_EXT2", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_FAX_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_FAX_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_Contact", "");
                                lineOfData = ReplaceTag(lineOfData, "HospitalAddress_EMAIL_ADDRESS", "");
                                log.CreateLogEntry("ReplaceTag DISPLAYHospitalAddress  with all details " + regID, Logging.LogPriority.Information);

                            }
                        }

                        if (isLongTermCareAddressVisible && longTermAddress.Rows.Count > 0)
                        {

                            // Long Term Information
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYLongTermAddress", "block");
                            lineOfData = ReplaceTag(lineOfData, "LongTermLocation_Type", ObjectControllerHelper.GetString("LOCATION_TYPE_NAME", longTermAddress.Rows[0]));
                            log.CreateLogEntry("ReplaceTag Long Term Information  with all details " + regID, Logging.LogPriority.Information);

                            //  lineOfData = ReplaceTag(lineOfData, "LongTermAddress_Type", ObjectControllerHelper.GetString("CONTACT_TYPE", homeAddress.Rows[0]) == CON.ContactType.Organization
                            //                   ? nameof(CON.ContactType.Organization) : nameof(CON.ContactType.Individual));

                            StringBuilder sb_type = new StringBuilder();
                            log.CreateLogEntry("ReplaceTag Long Term Information  with Contact type Response  " + regID, Logging.LogPriority.Information);

                            string response = ObjectControllerHelper.GetString("CONTACT_TYPE", longTermAddress.Rows[0]);
                            if (response == "P")
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");

                            }
                            else
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\"  />"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");

                            }
                            if (response == "B")
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");

                            }
                            else
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\"  />"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");

                            }
                            log.CreateLogEntry("ReplaceTag LongTermAddress_Type" + regID, Logging.LogPriority.Information);

                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_Type", sb_type.ToString());
                            string orgname = ObjectControllerHelper.GetString("PRACTICE_NAME", longTermAddress.Rows[0]);
                            if (response == "P")
                            {

                                lineOfData = ReplaceTag(lineOfData, "DISPLAYLongTermAddressOrg", "none");
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYLongTermAddressInd", "table-row");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_FName", ObjectControllerHelper.GetString("FIRST_NAME", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_MName", ObjectControllerHelper.GetString("MIDDLE_NAME", homeAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_LName", ObjectControllerHelper.GetString("LAST_NAME", homeAddress.Rows[0]));
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYLongTermAddressOrg", "table-row");
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYLongTermAddressInd", "none");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_TITLE", orgname);
                            }
                            log.CreateLogEntry("ReplaceTag LongTermAddress_Type with all response" + regID, Logging.LogPriority.Information);


                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_ADDRESS1", ObjectControllerHelper.GetString("ADDRESS1", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_ADDRESS2", ObjectControllerHelper.GetString("ADDRESS2", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_CITY", ObjectControllerHelper.GetString("CITY", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_STATE", ObjectControllerHelper.GetString("STATE", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_ZIP", ObjectControllerHelper.GetString("ZIP", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_COUNTY", ObjectControllerHelper.GetString("CountyName", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_EXT_ZIP", ObjectControllerHelper.GetString("EXT_ZIP", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_PHONE_NUMBER1", ObjectControllerHelper.GetString("PHONE1", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_PHONE_EXT1", ObjectControllerHelper.GetString("PHONE1_EXT", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_PHONE_NUMBER2", ObjectControllerHelper.GetString("PHONE2", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_PHONE_EXT2", ObjectControllerHelper.GetString("PHONE2_EXT", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_FAX_NUMBER1", ObjectControllerHelper.GetString("FAX1", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_FAX_NUMBER2", ObjectControllerHelper.GetString("FAX2", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_Contact", ObjectControllerHelper.GetString("CONTACT_NAME", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LongTermAddress_EMAIL_ADDRESS", ObjectControllerHelper.GetString("EMAIL1", homeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "LONG_TERM_ADDRESS", "");
                            log.CreateLogEntry("ReplaceTag LONG_TERM_ADDRESS with data" + regID, Logging.LogPriority.Information);

                        }
                        else
                        {
                            log.CreateLogEntry("ReplaceTag DISPLAYLongTermAddress with data" + regID, Logging.LogPriority.Information);
                            // Long Contact Information
                            if (isLongTermCareAddressVisible)
                            {
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYLongTermAddress", "block");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_Type", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_TITLE", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_FName", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_MName", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_LName", "");

                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_ADDRESS1", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_ADDRESS2", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_CITY", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_STATE", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_COUNTY", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_EXT_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_PHONE_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_PHONE_EXT1", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_PHONE_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_PHONE_EXT2", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_FAX_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_FAX_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_Contact", "");
                                lineOfData = ReplaceTag(lineOfData, "LongTermAddress_EMAIL_ADDRESS", "");
                                lineOfData = ReplaceTag(lineOfData, "LONG_TERM_ADDRESS", "No Long Term Address information found");
                            }
                            else
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYLongTermAddress", "none");
                        }

                        if (dtQuestion != null)
                        {
                            log.CreateLogEntry("ReplaceTag for DtQuestion started for each" + regID, Logging.LogPriority.Information);
                            foreach (DataRow row in dtQuestion.Rows)
                            {
                                switch (ObjectControllerHelper.GetString("QUESTION_TYPE_ID", row))
                                {
                                    case "BH01":
                                        //questionsAnswered = true;
                                        //lineOfData = ReplaceTag(lineOfData, "BH01", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[0]));
                                        lineOfData = ReplaceTag(lineOfData, "BH01YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH01NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");
                                        break;
                                    case "BH02":
                                        //lineOfData = ReplaceTag(lineOfData, "BH02", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[1]));
                                        lineOfData = ReplaceTag(lineOfData, "BH02YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH02NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH03":
                                        //lineOfData = ReplaceTag(lineOfData, "BH03", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[2]));
                                        lineOfData = ReplaceTag(lineOfData, "BH03YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH03NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH04":
                                        //lineOfData = ReplaceTag(lineOfData, "BH04", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[3]));
                                        lineOfData = ReplaceTag(lineOfData, "BH04YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH04NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH05":
                                        //lineOfData = ReplaceTag(lineOfData, "BH05", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[4]));
                                        lineOfData = ReplaceTag(lineOfData, "BH05YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH05NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH06":
                                        //lineOfData = ReplaceTag(lineOfData, "BH06", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[5]));
                                        lineOfData = ReplaceTag(lineOfData, "BH06YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH06NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH07":
                                        //lineOfData = ReplaceTag(lineOfData, "BH07", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[6]));
                                        lineOfData = ReplaceTag(lineOfData, "BH07YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH07NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH08":
                                        //lineOfData = ReplaceTag(lineOfData, "BH08", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[7]));
                                        lineOfData = ReplaceTag(lineOfData, "BH08YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH08NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH09":
                                        //lineOfData = ReplaceTag(lineOfData, "BH09", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[8]));
                                        lineOfData = ReplaceTag(lineOfData, "BH09YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH09NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH10":
                                        //lineOfData = ReplaceTag(lineOfData, "BH10", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[9]));
                                        lineOfData = ReplaceTag(lineOfData, "BH10YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH10NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH11":
                                        //lineOfData = ReplaceTag(lineOfData, "BH11", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[10]));
                                        lineOfData = ReplaceTag(lineOfData, "BH11YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH11NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH12":
                                        //lineOfData = ReplaceTag(lineOfData, "BH12", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[11]));
                                        lineOfData = ReplaceTag(lineOfData, "BH12YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH12NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "BH13":
                                        //lineOfData = ReplaceTag(lineOfData, "BH13", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[12]));
                                        lineOfData = ReplaceTag(lineOfData, "BH13YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "BH13NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");
                                        break;
                                    case "NVW0":
                                        bool rdoOptionSelect = ObjectControllerHelper.GetBool("RESPONSE", row);
                                        if (rdoOptionSelect)
                                        {

                                            lineOfData = ReplaceTag(lineOfData, "SHOWNVQUESTIONS", "block");
                                            lineOfData = ReplaceTag(lineOfData, "NV01", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[0]));
                                            lineOfData = ReplaceTag(lineOfData, "NV02", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[1]));
                                            lineOfData = ReplaceTag(lineOfData, "NV03", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[2]));
                                            lineOfData = ReplaceTag(lineOfData, "NV04", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[3]));
                                            lineOfData = ReplaceTag(lineOfData, "NV05", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[4]));
                                            lineOfData = ReplaceTag(lineOfData, "NV06", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[5]));
                                            lineOfData = ReplaceTag(lineOfData, "NV07", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[6]));
                                            lineOfData = ReplaceTag(lineOfData, "NV08", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[7]));

                                            lineOfData = ReplaceTag(lineOfData, "NW01", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNWQuestions.Rows[0]));
                                            lineOfData = ReplaceTag(lineOfData, "NW02", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNWQuestions.Rows[1]));
                                            lineOfData = ReplaceTag(lineOfData, "NW03", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNWQuestions.Rows[2]));
                                        }
                                        else
                                            lineOfData = ReplaceTag(lineOfData, "SHOWNVQUESTIONS", "none");


                                        lineOfData = ReplaceTag(lineOfData, "NVW0YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NVW0NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");
                                        break;


                                    case "NV01":
                                        //questionsAnswered = true;
                                        //lineOfData = ReplaceTag(lineOfData, "NV01", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[0]));
                                        lineOfData = ReplaceTag(lineOfData, "NV01YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NV01NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");
                                        break;
                                    case "NV02":
                                        //lineOfData = ReplaceTag(lineOfData, "NV02", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[1]));
                                        lineOfData = ReplaceTag(lineOfData, "NV02YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NV02NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "NV03":
                                        //lineOfData = ReplaceTag(lineOfData, "NV03", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[2]));
                                        lineOfData = ReplaceTag(lineOfData, "NV03YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NV03NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "NV04":
                                        //lineOfData = ReplaceTag(lineOfData, "NV04", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[3]));
                                        lineOfData = ReplaceTag(lineOfData, "NV04YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NV04NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "NV05":
                                        //lineOfData = ReplaceTag(lineOfData, "NV05", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[4]));
                                        lineOfData = ReplaceTag(lineOfData, "NV05YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NV05NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "NV06":
                                        //lineOfData = ReplaceTag(lineOfData, "NV06", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[5]));
                                        lineOfData = ReplaceTag(lineOfData, "NV06YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NV06NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "NV07":
                                        //lineOfData = ReplaceTag(lineOfData, "NV07", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[6]));
                                        lineOfData = ReplaceTag(lineOfData, "NV07YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NV07NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "NV08":
                                        //lineOfData = ReplaceTag(lineOfData, "NV08", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNVQuestions.Rows[7]));
                                        lineOfData = ReplaceTag(lineOfData, "NV08YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NV08NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;

                                    case "NW01":
                                        //questionsAnswered = true;
                                        //lineOfData = ReplaceTag(lineOfData, "NW01", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNWQuestions.Rows[0]));
                                        lineOfData = ReplaceTag(lineOfData, "NW01YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NW01NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");
                                        break;
                                    case "NW02":
                                        //lineOfData = ReplaceTag(lineOfData, "NW02", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNWQuestions.Rows[1]));
                                        lineOfData = ReplaceTag(lineOfData, "NW02YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NW02NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;
                                    case "NW03":
                                        //lineOfData = ReplaceTag(lineOfData, "NW03", ObjectControllerHelper.GetString("QUESTION_TEXT", dtNWQuestions.Rows[2]));
                                        lineOfData = ReplaceTag(lineOfData, "NW03YES", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Checked" : "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "NW03NO", ObjectControllerHelper.GetBool("RESPONSE", row) ? "Unchecked" : "Checked");

                                        break;

                                    case "PA01":
                                        if (Convert.ToBoolean(ObjectControllerHelper.GetString("RESPONSE", row)))
                                            lineOfData = ReplaceTag(lineOfData, "checkA1", "checked");
                                        break;
                                    case "PA02":
                                        if (Convert.ToBoolean(ObjectControllerHelper.GetString("RESPONSE", row)))
                                            lineOfData = ReplaceTag(lineOfData, "checkA2", "checked");
                                        break;
                                    case "PA03":
                                        if (Convert.ToBoolean(ObjectControllerHelper.GetString("RESPONSE", row)))
                                            lineOfData = ReplaceTag(lineOfData, "checkA3", "checked");
                                        //OHPNM-16669 SAM530 Remove provision check language in agreement​ during revalidation. 
                                        if (wfEventTypeID == CON.WorkflowEventType.RevalReg
                                             && !isReapplication && !isReactivation)
                                            lineOfData = ReplaceTag(lineOfData, "DISPLAYPROVCHECKINFO", "none");
                                        else
                                            lineOfData = ReplaceTag(lineOfData, "DISPLAYPROVCHECKINFO", "block");
                                        break;
                                    case "PA07":
                                        if (Convert.ToBoolean(ObjectControllerHelper.GetString("RESPONSE", row)))
                                            lineOfData = ReplaceTag(lineOfData, "checkA4", "checked");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            log.CreateLogEntry("ReplaceTag for DtQuestion END for each" + regID, Logging.LogPriority.Information);

                        }
                        StringBuilder sb = new StringBuilder();
                        if (dtQuestion != null)
                        {
                            log.CreateLogEntry("ReplaceTag for DtQuestion Started QUESTION_TYPE's for each" + regID, Logging.LogPriority.Information);

                            //OHPNM-13403 -  To display agreement questions in the pdf
                            StringBuilder sbIPQns = new StringBuilder();
                            int workflowEventTypeId = wfEventTypeID;
                            bool isIpQnsVisible = (entitytypeID == CON.ProviderCategoryTypeID.Individual
                                && (workflowEventTypeId == CON.WorkflowEventType.NewReg || workflowEventTypeId == CON.WorkflowEventType.RevalReg || workflowEventTypeId == CON.WorkflowEventType.ChangeProviderType)
                                && applicationTypeID != CON.ApplicationType.CPC);

                            foreach (DataRow dr in dtQuestion.Rows)
                            {
                                string response = dr["RESPONSE"].ToString().ToLower();
                                string yesValue = response == "true" ? "Checked" : "Unchecked";
                                string noValue = response == "false" ? "Checked" : "Unchecked";
                                if (dr["QUESTION_TYPE_ID"].ToString() == "PA08" || dr["QUESTION_TYPE_ID"].ToString() == "PA09" || dr["QUESTION_TYPE_ID"].ToString() == "PA10"
                                    || dr["QUESTION_TYPE_ID"].ToString() == "PA11" || dr["QUESTION_TYPE_ID"].ToString() == "PA12")
                                {
                                    sb.AppendLine("<p style=\"text-align:left;font-size:10pt; font-weight:normal\">" + dr["QUESTION_TEXT"].ToString() + "\"");
                                    sb.AppendLine(string.Format("</br><input type=\"radio\" name=\"{0}\" {1} />Yes<input type=\"radio\" name=\"{0}\" {2} />No</br>", dr["QUESTION_TYPE_ID"].ToString(), yesValue, noValue));
                                    if (response == "true")
                                        sb.AppendLine(string.Format("</br><div style=\"border:gray 1px solid;width:800px;padding:3px;\">'" + dr["RESPONSE_COMMENT"].ToString() + "'</div>"));
                                    //lineOfData = ReplaceTag(lineOfData, "Q09", ObjectControllerHelper.GetString("QUESTION_TEXT", dr));
                                }
                                if (isIpQnsVisible && (dr["QUESTION_TYPE_ID"].ToString() == "IP01" || dr["QUESTION_TYPE_ID"].ToString() == "IP02" || dr["QUESTION_TYPE_ID"].ToString() == "IP03" || dr["QUESTION_TYPE_ID"].ToString() == "IP04"))
                                {
                                    sbIPQns.AppendLine("<p style=\"text-align:left;font-size:11pt; font-weight:normal\">" + dr["QUESTION_TEXT"].ToString() + "\"");
                                    sbIPQns.AppendLine(string.Format("</br><input type=\"radio\" name=\"{0}\" {1} />No<input type=\"radio\" name=\"{0}\" {2} />Yes</br>", dr["QUESTION_TYPE_ID"].ToString(), noValue, yesValue));
                                    if (response == "true")
                                        sbIPQns.AppendLine(string.Format("</br><div style=\"border:gray 1px solid;font-size:10pt; font-weight:normal;width:800px;padding:3px;\">'" + dr["RESPONSE_COMMENT"].ToString() + "'</div>"));

                                }
                            }
                            log.CreateLogEntry("ReplaceTag for DtQuestion Ended QUESTION_TYPE's for each" + regID, Logging.LogPriority.Information);

                            lineOfData = ReplaceTag(lineOfData, "IPQuestions", sbIPQns.ToString());
                            if (isIpQnsVisible && sbIPQns.Length != 0)
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYIPINFO", "block");
                            else
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYIPINFO", "none");
                        }
                        lineOfData = ReplaceTag(lineOfData, "CredentialingText", sb.ToString());
                        if (sb.Length != 0)
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCredentialing", "block");
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCredentialing", "none");

                        lineOfData = ReplaceTag(lineOfData, "SHOWNVQUESTIONS", "none");
                        if (isAddress1099FormVisible && oneAddress.Rows.Count > 0)
                        {
                            log.CreateLogEntry("ReplaceTag 1099 address" + regID, Logging.LogPriority.Information);

                            // 1099 address
                            lineOfData = ReplaceTag(lineOfData, "DISPLAY1099", "block");
                            StringBuilder sb_type = new StringBuilder();

                            string response = ObjectControllerHelper.GetString("CONTACT_TYPE", oneAddress.Rows[0]);
                            if (response == "P")
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");

                            }
                            else
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\"  />"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");

                            }
                            if (response == "B")
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");

                            }
                            else
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\" />"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");

                            }
                            log.CreateLogEntry("ReplaceTag 1099 DISPLAY1099 End" + regID, Logging.LogPriority.Information);

                            lineOfData = ReplaceTag(lineOfData, "One_Type", sb_type.ToString());
                            string orgname = ObjectControllerHelper.GetString("PRACTICE_NAME", oneAddress.Rows[0]);

                            if (response == "P")
                            {

                                lineOfData = ReplaceTag(lineOfData, "DISPLAY1099Org", "none");
                                lineOfData = ReplaceTag(lineOfData, "DISPLAY1099Ind", "table-row");
                                lineOfData = ReplaceTag(lineOfData, "One_FN", ObjectControllerHelper.GetString("FIRST_NAME", oneAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "One_MN", ObjectControllerHelper.GetString("MIDDLE_NAME", oneAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "One_LN", ObjectControllerHelper.GetString("LAST_NAME", oneAddress.Rows[0]));
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "DISPLAY1099Org", "table-row");
                                lineOfData = ReplaceTag(lineOfData, "DISPLAY1099Ind", "none");
                                lineOfData = ReplaceTag(lineOfData, "One_orgname", orgname);
                            }
                            log.CreateLogEntry("ReplaceTag PRACTICE_NAME End" + regID, Logging.LogPriority.Information);
                            lineOfData = ReplaceTag(lineOfData, "One_TITLE", ObjectControllerHelper.GetString("TITLE", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_ADDRESS1", ObjectControllerHelper.GetString("ADDRESS1", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_ADDRESS2", ObjectControllerHelper.GetString("ADDRESS2", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_CITY", ObjectControllerHelper.GetString("CITY", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_STATE", ObjectControllerHelper.GetString("STATE", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_ZIP", ObjectControllerHelper.GetString("ZIP", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_COUNTY", ObjectControllerHelper.GetString("CountyName", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_EXT_ZIP", ObjectControllerHelper.GetString("EXT_ZIP", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_PHONE_NUMBER1", ObjectControllerHelper.GetString("PHONE1", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_PHONE_EXT1", ObjectControllerHelper.GetString("PHONE1_EXT", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_PHONE_NUMBER2", ObjectControllerHelper.GetString("PHONE2", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_PHONE_EXT2", ObjectControllerHelper.GetString("PHONE2_EXT", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_FAX_NUMBER1", ObjectControllerHelper.GetString("FAX1", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_FAX_NUMBER2", ObjectControllerHelper.GetString("FAX2", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_Contact", ObjectControllerHelper.GetString("CONTACT_NAME", oneAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "One_EMAIL_ADDRESS", ObjectControllerHelper.GetString("EMAIL1", oneAddress.Rows[0]));
                            log.CreateLogEntry("ReplaceTag One_TITLE End" + regID, Logging.LogPriority.Information);
                            if (OneForm.Rows.Count > 0)
                            {
                                lineOfData = ReplaceTag(lineOfData, "One_TAXID", ObjectControllerHelper.GetString("IRS_TAX_ID", OneForm.Rows[0]));
                                log.CreateLogEntry("ReplaceTag One_TAXID Started" + regID, Logging.LogPriority.Information);
                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("IRS_TAX_TYPE_ID", OneForm.Rows[0])))
                                    if (ObjectControllerHelper.GetString("IRS_TAX_TYPE_ID", OneForm.Rows[0]) == "15")
                                    {
                                        lineOfData = ReplaceTag(lineOfData, "IRSYES", "Checked");
                                        lineOfData = ReplaceTag(lineOfData, "IRSNO", "Unchecked");
                                    }
                                    else
                                    {
                                        lineOfData = ReplaceTag(lineOfData, "IRSNO", "Checked");
                                        lineOfData = ReplaceTag(lineOfData, "IRSYES", "Unchecked");
                                    }
                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("IS_TAX_EXEMPT", OneForm.Rows[0])))
                                    if (ObjectControllerHelper.GetBool("IS_TAX_EXEMPT", OneForm.Rows[0]))
                                    {
                                        lineOfData = ReplaceTag(lineOfData, "TaxExemptYES", "Checked");
                                        lineOfData = ReplaceTag(lineOfData, "TaxExemptNO", "Unchecked");
                                    }
                                    else
                                    {
                                        lineOfData = ReplaceTag(lineOfData, "TaxExemptYES", "Unchecked");
                                        lineOfData = ReplaceTag(lineOfData, "TaxExemptNO", "Checked");
                                    }
                                log.CreateLogEntry("ReplaceTag IS_TAX_EXEMPT END" + regID, Logging.LogPriority.Information);
                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("IS_FORM_W9", OneForm.Rows[0])))
                                    if (ObjectControllerHelper.GetBool("IS_FORM_W9", OneForm.Rows[0]))
                                    {
                                        lineOfData = ReplaceTag(lineOfData, "OneW9YES", "Checked");
                                        lineOfData = ReplaceTag(lineOfData, "OneW9NO", "Unchecked");
                                    }
                                    else
                                    {
                                        lineOfData = ReplaceTag(lineOfData, "OneW9NO", "Checked");
                                        lineOfData = ReplaceTag(lineOfData, "OneW9YES", "Unchecked");
                                    }

                                log.CreateLogEntry("ReplaceTag IS_FORM_W9 END" + regID, Logging.LogPriority.Information);

                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("IS_FORM_147", OneForm.Rows[0])))
                                    if (ObjectControllerHelper.GetBool("IS_FORM_147", OneForm.Rows[0]))
                                    {
                                        lineOfData = ReplaceTag(lineOfData, "One147YES", "Checked");
                                        lineOfData = ReplaceTag(lineOfData, "One147NO", "Unchecked");

                                    }
                                    else
                                    {
                                        lineOfData = ReplaceTag(lineOfData, "One147NO", "Checked");
                                        lineOfData = ReplaceTag(lineOfData, "One147YES", "Unchecked");
                                    }
                                log.CreateLogEntry("ReplaceTag IS_FORM_147 END" + regID, Logging.LogPriority.Information);
                            }

                        }
                        else
                        {
                            lineOfData = isAddress1099FormVisible ? ReplaceTag(lineOfData, "DISPLAY1099", "block") : ReplaceTag(lineOfData, "DISPLAY1099", "none");
                        }

                        log.CreateLogEntry("ReplaceTag DISPLAY1099 END" + regID, Logging.LogPriority.Information);

                        // Specialty   
                        log.CreateLogEntry("ReplaceTag Specialty Start" + regID, Logging.LogPriority.Information);

                        if (isSpecialtiesVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYSPECIALTY", "block");
                            lineOfData = !string.IsNullOrEmpty(formattedSpecialties)
                                ? ReplaceTag(lineOfData, "PRIMARY_SPECIALTY_LIST", formattedSpecialties)
                                : ReplaceTag(lineOfData, "PRIMARY_SPECIALTY_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Specialty records found.")));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYSPECIALTY", "none");

                        log.CreateLogEntry("ReplaceTag Specialty END" + regID, Logging.LogPriority.Information);

                        // Additional Specialty
                        log.CreateLogEntry("ReplaceTag Additional Specialty Start" + regID, Logging.LogPriority.Information);
                        if (isCPCSpecialtiesVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYADDLSPECIALTY", "block");
                            lineOfData = !string.IsNullOrEmpty(formattedAddlSpecialties)
                                   ? ReplaceTag(lineOfData, "ADDL_SPECIALTY_LIST", formattedSpecialties)
                                   : ReplaceTag(lineOfData, "ADDL_SPECIALTY_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No CPC Specialty records found.")));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYADDLSPECIALTY", "none");
                        log.CreateLogEntry("ReplaceTag Additional Specialty END" + regID, Logging.LogPriority.Information);
                        // Taxonomy
                        log.CreateLogEntry("ReplaceTag Taxonomy Started" + regID, Logging.LogPriority.Information);
                        if (isSpecialtiesTaxonomyVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYTAXONOMY", "block");
                            lineOfData = !string.IsNullOrEmpty(formattedSpecialties)
                                ? ReplaceTag(lineOfData, "PRIMARY_TAXONOMY_LIST", formattedTaxonomies)
                                : ReplaceTag(lineOfData, "PRIMARY_TAXONOMY_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No primary taxonomy records found.")));
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYTAXONOMY", "none");
                        }
                        log.CreateLogEntry("ReplaceTag Taxonomy End" + regID, Logging.LogPriority.Information);

                        // Additional Taxonomy
                        if (addlTaxonomies.Count > 0)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYADDLTAXONOMY", "block");
                            lineOfData = ReplaceTag(lineOfData, "ADDL_TAXONOMY_LIST", formattedAddlTaxonomies);
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYADDLTAXONOMY", "none");
                        }
                        log.CreateLogEntry("ReplaceTag  Additional Taxonomy End" + regID, Logging.LogPriority.Information);

                        // Professional Licenses
                        if (isLicenseVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYLICENSE", "block");
                            lineOfData = licenses.Count > 0
                                ? ReplaceTag(lineOfData, "LICENSE_LIST", formattedLicenses)
                                : ReplaceTag(lineOfData, "LICENSE_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No License information found.")));
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYLICENSE", "none");
                        }
                        log.CreateLogEntry("ReplaceTag  Professional Licenses End" + regID, Logging.LogPriority.Information);

                        // DEA Numbers
                        if (isFederalDEAVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYDEA", "block");
                            if (deaNumbers.Count > 0)
                            {
                                lineOfData = ReplaceTag(lineOfData, "DEA_LIST", formattedDEANumbers);
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "DEA_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No DEA information found.")));
                            }
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYDEA", "none");
                        }
                        log.CreateLogEntry("ReplaceTag   DEA Numbers End" + regID, Logging.LogPriority.Information);

                        // Behavioral Health Information
                        if (isBHInfoVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYBHINFO", "block");
                            if (behavioralHealthInfo.Count > 0)
                            {
                                lineOfData = ReplaceTag(lineOfData, "BHINFO_LIST", formattedbehavioralHealth);
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYBHINFO_COMMENT", string.Empty);
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYBHINFO_COMMENT", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Behaviroal information found.")));
                            }
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYBHINFO", "none");
                        }
                        log.CreateLogEntry("ReplaceTag  Behavioral Health Information End" + regID, Logging.LogPriority.Information);

                        // CLIA Numbers
                        if (isCLIAVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCLIA", "block");
                            lineOfData = ReplaceTag(lineOfData, "CLIA_LIST", formattedCLIANumbers);
                            lineOfData = cliaNumbers.Count > 0
                                ? ReplaceTag(lineOfData, "DISPLAYCLIA_COMMENT", string.Empty)
                                : ReplaceTag(lineOfData, "DISPLAYCLIA_COMMENT", formattedCLIANumbers);
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCLIA", "none");
                        }
                        log.CreateLogEntry("ReplaceTag  CLIA Numbers End" + regID, Logging.LogPriority.Information);

                        // Dental License 
                        if (isDentalLicenseisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYDENTALLICENSE", "block");
                            lineOfData = dentalLicenses.Count > 0
                                ? ReplaceTag(lineOfData, "DENTAL_LICENSE_LIST", formattedDentalLicenses)
                                : ReplaceTag(lineOfData, "DENTAL_LICENSE_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Dentail information records found.")));
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYDENTALLICENSE", "none");
                        }
                        log.CreateLogEntry("ReplaceTag Dental License  End" + regID, Logging.LogPriority.Information);
                        // Vision Certifications
                        if (isVisionVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYVISIONCERT", "block");
                            lineOfData = visionCerts.Count > 0
                                ? ReplaceTag(lineOfData, "VISION_CERT_LIST", formattedVisionCerts)
                                : ReplaceTag(lineOfData, "VISION_CERT_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Vision Certifications records found.")));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYVISIONCERT", "none");
                        log.CreateLogEntry("ReplaceTag Vision Certifications  End" + regID, Logging.LogPriority.Information);


                        // Pharmacy Details
                        if (isPharmacyVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYPHARMACY", "block");
                            if (dtPhrmaDetails.Rows.Count > 0)
                            {
                                lineOfData = ReplaceTag(lineOfData, "PHAR_NAME", ObjectControllerHelper.GetString("PHARMACY_NAME", dtPhrmaDetails.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "CHIEF_PHAR_NAME", ObjectControllerHelper.GetString("CHIEF_PHARMACIST_NAME", dtPhrmaDetails.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PHAR_NUMBER", ObjectControllerHelper.GetString("OCCUPANCY_PERMIT_NUMBER", dtPhrmaDetails.Rows[0]));
                            }

                            lineOfData = pharmacists.Count > 0
                                ? ReplaceTag(lineOfData, "PHARMACY_LIST", formattedPharmacists)
                                : ReplaceTag(lineOfData, "PHARMACY_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Pharmacist found.")));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYPHARMACY", "none");
                        log.CreateLogEntry("ReplaceTag Pharmacy Details  End" + regID, Logging.LogPriority.Information);


                        // Pharmacists
                        //if (pharmacists.Count > 0)
                        //{
                        //    lineOfData = ReplaceTag(lineOfData, "DISPLAYPHARMACIST", "block");
                        //    lineOfData = ReplaceTag(lineOfData, "PHARMACIST_LIST", formattedPharmacists);
                        //}
                        //else
                        //{
                        //    lineOfData = ReplaceTag(lineOfData, "DISPLAYPHARMACIST", "none");
                        //}

                        // State CDS Numbers
                        if (isStateCDSNumberVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCDS", "block");
                            lineOfData = stateCDSNumbers.Count > 0
                                ? ReplaceTag(lineOfData, "CDS_LIST", formattedStateCDSNumbers)
                                : ReplaceTag(lineOfData, "CDS_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No State CDS Numbers records found.")));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCDS", "none");

                        log.CreateLogEntry("Replace Tag State CDS Numbers End" + regID, Logging.LogPriority.Information);

                        // CPR Certifications
                        if (isCPRVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCPRCERT", "block");
                            lineOfData = cprCerts.Count > 0
                                ? ReplaceTag(lineOfData, "CPR_CERT_LIST", formattedCPRCerts)
                                : ReplaceTag(lineOfData, "CPR_CERT_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No CPR certifications information found.")));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCPRCERT", "none");
                        log.CreateLogEntry("Replace Tag  CPR Certifications End" + regID, Logging.LogPriority.Information);

                        // Nursing Certifications
                        if (isNursingCertVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYNURSINGCERT", "block");
                            lineOfData = nursingCerts.Count > 0
                                    ? ReplaceTag(lineOfData, "NURSING_CERT_LIST", formattedNursingCerts)
                                    : ReplaceTag(lineOfData, "NURSING_CERT_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Nursing certifications information found.")));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYNURSINGCERT", "none");
                        log.CreateLogEntry("Replace Tag  Nursing Certifications End" + regID, Logging.LogPriority.Information);


                        // Categories of Servcie
                        if (isCategoryVisible && categoryOfSvcList.Count > 0)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCATEGORYOFSVC", "block");
                            lineOfData = nursingCerts.Count > 0
                                       ? ReplaceTag(lineOfData, "CATEGORY_OF_SVC_LIST", formattedCategoryOfSvcList)
                                       : ReplaceTag(lineOfData, "CATEGORY_OF_SVC_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Categories of Servcie information found.")));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCATEGORYOFSVC", "none");
                        log.CreateLogEntry("Replace Tag   Categories of Servcie End" + regID, Logging.LogPriority.Information);

                        // Medicare
                        if (medicare.Rows.Count > 0)
                        {

                            //string.IsNullOrEmpty(ObjectControllerHelper.GetString("MEDICARE_NUMBER", dataRow))
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYMEDICAREDETAIL", "block");

                            string state = "";
                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("MEDICARE_STATE", medicare.Rows[0])))
                            {
                                string stateid = ObjectControllerHelper.GetString("MEDICARE_STATE", medicare.Rows[0]);


                                DataTable dtStateCodes = ds.Tables[64];
                                state = dtStateCodes.AsEnumerable().Where(r => r.Field<string>("StateId")
                                                                   == stateid).Select(r => r.Field<string>("StateName")).FirstOrDefault(); ;
                            }
                            lineOfData = ReplaceTag(lineOfData, "MEDICAID_LIST", formattedMedicaidDetailsInfo);
                            lineOfData = ReplaceTag(lineOfData, "MEDICARE_STATE", state);
                            StringBuilder sb_type = new StringBuilder();

                            string response = ObjectControllerHelper.GetString("MEDICARE_NUMBER_TYPE", medicare.Rows[0]);
                            if (response == "CCN")
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\" name=\"{0}\" {1} />", "CCN (CMS Certification Number)", "Checked"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\">CCN (CMS Certification Number)</span></br>");

                            }
                            else
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\" name=\"{0}\" {1} />", "CCN (CMS Certification Number)", "Unchecked"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\">CCN (CMS Certification Number)</span></br>");

                            }
                            if (response == "PTAN")
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\" name=\"{0}\" {1}/>", "PTAN (Provider Transaction Access Number)", "Checked"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\">PTAN (Provider Transaction Access Number)</span>");

                            }
                            else
                            {
                                sb_type.AppendLine(string.Format("<input type=\"radio\" name=\"{0}\" {1}/>", "PTAN (Provider Transaction Access Number)", "Unchecked"));
                                sb_type.AppendLine("<span   class=\"tooltipNew\">PTAN (Provider Transaction Access Number)</span>");

                            }
                            //lineOfData = ReplaceTag(lineOfData, "Q09", ObjectControllerHelper.GetString("QUESTION_TEXT", dr));
                            lineOfData = ReplaceTag(lineOfData, "radionumbertype", sb_type.ToString());



                            lineOfData = ReplaceTag(lineOfData, "MEDICARE_NUMBER", ObjectControllerHelper.GetString("MEDICARE_NUMBER", medicare.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "MEDICARE_SNPI", ObjectControllerHelper.GetString("NPI", medicare.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "MEDICARE_ENROLLMENT_STATUS", ObjectControllerHelper.GetString("ENROLLMENT_STATUS_TYPE_NAME", medicare.Rows[0]));
                            if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("MEDICARE_EFF_DATE", medicare.Rows[0])))
                            {
                                lineOfData = ReplaceTag(lineOfData, "MEDICARE_ENROLLMENT_DATE", "");
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "MEDICARE_ENROLLMENT_DATE", ObjectControllerHelper.GetDateTime("MEDICARE_EFF_DATE", medicare.Rows[0]).ToString("MM/dd/yyyy"));
                            }



                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "MEDICAID_LIST", formattedMedicaidDetailsInfo);
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYMEDICAREDETAIL", "none");
                        }
                        log.CreateLogEntry("Replace Tag   Medicare End" + regID, Logging.LogPriority.Information);

                        // Medicare
                        //if (medicare.Rows.Count > 0)
                        //{
                        //    //string.IsNullOrEmpty(ObjectControllerHelper.GetString("MEDICARE_NUMBER", dataRow))
                        //    lineOfData = ReplaceTag(lineOfData, "DISPLAYMEDICARE", "block");

                        //    lineOfData = ReplaceTag(lineOfData, "MEDICARE_ENROLLMENT_STATUS", ObjectControllerHelper.GetString("ENROLLMENT_STATUS_TYPE_NAME", medicare.Rows[0]));
                        //    if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("MEDICARE_EFF_DATE", medicare.Rows[0])))
                        //    {
                        //        lineOfData = ReplaceTag(lineOfData, "MEDICARE_EFF_DATE", "");
                        //    }
                        //    else
                        //    {
                        //        lineOfData = ReplaceTag(lineOfData, "MEDICARE_EFF_DATE", ObjectControllerHelper.GetDateTime("MEDICARE_EFF_DATE", medicare.Rows[0]).ToString("MM/dd/yyyy"));
                        //    }
                        //    lineOfData = ReplaceTag(lineOfData, "MEDICARE_NUMBER", ObjectControllerHelper.GetString("MEDICARE_NUMBER", medicare.Rows[0]));
                        //    lineOfData = ReplaceTag(lineOfData, "NPI", ObjectControllerHelper.GetString("NPI", medicare.Rows[0]));
                        //    lineOfData = ReplaceTag(lineOfData, "NCPDP_NUMBER", ObjectControllerHelper.GetString("NCPDP_NUMBER", medicare.Rows[0]));
                        //    if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("NCPDP_START_DATE", medicare.Rows[0])))
                        //    {
                        //        lineOfData = ReplaceTag(lineOfData, "NCPDP_START_DATE", "");
                        //    }
                        //    else
                        //    {
                        //        lineOfData = ReplaceTag(lineOfData, "NCPDP_START_DATE", ObjectControllerHelper.GetDateTime("NCPDP_START_DATE", medicare.Rows[0]).ToString("MM/dd/yyyy"));
                        //    }
                        //    if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("NCPDP_END_DATE", medicare.Rows[0])))
                        //    {
                        //        lineOfData = ReplaceTag(lineOfData, "NCPDP_END_DATE", "");
                        //    }
                        //    else
                        //    {
                        //        lineOfData = ReplaceTag(lineOfData, "NCPDP_END_DATE", ObjectControllerHelper.GetDateTime("NCPDP_END_DATE", medicare.Rows[0]).ToString("MM/dd/yyyy"));
                        //    }
                        //    if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("REBATE_EXEMPTION_START_DATE", medicare.Rows[0])))
                        //    {
                        //        lineOfData = ReplaceTag(lineOfData, "REBATE_EXEMPTION_START_DATE", "");
                        //    }
                        //    else
                        //    {
                        //        lineOfData = ReplaceTag(lineOfData, "REBATE_EXEMPTION_START_DATE", ObjectControllerHelper.GetDateTime("REBATE_EXEMPTION_START_DATE", medicare.Rows[0]).ToString("MM/dd/yyyy"));
                        //    }
                        //    if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("REBATE_EXEMPTION_END_DATE", medicare.Rows[0])))
                        //    {
                        //        lineOfData = ReplaceTag(lineOfData, "REBATE_EXEMPTION_END_DATE", "");
                        //    }
                        //    else
                        //    {
                        //        lineOfData = ReplaceTag(lineOfData, "REBATE_EXEMPTION_END_DATE", ObjectControllerHelper.GetDateTime("REBATE_EXEMPTION_END_DATE", medicare.Rows[0]).ToString("MM/dd/yyyy"));
                        //    }
                        //    lineOfData = ReplaceTag(lineOfData, "B340PARTICIPANT", ObjectControllerHelper.GetString("B340PARTICIPANT", medicare.Rows[0]));
                        //}
                        //else
                        //{
                        //    lineOfData = ReplaceTag(lineOfData, "DISPLAYMEDICARE", "none");
                        //}

                        // Medicaid
                        if (isMiscellaneousVisible && medicare != null && medicare.Rows.Count > 0)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYMEDICAID", "block");
                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("MEDICARE_STATE", medicare.Rows[0])))
                            {
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYMEDICAID", "block");

                                lineOfData = ReplaceTag(lineOfData, "MEDICAID_ENROLLMENT_STATUS", ObjectControllerHelper.GetString("ENROLLMENT_STATUS_TYPE_NAME", medicare.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MEDICAID_STATE", ObjectControllerHelper.GetString("MEDICARE_STATE", medicare.Rows[0]));
                                if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("MEDICARE_EFF_DATE", medicare.Rows[0])) ||
                                    ObjectControllerHelper.GetString("MEDICARE_EFF_DATE", medicare.Rows[0]).Contains("1900"))
                                {
                                    lineOfData = ReplaceTag(lineOfData, "MEDICARE_EFF_DATE", "");
                                }
                                else
                                {
                                    lineOfData = ReplaceTag(lineOfData, "MEDICAID_EFF_DATE", ObjectControllerHelper.GetDateTime("MEDICARE_EFF_DATE", medicare.Rows[0]).ToString("MM/dd/yyyy"));
                                }
                                lineOfData = ReplaceTag(lineOfData, "NPI", ObjectControllerHelper.GetString("NPI", medicare.Rows[0]));
                            }
                            else
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYMEDICAID", "none");
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYMEDICAID", "none");

                        log.CreateLogEntry("Replace Tag   Medicaid End" + regID, Logging.LogPriority.Information);

                        // Insurance
                        if (isInsuranceVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYINSURANCE", "block");
                            if (dtInsurances.Rows != null && dtInsurances.Rows.Count > 0)
                            {
                                lineOfData = ReplaceTag(lineOfData, "INSURANCE_LIST", formattedInsurances);
                                lineOfData = ReplaceTag(lineOfData, "INSURANCE_COMMENT", string.Empty);
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "INSURANCE_COMMENT", formattedInsurances);
                            }
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYINSURANCE", "none");
                        log.CreateLogEntry("Replace Tag   Insurance End" + regID, Logging.LogPriority.Information);

                        // Dental Licenses
                        if (insurances.Count > 0)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYDENTALLICENSES", "block");
                            lineOfData = ReplaceTag(lineOfData, "DENTAL_LICENSE_LIST", formattedDentalLicenses);
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYDENTALLICENSES", "none");
                        log.CreateLogEntry("Replace Tag    Dental Licenses End" + regID, Logging.LogPriority.Information);

                        //Primary Service Address
                        if (!isPrimaryServiceVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYPhysicalAddr", "none");
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYPhysicalAddr", "block");
                            if (physicalAddress != null && physicalAddress.Rows.Count > 0)
                            {
                                // Primary Service Address
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_Type", ObjectControllerHelper.GetString("NAME", ds.Tables[0].Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_TITLE", ObjectControllerHelper.GetString("PRACTICE_NAME", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_FN", ObjectControllerHelper.GetString("FIRST_NAME", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_LN", ObjectControllerHelper.GetString("LAST_NAME", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_ADDRESS1", ObjectControllerHelper.GetString("ADDRESS1", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_ADDRESS2", ObjectControllerHelper.GetString("ADDRESS2", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_ADDRESS3", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_CITY", ObjectControllerHelper.GetString("CITY", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_STATE", ObjectControllerHelper.GetString("STATE", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_COUNTY", ObjectControllerHelper.GetString("CountyName", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_ZIP", ObjectControllerHelper.GetString("ZIP", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_ZIP_EXT", ObjectControllerHelper.GetString("EXT_ZIP", physicalAddress.Rows[0]));
                                string phone = System.Text.RegularExpressions.Regex.Replace(ObjectControllerHelper.GetString("PHONE1", physicalAddress.Rows[0]), "\\D", string.Empty);
                                log.CreateLogEntry("Replace Tag    PHONE1" + regID, Logging.LogPriority.Information);

                                lineOfData = ReplaceTag(lineOfData, "SERVICING_PHONE_NUMBER1", FormatPhone(phone));
                                //  lineOfData = ReplaceTag(lineOfData, "SERVICING_PHONE_NUMBER1", ObjectControllerHelper.GetString("PHONE1", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_PHONE_EXT1", ObjectControllerHelper.GetString("PHONE1_EXT", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_PHONE_NUMBER2", ObjectControllerHelper.GetString("PHONE2", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_PHONE_EXT2", ObjectControllerHelper.GetString("PHONE2_EXT", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_FAX_NUMBER1", ObjectControllerHelper.GetString("FAX1", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_FAX_NUMBER2", ObjectControllerHelper.GetString("FAX2", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_CONTACT_Name", ObjectControllerHelper.GetString("CONTACT_NAME", physicalAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_EMAIL_ADDRESS1", ObjectControllerHelper.GetString("EMAIL1", physicalAddress.Rows[0]));

                            }
                            else
                            {
                                // Primary Service Address
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_Type", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_TITLE", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_FN", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_LN", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_ADDRESS1", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_ADDRESS2", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_ADDRESS3", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_CITY", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_STATE", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_EXT_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_PHONE_NUMBER", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_FAX_NUMBER", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_PHONE_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_PHONE_EXT1", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_PHONE_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_PHONE_EXT2", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_FAX_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_FAX_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "SERVICING_EMAIL_ADDRESS1", "");
                                log.CreateLogEntry("Replace Tag    Primary Service Address End" + regID, Logging.LogPriority.Information);

                            }
                        }

                        if (isCPCContactInformationVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCPCCONTACT", "block");
                            lineOfData = ReplaceTag(lineOfData, "CPC_CONTACT_NAME", ObjectControllerHelper.GetString("CONTACT_NAME", ds.Tables[85].Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "CPC_CONTACT_TITLE", ObjectControllerHelper.GetString("TITLE", ds.Tables[85].Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "CPC_CONTACT_PHONE_NUMBER", ObjectControllerHelper.GetString("PHONE1", ds.Tables[85].Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "CPC_CONTACT_PHONE_EXT", ObjectControllerHelper.GetString("PHONE1_EXT", ds.Tables[85].Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "CPC_CONTACT_EMAIL", ObjectControllerHelper.GetString("EMAIL1", ds.Tables[85].Rows[0]));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCPCCONTACT", "none");

                        //Billing & Payment Address
                        if (!isBillingVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYBillingAddr", "none");
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYBillingAddr", "block");
                            if (billingAddress != null && billingAddress.Rows.Count > 0)
                            {
                                //Billing/Payment Contact Information
                                StringBuilder sb_type = new StringBuilder();

                                string response = ObjectControllerHelper.GetString("CONTACT_TYPE", billingAddress.Rows[0]);
                                if (response == "P")
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");

                                }
                                else
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\"  />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");

                                }
                                if (response == "B")
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");

                                }
                                else
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\" />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");

                                }
                                log.CreateLogEntry("Replace Tag   Billing or Payment Contact Information End" + regID, Logging.LogPriority.Information);

                                lineOfData = ReplaceTag(lineOfData, "BillingAddrType", sb_type.ToString());
                                if (response == "P")
                                {

                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYBillingAddrOrg", "none");
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYBillingAddrInd", "table-row");
                                    lineOfData = ReplaceTag(lineOfData, "PAYTO_FN", ObjectControllerHelper.GetString("FIRST_NAME", billingAddress.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "PAYTO_MN", ObjectControllerHelper.GetString("MIDDLE_NAME", billingAddress.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "PAYTO_LN", ObjectControllerHelper.GetString("LAST_NAME", billingAddress.Rows[0]));
                                }
                                else
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYBillingAddrOrg", "table-row");
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYBillingAddrInd", "none");
                                    lineOfData = ReplaceTag(lineOfData, "CHECK_PAYABLE_TO_NAME", ObjectControllerHelper.GetString("PRACTICE_NAME", billingAddress.Rows[0]));
                                }
                                log.CreateLogEntry("Replace Tag   Billing or Payment Contact Information all reasponse" + regID, Logging.LogPriority.Information);

                                lineOfData = ReplaceTag(lineOfData, "PAYTO_ADDRESS1", ObjectControllerHelper.GetString("ADDRESS1", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_ADDRESS2", ObjectControllerHelper.GetString("ADDRESS2", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_ADDRESS3", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_CITY", ObjectControllerHelper.GetString("CITY", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_STATE", ObjectControllerHelper.GetString("STATE", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_COUNTY", ObjectControllerHelper.GetString("CountyName", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_ZIP", ObjectControllerHelper.GetString("ZIP", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_ZIP_EXT", ObjectControllerHelper.GetString("EXT_ZIP", billingAddress.Rows[0]));
                                string phone = System.Text.RegularExpressions.Regex.Replace(ObjectControllerHelper.GetString("PHONE1", billingAddress.Rows[0]), "\\D", string.Empty);

                                lineOfData = ReplaceTag(lineOfData, "PAYTO_PHONE_NUMBER1", FormatPhone(phone));
                                // lineOfData = ReplaceTag(lineOfData, "PAYTO_PHONE_NUMBER1", ObjectControllerHelper.GetString("PHONE1", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_PHONE_EXT1", ObjectControllerHelper.GetString("PHONE1_EXT", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_PHONE_NUMBER2", ObjectControllerHelper.GetString("PHONE2", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_PHONE_EXT2", ObjectControllerHelper.GetString("PHONE2_EXT", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_FAX_NUMBER1", FormatPhone(ObjectControllerHelper.GetString("FAX1", billingAddress.Rows[0])));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_FAX_NUMBER2", ObjectControllerHelper.GetString("FAX2", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_CONTACT_NAME", ObjectControllerHelper.GetString("CONTACT_NAME", billingAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_EMAIL_ADDRESS1", ObjectControllerHelper.GetString("EMAIL1", billingAddress.Rows[0]));
                                log.CreateLogEntry("Replace Tag    PAYTO Details" + regID, Logging.LogPriority.Information);

                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "CHECK_PAYABLE_TO_NAME", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_ADDRESS1", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_ADDRESS2", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_ADDRESS3", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_CITY", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_STATE", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_PHONE_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_PHONE_EXT1", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_PHONE_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_PHONE_EXT2", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_FAX_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_FAX_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "PAYTO_EMAIL_ADDRESS1", "");
                                log.CreateLogEntry("Replace Tag    CHECK_PAYABLE_TO Details" + regID, Logging.LogPriority.Information);

                            }
                        }

                        //Correspondence Address 
                        if (!isCorrespondenceVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCorrespAddr", "none");
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYCorrespAddr", "block");
                            if (correspondenceAddress != null && correspondenceAddress.Rows.Count > 0)
                            {
                                //Correspondence Address 
                                StringBuilder sb_type = new StringBuilder();

                                string response = ObjectControllerHelper.GetString("CONTACT_TYPE", correspondenceAddress.Rows[0]);
                                if (response == "P")
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");

                                }
                                else
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\"  />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">Individual</span>");

                                }
                                if (response == "B")
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\" checked=\"Checked\" />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");

                                }
                                else
                                {
                                    sb_type.AppendLine(string.Format("<input type=\"radio\"  />"));
                                    sb_type.AppendLine("<span   class=\"tooltipNew\">Organization</span>");

                                }
                                log.CreateLogEntry("Replace Tag   Correspondence Address  Details with response" + regID, Logging.LogPriority.Information);
                                lineOfData = ReplaceTag(lineOfData, "CorrespType", sb_type.ToString());
                                //Billing/Payment Contact Information
                                if (response == "P")
                                {

                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYCorrespAddrOrg", "none");
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYCorrespAddrOrgInd", "table-row");
                                    lineOfData = ReplaceTag(lineOfData, "Corresp_title", ObjectControllerHelper.GetString("TITLE", correspondenceAddress.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "Corresp_FN", ObjectControllerHelper.GetString("FIRST_NAME", correspondenceAddress.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "Corresp_MN", ObjectControllerHelper.GetString("MIDDLE_NAME", correspondenceAddress.Rows[0]));
                                    lineOfData = ReplaceTag(lineOfData, "Corresp_LN", ObjectControllerHelper.GetString("LAST_NAME", correspondenceAddress.Rows[0]));
                                }
                                else
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYCorrespAddrOrg", "table-row");
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYCorrespAddrOrgInd", "none");
                                    lineOfData = ReplaceTag(lineOfData, "Corresp_NAME", ObjectControllerHelper.GetString("PRACTICE_NAME", correspondenceAddress.Rows[0]));
                                }
                                log.CreateLogEntry("Replace Tag   Correspondence Address  for type Details End" + regID, Logging.LogPriority.Information);
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_ADDRESS1", ObjectControllerHelper.GetString("ADDRESS1", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_ADDRESS2", ObjectControllerHelper.GetString("ADDRESS2", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_ADDRESS3", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_CITY", ObjectControllerHelper.GetString("CITY", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_STATE", ObjectControllerHelper.GetString("STATE", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_COUNTY", ObjectControllerHelper.GetString("CountyName", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_ZIP", ObjectControllerHelper.GetString("ZIP", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_ZIP_EXT", ObjectControllerHelper.GetString("EXT_ZIP", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_PHONE_NUMBER1", FormatPhone(ObjectControllerHelper.GetString("PHONE1", correspondenceAddress.Rows[0])));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_PHONE_EXT1", ObjectControllerHelper.GetString("PHONE1_EXT", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_PHONE_NUMBER2", ObjectControllerHelper.GetString("PHONE2", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_PHONE_EXT2", ObjectControllerHelper.GetString("PHONE2_EXT", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_FAX_NUMBER1", FormatPhone(ObjectControllerHelper.GetString("FAX1", correspondenceAddress.Rows[0])));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_FAX_NUMBER2", ObjectControllerHelper.GetString("FAX2", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_CONTACT_NAME", ObjectControllerHelper.GetString("CONTACT_NAME", correspondenceAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_EMAIL_ADDRESS1", ObjectControllerHelper.GetString("EMAIL1", correspondenceAddress.Rows[0]));
                                log.CreateLogEntry("Replace Tag   MAILTO Details End" + regID, Logging.LogPriority.Information);

                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_ADDRESS1", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_ADDRESS2", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_ADDRESS3", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_CITY", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_STATE", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_PHONE_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_PHONE_EXT1", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_PHONE_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_PHONE_EXT2", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_FAX_NUMBER1", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_FAX_NUMBER2", "");
                                lineOfData = ReplaceTag(lineOfData, "MAILTO_EMAIL_ADDRESS1", "");
                            }
                        }



                        if (isOtherAddressVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOtherAddr", "block");
                            if (otherAddress != null && otherAddress.Rows.Count > 0)
                            {
                                //Other Address Information
                                lineOfData = ReplaceTag(lineOfData, "OTHER_ADDRESS1", ObjectControllerHelper.GetString("ADDRESS1", otherAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "OTHER_ADDRESS2", ObjectControllerHelper.GetString("ADDRESS2", otherAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "OTHER_CITY", ObjectControllerHelper.GetString("CITY", otherAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "OTHER_STATE", ObjectControllerHelper.GetString("STATE", otherAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "OTHER_ZIP", ObjectControllerHelper.GetString("ZIP", otherAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "OTHER_EXT_ZIP", ObjectControllerHelper.GetString("EXT_ZIP", otherAddress.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "OTHER_PHONE_NUMBER", ObjectControllerHelper.GetString("PHONE1", otherAddress.Rows[0]));
                                log.CreateLogEntry("Replace Tag   Other Address Information End" + regID, Logging.LogPriority.Information);

                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "OTHER_ADDRESS1", "");
                                lineOfData = ReplaceTag(lineOfData, "OTHER_ADDRESS2", "");
                                lineOfData = ReplaceTag(lineOfData, "OTHER_CITY", "");
                                lineOfData = ReplaceTag(lineOfData, "OTHER_STATE", "");
                                lineOfData = ReplaceTag(lineOfData, "OTHER_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "OTHER_EXT_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "OTHER_PHONE_NUMBER", "");
                            }
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOtherAddr", "none");
                        }

                        //display settings
                        lineOfData = ReplaceTag(lineOfData, "DISPLAYOfficeH", "block");
                        if (entitytypeID == CON.ProviderCategoryTypeID.Individual)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOffice", "block");
                            //testing-lineOfData = ReplaceTag(lineOfData, "DISPLAYOffice", "block");
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOfficeInd", "block");
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOffice", "block");
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOfficeInd", "block");
                        }
                        log.CreateLogEntry("Replace Tag   display settings End" + regID, Logging.LogPriority.Information);


                        if (isOfficeVisible && officeAddress != null && officeAddress.Rows.Count > 0)
                        {
                            lineOfData = ReplaceTag(lineOfData, "disOfficeInformation", "block");
                            lineOfData = ReplaceTag(lineOfData, "disOrgInfo", "block");
                            log.CreateLogEntry("Replace Tag   CULTURAL_COMPETENCY Start" + regID, Logging.LogPriority.Information);
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOfficeH", "block");
                            //if (entitytypeID == CON.ProviderCategoryTypeID.Individual)
                            //{

                            string Competencys = "";
                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CULTURAL_COMPETENCY", officeTiming.Rows[0])))
                            {
                                string[] idarray = ObjectControllerHelper.GetString("CULTURAL_COMPETENCY", officeTiming.Rows[0]).Split(',');
                                log.CreateLogEntry("Replace Tag   CULTURAL_COMPETENCY Start foreach" + regID, Logging.LogPriority.Information);
                                foreach (var id in idarray)
                                {
                                    var Idval = Convert.ToInt32(id.Trim());
                                    var Competencystr = Competency.AsEnumerable().Where(r => r.Field<int>("Cultural_competencies_ID")
                                                                       == Idval).Select(r => r.Field<string>("DSC_Cultural_competencies")).FirstOrDefault(); ;

                                    Competencys = Competencys + (Competencys != "" ? ", " : "") + Competencystr;
                                }
                                log.CreateLogEntry("Replace Tag   CULTURAL_COMPETENCY end foreach" + regID, Logging.LogPriority.Information);

                            }
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_CULTURAL_COMPETENCIES", Competencys);
                            log.CreateLogEntry("Replace Tag   OFFICE_CULTURAL_COMPETENCIES end" + regID, Logging.LogPriority.Information);
                            string gender_of_patients = "";
                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("gender_of_patients", officeAddress.Rows[0])))
                            {
                                int genderid = ObjectControllerHelper.GetInt("gender_of_patients", officeAddress.Rows[0]);


                                gender_of_patients = GENDER.AsEnumerable().Where(r => r.Field<int>("provider_gender_id")
                                                                   == genderid).Select(r => r.Field<string>("provider_gender_name")).FirstOrDefault();
                            }
                            log.CreateLogEntry("Replace Tag Gender_of_patients end" + regID, Logging.LogPriority.Information);
                            string lang = "";
                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("STAFF_LANGUAGES_SPOKEN", officeTiming.Rows[0])))
                            {
                                string[] idarraylang = ObjectControllerHelper.GetString("STAFF_LANGUAGES_SPOKEN", officeTiming.Rows[0]).Split(',');
                                log.CreateLogEntry("Replace TagSTAFF_LANGUAGES_SPOKEN Start foreach" + regID, Logging.LogPriority.Information);
                                foreach (var id in idarraylang)
                                {
                                    var Idval = Convert.ToInt32(id.Trim());
                                    var language = languages.AsEnumerable().Where(r => r.Field<int>("Languages_Spoken_ID")
                                                                       == Idval).Select(r => r.Field<string>("DESC_Languages_Spoken")).FirstOrDefault(); ;

                                    lang = lang + (lang != "" ? ", " : "") + language;
                                }
                                log.CreateLogEntry("Replace TagSTAFF_LANGUAGES_SPOKEN End foreach" + regID, Logging.LogPriority.Information);
                            }
                            string trainings = "";
                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("SPECIALIZED_TRAINING", officeTiming.Rows[0])))
                            {
                                string[] traingidarray = ObjectControllerHelper.GetString("SPECIALIZED_TRAINING", officeTiming.Rows[0]).Split(',');
                                log.CreateLogEntry("Replace Tag SPECIALIZED_TRAINING Start foreach" + regID, Logging.LogPriority.Information);
                                foreach (var id in traingidarray)
                                {
                                    var Idval = Convert.ToInt32(id.Trim());
                                    var trainingstr = training.AsEnumerable().Where(r => r.Field<int>("Specialized_Training_ID")
                                                                       == Idval).Select(r => r.Field<string>("DSC_Specialized_Training")).FirstOrDefault(); ;

                                    trainings = trainings + (trainings != "" ? ", " : "") + trainingstr;
                                }
                                log.CreateLogEntry("Replace Tag SPECIALIZED_TRAINING End foreach" + regID, Logging.LogPriority.Information);
                            }
                            //ohpnm-15690
                            string CompetencysGrp = "";
                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CULTURAL_COMPETENCY", officeTimingGrp.Rows[0])))
                            {
                                string[] idarray = ObjectControllerHelper.GetString("CULTURAL_COMPETENCY", officeTimingGrp.Rows[0]).Split(',');
                                log.CreateLogEntry("Replace Tag   CULTURAL_COMPETENCY Start foreach" + regID, Logging.LogPriority.Information);
                                foreach (var id in idarray)
                                {
                                    var Idval = Convert.ToInt32(id.Trim());
                                    var Competencystr = Competency.AsEnumerable().Where(r => r.Field<int>("Cultural_competencies_ID")
                                                                       == Idval).Select(r => r.Field<string>("DSC_Cultural_competencies")).FirstOrDefault(); ;

                                    CompetencysGrp = CompetencysGrp + (CompetencysGrp != "" ? ", " : "") + Competencystr;
                                }
                                log.CreateLogEntry("Replace Tag   CULTURAL_COMPETENCY end foreach" + regID, Logging.LogPriority.Information);

                            }
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_CULTURAL_COMPETENCIES_GRP", CompetencysGrp);
                            log.CreateLogEntry("Replace Tag   OFFICE_CULTURAL_COMPETENCIES_GRP end" + regID, Logging.LogPriority.Information);

                            string langrp = "";
                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("STAFF_LANGUAGES_SPOKEN", officeTimingGrp.Rows[0])))
                            {
                                string[] idarraylang = ObjectControllerHelper.GetString("STAFF_LANGUAGES_SPOKEN", officeTimingGrp.Rows[0]).Split(',');
                                log.CreateLogEntry("Replace TagSTAFF_LANGUAGES_SPOKEN Start foreach" + regID, Logging.LogPriority.Information);
                                foreach (var id in idarraylang)
                                {
                                    var Idval = Convert.ToInt32(id.Trim());
                                    var language = languages.AsEnumerable().Where(r => r.Field<int>("Languages_Spoken_ID")
                                                                       == Idval).Select(r => r.Field<string>("DESC_Languages_Spoken")).FirstOrDefault(); ;

                                    langrp = langrp + (langrp != "" ? ", " : "") + language;
                                }
                                log.CreateLogEntry("Replace TagSTAFF_LANGUAGES_SPOKEN End foreach" + regID, Logging.LogPriority.Information);
                            }
                            string trainingsGrp = "";
                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("SPECIALIZED_TRAINING", officeTimingGrp.Rows[0])))
                            {
                                string[] traingidarrayGrp = ObjectControllerHelper.GetString("SPECIALIZED_TRAINING", officeTimingGrp.Rows[0]).Split(',');
                                log.CreateLogEntry("Replace Tag SPECIALIZED_TRAINING Start foreach" + regID, Logging.LogPriority.Information);
                                foreach (var id in traingidarrayGrp)
                                {
                                    var Idval = Convert.ToInt32(id.Trim());
                                    var trainingstr = training.AsEnumerable().Where(r => r.Field<int>("Specialized_Training_ID")
                                                                       == Idval).Select(r => r.Field<string>("DSC_Specialized_Training")).FirstOrDefault(); ;

                                    trainingsGrp = trainingsGrp + (trainingsGrp != "" ? ", " : "") + trainingstr;
                                }
                                log.CreateLogEntry("Replace Tag SPECIALIZED_TRAINING End foreach" + regID, Logging.LogPriority.Information);
                            }
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOfficeInd", "block");
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOffice", "block");
                            //    lineOfData = ReplaceTag(lineOfData, "OFFICE_CULTURAL_COMPETENCIES", ObjectControllerHelper.GetString("DSC_Cultural_competencies", officeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_LANGUAGES_SPOKEN", lang);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_SPECIALIZED_TRAINING", trainings);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_LANGUAGES_SPOKEN_GRP", langrp);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_SPECIALIZED_TRAINING_GRP", trainingsGrp);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_ACCEPT_NEW_PATIENTS", ObjectControllerHelper.GetBool("NEWPATIENT", officeAddress.Rows[0]) ? "Yes" : "No");
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_ACCEPT_NEW_PATIENTS_REFERRAL", ObjectControllerHelper.GetBool("REFFERAL", officeAddress.Rows[0]) ? "Yes" : "No");
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_YOUNGEST_PATIENTS_ACCEPT", ObjectControllerHelper.GetString("YOUNGEST_PATIENTS", officeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_OLDEST_PATIENTS_ACCEPT", ObjectControllerHelper.GetString("OLDEST_PATIENT", officeAddress.Rows[0]));
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_GENDER_PATIENT_ACCEPT", gender_of_patients);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_NEWBORN_ACCEPT", ObjectControllerHelper.GetBool("IS_NEW_BORN", officeAddress.Rows[0]) ? "Yes" : "No");
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_PREGNANT_WOMEN_ACCEPT", ObjectControllerHelper.GetBool("IS_PREGNANT", officeAddress.Rows[0]) ? "Yes" : "No");
                            log.CreateLogEntry("Replace Tag DISPLAYOfficeInd" + regID, Logging.LogPriority.Information);
                            if ((ObjectControllerHelper.GetInt("IsProviderDirectoryOptout", officeAddress.Rows[0])) == 1)
                            {
                                lineOfData = ReplaceTag(lineOfData, "OPT_out", "Checked");
                                log.CreateLogEntry("Replace Tag OPT_out" + regID, Logging.LogPriority.Information);
                            }
                            //}
                            //else
                            //{
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOffice", "block");
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOfficeInd", "block");
                            //Office Hours
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_WEBSITE", ObjectControllerHelper.GetString("WEBSITE", officeAddress.Rows[0]));
                            log.CreateLogEntry("Replace Tag OFFICE_WEBSITE" + regID, Logging.LogPriority.Information);

                            string monOfficeHours = "";
                            string tueOfficeHours = "";
                            string wedOfficeHours = "";
                            string thuOfficeHours = "";
                            string friOfficeHours = "";
                            string satOfficeHours = "";
                            string sunOfficeHours = "";

                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("MON_START_TIME", officeAddress.Rows[0])) || (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("MON_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("MON_OPEN_24_HRS", officeAddress.Rows[0])))
                            {

                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("MON_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("MON_OPEN_24_HRS", officeAddress.Rows[0]))
                                {
                                    monOfficeHours = "Open 24 Hours";
                                }
                                else
                                {
                                    monOfficeHours = ObjectControllerHelper.GetString("MON_START_TIME", officeAddress.Rows[0]) + " - " + ObjectControllerHelper.GetString("MON_END_TIME", officeAddress.Rows[0]);
                                }
                                log.CreateLogEntry("Replace Tag MON_START_TIME end" + regID, Logging.LogPriority.Information);
                            }

                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("TUE_START_TIME", officeAddress.Rows[0])) || (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("TUE_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("TUE_OPEN_24_HRS", officeAddress.Rows[0])))
                            {

                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("TUE_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("TUE_OPEN_24_HRS", officeAddress.Rows[0]))
                                {
                                    tueOfficeHours = "Open 24 Hours";
                                }
                                else
                                {
                                    tueOfficeHours = ObjectControllerHelper.GetString("TUE_START_TIME", officeAddress.Rows[0]) + " - " + ObjectControllerHelper.GetString("TUE_END_TIME", officeAddress.Rows[0]);
                                }
                                log.CreateLogEntry("Replace Tag TUE_START_TIME end" + regID, Logging.LogPriority.Information);
                            }

                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("WED_START_TIME", officeAddress.Rows[0])) || (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("WED_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("WED_OPEN_24_HRS", officeAddress.Rows[0])))
                            {

                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("WED_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("WED_OPEN_24_HRS", officeAddress.Rows[0]))
                                {
                                    wedOfficeHours = "Open 24 Hours";
                                }
                                else
                                {
                                    wedOfficeHours = ObjectControllerHelper.GetString("WED_START_TIME", officeAddress.Rows[0]) + " - " + ObjectControllerHelper.GetString("WED_END_TIME", officeAddress.Rows[0]);
                                }
                                log.CreateLogEntry("Replace Tag WED_START_TIME end" + regID, Logging.LogPriority.Information);
                            }

                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("THU_START_TIME", officeAddress.Rows[0])) || (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("THU_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("THU_OPEN_24_HRS", officeAddress.Rows[0])))
                            {

                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("THU_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("THU_OPEN_24_HRS", officeAddress.Rows[0]))
                                {
                                    thuOfficeHours = "Open 24 Hours";
                                }
                                else
                                {
                                    thuOfficeHours = ObjectControllerHelper.GetString("THU_START_TIME", officeAddress.Rows[0]) + " - " + ObjectControllerHelper.GetString("THU_END_TIME", officeAddress.Rows[0]);
                                }
                                log.CreateLogEntry("Replace Tag THU_START_TIME end" + regID, Logging.LogPriority.Information);
                            }

                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("FRI_START_TIME", officeAddress.Rows[0])) || (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("FRI_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("FRI_OPEN_24_HRS", officeAddress.Rows[0])))
                            {

                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("FRI_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("FRI_OPEN_24_HRS", officeAddress.Rows[0]))
                                {
                                    friOfficeHours = "Open 24 Hours";
                                }
                                else
                                {
                                    friOfficeHours = ObjectControllerHelper.GetString("FRI_START_TIME", officeAddress.Rows[0]) + " - " + ObjectControllerHelper.GetString("FRI_END_TIME", officeAddress.Rows[0]);
                                }
                                log.CreateLogEntry("Replace Tag FRI_START_TIME end" + regID, Logging.LogPriority.Information);
                            }

                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("SAT_START_TIME", officeAddress.Rows[0])) || (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("SAT_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("SAT_OPEN_24_HRS", officeAddress.Rows[0])))
                            {

                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("SAT_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("SAT_OPEN_24_HRS", officeAddress.Rows[0]))
                                {
                                    satOfficeHours = "Open 24 Hours";
                                }
                                else
                                {
                                    satOfficeHours = ObjectControllerHelper.GetString("SAT_START_TIME", officeAddress.Rows[0]) + " - " + ObjectControllerHelper.GetString("SAT_END_TIME", officeAddress.Rows[0]);
                                }
                                log.CreateLogEntry("Replace Tag SAT_START_TIME end" + regID, Logging.LogPriority.Information);
                            }

                            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("SUN_START_TIME", officeAddress.Rows[0])) || (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("SUN_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("SUN_OPEN_24_HRS", officeAddress.Rows[0])))
                            {

                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("SUN_OPEN_24_HRS", officeAddress.Rows[0])) && ObjectControllerHelper.GetBool("SUN_OPEN_24_HRS", officeAddress.Rows[0]))
                                {
                                    sunOfficeHours = "Open 24 Hours";
                                }
                                else
                                {
                                    sunOfficeHours = ObjectControllerHelper.GetString("SUN_START_TIME", officeAddress.Rows[0]) + " - " + ObjectControllerHelper.GetString("SUN_END_TIME", officeAddress.Rows[0]);
                                }
                                log.CreateLogEntry("Replace Tag SUN_START_TIME end" + regID, Logging.LogPriority.Information);
                            }

                            lineOfData = ReplaceTag(lineOfData, "OFFICE_Mon", monOfficeHours);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_Tue", tueOfficeHours);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_Wed", wedOfficeHours);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_Thu", thuOfficeHours);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_Fri", friOfficeHours);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_Sat", satOfficeHours);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_Sun", sunOfficeHours);
                            log.CreateLogEntry("Replace Tag OFFICE all day end" + regID, Logging.LogPriority.Information);
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_TOTAL", ObjectControllerHelper.GetString("OFFICE_TOTAL", dataRow).ToString());
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_telephone", ObjectControllerHelper.GetBool("TELEPHONE", officeAddress.Rows[0]) ? "Yes" : "No");
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_TRANSPORT", ObjectControllerHelper.GetBool("TRANSPORT", officeAddress.Rows[0]) ? "Yes" : "No");
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_Electronic", ObjectControllerHelper.GetBool("EBILLING", officeAddress.Rows[0]) ? "Yes" : "No");
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_TDD", ObjectControllerHelper.GetBool("TDD", officeAddress.Rows[0]) ? "Yes" : "No");
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_ASL", ObjectControllerHelper.GetBool("OFFICE_ASLOFFERED", officeAddress.Rows[0]) ? "Yes" : "No");
                            lineOfData = ReplaceTag(lineOfData, "OFFICE_TRANSPORT", ObjectControllerHelper.GetBool("TRANSPORT", officeAddress.Rows[0]) ? "Yes" : "No");
                            log.CreateLogEntry("Replace Tag OFFICE all Details end" + regID, Logging.LogPriority.Information);
                            if (officeTiming != null && officeTiming.Rows.Count > 0 && !string.IsNullOrEmpty(ObjectControllerHelper.GetString("TRANSLATIONsERVICETYPE", officeTiming.Rows[0])))
                            {
                                string[] TranslationServiceTypeIds = ObjectControllerHelper.GetString("TRANSLATIONsERVICETYPE", officeTiming.Rows[0]).Split(',');
                                log.CreateLogEntry("Replace Tag TRANSLATIONsERVICETYPE Details Start for each" + regID, Logging.LogPriority.Information);
                                foreach (var TranslationServiceTypeId in TranslationServiceTypeIds)
                                {
                                    if (TranslationServiceTypeId.ToString().Trim() == "1")
                                        lineOfData = ReplaceTag(lineOfData, "LLine", "checked");
                                    else
                                        lineOfData = ReplaceTag(lineOfData, "Translation", "checked");

                                }
                                log.CreateLogEntry("Replace Tag TRANSLATIONsERVICETYPE Details END for each" + regID, Logging.LogPriority.Information);
                            }
                            if (officeTiming != null && officeTiming.Rows.Count > 0 && !string.IsNullOrEmpty(ObjectControllerHelper.GetString("STAFF_LANGUAGES_SPOKEN", officeTiming.Rows[0])))
                            {
                                string lang1 = "";
                                string[] idarray = ObjectControllerHelper.GetString("STAFF_LANGUAGES_SPOKEN", officeTiming.Rows[0]).Split(',');
                                log.CreateLogEntry("Replace Tag STAFF_LANGUAGES_SPOKEN Details start for each" + regID, Logging.LogPriority.Information);
                                foreach (var id in idarray)
                                {
                                    var Idval = Convert.ToInt32(id.Trim());
                                    var language = languages.AsEnumerable().Where(r => r.Field<int>("Languages_Spoken_ID")
                                                                       == Idval).Select(r => r.Field<string>("DESC_Languages_Spoken")).FirstOrDefault(); ;

                                    lang1 = lang1 + (lang1 != "" ? ", " : "") + language;
                                }
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_Language", lang1);
                                log.CreateLogEntry("Replace Tag STAFF_LANGUAGES_SPOKEN Details end for each" + regID, Logging.LogPriority.Information);
                            }
                            if (officeTiming != null && officeTiming.Rows.Count > 0 && !string.IsNullOrEmpty(ObjectControllerHelper.GetString("ADA_ACCOMODATIONS", officeTiming.Rows[0])))
                            {
                                string lang2 = "";
                                string[] accomodationIds = ObjectControllerHelper.GetString("ADA_ACCOMODATIONS", officeTiming.Rows[0]).Split(',');
                                log.CreateLogEntry("Replace Tag ADA_ACCOMODATIONS Details Start for each" + regID, Logging.LogPriority.Information);
                                foreach (var accomodationId in accomodationIds)
                                {
                                    var Idval = Convert.ToInt32(accomodationId.Trim());
                                    var addressTbl = accomodations.AsEnumerable().Where(r => r.Field<int>("Office_Accommodations_ID")
                                    == Idval).Select(r => r.Field<string>("DSC_Office_Accommodations")).FirstOrDefault(); ;
                                    lang2 = lang2 + (lang2 != "" ? ", " : "") + addressTbl;
                                }
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_ADA", lang2);
                                log.CreateLogEntry("Replace Tag ADA_ACCOMODATIONS Details End for each" + regID, Logging.LogPriority.Information);
                            }
                            //}
                        }
                        else
                        {
                            if (isOfficeVisible)
                            {
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYOffice", "block");
                                //Office Hours
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_WEBSITE", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_Mon", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_Tue", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_Wed", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_Thu", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_Fri", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_Sat", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_Sun", "");
                                log.CreateLogEntry("Replace Tag DISPLAYOffice Details End" + regID, Logging.LogPriority.Information);
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_TOTAL", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_telephone", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_TRANSPORT", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_Electronic", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_TDD", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_ASL", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_ADA", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_Language", "");
                                lineOfData = ReplaceTag(lineOfData, "OFFICE_TRANSPORT", "");
                            }
                            else
                                lineOfData = ReplaceTag(lineOfData, "DISPLAYOffice", "block");
                        }
                        // Affiliations

                        lineOfData = ReplaceTag(lineOfData, "DISPLAYAFFILIATIONS", "block");
                        lineOfData = !string.IsNullOrEmpty(affiliations)
                            ? ReplaceTag(lineOfData, "AFFILIATION_LIST", affiliations)
                            : ReplaceTag(lineOfData, "AFFILIATION_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Affiliation records found.")));
                        lineOfData = !string.IsNullOrEmpty(formattedaffiliations)
                                ? ReplaceTag(lineOfData, "AFFILIATION_LIST_NEW", formattedaffiliations)
                                : ReplaceTag(lineOfData, "AFFILIATION_LIST_NEW", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Affiliation records found.")));

                        log.CreateLogEntry("Replace Tag Affiliations Details End" + regID, Logging.LogPriority.Information);
                        //Educations
                        if (isEducationVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYEDUCATION", "block");
                            lineOfData = !string.IsNullOrEmpty(educations)
                                ? ReplaceTag(lineOfData, "EDUCATION_LIST", educations)
                                : ReplaceTag(lineOfData, "EDUCATION_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Education records found.")));
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYEDUCATION", "none");
                        }
                        log.CreateLogEntry("Replace Tag Educations Details Start" + regID, Logging.LogPriority.Information);
                        if (isAgreementsVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYAGREEMENT", "block");
                            lineOfData = !string.IsNullOrEmpty(submittedAgreements)
                                ? ReplaceTag(lineOfData, "Provider_Agreement_String", submittedAgreements)
                                : ReplaceTag(lineOfData, "Provider_Agreement_String", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Agreement records found.")));
                            lineOfData = !string.IsNullOrEmpty(submittedUpdateAgreements)
                                ? ReplaceTag(lineOfData, "Provider_Update_Agreement_String", submittedUpdateAgreements)
                                : ReplaceTag(lineOfData, "Provider_Update_Agreement_String", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Agreement records found.")));
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYAGREEMENT", "none");
                        }
                        log.CreateLogEntry("Replace Tag Educations Details End" + regID, Logging.LogPriority.Information);
                        //workflows
                        if (isWorkflowStepsVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYWORKFLOW", "block");
                            lineOfData = !string.IsNullOrEmpty(workflows)
                                ? ReplaceTag(lineOfData, "WORKFLOW_LIST", workflows)
                                : ReplaceTag(lineOfData, "WORKFLOW_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Workflow records found.")));
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYWORKFLOW", "none");
                        }
                        log.CreateLogEntry("Replace Tag workflows Details End" + regID, Logging.LogPriority.Information);
                        // Provider Identifying Info
                        //lineOfData = ReplaceTag(lineOfData, "PROVIDER_IDENT_LIST", formattedProviderIdentifiers);

                        // Owner Information
                        if (isOwnerVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOwnerInfo", "block");
                            lineOfData = ReplaceTag(lineOfData, "OWNER_LIST", formattedOwners);
                            lineOfData = ReplaceTag(lineOfData, "REAL_ESTATEOWNER_LIST", formattedRealOwners);
                            lineOfData = ReplaceTag(lineOfData, "ADDITIONAL_DISCLOUSER", formattedadditonaldiscluser);
                            StringBuilder sbowner = new StringBuilder();
                            if (dtOwnerQuestion != null)
                            {
                                sbowner.AppendLine("<table style=\"padding: 3px; width: 100 % \">");
                                sbowner.AppendLine("<tr>");
                                foreach (DataRow dr in dtOwnerQuestion.Rows)
                                {
                                    if (dr["QUESTION_TYPE_ID"].ToString() == "Q01" || dr["QUESTION_TYPE_ID"].ToString() == "Q02" || dr["QUESTION_TYPE_ID"].ToString() == "Q04" || dr["QUESTION_TYPE_ID"].ToString() == "Q05" || dr["QUESTION_TYPE_ID"].ToString() == "Q06" || dr["QUESTION_TYPE_ID"].ToString() == "Q07" || dr["QUESTION_TYPE_ID"].ToString() == "Q09" || dr["QUESTION_TYPE_ID"].ToString() == "Q13")
                                    {
                                        sbowner.AppendLine("<tr><td>");
                                        string response = dr["RESPONSE"].ToString().ToLower();
                                        string yesValue = response == "true" ? "Checked" : "Unchecked";
                                        string noValue = response == "false" ? "Checked" : "Unchecked";
                                        sbowner.AppendLine("<p style=\"text-align:left;font-size:10pt; font-weight:normal\">" + dr["QUESTION_TEXT"].ToString() + "\"");
                                        sbowner.AppendLine(string.Format("</br><input type=\"radio\" name=\"{0}\" {1} />Yes<input type=\"radio\" name=\"{0}\" {2} />No</br>", dr["QUESTION_TYPE_ID"].ToString(), yesValue, noValue));

                                        //lineOfData = ReplaceTag(lineOfData, "Q09", ObjectControllerHelper.GetString("QUESTION_TEXT", dr));
                                        sbowner.AppendLine("</td></tr>");
                                    }

                                }
                                sbowner.AppendLine("</tr></table>");
                                lineOfData = ReplaceTag(lineOfData, "Owner_Questions", sbowner.ToString());
                            }
                            ////lineOfData = !string.IsNullOrEmpty(workflows)
                            // ? ReplaceTag(lineOfData, "OWNER_LIST", formattedOwners)
                            //   : ReplaceTag(lineOfData, "OWNER_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Owner //Information records found.")));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYOwnerInfo", "none");
                        log.CreateLogEntry("Replace Tag  Provider Identifying Info Details End" + regID, Logging.LogPriority.Information);
                        // Owner Additional Addresses
                        //lineOfData = ReplaceTag(lineOfData, "ADDL_ADDRESSES_LIST", formattedAddlAddresses);

                        // EFT Banking
                        if (isACHAuthorizationVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYEftBanking", "block");
                            StringBuilder sbEft = new StringBuilder();
                            sbEft.AppendLine("<table style=\"padding: 3px; width: 100 % \">");
                            sbEft.AppendLine("<tr><td>");
                            string response = Convert.ToString(dataRow["ACH_INTEND_TO_RECEIVE_MCC"]).ToLower();
                            string yesValue = response == "true" ? "Checked" : "Unchecked";
                            string noValue = response == "false" ? "Checked" : "Unchecked";
                            sbEft.AppendLine(string.Format("</br><input type=\"radio\" name=\"{0}\" {1} />Yes<input type=\"radio\" name=\"{0}\" {2} />No</br>", "achIntendToReceive", yesValue, noValue));
                            sbEft.AppendLine("</td></tr></table>");
                            lineOfData = ReplaceTag(lineOfData, "Ach_Intend_To_Receive_MCC", sbEft.ToString());
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYEftBanking", "none");

                        // Required Documents
                        if (isOtherDocumentsVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "REQUIREDDOCUMENTS", "block");

                            lineOfData = !string.IsNullOrEmpty(formattedRequriedDocuments)
                                    ? ReplaceTag(lineOfData, "REQUIRED_DOCUMENTS", formattedRequriedDocuments)
                                    : ReplaceTag(lineOfData, "REQUIRED_DOCUMENTS", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Required Documents records found.")));
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "REQUIREDDOCUMENTS", "none");
                        }
                        log.CreateLogEntry("Replace Tag Required Documents Details End" + regID, Logging.LogPriority.Information);
                        // Substitute W9 Form
                        if (isW9visible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYW9", "block");
                            lineOfData = ReplaceTag(lineOfData, "TAX_ENTITY_TYPE", ObjectControllerHelper.GetString("TAX_ENTITY_TYPE", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "TAX_ENTITY_STATE_ID", ObjectControllerHelper.GetString("TAX_ENTITY_STATE_ID", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "PRACTICE_TYPE_NAME", ObjectControllerHelper.GetString("PRACTICE_TYPE_NAME", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "W9_BusName", ObjectControllerHelper.GetString("NAME", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "W9_EIN", ObjectControllerHelper.GetString("TAX_ID", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "lblSSN",
                            ObjectControllerHelper.GetString("TAX_ID_TYPE_ID", dataRow) == CON.TaxIDType.SSN.ToString() ? "Individual Name" : "Legal Business Name");
                            lineOfData = ReplaceTag(lineOfData, "lblName",
                            ObjectControllerHelper.GetString("TAX_ID_TYPE_ID", dataRow) == CON.TaxIDType.SSN.ToString() ? "SSN" : "EIN");
                            //lineOfData = ReplaceTag(lineOfData, "W9_Category", ObjectControllerHelper.GetString("PROVIDER_TYPE_CATEGORY_NAME", dataRow));
                            StringBuilder sbTax = new StringBuilder();
                            if (W9TaxInfo != null)
                            {
                                if (W9TaxInfoSelect != null && W9TaxInfoSelect.Rows.Count > 0)
                                {
                                    if (!String.IsNullOrEmpty(Convert.ToString(W9TaxInfoSelect.Rows[0]["INDICATE_FORM"])))
                                    {
                                        String strw9 = Convert.ToString(W9TaxInfoSelect.Rows[0]["INDICATE_FORM"]);
                                        if (strw9 == "W9")
                                        {
                                            lineOfData = ReplaceTag(lineOfData, "W9_UploadingYES", "Checked");
                                        }
                                        else if (strw9 == "Form 147")
                                        {
                                            lineOfData = ReplaceTag(lineOfData, "W9_UploadingNO", "Checked");
                                        }
                                    }
                                }

                                foreach (DataRow dr in W9TaxInfo.Rows)
                                {
                                    int tax_id = 0;
                                    tax_id = W9TaxInfoSelect.AsEnumerable().Where(r => r.Field<int>("TAX_FORM_ID")
                                                                       == Convert.ToInt32(dr["TAX_ENTITY_TYPE_ID"])).Select(r => r.Field<int>("TAX_FORM_ID")).FirstOrDefault();
                                    if (tax_id > 0)
                                    {
                                        sbTax.AppendLine(string.Format("</br><input type=\"radio\" name=\"{0}\" {1} />", dr["TAX_ENTITY_TYPE"].ToString(), "Checked"));
                                        sbTax.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">" + dr["TAX_ENTITY_TYPE"].ToString() + "</span>");
                                    }
                                    else
                                    {
                                        sbTax.AppendLine(string.Format("</br><input type=\"radio\" name=\"{0}\" {1} />", dr["TAX_ENTITY_TYPE"].ToString(), "Unchecked"));
                                        sbTax.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">" + dr["TAX_ENTITY_TYPE"].ToString() + "</span>");
                                    }
                                }
                                lineOfData = ReplaceTag(lineOfData, "W9_TYPE_NAME", sbTax.ToString());
                            }
                            if (w9Address.Rows.Count > 0)
                            {

                                lineOfData = ReplaceTag(lineOfData, "DISPLAYW9Addr", "block");
                                //W9 Address Information
                                lineOfData = ReplaceTag(lineOfData, "W9_ADDRESS1", ObjectControllerHelper.GetString("ADDRESS1", w9Address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "W9_ADDRESS2", ObjectControllerHelper.GetString("ADDRESS2", w9Address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "W9_CITY", ObjectControllerHelper.GetString("CITY", w9Address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "W9_STATE", ObjectControllerHelper.GetString("STATE", w9Address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "W9_ZIP", ObjectControllerHelper.GetString("ZIP", w9Address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "W9_EXT_ZIP", ObjectControllerHelper.GetString("EXT_ZIP", w9Address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "W9_PHONE_NUMBER", ObjectControllerHelper.GetString("PHONE1", w9Address.Rows[0]));
                                lineOfData = ReplaceTag(lineOfData, "W9_COMMENTS", string.Empty);

                            }
                            else
                            {

                                lineOfData = ReplaceTag(lineOfData, "DISPLAYW9Addr", "block");
                                //W9 Address Information
                                lineOfData = ReplaceTag(lineOfData, "W9_ADDRESS1", "");
                                lineOfData = ReplaceTag(lineOfData, "W9_ADDRESS2", "");
                                lineOfData = ReplaceTag(lineOfData, "W9_ADDRESS2", "");
                                lineOfData = ReplaceTag(lineOfData, "W9_CITY", "");
                                lineOfData = ReplaceTag(lineOfData, "W9_STATE", "");
                                lineOfData = ReplaceTag(lineOfData, "W9_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "W9_EXT_ZIP", "");
                                lineOfData = ReplaceTag(lineOfData, "W9_PHONE_NUMBER", "");
                                lineOfData = ReplaceTag(lineOfData, "W9_COMMENTS", "No W9 Address information found.");
                            }
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYW9", "none");
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYW9Addr", "none");
                        }
                        log.CreateLogEntry("Replace TagSubstitute W9 Form Details End" + regID, Logging.LogPriority.Information);

                        // Substitute W4 Form
                        //if (isW4visible)
                        //{
                        //    lineOfData = ReplaceTag(lineOfData, "DISPLAYW4", "block");

                        //    lineOfData = !string.IsNullOrEmpty(ObjectControllerHelper.GetString("ISEXEMPT", dataRow))
                        //        ? ReplaceTag(lineOfData, "W4ISEXEMPT", ObjectControllerHelper.GetString("ISEXEMPT", dataRow) == "Y" ? "Checked" : "Unchecked")
                        //        : ReplaceTag(lineOfData, "W4ISEXEMPT", string.Empty);
                        //    lineOfData = !string.IsNullOrEmpty(ObjectControllerHelper.GetString("last_Name_CHANGED", dataRow))
                        //        ? ReplaceTag(lineOfData, "W4last_Name_CHANGED", ObjectControllerHelper.GetString("last_Name_CHANGED", dataRow) == "Y" ? "Checked" : "Unchecked")
                        //        : ReplaceTag(lineOfData, "W4last_Name_CHANGED", string.Empty);

                        //    int maritalStatus = ObjectControllerHelper.GetInt("MARITAL_STATUS_ID", dataRow);
                        //    switch (maritalStatus)
                        //    {
                        //        case 1:
                        //            lineOfData = ReplaceTag(lineOfData, "SINGLE", "Checked");
                        //            break;
                        //        case 2:
                        //            lineOfData = ReplaceTag(lineOfData, "MARRIED", "Checked");
                        //            break;
                        //        case 3:
                        //            lineOfData = ReplaceTag(lineOfData, "MARRIEDWITHHOLD", "Checked");
                        //            break;
                        //    }

                        //    lineOfData = ReplaceTag(lineOfData, "W4NAME", ObjectControllerHelper.GetString("NAME", dataRow));
                        //    lineOfData = ReplaceTag(lineOfData, "W4DBA", ObjectControllerHelper.GetString("DBA", dataRow));
                        //    lineOfData = ReplaceTag(lineOfData, "W4TAX_ID", ObjectControllerHelper.GetString("TAX_ID", dataRow));
                        //    lineOfData = ReplaceTag(lineOfData, "W4CLAIM_NOOF_ALLOWANCES", ObjectControllerHelper.GetString("CLAIM_NOOF_ALLOWANCES", dataRow));
                        //    lineOfData = ReplaceTag(lineOfData, "W4ADDITIONAL_AMOUNT", ObjectControllerHelper.GetString("ADDITIONAL_AMOUNT", dataRow));
                        //}
                        //else
                        //{
                        //    lineOfData = ReplaceTag(lineOfData, "DISPLAYW4", "none");
                        //}

                        // ACH Authorization
                        //code checking if the registration has reliacard or banking info

                        if (string.IsNullOrEmpty(IsDirectOrReliaCard) || IsDirectOrReliaCard == CON.WaiverPaymentType.DirectDeposit)
                        {
                            string IsAttest = ObjectControllerHelper.GetString("ACH_IS_BANK_OUTSIDE_US", dataRow);
                            lineOfData = ReplaceTag(lineOfData, "displayBankingInfo", "block");
                            lineOfData = ReplaceTag(lineOfData, "displayReliacard", "none");
                            //string.IsNullOrEmpty(ObjectControllerHelper.GetString("ACH_BANK_NAME", dataRow)) ? "Checked" : - removed because ACH_BANK_OUTSIDE_US is not saved
                            lineOfData = ReplaceTag(lineOfData, "ACH_BANK_OUTSIDE_US", string.IsNullOrEmpty(IsAttest) || IsAttest == "False" ? "Unchecked" : "Checked");
                            lineOfData = ReplaceTag(lineOfData, "ACH_TradingPartnerID", ObjectControllerHelper.GetString("ACH_TradingPartnerID", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_BANK_NAME", ObjectControllerHelper.GetString("ACH_BANK_NAME", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_BRANCH", ObjectControllerHelper.GetString("ACH_BRANCH", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_CITY", ObjectControllerHelper.GetString("ACH_CITY", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_STATE", ObjectControllerHelper.GetString("ACH_STATE", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_ZipCode", ObjectControllerHelper.GetString("ACH_ZipCode", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_ZipExt", ObjectControllerHelper.GetString("ACH_ZipExt", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_BANK_CONTACT_PERSON", ObjectControllerHelper.GetString("ACH_BANK_CONTACT_PERSON", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_PHONE", ObjectControllerHelper.GetString("ACH_PHONE", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_PHONE_EXT", ObjectControllerHelper.GetString("ACH_PHONE_EXT", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_ABA_NUMBER", ObjectControllerHelper.GetString("ACH_ABA_NUMBER", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_ACCOUNT_NUMBER", ObjectControllerHelper.GetString("ACH_ACCOUNT_NUMBER", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_ACCOUNT_TYPE", ObjectControllerHelper.GetString("ACH_ACCOUNT_TYPE", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_EFT_TYPE_NAME", ObjectControllerHelper.GetString("ACH_EFT_TYPE_NAME", dataRow));
                            if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("ACH_EFT_START_DATE", dataRow)))
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_EFT_START_DATE", "");
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_EFT_START_DATE", ObjectControllerHelper.GetDateTime("ACH_EFT_START_DATE", dataRow).ToString("MM/dd/yyyy"));
                            }
                            if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("ACH_EFT_END_DATE", dataRow)))
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_EFT_END_DATE", "");
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_EFT_END_DATE", ObjectControllerHelper.GetDateTime("ACH_EFT_END_DATE", dataRow).ToString("MM/dd/yyyy"));
                            }
                            log.CreateLogEntry("Replace Tag ACH_EFT_END_DATE Details End" + regID, Logging.LogPriority.Information);
                            lineOfData = ReplaceTag(lineOfData, "ACH_TITLE_OF_PERSON_SUBMITTIMG", ObjectControllerHelper.GetString("ACH_TITLE_OF_PERSON_SUBMITTIMG", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACH_ACCOUNT_TYPE_ENTITY_NAME", ObjectControllerHelper.GetString("ACH_ACCOUNT_TYPE_ENTITY_NAME", dataRow));
                            lineOfData = ReplaceTag(lineOfData, "ACHchecked", string.IsNullOrEmpty(achAttest) ? "Unchecked" : "Checked");
                        }
                        else if (IsDirectOrReliaCard == CON.WaiverPaymentType.ReliaCard)
                        {
                            log.CreateLogEntry("Replace Tag displayBankingInfo Start" + regID, Logging.LogPriority.Information);
                            lineOfData = ReplaceTag(lineOfData, "displayBankingInfo", "none");
                            lineOfData = ReplaceTag(lineOfData, "displayReliacard", "block");
                            if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("reliaLAST_NAME", dataRow)))
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_LASTNAME", "");
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_LASTNAME", ObjectControllerHelper.GetString("reliaLAST_NAME", dataRow));
                            }
                            if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("reliaFIRST_NAME", dataRow)))
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_FIRSTNAME", "");
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_FIRSTNAME", ObjectControllerHelper.GetString("reliaFIRST_NAME", dataRow));
                            }
                            if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("reliaMIDDLE_INITIAL", dataRow)))
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_MIDDLENAME", "");
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_MIDDLENAME", ObjectControllerHelper.GetString("reliaMIDDLE_INITIAL", dataRow));
                            }
                            if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("reliaSTREET", dataRow)))
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_STREET", "");
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_STREET", ObjectControllerHelper.GetString("reliaSTREET", dataRow));
                            }
                            if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("reliaCITY", dataRow)))
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_CITY", "");
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_CITY", ObjectControllerHelper.GetString("reliaCITY", dataRow));
                            }
                            if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("reliaSTATE", dataRow)))
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_STATE", "");
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_STATE", ObjectControllerHelper.GetString("reliaSTATE", dataRow));
                            }
                            if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("reliaZIPCODE", dataRow)))
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_ZIP", "");
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "ACH_RELIACARD_ZIP", ObjectControllerHelper.GetString("reliaZIPCODE", dataRow));
                            }
                            lineOfData = ReplaceTag(lineOfData, "ACHchecked", string.IsNullOrEmpty(achAttest) ? "Unchecked" : "Checked");
                            log.CreateLogEntry("Replace Tag displayBankingInfo End" + regID, Logging.LogPriority.Information);

                        }
                        //Application Fee
                        if (isApplicationVisible)
                        {
                            if (dtApplicationFee.Rows != null && dtApplicationFee.Rows.Count > 0)
                            {
                                DataRow dataRowApp = dtApplicationFee.Rows[0];
                                lineOfData = ReplaceTag(lineOfData, "displayApplicationFee", "block");
                                string Status_ID = ObjectControllerHelper.GetString("APPLICATION_FEE_STATUS_ID", dataRowApp);
                                string Status_Name = "";
                                log.CreateLogEntry("Replace Tag DisplayApplicationFee Start" + regID, Logging.LogPriority.Information);
                                if (Status_ID == CON.ApplicationFeePaymentStatus.Pending)
                                {
                                    Status_Name = CON.ApplicationFeePaymentStatusName.Pending;
                                }
                                else if (Status_ID == CON.ApplicationFeePaymentStatus.Paid)
                                {
                                    Status_Name = CON.ApplicationFeePaymentStatusName.Paid;
                                }
                                else if (Status_ID == CON.ApplicationFeePaymentStatus.Waived)
                                {
                                    Status_Name = CON.ApplicationFeePaymentStatusName.Waived;
                                }
                                log.CreateLogEntry("Replace Tag DisplayApplicationFee End" + regID, Logging.LogPriority.Information);
                                if (dtApplicationFeeAmount.Rows.Count > 0)
                                {
                                    lineOfData = ReplaceTag(lineOfData, "APP_FEE_AMOUNT", "$" + dtApplicationFeeAmount.Rows[0]["FEE_AMOUNT"].ToString());
                                    lineOfData = ReplaceTag(lineOfData, "APP_FEE_STATUS", Status_Name);
                                    if (!string.IsNullOrEmpty(dtApplicationFeeAmount.Rows[0]["FEE_AMOUNT"].ToString()))
                                    {
                                        StringBuilder sbFeeType = new StringBuilder();
                                        sbFeeType.AppendLine(string.Format("</br>" + "$" + dtApplicationFeeAmount.Rows[0]["FEE_AMOUNT"].ToString()));
                                        lineOfData = ReplaceTag(lineOfData, "APP_FEE_AMOUNTSTRING", sbFeeType.ToString());
                                    }
                                    log.CreateLogEntry("Replace Tag APP_FEE_AMOUNT End" + regID, Logging.LogPriority.Information);
                                }
                                if (dtApplicationPaymentType.Rows.Count > 0)
                                {
                                    StringBuilder sbPatType = new StringBuilder();
                                    log.CreateLogEntry("Replace Tag DtApplicationPaymentType Start foreach" + regID
                                            + " and Payment typt id=" + dtApplicationFee.Rows[0]["PAYMENT_TYPE_ID"].ToString(), Logging.LogPriority.Information);
                                    if (ObjectControllerHelper.HasRows(dtApplicationFee))
                                    {
                                        if (!string.IsNullOrEmpty(dtApplicationFee.Rows[0]["PAYMENT_TYPE_ID"].ToString())
                                            || !string.IsNullOrWhiteSpace(dtApplicationFee.Rows[0]["PAYMENT_TYPE_ID"].ToString()))
                                        {
                                            foreach (DataRow dr in dtApplicationPaymentType.Rows)
                                            {
                                                int pay_id = 0;
                                                pay_id = dtApplicationFee.AsEnumerable().Where(r => r.Field<int?>("PAYMENT_TYPE_ID")
                                                                                   == Convert.ToInt32(dr["PAYMENT_TYPE_ID"])).Select(r => r.Field<int>("PAYMENT_TYPE_ID")).FirstOrDefault();
                                                if (pay_id > 0)
                                                {
                                                    sbPatType.AppendLine(string.Format("</br><input type=\"radio\" name=\"{0}\" {1} />", dr["PAYMENT_TYPE_NAME"].ToString(), "Checked"));
                                                    sbPatType.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">" + dr["PAYMENT_TYPE_NAME"].ToString() + "</span>");
                                                }
                                                else
                                                {
                                                    sbPatType.AppendLine(string.Format("</br><input type=\"radio\" name=\"{0}\" {1} />", dr["PAYMENT_TYPE_NAME"].ToString(), "Unchecked"));
                                                    sbPatType.AppendLine("<span   class=\"tooltipNew\" style=\"margin-right:10px\">" + dr["PAYMENT_TYPE_NAME"].ToString() + "</span>");
                                                }
                                            }
                                        }
                                    }
                                    lineOfData = ReplaceTag(lineOfData, "APP_FEE_PAYMENT_TYPE", sbPatType.ToString());
                                    log.CreateLogEntry("Replace Tag DtApplicationPaymentType End foreach" + regID, Logging.LogPriority.Information);
                                }
                                if (!string.IsNullOrEmpty(dtApplicationFee.Rows[0]["APPLICATION_FEE_WAIVER_REASON_NAME"].ToString()))
                                {
                                    StringBuilder sWreason = new StringBuilder();
                                    sWreason.AppendLine(string.Format(dtApplicationFee.Rows[0]["APPLICATION_FEE_WAIVER_REASON_NAME"].ToString()));
                                    lineOfData = ReplaceTag(lineOfData, "APP_FEE_WAIVER_REASONSTRING", sWreason.ToString());
                                    log.CreateLogEntry("Replace Tag APPLICATION_FEE_WAIVER_REASON_NAME End" + regID, Logging.LogPriority.Information);
                                }
                                if (!string.IsNullOrEmpty(dtApplicationFee.Rows[0]["COMMENT"].ToString()))
                                {
                                    StringBuilder sbcomment = new StringBuilder();
                                    sbcomment.AppendLine(string.Format(dtApplicationFee.Rows[0]["COMMENT"].ToString()));
                                    lineOfData = ReplaceTag(lineOfData, "APP_FEE_COMMENT", sbcomment.ToString());
                                    log.CreateLogEntry("Replace Tag APP_FEE_COMMENT End" + regID, Logging.LogPriority.Information);
                                }
                                if (dtApplicationFee.Rows[0]["PAYMENT_TYPE_ID"].ToString() == CON.ApplicationFeePaymentTypeID.CreditCard)
                                {
                                    lineOfData = ReplaceTag(lineOfData, "displayAppFeeAmount", "none");
                                    lineOfData = ReplaceTag(lineOfData, "displayAppFeeComments", "block;");
                                    lineOfData = ReplaceTag(lineOfData, "displayAppFeeWaiverReason", "none");
                                    lineOfData = ReplaceTag(lineOfData, "APP_FEE_PAYMENT_TYPE", CON.ApplicationFeePaymentType.PayByeCheck);
                                    log.CreateLogEntry("Replace Tag DisplayAppFeeWaiverReason End" + regID, Logging.LogPriority.Information);
                                }
                                else if (dtApplicationFee.Rows[0]["PAYMENT_TYPE_ID"].ToString() == CON.ApplicationFeePaymentTypeID.RequestWaiver)
                                {
                                    lineOfData = ReplaceTag(lineOfData, "displayAppFeeAmount", "none");
                                    lineOfData = ReplaceTag(lineOfData, "displayAppFeeComments", "block;");
                                    lineOfData = ReplaceTag(lineOfData, "displayAppFeeWaiverReason", "block;");
                                    lineOfData = ReplaceTag(lineOfData, "APP_FEE_PAYMENT_TYPE", CON.ApplicationFeePaymentType.RequestWaiver);
                                    lineOfData = ReplaceTag(lineOfData, "APP_FEE_WAIVER_REASON", dtApplicationFee.Rows[0]["APPLICATION_FEE_WAIVER_REASON_NAME"].ToString());
                                    log.CreateLogEntry("Replace Tag APP_FEE_WAIVER_REASON End" + regID, Logging.LogPriority.Information);
                                }
                                lineOfData = ReplaceTag(lineOfData, "APP_FEE_STRING", formattedApplicationFee);
                                log.CreateLogEntry("Replace Tag APP_FEE_STRING End" + regID, Logging.LogPriority.Information);
                            }
                            else
                            {
                                lineOfData = ReplaceTag(lineOfData, "displayApplicationFee", "block");
                                lineOfData = ReplaceTag(lineOfData, "APP_FEE_COMMENT", formattedApplicationFee);
                            }
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "displayApplicationFee", "none");
                        log.CreateLogEntry("Replace Tag DisplayApplicationFee End" + regID, Logging.LogPriority.Information);
                        //Change 
                        //Personnel questions
                        if (isChangeOperatorInformationVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "chopDisplay", "block");
                            if (dtChopSelect.Rows != null && dtChopSelect.Rows.Count > 0)
                            {
                                DataRow drChopSelectInfo = dtChopSelect.Rows[0];
                                string CHOP_TYPE = "";
                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CHOP_TYPE_ID", drChopSelectInfo)))
                                {
                                    int chopid = ObjectControllerHelper.GetInt("CHOP_TYPE_ID", drChopSelectInfo);


                                    CHOP_TYPE = dtChopType.AsEnumerable().Where(r => r.Field<int>("CHOP_TYPE_ID")
                                                                       == chopid).Select(r => r.Field<string>("CHOP_TYPE_DESC")).FirstOrDefault();
                                }
                                log.CreateLogEntry("Replace Tag CHOP_TYPE_ID End" + regID, Logging.LogPriority.Information);
                                string startDate = "";
                                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("Service_Location_Effective_Date", drChopSelectInfo)))
                                {
                                    startDate = ObjectControllerHelper.GetDateTime("Service_Location_Effective_Date", drChopSelectInfo).ToString("MM/dd/yyyy");
                                }
                                log.CreateLogEntry("Replace Tag Service_Location_Effective_Date End" + regID, Logging.LogPriority.Information);
                                lineOfData = ReplaceTag(lineOfData, "CHOP_Type", CHOP_TYPE);
                                lineOfData = ReplaceTag(lineOfData, "Exiting_OperatoMediID", ObjectControllerHelper.GetString("MEDICAID_ID_SELL", drChopSelectInfo));
                                lineOfData = ReplaceTag(lineOfData, "Purchase_Price", "$ " + ObjectControllerHelper.GetDecimal("AMT_CHOP", drChopSelectInfo).ToString());
                                lineOfData = ReplaceTag(lineOfData, "Sub_lease_Amt", "$ " + ObjectControllerHelper.GetDecimal("AMT_SUB_LEASE", drChopSelectInfo).ToString());
                                lineOfData = ReplaceTag(lineOfData, "Total_Lease_Amt", "$ " + ObjectControllerHelper.GetDecimal("AMT_MASTER_LEASE", drChopSelectInfo).ToString());
                                lineOfData = ReplaceTag(lineOfData, "Effective_Date_Chop", startDate);
                                log.CreateLogEntry("Replace Tag Effective_Date_Chop End" + regID, Logging.LogPriority.Information);
                            }
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "chopDisplay", "none");


                        // DME
                        if (isDMEvisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYDME", "block;");
                            //Personnel questions
                            if (ObjectControllerHelper.HasRows(dtDMEPersonnelInfo))
                            {
                                DataRow drDMEPersonnelInfo = dtDMEPersonnelInfo.Rows[0];
                                lineOfData = ReplaceTag(lineOfData, "DMEENTERYES", ObjectControllerHelper.GetBool("IsBeneficiaryVisited", drDMEPersonnelInfo) ? "Checked" : "Unchecked");
                                lineOfData = ReplaceTag(lineOfData, "DMEENTERNO", ObjectControllerHelper.GetBool("IsBeneficiaryVisited", drDMEPersonnelInfo) ? "Unchecked" : "Checked");
                                lineOfData = ReplaceTag(lineOfData, "DMECONTACTYES", ObjectControllerHelper.GetBool("IsBeneficiaryPhysicallyContacted", drDMEPersonnelInfo) ? "Checked" : "Unchecked");
                                lineOfData = ReplaceTag(lineOfData, "DMECONTACTNO", ObjectControllerHelper.GetBool("IsBeneficiaryPhysicallyContacted", drDMEPersonnelInfo) ? "Unchecked" : "Checked");
                                lineOfData = ReplaceTag(lineOfData, "DMENUMLICENSES", ObjectControllerHelper.GetInt("NoOfLicenseCopies", drDMEPersonnelInfo).ToString());
                                lineOfData = ReplaceTag(lineOfData, "DMENUMCHECKS", ObjectControllerHelper.GetInt("NoOfBackgroundChecks", drDMEPersonnelInfo).ToString());
                                log.CreateLogEntry("Replace Tag Personnel questions End" + regID, Logging.LogPriority.Information);

                                if (dmeProfessionals.Count > 0)
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYDMEPERSONNEL", "block");
                                    lineOfData = ReplaceTag(lineOfData, "DME_PERSONNEL_LIST", formattedDMEProfessionals);
                                }
                                else
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYDMEPERSONNEL", "none");
                                }

                                if (dmeProducts.Count > 0)
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYDMEPRODUCTS", "block");
                                    lineOfData = ReplaceTag(lineOfData, "DME_PRODUCT_LIST", formattedDMEProducts);
                                }
                                else
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYDMEPRODUCTS", "none");
                                }

                                if (dmeAccreditations.Count > 0)
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYDMEQUALIFICATIONS", "block");
                                    lineOfData = ReplaceTag(lineOfData, "DME_QUALIFICATION_LIST", formattedDMEAccreditations);
                                }
                                else
                                {
                                    lineOfData = ReplaceTag(lineOfData, "DISPLAYDMEQUALIFICATIONS", "none");
                                }
                                log.CreateLogEntry("Replace TagDISPLAYDMEQUALIFICATIONS End" + regID, Logging.LogPriority.Information);

                            }

                            lineOfData = ReplaceTag(lineOfData, "DISPLAYDME", "block;");
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYDME", "none;");

                        // Agreements
                        bool hospiceProvider = this.IsHospiceProvider(regID, ds.Tables[4], dataRow);
                        if (hospiceProvider)
                        {
                            lineOfData = ReplaceTag(lineOfData, "displayAPCONTRACT", "block;page-break-after:always;page-break-before:always;");
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "displayAPCONTRACT", "none");
                        }
                        log.CreateLogEntry("Checking  IsHospiceProvider with bool" + regID, Logging.LogPriority.Information);
                        lineOfData = ReplaceTag(lineOfData, "APCONTRACT", hospiceProvider ? "Checked" : "Unchecked");
                        string submitDate = "";
                        if (!String.IsNullOrEmpty(submitDateTime))
                        {
                            submitDate = Convert.ToDateTime(submitDateTime).ToShortDateString();
                        }
                        log.CreateLogEntry("Checking SUBMIT_DATE_TIME submitDate" + regID, Logging.LogPriority.Information);
                        if (!string.IsNullOrEmpty(submitDate))
                        {
                            lineOfData = ReplaceTag(lineOfData, "DATESUBMITTED", submitDate);
                            lineOfData = ReplaceTag(lineOfData, "AGREEMENTDATE", submitDate);
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DATESUBMITTED", "Not Yet Submitted");
                            lineOfData = ReplaceTag(lineOfData, "AGREEMENTDATE", "Not Yet Submitted");
                        }
                        log.CreateLogEntry("ReplacTag AGREEMENTDATE" + regID, Logging.LogPriority.Information);
                        // Malpractice Claims
                        if (isMalpracticeClaimsHistoryVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYMALPRACTICE", "block");
                            //lineOfData = ReplaceTag(lineOfData, "MALPRACTICE_LIST", formattedmalpracticeClaims);
                            // malpracticeClaims.Count > 0
                            lineOfData = !string.IsNullOrEmpty(formattedmalpracticeClaims)
                                ? ReplaceTag(lineOfData, "MALPRACTICE_LIST", formattedmalpracticeClaims)
                                : ReplaceTag(lineOfData, "MALPRACTICE_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Malpractice Claims found.")));
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYMALPRACTICE", "none");
                        log.CreateLogEntry("ReplacTagMalpractice Claims" + regID, Logging.LogPriority.Information);
                        // Work History, work gap
                        if (isEmploymentHistoryVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYWorkHistory", "block");
                            lineOfData = ReplaceTag(lineOfData, "WORKHISTORY_DETAILS", workHistoryDetails);
                            lineOfData = ReplaceTag(lineOfData, "WORKGAPHISTORY_DETAILS", workGapDetails);
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYWorkHistory", "none");
                        log.CreateLogEntry("ReplacTa Work History, work gap" + regID, Logging.LogPriority.Information);
                        lineOfData = ReplaceTag(lineOfData, "CONFIRM_GROUP_AFFFI", confGrpAffl);
                        lineOfData = ReplaceTag(lineOfData, "PENDING_GROUP_AFFFI", pendingGrpAffl);
                        lineOfData = ReplaceTag(lineOfData, "Hospital_AFFFI", hospitalGrpAffl);
                        lineOfData = ReplaceTag(lineOfData, "Deligate_AFFFI", delegateGrpAffl);

                        lineOfData = ReplaceTag(lineOfData, "DODD_LIST", DODD_LIST);
                        lineOfData = ReplaceTag(lineOfData, "ODA_LIST", ODA_LIST);
                        lineOfData = ReplaceTag(lineOfData, "ODAPassport_LIST", ODAPassport_LIST);
                        lineOfData = ReplaceTag(lineOfData, "ODATransportation_LIST", ODATransportation_LIST);
                        lineOfData = ReplaceTag(lineOfData, "ODAOther_LIST", ODAOther_LIST);
                        log.CreateLogEntry("ReplacTa Work History, work gap End" + regID, Logging.LogPriority.Information);
                        //Site Visit Screening
                        if (isSiteVisitScreeningVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYSiteVisit", "block");
                            lineOfData = ReplaceTag(lineOfData, "SITEVISIT_DETAILS", siteVisitDetails);
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYSiteVisit", "none");
                        log.CreateLogEntry("ReplacTag Site Visit Screening End" + regID, Logging.LogPriority.Information);
                        lineOfData = ReplaceTag(lineOfData, "SHOWAGREEMENTS", (isAgreementsVisible) ? "block" : "none");
                        lineOfData = ReplaceTag(lineOfData, "SHOWMEDICAIDAGREEMENTS", (isMedicaidProviderAgreementVisible) ? "block" : "none");
                        lineOfData = ReplaceTag(lineOfData, "SHOWOWNERSHIPAGREEMENT", (isOwnershipAckVisible) ? "block" : "none");
                        lineOfData = ReplaceTag(lineOfData, "SHOWFELONYRELEASE", (isFelonyOrMisdemeanorVisible) ? "block" : "none");
                        lineOfData = ReplaceTag(lineOfData, "SHOWAUTHRELEASE", (isAuthorizationReleaseInformationVisible) ? "block" : "none");
                        if (isBHInfoVisible && (dtBHQuestions.Rows != null || dtBHQuestions.Rows.Count > 0))
                            DisplayBehavioralInfoQuestions(dtBHQuestions, ref lineOfData);
                        log.CreateLogEntry("ReplacTag SHOWAUTHRELEASE End" + regID, Logging.LogPriority.Information);
                        if (isPracticePartnershipVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYPRACTICEPARTNERSHIP", "block");
                            lineOfData = ReplaceTag(lineOfData, "PRACTICE_PARTNERSHIP_LIST", formattedPracticePartnershipInfo);
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYPRACTICEPARTNERSHIP", "none");
                        log.CreateLogEntry("ReplacTag DISPLAYPRACTICEPARTNERSHIP End" + regID, Logging.LogPriority.Information);
                        if (isCPCSpecialtiesVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYADDLSPECIALTY", "block");
                        }
                        else
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYADDLSPECIALTY", "none");
                        log.CreateLogEntry("ReplacTag DISPLAYADDLSPECIALTY End" + regID, Logging.LogPriority.Information);
                        if (isBHInfoVisible)
                            lineOfData = ReplaceTag(lineOfData, "SHOWBHQUESTIONS", "block");

                        // Waiver Services
                        if (isWaiverServicesVisible)
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYWAIVERSVC", "block");
                            lineOfData = !string.IsNullOrEmpty(formattedServices)
                                    ? ReplaceTag(lineOfData, "SERVICE_LIST", formattedServices)
                                    : ReplaceTag(lineOfData, "SERVICE_LIST", this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No Waiver Services records found.")));
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYWAIVERSVC", "none");
                        }
                        log.CreateLogEntry("ReplacTag DISPLAYWAIVERSVC End" + regID, Logging.LogPriority.Information);
                        // Household members
                        if (!string.IsNullOrEmpty(householdMembers))
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYHOUSEHOLDMEMBERS", "block");
                            lineOfData = ReplaceTag(lineOfData, "HOUSEHOLD_MEMBER_LIST", householdMembers);
                        }
                        else
                        {
                            lineOfData = ReplaceTag(lineOfData, "DISPLAYHOUSEHOLDMEMBERS", "none");
                        }
                        log.CreateLogEntry("ReplacTag  Household members End" + regID, Logging.LogPriority.Information);
                        var pattern = @"\[(.*?)\]";
                        var matches = Regex.Matches(lineOfData, pattern);
                        log.CreateLogEntry("ReplacTag  Group start" + regID, Logging.LogPriority.Information);
                        foreach (Match m in matches)
                        {
                            lineOfData = ReplaceTag(lineOfData, m.Groups[1].Value.Replace("[", ""), "");
                        }
                        log.CreateLogEntry("ReplacTag  Group End" + regID, Logging.LogPriority.Information);
                        if (FindTag(lineOfData, "PAGE_NUMBER"))
                        {
                            //Force a page break
                            lineOfData = ReplaceTag(lineOfData, "PAGE_NUMBER", string.Empty); //TODO:  verify handling
                            AddLine(ref sbDocPages, ref pageNbr, ref lineNbr, lineOfData, true);
                        }
                        else
                        {
                            AddLine(ref sbDocPages, ref pageNbr, ref lineNbr, lineOfData, false);
                        }
                        log.CreateLogEntry("ReplacTag  Force a page break End" + regID, Logging.LogPriority.Information);
                    }
                }
                log.CreateLogEntry("Updated all the data as per db value in RegistrationApplication   regid " + regID, Logging.LogPriority.Information);
                if (sbDocPages[sbDocPages.Count - 1].Length == 0)
                {
                    sbDocPages.RemoveAt(sbDocPages.Count - 1);
                    log.CreateLogEntry("Remove sub doc pages  regid " + regID, Logging.LogPriority.Information);

                }
                finalDoc = new string[sbDocPages.Count];
                pageNbr = 0;
                log.CreateLogEntry("Return sbDocPages foreach regid " + regID, Logging.LogPriority.Information);
                foreach (StringBuilder sbPage in sbDocPages)
                {
                    finalDoc[pageNbr] = sbPage.ToString();
                    pageNbr++;
                }
                log.CreateLogEntry("Return document with data  regid " + regID, Logging.LogPriority.Information);
                return finalDoc;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Error encountered during Generation of ICF/IID Contract pdf [RegID-" + regID.ToString() + "]: " + ex.Message, Logging.LogPriority.Error);

            }
            return finalDoc;
        }

        private string GetSiteVisitScreeningDetails(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                sb.AppendLine("<table id=\"idTable\" >");
                ShouldDisplayMessageBasedOnData(sb, null, "No Site Visit Screening information found");
                sb.AppendLine("</table>");
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CommonSiteVisitScreeningHtmlCodeGenerator(sb, dr);
                }
            }
            return sb.ToString();
        }
        private List<string> GetMedicaidDetails(DataTable dt)
        {
            List<string> sb = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                return this.CommonHtmlMapperWhenNoRecordFound("No Other State Medicaid Number found");

            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string startDate = "";
                    if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("MEDICAID_EFF_DATE", dr)))
                    {
                        startDate = ObjectControllerHelper.GetDateTime("MEDICAID_EFF_DATE", dr).ToString("MM/dd/yyyy");
                    }
                    sb.Add("<tr>");
                    sb.Add("<td>");
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("MEDICAID_STATE", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("MEDICAID_NUMBER", dr)));

                    sb.Add(string.Format("<span>{0}</span></td><td>", startDate));

                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NPI", dr)));
                    sb.Add("</td>");
                    sb.Add("</tr>");
                }
            }
            return sb;
        }
        private List<string> GetPracticePartnershipDetails(int regID)
        {
            DataSet ds = RegistrationController.GetPracticePartnership(regID);

            List<string> sb = new List<string>();
            if (ds.Tables[0] == null || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No Practice Partnership Information found");
            else
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Add("<tr>");
                    sb.Add("<td>");
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("AffiliateName", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("GroupMedicaidID", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("AffiliateMedicaidID", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>",
                        !string.IsNullOrEmpty(ObjectControllerHelper.GetString("StartDate", dr)) ?
                        ObjectControllerHelper.GetDateTime("StartDate", dr).ToString(dateFormat) : ""));
                    sb.Add(string.Format("<span>{0}</span>",
                        !string.IsNullOrEmpty(ObjectControllerHelper.GetString("EndDate", dr))
                        ? ObjectControllerHelper.GetDateTime("EndDate", dr).ToString(dateFormat) : ""));
                    sb.Add("</td>");
                    sb.Add("</tr>");
                }
            }
            return sb;
        }
        private List<string> GetWavierServiceDisplay(DataTable dt, string noRecordMsg)
        {
            List<string> sb = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound(noRecordMsg);
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Add("<tr>");
                    sb.Add("<td>");
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("SERVICE_NAME", dr)));

                    sb.Add(string.Format("<span>{0}</span></td><td>",
                        !string.IsNullOrEmpty(ObjectControllerHelper.GetString("START_DATE", dr)) ?
                        ObjectControllerHelper.GetDateTime("START_DATE", dr).ToString(dateFormat) : ""));
                    sb.Add(string.Format("<span>{0}</span>",
                         !string.IsNullOrEmpty(ObjectControllerHelper.GetString("END_DATE", dr))
                        ? ObjectControllerHelper.GetDateTime("END_DATE", dr).ToString(dateFormat) : ""));
                    sb.Add("</td>");
                    sb.Add("</tr>");
                }
            }
            return sb;
        }
        private List<string> GetPendingAffilation(DataTable dt, bool isPending)
        {
            List<string> sb = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound(isPending ?
                    "No Pending Affilation found" : "No confirmed affiliations found.");
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Add("<tr>");
                    sb.Add("<td>");
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("GroupName", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NPI", dr)));
                    if (isPending)
                        sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("Medicaid_ID", dr)));
                    else
                        sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("MedicaidID", dr)));
                    if (isPending)
                        sb.Add(string.Format("<span>{0}</span></td><td>",
                         !string.IsNullOrEmpty(ObjectControllerHelper.GetString("Start_Date", dr)) ?
                         ObjectControllerHelper.GetDateTime("Start_Date", dr).ToString(dateFormat) : ""));
                    else
                        sb.Add(string.Format("<span>{0}</span></td><td>",
                       !string.IsNullOrEmpty(ObjectControllerHelper.GetString("StartDate", dr)) ?
                        ObjectControllerHelper.GetDateTime("StartDate", dr).ToString(dateFormat) : ""));

                    if (isPending)
                        sb.Add(string.Format("<span>{0}</span>",
                        !string.IsNullOrEmpty(ObjectControllerHelper.GetString("End_Date", dr)) ?
                         ObjectControllerHelper.GetDateTime("End_Date", dr).ToString(dateFormat) : ""));
                    else
                        sb.Add(string.Format("<span>{0}</span>",
                         !string.IsNullOrEmpty(ObjectControllerHelper.GetString("EndDate", dr))
                        ? ObjectControllerHelper.GetDateTime("EndDate", dr).ToString(dateFormat) : ""));
                    sb.Add("</td>");
                    sb.Add("<td>");
                    if (isPending)
                        sb.Add(string.Format("<span>{0}</span></td><td>", " Pending Approval"));
                    else
                        sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("AffiliationStatus", dr)));
                    sb.Add(string.Format("<span>{0}</span></td>", FormatGrpAddress(dr, isPending)));
                    sb.Add("</tr>");
                }
            }
            return sb;
        }
        private List<string> GethospitalAff(DataTable dt)
        {
            List<string> sb = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No hospital affiliations found.");
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Add("<tr>");
                    sb.Add("<td>");
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("FacilityName", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("StaffCategory", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("StatusofPrivileges", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("Is_Primary_Facility", dr)));
                    sb.Add(string.Format("<span>{0}</span>",
                        !string.IsNullOrEmpty(ObjectControllerHelper.GetString("StartDate", dr)) ?
                        ObjectControllerHelper.GetDateTime("StartDate", dr).ToString(dateFormat) : ""));
                    sb.Add("</td>");
                    sb.Add("<td>");
                    sb.Add(string.Format("<span>{0}</span>",
                       !string.IsNullOrEmpty(ObjectControllerHelper.GetString("EndDate", dr))
                        ? ObjectControllerHelper.GetDateTime("EndDate", dr).ToString(dateFormat) : ""));
                    sb.Add("</td>");
                    sb.Add("</tr>");
                }
            }
            return sb;
        }
        private List<string> GetDelegateAff(DataTable dt)
        {
            List<string> sb = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No delegates.");
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Add("<tr>");
                    sb.Add("<td>");
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("DELEGATES_NAME", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("MEDICAID_ID", dr)));


                    sb.Add("</tr>");
                }
            }
            return sb;
        }

        private List<string> GetSubmittedagreement(DataTable dt, bool saveDocument, int regID, DataTable dtWork)
        {
            List<string> sb = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No Submitted Agreement found");
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Add("<tr>");
                    sb.Add("<td>");
                    sb.Add(string.Format("<span>{0}</span></td><td>",
                       !string.IsNullOrEmpty(ObjectControllerHelper.GetString("AGREEMENT_SUBMITTED_DATE", dr)) ?
                      ObjectControllerHelper.GetString("AGREEMENT_SUBMITTED_DATE", dr) : ""));
                    sb.Add(string.Format("<span>{0}</span></td><td>",
                       !string.IsNullOrEmpty(ObjectControllerHelper.GetString("AGREEMENT_EFFECTIVE_DATE", dr)) ?
                      ObjectControllerHelper.GetString("AGREEMENT_EFFECTIVE_DATE", dr) : ""));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("UserName", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("REG_ID", dr)));

                    sb.Add("</td>");
                    sb.Add("</tr>");
                }
                //if(saveDocument)
                //{
                //    bool isSubmittedAgreement = ObjectControllerHelper.GetBool("IS_AGREEEMENT_SUBMITTED", dtWork.Rows[0]);
                //    string uname = ObjectControllerHelper.GetString("UserName", dtWork.Rows[0]);
                //    if (isSubmittedAgreement)
                //    {
                //        sb.Add("<tr>");
                //        sb.Add("<td>");
                //        sb.Add(string.Format("<span>{0}</span></td><td>", DateTime.Now.ToString()));
                //        sb.Add(string.Format("<span>{0}</span></td><td>", uname));
                //        sb.Add(string.Format("<span>{0}</span></td><td>", regID.ToString()));

                //        sb.Add("</td>");
                //        sb.Add("</tr>");
                //    }
                //}
            }
            return sb;
        }
        private List<string> GetSubmittedUpdateWFagreement(DataTable dt, bool saveDocument, int regID)
        {
            List<string> sb = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No Update Workflow Agreement found");
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Add("<tr>");
                    sb.Add("<td>");
                    sb.Add(string.Format("<span>{0}</span></td><td>",
                       !string.IsNullOrEmpty(ObjectControllerHelper.GetString("AGREEMENT_SUBMITTED_DATE", dr)) ?
                      ObjectControllerHelper.GetString("AGREEMENT_SUBMITTED_DATE", dr) : ""));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("UserName", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("REG_ID", dr)));

                    sb.Add("</td>");
                    sb.Add("</tr>");
                }
            }
            return sb;
        }

        private void CommonSiteVisitScreeningHtmlCodeGenerator(StringBuilder sb, DataRow dr)
        {
            //TODO
            sb.AppendLine("<table id=\"idTable\" >");


            sb.AppendLine("</table>");
        }
        private string GetWorkGapDetails(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                sb.AppendLine("<table id=\"idTable\" >");
                ShouldDisplayMessageBasedOnData(sb, null, "No Work Gap information found");
                sb.AppendLine("</table>");
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CommonWorkGapHistoryHtmlCodeGenerator(sb, dr);
                }
            }
            return sb.ToString();
        }

        private void CommonWorkGapHistoryHtmlCodeGenerator(StringBuilder sb, DataRow dr)
        {
            var startDate = String.Empty;
            var endDate = String.Empty;
            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("GAP_START_DATE", dr)))
            {
                startDate = ObjectControllerHelper.GetDateTime("GAP_START_DATE", dr).ToString("MM/dd/yyyy");
            }
            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("GAP_END_DATE", dr)))
            {
                endDate = ObjectControllerHelper.GetDateTime("GAP_END_DATE", dr).ToString("MM/dd/yyyy");
            }
            sb.AppendLine("<table id=\"idTable\" >");

            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Gap Start Date</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", startDate));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Gap End Date</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", endDate));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Gap Reason</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("GAP_REASON", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");
        }

        private void DisplayBehavioralInfoQuestions(DataTable dtBHQuestions, ref string lineOfData)
        {

            lineOfData = ReplaceTag(lineOfData, "BH01", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[0]));

            lineOfData = ReplaceTag(lineOfData, "BH02", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[1]));

            lineOfData = ReplaceTag(lineOfData, "BH03", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[2]));

            lineOfData = ReplaceTag(lineOfData, "BH04", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[3]));

            lineOfData = ReplaceTag(lineOfData, "BH05", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[4]));

            lineOfData = ReplaceTag(lineOfData, "BH06", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[5]));

            lineOfData = ReplaceTag(lineOfData, "BH07", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[6]));

            lineOfData = ReplaceTag(lineOfData, "BH08", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[7]));

            lineOfData = ReplaceTag(lineOfData, "BH09", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[8]));

            lineOfData = ReplaceTag(lineOfData, "BH10", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[9]));

            lineOfData = ReplaceTag(lineOfData, "BH11", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[10]));

            lineOfData = ReplaceTag(lineOfData, "BH12", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[11]));

            lineOfData = ReplaceTag(lineOfData, "BH13", ObjectControllerHelper.GetString("QUESTION_TEXT", dtBHQuestions.Rows[12]));
        }

        private List<string> GetBehavioralHealthInfo(DataTable dt)
        {
            List<string> behavioralHealth = new List<string>();
            if (dt == null || dt.Rows != null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No Behavioral Health Information found");
            foreach (DataRow dr in dt.Rows)
            {
                behavioralHealth.Add("<tr>");
                behavioralHealth.Add("<td>");
                behavioralHealth.Add(string.Format("<span>{0}</span></td><td>",
                    ObjectControllerHelper.GetString("Certification_Type_Id", dr)));
                behavioralHealth.Add(string.Format("<span>{0}</span>",
                    !string.IsNullOrEmpty(ObjectControllerHelper.GetString("Certification_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("Certification_DATE", dr).ToString(dateFormat) : ""));
                behavioralHealth.Add("</td>");
                behavioralHealth.Add("</tr>");
            }

            return behavioralHealth;
        }

        private List<string> GetRequiredDocuments()
        {
            return this.CommonHtmlMapperWhenNoRecordFound(@"You may also mail in additional documentation, which may result in a delay to process your application.  <br />
                    Mailing Address:<br />
                    MES Department of Medicaid <br />
                    Provider Enrollment Unit <br />
                    1600 Tysons Blvd,Suite 1400<br />
                    McLean, VA 22102");
        }

        private List<string> GetApplicationFee(DataTable dt)
        {
            List<string> sb = new List<string>();
            if (dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No Fee Payment information found");
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string Status_ID = ObjectControllerHelper.GetString("APPLICATION_FEE_STATUS_ID", dr);
                    string Status_Name = "";

                    if (Status_ID == CON.ApplicationFeePaymentStatus.Pending)
                    {
                        Status_Name = CON.ApplicationFeePaymentStatusName.Pending;
                    }
                    else if (Status_ID == CON.ApplicationFeePaymentStatus.Paid)
                    {
                        Status_Name = CON.ApplicationFeePaymentStatusName.Paid;
                    }
                    else if (Status_ID == CON.ApplicationFeePaymentStatus.Waived)
                    {
                        Status_Name = CON.ApplicationFeePaymentStatusName.Waived;
                    }
                    string startDate = "";
                    if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("FEE_STATUS_DATE_TIME", dr)))
                    {
                        startDate = ObjectControllerHelper.GetDateTime("FEE_STATUS_DATE_TIME", dr).ToString("MM/dd/yyyy");
                    }
                    sb.Add("<tr>");
                    sb.Add("<td>");
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("FEE_AMOUNT", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", Status_Name));
                    sb.Add(string.Format("<span>{0}</span></td><td>", startDate));

                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("APPLICATION_FEE_WAIVER_REASON_NAME", dr)));
                    sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("TRANSACTION_ID", dr)));
                    sb.Add("</td>");
                    sb.Add("</tr>");
                }
            }
            return sb;
        }

        private List<string> GetRegistrationServices(DataTable dt)
        {
            List<string> services = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                services.Add("<tr>");
                services.Add("<td>");

                services.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("WAIVERNAME", dr)));
                services.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("WAIVER_TYPE_CODE", dr)));
                services.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NAME", dr)));
                if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("DIDDCONTRACT_FROMDATE", dr)))
                {
                    services.Add(string.Format("<span>{0}</span></td><td>", ""));
                }
                else
                {
                    services.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetDateTime("DIDDCONTRACT_FROMDATE", dr).ToString("MM/dd/yyyy")));
                }
                if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("DIDDCONTRACT_TODATE", dr)))
                {
                    services.Add(string.Format("<span>{0}</span>", ""));
                }
                else
                {
                    services.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetDateTime("DIDDCONTRACT_TODATE", dr).ToString("MM/dd/yyyy")));
                }

                services.Add("</td>");
                services.Add("</tr>");
            }

            return services;
        }
        public string FormatAddress(DataRow drAddress)
        {
            string address = string.Empty;
            address = GetFormattedAddress(ObjectControllerHelper.GetString("ADDRESS1", drAddress), ObjectControllerHelper.GetString("ADDRESS2", drAddress), ObjectControllerHelper.GetString("CITY", drAddress), ObjectControllerHelper.GetString("STATE", drAddress), ObjectControllerHelper.GetString("ZIP", drAddress), string.Empty);
            address = string.IsNullOrEmpty(ObjectControllerHelper.GetString("COUNTYNAME", drAddress)) ? address : address + "<BR/>" + ObjectControllerHelper.GetString("COUNTYNAME", drAddress);
            return address;
        }
        public string FormatGrpAddress(DataRow drAddress, bool isPending)
        {
            string address = string.Empty;
            if (isPending)
            {
                address = GetFormattedAddress(ObjectControllerHelper.GetString("SERVICING_ADDRESS1", drAddress), ObjectControllerHelper.GetString("SERVICING_ADDRESS2", drAddress), ObjectControllerHelper.GetString("SERVICING_CITY", drAddress), ObjectControllerHelper.GetString("SERVICING_STATE", drAddress), ObjectControllerHelper.GetString("SERVICING_ZIP", drAddress), string.Empty);

            }
            else
            {
                address = GetFormattedAddress(ObjectControllerHelper.GetString("ADDRESS1", drAddress), ObjectControllerHelper.GetString("ADDRESS2", drAddress), ObjectControllerHelper.GetString("CITY1", drAddress), ObjectControllerHelper.GetString("STATE1", drAddress), ObjectControllerHelper.GetString("ZIP1", drAddress), string.Empty);
            }
            return address;
        }
        protected string FormatSpecialtyFocus(DataTable dtFocusInfo)
        {
            string specialtyFocus = string.Empty;

            if (dtFocusInfo.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFocusInfo.Rows)
                {
                    if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("ENDORSEMENT_NUMBER", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Number:" + ObjectControllerHelper.GetString("ENDORSEMENT_NUMBER", dr) : specialtyFocus + "<Br/>" + " Number: " + ObjectControllerHelper.GetString("ENDORSEMENT_NUMBER", dr);
                    }
                    if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("ENDORSEMENT_SPECIALITY", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Specialty:" + ObjectControllerHelper.GetString("ENDORSEMENT_SPECIALITY", dr) : specialtyFocus + "<Br/>" + " Specialty: " + ObjectControllerHelper.GetString("ENDORSEMENT_SPECIALITY", dr);
                    }
                    if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("ENDORSEMENT_FOCUS", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Focus:" + ObjectControllerHelper.GetString("ENDORSEMENT_FOCUS", dr) : specialtyFocus + "<Br/>" + " Focus: " + ObjectControllerHelper.GetString("ENDORSEMENT_FOCUS", dr);
                    }
                    if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("ENDORSEMENT_STATUS", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Status:" + ObjectControllerHelper.GetString("ENDORSEMENT_STATUS", dr) : specialtyFocus + "<Br/>" + " Status: " + ObjectControllerHelper.GetString("ENDORSEMENT_STATUS", dr);
                    }
                    if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CERTIFYING_ORGANIZATION", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Certifying Org:" + ObjectControllerHelper.GetString("CERTIFYING_ORGANIZATION", dr) : specialtyFocus + "<Br/>" + " Certifying Org: " + ObjectControllerHelper.GetString("CERTIFYING_ORGANIZATION", dr);
                    }
                    if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CERTIFICATE_DATE", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Certification Date:" + ObjectControllerHelper.GetDateTime("CERTIFICATE_DATE", dr).ToString("MM/dd/yyyy") : specialtyFocus + "<Br/>" + " Certification Date: " + ObjectControllerHelper.GetDateTime("CERTIFICATE_DATE", dr).ToString("MM/dd/yyyy");
                    }
                    if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CERTIFICATE_EXPIRATION", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Certificate Expiration:" + ObjectControllerHelper.GetDateTime("CERTIFICATE_EXPIRATION", dr).ToString("MM/dd/yyyy") : specialtyFocus + "<Br/>" + " Certificate Expiration: " + ObjectControllerHelper.GetDateTime("CERTIFICATE_EXPIRATION", dr).ToString("MM/dd/yyyy");
                    }
                }
            }
            return specialtyFocus;
        }
        private List<string> GetAffiliationsGrid(DataTable dt)
        {


            List<string> affiliations = new List<string>();
            List<string> sb = new List<string>();
            int affIndex = 0;

            foreach (DataRow dr in dt.Rows)
            {
                string startDate = string.Empty;
                string endDate = string.Empty;
                string RevalidationDate = string.Empty;
                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("START_DATE", dr)))
                {
                    startDate = ObjectControllerHelper.GetDateTime("START_DATE", dr).ToString("MM/dd/yyyy");
                }
                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("END_DATE", dr)))
                {
                    endDate = ObjectControllerHelper.GetDateTime("END_DATE", dr).ToString("MM/dd/yyyy");
                }
                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("RevalidationDate", dr)))
                {
                    RevalidationDate = ObjectControllerHelper.GetDateTime("RevalidationDate", dr).ToString("MM/dd/yyyy");
                }
                sb.Add("<tr>");
                sb.Add("<td>");
                sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("Name", dr)));
                sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NPI", dr)));

                sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("ProviderType", dr)));
                sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("SPECIALTY_TYPE_NAME", dr)));
                sb.Add(string.Format("<span>{0}</span></td><td>", startDate));

                sb.Add(string.Format("<span>{0}</span></td><td>", endDate));
                sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("AffiliationStatus", dr)));
                sb.Add(string.Format("<span>{0}</span></td><td>", RevalidationDate));
                sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("MedicaidID", dr)));
                sb.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("AddressLocation", dr)));
                sb.Add("</td>");
                sb.Add("</tr>");
                affIndex++;
            }

            return sb;
        }
        private string GetAffiliations(DataSet ds)
        {
            string affilitions = "Affiliations";
            string affilitionQuestions = "AffiliationQuestions";
            string affiliationID = "REG_AFFILIATION_ID";
            string relationName = "AffiliationToQuestion";

            ds.Tables[2].TableName = affilitions;
            ds.Tables[8].TableName = affilitionQuestions;
            DataTable filtered = new DataTable();
            if (ds.Tables[affilitions].Rows.Count > 0)
                filtered = ds.Tables[affilitions].AsEnumerable().GroupBy(a => a.Field<int>("REG_AFFILIATION_ID"))
                                         .Select(y => y.First())
                      .CopyToDataTable();
            ds.Tables[affilitions].Rows.Clear();

            filtered.TableName = affilitions;
            ds.Tables[affilitions].Merge(filtered);
            DataRelation relation;
            relation = new DataRelation(relationName, ds.Tables[affilitions].Columns[affiliationID]
                , ds.Tables[affilitionQuestions].Columns[affiliationID]);

            if (relation != null)
                ds.Relations.Add(relation);

            List<string> affiliations = new List<string>();
            StringBuilder sb = new StringBuilder();
            int affIndex = 0;

            foreach (DataRow dr in filtered.Rows)
            {


                foreach (DataRow gaqRow in dr.GetChildRows(relationName))
                {
                    string responseComment = gaqRow["RESPONSE_COMMENT"].ToString();
                    string response = gaqRow["RESPONSE"].ToString().ToLower();
                    string yesValue = response == "true" ? "Checked" : "Unchecked";
                    string noValue = response == "false" ? "Checked" : "Unchecked";

                    switch (gaqRow["QUESTION_TYPE_ID"].ToString())
                    {
                        case "GA01":
                            sb.AppendLine("<p style=\"text-align:left;font-size:10pt; font-weight:normal\">Has there ever been disciplinary action against this provider's license by a licensing board in any state?");
                            break;
                        case "GA02":
                            sb.AppendLine("<p style=\"text-align:left;font-size:10pt; font-weight:normal\">Has the provider ever been sanctioned by Medicare, District of Columbia Medicaid, or any state health program?");
                            break;
                        case "GA03":
                            sb.AppendLine("<p style=\"text-align:left;font-size:10pt; font-weight:normal\">Is this individual identified on the EPLS website as debarred, suspended, proposed for debarment, excluded or disqualified under the nonprocurement common rule, or otherwise declared ineligible from receiving Federal Contracts, certain subcontracts, and certain Federal assistance and benefits?");
                            break;
                        case "GA04":
                            sb.AppendLine("<p style=\"text-align:left;font-size:10pt; font-weight:normal\">Is this individual identified on the OIG List ofExcluded Individuals / Entities as excluded from receiving payment by a Federal health care program?");
                            break;
                        case "GA05":
                            sb.AppendLine("<p style=\"text-align:left;font-size:10pt; font-weight:normal\">In compliance with Title 8 U.S.C. &sect;1324a, has employment eligibility been verified for this individual?");
                            break;
                    }
                    sb.AppendLine(string.Format("</br><input type=\"radio\" name=\"{0}\" {1} />Yes<input type=\"radio\" name=\"{0}\" {2} />No</br>", gaqRow["QUESTION_TYPE_ID"].ToString() + affIndex.ToString(), yesValue, noValue));
                    sb.AppendLine(string.Format("If, \"YES\" a comment is required.</br>{0}</p>", responseComment));
                }
                affIndex++;
            }

            return sb.ToString();
        }
        private string GetEducations(DataTable dataTable)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                CommonEducationsHtmlCodeGenerator(stringBuilder, null);
            }
            else
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    CommonEducationsHtmlCodeGenerator(stringBuilder, dr);
                }
            }
            return stringBuilder.ToString();
        }

        private static void CommonEducationsHtmlCodeGenerator(StringBuilder sb, DataRow dr)
        {
            var startDate = String.Empty;
            var endDate = String.Empty;
            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("START_YEAR", dr)))
            {
                startDate = ObjectControllerHelper.GetDateTime("START_YEAR", dr).ToString("MM/dd/yyyy");
            }
            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("END_YEAR", dr)))
            {
                endDate = ObjectControllerHelper.GetDateTime("END_YEAR", dr).ToString("MM/dd/yyyy");
            }
            sb.AppendLine("<table id=\"idTable\" >");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>School</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("School", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Education</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("EDUCATION_TYPE", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Speciality</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("SPECIALTY_NAME", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Start Date</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", startDate));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>End Date</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", endDate));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>DEGREE</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("DEGREE_ABBREV", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Address1</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("ADDRESS1", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Address2</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("ADDRESS2", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>City</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("CITY", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>State</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("STATE", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Zip</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("ZIP", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Country</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("COUNTRY", dr)));
            sb.AppendLine("</tr>");
            ShouldDisplayMessageBasedOnData(sb, dr, "No Educations information found");
            sb.AppendLine("</table>");
        }
        private string GeConfirmedAff(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {

                sb.AppendLine("<tr>");
                sb.AppendLine(string.Format("<td class='col2'>{0}</td>",
                    ObjectControllerHelper.GetString("GroupName", dr)));
                sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("NPI", dr)));
                sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("Medicaid_ID", dr)));
                sb.AppendLine(string.Format("<td class='col2'>{0}</td>",
                    ObjectControllerHelper.GetString("START_DATE", dr)));
                sb.AppendLine(string.Format("<td class='col2'>{0}</td>",
                   ObjectControllerHelper.GetString("END_DATE", dr)));
                sb.AppendLine("</tr>");


            }
            return sb.ToString();
        }

        private string GeWorkFlows(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {

                sb.AppendLine("<table id=\"idTable\" >");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='col1'>Task Name</td>");
                sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("Task Name", dr)));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='col1'>User Name</td>");
                sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("User Name", dr)));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='col1'>Start Date</td>");
                sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("Start Date", dr)));
                sb.AppendLine("</tr>");

                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='col1'>End Date</td>");
                sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("End Date", dr)));
                sb.AppendLine("</tr>");

                sb.AppendLine("</table>");

            }
            return sb.ToString();
        }
        private string GetOtherService(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                sb.AppendLine(this.FormatList(this.CommonHtmlMapperWhenNoRecordFound("No other service records found.")));
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CommonOtherServiceHtmlCodeGenerator(sb, dr);

                }
            }
            return sb.ToString();
        }

        private static void CommonOtherServiceHtmlCodeGenerator(StringBuilder sb, DataRow dr)
        {
            sb.AppendLine("<tr >");

            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("PRACTICE_NAME", dr)));

            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", GetFormattedAddress(ObjectControllerHelper.GetString("ADDRESS1", dr),
                ObjectControllerHelper.GetString("ADDRESS2", dr), ObjectControllerHelper.GetString("CITY", dr),
                ObjectControllerHelper.GetString("STATE", dr), ObjectControllerHelper.GetString("ZIP", dr), ObjectControllerHelper.GetString("EXT_ZIP", dr))));
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("PHONE1", dr)));
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("ADR_EFFECTIVE_DATE", dr)));
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("ADR_END_DATE", dr)));
            sb.AppendLine("</tr>");
        }

        public static string GetFormattedAddress(string addressLine1, string addressLine2, string city, string state, string zip, string zip4)
        {
            string retAddress = string.Empty;
            var sbAddresss = new StringBuilder();
            sbAddresss.Append(addressLine1);
            sbAddresss.Append("<br/>");
            sbAddresss.AppendFormat("{0} {1}, {2} {3}", addressLine2, city, state, zip);
            retAddress = string.IsNullOrEmpty(zip4) ? sbAddresss.ToString().Trim() : sbAddresss.AppendFormat("- {0}", zip4).ToString().Trim();
            return retAddress;

        }
        public static string GetFomattedLicenseSpecialtyFocusInfo(DataTable dtFocusInfo)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(new Guid(), logMsg);

            string specialtyFocus = string.Empty;
            try
            {
                if (dtFocusInfo.Rows.Count > 0)
                {


                    foreach (DataRow dr in dtFocusInfo.Rows)
                    {
                        if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("ENDORSEMENT_NUMBER", dr)))
                        {

                            specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Number:" + ObjectControllerHelper.GetString("ENDORSEMENT_NUMBER", dr) : specialtyFocus + "<Br/>" + " Number: " + ObjectControllerHelper.GetString("ENDORSEMENT_NUMBER", dr);
                        }

                        if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("ENDORSEMENT_SPECIALITY", dr)))
                        {

                            specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Specialty:" + ObjectControllerHelper.GetString("ENDORSEMENT_SPECIALITY", dr) : specialtyFocus + "<Br/>" + " Specialty: " + ObjectControllerHelper.GetString("ENDORSEMENT_SPECIALITY", dr);
                        }
                        if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("ENDORSEMENT_FOCUS", dr)))
                        {
                            specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Focus:" + ObjectControllerHelper.GetString("ENDORSEMENT_FOCUS", dr) : specialtyFocus + "<Br/>" + " Focus: " + ObjectControllerHelper.GetString("ENDORSEMENT_FOCUS", dr);
                        }
                        if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("ENDORSEMENT_STATUS", dr)))
                        {
                            specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Status:" + ObjectControllerHelper.GetString("ENDORSEMENT_STATUS", dr) : specialtyFocus + "<Br/>" + " Status: " + ObjectControllerHelper.GetString("ENDORSEMENT_STATUS", dr);
                        }
                        if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CERTIFYING_ORGANIZATION", dr)))
                        {
                            specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Certifying Org:" + ObjectControllerHelper.GetString("CERTIFYING_ORGANIZATION", dr) : specialtyFocus + "<Br/>" + " Certifying Org: " + ObjectControllerHelper.GetString("CERTIFYING_ORGANIZATION", dr);
                        }
                        if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CERTIFICATE_DATE", dr)))
                        {
                            specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Certification Date:" + ObjectControllerHelper.GetString("CERTIFICATE_DATE", dr).ToString() : specialtyFocus + "<Br/>" + " Certification Date: " + ObjectControllerHelper.GetString("CERTIFICATE_DATE", dr).ToString();
                        }
                        if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CERTIFICATE_EXPIRATION", dr)))
                        {
                            specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Certificate Expiration:" + ObjectControllerHelper.GetString("CERTIFICATE_EXPIRATION", dr).ToString() : specialtyFocus + "<Br/>" + " Certificate Expiration: " + ObjectControllerHelper.GetString("CERTIFICATE_EXPIRATION", dr).ToString();
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to Get Formatted License Specialty Focus Info"
                                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                                 + ex.StackTrace, Logging.LogPriority.Error);
            }
            return specialtyFocus;
        }
        private string GetMCQs(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                sb.AppendLine("<table id=\"idTable\" >");

                ShouldDisplayMessageBasedOnData(sb, null, "No MCP information found");
                sb.AppendLine("</table>");
            }
            else
            {
                CommonMCQsHtmlCodeGenerator(sb, dt);

            }
            return sb.ToString();
        }

        private static void CommonMCQsHtmlCodeGenerator(StringBuilder sb, DataTable dt)
        {
            sb.AppendLine("<table border='0' cellpadding='2' cellspacing='5'> <tr> <td><b>Name</b></td><td><b>Start Date</b></td><td><b>End Date</b></td>");
            sb.AppendLine("<td><b>Provider Type</b></td><td><b>Tracking Number</b></td><td><b>MITS Specialty</b></td>");
            sb.AppendLine("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>");
                sb.AppendLine(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("provider_name", dr)));
                sb.AppendLine(string.Format("<span>{0}</span></td><td>",
                   !string.IsNullOrEmpty(ObjectControllerHelper.GetString("Start_Date", dr)) ?
                    ObjectControllerHelper.GetDateTime("Start_Date", dr).ToString(dateFormat) : ""));
                sb.AppendLine(string.Format("<span>{0}</span></td><td>",
                   !string.IsNullOrEmpty(ObjectControllerHelper.GetString("End_Date", dr)) ?
                    ObjectControllerHelper.GetDateTime("End_Date", dr).ToString(dateFormat) : ""));
                sb.AppendLine(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("MITS_Provider_Type", dr)));
                sb.AppendLine(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("SL_TRACKING_NUMBER", dr)));
                sb.AppendLine(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("MITS_SPECIALTY", dr)));
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");
            //ShouldDisplayMessageBasedOnData(sb, dr, "No MCQs information found");
        }

        private string GetBoardCertificates(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                sb.AppendLine("<table id=\"idTable\" >");
                ShouldDisplayMessageBasedOnData(sb, null, "No Board Certifications information found");
                sb.AppendLine("</table>");
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CommonBoardCertificationsHtmlCodeGenerator(sb, dr);
                }
            }
            return sb.ToString();
        }
        private string GetWorkHistory(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                sb.AppendLine("<table id=\"idTable\" >");
                ShouldDisplayMessageBasedOnData(sb, null, "No Work History information found");
                sb.AppendLine("</table>");
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CommonWorkHistoryHtmlCodeGenerator(sb, dr);
                }
            }
            return sb.ToString();
        }
        private static void CommonWorkHistoryHtmlCodeGenerator(StringBuilder sb, DataRow dr)
        {
            var startDate = String.Empty;
            var endDate = String.Empty;
            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("WORKED_FROM", dr)))
            {
                startDate = ObjectControllerHelper.GetDateTime("WORKED_FROM", dr).ToString("MM/dd/yyyy");
            }
            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("WORKED_TO", dr)))
            {
                endDate = ObjectControllerHelper.GetDateTime("WORKED_TO", dr).ToString("MM/dd/yyyy");
            }
            sb.AppendLine("<table id=\"idTable\" >");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Practice/ Employer Name</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("NAME", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Start Date</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", startDate));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>End Date</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", endDate));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Organization Name*</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("PRACTICE_NAME", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Address Line1</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("ADDRESS1", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Address Line2</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("ADDRESS2", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>City</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("CITY", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>State</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("STATE", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>County</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("CountyName", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Zip</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("ZIP", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Phone Number 1</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("CONTACT_PHONE_NUMBER", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Phone Ext 1</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("CONTACT_PHONE_EXT", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Fax Number 1</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("FAX1", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Contact Name</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("CONTACT_NAME", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Email Address 1</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("CONTACT_EMAIL_ADDRESS", dr)));
            sb.AppendLine("</tr>");

            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Additional Information</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("ADDITIONAL_INFO", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Reason For Departure</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("REASON_FOR_DEPARTURE", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Are you currently on active miltary duty or miltary reserve?</td>");
            string is_llve = ObjectControllerHelper.GetString("MILTARY_RESERVE", dr);
            if (is_llve == "True")
                is_llve = "Yes";
            else
                is_llve = "No";

            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", is_llve));

            sb.AppendLine("</tr>");

            ShouldDisplayMessageBasedOnData(sb, dr, "No Work History information found");

            sb.AppendLine("</table>");
        }
        private static void CommonBoardCertificationsHtmlCodeGenerator(StringBuilder sb, DataRow dr)
        {
            var date = String.Empty;
            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("Expiration_Date", dr)))
            {
                date = ObjectControllerHelper.GetDateTime("Expiration_Date", dr).ToString("MM/dd/yyyy");
            }
            sb.AppendLine("<table id=\"idTable\" >");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Board Certification</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("Board_Certification_name", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Board Specialty</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("Board_Specialty_name", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Expiration Date</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", date));
            sb.AppendLine("</tr>");
            ShouldDisplayMessageBasedOnData(sb, dr, "No Board Certification information found");

            sb.AppendLine("</table>");
        }
        /// <summary>
        /// When no data received from data source, then append input message to string builder
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="dr"></param>
        /// <param name="displayMessageToUser"></param>
        private static void ShouldDisplayMessageBasedOnData(StringBuilder sb, DataRow dr, string displayMessageToUser)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }
            if (string.IsNullOrEmpty(displayMessageToUser))
            {
                throw new ArgumentException("message", nameof(displayMessageToUser));
            }
            if (dr == null)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>");
                sb.AppendLine(string.Format("<span>{0}</span></td><td>", displayMessageToUser));
                sb.AppendLine("</td>");
                sb.AppendLine("</tr>");
            }
        }

        private string GetCredential(DataTable dataTable)
        {

            StringBuilder stringBuilder = new StringBuilder();
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                CommonCredentialHtmlCodeGenerator(stringBuilder, null);
            }
            else
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    CommonCredentialHtmlCodeGenerator(stringBuilder, dr);
                }
            }
            return stringBuilder.ToString();
        }

        private static void CommonCredentialHtmlCodeGenerator(StringBuilder sb, DataRow dr)
        {
            sb.AppendLine("<table id=\"idTable\" >");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Contact Name</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("CONTACT_NAME", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Practice Name</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("PRACTICE_NAME", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Phone</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.FormatPhone(ObjectControllerHelper.GetString("CONTACT_NUMBER", dr))));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Email</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("EMAIL_ID", dr)));
            sb.AppendLine("</tr>");
            ShouldDisplayMessageBasedOnData(sb, dr, "No Credential information found");
            sb.AppendLine("</table>");
        }

        private string GetContracts(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                CommonContactsHtmlCodeGenerator(sb, null);
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CommonContactsHtmlCodeGenerator(sb, dr);

                }
            }
            return sb.ToString();
        }

        private static void CommonContactsHtmlCodeGenerator(StringBuilder sb, DataRow dr)
        {
            var date = String.Empty;
            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CONTRACT_START_DATE", dr)))
            {
                date = ObjectControllerHelper.GetDateTime("CONTRACT_START_DATE", dr).ToString("MM/dd/yyyy");
            }
            var endDate = String.Empty;
            if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("CONTRACT_END_DATE", dr)))
            {
                endDate = ObjectControllerHelper.GetDateTime("CONTRACT_END_DATE", dr).ToString("MM/dd/yyyy");
            }
            sb.AppendLine("<table id=\"idTable\" >");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Contract Name</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("CONTRACT_TYPE_NAME", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Contract Status</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("CONTRACT_STATUS_NAME", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Status Reason</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", ObjectControllerHelper.GetString("ENROLLMENT_STATUS_REASONS_DESC", dr)));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>Start Date</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", date));
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='col1'>End Date</td>");
            sb.AppendLine(string.Format("<td class='col2'>{0}</td>", endDate));
            sb.AppendLine("</tr>");
            ShouldDisplayMessageBasedOnData(sb, dr, "No Contracts information found");
            sb.AppendLine("</table>");
        }

        private string GetHouseholdMembers(DataSet ds)
        {
            string members = "HouseholdMembers";
            string addresses = "HouseholdMemberAddresses";
            string criminalRecord = "HouseholdMemberCriminalRecord";
            string memberID = "REG_HOUSEHOLD_MEMBER_ID";
            string addressRelationName = "MemberToAddress";
            string criminalRecordRelationName = "MemberToCriminalRecord";

            ds.Tables[9].TableName = members;
            ds.Tables[10].TableName = addresses;
            ds.Tables[11].TableName = criminalRecord;

            DataRelation relation;
            relation = new DataRelation(addressRelationName, ds.Tables[members].Columns[memberID]
                , ds.Tables[addresses].Columns[memberID]);
            ds.Relations.Add(relation);

            relation = new DataRelation(criminalRecordRelationName, ds.Tables[members].Columns[memberID]
                , ds.Tables[criminalRecord].Columns[memberID]);
            ds.Relations.Add(relation);

            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in ds.Tables[9].Rows)
            {
                string birthDate = string.Empty;
                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("BIRTH_DATE", dr)))
                {
                    birthDate = ObjectControllerHelper.GetDateTime("BIRTH_DATE", dr).ToString("MM/dd/yyyy");
                }

                sb.AppendLine("<table id=\"idTable\" >");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td >Name</td>");
                sb.AppendLine(string.Format("<td >{0}</td>", ObjectControllerHelper.GetString("NAME", dr)));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td >Household Status</td>");
                sb.AppendLine(string.Format("<td >{0}</td>", ObjectControllerHelper.GetString("RELATIONSHIP", dr)));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td >Birth Date</td>");
                sb.AppendLine(string.Format("<td >{0}</td>", birthDate));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td >SSN</td>");
                sb.AppendLine(string.Format("<td >{0}</td>", ObjectControllerHelper.GetString("SSN", dr)));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td >Sex</td>");
                sb.AppendLine(string.Format("<td >{0}</td>", ObjectControllerHelper.GetString("SEX", dr)));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td >Previous Last Name</td>");
                sb.AppendLine(string.Format("<td >{0}</td>", ObjectControllerHelper.GetString("PREVIOUS_LAST_NAMES", dr)));
                sb.AppendLine("</tr>");
                sb.AppendLine("</table>");

                if (dr.GetChildRows(addressRelationName).Length > 0)
                {
                    // Header
                    sb.AppendLine("<div style=\"text-align:left;font-size:14pt; font-weight:bold\"><b>Member Address History</b></div>");
                    // Table Header
                    sb.AppendLine("<table border=\"0\" cellpadding=\"2\" cellspacing=\"5\">");
                    sb.AppendLine("<tr><td ><b>County</b></td><td ><b>City</b></td><td ><b>State</b></td><td ><b>from Date</b></td><td ><b>ToString Date</b></td></tr>");
                    // Table Data
                    foreach (DataRow address in dr.GetChildRows(addressRelationName))
                    {
                        string county = address["COUNTY"].ToString();
                        string city = address["CITY"].ToString();
                        string state = address["STATE"].ToString();
                        string fromDate = string.Empty;
                        if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("FROM_DATE", address)))
                        {
                            fromDate = ObjectControllerHelper.GetDateTime("FROM_DATE", address).ToString("MM/dd/yyyy");
                        }
                        string toDate = string.Empty;
                        if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("TO_DATE", address)))
                        {
                            toDate = ObjectControllerHelper.GetDateTime("TO_DATE", address).ToString("MM/dd/yyyy");
                        }
                        sb.AppendLine("<tr><td>");

                        sb.AppendLine(string.Format("<span>{0}</span></td><td>", county));
                        sb.AppendLine(string.Format("<span>{0}</span></td><td>", city));
                        sb.AppendLine(string.Format("<span>{0}</span></td><td>", state));
                        sb.AppendLine(string.Format("<span>{0}</span></td><td>", fromDate));
                        sb.AppendLine(string.Format("<span>{0}</span>", toDate));

                        sb.AppendLine("</tr></td>");
                    }
                    sb.AppendLine("</table>");
                }

                if (dr.GetChildRows(criminalRecordRelationName).Length > 0)
                {
                    sb.AppendLine("<div style=\"text-align:left;font-size:14pt; font-weight:bold\"><b>Member Criminal History</b></div>");
                    sb.AppendLine("<table border=\"0\" cellpadding=\"2\" cellspacing=\"5\">");
                    sb.AppendLine("<tr><td ><b>Offense</b></td></tr>");
                    foreach (DataRow criminalRecordRow in dr.GetChildRows(criminalRecordRelationName))
                    {
                        string offense = criminalRecordRow["CRIMINAL_HISTORY"].ToString();
                        sb.AppendLine("<tr><td>");

                        sb.AppendLine(string.Format("<span>{0}</span>", offense));

                        sb.AppendLine("</td></tr></br>");
                    }
                    sb.AppendLine("</table>");
                }
            }

            return sb.ToString();
        }
        private DataTable Get1099FormDataByAddressId(int regid)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ADDRESS_ID", _addressRegId.ToString());
            parms.Add("REG_ID ", regid.ToString());
            DataSet dsForm = RegistrationController.SelectRegistrationDataWithParams("usp_SelectREG_FORM_1099_INFO", parms);
            DataTable dtFormInfo = ObjectControllerHelper.HasRows(dsForm) ? dsForm.Tables[0] : null;
            return dtFormInfo;
        }

        private bool IsHospiceProvider(int regID, DataTable specialties, DataRow providerRow)
        {
            bool hospiceProvider = false;
            if (specialties.Rows.Count > 0)
            {
                // Get provider type
                string nursingHomeProviderTypeName = AppSettings.Get("NursingHomeProviderTypeName");
                string providerTypeName = ObjectControllerHelper.GetString("PROVIDER_TYPE_NAME", providerRow);
                // 

                // Get specialty
                string hospiceSpecialtyName = AppSettings.Get("HospiceSpecialtyName");
                string specialtyName = ObjectControllerHelper.GetString("SPECIALTY_TYPE_NAME", specialties.Rows[0]);

                if (nursingHomeProviderTypeName == providerTypeName && hospiceSpecialtyName == specialtyName)
                {
                    hospiceProvider = true;
                }
            }
            return hospiceProvider;
        }

        private List<string> GetSpecialties(DataTable dt)
        {
            List<string> specialties = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                specialties.Add("<tr>");
                specialties.Add("<td>");

                specialties.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("SPECIALTY_TYPE_NAME", dr)));
                specialties.Add(string.Format("<span>{0}</span></td><td>", Convert.ToBoolean(ObjectControllerHelper.GetString("PRIMARY_FLAG", dr)) == true ? "Yes" : "No"));
                specialties.Add(string.Format("<span>{0}</span></td><td>",
                      !string.IsNullOrEmpty(ObjectControllerHelper.GetString("START_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("START_DATE", dr).ToString(dateFormat) : ""));
                specialties.Add(string.Format("<span>{0}</span></td><td>",
                      !string.IsNullOrEmpty(ObjectControllerHelper.GetString("END_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("END_DATE", dr).ToString(dateFormat) : ""));
                specialties.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("ENROLL_STATUS_DESC", dr)));
                specialties.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("ENROLLMENT_STATUS_REASONS_DESC", dr)));

                specialties.Add("</td>");
                specialties.Add("</tr>");
            }

            return specialties;
        }

        private List<string> GetTaxonomies(DataTable dt)
        {
            List<string> taxonomies = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                taxonomies.Add("<tr>");
                taxonomies.Add("<td>");

                taxonomies.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("TAXONOMY_CODE", dr)));
                taxonomies.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("TAXONOMY_NAME", dr)));
                taxonomies.Add(string.Format("<span>{0}</span></td><td>", Convert.ToBoolean(ObjectControllerHelper.GetString("PRIMARY_FLAG", dr)) == true ? "Yes" : "No"));
                taxonomies.Add(string.Format("<span>{0}</span></td><td>",
                   !string.IsNullOrEmpty(ObjectControllerHelper.GetString("START_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("START_DATE", dr).ToString(dateFormat) : ""));
                taxonomies.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("END_DATE", dr) != null || ObjectControllerHelper.GetString("END_DATE", dr) != string.Empty ? ObjectControllerHelper.GetDateTime("END_DATE", dr).ToString(dateFormat) : string.Empty));
                //taxonomies.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetDateTime("END_DATE", dr)?.ToString(dateFormat)));


                taxonomies.Add("</td>");
                taxonomies.Add("</tr>");
            }

            return taxonomies;
        }

        private List<string> GetDEANumbers(DataTable dt)
        {
            List<string> deaNumbers = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                deaNumbers.Add("<tr>");
                deaNumbers.Add("<td>");

                deaNumbers.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("DEA_NUMBER", dr)));
                deaNumbers.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("DEA_STATE", dr)));
                deaNumbers.Add(string.Format("<span>{0}</span></td><td>",
                     !string.IsNullOrEmpty(ObjectControllerHelper.GetString("DEA_EFF_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("DEA_EFF_DATE", dr).ToString(dateFormat) : ""));
                deaNumbers.Add(string.Format("<span>{0}</span>",
                     !string.IsNullOrEmpty(ObjectControllerHelper.GetString("DEA_END_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("DEA_END_DATE", dr).ToString(dateFormat) : ""));

                deaNumbers.Add("</td>");
                deaNumbers.Add("</tr>");
            }

            return deaNumbers;
        }

        private List<string> GetCLIANumbers(DataTable dt)
        {
            List<string> cliaNumbers = new List<string>();
            if (dt == null || dt.Rows != null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No CLIA information found");
            foreach (DataRow dr in dt.Rows)
            {
                cliaNumbers.Add("<tr>");
                cliaNumbers.Add("<td>");

                cliaNumbers.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("CLIA_NUMBER", dr)));
                cliaNumbers.Add(string.Format("<span>{0}</span></td><td>",
                     !string.IsNullOrEmpty(ObjectControllerHelper.GetString("CLIA_EFF_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("CLIA_EFF_DATE", dr).ToString(dateFormat) : ""));
                cliaNumbers.Add(string.Format("<span>{0}</span>",
                      !string.IsNullOrEmpty(ObjectControllerHelper.GetString("CLIA_END_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("CLIA_END_DATE", dr).ToString(dateFormat) : ""));

                cliaNumbers.Add("</td>");
                cliaNumbers.Add("</tr>");
            }

            return cliaNumbers;
        }

        private List<string> GetLicenses(DataTable dt, DataRow licenseAddress, DataTable dtlicence, DataTable endrosment)
        {
            List<string> licenses = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No Professional Licenses found");
            foreach (DataRow dr in dt.Rows)
            {
                DataTable licenseAddresstbl = new DataTable();
                if (dtlicence != null && ObjectControllerHelper.GetInt("REG_ADDRESSID", dr) > 0)
                {
                    var professionalAddressTbl = dtlicence.AsEnumerable().Where(r => r.Field<int>("REG_ADDRESS_ID") == ObjectControllerHelper.GetInt("REG_ADDRESSID", dr));
                    if (professionalAddressTbl.Any())
                        licenseAddresstbl = professionalAddressTbl.AsEnumerable().CopyToDataTable();
                }
                DataTable endrosmentdt = new DataTable();
                if (ObjectControllerHelper.GetInt("REG_LICENSURE_ID", dr) > 0 && endrosment != null)
                {

                    var endrosmentdtN = endrosment.AsEnumerable().Where(r => r.Field<int>("REG_LICENSURE_ID") == ObjectControllerHelper.GetInt("REG_LICENSURE_ID", dr));

                    if (endrosmentdtN.Any())
                        endrosmentdt = endrosmentdtN.AsEnumerable().CopyToDataTable();
                }
                licenses.Add("<tr>");
                licenses.Add("<td>");

                licenses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("LICENSE_NUMBER", dr)));
                licenses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("LICENSE_BOARD_NAME", dr)));
                licenses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("LICENSE_STATE", dr)));

                if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("LICENSE_EFF_DATE", dr)))
                {
                    licenses.Add(string.Format("<span>{0}</span></td><td>", ""));
                }
                else
                {
                    licenses.Add(string.Format("<span>{0}</span></td><td>",
                        ObjectControllerHelper.GetDateTime("LICENSE_EFF_DATE", dr).ToString(dateFormat)));
                }
                if (string.IsNullOrEmpty(ObjectControllerHelper.GetString("LICENSE_END_DATE", dr)))
                {
                    licenses.Add(string.Format("<span>{0}</span></td><td>", ""));
                }
                else
                {
                    licenses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetDateTime("LICENSE_END_DATE", dr).ToString("MM/dd/yyyy")));
                }
                if (licenseAddresstbl != null && licenseAddresstbl.Rows.Count > 0)
                    licenses.Add(string.Format("<span>{0}</span></td><td>", FormatAddress(licenseAddresstbl.Rows[0])));
                if (endrosmentdt != null && endrosmentdt.Rows.Count > 0)
                    licenses.Add(string.Format("<span>{0}</span>", FormatSpecialtyFocus(endrosmentdt)));

                licenses.Add("</td>");
                licenses.Add("</tr>");
            }

            return licenses;
        }

        private List<string> GetInsurance(DataTable dt)
        {
            List<string> insurances = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No Professional Liability Insurance records found");
            foreach (DataRow dr in dt.Rows)
            {
                insurances.Add("<tr>");
                insurances.Add("<td>");

                insurances.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("IsMalpracticeClaimed", dr) == "True" ? "Yes" : "No"));
                insurances.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("POLICY_NUMBER", dr)));
                insurances.Add(string.Format("<span>{0}</span></td><td>",
                    !string.IsNullOrEmpty(ObjectControllerHelper.GetString("EFFECTIVE_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("EFFECTIVE_DATE", dr).ToString(dateFormat) : ""));
                insurances.Add(string.Format("<span>{0}</span></td><td>", !string.IsNullOrEmpty(ObjectControllerHelper.GetString("EXPIRATION_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("EXPIRATION_DATE", dr).ToString(dateFormat) : ""));
                insurances.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("PolicyHolder", dr)));
                insurances.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("CoverageAmountPerOccurance", dr)));
                insurances.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("CoverageAmountPerAggregate", dr)));
                insurances.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("MalpracticeInsuranceReason", dr)));
                insurances.Add("</td>");
                insurances.Add("</tr>");
            }

            return insurances;
        }

        private List<string> GetDentalLicenses(DataTable dt)
        {
            List<string> licenses = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("A copy of each license must be uploaded to this page.");
            foreach (DataRow dr in dt.Rows)
            {
                licenses.Add("<tr>");
                licenses.Add("<td>");

                licenses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("LICENSE_DETAILS", dr)));
                licenses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("SPECIALIST", dr)));
                licenses.Add(string.Format("<span>{0}</span></td><td>", (ObjectControllerHelper.GetString("ANESTHESIA_PERMIT", dr) == "1") ? "Yes" : "No"));
                licenses.Add(string.Format("<span>{0}</span>", (ObjectControllerHelper.GetString("SEDATION_PERMIT", dr) == "1") ? "Yes" : "No"));

                licenses.Add("</td>");
                licenses.Add("</tr>");
            }

            return licenses;
        }

        private List<string> GetVisionCertifications(DataTable dt)
        {
            List<string> visionCerts = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                string paType = "";
                bool topical = ObjectControllerHelper.GetBool("TopicalPA", dr);
                bool therapeutic = ObjectControllerHelper.GetBool("TherapeuticPA", dr);
                bool diagnostics = ObjectControllerHelper.GetBool("DignosticPA", dr);
                if (topical)
                    paType += "Topical Ocular Diagnostic Pharmaceutical Agents" + "<br/>";
                if (therapeutic)
                    paType += "Therapeutic Pharmaceutical Agents" + "<br/>";
                if (diagnostics)
                    paType += "Diagnostic Pharmaceutical Agents" + "<br/>";

                visionCerts.Add("<tr>");
                visionCerts.Add("<td>");

                visionCerts.Add(string.Format("<span>{0}</span></td><td>", paType));
                visionCerts.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetBool("OnSiteLab", dr) == true ? "Yes" : "No"));

                visionCerts.Add("</td>");
                visionCerts.Add("</tr>");
            }

            return visionCerts;
        }

        private List<string> GetPharmacyProviderInfo(DataTable dt)
        {
            List<string> pharmacyDetails = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                pharmacyDetails.Add("<tr>");
                pharmacyDetails.Add("<td>");

                pharmacyDetails.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("PHARMACY_NAME", dr)));
                pharmacyDetails.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("CHIEF_PHARMACIST_NAME", dr)));
                pharmacyDetails.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetString("OCCUPANCY_PERMIT_NUMBER", dr)));

                pharmacyDetails.Add("</td>");
                pharmacyDetails.Add("</tr>");
            }

            return pharmacyDetails;
        }

        private List<string> GetPharmacistInfo(DataTable dt)
        {
            List<string> pharmacistDetails = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                pharmacistDetails.Add("<tr>");
                pharmacistDetails.Add("<td>");

                pharmacistDetails.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("PHARMACIST_NAME", dr)));
                pharmacistDetails.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("LICENSE_NUMBER", dr).ToString()));
                pharmacistDetails.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetString("LICENSE_STATE", dr)));

                pharmacistDetails.Add("</td>");
                pharmacistDetails.Add("</tr>");
            }

            return pharmacistDetails;
        }

        private List<string> GetStateCDSNumber(DataTable dt)
        {
            List<string> stateCDSNumbers = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                stateCDSNumbers.Add("<tr>");
                stateCDSNumbers.Add("<td>");

                stateCDSNumbers.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("STATE_CDS_Number", dr)));
                stateCDSNumbers.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("State", dr)));
                stateCDSNumbers.Add(string.Format("<span>{0}</span></td><td>",
                    !string.IsNullOrEmpty(ObjectControllerHelper.GetString("DateIssued", dr)) ? ObjectControllerHelper.GetDateTime("DateIssued", dr).ToString(dateFormat) : ""));
                stateCDSNumbers.Add(string.Format("<span>{0}</span>",
                   !string.IsNullOrEmpty(ObjectControllerHelper.GetString("ExpirationDate", dr)) ? ObjectControllerHelper.GetDateTime("ExpirationDate", dr).ToString(dateFormat) : ""));

                stateCDSNumbers.Add("</td>");
                stateCDSNumbers.Add("</tr>");
            }

            return stateCDSNumbers;
        }
        /// <summary>
        /// Method to generate common html mapper when no records are found in the source
        /// </summary>
        /// <param name="displayMessageToUser">Message thats needs to be displayed over to user</param>
        /// <returns></returns>
        private List<string> CommonHtmlMapperWhenNoRecordFound(string displayMessageToUser)
        {
            if (string.IsNullOrWhiteSpace(displayMessageToUser))
            {
                throw new ArgumentException($"Received empty value in {nameof(CommonHtmlMapperWhenNoRecordFound)} method", nameof(displayMessageToUser));
            }

            List<string> lstDisplayMessageToUser = new List<string>();
            lstDisplayMessageToUser.Add("<tr>");
            lstDisplayMessageToUser.Add("<td>");
            lstDisplayMessageToUser.Add(string.Format("<span>{0}</span></td><td>", displayMessageToUser));
            lstDisplayMessageToUser.Add("</td>");
            lstDisplayMessageToUser.Add("</tr>");
            return lstDisplayMessageToUser;
        }

        private List<string> GetCPRCertifications(DataTable dt)
        {
            List<string> cprCerts = new List<string>();
            if (dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No CPR Certifications Found");
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    cprCerts.Add("<tr>");
                    cprCerts.Add("<td>");

                    cprCerts.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetBool("IsCPRCertified", dr) ? "Yes" : "No"));
                    cprCerts.Add(string.Format("<span>{0}</span>",
                        !string.IsNullOrEmpty(ObjectControllerHelper.GetString("ExpirationDate", dr)) ?
                        ObjectControllerHelper.GetDateTime("ExpirationDate", dr).ToString(dateFormat) : ""));

                    cprCerts.Add("</td>");
                    cprCerts.Add("</tr>");
                }
            }
            return cprCerts;
        }

        private List<string> GetNursingCertifications(DataTable dt)
        {
            List<string> nursingCerts = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No First Aid Certifications Found");
            foreach (DataRow dr in dt.Rows)
            {
                nursingCerts.Add("<tr>");
                nursingCerts.Add("<td>");

                nursingCerts.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("Certification_ID", dr)));
                nursingCerts.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("Received_From", dr)));
                nursingCerts.Add(string.Format("<span>{0}</span>",
                     !string.IsNullOrEmpty(ObjectControllerHelper.GetString("EXPIRATION", dr)) ?
                     ObjectControllerHelper.GetDateTime("EXPIRATION", dr).ToString(dateFormat) : ""));

                nursingCerts.Add("</td>");
                nursingCerts.Add("</tr>");
            }

            return nursingCerts;
        }

        private List<string> GetCategoriesOfService(DataTable dt)
        {
            List<string> categoriesOfSvc = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No Category of service information found.");
            foreach (DataRow dr in dt.Rows)
            {
                categoriesOfSvc.Add("<tr>");
                categoriesOfSvc.Add("<td>");

                categoriesOfSvc.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("MAX_CATEGORY_OF_SERVICE_TYPE", dr)));
                categoriesOfSvc.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("CATEGORY_OF_SERVICE_TYPE_NAME", dr)));
                categoriesOfSvc.Add(string.Format("<span>{0}</span></td><td>",
                     !string.IsNullOrEmpty(ObjectControllerHelper.GetString("StartDate", dr)) ?
                     ObjectControllerHelper.GetDateTime("StartDate", dr).ToString(dateFormat) : ""));
                categoriesOfSvc.Add(string.Format("<span>{0}</span>",
                     !string.IsNullOrEmpty(ObjectControllerHelper.GetString("EndDate", dr)) ?
                    ObjectControllerHelper.GetDateTime("EndDate", dr).ToString(dateFormat) : ""));

                categoriesOfSvc.Add("</td>");
                categoriesOfSvc.Add("</tr>");
            }

            return categoriesOfSvc;
        }

        private List<string> GetProviderIdentifiers(DataTable dt)
        {
            List<string> providerIdentifiers = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No  information found.");
            foreach (DataRow dr in dt.Rows)
            {
                providerIdentifiers.Add("<tr>");
                providerIdentifiers.Add("<td>");

                providerIdentifiers.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("ENTITY_Name", dr)));
                providerIdentifiers.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("DBA_NAME", dr)));
                providerIdentifiers.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NPI", dr)));
                providerIdentifiers.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("TAX_ID", dr)));
                providerIdentifiers.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetString("MEDICAID_ID", dr)));

                providerIdentifiers.Add("</td>");
                providerIdentifiers.Add("</tr>");
            }

            return providerIdentifiers;
        }

        private List<string> GetOwners(DataTable dt)
        {
            List<string> owners = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No  information found.");
            foreach (DataRow dr in dt.Rows)
            {
                owners.Add("<tr>");
                owners.Add("<td>");

                owners.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("OWNER_TYPE_NAME", dr)));
                owners.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NAME", dr)));
                owners.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("TITLE", dr)));
                owners.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetString("PERCENTAGE_OF_OWNERSHIP", dr)));

                owners.Add("</td>");
                owners.Add("</tr>");
            }

            return owners;
        }
        private List<string> GetRealestateOwners(DataTable dt)
        {
            List<string> owners = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No  information found.");
            foreach (DataRow dr in dt.Rows)
            {
                owners.Add("<tr>");
                owners.Add("<td>");

                owners.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("OWNER_TYPE_NAME", dr)));
                owners.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NAME", dr)));
                owners.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetString("PERCENTAGE_OF_OWNERSHIP", dr)));

                owners.Add("</td>");
                owners.Add("</tr>");
            }

            return owners;
        }
        private List<string> GetAdditionalClouser(DataTable dt)
        {
            List<string> owners = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No  information found.");
            foreach (DataRow dr in dt.Rows)
            {
                owners.Add("<tr>");
                owners.Add("<td>");

                owners.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("OWNER_TYPE_NAME", dr)));
                owners.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NAME", dr)));
                owners.Add("</td>");
                owners.Add("</tr>");
            }

            return owners;
        }
        private List<string> GetAdditionalAddresses(DataTable dt)
        {
            List<string> addlAddresses = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                addlAddresses.Add("<tr>");
                addlAddresses.Add("<td>");

                addlAddresses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("ADDRESS_DESC", dr)));
                addlAddresses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("ADDRESS1", dr)));
                addlAddresses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("ADDRESS2", dr)));
                addlAddresses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("CITY", dr)));
                addlAddresses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("STATE", dr)));
                addlAddresses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("ZIP", dr)));
                addlAddresses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("EXT_ZIP", dr)));
                addlAddresses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("COUNTY", dr)));
                addlAddresses.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("PHONE", dr)));
                addlAddresses.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetString("PHONE_EXT", dr)));

                addlAddresses.Add("</td>");
                addlAddresses.Add("</tr>");
            }

            return addlAddresses;
        }
        private List<string> GetOwnerXREF(DataTable dt)
        {
            List<string> OwnerXREF = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                OwnerXREF.Add("<tr>");
                OwnerXREF.Add("<td>");

                OwnerXREF.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("OWNER1", dr)));
                OwnerXREF.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("RELATIONSHIP_NAME", dr)));
                OwnerXREF.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("OWNER2", dr)));


                OwnerXREF.Add("</td>");
                OwnerXREF.Add("</tr>");
            }

            return OwnerXREF;
        }
        private List<string> GetOwnerOther(DataTable dt)
        {
            List<string> OwnerOther = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                OwnerOther.Add("<tr>");
                OwnerOther.Add("<td>");

                OwnerOther.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("PDMS_PARTY_TYPE_NAME", dr)));

                OwnerOther.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NAME", dr)));

                OwnerOther.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("TAX_ID", dr)));

                OwnerOther.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("PERCENTAGE_OF_OWNERSHIP", dr)));


                OwnerOther.Add("</td>");
                OwnerOther.Add("</tr>");
            }

            return OwnerOther;
        }
        private List<string> GetOwnerConviction(DataTable dt)
        {
            List<string> OwnerConviction = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                OwnerConviction.Add("<tr>");
                OwnerConviction.Add("<td>");

                OwnerConviction.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("OWNER_NAME", dr)));
                OwnerConviction.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetDateTime("DOB", dr).ToShortDateString()));
                OwnerConviction.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("TAX_ID", dr)));
                OwnerConviction.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("MATTER_OF_OFFENSE", dr)));


                OwnerConviction.Add("</td>");
                OwnerConviction.Add("</tr>");
            }

            return OwnerConviction;
        }

        private List<string> GetMalpracticeClaims(DataTable dt)
        {
            List<string> malpracticeClaims = new List<string>();
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                return this.CommonHtmlMapperWhenNoRecordFound("No Mal Practices information found");
            foreach (DataRow dr in dt.Rows)
            {
                malpracticeClaims.Add("<tr>");
                malpracticeClaims.Add("<td>");

                malpracticeClaims.Add(string.Format("<span>{0}</span></td><td>",
                    !string.IsNullOrEmpty(ObjectControllerHelper.GetString("DateOccurance", dr)) ?
                    ObjectControllerHelper.GetDateTime("DateOccurance", dr).ToString(dateFormat) : ""));

                malpracticeClaims.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("Claim_Status_Name", dr)));
                malpracticeClaims.Add(string.Format("<span>{0}</span></td><td",
                    !string.IsNullOrEmpty(ObjectControllerHelper.GetString("DateCLAIMFiled", dr)) ?
                    ObjectControllerHelper.GetDateTime("DateCLAIMFiled", dr).ToString(dateFormat) : ""));
                malpracticeClaims.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("ProfessionalCarrier", dr)));
                malpracticeClaims.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("SETTLEMENT_AMOUNT", dr)));
                malpracticeClaims.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("Resolution_Desc", dr)));
                malpracticeClaims.Add(string.Format("<span>{0}</span>",
                  !string.IsNullOrEmpty(ObjectControllerHelper.GetString("CLAIM_SETTLED_DATE", dr)) ?
                  ObjectControllerHelper.GetDateTime("CLAIM_SETTLED_DATE", dr).ToString(dateFormat) : ""));
                malpracticeClaims.Add("</td>");
                malpracticeClaims.Add("</tr>");
            }

            return malpracticeClaims;
        }

        private List<string> GetDMEProfessionals(DataTable dt)
        {
            List<string> dmeProfessionals = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                dmeProfessionals.Add("<tr>");
                dmeProfessionals.Add("<td>");

                dmeProfessionals.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("Name", dr)));
                dmeProfessionals.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetBool("IsBackgroundCheck", dr) ? "True" : "False"));
                dmeProfessionals.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetBool("IsProfessionalLicense", dr) ? "True" : "False"));

                dmeProfessionals.Add("</td>");
                dmeProfessionals.Add("</tr>");
            }

            return dmeProfessionals;
        }

        private List<string> GetDMEProducts(DataTable dt)
        {
            List<string> dmeProducts = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                dmeProducts.Add("<tr>");
                dmeProducts.Add("<td>");

                dmeProducts.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("DME_PRODUCT_SERVICE_CATEGORY_TYPE_NAME", dr)));
                dmeProducts.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetString("DME_PRODUCT_SERVICE_SUB_CATEGORY_TYPE_NAME", dr)));

                dmeProducts.Add("</td>");
                dmeProducts.Add("</tr>");
            }

            return dmeProducts;
        }

        private List<string> GetDMEAccreditations(DataTable dt)
        {
            List<string> dmeAccreditations = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                dmeAccreditations.Add("<tr>");
                dmeAccreditations.Add("<td>");

                dmeAccreditations.Add(string.Format("<span><input type=\"checkbox\" {0} /></span></td><td>", ObjectControllerHelper.GetInt("REG_DME_ACCREDITATION_AGENCY_ID", dr) > 0 ? "Checked" : "Unchecked"));
                dmeAccreditations.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetString("ACCREDITATION_AGENCY_DESC", dr)));

                dmeAccreditations.Add("</td>");
                dmeAccreditations.Add("</tr>");
            }

            return dmeAccreditations;
        }

        private List<string> GetOwnerSubcontractors(DataTable dt)
        {
            List<string> ownerSubcontractors = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                ownerSubcontractors.Add("<tr>");
                ownerSubcontractors.Add("<td>");

                ownerSubcontractors.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NAME", dr)));
                ownerSubcontractors.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetString("TAX_ID", dr)));

                ownerSubcontractors.Add("</td>");
                ownerSubcontractors.Add("</tr>");
            }

            return ownerSubcontractors;
        }

        private List<string> GetOwnerTransactions(DataTable dt)
        {
            List<string> ownerTransactions = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                ownerTransactions.Add("<tr>");
                ownerTransactions.Add("<td>");

                ownerTransactions.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("PERSON_NAME", dr)));
                ownerTransactions.Add(string.Format("<span>{0}</span></td><td>",
                     !string.IsNullOrEmpty(ObjectControllerHelper.GetString("TRANSACTION_DATE", dr)) ?
                     ObjectControllerHelper.GetDateTime("TRANSACTION_DATE", dr).ToString(dateFormat) : ""));
                ownerTransactions.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetDecimal("TRANSACTION_AMOUNT", dr).ToString("C")));

                ownerTransactions.Add("</td>");
                ownerTransactions.Add("</tr>");
            }

            return ownerTransactions;
        }

        private List<string> GetProviderConvictions(DataTable dt)
        {
            List<string> providerConvictions = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                providerConvictions.Add("<tr>");
                providerConvictions.Add("<td>");

                providerConvictions.Add(string.Format("<span>{0}</span></td><td>", ObjectControllerHelper.GetString("NAME", dr)));
                providerConvictions.Add(string.Format("<span>{0}</span></td><td>",
                     !string.IsNullOrEmpty(ObjectControllerHelper.GetString("BIRTH_DATE", dr)) ?
                    ObjectControllerHelper.GetDateTime("BIRTH_DATE", dr).ToString(dateFormat) : ""));
                providerConvictions.Add(string.Format("<span>{0}</span>", ObjectControllerHelper.GetString("TAX_ID", dr)));

                providerConvictions.Add("</td>");
                providerConvictions.Add("</tr>");
            }

            return providerConvictions;
        }

        private string[] MergeICFIIDTemplateWithRegData(int regID, string submitDate, string day, string month, string year, string providerSignDate, string psSignDate, string providerNumber,
                                   string providerName, string psCommissioner, string contractStartDate, string contractEndDate, string administrator, string addrLine1, string addrLine2, string addrCityStateZip, string templatePath, string templateName)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            string[] finalDoc = new string[0];

            try
            {
                List<StringBuilder> sbDocPages = new List<StringBuilder>();
                sbDocPages.Add(new StringBuilder());

                int lineNbr = 0, pageNbr = 0;

                string templateNameWithPath = templatePath + templateName;

                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(templateNameWithPath))
                {
                    string lineOfData;
                    // Read and display lines from the file until the end of the file is reached.
                    while ((lineOfData = sr.ReadLine()) != null)
                    {
                        lineOfData = ReplaceTag(lineOfData, "DAY", day);
                        lineOfData = ReplaceTag(lineOfData, "MONTH", month);
                        lineOfData = ReplaceTag(lineOfData, "YEAR", year);
                        lineOfData = ReplaceTag(lineOfData, "PROVIDER_NUMBER", providerNumber); //npi
                        lineOfData = ReplaceTag(lineOfData, "PROVIDER_NAME", providerName);
                        lineOfData = ReplaceTag(lineOfData, "PS_COMMISSIONER", psCommissioner);
                        lineOfData = ReplaceTag(lineOfData, "CONTRACT_START_DATE", contractStartDate);
                        lineOfData = ReplaceTag(lineOfData, "CONTRACT_END_DATE", contractEndDate);
                        lineOfData = ReplaceTag(lineOfData, "PROVIDER_SIGN_DATE", providerSignDate);
                        lineOfData = ReplaceTag(lineOfData, "ADMINISTRATOR_NAME", administrator);
                        lineOfData = ReplaceTag(lineOfData, "PS_SIGN_DATE", psSignDate);
                        lineOfData = ReplaceTag(lineOfData, "CONTACT_ADDR_1", addrLine1);
                        lineOfData = ReplaceTag(lineOfData, "CONTACT_ADDR_2", addrLine2);
                        lineOfData = ReplaceTag(lineOfData, "CONTACT_CITY_STATE_ZIP", addrCityStateZip);

                        if (FindTag(lineOfData, "PAGE_NUMBER"))
                        {
                            //Force a page break
                            lineOfData = ReplaceTag(lineOfData, "PAGE_NUMBER", string.Empty); //TODO:  verify handling
                            AddLine(ref sbDocPages, ref pageNbr, ref lineNbr, lineOfData, true);
                        }
                        else
                        {
                            AddLine(ref sbDocPages, ref pageNbr, ref lineNbr, lineOfData, false);
                        }
                    }
                }
                if (sbDocPages[sbDocPages.Count - 1].Length == 0)
                {
                    sbDocPages.RemoveAt(sbDocPages.Count - 1);
                }
                finalDoc = new string[sbDocPages.Count];
                pageNbr = 0;

                foreach (StringBuilder sbPage in sbDocPages)
                {
                    finalDoc[pageNbr] = sbPage.ToString();
                    pageNbr++;
                }

                return finalDoc;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Error encountered during Generation of ICF/IID Contract pdf [RegID-" + regID.ToString() + "]: " + ex.Message, Logging.LogPriority.Error);
            }
            return finalDoc;
        }

        /*Used for both the DIDD Contract and Amendment */
        private string[] MergeDIDDTemplateWithRegData(int regID, string appNbr, string appSuffix, string providerName, string effectiveDate, string psSignDate,
                                    string diddSignDate, string providerSignDate, string diddCommissonerName, string psCommissonerName,
                                    string taxID, string contractStartDate, string contractEndDate, string signatory, string legalName, string addrLine1, string addrLine2,
                                    string addrCity, string addrState, string addrZip, string addrPhone,
                                    string addrFax, List<string> services, string templatePath, string templateName)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            string[] finalDoc = new string[0];

            try
            {
                string formattedServices = FormatList(services);

                List<StringBuilder> sbDocPages = new List<StringBuilder>();
                sbDocPages.Add(new StringBuilder());

                int lineNbr = 0, pageNbr = 0;

                string templateNameWithPath = templatePath + templateName;

                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(templateNameWithPath))
                {
                    string lineOfData;
                    // Read and display lines from the file until the end of the file is reached.
                    while ((lineOfData = sr.ReadLine()) != null)
                    {
                        lineOfData = ReplaceTag(lineOfData, "APPLICATION_NUMBER", appNbr);
                        lineOfData = ReplaceTag(lineOfData, "REFERRAL_SUFFIX", appSuffix);
                        lineOfData = ReplaceTag(lineOfData, "PROVIDER_NAME", providerName);
                        lineOfData = ReplaceTag(lineOfData, "LEGAL_NAME", providerName);
                        lineOfData = ReplaceTag(lineOfData, "NAME", providerName);
                        lineOfData = ReplaceTag(lineOfData, "SERVICE_LIST", formattedServices);
                        lineOfData = ReplaceTag(lineOfData, "EFFECTIVE_DATE", effectiveDate);
                        lineOfData = ReplaceTag(lineOfData, "PS_SIGN_DATE", psSignDate);
                        lineOfData = ReplaceTag(lineOfData, "PS_COMMISSIONER", psCommissonerName);
                        lineOfData = ReplaceTag(lineOfData, "PROVIDER_SIGN_DATE", providerSignDate);
                        lineOfData = ReplaceTag(lineOfData, "SIGNATORY", signatory);
                        lineOfData = ReplaceTag(lineOfData, "DIDD_SIGN_DATE", diddSignDate);
                        lineOfData = ReplaceTag(lineOfData, "DIDD_COMMISSIONER", diddCommissonerName);
                        lineOfData = ReplaceTag(lineOfData, "TAX_ID", taxID);
                        lineOfData = ReplaceTag(lineOfData, "CONTACT_ADDR_1", addrLine1);
                        lineOfData = ReplaceTag(lineOfData, "CONTACT_ADDR_2", addrLine2);
                        lineOfData = ReplaceTag(lineOfData, "CONTACT_CITY", addrCity);
                        lineOfData = ReplaceTag(lineOfData, "CONTACT_STATE", addrState);
                        lineOfData = ReplaceTag(lineOfData, "CONTACT_ZIP", addrZip);
                        lineOfData = ReplaceTag(lineOfData, "CONTACT_PHONE", addrPhone);
                        lineOfData = ReplaceTag(lineOfData, "CONTACT_FAX", addrFax);
                        lineOfData = ReplaceTag(lineOfData, "CONTRACT_START_DATE", contractStartDate);
                        lineOfData = ReplaceTag(lineOfData, "CONTRACT_END_DATE", contractEndDate);

                        if (FindTag(lineOfData, "PAGE_NUMBER"))
                        {
                            //Force a page break
                            lineOfData = ReplaceTag(lineOfData, "PAGE_NUMBER", string.Empty); //TODO:  verify handling
                            AddLine(ref sbDocPages, ref pageNbr, ref lineNbr, lineOfData, true);
                        }
                        else
                        {
                            AddLine(ref sbDocPages, ref pageNbr, ref lineNbr, lineOfData, false);
                        }
                    }
                }
                if (sbDocPages[sbDocPages.Count - 1].Length == 0)
                {
                    sbDocPages.RemoveAt(sbDocPages.Count - 1);
                }
                finalDoc = new string[sbDocPages.Count];
                pageNbr = 0;

                foreach (StringBuilder sbPage in sbDocPages)
                {
                    finalDoc[pageNbr] = sbPage.ToString();
                    pageNbr++;
                }

                return finalDoc;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Error encountered during Generation of DIDD Contract pdf [RegID-" + regID.ToString() + "]: " + ex.Message, Logging.LogPriority.Error);
            }
            return finalDoc;
        }

        //TODO:  refactor, a flavor of these methods exist in ProviderNotification.cs
        private string ReplaceTag(string input, string tag, string replace)
        {
            string rtn = input.Replace("[[" + tag + "]]", replace);
            return rtn;
        }

        public bool FindTag(string input, string tag)
        {
            return (input.IndexOf("[[" + tag + "]]") != -1);
        }

        private void AddLine(ref List<StringBuilder> sbEntireDoc, ref int pageNbr, ref int lineNbr, string lineOfData, bool forcePageBreak)
        {
            if (forcePageBreak)
            {
                sbEntireDoc.Add(new StringBuilder());
                pageNbr++;
                return;
            }

            sbEntireDoc[pageNbr].AppendLine(lineOfData);
            lineNbr++;
            if (lineNbr % 50 == 0)
            {
                sbEntireDoc.Add(new StringBuilder());
                pageNbr++;
            }
        }

        private string FormatList(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in list)
            {
                sb.AppendLine(string.Format("{0}", item));
            }
            return sb.ToString();
        }

        private bool SaveHeaderImageToTempLocation(string templatePath, string storagePath, string headerImg)
        {
            string storageDirectory = storagePath + "Temporary_HtmlFiles";

            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }
            if (storageDirectory.LastIndexOf("\\") != storageDirectory.Length - 1)
            {
                storageDirectory += "\\";
            }

            string sourceFile = string.Concat(templatePath, headerImg);
            string destFile = string.Concat(storageDirectory, headerImg);
            if (!File.Exists(destFile))
            {
                if (File.Exists(sourceFile))
                {
                    File.Copy(sourceFile, destFile);
                }
                else
                {
                    return false;
                }
            }
            else // File exists, check if the files are the same size. If not, overwrite existing
            {
                if (File.Exists(sourceFile))
                {
                    // Compare file dates before copying
                    if (File.GetLastWriteTimeUtc(sourceFile) != File.GetLastWriteTimeUtc(destFile))
                        File.Copy(sourceFile, destFile, true);
                }
                else
                    return false;
            }
            return true;
        }

        private string SaveAsHtml(int regID, string templateName, string storagePath, string[] finalDoc)
        {

            string htmlDirectory = storagePath + "Temporary_HtmlFiles";
            string shortFileName = Guid.NewGuid().ToString() + ".html";

            if (!Directory.Exists(htmlDirectory))
            {
                Directory.CreateDirectory(htmlDirectory);
            }

            if (htmlDirectory.LastIndexOf("\\") != htmlDirectory.Length - 1)
            {
                htmlDirectory += "\\";
            }

            File.WriteAllLines(htmlDirectory + shortFileName, finalDoc);
            string newFileName = RenameAttachmentFile(regID, htmlDirectory, shortFileName, templateName.Replace("Template", string.Empty));

            return newFileName;
        }

        private string RenameAttachmentFile(int regID, string path, string generatedFileName, string fileName)
        {
            string rtn = string.Empty;
            try
            {

                //filename will come in as a path/guid + .pdf
                //TODO:  what should it look like
                string suffix = string.Empty, prefix = string.Empty;
                int pos = generatedFileName.LastIndexOf(".");
                if (pos != -1) suffix = generatedFileName.Substring(pos + 1);
                pos = fileName.LastIndexOf(".");
                if (pos != -1) prefix = fileName.Substring(0, pos);
                int idx = 0;
                do
                {
                    idx += 1;
                    rtn = prefix + regID.ToString() + "_" + idx.ToString() + "." + suffix;
                }
                while (File.Exists(path + rtn));
                File.Copy(path + generatedFileName, path + rtn);
                File.Delete(path + generatedFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtn;
        }

        private bool InsertDocumentRecord(int regID, string pdfFileName, string signedBy, string name, string description)
        {
            bool rtn = false;
            try
            {
                RegistrationController.InsertRegDocument(regID, Constants.RegistrationPageType.Contracts, signedBy, name, description, pdfFileName, DateTime.Now, Constants.appWorkflowUserId, null, null, null);
                rtn = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtn;

        }
        private bool PageIsVisible(int entityTypeId, int providerTypeId, int diddReferralId, string pageName, int taxIDTypeID)
        {
            // These pages are only visible if you have a DIDD Referral Id



            if (diddReferralId > 0 && (pageName == "Substitute W4 Form" || pageName == "Substitute W9 Form"))
            {
                if (pageName == "Substitute W9 Form")
                {
                    //always show w9 for organization
                    if (entityTypeId != CON.ProviderCategoryTypeID.Individual)
                        return true;

                    //always show w9 for individual filing with EIN tax type
                    if (entityTypeId == CON.ProviderCategoryTypeID.Individual && taxIDTypeID == CON.TaxIDType.EIN)
                        return true;
                }

                if (pageName == "Substitute W4 Form")
                {
                    //never show w4 for organization
                    if (entityTypeId != CON.ProviderCategoryTypeID.Individual)
                        return false;

                    //never show w4 for individual filing with EIN tax type
                    if (entityTypeId == CON.ProviderCategoryTypeID.Individual && taxIDTypeID == CON.TaxIDType.EIN)
                        return false;
                }

                //BEGIN SERVICE SPECIFIC SETTINGS FOR HIDE/SHOW OF W9/W4.
                //Never show both

                DataSet dsServices = DIDDController.SelectDIDDReferralService(diddReferralId);
                if (ObjectControllerHelper.HasRows(dsServices))
                {
                    DataTable dt = FilterServicesForW4Display(dsServices);
                    bool showW4 = dt == null ? false : true;
                    if (pageName == "Substitute W4 Form")
                        return showW4;
                    else if (pageName == "Substitute W9 Form")
                        return !showW4;
                }
                else
                {
                    //default, but should never have an nfocus registration with 0 services
                    return false;
                }
            }
            if (diddReferralId == 0 && pageName == "Substitute W9 Form")
            {
                return true;
            }
            return false;
        }

        private bool PageIsVisible(string pageName, DataTable dtRegPageSettings)
        {
            bool rtn = false;

            DataRow rowPageSetting = dtRegPageSettings.Select(string.Format("REG_PAGE_NAME = '{0}'", pageName)).FirstOrDefault();
            if (rowPageSetting != null)
            {
                rtn = ObjectControllerHelper.GetBool("IS_VISIBLE", rowPageSetting);
            }
            return rtn;
        }


        private bool SectionIsVisible(string sectionName, DataTable dtRegPageSectionSettings)
        {
            bool rtn = false;

            DataRow rowPageSectionSetting = dtRegPageSectionSettings.Select(string.Format("REG_Section_NAME = '{0}'", sectionName)).FirstOrDefault();
            if (rowPageSectionSetting != null)
            {
                rtn = ObjectControllerHelper.GetBool("IS_VISIBLE", rowPageSectionSetting);
            }
            return rtn;
        }


        public static bool SectionIsVisible(int applicationTypeID, int entityTypeId, int providerTypeId, int diddReferralId, string pageName, string pageSection, string taskName = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("ENTITY_TYPE_ID", entityTypeId.ToString());
            if (providerTypeId > 0) parms.Add("PROVIDER_TYPE_ID", providerTypeId.ToString());
            parms.Add("REG_PAGE_NAME", pageName);
            if (!string.IsNullOrEmpty(pageSection)) parms.Add("REG_PAGE_SECTION", pageSection);
            if (!string.IsNullOrEmpty(taskName)) parms.Add("TASK_NAME", taskName);
            if (applicationTypeID > 0) parms.Add("APPLICATION_TYPE_ID", applicationTypeID.ToString());
            DataSet ds = RegistrationController.SelectRegistrationDataWithParams("usp_SelectREG_PAGE_SECTION_SETTING", parms);
            // If there is not an entry in the table then there is no exclusion and thereby Page is visible
            if (!ObjectControllerHelper.HasRows(ds)) return true;

            return ObjectControllerHelper.GetBool("IS_VISIBLE", ds.Tables[0].Rows[0]);
        }


        private bool SectionIsVisible(int entityTypeId, int providerTypeId, int diddReferralId, string pageName, int taxIDTypeID)
        {
            // These pages are only visible if you have a DIDD Referral Id



            if (diddReferralId > 0 && (pageName == "Substitute W4 Form" || pageName == "Substitute W9 Form"))
            {
                if (pageName == "Substitute W9 Form")
                {
                    //always show w9 for organization
                    if (entityTypeId != CON.ProviderCategoryTypeID.Individual)
                        return true;

                    //always show w9 for individual filing with EIN tax type
                    if (entityTypeId == CON.ProviderCategoryTypeID.Individual && taxIDTypeID == CON.TaxIDType.EIN)
                        return true;
                }

                if (pageName == "Substitute W4 Form")
                {
                    //never show w4 for organization
                    if (entityTypeId != CON.ProviderCategoryTypeID.Individual)
                        return false;

                    //never show w4 for individual filing with EIN tax type
                    if (entityTypeId == CON.ProviderCategoryTypeID.Individual && taxIDTypeID == CON.TaxIDType.EIN)
                        return false;
                }

                //BEGIN SERVICE SPECIFIC SETTINGS FOR HIDE/SHOW OF W9/W4.
                //Never show both

                DataSet dsServices = DIDDController.SelectDIDDReferralService(diddReferralId);
                if (ObjectControllerHelper.HasRows(dsServices))
                {
                    DataTable dt = FilterServicesForW4Display(dsServices);
                    bool showW4 = dt == null ? false : true;
                    if (pageName == "Substitute W4 Form")
                        return showW4;
                    else if (pageName == "Substitute W9 Form")
                        return !showW4;
                }
                else
                {
                    //default, but should never have an nfocus registration with 0 services
                    return false;
                }
            }
            if (diddReferralId == 0 && pageName == "Substitute W9 Form")
            {
                return true;
            }
            return false;
        }



        public static bool PageIsVisible(int applicationTypeID, int entityTypeId, int providerTypeId, int diddReferralId, string pageName, string pageSection, string taskName = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("ENTITY_TYPE_ID", entityTypeId.ToString());
            if (providerTypeId > 0) parms.Add("PROVIDER_TYPE_ID", providerTypeId.ToString());
            parms.Add("REG_PAGE_NAME", pageName);
            if (!string.IsNullOrEmpty(pageSection)) parms.Add("REG_PAGE_SECTION", pageSection);
            if (!string.IsNullOrEmpty(taskName)) parms.Add("TASK_NAME", taskName);
            if (applicationTypeID > 0) parms.Add("APPLICATION_TYPE_ID", applicationTypeID.ToString());
            DataSet ds = RegistrationController.SelectRegistrationDataWithParams("usp_SelectREG_PAGE_SETTING", parms);
            // If there is not an entry in the table then there is no exclusion and thereby Page is visible
            if (!ObjectControllerHelper.HasRows(ds)) return true;

            return ObjectControllerHelper.GetBool("IS_VISIBLE", ds.Tables[0].Rows[0]);
        }

        private DataTable FilterServicesForW4Display(DataSet ds)
        {
            //TODO, these are hard-coded and should be in an appsetting
            string selectPart = "WAIVER_TYPE_CODE IN (5665, 1691, 2500, 6700, 4456, 4475, 1113, 9233, 2456)";

            DataTable dt = ds.Tables[0];
            if (dt.Select(selectPart).Count() > 0)
            {
                return dt.Select(selectPart).CopyToDataTable();
            }
            else
            {
                return null;
            }
        }

        public bool Generate1099(int regID, string taxId, string fedWithhold, string payments, string year, string userName, string appPath, out string pdfFileName, string payerName = "OHIO DEPARTMENT OF MEDICAID", string payerAddress1 = "Bureau of Provider Services", string payerAddress2 = "P.O. Box 182709", string payerCity = "COLUMBUS", string payerState = "OH", string payerZip = "43218-2709")
        {
            pdfFileName = string.Empty;
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string logHeader = string.Format("Error generating 1099 PDF. Reg Id- {0}:", regID.ToString());

            string templatePath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
#if DEBUG
            templatePath = @"C:\Users\CA_OHPNM_DEV_11\source\repos\p3\ohpnm-src-p3\PDMS\ProviderDataManagementSystemService\Documents\";
#endif
            if (string.IsNullOrEmpty(templatePath))
            {
                log.CreateLogEntry(string.Format("{0} Creation of PDF failed", logHeader), Logging.LogPriority.Error);
                return false;
            }

            try
            {
                string documentName = "1099 Template";
                string templateName = GetTemplateName(documentName);
                if (string.IsNullOrEmpty(templateName))
                {
                    log.CreateLogEntry(string.Format("{0} Document Type \"{1}\" not found", logHeader, documentName), Logging.LogPriority.Error);
                    return false;
                }

                DataRow provRow = GetProviderInfo(regID);

                string name = "";
                string address = "";
                string cityStateZip = "";


                if (provRow != null)
                {
                    taxId = provRow["TAX_ID"].ToString();
                    name = provRow["NAME"].ToString();

                    address = provRow["CONTACT_ADDRESS1"].ToString();
                    string address2 = provRow["CONTACT_ADDRESS2"].ToString();
                    if (!string.IsNullOrEmpty(address2))
                    {
                        address = address.Trim() + "<br>" + address2;
                    }

                    cityStateZip = provRow["CONTACT_CITY"].ToString() + ", " + provRow["CONTACT_STATE"].ToString() + " " + provRow["CONTACT_ZIP"].ToString();
                    string zipExt = provRow["CONTACT_EXT_ZIP"].ToString();
                    if (!string.IsNullOrEmpty(zipExt))
                    {
                        cityStateZip = cityStateZip.Trim() + "-" + zipExt;
                    }
                }

                //PDF E2E
                string storagePath = AppSettings.Get("RegistrationApplication_Path", string.Empty);
#if DEBUG
                storagePath = @"C:\Temp\OHPNM\";
#endif
                if (Environment.MachineName.ToUpper().Contains("WSAMZN"))
                {
                    templatePath = HttpRuntime.AppDomainAppPath + @"Documents\";
                    storagePath = Path.GetTempPath();
                }
                string templateFile = templatePath + templateName;
                string html1099 = File.ReadAllText(templateFile);

                // replace fields with values
                html1099 = html1099.Replace("*watermark*", appPath + @"Images/Pdf_watermark.png");
                html1099 = html1099.Replace("*YYYY*", year);
                html1099 = html1099.Replace("*fedtax*", fedWithhold);
                html1099 = html1099.Replace("*payamt*", payments);
                html1099 = html1099.Replace("*recid*", taxId);
                html1099 = html1099.Replace("*name*", name);
                html1099 = html1099.Replace("*address*", address);
                html1099 = html1099.Replace("*city,state,zip*", cityStateZip);
                html1099 = html1099.Replace("*username*", userName);
                html1099 = html1099.Replace("*printdate*", DateTime.Now.ToString());
                html1099 = html1099.Replace("*payername*", payerName);
                html1099 = html1099.Replace("*payeraddress1*", payerAddress1);
                html1099 = html1099.Replace("*payeraddress2*", payerAddress2);
                html1099 = html1099.Replace("*payercity*", payerCity);
                html1099 = html1099.Replace("*payerstate*", payerState);
                html1099 = html1099.Replace("*payerzip*", payerZip);
                if (payerName.Equals("OHIO DEPARTMENT OF MEDICAID") && payerAddress1.Equals("Bureau of Provider Services"))
                {
                    html1099 = html1099.Replace("*paidby", "THE STATE OF OHIO 31-1334825");
                }
                else
                {
                    html1099 = html1099.Replace("*paidby", payerName);
                }

                // Write to temp location
                storagePath = Path.Combine(storagePath, "Temporary_Files");

                string tempFile = storagePath + @"\" + regID.ToString() + "_1099.html";
                File.WriteAllText(tempFile, html1099);
                // Create PDF from temp file

                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Url = tempFile;
                PdfOutput pdfOut = new PdfOutput();
                pdfFileName = tempFile.Replace(".html", ".pdf");
                pdfOut.OutputFilePath = pdfFileName;
                PdfConvert.ConvertHtmlToPdf(pdfDoc, pdfOut);

                return true;

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.Message), Logging.LogPriority.Error);
            }
            return false;
        }
        #endregion
    }
}
