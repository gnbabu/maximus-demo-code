using MAXIMUS.Core.Libraries;
using Models.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;

namespace Models.Data
{
    public class Form1099 : IForm1099
	{
		[MatchParent("RegFormId")]
		public int RegFormId { get; set; }

		[MatchParent("AddressId")]
		public int AddressId { get; set; }

		[MatchParent("TaxTypeId")]
		public int TaxTypeId { get; set; }

		[MatchParent("TaxId")]
		public string TaxId { get; set; }

		[MatchParent("EffectiveDate")]
		public DateTime EffectiveDate { get; set; }

		[MatchParent("EndDate")]
		public DateTime EndDate { get; set; }

		[MatchParent("IsTaxExempt")]
		public bool IsTaxExempt { get; set; }

		[MatchParent("IsFormW9")]
		public bool IsFormW9 { get; set; }

		[MatchParent("IsForm147")]
		public bool IsForm147 { get; set; }


		private PDMSService.PDMSServiceClient _svc;

		private PDMSService.PDMSServiceClient svc
		{
			get { return _svc ?? (_svc = new PDMSService.PDMSServiceClient()); }
		}


		public void Load(DataRow row)
		{
			if (row == null)
			{
				RegFormId = 0;
				AddressId = 0;
				TaxTypeId = -1;
				TaxId = string.Empty;
				EffectiveDate = DateTime.Today;
				EndDate = new DateTime(2299, 12, 31);
				IsTaxExempt = false;
				IsFormW9 = false;
				IsForm147 = false;
			}
			else
			{
				RegFormId = Methods.GetIntValue(row, "REG_FORM_1099_INFO_ID");
				AddressId = Methods.GetIntValue(row, "REG_ADDRESS_ID");
				TaxTypeId = Methods.GetIntValue(row, "IRS_TAX_TYPE_ID");
				TaxId = Methods.GetString("IRS_TAX_ID", row);
				EffectiveDate = Methods.GetDateValue(row, "EFFECTIVE_DATE");
				EndDate = Methods.GetDateValue(row, "END_DATE") == DateTime.MinValue ? new DateTime(2299, 12, 31) : Methods.GetDateValue(row, "END_DATE");
				IsTaxExempt = Methods.GetBool("IS_TAX_EXEMPT", row);
				IsFormW9 = Methods.GetBool("IS_FORM_W9", row);
				IsForm147 = Methods.GetBool("IS_FORM_147", row);
			}
		}

		public Dictionary<string, string> CreateParameterList(Form1099 taxForm)
		{
			Dictionary<string, string> parms = new Dictionary<string, string>();
			parms.Add("REG_ADDRESS_ID", taxForm.AddressId.ToString());
			parms.Add("IRS_TAX_TYPE_ID", taxForm.TaxTypeId.ToString());
			parms.Add("IRS_TAX_ID", taxForm.TaxId);
			parms.Add("EFFECTIVE_DATE", taxForm.EffectiveDate.ToString("G"));
			parms.Add("END_DATE", taxForm.EndDate.ToString("G"));
			parms.Add("IS_TAX_EXEMPT", taxForm.IsTaxExempt.ToString());
			parms.Add("IS_FORM_W9", taxForm.IsFormW9.ToString());
			parms.Add("IS_FORM_147", taxForm.IsForm147.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
			parms.Add("LAST_MODIFIED_USER", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
			return parms;
		}


		public int Insert(Form1099 taxForm, Dictionary<string, string> parms)
		{
			parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString(CultureInfo.InvariantCulture));
			parms.Add("CREATED_BY_USER", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
			return svc.InsertRegistrationData(taxForm.RegFormId, "FORM_1099_INFO", parms);
		}

		public void Update(Form1099 taxForm, Dictionary<string, string> parms)
		{
			svc.UpdateRegistrationData(taxForm.RegFormId, "FORM_1099_INFO", parms);
		}
    }
}
