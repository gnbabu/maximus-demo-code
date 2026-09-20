using ReportingService.Services.Security;
using System.Threading.Tasks;

namespace ReportingService.Services.Interfaces
{
    public interface ICapabilityService
    {
        Task<bool> UserHasCapabilityAsync(User user, string capability);
    }
}
