using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static ReportingService.Services.Security.SecretsMgrExtensions;

namespace ReportingService.Services.Security
{
    public class SecretsMgrOptions
    {
        public SecretsMgrOptions()
        {
            SecretsMgrSettings = new SecretsMgrSettings();
        }

        /// <summary>
        /// fully qualified type of secrets manager repository
        /// </summary>
        public string RepositoryType { get; set; }

        /// <summary>
        /// Repository Assembly
        /// </summary>
        public string RepositoryAssembly { get; set; }

        /// <summary>
        /// Settings
        /// </summary>
        public SecretsMgrSettings SecretsMgrSettings { get; set; }
    }

    public class SecretsMgr
    {
        ISecretsMgrRepository _repo;
        SecretsMgrSettings _settings;

        public SecretsMgr(ISecretsMgrRepository repo)
        {
            _repo = repo;

        }

        public virtual string GetSecretString(string secretKey)
        {
            return _repo.GetSecretString(secretKey);
        }

        public virtual T GetSecret<T>(string secretKey)
        {
            return _repo.GetSecret<T>(secretKey);
        }



    }

    public static class SecretsMgrExtensions
    {
        /// <summary>
        /// register secrets manager with dependancy injection based on appsettings value configuration
        /// </summary>
        /// <param name="services">DI service container</param>
        /// <param name="config">Configuration provider</param>
        /// <returns></returns>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSecretsMgr(
            this Microsoft.Extensions.DependencyInjection.IServiceCollection services,
            IConfiguration config)
        {
            var configSection = config.GetSection("SecretsMgr");

            var options = new SecretsMgrOptions();
            configSection.Bind(options);

            var settings = options.SecretsMgrSettings;

            var assembly = GetAssemblyByName(options != null ? options.RepositoryAssembly : "");

            services.AddScoped<SecretsMgrSettings>(x => settings);

            services.AddScoped(
                typeof(ISecretsMgrRepository),
                Type.GetType(options != null ? options.RepositoryType : "") ?? typeof(AppSecretsRepo)
            );

            services.AddScoped<SecretsMgr>();

            return services;
        }

        private static Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SingleOrDefault(assembly =>
                    string.Equals(assembly.GetName().Name ?? "", name, StringComparison.Ordinal));
        }

        public interface ISecretsMgrRepository
        {
            string GetSecretString(string secretName);
            Task<string> GetSecretStringAsync(string secretName);

            T GetSecret<T>(string secretName);
            Task<T> GetSecretAsync<T>(string secretName);
        }

    }
}