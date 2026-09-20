using System;
using System.Collections.Generic;
using Corp.Core.Libraries;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAXIMUS.Core.Libraries;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.DataExchange.PDMS.PNM
{
    public class PMFShared 
    {
        public static int PMFFullExtractJobCheckUpdate(int EXTRACT_ID, string JOB_TYPE, string TYPE, int JOB_ID)
        {
            int returnVal = 0;

            try
            {             
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("EXTRACT_ID", DbType.Int32, EXTRACT_ID, true));
                parameters.Add(SqlHelper.CreateParameter("JOB_TYPE", DbType.String, JOB_TYPE, true));
                parameters.Add(SqlHelper.CreateParameter("TYPE", DbType.String, TYPE, true));
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.String, JOB_ID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_CheckUpdatePMFExtractJob", parameters));                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public static int InsertPMFJobsSummary(string JOB_TYPE, string STATUS, int @JOB_ID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_TYPE", DbType.String, JOB_TYPE, true));
                parameters.Add(SqlHelper.CreateParameter("STATUS", DbType.String, STATUS, true));
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, JOB_ID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertPMFJobsSummary", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
    }
}
