using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Core.Libraries
{
    public class PriorAuthServiceHelper
    {

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

        // Return TRUE if the dataset has rows
        public static bool HasRows(DataSet ds)
        {
            return (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
        }

        public static string fillSpaces(int count)
        {
            string str = new string(' ', count);
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
