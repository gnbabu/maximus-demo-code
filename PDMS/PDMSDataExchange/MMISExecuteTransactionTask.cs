using FileHelpers;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MMISInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.MMISTasks
{
    public enum TransactionDirection
    {
        Submit = 0,
        Retrieve = 1
    }

    public class ExecuteTransaction<TRequest> : BaseTask
        where TRequest : MMISRequest
    {
        #region Internal enums


        #endregion


        #region Properties

        public Guid ThreadID { get; set; }
        public int TransactionTypeID { get; set; }
        public TransactionDirection TransactionDirection { get; set; }
        #endregion

        #region Steps

        public IMMISLoadDataSet LoadDataSetStep { get; set; }
        public IMMISPopulateRequest<TRequest> PopulateRequestStep { get; set; }
        public IMMISUpdateTransaction UpdateTransactionStep { get; set; }
        public List<IMMISPostProcess<TRequest>> PostProcessSteps { get; set; }
        public IMMISFilePath<TRequest> GetFilePathStep { get; set; }
        public IMMISSendFile SendFileStep { get; set; }
        public IMMISProcessErrorStep<TRequest> ProcessErrorStep { get; set; }

        #endregion


        #region Constructors


        public ExecuteTransaction(Guid threadID, int transactionTypeID, TransactionDirection transactionDirection) :
            this(threadID, "MMISSubmitTransactionTask", transactionTypeID, transactionDirection)
        {

        }

        public ExecuteTransaction(Guid threadID, string name, int transactionTypeID, TransactionDirection transactionDirection) :
            base(name)
        {
            this.ThreadID = threadID;
            this.TransactionTypeID = transactionTypeID;
            this.TransactionDirection = transactionDirection;

            this.PostProcessSteps = new List<IMMISPostProcess<TRequest>>();
        }

        #endregion

        #region Logging Properties

        public string LogEntryRecordsStartingFormat
        {
            get
            {
                switch (TransactionDirection)
                {
                    case MMISTasks.TransactionDirection.Submit:
                        return Constants.LogString.SubmittingMMISRecordsStart;
                    case MMISTasks.TransactionDirection.Retrieve:
                        return Constants.LogString.RetrievingMMISRecordsStart;
                    default:
                        return string.Empty;
                }
            }
        }

        public string LogEntryRecordFormat
        {
            get
            {
                switch (TransactionDirection)
                {
                    case MMISTasks.TransactionDirection.Submit:
                        return Constants.LogString.SubmittingMMISRecord;
                    case MMISTasks.TransactionDirection.Retrieve:
                        return Constants.LogString.RetrievingMMISRecord;
                    default:
                        return string.Empty;
                }
            }
        }

        public string LogEntryRecordNotProcessedFormat
        {
            get
            {
                switch (TransactionDirection)
                {
                    case MMISTasks.TransactionDirection.Submit:
                        return Constants.LogString.SubmittingMMISRecordNotProcessed;
                    case MMISTasks.TransactionDirection.Retrieve:
                        return Constants.LogString.RetrievingMMISRecordNotProcessed;
                    default:
                        return string.Empty;
                }
            }
        }

        public string LogEntryRecordsEndingFormat
        {
            get
            {
                switch (TransactionDirection)
                {
                    case MMISTasks.TransactionDirection.Submit:
                        return Constants.LogString.SubmittingMMISRecordsEnd;
                    case MMISTasks.TransactionDirection.Retrieve:
                        return Constants.LogString.RetrievingMMISRecordsEnd;
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        public override bool ExecuteTask(TaskData result)
        {

            // create log object
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadID, logProcessName);

            log.CreateLogEntry(string.Format(Constants.LogString.BeginTaskName, Name));

            DataSet ds;
            DateTime now = DateTime.Now;

            try
            {
                MMISShared ms = new MMISShared(this.ThreadID);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());

                if (LoadDataSetStep == null)
                {
                    throw new Exception("LoadDataSetStep property must be set.");
                }

                string fullFileName = GetFilePathStep.GetFilePath();
                string fileName = System.IO.Path.GetFileName(fullFileName);

                ds = LoadDataSetStep.LoadDataSet();
                #region Validate DataSet

                if (ds == null)
                {
                    throw new Exception("No DataSet returned from LoadDataSetStep.LoadDataSet().");
                }

                if (ds.Tables.Count < 1)
                {
                    throw new Exception("No tables returned from LoadDataSetStep.LoadDataSet().");
                }

                #endregion

                // Create the fixed file engine and set to local object
                FileHelpers.DelimitedFileEngine engine = new FileHelpers.DelimitedFileEngine(typeof(TRequest));
                // Sometimes there is race condition between file.Create and engine.AppendToFile, so only create empty file 
                // if there is no record to write. engine.AppendToFile does create the file
                if (!File.Exists(fullFileName) && ds.Tables[0].Rows.Count < 1)
                {
                    FileStream fs = File.Create(fullFileName);
                    fs.Close();
                }
                
                log.CreateLogEntry(String.Format(LogEntryRecordsStartingFormat, LoadDataSetStep.GetRowCount(ds), this.TransactionTypeID));

                /// Loop through the records in the first table returned in the dataset.
                foreach (DataRow dataRow in ds.Tables[0].Rows)
                {
                    TransactionData data = new TransactionData();
                    data.TransactionTypeID = this.TransactionTypeID;

                    try
                    {
                        if (PopulateRequestStep == null)
                        {
                            throw new Exception("PopulateRequestStep property must be set.");
                        }

                        TRequest request = PopulateRequestStep.PopulateRequest(dataRow, ms);

                        PopulateRequestStep.PopulateTransactionData(data, dataRow);

                        //  append the record to the file
                        engine.AppendToFile(fullFileName, request);

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
                            step.PostProcess(request, data, ms, now, log);
                        }
                        if (ProcessErrorStep != null)
                        {
                            ProcessErrorStep.ProcessDataError(data, request, ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw CoreException.ThrowException(this.ThreadID, ex, logProcessName);
                    }

                    result.Transactions.Add(data);
                }

                if (SendFileStep != null)
                {
                    SendFileStep.SendFile(fileName, ThreadID);
                }
                // record all bad errors
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        ms.LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                    }
                }
                log.CreateLogEntry(LogEntryRecordsEndingFormat);

                log.CreateLogEntry(string.Format(Constants.LogString.EndTaskName, Name));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadID, ex, logProcessName);
            }

            return true;
        }
    }
}
