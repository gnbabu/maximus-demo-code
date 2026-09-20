using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml.Schema;
using Corp.Core.Libraries;
using System.Xml;

namespace MAXIMUS.DataExchange.PDMS
{
    public class RemittanceAdviceZip : BaseJob, IJob
    {
        public RemittanceAdviceZip(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private const string thisGuidString = "1F3E1FB4-9F7A-4BB6-B0FE-A6E456F662BF";
        private Guid thisGuid = new Guid(thisGuidString);

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(thisGuid);
        }

        override public void ExecuteJob(Guid jobId)
        {
            ZipReadmittanceReturns(jobId);
        }

        private void ZipReadmittanceReturns(Guid jobId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry(String.Format("Starting ZIP Remittance Advice Task: {0}", jobId.ToString()), Logging.LogPriority.Information);

            DirectoryInfo localDirectory;
            string localPath = AppSettings.Get("RALocalPath");

            string localPathOutput = AppSettings.Get("RALocalPathOutput"); // AppSettings.Get("RALocalPath");
            if (!(Directory.Exists(localPathOutput)))
            {
                Directory.CreateDirectory(localPathOutput);
            }
            string localPathArchive = AppSettings.Get("RALocalArchivePath");
            if (!(Directory.Exists(localPathArchive)))
            {
                Directory.CreateDirectory(localPathArchive);
            }
            string localPathReturn = AppSettings.Get("RALocalReturnPath");
            if (!(Directory.Exists(localPathReturn)))
            {
                Directory.CreateDirectory(localPathReturn);
            }
            FileCompression fc = new FileCompression(this.ThreadId);

            localDirectory = new DirectoryInfo(localPathReturn);
            string previousMCE = "0";
            string directoryName = "";
            log.CreateLogEntry(String.Format("Evaluating Directory: {0}, which reports {1} files, {2} are XML type", localPathReturn, localDirectory.GetFiles().Length, localDirectory.GetFiles(".xml").Length), Logging.LogPriority.Information);

            foreach (FileInfo file in localDirectory.GetFiles("*.xml"))
            {
                string fileName = file.Name;
                string currentMCE = fileName.Split('.')[0];
                if ((currentMCE.Length > 10) && currentMCE.Contains("-"))
                {
                    currentMCE = fileName.Split('-')[0];
                }

                if (currentMCE != previousMCE)
                {
                    previousMCE = currentMCE;
                    directoryName = String.Format("{0}{1}{2}.{3}.RA.Errors", localPathReturn, Path.DirectorySeparatorChar, currentMCE, DateTime.Now.ToString("yyyyMMdd"));
                    if (!(Directory.Exists(directoryName)))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                }
                log.CreateLogEntry(String.Format("Moving Return File {0} to {1}", file.FullName, directoryName), Logging.LogPriority.Information);
                File.Copy(file.FullName, Path.Combine(directoryName, file.Name.Replace(".xml", ".Errors.xml").Replace(".Errors.Errors", ".Errors")), true);
                File.Delete(file.FullName);
            }
            log.CreateLogEntry(String.Format("Zipping Directories"), Logging.LogPriority.Information);
            // create ZIP files for delivery
            foreach (DirectoryInfo directory in localDirectory.GetDirectories())
            {
                if (directory.FullName.Contains(".RA."))
                {
                    log.CreateLogEntry(String.Format("attempting to Zip / Delete {0}", directory.FullName), Logging.LogPriority.Information);
                    fc.ZipDirectory(directory.FullName, String.Format("{0}.zip", directory.FullName));
                    log.CreateLogEntry(String.Format("attempting to delete directory {0}", directory.FullName), Logging.LogPriority.Information);
                    directory.Delete(true);
                }
            }
        }
    }
}
