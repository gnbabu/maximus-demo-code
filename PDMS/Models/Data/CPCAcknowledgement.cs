using MAXIMUS.Core.Libraries;
using Models.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models.Data
{
    public class CPCAcknowledgement
    {

        public int RegID { get; set; }


        private PDMSService.PDMSServiceClient _svc;

        private PDMSService.PDMSServiceClient svc
        {
            get { return _svc ?? (_svc = new PDMSService.PDMSServiceClient()); }
        }


        /// <summary>
        /// Creates a basic Disctionary to hold parameters.
        /// </summary>
        /// <param name="AckForm"></param>
        /// <returns></returns>
        public Dictionary<string, object> CreateParameterList()
        {
            Dictionary<string, object> parms = new Dictionary<string, object>();

            parms.Add("RegID", RegID.ToString());
            parms.Add("UserID", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            return parms;

        }

        public int SaveData(Dictionary<string, object> parms)
        {
            return svc.InsertRegistrationDataObject(parms);
        }

    }
}
