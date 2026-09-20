using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace FileProcessorCore
{
	public static class Parse
	{
		private static Regex SSN_REGEX = new Regex(@"^\d{3}-\d{2}-\d{4}$", RegexOptions.Compiled);



		public static string SSN(string val)
		{
			if (SSN_REGEX.IsMatch(val))
			{
				return val.Replace("-", string.Empty);
			}
			else
			{
				return string.Empty;
			}
		}

		public static string PhoneNumber(string val)
		{
			return val.Replace("-", string.Empty).Replace(" ", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);
			
		}
	}
}
