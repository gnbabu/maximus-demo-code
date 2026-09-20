using ReportingService.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Services.Interfaces
{
    public interface IAuthenticationService
    {
        User GetAuthenticatedUser();
        Task<User> GetAuthenticatedUserAsync();
    }
}
