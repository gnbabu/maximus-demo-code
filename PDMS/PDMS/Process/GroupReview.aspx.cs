using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_GroupReview : RegistrationProvider
{
    private void SetTitle()
    {
        lblTitle.Text = Resources.BrandingResource.GROUP_REVIEW_TITLE;
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
        //Set the title same as setTitle function to avoid 508 complaint
        Page.Title = Resources.BrandingResource.GROUP_REVIEW_TITLE;
    }

    private void LoadDashboardRegistration(int tableId, DataTable dt, int statusID, int ordinal, ref string registrationList, Boolean IsAssigned)
    {
        dt = SessionVarRetriever.GetDashBoardTotalIdList(tableId);
        
        foreach (DataRow dr in dt.Rows)
        {
            bool fnd = false;
            if ((statusID == 99 || statusID == Helper.GetInt("StatusID", dr)) &&
                (ordinal == 99 || ordinal == Helper.GetInt("Ordinal", dr))) fnd = true;
            if (fnd)
            {
                if (Helper.GetInt("REG_ID", dr) > 0)
                {
                    if (IsAssigned == Convert.ToBoolean(dr["IsAssigned"]))
                    {
                        registrationList += Helper.GetString("REG_ID", dr) + ",";
                    }
                }
            }
        }
    }

    private void LoadDashboard(int tableId, int statusID, int ordinal, int count,Boolean IsAssigned)
    {
        string registrationList = string.Empty;
        SessionVarRetriever.DashBoardTableId = SessionVarRetriever.DashBoardStatusID = SessionVarRetriever.DashBoardOrdinal = 0;
        SessionVarRetriever.DashBoardIsAssigned = true;
        DataTable dt = new DataTable();
        if (tableId >= CON.DashboardTableType.IndividualNewReg)                   // RegistrationIds
        {
            if (count > 250)   // If over a max of 250 results OR Group Totals result
            {
                SessionVarRetriever.DashBoardTableId = tableId;
                SessionVarRetriever.DashBoardStatusID = statusID;
                SessionVarRetriever.DashBoardOrdinal = ordinal;
                SessionVarRetriever.DashBoardIsAssigned = IsAssigned;
            }
            else LoadDashboardRegistration(tableId, dt, statusID, ordinal, ref registrationList,IsAssigned);
        }
        if (!string.IsNullOrEmpty(registrationList)) registrationList = registrationList.Substring(0, registrationList.Length - 1);
        SessionVarRetriever.DashBoardRegistrationIds = registrationList;
    }

    private void SetUserReadOnly()
    {
        Helper.SetUserReadOnly(HttpContext.Current.User.Identity.Name, this.Page, "btnAddNote");
        Helper.SetUserReadOnly(HttpContext.Current.User.Identity.Name, this.Page, "btnEmailNotification");
        Helper.SetUserReadOnly(HttpContext.Current.User.Identity.Name, this.Page, "btnOverrideError");
        Helper.SetUserReadOnly(HttpContext.Current.User.Identity.Name, this.Page, "btnUpdateRecord");
        Helper.SetUserReadOnly(HttpContext.Current.User.Identity.Name, this.Page, "lnkEdit");
    }

    private bool SearchIds()
    {
        if (!string.IsNullOrEmpty(SessionVarRetriever.DashBoardRegistrationIds)) return true;
        if (SessionVarRetriever.DashBoardTableId > 0) return true;
        return false;
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
            SetTitle();
            if (Request["TableId"] != null && Request["StatusID"] != null && Request["Ordinal"] != null)
            {
                int count = 0;
                if (Request["Count"] != null) count = Convert.ToInt32(Request["Count"]);
                LoadDashboard(Convert.ToInt32(Request["TableId"]), Convert.ToInt32(Request["StatusID"]), 
                    Convert.ToInt32(Request["Ordinal"]), count,Convert.ToBoolean(Request["IsAssigned"]));
            }
            else SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SetUserReadOnly();
            if (Helper.IsLoggedInUserInAdminRole())   
            {
	            var tblAssignUserCtl = ucGroupReview.FindControl("tblAssignUser");
	            if (tblAssignUserCtl != null)
		            tblAssignUserCtl.Visible = true;

                var tblBulkManageCtl = ucGroupReview.FindControl("tblBulkManage");
                if (tblBulkManageCtl != null)
                    tblBulkManageCtl.Visible = true;
            }
        }

        ucGroupReview.RegistrationViewEvent += new UserControls_GroupReview.RegistrationViewEventHandler(ucGroupReview_RegistrationViewEvent);
        pnlReturn.Visible = SearchIds();
     }

    void ucGroupReview_RegistrationViewEvent(int registrationId)
    {
        //TODO: EDV How to set RegistrationIDSlected for view 
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        if (SearchIds())
        {
            SessionVarRetriever.ReturnToDashboard = false;
            Response.Redirect("~/Default.aspx");
            return;
        }

        pnlReturn.Visible = SearchIds();
        SetTitle();
    }

    private void ucErrorProcess_ReturnEvent(bool refreshGrid)
    {
        btnReturn_Click(new object(), new EventArgs());
    }

    protected void btnReturnToGroupAffiliations_Click(object sender, EventArgs e)
    {
        // TODO: EDV What functionality is this??
        //Response.Redirect("Registration.aspx?Step=4");
    }
}