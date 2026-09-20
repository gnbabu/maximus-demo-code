using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class WF_Engine : System.Web.UI.Page
{
    public int ProcessId
    {
        get
        {
            if (ViewState["ProcessId"] == null) ViewState["ProcessId"] = 0;
            return Convert.ToInt32(ViewState["ProcessId"]);
        }
        set { ViewState["ProcessId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Helper.IsLoggedInUserInAdminRole())
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
            PopulateGrid();
        }
    }



    public void DeleteRegistration(int RegistrationId, int processid)
    {
        var modifiedBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("process_ID", DbType.Int32, processid, false));
        parameters.Add(SqlParms.CreateParameter("REGISTRATION_ID", DbType.Int32, RegistrationId, false));

        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));

        DataSet deletedData = DataAccess.ExecuteStoredProcedure("USP_DELETE_WORFLOW_REGISTRATION", parameters, "WFDeletedData");
        if (Helper.HasRows(deletedData))
        {
            DataTable purgedrowCount = deletedData.Tables[0];

        }
    }

    private void PopulateGrid(int pageID = 1)
    {
        DataSet ds = null;
        string countRows = string.Empty;
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("IgnoreSleepTime", DbType.Boolean, 1, false));
        parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageID, false));
        ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectActiveSystemStepsPaging", parameters, "WorkflowSteps");
        if (Helper.HasRows(ds))
        {
            DataView dv;
            dv = ds.Tables[0].DefaultView;

            foreach (DataRow row in dv.Table.Rows)
            {
                if (string.IsNullOrEmpty(Convert.ToString(row["WORKFLOW_EVENT_TYPE_ID"])))
                {
                    row["WORKFLOW_EVENT_TYPE_ID"] = 1;
                }
            }

            gvAwaitingAction.DataSource = dv;
            countRows = dv[0]["TotalRows"].ToString();
        }
        else gvAwaitingAction.DataSource = null;
        if (!string.IsNullOrEmpty(countRows))
        {
            gvAwaitingAction.VirtualItemCount = Convert.ToInt32(countRows);
        }

        gvAwaitingAction.DataBind();
        lblTotal.Text = countRows;
        String en = DataAccess.GetEnvironment();
        if (en == CON.Environment.UAT || en == CON.Environment.PROD || en == CON.Environment.INT)
        {
            gvAwaitingAction.Columns[11].Visible = false;
        }
        else
        {
            gvAwaitingAction.Columns[11].Visible = true;
        }
    }

    protected void btnGetProcess_Click(object sender, EventArgs e)
    {
        vsWorkflowEngine.Visible = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        List<string> results = psc.WF_ProcessAllTasks();
        if (results.Count > 0)
        {
            foreach (string str in results) AddError(str);
        }
        else PopulateGrid();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    private void AddError(string errMsg)
    {
        vsWorkflowEngine.Visible = true;
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valWorkflowEngine";
        this.Page.Validators.Add(val);
    }

    private bool AdvanceStep(object sender, int processID)
    {
        bool rtn = false;
        Control ctl = sender as Control;
        GridViewRow CurrentRow = ctl.NamingContainer as GridViewRow;
        CheckBox chk = (CheckBox)CurrentRow.FindControl("chkAdvance");
        if (chk != null)
        {
            if (chk.Checked)
            {
                // If advance then move to the next step regardless of error
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = psc.WF_SelectProcess(processID);
                if (!Helper.HasRows(ds)) return false;
                ds = psc.WF_SelectStepActions(Helper.GetInt("CURRENT_STEP_ID", ds.Tables[0].Rows[0]));
                if (!Helper.HasRows(ds)) return false;
                DataRow[] actionRows = ds.Tables[0].Select("1=1");          // Get all the rows
                if (actionRows.Length > 1)                                  // More than 1, get the NEXT action only
                    actionRows = ds.Tables[0].Select("ACTION_NAME = 'Next'");
                if (actionRows.Length == 0) return false;
                WriteLog("User " + HttpContext.Current.User.Identity.Name + " Advacing task from WF UI for process = " + processID);

                psc.WF_TakeAction(processID, Helper.GetString("ACTION_NAME", actionRows[0]), "Advance option from WF_Engine.aspx");
                rtn = true;
            }
        }
        return rtn;
    }

    protected void lnkProcess_Click(object sender, CommandEventArgs e)
    {
        if (("ProcessRow").Equals(e.CommandName))
        {
            vsWorkflowEngine.Visible = false;
            int processID = Convert.ToInt32(e.CommandArgument);
            if (AdvanceStep(sender, processID))
            {
                PopulateGrid();
            }
            else
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = psc.WF_SelectProcess(processID);
                if (Helper.HasRows(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    WriteLog("User " + HttpContext.Current.User.Identity.Name + " Processing task from WF UI for process = " + Convert.ToInt32(dr["PROCESS_ID"]));

                    string result = psc.WF_ProcessTask(Convert.ToInt32(dr["PROCESS_ID"]), dr["ASSEMBLY_NAME"].ToString(),
                        dr["CLASS_NAME"].ToString());
                    if (!string.IsNullOrEmpty(result)) AddError(result);
                    else PopulateGrid();
                }
                else AddError("Process has no data in Workflow tables");
            }
        }
    }

    protected void lnkPause_Click(object sender, CommandEventArgs e)
    {
        if (("PauseRow").Equals(e.CommandName))
        {
            LinkButton lnk = (LinkButton)sender;
            if (lnk != null)
            {
                if (lnk.Text == "Pause")
                {
                    ddlMinutes.SelectedIndex = -1;
                    ProcessId = Convert.ToInt32(e.CommandArgument);
                    lblTitle.Text = "Pause Workflow Process " + ProcessId.ToString();
                    btnSave.Attributes.Add("onclick", "javascript:return confirm('Pause Process ID " + ProcessId.ToString() +
                        ". Are you sure?');");
                    mpe.Show();
                }
                else
                {
                    // Unpause the Process, DO NOT pass Minutes parameter and thereby they will be NULL
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, Convert.ToInt32(lnk.CommandArgument), false));
                    DataAccess.ExecuteStoredProcedure("usp_WF_PauseProcess", parameters);
                    WriteLog("User " + HttpContext.Current.User.Identity.Name + " took Workflow Process " +
                        lnk.CommandArgument.ToString() + " off of Pause");
                    PopulateGrid();
                }
            }
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        int processID = 0;
        if (!string.IsNullOrEmpty(System.Text.RegularExpressions.Regex.Replace(txtProcessID.Text, "\\D", string.Empty)))
            processID = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(txtProcessID.Text, "\\D", string.Empty));
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.WF_SelectProcess(processID);
        if (Helper.HasRows(ds)) gvProcess.DataSource = ds.Tables[0];
        else gvProcess.DataSource = null;
        gvProcess.DataBind();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtProcessID.Text = string.Empty;
        gvProcess.DataSource = null;
        gvProcess.DataBind();
    }

    protected void gvAwaitingAction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk = (LinkButton)e.Row.FindControl("lnkEdit");
            if (lnk != null)
            {
                lnk.Attributes.Add("onclick", " this.disabled = true;");
            }
            lnk = (LinkButton)e.Row.FindControl("lnkPause");
            if (lnk != null)
            {
                int pauseMinutes = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PauseMinutes"));
                if (pauseMinutes > 0)
                {
                    lnk.Text = pauseMinutes.ToString() + " Min";
                    lnk.Attributes.Add("onclick", "javascript:return confirm('Turn off Pause. Are you sure?');");
                }
            }
            //string a = e.Row.Cells[15].Text;
            int REG_PROGRAM_STATUS_TYPE_ID = (int)((DataRowView)e.Row.DataItem).Row["REG_PROGRAM_STATUS_TYPE_ID"];


            int WORKFLOW_EVENT_TYPE_ID = (int)((DataRowView)e.Row.DataItem).Row["WORKFLOW_EVENT_TYPE_ID"];
            bool isNewCustomer = (REG_PROGRAM_STATUS_TYPE_ID == 1 && WORKFLOW_EVENT_TYPE_ID == 1);



            LinkButton btnDelete = ((LinkButton)e.Row.FindControl("btnDelete"));
            LinkButton btnCancel = ((LinkButton)e.Row.FindControl("btnCancel"));


            if (isNewCustomer)
            {
                btnDelete.Visible = true;
                btnCancel.Visible = false;
            }
            else
            {
                btnDelete.Visible = false;
                btnCancel.Visible = true;
            }

        }
    }

    protected void gvAwaitingAction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAwaitingAction.PageIndex = e.NewPageIndex;
        PopulateGrid(e.NewPageIndex + 1);
    }

    private void WriteLog(string msg)
    {
        // create log object
        string logProcessName = "Workflow Engine Administration UI";
        Logging log = new Logging(new Guid("2CBDE776-04F6-4CAD-8598-B1131B6A2B99"), logProcessName);
        log.CreateLogEntry(msg);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessId, false));
        parameters.Add(SqlParms.CreateParameter("Minutes", DbType.Int32, ddlMinutes.SelectedValue, false));
        DataAccess.ExecuteStoredProcedure("usp_WF_PauseProcess", parameters);
        WriteLog("User " + HttpContext.Current.User.Identity.Name + " paused Workflow Process " + ProcessId.ToString() +
            " for " + ddlMinutes.SelectedValue + " minutes");
        PopulateGrid();
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        // Make sure the user is logged in before we disconnect them
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();                  // Remove the session entries
        }
        Response.Redirect(Helper.RedirectLoginURL());
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        GridViewRow grdRow = (GridViewRow)linkButton.NamingContainer;
        int RowIndex = grdRow.RowIndex;

        int RegitrationId = Convert.ToInt32(grdRow.Cells[8].Text);
        int processID = Convert.ToInt32(grdRow.Cells[6].Text);

        DeleteRegistration(RegitrationId, processID);

        PopulateGrid();
    }
    private void CancelRegistration(int RegistrationId, int StepId, int ProcessID)
    {
        var modifiedBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, false));
        parameters.Add(SqlParms.CreateParameter("REGISTRATION_ID", DbType.Int32, RegistrationId, false));
        parameters.Add(SqlParms.CreateParameter("STEP_ID", DbType.Int32, StepId, false));
        parameters.Add(SqlParms.CreateParameter("USER", DbType.Guid, modifiedBy, false));

        DataSet deletedData = DataAccess.ExecuteStoredProcedure("USP_CANCEL_WORKFLOW", parameters, "WFDeletedData");
        if (Helper.HasRows(deletedData))
        {
            DataTable purgedrowCount = deletedData.Tables[0];

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        GridViewRow grdRow = (GridViewRow)linkButton.NamingContainer;
        int RowIndex = grdRow.RowIndex;

        int RegistrationId = Convert.ToInt32(grdRow.Cells[8].Text);
        int StepId = Convert.ToInt32(grdRow.Cells[7].Text);
        int processID = Convert.ToInt32(grdRow.Cells[6].Text);

        CancelRegistration(RegistrationId, StepId, processID);

        PopulateGrid();
    }
}