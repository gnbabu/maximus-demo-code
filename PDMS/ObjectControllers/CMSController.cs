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
    public static class CMSController
    {
        public static void ApproveCMSCodeSetsByName(string codeSetName, bool isApproved)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
            parameters.Add(SqlParms.CreateParameter("IsApproved", DbType.Boolean, isApproved, true));
            DataAccess.ExecuteStoredProcedure("usp_Approve_CMS_CodeSet_Review", parameters);

        }
        public static DataSet SelectCMSCodeSetStatusByName(string codeSetName)
        {
            try
            {
                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_Select_CMS_CodeSet_Status", parameters, "CMSCodesets");

                lookup.Tables[0].TableName = "CMSCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int SelectCMSCodeSetChangeCount(string codeSetName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                int changeCnt = Convert.ToInt32(DataAccess.ExecuteScalar("usp_ReviewAndUpdate_CMS_CodeSet_Count", parameters));

                return changeCnt;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}