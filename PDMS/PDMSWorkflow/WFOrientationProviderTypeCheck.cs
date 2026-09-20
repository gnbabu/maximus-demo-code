using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFOrientationProviderTypeCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFOrientationProviderTypeCheck(int processID, int stepID)
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

                string ProviderTypeAbbr = "";
                DataSet ds = RegistrationController.SelectRegistrationByRegID(regID);
                if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //providerCategoryID = int.Parse(ds.Tables[0].Rows[0]["ProviderCategoryTypeID"].ToString());
                    ProviderTypeAbbr = ds.Tables[0].Rows[0]["ProviderTypeAbbr"].ToString();
                }

                //if (providerCategoryID == Constants.ProviderCategoryTypeID.Pharmacy)
                //{
                //    nextStep = "Send Pharma Email";
                //    toReturn = true;
                //}
                //else 
                if (ProviderTypeAbbr == "DME")
                {
                    nextStep = "Send DME Email";
                    toReturn = true;
                }
                else
                {
                    nextStep = "Send Non-DME Email";
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
