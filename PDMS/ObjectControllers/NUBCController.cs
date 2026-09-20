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
    public static class NUBCController
    {
        public static void ApproveNUBCCodeSetsByName(string codeSetName, bool isApproved)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
            parameters.Add(SqlParms.CreateParameter("IsApproved", DbType.Boolean, isApproved, true));
            DataAccess.ExecuteStoredProcedure("usp_Approve_NUBC_CodeSet_Review", parameters);

        }
        public static DataSet SelectNUBCCodeSetStatusByName(string codeSetName)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_NUBC_CodeSet_Status", parameters, "CMSCodesets");

                lookup.Tables[0].TableName = "NUBCCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int SelectNUBCCodeSetChangeCount(string codeSetName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                int changeCnt = Convert.ToInt32(DataAccess.ExecuteScalar("usp_ReviewAndUpdate_NUBC_CodeSet_Count", parameters));

                return changeCnt;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}