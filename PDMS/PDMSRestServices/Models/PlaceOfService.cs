using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class PlaceOfService
    {
        public PlaceOfService(string placeofservice_code, string placeofservice_desc)
        {
            this.Placeofservice_Code = placeofservice_code;
            this.Placeofservice_Desc = placeofservice_desc;
        }
        private string _placeofservicecode;
        private string _placeofservicedesc;
        public string Placeofservice_Code
        {
            get { return _placeofservicecode; }
            set { _placeofservicecode = value; }
        }
        public string Placeofservice_Desc
        {
            get { return _placeofservicedesc; }
            set { _placeofservicedesc = value; }
        }
    }
}