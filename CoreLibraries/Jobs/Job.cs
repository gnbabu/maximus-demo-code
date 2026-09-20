
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

// Database objects generated for this class:
//  TABLEs:     Jobs, JobParams, JobTypes
//  SPs:        usp_GetJobById, usp_GetJobLogByThreadId
//  VIEWs:  
//  FUNCTIONs:  

namespace MAXIMUS.Core.Libraries
{

    /// <summary>
    ///     Author(s):      jfetters
    ///     Date:           2012.02.20
    ///     Name:           Jobs
    ///     Description:    This class encapsulates provides the ability to execute jobs based on a specific job id.
    /// </summary>
    public class Job
    {
        #region "Constructors"

        /// <summary>
        ///     Default constructor which assinged new Guid for the thread Id
        /// </summary>
        public Job()
        {
            // generate a new thread id GUID
            this.ThreadId = Guid.NewGuid();
        }

        /// <summary>
        ///     Constructor which provides the ability to pass a ThreadId Guid
        /// </summary>
        /// <param name="threadId"></param>
        public Job(Guid threadId)
        {
            this.ThreadId = threadId;
        }

        #endregion

        #region "Logging Objects"

        private int logCnt = 0;
        private Guid threadId;
        private Guid ThreadId
        {
            get
            {
                return this.threadId;
            }
            set
            {
                this.threadId = value;
            }
        }

        #endregion

        // export payment file values
        private const string dbtJobs = "Jobs";
        private const string dbtJobParams = "JobParams";

        private const string dbfId = "Id";
        private const string dbfJobParamsJobsId = "JobParamsJobsId";
        private const string dbfSendEmail = "SendEmail";
        private const string dbfName = "Name";
        private const string dbfDllPath = "DllPath";
        private const string dbfAssemblyName = "AssemblyName";
        private const string dbfClassName = "ClassName";
        private const string dbfEmailOnErrors = "EmailOnErrors";
        private const string dbfParamName = "JobParamsName";
        private const string dbfParamValue = "JobParamsAppSettingsValue";
        private const string dbfParamOrder = "JobParamsOrder";
        private const string dbfEncrypt = "JobParamsEncryptOnExport";
        private const string dbfTypeId = "JobTypesId";
        private const string dbfTypeName = "JobTypesName";
        private const string dbfLocation = "JobTypesLocation";
        private const string dbfIsActive = "IsActive";

        private const string dbrJob2Parms = dbtJobs + "2" + dbtJobParams;

        private const string jtInterface = "1";
        private const string jtSetsFTPWithKey = "2";
        private const string jtRunSQL = "3";
        private const string jtRunProc = "4";
        private const string jtGetProc = "5";
        private const string jtSetsFTP = "6";

        private string exText = string.Empty;
        private string jobName = string.Empty;

        private bool hasErrors = false;
        private bool emailOnErrors = false;

        #region "Public Methods"

        /// <summary>
        ///     Executes the provided job Id
        /// </summary>
        /// <param name="JobId">The id (a.k.a. Guid) of the job to execute</param>
        public void ExecuteJob(string JobId)
        {
            // clean up the job id
            JobId = JobId.ToUpper();
            JobId = JobId.Trim();

            // jf :: 2020.10.08 :: Fix issue with not getting job logs
            Guid thisThreadId;
            if (this.ThreadId == Guid.Empty || !Guid.TryParse(this.ThreadId.ToString(), out thisThreadId))
            {
                this.ThreadId = Guid.NewGuid();
            }

            DataSet job = new DataSet();
            // create log object
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logProcessName);
            bool sendEmail = false;

            log.CreateLogEntry(String.Format("Executing JobId {0}", JobId), +logCnt);
            Console.Out.WriteLine("Executing JobId - {0}", JobId);
            // get the job from the database
            job = GetJobDetail(JobId);

            try
            {

                // get the job table from the dataset
                DataTable jobTable = job.Tables[0];

                bool isActive = Convert.ToBoolean(jobTable.Rows[0][dbfIsActive]);

                // if a valid job was not returned
                if (jobTable.Rows.Count == 0)
                {
                    //  generate a log entry with Id stating it failed
                    exText = string.Format("The JobId passed to Scheduler is invalid [Id: {0}]. Check the Jobs table to verify the job exists.", JobId);
                    log.CreateLogEntry(exText, Logging.LogPriority.Error);
                }
                else
                {
                    if (isActive)
                    {
                        // set the job name
                        jobName = Convert.ToString(jobTable.Rows[0][dbfName]);
                        emailOnErrors = Methods.GetBoolean(jobTable.Rows[0][dbfEmailOnErrors]);

                        // get the job type from the first row
                        string filePath = string.Empty;
                        string fullName = string.Empty;
                        string hostName = string.Empty;
                        string remotePath = string.Empty;
                        string userName = string.Empty;
                        string password = string.Empty;
                        string hostKey = string.Empty;
                        int portNumber = 22;
                        string privateKey = string.Empty;
                        string fileName = string.Empty;
                        string jobTypeId = Convert.ToString(jobTable.Rows[0][dbfTypeId]);
                        string procName = string.Empty;
                        DataSet procResults;
                        string procResultsText = string.Empty;
                        string recipients = string.Empty;
                        string sendEmailString = Convert.ToString(jobTable.Rows[0][dbfSendEmail]);
                        bool tempBool = bool.TryParse(sendEmailString, out sendEmail);

                        switch (jobTypeId.Trim())
                        {
                            case jtGetProc:

                                procName = string.Empty;
                                recipients = string.Empty;
                                fileName = string.Empty;
                                string wildcard = string.Empty;
                                string delimiter = "|";
                                filePath = string.Empty;

                                // if the job has parameters
                                foreach (DataRow param in jobTable.ChildRelations[dbrJob2Parms].ChildTable.Rows)
                                {

                                    // set the value to a local string
                                    string order = Convert.ToString(param[dbfParamOrder]);
                                    string val = Convert.ToString(param[dbfParamValue]);

                                    // depending on the order number, set value 
                                    switch (order)
                                    {

                                        case "1":
                                            procName = val;
                                            break;

                                        case "2":
                                            filePath = AppSettings.Get(val);
                                            break;

                                        case "3":
                                            fileName = AppSettings.Get(val);
                                            break;

                                        case "4":
                                            wildcard = AppSettings.Get(val);
                                            break;

                                        case "5":
                                            delimiter = val;
                                            break;
                                    }
                                }

                                // retrieve job log from database
                                procResults = ExecuteStoredProc(procName);

                                // load results into delimited file
                                string fileContents = GetDataTableAsDelimited(procResults.Tables[0], delimiter);

                                // write the file
                                fileName = fileName.Replace(wildcard, DateTime.Now.ToString(wildcard));
                                fullName = filePath + fileName;
                                Methods.WriteStringToFile(filePath, fileName, fileContents, false);
                                log.CreateLogEntry("Writing stored procedure results to file: " + fullName, Logging.LogPriority.Information);

                                break;

                            case jtRunProc:

                                procName = string.Empty;
                                recipients = string.Empty;

                                // if the job has parameters
                                foreach (DataRow param in jobTable.ChildRelations[dbrJob2Parms].ChildTable.Rows)
                                {

                                    // set the value to a local string
                                    string order = Convert.ToString(param[dbfParamOrder]);
                                    string val = Convert.ToString(param[dbfParamValue]);

                                    // depending on the order number, set value 
                                    switch (order)
                                    {

                                        case "1":
                                            procName = val;
                                            break;

                                        case "2":
                                            recipients = AppSettings.Get(val);
                                            break;
                                    }
                                }

                                // retrieve job log from database
                                procResults = ExecuteStoredProc(procName);

                                // convert dataset to html table
                                procResultsText = GetDataTableAsHTML(procResults.Tables[0]);

                                // generate an email
                                SendEmail(JobId, procResultsText);

                                break;

                            case jtRunSQL:

                                // 404CE0E4-7A6B-4A9F-B019-843847028198 - send a count of current attestation states

                                string sql = string.Empty;
                                recipients = string.Empty;

                                // if the job has parameters
                                foreach (DataRow param in jobTable.ChildRelations[dbrJob2Parms].ChildTable.Rows)
                                {

                                    // set the value to a local string
                                    string order = Convert.ToString(param[dbfParamOrder]);
                                    string val = Convert.ToString(param[dbfParamValue]);

                                    // depending on the order number, set value 
                                    switch (order)
                                    {

                                        case "1":
                                            sql = val;
                                            break;

                                        case "2":
                                            recipients = AppSettings.Get(val);
                                            break;
                                    }
                                }

                                // retrieve job log from database
                                DataSet sqlResults = ExecuteSQL(sql);

                                // convert dataset to html table
                                string sqlTextResults = GetDataTableAsHTML(sqlResults.Tables[0]);

                                // generate an email
                                SendEmail(JobId, sqlTextResults);

                                break;

                            case jtInterface:

                                //////////////////////////////////
                                // TN PDMS Jobs                 //
                                //////////////////////////////////

                                try
                                {
                                    string assemblyName = Convert.ToString(jobTable.Rows[0][dbfAssemblyName]);
                                    string className = Convert.ToString(jobTable.Rows[0][dbfClassName]);
                                    string assemblyPath = Methods.GetStringValue(jobTable.Rows[0][dbfDllPath]);
                                    if (assemblyPath.Length > 0)
                                    {
                                        assemblyPath = new DirectoryInfo(assemblyPath).FullName;
                                    }
                                    else
                                    {
                                        assemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
                                    }
                                    if (System.Diagnostics.Debugger.IsAttached)
                                    {
                                        assemblyPath = @"C:\Users\CA_OHPNM_DEV_11\source\repos\p3\ohpnm-src-p3\PDMS\ProviderDataManagementSystemService\bin\";
                                    }
                                    string assemblyFullName = Path.Combine(new Uri(assemblyPath).LocalPath, assemblyName);
                                    /////////////////////////////////////////////////////////////////
                                    // IF RECEIVING AN ERROR ON BELOW LINE OF CODE 
                                    //  Verify that the App.config contains the following in the 
                                    //  <?xml version="1.0" encoding="utf-8" ?>
                                    //  <configuration>
                                    //      <runtime>
                                    //          <loadFromRemoteSources enabled="true" />
                                    //      </runtime>
                                    //  </configuration>
                                    /////////////////////////////////////////////////////////////////
                                    Assembly jobAssembly = Assembly.LoadFrom(assemblyFullName);
                                    Type classType = jobAssembly.GetType(className);

                                    if (classType == null)
                                    {
                                        throw new ArgumentException(string.Format("Class '{0}' does not exist in Assembly '{1}'", className, assemblyFullName));
                                    }
                                    var jobClass = (IJob)Activator.CreateInstance(classType, ThreadId);

                                    Guid id = Guid.Parse(JobId);

                                    ((IJob)jobClass).ExecuteJob(id);

                                }
                                catch (Exception ex)
                                {
                                    log.CreateLogEntry("ExecuteJob failed. Reason: " + ex.Message, Logging.LogPriority.Error, +logCnt);
                                }
                                break;

                            default:

                                //  generate a log entry with job type failed
                                exText = string.Format("The job type provided to Scheduler is invalid [JobTypeId: {0}]", jobTypeId);
                                log.CreateLogEntry(exText, +logCnt);
                                break;

                        }
                    }
                    else
                    {
                        exText = string.Format("The JobId is current set to inactive [Id: {0}] in the Jobs table", JobId);
                        log.CreateLogEntry(exText, Logging.LogPriority.Error);
                    }
                }
                log.CreateLogEntry(String.Format("JobId {0} completed", JobId), +logCnt);

                UpdateJobInfo(JobId);
            }
            catch (Exception ex)
            {
                CoreException.ThrowException(this.ThreadId, ex, logProcessName);
                // log.CreateLogEntry("ExecuteJob failed. Reason: " + ex.Message, Logging.LogPriority.Error, +logCnt);
                // do not rethrow exception so everything completes correctly.
            }

            // if the job should send notifications
            if (sendEmail)
            {
                if ((emailOnErrors == true && hasErrors == true) || emailOnErrors == false)
                {
                    // retrieve job log from database
                    DataSet logEntries = GetJobLog(this.ThreadId.ToString());

                    // convert dataset to html table
                    string logEntriesTable = GetDataTableAsHTML(logEntries.Tables[0]);

                    // jf :: 2020.10.08 :: Fix issue with not getting job logs
                    // SendEmail(Constants.emptyGuid, logEntriesTable);
                    SendEmail(JobId, logEntriesTable);
                }
            }
        }

        #endregion

        #region "Private Methods"

        private string GetDataTableAsDelimited(DataTable thisTable, string delimiter)
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

        private string GetDataTableAsHTML(DataTable thisTable)
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
                        hasErrors = true;
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

        private DataSet ExecuteStoredProc(string procedureName)
        {
            DataSet returnValue = new DataSet();

            try
            {
                returnValue = DataAccess.ExecuteStoredProcedure(procedureName);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            return returnValue;
        }

        private DataSet ExecuteSQL(string sql)
        {
            DataSet returnValue = new DataSet();

            try
            {
                returnValue = DataAccess.ExecuteSelectSql(sql);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            return returnValue;
        }

        private DataSet GetJobDetail(string jobId)
        {
            DataSet jobs = new DataSet();

            try
            {
                // execute the select stored procedure with parameters
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter(dbfId, DbType.Guid, jobId, false));
                jobs = DataAccess.ExecuteStoredProcedure("usp_GetJobById", parameters, "Jobs");

                jobs.Tables[0].TableName = dbtJobs;
                jobs.Tables[1].TableName = dbtJobParams;

                DataRelation relation;
                relation = new DataRelation(dbrJob2Parms
                    , jobs.Tables[dbtJobs].Columns[dbfId]
                    , jobs.Tables[dbtJobParams].Columns[dbfJobParamsJobsId]);
                jobs.Relations.Add(relation);

                return jobs;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private DataSet GetJobLog(string threadId)
        {
            DataSet returnValue = new DataSet();

            try
            {
                // execute the select stored procedure with parameters
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ThreadId", DbType.Guid, threadId, false));
                returnValue = DataAccess.ExecuteStoredProcedure("usp_GetJobLogByThreadId", parameters, "Jobs");
                return returnValue;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private void SendEmail(string jobId, string emailBody)
        {
            try
            {
                // generate an email
                string subject = AppSettings.Get("Jobs-NotificationSubject");
                string emailSubject = String.Format(subject, jobName);
                EMailNotification notify = new EMailNotification(emailBody, emailSubject, string.Empty, this.ThreadId);
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
        private void UpdateJobInfo(string jobId)
        {
            string logMethod = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep 24 2012 12:30PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("Id", DbType.Guid, jobId, true));
                parameters.Add(SqlParms.CreateParameter("LastRan", DbType.DateTime, DateTime.Now, true));

                DataAccess.ExecuteStoredProcedure("sp_UpdateJobById", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
                //WriteLog(String.Format(Constants.LogString.ExceptionEncountered, ex.Message)
                //        , Logging.LogPriority.Error, logMethod);
            }
        }

        #endregion
    }
}