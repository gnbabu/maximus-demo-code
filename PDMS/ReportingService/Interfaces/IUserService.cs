using ReportingService.Services.DataModels;
using System.Threading.Tasks;

namespace ReportingService.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserRole[]> GetUserRolesAsync(int userId);
    }
}
