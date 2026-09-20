using IBM.WMQ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBMMQWP
{
    static class WebSphereLib
    {
        static MQQueueManager myQM;
        static MQMessage queueMessage;

        static public string strQueueManagerName
        {

            get;
            set;
        }
        static public string strChannelName
        {
            get;
            set;
        }
        static public string strQueueName
        {
            get;
            set;
        }
        static public string strServerName
        {
            get;
            set;
        }
        static public string strUserName
        {
            get;
            set;
        }
        static public string strPassword
        {
            get;
            set;
        }
        static public string strCertificateName
        {
            get;
            set;
        }
        static public string strCipherName
        {
            get;
            set;
        }
        static public string strCertificateStore
        {
            get;
            set;
        }
        static public string strTransportProperty
        {
            get;
            set;
        }
        static public string strMsg
        {
            get;
            set;
        }
        static public int intPort
        {
            get;
            set;
        }
        static string MainLogDirectory;
        static string AppLogDirectory;

        static DateTime LogDateDT;
        static string LogDate;
        static string AppLogFile;

        static public void Initialize_WebSphereLib()
        {
            //Initialisation
            MainLogDirectory = Directory.GetCurrentDirectory();
            AppLogDirectory = "\\App";

            MainLogDirectory += "\\IBMMQ";

            LogDateDT = DateTime.Parse(DateTime.Now.ToString());
            LogDate = LogDateDT.ToString("MM.dd.yyyy");
            AppLogFile = (MainLogDirectory + AppLogDirectory + "\\AppLog." + LogDate + ".txt");


            AppLogDirectory = MainLogDirectory + AppLogDirectory;
            if (Directory.Exists(AppLogDirectory) == false)
            {
                Directory.CreateDirectory(AppLogDirectory);
            }

        }

        public static string WriteLogToFile(string LogName, string LogText)
        {
            try
            {
                using (System.IO.StreamWriter file = System.IO.File.AppendText(LogName))
                {
                    file.WriteLine(DateTime.Now.ToString() + " -- " + LogText);
                    file.Close();
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void SetDefaults()
        {
            WriteLogToFile(AppLogFile, "Start SetDefaults");
            /*
            strQueueManagerName = "OMES_SI_QMGR_PROXY_SIT";
            strChannelName = "PNM.CHANNEL";
            strQueueName = "PNM.PROVIDER.REPLY.Q";
            strServerName = "mqproxy.test.oh.healthinteractive.net";
            strUserName = "pnmmqsit";
            strPassword = "Password";
            strCertificateStore = "SYSTEM";
            strCertificateName = "service.ohpnm-testing.omes.maximus.com";
            strCipherName = "TLS_RSA_WITH_AES_256_CBC_SHA256";
            strTransportProperty = "Managed Client";
            intPort = 1414;
            strMsg = "This is a test Maximus Message";
            */
            strQueueManagerName = "QM1";
            strChannelName = "DEV.APP.SVRCONN";
            strQueueName = "DEV.QUEUE.1";
            strServerName = "192.168.1.3";
            strUserName = "app";
            strPassword = "passw0rd";
            strCertificateStore = "USER";
            strCertificateName = "";
            strCipherName = "TLS_RSA_WITH_AES_256_CBC_SHA256";
            strTransportProperty = "MQSeries";
            intPort = 1414;
            strMsg = "This is a test Maximus Message";
        }

        public static string Connect()
        {
            WriteLogToFile(AppLogFile, "Start Connect Method");
            WriteLogToFile(AppLogFile, "Server Name = "+ strServerName);
            WriteLogToFile(AppLogFile, "Channel Name = " + strChannelName);
            WriteLogToFile(AppLogFile, "Port Number = " + intPort.ToString());
            WriteLogToFile(AppLogFile, "Certificate Name = " + strCertificateName);
            WriteLogToFile(AppLogFile, "Certificate Store = " + strCertificateStore);
            WriteLogToFile(AppLogFile, "Cipher Name = " + strCipherName);
            WriteLogToFile(AppLogFile, "Transport Property = " + strTransportProperty);
            WriteLogToFile(AppLogFile, "User Name = " + strUserName);
            WriteLogToFile(AppLogFile, "Password  = <Omitted>");
            WriteLogToFile(AppLogFile, "Queue Manager Name = " + strQueueManagerName);

            string ReturnValue = string.Empty;

            //Setup Queue manager Properties
            Hashtable queueProperties = new Hashtable
            {
                { MQC.CHANNEL_PROPERTY, strChannelName },
                { MQC.HOST_NAME_PROPERTY, strServerName },
                { MQC.PORT_PROPERTY, intPort },

            };

            if (strTransportProperty == "Managed Client")
            {
                queueProperties.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED);
            }
            else
            {
                queueProperties.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES);
            }

            //queueProperties.Add(MQC.SSL_CERT_STORE_PROPERTY, @"*USER");
            //queueProperties.Add(MQC.SSL_CERT_STORE_PROPERTY, @"*SYSTEM");
            queueProperties.Add(MQC.SSL_CERT_STORE_PROPERTY, @"*" + strCertificateStore);
            queueProperties.Add(MQC.SSL_CIPHER_SPEC_PROPERTY, strCipherName);

            WriteLogToFile(AppLogFile, "Set MQEnvironment");

            //Enable SSL
            MQEnvironment.SSLCipherSpec = strCipherName;
            /*
                CertificateLabel doesn't seem to be used by the library to find the certificate
                The code looks for a certificate in the specified SSL_CERT_STORE_PROPERTY the matches: 
                the string: ibmwebspheremq<username> as in ibmwebspheremqbrian
                However, if the certificate label is set it needs to match the ibmwebspheremq<username>
            */
            MQEnvironment.CertificateLabel = strCertificateName;


            //Set Username
            MQEnvironment.UserId = strUserName;

            //Set Passowrd
            MQEnvironment.Password = strPassword;

            //Define a Queue Manager
            try
            {
                WriteLogToFile(AppLogFile, "Define a Queue Manager");

                myQM = new MQQueueManager(strQueueManagerName, queueProperties);

                ReturnValue = "Successfully Connected to " + strQueueManagerName;
            }
            catch (Exception ex)
            {
                ReturnValue = ex.Message;
                if (!(ex.InnerException is null))
                {
                    if (string.IsNullOrEmpty(ex.InnerException.Message) == false)
                    {
                        ReturnValue += ":: InnerException : " + ex.InnerException.Message;
                    }
                }
                WriteLogToFile(AppLogFile, "End Connect Method "+ReturnValue);
                return ReturnValue;
            }

            WriteLogToFile(AppLogFile, "End Connect Method" + ReturnValue);
            return ReturnValue;
        }

        public static string PubMsg()
        {
            WriteLogToFile(AppLogFile, "Start PubMsg");
            WriteLogToFile(AppLogFile, "Message = " + strMsg);
            WriteLogToFile(AppLogFile, "Put Queue Name = " + strQueueName);

            string ReturnValue = string.Empty;
            try
            {
                WriteLogToFile(AppLogFile, "Setup Queue Message");
                queueMessage = new MQMessage();
                queueMessage.Format = MQC.MQFMT_STRING;
                queueMessage.CharacterSet = Encoding.UTF8.CodePage;
                queueMessage.Write(Encoding.UTF8.GetBytes(strMsg));

                WriteLogToFile(AppLogFile, "Define a Queue" + ReturnValue);
                //Define a Queue
                var queue = myQM.AccessQueue(strQueueName, MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);
                MQPutMessageOptions queuePutMessageOptions = new MQPutMessageOptions();
                queue.Put(queueMessage, queuePutMessageOptions);
                queue.Close();
                ReturnValue = "Success :: " + strMsg;
            }
            catch (Exception exp)
            {
                ReturnValue = exp.Message;
                if (!(exp.InnerException is null))
                {
                    if (string.IsNullOrEmpty(exp.InnerException.Message) == false)
                    {
                        ReturnValue += ":: InnerException : " + exp.InnerException.Message;
                    }
                }

                WriteLogToFile(AppLogFile, "End PubMsg "+ReturnValue);
                return ReturnValue;
            }
            WriteLogToFile(AppLogFile, "End PubMsg " + ReturnValue);
            return ReturnValue;
        }

        public static string ReadMsg()
        {
            WriteLogToFile(AppLogFile, "Start ReadMsg");
            WriteLogToFile(AppLogFile, "Message = " + strMsg);
            WriteLogToFile(AppLogFile, "Get Queue Name = " + strQueueName);

            string ReturnValue = string.Empty;

            try
            {
                WriteLogToFile(AppLogFile, "Setup QueueMessage");

                queueMessage = new MQMessage();
                queueMessage.Format = MQC.MQFMT_STRING;
                queueMessage.CharacterSet = Encoding.UTF8.CodePage;

                WriteLogToFile(AppLogFile, "Define a Queue");
                //Define a Queue
                var queue = myQM.AccessQueue(strQueueName, MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);
                queue.Get(queueMessage);
                strMsg = queueMessage.ReadString(queueMessage.MessageLength);
                ReturnValue = "Success :: " + strMsg;
                queue.Close();
            }
            catch (Exception exp)
            {
                ReturnValue = exp.Message;
                if (!(exp.InnerException is null))
                {
                    if (string.IsNullOrEmpty(exp.InnerException.Message) == false)
                    {
                        ReturnValue += ":: InnerException : " + exp.InnerException.Message;
                    }
                }
                WriteLogToFile(AppLogFile, "End ReadMsg "+ReturnValue);
                return ReturnValue;
            }
            WriteLogToFile(AppLogFile, "End ReadMsg " + ReturnValue);
            return ReturnValue;
        }
    }
}
