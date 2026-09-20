using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;
using System.Collections.Generic;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class ProfessionalLicense:  ISearchCriteria
    {

        public ProfessionalLicense() { }

        public Guid? UserID { get; set; }


        public int RegLicensureID { get; set; }

        
        public string LicenseNumber { get; set; }
        public string LicenseState { get; set; }

        public string LicenseStatus { get; set; }
        public string LicenseSubStatus { get; set; }
        public string DisciplinaryAction { get; set; }
        public string LicenseBoardName { get; set; }
        public string LicenseType { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CountyName { get; set; }

        public string County { get; set; }
        public bool ELicenseVerified { get; set; }
        public DateTime LicenseEffectiveDate { get; set; }
        public DateTime LicenseExpirationDate { get; set; }

        public int RegAddressID { get; set; }

        public int RegID { get; set; }

        public Guid? Created_By_User { get; set; }

        public DateTime LastModifiedDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }
        
        public List<SpecialtyFocusCertificate> SpecialtyFocusCertificates { get; set; }
        //*ISearchCriteria
        public int SortBy { get; set; }

        public int PageNumber { get; set; }

        public int RowsPerPage { get; set; }

        public bool SortAsc { get; set; }
        //*ISearchCriteria

        public string SortByColName { get; set; }

        public string SortDirection { get; set; } //uses either SortDirection instead of SortAsc - not a common approach to paging and sorting at this time
    }

    public class SpecialtyFocusCertificate
    {
        public string EndorsementFocus { get; set; }
        public string CertifyingOrganization { get; set; }
        public string EndorsementSpecialty { get; set; }
        public DateTime CertificateDate { get; set; }
        public DateTime CertificateExpirationDate { get; set; }

      
        public string EndorsementNumber { get; set; }
        public string EndorsementStatus { get; set; }
        public string EndorsementSubStatus { get; set; }
        public string EndorsementBoardAction { get; set; }
        public DateTime EndorsementIssuedDate { get; set; }
        public DateTime EndorsementExpirationDate { get; set; }
        public SpecialtyFocusCertificate()
        {

        }
    }
}
