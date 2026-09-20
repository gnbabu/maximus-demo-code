using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace FileProcessorCore
{
	public static class XmlUtility
	{
		public static void XmlColumnToDictionary(Dictionary<string, string> dict, XmlReader noticeResponseReader)
		{
			while (noticeResponseReader.Read())
			{
				if (noticeResponseReader.NodeType == XmlNodeType.Element)
				{
					string key = null;
					try
					{
						key = noticeResponseReader.LocalName;
						if (("DI").Equals(key))
						{
							key = noticeResponseReader.GetAttribute("N");
							if (!noticeResponseReader.IsEmptyElement)
							{
								noticeResponseReader.Read();
							}
							dict[key] = noticeResponseReader.Value;
						}
					}
					catch (Exception ex)
					{
						throw new Exception(string.Format("Error adding key '{0}': {1}", key, ex.Message), ex);
					}
				}
			}
		}
	}
}
