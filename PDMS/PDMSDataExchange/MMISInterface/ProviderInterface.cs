using FileHelpers;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{
    public class ProviderInterface : BaseJob, IJob
    {
#region "Class Level Declarations"

#endregion

#region "Constructors"
        public ProviderInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }
#endregion

#region "Public Methods"
        override public void ExecuteJob()
        {
            // Default Job - MMIS Retrieve submit provider response
            this.ExecuteJob(Guid.Parse("A95CFD86-F9FD-414C-9F97-7B8EC88E947C"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                // MMIS Retrieve Provider
                case "A95CFD86-F9FD-414C-9F97-7B8EC88E947C":
                    this.RetrieveProviders();
                    break;

                // MMIS Submit Provider
                case "6CED36D6-2683-4383-A50F-1D50B46F2C18":
                    this.SubmitProviders();
                    break;

                
                // MMIS retrieve Provider group affiliation
                case "A5DEB662-3D2D-4811-8FE3-6CAAB5BA7825":
                    this.RetrieveGroupAffiliation();
                    break;
                

                // MMIS submit Provider group affiliation
                case "2579702E-FF23-422B-A0FA-3CDC2D6DB1F5":
                    this.SubmitGroupAffiliation();
                    break;

                //// MMIS submit Provider non-primary specialties/taxonomies
                //case "F682F771-3B56-4D0A-A182-A1164CDA2927":
                //    this.SubmitAdditionalSpecialties();
                //    break;
            }
        }

        /// <summary>
        /// Creates the interface file to be sent to MMIS.  
        /// Includes AP, UP, AM and UM transactions.
        /// </summary>
        public void SubmitProviders()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // set the sFTP variables prior to retrieving file from the MMIS
                string hostName = AppSettings.Get("MMIS-HostName");
                string remotePath = AppSettings.Get("MMIS-SendRemotePath"); // + AppSettings.Get("MMIS-StandardExtract");
                string userName = AppSettings.Get("MMIS-UserName");
                string password = AppSettings.Get("MMIS-Pwd");
                string localPath = AppSettings.Get("MMIS-SubmitProviderLocalPath");
                string fileName = AppSettings.Get("MMIS-SubmitProviderFile");
                string dtmWildcard = AppSettings.Get("MMIS-SubmitProviderFileWildcardDTM");
                string hostKey = AppSettings.Get("MMIS-sFTPHostKey");
                
                fileName = fileName.Replace(dtmWildcard, DateTime.Now.ToString(dtmWildcard));

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    localPath = @"C:\PDMSMMISInterfaceFiles\";
                }

                // Create local path if it does not exist and append filename
                Directory.CreateDirectory(localPath);
                DirectoryInfo downloadDirectory = new DirectoryInfo(localPath);
                string fullFileName = localPath + fileName;
                
                // Create the fixed file engine and set to local object
                FileHelpers.FixedFileEngine engine = new FileHelpers.FixedFileEngine(typeof(SubmitProvider));
                if (!File.Exists(fullFileName))
                {
                    FileStream fs = File.Create(fullFileName);
                    fs.Close();
                }

                // create and write the provider information to the interface file.
                SubmitGroupProviders(engine, fullFileName, Constants.TransactionType.RequestMedicaidIDfromMMIS);
                SubmitGroupProviders(engine, fullFileName, Constants.TransactionType.SendProviderUpdatestoMMIS);
                SubmitIndividualProviders(engine, fullFileName);
                SubmitGroupAffiliation(engine, fullFileName);
                SubmitMCOAffiliation(engine, fullFileName);

                // Always send file to MMIS even though its empty
                // if testing is enabled
                if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())                        
                {
                    //  send the file to MMIS
                }
                else
                {
                    // no action
                }
                // record all bad errors
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        public void SubmitGroupAffiliation()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                string localPath = AppSettings.Get("MMIS-SubmitGroupAffiliationLocalPath");
                string fileName = AppSettings.Get("MMIS-SubmitGroupAffiliationFile");
                string dtmWildcard = AppSettings.Get("MMIS-SubmitGroupAffiliationFileWildcardDTM");
                
                fileName = fileName.Replace(dtmWildcard, DateTime.Now.ToString(dtmWildcard));

                // Create local path if it does not exist and append filename
                Directory.CreateDirectory(localPath);
                DirectoryInfo downloadDirectory = new DirectoryInfo(localPath);
                string fullFileName = localPath + fileName;
                
                // Create the fixed file engine and set to local object
                FileHelpers.FixedFileEngine engine = new FileHelpers.FixedFileEngine(typeof(SubmitGroupAffiliation));
                if (!File.Exists(fullFileName))
                {
                    File.Create(fullFileName);
                }

                SubmitGroupAffiliation(engine, fullFileName);

                // Always send file to MMIS even though its empty
                // if testing is enabled
                if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())                        
                {
                    // do nothing for now
                }
                {
                    // no action
                }
                
                // record all bad errors
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }


        public void SubmitAdditionalSpecialties()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                //Append to the Submit Provider File - no distinct file for the additional specialties
                string localPath = AppSettings.Get("MMIS-SubmitProviderLocalPath");
                string fileName = AppSettings.Get("MMIS-SubmitProviderFile");
                string dtmWildcard = AppSettings.Get("MMIS-SubmitProviderFileWildcardDTM");

                fileName = fileName.Replace(dtmWildcard, DateTime.Now.ToString(dtmWildcard));

                // Create local path if it does not exist and append filename
                Directory.CreateDirectory(localPath);
                DirectoryInfo downloadDirectory = new DirectoryInfo(localPath);
                string fullFileName = localPath + fileName;

                // Create the fixed file engine and set to local object
                FileHelpers.DelimitedFileEngine engine = new FileHelpers.DelimitedFileEngine(typeof(SubmitProvider));
                if (!File.Exists(fullFileName))
                {
                    File.Create(fullFileName);
                }
                SubmitAdditionalSpecialties(engine, fullFileName, log);

                // if testing is enabled
                if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                {
                    // do nothing for now
                }
                {
                    // no action
                }

                // record all bad errors
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                    }
                }

                // log entry
                log.CreateLogEntry(Constants.LogString.SubmittingMMISRecordsEnd);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        /// <summary>
        /// Reads the MMIS response file and updates the provider data
        /// accordingly.
        /// </summary>
        public void RetrieveProviders()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            DateTime now = DateTime.Now;

            try
            {
                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart));

                // retrieve and set required variables
                string localPath = AppSettings.Get("MMIS-ReturnProviderLocalPath");
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    localPath = @"C:\PDMSMMISInterfaceFiles\Incoming\";
                }
                string fileName = AppSettings.Get("MMIS-ReturnProviderFile");
                string dtmWildcard = AppSettings.Get("MMIS-ReturnProviderFileWildcardDTM");
                string archiveFolder = localPath + AppSettings.Get("MMIS-ArchinveFolderName");
                DirectoryInfo localDirectory;
                string testingEnabled = AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower();
                // Get file name
                fileName = fileName.Replace(dtmWildcard, now.ToString(dtmWildcard));
                
                // create local path if it does not exist
                Directory.CreateDirectory(localPath);
                localDirectory = new DirectoryInfo(localPath);

                // create archive path if it does not exist
                if (!Directory.Exists(archiveFolder))
                {
                    Directory.CreateDirectory(archiveFolder);
                }
    
                // if testing enabled
                if (testingEnabled == bool.TrueString.ToLower())
                {
                    this.UpdateProvider(null, log);
//                    this.UpdateGroup(null, log, Constants.TransactionType.SendCBSAUpdateToMMIS);
//                    this.UpdateGroup(null, log, Constants.TransactionType.SendNPIUpdateToMMIS);
                    this.UpdateGroup(null, log, Constants.TransactionType.RequestMedicaidIDfromMMIS);
                    this.UpdateGroup(null, log, Constants.TransactionType.SendProviderUpdatestoMMIS);
                    this.UpdateGroupAffiliation(null, log);
                    this.UpdateMCOAffiliation(null, log);
//                    this.UpdateAdditionalSpecialties(null, log, Constants.TransactionType.SendSpecialty2ToMMIS);
                }
                else
                {
                    foreach (FileInfo file in localDirectory.GetFiles().OrderBy(f => f.LastWriteTime))
                    {
                        this.LoadMMISResponseFile(log, file);
                        File.Move(file.FullName, archiveFolder + "\\" + file.Name);
     //                   CreateSubmitProviderResultsReport(DateTime.Now, Path.Combine(localPath, "Rpt_" + fileName.Substring(0,fileName.IndexOf(".txt")) + ".pdf"),"MMISInterfaceDetailReport.rdlc");
                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);
        }

        public void RetrieveGroupAffiliation()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            DateTime now = DateTime.Now;

            try
            {
                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart));

                // retrieve and set required variables
                // retrieve and set required variables
                string fileName = AppSettings.Get("MMIS-ReturnGroupAffiliationFile");
                string dtmWildcard = AppSettings.Get("MMIS-ReturnGroupAffiliationFileWildcardDTM");
                
                // Get file name
                fileName = fileName.Replace(dtmWildcard, now.ToString(dtmWildcard));

                string testingEnabled = AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower();

                // if testing enabled
                if (testingEnabled == bool.TrueString.ToLower())
                {
                    this.UpdateGroupAffiliation(null, log);
                }
                else
                {
                    string localPath = AppSettings.Get("MMIS-ReturnGroupAffiliationLocalPath");
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        localPath = @"C:\PDMSMMISInterfaceFiles\Incoming\";
                    }
                    string archiveFolder = localPath + AppSettings.Get("MMIS-ArchinveFolderName");

                    // create local path if it does not exist
                    Directory.CreateDirectory(localPath);
                    DirectoryInfo localDirectory = new DirectoryInfo(localPath);

                    // create archive path if it does not exist
                    if (!Directory.Exists(archiveFolder))
                    {
                        Directory.CreateDirectory(archiveFolder);
                    }
    
                    foreach (FileInfo file in localDirectory.GetFiles().OrderBy(f => f.LastWriteTime))
                    {
                        this.LoadMMISGroupAffiliationResponseFile(log, file);
                        File.Move(file.FullName, archiveFolder + "\\" + file.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);
        }

#endregion "Public Methods"

        #region "Private Methods"
        /// <summary>
        /// Creates the interface file lines for group providers.
        /// </summary>
        /// <param name="engine">FileHelper object to create the lines</param>
        /// <param name="fullFileName">path to the file to create</param>
        /// <param name="transactionType">type of transaction, 1 = "AP", 2 = "UP" </param>
        private void SubmitGroupProviders(FileHelpers.FixedFileEngine engine, string fullFileName, int transactionType)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            DateTime now = DateTime.Now;
            string groupPk = string.Empty;
            
            try
            {
                // Populate the staging tables
                this.PopulateGroupStagingData(transactionType);
                    
                // get the staging records
                MMISShared ms = new MMISShared(this.ThreadId);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());

                // get the staging records
                DataSet groupDs = ms.GetUnsubmittedStagingRecords(transactionType);
        
                // Log entry
                log.CreateLogEntry(String.Format(Constants.LogString.SubmittingMMISGroupsStart, groupDs.Tables[MMISShared.dbtGrps].Rows.Count, transactionType));
                // loop over records in dataset
                foreach (DataRow groupRow in groupDs.Tables[0].Rows)
                {
                    try
                    {
                        // get the group primary key reference
                        groupPk = Methods.GetStringValue(groupRow[MMISShared.groupStagingPk].ToString());
                        // create the object to submit
                        SubmitProvider group = new SubmitProvider();
                        int tqId = DataExchangeHelper.GetInt("TRANSACTION_QUEUE_ID", groupRow);
                        int transType = DataExchangeHelper.GetInt("TRANSACTION_TYPE_ID", groupRow);
                        group.max_trans_id = tqId.ToString("0000000000");
                        int partyID = DataExchangeHelper.GetInt("PARTY_ID", groupRow);
                        group.max_pdms_id = partyID.ToString("0000000000").ToUpper();
                        group.max_action = DataExchangeHelper.GetString("ACTION", groupRow);
                        group.id = DataExchangeHelper.GetString("MEDICAID_ID", groupRow);
                        group.tot_bed_num = DataExchangeHelper.GetInt("NUMBER_OF_BEDS", groupRow).ToString("0000");
                        int providerCategoryTypeID = MMISShared.GetProviderCategoryTypeID(partyID);
                        group.indiv_grcd = providerCategoryTypeID == 2 ? "G" : "I";
                        group.nam_org_ind = providerCategoryTypeID == 1 ? "N" : "Y";
                        group.nam = group.last_nam = DataExchangeHelper.GetString("NAME", groupRow);
                        group.fst_nam = "";
                        group.mi_nam = "";
                        group.npi_num = DataExchangeHelper.GetString("NPI", groupRow);
                        group.atyind = "N";
                        group.dba_nam = group.dba_last_nam = DataExchangeHelper.GetString("DBA", groupRow);
                        group.dba_org_ind = group.dba_nam.Trim().Length > 0 ? "Y" : "N";
                        group.dea_eff_dt = DataExchangeHelper.GetDateTime("DEA_EFFECTIVE_DATE", groupRow);
                        group.dea_exdt = SetEndDate(group.dea_eff_dt, DataExchangeHelper.GetDateTime("DEA_TERM_DATE", groupRow));
                        group.dea_num = DataExchangeHelper.GetString("DEA_NUMBER", groupRow);
                        DateTime fiscalYearEndDate = DataExchangeHelper.GetDateTime("FISCAL_YEAR_END", groupRow);
                        group.faci_fy_mo_num = (fiscalYearEndDate.Year == 1753) ? "12" : GetFYBeginMonthFromEndMonth(fiscalYearEndDate.Month).ToString("00");
                        group.ra_sort_seq_cd = "1";
                        group.ra_prt_suscd = "A";
                        group.max_member_pdms_id = "0000000000";
                        DataRow dr = MMISShared.GetPracticeAndOwnershipCodes(partyID);
                        if (dr != null)
                        {
                            group.pract_ty_cd = DataExchangeHelper.GetString("PRACTICE_TYPE_CODE", dr);
                            group.owner_ty_cd = DataExchangeHelper.GetString("TAX_ENTITY_TYPE_CODE", dr);
                            if (group.owner_ty_cd.Trim().Length == 0)
                            {
                                group.owner_ty_cd = "O";
                            }
                        }
                        else
                        {
                            group.owner_ty_cd = "O";
                        }
                        group.prof_tech_ind = "P";
                        DataRow drMCare = MMISShared.GetMedicareInfo(partyID);
                        if (drMCare != null)
                        {
                            group.mcare_end_dt = DataExchangeHelper.GetDateTime("MEDICAREEndDate", drMCare);
                            if (group.mcare_end_dt == null || group.mcare_end_dt.Value.Year == 1753 || (group.mcare_end_dt != null && group.mcare_end_dt.HasValue && group.mcare_end_dt.Value > DateTime.Now))
                            {
                                group.mcare_num = DataExchangeHelper.GetString("MEDICARENumber", drMCare).PadRight(10);
                                group.mcare_beg_dt = DataExchangeHelper.GetDateTime("MEDICAREEffDate", drMCare);
                                group.mcare_end_dt = SetEndDate(group.mcare_beg_dt, group.mcare_end_dt);
                                group.mcare_ind = "Y";
                            }
                            else
                            {
                                group.mcare_num = "";
                                group.mcare_beg_dt = null;
                                group.mcare_end_dt = null;
                                group.mcare_ind = "N";
                            }
                        }
                        else
                        {
                            group.mcare_num = "";
                            group.mcare_beg_dt = null;
                            group.mcare_end_dt = null;
                            group.mcare_ind = "N";
                        }
                        string rawProfitStatus = DataExchangeHelper.GetString("PROFIT_STATUS", groupRow);
                        group.profit_ind = (group.owner_ty_cd == "G" || group.owner_ty_cd == "N") ? "N" : "Y";
                        group.bkuwhold_ind = "W";
                        group.sec_bnote_yr_num = "0000";
                        DateTime tempAppDate = MMISShared.GetLatestApplicationDate(partyID);

                        //Only send P-APPL-DT field on a Provider Add transaction
                        if (group.max_action.Trim() == "AP")
                            group.appl_dt = tempAppDate.Year == 1753 ? "" : tempAppDate.ToString("yyyy-MM-dd");
                        
                        DataRow officeRow = MMISShared.GetRegOfficeInfo(partyID);
                        group.qstnr_npp_ind = DataExchangeHelper.GetBool("OFFICE_NEWPATIENT", officeRow) ? "Y" : "N";
                        group.qstnr_npr_ind = DataExchangeHelper.GetBool("OFFICE_REFFERAL", officeRow) ? "Y" : "N";

                        if (group.max_action.Trim() == "AP")
                        {
                            DataRow cliaRow = MMISShared.GetCLIAInfo(partyID);
                            if (cliaRow != null)
                            {
                                group.clia_num = DataExchangeHelper.GetString("CLIANumber", cliaRow);
                                group.clia_cert_ty_cd = "1";
                                group.clia_cert_eff_dt = DataExchangeHelper.GetDateTime("CLIAEffDate", cliaRow);
                                group.clia_cert_expir_dt = SetEndDate(group.clia_cert_eff_dt, DataExchangeHelper.GetDateTime("CLIAEndDate", cliaRow));
                            }
                        }

                        // find the tax id for the provider
                        DataRow taxIDRow = MMISShared.GetTaxIDInfo(partyID);
                        if (taxIDRow != null)
                        {
                            // get the type of tax id
                            int taxIDTypeID = DataExchangeHelper.GetInt("TAX_ID_TYPE_ID", taxIDRow);
                            int altTaxIDTypeID = DataExchangeHelper.GetInt("ALT_TAX_ID_TYPE_ID", taxIDRow);
                            string taxID = DataExchangeHelper.GetString("TAX_ID", taxIDRow).Trim();
                            string altTaxID = DataExchangeHelper.GetString("ALT_TAX_ID", taxIDRow).Trim();

                            // set the tax ID's for an SSN
                            if (taxIDTypeID == 15)
                            {
                                group.ssn_num = group.ssn_num_1 = taxID;
                            }
                            else if (altTaxIDTypeID == 15)
                            {
                                group.ssn_num = group.ssn_num_1 = altTaxID;
                            }
                            else
                            {
                                group.ssn_num = group.ssn_num_1 = "";
                            }

                            // set the tax ID's for an FEIN
                            if (taxIDTypeID == 16)
                            {
                                group.fed_tax_id_1 = group.billing_tax_key_id = taxID;
                            }
                            else if (altTaxIDTypeID == 16)
                            {
                                group.fed_tax_id_1 = group.billing_tax_key_id = altTaxID;
                            }
                            else
                            {
                                group.fed_tax_id_1 = "";
                                group.billing_tax_key_id = taxID.Trim().Length > 0 ? taxID : altTaxID;
                            }

                            // set the tax ID type
                            if (taxID.Length > 0 && altTaxID.Length > 0 && taxIDTypeID != altTaxIDTypeID)
                            {
                                group.fed_id_ind_1 = "B";
                            }
                            else if (taxIDTypeID == 16 || altTaxIDTypeID == 16)
                            {
                                group.fed_id_ind_1 = "F";
                            }
                            else
                            {
                                group.fed_id_ind_1 = "S";
                            }

                            if (group.fed_id_ind_1 == "S")
                            {
                                group.tax_key_id = group.ssn_num;
                            }
                            else
                            {
                                group.tax_key_id = group.fed_tax_id_1;
                            }
                        }

                        string suffixName =""; // keeping this to be consistent with the indiv provider submit code
                        DataRow drAddlInfo = null;
                        string addressFirstName = "";
                        string addressMiddleName = "";
                        string addressLastName = "";
                        string addressTitle = "";
                        string addrType = "";
                        string addressOrgName = "";
                        string addressPhone = "";
                        //// populate addresses
                        foreach (DataRow address in groupRow.GetChildRows(MMISShared.dbrGrps2Addrs))
                        {
                            // get the user's address info from the database.
                            string addressType = address["ADDRESS_TYPE"].ToString().Trim().ToUpper();
                            string countyName = Methods.GetStringValue(address["COUNTY"], false).ToString();
                            string groupName = Methods.GetStringValue(address["NAME"], false).ToString().ToUpper();
                            string groupLastName = Methods.GetStringValue(address["LAST_NAME"], false).ToString().ToUpper();
                            string groupFirstName = Methods.GetStringValue(address["FIRST_NAME"], false).ToString().ToUpper();
                            string groupMIName = Methods.GetStringValue(address["MI_NAME"], false).ToString().ToUpper();
                            string street1 = Methods.GetStringValue(address["STREET1"], false).ToString().ToUpper();
                            string street2 = Methods.GetStringValue(address["STREET2"], false).ToString().ToUpper();
                            string street3 = Methods.GetStringValue(address["STREET3"], false).ToString().ToUpper();
                            string city = Methods.GetStringValue(address["CITY"], false).ToString().ToUpper();
                            string state = Methods.GetStringValue(address["STATE"], false).ToString().ToUpper();
                            string zip = Methods.GetStringValue(address["ZIP5"], false).ToString();
                            string zipFour = Methods.GetStringValue(address["ZIP4"], false).ToString();
                            if (zipFour.Trim().Length == 0)
                            {
                                zipFour = "0000";
                            }
                            string phone = Methods.GetStringValue(address["PHONE"], false).ToString();
                            string phoneExt = Methods.GetStringValue(address["PHONE_EXT"], false).ToString();
                            string fax = Methods.GetStringValue(address["FAX"], false).ToString();
                            string email = Methods.GetStringValue(address["EMAIL"], false).ToString().ToUpper();
                            string countyCode = MMISShared.GetCountyCode(countyName, state);
                            string quadWard = MMISShared.GetQuadWard(partyID, addressType);

                            drAddlInfo = MMISShared.GetAdditionalAddressInfo(partyID, (Enumerations.ContactMechanismRoleTypeId)MMISShared.ConvertAddressTypeCodeToID(addressType));
                            addressFirstName = "";
                            addressMiddleName = "";
                            addressLastName = "";
                            addressTitle = "";
                            addrType = "";
                            addressOrgName = "";
                            addressPhone = "";
                            if (drAddlInfo != null)
                            {
                                addressPhone = DataExchangeHelper.GetString("PHONE", drAddlInfo).Trim();
                                if (addressType != "S")
                                {
                                    addressFirstName = DataExchangeHelper.GetString("FIRST_NAME", drAddlInfo).Trim();
                                    addressMiddleName = DataExchangeHelper.GetString("MIDDLE_NAME", drAddlInfo).Trim();
                                    addressLastName = DataExchangeHelper.GetString("LAST_NAME", drAddlInfo).Trim();
                                    addressTitle = DataExchangeHelper.GetString("TITLE", drAddlInfo).Trim();
                                    addrType = DataExchangeHelper.GetString("ADDRESS_TYPE", drAddlInfo).Trim();
                                    addressOrgName = DataExchangeHelper.GetString("ORG_NAME", drAddlInfo).Trim();
                                }
                            }

                            if (state.Trim().Length > 0)
                            {
                                // fill the address info in according to the address type
                                switch (addressType)
                                {
                                    case "M":  // mailing address
                                        group.g_mail_to_quad_cd = quadWard.Substring(0, 2);
                                        group.g_mail_to_ward_cd = quadWard.Substring(2);
                                        group.mail_contct_nam = MMISShared.GetMailToContactName(partyID);
                                        if (addrType.StartsWith("O"))
                                        {
                                            group.mail_to_nam_org_ind = "Y";
                                            group.mail_to_last_nam = group.mail_to_nam = addressOrgName;
                                            group.mail_to_fst_nam = "";
                                            group.mail_to_mi_nam = "";
                                            group.mail_to_sfx_nam = "";
                                        }
                                        else
                                        {
                                            group.mail_to_nam_org_ind = "N";
                                            group.mail_to_nam = DataExchangeHelper.FormatName(addressFirstName, addressMiddleName, addressLastName, addressTitle);
                                            group.mail_to_last_nam = addressLastName;
                                            group.mail_to_fst_nam = addressFirstName;
                                            group.mail_to_mi_nam = addressMiddleName;
                                            group.mail_to_sfx_nam = addressTitle;
                                        }

                                        group.mail_contct_email_ad = email;
                                        group.mail_contct_phon_num = phone;
                                        group.mail_fax_num = fax;
                                        group.mail_phon_num = addressPhone;
                                        group.mail_to_city_nam = city;
                                        group.mail_to_line1_ad = street1;
                                        group.mail_to_line2_ad = street2;
                                        group.mail_to_st_cd = state;
                                        group.mail_to_zip5_cd = zip;
                                        group.mail_to_zip4_cd = zipFour;
                                        group.mail_to_cnty_cd = countyCode;
                                        break;

                                    case "S":  // servicing address
                                        group.servicing_nam_org_ind = group.nam_org_ind;
                                        group.g_servicing_quad_cd = quadWard.Substring(0, 2);
                                        group.g_servicing_ward_cd = quadWard.Substring(2);
                                        group.servicing_contct_email_ad = email;
                                        group.servicing_contct_nam = MMISShared.GetServicingContactName(partyID);
                                        group.servicing_nam = group.nam;
                                        group.servicing_last_nam = group.last_nam;
                                        group.servicing_fst_nam = group.fst_nam;
                                        group.servicing_mi_nam = group.mi_nam;
                                        group.servicing_sfx_nam = suffixName;
                                        group.servicing_contct_phon_num = phone;
                                        group.servicing_fax_num = fax;
                                        group.servicing_phon_num = addressPhone;
                                        group.servicing_contct_email_ad = email;
                                        group.servicing_line1_ad = street1;
                                        group.servicing_line2_ad = street2;
                                        group.servicing_city_nam = city;
                                        group.servicing_st_cd = state;
                                        group.servicing_zip5_cd = zip;
                                        group.servicing_zip4_cd = zipFour;
                                        group.servicing_cnty_cd = countyCode;
                                        group.locn_cd = (state == "DC") ? "I" : "O";
                                        break;

                                    case "C":
                                        break;

                                    case "P": // billing address
                                        group.g_billing_quad_cd = quadWard.Substring(0, 2);
                                        group.g_billing_ward_cd = quadWard.Substring(2);
                                        group.billing_contct_email_ad = email;
                                        group.billing_contct_nam = MMISShared.GetBillingContactName(partyID);
                                        if (addrType.StartsWith("O"))
                                        {
                                            group.billing_nam_org_ind = "Y";
                                            group.billing_last_nam = group.billing_nam = addressOrgName;
                                            group.billing_fst_nam = "";
                                            group.billing_mi_nam = "";
                                            group.billing_sfx_nam = "";
                                        }
                                        else
                                        {
                                            group.billing_nam_org_ind = "N";
                                            group.billing_nam = DataExchangeHelper.FormatName(addressFirstName, addressMiddleName, addressLastName, addressTitle);
                                            group.billing_last_nam = addressLastName;
                                            group.billing_fst_nam = addressFirstName;
                                            group.billing_mi_nam = addressMiddleName;
                                            group.billing_sfx_nam = addressTitle;
                                        }
                                        group.billing_contct_phon_num = phone;
                                        group.billing_fax_num = fax;
                                        group.billing_phon_num = addressPhone;
                                        group.billing_line1_ad = street1;
                                        group.billing_line2_ad = street2;
                                        group.billing_city_nam = city;
                                        group.billing_st_cd = state;
                                        group.billing_zip5_cd = zip;
                                        group.billing_zip4_cd = zipFour;
                                        group.billing_cnty_cd = countyCode;
                                        break;
                                }
                            }
                        }

                        // get the remittance advice address info and fill it in
                        DataRow drRAAddr = MMISShared.GetRegAddress(partyID, "R");
                        drAddlInfo = MMISShared.GetAdditionalAddressInfo(partyID, Enumerations.ContactMechanismRoleTypeId.Remittance);
                        if (drRAAddr != null && drAddlInfo != null)
                        {
                            group.ra_mailing_st_cd = DataExchangeHelper.GetString("STATE", drRAAddr).ToUpper();
                            // if the state is filled in, then assume the address is present...
                            if (group.ra_mailing_st_cd.Trim().Length > 0)
                            {
                                addressPhone = DataExchangeHelper.GetString("PHONE", drAddlInfo).Trim();
                                addressFirstName = DataExchangeHelper.GetString("FIRST_NAME", drAddlInfo).Trim();
                                addressMiddleName = DataExchangeHelper.GetString("MIDDLE_NAME", drAddlInfo).Trim();
                                addressLastName = DataExchangeHelper.GetString("LAST_NAME", drAddlInfo).Trim();
                                addressTitle = DataExchangeHelper.GetString("TITLE", drAddlInfo).Trim();
                                addrType = DataExchangeHelper.GetString("ADDRESS_TYPE", drAddlInfo).Trim();
                                addressOrgName = DataExchangeHelper.GetString("ORG_NAME", drAddlInfo).Trim();

                                string cntyName = DataExchangeHelper.GetString("COUNTY", drRAAddr);
                                group.ra_mailing_contct_nam = DataExchangeHelper.GetString("CONTACT_NAME", drRAAddr).ToUpper();
                                group.ra_mailing_contct_phon_num = DataExchangeHelper.GetString("PHONE", drRAAddr);
                                if (addrType.StartsWith("O"))
                                {
                                    group.ra_mailing_nam_org_ind = "Y";
                                    group.ra_mailing_last_nam = group.ra_mailing_nam = addressOrgName;
                                    group.ra_mailing_fst_nam = "";
                                    group.ra_mailing_mi_nam = "";
                                    group.ra_mailing_sfx_nam = "";
                                }
                                else
                                {
                                    group.ra_mailing_nam_org_ind = "N";
                                    group.ra_mailing_nam = DataExchangeHelper.FormatName(addressFirstName, addressMiddleName, addressLastName, addressTitle);
                                    group.ra_mailing_last_nam = addressLastName;
                                    group.ra_mailing_fst_nam = addressFirstName;
                                    group.ra_mailing_mi_nam = addressMiddleName;
                                    group.ra_mailing_sfx_nam = addressTitle;
                                }
                                group.ra_mailing_line1_ad = DataExchangeHelper.GetString("STREET1", drRAAddr).ToUpper();
                                group.ra_mailing_line2_ad = DataExchangeHelper.GetString("STREET2", drRAAddr).ToUpper();
                                group.ra_mailing_city_nam = DataExchangeHelper.GetString("CITY", drRAAddr).ToUpper();
                                group.ra_mailing_st_cd = DataExchangeHelper.GetString("STATE", drRAAddr).ToUpper();
                                group.ra_mailing_zip5_cd = DataExchangeHelper.GetString("ZIP5", drRAAddr);
                                group.ra_mailing_zip4_cd = DataExchangeHelper.GetString("ZIP4", drRAAddr);
                                if (group.ra_mailing_zip4_cd.Trim().Length == 0)
                                {
                                    group.ra_mailing_zip4_cd = "0000";
                                }
                                group.ra_mailing_phon_num = addressPhone;
                                group.ra_mailing_fax_num = DataExchangeHelper.GetString("FAX", drRAAddr);
                                group.ra_mailing_contct_email_ad = DataExchangeHelper.GetString("EMAIL", drRAAddr).ToUpper();
                                group.ra_mailing_cnty_cd = MMISShared.GetCountyCode(cntyName, group.ra_mailing_st_cd);
                                string quWa = MMISShared.GetQuadWard(partyID, "R");
                                group.g_ra_mailing_quad_cd = quWa.Substring(0, 2);
                                group.g_ra_mailing_ward_cd = quWa.Substring(2);
                            }
                        }

                        // pull the other address information from the database and fill it in
                        DataRow drOtherAddr = MMISShared.GetRegAddress(partyID, "O");
                        drAddlInfo = MMISShared.GetAdditionalAddressInfo(partyID, Enumerations.ContactMechanismRoleTypeId.Other);
                        if (drOtherAddr != null && drAddlInfo != null)
                        {
                            group.other_st_cd = DataExchangeHelper.GetString("STATE", drOtherAddr).ToUpper();
                            if (group.other_st_cd.Trim().Length > 0)
                            {
                                addressPhone = DataExchangeHelper.GetString("PHONE", drAddlInfo).Trim();
                                addressFirstName = DataExchangeHelper.GetString("FIRST_NAME", drAddlInfo).Trim();
                                addressMiddleName = DataExchangeHelper.GetString("MIDDLE_NAME", drAddlInfo).Trim();
                                addressLastName = DataExchangeHelper.GetString("LAST_NAME", drAddlInfo).Trim();
                                addressTitle = DataExchangeHelper.GetString("TITLE", drAddlInfo).Trim();
                                addrType = DataExchangeHelper.GetString("ADDRESS_TYPE", drAddlInfo).Trim();
                                addressOrgName = DataExchangeHelper.GetString("ORG_NAME", drAddlInfo).Trim();

                                string cntyName = DataExchangeHelper.GetString("COUNTY", drOtherAddr);
                                group.other_nam = DataExchangeHelper.GetString("NAME", drOtherAddr).ToUpper();
                                group.other_contct_nam = DataExchangeHelper.GetString("CONTACT_NAME", drOtherAddr).ToUpper();
                                group.other_contct_phon_num = DataExchangeHelper.GetString("PHONE", drOtherAddr);
                                if (addrType.StartsWith("O"))
                                {
                                    group.other_nam_org_ind = "Y";
                                    group.other_last_nam = group.other_nam = addressOrgName;
                                    group.other_fst_nam = "";
                                    group.other_mi_nam = "";
                                    group.other_sfx_nam = "";
                                }
                                else
                                {
                                    group.other_nam_org_ind = "N";
                                    group.other_nam = DataExchangeHelper.FormatName(addressFirstName, addressMiddleName, addressLastName, addressTitle);
                                    group.other_last_nam = addressLastName;
                                    group.other_fst_nam = addressFirstName;
                                    group.other_mi_nam = addressMiddleName;
                                    group.other_sfx_nam = addressTitle;
                                }
                                group.other_line1_ad = DataExchangeHelper.GetString("STREET1", drOtherAddr).ToUpper();
                                group.other_line2_ad = DataExchangeHelper.GetString("STREET2", drOtherAddr).ToUpper();
                                group.other_city_nam = DataExchangeHelper.GetString("CITY", drOtherAddr).ToUpper();
                                group.other_zip5_cd = DataExchangeHelper.GetString("ZIP5", drOtherAddr);
                                group.other_zip4_cd = DataExchangeHelper.GetString("ZIP4", drOtherAddr);
                                if (group.other_zip4_cd.Trim().Length == 0)
                                {
                                    group.other_zip4_cd = "0000";
                                }
                                group.other_phon_num = addressPhone;
                                group.other_fax_num = DataExchangeHelper.GetString("FAX", drOtherAddr);
                                group.other_contct_email_ad = DataExchangeHelper.GetString("EMAIL", drOtherAddr).ToUpper();
                                group.other_cnty_cd = MMISShared.GetCountyCode(cntyName, group.other_st_cd);
                                string quWa = MMISShared.GetQuadWard(partyID, "O");
                                group.g_other_quad_cd = quWa.Substring(0, 2);
                                group.g_other_ward_cd = quWa.Substring(2);
                            }
                        }

                        // pull the W9 form address information from the database and fill it in
                        DataRow drW9Addr = MMISShared.GetRegAddress(partyID, "W");
                        drAddlInfo = MMISShared.GetAdditionalAddressInfo(partyID, Enumerations.ContactMechanismRoleTypeId.W9);
                        if (drW9Addr != null && drAddlInfo != null)
                        {
                            group.w9_1099_st_cd = DataExchangeHelper.GetString("STATE", drW9Addr).ToUpper();
                            if (group.w9_1099_st_cd.Trim().Length > 0)
                            {
                                addressPhone = DataExchangeHelper.GetString("PHONE", drAddlInfo).Trim();
                                addressFirstName = DataExchangeHelper.GetString("FIRST_NAME", drAddlInfo).Trim();
                                addressMiddleName = DataExchangeHelper.GetString("MIDDLE_NAME", drAddlInfo).Trim();
                                addressLastName = DataExchangeHelper.GetString("LAST_NAME", drAddlInfo).Trim();
                                addressTitle = DataExchangeHelper.GetString("TITLE", drAddlInfo).Trim();
                                addrType = DataExchangeHelper.GetString("ADDRESS_TYPE", drAddlInfo).Trim();
                                addressOrgName = DataExchangeHelper.GetString("ORG_NAME", drAddlInfo).Trim();

                                string cntyName = DataExchangeHelper.GetString("COUNTY", drW9Addr);
                                group.w9_1099_contct_nam = DataExchangeHelper.GetString("CONTACT_NAME", drW9Addr);
                                group.w9_1099_contct_phon_num = DataExchangeHelper.GetString("PHONE", drW9Addr);
                                group.w9_1099_contct_email_ad = DataExchangeHelper.GetString("EMAIL", drW9Addr);
                                if (addrType.StartsWith("O"))
                                {
                                    group.w9_1099_nam_org_ind = "Y";
                                    group.w9_1099_last_nam = group.w9_1099_nam = addressOrgName;
                                    group.w9_1099_fst_nam = "";
                                    group.w9_1099_mi_nam = "";
                                    group.w9_1099_sfx_nam = "";
                                }
                                else
                                {
                                    group.w9_1099_nam_org_ind = "N";
                                    group.w9_1099_nam = DataExchangeHelper.FormatName(addressFirstName, addressMiddleName, addressLastName, addressTitle);
                                    group.w9_1099_last_nam = addressLastName;
                                    group.w9_1099_fst_nam = addressFirstName;
                                    group.w9_1099_mi_nam = addressMiddleName;
                                    group.w9_1099_sfx_nam = addressTitle;
                                }
                                group.w9_1099_line1_ad = DataExchangeHelper.GetString("STREET1", drW9Addr).ToUpper();
                                group.w9_1099_line2_ad = DataExchangeHelper.GetString("STREET2", drW9Addr).ToUpper();
                                group.w9_1099_city_nam = DataExchangeHelper.GetString("CITY", drW9Addr).ToUpper();
                                group.w9_1099_zip5_cd = DataExchangeHelper.GetString("ZIP5", drW9Addr);
                                group.w9_1099_zip4_cd = DataExchangeHelper.GetString("ZIP4", drW9Addr);
                                if (group.w9_1099_zip4_cd.Trim().Length == 0)
                                {
                                    group.w9_1099_zip4_cd = "0000";
                                }
                                group.w9_1099_phon_num = addressPhone;
                                group.w9_1099_fax_num = DataExchangeHelper.GetString("FAX", drW9Addr);
                                group.w9_1099_cnty_cd = MMISShared.GetCountyCode(cntyName, group.other_st_cd);
                                string quWa = MMISShared.GetQuadWard(partyID, "W");
                                group.g_w9_1099_quad_cd = quWa.Substring(0, 2);
                                group.g_w9_1099_ward_cd = quWa.Substring(2);
                            }
                        }

                        DateTime earliestEnrollmentDate = new DateTime(9999, 12, 31);

                        // add the enrollment info 
                        DataRowCollection enrollInfo = MMISShared.GetEnrollmentInfo(partyID);
                        if (enrollInfo != null && enrollInfo.Count > 0)
                        {
                            // fill in the enrollment information
                            group.ProviderStatus[0] = new SubmitProviderStatus();
                            group.ProviderStatus[0].enrol_stat_ty_cd = DataExchangeHelper.GetString("EnrollmentStatusCode", enrollInfo[0]);
                            group.ProviderStatus[0].stat_eff_dt = DataExchangeHelper.GetDateTime("EnrollStartDate", enrollInfo[0]);
                            group.ProviderStatus[0].stat_end_dt = SetEndDate(group.ProviderStatus[0].stat_eff_dt, DataExchangeHelper.GetDateTime("EnrollEndDate", enrollInfo[0]));
                            group.ProviderStatus[0].ty_cd = DataExchangeHelper.GetString("ProviderType", enrollInfo[0]);
                            // if the application type is QMB/Crossover and the provider type is not R01 or R02...
                            if (DataExchangeHelper.GetInt("ApplicationTypeID", enrollInfo[0]) == 6 &&
                                group.ProviderStatus[0].ty_cd != "R01" &&
                                group.ProviderStatus[0].ty_cd != "R02")
                            {
                                group.ProviderStatus[0].ty_cd = "R02";
                            }
                            // set the NABP number, if appropriate
                            group.nabnum = "";
                            if (group.ProviderStatus[0].ty_cd == "H00" ||
                                group.ProviderStatus[0].ty_cd == "H01" ||
                                group.ProviderStatus[0].ty_cd == "H02")
                            {
                                group.nabnum = group.npi_num;
                            }

                            if (group.ProviderStatus[0].stat_eff_dt != null && group.ProviderStatus[0].stat_eff_dt.HasValue && group.ProviderStatus[0].stat_eff_dt < earliestEnrollmentDate)
                            {
                                earliestEnrollmentDate = group.ProviderStatus[0].stat_eff_dt.Value;
                            }

                            // now fill in the rest of the rows
                            // step through each enrollment history record
                            for (int index = 1; index < group.ProviderStatus.Count(); index++)
                            {
                                // create a new enrollment history record.
                                group.ProviderStatus[index] = new SubmitProviderStatus();

                                // if there is history data for this record...
                                if (index < enrollInfo.Count)
                                {
                                    // fill in the history record with the data
                                    DataRow drEH = enrollInfo[index];
                                    group.ProviderStatus[index].enrol_stat_ty_cd = DataExchangeHelper.GetString("EnrollmentStatusCode", drEH);
                                    group.ProviderStatus[index].stat_eff_dt = DataExchangeHelper.GetDateTime("EnrollStartDate", drEH);
                                    group.ProviderStatus[index].stat_end_dt = DataExchangeHelper.GetDateTime("EnrollEndDate", drEH);
                                    group.ProviderStatus[index].ty_cd = group.ProviderStatus[0].ty_cd;
                                    if (group.ProviderStatus[index].stat_eff_dt != null && group.ProviderStatus[index].stat_eff_dt.HasValue && group.ProviderStatus[index].stat_eff_dt < earliestEnrollmentDate)
                                    {
                                        earliestEnrollmentDate = group.ProviderStatus[index].stat_eff_dt.Value;
                                    }
                                }
                                else
                                {
                                    // fill the record with blanks
                                    group.ProviderStatus[index].enrol_stat_ty_cd = "";
                                    group.ProviderStatus[index].stat_eff_dt = new DateTime(1753, 1, 1);
                                    group.ProviderStatus[index].stat_end_dt = new DateTime(1753, 1, 1);
                                    group.ProviderStatus[index].ty_cd = "";
                                }
                            }
                        }

                        // fill the program code and tax id information with the enrollment info
                        // we're using these values as the best we can find
                        group.prog_cd_1 = group.ProviderStatus[0].ty_cd == "H03" ? "A" : "M";
                        group.tax_beg_dt_1 = group.prog_beg_dt_1 = earliestEnrollmentDate;
                        group.prog_end_dt_1 = group.tax_end_dt_1 = new DateTime(9999, 12, 31);

                        // add the provider specialties
                        DataRow[] specRows = groupRow.GetChildRows(MMISShared.dbrProv2Spec).Where(r => r.Field<int>("PARTY_ID") == partyID).ToArray();
                        for (int index = 0; index < group.ProviderSpecialty.Length; index++)
                        {
                            group.ProviderSpecialty[index] = new SubmitProviderSpecialty();
                            if (index < specRows.Count())
                            {
                                DataRow currentSpecialty = specRows[index];
                                group.ProviderSpecialty[index].specl_beg_dt = DataExchangeHelper.GetDateTime("SPEC_BEGIN_DATE", currentSpecialty);
                                group.ProviderSpecialty[index].specl_cd = DataExchangeHelper.GetString("SPEC_CODE", currentSpecialty);
                                group.ProviderSpecialty[index].specl_end_dt = SetEndDate(group.ProviderSpecialty[index].specl_beg_dt, DataExchangeHelper.GetDateTime("SPEC_END_DATE", currentSpecialty));
                                group.ProviderSpecialty[index].lic_brd_num = "";
                            }
                            else
                            {
                                group.ProviderSpecialty[index].specl_beg_dt = new DateTime(1753, 1, 1);
                                group.ProviderSpecialty[index].specl_cd = "";
                                group.ProviderSpecialty[index].specl_end_dt = new DateTime(1753, 1, 1);
                                group.ProviderSpecialty[index].lic_brd_num = "";
                            }
                        }

                        // add the license and cert info
                        DataRow[] licenseRows = groupRow.GetChildRows(MMISShared.dbrProv2LicCert).Where(r => r.Field<int>("PARTY_ID") == partyID).ToArray(); 
                        for (int index = 0; index < group.LicenseCerts.Length; index++)
                        {
                            group.LicenseCerts[index] = new SubmitProviderLicense();
                            if (index < licenseRows.Count())
                            {
                                DataRow currentLic = licenseRows[index];
                                group.LicenseCerts[index].lic_cert_cd = DataExchangeHelper.GetString("LICENSE_TYPE", currentLic);
                                // DCPDMS-2379 - shifts license number to the left when it's greater
                                // than 10 characters.
                                string tempLicenseNumber = DataExchangeHelper.GetString("LICENSE_NUMBER", currentLic).Trim();
                                if (tempLicenseNumber.Trim().Length > 10)
                                {
                                    tempLicenseNumber = tempLicenseNumber.Substring(tempLicenseNumber.Length - 10);
                                }
                                group.LicenseCerts[index].lic_cert_num = tempLicenseNumber;
                                if (!string.IsNullOrEmpty(DataExchangeHelper.GetString("LICENSE_NUMBER", currentLic)) && DataExchangeHelper.GetDateTime("LICENSE_EFF_DATE", currentLic) == Convert.ToDateTime("1753/1/1") && transactionType == Constants.TransactionType.SendProviderUpdatestoMMIS)
                                    group.LicenseCerts[index].lic_eff_dt = "0001-01-01";
                                /*else if (!string.IsNullOrEmpty(group.LicenseCerts[index].lic_eff_dt.ToString()))
                                    group.LicenseCerts[index].lic_eff_dt = DataExchangeHelper.GetDateTime("LICENSE_EFF_DATE", currentLic).ToString();*/
                                else
                                    group.LicenseCerts[index].lic_eff_dt = DataExchangeHelper.GetDateTime("LICENSE_EFF_DATE", currentLic).GetDateTimeFormats()[5];
                                if (group.LicenseCerts[index].lic_eff_dt == "0001-01-01")
                                    group.LicenseCerts[index].lic_expir_dt = DataExchangeHelper.GetDateTime("LICENSE_END_DATE", currentLic);
                                else
                                    group.LicenseCerts[index].lic_expir_dt = SetEndDate(DataExchangeHelper.GetDateTime("LICENSE_EFF_DATE", currentLic), DataExchangeHelper.GetDateTime("LICENSE_END_DATE", currentLic));
                                group.LicenseCerts[index].lic_rstrct_cd = "A";
                                group.LicenseCerts[index].st_cd = DataExchangeHelper.GetString("LICENSE_STATE", currentLic);
                                group.ProviderSpecialty[index].lic_brd_num = DataExchangeHelper.GetString("LICENSE_BOARD_NAME", currentLic);
                            }
                            else
                            {
                                group.LicenseCerts[index].lic_cert_cd = "";
                                group.LicenseCerts[index].lic_cert_num = "";
                                group.LicenseCerts[index].lic_eff_dt = "";
                                group.LicenseCerts[index].lic_expir_dt = new DateTime(1753, 1, 1);
                                group.LicenseCerts[index].lic_rstrct_cd = "";
                                group.LicenseCerts[index].st_cd = "";
                                group.ProviderSpecialty[index].lic_brd_num = "";
                            }
                        }

                        // add the provider category of service
                        DataRow[] catRows = groupRow.GetChildRows(MMISShared.dbrProv2Cat).Where(r => r.Field<int>("PARTY_ID") == partyID).ToArray(); 
                        for (int index = 0; index < group.ProviderCOS.Length; index++)
                        {
                            group.ProviderCOS[index] = new SubmitProviderCOS();
                            if (index < catRows.Count())
                            {
                                DataRow currentCOS = catRows[index];
                                group.ProviderCOS[index].cos_beg_dt = DataExchangeHelper.GetDateTime("COS_EFF_DATE", currentCOS);
                                group.ProviderCOS[index].cos_cd = DataExchangeHelper.GetString("COS_TYPE_CODE", currentCOS).Trim();
                                if (group.ProviderCOS[index].cos_cd.Length == 1)
                                {
                                    group.ProviderCOS[index].cos_cd = "0" + group.ProviderCOS[index].cos_cd;
                                }
                                group.ProviderCOS[index].cos_end_dt = SetEndDate(group.ProviderCOS[index].cos_beg_dt, DataExchangeHelper.GetDateTime("COS_EXPIR_DATE", currentCOS));
                            }
                            else
                            {
                                group.ProviderCOS[index].cos_beg_dt = new DateTime(1753, 1, 1);
                                group.ProviderCOS[index].cos_cd = "";
                                group.ProviderCOS[index].cos_end_dt = new DateTime(1753, 1, 1);
                            }
                        }


                        // add the provider taxonomys
                        DataRow[] taxRows = groupRow.GetChildRows(MMISShared.dbrProv2Tax).Where(r => r.Field<int>("PARTY_ID") == partyID).ToArray(); 
                        for (int index = 0; index < group.ProviderTaxonomy.Length; index++)
                        {
                            group.ProviderTaxonomy[index] = new SubmitProviderTaxonomy();
                            if (index < taxRows.Count())
                            {
                                DataRow currentSpecialty = taxRows[index];
                                group.ProviderTaxonomy[index].taxon_beg_dt = DataExchangeHelper.GetDateTime("TAXONOMY_START_DATE", currentSpecialty);
                                group.ProviderTaxonomy[index].taxonomy_cd = DataExchangeHelper.GetString("TAXONOMY_CODE", currentSpecialty);
                                group.ProviderTaxonomy[index].taxon_end_dt = SetEndDate(group.ProviderTaxonomy[index].taxon_beg_dt, DataExchangeHelper.GetDateTime("TAXONOMY_END_DATE", currentSpecialty));
                            }
                            else
                            {
                                group.ProviderTaxonomy[index].taxon_beg_dt = new DateTime(1753, 1, 1);
                                group.ProviderTaxonomy[index].taxonomy_cd = "";
                                group.ProviderTaxonomy[index].taxon_end_dt = new DateTime(1753, 1, 1);
                            }
                        }

                        //  append the record to the file
                        engine.AppendToFile(fullFileName, group);
                           
                        // provider submitted log
                      //  log.CreateLogEntry(String.Format(Constants.LogString.SubmittingMMISRecord, groupPk, group.pdmsProviderId, tqId));

                        // Update TRANSACTION table with Submit Date information
                        // -----------------------------------------------------
                        TransactionController.UpdateTransactionQueue(
                            tqId
                            , now
                            , null
                            , now
                            , Constants.appPDMSDataExchangeUserId);

                        // generated by sp_Admin_StoredProcBuilder on Sep 12 2013 11:07AM
                        // create parameters objects and fill with values
                        List<SqlParameter> parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_GROUP_PK", DbType.Int32, groupPk, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));
                        parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, "Submitted", true));
                        parameters.Add(SqlParms.CreateParameter("CORRELATION_ID", DbType.String, string.Empty, true));
                    
                        DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingGroup", parameters);

                        /*//check for affiliation start date for the group confirmed affiliates
                        //get all group confirmed affiliates

                            List<SqlParameter> parameters1 = new List<SqlParameter>();
                            parameters1.Add(SqlParms.CreateParameter("partyID", DbType.String, partyID, true));
                            DataAccess.ExecuteStoredProcedure("usp_UpdateAffiliationStartDate", parameters1);*/

                    }
                    catch (Exception ex)
                    {
                        LogErrorRecord(log, "MMIS Submit Exception", MMISShared.dbtGrps, groupPk, ex.Message);
                    }
                }
                log.CreateLogEntry(String.Format(Constants.LogString.SubmittingMMISGroupsEnd, transactionType));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private void SubmitGroupAffiliation(FileHelpers.FixedFileEngine engine, string fullFileName)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string providerPK = string.Empty;
            DateTime now = DateTime.Now;

            try
            {
                // Populate the staging tables
                int transactionType = Constants.TransactionType.SendGroupAffiliationstoMMIS;
                this.PopulateGroupStagingData(transactionType);

                // get the staging records
                MMISShared ms = new MMISShared(this.ThreadId);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());

                // get the staging records
                DataSet groupDs = ms.GetUnsubmittedStagingRecords(transactionType);

                // Log entry
                log.CreateLogEntry(String.Format(Constants.LogString.SumittingMMISGroupAffiliationStart, groupDs.Tables[MMISShared.dbtGrps].Rows.Count));
                
                // loop over records in dataset
                foreach (DataRow groupRow in groupDs.Tables[0].Rows)
                {
                    try
                    {
                        string groupPk = Methods.GetStringValue(groupRow[MMISShared.groupStagingPk].ToString());
               
                        // create the object to submit
                        SubmitProvider groupAffiliate = new SubmitProvider();
                        DataRow[] affiliates = groupRow.GetChildRows(MMISShared.dbrGrps2dbtAffs);
                        int groupPartyId = Convert.ToInt32(groupRow["PARTY_ID"].ToString());

                        log.CreateLogEntry(String.Format(Constants.LogString.SumittingMMISGroupAffiliationGroupStart, groupPartyId));
                        
                        foreach (DataRow affiliate in affiliates)
                        {
                            int transQueueId = Convert.ToInt32(affiliate["TRANSACTION_QUEUE_ID"].ToString());
                            int affiliatePartyId = Convert.ToInt32(affiliate["PARTY_ID"].ToString());
                            
                            // Log entry
                            log.CreateLogEntry(String.Format(Constants.LogString.SumittingMMISIndividualAffiliateStart, groupPartyId, affiliatePartyId));

                            groupAffiliate.max_trans_id = transQueueId.ToString("0000000000");
                            groupAffiliate.max_action = DataExchangeHelper.GetString("ACTION", affiliate);;
                            groupAffiliate.max_pdms_id = groupPartyId.ToString("0000000000");
                            groupAffiliate.max_member_pdms_id = affiliatePartyId.ToString("0000000000");
                            groupAffiliate.id = DataExchangeHelper.GetString("GROUP_MEDICAID_ID", affiliate);
                            groupAffiliate.member_id = DataExchangeHelper.GetString("MEDICAID_ID", affiliate);
                            if (!String.IsNullOrWhiteSpace(affiliate["EFFECTIVE_DATE"].ToString()))
                            {
                                groupAffiliate.affl_beg_dt = Methods.GetDateValue(affiliate["EFFECTIVE_DATE"]);
                            }
                            if (groupAffiliate.max_action == "AM")
                            {
                                groupAffiliate.affl_end_dt = new DateTime(9999, 12, 31);
                            }
                            else
                            {
                                groupAffiliate.affl_end_dt = SetEndDate(groupAffiliate.affl_beg_dt, Methods.GetDateValue(affiliate["END_DATE"]));
                            }

                            groupAffiliate.aff_ssn_num = affiliate["SSN"].ToString();
                            DataRowCollection enrollmentInfo = MMISShared.GetEnrollmentInfo(affiliatePartyId);
                            if (enrollmentInfo != null)
                            {
                                groupAffiliate.enrol_stat_ty_cd = DataExchangeHelper.GetString("EnrollmentStatusCode", enrollmentInfo[0]);
                                // reassign enrollment status to 00 for MCO providers.
                                groupAffiliate.enrol_stat_ty_cd = groupAffiliate.enrol_stat_ty_cd == "90" ? "00" : groupAffiliate.enrol_stat_ty_cd;
                            }

                            groupAffiliate.affl_ty_cd = "G";
                            // DCPDMS-2266 - if the transaction is AM and the 
                            // affiliate provider type is Physician Assistant
                            // and the group is an individual...
                            if ((groupAffiliate.max_action == "AM" || groupAffiliate.max_action == "UM") &&
                                 DataExchangeHelper.GetString("PROVIDER_TYPE", affiliate) == "A07" &&
                                 MMISShared.IsProviderIndividual(groupPartyId))
                            {
                                // set the affiliation type to supervisor
                                groupAffiliate.affl_ty_cd = "S";
                            }
                            // fill in the multi-valued variables to keep FileHelper happy
                            FillMultivaluesWithBlanks(groupAffiliate);

                            //  append the record to the file
                            engine.AppendToFile(fullFileName, groupAffiliate);

                            // Update TRANSACTION table with Submit Date information
                                // -----------------------------------------------------
                            TransactionController.UpdateTransactionQueue(
                                 transQueueId
                                 , now
                                 , null
                                 , now
                                 , Constants.appPDMSDataExchangeUserId);
                            
                            // Log entry
                            log.CreateLogEntry(Constants.LogString.SumittingMMISIndividualAffiliateEnd);
                        }
                        // generated by sp_Admin_StoredProcBuilder on Sep 12 2013 11:07AM
                        // create parameters objects and fill with values
                        List<SqlParameter> parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_GROUP_PK", DbType.Int32, groupPk, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));
                        parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, "Submitted", true));
                        parameters.Add(SqlParms.CreateParameter("CORRELATION_ID", DbType.String, string.Empty, true));

                        DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingGroup", parameters);

                        log.CreateLogEntry(String.Format(Constants.LogString.SumittingMMISGroupAffiliationGroupEnd, groupPartyId));
                    }
                    catch (Exception ex)
                    {
                        LogErrorRecord(log, "MMIS Submit Exception", MMISShared.dbtGrps, providerPK, ex.Message);
                    }
                }

                // Log entry
                log.CreateLogEntry(Constants.LogString.SumittingMMISGroupAffiliationEnd);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private void FillMultivaluesWithBlanks(SubmitProvider provider)
        {
            // fill the enrollment history here 
            for (int index = 0; index < provider.ProviderStatus.Count(); index++)
            {
                provider.ProviderStatus[index] = new SubmitProviderStatus();
                provider.ProviderStatus[index].enrol_stat_ty_cd = "";
                provider.ProviderStatus[index].stat_eff_dt = new DateTime(1753, 1, 1);
                provider.ProviderStatus[index].stat_end_dt = new DateTime(1753, 1, 1);
                provider.ProviderStatus[index].ty_cd = "";
            }

            // add the license and cert info
            for (int index = 0; index < provider.LicenseCerts.Length; index++)
            {
                provider.LicenseCerts[index] = new SubmitProviderLicense();
                provider.LicenseCerts[index].lic_cert_cd = "";
                provider.LicenseCerts[index].lic_cert_num = "";
                provider.LicenseCerts[index].lic_eff_dt = "";
                provider.LicenseCerts[index].lic_expir_dt = new DateTime(1753, 1, 1);
                provider.LicenseCerts[index].lic_rstrct_cd = "";
                provider.LicenseCerts[index].st_cd = "";
            }

            // add the provider category of service

            for (int index = 0; index < provider.ProviderCOS.Length; index++)
            {
                provider.ProviderCOS[index] = new SubmitProviderCOS();
                provider.ProviderCOS[index].cos_beg_dt = new DateTime(1753, 1, 1);
                provider.ProviderCOS[index].cos_cd = "";
                provider.ProviderCOS[index].cos_end_dt = new DateTime(1753, 1, 1);
            }

            // add the provider specialties
            for (int index = 0; index < provider.ProviderSpecialty.Length; index++)
            {
                provider.ProviderSpecialty[index] = new SubmitProviderSpecialty();
                provider.ProviderSpecialty[index].specl_beg_dt = new DateTime(1753, 1, 1);
                provider.ProviderSpecialty[index].specl_cd = "";
                provider.ProviderSpecialty[index].specl_end_dt = new DateTime(1753, 1, 1);
                provider.ProviderSpecialty[index].lic_brd_num = "";
            }

            // add the provider taxonomys
            for (int index = 0; index < provider.ProviderTaxonomy.Length; index++)
            {
                provider.ProviderTaxonomy[index] = new SubmitProviderTaxonomy();
                provider.ProviderTaxonomy[index].taxon_beg_dt = new DateTime(1753, 1, 1);
                provider.ProviderTaxonomy[index].taxonomy_cd = "";
                provider.ProviderTaxonomy[index].taxon_end_dt = new DateTime(1753, 1, 1);
            }

        }
        private void SubmitIndividualProviders(FileHelpers.FixedFileEngine engine, string fullFileName)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            string providerPK = string.Empty;
            DateTime now = DateTime.Now;

            try
            {
                // Populate the staging tables
                this.PopulateIndividualStagingData();
                // Get the staging records
                int transactionType = Constants.TransactionType.SendProviderUpdatestoMMIS;
                MMISShared ms = new MMISShared(this.ThreadId);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());
                
                // Retrieve added staging records
                DataSet providersDS = ms.GetIndividualStagingRecords(Constants.PDMSStatusType.PendingMMISSubmission);
               
                // Log entry
                log.CreateLogEntry(String.Format(Constants.LogString.SubmittingMMISIndividualsStart, providersDS.Tables[MMISShared.dbtProvs].Rows.Count));

                // loop over provider records in dataset
                foreach (DataRow providerRow in providersDS.Tables[MMISShared.dbtProvs].Rows)
                {
                    try
                    {
                        // Create the object to submit
                        SubmitProvider provider = new SubmitProvider();
                        int transQueueId = DataExchangeHelper.GetInt("TRANSACTION_QUEUE_ID", providerRow);
                        providerPK = DataExchangeHelper.GetString(MMISShared.dbfPK, providerRow);
                        int transType = DataExchangeHelper.GetInt("TRANSACTION_TYPE_ID",providerRow);
                        provider.max_trans_id = transQueueId.ToString("0000000000");
                        int partyID = DataExchangeHelper.GetInt("PARTY_ID", providerRow);
                        provider.max_pdms_id = partyID.ToString("0000000000").ToUpper();
                        provider.max_action = DataExchangeHelper.GetString("ACTION",providerRow);
                        provider.id = DataExchangeHelper.GetString("PROVIDER_ID", providerRow);
                        int providerCategoryTypeID = MMISShared.GetProviderCategoryTypeID(partyID);
                        provider.indiv_grcd = providerCategoryTypeID == 2 ? "G" : "I";
                        provider.nam_org_ind = providerCategoryTypeID == 1 ? "N" : "Y";
                        //provider.providerCategory = providerRow["PROVIDER_CATEGORY"].ToString();
                        provider.last_nam = DataExchangeHelper.GetString("LAST_NAME",providerRow).ToUpper();
                        provider.fst_nam = DataExchangeHelper.GetString("FIRST_NAME",providerRow).ToUpper();
                        provider.mi_nam = DataExchangeHelper.GetString("MIDDLE_NAME",providerRow).ToUpper();
                        string suffix = MMISShared.GetTitle(partyID);
                        provider.nam = provider.fst_nam + (provider.mi_nam.Trim().Length > 0 ? " " + provider.mi_nam : "") + " " + provider.last_nam + (suffix.Length > 0 ? " " + suffix : ""); 
                        provider.npi_num = DataExchangeHelper.GetString("NPI_ID",providerRow);
                        provider.atyind = "N";
                        provider.dba_nam = provider.dba_last_nam = DataExchangeHelper.GetString("DBA", providerRow);
                        provider.dba_org_ind = provider.dba_nam.Trim().Length > 0 ? "Y" : "N";
                        provider.dea_eff_dt = DataExchangeHelper.GetDateTime("DEA_EFFECTIVE_DATE_TIME",providerRow);
                        provider.dea_exdt = SetEndDate(provider.dea_eff_dt, DataExchangeHelper.GetDateTime("DEA_END_DATE_TIME",providerRow));
                        provider.dea_num = DataExchangeHelper.GetString("DEA_NUMBER",providerRow);
                        DateTime fiscalYearEndDate = DataExchangeHelper.GetDateTime("FISCAL_YEAR_END", providerRow);
                        provider.faci_fy_mo_num = (fiscalYearEndDate.Year == 1753) ? "12" : GetFYBeginMonthFromEndMonth( fiscalYearEndDate.Month).ToString("00");
                        provider.tot_bed_num = DataExchangeHelper.GetInt("NUMBER_OF_BEDS", providerRow).ToString("0000");
                        provider.prof_tech_ind = "P";
                        DataRow drMCare = MMISShared.GetMedicareInfo(partyID);
                        if (drMCare != null)
                        {
                            provider.mcare_end_dt = DataExchangeHelper.GetDateTime("MEDICAREEndDate", drMCare);
                            if (provider.mcare_end_dt == null || provider.mcare_end_dt.Value.Year == 1753 || (provider.mcare_end_dt != null && provider.mcare_end_dt.HasValue && provider.mcare_end_dt.Value > DateTime.Now))
                            {
                                provider.mcare_num = DataExchangeHelper.GetString("MEDICARENumber", drMCare).PadRight(10);
                                provider.mcare_beg_dt = DataExchangeHelper.GetDateTime("MEDICAREEffDate", drMCare);
                                provider.mcare_end_dt = SetEndDate(provider.mcare_beg_dt, provider.mcare_end_dt);
                                provider.mcare_ind = "Y";
                            }
                            else
                            {
                                provider.mcare_num = "";
                                provider.mcare_beg_dt = null;
                                provider.mcare_end_dt = null;
                                provider.mcare_ind = "N";
                            }
                        }
                        else
                        {
                            provider.mcare_num = "";
                            provider.mcare_beg_dt = null;
                            provider.mcare_end_dt = null;
                            provider.mcare_ind = "N";
                        }
                        DataRow dr = MMISShared.GetPracticeAndOwnershipCodes(partyID);
                        if (dr != null)
                        {
                            provider.pract_ty_cd = DataExchangeHelper.GetString("PRACTICE_TYPE_CODE", dr);
                            provider.owner_ty_cd = DataExchangeHelper.GetString("TAX_ENTITY_TYPE_CODE", dr);
                            if (provider.owner_ty_cd.Trim().Length == 0)
                            {
                                provider.owner_ty_cd = "O";
                            }
                        }
                        else
                        {
                            provider.owner_ty_cd = "O";
                        } 
                        string rawProfitStatus = DataExchangeHelper.GetString("PROFIT_STATUS", providerRow);
                        provider.profit_ind = (provider.owner_ty_cd == "G" || provider.owner_ty_cd == "N") ? "N" : "Y";
                        provider.ra_sort_seq_cd = "1";
                        provider.ra_prt_suscd = "A";
                        provider.max_member_pdms_id = "0000000000";
                        DataRow taxIDRow = MMISShared.GetTaxIDInfo(partyID);
                        if (taxIDRow != null)
                        {
                            // get the type of tax id
                            int taxIDTypeID = DataExchangeHelper.GetInt("TAX_ID_TYPE_ID", taxIDRow);
                            int altTaxIDTypeID = DataExchangeHelper.GetInt("ALT_TAX_ID_TYPE_ID", taxIDRow);
                            string taxID = DataExchangeHelper.GetString("TAX_ID", taxIDRow).Trim();
                            string altTaxID = DataExchangeHelper.GetString("ALT_TAX_ID", taxIDRow).Trim();

                            // set the tax ID's for an SSN
                            if (taxIDTypeID == 15)
                            {
                                provider.ssn_num = provider.ssn_num_1 = taxID;
                            }
                            else if (altTaxIDTypeID == 15)
                            {
                                provider.ssn_num = provider.ssn_num_1 = altTaxID;
                            }
                            else
                            {
                                provider.ssn_num = provider.ssn_num_1 = "";
                            }

                            // set the tax ID's for an FEIN
                            if (taxIDTypeID == 16)
                            {
                                provider.fed_tax_id_1 = provider.billing_tax_key_id = taxID;
                            }
                            else if (altTaxIDTypeID == 16)
                            {
                                provider.tax_key_id = provider.fed_tax_id_1 = provider.billing_tax_key_id = altTaxID;
                            }
                            else
                            {
                                provider.fed_tax_id_1 = "";
                                provider.billing_tax_key_id = taxID.Trim().Length > 0 ? taxID : altTaxID;
                            }

                            // set the tax ID type
                            if (taxID.Length > 0 && altTaxID.Length > 0 && taxIDTypeID != altTaxIDTypeID)
                            {
                                provider.fed_id_ind_1 = "B";
                            }
                            else if (taxIDTypeID == 16 || altTaxIDTypeID == 16)
                            {
                                provider.fed_id_ind_1 = "F";
                            }
                            else
                            {
                                provider.fed_id_ind_1 = "S";
                            }

                            // if the provider only has SSN...
                            if (provider.fed_id_ind_1 == "S")
                            {
                                // set the tax key id to SSN
                                provider.tax_key_id = provider.ssn_num;
                            }
                            else // provider has EIN or Both...
                            {
                                // set tax key id to EIN
                                provider.tax_key_id = provider.fed_tax_id_1;
                            }
                        }

                        if (provider.max_action.Trim() == "AP")
                        {
                            DataRow cliaRow = MMISShared.GetCLIAInfo(partyID);
                            if (cliaRow != null)
                            {
                                provider.clia_num = DataExchangeHelper.GetString("CLIANumber", cliaRow);
                                provider.clia_cert_ty_cd = "1";
                                provider.clia_cert_eff_dt = DataExchangeHelper.GetDateTime("CLIAEffDate", cliaRow);
                                provider.clia_cert_expir_dt = SetEndDate(provider.clia_cert_eff_dt, DataExchangeHelper.GetDateTime("CLIAEndDate", cliaRow));
                            }
                        }
                        provider.bkuwhold_ind = "W";
                        provider.sec_bnote_yr_num = "0000";
                        DateTime tempAppDate = MMISShared.GetLatestApplicationDate(partyID);

                        //Only send P-APPL-DT field on a Provider Add transaction
                        if (provider.max_action.Trim() == "AP")
                            provider.appl_dt = tempAppDate.Year == 1753 ? "" : tempAppDate.ToString("yyyy-MM-dd");
                        
                        string suffixName = DataExchangeHelper.GetString("SUFFIX_NAME", providerRow);
                        DataRow officeRow = MMISShared.GetRegOfficeInfo(partyID);
                        if (officeRow != null)
                        {
                            provider.qstnr_npp_ind = DataExchangeHelper.GetBool("OFFICE_NEWPATIENT", officeRow) ? "Y" : "N";
                            provider.qstnr_npr_ind = DataExchangeHelper.GetBool("OFFICE_REFFERAL", officeRow) ? "Y" : "N";
                        }
                        DataRow drAddlInfo = null;
                        string addressFirstName = "";
                        string addressMiddleName = "";
                        string addressLastName = "";
                        string addressTitle = "";
                        string addrType = "";
                        string addressOrgName = "";
                        string addressPhone = "";
                        //// populate addresses
                        foreach (DataRow address in providerRow.GetChildRows(MMISShared.dbrProv2Addrs))
                        {
                            int addressType = Convert.ToInt32(address["ADDRESS_STAGING_TYPE_ID"]);
                            string countyName = Methods.GetStringValue(address["COUNTY"], false).ToString();
                            string providerName = Methods.GetStringValue(address["NAME"], false).ToString().ToUpper();
                            string provLastName = Methods.GetStringValue(address["LAST_NAME"], false).ToString().ToUpper();
                            string provFirstName = Methods.GetStringValue(address["FIRST_NAME"], false).ToString().ToUpper();
                            string provMIName = Methods.GetStringValue(address["MI_NAME"], false).ToString().ToUpper(); 
                            string street1 = Methods.GetStringValue(address["ADDRESS_STREET1"], false).ToString().ToUpper();
                            string street2 = Methods.GetStringValue(address["ADDRESS_STREET2"], false).ToString().ToUpper();
                            string street3 = Methods.GetStringValue(address["ADDRESS_STREET3"], false).ToString().ToUpper();
                            string city = Methods.GetStringValue(address["ADDRESS_CITY"], false).ToString().ToUpper();
                            string state = Methods.GetStringValue(address["ADDRESS_STATE"], false).ToString().ToUpper();
                            string zip = Methods.GetStringValue(address["ADDRESS_ZIP"], false).ToString();
                            string zipFour = Methods.GetStringValue(address["ADDRESS_ZIP_FOUR"], false).ToString();
                            if (zipFour.Trim().Length == 0)
                            {
                                zipFour = "0000";
                            }
                            string phone = Methods.GetStringValue(address["ADDRESS_PHONE"], false).ToString();
                            string phoneExt = Methods.GetStringValue(address["ADDRESS_PHONE_EXT"], false).ToString();
                            string fax = Methods.GetStringValue(address["ADDRESS_FAX"], false).ToString();
                            string email = Methods.GetStringValue(address["ADDRESS_EMAIL"], false).ToString().ToUpper();
                            string countyCode = MMISShared.GetCountyCode(countyName, state);
                            string quadWard = MMISShared.GetQuadWard(partyID, MMISShared.ConvertAddressTypeIDToCode(addressType));

                            drAddlInfo = MMISShared.GetAdditionalAddressInfo(partyID, (Enumerations.ContactMechanismRoleTypeId)addressType);
                            addressFirstName = "";
                            addressMiddleName = "";
                            addressLastName = "";
                            addressTitle = "";
                            addrType = "";
                            addressOrgName = "";
                            addressPhone = "";
                            if (drAddlInfo != null)
                            {
                                addressPhone = DataExchangeHelper.GetString("PHONE", drAddlInfo).Trim();
                                if (addressType != (int)Enumerations.ContactMechanismRoleTypeId.Servicing)
                                {
                                    addressFirstName = DataExchangeHelper.GetString("FIRST_NAME", drAddlInfo).Trim();
                                    addressMiddleName = DataExchangeHelper.GetString("MIDDLE_NAME", drAddlInfo).Trim();
                                    addressLastName = DataExchangeHelper.GetString("LAST_NAME", drAddlInfo).Trim();
                                    addressTitle = DataExchangeHelper.GetString("TITLE", drAddlInfo).Trim();
                                    addrType = DataExchangeHelper.GetString("ADDRESS_TYPE", drAddlInfo).Trim();
                                    addressOrgName = DataExchangeHelper.GetString("ORG_NAME", drAddlInfo).Trim();
                                }
                            }
                            if (state.Trim().Length > 0)
                            {
                                switch (addressType)
                                {
                                    case (int)Enumerations.ContactMechanismRoleTypeId.MailTo:
                                        provider.g_mail_to_quad_cd = quadWard.Substring(0, 2);
                                        provider.g_mail_to_ward_cd = quadWard.Substring(2);
                                        provider.mail_contct_nam = MMISShared.GetMailToContactName(partyID);
                                        if (addrType.StartsWith("O"))
                                        {
                                            provider.mail_to_nam_org_ind = "Y";
                                            provider.mail_to_last_nam = provider.mail_to_nam = addressOrgName;
                                            provider.mail_to_fst_nam = "";
                                            provider.mail_to_mi_nam = "";
                                            provider.mail_to_sfx_nam = "";
                                        }
                                        else
                                        {
                                            provider.mail_to_nam_org_ind = "N";
                                            provider.mail_to_nam = DataExchangeHelper.FormatName(addressFirstName, addressMiddleName, addressLastName, addressTitle);
                                            provider.mail_to_last_nam = addressLastName;
                                            provider.mail_to_fst_nam = addressFirstName;
                                            provider.mail_to_mi_nam = addressMiddleName;
                                            provider.mail_to_sfx_nam = addressTitle;
                                        }

                                        provider.mail_contct_email_ad = email;
                                        provider.mail_contct_phon_num = phone;
                                        provider.mail_fax_num = fax;
                                        provider.mail_phon_num = addressPhone;
                                        provider.mail_to_city_nam = city;
                                        provider.mail_to_line1_ad = street1;
                                        provider.mail_to_line2_ad = street2;
                                        provider.mail_to_st_cd = state;
                                        provider.mail_to_zip5_cd = zip;
                                        provider.mail_to_zip4_cd = zipFour;
                                        provider.mail_to_cnty_cd = countyCode;
                                        break;

                                    case (int)Enumerations.ContactMechanismRoleTypeId.Servicing:
                                        provider.servicing_nam_org_ind = provider.nam_org_ind;
                                        provider.g_servicing_quad_cd = quadWard.Substring(0, 2);
                                        provider.g_servicing_ward_cd = quadWard.Substring(2);
                                        provider.servicing_contct_email_ad = email;
                                        provider.servicing_contct_nam = MMISShared.GetServicingContactName(partyID);
                                        provider.servicing_nam = provider.nam;
                                        provider.servicing_last_nam = provider.last_nam;
                                        provider.servicing_fst_nam = provider.fst_nam;
                                        provider.servicing_mi_nam = provider.mi_nam;
                                        provider.servicing_sfx_nam = suffix;
                                        provider.servicing_contct_phon_num = phone;
                                        provider.servicing_fax_num = fax;
                                        provider.servicing_phon_num = addressPhone;
                                        provider.servicing_contct_email_ad = email;
                                        provider.servicing_line1_ad = street1;
                                        provider.servicing_line2_ad = street2;
                                        provider.servicing_city_nam = city;
                                        provider.servicing_st_cd = state;
                                        provider.servicing_zip5_cd = zip;
                                        provider.servicing_zip4_cd = zipFour;
                                        provider.servicing_cnty_cd = countyCode;
                                        provider.locn_cd = (state == "DC") ? "I" : "O";
                                        break;

                                    case (int)Enumerations.ContactMechanismRoleTypeId.Credentialing:
                                        break;

                                    case (int)Enumerations.ContactMechanismRoleTypeId.PayTo:
                                        provider.g_billing_quad_cd = quadWard.Substring(0, 2);
                                        provider.g_billing_ward_cd = quadWard.Substring(2);
                                        provider.billing_contct_email_ad = email;
                                        provider.billing_contct_nam = MMISShared.GetBillingContactName(partyID);
                                        if (addrType.StartsWith("O"))
                                        {
                                            provider.billing_nam_org_ind = "Y";
                                            provider.billing_last_nam = provider.billing_nam = addressOrgName;
                                            provider.billing_fst_nam = "";
                                            provider.billing_mi_nam = "";
                                            provider.billing_sfx_nam = "";
                                        }
                                        else
                                        {
                                            provider.billing_nam_org_ind = "N";
                                            provider.billing_nam = DataExchangeHelper.FormatName(addressFirstName, addressMiddleName, addressLastName, addressTitle);
                                            provider.billing_last_nam = addressLastName;
                                            provider.billing_fst_nam = addressFirstName;
                                            provider.billing_mi_nam = addressMiddleName;
                                            provider.billing_sfx_nam = addressTitle;
                                        }
                                        provider.billing_contct_phon_num = phone;
                                        provider.billing_fax_num = fax;
                                        provider.billing_phon_num = addressPhone;
                                        provider.billing_line1_ad = street1;
                                        provider.billing_line2_ad = street2;
                                        provider.billing_city_nam = city;
                                        provider.billing_st_cd = state;
                                        provider.billing_zip5_cd = zip;
                                        provider.billing_zip4_cd = zipFour;
                                        provider.billing_cnty_cd = countyCode;
                                        break;
                                }
                            }
                        }

                        DataRow drRAAddr = MMISShared.GetRegAddress(partyID, "R");
                        drAddlInfo = MMISShared.GetAdditionalAddressInfo(partyID, Enumerations.ContactMechanismRoleTypeId.Remittance);
                        if (drRAAddr != null && drAddlInfo != null)
                        {
                            provider.ra_mailing_st_cd = DataExchangeHelper.GetString("STATE", drRAAddr).ToUpper();
                            if (provider.ra_mailing_st_cd.Trim().Length > 0)
                            {
                                addressPhone = DataExchangeHelper.GetString("PHONE", drAddlInfo).Trim();
                                addressFirstName = DataExchangeHelper.GetString("FIRST_NAME", drAddlInfo).Trim();
                                addressMiddleName = DataExchangeHelper.GetString("MIDDLE_NAME", drAddlInfo).Trim();
                                addressLastName = DataExchangeHelper.GetString("LAST_NAME", drAddlInfo).Trim();
                                addressTitle = DataExchangeHelper.GetString("TITLE", drAddlInfo).Trim();
                                addrType = DataExchangeHelper.GetString("ADDRESS_TYPE", drAddlInfo).Trim();
                                addressOrgName = DataExchangeHelper.GetString("ORG_NAME", drAddlInfo).Trim();

                                string cntyName = DataExchangeHelper.GetString("COUNTY", drRAAddr);
                                provider.ra_mailing_contct_nam = DataExchangeHelper.GetString("CONTACT_NAME", drRAAddr).ToUpper();
                                provider.ra_mailing_contct_phon_num = DataExchangeHelper.GetString("PHONE", drRAAddr);
                                if (addrType.StartsWith("O"))
                                {
                                    provider.ra_mailing_nam_org_ind = "Y";
                                    provider.ra_mailing_last_nam = provider.ra_mailing_nam = addressOrgName;
                                    provider.ra_mailing_fst_nam = "";
                                    provider.ra_mailing_mi_nam = "";
                                    provider.ra_mailing_sfx_nam = "";
                                }
                                else
                                {
                                    provider.ra_mailing_nam_org_ind = "N";
                                    provider.ra_mailing_nam = DataExchangeHelper.FormatName(addressFirstName, addressMiddleName, addressLastName, addressTitle);
                                    provider.ra_mailing_last_nam = addressLastName;
                                    provider.ra_mailing_fst_nam = addressFirstName;
                                    provider.ra_mailing_mi_nam = addressMiddleName;
                                    provider.ra_mailing_sfx_nam = addressTitle;
                                } 
                                provider.ra_mailing_line1_ad = DataExchangeHelper.GetString("STREET1", drRAAddr).ToUpper();
                                provider.ra_mailing_line2_ad = DataExchangeHelper.GetString("STREET2", drRAAddr).ToUpper();
                                provider.ra_mailing_city_nam = DataExchangeHelper.GetString("CITY", drRAAddr).ToUpper();
                                provider.ra_mailing_st_cd = DataExchangeHelper.GetString("STATE", drRAAddr).ToUpper();
                                provider.ra_mailing_zip5_cd = DataExchangeHelper.GetString("ZIP5", drRAAddr);
                                provider.ra_mailing_zip4_cd = DataExchangeHelper.GetString("ZIP4", drRAAddr);
                                if (provider.ra_mailing_zip4_cd.Trim().Length == 0)
                                {
                                    provider.ra_mailing_zip4_cd = "0000";
                                }
                                provider.ra_mailing_phon_num = addressPhone;
                                provider.ra_mailing_fax_num = DataExchangeHelper.GetString("FAX", drRAAddr);
                                provider.ra_mailing_contct_email_ad = DataExchangeHelper.GetString("EMAIL", drRAAddr).ToUpper();
                                provider.ra_mailing_cnty_cd = MMISShared.GetCountyCode(cntyName, provider.ra_mailing_st_cd);
                                string quWa = MMISShared.GetQuadWard(partyID, "R");
                                provider.g_ra_mailing_quad_cd = quWa.Substring(0, 2);
                                provider.g_ra_mailing_ward_cd = quWa.Substring(2);
                            }
                        }

                        DataRow drOtherAddr = MMISShared.GetRegAddress(partyID, "O");
                        drAddlInfo = MMISShared.GetAdditionalAddressInfo(partyID, Enumerations.ContactMechanismRoleTypeId.Other);
                        if (drOtherAddr != null && drAddlInfo != null)
                        {
                            provider.other_st_cd = DataExchangeHelper.GetString("STATE", drOtherAddr).ToUpper();
                            if (provider.other_st_cd.Trim().Length > 0)
                            {
                                addressPhone = DataExchangeHelper.GetString("PHONE", drAddlInfo).Trim();
                                addressFirstName = DataExchangeHelper.GetString("FIRST_NAME", drAddlInfo).Trim();
                                addressMiddleName = DataExchangeHelper.GetString("MIDDLE_NAME", drAddlInfo).Trim();
                                addressLastName = DataExchangeHelper.GetString("LAST_NAME", drAddlInfo).Trim();
                                addressTitle = DataExchangeHelper.GetString("TITLE", drAddlInfo).Trim();
                                addrType = DataExchangeHelper.GetString("ADDRESS_TYPE", drAddlInfo).Trim();
                                addressOrgName = DataExchangeHelper.GetString("ORG_NAME", drAddlInfo).Trim();

                                string cntyName = DataExchangeHelper.GetString("COUNTY", drOtherAddr);
                                provider.other_nam = DataExchangeHelper.GetString("NAME", drOtherAddr).ToUpper();
                                provider.other_contct_nam = DataExchangeHelper.GetString("CONTACT_NAME", drOtherAddr).ToUpper();
                                provider.other_contct_phon_num = DataExchangeHelper.GetString("PHONE", drOtherAddr);
                                if (addrType.StartsWith("O"))
                                {
                                    provider.other_nam_org_ind = "Y";
                                    provider.other_last_nam = provider.other_nam = addressOrgName;
                                    provider.other_fst_nam = "";
                                    provider.other_mi_nam = "";
                                    provider.other_sfx_nam = "";
                                }
                                else
                                {
                                    provider.other_nam_org_ind = "N";
                                    provider.other_nam = DataExchangeHelper.FormatName(addressFirstName, addressMiddleName, addressLastName, addressTitle);
                                    provider.other_last_nam = addressLastName;
                                    provider.other_fst_nam = addressFirstName;
                                    provider.other_mi_nam = addressMiddleName;
                                    provider.other_sfx_nam = addressTitle;
                                } 
                                provider.other_line1_ad = DataExchangeHelper.GetString("STREET1", drOtherAddr).ToUpper();
                                provider.other_line2_ad = DataExchangeHelper.GetString("STREET2", drOtherAddr).ToUpper();
                                provider.other_city_nam = DataExchangeHelper.GetString("CITY", drOtherAddr).ToUpper();
                                provider.other_zip5_cd = DataExchangeHelper.GetString("ZIP5", drOtherAddr);
                                provider.other_zip4_cd = DataExchangeHelper.GetString("ZIP4", drOtherAddr);
                                if (provider.other_zip4_cd.Trim().Length == 0)
                                {
                                    provider.other_zip4_cd = "0000";
                                }
                                provider.other_phon_num = addressPhone;
                                provider.other_fax_num = DataExchangeHelper.GetString("FAX", drOtherAddr);
                                provider.other_contct_email_ad = DataExchangeHelper.GetString("EMAIL", drOtherAddr).ToUpper();
                                provider.other_cnty_cd = MMISShared.GetCountyCode(cntyName, provider.other_st_cd);
                                string quWa = MMISShared.GetQuadWard(partyID, "O");
                                provider.g_other_quad_cd = quWa.Substring(0, 2);
                                provider.g_other_ward_cd = quWa.Substring(2);
                            }
                        }

                        DataRow drW9Addr = MMISShared.GetRegAddress(partyID, "W");
                        drAddlInfo = MMISShared.GetAdditionalAddressInfo(partyID, Enumerations.ContactMechanismRoleTypeId.W9);
                        if (drW9Addr != null && drAddlInfo != null)
                        {
                            provider.w9_1099_st_cd = DataExchangeHelper.GetString("STATE", drW9Addr).ToUpper();
                            if (provider.w9_1099_st_cd.Trim().Length > 0)
                            {
                                addressPhone = DataExchangeHelper.GetString("PHONE", drAddlInfo).Trim();
                                addressFirstName = DataExchangeHelper.GetString("FIRST_NAME", drAddlInfo).Trim();
                                addressMiddleName = DataExchangeHelper.GetString("MIDDLE_NAME", drAddlInfo).Trim();
                                addressLastName = DataExchangeHelper.GetString("LAST_NAME", drAddlInfo).Trim();
                                addressTitle = DataExchangeHelper.GetString("TITLE", drAddlInfo).Trim();
                                addrType = DataExchangeHelper.GetString("ADDRESS_TYPE", drAddlInfo).Trim();
                                addressOrgName = DataExchangeHelper.GetString("ORG_NAME", drAddlInfo).Trim();

                                string cntyName = DataExchangeHelper.GetString("COUNTY", drW9Addr);
                                provider.w9_1099_contct_nam = DataExchangeHelper.GetString("CONTACT_NAME", drW9Addr);
                                provider.w9_1099_contct_phon_num = DataExchangeHelper.GetString("PHONE", drW9Addr);
                                provider.w9_1099_contct_email_ad = DataExchangeHelper.GetString("EMAIL", drW9Addr);
                                if (addrType.StartsWith("O"))
                                {
                                    provider.w9_1099_nam_org_ind = "Y";
                                    provider.w9_1099_last_nam = provider.w9_1099_nam = addressOrgName;
                                    provider.w9_1099_fst_nam = "";
                                    provider.w9_1099_mi_nam = "";
                                    provider.w9_1099_sfx_nam = "";
                                }
                                else
                                {
                                    provider.w9_1099_nam_org_ind = "N";
                                    provider.w9_1099_nam = DataExchangeHelper.FormatName(addressFirstName, addressMiddleName, addressLastName, addressTitle);
                                    provider.w9_1099_last_nam = addressLastName;
                                    provider.w9_1099_fst_nam = addressFirstName;
                                    provider.w9_1099_mi_nam = addressMiddleName;
                                    provider.w9_1099_sfx_nam = addressTitle;
                                } 
                                provider.w9_1099_line1_ad = DataExchangeHelper.GetString("STREET1", drW9Addr).ToUpper();
                                provider.w9_1099_line2_ad = DataExchangeHelper.GetString("STREET2", drW9Addr).ToUpper();
                                provider.w9_1099_city_nam = DataExchangeHelper.GetString("CITY", drW9Addr).ToUpper();
                                provider.w9_1099_zip5_cd = DataExchangeHelper.GetString("ZIP5", drW9Addr);
                                provider.w9_1099_zip4_cd = DataExchangeHelper.GetString("ZIP4", drW9Addr);
                                if (provider.w9_1099_zip4_cd.Trim().Length == 0)
                                {
                                    provider.w9_1099_zip4_cd = "0000";
                                }
                                provider.w9_1099_phon_num = addressPhone;
                                provider.w9_1099_fax_num = DataExchangeHelper.GetString("FAX", drW9Addr);
                                provider.w9_1099_cnty_cd = MMISShared.GetCountyCode(cntyName, provider.other_st_cd);
                                string quWa = MMISShared.GetQuadWard(partyID, "W");
                                provider.g_w9_1099_quad_cd = quWa.Substring(0, 2);
                                provider.g_w9_1099_ward_cd = quWa.Substring(2);
                            }
                        }

                        DateTime earliestEnrollmentDate = new DateTime(9999, 12, 31);

                        // add the enrollment info 
                        DataRowCollection enrollInfo = MMISShared.GetEnrollmentInfo(partyID);
                        if (enrollInfo != null && enrollInfo.Count > 0)
                        {
                            // fill in the enrollment information
                            provider.ProviderStatus[0] = new SubmitProviderStatus();
                            provider.ProviderStatus[0].enrol_stat_ty_cd = DataExchangeHelper.GetString("EnrollmentStatusCode", enrollInfo[0]);
                            provider.ProviderStatus[0].stat_eff_dt = DataExchangeHelper.GetDateTime("EnrollStartDate", enrollInfo[0]);
                            provider.ProviderStatus[0].stat_end_dt = SetEndDate(provider.ProviderStatus[0].stat_eff_dt, DataExchangeHelper.GetDateTime("EnrollEndDate", enrollInfo[0]));
                            provider.ProviderStatus[0].ty_cd = DataExchangeHelper.GetString("ProviderType", enrollInfo[0]);
                            // if the application type is QMB/Crossover and the provider type is not R01 or R02...
                            if (DataExchangeHelper.GetInt("ApplicationTypeID", enrollInfo[0]) == 6 &&
                                provider.ProviderStatus[0].ty_cd != "R01" &&
                                provider.ProviderStatus[0].ty_cd != "R02")
                            {
                                provider.ProviderStatus[0].ty_cd = "R02";
                            }
                            // set the NABP number, if appropriate
                            provider.nabnum = "";
                            if (provider.ProviderStatus[0].ty_cd == "H00" ||
                                provider.ProviderStatus[0].ty_cd == "H01" ||
                                provider.ProviderStatus[0].ty_cd == "H02")
                            {
                                provider.nabnum = provider.npi_num;
                            }

                            if (provider.ProviderStatus[0].stat_eff_dt != null && provider.ProviderStatus[0].stat_eff_dt.HasValue && provider.ProviderStatus[0].stat_eff_dt < earliestEnrollmentDate)
                            {
                                earliestEnrollmentDate = provider.ProviderStatus[0].stat_eff_dt.Value;
                            }

                            // now fill in the rest of the rows
                            // step through each enrollment history record
                            for (int index = 1; index < provider.ProviderStatus.Count(); index++)
                            {
                                // create a new enrollment history record.
                                provider.ProviderStatus[index] = new SubmitProviderStatus();

                                // if there is history data for this record...
                                if (index < enrollInfo.Count)
                                {
                                    // fill in the history record with the data
                                    DataRow drEH = enrollInfo[index];
                                    provider.ProviderStatus[index].enrol_stat_ty_cd = DataExchangeHelper.GetString("EnrollmentStatusCode", drEH);
                                    provider.ProviderStatus[index].stat_eff_dt = DataExchangeHelper.GetDateTime("EnrollStartDate", drEH);
                                    provider.ProviderStatus[index].stat_end_dt = DataExchangeHelper.GetDateTime("EnrollEndDate", drEH);
                                    provider.ProviderStatus[index].ty_cd = provider.ProviderStatus[0].ty_cd;
                                    if (provider.ProviderStatus[index].stat_eff_dt != null && provider.ProviderStatus[index].stat_eff_dt.HasValue && provider.ProviderStatus[index].stat_eff_dt < earliestEnrollmentDate)
                                    {
                                        earliestEnrollmentDate = provider.ProviderStatus[index].stat_eff_dt.Value;
                                    }
                                }
                                else
                                {
                                    // fill the record with blanks
                                    provider.ProviderStatus[index].enrol_stat_ty_cd = "";
                                    provider.ProviderStatus[index].stat_eff_dt = new DateTime(1753, 1, 1);
                                    provider.ProviderStatus[index].stat_end_dt = new DateTime(1753, 1, 1);
                                    provider.ProviderStatus[index].ty_cd = "";
                                }
                            }
                        }

                        // fill the program code and tax id information with the enrollment info
                        // we're using these values as the best we can find
                        provider.prog_cd_1 = provider.ProviderStatus[0].ty_cd == "H03" ? "A" : "M";
                        provider.tax_beg_dt_1 = provider.prog_beg_dt_1 = earliestEnrollmentDate;
                        provider.prog_end_dt_1 = provider.tax_end_dt_1 = new DateTime(9999, 12, 31);

                        // add the provider specialties
                        DataRow[] specRows = providerRow.GetChildRows(MMISShared.dbrProv2Spec).Where(r => r.Field<int>("PARTY_ID") == partyID).ToArray(); 
                        for (int index = 0; index < provider.ProviderSpecialty.Length; index++)
                        {
                            provider.ProviderSpecialty[index] = new SubmitProviderSpecialty();
                            if (index < specRows.Count())
                            {
                                DataRow currentSpecialty = specRows[index];
                                provider.ProviderSpecialty[index].specl_beg_dt = DataExchangeHelper.GetDateTime("SPEC_BEGIN_DATE", currentSpecialty);
                                provider.ProviderSpecialty[index].specl_cd = DataExchangeHelper.GetString("SPEC_CODE", currentSpecialty);
                                provider.ProviderSpecialty[index].specl_end_dt = SetEndDate(provider.ProviderSpecialty[index].specl_beg_dt, DataExchangeHelper.GetDateTime("SPEC_END_DATE", currentSpecialty));
                                provider.ProviderSpecialty[index].lic_brd_num = "";
                            }
                            else
                            {
                                provider.ProviderSpecialty[index].specl_beg_dt = new DateTime(1753, 1, 1);
                                provider.ProviderSpecialty[index].specl_cd = "";
                                provider.ProviderSpecialty[index].specl_end_dt = new DateTime(1753, 1, 1);
                                provider.ProviderSpecialty[index].lic_brd_num = "";
                            }
                        }

                        // add the license and cert info
                        DataRow[] licenseRows = providerRow.GetChildRows(MMISShared.dbrProv2LicCert).Where(r => r.Field<int>("PARTY_ID") == partyID).ToArray();
                        for (int index = 0; index < provider.LicenseCerts.Length; index++)
                        {
                            provider.LicenseCerts[index] = new SubmitProviderLicense();
                            if (index < licenseRows.Count())
                            {
                                DataRow currentLic = licenseRows[index];
                                provider.LicenseCerts[index].lic_cert_cd = DataExchangeHelper.GetString("LICENSE_TYPE", currentLic);
                                // DCPDMS-2379 - shifts license number to the left when it's greater
                                // than 10 characters.
                                string tempLicenseNumber = DataExchangeHelper.GetString("LICENSE_NUMBER", currentLic).Trim();
                                if (tempLicenseNumber.Trim().Length > 10)
                                {
                                    tempLicenseNumber = tempLicenseNumber.Substring(tempLicenseNumber.Length - 10);
                                }
                                provider.LicenseCerts[index].lic_cert_num = tempLicenseNumber;
                                if (!string.IsNullOrEmpty(DataExchangeHelper.GetString("LICENSE_NUMBER", currentLic)) && DataExchangeHelper.GetDateTime("LICENSE_EFF_DATE", currentLic) == Convert.ToDateTime("1753/1/1") && transactionType == Constants.TransactionType.SendProviderUpdatestoMMIS)
                                    provider.LicenseCerts[index].lic_eff_dt = "0001-01-01";
                                /*else if (!string.IsNullOrEmpty(provider.LicenseCerts[index].lic_eff_dt.ToString()))
                                    provider.LicenseCerts[index].lic_eff_dt = DataExchangeHelper.GetDateTime("LICENSE_EFF_DATE", currentLic).ToString();*/
                                else
                                    provider.LicenseCerts[index].lic_eff_dt = DataExchangeHelper.GetDateTime("LICENSE_EFF_DATE", currentLic).GetDateTimeFormats()[5];
                                //provider.LicenseCerts[index].lic_eff_dt = DataExchangeHelper.GetDateTime("LICENSE_EFF_DATE", currentLic);
                                if (provider.LicenseCerts[index].lic_eff_dt == "0001-01-01")
                                    provider.LicenseCerts[index].lic_expir_dt = DataExchangeHelper.GetDateTime("LICENSE_END_DATE", currentLic);
                                else
                                    provider.LicenseCerts[index].lic_expir_dt = SetEndDate(DataExchangeHelper.GetDateTime("LICENSE_EFF_DATE", currentLic), DataExchangeHelper.GetDateTime("LICENSE_END_DATE", currentLic));
                                provider.LicenseCerts[index].lic_rstrct_cd = "A";
                                provider.LicenseCerts[index].st_cd = DataExchangeHelper.GetString("LICENSE_STATE", currentLic);
                                provider.ProviderSpecialty[index].lic_brd_num = DataExchangeHelper.GetString("LICENSE_BOARD_NAME", currentLic);
                            }
                            else
                            {
                                provider.LicenseCerts[index].lic_cert_cd = "";
                                provider.LicenseCerts[index].lic_cert_num = "";
                                provider.LicenseCerts[index].lic_eff_dt = "";
                                provider.LicenseCerts[index].lic_expir_dt = new DateTime(1753, 1, 1);
                                provider.LicenseCerts[index].lic_rstrct_cd = "";
                                provider.LicenseCerts[index].st_cd = "";
                            }
                        }

                        // add the provider category of service
                        DataRow[] catRows = providerRow.GetChildRows(MMISShared.dbrProv2Cat).Where(r => r.Field<int>("PARTY_ID") == partyID).ToArray(); 
                        for (int index = 0; index < provider.ProviderCOS.Length; index++)
                        {
                            provider.ProviderCOS[index] = new SubmitProviderCOS();
                            if (index < catRows.Count())
                            {
                                DataRow currentCOS = catRows[index];
                                provider.ProviderCOS[index].cos_beg_dt = DataExchangeHelper.GetDateTime("COS_EFF_DATE", currentCOS);
                                provider.ProviderCOS[index].cos_cd = DataExchangeHelper.GetString("COS_TYPE_CODE", currentCOS);
                                if (provider.ProviderCOS[index].cos_cd.Length == 1)
                                {
                                    provider.ProviderCOS[index].cos_cd = "0" + provider.ProviderCOS[index].cos_cd;
                                }
                                provider.ProviderCOS[index].cos_end_dt = SetEndDate(provider.ProviderCOS[index].cos_beg_dt , DataExchangeHelper.GetDateTime("COS_EXPIR_DATE", currentCOS));
                            }
                            else
                            {
                                provider.ProviderCOS[index].cos_beg_dt = new DateTime(1753, 1, 1);
                                provider.ProviderCOS[index].cos_cd = "";
                                provider.ProviderCOS[index].cos_end_dt = new DateTime(1753, 1, 1);
                            }
                        }

                        // add the provider taxonomys
                        DataRow[] taxRows = providerRow.GetChildRows(MMISShared.dbrProv2Tax).Where(r => r.Field<int>("PARTY_ID") == partyID).ToArray(); 
                        for (int index = 0; index < provider.ProviderTaxonomy.Length; index++)
                        {
                            provider.ProviderTaxonomy[index] = new SubmitProviderTaxonomy();
                            if (index < taxRows.Count())
                            {
                                DataRow currentSpecialty = taxRows[index];
                                provider.ProviderTaxonomy[index].taxon_beg_dt = DataExchangeHelper.GetDateTime("TAXONOMY_START_DATE", currentSpecialty);
                                provider.ProviderTaxonomy[index].taxonomy_cd = DataExchangeHelper.GetString("TAXONOMY_CODE", currentSpecialty);
                                provider.ProviderTaxonomy[index].taxon_end_dt = SetEndDate(provider.ProviderTaxonomy[index].taxon_beg_dt, DataExchangeHelper.GetDateTime("TAXONOMY_END_DATE", currentSpecialty));
                            }
                            else
                            {
                                provider.ProviderTaxonomy[index].taxon_beg_dt = new DateTime(1753, 1, 1);
                                provider.ProviderTaxonomy[index].taxonomy_cd = "";
                                provider.ProviderTaxonomy[index].taxon_end_dt = new DateTime(1753, 1, 1);
                            }
                        }

                        //  append the record to the file
                        engine.AppendToFile(fullFileName, provider);

                        // provider submitted log

                        // Update TRANSACTION table with Submit Date information
                        // -----------------------------------------------------
                        TransactionController.UpdateTransactionQueue(
                            transQueueId
                            , now
                            , null
                            , now
                            , Constants.appPDMSDataExchangeUserId);

                        // Create parameters objects and fill with values
                        List<SqlParameter> parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("PROVIDER_STAGING_PK", DbType.Int32, providerPK, true));
                        parameters.Add(SqlParms.CreateParameter("PDMS_STATUS_TYPE_ID", DbType.Int32, Constants.PDMSStatusType.SubmittedtoMMIS, true));
                        parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_TYPE_ID", DbType.Int32, Enumerations.ProviderStatusChangeTypeId.MMIS, true));
                        parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, "Submitted", true));
                        parameters.Add(SqlParms.CreateParameter("CORRELATION_ID", DbType.String, "Submitted", true)); 
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                        DataAccess.ExecuteStoredProcedure("usp_UpdatePDMSToMMISExportRecords", parameters);
                    }
                    catch (Exception ex)
                    {
                        LogErrorRecord(log, "MMIS Submit Exception", MMISShared.dbtProvs, providerPK, ex.Message);
                    }
                }
                log.CreateLogEntry(Constants.LogString.SubmittingMMISIndividualsEnd);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private int GetFYBeginMonthFromEndMonth(int fyEndMonth)
        {
            int retMonth = 12;
            switch (fyEndMonth)
            {
                case 1:
                    retMonth = 2;
                    break;
                case 2:
                    retMonth = 3;
                    break;
                case 3:
                    retMonth = 4;
                    break;

                case 4:
                    retMonth = 5;
                    break;
                case 5:
                    retMonth = 6;
                    break;
                case 6:
                    retMonth = 7;
                    break;
                case 7:
                    retMonth = 8;
                    break;
                case 8:
                    retMonth = 9;
                    break;
                case 9:
                    retMonth = 10;
                    break;
                case 10:
                    retMonth = 11;
                    break;
                case 11:
                    retMonth = 12;
                    break;
                case 12:
                    retMonth = 1;
                    break;
            }

            return retMonth;
        }
        private void SubmitAdditionalSpecialties(FileHelpers.DelimitedFileEngine engine, string fullFileName, Logging log)
        {
            int stagingRecID = 0;
            DateTime now = DateTime.Now;

            try
            {
                // Populate the staging tables
                int transactionType = Constants.TransactionType.SendSpecialty2ToMMIS;
                this.PopulateSpecialtyStagingData();

                // get the staging records
                MMISShared ms = new MMISShared(this.ThreadId);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());

                // get the staging records
                DataSet stagingData = ms.GetUnsubmittedStagingRecords(transactionType);

                // Log entry
                log.CreateLogEntry(String.Format(Constants.LogString.SubmittingMMISRecordsStart, stagingData.Tables[0].Rows.Count));
                if (!ObjectControllerHelper.HasRows(stagingData))
                {
                    log.CreateLogEntry("No records found.");
                    return;
                }

                // loop over records in dataset
                foreach (DataRow row in stagingData.Tables[0].Rows)
                {
                    try
                    {
                        // create the object to submit
                        SubmitProvider providerOther = new SubmitProvider();
                        stagingRecID = ObjectControllerHelper.GetInt("MMIS_STAGING_SPECIALTY_ID", row);
                        int partyID = ObjectControllerHelper.GetInt("PARTY_ID", row);
                        int transQueueId = ObjectControllerHelper.GetInt("TRANSACTION_QUEUE_ID", row);
                        int transactionTypeID = ObjectControllerHelper.GetInt("TRANSACTION_TYPE_ID", row);

                        // Log entry
                        log.CreateLogEntry(string.Format(Constants.LogString.SubmittingMMISAdditionalSpecialtyStart, stagingRecID.ToString()));

                        //providerOther.actionFlag = ObjectControllerHelper.GetString("ACTION", row);
                        //providerOther.providerCategory = ObjectControllerHelper.GetString("PROVIDER_CATEGORY", row);
                        //providerOther.atypicalIndicator = ObjectControllerHelper.GetString("ATYPICAL_INDICATOR", row).ToLower() == bool.TrueString.ToLower() ? "N" : "Y";
                        //providerOther.transactionId = transQueueId.ToString();
                        //providerOther.pdmsProviderId = partyID.ToString();
                        //providerOther.medicaidId = ObjectControllerHelper.GetString("MEDICAID_ID", row);
                        //providerOther.npi = ObjectControllerHelper.GetString("NPI", row);
                        //providerOther.taxonomyCode = ObjectControllerHelper.GetString("TAXONOMY_CODE", row);
                        //if (transactionTypeID == Constants.TransactionType.SendSpecialty2ToMMIS)
                        //{
                        //    providerOther.specialty2 = ObjectControllerHelper.GetString("SPECIALTY_CODE", row);
                        //}
                        //else
                        //{
                        //    providerOther.specialty3 = ObjectControllerHelper.GetString("SPECIALTY_CODE", row);
                        //}

                        //  append the record to the file
                        engine.AppendToFile(fullFileName, providerOther);

                        // generated by sp_Admin_StoredProcBuilder on Sep 12 2013 11:07AM
                        // create parameters objects and fill with values
                        List<SqlParameter> parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_SPECIALTY_ID", DbType.Int32, stagingRecID, true));
                        parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, "Submitted", true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                        DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingSpecialty", parameters);

                        // update TQ table with new status
                        // -----------------------------------------------------
                        TransactionController.UpdateTransactionQueue(
                            transQueueId
                            , now
                            , null
                            , now
                            , Constants.appPDMSDataExchangeUserId);

                        // Log entry
                        log.CreateLogEntry(string.Format(Constants.LogString.SubmittingMMISAdditionalSpecialtyEnd, stagingRecID.ToString()));
                    }
                    catch (Exception ex)
                    {
                        LogErrorRecord(log, "MMIS Submit Exception", MMISShared.dbtSpecialties, stagingRecID.ToString(), ex.Message);
                    }
                }

                log.CreateLogEntry(Constants.LogString.SubmittingMMISRecordsEnd);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private void PopulateGroupStagingData(int transactionType)
        {
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep 11 2013  5:21PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, transactionType, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_InsertMMISStagingGroups", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
        }

        private void PopulateIndividualStagingData()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                // add records to staging tables
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                DataAccess.ExecuteStoredProcedure("usp_AddPDMSToMMISExportRecords", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private void PopulateMCOStagingData()
        {
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep 11 2013  5:21PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_InsertMMISStagingMCOAffiliations", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
        }

        private DateTime SetEndDate(DateTime? beginDate, DateTime? endDate)
        {
            DateTime retDate = (endDate == null || !endDate.HasValue) ? new DateTime(9999,12,31) : endDate.Value;
            if (beginDate == null || !beginDate.HasValue || beginDate.Value.Year == 1753)
            {
                retDate = new DateTime(1753, 1, 1);
            }
            else if (endDate == null || !endDate.HasValue || endDate.Value.Year == 1753)
            {
                retDate = new DateTime(9999, 12, 31);
            }
            return retDate;
        }


        private void PopulateSpecialtyStagingData()
        {
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep 11 2013  5:21PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_InsertMMISStagingSpecialty", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex, logProcessName);
            }
        }

        private void LogFileRecordError(Logging log, string lineNumber, string fileName, string errorMessage)
        {
            string fullMessage = "Record failed. Line No: {0}; File: {1}; Reason: {2}";
            log.CreateLogEntry(String.Format(fullMessage, lineNumber, fileName, errorMessage), Logging.LogPriority.DataLoadIssues);
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

        private void LoadMMISGroupAffiliationResponseFile(Logging log, FileInfo file)
        {
            string fileName = file.Name;

            // create log entry
            log.CreateLogEntry(String.Format("Loading group affiliation return file {0}", fileName));

            // open the file with FileHelper class and set internal variables
            RetrieveGroupAffiliation[] records;
            FileHelperEngine engine = new FileHelperEngine(typeof(RetrieveGroupAffiliation));

            try
            {
                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart));

                // Read records from file
                engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                records = engine.ReadFile(file.FullName) as RetrieveGroupAffiliation[];

                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString()
                    , engine.ErrorManager.ErrorCount.ToString()));

                // not used anymore this.UpdateGroupAffiliation(records, log);

                // record all bad errors
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileRecordError(log, engine.LineNumber.ToString(), fileName, Constants.LogString.FileLoadFailure + ex.Message);
                // do not rethrow exception so the next file is processed
            }

            // create log entry
            log.CreateLogEntry(String.Format("Return provider {0} file load complete", fileName));
        }

        private void LoadMMISResponseFile(Logging log, FileInfo file)
        {
            string fileName = file.Name;

            // create log entry
            log.CreateLogEntry(String.Format("Loading return file {0}", fileName));

            // open the file with FileHelper class and set internal variables
            RetrieveProvider[] records;
            FileHelperEngine engine = new FileHelperEngine(typeof(RetrieveProvider));

            try
            {
                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart));

                // Read records from file
                engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                records = engine.ReadFile(file.FullName) as RetrieveProvider[];

                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString()
                    , engine.ErrorManager.ErrorCount.ToString()));

               SaveRetrieveProviderTransactions(records);
               // this.UpdateGroup(records, log, Constants.TransactionType.SendCBSAUpdateToMMIS);
               // this.UpdateGroup(records, log, Constants.TransactionType.SendNPIUpdateToMMIS);
                this.UpdateProvider(records, log);
                this.UpdateGroup(records, log, Constants.TransactionType.RequestMedicaidIDfromMMIS);
                this.UpdateGroup(records, log, Constants.TransactionType.SendProviderUpdatestoMMIS);
                this.UpdateGroupAffiliation(records, log);
                this.UpdateMCOAffiliation(records, log);
               // this.UpdateAdditionalSpecialties(records, log, Constants.TransactionType.SendSpecialty2ToMMIS);

                // record all bad errors
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileRecordError(log, engine.LineNumber.ToString(), fileName, Constants.LogString.FileLoadFailure + ex.Message);
                // do not rethrow exception so the next file is processed
            }

            // create log entry
            log.CreateLogEntry(String.Format("Return provider {0} file load complete", fileName));
        }

        private void SaveRetrieveProviderTransactions(RetrieveProvider[] records)
        {
            List<SqlParameter> parameters = null;
            foreach (RetrieveProvider rec in records)
            {
                if (parameters == null)
                {
                    parameters = new List<SqlParameter>();
                }
                else
                {
                    parameters.Clear();
                }

                parameters.Add(SqlParms.CreateParameter("pin_transaction_queue_id", DbType.Int32, Convert.ToInt32(rec.transactionId), false));
                parameters.Add(SqlParms.CreateParameter("pin_provider_id", DbType.String, rec.medicaidId, false));
	            parameters.Add(SqlParms.CreateParameter("pin_party_id", DbType.Int32, Convert.ToInt32(rec.pdmsProviderId), false));
	            parameters.Add(SqlParms.CreateParameter("pin_npi", DbType.String, rec.npi, false));
	            parameters.Add(SqlParms.CreateParameter("pin_mmis_status_accepted", DbType.Boolean, rec.statusCode.StartsWith("ACC"), false));
	            parameters.Add(SqlParms.CreateParameter("pin_mmis_error_code", DbType.String, rec.errorCode, false));
	            parameters.Add(SqlParms.CreateParameter("pin_mmis_rejected_value", DbType.String, rec.rejectedValue, false));
	            parameters.Add(SqlParms.CreateParameter("pin_import_date_time", DbType.DateTime, DateTime.Now, false));
                
                DataAccess.ExecuteStoredProcedure("usp_InsertMMISRetreiveProviderTransaction", parameters);

            }
        }

        private void UpdateProvider(RetrieveProvider[] records, Logging log)
        {
            // Retrieve added staging records
            int transactionType = Constants.PDMSStatusType.SubmittedtoMMIS;
            MMISShared ms = new MMISShared(this.ThreadId);
            ms.SetInternalProperties(MethodBase.GetCurrentMethod());

            int countRow = 0;
            string partyId = string.Empty;
            string providerPk = string.Empty;
            DataSet providersDS = ms.GetIndividualStagingRecords(transactionType);

            // loop over all records and write to database
            foreach (DataRow providerRow in providersDS.Tables[MMISShared.dbtProvs].Rows)
            {
                try
                {
                    // populate the object credentials and provider object
                    int transactionID = DataExchangeHelper.GetInt("TRANSACTION_QUEUE_ID", providerRow);
                    partyId = providerRow["PARTY_ID"].ToString();
                    string medicaidId = Methods.GetStringValue(providerRow["MEDICARE_ID"]);
                    string medicaidPk = Methods.GetStringValue(providerRow["MEDICAID_PK"].ToString());
                    string errorCode = string.Empty;
                    string statusCode = string.Empty;
                    providerPk = Methods.GetStringValue(providerRow[MMISShared.dbfPK]);
                    int pdmsStatus = Constants.PDMSStatusType.Processed;

                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, partyId));

                    DateTime now = DateTime.Now;
                    if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                    {
                        // Retrieve from the MMIS file
                        List<RetrieveProvider> providers = records.Where(record => Convert.ToInt32(record.transactionId) == transactionID).ToList();
                        foreach (RetrieveProvider provider in providers)
                        {
                            if (provider == null)
                            {
                                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotFound, partyId));
                                continue;
                            }
                            else
                            {
                                partyId = RemoveLeadingZeroes(provider.pdmsProviderId).Trim();
                                medicaidId = provider.medicaidId.Trim();
                                errorCode = provider.errorCode.Trim();
                                statusCode = provider.statusCode.Trim() == Constants.MMISRecordAccepted ? Constants.MMISStatusType.Processed.ToString() : Constants.MMISStatusType.Errors.ToString();
                                if (provider.statusCode.Trim() == Constants.MMISRecordRejected)
                                {
                                    pdmsStatus = Constants.PDMSStatusType.Errors;
                                }
                            }
                            UpdateIndividualByRetrievalRecord(now, transactionID, providerPk, pdmsStatus, medicaidId, statusCode, errorCode, partyId); 
                        }
                    }
                    else
                    {
                        // get test values
                        if (++countRow <= ms.testRowsCount)
                        {
                            errorCode = AppSettings.Get("MMIS-TestGroupErrors", string.Empty);
                            statusCode = AppSettings.Get("MMIS-TestGroupStatus", Constants.MMISStatusType.Processed.ToString());
                            int medicaidLength = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));
                            if (string.IsNullOrWhiteSpace(medicaidId))
                            {
                                string medicaidPrefix = new string('1', medicaidLength - 5);
                                medicaidId = medicaidPrefix + DateTime.Now.ToString("ssfff");
                            }
                        }
                        UpdateIndividualByRetrievalRecord(now, transactionID, providerPk, pdmsStatus, medicaidId, statusCode, errorCode, partyId); 
                    }
                }
                catch (Exception ex)
                {
                    CoreException.ThrowException(this.ThreadId, ex, log.ProcessName);
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, partyId));
                }
            }
        }

        private void UpdateGroupByRetrievalRecord(DateTime importDate, int transactionQueueID, string groupPk, string medicaidId, string statusCode, string errorCode, string medicaidPk, string partyId)
        {
            MMISShared ms = new MMISShared(this.ThreadId);
            ms.SetInternalProperties(MethodBase.GetCurrentMethod());
            // Update TRANSACTION table with Process Date information
            // -----------------------------------------------------
            TransactionController.UpdateTransactionQueue(
                transactionQueueID
                , null
                , importDate
                , importDate
                , Constants.appPDMSDataExchangeUserId);

            // generated by sp_Admin_StoredProcBuilder on Sep 12 2013 11:07AM
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_GROUP_PK", DbType.Int32, groupPk, true));
            parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, true));
            parameters.Add(SqlParms.CreateParameter("STATUS_CODE", DbType.Int32, statusCode.Trim(), true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, importDate, false));
            parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, "Processed", true));
            parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, errorCode, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

            DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingGroup", parameters);

            parameters.Clear();
            parameters.Add(SqlParms.CreateParameter("pin_transaction_id", DbType.Int32, transactionQueueID, true));
            parameters.Add(SqlParms.CreateParameter("pin_status_code", DbType.Int32, statusCode.Trim(), true));

            DataAccess.ExecuteStoredProcedure("usp_UpdateAffiliationSentToMMISIndicator", parameters);

            if (statusCode == Constants.MMISStatusType.Errors.ToString())
            {
                ms.ProcessDataErrors(errorCode, partyId, medicaidPk);
            }

        }

        /// <summary>
        /// Updates an individual provider's data based on a record in the MMIS
        /// response file.
        /// </summary>
        /// <param name="importTime">time the import started</param>
        /// <param name="transQueueId">transaction queue id of the record</param>
        /// <param name="providerPk">links the provider to staging table data</param>
        /// <param name="pdmsStatus">pdms status response for the record</param>
        /// <param name="medicaidId">provider's MEDICAID ID (usually comes from MMIS)</param>
        /// <param name="statusCode">status code of the MMIS update</param>
        /// <param name="errorCode">MMIS error code for the MMIS update</param>
        /// <param name="partyId">Party ID of the provider</param>
        private void UpdateIndividualByRetrievalRecord(DateTime importTime, int transQueueId, string providerPk, int pdmsStatus, string medicaidId, string statusCode, string errorCode, string partyId)
        {
            MMISShared ms = new MMISShared(this.ThreadId);
            ms.SetInternalProperties(MethodBase.GetCurrentMethod());
            // Update TRANSACTION table with Process Date information
            // -----------------------------------------------------
            TransactionController.UpdateTransactionQueue(
                transQueueId
                , null
                , importTime
                , importTime
                , Constants.appPDMSDataExchangeUserId);

            // generated by sp_Admin_StoredProcBuilder on Aug 14 2012  9:56AM
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("PROVIDER_STAGING_PK", DbType.Int32, Convert.ToInt32(providerPk), false));
            parameters.Add(SqlParms.CreateParameter("PDMS_STATUS_TYPE_ID", DbType.Int32, pdmsStatus, false));
            parameters.Add(SqlParms.CreateParameter("PROVIDER_STATUS_CHANGE_TYPE_ID", DbType.Int32, Enumerations.ProviderStatusChangeTypeId.MMIS, false));
            parameters.Add(SqlParms.CreateParameter("MEDICARE_ID", DbType.String, medicaidId.Trim(), true));
            parameters.Add(SqlParms.CreateParameter("STATUS_CODE", DbType.String, statusCode.Trim(), true));
            parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, "Processed", true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));

            DataAccess.ExecuteStoredProcedure("usp_UpdatePDMSToMMISExportedRecords", parameters);

            parameters.Clear();
            parameters.Add(SqlParms.CreateParameter("pin_transaction_id", DbType.Int32, transQueueId, true));
            parameters.Add(SqlParms.CreateParameter("pin_status_code", DbType.Int32, statusCode.Trim(), true));

            DataAccess.ExecuteStoredProcedure("usp_UpdateAffiliationSentToMMISIndicator", parameters);

            if (statusCode == Constants.MMISStatusType.Errors.ToString())
            {
                ms.ProcessDataErrors(errorCode, partyId, providerPk);
            }
        }

        private string RemoveLeadingZeroes(string inStr)
        {
            string retStr = "0";
            int startIndex = -1;
            for (int index = 0; index < inStr.Length; index++)
            {
                if (inStr[index] != '0')
                {
                    startIndex = index;
                    break;
                }
            }

            if (startIndex > -1)
            {
                retStr = inStr.Substring(startIndex);
            }

            return retStr;
        }

        private void UpdateGroup(RetrieveProvider[] records, Logging log, int transactionType)
        {
            // Retrieve added staging records
            MMISShared ms = new MMISShared(this.ThreadId);
            ms.SetInternalProperties(MethodBase.GetCurrentMethod());
            
            int countRow = 0;
            DataSet providersDS = ms.GetSubmittedStagingRecords(transactionType);
            string partyId = string.Empty;

            // loop over all records and write to database
            foreach (DataRow groupRow in providersDS.Tables[MMISShared.dbtGrps].Rows)           
            {
                try
                 {
                    // populate the object credentials and provider object
                    partyId = groupRow["PARTY_ID"].ToString();
                    string medicaidId = Methods.GetStringValue(groupRow["MEDICAID_ID"]);
                    int tqId = Convert.ToInt32(groupRow["TRANSACTION_QUEUE_ID"].ToString());
                    string medicaidPk = Methods.GetStringValue(groupRow["MEDICAID_PK"].ToString());
                    string groupPk = Methods.GetStringValue(groupRow[MMISShared.groupStagingPk].ToString());
                    string errorCode = Methods.GetStringValue(groupRow["ERROR_CODE"].ToString());
                    string statusCode = string.Empty;

                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, partyId));

                    DateTime now = DateTime.Now;
                    
                    if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                    {
                        // Retrieve from the MMIS file
                        List<RetrieveProvider> providers = records.Where(record => RemoveLeadingZeroes(record.pdmsProviderId) == partyId).ToList();

                        if (providers == null || providers.Count == 0)
                        {
                            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotFound, partyId));
                            continue;
                        }
                        else
                        {
                            foreach (RetrieveProvider provider in providers)
                            {
                                partyId = RemoveLeadingZeroes(provider.pdmsProviderId.Trim());
                                medicaidId = provider.medicaidId.Trim();
                                errorCode = provider.errorCode.Trim();
                                statusCode = provider.statusCode.Trim() == Constants.MMISRecordAccepted ? Constants.MMISStatusType.Processed.ToString() : Constants.MMISStatusType.Errors.ToString();
                                if (string.IsNullOrEmpty(errorCode))
                                {
                                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                                    parameters1.Add(SqlParms.CreateParameter("partyID", DbType.String, partyId, true));
                                    DataAccess.ExecuteStoredProcedure("usp_UpdateAffiliationStartDate", parameters1);
                                }
                                UpdateGroupByRetrievalRecord(now, tqId, groupPk, medicaidId, statusCode, errorCode, medicaidPk, partyId);
                            }

                        }
                    }
                    else
                    {
                        // get test values
                        if (++countRow <= ms.testRowsCount)
                        {
                            errorCode = AppSettings.Get("MMIS-TestGroupErrors", string.Empty);
                            statusCode = AppSettings.Get("MMIS-TestGroupStatus", Constants.MMISStatusType.Processed.ToString());
                            int medicaidLength = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));

                            if (string.IsNullOrWhiteSpace(medicaidId))
                            {
                                string medicaidPrefix = new string('1', medicaidLength - 5);
                                medicaidId = medicaidPrefix + DateTime.Now.ToString("ssfff");
                            }
                            if (string.IsNullOrEmpty(errorCode))
                            {
                                List<SqlParameter> parameters1 = new List<SqlParameter>();
                                parameters1.Add(SqlParms.CreateParameter("partyID", DbType.String, partyId, true));
                                DataAccess.ExecuteStoredProcedure("usp_UpdateAffiliationStartDate", parameters1);
                            }
                            UpdateGroupByRetrievalRecord(now, tqId, groupPk, medicaidId, statusCode, errorCode, medicaidPk, partyId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CoreException.ThrowException(this.ThreadId, ex, log.ProcessName);
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, partyId));
                }
            }
        }

        private void UpdateGroupAffiliation(RetrieveProvider[] records, Logging log)
        {
            // Retrieve added staging records
            int transactionType = Constants.TransactionType.SendGroupAffiliationstoMMIS;
            MMISShared ms = new MMISShared(this.ThreadId);
            ms.SetInternalProperties(MethodBase.GetCurrentMethod());

            DataSet providersDS = ms.GetSubmittedStagingRecords(transactionType);
            int countRow = 0;
                        
            // loop over all records and write to database
            foreach (DataRow groupRow in providersDS.Tables[MMISShared.dbtGrps].Rows)
            {
                try
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISGroupAffiliationStart, groupRow["PARTY_ID"].ToString()));
                    int transactionID = DataExchangeHelper.GetInt("TRANSACTION_QUEUE_ID", groupRow);
                    string groupPk = Methods.GetStringValue(groupRow[MMISShared.groupStagingPk].ToString());
                    string medicaidPk = Methods.GetStringValue(groupRow["MEDICAID_PK"].ToString());
                    string groupMedicaidID = Methods.GetStringValue(groupRow["MEDICAID_ID"].ToString());
                    string sakTrans = Methods.GetStringValue(groupRow["SAK_TRANS"]);

                    DateTime now = DateTime.Now;
                    string partyId;

                    foreach (DataRow affilRow in groupRow.GetChildRows(MMISShared.dbrGrps2dbtAffs))
                    {
                        string affilPDMSId = Methods.GetStringValue(affilRow["MMIS_STAGING_AFFILIATE_PK"]);
                        string errorCode = string.Empty;
                        string statusCode = string.Empty;
                        string action = string.Empty;
                        string affiliationMedicaidId = string.Empty;
                    
                        partyId = affilRow["PARTY_ID"].ToString();
                        action = affilRow["ACTION"].ToString();

                        // Retrieve from the MMIS file
                        if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                        {
                            // Retrieve from the MMIS file
                            RetrieveProvider affiliate = records.FirstOrDefault(record => Convert.ToInt32(record.transactionId) == transactionID);

                            if (affiliate == null)
                            {
                                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotFound, partyId));
                                continue;
                            }
                            else
                            {
                                //affiliationMedicaidId = affiliate.medicaidId.Trim();
                                errorCode = affiliate.errorCode.Trim();
                                statusCode = affiliate.statusCode.Trim() == Constants.MMISRecordAccepted ? Constants.MMISStatusType.Processed.ToString() : Constants.MMISStatusType.Errors.ToString();
                            }
                        }
                        else
                        {
                            // get test values
                            if (++countRow <= ms.testRowsCount)
                            {
                                statusCode = AppSettings.Get("MMIS-TestAffilStatus", Constants.MMISStatusType.Processed.ToString());
                                errorCode = AppSettings.Get("MMIS-TestAffilErrors", string.Empty);
                                if (string.IsNullOrWhiteSpace(affiliationMedicaidId))
                                {
                                    affiliationMedicaidId = "222" + DateTime.Now.ToString("ssffffff");
                                }
                            }
                        }

                        // generated by sp_Admin_StoredProcBuilder on Sep 24 2013  5:04PM
                        // create parameters objects and fill with values
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_AFFILIATE_PK", DbType.Int32, affilPDMSId, true));
                        //parameters.Add(SqlParms.CreateParameter("AFFILIATION_MEDICAID_ID", DbType.String, affiliationMedicaidId, true));
                        parameters.Add(SqlParms.CreateParameter("MMIS_STATUS_TYPE_ID", DbType.Int32, statusCode, true));
                        parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, errorCode, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                        DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingAffiliate", parameters);

                        // Update TRANSACTION table with Process Date information
                        int transQueueId = Convert.ToInt32(affilRow["TRANSACTION_QUEUE_ID"].ToString());

                        // -----------------------------------------------------
                        TransactionController.UpdateTransactionQueue(
                            transQueueId
                            , null
                            , now
                            , now
                            , Constants.appPDMSDataExchangeUserId);

                        // generated by sp_Admin_StoredProcBuilder on Nov 18 2013  8:40PM
                        // create parameters objects and fill with values
                        parameters = new List<SqlParameter>();

                        parameters.Add(SqlParms.CreateParameter("IndProvPartyID", DbType.Int32, partyId, true));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                        DataAccess.ExecuteStoredProcedure("usp_UpdateAffiliations", parameters);

                        int affilPartyId = ms.GetAffiliatePartyId(affilPDMSId);
                        if (statusCode == Constants.MMISStatusType.Processed.ToString() && action == "AM")
                        {
                            
                            string recipients = string.Empty;
                            string userEmail = ObjectControllerHelper.GetString("Email", groupRow);
                            string contactEmail = ObjectControllerHelper.GetString("CONTACT_EMAIL_ADDRESS", groupRow);
                            bool hasTwo = (!string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(contactEmail));
                            bool isDuplicate = userEmail.Equals(contactEmail, StringComparison.InvariantCultureIgnoreCase);
                            recipients = userEmail + (!isDuplicate && hasTwo ? "," : string.Empty) + (isDuplicate ? string.Empty : contactEmail);
                            string subject = "Affiliation Confirmation";
                            //bool rtn = false;
                            Dictionary<string, object> fields = new Dictionary<string, object>();
                            fields.Add("CURRENTDATE", Methods.GetShortDate(DateTime.Now.ToString()));
                            fields.Add("LEGALNAME", ObjectControllerHelper.GetString("NAME", groupRow));
                            fields.Add("PRIMARYCONTACTNAME", ObjectControllerHelper.GetString("ServicingAddressName", groupRow));
                            fields.Add("GROUPNPI", ObjectControllerHelper.GetString("NPI", groupRow));
                            fields.Add("PRIMARYCONTACTADDRESS1", ObjectControllerHelper.GetString("ServicingAddress1", groupRow));
                            fields.Add("GROUPMEDICAID", ObjectControllerHelper.GetString("MEDICAID_ID", groupRow));
                            fields.Add("PRIMARYCONTACTADDRESS2", ObjectControllerHelper.GetString("ServicingAddress2", groupRow));
                            fields.Add("EFFECTIVEDATE", Methods.GetShortDate(ObjectControllerHelper.GetString("EFFECTIVE_DATE", groupRow)));
                            fields.Add("GROUPCITYSTATEZIP", ObjectControllerHelper.GetString("ServicingCity", groupRow) + ", " + ObjectControllerHelper.GetString("ServicingState", groupRow) + " " + ObjectControllerHelper.GetString("ServicingZip", groupRow));
                            fields.Add("PDMSURL", AppSettings.Get("PDMSURL"));
                            fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL",string.Empty));

                            string entityTypeId = "";
                            entityTypeId = ObjectControllerHelper.GetString("ProviderCategoryTypeID", groupRow);

                            if (entityTypeId == "2")
                            {
                                fields.Add("NPILabel", "Type 2 NPI");
                                fields.Add("GroupMedicaidLabel", "Group ");
                                fields.Add("grouplabel", AppSettings.Get("group "));
                                fields.Add("groupOrPracticeLabel", "group");
                            }
                            else
                            {
                                fields.Add("NPILabel", "NPI");
                                fields.Add("GroupMedicaidLabel", "");
                                fields.Add("grouplabel", AppSettings.Get("group "));
                                fields.Add("groupOrPracticeLabel", "practice");
                            }
                             
                            //int logCnt = 0;
                            //Send email for each affiliated individual
                            if (ObjectControllerHelper.HasRows(providersDS.Tables[2]) && !string.IsNullOrEmpty(affilPartyId.ToString()))
                            {
                                //Get individual level data by part_id
                                // DataRow[] rows = group.Tables[2].Select("PARTY_ID = " + affilPartyId.ToString());
                                DataRow[] rows = groupRow.GetChildRows(MMISShared.dbrGrps2dbtAffs);
                                foreach (DataRow row in rows)
                                {
                                    if (affilPartyId.ToString() == ObjectControllerHelper.GetString("PARTY_ID", row))
                                    {
                                        //set field value if it exits. otherwise, add it
                                        if (fields.ContainsKey("INDIVIDUALPROVIDERNAME"))
                                        {
                                            fields["INDIVIDUALPROVIDERNAME"] = ObjectControllerHelper.GetString("ProviderName", row);
                                            fields["INDIVIDUALNPI"] = ObjectControllerHelper.GetString("NPI", row);
                                            fields["INDIVIDUALMEDICAIDID"] = ObjectControllerHelper.GetString("Medicaid_ID", row);
                                            fields["AFFILIATIONDATE"] = Methods.GetShortDate(ObjectControllerHelper.GetString("AffiliationDate", row));
                                        }
                                        else
                                        {
                                            fields.Add("INDIVIDUALPROVIDERNAME", ObjectControllerHelper.GetString("ProviderName", row));
                                            fields.Add("INDIVIDUALNPI", ObjectControllerHelper.GetString("NPI", row));
                                            fields.Add("INDIVIDUALMEDICAIDID", ObjectControllerHelper.GetString("Medicaid_ID", row));
                                            fields.Add("AFFILIATIONDATE", Methods.GetShortDate(ObjectControllerHelper.GetString("AffiliationDate", row)));
                                            //recipients = ObjectControllerHelper.GetString("Recipient", row);
                                        }
                                        List<SqlParameter> prms = new List<SqlParameter>();
                                        prms.Add(new SqlParameter("@PartyID", affilPartyId));
                                        DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByPartyID", prms,"REG_DS");
                                        string regId = "";
                                        if(ObjectControllerHelper.HasRows(ds))
                                        {
                                            regId = ds.Tables[0].Rows[0]["RegId"].ToString();
                                        }
                                        string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty) + @"\";

                                        if (System.Diagnostics.Debugger.IsAttached)
                                        {
                                            templateActualPath = @"C:\Users\rmays\Desktop\Development\PDMS\PDMS\PDMS\ProviderDataManagementSystemService\Documents\";
                                        }

                                        //bool paperMail = false;
                                        string Template_Name = @"AffiliationConfirmationLetter.txt";
                                        int adminPartyID = GetAdminPartyId();
                                        if (ObjectControllerHelper.RequirePaperNotice(recipients, DataAccess.GetAppSetting("PSEmailTypes").Split(',')))
                                        {
                                            PaperNotification notify = new PaperNotification(subject, this.ThreadId);
                                            notify.SendNotification(templateActualPath + Template_Name, fields, true);
                                            notify.CreateCommunicationEvent( fields, new Guid(), "0", Template_Name);
                                        }
                                        else
                                        {
                                            EMailNotification notify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId);
                                            notify.SendNotification(templateActualPath + Template_Name, fields, true);
                                            notify.CreateCommunicationEvent(fields, new Guid(), "0", Template_Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    

                    //// generated by sp_Admin_StoredProcBuilder on Sep 12 2013 11:07AM
                    //// create parameters objects and fill with values
                    //List<SqlParameter> parameters = new List<SqlParameter>();
                    //parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_GROUP_PK", DbType.Int32, groupPk, true));
                    //parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, sakTrans, true));
                    //parameters.Add(SqlParms.CreateParameter("CORRELATION_ID", DbType.String, correlationId, true));
                    //parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, errorCodes, true));
                    //parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, true));
                    //parameters.Add(SqlParms.CreateParameter("STATUS_CODE", DbType.Int32, statusCode, true));
                    //parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    //parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                    //DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingGroup", parameters);
                }
                catch (Exception ex)
                {
                    CoreException.ThrowException(this.ThreadId, ex, log.ProcessName);
                    // todo write to log
                }
            }
        }
        private void UpdateMCOAffiliation(RetrieveProvider[] records, Logging log)
        {
            // Retrieve added staging records
            MMISShared ms = new MMISShared(this.ThreadId);
            ms.SetInternalProperties(MethodBase.GetCurrentMethod());

            DataSet providersDS = ms.GetSubmittedMCOStagingRecords();

            // loop over all records and write to database
            foreach (DataRow affilRow in providersDS.Tables[0].Rows)
            {
                try
                {
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISGroupAffiliationStart, affilRow["MCO_PARTY_ID"].ToString()));
                    int transactionID = DataExchangeHelper.GetInt("TRANSACTION_QUEUE_ID", affilRow);
                    int mcoAffiliationID = DataExchangeHelper.GetInt("MMIS_STAGING_MCO_AFFILIATE_ID", affilRow);
                    string mcoMedicaidID = Methods.GetStringValue(affilRow["MCO_MEDICAID_ID"].ToString());

                    DateTime now = DateTime.Now;
                    string mcoPartyId;
                    int indivPDMSId = Methods.GetIntValue(affilRow["MEMBER_PARTY_ID"]);
                    string errorCode = string.Empty;
                    string statusCode = string.Empty;
                    string action = string.Empty;
                    string affiliationMedicaidId = string.Empty;

                    mcoPartyId = affilRow["MCO_PARTY_ID"].ToString();
                    action = affilRow["ACTION_CODE"].ToString();
                    if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
                    {
                    // Retrieve from the MMIS file
                    RetrieveProvider affiliate = records.FirstOrDefault(record => Convert.ToInt32(record.transactionId) == transactionID);

                    if (affiliate == null)
                    {
                        log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotFound, mcoPartyId));
                        continue;
                    }
                    else
                    {
                        errorCode = affiliate.errorCode.Trim();
                        statusCode = affiliate.statusCode.Trim() == Constants.MMISRecordAccepted ? Constants.MMISStatusType.Processed.ToString() : Constants.MMISStatusType.Errors.ToString();
                    }
                    }
                    else
                    {
                        statusCode = AppSettings.Get("MMIS-TestAffilStatus", Constants.MMISStatusType.Processed.ToString());
                        errorCode = AppSettings.Get("MMIS-TestAffilErrors", string.Empty);
                    }
                    // generated by sp_Admin_StoredProcBuilder on Sep 24 2013  5:04PM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("mmis_staging_mco_affiliate_ID", DbType.Int32, mcoAffiliationID, true));
                    parameters.Add(SqlParms.CreateParameter("status_code", DbType.Int32, statusCode, true));
                    parameters.Add(SqlParms.CreateParameter("error_code", DbType.String, errorCode, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

                    DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingMCOAffiliate", parameters);

                    // -----------------------------------------------------
                    TransactionController.UpdateTransactionQueue(
                        transactionID
                        , null
                        , now
                        , now
                        , Constants.appPDMSDataExchangeUserId);

                    if (statusCode == Constants.MMISStatusType.Processed.ToString() && action == "AM")
                    {
                        DataRow mcoAffilNotificationDR = ms.GetMCOAffiliationNotificationInfo(mcoAffiliationID);
                        string recipients = string.Empty;
                        string userEmail = ObjectControllerHelper.GetString("GROUP_Email", mcoAffilNotificationDR);
                        string contactEmail = ObjectControllerHelper.GetString("GROUP_CONTACT_EMAIL_ADDRESS", mcoAffilNotificationDR);
                        bool hasTwo = (!string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(contactEmail));
                        bool isDuplicate = userEmail.Equals(contactEmail, StringComparison.InvariantCultureIgnoreCase);
                        recipients = userEmail + (!isDuplicate && hasTwo ? "," : string.Empty) + (isDuplicate ? string.Empty : contactEmail);
                        string subject = "Affiliation Confirmation";
                        Dictionary<string, object> fields = new Dictionary<string, object>();
                        fields.Add("CURRENTDATE", Methods.GetShortDate(DateTime.Now.ToString()));
                        fields.Add("LEGALNAME", ObjectControllerHelper.GetString("GROUP_NAME", mcoAffilNotificationDR));
                        fields.Add("PRIMARYCONTACTNAME", ObjectControllerHelper.GetString("GROUP_ServicingAddressName", mcoAffilNotificationDR));
                        fields.Add("GROUPNPI", ObjectControllerHelper.GetString("GROUP_NPI", mcoAffilNotificationDR));
                        fields.Add("PRIMARYCONTACTADDRESS1", ObjectControllerHelper.GetString("GROUP_ServicingAddress1", mcoAffilNotificationDR));
                        fields.Add("GROUPMEDICAID", ObjectControllerHelper.GetString("GROUP_MEDICAID_ID", mcoAffilNotificationDR));
                        fields.Add("PRIMARYCONTACTADDRESS2", ObjectControllerHelper.GetString("GROUP_ServicingAddress2", mcoAffilNotificationDR));
                        fields.Add("EFFECTIVEDATE", Methods.GetShortDate(ObjectControllerHelper.GetString("GROUP_EFFECTIVE_DATE", mcoAffilNotificationDR)));
                        fields.Add("GROUPCITYSTATEZIP", ObjectControllerHelper.GetString("GROUP_ServicingCity", mcoAffilNotificationDR) + ", " + ObjectControllerHelper.GetString("GROUP_ServicingState", mcoAffilNotificationDR) + " " + ObjectControllerHelper.GetString("GROUP_ServicingZip", mcoAffilNotificationDR));
                        //fields.Add("PDMSURL", AppSettings.Get("PDMSURL"));
                        fields.Add("PDMSURL", AppSettings.Get("PDMS-URL", string.Empty));
                        fields.Add("PDMSEMAIL", AppSettings.Get("PDMSEMAIL", string.Empty));

                        string entityTypeId = "";
                        entityTypeId = ObjectControllerHelper.GetString("GROUP_ProviderCategoryTypeID", mcoAffilNotificationDR);

                        if (entityTypeId == "2")
                        {
                            fields.Add("NPILabel", "Type 2 NPI");
                            fields.Add("GroupMedicaidLabel", "Group ");
                            fields.Add("grouplabel", AppSettings.Get("group "));
                            fields.Add("groupOrPracticeLabel", "group");
                        }
                        else
                        {
                            fields.Add("NPILabel", "NPI");
                            fields.Add("GroupMedicaidLabel", "");
                            fields.Add("grouplabel", AppSettings.Get("group "));
                            fields.Add("groupOrPracticeLabel", "practice");
                        }

                        //int logCnt = 0;
                        //Send email for each affiliation

                        if (indivPDMSId.ToString() == ObjectControllerHelper.GetString("INDIV_PARTY_ID", mcoAffilNotificationDR))
                        {
                            //set field value if it exits. otherwise, add it
                            if (fields.ContainsKey("INDIVIDUALPROVIDERNAME"))
                            {
                                fields["INDIVIDUALPROVIDERNAME"] = ObjectControllerHelper.GetString("INDIV_ProviderName", mcoAffilNotificationDR);
                                fields["INDIVIDUALNPI"] = ObjectControllerHelper.GetString("INDIV_NPI", mcoAffilNotificationDR);
                                fields["INDIVIDUALMEDICAIDID"] = ObjectControllerHelper.GetString("INDIV_Medicaid_ID", mcoAffilNotificationDR);
                                fields["AFFILIATIONDATE"] = Methods.GetShortDate(ObjectControllerHelper.GetString("AffiliationDate", mcoAffilNotificationDR));
                            }
                            else
                            {
                                fields.Add("INDIVIDUALPROVIDERNAME", ObjectControllerHelper.GetString("INDIV_ProviderName", mcoAffilNotificationDR));
                                fields.Add("INDIVIDUALNPI", ObjectControllerHelper.GetString("INDIV_NPI", mcoAffilNotificationDR));
                                fields.Add("INDIVIDUALMEDICAIDID", ObjectControllerHelper.GetString("INDIV_Medicaid_ID", mcoAffilNotificationDR));
                                fields.Add("AFFILIATIONDATE", Methods.GetShortDate(ObjectControllerHelper.GetString("AffiliationDate", mcoAffilNotificationDR)));
                                //recipients = ObjectControllerHelper.GetString("Recipient", row);
                            }
                            string strFromAddress = AppSettings.Get("Mail-FromAddress", string.Empty);
                            fields.Add("FROMADDRESS", strFromAddress);
                            List<SqlParameter> prms = new List<SqlParameter>();
                            prms.Add(new SqlParameter("@PartyID", indivPDMSId));
                            DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByPartyID", prms, "REG_DS");
                            string regId = "";
                            if (ObjectControllerHelper.HasRows(ds))
                            {
                                regId = ds.Tables[0].Rows[0]["RegId"].ToString();


                                List<SqlParameter> parms = new List<SqlParameter>();
                                parms.Add(new SqlParameter("REG_ID", regId));
                                DataSet dsProv = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms, "dsProv");
                                bool hasRows = dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0;

                                if (Methods.HasRows(dsProv))
                                {
                                    DataRow Provrow = dsProv.Tables[0].Rows[0];
                                    string ToName = Provrow["CONTACT_NAME"].ToString() == "" ? Provrow["NAME"].ToString() : Provrow["CONTACT_NAME"].ToString();
                                    fields.Add("ToName", ToName);
                                    fields.Add("Address1", Provrow["CONTACT_ADDRESS1"].ToString() + " " + Provrow["CONTACT_QUADRANT"].ToString());
                                    fields.Add("Address2", Provrow["CONTACT_ADDRESS2"].ToString());
                                    fields.Add("CityState", Provrow["CONTACT_CITY"].ToString() + ", " +
                                        Provrow["CONTACT_STATE"].ToString() + " " + Provrow["CONTACT_ZIP"].ToString() +
                                        (!string.IsNullOrEmpty(Provrow["CONTACT_EXT_ZIP"].ToString()) ? "-" + Provrow["CONTACT_EXT_ZIP"].ToString() : string.Empty));
                                }
                            }



                            string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty) + @"\";

                            if (System.Diagnostics.Debugger.IsAttached)
                            {
                                templateActualPath = @"C:\DCPDMS\projects\DCPDMS\PDMS\ProviderDataManagementSystemService\Documents\";
                            }

                            //bool paperMail = false;
                            string Template_Name = @"AffiliationConfirmationLetter.txt";
                            int adminPartyID = GetAdminPartyId();
                            if (ObjectControllerHelper.RequirePaperNotice(recipients, DataAccess.GetAppSetting("PSEmailTypes").Split(',')))
                            {
                                PaperNotification notify = new PaperNotification(subject, this.ThreadId);
                                notify.SendNotification(templateActualPath + Template_Name, fields, true);
                                notify.CreateCommunicationEvent(fields, new Guid(), "0", Template_Name);
                            }
                            else
                            {
                                EMailNotification notify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId);
                                notify.SendNotification(templateActualPath + Template_Name, fields, true);
                                notify.CreateCommunicationEvent(fields, new Guid(), "0", Template_Name);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CoreException.ThrowException(this.ThreadId, ex, log.ProcessName);
                    // todo write to log
                }
            }
        }
       public static int GetAdminPartyId()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string rtn = DataAccess.ExecuteScalar("sp_SelectAdminPartyID", parameters);
            if (string.IsNullOrEmpty(rtn)) return 0;
            return Convert.ToInt32(rtn);
        }
        private int AffiliationLetterCount(int groupPartyId, int affilPartyId)
        {

            int emailCount = 0;

            // generated by sp_Admin_StoredProcBuilder on Jul 15 2014  8:52AM
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("GROUP_PARTY_ID", DbType.Int32, groupPartyId, true));
            parameters.Add(SqlParms.CreateParameter("AFFIL_PARTY_ID", DbType.Int32, affilPartyId, true));

            emailCount = Methods.GetIntValue(DataAccess.ExecuteScalar("usp_ValidateAffiliationEmail", parameters));

            return emailCount;
        }
        private void SubmitMCOAffiliation(FileHelpers.FixedFileEngine engine, string fullFileName)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string providerPK = string.Empty;
            DateTime now = DateTime.Now;

            try
            {
                // get the staging records
                MMISShared ms = new MMISShared(this.ThreadId);
                ms.SetInternalProperties(MethodBase.GetCurrentMethod());
                // Populate the staging tables
                ms.PopulateMCOStagingData();
                // get the staging records
                DataSet affiliates = ms.GetUnsubmittedMCORecords();

                // Log entry
                log.CreateLogEntry(String.Format(Constants.LogString.SumittingMMISGroupAffiliationStart, affiliates.Tables[0].Rows.Count));

                // create the object to submit
                SubmitProvider mcoAffiliate = new SubmitProvider();

                foreach (DataRow affiliate in affiliates.Tables[0].Rows)
                {
                    int mcoAffiliationStagingID = Convert.ToInt32(affiliate["MMIS_STAGING_MCO_AFFILIATE_ID"].ToString());
                    int transQueueId = Convert.ToInt32(affiliate["TRANSACTION_QUEUE_ID"].ToString());
                    int affiliatePartyId = Convert.ToInt32(affiliate["MEMBER_PARTY_ID"].ToString());
                    int mcoPartyId = Convert.ToInt32(affiliate["MCO_PARTY_ID"].ToString());

                    // Log entry
                    log.CreateLogEntry(String.Format(Constants.LogString.SumittingMMISMCOAffiliationGroupStart, mcoPartyId, affiliatePartyId));

                    mcoAffiliate.max_trans_id = transQueueId.ToString("0000000000");
                    mcoAffiliate.max_action = DataExchangeHelper.GetString("ACTION_CODE", affiliate); ;
                    mcoAffiliate.max_pdms_id = mcoPartyId.ToString("0000000000");
                    mcoAffiliate.max_member_pdms_id = affiliatePartyId.ToString("0000000000");
                    mcoAffiliate.id = DataExchangeHelper.GetString("MCO_MEDICAID_ID", affiliate);
                    mcoAffiliate.member_id = DataExchangeHelper.GetString("MEMBER_MEDICAID_ID", affiliate);

                    if (!String.IsNullOrWhiteSpace(affiliate["START_DATE"].ToString()))
                    {
                        mcoAffiliate.affl_beg_dt = Methods.GetDateValue(affiliate["START_DATE"]);
                    }
                    if (mcoAffiliate.max_action == "AM")
                    {
                        mcoAffiliate.affl_end_dt = new DateTime(9999, 12, 31);
                    }
                    else
                    {
                        mcoAffiliate.affl_end_dt = SetEndDate(mcoAffiliate.affl_beg_dt, Methods.GetDateValue(affiliate["END_DATE"]));
                    }

                    mcoAffiliate.aff_ssn_num = affiliate["MEMBER_SSN"].ToString();
                    DataRowCollection enrollmentInfo = MMISShared.GetEnrollmentInfo(affiliatePartyId);
                    if (enrollmentInfo != null)
                    {
                        string enrollStatus = DataExchangeHelper.GetString("EnrollmentStatusCode", enrollmentInfo[0]);
                        mcoAffiliate.enrol_stat_ty_cd = (enrollStatus == "90" ? "00" : enrollStatus);
                    }
                    mcoAffiliate.affl_ty_cd = "P";

                    // fill in the multi-valued variables to keep FileHelper happy
                    FillMultivaluesWithBlanks(mcoAffiliate);

                    //  append the record to the file
                    engine.AppendToFile(fullFileName, mcoAffiliate);

                    // Update TRANSACTION table with Submit Date information
                    // -----------------------------------------------------
                    TransactionController.UpdateTransactionQueue(
                            transQueueId
                            , now
                            , null
                            , now
                            , Constants.appPDMSDataExchangeUserId);

                    // Log entry
                    log.CreateLogEntry(Constants.LogString.SumittingMMISIndividualAffiliateEnd);

                    // generated by sp_Admin_StoredProcBuilder on Sep 12 2013 11:07AM
                    // create parameters objects and fill with values
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_MCO_AFFILIATE_ID", DbType.Int32, mcoAffiliationStagingID, true));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));
                    parameters.Add(SqlParms.CreateParameter("SUBMITTED_TO_INTERFACE_FILE", DbType.Boolean, true, true));

                    DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingMCOAffiliate", parameters);

                    log.CreateLogEntry(String.Format(Constants.LogString.SumittingMMISGroupAffiliationGroupEnd, mcoPartyId));
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord(log, "MMIS Submit Exception", MMISShared.dbtGrps, providerPK, ex.Message);
            }

            // Log entry
            log.CreateLogEntry(Constants.LogString.SumittingMMISGroupAffiliationEnd);
        }


        #region "Not used for DC PDMS"
        //private void UpdateAdditionalSpecialties(RetrieveProvider[] records, Logging log, int transactionType)
        //{
        //    // Retrieve added staging records
        //    MMISShared ms = new MMISShared(this.ThreadId);
        //    ms.SetInternalProperties(MethodBase.GetCurrentMethod());

        //    int countRow = 0;
        //    DataSet ds = ms.GetSubmittedStagingRecords(transactionType);
        //    string partyID = string.Empty;

        //    if (!ObjectControllerHelper.HasRows(ds))
        //        return;

        //    // loop over all records and write to database
        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        try
        //        {
        //            // populate the object credentials and provider object
        //            partyID = row["PARTY_ID"].ToString();
        //            int transactionQueueID = ObjectControllerHelper.GetInt("TRANSACTION_QUEUE_ID", row);
        //            int stagingRecID = ObjectControllerHelper.GetInt("MMIS_STAGING_SPECIALTY_ID", row);
        //            string errorCode = ObjectControllerHelper.GetString("ERROR_CODE", row);
        //            int statusCode = 0;

        //            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, partyID));

        //            DateTime now = DateTime.Now;

        //            if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
        //            {
        //                // Retrieve from the MMIS file
        //                RetrieveProvider provider = records.FirstOrDefault(record => record.pdmsProviderId == partyID);
        //                if (provider == null)
        //                {
        //                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordNotFound, partyID));
        //                    continue;
        //                }
        //                else
        //                {
        //                    partyID = provider.pdmsProviderId.Trim();
        //                    errorCode = provider.errorCode.Trim();
        //                    statusCode = provider.statusCode.Trim() == Constants.MMISRecordAccepted ? Constants.MMISStatusType.Processed : Constants.MMISStatusType.Errors;
        //                }
        //            }
        //            else
        //            {
        //                // get test values
        //                if (++countRow <= ms.testRowsCount)
        //                {
        //                    errorCode = AppSettings.Get("MMIS-TestGroupErrors", string.Empty);
        //                    statusCode = Convert.ToInt32(AppSettings.Get("MMIS-TestGroupStatus", Constants.MMISStatusType.Processed.ToString()));

        //                }
        //            }

        //            // Update TRANSACTION table with Process Date information
        //            // -----------------------------------------------------
        //            TransactionController.UpdateTransactionQueue(
        //                transactionQueueID
        //                , null
        //                , now
        //                , now
        //                , Constants.appPDMSDataExchangeUserId);

        //            List<SqlParameter> parameters = new List<SqlParameter>();

        //            parameters.Add(SqlParms.CreateParameter("MMIS_STAGING_SPECIALTY_ID", DbType.Int32, stagingRecID, true));
        //            parameters.Add(SqlParms.CreateParameter("SAK_TRANS", DbType.String, "Processed", true));
        //            parameters.Add(SqlParms.CreateParameter("MMIS_STATUS_TYPE_ID", DbType.Int32, statusCode, true));
        //            parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, errorCode, true));
        //            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, false));
        //            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, false));

        //            DataAccess.ExecuteStoredProcedure("usp_UpdateMMISStagingSpecialty", parameters);

        //            //TBD:  how to handle errors for additional specialties
        //            //if (statusCode == Constants.MMISStatusType.Errors)
        //            //{
        //            //    ms.ProcessDataErrors(errorCode, partyID, medicaidPk);
        //            //}
        //        }
        //        catch (Exception ex)
        //        {
        //            CoreException.ThrowException(this.ThreadId, ex, log.ProcessName);
        //            log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecord, partyID));
        //        }
        //    }
        //}
        #endregion
        #endregion "Private Methods"
    }
}
