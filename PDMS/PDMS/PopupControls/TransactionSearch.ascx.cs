using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_TransactionSearch : System.Web.UI.UserControl
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
            // no action
            SetSearchEntries();
        }
        Page.Title = "Transaction Search";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // Clear these out prior to doing the Search
        this.lblMessages.Text = string.Empty;
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        Search(true);
        lnkRowCount.Visible = lnkExcel.Visible = lnkPDF.Visible = gvTransactions.Rows.Count > 0;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.lblMessages.Text = string.Empty;
        txtFromDate.Text = txtToDate.Text = txtRegId.Text = txtRegId.Text = txtMedicaidId.Text = string.Empty;
        ddlStatus.SelectedIndex = 0;
        ddlSubscriberFails.SelectedIndex = 0;
        hdnRowCount.Value = "0";
        gvTransactions.DataSource = null;
        gvTransactions.DataBind();
        lblResultHeader.Text = string.Empty;
        pnlResultHeader.Visible = false;
    }

    protected void gvTransactions_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }

    protected void gvTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["GridData"] = this.gvTransactions.CurrentPageIndex + 1;
        RefreshData();
    }


    protected void gvTransactions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ResubmitTransaction" || e.CommandName == "CancelTransaction")
        {
            string currentUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            DateTime now = DateTime.Now;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            int index = Convert.ToInt32(e.CommandArgument);
            int tqId = (int)this.gvTransactions.DataKeys[index].Values["TRANSACTION_QUEUE_ID"];
            switch (e.CommandName)
            {
                case "ResubmitTransaction":
                    psc.ResubmitProcessedTransaction(tqId, now, currentUser, true);
                    break;

                case "CancelTransaction":
                    psc.CancelTransaction(tqId, now, currentUser);
                    break;

                default:
                    break;
            }
        }

        RefreshData();
    }

    protected void gvTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton lnkResubmitTransaction = (LinkButton)e.Row.FindControl("lnkResubmitTransaction");
        LinkButton lnkCancelTransaction = (LinkButton)e.Row.FindControl("lnkCancelTransaction");


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string txnStatus = DataBinder.Eval(e.Row.DataItem, "Status").ToString();

            string txtSubmitDate = DataBinder.Eval(e.Row.DataItem, "SUBMIT_DATE_TIME").ToString();
            string txtProcessDate = DataBinder.Eval(e.Row.DataItem, "PROCESS_DATE_TIME").ToString();

            lnkResubmitTransaction.Visible = false;
            lnkCancelTransaction.Visible = false;

            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin))
            {
                // hide links that do not match current status         
                switch (txnStatus)
                {
                    case "Failed":
                        lnkResubmitTransaction.Visible = false;
                        break;

                    default:

                        lnkResubmitTransaction.Visible = false;
                        break;
                }

                if (string.IsNullOrEmpty(txtSubmitDate) || string.IsNullOrEmpty(txtProcessDate))
                {
                    lnkCancelTransaction.Visible = true;
                }
            }
            else
            {
                //   gvTransactions.Columns[12].Visible = false;
            }
        }
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

    #endregion

    #region Public Methods
    public void ResetSelection()
    {
        btnClear_Click(new object(), new EventArgs());
    }

    public void RefreshData()
    {
        int totalResultCount = 0;
        DataTable dt = GetData(out totalResultCount, gvTransactions.PageSize);

        hdnRowCount.Value = totalResultCount.ToString();
        gvTransactions.DataSource = dt;
        gvTransactions.VirtualItemCount = totalResultCount;
        gvTransactions.DataBind();
        gvTransactions.PageIndexCount = gvTransactions.PageCount;
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
        txtFromDate.Focus();

        int selectedItem = ddlStatus.SelectedIndex;

        ListItemCollection lic = new ListItemCollection
        {
            new ListItem(string.Empty, "%"),
            new ListItem("Completed", "Completed"),
            new ListItem("Failed", "Failed"),
            new ListItem("InProcess", "InProcess"),
            new ListItem("Pending", "Pending")
        };
        ddlStatus.DataSource = lic;
        ddlStatus.DataBind();
        ddlStatus.SelectedIndex = selectedItem;

        int selectedItem2 = ddlSubscriberFails.SelectedIndex;

        ListItemCollection lic2 = new ListItemCollection
        {
            new ListItem(string.Empty, "-1"),
            new ListItem("SI", "SI"),
            new ListItem("MITS", "MITS"),
            new ListItem("FI", "FI"),
            new ListItem("SPBM", "SPBM"),
            new ListItem("EVV", "EVV"),
            new ListItem("PSM", "PSM"),
            new ListItem("PCW", "PCW")
        };

        ddlSubscriberFails.DataSource = lic2;
        ddlSubscriberFails.DataBind();
        ddlSubscriberFails.SelectedIndex = selectedItem2;

    }

    private void Search(bool clearIds)
    {
        if (clearIds)
        {
            lblResultHeader.Text = string.Empty;
            pnlResultHeader.Visible = false;
        }
        ViewState["GridData"] = 0;
        RefreshData();
    }

    private DataTable GetData(out int totalResultCount, int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        string sortColumn = gvTransactions.GridViewSortColumn;
        bool isASC = this.gvTransactions.GridViewSortDirection == SortDirection.Ascending;
        // string sortColWithDirection = this.gvTransactions.GridViewSortDirection == SortDirection.Descending ? gvTransactions.GridViewSortColumn + " DESC" : gvTransactions.GridViewSortColumn;
        totalResultCount = 0;
        int pagenumber = int.Parse(ViewState["GridData"].ToString());
        ds = psc.SearchTransactions(ddlStatus.SelectedValue, txtRegId.Text, txtMedicaidId.Text, txtFromDate.Text, txtToDate.Text,
            sortColumn, pageSize, pagenumber, isASC, ddlSubscriberFails.SelectedValue, out totalResultCount);
        //TODO
        if (Helper.HasRows(ds))
        {
            //totalResultCount = ds.Tables[0].Rows.Count;
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    #endregion
}