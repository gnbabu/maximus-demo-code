using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MMISInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.MMISTasks
{
    public class MMISExecuteRetrieveTask<TRequest> : BaseTask
        where TRequest : MMISRequest
    {
        #region Properties

        public Guid ThreadID { get; set; }
        public int TransactionTypeID { get; set; }
        #endregion

        #region Steps

        public IMMISLoadDataSet LoadDataSetStep { get; set; }
        public IMMISReadFile<TRequest> RetrieveProviderRecordsStep { get; set; }
        public IMMISUpdateTransaction UpdateTransactionStep { get; set; }
        public List<IMMISPostProcess<TRequest>> PostProcessSteps { get; set; }
        public IMMISPopulateRequest<TRequest> PopulateRequestStep { get; set; }
        public IMMISProcessResponse<TRequest> ProcessResponseStep { get; set; }
        public IMMISProcessErrorStep<TRequest> ProcessErrorStep { get; set; }
        public IMMISSendFile SendFileStep { get; set; }

        #endregion


        #region Constructors

        public MMISExecuteRetrieveTask(Guid threadID, int transactionTypeID) :
            this(threadID, "MMISSubmitTransactionTask", transactionTypeID)
        {

        }

        public MMISExecuteRetrieveTask(Guid threadID, string name, int transactionTypeID) :
            base(name)
        {
            this.ThreadID = threadID;
            this.TransactionTypeID = transactionTypeID;

            this.PostProcessSteps = new List<IMMISPostProcess<TRequest>>();
        }

        #endregion

        public override bool ExecuteTask(TaskData result)
        {
            // create log object
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadID, logProcessName);

            log.CreateLogEntry(string.Format(Constants.LogString.BeginTaskName, Name));

            try
            {
                MMISShared ms = new MMISShared(this.ThreadID);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());
                
                if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                {
                    FileInfo[] files = RetrieveProviderRecordsStep.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        this.ProcessFile(ms, log, result, file);
                    } 
                }
                else
                {
                    this.Process(null, ms, log, result);
                }
                

                log.CreateLogEntry(Constants.LogString.RetrievingMMISRecordsEnd);

                log.CreateLogEntry(string.Format(Constants.LogString.EndTaskName, Name));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadID, ex, logProcessName);
            }

            return true;
        }

        private void ProcessFile(MMISShared ms, Logging log, TaskData taskData, FileInfo file)
        {
            // Read returned records from file
            if (RetrieveProviderRecordsStep == null)
            {
                throw new Exception("RetrieveProviderRecordsStep property must be set.");
            }

            TRequest[] returnedRecords = RetrieveProviderRecordsStep.ReadRecords(file);

            this.Process(returnedRecords, ms, log, taskData);
        }

        private void Process(TRequest[] returnedRecords, MMISShared ms, Logging log, TaskData taskData)
        {
            if (LoadDataSetStep == null)
            {
                throw new Exception("LoadDataSetStep property must be set.");
            }

            // Load submitted provider data 
            DataSet ds = LoadDataSetStep.LoadDataSet();

            #region Validate DataSet

            if (ds == null)
            {
                throw new Exception("No DataSet returned from LoadDataSetStep.LoadDataSet().");
            }

            if (ds.Tables.Count < 1)
            {
                throw new Exception("No tables returned from LoadDataSetStep.LoadDataSet().");
            }
 
            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart, LoadDataSetStep.GetRowCount(ds), this.TransactionTypeID));

            #endregion

            DateTime now = DateTime.Now;
            
            /// Loop through the records in the first table returned in the dataset.
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                TransactionData data = new TransactionData();
                data.TransactionTypeID = this.TransactionTypeID;

                try
                {
                    PopulateRequestStep.PopulateTransactionData(data, dataRow);

                    if (ProcessResponseStep  == null)
                    {
                        throw new Exception("ProcessResponseStep  property must be set.");
                    }
                    TRequest response =  ProcessResponseStep.Process(returnedRecords, dataRow, ms, log);

                    if (response != null)
                    {
                        if (UpdateTransactionStep != null)
                        {
                            UpdateTransactionStep.UpdateTransaction(data, dataRow, now);
                        }

                        if (PostProcessSteps == null)
                        {
                            /// Note: Maybe this doesn't need to be required? Or force to create an 'empty' update transaction step
                            throw new Exception("PostProcessStep property must be set.");
                        }

                        foreach (var step in PostProcessSteps)
                        {
                            step.PostProcess(response, data, ms, now, log);
                        }
                        if (ProcessErrorStep != null)
                        {
                            ProcessErrorStep.ProcessDataError(data, response, ms);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw CoreException.ThrowException(this.ThreadID, ex, log.ProcessName);
                }

                taskData.Transactions.Add(data);
            }
        }
    }
}
