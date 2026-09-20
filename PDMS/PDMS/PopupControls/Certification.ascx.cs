using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MAXIMUS.Core.Libraries;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_Certification : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public event System.EventHandler ShowPendingStatus;
    public System.EventHandler InvalidateAgreements;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Certification)
        {
            btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup +
                "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        }

    }

    public void LoadData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CERTIFICATION");
        grdCertification.DataSource = ucCertification.DataList = ds.Tables[0];
        grdCertification.DataBind();
        btnHistoryCertification.Visible = (grdCertification.Rows.Count > 0);
    }

    public bool SaveData()
    {
        return true;
    }

    public bool ValidateData()
    {
        return true;
    }

    private void SetButtons(string commandName)
    {
        if (commandName == "History" || this.WorkflowPage.RegistrationId == 0)        // If on History OR do not have a RegistrationId created yet
        {
            btnSave.Visible = false;
            btnCancel.Text = "Close";
            return;
        }

        btnSave.Visible = true;
        btnCancel.Text = "Cancel";
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        DataRow dr;

        SetButtons(e.CommandName);
        switch (e.CommandName)
        {
            case "Certification":
                lblTitle.Text = "Certification Transmittal Information";
                btnSave.ValidationGroup = "valCertTrans";
                dr = ucCertification.DataList.Rows[index];
                ucCertification.LoadData(dr);
                mltPopup.ActiveViewIndex = 0;
                mpe.Show();
                break;
            default:
                break;
        }
    }

    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        SetButtons(e.CommandName);
        switch (e.CommandName)
        {
            case "Certification":
                lblTitle.Text = "Certification Transmittal Information";
                btnSave.ValidationGroup = "valCertTrans";
                ucCertification.LoadData(null);
                mltPopup.ActiveViewIndex = 0;
                mpe.Show();
                break;
        }
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        SetButtons("History");
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER_History");
        DataTable dt = null;
        if (Helper.HasRows(ds)) dt = ds.Tables[0];

        switch (e.CommandName)
        {
            case "Certification":
                lblTitle.Text = "Certification Transmittal Information History";
                mltPopup.ActiveViewIndex = 1;
                ucCertificationHistory.LoadData();
                mpe.Show();
                break;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            mpe.Show();
            return;
        }

        if (mltPopup.ActiveViewIndex == 0)
        {
            if (!ucCertification.ValidateData())
            {
                btnSave.Enabled = true;
                mpe.Show();
                return;
            }
            ucCertification.SaveData();
            Registration.SetNodeStatusId(this.WorkflowPage.RegistrationId, CON.RegistrationPageType.Certification, CON.RegistrationProviderServicesStatusTypeId.Pending);
            this.ShowPendingStatus(this, new EventArgs());
            LoadData();
        }
        btnSave.Enabled = true;
    }

  
}