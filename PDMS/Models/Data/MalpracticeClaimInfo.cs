using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class MalpracticeClaimInfo: ISearchCriteria
    {
        public MalpracticeClaimInfo() { }

        public Guid? UserID { get; set; }

        public int REG_MALPRACTICE_CLAIM_ID { get; set; }

        public bool? IsMalPractice { get; set; }

        public DateTime? DateOccurance { get; set; }

        public DateTime? DateClaimFiled { get; set; }

        public string ProfessionalCarrier { get; set; }

        public int? DefendentTypeID { get; set; }

        public string OtherDefendents { get; set; }

        public string Allegations { get; set; }

        public string AllegedInjury { get; set; }

        public bool? IsFiledSuitInCourt { get; set; }

        public DateTime? DateFiled { get; set; }

        public string StateCaseNumber { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string FederalCaseNumber { get; set; }

        public string District { get; set; }

        public int? ClaimStatusID { get; set; }

        public string OtherStatus { get; set; }

        public string AdditionalInfo { get; set; }

        public int RegID { get; set; }

        public DateTime? DateClaimSettled { get; set; }

        public DateTime LastModifiedDateTime { get; set; }

        //*ISearchCriteria
        public int SortBy { get; set; }

        public int PageNumber { get; set; }

        public int RowsPerPage { get; set; }

        public bool SortAsc { get; set; }
        //*ISearchCriteria

        public string SortByColName { get; set; } 

        public string SortDirection { get; set; } //uses either SortDirection instead of SortAsc - not a common approach to paging and sorting at this time

        public string PolicyNumber { get; set; }
        public bool? ISNPDB { get; set; }
        public int? NoofOtherDefendents { get; set; }
        public int? MethodOfResolutionId { get; set; }

        public double? SettledAmount { get; set; }
        public bool? ResultedInDeath { get; set; }
        public string Involvement { get; set; }
        public int? RegAddressId { get; set; }
    }
}
