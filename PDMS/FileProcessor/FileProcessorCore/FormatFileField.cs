using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessorCore
{
	public class FormatFileField : IFixedDelimitedTemplateField, ICSVTemplateField
	{
		#region Properties
		
		public DataType Type { get; set; }

		public int Length { get; set; }

		public string FieldName { get; set; }

		public int Index { get; set; }

		#endregion

		#region Constructors

		public FormatFileField(string fieldName, int index, int length, DataType dataType)
		{
			FieldName = fieldName;
			Index = index;
			Length = length;
			Type = dataType;
		}

		#endregion

		
	}
}
