using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;

using MAXIMUS.Core.Libraries;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;
using CommandLine;
using CommandLine.Text;
using log4net.Repository.Hierarchy;
using log4net;
using log4net.Appender;
using System.IO;

namespace MAXIMUS.Core.Services
{
    public partial class JobService : ServiceBase
    {

        class Options
        {
            [Option('d', "database", Required = true,
              HelpText = "Database to be used for running jobs")]
            public string DBConnectionStringName { get; set; }

            [Option('n', "servicename", Required = true,
              HelpText = "Service name for the job")]
            public string ServiceName { get; set; }


            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        #region "Class Variables"

            Thread listenThread;
            bool serviceStarted = true;
            DateTime currentDateTime = DateTime.Now;
            string dbName = "mainDB";
            IScheduler scheduler;

        #endregion

        #region "Constructors"

        public JobService()
        {
            var commandLineArgs = Environment.GetCommandLineArgs();
            Options options = new Options();
            CommandLine.Parser.Default.ParseArguments(commandLineArgs, options);

            InitializeComponent();
            // generate a new thread id GUID
            this.ThreadId = Guid.NewGuid();
            this.ServiceName = options.ServiceName;

        }

        #endregion

        #region "Logging Objects"

        private Guid threadId;
        private Guid ThreadId
        {
            get
            {
                return this.threadId;
            }
            set
            {
                this.threadId = value;
            }
        }

        #endregion

        internal void TestStart()
        {
            //string[] args = new string[0];
            //OnStart(args);
            ServiceProcess();
        }

        private static void ConfigureLog4Net(string connectionString)
        {
            Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            if (hierarchy != null && hierarchy.Configured)
            {
                foreach (IAppender appender in hierarchy.GetAppenders())
                {
                    if (appender is AdoNetAppender)
                    {
                        var adoNetAppender = (AdoNetAppender)appender;
                        adoNetAppender.ConnectionString = connectionString;
                        adoNetAppender.ActivateOptions(); //Refresh AdoNetAppenders Settings
                    }
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            string logMethod = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            string startMessage = String.Format(Constants.LogString.JobServiceStartStopMessage, "started", this.ThreadId);
            EventLog.WriteEntry(startMessage, EventLogEntryType.Information);
            WriteLog(startMessage, Logging.LogPriority.Information, logMethod);
            var commandLineArgs = Environment.GetCommandLineArgs();
            Options options = new Options();
            CommandLine.Parser.Default.ParseArguments(commandLineArgs, options);
 
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Modernization"]))
            {
                NameValueCollection schedulerPropertiesFromConfig = (NameValueCollection)ConfigurationManager.GetSection(StdSchedulerFactory.ConfigurationSectionName);
                NameValueCollection schedulerProperties = new NameValueCollection(schedulerPropertiesFromConfig); // The collection schedulerPropertiesFromConfig is readonly

                schedulerProperties[StdSchedulerFactory.PropertyDataSourcePrefix + ".default." + StdSchedulerFactory.PropertyDataSourceConnectionString] = AppSettings.GetConnectionString(options.DBConnectionStringName);
                string jobServicePort = AppSettings.Get("JobServicePort");
                schedulerProperties[StdSchedulerFactory.PropertySchedulerExporterPrefix + ".port"] = jobServicePort;
                StdSchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerProperties);
                ConfigureLog4Net(AppSettings.GetConnectionString(options.DBConnectionStringName));

                scheduler = schedulerFactory.GetScheduler();
                scheduler.Start();
            }
            else
            {
                ThreadStart st = new ThreadStart(ServiceProcess);
                listenThread = new Thread(st);
                listenThread.Start();
            }
        }

        protected override void OnStop()
        {
            string logMethod = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            string stopMessage = String.Format(Constants.LogString.JobServiceStartStopMessage, "stopped", this.ThreadId);
            EventLog.WriteEntry(stopMessage, EventLogEntryType.Information);
            WriteLog(stopMessage, Logging.LogPriority.Information, logMethod);
            serviceStarted = false;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Modernization"]))
            {
                scheduler.Shutdown();
            }
            else
            {
                listenThread.Abort();
                listenThread.Join();
            }
        }

        public void ServiceProcess()
        {
            string logMethod = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            DataSet jobs;
            string JobTableName = "Job";
            int sleepTime = 60000; 

            while (serviceStarted)
            // if (serviceStarted)
            {
                List<string> dbases = new List<string>();
                dbases = GetDbStrings();

                foreach (string dbase in dbases)
                {
                    dbName = dbase;
                    sleepTime = Convert.ToInt32(AppSettings.Get(dbName, "JobService-SleepTime", Convert.ToString(60000))); 

                    try
                    {
                        // generated by sp_Admin_StoredProcBuilder on Sep 20 2012  3:36PM
                        // create parameters objects and fill with values
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("IsActive", DbType.Boolean, true, true));
                        jobs = DataAccess.ExecuteStoredProcedure(dbase,"sp_GetJobs", parameters, JobTableName + Constants.pluralEnding);

                        // if the Job datatable exists
                        if (jobs.Tables.Count > 0)
                        {
                            foreach (DataRow job in jobs.Tables[0].Rows)
                            {

                                // stagingProviders.Relations[dbrProvider2Address].ChildTable);
                                string jobId = job["Id"].ToString();
                                if (!string.IsNullOrEmpty(job["Cronexpression"].ToString()))
                                {
                                    var sch = NCrontab.Advanced.CrontabSchedule.Parse(job["Cronexpression"].ToString(), NCrontab.Advanced.Enumerations.CronStringFormat.WithSecondsAndYears);
                                    if (sch.IsMatch(DateTime.Now))
                                    {
                                        // run the job
                                        executeJob(jobId);
                                        UpdateJobInfo(dbase, jobId);
                                    }

                                }
                                else
                                {
                                    string internalInMinutes = Methods.GetStringValue(job["IntervalInMinutes"]);
                                    if (!String.IsNullOrEmpty(job["CurrentDTM"].ToString().Trim()))
                                    {
                                        currentDateTime = Convert.ToDateTime(job["CurrentDTM"].ToString().Trim());
                                    }
                                    DateTime? startDateTime = null;
                                    if (!String.IsNullOrEmpty(job["StartTime"].ToString().Trim()))
                                    {
                                        startDateTime = Convert.ToDateTime(job["StartTime"].ToString().Trim());
                                    }
                                    DateTime? endDateTime = null;
                                    if (!String.IsNullOrEmpty(job["EndTime"].ToString().Trim()))
                                    {
                                        endDateTime = Convert.ToDateTime(job["EndTime"].ToString().Trim());
                                    }
                                    DateTime? nextStartTime = null;
                                    if (!String.IsNullOrEmpty(startDateTime.ToString().Trim()))
                                    {
                                        nextStartTime = startDateTime;
                                    }
                                    DateTime? nextEndTime = null;
                                    if (!String.IsNullOrEmpty(endDateTime.ToString().Trim()))
                                    {
                                        nextEndTime = endDateTime;
                                    }
                                    DateTime lastRunTime = String.IsNullOrEmpty(job["LastRan"].ToString()) ? currentDateTime.AddDays(-1) : Convert.ToDateTime(job["LastRan"].ToString());
                                    // TODO: string emaddr = String.IsNullOrEmpty(job["EmailAddresses"].ToString().Trim()) ? ConfigurationManager.AppSettings["EmailAddresses"].ToString() : nlritem["EmailAddresses"].ToString().Trim();

                                    // get the interval and days of week the interface should execute
                                    bool intMinValid = false;
                                    int interval = 0;
                                    ArrayList weekDays = DaysToExecuteInterface(Methods.GetStringValue(job["DaysOfWeek"]));
                                    string today = currentDateTime.DayOfWeek.ToString();

                                    // check to see if interval is a number
                                    intMinValid = int.TryParse(internalInMinutes, out interval);

                                    // if the today is one of the weekdays the interface should be executed
                                    if (weekDays.Contains(today))
                                    {
                                        // if interval is not an empty string and is a number
                                        if (intMinValid == true)
                                        {

                                            int runJob = 0;
                                            bool validStart = false;
                                            bool validEnd = false;

                                            // if the start time is within the time frame or null
                                            if (string.IsNullOrWhiteSpace(startDateTime.ToString()) || (currentDateTime.TimeOfDay >= startDateTime.Value.TimeOfDay))
                                            {
                                                validStart = true;
                                            }

                                            // if the end time is within the time frame or null
                                            if (string.IsNullOrWhiteSpace(endDateTime.ToString()) || (currentDateTime.TimeOfDay <= endDateTime.Value.TimeOfDay))
                                            {
                                                validEnd = true;
                                            }

                                            // if the current interval job has a start and end time or both are null
                                            if (validStart & validEnd)
                                            {
                                                runJob += 1;
                                            }

                                            if (!string.IsNullOrWhiteSpace(startDateTime.ToString()))
                                            {
                                                // add minutes to the last run time
                                                nextStartTime = lastRunTime.AddMinutes(interval);
                                            }

                                            // if the job has not run within the interval
                                            if (string.IsNullOrWhiteSpace(nextStartTime.ToString().Trim()) || (currentDateTime > nextStartTime))
                                            {
                                                runJob += 1;
                                            }

                                            // if the runJob variable has passed all checks
                                            if (runJob == 2)
                                            {
                                                // run the job
                                                executeJob(jobId);
                                                UpdateJobInfo(dbase, jobId);
                                            }
                                        }
                                        else // job is a daily job
                                        {
                                            int runJob = 0;

                                            // if the current is between the job start and end time
                                            if ((currentDateTime.TimeOfDay >= startDateTime.Value.TimeOfDay && currentDateTime.TimeOfDay <= endDateTime.Value.TimeOfDay))
                                            {
                                                runJob += 1;
                                            }

                                            // if the job has not run today
                                            if (currentDateTime.Date > lastRunTime.Date)
                                            {
                                                runJob += 1;
                                            }

                                            // if the runJob variable has passed all checks
                                            if (runJob == 2)
                                            {
                                                // run the job
                                                executeJob(jobId);
                                                UpdateJobInfo(dbase, jobId);
                                            }
                                        }
                                    }
                                }
                            }   // End of foreach

                                // if no active jobs exist
                                if (jobs.Tables[0].Rows.Count == 0)
                                {
                                    WriteLog(Constants.LogString.JobServiceNoActiveJobs, Logging.LogPriority.Information, logMethod);
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog(String.Format(Constants.LogString.ExceptionEncountered, ex.Message), Logging.LogPriority.Error, logMethod);
                    }
                }
                if (serviceStarted)
                {
                    // Put a breakpoint on the following line to always catch
                    // your service when it has finished its work
                    Thread.Sleep(sleepTime);
                }
            }
        }

        #region "Private Methods"

        private void executeJob(string jobId)
        {
            try
            {
                string cmdName = "Jobs.exe";
                string settingName = String.Format("JobService-{0}", cmdName);
                string location = AppSettings.Get(dbName, settingName, string.Empty);
                if (System.Diagnostics.Debugger.IsAttached) location = @"C:\Projects\DCPDMS\CoreLibraries\Jobs\bin\Debug\";
                executeProcess(location, cmdName, jobId);

            }
            catch
            {
                // no action
            }
        }

        private void executeProcess(string commandLocation, string commandToExecute, string commandParameters, string workingDirectory = "")
        {
            // create log object
            string logMethod = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            String commandOutput = string.Empty;

            try
            {
                string command = commandLocation + commandToExecute;
                string parms = @"/JobId " + commandParameters;
                System.Diagnostics.Process pProcess = new System.Diagnostics.Process();

                //strCommand is path and file name of command to run 
                pProcess.StartInfo.FileName = command;

                //strCommandParameters are parameters to pass to program 
                pProcess.StartInfo.Arguments = parms;
                // pProcess.StartInfo.Arguments = commandParameters;
                pProcess.StartInfo.UseShellExecute = false;                //Set output of program to be written to process output stream
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.RedirectStandardError = true;

                //Optional 
                if (!String.IsNullOrEmpty(workingDirectory))
                {
                    pProcess.StartInfo.WorkingDirectory = workingDirectory;
                }
                pProcess.StartInfo.CreateNoWindow = true;

                //Start the process 
                pProcess.Start();

                //Get program output 
                commandOutput = pProcess.StandardOutput.ReadToEnd();

                bool showCmdData = Convert.ToBoolean(AppSettings.Get(dbName,"JobService-ShowCmdData", bool.TrueString));

                //Wait for process to finish 
                pProcess.WaitForExit();
                if (showCmdData)
                {
                    WriteLog("Executing command: " + command, Logging.LogPriority.Information, logMethod);
                    WriteLog("Parameters: " + parms, Logging.LogPriority.Information, logMethod);
                    WriteLog("Output: " + commandOutput.ToString(), Logging.LogPriority.Information, logMethod);
                }

                pProcess.Close();
                pProcess.Dispose();
            }

            catch (Exception ex)
            {
                WriteLog(String.Format(Constants.LogString.ExceptionEncountered, ex.Message)
                        , Logging.LogPriority.Error, logMethod);
            }
        }

        /// <summary>
        ///     Retrieves all days that an interfaces should be executed from the NLRBatchPrograms table, DaysOfWeek field.
        ///     The format is Sunday - Saturday, binary (0-off, 1-on).
        ///     For example: an interface that only runs on Monday and Wednesday would have the input parameter
        ///         days string: 01010000
        /// </summary>
        /// <param name="days">The days binary string indicating which days the interface should run</param>
        /// <returns>An array of all days which the interface should run</returns>
        private ArrayList DaysToExecuteInterface(string days)
        {
            string logMethod = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            const string dayEnabled = "1";
            ArrayList returnValue = new ArrayList();

            try
            {
                // loop over the days one-by-one
                for (int i = 0; i < days.Length; i++)
                {
                    switch (i)
                    {
                        case 0: // sunday
                            if (days[i].ToString() == dayEnabled) returnValue.Add(DayOfWeek.Sunday.ToString());
                            break;
                        case 1: // monday
                            if (days[i].ToString() == dayEnabled) returnValue.Add(DayOfWeek.Monday.ToString());
                            break;
                        case 2: // tuesday
                            if (days[i].ToString() == dayEnabled) returnValue.Add(DayOfWeek.Tuesday.ToString());
                            break;
                        case 3: // wednesday
                            if (days[i].ToString() == dayEnabled) returnValue.Add(DayOfWeek.Wednesday.ToString());
                            break;
                        case 4: // thursday
                            if (days[i].ToString() == dayEnabled) returnValue.Add(DayOfWeek.Thursday.ToString());
                            break;
                        case 5: // friday
                            if (days[i].ToString() == dayEnabled) returnValue.Add(DayOfWeek.Friday.ToString());
                            break;
                        case 6: // saturday
                            if (days[i].ToString() == dayEnabled) returnValue.Add(DayOfWeek.Saturday.ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(String.Format(Constants.LogString.ExceptionEncountered, ex.Message)
                        , Logging.LogPriority.Error, logMethod);
            }
            return returnValue;
        }

        /// <summary>
        ///     Attempts to retrieve additional database strings from AppSettings.dll.config file
        /// </summary>
        /// <returns>A list of strings</returns>
        private List<string> GetDbStrings()
        {
            List<string> returnVal = new List<string>();
            string conn = string.Empty;

            try
            {
                conn = AppSettings.GetConnectionString();
                if (conn.Length > 0) returnVal.Add("mainDB");
            }
            catch
            {
                // no action
            }
            try
            {
                conn = AppSettings.GetConnectionString("JobDb2");
                if (conn.Length > 0) returnVal.Add("JobDb2");
            }
            catch
            {
                // no action
            }
            try
            {
                conn = AppSettings.GetConnectionString("JobDb3");
                if (conn.Length > 0) returnVal.Add("JobDb3");
            }
            catch
            {
                // no action
            }
            try
            {
                conn = AppSettings.GetConnectionString("JobDb4");
                if (conn.Length > 0) returnVal.Add("JobDb4");
            }
            catch
            {
                // no action
            }
            try
            {
                conn = AppSettings.GetConnectionString("JobDb5");
                if (conn.Length > 0) returnVal.Add("JobDb5");
            }
            catch
            {
                // no action
            }
            return returnVal;
        }

        private void UpdateJobInfo(string dbase, string jobId)
        {
            string logMethod = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep 24 2012 12:30PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("Id", DbType.Guid, jobId, true));
                parameters.Add(SqlParms.CreateParameter("LastRan", DbType.DateTime, currentDateTime, true));

                DataAccess.ExecuteStoredProcedure(dbase, "sp_UpdateJobById", parameters,string.Empty);
            }
            catch (Exception ex)
            {
                WriteLog(String.Format(Constants.LogString.ExceptionEncountered, ex.Message)
                        , Logging.LogPriority.Error, logMethod);
            }
        }

        private void WriteLog(string logMessage, Logging.LogPriority logPriority, string logMethod)
        {
            try
            {
                // create log object
                Logging log = new Logging(this.ThreadId, logMethod);
                string logEntryFormat = "{0}: {1}||{2}";
                bool logSuccess = log.CreateLogEntry(logMessage, logPriority);
                if (logSuccess == false)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    string fileName = "JobServiceLog.txt";
                    string fullName = path + fileName;
                    log.CreateLogEntry("Location of log file: " + fullName);
                    System.IO.StreamWriter file = new System.IO.StreamWriter(fullName);
                    file.WriteLine(String.Format(logEntryFormat, currentDateTime.ToString(), logMessage, logPriority));
                }
            }
            catch(Exception ex)
            {
                EventLog.WriteEntry(String.Format(Constants.LogString.ExceptionEncountered, ex.Message)
                       , EventLogEntryType.Error);
                // sleepTime = 300000; // set default sleep to 5 minutes
            }
        }
        #endregion
    }
}
