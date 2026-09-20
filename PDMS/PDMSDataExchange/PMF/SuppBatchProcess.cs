using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace MAXIMUS.DataExchange.PDMS.PNM
{
    public class SuppBatchProcess : BaseJob, IJob
    {
        private Logging log = null;
        public int jobID;
        private readonly string appID = "d6b89235-f2ca-4e0c-942e-c828324478bd"; //JobId for Supplemental File Processing
        private Dictionary<string, Dictionary<string, int>> _SectionMapping;

        public SuppBatchProcess(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
            _SectionMapping = new Dictionary<string, Dictionary<string, int>>();
            FillMapping();
        }

        override public void ExecuteJob()
        {
            // Default Job - Supplemental File Generation
            this.ExecuteJob(Guid.Parse(appID.ToString()));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    GenerateSupplementalFiles();
                    break;
            }
        }
        //Generates Supplemental File
        public void GenerateSupplementalFiles()
        {
            try
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Guid threadID = new Guid(appID);
                log = new Logging(threadID, logMsg);

                DirectoryInfo localDirectory;
                string extractType = "PROV_PMF_SUPPLEMENTAL";
                string localPath = AppSettings.Get("PNM-ExportLocalPath");
                localDirectory = new DirectoryInfo(String.Format(localPath));
                if (!localDirectory.Exists)
                {
                    localDirectory.Create();
                }
                //Constructs File name 
                string exportFileName = extractType + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHMM") + ".txt";
                StringBuilder strBuilder = new StringBuilder();
                //Retieves data related to Supplemental by calling below 10 SPs 

                //job_summary--inprogress,10 tables date_process,job-IDataAdapter update,job_summary--success
                jobID = PMFShared.InsertPMFJobsSummary("SupplementalExtractFile", "IN-PROGRESS", 0);
                DataTable codeSetData = SuppBatchProcess.SuppBatchProcessSp(log, threadID, "usp_GetSupplementalCodeSetData").Tables[0];
                if (codeSetData.Rows.Count > 0)
                {
                    foreach (DataRow dr in codeSetData.Rows)
                    {
                        int EXTRACT_ID = Convert.ToInt32(dr["SUPPLEMENTAL_CODESET_DATAE_ID"]);
                        int PNMSUPPJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "", "CodeSetData", jobID);
                    }

                    BuildString(codeSetData, strBuilder);
                }
                DataTable licenseInformation = SuppBatchProcess.SuppBatchProcessSp(log, threadID, "usp_GetSupplementalLicenseInformation").Tables[0];
                if (licenseInformation.Rows.Count > 0)
                {
                    foreach (DataRow dr in licenseInformation.Rows)
                    {
                        int EXTRACT_ID = Convert.ToInt32(dr["SUPPLEMENTAL_LICENSE_ID"]);
                        int PNMSUPPJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "", "LicenseInfo", jobID);
                    }
                    BuildString(licenseInformation, strBuilder);
                }
                DataTable licenseNCInformation = SuppBatchProcess.SuppBatchProcessSp(log, threadID, "usp_GetSupplementalNationalCertificateInformation").Tables[0];
                if (licenseNCInformation.Rows.Count > 0)
                {
                    foreach (DataRow dr in licenseNCInformation.Rows)
                    {
                        int EXTRACT_ID = Convert.ToInt32(dr["SUPPLEMENTAL_LICENSE_NATIONAL_ID"]);
                        int PNMSUPPJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "", "NationalCertificateInfo", jobID);
                    }
                    BuildString(licenseNCInformation, strBuilder);
                }
                DataTable primaryNPIInformation = SuppBatchProcess.SuppBatchProcessSp(log, threadID, "usp_GetSupplementalPrimaryNPIInformation").Tables[0];
                if (primaryNPIInformation.Rows.Count > 0)
                {
                    foreach (DataRow dr in primaryNPIInformation.Rows)
                    {
                        int EXTRACT_ID = Convert.ToInt32(dr["SUPPLEMENTAL_PRIMARYNPI_ID"]);
                        int PNMSUPPJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "", "PrimaryNPIInfo", jobID);
                    }
                    BuildString(primaryNPIInformation, strBuilder);
                }
                DataTable secondaryNPIInformation = SuppBatchProcess.SuppBatchProcessSp(log, threadID, "usp_GetSupplementalSecondaryNPIInformation").Tables[0];
                if (secondaryNPIInformation.Rows.Count > 0)
                {
                    foreach (DataRow dr in secondaryNPIInformation.Rows)
                    {
                        int EXTRACT_ID = Convert.ToInt32(dr["SUPPLEMENTAL_SECONDARYNPI_ID"]);
                        int PNMSUPPJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "", "SecondaryNPIInfo", jobID);
                    }
                    BuildString(secondaryNPIInformation, strBuilder);
                }
                DataTable taxIdInformation = SuppBatchProcess.SuppBatchProcessSp(log, threadID, "usp_GetSupplementalTaxIDInformation").Tables[0];
                if (taxIdInformation.Rows.Count > 0)
                {
                    foreach (DataRow dr in taxIdInformation.Rows)
                    {
                        int EXTRACT_ID = Convert.ToInt32(dr["SUPPLEMENTAL_TAXID_ID"]);
                        int PNMSUPPJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "", "TaxIDInfo", jobID);
                    }
                    BuildString(taxIdInformation, strBuilder);
                }
                DataTable providerDemoInformation = SuppBatchProcess.SuppBatchProcessSp(log, threadID, "usp_GetSupplementalProviderDemographicInformation").Tables[0];
                if (providerDemoInformation.Rows.Count > 0)
                {
                    foreach (DataRow dr in providerDemoInformation.Rows)
                    {
                        int EXTRACT_ID = Convert.ToInt32(dr["SUPPLEMENTAL_PROVIDER_DEMOGRAPHIC_ID"]);
                        int PNMSUPPJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "", "ProviderDemographicInfo", jobID);
                    }
                    BuildString(providerDemoInformation, strBuilder);
                }
                DataTable providerMedicareInformation = SuppBatchProcess.SuppBatchProcessSp(log, threadID, "usp_GetSupplementalProviderMedicareInformation").Tables[0];
                if (providerMedicareInformation.Rows.Count > 0)
                {
                    foreach (DataRow dr in providerMedicareInformation.Rows)
                    {
                        int EXTRACT_ID = Convert.ToInt32(dr["SUPPLEMENTAL_PROVIDER_MEDICARE_ID"]);
                        int PNMSUPPJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "", "ProviderMedicareInfo", jobID);
                    }
                    BuildString(providerMedicareInformation, strBuilder);
                }
                DataTable provider340BInformation = SuppBatchProcess.SuppBatchProcessSp(log, threadID, "usp_GetSupplementalProvider340BInformation").Tables[0];
                if (provider340BInformation.Rows.Count > 0)
                {
                    foreach (DataRow dr in provider340BInformation.Rows)
                    {
                        int EXTRACT_ID = Convert.ToInt32(dr["SUPPLEMENTAL_PROVIDER_340B_ID"]);
                        int PNMSUPPJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "", "Provider340BInfo", jobID);
                    }
                    BuildString(provider340BInformation, strBuilder);
                }
                DataTable providerLangInformation = SuppBatchProcess.SuppBatchProcessSp(log, threadID, "usp_GetSupplementalProviderLanguageInformation").Tables[0];
                if (providerLangInformation.Rows.Count > 0)
                {
                    foreach (DataRow dr in providerLangInformation.Rows)
                    {
                        int EXTRACT_ID = Convert.ToInt32(dr["SUPPLEMENTAL_PROVIDER_LANGUAGE_ID"]);
                        int PNMSUPPJobUpdate = PMFShared.PMFFullExtractJobCheckUpdate(EXTRACT_ID, "", "ProviderLanguageInfo", jobID);
                    }
                    BuildString(providerLangInformation, strBuilder);
                }
                string strFilePath = Path.Combine(localDirectory.ToString(), exportFileName);
                using (StreamWriter swriter = new StreamWriter(strFilePath))
                {
                    swriter.Write(strBuilder.ToString());//writes data to a file
                }
                int newJobID = PMFShared.InsertPMFJobsSummary("SupplementalExtractFile", "SUCCESS", jobID);

                //This one is purposely commented as it's a future feature 
                //Verify the file length and throw exception if overall file length is greater than 20MB
                //FileInfo fileInfo = new FileInfo(strFilePath);
                //if (fileInfo != null && ConvertBytesToMegabytes(fileInfo.Length) > 20)
                //{
                //    throw new Exception("File " + strFilePath + " exceeds 20MB limit.");
                //}

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Supplemental File Exception: Error while generating the file {0}", ex.ToString()));
            }
        }

        //Executes Supplemetal SPs
        public static DataSet SuppBatchProcessSp(Logging log, Guid threadId, string spName)
        {
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure(spName);
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("Supplemental Batch Process Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        private void BuildString(DataTable dt, StringBuilder stringBuilder)
        {
            string rest;
            var res = _SectionMapping[dt.Rows[0]["SectionType"].ToString()];
            foreach (DataRow dr in dt.Rows)
            {

                foreach (DataColumn column in dt.Columns)
                {
                    if (res.ContainsKey(column.ColumnName))
                    {
                        int pos = res[column.ColumnName];
                        stringBuilder.AppendFixed(pos, dr[column] != null ? dr[column].ToString() : "", out rest);

                    }
                }
                stringBuilder.Append("\x0a");//To have LF as a delimiter. 

            }

        }
        //Mapping for Column names, Starting & Ending positions
        private void FillMapping()
        {
            _SectionMapping.Add("Record Key", new Dictionary<string, int>());
            _SectionMapping["Record Key"].Add("Record Type", 4);
            _SectionMapping["Record Key"].Add("Provider ID", 8);
            _SectionMapping["Record Key"].Add("Supplemental Key Data", 30);

            _SectionMapping.Add("LICENSE INFORMATION", new Dictionary<string, int>());
            _SectionMapping["LICENSE INFORMATION"].Add("RECORD_TYPE", 4);
            _SectionMapping["LICENSE INFORMATION"].Add("PROVIDER_ID", 8);
            _SectionMapping["LICENSE INFORMATION"].Add("SUPPLEMENTAL_LICENSE_NUMBER", 30);
            _SectionMapping["LICENSE INFORMATION"].Add("LICENSE_TYPE", 30);
            _SectionMapping["LICENSE INFORMATION"].Add("LICENSE_EFFECTIVE_DATE", 8);
            _SectionMapping["LICENSE INFORMATION"].Add("LICENSE_END_DATE", 8);
            _SectionMapping["LICENSE INFORMATION"].Add("LICENSE_TYPE_CODE", 1);

            _SectionMapping.Add("LICENSE NATIONAL CERTIFICATE INFORMATION", new Dictionary<string, int>());
            _SectionMapping["LICENSE NATIONAL CERTIFICATE INFORMATION"].Add("RECORD_TYPE", 4);
            _SectionMapping["LICENSE NATIONAL CERTIFICATE INFORMATION"].Add("PROVIDER_ID", 8);
            _SectionMapping["LICENSE NATIONAL CERTIFICATE INFORMATION"].Add("SUPPLEMENTAL_KEY_DATA_LICENSE_NUMBER", 30);
            _SectionMapping["LICENSE NATIONAL CERTIFICATE INFORMATION"].Add("CERTIFICATE_SPECIALTY", 75);
            _SectionMapping["LICENSE NATIONAL CERTIFICATE INFORMATION"].Add("CERTIFICATE_FOCUS", 75);
            _SectionMapping["LICENSE NATIONAL CERTIFICATE INFORMATION"].Add("CERTIFYING_ORGANIZATION", 75);
            _SectionMapping["LICENSE NATIONAL CERTIFICATE INFORMATION"].Add("CERTIFICATE_EFFECTIVE_DATE", 8);
            _SectionMapping["LICENSE NATIONAL CERTIFICATE INFORMATION"].Add("CERTIFICATE_END_DATE", 8);

            _SectionMapping.Add("PRIMARY NPI INFORMATION", new Dictionary<string, int>());
            _SectionMapping["PRIMARY NPI INFORMATION"].Add("RECORD_TYPE", 4);
            _SectionMapping["PRIMARY NPI INFORMATION"].Add("PROVIDER_ID", 8);
            _SectionMapping["PRIMARY NPI INFORMATION"].Add("SUPPLEMENTAL_KEY_DATA_PRIMARY_NPI", 30);
            _SectionMapping["PRIMARY NPI INFORMATION"].Add("NPI_EFFECTIVE_DATE", 8);
            _SectionMapping["PRIMARY NPI INFORMATION"].Add("NPI_END_DATE", 8);

            _SectionMapping.Add("SECONDARY NPI INFORMATION", new Dictionary<string, int>());
            _SectionMapping["SECONDARY NPI INFORMATION"].Add("RECORD_TYPE", 4);
            _SectionMapping["SECONDARY NPI INFORMATION"].Add("PROVIDER_ID", 8);
            _SectionMapping["SECONDARY NPI INFORMATION"].Add("SUPPLEMENTAL_KEY_DATA_SECONDARY_NPI", 30);
            _SectionMapping["SECONDARY NPI INFORMATION"].Add("NPI_EFFECTIVE_DATE", 8);
            _SectionMapping["SECONDARY NPI INFORMATION"].Add("NPI_END_DATE", 8);

            _SectionMapping.Add("TAX ID INFORMATION", new Dictionary<string, int>());
            _SectionMapping["TAX ID INFORMATION"].Add("RECORD_TYPE", 4);
            _SectionMapping["TAX ID INFORMATION"].Add("PROVIDER_ID", 8);
            _SectionMapping["TAX ID INFORMATION"].Add("SUPPLEMENTAL_KEY_DATA_TAX_ID", 30);
            _SectionMapping["TAX ID INFORMATION"].Add("SAK_PROV", 9);
            _SectionMapping["TAX ID INFORMATION"].Add("TAX_ID_EFFECTIVE_DATE", 8);
            _SectionMapping["TAX ID INFORMATION"].Add("TAX_ID_END_DATE", 8);
            _SectionMapping["TAX ID INFORMATION"].Add("TAX_ID_TYPE", 1);

            _SectionMapping.Add("PROVIDER DEMOGRAPHIC INFORMATION", new Dictionary<string, int>());
            _SectionMapping["PROVIDER DEMOGRAPHIC INFORMATION"].Add("RECORD_TYPE", 4);
            _SectionMapping["PROVIDER DEMOGRAPHIC INFORMATION"].Add("PROVIDER_ID", 8);
            _SectionMapping["PROVIDER DEMOGRAPHIC INFORMATION"].Add("SUPPLEMENTAL_KEY_DATA_N/A", 30);
            _SectionMapping["PROVIDER DEMOGRAPHIC INFORMATION"].Add("DATE_OF_BIRTH", 8);
            _SectionMapping["PROVIDER DEMOGRAPHIC INFORMATION"].Add("GENDER_CODE", 1);
            _SectionMapping["PROVIDER DEMOGRAPHIC INFORMATION"].Add("TITLE_CODE", 15);
            _SectionMapping["PROVIDER DEMOGRAPHIC INFORMATION"].Add("DIRECTORY_SEARCH_INDICATOR", 1);

            _SectionMapping.Add("PROVIDER MEDICARE INFORMATION", new Dictionary<string, int>());
            _SectionMapping["PROVIDER MEDICARE INFORMATION"].Add("RECORD_TYPE", 4);
            _SectionMapping["PROVIDER MEDICARE INFORMATION"].Add("PROVIDER_ID", 8);
            _SectionMapping["PROVIDER MEDICARE INFORMATION"].Add("SUPPLEMENTAL_KEY_DATA_MEDICARE_NUMBER", 30);
            _SectionMapping["PROVIDER MEDICARE INFORMATION"].Add("EFFECTIVE_DATE", 8);
            _SectionMapping["PROVIDER MEDICARE INFORMATION"].Add("END_DATE", 8);
            _SectionMapping["PROVIDER MEDICARE INFORMATION"].Add("MEDICARE_TYPE_CODE", 1);

            _SectionMapping.Add("PROVIDER 340B INFORMATION", new Dictionary<string, int>());
            _SectionMapping["PROVIDER 340B INFORMATION"].Add("RECORD_TYPE", 4);
            _SectionMapping["PROVIDER 340B INFORMATION"].Add("PROVIDER_ID", 8);
            _SectionMapping["PROVIDER 340B INFORMATION"].Add("SUPPLEMENTAL_KEY_DATA_340B_ID", 30);
            _SectionMapping["PROVIDER 340B INFORMATION"].Add("EFFECTIVE_DATE", 8);
            _SectionMapping["PROVIDER 340B INFORMATION"].Add("END_DATE", 8);

            _SectionMapping.Add("PROVIDER LANGUAGE INFORMATION", new Dictionary<string, int>());
            _SectionMapping["PROVIDER LANGUAGE INFORMATION"].Add("RECORD_TYPE", 4);
            _SectionMapping["PROVIDER LANGUAGE INFORMATION"].Add("PROVIDER_ID", 8);
            _SectionMapping["PROVIDER LANGUAGE INFORMATION"].Add("SUPPLEMENTAL_KEY_DATA_LANGUAGE_CODE", 30);
            _SectionMapping["PROVIDER LANGUAGE INFORMATION"].Add("EFFECTIVE_DATE", 8);
            _SectionMapping["PROVIDER LANGUAGE INFORMATION"].Add("END_DATE", 8);

            _SectionMapping.Add("CODE SET DATA", new Dictionary<string, int>());
            _SectionMapping["CODE SET DATA"].Add("RECORD_TYPE", 4);
            _SectionMapping["CODE SET DATA"].Add("RESERVED", 2);
            _SectionMapping["CODE SET DATA"].Add("CODE_SET_GROUP", 18);
            _SectionMapping["CODE SET DATA"].Add("CODE_SET_VALUE", 18);
            _SectionMapping["CODE SET DATA"].Add("CODE_SET_VALUE_DESCRIPTION", 250);
        }

    }
}
