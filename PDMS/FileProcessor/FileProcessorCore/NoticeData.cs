using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessorCore
{
	public class NoticeData
	{
		public string EmployerContactName { get; set; }
		public string EmployerContactPhone { get; set; }
		public int? EmployerID { get; set; }
		public string EmployerName { get; set; }
		public string EmployerWebsite { get; set; }
		public string HealthPlanContactName { get; set; }
		public string HealthPlanContactPhone { get; set; }
		public string HealthPlanContactFax { get; set; }
		public string HealthPlanContactEmail { get; set; }
		public string HealthPlanCompanyName { get; set; }
		public string HealthPlanAddress1 { get; set; }
		public string HealthPlanAddress2 { get; set; }
		public string HealthPlanAddress3 { get; set; }
		public string HealthPlanAddressCity { get; set; }
		public string HealthPlanAddressState { get; set; }
		public string HealthPlanAddressPostalCodeFull { get; set; }
		public string EmployerContactEmail { get; set; }
		public string EmployerDoingBusinessAs { get; set; }
		public DateTime DT_Insert { get; set; }

		public void AddToXml(StringBuilder sbXML)
		{
			if (EmployerID.HasValue)
			{
				sbXML.AppendLine(string.Format("<DI N=\"EmployerID\">{0}</DI>", System.Security.SecurityElement.Escape(EmployerID.Value.ToString())));
			}

			if (!string.IsNullOrWhiteSpace(EmployerContactName))
			{
				sbXML.AppendLine(string.Format("<DI N=\"ER_CN\">{0}</DI>", System.Security.SecurityElement.Escape(EmployerContactName)));
			}

			if (!string.IsNullOrWhiteSpace(EmployerContactPhone))
			{
				sbXML.AppendLine(string.Format("<DI N=\"ER_CPH\">{0}</DI>", System.Security.SecurityElement.Escape(EmployerContactPhone)));
			}

			if (!string.IsNullOrWhiteSpace(EmployerContactEmail))
			{
				sbXML.AppendLine(string.Format("<DI N=\"ER_CEM\">{0}</DI>", System.Security.SecurityElement.Escape(EmployerContactEmail)));
			}

			if (!string.IsNullOrWhiteSpace(EmployerName))
			{
				sbXML.AppendLine(string.Format("<DI N=\"ER_N\">{0}</DI>", System.Security.SecurityElement.Escape(EmployerName)));
			}

			if (!string.IsNullOrWhiteSpace(EmployerWebsite))
			{
				sbXML.AppendLine(string.Format("<DI N=\"ER_Web\">{0}</DI>", System.Security.SecurityElement.Escape(EmployerWebsite)));
			}

			if (!string.IsNullOrWhiteSpace(HealthPlanContactName))
			{
				sbXML.AppendLine(string.Format("<DI N=\"HP_CN\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanContactName)));
			}

			if (!string.IsNullOrWhiteSpace(HealthPlanContactPhone))
			{
				sbXML.AppendLine(string.Format("<DI N=\"HP_CPH\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanContactPhone)));
			}

			if (!string.IsNullOrWhiteSpace(HealthPlanContactEmail))
			{
				sbXML.AppendLine(string.Format("<DI N=\"HP_CEM\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanContactEmail)));
			}

			//if (!string.IsNullOrWhiteSpace(HealthPlanContactFax))
			//{
			//	sbXML.AppendLine(string.Format("<DI N=\"HP_CFX\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanContactFax)));
			//}

			

			//if (!string.IsNullOrWhiteSpace(HealthPlanCompanyName))
			//{
			//	sbXML.AppendLine(string.Format("<DI N=\"HP_Company\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanCompanyName)));
			//}

			//if (!string.IsNullOrWhiteSpace(HealthPlanAddress1))
			//{
			//	sbXML.AppendLine(string.Format("<DI N=\"HP_A1\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanAddress1)));
			//}

			//if (!string.IsNullOrWhiteSpace(HealthPlanAddress2))
			//{
			//	sbXML.AppendLine(string.Format("<DI N=\"HP_A2\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanAddress2)));
			//}

			//if (!string.IsNullOrWhiteSpace(HealthPlanAddress3))
			//{
			//	sbXML.AppendLine(string.Format("<DI N=\"HP_A3\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanAddress3)));
			//}

			//if (!string.IsNullOrWhiteSpace(HealthPlanAddressCity))
			//{
			//	sbXML.AppendLine(string.Format("<DI N=\"HP_AC\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanAddressCity)));
			//}

			//if (!string.IsNullOrWhiteSpace(HealthPlanAddressState))
			//{
			//	sbXML.AppendLine(string.Format("<DI N=\"HP_AS\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanAddressState)));
			//}

			//if (!string.IsNullOrWhiteSpace(HealthPlanAddressPostalCodeFull))
			//{
			//	sbXML.AppendLine(string.Format("<DI N=\"HP_APC\">{0}</DI>", System.Security.SecurityElement.Escape(HealthPlanAddressPostalCodeFull)));
			//}

			

			//if (!string.IsNullOrWhiteSpace(EmployerDoingBusinessAs))
			//{
			//	sbXML.AppendLine(string.Format("<DI N=\"ER_DBA\">{0}</DI>", System.Security.SecurityElement.Escape(EmployerDoingBusinessAs)));
			//}

			sbXML.AppendLine(string.Format("<DI N=\"DT_Insert\">{0}</DI>", System.Security.SecurityElement.Escape(DT_Insert.ToString("yyyyMMdd"))));
		}
	}
}
