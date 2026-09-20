using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace FileProcessorCore
{
	public abstract class LineRecordImportLineProcessor : LineRecordLineProcessor
	{
		#region Static Members

		private static Regex COMPOSITE_FIELD_REGEX = new Regex(@"{(?<key>\w+)}", RegexOptions.Compiled);
		private static Regex FILTER_EXPRESSION_REGEX = new Regex(@"(?<fieldName>\w+)=(?<values>.*)");
		#endregion

		#region Properties

		private string _filterExpressionDataKey;

		public string FilterExpressionDataKey
		{
			get
			{
				if (_filterExpressionDataKey == null)
				{
					Match match = FILTER_EXPRESSION_REGEX.Match(FilterExpression);

					if (match.Success && match.Groups["fieldName"] != null && !string.IsNullOrEmpty(match.Groups["fieldName"].Value))
					{
						_filterExpressionDataKey = match.Groups["fieldName"].Value;
					}
					else
					{
						_filterExpressionDataKey = string.Empty;
					}
				}

				return _filterExpressionDataKey;
			}
		}

		private List<string> _filterExpressionValidValues;

		public List<string> FilterExpressionValidValues
		{
			get
			{
				if (_filterExpressionValidValues == null)
				{
					_filterExpressionValidValues = new List<string>();

					Regex reg = new Regex(@"(?<fieldName>\w+)=(?<values>.*)");

					Match match = reg.Match(FilterExpression);

					if (match.Success && match.Groups["values"] != null && !string.IsNullOrEmpty(match.Groups["values"].Value))
					{
						string values = match.Groups["values"].Value;

						Regex valReg = new Regex(@"'(?<value>\w+)'\|?");

						MatchCollection matches = valReg.Matches(values);

						foreach (Match m in matches)
						{
							if (m.Success && m.Groups["value"] != null && !string.IsNullOrEmpty(m.Groups["value"].Value))
							{
								_filterExpressionValidValues.Add(m.Groups["value"].Value);
							}
						}
					}
				}

				return _filterExpressionValidValues;
			}
		}

		public string TestFilenameTemplate { get; set; }

		public string FilterExpression
		{
			get;
			set;
		}

		public Regex FilenamePattern
		{
			get;
			set;
		}

		public FileFormat Format { get; set; }

		public bool IgnoreHeader { get; set; }

        public int LineLength { get; set; }

        public string InsertStoredProc { get; set; }

		#endregion

		#region Constructor

		public LineRecordImportLineProcessor(LogFile log, string templateName, FileFormat format)
			: base(log, templateName)
		{
			Format = format;
		}

		#endregion


		protected override void ParseExtraXmlElements(XElement root)
		{
			FilterExpression = root.GetAttributeValue<string>("filterexp");
			string filenamePattern = root.GetAttributeValue<string>("filenamepattern");
			if (!string.IsNullOrEmpty(filenamePattern))
			{
				FilenamePattern = new Regex(filenamePattern);
			}

            TestFilenameTemplate = root.GetAttributeValue<string>("testfilenametemplate");

            LineLength = root.GetAttributeValue<int>("lineLength");

            InsertStoredProc = root.GetAttributeValue<string>("insertStoredProc");

			IgnoreHeader = root.GetAttributeValue<bool>("ignoreHeader");

			base.ParseExtraXmlElements(root);
		}

		public bool IsValidRecord(Record record)
		{
			if (string.IsNullOrEmpty(FilterExpression))
			{
				return true;
			}
			else
			{
				string testValue = record.Data[FilterExpressionDataKey];

				return FilterExpressionValidValues.Contains(testValue);
			}
		}

		public abstract Record NewRecord(string workingFilePath, int lineNumber);

		public Record ProcessLine(string workingFile, int lineNumber, string text, string[] textArray)
		{
			Record record = NewRecord(Path.GetFileName(workingFile), lineNumber);

			int maxLineLength = TemplateFields.Max(t => t.EndIndex);
			if (text.Length < maxLineLength)
			{
				text = text.PadRight(maxLineLength);
			}

			if (Format == FileFormat.FixedLength)
			{
				foreach (var field in TemplateFields.Where(p => p.Type != DataType.Composite && p.Type != DataType.Filler))
				{
					try
					{
						string value = string.Empty;

						value = text.Substring(field.StartIndex, field.Length);
						
						if(!field.NoTrim)
						{	
							value = value.Trim();
						}

						AddDataField(record, field, value);

						if (field.Type == DataType.Lookup)
						{
							string lookupValue = string.Empty;
							if (!string.IsNullOrEmpty(value))
							{
								lookupValue = FindLookup(field.LookupType, value);
							}

							record.Data.Add(field.LookupLabel, lookupValue);
						}
					}
					catch (Exception ex)
					{
						throw new Exception(string.Format("Error parsing field '{0}': {1}", field.FieldName, ex.Message), ex);
					}
				}

				/// Process composite fields
				foreach (var field in TemplateFields.Where(p => p.Type == DataType.Composite))
				{
					string value = field.Format;

					MatchCollection matches = COMPOSITE_FIELD_REGEX.Matches(field.Format);

					foreach (Match match in matches)
					{
						value = value.Replace(match.Value, record.Data[match.Groups["key"].Value]);
					}

					AddDataField(record, field, value);
				}
			}
			else if (Format == FileFormat.CSV)
			{
				foreach (var field in FormatFileFields)
				{
					try
					{
						if (field.Type != DataType.Filler && field.Index > -1)
						{
							string value = string.Empty;

							value = textArray[field.Index];

							AddDataField(record, field, value);
						}
					}
					catch (Exception ex)
					{
						throw new Exception(string.Format("Error parsing field '{0}': {1}", field.FieldName, ex.Message), ex);
					}
				}
			}
		

			if (!IsValidRecord(record))
			{
				record.RecordFilteredOut = true;
				record.IsValid = false;
				return record;
			}

			/// Includes Employer information.
			try
			{
				AddCustomProperties(record);
			}
			catch (Exception ex)
			{ 
				record.IsValid = false;
				if (record.Errors == null)
				{
					record.Errors = new List<string>();
				}

				record.Errors.Add(string.Format("Error adding custom properties to record: {0}", ex));
				return record;
			}

			Log.WriteVerbose("Begin validating record...");

			List<string> lstErrors = Validate(record);

			Log.WriteVerbose("End validating record.");

			if (lstErrors.Count > 0)
			{
				record.FailedValidation = true;
				record.Errors = lstErrors;
				record.IsValid = false;
				return record;
			}
			else
			{

				bool result = SaveRecord(record);

				if (!result)
				{
					Log.WriteError("Error saving record.");
				}

				Log.WriteVerbose(string.Format("End saving record. ID: {0}.", record.ID));

				record.IsValid = result;
				return record;
			}
		}

		private string FindLookup(string lookupType, string value)
		{
			Lookup lookup = Lookups.FirstOrDefault(p => p.Name == lookupType);

			if (lookup == null)
			{
				throw new Exception(string.Format("Unrecognized lookuptype value: '{0}'", lookupType));
			}

			if (!lookup.Codes.ContainsKey(value)) 
			{
				///Normaly throw an exception, but for now 
				return string.Format("Unknown Lookup: {0}", value);
			//	throw new Exception(string.Format("Lookup code '{0}' not found in lookup with name '{1}'", value, lookupType));
			}

			return lookup.Codes[value];
		}

		private static void AddDataField(Record record, IFixedDelimitedTemplateField field, string value)
		{

			if (field.Type == DataType.DateYYYYMMDD && !string.IsNullOrWhiteSpace(value))
			{
				value = DateTime.ParseExact(value, "yyyyMMdd", null).ToString("yyyyMMdd");
			}
			else if (field.Type == DataType.DateMMDDYYYY && !string.IsNullOrWhiteSpace(value))
			{
				value = DateTime.ParseExact(value, "MMddyyyy", null).ToString("yyyyMMdd");
			}
			else if (field.Type == DataType.DateDDMMYY && !string.IsNullOrWhiteSpace(value))
			{
				value = DateTime.ParseExact(value, "dd/MM/yy", null).ToString("yyyyMMdd");
			}
			else if (field.Type == DataType.DateMMDDYYYYSlash && !string.IsNullOrWhiteSpace(value))
			{
				value = DateTime.ParseExact(value, "MM/dd/yyyy", null).ToString("yyyyMMdd");
			}
			else if (field.Type == DataType.DateMMDDYYSlash && !string.IsNullOrWhiteSpace(value))
			{
				value = DateTime.ParseExact(value, "MM/dd/yy", null).ToString("yyyyMMdd");
			}
			else if (field.Type == DataType.DateYYYYMMDDDash && !string.IsNullOrWhiteSpace(value))
			{
				value = DateTime.ParseExact(value, "yyyy-MM-dd", null).ToString("yyyyMMdd");
			}
			else if (field.Type == DataType.SSN && !string.IsNullOrWhiteSpace(value))
			{
				value = Parse.SSN(value);
			}
			else if (field.Type == DataType.PhoneNumber && !string.IsNullOrWhiteSpace(value))
			{
				value = Parse.PhoneNumber(value);
			}
			else if (field.Type == DataType.Currency)
			{
				value = (double.Parse(value) / 100.0).ToString();
			}

			record.Data.Add(field.FieldName, value);
		}


		protected virtual void AddCustomProperties(Record record)
		{

		}

		protected bool SaveRecord(Record record)
		{
			if (IsInsertRecord(record))
			{
				return InsertRecord(record);
			}
			else if (IsUpdateRecord(record))
			{
				return UpdateRecord(record);
			}
			else if (IsDeleteRecord(record))
			{
				return DeleteRecord(record);
			}
			else
			{
				Log.WriteVerbose("Record not saved.");
				return false;
			}
		}

		protected abstract bool IsInsertRecord(Record record);

		protected abstract bool InsertRecord(Record record);

		protected abstract bool IsUpdateRecord(Record record);

		protected abstract bool UpdateRecord(Record record);

		protected abstract bool IsDeleteRecord(Record record);

		protected abstract bool DeleteRecord(Record record);

		
	}
}
