using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Xml.Linq;
using System.Diagnostics;


/// <summary>
/// Summary description for Helper
/// </summary>
public class CHelper
{
    private CHelper()
    {
        // can't instantiate
    }

    // null date definition
    public static DateTime GetNullDate()
    {
        return new DateTime(1753, 1, 1);
    }

    /// <summary>
    /// Determines if the two dates are the same day
    /// </summary>
    /// <param name="dt1">first date to compare</param>
    /// <param name="dt2">second date to compare</param>
    /// <returns>true if same day; false if not</returns>
    public static bool IsSameDay(DateTime dt1, DateTime dt2)
    {
        return (dt1.Month == dt2.Month) && (dt1.Day == dt2.Day) && (dt1.Year == dt2.Year);
    }

    public static string DisplayFormatSSN(string ssn)
    {
        string retSSN = "";
        if (ssn == null)
        {
            retSSN = "";
        }
        else
        {
            if (ssn.Trim().Length != 9)
            {
                retSSN = ssn;
            }
            else
            {
                retSSN = ssn.Substring(0, 3) + "-" + ssn.Substring(3, 2) + "-" + ssn.Substring(5, 4);
            }
        }
        return retSSN;
    }
    /// <summary>
    /// Formats the phone number if is 10 digits in (NNN) NNN-NNNN format.
    /// </summary>
    /// <param name="phone">Passed in phone number</param>
    /// <returns></returns>
    public static string DisplayFormatPhone(string phone)
    {
        string retPhone = "";
        if (phone.Trim().Length != 10)
        {
            retPhone = phone;
        }
        else
        {
            retPhone = "(" + phone.Substring(0, 3) + ") " + phone.Substring(3, 3) + "-" + phone.Substring(6, 4);
        }

        return retPhone;
    }

    /// <summary>
    /// Convenience function to execute an sqlStatement
    /// </summary>
    /// <param name="sqlStatement">sql statement to execute</param>
    /// <returns>results of the sql statement</returns>
    public static DataSet ExecuteSql(string sqlStatement)
    {
        SqlConnection conn = null;
        DataSet returnData = new DataSet();
        try
        {
            conn = new SqlConnection(GetConnectionString());
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlStatement, conn);
            da.SelectCommand.CommandTimeout = 340;
            da.Fill(returnData);
        }
        catch
        {
            throw;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }


        return returnData;
    }

    /// <summary>
    /// Convenience function to execute an sqlStatement
    /// </summary>
    /// <param name="sqlStatement">sql statement to execute</param>
    /// <param name="sqlParams">parameters to pass to the executing statement</param>
    /// <returns>results of the sql statement</returns>
    public static object ExecuteSqlScalar(string sqlStatement, List<SqlParameter> sqlParams)
    {
        SqlConnection conn = null;
        object rtn = null;
        try
        {
            conn = new SqlConnection(GetConnectionString());
            conn.Open();
            SqlCommand cmd = new SqlCommand(sqlStatement, conn);
            cmd.CommandTimeout = 340;
            foreach (SqlParameter sqlParam in sqlParams)
            {
                cmd.Parameters.Add(sqlParam);
            }
            rtn = cmd.ExecuteScalar();
        }
        catch
        {
            throw;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return rtn;
    }

    /// <summary>
    /// Executes a SQL statement and returns a DataSet with the results.
    /// </summary>
    /// <param name="sqlStatement">SQL statement to execute.</param>
    /// <param name="trans" >Transaction within which to execute the SQL statement. This can be used to chain multiple SQL calls.</param>
    /// <returns>A DataSet holding the results of the query.</returns>
    public static DataSet ExecuteSql(string sqlStatement, SqlTransaction trans)
    {
        DataSet returnData = new DataSet();

        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = trans.Connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlStatement;
            cmd.Transaction = trans;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(returnData);
        }
        catch
        {
            throw;
        }

        return (returnData);

    }

    public static SqlTransaction CreateTransaction()
    {
        SqlConnection conn = null;
        SqlTransaction retTransaction = null;
        try
        {
            conn = new SqlConnection(GetConnectionString());
            conn.Open();
            retTransaction = conn.BeginTransaction();
        }
        catch
        {
            if (retTransaction != null)
            {
                retTransaction = null;
            }

            if (conn != null)
            {
                conn.Close();
                conn = null;
            }
            throw;
        }

        return retTransaction;
    }

    /// <summary>
    /// Executes a SQL statement using a list of passed in SQLParameters.
    /// </summary>
    /// <param name="sqlStatement">SQL statement to execute.</param>
    /// <param name="sqlParams">A list of SQLParameter objects.</param>
    /// <returns>A DataSet holding the results of the query.</returns>
    public static DataSet ExecuteSql(string sqlStatement, List<SqlParameter> sqlParams)
    {
        DataSet returnData = new DataSet();

        try
        {
            SqlDataAdapter da = new SqlDataAdapter(sqlStatement, GetConnectionString());
            foreach (SqlParameter sqlParam in sqlParams)
            {
                da.SelectCommand.Parameters.Add(sqlParam);
            }

            da.Fill(returnData);
        }
        catch
        {
            throw;
        }

        return (returnData);

    }

    public static DataSet ExecuteSql(string sqlStatement, List<SqlParameter> sqlParams, SqlTransaction trans)
    {
        DataSet returnData = new DataSet();

        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = trans.Connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlStatement;
            cmd.Transaction = trans;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            foreach (SqlParameter sqlParam in sqlParams)
            {
                da.SelectCommand.Parameters.Add(sqlParam);
            }
            da.Fill(returnData);
        }
        catch
        {
            throw;
        }

        return (returnData);


    }

    /// <summary>
    /// Executes a stored procedure using the passed in list of SQLParameter objects.
    /// </summary>
    /// <param name="spName">Name of the stored procedure to execute.</param>
    /// <param name="parameters">A list of SQLParamenter objects to pass to the stored procedure call.</param>
    /// <returns>A DataSet holding the results of the stored procedure call.</returns>
    public static DataSet ExecuteStoredProcedure(string spName, List<SqlParameter> parameters)
    {
        SqlConnection conn = null;
        DataSet returnData = new DataSet();

        try
        {
            conn = new SqlConnection(GetConnectionString());
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 0;
            foreach (SqlParameter prm in parameters)
            {
                cmd.Parameters.Add(prm);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(returnData);
        }
        catch
        {
            throw;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
        return (returnData);
    }

    /// <summary>
    /// Returns a string representation of the numeric month.
    /// </summary>
    /// <param name="month">A numeric representation of the month.</param>
    /// <returns></returns>
    public static string GetMonthFromInt(int month)
    {
        string retName = "Unknown";
        switch (month)
        {
            case 1:
                retName = "January";
                break;

            case 2:
                retName = "February";
                break;

            case 3:
                retName = "March";
                break;

            case 4:
                retName = "April";
                break;

            case 5:
                retName = "May";
                break;

            case 6:
                retName = "June";
                break;

            case 7:
                retName = "July";
                break;

            case 8:
                retName = "August";
                break;

            case 9:
                retName = "September";
                break;

            case 10:
                retName = "October";
                break;

            case 11:
                retName = "November";
                break;

            case 12:
                retName = "December";
                break;
        }

        return retName;
    }

    /// <summary>
    /// Executes a stored procedure using the passed in list of SQLParameter objects.
    /// </summary>
    /// <param name="spName">Name of the stored procedure to execute.</param>
    /// <param name="parameters">A list of SQLParamenter objects to pass to the stored procedure call.</param>
    /// <param name="trans">A SQLTransaction object. </param>
    /// <returns>A DataSet holding the results of the stored procedure call.</returns>
    public static DataSet ExecuteStoredProcedure(string spName, List<SqlParameter> parameters, SqlTransaction trans)
    {
        DataSet returnData = new DataSet();

        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = trans.Connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            cmd.Transaction = trans;

            foreach (SqlParameter prm in parameters)
            {
                cmd.Parameters.Add(prm);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(returnData);
        }
        catch
        {
            throw;
        }

        return (returnData);
    }

    /// <summary>
    /// Executes a stored procedure using the passed in list of SQLParameter objects.
    /// </summary>
    /// <param name="spName">Name of the stored procedure to execute.</param>
    /// <param name="parameters">A list of SQLParamenter objects to pass to the stored procedure call.</param>
    /// <returns>A DataSet holding the results of the stored procedure call.</returns>
    public static int ExecuteStoredProcedure2(string spName, List<SqlParameter> parameters)
    {
        SqlConnection conn = null;
        int li_rows_affected = -1;
        try
        {
            conn = new SqlConnection(GetConnectionString());
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 340;
            foreach (SqlParameter prm in parameters)
            {
                cmd.Parameters.Add(prm);
            }
            li_rows_affected = Convert.ToInt32(cmd.ExecuteScalar());

        }
        catch
        {
            throw;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
        return (li_rows_affected);
    }

    /// <summary>
    /// Gets the connection string from the app config file
    /// </summary>
    /// <returns>connection string</returns>
    public static String GetConnectionString()
    {
        return ConfigurationManager.ConnectionStrings["PrelimDB"].ConnectionString;
    }

    /// <summary>
    /// Gets an integer from the first row of a dataset
    /// </summary>
    /// <param name="elementName">name of the data element to get</param>
    /// <param name="ds">data set to get data from</param>
    /// <returns>integer from the data set</returns>
    public static int GetInt(string elementName, DataSet ds)
    {
        int returnValue = 0;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (!ds.Tables[0].Rows[0].IsNull(elementName))
            {
                returnValue = Convert.ToInt32(ds.Tables[0].Rows[0][elementName]);
            }
        }
        return returnValue;
    }


    /// <summary>
    /// Gets a string from the first row of a dataset
    /// </summary>
    /// <param name="elementName">name of the data element to get</param>
    /// <param name="ds">data set to get data from</param>
    /// <returns>string from the dataset</returns>
    public static string GetString(string elementName, DataSet ds)
    {
        string returnValue = "";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (!ds.Tables[0].Rows[0].IsNull(elementName))
            {
                returnValue = ds.Tables[0].Rows[0][elementName].ToString().Replace("''", "'");
            }
        }

        return returnValue;
    }

    public static string GetString(object astr_data)
    {
        string returnValue = "";
        returnValue = astr_data.ToString().Replace("''", "'");
        return returnValue;
    }

    /// <summary>
    /// Gets a boolean from the first row of a dataset
    /// </summary>
    /// <param name="elementName">name of the data element to get</param>
    /// <param name="ds">data set to get data from</param>
    /// <returns>boolean from dataset</returns>
    public static bool GetBool(string elementName, DataSet ds)
    {
        bool returnValue = false;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (!ds.Tables[0].Rows[0].IsNull(elementName))
            {
                returnValue = Convert.ToBoolean(ds.Tables[0].Rows[0][elementName]);
            }
        }
        return returnValue;
    }

    /// <summary>
    /// Gets a decimal from the first row of a dataset
    /// </summary>
    /// <param name="elementName">name of the data element to get</param>
    /// <param name="ds">data set to get data from</param>
    /// <returns>boolean from dataset</returns>
    public static decimal GetDecimal(string elementName, DataSet ds)
    {
        decimal returnValue = 0.0M;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (!ds.Tables[0].Rows[0].IsNull(elementName))
            {
                returnValue = Convert.ToDecimal(ds.Tables[0].Rows[0][elementName]);
            }
        }
        return returnValue;
    }

    /// <summary>
    /// Gets a DateTime from the first row of a dataset
    /// </summary>
    /// <param name="elementName">name of the data element to get</param>
    /// <param name="ds">data set to get data from</param>
    /// <returns>boolean from dataset</returns>
    public static DateTime GetDateTime(string elementName, DataSet ds)
    {
        DateTime returnValue = new DateTime(1753, 1, 1);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (!ds.Tables[0].Rows[0].IsNull(elementName))
            {
                returnValue = Convert.ToDateTime(ds.Tables[0].Rows[0][elementName]);
            }
        }
        return returnValue;
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

    public static int GetInt(object value)
    {
        int returnValue = 0;

        if (!(value.GetType() == DBNull.Value.GetType()))
        {
            returnValue = (int)value;
        }

        return returnValue;
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
            returnValue = dr[elementName].ToString().Replace("''", "'");
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


    /// <summary>
    /// Formats the date for a database write
    /// </summary>
    /// <param name="dt">date to format</param>
    /// <returns>formatted date</returns>
    public static DateTime FormatDate(DateTime dt)
    {
        DateTime returnValue = dt;
        if (dt.Year == 1 && dt.Month == 1 && dt.Day == 1)
        {
            returnValue = new DateTime(1753, 1, 1);
        }

        return returnValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static String FormatDBDateAsString(object date)
    {
        string rtn = "";

        if (date.GetType() == DBNull.Value.GetType())
        {
            rtn = "";
        }
        else
        {
            rtn = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
        }

        return rtn;
    }

    /// <summary>
    /// Formats the phone number for a database write
    /// </summary>
    /// <param name="dt">phone number to format</param>
    /// <returns>formatted phone number</returns>
    public static string FormatPhone(string phone)
    {
        string returnValue = "";
        if (phone != null)
        {
            returnValue = phone.Replace("-", "");
            returnValue = returnValue.Replace(" ", "");
            returnValue = returnValue.Replace("_", "");
            returnValue = returnValue.Replace("(", "");
            returnValue = returnValue.Replace(")", "");
        }
        return returnValue;
    }

    public static string FormatPhone2(string phone)
    {
        string returnValue = null;
        if (phone == null)
        {
            returnValue = "";
        }
        else
        {
            returnValue = phone.Replace("-", "");
            returnValue = returnValue.Replace(" ", "");
            returnValue = returnValue.Replace("_", "");
            returnValue = returnValue.Replace("(", "");
            returnValue = returnValue.Replace(")", "");
        }
        return returnValue;
    }

    /// <summary>
    /// Formats the SSN for a database write
    /// </summary>
    /// <param name="dt">SSN to format</param>
    /// <returns>formatted SSN</returns>
    public static string FormatSSN(string SSN)
    {
        string returnValue = SSN.Replace("-", "");
        returnValue = returnValue.Replace("_", "");

        return returnValue;
    }

    /// <summary>
    /// Formats the string for a database write
    /// </summary>
    /// <param name="dt">string to format</param>
    /// <returns>formatted string</returns>
    public static string FormatString(string str)
    {
        if (str == null)
        {
            str = "";
        }
        return str.Trim().Replace("'", "''");
    }


    /// <summary>
    /// Checks to see if the given date is null
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static bool IsDateNull(DateTime dt)
    {
        return (dt == null || (dt.Year == 1753 && dt.Month == 1 && dt.Day == 1));
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

    public static bool IsNumeric(object Expression)
    {
        bool isNum;
        double retNum;
        isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        return isNum;
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


    public static string ConvertCaps(string inStr)
    {
        /* string strOut = "";
         if (inStr.Length > 0)
         {
             string[] strArray = inStr.Split(' ');
             for (int index = 0; index < strArray.Length; index++)
             {
                 if (strArray[index].Contains("\r\n"))
                 {
                     string[] seperators = new string[1];
                     seperators[0] = "\r\n";
                     string[] strArray2 = strArray[index].Split(seperators, StringSplitOptions.None);
                     for (int index2 = 0; index2 < strArray2.Length; index2++)
                     {
                         if (strArray2[index2].Length > 1 && IsAlpha(strArray2[index2].Substring(0, 1)) && !strArray2[index2].Equals("NE") && !strArray2[index2].Equals("NW") && !strArray2[index2].Equals("SE") && !strArray2[index2].Equals("SW"))
                         {
                             strOut += strArray2[index2].Substring(0, 1).ToUpper() + strArray2[index2].Substring(1).ToLower();
                         }
                         else
                         {
                             strOut += strArray2[index2];
                         }
                         if (index2 < strArray2.Length - 1)
                         {
                             strOut += "\r\n";
                         }
                     }
                 }
                 else if (strArray[index].Length > 1 && IsAlpha(strArray[index].Substring(0, 1)) && !strArray[index].Equals("NE") && !strArray[index].Equals("NW") && !strArray[index].Equals("SE") && !strArray[index].Equals("SW"))
                 {
                     strOut += strArray[index].Substring(0, 1).ToUpper() + strArray[index].Substring(1).ToLower();
                 }
                 else
                 {
                     strOut += strArray[index];
    
                 }
    
                 if (index < strArray.Length - 1)
                 {
                     strOut += " ";
                 }
             }
             //strOut = strOut.Replace("'", "''");
         }*/

        return inStr;
    }

    public static string NameHyphen(string name)
    {
        if (name != null)
        {
            int pos = name.IndexOf('-');
            if (name.IndexOf('-') > -1)
            {
                name = name.Substring(0, pos) + "-" + name.Substring(pos + 1, 1).ToUpper() + name.Substring(pos + 2);
            }
        }
        else
        {
            name = "";
        }

        return name;
    }

    private static bool IsAlpha(string strIn)
    {
        char[] charArray = strIn.ToCharArray();
        if ((charArray[0] > 0x40 && charArray[0] < 0x5B) || (charArray[0] > 0x60 && charArray[0] < 0x7B))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string PadString(string inStr, int paddedLength)
    {
        string retVal = inStr;
        if (inStr.Length > paddedLength)
        {
            retVal = inStr.Substring(0, paddedLength);
        }
        else
        {
            retVal = inStr.PadRight(paddedLength, ' ');
        }
        return retVal;
    }

    public static bool CheckAddress(string addr)
    {
        // make sure address is filled
        return (addr.Trim().Length > 0);

    }

    public static bool CheckZipCode(string zip)
    {
        bool isValid = true;
        //check if zip is valid
        try
        {
            if (zip.Trim().Length > 0)
            {
                Convert.ToInt64(zip);
            }
        }
        catch
        {
            isValid = false;
        }

        return isValid;
    }

    public static bool CheckState(string state)
    {
        bool isValid = false;
        DataSet ds = ExecuteSql("SELECT * FROM States WHERE Abbreviation='" + state + "'");
        if (CHelper.HasRows(ds))
        {
            isValid = true;
        }

        return isValid;
    }



    public static bool HasRows(DataSet ds)
    {
        return (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
    }



    public static string DispDate(DateTime dt)
    {
        if (CHelper.IsDateNull(dt))
        {
            return "";
        }
        else
        {
            return dt.ToShortDateString();
        }
    }

    public static string FormatExceptionForLog(Exception applicationException)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("Source: {0}\n", applicationException.Source);
        sb.AppendFormat("Message: {0}\n", applicationException.Message);
        sb.AppendFormat("StackTrace: {0}\n", applicationException.StackTrace);
        sb.AppendLine("");

        while (applicationException.InnerException != null)
        {
            applicationException = applicationException.InnerException;
            sb.AppendLine("Inner Exception:");
            sb.Append(FormatExceptionForLog(applicationException));
        }

        return sb.ToString();
    }

    public static bool IsADate(String str)
    {
        DateTime date;
        bool rtn;

        try
        {
            date = DateTime.Parse(str);
            rtn = true;
        }
        catch (Exception)
        {
            rtn = false;
        }

        return rtn;
    }
}

public class FixedLengthParser
{
    string m_dataToParse = "";
    int m_parsingIndex = 0;

    public int OverallSize
    {
        get
        {
            return m_dataToParse.Length;
        }
    }

    public FixedLengthParser(string dataString)
    {
        m_dataToParse = dataString;
        m_parsingIndex = 0;
    }

    public void Reset()
    {
        m_parsingIndex = 0;
    }

    public void Skip(int length)
    {
        m_parsingIndex += length;
    }

    public string GetString(int startIndex, int length)
    {
        string retValue = "";
        m_parsingIndex = startIndex;
        retValue = m_dataToParse.Substring(m_parsingIndex, length).Trim();
        m_parsingIndex += length;
        return retValue;
    }
    public string GetString(int length)
    {
        string retValue = "";
        retValue = m_dataToParse.Substring(m_parsingIndex, length).Trim();
        m_parsingIndex += length;
        return retValue;
    }

    public int GetInt(int length)
    {
        string rawValue = GetString(length);
        return Convert.ToInt32(rawValue);
    }

    public DateTime GetDateTime()
    {
        // set to standard date format of yyyyMMdd

        string rawDate = GetString(8);
        DateTime retDate = CHelper.GetNullDate();
        if (rawDate != "00000000")
        {
            try
            {
                retDate = DateTime.ParseExact(rawDate, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch
            {
                retDate = CHelper.GetNullDate();
            }
        }

        return retDate;
    }

    public decimal GetDecimal(int length, int numberOfDecimalDigits)
    {
        string rawValue = GetString(length + numberOfDecimalDigits);

        return Convert.ToDecimal(rawValue.Substring(0, 6)) + Convert.ToDecimal(rawValue.Substring(6, 2)) / 100.0M;
    }

}

