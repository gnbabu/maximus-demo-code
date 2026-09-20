namespace MAXIMUS.Core.Libraries
{
   public class ELicenseVerificationRequest
    {
        
        public string last_name { get; set; } 

        public string dob { get; set; }
        public string last_4_ssn { get; set; }
        public string board_type { get; set; }

       public  ELicenseVerificationRequest()
        {

        }

        public ELicenseVerificationRequest(string lname,string dateofbirth, string last4ssn,string boardtype)
        {
            last_name = lname;
            dob = dateofbirth;
            last_4_ssn = last4ssn;
            board_type = boardtype;
        }
    }
}
