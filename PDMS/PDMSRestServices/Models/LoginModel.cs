using System.ComponentModel.DataAnnotations;

namespace PDMSRestServices.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserInfoModel
    {
        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
    }

    public class GetUserInfoModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayUserName { get; set; }
        public string OHID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

}