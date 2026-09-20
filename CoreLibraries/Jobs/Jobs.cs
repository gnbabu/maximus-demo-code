using System;
using System.Collections;
using System.Reflection;

namespace MAXIMUS.Core.Libraries
{
    class Scheduler
    {
        private const string parmJobId = "JobId";

        static void Main(string[] args)
        {

            // generate a new thread id GUID
            int logCnt = 0;
            Guid ThreadId = Guid.NewGuid();
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(ThreadId, logMsg);

            try
            {
                //Uncomment the next line to debug while the JobService is running.
                //System.Diagnostics.Debugger.Break();
                Hashtable mapArguments = new Hashtable();
                if (args.Length <= 1)
                {

                    log.CreateLogEntry("No parameters provided", +logCnt);
                    
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("WARNING: No parameters given!");
                    Console.Out.WriteLine("SYNTAX: Jobs.exe /JobId {Guid}");
                    Console.Out.WriteLine("Note: The Guid should not be enclosed in curly brackets but can be lower or upper case");
                    Console.Out.WriteLine("Example: Scheduler.exe /" + parmJobId + " 52AE6538-49BC-4651-9500-8AAB64A3BFF5");
                    Console.Out.WriteLine(Environment.UserDomainName + @"\" + Environment.UserName);
                    Console.Out.WriteLine();
                }
                else
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        String sName = args[i];
                        if (sName.Substring(0, 1) == "/" && (i + 1) < args.Length)
                            mapArguments[sName.Substring(1, sName.Length - 1)] = args[++i];
                    }

                    if (!mapArguments.ContainsKey("JobId"))
                    {
                        Console.Out.WriteLine("Missing argument " + parmJobId);
                        System.Environment.Exit(1);
                    }
                    else
                    {
                        Guid tempGuid;
                        string jobId = Convert.ToString(mapArguments[parmJobId]);
                        // sanitize the arguments - SonarQube high issue
                        if (Guid.TryParse(jobId, out tempGuid))
                        {
                            log.CreateLogEntry(String.Format("Attempting to start job {0}", jobId), +logCnt);
                            Console.Out.WriteLine("Attempting to start job - {0}", jobId);
                            // run the job
                            Job job = new Job(ThreadId);
                            job.ExecuteJob(jobId); 
                        }
                        else
                        {
                            Console.Out.WriteLine("Invalid input - {0}", jobId);
                            System.Environment.Exit(1);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("MAXIMUS.Core.Libraries.Scheduler.Main failed. Reason: " + ex.Message);
                log.CreateLogEntry("MAXIMUS.Core.Libraries.Scheduler.Main failed. Reason: " + ex.Message, Logging.LogPriority.Error, +logCnt);
            }
        }
    }
}
