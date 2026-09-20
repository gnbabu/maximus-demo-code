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
    public class WFCheckSendPayloadODA : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckSendPayloadODA(int processID, int stepID)
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
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                bool HasODASpecialty = ObjectControllerHelper.GetBool("HasODASpecialty", drReg);
                bool HasActiveODASpecialty = ObjectControllerHelper.GetBool("HasActiveODASpecialty", drReg);
                int workflowID = ObjectControllerHelper.GetInt("WORKFLOWID", drReg);
                bool isTermed = !string.IsNullOrEmpty(ObjectControllerHelper.GetString("TerminationDate", drReg)) ? true : false;
                string medicaidID = ObjectControllerHelper.GetString("MedicaidID", drReg);
                int registrationStatusTypeID = ObjectControllerHelper.GetInt("RegistrationStatusTypeID", drReg);

                bool isWaiverServiceUpdate = false;
				bool isReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", drReg);
				bool isReapplication = ObjectControllerHelper.GetBool("isreapplication", drReg);
				bool isReconsideration = false;

                int WaiverServiceUpdateTypeID = 0;
                if (!string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId)))
                {
                    WaiverServiceUpdateTypeID = Convert.ToInt32(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId));
                }

                if (ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg) == CON.WorkflowEventType.Reconsideration || ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg) == CON.WorkflowEventType.CredentialReconsideration)
				{
					isReconsideration = true;
				}

				if (isReactivation || isReapplication || isReconsideration)
					isWaiverServiceUpdate = true;

				

				List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                bool isODAIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfODAInitialApplication", parameters));

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                DataSet dsTerm = DataAccess.ExecuteStoredProcedure("usp_CheckIfSendTermEmailtoDODDorODA", parameters, "TermData");
                DataRow drTerm = ObjectControllerHelper.HasRows(dsTerm) ? dsTerm.Tables[0].Rows[0] : null;
                bool isSendTermEmailODA = ObjectControllerHelper.GetBool("IS_SEND_EMAIL_ODA", drTerm);

                if (HasODASpecialty && (WaiverTypeID == CON.WaiverApplicationTypeID.ODA || workflowEventTypeID == CON.WorkflowEventType.RevalReg ||
                    (((WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODA || WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODM 
					|| WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.OperatorUpdate || isWaiverServiceUpdate) ||
                     (WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD))
                     && workflowEventTypeID == CON.WorkflowEventType.UpdateReg) 
                     || (workflowEventTypeID == CON.WorkflowEventType.SiteVisitEvent && workflowID == CON.WorkflowType.SiteVistEvent)))  // SAM538 New Site Visit Event
                {
                    if (isTermed)
                    {
                        if (string.IsNullOrEmpty(medicaidID)) // If DODD Non Medicaid adding ODA Medicaid fails screening or review then it is Denied, If Denied no Term email is sent to ODA only a Transaction is sent 
                        {
                            nextStep = isODAIntialApp ? "Yes" : "No";
                        }
                        else
                            nextStep = isSendTermEmailODA ? "Terminated" : "No";  // If Terminated in this workflow Send Term Email to ODA, Termination Payloads are not send to ODA
                        toReturn = true;
                    }
                    else
                    {
                        if (isODAIntialApp || HasActiveODASpecialty)
                        {
                            nextStep = "Yes";
                            toReturn = true;
                        }
                        else
                        {
                            nextStep = "No";
                            toReturn = true;
                        }

                    }

                }
                else
                {
                    nextStep = "No";
                    toReturn = true;
                }

                if (nextStep == "No")
                {
                    if ((WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD) && (workflowEventTypeID == CON.WorkflowEventType.UpdateReg))
                    {
                        nextStep = (registrationStatusTypeID == CON.RegistrationStatusTypeId.NotProcessed) ? "Not Processed" : "Check Waiver Queue";
                        toReturn = true;

                        int additionalApplicationID;
                        if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID), out additionalApplicationID))
                        {
                            throw new Exception(string.Format(
                                Constants.LogString.WorkflowParameterNotNumeric,
                                Assembly.GetExecutingAssembly().GetName().Name,
                                Constants.ProcessParameter.AdditionalApplicationID,
                                GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID)));
                        }
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                        param.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, true));
                        param.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, true));
                        param.Add(SqlParms.CreateParameter("USER", DbType.Guid, CON.appAdminUserId, true));
                        param.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, true));
                        DataAccess.ExecuteStoredProcedure("usp_UpdateWaiverQueuedTransactionStatus", param);
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
