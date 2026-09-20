using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;

namespace FileProcessorCore
{
	public class EWSConfiguration
	{


		private static Dictionary<string, string> _overloads = new Dictionary<string, string>();
		private const string CURRENT_DATE_TIME = "CurrentDateTime";
		private const string CURRENT_DATE = "CurrentDate";

		public static string AppSettings(string key)
		{
			if (_overloads.ContainsKey(key))
			{
				return _overloads[key];
			}

			return ConfigurationManager.AppSettings[key];
		}

		public static DateTime CurrentDateTime
		{
			get
			{
				if (_overloads.ContainsKey(CURRENT_DATE_TIME))
				{
					return DateTime.Parse(_overloads[CURRENT_DATE_TIME]);
				}

				return DateTime.Now;
			}
		}

		public static DateTime CurrentDate
		{
			get
			{
				if (_overloads.ContainsKey(CURRENT_DATE))
				{
					return DateTime.Parse(_overloads[CURRENT_DATE]);
				}

				return DateTime.Today;
			}
		}

		public static void OverrideConfig(string overrideConfigValues)
		{
			Regex configRegex = new Regex(@"(?<key>\w+)=(?<value>[^;]+)");

			var matches = configRegex.Matches(overrideConfigValues);

			foreach (Match match in matches)
			{
				_overloads[match.Groups["key"].Value] = match.Groups["value"].Value;
			}
		}
	}
}
