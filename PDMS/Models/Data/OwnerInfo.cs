using MAXIMUS.Core.Libraries;
using Models.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;

namespace Models.Data
{
    public class OwnerInfo: IOwner
    {
        public int RegOwnerId { get; set; }
        public int RegId { get; set; }
        public int? AddressId { get; set; }
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public string TaxId { get; set; }
        public decimal? PercentageOfOwnership { get; set; }
        public int? ModifiedStatusTypeId { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public Guid LastModifiedUser { get; set; }
        public int RegOwnerTypeId { get; set; }
        public int? OwnerId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ResidentFlag { get; set; }
        public DateTime? CreatedOnDateTime { get; set; }
        public Guid? CreatedByUser { get; set; }
        public int AffiliationTypeId { get; set; }

        private PDMSService.PDMSServiceClient _svc;

        private PDMSService.PDMSServiceClient svc
        {
            get { return _svc ?? (_svc = new PDMSService.PDMSServiceClient()); }
        }


        public void Load(DataRow row)
        {
            if (row == null)
            {
                RegOwnerId = 0;
                AddressId = 0;
                TaxId = string.Empty;
				DOB = null;
				PercentageOfOwnership = 0;
				OwnerId = 0;
				RegOwnerTypeId = 0;
                BeginDate = DateTime.Today;
                EndDate = new DateTime(2299, 12, 31);
                //ResidentFlag = false;
            }
            else
            {
	            RegOwnerId = Methods.GetIntValue(row, "REG_OWNER_ID");
                AddressId = Methods.GetIntValue(row, "REG_ADDRESS_ID");
				TaxId = Methods.GetString("TAX_ID", row);
				DOB = Methods.GetDateValue(row, "DOB");
				PercentageOfOwnership = Methods.GetIntValue(row, "PERCENTAGE_OF_OWNERSHIP");
				RegOwnerTypeId = Methods.GetIntValue(row, "REG_OWNER_TYPE_ID");
                BeginDate = Methods.GetDateValue(row, "BEGIN_DATE");
                EndDate = Methods.GetDateValue(row, "END_DATE") == DateTime.MinValue ? new DateTime(2299, 12, 31) : Methods.GetDateValue(row, "END_DATE");
                AffiliationTypeId = Methods.GetIntValue(row, "affiliation_type");
                //ResidentFlag = false; // Methods.GetBool("RESIDENT_FLAG", row);
			}
        }

        public Dictionary<string, string> CreateParameterList(OwnerInfo ownerinfo)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>(); 
            parms.Add("REG_OWNER_ID", ownerinfo.AddressId.ToString());
            parms.Add("REG_ADDRESS_ID", ownerinfo.AddressId.ToString());
            parms.Add("TAX_ID", ownerinfo.TaxId);
            parms.Add("PERCENTAGE_OF_OWNERSHIP", ownerinfo.PercentageOfOwnership.ToString());
            parms.Add("BEGIN_DATE", ownerinfo.BeginDate == null ? null :  ownerinfo.BeginDate.ToString());
            parms.Add("END_DATE", ownerinfo.EndDate.ToString());
            parms.Add("OWNER_ID", ownerinfo.OwnerId.ToString());
            parms.Add("REG_OWNER_ID", ownerinfo.RegOwnerId.ToString());
            parms.Add("REG_OWNER_TYPE_ID", ownerinfo.RegOwnerTypeId.ToString());
            parms.Add("RESIDENT_FLAG", ownerinfo.ResidentFlag.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            parms.Add("LAST_MODIFIED_USER", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            return parms;
        }


        public int Insert(OwnerInfo ownerinfo, Dictionary<string, string> parms)
        {
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            parms.Add("CREATED_BY_USER", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            return svc.InsertRegistrationData(ownerinfo.RegOwnerId, "OWNER", parms);
        }

        public void Update(OwnerInfo ownerinfo, Dictionary<string, string> parms)
        {
            svc.UpdateRegistrationData(ownerinfo.RegOwnerId, "OWNER", parms);
        }
    }
}
