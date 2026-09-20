using Corp.Core.Libraries.Helper;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

// TODO: Step 3 from New Registration Visio
namespace PDMSWorkflow
{
    public class WFPromoteToActive : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        int OperatorRegID = 0;
        DateTime CPC_Prog_Year = DateTime.ParseExact(AppSettings.Get("CPCProgramStartDate", string.Empty) + "/" + AppSettings.Get("CPCProgramYear"), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        public WFPromoteToActive(int processID, int stepID)
            : base(processID, stepID)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool returnValue = false;
            string partyId = string.Empty;
            string serviceLocationId = string.Empty;

            int registrationId = 0;
            try
            {
                Workflow.Process p = new Workflow.Process(ProcessID);
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out registrationId))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, registrationId, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;

                if (drReg == null)
                {
                    throw new Exception(string.Format(Constants.LogString.WorkflowError, Assembly.GetExecutingAssembly().GetName().Name,
                                "Unable to retrieve registration using usp_SelectREGISTRATIONByRegId."));
                }

                string mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
                int applicationTypeIDmn = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                int workflowIDmn = ObjectControllerHelper.GetInt("WorkflowID", drReg);
                int WaiverTypeIDmn = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                int wfEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);


                if ((mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR))
                    && workflowIDmn != CON.WorkflowType.CHOP) //LTC 86 or 89
                {
                    OperatorRegID = registrationId;
                    PromoteToActive(registrationId, p);
                    registrationId = RegistrationController.SelectBuildingRegistration(registrationId);
                    if (registrationId != 0)
                    {
                        PromoteToActive(registrationId, p);
                    }
                }
                else
                {
                    PromoteToActive(registrationId, p);
                }
                returnValue = true;
               
                if (workflowIDmn == CON.WorkflowType.CPC && wfEventTypeID == CON.WorkflowEventType.UpdateReg)
                    nextStep = "Update";
                else
                    nextStep = "Next";

            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, "Registration Id: " + registrationId.ToString() + " - " + logMsg);
            }
            return returnValue;
        }

        private void PromoteToActive(int registrationId, Workflow.Process p)
        {
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, registrationId, false));
            DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");

            dsReg.Tables[0].TableName = "RegData";
            DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;

            if (drReg == null)
            {
                throw new Exception(string.Format(Constants.LogString.WorkflowError, Assembly.GetExecutingAssembly().GetName().Name,
                            "Unable to retrieve registration using usp_SelectREGISTRATIONByRegId."));
            }

            bool setRevalidationDate = false;
            bool updateRevalidationDate = false;
            int regProgramStatusTypeID = ObjectControllerHelper.GetInt("RegProgramStatusTypeID", drReg);
            string mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
            string enrollmentStatusCode = ObjectControllerHelper.GetString("EnrollmentStatusCode", drReg);
            int enrollmentStatusReasonID = ObjectControllerHelper.GetInt("EnrollmentStatusReasonID", drReg);
            bool isReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", drReg);
            bool isReapplication = ObjectControllerHelper.GetBool("isreapplication", drReg); 
            DateTime reqEffDate = ObjectControllerHelper.GetDateTime("RequestedEffectiveDate", drReg);
            DateTime submitDateTime = ObjectControllerHelper.GetDateTime("SubmitDateTime", drReg);
            bool retroEffDate = ObjectControllerHelper.GetBool("RetroEffectiveDate", drReg);
            int workflowID = ObjectControllerHelper.GetInt("WORKFLOWID", drReg);
            int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
            int ReconsiderationworkflowEventTypeID = 0;
            string npi = ObjectControllerHelper.GetString("NPI", drReg);
            bool changedApplicationTypeID = ObjectControllerHelper.GetBool("CHANGED_APPLICATION_TYPE", drReg);
            DateTime EndDateTime = ObjectControllerHelper.GetDateTime("EndDate", drReg);
            DateTime RevalidationDatePriorToTermination = ObjectControllerHelper.GetDateTime("Revalidation_Date_Prior_To_Termination", drReg);
            string medicaidid = ObjectControllerHelper.GetString("MedicaidID", drReg);
            int referralID = ObjectControllerHelper.GetInt("DIDD_REFERRAL_ID", drReg);
            int referralTypeID = ObjectControllerHelper.GetInt("DIDD_REFERRAL_TYPE_ID", drReg);
            int applicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
            DateTime oldtermDate = ObjectControllerHelper.GetDateTime("TerminationDate", drReg);
            string MMISProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
            string CPCPracticeTypeID = ObjectControllerHelper.GetString("MMISCPCPracticeTypeID", drReg);
            int EntitytypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID", drReg);
            bool requiresCredentialing = ObjectControllerHelper.GetBool("IsCredentialingProvider", drReg);
            DateTime EnrollmentSpecialistApprovalDate = ObjectControllerHelper.GetDateTime("NEW_REG_ENROLL_SPECIALIST_APPROVAL_DTE", drReg);
            DateTime regChangeEffectiveDate = ObjectControllerHelper.GetDateTime("ChangeEffectiveDate", drReg);
			DateTime possibleRevalDate = ObjectControllerHelper.GetDateTime("POSSIBLE_REVAL_DATE", drReg);
            DateTime credentialApprovalDate;
            //DateTime? credentialApprovalDate = null;
            DateTime? operatorRegistrationChangeEffectiveDate = null; // OHPNM-5361
            DateTime doddStartDate;
			bool isRevalRegOfTerminatedProvider = (!ObjectControllerHelper.IsDateNull(oldtermDate) && workflowEventTypeID == CON.WorkflowEventType.RevalReg); // if it has a term date and is a revalreg
            DateTime? revalidationDate = null;

            //SAM537            
            string RevertSuspension = p.GetProcessParameter(Constants.ProcessParameter.IsRevertSuspension);
            bool isRevertSuspension = string.IsNullOrEmpty(RevertSuspension) ? false : Convert.ToBoolean(RevertSuspension);

            string ConvertFrmORPWF = p.GetProcessParameter(Constants.ProcessParameter.IsConvertFrmORPWF);
            bool IsConvertFrmORPWF = string.IsNullOrEmpty(ConvertFrmORPWF) ? changedApplicationTypeID : Convert.ToBoolean(ConvertFrmORPWF);
            /* 
            Changes for OHPNM-10264

            For inflight applications that haven't gone through the new NPIandMedID screen, things should work the same as before.

            * New Applications: for inflight applications, we'll grab the effective date as-is; for the new way, we will get this from usp_CheckRegNPIEnrollmentExists; everything else will work the same

            * Change Provider Type: this is the same as "New Applications", except that usp_ProcessStgRegEnrollment is run in order to update the end date of the previous registration's span; 

            * No Gap: this will work exactly as it always has, only the SP will not affect REG_SPECIALTY data if it's using the new way; for inflight applications, things will work as they were before

            * Reconsideration: although Reconsideration is essentially a no gap scenario, there is different code for this (usp_ApproveReconsiderationProvider); for inflight, this will run as-is; for the new code, we are skipping the REG_SPECIALTY code

            * Gap: for inflight applications, this will work as before, using usp_ReactivationReapplicationWithGap to do its magic; for the new way, usp_ProcessStgRegEnrollment is run to copy over the end date of the inactive time period that they entered into the REG_ENROLLMENT table's related row, and then a new version of usp_ReactivationReapplicationWithGap will run that will take care of adding the new active row using the start date they entered and it'll use the calculated end date for that enrollment and the provider record.    It will also take care of the Tax History data.  Specialty data will not be touched.

            * Change Effective Date: this will not be dealt with here since that doesn't have a workflow; it'll be dealt with directly in the NPIandMedID control
            */
            string npiMedIdSelection = p.GetProcessParameter(Constants.ProcessParameter.NpiMedIDSelection);
            npiMedIdSelection = npiMedIdSelection == CON.EnrollmentSpanOptions.ConvertFromORPWFGap ? CON.EnrollmentSpanOptions.Gap : npiMedIdSelection; // SAM719 treat ORP with gap as normal gap scenario.
            
            DateTime enrollStartDateTime;
            bool stagingTableHasData = false; // stagingTableHasData = false means that this workflow is "inflight" and hasn't gone through the new OHPNM-10264 process
            List<SqlParameter> sqlStgRegEnrollParms = new List<SqlParameter>();
            sqlStgRegEnrollParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, false));
            sqlStgRegEnrollParms.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
            DataSet dsStgRegEnroll = DataAccess.ExecuteStoredProcedure("usp_CheckRegNPIEnrollmentExists", sqlStgRegEnrollParms, "StgRegEnrollData");
            if (!string.IsNullOrEmpty(npiMedIdSelection) && ObjectControllerHelper.HasRows(dsStgRegEnroll))
            {
                stagingTableHasData = true;

                DataRow drStgRegEnroll = dsStgRegEnroll.Tables[0].Rows[0];
                enrollStartDateTime = ObjectControllerHelper.GetDateTime("ENROLL_START_DATE_TIME", drStgRegEnroll); // this won't matter for no gap scenario; it'll actually be grabbing the inactive row's start date, but it's never used

                if (npiMedIdSelection == CON.EnrollmentSpanOptions.Gap || npiMedIdSelection == CON.EnrollmentSpanOptions.ProviderTypeChange)
                {
                    List<SqlParameter> sqlStgRegEnrollParmsForProcessing = new List<SqlParameter>();
                    sqlStgRegEnrollParmsForProcessing.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, false));
                    sqlStgRegEnrollParmsForProcessing.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                    // For GAP, need to call usp_ProcessStgRegEnrollment to create the new active row and copy over the end date they entered for the end of the inactive gap
                    // For ProviderTypeChange, need to call usp_ProcessStgRegEnrollment to copy over end date of the previous registration that they entered
                    DataAccess.ExecuteStoredProcedure("usp_ProcessRegNPIEnrollment", sqlStgRegEnrollParmsForProcessing, "StgRegProcessEnrollData"); // no data comes back from this
                }
            }
            else
            {
                // assign enrollStartDateTime to null, so we don't cause issues below
                enrollStartDateTime = new DateTime(1753, 1, 1);
            }

            //OHPNM-14290, OHPNM-13109 - Reactivations or reapplication
            if (!requiresCredentialing && (isReactivation || isReapplication || workflowEventTypeID == CON.WorkflowEventType.Reconsideration))
            {
                List<SqlParameter> sqlParmsR = new List<SqlParameter>();
                sqlParmsR.Add(SqlParms.CreateParameter("RegID", DbType.Int32, registrationId, false));
                requiresCredentialing = Convert.ToBoolean(DataAccess.ExecuteStoredProcedure("usp_CheckIfNeedCredentialingforReactivationorReapplication", sqlParmsR, "IS_CRED_REQUIRED", SqlDbType.Bit, 100));                
            }

            // OHPNM-5521 - set-up gap variables for use later on
            int gapDays = int.Parse(AppSettings.Get("AutoTerminateDaysForNoGap"));
            bool beenAGapSinceTermination = false;
            bool needToAdjustEnrollmentAndTaxHistoryForGap = false;
            // only set-up gap variables if we actually have a terminated provider
            if (!ObjectControllerHelper.IsDateNull(oldtermDate))
            {
                // OHPNM-9424 BEGIN - this is a terminated provider; see if the NPI span (beg to 12/31/2299) is in the reg_npi_span_history table; if so, mark it as deleted; if it's not in the reg_npi_span_history table then add the current npi span of reg_provider
                bool isCopyNpiSpanHistory = AppSettings.Get("CopyNpiSpanHistory") == "true" ? true : false;
                if (isCopyNpiSpanHistory && !string.IsNullOrEmpty(npi))
                {
                    DateTime? npiStartDate = null;
                    DateTime? npiEndDate = null;
                    if (drReg["NPIStartDate"] != DBNull.Value)
                    {
                        npiStartDate = Methods.GetDateValue(drReg["NPIStartDate"]);
                    }
                    if (drReg["NPIEndDate"] != DBNull.Value)
                    {
                        npiEndDate = Methods.GetDateValue(drReg["NPIEndDate"]);
                    }
                    else
                    {
                        // if npi end date is null on a terminated provider, make it equal to term date
                        npiEndDate = oldtermDate;
                    }
                    List<SqlParameter> parmsCopyNpi = new List<SqlParameter>();
                    parmsCopyNpi.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, false));
                    parmsCopyNpi.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
                    parmsCopyNpi.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, Constants.appAdminUserId, false));
                    parmsCopyNpi.Add(SqlParms.CreateParameter("NPI_START_DATE", DbType.DateTime, npiStartDate.ToString(), false));
                    parmsCopyNpi.Add(SqlParms.CreateParameter("NPI_END_DATE", DbType.DateTime, npiEndDate.ToString(), false));
                    parmsCopyNpi.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                    DataAccess.ExecuteStoredProcedure("usp_CopyNpiDataToSpanHistory", parmsCopyNpi);
                }
                // OHPNM-9424 END

                // OHPNM-10264 - if this is using the new process and has data in the staging table, then get whether or not this is a gap scenario from the WF_PROCESS_PARAMETER_XREF.NPI_MEDID_RB_SELECTION field
                if (stagingTableHasData)
                {
                    if (npiMedIdSelection == CON.EnrollmentSpanOptions.Gap)
                    {
                        beenAGapSinceTermination = true;
                    }
                }
                else
                {
                    // figuring out gap without staging table
                    // if it's been 60 days or more since they were terminated, they are going to have a gap in coverage
                    if (oldtermDate.AddDays(gapDays) < DateTime.Now)
                    {
                        beenAGapSinceTermination = true;
                    }
                }
				
                // if there has been a gap AND this is a 1) reactivation or 2) reapplication or a 3) revalReg for a non-credentialed provider, then we need to deal with gap logic
                if (beenAGapSinceTermination && (isReactivation || isReapplication || isRevalRegOfTerminatedProvider))
                {
                    needToAdjustEnrollmentAndTaxHistoryForGap = true;
                }
            }

            // OHPNM-12182
            // When EnrollmentSpecialistApprovalDate is NULL or 1/1/1753 get the date from workflow end_date_time where TASK ID is 5/Provider Review
            // Set to current date when there is no Provider Review
            //OHPNM-15321 - Revalidation Dates for Non - Credentialed Providers calculate from ES approval date on thier current workflow
            if (ObjectControllerHelper.IsDateNull(EnrollmentSpecialistApprovalDate) || (workflowEventTypeID == CON.WorkflowEventType.RevalReg && !requiresCredentialing)) 
            {
                EnrollmentSpecialistApprovalDate = GetEnrollmentSpecialistApprovalDate(registrationId);
            }
            // OHPNM-5521
            // OHPNM-12182 Added ISNULL to check NULL so that current date will be assigned when there is no date available
            
            DateTime? maxDate = dsReg.Tables[0].AsEnumerable()
    .Where(row => row.Field<DateTime?>("COMMITTEE_DECISION_DATE") != null)
    .Max(row => row.Field<DateTime?>("COMMITTEE_DECISION_DATE"));

            if (maxDate.HasValue)
            {
                credentialApprovalDate = maxDate.Value;
            }
            else
            {
                credentialApprovalDate = DateTime.Now;
            }
            //if (drReg["COMMITTEE_DECISION_DATE"] == DBNull.Value || drReg.IsNull("COMMITTEE_DECISION_DATE"))
            //{
            //    credentialApprovalDate = DateTime.Now;
            //}
            //else
            //{
            //    credentialApprovalDate = ObjectControllerHelper.GetDateTime("COMMITTEE_DECISION_DATE", drReg);
            //}

            bool HasActiveODASpecialty = ObjectControllerHelper.GetBool("HasActiveODASpecialty", drReg);
            bool HasActiveDODDSpecialty = ObjectControllerHelper.GetBool("HasActiveDODDSpecialty", drReg);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, registrationId, true));
            bool isDoDDIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfDODDInitialApplication", parameters));

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, registrationId, true));
            bool isODAIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfODAInitialApplication", parameters));

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, registrationId, true));
            parameters.Add(SqlParms.CreateParameter("ProcessId", DbType.Int32, p.ProcessID, true));
            bool IsCalcualteRevalDoDD = Convert.ToBoolean(DataAccess.ExecuteStoredProcedure("usp_CheckIfRecalcualteRevalDoDD", parameters, "IsRecalc", SqlDbType.Bit, 100));


            if (ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg) == CON.WorkflowEventType.Reconsideration)
            {
                ReconsiderationworkflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
            }
            if (mmisProviderTypeID == "LT")
            {
                List<SqlParameter> sqlParmsOP = new List<SqlParameter>();
                sqlParmsOP.Add(SqlParms.CreateParameter("RegID", DbType.Int32, OperatorRegID, false));
                DataSet dsRegOP = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParmsOP, "RegData");

                dsRegOP.Tables[0].TableName = "RegData";
                DataRow drRegOP = ObjectControllerHelper.HasRows(dsRegOP) ? dsRegOP.Tables[0].Rows[0] : null;
                workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drRegOP);
				operatorRegistrationChangeEffectiveDate = ObjectControllerHelper.GetDateTime("ChangeEffectiveDate", drRegOP); // OHPNM-5361 
            }
            // Reconsideration for terminated registrations (since RevalidationDatePriorToTermination is not null); OHPNM-10265 -- not changing this since reconsideration is always a no gap scenario
            if (ReconsiderationworkflowEventTypeID == CON.WorkflowEventType.Reconsideration && !ObjectControllerHelper.IsDateNull(RevalidationDatePriorToTermination) && !ObjectControllerHelper.IsDateNull(EndDateTime))
            {
                List<SqlParameter> parmsup = new List<SqlParameter>();
                parmsup.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, registrationId, false));
                parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
                parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, Constants.appAdminUserId, false));
                if (stagingTableHasData)
                {
                    parmsup.Add(SqlParms.CreateParameter("SKIP_SPECIALTY", DbType.Boolean, true, false));
                }
                else
                {
                    parmsup.Add(SqlParms.CreateParameter("SKIP_SPECIALTY", DbType.Boolean, false, false));
                }
                DataAccess.ExecuteStoredProcedure("usp_ApproveReconsiderationProvider", parmsup);

            }
            else
            {
                // OHPNM-5521
                if (((isReactivation || isReapplication || isRevalRegOfTerminatedProvider) && !beenAGapSinceTermination && !IsConvertFrmORPWF) ||(workflowEventTypeID == CON.WorkflowEventType.CredentialReconsideration))
                {
                    // this is a reactivation or reapplication without a gap; go adjust dates for enrollment and specialties without worrying about a gap and move on  (this also applies to revalreg that was terminated previously)
                    List<SqlParameter> parmsup = new List<SqlParameter>();
                    parmsup.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, registrationId, false));
                    parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
                    parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, Constants.appAdminUserId, false));
                    DataAccess.ExecuteStoredProcedure("usp_ReactivationReapplicationNoGap", parmsup);
                }

                // OHPNM-10264 - change ChangeProviderType to NewReg, since everything is the same at this point
                if (workflowEventTypeID == CON.WorkflowEventType.ChangeProviderType)
                {
                    // just make this a new registration since the logic for this provider is the same
                    workflowEventTypeID = CON.WorkflowEventType.NewReg;
                }
                bool isUpdateRegistration = false;

                // OHPNM-9367 - if we are forcing them to review for a DODD terminated provider with an 'initial' or 'renewal' payload, then figure out if this is gap or not and fake-activate the dodd specialty for now to get us to the right code below
                bool forceWfToReview = false;
                bool useGap = false;
                forceWfToReview = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.ForceWfToReview))
                        ? false : Convert.ToBoolean(p.GetProcessParameter(Constants.ProcessParameter.ForceWfToReview));
                if (forceWfToReview)
                {
                    // need to also grab useGap parameter
                    useGap = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.UseGap))
                    ? false : Convert.ToBoolean(p.GetProcessParameter(Constants.ProcessParameter.UseGap));

                    // override this since we are fixing it in this class
                    HasActiveDODDSpecialty = true;
                }

                //check for update or revalidation workflow based on reg_create_date_time in reval window
                DateTime regCreateDateTime = ObjectControllerHelper.GetDateTime("REG_CREATE_DATE_TIME", drReg);
                List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                sqlParms1.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, registrationId, false));

                if ((ReconsiderationworkflowEventTypeID == CON.WorkflowEventType.Reconsideration || workflowEventTypeID == CON.WorkflowEventType.CredentialReconsideration)
                    && ObjectControllerHelper.IsDateNull(RevalidationDatePriorToTermination))
                {
                    workflowEventTypeID = CON.WorkflowEventType.NewReg;
                    List<SqlParameter> parmsup = new List<SqlParameter>();
                    parmsup.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, registrationId, false));
                    parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
                    parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, Constants.appAdminUserId, false));
                    DataAccess.ExecuteStoredProcedure("usp_DeleteReconsiderationApprove", parmsup);
                }
                if (workflowEventTypeID == CON.WorkflowEventType.CredentialReconsideration && !ObjectControllerHelper.IsDateNull(RevalidationDatePriorToTermination))
                {
                    workflowEventTypeID = CON.WorkflowEventType.RevalReg;
                }
                DateTime? changeEffectiveDate = null;

                // OHPNM-5501 ; moved block out of OHPNM-5361 block
                if (mmisProviderTypeID == "LT" && operatorRegistrationChangeEffectiveDate != null)
                {
                    // OHPNM-5361 - need to use the effective date of the main provider
                    changeEffectiveDate = operatorRegistrationChangeEffectiveDate;
                }

                // OHPNM-6125 leave eff date alone for LT
                if ((workflowEventTypeID == CON.WorkflowEventType.NewReg || ObjectControllerHelper.IsDateNull(regChangeEffectiveDate)) && mmisProviderTypeID != "LT")
                {//changes as per OHPNM-3509
                    if (workflowID == CON.WorkflowType.CPC)
                    {
                        changeEffectiveDate = CPC_Prog_Year;
                    }
                    else
                    {
                        if (stagingTableHasData)
                        {
                            changeEffectiveDate = enrollStartDateTime;
                        }
                        else
                        {
                            // for any providers inflight, gather changeEffectiveDate the previous way
                            if (retroEffDate && !requiresCredentialing)
                                changeEffectiveDate = reqEffDate;
                            else if (requiresCredentialing)
                                changeEffectiveDate = credentialApprovalDate;
                            else if (!retroEffDate && !requiresCredentialing)
                                changeEffectiveDate = EnrollmentSpecialistApprovalDate;
                        }
                    }
                }


                // OHPNM-5361 - don't grab the changeEffectiveDate from the registration for the "LT" provider that is dependent on something like an "86" ... we already got it above in the 'if (mmisProviderTypeID == "LT")' part
                if (drReg["ChangeEffectiveDate"] != DBNull.Value && operatorRegistrationChangeEffectiveDate == null)
                {

                    if (workflowEventTypeID == CON.WorkflowEventType.NewReg || workflowEventTypeID == CON.WorkflowEventType.RevalReg || workflowEventTypeID == CON.WorkflowEventType.UpdateReg || workflowEventTypeID == CON.WorkflowEventType.CPCReattest)
                        changeEffectiveDate = ObjectControllerHelper.GetDateTime("ChangeEffectiveDate", drReg);
                }
                //do not move this code and do not add any logic above this related to enddate enrollment status, enrollment status reason
                DataSet dsEnrollment = RegistrationController.SelectRegistrationData(registrationId, "ENROLLMENT");

                if (MMISProviderTypeID == "86" && applicationTypeID == CON.ApplicationType.ChangeOfOperator && workflowEventTypeID == CON.WorkflowEventType.NewReg)
                {
                    int chopParentRegId = GetExitingProviderRegId(registrationId);
                    changeEffectiveDate = GetChopFinalDate(chopParentRegId);

                    //Update Chop date to Openend date                   
                    Dictionary<string, string> parmsChop = new Dictionary<string, string>();
                    parmsChop.Add("REG_ID", registrationId.ToString());
                    parmsChop.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parmsChop.Add("LAST_MODIFIED_USER", Constants.appAdminUserId);
                    parmsChop.Add("Service_Location_Expiration_Date", CON.PNMDate.MaxDateString);
                    RegistrationController.UpdateRegistrationDataWithParams("updateREG_CHOP_PARENTCustom", parmsChop);
                }
                
                bool revalidationNeeded = (workflowEventTypeID == CON.WorkflowEventType.RevalReg) && workflowID != CON.WorkflowType.CPC;

                int RevalDueWindow = int.Parse(AppSettings.Get("RevalidationDueWindow"));
                bool isCalculateRevalDate = true;
                if(IsConvertFrmORPWF)
                {
                    enrollmentStatusCode = Constants.EnrollmentStatusTypeID.Active.ToString();
                }

                if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                {
                    isUpdateRegistration = true;
                }
                updateRevalidationDate = revalidationNeeded || (workflowEventTypeID == CON.WorkflowEventType.CPCReattest && workflowID == CON.WorkflowType.CPC && MMISProviderTypeID == "99") 
                                            || (isUpdateRegistration && (isDoDDIntialApp || isODAIntialApp || IsCalcualteRevalDoDD));

                //OHPNM-18162 - Add enrollment span on reactivation of denied providers.
                if(isUpdateRegistration && isReactivation && !ObjectControllerHelper.HasRows(dsEnrollment))
                {
                    updateRevalidationDate = true;
                    isCalculateRevalDate=false;
                }

                sqlParms = new List<SqlParameter>();

                if ((changeEffectiveDate.HasValue && !isUpdateRegistration && workflowEventTypeID == CON.WorkflowEventType.NewReg)// p.WorkflowID != Constants.WorkflowType.RegistrationExpressTermination)
                    )
                {
                    sqlParms.Add(SqlParms.CreateParameter("CHANGE_EFFECTIVE_DATE", DbType.DateTime, changeEffectiveDate.Value, false));
                    setRevalidationDate = true;
                    if (ReconsiderationworkflowEventTypeID == CON.WorkflowEventType.Reconsideration || workflowEventTypeID == CON.WorkflowEventType.CredentialReconsideration)
                    {
                        sqlParms.Add(SqlParms.CreateParameter("REG_PROGRAM_STATUS_TYPE_ID", DbType.Int32, CON.RegistrationProgramStatusTypeId.Revalidation, false));                       
                    }

                    //OHPNM-14266 If it is a new registration update the latest clia effective date to the provider effective date.
                    DataSet dsCLIA = RegistrationController.SelectRegistrationData(registrationId, "CLIA");

                    if (ObjectControllerHelper.HasRows(dsCLIA))
                    {                        
                        DateTime cliaEffDate = ObjectControllerHelper.GetDateTime("CLIA_EFF_DATE", dsCLIA.Tables[0].Rows[0]);
                        if (!ObjectControllerHelper.IsDateNull(cliaEffDate) && changeEffectiveDate.Value > cliaEffDate)
                        {
                            //Update CLIA effective date based on change effetcive date
                            UpdateRegCliaEffectivateByRegistrationEffectiveDate(registrationId, changeEffectiveDate.Value);
                        }
                    }
                }
                else if (changeEffectiveDate.HasValue && (ObjectControllerHelper.IsDateNull(regChangeEffectiveDate)))
                {
                    // for OHPNM-10264, need to change the effective date for the start of the new gap
                    sqlParms.Add(SqlParms.CreateParameter("CHANGE_EFFECTIVE_DATE", DbType.DateTime, changeEffectiveDate.Value, false));
                }
                //OHPNM 1349 - Once a converted provider updates change the REG_PROGRAM_STATUS_TYPE_ID to Maintenance.
                if (isUpdateRegistration && regProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion)
                {
                    sqlParms.Add(SqlParms.CreateParameter("REG_PROGRAM_STATUS_TYPE_ID", DbType.Int32, CON.RegistrationProgramStatusTypeId.Maintenance, false));
                }

                //OHPNM-9822-Update with Approved status on Promote to Active
                sqlParms.Add(SqlParms.CreateParameter("REGISTRATION_STATUS_TYPE_ID", DbType.Int32, CON.RegistrationStatusTypeId.Approved, false));

                if (sqlParms.Count > 0)
                {                 
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, false));
                    DataAccess.ExecuteStoredProcedure("updateREGISTRATION", sqlParms);
                }
                               
                DateTime prevRevaldate = ObjectControllerHelper.GetDateTime("RevalidationDate", drReg);               
                // 13109,14290
                //OHPNM - 14047 Reapplications should be treated like a new enrollment.
                //OHPNM-14276:Revalidation date for credentialed providers should be based on the cred approved date + 36 months
                //OHPNM-15321 :Revalidation Dates for Non - Credentialed Providers calculate from ES approval date on their current workflow
                prevRevaldate = requiresCredentialing ? credentialApprovalDate : EnrollmentSpecialistApprovalDate;

                DateTime approvalDate = changeEffectiveDate.GetValueOrDefault();
                DateTime EnrollmentStartDt = (workflowID == CON.WorkflowType.CPC) ? Convert.ToDateTime(CPC_Prog_Year) : Convert.ToDateTime(changeEffectiveDate);
                bool isRevalDateCalculated = false;
                if (setRevalidationDate || updateRevalidationDate || string.IsNullOrEmpty(enrollmentStatusCode)) 
                {
                    Dictionary<string, string> dictParms = new Dictionary<string, string>();
                    dictParms.Add("REG_ID", registrationId.ToString());
                    int providerRiskLevelID = ObjectControllerHelper.GetInt("PROVIDER_RISK_LEVEL_ID", drReg);
                    bool DelegateCredentialingRequired = ObjectControllerHelper.GetBool("DelegateCredentialingRequired", drReg);
                    bool IsHospitalBasedProvider = CredentialController.IsHospitalBasedProvider(registrationId);
                    
                    if (isCalculateRevalDate)
                    {
                        int reval5Yrs = Convert.ToInt32(DataAccess.GetAppSetting("RevalidationDelta5years"));
                        int reval3Yrs = Convert.ToInt32(DataAccess.GetAppSetting("RevalidationDelta3years"));

                        if (HasActiveDODDSpecialty)     //DODD Providers
                        {
                            doddStartDate = ObjectControllerHelper.GetDateTime("dodd_start_Date", drReg);
                            if (drReg["dodd_start_Date"] == DBNull.Value || drReg.IsNull("dodd_start_Date"))
                            {
                                doddStartDate = (changeEffectiveDate.HasValue) ? changeEffectiveDate.Value : DateTime.Now;
                            }
                            if (workflowEventTypeID == CON.WorkflowEventType.NewReg)
                            {
                                dictParms.Add("END_DATE", doddStartDate.AddYears(reval5Yrs).ToString());
                            }
                            if (isUpdateRegistration && (isDoDDIntialApp || IsCalcualteRevalDoDD))
                            {
                                if (requiresCredentialing && (!DelegateCredentialingRequired && !IsHospitalBasedProvider))
                                {
                                    if (enrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString())
                                    {
                                        isRevalDateCalculated = true;
                                        dictParms.Add("END_DATE", doddStartDate.AddYears(reval5Yrs).ToString());
                                    }
                                }
                                else
                                {
                                    isRevalDateCalculated = true;
                                    dictParms.Add("END_DATE", doddStartDate.AddYears(reval5Yrs).ToString());
                                }
                                if (enrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString())
                                    EnrollmentStartDt = doddStartDate; // for REG_ENROLLMENT dates
                            }
                            if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                            {
                                dictParms.Add("END_DATE", prevRevaldate.AddYears(reval5Yrs).ToString());
                                revalidationDate = prevRevaldate.AddYears(reval5Yrs);
                            }
                        }
                        else if (HasActiveODASpecialty)     //ODA Providers
                        {
                            if (workflowEventTypeID == CON.WorkflowEventType.NewReg)
                            {
                                dictParms.Add("END_DATE", approvalDate.AddYears(reval5Yrs).ToString());
                            }
                            if (isUpdateRegistration && isODAIntialApp && !HasActiveDODDSpecialty)
                            {
                                if (requiresCredentialing && (!DelegateCredentialingRequired && !IsHospitalBasedProvider))
                                {
                                    if (enrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString())
                                    {
                                        isRevalDateCalculated = true;
                                        dictParms.Add("END_DATE", DateTime.Now.AddYears(reval5Yrs).ToString());
                                    }
                                }
                                else
                                {
                                    isRevalDateCalculated = true;
                                    dictParms.Add("END_DATE", DateTime.Now.AddYears(reval5Yrs).ToString());
                                }

                                if (enrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString())
                                    EnrollmentStartDt = DateTime.Now; // for REG_ENROLLMENT dates
                            }
                            if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                            {
                                dictParms.Add("END_DATE", prevRevaldate.AddYears(reval5Yrs).ToString());
                                revalidationDate = prevRevaldate.AddYears(reval5Yrs);
                            }
                        }                      
                        else if (MMISProviderTypeID == "08" || MMISProviderTypeID == "15" || MMISProviderTypeID == "77" || MMISProviderTypeID == "93" || MMISProviderTypeID == "FF" || MMISProviderTypeID == "VH" || MMISProviderTypeID == "LT")
                        {
                            // these internal provider types are not required to revalidate. so, set the revalidation date to be the high-end date (12/31/2299) Bug OHPNM - 1242
                            DateTime dt = Convert.ToDateTime("12/31/2299");
                            dictParms.Add("END_DATE", dt.ToString());
                        }
                        else if (MMISProviderTypeID == "99")    // CPC Providers
                        {
                            DateTime dt = DateTime.ParseExact(AppSettings.Get("CPCProgramEndDate", string.Empty) + "/" + AppSettings.Get("CPCProgramYear"), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            dictParms.Add("END_DATE", dt.ToString());
                        }
                        else                // Medicaid Providers
                        {
                            int credentialingResultID = 0;
                            int credentialingStatusId = 0;
                            //OHPNM-3623
                            List<SqlParameter> sqlParmscr = new List<SqlParameter>();
                            sqlParmscr.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, registrationId, false));
                            DataSet dsCR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_Credentialing_Result", sqlParmscr, "RegCredentialResult");

                            DataRow drCR = ObjectControllerHelper.HasRows(dsCR) ? dsCR.Tables[0].Rows[0] : null;

                            if (drCR != null)
                            {
                                credentialingResultID = ObjectControllerHelper.GetInt("CREDENTIALING_RESULT_ID", drCR);
                                credentialingStatusId = ObjectControllerHelper.GetInt("CREDENTIALING_STATUS_ID", drCR);
                            }

                            if (applicationTypeID == Constants.ApplicationType.ORP || DelegateCredentialingRequired || CredentialController.IsHospitalBasedProvider(registrationId) || credentialingStatusId == CON.CredentilaingStatus.ProcessDiscontinued)
                            {
                                if (workflowEventTypeID == CON.WorkflowEventType.NewReg)
                                {
                                    dictParms.Add("END_DATE", approvalDate.AddYears(reval5Yrs).ToString());
                                }
                                if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                                {
                                    dictParms.Add("END_DATE", prevRevaldate.AddYears(reval5Yrs).ToString());
                                    revalidationDate = prevRevaldate.AddYears(reval5Yrs);
                                }
                            }
                            else if (requiresCredentialing && (!DelegateCredentialingRequired && !IsHospitalBasedProvider))
                            {
                                if (workflowEventTypeID == CON.WorkflowEventType.NewReg)
                                {
                                    if (credentialingResultID != CON.CredentialingResult.Pass1Year)
                                    {
                                        dictParms.Add("END_DATE", approvalDate.AddYears(reval3Yrs).ToString());
                                    }
                                    else
                                    {
                                        dictParms.Add("END_DATE", approvalDate.AddYears(1).ToString());
                                    }
                                }
                                if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                                {
                                    if (credentialingResultID != CON.CredentialingResult.Pass1Year)
                                    {
                                        dictParms.Add("END_DATE", prevRevaldate.AddYears(reval3Yrs).ToString());
                                        revalidationDate = prevRevaldate.AddYears(reval3Yrs);
                                    }
                                    else
                                    {
                                        dictParms.Add("END_DATE", prevRevaldate.AddYears(1).ToString());
                                        revalidationDate = prevRevaldate.AddYears(1);
                                    }
                                }
                            }
                            else
                            {
                                if (!stagingTableHasData)
                                {
                                    if (retroEffDate)
                                        approvalDate = reqEffDate;
                                    else
                                        approvalDate = submitDateTime;
                                }

                                if (workflowEventTypeID == CON.WorkflowEventType.NewReg)
                                {
                                    dictParms.Add("END_DATE", approvalDate.AddYears(reval5Yrs).ToString());
                                }
                                if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                                {
                                    dictParms.Add("END_DATE", prevRevaldate.AddYears(reval5Yrs).ToString());
                                    revalidationDate = prevRevaldate.AddYears(reval5Yrs);
                                }
                            }
                        }
                    }

                    int providerTypeID = 0;
                    providerTypeID = ObjectControllerHelper.GetInt("ProviderTypeID", drReg);

                    if (string.IsNullOrEmpty(enrollmentStatusCode) || (!string.IsNullOrEmpty(enrollmentStatusCode)))
                    {
                        if (applicationTypeID == CON.ApplicationType.ORP)
                        {
                            dictParms.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.ORPActive.ToString());
                            dictParms.Add("ENROLLMENT_STATUS_REASON_ID", Constants.EnrollStatusReasonID.ORPNonBilling.ToString());
                        }
                        else if (applicationTypeID == CON.ApplicationType.MCP)
                        {
                            dictParms.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.ORPActive.ToString());
                            dictParms.Add("ENROLLMENT_STATUS_REASON_ID", Constants.EnrollStatusReasonID.NonParticipatingRPTOnly.ToString());
                        }
                        else
                        {
                            dictParms.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.Active.ToString());
                            dictParms.Add("ENROLLMENT_STATUS_REASON_ID", Constants.EnrollStatusReasonID.Active.ToString());
                        }
                        if(!ObjectControllerHelper.IsDateNull(oldtermDate))
                        {
                            dictParms.Add("RESET_TERM", "1");
                            dictParms.Add("TERM_DATE", null);
                            // OHPNM-9424 - we are setting term_date to null, so we need to set NPI_END_DATE to 12/31/2299
                            dictParms.Add("NPI_END_DATE", "12/31/2299");
                        }

                        // SAM537 Dont set ESRC to active if Suspended prov does ODM update
                        if (!isRevertSuspension && workflowEventTypeID == CON.WorkflowEventType.UpdateReg && !isReactivation && regProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Suspended)
                        {
                            dictParms.Add("ENROLLMENT_STATUS_REASON_ID", enrollmentStatusReasonID.ToString()); 
                        }


                    }

                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    DataSet dsACH = RegistrationController.SelectRegistrationData(registrationId, "ACH_REQUEST");

                    if (setRevalidationDate)
                    {
                        // Bug 6805: Set NPI_START_DATE = Change effective date for new registration.
                        if (!string.IsNullOrEmpty(npi))
                        {
                            dictParms.Add("NPI_START_DATE", changeEffectiveDate.Value.ToString());
                            dictParms.Add("NPI_END_DATE", "12/31/2299");
                        }
                        // Bug 6805: Set EFT_START_DATE = Change effective date for new registration.
                        parms.Add("EFT_START_DATE", changeEffectiveDate.Value.ToString());
                        // But 7496://if hospice save CBSAStartDate with change_effective date
                        if (IsHospiceProvider(dsReg.Tables[0].Rows[0]) && p.WorkflowID == Constants.WorkflowType.RegistrationNew)
                        {
                            Dictionary<string, string> parms1 = new Dictionary<string, string>();
                            parms1.Add("REG_ID", registrationId.ToString());
                            parms1.Add("CBSA_START_DATE", changeEffectiveDate.Value.ToString());
                            DataSet dsSL = RegistrationController.SelectRegistrationData(registrationId, "SERVICE_LOCATION");
                            if (ObjectControllerHelper.HasRows(dsSL) && dsSL.Tables[0].Rows[0]["CBSA_START_DATE"].ToString() != changeEffectiveDate.Value.ToString())
                            {
                                parms1.Add("REG_SERVICE_LOCATION_ID", dsSL.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"].ToString());
                                parms1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                                parms1.Add("LAST_MODIFIED_USER", Constants.appAdminUserId);
                                RegistrationController.UpdateRegistrationData("SERVICE_LOCATIONcustom", parms1); // if this runs before revalidation code, it's going to cause issues because term_date will be null
                            }
                        }
                    }

                    if ((dsACH != null && dsACH.Tables.Count > 0 && dsACH.Tables[0].Rows.Count > 0))
                    {
                        // Update
                        parms.Add("REG_ID", registrationId.ToString());
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", Constants.appAdminUserId);
                        parms.Add("REG_ACH_REQUEST_ID", Methods.GetStringValue(dsACH.Tables[0].Rows[0]["REG_ACH_REQUEST_ID"]));
                        RegistrationController.UpdateRegistrationData("ACH_REQUEST", parms);
                    }
                    RegistrationController.UpdateRegistrationData("PROVIDERCustom", dictParms);

                    sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, registrationId, false));
                    dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");

                    dsReg.Tables[0].TableName = "RegData";
                    drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;

                    if (drReg == null)
                    {
                        throw new Exception(string.Format(Constants.LogString.WorkflowError, Assembly.GetExecutingAssembly().GetName().Name,
                                    "Unable to retrieve registration using usp_SelectREGISTRATIONByRegId."));
                    }


                    DateTime EndDateTimeNew = ObjectControllerHelper.GetDateTime("EndDate", drReg);
                    
                    Dictionary<string, string> parmsenroll = new Dictionary<string, string>();
                   

                    // OHPNM-5521 - if this is a reactivation or reapplication (or revalreg for non credentialed provider), we can skip the enrollment logic below in the other if-statements since the reactivation/reapplication SPC takes care of this
                    // eventually this enrollment logic needs to be analyzed because it doesn't look right
                    // we'll also never get here if this is a reactivation or reapplication with no gap because that's dealt with higher up and will never get here

                    // OHPNM-9367 - reactivate dodd specialty and enrollment for a terminated provider
					if (forceWfToReview)
					{
						doddStartDate = ObjectControllerHelper.GetDateTime("dodd_start_Date", drReg); // regrab the dodd start date from the registration re-grab
						// activate DODD specialty and enrollment
						List<SqlParameter> parmsup = new List<SqlParameter>();
						parmsup.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, registrationId, false));
						parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
						parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, Constants.appAdminUserId, false));
						parmsup.Add(SqlParms.CreateParameter("USE_GAP", DbType.Boolean, useGap, false));
						parmsup.Add(SqlParms.CreateParameter("CERT_START_DATE", DbType.DateTime, doddStartDate.ToString(), false));
						parmsup.Add(SqlParms.CreateParameter("END_DATE", DbType.DateTime, EndDateTimeNew.ToString(), false));
						parmsup.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, oldtermDate.ToString(), false));
						DataAccess.ExecuteStoredProcedure("usp_ActivateDoddSpecialtyAndEnrollment", parmsup);
					}
                    else if (!needToAdjustEnrollmentAndTaxHistoryForGap)
                    {                        
                        if (ObjectControllerHelper.HasRows(dsEnrollment) && (workflowEventTypeID == CON.WorkflowEventType.RevalReg || (isUpdateRegistration && isRevalDateCalculated)) 
							 && (enrollmentStatusCode == CON.EnrollmentStatusTypeID.Active.ToString() || enrollmentStatusCode == CON.EnrollmentStatusTypeID.ORPActive.ToString()))
						{
                            // process to convert ORP providers to standard providers; this will always be a revalidation workflow even type
							if (changedApplicationTypeID == true)
							{
                                List<SqlParameter> parmsup = new List<SqlParameter>();
                                parmsup.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, false));
                                parmsup.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                                parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
                                parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, Constants.appAdminUserId, false));
                                parmsup.Add(SqlParms.CreateParameter("REVALIDATION_DATE", DbType.DateTime, EndDateTimeNew, false));
                                parmsup.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, oldtermDate, false));
                                parmsup.Add(SqlParms.CreateParameter("IS_TERMED_PROVIDER", DbType.Boolean, isRevalRegOfTerminatedProvider, false));
                                DataAccess.ExecuteStoredProcedure("usp_SetConvertORPtoStd_EnrollmentAndSpecialty", parmsup);
                            }
							else
							{
								parmsenroll = new Dictionary<string, string>();
								int regEnrollmentID = ObjectControllerHelper.GetInt("REG_ENROLLMENT_ID", dsEnrollment.Tables[0].Rows[0]);
								//Update and insert
								parmsenroll.Add("REG_ENROLLMENT_ID", regEnrollmentID.ToString());
								parmsenroll.Add("REG_ID", registrationId.ToString());
								parmsenroll.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
								parmsenroll.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
								parmsenroll.Add("ENROLL_END_DATE_TIME", EndDateTimeNew.ToString());

								RegistrationController.UpdateRegistrationData("ENROLLMENT", parmsenroll);
							}
						}
                        else if (workflowEventTypeID == CON.WorkflowEventType.NewReg || !ObjectControllerHelper.HasRows(dsEnrollment))
                        {
                            parmsenroll = new Dictionary<string, string>();
                            parmsenroll.Add("REG_ID", registrationId.ToString());
                            parmsenroll.Add("ENROLL_START_DATE_TIME", EnrollmentStartDt.ToString());
                            parmsenroll.Add("ENROLL_END_DATE_TIME", EndDateTimeNew.ToString());
                            if (applicationTypeID == CON.ApplicationType.ORP)
                            {
                                parmsenroll.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.ORPActive.ToString());
                                parmsenroll.Add("ENROLL_STATUS_REASON_ID", Constants.EnrollStatusReasonID.ORPNonBilling.ToString());
                            }
                            else if (applicationTypeID == CON.ApplicationType.MCP)
                            {
                                parmsenroll.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.ORPActive.ToString());
                                parmsenroll.Add("ENROLL_STATUS_REASON_ID", Constants.EnrollStatusReasonID.NonParticipatingRPTOnly.ToString());
                            }
                            else
                            {
                                parmsenroll.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.Active.ToString());
                                parmsenroll.Add("ENROLL_STATUS_REASON_ID", Constants.EnrollStatusReasonID.Active.ToString());
                            }

                            if (!ObjectControllerHelper.HasRows(dsEnrollment))
                            {
                                parmsenroll.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                                parmsenroll.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                                parmsenroll.Add("CREATE_DATE_TIME", DateTime.Now.ToString());
                                parmsenroll.Add("CREATED_BY_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                                RegistrationController.InsertRegistrationData("ENROLLMENT", parmsenroll);
                            }
                            else if (ObjectControllerHelper.HasRows(dsEnrollment))
                            {

                                int regEnrollmentID = ObjectControllerHelper.GetInt("REG_ENROLLMENT_ID", dsEnrollment.Tables[0].Rows[0]);
                                //Update and insert
                                parmsenroll.Add("REG_ENROLLMENT_ID", regEnrollmentID.ToString());
                                parmsenroll.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                                parmsenroll.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                                RegistrationController.UpdateRegistrationData("ENROLLMENT", parmsenroll);
                            }
                        }
                        else if ((((workflowEventTypeID == CON.WorkflowEventType.RevalReg || (isUpdateRegistration && isReactivation)) && !stagingTableHasData)
                           || (isUpdateRegistration && isRevalDateCalculated)) && enrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString())
                        {

                            if (ObjectControllerHelper.HasRows(dsEnrollment) &&
                                (workflowEventTypeID == CON.WorkflowEventType.RevalReg || (isUpdateRegistration && isReactivation)))
                            {
								parmsenroll = new Dictionary<string, string>();
								int regEnrollmentID = ObjectControllerHelper.GetInt("REG_ENROLLMENT_ID", dsEnrollment.Tables[0].Rows[0]);
								//Update and insert
								parmsenroll.Add("REG_ENROLLMENT_ID", regEnrollmentID.ToString());
								parmsenroll.Add("REG_ID", registrationId.ToString());
								parmsenroll.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
								parmsenroll.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
								parmsenroll.Add("ENROLL_END_DATE_TIME", EndDateTimeNew.ToString());

								RegistrationController.UpdateRegistrationData("ENROLLMENT", parmsenroll);
							}
                            //JIRA 1502 pschwarz double inserting reg enrollment 
                            if ((isUpdateRegistration && isRevalDateCalculated) && enrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString())
                            {
                                parmsenroll = new Dictionary<string, string>();
                                int regEnrollmentID = ObjectControllerHelper.GetInt("REG_ENROLLMENT_ID", dsEnrollment.Tables[0].Rows[0]);
                                //Update and insert
                                parmsenroll.Add("REG_ENROLLMENT_ID", regEnrollmentID.ToString());
                                parmsenroll.Add("REG_ID", registrationId.ToString());
                                parmsenroll.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                                parmsenroll.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                                parmsenroll.Add("ENROLL_END_DATE_TIME", DateTime.Now.AddDays(-1).ToString());

                                RegistrationController.UpdateRegistrationData("ENROLLMENT", parmsenroll);

                                parmsenroll = new Dictionary<string, string>();
                                parmsenroll.Add("REG_ID", registrationId.ToString());
								parmsenroll.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
								parmsenroll.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
								parmsenroll.Add("CREATE_DATE_TIME", DateTime.Now.ToString());
								parmsenroll.Add("CREATED_BY_USER", Guid.Parse(Constants.appAdminUserId).ToString());
								parmsenroll.Add("ENROLL_END_DATE_TIME", EndDateTimeNew.ToString());
                                if (applicationTypeID == CON.ApplicationType.ORP)
                                {
                                    parmsenroll.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.ORPActive.ToString());
                                    parmsenroll.Add("ENROLL_STATUS_REASON_ID", Constants.EnrollStatusReasonID.ORPNonBilling.ToString());
                                }
                                else if (applicationTypeID == CON.ApplicationType.MCP)
                                {
                                    parmsenroll.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.ORPActive.ToString());
                                    parmsenroll.Add("ENROLL_STATUS_REASON_ID", Constants.EnrollStatusReasonID.NonParticipatingRPTOnly.ToString());
                                }
                                else
                                {
                                    parmsenroll.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.Active.ToString());
                                    parmsenroll.Add("ENROLL_STATUS_REASON_ID", Constants.EnrollStatusReasonID.Active.ToString());
                                }

                                if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                                {
                                    EnrollmentStartDt = DateTime.Now;
                                }
                                parmsenroll.Add("ENROLL_START_DATE_TIME", EnrollmentStartDt.ToString());

                                RegistrationController.InsertRegistrationData("ENROLLMENT", parmsenroll);
							}							

						}
                        else if ((workflowEventTypeID == CON.WorkflowEventType.RevalReg || (isUpdateRegistration && isReactivation)) && stagingTableHasData
                                 && npiMedIdSelection == CON.EnrollmentSpanOptions.NoGap && enrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString())
                        {
                            parmsenroll = new Dictionary<string, string>();
                            int regEnrollmentID = ObjectControllerHelper.GetInt("REG_ENROLLMENT_ID", dsEnrollment.Tables[0].Rows[0]);
                            //Update and insert
                            parmsenroll.Add("REG_ENROLLMENT_ID", regEnrollmentID.ToString());
                            parmsenroll.Add("REG_ID", registrationId.ToString());
                            parmsenroll.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            parmsenroll.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                            parmsenroll.Add("ENROLL_END_DATE_TIME", EndDateTimeNew.ToString());
                            revalidationDate = EndDateTimeNew;

                            RegistrationController.UpdateRegistrationData("ENROLLMENT", parmsenroll);
                        }
					}
                }                
                if (!changedApplicationTypeID && ReconsiderationworkflowEventTypeID == CON.WorkflowEventType.Reconsideration && workflowEventTypeID == CON.WorkflowEventType.NewReg)
                {

                    List<SqlParameter> parmsup = new List<SqlParameter>();
                    parmsup.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, registrationId, false));
                    parmsup.Add(SqlParms.CreateParameter("term_date", DbType.DateTime, oldtermDate.ToString(), false));
                    parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
                    parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, Constants.appAdminUserId, false));
                    DataAccess.ExecuteStoredProcedure("usp_UpdateReconsiderationAffiliations", parmsup);
                }
                
                //do not add code after this because change_effective_date and end_date will be saved in reg_enrollment
                sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, registrationId, false));
                dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;

                DateTime effectiveDate = ObjectControllerHelper.GetDateTime("ChangeEffectiveDate", drReg);
                DateTime endDate = ObjectControllerHelper.GetDateTime("EndDate", drReg);
                string taxid = ObjectControllerHelper.GetString("TaxID", drReg);
                int taxidTypeID = ObjectControllerHelper.GetInt("TaxIDTypeID", drReg);
               
                if ((workflowEventTypeID == CON.WorkflowEventType.NewReg) || (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                    || (workflowEventTypeID == CON.WorkflowEventType.CPCReattest))
                {
                    List<SqlParameter> sqlParmsNew = new List<SqlParameter>();
                    sqlParmsNew.Add(SqlParms.CreateParameter("RegID", DbType.Int32, registrationId, false));
                    sqlParmsNew.Add(SqlParms.CreateParameter("TaxId", DbType.Int32, taxid, false));
                    sqlParmsNew.Add(SqlParms.CreateParameter("TaxIdTypeId", DbType.Int32, taxidTypeID, false));
                    DataSet dsRegnew = DataAccess.ExecuteStoredProcedure("usp_SelectREG_TAX_HISTORY_By_Tax_id", sqlParmsNew, "RegData");
                    DataTable dtNotes = null;
                    int regtaxHistoryID = 0; DateTime? regTaxEndDate = null;
                    if (dsRegnew.Tables[0].Rows.Count > 0)
                    {
                        dtNotes = dsRegnew.Tables[0];
                        regtaxHistoryID = ObjectControllerHelper.GetInt("REG_TAX_HISTORY_ID", dtNotes.Rows[0]);
                        regTaxEndDate = ObjectControllerHelper.GetDateTime("END_DATE", dtNotes.Rows[0]);
                    }


                    if (dsRegnew.Tables.Count > 0 && dsRegnew.Tables[0].Rows.Count > 0 && (workflowEventTypeID == CON.WorkflowEventType.RevalReg))
                    {
                        //StringBuilder selectPart = new StringBuilder();
                        //selectPart.Append(string.Format("(END_DATE >= '{0}' AND END_DATE <= '{1}') OR ", DateTime.Now,prevRevaldate.AddDays(60)));

                        string MMISproviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
                        if (prevRevaldate > DateTime.Now || prevRevaldate >= DateTime.Now.AddDays(-60) || (MMISproviderTypeID == "01" || MMISproviderTypeID == "02" || MMISproviderTypeID == "03" || MMISproviderTypeID == "28" || MMISproviderTypeID == "86" ||
                            MMISproviderTypeID == "88" || MMISproviderTypeID == "89" || MMISproviderTypeID == "59" || MMISproviderTypeID == "74" || MMISproviderTypeID == "10"))    //SAM635 ADD 59,74 AND 10 TO THIS LIST
                        {
                            //no gap so update the existing record
                            Dictionary<string, string> sqlParmsnew = new Dictionary<string, string>();
                            sqlParmsnew.Add("REG_ID", registrationId.ToString());
                            sqlParmsnew.Add("REG_TAX_HISTORY_ID", regtaxHistoryID.ToString());
                            sqlParmsnew.Add("END_DATE", endDate.ToString());
                            sqlParmsnew.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            sqlParmsnew.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                            RegistrationController.UpdateRegistrationData("tax_history", sqlParmsnew);
                        }
                        if (prevRevaldate < DateTime.Now.AddDays(-60))
                        {
                            //update existing record and insert new record
                            Dictionary<string, string> sqlParmsnew1 = new Dictionary<string, string>();
                            sqlParmsnew1.Add("REG_ID", registrationId.ToString());
                            sqlParmsnew1.Add("REG_TAX_HISTORY_ID", regtaxHistoryID.ToString());
                            sqlParmsnew1.Add("END_DATE", prevRevaldate.ToString());
                            sqlParmsnew1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            sqlParmsnew1.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                            RegistrationController.UpdateRegistrationData("tax_history", sqlParmsnew1);

                            Dictionary<string, string> sqlParmsnew = new Dictionary<string, string>();
                            sqlParmsnew.Add("REG_ID", registrationId.ToString());
                            sqlParmsnew.Add("tax_ID", taxid);
                            sqlParmsnew.Add("tax_id_Type_ID", taxidTypeID.ToString());
                            sqlParmsnew.Add("effecTive_datE", DateTime.Now.ToString());
                            sqlParmsnew.Add("END_DATE", endDate.ToString());
                            //sqlParmsnew.Add("END_DATE", endDate.ToString());
                            sqlParmsnew.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            sqlParmsnew.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                            sqlParmsnew.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                            sqlParmsnew.Add("CREATED_BY_USER", Guid.Parse(Constants.appAdminUserId).ToString());

                            RegistrationController.InsertRegistrationData("tax_history", sqlParmsnew);
                        }
                    }
                    if ((workflowEventTypeID == CON.WorkflowEventType.NewReg) || ((workflowEventTypeID == CON.WorkflowEventType.RevalReg || workflowEventTypeID == CON.WorkflowEventType.Reconsideration) && dsRegnew.Tables[0].Rows.Count == 0))
                    {
                        if (dsRegnew.Tables.Count > 0 && dsRegnew.Tables[0].Rows.Count > 0)
                        {
                            //update existing record
                            Dictionary<string, string> sqlParmsnew1 = new Dictionary<string, string>();
                            sqlParmsnew1.Add("REG_ID", registrationId.ToString());
                            sqlParmsnew1.Add("REG_TAX_HISTORY_ID", regtaxHistoryID.ToString());
                            sqlParmsnew1.Add("END_DATE", prevRevaldate.ToString());
                            sqlParmsnew1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            sqlParmsnew1.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                            RegistrationController.UpdateRegistrationData("tax_history", sqlParmsnew1);
                        }
                        else
                        {
                            Dictionary<string, string> sqlParmsnew = new Dictionary<string, string>();
                            sqlParmsnew.Add("REG_ID", registrationId.ToString());
                            sqlParmsnew.Add("tax_ID", taxid);
                            sqlParmsnew.Add("tax_id_Type_ID", taxidTypeID.ToString());
                            sqlParmsnew.Add("effecTive_datE", effectiveDate.ToString());
                            sqlParmsnew.Add("END_DATE", endDate.ToString());
                            //sqlParmsnew.Add("END_DATE", endDate.ToString());
                            sqlParmsnew.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            sqlParmsnew.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                            sqlParmsnew.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                            sqlParmsnew.Add("CREATED_BY_USER", Guid.Parse(Constants.appAdminUserId).ToString());

                            RegistrationController.InsertRegistrationData("tax_history", sqlParmsnew);
                        }
                    }
                    if ((workflowEventTypeID == CON.WorkflowEventType.CPCReattest))
                    {
                        if (dsRegnew.Tables.Count > 0 && dsRegnew.Tables[0].Rows.Count > 0 && regTaxEndDate.Value.Year == CPC_Prog_Year.Year - 1)
                        {
                            //update existing record
                            Dictionary<string, string> sqlParmsnew1 = new Dictionary<string, string>();
                            sqlParmsnew1.Add("REG_ID", registrationId.ToString());
                            sqlParmsnew1.Add("REG_TAX_HISTORY_ID", regtaxHistoryID.ToString());
                            sqlParmsnew1.Add("END_DATE", endDate.ToString());
                            sqlParmsnew1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            sqlParmsnew1.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                            RegistrationController.UpdateRegistrationData("tax_history", sqlParmsnew1);
                        }
                        else
                        {
                            Dictionary<string, string> sqlParmsnew = new Dictionary<string, string>();
                            sqlParmsnew.Add("REG_ID", registrationId.ToString());
                            sqlParmsnew.Add("tax_ID", taxid);
                            sqlParmsnew.Add("tax_id_Type_ID", taxidTypeID.ToString());
                            sqlParmsnew.Add("effecTive_datE", CPC_Prog_Year.ToString());
                            sqlParmsnew.Add("END_DATE", endDate.ToString());
                            sqlParmsnew.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            sqlParmsnew.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                            sqlParmsnew.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                            sqlParmsnew.Add("CREATED_BY_USER", Guid.Parse(Constants.appAdminUserId).ToString());

                            RegistrationController.InsertRegistrationData("tax_history", sqlParmsnew);
                        }
                    }
                }
                //Update the PNMApplicationStatus
                RegistrationController.UpdatePNMApplicationStatus(registrationId, CON.PSMApplicationStatusID.ACCEPTED, DateTime.Now, new Guid(CON.appAdminUserId),p.ProcessID);

                #region CR096 OHPNM-4561
                if ((!forceWfToReview) && (workflowEventTypeID == CON.WorkflowEventType.NewReg || workflowEventTypeID == CON.WorkflowEventType.UpdateReg
                    || workflowEventTypeID == CON.WorkflowEventType.RevalReg || applicationTypeID == CON.ApplicationType.ORP
                    || applicationTypeID == CON.ApplicationType.MCP || workflowEventTypeID == CON.WorkflowEventType.CPCReattest))
                {
                    Dictionary<string, string> dictParms = new Dictionary<string, string>();
                    dictParms.Add("REG_ID", registrationId.ToString());
                    if (applicationTypeID == CON.ApplicationType.ORP)
                    {
                        dictParms.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.ORPActive.ToString());
                        dictParms.Add("ENROLLMENT_STATUS_REASONS_ID", Constants.EnrollStatusReasonID.ORPNonBilling.ToString());
                    }
                    else if (applicationTypeID == CON.ApplicationType.MCP)
                    {
                        dictParms.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.ORPActive.ToString());
                        dictParms.Add("ENROLLMENT_STATUS_REASONS_ID", Constants.EnrollStatusReasonID.NonParticipatingRPTOnly.ToString());
                    }
                    else
                    {
                        dictParms.Add("ENROLLMENT_STATUS_CODE", Constants.EnrollmentStatusTypeID.Active.ToString());
                        dictParms.Add("ENROLLMENT_STATUS_REASONS_ID", Constants.EnrollStatusReasonID.Active.ToString());
                    }

                    dictParms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
                    dictParms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    dictParms.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
                   
                    RegistrationController.UpdateRegistrationData("specialty_enroll_status", dictParms);
                }
                #endregion
				
				// OHPNM-5521
                if (needToAdjustEnrollmentAndTaxHistoryForGap)
                {
                    List<SqlParameter> parmsup = new List<SqlParameter>();
                    parmsup.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, registrationId, false));
                    parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
                    parmsup.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, CON.appAdminUserId, false));
                    parmsup.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, oldtermDate.ToString(), false));
                    if (stagingTableHasData)
                    {
                        // workflow is using new mechanism; do things the new way
                        parmsup.Add(SqlParms.CreateParameter("START_DATE", DbType.DateTime, enrollStartDateTime.ToString(), false));

                        // The end date for the gap will either be
                        // a) for IsReactivation and update workflow, it'll be the entered reg.POSSIBLE_REVAL_DATE
                        // b) for all other cases, it'll be the calculated end_date (calculated above), which is in the variable "endDate" at this point from when we regrab it
                        
                        if (isReactivation && workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                        {
                            parmsup.Add(SqlParms.CreateParameter("END_DATE", DbType.DateTime, possibleRevalDate.ToString(), false));
                        }
                        else
                        {
                            parmsup.Add(SqlParms.CreateParameter("END_DATE", DbType.DateTime, endDate.ToString(), false));
                        }

                        DataAccess.ExecuteStoredProcedure("usp_ReactivationReapplicationWithGapNewWay", parmsup);
                    }
                    else
                    {
                        // workflow is inflight, use the old way
                        DataAccess.ExecuteStoredProcedure("usp_ReactivationReapplicationWithGap", parmsup);
                    }
                }

				if (workflowEventTypeID == CON.WorkflowEventType.NewReg)
				{
					sqlParms = new List<SqlParameter>();
					sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, false));
					sqlParms.Add(SqlParms.CreateParameter("MEDICAIDID", DbType.String, medicaidid, false));
					sqlParms.Add(SqlParms.CreateParameter("ENTITYTYPEID", DbType.Int32, EntitytypeID, false));
					sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString(), false));
					sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Guid.Parse(Constants.appAdminUserId).ToString(), false));
                    sqlParms.Add(SqlParms.CreateParameter("ENROLL_START_DATE", DbType.DateTime, changeEffectiveDate.ToString(), false));
                    sqlParms.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));

                    DataAccess.ExecuteStoredProcedure("usp_Update_Specialty_Affiliation_StartDate_NewReg", sqlParms);
				}
				
				
                if (workflowID == CON.WorkflowType.CPC && MMISProviderTypeID == "99" &&
                   (workflowEventTypeID == CON.WorkflowEventType.CPCReattest || workflowEventTypeID == CON.WorkflowEventType.NewReg))
                {
                    sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, false));
                    sqlParms.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                    sqlParms.Add(SqlParms.CreateParameter("CPC_PROGRAM_YEAR", DbType.String, CPC_Prog_Year.Year.ToString(), false));
                    sqlParms.Add(SqlParms.CreateParameter("PROVIDER_CATEGORY_TYPE_ID", DbType.Int32, EntitytypeID, false));
                    sqlParms.Add(SqlParms.CreateParameter("CPC_PRACTICE_TYPE", DbType.String, CPCPracticeTypeID, false));
                    sqlParms.Add(SqlParms.CreateParameter("IS_CPC_REATTESTATION", DbType.Boolean, workflowEventTypeID == CON.WorkflowEventType.CPCReattest, false));
                    sqlParms.Add(SqlParms.CreateParameter("IS_CPC_REATTEST_TERM_PROV", DbType.Boolean, !ObjectControllerHelper.IsDateNull(oldtermDate), false));
                    sqlParms.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, oldtermDate.ToString(), false));
                    sqlParms.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, false));
                    sqlParms.Add(SqlParms.CreateParameter("User", DbType.Guid, Guid.Parse(Constants.appAdminUserId), false));

                    DataAccess.ExecuteStoredProcedure("usp_ProcessCPCRegistration", sqlParms, "RegData");

                }
            }
			
            // OHPNM-6800 - if active REG_ACH_REQUEST row is missing EFT_START_DATE and REGISTRATION.CHANGE_EFFECTIVE_DATE isn't null, update EFT_START_DATE with that CHANGE_EFFECTIVE_DATE
            DataSet dsAchRequest = RegistrationController.SelectRegistrationData(registrationId, "ACH_REQUEST");
            if ((dsAchRequest != null && dsAchRequest.Tables.Count > 0 && dsAchRequest.Tables[0].Rows.Count > 0))
            {
                // found an active REG_ACH_REQUEST; check EFT_START_DATE
                DataRow drAchRequest = ObjectControllerHelper.HasRows(dsAchRequest) ? dsAchRequest.Tables[0].Rows[0] : null;
                DateTime achEftStartDate = ObjectControllerHelper.GetDateTime("EFT_START_DATE", drAchRequest);
                if (ObjectControllerHelper.IsDateNull(achEftStartDate))
                {
                    // EFT_START_DATE is null; let's see if the change_effective_date is null
                    DataSet dsRegToFindChangeEffectiveDate = GetProviderRegistrationInfo(registrationId);
                    DataRow drRegToFindChangeEffectiveDate = ObjectControllerHelper.HasRows(dsRegToFindChangeEffectiveDate) ? dsRegToFindChangeEffectiveDate.Tables[0].Rows[0] : null;
                    if (drRegToFindChangeEffectiveDate != null)
                    {
                        // got data back from registration; let's see if change effective date is null
                        DateTime registrationChangeEffectiveDate = ObjectControllerHelper.GetDateTime("ChangeEffectiveDate", drRegToFindChangeEffectiveDate);
                        if (!ObjectControllerHelper.IsDateNull(registrationChangeEffectiveDate))
                        {
                            // CHANGE_EFFECTIVE_DATE of registration isn't null; let's use that for the EFT_START_DATE of the REG_ACH_REQUEST record
                            Dictionary<string, string> achParms = new Dictionary<string, string>();
                            achParms.Add("REG_ID", registrationId.ToString());
                            achParms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            achParms.Add("EFT_START_DATE", registrationChangeEffectiveDate.ToString());
                            achParms.Add("LAST_MODIFIED_USER", Constants.appAdminUserId);
                            achParms.Add("ProcessId", p.ProcessID.ToString()); // have to pass the processId to the SP; a matched processId will do an update; otherwise a new row will get inserted
                            achParms.Add("REG_ACH_REQUEST_ID", Methods.GetStringValue(drAchRequest["REG_ACH_REQUEST_ID"]));
                            RegistrationController.UpdateRegistrationData("ACH_REQUEST", achParms);
                        }
                    }
                }
            }

            DataSet ds = UserController.SelectRegistrationStatuses(Constants.appAdminUserId, registrationId);
            if (ds.Tables.Count == 0)
            {
                throw new Exception(string.Format(
                    Constants.LogString.WorkflowError,
                    Assembly.GetExecutingAssembly().GetName().Name, "Registration Id: " + registrationId.ToString() +
                    " - Expected RegProgramStatusID from usp_SelectRegistrationStatuses when migrating registration data to PDMS Core"));
            }
            // START OF OHPNM-7913
            if (workflowEventTypeID == CON.WorkflowEventType.RevalReg && !isRevertSuspension)
            {
                List<SqlParameter> sqlparameters = new List<SqlParameter>();
                sqlparameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, true));
                DataSet dsRegRestrict = DataAccess.ExecuteStoredProcedure("usp_SelectREG_RESTRICTION", sqlparameters, "regRestrict");
                if (dsRegRestrict.Tables[0].Rows.Count > 0)
                {
                    sqlparameters = new List<SqlParameter>();
                    sqlparameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, true));
                    sqlparameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                    sqlparameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, true));
                    DataAccess.ExecuteStoredProcedure("usp_UPDATE_REG_RESTRICTION", sqlparameters);
                }
            }
            // END OF OHPNM-7913

// START OF OHPNM-12227
            if (workflowEventTypeID == CON.WorkflowEventType.NewReg && mmisProviderTypeID == "LT")
            {
                List<SqlParameter> sqlparameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, true)
                };
                DataAccess.ExecuteStoredProcedure("usp_UpdateProviderEndDateFor_LT_Buildings", sqlparameters);
            }
            //END OF OHPNM-12227

            // PRGCR313
            List<SqlParameter> sqlpar1 = new List<SqlParameter>();
            sqlpar1.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationId, true));
            sqlpar1.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, true));
            sqlpar1.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            sqlpar1.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, true));
            DataAccess.ExecuteStoredProcedure("usp_RELOAD_REG_NPI_MEDID_ENROLLMENT_SPAN", sqlpar1);

            try
            {
                if (workflowEventTypeID == CON.WorkflowEventType.RevalReg && revalidationDate != null)
                {
                    string note = "New Revalidation Date: " + (revalidationDate.HasValue ? revalidationDate.Value.ToString("yyyy-MM-dd") : "N/A");
                    RegistrationController.InsertProviderFeedNotes(registrationId, 0, Constants.appPDMSDataExchangeUserId, note, processID: p.ProcessID);
                }
            }
            catch (Exception ex)
            {
                
            }


            SetProcessParameter(Constants.ProcessParameter.RegProgramStatusTypeID, ds.Tables[0].Rows[0]["TennCareStatusID"].ToString());
            SetProcessParameter(Constants.ProcessParameter.RegistrationStatusTypeID, ds.Tables[0].Rows[0]["REGISTRATION_STATUS_TYPE_ID"].ToString());
            SaveProcessParameters();

        }

        private int GetExitingProviderRegId(int regid)
        {
            DataSet dsChopParent = new DataSet();
            List<SqlParameter> spChop = new List<SqlParameter>();
            spChop.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regid, false));
            dsChopParent = DataAccess.ExecuteStoredProcedure("usp_SelectREG_CHOP_PARENT", spChop, "regCHOPParent");
            int reg_ID_SELL = 0;
            if (ObjectControllerHelper.HasRows(dsChopParent))
            {
                DataRow drCHOPParent = dsChopParent.Tables[0].Rows[0];
                reg_ID_SELL = ObjectControllerHelper.GetInt("REG_ID_SELL", drCHOPParent);

            }
            return reg_ID_SELL;
        }

        private DataSet GetProviderRegistrationInfo(int regid)
        {
            List<SqlParameter> sqlParmsReg = new List<SqlParameter>();
            sqlParmsReg.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regid, false));
            DataSet dsReginfo = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParmsReg, "RegData");
            return dsReginfo;
        }
        private DateTime GetChopFinalDate(int regid)
        {
            DateTime dtChopFinalDate = new DateTime();
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regid, false));
            DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREG_LTC_RISK_ALERT", sqlParms, "regCHOPEffective");

            if (ObjectControllerHelper.HasRows(dsReg))
            {
                DataRow drReg = dsReg.Tables[0].Rows[0];
                string finalDateCHOP = ObjectControllerHelper.GetString("CHOP_Final_Date", drReg);
                if (!string.IsNullOrEmpty(finalDateCHOP))
                {
                    dtChopFinalDate = Convert.ToDateTime(finalDateCHOP);
                }
            }
            return dtChopFinalDate;
        }

        private DateTime GetEnrollmentSpecialistApprovalDate(int regid)
        {
            DateTime dtEnrollmentSpecialistApprovalDate = new DateTime();
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regid, false));
            DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREG_EnrollmentSpecialistApprovalDate", sqlParms, "regEnrollmentSpecialistApproval");

            if (ObjectControllerHelper.HasRows(dsReg))
            {
                DataRow drReg = dsReg.Tables[0].Rows[0];
                string approvalDateES = ObjectControllerHelper.GetString("StepEndDateTime", drReg);
                if (!string.IsNullOrEmpty(approvalDateES))
                {
                    dtEnrollmentSpecialistApprovalDate = Convert.ToDateTime(approvalDateES);
                }
                else
                {
                    dtEnrollmentSpecialistApprovalDate = DateTime.Now;
                }
            }
            else
            {
                dtEnrollmentSpecialistApprovalDate = DateTime.Now;
            }
            return dtEnrollmentSpecialistApprovalDate;
        }
        //private bool IsRequiresCredentialing(string mmisProviderTypeID)
        //{
        //    bool reqCredentialing = false;

        //    if (mmisProviderTypeID == "86")
        //        reqCredentialing = true;

        //    return reqCredentialing;
        //}
        private bool IsHospiceProvider(DataRow dr)
        {
            bool hospiceProvider = false;


            string providerTypeName = ObjectControllerHelper.GetString("ProviderTypeName", dr);


            if (providerTypeName.ToUpper().Contains("HOSPICE"))
            {
                hospiceProvider = true;
            }


            return hospiceProvider;
        }
        //private bool IsHospitalBasedProvider(int regId)
        //{
        //    bool hospitalProvider = false;

        //    List<SqlParameter> sqlParms5 = new List<SqlParameter>();
        //    sqlParms5.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
        //    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_HEALTH_CARE_FACILITY_AFFILIATION", sqlParms5,"HospitalAffiliations");

        //    if (ObjectControllerHelper.HasRows(ds))
        //    {
        //        foreach(DataRow dr in ds.Tables[0].Rows)
        //        {
        //            string IsInpatientSetting = ObjectControllerHelper.GetString("IsInpatientSetting", dr);
        //            if (string.IsNullOrEmpty(IsInpatientSetting) || IsInpatientSetting == "False")
        //                hospitalProvider = false;
        //            else
        //                hospitalProvider = true;
        //        }

        //    }

        //    return hospitalProvider;
        //}

        private void UpdateRegCliaEffectivateByRegistrationEffectiveDate(int regId,DateTime changeEffectiveDate)
        {

            Dictionary<string, string> sqlParms = new Dictionary<string, string>();
            sqlParms.Add("REG_ID", regId.ToString());            
            sqlParms.Add("CLIA_EFF_DATE", changeEffectiveDate.ToString());
            sqlParms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
            sqlParms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            sqlParms.Add("LAST_MODIFIED_USER", Guid.Parse(Constants.appAdminUserId).ToString());
            RegistrationController.UpdateRegistrationData("cliacustom", sqlParms);
        }
        override public string NextStep()
        {
            return nextStep;
            //return null;
        }

    }
}