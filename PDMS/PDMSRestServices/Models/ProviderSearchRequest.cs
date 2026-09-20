using PDMSRestServices.Models;

namespace PDMSRestServices.Models
{
    public class ProviderSearchRequest
    {
        public string GroupName { get; set; }
        public string BaseMedicaidID { get; set; }
        public string NPI { get; set; }
        public string TaxID { get; set; }
        public string PDMSStatus { get; set; }

        public string PDMSStatusDate { get; set; }
        public string TennCareStatus { get; set; }
        public string ProviderCategoryTypeId { get; set; }
        public string ProviderTypeId { get; set; }
        public string SpecialtyId { get; set; }
        public string DBAName { get; set; }
        public string ApplicationTypeId { get; set; }
        public string RoleName { get; set; }
        public string WaiverType { get; set; }
        public string ContractType { get; set; }
        public string MCP { get; set; }
        public string Program { get; set; }

        public string DateReceived { get; set; }
        public string Taxonomy { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string ContractStatus { get; set; }
        public string EnrollmentStatusReason { get; set; }
        public string RegID { get; set; }
        public string CredId { get; set; }
        public string MedicareNumber { get; set; }
        public string DODDContractNumber { get; set; }
        public string ODARegistrationStatus { get; set; }
        public string DODDRegistrationStatus { get; set; }
        public string PNMEnrollmentStatus { get; set; }
        public string PNMApplicationStatus { get; set; }
        public string SortExpression { get; set; }
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int StartRowIndex { get; set; }

        public bool GetTotalResultCount { get; set; }
    }

    public static class ProviderSearchRequestExtensions
    {
        public static Dictionary<string, object> ToParams(this ProviderSearchRequest r)
        {
            if (r == null)
                throw new ArgumentNullException(nameof(r));

            return new Dictionary<string, object>
            {
                ["GroupName"] = r.GroupName,
                ["BaseMedicaidID"] = r.BaseMedicaidID,
                ["NPI"] = r.NPI,
                ["TaxID"] = r.TaxID,
                ["PDMSStatus"] = r.PDMSStatus,
                ["PDMSStatusDate"] = r.PDMSStatusDate,
                ["TennCareStatus"] = r.TennCareStatus,
                ["ProviderCategoryTypeId"] = r.ProviderCategoryTypeId,
                ["ProviderTypeId"] = r.ProviderTypeId,
                ["SpecialtyId"] = r.SpecialtyId,
                ["DBAName"] = r.DBAName,
                ["applicationTypeId"] = r.ApplicationTypeId,
                ["RoleName"] = r.RoleName,
                ["WaiverType"] = r.WaiverType,
                ["ContractType"] = r.ContractType ?? string.Empty,
                ["MCP"] = r.MCP,
                ["Program"] = r.Program,
                ["DateReceived"] = r.DateReceived,
                ["Taxonomy"] = r.Taxonomy,
                ["County"] = r.County,
                ["City"] = r.City,
                ["Area"] = r.Area,
                ["ContractStatus"] = r.ContractStatus ?? string.Empty,
                ["EnrollmentStatusReason"] = r.EnrollmentStatusReason,
                ["RegID"] = r.RegID,
                ["CredId"] = r.CredId,
                ["MedicareNumber"] = r.MedicareNumber,
                ["DODDContractNumber"] = r.DODDContractNumber,
                ["ODARegistrationStatus"] = r.ODARegistrationStatus,
                ["DODDRegistrationStatus"] = r.DODDRegistrationStatus,
                ["PNMEnrollmentStatus"] = r.PNMEnrollmentStatus,
                ["PNMApplicationStatus"] = r.PNMApplicationStatus,
                ["SortExpression"] = r.SortExpression,
                ["PageSize"] = r.PageSize
            };
        }
    }
}


