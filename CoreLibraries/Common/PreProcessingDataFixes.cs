using Corp.Core.Libraries.IntelligentAddressVerification;
using Corp.Core.Libraries.ServiceAgent;
using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace Corp.Core.Libraries
{
    public static class PreProcessingDataFixes
    {

        public static void PreProcessingDataFixesMain(Logging log, int transactionID)
        {
            try
            {
                DataSet dsTXn = InfoAccessController.GetRegIDfromTransactionID(transactionID);
                DataRow drTxn = Methods.HasRows(dsTXn) ? dsTXn.Tables[0].Rows[0] : null;
                int regID = ProviderManagementHelper.GetInt("REG_ID", drTxn);
                CountyCodeFixByRegID(log, regID);
                DeleteDuplicateAddresses(log, regID);
                UpdateRegAffiliations(log, regID);
                UpdateBoardCertification(log, regID);
                InsertAdditionalApplicationIdIfNotExistsInWFPARAMETER(log, regID);
                UpdateRegCLIA(log, regID);
                UpdateRegSpecialty(log, regID);
                UpdateRegAddress(log, regID);
            }
            catch (Exception Ex)
            {
                log.CreateLogEntry(string.Format("Error in PreProcessingDataFixesMain {0} ", Ex));
            }
        }

        public static void CountyCodeFixByRegID(Logging log, int regID)
        {
            try
            {
                int regAddrID = 0;
                string county = "";
                string origStreetAddress;
                string origUnitAddress;
                string origCity;
                string origState;
                string origZip5;
                string origZip4;
				int addressTypeId;

                DataSet dsReg = InfoAccessController.SelectRegAddressesWithoutCountyByRegID(regID, 1);
                DataTable dtReg = Methods.HasRows(dsReg) ? dsReg.Tables[0] : null;

                if (dtReg != null)
                {
                    foreach (DataRow row in dtReg.Rows)
                    {
                        regAddrID = ProviderManagementHelper.GetInt("REG_ADDRESS_ID", row);
                        origStreetAddress = ProviderManagementHelper.GetString("ADDRESS1", row);
                        origUnitAddress = ProviderManagementHelper.GetString("ADDRESS2", row);
                        origCity = ProviderManagementHelper.GetString("CITY", row);
                        origState = ProviderManagementHelper.GetString("STATE", row);
                        origZip5 = ProviderManagementHelper.GetString("ZIP", row);
                        origZip4 = ProviderManagementHelper.GetString("EXT_ZIP", row);
						addressTypeId = ProviderManagementHelper.GetInt("ADDRESS_TYPE_ID", row);

                        var searchAgent = new IntelligentSearchAgent();
                        
                        var request = new AddressVerificationRequest()
                        {
                            AddressLine = origStreetAddress + ", ",
                            AddressLine2 = origUnitAddress,
                            City = origCity,
                            State = origState,
                            PostalCode = origZip5 + "-" + origZip4
                        };
                        var result = searchAgent.GetAddressVerificatonWS(request);
                        
                        if (result != null && !string.IsNullOrEmpty(result.CountyNumber) && !string.IsNullOrWhiteSpace(result.CountyNumber) && !string.IsNullOrEmpty(result.State) && !string.IsNullOrWhiteSpace(result.State))
                        {
                            county = result.CountyNumber;
                            origState = result.State;

                            InfoAccessController.UpdateRegAddressesWithoutCountyByRegID(regID, regAddrID, county, origState, 3);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                log.CreateLogEntry(string.Format("Error in CountyCodeFixByRegID {0} ", Ex));
            }
        }

        public static void DeleteDuplicateAddresses(Logging log, int regID)
        {
            try
            {
                InfoAccessController.DeleteDuplicateAddresses(regID);
            }
            catch (Exception Ex)
            {
                log.CreateLogEntry(string.Format("Error in DeleteDuplicateAddresses {0} ", Ex));
            }
        }

        /// <summary>
        /// Update RegAffiliation's GROUP_AFFILIATION_STATUS_ID to 10 when StartDate greater than EndDate
        /// to prevent it from going to staging table
        /// </summary>
        /// <param name="log"></param>
        /// <param name="regID"></param>
        public static void UpdateRegAffiliations(Logging log, int regID)
        {
            try
            {
                InfoAccessController.UpdateRegAffiliations(regID);
            }
            catch (Exception Ex)
            {
                log.CreateLogEntry(string.Format("Error in UpdateRegAffiliations {0} ", Ex));
            }
        }

        /// <summary>
        /// Insert Additional_Application_Id If Not Exists In WF_PARAMETER table
        /// </summary>
        /// <param name="log"></param>
        /// <param name="regID"></param>
        public static void InsertAdditionalApplicationIdIfNotExistsInWFPARAMETER(Logging log, int regID)
        {
            try
            {
                InfoAccessController.InsertAdditionalApplicationIdIfNotExistsInWFPARAMETER(regID);
            }
            catch (Exception Ex)
            {
                log.CreateLogEntry(string.Format("Error in InsertAdditionalApplicationIdIfNotExistsInWFPARAMETER {0} ", Ex));
            }
        }

        public static void UpdateRegCLIA(Logging log, int regID)
        {
            try
            {
                InfoAccessController.UpdateRegCLIA(regID);
            }
            catch (Exception Ex)
            {
                log.CreateLogEntry(string.Format("Error in UpdateRegCLIA {0} ", Ex));
            }
        }
        /// <summary>
        /// Update RegSpecialty and RegSpecialtyHistory's  Start_Date greater than End_Date
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="regID"></param>
        public static void UpdateRegSpecialty(Logging log, int regID)
        {
            try
            {
                InfoAccessController.UpdateRegSpecialty(regID);
            }
            catch (Exception Ex)
            {
                log.CreateLogEntry(string.Format("Error in UpdateRegSpecialty {0} ", Ex));
            }
        }
        /// <summary>
        /// Update UpdateBoardCertification's  Effective_Date greater than Expiration_Date
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="regID"></param>
        public static void UpdateBoardCertification(Logging log, int regID)
        {
            try
            {
                InfoAccessController.UpdateBoardCertification(regID);
            }
            catch (Exception Ex)
            {
                log.CreateLogEntry(string.Format("Error in UpdateBoardCertification {0} ", Ex));
            }
        }

        public static void UpdateRegAddress(Logging log, int regID)
        {
            try
            {
                InfoAccessController.UpdateRegAddress(regID);
            }
            catch (Exception Ex)
            {
                log.CreateLogEntry(string.Format("Error in UpdateRegAddress {0} ", Ex));
            }
        }
    }
}
