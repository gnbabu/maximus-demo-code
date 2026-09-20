using Corp.Core.Libraries;
using Corp.Core.Libraries.Helper;
using DocumentFormat.OpenXml.Spreadsheet;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.PeerToPeer;
using System.Reflection;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS
{
    public class GenerateProviderAgreement : BaseJob, IJob
    {
        private PDMSService.PDMSServiceClient _svc;
        private PDMSService.PDMSServiceClient svc
        {
            get
            {
                if (_svc == null)
                {
                    _svc = new PDMSService.PDMSServiceClient();
                }

                return _svc;
            }
        }
        public GenerateProviderAgreement(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private Logging log = null;

        private readonly string appID = "213D5B7F-EC52-4460-B0E3-5717DC6EC24C";

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse("213D5B7F-EC52-4460-B0E3-5717DC6EC24C"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            if (jobGuid == "213D5B7F-EC52-4460-B0E3-5717DC6EC24C")
            {
                ProcessAgreements();
            }
        }

        public void ProcessAgreements()
        {
            // Create log object
            string logMsg = String.Format(CON.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(appID), logMsg);
            string Env = AppSettings.Get("Environment");

            try
            {
                log.CreateLogEntry("Start GenerateAgreementPDFService in Environment - " + Env);

                // Get all providers to generate agreeements for
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectRegToGenerateAgreement");
                if (ObjectControllerHelper.HasRows(dsReg))
                {
                    foreach (DataRow row in dsReg.Tables[0].Rows)
                    {
                        int regID = ObjectControllerHelper.GetInt("REG_ID", row);
                        string userID = ObjectControllerHelper.GetString("PROV_ADMIN_USER_ID", row);
                        int canProcess = ObjectControllerHelper.GetInt("CAN_PROCESS", row);
                        int stgPDFid = ObjectControllerHelper.GetInt("STG_PDF_ID", row);
                        int versionID = ObjectControllerHelper.GetInt("VERSION_ID", row);

                        if (canProcess == 1)
                        {
                            log.CreateLogEntry("Start for Reg ID - " + regID);
                            string fileName = string.Empty;
                            MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments doc = new MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments(new Guid(appID));
                            if (versionID == 0)
                            {
                                //ProcessDocumentController.GenerateApplication(regID, userID, true, out fileName);
                                
                                doc.GenerateApplication(regID, userID, true, out fileName, versionID);
                            }
                            else
                            {
                                //generate from version tables
                                doc.GenerateApplication(regID, userID, true, out fileName,versionID);
                            }

                            int docId = GetDocumentIDByFileName(log, fileName);
                            if (docId > 0)
                            {
                                try
                                {
                                    int onbaseDocID = 0;
                                    onbaseDocID = GetOnBaseDocumentID(log, docId);
                                    
                                    if (onbaseDocID == 0)
                                    {
                                        log.CreateLogEntry(String.Format("Start Onbase upload for Doc ID: {0} and file Name - {1}", docId, fileName), Logging.LogPriority.Error);

                                        string storagePath = AppSettings.Get("RegistrationApplication_Path_PDF", string.Empty);
                                        string pdfDirectory = string.Concat(storagePath, "Temporary_Files\\");
#if DEBUG
                                        pdfDirectory = @"C:\temp\OHPNM\Temporary_Files\";
#endif
                                        byte[] fileBytes = File.ReadAllBytes(pdfDirectory + fileName);

                                        OnBaseInterface onBaseInterface = new OnBaseInterface();
                                        onbaseDocID = onBaseInterface.GetSubmitedFileID(regID, docId, fileBytes, fileName, true);

                                        log.CreateLogEntry(String.Format("Onbase Doc ID: {0} for file Name - {1}", onbaseDocID, fileName), Logging.LogPriority.Error);

                                    }
                                    UpdateAgreementProcessedStatus(stgPDFid, docId, onbaseDocID);
                                    log.CreateLogEntry(String.Format("GenerateProviderAgreementJob Update Processed status complete on reg ID: {0} for file Name - {1}", regID, fileName), Logging.LogPriority.Error);

                                }
                                catch (Exception ex)
                                {
                                    log.CreateLogEntry(String.Format("GenerateProviderAgreementJob Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                                    throw CoreException.ThrowException(ThreadId, ex);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("GenerateProviderAgreementJob failed. Reason: " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);              
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            }

        }

        public void UpdateAgreementProcessedStatus(int stgPDFid, int docId, int onbaseDocID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("STG_PDF_ID", DbType.Int32, stgPDFid, true));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docId, true));
                parameters.Add(SqlParms.CreateParameter("ONBASE_DOCUMENT_ID", DbType.Int32, onbaseDocID, true));
                parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, appID, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateSTG_GENERATE_PROVIDER_AGREEMENT_PDF", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("GenerateProviderAgreementJob UpdateAgreementProcessedStatus Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(ex);
            }

        }
        public static int GetDocumentIDByFileName(Logging log, string fileName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_NAME", DbType.String, fileName, true));

                string newIdString = DataAccess.ExecuteScalar("usp_GetDocumentIDByFileName", parameters);
                int newId = Int32.Parse(newIdString);

                return newId;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("GenerateProviderAgreementJob GetDocumentIDByFileName Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(ex);
            }
        }

        public static int GetOnBaseDocumentID(Logging log, int docID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docID, true));

                string newIdString = DataAccess.ExecuteScalar("usp_GetOnBaseDocumentId", parameters);
                int newId = Int32.Parse(newIdString);

                return newId;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("GenerateProviderAgreementJob GetOnBaseDocumentID Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
