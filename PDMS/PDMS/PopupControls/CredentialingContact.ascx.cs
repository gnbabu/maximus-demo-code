using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_CredentialingContact : BaseSectionControl
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

    private bool _ExportHistory
    {
        get
        {
            return Convert.ToBoolean(ViewState["ExportHistory"]);
        }
        set
        {
            ViewState["ExportHistory"] = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        //OHPNM-3487 - on click of View provider file Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                btnAddCredentialingContactItem.Visible = false;
            }
        }

    }


    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Credentialing Contact";
    }
    public override void LoadControlData()
    {
        LoadCredentialingContactDetails();
    }

    private void LoadCredentialingContactDetails()
    {
        if (!pnlCredentialingContactGrid.Visible) return;

        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_CREDENTIALING_CONTACT", parms);
            if (Helper.HasRows(ds)) grdCredentialingContact.DataSource = this.DataList = ds.Tables[0];
            else grdCredentialingContact.DataSource = this.DataList = null;
            grdCredentialingContact.DataBind();
        }

        //btnCredentialingContactHistory.Visible = (grdCredentialingContact.Rows.Count > 0);
    }

    public override void LoadData(System.Data.DataRow dr = null)
    {
        hdnRegCredentialingContactId.Value = string.Empty;
        if (dr != null)
        {
            pnlCredentialingContactEntry.Visible = true;
            hdnRegCredentialingContactId.Value = Helper.GetData("CONTACT_ID", dr);


            if (!string.IsNullOrEmpty(Helper.GetData("CONTACT_NAME", dr)))
            {
                txtContactName.Text = Helper.GetString("CONTACT_NAME", dr);
            }
            if (!string.IsNullOrEmpty(Helper.GetData("PRACTICE_NAME", dr)))
            {
                txtPracticeName.Text = Helper.GetString("PRACTICE_NAME", dr);
            }
            if (!string.IsNullOrEmpty(Helper.GetData("EMAIL_ID", dr)))
            {
                txtContactEmail.Text = Helper.GetString("EMAIL_ID", dr);
            }
            if (!string.IsNullOrEmpty(Helper.GetData("CONTACT_NUMBER", dr)))
            {
                txtPhoneNumber.Text = Helper.FormatPhone(Helper.GetString("CONTACT_NUMBER", dr));
            }
            if (!string.IsNullOrEmpty(Helper.GetData("CONTACT_EXT", dr)))
            {
                txtExtension.Text = Helper.GetString("CONTACT_EXT", dr);
            }
            if (!string.IsNullOrEmpty(Helper.GetData("CONTACT_FAX", dr)))
            {
                txtFaxNo.Text = Helper.FormatPhone(Helper.GetString("CONTACT_FAX", dr));
            }
            if (!string.IsNullOrEmpty(Helper.GetData("COMMENTS", dr)))
            {
                txtComments.Text = Helper.GetString("COMMENTS", dr);
            }
        }
    }

    public override bool SaveData()
    {
        if (pnlCredentialingContactEntry.Visible == false)
        return true;
        Page.Validate("vgCredentialContact");
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("vgCredentialContact") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }
        if (ValidateData())
        {
            if (!string.IsNullOrEmpty(txtContactName.Text))
            {
                //Insert New record
                try
                {
                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    parms.Add("Practice_Name", txtPracticeName.Text);
                    parms.Add("Contact_Name", txtContactName.Text);
                    parms.Add("EMAIL_ID", txtContactEmail.Text);
                    parms.Add("CONTACT_NUMBER", Helper.StripNonNumerics(txtPhoneNumber.Text));
                    parms.Add("Contact_Ext", txtExtension.Text);
                    parms.Add("Contact_Fax", Helper.StripNonNumerics(txtFaxNo.Text));
                    parms.Add("Comments", txtComments.Text);


                    //parms.Add("contact_phone", Helper.StripNonNumerics(tbPhone.Text.Trim()));
                    // parms.Add("contact_phone_ext", string.empty);
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                    if (!string.IsNullOrEmpty(hdnRegCredentialingContactId.Value))
                    {
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                        parms.Add("CONTACT_ID", hdnRegCredentialingContactId.Value.Trim());
                        parms.Add("Created_On_Date_Time", null);
                        parms.Add("Created_By_User",null);
                        psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "Credentialing_Contact", parms);
                    }
                    else
                    {
                        parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                        parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
                        psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "Credentialing_Contact", parms);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("ucCredentialingContact_SaveData - " + ex.Message));
                }
            }
        }

        return this.DataList !=null && this.DataList.Rows != null && this.DataList.Rows.Count > 0?true:false;

    }

    public override bool ValidateData()
    {
        return true;
    }

    public override string Title
    {
        get { return "Credentialing Contact"; }
    }

    public override string IdText
    {
        get { return "ucCredentialingContact_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "vgCredentialContact"; }
    }

    private void DeleteCredentailingContact(DataRow dr)
    {

        if (dr != null)
        {
            int CONTACT_ID = Helper.GetInt("CONTACT_ID", dr);

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            psc.DeleteRegistrationData("CREDENTIALING_CONTACT", "CONTACT_ID", CONTACT_ID);

        }
    }
    protected void grdCredentialingContact_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        DataRow dr;
        dr = this.DataList.Rows[index];

        if (e.CommandName == "DeleteRow")
        {
            DeleteCredentailingContact(dr);
            LoadControlData();
        }
        else if(e.CommandName == "EditCredentialingContact")
        {
            pnlCredentialingContactGrid.Visible = true;
            if (Helper.HasRows(this.DataList))
                this.LoadData(this.DataList.Rows[index]);
            else
                this.LoadData(null);
        }
    }
    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (!string.IsNullOrEmpty(this.txtContactName.Text) || 
            !string.IsNullOrEmpty(this.txtPracticeName.Text) ||
            !string.IsNullOrEmpty(this.txtPhoneNumber.Text) ||
            !string.IsNullOrEmpty(this.txtContactEmail.Text))
            isRequired = true;
        else if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 0)
            isRequired = true;
            
        return isRequired;
    }
    protected void btnAddCredentialingContactItem_Click(object sender, CommandEventArgs e)
    {
        pnlCredentialingContactEntry.Visible = true;
        this.LoadData(null);
    }
    protected string FormatPhone(object phn)
    {
        string phone = phn is string ? phn.ToString() : string.Empty;
        return Helper.FormatPhone(phone);
    }
    protected void cvCredentialContact_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (!string.IsNullOrEmpty(txtExtension.Text.Trim()) && txtExtension.Text.Length > 5)
        {
            cvCredentialContact.ErrorMessage = "* Phone Extension cannot be more than 5 digits";
            args.IsValid = false;
            return;
        }
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        lblTitle.Text = "Credentialing Contact History";
        ucCredentialingContactHistory.LoadControlData();
        mpe.Show();
    }


    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_CREDENTIALING_CONTACT_History", parms);
        if (Helper.HasRows(ds))
        {
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
            grd.MasterTableView.ExportToExcel();
        }
    }
}