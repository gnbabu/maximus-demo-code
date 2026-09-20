using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Pages_WorkflowSteps : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public override void LoadData(DataRow dr)
    {
    }

    public override bool SaveData()
    {
        return true;
    }

    public override bool ValidateData()
    {
        return true;
    }

    protected void ddlProcessIdList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadWorkflowInfo();
    }

    private void LoadWorkflowInfo()
    {
        if(ddlProcessIdList.SelectedIndex >= 0)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet workflowInfo = psc.GetWorkflowStepsForProcessID(int.Parse(ddlProcessIdList.SelectedValue));
            string wfEventType = string.Empty;

            lblProcessID.Text = ddlProcessIdList.SelectedValue;
            if (workflowInfo.Tables[1].Rows.Count > 0)
                lblRegId.Text = Convert.ToString(workflowInfo.Tables[1].Rows[0]["REG_ID"]);

            DataRow regInfo = Registration.GetRegistration(int.Parse(lblRegId.Text));
            lblTaxId.Text = Helper.GetString("TAX_ID", regInfo);
            lblNPI.Text = Helper.GetString("NPI", regInfo);
            //lblEnrollmentAction.Text = Helper.GetString("WORKFLOW_EVENT_TYPE", regInfo); 

            grdHistory.DataSource = workflowInfo.Tables[0];
            grdHistory.DataBind();

            DataSet dsProcessApplications = psc.GetApplicationDetailsbyProcessID(this.WorkflowPage.RegistrationId, int.Parse(ddlProcessIdList.SelectedValue));
            grdApplications.DataSource = dsProcessApplications.Tables[0];
            grdApplications.DataBind();


            if (dsProcessApplications.Tables[1].Rows.Count > 0)
                lblWaiverSvcUpd.Text = Convert.ToString(dsProcessApplications.Tables[1].Rows[0]["WAIVER_SERVICE_UPDATE_TYPE"]);
            else
                lblWaiverSvcUpd.Text = string.Empty;

            if (dsProcessApplications.Tables[2].Rows.Count > 0)
                lblDODDAppID.Text = Convert.ToString(dsProcessApplications.Tables[2].Rows[0]["DODDAppID"]);
            else
                lblDODDAppID.Text = string.Empty;

            if (dsProcessApplications.Tables[3].Rows.Count > 0)
                wfEventType = Convert.ToString(dsProcessApplications.Tables[3].Rows[0]["WORKFLOW_EVENT_TYPE"]);

            lblEnrollmentAction.Text = string.IsNullOrEmpty(wfEventType) ? Helper.GetString("WORKFLOW_EVENT_TYPE", regInfo) : wfEventType;

            //OHPNM-8272
            if (dsProcessApplications.Tables[4].Rows.Count > 0)
                lblDODDContractNo.Text = Convert.ToString(dsProcessApplications.Tables[4].Rows[0]["DD_CONTRACT_NUMBER"]);
            else
                lblDODDContractNo.Text = string.Empty;

        }
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Workflow Step";
    }
    public override void LoadControlData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet allProcesses = psc.GetWorkflowProcessesForRegID(this.WorkflowPage.RegistrationId);
		ddlProcessIdList.Items.Clear();
		ddlProcessIdList.DataSource = allProcesses;
        ddlProcessIdList.DataTextField = "WorkflowInfo";
        ddlProcessIdList.DataValueField = "PROCESS_ID";
        ddlProcessIdList.DataBind();

        //Set to latest process which should correspond to the latest registration latest process due to sql sort
        ddlProcessIdList.SelectedIndex = ddlProcessIdList.Items.Count - 1;

        LoadWorkflowInfo();

        LoadProgramData();
    }
    private void LoadProgramData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet workflowInfo = psc.GetCPCandCMCdtlsForRegID(this.WorkflowPage.RegistrationId);
        grdCPC.DataSource = workflowInfo.Tables[0];
        grdCPC.DataBind();

        grdCMC.DataSource = workflowInfo.Tables[1];
        grdCMC.DataBind();
    }
    public override string ValidationGroup
    {
        get { return "valWorkflowSteps"; }
    }

    public override string Title
    {
        get { return "Workflow Steps"; }
    }

    public override string IdText
    {
        get { return "ucWorkflowSteps_" + this.WorkflowPage.RegistrationId; }
    }

}