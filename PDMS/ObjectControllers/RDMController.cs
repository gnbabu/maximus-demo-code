using MAXIMUS.Core.Libraries;
using MAXIMUS.Services.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;

namespace MAXIMUS.Controllers.PDMS
{
    public static class RDMController
    {


        public static void ApproveRDMCodeSetsByName(string codeSetName, bool isApproved, Guid userID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
            parameters.Add(SqlParms.CreateParameter("IsApproved", DbType.Boolean, isApproved, true));
            parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userID, true));
            DataAccess.ExecuteStoredProcedure("usp_ApproveRDMCodeSetReview", parameters);

        }
        public static DataSet SelectRDMCodeSetstatusByName(string codeSetName)
        {
            try
            {

                DataSet lookup = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectRDMCodeSetstatus", parameters, "RDMCodesets");

                lookup.Tables[0].TableName = "RDMCodeSets";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int SelectRDMCodeSetChangeCount(string codeSetName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("tableName", DbType.String, codeSetName, true));
                int changeCnt = Convert.ToInt32(DataAccess.ExecuteScalar("usp_ReviewAndUpdateRDMCodeSetCount", parameters));
                
                return changeCnt;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
