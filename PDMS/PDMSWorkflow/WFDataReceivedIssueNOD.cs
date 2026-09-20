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
    public class WFDataReceivedIssueNOD : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFDataReceivedIssueNOD(int processID, int stepID)
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
                    logMsg = string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID));
                    log.CreateLogEntry(logMsg, Logging.LogPriority.Error);
                }
                else
                {                
                    bool checkSuppStatusExists = false;
                    string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                    if (canMakeWSRequest.Equals("true"))
                    {
                        DataSet dsRegTR = RegistrationController.SelectRegApplicationByRegID(regID);
                        DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                        string AppStatus = ObjectControllerHelper.GetString("Application_Status", drRegTR);

                        //dsRegTR = WorkflowController.SelectStepInfo(p.CurrentStepID);
                        //drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                        //DateTime stepCreateDate = ObjectControllerHelper.GetDateTime("STEP_CREATE_DATE_TIME", drRegTR);

                        dsRegTR = RegistrationController.SelectApplicationStatusFromAudit(regID, CON.ApplicationStatus.Supplemental_Application_Required_ID, ProcessID);
                        if (dsRegTR.Tables.Count > 0 && dsRegTR.Tables[0].Rows.Count > 0)
                        {
                            checkSuppStatusExists = true;
                        }
                        if (checkSuppStatusExists)
                        {                        
                            if (AppStatus.Equals(CON.ApplicationStatus.Pending_External_Medicaid_Approval) || AppStatus.Equals(CON.ApplicationStatus.Pending_External_Medicaid_Approval_ID))
                            {
                                nextStep = "Next";
                                toReturn = true;
                            }
                            else
                            {
                                nextStep = "";
                                toReturn = false;
                            }
                        }
                        else
                        {
                            nextStep = "";
                            toReturn = false;
                        }
                    }
                    else
                    {
                        nextStep = "Next";
                        toReturn = true;
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
