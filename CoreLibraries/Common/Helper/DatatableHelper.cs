using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Corp.Core.Libraries
{
    public static class DatatableHelper
    {
        public static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (pro.PropertyType.IsEnum)
                        {
                            pro.SetValue(obj, Enum.Parse(pro.PropertyType, Convert.ToString(dr[column])));
                        }
                        else
                        {
                            if (dr[column] != DBNull.Value)
                            {
                                pro.SetValue(obj, Convert.ChangeType(dr[column], Type.GetType(pro.PropertyType.ToString())), null);
                            }
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
