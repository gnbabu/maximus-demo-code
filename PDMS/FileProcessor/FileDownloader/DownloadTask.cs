using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace FileDownloader
{
	public class DownloadTask
	{
		private DateTime _formatDate;

		private static Regex FormatRegex = new Regex(@"\{.+?\}", RegexOptions.Compiled);

		[XmlElement("name")]
		public string Name { get; set; }
		
		[XmlElement("urlPattern")]
		public string UrlPattern { get; set; }

		[XmlElement("seedStartValue")]
		public int? SeedStartValue { get; set; }

		[XmlElement("seedStartDate")]
		public DateTime? SeedStartDate { get; set; }

		[XmlElement("seedIncrement")]
		public int? SeedIncrement { get; set; }

		[XmlElement("yearOffset")]
		public int? YearOffset { get; set; }

		[XmlElement("monthOffset")]
		public int? MonthOffset { get; set; }

		[XmlElement("dayOffset")]
		public int? DayOffset { get; set; }


		public string FormatUrl(DateTime dt)
		{
			if (string.IsNullOrEmpty(UrlPattern))
			{
				return string.Empty;
			}
			_formatDate = dt;

			if (YearOffset.HasValue)
			{
				_formatDate = _formatDate.AddYears(YearOffset.Value);
			}

			if (MonthOffset.HasValue)
			{
				_formatDate = _formatDate.AddMonths(MonthOffset.Value);
			}

			if (DayOffset.HasValue)
			{
				_formatDate = _formatDate.AddDays(DayOffset.Value);
			}

			return FormatRegex.Replace(UrlPattern, new MatchEvaluator(EvalMatch));
		}

		private string EvalMatch(Match m)
		{
			if (m.Value == "{Seed}")
			{
				if (!SeedStartValue.HasValue || !SeedStartDate.HasValue || !SeedIncrement.HasValue)
				{
					return string.Empty;
				}
				else
				{
					return (SeedStartValue.Value + ((_formatDate.Date - SeedStartDate.Value.Date).TotalDays * SeedIncrement.Value)).ToString();
				}
			}
			else
			{
				/// Assume part of a date
				return _formatDate.ToString(m.Value.Replace("{", string.Empty).Replace("}", string.Empty));
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
