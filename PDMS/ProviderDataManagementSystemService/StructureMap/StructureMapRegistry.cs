using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Services.PDMS.Contracts;
using MAXIMUS.Services.PDMS.Validation;
using MAXIMUS.Services.PDMS.Validation.RequestValidator;
using StructureMap.Configuration.DSL;

namespace MAXIMUS.Services.PDMS.StructureMap
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
            {
            For<IValidator<AuthenticateUserRequest>>().Use<AuthenticateUserRequestValidator>();
            For<IAuthenticationController>().Use<AuthenticationController>();
        }
    }
}