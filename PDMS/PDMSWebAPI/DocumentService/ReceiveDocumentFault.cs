using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace PDMSWebAPI.DocumentService
{
    [DataContract(Namespace = "http://Maximus.OHPNM.Services/")]
    public class ReceiveDocumentFault
    {

        string errorCode = string.Empty;
        string errorMessage = string.Empty;
        [DataMember]
        public string ErrorCode { get => errorCode; set => errorCode = value; }
        [DataMember]
        public string ErrorMessage { get => errorMessage; set => errorMessage = value; }
    }
}