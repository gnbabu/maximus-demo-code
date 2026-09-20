using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class NursingProfessionalCertificationsPresenter : PresenterBase, IPresenter<INursingProfessionalCertificationsView, NursingProfessionalCertifications>
    {
        private INursingProfessionalCertificationsView view = null;

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

        public NursingProfessionalCertificationsPresenter(INursingProfessionalCertificationsView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new NursingProfessionalCertifications();
        }



        public void SaveRegNursingProfessionalCertification(NursingProfessionalCertifications nursingProfessional)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", nursingProfessional.RegID.ToString());
            parms.Add("Certification_ID", nursingProfessional.CertificationName);



            if (!string.IsNullOrWhiteSpace(nursingProfessional.ReceivedFrom))
            {

                parms.Add("Received_From", nursingProfessional.ReceivedFrom);
            }


            if (!string.IsNullOrWhiteSpace(nursingProfessional.Expiration.ToString()) && nursingProfessional.Expiration != DateTime.MinValue)
            {

                parms.Add("Expiration", nursingProfessional.Expiration.ToShortDateString());
            }
            else
            {
                parms.Add("Expiration", null);
            }

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", nursingProfessional.UserID.ToString());

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            if (nursingProfessional.RegNursingProfessionalCertificationID == 0)
            {
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", nursingProfessional.UserID.ToString());
                svc.InsertRegistrationDataTable("NURSING_PROFESSIONAL_CERTIFICATION", parms);
            }
            else
            {
                parms.Add("REG_NURSING_PROFESSIONAL_CERTIFICATION_ID", nursingProfessional.RegNursingProfessionalCertificationID.ToString());
                svc.UpdateRegistrationDataTable("NURSING_PROFESSIONAL_CERTIFICATION", parms);
            }
        }



        public DataSet GetNursingProfessionalCertificationByID(int NursingProfessionalCertificationID)
        {
            DataSet ds;
            ds = svc.SelectNursingProfessionalCertificationByID(NursingProfessionalCertificationID);
            return ds;
        }





		#endregion
    
    }
}
