using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class HealthCareAffiliationsPresenter : PresenterBase, IPresenter<IHealthCareAffiliationsView, HealthCareAffiliations>
    {
        private IHealthCareAffiliationsView view = null;

        #region svc
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
        #endregion

        public HealthCareAffiliationsPresenter(IHealthCareAffiliationsView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new HealthCareAffiliations();
        }

        /*public void RequestAchFeeSearchResults()
		{
            try
            {
                GroupAndFacilityAffiliations searchCriteria = this.view.Model;

                // Set the search criteria 
                string name = searchCriteria.Name;
                string taxID = searchCriteria.TaxID;
                string npi = searchCriteria.NPI;
                string medicaidID = searchCriteria.MedicaidID;
                DateTime? feePaidDateFrom = searchCriteria.FeePaidDateFrom;
                DateTime? feePaidDateTo = searchCriteria.FeePaidDateTo;
                DateTime? feeDueDateFrom = searchCriteria.FeeDueDateFrom;
                DateTime? feeDueDateTo = searchCriteria.FeeDueDateTo;
                int sortBy = searchCriteria.SortBy;
                int pageNumber = searchCriteria.PageNumber;
                int rowsPerPage = searchCriteria.RowsPerPage;
                bool sortAsc = searchCriteria.SortAsc;

                DataSet fees = svc.SearchACHFeeInformation(name, taxID, npi, medicaidID, feePaidDateFrom, feePaidDateTo, feeDueDateFrom, feeDueDateTo, sortBy, pageNumber, rowsPerPage, sortAsc);
                view.SetACHFeeSearchResults(fees);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }*/

        /*public void ValidateSearchCriteria()
        {
            AchFeeSearch searchCriteria = this.view.Model;

            if (string.IsNullOrEmpty(searchCriteria.Name) &&
                string.IsNullOrEmpty(searchCriteria.TaxID) &&
                string.IsNullOrEmpty(searchCriteria.NPI) &&
                string.IsNullOrEmpty(searchCriteria.MedicaidID) &&
                searchCriteria.FeePaidDateFrom.HasValue == false &&
                searchCriteria.FeePaidDateTo.HasValue == false &&
                searchCriteria.FeeDueDateFrom.HasValue == false &&
                searchCriteria.FeeDueDateTo.HasValue == false)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.AtLeastOneSearchCriteriaRequired);
            }

            if (searchCriteria.FeePaidDateFrom.HasValue)
            {
                if (searchCriteria.FeePaidDateFrom.Value.Date > DateTime.Today)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.PaidDateFromNotFuture);
                }

                if (!searchCriteria.AdditionalCritierionSet && !searchCriteria.FeePaidDateTo.HasValue)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.PaidDateFromOnlyError);
                }
            }

            if (searchCriteria.FeePaidDateTo.HasValue)
            {
                if (searchCriteria.FeePaidDateTo.Value.Date > DateTime.Today)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.PaidDateToNotFuture);
                }

                if (!searchCriteria.AdditionalCritierionSet && !searchCriteria.FeePaidDateFrom.HasValue)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.PaidDateToOnlyError);
                }
            }

            if (searchCriteria.FeePaidDateFrom.HasValue && searchCriteria.FeePaidDateTo.HasValue)
            {
                if (searchCriteria.FeePaidDateFrom.Value > searchCriteria.FeePaidDateTo.Value)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.PaidDateToPriorToDateFrom);
                }

                if (searchCriteria.FeePaidDateFrom.Value.AddMonths(12) < searchCriteria.FeePaidDateTo.Value)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.PaidDateRangeError);
                }
            }

            if (searchCriteria.FeeDueDateFrom.HasValue)
            {
                if (!searchCriteria.FeeDueDateTo.HasValue)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.DueDateToRequired);
                }

            }

            if (searchCriteria.FeeDueDateTo.HasValue)
            {
                if (!searchCriteria.FeeDueDateFrom.HasValue)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.DueDateFromRequired);
                }
            }

            if (searchCriteria.FeeDueDateFrom.HasValue && searchCriteria.FeeDueDateTo.HasValue)
            {
                if (searchCriteria.FeeDueDateFrom.Value > searchCriteria.FeeDueDateTo.Value)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.DueDateToPriorToDateFrom);
                }

                if (searchCriteria.FeeDueDateFrom.Value.AddDays(60) < searchCriteria.FeeDueDateTo.Value)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.AchFeeSearch.DueDateRangeError);
                }
            }

            if (hasErrors)
            {
                view.SetErrorMessages();
            }
            else
            {
                view.SetValidationSuccess();
            }
        }*/

        public void SaveRegHealthCareAffiliation(HealthCareAffiliations healthCareAff)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", healthCareAff.RegID.ToString());
            parms.Add("FacilityName", healthCareAff.FacilityName);
            parms.Add("FacilityMedicaidID", healthCareAff.FacilityMedicaidID);
            
            parms.Add("Is_Primary_Facility", healthCareAff.IsPrimaryFacility.ToString());
            parms.Add("IsInpatientSetting", healthCareAff.IsInpatientSetting.ToString());
            parms.Add("IsHospitalPrivileges", healthCareAff.IsHospitalPrivileges.ToString());
            
            if (!string.IsNullOrWhiteSpace(healthCareAff.StaffCategory))
            {

                parms.Add("StaffCategory", healthCareAff.StaffCategory);
            }

            if (!string.IsNullOrWhiteSpace(healthCareAff.StatusOfPrivileges))
            {

                parms.Add("StatusOfPrivileges", healthCareAff.StatusOfPrivileges);
            }
            /*if (healthCareAff.RegHealthCareID != null)
            {

                parms.Add("REG_HEALTH_CARE_FACILITY_AFFILIATION_ID", healthCareAff.RegHealthCareID.ToString());
            }*/
            if (!string.IsNullOrWhiteSpace(healthCareAff.StartDate.ToString()) && healthCareAff.StartDate != DateTime.MinValue)
            {

                parms.Add("StartDate", healthCareAff.StartDate.ToShortDateString());
            }
            else
            {
                parms.Add("StartDate", null);
            }
            if (!string.IsNullOrWhiteSpace(healthCareAff.EndDate.ToString()) && healthCareAff.EndDate != DateTime.MinValue)
            {

                parms.Add("EndDate", healthCareAff.EndDate.ToShortDateString());
            }
            else
            {

                parms.Add("EndDate", null);
            }
            if (healthCareAff.IsRestrictedPrivilege != null)
            {

                parms.Add("IsRestrictedPrivilege", healthCareAff.IsRestrictedPrivilege.ToString());
            }
            if (!string.IsNullOrWhiteSpace(healthCareAff.Reason))
            {

                parms.Add("Reason", healthCareAff.Reason.ToString());
            }
            if (!string.IsNullOrWhiteSpace(healthCareAff.HospitalPrivilegesReason))
            {

                parms.Add("HospitalPrivilegesReason", healthCareAff.HospitalPrivilegesReason.ToString());
            }
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", healthCareAff.UserID.ToString());

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            if (healthCareAff.RegHealthCareID == 0)
            {
                parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", healthCareAff.UserID.ToString());
                svc.InsertRegistrationDataTable("HEALTH_CARE_FACILITY_AFFILIATIONCustom", parms);
            } 
            else
            {
                parms.Add("REG_HEALTH_CARE_FACILITY_AFFILIATION_ID", healthCareAff.RegHealthCareID.ToString());
                svc.UpdateRegistrationDataTable("HEALTH_CARE_FACILITY_AFFILIATIONCustom", parms);
            }
        }



        public DataSet GetHealthCareAffiliationByID(int healthCareAffiliationID)
        {
            DataSet ds;
            ds = svc.SelectHealthCareAffiliationByID(healthCareAffiliationID);
            return ds;
        }

        public DataSet GetHospitalFacilityByMedicaidID(string MedicaidID)
        {
            DataSet ds;
            ds = svc.SelectHospitalFacilityByMedicaidID(MedicaidID);
            return ds;
        }


        public void DeleteHealthCareAffiliationByID(int healthCareAffiliationID)
        {
            svc.DeleteHealthCareFacilityAffiliationByID(healthCareAffiliationID);
            
        }

		#endregion
    
    }
}
