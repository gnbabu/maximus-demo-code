using Irony;
using System;

public partial class Process_PowerAgentAccessMgmt : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Initialize page
            ucPowerAgentGrid.ShowPanel(false);
        }
    }

    protected void ucGlobalAdminChange_AdminChanged(object sender, EventArgs e)
    {
        // Handle admin change
        ucPowerAgentEnable.InitView();
    }

    protected void ucPowerAgentEnable_PowerAgentEnableChanged(object sender, MAXIMUS.Models.PowerAgentEnableEventArgs e)
    {
        // Show or hide the Power Agent Grid based on selection
        ucPowerAgentGrid.ShowPanel(e.IsEnabled);
        
    }

    protected void ucPowerAgentGrid_PowerAgentAdded(object sender, EventArgs e)
    {
        // Handle power agent added
        // You can show a success message
    }

    protected void ucPowerAgentGrid_PowerAgentUpdated(object sender, EventArgs e)
    {
        // Handle power agent updated
        // You can show a success message
    }

    protected void ucPowerAgentGrid_PowerAgentDeactivated(object sender, EventArgs e)
    {
        // Handle power agent deactivated
        // You can show a success message
    }
}