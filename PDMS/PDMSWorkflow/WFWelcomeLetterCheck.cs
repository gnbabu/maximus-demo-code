using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFWelcomeLetterCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFWelcomeLetterCheck(int processID, int stepID)
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

                DataSet ds = RegistrationController.SelectWorkflowEventTypeByRegId(rId);
                int workflowEventTypeID = 0;

                if (ObjectControllerHelper.HasRows(ds))
                {
                    DataTable dt =   ObjectControllerHelper.HasRows(ds) ? ds.Tables["WorkflowEventType"] : null;

                    if (ObjectControllerHelper.HasRows(dt))                    
                        workflowEventTypeID= ObjectControllerHelper.GetInt("workflow_event_type_id",dt.Rows[0]);
                }
                // If we have already sent a Welcome Letter then do not send one
               // DataSet ds = RegistrationController.SelectRegistrationEmails(rId, "Welcome");
                //if (ObjectControllerHelper.HasRows(ds))
                if (workflowEventTypeID == CON.WorkflowEventType.NewReg)
                {
                    nextStep = "Send Welcome Letter";
                    log.CreateLogEntry("Registration Id: " + rId.ToString() + " - send Welcome Letter");
                }
                else if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                {
                    nextStep = "Send Re-enrollment Letter";
                    log.CreateLogEntry("Registration Id: " + rId.ToString() + " - send Re-enrollment Letter");
                }
                else
                {    nextStep = "No Send";
                    log.CreateLogEntry("Registration Id: " + rId.ToString() + " - Update Registraion, no letter sent");
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
