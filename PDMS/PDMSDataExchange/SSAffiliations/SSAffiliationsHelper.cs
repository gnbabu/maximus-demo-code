using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS.SSAffiliations
{
    public static class SSAffiliationsHelper
    {
        public static void ProcessSSAffiliationsRecords(Logging log, Guid threadId)
        {
            try
            {
                DataAccess.ExecuteStoredProcedure("usp_InsertWaiverAffiliations_New", "InsertWaiverAffiliations");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("SSAffiliations Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }
    }
}
