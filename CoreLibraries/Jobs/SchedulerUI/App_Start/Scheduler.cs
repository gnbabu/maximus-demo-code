using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;


/// <summary>
/// Summary description for Scheduler
/// </summary>
namespace Scheduler.Business
{

    public class GroupStatus
    {
        public string Group { get; set; }
        public bool IsJobGroupPaused { get; set; }
        public bool IsTriggerGroupPaused { get; set; }
    }

    public class JobTrigger
    {
        public int Priority { get; set; }
        public string TriggerType { get; set; }
        public string TriggerState { get; set; }
        public string CronExpr { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime LastFire { get; set; }
        public DateTime NextFire { get; set; }

        public static JobTrigger FromTrigger(ITrigger trigger, IScheduler Instance)
        {
            var triggerItem = new JobTrigger();
            triggerItem.TriggerType = trigger.GetType().Name;
            triggerItem.TriggerState = Instance.GetTriggerState(trigger.Key).ToString();
            triggerItem.Priority = trigger.Priority;

            if (trigger is CronTriggerImpl)
            {
                CronTriggerImpl cronTrigger = (CronTriggerImpl)trigger;
                String cronExpr = cronTrigger.CronExpressionString;
                triggerItem.CronExpr = cronExpr;
            }

            DateTimeOffset? startTime = trigger.StartTimeUtc;
            triggerItem.StartTime = TimeZone.CurrentTimeZone.ToLocalTime(startTime.Value.DateTime);

            DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
            if (nextFireTime.HasValue)
            {
                triggerItem.NextFire = TimeZone.CurrentTimeZone.ToLocalTime
                              (nextFireTime.Value.DateTime);
            }

            DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
            if (previousFireTime.HasValue)
            {
                triggerItem.LastFire = TimeZone.CurrentTimeZone.ToLocalTime
                              (previousFireTime.Value.DateTime);
            }

            return triggerItem;
        }
    }

    public class JobSchedule
    {
        public JobSchedule()
        {
            TriggerList = new List<JobTrigger>();
        }

        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string AssemblyPath { get; set; }
        public string ClassName { get; set; }
        public string AssemblyName { get; set; }
        public bool EmailErrors { get; set; }
        public bool EmailEndofExecution { get; set; }

        public List<JobTrigger> TriggerList { get; set; }

        public static JobSchedule FromJobKey(JobKey jobKey, IScheduler Instance)
        {
            var jobDetail = Instance.GetJobDetail(jobKey);
            var triggers = Instance.GetTriggersOfJob(jobKey);
            var js = new JobSchedule();
            js.Name = jobKey.Name;
            js.Group = jobKey.Group;
            js.AssemblyName = jobDetail.JobDataMap["AssemblyName"].ToString();
            js.AssemblyPath = jobDetail.JobDataMap["AssemblyPath"].ToString();
            js.ClassName = jobDetail.JobDataMap["ClassName"].ToString();
            js.EmailEndofExecution = bool.Parse(jobDetail.JobDataMap["EmailEndOfExecution"].ToString());
            js.EmailErrors = bool.Parse(jobDetail.JobDataMap["EmailErrors"].ToString());

            foreach (var trigger in triggers)
            {
                js.TriggerList.Add(JobTrigger.FromTrigger(trigger, Instance));
            }

            return js;
        }

    }

    public class Scheduler
    {
        public readonly IScheduler Instance;

        public Scheduler()
        {
            try
            {
                Instance = StdSchedulerFactory.GetDefaultScheduler();

                if (!Instance.IsStarted)
                    Instance.Start();
            }
            catch (SchedulerException ex)
            {
                throw new Exception(string.Format("Failed: {0}", ex.Message));
            }
        }

        private static NameValueCollection GetProperties(string address)
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "ServerScheduler";
            properties["quartz.scheduler.proxy"] = "true";
            properties["quartz.threadPool.threadCount"] = "0";
            properties["quartz.scheduler.proxy.address"] = address;
            return properties;
        }

        public IScheduler GetScheduler()
        {
            return Instance;
        }

        public List<GroupStatus> GetGroups()
        {
            var results = new List<GroupStatus>();
            foreach (var gp in Instance.GetJobGroupNames())
            {
                results.Add(new GroupStatus()
                {
                    Group = gp,
                    IsJobGroupPaused = Instance.IsJobGroupPaused(gp),
                    IsTriggerGroupPaused = Instance.IsTriggerGroupPaused(gp)
                });
            }
            return results;
        }

        public JobSchedule GetSchedule(string jobName, string jobGroup)
        {
            var jobKey = new JobKey(jobName, jobGroup);

            return JobSchedule.FromJobKey(jobKey, Instance);
        }

        public List<JobSchedule> GetSchedules()
        {
            var jcs = new List<JobSchedule>();

            foreach (var group in Instance.GetJobGroupNames())
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = Instance.GetJobKeys(groupMatcher);

                foreach (var jobKey in jobKeys)
                {
                    jcs.Add(JobSchedule.FromJobKey(jobKey, Instance));
                }
            }
            return jcs;
        }

        public List<JobSchedule> GetSchedules(string groupName)
        {
            var jcs = new List<JobSchedule>();

            var groupMatcher = GroupMatcher<JobKey>.GroupContains(groupName);
            var jobKeys = Instance.GetJobKeys(groupMatcher);

            foreach (var jobKey in jobKeys)
            {
                jcs.Add(JobSchedule.FromJobKey(jobKey, Instance));
            }

            return jcs;
        }

        public string GetMetaData()
        {
            var metaData = Instance.GetMetaData();

            return string.Format(
                "{0}Name: '{1}'{0}Version: '{2}'{0}ThreadPoolSize: '{3}'{0}IsRemote: '{4}'{0}JobStoreName: '{5}' {0}SupportsPersistance: '{6}'{0}IsClustered: '{7}'", Environment.NewLine, metaData.SchedulerName, metaData.Version, metaData.ThreadPoolSize, metaData.SchedulerRemote, metaData.JobStoreType.Name, metaData.JobStoreSupportsPersistence, metaData.JobStoreClustered);
        }

        public bool UnscheduleJob(string jobName, string jobGroup)
        {
            var jobKey = new JobKey(jobName, jobGroup);

            if (Instance.CheckExists(jobKey))
            {
                return Instance.UnscheduleJob(new TriggerKey(jobName, jobGroup));
            }
            return false;
        }

        public bool UnscheduleAll()
        {
            foreach (var group in Instance.GetTriggerGroupNames())
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = Instance.GetJobKeys(groupMatcher);

                foreach (var triggers in jobKeys.Select(jobKey => Instance.GetTriggersOfJob(jobKey)))
                {
                    return Instance.UnscheduleJobs(triggers.Select(t => t.Key).ToList());
                }
            }
            return false;
        }

        public void DeleteAll()
        {
            Instance.Clear();
        }

        public void RescheduleJob(string jobName, string jobGroup, string cronExpression, int priority)
        {
            // Build new trigger
            var trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity(jobName, jobGroup)
                .WithCronSchedule(cronExpression)
                .WithPriority(priority)
                //.StartAt(StartAt.ToUniversalTime())
                .Build();

            Instance.RescheduleJob(new TriggerKey(jobName, jobGroup), trigger);
        }

        public void TriggerJob(string jobName, string jobGroup)
        {
            var jobKey = new JobKey(jobName, jobGroup);
            var triggers = Instance.GetTriggersOfJob(jobKey);
            foreach (var trigger in triggers)
            {
                if (Instance.GetTriggerState(trigger.Key) == TriggerState.Error)
                {
                    Instance.ResumeTrigger(trigger.Key);
                }
            }

            Instance.TriggerJob(new JobKey(jobName, jobGroup));
        }
    }
}
