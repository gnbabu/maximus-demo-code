using System;
using System.Data;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_DIDDProviderSearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            /*SetTitle();*/
            if (Request["TableId"] != null && Request["StatusID"] != null && Request["Ordinal"] != null)
            {
                int count = 0;
                if (Request["Count"] != null) count = Convert.ToInt32(Request["Count"]);
                LoadDashboard(Convert.ToInt32(Request["TableId"]), Convert.ToInt32(Request["StatusID"]),
                    Convert.ToInt32(Request["Ordinal"]), count, Convert.ToBoolean(Request["IsAssigned"]));
            }
            else SessionVarRetriever.DashBoardRegistrationIds = string.Empty;

        }
        ucDIDDProviderSearch.RegistrationViewEvent += ucDIDDProviderSearch_RegistrationViewEvent;
    }


    private void LoadDashboard(int tableId, int statusID, int ordinal, int count, Boolean IsAssigned)
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
            else LoadDashboardRegistration(tableId, dt, statusID, ordinal, ref registrationList, IsAssigned);
        }
        if (!string.IsNullOrEmpty(registrationList)) registrationList = registrationList.Substring(0, registrationList.Length - 1);
        if (statusID == 91 || statusID == 1091)
        {
            SessionVarRetriever.DashBoardReferralIds = registrationList;
            SessionVarRetriever.DashBoardRegistrationIds = "";
        }
        else
        {
            SessionVarRetriever.DashBoardRegistrationIds = registrationList;
            SessionVarRetriever.DashBoardReferralIds = "";
        }
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

    void ucDIDDProviderSearch_RegistrationViewEvent(int registrationId)
    {
        (this.Page as RegistrationProvider).RegistrationId = registrationId;
        (this.Page as RegistrationProvider).IsReadOnly = true;
    }
}