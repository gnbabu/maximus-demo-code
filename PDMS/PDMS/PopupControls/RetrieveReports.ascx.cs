using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;
using System.Xml;
using Irony.Parsing;

public partial class PopupControls_RetrieveReports : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                BindDropDown();
            }
            catch (Exception ex)
            {
                MessageBox2.Show("An error occurred when loading the work queue.<br>Error: " + ex.Message + " " + ex.StackTrace, "Error");
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ValidateData();
        if (Page.IsValid)
        {
            this.lblMessages.Text = string.Empty;
            this.gvRetrieveReports.CurrentPageIndex = 0;
            Search(true);
            lnkRowCount.Visible = lnkExcel.Visible = lnkPDF.Visible = gvRetrieveReports.Rows.Count > 0;
        }
    }

    private void Search(bool clearIds)
    {
        divRRSearchHeader.Visible = clearIds;
        this.gvRetrieveReports.CurrentPageIndex = 0;
        RefreshData();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        divRRSearchHeader.Visible = lnkRowCount.Visible = lnkExcel.Visible = lnkPDF.Visible = false;
        gvRetrieveReports.EmptyDataText = txtDateAvailableFrom.Text = txtDateAvailableTo.Text = String.Empty;
        ddlDocumentType.SelectedIndex = -1;
        gvRetrieveReports.DataSource = null;
        gvRetrieveReports.DataBind();
    }
    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        int totalResultCount = 0;
        DataTable dt = GetData(out totalResultCount, 10000);
        RadGridExport.DataSource = dt;
        RadGridExport.DataBind();
        RadGridExport.MasterTableView.ExportToPdf();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        int totalResultCount = 0;
        DataTable dt = GetData(out totalResultCount, 10000);
        RadGridExport.DataSource = dt;
        RadGridExport.DataBind();
        RadGridExport.MasterTableView.ExportToExcel();
    }

    protected void gvRetrieveReports_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }

    protected void gvRetrieveReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }
    //protected void gvRetrieveReports277_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    RefreshData();
    //}

    protected void gvRetrieveReports277_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }
    protected void gvPASRRReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }
    //protected void gvRetrieveReports278_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    RefreshData();
    //}

    protected void gvRetrieveReports278_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }

    private void BindDropDown()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_Select_RetrieveReport_Type", parms);

        for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = ds.Tables[0].Rows[i];
            if (Convert.ToString(dr["RetrieveReport_Type_DESC"]) == CON.RetrieveReports.POTENTIALLYPREVENTABLEREADMISSIONS || Convert.ToString(dr["RetrieveReport_Type_DESC"]) == CON.RetrieveReports.CLAIMSUBMISSIONRESPONSE
                || Convert.ToString(dr["RetrieveReport_Type_DESC"]) == CON.RetrieveReports.PRIORAUTHSUBMISSIONRESPONSE || (Convert.ToString(dr["RetrieveReport_Type_DESC"]) == CON.RetrieveReports.PASRRReports && AppSettings.Get("EnableSAM684") == "true"))
            {

            }
            else
            {
                dr.Delete();
            }
        }
        ds.Tables[0].AcceptChanges();

        if (Helper.HasRows(ds)) Helper.LoadList(ddlDocumentType, ds.Tables[0], "RetrieveReport_Type_DESC", "RetrieveReport_Type_ID", true);

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
        return isValid;
    }

    public void RefreshData()
    {
        int totalResultCount = 0;
        DataTable dt = GetData(out totalResultCount, gvRetrieveReports.PageSize);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvRetrieveReports.EmptyDataText = "No Reports found.";
        }
        if (ddlDocumentType.SelectedItem.Text == "277 Claim Submission Response")
        {
            pnlReportResult277.Visible = true;
            pnlrReportResult.Visible = false;
            pnlReportResult278.Visible = false;
            gvRetrieveReports277.DataSource = dt;
            gvRetrieveReports277.VirtualItemCount = totalResultCount;
            gvRetrieveReports277.DataBind();
        }
        else if (ddlDocumentType.SelectedItem.Text == "278 Prior Auth Submission Response")
        {
            pnlReportResult277.Visible = false;
            pnlrReportResult.Visible = false;
            pnlReportResult278.Visible = true;
            gvRetrieveReports278.DataSource = dt;
            gvRetrieveReports278.VirtualItemCount = totalResultCount;
            gvRetrieveReports278.DataBind();
        }
        else if (ddlDocumentType.SelectedItem.Text == "PASRR Reports")
        {
            pnlPASRRReports.Visible = false;
            /*pnlrReportResult.Visible = false;*/
            pnlPASRRReports.Visible = true;
            gvPASRRReports.DataSource = dt;
            gvPASRRReports.VirtualItemCount = totalResultCount;
            gvPASRRReports.DataBind();
        }
        else
        {
            divRRSearchHeader.InnerText = "Potentially Preventable Readmissions Report Search Result Screen";
            pnlReportResult277.Visible = false;
            pnlrReportResult.Visible = true;
            pnlReportResult278.Visible = false;
            gvRetrieveReports.DataSource = dt;
            gvRetrieveReports.VirtualItemCount = totalResultCount;
            gvRetrieveReports.DataBind();
        }
    }

    private DataTable GetData(out int totalResultCount, int pageSize)
    {
        string MedicaidNumber = this.WorkflowPage.MedicaidID;   //Convert.ToString(Request.QueryString["MedicaidNumber"]);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        string sortColWithDirection = this.gvRetrieveReports.GridViewSortDirection == SortDirection.Descending ? gvRetrieveReports.GridViewSortColumn + " DESC" : gvRetrieveReports.GridViewSortColumn;
        totalResultCount = 0;
        Dictionary<string, object> parms = new Dictionary<string, object>();

        parms.Add("DocumentTypeID", ddlDocumentType.SelectedValue);
        parms.Add("DateAvailableFrom", txtDateAvailableFrom.Text);
        parms.Add("DateAvailableTo", txtDateAvailableTo.Text);
        parms.Add("MedicaidID", MedicaidNumber);

        parms.Add("SortExpression", sortColWithDirection);
        parms.Add("PageSize", pageSize);
        parms.Add("StartRowIndex", gvRetrieveReports.CurrentRowIndex);
        parms.Add("GetTotalResultCount", true);

        // If list of IDs has been passed in, display in search results list.
        ds = psc.SearchRetrieveReports(parms, ddlDocumentType.SelectedItem.Text, out totalResultCount);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    public override bool SaveData()
    {
        return true;
    }

    protected void gvRetrieveReports_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "OnFileDownload")
        {
            bool fileDownloaded = false;
            int idxRetrieveReports = Convert.ToInt32(this.gvRetrieveReports.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
            string filename = this.gvRetrieveReports.DataKeys[index].Values["FileName"].ToString();
            DownloadFile(filename, out fileDownloaded);
            if (fileDownloaded)
            {
                updateDownloadDate(idxRetrieveReports);
            }
            RefreshData();

        }
    }




    protected void gvRetrieveReports277_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "OnFileDownload")
        {
            bool fileDownloaded = false;
            int idxRetrieveReports = Convert.ToInt32(this.gvRetrieveReports277.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
            string filename = this.gvRetrieveReports277.DataKeys[index].Values["FileName"].ToString();
            string name = this.gvRetrieveReports277.DataKeys[index].Values["Name"].ToString();
            string htmBody = this.gvRetrieveReports277.DataKeys[index].Values["HTML_BODY"].ToString();
            DownloadFileNew(name, filename, htmBody, out fileDownloaded);
            if (fileDownloaded)
            {
                updateDownloadDate(idxRetrieveReports);
            }
            RefreshData();

        }
    }
    protected void gvPASRRReports_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "OnFileDownload")
        {
            bool fileDownloaded = false;
            int idxRetrieveReports = Convert.ToInt32(this.gvPASRRReports.DataKeys[index].Values["DOCUMENT_ID"].ToString());
            string filename = this.gvPASRRReports.DataKeys[index].Values["FileName"].ToString();
            int docid = Convert.ToInt32(this.gvPASRRReports.DataKeys[index].Values["DOCUMENT_ID"]);
            DownloadFileByDocID(filename,docid,  out fileDownloaded);
            if (fileDownloaded)
            {
                updateDownloadDate(idxRetrieveReports);
            }
            RefreshData();

        }
    }
    protected void gvRetrieveReports278_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "OnFileDownload")
        {
            bool fileDownloaded = false;
            int idxRetrieveReports = Convert.ToInt32(this.gvRetrieveReports278.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
            string filename = this.gvRetrieveReports278.DataKeys[index].Values["FileName"].ToString();
            string name = this.gvRetrieveReports278.DataKeys[index].Values["Name"].ToString();
            string htmBody = this.gvRetrieveReports278.DataKeys[index].Values["HTML_BODY"].ToString();
            DownloadFileNew(name, filename, htmBody, out fileDownloaded);
            if (fileDownloaded)
            {
                updateDownloadDate(idxRetrieveReports);
            }
            RefreshData();

        }
    }


    protected void OnFileDownload(object sender, EventArgs e)
    {

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

    private void DownloadFileNew(string name, string fileName, string body, out bool fileDownloaded)
    {
        fileDownloaded = false;
        try
        {
            string pdfName = name+".pdf";
            string htmName = fileName;
            string GenericTemplatePath = AppSettings.Get("FileStorePath", string.Empty);
            string[] finalDoc = new string[1];
            finalDoc[0] = body;
            File.AppendAllLines(GenericTemplatePath + fileName, finalDoc);
            ProcessDocumentController.CreatePDFFromHTML(GenericTemplatePath, fileName, true);
            byte[] abb = File.ReadAllBytes(GenericTemplatePath + pdfName);
            File.Delete(GenericTemplatePath + htmName);
            File.Delete(GenericTemplatePath + pdfName);
            
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + pdfName);
            
            HttpContext.Current.Response.BinaryWrite(abb);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End(); 
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
    private void DownloadFileByDocID(string fileName, int docid, out bool fileDownloaded)
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

            // If isFileLocal is false, that means file is on onbase. 
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
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath,docid );
            if (fileDownloaded && decryptedFile.Length > 0)
            {
                updateDownloadDate(docid);
            }
            RefreshData();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(decryptedFile);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();
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

    private void DownloadFile(string fileName, out bool fileDownloaded)
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

            // If isFileLocal is false, that means file is on onbase. 
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
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(decryptedFile);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();
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

    public override void LoadControlData()
    {

    }

    public override void LoadData(DataRow dr)
    {
    }

    public override string ValidationGroup
    {
        get { return "valRetrieveReports"; }
    }

    public override string Title
    {
        get { return "Retrieve Reports"; }
    }

    public override string IdText
    {
        get { return "ucRetrieveReports_" + this.WorkflowPage.RegistrationId; }
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valRetrieveReports";
        this.Page.Validators.Add(val);
        isGood = false;
    }

}