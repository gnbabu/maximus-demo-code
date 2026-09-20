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
    public class WFReceiveResponseMMIS : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
		static List<string> m_ApplicationStatusIDs = new List<string>();

        public WFReceiveResponseMMIS(int processID, int stepID)
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
            int transactionQueueID = 0;
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
                bool HasODASpecialty = ObjectControllerHelper.GetBool("HasODASpecialty", drReg);
                bool HasDODDSpecialty = ObjectControllerHelper.GetBool("HasDODDSpecialty", drReg);
                bool HasDDContractNumber = !string.IsNullOrEmpty(ObjectControllerHelper.GetString("dd_contract_number", drReg)) ? true : false;
                //ohpnm-17462
                int WaiverServiceUpdateTypeID = 0;
                if (!string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId)))
                {
                    WaiverServiceUpdateTypeID = Convert.ToInt32(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId));
                }        
                 
                bool isUpdateCPCContactOnly = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.UpdateCpcContact)) 
						? false : Convert.ToBoolean(p.GetProcessParameter(Constants.ProcessParameter.UpdateCpcContact));
				int EnrollmentStatusCode = ObjectControllerHelper.GetInt("EnrollmentStatusCode", drReg);
				bool isReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", drReg);
				bool isReapplication = ObjectControllerHelper.GetBool("isreapplication", drReg);
                bool isFromORP = ObjectControllerHelper.GetBool("is_fromorp", drReg);
                int EntitytypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID", drReg);

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
                    transactionQueueID = ObjectControllerHelper.GetInt("TRANSACTION_QUEUE_ID", drRegTR);

                    int transactionTypeID = ObjectControllerHelper.GetInt("TRANSACTION_TYPE_ID", drRegTR);
                    string SPBMAcknowledgement = ObjectControllerHelper.GetString("SPBM_RESPONSE_CODE", drRegTR);
                    string ACKAcknowledgement = ObjectControllerHelper.GetString("MITS_RESPONSE_CODE", drRegTR);
                    bool enableCR386 = Convert.ToBoolean(AppSettings.Get("EnableCR386", "false"));
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
                            else if ((WaiverTypeID == CON.WaiverApplicationTypeID.ODA || WaiverTypeID == CON.WaiverApplicationTypeID.DODD || 
                                WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)
                                ||(HasDODDSpecialty || HasODASpecialty || HasDDContractNumber || WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD ||
                                WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODA))
                            {
								if ((workflowEventTypeID == CON.WorkflowEventType.RevalReg && EnrollmentStatusCode != CON.EnrollmentStatusTypeID.InActive) && !(isReactivation || isReapplication || isFromORP))
								{
									nextStep = "Revalidation Notice Reval Waiver"; // send letter to customer first; that task will take care of getting us to the rest of the "Waiver Provider" flow
								}
                                // SAM751 new notice for Reapplication, Reactivation and ORP to Active Standard providers.
                                else if (isReactivation || isReapplication || isFromORP || (workflowEventTypeID == CON.WorkflowEventType.RevalReg && EnrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive))
                                {
                                    nextStep = "Reapplication Notice";
                                }
                                else 
                                {
                                    nextStep = "Waiver Provider";
                                }
                            }
                            else if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg && workflowID != CON.WorkflowType.CPC)
                            {
                                //SAM751 new notice for Reapplication, Reactivation and ORP to Active Standard providers.
                                if (isReactivation)
                                {
                                    nextStep = "Reapplication Notice"; // SAM751
                                }
                                else
                                    nextStep = "Update Event";
                            }
							else
							{
								if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
								{
									if (isReactivation || isReapplication || isFromORP || EnrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive)
									{
                                        nextStep = "Reapplication Notice"; // SAM751
                                    }
									else
									{
										if (EnrollmentStatusCode != CON.EnrollmentStatusTypeID.InActive)
											nextStep = "Revalidation Notice";
                                    }
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
                            /*if(nextStep == "Transaction Passed" && workflowID == CON.WorkflowType.CPC && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                            {
                                nextStep = "Complete Workflow";
                            }*/
                            if (nextStep == "Waiver Provider" && WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                            {
                                int additionalApplicationID;
                                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID), out additionalApplicationID))
                                {
                                    throw new Exception(string.Format(
                                        Constants.LogString.WorkflowParameterNotNumeric,
                                        Assembly.GetExecutingAssembly().GetName().Name,
                                        Constants.ProcessParameter.AdditionalApplicationID,
                                        GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID)));
                                }
                                List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                                sqlParms2.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, false));
                                sqlParms2.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, false));
                                sqlParms2.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, false));
                                DataSet dsDoDDApp = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDITIONAL_APPLICATIONCustom", sqlParms2, "RegAddApplication");

                                if (ObjectControllerHelper.HasRows(dsDoDDApp))
                                {
                                    bool isRequireReview = ObjectControllerHelper.GetBool("IS_REQUIRE_REVIEW", dsDoDDApp.Tables[0].Rows[0]);
                                    string OtherAppStatusID = ObjectControllerHelper.GetString("OTHER_APPLICATION_STATUS_ID", dsDoDDApp.Tables[0].Rows[0]);
                                    string OtherAppLegalStatusID = ObjectControllerHelper.GetString("OTHER_APPLICATION_LEGAL_STATUS_ID", dsDoDDApp.Tables[0].Rows[0]);
                                    nextStep = !isRequireReview ? "Check Waiver Queue" : nextStep;

                                    // OHPNM-5822 - only go to the queue step if there are no ODA specialties; otherwise we need to go to task_id #830 in order to pass along updates to the ODA end-point					
                                    if (nextStep == "Check Waiver Queue" && HasODASpecialty)
                                    {
                                        // overwrite nextStep to send them over to ODA
                                        nextStep = "Check ODA";
                                    }

                                    if (nextStep == "Check Waiver Queue")
                                    {
                                        // OHPNM-6301 - if we should be going back to the waiver queue, we need to see if we are closed partial provider w/o legal status data; if so, we need to go wait for a bit first only if isRequireReview is false
                                        if (!enableCR386 && !isRequireReview && OtherAppStatusID.Equals(CON.ApplicationStatus.Closed_Partial_Approval) && !IsApplicationStatus(OtherAppLegalStatusID))
                                        {
                                            nextStep = "Wait for Legal Status"; // this will take them to the new WFWaitForLegalStatus step  (task #572)
                                        }
                                        else
                                        {
                                            // Update the waiver queue status to Processed
                                            List<SqlParameter> param = new List<SqlParameter>();
                                            param.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                                            param.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, true));
                                            param.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, true));
                                            param.Add(SqlParms.CreateParameter("USER", DbType.Guid, CON.appAdminUserId, true));
                                            param.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, true));
                                            DataAccess.ExecuteStoredProcedure("usp_UpdateWaiverQueuedTransactionStatus", param);
                                        }
                                    }
                                }
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
                    else if (string.IsNullOrEmpty(SIAcknowledgment) || SIAcknowledgment.Equals(CON.ResponseCodes.SI_Warning))
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
                    else if ((WaiverTypeID == CON.WaiverApplicationTypeID.ODA || WaiverTypeID == CON.WaiverApplicationTypeID.DODD || WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)
                                || (HasDODDSpecialty || HasODASpecialty || HasDDContractNumber || WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD || WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODA))
                    {
                        //nextStep = "Waiver Provider";
                        if ((workflowEventTypeID == CON.WorkflowEventType.RevalReg && EnrollmentStatusCode != CON.EnrollmentStatusTypeID.InActive) && !(isReactivation || isReapplication || isFromORP))
                        {
                            nextStep = "Revalidation Notice Reval Waiver"; // send letter to customer first; that task will take care of getting us to the rest of the "Waiver Provider" flow
                        }
                        // SAM751 new notice for Reapplication, Reactivation and ORP to Active Standard providers.
                        else if (isReactivation || isReapplication || isFromORP || (workflowEventTypeID == CON.WorkflowEventType.RevalReg && EnrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive))
                        {
                            nextStep = "Reapplication Notice";
                        }
                        else
                        {
                            nextStep = "Waiver Provider";

                        }
                    }
                    else if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg && workflowID != CON.WorkflowType.CPC)
                    {
                        //SAM751 new notice for Reapplication, Reactivation and ORP to Active Standard providers.
                        if (isReactivation)
                        {
                            nextStep = "Reapplication Notice"; // SAM751
                        }
                        else
                            nextStep = "Update Event";
                    }
                    else
                    {
                        if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                        { 
                            if(isReactivation || isReapplication || isFromORP || EnrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive)
                            {
								nextStep = "Reapplication Notice"; // SAM751
                            }
                            else
                            {
                                if(EnrollmentStatusCode != CON.EnrollmentStatusTypeID.InActive)
									nextStep = "Revalidation Notice";
							}
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
                    if (nextStep == "Waiver Provider" && WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                    {
                        int additionalApplicationID;
                        if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID), out additionalApplicationID))
                        {
                            throw new Exception(string.Format(
                                Constants.LogString.WorkflowParameterNotNumeric,
                                Assembly.GetExecutingAssembly().GetName().Name,
                                Constants.ProcessParameter.AdditionalApplicationID,
                                GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID)));
                        }
                        List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                        sqlParms2.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, false));
                        sqlParms2.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, false));
                        sqlParms2.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, false));
                        DataSet dsDoDDApp = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDITIONAL_APPLICATIONCustom", sqlParms2, "RegAddApplication");
                        if (ObjectControllerHelper.HasRows(dsDoDDApp))
                        {
                            bool isRequireReview = ObjectControllerHelper.GetBool("IS_REQUIRE_REVIEW", dsDoDDApp.Tables[0].Rows[0]);
                            nextStep = !isRequireReview ? "Check Waiver Queue" : nextStep;

							// OHPNM-5822 - only go to the queue step if there are no ODA specialties; otherwise we need to go to task_id #830 in order to pass along updates to the ODA end-point					
							if (nextStep == "Check Waiver Queue" && HasODASpecialty) 
							{
								// overwrite nextStep to send them over to ODA
								nextStep = "Check ODA";
							}
									
                            if (nextStep == "Check Waiver Queue")
                            {
                                // Update the waiver queue status to Processed
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                                param.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, true));
                                param.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, true));
                                param.Add(SqlParms.CreateParameter("USER", DbType.Guid, CON.appAdminUserId, true));
                                param.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, true));
                                DataAccess.ExecuteStoredProcedure("usp_UpdateWaiverQueuedTransactionStatus", param);
                            }
                        }
                    }

                    txnSuccess = true;
                    toReturn = true;
                }
                


                if (txnSuccess)
                {
                    if (((applicationTypeID == CON.ApplicationType.Standard || applicationTypeID == CON.ApplicationType.ORP || applicationTypeID == CON.ApplicationType.Waiver)
                            && (mmisProviderTypeID != CON.LTCProviderTypes.NursingFacility && mmisProviderTypeID != CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR && mmisProviderTypeID != CON.LTCProviderTypes.STATE_OPERATED_ICF_MR)
                            && ProviderCategoryTypeID != CON.ProviderCategoryTypeID.Individual) || (applicationTypeID == CON.ApplicationType.CPC && mmisProviderTypeID == "99"))
                    {
                        // Update Affiliation Status
                        List<SqlParameter> sqlParms6 = new List<SqlParameter>();
                        sqlParms6.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                        sqlParms6.Add(SqlParms.CreateParameter("TRANSACTION_QUEUE_ID", DbType.Int32, transactionQueueID, false));
                        sqlParms6.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
                        sqlParms6.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, false));
                        DataAccess.ExecuteStoredProcedure("usp_UpdateAffiliationStatus", sqlParms6);
                    }

                    if (workflowID == CON.WorkflowType.CPC)
                    {
                        //SAM582 - update cpc contact on the primary regid
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                        param.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                        param.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, Constants.AddressType.CPCAddressType, false));
                        param.Add(SqlParms.CreateParameter("WorkflowEventTypeId", DbType.Int32, workflowEventTypeID, false));
                        param.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, DateTime.Now.ToString(), false));
                        param.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, Constants.appAdminUserId, false));
                        DataAccess.ExecuteStoredProcedure("usp_SaveCPCContactInfo_PrimaryReg", param);

                        // OHPNM-7214 if this was just a CPC Contact Update, we don't need to do anything else; just complete the workflow
                        if (nextStep == "Transaction Passed" && workflowEventTypeID == CON.WorkflowEventType.UpdateReg && isUpdateCPCContactOnly)
                        {
                            nextStep = "Complete Workflow";
                        }
                        else
                        {
                            //Update the CPC Member specialties 
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                            parameters.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, AppSettings.Get("CPCProgramYear"), false));
                            parameters.Add(SqlParms.CreateParameter("CPC_PRACTICE_TYPE", DbType.String, CPCPracticeTypeID, false));
                            parameters.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, true));
                            parameters.Add(SqlParms.CreateParameter("User", DbType.Guid, Constants.appAdminUserId, false));
                            parameters.Add(SqlParms.CreateParameter("CALL_TYPE", DbType.Int32, 1, false));
                            parameters.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, EntitytypeID, false));
                            DataAccess.ExecuteStoredProcedure("usp_UpdateCPCMemberSpecialties", parameters);
                        }
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
		
        static List<string> ApplicationStatusIDs()
        {
            if (m_ApplicationStatusIDs.Count == 0)
            {
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Closed_By_ODM);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Certified);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Closed_As_Legal_Override);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Approved_ID);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.ClosedAsAdjudicationOrderIssued);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Closed_As_Complete_ID);
            }
            return m_ApplicationStatusIDs;
        }

        public static bool IsApplicationStatus(string statusName)
        {
            return ApplicationStatusIDs().Contains(statusName);
        }
    }
}
