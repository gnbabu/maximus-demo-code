using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MMISInterface;
using System;
using System.Collections.Generic;

namespace MAXIMUS.DataExchange.PDMS
{
	public class MMISDCS : BaseJob, IJob
	{
		#region "Constructors"

		public MMISDCS(Guid threadId)
			: base(threadId)
		{
			this.ThreadId = threadId;
		}

		#endregion

		#region Properties

		public List<TaskData> TaskResults { get; set; }

		#endregion
		
		public const string JOB_ID_MMIS_DIDD_SUBMIT_UPDATE_PROVIDER = "B47AEC16-F12B-49E9-83EF-364DF91E4FC3";
		public const string JOB_ID_MMIS_DIDD_RETRIEVE_UPDATE_PROVIDER = "CA06AEE2-C068-426C-B36E-2FDE0D80E51A";
        public const string JOB_ID_MMIS_WAIVER_SUBMIT_UPDATE_SERVICE_TYPE = "32BAE3EC-AE53-4193-9FE0-B6703CDBFEA2";
        public const string JOB_ID_MMIS_WAIVER_RETRIEVE_UPDATE_SERVICE_TYPE = "C0B39409-6FA4-42B9-9916-15CD14DDC2F6";

		// NOTE: We do not have ANY implementation regarding DIDD Terminations!  Waiting on spec for these.



		#region "Public Methods"

		override public void ExecuteJob()
		{
			// Default Job - MMIS DCS Retrieve Update Provider
            this.ExecuteJob(Guid.Parse(JOB_ID_MMIS_DIDD_RETRIEVE_UPDATE_PROVIDER));
		}


		override public void ExecuteJob(Guid jobId)
		{
			string jobGuid = jobId.ToString().ToUpper();
			int transactionType = 0;

			ITask task = null;

			switch (jobGuid)
			{
				/* DIDD */
				case JOB_ID_MMIS_DIDD_SUBMIT_UPDATE_PROVIDER:
					transactionType = Constants.TransactionType.SendDIDDAddProviderUpdatetoMMIS;
					task = CreateTaskForSubmitUpdateProvider(transactionType);

					break;
				case JOB_ID_MMIS_DIDD_RETRIEVE_UPDATE_PROVIDER:
					transactionType = Constants.TransactionType.SendDIDDAddProviderUpdatetoMMIS;
					task = CreateTaskForRetrieveUpdateProvider(transactionType);
					
					break;
                case JOB_ID_MMIS_WAIVER_SUBMIT_UPDATE_SERVICE_TYPE:
                    transactionType = Constants.TransactionType.SendWaiverServicesUpdatetoMMIS;
                    task = CreateTaskForSubmitUpdateServiceType(transactionType);

                    break;
                case JOB_ID_MMIS_WAIVER_RETRIEVE_UPDATE_SERVICE_TYPE:
                    transactionType = Constants.TransactionType.SendWaiverServicesUpdatetoMMIS;
                    task = CreateTaskForRetrieveUpdateServiceType(transactionType);

                    break;
            }

			if (task != null)
			{
				TaskResults = task.Run();
			}
		}

		private ITask CreateTaskForSubmitUpdateProvider(int transactionType)
		{
			var populateIndividuals = new MMISTasks.PopulateStagingTables(ThreadId, "Populate Individual Staging Tables", transactionType);
			populateIndividuals.PopulateStagingTablesStep = new MMISPopulateIndividualStagingTables(ThreadId);

			var populateGroups = new MMISTasks.PopulateStagingTables(ThreadId, "Populate Group Staging Tables", transactionType);
			populateGroups.PopulateStagingTablesStep = new MMISPopulateGroupStagingTables(ThreadId);

			var individualSubmitTask = new MMISTasks.ExecuteTransaction<SubmitWaiverProvider>(ThreadId, "Submit Individuals", transactionType, MMISTasks.TransactionDirection.Submit);
			
			individualSubmitTask.LoadDataSetStep = new MMISLoadDataSet.IndividualPendingStagingRecords(ThreadId, transactionType);
				/// foreach() all records in the Dataset
                individualSubmitTask.GetFilePathStep = new InterfaceFile.SubmitFilePath(ThreadId);
                individualSubmitTask.PopulateRequestStep = new MMISPopulateRequest.IndividualSubmit.Populate(ThreadId);
				individualSubmitTask.UpdateTransactionStep = new MMISUpdateTransaction.ByTransactionIDSubmitted(ThreadId);
                individualSubmitTask.PostProcessSteps.Add(new MMISPostProcess.IndividualSubmit.UpdateStagingAndProvider(ThreadId));

                var groupSubmitTask = new MMISTasks.ExecuteTransaction<SubmitWaiverProvider>(ThreadId, "Submit Groups", transactionType, MMISTasks.TransactionDirection.Submit);
                groupSubmitTask.GetFilePathStep = new InterfaceFile.SubmitFilePath(ThreadId);
                groupSubmitTask.LoadDataSetStep = new MMISLoadDataSet.GroupStagingRecords(ThreadId, transactionType, false);
			    groupSubmitTask.PopulateRequestStep = new MMISPopulateRequest.GroupSubmit.Populate(ThreadId);
			    groupSubmitTask.UpdateTransactionStep = new MMISUpdateTransaction.ByPartyIDSubmitted(ThreadId);
                groupSubmitTask.PostProcessSteps.Add(new MMISPostProcess.GroupSubmit.UpdateStagingAndProvider(ThreadId));
                groupSubmitTask.SendFileStep = new InterfaceFile.SubmitProviderFile(ThreadId);

			populateIndividuals.SetNextTask(populateGroups);
			populateGroups.SetNextTask(individualSubmitTask);
			individualSubmitTask.SetNextTask(groupSubmitTask);

			return populateIndividuals;
		}

		private ITask CreateTaskForRetrieveUpdateProvider(int transactionType)
		{
            var individualRetrieveTask = new MMISTasks.MMISExecuteRetrieveTask<RetrieveWaiverProvider>(ThreadId, "Retrieve Individuals", transactionType);

            individualRetrieveTask.LoadDataSetStep = new MMISLoadDataSet.IndividualSubmittedStagingRecords(ThreadId, transactionType);
            individualRetrieveTask.RetrieveProviderRecordsStep = new InterfaceFile.RetrieveIndividualProviderRecords(ThreadId);
			individualRetrieveTask.PopulateRequestStep = new MMISPopulateRequest.IndividualRetrieve.Populate(ThreadId);
            individualRetrieveTask.ProcessResponseStep = new MMISProcess.IndividualRetrieve.ProcessProvider(ThreadId);
			individualRetrieveTask.UpdateTransactionStep = new MMISUpdateTransaction.ByTransactionIDProcessed(ThreadId);
            individualRetrieveTask.ProcessErrorStep = new MMISProcessError.IndividualRetrieve.ProcessError(ThreadId);
            individualRetrieveTask.PostProcessSteps.Add(new MMISPostProcess.IndividualRetrieve.UpdateStagingAndProvider(ThreadId));

            var groupRetrieveTask = new MMISTasks.MMISExecuteRetrieveTask<RetrieveWaiverProvider>(ThreadId, "Retrieve Groups", transactionType);
            groupRetrieveTask.LoadDataSetStep = new MMISLoadDataSet.GroupStagingRecords(ThreadId, transactionType, true);
            groupRetrieveTask.RetrieveProviderRecordsStep = new InterfaceFile.RetrieveProviderRecords(ThreadId);
            groupRetrieveTask.PopulateRequestStep = new MMISPopulateRequest.GroupRetrieve.Populate(ThreadId);
            groupRetrieveTask.ProcessResponseStep = new MMISProcess.GroupRetrieve.ProcessProvider(ThreadId);
            groupRetrieveTask.UpdateTransactionStep = new MMISUpdateTransaction.ByTransactionIDProcessed(ThreadId);
            groupRetrieveTask.ProcessErrorStep = new MMISProcessError.GroupRetrieve.ProcessError(ThreadId);
            groupRetrieveTask.PostProcessSteps.Add(new MMISPostProcess.GroupRetrieve.UpdateStagingAndProvider(ThreadId));

			individualRetrieveTask.SetNextTask(groupRetrieveTask);

			return individualRetrieveTask;
		}

        private ITask CreateTaskForSubmitUpdateServiceType(int transactionType)
        {
            var populatewaiverService = new MMISTasks.PopulateStagingTables(ThreadId, "Populate Waiver Service Staging Tables", transactionType);
            populatewaiverService.PopulateStagingTablesStep = new MMISPopulatesWaiverServiceStagingTables(ThreadId);

            var waiverServiceSubmitTask = new MMISTasks.ExecuteTransaction<SubmitWaiverServices>(ThreadId, "Submit Waiver Services", transactionType, MMISTasks.TransactionDirection.Submit);

            waiverServiceSubmitTask.LoadDataSetStep = new MMISLoadDataSet.WaiverServicesStagingRecords(ThreadId, transactionType, false);
            /// foreach() all records in the Dataset
            waiverServiceSubmitTask.GetFilePathStep = new InterfaceFile.SubmitWaiverServiceFilePath(ThreadId);
            waiverServiceSubmitTask.PopulateRequestStep = new MMISPopulateRequest.WaiverServicesSubmit.Populate(ThreadId);
            waiverServiceSubmitTask.UpdateTransactionStep = new MMISUpdateTransaction.ByTransactionIDSubmitted(ThreadId);
            waiverServiceSubmitTask.PostProcessSteps.Add(new MMISPostProcess.WaiverServicesSubmit.UpdateStagingAndProvider(ThreadId));
            waiverServiceSubmitTask.SendFileStep = new InterfaceFile.SubmitProviderFile(ThreadId);

            populatewaiverService.SetNextTask(waiverServiceSubmitTask);

            return populatewaiverService;
        }

        private ITask CreateTaskForRetrieveUpdateServiceType(int transactionType)
        {
            var waiverServicesRetrieveTask = new MMISTasks.MMISExecuteRetrieveTask<RetrieveWaiverServices>(ThreadId, "Retrieve waiver Services", transactionType);

            waiverServicesRetrieveTask.LoadDataSetStep = new MMISLoadDataSet.WaiverServicesStagingRecords(ThreadId, transactionType, true);
            waiverServicesRetrieveTask.RetrieveProviderRecordsStep = new InterfaceFile.RetrieveWaiverServiceRecords(ThreadId);
            waiverServicesRetrieveTask.PopulateRequestStep = new MMISPopulateRequest.WaiverServicesRetrieve.Populate(ThreadId);
            waiverServicesRetrieveTask.ProcessResponseStep = new MMISProcess.WaiverServicesRetrieve.ProcessProvider(ThreadId);
            waiverServicesRetrieveTask.UpdateTransactionStep = new MMISUpdateTransaction.ByTransactionIDProcessed(ThreadId);
            waiverServicesRetrieveTask.PostProcessSteps.Add(new MMISPostProcess.WaiverServicesRetrieve.UpdateStagingAndProvider(ThreadId));

            return waiverServicesRetrieveTask;
        }
		#endregion
	}
}
