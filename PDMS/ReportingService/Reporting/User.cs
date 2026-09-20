using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Service.Reporting
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RptUser
    {
        public string ContactNumber { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveDirectoryUser { get; set; }
        public string Lastname { get; set; }
        public int UserStatus { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool IsAzureAdUser { get; set; }
        public string UserDomain { get; set; }
        public string Avatar { get; set; }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RptNewUser
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string ExternalId { get; set; }
    }
}
