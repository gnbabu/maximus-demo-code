using FileHelpers;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MCPN;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS
{
    public class CPCPracticeExport : BaseJob, IJob
    {

        private const string thisGuidString = "AE92E316-A3B9-41CF-87D5-65D1879DCC7D";
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

        public CPCPracticeExport(Guid threadId)
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
            LoadCPCProviders();
        }


        private void LoadCPCProviders()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry("Exporting CPC Practice Providers data");

            try
            {
                List<CPCPracticeInterfaceFileRecord> records = new List<CPCPracticeInterfaceFileRecord>();
                var cpcPracticeDataSet = DataAccess.ExecuteStoredProcedure("usp_SelectCPCPractice");
                if (cpcPracticeDataSet != null && cpcPracticeDataSet.Tables[0] != null && cpcPracticeDataSet.Tables[0].Rows != null)
                    log.CreateLogEntry(String.Format("Retrieved CPC Practice data [Records: {0}]", cpcPracticeDataSet.Tables[0].Rows.Count));
                else
                    log.CreateLogEntry("No Records for CPC Practice data");

                // loop over all records and write to database
                foreach (DataRow r in cpcPracticeDataSet.Tables[0].Rows)
                {
                    records.Add(CreateItemFromRow<CPCPracticeInterfaceFileRecord>(r));
                }
                if (records != null && records.Count > 0)
                {
                    //header record
                    records.Insert(0, CreateHeader());
                    log.CreateLogEntry(String.Format("Creating Roster data file [Records: {0}]", records.Count));
                    CreateFile(records);
                }
                log.CreateLogEntry("Exported Roster data");

            }
            catch (Exception ex)
            {
                MCPShared.LogFileRecordError(log, MethodBase.GetCurrentMethod().Name, "Credential Roster", Constants.LogString.FileLoadFailure + ex.Message);
                // NOTE: Do *not* rethrow exception so the next file is processed
            }

        }
        public bool CreateFile(List<CPCPracticeInterfaceFileRecord> records)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            bool returnVal = false;

            try
            {
                log.CreateLogEntry(Constants.LogString.LoadingConfig, +logCnt);

                string localPath = AppSettings.Get("CPCPracticeEnrollment-ExportLocalPath");
                string fileName = AppSettings.Get("CPCPracticeEnrollmentFile");
                string dtmWildcard = AppSettings.Get("CPCPracticeFileWildcardDTM");

                // Get file name
                fileName = fileName.Replace(dtmWildcard, DateTime.Now.ToString(dtmWildcard));
                localPath = localPath + fileName;

                FileHelperEngine engine = new FileHelperEngine(typeof(CPCPracticeInterfaceFileRecord));
                engine.WriteFile(localPath, records);

            }
            catch (Exception ex)
            {
                //string fullMessage = "File action failed. File Name: {0} Reason: {1}";
                //log.CreateLogEntry(String.Format(fullMessage, fileName, ex.Message), Logging.LogPriority.Error, +logCnt);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete, +logCnt);

            return returnVal;
        }
        private CPCPracticeInterfaceFileRecord CreateItemFromRow<T>(DataRow row) where T : new()
        {
            CPCPracticeInterfaceFileRecord CPCPracticeInterfaceFileRecord = SetItemFromRow(new CPCPracticeInterfaceFileRecord(), row);
            return CPCPracticeInterfaceFileRecord;
        }

        private CPCPracticeInterfaceFileRecord SetItemFromRow(CPCPracticeInterfaceFileRecord item, DataRow row)
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                Type type = typeof(CPCPracticeInterfaceFileRecord);
                PropertyInfo p = type.GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null)
                {
                    if (row[c] != DBNull.Value)
                    {
                        p.SetValue(item, "'" + row[c].ToString().Trim() + "'");
                    }
                    else
                    {
                        p.SetValue(item, "(null)");
                    }                   
                }
                
            }
            return item;
        }

        private CPCPracticeInterfaceFileRecord CreateHeader()
        {
            var cpcHeader = new CPCPracticeInterfaceFileRecord
            {
                
                PracticeName = "PracticeName",
                OriginalEnrollmentYear = "OriginalEnrollmentYear",
                MedicaidId = "MedicaidId",
                CPCID = "CPCID",
                PracticePartnershipID = "PracticePartnershipID",
                ConvenerPractice = "ConvenerPractice",
                FederalTaxID = "FederalTaxID",
                CPCKidsFlag = "CPCKidsFlag",
                ContactName = "ContactName",
                Email = "Email",
                Phone = "Phone",
                Address = "Address",
                City = "City",
                State = "State",
                Zip = "Zip",
                PracticeLocationCounty = "PracticeLocationCounty",
                Track = "Track",
                ProviderType = "ProviderType"
            };
            return cpcHeader;
        }


    }
}
