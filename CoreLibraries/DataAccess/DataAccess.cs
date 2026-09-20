using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

namespace MAXIMUS.Core.Libraries
{
    public class DataAccess
    {
        
        private static int defaultCommandTimeout = 720;
        private static int extraCommandTimeout = 1500;
        ////FIXME: Replaced static SqlConnection with local variable in each method to resolve connection pool problems in a multi-threading environment (cascading drop-downs).
        ////private static SqlConnection connection = null;

        #region "Public Methods"
        // 01/29/2013 MHH - Modified all SQLConnection access methods to incorporate the "using" block statement as it disposes of the connection correctly.
        //
        //          Example:    using (SqlConnection connection = new SqlConnection(connString))
        //

        public static void ExecuteNonQuery(string storedProcedureName)
        {
            Exception thrownException = null;
            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = defaultCommandTimeout;
                    cmd.ExecuteNonQuery();
                    connection.Close();
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

        public static string ExecuteScalar(string storedProcedureName, List<SqlParameter> inputParameters)
        {
            string returnData = string.Empty;
            Exception thrownException = null;
           
            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
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
                   connection.Close();
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

        public static string ExecuteScalar(string storedProcedureName, List<SqlParameter> inputParameters, CommandType commandType)
        {
	        string returnData = string.Empty;
	        Exception thrownException = null;

	        try
	        {
		        string connString = AppSettings.GetConnectionString();
		        using (SqlConnection connection = new SqlConnection(connString))
		        {
			        connection.Open();
			        SetArthAbortOnConnection(connection);

			        SqlCommand cmd = new SqlCommand();
			        cmd.Connection = connection;
			        cmd.CommandType = commandType;
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
                    connection.Close();
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





        public static DataSet ExecuteSelectSql(string connString, string selectSql)
        {
            DataSet returnData = new DataSet();
            Exception thrownException = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = selectSql;
                    cmd.CommandTimeout = defaultCommandTimeout;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(returnData);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                thrownException = ex;
                throw ex;
            }
            finally
            {
                WriteExecutionInformation(Constants.DataAccessCallType.SQL, selectSql, null, thrownException);
            }
            return (returnData);
        }

        public static DataSet ExecuteSelectSql(string selectSql)
        {
            return ExecuteSelectSql(AppSettings.GetConnectionString(), selectSql);
        }

        public static DataSet ExecuteStoredProcedure(string storedProcedureName)
        {
            DataSet returnData = new DataSet();
            Exception thrownException = null;

            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
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
                    connection.Close();
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

        public static DataSet ExecuteStoredProcedure(string storedProcedureName, string dataSetName)
        {
            DataSet returnData = new DataSet();
            Exception thrownException = null;

            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
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
                    connection.Close();
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

        public static object ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters, SqlDbType primaryKey)
        {
            int pkIndex;
            const string pk = "@PrimaryKey";
            object returnVal = null;
            Exception thrownException = null;

            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = defaultCommandTimeout;

                    SqlParameter parameter;
                    parameter = new SqlParameter(pk, primaryKey);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    parameters.Add(parameter);

                    foreach (SqlParameter prm in parameters)
                    {
                        cmd.Parameters.Add(prm);
                    }
                    cmd.ExecuteNonQuery();

                    pkIndex = cmd.Parameters.IndexOf(pk);
                    if (pkIndex >= 0)
                    {
                        returnVal = cmd.Parameters[pkIndex].Value;
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                thrownException = ex;
                string msg = ex.Message;
                throw ex;
            }
            finally
            {
                WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, parameters, thrownException);
            }
            return returnVal;
        }

        public static string ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters, string outputParmName
            , SqlDbType outputParmType, int Size = 0)
        {
            string returnVal = string.Empty;
            SqlParameter outputParm;
            Exception thrownException = null;

            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
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

                    if (Size == 0) outputParm = new SqlParameter("@" + outputParmName, outputParmType);
                    else outputParm = new SqlParameter("@" + outputParmName, outputParmType, Size);
                    outputParm.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParm);
                    cmd.ExecuteNonQuery();

                    returnVal = outputParm.Value.ToString();
                    connection.Close();
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
            return returnVal;
        }


        public static DataSet ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters, string outputParmName, SqlDbType outputParmType, out string outValue, int Size = 0)
        {
            DataSet returnData = new DataSet();
            outValue = string.Empty;
            SqlParameter outputParm;
            Exception thrownException = null;

            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
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

                    if (Size == 0) outputParm = new SqlParameter("@" + outputParmName, outputParmType);
                    else outputParm = new SqlParameter("@" + outputParmName, outputParmType, Size);
                    outputParm.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParm);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName);
                    dataAdapter.Fill(returnData);

                    outValue = outputParm.Value.ToString();
                    connection.Close();
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
            return (returnData);
        }

        public static void ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters)
        {
            int returnVal = 0;
            Exception thrownException = null;
            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = extraCommandTimeout;

                    foreach (SqlParameter prm in parameters)
                    {
                        cmd.Parameters.Add(prm);
                    }
                    WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, parameters);
                    returnVal = cmd.ExecuteNonQuery();
                    connection.Close();
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

        /// <summary>
        /// Use this in a tran
        /// </summary>
        /// <param name="tran">An existing database connection</param>
        /// <param name="storedProcedureName">SP Name</param>
        /// <param name="parameters">Parameters</param>
        public static void ExecuteStoredProcedure(SqlTransaction tran, string storedProcedureName, List<SqlParameter> parameters)
        {
            int returnVal = 0;
            Exception thrownException = null;
            try
            {
                if (tran == null || tran.Connection == null)
                {
                    throw new ArgumentException("Transaction or its connection is null.");
                }
                //string connString = AppSettings.GetConnectionString();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = tran.Connection;
                    cmd.Transaction = tran;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = extraCommandTimeout;

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

        public static void ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters
            , int commandTimeoutInSeconds)
        {
            int returnVal = 0;
            Exception thrownException = null;
            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = commandTimeoutInSeconds;

                    foreach (SqlParameter prm in parameters)
                    {
                        cmd.Parameters.Add(prm);
                    }
                    WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, parameters);
                    returnVal = cmd.ExecuteNonQuery();
                    connection.Close();
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

        public static DataSet ExecuteStoredProcedure(string connectionStringName, string storedProcedureName, List<SqlParameter> parameters, 
            string dataSetName = null)
        {
            DataSet returnData = new DataSet();
            Exception thrownException = null;

            try
            {
                string connectionString;
                if (!string.IsNullOrEmpty(connectionStringName)) connectionString = AppSettings.GetConnectionString(connectionStringName);
                else connectionString =  AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = defaultCommandTimeout;
                    if (parameters != null)
                    {
                        foreach (SqlParameter prm in parameters)
                        {
                            cmd.Parameters.Add(prm);
                        }
                    }

                    if (dataSetName != null) returnData.DataSetName = dataSetName;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, parameters);
                    dataAdapter.Fill(returnData);
                    connection.Close();
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
            return (returnData);
        }

        public static DataSet ExecuteStoredProcedure(string connectionStringName, string storedProcedureName, List<SqlParameter> parameters,
            string dataSetName = null, int commandTimeout = 720)
        {
            DataSet returnData = new DataSet();
            Exception thrownException = null;

            try
            {
                string connectionString;
                if (!string.IsNullOrEmpty(connectionStringName)) connectionString = AppSettings.GetConnectionString(connectionStringName);
                else connectionString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = commandTimeout;
                    if (parameters != null)
                    {
                        foreach (SqlParameter prm in parameters)
                        {
                            cmd.Parameters.Add(prm);
                        }
                    }

                    if (dataSetName != null) returnData.DataSetName = dataSetName;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    WriteExecutionInformation(Constants.DataAccessCallType.StoredProcedure, storedProcedureName, parameters);
                    dataAdapter.Fill(returnData);
                    connection.Close();
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
            return (returnData);
        }

        public static DataSet ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters, string dataSetName)
        {
            return ExecuteStoredProcedure(string.Empty, storedProcedureName, parameters, dataSetName);
        }

        public static DataSet ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters, string dataSetName, int commandTimeout)
        {
            return ExecuteStoredProcedure(string.Empty, storedProcedureName, parameters, dataSetName, commandTimeout);
        }

        public static string GetEnvironment()
        {
            return AppSettings.Get("Environment");
        }

        //Bug for branding
        /*public static string GetStateName()
        {
            return AppSettings.Get("StateName");
        }
        public static string GetBrandName()
        {
            return AppSettings.Get("BrandName");
        }*/
        public static string GetAppSetting(string key)
        {
            return AppSettings.Get(key);
        }
        

#endregion

        private static void WriteExecutionInformation(string dataAccessCallType, string itemName
            , List<SqlParameter> parameters = null, Exception exception = null)
        {
            Boolean sqlLogging = AppSettings.GetSQLLogging();

            if (sqlLogging)
            {
                const string fileName = "DataAccessLog.txt";
                string assemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
                string fullFileName = Path.Combine(assemblyPath, fileName);
                const string delimiter = "|";
                StringBuilder sb = new StringBuilder();
                System.IO.StreamWriter file = null;

                try
                {
                    if (!System.IO.File.Exists(fullFileName))
                    {
                        file = new System.IO.StreamWriter(fullFileName);
                        file.WriteLine("TimeStamp|Type|Name|Parameters[ParmName/ParmValue|]|Exception");
                    }
                    else
                    {
                        file = new System.IO.StreamWriter(fullFileName, true);
                    }
                    sb.Append(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                    sb.Append(delimiter);
                    sb.Append(dataAccessCallType);
                    sb.Append(delimiter);
                    sb.Append(itemName);
                    if (parameters != null)
                    {
                        foreach (SqlParameter prm in parameters)
                        {
                            sb.Append(delimiter);
                            sb.Append(prm.ParameterName);
                            sb.Append(@"/");
                            sb.Append(prm.Value.ToString());
                        }
                    }
                    sb.Append(delimiter);
                    if (exception != null)
                    {
                        sb.Append(exception.Message);
                    }
                    file.WriteLine(sb.ToString());
                }
                catch
                {
                    // no action
                }
                finally
                {
                    if (file != null)
                    {
                        file.Close();
                    }
                }
            }
        }
    }
}
