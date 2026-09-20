using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_MySiteVisitQueue : RegistrationProvider
{
    private const int npiColumn = 4;
    private const int hiddenColumn = 8;
    private int WorkflowIDSelected
    {
        get
        {
            if (ViewState["WorkflowIDSelected"] == null) ViewState["WorkflowIDSelected"] = 0;
            return (int)ViewState["WorkflowIDSelected"];
        }
        set { ViewState["WorkflowIDSelected"] = value; }
    }
    private class MyQueueItem

    {

        public string WORKFLOW_NAME { get; set; }
        public int WORKFLOW_ID { get; set; }
        public int ASSIGNED { get; set; }
        public int UNASSIGNED { get; set; }

        public MyQueueItem() { }

        public MyQueueItem(string workFlowName, int workflowID, int assigned, int unassigned)
        {
            WORKFLOW_NAME = workFlowName;
            WORKFLOW_ID = workflowID;
            ASSIGNED = assigned;
            UNASSIGNED = unassigned;
        }
    }

    public class Data
    {
        public string ColumnName = "";
        public int Value = 0;
        public string Category = "";
        public Data(string category, int value, string columnName)
        {
            Category = category;
            ColumnName = columnName;
            Value = value;
        }
    }


    private class MySiteItem
    {
        public string WORKFLOW_NAME { get; set; }
        public int WORKFLOW_ID { get; set; }
        public int ASSIGNED { get; set; }
        public string Attempt { get; set; }

        public MySiteItem() { }

        public MySiteItem(int workflowID, string attempt, int assigned)
        {
            //WORKFLOW_NAME = workFlowName;
            WORKFLOW_ID = workflowID;
            ASSIGNED = assigned;
            Attempt = attempt;
        }
    }
    List<MySiteItem> MySiteItems;


    private void SetList()
    {
        if (!Helper.IsLoggedInUserInAdminRole()) return;
        if (SessionVarRetriever.UserIdSelected.Length > 0)
        {
            ListItem itemToSelect = ddlUserSelect.Items.FindByValue(SessionVarRetriever.UserIdSelected);
            if (itemToSelect != null)
            {
                ddlUserSelect.SelectedValue = itemToSelect.Value;
                ddlUserSelect_SelectedIndexChanged(new object(), new EventArgs());
            }
        }
        else
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.WF_SelectActiveOwnerStepsProvider(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), false, SessionVarRetriever.MyQueueSelectedRoleName);
            if (!Helper.HasRows(ds)) return;
            ddlUserSelect.SelectedValue = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            ddlUserSelect_SelectedIndexChanged(new object(), new EventArgs());
        }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
        }
        else
        {
            Page.Theme = "Default";
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        if (Helper.IsModern())
        {
            quickJump.Visible = !(true && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.SiteVisitOperator) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.SiteVisitAdministrator));
            tdMyDashGraph.Visible = true && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.SiteVisitOperator) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.SiteVisitAdministrator);

        }
        else
        {
            quickJump.Visible = true;
            tdMyDashGraph.Visible = false;
            //tdMyDashGraphTable.Visible = false; 
        }

        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "DIDDOperator"))
        {
            Table1.Visible = false;
            DIDDSearch.Visible = true;
        }
        else
        {
            Table1.Visible = true;
            DIDDSearch.Visible = false;
        }

        if (!Page.IsPostBack)
        {

            SessionVarRetriever.ClearRegistrationSessionVars();
            pnlUserSelect.Visible = (Helper.IsLoggedInUserInAdminRole());
            if (pnlUserSelect.Visible)
            {
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                DataSet ds = svc.SelectUsersInRoles(MAXIMUS.Core.Libraries.Constants.ProviderServiceRoles);
                Helper.LoadList(ddlUserSelect, ds.Tables[0], "UserName", "UserId", true);
                pnlMyQueue.Visible = false;
                pnlRoleSelect.Visible = false;
                SetList();
            }
            else
            {
                InitializeUserRoleSelect();
                pnlMyQueue.Visible = true;
                SessionVarRetriever.UserIdSelected = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            }
            Refresh();
        }
        loadMyDashboard();
    }

    protected void ddlUserSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlUserSelect.SelectedValue))
        {
            SessionVarRetriever.UserIdSelected = ddlUserSelect.SelectedValue;
            InitializeUserRoleSelect();
            Refresh();
            pnlMyQueue.Visible = true;
        }
        else pnlMyQueue.Visible = false;
    }

    private void loadMyDashboard()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        int attempt = 0;
        string assignedto = "";
        int PreEnrollmentinitialAssigned = 0;

        int PostEnrollmentinitialAssigned = 0;

        int PreEnrollmentfollowupAssigned = 0;

        int PostEnrollmentfollowupAssigned = 0;
        DataSet ds = psc.GetUnassignedSiteVisits(attempt, assignedto, 0);
        int countReferredToState = psc.GetSiteVisitsReferredToState();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Guid guid = new Guid();
            if (dr["OWNER_ID"].ToString() != null && dr["OWNER_ID"].ToString() != "" && dr["OWNER_ID"].ToString() != "&nbsp;")
                guid = new Guid(dr["OWNER_ID"].ToString());
            MembershipUser user = Membership.GetUser((object)guid);
            bool IsSiteVisitOperator = false;
            if (dr["OWNER_ID"].ToString() != null && dr["OWNER_ID"].ToString() != "" && dr["OWNER_ID"].ToString() != "&nbsp;" && user != null)
            {
                if (Helper.IsUserInRole(user.UserName, CON.UserRoleType.SiteVisitOperator) && user.ProviderUserKey.ToString() == SessionVarRetriever.UserIdSelected)
                    IsSiteVisitOperator = true;
            }
            if (dr["SITE_VISIT_TYPE_NAME"].ToString() == CON.SiteVisitType.PreEnrollment && dr["ATTEMPT"].ToString() == CON.SiteVisitAttempt.Initial && IsSiteVisitOperator)
            {
                PreEnrollmentinitialAssigned++;
            }

            else if (dr["SITE_VISIT_TYPE_NAME"].ToString() == CON.SiteVisitType.PostEnrollment && dr["ATTEMPT"].ToString() == CON.SiteVisitAttempt.Initial && IsSiteVisitOperator)
            {
                PostEnrollmentinitialAssigned++;
            }

            else if (dr["SITE_VISIT_TYPE_NAME"].ToString() == CON.SiteVisitType.PreEnrollment && dr["ATTEMPT"].ToString() == CON.SiteVisitAttempt.Follow_up && IsSiteVisitOperator)
            {
                PreEnrollmentfollowupAssigned++;
            }

            else if (dr["SITE_VISIT_TYPE_NAME"].ToString() == CON.SiteVisitType.PostEnrollment && dr["ATTEMPT"].ToString() == CON.SiteVisitAttempt.Follow_up && IsSiteVisitOperator)
            {
                PostEnrollmentfollowupAssigned++;
            }

        }
        DataTable dt = new DataTable();
        //dt.Columns.Add("Site Visit Type", typeof(string));
        dt.Columns.Add("Attempt", typeof(string));
        dt.Columns.Add("Assigned", typeof(string));
        MySiteItems = new List<MySiteItem>();
        if (dt != null)
        {

            DataRow dr = dt.NewRow();
            //dr["Site Visit Type"] = "Pre-Enrollment";
            dr["Attempt"] = "Initial";
            dr["Assigned"] = PreEnrollmentinitialAssigned.ToString();
            MySiteItems.Add(new MySiteItem(0, dr["Attempt"].ToString(), Convert.ToInt32(dr["Assigned"])));

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            //dr["Site Visit Type"] = "Pre-Enrollment";
            dr["Attempt"] = "Follow - Up";
            dr["Assigned"] = PreEnrollmentfollowupAssigned.ToString();
            // MySiteItems.Add(new MySiteItem(dr["Site Visit Type"].ToString(), 0, dr["Attempt"].ToString(), Convert.ToInt32(dr["Assigned"]) + 1));
            MySiteItems.Add(new MySiteItem(0, dr["Attempt"].ToString(), Convert.ToInt32(dr["Assigned"])));

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            // dr["Site Visit Type"] = "Post-Enrollment";
            dr["Attempt"] = "Initial";
            dr["Assigned"] = PostEnrollmentinitialAssigned.ToString();
            MySiteItems.Add(new MySiteItem(0, dr["Attempt"].ToString(), Convert.ToInt32(dr["Assigned"])));
            // MySiteItems.Add(new MySiteItem(dr["Site Visit Type"].ToString(), 0, dr["Attempt"].ToString(), Convert.ToInt32(dr["Assigned"]) + 1));

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            //dr["Site Visit Type"] = "Post-Enrollment";
            dr["Attempt"] = "Follow - Up";
            dr["Assigned"] = PostEnrollmentfollowupAssigned.ToString();
            //MySiteItems.Add(new MySiteItem(dr["Site Visit Type"].ToString(), 0, dr["Attempt"].ToString(), Convert.ToInt32(dr["Assigned"]) + 1));
            MySiteItems.Add(new MySiteItem(0, dr["Attempt"].ToString(), Convert.ToInt32(dr["Assigned"])));
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            // dr["Site Visit Type"] = "Referred to State";
            dr["Attempt"] = "N/A";
            dr["Assigned"] = countReferredToState.ToString();
            //MySiteItems.Add(new MySiteItem(dr["Site Visit Type"].ToString(), 0, dr["Attempt"].ToString(), Convert.ToInt32(dr["Assigned"]) + 1));
            MySiteItems.Add(new MySiteItem(0, dr["Attempt"].ToString(), Convert.ToInt32(dr["Assigned"])));
            //dr["UnAssigned"] = "0";
            dt.Rows.Add(dr);

            dt.AcceptChanges();
            SessionVarRetriever.MySiteItemsList = MySiteItems;
            gvMyDashboard.DataSource = dt;
            gvMyDashboard.DataBind();
        }

    }

    private void InitializeUserRoleSelect()
    {
        ddlRoleSelect.DataTextField = "Value";
        ddlRoleSelect.DataValueField = "Key";
        string userName = HttpContext.Current.User.Identity.Name;
        if (ddlUserSelect.Visible && ddlUserSelect.SelectedItem != null && ddlUserSelect.SelectedItem.Text.Trim().Length > 0)
        {
            userName = ddlUserSelect.SelectedItem.Text;
        }
        ddlRoleSelect.DataSource = Helper.GetUserRolesByUser(userName);
        ddlRoleSelect.DataBind();
        if (ddlRoleSelect.Items.Count < 2)
        {
            pnlRoleSelect.Visible = false;
            SessionVarRetriever.MyQueueSelectedRoleName = ddlRoleSelect.Items.Count == 0 ? "" : ddlRoleSelect.Items[0].Text;
            SessionVarRetriever.MyQueueSelectedRoleValue = ddlRoleSelect.Items.Count == 0 ? "" : ddlRoleSelect.Items[0].Value;
            if (!SessionVarRetriever.MyQueueSelectedRoleName.Equals(CON.SiteVisitOperatorRole))
            {
                Response.Redirect("~/Process/MyQueue.aspx");
            }
        }
        else
        {
            pnlRoleSelect.Visible = true;
            if ((string.IsNullOrEmpty(SessionVarRetriever.MyQueueSelectedRoleValue) &&
                 string.IsNullOrEmpty(SessionVarRetriever.MyQueueSelectedRoleName)) ||
                 ddlRoleSelect.Items.FindByValue(SessionVarRetriever.MyQueueSelectedRoleValue) == null)
            {
                if (ddlRoleSelect.SelectedIndex == 0)
                {
                    ddlRoleSelect_SelectedIndexChanged(new object(), null);
                }
                else
                {
                    ddlRoleSelect.SelectedIndex = 0;
                }
            }
            else
            {
                ListItem roleToSelect = ddlRoleSelect.Items.FindByText(SessionVarRetriever.MyQueueSelectedRoleName);
                if (roleToSelect == null)
                {
                    ddlRoleSelect.SelectedIndex = 0;
                }
                else
                {
                    ddlRoleSelect.SelectedValue = SessionVarRetriever.MyQueueSelectedRoleValue;
                    if (!SessionVarRetriever.MyQueueSelectedRoleName.Equals(CON.SiteVisitOperatorRole))
                    {
                        Response.Redirect("~/Process/MyQueue.aspx");
                    }
                }
                ddlRoleSelect.SelectedValue = SessionVarRetriever.MyQueueSelectedRoleValue;



            }

            SessionVarRetriever.MyQueueSelectedRoleName = ddlRoleSelect.SelectedItem.Text;
            SessionVarRetriever.MyQueueSelectedRoleValue = ddlRoleSelect.SelectedItem.Value;
            //((Menu)Master.FindControl("mnuLeftNav")).DataBind();
        }
    }



    private int CountWorkflows(DataTable dt, int workFlowID)
    {
        if (dt == null) return 0;
        int rtn = 0;
        foreach (DataRow row in dt.Rows)
        {
            if (Helper.GetInt("WORKFLOW_ID", row) == workFlowID) rtn += 1;
        }
        return rtn;
    }



    // Filter the assigned rows on the WORKFLOW_ID value, if 0 get all otherwise only those that equal the WORKFLOW_ID
    private DataTable FilterAssigned(DataTable dtOriginal)
    {
        DataTable rtn = dtOriginal.Clone();
        foreach (DataRow row in dtOriginal.Rows)
        {
            if (WorkflowIDSelected == 0 || Helper.GetInt("WORKFLOW_ID", row) == WorkflowIDSelected)
            {
                DataRow newRow = rtn.NewRow();
                newRow.ItemArray = row.ItemArray;
                rtn.Rows.Add(newRow);
            }
        }
        return rtn;
    }

    private void Refresh()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        // Only include my steps
        DataSet ds = svc.WF_SelectActiveOwnerStepsProvider(SessionVarRetriever.UserIdSelected, false, SessionVarRetriever.MyQueueSelectedRoleName);
        SessionVarRetriever.ProcessingList1 = ds.Tables[0];
        RefreshAssigned();

    }

    private void RefreshAssigned()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetPendingSiteVisits(SessionVarRetriever.UserIdSelected);
        gvAssignedDetail.DataSource = ds;
        gvAssignedDetail.DataBind();
    }

    protected void gvAssignedDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DoWork")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvAssignedDetail.Rows[index];
            int regID = string.IsNullOrEmpty(this.gvAssignedDetail.DataKeys[index].Values["REG_ID"].ToString()) ? 0 : Convert.ToInt32(this.gvAssignedDetail.DataKeys[index].Values["REG_ID"].ToString());
            HiddenField hdnPage = (HiddenField)row.FindControl("hdnPage");

            (this.Page as RegistrationProvider).RegistrationId = regID;
            (this.Page as RegistrationProvider).IsReadOnly = false;







            if (hdnPage.Value.Contains("Step="))
            {
                string queryString = hdnPage.Value.Split('?')[1];
                var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
                (this.Page as RegistrationProvider).RegistrationStep = int.Parse(queryDictionary["Step"]);
            }


        }
    }

    protected void lnkAssigned_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        if (lnk != null)
        {
            WorkflowIDSelected = Convert.ToInt32(lnk.CommandArgument);
            RefreshAssigned();
        }
    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valQuickJump";
        this.Page.Validators.Add(val);
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        string ssn = Helper.StripNonNumerics(txtSSN.Text);
        if (string.IsNullOrEmpty(ssn) && string.IsNullOrEmpty(nbNPI.Text) && string.IsNullOrEmpty(nbTaxID.Text))
        {
            AddError("Enter Tax ID, NPI, or SSN");
            return;
        }

        if (string.IsNullOrEmpty(nbNPI.Text) && (!string.IsNullOrEmpty(nbTaxID.Text) || !string.IsNullOrEmpty(ssn)))
        {
            AddError("Enter NPI");
            return;
        }

        if (!string.IsNullOrEmpty(nbNPI.Text) && string.IsNullOrEmpty(nbTaxID.Text) && string.IsNullOrEmpty(ssn))
        {
            AddError("Enter Tax ID or SSN");
            return;
        }

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SearchRegistration(nbTaxID.Text, nbNPI.Text, ssn, string.Empty);
        if (Helper.HasRows(ds))
        {
            int regId = Helper.GetInt("REG_ID", ds.Tables[0].Rows[0]);
            (this.Page as RegistrationProvider).RegistrationId = regId;
            (this.Page as RegistrationProvider).IsReadOnly = true; // TODO: EDV Should it be readonly????
        }
        else
        {
            AddError("No match on selection");
            return;
        }
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        txtSSN.Text = nbNPI.Text = nbTaxID.Text = string.Empty;
    }

    protected void gvAssignedDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        // If the NPI is all 9s then blank it out
        if (e.Row.Cells[npiColumn].Text == "9999999999") e.Row.Cells[npiColumn].Text = string.Empty;
    }
    protected void gvAssignedDetail_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[hiddenColumn].CssClass = "hiddencol";
        }
        else if (e.Row.RowType == DataControlRowType.Header)
        { e.Row.Cells[hiddenColumn].CssClass = "hiddencol"; }
    }
    protected void ddlRoleSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pnlRoleSelect.Visible)
        {
            SessionVarRetriever.MyQueueSelectedRoleName = ddlRoleSelect.SelectedItem.Text;
            SessionVarRetriever.MyQueueSelectedRoleValue = ddlRoleSelect.SelectedItem.Value;
            if (!SessionVarRetriever.MyQueueSelectedRoleName.Equals(CON.SiteVisitOperatorRole))
            {
                Response.Redirect("~/Process/MyQueue.aspx");
            }
            // ((Menu)Master.FindControl("mnuLeftNav")).DataBind();
        }
        else
        {
            SessionVarRetriever.MyQueueSelectedRoleName = ddlRoleSelect.Items.Count == 0 ? "" : ddlRoleSelect.Items[0].Text;
            SessionVarRetriever.MyQueueSelectedRoleValue = ddlRoleSelect.Items.Count == 0 ? "" : ddlRoleSelect.Items[0].Value;
        }

        Refresh();
    }
}