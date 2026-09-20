using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MAXIMUS.Core.Libraries;

public partial class MasterWorkflowPage : System.Web.UI.MasterPage
{
    private const string SQL_GET_STEP_INFO =
         "SELECT WF_WORKFLOW.NAME AS WORKFLOW_NAME"
         + " , WF_TASK.NAME AS TASK_NAME"
         + " , WF_PARAMETER.PARAMETER_NAME AS PARAMETER_NAME"
         + " , WF_PARAMETER.PARAMETER_VALUE AS PARAMETER_VALUE"
         + " FROM WF_STEP"
         + " LEFT OUTER JOIN WF_PARAMETER"
         + " ON WF_PARAMETER.PROCESS_ID = WF_STEP.PROCESS_ID"
         + " JOIN WF_TASK"
         + " ON WF_TASK.TASK_ID = WF_STEP.TASK_ID"
         + " JOIN WF_WORKFLOW"
         + " ON WF_WORKFLOW.WORKFLOW_ID = WF_TASK.WORKFLOW_ID"
         + " WHERE WF_STEP.STEP_ID = {0}";

    private const string SQL_GET_STEP_ACTIONS =
        "SELECT WF_ACTION.NAME AS ACTION_NAME"
        + " , WF_ACTION.ACTION_ID"
        + " , WF_ACTION.NEXT_TASK_ID"
        + " FROM WF_STEP"
        + " JOIN WF_ACTION"
        + " ON WF_ACTION.TASK_ID = WF_STEP.TASK_ID"
        + " WHERE WF_STEP.STEP_ID = {0}"
        + " ORDER BY WF_ACTION.ACTION_ID";

    private const string SQL_ADD_STEP_PARAMETER =
        "INSERT INTO WF_PARAMETER"
        + " ( STEP_ID"
        + " , PARAMETER_NAME"
        + " , PARAMETER_VALUE )"
        + " VALUES"
        + " ( {0}"
        + " , '{1}'"
        + " , '{2}' )";

    private int processID;
    private int stepID;
    public string workflowName;
    public string taskName;
    public string keyName;
    public string keyValue;

    protected void Page_Load(object sender, EventArgs e)
    {
        int.TryParse(Session["WF_ProcessID"].ToString(), out processID);
        int.TryParse(Session["WF_StepID"].ToString(), out stepID);

        if (!IsPostBack)
        {
            lblVersion.Text = "Version: " + Helper.GetAppSetting("Version");
            // Get version number

            DataSet ds1 = new DataSet();
            ds1 = DataAccess.ExecuteSelectSql(string.Format(SQL_GET_STEP_ACTIONS, stepID.ToString()));
            Helper.LoadDropDown(ddlAction, ds1.Tables[0], "ACTION_NAME", "NEXT_TASK_ID", true);
        }

        DataSet ds2 = new DataSet();
        ds2 = DataAccess.ExecuteSelectSql(string.Format(SQL_GET_STEP_INFO, stepID.ToString()));
        if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
        {
            workflowName = ds2.Tables[0].Rows[0]["WORKFLOW_NAME"].ToString();
            taskName = ds2.Tables[0].Rows[0]["TASK_NAME"].ToString();
            keyName = ds2.Tables[0].Rows[0]["PARAMETER_NAME"].ToString();
            keyValue = ds2.Tables[0].Rows[0]["PARAMETER_VALUE"].ToString();
        }
    }

    private void SetMenuItemText(MenuItemCollection col, string itemText, string newItemText)
    {
        foreach (MenuItem itm in col)
        {
            if (itm.ChildItems.Count > 0) SetMenuItemText(itm.ChildItems, itemText, newItemText);
            if (itm.Value == itemText)
            {
                itm.Text = newItemText;
                return;
            }
        }
    }

    private void RemoveMenuItem(MenuItemCollection col, string value)
    {
        foreach (MenuItem itm in col)
        {
            if (itm.ChildItems.Count > 0) RemoveMenuItem(itm.ChildItems, value);
            if (itm.Value == value)
            {
                col.Remove(itm);
                return;
            }
        }
    }

    // Remove the "PDMS to MMIS" menu option if necessary
    protected void mnuTop_PreRender(object sender, EventArgs e)
    {
        Menu mnu = (Menu)sender;
        if (mnu == null) return;
        if (!SessionVarRetriever.EnablePDMStoMMIS) RemoveMenuItem(mnu.Items, "PDMS to MMIS");
    }

    protected void btnProcess_Click(object sender, EventArgs e)
    {
        string action = ddlAction.SelectedItem.ToString();

        if (!string.IsNullOrEmpty(action))
        {
            if (!string.IsNullOrEmpty(txtParamterName.Text))
            {
                DataSet dsResult = new DataSet();
                dsResult = DataAccess.ExecuteSelectSql(string.Format(SQL_ADD_STEP_PARAMETER, stepID.ToString(), txtParamterName.Text, txtParamterValue.Text));
            }

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
            parameters.Add(SqlParms.CreateParameter("ActionName", DbType.String, action, true));
            parameters.Add(SqlParms.CreateParameter("Notes", DbType.String, txtNotes.Text, true));

            DataAccess.ExecuteStoredProcedure("usp_WF_TakeAction", parameters);
        }
    }
}
