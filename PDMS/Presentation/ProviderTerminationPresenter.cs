using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class ProviderTerminationPresenter : PresenterBase, IPresenter<IProviderTerminateView, ProviderManagementData>
    {
        private readonly IProviderTerminateView view;

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
        public ProviderTerminationPresenter(IProviderTerminateView view)
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

            GetEnrollmentStatuses();
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


        public void InsertExpressTerminateWorkflow()
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
                DataSet ds = svc.InsertExpressTerminatationWorkflow(view.Model.RegID, view.Model.LastModifiedDate.Value, view.Model.LastModifiedUser.Value);
                if (!Methods.HasRows(ds))
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ExpressTerminationData.WorkflowRequestUnsuccessful);
                    view.SetErrorMessages();
                    return;
                }

                DataSet regData = svc.SelectRegistrationByRegID(view.Model.RegID);
                ProviderManagementData details = new ProviderManagementData();
                details.LoadObjectFromDataset(regData);

                view.Model.ProcessID = details.ProcessID;
                view.Model.CurrentStepID = details.CurrentStepID;
                view.Model.CurrentTaskID = details.CurrentTaskID;
                view.Model.WorkflowID = details.WorkflowID;

                if (hasErrors)
                {
                    view.SetErrorMessages();
                }

            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void TerminateProvider()
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

                // Get prior enrollment status code
                string enrollmentStatusCodePrevious = string.Empty;
                DataSet dsProvider = svc.SelectRegistrationData(data.RegID, "PROVIDER");
                if (Methods.HasRows(dsProvider))
                {
                    DataRow rowProvider = dsProvider.Tables[0].Rows[0];
                    enrollmentStatusCodePrevious = Methods.GetStringValue(rowProvider, "ENROLLMENT_STATUS_CODE");
                }
                
                DataSet dsTransID = svc.TerminateProvider(data.RegID, data.TerminationDate.Value, (data.LastModifiedUser.Value).ToString(),data.EnrollmentStatusCode,data.enrollmentStatusReason,true, true);
                int transactionID = 0;
                if(Methods.HasRows(dsTransID))
                {
                    foreach (DataRow dr in dsTransID.Tables[0].Rows)
                    {
                        transactionID = Methods.GetIntValue(dr, "TRANSACTION_ID");
                        int transactionTypeID = Methods.GetIntValue(dr,"TRANSACTION_TYPE_ID");
                        if (transactionID > 0)
                        {
                            SendTerminationTransactionPayload stp = new SendTerminationTransactionPayload();
                            stp.SendTransaction(transactionID, transactionTypeID);
                        }
                    }
                }                

                view.SetUpdateResults();
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void ValidateTerminationInformation()
        {
            if (view.Model == null || view.Model.RegID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
            }

            if (!view.Model.TerminationDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.TerminationDateRequired);
            }
            else if (view.Model.ChangeEffectiveDate.HasValue)
            {
                if (view.Model.TerminationDate.Value < this.view.Model.ChangeEffectiveDate.Value)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ExpressTerminationData.TermDatePriorToEffectiveDate);
                }
            }
            if (string.IsNullOrEmpty(view.Model.Comments))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.CommentRequired);
            }
            if (string.IsNullOrEmpty(view.Model.EnrollmentStatusCode))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.NewEnrollmentStatusRequired);
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

        private void GetEnrollmentStatuses()
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }
                DataSet ds = svc.GetTerminationStatuses();
                view.SetEnrollmentStatuses(ds.Tables[0].AsDataView());
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }
    }
}
