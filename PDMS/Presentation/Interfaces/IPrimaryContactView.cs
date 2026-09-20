using MAXIMUS.Presentation.Interfaces.PDMS;
using Models.Data;

namespace Presentation.Interfaces
{
    public interface IPrimaryContactView : IView<Address>
    {
		Address PrimaryContact { get; set; }
	    void GetProviderAddressInfo(int regId, int addressTypeId);
    }
}
