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
    public class WFProviderReviewComplete : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFProviderReviewComplete(int processID, int stepID)
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

                // Get workflow event type
                int workflowEventTypeID = -1;
                List<SqlParameter> sqlParmsReg = new List<SqlParameter>();
                sqlParmsReg.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParmsReg, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                bool isReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", drReg);

                bool isProviderDisenrolling = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.IsProviderDisenrolling))
                       ? false : Convert.ToBoolean(p.GetProcessParameter(Constants.ProcessParameter.IsProviderDisenrolling));

                if (isProviderDisenrolling)
                {
                    List<SqlParameter> sqlParmsDisenroll = new List<SqlParameter>();
                    sqlParmsDisenroll.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    DataSet dsRegDisenroll = DataAccess.ExecuteStoredProcedure("usp_CheckDisEnrollmentByRegID", sqlParmsDisenroll, "RegDataRPD");
                    dsRegDisenroll.Tables[0].TableName = "RegDataRPD";
                    if (dsRegDisenroll.Tables[0].Rows.Count > 0)
                    {
                        //Jira OHPNM-897: 10/28/2020 Pschwarz added row and fetch of disenrollment date 
                        DataRow drDisenroll = dsRegDisenroll.Tables[0].Rows[0];
                        DateTime disenrollDate = new DateTime();
                        if (!drDisenroll.IsNull("DISENROLLMENT_DATE"))
                        {
                            disenrollDate = ObjectControllerHelper.GetDateTime("DISENROLLMENT_DATE", drDisenroll);
                        }

                        //11/8/2020 pschwarz added CON.EnrollmentStatusTypeID.InActive.ToString() to disenroll parameters 
                        RegistrationController.DisenrollProvider(regID, disenrollDate, string.Empty, DateTime.Now, Guid.Parse(CON.appAdminUserId), false, CON.EnrollmentStatusTypeID.InActive.ToString());
                        nextStep = "Disenroll Send to SI";
                        toReturn = true;
                    }
                }
                else
                {
                    //For SAM-764
                    string isAddBCIRcTPEmail = p.GetProcessParameter(Constants.ProcessParameter.IsAddBCIRTPEmail);
                    if (!string.IsNullOrEmpty(isAddBCIRcTPEmail) && Convert.ToInt32(isAddBCIRcTPEmail) > 0)
                    {
                        nextStep = "Background Check Pending";
                        toReturn = true;
                    }
                    else
                    {
                        //SAM505  
                        string showRTPBCICheckBox = p.GetProcessParameter(Constants.ProcessParameter.ShowRTPBCICheckBox);
                        bool isWaiverProvAddODMSpec = string.IsNullOrEmpty(showRTPBCICheckBox) ? false : Convert.ToBoolean(showRTPBCICheckBox);

                        // check for change in ownership
                        //  OHPNM-8788 - In Update WF, looks like presently, changes to an existing Owner is also making WF to trigger BCII. Per DSD 2.28 only newly added individual owners should kick the BCI for high risk providers
                        bool backgrounCheckNeeded = false;
                        if ((workflowEventTypeID == CON.WorkflowEventType.UpdateReg && RegistrationController.HasOwnerInsertedForHighRisk(regID)) ||
                            (workflowEventTypeID != CON.WorkflowEventType.UpdateReg && RegistrationController.HasOwnerChangedOrInsertedForHighRisk(regID)) ||
                            isWaiverProvAddODMSpec)
                        {
                            backgrounCheckNeeded = true;
                        }

                        if (backgrounCheckNeeded)
                        {
                            // if yes then BCII WF
                            nextStep = "Return To Background Check";
                            toReturn = true;
                        }
                        else
                        { // SAM538 For updates, if any Primary Service location or Other Service Location changes have been made , queue for the Site Visit according PT/ST SiteVisit mapping.
                            int requiresSiteVisit = ScreeningController.RegistrationRequiresSiteVisit(regID);

                            if(requiresSiteVisit == 1 && workflowEventTypeID == CON.WorkflowEventType.UpdateReg && !isReactivation)
                            {
                                requiresSiteVisit = RegistrationController.HasPracticeChangeForHighRisk(regID) ? 1 : 0;                               
                            }  
                            if (requiresSiteVisit == 1)
                            {
                                //send to Site Visit
                                ScreeningController.InsertProviderSiteVisitActivity(regID, DateTime.Now, Constants.appAdminUserId);
                                RegistrationController.UpdateRegistrationStatusType(regID, Constants.RegistrationStatusTypeId.SiteVisit, DateTime.Now, Constants.appAdminUserId);
                                nextStep = "Return To Site Visit";
                            }
                            else
                            {
                                nextStep = "No Change";
                            }
                            toReturn = true;
                            
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

    }
}
