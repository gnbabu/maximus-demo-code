using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class PaperRequestDocumentPresenter : PresenterBase, IPresenter<IPaperRequestDocumentView, PaperRequestDocumentData>
    {
        private IPaperRequestDocumentView view = null;

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

        public PaperRequestDocumentPresenter(IPaperRequestDocumentView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
		}

        public void Init()
        {
            this.view.Model = new PaperRequestDocumentData();
        }
         
        public void Init(int paperRequestQueueID)
        {
            this.view.Model = new PaperRequestDocumentData();
            this.view.Model.PaperRequestQueueID = paperRequestQueueID;

            RequestMyDocuments();
        }

        public void RequestMyDocuments()
        {
            try
            {
                if (view.Model == null || view.Model.PaperRequestQueueID == 0)
                {
                    this.ErrorList.Add(MAXIMUS.Presentation.PDMS.PresenterBase.ReturnType.VALIDATION_ERROR, ValidationConstants.PaperRequestQueueData.PaperRequestQueueIDRequired);
                    return;
                }
                DataSet ds = svc.SelectPaperRequestDocumentsByQueueID(view.Model.PaperRequestQueueID);
                view.SetMyDocuments(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public int InsertPaperRequestDocument()
        {
            int newID = -1;
            try
            {
                if (view.Model == null)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.VIEW_CANNOT_BE_NULL);
                    view.SetErrorMessages();
                    return newID;
                }

                newID = svc.InsertPaperRequestDocument(view.Model.PaperRequestQueueID, view.Model.Name, view.Model.Description, view.Model.FileName, view.Model.LastModifiedDate, view.Model.LastModifiedUser);

                if (newID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.INSERT_FAILED);
                    view.SetErrorMessages();
                    return newID;
                }

                view.SetInsertSuccess();
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
            return newID;
        }

    }
}