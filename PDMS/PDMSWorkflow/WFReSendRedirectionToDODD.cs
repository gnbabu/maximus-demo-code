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
    public class WFReSendRedirectionToDODD : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFReSendRedirectionToDODD(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);
            int sId = 0;
            string mId = string.Empty;
            bool toReturn = false;
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
                int regProgramStatusID = ObjectControllerHelper.GetInt("RegistrationProgramStatusTypeID", drReg);
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int wfEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);

                PartialProviderRequestResponse pprr = new PartialProviderRequestResponse();
                sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                bool isScreeningAutoTerminated = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckScreeningAutoTerminated", sqlParms));

                bool isPubSub = (regProgramStatusID == CON.RegistrationProgramStatusTypeId.Conversion || wfEventTypeID != CON.WorkflowEventType.NewReg);

                if (isScreeningAutoTerminated && workflowID == CON.WorkflowType.RegistrationNew)
                {
                    nextStep = "Complete Workflow";  // If it has come here from Screening Auto Termination then Create and send the transaction and end the workflow.
                }
                else
                {                    
                    string makeRequest = AppSettings.Get("MakeWSRequestCallToSI");
                    if (makeRequest.ToLower().Equals("true"))
                    {
                        int transactionType = (int)TransactionController.TransactionTypeNew.SendPSMPartial;
                        string payloadType = CON.TransactionTypeValues.PSM_Partial;

                        if (isPubSub)
                        {
                            transactionType = (int)TransactionController.TransactionTypeNew.SendPSMFull;
                            payloadType = CON.TransactionTypeValues.PSM_FULL;
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
                            
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                            if (isPubSub)
                            {
                                RegistrationController.PopulateStagingData(tqId, payloadType, string.Empty);
                                txnResult = pprr.partialProviderManagementSubmitRequest(tqId, Constants.PartialProviderSubscriberSystems.PSM, true, false);
                            }
                            else
                            {
                                RegistrationController.PopulateStagingData(tqId, payloadType, string.Empty);
                                txnResult = pprr.partialProviderManagementSubmitRequest(tqId, Constants.PartialProviderSubscriberSystems.PSM, true);
                            }
                            

                            if (txnResult.Equals(CON.TransactionResult.TransactionPassed))
                            {
                                nextStep = workflowEventTypeID == CON.WorkflowEventType.NewReg ? "New Event" : "Update Event";
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
