using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.TaxonomyLookup
{
    public class TaxonomyLookup : BaseJob, IJob
    {

        private Logging log = null;

        public TaxonomyLookup(Guid threadId) : base(threadId)
        {
            this.ThreadId = Guid.Parse("1AB1EDAA-3891-4F56-A92B-8EDFEDEB62FA");
        }
        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(Guid.Parse("1AB1EDAA-3891-4F56-A92B-8EDFEDEB62FA"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                case "1AB1EDAA-3891-4F56-A92B-8EDFEDEB62FA":
                    this.startTaxonomyLookup();
                    break;
            }
        }

        private void startTaxonomyLookup()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);

            try
            {
                int taxonomyTypeId = 0;
                string npi = string.Empty;
                string taxonomy = string.Empty;
                int maxRetries = 5;
                int count = 0;
                bool retry = false;

                DataSet dsTrans = TaxonomyLookupHelper.SelectNPITaxonomyIDs(log, this.ThreadId);
                if (ObjectControllerHelper.HasRows(dsTrans))
                {
                    foreach (DataRow row in dsTrans.Tables[0].Rows)
                    {
                        taxonomyTypeId = ObjectControllerHelper.GetInt("TAXONOMY_TYPE_ID", row);
                        npi = ObjectControllerHelper.GetString("NPI", row);
                        taxonomy = ObjectControllerHelper.GetString("TAXONOMY_CODE", row);

                        do
                        {
                            retry = TaxonomyLookupHelper.makeNPPESAPICall(log, this.ThreadId, taxonomyTypeId, npi, taxonomy);
                            if (retry)
                            {
                                npi = TaxonomyLookupHelper.FetchNewNPIFromTaxonomy(log, this.ThreadId, taxonomy);
                                count++;
                            }
                            
                        } while (retry && count < maxRetries);
                        count = 0;
                    }
                }
            }catch(Exception ex)
            {
                log.CreateLogEntry(String.Format("TaxonomyLookup Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }
    }
}
