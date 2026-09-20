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
    public class WFReceiveClosureDisposition : BaseWorkflowTask, IWorkflowTask
    {


        string nextStep = null;

        public WFReceiveClosureDisposition(int processID, int stepID)
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
                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                    sqlParmsTR.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_APPLICATIONCustom", sqlParmsTR, "RApplication");
                    dsRegTR.Tables[0].TableName = "RApplication";
                    DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                    string AppStatus = ObjectControllerHelper.GetString("Application_Status", drRegTR);
                    int WaiverTypeID = ObjectControllerHelper.GetInt("WAIVER_TYPE_ID", drRegTR);
                    if ((AppStatus.Equals(CON.ApplicationStatus.Approved_ID) || AppStatus.Equals(CON.ApplicationStatus.Closed_As_Complete_ID)) 
                        && (WaiverTypeID == CON.WaiverApplicationTypeID.DODD || WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD))
                    {
                        DataSet dsClosure = RegistrationController.SelectRegistrationData(regID, "ClosureNotice");
                        if (Methods.HasRows(dsClosure))
                        {
                            DateTime dtEffectiveDate = ObjectControllerHelper.GetDateTime("Closure_Effective_Date", dsClosure.Tables[0].Rows[0]);
                            if (dtEffectiveDate != null)
                                ProviderController.TerminateProvider(regID, dtEffectiveDate, Constants.appAdminUserId, CON.EnrollStatus.INACTIVE.ToString(), CON.EnrollStatusReason.CLOSED.ToString(), true);
                        }

                        nextStep = "Approve";
                        toReturn = true;
                    }
                    else if (AppStatus.Equals(CON.ApplicationStatus.ClosedAs_Denied_ID) && (WaiverTypeID == CON.WaiverApplicationTypeID.DODD || WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD))
                    {
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms.Add("REG_ID", regID.ToString());
                        parms.Add("REGISTRATION_STATUS_TYPE_ID", CON.RegistrationStatusTypeId.Submitted.ToString());
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", Constants.appAdminUserId);
                        parms.Add("SUBMIT_DATE_TIME", DateTime.Now.ToString());
                        RegistrationController.UpdateRegistration(parms);

                        nextStep = "Deny";
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
                    nextStep = "Approve";
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
