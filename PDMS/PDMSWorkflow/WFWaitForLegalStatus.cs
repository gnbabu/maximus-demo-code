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
    public class WFWaitForLegalStatus : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        static List<string> m_ApplicationStatusIDs = new List<string>();
        public WFWaitForLegalStatus(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int regID;
            int additionalApplicationID;
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
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID), out additionalApplicationID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID)));
                }

                // List<SqlParameter> sqlParms = new List<SqlParameter>();
                // sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                // DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                // dsReg.Tables[0].TableName = "RegData";
                // DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;

                string makeRequest = AppSettings.Get("MakeWSRequestCallToSI");
                if (makeRequest.ToLower().Equals("true"))
                {
                    List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                    sqlParmsTR.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, false));
                    sqlParmsTR.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, false));
                    sqlParmsTR.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, false));
                    DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDITIONAL_APPLICATIONCustom", sqlParmsTR, "RegAddApplication");
                    dsRegTR.Tables[0].TableName = "RegAddApplication";
                    DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                    string OtherAppStatusID = ObjectControllerHelper.GetString("OTHER_APPLICATION_STATUS_ID", drRegTR);
                    string OtherAppLegalStatusID = ObjectControllerHelper.GetString("OTHER_APPLICATION_LEGAL_STATUS_ID", drRegTR);
	
					// OHPNM-6301 - See if we have gotten the legal status yet; we got here because we are in Closed Partial Approval status without a legal status
					if (OtherAppStatusID.Equals(CON.ApplicationStatus.Closed_Partial_Approval) && IsApplicationStatus(OtherAppLegalStatusID))
					{
						nextStep = "Check Waiver Queue"; // take them to waiver queue now to move things forward
						
						List<SqlParameter> param = new List<SqlParameter>();
						param.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
						param.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, true));
						param.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, true));
						param.Add(SqlParms.CreateParameter("USER", DbType.Guid, CON.appAdminUserId, true));
						param.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, true));
						DataAccess.ExecuteStoredProcedure("usp_UpdateWaiverQueuedTransactionStatus", param);
						toReturn = true;
					}
                }
                else
                {
                    nextStep = "Check Waiver Queue";
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
        static List<string> ApplicationStatusIDs()
        {
            if (m_ApplicationStatusIDs.Count == 0)
            {
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Closed_By_ODM);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Certified);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Closed_As_Legal_Override);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Approved_ID);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.ClosedAsAdjudicationOrderIssued);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Closed_As_Complete_ID);
            }
            return m_ApplicationStatusIDs;
        }

        public static bool IsApplicationStatus(string statusName)
        {
            return ApplicationStatusIDs().Contains(statusName);
        }

    }
}
