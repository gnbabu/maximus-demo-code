using Corp.Core.Libraries;
using System.Xml.Serialization;

namespace PDMSRestServices.Models
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("wsCorrectA", Namespace = "http://www.intelligentsearch.com/HostedWebServices/")]
    public partial class USPSAddressVerificationServiceRequest
    {
        private string usernameField;
        private string passwordField;
        private string firmnameField;
        private string urbanizationField;
        private string delivery_line_1Field;
        private string delivery_line_2Field;
        private string city_state_zipField;
        private string ca_codesField;
        private string ca_fillerField;
        private string batchnameField;


        /// <remarks/>
        public string username
        {
            get
            {
                return this.usernameField;
            }
            set
            {
                this.usernameField = value;
            }
        }

        public string password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }
        public string firmname
        {
            get
            {
                return this.firmnameField;
            }
            set
            {
                this.firmnameField = value;
            }
        }
        public string urbanization
        {
            get
            {
                return this.urbanizationField;
            }
            set
            {
                this.urbanizationField = value;
            }
        }
        public string delivery_line_1
        {
            get
            {
                return this.delivery_line_1Field;
            }
            set
            {
                this.delivery_line_1Field = value;
            }
        }
        public string delivery_line_2
        {
            get
            {
                return this.delivery_line_2Field;
            }
            set
            {
                this.delivery_line_2Field = value;
            }
        }
        public string city_state_zip
        {
            get
            {
                return this.city_state_zipField;
            }
            set
            {
                this.city_state_zipField = value;
            }
        }
        public string ca_codes
        {
            get
            {
                return this.ca_codesField;
            }
            set
            {
                this.ca_codesField = value;
            }
        }
        public string ca_filler
        {
            get
            {
                return this.ca_fillerField;
            }
            set
            {
                this.ca_fillerField = value;
            }
        }
        public string batchname
        {
            get
            {
                return this.batchnameField;
            }
            set
            {
                this.batchnameField = value;
            }
        }
    }


    [XmlRootAttribute(Namespace = "http://www.intelligentsearch.com/HostedWebServices/", IsNullable = false, ElementName = "WsCorrectAddress")]
    public class USPSSoapResponseIS : Location
    {
        public string AddressLine2 { get; set; }
        public string StreetNumber { get; set; }
        public string PreDirectional { get; set; }
        public string StreetName { get; set; }
        public string StreetSuffix { get; set; }
        public string PostDirectional { get; set; }
        public string SecondaryDesignation { get; set; }
        public string SecondaryNumber { get; set; }
    }
}
