using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Corp.Core.Libraries;
using System.IO.Compression;

namespace MAXIMUS.DataExchange.PDMS.RAConvertToPDFs
{
    public class RAConvertToPDFs : BaseJob, IJob
    {


        private Logging log = null;

        public RAConvertToPDFs(Guid threadId) : base(threadId)
        {
            this.ThreadId = Guid.Parse("038FDA40-AD26-464A-ADD6-463D7507B793");
        }
        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(Guid.Parse("038FDA40-AD26-464A-ADD6-463D7507B793"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                case "038FDA40-AD26-464A-ADD6-463D7507B793":
                    this.startRATXTFileProcessing();
                    break;
            }
        }

        private void startRATXTFileProcessing()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);

            try
            {
                log.CreateLogEntry("RAConvertToPDFs Process Started");

                int remittance_advice_id = 0;
                int document_id = 0;
                string zipFileName = "";
                string pdfFileName = "";
                string GenericTemplatePath = AppSettings.Get("FileStorePath", string.Empty);
                int pdfDocumentID = 0;

                #if DEBUG
                    @GenericTemplatePath = @"C:\Temp\";
                #endif

                DataSet dsTrans = RAConvertToPDFsHelper.SelectRAZipFilesToProcess(log, this.ThreadId);
                if (ObjectControllerHelper.HasRows(dsTrans))
                {
                    log.CreateLogEntry("RAConvertToPDFs Records to process : " + dsTrans.Tables[0].Rows.Count.ToString());

                    foreach (DataRow row in dsTrans.Tables[0].Rows)
                    {
                        try
                        {
                            remittance_advice_id = ObjectControllerHelper.GetInt("remittance_advice_id", row);
                            document_id = ObjectControllerHelper.GetInt("DOCUMENT_ID", row);
                            zipFileName = ObjectControllerHelper.GetString("FileName", row) + ".zip";

                            StringBuilder builder = new StringBuilder(ObjectControllerHelper.GetString("FileName", row));
                            builder.Replace(".zip", "");
                            builder.Replace(".pdf", "");

                            pdfFileName = builder + ".pdf";

                            OnBaseInterface onBaseInterface = new OnBaseInterface();
                            byte[] decryptedFile = onBaseInterface.RetrieveFile(GenericTemplatePath, document_id, "N");

                            string decryptedFileSTR = Encoding.UTF8.GetString(decryptedFile);

                            byte[] dataB64 = Convert.FromBase64String(decryptedFileSTR);

                            File.WriteAllBytes(GenericTemplatePath + zipFileName, dataB64);

                            string startPath = GenericTemplatePath + zipFileName;
                            string extractPath = GenericTemplatePath + pdfFileName;

                            using (ZipArchive zip = ZipFile.Open(startPath, ZipArchiveMode.Read))
                                foreach (ZipArchiveEntry entry in zip.Entries)
                                {
                                    string ext = Path.GetExtension(entry.Name).ToLower();
                                    if (ext == ".pdf")
                                    {
                                        entry.ExtractToFile(extractPath, true);
                                    }
                                }

                            byte[] pdfFileBytes = File.ReadAllBytes(GenericTemplatePath + pdfFileName);

                            pdfDocumentID = RAConvertToPDFsHelper.StoreDocumentRecord(log, ThreadId, pdfFileName, pdfFileName, string.Empty);

                            onBaseInterface.SubmitFile(pdfDocumentID, pdfFileBytes, pdfFileName);

                            File.Delete(GenericTemplatePath + zipFileName);

                            File.Delete(GenericTemplatePath + pdfFileName);

                            RAConvertToPDFsHelper.UpdateRATXTFilesToProcess(log, ThreadId, remittance_advice_id, pdfDocumentID);

                        }
                        catch (Exception ex)
                        {
                            log.CreateLogEntry(String.Format("RAConvertToPDFs Exception for record {0} : {1}", remittance_advice_id.ToString(), ex.StackTrace), Logging.LogPriority.Error);
                        }
                    }
                }

                log.CreateLogEntry("RAConvertToPDFs Process Ended");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("RAConvertToPDFs Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }
    }
}
