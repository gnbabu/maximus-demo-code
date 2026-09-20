using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ReferToComplianceReasons : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.BindComplianceReasons();
        }
    }

    public void LoadControls()
    {       
        this.BindComplianceReasons();
    }

    private void BindComplianceReasons()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        // Page is 0 per Teresa return errors show on all pages based upon User's Role.
        DataSet ds = psc.SelectReferTocomplianceReasons();
        Helper.LoadList(this.rblComplReason, ds.Tables[0], "DESCRIPTION", "REFER_TO_COMPLIANCE_REASONS_ID", true);
        rblComplReason.DataBind();

    }
 
    public void SaveData()
    {
        if (rblComplReason.SelectedIndex == -1)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Refer to Compliance reason must be selected";
            val.ValidationGroup = "valReferToComplianceReasons";
            this.Page.Validators.Add(val);
        }
        else if (rblComplReason.SelectedValue == "10" && string.IsNullOrEmpty(txtInternalNotes.Text))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Please enter comments.";
            val.ValidationGroup = "valReferToComplianceReasons";
            this.Page.Validators.Add(val);
        }
        else
        {
            this.Page.Validate("valReferToComplianceReasons");
            if (Page.IsValid)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.WF_SaveProcessParameter(this.WorkflowPage.WF_ProcessID, CON.ProcessParameter.ReferToComplianceReasonID, rblComplReason.SelectedValue);

                if (txtInternalNotes.Text != null && txtInternalNotes.Text.Trim().Length > 0)
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("Reg_ID", this.WorkflowPage.RegistrationId.ToString());
                    parms.Add("Notes_Date", DateTime.Now.ToString()); //Note_Ref_ID
                    parms.Add("Initiated_By", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Person_Reviewed_By", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Enrollment_Type", "Refer To Compliance Reason");
                    parms.Add("Final_Disposition", "Other");
                    parms.Add("NOTES", txtInternalNotes.Text.ToString());
                    parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Last_Modified_Date_time", DateTime.Now.ToString());
                    parms.Add("CreatedBy", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("CreatedOn", DateTime.Now.ToString());
                    try
                    {
                        psc.InsertRegistrationDataTable("provider_feed", parms);
                    }
                    catch (Exception ex)
                    {

                    }
                    parms.Clear();
                }
            }
            else
            {
                return;
            }
        }

    }   
}