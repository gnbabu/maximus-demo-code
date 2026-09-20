using MAXIMUS.Presentation.Interfaces.PDMS;
using Models.Data;

namespace Presentation.Interfaces
{
    public interface IOwnerInfoView : IView<OwnerInfo>, IView<Address>
    {
		OwnerInfo OwnerInfo { get; set; }
		Address OwnerAddress { get; set; }
	    void GetOwnerAddressInfo(int regId, int addressTypeId);
    }
}
