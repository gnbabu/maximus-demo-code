namespace MAXIMUS.Core.Libraries
{
      

    /*
    Below classes were created similar to what we have in test JSON file.

      we are using same names/fields as JSON file though its violationg naming convension of c# property should start with upper case.
      Also in case of error we are only receiving a json string not ideal to make a class out of it. so need to look more once we have real Webservice URL and have accurate response's for request calls.
        */
    public class ELicenseVerificationResponse
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string dob { get; set; }

        public string last_4_ssn { get; set; }
        public string board_type { get; set; }
        public string license_number { get; set; }
        public string license_status { get; set; }
        public string license_sub_status { get; set; }
        public string disciplinary_action { get; set; }
        public string begin_date { get; set; }
        public string end_date { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string county { get; set; }

        public endorsement[] endorsements { get; set; }
    }
   
    public class endorsement
    {
        public string endorsement_number { get; set; }
        public string endorsement_status { get; set; }
        public string endorsement_substatus { get; set; }
        public string endorsement_boardaction { get; set; }
        public string endorsement_issuedate { get; set; }
        public string endorsement_expirationdate { get; set; }
        public specialityandfocus[] specialityandfocus { get; set; }
    }
    public class specialityandfocus
    {

        public string endorsement_speciality { get; set; }
        public string endorsement_focus { get; set; }
        public string certifying_organization { get; set; }
        public string certificate_date { get; set; }
        public string certificate_expiration { get; set; }

    }
   
}
