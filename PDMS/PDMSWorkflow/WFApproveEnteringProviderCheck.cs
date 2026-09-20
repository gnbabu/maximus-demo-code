using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFApproveEnteringProviderCheck : BaseWorkflowTask, IWorkflowTask
    {
  
        string nextStep = null;

        public WFApproveEnteringProviderCheck(int processID, int stepID)
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
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREG_LTC_RISK_ALERT", sqlParms, "regCHOPEffective");

                sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                bool isNotProcessed = Convert.ToBoolean(DataAccess.ExecuteStoredProcedure("usp_CheckifEnteringProviderNotProcessed", sqlParms, "IsNotProcessed", SqlDbType.Bit, 100));

                if (ObjectControllerHelper.HasRows(dsReg))
                {
                    DataRow drReg = dsReg.Tables[0].Rows[0];
                    string finalDateCHOP = ObjectControllerHelper.GetString("CHOP_Final_Date", drReg);

                    if (isNotProcessed)
                    {
                        log.CreateLogEntry("Mark CHOP as withdrawn- " + regID.ToString());
                        sqlParms = new List<SqlParameter>();
                        sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                        sqlParms.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, false));
                        sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now.ToString("d"), false));
                        sqlParms.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, false));
                        DataAccess.ExecuteStoredProcedure("usp_MarkCHOPWithdrawn", sqlParms, "regCHOPEffective");

                        toReturn = true;
                        nextStep = "Not Processed";
                    }
                    else if (!string.IsNullOrEmpty(finalDateCHOP))
                    {
                        //OHPNM-18368 Exiting Provider WF should not complete when Entering Provider is Not Approved 
                        sqlParms = new List<SqlParameter>();
                        sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                        bool isEnteringProviderApproved = Convert.ToBoolean(DataAccess.ExecuteStoredProcedure("usp_CheckifEnteringProviderApproved", sqlParms, "IsApproved", SqlDbType.Bit, 100));
                        if (isEnteringProviderApproved)
                        {
                            //if approved
                            //Enddate the exiting provider
                            log.CreateLogEntry("Terminating Provider exiting provider regid - " + regID.ToString());
                            string enrollmentStatusReason = Constants.EnrollStatusReason.INACTIVECHOP.ToString();
                            DateTime endateExiting = Convert.ToDateTime(finalDateCHOP);
                            endateExiting = endateExiting.AddDays(-1);
                            ProviderController.TerminateProvider(regID, endateExiting, Constants.appAdminUserId, Constants.EnrollStatus.INACTIVE.ToString(), enrollmentStatusReason, true);

                            toReturn = true;
                            nextStep = "Next";
                        }
                        else
                        {
                            log.CreateLogEntry("Entering Provider is not approved for this exiting provider regid - " + regID.ToString());
                        }
                    }
                    else
                    {
                        //if not approved
                        toReturn = true;
                        nextStep = "Risk Alert";

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

