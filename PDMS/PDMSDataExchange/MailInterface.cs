using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    public class MailInterface : BaseJob, IJob
    {
#region "Class Level Declarations"

#endregion

#region "Constructors"
        public MailInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }
#endregion

#region "Public Methods"
        override public void ExecuteJob()
        {
            // Default Job - B2P Retrieve Payment Information
            this.ExecuteJob(Guid.Parse("23CE7A2E-AE97-4C37-89E9-646F1FDF32E7"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                // MMIS Retrieve Payment Information
                case "23CE7A2E-AE97-4C37-89E9-646F1FDF32E7":
                    this.CreateMailFile();
                    break;
            }
        }

        public string CreateMailFile()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // retrieve and set required variables
                string localPath = AppSettings.Get("Mail-SubmitLocalPath");
                string fileName = AppSettings.Get("Mail-SubmitFile");
                string dtmWildcard = AppSettings.Get("Mail-SubmitFileWildcardDTM");
                DirectoryInfo localDirectory;
                    
                // Get file name
                fileName = fileName.Replace(dtmWildcard, DateTime.Now.ToString(dtmWildcard));

                // create local path if it does not exist
                Directory.CreateDirectory(localPath);
                localDirectory = new DirectoryInfo(localPath);

               
                this.CreateMailFile(log, localPath, fileName);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);

            return string.Empty;
        }

        private void CreateMailFile(Logging log, string filePath, string fileName)
        {
            // create log entry
            log.CreateLogEntry(String.Format("Start - Create Mail File {0}", filePath + fileName));
                
            try
            {
                // get the staging records
                DataSet groupDs = this.GetUnprintedMail();
                int sequenceNumber = 0;

                // create log entry
                log.CreateLogEntry(String.Format("Create Mail File, Total mails {0}", groupDs.Tables[0].Rows.Count));
                string [] finalDoc = new string[groupDs.Tables[0].Rows.Count];

                // loop over records in dataset
                foreach (DataRow mailRow in groupDs.Tables[0].Rows)
                {
                    int mailId = Convert.ToInt32(mailRow["MAIL_ID"].ToString());
                    string body = mailRow["BODY"].ToString();

                    //// todo replace sequence number, date in the template
                    //body = body.Replace("[sequencenumber]", sequenceNumber.ToString());
                    //body = body.Replace("[todaysdate]", DateTime.Today.ToString("MM dd yyyy"));
                    //// Append to file

                    //finalDoc[sequenceNumber] = body; 
                    //commented the blank pages for crawford implementation
                    if (sequenceNumber == 0)
                    {
                        //for first record no need to add page break
                        finalDoc[sequenceNumber] = body; //+ "<div style='page-break-after:always;'></div><div>&nbsp;</div>";
                    }
                    else
                    {
                        finalDoc[sequenceNumber] = "<div style='page-break-before:always;'></div>" + body;// + "<div style='page-break-after:always;'></div><div>&nbsp;</div>";
                    }

                    // Update Mail submit date time
                    this.UpdateMail(mailId, log);

                    ++sequenceNumber;
                     
                    //finalDoc[sequenceNumber] += @"<p style=""page-break-after:always;""></p>"; //adding page breaks after body

                }
                // create HTML file
                string htmlFileName = fileName.Replace(".pdf", ".html");
                string htmlFileFullPath = filePath + fileName.Replace(".pdf", ".html");
                
                File.AppendAllLines(htmlFileFullPath, finalDoc);
            
                // Create PDF from the html file
                string result = ProcessDocumentController.CreatePDFFromHTML(filePath, htmlFileName);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }

            // create log entry
            log.CreateLogEntry(String.Format("Mail File creation complete ", filePath));
        }

        private DataSet GetUnprintedMail()
        {
            DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetPendingMail");

            return ds;
        }

        private void UpdateMail(int mailId, Logging log)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("MAIL_ID", DbType.Int32, mailId, true));
            parameters.Add(SqlParms.CreateParameter("PRINT_DATE_TIME", DbType.DateTime, DateTime.Now, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

            DataAccess.ExecuteStoredProcedure("usp_UpdateMail", parameters);
        }
    }
#endregion
}