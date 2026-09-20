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
    public static class C4Controller
    {
        public static void ApproveC4CodeSetsByName(string codeSetName, bool isApproved)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
            parameters.Add(SqlParms.CreateParameter("IsApproved", DbType.Boolean, isApproved, true));
            DataAccess.ExecuteStoredProcedure("usp_Approve_C4_CodeSet_Review", parameters);

        }
        public static DataSet SelectC4CodeSetStatusByName(string codeSetName)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_C4_CodeSet_Status", parameters, "C4Codesets");

                lookup.Tables[0].TableName = "C4CodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int SelectC4CodeSetChangeCount(string codeSetName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                int changeCnt = Convert.ToInt32(DataAccess.ExecuteScalar("usp_ReviewAndUpdate_C4_CodeSet_Count", parameters));

                return changeCnt;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}