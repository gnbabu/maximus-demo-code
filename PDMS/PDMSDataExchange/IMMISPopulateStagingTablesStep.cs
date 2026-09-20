using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.DataExchange.PDMS
{
	public interface IMMISPopulateStagingTablesStep
	{
		void PopulateStagingTables(int transactionTypeID);
	}

	public class MMISPopulateIndividualStagingTables : MMISTransactionStep, IMMISPopulateStagingTablesStep
	{
		#region Constructors

		public MMISPopulateIndividualStagingTables(Guid threadID)
			: base(threadID)
		{

		}

		#endregion


		public void PopulateStagingTables(int transactionTypeID)
		{
			// add records to staging tables
			List<SqlParameter> parameters = new List<SqlParameter>();
			parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
			parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
			DataAccess.ExecuteStoredProcedure("usp_AddPDMSToMMISExportRecords", parameters);
		}
	}

    public class MMISPopulatesWaiverServiceStagingTables : MMISTransactionStep, IMMISPopulateStagingTablesStep
    {
        #region Constructors

        public MMISPopulatesWaiverServiceStagingTables(Guid threadID)
            : base(threadID)
        {

        }

        #endregion


        public void PopulateStagingTables(int transactionTypeID)
        {
            // add records to staging tables
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
            DataAccess.ExecuteStoredProcedure("usp_InsertMMISStagingWaiverServices", parameters);
        }
    }
    
    public class MMISPopulateGroupStagingTables : MMISTransactionStep, IMMISPopulateStagingTablesStep
	{
		#region Constructors

		public MMISPopulateGroupStagingTables(Guid threadID)
			: base(threadID)
		{

		}

		#endregion


		public void PopulateStagingTables(int transactionTypeID)
		{
			if(transactionTypeID == Constants.TransactionType.SendDIDDAddAffiliationtoMMIS ||
				transactionTypeID == Constants.TransactionType.SendDCSAddAffiliationtoMMIS ||
				transactionTypeID == Constants.TransactionType.SendDIDDTerminateAffiliationtoMMIS ||
				transactionTypeID == Constants.TransactionType.SendDCSTerminateAffiliationtoMMIS)
			{
				// add records to staging tables
				List<SqlParameter> parameters = new List<SqlParameter>();

				parameters.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, transactionTypeID, true));
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

				DataAccess.ExecuteStoredProcedure("usp_InsertMMISStagingEligibilityGroups", parameters);
			}
			else
			{
				// add records to staging tables
				List<SqlParameter> parameters = new List<SqlParameter>();

				parameters.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, transactionTypeID, true));
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

				DataAccess.ExecuteStoredProcedure("usp_InsertMMISStagingGroups", parameters);
			}
		}
	}
}
