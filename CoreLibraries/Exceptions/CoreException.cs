using System;
using System.Reflection;

// TODO: jfetters - write code to write all exceptions to flat file if database is down and rewrite everything to database once the 
//      database is available.

namespace MAXIMUS.Core.Libraries
{
    /// <summary>
    ///     Records and retrieves information based on a the type of exception generated.
    /// </summary>
    public static class CoreException
    {

    #region "Public Variables"
        /// <summary>
        ///     A hard-coded string which allows the Exception library to determine if the exception 
        ///         has been trapped by this class previously.
        /// </summary>
        public const string ExceptionRecorded = "[[CoreException]]";
    #endregion

    #region "Private Variables"
        #endregion

    #region "Public Methods"

    /// <summary>
    ///     Formats an exception message to the standard format defined for the exception service
    /// </summary>
    /// <param name="currentException">The currently encountered exception</param>
    /// <returns>Custom formatted exception message</returns>
    public static string FormatException(Exception currentException)
    {
        Exception formattedException = ThrowException(new Guid(), currentException);
        return formattedException.Message;
    }

    /// <summary>
    ///     Determines if the exception has been logged and if not
    ///         generates a new, nearly-identical exception with the 
    ///         exception library string added to allow identifying
    ///         future re-throws of same exception.
    /// </summary>
    /// <param name="currentException">The exception that was throw in the calling assembly</param>
    /// <returns>A recorded exception</returns>
    public static Exception ThrowException(Exception currentException)
    {
        return ThrowException(new Guid(), currentException);    
    }

    /// <summary>
    ///     Determines if the exception has been logged and if not
    ///         generates a new, nearly-identical exception with the 
    ///         exception library string added to allow identifying
    ///         future re-throws of same exception.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="currentException">The exception that was throw in the calling assembly</param>
    /// <param name="logProcessName"></param>
    /// <returns>A recorded exception</returns>
    /// 
    /// 
    public static Exception ThrowException(Guid threadId, Exception currentException, string logProcessName)
    {
        Logging log = new Logging(threadId, logProcessName);

        try
        {
            Exception returnVal;

            // check to see if the exception is already logged
            if (currentException.Source == ExceptionRecorded)
            {
                // return the existing exception due to already being handled by this class
                returnVal = currentException;

            }
            else // not recorded
            {
                returnVal = new Exception();

                string message = currentException.Message;

                string logEntry = String.Format(Constants.LogString.ExceptionEncounteredDetail, currentException.Message, currentException.Source
                    , currentException.Data, currentException.StackTrace);
                log.CreateLogEntry(logEntry, Logging.LogPriority.Error);

                // if exception user friendly message exists, use it

                // else, write to exceptions table

                // currentException.StackTrace;
                // currentException.Source;
                // currentException.Data;

                // write new information to exception
                returnVal = new Exception(message, currentException);
                returnVal.Source = ExceptionRecorded;
            }
            return returnVal;
        }
        catch
        {
            // TODO - jfetters: possibly log?
            //   ignore and return new exception specific to this class
            Exception exception = new Exception("MAXIMUS.Core.Libraries.CoreException Internal Error");
            exception.Source = ExceptionRecorded;
            return exception;
        }
    }

    /// <summary>
    ///     Determines if the exception has been logged and if not
    ///         generates a new, nearly-identical exception with the 
    ///         exception library string added to allow identifying
    ///         future re-throws of same exception.
    /// </summary>
    /// <param name="threadId">The thread id of the currently running thread</param>
    /// <param name="currentException">The exception that was throw in the calling assembly</param>
    /// <returns>A recorded exception</returns>
    public static Exception ThrowException(Guid threadId, Exception currentException)
    {
        string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
            , MethodBase.GetCurrentMethod().Name);
        return ThrowException(new Guid(), currentException, logProcessName);    
    }

    #endregion

    #region "Private Methods"
    #endregion

    }
}
