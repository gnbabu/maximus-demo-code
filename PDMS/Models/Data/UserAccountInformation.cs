using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class UserAccountInformation
    {
        public UserAccountInformation() { }

        public Guid? UserID { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public string ContactPhone { get; set; }

        public string ContactPhoneExt { get; set; }

        public string TaxID { get; set; }

        public int TaxIDTypeID { get; set; }

    }
}
