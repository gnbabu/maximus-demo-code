using MAXIMUS.Presentation.Interfaces.PDMS;
using Models.Data;

namespace Presentation.Interfaces
{
    public interface IOtherAddressView : IView<Address>
    {
	    void GetProviderAddressInfo(int regId, int addressTypeId);
    }
}
