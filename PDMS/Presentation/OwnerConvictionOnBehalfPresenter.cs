using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class OwnerConvictionOnBehalfPresenter : PresenterBase, IPresenter<IOwnerConvictionOnBehalfView, OwnerConvictionOnBehalf>
    {
        private IOwnerConvictionOnBehalfView view = null;

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

        public OwnerConvictionOnBehalfPresenter(IOwnerConvictionOnBehalfView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new OwnerConvictionOnBehalf();
        }



        public void SaveRegOwnerConvictionOnBehalf(OwnerConvictionOnBehalf ConOnBehalf)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", ConOnBehalf.RegID.ToString());
            parms.Add("Name", ConOnBehalf.Name);

            if (ConOnBehalf.Address1 != null)
            {

                parms.Add("Address1", ConOnBehalf.Address1.ToString());
            }

            if (!string.IsNullOrWhiteSpace(ConOnBehalf.Address2))
            {

                parms.Add("Address2", ConOnBehalf.Address2);
            }

            if (!string.IsNullOrWhiteSpace(ConOnBehalf.Address3))
            {

                parms.Add("Address3", ConOnBehalf.Address3);
            }

            if (!string.IsNullOrWhiteSpace(ConOnBehalf.BirthDate.ToString()) && ConOnBehalf.BirthDate != DateTime.MinValue)
            {

                parms.Add("BIRTH_DATE", ConOnBehalf.BirthDate.ToShortDateString());
            }
            else
            {
                parms.Add("BIRTH_DATE", null);
            }


            if (ConOnBehalf.Zip != null)
            {

                parms.Add("Zip", ConOnBehalf.Zip.ToString());
            }
            if (ConOnBehalf.ExtZip != null)
            {

                parms.Add("Ext_Zip", ConOnBehalf.ExtZip.ToString());
            }
            if (ConOnBehalf.County != null)
            {
                parms.Add("County", ConOnBehalf.County.ToString());
            }
            if (ConOnBehalf.City != null)
            {
                parms.Add("City", ConOnBehalf.City.ToString());
            }
            if (ConOnBehalf.State != null)
            {
                parms.Add("State", ConOnBehalf.State.ToString());
            }
            if (ConOnBehalf.SSN != null)
            {

                parms.Add("SSN", ConOnBehalf.SSN.ToString());
            }
            if (ConOnBehalf.TaxID != null)
            {

                parms.Add("Tax_ID", ConOnBehalf.TaxID.ToString());
            }
            if (!string.IsNullOrWhiteSpace(ConOnBehalf.MAtterOfOffense))
            {

                parms.Add("MAtterOfOffense", ConOnBehalf.MAtterOfOffense.ToString());
            }
            if (!string.IsNullOrWhiteSpace(ConOnBehalf.TimeOfOffense))
            {

                parms.Add("TimeOfOffense", ConOnBehalf.TimeOfOffense.ToString());
            }
            if (!string.IsNullOrWhiteSpace(ConOnBehalf.JuriDateOfOffense))
            {

                parms.Add("JuriDateOfOffense", ConOnBehalf.JuriDateOfOffense.ToString());
            }
            if (!string.IsNullOrWhiteSpace(ConOnBehalf.ProgramAreaOffense))
            {

                parms.Add("ProgramAreaOffense", ConOnBehalf.ProgramAreaOffense.ToString());
            }
            if (!string.IsNullOrWhiteSpace(ConOnBehalf.SanctionPeriodOffense))
            {

                parms.Add("SanctionPeriodOffense", ConOnBehalf.SanctionPeriodOffense.ToString());
            }
            if (!string.IsNullOrWhiteSpace(ConOnBehalf.FederalConviction))
            {

                parms.Add("FederalConviction", ConOnBehalf.FederalConviction.ToString());
            }
            if (!string.IsNullOrWhiteSpace(ConOnBehalf.StateOffense))
            {

                parms.Add("StateOffense", ConOnBehalf.StateOffense.ToString());
            }
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", ConOnBehalf.UserID.ToString());

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            if (ConOnBehalf.REG_OWNER_CONVICTION_ON_BEHALF_ID == 0)
            {
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", ConOnBehalf.UserID.ToString());
                svc.InsertRegistrationDataTable("OWNER_CONVICTION_ON_BEHALF", parms);
            }
            else
            {
                parms.Add("REG_OWNER_CONVICTION_ON_BEHALF_ID", ConOnBehalf.REG_OWNER_CONVICTION_ON_BEHALF_ID.ToString());
                svc.UpdateRegistrationDataTable("OWNER_CONVICTION_ON_BEHALF", parms);
            }
        }



        public DataSet GetOwnerConvictionOnBehalfByID(int ConOnBehalfID)
        {
            DataSet ds = null;
            //ds = svc.SelectConOnBehalfByID(ConOnBehalfID);
            view.SetOwnerConvictionOnBehalf(ds);
            return ds;
        }



        public void DeleteConOnBehalfiliationByID(int ConOnBehalfiliationID)
        {
            //svc.DeleteHealthCareFacilityAffiliationByID(ConOnBehalfiliationID);
            
        }


		#endregion
    
    }
}
