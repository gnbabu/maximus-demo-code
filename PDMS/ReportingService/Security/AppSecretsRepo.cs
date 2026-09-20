using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static ReportingService.Services.Security.SecretsMgrExtensions;

namespace ReportingService.Services.Security
{
    public class AppSecretsRepo : ISecretsMgrRepository
    {
        private AppSecrets _appSecrets;

        public AppSecretsRepo()
        {
            var x = System.IO.File.ReadAllText(@"AppSecrets.json");
            var root = JsonNode.Parse(x);
            _appSecrets = new AppSecrets(root != null ? root["AppSecrets"] : null);
        }

        public AppSecretsRepo(string appSecrestsJSON)
        {
            var root = JsonNode.Parse(appSecrestsJSON);
            _appSecrets = new AppSecrets(root != null ? root["AppSecrets"] : null);
        }

        public T GetSecret<T>(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName)) return default(T);

            try
            {
                var value = _appSecrets.ContainsKey(secretName)
                    ? _appSecrets[secretName].ToString()
                    : "";

                return JsonSerializer.Deserialize<T>(value);
            }
            catch
            {
                return default(T);
            }
        }

#pragma warning disable CS1998
        public async Task<T> GetSecretAsync<T>(string secretName)
#pragma warning restore CS1998
        {
            if (string.IsNullOrWhiteSpace(secretName)) return default(T);

            try
            {
                var value = _appSecrets.ContainsKey(secretName)
                    ? _appSecrets[secretName].ToString()
                    : "";

                return JsonSerializer.Deserialize<T>(value);
            }
            catch
            {
                return default(T);
            }
        }

        public string GetSecretString(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName)) return "";

            return _appSecrets.ContainsKey(secretName)
                ? _appSecrets[secretName].ToString()
                : "";
        }

#pragma warning disable CS1998
        public async Task<string> GetSecretStringAsync(string secretName)
#pragma warning restore CS1998
        {
            if (string.IsNullOrWhiteSpace(secretName)) return "";

            return _appSecrets.ContainsKey(secretName)
                ? _appSecrets[secretName].ToString()
                : "";
        }

        public class AppSecrets
        {
            private JsonNode _secrets;

            public AppSecrets(JsonNode jsonNode)
            {
                _secrets = jsonNode;
            }

            public JsonNode this[string key]
            {
                get { return _secrets != null ? _secrets[key] : null; }
            }

            public bool ContainsKey(string key)
            {
                if (string.IsNullOrWhiteSpace(key) || _secrets == null) return false;
                return _secrets[key] != null;
            }
        }
    }
}