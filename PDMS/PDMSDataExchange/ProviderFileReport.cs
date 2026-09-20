using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using static MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS
{
    public class ProviderFileReport : BaseJob, IJob
    {
        const string ProviderFileReportJobId = "A14B95E1-72F3-4062-B291-6B587BB54B67";

        static string ProviderFileReportsPath = AppSettings.Get("ProviderFileReportsPath", string.Empty);
        static string DailyReportsPath = ProviderFileReportsPath + @"\DailyReports";
        static string QuaterlyReportsPath = ProviderFileReportsPath + @"\QuaterlyReports";
        string TemporaryPath = ProviderFileReportsPath + @"\Temporary";

        string prescribingIndividualFileName = "DRRX.PROD.OHJFS.EDS.PPROV.DAT.nnn.txt";
        string prescribingZipFileName = "DRRX.PROD.OHJFS.EDS.PPROV.DAT.zip";

        string dispensingIndividualFileName = "DRRX.PROD.OHJFS.EDS.DPROV.DAT.nnn.txt";
        string dispensingZipFileName = "DRRX.PROD.OHJFS.EDS.DPROV.DAT.zip";

        private Logging log = null;

        public ProviderFileReport(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse(ProviderFileReportJobId));
        }

        override public void ExecuteJob(Guid jobId)
        {
            log = new Logging(this.ThreadId);
            //Daily Reports
            //ProviderFileType.Prescribing
            log.CreateLogEntry("Provider File Report Job: Job has STARTED!", Logging.LogPriority.Information);

            CreateProviderFileReports(jobId, ProviderFileType.Prescribing);

            //ProviderFileType.Dispensing
            CreateProviderFileReports(jobId, ProviderFileType.Dispensing);

            //Quaterly Reports
            //ProviderFileType.Prescribing
            CreateProviderFileReports(jobId, ProviderFileType.Prescribing, true);

            //ProviderFileType.Dispensing
            CreateProviderFileReports(jobId, ProviderFileType.Dispensing, true);

            log.CreateLogEntry("Provider File Report Job: Job has COMPLETED!", Logging.LogPriority.Information);
        }

        private void CreateProviderFileReports(Guid jobGuid, int providerFileTypeID, bool isQuaterly = false)
        {
            string strFileType = providerFileTypeID == 1 ? "Prescribing" : "Dispensing";

            if (System.Diagnostics.Debugger.IsAttached)
            {
                DailyReportsPath = @"C:\Temp\CR082\DailyReports";
                QuaterlyReportsPath = @"C:\Temp\CR082\QuaterlyReports";
                TemporaryPath = @"C:\Temp\CR082\Temporary";
            }

            try
            {
                DateTime lastRunDate = GetProviderFileReportLatestDate(providerFileTypeID, isQuaterly);
                log.CreateLogEntry("Provider File Report Job: Fetched the last run date for " + (isQuaterly ? "Quaterly " : "") + strFileType + ". It is: " + lastRunDate.ToShortDateString(), Logging.LogPriority.Information);

                //Create folders for Processed (success and failure) files
                if (!Directory.Exists(DailyReportsPath))
                {
                    log.CreateLogEntry("Provider File Report Job: Creating the Daily reports path folder", Logging.LogPriority.Information);
                    Directory.CreateDirectory(DailyReportsPath);
                }
                if (!Directory.Exists(QuaterlyReportsPath))
                {
                    log.CreateLogEntry("Provider File Report Job: Creating the Quaterly reports path folder", Logging.LogPriority.Information);
                    Directory.CreateDirectory(QuaterlyReportsPath);
                }
                if (!Directory.Exists(TemporaryPath))
                {
                    log.CreateLogEntry("Provider File Report Job: Creating the Temporary reports path folder", Logging.LogPriority.Information);
                    Directory.CreateDirectory(TemporaryPath);
                }

                List<ProviderFileData> lstProviderFileData = new List<ProviderFileData>();

                DirectoryInfo temporaryDirectory = new DirectoryInfo(TemporaryPath);
                log.CreateLogEntry("Provider File Report Job: Emptying the temporary path folder", Logging.LogPriority.Information);
                EmptyDirectory(TemporaryPath);

                string fileName = (providerFileTypeID == ProviderFileType.Prescribing ? prescribingIndividualFileName : dispensingIndividualFileName);
                string formattedFileName = fileName.Replace("nnn", "1");

                if (isQuaterly)
                {
                    log.CreateLogEntry("Provider File Report Job: Preparing to create " + strFileType + " Quaterly file", Logging.LogPriority.Information);

                    if ((DateTime.Now - lastRunDate).Days >= 90)
                    {
                        log.CreateLogEntry("Provider File Report Job: Getting Quaterly Provider File Data", Logging.LogPriority.Information);
                        lstProviderFileData = GetQuaterlyProviderFileData(lastRunDate, providerFileTypeID, jobGuid);
                        // OHPNM-8983 add quarterly to filename specification 
                        CreateIndividualFiles(formattedFileName.Replace("PROV.DAT", "PROV.QTRLY.DAT"), 1, temporaryDirectory, jobGuid, providerFileTypeID, isQuaterly, lstProviderFileData);

                        log.CreateLogEntry("Provider File Report Job: Creating " + strFileType + " Quaterly Zip file", Logging.LogPriority.Information);
                        CreateZipFileAndCleanUp(temporaryDirectory, jobGuid, providerFileTypeID, isQuaterly);
                        log.CreateLogEntry("Provider File Report Job: Successfully created " + strFileType + " Quaterly Zip file", Logging.LogPriority.Information);
                    }
                    else
                    {
                        log.CreateLogEntry("Provider File Report Job: Quaterly files are not due yet. Since the last run date is " + lastRunDate.ToShortDateString(), Logging.LogPriority.Information);
                    }
                }
                else
                {
                    log.CreateLogEntry("Provider File Report Job: Started creating " + strFileType + " Daily file(s)", Logging.LogPriority.Information);

                    lstProviderFileData = InsertAndGetProviderFileData(lastRunDate, providerFileTypeID, jobGuid);
                    CreateIndividualFiles(formattedFileName, 1, temporaryDirectory, jobGuid, providerFileTypeID, isQuaterly, lstProviderFileData);

                    log.CreateLogEntry("Provider File Report Job: Creating " + strFileType + " Daily Zip file", Logging.LogPriority.Information);
                    CreateZipFileAndCleanUp(temporaryDirectory, jobGuid, providerFileTypeID, isQuaterly);
                    log.CreateLogEntry("Provider File Report Job: Successfully created " + strFileType + " Daily Zip file", Logging.LogPriority.Information);
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Provider File Report Job: Failed to create Provider File Report. Exception Message: " + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
            }
        }

        private void CreateIndividualFiles(string fileName, int sequence, DirectoryInfo temporaryDirectory, Guid jobGuid, int providerFileTypeID, bool isQuaterly, List<ProviderFileData> lstProviderFileData)
        {
            if (lstProviderFileData.Count() > 0)
            {
                log.CreateLogEntry("Provider File Report Job: Creating individual files", Logging.LogPriority.Information);

                string filePath = temporaryDirectory + "\\" + fileName;

                var recordsCount = lstProviderFileData.Count();
                var providersCount = lstProviderFileData.GroupBy(x => x.REG_ID).Select(x => x).Count();

                //Header
                File.AppendAllLines(filePath, new[] { "HDRPROVIDER-" + (providerFileTypeID == 1 ? "P" : "D") + DateTime.Now.ToString("yyyyMMdd") + sequence.ToString().PadRight(4, '0') });

                var resultsOrder = new[] { "PA", "DE", "EP", "PS", "NM" };
                lstProviderFileData = lstProviderFileData.OrderBy(x => x.REG_ID).ThenBy(x => Array.IndexOf(resultsOrder, x.SegmentIdentifier)).ToList();

                foreach (var item in lstProviderFileData)
                {
                    File.AppendAllLines(filePath, new[] { FormatData(item) });
                }

                //Trailer
                File.AppendAllText(filePath, "TRL" + recordsCount.ToString().PadLeft(8, '0') + providersCount.ToString().PadLeft(8, '0') + String.Empty.PadRight(6, ' '));

                //capture the information on the database
                FileInfo[] individualTextFiles = temporaryDirectory.GetFiles("*.txt");

                log.CreateLogEntry("Provider File Report Job: Total individual files created count: " + individualTextFiles.Count(), Logging.LogPriority.Information);

                log.CreateLogEntry("Provider File Report Job: Saving Provider File report details to the database.", Logging.LogPriority.Information);
                for (int i = 0; i < individualTextFiles.Count(); i++)
                {
                    var file = individualTextFiles[i];
                    SaveProviderFileReportDetails(providerFileTypeID, file.FullName, file.Name, file.CreationTime, 1, recordsCount, providersCount, isQuaterly, jobGuid);
                }
            }
        }

        private void CreateZipFileAndCleanUp(DirectoryInfo temporaryDirectory, Guid jobGuid, int providerFileTypeID, bool isQuaterly)
        {
            try
            {
                if (temporaryDirectory.GetFiles().Any())
                {
                    FileCompression fc = new FileCompression(this.ThreadId);
                    string zipfileNameAndPath = "";
                    if (isQuaterly)
                    {
                        zipfileNameAndPath = QuaterlyReportsPath + "\\" + (providerFileTypeID == ProviderFileType.Prescribing ? prescribingZipFileName : dispensingZipFileName);
                        zipfileNameAndPath = zipfileNameAndPath.Replace(".DAT", ".QTLY.DAT");
                    }
                    else
                    {
                        zipfileNameAndPath = DailyReportsPath + "\\" + (providerFileTypeID == ProviderFileType.Prescribing ? prescribingZipFileName : dispensingZipFileName);
                    }

                    fc.ZipDirectory(TemporaryPath, zipfileNameAndPath);

                    EmptyDirectory(TemporaryPath);
                    temporaryDirectory.Delete();
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Provider File Report Job: Exception Message: " + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
            }
        }

        private string FormatData(ProviderFileData fileData)
        {
            string retValue = string.Empty;
            try
            {

                if (fileData.SegmentIdentifier.ToUpper() == "PA")
                {
                    retValue += fileData.SegmentIdentifier;
                    retValue += fileData.ProviderMedicaidID.PadRight(7, ' ');
                    retValue += fileData.AddressType.PadRight(1, ' '); ;
                    retValue += fileData.ProviderCounty.PadRight(2, ' ');
                    retValue += fileData.ProviderName.PadRight(50, ' ');
                    retValue += fileData.ProviderAddress1.PadRight(60, ' ');
                    retValue += fileData.ProviderAddress2.PadRight(60, ' ');
                    retValue += fileData.ProviderCity.PadRight(30, ' ');
                    retValue += fileData.ProviderState.PadRight(2, ' ');
                    retValue += fileData.ProviderZipcode.PadRight(5, ' ');
                    retValue += fileData.ProviderZipcodeExt.PadRight(4, ' ');
                    retValue += fileData.ProviderPhoneNumber.PadRight(10, ' ');
                    retValue += fileData.LicenseNumber.PadRight(9, ' ').Substring(0, 9);
                }
                else if (fileData.SegmentIdentifier.ToUpper() == "DE")
                {
                    retValue += fileData.SegmentIdentifier;
                    retValue += fileData.DEANumber.PadRight(9, ' ');
                    retValue += fileData.EffectiveDate.PadRight(8, ' ');
                    retValue += fileData.EndDate;
                }
                else if (fileData.SegmentIdentifier.ToUpper() == "EP")
                {
                    retValue += fileData.SegmentIdentifier;
                    retValue += fileData.ProviderProgramCode.PadRight(5, ' ');
                    retValue += fileData.ProviderEnrollmentDate.PadRight(8, ' ');
                    retValue += fileData.ProviderDisenrollmentDate.PadRight(8, ' ');
                    retValue += fileData.EnrollmentStatusID.PadRight(2, ' ');
                }
                else if (fileData.SegmentIdentifier.ToUpper() == "PS")
                {
                    retValue += fileData.SegmentIdentifier;
                    retValue += fileData.ProviderTypeID.PadRight(2, ' ');
                    retValue += fileData.ProviderSpecialtyID.PadRight(3, ' ');
                    retValue += fileData.SpecialtyEffectiveDate.PadRight(8, ' ');
                    retValue += fileData.SpecialtyEndDate;
                }
                else if (fileData.SegmentIdentifier.ToUpper() == "NM")
                {
                    retValue += fileData.SegmentIdentifier;
                    retValue += fileData.ProviderNPI.PadRight(10, ' ');
                    retValue += fileData.NPIEffectiveDate.PadRight(8, ' ');
                    retValue += fileData.NPIEndDate;
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Provider File Report Job: Exception in FormatData() method. Error message: " + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
            }

            return retValue;
        }

        private void EmptyDirectory(string directoryPath)
        {
            DirectoryInfo extractDirectory = new DirectoryInfo(directoryPath);
            foreach (FileInfo file in extractDirectory.GetFiles())
            {
                file.Delete();
            }
        }

        private bool SaveProviderFileReportDetails(int providerFileTypeID, string filePath, string fileName, DateTime fileCreationDate, int sequence, int recordsCount, int providersCount, bool isQuaterly, Guid creationUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_FILE_TYPE_ID", DbType.Int32, providerFileTypeID, false));
                parameters.Add(SqlParms.CreateParameter("FILE_PATH", DbType.String, filePath, false));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, fileName, false));
                parameters.Add(SqlParms.CreateParameter("FILE_CREATION_DATE", DbType.DateTime, fileCreationDate, false));
                parameters.Add(SqlParms.CreateParameter("SEQUENCE", DbType.Int32, sequence, false));
                parameters.Add(SqlParms.CreateParameter("RECORDS_COUNT", DbType.Int32, recordsCount, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_COUNT", DbType.Int32, providersCount, false));
                parameters.Add(SqlParms.CreateParameter("IS_QUARTERLY", DbType.Boolean, isQuaterly, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, creationUser, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, false));

                var retVal = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SaveProviderFileReportDetails", parameters));

                if (retVal > 0)
                    return true;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Provider File Report Job: Failed to Record Provider File report details To Database. Exception: " + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
            }

            return false;
        }

        private DateTime GetProviderFileReportLatestDate(int providerFileTypeID, bool isQuarterly)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_FILE_TYPE_ID", DbType.Int32, providerFileTypeID, false));
                parameters.Add(SqlParms.CreateParameter("Is_Quarterly", DbType.Boolean, isQuarterly, false));

                var latestDate = DataAccess.ExecuteScalar("usp_GetProviderFileReportLatestDate", parameters);

                if (!string.IsNullOrWhiteSpace(latestDate))
                    return Convert.ToDateTime(latestDate);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Provider File Report Job: Failed to Get Provider File Report Latest Date. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
            }

            return DateTime.Now.AddDays(-1);
        }

        private List<ProviderFileData> InsertAndGetProviderFileData(DateTime startDate, int providerFileTypeID, Guid threadId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("START_DATE", DbType.DateTime, startDate, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_FILE_TYPE_ID", DbType.Int32, providerFileTypeID, false));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_InsertAndSelectSTG_ProviderFileData", parameters, "ProviderFileData");
                return DataTableToList(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Provider File Report Job: Failed to Insert and Get Provider file data. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        private List<ProviderFileData> GetQuaterlyProviderFileData(DateTime startDate, int providerFileTypeID, Guid threadId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("START_DATE", DbType.DateTime, startDate, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_FILE_TYPE_ID", DbType.Int32, providerFileTypeID, false));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetQuaterlyProviderFileData", parameters, "ProviderFileData");
                return DataTableToList(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Provider File Report Job: Failed to Insert and Get Provider file data. Exception Message: " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        private List<ProviderFileData> DataTableToList(DataTable dt)
        {
            List<ProviderFileData> lst = new List<ProviderFileData>();
            string dateFormat = "yyyyMMdd";

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ProviderFileData fileData = new ProviderFileData();

                    fileData.REG_ID = Convert.ToInt32(dr["REG_ID"]);
                    fileData.SegmentIdentifier = Convert.ToString(dr["SegmentIdentifier"]);
                    fileData.ProviderMedicaidID = Convert.ToString(dr["ProviderMedicaidID"]);
                    fileData.AddressType = Convert.ToString(dr["AddressType"]);
                    fileData.ProviderCounty = Convert.ToString(dr["ProviderCounty"]);
                    fileData.ProviderName = Convert.ToString(dr["ProviderName"]);
                    fileData.ProviderAddress1 = Convert.ToString(dr["ProviderAddress1"]);
                    fileData.ProviderAddress2 = Convert.ToString(dr["ProviderAddress2"]);
                    fileData.ProviderCity = Convert.ToString(dr["ProviderCity"]);
                    fileData.ProviderState = Convert.ToString(dr["ProviderState"]);
                    fileData.ProviderZipcode = Convert.ToString(dr["ProviderZipcode"]);
                    fileData.ProviderZipcodeExt = Convert.ToString(dr["ProviderZipcodeExt"]);
                    fileData.ProviderPhoneNumber = Convert.ToString(dr["ProviderPhoneNumber"]);
                    fileData.LicenseNumber = Convert.ToString(dr["LicenseNumber"]);
                    fileData.DEANumber = Convert.ToString(dr["DEANumber"]);
                    fileData.EffectiveDate = dr["EffectiveDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["EffectiveDate"].ToString()) ? Convert.ToDateTime(dr["EffectiveDate"]).ToString(dateFormat) : "";
                    fileData.EndDate = dr["EndDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["EndDate"].ToString()) ? Convert.ToDateTime(dr["EndDate"]).ToString(dateFormat) : "";
                    fileData.ProviderProgramCode = Convert.ToString(dr["ProviderProgramCode"]);
                    fileData.ProviderEnrollmentDate = dr["ProviderEnrollmentDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["ProviderEnrollmentDate"].ToString()) ? Convert.ToDateTime(dr["ProviderEnrollmentDate"]).ToString(dateFormat) : "";
                    fileData.ProviderDisenrollmentDate = dr["ProviderDisenrollmentDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["ProviderDisenrollmentDate"].ToString()) ? Convert.ToDateTime(dr["ProviderDisenrollmentDate"]).ToString(dateFormat) : "";
                    fileData.EnrollmentStatusID = Convert.ToString(dr["EnrollmentStatusID"]);
                    fileData.ProviderTypeID = Convert.ToString(dr["ProviderTypeID"]);
                    fileData.ProviderSpecialtyID = Convert.ToString(dr["ProviderSpecialtyID"]);
                    fileData.SpecialtyEffectiveDate = dr["SpecialtyEffectiveDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["SpecialtyEffectiveDate"].ToString()) ? Convert.ToDateTime(dr["SpecialtyEffectiveDate"]).ToString(dateFormat) : "";
                    fileData.SpecialtyEndDate = dr["SpecialtyEndDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["SpecialtyEndDate"].ToString()) ? Convert.ToDateTime(dr["SpecialtyEndDate"]).ToString(dateFormat) : "";
                    fileData.ProviderNPI = Convert.ToString(dr["ProviderNPI"]);
                    fileData.NPIEffectiveDate = dr["NPIEffectiveDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["NPIEffectiveDate"].ToString()) ? Convert.ToDateTime(dr["NPIEffectiveDate"]).ToString(dateFormat) : "";
                    fileData.NPIEndDate = dr["NPIEndDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["NPIEndDate"].ToString()) ? Convert.ToDateTime(dr["NPIEndDate"]).ToString(dateFormat) : "";
                    fileData.CreationDate = Convert.ToDateTime(dr["CREATED_ON_DATE_TIME"]).ToString(dateFormat);

                    lst.Add(fileData);
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Provider File Report Job: Exception in DataTableToList() method. Error message: " + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
            }

            return lst;
        }
    }

    public class ProviderFileData
    {
        public int REG_ID { get; set; }
        public string SegmentIdentifier { get; set; }
        public string ProviderMedicaidID { get; set; }
        public string AddressType { get; set; }
        public string ProviderCounty { get; set; }
        public string ProviderName { get; set; }
        public string ProviderAddress1 { get; set; }
        public string ProviderAddress2 { get; set; }
        public string ProviderCity { get; set; }
        public string ProviderState { get; set; }
        public string ProviderZipcode { get; set; }
        public string ProviderZipcodeExt { get; set; }
        public string ProviderPhoneNumber { get; set; }
        public string LicenseNumber { get; set; }

        public string DEANumber { get; set; }
        public string EffectiveDate { get; set; }
        public string EndDate { get; set; }

        public string ProviderProgramCode { get; set; }
        public string ProviderEnrollmentDate { get; set; }
        public string ProviderDisenrollmentDate { get; set; }
        public string EnrollmentStatusID { get; set; }

        public string ProviderTypeID { get; set; }
        public string ProviderSpecialtyID { get; set; }
        public string SpecialtyEffectiveDate { get; set; }
        public string SpecialtyEndDate { get; set; }

        public string ProviderNPI { get; set; }
        public string NPIEffectiveDate { get; set; }
        public string NPIEndDate { get; set; }

        public string CreationDate { get; set; }
    }
}