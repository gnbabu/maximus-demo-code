using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
public partial class PopupControls_DentalLicense : BaseSectionControl
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
            DataSet ds = svc.SelectReg_Dental_Licenses(this.WorkflowPage.RegistrationId);
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }
    
    public int DentalLicenseID
    {
        get { return ViewState["DentalLicenseID"] != null ? (int)ViewState["DentalLicenseID"] : 0; }
        set { ViewState["DentalLicenseID"] = value; }
    }

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;
    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (!string.IsNullOrEmpty(cklLicenseStatus.SelectedValue) || rblSpecialist.SelectedValue == "True")
            isRequired = true;
        return isRequired;
    }
    public bool CanUserViewDelete()
    {
        return Registration.CanUserViewDelete(this.WorkflowPage.RegistrationId,this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
    }
    public override bool SaveData()
    {
        Page.Validate("valDentalLicense");
        ValidateData();
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valDentalLicense") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        Dictionary<string, string> parms2 = new Dictionary<string, string>();
        Int32 DentalLicenseID;
        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
     
        parms.Add("LICENSE_DETAILS", txtOther.Text);
        parms.Add("SPECIALIST", rblSpecialist.SelectedValue);
        parms.Add("SPECIALIST_DETAILS", txtSpecialistYN.Text);
        parms.Add("ANESTHESIA_PERMIT", rblanesthesia.SelectedValue);
        parms.Add("SEDATION_PERMIT", rblsedation.SelectedValue);
        parms.Add("NITROUSOXIDE_PERMIT", rblNO.SelectedValue);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        if (isEdit)
        {
            parms.Add("REG_DENTAL_LICENSE_ID", this.DentalLicenseID.ToString());
        
            psc.UpdateRegistrationDataTable("Dental_Licenses", parms);
            //delete all checkbox data and enter new one
            psc.DeleteRegistrationData("DENTAL_LICENSETYPE_LICENSEID", "REG_DENTAL_LICENSE_ID", this.DentalLicenseID);
                                                                       
            // Add new checkbox data to DENTAL_LICENSE_LICENSETYPE table
            psc = new PDMSService.PDMSServiceClient();
            parms2 = new Dictionary<string, string>();
            //int i = 0;
            foreach (ListItem item in cklLicenseStatus.Items)
            {
                if (item.Selected)
                {
                    parms2 = new Dictionary<string, string>();
                    parms2.Add("DENTAL_LICENSE_TYPEID", item.Value);
                    parms2.Add("REG_DENTAL_LICENSE_ID",  this.DentalLicenseID.ToString());
                    psc.InsertDentalRegistrationData("DENTAL_LICENSETYPE_LICENSEID", parms2);

                }
            }
        }
        else
        {
            DentalLicenseID = psc.InsertRegistrationDataTable("Dental_Licenses", parms);

            // Use the newly created ID to update DENTAL_LICENSE_LICENSETYPE table
            psc = new PDMSService.PDMSServiceClient();
            parms2 = new Dictionary<string, string>();
            //int i = 0;
            foreach (ListItem item in cklLicenseStatus.Items)
            {
                if (item.Selected)
                {
                    parms2 = new Dictionary<string, string>();
                    parms2.Add("DENTAL_LICENSE_TYPEID", item.Value);
                    parms2.Add("REG_DENTAL_LICENSE_ID", DentalLicenseID.ToString());
                    psc.InsertDentalRegistrationData("DENTAL_LICENSETYPE_LICENSEID", parms2);

                }
            }
        }

        return true;
    }
    public override  bool ValidateData()
    {
        if(string.IsNullOrEmpty(cklLicenseStatus.SelectedValue))
        {
            cvlLicenseStatus.IsValid = false;
        }
        if(cklLicenseStatus.SelectedValue == "5" && string.IsNullOrEmpty(txtOther.Text))
        {
            cvtxtOther.IsValid = false;
        }
        if(rblSpecialist.SelectedValue == "True" && string.IsNullOrEmpty(txtSpecialistYN.Text))
        {
            cvtxtSpecialistYN.IsValid = false;
        }

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
        DataSet dsDental = svc.SelectReg_Dental_Licenses(Convert.ToInt32(this.WorkflowPage.RegistrationId));

        setDefaultControlValues();

        if (dsDental.Tables[0].Rows.Count >= 1)
        {
            DataRow datarow = dsDental.Tables[0].Rows[0];
            if (Convert.ToBoolean(datarow["SPECIALIST"]))
                rblSpecialist.SelectedValue = "True";

            if (Convert.ToBoolean(datarow["ANESTHESIA_PERMIT"]))
                rblanesthesia.SelectedValue = "True";
            if (Convert.ToBoolean(datarow["NITROUSOXIDE_PERMIT"]))
                rblNO.SelectedValue = "True";
            if (Convert.ToBoolean(datarow["SEDATION_PERMIT"]))
                rblsedation.SelectedValue = "True";

            txtOther.Text = datarow["LICENSE_DETAILS"].ToString();
            txtSpecialistYN.Text = datarow["SPECIALIST_DETAILS"].ToString();
            this.DentalLicenseID  = Convert.ToInt32(datarow["REG_DENTAL_LICENSE_ID"]) ;
            
            //Check saved LicenseType from DB
            dsDental = new DataSet();
            dsDental = svc.SelectReg_Dental_LicenseType(Convert.ToInt32(datarow["REG_DENTAL_LICENSE_ID"]));
            if (dsDental.Tables[0].Rows.Count >= 1)
            {
                for (int i = 0; i <= dsDental.Tables[0].Rows.Count - 1; i++)
                {
                    string itemCheck = dsDental.Tables[0].Rows[i]["DENTAL_LICENSE_TYPEID"].ToString();
                    cklLicenseStatus.Items.FindByValue(itemCheck).Selected = true;


                }
            }

        }
        hidIsEdit.Text = isEdit.ToString();

    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadDentalLicenses();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            setDefaultControlValues();
        }

    }

    private void setDefaultControlValues()
    {
        rblSpecialist.SelectedValue = "False";
        rblanesthesia.SelectedValue = "False";
        rblsedation.SelectedValue = "False";
        rblNO.SelectedValue = "False";
    }

    protected void cvTxtOther_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (cklLicenseStatus.SelectedValue == "5")
        {
            //rfvOther.Enabled = true;
            args.IsValid = true;
        }
        else
        {
            args.IsValid = false;
        }
    }

    protected void cvLicenseStatus_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (cklLicenseStatus.Items.Count <= 0)
        {
            args.IsValid = false;
        }
    }


    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        dentalDetail.Visible = true;
        DataRow dr;
        dr = this.DataList.Rows[index];       

        //TODO: Need to implement Edit commmand functionality, which is missing here
        if (e.CommandName == "DeleteDentalLicensesCodeRow")
        {
           bool IsDeleted = DeleteDentalLicense(dr);
            if (IsDeleted) LoadDentalLicenses();
        }
        else
        {
            if (Helper.HasRows(this.DataList))
                this.LoadData(dr);
            else
                this.LoadData(null);
        }
    }

    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        dentalDetail.Visible = true;
        this.LoadData(null);
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        // TODO: EDV Here is where we should show/hide history
    }


    private void LoadDentalLicenses()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataTable dtDentalLicense = new DataTable();
        dtDentalLicense.Columns.Add("LICENSE_TYPE_DETAILS", typeof(String));
        dtDentalLicense.Columns.Add("SPECIALIST", typeof(String));
        dtDentalLicense.Columns.Add("ANESTHESIA_PERMIT", typeof(String));
        dtDentalLicense.Columns.Add("SEDATION_PERMIT", typeof(String));

        DataSet ds = psc.SelectReg_Dental_Licenses(this.WorkflowPage.RegistrationId);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataSet ds1 = psc.SelectReg_Dental_LicenseType(Convert.ToInt32(ds.Tables[0].Rows[0]["REG_DENTAL_LICENSE_ID"].ToString()));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                //concatenate the LicenseType
                String licesneType = "";
                foreach (DataRow row in ds1.Tables[0].Rows)
                {
                    licesneType += row["LICENSE_TYPE_DETAILS"].ToString() + "<br/>";
                }
                String anesthesia = (bool)ds.Tables[0].Rows[0]["ANESTHESIA_PERMIT"] ? "Yes" : "No";
                String sedation = (bool)ds.Tables[0].Rows[0]["SEDATION_PERMIT"] ? "Yes" : "No";
                String specialist = (bool)ds.Tables[0].Rows[0]["SPECIALIST"] ? "Yes" : "No";

                dtDentalLicense.Rows.Add(licesneType, specialist, anesthesia, sedation);
            }
        }

        dtDentalLicense = (Helper.HasRows(dtDentalLicense)) ? dtDentalLicense : null;
        grdDentalLicense.DataSource = this.DataList = dtDentalLicense;
        grdDentalLicense.DataBind();

        if (Helper.IsUserInDBHOperatorRole(HttpContext.Current.User.Identity.Name))
        {
            btnAddDentalLicenses.Visible = false;
        }
        else
        {
            btnAddDentalLicenses.Visible = (grdDentalLicense.Rows.Count == 0);
        }
    }
    private bool DeleteDentalLicense(DataRow dr)
    {
        bool isValid = true;
        if (this.DataList.Rows.Count == 1 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId, CON.RegistrationPageName.Certification, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.DentalLicense)))
        //if only one record and required page then prevent deletion or else the page will not turn back to blue once it is green and will cause issues for page submission
        {
            AddError("* This is required section. This dental License cannot be deleted.", ref isValid,ValidationGroup);
            return isValid;
        }
        if (dr != null)
        {
            int REG_DENTAL_LICENSE_ID = Helper.GetInt("REG_DENTAL_LICENSE_ID", dr);
            int DENTAL_LICENSE_ID = Helper.GetInt("DENTAL_LICENSE_ID", dr);
            if (DENTAL_LICENSE_ID == 0)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.DeleteRegistrationData("DENTAL_LICENSE", "REG_DENTAL_LICENSE_ID", REG_DENTAL_LICENSE_ID);
            }
            else
            {
                AddError("* Dental License cannot be deleted.",ref isValid,ValidationGroup);
                return isValid;
            }
        }
        return isValid;
    }
    public override string ValidationGroup
    {
        get { return "valDentalLicense"; }
    }

    public override string Title
    {
        get { return "Dental License"; }
    }

    public override string IdText
    {
        get { return "ucDentalLicense_" + this.WorkflowPage.RegistrationId; }
    }

    private void AddError(string errMsg, ref bool isGood,string ValidationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }
}