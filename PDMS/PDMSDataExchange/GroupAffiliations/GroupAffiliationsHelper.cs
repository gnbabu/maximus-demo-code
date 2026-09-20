using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS.GroupAffiliations
{
    public static class GroupAffiliationsHelper
    {
        public static void ProcessGroupAffiliationsRecords(Logging log, Guid threadId)
        {
            try
            {
                DataAccess.ExecuteStoredProcedure("usp_SendGroupTransactions", "SendGroupTransactions");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Group Affiliations Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        /// <summary>
        /// Affiliation which are not in revalidatind window and past revalidation date should change the status from 'Individual Requires Revalidation' to 'Confirmed'
        /// </summary>
        public static void UpdateGroupAffiliationStatus(Logging log, Guid threadId)
        {
            try
            {
                DataAccess.ExecuteStoredProcedure("usp_UpdatetoConfirmedGroupAffiliationStatus", "GroupAffiliationStatus");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Group Affiliation Status Update Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void UpdateGroupAffiliationtoPendingRemovalStatus(Logging log, Guid threadId)
        {
            try
            {
                DataAccess.ExecuteStoredProcedure("usp_UpdatetoPendingRemovalGroupAffiliationStatus", "PendingGroupAffiliationStatus");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Pending Group Affiliation Status Update Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }
    }
}
