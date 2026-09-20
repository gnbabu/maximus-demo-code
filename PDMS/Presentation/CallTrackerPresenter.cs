using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class CallTrackerPresenter : PresenterBase, IPresenter<ICallTrackerView, CallTrackingData>
    {
        private ICallTrackerView view = null;

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

        public CallTrackerPresenter(ICallTrackerView view)
        {
            Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new CallTrackingData();
            DataSet callReasons = svc.GetCallTrackingReasons();
            view.SetCallReasons(callReasons);
            DataSet callSources = svc.GetCallTrackingSources();
            view.SetCallSources(callSources);
            DataSet callNextActions = svc.GetCallTrackingNextActions();
            DataSet callResolutions = svc.GetCallTrackingResolutions();
            view.SetCallResolutions(callResolutions);
        }


        public void ValidationCallData()
        {
            CallTrackingData data = this.view.Model;

            if (string.IsNullOrEmpty(data.RegID) && !data.NoRegistrationOnFile)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.CallTracking.RegIDOrNotOnFileRequired);
            }

            if (data.SourceID <= 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.CallTracking.WhoIsCallerRequired);
            }

            if (data.SubjectID <= 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.CallTracking.ReasonForCallingRequired);
            }

            if (data.ResolutionID <= 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.CallTracking.ResolutionRequired);
            }

            if (hasErrors)
            {
                view.SetErrorMessages();
            }
            else
            {
                view.SetValidationSuccess();
            }
        }


        public void InsertNewCall()
        {
            try
            {
                if (view.Model == null)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.VIEW_CANNOT_BE_NULL);
                    view.SetErrorMessages();
                    return;
                }

                CallTrackingData data = view.Model;

                int newID = svc.InsertCallTrackingCall(data.SourceID, data.SubjectID, data.NextActionID, data.ResolutionID, data.StartTime.Value, data.EndTime.Value, data.Duration, data.RegID,
                        data.CallerOther, data.ReasonOther, data.CallDetails, data.NPI, data.MedicaidID, data.CreatedOn, data.CreatedBy);

                if (newID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.INSERT_FAILED);
                    view.SetErrorMessages();
                    return;
                }
                view.Model.CallTrackingID = newID;

                view.SetInsertResults();
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }


        #endregion

    }

}
