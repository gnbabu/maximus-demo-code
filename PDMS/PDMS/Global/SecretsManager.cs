using System.Collections.Generic;
using Amazon.SecretsManager;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon;
using System;
using MAXIMUS.Core.Libraries;

/// <summary>
/// Summary description for Class1
/// </summary>
public class SecretsManager
{
    public SecretsManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private readonly IAmazonSecretsManager secretsManager;
    private readonly SecretsManagerCache cache;

    public SecretsManager(string region)
    {        
        var config = new AmazonSecretsManagerConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(region), Timeout = TimeSpan.FromMilliseconds(Convert.ToDouble(AppSettings.Get("IOPResponseTimeOut"))) };
        this.secretsManager = new AmazonSecretsManagerClient(config);
        this.cache = new SecretsManagerCache(this.secretsManager);
    }

    public void Dispose()
    {
        this.secretsManager.Dispose();
        this.cache.Dispose();
    }

    public async Task<Dictionary<string, string>> GetSuperSecretPassword(string secretId)
    {
        var sec = await this.cache.GetSecretString(secretId);
        var jo = Newtonsoft.Json.Linq.JObject.Parse(sec);
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(sec);
    }
}