using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;
namespace UserControls
{
    public partial class UserControls_EmailTemplateMaintenance : System.Web.UI.UserControl
    {
        private Logging _log;

        protected void Page_Load(object sender, EventArgs e)
        {
            _log = new Logging();
            GetAllTemplates();
            if (!IsPostBack)
            {
                step1.Visible = true;
                step2.Visible = false;
                singleEmail.Visible = false;
            }
            if (SessionVarRetriever.BulkEmailProviderIds.Count > 1)
            {
                btnSingleEmail.Visible = false;
            }
        }

        protected void btnTemplateSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string notes = null;
            if (!string.IsNullOrWhiteSpace(txtTemplateDescription.Text))
            {
                notes = txtTemplateDescription.Text.Trim();
            }

            if (!TemplateFileUpload.HasFile)
            {
                UploadValidator.IsValid = false;
                UploadValidator.ErrorMessage = "No template file to upload";
                return;
            }

            string ext = System.IO.Path.GetExtension(TemplateFileUpload.FileName);
            string[] allowedExtensions = { ".txt", ".html", ".htm" };
            if (!allowedExtensions.Contains(ext.ToLower()))
            {
                UploadValidator.IsValid = false;
                UploadValidator.ErrorMessage = "Only file types of {.txt, .html or .htm} are allowed.";
                return;
            }

            // Use the InputStream to get the actual stream sent.
            string fileContents = null;
            using (StreamReader reader = new StreamReader(TemplateFileUpload.FileContent))
            {
                fileContents = reader.ReadToEnd();
            }


            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.AddEmailTempalte(txtTemplateName.Text.Trim(), fileContents, notes);

            txtTemplateName.Text = string.Empty;
            txtTemplateDescription.Text = string.Empty;
            GetAllTemplates();
        }



        private void GetAllTemplates()
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            var data = svc.GetTAllemplates();
            if (!data.Any())
            {
                return;
            }

            grdTemplateList.DataSource = data;
            grdTemplateList.DataBind();
        }

        protected void GridView_Button_Click(object sender, EventArgs e)
        {
            //Determine the RowIndex of the Row whose Button was clicked.
            int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;

            //Get the value of column from the DataKeys using the RowIndex.
            int templateId = Convert.ToInt32(grdTemplateList.DataKeys[rowIndex].Values[0]);


            ViewState["Template_ID"] = templateId;
            if (ViewState["Template_ID"] != null)
            {
                step1.Visible = false;
                step2.Visible = true;
                csvRow1.Visible = !SessionVarRetriever.BulkEmailProviderIds.Any();
                csvRow2.Visible = !SessionVarRetriever.BulkEmailProviderIds.Any();
                csvRow3.Visible = !SessionVarRetriever.BulkEmailProviderIds.Any();
                csvRow4.Visible = !SessionVarRetriever.BulkEmailProviderIds.Any();
                providerListRow1.Visible = SessionVarRetriever.BulkEmailProviderIds.Any();
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                var tmp = svc.GetTemplateById(templateId);
                lblCsvTemplateId.Text = string.Format("Template Id: {0} - {1}", tmp.TemplateId, tmp.TemplateName);
            }
        }


        protected void grdTemplateList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTemplateList.PageIndex = e.NewPageIndex;
            GetAllTemplates();
        }


        protected void btnProvider_Click(object sender, EventArgs e)
        {
            int templateId = 0;

            if (ViewState["Template_ID"] != null)
            {
                templateId = Convert.ToInt32(ViewState["Template_ID"]);
            }
            else
            {
                TemplateIdValidator.IsValid = false;
                TemplateIdValidator.ErrorMessage = "You must first select a template.";
                return;
            }

            SaveProviderList(templateId, txtEmailSubject.Text);
        }


        private void SaveProviderList(int templateId, string subject)
        {
            var fieldValues = new Dictionary<string, object>();
            List<EmailQueueItem> queue = new List<EmailQueueItem>();

            if (!SessionVarRetriever.BulkEmailProviderIds.Any())
            {
                string errorMsg = "No providers were selected.  Please return to Provider Search and select before returning to email.";
                if (singleEmail.Visible)
                {
                    valCustomEmailText.IsValid = false;
                    valCustomEmailText.ErrorMessage = errorMsg;
                }
                else
                {
                    TemplateIdValidator.IsValid = false;
                    TemplateIdValidator.ErrorMessage = errorMsg;
                    step1.Visible = false;
                    step2.Visible = true;
                }
                return;
            }

            var notification = new Notification(subject);

            string templateBody;
            string bodyTemplateName;

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            if (singleEmail.Visible)
            {
                templateBody = edCustomEmail.GetHtml(EditorStripHtmlOptions.None);
                bodyTemplateName = "EMAIL_TEMPLATE_FREE_FORM";
            }
            else
            {
                EmailTemplate template = svc.GetTemplateById(templateId);
                templateBody = template.TemplateBody;
                bodyTemplateName = template.BodyTemplateName;
            }
            bool isMailNotifiaction = false;
            string failedNotificationRegIds = string.Empty;
            if (singleEmail.Visible)
            {
                var emailAddresses = "";
                foreach (int Reg_ID in SessionVarRetriever.BulkEmailProviderIds)
                { 
                    emailAddresses = GetRecipients(CON.SendToTypeID.FreeFormEmails, Reg_ID);
                    if(emailAddresses != "")
                        emailAddresses += "," + txtCustomSenders.Text.Trim();
                    else
                        emailAddresses += txtCustomSenders.Text.Trim();
                    if (!string.IsNullOrEmpty(emailAddresses))
                    {
                        Dictionary<string, object> fields = new Dictionary<string, object>();
                        string body = "";
                        var Emailnotification = new EMailNotification(templateBody, subject, emailAddresses,Reg_ID.ToString());
                        //Emailnotification.SendNotification(emailAddresses, subject, templateBody, true);
                        body = templateBody;
                        string GenericTemplatePath = AppSettings.Get("PDMS_TemplatesPath", string.Empty);
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            GenericTemplatePath = @"C:\Users\CA_OHPNM_DEVLEADS_5\source\repos\ohpnm-src-p3\PDMS\ProviderDataManagementSystemService\Documents";
                        }
                        GenericTemplatePath = GenericTemplatePath + "//GenericNoticeTemplate.txt";
                        Emailnotification.SendGenericNotification(GenericTemplatePath, Reg_ID.ToString(), true);
                        Emailnotification.SendNotificationBody(body);
                        Emailnotification.CreateCommunicationEvent(fields, new Guid(), Reg_ID.ToString(), "Single Email");

                        //Emailnotification.CreateCommunicationEvent(new Dictionary<string, object>(), Guid.Empty, SessionVarRetriever.BulkEmailProviderIds[0].ToString(), "Single Email");
                        isMailNotifiaction = true;


                    }
                }
            }
            else
            {
                foreach (var regid in SessionVarRetriever.BulkEmailProviderIds)
                {
                    if (string.IsNullOrEmpty(bodyTemplateName))
                    {
                        fieldValues = notification.GetFieldValuesByRegid(regid);
                        AddMissingTemplateFields(fieldValues);
                        var itemQueue = AddItemToQueue(regid, fieldValues, templateId, templateBody, subject, false);
                        queue.Add(itemQueue);
                    }
                    else
                    {
                        if (notification.SendWorkFlowEngineNotification(regid.ToString(), bodyTemplateName))
                        {
                            notification.AddEmailAddressForProvider(regid.ToString());
                            fieldValues = notification.Fields;
                            AddMissingTemplateFields(fieldValues);
                            var parsedEmailBody = PopulateTemplateBody(templateBody, fieldValues, true).Trim();
                            var emailAddresses = fieldValues.ContainsKey("EMAILADDRESS") ? Convert.ToString(fieldValues["EMAILADDRESS"]) : string.Empty;
                            if (!string.IsNullOrEmpty(emailAddresses))
                            {
                                var Emailnotification = new EMailNotification(parsedEmailBody, subject, emailAddresses);
                                Emailnotification.SendNotification(emailAddresses, subject, parsedEmailBody, true);
                                isMailNotifiaction = true;
                            }
                        }
                        else
                        {
                            failedNotificationRegIds += string.IsNullOrEmpty(failedNotificationRegIds) ? regid.ToString() : "," + regid;
                        }
                    }
                }
                if (queue.Count > 0)
                {
                    // Create the batch Job
                    int batchId = svc.BatchJobCreate(queue.Count, templateId, subject);
                    if (batchId == 0)
                    {
                        _log.CreateLogEntry("Problem creating batch record for mass email");
                        TemplateIdValidator.IsValid = false;
                        TemplateIdValidator.ErrorMessage = "Problem creating batch record";
                        step1.Visible = false;
                        step2.Visible = false;
                    }

                    // Set the batch id for each record to be sent
                    queue.ForEach(x => x.BatchId = batchId);
                    foreach (var record in queue)
                    {
                        svc.AddItemToQueue(record);
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Email Notification added to Queue successfully.');", true);
                }
            }
            if (!string.IsNullOrEmpty(failedNotificationRegIds))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Email notification failed due to Data not found for RegId(s) : " + failedNotificationRegIds + " ' );", true);
            }
            else
            {
                if (isMailNotifiaction)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Email Notification sent successfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Email Notification failed.');", true);
                }
            }
        }
        public string GetRecipients(int sendToTypeID, int regId)
        {
            StringBuilder addrList = new StringBuilder();
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("SendToTypeID", sendToTypeID));
                parameters.Add(new SqlParameter("RegID", regId));
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters,"res");
                if (Methods.HasRows(ds))
                {
                    addrList = Methods.AddEmail(ds.Tables[0], "EmailAddress");
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

            return addrList.ToString();
        }
        private EmailQueueItem AddItemToQueue(int regid, Dictionary<string, object> fieldValues, int templateId, string templateBody, string subject, bool throwIfNotFound = true)
        {
            AddMissingTemplateFields(fieldValues);
            string fieldHeader = string.Join(";", fieldValues.Select(x => x.Key));
            string fieldValuesPair = string.Join(";", fieldValues.Select(x => x.Key + ":" + x.Value));

            // Create Email Queue Item
            EmailQueueItem item = new EmailQueueItem();

            try
            {
                item.TemplateId = templateId;
                item.TemplateBody = templateBody;
                item.NPI = fieldValues.ContainsKey("NPI") ? Convert.ToString(fieldValues["NPI"]) : string.Empty;
                item.TaxId = fieldValues.ContainsKey("TAXID") ? Convert.ToString(fieldValues["TAXID"]) : string.Empty;
                item.RegId = regid;
                item.Name = fieldValues.ContainsKey("LEGALNAME") ? Convert.ToString(fieldValues["LEGALNAME"]) : string.Empty;
                item.EmailAddresses = fieldValues.ContainsKey("EMAILADDRESS") ? Convert.ToString(fieldValues["EMAILADDRESS"]) : string.Empty;
                item.EmailFields = fieldHeader;
                item.EmailSubject = subject;
                item.IsError = false;
                item.LastErrorDateTime = null;
                item.ErrorMessage = null;
                item.CreateDateTime = DateTime.Now;
                item.EmailValues = fieldValuesPair;
                item.ParsedEmailBody = PopulateTemplateBody(templateBody, fieldValues, throwIfNotFound).Trim();
            }
            catch (Exception ex)
            {
                _log.CreateLogEntry(string.Format("Error Parsing email template [{0}]", ex.Message));
            }

            return item;
        }

        private void AddMissingTemplateFields(Dictionary<string, object> fieldValues)
        {
            if (!fieldValues.ContainsKey("FONTSIZE"))
                fieldValues.Add("FONTSIZE", "14pt");
            if (!fieldValues.ContainsKey("DISPLAYOMR"))
                fieldValues.Add("DISPLAYOMR", string.Empty);
            if (!fieldValues.ContainsKey("PDMSURL"))
                fieldValues.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
            if (!fieldValues.ContainsKey("PDMSEMAIL"))
                fieldValues.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));
            if (!fieldValues.ContainsKey("CURRENTDATE"))
                fieldValues.Add("CURRENTDATE", DateTime.Now.ToString("MM/dd/yyyy"));

        }


        protected void CSVUploadButton_Click(object sender, EventArgs e)
        {
            int templateId = 0;

            if (!Page.IsValid)
            {
                return;
            }

            if (ViewState["Template_ID"] != null)
            {
                templateId = Convert.ToInt32(ViewState["Template_ID"]);
            }
            else
            {
                TemplateIdValidator.IsValid = false;
                TemplateIdValidator.ErrorMessage = "You must first select a template.";
                return;
            }


            if (!csvFileUpload.HasFile)
            {
                TemplateIdValidator.IsValid = false;
                TemplateIdValidator.ErrorMessage = "No file to upload.";
                return;
            }

            string ext = System.IO.Path.GetExtension(csvFileUpload.FileName);
            string[] allowedExtensions = { ".txt", ".csv" };
            if (!allowedExtensions.Contains(ext.ToLower()))
            {
                TemplateIdValidator.IsValid = false;
                TemplateIdValidator.ErrorMessage = "Only .CSV or .TXT files allowed.";
                return;
            }

            try
            {
                string subject = txtEmailSubject.Text.Trim();
                // Use the InputStream to get the actual stream sent.
                StreamReader csvreader = new StreamReader(csvFileUpload.FileContent);

                List<string> lines = new List<string>();

                while (!csvreader.EndOfStream)
                {
                    string line = csvreader.ReadLine();
                    lines.Add(line);
                }
                csvreader.Close();

                if (lines.Count < 1)
                {
                    TemplateIdValidator.IsValid = false;
                    TemplateIdValidator.ErrorMessage = string.Format("file {0} has no data", csvFileUpload.FileName);
                    return;
                }

                txtEmailSubject.Text = string.Empty;
                //SaveFileToQueue(lines, templateId, subject);

                bool success = SaveToEmailQueue(lines, templateId, subject);

                if (success)
                {
                    step1.Visible = true;
                    step2.Visible = false;

                }
                else
                {
                    step1.Visible = false;
                    step2.Visible = true;
                    csvRow1.Visible = !SessionVarRetriever.BulkEmailProviderIds.Any();
                    csvRow1.Visible = !SessionVarRetriever.BulkEmailProviderIds.Any();
                    csvRow1.Visible = !SessionVarRetriever.BulkEmailProviderIds.Any();
                    providerListRow1.Visible = SessionVarRetriever.BulkEmailProviderIds.Any();
                }
            }
            catch (Exception ex)
            {
                TemplateIdValidator.IsValid = false;
                TemplateIdValidator.ErrorMessage = ex.Message;
                step1.Visible = false;
                step2.Visible = false;
            }
        }

        //private void SaveFileToQueue(List<string> fileContents, int templateId, string subject)
        //{
        //    SendEmail emailSender = new SendEmail();

        //    if (fileContents.Count > 0)
        //    {
        //        emailSender.SaveToEmailQueue(fileContents, templateId, subject);
        //    }
        //}

        public bool ValidateColumnHeaders(string header)
        {
            return true;


            //return header.Trim().Replace(',').ToLower() == "name,address,age,gender";
        }



        protected void CSVUploadCancel_Click(object sender, EventArgs e)
        {
            ViewState["Template_ID"] = null;
            step2.Visible = false;
            step1.Visible = true;
            singleEmail.Visible = false;
        }


        private void SendBinaryResponseToClient(byte[] responseBytes, string contentHeader, string contentType)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();

            Response.Buffer = true;
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", contentHeader);
            Response.BinaryWrite(responseBytes);
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                                                                  // Technically, I should be doing Response.End(), but due to a bug in ASP.NET 
                                                                  // we tried doing complete request http://support.microsoft.com/kb/312629/en-us
                                                                  // but complete request is putting all the page up there. So, we are swallowing the
                                                                  // thread abort exception here.
                                                                  // Response.End();
            try
            {
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            }
            catch (Exception ex)
            {
                CoreException.ThrowException(ex);
            }
            finally
            {
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void grdTemplateList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void GridView_Download_Click(object sender, EventArgs e)
        {
            //Determine the RowIndex of the Row whose Button was clicked.
            var rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;

            //Get the value of column from the DataKeys using the RowIndex.
            var templateId = Convert.ToInt32(grdTemplateList.DataKeys[rowIndex].Values[0]);
            var svc = new PDMSService.PDMSServiceClient();
            var template = svc.GetTemplateById(templateId);
            var body = template.TemplateBody;
            var contents = Encoding.ASCII.GetBytes(body);
            SendBinaryResponseToClient(contents, "attachment;filename=EmailTemplate.txt", "application/txt");
        }


        public bool SaveToEmailQueue(List<string> csvRecord, int templateId, string subject)
        {
            bool status = false;
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

            try
            {
                EmailTemplate template = svc.GetTemplateById(templateId);

                // Header
                List<string> header = csvRecord[0].Split('|').ToList();

                //  Validate the header to the fields in the template

                List<string> missingFields = ValidateHeader(header, template.TemplateBody);


                if (missingFields.Any())
                {
                    // Display a message if there is an error validating the header and required fields
                    string message = string.Format("The following fields are missing from the CSV file: {0}", string.Join(";", missingFields));

                    TemplateIdValidator.IsValid = false;
                    TemplateIdValidator.ErrorMessage = message;
                    step1.Visible = false;
                    step2.Visible = true;
                    return false;
                }

                List<EmailQueueItem> queue = new List<EmailQueueItem>();
                for (int i = 1; i < csvRecord.Count; i++)
                {
                    var item = ParseAndValidateLine(csvRecord[i], templateId, template.TemplateBody, header, subject);
                    if (item != null)
                    {
                        queue.Add(item);
                    }
                }

                if (!queue.Any())
                {
                    TemplateIdValidator.IsValid = false;
                    TemplateIdValidator.ErrorMessage = "No Records to process";
                    step1.Visible = false;
                    step2.Visible = true;
                    return false;
                }

                // Create the batch Job
                int batchId = svc.BatchJobCreate(queue.Count, templateId, subject);
                if (batchId == 0)
                {
                    _log.CreateLogEntry("Problem creating batch record for mass email");
                    TemplateIdValidator.IsValid = false;
                    TemplateIdValidator.ErrorMessage = "Problem creating batch record";
                    step1.Visible = false;
                    step2.Visible = true;
                    return false;
                }

                // Set the batch id for each record to be sent
                queue.ForEach(x => x.BatchId = batchId);

                foreach (var record in queue)
                {
                    svc.AddItemToQueue(record);
                }
            }
            catch (Exception ex)
            {
                _log.CreateLogEntry(ex.Message);
                throw;
            }


            return status;
        }


        private List<string> ValidateHeader(List<string> header, string emailBody)
        {
            List<string> missingFields = new List<string>();
            List<string> requiredFilds = new List<string>() { "REG_ID", "EMAILADDRESS" };

            IList<string> templateFields = new List<string>();
            using (TextReader reader = new StringReader(emailBody))
            {
                TemplateEvaluator ev = new TemplateEvaluator();
                ev.Load(reader);
                templateFields = ev.FieldNames;
            }

            if (!templateFields.Any())
            {
                throw new Exception("No fields found in template.");
            }

            // Validate the fields needed in the template are included in the csv file.
            foreach (var item in templateFields)
            {
                if (!header.Contains(item, StringComparer.OrdinalIgnoreCase))
                {
                    missingFields.Add(item);
                }
            }

            // Validate the required fields that have to be included in the CSV, even if not used in the template file
            foreach (var item in requiredFilds)
            {
                if (!header.Contains(item, StringComparer.OrdinalIgnoreCase))
                {
                    missingFields.Add(item);
                }
            }


            return missingFields;
        }

        private EmailQueueItem ParseAndValidateLine(string record, int templateId, string templateBody, List<string> header, string subject)
        {

            if (string.IsNullOrEmpty(record))
                return null;

            List<string> data = record.Split('|').ToList();


            Dictionary<string, object> fieldValues = new Dictionary<string, object>();
            for (int i = 0; i < header.Count; i++)
            {

                if (data[i] == "NULL")
                {
                    fieldValues.Add(header[i], "");
                }
                else
                {
                    fieldValues.Add(header[i], data[i]);
                }
            }

            string fieldValuesPair = string.Join(";", fieldValues.Select(x => x.Key + ":" + x.Value));
            string fieldsHeader = string.Join(";", header);

            //Add to address fields as per existing logic
            Notification n = new Notification();
            EmailQueueItem item = new EmailQueueItem();

            try
            {
                item.TemplateId = templateId;
                item.TemplateBody = templateBody;
                item.NPI = Convert.ToString(fieldValues["NPI"]);
                item.TaxId = Convert.ToString(fieldValues["TAXID"]);
                item.RegId = fieldValues["REG_ID"] != null && fieldValues["REG_ID"].ToString() != string.Empty ? Convert.ToInt32(fieldValues["REG_ID"]) : 0;
                item.Name = Convert.ToString(fieldValues["PROVIDERNAME"]);
                item.EmailAddresses = Convert.ToString(fieldValues["EMAILADDRESS"]);
                item.EmailFields = fieldsHeader;
                item.EmailSubject = subject;
                item.IsError = false;
                item.LastErrorDateTime = null;
                item.ErrorMessage = null;
                item.CreateDateTime = DateTime.Now;

                string contact_name = Convert.ToString(fieldValues["CONTACTNAME"]) == "" ? Convert.ToString(fieldValues["Provider_Name"]) : Convert.ToString(fieldValues["CONTACTNAME"]);
                string citystatezip = Convert.ToString(fieldValues["SERVICINGCITY"]) + ", " +
                                      Convert.ToString(fieldValues["SERVICINGSTATE"]) + " " + Convert.ToString(fieldValues["SERVICINGZIP"]) +
                                      (!string.IsNullOrEmpty(Convert.ToString(fieldValues["SERVICINGZIP4"])) ? "-" + Convert.ToString(fieldValues["SERVICINGZIP4"]) : string.Empty);

                // Call to Notification to create bookmarks for the address
                string toAddress = n.GetToAddress(contact_name, Convert.ToString(fieldValues["CONTACT_QUADRANT"]), Convert.ToString(fieldValues["SERVICINGADDRESS1"]),
                    Convert.ToString(fieldValues["SERVICINGADDRESS2"]), citystatezip, fieldValues, Convert.ToString(fieldValues["PROVIDERNAME"]), Convert.ToString(fieldValues["NPI"]), Convert.ToString(fieldValues["MEDICAIDID"]), "", "", "");

                //now replace in email fields the actual to address computed
                //assuming toaddress is not uploaded through csv
                fieldValues["TOADDRESS"] = toAddress;
                fieldValuesPair = fieldValuesPair.Replace("TOADDRESS:", "TOADDRESS:" + toAddress + ";");

                item.EmailValues = fieldValuesPair;
                item.ParsedEmailBody = PopulateTemplateBody(templateBody, fieldValues).Trim();
            }
            catch
            {
                //_log.CreateLogEntry(string.Format("Error Parsing email template [{0}]", ex.Message));
                return null;
            }

            return item;
        }

        private string PopulateTemplateBody(string templateBody, Dictionary<string, object> fieldValues, bool throwIfNotFound = true)
        {
            string content = null;

            Notification n = new Notification();
            //change the content based on paper email required or not.
            string recipients = fieldValues.ContainsKey("EMAILADDRESS") ? Convert.ToString(fieldValues["EMAILADDRESS"]) : string.Empty;
            if (Methods.RequirePaperNotice(recipients, AppSettings.Get("PSEmailTypes", string.Empty).Split(',')))
            {
                //if paper then change the FONTSIZE and DISPLAYOMR
                fieldValues["FONTSIZE"] = "14pt";
                fieldValues["DISPLAYOMR"] = "";
                content = n.ParseEmailBody(templateBody, fieldValues, throwIfNotFound);
            }
            else
            {
                //Email parsed Body
                content = n.ParseEmailBody(templateBody, fieldValues, throwIfNotFound);
            }

            return content;
        }

        private bool TemplateExists(int templateId)
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            bool itExists = false;
            try
            {
                var template = svc.GetTemplateById(templateId);

                if (template != null)
                {
                    if (template.TemplateId > 0)
                        itExists = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return itExists;
        }


        protected void btnSingleEmail_Click(object sender, EventArgs e)
        {
            step1.Visible = false;
            step2.Visible = false;
            singleEmail.Visible = true;
            Label2.Visible = true;
            txtCustomSenders.Visible = true;
            valCustomEmailTo.Enabled = false;
        }

        protected void btnSendCustomEmail_Click(object sender, EventArgs e)
        {
            int templateId = 0;

            if (String.IsNullOrEmpty(txtCustomSubject.Text))
            {
                valCustomEmailSubject.IsValid = false;
                valCustomEmailSubject.ErrorMessage = "You must first enter some text for the message.";
                return;
            }
            if (String.IsNullOrEmpty(edCustomEmail.Text))
            {
                valCustomEmailText.IsValid = false;
                return;
            }
            if (singleEmail.Visible && SessionVarRetriever.IsManualClosureEmail)
            {
                SendManualClosureEmailWithSenders();
            }
            else
            {
                SaveProviderList(templateId, txtCustomSubject.Text);
            }

            step1.Visible = true;
            step2.Visible = false;
            singleEmail.Visible = false;

        }

        private void SendManualClosureEmailWithSenders()
        {
            string templateBody = string.Empty;
            string bodyTemplateName = string.Empty;
            string senders = string.Empty;
            string subject = txtCustomSubject.Text;
            if (SessionVarRetriever.BulkEmailProviderIds.Any())
            {
                if (String.IsNullOrEmpty(txtCustomSenders.Text))
                {
                    valCustomEmailTo.IsValid = false;
                    return;
                }
            }
            try
            {
                var notification = new Notification();
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                templateBody = edCustomEmail.GetHtml(EditorStripHtmlOptions.None);
                bodyTemplateName = "EMAIL_TEMPLATE_FREE_FORM";
                var fieldValues = new Dictionary<string, object>();
                int CurrentStep = 0;
                int processID = 0;
                //Is wf is in Risk alert Closure
                foreach (var regid in SessionVarRetriever.BulkEmailProviderIds)
                {
                    bool isRiskAlert = Helper.HasActiveWorkflow(regid, CON.WorkflowType.RiskAlertClosure);
                    // isRiskAlert = true;
                    if (isRiskAlert)
                    {
                        //Create Communication email
                        var Emailnotification = new EMailNotification(templateBody, subject, txtCustomSenders.Text);
                        Emailnotification.SendNotification(txtCustomSenders.Text, subject, templateBody, true);

                        Emailnotification.CreateCommunicationEvent(
                                       fieldValues
                                         , Helper.GetUserId(HttpContext.Current.User.Identity.Name)
                                         , regid.ToString()
                                         , bodyTemplateName);


                        //Advance step 
                        DataSet dsStep = svc.GetWFProcessByRegId(regid);
                        if (Helper.HasRows(dsStep))
                        {
                            CurrentStep = Helper.GetInt("CURRENT_STEP_ID", dsStep.Tables[0].Rows[0]);
                            processID = Helper.GetInt("PROCESS_ID", dsStep.Tables[0].Rows[0]);
                            dsStep = svc.WF_SelectStepActions(CurrentStep);

                            if (Helper.HasRows(dsStep))
                            {
                                DataRow[] actionRows = dsStep.Tables[0].Select("1=1");
                                svc.WF_TakeAction(processID, Helper.GetString("ACTION_NAME", actionRows[0]), "Advanced from Manual send email page");
                            }
                        }
                    }
                    

                }

            }
            catch
            {
            }
            ClearClosureSession();

        }

        private void ClearClosureSession()
        {
            SessionVarRetriever.BulkEmailProviderIds = null;
            SessionVarRetriever.IsManualClosureEmail = false;
        }

        protected void edCustomEmail_PreRender(object sender, EventArgs e)
        {
            edCustomEmail.EnsureToolsFileLoaded();

            if (edCustomEmail.FindTool("ImageManager") != null)
            {
                edCustomEmail.FindTool("ImageManager").Visible = false;
            }
            if (edCustomEmail.FindTool("FlashManager") != null)
            {
                edCustomEmail.FindTool("FlashManager").Visible = false;
            }
            if (edCustomEmail.FindTool("MediaManager") != null)
            {
                edCustomEmail.FindTool("MediaManager").Visible = false;
            }
            if (edCustomEmail.FindTool("DocumentManager") != null)
            {
                edCustomEmail.FindTool("DocumentManager").Visible = false;
            }

        }

    }
}