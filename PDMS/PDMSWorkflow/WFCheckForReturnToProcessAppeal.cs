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
    public class WFCheckForReturnToProcessAppeal : BaseWorkflowTask, IWorkflowTask
    {
        
        string nextStep = null;

        public WFCheckForReturnToProcessAppeal(int processID, int stepID)
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
                //PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = RegistrationController.SelectRegistrationData(regID, "APPEAL");
                DataTable dtProvider = new DataTable();
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false)); 
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int referralID = ObjectControllerHelper.GetInt("DIDD_REFERRAL_ID", drReg);
                int party_ID = ObjectControllerHelper.GetInt("PartyID", drReg);
                if (ObjectControllerHelper.HasRows(ds))
                {
                    dtProvider = ds.Tables[0];
                    int partyID = ObjectControllerHelper.GetInt("PARTY_ID", dtProvider.Rows[0]);
                    if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("APPEAL_STATUS_ID", dtProvider.Rows[0])))
                    {
                        int AppealStatusID = Convert.ToInt32(ObjectControllerHelper.GetString("APPEAL_STATUS_ID", dtProvider.Rows[0]));
                        string FROMMMIS = ObjectControllerHelper.GetString("SEND_TO_MMIS", dtProvider.Rows[0]);

                        if (AppealStatusID == CON.AppealStatus.AppealWon_ReinstateProvider)
                        {
                            //If appeal won, continue.
                            if (referralID > 0)
                            {
                                nextStep = "NFOCUS Next";
                            }
                            else
                            {
                                nextStep = "Next";
                            }
                            toReturn = true;
                        }
                        else if (
                                (ObjectControllerHelper.GetString("IMMEDIATE_DENY_OR_TERMINATE", dtProvider.Rows[0]) == CON.ImmediateDenyOrTerminate.Immediate.ToString())
                                && FROMMMIS != "M"
                            )
                        {
                           //Determine if this is immediate term and need to go back to process appeal or final term.  ?base on if final status set or not?   
                            nextStep = "Return To Process Appeal";
                            if (p.WorkflowID != Constants.WorkflowType.PeriodicDatabaseChecks)
                            {
                                //need to change this to if last step was process appeal (site visit)
                                DataSet ds1 = ScreeningController.SelectSiteVisitScreeningData(regID);
                                if (ObjectControllerHelper.HasRows(ds1))
                                {
                                    nextStep = "Return To Process Appeal(Site Visit)";
                                }
                            }
                            toReturn = true;
                            //update reg_appeal with value 'M' to denote it is from MMIS
                            Dictionary<string, string> parms = new Dictionary<string, string>();
                            parms.Add("REG_ID", regID.ToString());
                            parms.Add("REG_APPEAL_ID", ObjectControllerHelper.GetString("REG_APPEAL_ID", dtProvider.Rows[0]));
                            parms.Add("SEND_TO_MMIS", "M");
                            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            RegistrationController.UpdateRegistrationData("APPEAL", parms);
                        }
                        else
                        {
                            if (referralID > 0)
                            {
                                nextStep = "NFOCUS Next";
                            }
                            else
                            {
                                nextStep = "Next";
                            }
                            toReturn = true;
                        }
                    }
                    else
                    {
                        if (referralID > 0)
                        {
                            nextStep = "NFOCUS Next";
                        }
                        else
                        {
                            nextStep = "Next";
                        }
                        toReturn = true;
                    }
                }
                else
                {
                    if (referralID > 0)
                    {
                        nextStep = "NFOCUS Next";
                    }
                    else
                    {
                        nextStep = "Next";
                    }
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
