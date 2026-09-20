using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MAXIMUS.Core.Libraries.QuartzLibrary
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

        public void Execute()
        {
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
            // TODO: Implement
            //Logging log = new Logging();
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

            //Since this is a quartz job that is multithreaded it's thread is a new guid
            var jobClass = (IJob)Activator.CreateInstance(classType, Guid.NewGuid());

            Guid id = Guid.Parse((string)context.JobDetail.JobDataMap.Get("JobID"));

            
            // ((IJob)jobClass).ExecuteJob(id);

        }
    }
}
