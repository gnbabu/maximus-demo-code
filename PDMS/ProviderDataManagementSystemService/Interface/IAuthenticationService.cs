using MAXIMUS.Services.PDMS.Contracts;
using System.ServiceModel;

namespace MAXIMUS.Services.PDMS
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        AuthenticateUserResponse AuthenticateUser(AuthenticateUserRequest value);
    }

}
