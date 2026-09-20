using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;
using System.Web.Security;

using CON = MAXIMUS.Core.Libraries.Constants;

/// <summary>
/// Convenience functions
/// </summary>
public class ObjectControllerHelper
{
	private ObjectControllerHelper()
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
            string msg = ex.Message;
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

    public static string GetKeyValueString(Dictionary<string, object> dict)
    {
        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<string, object> entry in dict)
        {
            sb.Append(entry.Key + ":" + entry.Value.ToString() + ",");
        }

        string keyValue = sb.ToString();

        if (!string.IsNullOrEmpty(keyValue))
        {
            keyValue = keyValue.Remove(keyValue.Length - 1); //remove the final comma
        }

        return keyValue;
    }
    // Get the aspnet UserId given the Username
  
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
        string returnValue = string.Empty;
        //To handle empty records from data source
        if (dr == null)
            return string.Empty;

        if (!dr.IsNull(elementName))
        {
            returnValue = HttpUtility.HtmlDecode(dr[elementName].ToString());
            returnValue = returnValue.Replace("''", "'");
        }

        return returnValue;
    }
    public static string GetStringForSequence(string elementName, DataRow dr,string sequencename)
    {
        string returnValue = string.Empty;
        //To handle empty records from data source
        if (dr == null)
            return string.Empty;

        if (!dr.IsNull(elementName))
        {
            returnValue = HttpUtility.HtmlDecode(dr[elementName].ToString());
            returnValue = returnValue.Replace("''", "'");
        }
        else
        {
            if(sequencename == "saqproviderinfo")
                return "1";
            if (sequencename == "saqprimarycontactinfo")
                return "2";
            if (sequencename == "saqcrdentailcontact")
                return "3";
            if (sequencename == "saqcpccontactinfo")
                return "4";
            if (sequencename == "saqofficeinfo")
                return "5";
            if (sequencename == "saqofficeind")
                return "6";
            if (sequencename == "saqPhysicalAddr")
                return "7";
            if (sequencename == "saqCorrespAddr")
                return "8";
            if (sequencename == "saqBillingAddr")
                return "9";
            if (sequencename == "saqOTHERSERVICE")
                return "10";
            if (sequencename == "saqWorkHistory")
                return "11";
            if (sequencename == "saqSiteVisit")
                return "12";
            if (sequencename == "saqNursingFacility")
                return "13";
            if (sequencename == "saqOtherAddr")
                return "14";
            if (sequencename == "saqHomeOffice")
                return "15";
            if (sequencename == "saq1099Add")
                return "16";
            if (sequencename == "saqLongTermAddress")
                return "17";
            if (sequencename == "saqSPECIALTY")
                return "18";
            if (sequencename == "saqADDLSPECIALTY")
                return "19";
            if (sequencename == "saqTAXONOMY")
                return "20";
            if (sequencename == "saqADDLTAXONOMY")
                return "21";
            if (sequencename == "saqLICENSE")
                return "22";
            if (sequencename == "saqBOARD")
                return "23";
            if (sequencename == "saqCLIA")
                return "24";
            if (sequencename == "saqMCQ")
                return "25";
            if (sequencename == "saqCDS")
                return "26";
            if (sequencename == "saqDEA")
                return "27";
            if (sequencename == "saqBHINFO")
                return "28";
            if (sequencename == "saqMEDICAREDETAIL")
                return "29";
            if (sequencename == "saqMEDICAID")
                return "30";
            if (sequencename == "saqINSURANCE")
                return "31";
            if (sequencename == "saqDNDENTALLICENSE")
                return "32";
            if (sequencename == "saqDENTALLICENSE")
                return "33";
            if (sequencename == "saqVISIONCERT")
                return "34";
            if (sequencename == "saqPHARMACY")
                return "35";
            if (sequencename == "saqCPRCERT")
                return "36";
            if (sequencename == "saqNURSINGCERT")
                return "37";
            if (sequencename == "saqCATEGORYOFSVC")
                return "38";
            if (sequencename == "saqAFFILIATIONS")
                return "39";
            if (sequencename == "saqEDUCATION")
                return "40";
            if (sequencename == "saqMALPRACTICE")
                return "41";
            if (sequencename == "saqCONTRACT")
                return "42";
            if (sequencename == "saqWORKFLOW")
                return "43";
            if (sequencename == "saqOwnerInfo")
                return "44";
            if (sequencename == "saqW9")
                return "45";
            if (sequencename == "saqWAIVERSVC")
                return "46";
            if (sequencename == "saqHOUSEHOLDMEMBERS")
                return "47";
            if (sequencename == "saqApplicationFee")
                return "48"; 
            if (sequencename == "saqidOprator")
                return "49";
            if (sequencename == "saqDME")
                return "50";
            if (sequencename == "saqREQUIREDDOCUMENTS")
                return "51";
            if (sequencename == "saqPracticePartnership")
                return "52";
            if (sequencename == "saqohoAgreement")
                return "53";
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
        if (dr != null && !dr.IsNull(elementName))
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
    /// Gets a DateTime from a datarow, defaulting to today
    /// </summary>
    /// <param name="elementName">name of the data element to get</param>
    /// <param name="ds">data row to get data from</param>
    /// <returns>boolean from datarow</returns>

    public static DateTime GetDateTimeDefaultToday(string elementName, DataRow dr)
    {
        DateTime returnValue = DateTime.Today;
        if (!dr.IsNull(elementName))
        {
            returnValue = Convert.ToDateTime(dr[elementName]);
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

    public static bool RequirePaperNotice(string recipients, string[] psEmailTypes)
    {
        bool isPSEmailType = false;

        if (string.IsNullOrEmpty(recipients))
        {
            isPSEmailType = true;
        }
        //else  // OHPNM1-1789 - Comment out this as there is no such requirement in OH to send paper email for an internal address
        //{
        //    // If email address is maximus.com or nebraska.com send paper mail
        //    foreach (string psEmailType in psEmailTypes)
        //    {
        //        if (recipients.Contains(psEmailType))
        //        {
        //            isPSEmailType = true;
        //            break;
        //        }
        //    }
        //}
        return isPSEmailType;
    }
    public static bool IsUserInRestrictedServiceViewRole(string username)
    {
        bool viewRole = Roles.IsUserInRole(username, CON.UserRoleType.Administrator)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.EnrollmentSpecialist)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.ProviderServices)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.ReadOnly)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.ComplianceSpecialist)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.TechAdminMAX)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.LTCInitialReval);

        return viewRole;

    }

    #endregion "DataRow Methods"


    
}
