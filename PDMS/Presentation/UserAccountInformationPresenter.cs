using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class UserAccountInformationPresenter : PresenterBase, IPresenter<IUserAccountInformationView, UserAccountInformation>
    {
        private IUserAccountInformationView view = null;

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

        public UserAccountInformationPresenter(IUserAccountInformationView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
		}

        public void Init()
        {
            this.view.Model = new UserAccountInformation();
        }

         
        public void Init(Guid userID)
        {
            this.view.Model = new UserAccountInformation();
            this.view.Model.UserID = userID;
            RequestUserData();
        }

        public void RequestUserData()
        {
            try
            {
                if (view.Model == null || !view.Model.UserID.HasValue)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.UserAccountInformation.UserIDRequired);
                    return;
                }
                DataSet ds = svc.GetUserAccountInformation(view.Model.UserID.Value.ToString());
                view.SetUserInformation(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

    }
}