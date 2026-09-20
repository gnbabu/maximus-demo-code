using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace FileProcessorCore
{
	public abstract class LineRecordExportLineProcessor : LineRecordLineProcessor
	{
		#region Constructor

		public LineRecordExportLineProcessor(LogFile log)
			: base(log, null)
		{

		}

		#endregion


		public abstract string GenerateFilename(string pickupFolderPath);

		public virtual void PreProcessRecord(Record record)
		{

		}

		public string FilenameTemplate
		{
			get;
			set;
		}

		public string HeaderLine { get; set; }

		protected override void ParseExtraXmlElements(XElement root)
		{
			FilenameTemplate = root.GetAttributeValue<string>("filenametemplate");
			
			XElement header = root.Element("header");

			if (header != null)
			{
				HeaderLine = header.Value;
			}

			base.ParseExtraXmlElements(root);
		}


		public void WriteRecordToFile(StreamWriter writer, Record record)
		{
			bool hasError = false;

			if (TemplateFormat == FileProcessorCore.TemplateFormat.Template)
			{
				foreach (var field in TemplateFields)
				{
					try
					{
						if (field.Type == DataType.Filler || field.Ignore)
						{
							writer.Write(new string(' ', field.Length));
						}
						else
						{
							string data = string.Empty;

							/// Check for concatenation field
							if (field.FieldName.Contains("+"))
							{
								string[] fieldNames = field.FieldName.Split('+');

								data = string.Empty;

								foreach (string f in fieldNames)
								{
									if (record.Data.ContainsKey(f))
									{
										if (string.IsNullOrEmpty(data))
										{
											data = record.Data[f];
										}
										else
										{
											data = string.Format("{0} {1}", data, record.Data[f]);
										}
									}
								}
							}
							else
							{
								if (record.Data.ContainsKey(field.FieldName))
								{
									data = record.Data[field.FieldName];
								}
							}

							if (!string.IsNullOrWhiteSpace(data))
							{
								if (data.Length > field.Length)
								{
									data = data.Substring(0, field.Length);
									//throw new InvalidDataException(string.Format("Value for field '{0}', '{1}', is greater than the maximum field length of {2}.",
									// field.FieldName, data, field.Length));
								}

								if (field.Type == DataType.DateMMDDYYYY && !string.IsNullOrWhiteSpace(data))
								{
									data = DateTime.ParseExact(data, "yyyyMMdd", null).ToString("MMddyyyy");
								}
								else if (field.Type == DataType.DateYYYYMMDD && !string.IsNullOrWhiteSpace(data))
								{
									data = DateTime.ParseExact(data, "yyyyMMdd", null).ToString("yyyyMMdd");
								}
								else if (field.Type == DataType.DateDDMMYY && !string.IsNullOrWhiteSpace(data))
								{
									data = DateTime.ParseExact(data, "yyyyMMdd", null).ToString("dd/MM/yy");
								}
								else if (field.Type == DataType.DateMMDDYYYYSlash && !string.IsNullOrWhiteSpace(data))
								{
									data = DateTime.ParseExact(data, "yyyyMMdd", null).ToString("MM/dd/yyyy");
								}
								else if (field.Type == DataType.DateMMDDYYSlash && !string.IsNullOrWhiteSpace(data))
								{
									data = DateTime.ParseExact(data, "yyyyMMdd", null).ToString("MM/dd/yy");
								}
								else if (field.Type == DataType.DateYYYYMMDDDash && !string.IsNullOrWhiteSpace(data))
								{
									data = DateTime.ParseExact(data, "yyyyMMdd", null).ToString("yyyy-MM-dd");
								}
								else if (field.Type == DataType.SSN && !string.IsNullOrWhiteSpace(data))
								{
									data = Format.SSN(data);
								}
								else if (field.Type == DataType.Currency)
								{
									if (string.IsNullOrWhiteSpace(data))
									{
										data = new string('0', field.Length);
									}
									else
									{
										data = (double.Parse(data) * 100).ToString().PadLeft(field.Length, '0');
									}
								}
							}

							writer.Write(data.PadRight(field.Length));
						}
					}
					catch (Exception ex)
					{
						Log.WriteError(string.Format("Error writing field '{0}': '{1}'", field.FieldName, ex.Message));
						hasError = true;
					}

				}
			}
			else if (TemplateFormat == FileProcessorCore.TemplateFormat.BCPFormatFile)
			{
				foreach (var field in FormatFileFields)
				{
					string data = string.Empty;
					if (record.Data.ContainsKey(field.FieldName))
					{
						data = record.Data[field.FieldName];
					}

					if (!string.IsNullOrWhiteSpace(data))
					{
						if (data.Length > field.Length)
						{
							data = data.Substring(0, field.Length);
							//throw new InvalidDataException(string.Format("Value for field '{0}', '{1}', is greater than the maximum field length of {2}.",
							// field.FieldName, data, field.Length));
						}
					}

					/// TOOD: Add support for more data types when needed, similar to above for TemplateFields. FOr now only supporting String.
					writer.Write(data.PadRight(field.Length));
				}
			}

			PostProcessRecord(record, hasError);
			
			
			if (hasError)
			{
				throw new Exception("Error processing output file.");
			}

		}

		protected abstract void PostProcessRecord(Record record, bool hasError);
	}
}
