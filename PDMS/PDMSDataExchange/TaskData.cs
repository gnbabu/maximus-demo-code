using System;
using System.Collections.Generic;

namespace MAXIMUS.DataExchange.PDMS
{
	public class TaskData
	{
		public string TaskName { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public bool Successful { get; set; }
		public Dictionary<string, string> ResultBag { get; set; }
		public List<TransactionData> Transactions { get; set; }
		
		public TaskData(string taskName)
		{
			TaskName = taskName;
			ResultBag = new Dictionary<string, string>();
			Transactions = new List<TransactionData>();
		}
	}

	public class TransactionData
	{
		public int PrimaryKey { get; set; }
		public int? TransactionQueueID { get; set; }
		public int PartyID { get; set; }
		public int? MedicaidPK { get; set; }
		public string SakTransID { get; set; }
		public int TransactionTypeID { get; set; }
		public int PDMSStatus { get; set; }
        public string StatusCode { get; set; }
	}
}
