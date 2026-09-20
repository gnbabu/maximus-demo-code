using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Text.RegularExpressions;

namespace PDMSWorkflow
{
    public class WFCheckBCIILetterNeeded : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCheckBCIILetterNeeded(int processID, int stepID)
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

                //Get the Ohio Resident Status
                bool isOhioResident = RegistrationController.CheckIfBCIIorFBINeeded(regID);

                // Get below required Notice Issue Flags
                (bool isBackgroundCheckRequired, bool issueFBILetter) = GetNoticeIssueRequiredFlags(regID);

                // Check Reg ID is Individual or Owner
                int providerCategoryTypeID = GetProviderCategoryTypeId(regID); 

                //Check if the provider was Return to Provider(RTP)
                bool isRTP = IsProviderWasRTP(regID);

                //If Provider was RTP and providerCategoryTypeID == 1 (Individual) then check isOhioResident. If yes then BCI, else FBI, return the step
                if (isRTP && providerCategoryTypeID == 1)
                {
                    if (isOhioResident)
                    {
                        RegistrationController.UpdateInitialBCNoticeDate(regID, DateTime.Now, DateTime.Now, CON.appAdminUserId);
                        nextStep = "Issue BCII Notice"; // Send BCII Email                           
                        toReturn = true;
                    }
                    else
                    {
                        RegistrationController.UpdateIssueFBILetterOnDate(regID, DateTime.Now, DateTime.Now, CON.appAdminUserId);
                        nextStep = "Issue FBI Notice"; // Send FBI Email
                        toReturn = true;
                    }
                    return toReturn;
                }
                // If Provider is forced to got with back ground check notice in the UI
                else if (isBackgroundCheckRequired) 
                {
                    if (providerCategoryTypeID == 1) // Check Individual Provider
                    {
                        if (!issueFBILetter)
                        {
                            RegistrationController.UpdateInitialBCNoticeDate(regID, DateTime.Now, DateTime.Now, CON.appAdminUserId);
                            nextStep = "Issue BCII Notice"; // Send BCII Email
                            toReturn = true;
                        }
                        else
                        {
                            RegistrationController.UpdateIssueFBILetterOnDate(regID, DateTime.Now, DateTime.Now, CON.appAdminUserId);
                            nextStep = "Issue FBI Notice"; // Send FBI Email
                            toReturn = true;
                        }
                    }
                    else
                    {
                        nextStep = "Issue BCII And FBI Notice"; // Send Issue BCII And FBI Notice Email
                        toReturn = true;
                    }
                }
                else
                {
                    if (providerCategoryTypeID == 1) //(if provider is Individual) 
                    {
                        if (isOhioResident)// If provider is Ohio Resident
                        {
                            RegistrationController.UpdateInitialBCNoticeDate(regID, DateTime.Now, DateTime.Now, CON.appAdminUserId);
                            nextStep = "Issue BCII Notice"; // Send BCII Email
                            toReturn = true;
                        }
                        else
                        {
                            RegistrationController.UpdateIssueFBILetterOnDate(regID, DateTime.Now, DateTime.Now, CON.appAdminUserId);
                            nextStep = "Issue FBI Notice"; // Send FBI Email
                            toReturn = true;
                        }
                    }
                    else
                    {
                        nextStep = "Issue BCII And FBI Notice"; // Send Issue BCII And FBI Notice Email
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

        private (bool isBackgroundCheckRequired, bool issueFBILetter) GetNoticeIssueRequiredFlags(int regID)
        {
            bool isBackgroundCheckRequired = false;
            bool issueFBILetter = false;

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("REG_ID", regID));
            DataSet dsBGReg = DataAccess.ExecuteStoredProcedure("usp_SelectREG_BACKGROUND_CHECK", sqlParams, "BackgroundCheckData");

            if (dsBGReg != null && dsBGReg.Tables.Count > 0 && dsBGReg.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsBGReg.Tables[0].Rows)
                {
                    isBackgroundCheckRequired = ObjectControllerHelper.GetBool("IsBackgroundCheckRequired", row);
                    if (isBackgroundCheckRequired)
                    {
                        break;
                    }
                }

                foreach (DataRow row in dsBGReg.Tables[0].Rows)
                {
                    issueFBILetter = ObjectControllerHelper.GetBool("IssueFBILetter", row);
                    if (issueFBILetter)
                    {
                        break;
                    }
                }
            }
            return (isBackgroundCheckRequired, issueFBILetter);
        }

        private static int GetProviderCategoryTypeId(int regID)
        {
            //Check the provider type - Individual or Owner
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
            DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
            DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
            int providerCategoryTypeID = 0;
            if (drReg != null)
            {
                providerCategoryTypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID", drReg);
            }

            return providerCategoryTypeID;
        }

        private bool IsProviderWasRTP(int regID)
        {
            List<SqlParameter> sqlParms3 = new List<SqlParameter>();
            sqlParms3.Add(SqlParms.CreateParameter("RegID", DbType.String, regID.ToString(), false));
            sqlParms3.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, ProcessID, false));
            DataSet dsRTP = DataAccess.ExecuteStoredProcedure("usp_CheckIFRegIDIsRTP", sqlParms3, "RegRTP");
            bool isRTP = ObjectControllerHelper.HasRows(dsRTP) ? true : false;
            return isRTP;
        }

        override public string NextStep()
        {
            return nextStep;
        }
    }
}
