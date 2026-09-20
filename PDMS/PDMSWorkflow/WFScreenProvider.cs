using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Workflow;
using static MAXIMUS.Core.Libraries.Constants;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFScreenProvider : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFScreenProvider(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool returnValue = false;

            int regID = 0;
            try
            {
                Process p = new Process(ProcessID);
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
                string mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
                string enrollmentStatusCode = ObjectControllerHelper.GetString("EnrollmentStatusCode", drReg);
                int ProviderCategorytypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID", drReg);
                bool isRejected = false; bool isDODDAbuserRejected = false;
                string taxId = string.Empty;
                string lastName = string.Empty;
                string dob = string.Empty;
                int ProviderDODDactivityStatus = 0;
                string OwnerDODDActivityStatus = string.Empty;
                int regOwnerId;

                try 
                {
                    if(ProviderCategorytypeID == 1)
                    {
                        taxId = ObjectControllerHelper.GetString("TaxID", drReg);
                        lastName = ObjectControllerHelper.GetString("LastName", drReg);
                        dob = ObjectControllerHelper.GetString("BirthDate", drReg);
                        isRejected = DODDHelper.CheckDODDAbsuerRegistry(regID, taxId, lastName, dob, 0);
                        ProviderDODDactivityStatus = isRejected ? CON.ScreeningActivityStatusId.Failed : CON.ScreeningActivityStatusId.Verified;
                    }
                    else
                    {
                        DataSet dsOwner = RegistrationController.SelectRegistrationData(regID, "owner");
                        DataTable dtIndOwners = new DataTable();

                        if (dsOwner.Tables[0].Rows.Count > 0)
                        {
                            dtIndOwners = dsOwner.Tables[0].AsEnumerable()
                                              .Where(r => (r.Field<int>("REG_OWNER_TYPE_ID") == int.Parse(CON.OwnerType.Person) || r.Field<int>("REG_OWNER_TYPE_ID") == int.Parse(CON.OwnerType.ManagingEmployee)) &&
                                                          r.Field<DateTime>("END_DATE") > DateTime.Now).CopyToDataTable(); 
                            if (dtIndOwners.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtIndOwners.Rows)
                                {
                                    taxId = ObjectControllerHelper.GetString("TAX_ID", dr);
                                    lastName = ObjectControllerHelper.GetString("LAST_NAME", dr);
                                    dob = ObjectControllerHelper.GetString("DOB", dr);
                                    regOwnerId = ObjectControllerHelper.GetInt("REG_OWNER_ID", dr);
                                    isRejected = DODDHelper.CheckDODDAbsuerRegistry(regID, taxId, lastName, dob, regOwnerId);
                                    if (isRejected)
                                        OwnerDODDActivityStatus += regOwnerId + ",";
                                }
                            }
                        }
                    }
                    isDODDAbuserRejected = (ProviderDODDactivityStatus == CON.ScreeningActivityStatusId.Failed || !string.IsNullOrEmpty(OwnerDODDActivityStatus));
                }
                catch (Exception ex)
                {

                    log.CreateLogEntry("Time out in DODDAbuser" + ex.InnerException, Logging.LogPriority.Error);

                }

                returnValue = ScreeningController.InsertProviderScreening(regID, p.WorkflowID, DateTime.Now, Constants.appAdminUserId, ProviderDODDactivityStatus, OwnerDODDActivityStatus, out bool isExactMatchFound, out bool isSoftMatchFound);
                if (isExactMatchFound)
                {
                    log.CreateLogEntry("Auto Terminating Provider as an exact match found regid - " + regID.ToString());

                    string enrollmentStatusReason = string.Empty;
                    //OHPNM-11657
                    DataSet dsScreeningActivityData = ScreeningController.GetProviderScreeningActivityByRegIdAndActivityType(regID, ScreeningActivityTypeId.NPPESVerification);
                    DataRow drScreening = ObjectControllerHelper.HasRows(dsScreeningActivityData) ? dsScreeningActivityData.Tables[0].Rows[0] : null;
                    if (drScreening != null)
                    {
                        int screeningActivityStatusId = ObjectControllerHelper.GetInt("SCREENING_ACTIVITY_STATUS_ID", drScreening);

                        if (screeningActivityStatusId == ScreeningActivityStatusId.NotVerified)
                        {
                            enrollmentStatusReason = EnrollStatusReason.TERMD_NONCOMPLIANCE_RULES;
                        }
                        else
                        {
                            enrollmentStatusReason = isDODDAbuserRejected ? Constants.EnrollStatusReason.STATE_INITIATED_TERMINATION : Constants.EnrollStatusReason.TERMINATEDFEDEXCLUSIONSYSTEM; // 01 - TERMINATED - FED EXCLUSION (SYSTEM)
                        }
                    }
                    else
                    {
                        enrollmentStatusReason = isDODDAbuserRejected ? Constants.EnrollStatusReason.STATE_INITIATED_TERMINATION : Constants.EnrollStatusReason.TERMINATEDFEDEXCLUSIONSYSTEM; // 01 - TERMINATED - FED EXCLUSION (SYSTEM)
                    }

                    ProviderController.TerminateProvider(regID, DateTime.Now, Constants.appAdminUserId, CON.EnrollStatus.INACTIVE.ToString(), enrollmentStatusReason, true, false);

                    //if (enrollmentStatusCode != "2")
                    //{
                    //    ProviderController.TerminateProvider(regID, DateTime.Now, Constants.appAdminUserId, CON.EnrollStatus.INACTIVE.ToString(), enrollmentStatusReason, true, false);
                    //}
                    nextStep = "Auto Terminate";
                    if (enrollmentStatusCode == "2")
                    {
                        /*nextStep = "Complete Workflow"*/
                        
                        WorkflowController.SaveProcessParameter(ProcessID, "IS_TERMINATED", "1");
                    }

                }
                else if (isSoftMatchFound && p.WorkflowID != Constants.WorkflowType.CHOP) //SAM763
                {
                    nextStep = "Soft Match";
                    WorkflowController.SaveProcessParameter(ProcessID, CON.ProcessParameter.ReferToComplianceReasonID, CON.ReferToComplianceReasons.ExclusionReview.ToString());
                }
                else if (mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR)) //LTC 86 or 89
                {
                    nextStep = "LTC Provider";
                }
                else
                {
                    nextStep = "Next";
                }
                // DODDHelper.UpdateDODDActivityStatus(regID, verified);
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                log.CreateLogEntry("Error Creating Screening records for RegistrationId: " + regID.ToString()
                                    + " Exception Message " + ex.Message + " Exception Stack = "
                                    + ex.StackTrace, Logging.LogPriority.Error);
            }

            return returnValue;
        }
        override public string NextStep()
        {
            return nextStep;
        }
    }
}
