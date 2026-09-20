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
    /// Summary description for WFCheckForGroupMemberRetroReview
    /// </summary>
    public class WFCheckForGroupMemberRetroReview : BaseWorkflowTask, IWorkflowTask
    {
        private string nextStep;

        public WFCheckForGroupMemberRetroReview(int processID, int stepID)
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
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREG_AFFILIATION", sqlParms, "RegData");
                if (ObjectControllerHelper.HasRows(dsReg))
                {
                    dsReg.Tables[0].TableName = "RegData";
                    DataTable dtAffiliations = null;
                    string filter = "";

                    if (ObjectControllerHelper.HasRows(dsReg))
                    {
                        DataRow[] rows = dsReg.Tables[0].Select(filter, "NAME ASC");
                        if (rows.Length > 0)
                        {
                            dtAffiliations = rows.CopyToDataTable();
                        }
                    }
                    DataRow[] drReg = ObjectControllerHelper.HasRows(dsReg) ? dtAffiliations.Select("RETRO_REVIEW_REQUIRED_ID = '" + CON.GroupMemberRetroStatusID.RetroReviewRequired + "'") : null;

                    if (drReg == null)
                    {
                        throw new Exception(string.Format(Constants.LogString.WorkflowError, Assembly.GetExecutingAssembly().GetName().Name,
                                    "Unable to retrieve registration using usp_SelectREG_AFFILIATION."));
                    }

                    //DataRow[] rows = drReg;
                    //int PartyID = ObjectControllerHelper.GetInt("PARTY_ID", drReg);
                    if (drReg != null && drReg.Length > 0)
                    {
                        nextStep = "Needs Group Member Review";
                        //Set page status for identification page as pending.
                        /*RegistrationController.SaveRegistrationPageStatus(registrationID, CON.RegistrationPageType.Identification, null,
                            Constants.RegistrationProviderServicesStatusTypeId.Pending, DateTime.Now, Constants.appAdminUserId);*/
                    }
                    else
                    {
                        nextStep = "Next";
                    }
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
