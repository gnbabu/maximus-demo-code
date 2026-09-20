using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    public class PasswordExpiryNotifictions : BaseJob, IJob
    {
        public PasswordExpiryNotifictions(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private Logging log = null;

        override public void ExecuteJob()
        {

            this.ExecuteJob(Guid.Parse("B387D29C-8340-41B0-B043-F6CBCF28965C"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            //get the usernames that need to send out emails about passowrd expiry.
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                if (jobGuid == "B387D29C-8340-41B0-B043-F6CBCF28965C")
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectPasswordExpiryNoticeUsers", parameters, "EmailRecipients");
                    int TotalRowsCount = ds.Tables[0].Rows.Count;
                    log.CreateLogEntry(String.Format("Total accounts that need to send password expiry notices {0}", TotalRowsCount));
                    if (ObjectControllerHelper.HasRows(ds))
                    {
                       
                        string username = string.Empty;
                        string userAccountEmail = string.Empty;
                        Guid userId = new Guid();
                        int noticesSent = 0;
                        int noticesNotSent = 0;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            username = ObjectControllerHelper.GetString("username", row);
                            userAccountEmail = ObjectControllerHelper.GetString("Email", row);
                            userId = (Guid)row["userid"];
                            bool noticeSent = false;
                            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userAccountEmail))
                            {
                                noticeSent = SendPasswordExpiryNotice(username, userAccountEmail, userId);
                            }
                            if (noticeSent)
                            {
                                noticesSent++;
                            }
                            else
                            {
                                noticesNotSent++;
                            }
                        }
                        log.CreateLogEntry(String.Format("Password Expiry Notifications, Sent count:{0}, Not sent count {1}", noticesSent, noticesNotSent));
                    }
                    else
                    {
                        log.CreateLogEntry(String.Format("No accounts exists that need to send password expiry notices {0}", ds.Tables[0].Rows.Count));
                    }

                 //Inactivate users who's password got expired and has enabled the rule.
                    parameters.Clear();
                    DataSet dsInactivateUsers = DataAccess.ExecuteStoredProcedure("usp_SelectPasswordExpiredUsersToInactivate", parameters, "EmailRecipients");
                    log.CreateLogEntry(String.Format("Total accounts that need to get inactivated {0}", dsInactivateUsers.Tables[0].Rows.Count));
                     if (ObjectControllerHelper.HasRows(dsInactivateUsers))
                     {
                         string inActivateUsername = string.Empty;
                         foreach (DataRow dr in dsInactivateUsers.Tables[0].Rows)
                         {
                             inActivateUsername = ObjectControllerHelper.GetString("username", dr);

                             if(!string.IsNullOrEmpty(inActivateUsername))
                             {

                                bool bUpdated= InactivatePasswordExpiredUser(inActivateUsername);
                                if(bUpdated)
                                 log.CreateLogEntry(String.Format("Inactivated account due to inactivity:{0}", inActivateUsername));
                             }

                         }


                     }



                }
            }
            catch (Exception ex)
            {
                //create exception log entry
                log.CreateLogEntry(String.Format("Exeption in Password expiry notification source:{0},error: {1}", ex.Source,ex.Message));
            }
        }
        
       
        public bool SendPasswordExpiryNotice(string username,string email,Guid userId)
        {
            bool noticeSent = false;
            string subject = Constants.PasswordExpiryNotices.subject;

            string templateFile = Constants.PasswordExpiryNotices.emailTemplateName;

            try
            {

                Notification n = new Notification();
                Dictionary<string, object> fields = new Dictionary<string, object>();
             
                fields.Add("USERNAME", username);
                fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                string body = string.Empty;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                
                string pwdTemplateContent = null;
                using (StreamReader reader = new StreamReader(templateActualPath + templateFile))
                {
                    pwdTemplateContent = reader.ReadToEnd();
                }
                body = n.ParseEmailBody(pwdTemplateContent, fields);
                EMailNotification notify = new EMailNotification(body, subject, email);
                body = notify.SendNotification(templateActualPath + templateFile, fields, true);
                
                //Create Communication event
                int partyId=Notification.GetAdminPartyId();
                notify.CreateCommunicationEvent(partyId,partyId, fields, userId, string.Empty, templateFile);
                noticeSent = true;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return noticeSent;
        }


        private bool InactivatePasswordExpiredUser(string username)
        {
            bool retval = true;
            try
            {
                    List<SqlParameter> parameters = new List<SqlParameter>();               
                    parameters.Add(SqlParms.CreateParameter("user_name", DbType.String, username, false));
                    DataAccess.ExecuteStoredProcedure("usp_UpdateUserToInactivate", parameters);
                
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Exeption in InactivatePasswordExpiredUser source:{0},error: {1}", ex.Source, ex.Message));
                retval = false;
            }

            return retval;
        }


    }
}
