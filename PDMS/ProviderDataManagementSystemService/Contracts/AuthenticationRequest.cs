namespace MAXIMUS.Services.PDMS.Contracts
{
    public class AuthenticateUserRequest : Request
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}