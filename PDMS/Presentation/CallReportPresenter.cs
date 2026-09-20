using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class CallReportPresenter : PresenterBase, IPresenter<ICallReportView, CallTrackingData>
    {
        private ICallReportView view = null;

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

        public CallReportPresenter(ICallReportView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
		}

        public void Init()
        {
            this.view.Model = new CallTrackingData();
            DataSet callReasons = svc.GetCallTrackingReasons();
            view.SetCallReasons(callReasons);
            DataSet callSources = svc.GetCallTrackingSources();
            view.SetCallSources(callSources);
            DataSet callResolutions = svc.GetCallTrackingResolutions();
            view.SetCallResolutions(callResolutions);
        }
         
        public void Init(Guid userID, string taxID)
        {
            this.view.Model = new CallTrackingData();

            //int defaultPageSize = 15;
            //int defaultStartRowIndex = 0;
            //bool getTotalRowCount = true;
            //string defaultSortColumn = "Source";
        }

        //public void RequestCallsByDateRange(string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount)
        public void RequestCallsByDateRange(int outputType)
        {
            try
            {
                if (!view.Model.StartTime.HasValue && 
                    !view.Model.EndTime.HasValue && 
                    view.Model.SourceID == 0 && 
                    view.Model.SubjectID == 0 && 
                    view.Model.ResolutionID == 0 && 
                    string.IsNullOrEmpty(view.Model.NPI) && 
                    string.IsNullOrEmpty(view.Model.MedicaidID) && 
                    view.Model.CallTrackingID == 0)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.CallTracking.AtLeastOneFieldRequired);
                    return;
                }
                else if ((view.Model.StartTime.HasValue && !view.Model.EndTime.HasValue) ||
                        (!view.Model.StartTime.HasValue && view.Model.EndTime.HasValue)) // Start date but no end date or vice versa
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.CallTracking.StartDateAndEndDateRequired);
                    return;
                }

                CallTrackingData data = view.Model;
                DateTime startDate = DateTime.Parse("1/1/1900");
                DateTime endDate = DateTime.Parse("1/1/2100");

                if (data.StartTime.HasValue)
                    startDate = data.StartTime.Value;
                if (data.EndTime.HasValue)
                    endDate = data.EndTime.Value;
                
                DataSet ds = svc.SelectCallsByRange(startDate, endDate, data.SourceID, data.SubjectID, data.NextActionID, data.ResolutionID, data.CallTrackingID, data.NPI, data.MedicaidID);
                view.SetCallData(ds, outputType);
                
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }

        }

        public DataSet GetCallDetails(int callID)
        {
            DataSet ds = svc.SelectCallsByRange(DateTime.Parse("1/1/1900"), DateTime.Parse("1/1/2100"), 0, 0, 0, 0, callID, string.Empty, string.Empty);
            return ds;
        }

    }
}