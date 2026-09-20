using DocumentFormat.OpenXml.Bibliography;
using MAXIMUS.Core.Libraries;
using Quartz;
using System;
using System.Data;
using System.Reflection;
using CON = MAXIMUS.Core.Libraries.Constants;
using IJob = MAXIMUS.Core.Libraries.IJob;

namespace MAXIMUS.DataExchange.PDMS.AutomatedReports
{
    internal class AutoReportsScheduler : BaseJob, IJob
    {
        public AutoReportsScheduler(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "017A8737-D07B-4DC7-92F4-DD1695510A27";

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse(appID));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();

            if (jobGuid.Equals(appID))
            {
                InitiateAutoReportsScheduler();
            }
        }

        public void InitiateAutoReportsScheduler()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg); 
    
            try
            {
                AutoReportsHelper.CreateLogEntry("InitiateAutoReportsScheduler", "Process Started", CON.AutomatedReports.LogPriorityInfo);
                int nextRuns = 10;
                int reportCount = 0;
                DateTime est = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

                DateTimeOffset? nextFire = null;
                DateTime nextFireDT;
                DataTable dtScheduledAllReports = AutoReportsHelper.getAllAutoReports();
                foreach (DataRow dr in dtScheduledAllReports.Rows)
                {
                    Guid reportID = new Guid(AutoReportsHelper.GetValue(dr, "REPORT_ID"));
                    try
                    {
                        string cronExp = AutoReportsHelper.GetValue(dr, "CRON_EXP");
                        reportCount = Convert.ToInt32(AutoReportsHelper.GetValue(dr, "REPORT_COUNT"));
                        if (!AutoReportsHelper.GetValue(dr, "TOPROCESS_DATE").Equals(""))
                        {
                            DateTime.TryParse(AutoReportsHelper.GetValue(dr, "TOPROCESS_DATE"), out est);
                        }
                        est = DateTime.SpecifyKind(est, DateTimeKind.Local);
                        var cron = new Quartz.CronExpression(cronExp);
                        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                        for (int i = 0; i < nextRuns - reportCount; i++)
                        {
                            nextFire = cron.GetNextValidTimeAfter(est);
                            if (timeZone.IsDaylightSavingTime(nextFire.GetValueOrDefault()))
                            {
                                nextFire = nextFire.Value.DateTime.AddHours(-4);
                            }
                            else
                            {
                                nextFire = nextFire.Value.DateTime.AddHours(-5);
                            }
                            est = DateTime.Parse(nextFire.ToString());
                            nextFireDT = nextFire.HasValue ? nextFire.Value.DateTime : DateTime.MaxValue;
                            AutoReportsHelper.insertScheduledReport(reportID, CON.AutomatedReports.ToProcess, "Report Scheduled for date " + nextFire.ToString(),
                                nextFireDT);
                        }
                    }
                    catch (Exception ex)
                    {
                        AutoReportsHelper.CreateLogEntry("InitiateAutoReportsScheduler", string.Format("ReportID : {0} Exception {1} {2}", reportID.ToString(), ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                    }
                }
                AutoReportsHelper.CreateLogEntry("InitiateAutoReportsScheduler", "Process Completed", CON.AutomatedReports.LogPriorityInfo);
            }
            catch (Exception ex)
            {
                AutoReportsHelper.CreateLogEntry("InitiateAutoReportsScheduler", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
            }
        }
    }
}
