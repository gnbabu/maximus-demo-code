using MAXIMUS.Core.Libraries;
using System;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    /// <summary>
    /// Summary description for WFRequiresReview
    /// </summary>
    public class WFRequiresReview : BaseWorkflowTask, IWorkflowTask
    {
        private string nextStep;

        public WFRequiresReview(int processID, int stepID)
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

            try
            {
                // Get the name of he task that will be seeded with the ownerID.
                int reviewMode;
                if (!int.TryParse(GetTaskParameter(Constants.TaskParameter.ReviewMode), out reviewMode))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.TaskParameter.ReviewMode,
                        GetTaskParameter(Constants.TaskParameter.ReviewMode)));
                }

                //09/17/2015: waiting on final from business on this, until then set to always needs review.
                //switch (reviewMode)
                //{
                //    case Constants.TaskParameter.ReviewModeType.PROVIDER_SERVICES:
                //        // Perform data checks associated with Provider Services Review.
                        nextStep = "Needs Review";
                //        break;
                //    case Constants.TaskParameter.ReviewModeType.ACCOUNTING:
                //        // Perform data checks associated with Accounting Review.
                //        nextStep = "Needs Review";
                //        break;
                //    default:
                //        throw new Exception(string.Format(
                //            Constants.LogString.WorkflowParameterNotValid,
                //            Assembly.GetExecutingAssembly().GetName().Name,
                //            Constants.TaskParameter.ReviewMode,
                //            reviewMode));
                //}

                //Process p = new Process(ProcessID);
                //if (p.WorkflowID == CON.WorkflowType.RegistrationUpdateProvider ||
                //    p.WorkflowID == CON.WorkflowType.RegistrationRevalidation)
                //{
                //    int rId;
                //    if (int.TryParse(p.GetProcessParameter(CON.ProcessParameter.RegistrationID), out rId))
                //    {
                //        DataSet loc = ProviderController.CheckHistoryChanges_REG_SERVICE_LOCATION(rId);
                //        DataSet pro = ProviderController.CheckHistoryChanges_REG_PROVIDER(rId);
                //        int locRowCount = ObjectControllerHelper.HasRows(loc) ? loc.Tables[0].Rows.Count : 0;
                //        int proRowCount = ObjectControllerHelper.HasRows(pro) ? pro.Tables[0].Rows.Count : 0;
                //        nextStep = (locRowCount > 1 || proRowCount > 1) ? "Needs Review" : "No Review Needed";
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return true;
        }
        override public string NextStep()
        {
            return nextStep;
        }
    }
}
