using FileHelpers;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MCPN;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Linq;

namespace MAXIMUS.DataExchange.PDMS.CredRoster
{
    public class CredentialRosterExport : BaseJob, IJob
    {
        private PDMSService.PDMSServiceClient _svc;
        private PDMSService.PDMSServiceClient svc
        {
            get
            {
                if (_svc == null)
                {
                    _svc = new PDMSService.PDMSServiceClient();
                }

                return _svc;
            }
        }       

        #region "Constructors"

        public CredentialRosterExport(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        #endregion

        #region "Logging Objects"

        private int logCnt = 0;
        private Logging log = null;

        #endregion

        #region "Public Methods"

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(Guid.Parse("DD9FB411-422F-4AE2-B0C3-B5632DFE8B69"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            // This will be a full file of all providers credentialed in PNM and sent daily in XML format.
           this.LoadCredentialRoster();
        }
        
        #endregion

        #region "Private Methods"
        private void LoadCredentialRoster()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry("Start Daily Credential Roster Export data");
           
            try
            {
                
                DataSet dsCredRosterReg = DataAccess.ExecuteStoredProcedure("usp_SelectProviders_For_CredentialRoster", "CredRosterRegIds");
                if (ObjectControllerHelper.HasRows(dsCredRosterReg))
                {
                    int CredRosterProvCnt = dsCredRosterReg.Tables[0].Rows.Count;
                    log.CreateLogEntry("Credential Roster Total Provider Count - " + CredRosterProvCnt.ToString());

                    CredentialRosterXMLReference.Providers CredProvider = new CredentialRosterXMLReference.Providers();
                    CredentialRosterXMLReference.CredRosterProviderInformation[] CredProvInfo 
                        = new CredentialRosterXMLReference.CredRosterProviderInformation[CredRosterProvCnt];

                    for (int i = 0; i < CredRosterProvCnt; i++)
                    {
                        DataRow row = dsCredRosterReg.Tables[0].Rows[i];
                        int regID = ObjectControllerHelper.GetInt("REG_ID", row);
                        try
                        {
                            List<SqlParameter> sqlParms = new List<SqlParameter>();
                            sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.String, regID.ToString(), false));
                            sqlParms.Add(SqlParms.CreateParameter("DTM", DbType.DateTime, DateTime.Now, false));
                            sqlParms.Add(SqlParms.CreateParameter("User", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));
                            DataSet dsCredRegData = DataAccess.ExecuteStoredProcedure("usp_Get_CredentialRosterData_ByRegID", sqlParms, "CredRosterProvData"+ regID);
                            if (ObjectControllerHelper.HasRows(dsCredRegData))
                            {
                                CredProvInfo[i] = new CredentialRosterXMLReference.CredRosterProviderInformation();
                                CredProvInfo[i].ProviderDemographic = FillCredRosterDemographic(dsCredRegData);
                                CredProvInfo[i].ProviderAlternateIdentifiers = FillCredRosterAlternateIdentifiers(dsCredRegData);
                                CredProvInfo[i].ProviderSpecialties = FillCredRosterSpecialties(dsCredRegData);
                                CredProvInfo[i].ProviderLicenses = FillCredRosterLicenses(dsCredRegData);
                                CredProvInfo[i].ProviderCertifications = FillCredRosterCertifications(dsCredRegData);
                                CredProvInfo[i].ProviderTaxonomies = FillCredRosterTaxonomies(dsCredRegData);
                                CredProvInfo[i].ProviderHospitalAffiliations = FillCredRosterHospAffiliations(dsCredRegData);
                                CredProvInfo[i].ProviderAffiliations = FillCredRosterAffiliations(dsCredRegData);
                                CredProvInfo[i].ProviderEducation = FillCredRosterEducation(dsCredRegData);
                                CredProvInfo[i].ProviderInsurance = FillCredRosterInsurance(dsCredRegData);
                                CredProvInfo[i].ProviderAddress = FillCredRosterAddress(dsCredRegData);
                                log.CreateLogEntry("Credential Roster added REG_ID - " + regID.ToString());
                            }
                        }
                        catch(Exception ex)
                        {
                            log.CreateLogEntry("Error getting the data for RegID - "+ regID + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                            throw CoreException.ThrowException(this.ThreadId, ex);
                        }
                    }
                    CredProvider.ProviderInformation = CredProvInfo;

                    //Create the XML file
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(CredentialRosterXMLReference.Providers));
                    var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    var settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.OmitXmlDeclaration = true;

                    var stream = new StringWriter();
                    var writer = XmlWriter.Create(stream, settings);
                    x.Serialize(writer, CredProvider, emptyNs);
                    string xml = stream.ToString();

                    xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + xml;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);
                    var newxml = xmlDoc.InnerXml.ToString().Replace("<Providers>", "<Providers xmlns=\"http://Maximus.OHPNM.Services/CredentialRoster\">");

                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(newxml);

                    string filePath = AppSettings.Get("CredentialRoster-ExportLocalPath");
                    string fileName = AppSettings.Get("CredentialRosterFile");
                    string dtmWildcard = AppSettings.Get("SubmitFileWildcardDTM");
                    fileName = fileName.Replace(dtmWildcard, DateTime.Now.ToString(dtmWildcard)) + ".xml";
                    
                    DirectoryInfo localDirectory;
                    localDirectory = new DirectoryInfo(String.Format(filePath));
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        localDirectory = new DirectoryInfo(String.Format(@"C:\Projects\Deployments\OHPNM\CredentialRoster"));
                    }
                    if (!localDirectory.Exists)
                    {
                        localDirectory.Create();
                    }

                    filePath = Path.Combine(localDirectory.ToString(), fileName);
                    //string fileName = "FC000" + DateTime.Now.ToString("yyyyMMdd") + ".daily.xml";
                    //filePath = "C:\\Projects\\MyScripts\\OHPDMS\\DesignDocs\\CR076\\" + "/" + fileName; // CredRoster2.xml";
                    File.AppendAllText(filePath, xmlDoc.InnerXml + Environment.NewLine);

                }
                else
                {
                    log.CreateLogEntry("Failed to get the RegID from usp_ExtractCredentialRoster");
                }

            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to get the Credential Roster Reg IDs." + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }

        }
        public static CredentialRosterXMLReference.CredRosterProviderDemographic FillCredRosterDemographic(DataSet ds)
        {
            CredentialRosterXMLReference.CredRosterProviderDemographic crDemographic = new CredentialRosterXMLReference.CredRosterProviderDemographic();
            
            if(ObjectControllerHelper.HasRows(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                crDemographic.RecordType = ObjectControllerHelper.GetString("RecordType",dr).Trim();
                crDemographic.ProviderStatusType = ObjectControllerHelper.GetString("ProviderStatusType", dr).Trim();
                crDemographic.MedicaidID = ObjectControllerHelper.GetString("MedicaidID", dr).Trim();
                crDemographic.PracticeName = ObjectControllerHelper.GetString("PracticeName", dr).Trim();
                crDemographic.LastName = ObjectControllerHelper.GetString("LastName", dr).Trim();
                crDemographic.FirstName = ObjectControllerHelper.GetString("FirstName", dr).Trim();
                crDemographic.MiddleName = ObjectControllerHelper.GetString("MiddleName", dr).Trim();
                crDemographic.Title = ObjectControllerHelper.GetString("Title", dr).Trim();
                crDemographic.Gender = ObjectControllerHelper.GetString("Gender", dr).Trim();
                crDemographic.TaxID = ObjectControllerHelper.GetString("TaxID", dr).Trim();
                crDemographic.DateOfBirth = ObjectControllerHelper.GetString("DateOfBirth", dr).Trim();
                crDemographic.NPI = ObjectControllerHelper.GetString("NPI", dr).Trim();
                crDemographic.MITSProviderType = ObjectControllerHelper.GetString("MITSProviderType", dr).Trim();
                crDemographic.EffectiveDate = ObjectControllerHelper.GetString("EffectiveDate", dr).Trim();
                crDemographic.ProviderCredentialDate = ObjectControllerHelper.GetString("ProviderCredentialDate", dr).Trim();
                crDemographic.ProviderRecredentialDueDate = ObjectControllerHelper.GetString("ProviderRecredentialDueDate", dr).Trim();
                crDemographic.CredentialingNotRequiredDate = ObjectControllerHelper.GetString("CredentialingNotRequiredDate", dr).Trim();
                crDemographic.ThreeFortyB = ObjectControllerHelper.GetString("ThreeFortyB", dr).Trim();
                crDemographic.Hospitalist = ObjectControllerHelper.GetString("Hospitalist", dr).Trim();
                crDemographic.OfficeUtilizePhysicianExtenders = ObjectControllerHelper.GetString("OfficeUtilizePhysicianExtenders", dr).Trim();
                crDemographic.NumberOfNPs = ObjectControllerHelper.GetString("NumberOfNPs", dr).Trim();
                crDemographic.NumberOfPAs = ObjectControllerHelper.GetString("NumberOfPAs", dr).Trim();
                crDemographic.CAQHNumber = ObjectControllerHelper.GetString("CAQHNumber", dr).Trim();
                crDemographic.CredentialingContactName = ObjectControllerHelper.GetString("CredentialingContactName", dr).Trim();
                crDemographic.CredentialingContactPhone = ObjectControllerHelper.GetString("CredentialingContactPhone", dr).Trim();
                crDemographic.CredentialingContactFax = ObjectControllerHelper.GetString("CredentialingContactFax", dr).Trim();
                crDemographic.CredentialingContactEmail = ObjectControllerHelper.GetString("CredentialingContactEmail", dr).Trim();
                crDemographic.BusinessContactName = ObjectControllerHelper.GetString("BusinessContactName", dr).Trim();
                crDemographic.BusinessContactPhone = ObjectControllerHelper.GetString("BusinessContactPhone", dr).Trim();
                crDemographic.BusinessContactFax = ObjectControllerHelper.GetString("BusinessContactFax", dr).Trim();
                crDemographic.BusinessContactEmailAddress = ObjectControllerHelper.GetString("BusinessContactEmailAddress", dr).Trim();
                crDemographic.MedicareOptOut = ObjectControllerHelper.GetString("MedicareOptOut", dr).Trim();
            }

            return crDemographic;
        }
        
        public static CredentialRosterXMLReference.CredRosterProviderAlternateIdentifiersList[] FillCredRosterAlternateIdentifiers(DataSet ds)
        {
            DataTable dt = ObjectControllerHelper.HasRows(ds) ? ds.Tables[1] : null;

            int countRow = dt.Rows.Count;
            if (countRow == 0) return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterProviderAlternateIdentifiersList[] crAlternateIdList = new CredentialRosterXMLReference.CredRosterProviderAlternateIdentifiersList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crAlternateIdList[i] = new CredentialRosterXMLReference.CredRosterProviderAlternateIdentifiersList();
                crAlternateIdList[i].AlternateIDType = ObjectControllerHelper.GetString("AlternateIDType", dr).Trim(); 
                crAlternateIdList[i].AlternateId = ObjectControllerHelper.GetString("AlternateId", dr).Trim();
                crAlternateIdList[i].EffectiveDate = ObjectControllerHelper.GetString("EffectiveDate", dr).Trim();
                crAlternateIdList[i].EndDate = ObjectControllerHelper.GetString("EndDate", dr).Trim();
                crAlternateIdList[i].AdditionalInformation = ObjectControllerHelper.GetString("AdditionalInformation", dr).Trim();
            }
            return crAlternateIdList;
        }

        public static CredentialRosterXMLReference.CredRosterProviderSpecialtiesList[] FillCredRosterSpecialties(DataSet ds)
        {
            DataTable dt = ObjectControllerHelper.HasRows(ds) ? ds.Tables[2] : null;

            int countRow = dt.Rows.Count;
            if (countRow == 0) return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterProviderSpecialtiesList[] crSpecList = new CredentialRosterXMLReference.CredRosterProviderSpecialtiesList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crSpecList[i] = new CredentialRosterXMLReference.CredRosterProviderSpecialtiesList();
                crSpecList[i].PrimarySpecialtyIndicator = ObjectControllerHelper.GetString("PrimarySpecialtyIndicator", dr).Trim();
                crSpecList[i].SpecialtyCode = ObjectControllerHelper.GetString("SpecialtyCode", dr).Trim();
                crSpecList[i].EffectiveDate = ObjectControllerHelper.GetString("EffectiveDate", dr).Trim();
                crSpecList[i].EndDate = ObjectControllerHelper.GetString("EndDate", dr).Trim();
            }
            return crSpecList;
        }

        public static CredentialRosterXMLReference.CredRosterProviderLicensesList[] FillCredRosterLicenses(DataSet ds)
        {
            DataTable dt = ObjectControllerHelper.HasRows(ds) ? ds.Tables[3] : null;

            int countRow = dt.Rows.Count;
            if (countRow == 0) return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterProviderLicensesList[] crLicenseList = new CredentialRosterXMLReference.CredRosterProviderLicensesList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crLicenseList[i] = new CredentialRosterXMLReference.CredRosterProviderLicensesList();
                crLicenseList[i].LicenseNumber = ObjectControllerHelper.GetString("LicenseNumber", dr).Trim();
                crLicenseList[i].LicenseeStateCode = ObjectControllerHelper.GetString("LicenseeStateCode", dr).Trim();
                crLicenseList[i].EffectiveDate = ObjectControllerHelper.GetString("EffectiveDate", dr).Trim();
                crLicenseList[i].EndDate = ObjectControllerHelper.GetString("EndDate", dr).Trim();
            }
            return crLicenseList;
        }
        public static CredentialRosterXMLReference.CredRosterProviderCertificationsList[] FillCredRosterCertifications(DataSet ds)
        {
            DataTable dt = ObjectControllerHelper.HasRows(ds) ? ds.Tables[4] : null;

            int countRow = dt.Rows.Count;
            if (countRow == 0) return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterProviderCertificationsList[] crBoardCertList = new CredentialRosterXMLReference.CredRosterProviderCertificationsList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crBoardCertList[i] = new CredentialRosterXMLReference.CredRosterProviderCertificationsList();
                crBoardCertList[i].PrimaryBoardCertifiedIndicator = ObjectControllerHelper.GetString("PrimaryBoardCertifiedIndicator", dr).Trim();
                crBoardCertList[i].CertificateName = ObjectControllerHelper.GetString("CertificateName", dr).Trim();
                crBoardCertList[i].EffectiveDate = ObjectControllerHelper.GetString("EffectiveDate", dr).Trim();
                crBoardCertList[i].EndDate = ObjectControllerHelper.GetString("EndDate", dr).Trim();
            }
            return crBoardCertList;
        }
        public static CredentialRosterXMLReference.CredRosterProviderTaxonomiesList[] FillCredRosterTaxonomies(DataSet ds)
        {
            DataTable dt = ObjectControllerHelper.HasRows(ds) ? ds.Tables[5] : null;

            int countRow = dt.Rows.Count;
            if (countRow == 0) return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterProviderTaxonomiesList[] crTaxList = new CredentialRosterXMLReference.CredRosterProviderTaxonomiesList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crTaxList[i] = new CredentialRosterXMLReference.CredRosterProviderTaxonomiesList();
                crTaxList[i].PrimaryTaxonomyIndicator = ObjectControllerHelper.GetString("PrimaryTaxonomyIndicator", dr).Trim();
                crTaxList[i].TaxonomyCode = ObjectControllerHelper.GetString("TaxonomyCode", dr).Trim();
            }
            return crTaxList;
        }
        public static CredentialRosterXMLReference.CredRosterProviderHospitalAffiliationsList[] FillCredRosterHospAffiliations(DataSet ds)
        {
            DataTable dt = ObjectControllerHelper.HasRows(ds) ? ds.Tables[6] : null;

            int countRow = dt.Rows.Count;
            if (countRow == 0) return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterProviderHospitalAffiliationsList[] crHospAffList = new CredentialRosterXMLReference.CredRosterProviderHospitalAffiliationsList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crHospAffList[i] = new CredentialRosterXMLReference.CredRosterProviderHospitalAffiliationsList();
                crHospAffList[i].PrimaryIndicator = ObjectControllerHelper.GetString("PrimaryIndicator", dr).Trim();
                crHospAffList[i].HospitalMedicaidID = ObjectControllerHelper.GetString("HospitalMedicaidID", dr).Trim();
                crHospAffList[i].HospitalName = ObjectControllerHelper.GetString("HospitalName", dr).Trim();
                crHospAffList[i].StatusofPrivileges = ObjectControllerHelper.GetString("StatusofPrivileges", dr).Trim();
                crHospAffList[i].StaffCategory = ObjectControllerHelper.GetString("StaffCategory", dr).Trim();
                crHospAffList[i].EffectiveDate = ObjectControllerHelper.GetString("EffectiveDate", dr).Trim();
                crHospAffList[i].EndDate = ObjectControllerHelper.GetString("EndDate", dr).Trim();
            }
            return crHospAffList;
        }
        public static CredentialRosterXMLReference.CredRosterProviderAffiliationsList[] FillCredRosterAffiliations(DataSet ds)
        {
            DataTable dt = ObjectControllerHelper.HasRows(ds) ? ds.Tables[7] : null;

            int countRow = dt.Rows.Count;
            if (countRow == 0) return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterProviderAffiliationsList[] crAffList = new CredentialRosterXMLReference.CredRosterProviderAffiliationsList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crAffList[i] = new CredentialRosterXMLReference.CredRosterProviderAffiliationsList();
                crAffList[i].GroupMedicaidID = ObjectControllerHelper.GetString("GroupMedicaidID", dr).Trim();
                crAffList[i].UniqueLocationID = ObjectControllerHelper.GetString("UniqueLocationID", dr).Trim();
                crAffList[i].OptOut = ObjectControllerHelper.GetString("OptOut", dr).Trim();
                crAffList[i].EffectiveDate = ObjectControllerHelper.GetString("EffectiveDate", dr).Trim();
                crAffList[i].EndDate = ObjectControllerHelper.GetString("EndDate", dr).Trim();
            }
            return crAffList;
        }
        public static CredentialRosterXMLReference.CredRosterProviderEducationList[] FillCredRosterEducation(DataSet ds)
        {
            DataTable dt = ObjectControllerHelper.HasRows(ds) ? ds.Tables[8] : null;

            int countRow = dt.Rows.Count;
            if (countRow == 0)  return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterProviderEducationList[] crEduList = new CredentialRosterXMLReference.CredRosterProviderEducationList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crEduList[i] = new CredentialRosterXMLReference.CredRosterProviderEducationList();
                crEduList[i].EducationType = ObjectControllerHelper.GetString("EducationType", dr).Trim();
                crEduList[i].ProviderDegree = ObjectControllerHelper.GetString("ProviderDegree", dr).Trim();
                crEduList[i].SchoolName = ObjectControllerHelper.GetString("SchoolName", dr).Trim();
                crEduList[i].Location = ObjectControllerHelper.GetString("Location", dr).Trim();
                crEduList[i].City = ObjectControllerHelper.GetString("City", dr).Trim();
                crEduList[i].State = ObjectControllerHelper.GetString("State", dr).Trim();
                crEduList[i].ZipCode = ObjectControllerHelper.GetString("ZipCode", dr).Trim();
                crEduList[i].Specialty = ObjectControllerHelper.GetString("Specialty", dr).Trim();
                crEduList[i].EffectiveDate = ObjectControllerHelper.GetString("EffectiveDate", dr).Trim();
                crEduList[i].EndDate = ObjectControllerHelper.GetString("EndDate", dr).Trim();
            }
            return crEduList;
        }
        public static CredentialRosterXMLReference.CredRosterProviderInsurance FillCredRosterInsurance(DataSet ds)
        {
            CredentialRosterXMLReference.CredRosterProviderInsurance crInsurance = new CredentialRosterXMLReference.CredRosterProviderInsurance();
            
            if (ds.Tables[9].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[9].Rows[0];
                crInsurance.MalpracticeCoverageIndicator = ObjectControllerHelper.GetString("MalpracticeCoverageIndicator",dr).Trim();
                crInsurance.StartDate = ObjectControllerHelper.GetString("StartDate", dr).Trim();
                crInsurance.ExpirationDate = ObjectControllerHelper.GetString("ExpirationDate", dr).Trim();
                crInsurance.LimitOccurance = ObjectControllerHelper.GetString("LimitOccurance", dr).Trim();
                crInsurance.LimitAggregate = ObjectControllerHelper.GetString("LimitAggregate", dr).Trim();
                crInsurance.Carrier = ObjectControllerHelper.GetString("Carrier", dr).Trim();
                crInsurance.FTCAIndicator = ObjectControllerHelper.GetString("FTCAIndicator", dr).Trim();
                crInsurance.PolicyNumber = ObjectControllerHelper.GetString("PolicyNumber", dr).Trim();
                return crInsurance;
            }
            else
            {
                return null;
            }
        }
        public static CredentialRosterXMLReference.CredRosterProviderAddressList[] FillCredRosterAddress(DataSet ds)
        {
            int RegAddressID = 0;
            DataTable dt = ds.Tables[10];

            int countRow = dt.Rows.Count;
            if (countRow == 0) return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterProviderAddressList[] crAddrList = new CredentialRosterXMLReference.CredRosterProviderAddressList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crAddrList[i] = new CredentialRosterXMLReference.CredRosterProviderAddressList();
                RegAddressID = ObjectControllerHelper.GetInt("AddressID", dr);
                crAddrList[i].AddressID = ObjectControllerHelper.GetString("AddressID", dr).Trim();
                crAddrList[i].AddressType = ObjectControllerHelper.GetString("AddressType", dr).Trim();
                crAddrList[i].AddressLine1 = ObjectControllerHelper.GetString("AddressLine1", dr).Trim();
                crAddrList[i].AddressLine2 = ObjectControllerHelper.GetString("AddressLine2", dr).Trim();
                crAddrList[i].City = ObjectControllerHelper.GetString("City", dr).Trim();
                crAddrList[i].State = ObjectControllerHelper.GetString("State", dr).Trim();
                crAddrList[i].CountyCode = ObjectControllerHelper.GetString("CountyCode", dr).Trim();
                crAddrList[i].ZipCode = ObjectControllerHelper.GetString("ZipCode", dr).Trim();
                crAddrList[i].Country = ObjectControllerHelper.GetString("Country", dr).Trim();
                crAddrList[i].AddressName = ObjectControllerHelper.GetString("AddressName", dr).Trim();
                crAddrList[i].AddressFirstName = ObjectControllerHelper.GetString("AddressFirstName", dr).Trim();
                crAddrList[i].AddressMiddleName = ObjectControllerHelper.GetString("AddressMiddleName", dr).Trim();
                crAddrList[i].AddressLastName = ObjectControllerHelper.GetString("AddressLastName", dr).Trim();
                crAddrList[i].PhoneNumber = ObjectControllerHelper.GetString("PhoneNumber", dr).Trim();
                crAddrList[i].FaxNumber = ObjectControllerHelper.GetString("FaxNumber", dr).Trim();
                crAddrList[i].Email = ObjectControllerHelper.GetString("Email", dr).Trim();
                crAddrList[i].PublicTransportationAccess = ObjectControllerHelper.GetString("PublicTransportationAccess", dr).Trim();
                crAddrList[i].TwentyFourHourPhoneCoverage = ObjectControllerHelper.GetString("TwentyFourHourPhoneCoverage", dr).Trim();
                crAddrList[i].BuildingAccess = ObjectControllerHelper.GetString("BuildingAccess", dr).Trim();
                crAddrList[i].ExamRoom = ObjectControllerHelper.GetString("ExamRoom", dr).Trim();
                crAddrList[i].ElectronicClaimSubmission = ObjectControllerHelper.GetString("ElectronicClaimSubmission", dr).Trim();
                crAddrList[i].TDDTTY = ObjectControllerHelper.GetString("TDDTTY", dr).Trim();
                crAddrList[i].ASLOffered = ObjectControllerHelper.GetString("ASLOffered", dr).Trim();
                crAddrList[i].LocationinProviderDirectory = ObjectControllerHelper.GetString("LocationinProviderDirectory", dr).Trim();
                crAddrList[i].LanguageLine = ObjectControllerHelper.GetString("LanguageLine", dr).Trim();
                crAddrList[i].TranslationServices = ObjectControllerHelper.GetString("TranslationServices", dr).Trim();
                crAddrList[i].AcceptingNewPatients = ObjectControllerHelper.GetString("AcceptingNewPatients", dr).Trim();
                crAddrList[i].AcceptNewPatientsFromReferralOnly = ObjectControllerHelper.GetString("AcceptNewPatientsFromReferralOnly", dr).Trim();
                crAddrList[i].GenderOfPatientsAccepted = ObjectControllerHelper.GetString("GenderOfPatientsAccepted", dr).Trim();
                crAddrList[i].YoungestPatientAccepted = ObjectControllerHelper.GetString("YoungestPatientAccepted", dr).Trim();
                crAddrList[i].OldestPatientAccepted = ObjectControllerHelper.GetString("OldestPatientAccepted", dr).Trim();
                crAddrList[i].WebsiteAddress = ObjectControllerHelper.GetString("WebsiteAddress", dr).Trim();
                crAddrList[i].ServiceLocationLanguages = FillCredRosterAddressLanguages(ds, RegAddressID);
                crAddrList[i].ServiceLocationTrainings = FillCredRosterAddressTrainings(ds, RegAddressID);
                crAddrList[i].ServiceLocationOfficeHours = FillCredRosterAddressOfficeHours(ds, RegAddressID);
            }
            return crAddrList;
        }

        public static CredentialRosterXMLReference.CredRosterServiceLocationLanguagesList[] FillCredRosterAddressLanguages(DataSet ds, int RegAddressID)
        {
            // DataTable dt = ds.Tables[11];
            try
            {
                DataTable dt = ds.Tables[11].AsEnumerable().Where(row => (row.Field<int?>("REG_ADDRESS_ID") == RegAddressID))
                                    .Count() > 0 ? ds.Tables[11].AsEnumerable().Where(row => (row.Field<int?>("REG_ADDRESS_ID") == RegAddressID)).CopyToDataTable() : null;
                if (dt == null) return null;
                int countRow = dt.Rows.Count;
                if (countRow == 0) return null;
                DataRow dr = null;
                CredentialRosterXMLReference.CredRosterServiceLocationLanguagesList[] crAddrLangList = new CredentialRosterXMLReference.CredRosterServiceLocationLanguagesList[countRow];
                for (int i = 0; i < countRow; i++)
                {
                    dr = dt.Rows[i];
                    crAddrLangList[i] = new CredentialRosterXMLReference.CredRosterServiceLocationLanguagesList();
                    crAddrLangList[i].LanguageSpokenBy = ObjectControllerHelper.GetString("LanguageSpokenBy", dr).Trim();
                    crAddrLangList[i].LanguageCode = ObjectControllerHelper.GetString("LanguageCode", dr).Trim();
                }
                return crAddrLangList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        public static CredentialRosterXMLReference.CredRosterServiceLocationTrainingsList[] FillCredRosterAddressTrainings(DataSet ds, int RegAddressID)
        {
            //DataTable dt = (ds.Tables[12]).AsEnumerable()
            //                                 .Where(row => row.Field<int?>("REG_ADDRESS_ID") == RegAddressID).CopyToDataTable();
            DataTable dt = ds.Tables[12].AsEnumerable().Where(row => (row.Field<int?>("REG_ADDRESS_ID") == RegAddressID))
                                 .Count() > 0 ? ds.Tables[12].AsEnumerable().Where(row => (row.Field<int?>("REG_ADDRESS_ID") == RegAddressID)).CopyToDataTable() : null;

            if (dt == null) return null;
            int countRow = dt.Rows.Count;
            if (countRow == 0) return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterServiceLocationTrainingsList[] crAddrTrngList = new CredentialRosterXMLReference.CredRosterServiceLocationTrainingsList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crAddrTrngList[i] = new CredentialRosterXMLReference.CredRosterServiceLocationTrainingsList();
                crAddrTrngList[i].TrainingBy = ObjectControllerHelper.GetString("TrainingBy", dr).Trim();
                crAddrTrngList[i].TrainingType = ObjectControllerHelper.GetString("TrainingType", dr).Trim();
                crAddrTrngList[i].TrainingIndicator = ObjectControllerHelper.GetString("TrainingIndicator", dr).Trim();
            }
            return crAddrTrngList;
        }

        public static CredentialRosterXMLReference.CredRosterServiceLocationOfficeHoursList[] FillCredRosterAddressOfficeHours(DataSet ds, int RegAddressID)
        {
            DataTable dt = ds.Tables[13].AsEnumerable().Where(row => (row.Field<int?>("REG_ADDRESS_ID") == RegAddressID))
                                 .Count() > 0 ? ds.Tables[13].AsEnumerable().Where(row => (row.Field<int?>("REG_ADDRESS_ID") == RegAddressID)).CopyToDataTable() : null;


            if (dt == null) return null;
            int countRow = dt.Rows.Count;
            if (countRow == 0) return null;
            DataRow dr = null;
            CredentialRosterXMLReference.CredRosterServiceLocationOfficeHoursList[] crAddrOffList = new CredentialRosterXMLReference.CredRosterServiceLocationOfficeHoursList[countRow];
            for (int i = 0; i < countRow; i++)
            {
                dr = dt.Rows[i];
                crAddrOffList[i] = new CredentialRosterXMLReference.CredRosterServiceLocationOfficeHoursList();
                crAddrOffList[i].Day = ObjectControllerHelper.GetString("DAY_NAME", dr).Trim();
                crAddrOffList[i].StartTime = ObjectControllerHelper.GetString("START_TIME", dr).Trim();
                crAddrOffList[i].EndTime = ObjectControllerHelper.GetString("END_TIME", dr).Trim();
                crAddrOffList[i].Open24HoursIndicator = ObjectControllerHelper.GetString("INDICATOR_24", dr).Trim();
            }
            return crAddrOffList;
        }
        #endregion

    }
}
