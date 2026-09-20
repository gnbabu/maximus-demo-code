using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.EMS;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace TibcoDocImportNetCore
{
    class GetTibcoMessage : IExceptionListener
    {
        Connection connection = null;
        Session session = null;
        MessageConsumer msgConsumer = null;
        Destination destination = null;

        public string LastMessage = "";
        public int WorkerId = 0;  // this is passed in the command line as the only argument and set from calling process
        public string WorkDir = "";  // set from calling process
        public int JobId = 1; // for logging
        public int CurrentLogId = 0; // for logging
        public bool StopExecution = false;
        public string CNString = "";  // set from calling process
        public string RJkey = "";  // set from calling process
        public string RJiv = "";  // set from calling process
        public string UploadURL = "";  // set from calling process
        public string ServerUrl = "";  // set from calling process
        public string QueueName = "";  // set from calling process
        public string TBUser = "";  // set from calling process
        public string TBPwd = "";  // set from calling process
        public string UseSSL = "";  // set from calling process
        public string TargetHost = "";  // set from calling process
        public string CertName = "";  // set from calling process
        public string KillFile = "";  // set from calling process

        public void OnException(EMSException e)
        {
            // print the connection exception status
            // Console.Error.WriteLine("Connection Exception: " + e.Message);
            // add stack trace
            string exmsg = "### CONNECTION EXCEPTION: " + e.ErrorCode + " >>> " + e.Message + " ### SOURCE: " + e.Source + " ### STACK TRACE: " + e.StackTrace;

            string fname = Path.Combine(this.WorkDir, "_LAST_CONNECTION_ERROR.txt");
            if (File.Exists(fname)) { File.Delete(fname); }
            File.WriteAllText(fname, exmsg);
            
        }

        public bool GetNextMessage()
        {
            // Console.WriteLine("opening settings file");

            Message msg = null;
            int msgNum = 0;


            String AllSettings = "\r\n serverUrl:" + this.ServerUrl + " \r\n queueName:" + this.QueueName + " \r\n tbUser:" + this.TBUser + " \r\n tbPwd:********* \r\n useSSL:" + this.UseSSL + " \r\n targetHost:" + this.TargetHost;
            Console.WriteLine(AllSettings);

            LogStatusMessage("Opening Tibco Queue to read messages", "STARTUP");

            LogStatusMessage("Subscribing to destination: " + this.QueueName);

            // initialize SSL environment
            Hashtable environment = new Hashtable();

            try
            {
                LogStatusMessage("creating factory for: " + this.ServerUrl + "\n");
                ConnectionFactory factory = new TIBCO.EMS.ConnectionFactory(this.ServerUrl);
                //QueueConnectionFactory factory = new QueueConnectionFactory(serverUrl);


                if (this.UseSSL.ToUpper() == "YES")
                {
                    environment.Add(EMSSSL.TRACE, true);
                    environment.Add(EMSSSL.TARGET_HOST_NAME, this.TargetHost);

                    LogStatusMessage("connected - init cert");

                    // - SET VIA STORED CERTS

                    EMSSSLSystemStoreInfo storeInfo = new EMSSSLSystemStoreInfo();

                    storeInfo.SetCertificateStoreLocation(System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine);
                    storeInfo.SetCertificateNameAsFullSubjectDN(this.CertName);

                    factory.SetCertificateStoreType(EMSSSLStoreType.EMSSSL_STORE_TYPE_SYSTEM, storeInfo);

                    factory.SetTargetHostName(this.TargetHost);

                    environment.Add(EMSSSL.STORE_INFO, storeInfo);
                    environment.Add(EMSSSL.STORE_TYPE, EMSSSLStoreType.EMSSSL_STORE_TYPE_SYSTEM);

                    LogStatusMessage("cert set - init connection");
                }
                else
                {
                    LogStatusMessage("useSSL = NO - init connection");
                }

                // create the connection
                connection = factory.CreateConnection(this.TBUser, this.TBPwd);
                //QueueConnection connection = factory.CreateQueueConnection(tbUser, tbPwd);

                LogStatusMessage("connected - creating session");

                // create the session
                session = connection.CreateSession(false, Session.CLIENT_ACKNOWLEDGE);

                //QueueSession session = connection.CreateQueueSession(false, TIBCO.EMS.Session.EXPLICIT_CLIENT_ACKNOWLEDGE);


                LogStatusMessage("created - setting exception listener");


                // set the exception listener
                connection.ExceptionListener = this;

                LogStatusMessage("set - creating destination queue");


                // create the destination
                destination = session.CreateQueue(this.QueueName);

                LogStatusMessage("created - creating consumer");


                // create the consumer
                msgConsumer = session.CreateConsumer(destination);

                LogStatusMessage("created - starting connection");


                // start the connection
                connection.Start();

                LogStatusMessage("connection started - begin primary message loop -- press ESC to interrupt \n");

                //-----------------------------------------------------------------
                // PRIMARY MESSAGE LOOP
                //-----------------------------------------------------------------

                while (this.StopExecution == false )
                {
                    // check for kill file  (in case connection is lost / can kill all at once by copying kill file to config location
                    
                    if (File.Exists(this.KillFile)) {
                        this.StopExecution = true;
                        break;  // break out of loop
                    }

                    // check for interrupt
                    //if (Console.KeyAvailable)
                    //{
                    //    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    //    {
                    //        this.StopExecution = true;
                    //        break;  // break out of loop
                    //    }
                    //}

                    // receive next message                    
                    InitLogItem();
                    msg = msgConsumer.Receive();  // this will sit and wait for a message to be placed in the queue so if there are no messages, you will get a long time for step 1 execution time
                    if (msg == null)  // should not happen
                    {
                        LogStatusMessage("ERROR Invalid or Empty Message", "ERROR");
                        this.LastMessage = "NO_MESSAGE_AVAILABLE";
                        this.StopExecution = true;
                        continue;
                    }

                    msgNum++;
                    // Console.WriteLine("processing message #" + msgNum.ToString());

                    this.LastMessage = msg.ToString();

                    this.ProcessMessage(this.LastMessage, msg.MessageID);

                    // Console.WriteLine("Acknowledging Message");
                    msg.Acknowledge();
                    // Console.Write(msgNum.ToString() + " message complete");

                }  // end primary message loop

                LogStatusMessage("interrupt found - closing connection");

                // close the connection
                connection.Close();

                LogStatusMessage("connection closed - shutting down worker", "SHUTDOWN");

                return true;


            }
            catch (Exception ex)
            {
                Exception x1;
                Exception x2;
                Exception x3;

                string exmsg = "### OUTER EXCEPTION: " + ex.Message;

                if (ex is EMSException)
                {
                    EMSException je = (EMSException)ex;
                    if (je.LinkedException != null)
                    {
                        exmsg += "\r\n\r\n ### Linked Exception Error Msg: " + je.Message;
                        exmsg += "\r\n\r\n ### Linked Exception:";
                        exmsg += "\r\n\r\n ### LINKED STACK TRACE" + je.LinkedException.StackTrace;
                    }
                }

                // check for nested exceptions
                if (ex.InnerException != null)
                {
                    x1 = ex.InnerException;
                    exmsg += "\r\n\r\n ### INNER EXCEPTION 1: " + x1.Message;

                    if (x1.InnerException != null)
                    {
                        x2 = x1.InnerException;
                        exmsg += "\r\n\r\n ### INNER EXCEPTION 2: " + x2.Message;

                        if (x2.InnerException != null)
                        {
                            x3 = x2.InnerException;
                            exmsg += "\r\n\r\n ### INNER EXCEPTION 3: " + x3.Message;

                            if (x3.InnerException != null)
                            {
                                exmsg += "\r\n\r\n ### INNER EXCEPTION 4: " + x3.InnerException.Message;
                            }
                        }
                    }
                }

                // add stack trace
                exmsg += "\r\n\r\n ### STACK TRACE: " + ex.StackTrace;

                // add current settings
                exmsg += "\r\n" + AllSettings;

                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string fname = Path.Combine(this.WorkDir, "_LAST_ERROR_" + unixTimestamp.ToString() + ".txt");
                if (File.Exists(fname)) { File.Delete(fname); }
                  File.WriteAllText(fname, exmsg);

                LogStatusMessage(ex.Message, "ERROR");
                LogStatusMessage("SHUTDOWN DUE TO ERROR" + ex.Message, "ERROR");
                return false;
            }


        }

        public string ProcessMessage(string msg, string msgid, bool SendToFile = false)
        {           
            string f = "";
            int docid = 0;
            int regid = 0;
            string doctype = "";
            string idxml = "";
            string[] sep = { "$MsgTextBody$" };
            string[] parts = msg.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            string x = "";
            string base64Encrypted = "";
            string outPath = Path.Combine(this.WorkDir, "test"); ;
            string base64Key = "";
            string base64IV = "";
            long TibcoDocId = 0;
            long TibcoSeq = 0;
            IConfiguration cfg = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            var sec = new SecretsManager(cfg.GetSection("TIBCOSecretsRegion").Value);
            var result = sec.GetSuperSecretPassword(cfg.GetSection("TIBCOSecretDictionary").Value);
            result.Wait();

            base64Key = result.Result["base64Key"];
            base64IV = result.Result["base64IV"];


            // for testing from file
            if (this.CurrentLogId == 0) { InitLogItem(); }
            
            string problemFileDir = Path.Combine(this.WorkDir, "problems");

            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            try
            {
                msg = msg.Split("Text={")[1];
                msg = msg.Replace("{", "");
                msg = msg.Replace("}", "");

                XDocument d = XDocument.Parse(msg);

                f = (string)d.Elements("DocumentIndex").FirstOrDefault().Elements("Filename").FirstOrDefault();
                doctype = (string)d.Elements("DocumentIndex").FirstOrDefault().Elements("DocumentType").FirstOrDefault();
                TibcoDocId = (Int64)d.Elements("DocumentIndex").FirstOrDefault().Elements("DocumentID").FirstOrDefault();
                TibcoSeq = (Int64)d.Elements("DocumentIndex").FirstOrDefault().Elements("SequenceNumber").FirstOrDefault();
                base64Encrypted = (string)d.Elements("DocumentIndex").FirstOrDefault().Elements("Document").FirstOrDefault();

                // get inner xml ids to pass to sql as xml string
                var startTag = "<identifierTypes>";
                int startIndex = msg.IndexOf(startTag) + startTag.Length;
                int endIndex = msg.IndexOf("</identifierTypes>", startIndex);
                idxml = "<root>" + msg.Substring(startIndex, endIndex - startIndex) + "</root>";

                f = f.Replace("{", "");
                f = f.Replace("}", "");
                // Console.WriteLine("Filename: " + f);

                // decrypt and write to file

                RijndaelManaged aes = new RijndaelManaged();
                aes.BlockSize = 128;
                aes.KeySize = 128;

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] keyArr = Encoding.ASCII.GetBytes(base64Key);
                byte[] KeyArrBytes16Value = new byte[16];
                Array.Copy(keyArr, KeyArrBytes16Value, 16);

                // Initialization vector.   
                // It could be any value or generated using a random number generator.
                byte[] ivArr = Encoding.ASCII.GetBytes(base64IV);
                byte[] IVBytes16Value = new byte[16];
                Array.Copy(ivArr, IVBytes16Value, 16);

                aes.Key = KeyArrBytes16Value;
                aes.IV = IVBytes16Value;

                ICryptoTransform decrypto = aes.CreateDecryptor();

                byte[] encryptedBytes = Convert.FromBase64CharArray(base64Encrypted.ToCharArray(), 0, base64Encrypted.Length);
                byte[] decryptedData = decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                // fix sql reserved words
                idxml = idxml.Replace("key", "xkey");
                idxml = idxml.Replace("value", "xval");
                // cleanup xml
                idxml = idxml.Replace(" ", "");
                idxml = idxml.Replace("\t", "");
                idxml = idxml.Replace("\r", "");
                idxml = idxml.Replace("\n", "");
                idxml = idxml.Replace("ENTRY", "entry");


                // register in database
                string spName = "usp_SaveProviderDocumentFromTibcoMQ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("IDXML", DbType.String, idxml, false));
                parameters.Add(SqlParms.CreateParameter("DOC_TYPE_CODE", DbType.String, doctype, false));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, f, false));
                string rtn = DataAccess.ExecuteScalar(spName, parameters, CNString);

                if (rtn.StartsWith("DOC_ID:"))
                {
                    string[] r = rtn.Split(':');
                    docid = int.Parse(r[1]);
                    regid = int.Parse(r[3]);

                    CompleteLogStep(1, false, false, "", msgid, docid, decryptedData.Length, TibcoDocId, TibcoSeq, 0);

                    if (SendToFile)  // only for testing
                    {
                        string fullPath = outPath + f;
                        if (File.Exists(fullPath)) { File.Delete(fullPath); }
                        File.WriteAllBytes(fullPath, decryptedData);
                    }

                    // send to onbase
                    // Console.WriteLine("sending to onbase");
                    OnBaseInterface obi = new OnBaseInterface();
                    obi.RJkey = this.RJkey;
                    obi.RJiv = this.RJiv;
                    obi.UploadURL = this.UploadURL;
                    obi.WorkDir = this.WorkDir;
                    obi.cnString = this.CNString;

                    int OnBDocId = obi.SubmitFile(docid, decryptedData, f);

                    if (OnBDocId == 0)
                    {
                        this.StopExecution = true;  // do this to avoid loading up lots of errors and filling up the disk
                        string emsg = "ERROR: ONBASE COULD NOT ACCEPT FILE: " + f;

                        // save message to problem dir
                        string fname = Path.Combine(problemFileDir, "onbase-err-" + unixTimestamp.ToString() + "-msg.txt");
                        fname = string.Concat(fname.Split(Path.GetInvalidFileNameChars()));
                        // Console.WriteLine("writing to file: " + fname + "  \n");
                        File.WriteAllText(fname, msg);

                        CompleteLogStep(2, true, true, emsg, msgid, OnBDocId, decryptedData.Length, TibcoDocId, TibcoSeq, 0);
                        return emsg;
                    } else
                    {
                        CompleteLogStep(2, true, false, "", msgid, OnBDocId, decryptedData.Length, TibcoDocId, TibcoSeq, 0);
                    }
                 
                }
                else  // problem occurred (save file to text?)
                {
                    string fname = Path.Combine(problemFileDir, "db-err-" + unixTimestamp.ToString() + "-msg.txt");
                    fname = string.Concat(fname.Split(Path.GetInvalidFileNameChars()));
                    // Console.WriteLine("writing to file: " + fname + "  \n");
                    File.WriteAllText(fname, msg);

                    this.StopExecution = true;  // do this to avoid loading up lots of errors and filling up the disk
                    
                    string emsg = "ERROR:" + rtn + " FILE: " + fname;
                    CompleteLogStep(1, true, true, emsg, msgid, 0, decryptedData.Length, TibcoDocId, TibcoSeq, 0);
                    return emsg;

                    
                }
            }
            catch (Exception ex)
            {
                string fname = Path.Combine(problemFileDir, "err-" + unixTimestamp.ToString() + "-msg.txt");
                fname = string.Concat(fname.Split(Path.GetInvalidFileNameChars()));

                // Console.WriteLine("writing error to file: " + fname + "  \n");
                msg = ex.Message + "\r\n\r\n --------------------------------------------------------------------------------------------------------------------------------- \r\n\r\n" + msg + " \r\n\r\n " + ex.StackTrace;
                File.WriteAllText(fname, msg);
                
                string emsg = "ERROR:" + ex.Message + " FILE: " + fname;                
                CompleteLogStep(1, true, true, emsg, msgid, 0, msg.Length, TibcoDocId, TibcoSeq, 0);
                this.StopExecution = true;  // do this to avoid loading up lots of errors and filling up the disk
                return emsg;
            }

            return f;

        }


        public void LogStatusMessage(string msg, string msgType = "MISC")
        {
            // Console.WriteLine(msg);
            if (msg.Length > 255) { msg = msg.Substring(0, 255);  }
            string spName = "usp_LogBulkImportStatusMessage";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("JobId", DbType.Int32, this.JobId, false));
            parameters.Add(SqlParms.CreateParameter("WorkerId", DbType.Int32, this.WorkerId, false));
            parameters.Add(SqlParms.CreateParameter("StatusType", DbType.String, msgType, false));
            parameters.Add(SqlParms.CreateParameter("StatusMessage", DbType.String, msg, false));
            DataAccess.ExecuteStoredProcedure(spName, parameters, CNString);
       }

        public void InitLogItem()  // init log for current piece of work
        {
            string spName = "usp_LogBulkImportInit";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("JobId", DbType.Int32, this.JobId, false));
            parameters.Add(SqlParms.CreateParameter("WorkerId", DbType.Int32, this.WorkerId, false));
            string rtn = DataAccess.ExecuteScalar(spName, parameters, CNString);
            this.CurrentLogId = int.Parse(rtn);
            // Console.WriteLine("Opening Log #:" + rtn);
        }

        public void CompleteLogStep(int stepNumber, bool isComplete, bool errOccurred, string errMessage, string UnitOfWork, long RefId, int Metric, long OtherId1, long OtherId2, long OtherId3)
        {
            // Console.WriteLine("complete step for log#:" + this.CurrentLogId.ToString() + " step: " + stepNumber.ToString() + " refid: " + RefId.ToString() + " metric:" + Metric.ToString() + " msg:" + UnitOfWork + " err:" + errMessage);
            string spName = "usp_LogBulkImportCompleteStep";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("LogId", DbType.Int32, this.CurrentLogId, false));
            parameters.Add(SqlParms.CreateParameter("Step", DbType.Int16, stepNumber, false));
            parameters.Add(SqlParms.CreateParameter("IsComplete", DbType.Int16, (isComplete ? 1 : 0), false));
            parameters.Add(SqlParms.CreateParameter("ErrOccurred", DbType.Int16, (errOccurred ? 1 : 0), false));
            parameters.Add(SqlParms.CreateParameter("ErrMessage", DbType.String, errMessage, false));
            parameters.Add(SqlParms.CreateParameter("UnitOfWork", DbType.String, UnitOfWork, false));  // this is only set on step 1
            parameters.Add(SqlParms.CreateParameter("RefId", DbType.Int64, RefId, false));
            parameters.Add(SqlParms.CreateParameter("Metric", DbType.Int32, Metric, false));
            parameters.Add(SqlParms.CreateParameter("OtherId1", DbType.Int64, OtherId1, false));
            parameters.Add(SqlParms.CreateParameter("OtherId2", DbType.Int64, OtherId2, false));
            parameters.Add(SqlParms.CreateParameter("OtherId3", DbType.Int64, OtherId3, false));
            DataAccess.ExecuteStoredProcedure(spName, parameters, CNString);
        }

    }
}
