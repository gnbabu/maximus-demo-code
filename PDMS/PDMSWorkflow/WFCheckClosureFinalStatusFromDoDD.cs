using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFCheckClosureFinalStatusFromDoDD : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckClosureFinalStatusFromDoDD(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;
            int regID;
            int sId = 0;
            string mId = string.Empty;
            string txnResult = string.Empty;
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
                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                    sqlParmsTR.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, false));
                    DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDITIONAL_APPLICATION", sqlParmsTR, "RApplication");
                    dsRegTR.Tables[0].TableName = "RApplication";
                    DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                    string AppStatus = ObjectControllerHelper.GetString("OTHER_APPLICATION_STATUS_DESC", drRegTR);
                    bool IsDDClosureDateChanged = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.IsDDClosureDateChanged))
                        ? false : Convert.ToBoolean(p.GetProcessParameter(Constants.ProcessParameter.IsDDClosureDateChanged));

                    if (AppStatus.Equals(CON.ApplicationStatus.ClosedAsDenied) || AppStatus.Equals(CON.ApplicationStatus.ClosedAsExpired) 
                        || AppStatus.Equals(CON.ApplicationStatus.ClosedAsWithdrawnbyProvider))
                    {
                        nextStep = "Withdrawn";
                        toReturn = true;
                    }
                    else if (AppStatus.Equals(CON.ApplicationStatus.Approved) || AppStatus.Equals(CON.ApplicationStatus.Closed_As_Complete) )
                    {
                        nextStep = "Approved";
                        toReturn = true;
                    }
                    else if (IsDDClosureDateChanged)
                    {
                        nextStep = "ChangedProEffDate";
                        SetProcessParameter(Constants.ProcessParameter.IsDDClosureDateChanged, "0");
                        SaveProcessParameters();
                        toReturn = true;
                    }
                    else
                    {
                        nextStep = "";
                        toReturn = false;
                    }

                }
                else
                {
                    nextStep = "Approved";
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
