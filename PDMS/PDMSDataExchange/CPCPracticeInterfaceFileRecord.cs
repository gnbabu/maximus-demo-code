using FileHelpers;

namespace MAXIMUS.DataExchange.PDMS
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines(true)]

    public class CPCPracticeInterfaceFileRecord
    {
        [FieldQuoted]
        public string PracticeName  { get; set; }

        [FieldQuoted]
        public string OriginalEnrollmentYear  { get; set; }

        [FieldQuoted]
        public string MedicaidId  { get; set; }

        [FieldQuoted]
        public string CPCID  { get; set; }

        [FieldQuoted]
        public string PracticePartnershipID  { get; set; }

        [FieldQuoted]
        public string ConvenerPractice { get; set; }

        [FieldQuoted]
        public string FederalTaxID { get; set; }

         [FieldQuoted]
        public string CPCKidsFlag { get; set; }

         [FieldQuoted]
        public string ContactName { get; set; }

         [FieldQuoted]
        public string Email { get; set; }

         [FieldQuoted]
        public string Phone { get; set; }

         [FieldQuoted]
        public string Address { get; set; }

         [FieldQuoted]
        public string City { get; set; }

         [FieldQuoted]
        public string State { get; set; }

         [FieldQuoted] 
        public string Zip { get; set; }

         [FieldQuoted]
        public string PracticeLocationCounty { get; set; }

         [FieldQuoted]
        public string Track { get; set; }

         [FieldQuoted] 
        public string ProviderType { get; set; }



    }
}
