using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class CategoryOfServicePresenter : PresenterBase, IPresenter<ICategoryOfServiceView, CategoryOfService>
    {
        private ICategoryOfServiceView view = null;

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

        public CategoryOfServicePresenter(ICategoryOfServiceView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new CategoryOfService();
        }



        public void SaveRegCategoryOfService(CategoryOfService COS)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", COS.RegID.ToString());
            parms.Add("CATEGORY_OF_SERVICE_TYPE_ID", COS.CategoryOfServiceID.ToString());



            if (!string.IsNullOrWhiteSpace(COS.StartDate.ToString()) && COS.StartDate != DateTime.MinValue)
            {

                parms.Add("StartDate", COS.StartDate.ToShortDateString());
            }
            else
            {
                parms.Add("StartDate", null);
            }
            if (!string.IsNullOrWhiteSpace(COS.EndDate.ToString()) && COS.EndDate != DateTime.MinValue)
            {

                parms.Add("EndDate", COS.EndDate.ToShortDateString());
            }
            else
            {
                parms.Add("EndDate", null);
            }

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", COS.UserID.ToString());

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            if (COS.RegCategoryOfServiceInfoID == 0)
                svc.InsertRegistrationDataTable("CATEGORY_OF_SERVICE_INFO", parms);
            else
            {
                parms.Add("REG_CATEGORY_OF_SERVICE_INFO_ID", COS.RegCategoryOfServiceInfoID.ToString());
                svc.UpdateRegistrationDataTable("CATEGORY_OF_SERVICE_INFO", parms);
            }
        }



        public DataSet GetCategoryOfServiceByID(int CategoryOfServiceID)
        {
            DataSet ds;
            ds = svc.SelectCPRCertificationByID(CategoryOfServiceID);
            return ds;
        }

        public DataSet GetCategoryOfServiceByProviderTypeID(int providerTypeID)
        {
            DataSet ds;
            ds = svc.SelectCategoryOfServiceType(providerTypeID);
            return ds;
        }



		#endregion
    
    }
}
