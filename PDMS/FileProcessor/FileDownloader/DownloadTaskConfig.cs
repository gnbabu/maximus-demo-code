using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace FileDownloader
{
	public class DownloadTaskConfig
	{
		public DownloadTaskConfig()
		{
			Tasks = new List<DownloadTask>();
		}

		private static XmlSerializer _serializer;

		private static XmlSerializer Serializer
		{
			get
			{
				if (_serializer == null)
				{
					_serializer = new XmlSerializer(typeof(DownloadTaskConfig));
				}

				return _serializer;
			}
		}

		public static DownloadTaskConfig ParseConfig(string path)
		{
			DownloadTaskConfig config;

			if (!File.Exists(path))
			{
				throw new ArgumentException(string.Format("File not found: {0}", path));
			}

			using (StreamReader sr = new StreamReader(path))
			{
				config = (DownloadTaskConfig)Serializer.Deserialize(sr);
			}

			if (config == null)
			{
				throw new Exception("Error de-serializing FileProcessorConfig");
			}
			return config;
		}

		public void Save(string path)
		{
			using (StreamWriter sw = new StreamWriter(path))
			{
				Serializer.Serialize(sw, this);
			}
		}


		[XmlArray("Tasks")]
		public List<DownloadTask> Tasks { get; set; }
	}
}
