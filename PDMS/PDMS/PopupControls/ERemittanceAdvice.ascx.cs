using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MMSWebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corp.Core.Libraries;
using System.IO.Compression;

public partial class PopupControls_ERemittanceAdvice : BaseSectionControl
{

    #region spa
    private String CurrentSortOrder = "DESC";
    private String CurrentSortField = "DOCUMENT_RECEIVED_DATE";
    private PDMSService.PDMSServiceClient _spa;
    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
    }
    #endregion

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["CheckRefresh"] != null)
        {
            ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }
    }
    public void LoadDestinationPayer()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlPrimaryDestinationPayer.Items.Clear();
        DataSet dataSet = _spa.LoadDestinationPayer(true);
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlPrimaryDestinationPayer, dt, "DESTINATION_PAYER_DESC", "DESTINATION_PAYER_ID", true);
        ddlPrimaryDestinationPayer.SelectedIndex = 1;
    }

    Boolean IsPageRefresh;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                LoadDestinationPayer();
            }
            catch (Exception ex)
            {
                MessageBox2.Show("An error occurred when loading the work queue.<br>Error: " + ex.Message + " " + ex.StackTrace, "Error");
            }
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            if (ViewState["postids"] != null)
            {
                Session["postid"] = ViewState["postids"].ToString();
            }
        }
        else
        {
            if (Session["postid"] != null)
            {
                if (string.IsNullOrEmpty(Session["postid"].ToString()))
                {
                    if (ViewState["postids"].ToString() != Session["postid"].ToString())
                    {
                        IsPageRefresh = true;
                    }
                }
            }
            Session["postid"] = System.Guid.NewGuid().ToString();
            if (Session["postid"] != null)
            {
                ViewState["postids"] = Session["postid"].ToString();
            }
        }
    }
    public override bool SaveData()
    {
        return true;
    }
    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }

            return _svc;
        }
    }
    #endregion

    public override void LoadControlData()
    {
        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
        {

            sepInstructions.InnerText = sepInstructions.InnerText.Replace('+', '-');
            sepOwnInfo.InnerText = sepOwnInfo.InnerText.Replace('-', '+');

        }
    }

    private void LoadPASTATUS()
    {

    }
    public override void LoadData(DataRow dr)
    {
    }


    public override bool ValidateData()
    {
        bool isValid = true;

        DateTime value;
        if (!String.IsNullOrEmpty(txtDateAvailableFrom.Text))
        {
            if (DateTime.TryParse(txtDateAvailableFrom.Text, out value))
            {
                if (DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtDateAvailableFrom.Text)) < 0)
                {
                    AddError("* Date From cannot be after todays date.", ref isValid);
                }
            }
            else
            {
                AddError("* Date From is not a date format.", ref isValid);
            }
        }

        if (!String.IsNullOrEmpty(txtDateAvailableTo.Text))
        {
            if (DateTime.TryParse(txtDateAvailableTo.Text, out value))
            {
                if (DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtDateAvailableTo.Text)) < 0)
                {
                    AddError("* Date To cannot be after todays date.", ref isValid);
                }
            }
            else
            {
                AddError("* Date To is not a date format.", ref isValid);
            }
        }

        if (!String.IsNullOrEmpty(txtDateAvailableFrom.Text) && !String.IsNullOrEmpty(txtDateAvailableTo.Text))
        {
            DateTime enteredFrom = DateTime.Parse(txtDateAvailableFrom.Text);
            DateTime enteredTo = DateTime.Parse(txtDateAvailableTo.Text);
            if (DateTime.Compare(enteredFrom, enteredTo) > 0)
            {
                AddError("* Choose DateFrom value prior to DateTo value", ref isValid);
            }

        }

        if (String.IsNullOrEmpty(ddlPrimaryDestinationPayer.SelectedValue.ToString()))
        {
            AddError("* Payer is required", ref isValid);
        }

        //if (String.IsNullOrEmpty(txtRANumber.Text))
        //{
        //    AddError("* RA number upto 9-digits is required", ref isValid);

        //}
        return isValid;
    }

    public override string ValidationGroup
    {
        get { return "valERemittanceAdvice"; }
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valERemittanceAdvice";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public override string Title
    {
        get { return "REMITTANCE ADVICE SEARCH"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucERemittanceAdvice_" + this.WorkflowPage.RegistrationId; }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!(IsPageRefresh))
        {
            ValidateData();
            if (Page.IsValid)
            {
                divERemittanceAdvicehHeader.Visible = true;
                this.gvRemittanceAdvicesearch.PageIndex = 0;
                Search(true);
                // lnkRowCount.Visible = lnkExcel.Visible = lnkPDF.Visible = gvRemittanceAdvicesearch.Rows.Count > 0;
            }
            if (gvRemittanceAdvicesearch.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>LoadGrid()</script>", false);
            }
        }
    }
    private void Search(bool clearIds)
    {
        divERemittanceAdvicehHeader.Visible = clearIds;
        this.gvRemittanceAdvicesearch.PageIndex = 0;
        RefreshData(CurrentSortOrder);
    }
    protected void PageSize_Changed(object sender, EventArgs e)
    {
        this.gvRemittanceAdvicesearch.PageIndex = 0;
        RefreshData(CurrentSortOrder);
    }
    protected void gvRemittanceAdvicesearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index;
        bool isValid = Int32.TryParse(e.CommandArgument.ToString(), out index);

        if ((e.CommandName == "OnFileDownload" || e.CommandName == "OnFileDownloadPDF_Migra") && isValid)
        {
            int n;
            bool isNumeric = int.TryParse(this.gvRemittanceAdvicesearch.DataKeys[index].Values["DOCUMENT_ID_PDF"].ToString(), out n);


            if (isNumeric && n != 0)
            {
                int documentID = Convert.ToInt32(n);
                bool fileDownloaded = false;
                int idRAReports = Convert.ToInt32(this.gvRemittanceAdvicesearch.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
                string filename = this.gvRemittanceAdvicesearch.DataKeys[index].Values["FileNamePDF"].ToString();
                DownloadFile(filename, documentID, out fileDownloaded);
                CurrentSortOrder = this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                RefreshData(CurrentSortOrder);
            }
            else
            {
                bool fileDownloaded = false;
                int idRAReports = Convert.ToInt32(this.gvRemittanceAdvicesearch.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
                string filename = this.gvRemittanceAdvicesearch.DataKeys[index].Values["FileName"].ToString();
                int documentID = Convert.ToInt32(this.gvRemittanceAdvicesearch.DataKeys[index].Values["DOCUMENT_ID"].ToString());
                string docType = this.gvRemittanceAdvicesearch.DataKeys[index].Values["document_type"].ToString();
                string uuid = this.gvRemittanceAdvicesearch.DataKeys[index].Values["UUID"].ToString();
                int raID = Convert.ToInt32(this.gvRemittanceAdvicesearch.DataKeys[index].Values["remittance_advice_id"].ToString());

                if (docType == "txt")
                {
                    DownloadPDFSharpFile(raID, filename, documentID, uuid, out fileDownloaded);
                    if (fileDownloaded)
                    {
                        updateDownloadDate(idRAReports);
                    }
                    CurrentSortOrder = this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                    RefreshData(CurrentSortOrder);
                }
                else if (docType == "zip")
                {
                    DownloadPDFZipFile(raID, filename, documentID, uuid, out fileDownloaded);
                    if (fileDownloaded)
                    {
                        updateDownloadDate(idRAReports);
                    }
                    CurrentSortOrder = this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                    RefreshData(CurrentSortOrder);
                }
                else
                {
                    DownloadFile(filename, documentID, out fileDownloaded);
                    if (fileDownloaded)
                    {
                        updateDownloadDate(idRAReports);
                    }
                    CurrentSortOrder = this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                    RefreshData(CurrentSortOrder);
                }
            }
        }

        if ((e.CommandName == "OnFileDownloadPDF_Zip") && isValid)
        {
            int n;
            bool isNumeric = int.TryParse(this.gvRemittanceAdvicesearch.DataKeys[index].Values["DOCUMENT_ID_PDF"].ToString(), out n);


            if (isNumeric && n != 0)
            {
                int documentID = Convert.ToInt32(n);
                bool fileDownloaded = false;
                int idRAReports = Convert.ToInt32(this.gvRemittanceAdvicesearch.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
                string filename = this.gvRemittanceAdvicesearch.DataKeys[index].Values["FileNamePDF"].ToString();
                DownloadFile(filename, documentID, out fileDownloaded);
                CurrentSortOrder = this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                RefreshData(CurrentSortOrder);
            }
            else
            {
                bool fileDownloaded = false;
                int idRAReports = Convert.ToInt32(this.gvRemittanceAdvicesearch.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
                string filename = this.gvRemittanceAdvicesearch.DataKeys[index].Values["FileName"].ToString();
                int documentID = Convert.ToInt32(this.gvRemittanceAdvicesearch.DataKeys[index].Values["DOCUMENT_ID"].ToString());
                string docType = this.gvRemittanceAdvicesearch.DataKeys[index].Values["document_type"].ToString();
                string uuid = this.gvRemittanceAdvicesearch.DataKeys[index].Values["UUID"].ToString();
                int raID = Convert.ToInt32(this.gvRemittanceAdvicesearch.DataKeys[index].Values["remittance_advice_id"].ToString());

                if (docType == "zip")
                {
                    DownloadPDFZipFile(raID, filename, documentID, uuid, out fileDownloaded);
                    if (fileDownloaded)
                    {
                        updateDownloadDate(idRAReports);
                    }
                    CurrentSortOrder = this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                    RefreshData(CurrentSortOrder);
                }
            }
        }

        if (e.CommandName == "OnFileDownloadPDF" && isValid)
        {
            bool fileDownloaded = false;
            int idRAReports = Convert.ToInt32(this.gvRemittanceAdvicesearch.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
            string filename = this.gvRemittanceAdvicesearch.DataKeys[index].Values["FileNamePDF"].ToString();
            int documentID = Convert.ToInt32(this.gvRemittanceAdvicesearch.DataKeys[index].Values["DOCUMENT_ID_PDF"].ToString());
            DownloadFile(filename, documentID, out fileDownloaded);
            CurrentSortOrder = this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
            RefreshData(CurrentSortOrder);

        }
    }


    public void RefreshData(string sortDirection)
    {
        int totalResultCount = 0;
        gvRemittanceAdvicesearch.PageSize = !string.IsNullOrEmpty(ddlPageSize.SelectedValue) ? Convert.ToInt32(ddlPageSize.SelectedValue) : 15;
        DataTable dt = GetData(out totalResultCount, gvRemittanceAdvicesearch.PageSize, sortDirection);
        hdnRowCount.Value = totalResultCount.ToString();
        // lnkRowCount.Text = String.Format("Row Count: {0}", totalResultCount);
        if (totalResultCount == 0)
        {
            gvRemittanceAdvicesearch.EmptyDataText = "~~No Reports found~~";
        }
        gvRemittanceAdvicesearch.DataSource = dt;
        gvRemittanceAdvicesearch.VirtualItemCount = totalResultCount;
        gvRemittanceAdvicesearch.DataBind();


    }
    private void updateDownloadDate(int idxRetrieveReports)
    {
        try
        {
            int index = idxRetrieveReports;
            bool isDownloaded = true;
            DateTime downloadDate = DateTime.Now;
            DateTime lastModifiedDate = DateTime.Now;
            Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            svc.UpdateRetrieveReports(index, isDownloaded, downloadDate, lastModifiedDate, lastModifiedUser);
        }
        catch (Exception ex)
        {
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");
        }
    }

    public static string clnFileName(string fileName)
    {
        StringBuilder builder = new StringBuilder(fileName);
        builder.Replace(".txt", "");

        return builder.ToString();
    }

    public static string InsertHeader(string sourcePath, string fileName, string header, string footer)
    {
        string cleanFileName = clnFileName(fileName);

        var srcfile = sourcePath + fileName;
        using (var writer = new StreamWriter(sourcePath + cleanFileName))
        using (var reader = new StreamReader(srcfile))
        {
            writer.WriteLine(header);
            while (!reader.EndOfStream)
                writer.WriteLine(reader.ReadLine());
            writer.WriteLine(footer);
        }
        File.Delete(srcfile);

        return cleanFileName;
    }


    private void DownloadPDFSharpFile(int raID, string fileName, int documentID, string uuid, out bool fileDownloaded)
    {
        fileDownloaded = false;
        try
        {
            string filePath = string.Empty;
            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            string htmFileName = fileName + ".html";
            StringBuilder builder = new StringBuilder(fileName);
            builder.Replace(".txt", "");
            string pdfFileName = builder + ".pdf";
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName);
#if DEBUG
                   @filePath = @"C:\Temp\";
#endif

            bool isFileLocal = bool.Parse(AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString));

            if (isFileLocal && !File.Exists(filePath))
            {
                #if DEBUG
                    MessageBox2.Show("File or directory does not exist.: " + filePath, "Error");
                #endif

                MessageBox2.Show("File or directory does not exist.: " + filePath, "Error");

            }
            else
            {
                fileDownloaded = true;
            }

            bool fileExistsonLocal = System.IO.File.Exists(filePath);

            string isEncrypted = "N";
            if (uuid.Equals("11111111-1111-1111-1111-111111111111"))
            {
                isEncrypted = "Y";
            }
            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath, documentID, isEncrypted);

            

            File.WriteAllBytes(filePath + htmFileName, decryptedFile);

            htmFileName = InsertHeader(filePath, htmFileName, "<pre>", "</pre>");

            ProcessDocumentController.CreatePDFFromHTML(filePath, htmFileName, true);

            byte[] pdfFileBytes = File.ReadAllBytes(filePath + pdfFileName);

            int pdfDocumentID = StoreDocumentRecord(pdfFileName, pdfFileName, string.Empty);

            onBaseInterface.SubmitFile(pdfDocumentID, pdfFileBytes, pdfFileName);

            File.Delete(filePath + htmFileName);

            File.Delete(filePath + pdfFileName);

            UpdateOnBasePDFID(raID, pdfDocumentID);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + pdfFileName);
            HttpContext.Current.Response.BinaryWrite(pdfFileBytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
#if DEBUG
                MessageBox2.Show("Error.: " + ex.Message, "Error");
#endif
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");
        }

    }

    private void DownloadPDFZipFile(int raID, string fileName, int documentID, string uuid, out bool fileDownloaded)
    {
        fileDownloaded = false;
        try
        {
            string filePath = string.Empty;
            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            string zipFileName = fileName + ".zip";
            StringBuilder builder = new StringBuilder(fileName);
            builder.Replace(".zip", "");
            builder.Replace(".pdf", "");
            string pdfFileName = builder + ".pdf";
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName);
#if DEBUG
                   @filePath = @"C:\Temp\";
#endif

            bool isFileLocal = bool.Parse(AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString));

            if (isFileLocal && !File.Exists(filePath))
            {
#if DEBUG
                    MessageBox2.Show("File or directory does not exist.: " + filePath, "Error");
#endif

                MessageBox2.Show("File or directory does not exist.: " + filePath, "Error");

            }
            else
            {
                fileDownloaded = true;
            }

            string isEncrypted = "N";
            if (uuid.Equals("11111111-1111-1111-1111-111111111111"))
            {
                isEncrypted = "Y";
            }
            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath, documentID, isEncrypted);

            string decryptedFileSTR = Encoding.UTF8.GetString(decryptedFile);

            byte[] dataB64 = Convert.FromBase64String(decryptedFileSTR);

            File.WriteAllBytes(filePath + zipFileName, dataB64);

            string startPath = filePath + zipFileName;
            string extractPath = filePath + pdfFileName;

            using (ZipArchive zip = ZipFile.Open(startPath, ZipArchiveMode.Read))
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    string ext = Path.GetExtension(entry.Name).ToLower();
                    if (ext == ".pdf")
                    {
                        entry.ExtractToFile(extractPath, true);
                    }
                }

            byte[] pdfFileBytes = File.ReadAllBytes(filePath + pdfFileName);

            int pdfDocumentID = StoreDocumentRecord(pdfFileName, pdfFileName, string.Empty);

            onBaseInterface.SubmitFile(pdfDocumentID, pdfFileBytes, pdfFileName);

            File.Delete(filePath + zipFileName);

            File.Delete(filePath + pdfFileName);

            UpdateOnBasePDFID(raID, pdfDocumentID);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + pdfFileName);
            HttpContext.Current.Response.BinaryWrite(pdfFileBytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
#if DEBUG
                MessageBox2.Show("Error.: " + ex.Message, "Error");
#endif
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");
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
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid("0446D773-C0DD-458D-AC82-E9AD12CC2725"), false));

            DataSet ds = new DataSet();
            retVal = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT", parameters));


        }
        catch (Exception ex)
        {
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");
        }

        return retVal;
    }

    private void UpdateOnBasePDFID(int remittance_advice_id, int pdf_document_id)
    {
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("remittance_advice_id", DbType.Int32, remittance_advice_id, true));
            parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID_PDF", DbType.Int32, pdf_document_id, true));
            parameters.Add(SqlParms.CreateParameter("Last_Modified_Date_Time", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("Last_Modified_User", DbType.Guid, new Guid("0446D773-C0DD-458D-AC82-E9AD12CC2725"), true));
            DataAccess.ExecuteScalar("usp_UpdateRATXTFilesToProcess", parameters);
        }
        catch (Exception ex)
        {
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");
        }
    }

    private void DownloadFile(string fileName, int documentID, out bool fileDownloaded)
    {
        fileDownloaded = false;
        try
        {
            string filePath = string.Empty;
            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName);
#if DEBUG
                   @filePath = @"C:\Temp";
#endif

            bool isFileLocal = bool.Parse(AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString));

            if (isFileLocal && !File.Exists(filePath))
            {
#if DEBUG
                    MessageBox2.Show("File or directory does not exist.: " + filePath, "Error");
#endif

                MessageBox2.Show("File or directory does not exist.: " + filePath, "Error");

            }
            else
            {
                fileDownloaded = true;
            }

            bool fileExistsonLocal = System.IO.File.Exists(filePath);
            bool downloadFile = isFileLocal ? fileExistsonLocal : true;

            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath, documentID, "Y");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(decryptedFile);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
#if DEBUG
                MessageBox2.Show("Error.: " + ex.Message, "Error");
#endif
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");
        }

    }


    protected void gvRemittanceAdvicesearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.gvRemittanceAdvicesearch.PageIndex = e.NewPageIndex;
        CurrentSortOrder = gvRemittanceAdvicesearch.Attributes["CurrentSortDirection"];//this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
        RefreshData(CurrentSortOrder);
    }

    private DataTable GetData(out int totalResultCount, int pageSize,string sortDirection)
    {
        DataSet ds;
        totalResultCount = 0;

        List<SqlParameter> parameters = new List<SqlParameter>();
        if (!(String.IsNullOrEmpty(this.WorkflowPage.MedicaidID)))
        {
            parameters.Add(SqlParms.CreateParameter("MED_ID", DbType.String, this.WorkflowPage.MedicaidID, true));
        }
        if (!(String.IsNullOrEmpty(txtRANumber.Text)))
        {
            parameters.Add(SqlParms.CreateParameter("RA_NUMBER", DbType.String, txtRANumber.Text, true)); // is this in constants?
        }
        if (!(String.IsNullOrEmpty(txtDateAvailableFrom.Text)))
        {
            parameters.Add(SqlParms.CreateParameter("MIN_DATE", DbType.Date, txtDateAvailableFrom.Text, true));
        }
        if (!(String.IsNullOrEmpty(txtDateAvailableTo.Text)))
        {
            parameters.Add(SqlParms.CreateParameter("MAX_DATE", DbType.Date, txtDateAvailableTo.Text, true));
        }
        if (!string.IsNullOrEmpty(sortDirection))
        {
            parameters.Add(SqlParms.CreateParameter("Sorting_Direction", DbType.String, sortDirection, true));
        }
		if (!(String.IsNullOrEmpty(txtICN.Text)))
        {
            parameters.Add(SqlParms.CreateParameter("ICN", DbType.String, txtICN.Text, true));
        }
        parameters.Add(SqlParms.CreateParameter("Payment_ID", DbType.String, ddlPrimaryDestinationPayer.SelectedValue, false));
        ds = DataAccess.ExecuteStoredProcedure("usp_Get_Remittance_Advice", parameters, "RemittanceAdvice");
        // ds = psc.SearchRAReports(parms, out totalResultCount);

        if (Helper.HasRows(ds))
        {
            totalResultCount = ds.Tables[0].Rows.Count;
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        divERemittanceAdvicehHeader.Visible = lnkRowCount.Visible = lnkExcel.Visible = lnkPDF.Visible = false;
        gvRemittanceAdvicesearch.EmptyDataText = txtDateAvailableFrom.Text = txtDateAvailableFrom.Text = String.Empty;
        txtRANumber.Text = "";
        txtDateAvailableTo.Text = string.Empty;
        txtDateAvailableFrom.Text = string.Empty;
        ddlPrimaryDestinationPayer.ClearSelection();
        gvRemittanceAdvicesearch.DataSource = null;
        gvRemittanceAdvicesearch.DataBind();
    }


    protected void grdRemittanceAdvicesearch_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (gvRemittanceAdvicesearch.Attributes["CurrentSortField"] != null && gvRemittanceAdvicesearch.Attributes["CurrentSortDirection"] != null)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.HasControls())
                    {
                        LinkButton sortLink = null;
                        if (cell.Controls[0] is LinkButton)
                        {
                            sortLink = (LinkButton)cell.Controls[0];
                        }
                        if (sortLink != null && gvRemittanceAdvicesearch.Attributes["CurrentSortField"] == sortLink.CommandArgument)
                        {
                            Image img = new Image();
                            img.Width = System.Web.UI.WebControls.Unit.Pixel(16);
                            img.Height = System.Web.UI.WebControls.Unit.Pixel(16);
                            if (gvRemittanceAdvicesearch.Attributes["CurrentSortDirection"] == "ASC")
                            {
                                img.ImageUrl = "~/App_Themes/Default/Grid/SortAsc.gif";
                            }
                            else
                            {
                                img.ImageUrl = "~/App_Themes/Default/Grid/SortDesc.gif";
                            }
                            cell.Controls.Add(new LiteralControl("&nbsp;"));
                            cell.Controls.Add(img);
                        }
                    }
                }
            }
        }
    }

    protected void gvRemittanceAdvicesearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (gvRemittanceAdvicesearch.Attributes["CurrentSortDirection"] == "ASC")
        {
            gvRemittanceAdvicesearch.Attributes["CurrentSortDirection"] = "DESC";
        }
        else
        {
            gvRemittanceAdvicesearch.Attributes["CurrentSortDirection"] = "ASC";
        }      

        CurrentSortField = e.SortExpression;
        CurrentSortOrder = gvRemittanceAdvicesearch.Attributes["CurrentSortDirection"];//this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
        RefreshData(CurrentSortOrder);
    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        int totalResultCount = 0;
        CurrentSortOrder = this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
        DataTable dt = GetData(out totalResultCount, 10000, CurrentSortOrder);
        RadGridExport.DataSource = dt;
        RadGridExport.DataBind();
        RadGridExport.MasterTableView.ExportToPdf();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        int totalResultCount = 0;
        CurrentSortOrder = this.gvRemittanceAdvicesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
        DataTable dt = GetData(out totalResultCount, 10000, CurrentSortOrder);
        RadGridExport.DataSource = dt;
        RadGridExport.DataBind();
        RadGridExport.MasterTableView.ExportToExcel();
    }
    protected void gvRemittanceAdvicesearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (gvRemittanceAdvicesearch.Attributes["CurrentSortField"] != null && gvRemittanceAdvicesearch.Attributes["CurrentSortDirection"] != null)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.HasControls())
                    {
                        LinkButton sortLink = null;
                        if (cell.Controls[0] is LinkButton)
                        {
                            sortLink = (LinkButton)cell.Controls[0];
                        }
                        if (sortLink != null && gvRemittanceAdvicesearch.Attributes["CurrentSortField"] == sortLink.CommandArgument)
                        {
                            Image img = new Image();
                            img.Width = System.Web.UI.WebControls.Unit.Pixel(16);
                            img.Height = System.Web.UI.WebControls.Unit.Pixel(16);
                            if (gvRemittanceAdvicesearch.Attributes["CurrentSortDirection"] == "ASC")
                            {
                                img.ImageUrl = "~/App_Themes/Default/Grid/SortAsc.gif";
                            }
                            else
                            {
                                img.ImageUrl = "~/App_Themes/Default/Grid/SortDesc.gif";
                            }
                            cell.Controls.Add(new LiteralControl("&nbsp;"));
                            cell.Controls.Add(img);
                        }
                    }
                }
            }
        }
    }

}