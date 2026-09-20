using MAXIMUS.Core.Libraries;
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

public partial class UserControls_EmailNotifications : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    
    #region SVC
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

    public int CurrentPageIndex
    {
        get
        {
            return ViewState["CurrentPage"] == null ? 0 : Convert.ToInt32(ViewState["CurrentPage"]);
        }
        set
        {
            ViewState["CurrentPage"] = value;
        }
    }
    private string GridViewSortDirection
    {
        get { return ViewState["GridEmailSortDirection"] == null ? "ASC" : ViewState["GridEmailSortDirection"].ToString(); }
        set { ViewState["GridEmailSortDirection"] = value; }
    }

    private string GridViewSortExpression
    {
        get { return ViewState["GridEmailSortExpression"] == null ? "LAST_MODIFIED_DATE_TIME" : ViewState["GridEmailSortExpression"].ToString(); }
        set { ViewState["GridEmailSortExpression"] = value; }
    }



    private string COMMUNICATION_EVENT_ID
    {
        get
        {
            return ViewState["EmailNotifications_CEI"] != null ? ViewState["EmailNotifications_CEI"].ToString() : null;
        }
        set
        {
            if (ViewState["EmailNotifications_CEI"] != null)
            {
                if (!ViewState["EmailNotifications_CEI"].ToString().Equals(value.ToString()))
                    _dtAttachments = null;
            }

            ViewState["EmailNotifications_CEI"] = value;
        }
    }

    private DataTable _dtAttachments;
    private DataTable dtAttachments
    {
        get
        {
            if (_dtAttachments == null)
            {
                DataSet ds = svc.GetEmailAttachments(COMMUNICATION_EVENT_ID.ToString());
                _dtAttachments = Helper.HasRows(ds) ? ds.Tables[0] : null;
            }

            return _dtAttachments;
        }
    }

    protected DataView SortDataTable()
    {
        try
        {
            DataView dv;
            gvEmail.PageIndex = this.CurrentPageIndex;
            if (dtEmails != null)
            {
                dv = dtEmails.DefaultView;
                if ((GridViewSortExpression != string.Empty) && (GridViewSortDirection != string.Empty))
                    dv.Sort = GridViewSortExpression + " " + GridViewSortDirection;
            }
            else dv = new DataView();
            return dv;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataTable _dtEmails;
    private DataTable dtEmails
    {
        get
        {
            if (_dtEmails == null)
            {
                //sortColWithDirection = ViewState["SDirection"] != null ? (ViewState["SDirection"].ToString() == SortDirection.Descending.ToString() ?  "DESC" : "ASC") : "ASC";
                Guid userid;
                if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name)
                    || Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name))
                    userid = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                else if (!Guid.TryParse(SessionVarRetriever.UserIdSelected, out userid))
                    userid = new Guid("00000000-0000-0000-0000-000000000000");
                else if (HttpContext.Current.Request.Url.AbsolutePath.Contains("Registration.aspx") && Helper.IsUserInPSRoles(System.Web.Security.Membership.GetUser(userid).UserName))
                    userid = new Guid("00000000-0000-0000-0000-000000000000");
                else if(Helper.IsUserInLTCCHOPRole(HttpContext.Current.User.Identity.Name))
                {
                    userid = new Guid("00000000-0000-0000-0000-000000000000");
                }

                // TODO: EDV it may not be correct that this control is part of workflow pages
                DataSet ds = svc.SelectEmailNotifications(userid, this.WorkflowPage.RegistrationId, 1000, 1, null, null, null, null);

                _dtEmails = Helper.HasRows(ds) ? ds.Tables[0] : null;
            }

            return _dtEmails;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            COMMUNICATION_EVENT_ID = string.Empty;
        }
        else
        {
            if (!string.IsNullOrEmpty(COMMUNICATION_EVENT_ID))
            {
                SetupAttachments();
                upPreview.Update();
            }
        }
    }

    protected void gvEmail_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (GridViewSortExpression != e.SortExpression)
        {
            GridViewSortExpression = e.SortExpression;
            GridViewSortDirection = "ASC";
        }
        else
        {
            if (GridViewSortDirection == "ASC") GridViewSortDirection = "DESC";
            else GridViewSortDirection = "ASC";
        }
        Setup();
    }

    protected void gvEmail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void gvEmail_RowCreated(object sender, GridViewRowEventArgs e)
    {
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
                if (GridViewSortExpression.ToString() == GridSort.Columns[c].SortExpression)
                {
                    e.Row.Cells[c].Controls.Add(img);
                    break;
                }
            }
        }
    }

    protected void gvEmail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("ViewDetails"))
        {
            pnlPreview.Visible = true;
            int i;
            if (!int.TryParse(e.CommandArgument.ToString(), out i))
                return;

            COMMUNICATION_EVENT_ID = gvEmail.DataKeys[i].Value.ToString();
            DataRow[] rows = dtEmails.Select("COMMUNICATION_EVENT_ID = " + COMMUNICATION_EVENT_ID);
            if (rows.Length == 0) return;
            DataRow row = rows[0];
            txtSubject.Text = Helper.GetString("SUBJECT", row);
            SetupBody(Helper.GetString("BODY", row));
            SetupAttachments();
            SetupSendTo(row);
            upPreview.Update();
        }
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

    protected void lb_Click(object sender, CommandEventArgs e)
    {
        if (!Helper.HasRows(dtAttachments))
            return;

        DataRow row = dtAttachments.Select("COMMUNICATION_EVENT_ID = " + e.CommandArgument)[0];
        string filename = CreateAttachmentDocument(row).Replace(".pdf", string.Empty);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", "window.open('../Pages/PdfViewer.aspx?filename=" + filename + "','_blank');", true);
        //Response.Write("<script type='text/javascript'>window.open('~/Pages/PdfViewer.aspx?filename=" + filename + "','_blank');</script>");
    }

    private static string CreateAttachmentDocument(DataRow row)
    {
        string templatesDirectory = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
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

    private void SetupBody(string s)
    {
        Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
        if (tagRegex.IsMatch(s))
        {
            txtBody.Style["display"] = "none";
            divBody.Style["display"] = "block";
            divBody.InnerHtml = s;
        }
        else
        {
            txtBody.Style["display"] = "block";
            divBody.Style["display"] = "none";
            txtBody.Text = s;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlPreview.Visible = false;
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

        DataRow[] rows = dtEmails.Select("COMMUNICATION_EVENT_ID = " + COMMUNICATION_EVENT_ID);
        if (rows.Length == 0) return;
        DataRow row = rows[0];
        Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
        bool isHTML = tagRegex.IsMatch(Helper.GetData("BODY", row));
        string templatesDirectory = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
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

        EMailNotification n = new EMailNotification(Helper.GetData("BODY", row), txtSubject.Text, sbTo.ToString());
        string returned = n.ResendNotification(isHTML, attachmentsList);
        if (string.IsNullOrEmpty(returned))
        {
            //MessageBox2.Show("Resend Failed", "ERROR");
        }
        else
        {
            //MessageBox2.Show("Email has been resent.", "Success");
            //lblModal.Text = "Email has been resent.";
            //mpeChangesSaved.Show();
            pnlPreview.Visible = false;
            upPreview.Update();
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        btnPrint.Attributes.Add("onclick", "return getPrint('divEmail');");
    }

    private void SetupSendTo(DataRow row)
    {
        string emailTo = Helper.GetString("EMAIL_TO", row).Trim().ToLower();
        string userId = Helper.GetString("USER_ID", row);
        string regId = Helper.GetString("REG_ID", row);
        //string partyId = Helper.GetString("PARTY_ID", row);
        string emailUser = null;
        string emailReg = null;
        // string emailParty = null;

        #region Get Email from aspnet_Membership
        Guid userGuid;
        if (Guid.TryParse(userId, out userGuid))
        {
            MembershipUser u = Membership.GetUser(userGuid);
            emailUser = u == null ? string.Empty : u.Email.Trim().ToLower();
        }
        #endregion

        #region Get CONTACT_EMAIL_ADDRESS from reg_provider
        int iReg;
        if (int.TryParse(regId, out iReg))
        {
            DataSet dsProv = svc.SelectRegistrationData(iReg, "PROVIDER");
            if (Helper.HasRows(dsProv))
            {
                emailReg = Helper.GetString("CONTACT_EMAIL_ADDRESS", dsProv.Tables[0].Rows[0]);
                emailReg = string.IsNullOrEmpty(emailReg) ? string.Empty : emailReg.Trim().ToLower();
            }
        }
        #endregion
        // DCPDMS-2333
        emailReg = "";

        //#region Get CREDENTIALING_EMAIL from usp_SelectProviderContactEmail
        //int iParty;
        //if(int.TryParse(partyId, out iParty))
        //{
        //    DataSet dsParty = svc.SelectProviderContactEmail(iParty);
        //    if (Helper.HasRows(dsParty))
        //    {
        //        emailParty = Helper.GetString("CREDENTIALING_EMAIL", dsParty.Tables[0].Rows[0]);
        //        emailParty = string.IsNullOrEmpty(emailParty) ? string.Empty : emailParty.Trim().ToLower();
        //    }
        //}
        //#endregion

        List<string> emailList = new List<string>();

        if (!string.IsNullOrEmpty(emailTo))
            emailList.Add(emailTo);

        if (!string.IsNullOrEmpty(emailUser) && !emailList.Contains(emailUser))
            emailList.Add(emailUser);

        if (!string.IsNullOrEmpty(emailReg) && !emailList.Contains(emailReg))
            emailList.Add(emailReg);

        //if (!string.IsNullOrEmpty(emailParty) && !emailList.Contains(emailParty))
        //    emailList.Add(emailParty);

        lboxSendTo.DataSource = emailList;
        lboxSendTo.DataBind();
    }

    public void Setup()
    {
        gvEmail.DataSource = SortDataTable();
        gvEmail.DataBind();
        upEmails.Update();
    }

    protected void SendToRequired(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = lboxSendTo.GetSelectedIndices().Length > 0;
    }

    protected void gvEmail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPageIndex = e.NewPageIndex;
        gvEmail.DataSource = SortDataTable();
        gvEmail.PageIndex = e.NewPageIndex;
        gvEmail.DataBind();
    }
}