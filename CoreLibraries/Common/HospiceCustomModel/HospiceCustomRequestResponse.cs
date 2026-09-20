using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Corp.Core.Libraries.HospiceReference
{
    [XmlRoot("SearchRequest")]
    public partial class SearchHospiceRequest
    {
    }

    [System.SerializableAttribute()]
    public partial class SearchHospiceResponse
    {
    }

    [XmlRoot("InquireRequest")]
    public partial class InquireHospiceRequest
    {
    }

    [System.SerializableAttribute()]
    public partial class InquireHospiceResponse
    {
    }

    [XmlRoot("HospiceRequestResponse")]
    [System.SerializableAttribute()]
    public partial class AddUpdateHospiceRequest
    {
    }

    [System.SerializableAttribute()]
    public partial class AddUpdateHospiceResponse
    {
    }
}
