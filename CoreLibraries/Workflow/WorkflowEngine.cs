using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Workflow
{
    /// <summary>
    /// Summary description for WorkflowEngine
    /// </summary>
    public class WorkflowEngine : BaseJob, IJob
    {
        private int LogCount { get; set; }
        private int CurrentProcessID { get; set; }

        public WorkflowEngine(Guid threadID) : base(threadID)
        {
            // no action
        }

        override public void ExecuteJob()
        {
            // Default Job - Workflow - Run Processes
            this.ExecuteJob(Guid.Parse("95AAACAA-C734-4785-9CFB-DA9FE61E6715"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                // Workflow - Run Processes
                case "95AAACAA-C734-4785-9CFB-DA9FE61E6715":
                    this.ProcessAllTasks();
                    break;
            }
        }

        private string ProcessTask(int processID, string assemblyName, string className, ref Logging log)
        {
            string rtn = string.Empty;

            int stepID = 0;
            try
            {
                CurrentProcessID = processID;
                // Flag the task as started.
                string nextStep = string.Empty;
                stepID = WorkflowController.StartStep(processID, Constants.appWorkflowUserId);
                if (stepID > 0)
                {
                    //if (System.Diagnostics.Debugger.IsAttached)
                    //{
                    //    assemblyPath = @"C:\Projects\TN_PDMS\PDMS\ProviderDataManagementSystemService\bin";
                    //}
                    string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                    string dllPath = new Uri(assemblyPath).LocalPath;
                    Assembly workflowAssembly = Assembly.LoadFrom(Path.Combine(dllPath, assemblyName));
                    Type workflowClassType = workflowAssembly.GetType(className);
                    var stepClass = (IWorkflowTask)Activator.CreateInstance(workflowClassType, processID, stepID);
                    if (((IWorkflowTask)stepClass).ProcessTask())
                    {
                        // If ProcessTask() was successful, proceed to next step in the workflow.
                        nextStep = ((IWorkflowTask)stepClass).NextStep();
                        WorkflowController.TakeAction(processID, nextStep, string.Empty);
                    }
                    else
                    {
                        // If ProcessTask() was not successful, put this step to sleep and try again later.
                        WorkflowController.SleepStep(processID);
                    }
                }
                else
                {
                    // If ProcessTask() was not successful, put this step to sleep and try again later.
                    WorkflowController.SleepStep(processID);
                }
            }
            catch (Exception ex)
            {
                string msg = String.Format(Constants.LogString.WorkflowTaskFailed, "Process Id: " + processID.ToString(),
                    "Step Id: " + stepID.ToString(), ex.Message);
                log.CreateLogEntry(msg, Logging.LogPriority.Error, +LogCount);
                rtn = msg;

                WorkflowController.InsertExceptionForProcessTask(msg, processID, stepID);

                // If ProcessTask() was not successful, put this step to sleep and try again later.
                WorkflowController.SleepStep(processID);

                SendNotification(msg);
            }

            if (!string.IsNullOrEmpty(rtn)) rtn = "Process: " + processID.ToString() + " - " + rtn;
            return rtn;
        }

        public string ProcessTask(int processID, string assemblyName, string className)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry("Processing Workflow Process Id: " + processID.ToString(), +LogCount);
            return ProcessTask(processID, assemblyName, className, ref log);
        }

        public List<string> ProcessAllTasks()
        {
            List<string> rtn = new List<string>();

            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // Get list of tasks awaiting system action.
                DataSet dsAsyncTasks = new DataSet(); // We can parallely executes these tasks
                DataSet dsSyncTasks = new DataSet(); // We can only executes these tasks sequentially 

                //Ge the work flow steps which need to run synchronously
                string syncTaskIds = AppSettings.Get("WorkflowEngine-SyncFlowTaskIDs", "");

                //This will return us all the steps which we can execute parallely
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("Task_IDs", syncTaskIds));
                dsAsyncTasks = DataAccess.ExecuteStoredProcedure("usp_WF_SelectAsycActiveSystemSteps", parameters, "AsyncWFTasks");

                //This will return us all the steps which we need to run sequentially
                parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("Task_IDs", syncTaskIds));
                dsSyncTasks = DataAccess.ExecuteStoredProcedure("usp_WF_SelectSyncActiveSystemSteps", parameters, "SyncWFTasks");


                if (dsAsyncTasks != null && dsAsyncTasks.Tables.Count > 0 && dsAsyncTasks.Tables[0].Rows != null)
                {
                    //log.CreateLogEntry("Total async tasks to process " + dsAsyncTasks.Tables[0].Rows.Count);
                    //log.CreateLogEntry("Processing started at " + Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")));

                    bool isWorkFlowEngineParallelProcessingEnabled = Convert.ToBoolean(AppSettings.Get("WorkflowEngine-Parallel-Processing", "false"));
                    //log.CreateLogEntry("Parallel processing of tasks enabled: " + isWorkFlowEngineParallelProcessingEnabled);

                    int workflowEngineMaxDegreefParallelism = Convert.ToInt16(AppSettings.Get("WorkflowEngine-MaxDegreeOfParallelism", "10"));
                    if (isWorkFlowEngineParallelProcessingEnabled)
                    {
                        List<string> processIds = new List<string>();
                        Parallel.ForEach(dsAsyncTasks.Tables[0].AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = workflowEngineMaxDegreefParallelism }, dr =>
                        {
                            if (!processIds.Contains(dr["PROCESS_ID"].ToString()))
                            {
                                processIds.Add(dr["PROCESS_ID"].ToString());
                                var sch = NCrontab.Advanced.CrontabSchedule.Parse(dr["TASK_SCHEDULE"].ToString(), NCrontab.Advanced.Enumerations.CronStringFormat.WithSecondsAndYears);

                                if (sch.IsMatch(DateTime.Now))
                                {
                                    string result = ProcessTask(Convert.ToInt32(dr["PROCESS_ID"]), dr["ASSEMBLY_NAME"].ToString(),
                                        dr["CLASS_NAME"].ToString(), ref log);

                                    if (!string.IsNullOrEmpty(result)) rtn.Add(result);
                                }
                            }
                        });
                    }
                    else
                    {
                        //Execute async workflow steps sequentially if parallel pocessing is disabled
                        ProcessWorkFlowStepsSynchronously(dsAsyncTasks, log, rtn);
                    }
                }
                //Execute all the sequential workflow steps
                ProcessWorkFlowStepsSynchronously(dsSyncTasks, log, rtn);

                //log.CreateLogEntry("Processing all records completed at " + Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")));

                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Error occured while processing tasks. " + ex.StackTrace);
                throw ex;
            }
        }

        //All the steps in input table should always run in sync manner to avoid workflow issues
        private void ProcessWorkFlowStepsSynchronously(DataSet ds, Logging log, List<string> rtn)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var sch = NCrontab.Advanced.CrontabSchedule.Parse(dr["TASK_SCHEDULE"].ToString(), NCrontab.Advanced.Enumerations.CronStringFormat.WithSecondsAndYears);

                    if (sch.IsMatch(DateTime.Now))
                    {
                        string result = ProcessTask(Convert.ToInt32(dr["PROCESS_ID"]), dr["ASSEMBLY_NAME"].ToString(),
                            dr["CLASS_NAME"].ToString(), ref log);

                        if (!string.IsNullOrEmpty(result)) rtn.Add(result);
                    }
                }
            }
        }

        // Evalutate the last time an email was sent out for the specific Process ID. If it was longer ago than the specified minutes
        // between sending an email then send it. Otherwise do not send the email for the Process ID out.
        private bool SendIt(string emailBody)
        {
            bool rtn = false;
            if (!string.IsNullOrEmpty(emailBody))
            {
                if (emailBody.Length > 1000) emailBody = emailBody.Substring(0, 1000);
            }
            DataSet ds = WorkflowController.SelectLogWorkflow(CurrentProcessID);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DateTime lastUpdate = Convert.ToDateTime(ds.Tables[0].Rows[0]["LastActivityDateTime"].ToString());
                System.TimeSpan span = DateTime.Now - lastUpdate;
                // It has been more than specified minutes so send the Email. Default is 5 if the AppSetting does not exist.
                int minutes = Convert.ToInt32(AppSettings.Get("Workflow-Notifications-Interval-Minutes", "5"));
                if (span.Minutes >= minutes)
                {
                    WorkflowController.SaveLogWorkflow(CurrentProcessID, DateTime.Now, emailBody);
                    rtn = true;
                }
            }
            else
            {
                WorkflowController.SaveLogWorkflow(CurrentProcessID, DateTime.Now, emailBody);
                rtn = true;
            }
            return rtn;
        }

        private void SendNotification(string emailBody)
        {
            bool send = true;

            try
            {
                send = Convert.ToBoolean(AppSettings.Get("WorkflowNotificationsEnabled", bool.TrueString));
                if (send) send = SendIt(emailBody);
            }
            catch
            {
                // no action
            }

            if (send)
            {
                string recipients = AppSettings.Get("Jobs-NotificationEmailAddresses", "OHPNMCodeJunkies@maximus.com");
                string subject = AppSettings.Get("Workflow-Email-ExceptionSubject", "Workflow Exception");
                EMailNotification notify = new EMailNotification(emailBody, subject, recipients, this.ThreadId);

                // HACK: notify.SendNotification(recipients, subject, emailBody);
                // TODO: Temporary workaround below to bypass test email and send emails exceptions to Jobs-NotificationEmailAddresses
                notify.SendNotification(false);
            }
        }

    }
}