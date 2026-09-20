using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OwnerIdentifyingInfo : BasePopupControl, IOwnerIdentifyingInfoView
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public bool OwnershipChangedFlag { get; set; }
    private OwnerIdentifyingInfoPresenter _presenter;

    public OwnerIdentifyingInfoPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new OwnerIdentifyingInfoPresenter(this);
            }

            return _presenter;
        }
    }

    public OwnerIdentifyingInfo Model { get; set; }
    public string MedicaidId
    {
        get
        {
            if (ViewState["MedicaidId"] == null) ViewState["MedicaidId"] = string.Empty;
            return ViewState["MedicaidId"].ToString();
        }
        set { ViewState["MedicaidId"] = value; }
    }


    
    public string NPI
    {
        get
        {
            if (ViewState["UserAccount_NPI"] == null) ViewState["UserAccount_NPI"] = string.Empty;
            return ViewState["UserAccount_NPI"].ToString();
        }
        set { ViewState["UserAccount_NPI"] = value; }
    }



    public bool IndividualProviderType
    {
        get
        {
            return ViewState["IndividualProviderType"] == null ? false : Convert.ToBoolean(ViewState["IndividualProviderType"].ToString());
        }
        set { ViewState["IndividualProviderType"] = value; }
    }


    public bool InProviderDataEntry
    {
        get
        {
            return ViewState["InProviderDataEntry"] == null ? false : Convert.ToBoolean(ViewState["InProviderDataEntry"]);
        }
        set
        {
            ViewState["InProviderDataEntry"] = value;
        }
    }

    private void LoadDropDowns()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetProviderTypes();
        


    }



    public override void LoadData(DataRow dr)
    {
        //Need to split this out into Get Data, Load Data, Set Editability and Set Visibility methods.
        string medId;
        txtDBA.Text = txtLegalBusinessName.Text = string.Empty;
        nbNPI.Text = nbTaxID.Text = nbProviderNumber.Text = string.Empty;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds1 = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
        if (Helper.HasRows(ds1))
         medId = Helper.GetString("MEDICAID_ID", ds1.Tables[0].Rows[0]);

        bool individual = Registration.IsIndividual(this.WorkflowPage.RegistrationId);
        trSSN.Visible = true;
        trTaxID.Visible = true;

        trTaxID.Visible = true;
        if (individual)//individual
        {
            trBusinessName.Visible = false;
            trDBA.Visible = false;
            trBirthDate.Visible = true;
        }
        else
        {
            trBusinessName.Visible = true;
            trDBA.Visible = true;
            trBirthDate.Visible = false;
        }

        DataSet ds = presenter.GetOwnerCategoryType();
        Helper.LoadList(rblEntityType, ds.Tables[0], "Owner_Category_Type_Name", "Owner_Category_Type_ID", false);
        //general enable or disable all fields (false = enable, true = disable)
        Helper.SetReadOnly(this, false);

        if (dr != null)
        {
            hdnRegOwnerIdentifyingInfoID.Value = Helper.GetString("REG_OWNER_PAPER_PROVIDER_ID", dr);
            //ddlOwner.SelectedValue = Helper.GetString("REG_OWNER_ID", dr);
            txtLegalBusinessName.Text = Helper.GetString("ENTITY_NAME", dr);

            if (!string.IsNullOrEmpty(Helper.GetString("BIRTH_DATE", dr)))
                txtBirthDate.Text = Helper.GetDate("BIRTH_DATE", dr);
            nbTaxID.Text = Helper.GetString("TAX_ID", dr);
            txtSSN.Text = Helper.GetString("SSN", dr);
            txtDBA.Text = Helper.GetString("DBA_NAME", dr);

            nbProviderNumber.Text = Helper.GetString("MEDICAID_ID", dr);
            nbNPI.Text =  Helper.GetString("NPI", dr);
            if(Helper.GetString("OWNER_CATEGORY_TYPE_ID", dr) != string.Empty)            
            rblEntityType.SelectedValue = Helper.GetString("OWNER_CATEGORY_TYPE_ID", dr);
            
            
            
            

        }
        //load radio button



        Helper.SetReadOnly(nbTaxID, true);

        Helper.SetReadOnly(nbNPI, true);
        Helper.SetReadOnly(txtDBA, true);
        Helper.SetReadOnly(nbProviderNumber, true);
        Helper.SetReadOnly(txtSSN, true);
        Helper.SetReadOnly(txtBirthDate, true);



    }

    private bool IsExternalUser()
    {
        return true;
    }

    private bool IsInternalUser()
    {
        return true;
    }



    // Todo This is only temporary, will be moved to a common file
    private bool IsIndividualProvider(DataRow dr)
    {
        int entityTypeID = Helper.GetInt("ENTITY_TYPE_ID", dr);
        return entityTypeID == CON.ProviderCategoryTypeID.Individual || entityTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile;
    }

    private bool IsNewReg(int partyID)
    {
        return string.IsNullOrEmpty(MedicaidId) && partyID == 0;
    }

    public bool SaveData()
    {
        // OHPNM-14088
        if (this.WorkflowPage.OwnerInfoPageIsReadOnly)
        {
            // this is a read only page; no need to validate or save
            return true;
        }

        if (!ValidateData())
        {
            return false;
        }

        if (!Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            return true;
        }

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        bool doUpdate = Helper.HasRows(ds);
        OwnerIdentifyingInfo obj = new OwnerIdentifyingInfo();
        if (doUpdate)
        {
            obj.EntityName = txtLegalBusinessName.Text;
            obj.DBAName = txtDBA.Text;
            if(trBirthDate.Visible == true)
            obj.BirthDate = Convert.ToDateTime(txtBirthDate.Text);
            obj.MedicaidID = nbProviderNumber.Text;
            obj.NPI = nbNPI.Text;
            obj.OWNER_CATEGORY_TYPE_ID = Convert.ToInt32(rblEntityType.SelectedValue);
            obj.RegID = this.WorkflowPage.RegistrationId;
            obj.SSN = txtSSN.Text;
            obj.TaxID = nbTaxID.Text;
            obj.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            if (hdnRegOwnerIdentifyingInfoID.Value !=  "")
                obj.REG_OWNER_PAPER_PROVIDER_ID = Convert.ToInt32(hdnRegOwnerIdentifyingInfoID.Value); 


            // Only for individual provider
            if (IsIndividualProvider(ds.Tables[0].Rows[0]))
            {




            }
            Model = obj;
            presenter.SaveRegOwnerIdentifyingInfo(Model);
        }
        return true;
    }
    
    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valOwnerIdentifyingInfo";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public bool ValidateData()
    {
        // OHPNM-14088
        if (this.WorkflowPage.OwnerInfoPageIsReadOnly)
        {
            // this is a read only page; no need to validate or save
            return true;
        }

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        bool isGood = true;
        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))   // Must be a Provider
        {
        }

        if (rblEntityType.SelectedValue == "")
        {
            AddError("* Please select appropriate category", ref isGood);
        }
        //if (trBirthDate.Visible == true)
        //{ 
        //    if(txtBirthDate.Text=="")
        //        AddError("* Please enter Birth date", ref isGood);
        //}
        //if (trTaxID.Visible == true)
        //{
        //    if (nbTaxID.Text == "")
        //        AddError("* Please enter Tax ID", ref isGood);
        //}
        //if (trBusinessName.Visible == true)
        //{
        //    if (txtLegalBusinessName.Text == "")
        //        AddError("* Please enter Entity Name", ref isGood);
        //}
        //if (nbNPI.Text == "")
        //    AddError("* Please enter NPI", ref isGood);

        return isGood;
    }


    public void GetOwnerIdentifyingInfo(DataSet ds)
    {

    }
}