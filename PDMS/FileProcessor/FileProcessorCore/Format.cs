using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessorCore
{
	public static class Format
	{
		public static string SSN(string val)
		{
			if (val != null && val.Length == 9)
			{
				return string.Format("{0}-{1}-{2}", val.Substring(0, 3), val.Substring(3, 2), val.Substring(5, 4));
			}
			else
			{
				return string.Empty;
			}
		}
	}
}
