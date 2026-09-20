using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;
using static MAXIMUS.Core.Libraries.Constants;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFReceiveResponseFromPCW : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
		static List<string> m_ApplicationStatusIDs = new List<string>();

        public WFReceiveResponseFromPCW(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;
            nextStep = "";
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
            
                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                string AppStatus = string.Empty;
                int workflowEventTypeID = -1;

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int WaiverServiceUpdateTypeID = ObjectControllerHelper.GetInt("WaiverServiceUpdateTypeID", drReg);
                bool HasActiveODASpecialty = ObjectControllerHelper.GetBool("HasActiveODASpecialty", drReg);
                int WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                bool isReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", drReg);
                
                if (!string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId)))
                {
                    WaiverServiceUpdateTypeID = Convert.ToInt32(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId));
                }
                if (canMakeWSRequest.Equals("true"))
                {                    

                    // we are in this class if there was an ODM update (or reval) to a provider with ODA specialties or there was an ODA update or new ODA registration or a DODD update to a provider with ODA specialties
                    // for the last case, this is related to CR177; a DODD update to a provider with ODA specialties needs to have those changes sent over to ODA; in that case, we'll end up in this class after the update has been sent

                    // if this is an ODM Update or OperatorUpdate (or it's a reval) and has an ODA specialty, we know to go look at the the latest ODA application status to see if we've gotten the closed status for this application since we just sent ODA a packet
                    if ((((WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODM || WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.OperatorUpdate) && workflowEventTypeID == CON.WorkflowEventType.UpdateReg) 
                        || workflowEventTypeID == CON.WorkflowEventType.RevalReg || isReactivation) && HasActiveODASpecialty)
                    {
                        List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                        sqlParmsTR.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                        DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_APPLICATIONCustom", sqlParmsTR, "RApplication");
                        dsRegTR.Tables[0].TableName = "RApplication";
                        DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                        AppStatus = ObjectControllerHelper.GetString("Application_Status", drRegTR);

						// since this an ODM update that we used to pass the packet to ODA, we know we didn't come in as a DODD update, so just close the workflow, and there's no need to route to the waiver queue
                        if(AppStatus.Equals(CON.ApplicationStatus.ODA_Closed))
                        {
                            nextStep = "Complete Workflow";
                            toReturn = true;
                        }
                    }
                    else
                    {
                        int additionalApplicationID;
                        if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID), out additionalApplicationID))
                        {
                            throw new Exception(string.Format(
                                Constants.LogString.WorkflowParameterNotNumeric,
                                Assembly.GetExecutingAssembly().GetName().Name,
                                Constants.ProcessParameter.RegistrationID,
                                GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID)));
                        }
                        List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                        sqlParmsTR.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, false));
                        sqlParmsTR.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, false));
                        sqlParmsTR.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, false));
                        DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDITIONAL_APPLICATIONCustom", sqlParmsTR, "RegAddApplication");
                        dsRegTR.Tables[0].TableName = "RegAddApplication";
                        DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                        string OtherAppStatus = ObjectControllerHelper.GetString("OTHER_APPLICATION_STATUS_DESC", drRegTR);
						bool isRequireReview = ObjectControllerHelper.GetBool("IS_REQUIRE_REVIEW", drRegTR);
						string OtherAppStatusID = ObjectControllerHelper.GetString("OTHER_APPLICATION_STATUS_ID", drRegTR);
						string OtherAppLegalStatusID = ObjectControllerHelper.GetString("OTHER_APPLICATION_LEGAL_STATUS_ID", drRegTR);
                        bool enableCR386 = Convert.ToBoolean(AppSettings.Get("EnableCR386", "false"));

                        // OHPNM-5822 we are here because we are either a DODD or ODA Update (or a new ODA registration); for an ODA Update or a new ODA registration, we don't care about the DODD waiver transaction queue.
                        // For a DODD update, we know we passed along a packet to ODA to inform them of a data update and need to get them back to the transaction queue to let that flow take care of closing out the workflow; 
                        // we actually don't need to wait to analyze the ODA packet because they aren't going to send another application status update in this scenario.  We just need to move forward instead.
                        // For an ODA update or new ODA reg, we need to still wait for a "closed" application status from ODA before moving forward.   Until that status is received, just stay locked on this workflow step.
                        if (WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD && workflowEventTypeID == CON.WorkflowEventType.UpdateReg) 
						{
                            // OHPNM-6301 - 6301 changed this a bit; we can't move forward to the waiver queue step if we haven't gotten the legal status on the Closed_Partial_Approval step; we got here because a review wasn't required
                            if (!enableCR386 && !isRequireReview && OtherAppStatusID.Equals(CON.ApplicationStatus.Closed_Partial_Approval) && !IsApplicationStatus(OtherAppLegalStatusID))
                            {
                                nextStep = "Wait for Legal Status"; // this will take them to the new WFWaitForLegalStatus step (task #572)
                            }
                            else
                            {
                                // we got here because of a DODD update; pass back to the waiver transaction queue to decide what to do next since DODD started this update; we don't need to wait for ODA
                                // closed status because ODA is just going to receive this DODD update and not send us an application status change for it
                                nextStep = "Check Waiver Queue";

                                // update the current transaction queue status to "processed"
                                setCurrentWaiverQueueStatusToProcessed(regID, p.ProcessID, additionalApplicationID);
                            }

                            toReturn = true;
						}
						else if ((WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODA && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
						|| (workflowEventTypeID == CON.WorkflowEventType.NewReg && WaiverTypeID == CON.WaiverApplicationTypeID.ODA))
						{
							// if this is an ODA update, we need to wait to get an application closed status before we can move forward; otherwise, just stay locked on this workflow step until then
							if (OtherAppStatus.Equals(CON.ApplicationStatus.Recommended_Certification) || OtherAppStatus.Equals(CON.ApplicationStatus.Recommended_Certification_ID)
							|| OtherAppStatus.Equals(CON.ApplicationStatus.Recommended_Denial_ID))
							{
								// we got here because we are a new ODA registration or an ODA update and received a "closed" application status; we don't care about DODD, so close the workflow
								nextStep = "Complete Workflow";
								toReturn = true;
							}
						}
                    }
                }
                else
                {
                    // this is the default flow for a non-live app in a test environment; change the values based upon how you want the workflow to continue since we aren't actually talking to the SI and getting responses
                    // right now, if it's a new ODA reg, it just completes the workflow; otherwise it goes to the waiver queue to check for more messages from DODD, which may not make sense for your test
                    nextStep = (WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD && workflowEventTypeID == CON.WorkflowEventType.UpdateReg) ? "Check Waiver Queue" : "Complete Workflow";
					
					// if this is a check waiver queue movement, we need to grab the additional application id and update the waiver queue status
					if (nextStep == "Check Waiver Queue") 
					{
						// need to get the current additionalApplicationId, so we can update the transaction queue status
                        int additionalApplicationID;
                        if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID), out additionalApplicationID))
                        {
                            throw new Exception(string.Format(
                                Constants.LogString.WorkflowParameterNotNumeric,
                                Assembly.GetExecutingAssembly().GetName().Name,
                                Constants.ProcessParameter.RegistrationID,
                                GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID)));
                        }
						
						// update the current transaction queue status to "processed"
						setCurrentWaiverQueueStatusToProcessed(regID, p.ProcessID, additionalApplicationID);	
					}
					
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
		
		private void setCurrentWaiverQueueStatusToProcessed(int registrationId, int processId, int addAppId) {
			List<SqlParameter> param = new List<SqlParameter>();
			param.Add(SqlParms.CreateParameter("RegID", DbType.Int32, registrationId, true));
			param.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processId, true));
			param.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, true));
			param.Add(SqlParms.CreateParameter("USER", DbType.Guid, CON.appAdminUserId, true));
			param.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, addAppId, true));
			DataAccess.ExecuteStoredProcedure("usp_UpdateWaiverQueuedTransactionStatus", param);
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
