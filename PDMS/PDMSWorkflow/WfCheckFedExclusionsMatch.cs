using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;


namespace PDMSWorkflow
{
    public class WfCheckFedExclusionsMatch : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WfCheckFedExclusionsMatch(int processID, int stepID)
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
                    bool isMatch = ScreeningController.CheckFedExclusionsMatch(regID);
                    if (isMatch)
                    {
                        //#region OHPNM-4561
                        //Dictionary<string, string> dictParms = new Dictionary<string, string>();
                        //dictParms.Add("REG_ID", regID.ToString());
                        //dictParms.Add("END_DATE", DateTime.Now.ToString("MM/dd/yyyy"));
                        //dictParms.Add("ENROLLMENT_STATUS_CODE", CON.EnrollmentStatusTypeID.InActive.ToString());
                        //dictParms.Add("ENROLLMENT_STATUS_REASONS_ID", CON.EnrollStatusReasonID.TerminatedFedExclusionSystem.ToString());
                        //dictParms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
                        //dictParms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        //dictParms.Add("LAST_MODIFIED_USER", Guid.Parse(CON.appAdminUserId).ToString());

                        //RegistrationController.UpdateRegistrationData("specialty_enroll_status", dictParms);
                        //#endregion

                        //Send Fed Exclusion Notice
                        nextStep = "Yes";
                        toReturn = true;
                    }
                    else
                    {
                        //Do not Send Fed Exclusion Notice
                        nextStep = "No";
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
