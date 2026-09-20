using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFSendPayloadODA : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFSendPayloadODA(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int regID;
            Process p = new Process(ProcessID);

            string txnResult = string.Empty;
            int transactionType = 0;
            string partialTTService = string.Empty;
            string partialSubService = string.Empty;

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
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int workflowID = ObjectControllerHelper.GetInt("WorkflowID", drReg);
                int ApplicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                int WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                string mId = ObjectControllerHelper.GetString("MedicaidID", drReg);
                int sId = ObjectControllerHelper.GetInt("REG_SERVICE_LOCATION_ID", drReg);

                PartialProviderRequestResponse pprr = new PartialProviderRequestResponse();

                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    transactionType = (int)TransactionController.TransactionTypeNew.SendPCWFull;
                    partialTTService = CON.TransactionTypeValues.PCW_Full;
                    partialSubService = Constants.PartialProviderSubscriberSystems.PCW;

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

                        // Insert the Staging Tables
                        RegistrationController.PopulateStagingData(tqId, partialTTService, string.Empty);   
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                        txnResult = pprr.partialProviderManagementSubmitRequest(tqId, partialSubService, true, false);  // Txn sent through Partial PubSub

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

                        TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, null, DateTime.Now, Constants.appWorkflowUserId);
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

