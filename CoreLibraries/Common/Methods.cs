using Corp.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.Core.Libraries
{
    public static class Methods
    {
        public static bool GetBoolFromCAQHAffirmed(string value)
        {
            if (value == Constants.BooleanCAQH.True)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidXml(string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool GetBoolFromDataModelAffirmed(string value)
        {
            if (value == Constants.BooleanDataModel.True)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool HasRows(DataSet ds)
        {
            return (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
        }

        public static bool HasRows(DataTable dt)
        {
            if (dt == null) return false;
            if (dt.Rows.Count == 0) return false;
            return true;
        }

        public static bool GetBoolean(object value)
        {
            string stringVal = string.Empty;
            bool returnVal = false;
            string boolTrue = bool.TrueString.ToUpper();
            string caqhTrue = Constants.BooleanCAQH.True;
            string dmTrue = Constants.BooleanDataModel.True.ToUpper();

            try
            {
                stringVal = GetStringValue(value).ToUpper();

                if (stringVal == "1" || stringVal == boolTrue || stringVal == caqhTrue || stringVal == dmTrue)
                {
                    returnVal = true;
                }
            }
            catch (Exception ex)
            {
                //  no action
                string msg = ex.Message;
            }
            return returnVal;
        }

        public static Object GetCharValue(object value, bool allowNulls)
        {

            if (IsNullOrZeroLength(value))
            {
                if (allowNulls)
                {
                    return DBNull.Value;
                }
                else
                {
                    return ' ';
                }
            }
            else
            {
                return Convert.ToChar(value);
            }
        }

        public static DateTime GetDateValue(object value)
        {

            if (IsNullOrZeroLength(value))
            {
                return DateTime.MinValue;
            }
            else
            {
                return Convert.ToDateTime(value).Date;
            }
        }

        public static Object GetDateValue(object value, bool allowNulls)
        {

            Object returnVal;

            if (IsNullOrZeroLength(value))
            {
                if (allowNulls)
                {
                    returnVal = DBNull.Value;
                }
                else
                {
                    returnVal = DateTime.MinValue;
                }
            }
            else
            {
                if (allowNulls && value.ToString() == DateTime.MinValue.ToString())
                {
                    returnVal = DBNull.Value;
                }
                else
                {
                    returnVal = Convert.ToDateTime(value).Date;
                }
            }
            return returnVal;
        }

        public static Object GetDateTimeValue(object value, bool allowNulls, string dateTimeFormat)
        {

            if (value.ToString().Length == 0 || Convert.ToDateTime(value) == default(DateTime))
            {
                if (allowNulls)
                {
                    return DBNull.Value;
                }
                else
                {
                    return default(DateTime);
                }
            }
            else
            {
                if (dateTimeFormat.Length == 0)
                {
                    return Convert.ToDateTime(value);
                }
                else
                {
                    return Convert.ToDateTime(value).ToString(dateTimeFormat);
                }
            }
        }

        public static Object GetDateTimeValue(object value, bool allowNulls)
        {

            if (value.ToString().Length == 0 || Convert.ToDateTime(value) == default(DateTime))
            {
                if (allowNulls)
                {
                    return DBNull.Value;
                }
                else
                {
                    return default(DateTime);
                }
            }
            else
            {
                return Convert.ToDateTime(value);
            }
        }

        public static decimal GetDecimalValue(object value)
        {
            decimal returnValue = Convert.ToDecimal(GetDecimalValue(value, false));
            return returnValue;
        }

        public static Object GetDecimalValue(object value, bool allowNulls)
        {

            if (IsNullOrZeroLength(value))
            {
                if (allowNulls)
                {
                    return DBNull.Value;
                }
                else
                {
                    return 0M;
                }
            }
            else
            {
                return Convert.ToDecimal(value);
            }
        }

        public static double GetDoubleValue(object value)
        {
            double returnValue = Convert.ToDouble(GetDoubleValue(value, false));
            return returnValue;
        }

        public static Object GetDoubleValue(object value, bool allowNulls)
        {

            if (IsNullOrZeroLength(value))
            {
                if (allowNulls)
                {
                    return DBNull.Value;
                }
                else
                {
                    return 0M;
                }
            }
            else
            {
                return Convert.ToDouble(value);
            }
        }

        public static string GetChangeTypeString(Enumerations.CAQHChangeType value)
        {
            return Enum.GetName(typeof(Enumerations.CAQHChangeType), value);
        }

        public static Object GetGuidValue(object value, bool allowNulls)
        {

            if (IsNullOrZeroLength(value))
            {
                if (allowNulls)
                {
                    return DBNull.Value;
                }
                else
                {
                    return new Guid(Constants.appAdminUserId);
                }
            }
            else
            {
                return value;
            }
        }


        public static int GetIntValue(object value)
        {
            int returnValue = Convert.ToInt32(GetIntValue(value, false));
            return returnValue;
        }

        public static int GetIntValue(DataRow dr, string columnName)
        {
            if (ColumnExists(columnName, dr))
            {
                return GetIntValue(dr[columnName]);
            }
            else
            {
                return 0;
            }
        }


        public static Object GetIntValue(object value, bool allowNulls)
        {

            if (IsNullOrZeroLength(value))
            {
                if (allowNulls)
                {
                    return DBNull.Value;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }


        public static Object GetIntValue64(object value, bool allowNulls)
        {

            if (IsNullOrZeroLength(value))
            {
                if (allowNulls)
                {
                    return DBNull.Value;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return Convert.ToInt64(value);
            }
        }

        public static string GetShortDate(string date, string format = "MM-dd-yyyy")
        {
            string returnValue = string.Empty;

            DateTime parsedDate;
            if (date.Length > 0)
            {
                if (DateTime.TryParse(date, out parsedDate))
                {
                    returnValue = parsedDate.Date.ToString(format);
                }
            }
            return returnValue;
        }

        public static Object GetStringValue(object value, bool allowNulls)
        {
            if (IsNullOrZeroLength(value))
            {
                if (allowNulls)
                {
                    return DBNull.Value;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return Convert.ToString(value);
            }
        }

        public static string GetStringValue(object value)
        {
            string returnValue = Convert.ToString(GetStringValue(value, false));
            return returnValue;
        }

        public static string GetStringValue(DataRow dr, string columnName)
        {
            if (ColumnExists(columnName, dr))
            {
                return GetStringValue(dr[columnName]);
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        ///     Gets the time in the correct timezone format
        /// </summary>
        /// <param name="TimeZone">The proper time zone, use the Common.Constants.TimeZone enumeration list</param>
        /// <returns>The DateTime in the proper timezone format</returns>
        public static DateTime GetDateTime(string TimeZone)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
            DateTime offsetTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            return offsetTime;
        }

        public static DateTime GetDateValue(DataRow dr, string columnName)
        {
            if (ColumnExists(columnName, dr))
            {
                return GetDateValue(dr[columnName]);
            }
            else
            {
                return default(DateTime);
            }
        }

        public static bool IsNumeric(Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 ||
                Expression is Int64 || Expression is Decimal ||
                Expression is Single || Expression is Double ||
                Expression is Boolean)

                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch
            {
            }
            return false;
        }

        public static bool IsNumeric(Object Expression, int length)
        {
            bool returnVal = false;
            try
            {
                if (!object.Equals(Expression, null))
                {
                    bool numeric = IsNumeric(Expression);
                    if (numeric == true && (Expression.ToString().Length == length))
                    {
                        returnVal = true;
                    }
                }
            }
            catch
            {
            }
            return returnVal;
        }

        public static string Left(string text, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, Constants.CustomExceptionMessages.LengthZero);
            else if (length == 0 || text.Length == 0)
                return string.Empty;
            else if (text.Length <= length)
                return text;
            else
                return text.Substring(0, length);
        }

        public static void CopyFiles(DirectoryInfo source,
                              DirectoryInfo destination,
                              bool overwrite,
                              string searchPattern)
        {
            destination.Create();
            FileInfo[] files = source.GetFiles(searchPattern);

            foreach (FileInfo file in files)
            {
                file.CopyTo(destination.FullName + "\\" + file.Name, overwrite);
            }
        }

        public static void MoveFiles(DirectoryInfo source,
                              DirectoryInfo destination,
                              string searchPattern)
        {
            destination.Create();
            FileInfo[] files = source.GetFiles(searchPattern);

            foreach (FileInfo file in files)
            {
                file.MoveTo(destination.FullName + "\\" + file.Name);
            }
        }

        public static string Right(string text, int length)
        {
            int startPosition;
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, Constants.CustomExceptionMessages.LengthZero);
            else if (length == 0 || text.Length == 0)
                return string.Empty;
            else if (text.Length <= length)
                return text;
            else
                startPosition = text.Length - length;
            return text.Substring(startPosition, length);
        }

        public static string SerializeObjectToXml(object obj)
        {
            string returnVal = string.Empty;
            try
            {
                StringWriter writer = new StringWriter();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj);
                string xml = writer.ToString();
                returnVal = xml.Replace("utf-16", "utf-8");
            }
            catch (Exception ex)
            {
                // set the return value to the exception
                string msg;
                msg = "Source: " + ex.Source;
                msg += "; Message: " + ex.Message;
                msg += "; Inner Exception: " + ex.InnerException;
                msg += "; Data: " + ex.Data;
                returnVal = msg;
            }
            return returnVal;
        }

        //public static void SerializeException(Exception ex, Stream stream)
        //{
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    formatter.Serialize(stream, ex);
        //}

        //public static Exception DeserializeException(Stream stream)
        //{
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    return (Exception)formatter.Deserialize(stream);
        //}

        public static string GetDataSetXml(DataSet dataset)
        {
            StringWriter writer = new StringWriter();
            dataset.WriteXml(writer, XmlWriteMode.WriteSchema);
            string output = writer.ToString();
            return output;
        }

        public static DataSet SetDataSetXml(string datasetXml)
        {
            DataSet ds = new DataSet();
            StringReader reader = new StringReader(datasetXml);
            ds.ReadXml(reader);
            return ds;
        }

        public static DataRow UpdateStandardRowValues(DataRow datarow, string user, Enumerations.RecordStatusEnum status)
        {
            datarow[Constants.dbfModDate] = DateTime.Now;
            datarow[Constants.dbfModUser] = user;
            datarow[Constants.dbfRecordStatus] = status;

            return datarow;
        }

        public static void WriteDataSet(DataSet ds, string fileNameWithoutExtension)
        {
            string fullName = @"c:\temp\" + fileNameWithoutExtension + ".{0}";
            string fileName = String.Format(fullName, "xml");
            ds.WriteXml(fileName);
            ds.WriteXmlSchema(String.Format(fullName, "xsd"));
        }

        public static string WriteStringToFile(string path, string text, string filePrefix)
        {
            Directory.CreateDirectory(path);
            string fileName = path + filePrefix + "-" + Guid.NewGuid().ToString() + ".xml";
            StreamWriter outfile = new StreamWriter(fileName, false, System.Text.Encoding.UTF8);
            outfile.WriteLine(text);
            outfile.Close();
            return fileName;
        }

        public static void WriteStringToFile(string path, string fileName, string text, bool appendToFile)
        {
            Directory.CreateDirectory(path);
            StreamWriter outfile = new StreamWriter(path + fileName, appendToFile, System.Text.Encoding.UTF8);
            outfile.WriteLine(text);
            outfile.Close();
        }

        private static bool IsNullOrZeroLength(object value)
        {
            bool returnVal = false;

            if (value == null)
            {
                returnVal = true;
            }
            else
            {
                if (value.ToString().Length == 0)
                {
                    returnVal = true;
                }
            }
            return returnVal;
        }

        public static int? ToNullableInt32(string toParse)
        {
            int toReturn;

            if (Int32.TryParse(toParse, out toReturn))
            {
                return toReturn;
            }
            else
            {
                return null;
            }
        }

        public static DateTime? ToNullableDateTime(string toParse)
        {
            DateTime toReturn;

            if (DateTime.TryParse(toParse, out toReturn))
            {
                if (SqlMinDateTime() <= toReturn && toReturn <= SqlMaxDateTime())
                {
                    return toReturn;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static object DbNullIfNull(object obj)
        {
            return obj == null ? DBNull.Value : obj;
        }

        public static DateTime SqlMinDateTime()
        {
            return new DateTime(1753, 1, 1);
        }

        public static DateTime SqlMaxDateTime()
        {
            return new DateTime(9999, 12, 31, 23, 59, 59, 997);
        }

        /// <summary>
        /// Converts a data table to a formatted string.
        /// </summary>
        /// <param name="dt">The data table as <see cref="System.Data.DataTable"/>.</param>
        /// <param name="startMessage">The message to include at the heeader of the table.</param>
        /// <param name="endMessage">The message to include at the footer of the table.</param>
        /// <returns>The formatted string.</returns>
        public static string ConvertDataTableToFormattedString(DataTable dt, string startMessage, string endMessage)
        {
            var sb = new StringBuilder();
            if (dt == null) return sb.ToString();
            if (startMessage != null)
                sb.Append(startMessage + Environment.NewLine);
            var r2 = string.Empty;
            for (var j = 0; j < dt.Columns.Count; j++)
            {
                r2 += dt.Columns[j].ColumnName + ", ";
            }
            if (r2 != string.Empty)
                r2 = r2.Substring(0, r2.Length - 2);
            sb.Append(r2 + Environment.NewLine);
            r2 = string.Empty;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    r2 += dt.Rows[i][j] + ", ";
                }
                if (r2 != string.Empty)
                    r2 = r2.Substring(0, r2.Length - 2);
                sb.Append(r2 + Environment.NewLine);
                r2 = string.Empty;
            }
            sb.Append(Environment.NewLine + "Rows = " + dt.Rows.Count + ", " + "Columns = " + dt.Columns.Count + Environment.NewLine);
            if (endMessage != null)
                sb.Append(endMessage + Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>
        /// Converts a data table to Xml.
        /// </summary>
        /// <param name="dt">The data table as <see cref="System.Data.DataTable"/>.</param>
        /// <param name="startMessage">The message to include at the header of the table.</param>
        /// <param name="endMessage">The message to include at the footer of the table.</param>
        /// <returns>The Xml as <see cref="System.String"/>.</returns>
        public static string ConvertDataTableToXml(DataTable dt, string startMessage, string endMessage)
        {
            var sb = new StringBuilder();
            if (dt == null) return sb.ToString();
            if (startMessage != null)
                sb.Append(startMessage + Environment.NewLine);
            using (var sw = new StringWriter())
            {
                dt.WriteXml(sw);
                var xml = sw.ToString();
                sb.Append(xml + Environment.NewLine);
            }
            sb.Append(Environment.NewLine + "Rows = " + dt.Rows.Count + ", " + "Columns = " + dt.Columns.Count + Environment.NewLine);
            if (endMessage != null)
                sb.Append(endMessage + Environment.NewLine);
            return sb.ToString();
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
            catch { }  // this error must be left uncaught as there appear to be columns that do not exist?
            return rtn;
        }

        public static StringBuilder AddEmail(DataTable dt, string colName)
        {
            StringBuilder addressList = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                if (addressList.Length > 0)
                    addressList.Append(",");

                addressList.Append(Methods.GetStringValue(dr[colName], false));
            }
            return addressList;
        }

        public static bool RequirePaperNotice(string recipients, string[] psEmailTypes)
        {
            bool isPSEmailType = false;
            if (string.IsNullOrEmpty(recipients))
            {
                isPSEmailType = true;
            }
            //else              // OHPNM1-1789 - Comment out this as there is no such requirement in OH to send paper email for an internal address
            //{
            //   string[] recipientsArray = recipients.Split(',');

            //   bool hasInternalAddress = false;

            //    // If email address is maximus.com or nebraska.com send paper mail 

            //       foreach (string recipient in recipientsArray)
            //       {
            //           hasInternalAddress = false;

            //           foreach (string psEmailType in psEmailTypes)
            //           {
            //               if (recipient.Contains(psEmailType))
            //               {
            //                   hasInternalAddress = true;
            //                   break;
            //               }
            //           }
            //           if (!hasInternalAddress) break; //if any email contains external email then email is sent                    
            //       }

            //   if (hasInternalAddress)
            //       isPSEmailType = true;                 

            //}
            return isPSEmailType;
        }

        public static void CopyPropertiesFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
                foreach (var toProperty in toProperties)
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent, null), null);
                        break;
                    }
        }


        public static void MatchPropertiesFrom(this object self, object parent)
        {
            var childProperties = self.GetType().GetProperties();
            foreach (var childProperty in childProperties)
            {
                var attributesForProperty = childProperty.GetCustomAttributes(typeof(MatchParentAttribute), true);
                var isOfTypeMatchParentAttribute = false;

                MatchParentAttribute currentAttribute = null;

                foreach (var attribute in attributesForProperty)
                {
                    if (attribute.GetType() == typeof(MatchParentAttribute))
                    {
                        isOfTypeMatchParentAttribute = true;
                        currentAttribute = (MatchParentAttribute)attribute;
                        break;
                    }
                }

                if (isOfTypeMatchParentAttribute)
                {
                    var parentProperties = parent.GetType().GetProperties();
                    object parentPropertyValue = null;
                    foreach (var parentProperty in parentProperties)
                    {
                        if (parentProperty.Name == currentAttribute.ParentPropertyName)
                        {
                            if (parentProperty.PropertyType == childProperty.PropertyType)
                            {
                                parentPropertyValue = parentProperty.GetValue(parent, null);
                            }
                        }
                    }

                    childProperty.SetValue(self, parentPropertyValue, null);
                }
            }
        }

        public static Guid GetUserId(string username)
        {
            MembershipUser usr = Membership.GetUser(username);
            if (usr == null)
            {
                return Guid.Empty;
            }
            else
            {
                return new Guid(usr.ProviderUserKey.ToString());
            }
        }

        public static Guid GetCurrentUserId()
        {
            var user = HttpContext.Current?.User;
            if (user != null)
            {
                return GetUserId(user.Identity.Name);
            }
            return Guid.Empty;
        }

        public static string StripNonNumerics(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return System.Text.RegularExpressions.Regex.Replace(input, "\\D", string.Empty);
        }

        public static string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static bool Exists(DataRow dr, string col)
        {
            return dr.Table.Columns.Contains(col);
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
            if (dr.Table.Columns.Contains(elementName))
            {
                if (!dr.IsNull(elementName))
                {
                    returnValue = HttpUtility.HtmlDecode(dr[elementName].ToString());
                    returnValue = returnValue.Replace("''", "'");
                }
            }
            return returnValue;
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

        public static bool IsSsnValid(string ssn)
        {
            var area = Convert.ToInt32(ssn.Substring(0, 3));
            if (area >= 900)
            {
                return false;
            }

            if (ssn.Substring(3, 2) == "00")
            {
                return false;
            }

            if (ssn.Substring(5, 4) == "0000")
            {
                return false;
            }

            if (ssn.StartsWith("666"))
            {
                return false;
            }

            if (ssn.StartsWith("000"))
            {
                return false;
            }

            if (ssn == "219099999" || ssn == "078051120")
            {
                return false;
            }

            return true;
        }
        public static async Task<SMSSubscriptionResult> SubscriptionAsync(Subscription subscription)
        {
            SMSSubscriptionResult smsResult = null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Apikey", string.Format("{0}", MAXIMUS.Core.Libraries.Constants.ApiKey));
                string info = JsonConvert.SerializeObject(new SMSSubscription() { list_name = "Welcome", subscriptions = new Subscription[] { subscription } });
                Uri postUrl = new Uri(string.Format("http://api.maximus.messagingchannel.com/rest/v1/OHPROVIDER/subscription"));
                var httpContent = new StringContent(info, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(postUrl.ToString(), httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    smsResult = JsonConvert.DeserializeObject<SMSSubscriptionResult>(await response.Content.ReadAsStringAsync());
                }
            }
            return smsResult;
        }

        public static async Task<SmsResult> SMSAsync(SmsInfo sms)
        {
            SmsResult result = new SmsResult();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Apikey", string.Format("{0}", MAXIMUS.Core.Libraries.Constants.ApiKey));
                Uri postUrl = new Uri(string.Format("http://api.maximus.messagingchannel.com/rest/v1/OHPROVIDER/sms"));
                var response = client.PutAsJsonAsync(postUrl.ToString(), sms).Result;
                if (response.IsSuccessStatusCode)
                {
                    SmsServiceResult serviceResult = response.Content.ReadAsAsync<SmsServiceResult>().Result;
                    if (serviceResult.status_code == "MPCE4001" || serviceResult.status_code == "MPCE0000")
                    {
                        result.success = true;
                    }
                    else
                    {
                        result.success = false;
                    }
                    result.message = serviceResult.ToString();
                }
                else
                {
                    result.message = response.Content.ReadAsStringAsync().Result;
                    result.success = false;
                }
                return result;
            }
        }

        public static string FullExceptionMessage(Exception ex)
        {
            var msg = new StringBuilder();
            msg.AppendLine(ex.Message);
            var innerException = ex.InnerException;
            while (innerException != null)
            {
                msg.AppendLine(innerException.Message);
                innerException = innerException.InnerException;
            }
            return msg.ToString();
        }

        public static bool IsUserInOperatorRolls(string username)
        {
            string[] roles = Constants.OperatorRoles.Split(',');
            foreach (string role in roles)
            {
                if (Roles.IsUserInRole(username, role)) return true;
            }
            return false;
        }

        public static bool IsUserInTechAdmin(string username)
        {
            string[] roles = Constants.UserRole.TechAdmin.Split(',');
            foreach (string role in roles)
            {
                if (Roles.IsUserInRole(username, role)) return true;
            }
            return false;
        }
        public static bool IsUserInPNMSuperUser(string username)
        {
            string[] roles = Constants.UserRole.PNMSuperUser.Split(',');
            foreach (string role in roles)
            {
                if (Roles.IsUserInRole(username, role)) return true;
            }
            return false;
        }

        public static bool IsUserInEnrollmentSpecialistRole(string username)
        {
            string[] roles = Constants.EnrollmentSpecialistRole.Split(',');
            foreach (string role in roles)
            {
                if (Roles.IsUserInRole(username, role)) return true;
            }
            return false;
        }

        public static bool checkIfEVVProviderType(int transacationQueueID)
        {
            bool checkIfEVV = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("TRANSACTION_QUEUE_ID", DbType.Int32, transacationQueueID, false));
                checkIfEVV = Convert.ToBoolean(InfoAccess.ExecuteScalar("usp_checkIfEVVProviderTypeByTxnID", parameters));
            }
            catch (Exception ex)
            { }
            return checkIfEVV;
        }


        public static int validateTxnAcknowledgment(int transacationQueueID, int transactionType, string ACKacknowledgment = "", string SPBMacknowledgment = "", string MITSacknowledgment = "", string EVVacknowledgment = "")
        {
            int ackVal = CON.TransactionResultIds.ACK_RECEIVED_FAILED;
            List<int> waiverIds = new List<int> { CON.TransactionTypeIds.PCW_Partial, CON.TransactionTypeIds.PSM_Partial, CON.TransactionTypeIds.PSM_FULL, CON.TransactionTypeIds.PCW_Full };
            string vendorACKS = AppSettings.Get("VENDORACKSTOPROCESS", "ALL");

            if (!checkIfEVVProviderType(transacationQueueID))
            {
                EVVacknowledgment = CON.ResponseCodes.MITS_Acknowledgment_SUCCESS;
            }
            //skip vendor acks eg. vendorACKS "FI,MITS" will skip checks for FI and MITS
            if (!waiverIds.Contains(transactionType))
            {
                if (Array.IndexOf(vendorACKS.Split(','), CON.ProviderManagementSubscriberSystems.MITS) >= 0)
                {
                    ACKacknowledgment = CON.ResponseCodes.MITS_Acknowledgment_SUCCESS;
                }
                if (Array.IndexOf(vendorACKS.Split(','), CON.ProviderManagementSubscriberSystems.FI) >= 0)
                {
                    MITSacknowledgment = CON.ResponseCodes.MITS_Acknowledgment_SUCCESS;
                }
                if (Array.IndexOf(vendorACKS.Split(','), CON.ProviderManagementSubscriberSystems.SPBM) >= 0)
                {
                    SPBMacknowledgment = CON.ResponseCodes.MITS_Acknowledgment_SUCCESS;
                }
                if (Array.IndexOf(vendorACKS.Split(','), CON.ProviderManagementSubscriberSystems.EVV) >= 0)
                {
                    EVVacknowledgment = CON.ResponseCodes.MITS_Acknowledgment_SUCCESS;
                }
            }

            //WAIVER APPS
            if (waiverIds.Contains(transactionType))
            {
                if (ACKacknowledgment.Equals(CON.ResponseCodes.MITS_Acknowledgment_SUCCESS) && !string.IsNullOrEmpty(ACKacknowledgment))
                {
                    ackVal = CON.TransactionResultIds.ACK_RECEIVED_PASSED;
                }
                else if (string.IsNullOrEmpty(ACKacknowledgment))
                {
                    ackVal = CON.TransactionResultIds.ACK_RECEIVED_WAIT;
                }
            }
            else
            {
                if (ACKacknowledgment.Equals(CON.ResponseCodes.MITS_Acknowledgment_SUCCESS) && !string.IsNullOrEmpty(ACKacknowledgment)
                    && SPBMacknowledgment.Equals(CON.ResponseCodes.MITS_Acknowledgment_SUCCESS) && !string.IsNullOrEmpty(SPBMacknowledgment)
                    && EVVacknowledgment.Equals(CON.ResponseCodes.MITS_Acknowledgment_SUCCESS) && !string.IsNullOrEmpty(EVVacknowledgment)
                    && MITSacknowledgment.Equals(CON.ResponseCodes.MITS_Acknowledgment_SUCCESS) && !string.IsNullOrEmpty(MITSacknowledgment))
                {
                    ackVal = CON.TransactionResultIds.ACK_RECEIVED_PASSED;
                }
                else if (string.IsNullOrEmpty(ACKacknowledgment) || string.IsNullOrEmpty(SPBMacknowledgment) || string.IsNullOrEmpty(EVVacknowledgment) || string.IsNullOrEmpty(MITSacknowledgment))
                {
                    ackVal = CON.TransactionResultIds.ACK_RECEIVED_WAIT;
                }
            }

            return ackVal;
        }

    }
}
