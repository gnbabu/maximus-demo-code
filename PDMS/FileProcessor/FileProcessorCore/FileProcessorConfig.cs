using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace FileProcessorCore
{
	public enum FileFormat
	{
		FixedLength,
		CSV,
		XML
	}

	public enum TemplateFormat
	{
		Template,
		BCPFormatFile
	}

	public class FileProcessorConfig
	{
		private static XmlSerializer _serializer;

		private static XmlSerializer Serializer
		{
			get
			{
				if (_serializer == null)
				{
					_serializer = new XmlSerializer(typeof(FileProcessorConfig));
				}

				return _serializer;
			}
		}

		public static FileProcessorConfig ParseConfig(string path)
		{
			FileProcessorConfig config;

			if (!File.Exists(path))
			{
				throw new ArgumentException(string.Format("File not found: {0}", path));
			}

			using(StreamReader sr = new StreamReader(path))
			{
				config =  (FileProcessorConfig)Serializer.Deserialize(sr);
			}

			if (config == null)
			{
				throw new Exception("Error de-serializing FileProcessorConfig");
			}
			return config;
		}

		[XmlArray("Folders")]
		public List<Folder> Folders { get; set; }


		public class Folder
		{
			[XmlAttribute("path")]
			public string Path { get; set; }

			[XmlAttribute("templateFile")]
			public string TemplateFile { get; set; }

			[XmlAttribute("archiveFolder")]
			public string ArchiveFolder { get; set; }

			[XmlAttribute("errorFolder")]
			public string ErrorFolder { get; set; }
		}
	}


}
