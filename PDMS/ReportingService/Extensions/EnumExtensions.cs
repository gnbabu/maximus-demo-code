using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Services.Extensions
{
    public static class EnumExtensions
    {
        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            return default(T);
        }


        /// <summary>
        /// Returns the description from the DescriptionAttribute associated with the supplied value,
        /// if present; otherwise, returns the value itself as a string.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Enum value</param>
        /// <returns>Friendly description of the value, or value itself converted to string</returns>
        public static string GetDescription<T>(this T value) where T : Enum
        {
            var field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }
            return value.ToString();
        }

        /// <summary>
        /// Converts the values of an enumeration type to an array of name-value pairs.
        /// </summary>
        /// <remarks>This method retrieves all values of the specified enumeration type, maps each value
        /// to its description (if available), and pairs it with its underlying numeric value. The resulting array can
        /// be used for populating a drop-down list, or other purposes requiring name-value pairs.</remarks>
        /// <typeparam name="TEnum">The enumeration type to process. Must be a struct and derive from <see cref="System.Enum"/>.</typeparam>
        /// <returns>An array of objects, where each object contains a pair of the enumeration field name and its underlying value.</returns>
        public static object[] EnumToNameValuePairs<TEnum>() where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enum");

            var values = Enum.GetValues(typeof(TEnum)).Cast<object>().ToList();

            var names = values
                .Select(v => GetDescription((Enum)v))
                .ToList();

            return names
                .Zip(values, (name, value) => new { name, value })
                .ToArray();
        }
    }
}
