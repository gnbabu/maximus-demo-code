using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Workflow
{
    public class BaseWorkflowTask : IDisposable
    {
        public int WorkflowID { get; set; }
        public string WorkflowName { get; set; }
        public int TaskID { get; set; }
        public string TaskType { get; set; }
        public string TaskName { get; set; }
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public string GroupNameList { get; set; }
        public int StepID { get; set; }
        public int CallingStepID { get; set; }
        public string StepOwner { get; set; }
        public DateTime StepCreateDate { get; set; }
        public DateTime StepStartDate { get; set; }
        public DateTime? StepEndDate { get; set; }
        public string StepNotes { get; set; }
        public int ProcessID { get; set; }
        public string ProcessOwner { get; set; }
        public DateTime ProcessStartDate { get; set; }
        public DateTime? ProcessEndDate { get; set; }

        public int LogCount { get; set; }
        public Guid LogThreadID { get; set; }

        private Dictionary<string, string> TaskParameter;
        private Dictionary<string, string> ProcessParameter;
        private Dictionary<string, string> StepParameter;

        /// <summary>
        /// Launch a workflow task that is already in-flight.
        /// </summary>
        /// <param name="processID">The process identifier</param>
        /// <param name="stepID">The step identifier</param>
        public BaseWorkflowTask(int processID, int stepID)
        {
            ProcessID = processID;
            StepID = stepID;
            LogCount = 0;
            InitializeAttributes();
        }

        public void Dispose()
        {
            SaveProcessParameters();
            SaveStepParameters();
        }

        public virtual bool ProcessTask()
        {
            throw new NotImplementedException("This Workflow Task has an incomplete ProcessTask() method.");
        }

        public virtual bool ProcessElapsedTask()
        {
            throw new NotImplementedException("This Workflow Task has an incomplete ProcessTask() method.");
        }

        public virtual string NextStep()
        {
            return null;
        }

        private void InitializeAttributes()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);
            GetProcessParameters();
            GetStepParameters();

            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("StepID", DbType.Int32, StepID, true));
            ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectStepInfo", parameters, "StepInfo");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                WorkflowID = Convert.ToInt32(ds.Tables[0].Rows[0]["WORKFLOW_ID"]);
                WorkflowName = ds.Tables[0].Rows[0]["WORKFLOW_NAME"].ToString();
                TaskID = Convert.ToInt32(ds.Tables[0].Rows[0]["TASK_ID"]);
                TaskType = ds.Tables[0].Rows[0]["TASK_TYPE"].ToString();
                TaskName = ds.Tables[0].Rows[0]["TASK_NAME"].ToString();
                AssemblyName = ds.Tables[0].Rows[0]["ASSEMBLY_NAME"].ToString();
                ClassName = ds.Tables[0].Rows[0]["CLASS_NAME"].ToString();
                GroupNameList = ds.Tables[0].Rows[0]["GROUP_NAME_LIST"].ToString();
                StepID = Convert.ToInt32(ds.Tables[0].Rows[0]["STEP_ID"]);
                CallingStepID = Convert.ToInt32(ds.Tables[0].Rows[0]["CALLING_STEP_ID"]);
                StepOwner = ds.Tables[0].Rows[0]["STEP_OWNER_ID"].ToString();
                StepCreateDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["STEP_CREATE_DATE_TIME"]);
                
                if (ds.Tables[0].Rows[0]["STEP_START_DATE_TIME"] != DBNull.Value)
                {
                    StepStartDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["STEP_START_DATE_TIME"]);
                }

                StepEndDate = ds.Tables[0].Rows[0]["STEP_END_DATE_TIME"] != DBNull.Value
                    ? (DateTime?)Convert.ToDateTime(ds.Tables[0].Rows[0]["STEP_END_DATE_TIME"])
                    : null;
                StepNotes = ds.Tables[0].Rows[0]["STEP_NOTES"].ToString();
                ProcessID = Convert.ToInt32(ds.Tables[0].Rows[0]["PROCESS_ID"]);
                ProcessOwner = ds.Tables[0].Rows[0]["PROCESS_OWNER_ID"].ToString();
                
                if (ds.Tables[0].Rows[0]["PROCESS_START_DATE_TIME"] != DBNull.Value)
                {
                    ProcessStartDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PROCESS_START_DATE_TIME"]);
                }

                ProcessEndDate = ds.Tables[0].Rows[0]["PROCESS_END_DATE_TIME"] != DBNull.Value
                    ? (DateTime?)Convert.ToDateTime(ds.Tables[0].Rows[0]["PROCESS_END_DATE_TIME"])
                    : null;
                LogThreadID = new Guid(ds.Tables[0].Rows[0]["LOG_THREAD_NUMBER"].ToString());

                // Only get task parameters if Task ID is retrieved in Step Info (should always happen.)
                GetTaskParameters();
            }
        }

        /// TODO: It MAY make sense to eliminate GetTaskParameter() and instead change GetStepParameter()
        /// to look for a parameter of type "key" associated to the current TaskID if it is unable to find
        /// a parameter of type "key" associated to the current StepID.  This would turn task parameters into
        /// default values of step paramaters.  If we do this, it may also make sense to change 
        /// SetStepParameter() so that you can specify to which step/task in the workflow to store the 
        /// key/value pair (instead of assuming it will be stored in the CURRENT StepID), as a way to pass a
        /// parameter from where the process is now, to the step that is  going to consume the parameter.
        /// 
        /// A better approach may be to have a new kind of parameter (called override task parameter) that
        /// stores both the ProcessID and TaskID in the WF_PARAMETER table.  With this, we could keep 
        /// GetStepParameter() as-is and instead adjust GetTaskParameter() to first look for overridden task
        /// parameters (ones that have the TaskID and the current ProcessID specified) before defaulting to
        /// the task parameters (that have null ProcessID).  Then, we would need to add a method called 
        /// SetTaskParameter(string TaskName, string key, string value) for setting override task parameters.
        /// Default task parameters (that have null ProcessID) would continue to have no Set() method, since
        /// these are defined at the time of workflow definition.
        /// 
        /// These changes could come in handy if, for example, the 4th task in a workflow NORMALLY sends out an
        /// email with the subject of "foo" and the template of "EMAIL_TEMPLATE_FOO" but something that the
        /// system encounters in an earlier system task in the workflow causes a need to change the email
        /// subject to "bar" with the template of "EMAIL_TEMPLATE_BAR".

        public string GetTaskParameter(string key)
        {
            string keyValue = string.Empty;
            if (TaskParameter.ContainsKey(key))
            {
                keyValue = TaskParameter[key];
            }
            return keyValue;
        }

        public string GetProcessParameter(string key)
        {
            string keyValue = string.Empty;
            if (ProcessParameter.ContainsKey(key))
            {
                keyValue = ProcessParameter[key];
            }
            return keyValue;
        }

        public void SetProcessParameter(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (ProcessParameter.ContainsKey(key))
                {
                    ProcessParameter.Remove(key);
                }
            }
            else
            {
                if (ProcessParameter.ContainsKey(key))
                {
                    ProcessParameter[key] = value;
                }
                else
                {
                    ProcessParameter.Add(key, value);
                }
            }
        }
 
        public string GetStepParameter(string key)
        {
            string keyValue = string.Empty;
            if (StepParameter.ContainsKey(key))
            {
                keyValue = StepParameter[key];
            }
            return keyValue;
        }

        public void SetStepParameter(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (StepParameter.ContainsKey(key))
                {
                    StepParameter.Remove(key);
                }
            }
            else
            {
                if (StepParameter.ContainsKey(key))
                {
                    StepParameter[key] = value;
                }
                else
                {
                    StepParameter.Add(key, value);
                }
            }
        }

        private void GetTaskParameters()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("TaskID", DbType.Int32, TaskID, true));
            ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectTaskParameters", parameters, "Parameters");
            
            TaskParameter = new Dictionary<string, string>();
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TaskParameter.Add(dr["PARAMETER_NAME"].ToString(), dr["PARAMETER_VALUE"].ToString());
                }
            }
        }

        private void GetProcessParameters()
        {
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, ProcessID, true));
            ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectProcessParameters", parameters, "Parameters");

            ProcessParameter = new Dictionary<string, string>();
			
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				DataRow myRow = ds.Tables[0].Rows[0];
				for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
				{
					ProcessParameter.Add(ds.Tables[0].Columns[j].ColumnName.ToString(), myRow.ItemArray[j].ToString());
				}
			}
        }

        private void GetStepParameters()
        {
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("StepID", DbType.Int32, StepID, true));
            ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectStepParameters", parameters, "Parameters");

            StepParameter = new Dictionary<string, string>();
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    StepParameter.Add(dr["PARAMETER_NAME"].ToString(), dr["PARAMETER_VALUE"].ToString());
                }
            }
        }

        public void SaveProcessParameters()
        {
            foreach (KeyValuePair<string, string> parm in ProcessParameter)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, ProcessID, true));
                parameters.Add(SqlParms.CreateParameter("ParameterName", DbType.String, parm.Key, true));
                parameters.Add(SqlParms.CreateParameter("ParameterValue", DbType.String, parm.Value, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_SaveProcessParameter", parameters);
            }
        }

        private void SaveStepParameters()
        {
            foreach (KeyValuePair<string, string> parm in StepParameter)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("StepID", DbType.Int32, StepID, true));
                parameters.Add(SqlParms.CreateParameter("ParameterName", DbType.String, parm.Key, true));
                parameters.Add(SqlParms.CreateParameter("ParameterValue", DbType.String, parm.Value, true));
                DataAccess.ExecuteStoredProcedure("usp_WF_SaveStepParameter", parameters);
            }
        }
    }
}

