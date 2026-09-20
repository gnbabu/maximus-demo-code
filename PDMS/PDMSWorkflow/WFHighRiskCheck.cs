using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    class WFHighRiskCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFHighRiskCheck(int processID, int stepID)
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
                //SAM505  
                string showRTPBCICheckBox = p.GetProcessParameter(Constants.ProcessParameter.ShowRTPBCICheckBox);
                bool isWaiverProvAddODMSpec = string.IsNullOrEmpty(showRTPBCICheckBox) ? false : Convert.ToBoolean(showRTPBCICheckBox);

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                int riskLevel = 0;
                string mmisProviderTypeID = string.Empty;
                string mmisSpecialtyTypeID = string.Empty;
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_CheckRiskLevelByRegID", sqlParms, "RegDataHR");
                dsReg.Tables[0].TableName = "RegDataHR";
                List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                sqlParms1.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsReg1 = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms1, "RegData");
                dsReg1.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg1) ? dsReg1.Tables[0].Rows[0] : null;
                int providerCategoryTypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID", drReg);
                if (dsReg.Tables[0].Rows.Count > 0)
                {
                    riskLevel = Convert.ToInt32(dsReg.Tables[0].Rows[0]["PROVIDER_RISK_LEVEL_ID"]);
                    mmisProviderTypeID = dsReg.Tables[0].Rows[0]["MMIS_PROVIDER_TYPE_ID"].ToString();
                }
                if (dsReg1.Tables[0].Rows.Count > 0 && mmisProviderTypeID == "38")
                {
                    mmisSpecialtyTypeID = dsReg1.Tables[0].Rows[0]["MMISSpecialtyTypeID"].ToString(); // just looking at primary; might have to look at all specialty types (usp_SelectREG_SPECIALTYByRegID)?

                    if (mmisSpecialtyTypeID == "384" || mmisSpecialtyTypeID == "385")
                    {
                        List<SqlParameter> sqlParmssp = new List<SqlParameter>();
                        sqlParmssp.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                        DataSet dsRegsp = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SPECIALTY", sqlParmssp, "RegData");

                        var specialtyIds = new List<string> { "380","381","382","383" };

                        bool kidsSpecialtyPresent = dsRegsp.Tables[0].AsEnumerable()
                                          .Where(r => specialtyIds.Contains(r.Field<string>("MMIS_SPECIALTY_TYPE_ID")) &&
                                                      (r["SENT_TO_SI"] != DBNull.Value ? Convert.ToBoolean(r["SENT_TO_SI"]) : true) == false)
                                          .Any();
                        // high risk level
                        if(kidsSpecialtyPresent)
                        riskLevel = 3;
                        
                    }
                }
                if (mmisProviderTypeID == "45" || mmisProviderTypeID == "55" || mmisProviderTypeID == "25")
                {

                        List<SqlParameter> sqlParmssp2 = new List<SqlParameter>();
                        sqlParmssp2.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                        DataSet dsRegsp = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SPECIALTY", sqlParmssp2, "RegData");

                        var specialtyIds = new List<string> { "ORR", "TSS", "BHR"};

                        bool kidsSpecialtyPresent = dsRegsp.Tables[0].AsEnumerable()
                                          .Where(r => specialtyIds.Contains(r.Field<string>("MMIS_SPECIALTY_TYPE_ID")) &&
                                                      (r["SENT_TO_SI"] != DBNull.Value ? Convert.ToBoolean(r["SENT_TO_SI"]) : true) == false)
                                          .Any();
                        // high risk level
                        if (kidsSpecialtyPresent)
                            riskLevel = 3;


                }
                // OHPNM-5459 - Only the Waivered Services Organization (MMIS PROVIDER TYPE 45) with a specialty of 451, 452 and 453 - goes to Fingerprint and Background Entry but not other specialty combinations
                if (dsReg1.Tables[0].Rows.Count > 0 && mmisProviderTypeID == "45")
                {
                    mmisSpecialtyTypeID = dsReg1.Tables[0].Rows[0]["MMISSpecialtyTypeID"].ToString(); // just looking at primary; might have to look at all specialty types (usp_SelectREG_SPECIALTYByRegID)?

                    if (mmisSpecialtyTypeID != "451" && mmisSpecialtyTypeID != "452" && mmisSpecialtyTypeID != "453" && mmisSpecialtyTypeID != "45P")
                    {
                        // lower risk level
                        riskLevel = 1;
                    } 
                }

                if (mmisProviderTypeID == "60" || mmisProviderTypeID == "16" || mmisProviderTypeID == "25" || mmisProviderTypeID == "26" || mmisProviderTypeID == "38" || mmisProviderTypeID == "55" || mmisProviderTypeID == "45" || mmisProviderTypeID == "10" || mmisProviderTypeID == "76" || mmisProviderTypeID == "83")
                {
                    // for these provider types, force them to high risk (but let the code below decide if it should go to FCBC if they have REG_OWNERS that are individuals)
                    riskLevel = 3;
                }

                if (riskLevel > 2 || isWaiverProvAddODMSpec)
                {
                    //Need to be High Or Moderate Risk Level
                    //Only individual owners (not managing employee or organizations) need to go to FCBC check OHPNM-1398
                    bool individualOwnerCount = false;
                    List<SqlParameter> sqlParOwner = new List<SqlParameter>();
                    sqlParOwner.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                    DataSet dsOwner = DataAccess.ExecuteStoredProcedure("usp_SelectREG_OWNER", sqlParOwner, "RegOwner");
                    if(dsOwner.Tables[0].Rows.Count > 0)
                    {
                        //OHPNM-15129-Fix for only active individual owners go to FCBC check
                        //individualOwnerCount=dsOwner.Tables[0].Select("REG_OWNER_TYPE_ID='" + CON.OwnerType.Person + "'").Length;
                        individualOwnerCount = dsOwner.Tables[0].AsEnumerable()
                                          .Where(r => r.Field<int>("REG_OWNER_TYPE_ID") == int.Parse(CON.OwnerType.Person) &&
                                                      r.Field<DateTime>("END_DATE") > DateTime.Now).Any();

                    }
                    nextStep = (individualOwnerCount || providerCategoryTypeID == CON.ProviderCategoryTypeID.Individual || isWaiverProvAddODMSpec) ? "High Risk" : "Not High Risk";
                    toReturn = true;
                }
                else
                {
                    nextStep = "Not High Risk";
                    toReturn = true;
                }
                if(nextStep == "High Risk")
                {
                    bool rtpBCISent = true;
                    DataSet ds1 = WorkflowController.SelectProcessParameters(this.ProcessID);
                    if (ObjectControllerHelper.HasRows(ds1))
                    {
                        DataRow dr = ds1.Tables[0].Rows[0];
                        rtpBCISent = dr[CON.ProcessParameter.IsAddBCIRTPEmail].ToString() == CON.BCITextRTPEmail.Sent ? true : false;
                    }
                    if(rtpBCISent)
                    {
                        nextStep = "Do not Send BCI Letter";
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
