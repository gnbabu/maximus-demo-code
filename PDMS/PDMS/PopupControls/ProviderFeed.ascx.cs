using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Crypto.Parameters;

public partial class PopupControls_ProviderFeed : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string Title
    {
        get { return "Provider Feed"; }
    }

    public override string IdText
    {
        get { return "ucProviderFeed_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "valProviderFeed"; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public override void LoadControlData()
    {
        throw new NotImplementedException();
    }

    public override void LoadData(DataRow dr = null)
    {
        throw new NotImplementedException();
    }

    public override bool SaveData()
    {
        throw new NotImplementedException();
    }

    public override bool ValidateData()
    {
        throw new NotImplementedException();
    }
}
