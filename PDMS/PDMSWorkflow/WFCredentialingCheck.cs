using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;
using MAXIMUS.Controllers.PDMS;

namespace PDMSWorkflow
{
    public class WFCredentialingCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCredentialingCheck(int processID, int stepID)
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
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegID", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int providerRiskLevel = ObjectControllerHelper.GetInt("PROVIDER_RISK_LEVEL_ID", drReg);
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int applicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                bool requiresCredentialing = ObjectControllerHelper.GetBool("CredentialingRequired", drReg);
                int providerCategoryTypeId = ObjectControllerHelper.GetInt("ProviderCategoryTypeId", drReg);
                int enityTypeId = ObjectControllerHelper.GetInt("entity_type_id", drReg);
                int workflowID = ObjectControllerHelper.GetInt("WorkflowID", drReg);
                bool isHospitalBasedProvider = CredentialController.IsHospitalBasedProvider(regID);
                bool isProviderReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", drReg);
                bool isReapplication = ObjectControllerHelper.GetBool("isreapplication", drReg);
                string mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);

                log.CreateLogEntry("Process ID : " + ProcessID + "Reg ID : " + regID);
                log.CreateLogEntry("Inital step to check if credentialing needed : " + requiresCredentialing);

                if (workflowID == CON.WorkflowType.CHOP)
                {
                    if (mmisProviderTypeID == CON.MMISProviderType.NURSING_FACILITY && workflowEventTypeID == CON.WorkflowEventType.NewReg)
                        nextStep = "Credentialing needed";
                    else
                        nextStep = "Credentialing not needed";
                }
                else
                {
                    //OHPNM-14290, OHPNM-13109 - Reactivations or reapplication
                    if (!requiresCredentialing && (isProviderReactivation || isReapplication || workflowEventTypeID == CON.WorkflowEventType.Reconsideration))
                    {
                        List<SqlParameter> sqlParmsR = new List<SqlParameter>();
                        sqlParmsR.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                        requiresCredentialing = Convert.ToBoolean(DataAccess.ExecuteStoredProcedure("usp_CheckIfNeedCredentialingforReactivationorReapplication", sqlParmsR, "IS_CRED_REQUIRED", SqlDbType.Bit, 100));
                        log.CreateLogEntry("Check if credentialing needed for Reapplication/Reactivation : " + requiresCredentialing);
                    }

                    //HospitalBasedProviders should n't go to credentialing
                    if (isHospitalBasedProvider == false && requiresCredentialing)
                    {
                        requiresCredentialing = true;
                    }
                    else
                    {
                        requiresCredentialing = false;
                    }
                    // OHPNM-2797
                    bool credentialedByDelegate = ObjectControllerHelper.GetBool("DelegateCredentialingRequired", drReg);

                    log.CreateLogEntry("DelegateCredentialingRequired: " + credentialedByDelegate);

                    if (credentialedByDelegate)
                    {
                        // if they are credentialed by a delegate, don't require credentialing since the delegate takes care of it
                        requiresCredentialing = false;
                    }
                    // OHPNM-9076 treat Reactivation and Reapplication as new
                    //OHPNM-12579 - PT86 should go through credentialing in case of reactivation/reapplication/revalidation wf
                    if (isProviderReactivation || isReapplication)
                    {
                        workflowEventTypeID = CON.WorkflowEventType.RevalReg;
                    }

                    if (requiresCredentialing && workflowID == CON.WorkflowType.RegistrationNew && workflowEventTypeID != CON.WorkflowEventType.UpdateReg && (applicationTypeID == CON.ApplicationType.Standard || applicationTypeID == CON.ApplicationType.Waiver))
                    {
                        nextStep = "Credentialing needed";

                    }
                    else if (requiresCredentialing && (workflowEventTypeID == CON.WorkflowEventType.Reconsideration || workflowEventTypeID == CON.WorkflowEventType.CredentialReconsideration) && (applicationTypeID == CON.ApplicationType.Standard || applicationTypeID == CON.ApplicationType.Waiver))
                    {
                        nextStep = "Credentialing needed";

                    }
                    else if (requiresCredentialing && workflowID == CON.WorkflowType.RegistrationNew && workflowEventTypeID == CON.WorkflowEventType.UpdateReg && (applicationTypeID == CON.ApplicationType.Standard || applicationTypeID == CON.ApplicationType.Waiver))
                    {
                        // OHPNM - 4707 Provider Update also required credentialing check
                        List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                        sqlParms1.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                        sqlParms1.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, ProcessID, false));
                        var isCredUpdateReqAgain = DataAccess.ExecuteScalar("usp_CheckIfNeedCredentialingforUpdate", sqlParms1);
                        nextStep = isCredUpdateReqAgain == "True" && requiresCredentialing == true ? "Credentialing needed" : "Credentialing not needed";
                    }
                    else if (applicationTypeID == CON.ApplicationType.ChangeOfOperator && requiresCredentialing && workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                    {
                        nextStep = "Credentialing needed";
                    }
                    else
                    {
                        nextStep = "Credentialing not needed";
                    }
                    log.CreateLogEntry("Next step before checking if credentialing needed again: " + nextStep);
                    // OHPNM - 1332 Providers who are rejected via MITS, and reprocessed in PNM that are credentialed providers which already have an approved credentialing do not need to be credentialed again, unless they were Returned to Provider. 
                    if (nextStep == "Credentialing needed")
                    {
                        List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                        sqlParms1.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                        sqlParms1.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, ProcessID, false));

                        var isCredReqAgain = DataAccess.ExecuteScalar("usp_CheckIfNeedCredentialingAgain", sqlParms1);

                        log.CreateLogEntry("Is Credentialing required : " + isCredReqAgain + " for Registration ID: " + regID);

                        nextStep = isCredReqAgain == "True" ? "Credentialing needed" : "Credentialing not needed";
                        //bool isCredReqAgain = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfNeedCredentialingAgain", sqlParms1));
                        //nextStep = isCredReqAgain ? "Credentialing needed" : "Credentialing not needed";
                        log.CreateLogEntry("Next step: " + nextStep);
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
        override public string NextStep()
        {
            return nextStep;
        }
    }
}
