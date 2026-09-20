using MAXIMUS.Core.Libraries;
using System;
using System.Data;

namespace MAXIMUS.Models.Data.PDMS
{
    public class PartyEligibility
    {
        public int PartyEligibilityID { get; set; }
        public int PartyID { get; set; }
        public int PartyTypeID { get; set; }
        public int PartyEligibilityStatusID { get; set; }
        public string PartyEligibiityStatus { get; set; }
        public int? EligibilityID { get; set; }
        public string EligibilityName { get; set; }
        public int EligibilityTypeID { get; set; }
        public string EligibilityType { get; set; }
        public string EligibilityMedicaidID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime ParyEligibilityCreatedDateTime { get; set; }
        public Guid PartyEligibilityCreatedUser { get; set; }
        public string PartyEligibilityCreatedUserName { get; set; }
        public DateTime ParyEligibilityLastModifiedDateTime { get; set; }
        public Guid PartyEligibilityLastModifiedUser { get; set; }
        public string PartyEligibilityLastModifiedUserName { get; set; }
        public DateTime StatusLastModifiedDateTime { get; set; }
        public Guid StatusLastModifiedUser { get; set; }
        public string StatusLastModifiedUserName { get; set; }


     #region Public Methods
        public void LoadObjectFromDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];
            PartyEligibilityID = Methods.GetIntValue(dr["PARTY_ELIGIBILITY_ID"]);
            PartyID = Methods.GetIntValue(dr["PARTY_ID"]);
            EligibilityID = Methods.GetIntValue(dr["ELIGIBILITY_ID"]);
            EligibilityName = Methods.GetStringValue(dr["ELIGIBILITY_NAME"]);
            EligibilityTypeID = Methods.GetIntValue(dr["ELIGIBILITY_TYPE_ID"]);
            EligibilityType = Methods.GetStringValue(dr["ELIGIBILITY_TYPE_NAME"]);
            EligibilityMedicaidID = Methods.GetStringValue(dr["MEDICAID_ID"]);
            PartyEligibilityStatusID = Methods.GetIntValue(dr["PARTY_ELIGIBILITY_STATUS_ID"]);
            PartyEligibiityStatus = Methods.GetStringValue(dr["PARTY_ELIGIBILITY_STATUS_NAME"]);
            StartDate = Methods.GetDateValue(dr["START_DATE"]);
            EndDate = Methods.GetDateValue(dr["END_DATE"]);
            ParyEligibilityLastModifiedDateTime = Convert.ToDateTime(Methods.GetDateTimeValue(dr["PARTY_ELIGIBILITY_LAST_MODIFIED_DATETIME"], false));
            PartyEligibilityLastModifiedUserName = Methods.GetStringValue(dr["PARTY_ELIGIBILITY_LAST_MODIFIED_USERNAME"]);
            StatusLastModifiedDateTime = Convert.ToDateTime(Methods.GetDateTimeValue(dr["STATUS_LAST_MODIFIED_DATETIME"], false));
            StatusLastModifiedUserName = Methods.GetStringValue(dr["STATUS_LAST_MODIFIED_USER"]);
        }

       
        #endregion

    }
}
