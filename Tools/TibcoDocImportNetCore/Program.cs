using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace TibcoDocImportNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
       
            var builder = new HostBuilder();
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            // get worker id
            string WorkerId = "";
            int WrkId = 0;
            Worker w;

            if (args.Length > 0)
            {
                WorkerId = args[0];
                WrkId = int.Parse(WorkerId);
            }
            else
            {
                WorkerId = "1";
                WrkId = 1;
            }

            // Console.WriteLine("Starting Worker: " + WorkerId);

            Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Worker" + WorkerId + "log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            //w = new Worker(Log.Logger, WrkId);

            return Host.CreateDefaultBuilder(args)                              
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddSerilog();
                })
                .UseWindowsService().ConfigureServices((hostContext, services) =>
                {                   
                    // services.AddHostedService<Worker>();
                    services.AddHostedService
                        (serviceProvider =>
                            new Worker(
                                serviceProvider.GetService<ILogger<Worker>>(),
                                WrkId));
                });
        }
    }
}
