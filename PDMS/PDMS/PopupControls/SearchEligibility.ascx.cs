using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SearchEligibility : BasePopupControl
{
  
    protected void btnSave_Click(object sender, EventArgs e)
    {
        _spa = new PDMSService.PDMSServiceClient();
        this.ValidateData();
        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return;
        }
        try
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
            //parms.Add("PRIOR_AUTH_HOSPITAL_ID", hospitalId.ToString());
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
         


            return;
        }
        catch
        {
        }

    }
    public bool ValidateData(string msg)
    {
        //bool isGood = true;
        RequiredFieldValidator req = new RequiredFieldValidator();
        bool isValid = true;
        //if (String.IsNullOrEmpty(ddlServiceTypeCode.SelectedValue))
        //{
        //    AddValidationErrorMessage("*Service Type Code is required for detail N");
        //    isValid = false;
        //}
        //if (String.IsNullOrEmpty(txtServiceCode.Text))
        //{
        //    AddValidationErrorMessage("*Service code is required for detail N");
        //    isValid = false;

        //}
       
       return isValid;
    }
    #region spa
    private PDMSService.PDMSServiceClient _spa;
    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
    }
    #endregion
    public override void LoadData(DataRow dr)
    {
        base.LoadData(dr);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            try
            {
                
              
            }
            catch
            {
            }
        }

    }

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


    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;



    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        //isGood = false;
        return false;
    }
    private void ValidateData()
    {

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }
    protected void ReportBirthDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtBirthDate.Text, true) && Helper.IsValidDate(txtBirthDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtBirthDate.Text).Subtract(Convert.ToDateTime(txtBirthDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
   
    protected void ReportDOB_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtDOB.Text, true) && Helper.IsValidDate(txtDOB.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtDOB.Text).Subtract(Convert.ToDateTime(txtDOB.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }

    protected void grdBenefitsassplan_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void grdManagedCarePlan_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void grdPrivateInsur_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void grdLockin_SelectedIndexChanged(object sender, EventArgs e)
    {
       
       
    }

    protected void grdMedicare_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void grdAppealStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}



