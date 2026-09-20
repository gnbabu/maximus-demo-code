using System;
using System.Web;
using System.Web.UI.WebControls;

public partial class PopupControls_LTProviderEnrollment : BasePopupControl
{

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

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void LoadControls()
    {
        //SelectRegistrationByRegID
        //DataSet ds = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        //if (Helper.HasRows(ds))
        //{
        //    DataTable dtProvider = ds.Tables[0];
        //    txtlblLTEnrollmentStatus.Text = Helper.GetString("EnrollmentStatusCodeDescription", dtProvider.Rows[0]);
        //}
    }

    public void SaveData()
    {
        this.Page.Validate("valLTCEnrollment");
        if (ValidateLTEnrollmentComplete())
        {
            DateTime myDate = DateTime.Now;
            if (!DateTime.TryParse(txtLTEffectiveDate.Text, out myDate)) { }
            svc.UpdateRSLFacilityHomeNumber(this.WorkflowPage.RegistrationId, myDate, Convert.ToInt32(txtLTHomeNumber.Text),
                txtLTComments.Text, Helper.GetUserId(HttpContext.Current.User.Identity.Name));
            svc.InsertUpdateODHFacilityHomeNumber(this.WorkflowPage.RegistrationId, myDate, Convert.ToInt32(txtLTHomeNumber.Text),
                Helper.GetUserId(HttpContext.Current.User.Identity.Name));
        }
    }

    private bool ValidateLTEnrollmentComplete()
    {
        bool isGood = true; 
        DateTime myDate;
        string strDate = txtLTEffectiveDate.Text;
        if (string.IsNullOrEmpty(txtLTHomeNumber.Text)) AddError("* Home Number is required.", ref isGood, "valLTCEnrollment");
        if (string.IsNullOrEmpty(txtLTComments.Text)) AddError("* Comment is required.", ref isGood, "valLTCEnrollment");
        if(ddlEnrollStatus.SelectedIndex == 0) AddError("* Enrollment Status is required.", ref isGood, "valLTCEnrollment");
        if (!string.IsNullOrWhiteSpace(strDate))
        {
            if (!DateTime.TryParse(txtLTEffectiveDate.Text, out myDate))
            {
                AddError("* Incorrect Date Value for Effective Date", ref isGood, "valLTCEnrollment");
            }
        }
        return isGood;
    }

    private void AddError(string errMsg, ref bool isGood, string valGroup = "valLTCEnrollment")
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = valGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }

}