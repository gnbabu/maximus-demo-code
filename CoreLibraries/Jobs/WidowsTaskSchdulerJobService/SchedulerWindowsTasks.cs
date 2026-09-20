using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32.TaskScheduler;
using MAXIMUS.Core.Libraries;
using System.Reflection;

namespace WidowsTaskSchdulerJobService
{
    class SchedulerWindowsTasks
    {
        static void Main(string[] args)
        {
            int logCnt = 0;
            // generate a new thread id GUID
            Guid ThreadId = Guid.NewGuid();
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(ThreadId, logMsg);
            try
            {
                Console.Out.WriteLine("Start WidowsTaskSchdulerJobService");
                if (args.Length != 3)
                {
                    log.CreateLogEntry("No parameters provided", +logCnt);
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("WARNING: No parameters given!");
                    Console.Out.WriteLine("SYNTAX: WidowsTaskSchdulerJobService.exe username password DeployServer");
                    Console.Out.WriteLine();
                }
                else
                {
                    Console.Out.WriteLine("Reading input parameters");
                    string username = args[0];
                    string password = args[1];
                    string DeployServer = args[2];
                    Console.Out.WriteLine("UserName is - " + username);

                    string Env = AppSettings.Get("Environment");
                    string jobExePath = string.Empty;

                    if (DeployServer.Equals("WEBAPI"))
                    {
                        jobExePath = AppSettings.Get("JobService-Jobs-WebAPI.exe");
                    } 
                    else if (DeployServer.Equals("WEB"))
                    {
                        jobExePath = AppSettings.Get("JobService-Jobs-Web.exe");
                    }
                    else
                    {
                        jobExePath = AppSettings.Get("JobService-Jobs.exe");
                    }

                    string folderName = "Maximus\\" + Env;

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("IsActive", DbType.Boolean, true, true));
                    parameters.Add(SqlParms.CreateParameter("DeployServer", DbType.String, DeployServer, true));
                    DataSet dsJobs = DataAccess.ExecuteStoredProcedure("usp_GetJobsInformation", parameters, "Jobs");

                    
                    Console.Out.WriteLine("Executing WidowsTaskSchdulerJobService in Environment - " + Env);
                    log.CreateLogEntry("Start WidowsTaskSchdulerJobService in Environment - " + Env, +logCnt);

                    if (dsJobs.Tables.Count > 0)
                    {
                        Console.Out.WriteLine("Start reading the active jobs from dataset.");
                        log.CreateLogEntry("Start reading the active jobs from dataset.", +logCnt);

                        // Create the folder in the TaskScheduler if not exists
                        using (TaskService tsvc = new TaskService())
                        {
                            if (tsvc.RootFolder.SubFolders.Any(a => a.Name == "Maximus"))
                            {
                                TaskFolder folder = tsvc.GetFolder("Maximus");
                                if (!folder.SubFolders.Any(a => a.Name == Env))
                                {
                                    Console.Out.WriteLine("Creating the " + Env +" folder under Maximus folder.");                                    
                                    folder.CreateFolder(Env);
                                    log.CreateLogEntry("Creating the " + Env + " folder under Maximus folder.", +logCnt);
                                }
                            }
                            else
                            {
                                Console.Out.WriteLine("Creating the folder structure - " + folderName);                                
                                tsvc.RootFolder.CreateFolder(folderName);
                                log.CreateLogEntry("Creating the folder structure - " + folderName, +logCnt);
                            }
                                
                        }
                        foreach (DataRow job in dsJobs.Tables[0].Rows)
                        {
                            string jobName = job["Name"].ToString();
                            string jobDesc = job["Description"].ToString();
                            string jobId = job["Id"].ToString();
                            TimeSpan jobstartTime = (TimeSpan)job["StartTime"];
                            string jobdaysOfWeek = job["DaysOfWeek"].ToString();
                            string jobintervalinMin = job["IntervalInMinutes"].ToString();
                            string jobcronexpr = job["CRONEXPRESSION"].ToString();

                            bool jobIsUpdated; 
                            if (string.IsNullOrEmpty(job["IsUpdated"].ToString())) jobIsUpdated = false;
                            else jobIsUpdated = Convert.ToBoolean(job["IsUpdated"]);

                            int jobExecutionTimeLimitinHours;
                            if (string.IsNullOrEmpty(job["ExecutionTimeLimitinHours"].ToString())) jobExecutionTimeLimitinHours = 2;
                            else jobExecutionTimeLimitinHours = Convert.ToInt32(job["ExecutionTimeLimitinHours"]);

                            string JobActionPath = Path.Combine(jobExePath , "Jobs.exe");
                            string JobActionParameter = "/JobId " + jobId;


                            Console.Out.WriteLine("Start for job - " + jobName + "-" + jobId);

                            using (TaskService ts = new TaskService())
                            {
                                TaskFolder folder = ts.GetFolder(folderName);
                                if (jobIsUpdated)// && folder.AllTasks.Any(t => t.Name == jobName))
                                {
                                    Console.Out.WriteLine("Job Definition Updated for job - " + jobName);
                                    //If there is any update to job information delete the existing task and recreate it
                                    var tasks = folder.GetTasks();
                                    foreach (Microsoft.Win32.TaskScheduler.Task task in tasks)
                                    {
                                        string taskdesc = task.Definition.RegistrationInfo.Description;
                                        string id = taskdesc.Substring(taskdesc.IndexOf("-") + 1);
                                        //Console.Out.WriteLine("Reading Task id - " + id);
                                        if (id.Trim() == jobId)
                                        {
                                            List<SqlParameter> param = new List<SqlParameter>();
                                            param.Add(SqlParms.CreateParameter("Id", DbType.Guid, jobId, true));
                                            bool isDeleteTask = Convert.ToBoolean(DataAccess.ExecuteScalar("sp_CheckDeletejobTask", param));

                                            if (isDeleteTask)
                                            {                                               
                                                Console.Out.WriteLine("Deleting task for job " + jobName);
                                                folder.DeleteTask(task.Name);
                                                log.CreateLogEntry("Job Definition Updated so deleting task for job - " + jobName, +logCnt);
                                            }
                                        }                                        
                                    }                                 
                                        
                                }
                                if (!folder.AllTasks.Any(t => t.Name == jobName))
                                {
                                    Console.Out.WriteLine("Creating task for job " + jobName);
                                    log.CreateLogEntry("Creating task for job " + jobName, +logCnt);
                                    // Create a new task definition and assign properties
                                    TaskDefinition td = ts.NewTask();
                                    td.RegistrationInfo.Description = jobDesc + " - " + jobId;

                                    string dt = DateTime.Today.ToShortDateString() + " " + jobstartTime.ToString();
                                    DateTime taskStartTime = Convert.ToDateTime(dt);

                                    // Create a trigger that will fire the task at this time every other day
                                    // td.Triggers.Add(new DailyTrigger { DaysInterval = 1, StartBoundary = Convert.ToDateTime(startTime), Enabled = true });
                                    if (string.IsNullOrEmpty(jobcronexpr))
                                    {
                                        Console.Out.WriteLine("The cron expression is null for this job.");
                                        log.CreateLogEntry("Reading the job schedule from DaysofWeek Column as cron expression is null", +logCnt);
                                        if (jobdaysOfWeek == "1111111")
                                        {
                                            Console.Out.WriteLine("Adding Daily Trigger");
                                            DailyTrigger daily = new DailyTrigger();
                                            daily.ExecutionTimeLimit = new TimeSpan(jobExecutionTimeLimitinHours, 0, 0);
                                            daily.DaysInterval = 1;
                                            daily.StartBoundary = taskStartTime;
                                            daily.Enabled = true;
                                            if (!string.IsNullOrEmpty(jobintervalinMin) && Convert.ToInt32(jobintervalinMin) > 0)
                                            {
                                                daily.Repetition.Interval = new TimeSpan(0, Convert.ToInt32(jobintervalinMin), 0);
                                                daily.Repetition.Duration = TimeSpan.FromDays(1);   // Change from repeat task every for duration of indefinite (TimeSpan.Zero) to 1 day
                                            }
                                            td.Triggers.Add(daily);
                                        }
                                        else
                                        {
                                            Console.Out.WriteLine("Adding Weekly Trigger");
                                            WeeklyTrigger weekly = new WeeklyTrigger();
                                            weekly.Enabled = true;
                                            weekly.StartBoundary = taskStartTime;
                                            weekly.ExecutionTimeLimit = new TimeSpan(jobExecutionTimeLimitinHours, 0, 0);
                                            if (!string.IsNullOrEmpty(jobintervalinMin) && Convert.ToInt32(jobintervalinMin) > 0)
                                            {
                                                weekly.Repetition.Interval = new TimeSpan(0, Convert.ToInt32(jobintervalinMin), 0);
                                                weekly.Repetition.Duration = TimeSpan.Zero;
                                            }

                                            List<string> daysToAdd = new List<string>();
                                            char[] days = jobdaysOfWeek.ToArray();
                                            for (int i = 0; i < 7; i++)
                                            {
                                                if (days[i] == '1')
                                                {
                                                    switch (i)
                                                    {
                                                        case 0:
                                                            daysToAdd.Add("1");
                                                            break;
                                                        case 1:
                                                            daysToAdd.Add("2");
                                                            break;
                                                        case 2:
                                                            daysToAdd.Add("4");
                                                            break;
                                                        case 3:
                                                            daysToAdd.Add("8");
                                                            break;
                                                        case 4:
                                                            daysToAdd.Add("16");
                                                            break;
                                                        case 5:
                                                            daysToAdd.Add("32");
                                                            break;
                                                        case 6:
                                                            daysToAdd.Add("64");
                                                            break;
                                                    }
                                                }
                                            }
                                            int ctr = 0;
                                            foreach (string item in daysToAdd)
                                            {
                                                DaysOfTheWeek daysofWeek = (DaysOfTheWeek)Enum.Parse(typeof(DaysOfTheWeek), item);
                                                if (ctr == 0)
                                                    weekly.DaysOfWeek = daysofWeek;
                                                else
                                                    weekly.DaysOfWeek = weekly.DaysOfWeek | daysofWeek;
                                                ctr++;
                                            }
                                            td.Triggers.Add(weekly);
                                        }
                                    }
                                    else             // Read the cron expression * * * * * *  - sec min hrs DayofMonth month Dayofweek
                                    {
                                        Console.Out.WriteLine("Reading the cron expression");
                                        log.CreateLogEntry("Reading the cron expression", +logCnt);
                                        jobcronexpr = jobcronexpr.Replace(" ", "|");

                                        //Console.WriteLine(jobcronexpr);

                                        List<string> jobcronList = new List<string>();
                                        foreach (var i in jobcronexpr.Split('|'))
                                        {
                                            jobcronList.Add(i);
                                        }

                                        int cronSeconds = 0;
                                        int cronMinutes = 0;
                                        int cronHours = 0;
                                        string cronDayofMonth = string.Empty;
                                        string cronMonth = string.Empty;
                                        string cronDayofWeek = string.Empty;
                                        string cronIntervalinMin = string.Empty;

                                        for (int i = 0; i < jobcronList.Count; i++)
                                        {
                                            switch (i)
                                            {
                                                case 0:
                                                    Console.Out.WriteLine("Reading Seconds from cronexpression - " + Convert.ToString(jobcronList[i]));
                                                    log.CreateLogEntry("Reading Seconds from cronexpression - " + Convert.ToString(jobcronList[i]), +logCnt);
                                                    cronSeconds = jobcronList[i] == "*" ? 0 : Convert.ToInt32(jobcronList[i]);
                                                    //Console.WriteLine(cronSeconds);
                                                    break;
                                                case 1:
                                                    Console.Out.WriteLine("Reading Minutes from cronexpression - " + Convert.ToString(jobcronList[i]));
                                                    log.CreateLogEntry("Reading Minutes from cronexpression - " + Convert.ToString(jobcronList[i]), +logCnt);
                                                    cronMinutes = jobcronList[i] == "*" ? 0 : Convert.ToInt32(jobcronList[i]);
                                                    if (jobcronList[i] == "*") cronIntervalinMin = "1";
                                                    //Console.WriteLine(cronMinutes);
                                                    break;
                                                case 2:
                                                    Console.Out.WriteLine("Reading Hours from cronexpression - " + Convert.ToString(jobcronList[i]));
                                                    log.CreateLogEntry("Reading Hours from cronexpression - " + Convert.ToString(jobcronList[i]), +logCnt);
                                                    cronHours = jobcronList[i] == "*" ? 0 : Convert.ToInt32(jobcronList[i]);
                                                    //Console.WriteLine(cronHours);
                                                    break;
                                                case 3:
                                                    Console.Out.WriteLine("Reading DayofMonth from cronexpression - " + Convert.ToString(jobcronList[i]));
                                                    log.CreateLogEntry("Reading DayofMonth from cronexpression - " + Convert.ToString(jobcronList[i]), +logCnt);
                                                    cronDayofMonth = jobcronList[i];
                                                    //Console.WriteLine(cronDayofMonth);
                                                    break;
                                                case 4:
                                                    Console.Out.WriteLine("Reading Month from cronexpression - " + Convert.ToString(jobcronList[i]));
                                                    log.CreateLogEntry("Reading Month from cronexpression - " + Convert.ToString(jobcronList[i]), +logCnt);
                                                    cronMonth = jobcronList[i];
                                                    //Console.WriteLine(cronMonth);
                                                    break;
                                                case 5:
                                                    Console.Out.WriteLine("Reading DayofWeek from cronexpression - " + Convert.ToString(jobcronList[i]));
                                                    log.CreateLogEntry("Reading DayofWeek from cronexpression - " + Convert.ToString(jobcronList[i]), +logCnt);
                                                    cronDayofWeek = jobcronList[i];
                                                    //Console.WriteLine(cronDayofWeek);
                                                    break;
                                            }
                                        }

                                        TimeSpan trgTimespan = new TimeSpan(cronHours, cronMinutes, cronSeconds);
                                        string cronDT = DateTime.Today.ToShortDateString() + " " + trgTimespan.ToString();
                                        taskStartTime = Convert.ToDateTime(cronDT);

                                        if ((cronDayofMonth == "*" || cronDayofMonth == "?") && (cronMonth == "*" || cronMonth == "?") && (cronDayofWeek == "*"
                                            || cronDayofWeek == "?"))
                                        {
                                            Console.Out.WriteLine("Creating Daily Trigger from cron expression");
                                            log.CreateLogEntry("Creating Daily Trigger from cron expression", +logCnt);
                                            DailyTrigger daily1 = new DailyTrigger();
                                            daily1.ExecutionTimeLimit = new TimeSpan(jobExecutionTimeLimitinHours, 0, 0);
                                            daily1.DaysInterval = 1;
                                            daily1.StartBoundary = taskStartTime;
                                            daily1.Enabled = true;
                                            if (!string.IsNullOrEmpty(cronIntervalinMin))
                                            {
                                                daily1.Repetition.Interval = new TimeSpan(0, Convert.ToInt32(cronIntervalinMin), 0);
                                                daily1.Repetition.Duration = TimeSpan.FromDays(1); 
                                            }
                                            td.Triggers.Add(daily1);
                                        }

                                        if (((cronDayofMonth != "*" && cronDayofMonth != "?") || (cronMonth != "*" && cronMonth != "?")) && (cronDayofWeek == "*"
                                            || cronDayofWeek == "?"))
                                        {
                                            Console.Out.WriteLine("Creating Monthly Trigger from cron expression");
                                            log.CreateLogEntry("Creating Monthly Trigger from cron expression", +logCnt);
                                            MonthlyTrigger monthly1 = new MonthlyTrigger();
                                            monthly1.StartBoundary = taskStartTime;

                                            List<int> daysList = new List<int>();
                                            foreach (var mon in cronDayofMonth.Split(','))
                                            {
                                                daysList.Add(Convert.ToInt16(mon));
                                            }
                                            monthly1.DaysOfMonth = daysList.ToArray();
                                            monthly1.Enabled = true;
                                            monthly1.ExecutionTimeLimit = new TimeSpan(jobExecutionTimeLimitinHours, 0, 0);
                                            if (!string.IsNullOrEmpty(cronIntervalinMin))
                                            {
                                                monthly1.Repetition.Interval = new TimeSpan(0, Convert.ToInt32(cronIntervalinMin), 0);
                                                monthly1.Repetition.Duration = TimeSpan.Zero;
                                            }
                                            List<string> months = new List<string>();
                                            foreach (var mon in cronMonth.Split(','))
                                            {
                                                months.Add(mon);
                                            }

                                            List<string> monthsToAdd = new List<string>();
                                            foreach (string i in months)
                                            {
                                                switch (i.ToLower())
                                                {
                                                    case "jan":
                                                        monthsToAdd.Add("1");
                                                        break;
                                                    case "feb":
                                                        monthsToAdd.Add("2");
                                                        break;
                                                    case "mar":
                                                        monthsToAdd.Add("4");
                                                        break;
                                                    case "apr":
                                                        monthsToAdd.Add("8");
                                                        break;
                                                    case "may":
                                                        monthsToAdd.Add("16");
                                                        break;
                                                    case "jun":
                                                        monthsToAdd.Add("32");
                                                        break;
                                                    case "jul":
                                                        monthsToAdd.Add("64");
                                                        break;
                                                    case "aug":
                                                        monthsToAdd.Add("128");
                                                        break;
                                                    case "sep":
                                                        monthsToAdd.Add("256");
                                                        break;
                                                    case "oct":
                                                        monthsToAdd.Add("512");
                                                        break;
                                                    case "nov":
                                                        monthsToAdd.Add("1024");
                                                        break;
                                                    case "dec":
                                                        monthsToAdd.Add("2048");
                                                        break;
                                                }
                                            }
                                            int ctr = 0;
                                            foreach (string item in monthsToAdd)
                                            {
                                                MonthsOfTheYear monthofyear = (MonthsOfTheYear)Enum.Parse(typeof(MonthsOfTheYear), item);
                                                if (ctr == 0)
                                                    monthly1.MonthsOfYear = monthofyear;
                                                else
                                                    monthly1.MonthsOfYear = monthly1.MonthsOfYear | monthofyear;
                                                ctr++;
                                            }
                                            td.Triggers.Add(monthly1);
                                        }

                                        if ((cronDayofMonth == "*" || cronDayofMonth == "?") && (cronMonth == "*" || cronMonth == "?") && (cronDayofWeek != "*"
                                            && cronDayofWeek != "?"))
                                        {
                                            Console.Out.WriteLine("Creating Weekly Trigger from cron expression");
                                            log.CreateLogEntry("Creating Weekly Trigger from cron expression", +logCnt);
                                            WeeklyTrigger weekly1 = new WeeklyTrigger();
                                            weekly1.Enabled = true;
                                            weekly1.StartBoundary = taskStartTime;
                                            weekly1.ExecutionTimeLimit = new TimeSpan(jobExecutionTimeLimitinHours, 0, 0);
                                            if (!string.IsNullOrEmpty(cronIntervalinMin))
                                            {
                                                weekly1.Repetition.Interval = new TimeSpan(0, Convert.ToInt32(cronIntervalinMin), 0);
                                                weekly1.Repetition.Duration = TimeSpan.Zero;
                                            }
                                            List<string> weekdays = new List<string>();
                                            foreach (var mon in cronDayofWeek.Split(','))
                                            {
                                                weekdays.Add(mon);
                                            }
                                            List<string> daysToAdd = new List<string>();
                                            foreach (string i in weekdays)
                                            {
                                                switch (i.ToUpper())
                                                {
                                                    case "1":
                                                        daysToAdd.Add("1");
                                                        break;
                                                    case "SUN":
                                                        daysToAdd.Add("1");
                                                        break;
                                                    case "2":
                                                        daysToAdd.Add("2");
                                                        break;
                                                    case "MON":
                                                        daysToAdd.Add("2");
                                                        break;
                                                    case "3":
                                                        daysToAdd.Add("4");
                                                        break;
                                                    case "TUE":
                                                        daysToAdd.Add("4");
                                                        break;
                                                    case "4":
                                                        daysToAdd.Add("8");
                                                        break;
                                                    case "WED":
                                                        daysToAdd.Add("8");
                                                        break;
                                                    case "5":
                                                        daysToAdd.Add("16");
                                                        break;
                                                    case "THU":
                                                        daysToAdd.Add("16");
                                                        break;
                                                    case "6":
                                                        daysToAdd.Add("32");
                                                        break;
                                                    case "FRI":
                                                        daysToAdd.Add("32");
                                                        break;
                                                    case "7":
                                                        daysToAdd.Add("64");
                                                        break;
                                                    case "SAT":
                                                        daysToAdd.Add("64");
                                                        break;
                                                }

                                            }
                                            int ctr = 0;
                                            foreach (string item in daysToAdd)
                                            {
                                                DaysOfTheWeek daysofWeek = (DaysOfTheWeek)Enum.Parse(typeof(DaysOfTheWeek), item);
                                                if (ctr == 0)
                                                    weekly1.DaysOfWeek = daysofWeek;
                                                else
                                                    weekly1.DaysOfWeek = weekly1.DaysOfWeek | daysofWeek;
                                                ctr++;
                                            }
                                            td.Triggers.Add(weekly1);
                                        }
                                    }

                                    // Create an action for the task
                                    td.Actions.Add(new ExecAction(JobActionPath, JobActionParameter, jobExePath));

                                    td.Settings.Enabled = true;
                                    td.Settings.ExecutionTimeLimit = new TimeSpan(jobExecutionTimeLimitinHours, 0, 0);
                                    
                                    if(!string.IsNullOrEmpty(jobintervalinMin) && Convert.ToInt32(jobintervalinMin) > 0)
                                       td.Settings.StartWhenAvailable = true;  // to enable setting 'Run task as soon as possible after a scheduled start is missed'

                                    // Register the task in the given folder path
                                    // TaskLogonType.Password - to set the settings - Run whether user is logged on or not
                                    folder.RegisterTaskDefinition(jobName, td, TaskCreation.CreateOrUpdate, username, password, TaskLogonType.Password);

                                    Console.Out.WriteLine("Done Creating task in task scheduler.");
                                    log.CreateLogEntry("Done Creating task in task scheduler.", +logCnt);

                                    // Store the job details set in the task in table JobsData_TaskScheduler 
                                    List<SqlParameter> param1 = new List<SqlParameter>();
                                    param1.Add(SqlParms.CreateParameter("Id", DbType.Guid, jobId, true));
                                    param1.Add(SqlParms.CreateParameter("Name", DbType.String, jobName, true));
                                    param1.Add(SqlParms.CreateParameter("StartTime", DbType.String, jobstartTime.ToString(), true));
                                    param1.Add(SqlParms.CreateParameter("IsActive", DbType.Boolean, true, true));
                                    if(!string.IsNullOrEmpty(jobintervalinMin))
                                        param1.Add(SqlParms.CreateParameter("IntervalInMinutes", DbType.Int32, Convert.ToInt32(jobintervalinMin), true));
                                    param1.Add(SqlParms.CreateParameter("DaysOfWeek", DbType.String, jobdaysOfWeek, true));
                                    param1.Add(SqlParms.CreateParameter("CRONEXPRESSION", DbType.String, jobcronexpr, true));
                                    param1.Add(SqlParms.CreateParameter("ExecutionTimeLimitinHours", DbType.Int32, jobExecutionTimeLimitinHours, true));
                                    DataAccess.ExecuteStoredProcedure("sp_InsertorUpdate_JobsData_TaskScheduler", param1, "JobsUpd");

                                    log.CreateLogEntry("Done Creating task for job - " + jobName, +logCnt);
                                }
                                else
                                    Console.Out.WriteLine("Task already exist for job - " + jobName);
                            }
                            
                        }
                        Console.Out.WriteLine();
                        Console.Out.WriteLine("Completed Execution. EXITING..");
                        log.CreateLogEntry("Done Creating tasks for all Active jobs", +logCnt);                        
                    }
                    else
                    {
                        Console.Out.WriteLine("No Active jobs found in the Jobs table. EXITING..");
                        log.CreateLogEntry("Done no Active jobs in the Jobs table", +logCnt);
                    }
                }
                //Console.ReadLine();
                System.Environment.Exit(1);
            }
            catch(Exception ex)
            {
                log.CreateLogEntry("WidowsTaskSchdulerJobService failed. Reason: " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error, +logCnt);
                Console.WriteLine(ex.Message + " - " + ex.StackTrace);                
               // Console.ReadLine();
                System.Environment.Exit(1);
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            }            
        }
    }
}
