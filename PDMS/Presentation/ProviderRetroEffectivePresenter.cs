using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;
using Corp.Core.Libraries;
using System.Net;
using MAXIMUS.Controllers.PDMS;

namespace MAXIMUS.Presentation.PDMS
{
    public class ProviderRetroEffectivePresenter : PresenterBase, IPresenter<IProviderRetroEffectiveView, ProviderManagementData>
    {
        private readonly IProviderRetroEffectiveView view;

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
        public ProviderRetroEffectivePresenter(IProviderRetroEffectiveView view)
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

        public void RetroEffectiveDateProvider()
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
                svc.RetroEffectiveDateProvider(data.RegID, data.ChangeEffectiveDate.Value, data.Comments, data.LastModifiedDate.Value, data.LastModifiedUser.Value, false, data.PreviousEndDate.Value);
                
                //insert transaction queue and call web service
                string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");
                if (canMakeWSRequest.Equals("true"))
                {
                    int serviceLocationID = 0;
                    string txnResult = string.Empty;

                    DataSet ds = svc.SelectRegistrationData(data.RegID, "SERVICE_LOCATION");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        serviceLocationID = Convert.ToInt32(ds.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
                    
                    }
                
                    int tqId =  TransactionController.InsertTransactionQueue((int)TransactionController.TransactionTypeNew.SendMMISUpdate, data.RegID, serviceLocationID, DateTime.Now, null, null, DateTime.Now, Constants.appWorkflowUserId);
                    InfoAccessController.PopulateStagingData(tqId, Constants.TransactionTypeValues.MITS, string.Empty,true);
                                
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                                    
                    ProviderManagementReqRes prr = new ProviderManagementReqRes();
                    txnResult = prr.providerManagementUpdateRequest(tqId, true);
                    TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, DateTime.Now, DateTime.Now, Constants.appWorkflowUserId);
                }
                view.SetUpdateResults();
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

         public void ValidateRetroEffectiveInformation(DateTime? existingEffectiveDate, DateTime? existingRevalidationDate)
        {
            if (view.Model == null || view.Model.RegID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
            }

            if (!view.Model.ChangeEffectiveDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ChangeEffectiveDateRequired);
            }
            else if (existingEffectiveDate.HasValue && view.Model.ChangeEffectiveDate.Value.Date == existingEffectiveDate.Value.Date)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ChangeEffectiveSameDate);
            }
            else if (existingRevalidationDate.HasValue && view.Model.ChangeEffectiveDate.Value.Date > existingRevalidationDate.Value.Date)
            {
                this.ErrorList.Add(NextValidationKey(), "New Effective date cannot be greater than Current Revalidation Date");
            }

            if (string.IsNullOrEmpty(view.Model.Comments))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.CommentRequired);
            }

            DataSet ds = RegistrationController.CheckSpanOnProvwithSameNPIonEffectiveDateChange(view.Model.RegID, view.Model.ChangeEffectiveDate.Value.Date);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                this.ErrorList.Add(NextValidationKey(), "This causes an overlapping span on the NPI. You must first alter the end date of the previous record.");
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
