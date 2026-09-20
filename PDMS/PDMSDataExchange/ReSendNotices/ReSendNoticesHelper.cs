using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using static NPOI.HSSF.UserModel.HeaderFooter;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS.ReSendNotices
{
    public static class ReSendNoticesHelper
    {
        public static void CreateLogEntry(string processName, string message, int priority)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ThreadId", DbType.Guid, new Guid(CON.ReSendNotices.ReSendNoticesAppID), true));
                parameters.Add(SqlParms.CreateParameter("Message", DbType.String, message, true));
                parameters.Add(SqlParms.CreateParameter("ProcessName", DbType.String, processName, true));
                parameters.Add(SqlParms.CreateParameter("Machine", DbType.String, Environment.MachineName, true));
                parameters.Add(SqlParms.CreateParameter("User", DbType.String, Environment.UserDomainName + @"\" + Environment.UserName, true));
                parameters.Add(SqlParms.CreateParameter("Priority", DbType.Int32, priority, true));

                DataAccess.ExecuteScalar("usp_CreateLogEntryReSendEmailNotices", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Guid(CON.ReSendNotices.ReSendNoticesAppID), ex);
            }
        }

        public static void UpdateReSendEmailNoticesStatusMessage(int id, string status, string msg, int ops)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, id, true));
                parameters.Add(SqlParms.CreateParameter("STATUS", DbType.String, status, true));
                parameters.Add(SqlParms.CreateParameter("MESSAGE", DbType.String, msg, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.ReSendNotices.ReSendNoticesAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, ops, true));

                DataAccess.ExecuteScalar("usp_SelectInsertUpdateDeleteSTG_ReSendEmailNotices", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateReSendEmailNoticesStatus", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
            }
        }

        public static void UpdateReSendEmailNoticesSentDate(int id, DateTime sentDate, int ops)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, id, true));
                parameters.Add(SqlParms.CreateParameter("SENT_DATE", DbType.DateTime, sentDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.ReSendNotices.ReSendNoticesAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, ops, true));

                DataAccess.ExecuteScalar("usp_SelectInsertUpdateDeleteSTG_ReSendEmailNotices", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateReSendEmailNoticesStatus", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
            }
        }

        public static void InsertSMS(string storedProc, List<SqlParameter> param)
        {
            try
            {
                DataAccess.ExecuteScalar(storedProc, param);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateReSendEmailNoticesStatus", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
            }
        }

        public static string GetKeyValueString(Dictionary<string, object> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, object> entry in dict)
            {
                sb.Append(entry.Key + ":" + entry.Value.ToString() + ",");
            }

            string keyValue = sb.ToString();

            if (!string.IsNullOrEmpty(keyValue))
            {
                keyValue = keyValue.Remove(keyValue.Length - 1); //remove the final comma
            }

            return keyValue;
        }

        public static void InsertCOMMUNICATIONEVENT_AND_EMAIL(string storedProc, List<SqlParameter> param)
        {
            try
            {
                DataAccess.ExecuteScalar(storedProc, param);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateReSendEmailNoticesStatus", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
            }
        }


        public static DataTable getAllToProcessReSendNotices()
        {
            DataTable dt = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetAllToProcessReSendNotices", parameters, "getAllToProcessReSendNotices");
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                CreateLogEntry("getAllLTCProvideFileDetails", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
            return dt;
        }

        public static DataSet SelectREG_PROVIDER(int regId)
        {
            DataSet ds = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parameters, "SelectREG_PROVIDER");
            }
            catch (Exception ex)
            {
                CreateLogEntry("SelectREG_PROVIDER", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
            return ds;
        }

        public static DataSet SelectREG_SERVICE_LOCATION(int regId)
        {
            DataSet ds = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SERVICE_LOCATION", parameters, "SelectREG_SERVICE_LOCATION");
            }
            catch (Exception ex)
            {
                CreateLogEntry("SelectREG_SERVICE_LOCATION", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
            return ds;
        }

        public static DataSet SelectProviderAddressInfo(int regId)
        {
            DataSet ds = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, 8, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectProviderAddressInfo", parameters, "SelectREG_PROVIDER");
            }
            catch (Exception ex)
            {
                CreateLogEntry("SelectREG_PROVIDER", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
            return ds;
        }

        public static string GetRecipients(int sendToTypeID, int regId)
        {
            StringBuilder addrList = new StringBuilder();
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SendToTypeID", DbType.Int32, sendToTypeID, false));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters, "EmailRecipients");
                if (Methods.HasRows(ds))
                {
                    addrList = Methods.AddEmail(ds.Tables[0], "EmailAddress");
                }
            }
            catch (Exception ex)
            {
                CreateLogEntry("GetRecipients", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
                return null;
            }

            return addrList.ToString();
        }

        public static int SelectCommunicationEventTypes(string eventType)
        {
            int commEventTypeID = 0;
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "Print", false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("sp_SelectCommunicationEventTypes", parameters, "SelectCommunicationEventTypes");
                DataTable dt = null;

                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (dt.Columns.Contains("COMMUNICATION_EVENT_TYPE_ID"))
                    {
                        commEventTypeID = Convert.ToInt32(row["COMMUNICATION_EVENT_TYPE_ID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                CreateLogEntry("SelectCommunicationEventTypes", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
                return 0;
            }

            return commEventTypeID;
        }

        
        public static void InsertCOMMUNICATIONEVENT_AND_MAIL(int id, string subject, string body, string templateName, string fields, int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, id, true));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, true));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, true));
                parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, true));
                parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, templateName, true));
                parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, fields, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, new Guid(CON.ReSendNotices.ReSendNoticesAppID), true));;
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.ReSendNotices.ReSendNoticesAppID), true));
                DataAccess.ExecuteScalar("sp_insertCOMMUNICATIONEVENT_AND_MAIL", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateReSendEmailNoticesStatus", String.Format("ReSendNotices Exception: {0} {1}", ex.Message, ex.StackTrace), CON.ReSendNotices.LogPriorityError);
            }
        }
    }
}
