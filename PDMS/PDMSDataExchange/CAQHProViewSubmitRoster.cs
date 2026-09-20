using FileHelpers;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    class CAQHProViewSubmitRoster : BaseJob, IJob
    {
#region "Constructors"

        public CAQHProViewSubmitRoster(Guid threadId)
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
            // Default Job - CAQH Submit Provider Roster ProView
            this.ExecuteJob(Guid.Parse("0AC4E189-A97E-42D4-A236-074A749DCD5B"));
        }
        
        #endregion
    }
}
