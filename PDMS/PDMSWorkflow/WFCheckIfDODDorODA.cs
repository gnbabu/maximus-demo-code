using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;


namespace PDMSWorkflow
{
    public class WFCheckIfDODDorODA : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckIfDODDorODA(int processID, int stepID)
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
                int providerScreening = 1;
                string mediacidID = string.Empty;
                int eventTypeID = 0;
                int ApplicationTypeID = 0;
                int WaiverTypeID = -1;
                int WaiverServiceUpdateTypeID = -1;
                bool HasODASpecialty = false;
                DataSet dsTQ = null;
                string txnResult = string.Empty;

                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if(drReg != null)
                {
                    mediacidID = ObjectControllerHelper.GetString("MedicaidID", drReg);
                    eventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                    ApplicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                    WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                    WaiverServiceUpdateTypeID = ObjectControllerHelper.GetInt("WaiverServiceUpdateTypeID", drReg);
                    HasODASpecialty = ObjectControllerHelper.GetBool("HasODASpecialty", drReg);
                }
                //nextStep = "Next";
                //toReturn = true;
                bool hasMedicaidID = false;
                bool isNewReg = eventTypeID == CON.WorkflowEventType.NewReg;
                bool isUpdateReg = eventTypeID == CON.WorkflowEventType.UpdateReg;
                if (!string.IsNullOrEmpty(mediacidID))
                {
                    hasMedicaidID = true;
                }
                bool screenComplete = ScreeningController.ScreeningComplete(regID, CON.ScreeningFor.Provider, providerScreening);

                if((isNewReg && !hasMedicaidID && !screenComplete && ApplicationTypeID == CON.ApplicationType.Waiver && WaiverTypeID == CON.WaiverApplicationTypeID.ODA) ||
                        (isUpdateReg && WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODA))
                {
                    dsTQ  = TransactionController.SelectRescentTransactionIDByRegID(regID);                    
                    if (dsTQ.Tables[0].Rows.Count > 0)
                    {
                        txnResult = dsTQ.Tables[0].Rows[0]["SI_RESPONSE_CODE"].ToString();
                    }
                    if (!string.IsNullOrEmpty(txnResult))
                    {                    
                        if (txnResult.Equals(CON.ResponseCodes.WAIVER_SI_SUCCESS) || txnResult.Equals(CON.ResponseCodes.WAIVER_SI_SUCCESS_ACK))
                        {
                            nextStep = isNewReg ? "New Event" : "ODA Update";// CON.TransactionResult.TransactionPassed;
                            toReturn = true;
                        }
                        else
                        {
                            nextStep = CON.TransactionResult.TransactionFailed;
                            toReturn = true;
                        }
                    }
                }
                else if ((isNewReg && !hasMedicaidID && !screenComplete && ApplicationTypeID == CON.ApplicationType.Waiver && (WaiverTypeID == CON.WaiverApplicationTypeID.DODD || WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD))
                         ||(isUpdateReg && WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD))
                {
                    dsTQ = TransactionController.SelectRescentTransactionIDByRegID(regID);
                    if (dsTQ.Tables[0].Rows.Count > 0)
                    {
                        txnResult = dsTQ.Tables[0].Rows[0]["SI_RESPONSE_CODE"].ToString();
                    }
                    if (!string.IsNullOrEmpty(txnResult))
                    {
                        if (txnResult.Equals(CON.ResponseCodes.WAIVER_SI_SUCCESS) || txnResult.Equals(CON.ResponseCodes.WAIVER_SI_SUCCESS_ACK))
                        {
                            nextStep = isNewReg ? "New Event" : "DODD Update";// CON.TransactionResult.TransactionPassed;
                            toReturn = true;
                        }
                        else
                        {
                            nextStep = CON.TransactionResult.TransactionFailed;
                            toReturn = true;
                        }
                    }
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
