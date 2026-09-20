using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.Controllers.PDMS
{
    public static class WPCController
    {
        public static void ApproveWPCCodeSetsByName(string codeSetName, bool isApproved)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
            parameters.Add(SqlParms.CreateParameter("IsApproved", DbType.Boolean, isApproved, true));
            DataAccess.ExecuteStoredProcedure("usp_Approve_WPC_CodeSet_Review", parameters);

        }
        public static DataSet SelectWPCCodeSetStatusByName(string codeSetName)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_WPC_CodeSet_Status", parameters, "WPCCodesets");

                lookup.Tables[0].TableName = "WPCCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int SelectWPCCodeSetChangeCount(string codeSetName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                int changeCnt = Convert.ToInt32(DataAccess.ExecuteScalar("usp_ReviewAndUpdate_WPC_CodeSet_Count", parameters));

                return changeCnt;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
