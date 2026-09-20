using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class CPRCertificationsPresenter : PresenterBase, IPresenter<ICPRCertificationsView, CPRCertifications>
    {
        private ICPRCertificationsView view = null;

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

        public CPRCertificationsPresenter(ICPRCertificationsView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new CPRCertifications();
        }



        public string SaveRegCPRCertification(CPRCertifications CPR)
        {
            int CPRRecordID = 0;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", CPR.RegID.ToString());
            parms.Add("IsCPRCertified", CPR.IsCPRCertified.ToString());
            parms.Add("Classifications", CPR.Classifications);
            if (!string.IsNullOrWhiteSpace(CPR.ExpirationDate.ToString()) && CPR.ExpirationDate != DateTime.MinValue && !string.IsNullOrEmpty(CPR.ExpirationDate.ToString()))
            {
                parms.Add("ExpirationDate", CPR.ExpirationDate.ToShortDateString());
            }
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", CPR.UserID.ToString());
            if (CPR.RegCPRCertificationID == 0)
            {
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", CPR.UserID.ToString());
                CPRRecordID = svc.InsertRegistrationDataTable("CPR_CERTIFICATION", parms);
                return CPRRecordID.ToString();
            }
            else
            {
                parms.Add("REG_CPR_CERTIFICATION_ID", CPR.RegCPRCertificationID.ToString());
                svc.UpdateRegistrationDataTable("CPR_CERTIFICATION", parms);
                CPRRecordID = CPR.RegCPRCertificationID;
                return CPRRecordID.ToString();
            }
        }

        public string SaveRegFirstAidCertification(CPRCertifications CPR)
        {
            int FARecordID = 0;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", CPR.RegID.ToString());
            parms.Add("IsFirstAidCertified", CPR.IsFirstAidCertified.ToString());
            parms.Add("FirstAidClassifications", CPR.FirstAidClassifications);
            if (!string.IsNullOrWhiteSpace(CPR.FirstAidExpirationDate.ToString()) && CPR.FirstAidExpirationDate != DateTime.MinValue && !string.IsNullOrEmpty(CPR.FirstAidExpirationDate.ToString()))
            {
                parms.Add("FirstAidExpirationDate", CPR.FirstAidExpirationDate.ToShortDateString());
            }

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", CPR.UserID.ToString());
            if (CPR.RegFirstAidCertificationID == 0)
            {
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", CPR.UserID.ToString());
                FARecordID = svc.InsertRegistrationDataTable("FIRSTAID_CERTIFICATION", parms);
                return FARecordID.ToString();
            }
            else
            {
                parms.Add("REG_FIRSTAID_CERTIFICATION_ID", CPR.RegCPRCertificationID.ToString());
                svc.UpdateRegistrationDataTable("FIRSTAID_CERTIFICATION", parms);
                FARecordID = CPR.RegFirstAidCertificationID;
                return FARecordID.ToString();
            }
        }


        public DataSet GetCPRCertificationByID(int CPRCertificationID)
        {
            DataSet ds;
            ds = svc.SelectCPRCertificationByID(CPRCertificationID);
            return ds;
        }





		#endregion
    
    }
}
