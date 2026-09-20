using Corp.Core.Libraries;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Corp.Core.Libraries.ServiceAgent
{
    public class AddressVerificatonDetail : Location
    {
        public string AddressLine2 { get; set; }
        public string StreetNumber { get; set; }
        public string PreDirectional { get; set; }
        public string StreetName { get; set; }
        public string StreetSuffix { get; set; }
        public string PostDirectional { get; set; }
        public string SecondaryDesignation { get; set; }
        public string SecondaryNumber { get; set; }
    }
}
