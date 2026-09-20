using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS.NUBC
{
    public class NubcHelper
    {
        //Save STG_NUBC_RFD_<TableName> in a transaction
        public static void SaveNubcTable(string tableName, Logging log, string code, int? deleted, int? futureUse, string desc, string additionalInfo, DateTime loadDate, SqlTransaction trans)
        {
            //try
            //{
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CODE", DbType.String, code, true));
                parameters.Add(SqlParms.CreateParameter("DESC", DbType.String, desc, true));
                parameters.Add(SqlParms.CreateParameter("ADDITIONAL", DbType.String, additionalInfo, false));
                parameters.Add(SqlParms.CreateParameter("DELETED", DbType.Int16, deleted,false));
                parameters.Add(SqlParms.CreateParameter("FUTUREUSE", DbType.Int32, futureUse, false));
                parameters.Add(SqlParms.CreateParameter("LOADDATE", DbType.DateTime, loadDate, true));

                var table = "usp_Insert" + tableName;

                DataAccess.ExecuteStoredProcedure(trans, table, parameters);
            //}
            //catch (Exception ex)
            //{
            //    log.CreateLogEntry(String.Format("Error saving {0}: {1}", tableName, ex.StackTrace), Logging.LogPriority.Error);
            //}
        }
    }
}