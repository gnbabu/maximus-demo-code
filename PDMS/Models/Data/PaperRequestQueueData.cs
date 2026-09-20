using MAXIMUS.Core.Libraries;
using System;
using System.Data;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class PaperRequestQueueData
    {
        public PaperRequestQueueData() { }

        public int PaperRequestQueueID { get; set; }

        public Guid? UserID { get; set; }

        public int RegID { get; set; }

        public int PartyID { get; set; }

        public int RequestTypeID { get; set; }

        public int RequestStatusTypeID { get; set; }

        public int DocumentTypeID { get; set; }

        public int DocumentHandleID { get; set; }

        public int ProviderCategoryTypeID { get; set; }

        public int ProviderTypeID { get; set; }

        public string ProviderTypeName { get; set; }

        public int SpecialtyTypeID { get; set; }

        public int TaxonomyTypeID { get; set; }

        public string TaxonomyCode { get; set; }

        public string ProviderName { get; set; }

        public string NPI { get; set; }

        public string TaxID { get; set; }

        public string MedicaidID { get; set; }

        public string ZipCode { get; set; }

        public string ZipExt { get; set; }

        public string Comments { get; set; }

        public int ReferralID { get; set; }

        public string ReferralNumber { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? LastModifiedUser { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public DateTime? RevalidationDate { get; set; }

        public int WorkflowRequestedID { get; set; }

        public bool ConvertedProvider{ get; set; }

        public int ApplicationTypeID { get; set; }
        
        public int SortBy { get; set; }

        public int PageNumber { get; set; }

        public int RowsPerPage { get; set; }

        public bool SortAsc  { get; set; }

        public int PaperRequestTypeID { get; set; }

        public void LoadPaperFieldsFromDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];
            PaperRequestQueueID = Methods.GetIntValue(dr["PaperRequestQueueID"]);
            RequestTypeID = Methods.GetIntValue(dr["PaperRequestTypeID"]);
            DocumentHandleID = Methods.GetIntValue(dr["DocumentHandleID"]);
            RequestStatusTypeID = Methods.GetIntValue(dr["PaperRequestStatusTypeID"]);
            DocumentTypeID = Methods.GetIntValue(dr["PaperRequestDocumentTypeID"]);
            RegID = Methods.GetIntValue(dr["RegID"]);
            ReferralID = Methods.GetIntValue(dr["DIDDReferralID"]);

            ProviderName = Methods.GetStringValue(dr["ProviderName"]);
            NPI = Methods.GetStringValue(dr["NPI"]);
            TaxID = Methods.GetStringValue(dr["TaxID"]);
            MedicaidID = Methods.GetStringValue(dr["MedicaidID"]);
            ApplicationTypeID = Methods.GetIntValue(dr["ApplicationTypeID"]);
            ProviderTypeID = Methods.GetIntValue(dr["ProviderTypeID"]);
            ProviderCategoryTypeID = Methods.GetIntValue(dr["ProviderCategoryTypeID"]);
            ZipCode = Methods.GetStringValue(dr["ZipCode"]);
            ZipExt = Methods.GetStringValue(dr["ZipExt"]);
            SpecialtyTypeID = Methods.GetIntValue(dr["SpecialtyTypeID"]);
            TaxonomyTypeID = Methods.GetIntValue(dr["TaxonomyTypeID"]);
            TaxonomyCode = Methods.GetStringValue(dr["TaxonomyCode"]);
            Comments = Methods.GetStringValue(dr["Comments"]);

            if (dr["RevalidationDate"] != DBNull.Value)
            {
                RevalidationDate = Methods.GetDateValue(dr["RevalidationDate"]);
            }

            if (dr["LastModifiedDateTime"] != DBNull.Value)
            {
                LastModifiedDate = Methods.GetDateValue(dr["LastModifiedDateTime"]);
            }
        }

        public void LoadMatchFieldsFromDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];
            PaperRequestQueueID = Methods.GetIntValue(dr["PaperRequestQueueID"]);
            RegID = Methods.GetIntValue(dr["RegID"]);
            ReferralID = Methods.GetIntValue(dr["DIDDReferralID"]);
            ProviderName = Methods.GetStringValue(dr["ProviderName"]);
            NPI = Methods.GetStringValue(dr["NPI"]);
            TaxID = Methods.GetStringValue(dr["TaxID"]);
            MedicaidID = Methods.GetStringValue(dr["MedicaidID"]);
            ApplicationTypeID = Methods.GetIntValue(dr["ApplicationTypeID"]);
            ProviderTypeID = Methods.GetIntValue(dr["ProviderTypeID"]);
            ProviderCategoryTypeID = Methods.GetIntValue(dr["ProviderCategoryTypeID"]);
            ZipCode = Methods.GetStringValue(dr["ZipCode"]);
            ZipExt = Methods.GetStringValue(dr["ZipExt"]);
            SpecialtyTypeID = Methods.GetIntValue(dr["SpecialtyTypeID"]);
            TaxonomyTypeID = Methods.GetIntValue(dr["TaxonomyTypeID"]);
            TaxonomyCode = Methods.GetStringValue(dr["TaxonomyCode"]);
            ConvertedProvider = Methods.GetBoolean(dr["PendingConvertedProvider"]);

            if (dr["RevalidationDate"] != DBNull.Value)
            {
                RevalidationDate = Methods.GetDateValue(dr["RevalidationDate"]);
            }

        }
    }
}
