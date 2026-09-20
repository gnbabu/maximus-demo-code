using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFApprovedServicesCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFApprovedServicesCheck(int processID, int stepID)
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

                // Only send for DDS (IDDD) Waiver providers and EPD Waiver providers
                if ((workflowEventTypeID == CON.WorkflowEventType.NewReg || workflowEventTypeID == CON.WorkflowEventType.RevalReg) 
                    && (IsIDDDWaiverProvider(rId) || WorkflowID == Constants.WorkflowType.EPD))
                {
                    nextStep = "Send Approved Services";
                    log.CreateLogEntry("Registration Id: " + rId.ToString() + " - send Approved Services");
                }
                else
                {    
                    nextStep = "No Send";
                    log.CreateLogEntry("Registration Id: " + rId.ToString() + " - Update Registration, no approved services letter sent");
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

        public static bool IsIDDDWaiverProvider(int regId)
        {
            bool idddWaiverProvider = false;
            DataSet ds = RegistrationController.SelectRegistrationData(regId, "PROVIDER");
            if (ObjectControllerHelper.HasRows(ds))
            {
                DataTable reg = ds.Tables[0];
                if (reg.Rows.Count > 0)
                {
                    DataRow dr = reg.Rows[0];

                    // Get provider type
                    string IDDDWaiverProviderTypeID = AppSettings.Get("IDDDWaiverProviderMMISTypeID");
                    string providerTypeID = ObjectControllerHelper.GetString("MMIS_Provider_Type_ID", dr);

                    if (IDDDWaiverProviderTypeID == providerTypeID)
                    {
                        idddWaiverProvider = true;
                    }
                }
            }
            
            return idddWaiverProvider;
        }

    }
}
