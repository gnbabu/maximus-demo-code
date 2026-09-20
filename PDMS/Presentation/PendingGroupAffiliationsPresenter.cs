using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class PendingGroupAffiliationsPresenter : PresenterBase, IPresenter<IPendingGroupAffiliationsView, PendingGroupAffiliations>
    {
        private IPendingGroupAffiliationsView view = null;

        private string endDate = Convert.ToDateTime("12/31/2299").ToString();

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

        public PendingGroupAffiliationsPresenter(IPendingGroupAffiliationsView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new PendingGroupAffiliations();
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

        public void SaveRegPendingAffiliation(PendingGroupAffiliations pendingAff)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", pendingAff.RegID.ToString());
            parms.Add("GroupName", pendingAff.GroupName);

            if (!string.IsNullOrWhiteSpace(pendingAff.NPI))
            {

                parms.Add("NPI", pendingAff.NPI);
            }

            if (!string.IsNullOrWhiteSpace(pendingAff.MedicaidID))
            {

                parms.Add("MEDICAID_ID", pendingAff.MedicaidID);
            }

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", pendingAff.UserID.ToString());
            parms.Add("START_DATE", DateTime.Now.ToString());
            parms.Add("END_DATE", endDate);
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            if (pendingAff.RegPendingAffiliationID == 0)
            {
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", pendingAff.UserID.ToString());
                svc.InsertRegistrationDataTable("PENDING_AFFILIATION", parms);
            }
            else
            {
                parms.Add("REG_PENDING_AFFILIATION_ID", pendingAff.RegPendingAffiliationID.ToString());
                svc.UpdateRegistrationDataTable("PENDING_AFFILIATION", parms);
            }
        }

        public void SaveIndividualRegAffiliation(PendingGroupAffiliations pendingAff)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", pendingAff.GrpRegID.ToString());
            parms.Add("FIRST_NAME", pendingAff.FirstName);
            parms.Add("LAST_NAME", pendingAff.LastName);
            parms.Add("NAME", pendingAff.IndvName);
            parms.Add("NPI", pendingAff.IndvNPI);
            parms.Add("GROUP_AFFILIATION_STATUS_ID", pendingAff.grpAffiliationStatus.ToString());
            parms.Add("START_DATE", DateTime.Now.ToString());
            parms.Add("END_DATE", endDate);
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", pendingAff.UserID.ToString());
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", pendingAff.UserID.ToString());
            svc.InsertRegistrationData(pendingAff.GrpRegID, "AFFILIATIONcustom", parms);
        }

        public DataSet GetIndvRegistrationData(int RegID)
        {
            DataSet ds;
            ds = svc.SelectRegistrationData(RegID, "PROVIDER");
            return ds;
        }

        public DataSet GetProviderByMedicaidID(string MedicaidID)
        {
            DataSet ds;
            ds = svc.SelectProviderByGRPMedicaidID(MedicaidID);
            return ds;
        }

        public DataSet GetPendingAffiliationByID(int pendingAffiliationID)
        {
            DataSet ds;
            ds = svc.SelectPendingAffiliationByID(pendingAffiliationID);
            return ds;
        }

        public DataSet GetPendingAffiliationByMedicaidID(string medicaidID, int regID)
        {
            DataSet ds;
            ds = svc.SelectPendingAffiliationByMedicaidID(medicaidID, regID);
            return ds;
        }

        public DataSet GetAffiliationByStatusMedicaidId(string medicaidID, int regID, int grpAffiliationStatusId)
        {
            DataSet ds;
            ds = svc.SelectAffiliationByStatusMedicaidID(medicaidID, regID, grpAffiliationStatusId);
            return ds;
        }

        public DataSet GetAffiliationByGRPMedicaidID(int regid,string MedicaidID, string TaxID, string NPI)
        {
            DataSet ds;
            ds = svc.SelectAffiliationByGRPMedicaidID( regid, MedicaidID,  TaxID,  NPI);
            return ds;
        }

        public void DeletePendingAffiliationByID(int pendingAffiliationID)
        {
            svc.DeletePendingAffiliationByID(pendingAffiliationID);
            
        }

		#endregion
    
    }
}
