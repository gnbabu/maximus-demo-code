using MAXIMUS.Core.Libraries;
using Models.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;

namespace Models.Data
{
    public class Address : IAddress
    {
        [MatchParent("RegId")]
        public int RegId { get; set; }

        [MatchParent("AddressId")]
        public int AddressId { get; set; }

        [MatchParent("AddressTypeId")]
        public int AddressTypeId { get; set; }

        [MatchParent("ContactName")]
        public string ContactName { get; set; }

        [MatchParent("ContactType")]
        public string ContactType { get; set; }        

        [MatchParent("StreetAddress")]
        public string StreetAddress { get; set; }

        [MatchParent("UnitAddress")]
        public string UnitAddress { get; set; }

        [MatchParent("City")]
        public string City { get; set; }

        [MatchParent("State")]
        public string State { get; set; }

        [MatchParent("County")]
        public string County { get; set; }

        [MatchParent("CountyDisplay")]
        public string CountyDisplay { get; set; }

        [MatchParent("Zip5")]
        public string Zip5 { get; set; }

        [MatchParent("Zip4")]
        public string Zip4 { get; set; }
        
        [MatchParent("BorderStateInd")]
        public string BorderStateInd { get; set; }

        [MatchParent("FirstName")]
        public string FirstName { get; set; }

        [MatchParent("MiddleName")]
        public string MiddleName { get; set; }

        [MatchParent("LastName")]
        public string LastName { get; set; }

        [MatchParent("Title")]
        public string Title { get; set; }

        [MatchParent("ProviderTitle")]
        public string ProviderTitle { get; set; }

        [MatchParent("Suffix")]
        public string Suffix { get; set; }

        [MatchParent("OrgName")]
        public string OrgName { get; set; }

        [MatchParent("Name")]
        public string Name { get; set; }

        [MatchParent("PhoneNumber1")]
        public string PhoneNumber1 { get; set; }

        [MatchParent("PhoneExt1")]
        public string PhoneExt1 { get; set; }

        [MatchParent("CanText1")]
        public string CanText1 { get; set; }

        [MatchParent("PhoneNumber2")]
        public string PhoneNumber2 { get; set; }

        [MatchParent("PhoneExt2")]
        public string PhoneExt2 { get; set; }

        [MatchParent("EffectiveDate")]
        public string EffectiveDate { get; set; }

        [MatchParent("EndDate")]
        public string EndDate { get; set; }

        [MatchParent("CanText2")]
        public string CanText2 { get; set; }

        [MatchParent("FaxNumber1")]
        public string FaxNumber1 { get; set; }

        [MatchParent("FaxNumber2")]
        public string FaxNumber2 { get; set; }

        [MatchParent("Email1")]
        public string Email1 { get; set; }

        [MatchParent("Email2")]
        public string Email2 { get; set; }

        [MatchParent("OfficeManager")]
        public string OfficeManager { get; set; }

		public string Longitude { get; set; }
		public string Latitude { get; set; }

        public Dictionary<string, string> CreateParameterList(Address address)
	    {
		    Dictionary<string, string> parms = new Dictionary<string, string>();
		    parms.Add("REG_ID", address.RegId.ToString());
		    parms.Add("ADDRESS_TYPE_ID", address.AddressTypeId.ToString());
		    parms.Add("PRACTICE_NAME", address.OrgName);
		    parms.Add("ADDRESS1", address.StreetAddress);
		    parms.Add("ADDRESS2", address.UnitAddress);
            parms.Add("CONTACT_TYPE", address.ContactType);
            parms.Add("TITLE", address.Title);
            parms.Add("INDIVIDUAL_NAME", address.Name);
            parms.Add("CONTACT_NAME", address.ContactName);
		    parms.Add("FIRST_NAME", address.FirstName);
		    parms.Add("MIDDLE_NAME", address.MiddleName);
		    parms.Add("LAST_NAME", address.LastName);
		    parms.Add("SUFFIX", address.Suffix);
		    parms.Add("CITY", address.City);
		    parms.Add("STATE", address.State);
		    parms.Add("ZIP", address.Zip5);			
		    parms.Add("EXT_ZIP", address.Zip4);
            if (address.BorderStateInd =="Yes")  parms.Add("BorderStateIndicatorCode", Constants.BorderStateText.Yes);
            if (address.BorderStateInd == "No") parms.Add("BorderStateIndicatorCode", Constants.BorderStateText.No);
            if (!string.IsNullOrEmpty(address.County))
            {
                string addressCounty = address.County;
                string SplitText = "-CountyName-";
                string[] CountyCodeWithName = Regex.Split(addressCounty, SplitText);
                if (CountyCodeWithName.Length==2)
                {
                    parms.Add("COUNTY", CountyCodeWithName[0]);
                    parms.Add("COUNTYNAME", CountyCodeWithName[1]);
                }               
                
            }
            else
            {
                string county = CountyDisplay;
                string countyValue = County;
                if (!string.IsNullOrEmpty(county)) parms.Add("COUNTY", countyValue);
                if (!string.IsNullOrEmpty(countyValue)) parms.Add("COUNTYNAME", county);                
            }
            if(!string.IsNullOrEmpty(address.EffectiveDate))
            {
                parms.Add("ADR_EFFECTIVE_DATE", address.EffectiveDate);
            }
            if (!string.IsNullOrEmpty(address.EndDate))
            {
                parms.Add("ADR_END_DATE", address.EndDate);
            }

            parms.Add("PHONE1", Methods.StripNonNumerics(address.PhoneNumber1));
		    parms.Add("PHONE1_EXT", Methods.StripNonNumerics(address.PhoneExt1));
		    parms.Add("CAN_TEXT1", address.CanText1);
		    parms.Add("PHONE2", Methods.StripNonNumerics(address.PhoneNumber2));
		    parms.Add("PHONE2_EXT", Methods.StripNonNumerics(address.PhoneExt2));
		    parms.Add("CAN_TEXT2", address.CanText2);
		    parms.Add("FAX1", Methods.StripNonNumerics(address.FaxNumber1));
		    parms.Add("FAX2", Methods.StripNonNumerics(address.FaxNumber2));
		    parms.Add("EMAIL1", address.Email1);
		    parms.Add("EMAIL2", address.Email2);
		    parms.Add("OFFICE_MANAGER", address.OfficeManager);
		    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
		    parms.Add("LAST_MODIFIED_USER", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
			parms.Add("LONGITUDE", address.Longitude);
			parms.Add("LATITUDE", address.Latitude);
		    return parms;
        }

        private PDMSService.PDMSServiceClient _svc;

        private PDMSService.PDMSServiceClient svc
        {
	        get { return _svc ?? (_svc = new PDMSService.PDMSServiceClient()); }
        }

        public void Load(DataRow addressRow, DataRow provRow = null)
        {
	        RegId = Methods.Exists(addressRow, "REG_ID") ? Convert.ToInt32(addressRow["REG_ID"].ToString()) : 0;
            if (Methods.Exists(addressRow, "REG_ADDRESS_ID"))
            {
	            AddressId = Convert.ToInt32(addressRow["REG_ADDRESS_ID"].ToString());
            }

            ContactName = Methods.GetString("CONTACT_NAME", addressRow);
            ContactType = Methods.GetString("CONTACT_TYPE", addressRow);            
            StreetAddress = addressRow["ADDRESS1"].ToString();
            UnitAddress = addressRow["ADDRESS2"].ToString();
            City = addressRow["CITY"].ToString();
            State = addressRow["STATE"].ToString();
            County = addressRow["COUNTY"].ToString();
            if (addressRow.Table.Columns.Contains("CountyName"))
            {
                CountyDisplay = addressRow["CountyName"].ToString();
            }
            else
            {
                CountyDisplay = string.Empty;
            }
               
            Zip5 = addressRow["ZIP"].ToString();
            Zip4 = addressRow["EXT_ZIP"].ToString();
            if (!string.IsNullOrEmpty(Methods.GetString("BorderStateIndicatorCode", addressRow)))
            {
                if (addressRow["BorderStateIndicatorCode"].ToString() == Constants.BorderStateText.No)
                    BorderStateInd = "No";
                if (addressRow["BorderStateIndicatorCode"].ToString() == Constants.BorderStateText.Yes)
                    BorderStateInd = "Yes";
            }
            if (!string.IsNullOrEmpty(Methods.GetString("ADR_EFFECTIVE_DATE", addressRow)))
            {
                EffectiveDate = Convert.ToDateTime(Methods.GetString("ADR_EFFECTIVE_DATE", addressRow)).ToString("MM/dd/yyyy");
            }
            if (!string.IsNullOrEmpty(Methods.GetString("ADR_END_DATE", addressRow)))
            {
                EndDate = Convert.ToDateTime(Methods.GetString("ADR_END_DATE", addressRow)).ToString("MM/dd/yyyy");
            }
            if (provRow != null)
            {
                FirstName = !string.IsNullOrEmpty(Methods.GetString("FIRST_NAME", addressRow))
                    ? addressRow["FIRST_NAME"].ToString()
                    : provRow["FIRST_NAME"].ToString();
                MiddleName = !string.IsNullOrEmpty(Methods.GetString("MIDDLE_NAME", addressRow))
                    ? addressRow["MIDDLE_NAME"].ToString()
                    : provRow["MIDDLE_INITIAL"].ToString();
                LastName = !string.IsNullOrEmpty(Methods.GetString("LAST_NAME", addressRow))
                    ? addressRow["LAST_NAME"].ToString()
                    : provRow["LAST_NAME"].ToString();
                Suffix = !string.IsNullOrEmpty(Methods.GetString("SUFFIX", addressRow))
                    ? addressRow["SUFFIX"].ToString()
                    : provRow["TITLE"].ToString();
                OrgName = !string.IsNullOrEmpty(Methods.GetString("PRACTICE_NAME", addressRow))
                    ? addressRow["PRACTICE_NAME"].ToString()
                    : provRow["NAME"].ToString();
                Title = Methods.GetString("TITLE", addressRow);
                Name = !string.IsNullOrEmpty(Methods.GetString("INDIVIDUAL_NAME", addressRow))
                    ? addressRow["INDIVIDUAL_NAME"].ToString()
                    : provRow["NAME"].ToString();
            }
            else
            {
                FirstName = Methods.GetString("FIRST_NAME", addressRow);
                MiddleName = Methods.GetString("MIDDLE_NAME", addressRow);
                LastName = Methods.GetString("LAST_NAME", addressRow);
                Suffix = Methods.GetString("SUFFIX", addressRow);
                OrgName = Methods.GetString(Methods.Exists(addressRow,"PRACTICE_ROW") ? "PRACTICE_NAME" : "NAME", addressRow);
                Title  = Methods.GetString("TITLE", addressRow);
            }
			if(Methods.Exists(addressRow,"PHONE1"))
                PhoneNumber1 = Methods.FormatPhone(addressRow["PHONE1"].ToString());
            if (Methods.Exists(addressRow, "PHONE1_EXT"))
                PhoneExt1 = Methods.FormatPhone(addressRow["PHONE1_EXT"].ToString());
            if (Methods.Exists(addressRow, "PHONE2"))
                PhoneNumber2 = Methods.FormatPhone(addressRow["PHONE2"].ToString());
            if (Methods.Exists(addressRow, "PHONE2_EXT"))
                PhoneExt2 = Methods.FormatPhone(addressRow["PHONE2_EXT"].ToString());
            if (Methods.Exists(addressRow, "FAX1"))
                FaxNumber1 = Methods.FormatPhone(addressRow["FAX1"].ToString());
            if (Methods.Exists(addressRow, "FAX2"))
                FaxNumber2 = Methods.FormatPhone(addressRow["FAX2"].ToString());

            Email1 = Methods.GetString("EMAIL1", addressRow);
            Email2 = Methods.GetString("EMAIL2", addressRow);

            OfficeManager = string.Empty;
            CanText1 = "0";
            CanText2 = "0";
            try
            {
                OfficeManager = Methods.GetString("OFFICE_MANAGER", addressRow);
            }
            catch (Exception)
            {
                // Not exactly an error if there is no OFFICE_MANAGER. 
                // Just squelch it.
                OfficeManager = string.Empty;
            }

            try
            {
                CanText1 = Methods.GetString("CAN_TEXT1", addressRow);
            }
            catch (Exception)
            {
                CanText1 = "0";
            }

            try
            {
                CanText2 = Methods.GetString("CAN_TEXT2", addressRow);
            }
            catch (Exception)
            {
                CanText2 = "0";
            }

            Longitude = Methods.GetString("LONGITUDE", addressRow);
            Latitude = Methods.GetString("LATITUDE", addressRow);
        }

        public int Insert(Address address, Dictionary<string, string> parms)
        {
	        parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
	        parms.Add("CREATED_BY_USER", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
	        return svc.InsertRegistrationData(address.RegId, "ADDRESS", parms);
        }

        public void Update(Address address, Dictionary<string, string> parms)
        {
	        try
	        {
		        parms.Remove("CREATED_ON_DATE_TIME");
		        parms.Remove("CREATED_BY_USER");
                parms.Add("DATE_ADDRESS_CHANGED", DateTime.Now.ToString());
            }
	        catch (Exception)
	        {
		        // It the parameters don't exist, we don't care
	        }

	        svc.UpdateRegistrationData(address.AddressId, "ADDRESS", parms);
        }
    }
}
