using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MMISInterface;
using System;
using System.Data;
using System.Linq;

namespace MAXIMUS.DataExchange.PDMS
{
    public interface IMMISProcessResponse<TRequest>
    where TRequest : MMISRequest
    {
        TRequest Process(TRequest[] returnedProviders, DataRow providerRow, MMISShared ms, Logging log);
    }
}

namespace MAXIMUS.DataExchange.PDMS.MMISProcess.IndividualRetrieve
{
	public class ProcessProvider : MMISTransactionStep, IMMISProcessResponse<RetrieveWaiverProvider>
	{
		#region Constructors

		public ProcessProvider(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISPostProcess Methods


        public RetrieveWaiverProvider Process(RetrieveWaiverProvider[] returnedProviders, DataRow providerRow, MMISShared ms, Logging log)
		{
            // populate the object credentials and provider object
            string partyId = providerRow["PARTY_ID"].ToString();
            string medicaidId = Methods.GetStringValue(providerRow["MEDICARE_ID"]);
            string transQueueId = Methods.GetStringValue(providerRow["TRANSACTION_QUEUE_ID"]);
            RetrieveWaiverProvider returnedProvider = null;

            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, partyId));

            DateTime now = DateTime.Now;

            if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
            {
                // Retrieve from the MMIS file
                returnedProvider = returnedProviders.FirstOrDefault(record => record.pdmsProviderId == partyId);

                if (returnedProvider == null)
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotFound, partyId));
                }
            }
            else
            {
                int length = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));
                // get test values
                if (string.IsNullOrWhiteSpace(medicaidId))
                {
                    string medicaidPrefix = new string('1', length - 5);
                    medicaidId = medicaidPrefix + DateTime.Now.ToString("ssfff");
                }

                returnedProvider = new RetrieveWaiverProvider()
                {
                    transactionId = transQueueId,
                    pdmsProviderId = partyId,
                    organizationId = medicaidId,
                    statusCode = string.Empty
                };
            }
            return returnedProvider;
		}

		#endregion
	}
}

namespace MAXIMUS.DataExchange.PDMS.MMISProcess.GroupRetrieve
{
    public class ProcessProvider : MMISTransactionStep, IMMISProcessResponse<RetrieveWaiverProvider>
    {
        #region Constructors

        public ProcessProvider(Guid threadID)
            : base(threadID)
        {

        }

        #endregion

        #region IMMISPostProcess Methods

        public RetrieveWaiverProvider Process(RetrieveWaiverProvider[] returnedProviders, DataRow providerRow, MMISShared ms, Logging log)
        {
            // populate the object credentials and provider object
            string partyId = providerRow["PARTY_ID"].ToString();
            string medicaidId = Methods.GetStringValue(providerRow["MEDICAID_ID"]);
            string transQueueId = Methods.GetStringValue(providerRow["TRANSACTION_QUEUE_ID"]);
            RetrieveWaiverProvider returnedProvider = null;

            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, partyId));

            DateTime now = DateTime.Now;

            if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
            {
                // Retrieve from the MMIS file
                returnedProvider = returnedProviders.FirstOrDefault(record => record.pdmsProviderId == partyId);

                if (returnedProvider == null)
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotFound, partyId));
                }
            }
            else
            {
                int length = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));
                // get test values
                if (string.IsNullOrWhiteSpace(medicaidId))
                {
                    string medicaidPrefix = new string('1', length - 5);
                    medicaidId = medicaidPrefix + DateTime.Now.ToString("ssfff");
                }

                returnedProvider = new RetrieveWaiverProvider()
                {
                    transactionId = transQueueId,
                    pdmsProviderId = partyId,
                    organizationId = medicaidId,
                    statusCode = string.Empty
                };
            }
            return returnedProvider;
        }

        #endregion
    }
}

namespace MAXIMUS.DataExchange.PDMS.MMISProcess.WaiverServicesRetrieve
{
    public class ProcessProvider : MMISTransactionStep, IMMISProcessResponse<RetrieveWaiverServices>
    {
        #region Constructors

        public ProcessProvider(Guid threadID)
            : base(threadID)
        {

        }

        #endregion

        #region IMMISPostProcess Methods

        public RetrieveWaiverServices Process(RetrieveWaiverServices[] returnedProviders, DataRow providerRow, MMISShared ms, Logging log)
        {
            // populate the object credentials and provider object
            string partyId = providerRow["PARTY_ID"].ToString();
            string medicaidId = Methods.GetStringValue(providerRow["ORGANIZATION_ID"]);
            string transQueueId = Methods.GetStringValue(providerRow["TRANSACTION_QUEUE_ID"]);
            string serviceType = Methods.GetStringValue(providerRow["SERVICE_CODE"]);
            string programCode = Methods.GetStringValue(providerRow["PROGRAM_CODE"]);

            RetrieveWaiverServices returnedProvider = null;

            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, partyId));

            DateTime now = DateTime.Now;

            if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
            {
                // Retrieve from the MMIS file
                returnedProvider = returnedProviders.FirstOrDefault(record => record.pdmsProviderId == partyId);

                if (returnedProvider == null)
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotFound, partyId));
                }
                else
                {
                    returnedProvider.transactionId = transQueueId;
                }
            }
            else
            {
                int length = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));
                // get test values
                if (string.IsNullOrWhiteSpace(medicaidId))
                {
                    string medicaidPrefix = new string('1', length - 5);
                    medicaidId = medicaidPrefix + DateTime.Now.ToString("ssfff");
                }

                returnedProvider = new RetrieveWaiverServices()
                {
                    transactionId = transQueueId,
                    pdmsProviderId = partyId,
                    organizationId = medicaidId,
                    serviceType = serviceType,
                    programCode = programCode,
                    statusCode = string.Empty
                };
            }
            return returnedProvider;
        }

        #endregion
    }
}
