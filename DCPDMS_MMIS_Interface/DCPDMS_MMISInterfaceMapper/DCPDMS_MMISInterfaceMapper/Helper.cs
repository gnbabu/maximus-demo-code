using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Configuration;
using System.Web;
using System.Reflection;

namespace DCPDMS_MMISInterfaceMapper
{
    public class Helper
    {
        public static decimal DeNullable(decimal? num)
        {
            return (num == null ? 0.0M : (decimal)num);
        }

        public static decimal DeNullable(object num)
        {
            return num == null || num == DBNull.Value ? 0.0M : (decimal)num;
        }

        /// <summary>
        /// Creates a multi-value string from an object array
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string CreateMultiValueString(List<object> arr)
        {
            StringBuilder retMVString = new StringBuilder();
            for (int index = 0; index < arr.Count(); index++)
            {
                retMVString.Append(arr[index].ToString() + "|");
            }

            if (retMVString.Length > 0)
            {
                retMVString.Remove(retMVString.Length - 1, 1);
            }

            return retMVString.ToString();
        }

     
        public static List<string> ParseMultiValueString(string mvString)
        {
            return mvString.Split('|').ToList();
        }

        public static void AddLogEntry(string sMsg, string baseDirectory = "")
        {
            string logFormat;
            string pathName;
            string fileName;

            // pathName used to create log folder format: ~\Log\YYYYMMMM
            if (baseDirectory == "")
            {
                pathName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            }
            else
            {
                pathName = Path.Combine(baseDirectory, "Logs");
            }
            if (!(Directory.Exists(pathName)))
            {
                Directory.CreateDirectory(pathName);
            }
            pathName = Path.Combine(pathName,
                DateTime.Today.ToString("yyyy")
                + DateTime.Today.ToString("MMMM"));
            if (!(Directory.Exists(pathName)))
            {
                Directory.CreateDirectory(pathName);
            }

            // fileName used to create log filename format: YYYYMMDD.txt
            fileName = DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".txt";

            // logFormat used to create log entry format: dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            logFormat = DateTime.UtcNow.ToShortDateString().ToString() + " " + DateTime.UtcNow.ToString("HH:mm:ss.ffffff") + " ==> ";

            StreamWriter sw = new StreamWriter(Path.Combine(pathName, fileName), true);
            sw.WriteLine(logFormat + sMsg);
            sw.Flush();
            sw.Close();
        }

        public static DataRow CopyGenericToDataTable<T>(T item)
        {
            var properties = typeof(T).GetProperties();
            var result = new DataTable();

            //Build the columns
            foreach (var prop in properties)
            {
                if (!prop.PropertyType.FullName.Contains("Null"))
                {
                    result.Columns.Add(prop.Name, prop.PropertyType);
                }
            }

            //Fill the DataTable
            var row = result.NewRow();

            foreach (var prop in properties)
            {
                if (result.Columns.Contains(prop.Name))
                {
                    var itemValue = prop.GetValue(item, new object[] { });
                    row[prop.Name] = itemValue;
                }
            }

            return row;
        }

        [Obsolete("This is used for copying from file shares instead of virtual directories.")]
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }


        public static string ErrorMsgFromExceptionChain(Exception ex, bool includeStackTrace = false)
        {
            string rtn = "";

            rtn = ex.Message + "\r\n";

            if (includeStackTrace)
            {
                rtn += ex.StackTrace + "\r\n";
            }

            while (ex.InnerException != null)
            {
                rtn += ex.InnerException.Message + "\r\n";
                if (includeStackTrace)
                {
                    rtn += ex.StackTrace + "\r\n";
                }

                ex = ex.InnerException;
            }

            while (rtn.Contains("\r\n \r\n"))
            {
                
                rtn = rtn.Replace("\r\n \r\n", "\r\n");
            }

            return rtn;
        }

        /// <summary>
        /// Gets an integer from the datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="dr">data row to get data from</param>
        /// <returns>integer from the data row</returns>
        public static int GetInt(string elementName, DataRow dr)
        {
            int returnValue = 0;
            if (!dr.IsNull(elementName))
            {
                returnValue = Convert.ToInt32(dr[elementName]);
            }
            return returnValue;
        }


        /// <summary>
        /// Gets a boolean from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>boolean from datarow</returns>
        public static bool GetBool(string elementName, DataRow dr)
        {
            bool returnValue = false;
            if (!dr.IsNull(elementName))
            {
                returnValue = Convert.ToBoolean(dr[elementName]);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a decimal from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>boolean from datarow</returns>
        public static decimal GetDecimal(string elementName, DataRow dr)
        {
            decimal returnValue = 0.0M;
            if (!dr.IsNull(elementName))
            {
                returnValue = Convert.ToDecimal(dr[elementName]);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a DateTime from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>boolean from datarow</returns>
        public static DateTime GetDateTime(string elementName, DataRow dr)
        {
            DateTime returnValue = new DateTime(1753, 1, 1);
            if (!dr.IsNull(elementName))
            {
                returnValue = Convert.ToDateTime(dr[elementName]);
            }

            return returnValue;
        }

        public static Guid GetGuid(string elementName, DataRow dr)
        {
            Guid returnValue = Guid.Empty;
            if (!dr.IsNull(elementName))
            {
                returnValue = (Guid)dr[elementName];
            }

            return returnValue;
        }
        /// <summary>
        /// Gets an integer from the datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="dr">data row to get data from</param>
        /// <returns>integer from the data row</returns>
        public static int GetInt(string elementName, SqlDataReader dr)
        {
            int returnValue = 0;
            if (!dr.IsDBNull(dr.GetOrdinal(elementName)))
            {
                returnValue = Convert.ToInt32(dr[elementName]);
            }
            return returnValue;
        }


        /// <summary>
        /// Gets a string from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>string from the data row</returns>
        public static string GetString(string elementName, SqlDataReader dr)
        {
            string returnValue = "";
            if (!dr.IsDBNull(dr.GetOrdinal(elementName)))
            {
                returnValue = dr[elementName].ToString().Replace("''", "'");
               // returnValue = HttpUtility.HtmlDecode(returnValue);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a boolean from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>boolean from datarow</returns>
        public static bool GetBool(string elementName, SqlDataReader dr)
        {
            bool returnValue = false;
            if (!dr.IsDBNull(dr.GetOrdinal(elementName)))
            {
                returnValue = Convert.ToBoolean(dr[elementName]);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a decimal from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>boolean from datarow</returns>
        public static decimal GetDecimal(string elementName, SqlDataReader dr)
        {
            decimal returnValue = 0.0M;
            if (!dr.IsDBNull(dr.GetOrdinal(elementName)))
            {
                returnValue = Convert.ToDecimal(dr[elementName]);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a DateTime from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>boolean from datarow</returns>
        public static DateTime GetDateTime(string elementName, SqlDataReader dr)
        {
            DateTime returnValue = new DateTime(1753, 1, 1);
            if (!dr.IsDBNull(dr.GetOrdinal(elementName)))
            {
                returnValue = Convert.ToDateTime(dr[elementName]);
            }

            return returnValue;
        }

        public static Guid GetGuid(string elementName, SqlDataReader dr)
        {
            Guid returnValue = Guid.Empty;
            if (!dr.IsDBNull(dr.GetOrdinal(elementName)))
            {
                returnValue = (Guid)dr[elementName];
            }

            return returnValue;
        }
        /// <summary>
        /// Checks to see if a DataSet has at least one row in one table.
        /// </summary>
        /// <param name="ds">The DataSet to check for the existence of rows.</param>
        /// <returns>Returns true if the DataSet has rows. Otherwise it returns false.</returns>
        public static bool HasRows(DataSet ds)
        {
            return (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
        }
        /// <summary>
        /// Convenience function to execute an sqlStatement
        /// </summary>
        /// <param name="sqlStatement">sql statement to execute</param>
        /// <returns>results of the sql statement</returns>
        public static DataSet ExecuteSql(string sqlStatement)
        {
            SqlConnection conn = null;
            DataSet returnData = new DataSet();
            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sqlStatement, conn);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(returnData);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }


            return returnData;
        }

        /// <summary>
        /// Convenience function to execute an sqlStatement
        /// </summary>
        /// <param name="sqlStatement">sql statement to execute</param>
        /// <returns>results of the sql statement</returns>
        public static DataSet ExecuteSql(string sqlStatement, bool isOnline)
        {
            SqlConnection conn = null;
            DataSet returnData = new DataSet();
            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sqlStatement, conn);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(returnData);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }


            return returnData;
        }


        /// <summary>
        /// Convenience function to execute an sqlStatement
        /// </summary>
        /// <param name="sqlStatement">sql statement to execute</param>
        /// <param name="sqlParams">parameters to pass to the executing statement</param>
        /// <returns>results of the sql statement</returns>
        public static object ExecuteSqlScalar(string sqlStatement, List<SqlParameter> sqlParams)
        {
            SqlConnection conn = null;
            object rtn = null;
            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStatement, conn);
                cmd.CommandTimeout = 0;
                foreach (SqlParameter sqlParam in sqlParams)
                {
                    cmd.Parameters.Add(sqlParam);
                }
                rtn = cmd.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return rtn;
        }

        /// <summary>
        /// Convenience function to execute an sqlStatement
        /// </summary>
        /// <param name="sqlStatement">sql statement to execute</param>
        /// <param name="sqlParams">parameters to pass to the executing statement</param>
        /// <returns>results of the sql statement</returns>
        public static object ExecuteSqlScalar(string sqlStatement, List<SqlParameter> sqlParams, bool isOnline)
        {
            SqlConnection conn = null;
            object rtn = null;
            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStatement, conn);
                cmd.CommandTimeout = 0;
                foreach (SqlParameter sqlParam in sqlParams)
                {
                    cmd.Parameters.Add(sqlParam);
                }
                rtn = cmd.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return rtn;
        }


        /// <summary>
        /// Convenience function to execute an sqlStatement
        /// </summary>
        /// <param name="sqlStatement">sql statement to execute</param>
        /// <param name="sqlParams">parameters to pass to the executing statement</param>
        /// <returns>results of the sql statement</returns>
        public static object ExecuteSqlScalar(string sqlStatement, List<SqlParameter> sqlParams, SqlTransaction trans)
        {
            SqlConnection conn = null;
            object rtn = null;
            try
            {
                conn = trans.Connection;
                SqlCommand cmd = new SqlCommand(sqlStatement, conn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = trans;
                foreach (SqlParameter sqlParam in sqlParams)
                {
                    cmd.Parameters.Add(sqlParam);
                }
                rtn = cmd.ExecuteScalar();
            }
            catch
            {
                throw;
            }

            return rtn;
        }

        /// <summary>
        /// Executes a SQL statement and returns a DataSet with the results.
        /// </summary>
        /// <param name="sqlStatement">SQL statement to execute.</param>
        /// <param name="trans" >Transaction within which to execute the SQL statement. This can be used to chain multiple SQL calls.</param>
        /// <returns>A DataSet holding the results of the query.</returns>
        public static DataSet ExecuteSql(string sqlStatement, SqlTransaction trans)
        {
            DataSet returnData = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = trans.Connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStatement;
                cmd.Transaction = trans;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(returnData);
            }
            catch
            {
                throw;
            }

            return (returnData);

        }

        /// <summary>
        /// Creates a SQL transaction.
        /// </summary>
        /// <returns>Returns a SQL transaction.</returns>
        public static SqlTransaction CreateTransaction()
        {
            SqlConnection conn = null;
            SqlTransaction retTransaction = null;
            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
                retTransaction = conn.BeginTransaction();
            }
            catch
            {
                if (retTransaction != null)
                {
                    retTransaction = null;
                }

                if (conn != null)
                {
                    conn.Close();
                    conn = null;
                }
                throw;
            }

            return retTransaction;
        }

        public static SqlTransaction CreateTransaction(bool isOnline)
        {
            SqlConnection conn = null;
            SqlTransaction retTransaction = null;
            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
                retTransaction = conn.BeginTransaction();
            }
            catch
            {
                if (retTransaction != null)
                {
                    retTransaction = null;
                }

                if (conn != null)
                {
                    conn.Close();
                    conn = null;
                }
                throw;
            }

            return retTransaction;
        }


        /// <summary>
        /// Executes a SQL statement using a list of passed in SQLParameters.
        /// </summary>
        /// <param name="sqlStatement">SQL statement to execute.</param>
        /// <param name="sqlParams">A list of SQLParameter objects.</param>
        /// <returns>A DataSet holding the results of the query.</returns>
        public static DataSet ExecuteSql(string sqlStatement, List<SqlParameter> sqlParams)
        {
            DataSet returnData = new DataSet();

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlStatement, GetConnectionString());
                foreach (SqlParameter sqlParam in sqlParams)
                {
                    da.SelectCommand.Parameters.Add(sqlParam);
                }

                da.Fill(returnData);
            }
            catch
            {
                throw;
            }

            return (returnData);

        }

        /// <summary>
        /// Executes a SQL statement using a list of passed in SQLParameters.
        /// </summary>
        /// <param name="sqlStatement">SQL statement to execute.</param>
        /// <param name="sqlParams">A list of SQLParameter objects.</param>
        /// <returns>A DataSet holding the results of the query.</returns>
        public static DataSet ExecuteSql(string sqlStatement, List<SqlParameter> sqlParams, bool isOnline)
        {
            DataSet returnData = new DataSet();

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlStatement, GetConnectionString());
                foreach (SqlParameter sqlParam in sqlParams)
                {
                    da.SelectCommand.Parameters.Add(sqlParam);
                }

                da.Fill(returnData);
            }
            catch
            {
                throw;
            }

            return (returnData);

        }


        /// <summary>
        /// Executes a SQL statement. This method takes a list of sql parameters and a transaction as parameters.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement to execute.</param>
        /// <param name="sqlParams">A list of SQL parameters to be used when executing the sql statement.</param>
        /// <param name="trans">An existing SQL transaction.</param>
        /// <returns>Returns a DataSet.</returns>
        public static DataSet ExecuteSql(string sqlStatement, List<SqlParameter> sqlParams, SqlTransaction trans)
        {
            DataSet returnData = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = trans.Connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStatement;
                cmd.Transaction = trans;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                foreach (SqlParameter sqlParam in sqlParams)
                {
                    da.SelectCommand.Parameters.Add(sqlParam);
                }
                da.Fill(returnData);
            }
            catch
            {
                throw;
            }

            return (returnData);


        }

        /// <summary>
        /// Executes a stored procedure using the passed in list of SQLParameter objects.
        /// </summary>
        /// <param name="spName">Name of the stored procedure to execute.</param>
        /// <param name="parameters">A list of SQLParamenter objects to pass to the stored procedure call.</param>
        /// <returns>A DataSet holding the results of the stored procedure call.</returns>
        public static DataSet ExecuteStoredProcedure(string spName, List<SqlParameter> parameters, string connStringName = "")
        {
            SqlConnection conn = null;
            DataSet returnData = new DataSet();

            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                cmd.CommandTimeout = 0;
                foreach (SqlParameter prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(returnData);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return (returnData);
        }

        /// <summary>
        /// Executes a stored procedure using the passed in list of SQLParameter objects.
        /// </summary>
        /// <param name="spName">Name of the stored procedure to execute.</param>
        /// <param name="parameters">A list of SQLParamenter objects to pass to the stored procedure call.</param>
        /// <returns>A DataSet holding the results of the stored procedure call.</returns>
        public static DataSet ExecuteStoredProcedure(string spName, List<SqlParameter> parameters, bool isOnline)
        {
            SqlConnection conn = null;
            DataSet returnData = new DataSet();

            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                cmd.CommandTimeout = 0;
                foreach (SqlParameter prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(returnData);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return (returnData);
        }

        /// <summary>
        /// Returns a string representation of the numeric month.
        /// </summary>
        /// <param name="month">A numeric representation of the month.</param>
        /// <returns></returns>
        public static string GetMonthFromInt(int month)
        {
            string retName = "Unknown";
            switch (month)
            {
                case 1:
                    retName = "January";
                    break;

                case 2:
                    retName = "February";
                    break;

                case 3:
                    retName = "March";
                    break;

                case 4:
                    retName = "April";
                    break;

                case 5:
                    retName = "May";
                    break;

                case 6:
                    retName = "June";
                    break;

                case 7:
                    retName = "July";
                    break;

                case 8:
                    retName = "August";
                    break;

                case 9:
                    retName = "September";
                    break;

                case 10:
                    retName = "October";
                    break;

                case 11:
                    retName = "November";
                    break;

                case 12:
                    retName = "December";
                    break;
            }

            return retName;
        }

        /// <summary>
        /// Executes a stored procedure using the passed in list of SQLParameter objects.
        /// </summary>
        /// <param name="spName">Name of the stored procedure to execute.</param>
        /// <param name="parameters">A list of SQLParamenter objects to pass to the stored procedure call.</param>
        /// <param name="trans">A SQLTransaction object. </param>
        /// <returns>A DataSet holding the results of the stored procedure call.</returns>
        public static DataSet ExecuteStoredProcedure(string spName, List<SqlParameter> parameters, SqlTransaction trans)
        {
            DataSet returnData = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = trans.Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                cmd.Transaction = trans;

                foreach (SqlParameter prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(returnData);
            }
            catch
            {
                throw;
            }

            return (returnData);
        }

        /// <summary>
        /// Executes a stored procedure using the passed in list of SQLParameter objects.
        /// </summary>
        /// <param name="spName">Name of the stored procedure to execute.</param>
        /// <param name="parameters">A list of SQLParamenter objects to pass to the stored procedure call.</param>
        /// <returns>A DataSet holding the results of the stored procedure call.</returns>
        public static int ExecuteStoredProcedure2(string spName, List<SqlParameter> parameters)
        {
            SqlConnection conn = null;
            int li_rows_affected = -1;
            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                cmd.CommandTimeout = 0;
                foreach (SqlParameter prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }
                li_rows_affected = Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return (li_rows_affected);
        }

        public static int ExecuteStoredProcedure2(string spName, List<SqlParameter> parameters, bool isOnline)
        {
            SqlConnection conn = null;
            int li_rows_affected = -1;
            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                cmd.CommandTimeout = 0;
                foreach (SqlParameter prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }
                li_rows_affected = Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return (li_rows_affected);
        }


        /// <summary>
        /// Gets the connection string from the app config file
        /// </summary>
        /// <returns>connection string</returns>
        public static String GetConnectionString()
        {
            string retConnString = "";
            ConnectionStringSettingsCollection connectionStrings = null;
            try
            {
                connectionStrings = ConfigurationManager.ConnectionStrings as ConnectionStringSettingsCollection;
            }
            catch
            {
                connectionStrings = null;
            }

            retConnString = connectionStrings["InterfaceMetaDataConn"].ConnectionString;
            return retConnString;
        }

        /// <summary>
        /// Gets an integer from the first row of a dataset
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data set to get data from</param>
        /// <returns>integer from the data set</returns>
        public static int GetInt(string elementName, DataSet ds)
        {
            int returnValue = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!ds.Tables[0].Rows[0].IsNull(elementName))
                {
                    returnValue = Convert.ToInt32(ds.Tables[0].Rows[0][elementName]);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Gets a string from the first row of a dataset
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data set to get data from</param>
        /// <returns>string from the dataset</returns>
        public static string GetString(string elementName, DataSet ds)
        {
            string returnValue = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!ds.Tables[0].Rows[0].IsNull(elementName))
                {
                    returnValue = ds.Tables[0].Rows[0][elementName].ToString().Replace("''", "'");
                    //returnValue = HttpUtility.HtmlDecode(returnValue);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Formats a safe string, replacing two single quotes with one single quote.
        /// </summary>
        /// <param name="astr_data">The string to be formatted.</param>
        /// <returns>Returns a string.</returns>
        public static string GetString(object astr_data)
        {
            string returnValue = "";
            returnValue = astr_data.ToString().Replace("''", "'");
          //  returnValue = HttpUtility.HtmlDecode(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets a boolean from the first row of a dataset
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data set to get data from</param>
        /// <returns>boolean from dataset</returns>
        public static bool GetBool(string elementName, DataSet ds)
        {
            bool returnValue = false;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!ds.Tables[0].Rows[0].IsNull(elementName))
                {
                    returnValue = Convert.ToBoolean(ds.Tables[0].Rows[0][elementName]);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Gets a decimal from the first row of a dataset
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data set to get data from</param>
        /// <returns>boolean from dataset</returns>
        public static decimal GetDecimal(string elementName, DataSet ds)
        {
            decimal returnValue = 0.0M;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!ds.Tables[0].Rows[0].IsNull(elementName))
                {
                    returnValue = Convert.ToDecimal(ds.Tables[0].Rows[0][elementName]);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Gets a DateTime from the first row of a dataset
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data set to get data from</param>
        /// <returns>boolean from dataset</returns>
        public static DateTime GetDateTime(string elementName, DataSet ds)
        {
            DateTime returnValue = new DateTime(1753, 1, 1);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!ds.Tables[0].Rows[0].IsNull(elementName))
                {
                    returnValue = Convert.ToDateTime(ds.Tables[0].Rows[0][elementName]);
                }
            }
            return returnValue;
        }


        /// <summary>
        /// Returns a safe integer, handling the case where the passed-in value is a DBNull.
        /// </summary>
        /// <param name="value">The value to return as an integer.</param>
        /// <returns>Returns an integer.</returns>
        public static int GetInt(object value)
        {
            int returnValue = 0;

            if (!(value.GetType() == DBNull.Value.GetType()))
            {
                returnValue = (int)value;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a string from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>string from the data row</returns>
        public static string GetString(string elementName, DataRow dr)
        {
            string returnValue = "";
            if (!dr.IsNull(elementName))
            {
                returnValue = dr[elementName].ToString().Replace("''", "'");
              //  returnValue = HttpUtility.HtmlDecode(returnValue);
            }

            return returnValue;
        }

        public static bool WebDirectoryExists(string uri)
        {
            bool rtn = false;

            try
            {
                // Test that the directory can be opened
                System.Net.WebClient webClient = new System.Net.WebClient();
                Stream stream = webClient.OpenRead(uri);
                stream.Close();
                rtn = true;
            }
            catch (System.Net.WebException webEx)
            {
                // If the server is accessible, but the directory isn't found
                if (webEx.Response != null &&
                    webEx.Response.GetType() == typeof(System.Net.HttpWebResponse))
                {
                    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)webEx.Response;
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        rtn = false;
                    }
                }

                // Otherwise, if the server can't be reached
            }
            catch (Exception ex)
            {
                // All other errors
                throw ex;
            }

            return rtn;
        }

        /// <summary>
        /// Gets the property for the given object
        /// </summary>
        /// <typeparam name="T">type of the object</typeparam>
        /// <param name="propertyName">name of property to get</param>
        /// <param name="sourceObject">object that contains property</param>
        /// <returns></returns>
        public static object GetGenericProperty<T>(string propertyName, object sourceObject)
        {
            object retProperty = null;

            // get the type's property info
            PropertyInfo propInfo = typeof(T).GetProperty(propertyName);

            // if the property info is found...
            if (propInfo != null)
            {
                // get the value for the property
                retProperty = propInfo.GetValue(sourceObject);
            }

            return retProperty;
        }

        /// <summary>
        /// Gets the property for the given object
        /// </summary>
        /// <typeparam name="T">type of the object</typeparam>
        /// <param name="propertyName">name of property to get</param>
        /// <param name="sourceObject">object that contains property</param>
        /// <returns></returns>
        public static void SetGenericProperty<T>(string propertyName, object newValue, object targetObject)
        {
            // get the type's property info
            PropertyInfo propInfo = typeof(T).GetProperty(propertyName);

            // if the property info is found...
            if (propInfo != null)
            {
                // set the value for the property
                propInfo.SetValue(targetObject, newValue);
            }
        }
    }
}
