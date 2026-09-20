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
    public class WFSendToApplicationDisposition : BaseWorkflowTask, IElapsedTask
    {
        string nextStep = null;

        public WFSendToApplicationDisposition(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        private Logging log = null;
        override public bool ProcessElapsedTask()
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

                //Step 1: Get Poor Quality Filgerprints.
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("REG_ID", regID));
                DataSet dsBGReg = DataAccess.ExecuteStoredProcedure("usp_SelectREG_BACKGROUND_CHECK", sqlParams, "BackgroundCheckData");
                DataRow drReg = ObjectControllerHelper.HasRows(dsBGReg) ? dsBGReg.Tables[0].Rows[0] : null;
                bool IsPoorQualityPrints = false;
                if (drReg != null)
                {
                    IsPoorQualityPrints = ObjectControllerHelper.GetBool("IsPoorQualityPrints", drReg);
                }

                // Fetch Step Start Date
                DateTime? StepStartDate = GetStepStateDate();

                //Step 2: Add below If conditions
                //if 
                //IsPoorQualityPrints == 1 && stepStartDate is greater than equal to 30 days & less than 60 days               
                //OR
                //start_date of the stepStartDate is greater than equal 60 days
                if (StepStartDate != null)
                {
                    int days = CalculateDays(StepStartDate.Value);
                    if ((IsPoorQualityPrints && days >= 30 && days < 60) || days >= 60)
                    {
                        SetProcessParameter(Constants.ProcessParameter.ReferToComplianceReasonID, CON.ReferToComplianceReasons.BCIComplianceReview.ToString());
                        SaveProcessParameters();
                    }
                }

                nextStep = "Send To Application Disposition";
                toReturn = true;
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return toReturn;
        }

        private DateTime? GetStepStateDate()
        {
            DateTime? stepStateDate = null;
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, ProcessID, true));
            ds = DataAccess.ExecuteStoredProcedure("usp_WF_StepProcessInfo", parameters, "StepProcessInfo");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                stepStateDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["START_DATE_TIME"]);
            }
            return stepStateDate;
        }

        private int CalculateDays(DateTime stepStartDate)
        {
            TimeSpan difference = DateTime.Now - stepStartDate;
            return difference.Days;
        }

        override public string NextStep()
        {
            return nextStep;
        }
    }
}
