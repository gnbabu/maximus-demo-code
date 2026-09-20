using FileHelpers;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    class CAQHProViewRosterException : BaseJob, IJob
    {
#region "Constructors"

        public CAQHProViewRosterException(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

#endregion
#region "Logging Objects"

        private int logCnt = 0;

 #endregion

#region "Public Methods"

        override public void ExecuteJob()
        {
            // Default Job - CAQH - ProView - Retrieve Exception Roster ProView
            this.ExecuteJob(Guid.Parse("A140000D-4ECC-4341-BA9F-77C4011198AC"));
        }

        public void LogFileRecordError(Logging log, string lineNumber, string fileName, string errorMessage)
        {
            string fullMessage = "Record failed. Line No: {0}; File: {1}; Reason: {2}";
            log.CreateLogEntry(String.Format(fullMessage, lineNumber, fileName, errorMessage), Logging.LogPriority.DataLoadIssues, +logCnt);
        }

#endregion

    }
}
