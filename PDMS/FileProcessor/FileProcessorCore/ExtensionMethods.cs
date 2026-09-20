using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace FileProcessorCore
{
	public static class ExtensionMethods
	{
		public static T GetAttributeValue<T>(this XElement element, XName attributeName)
		{
			XAttribute attr = element.Attribute(attributeName);

			if (attr == null)
			{
				return default(T);
			}
			else
			{
				return (T)Convert.ChangeType(attr.Value, typeof(T));
			}
		}

		public static T GetElementValue<T>(this XElement element, XName attributeName)
		{
			XElement el = element.Element(attributeName);

			if (el == null)
			{
				return default(T);
			}
			else
			{
				return (T)Convert.ChangeType(el.Value, typeof(T));
			}
		}

		public static T GetValue<T>(this SqlDataReader reader, string columnName)
		{
			var temp = reader[columnName];

			if (temp == DBNull.Value)
			{
				return default(T);
			}
			else
			{
				return (T)temp;
			}
		}


		public static SqlParameter AddParameter(this SqlCommand command, string parameterName, SqlDbType dataType, object value)
		{
			SqlParameter parameter = new SqlParameter(parameterName, dataType);

			if (value != null)
			{
				parameter.Value = value;
			}
			else
			{
				parameter.Value = DBNull.Value;
			}

			command.Parameters.Add(parameter);

			return parameter;
		}

		public static SqlParameter AddParameter(this SqlCommand command, string parameterName, SqlDbType dataType, int size, object value)
		{
			SqlParameter parameter = new SqlParameter(parameterName, dataType, size);

			if (value != null)
			{
				parameter.Value = value;
			}
			else
			{
				parameter.Value = DBNull.Value;
			}

			command.Parameters.Add(parameter);

			return parameter;
		}

		public static SqlParameter AddParameter(this SqlCommand command, string parameterName, SqlDbType dataType, ParameterDirection direction, object value)
		{
			SqlParameter parameter = new SqlParameter(parameterName, dataType);
			parameter.Direction = direction;

			if (value != null)
			{
				parameter.Value = value;
			}
			else
			{
				parameter.Value = DBNull.Value;
			}

			command.Parameters.Add(parameter);

			return parameter;
		}

		public static SqlParameter AddParameter(this SqlCommand command, string parameterName, SqlDbType dataType, int size, ParameterDirection direction, object value)
		{
			SqlParameter parameter = new SqlParameter(parameterName, dataType, size);
			parameter.Direction = direction;

			if (value != null)
			{
				parameter.Value = value;
			}
			else
			{
				parameter.Value = DBNull.Value;
			}

			command.Parameters.Add(parameter);

			return parameter;
		}

		public static SqlParameter AddParameter(this SqlCommand command, string parameterName, SqlDbType dataType, ParameterDirection direction)
		{
			SqlParameter parameter = new SqlParameter(parameterName, dataType);
			parameter.Direction = direction;

			command.Parameters.Add(parameter);

			return parameter;
		}

		public static SqlParameter AddParameter(this SqlCommand command, string parameterName, SqlDbType dataType, int size, ParameterDirection direction)
		{
			SqlParameter parameter = new SqlParameter(parameterName, dataType, size);
			parameter.Direction = direction;

			command.Parameters.Add(parameter);

			return parameter;
		}

		public static SqlParameter AddRecordDataParameter(this SqlCommand command, string parameterName, SqlDbType dataType, Record record, string key)
		{
			SqlParameter parameter = new SqlParameter(parameterName, dataType);

			if (record != null && record.Data != null && record.Data.ContainsKey(key) && record.Data[key] != null)
			{
				if (dataType == SqlDbType.Date || dataType == SqlDbType.DateTime || dataType == SqlDbType.DateTime2 || dataType == SqlDbType.SmallDateTime || dataType == SqlDbType.Time || dataType == SqlDbType.Timestamp)
				{
					if (!string.IsNullOrEmpty(record.Data[key]))
					{
						parameter.Value = DateTime.ParseExact(record.Data[key], "yyyyMMdd", null);
					}
					else
					{
						parameter.Value = DBNull.Value;
					}
				}
				else
				{
					parameter.Value = record.Data[key];
				}
			}
			else
			{
				parameter.Value = DBNull.Value;
			}

			command.Parameters.Add(parameter);

			return parameter;
		}

		public static SqlParameter AddRecordDataParameter(this SqlCommand command, string parameterName, SqlDbType dataType, ParameterDirection direction, Record record, string key)
		{
			SqlParameter parameter = new SqlParameter(parameterName, dataType);
			parameter.Direction = direction;

			if (record != null && record.Data != null && record.Data.ContainsKey(key) && record.Data[key] != null)
			{
				if (dataType == SqlDbType.Date || dataType == SqlDbType.DateTime || dataType == SqlDbType.DateTime2 || dataType == SqlDbType.SmallDateTime || dataType == SqlDbType.Time || dataType == SqlDbType.Timestamp)
				{
					if (!string.IsNullOrEmpty(record.Data[key]))
					{
						parameter.Value = DateTime.ParseExact(record.Data[key], "yyyyMMdd", null);
					}
					else
					{
						parameter.Value = DBNull.Value;
					}
				}
				else
				{
					parameter.Value = record.Data[key];
				}
			}
			else
			{
				parameter.Value = DBNull.Value;
			}

			command.Parameters.Add(parameter);

			return parameter;
		}

		public static SqlParameter AddRecordDataParameter(this SqlCommand command, string parameterName, SqlDbType dataType, int size, Record record, string key)
		{
			SqlParameter parameter = new SqlParameter(parameterName, dataType, size);

			if (record != null && record.Data != null && record.Data.ContainsKey(key) && record.Data[key] != null)
			{
				if (dataType == SqlDbType.Date || dataType == SqlDbType.DateTime || dataType == SqlDbType.DateTime2 || dataType == SqlDbType.SmallDateTime || dataType == SqlDbType.Time || dataType == SqlDbType.Timestamp)
				{
					if (!string.IsNullOrEmpty(record.Data[key]))
					{
						parameter.Value = DateTime.ParseExact(record.Data[key], "yyyyMMdd", null);
					}
					else
					{
						parameter.Value = DBNull.Value;
					}
				}
				else
				{
					parameter.Value = record.Data[key];
				}
			}
			else
			{
				parameter.Value = DBNull.Value;
			}

			command.Parameters.Add(parameter);

			return parameter;
		}

		public static SqlParameter AddRecordDataParameter(this SqlCommand command, string parameterName, SqlDbType dataType, int size, ParameterDirection direction, Record record, string key)
		{
			SqlParameter parameter = new SqlParameter(parameterName, dataType, size);
			parameter.Direction = direction;

			if (record != null && record.Data != null && record.Data.ContainsKey(key) && record.Data[key] != null)
			{
				if (dataType == SqlDbType.Date || dataType == SqlDbType.DateTime || dataType == SqlDbType.DateTime2 || dataType == SqlDbType.SmallDateTime || dataType == SqlDbType.Time || dataType == SqlDbType.Timestamp)
				{
					if (!string.IsNullOrEmpty(record.Data[key]))
					{
						parameter.Value = DateTime.ParseExact(record.Data[key], "yyyyMMdd", null);
					}
					else
					{
						parameter.Value = DBNull.Value;
					}
				}
				else
				{
					parameter.Value = record.Data[key];
				}
			}
			else
			{
				parameter.Value = DBNull.Value;
			}

			command.Parameters.Add(parameter);

			return parameter;
		}
	}
}
