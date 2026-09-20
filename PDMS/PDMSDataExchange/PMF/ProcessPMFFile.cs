using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

namespace MAXIMUS.DataExchange.PDMS.PNM

{
    public class ProcessPMFFile : BaseJob, IJob
    {

        private Logging log = null;
        public int jobID;
        private readonly string appID = "d1f9835f-dc51-4186-b322-766067c23394";//JobId for Full Extract File Processing

        public ProcessPMFFile(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        override public void ExecuteJob()
        {
            // Default Job - Full Extract File Generation
            this.ExecuteJob(Guid.Parse(appID.ToString()));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    GenerateStagingFiles();
                    break;
            }
        }

        public void GenerateStagingFiles()
        {

            try
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Guid threadID = new Guid(appID);
                log = new Logging(threadID, logMsg);

                List<SqlParameter> parameters = new List<SqlParameter>();

                DirectoryInfo localDirectory;
                string extractType = "PROV_PMF_FULL_EXTRACT";
                 string localPath = AppSettings.Get("PNM-ExportLocalPath");                
                localDirectory = new DirectoryInfo(String.Format(localPath));

                if (!localDirectory.Exists)
                {
                    localDirectory.Create();
                }

                string exportFileName = extractType + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHMM") + ".txt";

                //Stage the date
                DataTable mapping = ProcessPMFFile.PMFStaggingMapping(log, threadID).Tables[0];
                DataTable dt = ProcessPMFFile.PMFFullExtract(log, threadID).Tables[0];

                //Maintain the dictionary to store start and end positions.
                Dictionary<string, int> positionMapping = new Dictionary<string, int>();
                int start, end;
                string colName;

                if (dt.Rows.Count > 0)
                {
                    jobID = PMFShared.InsertPMFJobsSummary("FullExtractFile", "IN-PROGRESS", 0);//job entry for Master File Processing with status IN-PROGRESS

                    foreach (DataRow item in mapping.Rows)
                    {
                        start = Convert.ToInt32(item["STARTINGPOSITION"]);
                        end = Convert.ToInt32(item["ENDINGPOSITION"]);
                        colName = item["SourceColumnName"].ToString();
                        positionMapping.Add(colName, end - start + 1);
                    }
                }
                //Export file using mapping data to align the records with preceeding and trailing spaces.
                StringBuilder stringBuilder = new StringBuilder();
                string rest;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (positionMapping.ContainsKey(column.ColumnName))
                            {
                                int pos = positionMapping[column.ColumnName];
                                stringBuilder.AppendFixed(pos, dr[column] != null ? dr[column].ToString() : "", out rest);
                            }

                        }
                        stringBuilder.Append("\x0a");//To have LF as a delimiter. 
                        int EXTRACT_ID = Convert.ToInt32(dr["PNM_FULLEXTRACT_ID"]);
                        int PMFFullJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "FullExtractFile", "FullExtract", jobID);
                    }
                    int newJobID = PMFShared.InsertPMFJobsSummary("FullExtractFile", "SUCCESS", jobID);//job entry for Master File Processing with status IN-PROGRESS
                }
                string strFilePath = Path.Combine(localDirectory.ToString(), exportFileName);
                using (StreamWriter swriter = new StreamWriter(strFilePath))
                {
                    swriter.Write(stringBuilder.ToString());
                }
                //PMFFullExtract table update with the job id generated after file write and processed date

                //This one is purposely commented as it's a future feature 
                //Verify the file length and throw exception if overall file length is greater than 5MB
                //FileInfo fileInfo = new FileInfo(strFilePath);
                //if (fileInfo != null && ConvertBytesToMegabytes(fileInfo.Length) > 5)
                //{
                //    throw new Exception("File " + strFilePath + " exceeds 5MB limit.");
                //}


            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("PMF File Exception: Error while generating the file {0}", ex.ToString()));
            }
        }

        //static double ConvertBytesToMegabytes(long bytes)
        //{
        //    return (bytes / 1024f) / 1024f;
        //}

        public static DataSet PMFStaggingMapping(Logging log, Guid threadId)
        {
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GETStagingMappingTable");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("PMF File Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }
        public static DataSet PMFFullExtract(Logging log, Guid threadId)
        {
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetPNMFullExtract");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("PMF File Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

    }

    //Extension method to add empty spaces to match to the ending position
    public static class ExtensionMethod
    {
        public static StringBuilder AppendFixed(this StringBuilder sb, int length, string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return sb.Append(string.Empty.PadLeft(length));

            if (value.Length <= length)
                return sb.Append(value.PadLeft(length));
            else
                return sb.Append(value.Substring(0, length));
        }

        public static StringBuilder AppendFixed(this StringBuilder sb, int length, string value, out string rest)
        {
            rest = string.Empty;

            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return sb.AppendFixed(length, value.Trim());

            if (value.Length > length)
                rest = value.Substring(length);

            return sb.AppendFixed(length, value);
        }
    }




}
