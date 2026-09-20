using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MAXIMUS.Models.Data.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Correspondence : BaseSectionControl
{
    #region 
    private PDMSService.PDMSServiceClient _spa;
    private int totalResultCount = 0;
    private CorrespodenceInfo data = null;
    private String CurrentSortOrder = "DESC";
    private String CurrentSortField = "DATE_SENT";


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

    private int RegID
    {
        get
        {
            return this.WorkflowPage.RegistrationId;
        }
        set
        {
            ViewState["RegID"] = value;
            if (value > 0)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", value.ToString());
                parms.Add("UserID", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectRegistrationHeader", parms);
                if (Helper.HasRows(ds))
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    this.MedicaidID = Helper.GetString("MEDICAID_ID", row);
                    this.NPI = Helper.GetString("NPI", row);
                }

            }
        }
    }

    private string MedicaidID
    {
        get
        {
            return this.WorkflowPage.MedicaidID;
        }
        set
        {
            ViewState["MedicaidID"] = value;
        }
    }

    private string NPI
    {
        get
        {
            if (ViewState["NPI"] != null)
            {
                return ViewState["NPI"].ToString();
            }
            else
            {
                return "";
            }
            
        }
        set
        {
            ViewState["NPI"] = value;
        }
    }


    private string UserEmailAddress
    {
        get
        {
            return ViewState["UserEmailAddress"].ToString();
        }
        set
        {
            ViewState["UserEmailAddress"] = value;
        }
    }

    private string ContactRegEmailAddress
    {
        get
        {
            return ViewState["ContactEmailAddress"] == null ? string.Empty : ViewState["ContactEmailAddress"].ToString();
        }
        set
        {
            ViewState["ContactEmailAddress"] = value;
        }
    }

    private string CredentialingEmailAddress
    {
        get
        {
            return ViewState["CredentialingEmailAddress"] == null ? string.Empty : ViewState["CredentialingEmailAddress"].ToString();
        }
        set
        {
            ViewState["CredentialingEmailAddress"] = value;
        }
    }
    private Guid UserID
    {
        get
        {
            return Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        }
    }

    private int GridViewCurrentPage
    {
        get { return ViewState["GridViewCurrentPage"] == null ? 0 : (int)ViewState["GridViewCurrentPage"]; }
        set { ViewState["GridViewCurrentPage"] = value; }
    }

    private string GridViewSortDirection
    {
        get { return ViewState["GridViewSortDirection"] == null ? "ASC" : ViewState["GridViewSortDirection"].ToString(); }
        set { ViewState["GridViewSortDirection"] = value; }
    }

    private string GridViewSortExpression
    {
        get { return ViewState["GridViewSortExpression"] == null ? "SUBJECT" : ViewState["GridViewSortExpression"].ToString(); }
        set { ViewState["GridViewSortExpression"] = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            try
            {
                // BindDropDown();
                GetCorrespondenceType();
                SetDateVisibility();
            }
            catch (Exception ex)
            {
                MessageBox2.Show("An error occurred when loading the work queue.<br>Error: " + ex.Message + " " + ex.StackTrace, "Error");
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

            sepCorrespondence.InnerText = sepCorrespondence.InnerText.Replace('+', '-');
            sepCorrespondencetype.InnerText = sepCorrespondencetype.InnerText.Replace('-', '+');

        }
    }

    private void LoadPASTATUS()
    {

    }
    public override void LoadData(DataRow dr)
    {
    }

    public void InitView(ProviderManagerData data)
    {
        this.MedicaidID = data.MedicaidID;
        this.RegID = data.RegID;
        SetUserEmailAddress();

        GridViewCurrentPage = 1;
        GridViewSortDirection = "ASC";
        GridViewSortExpression = "SUBJECT";

    }

    private void SetDateVisibility()
    {
        calExtDateAvailableFrom.EndDate = DateTime.Today;
        calExtDateAvailableTo.EndDate = DateTime.Today;
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

        if (string.IsNullOrEmpty(this.RegID.ToString()))
        {
            AddError("* RegID is invalid.", ref isValid);
        }

        return isValid;
    }

    public override string ValidationGroup
    {
        get { return "valCorrespondence"; }
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valCorrespondence";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public override string Title
    {
        get { return "CORRESPONDENCE SEARCH"; }
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

        if (Page.IsValid)
        {

            this.gvCorrespondencesearch.PageIndex = 0;
            if (ValidateData())
                Search(true);

        }
    }
    private void Search(bool clearIds)
    {
        //divCorrespondenceHeader.Visible = clearIds;
        this.gvCorrespondencesearch.PageIndex = 0;
        RefreshData();
        BindGrid(data);
    }
    protected void PageSize_Changed(object sender, EventArgs e)
    {
        //this.GetCustomersPageWise(1);
    }

    private void GetCorrespondenceType()
    {

        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlCorrespondenceType.Items.Clear();
        DataSet dataSet = _spa.GetCorrespondenceType();
        DataTable dt = dataSet.Tables[0];
        dt.DefaultView.Sort = "CORRESPONDENCE_TYPE_DESC";
        Helper.LoadList(ddlCorrespondenceType, dt, "CORRESPONDENCE_TYPE_DESC", "CORRESPONDENCE_TYPE_ID", true);

    }

    public void RefreshData()
    {
        data = null;
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        var correspondeceType = 0;
        if (ddlCorrespondenceType.SelectedValue != "")
        {
            correspondeceType = ddlCorrespondenceType.SelectedValue != "" ? Convert.ToInt32(ddlCorrespondenceType.SelectedValue) : 0;
        }

        DateTime? fromDate = null;
        DateTime? toDate = null;
        if (!string.IsNullOrEmpty(txtDateAvailableFrom.Text))
        {
            fromDate = Convert.ToDateTime(txtDateAvailableFrom.Text);
        }
        if (!string.IsNullOrEmpty(txtDateAvailableTo.Text))
        {
            toDate = Convert.ToDateTime(txtDateAvailableTo.Text);
        }
        gvCorrespondencesearch.Attributes["CurrentSortField"] = "DATE_SENT";
        gvCorrespondencesearch.Attributes["CurrentSortDirection"] = "DESC";
        //MedicaidID = "112233456";
        data = _spa.GetSearchCorrespondenceType(correspondeceType, UserID, fromDate, toDate, this.RegID, this.NPI, this.MedicaidID, "DESC", "DATE_SENT", 15, 0);
        hdnRowCount.Value = data.CorrespondenceResultCount.ToString();
    }

    private CorrespodenceInfo getSortedCorrespondenceData(string sortfield, string sortDirection)
    {
        data = null;
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        var correspondeceType = ddlCorrespondenceType.SelectedValue != "" ? Convert.ToInt32(ddlCorrespondenceType.SelectedValue) : 0;
        DateTime? fromDate = null;
        DateTime? toDate = null;
        if (!string.IsNullOrEmpty(txtDateAvailableFrom.Text))
        {
            fromDate = Convert.ToDateTime(txtDateAvailableFrom.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txtDateAvailableTo.Text))
        {
            toDate = Convert.ToDateTime(txtDateAvailableTo.Text.Trim());
        }
        data = _spa.GetSearchCorrespondenceType(correspondeceType, UserID, fromDate, toDate, this.RegID, this.NPI, this.MedicaidID, sortDirection, sortfield, 15, 0);
        hdnRowCount.Value = data.CorrespondenceResultCount.ToString();
        return data;
    }

    private void BindGrid(CorrespodenceInfo datOabj)
    {
        if (!Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.SiteVisitAdministrator))
        {
            if (datOabj.CorrespondenceInfo != null)
            {
                DataTable dtcorrecpondence = datOabj.CorrespondenceInfo.Tables["CorrespondenceResults"];

                if (dtcorrecpondence != null)
                {
                    foreach (DataRow dr in dtcorrecpondence.Rows)
                    {
                        if ((dr["SUBJECT"].ToString() == "Registration requiring site visit assignment")|| (dr["SUBJECT"].ToString() == ""))
                            dr.Delete();
                    }
                    gvCorrespondencesearch.DataSource = dtcorrecpondence;
                    gvCorrespondencesearch.VirtualItemCount = dtcorrecpondence.Rows.Count;
                    gvCorrespondencesearch.DataBind();
                }
                
            }
            else
            {                
                gvCorrespondencesearch.DataSource = null;
                gvCorrespondencesearch.DataBind();
            }

            if(datOabj.ConvertedCorrespondenceInfo !=null)
            {
                var dt = datOabj.ConvertedCorrespondenceInfo.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    gvConvertedCorrespondencesearch.DataSource = datOabj.ConvertedCorrespondenceInfo.Tables[0];
                }

                gvConvertedCorrespondencesearch.VirtualItemCount = datOabj.ConvertedCorrespondenceResultCount;
                gvConvertedCorrespondencesearch.DataBind();

            }
            else
            {
                gvConvertedCorrespondencesearch.DataSource = null;
                gvConvertedCorrespondencesearch.DataBind();
            }
        }
        else
        {
            if (datOabj.CorrespondenceInfo != null)
            {
                gvCorrespondencesearch.DataSource = datOabj.CorrespondenceInfo.Tables[0];
                gvCorrespondencesearch.VirtualItemCount = datOabj.CorrespondenceResultCount;
                gvCorrespondencesearch.DataBind();                
            }
            else
            {
                gvCorrespondencesearch.DataSource = null;
                gvCorrespondencesearch.DataBind();
            }

            if (datOabj.ConvertedCorrespondenceInfo != null)
            {
                var dt = datOabj.ConvertedCorrespondenceInfo.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    gvConvertedCorrespondencesearch.DataSource = datOabj.ConvertedCorrespondenceInfo.Tables[0];
                }

                gvConvertedCorrespondencesearch.VirtualItemCount = datOabj.ConvertedCorrespondenceResultCount;
                gvConvertedCorrespondencesearch.DataBind();

            }
            else
            {
                gvConvertedCorrespondencesearch.DataSource = null;
                gvConvertedCorrespondencesearch.DataBind();
            }
        }
    }


    protected void gvCorrespondencesearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCorrespondencesearch.PageIndex = e.NewPageIndex;
        if (ViewState["CurSortField"] != null && ViewState["CurrentSortDirection"] != null)
        {
            CurrentSortField = (string)ViewState["CurSortField"];
            CurrentSortOrder = (string)ViewState["CurrentSortDirection"];
            var dataObj = getSortedCorrespondenceData(CurrentSortField, CurrentSortOrder);
            BindGrid(dataObj);
        }
        else
        {
            RefreshData();
            BindGrid(data);
        }
    }
    protected void gvConvertedCorrespondencesearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvConvertedCorrespondencesearch.PageIndex = e.NewPageIndex;
        if (ViewState["CurSortField"] != null && ViewState["CurrentSortDirection"] != null)
        {
            CurrentSortField = (string)ViewState["CurSortField"];
            CurrentSortOrder = (string)ViewState["CurrentSortDirection"];
            var dataObj = getSortedCorrespondenceData(CurrentSortField, CurrentSortOrder);
            BindGrid(dataObj);
        }
        else
        {
            RefreshData();
            BindGrid(data);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clearFields();
    }

    private void clearFields()
    {
        gvCorrespondencesearch.EmptyDataText = txtDateAvailableFrom.Text = txtDateAvailableFrom.Text = String.Empty;
        txtDateAvailableTo.Text = string.Empty;
        txtDateAvailableFrom.Text = string.Empty;
        gvCorrespondencesearch.DataSource = null;
        gvCorrespondencesearch.DataBind();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpeEmailPreview.Hide();
        if (ViewState["CurSortField"] != null && ViewState["CurrentSortDirection"] != null)
        {
            CurrentSortField = (string)ViewState["CurSortField"];
            CurrentSortOrder = (string)ViewState["CurrentSortDirection"];
            var dataObj = getSortedCorrespondenceData(CurrentSortField, CurrentSortOrder);
            BindGrid(data);
        }
        else
        {
            RefreshData();
            BindGrid(data);
        }
    }

    protected void gvCorrespondencesearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (gvCorrespondencesearch.Attributes["CurrentSortDirection"] == "ASC")
        {
            gvCorrespondencesearch.Attributes["CurrentSortDirection"] = "DESC";
            CurrentSortOrder = "DESC";
        }
        else
        {
            gvCorrespondencesearch.Attributes["CurrentSortDirection"] = "ASC";
            CurrentSortOrder = "ASC";
        }
        CurrentSortField = e.SortExpression;
        ViewState["CurSortField"] = e.SortExpression;
        ViewState["CurrentSortDirection"] = gvCorrespondencesearch.Attributes["CurrentSortDirection"].ToString();
        gvCorrespondencesearch.Attributes["CurrentSortField"] = CurrentSortField;
        var dataObj = getSortedCorrespondenceData(CurrentSortField, CurrentSortOrder);
        BindGrid(data);
    }

    protected void gvConvertedCorrespondencesearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (gvConvertedCorrespondencesearch.Attributes["CurrentSortDirection"] == "ASC")
        {
            gvConvertedCorrespondencesearch.Attributes["CurrentSortDirection"] = "DESC";
            CurrentSortOrder = "DESC";
        }
        else
        {
            gvConvertedCorrespondencesearch.Attributes["CurrentSortDirection"] = "ASC";
            CurrentSortOrder = "ASC";
        }
        CurrentSortField = e.SortExpression;
        ViewState["CurSortField"] = e.SortExpression;
        ViewState["CurrentSortDirection"] = gvConvertedCorrespondencesearch.Attributes["CurrentSortDirection"].ToString();
        gvConvertedCorrespondencesearch.Attributes["CurrentSortField"] = CurrentSortField;
        var dataObj = getSortedCorrespondenceData(CurrentSortField, CurrentSortOrder);
        BindGrid(data);
    }
    protected void ddlCorrespondenceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        clearFields();
        SetDateVisibility();
    }
    private string COMMUNICATION_EVENT_ID
    {
        get
        {
            return ViewState["EmailNotifications_CEI"].ToString();
        }
        set
        {
            ViewState["EmailNotifications_CEI"] = value;
        }
    }
    protected void lb_Click(object sender, CommandEventArgs e)
    {

        if (!Helper.HasRows(dtAttachments))
            return;

        DataRow row = dtAttachments.Select("COMMUNICATION_EVENT_ID = " + e.CommandArgument)[0];
        string filename = CreateAttachmentDocument(row).Replace(".pdf", string.Empty);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", "window.open('../Pages/PdfViewer.aspx?filename=" + filename + "','_blank');", true);

    }

    //TODO:  
    private DataTable _dtAttachments;
    private DataTable dtAttachments
    {
        get
        {
            return _dtAttachments;
        }
        set
        {
            _dtAttachments = value;
        }
    }
    private static string CreateAttachmentDocument(DataRow row)
    {
        string templatesDirectory = MAXIMUS.Core.Libraries.AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
        string pdfDirectory = templatesDirectory + "Temporary_Files";
        string filename = row["ATTACHMENT_FILE_NAME"].ToString();
        if (!Directory.Exists(pdfDirectory)) Directory.CreateDirectory(pdfDirectory);
        if (pdfDirectory.LastIndexOf("\\") != pdfDirectory.Length - 1) pdfDirectory += "\\";
        string ToSaveFileTo = pdfDirectory + filename;
        byte[] fileData = (byte[])row["PDF_FILE"];
        using (System.IO.FileStream fs = new System.IO.FileStream(ToSaveFileTo, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
        {
            using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))
            {
                bw.Write(fileData);
                bw.Close();
            }
        }

        return filename;
    }
    protected void gvCorrespondencesearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index;
        bool isValid = Int32.TryParse(e.CommandArgument.ToString(), out index);

        if ((e.CommandName == "OpenEnrollment") && isValid)
        {
            var correspondeceType = 0;
            if (ddlCorrespondenceType.SelectedValue != "")
            {
                correspondeceType = ddlCorrespondenceType.SelectedValue != "" ? Convert.ToInt32(ddlCorrespondenceType.SelectedValue) : 0;
            }
            //if (correspondeceType == 1)
            //{
            int i;
            if (!int.TryParse(e.CommandArgument.ToString(), out i))
                return;

            COMMUNICATION_EVENT_ID = gvCorrespondencesearch.DataKeys[i].Value.ToString();
            txtSubject.Text = gvCorrespondencesearch.DataKeys[i]["SUBJECT"].ToString().Replace("(State)", "Ohio");
            if (gvCorrespondencesearch.DataKeys[i]["SUBJECT"].ToString().Replace("(State)", "Ohio").Trim().ToUpper() == "SECONDBACKGROUNDNOTICEWITHFBI")
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.RegID.ToString());

                DataSet dsReg = psc.SelectRegistrationDataWithParams("usp_SelectREG_BACKGROUND_CHECK", parms);

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                string background_date = string.Empty;
                string ProviderName = string.Empty;
                if (drReg != null)
                {
                    background_date = !string.IsNullOrEmpty(ObjectControllerHelper.GetString("DateOfInitialBCNotice", drReg)) ? ObjectControllerHelper.GetDateTime("DateOfInitialBCNotice", drReg).AddDays(30).ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
                    ProviderName = ObjectControllerHelper.GetString("Name", drReg);

                }
                var body = Convert.ToString(gvCorrespondencesearch.DataKeys[i]["BODY"]);
                SetupBody((body != null) ? body.Replace("(30) calendar days, or by are:", "(30) calendar days, or by  " + background_date + " are:" + ProviderName).Replace(" If we do not receive your fingerprints by", " If we do not receive your fingerprints by " + background_date) : body);

            }
            else
            {
                SetupBody(Convert.ToString(gvCorrespondencesearch.DataKeys[i]["BODY"]));
            }

            if (gvCorrespondencesearch.DataKeys[i]["DOCUMENT_ID"] != null && gvCorrespondencesearch.DataKeys[i]["ELIGIBILITY"] != null && gvCorrespondencesearch.DataKeys[i]["ELIGIBILITY"].ToString() == "TRUE" && gvCorrespondencesearch.DataKeys[i]["DATE_VIEWED"].ToString() == "NA")
            {
                updateDownloadDate(Convert.ToInt32(gvCorrespondencesearch.DataKeys[i]["DOCUMENT_ID"]));
            }

            SetupAttachments();

            SetupSendTo(gvCorrespondencesearch.DataKeys[i]["EMAIL_TO"].ToString());
            upPreview.Update();
            mpeEmailPreview.Show();

            //}
        }
        if ((e.CommandName == "OnFileDownload") && isValid)
        {
            int n;
            bool isNumeric = int.TryParse(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ID_PDF"].ToString(), out n);

            if (isNumeric && n != 0)
            {
                int documentID = Convert.ToInt32(n);
                bool fileDownloaded = false;
                //int idRAReports = Convert.ToInt32(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
                string filename = this.gvCorrespondencesearch.DataKeys[index].Values["FILE_NAME_PDF"].ToString();
                DownloadFile(filename, documentID, out fileDownloaded);
                CurrentSortOrder = this.gvCorrespondencesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                RefreshData(CurrentSortOrder);
            }
            else
            {
                bool fileDownloaded = false;
                int docAttachmentXrefId = Convert.ToInt32(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
                string filename = this.gvCorrespondencesearch.DataKeys[index].Values["FILE_NAME"].ToString();
                int documentID = Convert.ToInt32(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ID"].ToString());
                string docType = this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_TYPE"].ToString();
                string uuid = "12345";//this.gvCorrespondencesearch.DataKeys[index].Values["UUID"].ToString();

                if (docType == "txt")
                {
                    DownloadPDFSharpFile(docAttachmentXrefId, filename, documentID, uuid, out fileDownloaded);
                    if (fileDownloaded)
                    {
                        updateDownloadDate(documentID);
                    }
                    CurrentSortOrder = this.gvCorrespondencesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                    RefreshData(CurrentSortOrder);
                }
                else if (docType == "zip")
                {
                    DownloadPDFZipFile(docAttachmentXrefId, filename, documentID, uuid, out fileDownloaded);
                    if (fileDownloaded)
                    {
                        updateDownloadDate(documentID);
                    }
                    CurrentSortOrder = this.gvCorrespondencesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                    RefreshData(CurrentSortOrder);
                }
                else
                {
                    DownloadFile(filename, documentID, out fileDownloaded);
                    if (fileDownloaded)
                    {
                        updateDownloadDate(documentID);
                    }
                    CurrentSortOrder = this.gvCorrespondencesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                    RefreshData(CurrentSortOrder);
                }
            }
        }
        if ((e.CommandName == "OnFileDownloadPDF_Zip") && isValid)
        {
            int n;
            var dpdf = this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ID_PDF"];
            bool isNumeric = int.TryParse(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ID_PDF"].ToString(), out n);


            if (isNumeric && n != 0)
            {
                int documentID = Convert.ToInt32(n);
                bool fileDownloaded = false;
                //int idRAReports = Convert.ToInt32(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
                string filename = this.gvCorrespondencesearch.DataKeys[index].Values["FILE_NAME_PDF"].ToString();
                DownloadFile(filename, documentID, out fileDownloaded);
                CurrentSortOrder = this.gvCorrespondencesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                RefreshData(CurrentSortOrder);
            }
            else
            {

                bool fileDownloaded = false;
                int docAttachmentXrefId = Convert.ToInt32(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());
                string filename = this.gvCorrespondencesearch.DataKeys[index].Values["FILE_NAME"].ToString();
                int documentID = Convert.ToInt32(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ID"].ToString());
                string docType = this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_TYPE"].ToString();
                string uuid = "123456";//this.gvCorrespondencesearch.DataKeys[index].Values["UUID"].ToString();

                if (docType == "zip")
                {
                    DownloadPDFZipFile(docAttachmentXrefId, filename, documentID, uuid, out fileDownloaded);
                    if (fileDownloaded)
                    {
                        if (ddlCorrespondenceType.SelectedValue.Equals("6"))
                        {
                            UpdateDownloadDatePA(docAttachmentXrefId);
                        }
                        else if (ddlCorrespondenceType.SelectedValue.Equals("12"))
                        {
                            UpdateDownloadDateHospice(docAttachmentXrefId);
                        }
                        else
                        {
                            updateDownloadDate(documentID);
                        }
                    }
                    CurrentSortOrder = this.gvCorrespondencesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
                    RefreshData(CurrentSortOrder);
                }
            }
        }

        if (e.CommandName == "OnFileDownloadPDF" && isValid)
        {
            int n;
            bool isNumeric = int.TryParse(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ID_PDF"].ToString(), out n);
            bool fileDownloaded = false;
            string operation_name = "view";
            int documentID = 0;
            string filename = string.Empty;
            int docAttachmentXrefId = Convert.ToInt32(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ATTACHMENT_XREF_ID"].ToString());

            if (isNumeric && n != 0)
            {
                documentID = Convert.ToInt32(n);
                filename = this.gvCorrespondencesearch.DataKeys[index].Values["FILE_NAME_PDF"].ToString();
            }
            else
            {
                filename = this.gvCorrespondencesearch.DataKeys[index].Values["FILE_NAME"].ToString();
                documentID = Convert.ToInt32(this.gvCorrespondencesearch.DataKeys[index].Values["DOCUMENT_ID"].ToString());
            }
            DownloadFile(filename, documentID, out fileDownloaded, operation_name);
            CurrentSortOrder = this.gvCorrespondencesearch.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
            RefreshData(CurrentSortOrder);

        }

        if (e.CommandName.Equals("subject"))
        {
            var correspondeceType = 0;
            if (ddlCorrespondenceType.SelectedValue != "")
            {
                correspondeceType = ddlCorrespondenceType.SelectedValue != "" ? Convert.ToInt32(ddlCorrespondenceType.SelectedValue) : 0;
            }
            if (correspondeceType == 1)
            {
                int i;
                if (!int.TryParse(e.CommandArgument.ToString(), out i))
                    return;

                COMMUNICATION_EVENT_ID = gvCorrespondencesearch.DataKeys[i].Value.ToString();
                txtSubject.Text = gvCorrespondencesearch.DataKeys[i]["SUBJECT"].ToString().Replace("(State)", "Ohio");
                if (gvCorrespondencesearch.DataKeys[i]["SUBJECT"].ToString().Replace("(State)", "Ohio").Trim().ToUpper() == "SECONDBACKGROUNDNOTICEWITHFBI")
                {
                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.RegID.ToString());

                    DataSet dsReg = psc.SelectRegistrationDataWithParams("usp_SelectREG_BACKGROUND_CHECK", parms);

                    dsReg.Tables[0].TableName = "RegData";
                    DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                    string background_date = string.Empty;
                    string ProviderName = string.Empty;
                    if (drReg != null)
                    {
                        background_date = !string.IsNullOrEmpty(ObjectControllerHelper.GetString("DateOfInitialBCNotice", drReg)) ? ObjectControllerHelper.GetDateTime("DateOfInitialBCNotice", drReg).AddDays(30).ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
                        ProviderName = ObjectControllerHelper.GetString("Name", drReg);

                    }
                    var body = Convert.ToString(gvCorrespondencesearch.DataKeys[i]["BODY"]);
                    SetupBody((body != null) ? body.Replace("(30) calendar days, or by are:", "(30) calendar days, or by  " + background_date + " are:" + ProviderName).Replace(" If we do not receive your fingerprints by", " If we do not receive your fingerprints by " + background_date) : body);

                }
                else
                {
                    SetupBody(Convert.ToString(gvCorrespondencesearch.DataKeys[i]["BODY"]));
                    insertCommunicationEventHistory(Convert.ToInt32(gvCorrespondencesearch.DataKeys[i]["COMMUNICATION_EVENT_ID"]));
                }

                if (gvCorrespondencesearch.DataKeys[i]["DOCUMENT_ID"] != null && gvCorrespondencesearch.DataKeys[i]["ELIGIBILITY"] != null && gvCorrespondencesearch.DataKeys[i]["ELIGIBILITY"].ToString() == "TRUE")
                {
                    updateDownloadDate(Convert.ToInt32(gvCorrespondencesearch.DataKeys[i]["DOCUMENT_ID"]));
                }

                SetupAttachments();

                SetupSendTo(gvCorrespondencesearch.DataKeys[i]["EMAIL_TO"].ToString());
                upPreview.Update();
                mpeEmailPreview.Show();

            }

            if (ViewState["CurSortField"] != null && ViewState["CurrentSortDirection"] != null)
            {
                CurrentSortField = (string)ViewState["CurSortField"];
                CurrentSortOrder = (string)ViewState["CurrentSortDirection"];
                var dataObj = getSortedCorrespondenceData(CurrentSortField, CurrentSortOrder);
                BindGrid(data);
            }
            else
            {
                RefreshData();
                BindGrid(data);
            }

        }
    }


    private void DownloadPDFZipFile(int docAttachmentXrefId, string fileName, int documentID, string uuid, out bool fileDownloaded)
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

            string isEncrypted = "Y";

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

            UpdateOnBasePDFID(docAttachmentXrefId, pdfDocumentID);

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


    public void RefreshData(string sortDirection)
    {
        data = null;
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        var correspondeceType = 0;
        if (ddlCorrespondenceType.SelectedValue != "")
        {
            correspondeceType = ddlCorrespondenceType.SelectedValue != "" ? Convert.ToInt32(ddlCorrespondenceType.SelectedValue) : 0;
        }

        DateTime? fromDate = null;
        DateTime? toDate = null;
        if (!string.IsNullOrEmpty(txtDateAvailableFrom.Text))
        {
            fromDate = Convert.ToDateTime(txtDateAvailableFrom.Text);
        }
        if (!string.IsNullOrEmpty(txtDateAvailableTo.Text))
        {
            toDate = Convert.ToDateTime(txtDateAvailableTo.Text);
        }
        gvCorrespondencesearch.Attributes["CurrentSortField"] = "DATE_SENT";
        gvCorrespondencesearch.Attributes["CurrentSortDirection"] = sortDirection;
        //MedicaidID = "112233456";
        data = _spa.GetSearchCorrespondenceType(correspondeceType, UserID, fromDate, toDate, this.RegID, this.NPI, this.MedicaidID, sortDirection, "DATE_SENT", 15, 0);
        hdnRowCount.Value = data.CorrespondenceResultCount.ToString();
    }


    protected void gvConvertedCorrespondencesearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("subject"))
        {
            int i;
            if (!int.TryParse(e.CommandArgument.ToString(), out i))
                return;

            COMMUNICATION_EVENT_ID = gvConvertedCorrespondencesearch.DataKeys[i].Value.ToString();
            txtSubject.Text = gvConvertedCorrespondencesearch.DataKeys[i]["DOCUMENT_TYPE_DESC"].ToString().Replace("(State)", "Ohio");
            if (gvConvertedCorrespondencesearch.DataKeys[i]["DOCUMENT_TYPE_DESC"].ToString().Replace("(State)", "Ohio").Trim().ToUpper() == "SECONDBACKGROUNDNOTICEWITHFBI")
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.RegID.ToString());

                DataSet dsReg = psc.SelectRegistrationDataWithParams("usp_SelectREG_BACKGROUND_CHECK", parms);

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                string background_date = string.Empty;
                string ProviderName = string.Empty;
                if (drReg != null)
                {
                    background_date = !string.IsNullOrEmpty(ObjectControllerHelper.GetString("DateOfInitialBCNotice", drReg)) ? ObjectControllerHelper.GetDateTime("DateOfInitialBCNotice", drReg).AddDays(30).ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
                    ProviderName = ObjectControllerHelper.GetString("Name", drReg);

                }
                var body = Convert.ToString(gvConvertedCorrespondencesearch.DataKeys[i]["BODY"]);
                SetupBody((body != null) ? body.Replace("(30) calendar days, or by are:", "(30) calendar days, or by  " + background_date + " are:" + ProviderName).Replace(" If we do not receive your fingerprints by", " If we do not receive your fingerprints by " + background_date) : body);

            }
            else
            {
                SetupBody(Convert.ToString(gvConvertedCorrespondencesearch.DataKeys[i]["BODY"]));
            }

            if (gvConvertedCorrespondencesearch.DataKeys[i]["DOCUMENT_ID"] != null && gvConvertedCorrespondencesearch.DataKeys[i]["ELIGIBILITY"] != null && gvConvertedCorrespondencesearch.DataKeys[i]["ELIGIBILITY"].ToString() == "TRUE" && gvConvertedCorrespondencesearch.DataKeys[i]["DATE_VIEWED"].ToString() == "NA")
            {
                updateDownloadDate(Convert.ToInt32(gvConvertedCorrespondencesearch.DataKeys[i]["DOCUMENT_ID"]));
            }

            SetupAttachments();

            SetupSendTo(gvConvertedCorrespondencesearch.DataKeys[i]["EMAIL_TO"].ToString());
            upPreview.Update();
            mpeEmailPreview.Show();
            if (ViewState["CurSortField"] != null && ViewState["CurrentSortDirection"] != null)
            {
                CurrentSortField = (string)ViewState["CurSortField"];
                CurrentSortOrder = (string)ViewState["CurrentSortDirection"];
                var dataObj = getSortedCorrespondenceData(CurrentSortField, CurrentSortOrder);
                BindGrid(data);
            }
            else
            {
                RefreshData();
                BindGrid(data);
            }
        }
    }

    private void insertCommunicationEventHistory(int communicationEventId)
    {
        try
        {
            DateTime downloadDate = DateTime.Now;
            DateTime lastModifiedDate = DateTime.Now;
            Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent))
                svc.InsertCommunicationEventHistoryEvent(communicationEventId, downloadDate, lastModifiedUser);

        }
        catch (Exception ex)
        {
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");
        }
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



    protected void SendToRequired(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = lboxSendTo.GetSelectedIndices().Length > 0;
    }

    private void SetupSendTo(string emailTo)
    {
        List<string> emailList = new List<string>();

        if (!string.IsNullOrEmpty(emailTo))
            emailList.Add(emailTo);

        if (!string.IsNullOrEmpty(UserEmailAddress) && !emailList.Contains(UserEmailAddress))
            emailList.Add(UserEmailAddress);

        if (!string.IsNullOrEmpty(ContactRegEmailAddress) && !emailList.Contains(ContactRegEmailAddress))
            emailList.Add(ContactRegEmailAddress);

        if (!string.IsNullOrEmpty(CredentialingEmailAddress) && !emailList.Contains(CredentialingEmailAddress))
            emailList.Add(CredentialingEmailAddress);

        lboxSendTo.DataSource = emailList;
        lboxSendTo.DataBind();
    }
    private void SetupAttachments()
    {
        pnlAttachments.Controls.Clear();
        if (Helper.HasRows(dtAttachments))
        {
            lblAttach.Visible = pnlAttachments.Visible = true;

            foreach (DataRow row in dtAttachments.Rows)
            {
                LinkButton lb = new LinkButton();
                lb.Text = row["SUBJECT"].ToString();
                lb.ID = "lbAttachment" + row["COMMUNICATION_EVENT_ID"].ToString();
                lb.CommandArgument = row["COMMUNICATION_EVENT_ID"].ToString();
                lb.Command += new CommandEventHandler(lb_Click);
                pnlAttachments.Controls.Add(lb);
            }
        }
        else
        {
            lblAttach.Visible = pnlAttachments.Visible = false;
        }
    }
    private void SetupBody(string bodyText)
    {
        if (!string.IsNullOrEmpty(bodyText))
        {
            Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
            if (tagRegex.IsMatch(bodyText))
            {
                txtBody.Style["display"] = "none";
                divBody.Style["display"] = "block";
                divBody.InnerHtml = bodyText;
            }
            else
            {
                txtBody.Style["display"] = "block";
                divBody.Style["display"] = "none";
                txtBody.Text = bodyText;
            }
        }

    }

    private void SetUserEmailAddress()
    {
        MembershipUser mbr = Membership.GetUser(UserID);
        if (mbr != null)
            UserEmailAddress = mbr.Email.Trim().ToLower();

    }




    protected void gvCorrespondencesearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ddlCorrespondenceType.SelectedValue.Equals("6"))
        {
            this.gvCorrespondencesearch.Columns[2].Visible = true;
            this.gvCorrespondencesearch.Columns[3].Visible = false;
            this.gvCorrespondencesearch.Columns[4].Visible = true;
        }
        else if (ddlCorrespondenceType.SelectedValue.Equals("12"))
        {
            this.gvCorrespondencesearch.Columns[2].Visible = false;
            this.gvCorrespondencesearch.Columns[3].Visible = true;
            this.gvCorrespondencesearch.Columns[4].Visible = true;
        }
        else
        {
            this.gvCorrespondencesearch.Columns[2].Visible = false;
            this.gvCorrespondencesearch.Columns[3].Visible = false;
            this.gvCorrespondencesearch.Columns[4].Visible = false;
        }

        if (gvCorrespondencesearch.Attributes["CurrentSortField"] != null && gvCorrespondencesearch.Attributes["CurrentSortDirection"] != null)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.HasControls())
                    {
                        LinkButton lnkBtnSort = (LinkButton)cell.Controls[0];
                        if (lnkBtnSort != null && gvCorrespondencesearch.Attributes["CurrentSortField"] == lnkBtnSort.CommandArgument)
                        {
                            Image image = new Image();
                            if (gvCorrespondencesearch.Attributes["CurrentSortDirection"] == "ASC")
                            {
                                image.ImageUrl = "~/Images/icon-arrowup.gif";
                            }
                            else if (gvCorrespondencesearch.Attributes["CurrentSortDirection"] == "DESC")
                            {
                                image.ImageUrl = "~/Images/icon-arrowdown.gif";
                            }
                            cell.Controls.Add(new LiteralControl("&nbsp;"));
                            cell.Controls.Add(image);
                        }
                    }
                }
            }
        }
    }


    private void DownloadFile(string fileName, int documentID, out bool fileDownloaded, string operation = "download")
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
            if (string.Equals(operation, "view") || operation == "view")
            {
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("Content-disposition", "inline; filename=\"" + fileName + "\"");
            }
            else
            {
                HttpContext.Current.Response.ContentType = "application/force-download";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            }
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

    private void DownloadPDFSharpFile(int docAttachmentXrefId, string fileName, int documentID, string uuid, out bool fileDownloaded)
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

            UpdateOnBasePDFID(docAttachmentXrefId, pdfDocumentID);

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

    private void UpdateOnBasePDFID(int doc_attachment_xref_id, int pdf_document_id)
    {
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("document_attachment_xref_id", DbType.Int32, doc_attachment_xref_id, true));
            parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID_PDF", DbType.Int32, pdf_document_id, true));
            parameters.Add(SqlParms.CreateParameter("Last_Modified_Date_Time", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("Last_Modified_User", DbType.Guid, new Guid("0446D773-C0DD-458D-AC82-E9AD12CC2725"), true));

            if (ddlCorrespondenceType.SelectedValue.Equals("6"))
            {
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, 2, true));
                DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_PA_NOTIF_DTLS", parameters);
            }
            else if (ddlCorrespondenceType.SelectedValue.Equals("12"))
            {
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, 2, true));
                DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_HOSPICE_NOTIF_DTLS", parameters);
            }
            else
            {
                DataAccess.ExecuteScalar("usp_UpdateDocAttachmentXref", parameters);
            }
        }
        catch (Exception ex)
        {
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");
        }
    }

    private void UpdateDownloadDatePA(int doc_attachment_xref_id)
    {
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("document_attachment_xref_id", DbType.Int32, doc_attachment_xref_id, true));
            parameters.Add(SqlParms.CreateParameter("DOWNLOAD_DATE", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("Last_Modified_Date_Time", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("Last_Modified_User", DbType.Guid, Helper.GetUserId(HttpContext.Current.User.Identity.Name), true));
            parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, 2, true));
            DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_PA_NOTIF_DTLS", parameters);
        }
        catch (Exception ex)
        {
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");
        }
    }

    private void UpdateDownloadDateHospice(int doc_attachment_xref_id)
    {
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("document_attachment_xref_id", DbType.Int32, doc_attachment_xref_id, true));
            parameters.Add(SqlParms.CreateParameter("DOWNLOAD_DATE", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("Last_Modified_Date_Time", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("Last_Modified_User", DbType.Guid, Helper.GetUserId(HttpContext.Current.User.Identity.Name), true));
            parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, 2, true));
            DataAccess.ExecuteScalar("usp_InsertUpdateINBOUND_HOSPICE_NOTIF_DTLS", parameters);
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

}