using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileProcessorCore;

namespace MIPFileProcessor
{
	/// <summary>
	/// Custom ImportRecord class with overridden ID property.
	/// </summary>
	public class MMISProviderUpdateRecord : ImportRecord
	{
		#region Constructors

		public MMISProviderUpdateRecord(LogFile log, string filename, int lineNumber)
			: base(log, filename, lineNumber)
		{
			
		}

		#endregion

		public override string ID
		{
			get
			{
				return Data["ProviderNumber"];
			}
			set
			{
				Data["ProviderNumber"] = value;
			}
		}
	}
}
