using ClosedXML.Report;
using DocumentFormat.OpenXml.Wordprocessing;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Net.Mail;
using System.Reflection;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS.AutomatedReports
{
    public class AutoReportsEmail : BaseJob, IJob
    {

        public AutoReportsEmail(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "32C1CB30-6B17-47D6-B37D-F956F1EEC5D3";

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse(appID));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();

            if (jobGuid.Equals(appID))
            {
                InitiateAutoReportsEmail();
            }
        }

        public void InitiateAutoReportsEmail()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            try
            {
                AutoReportsHelper.CreateLogEntry("InitiateAutoReportsEmail", "Process Started", CON.AutomatedReports.LogPriorityInfo);
                DataTable dtReportsToEmail = AutoReportsHelper.getScheduledReports(CON.AutomatedReports.ToEmail);
                foreach (DataRow dr in dtReportsToEmail.Rows)
                {
                    int reportID = Convert.ToInt32(dr["ID"]);
                    try
                    {
                        string reportSubject = AutoReportsHelper.GetValue(dr, "REPORT_SUBJECT");
                        string reportFrom = AutoReportsHelper.GetValue(dr, "REPORT_FROM");
                        string reportTo = AutoReportsHelper.GetValue(dr, "REPORT_TO");
                        string reportBcc = AutoReportsHelper.GetValue(dr, "REPORT_BCC");
                        string reportStoredProc = AutoReportsHelper.GetValue(dr, "REPORT_SQL");
                        string reportBody = AutoReportsHelper.GetValue(dr, "REPORT_BODY");
                        int reportAttach = 0;
                        if (!AutoReportsHelper.GetValue(dr, "ATTACHMENT_ONBASEID").Equals(""))
                        {
                            reportAttach = Convert.ToInt32(AutoReportsHelper.GetValue(dr, "ATTACHMENT_ONBASEID"));
                        }
                        string reportUqName = AutoReportsHelper.GetValue(dr, "REPORT_UNIQUE_NAME");

                        bool emailed = AutoReportsHelper.SendEmail(reportID, reportSubject, reportFrom, reportTo, reportBody, reportAttach, reportUqName, reportBcc);
                        if (emailed)
                        {
                            AutoReportsHelper.updateScheduledReportStatus(reportID, CON.AutomatedReports.Completed, CON.AutomatedReports.AutomatedReportsEmailSuccess);
                        }
                        else
                        {
                            AutoReportsHelper.updateScheduledReportStatus(reportID, CON.AutomatedReports.Failed, CON.AutomatedReports.AutomatedReportsEmailFailed);
                        }
                    }
                    catch (Exception ex)
                    {
                        AutoReportsHelper.updateScheduledReportStatus(reportID, CON.AutomatedReports.Failed, ex.Message + " " + ex.StackTrace);
                    }
                }
                AutoReportsHelper.CreateLogEntry("InitiateAutoReportsEmail", "Process Completed", CON.AutomatedReports.LogPriorityInfo);
            }
            catch (Exception ex)
            {
                AutoReportsHelper.CreateLogEntry("InitiateAutoReportsEmail", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
            }
        }

    }


}
