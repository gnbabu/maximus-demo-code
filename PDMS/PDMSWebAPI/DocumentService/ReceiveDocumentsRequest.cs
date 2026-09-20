using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.Runtime.Serialization;

namespace PDMSWebAPI.DocumentService
{

   
    [DataContract(Namespace = "http://Maximus.OHPNM.Services/" )]
    public class ReceiveDocumentsRequest
    {

        string transactionKey;

        string requestorSystem;

        DateTime requestTimeStamp;

        string moduleTransactionId;
        string additionalModuleTransactionId;

        string stateCode;
        SendAttachment[] attachments;

        string businessFlow;

     

        [DataMember(IsRequired = true,Order =0)]
        public string BusinessFlow { get => businessFlow; set => businessFlow = value; }

        [DataMember(IsRequired = true,Order =1)]
        public string StateCode { get => stateCode; set => stateCode = value; }

        [DataMember(IsRequired = true,Order =2)]
        public string RequestorSystem { get => requestorSystem; set => requestorSystem = value; }

        [DataMember(Order =3)]
        public string ModuleTransactionId { get => moduleTransactionId; set => moduleTransactionId = value; }
        [DataMember(Order = 4)]
        public string AdditionalModuleTransactionId { get => additionalModuleTransactionId; set => additionalModuleTransactionId = value; }

                     
        [DataMember(IsRequired = true,Order = 5)]
        public DateTime RequestTimeStamp { get => requestTimeStamp; set => requestTimeStamp = value; }

        [DataMember(IsRequired = true, Order = 6)]
        public string SITransactionKey { get => transactionKey; set => transactionKey = value; }


        [DataMember(IsRequired = true, Order = 7)]
        public SendAttachment[] Attachments { get => attachments; set => attachments = value; }
    }
}