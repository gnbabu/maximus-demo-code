using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GenerateAgreementPDFService
{
    class GenerateAgreementInterface
    {
        static void Main()
        {
            int logCnt = 0;
            // generate a new thread id GUID
            Guid ThreadId = Guid.NewGuid();
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(ThreadId, logMsg);
            string Env = AppSettings.Get("Environment");

            string appID = "E033D359-BF94-4148-8924-5E0DBFEB4D99";
            try
            {
                Console.Out.WriteLine("Start GenerateAgreementPDFService");
                log.CreateLogEntry("Start GenerateAgreementPDFService in Environment - " + Env, +logCnt);

                DataSet dsReg = GenAgreementHelper.SelectRegToGenerateAgreement();
                if (ObjectControllerHelper.HasRows(dsReg))
                {
                    foreach (DataRow row in dsReg.Tables[0].Rows)
                    {
                        int regID = ObjectControllerHelper.GetInt("REG_ID", row);
                        string userID = ObjectControllerHelper.GetString("PROV_ADMIN_USER_ID", row);
                        int canProcess = 1;
                        int stgPDFid = ObjectControllerHelper.GetInt("STG_PDF_ID", row);
                        int versionID = ObjectControllerHelper.GetInt("VERSION_ID", row);

                        if (canProcess == 1)
                        {
                            Console.Out.WriteLine("Start for Reg ID - " + regID);
                            log.CreateLogEntry("Start for Reg ID - " + regID, +logCnt);

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
                                doc.GenerateApplication(regID, userID, true, out fileName, versionID);
                            }
                            Console.Out.WriteLine("File name - " + fileName);                            

                            int docId = GenAgreementHelper.GetDocumentIDbyFileName(fileName);                            
                            Console.Out.WriteLine("docId - " + docId);

                            if (docId > 0)
                            {
                                try
                                {
                                    int onbaseDocID = 0;
                                    onbaseDocID = GenAgreementHelper.GetOnBaseDocumentID(docId);
                                    Console.Out.WriteLine("OnBase Document ID - " + onbaseDocID);

                                    if (onbaseDocID == 0)
                                    {
                                        Console.Out.WriteLine("Start Onbase upload ");
                                        log.CreateLogEntry(String.Format("Start Onbase upload for Doc ID: {0} and file Name - {1}", docId, fileName), Logging.LogPriority.Error);
                                        
                                        string storagePath = AppSettings.Get("RegistrationApplication_Path_PDF", string.Empty);
                                        string pdfDirectory = string.Concat(storagePath, "Temporary_Files\\");

#if DEBUG
                                        pdfDirectory = @"C:\temp\OHPNM\Temporary_Files\";
#endif
                                        byte[] fileBytes = File.ReadAllBytes(pdfDirectory + fileName);

                                        OnBaseInterface onBaseInterface = new OnBaseInterface();
                                        onbaseDocID = onBaseInterface.GetSubmitedFileID(regID, docId, fileBytes, fileName);

                                        Console.Out.WriteLine("Onbase Doc ID - " + onbaseDocID, +logCnt);
                                        log.CreateLogEntry(String.Format("Onbase Doc ID: {0} for file Name - {1}", onbaseDocID, fileName), Logging.LogPriority.Error);

                                    }
                                    GenAgreementHelper.UpdateAgreementProcessedStatus(stgPDFid, docId, onbaseDocID, appID);
                                    log.CreateLogEntry(String.Format("Update Processed status complete on reg ID: {0} for file Name - {1}", regID, fileName), Logging.LogPriority.Error);

                                }
                                catch (Exception ex)
                                {
                                    Console.Out.WriteLine(String.Format("GenerateAgreementPDFService Exception: {0}", ex.StackTrace));
                                    log.CreateLogEntry(String.Format("GenerateAgreementPDFService Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                                    throw CoreException.ThrowException(ThreadId, ex);
                                }
                            }
                        }
                        else
                        {
                            Console.Out.WriteLine("Skip for Reg ID - " + regID);
                            log.CreateLogEntry("Skip for Reg ID - " + regID, +logCnt);

                        }
                    }
                }

                //Console.ReadLine();
                System.Environment.Exit(1);
            }
            catch(Exception ex) 
            {                
                Console.WriteLine(ex.Message + " - " + ex.StackTrace);
                log.CreateLogEntry("GenerateAgreementPDFService failed. Reason: " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error, +logCnt);
                //Console.ReadLine();
                System.Environment.Exit(1);
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            }

        }
    }
}
