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
    public class WFCMCReceiveResponseFromSI : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCMCReceiveResponseFromSI(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;
            bool txnSuccess = false;
            int regID;
            int buildingRegID = 0;
            int facilityRegID = 0;
            Process p = new Process(ProcessID);
            int ApplicationTypeID = 0;
            int WaiverTypeID = -1;
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
                int applicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                string mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
                int ProviderCategoryTypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID", drReg);
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int regServiceLocationID = ObjectControllerHelper.GetInt("REG_SERVICE_LOCATION_ID", drReg);
                ApplicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                int workflowID = ObjectControllerHelper.GetInt("WorkflowID", drReg);
                string CPCPracticeTypeID = ObjectControllerHelper.GetString("MMISCPCPracticeTypeID", drReg);

                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    if (mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR)) //LTC 86 or 89
                    {
                        facilityRegID = regID;
                        regID = RegistrationController.SelectBuildingRegistration(regID);
                        buildingRegID = regID;
                        List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                        sqlParms1.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                        dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms1, "RegData");
                        dsReg.Tables[0].TableName = "RegData";
                        drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                        regServiceLocationID = ObjectControllerHelper.GetInt("REG_SERVICE_LOCATION_ID", drReg);
                    }
                    List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                    sqlParmsTR.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    sqlParmsTR.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, regServiceLocationID, false));
                    DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectTRANSACTION_QUEUE", sqlParmsTR, "TRData");
                    dsRegTR.Tables[0].TableName = "TRData";
                    DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                    string SIAcknowledgment = ObjectControllerHelper.GetString("SI_ACK_RESPONSE_CODE", drRegTR);
                    string MITSAcknowledgement = ObjectControllerHelper.GetString("ACK_RESPONSE_CODE", drRegTR);
                    int transactionQueueID = ObjectControllerHelper.GetInt("TRANSACTION_QUEUE_ID", drRegTR);

                    int transactionTypeID = ObjectControllerHelper.GetInt("TRANSACTION_TYPE_ID", drRegTR);
                    string SPBMAcknowledgement = ObjectControllerHelper.GetString("SPBM_RESPONSE_CODE", drRegTR);
                    string ACKAcknowledgement = ObjectControllerHelper.GetString("MITS_RESPONSE_CODE", drRegTR);
                    string EVVAcknowledgement = ObjectControllerHelper.GetString("EVV_RESPONSE_CODE", drRegTR);

                    if (SIAcknowledgment.Equals(CON.ResponseCodes.SI_Acknowledgment_SUCCESS) && !string.IsNullOrEmpty(SIAcknowledgment))
                    {
                        if (Methods.validateTxnAcknowledgment(transactionQueueID, transactionTypeID, MITSAcknowledgement, SPBMAcknowledgement, ACKAcknowledgement, EVVAcknowledgement) == CON.TransactionResultIds.ACK_RECEIVED_PASSED)
                        {
                            if ((mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR))
                                   && p.WorkflowID != CON.WorkflowType.CHOP && p.WorkflowID != CON.WorkflowType.CredentialingApplication && p.WorkflowID != CON.WorkflowType.PeriodicDatabaseChecks) //LTC 86 or 89
                            {
                                // set building back to active.
                                nextStep = "LTC Provider";
                            }
                            else if ((ApplicationTypeID == CON.ApplicationType.Waiver && WaiverTypeID == CON.WaiverApplicationTypeID.ODA) || (WaiverTypeID == CON.WaiverApplicationTypeID.DODD || WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD))
                            {
                                nextStep = "Waiver Provider";
                            }
                            else if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                            {
                                nextStep = "Update Event";
                            }
                            else
                            {
                                if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                                {
                                    nextStep = "Revalidation Notice";
                                }
                                else
                                {
                                    nextStep = "Transaction Passed";
                                }
                            }
                            if (nextStep != "Waiver Provider" && workflowID == CON.WorkflowType.RegistrationNew)
                            {
                                List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                                sqlParms1.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                                bool isScreeningAutoTerminated = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckScreeningAutoTerminated", sqlParms1));

                                bool isDoDDAbuser = ScreeningController.CheckDODDAbuserRegistryMatch(regID);
                                bool isFedExc = ScreeningController.CheckFedExclusionsMatch(regID);
                                bool isNPPESeX = ScreeningController.CheckNPPESInactiveMatch(regID);

                                List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                                sqlParms2.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                                bool ifTerminatedFromCredentialing = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckTerminatedFromCredentialing", sqlParms2));

                                if ((isScreeningAutoTerminated || ifTerminatedFromCredentialing || isDoDDAbuser || isFedExc || isNPPESeX))
                                {
                                    nextStep = "Complete Workflow";  // If it has come here from Screening Auto Termination or terminated from credentialing then end the workflow.
                                }
                            }
                            if (nextStep == "Transaction Passed" && workflowID == CON.WorkflowType.CPC && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                            {
                                nextStep = "Complete Workflow";
                            }
                            txnSuccess = true;
                            toReturn = true;
                        }
                        else if (Methods.validateTxnAcknowledgment(transactionQueueID, transactionTypeID, MITSAcknowledgement, SPBMAcknowledgement, ACKAcknowledgement, EVVAcknowledgement) == CON.TransactionResultIds.ACK_RECEIVED_WAIT)
                        {
                            nextStep = "";
                            toReturn = false;
                        }
                        else
                        {
                            nextStep = "Transaction Failed";
                            toReturn = true;
                        }
                        if (nextStep == "Transaction Failed" || nextStep == "Transaction Passed" || nextStep == "LTC Provider" || nextStep == "Waiver Provider" || nextStep == "Complete Workflow")
                        {
                            TransactionController.UpdateTransactionQueue(transactionQueueID, null, DateTime.Now, DateTime.Now, Constants.appWorkflowUserId);
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
                    if ((mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR))
                                   && p.WorkflowID != CON.WorkflowType.CHOP && p.WorkflowID != CON.WorkflowType.CredentialingApplication && p.WorkflowID != CON.WorkflowType.PeriodicDatabaseChecks) //LTC 86 or 89
                    {
                        // set building back to active.
                        nextStep = "LTC Provider";
                    }
                    else if ((ApplicationTypeID == CON.ApplicationType.Waiver && WaiverTypeID == CON.WaiverApplicationTypeID.ODA) || (WaiverTypeID == CON.WaiverApplicationTypeID.DODD || WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD))
                    {
                        nextStep = "Waiver Provider";
                    }
                    else if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg && workflowID != CON.WorkflowType.CPC)
                    {
                        nextStep = "Update Event";
                    }
                    else
                    {
                        if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                        {
                            nextStep = "Revalidation Notice";
                        }
                        else
                        {
                            nextStep = "Transaction Passed";
                        }
                    }
                    if (nextStep != "Waiver Provider" && workflowID == CON.WorkflowType.RegistrationNew)
                    {
                        List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                        sqlParms1.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                        bool isScreeningAutoTerminated = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckScreeningAutoTerminated", sqlParms1));

                        bool isDoDDAbuser = ScreeningController.CheckDODDAbuserRegistryMatch(regID);
                        bool isFedExc = ScreeningController.CheckFedExclusionsMatch(regID);

                        List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                        sqlParms2.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                        bool ifTerminatedFromCredentialing = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckTerminatedFromCredentialing", sqlParms2));

                        if ((isScreeningAutoTerminated || ifTerminatedFromCredentialing || isDoDDAbuser || isFedExc))
                        {
                            nextStep = "Complete Workflow";  // If it has come here from Screening Auto Termination or terminated from credentialing then end the workflow.
                        }
                    }

                    txnSuccess = true;
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
