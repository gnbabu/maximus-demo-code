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
    public class WFAssignMedicaidID : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFAssignMedicaidID(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int regID;
            Process p = new Process(ProcessID);

            try
            {
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out regID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }

                nextStep = "Next";
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                DataSet dsReg;
                DataRow drReg;
                int workflowEventTypeID = 0;
                int applicationTypeID = 0;
                string mmisProviderTypeID = string.Empty;
                string provmedicaidID = string.Empty;
                bool assignMedicaidID = false;
                bool provmedicaidIDExists = false;
                int WaiverTypeID = -1;
                bool isReactivation = false;
                bool isReapplication = false;
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                    applicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
                    mmisProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);
                    provmedicaidID = ObjectControllerHelper.GetString("MedicaidID", drReg);
                    WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
                    isReactivation = ObjectControllerHelper.GetBool("IsProviderReactivation", drReg);
                    isReapplication = ObjectControllerHelper.GetBool("isreapplication", drReg);
                }

                if (!string.IsNullOrEmpty(provmedicaidID))
                {
                    provmedicaidIDExists = true;
                }



                if ((workflowEventTypeID == CON.WorkflowEventType.NewReg || workflowEventTypeID == CON.WorkflowEventType.Reconsideration || workflowEventTypeID == CON.WorkflowEventType.CredentialReconsideration
                    || workflowEventTypeID == CON.WorkflowEventType.UpdateReg || workflowEventTypeID == CON.WorkflowEventType.ChangeProviderType || isReactivation || isReapplication) && !provmedicaidIDExists &&
                    !(mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR)))
                {
                    assignMedicaidID = true;
                }
                else if (mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) || mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR))
                {
                    // OHPNM-7423 - if our provider hasn't been assigned a medicaid ID yet, we need to
                    if (!provmedicaidIDExists)
                    {
                        assignMedicaidID = true;
                    }

                    if (applicationTypeID == 3 && workflowEventTypeID != CON.WorkflowEventType.RevalReg)
                    {
                        nextStep = "Next";
                    }
                    else if (workflowEventTypeID == CON.WorkflowEventType.RevalReg)
                    {
                        // OHPNM-5450 we don't need to link the 86-89 to an "LT" but we should at least make sure all the addresses have been copied over; sometimes some of the 1, 2, 3, and 7 addresses are missing; probably from conversion
                        int facilityLTRegID = RegistrationController.SelectBuildingRegistration(regID);
                        RegistrationController.InsertRegAddressesForFacility(facilityLTRegID, regID, DateTime.Now, CON.appAdminUserId);
                        nextStep = "Next";
                        //nextStep = "Revalidation 86-89"; // Revalidation 86-89
                    }
                    else
                    {
                        nextStep = "Link LT To 86-89";
                    }

                }
                if (assignMedicaidID)
                {
                    string LastMedicaidID = string.Empty;
                    string nextMED_ID = string.Empty;

                    dsReg = RegistrationController.GenerateMedicaidID();
                    drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                    nextMED_ID = ObjectControllerHelper.GetString("MEDICAID_ID", drReg);

                    if (!string.IsNullOrEmpty(nextMED_ID))
                    {
                        RegistrationController.UpdateMedicaidIDByRegID(regID, nextMED_ID, DateTime.Now, CON.appPDMSAdminUserId);
                    }
                    else
                    {
                        log.CreateLogEntry("No more Medicaid ID left", Logging.LogPriority.Error);
                        if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                        throw new ArgumentOutOfRangeException("Medicaid ID of 9999999 is already assigned and no more 7 digit medicaid ids to assign.");
                    }

                    if (mmisProviderTypeID == "86")//Nursing Facility No
                    {
                        dsReg = RegistrationController.GetMaxNursingFacilityNo();
                        drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                        string NURSING_FACILITY_NO = ObjectControllerHelper.GetString("NURSING_FACILITY_NO", drReg);
                        string nextNURSING_FACILITY_NO = GenMedicaidIDController.GetNext4digitMCPNID(NURSING_FACILITY_NO);
                        RegistrationController.UpdateNursingFacilityNoByRegID(regID, nextNURSING_FACILITY_NO, DateTime.Now, CON.appPDMSAdminUserId);
                    }
                    if (mmisProviderTypeID == "05" || mmisProviderTypeID == "12" || mmisProviderTypeID == "50" || mmisProviderTypeID == "21") //Health Center No
                    {
                        dsReg = RegistrationController.GetMaxHealthCenterNo();
                        drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                        string HEALTH_CENTER_NO = ObjectControllerHelper.GetString("HEALTH_CENTER_NO", drReg);
                        string nextHEALTH_CENTER_NO = GenMedicaidIDController.GetNext4digitMCPNID(HEALTH_CENTER_NO);
                        RegistrationController.UpdateHealthCenterNoByRegID(regID, nextHEALTH_CENTER_NO, DateTime.Now, CON.appPDMSAdminUserId);
                    }
                    if (mmisProviderTypeID == "01" || mmisProviderTypeID == "02") //Hospital No
                    {
                        dsReg = RegistrationController.GetMaxHospitalNo();
                        drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                        string HOSPITAL_NO = ObjectControllerHelper.GetString("HOSPITAL_NO", drReg);
                        string nextHOSPITAL_NO = GenMedicaidIDController.GetNext4digitMCPNID(HOSPITAL_NO);
                        RegistrationController.UpdateHospitalNoByRegID(regID, nextHOSPITAL_NO, DateTime.Now, CON.appPDMSAdminUserId);
                    }

                    // OHPNM-7423 - don't override nextStep if 86, 88, or 89 since their logic was set above outside of the assignMedicaidID code
                    if (!mmisProviderTypeID.Equals(CON.LTCProviderTypes.NursingFacility) && !mmisProviderTypeID.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR) && !mmisProviderTypeID.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR))
                    {
                        nextStep = "Next";
                    }
                }

                //OHPNM-10510
                try
                {
                    DataSet dsADDRESSPrimaryPractice;
                    DataRow drRegPrimary;
                    DataSet dsADDRESSCustom;
                    DataTable dtSatellitePracticeLocations;
                    int regAddressID = 0;

                    if (mmisProviderTypeID == "86")//Nursing Facility No
                    {
                        // Primary Service Locations
                        dsADDRESSPrimaryPractice = RegistrationController.SelectAddressCustomData(regID, CON.AddressType.PrimaryPractice, "ADDRESSCustom");
                        drRegPrimary = ObjectControllerHelper.HasRows(dsADDRESSPrimaryPractice) ? dsADDRESSPrimaryPractice.Tables[0].Rows[0] : null;
                        if (drRegPrimary != null)
                        {
                            regAddressID = ObjectControllerHelper.GetInt("REG_ADDRESS_ID", drRegPrimary);

                            dsReg = RegistrationController.GetMaxNursingFacilityNoRegAddress();
                            drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                            string NURSING_FACILITY_ADDRESS_NO = ObjectControllerHelper.GetString("NURSING_FACILITY_NO", drReg);
                            string nextNURSING_FACILITY_ADDRESS_NO = GenMedicaidIDController.GetNext4digitMCPNID(NURSING_FACILITY_ADDRESS_NO);
                            RegistrationController.InsertAddressNursingFacilityNoByRegID(regID, regAddressID, nextNURSING_FACILITY_ADDRESS_NO, DateTime.Now, CON.appPDMSAdminUserId);
                        }
                    }

                    //OHPNM-10510
                    if (mmisProviderTypeID == "05" || mmisProviderTypeID == "12" || mmisProviderTypeID == "50") //excluding Professional Medical Group (21)
                    {
                        // Primary Service Locations
                        dsADDRESSPrimaryPractice = RegistrationController.SelectAddressCustomData(regID, CON.AddressType.PrimaryPractice, "ADDRESSCustom");
                        drRegPrimary = ObjectControllerHelper.HasRows(dsADDRESSPrimaryPractice) ? dsADDRESSPrimaryPractice.Tables[0].Rows[0] : null;
                        if (drRegPrimary != null)
                        {
                            regAddressID = ObjectControllerHelper.GetInt("REG_ADDRESS_ID", drRegPrimary);

                            dsReg = RegistrationController.GetMaxHealthCenterNoRegAddress();
                            drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                            string HEALTH_CENTER_ADDRESS_NO = ObjectControllerHelper.GetString("HEALTH_CENTER_NO", drReg);
                            string nextHEALTH_CENTER_ADDRESS_NO = GenMedicaidIDController.GetNext4digitMCPNID(HEALTH_CENTER_ADDRESS_NO);
                            RegistrationController.InsertAddressHealthCenterNoByRegID(regID, regAddressID, nextHEALTH_CENTER_ADDRESS_NO, DateTime.Now, CON.appPDMSAdminUserId);
                        }
                        // Other Service Locations
                        dsADDRESSCustom = RegistrationController.SelectAddressCustomData(regID, CON.AddressType.AlternateServiceLocation, "ADDRESSCustom");
                        dtSatellitePracticeLocations = ObjectControllerHelper.HasRows(dsADDRESSCustom) ? dsADDRESSCustom.Tables["RegistrationData"] : null;
                        if (dtSatellitePracticeLocations != null)
                        {
                            int countRow = dtSatellitePracticeLocations.Rows.Count;
                            DataRow dr = null;
                            for (int i = 0; i < countRow; i++)
                            {
                                dr = dtSatellitePracticeLocations.Rows[i];
                                regAddressID = ObjectControllerHelper.GetInt("REG_ADDRESS_ID", dr);

                                dsReg = RegistrationController.GetMaxHealthCenterNoRegAddress();
                                drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                                string HEALTH_CENTER_ADDRESS_NO = ObjectControllerHelper.GetString("HEALTH_CENTER_NO", drReg);
                                string nextHEALTH_CENTER_ADDRESS_NO = GenMedicaidIDController.GetNext4digitMCPNID(HEALTH_CENTER_ADDRESS_NO);
                                RegistrationController.InsertAddressHealthCenterNoByRegID(regID, regAddressID, nextHEALTH_CENTER_ADDRESS_NO, DateTime.Now, CON.appPDMSAdminUserId);
                            }
                        }
                    }

                    if (mmisProviderTypeID == "01" || mmisProviderTypeID == "02") //Hospital No
                    {

                        // Primary Service Locations
                        dsADDRESSPrimaryPractice = RegistrationController.SelectAddressCustomData(regID, CON.AddressType.PrimaryPractice, "ADDRESSCustom");
                        drRegPrimary = ObjectControllerHelper.HasRows(dsADDRESSPrimaryPractice) ? dsADDRESSPrimaryPractice.Tables[0].Rows[0] : null;
                        if (drRegPrimary != null)
                        {
                            regAddressID = ObjectControllerHelper.GetInt("REG_ADDRESS_ID", drRegPrimary);

                            dsReg = RegistrationController.GetMaxHospitalNoRegAddress();
                            drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                            string HOSPITAL_ADDRESS_NO = ObjectControllerHelper.GetString("HOSPITAL_NO", drReg);
                            string nextHOSPITAL_ADDRESS_NO = GenMedicaidIDController.GetNext4digitMCPNID(HOSPITAL_ADDRESS_NO);
                            RegistrationController.InsertAddressHospitalNoByRegID(regID, regAddressID, nextHOSPITAL_ADDRESS_NO, DateTime.Now, CON.appPDMSAdminUserId);
                        }
                        // Other Service Locations
                        dsADDRESSCustom = RegistrationController.SelectAddressCustomData(regID, CON.AddressType.AlternateServiceLocation, "ADDRESSCustom");
                        dtSatellitePracticeLocations = ObjectControllerHelper.HasRows(dsADDRESSCustom) ? dsADDRESSCustom.Tables["RegistrationData"] : null;
                        if (dtSatellitePracticeLocations != null)
                        {
                            int countRow = dtSatellitePracticeLocations.Rows.Count;
                            DataRow dr = null;
                            for (int i = 0; i < countRow; i++)
                            {
                                dr = dtSatellitePracticeLocations.Rows[i];
                                regAddressID = ObjectControllerHelper.GetInt("REG_ADDRESS_ID", dr);

                                dsReg = RegistrationController.GetMaxHospitalNoRegAddress();
                                drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                                string HOSPITAL_ADDRESS_NO = ObjectControllerHelper.GetString("HOSPITAL_NO", drReg);
                                string nextHOSPITAL_ADDRESS_NO = GenMedicaidIDController.GetNext4digitMCPNID(HOSPITAL_ADDRESS_NO);
                                RegistrationController.InsertAddressHospitalNoByRegID(regID, regAddressID, nextHOSPITAL_ADDRESS_NO, DateTime.Now, CON.appPDMSAdminUserId);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    log.CreateLogEntry("Update HCN Numbers failed. Reason: " + ex.Message, Logging.LogPriority.Error);
                }

                if (applicationTypeID == CON.ApplicationType.Waiver)
                {
                    nextStep = "Next";
                }

                toReturn = true;
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return toReturn;
        }

        override public string NextStep()
        {
            return nextStep;
        }
    }
}
