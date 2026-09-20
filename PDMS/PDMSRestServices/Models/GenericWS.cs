using System.ComponentModel.DataAnnotations;

namespace PDMSRestServices.Models
{
    public class GenericWS
    {
    }

    public class ResponseDetail
    {
        public string ResponseCode { get; set; }
        public string ResponseDescp { get; set; }
        public string ResponseBody { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorStackTrace { get; set; }
    }


    public class GenericWSModel
    {
        [Required(ErrorMessage = "Url is required.")]
        public string wsUrl { get; set; }

        [Required(ErrorMessage = "Host Name is required.")]
        public string hostName { get; set; }

        [Required(ErrorMessage = "Soap Action is required.")]
        public string soapAction { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Certificate Name is required.")]
        public string certificateName { get; set; }

        [Required(ErrorMessage = "WebService Request is required.")]
        public string requestWS { get; set; }
    }
}
