using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_TotalNumberBeds : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

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

    private DataTable dt;
    private void SetDt()
    {
        if (dt == null)
        {
            DataSet ds = svc.SelectLicTotNumOfBeds(this.WorkflowPage.RegistrationId);
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }

    public override bool SaveData()
    {
        Page.Validate("TotalBeds");
        
        if (Page.IsValid)
        {

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();

            bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);

            int numBeds;

            if (Int32.TryParse(txtTotalBeds.Text, out numBeds))
            {
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("TOTAL_NUM_BEDS", numBeds.ToString());
                psc.UpdateRegistrationDataTable("NUMBER_OF_BEDScustom", parms);
            }
            else
            {
                pdms_Beds.Text = "Please enter a valid number.";
            }

        }
        else
            return false;

        return true;
    }

    private void LoadNumberOfBeds()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectLicTotNumOfBeds(this.WorkflowPage.RegistrationId);
        DataTable dtTotalBeds = Helper.HasRows(ds) ? ds.Tables[0] : null;
        this.DataList = dtTotalBeds;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
 	        LoadNumberOfBeds();
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override void LoadData(DataRow row)
    {
        bool isEdit = false;

        if (row == null)
            isEdit = false;
        else
            isEdit = true;

        bool isPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);

        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.Administrator))
        {
            this.txtTotalBeds.Enabled = !isPending;
        }
        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            txtTotalBeds.Enabled = true;
        }
        else
        {
            txtTotalBeds.Enabled = false;
        }
        //ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") && isEdit ? "block" : "none";
        hidIsEdit.Text = isEdit.ToString();

        if (isEdit)
        {
            txtTotalBeds.Text = row["TOTAL_NUM_BEDS"].ToString();
        }
        else
        {
            hidID.Text = string.Empty;
            txtTotalBeds.Text = "";
        }
    }

    protected void ValidateLC59(object sender, ServerValidateEventArgs e)
    {
        int numBeds;

        if (Int32.TryParse(txtTotalBeds.Text, out numBeds))
        {
             e.IsValid = true;
        }
        else
        {
            e.IsValid = false;
        }

     

    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override string ValidationGroup
    {
        get { return "TotalBeds"; }
    }

    public override string Title
    {
        get { return "Number of Beds"; }
    }

    public override string IdText
    {
        get { return "ucTotalNumberBeds_" + this.WorkflowPage.RegistrationId; }
    }

}