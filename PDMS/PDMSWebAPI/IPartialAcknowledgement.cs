using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PDMSWebAPI.Models;

namespace PDMSWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPartialAcknowledgement" in both code and config file together.
    [ServiceContract(Namespace= "http://Maximus.OHPNM.Services")]
    [XmlSerializerFormat]
    public interface IPartialAcknowledgement
    {
        [OperationContract]
        void TargetVendorResponse(TargetVendorResponse acknowledgementModel);
    }
}
