using MAXIMUS.Core.Libraries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net;
using System.Security.Authentication;
using System.Web.Script.Serialization;
using MAXIMUS.Controllers.PDMS;


/// <summary>
/// Convenience functions
/// </summary>
public class Helper
{
    private const string ATTACHMENT_RETENTION_DATA = "ATTACHMENT_RETENTION_DATA";
    private const string INQUIRY_RESPONSE_RETENTION_DATA = "INQUIRY_RESPONSE_RETENTION_DATA";
    private const string RecipientEligibilityResponse = "RecipientEligibilityResponse";
    private const string ATTACHMENT_RETENTION_DATA1 = "ATTACHMENT_RETENTION_DATA1";
    private const string TempDataClear = "TempDataClear";

    public static void PurgeSessionData()
    {
        System.Web.HttpContext.Current.Session.Remove(ATTACHMENT_RETENTION_DATA);
        System.Web.HttpContext.Current.Session.Remove(INQUIRY_RESPONSE_RETENTION_DATA);
        System.Web.HttpContext.Current.Session.Remove(RecipientEligibilityResponse);
        System.Web.HttpContext.Current.Session.Remove(ATTACHMENT_RETENTION_DATA1);
        System.Web.HttpContext.Current.Session.Remove(TempDataClear);
    }

    private Helper()
    {
        //
        // Can't create this class
        //
    }



    public static DataTable ConvertToDataTable<T>(List<T> models)
    {
        DataTable table = new DataTable(typeof(T).Name);

        if (models == null || models.Count == 0)
            return table;

        PropertyInfo[] properties = typeof(T).GetProperties();
 
        foreach (var prop in properties)
        {
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }

        foreach (var model in models.Where(m => m != null))
        {
            var values = properties.Select(prop => prop.GetValue(model, null) ?? DBNull.Value).ToArray();
            table.Rows.Add(values);
        }

        return table;
    }

    public static string HtmlEncode(string unsafeString)
    {
        return System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(unsafeString, true);
    }

    public static string UrlEncode(string unsafeString)
    {
        return System.Web.Security.AntiXss.AntiXssEncoder.UrlEncode(unsafeString, Encoding.UTF8);
    }

    public static bool IsModern()
    {
        if (System.Web.Configuration.WebConfigurationManager.AppSettings["Modernization"] != null)
            if (Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["Modernization"]))
            {
                return true;
            }

        return false;
    }

    public static List<KeyValuePair<Guid, Guid>> GetUserRoleCompatibility()
    {
        List<KeyValuePair<Guid, Guid>> retCompatibleRoles = new List<KeyValuePair<Guid, Guid>>();
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetUserRoleCompatibility();
        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                retCompatibleRoles.Add(new KeyValuePair<Guid, Guid>(new Guid(Helper.GetString("USER_ROLE_ID", dr)), new Guid(Helper.GetString("COMPATIBLE_USER_ROLE_ID", dr))));
            }
        }

        return retCompatibleRoles;
    }

    public static List<KeyValuePair<Guid, string>> GetUserRolesByUser(string userName)
    {
        List<KeyValuePair<Guid, string>> retRoles = new List<KeyValuePair<Guid, string>>();
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetUserRolesByUser(userName);
        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                retRoles.Add(new KeyValuePair<Guid, string>(new Guid(Helper.GetString("RoleId", dr)), Helper.GetString("RoleName", dr)));
            }
        }

        return retRoles;
    }

    public static List<KeyValuePair<Guid, string>> GetUserRolesByUserforQueueEligible(string userName, string userId)
    {
        List<KeyValuePair<Guid, string>> retRoles = new List<KeyValuePair<Guid, string>>();
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        List<KeyValuePair<Guid, string>> workQueueEligibleRole = new List<KeyValuePair<Guid, string>>();

        DataSet dsLowered = svc.GetLoweredUserNameByUserName(userName, userId);
        if (Helper.HasRows(dsLowered))
        {
            DataSet ds = svc.GetUserRolesByUser(dsLowered.Tables[0].Rows[0].ItemArray[0].ToString());
            if (Helper.HasRows(ds))
            {

                var results = from myRow in ds.Tables[0].AsEnumerable()
                              where myRow.Field<bool>("Is_WorkQueueEligible") == true
                              select new { RoleId = myRow.Field<Guid>("RoleId"), Rolename = myRow.Field<string>("RoleName") };


                workQueueEligibleRole = results.AsEnumerable().Select(item => new KeyValuePair<Guid, string>(item.RoleId, item.Rolename)).ToList();
            }
        }

        return workQueueEligibleRole;
    }

    public static List<ListItem> GetAllUserRoles()
    {
        List<ListItem> retRoles = new List<ListItem>();
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetAllUserRoles();
        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                retRoles.Add(new ListItem(Helper.GetString("RoleName", dr), Helper.GetString("RoleId", dr)));
            }
        }

        return retRoles;

    }

    public static List<ListItem> GetUserRolesByOHID(string ohid)
    {
        
        List<ListItem> retRoles = new List<ListItem>();

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetUserRolesByOHID(ohid);

        // Check if the DataSet returned from the service has any rows.
        if (Helper.HasRows(ds))
        {
            // Loop through each row in the first table of the DataSet.
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                retRoles.Add(new ListItem(Helper.GetString("RoleName", dr), Helper.GetString("RoleId", dr)));
            }
        }

        // Return the list of roles. If no roles were found, this list will be empty.
        return retRoles;
    }

    public static string RedirectLoginURL()
    {
        return "~/Account/Login.aspx";
    }


    public static string LoginPageName()
    {
        string URL = RedirectLoginURL();
        string returnvalue;

        if (URL.Contains('/'))
        {
            string[] arrayUrl = URL.Split('/');

            returnvalue = Array.Find(arrayUrl,
            element => element.StartsWith("Login", StringComparison.Ordinal));//returns Login.aspx or LoginNew.aspx
        }
        else
            returnvalue = URL;// returns ~/Account/Login.aspx

        return returnvalue;
    }

    static Random random = new Random();
    public static int RandomNumber(int min, int max)
    {
        return random.Next(min, max);
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
            DateTime date = Convert.ToDateTime(dateEntry);

            if (date.Year.ToString().Length == 4)
            {
                rtn = true;
            }
        }
        catch { }
        return rtn;
    }

    public static bool IsValidEmail(string email)
    {
        try
        {
            MailAddress m = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
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

    // Remove non-numerics
    public static string StripNonNumerics(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        return System.Text.RegularExpressions.Regex.Replace(input, "\\D", string.Empty);
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

    /// <summary>
    /// Function to add formatting to a Social Security Number.
    /// If true will Redact the return string "***-**-9999"
    /// </summary>
    /// <param name="inputPhone">String holding the unformatted SSN</param>
    /// <returns>Formatted SSN string 999-99-9999</returns>
    public static string FormatSSN(string inputSSN, bool Redact = false)
    {
        string rtn = string.Empty;
        if (!string.IsNullOrEmpty(inputSSN))
        {
            if (Redact) rtn = "XXX-XX-" + inputSSN.Substring(5);
            else rtn = String.Format("{0}-{1}-{2}", inputSSN.Substring(0, 3), inputSSN.Substring(3, 2), inputSSN.Substring(5));
        }
        return rtn;
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

    public static string ConvertStrNullToString(Object aobj_data)
    {
        if (aobj_data == null || aobj_data == DBNull.Value)
        {
            return ("");
        }
        else
        {
            return (Convert.ToString(aobj_data));
        }
    }

    /// <summary>
    /// Remove Path Transversal patterens, Wildcards, and invalid characters
    /// </summary>
    /// <param name="path"></param>
    /// <param name="allowRootPath">When true paths with a leading slash will be allowed. when false a leading slash will be removed.</param>
    /// /// <param name="allowRootPath">When true paths with a leading double slash will be allowed. when false a leading double slash will be removed.</param>
    /// <returns></returns>
    public static string CleanFilePath(string path, bool allowRootPath = false, bool allowUNCPath = false)
    {
        var PathTrans = @"(^\.\./|(?<=/)\.\./)";
        PathTrans = PathTrans.Replace(@"/", Regex.Escape(System.IO.Path.DirectorySeparatorChar.ToString())); //replace slash with platform specific separator character.
        var invalidchars = Regex.Escape(new string(System.IO.Path.GetInvalidPathChars()));
        var driveRoot = "^[a-zA-Z]:" + Regex.Escape(System.IO.Path.DirectorySeparatorChar.ToString());
        var wildcards = "*?";
        var unc = System.IO.Path.DirectorySeparatorChar.ToString() + System.IO.Path.DirectorySeparatorChar.ToString();
        //order of sanitation is vital do not change. removing in different order could create unsafe patterns.
        path = System.Text.RegularExpressions.Regex.Replace(path, "[" + invalidchars + wildcards + "]", ""); //remove invalid chars and wildcards. This must happen first 
        if (allowRootPath)
        {
            var clean = false;
            var limit = 0;
            do
            {
                var dr = "";
                var cxPath = path;
                if (!allowUNCPath && cxPath.StartsWith(unc)) cxPath = cxPath.Substring(unc.Length); //remove double slash unc root
                var drMatch = System.Text.RegularExpressions.Regex.Match(cxPath, driveRoot); //match drive segment
                if (drMatch.Success)
                {
                    cxPath = cxPath.Substring(drMatch.Value.Length); //get path after drive segment
                    dr = drMatch.Value;
                }
                cxPath = dr + cxPath.Replace(":", "");

                if (path == cxPath) clean = true; //eliminate ':' not in drive segment
                else path = cxPath;
                if (limit++ == 100) throw new ApplicationException("File path contains too many invalid characters");
            } while (clean == false);
        }
        else
        {
            var clean = false;
            var limit = 0;
            do
            {
                var cxPath = path;
                if (!allowUNCPath && cxPath.StartsWith(unc)) cxPath = cxPath.Substring(unc.Length); //remove double slash unc root
                if (cxPath.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString())) cxPath = cxPath.Substring(1); //remove leading slash
                cxPath = System.Text.RegularExpressions.Regex.Replace(cxPath, driveRoot, ""); //remove DriveRoot i.e. C:\
                cxPath = cxPath.Replace(":", "");
                if (path == cxPath) clean = true; else path = cxPath;
                if (limit++ == 100) throw new ApplicationException("File path contains too many invalid characters");
            } while (clean == false);
        }
        path = System.Text.RegularExpressions.Regex.Replace(path, PathTrans, ""); //remove path transversals i.e. '../' 
        return path;
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

    public static string FormatDate2(string dateString)
    {
        string formattedDate = "";
        DateTime dateInput;
        try
        {
            dateInput = DateTime.Parse(dateString);
            if (dateInput == DateTime.MinValue
                || (dateInput.Year == 1900 && dateInput.Month == 1 && dateInput.Day == 1)
                || (dateInput.Year == 1753 && dateInput.Month == 1 && dateInput.Day == 1))
            {
                formattedDate = string.Empty;
            }
            else
            {
                formattedDate = dateInput.ToString("MM/dd/yyyy");
            }
        }
        catch
        {
            formattedDate = string.Empty;
        }


        return formattedDate;
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
        catch (Exception)
        {
            returnValue = DateTime.Parse("1/1/1753");
        }


        return returnValue;
    }


    public static UInt32 HashString(string str)
    {
        UInt32 hash = 5381;

        for (int i = 0; i < str.Length; i++)
        {
            hash = ((hash << 5) + hash) + str[i];
        }

        return (hash & 0x7FFFFFFF);
    }

    public static bool IsNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }

    public static bool ValueExistsInDropDown(DropDownList ddl, string value)
    {
        bool exists = false;
        ListItem li = ddl.Items.FindByValue(value);
        if (li != null)
            exists = true;

        return exists;
    }

    // Loads a dropdown or listview
    public static void LoadList(ListControl listObj, object listData, string dataText, string dataValue, bool insertSelect)
    {
        listObj.ClearSelection();
        listObj.Items.Clear();
        listObj.DataSource = listData;
        listObj.DataTextField = dataText;
        listObj.DataValueField = dataValue;
        listObj.DataBind();
        if (insertSelect) listObj.Items.Insert(0, new ListItem(string.Empty, string.Empty));
    }


    public static void radLoadList(DropDownList listObj, object listData, string dataText, string dataValue, bool insertSelect)
    {
        listObj.Items.Clear();
        listObj.DataSource = listData;
        listObj.DataTextField = dataText;
        listObj.DataValueField = dataValue;
        listObj.DataBind();
        if (insertSelect) listObj.Items.Insert(0, new DropDownListItem(string.Empty, string.Empty).Value);
    }

    // Load a drop down list with unique DataText items only
    public static void LoadDropDown(DropDownList ddl, DataTable dt, string dataText, string dataValue, bool insertSelect, bool upperCase = false)
    {
        ddl.Items.Clear();
        foreach (DataRow row in dt.Rows)
        {
            string value = Helper.GetString(dataText, row);
            if (!string.IsNullOrEmpty(value))
            {
                if (ddl.Items.FindByText(value) == null)
                {
                    ListItem lst = new ListItem();
                    lst.Text = value;
                    lst.Value = Helper.GetString(dataValue, row);
                    if (upperCase)
                    {
                        lst.Value = lst.Value.ToUpper();
                    }

                    ddl.Items.Add(lst);
                }
            }
        }
        if (insertSelect) ddl.Items.Insert(0, new ListItem(string.Empty, string.Empty));
    }


    public static void LoadRadDropDown(DropDownList rddl, DataTable dt, string dataText, string dataValue, bool insertSelect, bool upperCase = false)
    {
        rddl.Items.Clear();
        foreach (DataRow row in dt.Rows)
        {
            string value = Helper.GetString(dataText, row);
            if (!string.IsNullOrEmpty(value))
            {
                if (rddl.Items.FindByValue(value) == null)
                {
                    DropDownListItem lst = new DropDownListItem();
                    lst.Text = value;
                    lst.Value = Helper.GetString(dataValue, row);
                    if (upperCase)
                    {
                        lst.Value = lst.Value.ToUpper();
                    }

                    Helper.LoadList(rddl, dt, "COUNTY_NAME", "MMIS_COUNTY_CODE", true);
                }
            }
        }
        //if (insertSelect) rddl.Items.Insert(0, new DropDownListItem(string.Empty, string.Empty));
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

    // Return current user role
    public static string GetUserRole(string username)
    {
        string[] rolls = Roles.GetRolesForUser(username);
        if (rolls.Length > 0) return rolls[0];

        return string.Empty;
    }

    // Return true if the user is in the role specified
    public static bool IsUserInRole(string username, string roleName)
    {
        bool retIsInRole = false;
        retIsInRole = Roles.IsUserInRole(username, roleName);

        return retIsInRole;
    }

    // returns true if the search string appears in a comma delimited list
    public static bool IsStringInList(string searchString, string commaDelimitedList)
    {
        bool retInList = false;
        string[] parsedList = commaDelimitedList.Split(',');
        foreach (string temp in parsedList)
        {
            if (temp.Trim().Equals(searchString))
            {
                retInList = true;
                break;
            }
        }

        return retInList;
    }

    // Return true if the user is a Provider or ProviderOper
    public static bool IsUserInProviderRoles(string username)
    {
        string[] roles = CON.ProviderRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }
    // Return true if the user is in the Provider Services roles 
    public static bool IsUserInPSRoles(string username)
    {
        string[] roles = CON.ProviderServiceRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }
    public static bool IsUserInInternalRoles(string username)
    {
        string[] roles = CON.InternalRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }
    public static bool IsInitiateReconsiderationRoles(string username)
    {
        string[] roles = CON.InitiateReconsiderationRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }

    public static bool IsUserInSpecialityInternalRoles(string username)
    {
        string[] roles = CON.SpecialityInternalRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }

    // Return true if the user is in the Site Visit Operator role
    public static bool IsUserInSiteVisitOperatorRole(string username)
    {

        if (Roles.IsUserInRole(username, CON.SiteVisitOperatorRole))
            return true;

        return false;
    }

    public static bool IsUserComplianceSpecialist(string username)
    {

        if (Roles.IsUserInRole(username, CON.ComplianceSpecialist))
            return true;

        return false;
    }
    public static bool IsUserInLTCWorkerRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.LTCWorkerRole))
            return true;

        return false;
    }
    public static bool IsUserInORFAWorkerRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.ORFAWorkerRole))
            return true;

        return false;
    }

    public static bool IsUserInDBHOperatorRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.DBHOperatorRole))
            return true;

        return false;
    }
    public static bool IsUserInDBHReviewerRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.DBHReviewerRole))
            return true;

        return false;
    }
    public static bool IsUserInDDSOperatorRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.DDSOperatorRole))
            return true;

        return false;
    }
    public static bool IsUserInDDSReviewerRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.DDSReviewerRole))
            return true;

        return false;
    }
    // Return true if the user is in the DHCF reviewer roles 
    public static bool IsUserInStateReviewerRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.StateReviewerRole))
            return true;

        return false;
    }
    // Return true if the user is in the State Admin roles 
    public static bool IsUserInStateAdminRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.StateAdminRole))
            return true;

        return false;
    }
    // Return true if the user is in the role specified
    public static bool IsUserInSubRole(int regid, string userId, string roleName)
    {
        bool retIsInRole = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        retIsInRole = svc.IsUserInSubRole(regid, userId, roleName) == 1 ? true : false;

        return retIsInRole;
    }

    public static bool IsUserInSubRoles(int regid, string userId, string roleNames)
    {
        bool retIsInRole = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        retIsInRole = svc.IsUserInSubRole(regid, userId, roleNames) == 1;
        return retIsInRole;
    }

    // Return true if the user is in the Operator roles 
    public static bool IsUserInOperatorRolls(string username)
    {
        string[] roles = CON.OperatorRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }

    public static bool IsUserInScreeningRole(string username)
    {
        string[] roles = CON.ScreeningOperatorRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }

    public static bool IsUserInEnrollmentSpecialistRole(string username)
    {
        string[] roles = CON.EnrollmentSpecialistRole.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }

    public static bool IsUserInNetworkAdequacyReportRole(string username)
    {
        string[] roles = CON.NetworkAdequacyReportRole.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }

    public static bool IsUserInNetworkAdequacyReportRolesByPlanName(string username)
    {
        string[] roles = CON.NetworkAdequacyReportRolesByPlanName.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }

    public static bool OtherReportsVisibiltyToNetworkAdequacyReportRoles()
    {
        List<KeyValuePair<Guid, string>> listCurrentRoles = GetUserRolesByUser(HttpContext.Current.User.Identity.Name.ToString());
        string allNetWorkAdequacyRoles = string.Concat(CON.NetworkAdequacyReportRole, CON.NetworkAdequacyReportRolesByPlanName);
        string reportRoles = "ComplianceSpecialist,CredentialingQualityAssurance,ODMCredentialingQualityAssurance,CredentialingSupervisor,ODMStateAdministrator,TechAdmin,EnrollmentSpecialist,LTCCHOP,APMSpecialist,ODMCredentialingSupervisor";
        //If user only have NA report roles then don't show other reports to the user
        foreach (var item in listCurrentRoles)
        {
            if (!allNetWorkAdequacyRoles.ToUpper().Contains(item.Value.ToUpper()))
            {
                //check if the user has any of the report roles:
                if (reportRoles.ToUpper().Contains(item.Value.ToUpper()))
                {
                    return true;
                }
            }
        }
        return false;
    }


    public static bool IsUserInODMCredentialingRoleIndvAff(string username)
    {
        string[] roles = CON.ODMCredentialingIndvAffiliationRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }

    public static bool IsUserInCredentialingRole(string username)
    {
        string[] roles = CON.CredentialingRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }
    public static bool IsUserCredentialSpecialist(string username)
    {
        if (Roles.IsUserInRole(username, CON.CredentialSpecialistRole))
            return true;

        return false;
    }
    // Return true if the user is in the Accounting roles 
    public static bool IsUserInAccountingRolls(string username)
    {
        string[] roles = CON.AccountingRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }

    // Return true if the user is in the Accounting roles 
    public static bool IsUserInAppealSpecialist(string username)
    {
        if (Roles.IsUserInRole(username, CON.UserRole.AppealSpecialist))
            return true;

        return false;
    }
    // Return true if the user is in the DIDD roles 
    public static bool IsUserInDIDDRoles(string username)
    {
        string[] roles = CON.DIDDRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(username, role)) return true;
        }
        return false;
    }
    public static bool IsUserCredentialingSupervisor(string username)
    {
        if (Roles.IsUserInRole(username, CON.UserRole.CredentialingSupervisor))
            return true;

        return false;
    }
    public static bool IsUserODMCredentialingSpecialistLTC(string username)
    {
        if (Roles.IsUserInRole(username, CON.UserRole.ODMCredentialingSpecialist))
            return true;

        return false;
    }
    public static bool IsUserODMCredentialingSupervisor(string username)
    {
        if (Roles.IsUserInRole(username, CON.UserRole.ODMCredentialingSupervisor))
            return true;

        return false;
    }
    // Return true if the user is in the Admin roles 
    public static bool IsUserInAdminRole(string username)
    {
        if (Roles.IsUserInRole(username, MAXIMUS.Core.Libraries.Constants.UserRoleType.Administrator)) return true;

        return false;
    }

    public static bool IsLoggedInUserInAdminRole()
    {
        return Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.Administrator);
    }
    public static bool IsLoggedInUserInLTCChopRole()
    {
        return Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.LTCCHOP);
    }

    public static bool IsLoggedInUserInODMCredentialingQualityAssuranceRole()
    {
        return Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.ODMCredentialingQualityAssurance);
    }

    public static bool IsLoggedInUserInCommitteeQualitySpecialistRole()
    {
        return Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRole.CommitteeQualitySpecialist);
    }

    public static bool IsLoggedInUserInCredentialingQualityAssuranceRole()
    {
        return Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.CredentialingQualityAssurance);
    }

    public static bool IsLoggedInUserInPnmSuperUserRole()
    {
        return Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.PnmSuperUser);
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

    /// <summary>
    // Obtain from the database AppSettings table
    /// </summary>
    /// <returns></returns>
    public static string GetAppSettingFromDB(string key)
    {
        //string rtn = string.Empty;
        string rtn = AppSettings.Get(key);
        //PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        //DataSet ds = svc.GetAppSetting(key);
        //if (Helper.HasRows(ds)) rtn = ds.Tables[0].Rows[0][0].ToString();
        return rtn;
    }

    /// <summary>
    // Obtain from the database AppSettings table
    /// </summary>
    /// <returns></returns>
    public static string GetAppSettingFromDB(string key, string defaultValue)
    {
        string value = AppSettings.Get(key, defaultValue);
        return value;
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

    public static bool SecureWebPages
    {
        get
        {
            string val = Helper.GetAppSettingFromDB("SecureWebPages");
            return bool.Parse(val);
        }
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
        if (string.IsNullOrEmpty(srcString)) return string.Empty;
        if (string.IsNullOrEmpty(srcString.Trim())) return string.Empty;

        string rtn = string.Empty;
        for (int i = 0; i < srcString.Length; i++)
        {
            if (srcString.Substring(i, 1) == srcString.Substring(i, 1).ToUpper())
            {
                if ((i + 1) < srcString.Length)
                {
                    if (srcString.Substring(i + 1, 1) != srcString.Substring(i + 1, 1).ToUpper()) rtn += " ";
                }
            }
            rtn += srcString.Substring(i, 1);
        }
        return rtn;
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
    #endregion "DataRow Methods"

    // Get the data from the current GridViewRow given the element name
    public static string GetGridRowData(string elementName, GridViewRow dr)
    {
        string rtn = string.Empty;
        try
        {
            rtn = DataBinder.Eval(dr.DataItem, elementName).ToString();
        }
        catch { }
        return rtn;
    }

    // Get the data from the current RepeaterItem given the element name
    public static string GetRepeaterItemData(string elementName, RepeaterItem itm)
    {
        string rtn = string.Empty;
        try
        {
            rtn = DataBinder.Eval(itm.DataItem, elementName).ToString();
        }
        catch { }
        return rtn;
    }

    // Get the data dependent upon the type of row passed
    public static string GetData(string elementName, object obj)
    {
        if (obj.GetType() == typeof(GridViewRow))
            return GetGridRowData(elementName, (GridViewRow)obj);
        else if (obj.GetType() == typeof(RepeaterItem))
            return GetRepeaterItemData(elementName, (RepeaterItem)obj);
        else if (obj.GetType() == typeof(DataRow))
            return GetString(elementName, (DataRow)obj);
        else return string.Empty;
    }

    // Get the data dependent upon the type of control passed
    public static string GetData(string id, RepeaterItem itm, string defaultValue)
    {
        try
        {
            Control ctl = (Control)itm.FindControl(id);
            if (ctl == null) return defaultValue;
            if (ctl.GetType() == typeof(DropDownList)) return ((DropDownList)ctl).SelectedValue;
            else if (ctl.GetType() == typeof(TextBox)) return ((TextBox)ctl).Text;
            else if (ctl.GetType() == typeof(Label)) return ((Label)ctl).Text;
            else if (ctl.GetType() == typeof(HiddenField)) return ((HiddenField)ctl).Value;
        }
        catch { }
        return defaultValue;
    }

    // Set the data dependent upon the type of control passed
    public static void SetData(string id, RepeaterItem itm, string value)
    {
        try
        {
            Control ctl = (Control)itm.FindControl(id);
            if (ctl == null) return;
            if (ctl.GetType() == typeof(DropDownList)) ((DropDownList)ctl).SelectedValue = value;
            else if (ctl.GetType() == typeof(TextBox)) ((TextBox)ctl).Text = value;
            else if (ctl.GetType() == typeof(Label)) ((Label)ctl).Text = value;
            else if (ctl.GetType() == typeof(HiddenField)) ((HiddenField)ctl).Value = value;
        }
        catch { }
    }

    public static void UpdateCommonColumns(DataRow dr, int recordStatus)
    {
        dr["RecordStatus"] = recordStatus;
        dr["LastModifiedDateTime"] = DateTime.Now;
        dr["LastModifiedUser"] = GetUserId(HttpContext.Current.User.Identity.Name);
    }

    // Disable the backspace on any dropdowns found on the page
    public static void DisableBackSpace(System.Web.UI.Control theControl)
    {
        if (theControl.GetType() == typeof(DropDownList))
        {
            DropDownList ddl = (DropDownList)theControl;
            ddl.Attributes.Add("onkeydown", "return checkShortcut(1);");
        }
        else if (theControl.GetType() == typeof(TextBox))
        {
            TextBox txt = (TextBox)theControl;
            if (txt.ReadOnly) txt.Attributes.Add("onkeydown", "return checkShortcut(0);");
        }

        foreach (System.Web.UI.Control ctl in theControl.Controls)
        {
            DisableBackSpace(ctl);
        }
    }


    // Set the single control as read only
    private static void SetControlReadOnly(Control ctl, bool readOnly)
    {
        if (ctl.GetType() == typeof(TextBox))
        {
            TextBox txt = (TextBox)ctl;
            txt.ReadOnly = readOnly;
        }
        else if (ctl.GetType() == typeof(eWorld.UI.NumericBox))
        {
            eWorld.UI.NumericBox num = (eWorld.UI.NumericBox)ctl;
            num.ReadOnly = readOnly;
        }
        else if (ctl.GetType() == typeof(DropDownList))
        {
            DropDownList drp = (DropDownList)ctl;
            drp.Enabled = !readOnly;
        }
        else if (ctl.GetType() == typeof(RadioButton))
        {
            RadioButton btn = (RadioButton)ctl;
            btn.Enabled = !readOnly;
        }
        else if (ctl.GetType() == typeof(RadioButtonList))
        {
            RadioButtonList btn = (RadioButtonList)ctl;
            btn.Enabled = !readOnly;
        }
        else if (ctl.GetType() == typeof(CheckBox))
        {
            CheckBox chk = (CheckBox)ctl;
            chk.Enabled = !readOnly;
        }
        else if (ctl.GetType() == typeof(Button))
        {
            Button btn = (Button)ctl;
            btn.Enabled = !readOnly;
            if (!readOnly)
                btn.CssClass = "buttonBox";
        }
        else if (ctl.GetType() == typeof(LinkButton))
        {
            LinkButton btn = (LinkButton)ctl;
            btn.Enabled = !readOnly;
        }
        else if (ctl.GetType() == typeof(ImageButton))
        {
            ImageButton btn = (ImageButton)ctl;
            btn.Enabled = !readOnly;
        }
        else if (ctl.GetType() == typeof(CheckBoxList))
        {
            CheckBoxList chk = (CheckBoxList)ctl;
            chk.Enabled = !readOnly;
        }
        else if (ctl.GetType() == typeof(AjaxControlToolkit.ComboBox))
        {
            AjaxControlToolkit.ComboBox cmb = (AjaxControlToolkit.ComboBox)ctl;
            cmb.Enabled = !readOnly;
        }
        else if (ctl.GetType() == typeof(AjaxControlToolkit.CalendarExtender))
        {
            AjaxControlToolkit.CalendarExtender cal = (AjaxControlToolkit.CalendarExtender)ctl;
            cal.Enabled = !readOnly;
        }
    }

    // Set Controls as read only or enabled
    public static void SetReadOnly(Control mainCtl, bool readOnly)
    {
        if (mainCtl.Controls.Count == 0) SetControlReadOnly(mainCtl, readOnly);
        else
        {
            foreach (Control ctl in mainCtl.Controls)
            {
                if (ctl.Controls.Count > 0) SetReadOnly(ctl, readOnly);
                SetControlReadOnly(ctl, readOnly);
            }
        }
    }

    public static void SetReadOnlyForSpecificControl(Control ctl, bool readOnly, string styleCSS)
    {
        if (ctl.Controls.Count > 0) SetReadOnly(ctl, readOnly, styleCSS);
        if (ctl.GetType() == typeof(TextBox))
        {
            TextBox txt = (TextBox)ctl;
            txt.Attributes.Add("onkeydown", "return checkShortcut(" + (readOnly ? 1 : 0).ToString() + ");");
            txt.ReadOnly = readOnly;
            txt.Enabled = !readOnly;
            txt.CssClass = styleCSS;
        }
        else if (ctl.GetType() == typeof(eWorld.UI.NumericBox))
        {
            eWorld.UI.NumericBox num = (eWorld.UI.NumericBox)ctl;
            num.Attributes.Add("onkeydown", "return checkShortcut(" + (readOnly ? 1 : 0).ToString() + ");");
            num.ReadOnly = readOnly;
            num.CssClass = styleCSS;
        }
        else if (ctl.GetType() == typeof(DropDownList))
        {
            DropDownList drp = (DropDownList)ctl;
            drp.Enabled = !readOnly;
            drp.Attributes.Add("onkeydown", "return checkShortcut(1);");
            drp.CssClass = styleCSS;
        }
        else if (ctl.GetType() == typeof(CheckBox))
        {
            CheckBox chk = (CheckBox)ctl;
            chk.Enabled = !readOnly;
            // CheckBox doesn't look good with the formFieldReadOnly class; just setting guts to grey
            chk.CssClass = "formFieldReadOnlyBasic";
        }
        else if (ctl.GetType() == typeof(Telerik.Web.UI.RadComboBox))
        {

            Telerik.Web.UI.RadComboBox rCtrl = (Telerik.Web.UI.RadComboBox)ctl;
            rCtrl.Enabled = !readOnly;
            // RadComboBox doesn't look good with the formFieldReadOnly class; just setting guts to grey
            rCtrl.CssClass = "formFieldReadOnlyBasic";
        }
        else if (ctl.GetType() == typeof(RadioButton))
        {
            RadioButton rBtn = (RadioButton)ctl;
            rBtn.Enabled = !readOnly;
            rBtn.CssClass = styleCSS;
        }
        else if (ctl.GetType() == typeof(CheckBoxList))
        {
            CheckBoxList checkboxList = (CheckBoxList)ctl;
            checkboxList.Enabled = !readOnly;
            // checkboxList doesn't look good with the formFieldReadOnly class; just setting to grey
            checkboxList.CssClass = "formFieldReadOnlyBasic";
        }
        else if (ctl.GetType() == typeof(RadioButtonList))
        {
            RadioButtonList radioButtonList = (RadioButtonList)ctl;
            radioButtonList.Enabled = !readOnly;
            // radiolist doesn't look good with the formFieldReadOnly class; just setting to grey
            radioButtonList.CssClass = "formFieldReadOnlyBasic";
        }
        else if (ctl.GetType() == typeof(FileUpload))
        {
            FileUpload fileUpload = (FileUpload)ctl;
            fileUpload.Enabled = !readOnly;
            fileUpload.CssClass = styleCSS;
        }
        else if (ctl.GetType() == typeof(RadAsyncUpload))
        {
            RadAsyncUpload fileUpload = (RadAsyncUpload)ctl;
            fileUpload.Enabled = !readOnly;
            fileUpload.CssClass = styleCSS;
        }
        else if (ctl.GetType() == typeof(System.Web.UI.WebControls.FileUpload))
        {
            System.Web.UI.WebControls.FileUpload fileUpload = (System.Web.UI.WebControls.FileUpload)ctl;
            fileUpload.Enabled = !readOnly;
            fileUpload.CssClass = styleCSS;
        }
        else if (ctl.GetType() == typeof(MMSWebControls.EncryptedFileUpload))
        {
            MMSWebControls.EncryptedFileUpload fileUpload = (MMSWebControls.EncryptedFileUpload)ctl;
            fileUpload.Enabled = !readOnly;
            fileUpload.CssClass = styleCSS;
        }
        else if (ctl.GetType() == typeof(Button))
        {
            Button btn = (Button)ctl;
            btn.Enabled = !readOnly;
            btn.CssClass = styleCSS;
        }
        else if (ctl.GetType() == typeof(LinkButton))
        {
            LinkButton btn = (LinkButton)ctl;
            // Don't disable "Download" and "View" controls, and History button in read-only mode
            if (btn.ID != "LnkButtonDownload" && btn.ID != "LnkButtonView" && !btn.ID.Contains("History"))
            {
                btn.Enabled = !readOnly;
            }
        }
    }

    // Set Controls as read only or enabled with specific CSS Style
    public static void SetReadOnly(Control mainCtl, bool readOnly, string styleCSS)
    {
        foreach (Control ctl in mainCtl.Controls)
        {
            SetReadOnlyForSpecificControl(ctl, readOnly, styleCSS);
        }
    }

    // Set Control to ReadOnly if the user Role is ReadOnly
    public static void SetUserReadOnly(string username, Control mainCtl, string controlID)
    {
        try
        {
            if (!IsUserInRole(username, "ReadOnly")) return;
            Control ctl = FindTheControl(mainCtl, controlID);
            if (ctl.GetType() == typeof(Button))
            {
                // Disable the button
                ((Button)ctl).Enabled = false;
            }
            else if (ctl.GetType() == typeof(LinkButton))
            {
                // Disable the LinkButton
                ((LinkButton)ctl).Enabled = false;
            }
        }
        catch (Exception ex)
        {
            //do nothing if there is a problem
            Console.Write(ex.Message);
        }
    }

    // Turn off/on all validators
    public static void TurnOffValidators(Control mainCtl, bool turnON)
    {
        foreach (Control ctl in mainCtl.Controls)
        {
            if (ctl.Controls.Count > 0) TurnOffValidators(ctl, turnON);
            if (ctl.GetType() == typeof(RequiredFieldValidator))
            {
                RequiredFieldValidator req = (RequiredFieldValidator)ctl;
                req.Enabled = turnON;
            }
            else if (ctl.GetType() == typeof(RegularExpressionValidator))
            {
                RegularExpressionValidator reg = (RegularExpressionValidator)ctl;
                reg.Enabled = turnON;
            }
            else if (ctl.GetType() == typeof(CompareValidator))
            {
                CompareValidator cmp = (CompareValidator)ctl;
                cmp.Enabled = turnON;
            }
        }
    }

    public static void SortListBox(ref ListBox pList, bool pByValue)
    {
        SortedList lListItems = new SortedList();

        foreach (ListItem lItem in pList.Items)
        {
            if (pByValue) lListItems.Add(lItem.Value, lItem);
            else lListItems.Add(lItem.Text, lItem);
        }

        pList.Items.Clear();

        for (int i = 0; i < lListItems.Count; i++)
        {
            pList.Items.Add((ListItem)lListItems[lListItems.GetKey(i)]);
        }
    }

    public static string GetLicenseParts(string licenseNumber, int opt)
    {
        // Assumed format is "XX-XXXXXXX", where first "XX" is State Code and secondary "XXXXXXX" is the License Number
        // opt: 0 = State Code, 1 = License Number following State Code
        if (string.IsNullOrEmpty(licenseNumber)) return null;
        if (opt == 1) return licenseNumber.Substring(3);
        else return licenseNumber.Substring(0, 2);
    }

    public static void LoadComboboxWithStates(ref AjaxControlToolkit.ComboBox cb)//AjaxControlKit.ComboBox
    {

        if (cb.Items.Count > 0) return;                         // Already loaded don't load again
        cb.DataValueField = "StateId";
        cb.DataTextField = "StateName";
        cb.DataSource = ApplicationCache.StateAbbreviations();
        cb.DataBind();
        cb.Items.Insert(0, string.Empty);
    }

    public static void LoadDropDownListWithStates(ref DropDownList ddl, bool useStateAbbrev = false)
    {
        if (ddl.Items.Count > 0)
        {
            ddl.SelectedIndex = 0;              // Set "not selected" default index
            return;                             // Already loaded don't load again
        }
        ddl.ClearSelection();
        ddl.Items.Clear();
        ddl.SelectedValue = null;
        ddl.Text = null;
        if (useStateAbbrev)
        {
            Helper.LoadList(ddl, ApplicationCache.StateAbbreviations(), "StateId", "StateId", true);
        }
        else
        {
            Helper.LoadList(ddl, ApplicationCache.StateAbbreviations(), "StateName", "StateId", true);
        }
        ddl.SelectedIndex = 0;
    }


    public static void LoadRadDropDownListWithStates(ref DropDownList ddl, bool useStateAbbrev = false)
    {
        if (ddl.Items.Count > 0)
        {
            ddl.SelectedIndex = 0;              // Set "not selected" default index
            return;                             // Already loaded don't load again
        }
        ddl.ClearSelection();
        ddl.Items.Clear();
        if (useStateAbbrev)
        {
            Helper.radLoadList(ddl, ApplicationCache.StateAbbreviations(), "StateId", "StateId", true);
        }
        else
        {
            Helper.radLoadList(ddl, ApplicationCache.StateAbbreviations(), "StateName", "StateId", true);
        }
        ddl.SelectedIndex = 0;
    }

    public static bool RegistrationInReturnedToProvider(int regId)
    {
        bool InReturnedToProvider = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistration(regId);
        int status;
        if (Helper.HasRows(ds))
        {
            if (int.TryParse(Helper.GetData("REGISTRATION_STATUS_TYPE_ID", ds.Tables[0].Rows[0]), out status))
            {
                InReturnedToProvider = status == CON.RegistrationStatusTypeId.ReturnToProvider;
            }
        }
        return InReturnedToProvider;
    }
    public static bool RegistrationInReturnedToProviderFromSiteVisit(int regId)
    {
        bool InReturnedToProvider = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistration(regId);
        int status;
        if (Helper.HasRows(ds))
        {
            if (int.TryParse(Helper.GetData("REGISTRATION_STATUS_TYPE_ID", ds.Tables[0].Rows[0]), out status))
            {
                InReturnedToProvider = status == CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit;
            }
        }
        return InReturnedToProvider;
    }
    public static int GetProviderTypeIdByRegId(int regId)
    {
        int providerTypeId = 0;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistration(regId);
        if (Helper.HasRows(ds))
        {
            if (int.TryParse(Helper.GetData("PROVIDER_TYPE_ID", ds.Tables[0].Rows[0]), out providerTypeId))
            {
                return providerTypeId;
            }
        }
        return providerTypeId;
    }

    public static bool RegistrationIsPending(int regId)
    {
        bool toReturn = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistration(regId);
        int status;
        if (Helper.HasRows(ds))
        {
            if (int.TryParse(Helper.GetData("REGISTRATION_STATUS_TYPE_ID", ds.Tables[0].Rows[0]), out status))
            {
                toReturn = (status == CON.RegistrationStatusTypeId.Pending || status == CON.RegistrationStatusTypeId.ReturnToProvider);
            }
        }
        return toReturn;
    }

    public static string ApplicationStatusType(int regId)
    {
        //DLF: This is a guess. I spoke with Teresa and Mark Ham to gather as much information as I can. This is supposed to reflect
        //the mapping in https://spweb.ezinfoshare.com/sites/PMSGHC/TennCare%20Documents/TN%20PDMS/Design/Current%20Design%20Documents/PDMS%20II%20Internal%20Processing/PDMS%20II%20Workflow_Statuses_Actions.xlsx
        //in the "Registration - New Provider" tab in the External Application Status Display column using the workflow task name (per Teresa).
        //However, the tasks to not correspond to the names in the doc and Registration status may or may not be directly mapped to Application status.
        //Changing the workflow to be consistent with the documentation will cost too much right now.

        string toReturn = "Submitted";

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistration(regId);
        int status;
        if (Helper.HasRows(ds))
        {
            if (int.TryParse(Helper.GetData("REGISTRATION_STATUS_TYPE_ID", ds.Tables[0].Rows[0]), out status))
            {
                if (status == CON.RegistrationStatusTypeId.Approved)
                {
                    toReturn = "Approved";
                }
                else if (status == CON.RegistrationStatusTypeId.ReturnToProvider)
                {
                    toReturn = "Returned";
                }
                else if (status == CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit)
                {
                    toReturn = "Returned by Sitevisit";
                }
                else if (status == CON.RegistrationStatusTypeId.Pending)
                {
                    toReturn = "Not Submitted";
                }
                else
                {
                    toReturn = "Submitted";
                }
            }
            else
            {
                toReturn = "Not Submitted";
            }
        }

        return toReturn;
    }

    // Mask the inputed first characters leaving only "digitsToShow" as the number of digits to present.
    public static string MaskValue(string input, int digitsToShow)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        string rtn = input;
        if (rtn.Length > digitsToShow)
        {
            int len = rtn.Length - digitsToShow;
            rtn = new string('*', len) + rtn.Substring(len);
        }
        return rtn;
    }

    public static string GetUserName(object guid)
    {
        if (string.IsNullOrEmpty(guid.ToString()))
        {
            return string.Empty;
        }
        else
        {
            Guid g;
            if (Guid.TryParse(guid.ToString(), out g))
            {
                MembershipUser u = Membership.GetUser(g);
                if (u != null)
                {
                    return u.UserName;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public static string GetProviderTypeName(int regID)
    {
        string toReturn;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        toReturn = svc.GetProviderTypeByRegId(regID);

        return toReturn;
    }

    public static DataRow GetUserAccountInfo(string userId)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetUserAccountInformation(userId);
        if (!HasRows(ds)) return null;
        else return ds.Tables[0].Rows[0];
    }

    //TODO:  get npi by userid may find multiple records for one user to multiple provider environment.  Returns first one here.
    public static string GetNPI(string userId)
    {
        string toReturn = "";

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        DataSet ds = svc.GetUserAccountInformation(userId);
        if (HasRows(ds))
        {

            toReturn = GetString("NPI", ds.Tables[0].Rows[0]);
        }

        return toReturn;
    }

    public static bool EnableSpecialtyType(int specialtyTypeID)
    {
        bool enable = true;
        if (specialtyTypeID == CON.SpecialtyTypeID.ICF || specialtyTypeID == CON.SpecialtyTypeID.SNF
            || specialtyTypeID == CON.SpecialtyTypeID.ICFOther || specialtyTypeID == CON.SpecialtyTypeID.SNFOther)
        {
            enable = false;
        }
        return enable;
    }

    public static List<T> GetConstantValues<T>(Type type)
    {
        FieldInfo[] fields = type.GetFields(BindingFlags.Public
            | BindingFlags.Static
            | BindingFlags.FlattenHierarchy);

        return (fields.Where(fieldInfo => fieldInfo.IsLiteral
            && !fieldInfo.IsInitOnly
            && fieldInfo.FieldType == typeof(T)).Select(fi => (T)fi.GetRawConstantValue())).ToList();
    }

    public static bool ContainsColumn(string columnName, DataTable table)
    {
        bool found = false;
        DataColumnCollection columns = table.Columns;

        if (columns.Contains(columnName))
        {
            found = true;
        }
        return found;
    }

    public static bool IsRevalDue(string revalDateValue)
    {
        //Revalidation is due for the following reasons:
        //a converted Provider whose End Date (Revalidation Due Date) is either null, 12/31/9999 or with 120 days of current system date.

        bool revalidationNeeded = false;

        DateTime revalDate;
        if (string.IsNullOrEmpty(revalDateValue))
            return true;

        if (!DateTime.TryParse(revalDateValue, out revalDate))
        {
            //invalid date - force to revalidate
            return true;
        }

        string revalWindow = Helper.GetAppSetting(CON.AppSettingsKeyName.RevalidationDueWindow, "120");
        int dueWindow = string.IsNullOrEmpty(revalWindow) ? 1 : Convert.ToInt32(revalWindow) * -1;
        if (revalDate.ToShortDateString() == "12/31/9999" || revalDate <= DateTime.Today || revalDate.AddDays(dueWindow) <= DateTime.Today)
        {
            revalidationNeeded = true;
        }
        return revalidationNeeded;
    }

    public static bool IsWaiverServiceProvider(int regId)
    {
        int diddReferralId = 0;
        int diddReferralTypeId = 0;
        DataRow row = Registration.GetRegistration(regId);

        if (row != null)
        {
            diddReferralId = Helper.GetInt("DIDD_REFERRAL_ID", row);
            diddReferralTypeId = Helper.GetInt("DIDD_REFERRAL_TYPE_ID", row);
        }

        return (diddReferralId > 0 && diddReferralTypeId != CON.DiddReferralType.AssistedLiving);
    }

    public static bool IsProviderOfMMISProviderType(int providerTypeID, string mmisProviderTypeID)
    {
        bool rtn = false;

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectProviderTypesByMMISProviderTypeID(mmisProviderTypeID);

        if (Helper.HasRows(ds))
        {
            DataRow[] rowsFound = ds.Tables[0].Select(string.Format("PROVIDER_TYPE_ID = {0}", providerTypeID));
            rtn = (rowsFound != null && rowsFound.Length > 0);
        }

        return rtn;
    }

    public static List<KeyValuePair<Guid, string>> GetUserRoles(string userName)
    {
        List<KeyValuePair<Guid, string>> retRoles = new List<KeyValuePair<Guid, string>>();
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetUserRolesByUser(userName);
        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                retRoles.Add(new KeyValuePair<Guid, string>(new Guid(Helper.GetString("RoleId", dr)), Helper.GetString("RoleName", dr)));
            }
        }

        return retRoles;
    }

    public static DataTable GetServiceLocationColumns(DataTable dtServiceLocation, int addressType)
    {
        DataTable rtn = null;
        if (dtServiceLocation != null)
        {
            rtn = dtServiceLocation.Copy();

            for (int i = rtn.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn col = rtn.Columns[i];
                if (addressType == CON.AddressType.PrimaryPractice)
                {
                    if (!col.ColumnName.Contains("SERVICING") && !col.ColumnName.Contains("ContactCustom") && !col.ColumnName.Contains("CBSA") && !col.ColumnName.Equals("REQUESTED_EFFECTIVE_DATE") && !col.ColumnName.Equals("REG_SERVICE_LOCATION_ID"))
                        rtn.Columns.Remove(col);
                }
                if (addressType == CON.AddressType.Billing)
                {
                    if (!col.ColumnName.StartsWith("PAYTO") && !col.ColumnName.Equals("ContactCustom") && !col.ColumnName.Equals("REG_SERVICE_LOCATION_ID"))
                        rtn.Columns.Remove(col);
                }
                if (addressType == CON.AddressType.Correspondence)
                {
                    if (!col.ColumnName.StartsWith("MAILTO") && !col.ColumnName.Contains("ContactCustom") && !col.ColumnName.Equals("REG_SERVICE_LOCATION_ID"))
                        rtn.Columns.Remove(col);
                }
                if (addressType == CON.AddressType.Remittance)
                {
                    if (!col.ColumnName.StartsWith("REMITTANCE") && !col.ColumnName.Contains("ContactCustom") && !col.ColumnName.Equals("REG_SERVICE_LOCATION_ID"))
                        rtn.Columns.Remove(col);
                }
                if (addressType == CON.AddressType.Other)
                {
                    if (!col.ColumnName.StartsWith("OTHER") && !col.ColumnName.Contains("ContactCustom") && !col.ColumnName.Equals("REG_SERVICE_LOCATION_ID"))
                        rtn.Columns.Remove(col);
                }
            }

            if (Helper.HasRows(rtn))
            {
                bool isEmpty = true;
                foreach (DataColumn col in rtn.Columns)
                {
                    if (Helper.GetString(col.ColumnName, rtn.Rows[0]).Trim() != string.Empty)
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                    rtn.Rows[0].Delete();
            }

            rtn.AcceptChanges();

        }

        return rtn;
    }


    public static string GetFormattedContact(string name, string email, string phone)
    {
        string contact = string.Empty;

        if (!string.IsNullOrEmpty(name.ToString()))
        {
            contact = "Name:" + name.ToString();
        }
        if (!string.IsNullOrEmpty(email.ToString()))
        {
            contact = string.IsNullOrEmpty(contact) ? "Email:" + email.ToString() : contact + ",<br/>" + "Email:" + email.ToString();
        }
        if (!string.IsNullOrEmpty(phone.ToString()))
        {
            contact = string.IsNullOrEmpty(contact) ? "Phone:" + phone.ToString() : contact + ",<br/>" + "Phone:" + FormatPhone(phone.ToString());
        }

        return contact;
    }
    public static bool ShowCredential(int regID)
    {
        DataRow row = Registration.GetRegistration(regID);
        int entityTypeId = 0;
        bool retVal = false;
        try
        {
            if (row != null)
            {
                entityTypeId = Helper.GetInt("ENTITY_TYPE_ID", row);
                if (entityTypeId == 1)
                {
                    retVal = true;
                }
            }
        }
        catch (Exception)
        {
        }
        return retVal;
    }

    public static int GetPeningCommitteememberStatusCount(int regid)
    {
        int retval = 0;
        try
        {
            using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            {
                DataSet ds = svc.SelectCredentialingCommitteeActivity(regid);

                if (HasRows(ds))
                {
                    DataView dvPending = ds.Tables[0].DefaultView;
                    dvPending.RowFilter = "COMMITTEE_ACTION_STATUS_ID =" + Convert.ToInt32(CON.CommitteeCredentialActivityStatusId.Pending.ToString());
                    retval = dvPending.ToTable().Rows.Count;
                }

            }
        }
        catch (Exception)
        {


        }

        return retval;
    }
    public static string GetNextPeningCommitteemember(int regid)
    {
        string committeeUser = string.Empty;
        try
        {
            using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            {
                DataSet ds = svc.SelectCredentialingCommitteeActivity(regid);

                if (HasRows(ds))
                {
                    DataView dvPending = ds.Tables[0].DefaultView;
                    dvPending.RowFilter = "COMMITTEE_ACTION_STATUS_ID =" + Convert.ToInt32(CON.CommitteeCredentialActivityStatusId.Pending.ToString()) + "and MEMBER_RANK<>1";
                    dvPending.Sort = "MEMBER_RANK DESC";
                    int retval = dvPending.ToTable().Rows.Count;
                    if (retval > 0)
                    {
                        committeeUser = dvPending.ToTable().Rows[0]["MEMBER_USERNAME"].ToString();
                    }

                }

            }
        }
        catch (Exception)
        {


        }

        return committeeUser;
    }

    //public static bool isUserRoleinCredentialRoles(string UserRole)
    //{
    //    bool retVal = false;
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(UserRole))
    //        {
    //            if (UserRole == CON.CredentialingRoles)
    //            {
    //                retVal = true;
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    return retVal;
    //}

    //public static bool isUserNameinCredentialRoles(string UserName)
    //{
    //    bool retVal = false;
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(UserName))
    //        {
    //            if (IsUserInRole(UserName, CON.CredentialingRoles))
    //            {
    //                retVal = true;
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    return retVal;
    //}

    /// <summary>
    /// Gets Foramatted Address
    /// </summary>
    /// <param name="addressLine1">Address Line1</param>
    /// <param name="addressLine2">Address Line2</param>
    /// <param name="addressLine1">City</param>
    /// <param name="addressLine2">State</param>
    /// <param name="addressLine1">Zip</param>
    /// <param name="addressLine2">Extenstion Zip</param>
    /// <returns>Formmated 2 lines address</returns>
    public static string GetFormattedAddress(string addressLine1, string addressLine2, string city, string state, string zip, string zip4, string phone)
    {
        string retAddress = string.Empty;
        var sbAddresss = new StringBuilder();
        sbAddresss.Append(addressLine1);
        sbAddresss.Append("<br/>");
        sbAddresss.AppendFormat("{0} {1}, {2} {3}", addressLine2, city, state, zip);
        retAddress = string.IsNullOrEmpty(zip4) ? sbAddresss.ToString().Trim() : sbAddresss.AppendFormat("- {0}", zip4).ToString().Trim();
        retAddress += string.IsNullOrEmpty(phone) ? string.Empty : "<br/>" + Regex.Replace(phone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
        return retAddress;

    }

    public static string FormatEmailAddressList(string email)
    {
        char[] delimeterChars = { ';', ',' };
        return email.TrimStart(delimeterChars).TrimEnd(delimeterChars);
    }

    public static string FormatZipcode(string zip)
    {
        string outzip = "";
        if (zip.Length == 9)
        {
            outzip = zip.Substring(0, 5) + "-" + zip.Substring(5, 4);
        }
        else if (zip.Length == 5)
        {
            outzip = zip.Substring(0, 5);
        }
        else
        {
            outzip = zip;
        }
        return outzip;
    }



    public static string RedirectMyProfileURL()
    {
        return "~/Account/MyProfile.aspx";
    }

    public static bool IsMITSuser(string userId)
    {
        bool result = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetUserAccountInformation(userId);
        if (Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                result = !Helper.GetBool("IS_OHID", row);
            }
        }
        return result;
    }
    public static bool IsApprovedDelegate(string userId)
    {
        bool result = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetUserAccountInformation(userId);
        if (Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                result = Helper.GetBool("APPROVED_DELEGATE", row);
            }
        }
        return result;
    }
    public static bool IsMITSuserForce_Password_Reset(string userId)
    {
        bool result = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetUserAccountInformation(userId);
        if (Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                result = !Helper.GetBool("IS_OHID", row) && Helper.GetBool("FORCE_PASSWORD_RESET", row);
            }
        }
        return result;
    }
    public static bool IsTempPasswordSent(string userId)
    {
        bool result = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetUserAccountInformation(userId);
        if (Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                result = Helper.GetBool("IS_TEMP_PWD_SENT", row);
            }
        }
        return result;
    }
    public static DataTable GetPasswordExpiryInfo(string Username)
    {
        DataTable dtAccountInfo = null;
        try
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsUserInfo = svc.GetUsermembershipInfoByUserName(Username);

            if (Helper.HasRows(dsUserInfo))
            {
                dtAccountInfo = dsUserInfo.Tables[0];
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dtAccountInfo;
    }

    //check password expiry applicable to the current role

    public static bool IsPasswordExpiryApplicable(string Username)
    {
        bool isApplicable = true;
        const char delimiter1 = ',';
        const char delimiter2 = ';';
        try
        {

            List<KeyValuePair<Guid, string>> listCurrentRoles = GetUserRolesByUser(Username);
            string strExceptionRole = AppSettings.Get("PasswordExpiryExceptionRoles");
            List<string> userroles = (from lst in listCurrentRoles select lst.Value).Distinct().ToList();
            string[] strUserRoles = userroles.ToArray();

            char delimiter = delimiter1;
            if (strExceptionRole.IndexOf(delimiter1) == -1) delimiter = delimiter2;
            string[] exceptionRoles = strExceptionRole.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            var matchedRoles = strUserRoles.Intersect(exceptionRoles, StringComparer.OrdinalIgnoreCase).Count();

            if (matchedRoles > 0)
                isApplicable = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return isApplicable;
    }

    public static bool IsPasswordExpiredAndInactivated(string username)
    {
        bool retValInactivated = false;
        DataTable dtPwdInfo = null;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        int PwdChangeddays = 0;
        int PwdChangeLimit = 0;
        int priorNoticeDays = 0;
        try
        {
            dtPwdInfo = Helper.GetPasswordExpiryInfo(username);

            if (Helper.HasRows(dtPwdInfo))
            {
                DataRow dtRow = dtPwdInfo.Rows[0];
                PwdChangeddays = (dtRow["DayCount"] != null) ? Convert.ToInt32(dtPwdInfo.Rows[0]["DayCount"].ToString()) : 0;
                PwdChangeLimit = (dtRow["ExpiryLimitDays"] != null) ? Convert.ToInt32(dtPwdInfo.Rows[0]["ExpiryLimitDays"].ToString()) : 0;
                priorNoticeDays = (dtRow["PriorNoticeDays"] != null) ? Convert.ToInt32(dtPwdInfo.Rows[0]["PriorNoticeDays"].ToString()) : 0;

                if (PwdChangeddays > PwdChangeLimit)
                {
                    //Password crossed pwd expiry limit

                    string disableAccount = Helper.GetAppSettingFromDB(CON.AppSettingsKeyName.DisableAccountAfterPwdExpiry, "false");
                    bool bDisableAcct = string.IsNullOrEmpty(disableAccount) ? false : Convert.ToBoolean(disableAccount);
                    if (bDisableAcct)
                    {
                        string DaysLimitForDisable = Helper.GetAppSettingFromDB(CON.AppSettingsKeyName.DisableAccountAfterPwdExpiryDays);
                        if (!string.IsNullOrEmpty(DaysLimitForDisable))
                        {
                            if (PwdChangeddays > Convert.ToInt32(DaysLimitForDisable))
                            {
                                retValInactivated = true;
                            }
                        }
                    }
                }
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retValInactivated;
    }
    public static bool IsPasswordExpired(string username)
    {
        bool retVal = false;

        try
        {
            if (IsPasswordExpiryApplicable(username))
            {
                DataTable dtPwdInfo = Helper.GetPasswordExpiryInfo(username);

                if (Helper.HasRows(dtPwdInfo))
                {
                    DataRow dtRow = dtPwdInfo.Rows[0];
                    int PwdChangeddays = (dtRow["DayCount"] != null) ? Convert.ToInt32(dtPwdInfo.Rows[0]["DayCount"].ToString()) : 0;
                    int PwdChangeLimit = (dtRow["ExpiryLimitDays"] != null) ? Convert.ToInt32(dtPwdInfo.Rows[0]["ExpiryLimitDays"].ToString()) : 0;

                    if (PwdChangeddays > PwdChangeLimit)
                    {
                        retVal = true;
                    }

                }
            }

        }
        catch (Exception ex)
        {
            throw ex;

        }

        return retVal;
    }


    /// <summary>
    /// Gets a integer days from a given hours
    /// default value is 0
    /// </summary>
    /// <param name="Hours">hours in integer</param>
    /// <returns>returns integer days from given hours</returns>
    public static int ConvertHoursToDays(int Hours)
    {
        int days = 0;
        try
        {
            if (Hours > 0)
            {
                days = Decimal.ToInt32(Hours / 24);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return days;
    }

    /// <summary>
    /// Gets List item if found by text(case insensitive compare) in listcontrol 
    /// default value is null
    /// </summary>
    /// <param name="ListControl">ListControl</param>
    /// <param name="text">string</param>
    /// <returns>returns listitem</returns>
    public static ListItem FindByTextCaseInsensitive(ListControl lstControl, string text)
    {
        ListItem li = null;
        try
        {
            if (lstControl != null)
            {
                foreach (ListItem litem in lstControl.Items)
                {
                    if (string.Compare(litem.Text.Trim(), text.Trim(), true) == 0)
                    {
                        li = litem;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return li;
    }
    public static string GetFomattedLicenseSpecialtyFocusInfo(DataTable dtFocusInfo)
    {
        string specialtyFocus = string.Empty;
        try
        {
            if (dtFocusInfo.Rows.Count > 0)
            {


                foreach (DataRow dr in dtFocusInfo.Rows)
                {
                    if (!string.IsNullOrEmpty(Helper.GetString("ENDORSEMENT_NUMBER", dr)))
                    {

                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Number:" + Helper.GetString("ENDORSEMENT_NUMBER", dr) : specialtyFocus + "<Br/>" + " Number: " + Helper.GetString("ENDORSEMENT_NUMBER", dr);
                    }
                    if (!string.IsNullOrEmpty(Helper.GetString("ENDORSEMENT_SPECIALITY", dr)))
                    {

                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Specialty:" + Helper.GetString("ENDORSEMENT_SPECIALITY", dr) : specialtyFocus + "<Br/>" + " Specialty: " + Helper.GetString("ENDORSEMENT_SPECIALITY", dr);
                    }
                    if (!string.IsNullOrEmpty(Helper.GetString("ENDORSEMENT_FOCUS", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Focus:" + Helper.GetString("ENDORSEMENT_FOCUS", dr) : specialtyFocus + "<Br/>" + " Focus: " + Helper.GetString("ENDORSEMENT_FOCUS", dr);
                    }
                    if (!string.IsNullOrEmpty(Helper.GetString("ENDORSEMENT_STATUS", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Status:" + Helper.GetString("ENDORSEMENT_STATUS", dr) : specialtyFocus + "<Br/>" + " Status: " + Helper.GetString("ENDORSEMENT_STATUS", dr);
                    }
                    if (!string.IsNullOrEmpty(Helper.GetString("CERTIFYING_ORGANIZATION", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Certifying Org:" + Helper.GetString("CERTIFYING_ORGANIZATION", dr) : specialtyFocus + "<Br/>" + " Certifying Org: " + Helper.GetString("CERTIFYING_ORGANIZATION", dr);
                    }
                    if (!string.IsNullOrEmpty(Helper.GetString("CERTIFICATE_DATE", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Certification Date:" + Helper.GetDate("CERTIFICATE_DATE", dr).ToString() : specialtyFocus + "<Br/>" + " Certification Date: " + Helper.GetDate("CERTIFICATE_DATE", dr).ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetString("CERTIFICATE_EXPIRATION", dr)))
                    {
                        specialtyFocus = string.IsNullOrEmpty(specialtyFocus) ? "Certificate Expiration:" + Helper.GetDate("CERTIFICATE_EXPIRATION", dr).ToString() : specialtyFocus + "<Br/>" + " Certificate Expiration: " + Helper.GetDate("CERTIFICATE_EXPIRATION", dr).ToString();
                    }


                }

            }
        }
        catch (Exception)
        {

        }
        return specialtyFocus;
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
    public static bool IsUserInRestrictedServiceUpdateRole(string username)
    {
        bool updateRole = Roles.IsUserInRole(username, CON.UserRoleType.Administrator)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.EnrollmentSpecialist)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.ComplianceSpecialist);

        return updateRole;

    }

    public static bool IsUserInRoleForUpdatingSpans(string username)
    {
        bool updateRole = Roles.IsUserInRole(username, CON.UserRoleType.Administrator)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.EnrollmentSpecialist)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.ComplianceSpecialist)
                                       || Roles.IsUserInRole(username, CON.UserRoleType.LTCInitialReval);

        return updateRole;
    }

    public static bool ShowActionsButtions { get; set; }


    public static bool HasActiveWorkflow(int regID, int WorkflowId)
    {
        bool bInWorkFlow = false;
        int wfID = 0;
        using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
        {
            DataSet dsWorkflow = svc.GetWFProcessByRegId(regID);

            if (Helper.HasRows(dsWorkflow) && Helper.HasRows(dsWorkflow.Tables[0]))
            {

                wfID = Helper.GetInt("WORKFLOW_ID", dsWorkflow.Tables[0].Rows[0]);

                if (wfID == WorkflowId)
                {
                    bInWorkFlow = true;
                }
            }
        }
        return bInWorkFlow;

    }

    public static bool IsUserInLTCCHOPRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.UserRoleType.LTCCHOP))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsUserInProviderServicesRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.UserRoleType.ProviderServices))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsUserInReadOnlyRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.UserRoleType.ReadOnly))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsUserInInternalApplicationsEntryRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.UserRoleType.InternalApplicationsEntry))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsUserInTechAdminRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.UserRoleType.TechAdminMAX))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Checks if provider is in Allowed waiver services in ODM provider types
    /// default value is FALSE
    /// </summary>
    /// <param name="mmisProviderTypeId">string</param>
    /// <returns>returns bool</returns>
    public static bool IsProviderTypeInODMForWaiverServiceUpdate(string mmisProviderTypeId)
    {
        bool retVal = false;
        string strODMProviderTypes = GetAppSettingFromDB("ODMProviderTypesAllowedforWaiverServices");

        string[] ProviderTypes = strODMProviderTypes.Split(',');
        foreach (var providertype in ProviderTypes)
        {
            if (providertype.ToString().Trim() == mmisProviderTypeId.Trim())
            {
                retVal = true;
                break;

            }
        }

        return retVal;
    }
    /// <summary>
    /// Checks if provider is in Allowed waiver services in DODD provider types 
    /// default value is FALSE
    /// </summary>
    /// <param name="mmisProviderTypeId">string</param>  
    /// <returns>returns bool</returns>
    public static bool IsProviderTypeInDODDForWaiverServiceUpdate(string mmisProviderTypeId)
    {
        bool retVal = false;
        string strDODDProviderTypes = GetAppSettingFromDB("DODDProviderTypesAllowedforWaiverServices");
        bool enableCR318 = Convert.ToBoolean(GetAppSettingFromDB("EnableCR318"));

        string[] ProviderTypes = strDODDProviderTypes.Split(',');
        foreach (var providertype in ProviderTypes)
        {
            if ((providertype.ToString().Trim() == mmisProviderTypeId.Trim() && mmisProviderTypeId.Trim() != CON.MMISProviderType.Behavioral_Health_Para_Professionals)
                 || (mmisProviderTypeId.Trim() == CON.MMISProviderType.Behavioral_Health_Para_Professionals && enableCR318 && providertype.ToString().Trim() == mmisProviderTypeId.Trim()))
            {
                retVal = true;
                break;

            }
        }
        return retVal;
    }
    /// <summary>
    /// Checks if provider is in Allowed waiver services in ODA provider types 
    /// default value is FALSE
    /// </summary>
    /// <param name="mmisProviderTypeId">string</param>
    /// <returns>returns bool</returns>
    public static bool IsProviderTypeInODAForWaiverServiceUpdate(string mmisProviderTypeId)
    {
        bool retVal = false;
        string strODAProviderTypes = GetAppSettingFromDB("ODAProviderTypesAllowedforWaiverServices");

        string[] ProviderTypes = strODAProviderTypes.Split(',');
        foreach (var providertype in ProviderTypes)
        {
            if (providertype.ToString().Trim() == mmisProviderTypeId.Trim())
            {
                retVal = true;
                break;

            }
        }
        return retVal;
    }

    /// <summary>
    /// if date is sql min  date will return as empty string
    /// else return date as m/d/yyyy format
    /// </summary>
    /// <param name="obj">date obj</param>
    /// <returns>returns string date with m/d/yyyy format</returns>
    public static string GetDisplayFormatDate(object obj)
    {
        string dateDisplay = string.Empty;
        DateTime sqlMinDateTime = new DateTime(1753, 1, 1);
        try
        {

            #region default date check
            if (obj != null)
            {
                if (Convert.ToDateTime(obj).ToString("MM/dd/yyyy") != sqlMinDateTime.ToString("MM/dd/yyyy"))
                {
                    dateDisplay = Convert.ToDateTime(obj).ToString("M/d/yyyy");
                }
            }
            #endregion
        }
        catch (Exception)
        {

        }
        return dateDisplay;
    }
    public static bool isCPCEnrollmentPeriod()
    {
        bool isEnrollmentPeriod = false;
        string CPCEnrollmentPeriodStartDate = AppSettings.Get("CPCEnrollmentPeriodStartDate") + "/" + DateTime.Now.Year;
        string CPCEnrollmentPeriodEndDate = AppSettings.Get("CPCEnrollmentPeriodEndDate") + "/" + DateTime.Now.Year;
        DateTime beginDate = DateTime.ParseExact(CPCEnrollmentPeriodStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        DateTime endDate = DateTime.ParseExact(CPCEnrollmentPeriodEndDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        DateTime todayDate = DateTime.Now.Date;
        if (todayDate >= beginDate && todayDate <= endDate)
        {
            isEnrollmentPeriod = true;
        }
        return isEnrollmentPeriod;
    }

    public static bool isCMCEnrollmentPeriod()
    {
        bool isEnrollmentPeriod = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.CheckCMCEnrollmentPeriod();
        bool enableEnrollmentLink = Helper.HasRows(ds) ? Helper.GetBool("CMCEnrollmentEnabled", ds.Tables[0].Rows[0]) : false;
        if (enableEnrollmentLink)
        {
            isEnrollmentPeriod = true;
        }
        return isEnrollmentPeriod;
    }
    public static bool isCMCInvited(int RegId)
    {
        bool isInvited = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.CheckCMCInvited(RegId);
        if (Helper.HasRows(ds))
        {
            isInvited = true;
        }
        return isInvited;
    }

    public static bool IsCMCEnrolled(int RegId)
    {
        bool isCMCEnrolled = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.IsCMCEnrolled(RegId);
        if (Helper.HasRows(ds))
        {
            isCMCEnrolled = true;
        }
        return isCMCEnrolled;
    }

    public static bool isCPCLinkReenabled(int regId)
    {
        bool isReenabled = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistration(regId);
        bool enableAttestationLink = Helper.HasRows(ds) ? Helper.GetBool("IS_CPC_LINK_REENABLED", ds.Tables[0].Rows[0]) : false;
        if (enableAttestationLink)
        {
            isReenabled = true;
        }
        return isReenabled;
    }

    public static bool isCMCLinkReenabled(int regId)
    {
        bool isReenabled = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.CheckCMCLinkEnabledByRegID(regId);
        bool enableAttestationLink = Helper.HasRows(ds) ? Helper.GetBool("IS_CMC_LINK_REENABLED", ds.Tables[0].Rows[0]) : false;
        if (enableAttestationLink)
        {
            isReenabled = true;
        }
        return isReenabled;
    }

    public static bool IsDODDInitialApplication(int regID)
    {
        bool isDoddInitialSvcsApp = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        isDoddInitialSvcsApp = svc.CheckIfDODDInitialApplication(regID);
        return isDoddInitialSvcsApp;
    }
    public static bool IsODAInitialApplication(int regID)
    {
        bool isOdaInitialSvcsApp = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        isOdaInitialSvcsApp = svc.CheckIfODAInitialApplication(regID);
        return isOdaInitialSvcsApp;
    }
    public static bool IsUserInInternalRole(string roleNames)
    {
        string[] userRoles = Roles.GetRolesForUser();
        string[] rolesToCheck = roleNames.Split(',');


        foreach (string roleToCheck in rolesToCheck)
        {
            foreach (string oneOfTheirRoles in userRoles)
            {
                if (String.Equals(roleToCheck, oneOfTheirRoles))
                {
                    return true;

                }
            }
        }
        return false;
    }

    // OHPNM-7338
    public static bool IsUserInLTCInitialRevalidationRole(string username)
    {
        if (Roles.IsUserInRole(username, CON.UserRole.LTCInitialRevalidation))
            return true;

        return false;
    }

    public static bool ProviderTypeAllowedToViewLink(string MMISProviderTypeID, List<string> allowedProviderTypes)
    {
        if (allowedProviderTypes.Contains(MMISProviderTypeID))
        {
            return true;
        }
        return false;
    }

    public static bool IsUpdateCPCContact { get; set; }

    public static bool IsUpdateCMCContact { get; set; }

    public static string CleanTextOnlyKeyboardCharacters(string strIn)
    {
        // Only allow characters that are on the standard keyboard - Removes any control charcters
        // Replace invalid characters with empty strings.
        try
        {

            string retStr = Regex.Replace(strIn, @"([^a-zA-Z0-9_\.!@#$%&*()[\]{}\\:;?<>=+,~'/ ""]+)", "", RegexOptions.None, TimeSpan.FromSeconds(3.0));
            return retStr.Trim();
        }
        // If we timeout when replacing invalid characters,
        // we should return string that was passed.
        catch (RegexMatchTimeoutException)
        {
            return strIn;
        }
    }

    /// <summary>
    /// check whether the NPI has any other active registrations or not. Based on this Reapplciation and Reactive will be hidden.
    /// </summary>
    /// <param name="NPI"></param>
    /// <param name="regId"></param>
    /// <returns></returns>
    public static bool IsEligibleForReapplicationOrReactivation(string NPI, int regId, string MMISProviderTypeID)
    {
        bool isEligible = true;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet dsExistingNPIData = svc.GetExistingRegDataForNPI(NPI, regId, MMISProviderTypeID, false);
        if (Helper.HasRows(dsExistingNPIData))
        {
            foreach (DataRow dr in dsExistingNPIData.Tables[0].Rows)
            {
                //Check enrollment status is active then disable Reapplication functionality
                if (dr["ENROLLMENT_STATUS_CODE"] != DBNull.Value && Convert.ToInt16(dr["ENROLLMENT_STATUS_CODE"]) != CON.EnrollmentStatusTypeID.InActive && Convert.ToString(dr["MMIS_PROVIDER_TYPE_ID"]) != "LT")
                {
                    isEligible = false;
                    break;
                }
            }
        }
        return isEligible;
    }

    /// <summary>
    /// check whether the given registration can start convert from ORP process.
    /// </summary>
    /// <returns></returns>
    public static bool IsEligibleConvertFromORP(int regId)
    {
        bool isEligible = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        svc.CheckEligibletoConvertFromORP(regId, out isEligible);
        return isEligible;
    }

    public static bool IsShowProviderTypeChangeLink(string mmisProviderTypeId)
    {
        bool retVal = false;
        string strDODDProviderTypes = GetAppSettingFromDB("ProviderTypesAllowedToChangeType");

        string[] ProviderTypes = strDODDProviderTypes.Split(',');
        foreach (var providertype in ProviderTypes)
        {
            if (providertype.ToString().Trim().Replace("'", "") == mmisProviderTypeId.Trim())
            {
                retVal = true;
                break;

            }
        }
        return retVal;
    }

    public static string Encrypt(string plainText)
    {
        //Secret Key.
        string secretKey = "$ASPcAwSNIgcPPEoTSa0ODw#";

        //Secret Bytes.
        byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);

        //Plain Text Bytes.
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        //Encrypt with AES Alogorithm using Secret Key.
        using (Aes aes = Aes.Create())
        {
            aes.Key = secretBytes;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            byte[] encryptedBytes = null;
            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                encryptedBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
            }

            return Convert.ToBase64String(encryptedBytes);
        }
    }
    public static string Decrypt(string encryptedText)
    {
        //Secret Key.
        string secretKey = "$ASPcAwSNIgcPPEoTSa0ODw#";

        //Secret Bytes.
        byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);

        encryptedText = encryptedText.Replace(' ', '+');

        //Encrypted Bytes.
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        //Decrypt with AES Alogorithm using Secret Key.
        using (Aes aes = Aes.Create())
        {
            aes.Key = secretBytes;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            byte[] decryptedBytes = null;
            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            {
                decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

    // SAM537 - Revert Suspension workflow.
    public static bool IsRevertSuspensionWF(int ProcessID)
    {
        bool rtn = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.WF_SelectProcessParameters(ProcessID);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            rtn = string.IsNullOrEmpty(dr[CON.ProcessParameter.IsRevertSuspension].ToString()) ? false : Convert.ToBoolean(dr[CON.ProcessParameter.IsRevertSuspension]);
        }
        return rtn;
    }

    public static bool IsUserPowerAgent(string loggedinUserID, string provAdminUserID, int regID)
    {
        bool isPowerAgent = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetPowerAgentInformation(loggedinUserID, provAdminUserID, regID);
        if (Helper.HasRows(ds)) 
        {
            isPowerAgent = true;
        }
        return isPowerAgent;

    }
    public static bool IsUserPowerAgentWithMultipleProviderAdmin(string loggedinUserID, string provAdminUserID, int regID)
    {
        bool isPowerAgent = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetPowerAgentInformation(loggedinUserID, provAdminUserID, regID);
        if (Helper.HasRows(ds))
        {
            isPowerAgent = ds.Tables[0].Rows.Count > 1;
        }
        return isPowerAgent;

    }

    public static bool HasPowerAgentAccessMgmtRole(string loggedinUserID, string provAdminUserID, int regID)
    {
        bool isPowerAgentAccessMgmt = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetPowerAgentInformation(loggedinUserID, provAdminUserID, regID);
        if (Helper.HasRows(ds))
        {
            isPowerAgentAccessMgmt = ds.Tables[0].AsEnumerable()
                                          .Where(r => (r["ACCESS_MANAGEMENT"] != DBNull.Value ? Convert.ToBoolean(r["ACCESS_MANAGEMENT"]) : true) == true)
                                          .Any();
        }
        return isPowerAgentAccessMgmt;

    }

    public static bool IsProviderTypeChangeWfInProgress { get; set; }


    public static T TryGetValueAtIndex<T>(IList<T> list, int index)
    {
        return (list != null && index >= 0 && index < list.Count) ? list[index] : default(T);
    }
}
