using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
//using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

using MAXIMUS.Core.Libraries;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using MAXIMUS.Core.Libraries.QuartzJobs;

namespace MAXIMUS.Core.Services
{
    class JobServiceModernMigrator
    {
        static void Main(string[] args)
        {
            ClearScheduler();
            BuildJobs();
        }

        private static void ClearScheduler()
        {
            //// NOTE: Do not uncomment unless you understand it will wipe out all jobs in the Quartz scheduler 
            //IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            //scheduler.Clear();
        }

        private static void BuildJobs()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            string logMethod = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            DataSet jobs;
            string JobTableName = "Job";

            List<string> dbases = new List<string>();
            dbases = GetDbStrings();
            string dbName = string.Empty;

            foreach (string dbase in dbases)
            {
                dbName = dbase;

                try
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("IsActive", DbType.Boolean, true, true));
                    jobs = DataAccess.ExecuteStoredProcedure(dbase, "sp_GetJobs", parameters, JobTableName + Constants.pluralEnding);

                    if (jobs.Tables.Count > 0)
                    {
                        foreach (DataRow job in jobs.Tables[0].Rows)
                        {
                            string intervalInMinutes = Methods.GetStringValue(job["IntervalInMinutes"]);
                            int minutes = 0;
                            if (int.TryParse(intervalInMinutes, out minutes)) { };
                            TimeSpan startTime = (TimeSpan)job["StartTime"];
                            TimeSpan endTime = (TimeSpan)job["EndTime"];
                            string daysOfWeek = job["DaysOfWeek"].ToString();
                            string name = job["Name"].ToString();
                            string jobId = job["Id"].ToString();
                            string jobTypeId = job["JobTypesId"].ToString();
                            string cronexpr = job["CRONEXPRESSION"].ToString();
                            Console.WriteLine("Building job " + name);

                            JobDataMap jobData;

                            // if (!jobExists(scheduler, name, false) & name != "Notice of Unopened Cost Report")
                            if (!jobExists(scheduler, name, false))
                            {
                                QuartzJob jobToBuild = QuartzJobBuilder.GetQuartzJob((int)job["JobTypesId"]);

                                if (jobTypeId == "4")
                                {
                                    // jf ::2020.10.27 - get job detail to prevent exceptions
                                    DataSet fullJobData = GetJobDetail(jobId);

                                    jobData = QuartzJobBuilder.BuildJobDataFromDataTable(fullJobData.Tables[0].Rows[0]
                                        , jobToBuild);
                                }
                                else
                                {
                                    jobData = QuartzJobBuilder.BuildJobDataFromDataTable(job, jobToBuild);
                                }

                                var jobDetail = JobBuilder.Create(jobToBuild.GetType())
                                       .StoreDurably()
                                       .WithIdentity(name)
                                       .SetJobData(jobData)
                                       .Build(); ;

                                if ((bool)job["IsActive"])
                                {
                                    Console.WriteLine("*********************************************");
                                    Console.WriteLine("Scheduling Job " + name);
                                    Quartz.Collection.HashSet<ITrigger> triggers = CreateCronTriggers(name + " Trigger", minutes, startTime, endTime, daysOfWeek, cronexpr);
                                    scheduler.ScheduleJob(jobDetail, triggers, true);
                                }

                                Console.WriteLine("Finished Importing Job: " + name);
                                Console.WriteLine("*********************************************");
                            }
                            else
                            {
                                Console.WriteLine("Job exists, skipping Job: " + name);
                                Console.WriteLine("*********************************************");
                                Console.WriteLine("");
                                Console.WriteLine("");
                            }

                            #region Simple Minute Trigger Code
                            //if (minutes != 0)
                            //{
                            //    JobDataMap jobData = new JobDataMap();
                            //    jobData.Add("JobID", job["Id"].ToString());

                            //    var jobDetail = JobBuilder.Create<MAXIMUS.Core.Libraries.QuartzJob>()
                            //       .StoreDurably()
                            //       .WithIdentity(job["Name"].ToString())
                            //       .SetJobData(jobData)
                            //       .Build();


                            //    var trigger = TriggerBuilder.Create()
                            //        .WithIdentity(job["Name"].ToString())
                            //        .StartNow()
                            //         .WithSimpleSchedule(x => x
                            //                                .WithIntervalInMinutes(minutes)
                            //                                .RepeatForever())
                            //        .Build();

                            //    scheduler.ScheduleJob(jobDetail, trigger);
                            //}
                            #endregion
                        }
                        #region  JobExecuteJob code
                        //OldCode 
                        //foreach (DataRow job in jobs.Tables[0].Rows)
                        //{

                        //    // stagingProviders.Relations[dbrProvider2Address].ChildTable);
                        //    string jobId = job["Id"].ToString();
                        //    string internalInMinutes = Methods.GetStringValue(job["IntervalInMinutes"]);
                        //    if (!String.IsNullOrEmpty(job["CurrentDTM"].ToString().Trim()))
                        //    {
                        //        currentDateTime = Convert.ToDateTime(job["CurrentDTM"].ToString().Trim());
                        //    }
                        //    DateTime? startDateTime = null;
                        //    if (!String.IsNullOrEmpty(job["StartTime"].ToString().Trim()))
                        //    {
                        //        startDateTime = Convert.ToDateTime(job["StartTime"].ToString().Trim());
                        //    }
                        //    DateTime? endDateTime = null;
                        //    if (!String.IsNullOrEmpty(job["EndTime"].ToString().Trim()))
                        //    {
                        //        endDateTime = Convert.ToDateTime(job["EndTime"].ToString().Trim());
                        //    }
                        //    DateTime? nextStartTime = null;
                        //    if (!String.IsNullOrEmpty(startDateTime.ToString().Trim()))
                        //    {
                        //        nextStartTime = startDateTime;
                        //    }
                        //    DateTime? nextEndTime = null;
                        //    if (!String.IsNullOrEmpty(endDateTime.ToString().Trim()))
                        //    {
                        //        nextEndTime = endDateTime;
                        //    }
                        //    DateTime lastRunTime = String.IsNullOrEmpty(job["LastRan"].ToString()) ? currentDateTime.AddDays(-1) : Convert.ToDateTime(job["LastRan"].ToString());
                        //    // TODO: string emaddr = String.IsNullOrEmpty(job["EmailAddresses"].ToString().Trim()) ? ConfigurationManager.AppSettings["EmailAddresses"].ToString() : nlritem["EmailAddresses"].ToString().Trim();

                        //    // get the interval and days of week the interface should execute
                        //    bool intMinValid = false;
                        //    int interval = 0;
                        //    ArrayList weekDays = DaysToExecuteInterface(Methods.GetStringValue(job["DaysOfWeek"]));
                        //    string today = currentDateTime.DayOfWeek.ToString();

                        //    // check to see if interval is a number
                        //    intMinValid = int.TryParse(internalInMinutes, out interval);

                        //    // if the today is one of the weekdays the interface should be executed
                        //    if (weekDays.Contains(today))
                        //    {
                        //        // if interval is not an empty string and is a number
                        //        if (intMinValid == true)
                        //        {

                        //            int runJob = 0;
                        //            bool validStart = false;
                        //            bool validEnd = false;

                        //            // if the start time is within the time frame or null
                        //            if (string.IsNullOrWhiteSpace(startDateTime.ToString()) || (currentDateTime.TimeOfDay >= startDateTime.Value.TimeOfDay))
                        //            {
                        //                validStart = true;
                        //            }

                        //            // if the end time is within the time frame or null
                        //            if (string.IsNullOrWhiteSpace(endDateTime.ToString()) || (currentDateTime.TimeOfDay <= endDateTime.Value.TimeOfDay))
                        //            {
                        //                validEnd = true;
                        //            }

                        //            // if the current interval job has a start and end time or both are null
                        //            if (validStart & validEnd)
                        //            {
                        //                runJob += 1;
                        //            }

                        //            if (!string.IsNullOrWhiteSpace(startDateTime.ToString()))
                        //            {
                        //                // add minutes to the last run time
                        //                nextStartTime = lastRunTime.AddMinutes(interval);
                        //            }

                        //            // if the job has not run within the interval
                        //            if (string.IsNullOrWhiteSpace(nextStartTime.ToString().Trim()) || (currentDateTime > nextStartTime))
                        //            {
                        //                runJob += 1;
                        //            }

                        //            // if the runJob variable has passed all checks
                        //            if (runJob == 2)
                        //            {
                        //                // run the job
                        //                executeJob(jobId);
                        //                UpdateJobInfo(dbase, jobId);
                        //            }
                        //        }
                        //        else // job is a daily job
                        //        {
                        //            int runJob = 0;

                        //            // if the current is between the job start and end time
                        //            if ((currentDateTime.TimeOfDay >= startDateTime.Value.TimeOfDay && currentDateTime.TimeOfDay <= endDateTime.Value.TimeOfDay))
                        //            {
                        //                runJob += 1;
                        //            }

                        //            // if the job has not run today
                        //            if (currentDateTime.Date > lastRunTime.Date)
                        //            {
                        //                runJob += 1;
                        //            }

                        //            // if the runJob variable has passed all checks
                        //            if (runJob == 2)
                        //            {
                        //                // run the job
                        //                executeJob(jobId);
                        //                UpdateJobInfo(dbase, jobId);
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion

                        // if no active jobs exist
                        if (jobs.Tables[0].Rows.Count == 0)
                        {
                            throw new Exception("No Active Jobs Exist");
                        }
                    }

                    scheduler.Shutdown();
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                }
                catch (Exception ex)
                {
                    scheduler.Shutdown();
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                }
            }
        }

        private static List<string> GetDbStrings()
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

        private static Quartz.Collection.HashSet<ITrigger> CreateCronTriggers(string triggerName, int minutes, TimeSpan startTime, TimeSpan endTime, string daysOfWeek, string cronexpr)
        {
            Quartz.Collection.HashSet<ITrigger> triggers = new Quartz.Collection.HashSet<ITrigger>();

            //Build days running on
            string daysForCronString = daysOfWeek == "1111111" ? "*" : string.Empty;
            if (string.IsNullOrWhiteSpace(daysForCronString))
            {
                List<string> daysToAdd = new List<string>();
                char[] days = daysOfWeek.ToArray();
                for (int i = 0; i < 7; i++)
                {
                    if (days[i] == '1')
                    {
                        switch (i)
                        {
                            case 0:
                                daysToAdd.Add("SUN");
                                break;
                            case 1:
                                daysToAdd.Add("MON");
                                break;
                            case 2:
                                daysToAdd.Add("TUE");
                                break;
                            case 3:
                                daysToAdd.Add("WED");
                                break;
                            case 4:
                                daysToAdd.Add("THU");
                                break;
                            case 5:
                                daysToAdd.Add("FRI");
                                break;
                            case 6:
                                daysToAdd.Add("SAT");
                                break;
                        }
                    }
                }
                daysForCronString = string.Join(",", daysToAdd.ToArray());
            }

            if (!string.IsNullOrEmpty(cronexpr))
            {
                triggers.Add(CreateCronTrigger(cronexpr, triggerName));
            }
            else
            {
                string genericCronExpression = "* {0} {1} ? * {2}";     //sec min hrs DayofMonth month Dayofweek

                //for current job scheduler if no minutes specified it will only run once on the start time on the days specified
                if (minutes == 0)
                {
                    string cronString =
                        string.Format(genericCronExpression,
                                      startTime.Minutes,
                                      startTime.Hours,
                                      daysForCronString);
                    triggers.Add(CreateCronTrigger(cronString, triggerName));
                }
                //for current job scheduler 
                else if (minutes == 60)
                {
                    if (startTime.Minutes == 0)
                    {
                        string cronString = string.Format(genericCronExpression,
                                                          "0",
                                                          startTime.Hours,
                                                          daysForCronString);
                        triggers.Add(CreateCronTrigger(cronString, triggerName));
                    }
                    else
                    {
                        for (int i = startTime.Hours; i < endTime.Hours; i++)
                        {
                            string cronString = string.Format(genericCronExpression,
                                                               startTime.Minutes,
                                                               i,
                                                               daysForCronString);

                            triggers.Add(CreateCronTrigger(cronString, triggerName + " " + i.ToString()));
                        }
                    }
                }
                else if (minutes < 60)
                {
                    if (startTime.Minutes == 0)
                    {
                        string timeRange = startTime.Hours == 0 && startTime.Minutes == 0 && endTime.Hours == 23 && endTime.Minutes == 59 ? "*" :
                            startTime.Hours + "-" + endTime.Hours;

                        string cronString =
                            string.Format(genericCronExpression,
                                           "0/" + minutes,
                                           timeRange,
                                           daysForCronString);
                        triggers.Add(CreateCronTrigger(cronString, triggerName));
                    }
                    else
                    {
                        string cronString1 =
                            string.Format(genericCronExpression,
                                           startTime.Minutes + "/" + minutes,
                                           startTime.Hours + "-" + (startTime.Hours + 1),
                                           daysForCronString);
                        triggers.Add(CreateCronTrigger(cronString1, triggerName + " 1"));

                        string cronString2 =
                            string.Format(genericCronExpression,
                                           "0/" + minutes,
                                           (startTime.Hours + 1) + "-" + (endTime.Hours - 1),
                                           daysForCronString);
                        triggers.Add(CreateCronTrigger(cronString2, triggerName + " 2"));

                        string cronString3 = string.Format(genericCronExpression,
                            "0-" + minutes,
                            (endTime.Hours - 1) + "-" + endTime.Hours,
                            daysForCronString);
                        triggers.Add(CreateCronTrigger(cronString3, triggerName + " 3"));
                    }
                }
                else if (minutes > 60)
                {
                    throw new Exception("Minute intervals greater than 60 are not currently supported in migration utility");
                }
            }

            return triggers;
        }

        private static ITrigger CreateCronTrigger(string cronTrigger, string triggerName)
        {
            ITrigger trigger = TriggerBuilder.Create()
                       .WithIdentity(triggerName)
                       .WithCronSchedule(cronTrigger)
                       .StartNow()
                       .Build();

            return trigger;
        }

        private static DataSet GetJobDetail(string jobId)
        {
            DataSet jobs = new DataSet();
            string dbtJobs = "Jobs";
            string dbtJobParams = "JobParams";
            string dbfId = "Id";
            string dbrJob2Parms = dbtJobs + "2" + dbtJobParams;
            string dbfJobParamsJobsId = "JobParamsJobsId";

            try
            {
                // execute the select stored procedure with parameters
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter(dbfId, DbType.Guid, jobId, false));
                jobs = DataAccess.ExecuteStoredProcedure("usp_GetJobById", parameters, "Jobs");

                jobs.Tables[0].TableName = dbtJobs;
                jobs.Tables[1].TableName = dbtJobParams;

                DataRelation relation;
                relation = new DataRelation(dbrJob2Parms
                    , jobs.Tables[dbtJobs].Columns[dbfId]
                    , jobs.Tables[dbtJobParams].Columns[dbfJobParamsJobsId]);
                jobs.Relations.Add(relation);

                return jobs;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static bool jobExists(IScheduler scheduler, string jobName, bool deleteJob)
        {
            bool returnValue = false;

            IList<string> jobGroups = scheduler.GetJobGroupNames();
            // IList<string> triggerGroups = scheduler.GetTriggerGroupNames();

            foreach (string group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = scheduler.GetJobKeys(groupMatcher);
                foreach (var jobKey in jobKeys)
                {
                    if (jobKey.Name == jobName)
                    {
                        returnValue = true;

                        if (deleteJob)
                        {
                            scheduler.DeleteJob(jobKey);
                        }
                    }
                }
            }

            return returnValue;
        }
    }
}
