using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MAXIMUS.Core.Libraries.QuartzJobs
{
    /// <summary>
    ///     Author(s):      Ahackl
    ///     Date:           2017.06.12
    ///     Name:           QuartzJob
    ///     Description:    Interface Job
    /// </summary>
    [DisallowConcurrentExecution]
    public class InterfaceJob : QuartzJob
    {
        private int logCnt = 0;

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Job Type Id
        /// </summary>
        public override int JobTypesID
        {
            get { return 1; }
        }


        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Main Execution Method
        /// </summary>
        public override void Execute(Quartz.IJobExecutionContext context)
        {
            string assemblyName = Methods.GetStringValue(context.JobDetail.JobDataMap.Get("AssemblyName"));
            string className = Methods.GetStringValue(context.JobDetail.JobDataMap.Get("ClassName"));
            string assemblyPath = Methods.GetStringValue(context.JobDetail.JobDataMap.Get("AssemblyPath"));
            string id = Methods.GetStringValue(context.JobDetail.JobDataMap.Get("JobID"));

            Guid jobId;
            Guid.TryParse(id, out jobId);

            Guid loggingId = Guid.NewGuid();
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(loggingId, logMsg);

            log.CreateLogEntry(String.Format("Executing JobId {0}", jobId.ToString()), +logCnt);

            if (assemblyPath.Length > 0)
            {
                assemblyPath = new DirectoryInfo(assemblyPath).FullName;
            }
            else
            {
                assemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            }
            if (System.Diagnostics.Debugger.IsAttached)
            {
                assemblyPath = @"C:\Projects\DC_PDMS\trunk\PDMS\PDMSDataExchange\bin\Debug\";
            }
            string assemblyFullName = Path.Combine(new Uri(assemblyPath).LocalPath, assemblyName);

            Assembly jobAssembly = Assembly.LoadFrom(assemblyFullName);
            Type classType = jobAssembly.GetType(className);

            if (classType == null)
            {
                throw new ArgumentException(string.Format("Class '{0}' does not exist in Assembly '{1}'", className, assemblyFullName));
            }

            try
            {
                Guid testLoggingId = Guid.NewGuid();
                Logging logTest = new Logging(testLoggingId);
                logTest.CreateLogEntry(String.Format("Test logging for className: " + className), +logCnt);

                //Since this is a quartz job that is multithreaded it's thread is a new guid
                var jobClass = (IJob)Activator.CreateInstance(classType, loggingId.ToString());

                ((IJob)jobClass).ExecuteJob(jobId);

                log.CreateLogEntry(String.Format("JobId {0} completed", jobId.ToString()), +logCnt);
            }
            catch (Exception ex)
            {
                Guid exLoggingId = Guid.NewGuid();
                Logging logEx = new Logging(exLoggingId, logMsg);
                logEx.CreateLogEntry(String.Format("Issue Executing Job" + className), +logCnt);
                throw new Exception(String.Format("Issue Executing Job" + className), ex);
            }
        }
    }
}
