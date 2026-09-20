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
    public class WFSendPayloadCPCConvener : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        public WFSendPayloadCPCConvener(int processID, int stepID)
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
                string CPCPracticeTypeID = string.Empty;

                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    CPCPracticeTypeID = ObjectControllerHelper.GetString("MMISCPCPracticeTypeID", drReg);
                }
                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    sqlParms.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                    sqlParms.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, AppSettings.Get("CPCProgramYear"), false));
                    sqlParms.Add(SqlParms.CreateParameter("CPC_PRACTICE_TYPE", DbType.String, CPCPracticeTypeID, false));
                    DataSet dsCon = DataAccess.ExecuteStoredProcedure("usp_CheckCPCConvenerEndDated", sqlParms, "ConvenerData");
                    DataRow drCon = ObjectControllerHelper.HasRows(dsCon) ? dsCon.Tables[0].Rows[0] : null;
                    int convenerRegID = 0;
                    if (drCon != null)
                    {
                        convenerRegID = ObjectControllerHelper.GetInt("CONVENER_REG_ID", drCon);
                    }

                    int transactionType = (int)TransactionController.TransactionTypeNew.SendMMISUpdate;
                    int tId = 0;

                    DataSet ds1 = RegistrationController.SelectSvcLocIDTranType(convenerRegID);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        mId = ds1.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
                        sId = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
                        tId = Convert.ToInt32(ds1.Tables[0].Rows[0]["TRANSACTION_TYPE_ID"]);
                    }

                    if (transactionType > 0)
                    {
                        int tqId = TransactionController.InsertTransactionQueue(
                                transactionType,
                                convenerRegID,
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

                        txnResult = prr.providerManagementUpdateRequest(tqId, true);

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
