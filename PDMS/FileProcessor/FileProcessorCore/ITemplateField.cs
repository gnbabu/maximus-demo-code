using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessorCore
{
	public interface ITemplateField
	{
		string FieldName { get; }
		DataType Type { get; }
	}
}
