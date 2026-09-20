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
    /// <summary>
    /// Summary description for WFCheckForRetroReview
    /// </summary>
    public class WFCheckForRetroReview : BaseWorkflowTask, IWorkflowTask
    {
        private string nextStep;

        public WFCheckForRetroReview(int processID, int stepID)
            : base(processID, stepID)
        {
            //
            // TODO: Add constructor logic here
            //
        }
        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            int registrationID = 0;
            try
            {
                Process p = new Process(ProcessID);
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out registrationID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registrationID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATION", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;

                if (drReg == null)
                {
                    throw new Exception(string.Format(Constants.LogString.WorkflowError, Assembly.GetExecutingAssembly().GetName().Name,
                                "Unable to retrieve registration using usp_SelectREGISTRATION."));
                }

                bool isPriorEffectiveDate = false;
                DateTime requested_Effective_Date = ObjectControllerHelper.GetDateTime("REQUESTED_EFFECTIVE_DATE", drReg);
                DateTime reg_Create_Date_Time = ObjectControllerHelper.GetDateTime("REG_CREATE_DATE_TIME", drReg);
                int partyID = ObjectControllerHelper.GetInt("PARTY_ID", drReg);
                string medicaidId = ObjectControllerHelper.GetString("MEDICAID_ID", drReg);
                int entityTypeID = ObjectControllerHelper.GetInt("ENTITY_TYPE_ID", drReg);
                if (requested_Effective_Date != null && partyID == 0 && string.IsNullOrEmpty(medicaidId) && entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile)
                {
                    int days = Convert.ToInt32(AppSettings.Get("RetroEffectiveDateWindow"));
                    isPriorEffectiveDate = (requested_Effective_Date.AddDays(days) < reg_Create_Date_Time) ? true : false;
                }

                if (isPriorEffectiveDate)
                {
                    nextStep = "Needs Review";
                    //Set page status for identification page as pending.
                    RegistrationController.SaveRegistrationPageStatus(registrationID, CON.RegistrationPageType.Identification, null,
                        Constants.RegistrationProviderServicesStatusTypeId.Pending, DateTime.Now, Constants.appAdminUserId, null, null, null, null, null, null, null, null, null, null);
                }
                else
                {
                    nextStep = "Next";
                }

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return true;
        }
        override public string NextStep()
        {
            return nextStep;
        }
    }
}
