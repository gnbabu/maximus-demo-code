using MAXIMUS.Core.Libraries;
using System.Data;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class ReferralData
    {
        public ReferralData() { }

        public int ReferralID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string GroupName { get; set; }

        public string ReferralNumber { get; set; }

        public string TaxID { get; set; }

        public string NPI { get; set; } //always blank for NE

        public string Email { get; set; } 

        public string ZipCode { get; set; }

        public string ZipExt { get; set; }

        public bool IsActive { get; set; }

        public int ReferralSuffix { get; set; }

        public int ReferralTypeID { get; set; }

        public int ReferralLocationTypeID { get; set; }

        #region Public Methods
        public void LoadObjectFromDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];
            ReferralID = Methods.GetIntValue(dr, "DIDD_REFERRAL_ID");
            ReferralTypeID = Methods.GetIntValue(dr, "DIDD_REFERRAL_TYPE_ID");
            FirstName = Methods.GetStringValue(dr, "FIRST_NAME");
            LastName = Methods.GetStringValue(dr, "LAST_NAME");
            GroupName = Methods.GetStringValue(dr, "GROUP_ENTITY_NAME");
            ReferralNumber = Methods.GetStringValue(dr, "APPLICATION_NO");
            NPI = Methods.GetStringValue(dr, "NPI");
            TaxID = Methods.GetStringValue(dr, "TaxID");
            Email = Methods.GetStringValue(dr, "EMAIL");
            ZipCode = Methods.GetStringValue(dr, "SERVICING_ZIP");
            ZipExt = Methods.GetStringValue(dr, "SERVICING_EXT_ZIP");
            ReferralLocationTypeID = Methods.GetIntValue(dr, "DIDD_REFERRAL_LOCATION_TYPE_ID");

        }
        #endregion

    }
}