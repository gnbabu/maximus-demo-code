using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.Controllers.PDMS
{
    public static class PartyController
    {
        // db table constants
        public const string dboPerson = "Person";
        public const string dboPersonAddress = "PersonAddress";
        public const string dboPersonEmail = "PersonEmail";
        public const string dboPersonNumber = "PersonNumber";
        public const string dboPersonOrganization = "PersonOrganization";
        public const string dboPersonTelecomNumber = "PersonTelecomNumber";

        // db relationship constants
        private const string dbrPerson2Address = dboPerson + "2" + dboPersonAddress;
        private const string dbrPerson2Email = dboPerson + "2" + dboPersonEmail;
        private const string dbrPerson2Number = dboPerson + "2" + dboPersonNumber;
        private const string dbrPerson2Organization = dboPerson + "2" + dboPersonOrganization;
        private const string dbrPerson2TelecomNumber = dboPerson + "2" + dboPersonTelecomNumber;

        // next roster id
        //static int dbkSubmitRosterId = 0;
        //static int dbkReturnRosterId = 0;

        // db field constants
        private const string dbfPersonId = dboPerson + Constants.id;
        public const string dbfPersonAddressId = "PostalAddressId";
        private const string dbfPersonEmailId = dboPersonEmail + Constants.id;
        private const string dbfPersonNumberId = dboPersonNumber + Constants.id;
        private const string dbfPersonOrganizationId = dboPersonOrganization + Constants.id;
        private const string dbfPersonTelecomNumberId = dboPersonTelecomNumber + Constants.id;
        public const string dbfAddressTypeId = "AddressTypeId";
        public const string dbfContactMechanismRoleId = "ContactMechanismRoleId";

        public static DataSet SelectDashboardStatistics()
        {
            DataSet statistics = new DataSet();
            statistics = DataAccess.ExecuteStoredProcedure("usp_DashboardStatistics");
            statistics.Tables[0].TableName = "DashboardStatistics";

            return statistics;
        }

        public static DataSet SelectDashboardProviderSummary(string roleName)
        {
            DataSet statistics = new DataSet();
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(SqlParms.CreateParameter("RoleName", DbType.String, roleName, false));
            statistics = DataAccess.ExecuteStoredProcedure("usp_Dashboard_Provider_Summary", parms, "DashboardSummary");
            statistics.Tables[0].TableName = "DashboardProviderSummary";

            return statistics;
        }

        public static DataSet SelectDashboardStatisticsGroups(int workFlowId, int providerCategoryTypeId, int workflowEventTypeId, int applicationTypeId = 0, int waiverTypeId = 0, int dashboardTableId = 0)
        {
            // mmisProviderTypIds is a comma delimited list
            // applicationTypeIds is a comma delimited list
            DataSet statistics = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("WORKFLOW_ID", DbType.Int32, workFlowId, false));
            parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, providerCategoryTypeId, false));
            parameters.Add(SqlParms.CreateParameter("WORKFLOW_EVENT_TYPE_ID", DbType.Int32, workflowEventTypeId, false));
            parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, applicationTypeId, true));
            parameters.Add(SqlParms.CreateParameter("WAIVER_TYPE_ID", DbType.Int32, waiverTypeId, true));
            parameters.Add(SqlParms.CreateParameter("DASHBOARD_TABLE_ID", DbType.Int32, dashboardTableId, true));

            return DataAccess.ExecuteStoredProcedure("usp_DashboardStatisticsGroups", parameters, "DashboardStatistics");
        }

        public static DataSet SelectDashboardStatisticsGroups_Services(int workFlowId, int providerCategoryTypeId)
        {

            DataSet statistics = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("WORKFLOW_ID", DbType.Int32, workFlowId, false));
            parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, providerCategoryTypeId, false));
            return DataAccess.ExecuteStoredProcedure("usp_DashboardStatisticsGroups_Services", parameters, "DashboardStatistics");
        }

        public static DataSet SelectDashboardStatisticsGroups_forGroupMemberProfile()
        {

            DataSet statistics = new DataSet();
            statistics = DataAccess.ExecuteStoredProcedure("usp_GetGroupMemberProfile_ForDashboards");
            statistics.Tables[0].TableName = "DashboardStatistics";
            return statistics;
        }

        public static DataSet SelectDashboardStatisticsGroupTotals(int providerCategoryTypeId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, providerCategoryTypeId, false));
            return DataAccess.ExecuteStoredProcedure("usp_DashboardStatisticsGroupTotals", parameters, "DashboardGroupTotals");
        }

        public static DataSet SelectPartyErrorHistory(int errorId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ERROR_ID", DbType.Int32, errorId, false));
                return DataAccess.ExecuteStoredProcedure("sp_selectPARTY_ERROR_HISTORY", parameters, ("Error" + Constants.pluralEnding));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static List<string> GetMedicaidIdsByMedicaidId(string medicaidId)
        {
            List<string> medicaidIds = new List<string>();

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("medicaidId", DbType.String, medicaidId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_MedicaidIDsByMedicaidID", parameters, "usp_MedicaidIDsByMedicaidID");
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["MedicaidID"] != null)
                                medicaidIds.Add(row["MedicaidID"].ToString());

                        }
                    }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

            return medicaidIds;
        }


        public static List<string> GetMedicaidIdsByUserName(string username)
        {
            List<string> medicaidIds = new List<string>();

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("username", DbType.String, username, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetMedicaidIdByUserName", parameters, "usp_GetMedicaidIdByUserName");
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["MedicaidID"] != null)
                                medicaidIds.Add(row["MedicaidID"].ToString());

                        }
                    }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

            return medicaidIds;
        }

    }
}
