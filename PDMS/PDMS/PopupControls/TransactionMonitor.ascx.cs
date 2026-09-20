

using MAXIMUS.Core.Libraries;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;
using Telerik.Web.UI.Skins;

public partial class UserControls_TransactionMonitor : System.Web.UI.UserControl
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
            UpdateCountVariables();
            gvTransactions.Visible = false;
        }
        Page.Title = "Transaction Search";
    }

    protected void btnSearch1_Click(object sender, EventArgs e)
    {
        // Clear these out prior to doing the Search
        this.lblMessages.Text = string.Empty;
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        gvTransactions.Visible = true;
        Search(true);
        lnkRowCount.Visible = lnkExcel.Visible = gvTransactions.Items.Count > 0;
    }

    protected void btnClear1_Click(object sender, EventArgs e)
    {
        foreach (GridColumn column in gvTransactions.MasterTableView.Columns)
        {

            column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;

            column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
            column.AndCurrentFilterValue = string.Empty;
        }
        gvTransactions.MasterTableView.FilterExpression = string.Empty;
        gvTransactions.MasterTableView.Rebind();

        this.lblMessages.Text = string.Empty;
        txtFromDate.Text = txtToDate.Text = txtRegId.Text = txtRegId.Text = txtMedicaidId.Text = string.Empty;
        hdnRowCount.Value = "0";
        gvTransactions.DataSource = null;
        gvTransactions.DataBind();
        lblResultHeader.Text = string.Empty;
        pnlResultHeader.Visible = false;
        lnkRowCount.Visible = lnkExcel.Visible = false;
        chkHistoricNotes.Checked = false;
        chkNWFFailures.Checked = false;
    }

    protected void btnModalOk_Click(object sender, EventArgs e)
    {
        string assignedStatus = this.assignedStatusDropdownId.SelectedValue;
        if (!string.IsNullOrWhiteSpace(this.ModalHeader.Text) && this.ModalHeader.Text.Equals("Add Notes"))
        {
            if (this.notesTxtBoxID.Text == null || this.notesTxtBoxID.Text.Length == 0)
            {
                return;
            }
            else
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                string currentUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
                psc.InsertTransactionMonitorNote(Convert.ToInt32(txtRegId.Text), 15, string.Concat(DateTime.Now.ToString(), "", this.notesTxtBoxID.Text), DateTime.Now, currentUser);
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(txtCurrentTransactionID.Text))
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                string currentUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
                psc.UpdateTransactionAssignedStatus(Convert.ToInt32(txtCurrentTransactionID.Text), assignedStatus, currentUser);
            }

        }

        this.notesTxtBoxID.Text = "";
        this.assignedStatusDropdownId.SelectedValue = "Unassigned";
        this.txtRegId.Text = "";
        gvTransactions.Rebind();
        UpdateCountVariables();
    }


    protected void lnkAddNewNote_Click(object sender, EventArgs e)
    {
        this.actionModalController.Show();
        this.ModalHeader.Text = "Add Notes";
        this.notesTxtBoxID.Visible = true;
        this.assignedStatusLableID.Visible = false;
        this.assignedStatusDropdownId.Visible = false;
    }

    protected void lnkUpdateAssignedStatus_Click(object sender, EventArgs e)
    {
        this.actionModalController.Show();
        this.ModalHeader.Text = "Update Assigned Status";
        this.assignedStatusDropdownId.SelectedValue = txtCurrenntAssignedStatus.Text;
        this.notesTxtBoxID.Visible = false;
        this.assignedStatusLableID.Visible = true;
        this.assignedStatusDropdownId.Visible = true;
    }

    protected void lnkResend_Click(object sender, EventArgs e)
    {

    }

    protected void gvTransactions_Sorting(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        RefreshData();
    }

    protected void gvTransactions_PageIndexChanging(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        // ViewState["GridData"] = this.gvTransactions.CurrentPageIndex + 1;
        gvTransactions.CurrentPageIndex = e.NewPageIndex;
        ViewState["GridData"] = e.NewPageIndex;
    }

    protected void gvTransactions_PageSizeChanged(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
    {
        RefreshData(e.NewPageSize);
    }


    protected void gvTransactions_RowCommand(object sender, GridCommandEventArgs e)
    {
        int index;
        bool indexflag = false;

        indexflag = int.TryParse(e.CommandArgument.ToString(), out index);
        GridDataItem item = e.Item as GridDataItem;
        if (indexflag && item != null)
        {

            txtRegId.Text = (item).GetDataKeyValue("REG_ID").ToString();
            txtCurrentTransactionID.Text = (item).GetDataKeyValue("TRANSACTION_QUEUE_ID").ToString();
            var assignedStatus = (item).GetDataKeyValue("AssignedStatus").ToString();
            if (assignedStatus != null)
            {

                txtCurrenntAssignedStatus.Text = assignedStatus.ToString();
                this.assignedStatusDropdownId.SelectedValue = assignedStatus.ToString();
            }
            else
            {
                txtCurrenntAssignedStatus.Text = string.Empty;
                this.assignedStatusDropdownId.SelectedValue = assignedStatus.ToString();
            }
            if (e.CommandName == "ResendTransaction" || e.CommandName == "RouteToRegScreen" || e.CommandName == "RTRTransaction")
            {
                string currentUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
                DateTime now = DateTime.Now;
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                int tqId = (int)(item).GetDataKeyValue("TRANSACTION_QUEUE_ID");
                int processId = (int)(item).GetDataKeyValue("Process_ID");
                string action = (string)item.GetDataKeyValue("Action_Name");

                switch (e.CommandName)
                {
                    case "ResendTransaction":
                        psc.WF_TakeAction(processId, action, string.Empty);
                        RefreshData();
                        break;
                    case "RouteToRegScreen":
                        Response.Redirect("~/process/Registration.aspx?RegId=" + txtRegId.Text);
                        break;
                    case "RTRTransaction":
                        psc.WF_TakeAction(processId, "Return To Review", string.Empty);
                        RefreshData();
                        break;

                    default:
                        break;
                }
            }
        }

    }

    protected void gvTransactions_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton lnkResend = (LinkButton)item.FindControl("lnkResend");

            lnkResend.Visible = false;

            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin))
            {
                lnkResend.Visible = true;
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {


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
        btnClear1_Click(new object(), new EventArgs());
    }

    protected void gvTransactions_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RefreshData();
    }

    public void RefreshData(int pageSize = 0)
    {
        int totalResultCount = 0;
        int size = pageSize == 0 ? gvTransactions.PageSize : pageSize;
        DataTable dt = GetData(out totalResultCount, size);
        bool historicNotes = chkHistoricNotes.Checked;
        foreach (DataRow item in dt.Rows)
        {
            if (!historicNotes)
            {
                item["HistoricNotes"] = string.Empty;
            }
            else
            {
                if (item["HistoricNotes"] != null && !string.IsNullOrEmpty(item["HistoricNotes"].ToString()))
                {
                    string[] historyData = item["HistoricNotes"].ToString().Split(',');
                    StringBuilder sbHistoryData = new StringBuilder();
                    for (int i = historyData.Length - 1; i >= 0; i--)
                    {
                        sbHistoryData.Append(historyData[i] + "<br>");
                    }

                    item["HistoricNotes"] = sbHistoryData.ToString();
                }

            }
        }
        hdnRowCount.Value = totalResultCount.ToString();
        gvTransactions.DataSource = dt;
        gvTransactions.VirtualItemCount = totalResultCount;
    }

    public void UpdateCountVariables()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
       ds = psc.GetTransactionsMonitorCounts();
        if (Helper.HasRows(ds))
        {
            int TMCount = GetTMTotalCount();
            if (ds.Tables[0].Rows.Count >= 1)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    lblcurrentTMTotalCount.Text = TMCount.ToString();//item["CurentMonthCounts"].ToString();
                    lblPreviousWeekTotalCount.Text = item["PreviousWeekCounts"].ToString();
                    lblPreviousMonthTotalCount.Text = item["PreviousMonthCounts"].ToString();
                    //lblDODDCount.Text = item["DODDCounts"].ToString();
                    //lblRunBookCount.Text = item["RunBookCounts"].ToString();
                    //lblJiraCount.Text = item["JiraTicketCounts"].ToString();
                    //lblODMtoRTPCount.Text = item["ODMToRTPCounts"].ToString();
                    //lblTriageCount.Text = item["TriageCounts"].ToString();
                    //lblUnassignedCount.Text = item["UnassignedCounts"].ToString();
                    //lblODACount.Text = item["ODACounts"].ToString();
                    //lblODMOtherCount.Text = item["ODMOtherCounts"].ToString();
                    //lblOtherCount.Text = item["OtherCounts"].ToString();

                }
            }
        }

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


    }

    private void Search(bool clearIds)
    {
        if (clearIds)
        {
            lblResultHeader.Text = string.Empty;
            pnlResultHeader.Visible = false;
        }
        ViewState["GridData"] = 0;
        gvTransactions.Rebind();
    }

    private DataTable GetData(out int totalResultCount, int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        string sortColumn = "ProcessedDate";
        bool isASC = 0 == SortDirection.Ascending;
        // string sortColWithDirection = this.gvTransactions.GridViewSortDirection == SortDirection.Descending ? gvTransactions.GridViewSortColumn + " DESC" : gvTransactions.GridViewSortColumn;
        totalResultCount = 0;
        int pagenumber = ViewState["GridData"] != null ? int.Parse(ViewState["GridData"].ToString()) : 1;
        ds = psc.SearchTransactionsMonitor(txtRegId.Text, txtMedicaidId.Text, txtFromDate.Text, txtToDate.Text,
            sortColumn, pageSize, pagenumber, isASC, chkNWFFailures.Checked, out totalResultCount);
        //TODO
        if (Helper.HasRows(ds))
        {

            if (ds.Tables[1].Rows.Count >= 1)
            {
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    totalResultCount = (int)item["Count"];
                }
            }
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    private int GetTMTotalCount()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        string sortColumn = "ProcessedDate";
        bool isASC = 0 == SortDirection.Ascending;
        int totalResultCount = 0;
        int pagenumber = 0;
        int pageSize = 10;
        ds = psc.SearchTransactionsMonitor(txtRegId.Text, txtMedicaidId.Text, txtFromDate.Text, txtToDate.Text,
            sortColumn, pageSize, pagenumber, isASC, chkNWFFailures.Checked, out totalResultCount);
        if (Helper.HasRows(ds))
        {
            if (ds.Tables[1].Rows.Count >= 1)
            {
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    totalResultCount = (int)item["Count"];
                    break;
                }
            }
        }
        return totalResultCount;
    }


    #endregion
    protected void btnTransactionSend_Click(object sender, EventArgs e)
    {
        List<Int32> resendRegID = new List<Int32>();
        foreach (GridItem item in gvTransactions.MasterTableView.Items)
        {
            GridDataItem dataitem = (GridDataItem)item;
       
            CheckBox checkBox = item.FindControl("chkAssign") as CheckBox;

            if (checkBox.Checked)
            {
                int tqId = Convert.ToInt32(dataitem.GetDataKeyValue("TRANSACTION_QUEUE_ID"));
                int processId = Convert.ToInt32(dataitem.GetDataKeyValue("Process_ID"));
                string action = dataitem.GetDataKeyValue("Action_Name").ToString();
                int regId = Convert.ToInt32(dataitem.GetDataKeyValue("REG_ID"));
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.WF_TakeAction(processId, action, string.Empty);
             }
        }

        gvTransactions.Rebind();

        //if (resendRegID != null && resendRegID.Count > 0)
        //{

        //    resendRegID.RemoveAll(r => r == 0);

        //    foreach (Int32 regId in resendRegID)
        //    {
        //        int transType = Convert.ToInt32(ddlTransactionType.SelectedValue);
        //        string currentUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        //        List<SqlParameter> parameters = new List<SqlParameter>();

        //        parameters.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, transType, true));
        //        parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
        //        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, currentUser, true));
        //        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
        //        parameters.Add(SqlParms.CreateParameter("IS_TRANSACTION_CREATED_BY_JOB", DbType.Boolean, true, true));

        //        DataAccess.ExecuteStoredProcedure("usp_ResendTransaction", parameters);
        //    }
        //}
    }
}
