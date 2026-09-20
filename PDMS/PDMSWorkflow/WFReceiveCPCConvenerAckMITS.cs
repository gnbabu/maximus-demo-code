using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFReceiveCPCConvenerAckMITS : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        public WFReceiveCPCConvenerAckMITS(int processID, int stepID)
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
            int transactionQueueId = 0;
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
                string CPCPracticeTypeID = string.Empty;

                DataSet dsReg1 = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg1 = ObjectControllerHelper.HasRows(dsReg1) ? dsReg1.Tables[0].Rows[0] : null;
                if (drReg1 != null)
                {
                    CPCPracticeTypeID = ObjectControllerHelper.GetString("MMISCPCPracticeTypeID", drReg1);
                }

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

                sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, convenerRegID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int applicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                string mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
                int ProviderCategoryTypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID", drReg);
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int regServiceLocationID = ObjectControllerHelper.GetInt("REG_SERVICE_LOCATION_ID", drReg);
                CPCPracticeTypeID = ObjectControllerHelper.GetString("MMISCPCPracticeTypeID", drReg);
                int EntitytypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID", drReg);

                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                    sqlParmsTR.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, convenerRegID, false));
                    sqlParmsTR.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, regServiceLocationID, false));
                    DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectTRANSACTION_QUEUE", sqlParmsTR, "TRData");
                    dsRegTR.Tables[0].TableName = "TRData";
                    DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                    string SIAcknowledgment = ObjectControllerHelper.GetString("SI_ACK_RESPONSE_CODE", drRegTR);
                    string MITSAcknowledgement = ObjectControllerHelper.GetString("ACK_RESPONSE_CODE", drRegTR);
                    transactionQueueId = ObjectControllerHelper.GetInt("TRANSACTION_QUEUE_ID", drRegTR);

                    int transactionTypeID = ObjectControllerHelper.GetInt("TRANSACTION_TYPE_ID", drRegTR);
                    string SPBMAcknowledgement = ObjectControllerHelper.GetString("SPBM_RESPONSE_CODE", drRegTR);
                    string ACKAcknowledgement = ObjectControllerHelper.GetString("MITS_RESPONSE_CODE", drRegTR);
                    string EVVAcknowledgement = ObjectControllerHelper.GetString("EVV_RESPONSE_CODE", drRegTR);

                    if (SIAcknowledgment.Equals(CON.ResponseCodes.SI_Acknowledgment_SUCCESS) && !string.IsNullOrEmpty(SIAcknowledgment))
                    {
                        if (Methods.validateTxnAcknowledgment(transactionQueueId, transactionTypeID, MITSAcknowledgement, SPBMAcknowledgement, ACKAcknowledgement, EVVAcknowledgement) == CON.TransactionResultIds.ACK_RECEIVED_PASSED)
                        {
                              nextStep = "Transaction Passed";
                              toReturn = true;
                        }
                        else if (Methods.validateTxnAcknowledgment(transactionQueueId, transactionTypeID, MITSAcknowledgement, SPBMAcknowledgement, ACKAcknowledgement, EVVAcknowledgement) == CON.TransactionResultIds.ACK_RECEIVED_WAIT)
                        {
                            nextStep = "";
                            toReturn = false;
                        }
                        else
                        {
                            nextStep = "Transaction Failed";
                            toReturn = true;
                        }
                    }
                    else if (string.IsNullOrEmpty(SIAcknowledgment))
                    {
                        nextStep = "";
                        toReturn = false;
                    }
                    else
                    {
                        nextStep = "Transaction Failed";
                        toReturn = true;
                    }

                }
                else
                {
                    nextStep = "Transaction Passed";
                    toReturn = true;
                }

                if(nextStep == "Transaction Passed")
                {
                    // Update Affiliation Status 
                    List<SqlParameter> sqlParms6 = new List<SqlParameter>();
                    sqlParms6.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, convenerRegID, false));
                    sqlParms6.Add(SqlParms.CreateParameter("TRANSACTION_QUEUE_ID", DbType.Int32, transactionQueueId, false));
                    sqlParms6.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
                    sqlParms6.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, false));
                    DataAccess.ExecuteStoredProcedure("usp_UpdateAffiliationStatus", sqlParms6);

                    //Update the CPC Member specialties 
                    //List<SqlParameter> parameters = new List<SqlParameter>();
                    //parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, convenerRegID, false));
                    //parameters.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, AppSettings.Get("CPCProgramYear"), false));
                    //parameters.Add(SqlParms.CreateParameter("CPC_PRACTICE_TYPE", DbType.String, CPCPracticeTypeID, false));
                    //parameters.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, true));
                    //parameters.Add(SqlParms.CreateParameter("User", DbType.Guid, Constants.appAdminUserId, false));
                    //parameters.Add(SqlParms.CreateParameter("CALL_TYPE", DbType.Int32, 2, false));
                    //parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, EntitytypeID, false));
                    //DataAccess.ExecuteStoredProcedure("usp_UpdateCPCMemberSpecialties", parameters);
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
