using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_WorkflowEventHistoryInfo : BaseSectionControl
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
        hdnRegIds.Value = WorkflowPage.RegistrationId.ToString();
        hdnWebAPIURLs.Value = ConfigurationManager.AppSettings["PDMSWebAPI"].ToString();
        CredentialHelper.APIToken apiToken = ApplicationCache.RestAPIAccessToken();

        if (apiToken != null)
        {
            hdnAccessTokens.Value = apiToken.AccessToken;
        }
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