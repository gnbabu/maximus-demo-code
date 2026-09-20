using MAXIMUS.Services.PDMS.Contracts;

namespace MAXIMUS.Services.PDMS.Validation.RequestValidator
{
    public class AuthenticateUserRequestValidator : IValidator<AuthenticateUserRequest>
    {
        public ValidationResult Validate(AuthenticateUserRequest authenticateUserRequest)
        {
            return Validate(authenticateUserRequest, false);
        }

        public ValidationResult Validate(AuthenticateUserRequest authenticateUserRequest, bool suppressWarnings)
        {
            var validationResult = new ValidationResult();
            if (string.IsNullOrEmpty(authenticateUserRequest.UserName))
            {
                validationResult.AddError("Username cannot be empty");
            }

            if (string.IsNullOrEmpty(authenticateUserRequest.Password))
            {
                validationResult.AddError("Password cannot be empty");
            }

            return validationResult;
        }
    }
}