using MAXIMUS.Core.Libraries;
using System;
using System.Reflection;

namespace Workflow
{
    /// <summary>
    /// Summary description for WFSendEmails
    /// </summary>
    public class WFSendEmails : BaseWorkflowTask, Workflow.IWorkflowTask
    {
        public WFSendEmails(int processID, int stepID)
            : base(processID, stepID)
        {
            // TODO: Add constructor logic here
        }
        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool result = false;


            try
            {
                /// FIXME: Need to loop over list of recipients to send an email to each affiliated provider.

                /// FIXME: Where will list of recipient REALLY come from?
                string receipients = GetProcessParameter("Receipients");
                string body = GetTaskParameter("BODY_TEMPLATE");
                string subject = GetTaskParameter("EMAIL_SUBJECT");

                EMailNotification notify = new EMailNotification(body, subject, receipients, LogThreadID);
                notify.SendNotification(false);

                result = true;
            }
            catch
            {
                // no action
            }
            return result;
        }
        override public string NextStep()
        {
            return null;
        }
    }
}