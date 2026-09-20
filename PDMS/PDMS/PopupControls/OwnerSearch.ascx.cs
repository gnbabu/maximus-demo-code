using MAXIMUS.Models.Data.PDMS;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_OwnerSearch : System.Web.UI.UserControl
{
    public delegate void RegistrationViewEventHandler(int registrationId);
    public event RegistrationViewEventHandler RegistrationViewEvent;

    #region Properties
    private bool UserCanRapidAdminRegistrations
    {
        get
        {
            return Helper.IsLoggedInUserInAdminRole() || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name);
        }
    }

    private int SelectedRegID
    {
        get
        {
            return ViewState["SelectedRegID"] == null ? 0 : Convert.ToInt32(ViewState["SelectedRegID"]);
        }
        set
        {
            ViewState["SelectedRegID"] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            btnSearch.CssClass = "buttonBoxFocus";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                // If list of IDs has been passed in, make sure both ID lists are non-empty and skip to the search results view.
                if (SearchIds()) Search(false);
                pnlResultHeader.Visible = false;
                if (Request["DashboardLabel"] != null)
                {
                    lblResultHeader.Text = Request["DashboardLabel"].ToString();
                    pnlResultHeader.Visible = true;
                }
                if (Helper.IsModern())
                {
                    divAdditionalSearchCriteria.Visible = true;

                }
                else
                {
                    divAdditionalSearchCriteria.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox2.Show("An error occurred when loading the work queue.<br>Error: " + ex.Message + " " + ex.StackTrace, "Error");
            }
        }
        SetSearchEntries();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // Clear these out prior to doing the Search
        this.lblMessages.Text = string.Empty;
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        Search(true);
        lnkRowCount.Visible = lnkExcel.Visible = lnkPDF.Visible = gvProviders.Rows.Count > 0;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.lblMessages.Text = string.Empty;
        txtGroupName.Text = txtTaxID.Text = txtFirstName.Text = txtLastName.Text = string.Empty;
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        gvProviders.DataSource = null;
        gvProviders.DataBind();
        lblResultHeader.Text = string.Empty;
        pnlResultHeader.Visible = false;
    }

    protected void lnkReview_Click(object sender, CommandEventArgs e)
    {
        if (("ReviewRow").Equals(e.CommandName))
        {
            if (RegistrationViewEvent != null) RegistrationViewEvent(Convert.ToInt32(e.CommandArgument.ToString()));
        }
    }

    protected void gvProviders_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }

    protected void gvProviders_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }

    protected void gvProviders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "ReviewRow" && e.CommandName != "OperatorUpdate")
            return;

        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "ReviewRow")
        {
            int regId = (int)this.gvProviders.DataKeys[index].Values["RegID"];
            if (regId > 0)
            {
                (this.Page as RegistrationProvider).RegistrationId = regId;
                (this.Page as RegistrationProvider).IsReadOnly = false;
                if (RegistrationViewEvent != null) RegistrationViewEvent(regId);
            }
        }
    }

    protected void gvProviders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[6].Text == "9999999999")
            {
                e.Row.Cells[6].Text = string.Empty;
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        gvProviders.SelectRow(-1);

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

    private void View_NewRegistrationInsert(ProviderManagementData data)
    {
        //Open registration with this regID
        (this.Page as RegistrationProvider).RegistrationId = data.RegID;
        (this.Page as RegistrationProvider).IsReadOnly = false;
    }
    #endregion

    #region Public Methods
    public void ResetSelection()
    {
        btnClear_Click(new object(), new EventArgs());
    }

    public void RefreshData()
    {
        int totalResultCount = 0;
        DataTable dt = GetData(out totalResultCount, gvProviders.PageSize);
        hdnRowCount.Value = totalResultCount.ToString();
        gvProviders.DataSource = dt;
        gvProviders.VirtualItemCount = totalResultCount;
        gvProviders.DataBind();
        gvProviders.PageIndexCount = gvProviders.PageCount;
    }
    #endregion

    #region Private Methods
    private DataRow GetDataRow(DataTable dt, string providerID)
    {
        DataRow rtn = null;
        foreach (DataRow row in dt.Rows)
        {
            if (row["ProviderId"].ToString() == providerID)
            {
                rtn = row;
                break;
            }
        }
        return rtn;
    }

    private void SetSearchEntries()
    {
        // Set the search TextBoxes and Dropdowns so "Enter" key will fire the search
        foreach (Control ctl in gbSearch.Controls)
        {
            if (ctl.GetType() == typeof(TextBox))
            {
                TextBox txt = (TextBox)ctl;
                txt.Attributes.Add("onKeyPress", "doClick('" + btnSearch.ClientID + "',event)");
            }
            else if (ctl.GetType() == typeof(eWorld.UI.NumericBox))
            {
                eWorld.UI.NumericBox num = (eWorld.UI.NumericBox)ctl;
                num.Attributes.Add("onKeyPress", "doClick('" + btnSearch.ClientID + "',event)");
            }
            else if (ctl.GetType() == typeof(DropDownList))
            {
                DropDownList drp = (DropDownList)ctl;
                Helper.DisableBackSpace(drp);
                drp.Attributes.Add("onKeyPress", "doClick('" + btnSearch.ClientID + "',event)");
            }
        }
        txtGroupName.Focus();
    }

    private void Search(bool clearIds)
    {
        if (clearIds)
        {
            lblResultHeader.Text = string.Empty;
            pnlResultHeader.Visible = false;
        }
        this.gvProviders.CurrentPageIndex = 0;
        RefreshData();
    }

    private bool SearchIds()
    {
        if (!string.IsNullOrEmpty(SessionVarRetriever.DashBoardRegistrationIds)) return true;
        if (SessionVarRetriever.DashBoardTableId > 0) return true;
        return false;
    }

    private DataTable GetData(out int totalResultCount, int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        string sortColWithDirection = this.gvProviders.GridViewSortDirection == SortDirection.Descending ? gvProviders.GridViewSortColumn + " DESC" : gvProviders.GridViewSortColumn;
        totalResultCount = 0;

        // If list of IDs has been passed in, display in search results list.
        if (SearchIds())
        {
            ds = psc.SearchGroupProviderWithIds(SessionVarRetriever.DashBoardRegistrationIds, SessionVarRetriever.DashBoardTableId,
                SessionVarRetriever.DashBoardStatusID, SessionVarRetriever.DashBoardOrdinal, SessionVarRetriever.DashBoardIsAssigned,
                sortColWithDirection, pageSize, gvProviders.CurrentRowIndex, true, out totalResultCount);
        }
        else
        {
            //int riskLevelID = ddlRiskLevel.SelectedIndex == -1 || string.IsNullOrWhiteSpace(ddlRiskLevel.SelectedValue) ? 0 : Convert.ToInt32(ddlRiskLevel.SelectedValue);
            if (Helper.IsLoggedInUserInAdminRole())
            {
                ds = psc.SearchOwnerProviders(txtGroupName.Text.Trim(), txtFirstName.Text.Trim(), txtLastName.Text.Trim(),
                    txtTaxID.Text.Trim(), Helper.GetUserRole(HttpContext.Current.User.Identity.Name), "", "",
                    sortColWithDirection, pageSize, gvProviders.CurrentRowIndex, true, out totalResultCount);
            }
            else
            {
                ds = psc.SearchOwnerProviders(txtGroupName.Text.Trim(), txtFirstName.Text.Trim(), txtLastName.Text.Trim(),
                    txtTaxID.Text.Trim(), Helper.GetUserRole(HttpContext.Current.User.Identity.Name), "", "",
                sortColWithDirection, pageSize, gvProviders.CurrentRowIndex, true, out totalResultCount);
            }
        }

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    #endregion
}