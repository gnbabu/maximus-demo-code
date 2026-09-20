using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;


namespace PDMSWorkflow
{
    public class WFAddContract : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFAddContract(int processID, int stepID)
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
                int ApplicationTypeID = 0;
                int WaiverTypeID = -1;
                string mmisProviderTypeID = string.Empty;
                int workflowEventTypeID = 0;
                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    ApplicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                    WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                    mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
                    workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                }
                if (ApplicationTypeID == CON.ApplicationType.Standard ||
                    ApplicationTypeID == CON.ApplicationType.ORP ||
                    ApplicationTypeID == CON.ApplicationType.Waiver ||
                    ApplicationTypeID == CON.ApplicationType.ChangeOfOperator ||
                    ApplicationTypeID == CON.ApplicationType.MCP ||
                    ApplicationTypeID == CON.ApplicationType.Internal ||
                    ApplicationTypeID == CON.ApplicationType.CPC)
                {
                    //go to contract maintenance step
                    if (mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR)) //LTC 86 or 89
                    {
                        // As Per OHPNM-3217 LTC Providers Should Go to  Contract Maintainance UpdatedDSD on June 14 2021
                        //As per kelly Update WF ltc REGISTRATTION SHOULD N'T GO FOR CONTRACT MAINTENANCE till NOV 2020
                        //if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg) 
                        //{
                        //    nextStep = "Next"; 
                        //    toReturn = true;
                        //}  
                        //else 
                        //{
                        nextStep = "Add LTC Contract";
                        toReturn = true;
                        // }
                    }
                    else
                    {
                        nextStep = "Add Contract";
                        toReturn = true;
                    }
                }
                else
                {
                    //code to add contract system step
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
