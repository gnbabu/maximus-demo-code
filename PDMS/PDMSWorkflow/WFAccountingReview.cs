using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFAccountingReview : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFAccountingReview(int processID, int stepID)
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

                DataSet ds = RegistrationController.CheckRegAccountingReview(rId);
                if (ObjectControllerHelper.HasRows(ds))
                {
                    string result = ds.Tables[0].Rows[0]["Result"].ToString();

                    if (!string.IsNullOrEmpty(result))
                    {
                        if (Convert.ToInt32(result) > 0)
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
                            nextStep = "No Review Needed";
                            bool notMaintenanceStatus = false;
                            bool.TryParse(GetTaskParameter(Constants.TaskParameter.NotMaintenanceStatus), 
                                out notMaintenanceStatus);
                            if (notMaintenanceStatus == false)
                            {
                                // No review needed, set TennCare status as Maintenance
                                RegistrationController.SaveRegistrationProgramStatus(rId, Constants.RegistrationProgramStatusTypeId.Maintenance,  DateTime.Now,
                                    Constants.appAdminUserId);
                            }
                        }
                        toReturn = true;
                    }
                }
                else
                {
                    throw new Exception(string.Format(Constants.LogString.WorkflowError,
                        "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
                        "Registration Id: " + rId.ToString() + " does not have usp_CheckRegAccountingReview result"));
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

