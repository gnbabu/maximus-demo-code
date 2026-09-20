using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFBillingIndicator : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFBillingIndicator(int processID, int stepID)
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

            bool toReturn = false;

            int rId = 0;
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

                DataSet ds = RegistrationController.SelectREG_ACH_REQUEST(rId);
                if (ObjectControllerHelper.HasRows(ds))
                {
                    string intend = ds.Tables[0].Rows[0]["INTEND_TO_RECEIVE_MCC"].ToString();

                    if (!string.IsNullOrEmpty(intend))
                    {
                        bool billingIndicator;
                        if (bool.TryParse(intend, out billingIndicator))
                        {
                            if (billingIndicator)
                            {
                                nextStep = "Needs Review";
                                // Set both the W-9 and ACH page ready for review by the Accounting team
                                RegistrationController.SaveRegistrationPageStatus(rId, Constants.RegistrationPageType.SubstituteW9Form, null,
                                    Constants.RegistrationProviderServicesStatusTypeId.Pending, DateTime.Now, Constants.appAdminUserId, null, null, null, null, null, null, null, null, null, null);
                                RegistrationController.SaveRegistrationPageStatus(rId, Constants.RegistrationPageType.ACHAuthorization, null,
                                    Constants.RegistrationProviderServicesStatusTypeId.Pending, DateTime.Now, Constants.appAdminUserId, null, null, null, null, null, null, null, null, null, null);
                            }
                            else
                            {
                                nextStep = null;
                                // No review needed, set TennCare status as Maintenance
                                RegistrationController.SaveRegistrationProgramStatus(rId, Constants.RegistrationProgramStatusTypeId.Maintenance,  DateTime.Now,
                                    Constants.appAdminUserId);
                            }
                            toReturn = true;
                        }
                    }
                }
                else
                {
                    // If there is no REG_ACH_REQUEST entry then the user did not enter on this screen so by pass with default to FALSE.
                    nextStep = null;
                    // No review needed, set TennCare status as Maintenance
                    RegistrationController.SaveRegistrationProgramStatus(rId, Constants.RegistrationProgramStatusTypeId.Maintenance, DateTime.Now,
                        Constants.appAdminUserId);
                    toReturn = true;
                }
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, "RegistrationId: " + rId.ToString() + " - " + logMsg);
            }
            return toReturn;
        }
        override public string NextStep()
        {
            return nextStep;
        }
    }
}

