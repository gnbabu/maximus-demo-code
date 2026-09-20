using System;

namespace MAXIMUS.DataExchange.PDMS
{
	public abstract class MMISTransactionStep
	{
		#region Properties

		public Guid ThreadID { get; set; }

		#endregion

		#region Constructors

		public MMISTransactionStep(Guid threadID)
		{
			this.ThreadID = threadID;
		}

		#endregion
	}
}
