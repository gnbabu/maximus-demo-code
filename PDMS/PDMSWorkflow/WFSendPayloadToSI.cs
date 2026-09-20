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
    public class WFSendPayloadToSI : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFSendPayloadToSI(int processID, int stepID)
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
            //bool txnSuccess = true;
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
                string mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                bool isReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", drReg);
                bool isReapplication = ObjectControllerHelper.GetBool("isreapplication", drReg);
                bool isReconsideration = false;

                if (ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg) == CON.WorkflowEventType.Reconsideration || ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg) == CON.WorkflowEventType.CredentialReconsideration)
                {
                    isReconsideration = true;
                }
                //OHPNM-14823 - Send history in transaction on a termed provider that is being made active
                bool IsSendHistoryinTxn = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.IsSendHistoryinTxn))
                        ? false : Convert.ToBoolean(p.GetProcessParameter(Constants.ProcessParameter.IsSendHistoryinTxn));

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                ProviderManagementReqRes prr = new ProviderManagementReqRes();

                               
                    string makeRequest = AppSettings.Get("MakeWSRequestCallToSI");
                    if (makeRequest.ToLower().Equals("true"))
                    {
                        if (mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR)) //LTC 86 or 89
                        {
                            regID = RegistrationController.SelectBuildingRegistration(regID);
                        }
                        int transactionType = 0;
                        int tId = 0;

                        DataSet ds1 = RegistrationController.SelectSvcLocIDTranType(regID);
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            mId = ds1.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
                            sId = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
                            tId = Convert.ToInt32(ds1.Tables[0].Rows[0]["TRANSACTION_TYPE_ID"]);
                        }

                        if (tId == (int)TransactionController.TransactionTypeNew.SendMMISEnroll) 
                        {
                            transactionType = (int)TransactionController.TransactionTypeNew.SendMMISEnroll;
                        }
                        else
                        {
                            transactionType = (int)TransactionController.TransactionTypeNew.SendMMISUpdate;
                        }

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
                            if (isReactivation || isReapplication || isReconsideration || IsSendHistoryinTxn)
                            {
                                InfoAccessController.PopulateStagingData(tqId, Constants.TransactionTypeValues.MITS, string.Empty, true);
                            }
                            else { 
                                RegistrationController.PopulateStagingData(tqId, CON.TransactionTypeValues.MITS, string.Empty); 
                            }
							
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
