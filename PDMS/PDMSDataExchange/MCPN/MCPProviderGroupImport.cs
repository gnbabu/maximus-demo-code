using FileHelpers;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.MCPN
{
    public class MCPProviderGroupImport : BaseJob, IJob
    {

        private const string thisGuidString = "971FE58A-8F21-46D9-BF4F-E610550A5FCC";
        private const string weeklyGuidString = "2D80C32C-D971-49D7-90F7-69319A71E815";
        private Guid thisGuid = new Guid(thisGuidString);

        FileInfo _currentFile;

        FileInfo CurrentFile
        {
            get
            {
                return _currentFile;
            }
            set
            {
                _currentFile = value;
            }
        }

        string _planId;

        String PlanId
        {
            get
            {
                return _planId;
            }
            set
            {
                _planId = value;
            }
        }

        #region "Constructors"

        public MCPProviderGroupImport(Guid threadId, string guid = thisGuidString)
            : base(threadId)
        {
            this.ThreadId = threadId;
            this.thisGuid = new Guid(guid);
        }

        #endregion

        #region "Logging Objects"

        private int logCnt = 0;

        #endregion

        #region "Public Methods"

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(thisGuid);
        }
        public void ExportWeekly()
        {
            string exportType = "PG";
            MCPShared.ExportWeeklyRecords("145", exportType);
            MCPShared.ExportWeeklyRecords("315", exportType);
            MCPShared.ExportWeeklyRecords("325", exportType);
            MCPShared.ExportWeeklyRecords("420", exportType);
            MCPShared.ExportWeeklyRecords("731", exportType);
            MCPShared.ExportWeeklyRecords("761", exportType);
        }
        override public void ExecuteJob(Guid jobId)
        {
            switch (jobId.ToString().ToUpper())
            {
                case weeklyGuidString:
                    ExportWeekly();
                    break;
                default:
                    this.ImportMCPProviderGroupImport(string.Empty);
                    break;
            }
        }

        public bool ImportMCPProviderGroupImport(string fileMask)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            int filePrimaryKey;
            bool returnVal = false;

            try
            {
                log.CreateLogEntry(Constants.LogString.LoadingConfig, +logCnt);

                string fileWildcard = AppSettings.Get("MCP-ProviderGroupImportFileWildcard");
                fileWildcard = String.Format(fileWildcard, fileMask);
                DirectoryInfo localDirectory;
                string localPath = AppSettings.Get("MCP-ImportLocalPath");
                localDirectory = new DirectoryInfo(localPath);

                log.CreateLogEntry(String.Format(Constants.LogString.FileCountToProcess, localDirectory.GetFiles(fileWildcard).Length));

                // interate over the downloaded files
                foreach (FileInfo file in localDirectory.GetFiles(fileWildcard))
                {
                    this.CurrentFile = file;
                    this.PlanId = MCPShared.GetPlanIdFromFileName(this.ThreadId, file);

                    filePrimaryKey = MCPShared.FileImported(this.ThreadId, file);

                    if (filePrimaryKey != 0)
                    {
                        log.CreateLogEntry(String.Format(Constants.LogString.LoadingFile, file.Name));
                        LoadMCPProviderGroupImport(filePrimaryKey);
                        returnVal = true;
                    }
                    else
                    {
                        log.CreateLogEntry(String.Format("Skipping load of file [{0}], loaded previously", file.Name), +logCnt);
                    }

                    // move the file 
                    string planId = MCPShared.GetPlanIdFromFileName(this.ThreadId, file);
                    string archiveDirectory = localPath + @"Archive\" + String.Format(MCPShared.directoryFormat, planId);
                    Directory.CreateDirectory(archiveDirectory);
                    if (File.Exists(archiveDirectory + file.Name))
                    {
                        File.Delete(archiveDirectory + file.Name);
                    }
                    file.MoveTo(archiveDirectory + file.Name);
                }
            }
            catch (Exception ex)
            {
                //string fullMessage = "File action failed. File Name: {0} Reason: {1}";
                //log.CreateLogEntry(String.Format(fullMessage, fileName, ex.Message), Logging.LogPriority.Error, +logCnt);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete, +logCnt);

            return returnVal;
        }
        #endregion

        #region "Private Methods"
        private void LoadMCPProviderGroupImport(int fileId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            int rowCounter = 0;

            log.CreateLogEntry(String.Format("Loading MCPProviderGroupImport data [File: {0}]", this.CurrentFile.Name), +logCnt);

            // open the file with FileHelper class and set internal variables
            MCPProviderGroupRecord[] records;
            FileHelperEngine engine = new FileHelperEngine(typeof(MCPProviderGroupRecord));

            try
            {
                engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                records = engine.ReadFile(this.CurrentFile.FullName) as MCPProviderGroupRecord[];
                DateTime loadDateTime = DateTime.Now;
                string mcpDateFormat = AppSettings.Get("MCP-DateFormat");
                CultureInfo enUS = new CultureInfo(MCPShared.defaultCulture);
                //DateTimeStyles noStyle = DateTimeStyles.None;

                log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString()
                    , engine.ErrorManager.ErrorCount.ToString()), +logCnt);
                log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString()
                    , engine.ErrorManager.ErrorCount.ToString()), +logCnt);

                DataTable affiliations = new DataTable();
                affiliations.Columns.Add("CREATED_BY_USER", typeof(Guid));
                affiliations.Columns.Add("LAST_MODIFIED_USER", typeof(Guid));
                affiliations.Columns.Add("PG_RECORD_TYPE_ID", typeof(int));
                affiliations.Columns.Add("PRIMARY_MITS_Provider_Type_ID", typeof(int));
                affiliations.Columns.Add("SECONDARY_MITS_Provider_Type_ID", typeof(int));
                affiliations.Columns.Add("MCPN_FILE_ID", typeof(int));
                affiliations.Columns.Add("START_DATE", typeof(DateTime));
                affiliations.Columns.Add("END_DATE", typeof(DateTime));
                affiliations.Columns.Add("CREATED_ON_DATE_TIME", typeof(DateTime));
                affiliations.Columns.Add("LAST_MODIFIED_DATE_TIME", typeof(DateTime));
                affiliations.Columns.Add("GENDER", typeof(int));
                affiliations.Columns.Add("TRACKING_NUMBER", typeof(string));
                affiliations.Columns.Add("MEDICAID_ID", typeof(string));
                affiliations.Columns.Add("NPI", typeof(string));
                affiliations.Columns.Add("LICENSE_NUMBER", typeof(string));
                affiliations.Columns.Add("FIRST_NAME", typeof(string));
                affiliations.Columns.Add("MIDDLE_INITIAL", typeof(string));
                affiliations.Columns.Add("LAST_NAME", typeof(string));
                affiliations.Columns.Add("MCPN_Provider_Type_ID", typeof(string));
                affiliations.Columns.Add("PRIMARY_M_SPECIALTY_TYPE_ID", typeof(string));
                affiliations.Columns.Add("PRIMARY_SPECIALTY_TRACKING_NUMBER", typeof(string));
                affiliations.Columns.Add("PRIMARY_MITS_SPECIALTY_TYPE_ID", typeof(string));
                affiliations.Columns.Add("HOSPITAL_PRIVS_TRACKING_NUMBER", typeof(string));
                affiliations.Columns.Add("ERROR_CODES", typeof(string));

                int counter = 0;
                // loop over all records and write to database
                foreach (MCPProviderGroupRecord record in records)
                {

                    try
                    {
                        DataRow dataRow = affiliations.NewRow();
                        counter++;
                        rowCounter++;//used for exception handling
                        //get the error codes for the record
                        record.errors = MCPShared.ValidateObject(record);
                        dataRow["CREATED_BY_USER"] = Constants.appPDMSDataExchangeUserId;
                        dataRow["LAST_MODIFIED_USER"] = Constants.appPDMSDataExchangeUserId;
                        dataRow["PG_RECORD_TYPE_ID"] = record.recordType;
                        if (!String.IsNullOrEmpty(record.primaryMITSProviderType.Trim()))
                            dataRow["PRIMARY_MITS_Provider_Type_ID"] = record.primaryMITSProviderType;
                        if (!String.IsNullOrEmpty(record.secondaryMITSProviderType.Trim()))
                            dataRow["SECONDARY_MITS_Provider_Type_ID"] = record.secondaryMITSProviderType;
                        dataRow["MCPN_FILE_ID"] = fileId;

                        if (!string.IsNullOrWhiteSpace(record.startDate.Trim()))
                            dataRow["START_DATE"] = MCPShared.GetDateFromString(record.startDate);
                        else
                            dataRow["START_DATE"] = DateTime.Now;

                        if (!string.IsNullOrWhiteSpace(record.endDate.Trim()))
                            dataRow["END_DATE"] = MCPShared.GetDateFromString(record.endDate);
                        else
                            dataRow["END_DATE"] = DBNull.Value;

                        dataRow["CREATED_ON_DATE_TIME"] = DateTime.Now;
                        dataRow["LAST_MODIFIED_DATE_TIME"] = DateTime.Now;

                        if (!string.IsNullOrWhiteSpace(record.genderCode.Trim()))
                            dataRow["GENDER"] = record.genderCode;
                        dataRow["TRACKING_NUMBER"] = record.trackingNumber?.PadRight(20).Substring(0, 20);
                        dataRow["MEDICAID_ID"] = record.mpn?.PadRight(20).Substring(0, 20);
                        dataRow["NPI"] = record.npi?.PadRight(10).Substring(0, 10);
                        dataRow["LICENSE_NUMBER"] = record.licenseNumber?.PadRight(20).Substring(0, 20);
                        dataRow["FIRST_NAME"] = record.firstName?.PadRight(50).Substring(0, 50);
                        dataRow["MIDDLE_INITIAL"] = record.middleInitial?.PadRight(50).Substring(0, 50);
                        dataRow["LAST_NAME"] = record.lastName?.PadRight(50).Substring(0, 50);
                        dataRow["MCPN_Provider_Type_ID"] = record.providerType?.PadRight(3).Substring(0, 3);
                        dataRow["PRIMARY_M_SPECIALTY_TYPE_ID"] = record.primarySpecialty?.PadRight(3).Substring(0, 3);
                        dataRow["PRIMARY_SPECIALTY_TRACKING_NUMBER"] = record.primarySpecialtyTrackingNumber?.PadRight(11).Substring(0, 11);
                        dataRow["PRIMARY_MITS_SPECIALTY_TYPE_ID"] = record.PrimaryMITSSpecialty?.PadRight(3).Substring(0, 3);
                        dataRow["HOSPITAL_PRIVS_TRACKING_NUMBER"] = record.hospitalPrivilegesTrackingNumber;

                        affiliations.Rows.Add(dataRow);
                        if (counter >= 2500) 
                        { 

                            try
                            {
                                log.CreateLogEntry(String.Format("Bulk inserting MCPProviderGroup data [Count: {0}]", counter), +logCnt);
                                counter = 0;
                                CallBulkInsert(affiliations);
                                log.CreateLogEntry(String.Format("Bulk finished MCPProviderGroup data [Count: {0}]", counter), +logCnt);
                                affiliations.Clear();
                            }
                            catch (Exception ex)
                            {
                                MCPShared.LogFileRecordError(log, rowCounter.ToString(), this.CurrentFile.Name, ex.Message);
                                // NOTE: Do *not* rethrow exception so the next record is processed
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MCPShared.LogFileRecordError(log, rowCounter.ToString(), this.CurrentFile.Name, ex.Message);
                        // NOTE: Do *not* rethrow exception so the next record is processed
                    }
                }


                if (counter > 0)
                {
                    log.CreateLogEntry(String.Format("Bulk inserting MCPProviderGroup data [Count: {0}]", counter), +logCnt);
                    CallBulkInsert(affiliations);
                }
                // record all errors generated by FileHelper
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        MCPShared.LogFileRecordError(log, engine.LineNumber.ToString(), this.CurrentFile.Name, err.ExceptionInfo.ToString());
                    }
                }

                ExportMCPProviderGroup(records, fileId);

            }
            catch (Exception ex)
            {
                MCPShared.LogFileRecordError(log, engine.LineNumber.ToString(), this.CurrentFile.Name, Constants.LogString.FileLoadFailure + ex.Message);
                // NOTE: Do *not* rethrow exception so the next file is processed
            }

            log.CreateLogEntry(String.Format("MCPProviderGroupImport [File: {0}] load complete", this.CurrentFile.Name), +logCnt);
        }
    private void CallBulkInsert(DataTable affiliationDetails)
    {

        //Call the bulk insert 
        List<SqlParameter> parameters = new List<SqlParameter>();
        SqlParameter param = new SqlParameter();
        param.ParameterName = "Affiliation_details";
        param.SqlDbType = SqlDbType.Structured;
        param.Value = affiliationDetails;
        param.Direction = ParameterDirection.Input;
        parameters.Add(param);
        DataAccess.ExecuteScalar("USP_SAVE_REG_MCP_PG_AFFILIATION_TYPE", parameters);
    }
    private void SaveHospitalPrivileges(string newId, string hospitalPrivilegesValue)
        {
            string fieldDelimiter = AppSettings.Get("MCP-FieldDelimiter");
            char delimiter = Convert.ToChar(fieldDelimiter);

            string[] hospitalPrivileges = hospitalPrivilegesValue.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string hospitalPrivilege in hospitalPrivileges)
            {
                // generated by sp_Admin_StoredProcBuilder on Oct 26 2020  2:39PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));
                parameters.Add(SqlParms.CreateParameter("REG_MCP_PG_AFFILIATION_ID", DbType.Int32, newId, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("TRACKING_NUMBER", DbType.String, hospitalPrivilege.ToString(), false));

                DataAccess.ExecuteStoredProcedure("usp_InsertREG_MCP_PG_AFFILIATION_HOSPITAL_PRIVILEGES", parameters);
            }
        }

        private void ExportMCPProviderGroup(MCPProviderGroupRecord[] records, int fileId)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                DirectoryInfo localDirectory;
                string localPath = AppSettings.Get("MCP-ExportLocalPath");
                localDirectory = new DirectoryInfo(String.Format(localPath, this.PlanId));

                if (!localDirectory.Exists)
                {
                    localDirectory.Create();
                }

                string exportFile = "";
                if (this.CurrentFile.Extension == MCPShared.defaultMCPExtension)
                {
                    exportFile = this.CurrentFile.Name.Replace(MCPShared.defaultMCPExtension, MCPShared.defaultResponseExtension);
                }
                else if (this.CurrentFile.Extension == MCPShared.defaultMCPExtensionUpperCase)
                {
                    exportFile = this.CurrentFile.Name.Replace(MCPShared.defaultMCPExtensionUpperCase, MCPShared.defaultResponseExtension);
                }

                string fullExportFileName = Path.Combine(localDirectory.FullName, exportFile);

                FileInfo fi = new FileInfo(fullExportFileName);
                fi.Delete();
                //zero byte files return
                if (records.Length == 0)
                    fi.Create();

                FileHelpers.DelimitedFileEngine engine = new FileHelpers.DelimitedFileEngine(typeof(MCPProviderGroupRecord));
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(SqlParms.CreateParameter("fileId", DbType.Int32, fileId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_MCP_PG_AFFILIATION", paras, "ds");
                //  loop over all records and append to file
                foreach (MCPProviderGroupRecord record in records)
                {
                    try
                    {

                        if (!String.IsNullOrWhiteSpace(record.errors))
                            record.errors = record.errors;


                        if (!string.IsNullOrEmpty(record.trackingNumber) && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drPending = ds.Tables[0].Select("tracking_number=" + record.trackingNumber);
                            if (drPending != null && drPending.Length > 0)
                            {
                                record.errors = drPending[0]["error_codes"].ToString();
                            }
                        }
                        //  append the record to the file
                        engine.AppendToFile(fullExportFileName, record);
                    }
                    catch (Exception ex)
                    {
                        MCPShared.LogFileRecordError(log, engine.LineNumber.ToString(), exportFile, ex.Message);
                        // NOTE: Do *not* rethrow exception so the next record is processed
                    }
                }
                // record all bad errors
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        MCPShared.LogFileRecordError(log, engine.LineNumber.ToString(), exportFile, err.ExceptionInfo.ToString());
                    }
                }

                log.CreateLogEntry(String.Format(Constants.LogString.FileGenerated, exportFile), +logCnt);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }

        }

        #endregion

    }
}
