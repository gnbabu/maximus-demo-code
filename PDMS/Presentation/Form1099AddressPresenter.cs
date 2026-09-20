using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using Models.Data;
using Presentation.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace Presentation
{
    public class Form1099AddressPresenter : PresenterBase, IPresenter<IForm1099AddressView, Form1099>
    {
	    private IForm1099AddressView view = null;

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

	    public Form1099AddressPresenter(IForm1099AddressView view)
	    {
		    Check.IsNotNull(view, "view cannot be null");
		    this.view = view;
	    }

	    public void Init()
	    {
		    this.view.TaxModel = new Form1099();
	    }

	    public int Insert(Form1099 form1099, Dictionary<string, string> parms)
	    {
		    if (view.TaxModel == null)
		    {
			    view.TaxModel = new Form1099();
		    }

		    view.TaxModel = form1099;
			view.TaxModel.RegFormId = view.TaxModel.Insert(form1099, parms);
		    return view.TaxModel.RegFormId;
	    }
		

        public void Update(Form1099 form1099, Dictionary<string, string> parms)
	    {
		    if (view.TaxModel == null)
		    {
			    view.TaxModel = new Form1099();
		    }

		    view.TaxModel = form1099;
		    view.TaxModel.Update(form1099, parms);
	    }


        public int Insert(Address address, Dictionary<string, string> parms)
        {
	        if (view.TaxAddress == null)
	        {
		        view.TaxAddress = new Address();
	        }

	        view.TaxAddress = address;
	        view.TaxAddress.AddressId = view.TaxAddress.Insert(address, parms);
	        return view.TaxAddress.AddressId;
        }

        public void Update(Address address, Dictionary<string, string> parms)
        {
	        if (view.TaxAddress == null)
	        {
		        view.TaxAddress = new Address();
	        }

            view.TaxAddress = address;
	        view.TaxAddress.Update(address, parms);
        }

        public DataSet GetProviderAddressInfo(int regId, int addressTypeId)
        {
	        var ds = svc.SelectProviderAddressInfo(regId, addressTypeId);
	        return ds;
        }
    }
}
