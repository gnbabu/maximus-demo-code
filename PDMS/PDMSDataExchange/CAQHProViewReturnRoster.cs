using FileHelpers;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    class CAQHProViewReturnRoster : BaseJob, IJob
    {
#region "Constructors"

        public CAQHProViewReturnRoster(Guid threadId)
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
            // Default Job - CAQH - ProView - Retrieve Return Roster
            this.ExecuteJob(Guid.Parse("E8C9D6AA-9F7B-4647-8FD2-C3ED28BE07BA"));
        }
        
#endregion

    }
}
