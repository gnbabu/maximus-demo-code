using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.PNM
{
    public class ProcessMasterFileData : BaseJob, IJob
    {
        private Logging log = null;
        public int jobID;
        private const string weeklyMasterString = "84B7DFFD-8607-4E84-821B-2D01F8817CC5";//JobId for Full Extract Data Processing
        private const string weeklySpecialtyDataId = "9E8473FC-439E-4AF3-8258-EB9433E9F115";//JobId for Specialty Extract Data Processing
        private const string weeklySupplementalString = "AD704AB8-8380-47FD-9302-FAB4F648BCBD";//JobId for Supplemenatal Data Processing
        private Guid thisGuid;

        public ProcessMasterFileData(Guid threadId)
           : base(threadId)
        {
            this.ThreadId = threadId;
            this.thisGuid = new Guid(weeklyMasterString);
        }

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(thisGuid);
        }

        override public void ExecuteJob(Guid jobId)
        {
            switch (jobId.ToString().ToUpper())
            {
                case weeklySupplementalString:
                    ProcessSupplementData();
                    break;
                case weeklySpecialtyDataId:
                    ProcessPNMSpecialtyData();
                    break;
                default:
                    ProcessMasterData();
                    break;
            }
        }

        //Process PNM Full Extract data by giving a call to SP "usp_InsertPNMFullExtractData"
        public void ProcessMasterData()
        {
            try
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Guid threadID = this.thisGuid;
                log = new Logging(threadID, logMsg);
                //Create an entry in jobsummary table with status--inprogress.
                DataAccess.ExecuteStoredProcedure("usp_InsertPNMFullExtractData");
                //Update job status to success once executed sp for particular job id. Applied above logic inside sp
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("PNM Full Extract Data Exception: Error while generating the file {0}", ex.ToString()));
            }
        }
        //Process PNM Supplemental data by giving a call to below 10 SPs
        public void ProcessSupplementData()
        {
            try
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Guid threadID = this.thisGuid;
                log = new Logging(threadID, logMsg);
                //IN-PROGRESS status
                //insert sp o/p prameter jobid
                jobID = PMFShared.InsertPMFJobsSummary("SupplementalExtractData", "IN-PROGRESS", 0);
                int SP1RESULT = SupplementalCodeSetData(jobID);
                int SP2RESULT = SupplementalLicenseInformation(jobID);
                int SP3RESULT = SupplementalNationalCertificateInformation(jobID);
                int SP4RESULT = SupplementalPrimaryNPIInformation(jobID);
                int SP5RESULT = SupplementalSecondaryNPIInformation(jobID);
                int SP6RESULT = SupplementalTaxIDInformation(jobID);
                int SP7RESULT = SupplementalProviderDemographicInformation(jobID);
                int SP8RESULT = SupplementalProviderMedicareInformation(jobID);
                int SP9RESULT = SupplementalProvider340BInformation(jobID);
                int SP10RESULT = SupplementalProviderLanguageInformation(jobID);

                int newJobID = PMFShared.InsertPMFJobsSummary("SupplementalExtractData", "SUCCESS", jobID);

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Supplemental Extract Data Exception: Error while generating the file {0}", ex.ToString()));
            }
        }
        //Process PNM Specialty Extract data by giving a call to SP "usp_InsertPNMSpecialty"
        public void ProcessPNMSpecialtyData()
        {
            try
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Guid threadID = this.thisGuid;
                log = new Logging(threadID, logMsg);
                //IN-PROGRESS
                DataAccess.ExecuteStoredProcedure("usp_InsertPNMSpecialty");
                //SUCCESS
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("PNMSpecialty Data Exception: Error while generating the file {0}", ex.ToString()));
            }
        }
        public int SupplementalCodeSetData(int jobID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, jobID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertSupplementalCodeSetData", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public int SupplementalLicenseInformation(int jobID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, jobID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertSupplementalLicenseInformation", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public int SupplementalNationalCertificateInformation(int jobID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, jobID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertSupplementalNationalCertificateInformation", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public int SupplementalPrimaryNPIInformation(int jobID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, jobID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertSupplementalPrimaryNPIInformation", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public int SupplementalSecondaryNPIInformation(int jobID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, jobID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertSupplementalSecondaryNPIInformation", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public int SupplementalTaxIDInformation(int jobID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, jobID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertSupplementalTaxIDInformation", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public int SupplementalProviderDemographicInformation(int jobID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, jobID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertSupplementalProviderDemographicInformation", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public int SupplementalProviderMedicareInformation(int jobID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, jobID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertSupplementalProviderMedicareInformation", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public int SupplementalProvider340BInformation(int jobID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, jobID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertSupplementalProvider340BInformation", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public int SupplementalProviderLanguageInformation(int jobID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("JOB_ID", DbType.Int32, jobID, true));
                returnVal = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_InsertSupplementalProviderLanguageInformation", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
    }
}
