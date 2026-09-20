using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class OwnerIdentifyingInfoPresenter : PresenterBase, IPresenter<IOwnerIdentifyingInfoView, OwnerIdentifyingInfo>
    {
        private IOwnerIdentifyingInfoView view = null;

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

        public OwnerIdentifyingInfoPresenter(IOwnerIdentifyingInfoView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new OwnerIdentifyingInfo();
        }



        public void SaveRegOwnerIdentifyingInfo(OwnerIdentifyingInfo obj)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", obj.RegID.ToString());


            if (!string.IsNullOrEmpty(obj.EntityName.ToString()))
            {

                parms.Add("ENTITY_NAME", obj.EntityName.ToString());
            }

            if (obj.DBAName != null)
            {

                parms.Add("DBA_NAME", obj.DBAName.ToString());
            }

                      
            if (obj.SSN != null)
            {

                parms.Add("SSN", obj.SSN.ToString());
            }
            if (obj.TaxID != null)
            {

                parms.Add("Tax_ID", obj.TaxID.ToString());
            }

            if (obj.MedicaidID != null)
            {

                parms.Add("MEDICAID_ID", obj.MedicaidID.ToString());
            }

            if (obj.NPI != null)
            {

                parms.Add("NPI", obj.NPI.ToString());
            }

            parms.Add("OWNER_CATEGORY_TYPE_ID", obj.OWNER_CATEGORY_TYPE_ID.ToString());

            if (!string.IsNullOrWhiteSpace(obj.BirthDate.ToString()) && obj.BirthDate != DateTime.MinValue)
            {

                parms.Add("BIRTH_DATE", obj.BirthDate.ToShortDateString());
            }
            else
            {
                parms.Add("BIRTH_DATE", null);
            }

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", obj.UserID.ToString());

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            if (obj.REG_OWNER_PAPER_PROVIDER_ID == 0)
            {
                svc.InsertRegistrationDataTable("OWNER_PAPER_PROVIDER", parms);
            }
            else
            {
                parms.Add("REG_OWNER_PAPER_PROVIDER_ID", obj.REG_OWNER_PAPER_PROVIDER_ID.ToString());
                svc.UpdateRegistrationDataTable("OWNER_PAPER_PROVIDER", parms);
            }
        }



        public DataSet GetHealthCareAffiliationByID(int healthCareAffiliationID)
        {
            DataSet ds;
            ds = svc.SelectHealthCareAffiliationByID(healthCareAffiliationID);
            return ds;
        }



        public void DeleteHealthCareAffiliationByID(int healthCareAffiliationID)
        {
            svc.DeleteHealthCareFacilityAffiliationByID(healthCareAffiliationID);
            
        }

        public DataSet GetOwnerCategoryType()
        {
            return svc.SelectOwnerCategoryType();
        }
		#endregion
    
    }
}
