using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace StateSingleSignOn.App_Code
{
    public class SessionVarRetrieverSS
    {
        private SessionVarRetrieverSS()
        {

        }
        #region privateHelper
        /// <summary>
        /// Used to retrieve the Session for the current user
        /// </summary>
        private static System.Web.SessionState.HttpSessionState Session
        {
            get
            {
                return System.Web.HttpContext.Current.Session;
            }
        }
        #endregion

        #region Control Items

        public static void AddControlInitialized(string controlName, Guid objectId, bool value)
        {
            String SessionKey = controlName + "_" + objectId.ToString() + "_IsInitialized";
            HttpContext.Current.Session[SessionKey] = value;
        }

        public static bool ControlInitialized(string controlName, Guid objectId)
        {
            bool result = false;
            String SessionKey = controlName + "_" + objectId.ToString() + "_IsInitialized";
            if (HttpContext.Current.Session[SessionKey] != null)
            {
                result = (bool)HttpContext.Current.Session[SessionKey];
            }
            return result;
        }

        public static void AddSortDirection(string controlName, string sortExpression, SortDirection sortDirection)
        {
            String SessionKey = controlName + "_" + sortExpression;
            HttpContext.Current.Session[SessionKey] = sortDirection;
        }

        public static SortDirection PreviousSortDirection(string controlName, string sortExpression)
        {
            String SessionKey = controlName + "_" + sortExpression;
            if (HttpContext.Current.Session[SessionKey] == null)
                return SortDirection.Descending;
            else
                return (SortDirection)HttpContext.Current.Session[SessionKey];
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public static void ClearRegistrationSessionVars()
        {

            //Clear out any existing group user related session vars.
            Session["IndividualRegID"] = null;
            Session["IsGroup"] = null;
            Session["GroupUserRegID"] = null;
        }

        public static void ClearIsInitialized()
        {
            int i;
            string key;

            for (i = Session.Keys.Count - 1; i >= 0; i--)
            {
                key = Session.Keys[i];
                if (key.Contains("IsInitialized"))
                {
                    Session.Remove(key);
                }
            }
        }
        #endregion

        #region Dashboard
        public static DataTable ProcessingList1
        {
            get
            {
                if (Session["PDMSProcessingList1"] == null) Session["PDMSProcessingList1"] = new DataTable();
                return (DataTable)Session["PDMSProcessingList1"];
            }
            set { Session["PDMSProcessingList1"] = value; }
        }

        public class DashboardTable
        {
            public int TableId { get; set; }
            public DataTable DashboardData { get; set; }
        }
        public class DashboardTotal
        {
            public List<DashboardTable> TotalTables { get; set; }

            public DataTable ProviderSummary { get; set; }



            public DashboardTotal()
            {
                TotalTables = new List<DashboardTable>();
                ProviderSummary = new DataTable();
            }
        }


        public static DataTable GetDashBoardTotals(int tableId)
        {

            if (Session["DashBoardTotalsList"] == null) Session["DashBoardTotalsList"] = new DashboardTotal();
            DashboardTotal dashboardTotal = (DashboardTotal)Session["DashBoardTotalsList"];
            DashboardTable dashboardTable = dashboardTotal.TotalTables.Where(t => t.TableId == tableId).FirstOrDefault();
            if (dashboardTable == null)
            {
                dashboardTable = new DashboardTable
                {
                    TableId = tableId,
                    DashboardData = new DataTable()
                };
                dashboardTotal.TotalTables.Add(dashboardTable);
                Session["DashBoardTotalsList"] = dashboardTotal;
            }

            return dashboardTable.DashboardData;

        }

        public static void SetDashBoardTotals(int tableId, DataTable data)
        {

            if (Session["DashBoardTotalsList"] == null) Session["DashBoardTotalsList"] = new DashboardTotal();
            DashboardTotal dashboardTotal = (DashboardTotal)Session["DashBoardTotalsList"];
            DashboardTable dashboardTable = dashboardTotal.TotalTables.Where(t => t.TableId == tableId).FirstOrDefault();
            if (dashboardTable == null)
            {
                dashboardTable = new DashboardTable
                {
                    TableId = tableId,
                };
                dashboardTotal.TotalTables.Add(dashboardTable);

            }
            dashboardTable.DashboardData = data;
            Session["DashBoardTotalsList"] = dashboardTotal;
            return;

        }

        public static DataTable GetDashBoardTotalIdList(int tableId)
        {

            if (Session["DashBoardTotalIdList"] == null) Session["DashBoardTotalIdList"] = new DashboardTotal();
            DashboardTotal dashboardTotal = (DashboardTotal)Session["DashBoardTotalIdList"];
            DashboardTable dashboardTable = dashboardTotal.TotalTables.Where(t => t.TableId == tableId).FirstOrDefault();
            if (dashboardTable == null)
            {
                dashboardTable = new DashboardTable
                {
                    TableId = tableId,
                    DashboardData = new DataTable()
                };
                dashboardTotal.TotalTables.Add(dashboardTable);
                Session["DashBoardTotalIdList"] = dashboardTotal;
            }

            return dashboardTable.DashboardData;

        }

        public static void SetDashBoardTotalIdList(int tableId, DataTable data)
        {

            if (Session["DashBoardTotalIdList"] == null) Session["DashBoardTotalIdList"] = new DashboardTotal();
            DashboardTotal dashboardTotal = (DashboardTotal)Session["DashBoardTotalIdList"];
            DashboardTable dashboardTable = dashboardTotal.TotalTables.Where(t => t.TableId == tableId).FirstOrDefault();
            if (dashboardTable == null)
            {
                dashboardTable = new DashboardTable
                {
                    TableId = tableId,
                };
                dashboardTotal.TotalTables.Add(dashboardTable);

            }
            dashboardTable.DashboardData = data;
            Session["DashBoardTotalIdList"] = dashboardTotal;
            return;

        }



        public static DashboardTotal DashBoardProviderSummary
        {
            get
            {
                if (Session["DashBoardProviderSummary"] == null) Session["DashBoardProviderSummary"] = new DashboardTotal();
                return (DashboardTotal)Session["DashBoardProviderSummary"];
            }
            set { Session["DashBoardProviderSummary"] = value; }
        }

        public static bool ReturnToDashboard
        {
            get
            {
                if (Session["ReturnToDashboard"] == null) Session["ReturnToDashboard"] = false;
                return (bool)Session["ReturnToDashboard"];
            }
            set { Session["ReturnToDashboard"] = value; }
        }

        public static bool DashBoardHasValues(int opt)
        {
            switch (opt)
            {
                case 1:
                    if (!string.IsNullOrEmpty(DashBoardPartyIds) || !string.IsNullOrEmpty(DashBoardSubmitRosterIds) ||
                        SessionVarRetrieverSS.DashBoardTableId > 0) return true;
                    break;
                case 2:
                    if (!string.IsNullOrEmpty(DashBoardErrorIds) || SessionVarRetrieverSS.DashBoardTableId > 0) return true;
                    break;
                case 3:
                    return ReturnToDashboard;
            }
            return false;
        }

        public static string DashBoardPartyIds
        {
            get
            {
                if (Session["PDMSDashBoardPartyIds"] == null) Session["PDMSDashBoardPartyIds"] = string.Empty;
                return Session["PDMSDashBoardPartyIds"].ToString();
            }
            set { Session["PDMSDashBoardPartyIds"] = value; }
        }

        public static string DashBoardSubmitRosterIds
        {
            get
            {
                if (Session["PDMSDashBoardSubmitRosterIds"] == null) Session["PDMSDashBoardSubmitRosterIds"] = string.Empty;
                return Session["PDMSDashBoardSubmitRosterIds"].ToString();
            }
            set { Session["PDMSDashBoardSubmitRosterIds"] = value; }
        }

        public static string DashBoardErrorIds
        {
            get
            {
                if (Session["PDMSDashBoardErrorIds"] == null) Session["PDMSDashBoardErrorIds"] = string.Empty;
                return Session["PDMSDashBoardErrorIds"].ToString();
            }
            set { Session["PDMSDashBoardErrorIds"] = value; }
        }

        public static string DashBoardRegistrationIds
        {
            get
            {
                if (Session["PDMSDashBoardRegistrationIds"] == null) Session["PDMSDashBoardRegistrationIds"] = string.Empty;
                return Session["PDMSDashBoardRegistrationIds"].ToString();
            }
            set { Session["PDMSDashBoardRegistrationIds"] = value; }
        }

        public static string DashBoardReferralIds
        {
            get
            {
                if (Session["PDMSDashBoardReferralIds"] == null) Session["PDMSDashBoardReferralIds"] = string.Empty;
                return Session["PDMSDashBoardReferralIds"].ToString();
            }
            set { Session["PDMSDashBoardReferralIds"] = value; }
        }

        public static int DashBoardTableId
        {
            get
            {
                if (Session["DashBoardTableId"] == null) Session["DashBoardTableId"] = 0;
                return (int)Session["DashBoardTableId"];
            }
            set { Session["DashBoardTableId"] = value; }
        }

        public static int DashBoardStatusID
        {
            get
            {
                if (Session["DashBoardStatusID"] == null) Session["DashBoardStatusID"] = 0;
                return (int)Session["DashBoardStatusID"];
            }
            set { Session["DashBoardStatusID"] = value; }
        }

        public static int DashBoardOrdinal
        {
            get
            {
                if (Session["DashBoardOrdinal"] == null) Session["DashBoardOrdinal"] = 0;
                return (int)Session["DashBoardOrdinal"];
            }
            set { Session["DashBoardOrdinal"] = value; }
        }

        public static bool DashBoardIsAssigned
        {
            get
            {
                if (Session["PDMSDashBoardIsAssigned"] == null) Session["PDMSDashBoardIsAssigned"] = string.Empty;
                return Convert.ToBoolean(Session["PDMSDashBoardIsAssigned"].ToString());
            }
            set { Session["PDMSDashBoardIsAssigned"] = value; }
        }

        #endregion

        public static string UserIdSelected
        {
            get
            {
                if (Session["UserIdSelected"] == null) Session["UserIdSelected"] = string.Empty;
                return Session["UserIdSelected"].ToString();
            }
            set { Session["UserIdSelected"] = value; }
        }

        public static Object MyQueueItemsList
        {
            get
            {
                if (Session["MyQueueItemsList"] == null) Session["MyQueueItemsList"] = 0;
                return (Object)Session["MyQueueItemsList"];
            }
            set { Session["MyQueueItemsList"] = value; }
        }

        public static Object MySiteItemsList
        {
            get
            {
                if (Session["MySiteItemsList"] == null) Session["MySiteItemsList"] = 0;
                return (Object)Session["MySiteItemsList"];
            }
            set { Session["MySiteItemsList"] = value; }
        }

        public static string MyQueueSelectedRoleName
        {
            get
            {
                if (Session["MyQueueSelectedRoleName"] == null) Session["MyQueueSelectedRoleName"] = "";
                return (string)Session["MyQueueSelectedRoleName"];
            }
            set { Session["MyQueueSelectedRoleName"] = value; }
        }

        public static string MyQueueSelectedRoleValue
        {
            get
            {
                if (Session["MyQueueSelectedRoleValue"] == null) Session["MyQueueSelectedRoleValue"] = "";
                return (string)Session["MyQueueSelectedRoleValue"];
            }
            set { Session["MyQueueSelectedRoleValue"] = value; }
        }
        //This one right now only used to load individual registration from group registration to update information.
        public static int IndividualRegID
        {
            get
            {
                if (Session["IndividualRegID"] == null) Session["IndividualRegID"] = 0;
                return (int)Session["IndividualRegID"];
            }
            set { Session["IndividualRegID"] = value; }
        }
        public static int GroupUserRegID
        {
            get
            {
                if (Session["GroupUserRegID"] == null) Session["GroupUserRegID"] = 0;
                return (int)Session["GroupUserRegID"];
            }
            set { Session["GroupUserRegID"] = value; }
        }
        public static bool IsGroup
        {
            get
            {
                if (Session["IsGroup"] == null) Session["IsGroup"] = false;
                return (bool)Session["IsGroup"];
            }
            set { Session["IsGroup"] = value; }
        }
        public static bool PasswordExpiryMsgShown
        {
            get
            {
                if (Session["PasswordExpiryMsgShown"] == null) Session["PasswordExpiryMsgShown"] = false;
                return Convert.ToBoolean(Session["PasswordExpiryMsgShown"].ToString());
            }
            set { Session["PasswordExpiryMsgShown"] = value; }
        }

        public static List<int> BulkEmailProviderIds
        {
            get
            {
                if (Session["BulkEmailProviderIds"] == null) Session["BulkEmailProviderIds"] = new List<int>();
                return (List<int>)Session["BulkEmailProviderIds"];
            }
            set { Session["BulkEmailProviderIds"] = value; }
        }

        public static bool IsRealEstateOwner
        {
            get
            {
                if (Session["IsRealEstateOwner"] == null) Session["IsRealEstateOwner"] = false;
                return Convert.ToBoolean(Session["IsRealEstateOwner"].ToString());
            }
            set { Session["IsRealEstateOwner"] = value; }
        }


        public static bool IsAdditionalDisclosure
        {
            get
            {
                if (Session["IsAdditionalDisclosure"] == null) Session["IsAdditionalDisclosure"] = false;
                return Convert.ToBoolean(Session["IsAdditionalDisclosure"].ToString());
            }
            set { Session["IsAdditionalDisclosure"] = value; }
        }

        public static bool IsNewOwner
        {
            get
            {
                if (Session["IsNewOwner"] == null) Session["IsNewOwner"] = false;
                return Convert.ToBoolean(Session["IsNewOwner"].ToString());
            }
            set { Session["IsNewOwner"] = value; }
        }

        public static int WorkFlowRegID
        {
            get
            {
                if (Session["WorkFlowRegID"] == null) Session["WorkFlowRegID"] = 0;
                return (int)Session["WorkFlowRegID"];
            }
            set { Session["WorkFlowRegID"] = value; }
        }
        public static bool IsOhID
        {
            get
            {
                if (Session["IsOhID"] == null) Session["IsOhID"] = false;
                return (bool)Session["IsOhID"];
            }
            set { Session["IsOhID"] = value; }
        }

        public static string OhID
        {
            get
            {
                if (Session["OhID"] == null) Session["OhID"] = "";
                return (string)Session["OhID"];
            }
            set { Session["OhID"] = value; }
        }

        public static string UserName
        {
            get
            {
                if (Session["UserName"] == null) Session["UserName"] = "";
                return (string)Session["UserName"];
            }
            set { Session["UserName"] = value; }
        }
        public static bool IsNewUser
        {
            get
            {
                if (Session["IsNewUser"] == null) Session["IsNewUser"] = false;
                return (bool)Session["IsNewUser"];
            }
            set { Session["IsNewUser"] = value; }
        }
        public static bool IsManualClosureEmail
        {
            get
            {
                if (Session["IsManualClosureEmail"] == null) Session["IsManualClosureEmail"] = false;
                return (bool)Session["IsManualClosureEmail"];
            }
            set { Session["IsManualClosureEmail"] = value; }
        }
        public static string DisplayUserName
        {
            get
            {
                if (Session["DisplayUserName"] == null) Session["DisplayUserName"] = "";
                return (string)Session["DisplayUserName"];
            }
            set { Session["DisplayUserName"] = value; }
        }
    }
}