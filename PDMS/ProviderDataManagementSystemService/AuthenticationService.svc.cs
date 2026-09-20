using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Services.PDMS.Contracts;
using MAXIMUS.Services.PDMS.Validation;
using System;
using System.Data;
using System.Linq;
using System.ServiceModel;

namespace MAXIMUS.Services.PDMS
{
    [ServiceBehavior]
    public class AuthenticationService : IAuthenticationService
    {
        private IAuthenticationController authenticationController;
        private IValidationFactory validationFactory;

        public AuthenticationService() : this(new AuthenticationController(), new ValidationFactory())
        {
        }

        public AuthenticationService(IAuthenticationController authenticationController, IValidationFactory validationFactory)
        {
            this.authenticationController = authenticationController;
            this.validationFactory = validationFactory;
        }

        public Contracts.AuthenticateUserResponse AuthenticateUser(Contracts.AuthenticateUserRequest authenticateUserRequest)
        {
            var authenticateUserResponse = new AuthenticateUserResponse();
            authenticateUserResponse.TransactionId = Guid.NewGuid().ToString();
            var validationResults = validationFactory.Validate<AuthenticateUserRequest>(authenticateUserRequest);

            if (!validationResults.Valid)
            {
                authenticateUserResponse.Authenticated = false;
                authenticateUserResponse.ErrorCode = 1;
                authenticateUserResponse.ResponseCode = -1;
                authenticateUserResponse.ErrorDescription = validationResults.Messages.Select(x => x.Message).Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                authenticateUserResponse.Authenticated = this.authenticationController.ValidateUser(authenticateUserRequest.UserName, authenticateUserRequest.Password);
                authenticateUserResponse.ResponseCode = 0;
                if (authenticateUserResponse.Authenticated)
                {
                    authenticateUserResponse.LoginTime = DateTime.UtcNow;
                    //TODO: demani Update logoff time
                    authenticateUserResponse.LogoffTime = DateTime.UtcNow.AddMinutes(10);

                    var dataSet = authenticationController.GetProviderInfoFromUserName(authenticateUserRequest.UserName);

                    foreach (DataRow medicaidRow in dataSet.Tables[0].Rows)
                    {
                        authenticateUserResponse.MedicaidIds.Add(medicaidRow["MedicaidID"] as string);
                    }

                    authenticateUserResponse.NPI = dataSet.Tables[1].Rows[0]["NPI"] as string;
                    authenticateUserResponse.TaxId = dataSet.Tables[2].Rows[0]["TaxId"] as string;

                    authenticateUserResponse.ProviderName = dataSet.Tables[3].Rows[0]["FirstName"] as string;
                    authenticateUserResponse.ProviderID = dataSet.Tables[4].Rows[0]["PARTY_ID"] as string;
                }
            }

            authenticateUserResponse.TimeStamp = DateTime.UtcNow;
            authenticationController.LogAuthenticateRequest(authenticateUserRequest.UserName, new Guid(authenticateUserResponse.TransactionId), authenticateUserResponse.Authenticated,
                authenticateUserResponse.TimeStamp, authenticateUserResponse.ResponseCode, authenticateUserResponse.ErrorCode,
                authenticateUserResponse.ErrorDescription); 
            
            return authenticateUserResponse;
        }
    }
}
