using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class Controls_PowerAgentEnable : System.Web.UI.UserControl
{
    // Event to notify parent when selection changes
    public event EventHandler<MAXIMUS.Models.PowerAgentEnableEventArgs> PowerAgentEnableChanged;

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

    public bool IsPowerAgentEnabled
    {
        get { return rblEnablePowerAgents.SelectedValue == "Yes"; }
    }

    public void InitView()
    {
        string loggedinUserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        int totalResultCount = 0;
        DataSet ds = svc.GetPowerAgentByUserId(loggedinUserID, 10, 1,SessionVarRetriever.SelectedProviderAdminUserID, out totalResultCount);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            rblEnablePowerAgents.SelectedValue = "Yes";
            rblEnablePowerAgents_SelectedIndexChanged(rblEnablePowerAgents, EventArgs.Empty);
            rblEnablePowerAgents.Enabled = false;
        }
        else
        {
            rblEnablePowerAgents.SelectedValue = "No";
            rblEnablePowerAgents_SelectedIndexChanged(rblEnablePowerAgents, EventArgs.Empty);
            rblEnablePowerAgents.Enabled = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitView();
        }
    }    

    protected void rblEnablePowerAgents_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool isEnabled = (rblEnablePowerAgents.SelectedValue == "Yes");

        // Raise event to notify parent
        if (PowerAgentEnableChanged != null)
        {
            PowerAgentEnableChanged(this, new MAXIMUS.Models.PowerAgentEnableEventArgs(isEnabled));
        }
    }
}

// Custom EventArgs class
