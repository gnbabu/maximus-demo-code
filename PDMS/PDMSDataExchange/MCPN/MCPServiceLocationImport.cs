using FileHelpers;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MAXIMUS.DataExchange.PDMS.MCPN
{
    public class MCPServiceLocationImport : BaseJob, IJob
    {

        private const string thisGuidString = "C3C16983-98AA-4611-8795-D9B79BEA54CA";
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

        public MCPServiceLocationImport(Guid threadId, string guid = thisGuidString)
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
                    this.ImportMCPServiceLocation(string.Empty);
                    break;
            }
        }
        public void ExportWeekly()
        {
            string exportType = "SL";
            MCPShared.ExportWeeklyRecords("145", exportType);
            MCPShared.ExportWeeklyRecords("315", exportType);
            MCPShared.ExportWeeklyRecords("325", exportType);
            MCPShared.ExportWeeklyRecords("420", exportType);
            MCPShared.ExportWeeklyRecords("731", exportType);
            MCPShared.ExportWeeklyRecords("761", exportType);
        }
        public bool ImportMCPServiceLocation(string fileMask)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            int filePrimaryKey;
            bool returnVal = false;

            try
            {
                log.CreateLogEntry(Constants.LogString.LoadingConfig, +logCnt);

                string fileWildcard = AppSettings.Get("MCP-ServiceLocationImportFileWildcard");
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
                        LoadMCPServiceLocation(filePrimaryKey);
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
        private void LoadMCPServiceLocation(int fileId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            log.CreateLogEntry(String.Format("Loading MCPServiceLocation data [File: {0}]", this.CurrentFile.Name), +logCnt);

            // open the file with FileHelper class and set internal variables
            MCPServiceLocationRecord[] records;
            FileHelperEngine engine = new FileHelperEngine(typeof(MCPServiceLocationRecord));

            int counter1 = 0;
            try
            {
                engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                records = engine.ReadFile(this.CurrentFile.FullName) as MCPServiceLocationRecord[];
                DateTime loadDateTime = DateTime.Now;
                string mcpDateFormat = AppSettings.Get("MCP-DateFormat");
                CultureInfo enUS = new CultureInfo(MCPShared.defaultCulture);
                //DateTimeStyles noStyle = DateTimeStyles.None;

                log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString()
                    , engine.ErrorManager.ErrorCount.ToString()), +logCnt);

                DataTable affiliationDetails = new DataTable();
                affiliationDetails.Columns.Add("CREATED_BY_USER", typeof(Guid));
                affiliationDetails.Columns.Add("LAST_MODIFIED_USER", typeof(Guid));
                affiliationDetails.Columns.Add("REG_MCP_PG_AFFILIATION_ID", typeof(int));
                affiliationDetails.Columns.Add("MCO_RECORD_TYPE_CODE", typeof(int));
                affiliationDetails.Columns.Add("PROGRAM_CODE", typeof(int));
                affiliationDetails.Columns.Add("PANEL_CAPACITY", typeof(int));
                affiliationDetails.Columns.Add("GENDER_ACCEPTED", typeof(int));
                affiliationDetails.Columns.Add("AGE_LIMIT_LOW", typeof(int));
                affiliationDetails.Columns.Add("AGE_LIMIT_HIGH", typeof(int));
                affiliationDetails.Columns.Add("Full_Time_Equivalency", typeof(int));
                affiliationDetails.Columns.Add("PANEL_PCP_COUNT", typeof(int));
                affiliationDetails.Columns.Add("MODIFIED_STATUS_TYPE_ID", typeof(int));
                affiliationDetails.Columns.Add("MCPN_FILE_ID", typeof(int));
                affiliationDetails.Columns.Add("START_DATE", typeof(DateTime));
                affiliationDetails.Columns.Add("END_DATE", typeof(DateTime));
                affiliationDetails.Columns.Add("CREATED_ON_DATE_TIME", typeof(DateTime));
                affiliationDetails.Columns.Add("LAST_MODIFIED_DATE_TIME", typeof(DateTime));
                affiliationDetails.Columns.Add("IS_PCP", typeof(bool));
                affiliationDetails.Columns.Add("EXISTING_PATIENTS_ONLY", typeof(bool));
                affiliationDetails.Columns.Add("ACCEPT_NEW_BORNS", typeof(bool));
                affiliationDetails.Columns.Add("ACCEPT_PREGNANT_WOMAN", typeof(bool));
                affiliationDetails.Columns.Add("ACCEPT_FAMILY_MEMBERS", typeof(bool));
                affiliationDetails.Columns.Add("Twenty_Four_Hour_availability", typeof(bool));
                affiliationDetails.Columns.Add("TRACKING_NUMBER", typeof(string));
                affiliationDetails.Columns.Add("PROVIDER_GROUP_TRACKING_NUMBER", typeof(string));
                affiliationDetails.Columns.Add("HEALTH_CENTER_TRACKING_NUMBER", typeof(string));
                affiliationDetails.Columns.Add("GROUP_LOCATION_TRACKING_NUMBER", typeof(string));
                affiliationDetails.Columns.Add("NPI", typeof(string));
                affiliationDetails.Columns.Add("Address_Line_1", typeof(string));
                affiliationDetails.Columns.Add("Phone", typeof(string));
                affiliationDetails.Columns.Add("Phone_Extension", typeof(string));
                affiliationDetails.Columns.Add("TPA_NAME", typeof(string));
                affiliationDetails.Columns.Add("COMMENTS", typeof(string));
                affiliationDetails.Columns.Add("Address_Line_2", typeof(string));
                affiliationDetails.Columns.Add("City", typeof(string));
                affiliationDetails.Columns.Add("State", typeof(string));
                affiliationDetails.Columns.Add("Zip", typeof(string));
                affiliationDetails.Columns.Add("Zip4", typeof(string));
                affiliationDetails.Columns.Add("County_Code", typeof(string));
                affiliationDetails.Columns.Add("ERROR_CODES", typeof(string));
                affiliationDetails.Columns.Add("language_Code", typeof(string));
                affiliationDetails.Columns.Add("specialtiesAndBoardCertified", typeof(string));
                affiliationDetails.Columns.Add("mitsSpecialties", typeof(string));

                int counter = 0;
                // loop over all records and write to database
                foreach (MCPServiceLocationRecord record in records)
                {
                    DataRow dataRow = affiliationDetails.NewRow();
                    counter++;
                    counter1++;//used for exception handling
                    //get the error codes for the record
                    record.errors = MCPShared.ValidateObject(record);

                    // generated by sp_Admin_StoredProcBuilder on Oct 22 2020  7:07PM
                    // create parameters objects and fill with values
                    dataRow["CREATED_BY_USER"] = Constants.appPDMSDataExchangeUserId;
                    dataRow["LAST_MODIFIED_USER"] = Constants.appPDMSDataExchangeUserId;
                    dataRow["REG_MCP_PG_AFFILIATION_ID"] = 0;
                    dataRow["MCO_RECORD_TYPE_CODE"] = record.recordType;
                    if (!String.IsNullOrEmpty(record.programCode.Trim()))
                        dataRow["PROGRAM_CODE"] = record.programCode;
                    if(!String.IsNullOrEmpty(record.panelCapacity.Trim()) )
                        dataRow["PANEL_CAPACITY"] = record.PanelCapacity;
                    if (!String.IsNullOrEmpty(record.gendersAccepted.Trim()))
                        dataRow["GENDER_ACCEPTED"] = record.gendersAccepted;
                    if (!String.IsNullOrEmpty(record.ageLimitLow.Trim()))
                        dataRow["AGE_LIMIT_LOW"] = record.ageLimitLow;
                    if (!String.IsNullOrEmpty(record.ageLimitHigh.Trim()))
                        dataRow["AGE_LIMIT_HIGH"] = record.ageLimitHigh;
                    if (!String.IsNullOrEmpty(record.fulltimeEquivalency.Trim()))
                        dataRow["Full_Time_Equivalency"] = record.fulltimeEquivalency;
                    if (!String.IsNullOrEmpty(record.panelPCPCount.Trim()))
                        dataRow["PANEL_PCP_COUNT"] = record.panelPCPCount;
                    dataRow["MODIFIED_STATUS_TYPE_ID"] = DBNull.Value;
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

                    if (!String.IsNullOrEmpty(record.isPCP.Trim()))
                        dataRow["IS_PCP"] = Convert.ToBoolean(Convert.ToInt16(record.isPCP));
                    if (!String.IsNullOrEmpty(record.existingPatientsOnly.Trim()))
                        dataRow["EXISTING_PATIENTS_ONLY"] = Convert.ToBoolean(Convert.ToInt16(record.existingPatientsOnly));
                    if (!String.IsNullOrEmpty(record.AcceptNewborns.Trim()))
                        dataRow["ACCEPT_NEW_BORNS"] = Convert.ToBoolean(Convert.ToInt16(record.AcceptNewborns));
                    if (!String.IsNullOrEmpty(record.acceptPregnantWomen.Trim()))
                        dataRow["ACCEPT_PREGNANT_WOMAN"] = Convert.ToBoolean(Convert.ToInt16(record.acceptPregnantWomen));
                    if (!String.IsNullOrEmpty(record.acceptFamilyMembers.Trim()))
                        dataRow["ACCEPT_FAMILY_MEMBERS"] = Convert.ToBoolean(Convert.ToInt16(record.acceptFamilyMembers));
                    if (!String.IsNullOrEmpty(record.availability24Hours.Trim()))
                        dataRow["Twenty_Four_Hour_availability"] = Convert.ToBoolean(Convert.ToInt16(record.availability24Hours));

                    dataRow["TRACKING_NUMBER"] = record.trackingNumber?.PadRight(20).Substring(0,20);
                    dataRow["PROVIDER_GROUP_TRACKING_NUMBER"] = record.providerGroupTrackingNumber?.PadRight(20).Substring(0, 20);
                    dataRow["HEALTH_CENTER_TRACKING_NUMBER"] = record.healthCenterTrackingNumber?.PadRight(20).Substring(0, 20);
                    dataRow["GROUP_LOCATION_TRACKING_NUMBER"] = record.groupLocationTrackingNumber?.PadRight(20).Substring(0, 20);
                    dataRow["NPI"] = record.npiNumber?.PadRight(10).Substring(0, 10);
                    dataRow["Address_Line_1"] = record.addressLine1?.PadRight(100).Substring(0, 100);
                    dataRow["Phone"] = record.phone?.PadRight(20).Substring(0, 20);
                    dataRow["Phone_Extension"] = record.phoneExtension?.PadRight(20).Substring(0, 20);
                    dataRow["TPA_NAME"] = record.tpaName?.PadRight(200).Substring(0, 200);
                    dataRow["COMMENTS"] = record.comments?.PadRight(256).Substring(0, 256);
                    dataRow["Address_Line_2"] = record.addressLine2?.PadRight(100).Substring(0, 100);
                    dataRow["City"] = record.city?.PadRight(100).Substring(0, 100);
                    dataRow["State"] = record.state?.PadRight(100).Substring(0, 100);
                    dataRow["Zip"] = record.zip?.PadRight(5).Substring(0, 5);
                    dataRow["Zip4"] = record.zip4?.PadRight(4).Substring(0, 4);
                    dataRow["County_Code"] = record.countyCode?.PadRight(5).Substring(0, 5);
                    dataRow["ERROR_CODES"] = record.errors?.PadRight(1024).Substring(0, 1024);
                    dataRow["language_Code"] = record.languages;
                    dataRow["specialtiesAndBoardCertified"] = record.specialtiesAndBoardCertified?.PadRight(4).Substring(0, 4);
                    dataRow["mitsSpecialties"] = record.mitsSpecialties?.PadRight(4).Substring(0, 4);

                    affiliationDetails.Rows.Add(dataRow);
                    if (counter >= 2500)
                    try
                    {
                       log.CreateLogEntry(String.Format("Bulk inserting MCPServiceLocation data [Count: {0}]", counter), +logCnt);
                       counter = 0;
                       CallBulkInsert(affiliationDetails); 
                       log.CreateLogEntry(String.Format("Bulk finished MCPServiceLocation data", counter), +logCnt);
                       affiliationDetails.Clear();
                    }
                    catch (Exception ex)
                    {
                        MCPShared.LogFileRecordError(log, counter1.ToString(), this.CurrentFile.Name, ex.Message);
                        // NOTE: Do *not* rethrow exception so the next record is processed
                    }
                }

                if (counter > 0)
                {
                    log.CreateLogEntry(String.Format("Bulk inserting MCPServiceLocation data [Count: {0}]", counter), +logCnt);
                    CallBulkInsert(affiliationDetails);
                    log.CreateLogEntry(String.Format("Bulk finished MCPServiceLocation data", counter), +logCnt);
                }
                // record all errors generated by FileHelper
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        MCPShared.LogFileRecordError(log, engine.LineNumber.ToString(), this.CurrentFile.Name, err.ExceptionInfo.ToString());
                    }
                }

                ExportMCPServiceLocation(records, fileId);
            }
            catch (Exception ex)
            {
                MCPShared.LogFileRecordError(log, counter1.ToString(), this.CurrentFile.Name, Constants.LogString.FileLoadFailure + ex.Message);
                // NOTE: Do *not* rethrow exception so the next file is processed
            }

            log.CreateLogEntry(String.Format("MCPServiceLocation [File: {0}] load complete", this.CurrentFile.Name), +logCnt);
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
            DataAccess.ExecuteScalar("USP_SAVE_REG_MCP_PG_AFFILIATION_DETAIL_TYPE", parameters);
        }
        private void SaveLanguages(string newId, string languageValue)
        {
            try
            {
                string fieldDelimiter = AppSettings.Get("MCP-FieldDelimiter");
                char delimiter = Convert.ToChar(fieldDelimiter);

                string[] languages = languageValue.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string language in languages)
                {
                    // generated by sp_Admin_StoredProcBuilder on Oct 26 2020  2:45PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));
                    parameters.Add(SqlParms.CreateParameter("REG_MCP_PG_AFFILIATION_DETAIL_ID", DbType.Int32, newId, false));
                    parameters.Add(SqlParms.CreateParameter("Language_Code", DbType.Int32, language.ToString(), false));
                    parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));

                    DataAccess.ExecuteStoredProcedure("usp_InsertREG_MCP_PG_AFFILIATION_DETAIL_LANGUAGE", parameters);

                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }
        private void SaveSpecialties(string newId, string specialtiesValue)
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
                    parameters.Add(SqlParms.CreateParameter("REG_MCP_PG_AFFILIATION_DETAIL_ID", DbType.Int32, newId, false));
                    parameters.Add(SqlParms.CreateParameter("SPECIALTY_TYPE", DbType.String, sValue, false));
                    parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));

                    DataAccess.ExecuteStoredProcedure("usp_InsertREG_MCP_PG_AFFILIATION_DETAIL_SPECIALTY", parameters);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(MCPShared.invalidValueDBError))
                {
                    log.CreateLogEntry(String.Format("Specialty does not map [File={0}, REG_MCP_PG_AFFILIATION_DETAIL_ID={1}, Specialty={2}]"
                            , this.CurrentFile.Name, newId, sValue), Logging.LogPriority.DataLoadIssues, +logCnt);
                }
                else
                {
                    throw CoreException.ThrowException(this.ThreadId, ex);
                }
            }
        }

        private void ExportMCPServiceLocation(MCPServiceLocationRecord[] records,int fileId)
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

                FileHelpers.DelimitedFileEngine engine = new FileHelpers.DelimitedFileEngine(typeof(MCPServiceLocationRecord));
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(SqlParms.CreateParameter("fileId", DbType.Int32, fileId, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_MCP_PG_AFFILIATION_DETAIL", paras, "ds");
                //  loop over all records and append to file
                foreach (MCPServiceLocationRecord record in records)
                {
                    try
                    {

                        if (!string.IsNullOrEmpty(record.trackingNumber) && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drPending = ds.Tables[0].Select("tracking_number=" + record.trackingNumber);
                            if (drPending != null && drPending.Length > 0)
                            {
                                record.errors = drPending[0]["error_codes"].ToString().Trim();
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
