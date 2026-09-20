using DocumentFormat.OpenXml.Wordprocessing;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.AutomatedReports;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS.ReSendNotices
{
    public class ReSendNoticesInterface : BaseJob, IJob
    {
        public ReSendNoticesInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "F956B20F-E61F-43DD-AE2F-4322D4A2E75D";

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse(appID));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();

            if (jobGuid.Equals(appID))
            {
                InitiateReSendNoticesProcess();
            }
        }

        public void InitiateReSendNoticesProcess()
        {
            string logMsg = String.Format(CON.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            try
            {
                ReSendNoticesHelper.CreateLogEntry("InitiateReSendNoticesProcess", "Process Started", CON.AutomatedReports.LogPriorityInfo);
                string directoryPath = AppSettings.Get("ReSendNoticesProcessFolder");
                string archivePath = directoryPath + @"\Archive";
                string templatesPath = directoryPath + @"\Templates";


                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }
                if (!System.IO.Directory.Exists(archivePath))
                {
                    System.IO.Directory.CreateDirectory(archivePath);
                }
                if (!System.IO.Directory.Exists(templatesPath))
                {
                    System.IO.Directory.CreateDirectory(templatesPath);
                }
                int ID = 0;
                int REG_ID = 0;
                string subject = string.Empty;
                string templateFile = string.Empty;
                string templateName = string.Empty;
                string recipients = string.Empty;
                string paper = string.Empty;
                bool paperSuccess = false;
                bool emailSuccess = false;

                // Send Email to Providers
                try
                {
                    DataTable ToProcessNotices = ReSendNoticesHelper.getAllToProcessReSendNotices();
                    
                    foreach (DataRow row in ToProcessNotices.Rows)
                    {
                        if (ToProcessNotices.Columns.Contains("ID"))
                        {
                            ID = Convert.ToInt32(row["ID"].ToString());
                        }
                        if (ToProcessNotices.Columns.Contains("REG_ID"))
                        {
                            REG_ID = Convert.ToInt32(row["REG_ID"].ToString());
                        }
                        if (ToProcessNotices.Columns.Contains("NOTICE_TYPE_SUBJECT"))
                        {
                            subject = row["NOTICE_TYPE_SUBJECT"].ToString();
                        }
                        if (ToProcessNotices.Columns.Contains("NOTICE_TYPE_TEMPLATE_FILE"))
                        {
                            templateFile = row["NOTICE_TYPE_TEMPLATE_FILE"].ToString();
                        }
                        if (ToProcessNotices.Columns.Contains("NOTICE_TYPE_TEMPLATE_NAME"))
                        {
                            templateName = row["NOTICE_TYPE_TEMPLATE_NAME"].ToString();
                        }
                        if (ToProcessNotices.Columns.Contains("PAPER_NOTICE"))
                        {
                            paper = row["PAPER_NOTICE"].ToString();
                        }
                        recipients = ReSendNoticesHelper.GetRecipients(CON.SendToTypeID.Provider, REG_ID);

                        if (paper.Equals("Yes"))
                        {
                            PaperNotification notify = new PaperNotification(subject, new Guid(CON.ReSendNotices.ReSendNoticesAppID));
                            paperSuccess = notify.SendWorkFlowEngineNotification(REG_ID.ToString(), templateName);
                        }

                        if (!string.IsNullOrEmpty(recipients))
                        {
                            EMailNotification enotify = new EMailNotification(templateName, subject, recipients, new Guid(CON.ReSendNotices.ReSendNoticesAppID));
                            emailSuccess = enotify.SendWorkFlowEngineNotification(REG_ID.ToString(), templateName);
                            ReSendNoticesHelper.UpdateReSendEmailNoticesSentDate(ID, DateTime.Now, CON.ReSendNotices.ReSendNoticesUpdate);
                        }

                        if (paperSuccess)
                        {
                            ReSendNoticesHelper.UpdateReSendEmailNoticesStatusMessage(ID, CON.ReSendNotices.PaperNoticeSuccess, CON.ReSendNotices.PaperNoticeSuccess, CON.ReSendNotices.ReSendNoticesUpdate);
                        }
                        else 
                        {
                            ReSendNoticesHelper.UpdateReSendEmailNoticesStatusMessage(ID, CON.ReSendNotices.PaperNoticeFailed, CON.ReSendNotices.PaperNoticeFailed, CON.ReSendNotices.ReSendNoticesUpdate);
                        }

                        if (emailSuccess)
                        {
                            ReSendNoticesHelper.UpdateReSendEmailNoticesStatusMessage(ID, CON.ReSendNotices.EmailNoticeSuccess, CON.ReSendNotices.EmailNoticeSuccess, CON.ReSendNotices.ReSendNoticesUpdate);
                        }
                        else
                        {
                            ReSendNoticesHelper.UpdateReSendEmailNoticesStatusMessage(ID, CON.ReSendNotices.EmailNoticeFailed, CON.ReSendNotices.EmailNoticeFailed, CON.ReSendNotices.ReSendNoticesUpdate);
                        }
                    }

                }
                catch (Exception ex)
                {
                    ReSendNoticesHelper.UpdateReSendEmailNoticesStatusMessage(ID, CON.ReSendNotices.Failed, ex.StackTrace.ToString(), CON.ReSendNotices.ReSendNoticesUpdate);
                    ReSendNoticesHelper.CreateLogEntry("InitiateReSendNoticesProcess", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
                }

                ReSendNoticesHelper.CreateLogEntry("InitiateReSendNoticesProcess", "Process Completed", CON.AutomatedReports.LogPriorityInfo);
            }
            catch (Exception ex)
            {
                ReSendNoticesHelper.CreateLogEntry("InitiateReSendNoticesProcess", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
            }
        }
    }
}
