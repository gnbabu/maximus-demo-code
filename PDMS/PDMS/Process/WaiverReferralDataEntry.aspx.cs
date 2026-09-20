using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_DIDDReferralDataEntry : System.Web.UI.Page
{
    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

    private const string READONLY_CSS = "formFieldReadOnly";

    public int ReferralID
    {
        get
        {
            return ViewState["ReferralID"] == null ? 0 : Convert.ToInt32(ViewState["ReferralID"]);
        }
        set
        {
            ViewState["ReferralID"] = value;
        }
    }

    public int WaiverRegistrationCurrentStepID
    {
        get
        {
            return ViewState["WaiverRegistrationCurrentStepID"] == null ? 0 : Convert.ToInt32(ViewState["WaiverRegistrationCurrentStepID"]);
        }
        set
        {
            ViewState["WaiverRegistrationCurrentStepID"] = value;
        }
    }


    public int WaiverRegistrationStatusTypeID
    {
        get
        {
            return ViewState["WaiverRegistrationStatusTypeID"] == null ? 0 : Convert.ToInt32(ViewState["WaiverRegistrationStatusTypeID"]);
        }
        set
        {
            ViewState["WaiverRegistrationStatusTypeID"] = value;
        }
    }

    public int WaiverRegProgramStatusTypeID
    {
        get
        {
            return ViewState["WaiverRegProgramStatusTypeID"] == null ? 0 : Convert.ToInt32(ViewState["WaiverRegProgramStatusTypeID"]);
        }
        set
        {
            ViewState["WaiverRegProgramStatusTypeID"] = value;
        }
    }

    private bool WaiverRegistrationInMaint
    {
        get
        {
            return WaiverRegistrationCurrentStepID == 0  && WaiverRegistrationStatusTypeID > 0 ? true : false;
        }
    }

    private bool UserCanAddEdit
    {
        get
        {
            return Helper.IsUserInDIDDRoles(HttpContext.Current.User.Identity.Name) || Helper.IsLoggedInUserInAdminRole();
        }
    }

    private bool ReferralEditable
    {
        get
        {
            //editable when new referral, referral is not attached to a registration or registration is still in conversion status.
            return !UserCanAddEdit ? false : ReferralID == 0 || WaiverRegistrationStatusTypeID == 0 || WaiverRegProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion ? true : false;
        }
    }

    private bool ExistingServicesEditable
    {
        //created this as a flag so could edit when existing services could be editable  at the client level.  if always editable, just return true.
        get
        {
            return UserCanAddEdit && ReferralID == 0 ? true : false;
        }
    }

    private bool ServicesCanBeAdded
    {
        get
        { //new referral or not associated to a registration yet or in maint
            return !UserCanAddEdit ? false : ReferralID == 0 || WaiverRegistrationStatusTypeID == 0 ? true : ddlAction.SelectedIndex > 0 && WaiverRegistrationInMaint ? true : false;
        }
    }


    private void LoadServices()
    {
        LoadServicesByType(CON.WaiverType.AD);
        LoadServicesByType(CON.WaiverType.CDD);
        LoadServicesByType(CON.WaiverType.PAS);
        LoadServicesByType(CON.WaiverType.DDAC);
        LoadServicesByType(CON.WaiverType.DDAD);
     }

    private void LoadServicesByType(int waiverTypeID)
    {
        DataSet ds = psc.SelectDIDDServicesByWaiverID(waiverTypeID);
        GridView gv = null;

        switch (waiverTypeID)
        {
            case CON.WaiverType.AD:
                gv = this.grdServicesAD;
                break;
            case CON.WaiverType.CDD:
                gv = this.grdServicesCDD;
                break;
            case CON.WaiverType.PAS:
                gv = this.grdServicesPASS;
                break;
            case CON.WaiverType.DDAC:
                gv = this.grdServicesDDAC;
                break;
            case CON.WaiverType.DDAD:
                gv = this.grdServicesDDAD;
                break;
            default:
                break;
        }

        gv.DataSource = ds.Tables[0];
        gv.DataBind();

    }

    private void SetServiceChecked(int serviceID, int waiverTypeID)
    {
        GridView gv = null;
        switch (waiverTypeID)
        {
            case CON.WaiverType.AD:
                gv = this.grdServicesAD;
                break;
            case CON.WaiverType.CDD:
                gv = this.grdServicesCDD;
                break;
            case CON.WaiverType.PAS:
                gv = this.grdServicesPASS;
                break;
            case CON.WaiverType.DDAC:
                gv = this.grdServicesDDAC;
                break;
            case CON.WaiverType.DDAD:
                gv = this.grdServicesDDAD;
                break;
            default:
                break;
        }

        if (gv != null)
        {
            foreach (GridViewRow row in gv.Rows)
            {
                if (Convert.ToInt32(gv.DataKeys[row.RowIndex].Value) == serviceID)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkServiceID");
                    if (chk != null)
                    {
                        chk.Checked = true;
                        chk.Enabled = ExistingServicesEditable; //can only end date, cannot uncheck
                    }
                }
            }
        }
    }

    private void LoadReferral(DataTable dtServices)
    {
        DataSet ds = psc.SelectDIDDReferral(this.ReferralID);
        if (!Helper.HasRows(ds)) return;
        DataRow row = ds.Tables[0].Rows[0];
        txtApplicationNumber.Text = UIHelper.CreateDIDDApplicationNumberWithSuffix(row["Application_No"] as string, Convert.ToInt32(row["REFERRAL_SUFFIX"]));
        txtEmail.Text = Helper.GetString("EMAIL", row);
        txtFirstName.Text = Helper.GetString("FIRST_NAME", row);
        txtLastName.Text = Helper.GetString("LAST_NAME", row);
        txtGroupEntityName.Text = Helper.GetString("GROUP_ENTITY_NAME", row);
        nbTaxID.Text = Helper.GetString("TAX_ID", row);
        txtContractFromDate.Text = txtContractToDate.Text = string.Empty;
        if (!string.IsNullOrEmpty(Helper.GetString("DIDDCONTRACT_FROMDATE", row))) 
            txtContractFromDate.Text = Helper.GetDate("DIDDCONTRACT_FROMDATE", row);
        if (!string.IsNullOrEmpty(Helper.GetString("DIDDCONTRACT_TODATE", row)))
            txtContractToDate.Text = Helper.GetDate("DIDDCONTRACT_TODATE", row);
        if (!string.IsNullOrEmpty(Helper.GetString("SERVICING_ZIP", row)))
            txtZip.Text = Helper.GetString("SERVICING_ZIP", row);
        if (!string.IsNullOrEmpty(Helper.GetString("SERVICING_EXT_ZIP", row)))
            txtZipExt.Text = Helper.GetString("SERVICING_EXT_ZIP", row);
        int submitTypeID = Helper.GetInt("SUBMIT_TYPE_ID", row);
        rblSubmitType.SelectedValue = submitTypeID == 0 ? "2" : submitTypeID.ToString();
        int locationTypeID = Helper.GetInt("DIDD_REFERRAL_LOCATION_TYPE_ID", row);
        if (Helper.ValueExistsInDropDown(this.ddlLocation, locationTypeID.ToString()))
        {
            this.ddlLocation.SelectedValue = locationTypeID.ToString();
        }
        int convertedReferralReviewID = Helper.GetInt("CONVERTED_REFERRAL_REVIEW_ID", row);

        foreach (DataRow dr in ds.Tables[1].Rows)
        {
            SetServiceChecked(Helper.GetInt("DIDD_SERVICE_ID", dr), Helper.GetInt("DIDD_WAIVER_ID", dr));
        }

        // REFERRAL TYPE SHOULD BE READ ONLY ON EDIT
        chkAssistedLiving.Enabled = false;
                
        //If assisted living is selected
        if (Helper.GetInt("DIDD_REFERRAL_TYPE_ID", row) == CON.DiddReferralType.AssistedLiving)
        {
            chkAssistedLiving.Checked = true;

            // If it's a converted referral they cannot select NFOCUS boxes nor change from assisted living
            if (convertedReferralReviewID != 0)
            {
                foreach (ListItem li in chklstWaiver.Items)
                {
                    li.Enabled = false;
                }
            }
        }
        else
        {
            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                ListItem listItem = chklstWaiver.Items.FindByValue(Helper.GetString("DIDD_WAIVER_ID", dr));
                if (listItem != null)
                {
                    listItem.Selected = true;
                    listItem.Enabled = ExistingServicesEditable;//do not allow to unselect if services are not editable.
                }
            }
        }   
    }

    private void SetGridDisplays()
    {
        grdServicesAD.Visible = grdServicesCDD.Visible = grdServicesDDAC.Visible = grdServicesDDAD.Visible = grdServicesPASS.Visible = false;

        if (chklstWaiver.Items[0].Selected)
            grdServicesAD.Visible = true;
        if (chklstWaiver.Items[1].Selected)
            grdServicesCDD.Visible = true;
        if (chklstWaiver.Items[2].Selected)
            grdServicesPASS.Visible = true;
        if (chklstWaiver.Items[3].Selected)
            this.grdServicesDDAC.Visible = true;
        if (chklstWaiver.Items[4].Selected)
            this.grdServicesDDAD.Visible = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hdnDIDD_REFERRAL_TYPE_ID.Value = string.Empty;
            this.ReferralID = Request.QueryString["DIDDReferralId"] != null ? int.Parse(Request.QueryString["DIDDReferralId"]) : 0;
            if (Request.QueryString["DIDDReferralId"] != null)
            {
                hdnDIDD_REFERRAL_TYPE_ID.Value = ReferralID.ToString();
            }
            this.LoadDropDowns();

            DataTable dt = null;
            DataSet ds = psc.SelectDIDDServices();
            if (Helper.HasRows(ds)) dt = ds.Tables[0];
            this.LoadServices();
            this.SetRegistationStatuses();

            if (this.ReferralID > 0)
            {
                LoadReferral(dt);
                SetGridDisplays();
            }
            else
            {
                ResetAllChkBoxList();
                grdServicesAD.Visible = grdServicesCDD.Visible = grdServicesDDAC.Visible = grdServicesDDAD.Visible = grdServicesPASS.Visible = true;
            }
            SetFieldEditability();
            txtFirstName.Focus();
        }
    }

    private void LoadDropDowns()
    {
        LoadReferralTypes();
        LoadWaiverTypes();
        LoadLocationTypes();
    }

    private void LoadReferralTypes()
    {
        DataSet dsReferraltypes = psc.SelectDIDDReferralTypes();

        if (Helper.HasRows(dsReferraltypes))
        {
            Helper.LoadList(ddlAction, dsReferraltypes, "DIDD_REFERRAL_TYPE", "DIDD_REFERRAL_TYPE_ID", true);
            // Remove the normal contract and assisted living from the list
            ListItem itm = ddlAction.Items.FindByValue(CON.DiddReferralType.NormalContract.ToString());
            if (itm != null) ddlAction.Items.Remove(itm);
            
            itm = ddlAction.Items.FindByValue(CON.DiddReferralType.AssistedLiving.ToString());
            if (itm != null) ddlAction.Items.Remove(itm);

            if (ddlAction.Items.Count == 2)
                ddlAction.SelectedIndex = 1; //first one is blank one.
        }
    }
   
    private void LoadWaiverTypes()
    {
        DataSet dsWaivers = psc.SelectDIDDWaivers();
        if (Helper.HasRows(dsWaivers))
        {
            chklstWaiver.DataSource = dsWaivers;
            chklstWaiver.DataTextField = "NAME";
            chklstWaiver.DataValueField = "DIDD_WAIVER_ID";
            chklstWaiver.DataBind();
        }
    }
    
    private void LoadLocationTypes()
    {
        DataSet ds = psc.SelectDiddReferralLocationTypes();
        if (Helper.HasRows(ds))
        {
            this.ddlLocation.DataSource = ds;
            this.ddlLocation.DataTextField = "DIDD_REFERRAL_LOCATION_TYPE_NAME";
            this.ddlLocation.DataValueField = "DIDD_REFERRAL_LOCATION_TYPE_ID";
            this.ddlLocation.DataBind();
        }
        if (this.ddlLocation.Items.Count > 1)
        {
            this.ddlLocation.Items.Insert(0, new ListItem("", "0"));
        }
    }

    private void SetRegistationStatuses()
    {
        WaiverRegistrationStatusTypeID = 0;
        WaiverRegProgramStatusTypeID = 0;
        WaiverRegistrationCurrentStepID = 0;

        DataSet ds =  Registration.GetDIDDRegistrationStatuses(ReferralID);
        if (Helper.HasRows(ds))
        {
            DataRow row = ds.Tables[0].Rows[0];
            WaiverRegistrationStatusTypeID = Helper.GetInt("RegistrationStatusTypeID", row);
            WaiverRegProgramStatusTypeID = Helper.GetInt("RegProgramSTatusTypeID", row);
            WaiverRegistrationCurrentStepID = Helper.GetInt("RegCurrentStepID", row);
        }
    }



    private void ResetAllChkBoxList()
    {       
        foreach (ListItem lst in chklstWaiver.Items)
        {
            lst.Selected = true;
            lst.Enabled = true;
        }
    }

    private string GetFormFieldCSS(string appendCss, string currentCss)
    {
        return string.IsNullOrEmpty(appendCss) ? currentCss.Replace(READONLY_CSS, string.Empty) : currentCss + " " + READONLY_CSS; 
    }

    private void SetFieldEditability()
    {
        string readOnlyCSS = ReferralEditable ? string.Empty : READONLY_CSS;
        tr_Action.Visible = WaiverRegistrationInMaint;
        lblPageTitle.Text = ReferralID == 0 ? Resources.BrandingResource.WAIVER_SERVICE_INSERT_PAGE_TITLE : ServicesCanBeAdded ? Resources.BrandingResource.WAIVER_SERVICE_EDIT_PAGE_TITLE : Resources.BrandingResource.WAIVER_SERVICE_VIEW_PAGE_TITLE;
        nbTaxID.Enabled = ReferralEditable;
        nbTaxID.CssClass = GetFormFieldCSS(readOnlyCSS, nbTaxID.CssClass);
        txtFirstName.Enabled = ReferralEditable;
        txtFirstName.CssClass = GetFormFieldCSS(readOnlyCSS, txtFirstName.CssClass);
        txtLastName.Enabled = ReferralEditable;
        txtLastName.CssClass = GetFormFieldCSS(readOnlyCSS, txtLastName.CssClass);
        txtGroupEntityName.Enabled = ReferralEditable;
        txtGroupEntityName.CssClass = GetFormFieldCSS(readOnlyCSS, txtGroupEntityName.CssClass);
        txtZip.Enabled = ReferralEditable;
        txtZip.CssClass = GetFormFieldCSS(readOnlyCSS, txtZip.CssClass);
        txtZipExt.Enabled = ReferralEditable;
        txtZipExt.CssClass = GetFormFieldCSS(readOnlyCSS, txtZipExt.CssClass);
        txtEmail.Enabled = ReferralEditable;
        txtEmail.CssClass = GetFormFieldCSS(readOnlyCSS, txtEmail.CssClass); 
        rblSubmitType.Enabled = ReferralEditable;
        //txtContractFromDate.Enabled = ReferralEditable;
        //txtContractToDate.CssClass += " " + readOnlyCSS;

        chklstWaiver.Enabled = ServicesCanBeAdded;
        ceFromDate.Enabled = ceToDate.Enabled = ReferralEditable;

        grdServicesAD.Enabled = ServicesCanBeAdded;
        grdServicesCDD.Enabled = ServicesCanBeAdded;
        grdServicesPASS.Enabled = ServicesCanBeAdded;
        grdServicesDDAC.Enabled = ServicesCanBeAdded;
        grdServicesDDAD.Enabled = ServicesCanBeAdded;

        this.btnClear.Enabled = ReferralEditable;
        this.btnSubmit.Enabled = ServicesCanBeAdded;
        if (!ServicesCanBeAdded)
        {
            lblMessage.Text = "The Referral is tied to an In-progress Provider application and can not be modified at this time.";
        }
        else
        {
            lblMessage.Text = "";
        }
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valReferral";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    private bool ValidateServices()
    {
        if (!ValidateService(CON.WaiverType.AD))
            return false;
        if (!ValidateService(CON.WaiverType.CDD))
            return false;
         if (!ValidateService(CON.WaiverType.PAS))
            return false;
         if (!ValidateService(CON.WaiverType.DDAC))
            return false;
         if (!ValidateService(CON.WaiverType.DDAD))
             return false;
     
        return true;
    }

    private bool ValidateService(int waiverTypeID)
    {
        GridView gv = null;
        bool isOneChecked = false;

        switch (waiverTypeID)
        {
            case CON.WaiverType.AD:
                gv = this.grdServicesAD;
                break;
            case CON.WaiverType.CDD:
                gv = this.grdServicesCDD;
                break;
            case CON.WaiverType.PAS:
                gv = this.grdServicesPASS;
                break;
            case CON.WaiverType.DDAC:
                gv = this.grdServicesDDAC;
                break;
            case CON.WaiverType.DDAD:
                gv = this.grdServicesDDAD;
                break;
            default:
                break;
        }

        if (!gv.Visible)
            return true;

        foreach (GridViewRow row in gv.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("chkServiceID");
            if (chk != null)
            {
                if (chk.Visible && chk.Checked)
                {
                    isOneChecked = true;
                    break;
                }
            }
        }

        return isOneChecked;

    }

    private bool ValidateData()
    {
        bool isGood = true;

        if (string.IsNullOrEmpty(txtFirstName.Text) && string.IsNullOrEmpty(txtLastName.Text) && string.IsNullOrEmpty(txtGroupEntityName.Text))
            AddError("*Enter First/Last Name or Group/Entity Name", ref isGood);
        if ((!string.IsNullOrEmpty(txtFirstName.Text) || !string.IsNullOrEmpty(txtLastName.Text)) && !string.IsNullOrEmpty(txtGroupEntityName.Text))
            AddError("*Enter First/Last Name -OR- Group/Entity Name but not both", ref isGood);
        if (string.IsNullOrEmpty(txtFirstName.Text) && !string.IsNullOrEmpty(txtLastName.Text) && string.IsNullOrEmpty(txtGroupEntityName.Text))
            AddError("*Enter First Name", ref isGood);
        if (!string.IsNullOrEmpty(txtFirstName.Text) && string.IsNullOrEmpty(txtLastName.Text) && string.IsNullOrEmpty(txtGroupEntityName.Text))
            AddError("*Enter Last Name", ref isGood);
        if (string.IsNullOrEmpty(nbTaxID.Text))
            AddError("*Enter Tax ID", ref isGood);
        //if (string.IsNullOrEmpty(txtEmail.Text))
        //    AddError("*Enter Provider Email", ref isGood);
        if (!string.IsNullOrWhiteSpace(txtEmail.Text.Trim()))
        {
            bool isEmail = Regex.IsMatch(txtEmail.Text.Trim(), @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            if (!isEmail)
            {
                AddError("*Enter valid email address", ref isGood);
            }        
        }
        bool waiverValid = false;
        for (var i = 0; i < chklstWaiver.Items.Count; i++)
        {
            if (chklstWaiver.Items[i].Selected)
            {
                waiverValid = true;
            }
        }
        // No need to select program/service type in case of assisted living
        if (!IsAssistedLivingReferral() && waiverValid == false)
            AddError("*Select at least one program type", ref isGood);
        if (!IsAssistedLivingReferral() && !ValidateServices())
            AddError("*Select at least one Service for each type of program type selected.", ref isGood);
        if (string.IsNullOrEmpty(txtZip.Text))
            AddError("*Enter Zipcode", ref isGood);
        if (string.IsNullOrEmpty(txtZipExt.Text))
            AddError("*Enter Zip Ext", ref isGood);
        //check for existing referral with same tax id, zip bug 6727
        Dictionary<string, string> parms = new Dictionary<string, string>();
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        parms.Add("TAX_ID", nbTaxID.Text.ToString());
        parms.Add("ZIP", txtZip.Text);
        //added zipext for bug 7531
        parms.Add("ZIPEXT", txtZipExt.Text);
        if (ReferralID > 0)
        {
            parms.Add("REFERRAL_ID", ReferralID.ToString());
        }
        DataSet ds1 = svc.SelectRegistrationDataWithParams("usp_Select_Referral_By_TaxID_Zip", parms);
        if (Helper.HasRows(ds1))
        {
            AddError("Referral already exists with same Tax ID , Zip and Zip Ext. Please contact the help desk.", ref isGood);
        }
        return isGood;
    }

    private bool ValidateDuplicateReferral()
    {
        bool isGood = true;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        parms.Add("TAX_ID", nbTaxID.Text.ToString());
        parms.Add("ZIP", txtZip.Text);
        //added zipext for bug 7531
        parms.Add("ZIPEXT", txtZipExt.Text);
        if (ReferralID > 0)
        {
            parms.Add("REFERRAL_ID", ReferralID.ToString());
        }
        DataSet ds1 = svc.SelectRegistrationDataWithParams("usp_Select_Referral_By_TaxID_Zip", parms);
        if (Helper.HasRows(ds1))
        {
            AddError("Referral already exists with same Tax ID , Zip and Zip Ext. Please contact the help desk.", ref isGood);
        }
        return isGood;
    }
    private bool IsAssistedLivingReferral()
    {
        return chkAssistedLiving.Checked;
    }

    private void SaveServices(int referralID)
    {
        SaveService(CON.WaiverType.AD, referralID);
        SaveService(CON.WaiverType.CDD, referralID);
        SaveService(CON.WaiverType.PAS, referralID);
        SaveService(CON.WaiverType.DDAC, referralID);
        SaveService(CON.WaiverType.DDAD, referralID);
    }


    private void SaveService(int waiverTypeID, int referralID)
    {
        GridView gv = null;

        switch (waiverTypeID)
        {
            case CON.WaiverType.AD:
                gv = this.grdServicesAD;
                break;
            case CON.WaiverType.CDD:
                gv = this.grdServicesCDD;
                break;
            case CON.WaiverType.PAS:
                gv = this.grdServicesPASS;
                break;
            case CON.WaiverType.DDAC:
                gv = this.grdServicesDDAC;
                break;
            case CON.WaiverType.DDAD:
                gv = this.grdServicesDDAD;
                break;
            default:
                break;
        }

        if (gv.Visible)
        {
            foreach (GridViewRow row in gv.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkServiceID");
                if (chk != null )
                {
                    if (chk.Visible && chk.Checked && chk.Enabled) psc.InsertDIDDReferralService(referralID, Convert.ToInt32(gv.DataKeys[row.RowIndex].Value));
                }
            }

        }
    }


    //private void RewriteChildren(int action, int id, ref string Regions)
    //{
    //    //if (action == DIDDChildren.Services)
    //    //{
    //    //    psc.DeleteDIDDREFERRALSERVICE(id);
    //    //    SaveServices(id);
    //    //}
    //    //else if (action == DIDDChildren.Regions)
    //    //{
    //    //    psc.DeleteDIDDREFERRALREGION(id);
    //    //    foreach (ListItem lst in chklstRegion.Items)
    //    //    {
    //    //        if (lst.Selected == true)
    //    //        {
    //    //            if (lst.Enabled)
    //    //            {
    //    //                Regions += lst.Text.Substring(0, 1);
    //    //            }
    //    //            psc.InsertDIDDREFERRALREGION(id, Convert.ToInt32(lst.Value));
    //    //        }
    //    //    }
    //    //}
    //    else if (action == DIDDChildren.Waivers)
    //    {
    //        psc.DeleteDIDDREFERRALWAIVER(id);
    //        foreach (ListItem lst in chklstWaiver.Items)
    //        {
    //            if (lst.Selected == true)
    //            {
    //                psc.InsertDIDDREFERRALWAIVER(id, Convert.ToInt32(lst.Value));
    //            }
    //        }
    //    }
    //}

    private void SaveData()
    {
        try
        {
            string Regions = string.Empty;
            string Waiver = string.Empty;
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string entityName = txtGroupEntityName.Text.Trim();
            string taxID =  nbTaxID.Text.Trim();
            string email = txtEmail.Text.Trim();
            string zipCode = txtZip.Text.Trim();
            string zipExt = txtZipExt.Text.Trim();
            int locationTypeID = Convert.ToInt32(ddlLocation.SelectedValue);
            int submitTypeID = Convert.ToInt32(rblSubmitType.SelectedValue);
            DateTime modifiedDate = DateTime.Now;
            Guid modifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

            DateTime? fromDate = null;
            if (!string.IsNullOrWhiteSpace(txtContractFromDate.Text))
            {
                fromDate = Convert.ToDateTime(txtContractFromDate.Text);
            }
            DateTime? toDate = null;
            if (!string.IsNullOrWhiteSpace(txtContractToDate.Text))
            {
                toDate = Convert.ToDateTime(txtContractToDate.Text);
            }

            int actionTypeID = 1;
            if (!string.IsNullOrEmpty(ddlAction.SelectedValue))
                actionTypeID = actionTypeID = Convert.ToInt32(ddlAction.SelectedValue);


            if (ReferralID > 0)
            {
                string appNumber = txtApplicationNumber.Text;

                if (actionTypeID == CON.DiddReferralType.ContractExpansion)
                {   //just updating the regions via CreateExpansion - not applicable for NE
                    psc.UpdateDIDDReferralSuffix(ReferralID, Regions);
                }
                else
                {
                    psc.UpdateDIDDReferral(ReferralID, firstName, lastName, entityName, taxID, "", email, fromDate, toDate, zipCode, zipExt, submitTypeID, locationTypeID, modifiedDate, modifiedUser,
                        IsAssistedLivingReferral() ? CON.DiddReferralType.AssistedLiving : CON.DiddReferralType.NormalContract);

                    if (!IsAssistedLivingReferral())
                    {
                        if (ExistingServicesEditable)
                        {
                            //when services are not editable by the client, the Save Services will just insert the newly added services.
                            psc.DeleteDIDDREFERRALSERVICE(ReferralID);
                        }
                        SaveServices(ReferralID);

                        if (ExistingServicesEditable)
                        {
                            //when services are not editable by the client, just insert the newly added waiver / program types
                            psc.DeleteDIDDREFERRALWAIVER(ReferralID);
                        }
                        foreach (ListItem lst in chklstWaiver.Items)
                        {
                            if (lst.Selected == true && lst.Enabled)
                            {
                                psc.InsertDIDDREFERRALWAIVER(ReferralID, Convert.ToInt32(lst.Value));
                            }
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Referral " + appNumber + " was successfully modified");
                    
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        if (!SendNotification(email, firstName, lastName, entityName, appNumber, fromDate, toDate, "Referral " + appNumber + " Modified"))
                        {
                            sb.AppendLine("<br/><br/>However, sending the email notification failed.");
                        }
                    }

                }
                ucMessageBox.Show("Referral updated successfully", "Referral Entry");
                ClearAll();
               // DisableAllFields();
            }
            else
            {
                
                
                DataSet ds = psc.InsertDIDDReferral(firstName, lastName, entityName, "", taxID, "", email, fromDate, toDate, true, zipCode, zipExt, submitTypeID, locationTypeID, modifiedDate, modifiedUser,
                    IsAssistedLivingReferral() ? CON.DiddReferralType.AssistedLiving : CON.DiddReferralType.NormalContract);
                if (!Helper.HasRows(ds)) return;
                DataRow row = ds.Tables[0].Rows[0];
                int referralID = Helper.GetInt("ID", row);
                string refNum = Helper.GetString("RefNum", row);
                txtApplicationNumber.Text = refNum;

                if (!IsAssistedLivingReferral())
                {
                    SaveServices(referralID);

                    foreach (ListItem lst in chklstWaiver.Items)
                    {
                        if (lst.Selected == true)
                        {
                            psc.InsertDIDDREFERRALWAIVER(referralID, Convert.ToInt32(lst.Value));
                        }
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("Referral added successfully. <br/> Referral Number is " + refNum);

                if (!string.IsNullOrWhiteSpace(email))
                {
                    if (!SendNotification(email, firstName, lastName, entityName, refNum, fromDate, toDate, "Referral " + refNum + " Created"))
                    {
                        sb.AppendLine("<br/><br/>However, sending the email notification failed.");
                    }
                }

                ucMessageBox.Show(sb.ToString(), "Referral Entry");
                ClearAll();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool SendNotification(string email, string firstName,string  lastName, string entityName, string appNumber, DateTime? fromDate, DateTime? toDate, string inputSubject)
    {
        bool success = true;
        if (!string.IsNullOrWhiteSpace(email))
        {
            try
            {
                // Send an email to the Provider
                psc.NotifyProviderOfDIDDReferral(firstName, lastName, entityName, appNumber, fromDate, toDate, email, inputSubject);
            }
            catch
            {
                success = false;
            }
        }

        return success;

    }
    private bool ShowPopup()
    {
        try
        {
            btnSubmit.Attributes.Remove("onclick");
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            
            mpe.Show();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //if (!ValidateData()) return;
        if (hdnDIDD_REFERRAL_TYPE_ID.Value != "" && hdnDIDD_REFERRAL_TYPE_ID.Value != "0")
        {
        //if (!ShowPopup()) 
            if (!ValidateData())
            {
            
                return;
            }
            SaveData();
        }
        else
        {
                    //Show panel to confirm the data
                    BindProviderNPITaxIDMatches();
                    if (grdProviderNPITaxIDMatches.Rows.Count > 0)
                    {
                        lblErrorMessage.Text = "";
                        mpeConfirm.Show();
                    }
                    else
                    {
                        if (!ValidateData()) return;

                        SaveData();
                    }
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        ReferralID = WaiverRegistrationStatusTypeID = WaiverRegProgramStatusTypeID = WaiverRegistrationCurrentStepID =  0;
        tr_Action.Visible = false;

        lblPageTitle.Text = Resources.BrandingResource.WAIVER_SERVICE_INSERT_PAGE_TITLE;
        txtGroupEntityName.Text = txtApplicationNumber.Text = txtEmail.Text = txtFirstName.Text = txtLastName.Text = nbTaxID.Text = string.Empty;
        txtContractFromDate.Text = txtContractToDate.Text = txtZip.Text = txtZipExt.Text = string.Empty;
        grdServicesAD.Visible = this.grdServicesCDD.Visible = this.grdServicesPASS.Visible = this.grdServicesDDAC.Visible = this.grdServicesDDAD.Visible =  true;

        if (this.ddlLocation.Items.Count > 0)
            this.ddlLocation.SelectedIndex = 0;

        if (ddlAction.Items.Count > 0)
            ddlAction.SelectedIndex = 0;

        ResetAllChkBoxList();
        LoadServices();
        SetFieldEditability();
        txtFirstName.Focus();
        if (!string.IsNullOrEmpty(hdnDIDD_REFERRAL_TYPE_ID.Value) && hdnDIDD_REFERRAL_TYPE_ID.Value != "0")
        {
            PropertyInfo isreadonly =
                typeof(System.Collections.Specialized.NameValueCollection).GetProperty(
                "IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove("DIDDReferralId");
            Request.QueryString.Clear();
            hdnDIDD_REFERRAL_TYPE_ID.Value = "";
        }
        btnSubmit.Enabled = true;
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (!ValidateDuplicateReferral()) return;
        SaveData();
    }

    protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAction.SelectedValue == CON.DiddReferralType.CreateAmendment.ToString())
        {
            this.btnSubmit.Enabled = true;
            SetFieldEditability();
        }
        else if (ddlAction.SelectedValue == CON.DiddReferralType.ContractExpansion.ToString())
        {
            txtContractFromDate.Enabled = true;
            txtContractToDate.Enabled = true;
        }
    }

    
    protected void chklstWaiver_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.grdServicesAD.Visible = this.grdServicesCDD.Visible = this.grdServicesPASS.Visible = this.grdServicesDDAC.Visible = this.grdServicesDDAD.Visible = false;
        bool normalWaiverSelected = false;
        if (chklstWaiver.Items[0].Selected)
        {
            grdServicesAD.Visible = true;
            normalWaiverSelected = true;
        }
        if (chklstWaiver.Items[1].Selected)
        {
            grdServicesCDD.Visible = true;
            normalWaiverSelected = true;
        }
        if (chklstWaiver.Items[2].Selected)
        {
            grdServicesPASS.Visible = true;
            normalWaiverSelected = true;
        }
        if (chklstWaiver.Items[3].Selected)
        {
            grdServicesDDAC.Visible = true;
            normalWaiverSelected = true;
        }
        if (chklstWaiver.Items[4].Selected)
        {
            grdServicesDDAD.Visible = true;
            normalWaiverSelected = true;
        }

        if (normalWaiverSelected)
        {
            chkAssistedLiving.Checked = false;
            this.txtFirstName.Enabled = this.txtLastName.Enabled = true;
        }
    }

    protected void chkAssistedLiving_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAssistedLiving.Checked == true)
        {
            this.grdServicesAD.Visible = this.grdServicesCDD.Visible = this.grdServicesPASS.Visible = this.grdServicesDDAC.Visible = this.grdServicesDDAD.Visible = false;
            this.txtFirstName.Enabled = this.txtLastName.Enabled = false;
            foreach (ListItem item in chklstWaiver.Items)
            {
                item.Selected = false;
            }
        }
    }
    private void BindProviderNPITaxIDMatches()
    {
        DataTable dt = psc.SearchProviderAllTaxID(nbTaxID.Text).Tables[0];
        grdProviderNPITaxIDMatches.DataSource = dt;
        grdProviderNPITaxIDMatches.DataBind();
        lblReferralStatus.Text = "";
    }

    protected void btnSubmitSelected_Click(object sender, EventArgs e)
    {
        int rowCnt = 0;
        foreach (GridViewRow gvr in grdProviderNPITaxIDMatches.Rows)
        {
            rowCnt++;
            RadioButton rdo = (RadioButton)gvr.FindControl("rdoProvider");
            if (rdo.Checked)
            {

                int DIDDReferralID = Convert.ToInt32(grdProviderNPITaxIDMatches.DataKeys[gvr.RowIndex].Value);
                if (DIDDReferralID > 0)
                {
                    Response.Redirect("~/Process/WaiverReferralDataEntry.aspx?DIDDReferralId=" + DIDDReferralID.ToString());
                }
                mpeConfirm.Hide();
                return;
            }
            else if (rowCnt == grdProviderNPITaxIDMatches.Rows.Count)
            {
                lblErrorMessage.Text = "* Please select a provider to proceed";
                
                mpeConfirm.Show();
            }
        }


    }

    protected void grdProviderNPITaxIDMatches_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int cnt = 1; cnt < e.Row.Cells.Count; cnt++)
            {
                string encoded = e.Row.Cells[cnt].Text;
                e.Row.Cells[cnt].Text = Context.Server.HtmlDecode(encoded);
            }
        }


    }

    protected void rdoProvider_CheckedChanged(object sender, EventArgs e)
    {

        RadioButton selectButton = (RadioButton)sender;
        GridViewRow gvrow = (GridViewRow)selectButton.Parent.Parent;
        
        lblReferralStatus.Text = "";

        int index = gvrow.RowIndex;
        GridViewRow row1 = grdProviderNPITaxIDMatches.Rows[index];
        GridView gvServices = (GridView)row1.FindControl("grdServices");


        DataTable dt = RefreshData(Convert.ToInt32(grdProviderNPITaxIDMatches.DataKeys[index].Value));
        DataTable dtNew = dt.Clone();
        foreach (DataRow row in dt.Rows)
        {
            
            dtNew.Rows.Add(row.ItemArray);
        }
        


        gvServices.DataSource = dtNew;
        gvServices.DataBind();

        for (int i = 0; i < grdProviderNPITaxIDMatches.Rows.Count; i++)
        {
            if (i != index)
                grdProviderNPITaxIDMatches.Rows[i].FindControl("pnlServices").Visible = false;
            if (i == index)
                grdProviderNPITaxIDMatches.Rows[i].FindControl("pnlServices").Visible = true;
        }
        
        mpeConfirm.Show();
    }
    protected void btnDoNotUseAnyOfThese_Click(object sender, EventArgs e)
    {
        mpeConfirm.Hide();
        if (!ValidateData())
        {
            
            return;
        }
        
        SaveData();

    }
    protected void btnCancelConfirm_Click(object sender, EventArgs e)
    {
        mpeConfirm.Hide();
    }

    private DataTable RefreshData(int DIDDReferralID)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectDIDDReferral(DIDDReferralID);  //This select returns 4 tables.

        return ds.Tables[1];


    }

}