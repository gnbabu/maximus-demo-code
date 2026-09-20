using ReportingService.Services.DataModels;
using ReportingService.Services.Interfaces;
using System.Threading.Tasks;

namespace ReportingService.Services.Security
{
    public class UserService : IUserService
    {
        public Task<UserRole[]> GetUserRolesAsync(int userId)
        {
            return Task.FromResult(new UserRole[] { new UserRole { Id = 1, Name = "Admin" } });
        }
    }
}
