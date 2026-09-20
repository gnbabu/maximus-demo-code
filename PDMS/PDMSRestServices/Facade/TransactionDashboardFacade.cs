using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using PDMSRestServices.Models;
using System.Data;
using System.Data.SqlClient;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSRestServices.Facade
{
    public class TransactionDashboardFacade
    {
        public static List<TransactionDTO> GetTransactionMonitoringData(string serviceType, string operation, string timeframe)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("ServiceType", string.IsNullOrEmpty(serviceType) ? DBNull.Value : (object)serviceType),
        new SqlParameter("Operation", string.IsNullOrEmpty(operation) ? DBNull.Value : (object)operation),
        new SqlParameter("TimeFrame", string.IsNullOrEmpty(timeframe) ? DBNull.Value : (object)timeframe)
    };

            var ds = DataAccess.ExecuteStoredProcedure("[dbo].[usp_GetTransaction_Monitoring_Data]", parameters, "TransactionMonitoringData");
            var transactionList = new List<TransactionDTO>();

            if (ds?.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    transactionList.Add(new TransactionDTO
                    {
                        TransactionKey = row["SI_TRANSACTION_KEY"] != DBNull.Value ? row["SI_TRANSACTION_KEY"].ToString() : null,
                        RequestType = row["REQUEST_TYPE"] != DBNull.Value ? row["REQUEST_TYPE"].ToString() : null,
                        RequestTimestamp = row["REQUEST_TIMESTAMP"] != DBNull.Value ? Convert.ToDateTime(row["REQUEST_TIMESTAMP"]) : (DateTime?)null,
                        ResponseTimestamp = row["RESPONSE_TIMESTAMP"] != DBNull.Value ? Convert.ToDateTime(row["RESPONSE_TIMESTAMP"]) : (DateTime?)null,
                        DurationMs = row["DURATION_MS"] != DBNull.Value ? Convert.ToInt32(row["DURATION_MS"]) : (int?)null
                    });
                }
            }

            return transactionList;
        }

    }
}
