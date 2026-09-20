using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.Presentation.PDMS
{
    public class ProviderAddPresenter : PresenterBase, IPresenter<IProviderAddView, ProviderManagementData>
    {
        Logging log = new Logging();
        private readonly IProviderAddView view;

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
        public ProviderAddPresenter(IProviderAddView view)
        {
            Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }


        #endregion
        public void Init()
        {
            this.view.Model = new ProviderManagementData();
            DataSet categories = svc.GetProviderCategories(true);
            DataTable dt = FilterCategories(categories);
            view.SetProviderCategories(dt.AsDataView());
        }

        public void Init(ProviderManagerData managerData)
        {
            //yes, the object is provider manager data
            this.view.Model = new ProviderManagementData();
            view.Model.RegID = managerData.RegID;
            view.Model.TaxID = managerData.TaxID;
            view.Model.TaxIDTypeID = managerData.TaxIDTypeID;
            view.Model.PaperRequestQueueID = managerData.PaperRequestQueueID;
            view.Model.WorkflowIDRequested = managerData.WorkflowIDRequested;
            view.Model.ReferralID = managerData.ReferralID;
            view.Model.ReferralTypeID = managerData.ReferralTypeID;
            view.Model.ApplicationTypeID = managerData.ApplicationTypeID;
            view.Model.IsProviderReactivation = managerData.IsProviderReactivation;
            RequestApplicationTypes();
            RequestProviderCategories(view.Model.WorkflowIDRequested, view.Model.ApplicationTypeID, view.Model.WaiverTypeID);

            if (hasErrors)
            {
                view.SetErrorMessages();
                return;
            }

        }

        //public void RequestProviderCategories(bool removeIndividual)
        //{
        //    DataSet categories = svc.GetProviderCategories(true);
        //    DataTable dt = categories.Tables[0];

        //    dt = FilterCategories(categories, removeIndividual);

        //    view.SetProviderCategories(dt.AsDataView());
        //}

        public void RequestProviderCategories(int workflowID, int applicationTypeID, int waiverTypeID)
        {
            DataSet categories = svc.GetProviderCategoriesByApplication(applicationTypeID, waiverTypeID);
            DataTable dt = categories.Tables[0];
            if (view.Model.ReferralID <= 0 || (view.Model.ReferralID > 0 && view.Model.ReferralTypeID == Constants.DiddReferralType.AssistedLiving)) //make this a property
            {
                dt = FilterCategories(categories);
            }

            view.SetProviderCategories(dt.AsDataView());
        }

        public void RequestApplicationTypes()
        {
            DataSet applicationTypes = svc.GetApplicationTypes();
            DataTable dt = applicationTypes.Tables[0];
            DataSet waiverTypes = svc.GetWaiverTypes();
            DataTable dtWaiver = waiverTypes.Tables[0];
            view.SetApplicationTypes(dt.AsDataView(), dtWaiver.AsDataView());
        }

        public void RequestCategoryDependentFields(int applicationTypeId, int categoryID, string providerTypeName, int requestedWorkflowID, int WaiverTypeID)
        {
            try
            {
                DataSet providerTypes = svc.SelectRegistrationProviderTypesByCategory(applicationTypeId, categoryID, WaiverTypeID);

                DataTable dt = FilterProviderTypes(providerTypes, providerTypeName, requestedWorkflowID);
                view.SetProviderTypes(dt.AsDataView());
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "RequestCategoryDependentFields failed with: " + ex.Message);
                view.SetErrorMessages();
            }
        }

        public void RequestPracticeTypes(int categoryID, int providerTypeID)
        {
            try
            {
                DataSet types = svc.GetTypeofPractice();
                DataTable dtPractice = FilterPracticeTypes(types, categoryID, providerTypeID);
                if (hasErrors)
                {
                    view.SetErrorMessages();
                    return;
                }
                view.SetPracticeTypes(dtPractice.AsDataView());
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "RequestPracticeTypes failed with: " + ex.Message);
                view.SetErrorMessages();
            }
        }

        public void RequestSpecialities(int providerTypeID)
        {
            try
            {
                DataSet ds = svc.SelectAllSpecialtiesByProviderType(providerTypeID);
                if (hasErrors)
                {
                    view.SetErrorMessages();
                    return;
                }
                view.SetSpecialtyTypes(ds);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "RequestSpecialities failed with: " + ex.Message);
                view.SetErrorMessages();
            }

        }

        public void RequestTaxonomies(int providerTypeID, int specialtyTypeID)
        {
            try
            {
                DataSet ds = svc.SelectTaxonomyTypesBySpecProvType(specialtyTypeID, providerTypeID);
                DataTable dt = FilterTaxonomyTypes(ds); //remove the undefined one - comes over for conversion, but can not move forward with.
                if (hasErrors)
                {
                    //view.SetErrorMessages();
                    return;
                }
                view.SetTaxonomyTypes(dt.AsDataView());
            }
            catch (Exception ex)
            {
                //this.ErrorList.Add(NextErrorKey(), "RequestCategoryDependentFields failed with: " + ex.Message);
                //view.SetErrorMessages();
            }

        }

        private DataTable FilterCategories(DataSet categories)
        {
            StringBuilder selectPart = new StringBuilder();


            DataTable dt = categories.Tables[0];
            if (dt.Select(selectPart.ToString()).Count() > 0)
            {
                return dt.Select(selectPart.ToString()).CopyToDataTable();
            }
            return dt;
        }

        private DataTable FilterCategories(DataSet categories, bool removeIndividual)
        {
            StringBuilder selectPart = new StringBuilder();
            
            if (removeIndividual)
                selectPart.Append(string.Format(" AND PROVIDER_CATEGORY_TYPE_ID <> {0}", Constants.ProviderCategoryTypeID.Individual));


            DataTable dt = categories.Tables[0];
            if (dt.Select(selectPart.ToString()).Count() > 0)
            {
                return dt.Select(selectPart.ToString()).CopyToDataTable();
            }
            return dt;
        }

        private DataTable FilterProviderTypes(DataSet providerTypes, string providerTypeName, int requestWorkflowID)
        {
            StringBuilder selectPart = new StringBuilder();
            DataTable dt = providerTypes.Tables[0];
            if (dt.Select(selectPart.ToString()).Count() > 0)
            {
                return dt.Select(selectPart.ToString()).CopyToDataTable();
            }
            return dt;
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


        private DataTable FilterPracticeTypes(DataSet practiceTypes, int categoryTypeID, int providerTypeID)
        {
            StringBuilder selectPart = new StringBuilder();
            DataTable dt = practiceTypes.Tables[0];

            //Filter by category and specific provider type
            selectPart.Append(string.Format("PROVIDER_CATEGORY_TYPE_ID = '{0}' AND PROVIDER_TYPE_ID = {1}", categoryTypeID, providerTypeID));
            if (dt.Select(selectPart.ToString()).Count() > 0)
            {
                return dt.Select(selectPart.ToString()).CopyToDataTable();
            }
            else
            {
                //Nothing found, so Filter by category and general provider type (0)
                selectPart.Clear();
                selectPart.Append(string.Format("PROVIDER_CATEGORY_TYPE_ID = '{0}' AND PROVIDER_TYPE_ID = 0", categoryTypeID));
                if (dt.Select(selectPart.ToString()).Count() > 0)
                {
                    return dt.Select(selectPart.ToString()).CopyToDataTable();
                }
            }

            return dt;
        }

        public string SelectAppSetting(string appSettingKey)
        {
            return AppSettings.Get(appSettingKey, string.Empty);
        }

        public void RequestConvertedRegData()
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }
                DataSet ds = svc.SelectConvertedRegistrationByRegID(view.Model.RegID);
                ProviderManagementData data = new ProviderManagementData();
                data.LoadObjectFromDataset(ds);
                view.SetConvertedProviderData(data);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
                view.SetErrorMessages();
            }
        }

        public void RequestPaperRequestData()
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
                    PaperRequestQueueData matchData = RequestPaperMatchData();
                    data.RegID = matchData == null ? -1 : matchData.RegID;
                    data.PartyID = matchData == null ? -1 : matchData.PartyID;
                    data.MedicaidID = matchData == null ? string.Empty : matchData.MedicaidID;
                    data.ReferralID = matchData == null ? -1 : matchData.ReferralID;
                    data.ConvertedProvider = matchData == null ? false : matchData.ConvertedProvider;
                }
                view.SetPaperRequestData(data);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "RequestPaperRequestData failed with: " + ex.Message);
                view.SetErrorMessages();
            }
        }


        public void RequestReferralData()
        {
            try
            {
                if (view.Model == null || view.Model.ReferralID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.ReferralIDRequired);
                    view.SetErrorMessages();
                    return;
                }
                DataSet ds = svc.SelectDIDDReferralByID(view.Model.ReferralID);
                ReferralData details = new ReferralData();
                details.LoadObjectFromDataset(ds);
                view.SetExistingReferralData(details);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "RequestReferralData failed with: " + ex.Message);
                view.SetErrorMessages();
            }
        }


        public void RequestExistingRegData()
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }
                DataSet ds = svc.SelectRegistrationByRegID(view.Model.RegID);
                ProviderManagementData details = new ProviderManagementData();
                details.LoadObjectFromDataset(ds);
                view.SetExistingProviderData(details);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "RequestExistingRegData failed with: " + ex.Message);
                view.SetErrorMessages();
            }
        }

        private int GetTaxonomyTypeID()
        {
            int taxonomyTypeID = 0;

            DataSet ds = svc.SelectTaxonomyInfoByCode(view.Model.TaxonomyCode, view.Model.ProviderTypeID);
            if (Methods.HasRows(ds))
                taxonomyTypeID = Methods.GetIntValue(ds.Tables[0].Rows[0], "TAXONOMY_TYPE_ID");

            if (taxonomyTypeID == 0)
            {
                taxonomyTypeID = svc.InsertTaxonomyCode(0, view.Model.ProviderTypeID, view.Model.TaxonomyCode, view.Model.TaxonomyDetail, Convert.ToDateTime("9999-12-31"), "", DateTime.Now, view.Model.UserID);
            }
            return taxonomyTypeID;
        }

        public void InsertProvider(int oldRegId = 0)
        {
            Dictionary<string, string> parmsPaper = new Dictionary<string, string>();


            try
            {
                if (view.Model == null)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.VIEW_CANNOT_BE_NULL);
                    view.SetErrorMessages();
                    return;
                }

                ProviderManagementData data = view.Model;

                if (data.TaxonomyTypeID == 0 && !string.IsNullOrEmpty(data.TaxonomyDetail))
                {
                    if (view.Model.WaiverTypeID != 4)
                    {
                        data.TaxonomyTypeID = GetTaxonomyTypeID(); //get taxonomy from NPPES API, if not yet in PDMS
                    }
                }


                parmsPaper.Add("ENTITY_NAME", data.ProviderName);
                parmsPaper.Add("DBA_NAME", data.DBA);
                parmsPaper.Add("NPI", data.NPI);
                if (data.TaxIDTypeID == 15)
                    parmsPaper.Add("SSN", data.TaxID);
                else
                    parmsPaper.Add("TAX_ID", data.TaxID);
                parmsPaper.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parmsPaper.Add("LAST_MODIFIED_USER", data.LastModifiedUser.ToString());
                parmsPaper.Add("BIRTH_DATE", data.BirthDate.ToString());
                parmsPaper.Add("MEDICAID_ID", data.MedicaidID);

                int workfloweventtypeid = oldRegId > 0 ? CON.WorkflowEventType.ChangeProviderType : CON.WorkflowEventType.NewReg;
                // Jira 3041 - include data.DDContractNumber
                int newID = svc.InsertNewProviderRegistration(data.UserID, data.ApplicationTypeID, data.ProviderName, data.DBA, data.FirstName, data.MiddleInitial, data.LastName,
                                 data.BirthDate, data.Gender, data.TaxIDTypeID, data.TaxID, data.NPI, data.ProviderTypeID, data.ProviderCategoryTypeID,
                                 data.TypeOfPracticeID, data.RequestedEffectiveDate, data.RegistrationStatusTypeID, data.ReferralID, data.SpecialtyTypeID,
                                 data.TaxonomyTypeID, data.PracticeLocation, data.ZipCode, data.ZipExt, data.IsPaperApplication, data.PaperRequestQueueID,
                                  data.LastModifiedDate.Value, data.LastModifiedUser.Value, view.Model.WorkflowID, view.Model.RegCreateDateTime, workfloweventtypeid, data.WaiverTypeID, data.RetroEffectiveDate, data.DDFacilityNumber, data.DDContractNumber);

                //Saving owner info  REG_OWNER_PAPER_PROVIDER
                parmsPaper.Add("REG_ID", newID.ToString());
                svc.InsertRegistrationDataTable("OWNER_PAPER_PROVIDER", parmsPaper);

                if (newID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.INSERT_FAILED);
                    view.SetErrorMessages();
                    return;
                }
                view.Model.RegID = newID;
                if (data.ApplicationTypeID == CON.ApplicationType.Standard || data.WaiverTypeID == 1)
                {
                    //save info to service location
                    Dictionary<string, string> pairs = new Dictionary<string, string>();
                    pairs.Add("REG_ID", data.RegID.ToString());
                    pairs.Add("DD_Contract_Number", data.DDContractNumber);
                    pairs.Add("DD_Facility_Number", data.DDFacilityNumber);
                    pairs.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    pairs.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());
                    svc.InsertRegistrationDataTable("SERVICE_LOCATIONcustom", pairs);
                }


                //save reg_event_info
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", data.RegID.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());
                parms.Add("NEW_REG_START_DATE_TIME", DateTime.Now.ToString());

                svc.InsertRegistrationDataTable("EVENT_INFO", parms);
                if (oldRegId == 0)
                {
                    svc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", data.RegID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", CON.WorkflowEventType.NewReg.ToString() } });
                }
                else
                {
                    svc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", data.RegID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", CON.WorkflowEventType.ChangeProviderType.ToString() } });
                }

                //update model with new workflow values
                DataSet ds = svc.SelectRegistrationByRegID(view.Model.RegID);
                ProviderManagementData details = new ProviderManagementData();
                details.LoadObjectFromDataset(ds);

                view.Model.ProcessID = details.ProcessID;
                view.Model.CurrentStepID = details.CurrentStepID;
                view.Model.CurrentTaskID = details.CurrentTaskID;
                view.Model.WorkflowID = details.WorkflowID;

                if (oldRegId > 0)
                {
                    svc.SaveProviderTypeChangeRequest(oldRegId, newID, details.ProcessID, CON.ProviderTypeChangeRequestStatus.InProcess, DateTime.Now, data.UserID);
                }

                if (hasErrors)
                {
                    view.SetErrorMessages();
                }

            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "InsertProvider failed with: " + ex.Message);
                view.SetErrorMessages();
                log.CreateEnrollmentLogEntry("InsertProvider failed with: " + ex.Message + " stacktrace:" + ex.StackTrace,
                    HttpContext.Current.User.Identity.Name, IsException:1, className: "ProviderAddPresenter");
            }
        }
        public void InsertExitingMedicaidId()
        {
            try
            {
                if (view.Model == null)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.VIEW_CANNOT_BE_NULL);
                    view.SetErrorMessages();
                    return;
                }
                Dictionary<string, object> parms = new Dictionary<string, object>();

                parms.Add("CREATED_BY_USER", view.Model.LastModifiedUser.ToString());
                parms.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());
                parms.Add("REG_ID", view.Model.RegID);
                parms.Add("MEDICAID_ID_SELL", view.Model.ExitingMedicaId);
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
                parms.Add("CHOP_TYPE_ID", 0);
                svc.InsertRegChopParent(parms);


            }
            catch (Exception ex)
            {
                log.CreateEnrollmentLogEntry("InsertExitingMedicaidId failed with: " + ex.Message + " stacktrace:" + ex.StackTrace,
                    HttpContext.Current.User.Identity.Name, IsException: 1, className: "ProviderAddPresenter");
                throw ex;
            }
        }

        public void InsertLinkedProvider()
        {
            Dictionary<string, string> parmsPaper = new Dictionary<string, string>();


            try
            {
                if (view.Model == null)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.VIEW_CANNOT_BE_NULL);
                    view.SetErrorMessages();
                    return;
                }

                ProviderManagementData data = view.Model;
                parmsPaper.Add("ENTITY_NAME", data.ProviderName);
                parmsPaper.Add("DBA_NAME", data.DBA);
                parmsPaper.Add("NPI", data.NPI);
                if (data.TaxIDTypeID == 15)
                    parmsPaper.Add("SSN", data.TaxID);
                else
                    parmsPaper.Add("TAX_ID", data.TaxID);
                parmsPaper.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parmsPaper.Add("LAST_MODIFIED_USER", data.LastModifiedUser.ToString());
                parmsPaper.Add("BIRTH_DATE", data.BirthDate.ToString());
                parmsPaper.Add("MEDICAID_ID", data.MedicaidID);

                int newID = svc.InsertNewLinkedProviderRegistration(data.UserID, data.ApplicationTypeID, data.ProviderName, data.DBA, data.FirstName, data.MiddleInitial, data.LastName,
                                data.BirthDate, data.Gender, data.TaxIDTypeID, data.TaxID, data.NPI, data.ProviderTypeID, data.ProviderCategoryTypeID,
                                data.TypeOfPracticeID, data.RequestedEffectiveDate, data.RegistrationStatusTypeID, data.ReferralID, data.SpecialtyTypeID,
                                data.TaxonomyTypeID, data.PracticeLocation, data.ZipCode, data.ZipExt, data.IsPaperApplication, data.PaperRequestQueueID,
                                 data.LastModifiedDate.Value, data.LastModifiedUser.Value, view.Model.WorkflowID, view.Model.RegCreateDateTime,
                                 CON.WorkflowEventType.NewReg, data.RegID);


                if (newID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.INSERT_FAILED);
                    view.SetErrorMessages();
                    return;
                }
                view.Model.RegID = newID;
                //save reg_event_info
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", data.RegID.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());
                parms.Add("NEW_REG_START_DATE_TIME", DateTime.Now.ToString());

                svc.InsertRegistrationDataTable("EVENT_INFO", parms);
                svc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", data.RegID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", CON.WorkflowEventType.NewReg.ToString() } });
                //update model with new workflow values
                DataSet ds = svc.SelectRegistrationByRegID(view.Model.RegID);
                ProviderManagementData details = new ProviderManagementData();
                details.LoadObjectFromDataset(ds);

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
                this.ErrorList.Add(NextErrorKey(), "InsertProvider failed with: " + ex.Message);
                log.CreateEnrollmentLogEntry("InsertLinkedProvider failed with: " + ex.Message + " - " + ex.StackTrace, 
                    HttpContext.Current.User.Identity.Name, IsException: 1, className: "ProviderAddPresenter");
                view.SetErrorMessages();
            }
        }

        public void UpdateToMedicaidProvider()
        {
            try
            {

                if (this.ErrorList.Count > 0)
                {
                    view.SetErrorMessages();
                    return;
                }

                Dictionary<string, string> parmsPaper = new Dictionary<string, string>();

                if (view.Model == null)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.VIEW_CANNOT_BE_NULL);
                    view.SetErrorMessages();
                    return;
                }
                //INSERT INTO VERSION Tables....
                DataSet ds = svc.InsertIntoVersionTables(view.Model.RegID, view.Model.UserID.ToString());

                DataRow dr;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dr = ds.Tables[0].Rows[0];
                    if (!string.IsNullOrEmpty(Methods.GetStringValue(dr["ErrorMessage"])))
                    {
                        this.ErrorList.Add(NextErrorKey(), Methods.GetStringValue(dr["ErrorMessage"]));
                        this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.CopyToVersionTablesFailed);
                        view.SetErrorMessages();
                        return;
                    }
                }
                
                ProviderManagementData data = view.Model;

                if (data.TaxonomyTypeID == 0 && !string.IsNullOrEmpty(data.TaxonomyDetail))
                {
                    if (view.Model.WaiverTypeID != 4)
                    {
                        data.TaxonomyTypeID = GetTaxonomyTypeID(); //get taxonomy from NPPES API, if not yet in PDMS
                    }
                }

                
                data.WaiverServiceUpdateTypeID = data.WaiverTypeID == CON.WaiverApplicationTypeID.ODA? CON.WaiverServiceUpdateType.ODA : CON.WaiverServiceUpdateType.ODM;


                svc.UpdatetoMedicaidProviderRegistration(data.RegID, data.UserID, data.NPI, data.ProviderTypeID, data.ProviderCategoryTypeID, data.TypeOfPracticeID, data.RequestedEffectiveDate, 
                                                         data.TaxonomyTypeID, data.ZipCode, data.ZipExt, data.LastModifiedDate.Value, data.LastModifiedUser.Value, data.WorkflowID, 
                                                         data.NPIStartDate.Value, data.NPIEndDate, data.EndDate, data.ApplicationTypeID, data.WaiverTypeID, CON.WorkflowEventType.UpdateReg, 
                                                         data.WaiverServiceUpdateTypeID, data.Gender);

                //Saving owner info  REG_OWNER_PAPER_PROVIDER
                parmsPaper.Add("ENTITY_NAME", data.ProviderName);
                parmsPaper.Add("DBA_NAME", data.DBA);
                parmsPaper.Add("NPI", data.NPI);
                if (data.TaxIDTypeID == 15)
                    parmsPaper.Add("SSN", data.TaxID);
                else
                    parmsPaper.Add("TAX_ID", data.TaxID);
                parmsPaper.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parmsPaper.Add("LAST_MODIFIED_USER", data.LastModifiedUser.ToString());
                parmsPaper.Add("BIRTH_DATE", data.BirthDate.ToString());
                parmsPaper.Add("REG_ID", data.RegID.ToString());
                parmsPaper.Add("MEDICAID_ID", data.MedicaidID);
                svc.InsertRegistrationDataTable("OWNER_PAPER_PROVIDER", parmsPaper);

                //update model with new workflow values
                DataSet dsReg = svc.SelectRegistrationByRegID(data.RegID);
                ProviderManagementData details = new ProviderManagementData();
                details.LoadObjectFromDataset(dsReg);
                
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
                this.ErrorList.Add(NextErrorKey(), "UpdateToMedicaidProvider failed with: " + ex.Message);
                view.SetErrorMessages();
                log.CreateEnrollmentLogEntry("UpdateToMedicaidProvider failed with: " + ex.Message +" stacktrace:"+ex.StackTrace,
                    HttpContext.Current.User.Identity.Name, IsException: 1, className: "ProviderAddPresenter");
            }
        }

        public void ConnectConvertedProvider()
        {
            try
            {
                Dictionary<string, string> parmsPaper = new Dictionary<string, string>();


                if (view.Model == null)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.VIEW_CANNOT_BE_NULL);
                    view.SetErrorMessages();
                    return;
                }

                ProviderManagementData data = view.Model;

                svc.UpdateConvertedProviderRegistration(data.RegID, data.UserID, data.ProviderName, data.DBA, data.FirstName, data.MiddleInitial, data.LastName,
                                data.BirthDate, data.Gender, data.TaxIDTypeID, data.TaxID, data.NPI, data.ProviderTypeID, data.ProviderCategoryTypeID, data.TypeOfPracticeID, data.RequestedEffectiveDate,
                                data.ReferralID, data.SpecialtyTypeID, data.TaxonomyTypeID, data.PracticeLocation, data.ZipCode, data.ZipExt, data.IsPaperApplication, data.PaperRequestQueueID,
                                data.LastModifiedDate.Value, data.LastModifiedUser.Value, data.WorkflowID, data.NPIStartDate.Value, data.NPIEndDate, data.EndDate);


                //Saving owner info  REG_OWNER_PAPER_PROVIDER
                parmsPaper.Add("ENTITY_NAME", data.ProviderName);
                parmsPaper.Add("DBA_NAME", data.DBA);
                parmsPaper.Add("NPI", data.NPI);
                if (data.TaxIDTypeID == 15)
                    parmsPaper.Add("SSN", data.TaxID);
                else
                    parmsPaper.Add("TAX_ID", data.TaxID);
                parmsPaper.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parmsPaper.Add("LAST_MODIFIED_USER", data.LastModifiedUser.ToString());
                parmsPaper.Add("BIRTH_DATE", data.BirthDate.ToString());
                parmsPaper.Add("REG_ID", data.RegID.ToString());
                parmsPaper.Add("MEDICAID_ID", data.MedicaidID);
                svc.InsertRegistrationDataTable("OWNER_PAPER_PROVIDER", parmsPaper);

                //update model with new workflow values
                DataSet ds = svc.SelectRegistrationByRegID(data.RegID);
                ProviderManagementData details = new ProviderManagementData();
                details.LoadObjectFromDataset(ds);

                //Reinstating a provider that is converted as terminated
                //if it is terminated and allowed to manage then add reinstate the provider
                //this is done here so the original converted file gets updated with the birthdate and other info incase the new 
                //registration created by reactivation is cancelled out by 60 days workflow job
                if (details.EnrollmentStatusCode != null &&
                  details.EnrollmentStatusCode != CON.EnrollStatus.ACTIVE.ToString() &&
                  details.EnrollmentStatusCode != CON.EnrollStatus.REPORTINGONLY.ToString())
                {
                    DataSet dsReact = svc.UpdateRegForReactivateByProvider(data.RegID, DateTime.Now, data.UserID.ToString(), data.WorkflowID);
                    DataTable dtReg = new DataTable();
                    if (dsReact.Tables.Count == 1)
                        dtReg = dsReact.Tables[0];
                    if (dsReact.Tables.Count > 1)
                        dtReg = dsReact.Tables[1];

                    if (dtReg.Rows.Count > 0)
                    {
                        if (Methods.ColumnExists("Reg_Id", dtReg.Rows[0]))
                            data.RegID = Methods.GetIntValue(dtReg.Rows[0], "Reg_Id");


                        if (Methods.ColumnExists("REQUESTED_EFFECTIVE_DATE", dtReg.Rows[0]))
                            data.RequestedEffectiveDate = Convert.ToDateTime(Methods.GetDateTimeValue(dtReg.Rows[0]["REQUESTED_EFFECTIVE_DATE"], true));

                        if (Methods.ColumnExists("NPI_End_Date", dtReg.Rows[0]))
                            data.NPIEndDate = Convert.ToDateTime(Methods.GetDateTimeValue(dtReg.Rows[0]["NPI_End_Date"], true));

                        if (Methods.ColumnExists("End_Date", dtReg.Rows[0]))
                            data.EndDate = Convert.ToDateTime(Methods.GetDateTimeValue(dtReg.Rows[0]["End_Date"], true));


                    }

                    DataSet dsNewReg = svc.SelectRegistrationByRegID(data.RegID); //getting details of new registration
                    details = new ProviderManagementData();
                    details.LoadObjectFromDataset(dsNewReg);
                }


                //bool ConvertedProvider = (details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion);
                string revalDueWindow = DataAccess.GetAppSetting("RevalidationDueWindow");
                details.RevalidationDueWindow = Convert.ToInt32(revalDueWindow) * -1;
                bool revalidationNeeded = !details.TerminationDate.HasValue && ((details.RevalidationDate.HasValue && details.RevalidationDate.Value.AddDays(details.RevalidationDueWindow) <= DateTime.Today) || (details.EndDate == null && details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion));
                if (details.ChangedApplicationTypeID)
                    revalidationNeeded = true;
                if (!revalidationNeeded)
                {
                    view.Model.WorkflowEventType = CON.WorkflowEventType.UpdateReg;
                }
                else
                {
                    view.Model.WorkflowEventType = CON.WorkflowEventType.RevalReg;
                }
                DataSet ds1 = svc.SelectRegistrationData(data.RegID, "EVENT_INFO");
                int REGEventInfoID = 0;

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", data.RegID.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());
                if (view.Model.WorkflowEventType == CON.WorkflowEventType.NewReg)
                {
                    parms.Add("NEW_REG_START_DATE_TIME", DateTime.Now.ToString());
                }
                else if (view.Model.WorkflowEventType == CON.WorkflowEventType.RevalReg)
                {
                    parms.Add("REVALIDATION_REG_START_DATE_TIME", DateTime.Now.ToString());

                }
                else if (view.Model.WorkflowEventType == CON.WorkflowEventType.UpdateReg)
                {
                    parms.Add("UPDATE_REG_START_DATE_TIME", DateTime.Now.ToString());
                }
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    REGEventInfoID = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_EVENT_INFO_ID"]);
                    parms.Add("REG_EVENT_INFO_ID", REGEventInfoID.ToString());
                    svc.UpdateRegistrationDataTable("EVENT_INFO", parms);
                }
                else
                {
                    svc.InsertRegistrationDataTable("EVENT_INFO", parms);
                }
                svc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", data.RegID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", view.Model.WorkflowEventType.ToString() } });

                

                view.Model.ProcessID = details.ProcessID;
                view.Model.CurrentStepID = details.CurrentStepID;
                view.Model.CurrentTaskID = details.CurrentTaskID;
                view.Model.WorkflowID = details.WorkflowID;
                
                svc.WF_SaveProcessParameter(view.Model.ProcessID, "WORKFLOW_EVENT_TYPE_ID", view.Model.WorkflowEventType.ToString());
                svc.InsertRegApplicationRecord(data.RegID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), view.Model.ProcessID);

                if (hasErrors)
                {
                    view.SetErrorMessages();
                }

            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "ConnectConvertedProvider failed with: " + ex.Message);
                view.SetErrorMessages();
                log.CreateEnrollmentLogEntry("ConnectConvertedProvider failed with: " + ex.Message + " stacktrace:" + ex.StackTrace,
                    HttpContext.Current.User.Identity.Name, IsException: 1, className: "ProviderAddPresenter");
            }
        }


        public void UpdateProviderAdminKeyFields()
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
                if (data.TaxonomyTypeID == 0 && !string.IsNullOrEmpty(data.TaxonomyDetail))
                {
                    if (view.Model.WaiverTypeID != 4)
                    {
                        data.TaxonomyTypeID = GetTaxonomyTypeID(); //get taxonomy from NPPES API, if not yet in PDMS
                    }
                }
                svc.AdminUpdateRegistrationKeyFields(data.RegID, data.ProviderName, data.FirstName, data.MiddleInitial, data.LastName,
                                data.TaxID, data.TaxIDTypeID, data.NPI, data.TaxonomyTypeID, data.ZipCode, data.ZipExt, data.Gender,
                                data.LastModifiedDate.Value, data.LastModifiedUser.Value, data.BirthDate);


                if (hasErrors)
                {
                    view.SetErrorMessages();
                }
                else
                {
                    view.SetKeyFieldUpdateResults();
                }
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "UpdateProviderKeyFields failed with: " + ex.Message);
                view.SetErrorMessages();
                log.CreateEnrollmentLogEntry("UpdateProviderAdminKeyFields failed with: " + ex.Message + " StackTrace:" + ex.StackTrace,
                    HttpContext.Current.User.Identity.Name, IsException: 1, className: "ProviderAddPresenter");
            }
        }
        public void UpdateProviderKeyFields()
        {
            try
            {
                log.CreateEnrollmentLogEntry("Inside UpdateProviderKeyFields", HttpContext.Current.User.Identity.Name, className: "ProviderAddPresenter");
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }

                ProviderManagementData data = view.Model;
                if (data.TaxonomyTypeID == 0 && !string.IsNullOrEmpty(data.TaxonomyDetail))
                {
                    if (view.Model.WaiverTypeID != 4)
                    {
                        data.TaxonomyTypeID = GetTaxonomyTypeID(); //get taxonomy from NPPES API, if not yet in PDMS
                    }
                }

                try
                {
                    log.CreateEnrollmentLogEntry(message: string.Format("Inside UpdateRegistrationKeyFields " +
                        "regid:{0}, providername:{1},firstname:{2},middleInitial:{3},lastname:{4}," +
                        "taxidtypeid:{5},providertypeid:[6},providercattypeid:{7},typeofpractiveid:{8}," +
                        "specialtytypeid:{9},taxonomytypid:{10},practicelocation:{11},zipcode:{12},zipext:{13}," +
                        "gender:{14},lastmodifieddate:{15},lastmofidereduser:{16},npistartdate:{17},npienddate:{18},npi:{19}",
                        data.RegID, data.ProviderName, data.FirstName, data.MiddleInitial, data.LastName,
                                    data.TaxIDTypeID, data.ProviderTypeID, data.ProviderCategoryTypeID, data.TypeOfPracticeID,
                                    data.SpecialtyTypeID, data.TaxonomyTypeID, data.PracticeLocation, data.ZipCode, data.ZipExt, data.Gender,
                                    data.LastModifiedDate.Value, data.LastModifiedUser.Value,
                                    data.NPIStartDate.Value, data.NPIEndDate, data.NPI), HttpContext.Current.User.Identity.Name, className: "ProviderAddPresenter");
                }
                catch (Exception) 
                { 
                    //Suppress exception related to logs insertion to avoid actual flow of the method
                }

                svc.UpdateRegistrationKeyFields(data.RegID, data.ProviderName, data.FirstName, data.MiddleInitial, data.LastName,
                                data.TaxIDTypeID, data.NPI, data.ProviderTypeID, data.ProviderCategoryTypeID, data.TypeOfPracticeID,
                                data.SpecialtyTypeID, data.TaxonomyTypeID, data.PracticeLocation, data.ZipCode, data.ZipExt, data.Gender,
                                data.LastModifiedDate.Value, data.LastModifiedUser.Value,
                                data.NPIStartDate.Value, data.NPIEndDate);


                if (hasErrors)
                {
                    view.SetErrorMessages();
                }
                else
                {
                    view.SetKeyFieldUpdateResults();
                }
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "UpdateProviderKeyFields failed with: " + ex.Message);
                log.CreateEnrollmentLogEntry("UpdateProviderKeyFields failed with: " + ex.Message + " StackTrace:" + ex.StackTrace, 
                    HttpContext.Current.User.Identity.Name, IsException: 1, className: "ProviderAddPresenter");
                view.SetErrorMessages();
            }
        }

        public void RequestRegistrationRecreate()
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }

                //delete current registration and workflow data
                svc.DeleteRegistration(view.Model.RegID, false);

                //insert provider, create workflow, reconnect paper queue request, if applicable.
                this.InsertProvider();

                //return back to ui to do what it wants to with success results
                view.SetRecreateSuccess();
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "RequestRegistrationRecreate failed with: " + ex.Message);
                view.SetErrorMessages();
            }
        }


        public void RequestRegistrationDelete()
        {
            try
            {
                if (view.Model == null || view.Model.RegID == 0)
                {
                    this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.RegIDRequired);
                    view.SetErrorMessages();
                    return;
                }

                //delete current registration and workflow data
                svc.DeleteRegistration(view.Model.RegID, false);

                //return back to ui to do what it wants to with success results
                view.SetDeleteOnlySuccess();
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "RequestRegistrationDelete failed with: " + ex.Message);
                view.SetErrorMessages();
            }
        }

        public void ValidateConvertedProviderForOperatorUpdates()
        {
            if (view.Model == null || string.IsNullOrEmpty(view.Model.TaxID))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.TaxIDRequired);
            }

            if (view.Model.ProviderCategoryTypeID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ProviderCategoryRequired);
            }

            if (view.Model.ProviderTypeID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ProviderTypeRequired);
            }
            //if (view.Model.TaxonomyTypeID == 0)
            //{
            //    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.TaxonomyRequired);
            //}
            /****************************************************************************************************
             *  SpecialtyTypeID is not required in DC version
             ****************************************************************************************************/
            //if (view.Model.SpecialtyTypeID == 0)
            //{
            //    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.SpecialtyRequired);
            //}


            if (!view.Model.RequestedEffectiveDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ReqEffectiveDateRequired);
            }

            switch (view.Model.ProviderCategoryTypeID)
            {
                case Constants.ProviderCategoryTypeID.Individual:
                    ValidateForIndividualOnlyFieldsForOperatorUpdates();
                    break;
                default:
                    ValidateForGroupOnlyFields();
                    break;
            }

            if (string.IsNullOrEmpty(view.Model.NPI) && view.Model.ProviderTypeID > 0)
            {
                if (NPIRequired())
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.NPIRequired);
                }
            }
            else
            {
                if (view.Model.CheckNPIRequired) //for new registrations and only if NPI is changed in EditKeyIdentifiers for Provider & Admin
                    ValidateForNPIType();
            }

            //ValidateForLocationFieldsForOperatorUpdates();

            //if (!hasErrors)
            //{
            //    ValidateForExistingProvider();
            //}

            if (hasErrors)
            {
                view.SetErrorMessages();
            }
            else
            {
                view.SetValidationSuccess();
            }
        }
        public void ValidateProviderInformation(bool isProviderTypeChangeRequest = false)
        {
            if (view.Model == null || string.IsNullOrEmpty(view.Model.TaxID))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.TaxIDRequired);
            }

            if (view.Model.ProviderCategoryTypeID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ProviderCategoryRequired);
            }

            if (view.Model.ProviderTypeID == 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ProviderTypeRequired);
            }

            /****************************************************************************************************
             *  SpecialtyTypeID is not required in DC version
             ****************************************************************************************************/
            //if (view.Model.SpecialtyTypeID == 0)
            //{
            //    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.SpecialtyRequired);
            //}


            if (!view.Model.RequestedEffectiveDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ReqEffectiveDateRequired);
            }

            if (view.Model.RequestedEffectiveDate.HasValue && view.Model.RetroEffectiveDate)
            {
                if (view.Model.RequestedEffectiveDate > DateTime.Now)
                    this.ErrorList.Add(NextValidationKey(), "Effective date cannot be in future.");

                if (Convert.ToDateTime(view.Model.RequestedEffectiveDate).Date < DateTime.Now.Date.AddDays(-365))
                    this.ErrorList.Add(NextValidationKey(), "Requested effective date cannot be less than 365 days from today's date");
            }

            switch (view.Model.ProviderCategoryTypeID)
            {
                case Constants.ProviderCategoryTypeID.Individual:
                    ValidateForIndividualOnlyFields();
                    break;
                default:
                    ValidateForGroupOnlyFields();
                    break;
            }

            if (!string.IsNullOrEmpty(view.Model.ExitingMedicaId))
            {
                var ds = svc.SelectExitingProviderByMedicaidID(view.Model.ExitingMedicaId);
                if (!(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ExitingMedicaidIDInvalid);
                }
                else
                {
                    var dt = ds.Tables[0];
                    string mmisProviderId = Convert.ToString(dt.Rows[0]["MMIS_PROVIDER_TYPE_ID"]);
                    string enrollmentStatusCode = Convert.ToString(dt.Rows[0]["ENROLLMENT_STATUS_CODE"]);
                    if (GetMMISProviderTypeID(view.Model.ProviderTypeID) != mmisProviderId)
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ExitingMedicaidIDProviderTypeNotMatch);
                    }
                    if(enrollmentStatusCode==CON.EnrollmentStatusTypeID.InActive.ToString())
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ExitingMedicaidIDInvalid);
                    }

                }
            }
            if (view.Model.TaxIDTypeID == CON.TaxIDType.SSN)
            {
                var result = svc.VerifyTaxIdExistsAsEINForSSN(view.Model.TaxID);
                if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.SSNIsInValid);
                }
            }
            if (view.Model.ApplicationTypeID == CON.ApplicationType.CPC)
            {
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(view.Model.NPI) && view.Model.ProviderTypeID > 0)
                {
                    if (NPIRequired())
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.NPIRequired);
                    }
                }
                else
                {
                    if (view.Model.CheckNPIRequired) //for new registrations and only if NPI is changed in EditKeyIdentifiers for Provider & Admin
                        ValidateForNPIType(isProviderTypeChangeRequest);
                }
            }
            if (view.Model.ApplicationTypeID == CON.ApplicationType.Standard && view.Model.MMISProviderTypeID == CON.MMISProviderType.NON_STATE_OPERATED_ICF_MR)
            {
                if (string.IsNullOrEmpty(view.Model.DDFacilityNumber))
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.DDFacilityNumberRequired);
                }

                if (!string.IsNullOrEmpty(view.Model.DDFacilityNumber))
                {
                    bool facilityExists = svc.CheckDDFacilityNumberExists(view.Model.DDFacilityNumber);
                    if (!facilityExists)
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.DDFacilityNumberNotExists);
                    }                    
                }
            }

            if (view.Model.GetNursingLicense())
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.NursingLicenseErrorMessage);
            }


            if (view.Model.AdminKeyFieldEditRequest)
            {
                ValidateForExistingTaxonomy();
            }
            if (view.Model.WaiverTypeID != 4)
            {
                //ValidateForLocationFields();
                if (view.Model.ApplicationTypeID == CON.ApplicationType.Internal) // NPI is optional for internal Application 
                {
                    if (!string.IsNullOrEmpty(view.Model.NPI) && string.IsNullOrEmpty(view.Model.TaxonomyCode)) //Only if NPI enterred then Taxonomy is required
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.TaxonomyRequired);
                    }

                }
                else
                {
                    if (!hasErrors && view.Model.TaxonomyTypeID == 0 && (view.Model.TaxonomyCode == "" || view.Model.TaxonomyCode == "0" || view.Model.TaxonomyCode == null))
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.TaxonomyRequired);
                    }
                }
            }



            this.ValidateNursingLicenese();
            if (!hasErrors)
            {
                if (view.Model.WaiverTypeID != 4)
                {
                    ValidateForExistingProvider();
                }

            }

            if (hasErrors)
            {
                view.SetErrorMessages();
            }
            //else
            //{
            //    view.SetValidationSuccess();
            //}
        }
        private void ValidateNursingLicenese()  //aksh
        {
            if (view.Model.ApplicationTypeName == CON.ApplicationTypeName.MedicaidWaiver_DODD || view.Model.ApplicationTypeName == CON.ApplicationTypeName.MedicaidWaiver_DODD)
            {
                if (view.Model.GetNursingLicense() && view.Model.ProviderTypeName != CON.ProviderType.NON_AGENCY_NURSE_RN_OR_LPN)
                {
                    this.ErrorList.Add("NursingLicense", ValidationConstants.ProviderManagerData.NursingLicenseErrorMessage);
                }
            }
        }

        private void ValidateForExistingTaxonomy()
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", view.Model.RegID.ToString());
            parms.Add("PrimaryFlag", "1");
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_TAXONOMY", parms);
            if (Methods.HasRows(ds))
            {
                DataTable dt = null;
                if (ds.Tables[0].Select("PRIMARY_FLAG = 0 AND TAXONOMY_TYPE_ID = " + view.Model.TaxonomyTypeID.ToString()).Length > 0)
                {
                    dt = ds.Tables[0].Select("PRIMARY_FLAG = 0 AND TAXONOMY_TYPE_ID = " + view.Model.TaxonomyTypeID.ToString()).CopyToDataTable();
                }
                if (Methods.HasRows(dt))
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ExistingSecondaryTaxonomy);
                }
            }
        }
        private void ValidateForIndividualOnlyFieldsForOperatorUpdates()
        {
            if (string.IsNullOrEmpty(view.Model.FirstName))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.FirstNameRequired);
            }
            if (string.IsNullOrEmpty(view.Model.LastName))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.LastNameRequired);
            }
            if (view.Model.TaxIDTypeID <= 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.TaxIDTypeRequired);
            }


            if (!view.Model.RequestedEffectiveDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ReqEffectiveDateRequired);
            }
        }
        private void ValidateForIndividualOnlyFields()
        {
            if (string.IsNullOrEmpty(view.Model.FirstName))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.FirstNameRequired);
            }
            if (string.IsNullOrEmpty(view.Model.LastName))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.LastNameRequired);
            }
            if (view.Model.TaxIDTypeID <= 0)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.TaxIDTypeRequired);
            }
            if (string.IsNullOrEmpty(view.Model.Gender))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.GenderRequired);
            }
            if (!view.Model.BirthDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.BirthDateRequired);
            }
            else
            {
                if (view.Model.BirthDate.Value > DateTime.Now || view.Model.BirthDate.Value < DateTime.Now.AddYears(-100))
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.BirthDateRange);
                }
                if (view.Model.BirthDate.Value > DateTime.Now.AddYears(-18))
                {
                    this.ErrorList.Add(NextValidationKey(), "Age must be 18 years or above.");
                }

            }
            if (!view.Model.RequestedEffectiveDate.HasValue)
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ReqEffectiveDateRequired);
            }
        }

        private void ValidateForGroupOnlyFields()
        {
            if (string.IsNullOrEmpty(view.Model.ProviderName))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.OrganizationNameRequired);
            }
        }

        private void ValidateForReferralOnlyFields()
        {
            if (string.IsNullOrEmpty(view.Model.ReferralNumber))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ReferralNumberRequired);
            }
            if (hasErrors)
                return;

            DataTable dtReferal = GetReferral(); //get referral based on id
            if (hasErrors)
                return;
            ValidateForReferralNumberExists(dtReferal);
            if (this.ErrorList.Count < 1)
            {
                ValidateDuplicateReferrals();
            }
        }

        //Pulls back all providers associated to this tax id, so can make filtering dynamic based on provider category/type.
        private DataTable GetReferral()
        {
            DataTable dt = null;
            DataSet ds = svc.SelectDIDDReferralByID(view.Model.ReferralID);
            if (Methods.HasRows(ds))
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        private void ValidateForReferralNumberExists(DataTable dt)
        {
            if (Methods.HasRows(dt))
            {
                DataTable dtMatch = FilteredForReferralNumber(dt);
                if (!Methods.HasRows(dtMatch))
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ApplicationNumberNotFound);
                }
                else if (!view.Model.KeyFieldEditRequest && !Methods.HasRows(FilteredForReferralNumberUsed(dtMatch)))
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ApplicationNumberAlreadyUsed);
                }
            }
            else
            {
                //FATAL
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ReferralIDRequired);
            }
        }

        private DataTable FilteredForReferralNumber(DataTable dt)
        {
            StringBuilder selectPart = new StringBuilder();
            selectPart.Append(string.Format("APPLICATION_NO = '{0}'", view.Model.ReferralNumber));

            if (dt.Select(selectPart.ToString()).Count() > 0)
            {
                return dt.Select(selectPart.ToString()).CopyToDataTable();
            }
            return null;
        }

        private void ValidateDuplicateReferrals()
        {
            DataSet ds = svc.SelectDuplicateReferrals(view.Model.ReferralID, view.Model.TaxID, view.Model.ZipCode, view.Model.ZipExt);
            if (Methods.HasRows(ds))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ExistingReferralFoundOthers);
            }
        }

        private DataTable FilteredForReferralNumberUsed(DataTable dt)
        {
            StringBuilder selectPart = new StringBuilder();
            selectPart.Append("UserID IS NULL");

            if (dt.Select(selectPart.ToString()).Count() > 0)
            {
                return dt.Select(selectPart.ToString()).CopyToDataTable();
            }
            return null;
        }


        private bool ValidName(string first_name, string last_name)
        {
            return (first_name.ToLower().Trim() == view.Model.FirstName.ToLower().Trim() && last_name.ToLower().Trim() == view.Model.LastName.ToLower().Trim());
        }

        private void ValidateForNPITypeFromDB()
        {
            /*
             Category 1: Individual - if the NPI exists on NPPES database then it must be Entity_Type = 1 NPI
             Category 2: Group/Institution - if the NPI exists on NPPES database then it must be Entity_Type = 2 NPI
             Category 3: Facility - if the NPI exists on NPPES database then it must be Entity_Type = 2 NPI
             Category 5: Pharmacy - if the NPI exists on NPPES database then it must be Entity_Type = 2 NPI
              * */
            try
            {
                DataSet dsNPPES = svc.Search_NPPES_EntityType(view.Model.NPI);
                if (!Methods.HasRows(dsNPPES))
                {
                    return;
                }

                int nppesTypeID = Methods.GetIntValue(dsNPPES.Tables[0].Rows[0]["Entity_Type"]);

                if (view.Model.ProviderCategoryTypeID == Constants.ProviderCategoryTypeID.Individual)
                {
                    if (nppesTypeID != 1)
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.NPIMustBeType1);
                        return;
                    }
                }
                else
                {
                    if (nppesTypeID != 2)
                    {
                        this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.NPIMustBeType2);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "ValidateForNPIType failed with: " + ex.Message);
                view.SetErrorMessages();
            }
        }

        private void ValidateForNPIType(bool isProviderTypeChangeRequest = false)
        {
            /*
             Category 1: Individual - if the NPI exists on NPPES database then it must be Entity_Type = 1 NPI
             Category 2: Group/Institution - if the NPI exists on NPPES database then it must be Entity_Type = 2 NPI
             Category 3: Facility - if the NPI exists on NPPES database then it must be Entity_Type = 2 NPI
             Category 5: Pharmacy - if the NPI exists on NPPES database then it must be Entity_Type = 2 NPI
              * */
            bool isValid = true;

            bool isNPIAPIEnabled = AppSettings.Get("NPI-Registry-Enabled").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ? true : false;

            if (!isNPIAPIEnabled)
                ValidateForNPITypeFromDB();
            else
            {
                view.SetTaxonomyTypesFromNPPES(null, false);
                try
                {
                    bool isNursingFacility = view.Model.ProviderTypeID == 85 || view.Model.ProviderTypeID == 86;
                    // OHPNM-9469 - JLB ensure NPI is not already associated with an active REG_ID
                    bool isNPIUnique = svc.ValidateNPIUniqueness(view.Model.RegID, view.Model.NPI, isNursingFacility);
                    if (!isNPIUnique && !isProviderTypeChangeRequest)
                    {
                        this.ErrorList.Add(NextValidationKey(), "The NPI is already active with another registration or is currently being processed.If you have questions, please contact the Integrated Help Desk at 1 - 800 - 686 - 1516, Option 2, Option 2'");
                        isValid = false;
                    }

                    NPPESAPIResult result = svc.ValidNPIinNPPESApi(Convert.ToInt64(view.Model.NPI));

                    if (result.result_count > 0)
                    {
                        int nppesTypeID = Convert.ToInt32(result.results[0].enumeration_type.Split(new string[] { "NPI-" }, StringSplitOptions.None).Last());
                       
                        if (view.Model.TaxIDTypeID != nppesTypeID)
                        {
                            if (view.Model.ProviderCategoryTypeID == Constants.ProviderCategoryTypeID.Individual)
                            {
                                if (nppesTypeID != 1)
                                {
                                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.NPIMustBeType1);
                                    isValid = false;
                                }
                            }
                            else
                            {
                                if (nppesTypeID != 2)
                                {
                                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.NPIMustBeType2);
                                    isValid = false;
                                }
                            }
                        }

                        if (isValid)
                        {
                            bool isValidName = false;
                            bool isValidGender = false;
                            string NPPESReturnedName = "";

                            if (view.Model.ProviderCategoryTypeID == Constants.ProviderCategoryTypeID.Individual)
                            {
                                foreach (Result rs in result.results) //currently one record found, need to change match logic if more found
                                {
                                    Basic basic = rs.basic;

                                    isValidName = ValidName(basic.first_name, basic.last_name);
                                    NPPESReturnedName = basic.first_name + " " + basic.last_name;

                                    //dont check for gender during admin key field edit 
                                    if (basic.gender == view.Model.Gender || basic.sex == view.Model.Gender) //|| view.Model.AdminKeyFieldEditRequest for edit key field and converttofee dont check gender
                                        //if undefined=U in UI dont check
                                        isValidGender = true;
                                }
                            }
                            else
                            {
                                //for all other scenarios dont check names and gender
                                isValidName = true;
                                isValidGender = true;
                            }

                            if (!isValidName)
                            {
                                //if (view.Model.AdminKeyFieldEditRequest)
                                //    this.ErrorList.Add(NextValidationKey(), "The first name and last name you enter must match the names in the NPPES system. Your name in NPPES is " + NPPESReturnedName + " If this name is incorrect, you must go the NPPES system to change it.");
                                //else
                                this.ErrorList.Add(NextValidationKey(), "There is a name mis-match with NPPES.");
                            }
                            if (!isValidGender)
                                this.ErrorList.Add(NextValidationKey(), "There is a gender mis-match with NPPES.");

                            //dont bring taxonomy from NPPES for Converted and Existing Providers
                           //if (view.Model.RegID == 0) //|| view.Model.AdminKeyFieldEditRequest
                            //{
                                if (isValidName && isValidGender &&
                                    (view.Model.TaxonomyCode == "" || view.Model.TaxonomyCode == "0" || view.Model.TaxonomyCode == null))
                                {
                                    view.SetTaxonomyTypesFromNPPES(result.results[0], true);
                                }
                            //}
                        }
                    }
                    else
                    {
                        this.ErrorList.Add(NextValidationKey(), "The NPI entered is not in the NPPES list.");
                    }
                }

                catch (Exception ex)
                {
                    this.ErrorList.Add(NextErrorKey(), "ValidateForNPIType failed with: " + ex.Message);
                    view.SetErrorMessages();
                }
            }
        }

        /*
                private DataTable dsFilterTaxonomyType()
                {
                    DataSet dsFilterTaxonomyType = svc.SelectTaxonomyTypesToExclude(view.Model.ProviderTypeID);
                    DataTable dtFilterTaxonomyType = null;
                    if (Methods.HasRows(dsFilterTaxonomyType))
                        dtFilterTaxonomyType = dsFilterTaxonomyType.Tables[0];
                    return dtFilterTaxonomyType;
                }

             */
        //OHPNM-16766 SAM479 - Hide zip and zip plus 4 fields from UI.
        /*private void ValidateForLocationFieldsForOperatorUpdates()
        {
            if (string.IsNullOrEmpty(view.Model.ZipCode))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ZipCodeRequired);
            }


        }
        private void ValidateForLocationFields()
        {
            if (string.IsNullOrEmpty(view.Model.ZipCode))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ZipCodeRequired);
            }

            if (string.IsNullOrEmpty(view.Model.ZipExt))
            {
                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ZipCodeExtRequired);
            }
        }*/

        private void ValidateForExistingProvider()
        {
            DataTable dt = GetFilteredForKeyFields();
            //Check for existence based on key fields
            if (!view.Model.AdminKeyFieldEditRequest)
            {
                if (!hasErrors)
                {
                    if (Methods.HasRows(dt))
                    {
                        DataTable dtExisting = ExistingProviderWithUser(dt);
                        if (Methods.HasRows(dtExisting)){ }
                        else if (view.Model.PaperRequestQueueID == 0 && view.Model.RegID == 0)
                        {
                            if (view.Model.ProviderCategoryTypeID == Constants.ProviderCategoryTypeID.Individual)
                            {
                                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.ExistingIndividualFoundOthers);
                            }
                            else
                            {
                                this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.FoundUnusedConvertedProvider);
                            }
                        }
                    }
                }
            }
            if (view.Model.AdminKeyFieldEditRequest)
            {
                DataTable dt1 = GetFilteredForAdminUpdateKeyFields();
                if (Methods.HasRows(dt1))
                {
                    this.ErrorList.Add(NextValidationKey(), ValidationConstants.ProviderManagerData.FoundExistingWhileAdminUpdatesKeyIdentifiers);
                }
            }
        }



        ////Pulls back all providers associated to this tax id, so can make filtering dynamic based on provider category/type.
        //private DataTable GetExistingProviders()
        //{
        //    DataTable dt = null;
        //    DataSet ds = svc.SelectRegistrationsByTaxID(view.Model.TaxID);
        //    if (Methods.HasRows(ds))
        //    {
        //        dt = FilteredForKeyFields(ds.Tables[0]);
        //    }


        //    return dt;
        //}

        private string GetTaxonomyCode()
        {
            string TaxCode = "";
            DataSet dsTaxo = svc.SelectTaxonomyTypeByID(view.Model.TaxonomyTypeID);

            if (Methods.HasRows(dsTaxo))
            {
                DataRow dr = dsTaxo.Tables[0].Rows[0];

                TaxCode = Methods.GetStringValue(dr["TAXONOMY_CODE"]);
            }

            return TaxCode;
        }

        private DataTable GetFilteredForAdminUpdateKeyFields()
        {
            DataSet ds = svc.GetAppSetting("WaiverServicesProviderTypeName");
            string ServicesProviderTypeName = Methods.HasRows(ds) ? ds.Tables[0].Rows[0][0].ToString() : string.Empty;

            string TaxCode = GetTaxonomyCode();

            bool IsFilterOtherWaivers = view.Model.ReferralID > 0 && view.Model.ReferralTypeID != Constants.DiddReferralType.AssistedLiving;

            int RegId = 0; //if we want to filter all other RegIDs but not self pass RegID or else 0 then will not filter
            if (view.Model.AdminKeyFieldEditRequest)
                RegId = view.Model.RegID;
            string taxid = "";
            taxid = view.Model.TaxID;

            DataSet dsReg = svc.SelectRegistrationsByAdminUpdateKeyFields(view.Model.NPI, TaxCode, view.Model.ProviderTypeID,
                GetMMISProviderTypeID(view.Model.ProviderTypeID), view.Model.ZipCode, view.Model.ZipExt, IsFilterOtherWaivers, ServicesProviderTypeName,
                RegId, taxid);

            DataTable dt = null;

            if (Methods.HasRows(dsReg))
                dt = dsReg.Tables[0];
            return dt;
        }
        private DataTable GetFilteredForKeyFields()
        {
            string ServicesProviderTypeName = AppSettings.Get("WaiverServicesProviderTypeName", string.Empty);
            string TaxCode = GetTaxonomyCode();

            bool IsFilterOtherWaivers = view.Model.ReferralID > 0 && view.Model.ReferralTypeID != Constants.DiddReferralType.AssistedLiving;

            int RegId = 0; //if we want to filter all other RegIDs but not self pass RegID or else 0 then will not filter
            if (view.Model.KeyFieldEditRequest)
                RegId = view.Model.RegID;


            DataSet dsReg = svc.SelectRegistrationsByKeyFields(view.Model.NPI, TaxCode, view.Model.ProviderTypeID,
                GetMMISProviderTypeID(view.Model.ProviderTypeID), view.Model.ZipCode, view.Model.ZipExt, IsFilterOtherWaivers, ServicesProviderTypeName,
                RegId, view.Model.TaxID, view.Model.ApplicationTypeID);

            DataTable dt = null;

            if (Methods.HasRows(dsReg))
                dt = dsReg.Tables[0];
            return dt;
        }



        public bool NPIRequired()
        {
            //New field on TAXONOMY_TYPE table, populated with defaults from PROVIDER TYPE with overrides by SPECIALTY_TYPE
            bool rtn = false;
            //DataSet ds = svc.SelectTaxonomyTypeByID(view.Model.TaxonomyTypeID);
            DataSet ds = svc.GetProviderTypeById(view.Model.ProviderTypeID);
            if (Methods.HasRows(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (Methods.GetIntValue(dr["REQUIRE_NPI"]) > 0) rtn = true;
                if (view.Model.WaiverTypeID == 4)
                {
                    rtn = false;
                }
                if (view.Model.ApplicationTypeID == CON.ApplicationType.Internal)
                {
                    rtn = false;
                }
            }
            return rtn;
        }

        public string GetMMISProviderTypeID(int providerTypeId)
        {
            string rtn = "";
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.GetProviderTypeById(providerTypeId);
            if (Methods.HasRows(ds))
            {
                rtn = Methods.GetStringValue(ds.Tables[0].Rows[0], "MMIS_PROVIDER_TYPE_ID");
            }
            return rtn;
        }

        private DataTable ExistingProviderWithUser(DataTable dt)
        {
            string selectPart = null;
            selectPart = string.Format("UserID IS NOT null");

            try
            {
                if (dt.Select(selectPart).Count() > 0)
                {
                    return dt.Select(selectPart).CopyToDataTable();
                }
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
            }
            return null;
        }

        private bool ExistingProviderWithMyUser(DataTable dt)
        {
            string selectPart = null;
            selectPart = string.Format("UserID IS NOT NULL AND UserID = '{0}'", view.Model.UserID.ToString());

            try
            {
                if (dt.Select(selectPart).Count() > 0)
                {
                    if (Methods.HasRows(dt.Select(selectPart).CopyToDataTable()))
                        return true;
                }
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), ex.Message);
            }
            return false;
        }

        private void CreateNewWorkflow()
        {
            int EntryTaskID = 1;
            //Spawn new workflow
            if (view.Model.ApplicationTypeID == CON.ApplicationType.Waiver &&
                (view.Model.WaiverTypeID == CON.WaiverApplicationTypeID.ODA || view.Model.WaiverTypeID == CON.WaiverApplicationTypeID.DODD || view.Model.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD))
            {
                EntryTaskID = CON.WaiverEntryTaskID;
            }
            Workflow.Process pr = new Workflow.Process(view.Model.WorkflowID, view.Model.UserID.ToString(), Guid.NewGuid(), EntryTaskID);
            if (pr == null)
            {
                this.ErrorList.Add(NextErrorKey(), ValidationConstants.ProviderManagerData.CreationOfNewWorkflowFailed);
                return;
            }

            view.Model.ProcessID = pr.ProcessID;
            view.Model.CurrentStepID = pr.CurrentStepID;
            view.Model.CurrentTaskID = pr.TaskID;
            view.Model.WorkflowID = pr.WorkflowID;

            // Save the Registration ID as a process parameter of the Workflow
            svc.WF_SaveProcessParameter(view.Model.ProcessID, "REGISTRATION_ID", view.Model.RegID.ToString());
        }

        private PaperRequestQueueData RequestPaperMatchData()
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
        public void ConvertToFeeForServive(int RegID)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = svc.SelectRegistrationByRegID(RegID);
            ProviderManagementData details = new ProviderManagementData();
            details.LoadObjectFromDataset(ds);

            psc.UpdateToConvertToFeeForService(RegID);
        }
        public ProviderManagementData GetConvertedData()
        {
            DataSet ds = svc.SelectConvertedRegistrationByRegID(view.Model.RegID);
            ProviderManagementData data = new ProviderManagementData();
            data.LoadObjectFromDataset(ds);
            return data;
        }

        public void InsertPrimaryServiceAddress()
        {
            try
            {
                if (view.Model == null)
                {
                    this.ErrorList.Add(NextErrorKey(), GenericErrorMessage.VIEW_CANNOT_BE_NULL);
                    view.SetErrorMessages();
                    return;
                }
                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("REG_ID_ENTERING", view.Model.RegID.ToString());
                parms.Add("MEDICAID_ID_EXITING", view.Model.ExitingMedicaId);
                parms.Add("CREATED_BY_USER", view.Model.LastModifiedUser.ToString());
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
                parms.Add("LAST_MODIFIED_USER", view.Model.LastModifiedUser.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                svc.InsertRegChopPrimaryServiceAddress(parms);
            }
            catch (Exception ex)
            {
                this.ErrorList.Add(NextErrorKey(), "Insert CHOP Provider Primary Service Address failed with: " + ex.Message);
                view.SetErrorMessages();
                log.CreateEnrollmentLogEntry("Insert CHOP Provider Primary Service Address failed with: " + ex.Message + "stacktrace:" + ex.StackTrace, 
                    HttpContext.Current.User.Identity.Name, IsException: 1 ,className : "ProviderAddPresenter");
            }
        }
    }
}
