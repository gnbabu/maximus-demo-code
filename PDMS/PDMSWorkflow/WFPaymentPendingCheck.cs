using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFPaymentPendingCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFPaymentPendingCheck(int processID, int stepID)
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

                DataSet ds = RegistrationController.SelectRegistrationData(regID, "APPLICATION_FEE");
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string paymentType = row["PAYMENT_TYPE_ID"].ToString();
                    string paymentStatus = row["APPLICATION_FEE_STATUS_ID"].ToString();
                    switch (paymentType)
                    {
                        case CON.ApplicationFeePaymentTypeID.RequestWaiver:
                            string waiverReason = row["APPLICATION_FEE_WAIVER_REASON_ID"].ToString();
                            if (waiverReason == CON.WaiverReason.MedicareEnrolled || waiverReason == CON.WaiverReason.PaidinAnotherState || waiverReason == CON.WaiverReason.PaidinThePast5Years || waiverReason == CON.WaiverReason.MedicareEnrollmentPending)
                            {
                                nextStep = "Needs Review";
                                toReturn = true;
                            }
                            break;
                        case CON.ApplicationFeePaymentTypeID.CreditCard:
                        
                            nextStep = "Needs Review";
                            toReturn = true;
                            break;
                    }
                }
                else
                {
                    nextStep = "Next";
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
