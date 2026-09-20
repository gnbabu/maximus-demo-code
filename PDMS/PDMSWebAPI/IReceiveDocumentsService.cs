using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PDMSWebAPI.DocumentService;

namespace PDMSWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReceiveDocumentsService" in both code and config file together.
    [ServiceContract(Namespace = "http://Maximus.OHPNM.Services/")]
    public interface IReceiveDocumentsService
    {
       
        [OperationContract]
        [FaultContract(typeof(ReceiveDocumentFault))]
        ReceiveDocumentResponse ReceiveAttachmentDocuments(ReceiveDocumentsRequest request);

    }
}
