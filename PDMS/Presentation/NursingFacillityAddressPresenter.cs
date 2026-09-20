using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using Models.Data;
using Presentation.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace Presentation
{
    public class NursingFacilityAddressPresenter : PresenterBase, IPresenter<INursingFacilityAddressView, Address>
    {
	    private INursingFacilityAddressView view = null;

	    #region svc
	    private PDMSService.PDMSServiceClient _svc;
	    private PDMSService.PDMSServiceClient svc
	    {
		    get
		    {
			    if (_svc == null)
			    {
				    _svc = new PDMSService.PDMSServiceClient();
			    }

			    return _svc;
		    }
	    }
	    #endregion

	    public NursingFacilityAddressPresenter(INursingFacilityAddressView view)
	    {
		    Check.IsNotNull(view, "view cannot be null");
		    this.view = view;
	    }
	    public void Init()
	    {
		    this.view.Model = new Address();
	    }

	    public int Insert(Address address, Dictionary<string, string> parms)
	    {
		    if (view.Model == null)
		    {
			    view.Model = new Address();
		    }

		    view.Model = address;
		    view.Model.AddressId = view.Model.Insert(address, parms);
		    return view.Model.AddressId;
	    }

	    public void Update(Address address, Dictionary<string, string> parms)
	    {
		    if (view.Model == null)
		    {
			    view.Model = new Address();
		    }

		    view.Model = address;
		    view.Model.Update(address, parms);
	    }

	    public DataSet GetProviderAddressInfo(int regId, int addressTypeId)
	    {
		    var ds = svc.SelectProviderAddressInfo(regId, addressTypeId);
		    return ds;
	    }
    }
}
