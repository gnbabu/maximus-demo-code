using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.onBaseDocUpload
{
    internal class OnBaseDocUploadInterface339_1 : BaseJob, IJob
    {

        private int logCnt = 0;
        private Logging log = null;

        public OnBaseDocUploadInterface339_1(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "9731AA7E-E53E-45A6-A0BC-36A12D3AB6EF";

        override public void ExecuteJob()
        {
            // Default Job - Load files to Onbase sever
            this.ExecuteJob(Guid.Parse(appID));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();

            if (jobGuid.Equals(appID))
            {
                OnBaseDocUploadInitiate();
            }
        }

        public void OnBaseDocUploadInitiate()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);

            string onBaseWildcard = AppSettings.Get("OnBaseDocUpload-FileWildcardEXT"); //*.*
            string localPath = AppSettings.Get("OnBaseDocUpload-ImportLocalPath");
            string archiveFolder = AppSettings.Get("OnBaseDocUpload-ImportLocalPathArchive");
            DirectoryInfo localDirectory;
            OnBaseInterface onBaseInterface = new OnBaseInterface();

            log = new Logging(new Guid(appID), logMsg);
            log.CreateLogEntry(Constants.LogString.OnBaseDocUploadJobStart);

            try
            {

                // retrieve and set required variables - ToProcessFolder
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    localPath = @"D:\Files\OnbaseTest\";
                }

                // create local path if it does not exist
                Directory.CreateDirectory(localPath);
                localDirectory = new DirectoryInfo(localPath);

                // create archive path if it does not exist
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }

                // create archive path if it does not exist
                if (!Directory.Exists(archiveFolder))
                {
                    Directory.CreateDirectory(archiveFolder);
                }
                int i = 0;
                string startIndexStr = AppSettings.Get("OnbaseJobIdIndex-" + appID);
                int startIndex = !string.IsNullOrEmpty(startIndexStr) ? Convert.ToInt32(startIndexStr) : 0;

                //Reset any Pending status files to be processed again
                OnBaseDocUploadHelper.DeleteOnbaseUploadProcessById(log, this.ThreadId, 0);

                foreach (FileInfo file in localDirectory.GetFiles(onBaseWildcard).OrderBy(f => f.LastWriteTime))
                {
                    try
                    {
                        if (i >= startIndex)
                        {
                            //Check if the file is being processed? If not, then insert record and mark it as pending process
                            var onbaseProcessRec = OnBaseDocUploadHelper.GetOnbaseUploadProcessByFileName(log, this.ThreadId, file.Name);
                            if (onbaseProcessRec != null && onbaseProcessRec.Tables[0].Rows.Count == 0)
                            {
                                //Pending process record
                                string processStatus = "P";
                                var returnId = OnBaseDocUploadHelper.InsertOnbaseUploadProcess(log, new Guid(appID), file.Name, processStatus);

                                try
                                {
                                    int docId = 0;
                                    int onbaseId = 0;
                                    bool result = false;
                                    bool hasDocId = false;
                                    if (file.Name.IndexOf('_') != -1)
                                    {
                                        result = int.TryParse(file.Name.Substring(0, file.Name.IndexOf('_')), out docId);
                                        hasDocId = result;
                                    }
                                    if (docId == 0)
                                        docId = OnBaseDocUploadHelper.GetDocumentIDByFileName(log, this.ThreadId, file);

                                    //Before upload make sure the file is still being processed(not complete by other job processes)
                                    var onbaseProcessRec1 = OnBaseDocUploadHelper.GetOnbaseUploadProcessByFileName(log, this.ThreadId, file.Name);
                                    if (docId > 0 && onbaseProcessRec1 != null && onbaseProcessRec1.Tables[0].Rows.Count == 1 && onbaseProcessRec1.Tables[0].Rows[0]["PROCESS_STATUS_CD"].ToString() == "P")
                                    {
                                        try
                                        {
                                            //Check if OnbaseId exist for the doc then just archive the file else load to onbase and then archive.
                                            onbaseId = OnBaseDocUploadHelper.GetOnbaseIDByDocId(log, this.ThreadId, docId);
                                            if (onbaseId == 0)
                                            {
                                                byte[] fileBytes = File.ReadAllBytes(localPath + file.Name);
                                                if (hasDocId)
                                                {
                                                    int onBaseID = onBaseInterface.GetSubmitedFileID(docId, fileBytes, file.Name.Substring(file.Name.IndexOf('_') + 1));

                                                    OnBaseDocUploadHelper.UpdateOnBaseByDocumentId(log, new Guid(appID), docId, onBaseID);

                                                    OnBaseDocUploadHelper.OnBaseDocUploadTracking(log, new Guid(appID), file.Name.Substring(file.Name.IndexOf('_') + 1), docId, onBaseID);
                                                }
                                                else
                                                {
                                                    int onBaseID = onBaseInterface.GetSubmitedFileID(docId, fileBytes, file.Name);

                                                    OnBaseDocUploadHelper.UpdateOnBaseByDocumentId(log, new Guid(appID), docId, onBaseID);

                                                    OnBaseDocUploadHelper.OnBaseDocUploadTracking(log, new Guid(appID), file.Name, docId, onBaseID);
                                                }

                                                file.MoveTo(archiveFolder + @"\" + file.Name);
                                            }
                                            else
                                            {
                                                file.MoveTo(archiveFolder + @"\" + file.Name);
                                            }


                                            //After process, mark it as Complete
                                            processStatus = "C";
                                            OnBaseDocUploadHelper.UpdateOnbaseUploadProcess(log, new Guid(appID), Convert.ToInt32(returnId), processStatus);
                                        }
                                        catch
                                        {
                                            //Delete the record in the Process table that is in 'P - Process' status if the onbase upload fails/or archive fails if onbase is down
                                            if (!string.IsNullOrEmpty(returnId) && Convert.ToInt32(returnId) > 0)
                                                OnBaseDocUploadHelper.DeleteOnbaseUploadProcessById(log, this.ThreadId, Convert.ToInt32(returnId));
                                        }
                                    }
                                }
                                catch
                                {
                                    //Delete the record in the Process table that is in 'P - Process' status if the onbase upload fails/or archive fails if onbase is down
                                    if (!string.IsNullOrEmpty(returnId) && Convert.ToInt32(returnId) > 0)
                                        OnBaseDocUploadHelper.DeleteOnbaseUploadProcessById(log, this.ThreadId, Convert.ToInt32(returnId));
                                }
                            }
                        }
                        i++;
                    }
                    catch (Exception ex)
                    {
                        log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.OnBaseDocUploadJobEnd);
        }
    }
}