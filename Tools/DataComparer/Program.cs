

using ClosedXML.Excel;
using System.Data;
using System.Runtime.InteropServices;

namespace DataComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            FindDataDifferences();
        }
        private static void FindDataDifferences()
        {
            string filePath = @"C:\\Projects\\updateQueries.txt"; // Specify the file path

            List<QueryList> differences = new List<QueryList>();

            List<STG_HCPCS_LVL2_CODE> stgHCPCS = LOAD_STG_HCPCS_LVL2_CODEs();
            List<CLAIMS_HCPCS_PROCEDURE_CODE_DATA> claimsHCPCS = LOAD_CLAIMS_HCPCS_PROCEDURE_CODE_DATA();

            stgHCPCS = stgHCPCS.Distinct().ToList();
            claimsHCPCS = claimsHCPCS.Distinct().ToList();

            stgHCPCS.AsParallel().ForAll(s =>
            {
                s.SHORT_DESC.Trim();
                s.LONG_DESCRIPTION.Trim();
            });
            claimsHCPCS.AsParallel().ForAll(s =>
            {
                s.SHORT_DESC.Trim();
                s.LONG_DESC.Trim();
            });
            string modifiedUser = Guid.NewGuid().ToString();

            stgHCPCS.AsParallel().ForAll(stg =>
            {
                CLAIMS_HCPCS_PROCEDURE_CODE_DATA claimHcpcs = claimsHCPCS.FirstOrDefault(c => c.CLAIMS_HCPCS_PROCEDURE_CODE.Trim() == stg.CODE.Trim());

                if (claimHcpcs != null)
                {

                    List<string> updateParts = new List<string>();

                    if (!stg.SHORT_DESC.Equals(claimHcpcs.SHORT_DESC))
                    {
                        //string escapedValue = stg.SHORT_DESC.Replace("'", "''");
                        string escapedValue = stg.SHORT_DESC?.Replace("'", "''") ?? string.Empty;
                        updateParts.Add($"SHORT_DESC = '{escapedValue}'");
                    }

                    if (!stg.LONG_DESCRIPTION.Equals(claimHcpcs.LONG_DESC))
                    {
                        //string escapedValueLong = stg.LONG_DESCRIPTION.Replace("'", "''");
                        string escapedValueLong = stg.LONG_DESCRIPTION?.Replace("'", "''") ?? string.Empty;
                        updateParts.Add($"LONG_DESC = '{escapedValueLong}'");
                    }

                    if (!stg.ADD_EFF_DATE.Equals(claimHcpcs.EFFECTIVE_DATE))
                    {
                        updateParts.Add($"EFFECTIVE_DATE = '{stg.ADD_EFF_DATE}'");
                    }

                    if (stg.TERM_DATE != null && claimHcpcs.END_DATE != null && stg.TERM_DATE != claimHcpcs.END_DATE)
                    {
                        updateParts.Add($"END_DATE = '{stg.TERM_DATE}'");
                    }
                    if (updateParts.Any())
                    {

                        var updateQuery = $"UPDATE CLAIMS_HCPCS_PROCEDURE_CODE SET {string.Join(", ", updateParts)} " +
                                          $"WHERE CLAIMS_HCPCS_PROCEDURE_CODE = '{claimHcpcs.CLAIMS_HCPCS_PROCEDURE_CODE}'," +
                                          $"LAST_MODIFIED_DATE_TIME='{DateTime.Now.ToString("MM/dd/yyyy")}',LAST_MODIFIED_USER='{modifiedUser}'";


                        var sqlBuilder = new System.Text.StringBuilder()
                            .AppendLine($"IF EXISTS (SELECT * FROM CLAIMS_HCPCS_PROCEDURE_CODE WHERE CLAIMS_HCPCS_PROCEDURE_CODE = '{claimHcpcs.CLAIMS_HCPCS_PROCEDURE_CODE}')")
                            .AppendLine("BEGIN")
                            .AppendLine(updateQuery)
                            .AppendLine("END")
                            .AppendLine().AppendLine();

                        string sqlQuery = sqlBuilder.ToString();

                        differences.Add(new QueryList() { Query = sqlQuery });


                    }
                }
            });

            foreach (QueryList query in differences)
            {
                File.AppendAllText(filePath, query.Query + Environment.NewLine + Environment.NewLine + Environment.NewLine);
            }

            XLandCSVOperations xLandCSVOperations = new XLandCSVOperations();
            DataTable dataTable = xLandCSVOperations.ToDataTable(differences);

            dataTable = dataTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull
            || string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable, "Query");
                wb.SaveAs($"C:\\Projects\\differences.xlsx");
                wb.Dispose();
                ReleaseComObject(wb);
            }
            Console.WriteLine("data processed and found differences");
        }
        private static List<STG_HCPCS_LVL2_CODE> LOAD_STG_HCPCS_LVL2_CODEs()
        {
            List<STG_HCPCS_LVL2_CODE> stgCodes = new List<STG_HCPCS_LVL2_CODE>();

            string filePath = "C:\\Projects\\STG_HCPCS_LVL2_CODE.xlsx";

            XLandCSVOperations xLandCSVOperations = new XLandCSVOperations();
            System.Data.DataTable dt = xLandCSVOperations.ImportExcel(filePath);
            stgCodes = xLandCSVOperations.GetSTG_HCPCS_LVL2_CODEs(dt);

            return stgCodes;

        }
        private static List<CLAIMS_HCPCS_PROCEDURE_CODE_DATA> LOAD_CLAIMS_HCPCS_PROCEDURE_CODE_DATA()
        {
            List<CLAIMS_HCPCS_PROCEDURE_CODE_DATA> claimsHCPCSData = new List<CLAIMS_HCPCS_PROCEDURE_CODE_DATA>();
            string filePath = "C:\\Projects\\CLAIMS_HCPCS_PROCEDURE_CODE_DATA.xlsx";

            XLandCSVOperations xLandCSVOperations = new XLandCSVOperations();
            System.Data.DataTable dt = xLandCSVOperations.ImportExcel(filePath);
            claimsHCPCSData = xLandCSVOperations.GetCLAIMS_HCPCS_PROCEDURE_CODE_DATA(dt);
            return claimsHCPCSData;

        }
        private static void ReleaseComObject(object obj)
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
