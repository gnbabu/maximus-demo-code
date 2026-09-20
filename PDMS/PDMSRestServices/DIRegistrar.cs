using MainBoldReportsAPI.Web.Helper;
using MainBoldReportsAPI.Web.Services;
using ReportingService.Service.Reporting;
using ReportingService.Services.Configuration;
using ReportingService.Services.Data;
using ReportingService.Services.Interfaces;
using ReportingService.Services.Security;
using static ReportingService.Services.Security.SecretsMgrExtensions;

namespace MainBoldReportsAPI
{
    public class DIRegistrar
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            // Register application services here
            // e.g. services.AddScoped<IMyService, MyService>();
            services.AddHttpClient();
            services.AddScoped<FileVersionHash>();
            services.AddScoped<ISecretsMgrRepository>(_ => new AppSecretsRepo());
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<ReportingService.Service.Reporting.RptService>();
            services.AddScoped<ReportingUserAdminService>();
            services.AddScoped<IWorkContext, WebWorkContext>();
            services.AddScoped<ReportingService.Services.Interfaces.ICapabilityService, ReportingService.Services.Security.CapabilityService>();
            services.AddScoped<ReportingService.Services.Interfaces.IAuthenticationService, ReportingService.Services.Security.AuthenticationService>();
            services.AddScoped<ConnectionString>(_ => new ConnectionString(config.GetConnectionString("ConnectionString")));
            services.AddScoped<RestApiSettings>(_ => new RestApiSettings
            {
                BaseUrl = config["RestApiSettings:BaseUrl"],
            });
            services.AddScoped<BoldReportsSettings>(_ => new BoldReportsSettings
            {
                BaseUrl = config["BoldReports:BaseUrl"],
                SecretsMgrKey = config["BoldReports:SecretsMgrKey"],
                ServiceAccount = config["BoldReports:ServiceAccount"],
                SiteName = config["BoldReports:SiteName"]
            });
        }
    }

}