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
    public class WFReSendRedirectionToODA : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFReSendRedirectionToODA(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;
            int sId = 0;
            string mId = string.Empty;
            string txnResult = string.Empty;
            int regID;
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
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int workflowID = ObjectControllerHelper.GetInt("WorkflowID", drReg);
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int regProgramStatusID = ObjectControllerHelper.GetInt("RegistrationProgramStatusTypeID", drReg);
                PartialProviderRequestResponse pprr = new PartialProviderRequestResponse();

                bool isFullPayload = (regProgramStatusID == CON.RegistrationProgramStatusTypeId.Conversion || workflowEventTypeID != CON.WorkflowEventType.NewReg);

                string makeRequest = AppSettings.Get("MakeWSRequestCallToSI");
                if (makeRequest.ToLower().Equals("true"))
                {
                    int transactionType = (int)TransactionController.TransactionTypeNew.SendPCWPartial;
                    string payloadType = CON.TransactionTypeValues.PCW_Partial;

                    if (isFullPayload)
                    {
                        transactionType = (int)TransactionController.TransactionTypeNew.SendPCWFull;
                        payloadType = CON.TransactionTypeValues.PCW_Full;
                    }
                    DataSet ds1 = RegistrationController.SelectMedicaidId(regID);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        mId = ds1.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
                        sId = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
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
                        RegistrationController.PopulateStagingData(tqId, payloadType, string.Empty);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                        txnResult = pprr.partialProviderManagementSubmitRequest(tqId, Constants.PartialProviderSubscriberSystems.PCW, true);

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
                    nextStep = workflowEventTypeID == CON.WorkflowEventType.NewReg ? "New Event" : "Update Event";
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



        private DataSet SelectMedicaidId(int regID)
        {
            //string returnVal = string.Empty;
            DataSet ds = null;
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));

                ds = DataAccess.ExecuteStoredProcedure("usp_SelectServiceLocationID_ByRegID", parameters, "ServiceLocations");
                ds.Tables[0].TableName = "ServiceLocations";
            }
            catch (Exception ex)
            {
                CoreException.ThrowException(LogThreadID, ex, logProcessName);
            }
            return ds;
        }

        private void PopulateStagingData(int transactionID, string transactionType, string Result)
        {
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep 11 2013  5:21PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("transaction_queue_id", DbType.Int32, transactionID, true));
                parameters.Add(SqlParms.CreateParameter("transactionType", DbType.String, transactionType, true));
                parameters.Add(SqlParms.CreateParameter("Result", DbType.String, Result, true));
                DataAccess.ExecuteStoredProcedure("usp_TransferPNMToStaging", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(LogThreadID, ex, logProcessName);
            }
        }
    }
}
