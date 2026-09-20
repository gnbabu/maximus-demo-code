using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml.Schema;
using Corp.Core.Libraries;
using System.Xml;

namespace MAXIMUS.DataExchange.PDMS
{
    internal class ZIPCodeStaleUpdater : BaseJob, IJob
    {
        public ZIPCodeStaleUpdater(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private const string thisGuidString = "7C4F6FAC-985A-4091-A678-A9E64C8AF872";
        private Guid thisGuid = new Guid(thisGuidString);

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(thisGuid);
        }

        override public void ExecuteJob(Guid jobId)
        {
            UpdateStaleZipCodes(jobId);
        }

        private void UpdateStaleZipCodes(Guid jobId)
        {
            String difference = AppSettings.Get("StaleZIPCodeDifference");

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("DIFFERENCE", DbType.Decimal, difference, false));

            DataSet ds = DataAccess.ExecuteStoredProcedure("usp_selectStaleZIPGeographyByDifference", parameters, "StaleZIPGeography");
            if (ds.Tables.Count > 0) {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) {
                        parameters.Clear();
                        parameters.Add(SqlParms.CreateParameter("REG_ADDRESS_ID", DbType.Int64, dr["REG_ADDRESS_ID"], false));
                        parameters.Add(SqlParms.CreateParameter("LATITUDE", DbType.Decimal, dr["LATITUDE"], false));
                        parameters.Add(SqlParms.CreateParameter("LONGITUDE", DbType.Decimal, dr["LONGITUDE"], false));

                        DataAccess.ExecuteScalar("usp_updateStaleZIPGeographyByRegAddressID", parameters);
                    }
                }
            }
        }
    }
}