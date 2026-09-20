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
    public class WFCheckForNameOwnerPracticeChange : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckForNameOwnerPracticeChange(int processID, int stepID)
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

                if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                {
                    if (!RegistrationController.HasNameOwnerPracticeChange(regID))
                    {
                        // OHPNM-2117 - Owner hasn't changed; let's see if they have a license change for an out-of-state license or non-everified license
                        // If a license has changed that is out-of-state or non e-verified send them to screening; otherwise, send to specialty
                        if (nonEverifiedOrOutOfStateLicChanged(regID) || isRequireReview)
                        {
                            nextStep = "Return To Screening";
                            toReturn = true;
                        }
                        else
                        {
                            // Since this is an in-state license or the license has been e-verified, send them to speciality like we used to
                            //No Change -> Specialty
                            nextStep = "No Change";
                            toReturn = true;
                        }
                    }
                    else
                    {
                        // Owner has Changed -> Return To Screening
                        nextStep = "Return To Screening";
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

        private bool nonEverifiedOrOutOfStateLicChanged(int regID)
        {
            List<SqlParameter> parms = new List<SqlParameter> { new SqlParameter("REG_ID", regID) };
            var dsLic = DataAccess.ExecuteStoredProcedure("usp_SelectREG_LICENSES", parms, "ChangedLicenses");
            var hasRows = dsLic.Tables.Count > 0 && dsLic.Tables[0].Rows.Count > 0;

            // this registration has at least one license
            if (Methods.HasRows(dsLic))
            {
                // go through each license
                foreach (DataRow license in dsLic.Tables[0].Rows)
                {
                    // see if this license has changed or was inserted
                    if (Methods.GetIntValue(license, "MODIFIED_STATUS_TYPE_ID") == CON.RegistrationModifiedStatusType.Changed || Methods.GetIntValue(license, "MODIFIED_STATUS_TYPE_ID") == CON.RegistrationModifiedStatusType.Inserted)
                    {
                        // yes, the license has changed or is new; let's see if it's not e-verified or if it's out of state
                        if (Methods.GetStringValue(license, "ELICENSE_VERIFIED") == "False" || AppSettings.Get("StateCode", string.Empty) != Methods.GetStringValue(license, "LICENSE_STATE"))
                        {
                            // this license has changed (or was inserted) and it's either out of state or not e-verfied; route them to conduct provider and owner screening
                            return true;
                        }
                    }
                }
            }
      
            return false;
        }

        override public string NextStep()
        {
            return nextStep;
        }

    }
}
