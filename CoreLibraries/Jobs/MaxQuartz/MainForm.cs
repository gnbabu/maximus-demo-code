using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace MaxQuartz
{
    public partial class MainForm : Form
    {
        private IScheduler _scheduler;
        private Color txtColor;

        public MainForm()
        {
            InitializeComponent();

            NameValueCollection myParamsCollection = (NameValueCollection)ConfigurationManager.GetSection("quartz");

            string db = myParamsCollection["quartz.dataSource.default.connectionString"].ToString();

            Dictionary<string, string> quartzConn = parseConnectionString(db);

            this.Text = "Maximus Quartz Job Manager";
            lblDatabase.Text = "Database: " + quartzConn["Initial Catalog"];

            txtColor = txtCronExpression.BackColor;
        }

        private void btnGetJobs_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                LoadJobs();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void GetJobTriggers(JobKey jobKey)
        {
            try
            {
                btnPauseJob.Enabled = false;
                btnEnableJob.Enabled = false;

                _scheduler = new StdSchedulerFactory().GetScheduler();

                lbTrigger.Items.Clear();

                var triggers = _scheduler.GetTriggersOfJob(jobKey);
                foreach (ITrigger trigger in triggers)
                {
                    TriggerState ts = _scheduler.GetTriggerState(trigger.Key);

                    lbTrigger.Items.Add(trigger);
                    lbTrigger.Items.Add("TriggerState: " + ts.ToString());
                    lbTrigger.Items.Add("trigger.Key.Name: " + trigger.Key.Name);
                    lbTrigger.Items.Add("trigger.Key.Group: " + trigger.Key.Group);
                    lbTrigger.Items.Add("trigger.GetType().Name: " + trigger.GetType().Name);
                    DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
                    if (nextFireTime.HasValue)
                    {
                        lbTrigger.Items.Add("nextFireTime: " + nextFireTime.Value.LocalDateTime.ToString());
                    }

                    DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
                    if (previousFireTime.HasValue)
                    {
                        lbTrigger.Items.Add("previousFireTime: " + previousFireTime.Value.LocalDateTime.ToString());
                    }

                    if (trigger.GetType().Name == "CronTriggerImpl")
                    {
                        ICronTrigger ict = (ICronTrigger)trigger;
                        lbTrigger.Items.Add("CronExpressionString: " + ict.CronExpressionString);
                        txtCronExpression.Text = ict.CronExpressionString;
                    }

                    if (ts == TriggerState.Paused)
                    {
                        btnEnableJob.Enabled = true;
                    }
                    else
                    {
                        btnPauseJob.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                _scheduler.Shutdown();
            }
        }

        private void lbJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                lbJobs.Enabled = false;

                JobKey jk = (JobKey)lbJobs.SelectedItem;

                GetJobTriggers(jk);

                lbJobs.Enabled = true;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnTesting_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                txtCronExpression.BackColor = txtColor;

                JobKey jk = (JobKey)lbJobs.SelectedItem;
                ITrigger trig = (ITrigger)lbTrigger.Items[0];

                UpdateJobCronTrigger(jk, trig.Key.Name, txtCronExpression.Text);

                GetJobTriggers(jk);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnNewJob_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                AddNewInterfaceJob("My New Job " + DateTime.Now.ToString("yyyyMMdd-HHmmss")
                        , "MyAssembly"
                        , "MyClass"
                        , @"C:\MyAssemblyPath\"
                        , "0 30 * ? * *");
            }
            catch (Exception ex)
            {
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void AddNewInterfaceJob(string jobName, string assemblyName, string className, string assemblyPath
                , string cronTriggerExpression)
        {
            try
            {
                _scheduler = new StdSchedulerFactory().GetScheduler();

                JobDataMap jobData = new JobDataMap();
                jobData.Add("JobName", jobName);
                jobData.Add("AssemblyName", assemblyName);
                jobData.Add("ClassName", className);
                jobData.Add("AssemblyPath", assemblyPath);

                IJobDetail jobDetail = JobBuilder.Create()
                    .StoreDurably()
                    .WithIdentity(jobName)
                    .SetJobData(jobData)
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(jobName + " Trigger")
                        .WithCronSchedule(cronTriggerExpression)
                        .StartNow()
                        .Build();

                Quartz.Collection.HashSet<ITrigger> triggers = new Quartz.Collection.HashSet<ITrigger>();
                triggers.Add(trigger);

                _scheduler.ScheduleJob(jobDetail, triggers, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                _scheduler.Shutdown();
            }
        }

        private void LoadJobs()
        {
            try
            {
                _scheduler = new StdSchedulerFactory().GetScheduler();

                lbJobs.Items.Clear();

                IList<string> jobGroups = _scheduler.GetJobGroupNames();
                IList<string> triggerGroups = _scheduler.GetTriggerGroupNames();
                // List<Task> jobGroups = _scheduler.GetJobGroupNames();

                foreach (string group in jobGroups)
                {
                    Debug.WriteLine("***");
                    Debug.WriteLine("***group: " + group);

                    var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);

                    Debug.WriteLine("***groupMatcher.ToString(): " + groupMatcher.ToString());

                    var jobKeys = _scheduler.GetJobKeys(groupMatcher);
                    foreach (var jobKey in jobKeys)
                    {
                        IJobDetail jd = _scheduler.GetJobDetail(jobKey);
                        JobDataMap jdm = jd.JobDataMap;
                        IList<string> il = jdm.GetKeys();


                        //string assemblyFullName = Path.Combine(new Uri("assemblyPath").LocalPath, "assemblyName");
                        //Assembly jobAssembly = Assembly.LoadFrom(assemblyFullName);
                        //Guid loggingId = Guid.NewGuid();
                        //string className = "";
                        //Type classType = jobAssembly.GetType(className);
                        //var jobClass = (IJob)Activator.CreateInstance(classType, loggingId.ToString());

                        //((IJob)jobClass).ExecuteJob(jobId);

                        foreach (string item in il)
                        {
                            Debug.WriteLine("******" + item.ToString());
                        }

                        lbJobs.Items.Add(jobKey);

                        Debug.WriteLine("******");
                        Debug.WriteLine("******jobKey.Group: " + jobKey.Group);
                        Debug.WriteLine("******jobKey.Name: " + jobKey.Name);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                _scheduler.Shutdown();
            }
        }

        private Dictionary<string, string> parseConnectionString(string connString)
        {
            Dictionary<string, string> connStringParts = connString.Split(';')
                    .Select(t => t.Split(new char[] { '=' }, 2))
                    .ToDictionary(t => t[0].Trim(), t => t[1].Trim(), StringComparer.InvariantCultureIgnoreCase);

            return connStringParts;
        }

        private void UpdateJobCronTrigger(JobKey jobKey, string oldTriggerName, string newCronExpression)
        {
            try
            {

                _scheduler = new StdSchedulerFactory().GetScheduler();

                var existingTriggers = _scheduler.GetTriggersOfJob(jobKey);
                foreach (ITrigger existingTrigger in existingTriggers)
                {
                    if ((existingTrigger is ICronTrigger) && (existingTrigger.Key.Name == oldTriggerName))
                    {

                        ITrigger newTrigger = TriggerBuilder.Create()
                                .WithIdentity(jobKey.Name + " Trigger")
                                .WithCronSchedule(newCronExpression)
                                .StartNow()
                                .Build();

                        _scheduler.RescheduleJob(existingTrigger.Key, newTrigger);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                _scheduler.Shutdown();
            }
        }

        private void btnPauseJob_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                _scheduler = new StdSchedulerFactory().GetScheduler();

                JobKey jk = (JobKey)lbJobs.SelectedItem;
                _scheduler.PauseJob(jk);

                GetJobTriggers(jk);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnEnableJob_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                _scheduler = new StdSchedulerFactory().GetScheduler();

                JobKey jk = (JobKey)lbJobs.SelectedItem;
                _scheduler.ResumeJob(jk);

                GetJobTriggers(jk);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        private void txtCronExpression_Validated(object sender, System.EventArgs e)
        {
            if (!CronExpression.IsValidExpression(txtCronExpression.Text))
            {
                cronErrorProvider.SetError(this.txtCronExpression, "Invalid Cron Expression, see URL");
            }
            else
            {
                cronErrorProvider.Clear();
            }
        }
    }
}
