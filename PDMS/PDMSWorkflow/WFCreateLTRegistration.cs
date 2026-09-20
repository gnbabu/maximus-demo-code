using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    /// <summary>
    /// Summary description for WFCheckForRetroReview
    /// </summary>
    public class WFCreateLTRegistration : BaseWorkflowTask, IWorkflowTask
    {
        private string nextStep;

        public WFCreateLTRegistration(int processID, int stepID)
            : base(processID, stepID)
        {
            //
            // TODO: Add constructor logic here
            //
        }
        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            int registrationID;
            try
            {
                Process p = new Process(ProcessID);
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out registrationID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, registrationID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegID", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;               

                if (drReg == null)
                {
                    throw new Exception(string.Format(Constants.LogString.WorkflowError, Assembly.GetExecutingAssembly().GetName().Name,
                                "Unable to retrieve registration using usp_SelectREGISTRATIONByRegID. (Testing)"));
                }
                else
                {
                    string MMISProviderType = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);               
                    int applicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                    string providerName = ObjectControllerHelper.GetString("ProviderName", drReg);
                    string dba = ObjectControllerHelper.GetString("DBA", drReg);
                    string firstName = ObjectControllerHelper.GetString("FirstName", drReg);
                    string middleInitial = ObjectControllerHelper.GetString("MiddleInitial", drReg);
                    string lastName = ObjectControllerHelper.GetString("LastName", drReg);
                    
                    DateTime? birthDate = null;
                    string gender = ObjectControllerHelper.GetString("Gender", drReg);
                    int practiceTypeCodeID = ObjectControllerHelper.GetInt("PRACTICE_TYPE_ID", drReg);
                    int ownershipTypeCodeID = ObjectControllerHelper.GetInt("OWNERSHIP_TYPE_ID", drReg);
                    int taxIDTypeID = ObjectControllerHelper.GetInt("TaxIDTypeID",drReg);
                    string taxID = ObjectControllerHelper.GetString("TaxID", drReg);
                    string npi = ObjectControllerHelper.GetString("NPI", drReg);
                    // Get the Provider Type ID from MMIS Provider Type ID
                    DataSet dsProviderType = ProviderController.SelectProviderTypesByMMISProviderTypeID(CON.LTCFaciitiesProviderTypeID.BuildingandWingIDforLTCFacilitesOnly);
                    dsProviderType.Tables[0].TableName = "ProviderTypeData";
                    DataRow drPT = ObjectControllerHelper.HasRows(dsProviderType) ? dsProviderType.Tables[0].Rows[0] : null;
                    int providerTypeID = ObjectControllerHelper.GetInt("PROVIDER_TYPE_ID", drPT);

                    int providerCategoryTypeID = ObjectControllerHelper.GetInt("ProviderCategoryTypeID",drReg);
                    int practiceTypeID = ObjectControllerHelper.GetInt("TypeOfPracticeID",drReg);
                    int registrationStatusTypeId = ObjectControllerHelper.GetInt("RegistrationStatusTypeID",drReg);
                    int referralID = ObjectControllerHelper.GetInt("ReferralNumber", drReg);
                    int specialtyTypeID = ObjectControllerHelper.GetInt("SpecialtyTypeID",drReg);
                    int taxonomyTypeID = ObjectControllerHelper.GetInt("TaxonomyTypeID", drReg);
                    string practiceName = "";
                    string zipCode = ObjectControllerHelper.GetString("ZipCode",drReg);
                    string zipExt = ObjectControllerHelper.GetString("ZipExt", drReg);
                    bool isPaperApplication = false;
                    int paperRequestQueueID = ObjectControllerHelper.GetInt("PaperRequestQueueID",drReg);

                    DateTime createDateTime = DateTime.Now;
                    Guid createdby = new Guid(CON.appAdminUserId);
                    int workflowID = 0;
                    int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                    int waiverTypeID = 0;
                    int facilityLTRegID = 0;
                    Guid userID = new Guid(ObjectControllerHelper.GetString("UserID", drReg));
                    bool retroEffectiveDate = ObjectControllerHelper.GetBool("RetroEffectiveDate", drReg);

                    bool isReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", drReg);
                    bool isReapplication = ObjectControllerHelper.GetBool("isreapplication", drReg);
                    bool isReconsideration = false;
                    if (ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg) == CON.WorkflowEventType.Reconsideration || ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg) == CON.WorkflowEventType.CredentialReconsideration)
                    {
                        isReconsideration = true;
                    }

                    string OperatorMedicaidID = string.Empty;

                    DataSet dsReg1 = RegistrationController.SelectRegistrationData(registrationID,"PROVIDER");
                    DataRow drReg1 = ObjectControllerHelper.HasRows(dsReg1) ? dsReg1.Tables[0].Rows[0] : null;
                    OperatorMedicaidID = ObjectControllerHelper.GetString("MEDICAID_ID", drReg1);

                 
                        DataSet dsReg2 = RegistrationController.SelectAffiliationByMedicaidID(OperatorMedicaidID);
                        DataRow drReg2 = ObjectControllerHelper.HasRows(dsReg2) ? dsReg2.Tables[0].Rows[0] : null;

                        if (ObjectControllerHelper.HasRows(dsReg2) && ObjectControllerHelper.GetInt("group_affiliation_status_id", drReg2) == CON.GroupAffiliationStatusTypeID.GroupConfirmed)
                        {
                            facilityLTRegID = ObjectControllerHelper.GetInt("REG_ID", drReg2);
							// add any missing addresses from the operator to the facility; this call will only add missing 1, 2, 3, and 7 type of addresses (SERVICE LOC, PAY TO, MAIL TO, and HOME OFFICE) and won't add duplicates
							RegistrationController.InsertRegAddressesForFacility(facilityLTRegID, registrationID, createDateTime, CON.appAdminUserId);
                        }
                        else
                        {
                             // we don't have a Group Confirmed REG_AFFILIATION for the LT provider at this point; see if we have a building at all
                            if (ObjectControllerHelper.HasRows(dsReg2))
                            {
                                // yes, they already have an LT, so just grab it's ID and don't make another one
                                facilityLTRegID = ObjectControllerHelper.GetInt("REG_ID", drReg2);
                            }
                            // 89 will get taken care of down below
                            if (!MMISProviderType.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR)) 
                            {
                                // insert LT registration if one doesn't exist already
                                if (facilityLTRegID == 0)
                                {
                                    facilityLTRegID = RegistrationController.InsertNewProviderRegistration(userID, CON.ApplicationType.Internal, providerName, dba,
                                    firstName, middleInitial, lastName, birthDate, gender, taxIDTypeID, taxID, npi, providerTypeID, providerCategoryTypeID, practiceTypeID, createDateTime, registrationStatusTypeId,
                                    referralID, specialtyTypeID, taxonomyTypeID, practiceName, zipCode, zipExt, isPaperApplication, paperRequestQueueID, createDateTime, createdby,
                                    workflowID, createDateTime, workflowEventTypeID, waiverTypeID, retroEffectiveDate, practiceTypeCodeID, ownershipTypeCodeID);
                                }
                                RegistrationController.InsertRegAddressesForFacility(facilityLTRegID, registrationID, createDateTime, CON.appAdminUserId);
                                RegistrationController.UpdateFacilityNumber(registrationID, facilityLTRegID, createDateTime, CON.appAdminUserId);
                            }

                            // make sure we don't have a medicaid ID for the facility
                            string FacilityMedicaidID = string.Empty;
                            if (facilityLTRegID > 0)
                            {
                                DataSet dsFacility = RegistrationController.SelectRegistrationData(facilityLTRegID, "PROVIDER");
                                DataRow drFacility = ObjectControllerHelper.HasRows(dsFacility) ? dsFacility.Tables[0].Rows[0] : null;
                                FacilityMedicaidID = ObjectControllerHelper.GetString("MEDICAID_ID", drFacility);
                            }

                            bool assignMedicaidID = false;
                            // [if this is a new registration] OR [it's a Reconsideration/reactivation/reapplication and facilityRegID has been assigned and facilityRegID doesn’t have a Medicaid id yet]
                            if (workflowEventTypeID == CON.WorkflowEventType.NewReg || ((isReconsideration || isReactivation || isReapplication) && facilityLTRegID > 0 && (FacilityMedicaidID == null || FacilityMedicaidID == "")))
                            {
                                assignMedicaidID = true;
                            }
                            if (assignMedicaidID)
                            {
                                if (OperatorMedicaidID == null)
                                {
                                    // this should never get here, but adding it just in case; if our operator provider doesn't have a medicaid ID by this point, assign one here
                                    dsReg = RegistrationController.GenerateMedicaidID();
                                    drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                                    OperatorMedicaidID = ObjectControllerHelper.GetString("MEDICAID_ID", drReg);
                                    if (!string.IsNullOrEmpty(OperatorMedicaidID))
                                    {
                                        // go and update the operator's REG_SERVICE_LOCATION with this medicaid ID we just grabbed
                                        RegistrationController.UpdateMedicaidIDByRegID(registrationID, OperatorMedicaidID, DateTime.Now, CON.appPDMSAdminUserId);
                                    }
                                    else
                                    {
                                        log.CreateLogEntry("No more Medicaid ID left", Logging.LogPriority.Error);
                                        if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                                        throw new ArgumentOutOfRangeException("Medicaid ID of 9999999 is already assigned and no more 7 digit medicaid ids to assign.");
                                    }
                                }

                                // get a medicaid ID for the building now
                                dsReg = RegistrationController.GenerateMedicaidID();
                                drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                                string facilityMedID = ObjectControllerHelper.GetString("MEDICAID_ID", drReg);
                                if (!string.IsNullOrEmpty(facilityMedID))
                                {
                                    if (!MMISProviderType.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR))
                                    {
                                        // update the medicaid ID for the facility in its REG_SERVICE_LOCATION
                                        RegistrationController.UpdateMedicaidIDByRegID(facilityLTRegID, facilityMedID, DateTime.Now, CON.appPDMSAdminUserId);

                                        // go link the operator provider to the facility; basically, insert a REG_AFFILIATION row with the operator's medicaid ID and the facility's REG_ID
                                        RegistrationController.LinkLTToLTCProvider(facilityLTRegID, firstName, lastName, providerName, npi, OperatorMedicaidID, DateTime.Now.ToString(),
                                            Convert.ToDateTime("12/31/2299").ToString(), DateTime.Now.ToString(), CON.appPDMSAdminUserId, CON.GroupAffiliationStatusTypeID.GroupConfirmed,
                                            CON.RegistrationModifiedStatusType.NoChange.ToString(), DateTime.Now.ToString(), CON.appAdminUserId);
                                    }
                                    else
                                    {
                                        // this code is for "89" providers:
                                        DataSet dsLTFac = LookupTableController.GetLTRegFromDDFacilityNumberByRegID(registrationID);
                                        DataRow drLTFac = ObjectControllerHelper.HasRows(dsLTFac) ? dsLTFac.Tables[0].Rows[0] : null;
                                        int LTFacilityRegID = ObjectControllerHelper.GetInt("BUILDING_REG_ID", drLTFac);
                                        RegistrationController.LinkLTToLTCProvider(LTFacilityRegID, firstName, lastName, providerName, npi, OperatorMedicaidID, DateTime.Now.ToString(),
                                                                Convert.ToDateTime("12/31/2299").ToString(), DateTime.Now.ToString(), CON.appPDMSAdminUserId, CON.GroupAffiliationStatusTypeID.GroupConfirmed,
                                                                CON.RegistrationModifiedStatusType.NoChange.ToString(), DateTime.Now.ToString(), CON.appAdminUserId);
                                        RegistrationController.UpdateFacilityNumber(registrationID, LTFacilityRegID, createDateTime, CON.appAdminUserId); // Add Home number to ICF facility
                                    }
                                }
                                else
                                {
                                    log.CreateLogEntry("No more Medicaid ID left", Logging.LogPriority.Error);
                                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                                    throw new ArgumentOutOfRangeException("Medicaid ID of 9999999 is already assigned and no more 7 digit medicaid ids to assign.");
                                }
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            nextStep = "Next";
            return true;
        }
        override public string NextStep()
        {
            return nextStep;
        }
    }
}
