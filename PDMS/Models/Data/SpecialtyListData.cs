namespace Models.Data
{
    public class SpecialtyListData 
    {

        public SpecialtyListData()
            : base()
        {

        }

        public SpecialtyListData(string specialtyTypeName, int specialtyTypeId, int providerTypeID, string taxonomyCode, int taxonomyTypeId)
		{
            SPECIALTY_TYPE_NAME = specialtyTypeName;
            SPECIALTY_TYPE_ID = specialtyTypeId;
            PROVIDER_TYPE_ID = providerTypeID;
            TAXONOMY_CODE = taxonomyCode;
            TAXONOMY_TYPE_ID = taxonomyTypeId;
		}


        public string SPECIALTY_TYPE_NAME { get; set; }
        public int SPECIALTY_TYPE_ID { get; set; }
        public int PROVIDER_TYPE_ID { get; set; }
        public string TAXONOMY_CODE { get; set; }
        public int TAXONOMY_TYPE_ID { get; set; }


    }
}
