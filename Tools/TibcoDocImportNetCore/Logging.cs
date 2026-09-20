using System;
using System.Data;
using System.Data.SqlClient;

// Database objects generated for this class:
//  TABLEs:     Log
//  SPs:        sp_CreateLogEntry 
//  VIEWs:      
//  FUNCTIONs:  

namespace TibcoDocImportNetCore
{

    /// <summary>
    ///     Author(s):      jfetters
    ///     Date:           2012.03.02
    ///     Name:           Logging
    ///     Description:    This c# class provides the ability to log activity to the server against a specific thread id
    /// </summary>
    public class Logging
    {

        // private fields for parameters passed to class method
        private Guid _threadId;
        private string _message;
        private string _processName;
        private string _machine;
        private string _userName;
        private int _step = 0;  // the default step is 0 if not passed
        private Logging.LogPriority _priority = LogPriority.Information;

        /// <summary>
        /// The default parameterless constructor. Generates a new GUID for the Logging threadId
        /// </summary>
        public Logging()
        {
            //  create the thread id for the duration of the object
            this._threadId = Guid.NewGuid();
        }

        /// <summary>
        /// The parameterized constructor which provides the Logging threadId GUID.
        /// </summary>
        /// <param name="threadId">The GUID which is used for tying all log entires across all classes.</param>
        public Logging(Guid threadId)
        {
            //  use an existing thread id 
            this._threadId = threadId;
        }

        /// <summary>
        /// The parameterized constructor which provides the Logging threadId GUID and the name of the process being executed.
        /// </summary>
        /// <param name="threadId">The GUID which is used for tying all log entires across all classes.</param>
        /// <param name="processName">The name of the process being executed. Usually the name correlates to the class.method name</param>
        public Logging(Guid threadId, string processName)
        {
            //  use an existing thread id and process name 
            this._threadId = threadId;
            this.ProcessName = processName;
        }

        /// <summary>
        /// The machine name, if unset will default to the Environment.MachineName
        /// </summary>
        public string MachineName
        {
            get
            {
                return this._machine;
            }
            set
            {
                this._machine = value;
            }
        }

        /// <summary>
        /// The process name, if unset will write "Unset" to the log entry
        /// </summary>
        public string ProcessName
        {
            get
            {
                return this._processName;
            }
            set
            {
                this._processName = value;
            }
        }

        /// <summary>
        /// The thread ID used for the log entry, if unset will default to a new GUID
        /// </summary>
        public Guid ThreadId
        {
            get
            {
                return this._threadId;
            }
        }

        /// <summary>
        /// The user name executing the method, if unset will default to the Environment.UserDomainName and Environment.UserName
        /// </summary>
        public string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
            }
        }

        /// <summary>
        /// The priority of the log entry, if unset will default to Information
        /// </summary>
        public enum LogPriority : long
        {
            /// <summary>
            ///     This enum should be used for entries which are utilized for tracking down issues. 
            ///     This is the most detailed level of logging and will generate a large number of log entries
            /// </summary>
            Debug = 100,

            /// <summary>
            ///     This enum is for information purposes and should be used for most log entries
            /// </summary>
            Information = 200,

            /// <summary>
            ///     This enum is used for logging error and should be used for Exceptions generated in the system
            /// </summary>
            Error = 305,

            /// <summary>
            ///     This is for important log entries that are not exceptions where 
            ///     processing can continue however someone should attend to the bad data.
            /// </summary>
            Important = 405,

            /// <summary>
            ///     This is for bad data log entries that are not exceptions but where 
            ///     data being imported could not be loaded complete and someone should attend to the bad data.
            /// </summary>
            DataLoadIssues = 505

        }


        // TODO: jfetters - 2012.03.26 - change to reference table and update log priority enum to autogenerate based on reference table
        /// <summary>
        /// Returns the string associated with the Log enumeration
        /// </summary>
        /// <param name="priorityNumber">The log entry enumeration value</param>
        /// <returns>Log enumeration as a string</returns>
        public string GetLogPriorityName(LogPriority priorityNumber)
        {
            string returnVal = string.Empty;

            switch (priorityNumber)
            {
                case Logging.LogPriority.Debug:
                    returnVal = "Debug";
                    break;

                case Logging.LogPriority.Error:
                    returnVal = "Error";
                    break;

                case Logging.LogPriority.Important:
                    returnVal = "Important";
                    break;

                case Logging.LogPriority.Information:
                    returnVal = "Information";
                    break;
            }

            return returnVal;
        }

        /// <summary>
        /// Creates a log entry based on the parameters provided
        /// </summary>
        /// <param name="message">The log entry message to save</param>
        /// <returns>A boolean indicating if the entry was written</returns>
        public bool CreateLogEntry(string message)
        {

            // set fields and call private log entry method
            this._message = message;
            return this.CreateLogEntry();
        }

        /// <summary>
        /// Creates a log entry based on the parameters provided
        /// </summary>
        /// <param name="message">The log entry message to save</param>
        /// <param name="step">The current step number being executed</param>
        /// <returns>A boolean indicating if the entry was written</returns>
        public bool CreateLogEntry(string message, int step)
        {

            // set fields and call private log entry method
            this._message = message;
            this._step = step;
            return this.CreateLogEntry();
        }

        /// <summary>
        /// Creates a log entry based on the parameters provided
        /// </summary>
        /// <param name="message">The log entry message to save</param>
        /// <param name="priority">The priority of the log message</param>
        /// <returns>A boolean indicating if the entry was written</returns>
        public bool CreateLogEntry(string message, LogPriority priority)
        {

            // set fields and call private log entry method
            this._message = message;
            this._priority = priority;
            return this.CreateLogEntry();
        }

        /// <summary>
        /// Creates a log entry based on the parameters provided
        /// </summary>
        /// <param name="message">The log entry message to save</param>
        /// <param name="priority">The priority of the log message</param>
        /// <param name="step">The current step number being executed</param>
        /// <returns>A boolean indicating if the entry was written</returns>
        public bool CreateLogEntry(string message, LogPriority priority, int step)
        {

            // set fields and call private log entry method
            this._message = message;
            this._priority = priority;
            this._step = step;
            return this.CreateLogEntry();
        }

        /// <summary>
        /// Creates a log entry based on the class attributes
        /// </summary>
        /// <returns>A boolean indicating if the entry was written</returns>
        private bool CreateLogEntry()
        {

            // initialize low risk variables and objects for method
            SqlConnection dbConnection = new SqlConnection();
            String connString = string.Empty;
            int recsRecorded;

            try
            {

                // get the logging app setting
                string loggingEnabled = AppSettings.Get("LoggingEnabled", bool.TrueString.ToLower()).ToLower();

                // if logging enabled
                if (loggingEnabled == bool.TrueString.ToLower())
                {

                    // if machine is not set
                    if (this.MachineName == null)
                    {
                        this.MachineName = Environment.MachineName;
                    }

                    // if the username is not set
                    if (this.UserName == null)
                    {
                        this.UserName = Environment.UserDomainName + @"\" + Environment.UserName;
                    }

                    // if the process name is not set
                    if (this.ProcessName == null)
                    {

                        this.ProcessName = "Unset";
                    }

                    // initialize the settings
                    connString = AppSettings.GetConnectionString();

                    // create and open a connection object
                    dbConnection.ConnectionString = connString;
                    dbConnection.Open();

                    // create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("sp_CreateLogEntry", dbConnection);

                    // set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // add log entry command parameters
                    cmd.Parameters.Add(new SqlParameter("@ThreadId", this.ThreadId));
                    cmd.Parameters.Add(new SqlParameter("@Message", this._message));
                    cmd.Parameters.Add(new SqlParameter("@ProcessName", this.ProcessName));
                    cmd.Parameters.Add(new SqlParameter("@Machine", this.MachineName));
                    cmd.Parameters.Add(new SqlParameter("@User", this.UserName));
                    cmd.Parameters.Add(new SqlParameter("@StepNumber", this._step));
                    cmd.Parameters.Add(new SqlParameter("@Priority", this._priority));

                    // execute the stored procedure
                    recsRecorded = cmd.ExecuteNonQuery();

                    // if a record was not added
                    if (recsRecorded != 1)
                    {
                        return false;
                    }
                    else
                    {
                        // reset to default status
                        _priority = LogPriority.Information;
                        return true;
                    }
                }
                else // logging disabled
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
                return false;
            }
            finally
            {
                if (dbConnection != null)
                {
                    dbConnection.Close();
                }
            }
        }

        /// <summary>
        /// Returns dataset of log entries for the Thread Id
        /// </summary>
        /// <returns>dataset of log entries</returns>
        public DataSet GetLogEntries()
        {
            // initialize low risk variables and objects for method
            SqlConnection dbConnection = new SqlConnection();
            String connString = string.Empty;
            DataSet returnData = new DataSet();

            try
            {
                // get the logging app setting
                string loggingEnabled = AppSettings.Get("LoggingEnabled", bool.TrueString.ToLower()).ToLower();

                // if logging enabled
                if (loggingEnabled == bool.TrueString.ToLower())
                {

                    // initialize the settings
                    connString = AppSettings.GetConnectionString();

                    // create and open a connection object
                    dbConnection.ConnectionString = connString;
                    dbConnection.Open();

                    // create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("sp_RetrieveLogEntries", dbConnection);

                    // set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // add log entry command parameters
                    cmd.Parameters.Add(new SqlParameter("@ThreadId", this.ThreadId));

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(returnData);

                }
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
            finally
            {
                if (dbConnection != null)
                {
                    dbConnection.Close();
                }
            }
            return returnData;
        }
    }
}
