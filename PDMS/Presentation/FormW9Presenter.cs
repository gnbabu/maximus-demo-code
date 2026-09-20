using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using Models.Data;
using Presentation.Interfaces;
using System.Collections.Generic;

namespace Presentation
{
    public class TaxInfoPresenter : PresenterBase, IPresenter<IFormW9View, TaxInfo>
    {
	    private IFormW9View view = null;

	    public TaxInfoPresenter(IFormW9View view)
	    {
		    Check.IsNotNull(view, "view cannot be null");
		    this.view = view;
	    }

	    public void Init()
	    {
		    this.view.Model = new TaxInfo();
	    }

	    public int Insert(TaxInfo taxInfo, Dictionary<string, string> parms)
	    {
		    if (view.Model == null)
		    {
			    view.Model = new TaxInfo();
		    }

		    view.Model = taxInfo;
		    view.Model.RegTaxFormId = view.Model.Insert(taxInfo, parms);
		    return view.Model.RegTaxFormId;
	    }


	    public void Update(TaxInfo taxInfo, Dictionary<string, string> parms)
	    {
		    if (view.Model == null)
		    {
			    view.Model = new TaxInfo();
		    }

		    view.Model = taxInfo;
		    view.Model.Update(taxInfo, parms);
	    }
    }
}
