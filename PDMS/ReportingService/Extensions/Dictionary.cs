using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportingService.Services.Extensions
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// Returns the dictionary element with the provided key value. If the key is not found in the dictionary the default value is returned i.e. null for reference types.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Item<K, T>(this Dictionary<K, T> self, K key)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            T value;
            if (!self.TryGetValue(key, out value))
                return default(T);

            return value;
        }

        public static void AddOrReplace<K, T>(this Dictionary<K, T> self, K key, T value)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            if (self.ContainsKey(key))
                self[key] = value;
            else
                self.Add(key, value);
        }
    }
}
