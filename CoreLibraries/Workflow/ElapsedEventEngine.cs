using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Workflow
{
    /// <summary>
    /// Summary description for WorkflowEngine
    /// </summary>
    public class ElapsedEventEngine : BaseJob, IJob
    {
        private int LogCount { get; set; }
        private int CurrentProcessID { get; set; }

        public ElapsedEventEngine(Guid threadID)
            : base(threadID)
        {
            // no action
        }

        override public void ExecuteJob()
        {
            // Default Job - Workflow - Run Processes
            this.ExecuteJob(Guid.Parse("0A2754A5-D8F0-438F-BD58-3121AAF9CA3D"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                // Workflow - Run Processes
                case "0A2754A5-D8F0-438F-BD58-3121AAF9CA3D":
                    this.ProcessAllTasks();
                    break;
            }
        }

        private string ProcessElapsedTask(int processID, string assemblyName, string className, ref Logging log)
        {
            string rtn = string.Empty;

            int stepID = 0;
            try
            {
                CurrentProcessID = processID;
                // Flag the task as started.
                string nextStep = string.Empty;
                //bool isRTP = false;
                //isRTP = isRTPRegistration(processID);
                //if(!isRTP)
                // stepID = WorkflowController.StartStep(processID, Constants.appWorkflowUserId);
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                //if (System.Diagnostics.Debugger.IsAttached)
                //{
                //    assemblyPath = @"C:\Projects\TN_PDMS\PDMS\ProviderDataManagementSystemService\bin";
                //}
                string dllPath = new Uri(assemblyPath).LocalPath;
                Assembly workflowAssembly = Assembly.LoadFrom(Path.Combine(dllPath, assemblyName));
                Type workflowClassType = workflowAssembly.GetType(className);
                var stepClass = (IElapsedTask)Activator.CreateInstance(workflowClassType, processID, stepID);
                if (((IElapsedTask)stepClass).ProcessElapsedTask())
                {
                    // If ProcessTask() was successful, proceed to next step in the workflow.
                    nextStep = ((IElapsedTask)stepClass).NextStep();
					if (!string.IsNullOrEmpty(nextStep))
					{
						WorkflowController.TakeAction(processID, nextStep, string.Empty);
					}
                }
                /*else
                {
                    // If ProcessTask() was not successful, put this step to sleep and try again later.
                    WorkflowController.SleepStep(processID);
                }*/
            }
            catch (Exception ex)
            {
                string msg = String.Format(Constants.LogString.WorkflowTaskFailed, "Process Id: " + processID.ToString(), 
                    "Step Id: " + stepID.ToString(), ex.Message);
                log.CreateLogEntry(msg, Logging.LogPriority.Error, +LogCount);
                rtn = msg;

                // If ProcessTask() was not successful, put this step to sleep and try again later.
                /*WorkflowController.SleepStep(processID);*/

                //SendNotification(msg);
            }

            if (!string.IsNullOrEmpty(rtn)) rtn = "Process: " + processID.ToString() + " - " + rtn;
            return rtn;
        }

/*        public string ProcessTask(int processID, string assemblyName, string className)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry("Processing Workflow Process Id: " + processID.ToString(), +LogCount);
            return ProcessElapsedTask(processID, assemblyName, className, ref log);
        }*/

        public void ProcessAllTasks()
        {
            List<string> rtn = new List<string>();

            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry("Processing All Elapsed Tasks", +LogCount);

            // Get list of tasks awaiting system action.
            DataSet ds = new DataSet();
            // usp_WF_SelectElapsedSystemSteps
            ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectElapsedSystemSteps");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string result = ProcessElapsedTask(Convert.ToInt32(dr["PROCESS_ID"]), dr["Elapsed_Event_Assembly"].ToString(),
                    dr["Elapsed_Event_Class_Name"].ToString(), ref log);
                if (!string.IsNullOrEmpty(result)) rtn.Add(result);
            }
            //return rtn;
        }

        private bool isRTPRegistration(int processId)
        {
            bool retVal = false;
            string processRegid =string.Empty;
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processId, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectProcessParameters", parameters, "ProcessParameters");
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        processRegid = ds.Tables[0].Rows[0]["PARAMETER_VALUE"].ToString();

                    }

                }
                if(processId>0 && !string.IsNullOrEmpty(processRegid))
                {
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.String, processRegid, false));
                    sqlParms.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, processId, false));
                    DataSet dsRTP = DataAccess.ExecuteStoredProcedure("usp_CheckRegistrationIsRTP", sqlParms, "RegRTP");
                    if (dsRTP != null && dsRTP.Tables[0].Rows.Count > 0)
                        retVal = true;

                }

            }
            catch(Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        

    }
}