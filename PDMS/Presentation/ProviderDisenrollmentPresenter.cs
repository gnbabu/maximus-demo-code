using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class ProviderDisenrollmentPresenter : PresenterBase, IPresenter<IProviderDisenrollmentView, ProviderManagementData>
    {
        private readonly IProviderDisenrollmentView view;

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
        public ProviderDisenrollmentPresenter(IProviderDisenrollmentView view)
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

        public void DisenrollProvider(bool IsSuspend=true)
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
                DataSet dsTransID = null;
                if (IsSuspend)
                    dsTransID = svc.SuspendProvider(data.RegID, data.TerminationDate.Value, data.Comments, data.LastModifiedDate.Value, data.LastModifiedUser.Value, true, data.EnrollmentStatusCode,true, data.enrollmentStatusReason);
                else
                    dsTransID = svc.DisenrollProvider(data.RegID, data.TerminationDate.Value, data.Comments, data.LastModifiedDate.Value, data.LastModifiedUser.Value, true, data.EnrollmentStatusCode, true);
                int transactionID = 0;
                if (dsTransID!=null && Methods.HasRows(dsTransID))
                {
                    foreach (DataRow dr in dsTransID.Tables[0].Rows)
                    {
                        transactionID = Methods.GetIntValue(dr, "TRANSACTION_ID");
                        int transactionTypeID = Methods.GetIntValue(dr, "TRANSACTION_TYPE_ID");
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

         public void ValidateDisenrollmentInformation(bool IsSuspend=false)
        {
            if (view.Model == null || view.Model.RegID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
            }

            if (!view.Model.TerminationDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), IsSuspend ? ValidationConstants.ProviderManagerData.DisenrollmentEffectiveDateRequired.Replace("Disenrollment", "Suspend Claims") :ValidationConstants.ProviderManagerData.DisenrollmentEffectiveDateRequired);
            }
            else if (view.Model.ChangeEffectiveDate.HasValue)
            {
                if (view.Model.TerminationDate.Value < this.view.Model.ChangeEffectiveDate.Value)
                {
                    this.ErrorList.Add(NextValidationKey(), IsSuspend ? ValidationConstants.DisenrollmentData.TermDatePriorToEffectiveDate.Replace("disenrollment", "Suspend Claims") : ValidationConstants.DisenrollmentData.TermDatePriorToEffectiveDate);
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

        public void DisenrollProviderInWorkflow( DataSet dsProcess,bool IsSuspend=true)
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
                DataSet dsTransID = null;
                if (IsSuspend)
                    dsTransID = svc.SuspendProvider(data.RegID, data.TerminationDate.Value, data.Comments, data.LastModifiedDate.Value, data.LastModifiedUser.Value, true, data.EnrollmentStatusCode, true, data.enrollmentStatusReason);
                else
                    dsTransID = svc.DisenrollProvider(data.RegID, data.TerminationDate.Value, data.Comments, data.LastModifiedDate.Value, data.LastModifiedUser.Value, false, data.EnrollmentStatusCode, true,false);
               
                view.SetUpdateResults();

                //Move WF After once its reviewed by ES.
               
                if (Methods.HasRows(dsProcess))
                {
                    DataRow drProcess = dsProcess.Tables[0].Rows[0];
                    int processId = Methods.GetIntValue(drProcess, "PROCESSID");
                    string currentTask = Methods.GetStringValue(drProcess, "CurrentTaskName");
                    if (processId > 0 && currentTask == Constants.RegistrationTaskName.ProviderReview)
                    {
                        svc.WF_TakeAction(processId, "Review Disenrollment Request", string.Empty);
                    }
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
