using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using Workflow;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace PDMSWorkflow
{
    public class WFCompleteWorkflow : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCompleteWorkflow(int processID, int stepID)
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
            int IsAddODMorODAMedicaidSvc = 0;

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
                string cpcDenied = p.GetProcessParameter(Constants.ProcessParameter.IsCPCDenied);
                bool isCPCDenied = string.IsNullOrEmpty(cpcDenied) ? false : Convert.ToBoolean(cpcDenied);

                string ConvertFrmORPWF = p.GetProcessParameter(Constants.ProcessParameter.IsConvertFrmORPWF);
                bool IsConvertFrmORPWF = string.IsNullOrEmpty(ConvertFrmORPWF) ? false : Convert.ToBoolean(ConvertFrmORPWF);

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");


                List<SqlParameter> sql_Parms = new List<SqlParameter>();
                sql_Parms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                string orpFlag = DataAccess.ExecuteScalar("usp_GetFlag_ConvertORP", sql_Parms);

                if (orpFlag == "True" || IsConvertFrmORPWF)
                { 
                    List<SqlParameter> sqlParams = new List<SqlParameter>();
                    sqlParams.Add(new SqlParameter("Reg_ID", regID));
                    sqlParams.Add(new SqlParameter("@Flag", false));
                    DataAccess.ExecuteStoredProcedure("usp_UpdateFlag_ConvertORP", sqlParams, "UpdateORPApplicationFlag");

                    SetProcessParameter(Constants.ProcessParameter.IsConvertFrmORPWF, "1");
                }


                //set workflow event type date

                sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, regID, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_EVENT_INFO", sqlParms, "REgData");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    int wfEventTypeID = 0;
                    int REGEventInfoID = Convert.ToInt32(ds.Tables[0].Rows[0]["REG_EVENT_INFO_ID"]);
                    if (ObjectControllerHelper.HasRows(dsReg))
                    {
                        DataRow dr = dsReg.Tables[0].Rows[0];
                        wfEventTypeID = Methods.GetIntValue(dr["WORKFLOW_EVENT_TYPE_ID"]);
                    }

                    sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    sqlParms.Add(SqlParms.CreateParameter("REG_EVENT_INFO_ID", DbType.Int32, REGEventInfoID, false));
                    if (wfEventTypeID == Constants.WorkflowEventType.NewReg)
                    {
                        sqlParms.Add(SqlParms.CreateParameter("NEW_REG_END_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        //wfEventTypeID = Constants.WorkflowEventType.NewReg;
                    }
                    else if (wfEventTypeID == Constants.WorkflowEventType.RevalReg)
                    {
                        sqlParms.Add(SqlParms.CreateParameter("REVALIDATION_REG_END_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        //wfEventTypeID = Constants.WorkflowEventType.RevalReg;
                    }
                    else if (wfEventTypeID == Constants.WorkflowEventType.UpdateReg)
                    {
                        sqlParms.Add(SqlParms.CreateParameter("UPDATE_REG_END_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        //wfEventTypeID = Constants.WorkflowEventType.UpdateReg;
                    }
                    else if (wfEventTypeID == Constants.WorkflowEventType.CPCReattest)
                    {
                        sqlParms.Add(SqlParms.CreateParameter("CPC_REATTEST_REG_END_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        //wfEventTypeID = Constants.WorkflowEventType.UpdateReg;
                    }
                    sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, false));
                    DataAccess.ExecuteStoredProcedure("updateREG_EVENT_INFO", sqlParms);
                    //RegistrationController.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", regID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", wfEventTypeID.ToString() } });
                }
                dsReg.Tables[0].TableName = "RegData";
                DateTime? termDate = null;
                //int partyID = 0;
                int currRegPgmStatusID = 0;
                int regStatusTypeID = 0;
                int newRegPgmStatusID = Constants.RegistrationProgramStatusTypeId.Maintenance;
                string userID = string.Empty;
                string enrollmentStatusCode = string.Empty;
                int workflowID = 0;
                DateTime? changeEffectiveDate = null;
                int workflowEventTypeID = 0, EntitytypeID = 0, waiverServiceUpdateTypeID = 0;
                string CPCPracticeTypeID = string.Empty;
                string MMISProviderTypeID = string.Empty;
                bool hasIncident = false;
                bool isReactivation = false;
                bool isReapplication = false;
                bool isReconsideration = false;
				
                bool isUpdateCPCContactOnly = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.UpdateCpcContact))
                        ? false : Convert.ToBoolean(p.GetProcessParameter(Constants.ProcessParameter.UpdateCpcContact));
                bool enableCR346 = Convert.ToBoolean(AppSettings.Get("EnableCR346", "false"));
                bool enableCR537 = Convert.ToBoolean(AppSettings.Get("EnableCR537", "false"));

                string RevertSuspension = p.GetProcessParameter(Constants.ProcessParameter.IsRevertSuspension);
                bool isRevertSuspension = string.IsNullOrEmpty(RevertSuspension) ? false : Convert.ToBoolean(RevertSuspension);

                if (ObjectControllerHelper.HasRows(dsReg))
                {
                    DataRow dr = dsReg.Tables[0].Rows[0];
                    //partyID = Methods.GetIntValue(dr["PartyID"]);
                    currRegPgmStatusID = Methods.GetIntValue(dr["RegProgramStatusTypeID"]);
                    regStatusTypeID = Methods.GetIntValue(dr["RegistrationStatusTypeID"]);
                    DateTime tmp;
                    if (DateTime.TryParse(Methods.GetStringValue(dr["TerminationDate"]), out tmp))
                    {
                        termDate = tmp;
                    }
                    enrollmentStatusCode = ObjectControllerHelper.GetString("EnrollmentStatusCode", dr);
                    userID = ObjectControllerHelper.GetString("UserID", dr);
                    workflowID = ObjectControllerHelper.GetInt("WorkflowID", dr);
                    if (DateTime.TryParse(Methods.GetStringValue(dr, "ChangeEffectiveDate"), out tmp))
                    {
                        changeEffectiveDate = tmp;
                    }
                    workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", dr);
                    MMISProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", dr);
                    CPCPracticeTypeID = ObjectControllerHelper.GetString("MMISCPCPracticeTypeID", dr);
                    EntitytypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID", dr);
                    hasIncident = ObjectControllerHelper.GetBool("IS_INCIDENT_ALERT_SET", dr);
                    isReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", dr);
                    isReapplication = ObjectControllerHelper.GetBool("isreapplication", dr);
					waiverServiceUpdateTypeID = ObjectControllerHelper.GetInt("WaiverServiceUpdateTypeID", dr);
					
                    if (workflowEventTypeID == Constants.WorkflowEventType.Reconsideration || workflowEventTypeID == Constants.WorkflowEventType.CredentialReconsideration)
                    {
                        isReconsideration = true;
                    }
                }

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                bool isDoDDIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfDODDInitialApplication", parameters));

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                bool isODAIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfODAInitialApplication", parameters));

                //OHPNM-17463  - if the program status is NotProcessed and if the providers present WF is not marked as "Not Processed" update program status to maintenance
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, true));
                bool isWFMarkedNotProcessed = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfWFMarkedNotProcessed", parameters));

                //Generate agreement
                string fileName;
                ProcessDocumentController.GenerateApplication(regID, userID, true, out fileName);                

                //PRGCR302 - For DD or ODA initial apps started on multiagency provider set the registration status back to what it was before not processed.
                if (isWFMarkedNotProcessed && currRegPgmStatusID == Constants.RegistrationProgramStatusTypeId.NotProcessed && ((workflowEventTypeID == Constants.WorkflowEventType.UpdateReg &&
                    ((waiverServiceUpdateTypeID == Constants.WaiverServiceUpdateType.DODD && isDoDDIntialApp) || (waiverServiceUpdateTypeID == Constants.WaiverServiceUpdateType.ODA && isODAIntialApp)))
                    || (workflowEventTypeID == Constants.WorkflowEventType.UpdateReg && waiverServiceUpdateTypeID == Constants.WaiverServiceUpdateType.ODM && AppSettings.Get("SAM615Enabled") =="true")
                    || (workflowEventTypeID == Constants.WorkflowEventType.RevalReg && isReactivation != true && isReapplication != true && AppSettings.Get("SAM615Enabled") == "true")
                    )
                    )
                {
                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                    DataAccess.ExecuteStoredProcedure("usp_RevertRegistrationStatus_NotProcessed", parameters);
                }
                else
                {
                    if (termDate.HasValue || ((currRegPgmStatusID == Constants.RegistrationProgramStatusTypeId.Denied || 
                        (currRegPgmStatusID == Constants.RegistrationProgramStatusTypeId.Suspended && enableCR537
                              && workflowEventTypeID == Constants.WorkflowEventType.UpdateReg && !isRevertSuspension)) && !(isReactivation || isReapplication || isReconsideration))
                        )  //SAM537 Retain provider Program status on Suspended if suspened prov does ODM update
                    {
                        newRegPgmStatusID = currRegPgmStatusID;

                        //OHPNM-14357:Updating RegistrationStatusTypeId for terminated providers
                        int RegStatusTypeID = getRegStatusByRegProgramStatus(currRegPgmStatusID);

                        if (RegStatusTypeID > 0 && RegStatusTypeID != regStatusTypeID)
                        {
                            Dictionary<string, string> parms = new Dictionary<string, string>();
                            parms.Add("REG_ID", regID.ToString());
                            parms.Add("REGISTRATION_STATUS_TYPE_ID", RegStatusTypeID.ToString());
                            parms.Add("REG_PROGRAM_STATUS_TYPE_ID", newRegPgmStatusID.ToString());
                            RegistrationController.UpdateRegistrationCustom(parms);
                        }

                    }
                    if (currRegPgmStatusID == Constants.RegistrationProgramStatusTypeId.MonthlyScreening
                        || (workflowID == Constants.WorkflowType.PeriodicDatabaseChecks && !termDate.HasValue && currRegPgmStatusID != Constants.RegistrationProgramStatusTypeId.Terminated))
                    {
                        DataSet dsWorkflow = RegistrationController.GetWorkflowProcessesForRegID(regID);
                        if (ObjectControllerHelper.HasRows(dsWorkflow))
                        {
                            DataTable dtWorkflow = dsWorkflow.Tables[0];
                            bool HasWFnotPeriodic = dtWorkflow.AsEnumerable()
                                               .Where(x => (Convert.ToInt32(x["WORKFLOW_ID"]) != Constants.WorkflowType.PeriodicDatabaseChecks))
                                               .Count() > 0 ? true : false;

                            newRegPgmStatusID = HasWFnotPeriodic ? Constants.RegistrationProgramStatusTypeId.Maintenance : Constants.RegistrationProgramStatusTypeId.Conversion;
                        }
                    }
                    if (currRegPgmStatusID != newRegPgmStatusID && !isWFMarkedNotProcessed)
                    {
                        RegistrationController.SaveRegistrationProgramStatus(regID, newRegPgmStatusID, DateTime.Now, Constants.appAdminUserId);
                    }
                }

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.String, ProcessID, false));
                DataSet dsProcess = DataAccess.ExecuteStoredProcedure("usp_WF_SelectProcessParameters", parameters, "ProcessDS");

				if (dsProcess.Tables.Count > 0 && dsProcess.Tables[0].Rows.Count > 0)
				{
					DataRow myRow = dsProcess.Tables[0].Rows[0];
					for (int j = 0; j < dsProcess.Tables[0].Columns.Count; j++)
					{
						if (dsProcess.Tables[0].Columns[j].ColumnName.ToString() == "ISADD_ODM_ODA_MEDICAID_SVC")
						{
							if (myRow.ItemArray[j] != null && myRow.ItemArray[j].ToString() == "True")
							{
								IsAddODMorODAMedicaidSvc = 1;
							}
							break;
						}
					}
				}
                if (IsAddODMorODAMedicaidSvc == 1)
                {
                    IsAddODMorODAMedicaidSvc = 0;
                    RegistrationController.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", regID.ToString() }, { "ISADD_ODM_ODA_MEDICAID_SVC", IsAddODMorODAMedicaidSvc.ToString() } });
                }
                if (WorkflowID == Constants.WorkflowType.CPC && !isCPCDenied && !isUpdateCPCContactOnly)
                {
                    List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                    sqlParms1.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    sqlParms1.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, AppSettings.Get("CPCProgramYear"), false));
                    sqlParms1.Add(SqlParms.CreateParameter("CPC_PRACTICE_TYPE", DbType.String, CPCPracticeTypeID, false));
                    sqlParms1.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                    sqlParms1.Add(SqlParms.CreateParameter("MMIS_PROVIDER_TYPE_ID", DbType.String, MMISProviderTypeID, false));
                    sqlParms1.Add(SqlParms.CreateParameter("ENTITY_TYPE_ID", DbType.Int32, EntitytypeID, false));
                    sqlParms1.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, false));
                    sqlParms1.Add(SqlParms.CreateParameter("USER", DbType.Guid, Constants.appAdminUserId, false));
                    sqlParms1.Add(SqlParms.CreateParameter("WORKFLOW_EVENT_TYPE_ID", DbType.Int32, workflowEventTypeID, false));
                    DataAccess.ExecuteStoredProcedure("usp_InsertCPCPracticeListHistory", sqlParms1);
                }
                if (WorkflowID == Constants.WorkflowType.CPC)
                {
                    RegistrationController.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", regID.ToString() }, { "IS_CPC_LINK_REENABLED", "0" } });
                }

                if (WorkflowID == Constants.WorkflowType.CMC && workflowEventTypeID != Constants.WorkflowEventType.CMCUpdate)
                {
                    List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                    sqlParms1.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    sqlParms1.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                    sqlParms1.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, false));
                    sqlParms1.Add(SqlParms.CreateParameter("USER", DbType.Guid, Constants.appAdminUserId, false));
                    DataAccess.ExecuteStoredProcedure("usp_UpdateCMCProcessIDByRegId", sqlParms1);
                }

                if (WorkflowID == Constants.WorkflowType.CPC && isUpdateCPCContactOnly)
                {
                    List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                    sqlParms1.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    sqlParms1.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                    sqlParms1.Add(SqlParms.CreateParameter("UPD_CPC_CONTACT", DbType.Boolean, isUpdateCPCContactOnly, false));
                    sqlParms1.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, AppSettings.Get("CPCProgramYear"), false));
                    sqlParms1.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    sqlParms1.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, false));
                    DataAccess.ExecuteStoredProcedure("updateREG_CPC_ENROLLMENTcustom", sqlParms1);
                }


                #region OHPNM-5035
                if (newRegPgmStatusID == Constants.RegistrationProgramStatusTypeId.Maintenance)
                {
                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    parameters.Add(SqlParms.CreateParameter("SENT_TO_SI", DbType.Boolean, true, false));
                    parameters.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, Constants.RegistrationModifiedStatusType.Changed, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Guid.Parse(Constants.appAdminUserId), false));

                    DataAccess.ExecuteScalar("usp_Update_REG_SPECIALTY_SI", parameters);


                    //FED DEA Registration
                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    parameters.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, Constants.RegistrationModifiedStatusType.NoChange, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Guid.Parse(Constants.appAdminUserId), false));

                    DataAccess.ExecuteScalar("usp_Update_REG_DEA_MODIFIED_STATUS_TYPE", parameters);

                    //Medicare
                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    parameters.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, Constants.RegistrationModifiedStatusType.NoChange, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Guid.Parse(Constants.appAdminUserId), false));

                    DataAccess.ExecuteScalar("usp_Update_REG_MEDICARE_MODIFIED_STATUS_TYPE", parameters);

                    //CLIA certifications
                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    parameters.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, Constants.RegistrationModifiedStatusType.NoChange, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Guid.Parse(Constants.appAdminUserId), false));

                    DataAccess.ExecuteScalar("usp_Update_REG_CLIA_MODIFIED_STATUS_TYPE", parameters);
                }

                //revert Registration table statuses
                if (hasIncident)
                {
                    RegistrationController.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", regID.ToString() }, { "IS_INCIDENT_ALERT_SET", "false" } });
                }

                // revert isReactivation if applicable
                if (isReactivation)
                {
					Dictionary<string, string> parms = new Dictionary<string, string>();
					parms.Add("REG_ID", regID.ToString());
					parms.Add("ISReactivation", "false");
					parms.Add("POSSIBLE_REVAL_DATE", null); // also, delete POSSIBLE_REVAL_DATE
					RegistrationController.UpdateRegistration(parms);					
                }
				
				// revert isReapplication if applicable
				if (isReapplication) {
					Dictionary<string, string> parms = new Dictionary<string, string>();
					parms.Add("REG_ID", regID.ToString());
					parms.Add("ISReapplication", "false");
					RegistrationController.UpdateRegistration(parms);	
				}
                               
                // OHPNM-9065
				if (waiverServiceUpdateTypeID > 0) {
                    // need to set the REGISTRATION waiverServiceUpdateTypeID back to null
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", regID.ToString());
                    parms.Add("WAIVER_SERVICE_UPDATE_TYPE_ID", "0");
                    // OHPNM-11192 - If DD Update WF on suspended provider set back registration status to suspended
                    if (enableCR346 && newRegPgmStatusID == Constants.RegistrationProgramStatusTypeId.Suspended && workflowEventTypeID == Constants.WorkflowEventType.UpdateReg
                         && waiverServiceUpdateTypeID == Constants.WaiverServiceUpdateType.DODD)
                    {
                        parms.Add("REGISTRATION_STATUS_TYPE_ID", Constants.RegistrationStatusTypeId.Suspended.ToString());
                    }
                    RegistrationController.UpdateRegistrationCustom(parms);
                }

                #endregion
                #region OHPNM-9390
                if (p.GetProcessParameter(Constants.ProcessParameter.IsProviderDisenrolling).Equals("True"))
                {
                    List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                    sqlParms2.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    sqlParms2.Add(SqlParms.CreateParameter("IS_PROCESSED", DbType.Boolean, true, true));

                    DataAccess.ExecuteStoredProcedure("usp_UpdateREG_PROVIDER_DISENROLLMENT", sqlParms2);
                }

                #endregion
                
                //PRGCR313
                if (workflowEventTypeID == Constants.WorkflowEventType.ChangeProviderType)
                {
                    int status = termDate.HasValue ? Constants.ProviderTypeChangeRequestStatus.Cancelled : Constants.ProviderTypeChangeRequestStatus.Complete;
                    RegistrationController.UpdateProviderTypeChangeStatus(regID, status, DateTime.Now, Constants.appAdminUserId);
                }
                
				ds = UserController.SelectRegistrationStatuses(Constants.appAdminUserId, regID);
				if (ds.Tables.Count == 0)
				{
					throw new Exception(string.Format(
					Constants.LogString.WorkflowError,
						Assembly.GetExecutingAssembly().GetName().Name, "Error when migrating Registration " + regID.ToString() +
						". Expected TennCareStatusID from usp_SelectRegistrationStatuses when migrating registration data to PDMS Core"));
				}
				
				SetProcessParameter(Constants.ProcessParameter.RegistrationStatusTypeID, ds.Tables[0].Rows[0]["REGISTRATION_STATUS_TYPE_ID"].ToString());
				SaveProcessParameters();
				toReturn = true;
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

        public int getRegStatusByRegProgramStatus(int regProgramStatus)
        {
            int regStatusTypeId = 0;

            if (regProgramStatus == Constants.RegistrationProgramStatusTypeId.Suspended)
            {
                regStatusTypeId = Constants.RegistrationStatusTypeId.Suspended;
            }
            else if (regProgramStatus == Constants.RegistrationProgramStatusTypeId.Disenrolled)
            {
                regStatusTypeId = Constants.RegistrationStatusTypeId.Disenrolled;
            }
            else if (regProgramStatus == Constants.RegistrationProgramStatusTypeId.Denied)
            {
                regStatusTypeId = Constants.RegistrationStatusTypeId.Deny;
            }
            else if(regProgramStatus == Constants.RegistrationProgramStatusTypeId.Terminated)
            {
                regStatusTypeId = Constants.RegistrationStatusTypeId.TerminateProvider;
            }
            return regStatusTypeId;

        }

    }
}
