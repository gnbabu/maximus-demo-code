using CommandLine;
using CommandLine.Text;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using System.Xml;
using Microsoft.Extensions.Configuration;

// Database objects generated for this class:
//  TABLEs:     AppSettings
//  SPs:        sp_AppSettingsUpdateValue, sp_AppSettingsSetValue, sp_AppSettingsGetValue
//  VIEWs:  
//  FUNCTIONs:  

// TODO: jfetters - add functionality to generate exception if attempted update to setting if it is read only.
// TODO: jfetters - add functionality to generate exception if attempting to add setting over existing read only setting.
// TODO: jfetters - add functionality to encrypt setting value field.
// TODO: jfetters - add functionality to delete setting value (if not read only).

namespace TibcoDocImportNetCore
{

    public class Options
    {
        [Option('d', "database", Required = true,
          HelpText = "Database to be used for running jobs")]
        public string DBConnectionStringName { get; set; }

        //[ParserState]
        //public CommandLine.IParserState LastParserState { get; set; }

        //[HelpOption]
        //public string GetUsage()
        //{
        //    return HelpText.AutoBuild(this,
        //      (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        //}
    }


    /// <summary>
    /// 
    /// </summary>
    public class AppSettings
    {

        private string returnValue = string.Empty;

        #region "Constructors"

        /// <summary>
        ///     The default parameterless constructor. Generates a new GUID for the Logging threadId
        /// </summary>
        public AppSettings()
        {
            // generate a new thread id GUID
            this.ThreadId = Guid.NewGuid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadId"></param>
        public AppSettings(Guid threadId)
        {
            this.ThreadId = threadId;
        }

        #endregion

        #region "Logging Objects"

        //// private int logCnt = 0;
        private Guid threadId;
        private Guid ThreadId
        {
            get
            {
                return this.threadId;
            }
            set
            {
                this.threadId = value;
            }
        }

        #endregion

        # region "Public Methods"

        /// <summary>
        ///     Retrieves a key from the database
        /// </summary>
        /// <param name="key">The name of the key</param>
        /// <returns>The value of the key</returns>
        public static string Get(string key)
        {
            string value = string.Empty;
            //if (!DictionaryCache.Exists(key))
            //{
            //    value = GetValue(key);
            //    DictionaryCache.Add<string>(key, value);
            //}
            //else
            //    value = DictionaryCache.Get<string>(key);
            value = GetValue(key);
            return value;
        }

        /// <summary>
        ///     Retrieves a key from the database
        /// </summary>
        /// <param name="key">The name of the key</param>
        /// <param name="defaultValue">The value to return if the key is not being stored</param>
        /// <returns>The value of the key or the default value provided</returns>
        public static string Get(string key, string defaultValue)
        {
            string value = string.Empty;
            //if (!DictionaryCache.Exists(key))
            //{
            //    value = GetValue(key, defaultValue);
            //    DictionaryCache.Add<string>(key, value);
            //}
            //else
            //    value = DictionaryCache.Get<string>(key);
            value = GetValue(key, defaultValue);
            return value;
        }

        /// <summary>
        ///     Retrieves a key from the database
        /// </summary>
        /// <param name="connectionStringName">the name of the database to use for retrieving values</param>
        /// <param name="key">The name of the key</param>
        /// <param name="defaultValue">The value to return if the key is not being stored</param>
        /// <returns></returns>
        public static string Get(string connectionStringName, string key, string defaultValue)
        {
            string value = string.Empty;
            //if (!DictionaryCache.Exists(key))
            //{
            //    value = GetValue(key, defaultValue, connectionStringName);
            //    DictionaryCache.Add<string>(key, value);
            //}
            //else
            //{
            //    value = DictionaryCache.Get<string>(key);
            //}
            value = GetValue(key, defaultValue, connectionStringName);
            return value;
        }

        /// <summary>
        ///     The SQL connection string to the database (not an OLEDB compliant connection string)
        /// </summary>
        /// <returns>The connection string</returns>
        public static string GetConnectionString(string connectionStringName)
        {
            try
            {
                // return Resource1.mainDB; 

                // Configuration appConfig = ConfigurationManager.OpenExeConfiguration("AppSettings.dll");
                //string connString = appConfig.ConnectionStrings.ConnectionStrings["mainDB"].ConnectionString;
                //return connString;

                IConfiguration cfg = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
                string connString = cfg.GetSection(connectionStringName).Value;

                if (connString == null)
                {
                    throw new Exception("Unable to retrieve connection string, check for existence of AppSettings configuration file or connection string value");
                }

                return connString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     The SQL connection string to the database (not an OLEDB compliant connection string)
        /// </summary>
        /// <returns>The connection string</returns>
        public static string GetConnectionString()
        {
            //if (IsRunningUnderService())
            //{
            //    var commandLineArgs = Environment.GetCommandLineArgs();
            //    Options options = new Options();
            //    CommandLine.Parser.Default.ParseArguments(commandLineArgs, options);

            //    return GetConnectionString(options.DBConnectionStringName);

            //}
            return GetConnectionString("mainDB");
        }

        /// <summary>
        /// Identifies if the application is running under a service
        /// </summary>
        /// <returns></returns>
        public static bool IsRunningUnderService()
        {
            bool isService = false;
            if (Process.GetCurrentProcess().ProcessName == "JobService")
            {
                isService = true;
            }

            return isService;
        }

        /// <summary>
        ///     Sets a new key value based on the parameters provided.
        /// </summary>
        /// <param name="key">The name of the key</param>
        /// <param name="value">The value to store for the key, what is returned with the Get method</param>
        /// <param name="readOnly">Indicates if the key should be read-only once saved</param>
        public static void Set(string key, string value, bool readOnly)
        {
            SetValue(key, value, readOnly);
        }

        #endregion

        # region "Private Methods"

        /// <summary>
        ///     Retreives the current system user
        /// </summary>
        /// <returns>The domain\username of the user being used for the running process</returns>
        //private static string GetCurrentUser()
        //{
        //    string returnValue = string.Empty;
        //    try
        //    {
        //        // Example: "CORP\jfetters"
        //        string name = WindowsIdentity.GetCurrent().Name;
        //    }
        //    catch
        //    {
        //        // no action     
        //        if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
        //    }
        //    return returnValue;
        //}

        private static string GetValue(string key, string defaultValue = "", string connectionStringName = "")
        {
            SqlConnection dbConnection = new SqlConnection();
            string returnValue = string.Empty;
            object scalarResults;

            try
            {
                returnValue = defaultValue;
                dbConnection = GetConnection(connectionStringName);

                // set up the command, parameters and execute to get setting
                SqlCommand cmd = new SqlCommand("sp_AppSettingsGetValue", dbConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = new SqlParameter("@key", key);
                cmd.Parameters.Add(parm);
                scalarResults = cmd.ExecuteScalar();

                // if the execution returned results
                if (scalarResults != null)
                {
                    returnValue = Convert.ToString(scalarResults);
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
                throw;
            }
            finally
            {
                if (dbConnection != null)
                {
                    dbConnection.Close();
                }
            }
        }

        private static void SetValue(string key, string value, bool readOnly)
        {
            SqlConnection dbConnection = new SqlConnection();
            string returnValue = string.Empty;

            try
            {
                // get the connection
                dbConnection = GetConnection();

                // set up the command, parameters and execute to get setting
                SqlCommand cmd = new SqlCommand("sp_AppSettingsSetValue", dbConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter parm1 = new SqlParameter();
                parm1.ParameterName = "@key";
                parm1.Value = key;
                cmd.Parameters.Add(parm1);
                SqlParameter parm2 = new SqlParameter();
                parm2.ParameterName = "@value";
                parm2.Value = value;
                cmd.Parameters.Add(parm2);
                SqlParameter parm3 = new SqlParameter();
                parm3.ParameterName = "@readOnly";
                parm3.Value = readOnly;
                cmd.Parameters.Add(parm3);
                //parm.ParameterName = "@userId";
                //parm.Value = userId;
                //cmd.Parameters.Add(parm);
                returnValue = Convert.ToString(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
                throw;
            }
            finally
            {
                if (dbConnection != null)
                {
                    dbConnection.Close();
                }
            }
        }

        private static void UpdateValue(string key, string value, bool readOnly)
        {
            SqlConnection dbConnection = new SqlConnection(); ;
            string returnValue = string.Empty;

            try
            {
                // get the connection
                dbConnection = GetConnection();

                // set up the command, parameters and execute to get setting
                SqlCommand cmd = new SqlCommand("sp_AppSettingsUpdateValue", dbConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter parm = new SqlParameter();
                parm.ParameterName = "@key";
                parm.Value = key;
                cmd.Parameters.Add(parm);
                parm.ParameterName = "@value";
                parm.Value = value;
                cmd.Parameters.Add(parm);
                parm.ParameterName = "@readOnly";
                parm.Value = readOnly;
                cmd.Parameters.Add(parm);
                //parm.ParameterName = "@userId";
                //parm.Value = userId;
                //cmd.Parameters.Add(parm);
                returnValue = Convert.ToString(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
                throw;
            }
            finally
            {
                if (dbConnection != null)
                {
                    dbConnection.Close();
                }
            }
        }

        private static string ExportValues(string authenticationToken)
        {
            SqlConnection dbConnection = new SqlConnection();
            string dbConnectionString = string.Empty;
            string returnValue = string.Empty;

            try
            {

                // TODO: jfetters - change to real value
                // if the authentication token is correct
                if (authenticationToken == "thankyou")
                {

                    // get the connection
                    dbConnection = GetConnection();

                    // set up the command, parameters and execute to get setting
                    SqlCommand cmd = new SqlCommand("SELECT * FROM AppSettings", dbConnection);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "AppSettings");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // return xml stream write
                        returnValue = ds.GetXml();
                    }
                    return returnValue;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw ex;
            }
            finally
            {
                if (dbConnection != null)
                {
                    dbConnection.Close();
                }
            }
        }

        private static SqlConnection GetConnection(string connectionStringName = "")
        {

            string dbConnectionString = string.Empty;

            // create a new connection
            SqlConnection dbConnection = new SqlConnection();

            if (connectionStringName == string.Empty)
            {
                // get the connection string from the app settings
                dbConnection.ConnectionString = GetConnectionString();
            }
            else
            {
                // get the connection string from the app settings
                dbConnection.ConnectionString = GetConnectionString(connectionStringName);
            }

            // open the connection
            dbConnection.Open();

            return dbConnection;
        }

        /// <summary>
        ///     Indiccates if SQL Logging is enabled
        /// </summary>
        /// <returns></returns>
        public static Boolean GetSQLLogging()
        {
            try
            {
                // return Resource1.mainDB; 

                // Configuration appConfig = ConfigurationManager.OpenExeConfiguration("AppSettings.dll");
                //string connString = appConfig.ConnectionStrings.ConnectionStrings["mainDB"].ConnectionString;
                //return connString;

                Boolean sqlLogging = false;

                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().CodeBase) + @"\";
                Uri uri = new Uri(path);
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = Path.Combine(uri.LocalPath, "AppSettings.dll.config");
                Configuration appConfig = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);


                if (appConfig.AppSettings.Settings["SQLLogging"] != null)

                {
                    //string s = appConfig.AppSettings.Settings["SQLLogging"].Value ;
                    if (appConfig.AppSettings.Settings["SQLLogging"].Value == "On")
                    {
                        sqlLogging = true;
                    }
                }


                return sqlLogging;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #endregion

    }






}
