using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.Presentation.PDMS
{
    public class ProviderManagementDetailsPresenter : PresenterBase, IPresenter<IProviderManagementDetailsView, ProviderManagementData>
    {
        private IProviderManagementDetailsView view = null;

        #region svc
        private PDMSService.PDMSServiceClient _svc;
        private PDMSService.PDMSServiceClient svc
        {
            get
            {
                if (_svc == null)
                {
                    _svc = new PDMSService.PDMSServiceClient();
                }

                return _svc;
            }
        }
        #endregion

        public ProviderManagementDetailsPresenter(IProviderManagementDetailsView view)
        {
            Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        public void Init()
        {
            this.view.Model = new ProviderManagementData();
        }


        public void Init(int regID)
        {
            this.view.Model = new ProviderManagementData();
            this.view.Model.RegID = regID;
            RequestRegistrationData();
        }

        public void RequestRegistrationData()
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }
                DataSet ds = svc.SelectRegistrationByRegID(view.Model.RegID);
                ProviderManagementData details = new ProviderManagementData();
                details.LoadObjectFromDataset(ds);
                if (details.RegistrationProgramStatusTypeID == Constants.RegistrationProgramStatusTypeId.EnforceMoratoria)
                {
                    DataSet moratoriaData = PerformMoratoriaRematch();
                    if (hasErrors)
                    {
                        view.SetErrorMessages();
                        return;
                    }
                    if (Methods.HasRows(moratoriaData))
                    {
                        DateTime? beginDate = null;
                        DateTime? endDate = null;

                        DataRow dr = moratoriaData.Tables[0].Rows[0];
                        if (dr["MoratoriaBeginDate"] != DBNull.Value)
                        {
                            beginDate = Methods.GetDateValue(dr["MoratoriaBeginDate"]);
                        }
                        if (dr["MoratoriaEndDate"] != DBNull.Value)
                        {
                            endDate = Methods.GetDateValue(dr["MoratoriaEndDate"]);
                        }
                        if (Methods.GetIntValue(dr["UpdatedRegistration"]) == 1)
                        {
                            //reget registration information as current step and registration statuses have now changed.
                            //reget to ensure getting all updated information (now and in future)
                            ds = svc.SelectRegistrationByRegID(view.Model.RegID);
                            details = new ProviderManagementData();
                            details.LoadObjectFromDataset(ds);
                        }
                        if (beginDate.HasValue)
                        {
                            details.MoratoriaBeginDate = beginDate.Value;
                        }
                        if (endDate.HasValue)
                        {
                            details.MoratoriaEndDate = endDate.Value;
                        }
                    }
                }
                string revalDueWindow = DataAccess.GetAppSetting("RevalidationDueWindow");
                if (string.IsNullOrEmpty(revalDueWindow))
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RevalidationAppSettingsRequired);
                    view.SetErrorMessages();
                    return;
                }

                details.RevalidationDueWindow = Convert.ToInt32(revalDueWindow) * -1;
                //update regeventinfotypeid -- workflowtype will be set based on the workflow_event_type_id in registration table
                /*DataSet ds1 = svc.SelectRegistrationData(view.Model.RegID, "EVENT_INFO");
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds1.Tables[0].Rows[0];
                    
                    bool ConvertedProvider = (details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion);
                    bool revalidationNeeded = !details.TerminationDate.HasValue && ((details.RevalidationDate.HasValue && details.RevalidationDate.Value.AddDays(details.RevalidationDueWindow) <= DateTime.Today) || (details.EndDate == null && details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion));

                    if (!revalidationNeeded && details.PartyID > 0)
                        details.WorkflowEventType = CON.WorkflowEventType.UpdateReg;
                    else if (revalidationNeeded)
                        details.WorkflowEventType = CON.WorkflowEventType.RevalReg;
                    else if (!revalidationNeeded)
                        details.WorkflowEventType = CON.WorkflowEventType.NewReg;

                    //details.WorkflowEventType = revalidationNeeded ? CON.WorkflowEventType.RevalReg : (details.PartyID == 0 ? CON.WorkflowEventType.NewReg : CON.WorkflowEventType.UpdateReg);
                }*/
                view.SetProviderDetails(details);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public DataSet PerformMoratoriaRematch()
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return null;
                }

                DataSet ds = svc.PerformMoratoriaRematch(view.Model.RegID, DateTime.Now, new Guid(Constants.appAdminUserId));
                return ds;
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
                return null;
            }
        }
        public void CreateNewCPCRegistration(int regID, string CPCLinkType, string CPC_Program_Year, Guid userID, string CPC_Practice_Type)
        {
            try
            {
                int cpcRegID = svc.InsertNewCPCProviderRegistration(regID, CPCLinkType, CPC_Program_Year, userID, CPC_Practice_Type);

                if (cpcRegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.INSERT_FAILED);
                    view.SetErrorMessages();
                    return;
                }

                //save reg_event_info
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", cpcRegID.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", userID.ToString());
                parms.Add("NEW_REG_START_DATE_TIME", DateTime.Now.ToString());

                svc.InsertRegistrationDataTable("EVENT_INFO", parms);
                svc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", cpcRegID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", CON.WorkflowEventType.NewReg.ToString() } });

                //update model with new workflow values
                DataSet ds = svc.SelectRegistrationByRegID(cpcRegID);
                ProviderManagementData details = new ProviderManagementData();
                details.LoadObjectFromDataset(ds);

                view.BeginNewWorkflow(details);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }

        }

        public void TransferLiveToReg(bool? changeApplicationTypeID = false, bool isProviderDisenrolling = false, int WaiverServiceUpdate = 0, string CPCProgramYear = null, bool isUpdateCPCContact = false, bool isTerminated = false)
        {
            try
            {
               
                CheckForRequiredFields();

                if (this.ErrorList.Count > 0)
                {
                    view.SetErrorMessages();
                    return;
                }

                
                //INSERT INTO VERSION Tables....
                DataSet ds = svc.InsertIntoVersionTables(view.Model.RegID, view.Model.UserID.ToString());

                DataRow dr;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dr = ds.Tables[0].Rows[0];
                    if (!string.IsNullOrEmpty(Methods.GetStringValue(dr["ErrorMessage"])))
                    {
                        Logging log = new Logging(Guid.NewGuid(), string.Empty );
                        log.CreateLogEntry(string.Format("{0} {1}", "Setting up Workflow", Methods.GetStringValue(dr["ErrorMessage"])), Logging.LogPriority.Error);
                        this.ErrorList.Add(NextErrorKey(), Methods.GetStringValue(dr["ErrorMessage"]));
                        this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.CopyToVersionTablesFailed);
                        view.SetErrorMessages();
                        return;
                    }
                }


                ProviderManagementData data = new ProviderManagementData();
                data.RegID = view.Model.RegID;

                //PAPER APPLICATION Change
                if (view.Model.PaperRequestQueueID > 0)
                {
                    //update paper request xref and paper request
                    svc.LinkPaperRequestQueueToReg(view.Model.PaperRequestQueueID, data.RegID, view.Model.LastModifiedDate.Value, view.Model.LastModifiedUser.Value);
                }

                if (view.Model.IsProviderReapplication)
                {
                    Dictionary<string, string> parms1 = new Dictionary<string, string>();


                    parms1.Add("REG_ID", data.RegID.ToString());
                    parms1.Add("ISReapplication", "1");
                    svc.UpdateRegistration(parms1);

                }
                //if RE INSTATEMENT/ REACTIVATION OF TERMINATED PROVIDER
                if (view.Model.IsProviderReactivation)
                {
                    if (data.RegID == 0)
                        data.RegID = view.Model.RegID;

                    DataSet dsReg = svc.UpdateRegForReactivateByProvider(data.RegID, DateTime.Now, view.Model.LastModifiedUser.ToString(), 0);

                    if (dsReg == null || dsReg.Tables.Count == 0)
                    {
                        this.ErrorList.Add(NextErrorKey(), "Reactivation Failed");
                        view.SetErrorMessages();
                        return;
                    }

                    DataTable dtReg = new DataTable();
                    if (dsReg.Tables.Count == 1)
                        dtReg = dsReg.Tables[0];
                    if (dsReg.Tables.Count > 1)
                        dtReg = dsReg.Tables[1];

                    if (dtReg.Rows.Count == 0)
                    {
                        this.ErrorList.Add(NextErrorKey(), "Reactivation Failed");
                        view.SetErrorMessages();
                        return;
                    }

                    string revalDueWindow = DataAccess.GetAppSetting("RevalidationDueWindow");
                    if (string.IsNullOrEmpty(revalDueWindow))
                    {
                        this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RevalidationAppSettingsRequired);
                        view.SetErrorMessages();
                        return;
                    }
                    if (Methods.ColumnExists("Reg_Id", dtReg.Rows[0]))
                        data.RegID = Methods.GetIntValue(dtReg.Rows[0]["Reg_Id"]);



                    if (Methods.ColumnExists("End_Date", dtReg.Rows[0]))
                        view.Model.RevalidationDate = Methods.GetDateValue(dtReg.Rows[0]["End_Date"]);

                    int RevalidationDueWindow = Convert.ToInt32(revalDueWindow) * -1;

                    bool revalidationNeeded = (view.Model.RevalidationDate.HasValue && view.Model.RevalidationDate.Value.AddDays(RevalidationDueWindow) <= DateTime.Today);
                    view.Model.WorkflowEventType = revalidationNeeded && view.Model.ApplicationTypeID != CON.ApplicationType.CPC ? CON.WorkflowEventType.RevalReg : CON.WorkflowEventType.UpdateReg;
                }
                DataSet ds1 = svc.SelectRegistrationData(data.RegID, "EVENT_INFO");
                int REGEventInfoID = 0;

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", data.RegID.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());
                if (view.Model.WorkflowEventType == CON.WorkflowEventType.NewReg)
                {
                    parms.Add("NEW_REG_START_DATE_TIME", DateTime.Now.ToString());
                }
                else if (view.Model.WorkflowEventType == CON.WorkflowEventType.RevalReg)
                {
                    parms.Add("REVALIDATION_REG_START_DATE_TIME", DateTime.Now.ToString());

                }
                else if (view.Model.WorkflowEventType == CON.WorkflowEventType.UpdateReg)
                {
                    parms.Add("UPDATE_REG_START_DATE_TIME", DateTime.Now.ToString());
                }
                else if (view.Model.WorkflowEventType == CON.WorkflowEventType.CPCReattest)
                {
                    parms.Add("CPC_REATTEST_REG_START_DATE_TIME", DateTime.Now.ToString());
                }
                //if NOT RE INSTATEMENT/ REACTIVATION OF TERMINATED PROVIDER
                if (!view.Model.IsProviderReactivation)
                {
                    //save old change effective date and end date into reg_enrollment
                    DataSet ds2 = svc.SelectRegistrationData(data.RegID, "ENROLLMENT");
                }

                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    REGEventInfoID = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_EVENT_INFO_ID"]);
                    parms.Add("REG_EVENT_INFO_ID", REGEventInfoID.ToString());
                    svc.UpdateRegistrationDataTable("EVENT_INFO", parms);
                }
                else
                {
                    svc.InsertRegistrationDataTable("EVENT_INFO", parms);
                }
                // OHPNM-18300 update waiver service update type id for all wfs
                svc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", data.RegID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", view.Model.WorkflowEventType.ToString() },{ "ISADD_ODM_ODA_MEDICAID_SVC", view.Model.IsAddODMorODAMedicaid.ToString() },{ "WAIVER_SERVICE_UPDATE_TYPE_ID", WaiverServiceUpdate.ToString() } });
                //change provider type application type taxonomy type if application type changed
                if (changeApplicationTypeID == true)
                {
                    svc.UpdateToConvertToFeeForService(data.RegID);
                }
                int EntryTaskID = 1;                

                //Reset Application Fee
                if (view.Model.IsRevalidation)
                {
                    svc.ResetApplicationFee(data.RegID);
                }
                Logging log2 = new Logging(new Guid("FDA76E16-1175-4603-AB14-21C2DE368723"), string.Empty);
                //Spawn new workflow
                if ((view.Model.ApplicationTypeID == CON.ApplicationType.Waiver &&
                    (view.Model.WaiverTypeID == CON.WaiverApplicationTypeID.ODA
                     || view.Model.WaiverTypeID == CON.WaiverApplicationTypeID.DODD
                     || view.Model.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD) && WaiverServiceUpdate != CON.WaiverServiceUpdateType.ODM)
                     || (WaiverServiceUpdate == CON.WaiverServiceUpdateType.DODD || WaiverServiceUpdate == CON.WaiverServiceUpdateType.ODA))
                {
                    log2.CreateLogEntry(string.Format("Setting up Workflow EntryTaskID {0} for RegID {1} with ApplicationTypeID {2} and WaiverTypeID {3} and WaiverServiceUpdate {4}", 
                        EntryTaskID, view.Model.RegID, view.Model.ApplicationTypeID, view.Model.WaiverTypeID, WaiverServiceUpdate), Logging.LogPriority.Error);

                    EntryTaskID = CON.WaiverEntryTaskID;
                    if (Methods.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) || WaiverServiceUpdate == CON.WaiverServiceUpdateType.OperatorUpdate || ((view.Model.WaiverTypeID != CON.WaiverApplicationTypeID.NonMedicaidDODD) && view.Model.WorkflowEventType == CON.WorkflowEventType.RevalReg))
                    {
                        // For revalidations it needs to go to provider data entry steps for revalidations
                        EntryTaskID = 1;
                    }
                }
                if (view.Model.WorkflowID == CON.WorkflowType.CHOP)
                {
                    EntryTaskID = CON.CHOPEntryTaskID;
                }
                string userid = view.Model.UserID.ToString();
                if (view.Model.WorkflowEventType == CON.WorkflowEventType.Reconsideration)
                {
                    EntryTaskID = CON.ReconsiderationLevelEntryTaskID;

                    //userid = null;
                    svc.WF_SaveProcessParameter(data.ProcessID, CON.ProcessParameter.IsSendHistoryinTxn, "1");
                }
                if (view.Model.WorkflowID == CON.WorkflowType.CPC)
                {
                    EntryTaskID = CON.CPCEntryTaskID;
                }
                if (view.Model.WorkflowID == CON.WorkflowType.CMC)
                {
                    EntryTaskID = CON.CMCEntryTaskID;
                }
                log2.CreateLogEntry(string.Format("Setting up Workflow EntryTaskID {0} for RegID {1}", EntryTaskID, view.Model.RegID), Logging.LogPriority.Error);

                Workflow.Process pr = new Workflow.Process(view.Model.WorkflowID, userid, Guid.NewGuid(), EntryTaskID);
                if (pr == null)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.CreationOfNewWorkflowFailed);
                    view.SetErrorMessages();
                    return;
                }

                data.ProcessID = pr.ProcessID;
                data.CurrentStepID = pr.CurrentStepID;
                data.CurrentTaskID = pr.TaskID;
                data.WorkflowID = pr.WorkflowID;

                // Save the Registration ID as a process parameter of the Workflow
                svc.WF_SaveProcessParameter(data.ProcessID, "REGISTRATION_ID", data.RegID.ToString());
                svc.WF_SaveProcessParameter(data.ProcessID, "WORKFLOW_EVENT_TYPE_ID", view.Model.WorkflowEventType.ToString());
                if (isUpdateCPCContact)
                {
                    svc.WF_SaveProcessParameter(data.ProcessID, "UPDATE_CPC_CONTACT", "1");
                }
                svc.WF_SaveProcessParameter(data.ProcessID, "WAIVER_SERVICE_UPDATE_TYPE_ID", (view.Model.WorkflowEventType == CON.WorkflowEventType.UpdateReg)?  WaiverServiceUpdate.ToString() : "0");
                if (isProviderDisenrolling)
                {
                    svc.WF_SaveProcessParameter(data.ProcessID, "IS_PROVIDER_DISENROLLING", "1");
                }
                if(view.Model.IsProviderReapplication)
                {
                    svc.WF_SaveProcessParameter(data.ProcessID, CON.ProcessParameter.IsReapplication, "1");
                    svc.WF_SaveProcessParameter(data.ProcessID, CON.ProcessParameter.IsSendHistoryinTxn, "1");
                }

                svc.InsertRegApplicationRecord(data.RegID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), data.ProcessID);

                if (view.Model.WorkflowID == CON.WorkflowType.CPC)
                {
                    //update Process ID to cpc enrollment table
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("REG_ID", data.RegID.ToString());
                    param.Add("PROCESS_ID", data.ProcessID.ToString());
                    param.Add("UPD_CPC_CONTACT", isUpdateCPCContact.ToString());
                    param.Add("CPC_PROGRAM_YEAR", CPCProgramYear);
                    param.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    param.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());
                    svc.UpdateRegistrationDataTable("CPC_ENROLLMENTcustom", param);

                    if (view.Model.WorkflowEventType == CON.WorkflowEventType.CPCReattest)
                    {
                        param = new Dictionary<string, string>();
                        param.Add("REG_ID", data.RegID.ToString());
                        param.Add("CPC_PRACTICE_TYPE", view.Model.MMISCPCPracticeTypeID);
                        param.Add("CPC_PROGRAM_YEAR", CPCProgramYear);
                        param.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        param.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());
                        svc.UpdateRegistrationDataWithParams("usp_UpdateCPCAffiliation_onReattest", param);
                    }
                }

                #region OHPNM-5224
                if (changeApplicationTypeID == true) 
                {
                    // ohpnm-18353 need to end date orp specialty span and add the Standard Specailty Span based on dates set in NPI MedID page.
                    svc.WF_SaveProcessParameter(data.ProcessID, CON.ProcessParameter.IsConvertFrmORPWF, "1");
                }
                #endregion

                if (isProviderDisenrolling)
                {
                    //if disenrolling, auto submit the update
                    Dictionary<string, string> parmsDisEn = new Dictionary<string, string>();
                    parmsDisEn.Add("REG_ID", data.RegID.ToString());
                    parmsDisEn.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parmsDisEn.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());
                    parmsDisEn.Add("REGISTRATION_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationStatusTypeId.Submitted.ToString());
                    parmsDisEn.Add("SUBMIT_DATE_TIME", DateTime.Now.ToString());
                    svc.UpdateRegistration(parmsDisEn);
                    svc.WF_TakeAction(data.ProcessID, "Submit Update", string.Empty);
                }

                //OHPNM-7670
                if(view.Model.IsRevalidation || view.Model.WorkflowEventType == CON.WorkflowEventType.CMCReAttest || view.Model.WorkflowEventType == CON.WorkflowEventType.CPCReattest)
                {
                    List<SqlParameter> paramReval = new List<SqlParameter>();
                    paramReval.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, data.RegID, false));
                    paramReval.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    paramReval.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Guid.Parse(Constants.appAdminUserId), false));
                    paramReval.Add(SqlParms.CreateParameter("SUBMIT_DATE_TIME", DbType.DateTime, DBNull.Value, true));
                    DataAccess.ExecuteScalar("usp_UpdateSubmitDateByRegID_ForRevalidation", paramReval);
                }
                //OHPNM-14823
                if((view.Model.IsRevalidation || view.Model.WorkflowEventType == CON.WorkflowEventType.UpdateReg) && isTerminated)
                {
                    svc.WF_SaveProcessParameter(data.ProcessID, CON.ProcessParameter.IsSendHistoryinTxn, "1");
                }

                view.BeginNewWorkflow(data);

            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void CancelRegistration(int processId, string commandName = "")
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", view.Model.RegID.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", view.Model.LastModifiedDate.ToString());
                parms.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());

                svc.CancelRegistration(view.Model.RegID, view.Model.LastModifiedDate.Value, view.Model.LastModifiedUser.Value, processId, commandName);

                // Return with cancel success
                view.CompleteCancelWorkflow();
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void CancelWorkflow()
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }

                // Cancel Workflow Proceses
                svc.WF_CancelWorkflowProcess(view.Model.RegID);

                // Return with cancel success
                view.CompleteCancelWorkflow();

            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void CancelCPCRegistration(int processId, int WorkflowEventTypeID)
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }

                svc.CancelRegistrationCPC(view.Model.RegID, view.Model.LastModifiedDate.Value, view.Model.LastModifiedUser.Value, processId, WorkflowEventTypeID);

                // Return with cancel success
                view.CompleteCancelWorkflow();
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        private void CheckForRequiredFields()
        {
            if (view.Model == null || view.Model.RegID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
            }

            if (!view.Model.ChangeEffectiveDate.HasValue && view.Model.WorkflowEventType != CON.WorkflowEventType.Reconsideration)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ChangeEffectiveDateMustBeDate);
            }

            if (!view.Model.RequestedEffectiveDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.RequestedEffectiveDateMustBeDate);
            }
        }


    }
}