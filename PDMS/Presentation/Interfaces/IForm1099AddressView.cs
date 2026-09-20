using MAXIMUS.Presentation.Interfaces.PDMS;
using Models.Data;

namespace Presentation.Interfaces
{
	public interface IForm1099AddressView : IView<Form1099>, IView<Address>
	{
        Form1099 TaxModel { get; set; }

		Address TaxAddress { get; set; }

        void GetProviderAddressInfo(int regId, int addressTypeId);
	}
}