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
    ///     Description:    Select SQL Job 
    /// </summary>
    [DisallowConcurrentExecution]
    public class SelectSQLJob : QuartzJob
    {
        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Job Type Id
        /// </summary>
        public override int JobTypesID
        {
            get { return 3; }
        }


        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Main Execution Method
        /// </summary>
        public override void Execute(Quartz.IJobExecutionContext context)
        {
            // TODO: Implement
            //string sql = (string)context.JobDetail.JobDataMap.Get("SQL");
            //string recipients = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("RecipientsAppSettingsKey"));

            //// retrieve job log from database
            //DataSet sqlResults = ExecuteSQL(sql, Guid.NewGuid());

            //// convert dataset to html table
            //string sqlTextResults = GetDataTableAsHTML(sqlResults.Tables[0]);

            //// generate an email
            //SendEmail((string)context.JobDetail.JobDataMap.Get("JobID"), sqlTextResults, (string)context.JobDetail.JobDataMap.Get("JobName"), Guid.NewGuid());
        }
    }
}
