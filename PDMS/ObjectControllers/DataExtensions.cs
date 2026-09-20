using System;
using System.Collections.Generic;
using System.Linq;

namespace MAXIMUS.Controllers.PDMS
{
    public static class DataExtensions
    {

        public static string GetString(this string value, string name, bool required = false)
        {
            if (string.IsNullOrEmpty(value) && required)
            {
                throw new Exception(string.Format("Value of {0} cannot be null or empty", name));
            }

            if (string.IsNullOrEmpty(value))
                return "";

            return value.Trim();
        }

        public static int GetInt(this string value, string name, bool required = false)
        {
            if (string.IsNullOrEmpty(value) && required)
            {
                throw new Exception(string.Format("Value of {0} cannot be null or empty", name));
            }

            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            return Convert.ToInt32(value.Trim());
        }

        public static int GetDataInt(this object value)
        {
            int returnValue = 0;
            if (value != null)
            {
                returnValue = Convert.ToInt32(value);
            }
            return returnValue;
        }

        public static string GetDataString(this object value)
        {
            string returnValue = null;
            if (value != null)
            {
                returnValue = Convert.ToString(value);
            }
            return returnValue;
        }

        public static bool GetDataBool(this object value)
        {
            bool returnValue = false;
            if (value != null)
            {
                returnValue = Convert.ToBoolean(value);
            }
            return returnValue;
        }

        public static List<string> GetDataList(this object value)
        {
            List<string> returnValue = null;
            if (value != null)
            {
                returnValue = Convert.ToString(value).Split(',').ToList();
            }
            return returnValue;
        }

        public static Nullable<DateTime> GetDataDateTime(this object value)
        {
            string valueToCheck = Convert.ToString(value);
            Nullable<DateTime> returnValue = null;
            try
            {
                if (!string.IsNullOrEmpty(valueToCheck))
                {
                    returnValue = Convert.ToDateTime(valueToCheck);

                }
            }
            catch (Exception)
            {
                returnValue = null;
            }
            return returnValue;
        }


    }
}
