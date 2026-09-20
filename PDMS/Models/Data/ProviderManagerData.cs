using MAXIMUS.Core.Libraries;
using System;
using System.Data;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class ProviderManagerData
    {
        public ProviderManagerData(){}

        public Guid? UserID { get; set; }

        public int RegID { get; set; }

        public int PartyID { get; set; }

        public int RegistrationStatusTypeID { get; set; }

        public string RegistrationStatusType { get; set; }

        public int RegistrationProgramStatusTypeID { get; set; }

        public string RegProgramStatusTypeInternal { get; set; }

        public string RegProgramStatusTypeExternal { get; set; }

        public int ReferralID { get; set; }

        public int ReferralTypeID { get; set; }

        public string MedicaidID { get; set; }

        public int ProviderTypeID { get; set; }

        public string ProviderTypeName { get; set; }

        public int ProviderCategoryTypeID { get; set; }

        public string ProviderCategoryTypeName { get; set; }

        public int SpecialtyTypeID { get; set; }

        public string SpecialtyTypeName { get; set; }

        public string TaxID { get; set; }

        public int TaxIDTypeID { get; set; }

        public string NPI { get; set; }

        public string ZipCode { get; set; }

        public string ZipExt { get; set; }

        public DateTime? RequestedEffectiveDate { get; set; }

        public DateTime? ChangeEffectiveDate { get; set; }

        public DateTime? SubmitDateTime { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public Guid? LastModifiedUserID { get; set; }

        public string LastModifiedUserName { get; set; }

        public int WorkflowIDRequested { get; set; }

        public bool ConvertedProvider { get; set; }

        public bool KeyFieldEditRequest { get; set; }
        public bool ProviderTypeChangeRequest { get; set; }

        /*PAPER REQUEST QUEUE FIELDS */
        public int PaperRequestQueueID { get; set; }
        public int PaperRequestTypeID { get; set; }

        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeName { get; set; }

        public bool AdminKeyFieldEditRequest { get; set; }

        public bool IsProviderReactivation { get; set; }
        public bool IsCredentialingProvider { get; set; }
        public int WaiverTypeId { get; set; }

        public bool IsAddODMorODAMedicaid { get; set; }

        #region Public Methods
        public void LoadProviderManagerObjectFromDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];
            if (dr["UserID"] != DBNull.Value)
            {
                UserID = new Guid(Methods.GetStringValue(dr["UserID"]));
            }
            RegID = Methods.GetIntValue(dr["RegID"]);
            //PartyID = Methods.GetIntValue(dr["PartyID"]);
            RegistrationStatusTypeID = Methods.GetIntValue(dr["RegistrationStatusTypeID"]);
            RegistrationStatusType = Methods.GetStringValue(dr["RegistrationStatusType"]);
            RegistrationProgramStatusTypeID = Methods.GetIntValue(dr["RegProgramStatusTypeID"]);
            RegProgramStatusTypeInternal = Methods.GetStringValue(dr["RegProgramStatusTypeInternal"]);
            RegProgramStatusTypeExternal = Methods.GetStringValue(dr["RegProgramStatusTypeExternal"]);
            //ReferralID = Methods.GetIntValue(dr["DIDDReferralID"]);
            MedicaidID = Methods.GetStringValue(dr["MedicaidID"]);
            ProviderTypeID = Methods.GetIntValue(dr["ProviderTypeID"]);
            ProviderTypeName = Methods.GetStringValue(dr["ProviderTypeName"]);
            ProviderCategoryTypeID = Methods.GetIntValue(dr["ProviderCategoryTypeID"]);
            ProviderCategoryTypeName = Methods.GetStringValue(dr["ProviderCategoryTypeName"]);
            SpecialtyTypeName = Methods.GetStringValue(dr["SpecialtyTypeName"]);
            TaxID = Methods.GetStringValue(dr["TaxID"]);
            if (dr["RequestedEffectiveDate"] != DBNull.Value)
            {
                RequestedEffectiveDate = Methods.GetDateValue(dr["RequestedEffectiveDate"]);
            }
            if (dr["SubmitDateTime"] != DBNull.Value)
            {
                SubmitDateTime = Methods.GetDateValue(dr["SubmitDateTime"]);
            }
            //if (dr["LastModifiedUserID"] != DBNull.Value)
            //{
            //    LastModifiedUserID = new Guid(Methods.GetStringValue(dr["LastModifiedUserID"]));
            //}
            //LastModifiedUserName = Methods.GetStringValue(dr["LastModifiedUserName"]);
            //if (dr["LastModifiedDateTime"] != DBNull.Value)
            //{
            //    LastModifiedDate = Methods.GetDateValue(dr["LastModifiedDateTime"]);
            //}
        }
        #endregion

    }
}