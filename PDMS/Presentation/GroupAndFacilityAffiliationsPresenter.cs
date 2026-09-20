using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class GroupAndFacilityAffiliationsPresenter : PresenterBase, IPresenter<IGroupAndFacilityAffiliationsView, GroupAndFacilityAffiliations>
    {
        private IGroupAndFacilityAffiliationsView view = null;

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

        public GroupAndFacilityAffiliationsPresenter(IGroupAndFacilityAffiliationsView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new GroupAndFacilityAffiliations();
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
        public void GetRegPendingAffiliation(GroupAndFacilityAffiliations provider)
        {
            DataSet ds;
            ds = svc.SelectPendingAffiliationByRegID(provider.RegID);
            view.SetRegPendingAffiliation(ds);
        }
        public void GetRegConfirmedAffiliation(GroupAndFacilityAffiliations provider)
        {
            DataSet ds;
            ds = svc.SelectConfirmedAffiliationByRegID(provider.RegID);
            view.SetRegConfirmedAffiliation(ds);
        }
        public void GetRegHealthCareFacilityAffiliation(GroupAndFacilityAffiliations provider)
        {
            DataSet ds;
            ds = svc.SelectHealthCareFacilityAffiliationByRegID(provider.RegID);
            view.SetRegHealthCareFacilityAffiliation(ds);
        }
        public void GetRegAssignedDelegates(GroupAndFacilityAffiliations provider)
        {
            DataSet ds;
            ds = svc.SelectCredentialingDelegatesByRegID(provider.RegID);
            view.SetRegAssignedDelegates(ds);
        }
        #endregion

    }
}
