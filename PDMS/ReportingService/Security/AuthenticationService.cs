using ReportingService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Services.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<User> GetAuthenticatedUserAsync()
        {
            var u = new User()
            {
                Id = 1,
                Username = "vishnuvnaatla",
                FirstName = "Vishnu V",
                LastName = "Naatla",
                Email = "vishnuvnaatla@maximus.com",
                UserGuid = Guid.NewGuid(),
                Active = true,
                RoleIds = new List<int>() { 1, 2 }

            };
            return Task.FromResult(u);
        }

        public User GetAuthenticatedUser()
        {
            var u = new User()
            {
                Id = 1,
                Username = "vishnuvnaatla",
                FirstName = "Vishnu V",
                LastName = "Naatla",
                Email = "vishnuvnaatla@maximus.com",
                UserGuid = Guid.NewGuid(),
                Active = true,
                RoleIds = new List<int>() { 1, 2 }

            };
            return u;
        }
    }
}
