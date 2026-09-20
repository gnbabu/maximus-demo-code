using MAXIMUS.Core.Libraries;
using System;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFCheckHearingRights : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckHearingRights(int processID, int stepID)
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
                /*
                bool hearingRightsEligible = RegistrationController.CheckIfHearingRightsEligible(regID);
                if (hearingRightsEligible)
                {
                    nextStep = "Yes"; // GoTo (Upload Reconsideration Request)
                    toReturn = true;
                }
                else
                {
                    nextStep = "No"; // GoTo End
                    toReturn = true;
                }
                */
                nextStep = "Yes";
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
    }
}
