using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace MAXIMUS.Core.Libraries
{
    /// <summary>
    ///     Author(s):      rnagvenkar
    ///     Date:           03:05:2020
    ///     Name:           UserActivityLogging
    ///     Description:    This c# class provides the ability to log user activity to the server against a specified userID
    /// </summary>
    public class UserActivityLogging
    {


        //private variables for values passed to class
        private string _aspFileName;
        private int _regID = 0;
        private Guid _userID;


        /// <summary>
        /// The parameterized constructor which provides the Logging userID GUID.
        /// </summary>
        /// <param name="userID">The GUID which is used for tying all log entires across all classes.</param>
        //parameterized constructor to pass the UserID
        public UserActivityLogging(Guid userID)
        {
            this._userID = userID;
        }

        /// <summary>
        /// The parameterized public boolean method which gets the aspFileName object.
        /// </summary>
        /// <param name="aspFileName">The aspFileName is used to match with the one in the Log table</param>
        /// <returns>A boolean indicating if the entry was written</returns>
        public bool AddLog(object aspFileName)
        {
            //checks to see if the object belongs to the UserController or a Page.
            if (aspFileName is UserControl)
            {
                this._aspFileName = System.IO.Path.GetFileName(((UserControl)aspFileName).AppRelativeVirtualPath).ToString();
            }
            else
            {
                this._aspFileName = System.IO.Path.GetFileName(((Page)aspFileName).AppRelativeVirtualPath).ToString();
            }
            return this.AddLog();
        }

        /// <summary>
        /// The parameterized public boolean method which gets the aspFileName object and the regID int.
        /// </summary>
        /// <param name="aspFileName">The aspFileName is used to match with the one in the Log table</param>
        /// <param name="regID">The regID gives more info regarding the user, future use</param>
        /// <returns>A boolean indicating if the entry was written</returns>
        public bool AddLog(object aspFileName, int regID)
        {
            //checks to see if the object belongs to the UserController or a Page.
            if (aspFileName is UserControl)
            {
                this._aspFileName = System.IO.Path.GetFileName(((UserControl)aspFileName).AppRelativeVirtualPath).ToString();
            }
            else
            {
                this._aspFileName = System.IO.Path.GetFileName(((Page)aspFileName).AppRelativeVirtualPath).ToString();
            }
            //default regID is set to 0
            this._regID = regID;
            return this.AddLog();
        }

        /// <summary>
        /// Creates a log entry based on the class attributes
        /// </summary>
        /// <returns>A boolean indicating if the entry was written</returns>
        private bool AddLog()
        {
            //initalize the sql objects
            SqlConnection dbConnection = new SqlConnection();
            String connectionString = string.Empty;
            int logRecorded;

            try
            {
                //Configuration conf = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
                //SessionStateSection section = (SessionStateSection)conf.GetSection("system.web/sessionState");
                //int timeout = (int)section.Timeout.TotalMinutes;
                //this._regID = timeout;
                // get the logging app setting
                string userActivityLogEnabled = AppSettings.Get("UserActivityLoggingEnabled", bool.TrueString.ToLower()).ToLower();

                // if logging enabled //set it to TrueString when commit
                if (userActivityLogEnabled == bool.TrueString.ToLower())
                {
                    // initialize the settings
                    connectionString = AppSettings.GetConnectionString();

                    // create and open a connection object
                    dbConnection.ConnectionString = connectionString;
                    dbConnection.Open();

                    // create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("insertUserActivityLog", dbConnection);

                    // set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // add log entry command parameters
                    cmd.Parameters.Add(new SqlParameter("@User_ID", this._userID));
                    if (this._regID != 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@REG_ID", this._regID));
                    }
                    cmd.Parameters.Add(new SqlParameter("@ASPFileName", this._aspFileName));

                    // execute the stored procedure
                    logRecorded = cmd.ExecuteNonQuery();

                    // check if the record is added 
                    if (logRecorded != 1)
                    {
                        return false;
                    }
                    else
                    {
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

                //Note: add the exception to the Logging Project log.CreateLogEntry(msg)

                return false;
            }
            finally
            {
                if (dbConnection != null)
                {
                    //close the sql connection
                    dbConnection.Close();
                }
            }
        }
    }
}
