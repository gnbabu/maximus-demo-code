using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;


namespace PDMSWorkflow
{
    public class WFContractSignature : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFContractSignature(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int rId;
            Process p = new Process(ProcessID);

            try
            {
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out rId))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }

                List<SqlParameter> parameters;
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, rId, true));
                DataSet regData = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATION", parameters, "Errors");
                int specialtyTypeID = 0;
                if (ObjectControllerHelper.HasRows(regData))
                {
                    DataRow dr = regData.Tables[0].Rows[0];
                    specialtyTypeID = ObjectControllerHelper.GetInt("SPECIALTY_TYPE_ID", dr);

                    if  (specialtyTypeID == Constants.SpecialtyTypeID.ICF_IID || specialtyTypeID == Constants.SpecialtyTypeID.ICF_IIDOther)
                    {
                        RegistrationController.SaveRegistrationPageStatus(rId, Constants.RegistrationPageType.Contracts, null, Constants.RegistrationProviderServicesStatusTypeId.Pending,
                            DateTime.Now, Constants.appAdminUserId, null, null, null, null, null, null, null, null, null, null);
                            nextStep = "Needs Signature";
                    }
                    else
                    {
                        nextStep = "No Signature Needed";
                    }
                    toReturn = true;
                }
                else
                {
                    throw new Exception(string.Format(Constants.LogString.WorkflowError,
                        "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
                        "Registration Id: " + rId.ToString() + " does not have usp_SelectREGISTRATION result set."));
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

