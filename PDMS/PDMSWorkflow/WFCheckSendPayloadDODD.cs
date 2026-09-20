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
    public class WFCheckSendPayloadDODD : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckSendPayloadDODD(int processID, int stepID)
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
                bool HasDODDSpecialty = ObjectControllerHelper.GetBool("HasDODDSpecialty", drReg);
                bool HasActiveDODDSpecialty = ObjectControllerHelper.GetBool("HasActiveDODDSpecialty", drReg);
                bool HasDDContractNumber = !string.IsNullOrEmpty(ObjectControllerHelper.GetString("dd_contract_number", drReg)) ? true : false;
                int workflowID = ObjectControllerHelper.GetInt("WORKFLOWID", drReg);
                bool isTermed = !string.IsNullOrEmpty(ObjectControllerHelper.GetString("TerminationDate", drReg)) ? true : false;
                string medicaidID = ObjectControllerHelper.GetString("MedicaidID", drReg);

                string DODDCertStatus = ObjectControllerHelper.GetString("DODDCertStatus", drReg);
                DateTime DoDD_Cert_EndDate = ObjectControllerHelper.GetDateTime("dodd_end_date", drReg); 
                bool HasActiveDDContract = ((drReg["dodd_end_date"] == DBNull.Value && DODDCertStatus == "Active") || 
                                            (drReg["dodd_end_date"] != DBNull.Value && DoDD_Cert_EndDate > DateTime.Now))  ? true : false;

				bool isWaiverServiceUpdate = false;
				bool isReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", drReg);
				bool isReapplication = ObjectControllerHelper.GetBool("isreapplication", drReg);
				bool isReconsideration = false;
                int WaiverServiceUpdateTypeID = 0;
                if (!string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId)))
                {
                    WaiverServiceUpdateTypeID = Convert.ToInt32(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId));
                }
                string enableCR318 = AppSettings.Get("EnableCR318");

                if (ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg) == CON.WorkflowEventType.Reconsideration || ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg) == CON.WorkflowEventType.CredentialReconsideration)
				{
					isReconsideration = true;
				}

				if (isReactivation || isReapplication || isReconsideration)
					isWaiverServiceUpdate = true;

				List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                bool isDoDDIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfDODDInitialApplication", parameters));

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                bool isODAIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfODAInitialApplication", parameters));

                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                DataSet dsTerm = DataAccess.ExecuteStoredProcedure("usp_CheckIfSendTermEmailtoDODDorODA", parameters, "TermData");
                DataRow drTerm = ObjectControllerHelper.HasRows(dsTerm) ? dsTerm.Tables[0].Rows[0] : null;
                bool isSendTermEmailDODD = ObjectControllerHelper.GetBool("IS_SEND_EMAIL_DODD", drTerm);

                //OHPNM-13528 - if DD app requires review send the payload.
                int additionalApplicationID = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID))
                                              ? 0 : Convert.ToInt32(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID));
                bool isRequireReview = false;
                if (additionalApplicationID > 0)
                {
                    List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                    sqlParmsTR.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, false));
                    sqlParmsTR.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, false));
                    sqlParmsTR.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, false));
                    DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDITIONAL_APPLICATIONCustom", sqlParmsTR, "RegAddApplication");
                    dsRegTR.Tables[0].TableName = "RegAddApplication";
                    DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                    isRequireReview = ObjectControllerHelper.GetBool("IS_REQUIRE_REVIEW", drRegTR);
                }

                if ((HasDODDSpecialty || HasDDContractNumber || HasActiveDDContract) && 
                    ((((WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD || WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODM ||
                         WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.OperatorUpdate || isWaiverServiceUpdate)
                      || (WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODA && isSendTermEmailDODD))
                       && workflowEventTypeID == CON.WorkflowEventType.UpdateReg) || workflowEventTypeID == CON.WorkflowEventType.RevalReg ||
                       WorkflowID == CON.WorkflowType.PeriodicDatabaseChecks || (workflowEventTypeID == CON.WorkflowEventType.SiteVisitEvent && workflowID == CON.WorkflowType.SiteVistEvent))) // SAM538 New Site Visit Event
                {
                    if (isTermed)
                    {
                        if (string.IsNullOrEmpty(medicaidID)) //Denied , if Non-Medicaid Add Designation or Add ODM/ODA fails screening or review then it is Denied. If Denied no term email is send to DODD
                        {
                            nextStep = "Yes";
                        }
                        else
                            nextStep = isRequireReview ? "Yes" : isSendTermEmailDODD ? "Terminated" : "No"; //only Send Term Email to DODD if terminated in this workflow event.
                        toReturn = true;
                    }
                    else
                    {
                        if(HasActiveDODDSpecialty || HasActiveDDContract)
                            nextStep = isODAIntialApp ? "No" : "Yes";
                        else
                            nextStep = "No";
                        toReturn = true;
                    }

                }
                else if (workflowEventTypeID == CON.WorkflowEventType.NewReg 
                         && ((WaiverTypeID == CON.WaiverApplicationTypeID.DODD || WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD) 
                            || (HasActiveDODDSpecialty && enableCR318.ToLower().Equals("true"))))
                {
                    if (isTermed)
                    {
                        if (string.IsNullOrEmpty(medicaidID)) //Denied , if Non-Medicaid Add Designation or Add ODM/ODA fails screening or review then it is Denied. If Denied no term email is send to DODD
                        {
                            nextStep = "Yes";
                        }
                        else
                            nextStep = isDoDDIntialApp ? "Yes" : isSendTermEmailDODD ? "Terminated" : "Yes"; //only Send Term Email to DODD if terminated in this workflow event.
                        toReturn = true;
                    }
                    else
                    {
                        nextStep = isODAIntialApp ? "No" : "Yes";
                        toReturn = true;
                    }

                }
                else
                {
                    nextStep = "No";
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
