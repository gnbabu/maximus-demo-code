using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Services.Extensions
{
    public static class NotNullExtension
    {
        
        /// <summary>
        /// Ensures the extended class never returns null when dereferenced.
        /// Returns the extended class when not null or returns a new instance of the extended class when it is null
        /// </summary>
        /// <typeparam name="T">A class with a parameterless constructor</typeparam>
        /// <returns>The extended class instance or a new instance of the extended class</returns>
        public static T NotNull<T>(this T value) where T : new()
        {  
            if (value == null) return new T();
            return value;
        }

        /// <summary>
        /// Ensures the extended array never returns null when dereferenced.
        /// Returns the extended array when not null or returns a new instance of the extended array when it is null
        /// </summary>
        /// <typeparam name="T">A array with a parameterless constructor</typeparam>
        /// <returns>The extended array instance or a new instance of the extended array</returns>
        public static T[] NotNull<T>(this T[] value)
        {
            if (value == null) return new T[] { };
            return value;
        }

        /// <summary>
        /// Ensures the extended string never returns null when dereferenced.
        /// Returns the extended string when not null or returns a new instance of the extended string when it is null
        /// </summary>
        /// <typeparam name="T">A string with a parameterless constructor</typeparam>
        /// <returns>The extended string instance or a new instance of the extended string</returns>
        public static string NotNull(this string value)
        {
            if (value == null) return "";
            return value;
        }

        /// <summary>
        /// Ensures the extended struct never returns null when dereferenced.
        /// Returns the extended clastructss when not null or returns a new instance of the extended struct when it is null
        /// </summary>
        /// <typeparam name="T">A struct with a parameterless constructor</typeparam>
        /// <returns>The extended struct instance or a new instance of the extended struct</returns>
        public static T? NotNull<T>(this T? value) where T: struct
        {
            if (value == null) return default(T);
            return value;
        }
    }
}
