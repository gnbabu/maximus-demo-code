using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_MQSearch : System.Web.UI.UserControl
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
            LoadQueueNames();
            SetSearchEntries();

        }
        
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        int totalResultCount = 0;

        DataTable dt = GetData(out totalResultCount, 10000);
        gvMQSearch.DataSource = dt;
        gvMQSearch.VirtualItemCount = totalResultCount;
        gvMQSearch.DataBind();
        RadGridExport.DataSource = dt;
        RadGridExport.DataBind();
        // Clear these out prior to doing the Search
        this.lblMessages.Text = string.Empty;
        hdnRowCount.Value = totalResultCount.ToString();
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        //Search(true);
        lnkRowCount.Visible = lnkExcel.Visible = lnkPDF.Visible = gvMQSearch.Rows.Count > 0;

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.lblMessages.Text = string.Empty;
      //  txtFromDate.Text = txtToDate.Text = txtRegId.Text = txtRegId.Text = txtMedicaidId.Text = string.Empty;
        ddlQueuename.SelectedIndex = 0;
        hdnRowCount.Value = "0";
        gvMQSearch.DataSource = null;
        gvMQSearch.DataBind();
        lblResultHeader.Text = string.Empty;
        pnlResultHeader.Visible = false;
    }

   
    protected void gvMQSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       ViewState["GridData"] = this.gvMQSearch.CurrentPageIndex + 1;
        RefreshData();
    }

  
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        gvMQSearch.SelectRow(-1);

    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        int totalResultCount = 0;

        DataTable dt = GetData(out totalResultCount, 10000);
        
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
        DataTable dt = GetData(out totalResultCount, gvMQSearch.PageSize);
        hdnRowCount.Value = totalResultCount.ToString();
      
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
        RefreshData();
    }

    private DataTable GetData(out int totalResultCount, int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        totalResultCount = 0;
        DateTime fromDate;
        DateTime toDate;
        if (string.IsNullOrWhiteSpace(txtFromDate.Text.Trim()) || string.IsNullOrEmpty(txtFromDate.Text.Trim()))
        {
            fromDate = new DateTime();
        }
        else
        {
            fromDate = DateTime.Parse(txtFromDate.Text.Trim());
        }

        if (string.IsNullOrWhiteSpace(txtToDate.Text.Trim()) || string.IsNullOrEmpty(txtToDate.Text.Trim()))
        {
            toDate = DateTime.Now;
        }
        else
        {
            toDate = DateTime.Parse(txtToDate.Text.Trim());
        }
        //ds = psc.SearchMQSearch(out totalResultCount, Convert.ToInt32(ddlQueuename.SelectedValue), fromDate, toDate);
        ds = psc.SearchMQSearchData(out totalResultCount, Convert.ToInt32(ddlQueuename.SelectedValue), fromDate, toDate, txtRegId.Text, txtMedicaidId.Text);

        if (Helper.HasRows(ds))
        {

            return ds.Tables[0];
        }
        else return new DataTable();

    }
    private void LoadQueueNames()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        var ds = psc.LoadQueueNames();
        var queuedata = ds.Tables[0].Rows;
        ddlQueuename.Items.Clear();
        ddlQueuename.Items.Add(new ListItem("--select--", "0"));
        foreach (DataRow item in queuedata)
        {
            ddlQueuename.Items.Add(new ListItem(item["MQ_QUEUES_NAME"].ToString(), item["MQ_QUEUES_ID"].ToString()));
        }       
    }
    #endregion

    protected void gvMQSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "showmessage")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow grow = gvMQSearch.Rows[rowIndex];
            Literal ltMessage = grow.FindControl("ltMessage") as Literal;
            LinkButton lbtnMessage = grow.FindControl("lbtnMessage") as LinkButton;
            if (ltMessage != null)
            {
                ltMessage.Visible = true;
                lbtnMessage.Visible = false;
            }
        }
        
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //green - &#128994;
        //yellow - &#128993;
        //red - &#128308;
        string LastRan = string.Empty;
        DataSet ds = svc.GetMQLastExecutionDT(CON.MQAppID.MQAppIDKey);
        if (ds.Tables.Count > 0)
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                LastRan = ds.Tables[0].Rows[0]["LastRan"].ToString();
            }

        }
        DateTime mqDateTime;
        DateTime start = DateTime.Now;
        
        if (!DateTime.TryParse(LastRan, out mqDateTime))
        { }

        try
        {
            double totalMins = (start - mqDateTime).TotalMinutes;
            int totalMinsINT = Convert.ToInt32(totalMins);
            if (totalMins >= 5)
            {
                checkHealthIconID.Text = "&#128308;";
                checkHealthID.Text = "Stopped";
                checkHealthDTID.Text = totalMinsINT.ToString() + " minutes ago";
            }else if ((start - mqDateTime).TotalMinutes >= 2)
            {
                checkHealthIconID.Text = "&#128993;";
                checkHealthID.Text = "UnHealthy";
                checkHealthDTID.Text = totalMinsINT.ToString() + " minutes ago";
            }
            else
            {
                checkHealthIconID.Text = "&#128994;";
                checkHealthID.Text = "Healthy";
                checkHealthDTID.Text = totalMinsINT.ToString() + " minutes ago";
            }
        }

        catch { }
    }
}