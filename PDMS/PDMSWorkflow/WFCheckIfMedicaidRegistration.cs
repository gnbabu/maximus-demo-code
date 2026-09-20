using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFCheckIfMedicaidRegistration : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckIfMedicaidRegistration(int processID, int stepID)
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
                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                
                if (drReg != null)
                {
                    string mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
                    if(mmisProviderTypeID == "SL" || mmisProviderTypeID == "LI" || mmisProviderTypeID == "OP" || mmisProviderTypeID == "SB")
                    {
                        RegistrationController.UpdatePNMApplicationStatus(regID, CON.PSMApplicationStatusID.ACCEPTED, DateTime.Now, new Guid(CON.appAdminUserId), p.ProcessID);
                        nextStep = "No";
                        toReturn = true;
                    }
                    else
                    {
                        nextStep = "Yes";
                        toReturn = true;
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
