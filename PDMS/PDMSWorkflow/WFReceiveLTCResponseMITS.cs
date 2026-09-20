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
     public class WFReceiveLTCResponseMITS : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFReceiveLTCResponseMITS(int processID, int stepID)
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

                string makeRequest = AppSettings.Get("MakeWSRequestCallToSI","false");
                if (makeRequest.ToLower().Equals("true"))
                {
                    List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                    sqlParmsTR.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    sqlParmsTR.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, regServiceLocationID, false));
                    //sqlParmsTR.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, CON.TransactionType.SendProviderUpdatestoMMIS, false));
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
                            int facilityRegID = regID;
                            regID = RegistrationController.SelectBuildingRegistration(regID);
                            int buildingRegID = regID;
                            RegistrationController.UpdateBuildingRegistrationToActive(facilityRegID, buildingRegID, CON.GroupAffiliationStatusTypeID.Active, DateTime.Now, new Guid(CON.appAdminUserId));
                            if (mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) && (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)) 
                            {
                                nextStep = "Next";
                            }
                            else if ((mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) 
                                || mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility)) && workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                            {
                                nextStep = "Revalidation 86-89";
                            }
                            else if ((mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR)) 
                                && workflowEventTypeID != CON.WorkflowEventType.RevalReg)
                            {
                                nextStep = "Provider Type 89";
                            }
                            else
                            {
                                nextStep = "Transaction Passed";
                            }
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
                else if (mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) && (workflowEventTypeID == CON.WorkflowEventType.UpdateReg))
                {
                    nextStep = "Next";
                    toReturn = true;
                }
                else
                {
                    nextStep = "Transaction Passed";
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
