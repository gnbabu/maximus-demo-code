using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibcoDocImportNetCore
{
    public static class DictionaryCache
    {
        private static ICacheManager<object> manager;
        //static DictionaryCache()
        //{
        //    manager = CacheFactory.Build<object>(p => p.WithSystemRuntimeCacheHandle());
        //}

        public static void Add(string key, object value)
        {
            manager.Add(new CacheItem<object>(key, value));
        }

        public static bool Exists(string key)
        {
            return manager.Exists(key);
        }

        public static void Add<T>(string key, T value)
        {
            manager.Add(new CacheItem<object>(key, value));
        }

        public static T Get<T>(string key)
        {
            return manager.Get<T>(key);
        }
    }
}
