using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MAXIMUS.Core.Libraries.QuartzJobs
{
    /// <summary>
    ///     Author(s):      Ahackl
    ///     Date:           2017.06.12
    ///     Name:           QuartzJob
    ///     Description:    Abstract Class For All Quartz Job Types
    /// </summary>
    public abstract class QuartzJob : Quartz.IJob
    {
        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Job Type Id abstract property 
        /// </summary>
        public abstract int JobTypesID { get; }

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Main execution abstract method
        /// </summary>
        public abstract void Execute(Quartz.IJobExecutionContext context);

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Virtual method to send email 
        /// </summary>
        protected virtual void SendEmail(string jobId, string emailBody, string jobName, Guid threadID)
        {
            try
            {
                // generate an email
                string subject = AppSettings.Get("Jobs-NotificationSubject");
                string emailSubject = String.Format(subject, jobName);
                EMailNotification notify = new EMailNotification(emailBody, emailSubject, string.Empty, threadID);
                if (jobId == Constants.emptyGuid)
                {
                    notify.SendGenericJobNotification(true);
                }
                else
                {
                    Guid jobGuid = new Guid(jobId);
                    notify.SendJobNotification(true, jobGuid);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Virtual method to execute SQL string
        /// </summary>
        protected virtual DataSet ExecuteSQL(string sql, Guid threadID)
        {
            DataSet returnValue = new DataSet();

            try
            {
                returnValue = DataAccess.ExecuteSelectSql(sql);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(threadID, ex);
            }
            return returnValue;
        }

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Virtual method to execute a stored procedure
        /// </summary>
        protected virtual DataSet ExecuteStoredProc(string procedureName, Guid threadID)
        {
            DataSet returnValue = new DataSet();

            try
            {
                returnValue = DataAccess.ExecuteStoredProcedure(procedureName);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(threadID, ex);
            }
            return returnValue;
        }

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Virtual method to get data table in in HTML format
        /// </summary>
        protected virtual string GetDataTableAsHTML(DataTable thisTable)
        {
            int errorCount = 0;
            string errorString = @"[[[[[0]]]]]";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendFormat(@"<caption> Total Rows = ");
            sb.AppendFormat(thisTable.Rows.Count.ToString());
            sb.AppendFormat(@"  </caption>");
            sb.AppendFormat(errorString);

            sb.Append("<table border=1>");

            sb.Append("<tr align='center' style='color:white;background-color:darkblue;'>");

            //first append the header with column names.
            foreach (DataColumn column in thisTable.Columns)
            {
                sb.Append("<td><b>");
                sb.Append(column.ColumnName);
                sb.Append("</b></td>");
            }

            sb.Append("</tr>");

            // next, the column values.
            foreach (DataRow row in thisTable.Rows)
            {
                if (thisTable.Columns.Contains("LogPriority"))
                {
                    string logPriority = Methods.GetStringValue(row["LogPriority"]);
                    if (logPriority.EndsWith("5"))
                    {
                        sb.Append("<tr align='center' style='background-color:#FFFF00'>");
                        errorCount += 1;
                    }
                    else
                    {
                        sb.Append("<tr align='center'>");
                    }
                }

                foreach (DataColumn column in thisTable.Columns)
                {
                    sb.Append("<td>");
                    if (row[column].ToString().Trim().Length > 0)
                        sb.Append(row[column]);
                    else
                        sb.Append("&nbsp;");
                    sb.Append("</td>");
                }

                sb.Append("</tr>");
            }
            sb.Append("</table>");

            System.Text.StringBuilder sbe = new System.Text.StringBuilder();

            if (errorCount > 0)
            {
                sbe.AppendFormat(@"<caption> Error Rows = ");
                sbe.AppendFormat(errorCount.ToString());
                sbe.AppendFormat(@"</caption>");
            }

            sb.Replace(errorString, sbe.ToString());

            return sb.ToString();
        }

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Virtual method to get data table in delimited format
        /// </summary>
        protected virtual string GetDataTableAsDelimited(DataTable thisTable, string delimiter)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder headerLine = new StringBuilder();


            //first append the header with column names.
            foreach (DataColumn column in thisTable.Columns)
            {
                headerLine.Append(column.ColumnName);
                headerLine.Append(delimiter);
            }
            sb.AppendLine(headerLine.ToString());

            // next, the column values.
            foreach (DataRow row in thisTable.Rows)
            {
                StringBuilder line = new StringBuilder();
                foreach (DataColumn column in thisTable.Columns)
                {
                    line.Append(row[column]);
                    line.Append(delimiter);
                }
                sb.AppendLine(line.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        protected virtual DataSet GetJobLog(Guid threadId)
        {
            DataSet returnValue = new DataSet();

            try
            {
                // execute the select stored procedure with parameters
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ThreadId", DbType.Guid, threadId.ToString(), false));
                returnValue = DataAccess.ExecuteStoredProcedure("usp_GetJobLogByThreadId", parameters, "Jobs");
                return returnValue;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(threadId, ex);
            }
        }
    }
}
