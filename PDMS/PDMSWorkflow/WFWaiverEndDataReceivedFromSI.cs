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
    public class WFWaiverEndDataReceivedFromSI : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        static List<string> m_ApplicationStatusIDs = new List<string>();

        public WFWaiverEndDataReceivedFromSI(int processID, int stepID)
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
				
                string canMakeWSRequest = AppSettings.Get("CheckEnvConnectedToSSA");
                int workflowEventTypeID = -1;

                if (canMakeWSRequest.Equals("true"))
                {
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                    DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                    dsReg.Tables[0].TableName = "RegData";
                    DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                    workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                   // int WaiverServiceUpdateTypeID = ObjectControllerHelper.GetInt("WaiverServiceUpdateTypeID", drReg);
                    bool HasActiveODASpecialty = ObjectControllerHelper.GetBool("HasActiveODASpecialty", drReg);
                    int WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                    int registrationStatusTypeID = ObjectControllerHelper.GetInt("RegistrationStatusTypeID",drReg);

                    int WaiverServiceUpdateTypeID = 0;
                    if (!string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId)))
                    {
                        WaiverServiceUpdateTypeID = Convert.ToInt32(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId));
                    }

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
					string OtherAppStatusID = ObjectControllerHelper.GetString("OTHER_APPLICATION_STATUS_ID", drRegTR);
					string OtherAppLegalStatusID = ObjectControllerHelper.GetString("OTHER_APPLICATION_LEGAL_STATUS_ID", drRegTR);
                    bool enableCR386 = Convert.ToBoolean(AppSettings.Get("EnableCR386", "false"));

                    // OHPNM-5822 - at this point, we know we are a DODD blue application, if the application is closed, we need to check if provider has oda services; if so, we need to forward changes to ODA
                    if ((WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
						|| (workflowEventTypeID == CON.WorkflowEventType.NewReg && (WaiverTypeID == CON.WaiverApplicationTypeID.DODD || WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)))
					{
                        //OHPNM-14214 - CR386 - Ignore Legal status check for Closed partial partial approval status
                        if ((IsApplicationStatus(OtherAppStatusID) || (enableCR386 && OtherAppStatusID.Equals(CON.ApplicationStatus.Closed_Partial_Approval)))
                            || (!enableCR386 && IsApplicationStatus(OtherAppLegalStatusID) && OtherAppStatusID.Equals(CON.ApplicationStatus.Closed_Partial_Approval)))
                        {
                            // if it's an update and has active ODA specialty, we need to send them along to ODA, so the changes can be propagated over to them
                            if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg && HasActiveODASpecialty)
                            {
                                nextStep = "Check ODA";
                            }
                            else
                            {
                                // not an update or doesn't have oda services; if it's a new registration, we can close the workflow; if not, we need to see if there is anything else in the waiver queue to process
                                nextStep = (registrationStatusTypeID == CON.RegistrationStatusTypeId.NotProcessed) ? "Not Processed" 
                                                      : (workflowEventTypeID == CON.WorkflowEventType.NewReg) ? "Complete Workflow" : "Check Waiver Queue";
                            }

                            toReturn = true; // either option above will result in moving the workflow step forward

                            // else: stay put in this workflow step until we get a closed status for this application

                            //if DODD update, update status in the waiver transaction queue table.
                            if (WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                            {
                                // CR302 Revert changes done on the multiagency provider in case of not processed. For DODD apps DONOT send Not Processed notice
                                if (OtherAppStatusID == CON.ApplicationStatus.Closed_By_ODM && registrationStatusTypeID == CON.RegistrationStatusTypeId.NotProcessed)
                                {
                                    List<SqlParameter> paramc = new List<SqlParameter>();
                                    paramc.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                                    paramc.Add(SqlParms.CreateParameter("USER", DbType.Guid, CON.appAdminUserId, true));
                                    DataAccess.ExecuteStoredProcedure("usp_RevertDDChanges_NotProcessed", paramc);


                                    if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID), out additionalApplicationID))
                                    {
                                        throw new Exception(string.Format(
                                            Constants.LogString.WorkflowParameterNotNumeric,
                                            Assembly.GetExecutingAssembly().GetName().Name,
                                            Constants.ProcessParameter.RegistrationID,
                                            GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID)));
                                    }
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
                }
                else if (canMakeWSRequest.Equals("false"))
                {
					// default step for testing without a live system ... make it a "Check ODA" for now to get it moving to that step for testing
                    nextStep = (workflowEventTypeID == CON.WorkflowEventType.NewReg) ? "Complete Workflow" : "Check ODA";
                    toReturn = true;
                }
                else
                {
                    nextStep = "";
                    toReturn = false;
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
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Closed_as_ODM_Denial_ID);
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
