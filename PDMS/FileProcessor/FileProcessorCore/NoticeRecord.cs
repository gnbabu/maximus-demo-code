using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessorCore
{
	public class NoticeRecord : ImportRecord
	{
		public int NoticeTypeID { get; set; }
		public string NoticeTypeName { get; protected set; }
		public int NoticeID { get; protected set; }
		public NoticeData NoticeData { get; set; }

		public string EmployeeName { get; set; }
		public string EmployeeSSN { get; set; }
		public string EmployerFederalEIN { get; set; }
		public string Notes { get; set; }
		public bool IsPaperNotice { get; set; }
		public string CaseID { get; set; }
		public string FormNo { get; set; }

		public string NoticeText
		{
			get
			{
				/// Build XML:
				StringBuilder sbXML = new StringBuilder();

				sbXML.AppendLine("<DIC>");

				if (NoticeData != null)
				{
					NoticeData.AddToXml(sbXML);
				}

				foreach (var pair in Data)
				{
					if (!string.IsNullOrWhiteSpace(pair.Value))
					{
						sbXML.AppendLine(string.Format("<DI N=\"{0}\">{1}</DI>", pair.Key, System.Security.SecurityElement.Escape(pair.Value)));
					}
				}

				sbXML.AppendLine("</DIC>");

				return sbXML.ToString();
			}
		}

		public NoticeRecord(LogFile log, int noticeTypeID, string noticeTypeName, string filename, int lineNumber)
			: base(log, filename, lineNumber)
		{
			NoticeTypeID = noticeTypeID;
			NoticeTypeName = noticeTypeName;
		}
	}
}
