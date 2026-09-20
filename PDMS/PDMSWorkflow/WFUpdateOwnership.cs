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
    public class WFUpdateOwnership : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFUpdateOwnership(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        private bool PromoteToActive(int regId, bool ownershipChanged)
        {
            bool rtn = false;
            try
            {
                DataSet ds;
                if (ownershipChanged)
                {
                    ds = UserController.SelectRegistrationStatuses(Constants.appAdminUserId, regId);
                    if (ds.Tables.Count == 0)
                    {
                        throw new Exception(string.Format(
                            Constants.LogString.WorkflowError,
                            Assembly.GetExecutingAssembly().GetName().Name, "Error when migrating Registration " + regId.ToString() + 
                            ". Expected TennCareStatusID from usp_SelectRegistrationStatuses when migrating registration data to PDMS Core"));
                    }
                    SetProcessParameter(Constants.ProcessParameter.RegProgramStatusTypeID, ds.Tables[0].Rows[0]["TennCareStatusID"].ToString());
                    SetProcessParameter(Constants.ProcessParameter.RegistrationStatusTypeID, ds.Tables[0].Rows[0]["REGISTRATION_STATUS_TYPE_ID"].ToString());
                    SaveProcessParameters();
                }

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("OwnershipChanged", DbType.Boolean, ownershipChanged, true));

                ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_TransferRegistrationToProviderTables", parameters, "NewProvider");

                if (ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowError,
                        Assembly.GetExecutingAssembly().GetName().Name, "Error when migrating Registration " + regId.ToString() + 
                        ". Expected PartyID and ServiceLocationID when migrating registration data to PDMS Core"));
                }
                
                DataRow dr = ds.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(ObjectControllerHelper.GetString("ErrorMessage", dr)))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowError,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        "Error when migrating Registration " + regId.ToString() + " data to PDMS Core - " + 
                        ObjectControllerHelper.GetString("ErrorMessage", dr)));
                }

                if (ownershipChanged)
                {
                    SetProcessParameter(Constants.ProcessParameter.PartyID, dr["PartyID"].ToString());
                    SetProcessParameter(Constants.ProcessParameter.ServiceLocationID, dr["ServiceLocationID"].ToString());
                    SaveProcessParameters();
                }
                rtn = true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.LogString.WorkflowError,
                    Assembly.GetExecutingAssembly().GetName().Name, "Error Promote To Active Registration " + regId.ToString() + 
                    " data to PDMS Core - " + ex.Message));
            }
            return rtn;
        }

        override public bool ProcessTask()
        {
            bool returnValue = false;

            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            log.CreateLogEntry(String.Format("Updating Ownership START, Process Id: {0}", ProcessID.ToString()), Logging.LogPriority.Information);
            int regId;
            Process p = new Process(ProcessID);
            if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out regId))
            {
                throw new Exception(string.Format(
                    Constants.LogString.WorkflowParameterNotNumeric,
                    Assembly.GetExecutingAssembly().GetName().Name,
                    Constants.ProcessParameter.RegistrationID,
                    GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
            }

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_FindLastApprovedRegistration", parameters, "RegistrationData");
                if (!ObjectControllerHelper.HasRows(ds))
                {
                    throw new Exception(string.Format(Constants.LogString.WorkflowError,
                        "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
                        "Registration Id: " + regId.ToString() + " does not have an Approved prior Registration"));
                }
                int oldRegId = Convert.ToInt32(ds.Tables[0].Rows[0]["REG_ID"].ToString());
                int oldPartyId = Convert.ToInt32(ds.Tables[0].Rows[0]["PARTY_ID"].ToString());

                // Get the New Registration Effective date which will be used to calculate the Termination date
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATION", parameters, "RegistrationData");
                if (!ObjectControllerHelper.HasRows(ds))
                {
                    throw new Exception(string.Format(Constants.LogString.WorkflowError,
                        "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
                        "Registration Id: " + regId.ToString() + " does any data from usp_SelectRegistration"));
                }
                DateTime newEffectiveDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CHANGE_EFFECTIVE_DATE"].ToString());

                // Get Old Service Location
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, oldRegId, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parameters, "RegistrationData");
                int oldRegServiceLocationId = Convert.ToInt32(ds.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"].ToString());
                int oldServiceLocationId = Convert.ToInt32(ds.Tables[0].Rows[0]["SERVICE_LOCATION_ID"].ToString());
                string oldMedicaidId = ds.Tables[0].Rows[0]["MEDICAID_ID"].ToString();

                // Update Old Service Location
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, oldRegId, true));
                parameters.Add(SqlParms.CreateParameter("REG_SERVICE_LOCATION_ID", DbType.Int32, oldRegServiceLocationId, true));
                // Set TermDate to one day before new Effective Date
                parameters.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, newEffectiveDate.AddDays(-1), true));
                // Set TermEnrollmentStatus to "P".  "P" = Term By Provider from ENROLLMENT_STATUS_TYPE table
                parameters.Add(SqlParms.CreateParameter("TERM_ENROLLMENT_STATUS", DbType.String, "P", true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appWorkflowUserId, true));
                DataAccess.ExecuteStoredProcedure("updateREG_SERVICE_LOCATIONcustom", parameters);
                
                // Update Old Registration 
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, oldRegId, true));
                // Set REG_PROGRAM_STATUS_TYPE_ ID = 3 Terminated from REG_PROGRAM_STATUS_TYPE table
                parameters.Add(SqlParms.CreateParameter("REG_PROGRAM_STATUS_TYPE_ID", DbType.Int32, 3, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appWorkflowUserId, true));
                DataAccess.ExecuteStoredProcedure("updateREGISTRATION", parameters);

                // Update Old Registration Provider
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, oldRegId, true));
                // Set EnrollmentStatus to "P".  "P" = Term By Provider from ENROLLMENT_STATUS_TYPE table
                parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, "P", true));
                // Set TermDate to one day before new Effective Date
                parameters.Add(SqlParms.CreateParameter("TERM_DATE", DbType.Date, newEffectiveDate.AddDays(-1), true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appWorkflowUserId, true));
                DataAccess.ExecuteStoredProcedure("updateREG_PROVIDERCustom", parameters);

                // Promote Old Registration to Active
                if (!PromoteToActive(oldRegId, false)) return false;

                // Send Transaction Type Update if Medicaid Id exists on old Registration
                if (!string.IsNullOrEmpty(oldMedicaidId))
                {
                    try
                    {
                        parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, oldPartyId, true));
                        parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, oldServiceLocationId, true));
                        // 2 = Send Provider Updates to MMIS from TRANSACTION_TYPE table
                        parameters.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, 2, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appWorkflowUserId, true));
                        DataAccess.ExecuteStoredProcedure("usp_SaveTransaction", parameters);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format(Constants.LogString.WorkflowError,
                            "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
                            "Registration Id: " + regId.ToString() + " problems saving Medicaid Id Transaction data - " + ex.Message));
                    }
                }
                log.CreateLogEntry(String.Format("Updating Ownership END, Process Id: {0}", ProcessID.ToString()), Logging.LogPriority.Information);

                // Promote current Ownership Registration to Active
                returnValue = PromoteToActive(regId, true);

                List<SqlParameter> parametersForNPITaxUpdate = new List<SqlParameter>();
                parametersForNPITaxUpdate.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, true));

                ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_UpdateNPITaxInUserAccountInformation", parametersForNPITaxUpdate, "NewProvider");

            }
            catch (Exception ex)
            {
                string msg = string.Format(Constants.LogString.WorkflowError,
                    "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
                    "Registration Id: " + regId.ToString() + " problems obtaining prior Registration data - " + ex.Message);
                Exception newEx = new Exception(msg,ex.InnerException);
                throw CoreException.ThrowException(LogThreadID, newEx, logMsg);
            }
            return returnValue;
        }

        override public string NextStep()
        {
            return nextStep;
        }
    }
}
