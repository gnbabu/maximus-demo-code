using MAXIMUS.Core.Libraries;
using System;
using System.Net;

namespace Corp.Core.Libraries
{
    public class SendTerminationTransactionPayload
    {
        public bool SendTransaction(int transactionID, int transactionType, bool sendHistory = false)
        {
            bool rtn = false;
            string txnResult = string.Empty;
            string transType = "";

            if (transactionType == 1)
            {
                transType = "MITS";
            }
            else if (transactionType == 5)
            {
                transType = "PSM Full";
            }
            else if (transactionType == 6)
            {
                transType = "PCW Full";
            }
            else if (transactionType == 2)
            {
                transType = "MITS Enroll";
            }
            string appEnv = AppSettings.Get("MakeWSRequestCallToSI");

            if (appEnv.ToLower().Equals("true"))
            {
                string manualUpdate = transType == "PSM Full" ? "ManualUpdate" : string.Empty;

                if (transType.Equals("MITS Enroll"))
                {
                    InfoAccessController.PopulateStagingData(transactionID, "MITS", manualUpdate, sendHistory);
                }
                else
                {
                    InfoAccessController.PopulateStagingData(transactionID, transType, manualUpdate, sendHistory);
                }
                
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                if (transType == "MITS")
                {
                    ProviderManagementReqRes prr = new ProviderManagementReqRes();
                    txnResult = prr.providerManagementUpdateRequest(transactionID, true);
                }
                else if (transType == "MITS Enroll")
                {
                    ProviderManagementReqRes prr = new ProviderManagementReqRes();
                    txnResult = prr.providerManagementEnrollRequest(transactionID, true);
                }
                else if (transType == "PSM Full")
                {
                    PartialProviderRequestResponse pprr = new PartialProviderRequestResponse();
                    txnResult = pprr.partialProviderManagementSubmitRequest(transactionID, Constants.PartialProviderSubscriberSystems.PSM, true);
                }
                else if (transType == "PCW Full")
                {
                    PartialProviderRequestResponse pprr = new PartialProviderRequestResponse();
                    txnResult = pprr.partialProviderManagementSubmitRequest(transactionID, Constants.PartialProviderSubscriberSystems.PCW, true);
                }

                InfoAccessController.UpdateTransactionQueue(transactionID, DateTime.Now, null, DateTime.Now, Constants.appWorkflowUserId);
            }
            
            return rtn;
        }

        public bool SendTransactionMITS(int transactionID, int transactionType)
        {
            bool rtn = false;
            string txnResult = string.Empty;
            string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
            if (canMakeWSRequest.Equals("true"))
            {
                InfoAccessController.PopulateStagingData(transactionID, transactionType.ToString(), string.Empty);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                ProviderManagementReqRes prr = new ProviderManagementReqRes();
                txnResult = prr.providerManagementUpdateRequest(transactionID, true);
            }

            InfoAccessController.UpdateTransactionQueue(transactionID, DateTime.Now, null, DateTime.Now, Constants.appWorkflowUserId);
            return rtn;
        }

    }
}
