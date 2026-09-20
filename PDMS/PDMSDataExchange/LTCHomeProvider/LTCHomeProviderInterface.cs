using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.AutomatedReports;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS.LTCHomeProvider
{
    public class LTCHomeProviderInterface : BaseJob, IJob
    {
        public LTCHomeProviderInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "D7774686-EBB7-4C5D-B873-FCF17E16A05C";

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse(appID));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();

            if (jobGuid.Equals(appID))
            {
                InitiateLTCHomeProviderProcess();
            }
        }

        public void InitiateLTCHomeProviderProcess()
        {
            string logMsg = String.Format(CON.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            try
            {
                LTCHomeProviderHelper.CreateLogEntry("InitiateLTCHomeProviderProcess", "Process Started", CON.AutomatedReports.LogPriorityInfo);
                string directoryPath = AppSettings.Get("LTCHomeProviderFolder");
                string archivePath = directoryPath + @"\Archive";
                

                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }
                if (!System.IO.Directory.Exists(archivePath))
                {
                    System.IO.Directory.CreateDirectory(archivePath);
                }

                int fileHomeID = 0;
                int fileProviderID = 0;
                int recordCount = 0;
                string fullFileName = string.Empty;
                string fileName = string.Empty;
                Guid fileUUID = Guid.NewGuid();
                string formattedDate = DateTime.Now.ToString("MMddyyyy");

                // Generate Provider Info File
                try
                {
                    recordCount = 0;
                    fileName = "LTC_ProvInfo_"+ formattedDate + ".txt";
                    fullFileName = directoryPath + "\\" + fileName;
                    fileProviderID = LTCHomeProviderHelper.InsertLTCHPFileStatus(fileName, fileUUID, CON.LTCHomeProvider.LTCHomeProviderFileStatusInProcess, CON.LTCHomeProvider.LTCHomeProviderInsert);
                    LTCHomeProviderHelper.stageLTCProviderFile(fileUUID);
                    DataTable tableProvider = LTCHomeProviderHelper.getAllLTCProvideFileDetails(fileUUID);
                    using (StreamWriter writer = new StreamWriter(fullFileName, false, System.Text.Encoding.ASCII))
                    {
                        if (tableProvider != null)
                        {
                            recordCount = tableProvider.Rows.Count;
                            foreach (DataRow rowProvider in tableProvider.Rows)
                            {
                                writer.WriteLine(String.Join("", rowProvider.ItemArray));
                            }
                        }
                    }
                    LTCHomeProviderHelper.UpdateLTCHPFileStatus(fileProviderID, recordCount, CON.LTCHomeProvider.LTCHomeProviderFileStatusSuccess, CON.LTCHomeProvider.LTCHomeProviderUpdate);
                }
                catch (Exception ex)
                {
                    LTCHomeProviderHelper.UpdateLTCHPFileStatus(fileProviderID, CON.LTCHomeProvider.LTCHomeProviderFileStatusFailed, CON.LTCHomeProvider.LTCHomeProviderUpdate);
                    LTCHomeProviderHelper.CreateLogEntry("InitiateLTCHomeProviderProcess", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                }

                // Generate Home Info File
                try
                {
                    recordCount = 0;
                    fileName = "LTC_HomeInfo_" + formattedDate + ".txt";
                    fullFileName = directoryPath + "\\" + fileName;
                    fileHomeID = LTCHomeProviderHelper.InsertLTCHPFileStatus(fileName, fileUUID, CON.LTCHomeProvider.LTCHomeProviderFileStatusInProcess, CON.LTCHomeProvider.LTCHomeProviderInsert);
                    LTCHomeProviderHelper.stageLTCHomeFile(fileUUID);
                    DataTable tableHome = LTCHomeProviderHelper.getAllLTCHomeFileDetails(fileUUID);
                    using (StreamWriter writer = new StreamWriter(fullFileName, false, System.Text.Encoding.ASCII))
                    {
                        if (tableHome != null)
                        {
                            recordCount = tableHome.Rows.Count;
                            foreach (DataRow rowHome in tableHome.Rows)
                            {
                                writer.WriteLine(String.Join("", rowHome.ItemArray));
                            }
                        }
                    }
                    LTCHomeProviderHelper.UpdateLTCHPFileStatus(fileHomeID, recordCount, CON.LTCHomeProvider.LTCHomeProviderFileStatusSuccess, CON.LTCHomeProvider.LTCHomeProviderUpdate);
                }
                catch (Exception ex)
                {
                    LTCHomeProviderHelper.UpdateLTCHPFileStatus(fileHomeID, CON.LTCHomeProvider.LTCHomeProviderFileStatusFailed, CON.LTCHomeProvider.LTCHomeProviderUpdate);
                    LTCHomeProviderHelper.CreateLogEntry("InitiateLTCHomeProviderProcess", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
                }
                LTCHomeProviderHelper.CreateLogEntry("InitiateLTCHomeProviderProcess", "Process Completed", CON.AutomatedReports.LogPriorityInfo);
            }
            catch (Exception ex)
            {
                LTCHomeProviderHelper.CreateLogEntry("InitiateLTCHomeProviderProcess", string.Format("Exception {0} {1}", ex.Message, ex.StackTrace), CON.AutomatedReports.LogPriorityError);
            }
        }
    }
}
