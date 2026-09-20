using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;

namespace MAXIMUS.DataExchange.PDMS.MCPN
{
    public static class MCPShared
    {
        internal const string dateFormat = "yyyyMMdd";
        internal const string defaultBoolString = "false";
        internal const string defaultMCPExtension = ".mcp";
        internal const string defaultMCPExtensionUpperCase = ".MCP";
        internal const string defaultResponseExtension = ".response";
        internal const string invalidValueDBError = "Cannot insert the value NULL into column";
        internal const string specialtyBoardCertifiedIndicator = "B";
        internal const string defaultCulture = "en-US";
        internal const string directoryFormat = @"Plans\{0}\"; // {0} = planId

        //***********************************VALIDATION VARIABLES*******************************************
        public const string regExDateStringFormat = @"^\d{4}(0?[1-9]|1[012])(0?[1-9]|[12][0-9]|3[01])$";
        public const string regExBoolean = @"^$|[0-1]";
        public const string regEx1NumOrBlank = @"^(?:\d{1}|)$";
        public const string regEx11LenNumOrBlank = @"^(?:\d{11}|)$";
        public const string regEx10LenNumOrBlank = @"^(?:\d{10}|)$";
        public const string regEx7LenNumOrBlank = @"^(?:\d{7}|)$";
        public const string regEx5LenNumOrBlank = @"^(?:\d{5}|)$";
        public const string regEx4LenNumOrBlank = @"^(?:\d{4}|)$";
        public const string regEx2LenNumOrBlank = @"^(?:\d{2}|)$";

        public const string regEx0To1NumOrBlank = @"^(?:\d{1}[0-1]|)$";
        public const string regEx1To3NumOrBlank = @"^(?:\d{1}[1-3]|)$";
        public const string regEx1To5NumOrBlank = @"^(?:\d{1}[1-5]|)$";
        public const string regEx24HourOrBlank = @"^(?:0[0-9]|1[0-9]|2[0-3]|)$";

        public const string regExUnicodeLen4 = @"^(?:[\p{L}\p{N}]{4}|)";
        public const string regExUnicodeLen6 = @"^(?:[\p{L}\p{N}]{6}|)";
        public const string regExUnicodeLen7 = @"^(?:[\p{L}\p{N}]{7}|)";
        public const string regExState = @"^(?-i:A[LKSZRAEP]|C[AOT]|D[EC]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEHINOPST]|N[CDEHJMVY]|O[HKR]|P[ARW]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY])$";





        //**************************************************************************************************

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="lineNumber"></param>
        /// <param name="fileName"></param>
        /// <param name="errorMessage"></param>
        public static void LogFileRecordError(Logging log, string lineNumber, string fileName, string errorMessage)
        {
            string fullMessage = "Record failed. Line No: {0}; File: {1}; Reason: {2}";
            log.CreateLogEntry(String.Format(fullMessage, lineNumber, fileName, errorMessage), Logging.LogPriority.DataLoadIssues, 0);
        }
        public static void CreateCSV(this DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers    
            //for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //{
            //    sw.Write(dtDataTable.Columns[i]);
            //    if (i < dtDataTable.Columns.Count - 1)
            //    {
            //        sw.Write(",");
            //    }
            //}
            //sw.Write(sw.NewLine);


            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            if (value == "True")
                                value = "1";
                            else if (value == "False")
                                value = "0";
                        }
                        value = String.Format("\"{0}\"", value);
                        sw.Write(value);
                    }
                    else
                        sw.Write("\"\"");

                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        public static void ExportWeeklyRecords(string planNumber, string extractType)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet dsMembers = new DataSet();
                parameters.Add(SqlParms.CreateParameter("PLAN_NUM", DbType.String, planNumber, false));
                parameters.Add(SqlParms.CreateParameter("EXTRACT_TYPE", DbType.String, extractType, false));
                dsMembers = DataAccess.ExecuteStoredProcedure("usp_SelectMCPN_WEEKLY", parameters, "WeeklyExtract");

                DirectoryInfo localDirectory;
                string localPath = AppSettings.Get("MCP-ExportLocalPath");
                localDirectory = new DirectoryInfo(String.Format(localPath, planNumber));

                if (!localDirectory.Exists)
                {
                    localDirectory.Create();
                }
                //string exportFile = localDirectory + extractType + planNumber + DateTime.Now.ToString("yyyyMMdd") + ".recon";
                string exportFile = localDirectory + extractType + planNumber + DateTime.Now.ToString("yyyyMMdd") + ".recon";

                if (extractType == "HP" || extractType == "HC" || extractType == "NF")
                    exportFile = localDirectory + extractType + "000" + DateTime.Now.ToString("yyyyMMdd") + ".recon";

                if (dsMembers != null &&
                    dsMembers.Tables != null &&
                    dsMembers.Tables.Count > 0 &&
                    dsMembers.Tables[0] != null &&
                    dsMembers.Tables[0].Rows != null &&
                    dsMembers.Tables[0].Rows.Count > 0)
                {
                    MCPShared.CreateCSV(dsMembers.Tables[0], exportFile);
                }
                else
                {
                    System.IO.File.WriteAllLines(exportFile, new string[0]);
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int FileImported(Guid threadId, FileInfo currentFile)
        {
            try
            {
                // generated by sp_Admin_StoredProcBuilder on Oct 21 2020  2:41PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("MCPN_FILE_LOAD_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("MCPN_FILE_DATE_TIME", DbType.DateTime, GetFileDateFromName(threadId, currentFile), true));
                parameters.Add(SqlParms.CreateParameter("MCPN_FILE_NAME", DbType.String, currentFile.Name, false));
                parameters.Add(SqlParms.CreateParameter("MCPN_FILE_PATH", DbType.String, currentFile.DirectoryName, false));

                string newIdString = DataAccess.ExecuteScalar("usp_SaveMCPN_FILE", parameters);
                int newId = Int32.Parse(newIdString);

                return newId;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(threadId, ex);
            }

        }
        public static DateTime GetFileDateFromName(Guid threadId, FileInfo currentFile)
        {
            try
            {
                // example: "MA73120200427.mcp"
                string fileName = Path.GetFileNameWithoutExtension(currentFile.FullName);

                DateTime fileDateTime = DateTime.ParseExact(fileName.Substring(fileName.Length - 8, 8), MCPShared.dateFormat, new CultureInfo(defaultCulture));

                return fileDateTime;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(threadId, ex);
            }
        }
        public static string GetPlanIdFromFileName(Guid threadId, FileInfo currentFile)
        {
            try
            {
                // example: "MA73120200427.mcp"
                string fileName = Path.GetFileNameWithoutExtension(currentFile.FullName);

                string planId = fileName.Substring(2, 3);

                return planId;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(threadId, ex);
            }
        }


        public static String ValidateObject(object @object)
        {

            ICollection<ValidationResult> results;
            StringBuilder errors = new StringBuilder();
            var context = new ValidationContext(@object, null, null);
            results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(@object, context, results, true);

            if (!isValid)
            {
                foreach (var result in results)
                {
                    if (!String.IsNullOrWhiteSpace(result.ErrorMessage))
                    {
                        if (errors.Length == 0)
                            errors.Append(result.ErrorMessage);
                        else
                            errors.Append(AppSettings.Get("MCP-FieldDelimiter") + result.ErrorMessage);
                    }
                }
            }

            return errors.ToString();

        }

        public static DateTime GetDateFromString(string date)
        {
            CultureInfo ci = new CultureInfo(defaultCulture);
            string format = dateFormat;
            DateTime dt = DateTime.ParseExact(date, format, ci);
            return dt;
        }

        public static int SaveMCPInterestFileDetails(Logging log, Guid threadId, string fileName, string filePath)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MCPInterestFileDetails_FILE_PATH", DbType.String, filePath, true));
                parameters.Add(SqlParms.CreateParameter("MCPInterestFileDetails_FILE_NAME", DbType.String, fileName, true));
                parameters.Add(SqlParms.CreateParameter("MCPInterestFileDetails_LOAD_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));

                string newIdString = DataAccess.ExecuteScalar("usp_SaveMCPInterestFileDetails", parameters);
                int newId = Int32.Parse(newIdString);

                return newId;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("MCP Interest File Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static DataSet SelectMCPInterestFileDataByFileID(Logging log, Guid threadId, int currentFileID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FileID", DbType.Int32, currentFileID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectMCPInterestFileDataByFileID", parameters, "SelectMCPInterestFileDataByFileID");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("MCP Interest File Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static DataSet SelectMCP_PLANS_SUBMITTER_DETAILS(Logging log, Guid threadId)
        {
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectMCP_PLANS_SUBMITTER_DETAILS");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("MCP Interest File Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static bool CheckIfMCPDataNeedsStaging(Logging log, Guid threadId, int submitterID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SubmitterID", DbType.Int32, submitterID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectMCPRecordsToStageBySubmitterID", parameters, "SelectMCPRecordsToStageBySubmitterID");
                if (ObjectControllerHelper.HasRows(ds.Tables[0]))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("MCP Interest File Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                return false;
            }
        }


        public static int ProcessSTG_MCPInterestFile(Logging log, Guid threadId, string fileName, string exportPath, int submitterID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MCPInterestFileDetails_FILE_NAME", DbType.String, fileName, true));
                parameters.Add(SqlParms.CreateParameter("MCPInterestFileDetails_FILE_PATH", DbType.String, exportPath, true));
                parameters.Add(SqlParms.CreateParameter("MCPInterestFileDetails_LOAD_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("SubmitterID", DbType.Int32, submitterID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("usp_STG_MCPInterestFile", parameters));
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("MCP Interest File Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                return 0;
            }
        }

        public static void CreateCSV(DataTable dtDataTable, string localPath, string fileName)
        {
            string strFilePath = Path.Combine(localPath, fileName);

            StreamWriter sw = new StreamWriter(strFilePath, false);

            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            if (value == "True")
                                value = "1";
                            else if (value == "False")
                                value = "0";
                        }
                        value = String.Format("\"{0}\"", value);
                        sw.Write(value);
                    }
                    else
                        sw.Write("\"\"");

                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
    }
}
