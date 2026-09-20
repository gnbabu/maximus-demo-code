using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace TibcoDocImportNetCore
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public int WrkId;

        public Worker(ILogger<Worker> logger, int WId)
        {
            this.WrkId = WId;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                IConfiguration cfg = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

                string testFile = cfg.GetSection("testFile").Value;
                string cnString = cfg.GetSection("mainDB").Value;
                string workDir = cfg.GetSection("workDir").Value;
                string killFile = Path.Combine(workDir, "kill.txt");  // base work dir without worker id, so all workers can be killed at once

                // Console.WriteLine("Test File: " + testFile);
                // Console.WriteLine("CNString: " + cnString);
                // Console.WriteLine("WorkDir: " + workDir);
                // Console.WriteLine("Kill File: " + killFile);

                // get worker id
                string WorkerId = "";
                int WrkId = 0;

                WrkId = this.WrkId;
                WorkerId = WrkId.ToString();

                // Console.WriteLine("Instance Worker: " + WorkerId);

                workDir = Path.Combine(workDir, "w" + WorkerId);

                // create directories
                string localWorkDir = Path.Combine(workDir, "onbase");
                string testDir = Path.Combine(workDir, "test");
                string problemFileDir = Path.Combine(workDir, "problems");
                System.IO.Directory.CreateDirectory(localWorkDir);
                System.IO.Directory.CreateDirectory(problemFileDir);
                System.IO.Directory.CreateDirectory(testDir);
                // if (File.Exists(killFile)) { File.Delete(killFile); }

                var sec = new SecretsManager(cfg.GetSection("TIBCOSecretsRegion").Value);
                var result = sec.GetSuperSecretPassword(cfg.GetSection("TIBCOSecretDictionary").Value);
                result.Wait();

                string RJkey = result.Result["RijndaelKey"];
                string RJiv = result.Result["RijndaelIV"];
                string TBUser = result.Result["tbUser"];
                string TBPwd = result.Result["tbPwd"];

                GetTibcoMessage tbq = new GetTibcoMessage();
                tbq.CNString = cnString;
                tbq.WorkerId = WrkId;
                tbq.WorkDir = workDir;
                tbq.RJkey = RJkey;//cfg.GetSection("RijndaelKey").Value
                tbq.RJiv = RJiv;//cfg.GetSection("RijndaelIV").Value
                tbq.UploadURL = cfg.GetSection("OnBaseFileHandlerURL_Worker" + WorkerId.PadLeft(2, '0')).Value;   // multiple handlers can be set in config for performance
                tbq.KillFile = killFile;
                tbq.ServerUrl = cfg.GetSection("serverUrl").Value;
                tbq.QueueName = cfg.GetSection("queueName").Value;
                tbq.TBUser = TBUser;//cfg.GetSection("tbUser").Value
                tbq.TBPwd = TBPwd;//cfg.GetSection("tbPwd").Value
                tbq.UseSSL = cfg.GetSection("useSSL").Value;
                tbq.TargetHost = cfg.GetSection("targetHost").Value;
                tbq.CertName = cfg.GetSection("certName").Value;

                /*
                Console.WriteLine("tbq.RJkey: " + tbq.RJkey);
                Console.WriteLine("tbq.RJiv: " + tbq.RJiv);
                Console.WriteLine("tbq.ServerUrl: " + tbq.ServerUrl);
                Console.WriteLine("tbq.TBUser: " + tbq.TBUser);
                Console.WriteLine("tbq.TargetHost: " + tbq.TargetHost);
                Console.WriteLine("tbq.CertName: " + tbq.CertName);
                Console.WriteLine("tbq.UploadURL: " + tbq.UploadURL);
                */


                if (testFile != "")  // local testing read from disk
                {
                    string msg = File.ReadAllText(testFile);
                    string Rslt = tbq.ProcessMessage(msg, testFile, true);
                }
                else
                {
                    tbq.GetNextMessage();
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }

        }
    }
}