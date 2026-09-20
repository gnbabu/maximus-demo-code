using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFSetPagePending : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFSetPagePending(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int rId;
            Process p = new Process(ProcessID);

            try
            {
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out rId))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }
                string idList = GetTaskParameter(Constants.TaskParameter.PageTypeIdList);
                if (string.IsNullOrEmpty(idList))
                {
                    throw new Exception(string.Format(Constants.LogString.WorkflowError,
                        Assembly.GetExecutingAssembly().GetName().Name, "Registration Id: " + rId.ToString() +
                        " - parameter PageTypeIdList does not exist"));
                }

                List<string> PageTypeIDs = new List<string>();
                PageTypeIDs.AddRange(idList.Split(','));
                foreach (string id in PageTypeIDs)
                {
                    RegistrationController.SaveRegistrationPageStatus(rId, Convert.ToInt32(id), null,
                        Constants.RegistrationProviderServicesStatusTypeId.Pending, DateTime.Now, Constants.appAdminUserId, null, null, null, null, null, null, null, null, null, null);
                }
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

