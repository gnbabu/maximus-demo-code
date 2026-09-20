using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Xml;
using mt = MAXIMUS.DataExchange.PDMS.TN_MMIS_Service_Test;

namespace MAXIMUS.DataExchange.PDMS
{
    public class MMISShared
    {

        #region "Class Level Declarations"

        public string path { get; set; }
        public string exportFolder { get; set; }
        public string uid { get; set; }
        public string pwd { get; set; }
        public int testRowsCount { get; set; }
        public Guid ThreadId { get; set; }
        internal const string dbtGrps = "Groups";
        internal const string dbtGrpAddrs = "Addresses";
        internal const string dbtAffs = "Affiliates";
        internal const string dbtSpecialties = "Specialties";
        internal const string dbrGrps2Addrs = dbtGrps + "2" + dbtGrpAddrs;
        internal const string dbrGrps2dbtAffs = dbtGrps + "2" + dbtAffs;
        internal const string groupStagingPk = "MMIS_STAGING_GROUP_PK";

        // Sole Proprietor
        internal const string dbtProvs = "PROVIDER_STAGING";
        internal const string dbtAddr = "ADDRESS_STAGING";
        internal const string dbtLicCert = "LICENSE_CERT_STAGING";
        internal const string dbtProvCat = "PROV_CATEGORY_STAGING";
        internal const string dbtProvSpec = "PROV_SPECIALTY_STAGING";
        internal const string dbtProvTax = "PROV_TAXONOMY_STAGING";
        internal const string dbrProv2Addrs = dbtProvs + "2" + dbtAddr;
        internal const string dbrProv2LicCert = dbtProvs + "2" + dbtLicCert;
        internal const string dbrProv2Cat = dbtProvs + "2" + dbtProvCat;
        internal const string dbrProv2Spec = dbtProvs + "2" + dbtProvSpec;
        internal const string dbrProv2Tax = dbtProvs + "2" + dbtProvTax;
        internal const string dbfPK = "PROVIDER_STAGING_PK";


        #endregion

        #region "Constructors"

        public MMISShared(Guid threadId)
        {
            this.ThreadId = threadId;
        }

        #endregion

        #region "Static Methods"

        public static DataRow GetAdditionalProviderInfo(int partyID)
        {
            DataRow drInfo = null;

            return drInfo;
        }

        /// <summary>
        /// Gets the servicing location contact name from the database
        /// </summary>
        /// <param name="partyID">provider's party id</param>
        /// <returns>servicing location contact name</returns>
        public static string GetServicingContactName(int partyID)
        {

            // TODO: Needs to add a new implementation for this method.
            // dbo.PROVIDER.PartyID object does not exists in database.

            throw new NotImplementedException();

            //string retName = "";
            //    // pull the info from the database
            //    DataSet ds = DataAccess.ExecuteSelectSql("SELECT [SERVICING_CONTACT_NAME],[SERVICING_CONTACT_TYPE],[SERVICING_ORGANIZATION_NAME] FROM dbo.REG_SERVICE_LOCATION WHERE REG_ID = (SELECT MAX(REG_ID) FROM dbo.REG_PROVIDER WHERE PARTY_ID = " + partyID.ToString() + ")");
            //    if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        string individualName = DataExchangeHelper.GetString("SERVICING_CONTACT_NAME", ds.Tables[0].Rows[0]);
            //        string contactNameType = DataExchangeHelper.GetString("SERVICING_CONTACT_TYPE", ds.Tables[0].Rows[0]);
            //        string orgName = DataExchangeHelper.GetString("SERVICING_ORGANIZATION_NAME", ds.Tables[0].Rows[0]);
            //        // return thr individual's name if it's not empty, otherwise return the group name
            //        retName = (individualName == null || individualName.Length == 0 ? orgName : individualName);
            //        }
            //    return retName;
        }

        public static string GetMailToContactName(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // dbo.PROVIDER.PartyID object does not exists in database.

            throw new NotImplementedException();

            //string retName = "";
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT [MAILTO_CONTACT_NAME],[MAILTO_CONTACT_TYPE],[MAILTO_ORGANIZATION_NAME] FROM dbo.REG_SERVICE_LOCATION WHERE REG_ID = (SELECT MAX(REG_ID) FROM dbo.REG_PROVIDER WHERE PARTY_ID = " + partyID.ToString() + ")");
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    string individualName = DataExchangeHelper.GetString("MAILTO_CONTACT_NAME", ds.Tables[0].Rows[0]);
            //    string contactNameType = DataExchangeHelper.GetString("MAILTO_CONTACT_TYPE", ds.Tables[0].Rows[0]);
            //    string orgName = DataExchangeHelper.GetString("MAILTO_ORGANIZATION_NAME", ds.Tables[0].Rows[0]);
            //    retName = (individualName == null || individualName.Length == 0 ? orgName : individualName);
            //}
            //return retName;
        }

        public static string GetBillingContactName(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // dbo.PROVIDER.PartyID object does not exists in database.

            throw new NotImplementedException();

            //string retName = "";
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT [PAYTO_CONTACT_NAME],[PAYTO_CONTACT_TYPE],[PAYTO_ORGANIZATION_NAME] FROM dbo.REG_SERVICE_LOCATION WHERE REG_ID = (SELECT MAX(REG_ID) FROM dbo.REG_PROVIDER WHERE PARTY_ID = " + partyID.ToString() + ")");
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    string individualName = DataExchangeHelper.GetString("PAYTO_CONTACT_NAME", ds.Tables[0].Rows[0]).Trim();
            //    string contactNameType = DataExchangeHelper.GetString("PAYTO_CONTACT_TYPE", ds.Tables[0].Rows[0]);
            //    string orgName = DataExchangeHelper.GetString("PAYTO_ORGANIZATION_NAME", ds.Tables[0].Rows[0]).Trim();
            //    retName = (individualName == null || individualName.Length == 0 ? orgName : individualName);
            //}
            //return retName;
        }

        public static string GetCountyCode(string countyName, string state)
        {
            string retCode = "15";
            List<SqlParameter> prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@COUNTY_NAME", countyName));
            prms.Add(new SqlParameter("@STATE_ABBREVIATION", state));
            DataSet ds = DataAccess.ExecuteStoredProcedure("usp_Select_MMIS_COUNTY_BYCOUNTY_NAME", prms, "MMIS_COUNTY_CODE");
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                retCode = DataExchangeHelper.GetString("MMIS_COUNTY_CODE", ds.Tables[0].Rows[0]);
            }
            return retCode;

        }

        public static int ConvertAddressTypeCodeToID(string code)
        {
            int retAdrTypeID = 0;
            switch (code)
            {
                case "M":
                    retAdrTypeID = (int)Enumerations.ContactMechanismRoleTypeId.MailTo;
                    break;
                case "P":
                    retAdrTypeID = (int)Enumerations.ContactMechanismRoleTypeId.PayTo;
                    break;
                case "S":
                    retAdrTypeID = (int)Enumerations.ContactMechanismRoleTypeId.Servicing;
                    break;
            }

            return retAdrTypeID;
        }

        public static string ConvertAddressTypeIDToCode(int id)
        {
            string retAdrType = "";
            switch (id)
            {
                case (int)Enumerations.ContactMechanismRoleTypeId.MailTo:
                    retAdrType = "M";
                    break;
                case (int)Enumerations.ContactMechanismRoleTypeId.PayTo:
                    retAdrType = "P";
                    break;
                case (int)Enumerations.ContactMechanismRoleTypeId.Servicing:
                    retAdrType = "S";
                    break;
            }

            return retAdrType;
        }

        public static DataRow GetRegOfficeInfo(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // PartyID is not a column in database

            throw new NotImplementedException();

            //DataRow retRow = null;
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT OFFICE_NEWPATIENT, OFFICE_REFFERAL, OFFICE_TELEPHONE, OFFICE_TRANSPORT, OFFICE_DISABLE, OFFICE_EBILLING, OFFICE_TDD, OFFICE_WEBSITE FROM dbo.REG_SERVICE_LOCATION WHERE REG_ID = (SELECT MAX(REG_ID) FROM dbo.REG_PROVIDER WHERE PARTY_ID = " + partyID.ToString() + ")");
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    retRow = ds.Tables[0].Rows[0];
            //}
            //return retRow;
        }

        public static bool IsProviderIndividual(int partyID)
        {
            List<SqlParameter> prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@pin_party_id", partyID));
            string retIsIndividual = DataAccess.ExecuteScalar("usp_IsProviderIndividual", prms);
            return (retIsIndividual == "1" || retIsIndividual.ToUpper().Trim() == "TRUE");
        }

        public static string GetTitle(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // Neither there is Party Table nor PartyID column in database.

            throw new NotImplementedException();

            //string retCode = "";
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT Title FROM PROVIDER WHERE PartyID = " + partyID.ToString());
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    retCode = DataExchangeHelper.GetString("Title", ds.Tables[0].Rows[0]);
            //}
            //return retCode;
        }

        public static DataRowCollection GetEnrollmentInfo(int partyID)
        {
            DataRowCollection retRows = null;
            List<SqlParameter> prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@pin_party_id", partyID));
            DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectEnrollmentInfo", prms, "EnrollmentInfo");
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                retRows = ds.Tables[0].Rows;
            }

            return retRows;
        }

        public static DataRow GetTaxIDInfo(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // PartyID is not a column in database

            throw new NotImplementedException();

            //DataRow retRow = null;
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT prov.TaxID AS ALT_TAX_ID, prov.TaxIdTypeId AS ALT_TAX_ID_TYPE_ID, sl.TaxID AS TAX_ID, sl.TaxIdTypeId AS TAX_ID_TYPE_ID  FROM PROVIDER prov INNER JOIN SERVICE_LOCATION sl ON sl.PartyID = prov.PartyID WHERE prov.PartyID = " + partyID.ToString());
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    retRow = ds.Tables[0].Rows[0];
            //}

            //return retRow;
        }

        public static DataTable GetSubmitProviderResultDetails(DateTime dt)
        {
            DataTable retData = null;
            DataSet ds = DataAccess.ExecuteSelectSql("EXEC usp_Reports_MMISInterfaceResponseDetail '" + dt.ToShortDateString() + "'");
            if (DataExchangeHelper.HasRows(ds))
            {
                retData = ds.Tables[0];
            }

            return retData;
        }

        public static DataTable GetSubmitProviderResultAggregates(DateTime dt)
        {
            DataTable retData = null;
            DataSet ds = DataAccess.ExecuteSelectSql("EXEC usp_Reports_MMISInterfaceResponseAggregates '" + dt.ToShortDateString() + "'");
            if (DataExchangeHelper.HasRows(ds))
            {
                retData = ds.Tables[0];
            }

            return retData;
        }


        public static DataRow GetCLIAInfo(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // PartyID is not a column in database

            throw new NotImplementedException();

            //DataRow retRow = null;
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT [LicensureID],[PartyID],[CLIANumber],[CLIAState],[CLIAEffDate],[CLIAEndDate],[CLIABillForSvcs],[CLIARadiologySvcs],[CLIALabSvcs] FROM [dbo].[CLIA] WHERE PartyID = " + partyID.ToString() + " ORDER BY CLIAEffDate DESC");
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    retRow = ds.Tables[0].Rows[0];
            //}

            //return retRow;
        }
        public static string GetQuadWard(int partyID, string addressType)
        {
            string retQuadWard = "    ";
            string qwSelect = "";
            switch (addressType)
            {
                case "M":
                    qwSelect = "MAILTO_QUADRANT AS Quad, MAILTO_WARD AS Ward";
                    break;

                case "S":
                    qwSelect = "SERVICING_QUADRANT AS Quad, SERVICING_WARD AS Ward";
                    break;

                case "P":
                    qwSelect = "PAYTO_QUADRANT AS Quad, PAYTO_WARD AS Ward";
                    break;

                case "R":
                    qwSelect = "REMITTANCE_QUADRANT AS Quad, REMITTANCE_WARD AS Ward";
                    break;

                case "O":
                    qwSelect = "OTHER_QUADRANT AS Quad, OTHER_WARD AS Ward";
                    break;

                case "W":
                    qwSelect = "W9_QUADRANT AS Quad, W9_WARD AS Ward";
                    break;
            }

            // TODO: Needs to add a new implementation for this method.
            // PartyID is not a column in database

            throw new NotImplementedException();

            //if (qwSelect.Length > 0)
            //{
            //    DataSet ds = DataAccess.ExecuteSelectSql("SELECT " + qwSelect + "  FROM dbo.REG_SERVICE_LOCATION WHERE REG_ID = (SELECT MAX(REG_ID) FROM dbo.REG_PROVIDER WHERE PARTY_ID = " + partyID.ToString() + ")");
            //    if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        DataRow dr = ds.Tables[0].Rows[0];
            //        string quad = DataExchangeHelper.GetString("Quad", dr).ToUpper().Trim();
            //        string ward = DataExchangeHelper.GetString("Ward", dr).ToUpper().Trim();
            //        if (ward.Length > 0)
            //        {
            //            ward = Convert.ToInt32(ward).ToString("00");
            //        }
            //        retQuadWard = (quad.Length > 0 ? quad : "  ") + (ward.Length > 0 ? ward : "  ");
            //    }
            //}

            //return retQuadWard;
        }

        public static DataRow GetRegAddress(int partyID, string addressType)
        {
            DataRow retRow = null;
            string adrSelect = "";
            switch (addressType)
            {
                case "R":
                    adrSelect = "REMITTANCE_COUNTY AS COUNTY, REMITTANCE_ORGANIZATION_NAME AS NAME, REMITTANCE_CONTACT_NAME AS CONTACT_NAME, '' AS LAST_NAME, '' AS FIRST_NAME, '' AS MI_NAME,";
                    adrSelect += "REMITTANCE_ADDRESS1 AS STREET1, REMITTANCE_ADDRESS2 AS STREET2, REMITTANCE_CITY AS CITY, REMITTANCE_STATE AS STATE, REMITTANCE_ZIP AS ZIP5, REMITTANCE_EXT_ZIP AS ZIP4,";
                    adrSelect += "REMITTANCE_PHONE_NUMBER AS PHONE, REMITTANCE_FAX_NUMBER AS FAX, REMITTANCE_EMAIL AS EMAIL";
                    break;

                case "O":
                    adrSelect = "OTHER_COUNTY AS COUNTY, OTHER_ORGANIZATION_NAME AS NAME, OTHER_CONTACT_NAME AS CONTACT_NAME, '' AS LAST_NAME, '' AS FIRST_NAME, '' AS MI_NAME,";
                    adrSelect += "OTHER_ADDRESS1 AS STREET1, OTHER_ADDRESS2 AS STREET2, OTHER_CITY AS CITY, OTHER_STATE AS STATE, OTHER_ZIP AS ZIP5, OTHER_EXT_ZIP AS ZIP4,";
                    adrSelect += "OTHER_PHONE_NUMBER AS PHONE, OTHER_FAX_NUMBER AS FAX, OTHER_EMAIL AS EMAIL";
                    break;

                case "W":
                    adrSelect = "W9_COUNTY AS COUNTY, '' AS NAME, W9_CONTACT_NAME AS CONTACT_NAME, '' AS LAST_NAME, '' AS FIRST_NAME, '' AS MI_NAME,";
                    adrSelect += "W9_ADDRESS1 AS STREET1, W9_ADDRESS2 AS STREET2, W9_CITY AS CITY, W9_STATE AS STATE, W9_ZIP AS ZIP5, W9_EXT_ZIP AS ZIP4,";
                    adrSelect += "W9_PHONE_NUMBER AS PHONE, W9_FAX_NUMBER AS FAX, W9_EMAIL_ADDRESS AS EMAIL";
                    break;
            }

            // TODO: Needs to add a new implementation for this method.
            // PartyID is not a column in database

            throw new NotImplementedException();

            //if (adrSelect.Length > 0)
            //{
            //    DataSet ds = DataAccess.ExecuteSelectSql("SELECT " + adrSelect + "  FROM dbo.REG_SERVICE_LOCATION WHERE REG_ID = (SELECT MAX(REG_ID) FROM dbo.REG_PROVIDER WHERE PARTY_ID = " + partyID.ToString() + ")");
            //    if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        retRow = ds.Tables[0].Rows[0];
            //    }
            //}
            //return retRow;
        }

        public static DataRow GetAdditionalAddressInfo(int partyID, Enumerations.ContactMechanismRoleTypeId addressType)
        {
            DataRow retRow = null;

            List<SqlParameter> prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@pin_party_id", (object)partyID));
            prms.Add(new SqlParameter("@pin_address_type", (object)((int)addressType)));
            DataSet ds = DataAccess.ExecuteSelectSql("EXEC usp_SelectAdditionalAddressInfo " + partyID.ToString() + "," + ((int)addressType).ToString());
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                retRow = ds.Tables[0].Rows[0];
            }

            return retRow;
        }

        public static int GetProviderCategoryTypeID(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // PartyID is not a column in database

            throw new NotImplementedException();

            //int retCat = 0;
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT [ProviderCategoryTypeID] FROM [dbo].[PROVIDER] WHERE PartyID = " + partyID.ToString());
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    retCat = DataExchangeHelper.GetInt("ProviderCategoryTypeID", ds.Tables[0].Rows[0]);
            //}

            //return retCat;
        }
        public static DataRow GetPracticeAndOwnershipCodes(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // PartyID is not a column in database

            throw new NotImplementedException();

            //DataRow retRow = null;
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT pt.PRACTICE_TYPE_MMIS_ID AS PRACTICE_TYPE_CODE, tet.MMIS AS TAX_ENTITY_TYPE_CODE FROM REG_SERVICE_LOCATION sl INNER JOIN PRACTICE_TYPE_W9 pt ON pt.PRACTICE_TYPE_ID = sl.PRACTICE_TYPE_ID INNER JOIN TAX_ENTITY_TYPE tet ON tet.TAX_ENTITY_TYPE_ID = sl.TAX_ENTITY_TYPE_ID WHERE REG_ID = (SELECT MAX(REG_ID) FROM REG_PROVIDER WHERE PARTY_ID = " + partyID.ToString() + ");");
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    retRow = ds.Tables[0].Rows[0];
            //}

            //return retRow;
        }

        public static DataRowCollection GetEnrollmentHistory(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // PartyID is not a column in database

            throw new NotImplementedException();

            //DataRowCollection retRows = null;
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT [ENROLL_START_DATE_TIME],[ENROLL_END_DATE_TIME],[ENROLLMENT_STATUS_CODE] FROM [dbo].[REG_ENROLLMENT] WHERE REG_ID = (SELECT MAX(REG_ID) FROM REG_PROVIDER WHERE PARTY_ID = " + partyID.ToString() + ") ORDER BY [ENROLL_START_DATE_TIME] DESC");
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            //{
            //    retRows = ds.Tables[0].Rows;
            //}
            //return retRows;
        }

        public static DataRow GetMedicareInfo(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // PartyID is not a column in table Medicare, nor is there such table Medicare in database

            throw new NotImplementedException();

            //DataRow retRow = null;
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT [MEDICARENumber], [MEDICAREState],[MEDICAREEffDate],[MEDICAREEndDate] FROM [dbo].[MEDICARE] WHERE PartyID = " + partyID.ToString() + " AND [MEDICAREEffDate] = (SELECT MAX([MEDICAREEffDate]) FROM dbo.MEDICARE WHERE PartyID = " + partyID.ToString() + ");");
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    retRow = ds.Tables[0].Rows[0];
            //}

            //return retRow;
        }

        public static DateTime GetLatestApplicationDate(int partyID)
        {
            // TODO: Needs to add a new implementation for this method.
            // PartyID is not a column in database

            throw new NotImplementedException();

            //DateTime retAppDate = new DateTime(1753, 1, 1);
            //DataSet ds = DataAccess.ExecuteSelectSql("SELECT MAX(SUBMIT_DATE_TIME) AS AppDate FROM [dbo].[REGISTRATION] WHERE REG_ID = (SELECT MAX(REG_ID) FROM [dbo].[REG_PROVIDER] WHERE PARTY_ID = " + partyID.ToString() + ");");
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    retAppDate = DataExchangeHelper.GetDateTime("AppDate", ds.Tables[0].Rows[0]);
            //}

            //return retAppDate;
        }

        #endregion

        #region "Internal Methods"

        internal int GetAffiliatePartyId(string affiliateStagingPk)
        {
            // create log object
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logProcessName);
            int returnVal = 0;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Oct 23 2013  5:21PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_AFFILIATE_PK", DbType.Int32, affiliateStagingPk, true));

                string partyId = DataAccess.ExecuteScalar("usp_SelectAffiliatePartyId", parameters);
                int retNum;
                bool isNumber = int.TryParse(partyId, out retNum);
                if (isNumber) returnVal = retNum;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
            return returnVal;
        }

        internal string GetAffiliateAction(string affiliateStagingPk)
        {
            // create log object
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logProcessName);
            string returnVal = string.Empty;

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Oct 23 2013  5:21PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_AFFILIATE_PK", DbType.Int32, affiliateStagingPk, true));

                returnVal = DataAccess.ExecuteScalar("usp_SelectAffiliateAction", parameters);
            }
            catch (Exception ex)
            {
                CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
            return returnVal;
        }

        internal mt.CAQH_DOTNET_WebServiceSoapClient GetServiceClient()
        {
            string baseAddress = AppSettings.Get("MMIS-WebServiceURI", string.Empty);
            int maxBufferSize = Convert.ToInt32(AppSettings.Get("MMIS-MaxMessageSize", "131072"));
            long maxMessageSize = Convert.ToInt64(maxBufferSize);

            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Name = "CAQH_DOTNET_WebServiceSoap_SSL";
            binding.CloseTimeout = new TimeSpan(0, 1, 0);
            binding.OpenTimeout = new TimeSpan(0, 1, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 1, 0);
            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = maxBufferSize;
            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = maxMessageSize;
            binding.MessageEncoding = WSMessageEncoding.Text;
            binding.TextEncoding = Encoding.UTF8;
            binding.TransferMode = TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;

            XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
            readerQuotas.MaxDepth = 32;
            readerQuotas.MaxStringContentLength = 8192;
            readerQuotas.MaxArrayLength = 16384;
            readerQuotas.MaxBytesPerRead = 4096;
            readerQuotas.MaxNameTableCharCount = 16384;
            binding.ReaderQuotas = readerQuotas;

            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Transport.Realm = string.Empty;

            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

            EndpointAddress epa = new EndpointAddress(baseAddress);

            mt.CAQH_DOTNET_WebServiceSoapClient mmis = new mt.CAQH_DOTNET_WebServiceSoapClient(binding, epa);

            return mmis;
        }

        internal DataSet GetUnsubmittedStagingRecords(int transactionType)
        {
            if (transactionType == Constants.TransactionType.SendSpecialty2ToMMIS || transactionType == Constants.TransactionType.SendSpecialty3ToMMIS)
            {
                return GetSpecialtyStagingRecords(false);
            }
            else
            {
                return GetStagingRecords(transactionType, false);
            }
        }

        internal DataSet GetIndividualStagingRecords(int pdmsStatusType)
        {
            DataSet ds = new DataSet();

            if (pdmsStatusType == Constants.PDMSStatusType.PendingMMISSubmission)
            {
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectPDMSToMMISExportRecords", (dbtProvs + Constants.pluralEnding));

                ds.Tables[0].TableName = dbtProvs;
                ds.Tables[1].TableName = dbtAddr;
                ds.Tables[2].TableName = dbtLicCert;
                ds.Tables[3].TableName = dbtProvCat;
                ds.Tables[4].TableName = dbtProvSpec;
                ds.Tables[5].TableName = dbtProvTax;

                DataRelation relation;
                // Add address relation
                relation = new DataRelation(dbrProv2Addrs, ds.Tables[dbtProvs].Columns[dbfPK]
                    , ds.Tables[dbtAddr].Columns[dbfPK]);
                ds.Relations.Add(relation);
                // add license/cert relation
                relation = new DataRelation(dbrProv2LicCert, ds.Tables[dbtProvs].Columns[dbfPK]
                , ds.Tables[dbtLicCert].Columns[dbfPK]);
                ds.Relations.Add(relation);
                // add category of service relation
                relation = new DataRelation(dbrProv2Cat, ds.Tables[dbtProvs].Columns[dbfPK]
                , ds.Tables[dbtProvCat].Columns[dbfPK]);
                ds.Relations.Add(relation);
                // add specialty relation
                relation = new DataRelation(dbrProv2Spec, ds.Tables[dbtProvs].Columns[dbfPK]
                , ds.Tables[dbtProvSpec].Columns[dbfPK]);
                ds.Relations.Add(relation);
                // add taxonomy relation
                relation = new DataRelation(dbrProv2Tax, ds.Tables[dbtProvs].Columns[dbfPK]
                , ds.Tables[dbtProvTax].Columns[dbfPK]);
                ds.Relations.Add(relation);
            }

            if (pdmsStatusType == Constants.PDMSStatusType.SubmittedtoMMIS)
            {
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectPDMSToMMISExportedRecords", (dbtProvs + Constants.pluralEnding));

                ds.Tables[0].TableName = dbtProvs;
            }
            return ds;
        }

        internal DataSet GetSubmittedStagingRecords(int transactionType)
        {
            if (transactionType == Constants.TransactionType.SendSpecialty2ToMMIS || transactionType == Constants.TransactionType.SendSpecialty3ToMMIS)
            {
                return GetSpecialtyStagingRecords(true);
            }
            else
            {
                return GetStagingRecords(transactionType, true);
            }
        }

        internal List<mt.Addresses> PopulateAddresses(DataRow[] addresses)
        {
            List<mt.Addresses> addressList = new List<mt.Addresses>();

            foreach (DataRow address in addresses)
            {
                mt.Addresses addr = new mt.Addresses();
                addr.pdmsId = Methods.GetStringValue(address["MMIS_STAGING_ADDRESS_PK"], false).ToString();
                addr.action = Methods.GetStringValue(address["ACTION"], false).ToString();
                addr.groupName = Methods.GetStringValue(address["NAME"], false).ToString();
                addr.type = Methods.GetStringValue(address["ADDRESS_TYPE"], false).ToString();
                addr.street1 = Methods.GetStringValue(address["STREET1"], false).ToString();
                addr.street2 = Methods.GetStringValue(address["STREET2"], false).ToString();
                addr.city = Methods.GetStringValue(address["CITY"], false).ToString();
                addr.county = Methods.GetStringValue(address["COUNTY"], false).ToString();
                addr.state = Methods.GetStringValue(address["STATE"], false).ToString();
                addr.zip = Methods.GetStringValue(address["ZIP5"], false).ToString();
                addr.zipFour = Methods.GetStringValue(address["ZIP4"], false).ToString();
                addr.phone = Methods.GetStringValue(address["PHONE"], false).ToString();
                addr.phoneExt = Methods.GetStringValue(address["PHONE_EXT"], false).ToString();
                addr.fax = Methods.GetStringValue(address["FAX"], false).ToString();
                addr.email = Methods.GetStringValue(address["EMAIL"], false).ToString();
                addressList.Add(addr);
            }
            return addressList;
        }

        internal List<mt.Affiliates> PopulateAffiliates(DataRow[] affiliates)
        {
            List<mt.Affiliates> affiliateList = new List<mt.Affiliates>();

            foreach (DataRow affiliate in affiliates)
            {
                mt.Affiliates affil = new mt.Affiliates();
                affil.action = Methods.GetStringValue(affiliate["ACTION"], false).ToString();
                affil.medicareId = Methods.GetStringValue(affiliate["BASE_MEDICAID_ID"], false).ToString();
                affil.memberEffectiveDate = Methods.GetDateValue(affiliate["EFFECTIVE_DATE"]);
                affil.memberTermDate = Methods.GetDateValue(affiliate["TERM_DATE"]);
                affil.npiId = Methods.GetStringValue(affiliate["NPI"], false).ToString();
                affil.pdmsId = Methods.GetStringValue(affiliate["MMIS_STAGING_AFFILIATE_PK"], false).ToString();
                affiliateList.Add(affil);
            }
            return affiliateList;
        }

        internal void ProcessDataErrors(string errorCode, string partyId, string medicaidPk)
        {
            // create log object
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logProcessName);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Aug 14 2012 11:32AM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, errorCode, true));
                string errorTypeId = DataAccess.ExecuteScalar("sp_SelectErrorTypeIdByErrorCode", parameters);

                // generated by sp_Admin_StoredProcBuilder on Sep 12 2013  5:09PM
                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyId, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_TYPE_ID", DbType.Int32, errorTypeId, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, medicaidPk, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                DataAccess.ExecuteStoredProcedure("usp_SavePDMSError", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
        }

        internal bool ProcessServiceErrors(string providerPK, mt.Error[] errors)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            bool returnVal = false;

            if (!object.Equals(errors, null))
            {

                // loop over errors and record information
                foreach (mt.Error err in errors)
                {
                    string code;
                    string desc;
                    code = err.errorCode.Trim();
                    desc = err.errorDescription.Trim();

                    // generated by sp_Admin_StoredProcBuilder on Jul 16 2012 12:30PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("PROVIDER_STAGING_PK", DbType.Int32, providerPK, true));
                    parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, code, true));
                    parameters.Add(SqlParms.CreateParameter("ERROR_DESCRIPTION", DbType.String, desc, true));

                    DataAccess.ExecuteStoredProcedure("insertSTAGING_ERROR", parameters);

                    LogErrorRecord(log, "MMIS Response Exception", "PK", providerPK, code + " " + desc);
                }

            }
            // if errors encountered, return true
            if (errors.Length > 0)
            {
                returnVal = true;
            }
            return returnVal;
        }

        internal void SetInternalProperties(MethodBase currentMethod)
        {
            this.exportFolder = DateTime.Now.ToString("yyyyMMddHH") + "-" + currentMethod.Name;
            this.uid = AppSettings.Get("MMIS-WebService-UserName");
            this.pwd = AppSettings.Get("MMIS-WebService-Pwd");
            path = Path.Combine(new Uri(AppSettings.Get("MMIS-SentProvidersXmlPath")).LocalPath + this.exportFolder) + @"\";
            this.testRowsCount = Convert.ToInt32(AppSettings.Get("MMIS-RetrieveRecordCount", "10"));
        }

        internal void LogFileRecordError(Logging log, string lineNumber, string fileName, string errorMessage)
        {
            string fullMessage = "Record failed. Line No: {0}; File: {1}; Reason: {2}";
            log.CreateLogEntry(String.Format(fullMessage, lineNumber, fileName, errorMessage), Logging.LogPriority.DataLoadIssues);
        }

        internal void CreateCommunicationEvent(int RegId, string emailFrom, string templateName, string emailTo, string subject, string body, Dictionary<string, object> fields, Guid userId, bool isEmailSent, string log_message)
        {
            // Create the communicaton event and email
            DateTime now = DateTime.Now;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
            string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

            // create parameters objects and fill with values
            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, now, true));
            parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
            parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, emailTo, false));
            parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
            parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
            parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, templateName, false));
            parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, ObjectControllerHelper.GetKeyValueString(fields), false));
            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, false));
            parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, userId, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
            parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean, isEmailSent, true));
            parameters.Add(SqlParms.CreateParameter("log_message", DbType.String, log_message, true));
            DataAccess.ExecuteStoredProcedure("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
            string comEventID = DataAccess.ExecuteScalar("sp_SelectLastCommunicationEventID", parameters);

            // generated by sp_Admin_StoredProcBuilder on Oct  3 2012  2:44PM
            // create parameters objects and fill with values
            parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegId, true));
            parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_ID", DbType.Int32, comEventID, true));
            parameters.Add(SqlParms.CreateParameter("RESOLVING_ACTION_TYPE_ID", DbType.Int32, Constants.ResolvingActionType.SystemGeneratedEmail, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

            DataAccess.ExecuteStoredProcedure("sp_UpdateCommunicationEventIdsForPartyErrorHistory", parameters);
        }

        public DataSet GetUnsubmittedMCORecords()
        {
            DataSet ds = null;
            DateTime now = DateTime.Now;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("sent_to_MMIS_but_response_not_processed", DbType.Int32, 0, true));
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectMCOAffiliations", parameters, "affil_ds");
            return ds;
        }

        public DataSet GetSubmittedMCOStagingRecords()
        {
            DataSet ds = null;
            DateTime now = DateTime.Now;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("sent_to_MMIS_but_response_not_processed", DbType.Int32, 1, true));
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectMCOAffiliations", parameters, "affil_ds");
            return ds;
        }

        public DataRow GetMCOAffiliationNotificationInfo(int mcoAffiliationID)
        {
            DataRow retRow = null;
            DateTime now = DateTime.Now;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("mco_affiliation_id", DbType.Int32, mcoAffiliationID, true));
            DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectMCOAffiliationsNotificationInfo", parameters, "affil_ds");
            retRow = (DataExchangeHelper.HasRows(ds) ? ds.Tables[0].Rows[0] : null);

            return retRow;
        }

        public void PopulateMCOStagingData()
        {
            DateTime now = DateTime.Now;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
            DataAccess.ExecuteStoredProcedure("usp_InsertMMISStagingMCOAffiliations", parameters, "affil_ds");
        }
        #endregion

        #region "Private Methods"

        private DataSet GetStagingRecords(int transactionType, bool submitted)
        {
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            DataSet ds;

            try
            {
                if (transactionType == Constants.TransactionType.ResendAffiliationNotifications)
                {
                    ds = DataAccess.ExecuteStoredProcedure("usp_ReprocessAffiliationEmails", "Groups");
                }
                else
                {
                    // generated by sp_Admin_StoredProcBuilder on Sep  9 2013  1:09PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, transactionType, true));
                    parameters.Add(SqlParms.CreateParameter("SUBMITTED", DbType.Boolean, submitted, true));

                    ds = DataAccess.ExecuteStoredProcedure("usp_SelectMMISStagingGroups", parameters, "Groups");
                }

                ds.Tables[0].TableName = dbtGrps;
                ds.Tables[1].TableName = dbtGrpAddrs;
                ds.Tables[2].TableName = dbtAffs;
                ds.Tables[3].TableName = dbtLicCert;
                ds.Tables[4].TableName = dbtProvCat;
                ds.Tables[5].TableName = dbtProvSpec;
                ds.Tables[6].TableName = dbtProvTax;

                DataRelation relation;
                relation = new DataRelation(dbrGrps2Addrs, ds.Tables[dbtGrps].Columns[groupStagingPk]
                    , ds.Tables[dbtGrpAddrs].Columns[groupStagingPk]);
                ds.Relations.Add(relation);
                relation = new DataRelation(dbrGrps2dbtAffs, ds.Tables[dbtGrps].Columns[groupStagingPk]
                    , ds.Tables[dbtAffs].Columns[groupStagingPk]);
                ds.Relations.Add(relation);

                // add license/cert relation
                relation = new DataRelation(dbrProv2LicCert, ds.Tables[dbtGrps].Columns[groupStagingPk]
                , ds.Tables[dbtLicCert].Columns[dbfPK]);
                ds.Relations.Add(relation);
                // add category of service relation
                relation = new DataRelation(dbrProv2Cat, ds.Tables[dbtGrps].Columns[groupStagingPk]
                , ds.Tables[dbtProvCat].Columns[dbfPK]);
                ds.Relations.Add(relation);
                // add specialty relation
                relation = new DataRelation(dbrProv2Spec, ds.Tables[dbtGrps].Columns[groupStagingPk]
                , ds.Tables[dbtProvSpec].Columns[dbfPK]);
                ds.Relations.Add(relation);
                // add taxonomy relation
                relation = new DataRelation(dbrProv2Tax, ds.Tables[dbtGrps].Columns[groupStagingPk]
                , ds.Tables[dbtProvTax].Columns[dbfPK]);
                ds.Relations.Add(relation);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
            return ds;
        }


        private DataSet GetSpecialtyStagingRecords(bool submitted)
        {
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            DataSet ds;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("SUBMITTED", DbType.Boolean, submitted, true));

                ds = DataAccess.ExecuteStoredProcedure("usp_SelectMMISStagedSpecialties", parameters, "Specialties");

                ds.Tables[0].TableName = dbtSpecialties;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
            return ds;
        }

        private void LogErrorRecord(Logging log, string actionType, string recordType, string recordValue, string exceptionMessage, string additionRecordInformation = "")
        {
            if (additionRecordInformation.Length > 0)
            {
                additionRecordInformation = ", " + additionRecordInformation;
            }
            string message = String.Format(Constants.LogString.ExtractErrorRecordFailure, actionType, (recordType + ": " + recordValue), additionRecordInformation);
            message += Environment.NewLine + exceptionMessage;
            log.CreateLogEntry(message, Logging.LogPriority.DataLoadIssues);
        }

        #endregion



    }
}
