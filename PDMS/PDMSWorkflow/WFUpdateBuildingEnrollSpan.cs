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
    public class WFUpdateBuildingEnrollSpan : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFUpdateBuildingEnrollSpan(int processID, int stepID)
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
                DataSet dsReg = null;
                DataRow drReg = null;
                dsReg = RegistrationController.SelectRegistration(regID);
                drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                string providerName = ObjectControllerHelper.GetString("PROVIDER_NAME", drReg);
                string NPI = ObjectControllerHelper.GetString("NPI", drReg);
                int providerTypeID = ObjectControllerHelper.GetInt("PROVIDER_TYPE_ID", drReg);

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                DataSet dsCHOPParent = DataAccess.ExecuteStoredProcedure("usp_SelectREG_CHOP_PARENT", sqlParms, "regCHOPParent");
                int reg_ID_SELL = 0;
                if (ObjectControllerHelper.HasRows(dsCHOPParent))
                {
                    DataRow drCHOPParent = dsCHOPParent.Tables[0].Rows[0];
                    reg_ID_SELL = ObjectControllerHelper.GetInt("REG_ID_SELL", drCHOPParent);
                }


                dsReg = RegistrationController.GetCHOPFacilityAffiliationRegID(reg_ID_SELL);
                if (ObjectControllerHelper.HasRows(dsReg))
                {                                   
                    drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                    int regAffiliaitonID = ObjectControllerHelper.GetInt("REG_AFFILIATION_ID", drReg);
                    int facilityLT_regID = ObjectControllerHelper.GetInt("REG_ID", drReg);

                    dsReg = RegistrationController.GenerateMedicaidID();
                    drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                    string nextMED_ID = ObjectControllerHelper.GetString("MEDICAID_ID", drReg);
                    DateTime createDateTime = DateTime.Now;
                    Guid createdby = new Guid(CON.appAdminUserId);

                    List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                    sqlParms1.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_ID_SELL, false));
                    DataSet dsRegRiskAlert = DataAccess.ExecuteStoredProcedure("usp_SelectREG_LTC_RISK_ALERT", sqlParms1, "regCHOPDate");
                    DateTime dtEffectiveForEntering = DateTime.Now;
                    if (ObjectControllerHelper.HasRows(dsRegRiskAlert))
                    {
                        DataRow drRiskAlert = dsRegRiskAlert.Tables[0].Rows[0];
                        DateTime finalDateCHOP = ObjectControllerHelper.GetDateTime("CHOP_Final_Date", drRiskAlert);
                        if (finalDateCHOP != null && !ObjectControllerHelper.IsDateNull(finalDateCHOP))
                        {
                           dtEffectiveForEntering = finalDateCHOP;
                        }
                    }

                    if (!string.IsNullOrEmpty(nextMED_ID))
                    {
                        RegistrationController.UpdateMedicaidIDByRegID(regID, nextMED_ID, DateTime.Now, CON.appPDMSAdminUserId);
                        RegistrationController.UpdateBuildingEnrollmentSpan(regAffiliaitonID, facilityLT_regID, providerName, NPI, nextMED_ID, providerTypeID, dtEffectiveForEntering,
                            Convert.ToDateTime("12/31/2299"), DateTime.Now, createdby, CON.GroupAffiliationStatusTypeID.GroupConfirmed, CON.RegistrationModifiedStatusType.NoChange, 
                            DateTime.Now, createdby);
                        nextStep = "Next";
                        toReturn = true;
                    }
                    else
                    {
                        log.CreateLogEntry("No more Medicaid ID left", Logging.LogPriority.Error);
                        if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                        throw new ArgumentOutOfRangeException("Medicaid ID of 9999999 is already assigned and no more 7 digit medicaid ids to assign.");
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
