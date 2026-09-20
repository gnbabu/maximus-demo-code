using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MCPN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS
{
    public class MultipleControlIdDeletion : BaseJob, IJob
    {
        public MultipleControlIdDeletion(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }
        override public void ExecuteJob()
        {
            // Default Job - B2P Retrieve Payment Information
            this.ExecuteJob(Guid.Parse("3ACC7F90-453D-454D-B1A1-D13001C59745"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                // MMIS Retrieve Payment Information
                case "3ACC7F90-453D-454D-B1A1-D13001C59745":
                    DeleteMultipleControlID();
                    break;
            }
        }
        public static void DeleteMultipleControlID()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet dsMembers = new DataSet();
                dsMembers = DataAccess.ExecuteStoredProcedure("usp_DeletingMultipleID");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}