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
    public class WFSendNODToDODD : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFSendNODToDODD(int processID, int stepID)
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
                    logMsg = string.Format(
                                Constants.LogString.WorkflowParameterNotNumeric,
                                Assembly.GetExecutingAssembly().GetName().Name,
                                Constants.ProcessParameter.RegistrationID,
                                GetProcessParameter(Constants.ProcessParameter.RegistrationID));

                    log.CreateLogEntry(logMsg, Logging.LogPriority.Error);
                }
                else
                {
                    bool chkServiceRemoved = false;
                    DataSet dsReg = ScreeningController.SelectServiceRemovedCheckBox(regID);
                    dsReg.Tables[0].TableName = "ScrData";
                    if (dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0)
                    {
                        chkServiceRemoved = ObjectControllerHelper.GetBool("SERVICE_REMOVED", dsReg.Tables[0].Rows[0]);
                    }
                    PartialProviderRequestResponse pprr = new PartialProviderRequestResponse();

                    if (chkServiceRemoved)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                        bool isDoDDIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfDODDInitialApplication", parameters));

                        List<SqlParameter> sqlParms = new List<SqlParameter>();
                        sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                        DataSet dsR = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                        dsR.Tables[0].TableName = "RegData";
                        DataRow drReg = ObjectControllerHelper.HasRows(dsR) ? dsR.Tables[0].Rows[0] : null;
                        int WaiverServiceUpdateTypeID = ObjectControllerHelper.GetInt("WaiverServiceUpdateTypeID", drReg);

                        string manualUpdate = string.Empty;
                        if (!isDoDDIntialApp && WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.DODD)
                        {
                            manualUpdate = "ManualUpdate";
                        }
                        int transactionType = (int)TransactionController.TransactionTypeNew.SendPSMFull;
                        DataSet ds1 = RegistrationController.SelectMedicaidId(regID);
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
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
                            RegistrationController.PopulateStagingData(tqId, CON.TransactionTypeValues.PSM_FULL, manualUpdate, CON.StageDocumentsTitle.NOD);
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                            txnResult = pprr.partialProviderManagementSubmitRequest(tqId, Constants.PartialProviderSubscriberSystems.PSM, true, false);

                            if (txnResult.Equals(CON.TransactionResult.TransactionPassed))
                            {
                                nextStep = CON.TransactionResult.TransactionPassed;
                            }
                            else
                            {
                                nextStep = CON.TransactionResult.TransactionFailed;
                            }

                            TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, null, DateTime.Now, Constants.appWorkflowUserId);
                            toReturn = true;
                        }
                    }
                    else
                    {
                        nextStep = "Next";
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
