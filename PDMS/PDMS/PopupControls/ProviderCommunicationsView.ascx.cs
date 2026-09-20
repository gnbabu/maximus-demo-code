using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ProviderCommunicationsView : System.Web.UI.UserControl, ICommunicationHistoryView
{
    #region Properties
    private CommunicationHistoryPresenter _presenter;

    public CommunicationHistoryPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new CommunicationHistoryPresenter(this);
            }

            return _presenter;
        }
    }

    public CommunicationEventData Model { get; set; }


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

    private int PartyID
    {
        get
        {
            return ViewState["PartyID"] == null ? 0 : (int)ViewState["PartyID"];
        }
        set
        {
            ViewState["PartyID"] = value;
        }
    }

    private int PaperRequestQueueID
    {
        get
        {
            return ViewState["PaperRequestQueueID"] == null ? 0 : (int)ViewState["PaperRequestQueueID"];
        }
        set
        {
            ViewState["PaperRequestQueueID"] = value;
        }
    }

    private Guid UserID
    {
        get
        {
            return Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        }
    }

    private int RegID
    {
        get
        {
            return ViewState["RegID"] == null ? 0 : (int)ViewState["RegID"];
        }
        set
        {
            ViewState["RegID"] = value;
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

    private int GridViewPageSize
    {
        get { return this.gvCommunications.PageSize == 0 ? 10 : this.gvCommunications.PageSize; }
    }

    private int GridViewCurrentPage
    {
        get { return ViewState["GridViewCurrentPage"] == null ?  0 : (int)ViewState["GridViewCurrentPage"]; }
        set { ViewState["GridViewCurrentPage"] = value; }
    }

    private string GridViewSortDirection
    {
        get { return ViewState["GridViewSortDirection"] == null ?  "ASC" : ViewState["GridViewSortDirection"].ToString(); }
        set { ViewState["GridViewSortDirection"] = value; }
    }

    private string GridViewSortExpression
    {
        get { return ViewState["GridViewSortExpression"] == null ?  "SUBJECT" : ViewState["GridViewSortExpression"].ToString(); }
        set { ViewState["GridViewSortExpression"] = value; }
    }

    private string GetSortDirection()
    {
        switch (GridViewSortDirection)
        {
            case "ASC":
                GridViewSortDirection = "DESC";
                break;
            case "DESC":
                GridViewSortDirection = "ASC";
                break;
        }
        return GridViewSortDirection;
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

    #endregion

    #region Parent Page Events
    public delegate void ErrorEventHandler(Dictionary<string, string> lstErrors);
    public event ErrorEventHandler ErrorEvent;
    #endregion

    #region Page Events
    
    //TODO:  straight copy over need to move to MVP
    //just dumped code in here, but nothing is tied in - rewrite, a lot of this should be in the presenter not in the ui
    public void InitView(ProviderManagerData data)
    {
        this.RegID = data.RegID;
        this.PartyID = data.PartyID;
        SetUserEmailAddress();  

        GridViewCurrentPage = 1;
        GridViewSortDirection = "ASC";
        GridViewSortExpression = "SUBJECT";
        LoadModelFromKeyData(data);
    }

    protected void gvCommunications_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRow row = ((DataRowView)e.Row.DataItem).Row;
            List<string> pairString = new List<string>(Helper.GetString("KEY_VALUE_PAIR", row).Split(','));

            string subject = Helper.GetString("SUBJECT", row);
            string npi = string.Empty;
            string date = Helper.GetDate("LAST_MODIFIED_DATE_TIME", row);

            string printValue = Helper.GetString("COMMUNICATION_EVENT_TYPE", row);

            if (pairString.Count > 0)
            {
                if (subject.ToLower().Contains("affiliation"))
                {
                    npi = GetNpiFromKVP(pairString.Find(s => s.StartsWith("INDIVIDUALNPI")));
                }
                else
                {
                    npi = GetNpiFromKVP(pairString.Find(s => s.StartsWith("NPI")));
                }
            }

            LinkButton lbSub = (LinkButton)e.Row.FindControl("lbtnSubject");
            LinkButton lbNPI = (LinkButton)e.Row.FindControl("lbtnNPI");
            LinkButton lbDate = (LinkButton)e.Row.FindControl("lbtnDate");

            Label lblPrint = (Label)e.Row.FindControl("lblChkPrinted");

            lbSub.Text = subject.Replace("(State)", "Ohio");
            lbNPI.Text = npi;
            lbDate.Text = date;

            if (printValue.Equals(CON.COMMUNICATION_EVENT_TYPE.print))
            {
                lblPrint.Visible = true;
            }
        }
    }

    protected void gvCommunications_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Communication"))
        {
            //pnlPreview.Visible = true;
            int i;
            if (!int.TryParse(e.CommandArgument.ToString(), out i))
                return;

            COMMUNICATION_EVENT_ID = gvCommunications.DataKeys[i].Value.ToString();
            txtSubject.Text = gvCommunications.DataKeys[i]["SUBJECT"].ToString().Replace("(State)", "Ohio");
            if (gvCommunications.DataKeys[i]["SUBJECT"].ToString().Replace("(State)", "Ohio").Trim().ToUpper() == "SECONDBACKGROUNDNOTICEWITHFBI")
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID",this.RegID.ToString());
               
                DataSet dsReg = psc.SelectRegistrationDataWithParams("usp_SelectREG_BACKGROUND_CHECK", parms);

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                string background_date = string.Empty;
                string  ProviderName = string.Empty;
                if (drReg != null)
                {
                    background_date =!string.IsNullOrEmpty(ObjectControllerHelper.GetString("DateOfInitialBCNotice", drReg))? ObjectControllerHelper.GetDateTime("DateOfInitialBCNotice", drReg).AddDays(30).ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
                    ProviderName = ObjectControllerHelper.GetString("Name", drReg);
                    //DateOfReminderBCNotice
                }

                SetupBody(gvCommunications.DataKeys[i]["BODY"].ToString().Replace("(30) calendar days, or by are:", "(30) calendar days, or by  " + background_date + " are:" + ProviderName).Replace(" If we do not receive your fingerprints by", " If we do not receive your fingerprints by " + background_date));
                //SetupBody(gvCommunications.DataKeys[i]["BODY"].ToString().Replace(" If we do not receive your fingerprints by", " If we do not receive your fingerprints by " + background_date));
            }//
            else {
                SetupBody(gvCommunications.DataKeys[i]["BODY"].ToString());
            }
           
            SetupAttachments();
            //SetupSendTo(gvCommunications.DataKeys[i]["EMAIL_TO"].ToString(), gvCommunications.DataKeys[i]["USER_ID"].ToString(), gvCommunications.DataKeys[i]["REG_ID"].ToString(), gvCommunications.DataKeys[i]["PARTY_ID"].ToString());
            SetupSendTo(gvCommunications.DataKeys[i]["EMAIL_TO"].ToString());
            upPreview.Update();
            mpeEmailPreview.Show();
        }
    }

    protected void gvCommunications_Sorting(Object sender, GridViewSortEventArgs e)
    {
        GridViewSortDirection = GetSortDirection();
        RefreshData();
    }

    protected void gvCommunications_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (string.IsNullOrEmpty(GridViewSortExpression)) return;
        GridView GridSort = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            Image img = new Image();
            string imgUrl;

            if (GridViewSortDirection == "ASC")
            {
                imgUrl = "~/images/icon-arrowup.gif";
            }
            else
            {
                imgUrl = "~/images/icon-arrowdown.gif";
            }
            img.ImageUrl = imgUrl;
            for (int c = 0; c < GridSort.Columns.Count; c++)
            {
                if (this.GridViewSortExpression.ToLower().Trim() == GridSort.Columns[c].SortExpression.ToLower().Trim())
                {
                    e.Row.Cells[c].Controls.Add(img);
                    break;
                }
            }
        }
    }

    protected void txtFilter_TextChanged(object sender, EventArgs e)
    {
        RefreshData();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //pnlPreview.Visible = false;
        mpeEmailPreview.Hide();
    }

    protected void btnResend_Click(object sender, EventArgs e)
    {
        Page.Validate("Emails");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v.ValidationGroup.Equals("Emails") && !v.IsValid)
                    return;
            }
            catch
            {
                continue;
            }
        }

        Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
        bool isHTML = tagRegex.IsMatch(divBody.InnerHtml);
        string templatesDirectory = MAXIMUS.Core.Libraries.AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
        string pdfDirectory = templatesDirectory + "Temporary_Files";
        if (pdfDirectory.LastIndexOf("\\") != pdfDirectory.Length - 1) pdfDirectory += "\\";

        List<string> attachmentsList = new List<string>();
        if (dtAttachments != null)
            foreach (DataRow rowAtt in dtAttachments.Rows) attachmentsList.Add(pdfDirectory + rowAtt["ATTACHMENT_FILE_NAME"].ToString());

        StringBuilder sbTo = new StringBuilder();

        for (int i = 0; i < lboxSendTo.Items.Count; i++)
        {
            //If some items are not selected, send to only those.
            if (0 < lboxSendTo.GetSelectedIndices().Length && lboxSendTo.Items.Count < lboxSendTo.GetSelectedIndices().Length)
            {
                if (lboxSendTo.Items[i].Selected)
                {
                    sbTo.Append(lboxSendTo.Items[i].Text);
                    if (i < lboxSendTo.Items.Count - 1) sbTo.Append(";");
                }
            }
            //If all or none of the items are selected, send to all.
            else
            {
                sbTo.Append(lboxSendTo.Items[i].Text);
                if (i < lboxSendTo.Items.Count - 1) sbTo.Append(";");
            }
        }

        if (sbTo.Length == 0) return;

        //Notification n = new Notification();
        //string returned = n.ResendNotification(txtSubject.Text, isHTML ? divBody.InnerHtml : txtBody.Text, isHTML, attachmentsList, sbTo.ToString());
        // todo  display paer communication
        EMailNotification n = new EMailNotification(isHTML ? divBody.InnerHtml : txtBody.Text, txtSubject.Text, sbTo.ToString());
        string returned = n.ResendNotification(isHTML, attachmentsList);
        mpeEmailPreview.Hide();
    }

    protected void lb_Click(object sender, CommandEventArgs e)
    {
        //lb is a dynamically created linkbutton in SetupAttachments.  Rework?
        if (!Helper.HasRows(dtAttachments))
            return;

        DataRow row = dtAttachments.Select("COMMUNICATION_EVENT_ID = " + e.CommandArgument)[0];
        string filename = CreateAttachmentDocument(row).Replace(".pdf", string.Empty);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", "window.open('../Pages/PdfViewer.aspx?filename=" + filename + "','_blank');", true);
        //Response.Write("<script type='text/javascript'>window.open('~/Pages/PdfViewer.aspx?filename=" + filename + "','_blank');</script>");
    }

    protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "PageNo")
        {
            GridViewCurrentPage = Convert.ToInt32(e.CommandArgument);
            RefreshData();
        }
    }

    protected void SendToRequired(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = lboxSendTo.GetSelectedIndices().Length > 0;
    }

    #endregion

    #region Presenter Methods

    public void SetCommunicationHistory(DataSet ds)
    {
        gvCommunications.DataSource = ds;
        gvCommunications.DataBind();
    }

    public void SetEmails(DataSet ds)
    {
        int searchResultsTotalRows = Helper.GetInt("TOTAL", ds.Tables[2].Rows[0]);
        BindPager(searchResultsTotalRows, GridViewCurrentPage);
        gvCommunications.DataSource = ds.Tables[1];
        gvCommunications.DataBind();
        upEmails.Update();
    }

    public void SetEmailAttachments(DataSet ds)
    {
        dtAttachments = Helper.HasRows(ds) ? ds.Tables[0] : new DataTable();
    }

    public void SetContactEmailInfo(DataSet ds)
    {
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ContactRegEmailAddress = Helper.GetString("CONTACT_EMAIL_ADDRESS", dr).Trim().ToLower();
            //PartyID = Helper.GetInt("PARTY_ID", dr);
            //if (PartyID > 0)
            //{
               // Model.PartyID = PartyID;
                presenter.RequestCredentialingEmailAddress();
            //}
        }
        else
        {
            ContactRegEmailAddress = string.Empty;
        }
    }

    public void SetCredentialingEmailInfo(DataSet ds)
    {
        if (Helper.HasRows(ds))
        {
            CredentialingEmailAddress = Helper.GetString("CREDENTIALING_EMAIL", ds.Tables[0].Rows[0]).Trim().ToLower();
        }
        else
        {
            CredentialingEmailAddress = string.Empty;
        }
    }

    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            if (ErrorEvent != null)
                ErrorEvent(presenter.ErrorList);
        }
    }
    #endregion

    #region Private Methods
     private void LoadModelFromKeyData(ProviderManagerData data)
    {
        Guid userId = this.UserID;

        if (Helper.IsUserInPSRoles(userId.ToString()))
            userId = new Guid("00000000-0000-0000-0000-000000000000");

        presenter.Init(RegID, PartyID, userId);
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

    private void SetupBody(string bodyText)
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

    private string GetNpiFromKVP(string kvp)
    {
        if (!string.IsNullOrEmpty(kvp))
        {
            string[] str = kvp.Split(':');
            return str[1];
        }
        else
        {
            return string.Empty;
        }
    }

    private void BindPager(int totalRows, int currentPage)
    {
        int totalPages = (int)Math.Ceiling((decimal)totalRows / this.GridViewPageSize);
        List<ListItem> pagerContainer = new List<ListItem>();
        for (int i = 1; i <= totalPages; i++)
        {
            pagerContainer.Add(new ListItem(i.ToString(), i.ToString(), currentPage == i ? false : true));
        }

        if (pagerContainer.Count > 0) pagerContainer[0].Text = "First";
        if (pagerContainer.Count > 1) pagerContainer[totalPages - 1].Text = "Last";
        dlPager.DataSource = pagerContainer;
        dlPager.DataBind();
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

    public void RefreshData()
    {
        this.ResetSelectedRecord();
        this.RefreshMyCommunications();
    }

    private void RefreshMyCommunications()
    {
        InitModel();

        string subjectFilter = ((TextBox)gvCommunications.HeaderRow.FindControl("txtFilterSubject")).Text;
        string npiFilter = ((TextBox)gvCommunications.HeaderRow.FindControl("txtFilterNPI")).Text;
        ((TextBox)gvCommunications.HeaderRow.FindControl("txtFilterSubject")).Text = subjectFilter;
        ((TextBox)gvCommunications.HeaderRow.FindControl("txtFilterNPI")).Text = npiFilter;

        if (Helper.IsUserInPSRoles(this.UserID.ToString()))
        {
            //set to retrieve for internal eyes only communications?
            Model.UserID = new Guid("00000000-0000-0000-0000-000000000000");
        }

        Model.SortByColName = GridViewSortExpression;
        Model.PageNumber = GridViewCurrentPage;
        Model.RowsPerPage = this.gvCommunications.PageSize;
        Model.SortDirection = GridViewSortDirection;
        presenter.RequestCommunicationHistoryData();
    }

    private void ResetSelectedRecord()
    {
        if (this.gvCommunications.SelectedIndex > -1)
            this.gvCommunications.SelectedIndex = -1;
    }


    private void InitModel()
    {
        if (Model == null)
        {
            presenter.Init();
            Model.UserID = this.UserID;
            Model.RegID = this.RegID;
            Model.PartyID = this.PartyID;
        }
    }

    private void SetUserEmailAddress()
    {
        MembershipUser mbr = Membership.GetUser(UserID);
        if (mbr != null && mbr.Email != null)
            UserEmailAddress = mbr.Email.Trim().ToLower();

    }
    #endregion
}