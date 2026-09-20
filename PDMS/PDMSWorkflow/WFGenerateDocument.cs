using MAXIMUS.Core.Libraries;
using MAXIMUS.ProcessDocuments.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFGenerateDocument: BaseWorkflowTask, IWorkflowTask
    {
        public WFGenerateDocument(int processID, int stepID)
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

            bool result = false;

            int regID = 0;
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

                ProcessDocuments doc = new ProcessDocuments(LogThreadID);

                List<SqlParameter> parameters;
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                DataSet regData = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATION", parameters, "Errors");
                int diddReferralID = 0;
                int specialtyTypeID = 0;
                if (ObjectControllerHelper.HasRows(regData))
                {
                    DataRow dr = regData.Tables[0].Rows[0];
                    diddReferralID = ObjectControllerHelper.GetInt("DIDD_Referral_ID", dr);
                    specialtyTypeID = ObjectControllerHelper.GetInt("SPECIALTY_TYPE_ID", dr);
                }

                //TODO:  better approach?
                    if (p.WorkflowID != Constants.WorkflowType.RegistrationDIDDReferral 
                        && specialtyTypeID != Constants.SpecialtyTypeID.ICF_IID 
                        && specialtyTypeID != Constants.SpecialtyTypeID.ICF_IIDOther)
                        return true;

                    //Determine type of document
                    string fileName = string.Empty;
                    switch (p.WorkflowID)
                    {
                        case Constants.WorkflowType.RegistrationDIDDReferral:
                            result = doc.GenerateDIDDContract(regID, out fileName);
                            break;
                        case Constants.WorkflowType.RegistrationNew:
                        case Constants.WorkflowType.RegistrationUpdateProvider:
                            result = doc.GenerateICF_IDDContract(regID, out fileName);
                            break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, "RegistrationId: " + regID.ToString() + " - " + logMsg);
            }

            return result;
        }
        override public string NextStep()
        {
            return null;
        }
    }
}

