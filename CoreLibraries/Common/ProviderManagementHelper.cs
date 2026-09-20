using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Corp.Core.Libraries
{
    public class ProviderManagementHelper
    {
        public static string GetString(string elementName, DataRow dr)
        {
            string returnValue = "";
            if (!dr.IsNull(elementName))
            {
                returnValue = HttpUtility.HtmlDecode(dr[elementName].ToString());
                returnValue = returnValue.Replace("''", "'");
            }
            returnValue = TrimSpacesBetweenString(returnValue);
            return returnValue;
        }

        public static string PaddedIncrementor(int startValue)
        {
            int returnValueInt = startValue;
            string returnValueStr = string.Empty;
            returnValueInt = Convert.ToInt32(returnValueInt);
            returnValueStr = String.Format("{0:D3}", returnValueInt);
            return returnValueStr;
        }

        public static string GetDecimalString(string elementName, DataRow dr)
        {
            decimal returnValueDecimal = 0.0M;
            int returnValueInt = 0;
            string returnValueStr = "";
            if (!dr.IsNull(elementName))
            {
                returnValueDecimal = Convert.ToDecimal(dr[elementName]);
            }
            returnValueInt = Convert.ToInt32(returnValueDecimal);
            returnValueStr = String.Format("{0:D3}", returnValueInt);
            return returnValueStr;
        }

        public static string GetStringPercentage(string elementName, DataRow dr)
        {
            string returnValue = "";
            if (!dr.IsNull(elementName))
            {
                returnValue = HttpUtility.HtmlDecode(dr[elementName].ToString());
                returnValue = returnValue.Replace("''", "'");
            }
            returnValue = TrimSpacesBetweenString(returnValue);
            returnValue = String.Format("{0:D3}", 5);
            return returnValue;
        }


        public static string GetStringDateTime(string elementName, DataRow dr)
        {
            string returnValue = "";
            DateTime date;
            if (!dr.IsNull(elementName))
            {
                returnValue = HttpUtility.HtmlDecode(dr[elementName].ToString());
                returnValue = returnValue.Replace("''", "'");
                date = Convert.ToDateTime(returnValue, CultureInfo.InvariantCulture);
                returnValue = date.ToString("yyyy-MM-dd'T'HH:mm:ss");
            }

            return returnValue;
        }

        public static int GetInt(string elementName, DataRow dr)
        {
            int returnValue = 0;
            if (!dr.IsNull(elementName))
            {
                returnValue = Convert.ToInt32(dr[elementName]);
            }
            return returnValue;
        }

        public static string GetStringDateTime(DateTime dateTime)
        {
            string returnValue = "";
            DateTime date;
            if (!string.IsNullOrEmpty(dateTime.ToString()))
            {
                returnValue = dateTime.ToString();
                returnValue = returnValue.Replace("''", "'");
                date = Convert.ToDateTime(returnValue, CultureInfo.InvariantCulture);
                returnValue = date.ToString("yyyy-MM-dd'T'HH:mm:ss");
            }

            return returnValue;
        }

        public static string TrimSpacesBetweenString(string s)
        {
            string str = s;
            str = str.Trim();
            str = Regex.Replace(str, @"\s+", " ");
            return str;
        }

        public static string GetUniqueKey(int size)
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(size)
                .ToList().ForEach(e => builder.Append(e));
            string id = builder.ToString();
            return id;
        }
    }
}
