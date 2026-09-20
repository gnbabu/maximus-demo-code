using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    class WFCMCAddSpecialtyContract : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCMCAddSpecialtyContract(int processID, int stepID)
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
                    logMsg = string.Format(
                                Constants.LogString.WorkflowParameterNotNumeric,
                                Assembly.GetExecutingAssembly().GetName().Name,
                                Constants.ProcessParameter.RegistrationID,
                                GetProcessParameter(Constants.ProcessParameter.RegistrationID));
                    log.CreateLogEntry(logMsg, Logging.LogPriority.Error);
                }
                else
                {
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                    DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                    dsReg.Tables[0].TableName = "RegData";
                    DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                    int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);

                    if (workflowEventTypeID != Constants.WorkflowEventType.CMCUpdate)
                    {
                        InsertRegSpeciality(CON.MMISSpecialtyType.MATERNALANDINFANTSUPPORT, regID);
                        nextStep = "Next";
                    }
                    else
                    {
                        nextStep = "Update";
                    }
                        
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
        private void InsertRegSpeciality(string MMIS_Code,int regID)
        {            
            RegistrationController.InsertRegSpecialityByMMIS(regID, MMIS_Code, Methods.GetCurrentUserId().ToString());
        }

        override public string NextStep()
        {
            return nextStep;
        }
    }
}
