using MAXIMUS.Core.Libraries;
using System;
using System.Runtime.Caching;

namespace Corp.Core.Libraries
{
    // using System;
    // using System.Runtime.Caching;
    public static class SettingCache
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;
        private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(5);

        public static string GetString(string key, string defaultValue, TimeSpan? ttl = null)
        {
            return Get(key, () => AppSettings.Get(key, defaultValue), ttl);
        }

        public static string GetString(string key, TimeSpan? ttl = null)
        {
            return Get(key, () => AppSettings.Get(key), ttl);
        }

        public static int GetInt(string key, int defaultValue, TimeSpan? ttl = null)
        {
            return Get(key, () => Convert.ToInt32(AppSettings.Get(key, defaultValue.ToString())), ttl);
        }

        public static bool GetBool(string key, bool defaultValue, TimeSpan? ttl = null)
        {
            return Get(key, () =>
            {
                var raw = AppSettings.Get(key, defaultValue ? "true" : "false");
                bool parsed;
                return bool.TryParse(raw, out parsed) ? parsed : defaultValue;
            }, ttl);
        }

        // ---- Core cache helper (single eval per key) ----
        private static T Get<T>(string key, Func<T> valueFactory, TimeSpan? ttl)
        {
            var value = Cache.Get(key);
            if (value != null && value is T) return (T)value;

            var newValue = valueFactory();
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.Add(ttl ?? DefaultTtl) };
            Cache.Set(key, newValue, policy);
            return newValue;
        }
    }
}
