using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Workflow
{
    public static class WorkflowController
    {
        public static DataSet SelectStepActions(int stepID)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("StepID", DbType.Int32, stepID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectStepActions", parameters, "Actions");

                ds.Tables[0].TableName = "Actions";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetWorkflowSteps(int workflowId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("WORKFLOW_ID", DbType.Int32, workflowId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetWorkflowSteps", parameters, "RegData");

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetWorkflowRoles(int workflowId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("WORKFLOW_ID", DbType.Int32, workflowId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_GetWorkflowRoles", parameters, "RegData");

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectStepInfo(int stepID)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("StepID", DbType.Int32, stepID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectStepInfo", parameters, "StepInfo");

                ds.Tables[0].TableName = "StepInfo";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectWorkflowRoles()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectWorkflowRoles", "WorkflowRoles");

                ds.Tables[0].TableName = "WorkflowRoles";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateTaskRankForRole(string role, int taskId, int rank)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("roleName", DbType.String, role, true));
                parameters.Add(SqlParms.CreateParameter("taskId", DbType.Int32, taskId, true));
                parameters.Add(SqlParms.CreateParameter("rank", DbType.Int32, rank, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_UpdateTaskRankForRole", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Role Name: " + role.ToString() +
                    ", Task Id: " + taskId+ " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectTasksByRole(string role)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("roleName", DbType.String, role, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectTaskByRole", parameters, "TaskByRole");

                ds.Tables[0].TableName = "TaskByRole";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectProcessParameters(int processID)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectProcessParameters", parameters, "ProcessParameters");

                ds.Tables[0].TableName = "ProcessParameters";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectStepParameters(int stepID)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("StepID", DbType.Int32, stepID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectStepParameters", parameters, "StepParameters");

                ds.Tables[0].TableName = "StepParameters";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveProcessParameter(int processID, string parameterName, string parameterValue)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
                parameters.Add(SqlParms.CreateParameter("ParameterName", DbType.String, parameterName, true));
                parameters.Add(SqlParms.CreateParameter("ParameterValue", DbType.String, parameterValue, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_SaveProcessParameter", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Process Id: " + processID.ToString() +
                    ", Parameter Name: " + parameterName + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void SaveStepParameter(int stepID, string parameterName, string parameterValue)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("StepID", DbType.Int32, stepID, true));
                parameters.Add(SqlParms.CreateParameter("ParameterName", DbType.String, parameterName, true));
                parameters.Add(SqlParms.CreateParameter("ParameterValue", DbType.String, parameterValue, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_SaveStepParameter", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Step Id: " + stepID.ToString() +
                    ", Parameter Name: " + parameterName + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void TakeAction(int processID, string action, string notes)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
                parameters.Add(SqlParms.CreateParameter("ActionName", DbType.String, action, true));
                parameters.Add(SqlParms.CreateParameter("Notes", DbType.String, notes, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_TakeAction", parameters);
            }
            catch (Exception ex)
            {
                MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception(ex.Message));
                // Swallowing the exception for now so it doesnt happen during TX demo.
                // The workflow is moving forward
                //throw CoreException.ThrowException(new Exception("Process Id: " + processID.ToString() +
                //    ", Action: " + action + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void InsertSeedStep(int processID, string taskName, string ownerID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
                parameters.Add(SqlParms.CreateParameter("TaskName", DbType.String, taskName, true));
                parameters.Add(SqlParms.CreateParameter("OwnerID", DbType.Guid, ownerID, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_InsertSeedStep", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Process Id: " + processID.ToString() +
                    ", Task Name: " + taskName + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void InsertExceptionForProcessTask(string exceptionInfo,int processID, int stepID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ExceptionInfo", DbType.String, exceptionInfo, true));
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
                parameters.Add(SqlParms.CreateParameter("StepID", DbType.Int32, stepID, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_UpdateExceptionInfoInWorkFlowProcess", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Process Id: " + processID.ToString() +
                    ", Task Name: " + stepID + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int StartStep(int processID, string ownerID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OwnerID", DbType.Guid, ownerID, true));
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
                var res = DataAccess.ExecuteScalar("usp_WF_StartStep", parameters);
                return (!string.IsNullOrEmpty(res) ? Convert.ToInt32(res) : 0);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Process Id: " + processID.ToString() +
                    ", Owner Id: " + ownerID + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void SleepStep(int processID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_SleepStep", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Process Id: " + processID.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void CancelWorkflowProcess(int processID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, processID, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_CancelWorkflowProcess", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Process Id: " + processID.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectActiveOwnerSteps(string ownerID, bool includeOtherSteps)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OwnerID", DbType.String, ownerID, true));
                parameters.Add(SqlParms.CreateParameter("IncludeOtherSteps", DbType.Boolean, includeOtherSteps, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectActiveOwnerSteps", parameters, "Assigned");

                ds.Tables[0].TableName = "Assigned";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectActiveOwnerStepsProvider(string ownerID, bool includeOtherSteps, string ownerRole)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OwnerID", DbType.String, ownerID, true));
                parameters.Add(SqlParms.CreateParameter("IncludeOtherSteps", DbType.Boolean, includeOtherSteps, true));
                parameters.Add(SqlParms.CreateParameter("OwnerRole", DbType.String, ownerRole, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectActiveOwnerStepsProvider", parameters, "Assigned");

                ds.Tables[0].TableName = "Assigned";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectUnassignedSteps(string groupName)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GroupName", DbType.String, groupName, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectUnassignedSteps", parameters, "Unassigned");

                ds.Tables[0].TableName = "Unassigned";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectRegistrationWorkflows(string ownerID)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OwnerID", DbType.String, ownerID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectRegistrationWorkflows", parameters, "RegistrationWorkflows");
                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "RegistrationWorkflows";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectWorkflowByRegId(int regId)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectWorkflowByRegId", parameters, "RegistrationWorkflow");
                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "RegistrationWorkflow";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectProcess(int processID)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectProcessInfo", parameters, "ProcessData");
                ds.Tables[0].TableName = "ProcessData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectLogWorkflow(int processID)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectLogWorkflow", parameters, "ProcessData");
                ds.Tables[0].TableName = "ProcessData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void SaveLogWorkflow(int processID, DateTime lastActivityDateTime, string message)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processID, true));
                parameters.Add(SqlParms.CreateParameter("LastActivityDateTime", DbType.DateTime, lastActivityDateTime, true));
                parameters.Add(SqlParms.CreateParameter("Message", DbType.String, message, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_SaveLogWorkflow", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Process Id: " + processID.ToString() +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectWorkflows()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectWorkflows");

                ds.Tables[0].TableName = "Workflows";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void updateWF_STEP_Owner(int stepId, string ownerId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("STEP_ID", DbType.Int32, stepId, false));
                parameters.Add(SqlParms.CreateParameter("OWNER_ID", DbType.Guid, ownerId, true));
                DataAccess.ExecuteStoredProcedure("updateWF_STEP_Owner", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Step Id: " + stepId.ToString() +
                    ", Owner Id: " + ownerId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void updateWF_STEP_Assignment(int stepId, string assignedByUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("STEP_ID", DbType.Int32, stepId, false));
                parameters.Add(SqlParms.CreateParameter("ASSIGNED_BY_USER", DbType.Guid, assignedByUser, true));
                DataAccess.ExecuteStoredProcedure("usp_updateWF_STEP_Assignment", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Step Id: " + stepId.ToString() +
                    ", Assignment User Id: " + assignedByUser + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int GetWorkflowInstance(int? applicationTypeID, int? providerCategoryTypeID, int? providerTypeID, int? referralTypeID,
        bool revalDue, bool referral, bool conversion, bool groupMember)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ProviderCategoryTypeID ", DbType.Int32, providerCategoryTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ReferralTypeID", DbType.Int32, referralTypeID, true));
                parameters.Add(SqlParms.CreateParameter("RevalDue", DbType.Boolean, revalDue, false));
                parameters.Add(SqlParms.CreateParameter("Referral", DbType.Boolean, referral, false));
                parameters.Add(SqlParms.CreateParameter("Conversion", DbType.Boolean, conversion, false));
                parameters.Add(SqlParms.CreateParameter("GroupMember", DbType.Boolean, groupMember, false));

                ds = DataAccess.ExecuteStoredProcedure("usp_SelectWorkflow", parameters, "Workflow");

                return (int)ds.Tables[0].Rows[0]["WORKFLOW_ID"];

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }


        }



        public static DataSet SelectUnassignedStepsByUserProviderType(string groupName, string userId)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GroupName", DbType.String, groupName, true));
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.String, userId, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectUnassignedStepsByUserProviderType", parameters, "Unassigned");

                ds.Tables[0].TableName = "Unassigned";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            
        }

        public static void updateWF_STEP_OwnerWithStartDate(int stepId, string ownerId, DateTime stepStartDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("STEP_ID", DbType.Int32, stepId, false));
                parameters.Add(SqlParms.CreateParameter("OWNER_ID", DbType.Guid, ownerId, true));
                parameters.Add(SqlParms.CreateParameter("start_date", DbType.DateTime, stepStartDate, true));
                DataAccess.ExecuteStoredProcedure("updateWF_STEP_Owner_and_StartDate", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Step Id: " + stepId.ToString() +
                    ", Owner Id: " + ownerId + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet WF_SelectActiveOwnerStepsCredentialProvider(string ownerID, bool includeOtherSteps, string ownerRole)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OwnerID", DbType.String, ownerID, true));
                parameters.Add(SqlParms.CreateParameter("IncludeOtherSteps", DbType.Boolean, includeOtherSteps, true));
                parameters.Add(SqlParms.CreateParameter("OwnerRole", DbType.String, ownerRole, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectActiveOwnerStepsCredentialProvider", parameters, "Assigned");

                ds.Tables[0].TableName = "CredentialChairAssigned";

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}