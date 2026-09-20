using System;
using Workflow;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System.Reflection;
using System.Data;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;


namespace PDMSWorkflow
{
    public class WFReturnToWorkflow : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFReturnToWorkflow(int processID, int stepID)
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
                int entryTaskID = Constants.ComplianceBackToStandardWF;
                Workflow.Process pr = new Workflow.Process(Constants.WorkflowType.RegistrationNew, Constants.appAdminUserId, Guid.NewGuid(), entryTaskID);
               
                int nProcessID = pr.ProcessID;
                int nCurrentStepID = pr.CurrentStepID;
                int nCurrentTaskID = pr.TaskID;
                int nWorkflowID = pr.WorkflowID;

                if (nProcessID > 0 && nWorkflowID > 0)
                {
                    // Save the Registration ID as a process parameter of the Workflow
                    WorkflowController.SaveProcessParameter(nProcessID, "REGISTRATION_ID", regID.ToString());
                    nextStep = "End";
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

    }
}
