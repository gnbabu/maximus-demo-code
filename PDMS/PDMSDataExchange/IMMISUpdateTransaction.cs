using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MAXIMUS.DataExchange.PDMS
{
	public interface IMMISUpdateTransaction
	{
		void UpdateTransaction(TransactionData transactionData, DataRow row, DateTime now);
	}
}

namespace MAXIMUS.DataExchange.PDMS.MMISUpdateTransaction
{
	public class ByTransactionIDSubmitted : MMISTransactionStep, IMMISUpdateTransaction
	{
		#region Constructors

		public ByTransactionIDSubmitted(Guid threadID)
			: base(threadID)
		{

		}

		#endregion
		public void UpdateTransaction(TransactionData transactionData, DataRow row, DateTime now)
		{
			if (transactionData.TransactionQueueID.HasValue)
			{
				TransactionController.UpdateTransactionQueue(
					transactionData.TransactionQueueID.Value
					, now
					, null
					, now
					, Constants.appPDMSDataExchangeUserId);
			}
			else
			{
				throw new Exception("Transaction Queue ID cannot be NULL.");
			}
		}
	}

	public class ByMMISStagingGroupPKSubmitted : MMISTransactionStep, IMMISUpdateTransaction
	{
		#region Constructors

		public ByMMISStagingGroupPKSubmitted(Guid threadID)
			: base(threadID)
		{
		
		}

		#endregion
		public void UpdateTransaction(TransactionData transactionData, DataRow row, DateTime now)
		{
			TransactionController.UpdateTransactionQueueByStagingGroupPK(
				transactionData.PrimaryKey
				, now
				, null
				, now
				, Constants.appPDMSDataExchangeUserId);

			
		}
	}

	public class ByMMISStagingGroupPKProcessed : MMISTransactionStep, IMMISUpdateTransaction
	{
		#region Private Members

		private List<int> _usedTransactionIDs = new List<int>();

		#endregion

		#region Constructors

		public ByMMISStagingGroupPKProcessed(Guid threadID)
			: base(threadID)
		{
			_usedTransactionIDs = new List<int>();
		}

		#endregion
		public void UpdateTransaction(TransactionData transactionData, DataRow row, DateTime now)
		{
			TransactionController.UpdateTransactionQueueByStagingGroupPK(
				transactionData.PrimaryKey
				, null
				, now
				, now
				, Constants.appPDMSDataExchangeUserId);
		}
	}

	public class ByTransactionIDProcessed : MMISTransactionStep, IMMISUpdateTransaction
	{
		#region Constructors

		public ByTransactionIDProcessed(Guid threadID)
			: base(threadID)
		{

		}

		#endregion
		public void UpdateTransaction(TransactionData transactionData, DataRow row, DateTime now)
		{
			if (transactionData.TransactionQueueID.HasValue)
			{
				TransactionController.UpdateTransactionQueue(
								   transactionData.TransactionQueueID.Value
								   , null
								   , now
								   , now
								   , Constants.appPDMSDataExchangeUserId);
			}
			else
			{
				throw new Exception("Transaction Queue ID cannot be NULL.");
			}
		}
	}

	public class ByPartyIDSubmitted : MMISTransactionStep, IMMISUpdateTransaction
	{
		#region Constructors

		public ByPartyIDSubmitted(Guid threadID)
			: base(threadID)
		{

		}

		#endregion
		public void UpdateTransaction(TransactionData transactionData, DataRow row, DateTime now)
		{
			TransactionController.UpdateTransactionQueue(
                          transactionData.TransactionTypeID
						  , transactionData.PartyID
						  , transactionData.MedicaidPK
						  , now
						  , null
						  , now
						  , Constants.appPDMSDataExchangeUserId);

		}
	}

	public class ByPartyIDProcessed : MMISTransactionStep, IMMISUpdateTransaction
	{
		#region Constructors

		public ByPartyIDProcessed(Guid threadID)
			: base(threadID)
		{

		}

		#endregion
		public void UpdateTransaction(TransactionData transactionData, DataRow row, DateTime now)
		{
			TransactionController.UpdateTransactionQueue(
								transactionData.TransactionTypeID
								, transactionData.PartyID
								, transactionData.MedicaidPK
								, null
								, now
								, now
								, Constants.appPDMSDataExchangeUserId);
		}
	}

	public class ForEachAffiliationSubmitted : MMISTransactionStep, IMMISUpdateTransaction
	{
		#region Constructors

		public ForEachAffiliationSubmitted(Guid threadID)
			: base(threadID)
		{

		}

		#endregion
		public void UpdateTransaction(TransactionData transactionData, DataRow row, DateTime now)
		{
			// update all transactions for each affiliation
			foreach (DataRow affiliate in row.GetChildRows(MMISShared.dbrGrps2dbtAffs))
			{
				// Update TRANSACTION table with Submit Date information
				// -----------------------------------------------------
				TransactionController.UpdateTransactionQueue(
					Convert.ToInt32(transactionData.TransactionTypeID)
					, Methods.GetIntValue(affiliate["PARTY_ID"])
					, Convert.ToInt32(affiliate["MEDICAID_PK"].ToString())
					, now
					, null
					, now
					, Constants.appPDMSDataExchangeUserId);
			}
		}
	}

	public class OneAffiliationPerTransactionSubmitted : MMISTransactionStep, IMMISUpdateTransaction
	{
		#region Constructors

		public OneAffiliationPerTransactionSubmitted(Guid threadID)
			: base(threadID)
		{

		}

		#endregion
		public void UpdateTransaction(TransactionData transactionData, DataRow row, DateTime now)
		{
			var firstAffiliation = row.GetChildRows(MMISShared.dbrGrps2dbtAffs).FirstOrDefault();
			// update all transactions for each affiliation
			if(firstAffiliation != null)
			{
				// Update TRANSACTION table with Submit Date information
				// -----------------------------------------------------
				TransactionController.UpdateTransactionQueue(
					Convert.ToInt32(transactionData.TransactionTypeID)
					, Methods.GetIntValue(firstAffiliation["PARTY_ID"])
					, Convert.ToInt32(firstAffiliation["MEDICAID_PK"].ToString())
					, now
					, null
					, now
					, Constants.appPDMSDataExchangeUserId);
			}
		}
	}


}
