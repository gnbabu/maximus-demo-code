using PDMSWebAPI.Models;
using System.ServiceModel;

namespace PDMSWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserInfo" in both code and config file together.    
    [ServiceContract(Namespace = "http://Maximus.OHPNM.Services", Name = "UserInfo")]
    public interface IUserInfo
    {
        [OperationContract]
        UserDetailModel GetUserInfoTKN(string AuthToken);

        [OperationContract]
        UserDetailModel GetUserInfo(string UserName, string EncryptedPassword);

        [OperationContract]
        UserAgentProvAssnModel GetUserAgentProvAssnDataTKN(string AuthToken);

        [OperationContract]
        UserAgentProvAssnModel GetUserAgentProvAssnData(string UserName, string WebRoleNameSearch, string FilterRole, string CreatedAfter = "");

        [OperationContract]
        string EchoSoapRequest(int input);

    }


}
