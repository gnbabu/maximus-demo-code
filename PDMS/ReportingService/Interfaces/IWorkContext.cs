using System.Threading.Tasks;

namespace ReportingService.Services.Security
{
    public interface IWorkContext
    {

        Task ClearUserCache();

        Task<User> GetCurrentUserAsync();

        Task<string> GetCurrentUserNameAsync();

    }
}