using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Data;
using System.Linq;
using System.Text;

namespace MAXIMUS.Presentation.PDMS
{
    public class PaperRequestAddPresenter : PresenterBase, IPresenter<IPaperRequestAddView, PaperRequestQueueData>
    {
        private readonly IPaperRequestAddView view;

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
        public PaperRequestAddPresenter(IPaperRequestAddView view)
		{
            Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

      
        #endregion
        public void Init()
        {
            this.view.Model = new PaperRequestQueueData();
            string waiverProviderTypeName = GetWaiverProviderTypeName();
            view.SetWaiverInfo(waiverProviderTypeName);
            DataSet categories = svc.GetProviderCategories(true);
            view.SetProviderCategories(categories);
        }

        public void Init(PaperRequestQueueData data)
        {
            this.view.Model = new PaperRequestQueueData();
            view.Model.PaperRequestQueueID = data.PaperRequestQueueID;

            string waiverProviderTypeName = GetWaiverProviderTypeName();
            view.SetWaiverInfo(waiverProviderTypeName);

            //Load initial drop downs
            DataSet ds = svc.GetProviderCategories(true);
            view.SetProviderCategories(ds);

            ds = svc.SelectPaperRequestTypes();
            view.SetPaperRequestTypes(ds);

            ds = svc.SelectPaperDocumentTypes();
            view.SetPaperDocumentTypes(ds);

            ds = svc.SelectPaperRequestStatusTypes();
            view.SetPaperRequestStatusTypes(ds);

            ds = svc.GetApplicationTypes();
            view.SetPaperApplicationTypes(ds);

            if (view.Model.PaperRequestQueueID > 0)
            {
                RequestPaperQueueData();
            }

        }

        public void RequestCategories(int applicationTypeId)
        {
            DataSet ds = svc.GetProviderCategoriesByApplication(applicationTypeId,0);
            if (hasErrors)
            {
                view.SetErrorMessages();
                return;
            }
            view.SetProviderCategories(ds);
        }

        public void RequestProviderTypes(int applicationTypeId, int categoryID)
        {
            DataSet ds = svc.GetProviderTypesByTypeId(applicationTypeId, categoryID, string.Empty);
            if (hasErrors)
            {
                view.SetErrorMessages();
                return;
            }
            view.SetProviderTypes(ds);
        }


        public void RequestSpecialities(int providerTypeID)
        {
            DataSet ds = svc.SelectAllSpecialtiesByProviderType(providerTypeID);
            if (hasErrors)
            {
                view.SetErrorMessages();
                return;
            }
            view.SetSpecialtyTypes(ds);

        }

        public void RequestTaxonomies(int providerTypeID, int specialtyTypeID)
        {
            DataSet ds = svc.SelectTaxonomyTypesBySpecProvType(specialtyTypeID, providerTypeID);
            DataTable dt = FilterTaxonomyTypes(ds);
            if (hasErrors)
            {
                view.SetErrorMessages();
                return;
            }
            view.SetTaxonomyTypes(dt.AsDataView());

        }

        public void RequestExceptionLog()
        {
            try
            {
                if (view.Model == null || view.Model.PaperRequestQueueID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.PaperRequestQueueData.PaperRequestQueueIDRequired);
                    view.SetErrorMessages();
                    return;
                }
                DataSet ds = svc.SelectPaperRequestErrorsByQueueID(view.Model.PaperRequestQueueID);
                view.SetExceptionLog(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }            
        }

        private PaperRequestQueueData RequestMatchData()
        {
            PaperRequestQueueData data = new PaperRequestQueueData();
            try
            {
                if (view.Model == null || view.Model.PaperRequestQueueID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.PaperRequestQueueData.PaperRequestQueueIDRequired);
                    view.SetErrorMessages();
                    return null;
                }
                DataSet ds = svc.SelectPaperRequestMatchData(view.Model.PaperRequestQueueID);
                data.LoadMatchFieldsFromDataset(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
            return data;
        }

        public void RequestPaperQueueData()
        {
            try
            {
                if (view.Model == null || view.Model.PaperRequestQueueID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.PaperRequestQueueData.PaperRequestQueueIDRequired);
                    view.SetErrorMessages();
                    return;
                }
                DataSet ds = svc.SelectPaperRequestQueueByQueueID(view.Model.PaperRequestQueueID);
                PaperRequestQueueData data = new PaperRequestQueueData();
                data.LoadPaperFieldsFromDataset(ds);

                if (data.RegID <= 0)
                {
                    PaperRequestQueueData matchData = RequestMatchData();
                    data.RegID = matchData == null ? -1 : matchData.RegID;
                    data.PartyID = matchData == null ? -1 : matchData.PartyID;
                    data.MedicaidID = matchData == null ? string.Empty : matchData.MedicaidID;
                    data.ReferralID = matchData == null ? -1 : matchData.ReferralID;
                    data.ConvertedProvider = matchData == null ? false : matchData.ConvertedProvider;
                    if (matchData != null)
                        data.RevalidationDate = matchData.RevalidationDate;
                }
                view.SetPaperRequestDetail(data);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void InsertPaperRequestQueueItem()
        {
            try
            {
                if (view.Model == null)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.VIEW_CANNOT_BE_NULL);
                    view.SetErrorMessages();
                    return;
                }

                PaperRequestQueueData data = view.Model;

                data.RequestStatusTypeID = Constants.PaperRequestStatusType.Received;

                int newID = svc.InsertPaperRequestQueue(data.RequestTypeID, data.DocumentTypeID, data.DocumentHandleID, data.RequestStatusTypeID, data.ApplicationTypeID, data.ProviderTypeID, data.SpecialtyTypeID,
                    data.TaxonomyTypeID, data.TaxonomyCode, data.TaxID, data.NPI, data.MedicaidID, data.ZipCode, data.ZipExt, data.Comments, data.CreatedOn, data.CreatedBy);

                if (newID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.INSERT_FAILED);
                    view.SetErrorMessages();
                    return;
                }
                else if (newID == -4)  //also validate against this.
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.PaperRequestQueueData.DocumentHandleExists);
                    view.SetErrorMessages();
                    return;
                }
                view.Model.PaperRequestQueueID = newID;

                svc.ValidatePaperRequest(newID, data.CreatedOn, data.CreatedBy);

                //run check for errors - want to insert record as is, then log errors - simulating data coming over from Onbase.
                view.SetInsertResults();
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

       

        public void UpdatePaperRequestQueueItem()
        {
            try
            {
                if (view.Model == null || view.Model.PaperRequestQueueID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.PaperRequestQueueData.PaperRequestQueueIDRequired);
                    view.SetErrorMessages();
                    return;
                }

                PaperRequestQueueData data = view.Model;

                svc.UpdatePaperRequestQueue(data.PaperRequestQueueID, data.RequestTypeID, data.DocumentTypeID, data.RequestStatusTypeID, data.ApplicationTypeID, data.ProviderTypeID, data.SpecialtyTypeID,
                    data.TaxonomyTypeID, data.TaxonomyCode, data.TaxID, data.NPI, data.MedicaidID, data.ZipCode, data.ZipExt, data.Comments, data.LastModifiedDate.Value, data.LastModifiedUser.Value);

                svc.ValidatePaperRequest(data.PaperRequestQueueID, data.LastModifiedDate.Value, data.LastModifiedUser.Value);

                if (data.RegID == 0)
                {
                    //check to see if can find match data.
                    PaperRequestQueueData matchData = RequestMatchData();
                    data.RegID = matchData == null ? -1 : matchData.RegID;
                    data.PartyID = matchData == null ? -1 : matchData.PartyID;
                    data.MedicaidID = matchData == null ? string.Empty : matchData.MedicaidID;
                    data.ReferralID = matchData == null ? -1 : matchData.ReferralID;
                    data.ConvertedProvider = matchData == null ? false : matchData.ConvertedProvider;
                    if (matchData != null)
                        data.RevalidationDate = matchData.RevalidationDate;
                }
                //run check for errors - want to insert record as is, then log errors - simulating data coming over from Onbase.
                view.SetUpdateResults(data);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void ValidateNewPaperRequest()
        {

            ValidateAlwaysRequiredFields();

            if (hasErrors)
            {
                view.SetErrorMessages();
            }
            else
            {
                view.SetValidationSuccess();
            }
        }

        private bool ValidateDocumentHandleExists()
        {
            DataSet ds = svc.SelectPaperRequestByDocumentHandle(view.Model.DocumentHandleID);
            return Methods.HasRows(ds) ? true : false;
        }

        private void ValidateAlwaysRequiredFields()
        {
            //On initial insert - very few fields are required.

            if (view.Model == null || string.IsNullOrEmpty(view.Model.UserID.ToString()))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.UserIDRequired);
            }

            if (view.Model.RequestTypeID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.PaperRequestQueueData.PaperRequestTypeRequired);
            }

            if (view.Model.DocumentHandleID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.PaperRequestQueueData.DocumentHandleRequired);
            }
            else if (view.Model.PaperRequestQueueID == 0)
            {
               if (ValidateDocumentHandleExists())
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.PaperRequestQueueData.DocumentHandleExists);
                }
            }

            if (view.Model.DocumentTypeID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.PaperRequestQueueData.DocumentTypeRequired);
            }
        }

        public void ValidateExistingPaperRequest()
        {
            ValidateAlwaysRequiredFields();

            if (string.IsNullOrEmpty(view.Model.Comments))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.PaperRequestQueueData.CommentRequired);
            }

            if (view.Model.ProviderCategoryTypeID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ProviderCategoryRequired);
            }

            if (view.Model.ProviderTypeID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ProviderTypeRequired);
            }

            //If the selected value is Re-Enrollment or Update Enrollment, then Provider Number is required        

            if (view.Model.MedicaidID.Trim() == "" &&  IsProviderNumberRequired(view.Model.RequestTypeID))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.MedicaidIDRequired);
            }

            //if (view.Model.SpecialtyTypeID == 0)
            //{
            //    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.SpecialtyRequired);
            //}

            if (view.Model.TaxonomyTypeID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.TaxonomyRequired);  //may change to accepting the code on the form
            }

            //if (string.IsNullOrEmpty(view.Model.NPI) && view.Model.ProviderTypeID > 0)
            //{
            //    if (NPIRequired())
            //    {
            //        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.NPIRequired);
            //    }
            //}

            if (view.Model.ProviderCategoryTypeID != Constants.ProviderCategoryTypeID.GroupMemberProfile)
            {
                if (string.IsNullOrEmpty(view.Model.ZipCode))
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ZipCodeRequired);
                }

                if (string.IsNullOrEmpty(view.Model.ZipExt))
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ZipCodeExtRequired);
                }
                else if (view.Model.ZipExt == "0000")
                {
                    string waiverProviderTypeName = GetWaiverProviderTypeName();
                    if (view.Model.ProviderTypeName != waiverProviderTypeName && view.Model.RequestTypeID == Constants.PaperRequestType.InitialEnrollment )
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.PaperRequestQueueData.ZipExtValueInvalid);
                    }
                }
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


        private DataTable FilterTaxonomyTypes(DataSet taxonomyTypes)
        {
            StringBuilder selectPart = new StringBuilder();
            selectPart.Append(string.Format("TAXONOMY_NAME <> '{0}'", "Not Defined"));

            DataTable dt = taxonomyTypes.Tables[0];
            if (dt.Select(selectPart.ToString()).Count() > 0)
            {
                return dt.Select(selectPart.ToString()).CopyToDataTable();
            }
            else
            {
                return new DataTable();
            }
        }

        private string GetWaiverProviderTypeName()
        {
            return  DataAccess.GetAppSetting("WaiverServicesProviderTypeName");
        }

        public bool IsProviderNumberRequired(int requestTypeID)
        {
            if (requestTypeID == Constants.PaperRequestType.ReEnrollment ||
                requestTypeID == Constants.PaperRequestType.AdditionalInformation ||
                requestTypeID == Constants.PaperRequestType.Reactivation  ||
                requestTypeID == Constants.PaperRequestType.Revalidation)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
