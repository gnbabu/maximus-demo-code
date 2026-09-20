using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace TibcoDocImportNetCore
{
    public class DataAccess
    {

        private static int defaultCommandTimeout = 340;
        ////FIXME: Replaced static SqlConnection with local variable in each method to resolve connection pool problems in a multi-threading environment (cascading drop-downs).
        ////private static SqlConnection connection = null;

        #region "Public Methods"
        // 01/29/2013 MHH - Modified all SQLConnection access methods to incorporate the "using" block statement as it disposes of the connection correctly.
        //
        //          Example:    using (SqlConnection connection = new SqlConnection(connString))
        //

        public static void ExecuteNonQuery(string storedProcedureName, string cnString)
        {
            Exception thrownException = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(cnString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = defaultCommandTimeout;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                thrownException = ex;
                throw ex;
            }
            finally
            {
                WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, null, thrownException);
            }
        }

        public static string ExecuteScalar(string storedProcedureName, List<SqlParameter> inputParameters, string cnString)
        {
            string returnData = string.Empty;
            Exception thrownException = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(cnString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = defaultCommandTimeout;

                    if (inputParameters != null)
                    {
                        foreach (SqlParameter prm in inputParameters)
                        {
                            cmd.Parameters.Add(prm);
                        }
                    }
                    returnData = Convert.ToString(cmd.ExecuteScalar());
                    //inputParameters.Clear();
                    // connection.Close();
                }

            }
            catch (Exception ex)
            {
                thrownException = ex;
                throw ex;
            }
            finally
            {
                WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, inputParameters, thrownException);
            }
            return (returnData);
        }



        private static void SetArthAbortOnConnection(SqlConnection connection)
        {
            using (SqlCommand comm = new SqlCommand("SET ARITHABORT ON", connection))
            {
                comm.ExecuteNonQuery();
            }
        }

        public static DataSet ExecuteStoredProcedure(string storedProcedureName, string cnString)
        {
            DataSet returnData = new DataSet();
            Exception thrownException = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(cnString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = defaultCommandTimeout;

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName);
                    dataAdapter.Fill(returnData);
                }
            }
            catch (Exception ex)
            {
                thrownException = ex;
                throw ex;
            }
            finally
            {
                WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, null, thrownException);
            }
            return (returnData);
        }

        public static DataSet ExecuteStoredProcedure(string storedProcedureName, string dataSetName, string cnString)
        {
            DataSet returnData = new DataSet();
            Exception thrownException = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(cnString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = defaultCommandTimeout;

                    returnData.DataSetName = dataSetName;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(returnData);
                }
            }
            catch (Exception ex)
            {
                thrownException = ex;
                throw ex;
            }
            finally
            {
                WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, null, thrownException);
            }
            return (returnData);
        }


        public static void ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters, string cnString)
        {
            int returnVal = 0;
            Exception thrownException = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(cnString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = defaultCommandTimeout;

                    foreach (SqlParameter prm in parameters)
                    {
                        cmd.Parameters.Add(prm);
                    }
                    WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, parameters);
                    returnVal = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                thrownException = ex;
                throw ex;
            }
            finally
            {
                WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, parameters, thrownException);
            }
        }

        //public static string GetEnvironment()
        //{
        //    return AppSettings.Get("Environment");
        //}

        ////Bug for branding
        ///*public static string GetStateName()
        //{
        //    return AppSettings.Get("StateName");
        //}
        //public static string GetBrandName()
        //{
        //    return AppSettings.Get("BrandName");
        //}*/
        //public static string GetAppSetting(string key)
        //{
        //    return AppSettings.Get(key);
        //}


        #endregion

        private static void WriteExecutionInformation(string dataAccessCallType, string itemName
            , List<SqlParameter> parameters = null, Exception exception = null)
        {
            //Boolean sqlLogging = AppSettings.GetSQLLogging();

            //if (sqlLogging)
            //{
            //    const string fileName = "DataAccessLog.txt";
            //    string assemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            //    string fullFileName = Path.Combine(assemblyPath, fileName);
            //    const string delimiter = "|";
            //    StringBuilder sb = new StringBuilder();
            //    System.IO.StreamWriter file = null;

            //    try
            //    {
            //        if (!System.IO.File.Exists(fullFileName))
            //        {
            //            file = new System.IO.StreamWriter(fullFileName);
            //            file.WriteLine("TimeStamp|Type|Name|Parameters[ParmName/ParmValue|]|Exception");
            //        }
            //        else
            //        {
            //            file = new System.IO.StreamWriter(fullFileName, true);
            //        }
            //        sb.Append(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            //        sb.Append(delimiter);
            //        sb.Append(dataAccessCallType);
            //        sb.Append(delimiter);
            //        sb.Append(itemName);
            //        if (parameters != null)
            //        {
            //            foreach (SqlParameter prm in parameters)
            //            {
            //                sb.Append(delimiter);
            //                sb.Append(prm.ParameterName);
            //                sb.Append(@"/");
            //                sb.Append(prm.Value.ToString());
            //            }
            //        }
            //        sb.Append(delimiter);
            //        if (exception != null)
            //        {
            //            sb.Append(exception.Message);
            //        }
            //        file.WriteLine(sb.ToString());
            //    }
            //    catch
            //    {
            //        // no action
            //    }
            //    finally
            //    {
            //        if (file != null)
            //        {
            //            file.Close();
            //        }
            //    }
            //}
        }
    }
}
