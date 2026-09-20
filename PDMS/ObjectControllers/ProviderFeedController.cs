using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.Controllers.PDMS
{
    public static class ProviderFeedController
    {
        public static DataSet GetProviderFeed(int regID)
        {
            DataSet ds = new DataSet();

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("REGID", DbType.Int32, regID, false));
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_Provider_Feed", parameters, "ProviderNotes");
            //ds = 
            return ds;
        }

        public static DataSet GetHistoricNotes(int regID)
        {
            // DataSet ds = ProviderFeedController.GetProviderFeed();
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("REGID", DbType.Int32, regID, false));
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectProvider_Notes", parameters,"ProviderNotes");


            return ds;

        }
    }
}
