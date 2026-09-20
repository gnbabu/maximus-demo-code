using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Workflow
{
    public class Process : IDisposable
    {
        public int WorkflowID { get; set; }
        public string WorkflowName { get; set; }
        public int ProcessID { get; set; }
        public int CurrentStepID { get; set; }
        public int TaskID { get; set; }
        public string ProcessOwner { get; set; }
        public DateTime ProcessStartDate { get; set; }
        public DateTime? ProcessEndDate { get; set; }

        public int LogCount { get; set; }
        public Guid LogThreadID { get; set; }

        public int EntryTaskID { get; set; }

        private Dictionary<string, string> ProcessParameter = new Dictionary<string,string>();

        public Process(int processID)
        {
            ProcessID = processID;
            LogCount = 0;
            GetProcessAttributes();
        }

        public Process(int workflowID, string processOwner, Guid threadID, int TaskID = 0)
        {
            WorkflowID = workflowID;
            ProcessOwner = processOwner;
            LogThreadID = threadID;
            LogCount = 0;
            EntryTaskID = TaskID;
            CreateProcess();
        }

        public void Dispose()
        {
            SaveProcessParameters();
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

        private void CreateProcess()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("WorkflowID", DbType.Int32, WorkflowID, true));
            parameters.Add(SqlParms.CreateParameter("OwnerID", DbType.String, ProcessOwner, true));
            parameters.Add(SqlParms.CreateParameter("LogThreadNumber", DbType.String, LogThreadID.ToString(), true));
            parameters.Add(SqlParms.CreateParameter("ENTRY_TASK_ID", DbType.Int32, EntryTaskID.ToString(), true));
            int processID = Convert.ToInt32(DataAccess.ExecuteScalar("usp_WF_CreateProcess", parameters));

            ProcessID = processID;
            GetProcessAttributes();
        }

        private void GetProcessAttributes()
        {
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, ProcessID, true));
            ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectProcessInfo", parameters, "ProcessInfo");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                WorkflowID = Convert.ToInt32(ds.Tables[0].Rows[0]["WORKFLOW_ID"]);
                WorkflowName = ds.Tables[0].Rows[0]["WORKFLOW_NAME"].ToString();
                CurrentStepID = Convert.ToInt32(ds.Tables[0].Rows[0]["CURRENT_STEP_ID"]);
                TaskID = Convert.ToInt32(ds.Tables[0].Rows[0]["TASK_ID"]);
                ProcessOwner = ds.Tables[0].Rows[0]["PROCESS_OWNER_ID"].ToString();
                ProcessStartDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PROCESS_START_DATE_TIME"]);
                ProcessEndDate = ds.Tables[0].Rows[0]["PROCESS_END_DATE_TIME"] != DBNull.Value
                    ? (DateTime?)Convert.ToDateTime(ds.Tables[0].Rows[0]["PROCESS_END_DATE_TIME"])
                    : null;
                LogThreadID = new Guid(ds.Tables[0].Rows[0]["LOG_THREAD_NUMBER"].ToString());
            }
            GetProcessParameters();
        }

        private void GetProcessParameters()
        {
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, ProcessID, true));
            ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectProcessParameters", parameters, "Actions");

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

        private void SaveProcessParameters()
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
    }
}
