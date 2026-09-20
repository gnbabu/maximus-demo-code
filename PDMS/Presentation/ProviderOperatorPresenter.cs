using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class ProviderOperatorPresenter : PresenterBase, IPresenter<IProviderOperatorView, PaperRequestQueueData>
    {
        private IProviderOperatorView view = null;

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

        public ProviderOperatorPresenter(IProviderOperatorView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
		}

        public void Init()
        {
            this.view.Model = new PaperRequestQueueData();
        }
         
        public void Init(Guid userID)
        {
            this.view.Model = new PaperRequestQueueData();
            this.view.Model.UserID = userID;
            this.view.Model.PaperRequestTypeID = 0;
            int defaultPageSize = 15;
            int defaultStartRowIndex = 0;
            bool getTotalRowCount = true;
            string defaultSortColumn = "Aging";

            RequestMyProviderList(defaultSortColumn, defaultPageSize, defaultStartRowIndex, getTotalRowCount);

            RequestMyDashboardList();
        }

        public void Init(Guid userID, int paperRequestTypeID)
        {
            this.view.Model = new PaperRequestQueueData();
            this.view.Model.UserID = userID;
            this.view.Model.PaperRequestTypeID = paperRequestTypeID;
            int defaultPageSize = 15;
            int defaultStartRowIndex = 0;
            bool getTotalRowCount = true;
            string defaultSortColumn = "Aging";

            RequestMyProviderList(defaultSortColumn, defaultPageSize, defaultStartRowIndex, getTotalRowCount);

            RequestMyDashboardList();
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
                DataSet ds = svc.SelectPaperRequestQueueByUserID(view.Model.UserID.Value, view.Model.PaperRequestTypeID, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
                view.SetMyProviders(ds, totalResultCount);

            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void RequestMyDashboardList()
        {
            try
            {
                if (view.Model == null || !view.Model.UserID.HasValue)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.UserIDRequired);
                    return;
                }

                DataSet ds = svc.SelectProviderOperatorDashboardByUserID(view.Model.UserID.Value);
                view.SetMyDashboard(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void RequestNextPaperRequest()
        {
            try
            {
                if (view.Model == null || !view.Model.UserID.HasValue)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.UserIDRequired);
                    return;
                }
                int nextID = svc.SelectNextUnassignedPaperRequest(view.Model.UserID.Value, DateTime.Now);
                if (nextID == 0)
                {
                    view.SetNextInQueueNotFound();
                }
                else
                {
                    view.SetNextInQueue(nextID);
                }
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }

        }

    }
}