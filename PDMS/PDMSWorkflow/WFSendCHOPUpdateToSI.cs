using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Net;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFSendCHOPUpdateToSI : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFSendCHOPUpdateToSI(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;
            int regID;
            int sId = 0;
            string mId = string.Empty;
            string txnResult = string.Empty;
            Process p = new Process(ProcessID);

            try
            {
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out regID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                ProviderManagementReqRes prr = new ProviderManagementReqRes();

                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    int transactionType = 0;
                    int tId = 0;

                    DataSet ds1 = RegistrationController.SelectSvcLocIDTranType(regID);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        mId = ds1.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
                        sId = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
                        tId = Convert.ToInt32(ds1.Tables[0].Rows[0]["TRANSACTION_TYPE_ID"]);
                    }

                    transactionType = (int)TransactionController.TransactionTypeNew.SendMMISUpdate;

                    if (transactionType > 0)
                    {
                        int tqId = TransactionController.InsertTransactionQueue(
                                transactionType,
                                regID,
                                sId,
                                DateTime.Now,
                                null,
                                null,
                                DateTime.Now,
                                Constants.appWorkflowUserId);
                        SetProcessParameter(Constants.ProcessParameter.TransactionQueueID, tqId.ToString());
                        SaveProcessParameters();

                        // Insert the MMIS Staging Tables
                        RegistrationController.PopulateStagingData(tqId, CON.TransactionTypeValues.MITS, string.Empty);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                        if (transactionType == (int)TransactionController.TransactionTypeNew.SendMMISEnroll)
                        {
                            txnResult = prr.providerManagementEnrollRequest(tqId, true);
                        }
                        else
                        {
                            txnResult = prr.providerManagementUpdateRequest(tqId, true);
                        }

                        if (txnResult.Equals(CON.TransactionResult.TransactionPassed))
                        {
                            nextStep = CON.TransactionResult.TransactionPassed;
                            toReturn = true;
                        }
                        else
                        {
                            nextStep = CON.TransactionResult.TransactionFailed;
                            toReturn = true;
                        }

                        TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, DateTime.Now, DateTime.Now, Constants.appWorkflowUserId);
                        toReturn = true;
                    }
                }
                else
                {
                    nextStep = CON.TransactionResult.TransactionPassed;
                    toReturn = true;
                }
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return toReturn;
        }
        override public string NextStep()
        {
            return nextStep;
        }

    }
}
