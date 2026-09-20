using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessorCore
{
	public class Lookup
	{
		public string Name { get; set; }

		public Dictionary<string, string> Codes { get; set; }

		public Lookup(string name, Dictionary<string, string> codes)
		{
			Name = name;
			Codes = codes;
		}
	}
}
