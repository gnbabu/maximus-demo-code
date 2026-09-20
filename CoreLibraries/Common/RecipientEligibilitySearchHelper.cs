using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace Corp.Core.Libraries
{
   public class RecipientEligibilitySearchHelper
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
        public static string TrimSpacesBetweenString(string s)
        {
            string str = s;
            str = str.Trim();
            str = Regex.Replace(str, @"\s+", " ");
            return str;
        }
        public static string GetStringDateTime(string elementName)
        {
            string returnValue = "";
            DateTime date;
            if (!string.IsNullOrEmpty(elementName))
            {
                returnValue = HttpUtility.HtmlDecode(elementName.Trim());
                returnValue = returnValue.Replace("''", "'");
                date = Convert.ToDateTime(returnValue, CultureInfo.InvariantCulture);
                returnValue = date.ToString("yyyy-MM-dd'T'HH:mm:ss");
            }

            return returnValue;
        }
        public static string GetStringDate(string elementName)
        {
            string returnValue = "";
            DateTime date;
            if (!string.IsNullOrEmpty(elementName))
            {
                returnValue = HttpUtility.HtmlDecode(elementName.Trim());
                returnValue = returnValue.Replace("''", "'");
                date = Convert.ToDateTime(returnValue, CultureInfo.InvariantCulture);
                returnValue = date.ToString("yyyyMMdd");
            }
            return returnValue;
        }

        public static string GetDateFormat(string elementName)
        {
            string returnValue = "";
            if (!string.IsNullOrEmpty(elementName) && elementName.Trim().Length  == 8)
            {
                returnValue = elementName[4].ToString() + elementName[5].ToString() + "/"+
                    elementName[6].ToString() + elementName[7].ToString() + "/"+elementName[0].ToString() +
                    elementName[1].ToString() + elementName[2].ToString() + elementName[3].ToString();
            }
            return returnValue;
        }



    }
}
