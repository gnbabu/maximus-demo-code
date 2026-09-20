using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MAXIMUS.Core.Libraries.QuartzLibrary
{

    /// <summary>
    ///     Author(s):      Ahackl
    ///     Date:           2017.06.12
    ///     Name:           QuartzJob
    ///     Description:    Select Procedure Job 
    /// </summary>
    [DisallowConcurrentExecution]
    public class SelectProcJob : QuartzJob
    {

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Job Type Id
        /// </summary>
        public override int JobTypesID
        {
            get { return 4; }
        }

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Main Execution Method
        /// </summary>
        public override void Execute(Quartz.IJobExecutionContext context)
        {
            //string procName = (string)context.JobDetail.JobDataMap.Get("ProcedureName");
            //string recipients = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("RecipientsAppSettingsKey"));

            //// retrieve job log from database
            //DataSet procResults = ExecuteStoredProc(procName, Guid.NewGuid());

            //// convert dataset to html table
            //string procResultsText = GetDataTableAsHTML(procResults.Tables[0]);

            //// generate an email
            //SendEmail((string)context.JobDetail.JobDataMap.Get("JobID"),procResultsText, (string)context.JobDetail.JobDataMap.Get("JobName"), Guid.NewGuid()); 
        }
    }
}
