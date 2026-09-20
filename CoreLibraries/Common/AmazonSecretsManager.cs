using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;
using Corp.Core.Libraries.AcknowledgmentService;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Summary description for Class1
/// </summary>
public class AmazonSecretsManager
{
    public AmazonSecretsManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private readonly IAmazonSecretsManager secretsManager;
    private readonly SecretsManagerCache cache;

    public AmazonSecretsManager(string region)
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


    public Dictionary<string, string> GetSuperSecretPasswordSync(string secretId, string region)
    {
        var config = new AmazonSecretsManagerConfig { RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region) };
        var client = new AmazonSecretsManagerClient(config);

        var request = new GetSecretValueRequest
        {
            SecretId = secretId
        };

        var response = client.GetSecretValue(request); // Synchronous call

        if (string.IsNullOrEmpty(response.SecretString))
            throw new Exception("Secret string is empty.");

        return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.SecretString);
    }

}