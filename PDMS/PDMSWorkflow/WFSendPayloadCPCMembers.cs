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
    public class WFSendPayloadCPCMembers : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        public WFSendPayloadCPCMembers(int processID, int stepID)
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
                int ApplicationTypeID = 0;
                int workflowEventTypeID = 0;
                string CPCPracticeTypeID = string.Empty;

                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    ApplicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                    workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                    CPCPracticeTypeID = ObjectControllerHelper.GetString("MMISCPCPracticeTypeID", drReg);
                }

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                ProviderManagementReqRes prr = new ProviderManagementReqRes();
                int transPassCnt = 0, transFailCnt = 0;
                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    int transactionType = (int)TransactionController.TransactionTypeNew.SendMMISUpdate;

                    if (transactionType > 0)
                    {
                        List<SqlParameter> sqlParms = new List<SqlParameter>();
                        sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                        sqlParms.Add(SqlParms.CreateParameter("WORKFLOW_EVENT_TYPE_ID", DbType.Int32, workflowEventTypeID, false));
                        sqlParms.Add(SqlParms.CreateParameter("CPC_PRACTICE_TYPE", DbType.String, CPCPracticeTypeID, false));
                        sqlParms.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                        sqlParms.Add(SqlParms.CreateParameter("IsConvenerEndDated", DbType.Boolean, false, false));
                        DataSet dsMem = DataAccess.ExecuteStoredProcedure("usp_GetCPCMembers_toSendPayload", sqlParms, "MemberRegData");
                        int MemRegID = 0;
                        if (dsMem.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dsMem.Tables[0].Rows)
                            {
                                MemRegID = ObjectControllerHelper.GetInt("MEMBER_REG_ID", row);
                                sId = ObjectControllerHelper.GetInt("REG_SERVICE_LOCATION_ID", row);

                                int tqId = TransactionController.InsertTransactionQueue(
                                        transactionType,
                                        MemRegID,
                                        sId,
                                        DateTime.Now,
                                        null,
                                        null,
                                        DateTime.Now,
                                        Constants.appWorkflowUserId);
                                SetProcessParameter(Constants.ProcessParameter.TransactionQueueID, tqId.ToString());
                                SaveProcessParameters();
                               
                                RegistrationController.PopulateStagingData(tqId, CON.TransactionTypeValues.MITS, string.Empty);
                                ServicePointManager.Expect100Continue = true;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                                txnResult = prr.providerManagementUpdateRequest(tqId, true);

                                if (txnResult.Equals(CON.TransactionResult.TransactionPassed))
                                {
                                    transPassCnt++;                                    
                                }
                                else
                                {
                                    transFailCnt++;                                    
                                }

                                TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, DateTime.Now, DateTime.Now, Constants.appWorkflowUserId);
                                toReturn = true;                               
                               
                            }

                            if(transFailCnt == 0 && transPassCnt == dsMem.Tables[0].Rows.Count)
                            {
                                nextStep = workflowEventTypeID == CON.WorkflowEventType.UpdateReg ? "Update Event" : CON.TransactionResult.TransactionPassed;
                                toReturn = true;
                            }
                            else
                            {
                                nextStep = CON.TransactionResult.TransactionFailed;
                                toReturn = true;
                            }
                        }
                    }
                }
                else
                {
                    nextStep = workflowEventTypeID == CON.WorkflowEventType.UpdateReg ? "Update Event" : CON.TransactionResult.TransactionPassed;
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
