using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace DataComparer
{
    public class XLandCSVOperations
    {
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public DataTable ImportExcel(string filePath)
        {
            //Create a new DataTable.
            DataTable dt = new DataTable();
            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                //Loop through the Worksheet rows.
                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;

                        for (int celVal = 1; celVal <= dt.Columns.Count; celVal++)
                        {
                            dt.Rows[dt.Rows.Count - 1][celVal - 1] = row.Cell(celVal).Value.ToString();
                        }
                    }
                }
            }
            return dt;
        }
        public string ConcantStringForSql(List<string> data)
        {
            string query = "";
            string lastRecord = data.Last();

            foreach (string s in data)
            {
                if (s != lastRecord)
                    query += $"'{s}',";
                else
                    query += $"'{s}'";
            }
            //query.TrimEnd(',');
            return query;
        }

        public List<STG_HCPCS_LVL2_CODE> GetSTG_HCPCS_LVL2_CODEs(DataTable dt)
        {
            var stgHcpcsLvl2Codes = new List<STG_HCPCS_LVL2_CODE>();

            foreach (DataRow dataRow in dt.Rows)
            {
                var stgHcpcsLvl2Code = new STG_HCPCS_LVL2_CODE
                {
                    CODE = GetStringValue(dataRow, "CODE"),
                    LONG_DESCRIPTION = GetStringValue(dataRow, "LONG_DESCRIPTION"),
                    SHORT_DESC = GetStringValue(dataRow, "SHORT_DESC"),
                    ADD_DATE = GetStringValue(dataRow, "ADD_DATE"),
                    ADD_EFF_DATE = GetStringValue(dataRow, "ADD_EFF_DATE"),
                    ACT_CDE = GetStringValue(dataRow, "ACT_CDE"),
                    TERM_DATE = GetStringValue(dataRow, "TERM_DATE")
                };

                stgHcpcsLvl2Codes.Add(stgHcpcsLvl2Code);
            }

            return stgHcpcsLvl2Codes;
        }
 
        public List<CLAIMS_HCPCS_PROCEDURE_CODE_DATA> GetCLAIMS_HCPCS_PROCEDURE_CODE_DATA(DataTable dt)
        {
            var claimsHcpcsProcedureCodeDataList = new List<CLAIMS_HCPCS_PROCEDURE_CODE_DATA>();

            foreach (DataRow dataRow in dt.Rows)
            {
                var claimsHcpcsProcedureCodeData = new CLAIMS_HCPCS_PROCEDURE_CODE_DATA
                {
                    CLAIMS_HCPCS_PROCEDURE_CODE = GetStringValue(dataRow, "CLAIMS_HCPCS_PROCEDURE_CODE"),
                    SHORT_DESC = GetStringValue(dataRow, "SHORT_DESC"),
                    LAY_DESC = GetStringValue(dataRow, "LAY_DESC"),
                    LONG_DESC = GetStringValue(dataRow, "LONG_DESC"),
                    EFFECTIVE_DATE = GetStringValue(dataRow, "EFFECTIVE_DATE"),
                    END_DATE = GetStringValue(dataRow, "END_DATE"),
                    CLAIMS_HCPCS_PROCEDURE_CODE_ORDER = GetStringValue(dataRow, "CLAIMS_HCPCS_PROCEDURE_CODE_ORDER"),
                    RECORD_STATUS = GetStringValue(dataRow, "RECORD_STATUS")
                };

                claimsHcpcsProcedureCodeDataList.Add(claimsHcpcsProcedureCodeData);
            }

            return claimsHcpcsProcedureCodeDataList;
        }

        private string GetStringValue(DataRow dataRow, string columnName)
        {
            return dataRow[columnName] == DBNull.Value || string.IsNullOrWhiteSpace(dataRow[columnName]?.ToString())
                ? string.Empty
                : dataRow[columnName].ToString().Trim();
        }

        private void ReleaseComObject(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
                obj = null;
            }
        }
    }
}
