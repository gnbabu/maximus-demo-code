namespace PDMSRestServices.Facade
{
    public class ProviderSearchResponse
    {
        public int ID { get; set; }
        public string RegID { get; set; }
        public string CredId { get; set; }

        public string OrganizationName { get; set; }
 
        public string ProviderTypeName { get; set; }
      
        public string TaxId { get; set; }
      
        public string NPI { get; set; }
       
        public string SpecialtyTypeName { get; set; }
 
        public string BaseMedicaidID { get; set; }
        
        public string AssignedTo { get; set; }

        public string CurrentStepID { get; set; }

        public int TotalResultCount { get; set; }

    }


    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }

}