using Glimpse.Core.Extensibility;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Interop;

public partial class Controls_PowerAgentGrid : System.Web.UI.UserControl
{
    // Events
    public event EventHandler PowerAgentAdded;
    public event EventHandler PowerAgentUpdated;
    public event EventHandler PowerAgentDeactivated;
    public DataSet dsPowerAgents = new DataSet();
    // Property to track editing row
    private int EditIndex
    {
        get
        {
            int index = -1;
            if (!string.IsNullOrEmpty(hfEditIndex.Value))
            {
                int.TryParse(hfEditIndex.Value, out index);
            }
            return index;
        }
        set
        {
            hfEditIndex.Value = value.ToString();
        }
    }

    private bool IsPowerAgent
    {
        get
        {
            return Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, 0);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadPowerAgents();
            if (IsPowerAgent)
            {
                string powerAgentId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
                List<ListItem> items = new List<ListItem>
                {
                    new ListItem("Select OHID", "0"),
                    new ListItem(Helper.GetUserName(SessionVarRetriever.SelectedProviderAdminUserID), SessionVarRetriever.SelectedProviderAdminUserID)
                };
                ddlProviderAdmins.DataSource = items;
                ddlProviderAdmins.DataTextField = "Text";
                ddlProviderAdmins.DataValueField = "Value";
                ddlProviderAdmins.DataBind();
                thPAdmin.Visible = true;
                tdPAdmin.Visible = true;
            }
            else
            {
                thPAdmin.Visible = false;
                tdPAdmin.Visible = false;
            }
        }
        lblError.Visible = false;
        lblSuccess.Visible = false;
    }

    public void ShowPanel(bool show)
    {
        pnlPowerAgents.Visible = show;
        if (show)
        {
            LoadPowerAgents();
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        // Clear fields
        txtNewOhId.Text = string.Empty;
        lblNewEmail.Text = string.Empty;
        lblNewUserName.Text = string.Empty;
        chkNewAccessManagement.Checked = false;
        lblValidationMessage.Text = string.Empty;
        divAddAgentTable.Visible = true;
        divConfirmMsg.Visible = false;
        // Show modal
        mpeAddNew.Show();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string searchTerm = txtSearch.Text.Trim();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            SearchPowerAgents(searchTerm);
        }
        else
        {
            LoadPowerAgents();
        }
    }
    protected void gvPowerAgents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        int pageIndexOffset = gvPowerAgents.PageIndex * gvPowerAgents.PageSize;
        int actualRowIndex = rowIndex - pageIndexOffset;

        if (e.CommandName == "EditAgent")
        {
            EditIndex = actualRowIndex;
            LoadPowerAgents();
        }
        else if (e.CommandName == "SaveAgent")
        {
            GridViewRow row = gvPowerAgents.Rows[actualRowIndex];

            CheckBox chkAccessManagement = (CheckBox)row.FindControl("chkAccessManagement");
            string id = gvPowerAgents.DataKeys[actualRowIndex].Value.ToString();

            UpdatePowerAgentAccessManagement(id, chkAccessManagement.Checked);

            EditIndex = -1;
            LoadPowerAgents();

            if (PowerAgentUpdated != null)
                PowerAgentUpdated(this, EventArgs.Empty);
        }
        else if (e.CommandName == "CancelEdit")
        {
            EditIndex = -1;
            LoadPowerAgents();
        }
        else if (e.CommandName == "DeactivateAgent")
        {
            string id = e.CommandArgument.ToString();
            DeactivateAgent(id);
        }
    }


    protected void gvPowerAgents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int rowIndex = e.Row.RowIndex;
            bool isEditing = (rowIndex == EditIndex);

            // Get controls
            CheckBox chkAccessManagement = (CheckBox)e.Row.FindControl("chkAccessManagement");
            Panel pnlViewMode = (Panel)e.Row.FindControl("pnlViewMode");
            Panel pnlEditMode = (Panel)e.Row.FindControl("pnlEditMode");

            if (isEditing)
            {
                // Enable checkbox for editing
                chkAccessManagement.Enabled = true;

                // Show edit mode buttons
                pnlViewMode.Visible = false;
                pnlEditMode.Visible = true;

                // Add visual indicator
                e.Row.CssClass = "editing-row";
            }
            else
            {
                // Keep checkbox disabled
                chkAccessManagement.Enabled = false;

                // Show view mode buttons
                pnlViewMode.Visible = true;
                pnlEditMode.Visible = false;
            }
        }
    }

    protected void btnSaveNew_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            mpeAddNew.Show();
            return;
        }
        else
        {
            // Add new power agent
            bool isValid = AddPowerAgent(false);

            if (isValid)
            {
                // Hide modal
                mpeAddNew.Hide();

                // Reload grid
                LoadPowerAgents();

                // Raise event
                if (PowerAgentAdded != null)
                    PowerAgentAdded(this, EventArgs.Empty);

                lblValidationMessage.Text = string.Empty;
            }
        }
    }

    protected void btnCancelNew_Click(object sender, EventArgs e)
    {
        lblValidationMessage.Text = string.Empty;
        mpeAddNew.Hide();
    }

    private void LoadPowerAgents(int pageIndex = 0)
    {
        string loggedinUserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        int totalResultCount = 0;
        DataSet ds = svc.GetPowerAgentByUserId(loggedinUserID, gvPowerAgents.PageSize, pageIndex + 1,SessionVarRetriever.SelectedProviderAdminUserID ,out totalResultCount);
        gvPowerAgents.DataSource = dsPowerAgents = ds;
        gvPowerAgents.DataBind();
    }

    private void SearchPowerAgents(string searchTerm)
    {
        // TODO: Implement search logic
        LoadPowerAgents(); // For now, just reload all
        var filteredRows = from row in dsPowerAgents.Tables[0].AsEnumerable()
                           where (row.Field<string>("PowerAgentOhId").Contains(searchTerm) || 
                                  row.Field<string>("PowerAgentUserName").Contains(searchTerm) || 
                                  row.Field<string>("PowerAgentEmail").Contains(searchTerm))
                           select row;
        if (filteredRows.Any()) // Check if there are any results before copying
        {
            gvPowerAgents.DataSource = filteredRows.CopyToDataTable();
            gvPowerAgents.DataBind();
        }
    }

    private void UpdatePowerAgentAccessManagement(string id, bool hasAccessManagement)
    {
        string userId = GetUserId();

        try
        {
            DataSet result = CallUpdateAccessManagementPowerAgentService(userId, id, hasAccessManagement);
            PowerAgentResult parsed = ParseServiceResult(result);

            if (parsed.IsSuccess)
            {
                ShowSuccessMessage(parsed.Message);
                // Reload grid
                LoadPowerAgents();

                // Raise event
                if (PowerAgentDeactivated != null)
                    PowerAgentDeactivated(this, EventArgs.Empty);
            }
            else
            {
                lblError.Text = GetErrorMessage(parsed.Message);
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error updating Access Management for Power Agent: " + ex.Message;
            lblError.Visible = true;
            lblSuccess.Visible = false;
        }
    }

    private void DeactivateAgent(string id)
    {
        string userId = GetUserId();

        try
        {
            DataSet result = CallDeactivatePowerAgentService(userId, id);
            PowerAgentResult parsed = ParseServiceResult(result);

            if (parsed.IsSuccess)
            {
                ShowSuccessMessage(parsed.Message);
                // Reload grid
                LoadPowerAgents();

                // Raise event
                if (PowerAgentDeactivated != null)
                    PowerAgentDeactivated(this, EventArgs.Empty);
            }
            else
            {
                lblError.Text = GetErrorMessage(parsed.Message);
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error deactivating Power Agent: " + ex.Message;
            lblError.Visible = true;
            lblSuccess.Visible = false;
        }
    }

    private bool AddPowerAgent(bool overRideValidation)
    {
        bool isValid = true;
        string ohId = txtNewOhId.Text.Trim();
        bool hasAccessManagement = chkNewAccessManagement.Checked;
        string userId = IsPowerAgent ? ddlProviderAdmins.SelectedValue : GetUserId();

        try
        {
            DataSet result = CallAddPowerAgentService(userId, ohId, hasAccessManagement, overRideValidation);
            PowerAgentResult parsed = ParseServiceResult(result);

            if (parsed.IsSuccess)
            {
                ShowSuccessMessage(parsed.Message);
            }
            else
            {
                if (lblValidationMessage != null)
                {

                    string errMsg = GetErrorMessage(parsed.Message);
                    if (errMsg == "ALREADY_PROVIDER_AGENT")
                    {
                        divAddAgentTable.Visible = false;
                        divConfirmMsg.Visible = true;
                    }
                    else
                    {
                        divAddAgentTable.Visible = true;
                        divConfirmMsg.Visible = false;
                        lblValidationMessage.Text = GetErrorMessage(parsed.Message);
                        lblValidationMessage.Visible = true;
                        
                    }
                    if (mpeAddNew != null) mpeAddNew.Show();
                }
                
                isValid = false;
            }
        }
        catch (Exception ex)
        {
            if (lblValidationMessage != null)
            {
                lblValidationMessage.Text = "Error adding Power Agent: " + ex.Message;
                lblValidationMessage.Visible = true;
            }
            if (mpeAddNew != null) mpeAddNew.Show();
        }
        return isValid;
    }
    private string GetUserId()
    {
        return Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
    }

    private PowerAgentResult ParseServiceResult(DataSet result)
    {
        PowerAgentResult output = new PowerAgentResult();

        if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
        {
            DataRow row = result.Tables[0].Rows[0];

            if (row["IsSuccess"] != DBNull.Value)
            {
                output.IsSuccess = Convert.ToBoolean(Convert.ToInt32(row["IsSuccess"]));
            }

            if (row["ErrorCode"] != DBNull.Value)
            {
                output.Message = row["ErrorCode"].ToString();
            }
        }

        return output;
    }

    private void ShowSuccessMessage(string message)
    {
        lblSuccess.Text = message;
        lblSuccess.Visible = true;
        lblError.Visible = false;
    }

    private string GetErrorMessage(string message)
    {
        string msg = string.Empty;
        if (message == "OHID_NOT_EXISTS")
        {
            msg = "OH ID entered does not exist.";
        }
        else if (message == "SAME_USER" || message == "NOT_PROVIDER_AGENT")
        {
            msg = "OH ID entered is not a provider agent.";
        }
        else if (message == "ALREADY_POWER_AGENT")
        {
            msg = "OH ID is already provisioned as power agent to this administrator.";
        }
        else if (message == "MAX_LIMIT_DELEGATE")
        {
            msg = "You have reached the maximum limit of 10 power agents. Delegates cannot add more than 10. ";
        }
        else if (message == "MAX_LIMIT_PROVIDER")
        {
            msg = "You have reached the maximum limit of 5 power agents. Provider admins cannot add more than 5. ";
        }
        else if(message == "ALREADY_PROVIDER_AGENT")
        {
            msg = "ALREADY_PROVIDER_AGENT";          
        }
        else
        {
            msg = message;
        }

        return msg;
    }  

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        AddPowerAgent(true);
        // Hide modal
        mpeAddNew.Hide();

        // Reload grid
        LoadPowerAgents();

        // Raise event
        if (PowerAgentAdded != null)
            PowerAgentAdded(this, EventArgs.Empty);

        lblValidationMessage.Text = string.Empty;
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

    /// <summary>
    /// Method to call Service layer for Add Power Agent
    /// </summary>
    /// <param name="providerAdminUserId">Provider Admin User</param>
    /// <param name="ohId">Power Agent Id</param>
    /// <param name="hasAccessManagement"></param>
    /// <returns></returns>
    private DataSet CallAddPowerAgentService(string providerAdminUserId, string ohId, bool hasAccessManagement, bool overRideValidation)
    {
        return svc.AddPowerAgent(providerAdminUserId, ohId, hasAccessManagement, providerAdminUserId, overRideValidation);
    }

    /// <summary>
    /// Method to call Service layer for Deactivate Power Agent
    /// </summary>
    /// <param name="userId">Modified By User Id</param>
    /// <param name="ohId">Power Agent Id</param>
    /// <returns></returns>
    private DataSet CallDeactivatePowerAgentService(string userId, string ohId)
    {
        return svc.DeactivatePowerAgent(userId, ohId);
    }

    /// <summary>
    /// Method to call service layer for Updating Access Management for Power Agent
    /// </summary>
    /// <param name="userId">Modified By User Id</param>
    /// <param name="ohId">Power Agent I</param>
    /// <param name="hasAccessManagement">Access Management true/false</param>
    /// <returns></returns>
    private DataSet CallUpdateAccessManagementPowerAgentService(string userId, string ohId, bool hasAccessManagement)
    {
        return svc.UpdateAccessManagementForPowerAgent(userId, ohId, hasAccessManagement);
    }
    #endregion

    protected void gvPowerAgents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPowerAgents.PageIndex = e.NewPageIndex;
        LoadPowerAgents(e.NewPageIndex);
    }


    protected void gvPowerAgents_DataBound(object sender, EventArgs e)
    {
        if (gvPowerAgents.BottomPagerRow != null)
        {
            // Force pager to show even if there's only one page
            gvPowerAgents.BottomPagerRow.Visible = true;
        }
    }
}

public class PowerAgentResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}