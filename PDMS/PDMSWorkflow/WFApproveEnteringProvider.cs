using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFApproveEnteringProvider : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFApproveEnteringProvider(int processID, int stepID)
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
                DataSet dsCHOPParent = DataAccess.ExecuteStoredProcedure("usp_SelectREG_CHOP_PARENT", sqlParms, "regCHOPParent");
                int reg_ID_SELL = 0;
                if (ObjectControllerHelper.HasRows(dsCHOPParent))
                {
                    DataRow drCHOPParent = dsCHOPParent.Tables[0].Rows[0];
                    reg_ID_SELL = ObjectControllerHelper.GetInt("REG_ID_SELL", drCHOPParent);
                }

                List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                sqlParms1.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_ID_SELL, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREG_LTC_RISK_ALERT", sqlParms1, "regCHOPDate");
                
                if (ObjectControllerHelper.HasRows(dsReg))
                {
                    DataRow drReg = dsReg.Tables[0].Rows[0];
                    DateTime finalDateCHOP = ObjectControllerHelper.GetDateTime("CHOP_Final_Date", drReg);
                    if (finalDateCHOP!=null && !ObjectControllerHelper.IsDateNull(finalDateCHOP))
                    {   
                        //Effective CHOP date has been placed; clear to proceed.

                        DateTime dtEffectiveForEntering = finalDateCHOP;
                        List<SqlParameter> sqlParmsDate = new List<SqlParameter>();
                        sqlParmsDate.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                        sqlParmsDate.Add(SqlParms.CreateParameter("CHANGE_EFFECTIVE_DATE", DbType.DateTime, dtEffectiveForEntering, false));
                        DataAccess.ExecuteStoredProcedure("updateREGISTRATION", sqlParmsDate);
                        //Update Enrollment span for entering provider


                        toReturn = true;
                        nextStep = "Approve Entering Provider";
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
