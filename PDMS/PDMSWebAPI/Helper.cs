using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace PDMSWebAPI
{
    public static class Helper
    {

        public static string TrimAndReduce(this string str)
        {
            if (!string.IsNullOrEmpty(str))
                return ConvertWhitespacesToSingleSpaces(str).Trim();
            else
                return "";
        }

        public static string ConvertWhitespacesToSingleSpaces(this string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        // Return TRUE if the dataset has rows
        public static bool HasRows(DataSet ds)
        {
            return (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
        }

        // Return TRUE if the datatable has rows
        public static bool HasRows(DataTable dt)
        {
            if (dt == null) return false;
            if (dt.Rows.Count == 0) return false;
            return true;
        }


        /// <summary>
        /// Gets an integer from the datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="dr">data row to get data from</param>
        /// <returns>integer from the data row</returns>
        public static int GetInt(string elementName, DataRow dr)
        {
            int returnValue = 0;
            if (!dr.IsNull(elementName))
            {
                returnValue = Convert.ToInt32(dr[elementName]);
            }
            return returnValue;
        }

        /// <summary>
        /// Gets an integer from the string
        /// </summary>
        /// <returns>integer from the string</returns>
        public static Int64 GetInt(string input)
        {
            Int64 rtn = 0;
            try
            {
                rtn = Convert.ToInt64(input);
            }
            catch { }
            return rtn;
        }

        // Test for non-existence of the Column Name in the DataRow
        public static bool ColumnExists(string elementName, DataRow dr)
        {
            bool rtn = false;
            try
            {
                // Simple test that if fails then it does not exist
                if (!dr.IsNull(elementName)) { }
                rtn = true;
            }
            catch { }
            return rtn;
        }

        /// <summary>
        /// Gets a string from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>string from the data row</returns>
        public static string GetString(string elementName, DataRow dr)
        {
            string returnValue = "";
            if (!dr.IsNull(elementName))
            {
                returnValue = HttpUtility.HtmlDecode(dr[elementName].ToString());
                returnValue = returnValue.Replace("''", "'");
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a boolean from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>boolean from datarow</returns>
        public static bool GetBool(string elementName, DataRow dr)
        {
            bool returnValue = false;
            if (!dr.IsNull(elementName))
            {
                returnValue = Convert.ToBoolean(dr[elementName]);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a decimal from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>boolean from datarow</returns>
        public static decimal GetDecimal(string elementName, DataRow dr)
        {
            decimal returnValue = 0.0M;
            if (!dr.IsNull(elementName))
            {
                returnValue = Convert.ToDecimal(dr[elementName]);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a Date from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>string representation of date or empty if no date</returns>
        public static string GetDate(string elementName, DataRow dr)
        {
            string rtn = string.Empty;
            if (!dr.IsNull(elementName)) rtn = Convert.ToDateTime(dr[elementName]).ToString("MM/dd/yyyy");
            return rtn;
        }

        /// <summary>
        /// Gets a DateTime from a datarow
        /// </summary>
        /// <param name="elementName">name of the data element to get</param>
        /// <param name="ds">data row to get data from</param>
        /// <returns>boolean from datarow</returns>
        public static DateTime GetDateTime(string elementName, DataRow dr)
        {
            DateTime returnValue = new DateTime(1753, 1, 1);
            if (!dr.IsNull(elementName))
            {
                returnValue = Convert.ToDateTime(dr[elementName]);
            }

            return returnValue;
        }

        public static Guid GetUserId(string username)
        {
            MembershipUser usr = Membership.GetUser(username);
            if (usr == null) return Guid.Empty;
            else return new Guid(usr.ProviderUserKey.ToString());
        }
    }
}