using ReportingService.Services.Interfaces;
using System.Threading.Tasks;

namespace ReportingService.Services.Security
{
    public  class CapabilityService :ICapabilityService
    {
        public Task<bool> UserHasCapabilityAsync(User user, string capability)
        {
            return Task.FromResult(true);
        }
    }
}
