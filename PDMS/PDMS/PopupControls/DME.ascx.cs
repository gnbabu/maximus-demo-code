using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using CustomControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using MAXIMUS.Core.Libraries;
using Telerik.Web.UI;
using System.Linq;

public partial class Pages_DME : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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

    #region " Properties "
    public int DmeId
    {
        get
        {
            if (ViewState["DmeId"] == null) ViewState["DmeId"] = 0;
            return Convert.ToInt32(ViewState["DmeId"]);
        }
        set { ViewState["DmeId"] = value; }
    }

    public int RegDmePersonnelInfoId
    {
        get
        {
            if (ViewState["RegDmePersonnelInfoId"] == null) ViewState["RegDmePersonnelInfoId"] = 0;
            return Convert.ToInt32(ViewState["RegDmePersonnelInfoId"]);
        }
        set { ViewState["RegDmePersonnelInfoId"] = value; }
    }
    #endregion

    #region " Page Events "
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucDMEProductsAndServices.ValidationEvent += new PopupControls_DMEProductsAndServices.ValidationEventHandler(KeepPopupOpen);
        this.ucDMEProductsAndServices.KeepOpenEvent += new PopupControls_DMEProductsAndServices.KeepOpenEventHandler(KeepPopupOpen);

        this.ucDMERegAgent.ValidationEvent += new PopupControls_DMERegisteredAgent.ValidationEventHandler(KeepPopupOpen);
        this.ucDMERegAgent.KeepOpenEvent += new PopupControls_DMERegisteredAgent.KeepOpenEventHandler(KeepPopupOpen);

        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
                ImageButton1.Visible = false;
            }
        }
    }

    protected void cblAgencies_DataBound(object sender, EventArgs e)
    {
        // Loop thru the checkbox list
        // Find the related row in the DataTable
        // See if there's a REG_ID associated
        // If there is, set the checkbox to selected
        foreach (ListItem li in cblAgencies.Items)
        {
            DataTable dtAgencies = (DataTable)cblAgencies.DataSource;
            if (dtAgencies != null)
            {
                DataRow rowAgency = dtAgencies.Select(string.Format("ACCREDITATION_AGENCY_ID = {0}", li.Value)).FirstOrDefault();
                li.Selected = Helper.GetInt("REG_ID", rowAgency) != 0;
                if (li.Text == "Other, please specify below (Other)" && li.Selected)
                {
                    txtOtherReasons.Enabled = true;
                    txtOtherReasons.Text = Helper.GetString("OTHER_REASONS", rowAgency);
                }
                else if (li.Text == "Other, please specify below (Other)" && !li.Selected)
                    txtOtherReasons.Enabled = false;
            }
        }
    }
    #endregion
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {        
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        LoadDMEInfo();
        LoadDMERegisteredAgent();
        LoadDMEPersonnelInfo();
        LoadDMEBackgroundChecks();
        LoadDMEProductsServices();
    }
    private void KeepPopupOpen()
    {
        this.mpe.Show();
    }
    private void LoadDMEInfo()
    {
        cblAgencies.Items.Clear();

        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_ACCREDITATION_AGENCY");
        DataTable tblAgencies = null;
        tblAgencies = ds.Tables[0];

        // Load the Agency checkbox list
        // Selections are handled during databinding event
        cblAgencies.DataTextField = "ACCREDITATION_AGENCY_DESC";
        cblAgencies.DataValueField = "ACCREDITATION_AGENCY_ID";
        cblAgencies.DataSource = tblAgencies;
        cblAgencies.DataBind();

    }

    private void CreateNewBlankDME()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "DME", parms);
    }

    public override bool SaveData()
    {
        ValidateData();
        Page.Validate("valProviderInfoHeader");
        if (Page.IsValid)
        {
            if (Registration.PreviewingRegistrationSection())
            {
                return false;
            }

            // Save the DME fields

            // Save agency selections/deselections
            // Get the saved selections
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_ACCREDITATION_AGENCY");
            DataTable tblAgencies = null;
            tblAgencies = ds.Tables[0];

            // Review each checkbox
            foreach (ListItem li in cblAgencies.Items)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();

                // Find row with selected agency
                DataRow rowAgency = tblAgencies.Select(string.Format("ACCREDITATION_AGENCY_ID = {0}", li.Value)).FirstOrDefault();

                // Insert any new selections
                if (li.Selected)
                {

                    // Only insert if row not found
                    if (Helper.GetInt("REG_ID", rowAgency) == 0)
                    {
                        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                        parms.Add("REG_DME_ID", DmeId.ToString());
                        parms.Add("ACCREDITATION_AGENCY_ID", li.Value);
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        if (!string.IsNullOrEmpty(txtOtherReasons.Text))
                        {
                            parms.Add("OTHER_REASONS", txtOtherReasons.Text);
                        }
                        svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "DME_ACCREDITATION_AGENCY", parms);
                    }
                }
                else // Delete where any de-selections occurred
                {
                    if (Helper.GetInt("REG_ID", rowAgency) != 0)
                    {
                        int regDMEAccreditationAgencyID = Helper.GetInt("REG_DME_ACCREDITATION_AGENCY_ID", rowAgency);
                        svc.DeleteRegistrationData("DME_ACCREDITATION_AGENCY", "REG_DME_ACCREDITATION_AGENCY_ID", regDMEAccreditationAgencyID);
                    }
                }
            }
            //save DME Personnel info
            Dictionary<string, string> parms1 = new Dictionary<string, string>();
            parms1.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            if (RegDmePersonnelInfoId > 0)
                parms1.Add("REG_DME_PERSONNEL_INFO_ID", RegDmePersonnelInfoId.ToString());
            if (rblYesNo.SelectedIndex >= 0)
                parms1.Add("IsBeneficiaryVisited", rblYesNo.SelectedValue.ToString());
            if (rblPhysicallyContacted.SelectedIndex >= 0)
                parms1.Add("IsBeneficiaryPhysicallyContacted", rblPhysicallyContacted.SelectedValue.ToString());
            if (!string.IsNullOrEmpty(txtProfessionalLicenses.Text))
                parms1.Add("NoofLicenseCopies", txtProfessionalLicenses.Text);
            if (!string.IsNullOrEmpty(txtCriminalBackgroundChecks.Text))
                parms1.Add("NoofBackgroundChecks", txtCriminalBackgroundChecks.Text);
            parms1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms1.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            if (RegDmePersonnelInfoId > 0)
                svc.UpdateRegistrationDataTable("DME_PERSONNEL_INFO", parms1);
            else
                svc.InsertRegistrationDataTable("DME_PERSONNEL_INFO", parms1);
        }
       
        else
            return false;
        return true;
    }

    private void LoadDMEPersonnelInfo()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_PERSONNEL_INFO");
        DataRow dr;
        if (Helper.HasRows(ds))
        {
            dr = ds.Tables[0].Rows[0];
            RegDmePersonnelInfoId = Helper.GetInt("REG_DME_PERSONNEL_INFO_ID", dr);
            if (Helper.GetBool("IsBeneficiaryVisited", dr) == true)
                rblYesNo.SelectedIndex = 0;
            else if (Helper.GetBool("IsBeneficiaryVisited", dr) == false)
                rblYesNo.SelectedIndex = 1;
            if (Helper.GetBool("IsBeneficiaryPhysicallyContacted", dr) == true)
                rblPhysicallyContacted.SelectedIndex = 0;
            else if (Helper.GetBool("IsBeneficiaryPhysicallyContacted", dr) == false)
                rblPhysicallyContacted.SelectedIndex = 1;
            txtProfessionalLicenses.Text = Helper.GetString("NoofLicenseCopies", dr);
            txtCriminalBackgroundChecks.Text = Helper.GetString("NoofBackgroundChecks", dr);
        }
    }
    private void LoadDMEBackgroundChecks()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_BACKGROUND_CHK_PROFESSIONAL_INFO");
        //gvDMEBackgroundChcks.DataSource = ds;
        //gvDMEBackgroundChcks.DataBind();
        if (Helper.HasRows(ds))
        {
            DataRow[] dr = ds.Tables[0].Select("IsBackgroundCheck = 1");
            DataRow[] dr1 = ds.Tables[0].Select("IsProfessionalLicense = 1");
            if(dr1.Length > 0)
            txtProfessionalLicenses.Text = dr1.Length.ToString();
            txtProfessionalLicenses.Enabled = false;
            if (dr.Length > 0)
            txtCriminalBackgroundChecks.Text = dr.Length.ToString();
            txtCriminalBackgroundChecks.Enabled = false;
        }
    }
    private void LoadDMEProductsServices()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_PRODUCT_CATEGORY_INFO");
        DataRow dr;
        gvProductsAndServices.DataSource = ds;
        gvProductsAndServices.DataBind();
        if (Helper.HasRows(ds))
        {

        }
    }


    private void LoadDMERegisteredAgent()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_REGISTERED_AGENT");
        gvRegAgent.DataSource = ds;
        gvRegAgent.DataBind();
        btnAddRegAgent.Visible = (this.WorkflowPage.RegistrationIdSelected == 0 ? (gvRegAgent.Rows.Count == 0) : false);
        btnAddRegAgentHistory.Visible = (gvRegAgent.Rows.Count > 0); 
    }

    public override bool ValidateData()
    {
        bool isGood = true;
        
        if ((rblYesNo.SelectedValue == "1" || rblPhysicallyContacted.SelectedValue == "1"))
        {
            if (string.IsNullOrEmpty(txtProfessionalLicenses.Text)|| string.IsNullOrEmpty(txtCriminalBackgroundChecks.Text))
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "You indicated some of your personnel have contact with beneficiaries. A copy of the license or background check must be uploaded.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);
                isGood = false;
            }
           
        }
        if (gvProductsAndServices.Rows.Count == 0)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Please enter the Products and Services to be furnished by Provider.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        int cntItemsSelected = 0;
        foreach (ListItem i in cblAgencies.Items)
        {
            if (i.Selected)
                cntItemsSelected++;
        }
        if (cntItemsSelected == 0)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "At least one agency must be selected.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        if (!CheckUploadedDocuments())
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Upload at least one document called Licenses or Certifications for all Products and Services.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        return isGood;
    }
    protected void btnAddDMEBackgroundChcks_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "DMEBackgroundChcks")
        {
            lblTitle.Text = "Verification Method";
            ucDMEBackgoundCheckProfessionalLicenses.InitializeFields();
            mltPopup.ActiveViewIndex = 0;
            mpe.Show();
        }
        else if (e.CommandName == "DMEProductsServices")
        {
            lblTitle.Text = "Products and Services";
            btnSave.ValidationGroup = "valDMEProductsAndServices";
            ucDMEProductsAndServices.InitializeFields();
            mltPopup.ActiveViewIndex = 1;

            mpe.Show();

        }

        if (e.CommandName == "DMERegAgent")
        {
            lblTitle.Text = "Registered Agent";
            btnSave.ValidationGroup = "valDMERegisteredAgent";
            ucDMERegAgent.InitializeFields();
            mltPopup.ActiveViewIndex = 2;
            mpe.Show();
        }
    }
    //protected void gvDMEBackgroundChcks_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
    //    DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_BACKGROUND_CHK_PROFESSIONAL_INFO");
    //    int index = Convert.ToInt32(e.CommandArgument);
    //    int REG_DME_BACKGROUND_CHK_PROFESSIONAL_INFO_ID = Convert.ToInt32(this.gvDMEBackgroundChcks.DataKeys[index].Values["REG_DME_BACKGROUND_CHK_PROFESSIONAL_INFO_ID"].ToString());
    //    Dictionary<string, string> parms = new Dictionary<string, string>();
    //    parms.Add("DMEBackgroundChkProfessionalID", REG_DME_BACKGROUND_CHK_PROFESSIONAL_INFO_ID.ToString());
    //    DataSet ds1 = svc.SelectRegistrationDataWithParams("usp_SelectREG_DME_BACKGROUND_CHK_PROFESSIONAL_INFO_ByID", parms);
    //    ucDMEBackgoundCheckProfessionalLicenses.LoadData(ds1.Tables[0].Rows[0]);
    //    mltPopup.ActiveViewIndex = 0;
    //    lblTitle.Text = "Verification Method";
    //    mpe.Show();
    //}



    protected void gvRegAgent_RowCommand(object sender, GridViewCommandEventArgs e)
    {             
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

        int index = Convert.ToInt32(e.CommandArgument);
        int REG_DME_REGISTERED_AGENT_ID = Convert.ToInt32(this.gvRegAgent.DataKeys[index].Values["REG_DME_REGISTERED_AGENT_ID"].ToString());
        parms.Add("REG_DME_REGISTERED_AGENT_ID", REG_DME_REGISTERED_AGENT_ID.ToString());
        DataSet ds1 = svc.SelectRegistrationDataWithParams("usp_SelectREG_DME_REGISTERED_AGENT", parms);
        DataRow dr = null;
        DataTable dt = null;
        if (Helper.HasRows(ds1)) dt = ds1.Tables[0];
        if (Helper.HasRows(dt)) dr = dt.Rows[0];
        ucDMERegAgent.LoadData(dr);
        mltPopup.ActiveViewIndex = 2;

        lblTitle.Text = "Registered Agent";
        mpe.Show();
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (mltPopup.ActiveViewIndex == 0)
        {
            ucDMEBackgoundCheckProfessionalLicenses.SaveData();
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_BACKGROUND_CHK_PROFESSIONAL_INFO");
            //gvDMEBackgroundChcks.DataSource = ds;
            //gvDMEBackgroundChcks.DataBind();
            DataRow[] dr = ds.Tables[0].Select("IsBackgroundCheck = 1");
            DataRow[] dr1 = ds.Tables[0].Select("IsProfessionalLicense = 1");
            txtProfessionalLicenses.Text = dr1.Length.ToString();
            txtProfessionalLicenses.Enabled = false;
            txtCriminalBackgroundChecks.Text = dr.Length.ToString();
            txtCriminalBackgroundChecks.Enabled = false;
        }
        else if (mltPopup.ActiveViewIndex == 1)
        {
                lblTitle.Text = "Products and Services";
                btnSave.ValidationGroup = "valDMEProductsAndServices";
                if (ucDMEProductsAndServices.SaveData())
                {
                    PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
                    DataSet ds1 = psc1.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_PRODUCT_CATEGORY_INFO");

                    gvProductsAndServices.DataSource = ds1;  
                    gvProductsAndServices.DataBind();
                    mpe.Hide();
                }
                else
                    mpe.Show();
        }
        else if (mltPopup.ActiveViewIndex == 2)
        {
            lblTitle.Text = "Registered Agent";
            btnSave.ValidationGroup = "valDMERegisteredAgent";

            if (ucDMERegAgent.SaveData())
            {
                LoadDMERegisteredAgent();


                mpe.Hide();
            }
            else
                mpe.Show();
        }


    }
    protected void gvProductsAndServices_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        mltPopup.ActiveViewIndex = 1;
        lblTitle.Text = "Products and Services";
        btnSave.ValidationGroup = "valDMEProductsAndServices";
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        int index = Convert.ToInt32(e.CommandArgument);
        //int categoryInfoID = 0;
            //string.IsNullOrEmpty(this.gvProductsAndServices.DataKeys[index].Values["REG_DME_PRODUCT_CATEGORY_INFO_ID"].ToString()) ? 0 : (int)this.gvProductsAndServices.DataKeys[index].Values["REG_DME_PRODUCT_CATEGORY_INFO_ID"];
        int categoryTypeID = string.IsNullOrEmpty(this.gvProductsAndServices.DataKeys[index].Values["DME_PRODUCT_SERVICE_CATEGORY_TYPE_ID"].ToString()) ? 0 : (int)this.gvProductsAndServices.DataKeys[index].Values["DME_PRODUCT_SERVICE_CATEGORY_TYPE_ID"];
        ucDMEProductsAndServices.LoadData(categoryTypeID);
        mpe.Show();
    }
    private void SetButtons(string commandName)
    {
        // Save is disabled by default
        btnSave.Visible = false;
        btnCancel.Text = "Close";

        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            btnSave.Visible = true;
            btnCancel.Text = "Cancel";
        }
    }



    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        SetButtons("History");
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_REGISTERED_AGENT_History");
        DataTable dt = null;
        if (Helper.HasRows(ds)) dt = ds.Tables[0];

        switch (e.CommandName)
        {

            case "RegAgentHistory":
                lblTitle.Text = "Registered Agent History";
                mltPopup.ActiveViewIndex = 3;
                ucDMERegAgentHistory.LoadData(dt);
                mpe.Show();
                break;
        }
    }
    public override bool HasInputValue()
    {
        //bool rtn = false;
        //if (gvRegAgent != null || gvProductsAndServices!=null)
        //{
        //    if (gvRegAgent.Rows.Count > 0 || gvProductsAndServices.Rows.Count>0)
        //    {
        //        rtn = true;
        //    }
        //}
        
        return true;
    }
    protected void btnProductandServiceHistory_Click(object sender, CommandEventArgs e)
    {
        SetButtons("History");
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DME_PRODUCT_CATEGORY_INFO_History");
        DataTable dt = null;
        if (Helper.HasRows(ds)) dt = ds.Tables[0];

        switch (e.CommandName)
        {

            case "ProductandServiceHistory":
                lblTitle.Text = "Product and Services History";
                mltPopup.ActiveViewIndex = 4;
                ucDMEProductsAndServicesHistory.LoadData(dt);
                mpe.Show();
                break;
        }
    }

    public override void LoadData(DataRow row = null)
    {

    }

    public override string ValidationGroup
    {
        get { return "valDME"; }
    }

    public override string Title
    {
        get { return "DME Information"; }
    }

    public override string IdText
    {
        get { return "ucDME_" + this.WorkflowPage.RegistrationId; }
    }

    protected void cblAgencies_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (ListItem i in cblAgencies.Items)
        {
            if (i.Selected && i.Text == "Other, please specify below (Other)")
                txtOtherReasons.Enabled = true;
            else if (i.Text == "Other, please specify below (Other)")
                txtOtherReasons.Enabled = false;
        }
    }
    private bool CheckUploadedDocuments()
    {
        // Check if there are documents uploaded per license
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        // If the page is not visible no reason to check further
        //if (!Registration.PageIsVisible(Registration.GetStepText(CON.RegistrationPageType.SubstituteW4Form))) return true;

        DataSet dsDoc = psc.SelectRegDocuments(this.WorkflowPage.RegistrationId, CON.RegistrationPageType.DMEInformation, string.Empty,
            CON.RegistrationPageType.Certification.ToString(), null);
        bool isGood = true;
        if (!Helper.HasRows(dsDoc)) isGood = false;

        return isGood;
    }
}