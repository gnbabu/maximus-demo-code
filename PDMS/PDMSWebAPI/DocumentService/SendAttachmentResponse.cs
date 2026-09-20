using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace PDMSWebAPI.DocumentService
{

    [DataContract(Namespace = "http://Maximus.OHPNM.Services/")]
    public class SendAttachmentResponse
    {


        string indexid;

        string status;

        string statusCode;

        string statusDescription;

        [DataMember(IsRequired = true,Order =0)]
        public string IndexID { get => indexid; set => indexid = value; }


        [DataMember(IsRequired = true, Order =1 )]
        public string ResponseCode { get => statusCode; set => statusCode = value; }

        [DataMember(IsRequired = true, Order = 2)]
        public string ResponseType { get => status; set => status = value; }

        [DataMember(IsRequired = true,Order = 3)]
        public string ResponseMessage { get => statusDescription; set => statusDescription = value; }
    }
}