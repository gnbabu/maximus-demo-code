using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class ProviderManagerPresenter : PresenterBase, IPresenter<IProviderManagerView, ProviderManagerData>
    {
        private IProviderManagerView view = null;

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

        public ProviderManagerPresenter(IProviderManagerView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
		}

        public void Init()
        {
            this.view.Model = new ProviderManagerData();
        }
         
        public void Init(Guid userID, string taxID, int taxIDType, string loggedinUserID = "")
        {
            this.view.Model = new ProviderManagerData();
            this.view.Model.UserID = userID;
            this.view.Model.TaxID = taxID;
            this.view.Model.TaxIDTypeID = taxIDType;

            int defaultPageSize = 15;
            int defaultStartRowIndex = 0;
            bool getTotalRowCount = true;
            string defaultSortColumn = "ProviderName";

            //RequestMyProviderList(defaultSortColumn, defaultPageSize, defaultStartRowIndex, getTotalRowCount); 
            //if (hasErrors) return;

            //RequestMyGroupMemberProfileList(defaultSortColumn, defaultPageSize, defaultStartRowIndex, getTotalRowCount);
            //if (hasErrors) return;

            //RequestMyPendingReferralsList(defaultSortColumn, defaultPageSize, defaultStartRowIndex, getTotalRowCount);
            //if (hasErrors) return;

            //RequestPendingProviderList(defaultSortColumn, defaultPageSize, defaultStartRowIndex, getTotalRowCount);
            //if (hasErrors) return;

            //Modernization
            RequestAllProviderList(defaultSortColumn, defaultPageSize, defaultStartRowIndex, getTotalRowCount, loggedinUserID);
            if (hasErrors) return;
        }

        public void RequestMyProviderList(string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount)
        {
            try
            {
                if (view.Model == null || !view.Model.UserID.HasValue)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.UserIDRequired);
                    return;
                }
                int totalResultCount = 0;
                DataSet ds = svc.SelectRegistrationsByUserID(view.Model.UserID.Value, 0, Constants.ProviderCategoryTypeID.GroupMemberProfile, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
                view.SetMyProviders(ds, totalResultCount);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }

        }

        public void RequestAllProviderList(string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, string loggedinUserID)
        {
            try
            {
                if (view.Model == null || !view.Model.UserID.HasValue)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.UserIDRequired);
                    return;
                }
                //if (string.IsNullOrEmpty(view.Model.TaxID))
                //{
                //    this.ErrorList.Add(ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.TaxIDRequired);
                //    return;
                //}
                int totalResultCount = 0;
                //DataSet ds = svc.SelectAllRegistrationsByUserIDAndTaxID(view.Model.UserID.Value,view.Model.TaxID, 0, Constants.ProviderCategoryTypeID.GroupMemberProfile, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
                DataSet ds = svc.SelectAllRegistrationsByUserID(view.Model.UserID.Value, 0, Constants.ProviderCategoryTypeID.GroupMemberProfile, sortColumn, pageSize, startRowIndex, getTotalRowCount, loggedinUserID,out totalResultCount);
                view.SetAllProviders(ds, totalResultCount);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }

        }

        public void RequestMyGroupMemberProfileList(string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount)
        {
            try
            {
                if (view.Model == null || !view.Model.UserID.HasValue)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.UserIDRequired);
                    return;
                }
                int totalResultCount = 0;
                DataSet ds = svc.SelectRegistrationsByUserID(view.Model.UserID.Value, Constants.ProviderCategoryTypeID.GroupMemberProfile, 0, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
                view.SetMyGroupMemberProfiles(ds, totalResultCount);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void RequestPendingProviderList(string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount)
        {
            try
            {
                if (string.IsNullOrEmpty(view.Model.TaxID))
                {
                    this.ErrorList.Add(ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.TaxIDRequired);
                    return;
                }
                int totalResultCount = 0;
                DataSet ds = svc.SelectAllRegistrationsByTaxID(view.Model.TaxID, (Guid)view.Model.UserID, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
                view.SetPendingProviders(ds, totalResultCount);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }


        public void RequestMyPendingReferralsList(string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount)
        {
            try
            {
                if (view.Model == null || string.IsNullOrEmpty(view.Model.TaxID))
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.TaxIDRequired);
                    return;
                }
                int totalResultCount = 0;
                DataSet ds = svc.SelectReferral_UnusedByTaxID(view.Model.TaxID, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
                view.SetMyPendingReferrals(ds, totalResultCount);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void SetApplicationTypes()
        {
            try
            {
                DataSet ds = svc.GetApplicationTypes();
                view.SetApplicationTypes(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }


    }
}