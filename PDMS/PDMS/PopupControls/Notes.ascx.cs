using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_Notes : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public enum ControlMode
    {
        None, Entry, Grid
    }
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
    public delegate void RefreshEventHandler();
    public event RefreshEventHandler RefreshEvent;

    private string GridViewSortDirection
    {
        get { return ViewState["GridNotesSortDirection"] == null ? "ASC" : ViewState["GridNotesSortDirection"].ToString(); }
        set { ViewState["GridNotesSortDirection"] = value; }
    }

    private string GridViewSortExpression
    {
        get { return ViewState["GridNotesSortExpression"] == null ? "NOTE_DATE_TIME" : ViewState["GridNotesSortExpression"].ToString(); }
        set { ViewState["GridNotesSortExpression"] = value; }
    }


    public int RegistrationId
    {
        get
        {
            if (ViewState["RegistrationId"] == null) ViewState["RegistrationId"] = 0;
            return (int)ViewState["RegistrationId"];
        }
        set { ViewState["RegistrationId"] = value; }
    }

    public int RegPageTypeId
    {
        get
        {
            if (ViewState["RegPageTypeId"] == null) ViewState["RegPageTypeId"] = 0;
            return (int)ViewState["RegPageTypeId"];
        }
        set { ViewState["RegPageTypeId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadProviderNoteTypes();
        }
        cpeNotes.Collapsed = true;
        cpeNotes.ClientState = "true";
        
    }

    private void LoadProviderNoteTypes()
    {

        if (ddlNoteType.Items.Count > 0)
        {
            return;                             // Already loaded don't load again
        }
        ddlNoteType.ClearSelection();
        ddlNoteType.Items.Clear();
        Helper.LoadDropDown(ddlNoteType, ApplicationCache.ProviderNoteTypes(), "PROVIDER_NOTE_TYPE", "PROVIDER_NOTE_TYPE_ID", true);
    }

    private DataSet GetProviderNotesTable(DataSet ds)
    {
        if (ds.Tables.Count > 0)
        {
            DataColumn dc1 = new DataColumn("TASK_NAME", typeof(System.String));
            DataColumn dc2 = new DataColumn("REG_PAGE_NAME", typeof(System.String));
            DataColumn dc3 = new DataColumn("DISPOSITION", typeof(System.String));
            dc1.DefaultValue = "N/A";
            dc2.DefaultValue = "N/A";
            dc3.DefaultValue = "N/A";
            ds.Tables[0].Columns.Add(dc1);
            ds.Tables[0].Columns.Add(dc2);
            ds.Tables[0].Columns.Add(dc3);
        }

        return ds;
    }

    private DataTable GetData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        try
        {
            DataSet ds = null;
            //if (RegistrationId > 0) ds = GetProviderNotesTable(psc.SelectProviderNotes(PartyId));
            //else 
            if (RegistrationId > 0) ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER_NOTEcustom");
            if (Helper.HasRows(ds)) return ds.Tables[0];
            else return null;
        }
        catch (Exception ex) { throw ex; }
    }

    protected void gvNotes_RowCreated(object sender, GridViewRowEventArgs e)
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

    protected DataView SortDataTable()
    {
        try
        {
            DataView dv;
            DataTable dtNotes = GetData();
            gvNotes.PageIndex = this.CurrentPageIndex;
            if (dtNotes != null)
            {
                dv = dtNotes.DefaultView;
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

    protected void gvNotes_Sorting(object sender, GridViewSortEventArgs e)
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

        //gvNotes.DataSource = SortDataTable();
        //gvNotes.DataBind();
        LoadNotes(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
    }

    private void LoadNotes()
    {
        gvNotes.DataSource = SortDataTable();
        gvNotes.DataBind();
    }

    public void LoadNotes(int regId, int regPageTypeId)
    {
        RegistrationId = regId;
        RegPageTypeId = regPageTypeId;
        LoadNotes();
    }

    public void LoadData()
    {
        LoadProviderNoteTypes();
        ddlNoteType.SelectedIndex = 0;
        txtDesc.Text = string.Empty;
        lblLastActivity.Text = string.Empty;
        // This is done if there are multiple Note Entry screens on the form validations need to be separate
        vsNoteEntry.ValidationGroup += DateTime.Now.Millisecond.ToString();
        reqDesc.ValidationGroup = vsNoteEntry.ValidationGroup;
        reqType.ValidationGroup = vsNoteEntry.ValidationGroup;
        btnSave.ValidationGroup = vsNoteEntry.ValidationGroup;
        reqDesc.Enabled = reqType.Enabled = true;
    }

    protected void gvNotes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        //AjaxControlToolkit.Accordion acc = (AjaxControlToolkit.Accordion)e.Row.FindControl("accNotes");
        //if (acc != null)
        //{
        //    Label lbl = (Label)Helper.FindTheControl(acc, "lblNotePreview");
        //    if (lbl != null)
        //    {
        //        int maxLength = 100;
        //        string txt = DataBinder.Eval(e.Row.DataItem, "NOTE_TEXT").ToString();
        //        if (txt.Length > maxLength) txt = txt.Substring(0, maxLength) + "...";
        //        else
        //        {
        //            lnkAccPreview.Visible = false;
        //            acc.Panes[1].Visible = false;
        //        }
        //        lbl.Text = txt;
        //    }
        //    lbl = (Label)Helper.FindTheControl(acc, "lblNoteFull");
        //    if (lbl != null) lbl.Text = DataBinder.Eval(e.Row.DataItem, "NOTE_TEXT").ToString();
        //}        
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Page.Validate("valNoteEntry");

        if (!Page.IsValid)
        {
            if (RefreshEvent != null) RefreshEvent();
            return;
        }
        btnSave.Enabled = false;
        reqDesc.Enabled = reqType.Enabled = false;
        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            if (hdnSelectedNoteType.Value != "")
            {
                if (this.WorkflowPage.RegistrationId > 0 && Convert.ToInt32(hdnSelectedNoteType.Value) > 0)
                {

                    int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                    if (RegPageTypeId == 0)
                        RegPageTypeId = this.WorkflowPage.RegistrationStep;
                    if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
                            Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
                            Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, Convert.ToInt32(hdnSelectedNoteType.Value), txtDesc.Text, DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    else
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, Convert.ToInt32(hdnSelectedNoteType.Value), txtDesc.Text, DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    LoadNotes(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);

                    // hdnSelectedNoteType.Value = "0";
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox2.Show(ex.Message, "Error");
        }
        btnSave.Enabled = true;
        
        if (RefreshEvent != null) RefreshEvent();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       // reqDesc.Enabled = reqType.Enabled = false;
        if (RefreshEvent != null) RefreshEvent();
    }

    protected void gvNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPageIndex = e.NewPageIndex;
        RefreshData();
        if (RefreshEvent != null) RefreshEvent();
    }
    public void RefreshData()
    {
        if (this.WorkflowPage.RegistrationId > 0) LoadNotes(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
        gvNotes.PageIndex = this.CurrentPageIndex;
        gvNotes.DataBind();
        return;
    }
}