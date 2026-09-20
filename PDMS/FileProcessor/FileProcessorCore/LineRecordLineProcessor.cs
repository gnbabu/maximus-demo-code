using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FileProcessorCore
{
	public abstract class LineRecordLineProcessor
	{
		#region Properties

		public LogFile Log { get; set; }

		protected abstract string TemplateName
		{
			get;
		}

		public List<TemplateField> TemplateFields
		{
			get;
			set;
		}

		public List<FormatFileField> FormatFileFields
		{
			get;
			set;
		}

		public TemplateFormat TemplateFormat { get; set; }

		public List<Lookup> Lookups { get; set; }

		#endregion

		#region Constructor

		public LineRecordLineProcessor(LogFile log, string templateFileName)
		{
			Log = log;
			
			if (string.IsNullOrEmpty(templateFileName))
			{
				templateFileName = TemplateName;
			}
			/// Parse the xml file and set up the Template Fields
			Uri assemblyFolder = new Uri(Assembly.GetCallingAssembly().CodeBase);

			Uri configPathUri;
			if (Path.IsPathRooted(templateFileName))
			{
				configPathUri = new Uri(templateFileName);
			}
			else
			{
				configPathUri = new Uri(assemblyFolder, templateFileName);
			}

			XElement root = XElement.Load(configPathUri.AbsolutePath);

			

			/// Check if this is a normal Template file, or a FormatFile file from bcp.
			if (root.Name.LocalName == "template")
			{
				TemplateFormat = TemplateFormat.Template;
				ParseTemplateFields(templateFileName, root);
			}
			else if (root.Name.LocalName == "BCPFORMAT")
			{
				TemplateFormat = TemplateFormat.BCPFormatFile;
				ParseBCPFormatFields(templateFileName, root);
			}

			ParseLookupFields(templateFileName, root);

			ParseExtraXmlElements(root);
		}

		private void ParseBCPFormatFields(string templateFileName, XElement root)
		{
			FormatFileFields = new List<FormatFileField>();

			

			foreach (XElement field in root.Element(XName.Get("RECORD", @"http://schemas.microsoft.com/sqlserver/2004/bulkload/format")).Elements(XName.Get("FIELD", @"http://schemas.microsoft.com/sqlserver/2004/bulkload/format")))
			{
				int length = field.GetAttributeValue<int>("LENGTH");

				int csvIndex = -1;

				if (!string.IsNullOrEmpty(field.GetAttributeValue<string>("CSVINDEX")))
				{
					csvIndex = field.GetAttributeValue<int>("CSVINDEX");
				}
				
				
				string fieldName = field.GetAttributeValue<string>("ID");

				DataType dataType;

				string dataTypeString = field.GetAttributeValue<string>(XName.Get("type", "http://www.w3.org/2001/XMLSchema-instance"));

				switch (dataTypeString)
				{
					case "NativeFixed":
						dataType = DataType.String;
						break;
					default:
						throw new ArgumentException(string.Format("Unrecognized Data Type string in attribute 'xsi:type': {0}", dataTypeString));
				}

				FormatFileFields.Add(new FormatFileField(fieldName, csvIndex, length, dataType));
			}
		}

		private void ParseLookupFields(string templateFileName, XElement root)
		{
			Lookups = new List<Lookup>();

			foreach (XElement lookup in root.Element("lookups").Elements("lookup"))
			{
				string name = lookup.GetAttributeValue<string>("name");
				Dictionary<string, string> dictCodes = new Dictionary<string, string>();

				foreach (XElement codes in lookup.Elements("code"))
				{
					dictCodes[codes.GetAttributeValue<string>("key")] = codes.GetAttributeValue<string>("value");
				}

				Lookups.Add(new Lookup(name, dictCodes));
			}
		}

		private void ParseTemplateFields(string templateFileName, XElement root)
		{
			TemplateFields = new List<TemplateField>();

			int nextIndex = 0;
			foreach (XElement field in root.Element("fields").Elements("field"))
			{
				int startIndex = field.GetAttributeValue<int>("start") - 1;
				int endIndex = field.GetAttributeValue<int>("end") - 1;

				string fieldLabel = field.GetElementValue<string>("label");
				DataType dataType = (DataType)Enum.Parse(typeof(DataType), field.Attribute("type").Value);

				if(dataType != DataType.Composite)
				{
					if ( startIndex != nextIndex)
					{
						throw new Exception(string.Format("File: {0}. Invalid start index found {1}, Label '{2}'.", templateFileName, startIndex, fieldLabel));
					}

					nextIndex = endIndex + 1;
				}


				TemplateFields.Add(new TemplateField(
					field.GetAttributeValue<bool>("required"),
					dataType,
					startIndex,
					endIndex,
					fieldLabel,
					field.GetElementValue<string>("testvalue"),
					field.GetAttributeValue<string>("conditionalrequired"),
					field.GetAttributeValue<string>("validvalues"),
					field.GetAttributeValue<string>("validregex"),
					field.GetAttributeValue<int>("testrandomnumberdigits"),
					field.GetAttributeValue<string>("testrandomnumberformat"),
					field.GetAttributeValue<bool>("ignore"),
					field.GetAttributeValue<bool>("notrim"),
					field.GetElementValue<string>("format"),
					field.GetElementValue<string>("lookuplabel"),
					field.GetAttributeValue<string>("lookuptype"))
					);
			}
		}

		#endregion

		public List<string> Validate(Record record)
		{
			List<string> lstErrors = new List<string>();

			if (TemplateFields != null)
			{
				foreach (TemplateField field in TemplateFields)
				{
					try
					{
						if (field.Required)
						{
							if (string.IsNullOrWhiteSpace(record.Data[field.FieldName]))
							{
								lstErrors.Add(string.Format("Field {0} is required.", field.FieldName));
							}
						}

						if (field.ConditionallyRequired)
						{
							Regex reg = new Regex(@"(?<fieldName>\w+)='(?<value>\w+)'");

							Match match = reg.Match(field.ConditionalRequiredExpression);

							if (match != null && match.Success && match.Groups["fieldName"] != null && match.Groups["value"] != null)
							{
								string fieldName = match.Groups["fieldName"].Value;
								string value = match.Groups["value"].Value;

								if (record.Data[fieldName] == value && string.IsNullOrWhiteSpace(record.Data[field.FieldName]))
								{
									lstErrors.Add(string.Format("{0} is required when {1}", field.FieldName, field.ConditionalRequiredExpression));
								}
							}

							reg = new Regex(@"(?<fieldName>\w+)!='(?<value>\w+)'");

							match = reg.Match(field.ConditionalRequiredExpression);

							if (match != null && match.Success && match.Groups["fieldName"] != null && match.Groups["value"] != null)
							{
								string fieldName = match.Groups["fieldName"].Value;
								string value = match.Groups["value"].Value;

								if (record.Data[fieldName] != value && string.IsNullOrWhiteSpace(record.Data[field.FieldName]))
								{
									lstErrors.Add(string.Format("{0} is required when {1}", field.FieldName, field.ConditionalRequiredExpression));
								}
							}

							reg = new Regex(@"AnyFieldHasValue\((?<fieldNames>.*)\)");

							match = reg.Match(field.ConditionalRequiredExpression);

							if (match != null && match.Success && match.Groups["fieldNames"] != null)
							{
								string[] fieldNames = match.Groups["fieldNames"].Value.Split(',');

								if (string.IsNullOrWhiteSpace(record.Data[field.FieldName]) &&
									fieldNames.All(fieldName => !string.IsNullOrWhiteSpace(record.Data[fieldName])))
								{
									lstErrors.Add(string.Format("{0} is required when any of these fields have a value: {1}", field.FieldName,
										string.Join(",", fieldNames)));
								}
							}

						}

						if (field.ValidValues != null && !string.IsNullOrWhiteSpace(record.Data[field.FieldName]))
						{
							if (!field.ValidValues.Contains(record.Data[field.FieldName]))
							{
								lstErrors.Add(string.Format("{0} value '{1}' must be one of the following: {2}.",
									field.FieldName, record.Data[field.FieldName], string.Join(",", field.ValidValues)));
							}
						}

						if (field.ValidRegex != null)
						{
							if (!string.IsNullOrEmpty(record.Data[field.FieldName]) && !field.ValidRegex.IsMatch(record.Data[field.FieldName]))
							{
								lstErrors.Add(string.Format("{0} field value '{1}' failed validation regex '{2}'", field.FieldName, record.Data[field.FieldName], field.ValidRegex));
							}
						}
					}
					catch (Exception ex)
					{
						throw new Exception(string.Format("Error validating field '{0}': {1}", field.FieldName, ex.Message), ex);
					}
				}
			}

			return lstErrors;
		}


		
		protected virtual void ParseExtraXmlElements(XElement root)
		{

		}

	}
}
