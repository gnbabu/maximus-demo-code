using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MCPN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS
{
    public class CHCProviderExport : BaseJob, IJob
    {

        private const string thisGuidString = "D2089AB4-053F-4131-B8FD-97CA90257AA4";
        private Guid thisGuid = new Guid(thisGuidString);


        FileInfo _currentFile;

        FileInfo CurrentFile
        {
            get
            {
                return _currentFile;
            }
            set
            {
                _currentFile = value;
            }
        }


        string _planId;

        String PlanId
        {
            get
            {
                return _planId;
            }
            set
            {
                _planId = value;
            }
        }

        public CHCProviderExport(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }



        private int logCnt = 0;


        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(thisGuid);
        }

        override public void ExecuteJob(Guid jobId)
        {
            LoadCHCProviders();
        }


        private void LoadCHCProviders()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry("Exporting CHC Providers data");

            try
            {
                List<string> records = new List<string>();
                int startingRegId = 0;
                int recordCount = 0;
                int providerCount = 0;
                log.CreateLogEntry("About to load providers");
                DataSet provider = LoadProvderData(startingRegId);

                if (provider != null && provider.Tables[0] != null && provider.Tables[0].Rows != null)
                    log.CreateLogEntry("Retrieved CHC Provider data starting extract");
                else
                    log.CreateLogEntry("No Records for CHC Provider data");


                string localPath = AppSettings.Get("CHCProviderFile-ExportLocalPath");
                string fileName = AppSettings.Get("CHCProviderFile");
                string dtmWildcard = AppSettings.Get("CHCProviderFileWildcardDTM");
                string seqWildcard = AppSettings.Get("CHCProviderFileWildcardSeq");
                string seqNo = AppSettings.Get("CHCProviderSeq");
                if (int.TryParse(seqNo, out int result))
                {
                    seqNo = (result + 1).ToString();
                }
                string env = fileName.ToLower().Contains("prod") ? "PROD" : "TEST";

                // Get file name
                fileName = fileName.Replace(dtmWildcard, DateTime.Now.ToString(dtmWildcard));

                fileName = fileName.Replace(seqWildcard, seqNo.ToString().PadLeft(6, '0'));
                localPath += fileName;

                int linesWritten = 0;

                if (provider.Tables[0].Rows.Count > 0)
                {
                    // Create a file to write to.
                    bool moreProviders = true;
                    using (StreamWriter sw = File.CreateText(localPath))
                    {
                        sw.WriteLine(CreateHeader(seqNo, env));
                        log.CreateLogEntry("CHC Header Created");
                        while (moreProviders)
                        {
                            // loop over all records and write to database
                            foreach (DataRow r in provider.Tables[0].Rows)
                            {
                                string provdierData = r["PROVIDER_DATA"].ToString();
                                sw.WriteLine(provdierData);
                                linesWritten++;
                            }
                            DataRow totalRow = provider.Tables[1].Rows[0];
                            providerCount += Convert.ToInt32(totalRow["PROVIDER_COUNT"]);
                            recordCount += Convert.ToInt32(totalRow["RECORD_COUNT"]);
                            startingRegId = Convert.ToInt32(totalRow["LAST_REG_ID"]);

                            // Release memory being used 
                            provider.Clear();
                            provider.Dispose();
                            log.CreateLogEntry(string.Format("New RegID: {0}", startingRegId));

                            provider = LoadProvderData(startingRegId);

                            if (provider.Tables[0].Rows.Count == 0)
                            {
                                moreProviders = false;
                            }
                        }

                        if (recordCount > 0)
                        {
                            //Trailer record
                            string trailer = CreateTrailer(providerCount, linesWritten); //do not count header or trailer row,

                            sw.WriteLine(trailer);
                            log.CreateLogEntry("Trailer Created");
                            sw.Close();
                            log.CreateLogEntry(String.Format("Created CHC Provider data file Providers in file [Records: {0}]", providerCount));
                            log.CreateLogEntry(String.Format("CHC Provider data file total records in file [Records: {0}]", recordCount));
                        }
                    }

                    DataAccess.ExecuteStoredProcedure("usp_CHCSeqIncrement");
                }


                log.CreateLogEntry("Exported CHC Provider data");

            }
            catch (Exception ex)
            {
                MCPShared.LogFileRecordError(log, MethodBase.GetCurrentMethod().Name, "CHC Provder", Constants.LogString.FileLoadFailure + ex.Message);
                // NOTE: Do *not* rethrow exception so the next file is processed
            }

        }
        private DataSet LoadProvderData(int startingRegId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("REG_ID", DbType.Int32, startingRegId, true)
                };

            DataSet provider = DataAccess.ExecuteStoredProcedure("usp_SelectCHCProviderData", parameters, "provider", 60000);
            return provider;
        }





        private string CreateHeader(string sequenceNumber, string env)
        {
            string header = "0001" + "DAILY_PROVIDER_FILE ".PadRight(40) + sequenceNumber.ToString().PadLeft(6, '0') + DateTime.Now.ToString("yyyyMMdd") + env.PadRight(10);

            return header;
        }

        private string CreateTrailer(int provderCount, int recordCount)
        {

            string trailer = "9999" + provderCount.ToString().PadLeft(9) + recordCount.ToString().PadLeft(9);
            return trailer;
        }





    }
}
