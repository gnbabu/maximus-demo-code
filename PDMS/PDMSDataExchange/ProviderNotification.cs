using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using mt = MAXIMUS.DataExchange.PDMS.TN_MMIS_Service_Test;

namespace MAXIMUS.DataExchange.PDMS
{
    [Serializable]
    public class MMISMedicaidId
    {
        public string MedicaidId { get; set; }
        public bool IsGroup { get; set; }
        public string Address { get; set; }
        public string GroupMedicaidId { get; set; }
        public string GroupName { get; set; }
        public bool Done { get; set; }

        public MMISMedicaidId()
        {
            Done = false;
            Address = GroupName = string.Empty;
        }
    }

    public class ProviderNotification
    {
        private Guid m_threadId;
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

        public ProviderNotification()
        {
        }

        public ProviderNotification(Guid threadId)
        {
            this.ThreadId = threadId;
        }

        private bool SetTemplate(string documentName, ref string subject, ref string templateName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_NAME", DbType.String, documentName, true));
                DataSet document = DataAccess.ExecuteStoredProcedure("sp_SelectDocumentTypes", parameters, "DOCUMENTS");
                if (!ObjectControllerHelper.HasRows(document)) return false;
                subject = ObjectControllerHelper.GetString("DOCUMENT_SUBJECT", document.Tables[0].Rows[0]);
                templateName = ObjectControllerHelper.GetString("DOCUMENT_FILE_NAME", document.Tables[0].Rows[0]);
                return true;
            }
            catch (Exception ex)
            {
                // Create log object
                string logMsg = String.Format(Constants.LogString.MethodSignature,
                    MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Logging log = new Logging(this.ThreadId, logMsg);
                log.CreateLogEntry("Document Name: " + documentName + " - " + ex.Message, Logging.LogPriority.Error);
            }
            return false;
        }

        // Get the Group Name from the response groups given the Medicaid Id
        private string GetGroupName(string medicaidId, TN_MMIS_Service.ProviderCaqhGetResponse response)
        {
            string rtn = string.Empty;
            try
            {
                foreach (TN_MMIS_Service.ProviderGroup group in response.groups)
                {
                    if (group.medicareId == medicaidId)
                    {
                        rtn = group.serviceName;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Create log object
                string logMsg = String.Format(Constants.LogString.MethodSignature,
                    MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Logging log = new Logging(this.ThreadId, logMsg);
                log.CreateLogEntry("MedicaidId: " + medicaidId.ToString() + " - " + ex.Message, Logging.LogPriority.Error);
            }
            return rtn;
        }

        // Get the Address based upon the Medicaid Id
        private string GetMedicaidIdAddress(string medicaidId)
        {
            string rtn = string.Empty;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MedicaidId", DbType.String, medicaidId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectAddressByMedicaidId", parameters, "MedicaidIdAddress");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    rtn = ObjectControllerHelper.GetString("Address", ds.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                // Create log object
                string logMsg = String.Format(Constants.LogString.MethodSignature,
                    MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Logging log = new Logging(this.ThreadId, logMsg);
                log.CreateLogEntry("MedicaidId: " + medicaidId.ToString() + " - " + ex.Message, Logging.LogPriority.Error);
            }
            return rtn;
        }

        // Load the Medicaid Ids from the response that do not exist for the Party Id
        public void LoadMissingMedicaidIds(int partyId, mt.ProviderCaqhGetResponse response, 
            ref List<MMISMedicaidId> medicaidIds)
        {
            //try
            //{
            //    string individualList = string.Empty, groupList = string.Empty;
            //    if (response.groups.Count() == 0) individualList = response.medicareId;
            //    else
            //    {
            //        foreach (TN_MMIS_Service.ProviderGroup group in response.groups)
            //        {
            //            if (!string.IsNullOrEmpty(group.medicareId)) groupList += group.medicareId + ",";
            //        }
            //        if (!string.IsNullOrEmpty(groupList)) groupList = groupList.Substring(0, groupList.Length - 1);
            //    }

            //    List<SqlParameter> parameters = new List<SqlParameter>();
            //    parameters.Add(SqlParms.CreateParameter("PartyId", DbType.Int32, partyId, true));
            //    parameters.Add(SqlParms.CreateParameter("IndividualList", DbType.String, individualList, true));
            //    parameters.Add(SqlParms.CreateParameter("GroupList", DbType.String, groupList, true));
            //    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectMissingMedicaidIds", parameters, "MedicaidIds");
            //    if (!ObjectControllerHelper.HasRows(ds)) return;
            //    foreach (DataRow row in ds.Tables[0].Rows)
            //    {
            //        MMISMedicaidId med = new MMISMedicaidId();
            //        med.IsGroup = ObjectControllerHelper.GetBool("IsGroup", row);
            //        if (med.IsGroup)
            //        {
            //            med.MedicaidId = response.medicareId;                           // Base Medicaid Id
            //            med.GroupMedicaidId = ObjectControllerHelper.GetString("MEDICAID_ID", row);     // Group Medicaid Id
            //            med.GroupName = GetGroupName(med.GroupMedicaidId, response);
            //        }
            //        else med.MedicaidId = ObjectControllerHelper.GetString("MEDICAID_ID", row);         // Individual Medicaid Id
            //        medicaidIds.Add(med);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Create log object
            //    string logMsg = String.Format(Constants.LogString.MethodSignature,
            //        MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            //    Logging log = new Logging(this.ThreadId, logMsg);
            //    log.CreateLogEntry("PartyId: " + partyId.ToString() + " - " + ex.Message, Logging.LogPriority.Error);
            //}
        }

        private void AddLine(ref List<StringBuilder> bld, ref int page, ref int line, string data)
        {
            bld[page].AppendLine(data);
            line++;
            if (line % 50 == 0)
            {
                bld.Add(new StringBuilder());
                page++;
            }
        }

        private string RenameAttachmentFile(int partyId, string path, string attachmentFileName, string newFileName)
        {
            string rtn = string.Empty;
            try
            {
                string suffix = string.Empty, prefix = string.Empty;
                int pos = attachmentFileName.LastIndexOf(".");
                if (pos != -1) suffix = attachmentFileName.Substring(pos + 1);
                pos = newFileName.LastIndexOf(".");
                if (pos != -1) prefix = newFileName.Substring(0, pos);
                int idx = 0;
                do
                {
                    idx += 1;
                    rtn = prefix + partyId.ToString() + "_" + idx.ToString() + "." + suffix;
                }
                while (File.Exists(path + rtn));
                File.Copy(path + attachmentFileName, path + rtn);
                File.Delete(path + attachmentFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtn;
        }

        private bool CreatePDFAttachment(int toPartyId, bool isGroup, string indOrBaseMedicaidId, 
            List<MMISMedicaidId> medicaidIds, string templatesDirectory, ref string pdfDirectory, string npi, 
            DateTime effectiveDate, ref string attachmentFileName)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                List<StringBuilder> bld = new List<StringBuilder>();
                bld.Add(new StringBuilder());

                string documentName;
                if (isGroup) documentName = "Welcome Letter Template Groups";
                else documentName = "Welcome Letter Template Sole Proprietor";
                string subject = string.Empty, templateName = string.Empty;
                SetTemplate(documentName, ref subject, ref templateName);
                if (templatesDirectory.LastIndexOf("\\") != templatesDirectory.Length - 1) templatesDirectory += "\\";
                string fileName = templatesDirectory + templateName;
                Notification notify = new Notification();
                List<string> groups = new List<string>();
                foreach (MMISMedicaidId med in medicaidIds)
                {
                    if (med.IsGroup && !med.Done)
                    {
                        if (groups.Find(itm => itm.ToString() == med.GroupName) == null)
                            groups.Add(med.GroupName);
                    }
                }
                string[] addresses = null;
                if (medicaidIds.Find(item => !item.IsGroup) != null)
                {
                    string address = GetMedicaidIdAddress(medicaidIds.Find(item => !item.IsGroup).MedicaidId); 
                    addresses = address.Split('|');
                }
                int line = 0, page = 0;
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string data;
                    // Read and display lines from the file until the end of the file is reached.
                    while ((data = sr.ReadLine()) != null)
                    {
                        data = notify.ReplaceTag(data, "Date", DateTime.Now.ToString("MM/dd/yyyy"));
                        data = notify.ReplaceTag(data, "NPI", npi);
                        data = notify.ReplaceTag(data, "MedicaidID", indOrBaseMedicaidId);
                        data = notify.ReplaceTag(data, "EffectiveDate", effectiveDate.ToString("MM/dd/yyyy"));
                        bool showGroups = false, showAddresses = false;
                        if (groups.Count > 0 && notify.FindTag(data, "Groups"))
                        {
                            data = notify.ReplaceTag(data, "Groups", groups[0]);
                            AddLine(ref bld, ref page, ref line, data);
                            showGroups = true;
                        }
                        else if (addresses.Length > 0 && notify.FindTag(data, "PracticeLocationAddress"))
                        {
                            data = notify.ReplaceTag(data, "PracticeLocationAddress", addresses[0]);
                            AddLine(ref bld, ref page, ref line, data);
                            showAddresses = true;
                        }
                        else AddLine(ref bld, ref page, ref line, data);
                        // Show more of the Groups or Addresses
                        if (showGroups && groups.Count > 1)
                        {
                            for (int i = 1; i < groups.Count; i++) AddLine(ref bld, ref page, ref line, groups[i]);
                        }
                        else if (showAddresses && addresses.Length > 1)
                        {
                            for (int i = 1; i < addresses.Length; i++) AddLine(ref bld, ref page, ref line, addresses[i]);
                        }
                    }
                }
                if (bld[bld.Count - 1].Length == 0)
                {
                    bld.RemoveAt(bld.Count - 1);
                }
                string[] final = new string[bld.Count];
                page = 0;
                foreach (StringBuilder itm in bld)
                {
                    final[page] = itm.ToString();
                    page++;
                }
                pdfDirectory = templatesDirectory + "Temporary_Files";
                if (!Directory.Exists(pdfDirectory)) Directory.CreateDirectory(pdfDirectory);
                if (pdfDirectory.LastIndexOf("\\") != pdfDirectory.Length - 1) pdfDirectory += "\\";
                FileConversion fil = new FileConversion();
                attachmentFileName = fil.GeneratePDF(pdfDirectory, final, templatesDirectory +
                    "Welcome Letter Header.jpg", "Verdana", 9);
                attachmentFileName = RenameAttachmentFile(toPartyId, pdfDirectory, attachmentFileName, 
                    templateName.Replace("Template", string.Empty));
                return true;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Error encountered during SendEmail [PartyId-" + toPartyId.ToString() +
                    "]: " + ex.Message, Logging.LogPriority.Error);
            }
            return false;
        }

        // Mark those that are done based upon Medicaid Id
        private void MarkDone(ref List<MMISMedicaidId> medicaidIds, string medicaidId)
        {
            for (int i = 0; i < medicaidIds.Count; i++)
            {
                if (medicaidIds[i].MedicaidId == medicaidId) medicaidIds[i].Done = true;
            }
        }

        // Send the Welcome letter email to the Provider because the Medicaid Id(s) are new
        public bool SendWelcomeLetterEmail(int toPartyId, List<MMISMedicaidId> medicaidIds)
        {

            //TODO REGTOLIVE
            Dictionary<string, object> fields = new Dictionary<string, object>();
            string body = string.Empty, emailFrom = string.Empty, emailTo = string.Empty, emailTo1 = string.Empty,
                emailTo2 = string.Empty, subject = string.Empty, templateName = string.Empty,
                templatesDirectory = string.Empty, providerName = string.Empty, NPI = string.Empty;
            int fromPartyId = 0;
            DateTime now = DateTime.Now;
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                fromPartyId = ProviderController.GetAdminPartyId();
                templatesDirectory = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                emailFrom = AppSettings.Get("SmtpFromEmailAddress", string.Empty);

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, toPartyId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectProviderContactEmail", parameters,
                    "EmailInfo");
                if (!ObjectControllerHelper.HasRows(ds))
                {
                    log.CreateLogEntry("Error encountered during SendEmail [PartyId-" + toPartyId.ToString() +
                        "]: " + "Provider Email Information not found", Logging.LogPriority.Error);
                    return false;
                }
                emailTo1 = ObjectControllerHelper.GetString("CREDENTIALING_EMAIL", ds.Tables[0].Rows[0]);
                emailTo2 = ObjectControllerHelper.GetString("SUBMIT_ROSTER_EMAIL", ds.Tables[0].Rows[0]);
                providerName = ObjectControllerHelper.GetString("PROVIDER_NAME", ds.Tables[0].Rows[0]);
                NPI = ObjectControllerHelper.GetString("NPI", ds.Tables[0].Rows[0]);
                if (string.IsNullOrEmpty(emailTo1) && string.IsNullOrEmpty(emailTo2))
                {
                    log.CreateLogEntry("Error encountered during SendEmail [PartyId-" + toPartyId.ToString() +
                        "]: " + "Provider Email Addresses are empty", Logging.LogPriority.Error);
                    return false;
                }
                else
                {
                    // Set "emailTo" address
                    if (!string.IsNullOrEmpty(emailTo1)) emailTo = emailTo1;
                    if (!string.IsNullOrEmpty(emailTo2))
                    {
                        if (!string.IsNullOrEmpty(emailTo)) emailTo += "," + emailTo2;
                        else emailTo = emailTo2;
                    }
                }

                if (!SetTemplate("Enrollment Notification", ref subject, ref templateName))
                {
                    log.CreateLogEntry("Error encountered during SendEmail [PartyId-" + toPartyId.ToString() +
                        "]: " + "Document \"Enrollment Notification\" not found", Logging.LogPriority.Error);
                    return false;
                }

                string pdfDirectory = string.Empty;
                List<string> attachmentFileNames = new List<string>();
                for (int i=0; i < medicaidIds.Count; i++)
                {
                    if (!medicaidIds[i].Done)
                    {
                        string attachmentFileName = string.Empty;
                        // TODO: MHH - Effective Date is defaulted to today's date
                        if (!CreatePDFAttachment(toPartyId, medicaidIds[i].IsGroup, medicaidIds[i].MedicaidId, medicaidIds,
                            templatesDirectory, ref pdfDirectory, NPI, DateTime.Now, ref attachmentFileName))
                        {
                            log.CreateLogEntry("Error encountered during SendEmail [PartyId-" + toPartyId.ToString() +
                                "]: " + "PDF not created", Logging.LogPriority.Error);
                            return false;
                        }
                        attachmentFileNames.Add(pdfDirectory + attachmentFileName);
                        MarkDone(ref medicaidIds, medicaidIds[i].MedicaidId);
                    }
                }

                fields.Add("PROVIDERNAME", providerName);
                fields.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));
                fields.Add("NPI", NPI);

                // If enabled, send the email
                EMailNotification notify = new EMailNotification(string.Empty, subject, emailTo, this.ThreadId);
                body = notify.SendNotification(templatesDirectory + "\\" + templateName, string.Empty, fields, false, attachmentFileNames);

                // Create the communicaton event and email
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
                string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);
                
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
                //parameters.Add(SqlParms.CreateParameter("COMMUNICATED_FROM_PARTY_ID", DbType.Int32, fromPartyId, false));
                //parameters.Add(SqlParms.CreateParameter("COMMUNICATED_TO_PARTY_ID", DbType.Int32, toPartyId, false));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
                parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, emailTo, false));
                parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
                parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
                parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, templateName, false));
                parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, ObjectControllerHelper.GetKeyValueString(fields), false));
                //parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, toPartyId, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, 0, false));
                parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, new Guid("00000000-0000-0000-0000-000000000000"), false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean,notify.isEmailSent,true));
                parameters.Add(SqlParms.CreateParameter("log_message", DbType.String,notify.log_message,true));
                string comId = DataAccess.ExecuteScalar("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);
                /* It looks like the sp_SelectLastCommunicationEventID does the same as getting the id from sp_insertCOMMUNICATIONEVENT_AND_EMAIL.
                 * Since I don't know the true reason it was done (SVN shows Mark Ham wrote this function), I don't want to interfere. Just 
                 * making a very simple change to the sp_insertCOMMUNICATIONEVENT_AND_EMAIL proc to return the COMMUNICATION_EVENT_ID. 
                 */
                for (int i=0; i < medicaidIds.Count; i++)
                {
                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, comId, false));
                    parameters.Add(SqlParms.CreateParameter("EMAIL_SUBJECT", DbType.String, subject, false));
                    parameters.Add(SqlParms.CreateParameter("TO_PARTY_ID", DbType.Int32, toPartyId, false));
                    parameters.Add(SqlParms.CreateParameter("IS_GROUP", DbType.Boolean, medicaidIds[i].IsGroup, false));
                    parameters.Add(SqlParms.CreateParameter("IND_OR_BASE_MEDICAID_ID", DbType.String, medicaidIds[i].MedicaidId, false));
                    parameters.Add(SqlParms.CreateParameter("MEDICAID_IDS", DbType.String, medicaidIds, false));
                    parameters.Add(SqlParms.CreateParameter("TEMPLATES_DIRECTORY", DbType.String, templatesDirectory, false));
                    parameters.Add(SqlParms.CreateParameter("PDF_DIRECTORY", DbType.String, pdfDirectory, false));
                    parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, false));
                    parameters.Add(SqlParms.CreateParameter("EFFECTIVE_DATE", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("ATTACHMENT_FILE_NAME", DbType.String, attachmentFileNames[i], false));
                    parameters.Add(SqlParms.CreateParameter("PDF_FILE", DbType.Binary, GetPdfSaveFormat(pdfDirectory, attachmentFileNames[i]), false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                    DataAccess.ExecuteStoredProcedure("insertATTACHMENTS", parameters);
                }

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                string comEventID = DataAccess.ExecuteScalar("sp_SelectLastCommunicationEventID", parameters);

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, toPartyId, true));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_ID", DbType.Int32, comEventID, true));
                parameters.Add(SqlParms.CreateParameter("RESOLVING_ACTION_TYPE_ID", DbType.Int32, Constants.ResolvingActionType.SystemGeneratedEmail, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("sp_UpdateCommunicationEventIdsForPartyErrorHistory", parameters);
                return true;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Error encountered during SendEmail [PartyId-" + toPartyId.ToString() +
                    "]: " + ex.Message, Logging.LogPriority.Error);
            }
            return false;
        }

        private Byte[] GetPdfSaveFormat(string directory, string filename)
        {
            FileStream fStream = File.OpenRead(directory + filename);
            byte[] contents = new byte[fStream.Length];
            fStream.Read(contents, 0, (int)fStream.Length);
            fStream.Close();
            return contents;
        }
    }
}
