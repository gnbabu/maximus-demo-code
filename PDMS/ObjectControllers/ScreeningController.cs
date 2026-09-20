using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.Controllers.PDMS
{
	public static class ScreeningController
	{
		public static DataSet SelectGroupProviderScreeningData(int regID)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				// Only add the RegistrationId if it is greater than -1
				if (regID > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
				DataSet ds = new DataSet();
				string storedProc = "usp_SelectPROVIDER_SCREENING";
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "ScreeningData");
				ds.Tables[0].TableName = "ScreeningData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

        public static DataSet SelectAffiliationsScreeningData(int regID)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				// Only add the RegistrationId if it is greater than -1
				if (regID > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
				DataSet ds = new DataSet();
				string storedProc = "usp_SelectAFFILIATION_SCREENING";
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "AffiliationScreeningData");
				ds.Tables[0].TableName = "AffiliationScreeningData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

		public static DataSet SelectOwnersScreeningData(int regID)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				// Only add the RegistrationId if it is greater than -1
				if (regID > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
				DataSet ds = new DataSet();
				string storedProc = "usp_SelectOWNER_SCREENING";
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "OwnerScreeningData");
				ds.Tables[0].TableName = "OwnerScreeningData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

        public static DataSet SelectOwnersScreeningEndDate(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Only add the RegistrationId if it is greater than -1
                if (regID > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectOWNER_SCREENING_EndDate";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "OwnerScreeningEndDate");
                ds.Tables[0].TableName = "OwnerScreeningEndDate";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet SelectHouseholdMemberScreeningData(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Only add the RegistrationId if it is greater than -1
                if (regID > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                DataSet ds = new DataSet();
                string storedProc = "[usp_SelectHOUSEHOLD_MEMBER_SCREENING]";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "HHMbrScreeningData");
                ds.Tables[0].TableName = "HHMbrScreeningData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        
        public static DataSet SelectProviderScreeningActivityData(int screeningID)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				// Only add the RegistrationId if it is greater than -1
				if (screeningID > -1) parameters.Add(SqlParms.CreateParameter("SCREENING_ID", DbType.Int32, screeningID, true));
				DataSet ds = new DataSet();
				string storedProc = "usp_SelectPROVIDER_SCREENING_ACTIVITY";
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "ScreeningActivityData");
				ds.Tables[0].TableName = "ScreeningActivityData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

        public static DataSet SelectProviderTypeWithLicenses(int providerTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, true));
            DataSet ds = new DataSet();
            string storedProc = "usp_SelectProviderTypeWithLicenses";
            ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "LicensedProviderTypesData");
            ds.Tables[0].TableName = "LicensedProviderTypesData";
            return ds;

        }


        public static DataSet SelectScreeningActivityMatchData(int screeningActivityTypeID, int screeningActivityID, int regID, int regAffiliationID, int regOwnerID)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				// Only add the RegistrationId if it is greater than -1
				if (screeningActivityID > -1) parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_ID", DbType.Int32, screeningActivityID, true));
				if (regID > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
				if (regAffiliationID > -1) parameters.Add(SqlParms.CreateParameter("REG_AFFILIATION_ID", DbType.Int32, regAffiliationID, true));
                if (regOwnerID > -1) parameters.Add(SqlParms.CreateParameter("REG_OWNER_ID", DbType.Int32, regOwnerID, true));
                DataSet ds = new DataSet();

                string storedProc = ""; 
                switch (screeningActivityTypeID)
                {
                    case (int)Enumerations.ScreeningActivityType.OIGLEIEVerification:
                        storedProc = "usp_SelectSCREENING_MATCH_GRID_RESULTS";
                        break;
                    case (int)Enumerations.ScreeningActivityType.SSDMFVerification:
                        storedProc = "usp_SelectSCREENING_MATCH_SCR_SSDMF";
                        break;
                    case (int)Enumerations.ScreeningActivityType.SAMVerification:
                        storedProc = "usp_SelectSCREENING_MATCH_GRID_RESULTS";
                        break;
                    case (int)Enumerations.ScreeningActivityType.LicenseVerification:
                    case (int)Enumerations.ScreeningActivityType.PECOSVerfication:
                    case (int)Enumerations.ScreeningActivityType.SAVEVerification:
                    case (int)Enumerations.ScreeningActivityType.SexOffenderVerification:
                    case (int)Enumerations.ScreeningActivityType.NDENVerification:
                    case (int)Enumerations.ScreeningActivityType.APSCPSVerification:
                    case (int)Enumerations.ScreeningActivityType.ControlledSubstanceVerification:
                    case (int)Enumerations.ScreeningActivityType.CriminalBackgroundCheck:
                    case (int)Enumerations.ScreeningActivityType.SiteVisitVerification:
                        storedProc = "usp_SelectSCREENING_MATCH_OUTSIDE_SCREENING";
                        break;
                    case (int)Enumerations.ScreeningActivityType.MCSISVerification:
                        storedProc = "usp_SelectSCREENING_MATCH_GRID_RESULTS";
                        break;
                    case (int)Enumerations.ScreeningActivityType.NEMEPLVerification:
                        storedProc = "usp_SelectSCREENING_MATCH_SCR_NEMEPL_EXCLUSION";
                        break;
                    case (int)Enumerations.ScreeningActivityType.NPIVerification:
                        storedProc = "usp_SelectSCREENING_MATCH_SCR_NPPES_EXCLUSION";
                        break;
                    case (int)Enumerations.ScreeningActivityType.MEDVerification:
                        storedProc = "usp_SelectSCREENING_MATCH_GRID_RESULTS";
                        break;
                    case (int)Enumerations.ScreeningActivityType.DODDAbuserRegistry:
                        storedProc = "usp_SelectSCREENING_MATCH_REG_DODD_VERIFICATION";
                        break;
                    case (int)Enumerations.ScreeningActivityType.DEAVerification:
                        storedProc = "usp_SelectSCREENING_MATCH_SCR_DEA_MATCH";
                        break;
                    case (int)Enumerations.ScreeningActivityType.OHMedExclSuspension:
                        storedProc = "usp_SelectSCREENING_MATCH_SCR_MEDPROV_EXCLUSION_SUSPENSION_MATCH";
                        break;
                    default:
                        throw new Exception("Invalid ActivityScreeningTypeID");
                }
                
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "ScreeningActivityMatchData");
				ds.Tables[0].TableName = "ScreeningActivityMatchData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

        public static DataSet SelectScreeningActivityDocuments(int screeningActivityID, string userRole)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_ID", DbType.Int32, screeningActivityID, true));
                parameters.Add(SqlParms.CreateParameter("USER_ROLE", DbType.String, userRole, true));
				DataSet ds = new DataSet();
				ds = DataAccess.ExecuteStoredProcedure("usp_SelectSCREENING_ACTIVITY_DOCUMENT", parameters, "ScreeningActivityDocumentData");
				ds.Tables[0].TableName = "ScreeningActivityDocumentData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

        public static bool RemoveScreeningExlusionData(int regID, int exclTypeID, string comments, Guid userID)
        {
            bool isDeleted = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("EXCLUSION_TYPE_ID", DbType.Int32, exclTypeID, true));
                parameters.Add(SqlParms.CreateParameter("COMMENTS", DbType.String, comments, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userID, true));
                DataSet ds = new DataSet();
                isDeleted = Convert.ToBoolean(DataAccess.ExecuteStoredProcedure("usp_Remove_Screening_Exclusion_Data", parameters, "IS_DELETED", SqlDbType.Bit, 100));   
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return isDeleted;
        }


        public static void InsertScreeningActivityDocument(int screeningActivityID, string name, string description, string fileName, DateTime? changedDate,
			string changedBy)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_ID", DbType.Int32, screeningActivityID, true));
				parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, true));
				parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, description, true));
				parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, fileName, true));
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
				DataAccess.ExecuteStoredProcedure("usp_InsertSCREENING_ACTIVITY_DOCUMENT", parameters);
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

		public static bool InsertProviderScreening(int regID, int workflowID, DateTime? changedDate, string userID, int DODDactivityStatus,string OwnerDODDActivityStatus, out bool isExactMatchFound, out bool isSoftMatchFound)
		{
            bool rtnVal = false;
            
            isExactMatchFound = false;
            isSoftMatchFound = false ;
            try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("WORKFLOW_ID", DbType.Int32, workflowID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userID, true));
                parameters.Add(SqlParms.CreateParameter("DODD_ACTIVITY_STATUS", DbType.Int32, DODDactivityStatus, true));
                parameters.Add(SqlParms.CreateParameter("OWNER_DODD_ACTIVITY_STATUS", DbType.String, OwnerDODDActivityStatus, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_InsertPROVIDER_SCREENING", parameters, "ProviderScreening");
                ds.Tables[0].TableName = "ProviderScreeningData";

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    isExactMatchFound = Convert.ToBoolean(ds.Tables[0].Rows[0]["ExactMatchFound"]);
                    isSoftMatchFound = Convert.ToBoolean(ds.Tables[0].Rows[0]["SoftMatchFound"]);
                }
                
                //DataAccess.ExecuteStoredProcedure("usp_InsertPROVIDER_SCREENING", parameters, "isExactMatchFound", SqlDbType.Bit, out outValue, 0);
                //if (!string.IsNullOrEmpty(outValue))
                //{
                //    isExactMatchFound = Convert.ToBoolean(outValue);
                //}
                rtnVal = true;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
            return rtnVal;
		}

        public static bool InsertProviderCredentialing(int regID, int workflowID, DateTime? changedDate, string userID)
        {
            bool rtnVal = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("WORKFLOW_ID", DbType.Int32, workflowID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userID, true));
                DataAccess.ExecuteStoredProcedure("usp_InsertPROVIDER_Credentialing", parameters);
                rtnVal = true;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return rtnVal;
        }

		public static void UpdateScreeningActivityStatus(int screeningActivityID, int screeningActivityStatusID, string adverseAction, DateTime? changedDate, string changedBy)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_ID", DbType.Int32, screeningActivityID, true));
				parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_STATUS_ID", DbType.Int32, screeningActivityStatusID, true));
                parameters.Add(SqlParms.CreateParameter("ADVERSE_ACTION", DbType.String, adverseAction, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
				DataAccess.ExecuteStoredProcedure("usp_UpdateSCREENING_ACTIVITY_STATUS", parameters);
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}


        public static void UpdateScreeningStatus(int screeningID, int screeningStatusID, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SCREENING_ID", DbType.Int32, screeningID, true));
                parameters.Add(SqlParms.CreateParameter("SCREENING_STATUS_ID", DbType.Int32, screeningStatusID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateSCREENING_STATUS", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateScreeningScreeningStatusID(int screeningID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SCREENING_ID", DbType.Int32, screeningID, true));
               
                DataAccess.ExecuteStoredProcedure("usp_UpdateSCREENING_ScreeningSTATUS", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectOwnersScreeningDataWithEndDate(int regID, int screeningID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Only add the RegistrationId if it is greater than -1
                if (regID > -1) 
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("SCREENING_ID", DbType.Int32, screeningID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectOWNER_SCREENING_WithEndDate";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "OwnerScreeningData");
                ds.Tables[0].TableName = "OwnerScreeningData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateScreeningPreviousStatus(int regID, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateSCREENING_PREVIOUS_STATUS", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateScreeningResult(int screeningID, int screeningResultId, DateTime? changedDate, string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SCREENING_ID", DbType.Int32, screeningID, true));
                parameters.Add(SqlParms.CreateParameter("SCREENING_RESULT_ID", DbType.Int32, screeningResultId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateSCREENING_RESULT", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        
        public static void InsertScreeningAdverseAction(int screeningActivityID, string description, DateTime? changedDate, string changedBy)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_ID", DbType.Int32, screeningActivityID, true));
				parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, description, true));
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
				DataAccess.ExecuteStoredProcedure("usp_InsertSCREENING_ADVERSE_ACTION", parameters);
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

		public static DataSet SelectScreeningAdverseActions(int regID)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				// Only add the RegistrationId if it is greater than -1
				if (regID > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
				DataSet ds = new DataSet();
				string storedProc = "usp_SelectSCREENING_ADVERSE_ACTION";
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "ScreeningAdverseActionData");
				ds.Tables[0].TableName = "ScreeningAdverseActionData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

        public static DataSet SelectScreeningStatus(int screeningID, int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Only add the RegistrationId if it is greater than -1
                if (screeningID > -1) parameters.Add(SqlParms.CreateParameter("SCREENING_ID", DbType.Int32, screeningID, true));
				parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectREG_SCREENING_STATUS";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "ScreeningStatusData");
                ds.Tables[0].TableName = "ScreeningStatusData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

		public static DataSet SelectAdverseActions(int screeningActivityID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Only add the RegistrationId if it is greater than -1
                if (screeningActivityID > -1) parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_ID", DbType.Int32, screeningActivityID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectSCREENING_ACTIVITY_ADVERSE_ACTION";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "AdverseActionData");
				ds.Tables[0].TableName = "AdverseActionData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectScreeningFailedActivities(int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Only add the RegistrationId if it is greater than -1
                if (regID > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectREG_SCREENING_MATCH";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "FailedActivityData");
                ds.Tables[0].TableName = "FailedActivityData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

		public static DataSet SelectSiteVisitScreeningData(int regID, int? screeningActivityTypeID = null)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				// Only add the RegistrationId if it is greater than -1
				if (regID > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
				DataSet ds = new DataSet();
				string storedProc = "usp_SelectSITE_VISIT_SCREENING";
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "SiteVisitScreeningData");
				ds.Tables[0].TableName = "SiteVisitScreeningData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

		public static DataSet SelectSiteVisitData(int RegID)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();

                // Only add the RegistrationId if it is greater than -1
                if (RegID > -1)
                {
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegID, true));
                }

                DataSet ds = new DataSet();
                //change the proc name before checkin
				string storedProc = "usp_SelectSITE_VISIT";
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "SiteVisitData");
				ds.Tables[0].TableName = "SiteVisitData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}


		public static DataSet SelectSiteVisitAttemptData(int siteVisitID)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				// Only add the RegistrationId if it is greater than -1
				if (siteVisitID > -1) parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ID", DbType.Int32, siteVisitID, true));
				DataSet ds = new DataSet();
				string storedProc = "usp_SelectSITE_VISIT_ATTEMPT";
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "SiteVisitAttemptData");
				ds.Tables[0].TableName = "SiteVisitAttemptData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

		public static DataSet SelectSiteVisitDetails(int siteVisitID)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				// Only add the RegistrationId if it is greater than -1
				if (siteVisitID > -1) parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ID", DbType.Int32, siteVisitID, true));
				DataSet ds = new DataSet();
				string storedProc = "usp_SelectSITE_VISIT_DETAILS";
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "SiteVisitDetailData");
				ds.Tables[0].TableName = "SiteVisitDetailData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

		public static DataSet SelectSiteVisitAttemptDetails(int siteVisitAttemptID)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				// Only add the RegistrationId if it is greater than -1
				if (siteVisitAttemptID > -1) parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ATTEMPT_ID", DbType.Int32, siteVisitAttemptID, true));
				DataSet ds = new DataSet();
				string storedProc = "usp_SelectSITE_VISIT_ATTEMPT_DETAILS";
				ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "SiteVisitAttemptDetailData");
				ds.Tables[0].TableName = "SiteVisitAttemptDetailData";
				return ds;
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}

        public static DataSet SelectServiceRemovedCheckBox(int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Only add the RegistrationId if it is greater than -1
                if (regId > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectREG_APPLICATIONCustom";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "regApplicationCustom");
                ds.Tables[0].TableName = "regApplicationCustom";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateSiteVisitResult(int siteVisitID, int resultID, int? recommendationID, DateTime? dateCompleted, string completedBy, string changedBy)
		{
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ID", DbType.Int32, siteVisitID, true));
				parameters.Add(SqlParms.CreateParameter("RESULT_ID", DbType.Int32, resultID, true));
				if (recommendationID.HasValue)
				{
					parameters.Add(SqlParms.CreateParameter("RECOMMENDATION_ID", DbType.Int32, recommendationID.Value, true));
				}
				else
				{
					parameters.Add(SqlParms.CreateParameter("RECOMMENDATION_ID", DbType.Int32, DBNull.Value, true));
				}
				if (dateCompleted.HasValue)
				{
					parameters.Add(SqlParms.CreateParameter("COMPLETED_DATE", DbType.Date, dateCompleted.Value, true));
				}
				else
				{
					parameters.Add(SqlParms.CreateParameter("COMPLETED_DATE", DbType.Date, DBNull.Value, true));
				}

				if (!string.IsNullOrEmpty(completedBy))
				{
					parameters.Add(SqlParms.CreateParameter("COMPLETED_USER", DbType.Guid, changedBy, true));
				}
				else
				{
					parameters.Add(SqlParms.CreateParameter("COMPLETED_USER", DbType.Guid, DBNull.Value, true));
				}
				
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
				parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
				DataAccess.ExecuteStoredProcedure("usp_UpdateSITE_VISIT", parameters);
			}
			catch (Exception ex)
			{
				throw CoreException.ThrowException(ex);
			}
		}




        public static void InsertSiteVisitAttempt(int siteVisitID, int siteVisitStatusID, int? siteVisitRecommendationID, DateTime? dtPerformed, string performedBy, string comments,
            DateTime? changedDate, string changedBy, int? siteVisitAttemptTypeID, DateTime? providerResponseDate, int? siteVisitAttemptStatusID, DateTime? requiredDate, int? siteVisitMethodID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ID", DbType.Int32, siteVisitID, true));
                parameters.Add(SqlParms.CreateParameter("SITE_VISIT_STATUS_ID", DbType.Int32, siteVisitStatusID, true));
                if (siteVisitRecommendationID.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("SITE_VISIT_RECOMMENDATION_ID", DbType.Int32, siteVisitRecommendationID.Value, true));
                }
                if (dtPerformed.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("PERFORMED_DATE", DbType.Date, dtPerformed.Value, true));
                }
                if (!string.IsNullOrEmpty(performedBy))
                {
                    parameters.Add(SqlParms.CreateParameter("PERFORMED_BY_USER", DbType.Guid, performedBy, true));
                }
                parameters.Add(SqlParms.CreateParameter("COMMENTS", DbType.String, comments, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                if (siteVisitAttemptTypeID.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ATTEMPT_TYPE_ID", DbType.Int32, siteVisitAttemptTypeID.Value, true));
                }
                if (providerResponseDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("PROVIDER_RESPONSE_DATE", DbType.Date, providerResponseDate.Value, true));
                }
                if (siteVisitAttemptStatusID.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ATTEMPT_STATUS_ID", DbType.Int32, siteVisitAttemptStatusID.Value, true));
                }
                if (requiredDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("REQUIRED_DATE", DbType.Date, requiredDate.Value, true));
                }
                if (siteVisitMethodID.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("SITE_VISIT_METHOD_ID", DbType.Int32, siteVisitMethodID.Value, true));
                }

                DataAccess.ExecuteStoredProcedure("usp_InsertSITE_VISIT_ATTEMPT", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static void UpdateSiteVisitAttempt(int siteVisitAttemptID, int siteVisitStatusID, int? siteVisitRecommendationID, DateTime? dtPerformed, string performedBy, string comments,
            DateTime? changedDate, string changedBy, int? siteVisitAttemptTypeID, DateTime? providerResponseDate, int? siteVisitAttemptStatusID, DateTime? requiredDate, int? siteVisitMethodID, 
            int? siteVisitFindingsID, DateTime? NodIssueDate, DateTime? POCDate, string complianceComments)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ATTEMPT_ID", DbType.Int32, siteVisitAttemptID, true));
                parameters.Add(SqlParms.CreateParameter("SITE_VISIT_STATUS_ID", DbType.Int32, siteVisitStatusID, true));
                if (siteVisitRecommendationID.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("SITE_VISIT_RECOMMENDATION_ID", DbType.Int32, siteVisitRecommendationID.Value, true));
                }
                if (dtPerformed.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("PERFORMED_DATE", DbType.Date, dtPerformed.Value, true));
                }
                if (!string.IsNullOrEmpty(performedBy))
                {
                    parameters.Add(SqlParms.CreateParameter("PERFORMED_BY_USER", DbType.Guid, performedBy, true));
                }
                parameters.Add(SqlParms.CreateParameter("COMMENTS", DbType.String, comments, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                if (siteVisitAttemptTypeID.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ATTEMPT_TYPE_ID", DbType.Int32, siteVisitAttemptTypeID.Value, true));
                }
                if (providerResponseDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("PROVIDER_RESPONSE_DATE", DbType.Date, providerResponseDate.Value, true));
                }
                if (siteVisitAttemptStatusID.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ATTEMPT_STATUS_ID", DbType.Int32, siteVisitAttemptStatusID.Value, true));
                }
                if (requiredDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("REQUIRED_DATE", DbType.Date, requiredDate.Value, true));
                }
                if (siteVisitMethodID.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("SITE_VISIT_METHOD_ID", DbType.Int32, siteVisitMethodID.Value, true));
                }
                if (siteVisitFindingsID.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("SITE_VISIT_FINDINGS_ID", DbType.Int32, siteVisitFindingsID.Value, true));
                }
                if (NodIssueDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("NOD_ISSUE_DATE", DbType.Date, NodIssueDate.Value, true));
                }
                if (POCDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("POC_DATE", DbType.Date, POCDate.Value, true));
                }
                parameters.Add(SqlParms.CreateParameter("SPECIALIST_COMMENTS", DbType.String, complianceComments, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateSITE_VISIT_ATTEMPT", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateSiteVisitAttemptDueByDate(int siteVisitID, DateTime? requiredDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ID", DbType.Int32, siteVisitID, true));
                if (requiredDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("REQUIRED_DATE", DbType.Date, requiredDate.Value, true));
                }
                DataAccess.ExecuteStoredProcedure("usp_UpdateSITE_VISIT_ATTEMPT_DUE_BY_DATE", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateSiteVisitAttemptStatus(int registraionId, int siteVisitStatusID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, registraionId, true));
                parameters.Add(SqlParms.CreateParameter("SITE_VISIT_STATUS_ID", DbType.Int32, siteVisitStatusID, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateSITE_VISIT_ATTEMPT_STATUS", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPendingSiteVisits()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                //no parms at this time
                DataSet ds = new DataSet();
                string storedProc = "usp_SelectPENDING_SITE_VISITS";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "PendingSiteVisitData");
                ds.Tables[0].TableName = "PendingSiteVisitData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool RegistrationRequiresFCBC(int regID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                returnVal = Convert.ToInt32(DataAccess.ExecuteScalar("usp_RegistrationRequiresFCBC", parameters));

                return returnVal == 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool IsDDSProvider(int regID)
        {
            bool returnVal = false;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                returnVal = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_RegistrationRequiresDDSReview", parameters));

                return returnVal;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int RegistrationRequiresSiteVisit(int regID)
        {
            int returnVal = 0;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                returnVal = Convert.ToInt32(DataAccess.ExecuteScalar("usp_RegistrationRequiresSiteVisit", parameters));

                return returnVal;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static void InsertProviderSiteVisitActivity(int regID, DateTime insertDate, string insertBy)
        {
            /*Inserts the activity for the site visit to initiate the site visite process */
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, insertDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, insertBy, true));
                DataAccess.ExecuteStoredProcedure("usp_InsertPROVIDER_SITEVISIT", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void UpdateScreeningStatus_PeriodicWF(int regID, DateTime insertDate, string insertBy)
        {
            /*Mark Provider and Owner screenings as Incomplete when complicance specialist returns to screening in Periodic Database Checks workflow. */
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, insertDate, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, insertBy, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateScreeningStatus_PeriodicWF", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPendingSiteVisits(string ownerid)
        {
            
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ownerid", DbType.Guid, ownerid, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_GetPendingSiteVisits";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "GetPendingSiteVisits");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        
        public static int GetSiteVisitsReferredToState()
        {
            
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                int countReferredToState = 0;
                string storedProc = "usp_GetSiteVisitsReferredToState";
                countReferredToState = Convert.ToInt32(DataAccess.ExecuteScalar(storedProc, parameters));
                return countReferredToState;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetUnassignedSiteVisits(int attempt,string assignedto, int regID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();                
                parameters.Add(SqlParms.CreateParameter("attempt", DbType.Int32, attempt, true));
                parameters.Add(SqlParms.CreateParameter("assignedto", DbType.Guid, assignedto, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                DataSet ds = new DataSet();
                string storedProc = "usp_GetUnassignedSiteVisits";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "GetUnassignedSiteVisitsData");
                ds.Tables[0].TableName = "UnassignedSiteVisits";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateSiteVisitAttemptComplianceSpecialist(int siteVisitAttemptID, int? siteVisitFindingsID, string comments, DateTime? nodIssueDate, DateTime? pocDate, string complianceUser)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SITE_VISIT_ATTEMPT_ID", DbType.Int32, siteVisitAttemptID, true));
                if (siteVisitFindingsID.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("SITE_VISIT_FINDINGS_ID", DbType.Int32, siteVisitFindingsID.Value, true));
                }
                parameters.Add(SqlParms.CreateParameter("SPECIALIST_COMMENTS", DbType.String, comments, true));
                if(nodIssueDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("NOD_ISSUE_DATE", DbType.DateTime, nodIssueDate, true));
                }
                if (pocDate.HasValue)
                {
                    parameters.Add(SqlParms.CreateParameter("POC_DATE", DbType.Date, pocDate.Value, true));
                }
                parameters.Add(SqlParms.CreateParameter("COMPLIANCE_USER", DbType.Guid, complianceUser, true));
                
                DataAccess.ExecuteStoredProcedure("usp_UpdateSITE_VISIT_ATTEMPT_COMPLIANCE_SPECIALIST", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetScreeningStatuses(int regId, int? providerScreeningID = null)
        {
            DataSet ds = SelectScreeningStatus(providerScreeningID != null ? providerScreeningID.Value : -1,
                regId > 0 ? regId : -1);
            return ds;
        }

        public static bool ScreeningComplete(int regId, string screeningFor, int providerScreeningID = -1)
        {
            //screeningFor:  Group, ActiveAffiliation, AllAffiliations, Owner, Site Visit
            bool isComplete = false;
            DataSet dsActivities = providerScreeningID == -1 ? GetScreeningStatuses(regId, null) : GetScreeningStatuses(regId, providerScreeningID);

            if (ObjectControllerHelper.HasRows(dsActivities))
            {
                DataTable Screenings = dsActivities.Tables[0];
                if (Screenings.Rows.Count == 0)
                {
                    return false;
                }

                var recs = (from screening in Screenings.AsEnumerable()
                                where screening.Field<Int32>("SCREENING_STATUS_ID") == 2
                                   && screening.Field<string>("SCREENING_TYPE") == screeningFor
                                select screening);

                    if (recs.AsDataView().Count == 0)
                    {
                        isComplete = false;
                    }
                    else
                    {
                        isComplete = true;
                    }

            }

            return isComplete;
        }

        public static bool CheckFedExclusionsMatch(int regID)
        {
            
            try
            {
                bool isMatch = false;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_CheckFedExclusionsMatch", parameters, "RegData_" + regID);
                lookup.Tables[0].TableName = "RegData";
                if (ObjectControllerHelper.HasRows(lookup))
                {
                    isMatch = true;
                }
                return isMatch;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool CheckDODDAbuserRegistryMatch(int regID)
        {
            try
            {
                bool isMatch = false;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_CheckDODDAbuserRegistryMatch", parameters, "RegData_" + regID);
                lookup.Tables[0].TableName = "RegData";
                if (ObjectControllerHelper.HasRows(lookup))
                {
                    isMatch = true;
                }
                return isMatch;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static bool CheckNPPESInactiveMatch(int regID)
        {
            try
            {
                bool isMatch = false;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_CheckInactiveNPPESMatch", parameters, "RegData_" + regID);
                lookup.Tables[0].TableName = "RegNPIScreenData";
                if (ObjectControllerHelper.HasRows(lookup))
                {
                    isMatch = true;
                }
                return isMatch;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        /// <summary>
        /// Get the screening Activity data based on the REG_ID and SCREENING_ACTIVITY_TYPE_ID
        /// </summary>
        /// <param name="regId"></param>
        /// <param name="screeningActivityTypeId"></param>
        /// <returns></returns>
        public static DataSet GetProviderScreeningActivityByRegIdAndActivityType(int regId, int screeningActivityTypeId)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_TYPE_ID", DbType.Int32, screeningActivityTypeId, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_SCREENING_ACTIVITY_ByRegIdAndActivityTypeId", parameters, "ScreeningActivityData");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
