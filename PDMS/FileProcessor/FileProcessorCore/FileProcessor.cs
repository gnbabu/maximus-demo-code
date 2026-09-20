using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;
using System.Configuration;
using System.IO;

using FileProcessorCore;

namespace FileProcessorCore
{
	public abstract class FileProcessor
	{
		public FileProcessor()
		{
			
		}

		protected virtual void ParseExtraXmlElements(XElement root)
		{
		
		}
	}
}
