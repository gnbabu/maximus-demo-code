using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

/// <summary>
/// Convenience functions
/// </summary>
public class Helper
{
	private Helper()
	{
		//
		// Can't create this class
		//
	}

    public static bool IsDateNull(DateTime dt)
    {
        return (dt.Year == 1753 && dt.Month == 1 && dt.Day == 1);
    }

    public static bool IsValidDate(string dateEntry, bool required)
    {
        if (string.IsNullOrEmpty(dateEntry.Trim())) return !required;
        bool rtn = false;
        try
        {
            Convert.ToDateTime(dateEntry);
            rtn = true;
        }
        catch { }
        return rtn;
    }

    public static bool IsChecked(CheckBox chk)
    {
        if (chk == null) return false;
        return chk.Checked;
    }

    public static string FormatPhone(string phone)
    {
        string outPhone = "";
        if (phone.Length == 10)
        {
            outPhone = "(" + phone.Substring(0, 3) + ") " + phone.Substring(3, 3) + "-" + phone.Substring(6, 4);
        }
        else
        {
            outPhone = phone;
        }
        return outPhone;
    }

    // Add a red asterisk to the end of a label
    public static void AddAsterisk(Label lbl)
    {
        if (lbl == null) return;
        if (lbl.Text.IndexOf("*") == -1) lbl.Text += "<span class='bodyTextRed'>*</span>";
    }

    public static string FormatYesNo(int ynValue)
    {
        string returnValue = "Not Set";
        if (ynValue == 1)
        {
            returnValue = "Yes";
        }
        else if (ynValue == 2)
        {
            returnValue = "No";
        }

        return returnValue;
    }

    public static string FormatSSN(string ssn)
    {
        string outSSN = "";
        if (ssn.Length == 9)
        {
            outSSN = ssn.Substring(0, 3) + "-" + ssn.Substring(3, 2) + "-" + ssn.Substring(5, 4); ;
        }
        else
        {
            outSSN = ssn;
        }

        return outSSN;
    }
	public static Object ConvertNull(Object aobj_data)
	{
	    if (aobj_data == null || aobj_data == DBNull.Value)
	    {
		return ("");
	    }
	    else
	    {
		return (aobj_data);
	    }
   	 }

    public static int ConvertStrNullToInt32(Object aobj_data)
    {
      if (aobj_data == null || aobj_data == DBNull.Value)
      {
          return (-1);
      }
      else
      {
          return (Convert.ToInt32(aobj_data));
      }
    }


    public static byte[] Compress(byte[] data)
    {
        MemoryStream output = new MemoryStream();
        GZipStream gzip = new GZipStream(output,
        CompressionMode.Compress, true);
        gzip.Write(data, 0, data.Length);
        gzip.Close();
        return output.ToArray();
    }

    public static byte[] Decompress(byte[] data)
    {
        MemoryStream input = new MemoryStream();
        input.Write(data, 0, data.Length);
        input.Position = 0;
        GZipStream gzip = new GZipStream(input,
                          CompressionMode.Decompress, true);
        MemoryStream output = new MemoryStream();
        byte[] buff = new byte[64];
        int read = -1;
        read = gzip.Read(buff, 0, buff.Length);
        while (read > 0)
        {
            output.Write(buff, 0, read);
            read = gzip.Read(buff, 0, buff.Length);
        }
        gzip.Close();
        return output.ToArray();
    }

    public static int ConvertStringToInt32(string astr_data)
    {
        int retValue;
        try
        {
            retValue = Convert.ToInt32(astr_data);
        }
        catch
        {
            retValue = 0;
        }

        return (retValue);
    }
    public static string FormatInt32(int ai_data)
    {
        string retValue;

        if (ai_data <= 0)
        {
            retValue = "";
        }
        else
        {
            retValue = ai_data.ToString();
        }
        return (retValue);
    }

    public static string FormatDate2(string astr_data)
    {
        string returnValue = "";
        DateTime lobj_dt;
        try
        {
            lobj_dt = DateTime.Parse(astr_data);
            if (
                   (lobj_dt.Year == 1900 && lobj_dt.Month == 1 && lobj_dt.Day == 1) ||
                    (lobj_dt.Year == 1753 && lobj_dt.Month == 1 && lobj_dt.Day == 1)
                )
            {
                returnValue = "";
            }
            else
            {
                returnValue = lobj_dt.ToString("MM/dd/yyyy");
            }

        }
        catch
        {
            returnValue = "";
        }


        return returnValue;
    }

    public static DateTime FormatDate2(DateTime adt_data)
    {
        DateTime returnValue;
        try
        {
            if (
                   (adt_data.Year == 1900 && adt_data.Month == 1 && adt_data.Day == 1) ||
                    (adt_data.Year == 1753 && adt_data.Month == 1 && adt_data.Day == 1)
                )
            {
                returnValue = DateTime.Parse("1/1/1753");
            }
            else
            {
                returnValue = adt_data;
            }

        }
        catch (Exception ex)
        {
            returnValue = DateTime.Parse("1/1/1753");
        }


        return returnValue;
    }

    public static UInt32 HashString(string str)
    {
       UInt32 hash = 5381;

       for(int i = 0; i < str.Length; i++)
       {
          hash = ((hash << 5) + hash) + str[i];
       }

       return (hash & 0x7FFFFFFF);
    }

    // Loads a dropdown or listview
    public static void LoadList(ListControl listObj, object listData, string dataText, string dataValue, bool insertSelect)
    {
        listObj.DataSource = listData;
        listObj.DataTextField = dataText;
        listObj.DataValueField = dataValue;
        listObj.DataBind();
        if (insertSelect) listObj.Items.Insert(0, new ListItem(string.Empty, string.Empty));
    }

    // Get the aspnet UserId given the Username
    public static Guid GetUserId(string username)
    {
        MembershipUser usr = Membership.GetUser(username);
        if (usr == null) return Guid.Empty;
        else return new Guid(usr.ProviderUserKey.ToString());
    }

    // Get the aspnet UserId info given Username and opt
    public static string GetUserId(string username, int opt)
    {
        MembershipUser usr = Membership.GetUser(username);
        if (usr == null) return string.Empty;
        string rtn = string.Empty;
        switch (opt)
        {
            case 1:
                rtn = usr.Email;
                break;
        }
        return rtn;
    }

    // Return true if the user is in the role specified
    public static bool IsUserInRoll(string username, string roleName)
    {
        return Roles.IsUserInRole(username, roleName);
    }

    // Finds the control recursively
    //    Use:   TextBox txt = (TextBox)FindTheControl(CreateUserWizard1, "UserName");
    //           if (txt != null) txt.Focus();
    public static System.Web.UI.Control FindTheControl(System.Web.UI.Control theControl, string controlID)
    {
        if (theControl.ID == controlID) return theControl;

        foreach (System.Web.UI.Control ctl in theControl.Controls)
        {
            System.Web.UI.Control rtn = FindTheControl(ctl, controlID);
            if (rtn != null) return rtn;
        }
        return null;
    }

    // Get all the attribute statement of the control for the "attribute" (Ex: onclick)
    public static string GetAttributes(object ctl, string attribute)
    {
        if (ctl == null) return string.Empty;

        string rtn = string.Empty;
        if (ctl.GetType().Name.Equals("ListItem"))
        {
            ListItem itm = (ListItem)ctl;
            if (itm == null) return rtn;
            foreach (object key in itm.Attributes.Keys)
            {
                if (key.ToString() == attribute)
                {
                    rtn = itm.Attributes[attribute].ToString();
                    break;
                }
            }
        }
        else if (ctl.GetType().Name.Equals("Panel"))
        {
            Panel pnl = (Panel)ctl;
            if (pnl == null) return rtn;
            foreach (object key in pnl.Attributes.Keys)
            {
                if (key.ToString() == attribute)
                {
                    rtn = pnl.Attributes[attribute].ToString();
                    break;
                }
            }
        }
        return rtn;
    }

    /// <summary>
    /// Gets the Application Setting from the configuration file
    /// </summary>
    /// <returns></returns>
    public static string GetAppSetting(string id, string defaultValue)
    {
        string rtn = defaultValue;
        try
        {
            if (ConfigurationManager.AppSettings[id] != null) rtn = ConfigurationManager.AppSettings[id].ToString();
        }
        catch { }
        return rtn;
    }

    // Get the current Date/Time, first look in the configuration file to see it a test Date/Time value is there.
    public static DateTime GetDateTimeNow()
    {
        DateTime rtn = DateTime.Now;
        try
        {
            if (!string.IsNullOrEmpty(GetAppSetting("Test_DateTimeNow", string.Empty)))
            {
                rtn = Convert.ToDateTime(GetAppSetting("Test_DateTimeNow", string.Empty));
            }
        }
        catch { }
        return rtn;
    }

    /// <summary>
    /// Prepares the GridView for export to Excel
    /// </summary>
    /// <returns></returns>
    public static string PrepareGridViewforExcel(GridView gv)
    {
        gv.AllowPaging = false;
        gv.AllowSorting = false;

        StringBuilder sBuilder = new StringBuilder();
        System.IO.StringWriter sWriter = new System.IO.StringWriter();
        sBuilder.Length = 0;
        int splitMasterCaseNoColumnIndex = 0;
        // Write out the headers
        for (int i = 0; i < gv.Columns.Count; i++)
        {
            //"Case/Cat/Seq"
            if (gv.Columns[i].HeaderText.Split('/').Length == 3)
            {
                splitMasterCaseNoColumnIndex = i;
                foreach (string sHeader in gv.Columns[i].HeaderText.Split('/'))
                {
                    sBuilder.Append(sHeader.Replace("&nbsp;", "") + ",");
                }
            }
            else
            {
                sBuilder.Append(gv.Columns[i].HeaderText.Replace("&nbsp;", "") + ",");
            }
        }
        sWriter.WriteLine(sBuilder.ToString().Substring(0, sBuilder.ToString().Length - 1));
        sBuilder.Length = 0;

        foreach (GridViewRow gvr in gv.Rows)
        {
            for (int i = 0; i < gvr.Cells.Count; i++)
            {
                if (i == splitMasterCaseNoColumnIndex)
                {
                    string[] caseParts = gvr.Cells[i].Text.Replace("&nbsp;", "").Split('/');
                    foreach (string sCasePart in caseParts)
                    {
                        sBuilder.Append(sCasePart.Trim() + ",");
                    }
                }
                else
                {
                    string str = gvr.Cells[i].Text.Replace("&nbsp;", "");
                    str = str.Replace(",", "");     // Remove imbedded commas
                    sBuilder.Append(str + ",");
                }
            }
            string sLine = sBuilder.ToString().Substring(0, sBuilder.ToString().Length - 1);
            sLine = sLine.Replace(System.Environment.NewLine, " ");
            sWriter.WriteLine(sLine);
            sBuilder.Length = 0;
        }
        return sWriter.ToString();
    }

    // Get the decimal value out of the string
    public static decimal GetDecimal(string value)
    {
        try
        {
            NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowParentheses | 
                NumberStyles.AllowCurrencySymbol;
            decimal number = Decimal.Parse(value, style);
            return number;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Applies Pascal-casing to a string.
    /// </summary>
    /// <param name="srcString">The source string.</param>
    /// <returns>The given string with pascal-casing applied.
    /// Example: "MyNameIsJoe" will return "My Name Is Joe"
    /// </returns>
    public static string PascalCaseParse(string srcString)
    {
        bool firstChar = true;
        StringBuilder output = new StringBuilder();

        foreach (char c in srcString.ToCharArray())
        {
            if (firstChar)
            {
                output.Append(c);
                firstChar = false;
            }
            else
            {
                if (c.ToString().ToUpper() == c.ToString())
                {
                    output.Append(" ");
                }
                output.Append(c);
            }
        }
        return output.ToString();
    }

    // Get the RadioButtonList selected item, opt: 1=Text, 2=Value
    public static string GetRadioButtonListSelect(RadioButtonList rbl, int opt)
    {
        string rtn = string.Empty;
        foreach (ListItem itm in rbl.Items)
        {
            if (itm.Selected)
            {
                switch (opt)
                {
                    case 1:
                        rtn = itm.Text;
                        break;
                    case 2:
                        rtn = itm.Value;
                        break;
                }
            }
        }
        return rtn;
    }

    // Set the RadioButtonList selected item
    public static void SetRadioButtonListSelect(RadioButtonList rbl, string value)
    {
        foreach (ListItem itm in rbl.Items)
        {
            if (itm.Value.ToUpper() == value.ToUpper())
            {
                itm.Selected = true;
                break;
            }
        }
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

    #region "DataRow Methods"
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
    #endregion "DataRow Methods"

    public static void UpdateCommonColumns(DataRow dr, int recordStatus)
    {
        dr["RecordStatus"] = recordStatus;
        dr["LastModifiedDateTime"] = DateTime.Now;
        dr["LastModifiedUser"] = GetUserId(HttpContext.Current.User.Identity.Name);
    }
}
