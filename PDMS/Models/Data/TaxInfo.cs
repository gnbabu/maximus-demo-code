using MAXIMUS.Core.Libraries;
using Models.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;

namespace Models.Data
{
    public class TaxInfo : ITaxInfo
    {
	    [MatchParent("RegTaxFormId")]
	    public int RegTaxFormId { get; set; }

	    [MatchParent("RegId")]
	    public int RegId { get; set; }

	    [MatchParent("TaxFormId")]
	    public int TaxFormId { get; set; }

        [MatchParent("StateRegistered")]
        public string State { get; set; }

        [MatchParent("INDICATE_FORM")]
        public string INDICATEFORM { get; set; }


        private PDMSService.PDMSServiceClient _svc;

        private PDMSService.PDMSServiceClient svc
        {
	        get { return _svc ?? (_svc = new PDMSService.PDMSServiceClient()); }
        }
		
        public void Load(DataRow row)
        {
	        if (row == null)
	        {
		        RegId = 0;
		        RegTaxFormId = 0;
		        TaxFormId = 0;
		        State = string.Empty;
                INDICATEFORM = string.Empty;
	        }
	        else
	        {
		        RegId = Methods.GetIntValue(row, "REG_ID");
		        RegTaxFormId = Methods.GetIntValue(row, "REG_TAX_INFO_ID");
		        TaxFormId = Methods.GetIntValue(row, "TAX_FORM_ID");
		        State = Methods.GetString("STATE_REGISTERED", row);
                INDICATEFORM = Methods.GetString("INDICATE_FORM", row);

            }
        }

        public Dictionary<string, string> CreateParameterList(TaxInfo taxInfo)
        {
	        Dictionary<string, string> parms = new Dictionary<string, string>();
	        parms.Add("REG_ID", taxInfo.RegId.ToString());
	        parms.Add("TAX_FORM_ID", taxInfo.TaxFormId.ToString());
	        parms.Add("STATE_REGISTERED", taxInfo.State);
            parms.Add("INDICATE_FORM", taxInfo.INDICATEFORM);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
	        parms.Add("LAST_MODIFIED_USER", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            return parms;
        }

        public int Insert(TaxInfo taxInfo, Dictionary<string, string> parms)
        {
	        parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString(CultureInfo.InvariantCulture));
	        parms.Add("CREATED_BY_USER", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
	        return svc.InsertRegistrationData(taxInfo.RegId, "TAX_INFO", parms);
        }

        public void Update(TaxInfo taxInfo, Dictionary<string, string> parms)
        {
	        parms.Add("REG_TAX_INFO_ID", taxInfo.RegTaxFormId.ToString());
            svc.UpdateRegistrationData(taxInfo.RegTaxFormId, "TAX_INFO", parms);
        }
    }
}
