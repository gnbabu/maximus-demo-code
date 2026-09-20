using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace MAXIMUS.DataExchange.PDMS.PNM

{
    public class ProcessSpecialtyFile : BaseJob, IJob
    {

        private Logging log = null;
        private readonly string appID = "DB6F0A20-9099-4AB2-BB89-62882BC57650"; //JobId for Specialty File Processing
        public int jobID;
        private Dictionary<string, int> _mappings;

        public ProcessSpecialtyFile(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
            FillInMappings();
        }

        override public void ExecuteJob()
        {
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

                // get the full extract for the staging
                DirectoryInfo localDirectory;
                string extractType = "PROV_PMF_SPECIALTY_EXTRACT";
                string localPath = AppSettings.Get("PNM-ExportLocalPath");
                localDirectory = new DirectoryInfo(String.Format(localPath));
                if (!localDirectory.Exists)
                {
                    localDirectory.Create();
                }
                string exportFileName = extractType + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHMM") + ".txt";


                DataTable dt = ProcessSpecialtyFile.PMFSpecialtyExtract(log, threadID).Tables[0];


                //export file using mapping data to align the records with preceeding and trailing spaces.

                StringBuilder stringBuilder = new StringBuilder();
                string rest;
                if (dt.Rows.Count > 0)
                {
                    jobID = PMFShared.InsertPMFJobsSummary("SpecialityExtractFile", "IN-PROGRESS", 0);//job entry for Speciality File Processing with status IN-PROGRESS                        
                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (_mappings.ContainsKey(column.ColumnName))
                            {
                                int pos = _mappings[column.ColumnName];
                                stringBuilder.AppendFixed(pos, dr[column] != null ? dr[column].ToString() : "", out rest);

                            }
                        }
                        stringBuilder.Append("\x0a");//To have LF as a delimiter. 
                        int EXTRACT_ID = Convert.ToInt32(dr["PNM_SPECIALTY_EXTRACT_ID"]);
                        int PMFFullJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "SpecialityExtractFile", "SpecialityExtract", jobID);

                    }
                    int newJobID = PMFShared.InsertPMFJobsSummary("SpecialityExtractFile", "SUCCESS", jobID);
                }
                string strFilePath = Path.Combine(localDirectory.ToString(), exportFileName);
                using (StreamWriter swriter = new StreamWriter(strFilePath))
                {
                    swriter.Write(stringBuilder.ToString());
                }

                //This one is purposely commented as it's a future feature 
                //Verify the file length and throw exception if overall file length is greater than 4MB
                //FileInfo fileInfo = new FileInfo(strFilePath);
                //if (fileInfo != null && ConvertBytesToMegabytes(fileInfo.Length) > 4)
                //{
                //    throw new Exception("File " + strFilePath + " exceeds 4MB limit.");
                //}

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("PMF Specialty File Exception: Error while generating the file {0}", ex.ToString()));
            }
        }
        //static double ConvertBytesToMegabytes(long bytes)
        //{
        //    return (bytes / 1024f) / 1024f;
        //}

        private static DataSet PMFSpecialtyExtract(Logging log, Guid threadId)
        {
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetPNMSpecialty");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("PMF Specialty File Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        private void FillInMappings()
        {
            _mappings = new Dictionary<string, int>();

            _mappings.Add("SAK_PROV", 9);
            _mappings.Add("ID_PROVIDER", 10);
            _mappings.Add("N/A", 10);
            _mappings.Add("CDE_PROV_TYPE", 2);
            _mappings.Add("CDE_PROV_SPEC", 3);
            _mappings.Add("DTE_EFFECTIVE", 10);
            _mappings.Add("CDE_ENROLL_STATUS", 2);
            _mappings.Add("CDE_ENROLL_REASON", 2);
            _mappings.Add("DTE_END", 10);
            _mappings.Add("N/A_1", 1);

        }


    }

}
