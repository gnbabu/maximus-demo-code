using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class CommunicationHistoryPresenter : PresenterBase, IPresenter<ICommunicationHistoryView, CommunicationEventData>
    {
        private ICommunicationHistoryView view = null;

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

        public CommunicationHistoryPresenter(ICommunicationHistoryView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
		}

        public void Init()
        {
            this.view.Model = new CommunicationEventData();
        }

         
        public void Init(int regID, int partyID, Guid userID)
        {
            this.view.Model = new CommunicationEventData();
            this.view.Model.RegID = regID;
            this.view.Model.PartyID = partyID;
            this.view.Model.UserID = userID;
            this.view.Model.PageNumber = 1;
            this.view.Model.RowsPerPage = 10;
            RequestContactEmailAddress();
            RequestCommunicationHistoryData();
        }

        public void RequestCommunicationHistoryData()
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.RegIDRequired);
                }
                if (!view.Model.UserID.HasValue)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.UserIDRequired);
                }
                if (this.hasErrors)
                    return;

                DataSet ds = svc.SelectEmailNotificationsByRegID(view.Model.UserID.Value, view.Model.RegID, view.Model.RowsPerPage, view.Model.PageNumber, view.Model.Subject,
                    view.Model.NPI, view.Model.SortByColName, view.Model.SortDirection);
                view.SetCommunicationHistory(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void RequestContactEmailAddress()
        {
            //not sure why we don't just get this in the main pull.
           // Get CONTACT_EMAIL_ADDRESS from reg_provider
             try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.RegIDRequired);
                    return;
                }

                DataSet ds = svc.SelectRegistrationData(view.Model.RegID, "PROVIDER");
                view.SetContactEmailInfo(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void RequestCredentialingEmailAddress()
        {
        // Get CREDENTIALING_EMAIL from usp_SelectProviderContactEmail
             try
            {
                if (view.Model == null)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.ProviderManagerData.PartyIDRequired);
                    return;
                }

            DataSet ds = svc.SelectProviderContactEmail(view.Model.RegID);
            view.SetCredentialingEmailInfo(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void RequestEmailAttachments(int communicationEventID)
        {
            try
            {
                if (communicationEventID == 0)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.CommunicationEventData.CommunicationEventIDRequired);
                    return;
                }

                DataSet ds = svc.GetEmailAttachments(communicationEventID.ToString());  //check on why to string
                view.SetEmailAttachments(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }


    }
}