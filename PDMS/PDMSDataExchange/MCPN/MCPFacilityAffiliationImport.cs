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
    public class MCPFacilityAffiliationImport : BaseJob, IJob
    {
        private const string thisGuidString = "EE397812-E103-4067-AE49-1EB1E5340AAE";
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

        public MCPFacilityAffiliationImport(Guid threadId, string guid = thisGuidString)
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

        override public void ExecuteJob(Guid jobId)
        {
            switch (jobId.ToString().ToUpper())
            {
                case weeklyGuidString:
                    ExportWeekly();
                    break;
                default:
                    this.ImportMCPFacilityAffiliation(string.Empty);
                    break;
            }
        }
        public void ExportWeekly()
        {
            string exportType = "FA";
            MCPShared.ExportWeeklyRecords("145", exportType);
            MCPShared.ExportWeeklyRecords("315", exportType);
            MCPShared.ExportWeeklyRecords("325", exportType);
            MCPShared.ExportWeeklyRecords("420", exportType);
            MCPShared.ExportWeeklyRecords("731", exportType);
            MCPShared.ExportWeeklyRecords("761", exportType);
        }
        public bool ImportMCPFacilityAffiliation(string fileMask)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            int filePrimaryKey;
            bool returnVal = false;

            try
            {
                log.CreateLogEntry(Constants.LogString.LoadingConfig, +logCnt);

                string fileWildcard = AppSettings.Get("MCP-FacilityAffiliationImportFileWildcard");
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
                        LoadMCPFacilityAffiliation(filePrimaryKey);
                        returnVal = true;
                    }
                    else
                    {
                        log.CreateLogEntry(String.Format(Constants.LogString.LoadingSkipped, file.Name), +logCnt);
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
        private void LoadMCPFacilityAffiliation(int fileId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            log.CreateLogEntry(String.Format("Loading MCPFacilityAffiliation data [File: {0}]", this.CurrentFile.Name), +logCnt);

            // open the file with FileHelper class and set internal variables
            MCPFacilityAffiliationRecord[] records;
            FileHelperEngine engine = new FileHelperEngine(typeof(MCPFacilityAffiliationRecord));

            int rowCounter = 0;

            try
            {
                engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                records = engine.ReadFile(this.CurrentFile.FullName) as MCPFacilityAffiliationRecord[];
                DateTime loadDateTime = DateTime.Now;
                string mcpDateFormat = AppSettings.Get("MCP-DateFormat");
                CultureInfo enUS = new CultureInfo(MCPShared.defaultCulture);
                //DateTimeStyles noStyle = DateTimeStyles.None;

                log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString()
                    , engine.ErrorManager.ErrorCount.ToString()), +logCnt);

                DataTable affiliation = new DataTable();
                affiliation.Columns.Add("CREATED_BY_USER", typeof(Guid));
                affiliation.Columns.Add("LAST_MODIFIED_USER", typeof(Guid));
                affiliation.Columns.Add("PG_RECORD_TYPE_ID", typeof(int));
                affiliation.Columns.Add("PRIMARY_MITS_Provider_Type_ID", typeof(int));
                affiliation.Columns.Add("SECONDARY_MITS_Provider_Type_ID", typeof(int));
                affiliation.Columns.Add("MCPN_FILE_ID", typeof(int));
                affiliation.Columns.Add("START_DATE", typeof(DateTime));
                affiliation.Columns.Add("END_DATE", typeof(DateTime));
                affiliation.Columns.Add("CREATED_ON_DATE_TIME", typeof(DateTime));
                affiliation.Columns.Add("LAST_MODIFIED_DATE_TIME", typeof(DateTime));
                affiliation.Columns.Add("GENDER", typeof(int));
                affiliation.Columns.Add("TRACKING_NUMBER", typeof(string));
                affiliation.Columns.Add("MEDICAID_ID", typeof(string));
                affiliation.Columns.Add("NPI", typeof(string));
                affiliation.Columns.Add("LICENSE_NUMBER", typeof(string));
                affiliation.Columns.Add("FIRST_NAME", typeof(string));
                affiliation.Columns.Add("MIDDLE_INITIAL", typeof(string));
                affiliation.Columns.Add("LAST_NAME", typeof(string));
                affiliation.Columns.Add("MCPN_Provider_Type_ID", typeof(string));
                affiliation.Columns.Add("PRIMARY_M_SPECIALTY_TYPE_ID", typeof(string));
                affiliation.Columns.Add("PRIMARY_SPECIALTY_TRACKING_NUMBER", typeof(string));
                affiliation.Columns.Add("PRIMARY_MITS_SPECIALTY_TYPE_ID", typeof(string));
                affiliation.Columns.Add("HOSPITAL_PRIVS_TRACKING_NUMBER", typeof(string));

                int counter = 0;
                // loop over all records and write to database
                foreach (MCPFacilityAffiliationRecord record in records)
                {

                    DataRow dataRow = affiliation.NewRow();
                    counter++;
                    rowCounter++;//used for exception handling
                    //get the error codes for the record
                    record.errors = MCPShared.ValidateObject(record);

                    try
                    {
                        // generated by sp_Admin_StoredProcBuilder on Oct 20 2020 10:00PM
                        // create parameters objects and fill with values
                        List<SqlParameter> parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                        parameters.Add(SqlParms.CreateParameter("MCO_RECORD_TYPE_CODE", DbType.Int32, record.recordType, false));
                        parameters.Add(SqlParms.CreateParameter("HOSPITAL_NUMBER", DbType.String, record.recordTypeNumber, true));
                        parameters.Add(SqlParms.CreateParameter("PROGRAM_CODE", DbType.Int32, record.programCode, false));
                        parameters.Add(SqlParms.CreateParameter("PANEL_CAPACITY", DbType.Int32, record.panelCapacity, true));
                        // ?? parameters.Add(SqlParms.CreateParameter("AFFILIATION_TYPE_ID", DbType.Int32, record, true));
                        // ?? parameters.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, record, true));
                        parameters.Add(SqlParms.CreateParameter("GENDER_ACCEPTED", DbType.Int32, record.gendersAccepted, true));
                        parameters.Add(SqlParms.CreateParameter("AGE_LIMIT_LOW", DbType.Int32, record.ageLimitLow, true));
                        parameters.Add(SqlParms.CreateParameter("AGE_LIMIT_HIGH", DbType.Int32, record.ageLimitHigh, true));
                        parameters.Add(SqlParms.CreateParameter("LANGUAGES_SPOKEN", DbType.Int32, record.languages, true));
                        parameters.Add(SqlParms.CreateParameter("PANEL_PCP_COUNT", DbType.Int32, record.panelPCPCount, true));
                        parameters.Add(SqlParms.CreateParameter("MITS_Provider_Type", DbType.Int32, record.mitsProviderType, true));
                        parameters.Add(SqlParms.CreateParameter("MCPN_Provider_Type", DbType.Int32, record.mcpnProviderType, true));
                        if (!string.IsNullOrWhiteSpace(record.startDate))
                            parameters.Add(SqlParms.CreateParameter("START_DATE", DbType.DateTime, MCPShared.GetDateFromString(record.startDate), false));

                        if (!string.IsNullOrWhiteSpace(record.endDate))
                            parameters.Add(SqlParms.CreateParameter("END_DATE", DbType.DateTime, MCPShared.GetDateFromString(record.endDate), true));

                        parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("IS_PCP", DbType.Boolean, record.isPCP, true));
                        parameters.Add(SqlParms.CreateParameter("EXISTING_PATIENTS_ONLY", DbType.Boolean, record.existingPatientsOnly, true));
                        parameters.Add(SqlParms.CreateParameter("ACCEPT_NEW_BORNS", DbType.Boolean, record.acceptNewborns, true));
                        parameters.Add(SqlParms.CreateParameter("ACCEPT_PREGNANT_WOMAN", DbType.Boolean, record.acceptPregnantWomen, true));
                        parameters.Add(SqlParms.CreateParameter("ACCEPT_FAMILY_MEMBERS", DbType.Boolean, record.acceptFamilyMembers, true));
                        parameters.Add(SqlParms.CreateParameter("TRACKING_NUMBER", DbType.String, record.TrackingNumber, false));
                        // ?? parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, record, true));
                        parameters.Add(SqlParms.CreateParameter("TPA_NAME", DbType.String, record.tpaName, true));
                        parameters.Add(SqlParms.CreateParameter("COMMENTS", DbType.String, record.comments, true));
                        parameters.Add(SqlParms.CreateParameter("MPN", DbType.String, record.mpn, true));
                        parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, record.npi, true));
                        parameters.Add(SqlParms.CreateParameter("Zip4", DbType.String, record.zip4, true));
                        parameters.Add(SqlParms.CreateParameter("County_Code", DbType.String, record.countyCode, true));
                        parameters.Add(SqlParms.CreateParameter("Phone", DbType.String, record.phone, true));
                        parameters.Add(SqlParms.CreateParameter("Name", DbType.String, record.name, true));
                        parameters.Add(SqlParms.CreateParameter("Address_Line_1", DbType.String, record.addressLine1, true));
                        parameters.Add(SqlParms.CreateParameter("Address_Line_2", DbType.String, record.addressLine2, true));
                        parameters.Add(SqlParms.CreateParameter("City", DbType.String, record.city, true));
                        parameters.Add(SqlParms.CreateParameter("State", DbType.String, record.state, true));
                        parameters.Add(SqlParms.CreateParameter("Zip", DbType.String, record.zip, true));
                        parameters.Add(SqlParms.CreateParameter("MCPN_FILE_ID", DbType.Int32, fileId, true));
                        parameters.Add(SqlParms.CreateParameter("MCPN_SPECIALTY", DbType.String, record.mcpnSpecialties, true));
                        parameters.Add(SqlParms.CreateParameter("MITS_SPECIALTY", DbType.String, record.mitsSpecialties, true));
                        

                        //get the error codes for the record
                        record.errors = MCPShared.ValidateObject(record);
                        parameters.Add(SqlParms.CreateParameter("ERROR_CODES", DbType.String, record.errors, true));
                        parameters.Add(SqlParms.CreateParameter("FILE_TYPE", DbType.String, "FA", true));
                        string newIdString = DataAccess.ExecuteScalar("usp_InsertREG_MCP_AFFILIATION", parameters);

                        if(!string.IsNullOrEmpty(newIdString))
                        {
                        List<SqlParameter> paras = new List<SqlParameter>();
                        paras.Add(SqlParms.CreateParameter("REG_MCP_AFFILIATION_ID", DbType.Int32, Convert.ToInt32(newIdString), true));
                        DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_MCP_AFFILIATION", paras,"ds");
                            if(ds.Tables[0].Rows.Count > 0)
                            {
                                record.errors = ds.Tables[0].Rows[0]["error_codes"].ToString();
                            }
                        }
                        SaveLanguages(newIdString, record.languages);
                        SaveSpecialties(newIdString, record.mcpnSpecialties, "MCPN");
                        SaveSpecialties(newIdString, record.mitsSpecialties, "MITS");

                        //if error codes from above check, write them to DB
                        if (!String.IsNullOrWhiteSpace(record.errors) && !String.IsNullOrWhiteSpace(newIdString))
                        {
                            List<SqlParameter> errorParams = new List<SqlParameter>();
                            parameters.Add(SqlParms.CreateParameter("REG_MCP_AFFILIATION_ID", DbType.Int32, newIdString, true));
                            parameters.Add(SqlParms.CreateParameter("ERROR_CODES", DbType.String, record.errors, true));
                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                            parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                            // DataAccess.ExecuteStoredProcedure("usp_InsertREG_MCP_AFFILIATION_MCPN_ERROR_CODE", errorParams);
                        }
                    }
                    catch (Exception ex)
                    {
                        MCPShared.LogFileRecordError(log, engine.LineNumber.ToString(), this.CurrentFile.Name, ex.Message);
                        // NOTE: Do *not* rethrow exception so the next record is processed
                    }
                }

                // record all errors generated by FileHelper
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        MCPShared.LogFileRecordError(log, engine.LineNumber.ToString(), this.CurrentFile.Name, err.ExceptionInfo.ToString());
                    }
                }

                ExportMCPFacilityAffiliation(records);
            }
            catch (Exception ex)
            {
                MCPShared.LogFileRecordError(log, engine.LineNumber.ToString(), this.CurrentFile.Name, Constants.LogString.FileLoadFailure + ex.Message);
                // NOTE: Do *not* rethrow exception so the next file is processed
            }

            log.CreateLogEntry(String.Format("MCPFacilityAffiliation [File: {0}] load complete", this.CurrentFile.Name), +logCnt);
        }
        private void SaveLanguages(string newId, string languageValue)
        {
            string fieldDelimiter = AppSettings.Get("MCP-FieldDelimiter");
            char delimiter = Convert.ToChar(fieldDelimiter);

            string[] languages = languageValue.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string language in languages)
            {
                // generated by sp_Admin_StoredProcBuilder on Oct 26 2020  2:21PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));
                parameters.Add(SqlParms.CreateParameter("REG_MCP_AFFILIATION_ID", DbType.Int32, newId, false));
                parameters.Add(SqlParms.CreateParameter("Language_Code", DbType.Int32, language.ToString(), false));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));

                DataAccess.ExecuteStoredProcedure("usp_InsertREG_MCP_AFFILIATION_LANGUAGE", parameters);
            }
        }

        private void SaveSpecialties(string newId, string specialtiesValue, string specialtyType)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            string sValue = string.Empty;

            try
            {
                string fieldDelimiter = AppSettings.Get("MCP-FieldDelimiter");
                char delimiter = Convert.ToChar(fieldDelimiter);

                string[] specialties = specialtiesValue.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string specialty in specialties)
                {
                    sValue = specialty.ToString();

                    // remove board certified flag
                    if (sValue.Length == 4 && sValue.Substring((sValue.Length - 1), 1) == MCPShared.specialtyBoardCertifiedIndicator)
                    {
                        sValue = sValue.Substring(0, (sValue.Length - 1));
                    }

                    // generated by sp_Admin_StoredProcBuilder on Oct 26 2020  6:01PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));
                    parameters.Add(SqlParms.CreateParameter("REG_MCP_AFFILIATION_ID", DbType.Int32, newId, false));
                    parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE", DbType.String, sValue, false));
                    parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LOAD_TYPE", DbType.String, specialtyType, false));

                    DataAccess.ExecuteStoredProcedure("usp_InsertREG_MCP_AFFILIATION_SPECIALTY", parameters);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(MCPShared.invalidValueDBError))
                {
                    log.CreateLogEntry(String.Format("Specialty does not map [File={0}, REG_MCP_AFFILIATION_ID={1}, Specialty={2}]"
                            , this.CurrentFile.Name, newId, sValue), Logging.LogPriority.DataLoadIssues, +logCnt);
                }
                else
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }
        }

        private void ExportMCPFacilityAffiliation(MCPFacilityAffiliationRecord[] records)
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

                //zero byte files return
                if (records.Length == 0)
                    fi.Create();

                FileHelpers.DelimitedFileEngine engine = new FileHelpers.DelimitedFileEngine(typeof(MCPFacilityAffiliationRecord));

                //  loop over all records and append to file
                foreach (MCPFacilityAffiliationRecord record in records)
                {
                    try
                    {
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
