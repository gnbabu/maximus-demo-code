using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Services.Extensions
{
    public static class StringExtension
    {
        public static DateTime? ParseAsDateTime(this string dateString, DateTimeKind kind)
        {
            if (string.IsNullOrEmpty(dateString)) return null;

            if (DateTime.TryParse(dateString, out DateTime date)) return DateTime.SpecifyKind(date, kind);

            return null;
        }

        public static string MaskLeftChars(this string input, char chr = '*', int charCount = 4)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            if (input.Length <= charCount)
                charCount = input.Length;
            return input.AsSpan().Slice(charCount).ToString().PadLeft(input.Length, chr);
        }
    }
}
