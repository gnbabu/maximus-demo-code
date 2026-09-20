using MAXIMUS.Core.Libraries;
using System;
using MemoryCache = System.Runtime.Caching.MemoryCache;


namespace Corp.Core.Libraries
{
    public static class ConfigCache
    {
        private static readonly MemoryCache _cache = MemoryCache.Default;

        public static bool HSWebAPITesting
        {
            get
            {
                var value = _cache["HSWebAPITesting"];
                if (value == null)
                {
                    value = AppSettings.Get("HSWebAPITesting", "false").ToLower() == "true";
                    _cache.Set("HSWebAPITesting", value, DateTimeOffset.Now.AddMinutes(5)); // expires in 5 minutes
                }
                return (bool)value;
            }
        }

        public static int HSTimeOut
        {
            get
            {
                var value = _cache["HSTimeOut"];
                if (value == null)
                {
                    value = Convert.ToInt32(AppSettings.Get("HSResponseTimeOut", "5000"));
                    _cache.Set("HSTimeOut", value, DateTimeOffset.Now.AddMinutes(5)); // expires in 5 minutes
                }
                return (int)value;
            }
        }

        public static void RefreshConfig()
        {
            _cache.Remove("HSWebAPITesting");
            _cache.Remove("HSTimeOut");
        }

    }

}
