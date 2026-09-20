using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MAXIMUS.Core.Libraries.QuartzJobs
{

    /// <summary>
    ///     Author(s):      Ahackl
    ///     Date:           2017.06.12
    ///     Name:           QuartzJob
    ///     Description:    Save Procedure Job
    /// </summary>
    [DisallowConcurrentExecution]
    public class SaveProcJob : QuartzJob
    {
        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Job Type Id
        /// </summary>
        public override int JobTypesID
        {
            get { return 5; }
        }

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Main Execution Method
        /// </summary>
        public override void Execute(Quartz.IJobExecutionContext context)
        {
            string procName = (string)context.JobDetail.JobDataMap.Get("ProcedureName");
            string fileName = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("FileNameAppSettingsKey"));
            string wildcard = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("WildCardAppSettingsKey"));
            string delimiter = (string)context.JobDetail.JobDataMap.Get("Delimiter");
            string filePath = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("FilePathAppSettingsKey"));

            // retrieve job log from database
            DataSet procResults = ExecuteStoredProc(procName, Guid.NewGuid());

            // load results into delimited file
            string fileContents = GetDataTableAsDelimited(procResults.Tables[0], delimiter);

            // write the file
            fileName = fileName.Replace(wildcard, DateTime.Now.ToString(wildcard));
            string fullName = filePath + fileName;
            Methods.WriteStringToFile(filePath, fileName, fileContents, false);
        }
    }
}
