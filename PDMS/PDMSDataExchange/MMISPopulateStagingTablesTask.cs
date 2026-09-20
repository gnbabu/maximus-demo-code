using MAXIMUS.Core.Libraries;
using System;
using System.Reflection;


namespace MAXIMUS.DataExchange.PDMS.MMISTasks
{
	public class PopulateStagingTables : BaseTask
	{
		#region Properties

		public Guid ThreadID { get; set; }
		public int TransactionTypeID { get; set; }

		#endregion

		#region Steps

		public IMMISPopulateStagingTablesStep PopulateStagingTablesStep { get; set; }

		#endregion

		#region Constructors

		public PopulateStagingTables(Guid threadID, int transactionTypeID)
			: this(threadID, "MMISPopulateStagingTablesTask", transactionTypeID)
		{

		}

		public PopulateStagingTables(Guid threadID, string name, int transactionTypeID) : base(name)
		{
			this.ThreadID = threadID;
			this.TransactionTypeID = transactionTypeID;
		}

		#endregion

		public override bool ExecuteTask(TaskData result)
		{
			// create log object
			string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
			Logging log = new Logging(this.ThreadID, logMsg);

			try
			{
				if (PopulateStagingTablesStep == null)
				{
					throw new Exception("PopulateStagingTablesStep property must be set.");
				}

				PopulateStagingTablesStep.PopulateStagingTables(this.TransactionTypeID);
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(this.ThreadID, ex);
			}

			return true;
		}
	}
}
