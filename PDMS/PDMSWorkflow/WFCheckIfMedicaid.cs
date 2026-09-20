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
    public class WFCheckIfMedicaid : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckIfMedicaid(int processID, int stepID)
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
            int additionalApplicationID;
            bool forceWfToReview = false;

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
                        Constants.ProcessParameter.AdditionalApplicationID,
                        GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID)));
                }

                // OHPNM-9367 - if we are forcing them to review for a DODD terminated provider with an 'initial' or 'renewal' payload, then set this flag to force the main "IF" statement down below to the nextStep = "Yes" section
                forceWfToReview = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.ForceWfToReview))
                        ? false : Convert.ToBoolean(p.GetProcessParameter(Constants.ProcessParameter.ForceWfToReview));

                int ApplicationTypeID = 0;
                int WaiverTypeID = -1;
                bool HasActiveDODDSpecialty = false;
                bool HasActiveODASpecialty = false;
                bool HasActiveODMSpecialty = false;
                string DDContractNumber = string.Empty;
                int WorkFlowEventTypeID = -1;
                int WaiverServiceUpdateTypeID = -1;                
                bool HasActiveDDContract = false;
                int currRegPgmStatusID = 0;

                DataSet dsReg = RegistrationController.SelectRegistrationByRegID(regID);
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    ApplicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                    WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                    HasActiveDODDSpecialty = ObjectControllerHelper.GetBool("HasActiveDODDSpecialty", drReg);
                    HasActiveODASpecialty = ObjectControllerHelper.GetBool("HasActiveODASpecialty", drReg);
                    HasActiveODMSpecialty = ObjectControllerHelper.GetBool("HasActiveODMSpecialty", drReg);
                    DDContractNumber = ObjectControllerHelper.GetString("dd_contract_number", drReg);
                    WorkFlowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                    WaiverServiceUpdateTypeID = ObjectControllerHelper.GetInt("WaiverServiceUpdateTypeID", drReg);
                    string DODDCertStatus = ObjectControllerHelper.GetString("DODDCertStatus", drReg);
                    DateTime DoDD_Cert_EndDate = ObjectControllerHelper.GetDateTime("dodd_end_date", drReg);
                    HasActiveDDContract = ((drReg["dodd_end_date"] == DBNull.Value && DODDCertStatus == "Active") ||
                                                (drReg["dodd_end_date"] != DBNull.Value && DoDD_Cert_EndDate > DateTime.Now)) ? true : false;
                    currRegPgmStatusID = Methods.GetIntValue(drReg["RegProgramStatusTypeID"]);
                }

                List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                sqlParmsTR.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, false));
                sqlParmsTR.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, false));
                sqlParmsTR.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, false));
                DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDITIONAL_APPLICATIONCustom", sqlParmsTR, "RegAddApplication");
                dsRegTR.Tables[0].TableName = "RegAddApplication";
                DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                bool isRequireReview = ObjectControllerHelper.GetBool("IS_REQUIRE_REVIEW", drRegTR);
                bool enableCR346 = Convert.ToBoolean(AppSettings.Get("EnableCR346", "false"));

                // OHPNM-11192 - If DD Update WF on suspended provider set pnm application status to Accepted so that DD will not reject the payload.
                if (enableCR346 && currRegPgmStatusID == Constants.RegistrationProgramStatusTypeId.Suspended && WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD)
                {
                    RegistrationController.UpdatePNMApplicationStatus(regID, CON.PSMApplicationStatusID.ACCEPTED, DateTime.Now, new Guid(CON.appAdminUserId), p.ProcessID);
                    nextStep = isRequireReview ? "No Require Review" : "No Not Require Review";
                    toReturn = true;
                }
                else
                {
                    if ((ApplicationTypeID == CON.ApplicationType.Waiver && WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD) ||
                    (WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD && !HasActiveDODDSpecialty && !HasActiveODASpecialty && !HasActiveODMSpecialty
                     && HasActiveDDContract && !string.IsNullOrEmpty(DDContractNumber)) && !forceWfToReview)  //Non-medicaid DODD
                    {
                        nextStep = isRequireReview ? "No Require Review" : "No Not Require Review";
                        toReturn = true;

                        if (nextStep == "No Not Require Review")
                        {
                            // Update the waiver queue status to Processed
                            List<SqlParameter> param = new List<SqlParameter>();
                            param.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
                            param.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, true));
                            param.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, true));
                            param.Add(SqlParms.CreateParameter("USER", DbType.Guid, CON.appAdminUserId, true));
                            param.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, true));
                            DataAccess.ExecuteStoredProcedure("usp_UpdateWaiverQueuedTransactionStatus", param);
                        }
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
