using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessorCore
{
	public interface ICSVTemplateField : ITemplateField
	{
		int Index { get; set; }
	}
}
