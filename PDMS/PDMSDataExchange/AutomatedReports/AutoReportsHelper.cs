using CON = MAXIMUS.Core.Libraries.Constants;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using MAXIMUS.Core.Libraries;
using System.Net.Mail;
using System.Reflection;
using Corp.Core.Libraries;
using System.Dynamic;
using System.Linq;
using ClosedXML.Report;
using System.IO;
using System.Collections;
using System.Text;
using Amazon.Runtime.Internal.Transform;

namespace MAXIMUS.DataExchange.PDMS.AutomatedReports
{
    public static class AutoReportsHelper
    {

        public static void CreateLogEntry(string processName, string message, int priority)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ThreadId", DbType.Guid, new Guid(CON.AutomatedReports.AutomatedReportsAppID), true));
                parameters.Add(SqlParms.CreateParameter("Message", DbType.String, message, true));
                parameters.Add(SqlParms.CreateParameter("ProcessName", DbType.String, processName, true));
                parameters.Add(SqlParms.CreateParameter("Machine", DbType.String, Environment.MachineName, true));
                parameters.Add(SqlParms.CreateParameter("User", DbType.String, Environment.UserDomainName + @"\" + Environment.UserName, true));
                parameters.Add(SqlParms.CreateParameter("Priority", DbType.Int32, priority, true));

                DataAccess.ExecuteScalar("usp_InsertLogAutomatedReports", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Guid(CON.AutomatedReports.AutomatedReportsAppID), ex);
            }
        }

        public static DataTable getScheduledReports(string status)
        {
            try
            {
                DataTable dt = null;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REPORT_STATUS", DbType.String, status, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetScheduledAutoReports", parameters, "GetScheduledAutoReports");
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
                return dt;
            }
            catch (Exception ex)
            {
                CreateLogEntry("getScheduledReports", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                return null;
            }
        }

        public static DataTable getReportsToEmail()
        {
            try
            {
                DataTable dt = null;
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetReportsToEmail", parameters, "GetReportsToEmail");
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
                return dt;
            }
            catch (Exception ex)
            {
                CreateLogEntry("getReportsToEmail", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                return null;
            }
        }

        private static string generateHTMLTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<table style=\"width: 100%; border-collapse: collapse; background-color: white; border: 1px solid #ddd;\">");

            // headers.
            sb.Append("<tr>");

            foreach (DataColumn dc in dt.Columns)
            {
                sb.AppendFormat("<th style=\"padding: 10px; font-size: 14px; background-color: #4f81bd; color: white; text-align: left; border: 1px solid #ddd;\">{0}</th>", dc.ColumnName);
            }

            sb.AppendLine("</tr>");

            // data rows
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");

                foreach (DataColumn dc in dt.Columns)
                {
                    string cellValue = dr[dc] != null ? dr[dc].ToString() : "";
                    sb.AppendFormat("<td style=\"padding: 8px; border: 1px solid #ddd;\">{0}</td>", cellValue);
                }

                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");

            return sb.ToString();
        }


        public static string GetValue(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) ? row[column].ToString() : "";
        }

        public static int SendToCMS(int docID, byte[] fileBytes, string fileName)
        {
            OnBaseInterface onBaseInterface = new OnBaseInterface();
            int onBaseId = onBaseInterface.GetSubmitedFileID(docID, fileBytes, fileName);
            return onBaseId;
        }

        public static byte[] RetrieveFromCMS(string onBaseID)
        {
            OnBaseInterface onBaseInterface = new OnBaseInterface();
            return onBaseInterface.DownloadOnbaseFile(onBaseID, "N");
        }

        public static int SaveUploadedFile(int reportSchID, string fileName, string fileDescription)
        {
            int docID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, reportSchID, true));
                parameters.Add(SqlParms.CreateParameter("Name", DbType.String, fileName, true));
                parameters.Add(SqlParms.CreateParameter("Description", DbType.String, fileDescription, true));
                parameters.Add(SqlParms.CreateParameter("File_Name", DbType.String, fileName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, CON.AutomatedReports.AutomatedReportsProcessAppID, true));
                docID = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertUpdateDocumentDetails", parameters));
                return docID;
            }
            catch (Exception ex)
            {
                AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, ex.Message + " " + ex.StackTrace);
                return docID;
            }

        }

        public static bool SendEmail(int reportID, string reportSubject, string reportFrom, string reportTo, string reportBody, 
            int reportAttach, string reportAttchName, string reportBcc)
        {
            try
            {
                // generate an email, loop through the table
                string emailSubject = reportSubject;
                MailMessage emailMessage = new MailMessage();
                emailMessage.Body = reportBody;
                if (reportAttach > 0)
                {
                    try
                    {
                        Byte[] bt = RetrieveFromCMS(reportAttach.ToString());
                        MemoryStream ms = new MemoryStream(bt);
                        System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(ms, reportAttchName, "application/vnd.ms-excel");
                        emailMessage.Attachments.Add(att);
                    }
                    catch (Exception ex)
                    {
                        CreateLogEntry("SendEmail", String.Format("AutoReports Attachment Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                    }
                }
                bool sentEmail = SendMessage(emailMessage, emailSubject, reportFrom, reportTo, reportBcc);
                if (sentEmail)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                AutoReportsHelper.updateScheduledReportStatus(reportID, CON.AutomatedReports.Failed, ex.Message + " " + ex.StackTrace);
                return false;
            }
        }

        private static bool SendMessage(MailMessage emailMessage, string emailSubject, string from, string recipients, string bccList)
        {

            const char delimiter1 = ',';
            const char delimiter2 = ';';
            string testing = bool.TrueString.ToLower();

            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            try
            {
                // add the To email address(es)
                char delimiter = delimiter1;
                if (recipients.IndexOf(delimiter1) == -1) delimiter = delimiter2;
                string[] Addrs = recipients.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string adr in Addrs)
                {
                    emailMessage.To.Add(new MailAddress(adr));
                }

                // add the BCC email address(es)
                if (!string.IsNullOrWhiteSpace(bccList))
                {
                    delimiter = delimiter1;
                    if (bccList.IndexOf(delimiter1) == -1) delimiter = delimiter2;
                    string[] bccAddrs1 = bccList.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string adr1 in bccAddrs1)
                    {
                        emailMessage.Bcc.Add(new MailAddress(adr1));
                    }
                }

                string replyToEmail = AppSettings.Get("SmtpReplyTo", string.Empty);
                string emailfromAddress = from;

                if (!string.IsNullOrEmpty(replyToEmail)) emailMessage.ReplyToList.Add(replyToEmail);
                if (!string.IsNullOrEmpty(emailfromAddress)) emailMessage.From = new MailAddress(emailfromAddress);

                emailMessage.Subject = emailSubject;
                emailMessage.IsBodyHtml = true;

                SmtpClient smtpclient;
                smtpclient = new SmtpClient();

                smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (!string.IsNullOrEmpty(AppSettings.Get("SmtpClientPortNumber")))
                {
                    smtpclient.Port = (int)Methods.GetIntValue(AppSettings.Get("SmtpClientPortNumber"), false);
                }
                smtpclient.Host = AppSettings.Get("SmtpClientHost");
                smtpclient.UseDefaultCredentials = Convert.ToBoolean(AppSettings.Get("SmtpUseDefaultCredentials", bool.FalseString.ToLower()));
                smtpclient.EnableSsl = false;
                smtpclient.Send(emailMessage);
                return true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                AutoReportsHelper.CreateLogEntry("InitiateAutoReportsEmail", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                return false;
            }
            catch (SmtpException ex)
            {
                AutoReportsHelper.CreateLogEntry("InitiateAutoReportsEmail", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                return false;
            }
            catch (Exception ex)
            {
                AutoReportsHelper.CreateLogEntry("InitiateAutoReportsEmail", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                return false;
            }
        }

        public static DataTable getAllAutoReports()
        {
            try
            {
                DataTable dt = null;
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetAllAutoReports", parameters, "GetAllAutoReports");
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
                return dt;
            }
            catch (Exception ex)
            {
                CreateLogEntry("getScheduledReports", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                return null;
            }
        }

        public static DataSet getAutoReportsData(string storedProc)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "getAutoReportsData");
                return ds;
            }
            catch (Exception ex)
            {
                CreateLogEntry("getAutoReportsData", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                return null;
            }
        }

        public static DataSet getAutoReportsDataWithParams(string storedProc, int ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, ID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "getAutoReportsDataWithParams");
                return ds;
            }
            catch (Exception ex)
            {
                CreateLogEntry("getAutoReportsDataWithParams", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                return null;
            }
        }

        public static void updateScheduledReportBody(int ID, string reportBody)
        {
            try
            {
                List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                sqlParms2.Add(SqlParms.CreateParameter("ID", DbType.Int32, ID, false));
                sqlParms2.Add(SqlParms.CreateParameter("REPORT_BODY", DbType.String, reportBody, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateScheduledReportBody", sqlParms2);
            }
            catch (Exception ex)
            {
                CreateLogEntry("updateScheduledReportBody", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
            }
        }

        public static List<dynamic> ToDynamic(DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName) {
                        if (dr[column.ColumnName] == null || dr[column.ColumnName] == DBNull.Value)
                        {
                            pro.SetValue(obj, "", null);
                        }
                        else
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }


        public static void updateScheduledReportStatuswithonBase(int ID, string status, string msg, int onBaseID)
        {
            try
            {
                List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                sqlParms2.Add(SqlParms.CreateParameter("ID", DbType.Int32, ID, false));
                sqlParms2.Add(SqlParms.CreateParameter("Status", DbType.String, status, false));
                sqlParms2.Add(SqlParms.CreateParameter("Msg", DbType.String, msg, false));
                if (onBaseID == 0)
                {
                    sqlParms2.Add(SqlParms.CreateParameter("ONBASEID", DbType.Int32, null, false));
                }
                else
                {
                    sqlParms2.Add(SqlParms.CreateParameter("ONBASEID", DbType.Int32, onBaseID, false));
                }
                sqlParms2.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, CON.AutomatedReports.AutomatedReportsProcessAppID, false));
                sqlParms2.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateScheduledReportStatus", sqlParms2);
            }
            catch (Exception ex)
            {
                CreateLogEntry("updateScheduledReportStatus", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
            }
        }

        public static void updateScheduledReportStatus(int ID, string status, string msg)
        {
            try
            {
                List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                sqlParms2.Add(SqlParms.CreateParameter("ID", DbType.Int32, ID, false));
                sqlParms2.Add(SqlParms.CreateParameter("Status", DbType.String, status, false));
                sqlParms2.Add(SqlParms.CreateParameter("Msg", DbType.String, msg, false));
                sqlParms2.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, CON.AutomatedReports.AutomatedReportsProcessAppID, false));
                sqlParms2.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                if (status.Equals(CON.AutomatedReports.Completed))
                {
                    sqlParms2.Add(SqlParms.CreateParameter("EMAIL_SENT", DbType.Boolean, CON.AutomatedReports.AutomatedReportsEmailSent, false));
                    sqlParms2.Add(SqlParms.CreateParameter("EMAIL_SENT_DATE", DbType.DateTime, DateTime.Now, false));
                }

                DataAccess.ExecuteStoredProcedure("usp_UpdateScheduledReportStatus", sqlParms2);
            }
            catch (Exception ex)
            {
                CreateLogEntry("updateScheduledReportStatus", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
            }
        }


        public static void insertScheduledReport(Guid reportID, string reportStatus, string reportMsg, DateTime nextRun)
        {
            try
            {
                List<SqlParameter> sqlParms3 = new List<SqlParameter>();
                sqlParms3.Add(SqlParms.CreateParameter("REPORT_ID", DbType.Guid, reportID, false));
                sqlParms3.Add(SqlParms.CreateParameter("REPORT_STATUS", DbType.String, reportStatus, false));
                sqlParms3.Add(SqlParms.CreateParameter("REPORT_MESSAGE", DbType.String, reportMsg, false));
                sqlParms3.Add(SqlParms.CreateParameter("EMAIL_SENT", DbType.String, 0, false));
                sqlParms3.Add(SqlParms.CreateParameter("TOPROCESS_DATE", DbType.String, nextRun, false));
                sqlParms3.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, CON.AutomatedReports.AutomatedReportsSchedulerAppID, false));
                sqlParms3.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                sqlParms3.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, CON.AutomatedReports.AutomatedReportsSchedulerAppID, false));
                sqlParms3.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                DataAccess.ExecuteStoredProcedure("usp_InsertScheduledReport", sqlParms3);
            }
            catch (Exception ex)
            {
                CreateLogEntry("updateScheduledReportStatus", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
            }
        }

        public static void AutomationReportEnrollment(DataRow dr, int reportSchID)
        {
            try
            {
                string templateActualPath = AppSettings.Get("AutomatedReportsFSXPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                string reportTitle = AutoReportsHelper.GetValue(dr, "REPORT_TITLE");
                string reportName = AutoReportsHelper.GetValue(dr, "REPORT_NAME");
                string reportUser = AutoReportsHelper.GetValue(dr, "REPORT_USER");
                string reportDescp = AutoReportsHelper.GetValue(dr, "REPORT_DESCP");
                string reportUniqueName = reportName + "_" + reportSchID.ToString() + ".xlsx";
                string reportStoredProc = AutoReportsHelper.GetValue(dr, "REPORT_SQL");
                string reportTemplate = templateActualPath + @"Templates\" + AutoReportsHelper.GetValue(dr, "REPORT_TEMPLATE");
                string reportAttachment = templateActualPath + @"Templates\" + AutoReportsHelper.GetValue(dr, "REPORT_ATTACHMENT");
                bool isAttached = false;
                DateTime now = DateTime.Now;
                string formattedDate = now.ToString("D");
                DataSet ds = AutoReportsHelper.getAutoReportsData(reportStoredProc);
                if (ds != null && ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        try 
                        { 
                            //Email Attachment
                            DataTable dtAttachment = ds.Tables[1];
                            //var context = AutoReportsHelper.GetContext();
                            List<TransactionQueue> transactionQueues = new List<TransactionQueue>();
                            transactionQueues = AutoReportsHelper.ConvertDataTable<TransactionQueue>(dtAttachment);

                            TransactionQueueDetails td = new TransactionQueueDetails();
                            td.TransactionQueue = transactionQueues;

                            var xlsxTemplate = new XLTemplate(reportAttachment);
                            xlsxTemplate.AddVariable(td);
                            xlsxTemplate.Generate();
                            var stream = new MemoryStream();
                            xlsxTemplate.Workbook.SaveAs(stream);
                            byte[] bytes = stream.ToArray();

                            int docID = AutoReportsHelper.SaveUploadedFile(reportSchID, reportUniqueName, CON.AutomatedReports.AutomatedReportsProcessDocDescp);
                            if (docID > 0)
                            {
                                int onBaseID = AutoReportsHelper.SendToCMS(docID, bytes, reportUniqueName);

                                if (onBaseID > 0)
                                {
                                    AutoReportsHelper.updateScheduledReportStatuswithonBase(reportSchID, CON.AutomatedReports.ToEmail, CON.AutomatedReports.AutomatedReportsProcessAttachmentSuccess, onBaseID);
                                    isAttached = true;
                                }
                                else
                                {
                                    AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, CON.AutomatedReports.AutomatedReportsProcessonBaseFail);
                                }
                            }
                            else
                            {
                                AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, CON.AutomatedReports.AutomatedReportsProcessDocIDFail);
                            }
                        }
                        catch (Exception ex)
                        {
                            AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, ex.Message + " " + ex.StackTrace);
                        }
                    }
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    //Email Body
                    DataTable dtBody = ds.Tables[0];
                    fields.Clear();
                    fields.Add("REPORT_TITLE", reportTitle);
                    fields.Add("REPORT_DATE", formattedDate);
                    fields.Add("REPORT_USER", reportUser);
                    fields.Add("REPORT_DESCP", reportDescp);
                    if (dtBody.Rows.Count > 0)
                    {
                        fields.Add("REPORT_TABLE", generateHTMLTable(dtBody));
                    }
                    else
                    {
                        fields.Add("REPORT_TABLE", CON.AutomatedReports.AutomationReportSubBodyNote);
                    }
                    if (isAttached)
                    {
                        fields.Add("FINAL_NOTE", CON.AutomatedReports.AutomationReportSubAttachNote);
                    }
                    else
                    {
                        fields.Add("FINAL_NOTE", CON.AutomatedReports.AutomationReportSubFinalNote);
                    }

                    TemplateEvaluator template = new TemplateEvaluator();
                    template.Load(reportTemplate);
                    string body = template.Eval(TemplateFieldAccessors.DictionaryAccessor(fields));
                    AutoReportsHelper.updateScheduledReportBody(reportSchID, body);

                    AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.ToEmail, CON.AutomatedReports.AutomatedReportsProcessBodySuccess);
                }
            }
            catch (Exception ex)
            {
                AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, ex.Message + " " + ex.StackTrace);
            }
        }

        internal static void AutomationReportAttachment(DataRow dr, int reportSchID)
        {
            throw new NotImplementedException();
        }

        public static void AutomationReportWSTransactions(DataRow dr, int reportSchID)
        {
            try
            {
                string templateActualPath = AppSettings.Get("AutomatedReportsFSXPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                string reportTitle = AutoReportsHelper.GetValue(dr, "REPORT_TITLE");
                string reportName = AutoReportsHelper.GetValue(dr, "REPORT_NAME");
                string reportUser = AutoReportsHelper.GetValue(dr, "REPORT_USER");
                string reportDescp = AutoReportsHelper.GetValue(dr, "REPORT_DESCP");
                DateTime currentDate = DateTime.Now;
                string formattedDate = currentDate.ToString("yyyyMMdd");
                string reportUniqueName = reportName + "_" + formattedDate + "_Counts" + ".xlsx";
                string reportStoredProc = AutoReportsHelper.GetValue(dr, "REPORT_SQL");
                string reportTemplate = templateActualPath + @"Templates\" + AutoReportsHelper.GetValue(dr, "REPORT_TEMPLATE");
                string reportAttachment = templateActualPath + @"Templates\" + AutoReportsHelper.GetValue(dr, "REPORT_ATTACHMENT");
                
                DateTime now = DateTime.Now;
                DataSet ds = AutoReportsHelper.getAutoReportsDataWithParams(reportStoredProc, reportSchID);
                if (ds != null && ds.Tables.Count > 1)
                {
                    try
                    {
                        TransactionAnalysis tn = new TransactionAnalysis();
                        try
                        {
                            //Email Attachment
                            if (ds.Tables.Contains("Table1"))
                            {
                                DataTable claimspaSummary = ds.Tables[1];

                                List<CLAIMS_PA_SUMMARY> lstclaimspaSummary = new List<CLAIMS_PA_SUMMARY>();
                                lstclaimspaSummary = AutoReportsHelper.ConvertDataTable<CLAIMS_PA_SUMMARY>(claimspaSummary);
                                tn.CLAIMS_PA_SUMMARY = lstclaimspaSummary;
                            }
                        }
                        catch(Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table1", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table2"))
                            {
                                DataTable claimspaDetails = ds.Tables[2];

                                List<CLAIMS_PA_ERROR_DETAILS> lstclaimspaDetails = new List<CLAIMS_PA_ERROR_DETAILS>();
                                lstclaimspaDetails = AutoReportsHelper.ConvertDataTable<CLAIMS_PA_ERROR_DETAILS>(claimspaDetails);
                                tn.CLAIMS_PA_ERROR_DETAILS = lstclaimspaDetails;
                            }
                        }
                        catch(Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table2", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }


                        try
                        {
                            if (ds.Tables.Contains("Table3"))
                            {
                                DataTable eligibilitySummary = ds.Tables[3];

                                //Sheet 2 Eligibility
                                List<ELIGIBILITY_SUMMARY> lsteligibilitySummary = new List<ELIGIBILITY_SUMMARY>();
                                lsteligibilitySummary = AutoReportsHelper.ConvertDataTable<ELIGIBILITY_SUMMARY>(eligibilitySummary);
                                tn.ELIGIBILITY_SUMMARY = lsteligibilitySummary;
                            }

                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table3", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table4"))
                            {
                                DataTable eligibilityDetails = ds.Tables[4];

                                List<ELIGIBILITY_ERROR_DETAILS> lsteligibilityDetails = new List<ELIGIBILITY_ERROR_DETAILS>();
                                lsteligibilityDetails = AutoReportsHelper.ConvertDataTable<ELIGIBILITY_ERROR_DETAILS>(eligibilityDetails);
                                tn.ELIGIBILITY_ERROR_DETAILS = lsteligibilityDetails;
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table4", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table5"))
                            {
                                DataTable hospiceSumary = ds.Tables[5];

                                List<HOSPICE_SUMMARY> lsthospiceSummary = new List<HOSPICE_SUMMARY>();
                                lsthospiceSummary = AutoReportsHelper.ConvertDataTable<HOSPICE_SUMMARY>(hospiceSumary);
                                tn.HOSPICE_SUMMARY = lsthospiceSummary;
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table5", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table6"))
                            {
                                DataTable hospiceDetails = ds.Tables[6];

                                List<HOSPICE_ERROR_DETAILS> lsthospiceDetails = new List<HOSPICE_ERROR_DETAILS>();
                                lsthospiceDetails = AutoReportsHelper.ConvertDataTable<HOSPICE_ERROR_DETAILS>(hospiceDetails);
                                tn.HOSPICE_ERROR_DETAILS = lsthospiceDetails;
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table6", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table7"))
                            {
                                DataTable pfSummary = ds.Tables[7];

                                List<PROV_FINC_SUMMARY> lstpfSummary = new List<PROV_FINC_SUMMARY>();
                                lstpfSummary = AutoReportsHelper.ConvertDataTable<PROV_FINC_SUMMARY>(pfSummary);
                                tn.PROV_FINC_SUMMARY = lstpfSummary;
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table7", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table8"))
                            {
                                DataTable pfDetails = ds.Tables[8];

                                List<PROV_FINC_ERROR_DETAILS> lstpfDetails = new List<PROV_FINC_ERROR_DETAILS>();
                                lstpfDetails = AutoReportsHelper.ConvertDataTable<PROV_FINC_ERROR_DETAILS>(pfDetails);
                                tn.PROV_FINC_ERROR_DETAILS = lstpfDetails;
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table8", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table9"))
                            {
                                DataTable evvSummary = ds.Tables[9];

                                List<EVV_SUMMARY> lstevvSummary = new List<EVV_SUMMARY>();
                                lstevvSummary = AutoReportsHelper.ConvertDataTable<EVV_SUMMARY>(evvSummary);
                                tn.EVV_SUMMARY = lstevvSummary;
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table9", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table10"))
                            {
                                DataTable pmSummary = ds.Tables[10];

                                List<PROV_MGMT_SUMMARY> lstpmSummary = new List<PROV_MGMT_SUMMARY>();
                                lstpmSummary = AutoReportsHelper.ConvertDataTable<PROV_MGMT_SUMMARY>(pmSummary);
                                tn.PROV_MGMT_SUMMARY = lstpmSummary;
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table10", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table11"))
                            {
                                DataTable pmDetails = ds.Tables[11];

                                List<PROV_MGMT_ERROR_DETAILS> lstpmDetails = new List<PROV_MGMT_ERROR_DETAILS>();
                                lstpmDetails = AutoReportsHelper.ConvertDataTable<PROV_MGMT_ERROR_DETAILS>(pmDetails);
                                tn.PROV_MGMT_ERROR_DETAILS = lstpmDetails;
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table11", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table12"))
                            {
                                DataTable atthClaimsSummary = ds.Tables[12];

                                List<ATTACHMENT_CLAIMS_SUMMARY> lstatthClaimsSummary = new List<ATTACHMENT_CLAIMS_SUMMARY>();
                                lstatthClaimsSummary = AutoReportsHelper.ConvertDataTable<ATTACHMENT_CLAIMS_SUMMARY>(atthClaimsSummary);
                                tn.ATTACHMENT_CLAIMS_SUMMARY = lstatthClaimsSummary;
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table12", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }

                        try
                        {
                            if (ds.Tables.Contains("Table13"))
                            {
                                DataTable atthPASummary = ds.Tables[13];

                                List<ATTACHMENT_PA_SUMMARY> lstatthPASummary = new List<ATTACHMENT_PA_SUMMARY>();
                                lstatthPASummary = AutoReportsHelper.ConvertDataTable<ATTACHMENT_PA_SUMMARY>(atthPASummary);
                                tn.ATTACHMENT_PA_SUMMARY = lstatthPASummary;
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogEntry("AutomationReportWSTransactions Table13", String.Format("AutoReports Exception: {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                        }


                        var xlsxTemplate = new XLTemplate(reportAttachment);
                        xlsxTemplate.AddVariable(tn);
                        xlsxTemplate.Generate();
                        var stream = new MemoryStream();
                        xlsxTemplate.Workbook.SaveAs(stream);
                        byte[] bytes = stream.ToArray();

                        int docID = AutoReportsHelper.SaveUploadedFile(reportSchID, reportUniqueName, CON.AutomatedReports.AutomatedReportsProcessDocDescp);
                        if (docID > 0)
                        {
                            int onBaseID = AutoReportsHelper.SendToCMS(docID, bytes, reportUniqueName);

                            if (onBaseID > 0)
                            {
                                AutoReportsHelper.updateScheduledReportStatuswithonBase(reportSchID, CON.AutomatedReports.ToEmail, CON.AutomatedReports.AutomatedReportsProcessAttachmentSuccess, onBaseID);
                            }
                            else
                            {
                                AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, CON.AutomatedReports.AutomatedReportsProcessonBaseFail);
                            }
                        }
                        else
                        {
                            AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, CON.AutomatedReports.AutomatedReportsProcessDocIDFail);
                        }
                    }
                    catch (Exception ex)
                    {
                        AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, ex.Message + " " + ex.StackTrace);
                    }
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    try 
                    { 
                        //Email Body
                        DataTable dtBody = ds.Tables[0];
                        DataRow dataRow = dtBody.Rows[0];
                        string startDate = AutoReportsHelper.GetValue(dataRow, "START_DATE");
                        string endDate = AutoReportsHelper.GetValue(dataRow, "END_DATE");

                        fields.Clear();
                        fields.Add("START_DATE", startDate);
                        fields.Add("END_DATE", endDate);
                        TemplateEvaluator template = new TemplateEvaluator();
                        template.Load(reportTemplate);
                        string body = template.Eval(TemplateFieldAccessors.DictionaryAccessor(fields));
                        AutoReportsHelper.updateScheduledReportBody(reportSchID, body);

                        AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.ToEmail, CON.AutomatedReports.AutomatedReportsProcessBodySuccess);
                    }
                    catch (Exception ex)
                    {
                        AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, ex.Message + " " + ex.StackTrace);
                    }
                }
                if (ds == null)
                {
                    AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, CON.AutomatedReports.AutomatedReportsProcessGenericFail);
                }
            }
            catch (Exception ex)
            {
                AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, ex.Message + " " + ex.StackTrace);
            }
        }

        public static void AutomationReportUploadAttachment(DataRow dr, int reportSchID)
        {
            try { 
            string templateActualPath = AppSettings.Get("AutomatedReportsFSXPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                string reportTitle = AutoReportsHelper.GetValue(dr, "REPORT_TITLE");
                string reportName = AutoReportsHelper.GetValue(dr, "REPORT_NAME");
                string reportUser = AutoReportsHelper.GetValue(dr, "REPORT_USER");
                string reportDescp = AutoReportsHelper.GetValue(dr, "REPORT_DESCP");
                string reportUniqueName = reportName + "_" + reportSchID.ToString() + ".xlsx";
                string reportStoredProc = AutoReportsHelper.GetValue(dr, "REPORT_SQL");
                string reportTemplate = templateActualPath + @"Templates\" + AutoReportsHelper.GetValue(dr, "REPORT_TEMPLATE");
                string reportAttachment = templateActualPath + @"Templates\" + AutoReportsHelper.GetValue(dr, "REPORT_ATTACHMENT");
                bool isAttached = false;
                DateTime now = DateTime.Now;
                string formattedDate = now.ToString("D");
                DataSet ds = AutoReportsHelper.getAutoReportsData(reportStoredProc);
                if (ds != null && ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        try 
                        { 
                            //Email Attachment
                            DataTable dtAttachment = ds.Tables[1];
                            //var context = AutoReportsHelper.GetContext();
                            List<UploadAttachment> uploadAttachment = new List<UploadAttachment>();
                            uploadAttachment = AutoReportsHelper.ConvertDataTable<UploadAttachment>(dtAttachment);

                            UploadAttachmentDetails td = new UploadAttachmentDetails();
                            td.UploadAttachment = uploadAttachment;

                            var xlsxTemplate = new XLTemplate(reportAttachment);
                            xlsxTemplate.AddVariable(td);
                            xlsxTemplate.Generate();
                            var stream = new MemoryStream();
                            xlsxTemplate.Workbook.SaveAs(stream);
                            byte[] bytes = stream.ToArray();

                            int docID = AutoReportsHelper.SaveUploadedFile(reportSchID, reportUniqueName, CON.AutomatedReports.AutomatedReportsProcessDocDescp);
                            if (docID > 0)
                            {
                                int onBaseID = AutoReportsHelper.SendToCMS(docID, bytes, reportUniqueName);

                                if (onBaseID > 0)
                                {
                                    AutoReportsHelper.updateScheduledReportStatuswithonBase(reportSchID, CON.AutomatedReports.ToEmail, CON.AutomatedReports.AutomatedReportsProcessAttachmentSuccess, onBaseID);
                                    isAttached = true;
                                }
                                else
                                {
                                    AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, CON.AutomatedReports.AutomatedReportsProcessonBaseFail);
                                }
                            }
                            else
                            {
                                AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, CON.AutomatedReports.AutomatedReportsProcessDocIDFail);
                            }
                        }
                        catch (Exception ex)
                        {
                            AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, ex.Message + " " + ex.StackTrace);
                        }
                    }
                }
            }
            catch { }
        }
        public static void AutomationReportDailyUploadAttachment(DataRow dr, int reportSchID)
        {
            try
            {
                string templateActualPath = AppSettings.Get("AutomatedReportsFSXPath", string.Empty);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                string reportTitle = AutoReportsHelper.GetValue(dr, "REPORT_TITLE");
                string reportName = AutoReportsHelper.GetValue(dr, "REPORT_NAME");
                string reportUser = AutoReportsHelper.GetValue(dr, "REPORT_USER");
                string reportDescp = AutoReportsHelper.GetValue(dr, "REPORT_DESCP");
                string reportUniqueName = reportName + "_" + reportSchID.ToString() + ".xlsx";
                string reportStoredProc = AutoReportsHelper.GetValue(dr, "REPORT_SQL");
                string reportTemplate = templateActualPath + @"Templates\" + AutoReportsHelper.GetValue(dr, "REPORT_TEMPLATE");
                string reportAttachment = templateActualPath + @"Templates\" + AutoReportsHelper.GetValue(dr, "REPORT_ATTACHMENT");
                bool isAttached = false;
                DateTime now = DateTime.Now;
                string formattedDate = now.ToString("D");
                DataSet ds = AutoReportsHelper.getAutoReportsData(reportStoredProc);
                if (ds != null && ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        try
                        {
                            //Email Attachment
                            DataTable dtAttachment = ds.Tables[1];
                            //var context = AutoReportsHelper.GetContext();
                            List<DailyUploadAttachment> uploadAttachment = new List<DailyUploadAttachment>();
                            uploadAttachment = AutoReportsHelper.ConvertDataTable<DailyUploadAttachment>(dtAttachment);

                            DailyUploadAttachmentDetails td = new DailyUploadAttachmentDetails();
                            td.DailyUploadAttachment = uploadAttachment;

                            var xlsxTemplate = new XLTemplate(reportAttachment);
                            xlsxTemplate.AddVariable(td);
                            xlsxTemplate.Generate();
                            var stream = new MemoryStream();
                            xlsxTemplate.Workbook.SaveAs(stream);
                            byte[] bytes = stream.ToArray();

                            int docID = AutoReportsHelper.SaveUploadedFile(reportSchID, reportUniqueName, CON.AutomatedReports.AutomatedReportsProcessDocDescp);
                            if (docID > 0)
                            {
                                int onBaseID = AutoReportsHelper.SendToCMS(docID, bytes, reportUniqueName);

                                if (onBaseID > 0)
                                {
                                    AutoReportsHelper.updateScheduledReportStatuswithonBase(reportSchID, CON.AutomatedReports.ToEmail, CON.AutomatedReports.AutomatedReportsProcessAttachmentSuccess, onBaseID);
                                    isAttached = true;
                                }
                                else
                                {
                                    AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, CON.AutomatedReports.AutomatedReportsProcessonBaseFail);
                                }
                            }
                            else
                            {
                                AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, CON.AutomatedReports.AutomatedReportsProcessDocIDFail);
                            }
                        }
                        catch (Exception ex)
                        {
                            AutoReportsHelper.updateScheduledReportStatus(reportSchID, CON.AutomatedReports.Failed, ex.Message + " " + ex.StackTrace);
                        }
                    }
                }
            }
            catch { }
        }
    }
}
