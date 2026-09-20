using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;

namespace ImportLookupTables
{
    public enum ParseState { GET_FIELD_NAME, GET_TABLE_NAME, GET_CODES, READING_CODES };
    public partial class Form1 : Form
    {
        private string m_DataTablePrefix = "DT_";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (dlgGetFilePath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtInputFile.Text = dlgGetFilePath.FileName;
            }
        }

        private void btnGO_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(txtInputFile.Text);
            StreamWriter sw = new StreamWriter("outfile.txt");
            List<CodeDesc> codeList = new List<CodeDesc>();
            string valueColumnName = "";
            string newTableName = "";
            string currentLine = "";
            string insertScript = "";
            string createScript = "";
            ParseState currentState = ParseState.GET_FIELD_NAME;
            while (!sr.EndOfStream)
            {
                currentLine = sr.ReadLine();
                if (currentState == ParseState.GET_FIELD_NAME && currentLine.StartsWith("Field: "))
                {
                    valueColumnName = currentLine.Replace("Field: ", "");
                    valueColumnName = valueColumnName.Substring(0, valueColumnName.IndexOf('\t'));
                    sw.WriteLine(valueColumnName);
                    currentState = ParseState.GET_TABLE_NAME;
                }
                else if (currentState == ParseState.GET_TABLE_NAME && currentLine.Trim().Length > 0)
                {
                    newTableName = "LKUP_" + currentLine.Trim().Replace(' ', '_').Replace(".", "").Replace('-', '_').Replace("#", "").Replace('/', '_').ToUpper();
                    if (newTableName.IndexOf('\t') > -1)
                    {
                        newTableName = newTableName.Substring(0, newTableName.IndexOf('\t'));
                    }
                    currentState = ParseState.GET_CODES;
                }
                else if (currentState == ParseState.GET_CODES && currentLine.StartsWith("Value"))
                {
                    codeList.Clear();
                    currentState = ParseState.READING_CODES;
                }
                else if (currentState == ParseState.READING_CODES)
                {
                    if (currentLine.StartsWith("---"))
                    {
                        SqlTransaction trans = null;
                        try
                        {
                            trans = CHelper.CreateTransaction();
                            createScript = BuildCreateScript(newTableName, valueColumnName, codeList);
                            CHelper.ExecuteSql(createScript, trans);
                            sw.WriteLine("Created Table " + newTableName);
                            foreach (CodeDesc code in codeList)
                            {
                                insertScript = BuildInsertScript(newTableName, valueColumnName, code);
                                CHelper.ExecuteSql(insertScript, trans);
                            }
                            sw.WriteLine("Populated Table " + newTableName);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            sw.WriteLine("Error: " + CHelper.FormatExceptionForLog(ex));
                        }
                        currentState = ParseState.GET_FIELD_NAME;
                    }
                    else
                    {
                        string[] codes = currentLine.Split('\t');
                        if (codes.Length == 4)
                        {
                            CodeDesc cdDesc = new CodeDesc();
                            cdDesc.Value = codes[0].Replace("'", "''");
                            cdDesc.ShortName = codes[1].Replace("'", "''");
                            cdDesc.LongName = codes[2].Replace("'", "''");
                            cdDesc.Mneumonic = codes[3].Replace("'", "''");
                            codeList.Add(cdDesc);
                        }
                    }
                }
            }
            sr.Close();
            sw.Close();
            MessageBox.Show("DONE!");
        }

        private string BuildInsertScript(string tableName, string valueCodeName, CodeDesc code)
        {
            string insertTemplate = "INSERT INTO [dbo].[<table-name>] ([<value-code-name>],[Short],[Long],[Mnemonic])";
            insertTemplate += " VALUES ('<value-code>','<short-name>','<long-name>','<mneumonic>'); ";
            insertTemplate = insertTemplate.Replace("<table-name>", tableName.Trim()).Replace("<value-code-name>", valueCodeName.Trim()).Replace("<value-code>", code.Value.Trim()).Replace("<short-name>", code.ShortName.Trim()).Replace("<long-name>", code.LongName.Trim()).Replace("<mneumonic>", code.Mneumonic.Trim());
            return insertTemplate;
        }

        private string BuildCreateScript(string tableName, string valueCodeName, List<CodeDesc> codes)
        {
            string createTemplate = "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = '<lookup-table-name>'))";
            createTemplate += Environment.NewLine + "	DROP TABLE [dbo].[<lookup-table-name>]";
            createTemplate += Environment.NewLine;
            createTemplate += Environment.NewLine + "CREATE TABLE [dbo].[<lookup-table-name>](";
            createTemplate += Environment.NewLine + "[<code-value-name>] [varchar](<max-code-value-name-length>) NULL,";
            createTemplate += Environment.NewLine + "[Short] [varchar](<max-short-name-length>) NULL,";
            createTemplate += Environment.NewLine + "[Long] [varchar](<max-long-name-length>) NULL,";
            createTemplate += Environment.NewLine + "[Mnemonic] [varchar](<max-mneumonic-length>) NULL";
            createTemplate += Environment.NewLine + ") ON [PRIMARY]";
            createTemplate = createTemplate.Replace("<lookup-table-name>", tableName).Replace("<code-value-name>", valueCodeName);
            createTemplate = createTemplate.Replace("<max-code-value-name-length>", codes.Max(mc => mc.Value.Trim().Length).ToString());
            createTemplate = createTemplate.Replace("<max-short-name-length>", codes.Max(mc => mc.ShortName.Trim().Length).ToString());
            createTemplate = createTemplate.Replace("<max-long-name-length>", codes.Max(mc => mc.LongName.Trim().Length).ToString());
            createTemplate = createTemplate.Replace("<max-mneumonic-length>", codes.Max(mc => mc.Mneumonic.Trim().Length).ToString());
            return createTemplate;
        }

        private void btnMetaDataFile_Click(object sender, EventArgs e)
        {
            if (dlgGetFilePath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtMetaDataFile.Text = dlgGetFilePath.FileName;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            StreamReader metaDataFile = new StreamReader(txtMetaDataFile.Text);
            StreamWriter logFile = new StreamWriter(Path.Combine(txtDataFileFolder.Text, "log.txt"));
            XLWorkbook resultsBook = new XLWorkbook();
            string resultsFileName = Path.Combine(txtDataFileFolder.Text, "StagingImportResults_Run_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx");
            string currentLine = "";
            string tableName = "";
            string importFileName = "";
            List<DataElement> elementList = new List<DataElement>();
            while ((currentLine = metaDataFile.ReadLine()) != null)
            {
                if (currentLine.Contains("/"))
                {
                    string[] tableNames = currentLine.Split('/');
                    if (tableNames.Length == 3)
                    {
                        if (elementList.Count > 0)
                        {
                            DataTable importDT = null;
                            try
                            {
                                // populate the table
                                importDT = PopulateDataTable(importFileName, tableName, elementList, logFile);
                                logFile.WriteLine("Populated TVP " + tableName + " successfully.");
                            }
                            catch (Exception ex)
                            {
                                logFile.WriteLine("Error populating TVP " + tableName);
                                logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                            }

                            try
                            {
                                // execute the sql query
                                DataSet ds = ExecuteInsertCommand(tableName, importDT);
                                logFile.WriteLine("Populated table " + tableName + " successfully.");
                                WriteResults(resultsBook, tableName, ds);

                            }
                            catch (Exception ex)
                            {
                                logFile.WriteLine("Error populating table " + tableName);
                                logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                            }
                        }

                        elementList.Clear();
                        importFileName = Path.Combine(txtDataFileFolder.Text, tableNames[1].Trim() + "." + txtImportFileExt.Text + ".TXT");
                        tableName = m_DataTablePrefix + tableNames[1].Trim() + "_STG";
                    }
                }
                else
                {
                    string[] dataData = currentLine.Split(',');
                    if (dataData.Length == 2)
                    {
                        DataElement de = new DataElement();
                        de.Name = dataData[0].Trim();
                        de.Size = Convert.ToInt32(dataData[1]);
                        elementList.Add(de);
                    }
                }
            }

            if (tableName.Trim().Length > 0 && elementList.Count > 0)
            {
                DataTable importDT = null;
                try
                {
                    // populate the table
                    importDT = PopulateDataTable(importFileName, tableName, elementList, logFile);
                    logFile.WriteLine("Populated TVP " + tableName + " successfully.");
                }
                catch (Exception ex)
                {
                    logFile.WriteLine("Error populating TVP " + tableName);
                    logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                }

                try
                {
                    // execute the sql query
                    DataSet ds = ExecuteInsertCommand(tableName, importDT);
                    logFile.WriteLine("Populated table " + tableName + " successfully.");
                    WriteResults(resultsBook, tableName, ds);
                }
                catch (Exception ex)
                {
                    logFile.WriteLine("Error populating table " + tableName);
                    logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                }
            }
            metaDataFile.Close();
            logFile.Close();
            resultsBook.SaveAs(resultsFileName);

            MessageBox.Show("All Done!");
        }

        private void WriteResults(XLWorkbook resultsBook, string tableName, DataSet ds)
        {
            IXLWorksheet currentWorksheet = resultsBook.Worksheets.Add(tableName);
            currentWorksheet.Row(1).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
            currentWorksheet.Cell(1, 1).Value = "RESULTS FOR STAGING TABLE " + tableName;

            if (CHelper.HasRows(ds))
            {
                int rowIndex = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string[] cellText = CHelper.GetString("ErrorLine", dr).Split('|');
                    for (int colIndex = 0; colIndex < cellText.Length; colIndex++)
                    {
                        currentWorksheet.Cell(rowIndex + 1, colIndex + 1).Value = cellText[colIndex];
                    }
                    rowIndex++;
                }
            }
        }
        private DataSet ExecuteInsertCommand(string tableName, DataTable importDT)
        {
            CHelper.ExecuteSql("DELETE FROM dbo.[" + tableName + "];");
            SqlParameter param = new SqlParameter("@dataInputTVP", importDT);
            param.SqlDbType = System.Data.SqlDbType.Structured;
            param.TypeName = "dbo.TVP_" + tableName;
            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(param);
            return CHelper.ExecuteStoredProcedure("dbo.Insert_" + tableName, p);
        }

        private DataTable PopulateDataTable(string importFileName, string tableName, List<DataElement> elementList, StreamWriter logFile)
        {
            DataTable dt = BuildDataInsertDataTable(tableName, elementList);
            StreamReader dataRdr = new StreamReader(importFileName);
            string dataLine = "";
            try
            {
                int lineNumber = 0;
                while ((dataLine = dataRdr.ReadLine()) != null)
                {
                    if (dataLine.Trim().Length > 0)
                    {
                        AddTableRow(dataLine, dt, ++lineNumber, logFile);
                    }
                }
                logFile.WriteLine("Populated table, " + tableName);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dataRdr != null)
                {
                    dataRdr.Close();
                }
            }
            return dt;
        }

        private void AddTableRow(string dataLine, DataTable dt, int lineNumber, StreamWriter logFile)
        {
            string[] dataList = dataLine.Split('|');
            if (dataList.Length != dt.Columns.Count)
            {
                logFile.WriteLine("Error on line:  " + lineNumber.ToString() + " : The number of table columns does not match what's in the import file.");
            }
            else
            {
                DataRow dr = dt.NewRow();
                for (int index = 0; index < dr.ItemArray.Length; index++)
                {
                    dataList[index] = dataList[index].Trim();
                    if (dt.Columns[index].DataType == typeof(int))
                    {
                        dr[index] = Convert.ToInt32(dataList[index]);
                    }
                    else
                    {
                        dr[index] = dataList[index];

                        // fix known data reference value problems
                        if (dt.Columns[index].ColumnName.Trim() == "P-EMPL-PSTN-CD" && IsNumeric(dataList[index]) && Convert.ToInt32(dataList[index]) > 5)
                        {
                            dr[index] = "5";
                        }
                        else if (dt.Columns[index].ColumnName.Trim() == "P-SPECL-DENT-IND" && dataList[index] == "#")
                        {
                            dr[index] = "N";
                        }
                        else if (dt.Columns[index].ColumnName.Trim() == "P-CERT-PRESCR-CD" && (dataList[index].Trim().Equals("0") || dataList[index].Trim().Length == 0))
                        {
                            dr[index] = "04";
                        }
                        else if (dataList[index].Equals("0001-01-01") || dataList[index].Equals("0101-01-01"))
                        {
                            // swap it to be a valid beginning date
                            dr[index] = "1753-01-01";
                        }
                        else if (IsDateTime(dataList[index]))
                        {
                            DateTime date = DateTime.Parse(dataList[index]);
                            if (date >= new DateTime(9899, 12, 31) && date <= new DateTime(9999, 12, 31))
                            {
                                dr[index] = "9999-12-31";
                            }
                        }

                    }
                }

                dt.Rows.Add(dr);
            }
        }

        public static bool IsNumeric(string stringToTest)
        {
            int result;
            return int.TryParse(stringToTest, out result);
        }

        /// <summary>
        /// Method checks if passed string is datetime
        /// </summary>
        /// <param name="text">string text for checking</param>
        /// <returns>bool - if text is datetime return true, else return false</returns>
        public bool IsDateTime(string text)
        {
            DateTime dateTime;
            bool isDateTime = false;

            // Check for empty string.
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            isDateTime = DateTime.TryParse(text, out dateTime);

            return isDateTime;
        }

        private DataTable BuildDataInsertDataTable(string tableName, List<DataElement> elementList)
        {
            DataTable retTable = new DataTable();
            foreach (DataElement element in elementList)
            {
                DataColumn dc = new System.Data.DataColumn("tvp_" + element.Name);
                if (element.Name.Contains("SYS-ID") || element.Name == "P-SEQ-NUM")
                {
                    dc.DataType = typeof(int);
                }
                else
                {
                    dc.DataType = typeof(string);
                }

                retTable.Columns.Add(dc);
            }
            return retTable;
        }

        private string BuildDataInsertScriptTVP(string tableName, List<DataElement> elementList, string variableList)
        {
            string insertTemplate = "INSERT INTO [dbo].[" + tableName + "] (";
            bool insertComma = false;
            foreach (DataElement de in elementList)
            {
                if (!insertComma)
                {
                    insertComma = true;
                }
                else
                {
                    insertTemplate += ",";
                }
                insertTemplate += "[" + de.Name + "]";
            }
            insertTemplate += ") VALUES (" + variableList + ");";
            return insertTemplate;
        }


        private void CreateDataTable(string tableName, List<DataElement> dataElements)
        {
            string sqlCmd = BuildDataTableCreateScript(tableName, dataElements);
            CHelper.ExecuteSql(sqlCmd);
        }



        private string BuildDataTableCreateScript(string tableName, List<DataElement> dataElements)
        {
            string createTemplate = "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = '<table-name>'))";
            createTemplate += Environment.NewLine + "	DROP TABLE [dbo].[<table-name>]";
            createTemplate += Environment.NewLine;
            createTemplate += Environment.NewLine + "CREATE TABLE [dbo].[<table-name>](";

            bool insertComma = false;
            foreach (DataElement de in dataElements)
            {
                if (!insertComma)
                {
                    insertComma = true;
                }
                else
                {
                    createTemplate += ",";
                }
                if (de.Name.Contains("SYS-ID") || de.Name == "P-SEQ-NUM")
                {
                    createTemplate += Environment.NewLine + "[" + de.Name + "] [int]";
                }
                //else if (de.Name.EndsWith("-DT"))
                //{
                //    createTemplate += Environment.NewLine + "[" + de.Name + "] [datetime]";
                //}
                else
                {
                    createTemplate += Environment.NewLine + "[" + de.Name + "] [varchar](" + de.Size + ") NULL";
                }
            }
            createTemplate += "," + Environment.NewLine + "[EnableConversion] bit DEFAULT 1,";
            createTemplate += "," + Environment.NewLine + "[ErrorCode] int DEFAULT 0,";
            createTemplate += "," + Environment.NewLine + "[ErrorMessage] varchar(100)";
            createTemplate += Environment.NewLine + ") ON [PRIMARY]";
            createTemplate = createTemplate.Replace("<table-name>", tableName);
            return createTemplate;
        }

        private string BuildDataTableTypeCreateScript(string tableName, List<DataElement> dataElements)
        {
            string tableTypeName = "TVP_" + tableName;
            string createTemplate = "IF TYPE_ID(N'" + tableTypeName + "') IS NOT NULL";
            createTemplate += Environment.NewLine + "	DROP TYPE " + tableTypeName;
            createTemplate += Environment.NewLine;
            createTemplate += Environment.NewLine + "CREATE TYPE [dbo].[" + tableTypeName + "] AS TABLE (";

            bool insertComma = false;
            foreach (DataElement de in dataElements)
            {
                if (!insertComma)
                {
                    insertComma = true;
                }
                else
                {
                    createTemplate += ",";
                }
                if (de.Name.Contains("SYS-ID") || de.Name == "P-SEQ-NUM")
                {
                    createTemplate += Environment.NewLine + "[tvp_" + de.Name + "] [int]";
                }
                else
                {
                    createTemplate += Environment.NewLine + "[tvp_" + de.Name + "] [varchar](" + de.Size + ") NULL";
                }
            }
            createTemplate += Environment.NewLine + ")";
            return createTemplate;
        }

        private string BuildDeleteInsertStoredProc(string tableName)
        {
            string sql = "IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Insert_" + tableName + "')";
            sql += Environment.NewLine + "DROP PROCEDURE Insert_" + tableName;
            return sql;
        }

        private string BuildInsertStoredProc(string tableName, List<DataElement> dataElements)
        {
            StreamReader sr = new StreamReader(Application.StartupPath + "\\SqlTemplates\\InsertStoredProcedureTemplate.txt");
            string insertTemplate = sr.ReadToEnd();
            sr.Close();
            List<string> variableNames = new List<string>();
            string variableList = "";
            string dataElementDeclaration = "";
            string dataElementSelectList = "";
            string variable_declarations = "";
            string variableErrorList = "";
            string errorListHeader = "";
            bool insertComma = false;
            int dataElementIndex = 0;
            foreach (DataElement de in dataElements)
            {
                if (!insertComma)
                {
                    insertComma = true;
                }
                else
                {
                    variableList += ",";
                    dataElementDeclaration += ",";
                    dataElementSelectList += ",";
                    variableErrorList += " + '|' + ";
                    errorListHeader += "|";
                }
                string varName = "@" + de.Name.Replace('-', '_').ToLower();
                dataElementDeclaration += Environment.NewLine + "[" + de.Name + "]" + " varchar(" + de.Size + ")";
                errorListHeader += de.Name;
                variableList += varName;
                variableErrorList += varName;
                variableNames.Add(varName);

                if (varName.Contains("SYS-ID") || varName == "P-SEQ-NUM")
                {
                    variable_declarations += Environment.NewLine + "DECLARE " + varName + " int;";
                }
                else
                {
                    variable_declarations += Environment.NewLine + "DECLARE " + varName + " varchar(" + dataElements[dataElementIndex].Size + ")";
                }
                dataElementSelectList += "[tvp_" + de.Name + "]";

                dataElementIndex++;
            }

            string insertDataScript = BuildDataInsertScriptTVP(tableName, dataElements, variableList);
            insertTemplate = insertTemplate.Replace("<table_name>", tableName);
            insertTemplate = insertTemplate.Replace("<data_element_declarations>", dataElementDeclaration);
            insertTemplate = insertTemplate.Replace("<variable_declarations>", variable_declarations);
            insertTemplate = insertTemplate.Replace("<data_element_select_list>", dataElementSelectList);
            insertTemplate = insertTemplate.Replace("<variable_list>", variableList);
            insertTemplate = insertTemplate.Replace("<insert_data_script>", insertDataScript);
            insertTemplate = insertTemplate.Replace("<variable_error_list>", variableErrorList);
            insertTemplate = insertTemplate.Replace("<error_list_header>", errorListHeader);
            return insertTemplate;
        }

        private void DropStoredProc(string tableName)
        {
            string delSql = BuildDeleteInsertStoredProc(tableName);
            CHelper.ExecuteSql(delSql);
        }
        private void CreateDataTableType(string tableName, List<DataElement> dataElements)
        {
            string sql = BuildDataTableTypeCreateScript(tableName, dataElements);
            CHelper.ExecuteSql(sql);
        }

        private void CreateInsertStoredProc(string tableName, List<DataElement> dataElements)
        {

            string sql = BuildInsertStoredProc(tableName, dataElements);
            CHelper.ExecuteSql(sql);
        }

        private void btnDataFileFolder_Click(object sender, EventArgs e)
        {
            if (dlgFolderPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDataFileFolder.Text = dlgFolderPicker.SelectedPath;
            }
        }

        private void btnCreateObjects_Click(object sender, EventArgs e)
        {
            StreamReader metaDataFile = new StreamReader(txtMetaDataFile.Text);
            StreamWriter logFile = new StreamWriter(Path.Combine(txtDataFileFolder.Text, "log.txt"));
            string currentLine = "";
            string tableName = "";
            string importFileName = "";
            List<DataElement> elementList = new List<DataElement>();
            while ((currentLine = metaDataFile.ReadLine()) != null)
            {
                if (currentLine.Contains("/"))
                {
                    string[] tableNames = currentLine.Split('/');
                    if (tableNames.Length == 3)
                    {
                        if (elementList.Count > 0)
                        {
                            try
                            {
                                // create the table
                                CreateDataTable(tableName, elementList);
                                logFile.WriteLine("Created table, " + tableName + " successfully.");
                            }
                            catch (Exception ex)
                            {
                                logFile.WriteLine("Error creating table " + tableName);
                                logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                            }
                            try
                            {
                                // drop the store proc
                                DropStoredProc(tableName);
                                logFile.WriteLine("Dropped stored proc for " + tableName + " successfully.");
                            }
                            catch (Exception ex)
                            {
                                logFile.WriteLine("Error dropping stored proc for " + tableName);
                                logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                            }

                            try
                            {
                                // create the TVP
                                CreateDataTableType(tableName, elementList);
                                logFile.WriteLine("Created TVP_" + tableName + " successfully.");
                            }
                            catch (Exception ex)
                            {
                                logFile.WriteLine("Error creating TVP " + tableName);
                                logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                            }

                            try
                            {
                                // create the insert stored proc
                                CreateInsertStoredProc(tableName, elementList);
                                logFile.WriteLine("Created TVP_" + tableName + " successfully.");
                            }
                            catch (Exception ex)
                            {
                                logFile.WriteLine("Error creating TVP " + tableName);
                                logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                            }
                        }

                        elementList.Clear();
                        importFileName = Path.Combine(txtDataFileFolder.Text, tableNames[1].Trim() + ".R033116.TXT");
                        tableName = m_DataTablePrefix + tableNames[1].Trim() + "_STG";
                    }
                }
                else
                {
                    string[] dataData = currentLine.Split(',');
                    if (dataData.Length == 2)
                    {
                        DataElement de = new DataElement();
                        de.Name = dataData[0].Trim();
                        de.Size = Convert.ToInt32(dataData[1]);
                        elementList.Add(de);
                    }
                }
            }

            if (tableName.Trim().Length > 0 && elementList.Count > 0)
            {
                try
                {
                    // create the table
                    CreateDataTable(tableName, elementList);
                    logFile.WriteLine("Created table, " + tableName + " successfully.");
                }
                catch (Exception ex)
                {
                    logFile.WriteLine("Error creating table " + tableName);
                    logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                }
                try
                {
                    // drop the store proc
                    DropStoredProc(tableName);
                    logFile.WriteLine("Dropped stored proc for " + tableName + " successfully.");
                }
                catch (Exception ex)
                {
                    logFile.WriteLine("Error dropping stored proc for " + tableName);
                    logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                }
                try
                {
                    // create the TVP
                    CreateDataTableType(tableName, elementList);
                    logFile.WriteLine("Created TVP_" + tableName + " successfully.");
                }
                catch (Exception ex)
                {
                    logFile.WriteLine("Error creating TVP " + tableName);
                    logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                }

                try
                {
                    // create the insert stored proc
                    CreateInsertStoredProc(tableName, elementList);
                    logFile.WriteLine("Created TVP_" + tableName + " successfully.");
                }
                catch (Exception ex)
                {
                    logFile.WriteLine("Error creating TVP " + tableName);
                    logFile.WriteLine(CHelper.FormatExceptionForLog(ex));
                }
            }
            metaDataFile.Close();
            logFile.Close();

            MessageBox.Show("All Done!");
        }

        private void btnBrowseForMMIS_Click(object sender, EventArgs e)
        {
            if (dlgGetFilePath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtMMISPath.Text = dlgGetFilePath.FileName;
            }
        }

        private void btnBrowseForPDMS_Click(object sender, EventArgs e)
        {
            if (dlgGetFilePath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPDMSPath.Text = dlgGetFilePath.FileName;
            }
        }


        private void btnLayoutSheetGO_Click(object sender, EventArgs e)
        {
            XLWorkbook pdmsBook = new XLWorkbook(txtPDMSPath.Text);
            IXLWorksheet pdmsSheet = pdmsBook.Worksheet(1);

            ClosedXML.Excel.XLWorkbook mmisBook = new XLWorkbook(txtMMISPath.Text);
            IXLWorksheet mmisSheet;
            mmisBook.Worksheets.TryGetWorksheet("Required Fields", out mmisSheet);

            string currentTable = "";
            IXLRows mmisRows = mmisSheet.Rows();
            foreach (IXLRow mmisRow in mmisRows)
            {
                if (mmisRow.Cell(2).Value.ToString().Contains("TB"))
                {
                    string rawTable = mmisRow.Cell(4).Value.ToString();
                    int firstSlashPos = rawTable.IndexOf('/');
                    int lastSlashPos = rawTable.LastIndexOf('/');
                    int length = lastSlashPos - firstSlashPos;
                    currentTable = rawTable.Substring(firstSlashPos + 1, length - 1).Trim();
                }
                else
                {
                    string currentElement = mmisRow.Cell(3).Value.ToString().Trim();
                    mmisRow.Cell(17).Value = FindElementInPDMS(currentTable, currentElement, pdmsSheet) ? "Y" : "N";
                }
            }

            mmisBook.Save();

            MessageBox.Show("Done");
        }
        private bool FindElementInPDMS(string table, string element, IXLWorksheet sheet)
        {
            bool isFound = false;

            IXLRows rows = sheet.Rows();
            foreach (IXLRow row in rows)
            {
                if (row.Cell(5).Value.ToString().Contains(table) && row.Cell(6).Value.ToString().Contains(element))
                {
                    isFound = true;
                    break;
                }
            }

            return isFound;
        }

        private void btnBrowseErrReportOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtOutputFileName.Text = dlg.FileName;
            }

        }

        private void btnCreateErrorReport_Click(object sender, EventArgs e)
        {
            DataSet ds = CHelper.ExecuteSql("SELECT * FROM DCConv_Counts WHERE ConversionRunID = '" + txtRunID.Text.Trim() + "'");
            if (CHelper.HasRows(ds))
            {
                XLWorkbook errorBook = new XLWorkbook();
                IXLWorksheet summarySheet = errorBook.Worksheets.Add("Conversion Summary Report");
                summarySheet.Row(1).Cell(1).Value = "Table Name";
                summarySheet.Row(1).Cell(2).Value = "Table Description";
                summarySheet.Row(1).Cell(3).Value = "Transactions Received";
                summarySheet.Row(1).Cell(4).Value = "Transactions Processed";
                summarySheet.Row(1).Cell(5).Value = "Transactions Not Processed";
                int summaryRowIndex = 2;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // fill in the summary sheet
                    string dataType = CHelper.GetString("DataTypeDescription", dr).Trim();
                    int numberConverted = CHelper.GetInt("NumberOfRecordsConverted", dr);
                    int numberFailed = CHelper.GetInt("NumberOfFailedRecords", dr);
                    int totalProcessed = CHelper.GetInt("TotalRecordsProcessed", dr);
                    summarySheet.Row(summaryRowIndex).Cell(1).Value = dataType;
                    summarySheet.Row(summaryRowIndex).Cell(3).Value = totalProcessed;
                    summarySheet.Row(summaryRowIndex).Cell(4).Value = numberConverted;
                    summarySheet.Row(summaryRowIndex).Cell(5).Value = numberFailed;
                    summaryRowIndex++;

                    // create the detail sheet
                    IXLWorksheet currentSheet = errorBook.Worksheets.Add(dataType);
                    int rowIndex = 1;
                    // get and set the header info for the sheet
                    IXLRow currentRow = currentSheet.Row(rowIndex);
                    currentRow.Cell(1).Value = "Error Reason";
                    currentRow.Cell(2).Value = "Error Type/Action";
                    currentRow.Cell(3).Value = "Provider ID (if available)";
                    string tableName = "DT_" + dataType.Trim() + "_STG";
                    if (dataType == "SERVICE_PADDRSTB" || dataType == "ADDITIONAL_PADDRSTB")
                    {
                        tableName = "DT_PADDRSTB_STG";
                    }
                    else if (dataType == "DEA")
                    {
                        tableName = "DT_PROVDRTB_STG";
                    }
                    DataSet dsDetailColumns = CHelper.ExecuteSql("USE [" + txtSourceDatabase.Text + "]; SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "'");
                    string selectClause = "";
                    bool firstOne = true;
                    int cellIndex = 4;
                    List<string> columnNames = new List<string>();
                    foreach (DataRow drDetailColumn in dsDetailColumns.Tables[0].Rows)
                    {
                        string colName = CHelper.GetString("COLUMN_NAME", drDetailColumn).Trim();
                        
                        if (colName != "EnableConversion" && colName != "ErrorCode" && colName != "ErrorMessage")
                        {
                            if (firstOne)
                            {
                                firstOne = false;
                            }
                            else
                            {
                                selectClause += ",";
                            }
                            selectClause += "dt.[" + colName + "]";
                            currentRow.Cell(cellIndex).Value = colName;
                            columnNames.Add(colName);
                            cellIndex++;
                        }
                    }
                    rowIndex++;
                    DataSet dsErrs = CHelper.ExecuteSql("SELECT * FROM DCConv_ConversionErrors WHERE DataType ='" + dataType + "' AND RunID = '" + txtRunID.Text.Trim() + "' ORDER BY SysID, ErrorCode");
                    if (CHelper.HasRows(dsErrs))
                    {

                        foreach (DataRow drErr in dsErrs.Tables[0].Rows)
                        {
                            int errCode = CHelper.GetInt("ErrorCode", drErr);
                            string errMessage = CHelper.GetString("ErrorMessage", drErr);
                            string errorSourceTable = CHelper.GetString("SourceTable", drErr);
                            int lineID = CHelper.GetInt("LineID", drErr);
                            string providerID = "";
                            string sysID = "";
                            string[] errComp = errMessage.Split('|');
                            switch (errCode)
                            {
                                case 1:
                                    if (errComp.Length == 6)
                                    {
                                        errMessage = "Invalid date: " + errComp[4].Trim() + " = '" + errComp[5] + "' for GROUP P-SYS-ID = " + errComp[0] + ((errComp[1].Trim().Length > 0) ? ", GROUP Provider ID = '" + errComp[1].Trim() + "'" : "") + "' for GROUP MEMBER P-SYS-ID = " + errComp[2] + ((errComp[1].Trim().Length > 0) ? ", GROUP MEMBER Provider ID = '" + errComp[3].Trim() + "'" : "");
                                        providerID = errComp[1].Trim();
                                        sysID = errComp[0].Trim();
                                    }
                                    else
                                    {
                                        errMessage = "Invalid date in " + errorSourceTable + " : " + errComp[2].Trim() + " = '" + errComp[3] + "'";
                                        providerID = errComp[1].Trim();
                                        sysID = errComp[0].Trim();
                                    }
                                    break;
                                case 2:
                                    if (errComp.Length == 6)
                                    {
                                        errMessage = "Invalid reference value: " + errComp[4].Trim() + " = '" + errComp[5] + "' for GROUP P-SYS-ID = " + errComp[0] + ((errComp[1].Trim().Length > 0) ? ", GROUP Provider ID = '" + errComp[1].Trim() + "'" : "") + "' for GROUP MEMBER P-SYS-ID = " + errComp[2] + ((errComp[1].Trim().Length > 0) ? ", GROUP MEMBER Provider ID = '" + errComp[3].Trim() + "'" : "");
                                    }
                                    else
                                    {
                                        errMessage = "Invalid reference value in " + errorSourceTable + " : " + errComp[2].Trim() + " = '" + errComp[3];
                                        providerID = errComp[1].Trim();
                                        sysID = errComp[0].Trim();
                                    }
                                    break;

                                case 3:
                                    errMessage = "Provider type excluded in " + errorSourceTable + " : " + errComp[2].Trim() + " = '" + errComp[3] + "'";
                                    providerID = errComp[1].Trim();
                                    sysID = errComp[0].Trim();
                                    break;

                                case 4:
                                    errMessage = "Invalid taxonomy type : " + errComp[2].Trim() + " = '" + errComp[3] + "'";
                                    providerID = errComp[1].Trim();
                                    sysID = errComp[0].Trim();
                                    break;

                                case 5:
                                    errMessage = "Invalid specialty type: " + errComp[2].Trim() + " = '" + errComp[3] + "'";
                                    providerID = errComp[1].Trim();
                                    sysID = errComp[0].Trim();
                                    break;
                            }

                            DataSet dsDetailDump = CHelper.ExecuteSql("USE [" + txtSourceDatabase.Text + "]; SELECT DISTINCT " + selectClause + " FROM [dbo].[" + tableName + "] dt WHERE [LineID] = " + lineID.ToString());
                            if (CHelper.HasRows(dsDetailDump))
                            {
                                foreach (DataRow drDump in dsDetailDump.Tables[0].Rows)
                                {
                                    currentRow = currentSheet.Row(rowIndex);
                                    currentRow.Cell(1).Value = errMessage;
                                    currentRow.Cell(3).Value = providerID;
                                    cellIndex = 4;
                                    foreach (string columnName in columnNames)
                                    {
                                        currentRow.Cell(cellIndex).DataType = XLCellValues.Text;
                                         if (columnName == "P-SYS-ID")
                                        {
                                            currentRow.Cell(cellIndex).SetValue<string>(CHelper.GetInt(columnName, drDump).ToString());
                                        }
                                        else
                                        {
                                            currentRow.Cell(cellIndex).SetValue<string>(CHelper.GetString(columnName, drDump));
                                        }
                                        cellIndex++;
                                    }
                                    rowIndex++;
                                }
                            }
                        }
                    }
                }
                errorBook.SaveAs(txtOutputFileName.Text);
                MessageBox.Show("All done!");
            }
        }
    }

    public class CodeDesc
    {
        public string Value;
        public string ShortName;
        public string LongName;
        public string Mneumonic;
    }

    public class DataElement
    {
        public string Name;
        public int Size;
    }
}
