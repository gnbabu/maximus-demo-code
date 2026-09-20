using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    public class EFTUpdateNotification : BaseJob, IJob
    {
        private Logging log = null;

        public EFTUpdateNotification(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "070AE469-881F-416B-89AE-606537050F80";

        override public void ExecuteJob()
        {

            this.ExecuteJob(Guid.Parse(appID));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    ProcessEFTNotification();
                    break;
            }
        }

        private void ProcessEFTNotification()
        {
            // create log object
            string logMsg = string.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(appID), logMsg);
            try
            {
                // create local path if it does not exist
                string displayomr = string.Empty;
                string pdmsurl = string.Empty;
                DataTable dt = GetDataForProcess();


                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        bool noticeSent = false;
                        int regId = ObjectControllerHelper.GetInt("REG_ID", row);
                        DataTable dtProvider = GetProviderAdministratorData(regId);
                        if (dtProvider != null && dtProvider.Rows.Count > 0)
                        {
                            var providerRow = dtProvider.Rows[0];
                            string email = ObjectControllerHelper.GetString("Email", providerRow);
                            noticeSent = SendOutEmails(email, regId);
                            if (noticeSent)
                            {
                                UpdateETFNotifiedStatus(regId); // status will be 2
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);
        }
        private DataTable GetProviderAdministratorData(int regId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("regId", DbType.Int32, regId, false));
            DataSet ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectAllEFTUPDATEProviderAdministrator", parameters, "providers");
            return ds.Tables[0];
        }

        private void UpdateETFNotifiedStatus(int regId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("regId", DbType.Int32, regId, false));

            DataAccess.ExecuteScalar("usp_UPDATE_REG_EFT_NOTIFICATION_STATUS", parameters);
        }
        private DataTable GetDataForProcess()
        {
            DataSet ds = new DataSet();

            ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER_EFT_UPDATE_TEMPLATE");
            DataTable dt = ds.Tables[0];
            return dt;
        }
        
        private bool SendOutEmails(string recipient, int regID)
        {
            bool noticeSent = false;
            string subject = Constants.EFTNotificationType.subject;
            string Template_Name = "EMAIL_TEMPLATE_EFT_UPDATE_NOTICE";

            try
            {
                EMailNotification notify = new EMailNotification(Template_Name, subject, recipient, this.ThreadId);
                if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                {
                    noticeSent = true;
                    if (log != null)
                    {
                        log.CreateLogEntry("Sent EFT email Template_Name - " + Template_Name + "for Reg ID: " + regID ,
                            Logging.LogPriority.Error);
                    }
                }

                else
                    log.CreateLogEntry("Reg Id: " + regID + " - problems sending EFT Update email",
                    Logging.LogPriority.Error);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Error while sending out Emails {0}", ex.ToString()));
            }
            return noticeSent;
        }
    }
}