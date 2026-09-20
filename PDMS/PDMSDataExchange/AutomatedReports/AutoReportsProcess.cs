using ClosedXML.Report;
using FileHelpers.Events;
using MathNet.Numerics.Distributions;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using CON = MAXIMUS.Core.Libraries.Constants;
using DataTable = System.Data.DataTable;

namespace MAXIMUS.DataExchange.PDMS.AutomatedReports
{
    public class AutoReportsProcess : BaseJob, IJob
    {

        public AutoReportsProcess(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "D1AFF5D8-D50A-4E3E-B4F3-9EBD0C290215";

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse(appID));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();

            if (jobGuid.Equals(appID))
            {
                InitiateAutomatedReportsProcess();
            }
        }

        public void InitiateAutomatedReportsProcess()
        {
            string logMsg = String.Format(CON.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            try
            {
                AutoReportsHelper.CreateLogEntry("InitiateAutomatedReportsProcess", "Process Started", CON.AutomatedReports.LogPriorityInfo);
                DataTable dtScheduledReports = AutoReportsHelper.getScheduledReports(CON.AutomatedReports.ToProcess);
                string reportID = string.Empty;
                int reportSchID = 0;

                foreach (DataRow dr in dtScheduledReports.Rows)
                {
                    reportID = AutoReportsHelper.GetValue(dr, "REPORT_ID");
                    reportSchID = Convert.ToInt32(AutoReportsHelper.GetValue(dr, "ID"));
                    switch (reportID.ToUpper())
                    {
                        case CON.AutomatedReports.AutomationReportEnrollment:
                            AutoReportsHelper.AutomationReportEnrollment(dr, reportSchID);
                            break;
                        case CON.AutomatedReports.AutomationReportAttachment:
                            AutoReportsHelper.AutomationReportAttachment(dr, reportSchID);
                            break;
                        case CON.AutomatedReports.AutomationReportWSTransactions:
                            AutoReportsHelper.AutomationReportWSTransactions(dr, reportSchID);
                            break;
                        case CON.AutomatedReports.AutomationReportUploadAttachment:
                            AutoReportsHelper.AutomationReportUploadAttachment(dr, reportSchID);
                            break;
                        case CON.AutomatedReports.AutomationReportDailyUploadAttachment:
                            AutoReportsHelper.AutomationReportDailyUploadAttachment(dr, reportSchID);
                            break;
                    }


                    
                }
                AutoReportsHelper.CreateLogEntry("InitiateAutomatedReportsProcess", "Process Completed", CON.AutomatedReports.LogPriorityInfo);
            }
            catch (Exception ex)
            {
                AutoReportsHelper.CreateLogEntry("InitiateAutomatedReports", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
            }
        }
        
    }
}
