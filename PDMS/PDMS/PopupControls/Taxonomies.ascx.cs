using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Taxonomies : BaseSectionControl
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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

    public delegate void ReloadPopupEventHandler();
    public event ReloadPopupEventHandler ReloadPopupEvent;
    public bool OwnershipChangedFlag { get; set; }

    public bool IsPrimary
    {
        get
        {
            if (ViewState["IsPrimary"] == null) ViewState["IsPrimary"] = false;
            return Convert.ToBoolean(ViewState["IsPrimary"]);
        }
        set { ViewState["IsPrimary"] = value; }
    }
    private PDMSService.PDMSServiceClient _psc;
    private PDMSService.PDMSServiceClient psc
    {
        get
        {
            if (_psc == null)
            {
                _psc = new PDMSService.PDMSServiceClient();
            }

            return _psc;
        }
    }
    public int ProviderTypeId
    {
        get
        {
            if (ViewState["ProviderTypeId"] == null) ViewState["ProviderTypeId"] = 0;
            return Convert.ToInt32(ViewState["ProviderTypeId"]);
        }
        set { ViewState["ProviderTypeId"] = value; }
    }

    public bool HasPrimary
    {
        get
        {
            if (ViewState["HasPrimary"] == null) ViewState["HasPrimary"] = false;
            return Convert.ToBoolean(ViewState["HasPrimary"]);
        }
        set { ViewState["HasPrimary"] = value; }
    }

    public bool CanUserViewDelete()
    {

        return Registration.CanUserViewDelete(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
    }
    public override void LoadData(DataRow dr)
    {
        //ParentTable.Rows[0].Cells[2].Style["display"] = Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) ? "block" : "none";
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        if (ddlTaxonomy.Items.Count == 0)
        {
            string npi = "";
            ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
            if (Helper.HasRows(ds) && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                npi = Helper.GetData("NPI", ds.Tables[0].Rows[0]);

            bool isValid = true;
            if (!string.IsNullOrEmpty(npi))
                npi = npi.Trim();
            if (string.IsNullOrEmpty(npi))
            {
                AddError("No NPI found for this provider.", ref isValid, "valTaxonomiesOverall");
                isValid = false;
            }
            else
            {
                NPPESAPIResult result = psc.ValidNPIinNPPESApi(Convert.ToInt64(npi));
                if (result.result_count > 0)
                {
                    DataTable dt = NPPESAPIHelper.LoadTaxonomyFromNPPES(result.results[0], null);

                    if (!Methods.HasRows(dt))
                        AddError(NPPESAPIHelper.ErrorMessageNoTaxonomy, ref isValid, "valTaxonomies");
                    else
                        Helper.LoadList(ddlTaxonomy, dt, "TaxonomyNameWithCode", "TaxonomyCode", true);
                }
                else
                    AddError(NPPESAPIHelper.ErrorMessageNoTaxonomy, ref isValid, "valTaxonomies");
            }
        }

        hdnRegTaxonomyID.Value = string.Empty;
        chkIsPrimary.Enabled = !HasPrimary;
        if (dr != null)
        {
            hdnRegTaxonomyID.Value = Helper.GetData("REG_TAXONOMY_ID", dr);
            int taxonomyTypeID;
            string taxonomyCode = Helper.GetData("TAXONOMY_CODE", dr);

            if (!string.IsNullOrEmpty(taxonomyCode) &&
                 ddlTaxonomy.Items.FindByValue(taxonomyCode) != null)
            {
                taxonomyTypeID = Helper.GetInt("TAXONOMY_TYPE_ID", dr);
                ddlTaxonomy.SelectedValue = taxonomyCode;
            }

            // Start and End Date
            if (!string.IsNullOrEmpty(Helper.GetData("START_DATE", dr)))
            {
                txtTaxonomyStart.Text = Helper.GetDate("START_DATE", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("END_DATE", dr)))
            {
                txtTaxonomyEnd.Text = Helper.GetDate("END_DATE", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("PRIMARY_FLAG", dr)))
            {
                chkIsPrimary.Checked = Helper.GetBool("PRIMARY_FLAG", dr);
                if (!chkIsPrimary.Checked)
                    chkIsPrimary.Enabled = true;
            }
            else
            {
                chkIsPrimary.Enabled = true;
            }
        }
        else
        {
            if (ddlTaxonomy.Items.Contains(new ListItem()))
            {
                ddlTaxonomy.SelectedIndex = 0;
            }
            txtTaxonomyStart.Text = "";
            txtTaxonomyEnd.Text = "";
            chkIsPrimary.Checked = false;
            chkIsPrimary.Enabled = true;
        }

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");

            //Enable History popup OK button
            Helper.SetReadOnly(pnlMain, false);
        }
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Convert.ToBoolean(Session["ViewProviderFile"]))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
            btnAddTaxonomies.Visible = false;
        }
    }

    public override bool SaveData()
    {
        if (!ValidateData())
            return false;

        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            if (taxonomyDetail.Visible && (ddlTaxonomy.SelectedIndex > 0))
            {
                int regTaxonomyID = 0;
                if (!string.IsNullOrEmpty(hdnRegTaxonomyID.Value))
                    regTaxonomyID = Convert.ToInt32(hdnRegTaxonomyID.Value);
                int taxonomyID = GetTaxonomyTypeID(); //get TaxonomyID if in our DB or create new ID for taxonomycode from NPPES

                bool duplicate = psc.VerifyDuplicateTaxonomyForReg(this.WorkflowPage.RegistrationId, ddlTaxonomy.SelectedValue, regTaxonomyID);
                if (!duplicate)
                {
                    //Update the Specialty
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    parms.Add("PRIMARY_FLAG", (chkIsPrimary.Checked) ? "1" : "0");
                    parms.Add("TAXONOMY_TYPE_ID", taxonomyID.ToString());
                    parms.Add("START_DATE", Convert.ToDateTime(txtTaxonomyStart.Text).ToString());
                    //OHPNM-9472 - making end date editable for internal users as per DSD.
                    if (Helper.IsUserInInternalRole(CON.InternalRoles))
                    {
                        if (txtTaxonomyEnd.Text.Trim() == "")
                            parms.Add("END_DATE", "12/31/2299");
                        else
                            parms.Add("END_DATE", Convert.ToDateTime(txtTaxonomyEnd.Text).ToString());
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(txtTaxonomyEnd.Text))
                            parms.Add("END_DATE", "12/31/2299");
                        else
                            parms.Add("END_DATE", Convert.ToDateTime(txtTaxonomyEnd.Text.Trim()).ToString());
                    }
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                    //OHPNM-9320 --moving this because all records for thisreg_id with primarty_flag true will be updated at once-- need not be inside the loop
                    //DCPDMS-2432 changes for updating other taxonomies primary flag to 0 if current one is selected
                    if (chkIsPrimary.Checked) //chkIsPrimary - is the value for newly added rec so always will be true is checkbox is checked
                    {
                        Dictionary<string, string> parms1 = new Dictionary<string, string>();
                        parms1.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                        parms1.Add("PrimaryFlag", "1");
                        parms1.Add("SetToPrimaryFlag", "0");
                        psc.UpdateRegistrationDataWithParams("usp_UpdateREG_TAXONOMY_Primary", parms1);
                    }

                    if (!string.IsNullOrEmpty(hdnRegTaxonomyID.Value))
                    {
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                        parms.Add("REG_TAXONOMY_ID", hdnRegTaxonomyID.Value);
                        psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "TAXONOMY", parms);
                    }
                    else
                    {
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
                        psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "TAXONOMYCustom", parms);
                    }

                    lblDuplicate.Visible = false; //The taxonomy has already been added to this registration. Please select a different one
                    taxonomyDetail.Visible = false; // add new taxonomy fields 
                    return true;
                }
                else
                {
                    lblDuplicate.Visible = true;
                    return false;
                }
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("ucTaxonomies_SaveData - " + ex.Message));
        }

    }
    private int GetTaxonomyTypeID()
    {
        int taxonomyTypeID = 0;
        string taxonomyCode = ddlTaxonomy.SelectedIndex > -1 ? ddlTaxonomy.SelectedValue : "0";

        DataSet ds = psc.SelectTaxonomyInfoByCode(taxonomyCode, ProviderTypeId);
        if (Methods.HasRows(ds))
            taxonomyTypeID = Methods.GetIntValue(ds.Tables[0].Rows[0], "TAXONOMY_TYPE_ID");

        if (taxonomyTypeID == 0)
        {
            //removing the code from the taxonomy name detail in the dropdown text ...
            string TaxonomyDetail = ddlTaxonomy.SelectedIndex > -1 ? ddlTaxonomy.SelectedItem.Text.Replace("(" + ddlTaxonomy.SelectedValue + ")", "").Trim() : "";

            taxonomyTypeID = psc.InsertTaxonomyCode(0, ProviderTypeId, taxonomyCode, TaxonomyDetail, Convert.ToDateTime("9999-12-31"), "", DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name));
        }
        return taxonomyTypeID;
    }

    private void AddError(string errMsg, ref bool isGood, string ValidationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public override bool ValidateData()
    {
        bool isValid = true;

        if (taxonomyDetail.Visible && ddlTaxonomy.SelectedIndex == 0)
        {
            AddError("* Taxonomy is required.", ref isValid, "valTaxonomies");
            isValid = false;
        }

        if (this.DataList != null && this.DataList.Rows.Count > 0)
        {
            int regTaxonomyID = 0;
            bool isPrimaryTaxonomy = false;
            if (!string.IsNullOrEmpty(hdnRegTaxonomyID.Value))
                regTaxonomyID = Convert.ToInt32(hdnRegTaxonomyID.Value);

            foreach (DataRow dr in this.DataList.Rows)
            {
                var drRegTaxonomyId = Helper.GetData("REG_TAXONOMY_ID", dr);
                if (Convert.ToInt32(drRegTaxonomyId) == regTaxonomyID)
                {
                    isPrimaryTaxonomy = Helper.GetBool("PRIMARY_FLAG", dr);
                    break;
                }
            }
            if(isPrimaryTaxonomy == true && chkIsPrimary.Checked == false)
            {
                AddError("* Primary taxonomy can not be changed to secondary.", ref isValid, "valTaxonomies");
                isValid = false;
            }
        }

        if (!string.IsNullOrEmpty(txtTaxonomyEnd.Text))
        {
            DateTime enddate;
            bool validDate = DateTime.TryParse(txtTaxonomyEnd.Text.Trim(), out enddate);
            if (validDate == false)
            {
                AddError("*Enter Valid Taxonomy End Date", ref isValid, "valTaxonomies");
                isValid = false;
            }
        }

        if (!string.IsNullOrEmpty(txtTaxonomyStart.Text))
        {
            DateTime enddate;
            bool validDate = DateTime.TryParse(txtTaxonomyStart.Text.Trim(), out enddate);
            if (validDate == false)
            {
                AddError("*Enter Valid Taxonomy Start Date", ref isValid, "valTaxonomies");
                isValid = false;
            }
        }

        if (isValid && !string.IsNullOrEmpty(txtTaxonomyEnd.Text))
        {
            if (Convert.ToDateTime(txtTaxonomyStart.Text) >= Convert.ToDateTime(txtTaxonomyEnd.Text))
            {
                AddError("* End date need to be greater than start date.", ref isValid, "valTaxonomies");
                isValid = false;
            }

            DataSet ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                int statusCode = Helper.GetInt("RegistrationProgramStatusTypeID", ds.Tables[0].Rows[0]);
                if (statusCode != 6)
                {
                    int result = DateTime.Compare(Convert.ToDateTime(txtTaxonomyEnd.Text), DateTime.Now);

                    if (result < 0) // expiration is before today's date
                    {
                        AddError("* End date need to be greater than today's date.", ref isValid, "valTaxonomies");
                        isValid = false;
                    }
                }
            }
        }

        if(this.DataList == null && !taxonomyDetail.Visible)
        {
            AddError("* This is a required section.", ref isValid, "valTaxonomies");
            isValid = false;
        }

        return isValid;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Taxonomies";
    }
    public override void LoadControlData()
    {
        LoadTaxonomies();
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_TAXONOMYHistory", parms);
            if (Helper.HasRows(ds))
            {
                grd.DataSource = ds.Tables[0];
                grd.DataBind();
                grd.MasterTableView.ExportToExcel();
            }
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        taxonomyDetail.Visible = true;
        DataRow dr = this.DataList.Rows[index];

        if (e.CommandName == "DeleteTaxonomiesRow")
        {
            bool IsDeleted = DeleteTaxonomy(dr);
            if (IsDeleted) LoadTaxonomies();
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
        lblDuplicate.Visible = false;
        taxonomyDetail.Visible = true;
        this.LoadData(null);
    }

    private void LoadTaxonomies()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PrimaryFlag", "1");
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_TAXONOMY", parms);
        if (Helper.HasRows(ds)) grdTaxonomies.DataSource = this.DataList = ds.Tables[0];
        else grdTaxonomies.DataSource = this.DataList = null;
        grdTaxonomies.DataBind();

        btnAddTaxonomies.Visible = true;
        //PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //DataSet ds1 = psc.SelectTaxonomyTypes();
        //if (Helper.HasRows(ds1))
        //{
        //    Dictionary<int, string> taxonomyMap = new Dictionary<int, string>();
        //    ds1.Tables[0].DefaultView.Sort = "TAXONOMY_CODE";

        //    foreach (DataRow row in ds1.Tables[0].Rows)
        //    {
        //        taxonomyMap.Add(Convert.ToInt32(row["TAXONOMY_TYPE_ID"]), row["TAXONOMY_CODE"] as string + " - " + row["TAXONOMY_NAME"] as string);
        //    }
        //    ddlTaxonomy.DataSource = taxonomyMap;
        //    ddlTaxonomy.DataTextField = "Value";
        //    ddlTaxonomy.DataValueField = "Key";
        //    ddlTaxonomy.DataBind();

        //}
        LoadData(null);
    }



    public override string ValidationGroup
    {
        get { return "valTaxonomies"; }
    }

    public override string Title
    {
        get { return "Edit Taxonomy"; }
    }

    public override string IdText
    {
        get { return "ucTaxonomies_" + this.WorkflowPage.RegistrationId; }
    }
    private bool DeleteTaxonomy(DataRow dr)
    {
        bool isValid = true;
        if (this.DataList.Rows.Count == 1 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId, CON.RegistrationPageName.Certification, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.SpecialtiesTaxonomies)))
        //if only one record and required page then prevent deletion or else the page will not turn back to blue once it is green and will cause issues for page submission
        {
            AddError("* This is required section. This taxonomy cannot be deleted.", ref isValid, ValidationGroup);
            return isValid;
        }
        if (dr != null)
        {
            int REG_TAXONOMY_ID = Helper.GetInt("REG_TAXONOMY_ID", dr);
            int TAXONOMY_ID = Helper.GetInt("TAXONOMY_ID", dr);
            bool IsPrimary = Helper.GetBool("PRIMARY_FLAG", dr);
            if (TAXONOMY_ID == 0 && !IsPrimary)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.DeleteRegistrationData("TAXONOMY", "REG_TAXONOMY_ID", REG_TAXONOMY_ID);
            }
            else
            {
                if (IsPrimary)
                    AddError("* The Primary Taxonomy can not be deleted.", ref isValid, ValidationGroup);
                else
                    AddError("* Taxonomy cannot be deleted.", ref isValid, ValidationGroup);

                return isValid;
            }
        }
        return isValid;
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        lblTitle.Text = "Taxonomies History";
        ucTaxonomiesHistory.LoadData(0);
        mpe.Show();
    }


    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }
}