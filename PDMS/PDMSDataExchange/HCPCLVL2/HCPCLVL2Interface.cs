using ClosedXML.Excel;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.NUBC;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS.HCPCLVL2
{
    public class HCPCLVL2Interface : BaseJob, IJob
    {
        private PDMSService.PDMSServiceClient _svc;

        #region "Constructors"

        public HCPCLVL2Interface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        #endregion

        #region "Logging Objects"

        private int logCnt = 0;
        private Logging log = null;

        #endregion

        #region "Public Methods"

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(Guid.Parse("EC5BF352-9B74-4081-B18A-BFABDFBDDE5F"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            // This will be an excel file with 15 worksheets
            LoadExcelData(jobId);
        }

        #endregion

        #region "Private Methods"
        private void LoadExcelData(Guid jobId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry("Start import of HCPCs Lvl2 excel data");
           
            string directoryPath = AppSettings.Get("HCPCLvl2DataFolder");
            string ToProcessPath = directoryPath + @"\ToProcess";
            string NotProcessPath = directoryPath + @"\NotProcessed";
            string ProcessedPath = directoryPath + @"\Processed";
            string LogPath = directoryPath + @"\Logs";
            string fullFileName = string.Empty;
            string fullFileNameProcessed = string.Empty;

            HCPCLVL2Helper.WriteLog("Start import of HCPCs Lvl2 excel data");
            StringBuilder strSuccess = new StringBuilder();
            StringBuilder strError = new StringBuilder();
            try
            {
                // Get all .xlsx files in the directory

                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }

                if (!System.IO.Directory.Exists(ToProcessPath))
                {
                    System.IO.Directory.CreateDirectory(ToProcessPath);
                }

                if (!System.IO.Directory.Exists(NotProcessPath))
                {
                    System.IO.Directory.CreateDirectory(NotProcessPath);
                }

                if (!System.IO.Directory.Exists(ProcessedPath))
                {
                    System.IO.Directory.CreateDirectory(ProcessedPath);
                }

                if (!System.IO.Directory.Exists(LogPath))
                {
                    System.IO.Directory.CreateDirectory(LogPath);
                }

                string FileName = "HCPC";
                DirectoryInfo direct = new DirectoryInfo(ToProcessPath);
                FileInfo[] files = direct.GetFiles();
                Array.Sort(files, (x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.CreationTime, y.CreationTime));
                files = Array.FindAll(files, a => a.FullName.Contains(FileName) == true);
                foreach (var file in files)
                {
                    fullFileName = ToProcessPath + @"\" + file.Name;
                    fullFileNameProcessed = ProcessedPath + @"\" + file.Name;

                    DataTable dt = HCPCLVL2Helper.GetDataTableFromExcel(fullFileName, log, ThreadId);
                    if (dt != null)
                    {
                        System.Data.DataColumn fileDetailsColumn = new System.Data.DataColumn("LOAD_DATE", typeof(System.DateTime));
                        dt.Rows.Remove(dt.Rows[0]);

                        fileDetailsColumn.DefaultValue = DateTime.Now;
                        dt.Columns.Add(fileDetailsColumn);

                        if (HCPCLVL2Helper.BulkLoadToStg(log, ThreadId, dt))
                        {
                            FileInfo fullFileNameFI = new FileInfo(fullFileName);
                            DirectoryInfo processedDir = new DirectoryInfo(ProcessedPath);
                            string pPath = Path.Combine(processedDir.FullName, Path.GetFileName(file.FullName));

                            if (fullFileNameFI.Exists)
                            {
                                if (File.Exists(fullFileNameProcessed))
                                {
                                    File.Delete(fullFileNameProcessed);
                                }
                                fullFileNameFI.MoveTo(pPath);
                                HCPCLVL2Helper.WriteLog("Processed without Error and file moved to Processed folder!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HCPCLVL2Helper.WriteLog($"An error occurred: {ex.ToString()}");
            }
        }

        #endregion
    }
}
