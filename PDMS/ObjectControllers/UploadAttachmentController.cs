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
    public static class UploadAttachmentController
    {


        public static DataSet GetUploadAttachmentStatus(string memberid, string providerid, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet ds;
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("memberid", DbType.String, memberid, true));
                parameters.Add(SqlParms.CreateParameter("providerid", DbType.String, providerid, true));
                parameters.Add(SqlParms.CreateParameter("PageSize", DbType.Int32, pageSize, true));
                parameters.Add(SqlParms.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, true));
                parameters.Add(SqlParms.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_Get_Upload_Attachment_Status", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);

                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
