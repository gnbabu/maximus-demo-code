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
    public class WFWaiverDataReceivedFromSI : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        static List<string> m_ApplicationStatusIDs = new List<string>();
        public WFWaiverDataReceivedFromSI(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int regID;
            int additionalApplicationID;
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
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID), out additionalApplicationID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.AdditionalApplicationID)));
                }

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int WaiverTypeID = ObjectControllerHelper.GetInt("WaiverTypeID", drReg);
              //  int WaiverServiceTypeID = ObjectControllerHelper.GetInt("WaiverServiceUpdateTypeID", drReg);
                string MedicaidID = ObjectControllerHelper.GetString("MedicaidID", drReg);
                bool HasDODDSpecialty = ObjectControllerHelper.GetBool("HasDODDSpecialty", drReg);
                bool HasODASpecialty = ObjectControllerHelper.GetBool("HasODASpecialty", drReg);
                string MMISProviderTypeID = ObjectControllerHelper.GetString("MMISProviderTypeID", drReg);

                int WaiverServiceTypeID = 0;
                if (!string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId)))
                {
                    WaiverServiceTypeID = Convert.ToInt32(p.GetProcessParameter(Constants.ProcessParameter.WaiverServiceUpdateTypeId));
                }

                string makeRequest = AppSettings.Get("CheckEnvConnectedToSSA");
                if (makeRequest.ToLower().Equals("true"))
                {
                    List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                    sqlParmsTR.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, false));
                    sqlParmsTR.Add(SqlParms.CreateParameter("AdditionalApplicationID", DbType.Int32, additionalApplicationID, false));
                    sqlParmsTR.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, p.ProcessID, false));
                    DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDITIONAL_APPLICATIONCustom", sqlParmsTR, "RegAddApplication");
                    dsRegTR.Tables[0].TableName = "RegAddApplication";
                    DataRow drRegTR = ObjectControllerHelper.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                    string OtherAppStatusID = ObjectControllerHelper.GetString("OTHER_APPLICATION_STATUS_ID", drRegTR);
                    string OtherAppStatus = ObjectControllerHelper.GetString("OTHER_APPLICATION_STATUS_DESC", drRegTR);
                    bool isRequireReview = ObjectControllerHelper.GetBool("IS_REQUIRE_REVIEW", drRegTR);
                    string additionalAppType = ObjectControllerHelper.GetString("ADDITIONAL_APPLICATION_TYPE_NAME", drRegTR);                    

                    if ((WaiverServiceTypeID == CON.WaiverServiceUpdateType.DODD ||
                        (WaiverTypeID == CON.WaiverApplicationTypeID.DODD || WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)
                        || HasDODDSpecialty) && (WaiverServiceTypeID != CON.WaiverServiceUpdateType.ODA))
                    {
						// OHPNM-6301 - removed the legal status check from the Closed_Partial_Approval part of this condition
                        if((isRequireReview && OtherAppStatus.Equals(CON.ApplicationStatus.Pending_External_Medicaid_Approval)) ||
                            (!isRequireReview && (IsApplicationStatus(OtherAppStatusID) || (OtherAppStatusID.Equals(CON.ApplicationStatus.Closed_Partial_Approval)))))
                        {
                            nextStep = "Next";
                            toReturn = true;
                        }
                    }                   
                    else if ((OtherAppStatus.Equals(CON.ApplicationStatus.Recommended_Certification) || OtherAppStatusID.Equals(CON.ApplicationStatus.Recommended_Certification_ID) ||
                        OtherAppStatus.Equals(CON.ApplicationStatus.Recommended_Denial) || OtherAppStatusID.Equals(CON.ApplicationStatus.Recommended_Denial_ID)) 
                        && (WaiverTypeID == CON.WaiverApplicationTypeID.ODA || WaiverServiceTypeID == CON.WaiverServiceUpdateType.ODA || HasODASpecialty))
                    {
                        nextStep = "Next";
                        toReturn = true;
                    }
                    else
                    {
                        nextStep = "";
                        toReturn = false;
                    }
                }
                else if (makeRequest.ToLower().Equals("false"))
                {
                    nextStep = "Next";
                    toReturn = true;
                }
                else
                {
                    nextStep = "";
                    toReturn = false;
                }

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
        static List<string> ApplicationStatusIDs()
        {
            if (m_ApplicationStatusIDs.Count == 0)
            {
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Closed_By_ODM);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Certified);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Closed_As_Legal_Override);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Approved_ID);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.ClosedAsAdjudicationOrderIssued);
                m_ApplicationStatusIDs.Add(CON.ApplicationStatus.Closed_As_Complete_ID);
            }
            return m_ApplicationStatusIDs;
        }

        public static bool IsApplicationStatus(string statusName)
        {
            return ApplicationStatusIDs().Contains(statusName);
        }

    }
}
