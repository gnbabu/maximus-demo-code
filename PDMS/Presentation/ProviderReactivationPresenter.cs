using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class ProviderReactivationPresenter : PresenterBase, IPresenter<IProviderReactivationView, ProviderManagementData>
    {
        private readonly IProviderReactivationView view;

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
 
        #region Public Methods
        public ProviderReactivationPresenter(IProviderReactivationView view)
		{
            Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

      
        #endregion

        public void Init()
        {
            this.view.Model = new ProviderManagementData();
        }

        public void Init(int regID)
        {
            this.view.Model = new ProviderManagementData();
            view.Model.RegID = regID;

            RequestRegistrationData();
        }


        private void RequestRegistrationData()
        {
            ProviderManagementData data = new ProviderManagementData();
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }
                DataSet ds = svc.SelectRegistrationByRegID(view.Model.RegID);
                data.LoadObjectFromDataset(ds);
                view.SetProviderInformation(data);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void ReactivateProvider()
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }

                ProviderManagementData data = view.Model;
                svc.ReactivateProvider(data.RegID, data.ChangeEffectiveDate.Value, data.RevalidationDate.Value, data.Comments, data.EnrollmentStatusCode, data.LastModifiedDate.Value, data.LastModifiedUser.Value);
                view.SetUpdateResults();
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void ValidateReactivationInformation()
        {
            if (view.Model == null || view.Model.RegID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
            }

            if (!view.Model.ChangeEffectiveDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ChangeEffectiveDateRequired);
            }
            if (!view.Model.RevalidationDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.RevalidationDateRequired);
            }
            else if (view.Model.ChangeEffectiveDate.HasValue)
            {
                if (view.Model.PreviousChangeEffectiveDate.HasValue && view.Model.EndDate.HasValue)
                {
                    if (view.Model.ChangeEffectiveDate.Value != view.Model.PreviousChangeEffectiveDate.Value
                        && view.Model.ChangeEffectiveDate.Value < view.Model.EndDate.Value)
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ReactivationData.EffectiveDatePriorToTerminationDate);
                    }
                    if (view.Model.RevalidationDate.Value < this.view.Model.ChangeEffectiveDate.Value)
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ReactivationData.RevalidationDatePriorToEffectiveDate);
                    }
                    if (view.Model.RevalidationDate.Value < this.view.Model.EndDate.Value)
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ReactivationData.RevalidationDatePriorToEndDate);
                    }
                }
            }
            if (string.IsNullOrEmpty(view.Model.Comments))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.CommentRequired);
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

    }
}
