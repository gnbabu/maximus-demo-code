using Corp.Core.Libraries.Helper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using CON = MAXIMUS.Core.Libraries.Constants;
using Label = System.Web.UI.WebControls.Label;

public partial class Process_SiteVisitAssignments : System.Web.UI.Page
{
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
        if (!IsPostBack)
        {
            if (Helper.IsLoggedInUserInAdminRole())
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
            ListItemCollection lic = new ListItemCollection();
            lic.Add(new ListItem("", ""));
            lic.Add(new ListItem(CON.SiteVisitAttempt.Initial, CON.SiteVisitAttempt.Initial));
            lic.Add(new ListItem(CON.SiteVisitAttempt.Follow_up, CON.SiteVisitAttempt.Follow_up));
            lic.Add(new ListItem(CON.SiteVisitAttempt.Failed_Follow_up, CON.SiteVisitAttempt.Failed_Follow_up));
            ddlAttempt.DataSource = lic;
            ddlAttempt.DataBind();

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet ds = svc.SelectUsersInRoles(MAXIMUS.Core.Libraries.Constants.SiteVisitOperatorRole);
            Helper.LoadList(ddlSearchAssignToUser, ds.Tables[0], "UserName", "UserId", true);
            if (ddlSearchAssignToUser.Items.Count > 0 && ddlSearchAssignToUser.Items[0].Text == "")
            {
                ddlSearchAssignToUser.Items.Insert(1, new ListItem("Unassigned", "00000000-0000-0000-0000-000000000000"));
            }
            else
            {
                ddlSearchAssignToUser.Items.Insert(0, new ListItem("Unassigned", "00000000-0000-0000-0000-000000000000"));
            }
            Helper.LoadList(ddlSiteVisitOper, ds.Tables[0], "UserName", "UserId", true);

            LoadPendingSiteVisits();
        }
        LoadMyDashboard();
        DisplayAssignment();
        btnSiteVisitNeeded.Visible = btnSiteVisitNotNeeded.Visible = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ComplianceSpecialist);
    }

    private void DisplayAssignment()
    {
        //Site visit Assignment can only be done by site visit administrator as per Security matrix.
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "SiteVisitAdministrator") || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ComplianceSpecialist))
        {
            pnlSiteVisitAssign.Visible = true;
        }
        else
        {
            pnlSiteVisitAssign.Visible = false;
        }

    }

    protected void gvPendingSiteVisits_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkAssign = (CheckBox)e.Row.FindControl("chkAssign");
            if (chkAssign != null)
            {
                chkAssign.Visible = true;
            }

            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ComplianceSpecialist))
            {
                string taskName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "TASK_NAME"));
                string sitevisitNeeded = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "SITE_VISIT_NEEDED"));
                if (taskName == "Check for Site Visit Disposition" && string.IsNullOrEmpty(sitevisitNeeded) ||
                    (taskName == "Assign Site Visit" || taskName == "Conduct Site Visit Step (PCG)"))
                {
                    chkAssign.Enabled = true;
                }
                else
                {
                    chkAssign.Enabled = false;
                }
            }
        }
    }
    protected void gvPendingSiteVisits_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPendingSiteVisits.PageIndex = e.NewPageIndex;
        LoadPendingSiteVisits();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadPendingSiteVisits();
    }

    private void LoadPendingSiteVisits()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        int attempt = 0;
        string assignedto = "";
        int regID = string.IsNullOrEmpty(txtRegID.Text) ? 0 : Convert.ToInt32(txtRegID.Text);

        if (ddlSearchAssignToUser.SelectedIndex != 0)
            assignedto = ddlSearchAssignToUser.SelectedValue;
        if (ddlAttempt.SelectedValue == CON.SiteVisitAttempt.Initial)
            attempt = 1;
        else if (ddlAttempt.SelectedValue == CON.SiteVisitAttempt.Follow_up)
            attempt = CON.SiteVisitRecommendationID.FollowupVisit;
        else if (ddlAttempt.SelectedValue == CON.SiteVisitAttempt.Failed_Follow_up)
            attempt = CON.SiteVisitRecommendationID.FailedFollowup;
        else
            attempt = 0;
        Page.Validate("valSiteVisitAssignment");
        if (!Page.IsValid)
        {
            return;
        }
        DataSet ds = psc.GetUnassignedSiteVisits(attempt, assignedto, regID);
        ds.Tables[0].DefaultView.Sort = "NAME";
        gvPendingSiteVisits.DataSource = ds.Tables[0].DefaultView;
        gvPendingSiteVisits.DataBind();
        lnkSVAExcel.Visible = Helper.HasRows(ds);
    }

    protected void btnSiteVisitNeeded_Click(object sender, EventArgs e)
    {
        GridViewRowCollection gvRowC = gvPendingSiteVisits.Rows;
        bool IscheckboxSelected = false;
        foreach (GridViewRow gvR in gvRowC)
        {
            if ((((CheckBox)gvR.FindControl("chkAssign"))).Checked)
            {
                IscheckboxSelected = true;
                string regid = ((HiddenField)gvR.FindControl("lblregid")).Value;
                string stepid = ((HiddenField)gvR.FindControl("hdnstepid")).Value;
                string pid = ((HiddenField)gvR.FindControl("hdnpid")).Value;

                UpdateSiteVisitDisposition(Convert.ToInt32(regid), "Y", Convert.ToInt32(stepid), Convert.ToInt32(pid), string.Empty);
            }
        }
        if (!IscheckboxSelected)
        {
            MessageBox2.Show(UserControls_MessageModal.MessageModalMode.Continue, "Error", "Please select a provider to queue for assignment or bypass.");
            return;
        }
        ddlAttempt.SelectedIndex = 0;
        ddlSearchAssignToUser.SelectedIndex = 0;

        LoadPendingSiteVisits();
        //Response.Redirect(Request.RawUrl);
    }
    private void UpdateSiteVisitDisposition(int regID, string siteVisitNeeded, int stepID, int processID, string notes)
    {
        List<SqlParameter> sqlParms = new List<SqlParameter>();
        sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
        sqlParms.Add(SqlParms.CreateParameter("SITE_VISIT_NEEDED", DbType.String, siteVisitNeeded, false));
        sqlParms.Add(SqlParms.CreateParameter("SITE_VISIT_DISPOSITION_REVIEWD_BY", DbType.Guid, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), false));
        sqlParms.Add(SqlParms.CreateParameter("STEP_ID", DbType.Int32, stepID, false));
        sqlParms.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, processID, false));
        sqlParms.Add(SqlParms.CreateParameter("NOTES", DbType.String, notes, false));
        DataAccess.ExecuteStoredProcedure("usp_UpdateSiteVisitDisposition", sqlParms);

        ProviderFeedHelper.InsertProviderFeedNotes(regID, 0, HttpContext.Current.User.Identity.Name, " Site Visit Assignment Note - " + notes, null, null, finalDisposition: CON.FinalDisposition.NotSubmitted, processID: processID);
    }
    protected void btnSiteVisitNotNeeded_Click(object sender, EventArgs e)
    {

        GridViewRowCollection gvRowC = gvPendingSiteVisits.Rows;
        bool IscheckboxSelected = false;
        foreach (GridViewRow gvR in gvRowC)
        {
            if ((((CheckBox)gvR.FindControl("chkAssign"))).Checked)
            {
                IscheckboxSelected = true;
                string regid = ((HiddenField)gvR.FindControl("lblregid")).Value;
                string stepid = ((HiddenField)gvR.FindControl("hdnstepid")).Value;
                string pid = ((HiddenField)gvR.FindControl("hdnpid")).Value;

                UpdateSiteVisitDisposition(Convert.ToInt32(regid), "N", Convert.ToInt32(stepid), Convert.ToInt32(pid), string.Empty);
            }
        }
        if (!IscheckboxSelected)
        {
            MessageBox2.Show(UserControls_MessageModal.MessageModalMode.Continue, "Error", "Please select a provider to queue for assignment or bypass.");
            return;
        }
        ddlAttempt.SelectedIndex = 0;
        ddlSearchAssignToUser.SelectedIndex = 0;

        LoadPendingSiteVisits();
        // Response.Redirect(Request.RawUrl);
    }
    protected void btnHideNoteSVA_Click(object sender, EventArgs e)
    {
        pnlSVAAddNote.Style.Add("display", "none");
        mpeSVAAddNote.Hide();
    }
    protected void btnSaveNoteSVA_Click(Object sender, EventArgs e)
    {
        UpdateSiteVisitDisposition(Convert.ToInt32(hdnRegisID.Value), string.Empty, 0, Convert.ToInt32(hdnProcessID.Value), string.Concat(DateTime.Now.ToString(), " - ", txtSVAComments.Text));
        pnlSVAAddNote.Style.Add("display", "none");
        mpeSVAAddNote.Hide();
        LoadPendingSiteVisits();
        //Response.Redirect(Request.RawUrl);
    }
    protected void lnkSVAExcel_Click(object sender, EventArgs e)
    {
        int attempt = 0;
        string assignedTo = "";
        DataSet ds = null;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        ds = svc.GetUnassignedSiteVisits(attempt, assignedTo, 0);
        RadGridExportSVA.DataSource = ds;
        RadGridExportSVA.DataBind();
        RadGridExportSVA.MasterTableView.ExportToExcel();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlAttempt.SelectedIndex = 0;
        ddlSearchAssignToUser.SelectedIndex = 0;
        txtRegID.Text = string.Empty;
        txtSVAComments.Text = string.Empty;
        LoadPendingSiteVisits();
    }

    private bool IsUserMember(string str)
    {
        bool isUnassigned = false;

        Guid value = new Guid();
        if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
        {
            value = new Guid(str);
            MembershipUser user = Membership.GetUser((object)value);
            if (user == null)
            {
                isUnassigned = true;
            }
            else
            {
                isUnassigned = false;
            }
        }
        else
        {
            isUnassigned = true;
        }
        return isUnassigned;
    }
    private void LoadMyDashboard()
    {
        int attempt = 0;
        string assignedTo = "";
        int regID = string.IsNullOrEmpty(txtRegID.Text) ? 0 : Convert.ToInt32(txtRegID.Text);

        int initialAssigned = 0;
        int initialUnAssigned = 0;
        int followupAssigned = 0;
        int followupUnAssigned = 0;

        var psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetUnassignedSiteVisits(attempt, assignedTo, regID);
        foreach (DataRow dataRow in ds.Tables[0].Rows)
        {
            bool isAssigned = string.IsNullOrEmpty(dataRow["OWNER_ID"].ToString()) ? false : true;

            var siteVisitAttempt = dataRow["ATTEMPT"].ToString();

            if (siteVisitAttempt == CON.SiteVisitAttempt.Initial && isAssigned)
            {
                initialAssigned++;
            }
            else if (siteVisitAttempt == CON.SiteVisitAttempt.Initial && !isAssigned)
            {
                initialUnAssigned++;
            }
            else if (siteVisitAttempt == CON.SiteVisitAttempt.Follow_up && isAssigned)
            {
                followupAssigned++;
            }
            else if (siteVisitAttempt == CON.SiteVisitAttempt.Follow_up && !isAssigned)
            {
                followupUnAssigned++;
            }
        }
        var dataTable = new DataTable();
        dataTable.Columns.Add("Attempt", typeof(string));
        dataTable.Columns.Add("Assigned", typeof(string));
        dataTable.Columns.Add("UnAssigned", typeof(string));

        var dr = dataTable.NewRow();
        dr["Attempt"] = "Initial";
        dr["Assigned"] = initialAssigned.ToString();
        dr["UnAssigned"] = initialUnAssigned.ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr["Attempt"] = "Follow - Up";
        dr["Assigned"] = followupAssigned.ToString();
        dr["UnAssigned"] = followupUnAssigned.ToString();
        dataTable.Rows.Add(dr);

        dataTable.AcceptChanges();
        grdMyDashBoard.DataSource = dataTable;
        grdMyDashBoard.DataBind();

    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        GridViewRowCollection gvRowC = gvPendingSiteVisits.Rows;
        bool IscheckboxSelected = false;
        foreach (GridViewRow gvR in gvRowC)
        {
            if ((((CheckBox)gvR.FindControl("chkAssign"))).Checked)
            {
                if (ddlSiteVisitOper.SelectedIndex <= 0)
                {
                    MessageBox2.Show(UserControls_MessageModal.MessageModalMode.Continue, "Error", "Please select the site visit operator.");
                    return;
                }
                const string action = "Next";
                IscheckboxSelected = true;
                string regid = ((HiddenField)gvR.FindControl("lblregid")).Value;
                string stepid = ((HiddenField)gvR.FindControl("hdnstepid")).Value;
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                string pid = ((HiddenField)gvR.FindControl("hdnpid")).Value;
                if (gvR.Cells[7].Text == "Unassigned")
                {
                    svc.WF_StartStep(Convert.ToInt32(pid), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    svc.WF_TakeAction(Convert.ToInt32(pid), action, string.Empty);
                    DataSet processDataSet = svc.WF_SelectProcess(Convert.ToInt32(pid));
                    if (Helper.HasRows(processDataSet))
                    {
                        int currentStepId = Helper.GetInt("CURRENT_STEP_ID", processDataSet.Tables[0].Rows[0]);
                        svc.updateWF_STEP_Owner(Convert.ToInt32(currentStepId), ddlSiteVisitOper.SelectedIndex == 0 ? null : ddlSiteVisitOper.SelectedValue);
                    }
                }
                else
                {
                    svc.updateWF_STEP_Owner(Convert.ToInt32(stepid), ddlSiteVisitOper.SelectedIndex == 0 ? null : ddlSiteVisitOper.SelectedValue);
                }
            }
        }
        if (!IscheckboxSelected)
        {
            MessageBox2.Show(UserControls_MessageModal.MessageModalMode.Continue, "Error", "Please select the check box to assign.");
            return;
        }
        ddlAttempt.SelectedIndex = 0;
        ddlSearchAssignToUser.SelectedIndex = 0;

        LoadPendingSiteVisits();
        Response.Redirect(Request.RawUrl);
    }
    protected void gvPendingSiteVisits_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        int.TryParse(e.CommandArgument.ToString(), out index);
        if (e.CommandName == "AddNote")
        {
            pnlSVAAddNote.Style.Remove("display");
            hdnRegisID.Value = gvPendingSiteVisits.DataKeys[index].Values["REG_ID"].ToString();
            hdnProcessID.Value = gvPendingSiteVisits.DataKeys[index].Values["PROCESS_ID"].ToString();
            mpeSVAAddNote.Show();
        }
    }
}