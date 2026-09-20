using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Workflow_WF_EngineNew : System.Web.UI.Page
{

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";

    }

    public int ProcessId
    {
        get
        {
            if (ViewState["ProcessId"] == null) ViewState["ProcessId"] = 0;
            return Convert.ToInt32(ViewState["ProcessId"]);
        }
        set { ViewState["ProcessId"] = value; }
    }

    protected void ImageButton_Click(object sender, ImageClickEventArgs e)
    {
        string alternateText = (sender as ImageButton).AlternateText;
        gvAwaitingAction.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), alternateText);
        gvAwaitingAction.ExportSettings.IgnorePaging = true;
        gvAwaitingAction.ExportSettings.ExportOnlyData = true;
        gvAwaitingAction.ExportSettings.OpenInNewWindow = true;
        gvAwaitingAction.ExportSettings.FileName = "WF_Engine";
        gvAwaitingAction.MasterTableView.ExportToExcel();
    }

    private void PopulateGrid(int pageID = 1, int RowsPerPage = 50)
    {
        DataSet ds = null;
        string countRows = string.Empty;
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("IgnoreSleepTime", DbType.Boolean, 1, false));
        parameters.Add(SqlParms.CreateParameter("PageNumber", DbType.Int32, pageID, false));
        parameters.Add(SqlParms.CreateParameter("RowsPerPage", DbType.Int32, RowsPerPage, false));
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
            // clear DataSource first
            gvAwaitingAction.DataSource = null;
            // reassignDataSource
            gvAwaitingAction.DataSource = dv;
            countRows = dv[0]["TotalRows"].ToString();
        }
        else gvAwaitingAction.DataSource = null;
        gvAwaitingAction.DataBind();
        if (!string.IsNullOrEmpty(countRows))
        {
            gvAwaitingAction.VirtualItemCount = Convert.ToInt32(countRows);
        }
        lblTotal.Text = countRows;
    }

    protected void lnkProcess_Click(object sender, CommandEventArgs e)
    {
        if (("ProcessRow").Equals(e.CommandName))
        {
            vsWorkflowEngine.Visible = false;
            int processID = Convert.ToInt32(e.CommandArgument);
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

    private void WriteLog(string msg)
    {
        // create log object
        string logProcessName = "Workflow Engine Administration UI";
        Logging log = new Logging(new Guid("2CBDE776-04F6-4CAD-8598-B1131B6A2B99"), logProcessName);
        log.CreateLogEntry(msg);
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

    protected void Page_Load(object sender, EventArgs e)
    {
		PopulateGrid(1,9999999);
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

}