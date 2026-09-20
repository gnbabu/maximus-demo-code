using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileProcessorCore
{
	public enum DataType
	{
		Integer,
		String,
		DateMMDDYYYY,
		DateYYYYMMDD,
		DateDDMMYY,
		Time,
		Currency,
		Filler,
		DateMMDDYYYYSlash,
		DateMMDDYYSlash,
		DateYYYYMMDDDash,
		SSN,
		PhoneNumber,
		Lookup,
		Composite
	}

	public class TemplateField : IFixedDelimitedTemplateField
	{
		public bool Required { get; set; }

		public DataType Type { get; set; }

		public int StartIndex { get; set; }
		
		public int EndIndex { get; set; }

		public int Length
		{
			get
			{
				return EndIndex - StartIndex + 1;
			}
		}

		public string FieldName { get; set; }

		public string TestValue { get; set; }

		public string ConditionalRequiredExpression { get; set; }

		public bool ConditionallyRequired
		{
			get
			{
				return !string.IsNullOrWhiteSpace(ConditionalRequiredExpression);
			}
		}

		public IEnumerable<string> ValidValues { get; set; }

		public Regex ValidRegex { get; set; }

		public int RandomNumberDigits { get; set; }

		public string TestRandomNumberFormat { get; set; }

		public bool Ignore { get; set; }

		public bool NoTrim { get; set; }

		public string Format { get; set; }

		public string LookupLabel { get; set; }

		public string LookupType { get; set; }

		public TemplateField(bool required, DataType dataType, int startIndex, int endIndex, string fieldName, 
			string testValue, string conditionalRequiredExpression, string validValues, string validRegex, int randomNumberDigits,
			string testRandomNumberFormat, bool ignore, bool noTrim, string format, string lookupLabel, string lookupType)
		{
			Required = required;
			Type = dataType;
			StartIndex = startIndex;
			EndIndex = endIndex;
			FieldName = fieldName;
			TestValue = testValue;
			ConditionalRequiredExpression = conditionalRequiredExpression;
			RandomNumberDigits = randomNumberDigits;
			TestRandomNumberFormat = testRandomNumberFormat;
			Ignore = ignore;
			NoTrim = noTrim;
			Format = format;
			LookupLabel = lookupLabel;
			LookupType = lookupType;

			if (!string.IsNullOrWhiteSpace(validValues))
			{
				ValidValues = validValues.Split(',').Select(p => p.Trim('\''));
			}

			if (!string.IsNullOrEmpty(validRegex))
			{
				ValidRegex = new Regex(validRegex, RegexOptions.Compiled);
			}
		}
	}
}
