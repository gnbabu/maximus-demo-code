using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using static ReportingService.Services.Security.SecretsMgrExtensions;

namespace ReportingService.Services.Security
{
    public class AWSSecretsRepo : ISecretsMgrRepository
    {
        static IAmazonSecretsManager client = null;
        static SecretsManagerCache _secretCache;
        static readonly object _secretCacheLock = new object();
        private readonly AwsSecretsMgrSettings _awsSettings;

        public AWSSecretsRepo(SecretsMgrSettings settings)
        {
            var settingsString = JsonSerializer.Serialize((SecretsMgrSettings)settings);
            _awsSettings = JsonSerializer.Deserialize<AwsSecretsMgrSettings>(settingsString);

            if (client == null)
            {
                lock (_secretCacheLock)
                {
                    if (client == null)
                    {
                        Amazon.RegionEndpoint awsRegion = null;
                        if (!string.IsNullOrWhiteSpace(_awsSettings.AwsRegion))
                            awsRegion = Amazon.RegionEndpoint.GetBySystemName(_awsSettings.AwsRegion);

                        client = awsRegion != null
                            ? new AmazonSecretsManagerClient(awsRegion)
                            : new AmazonSecretsManagerClient();

                        var config = new SecretCacheConfiguration()
                        {
                            Client = client,
                            CacheItemTTL = _awsSettings.CacheTTLSeconds * 100
                        };

                        _secretCache = new SecretsManagerCache(client, config);
                    }
                }
            }
        }

        public class AwsSecretsMgrSettings : SecretsMgrSettings
        {
            public string StoreName
            {
                get { return this.ContainsKey("StoreName") ? (this["StoreName"] ?? "").ToString() : ""; }
                set { this["StoreName"] = value; }
            }

            public string AwsRegion
            {
                get { return this.ContainsKey("AwsRegion") ? (this["AwsRegion"] ?? "").ToString() : ""; }
                set { this["AwsRegion"] = value; }
            }

            public uint CacheTTLSeconds
            {
                get
                {
                    if (this.ContainsKey("CacheTTLSeconds"))
                    {
                        var val = this["CacheTTLSeconds"];
                        if (!string.IsNullOrWhiteSpace(val))
                            return Convert.ToUInt32(val);
                    }
                    return 3600;
                }
                set { this["CacheTTLSeconds"] = value.ToString(); }
            }
        }

        public T GetSecret<T>(string secretName)
        {
            return GetSecretAsync<T>(secretName).GetAwaiter().GetResult();
        }

        public async Task<T> GetSecretAsync<T>(string secretName)
        {
            if (_secretCache == null) return default(T);
            if (string.IsNullOrWhiteSpace(secretName)) return default(T);

            try
            {
                var secretJson = await _secretCache.GetSecretString(_awsSettings.StoreName);
                var secretDic = JsonSerializer.Deserialize<Dictionary<string, string>>(secretJson);

                if (secretDic == null || !secretDic.ContainsKey(secretName))
                    return default(T);

                return JsonSerializer.Deserialize<T>(secretDic[secretName]);
            }
            catch
            {
                return default(T);
            }
        }

        public string GetSecretString(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName)) return "";
            return GetSecretStringAsync(secretName).GetAwaiter().GetResult();
        }

        public async Task<string> GetSecretStringAsync(string secretName)
        {
            if (_secretCache == null) return "";
            if (string.IsNullOrWhiteSpace(secretName)) return "";

            var secretJson = await _secretCache.GetSecretString(_awsSettings.StoreName);
            var secretDic = JsonSerializer.Deserialize<Dictionary<string, string>>(secretJson);

            if (secretDic == null || !secretDic.ContainsKey(secretName))
                return "";

            return secretDic[secretName];
        }
    }
}