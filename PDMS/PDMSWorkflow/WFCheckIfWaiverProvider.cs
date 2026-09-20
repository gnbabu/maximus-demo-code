using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFCheckIfWaiverProvider : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckIfWaiverProvider(int processID, int stepID)
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
                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    ApplicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                    WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                }
               
                if (ApplicationTypeID == CON.ApplicationType.Waiver && WaiverTypeID == CON.WaiverApplicationTypeID.ODA)
                {
                    nextStep = "SendToODA";
                    toReturn = true;
                }
                else if (ApplicationTypeID == CON.ApplicationType.Waiver && (WaiverTypeID == CON.WaiverApplicationTypeID.DODD || WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD))
                {
                    nextStep = "SendToDODD";
                    toReturn = true;
                }
                else
                {
                    nextStep = "SendToSI";
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
