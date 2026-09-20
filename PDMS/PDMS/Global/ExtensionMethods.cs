using System;
using System.Data;
using System.Web.UI.WebControls;

namespace CustomControls
{
    public static class ExtensionMethods
	{
		public static ListItem FindByValueCaseInsensitive(this ListControl ddl, string value)
		{
			foreach (ListItem item in ddl.Items)
			{
				if (string.Compare(value, item.Value, true) == 0)
				{
					return item;
				}
			}

			return null;
		}
		public static int GetRowIndexOfDateKey<T>(this GridView gridView, T key)
		{
			int index = 0;

			foreach (DataKey obj in gridView.DataKeys)
			{
				if (((T)obj.Value).Equals(key))
				{
					return index;
				}

				index++;
			}

			return -1;
		}

		public static int GetRowIndexOfDateKey<T>(this GridView gridView, T key, string keyName)
		{
			int index = 0;

			foreach (DataKey obj in gridView.DataKeys)
			{
				if (((T)obj.Values[keyName]).Equals(key))
				{
					return index;
				}

				index++;
			}

			return -1;
		}


		public static bool SelectRowByDataKey<T>(this GridView gridView, T key)
		{
			int selectedIndex = gridView.GetRowIndexOfDateKey<T>(key);

			if (selectedIndex >= 0)
			{
				gridView.SelectRow(selectedIndex);
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool SelectRowByDataKey<T>(this GridView gridView, T key, string keyName)
		{
			int selectedIndex = gridView.GetRowIndexOfDateKey<T>(key, keyName);

			if (selectedIndex >= 0)
			{
				gridView.SelectRow(selectedIndex);
				return true;
			}
			else
			{
				return false;
			}
		}

		public static T GetValue<T>(this DataRow row, string columnName)
		{
			var temp = row[columnName];

			if (temp == DBNull.Value)
			{
				return default(T);
			}
			else
			{
				return (T)temp;
			}
		}

		public static string GetString(this DataRow row, string columnName)
		{
			object temp = row[columnName];

			if (temp == DBNull.Value)
			{
				return null;
			}
			else
			{
                // TODO: We shouldnt need to do this. There is some issue in SSDMF 
                // that needs to be fixed. some datetime are coming to be converted to string
                if (temp is DateTime)
                    return temp.ToString();

				return (string)temp;
			}
		}
	}
}