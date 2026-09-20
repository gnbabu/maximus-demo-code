using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Corp.Core.Libraries;

namespace MAXIMUS.DataExchange.PDMS
{
    public class OnBaseFileUploader : BaseJob, IJob
    {
        public OnBaseFileUploader(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }


        override public void ExecuteJob()
        {
            // Default Job - CAQH Retrieve Return Roster
            this.ExecuteJob(Guid.Parse("288EC9C0-557A-447F-9DE4-B952A8ACCD60"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            if (jobGuid == "288EC9C0-557A-447F-9DE4-B952A8ACCD60")
            {
                //Check for files to upload and upload them
                UploadFilesToOnBase();
            }
        }
        
        public void UploadFilesToOnBase()
        {
            OnBaseInterface onBaseInterface = new OnBaseInterface();
            int regID = 0;
            int docID = 0;
            string fileName = string.Empty;
            string destinationPath = AppSettings.Get("FileStorePathWeb");
            if (System.Diagnostics.Debugger.IsAttached)
            {
                destinationPath = @"C:\Temp\D\Projects\Deployments\PDMS\PDMS_DEV\FileStoreLocal";
            }
            byte[] fileBytes;
            int batchSize = Convert.ToInt32(AppSettings.Get("OnBase-BatchSize", "100")); // limit the number of docs per job run

            // Find records with ONBASE_DOCUMENT_ID = null
            List<SqlParameter> parameters = new List<SqlParameter>();
            DataSet dsNullOnBaseIDs = DataAccess.ExecuteStoredProcedure("usp_SelectNullOnBaseIDs", parameters, "NullOnBaseIDs");

            // Loop through each record and upload to OnBase
            if (Methods.HasRows(dsNullOnBaseIDs))
            {
                for (int i = 0; i < dsNullOnBaseIDs.Tables[0].Rows.Count && i < batchSize; i++)
                {
                    DataRow row = dsNullOnBaseIDs.Tables[0].Rows[i];
                    regID = Methods.GetIntValue(row, "REG_ID");
                    docID = Methods.GetIntValue(row, "DOCUMENT_ID");
                    fileName = Methods.GetStringValue(row, "FILE_NAME");

                    try
                    {
                        fileBytes = File.ReadAllBytes(Path.Combine(@destinationPath, fileName));
                        onBaseInterface.SubmitFile(regID, docID, fileBytes, fileName);
                        WriteLog(string.Format("Sent DOCUMENT_ID {0}, REG_ID {1}, FILE_NAME '{2}' to OnBase", docID, regID, fileName));
                    }
                    catch (Exception ex)
                    {
                        batchSize++; // increase the batch size on failure since the file never made it to OnBase.
                        WriteLog(string.Format("Failed to send DOCUMENT_ID {0}, REG_ID {1} to OnBase. Exception text is: {2}", docID, regID, ex.Message));
                    }
                }
            }
        }

        private void WriteLog(string msg)
        {
            // create log object
            string logProcessName = "OnBaseFileUploader";
            Logging log = new Logging(new Guid("96A00C69-EDE1-4A82-9F73-5D1936D08050"), logProcessName);
            log.CreateLogEntry(msg);
        }

        private void WriteDocIDsToLog(string docIDsToSend)
        {
            if (docIDsToSend.Length > 1400) // In case the string length is longer than the db column can handle, split into 2 entries
            {
                WriteLog(string.Format("Successfully sent DOCUMENT_IDs to OnBase: {0}", docIDsToSend.Substring(0, 700)));
                WriteLog(string.Format("Successfully sent DOCUMENT_IDs to OnBase: {0}", docIDsToSend.Substring(700, docIDsToSend.Length - 700)));
            }
            else
                WriteLog(string.Format("Successfully sent DOCUMENT_IDs to OnBase: {0}", docIDsToSend));
        }
    }
}