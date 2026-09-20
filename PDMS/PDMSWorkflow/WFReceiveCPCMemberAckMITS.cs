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
    public class WFReceiveCPCMemberAckMITS : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        public WFReceiveCPCMemberAckMITS(int processID, int stepID)
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
                int workflowEventTypeID = 0;
                string CPCPracticeTypeID = string.Empty;

                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                }                

                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    sqlParms.Add(SqlParms.CreateParameter("WORKFLOW_EVENT_TYPE_ID", DbType.Int32, workflowEventTypeID, false));
                    int isReceviedAck = Convert.ToInt32(DataAccess.ExecuteScalar("usp_CheckCPCMembers_ReceiveACK", sqlParms));

                    if (isReceviedAck == 1)
                    {
                        if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                        {
                            nextStep = "Update Event";
                            toReturn = true;
                        }                            
                        else
                        {
                            nextStep = "Transaction Passed";
                            toReturn = true;
                        }                            
                    }                    
                    else if (isReceviedAck == 0)
                    {
                        nextStep = "Transaction Failed";
                        toReturn = true;
                    }
                    else if (isReceviedAck == 2)
                    {
                        nextStep = "";
                        toReturn = false;
                    }                        

                }
                else
                {
                    if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                    {
                        nextStep = "Update Event";
                        toReturn = true;
                    }                        
                    else
                    {
                        nextStep = "Transaction Passed";
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
