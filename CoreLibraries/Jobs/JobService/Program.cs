using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace MAXIMUS.Core.Services
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
// #if (!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new JobService() 
            };
            ServiceBase.Run(ServicesToRun);
//#else
//            // Debug code: this allows the process to run as a non-service.
//            // It will kick off the service start point, but never kill it.
//            // Shut down the debugger to exit
//            JobService service = new JobService();
//            service.TestStart();
//#endif 
        } 
    }
}

