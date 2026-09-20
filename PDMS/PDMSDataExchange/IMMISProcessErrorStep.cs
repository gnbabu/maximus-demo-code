using MAXIMUS.DataExchange.PDMS.MMISInterface;
using System;

namespace MAXIMUS.DataExchange.PDMS
{
	public interface IMMISProcessErrorStep<TRequest>
        where TRequest : MMISRequest
	{
		void ProcessDataError(TransactionData data, TRequest request, MMISShared ms);
	}
}

namespace MAXIMUS.DataExchange.PDMS.MMISProcessError.GroupRetrieve
{
	public class ProcessError : MMISTransactionStep, IMMISProcessErrorStep<RetrieveWaiverProvider>
	{
		#region Constructors

		public ProcessError(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

        public void ProcessDataError(TransactionData data, RetrieveWaiverProvider request, MMISShared ms)
		{
            string[] errors = request.StatusCode.Split(',');

            foreach (string error in errors)
            {
                string errorCode = 'N' + error;
                ms.ProcessDataErrors(errorCode, data.PartyID.ToString(), data.MedicaidPK.ToString());
            }
        }
    }
}

namespace MAXIMUS.DataExchange.PDMS.MMISProcessError.IndividualRetrieve
{
	public class ProcessError : MMISTransactionStep, IMMISProcessErrorStep<RetrieveWaiverProvider>
	{
		#region Constructors

		public ProcessError(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

        public void ProcessDataError(TransactionData data, RetrieveWaiverProvider request, MMISShared ms)
		{
            string[] errors = request.StatusCode.Split(',');

            foreach (string error in errors)
            {
                string errorCode = 'N' + error;
                    
                ms.ProcessDataErrors(errorCode, data.PartyID.ToString(), data.MedicaidPK.ToString());
            }
        }
    }
}