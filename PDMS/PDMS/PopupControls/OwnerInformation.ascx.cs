using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_OwnerInformation : BaseSectionControl
{
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
    public string _SortField
    {
        get
        {
            return (string)ViewState["SortField"] ?? "Index"; // default sort 
        }
        set
        {
            ViewState["SortField"] = value;
        }
    }
    public int EntityTypeId
    {
        get
        {
            if (ViewState["EntityTypeId"] == null) ViewState["EntityTypeId"] = 0;
            return Convert.ToInt32(ViewState["EntityTypeId"]);
        }
        set { ViewState["EntityTypeId"] = value; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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

    public int DIDDReferralId
    {
        get
        {
            if (ViewState["DIDDReferralId"] == null) ViewState["DIDDReferralId"] = 0;
            return Convert.ToInt32(ViewState["DIDDReferralId"]);
        }
        set { ViewState["DIDDReferralId"] = value; }
    }

    public int SpecialtyTypeID
    {
        get
        {
            if (ViewState["SpecialtyTypeID"] == null) ViewState["SpecialtyTypeID"] = 0;
            return Convert.ToInt32(ViewState["SpecialtyTypeID"]);
        }
        set { ViewState["SpecialtyTypeID"] = value; }
    }

    bool IdentifyingInfoExists = false;
    bool isActiveOwnersCount = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.OwnerInformation)
        {
            SetVisibility();
            btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup + "')){this.disabled=true;} else { return false; } " +
              this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        }
        this.ucOwnerInfo.KeepPopupOpenEvent += new PopupControls_OwnerInfo.KeepPopupOpenEventHandler(MpeShow);
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_OwnerHistory", parms);
        if (ds.Tables.Count > 0)
        {
            grdHistory.DataSource = ds.Tables[0];
            grdHistory.DataBind();

            grdExportHistory.DataSource = ds.Tables[0];
            grdExportHistory.DataBind();
            if (_ExportHistory)
            {
                _ExportHistory = false;
                grdExportHistory.MasterTableView.ExportToExcel();
            }
        }

    }
    private void MpeShow()
    {
        btnSave.Enabled = true;
        mpe.Show();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (this.WorkflowPage.InvalidateAgreements)
        {
            if (InvalidateAgreements != null) InvalidateAgreements(this, new EventArgs());
            this.WorkflowPage.InvalidateAgreements = false;
        }
        if (Helper.IsModern())
        {
            btnSave.CssClass = "buttonBoxFocus";
        }
        if (QuestionsHaveChanged())
        {
            cpeQuestions.Collapsed = false;
        }
    }

    private void LoadOwnerIdentifyingInfo()
    {
        // OWNER
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER_PAPER_PROVIDER");
    }

    protected override void OnLoad(EventArgs e)
    {
        if ((this.WorkflowPage.HasActiveDODDSpecialty && this.WorkflowPage.WaiverServiceUpdateTypeID != 1)
             || (this.WorkflowPage.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)) // OHPNM-15326 - DD Non medicaid provider shoud not update Billing payment address in PNM
        {
            this.WorkflowPage.OwnerInfoPageIsReadOnly = true;
        }
        LoadControlData();
        base.OnLoad(e);
    }

    private int _currentPNMStatus;
    private int RegistrationStatusType;
    private int WaiverServiceUpdateTypeID;
    public override void LoadControlData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        if (_ExportHistory == true)
        {
            _ExportHistory = false;
            grdExportHistory.MasterTableView.ExportToExcel();
            mpeHistory.Hide();
        }

        var dsStatus = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        DataRow row = dsStatus.Tables[0].Rows[0];
        _currentPNMStatus = row.Field<int>("RegProgramStatusTypeID");
        RegistrationStatusType = row.Field<int>("RegistrationStatusTypeID");
        WaiverServiceUpdateTypeID = row.Field<int>("WaiverServiceUpdateTypeID");
        LoadOwnerInfo();
        LoadRealEstateOwnerInfo();
        LoadAdditionalDisclosureInfo();


        //LoadOwnerIdentifyingInfo();
        ucQ01.LoadData();
        ucQ02.LoadData();
        ucQ04.LoadData();
        ucQ05.LoadData();
        ucQ06.LoadData();
        ucQ07.LoadData();
        ucQ09.LoadData();
        ucQ13.LoadData();

        btnSave.Visible = btnAddOwnerInfo.Visible = true;
        btnCancel.Text = "Cancel";
        Helper.SetReadOnly(this, false);
        if (!Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            btnSave.Visible = btnAddOwnerInfo.Visible = false;
            btnCancel.Text = "Close";
            Helper.SetReadOnly(this, true);
        }

        if ((Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) ||
            Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name)))
        {
            if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
            {
                btnSave.Visible = btnAddOwnerInfo.Visible = false;
                btnCancel.Text = "Close";
                Helper.SetReadOnly(this, false);
            }
        }

        // OHPNM-9086 - if enrollment specialist is doing an operator update make sure add new owner button is visible
        if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) && (WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.OperatorUpdate || this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.WHEELCHAIR_VAN))
        {
            btnSave.Visible = btnAddOwnerInfo.Visible = true;
        }

        //if (pnlIdentInfo.Visible)
        //{

        //}
        //btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup +
        //        "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
        {
            cpeDefinitions.Collapsed = cpeInstructions.Collapsed = cpeRealEstate.Collapsed = true;
            cpeOwnInfo.Collapsed = cpeQuestions.Collapsed = false;
            sepInstructions.InnerText = sepInstructions.InnerText.Replace('-', '+');
            sepRealEstateInfo.InnerText = sepRealEstateInfo.InnerText.Replace('+', '-');
            sepOwnInfo.InnerText = sepOwnInfo.InnerText.Replace('+', '-');
            sepQuestions.InnerText = sepQuestions.InnerText.Replace('+', '-');
        }
        // OHPNM-8313 - if they have 490 DODD specialty and update isn't being initiated by DODD, don't let them change this info: Name, Billing Payment Address, Ownership
        if (this.WorkflowPage.OwnerInfoPageIsReadOnly)
        {
            // hide "Add" buttons
            btnAddOwnerInfo.Visible = imgAddRealEstate.Visible = imgAddAdditionalDisclosure.Visible = false;
            if (!Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name))
            {
                // user isn't in internal role, turn off edit/view button; internal roles users still need to see the data
                grdOwnerInfo.Columns[4].Visible = false;
                grdRealEstateOwnerInfo.Columns[3].Visible = false;
                grdAdditionalDisclosureInfo.Columns[2].Visible = false;
            }
            // turn off delete options
            grdOwnerInfo.Columns[5].Visible = false;
            grdRealEstateOwnerInfo.Columns[4].Visible = false;
            grdAdditionalDisclosureInfo.Columns[3].Visible = false;
        }
    }

    private void LoadOwnerInfo()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>
        {
            {"REG_ID", this.WorkflowPage.RegistrationId.ToString()}
        };
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectOwnerAddressInfo", parms);
        if (Helper.HasRows(ds))
        {
            DataTable dtOwners = ds.Tables[0];
            //OHPNM - 16050 - Owner Information page should not allowing a SAVE without any active owners
            isActiveOwnersCount = dtOwners.AsEnumerable()
                                          .Where(r => r.Field<DateTime>("END_DATE").ToString("MM/dd/yyyy") == "12/31/2299").Any();
            grdOwnerInfo.DataSource = this.WorkflowPage.RegistrationOwnersList = this.WorkflowPage.RegistrationOwnerAddressList = dtOwners;
        }
        else grdOwnerInfo.DataSource = this.WorkflowPage.RegistrationOwnersList = this.WorkflowPage.RegistrationOwnerAddressList = null;
        grdOwnerInfo.DataBind();
        // btnHistoryOwnerInfo.Visible = grdOwnerInfo.Rows.Count > 0;
        // Read-Only users do not have the ability to delete owners; DCM 11/1/2022 - this was changed from "5" to "4" on 12/24/2020 when it shouldn't have been; changing it back
        grdOwnerInfo.Columns[5].Visible = !Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly);

    }

    private void LoadRealEstateOwnerInfo()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>
        {
            {"REG_ID", this.WorkflowPage.RegistrationId.ToString()}
        };
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectRealEstateOwnerAddressInfo", parms);
        if (Helper.HasRows(ds))
        {
            DataTable dtOwners = ds.Tables[0];
            grdRealEstateOwnerInfo.DataSource = this.WorkflowPage.RealEstateOwnersList = dtOwners;
        }
        else grdRealEstateOwnerInfo.DataSource = this.WorkflowPage.RealEstateOwnersList = null;
        grdRealEstateOwnerInfo.DataBind();
        // ImgHistRealEstate.Visible = grdRealEstateOwnerInfo.Rows.Count > 0;
        // Read-Only users do not have the ability to delete owners
        grdRealEstateOwnerInfo.Columns[4].Visible = !Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly);
    }


    private void LoadAdditionalDisclosureInfo()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>
        {
            {"REG_ID", this.WorkflowPage.RegistrationId.ToString()}
        };
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectAdditionalDisclosureAddressInfo", parms);
        if (Helper.HasRows(ds))
        {
            DataTable dtOwners = ds.Tables[0];
            grdAdditionalDisclosureInfo.DataSource = this.WorkflowPage.AdditionalDisclosureList = dtOwners;
        }
        else grdAdditionalDisclosureInfo.DataSource = this.WorkflowPage.AdditionalDisclosureList = null;
        grdAdditionalDisclosureInfo.DataBind();
        // ImgHistRealEstate.Visible = grdRealEstateOwnerInfo.Rows.Count > 0;
        // Read-Only users do not have the ability to delete owners
        grdAdditionalDisclosureInfo.Columns[3].Visible = !Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly);
    }
    private bool QuestionsHaveChanged()
    {
        if (ucQ01.HasChanged()) return true;
        if (ucQ02.HasChanged()) return true;
        if (ucQ04.HasChanged()) return true;
        if (ucQ05.HasChanged()) return true;
        if (ucQ06.HasChanged()) return true;
        if (ucQ07.HasChanged()) return true;
        if (ucQ09.HasChanged()) return true;
        if (ucQ13.HasChanged()) return true;

        return false;
    }

    public override bool SaveData()
    {
        // OHPNM-13974 - if this page is read only, don't validate or save
        if (this.WorkflowPage.OwnerInfoPageIsReadOnly)
        {
            // this is a read only page; no need to validate or save
            return true;
        }

        try
        {
            bool hasChanged = false;

            if (QuestionsHaveChanged())
            {
                ucQ01.SaveData();
                ucQ02.SaveData();
                ucQ04.SaveData();
                ucQ05.SaveData();
                ucQ06.SaveData();
                ucQ07.SaveData();
                ucQ09.SaveData();
                ucQ13.SaveData();

                // Remove the question data when the answer is NO
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.RemoveQuestionData(this.WorkflowPage.RegistrationId);
                hasChanged = true;
            }
            if (hasChanged)
            {
                if (InvalidateAgreements != null) InvalidateAgreements(this, new EventArgs());
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    private decimal TotalGrid()
    {
        decimal rtn = 0;
        foreach (GridViewRow row in grdOwnerInfo.Rows)
        {
            try
            {
                rtn += Convert.ToDecimal(row.Cells[3].Text);
            }
            catch { }
        }
        return rtn;
    }

    public override void LoadData(DataRow row = null)
    {

    }

    public override bool ValidateData()
    {
        bool isGood = true;

        if (grdOwnerInfo.Rows.Count == 0) AddError("*Enter Owner Information", ref isGood);
        if (!isActiveOwnersCount)
        {
            AddError("*An Active Owner is Required", ref isGood);
        }

        //  if (TotalGrid() > 100) AddError("Owner Information percent total cannot exceed 100", ref isGood);

        ucQ01.ValidateData(ref isGood);
        ucQ02.ValidateData(ref isGood);
        ucQ04.ValidateData(ref isGood);
        ucQ05.ValidateData(ref isGood);
        ucQ06.ValidateData(ref isGood);
        ucQ07.ValidateData(ref isGood);
        ucQ09.ValidateData(ref isGood);
        ucQ13.ValidateData(ref isGood);

        //if (!GridDataValid())
        //    isGood = false;

        return isGood;
    }

    private bool GridDataValid()
    {
        bool isValid = true;
        int testInt = 0;
        decimal testDecimal = 0;
        DateTime testdate;
        decimal percentTotal = 0;
        string ownType;
        string name;
        string birthDate;
        string taxId;
        string percent;
        string address;
        string city;
        string state;
        string zip;
        string zipExt;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER");
        if (Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ownType = Helper.GetData("REG_OWNER_TYPE_ID", row);
                name = Helper.GetData("NAME", row);
                birthDate = Helper.GetData("DOB", row);
                taxId = Helper.GetData("TAX_ID", row);
                percent = Helper.GetData("PERCENTAGE_OF_OWNERSHIP", row);


                if (string.IsNullOrEmpty(ownType))
                    isValid = AddValidationErrorMessage("* Owner Type is required");

                if (string.IsNullOrEmpty(name))
                    isValid = AddValidationErrorMessage("* Name of Individual or Organization is required");

                DataRow[] rowOwn = psc.SelectRegistrationData(-1, "OWNER_TYPE").Tables[0].Select("OWNER_TYPE_NAME = 'Individual' OR OWNER_TYPE_NAME = 'Real Estate Individual' OR OWNER_TYPE_NAME = 'Subcontractor Individual' OR OWNER_TYPE_NAME = 'Supplier Individual' OR OWNER_TYPE_NAME = 'Employee'");
                string personTypeId = rowOwn[0][0].ToString();
                if (ownType.Equals(personTypeId) && string.IsNullOrEmpty(birthDate))
                    isValid = AddValidationErrorMessage("* Birth Date is required");//required if person, not if org
                else if (!string.IsNullOrEmpty(birthDate))
                {
                    if (!DateTime.TryParse(birthDate, out testdate))
                        isValid = AddValidationErrorMessage("* Birth Date is invalid");
                    else if (testdate > DateTime.Today)
                        isValid = AddValidationErrorMessage("* Birth Date cannot be future date");
                }
                if (!string.IsNullOrEmpty(birthDate))
                {
                    isValid = Validate_BirthDateDeathDate(Convert.ToDateTime(birthDate));
                }
                if (string.IsNullOrEmpty(taxId))
                    isValid = AddValidationErrorMessage("* SSN is required");
                else if (taxId.Length != 9 || !int.TryParse(taxId, out testInt))
                    isValid = AddValidationErrorMessage("* SSN is invalid");

                if (string.IsNullOrEmpty(percent))
                    isValid = AddValidationErrorMessage("* Percentage of Ownership is required");
                else if (!decimal.TryParse(percent, out testDecimal))
                    isValid = AddValidationErrorMessage("* Percentage of Ownership is invalid");
                else
                    percentTotal += testDecimal;
            }

            DataSet dsAddress = psc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.PrimaryPractice);
            if (Helper.HasRows(dsAddress))
            {
                foreach (DataRow row in dsAddress.Tables[0].Rows)
                {
                    address = Helper.GetData("ADDRESS1", row);
                    city = Helper.GetData("CITY", row);
                    state = Helper.GetData("STATE", row);
                    zip = Helper.GetData("ZIP", row);
                    zipExt = Helper.GetData("EXT_ZIP", row);

                    if (string.IsNullOrEmpty(address))
                        isValid = AddValidationErrorMessage("* Address is required");

                    if (string.IsNullOrEmpty(city))
                        isValid = AddValidationErrorMessage("* City is required");

                    if (string.IsNullOrEmpty(state))
                        isValid = AddValidationErrorMessage("* State is required");

                    if (string.IsNullOrEmpty(zip))
                        isValid = AddValidationErrorMessage("* Zip is required");
                    else if (!int.TryParse(zip, out testInt) || zip.Length != 5)
                        isValid = AddValidationErrorMessage("* Enter 5 digits for the Zip (First 5)");

                    if (!string.IsNullOrEmpty(zipExt))
                        if (zipExt.Length != 4 || !int.TryParse(zipExt, out testInt))
                            isValid = AddValidationErrorMessage("* Enter 4 digits for the Zip (Last 4)");
                }
            }
        }

        return isValid;
    }

    private bool Validate_BirthDateDeathDate(DateTime? birthDate)
    {
        bool isValid = true;

        if (birthDate.HasValue)
        {
            if (birthDate.Value < DateTime.Now.AddYears(-100) || birthDate.Value > DateTime.Now)
            {
                isValid = false;
                AddValidationErrorMessage("* Birth Date can not be a future date and cannot result in an age over 100 years.");
            }
        }
        return isValid;
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
        if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) ||
               Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
        {
            btnSave.Visible = true;
            btnCancel.Text = "Close";
            Helper.SetReadOnly(this, false);
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        DataRow dr;
        SessionVarRetriever.IsNewOwner = false;
        SetButtons(e.CommandName);
        switch (e.CommandName)
        {
            case "OwnerInfo":
                SessionVarRetriever.IsRealEstateOwner = false;
                SessionVarRetriever.IsAdditionalDisclosure = false;
                ucOwnerInfo.AffiliationTypeVisible = true;
                ucOwnerInfo.PercentageOfOwnershipVisible = true;
                ucOwnerInfo.OwnerTitleVisible = true;
                lblTitle.Text = "Owner Information";
                btnSave.ValidationGroup = "valOwnerInfo";
                dr = this.WorkflowPage.RegistrationOwnerAddressList.Rows[index];
                ucOwnerInfo.LoadData(dr);
                mltPopup.ActiveViewIndex = 0;
                cpeOwnInfo.Collapsed = false;
                mpe.Show();
                break;
            case "RealEstateOwnerInfo":
                SessionVarRetriever.IsRealEstateOwner = true;
                SessionVarRetriever.IsAdditionalDisclosure = false;
                lblTitle.Text = "Real Estate Information";
                btnSave.ValidationGroup = "valOwnerInfo";
                ucRealEstateInfo.PercentageOfOwnershipVisible = true;
                ucRealEstateInfo.SSNVisible = true;
                ucRealEstateInfo.DOBVisible = false;
                dr = this.WorkflowPage.RealEstateOwnersList.Rows[index];
                ucRealEstateInfo.LoadData(dr);
                mltPopup.ActiveViewIndex = 2;
                cpeRealEstate.Collapsed = false;
                mpe.Show();
                break;
            case "AdditionalDisclosureInfo":
                SessionVarRetriever.IsRealEstateOwner = false;
                SessionVarRetriever.IsAdditionalDisclosure = true;
                lblTitle.Text = "Additional Disclosure Information";
                btnSave.ValidationGroup = "valOwnerInfo";
                dr = this.WorkflowPage.AdditionalDisclosureList.Rows[index];
                ucAdditionalDisclosureInfo.LoadData(dr);
                mltPopup.ActiveViewIndex = 4;
                cpeAdditionalDisclosure.Collapsed = false;
                ucAdditionalDisclosureInfo.PercentageOfOwnershipVisible = false;
                mpe.Show();
                break;

            case "OwnerInfoDelete":
                dr = this.WorkflowPage.RegistrationOwnerAddressList.Rows[index];
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.DeleteRegistrationData("OWNER", "REG_OWNER_ID", Convert.ToInt32(Helper.GetString("REG_OWNER_ID", dr)));
                ucQ01.LoadData();
                ucQ02.LoadData();
                ucQ04.LoadData();
                LoadOwnerInfo();
                break;
            case "RealEstateOwnerInfoDelete":
                dr = this.WorkflowPage.RealEstateOwnersList.Rows[index];
                PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
                psc1.DeleteRegistrationData("OWNER", "REG_OWNER_ID", Convert.ToInt32(Helper.GetString("REG_OWNER_ID", dr)));
                ucQ01.LoadData();
                ucQ02.LoadData();
                ucQ04.LoadData();
                LoadRealEstateOwnerInfo();
                break;

            default:
                break;
        }
    }

    protected int GetColumnIndexByName(GridViewRow row, string columnName)
    {
        int columnIndex = 0;
        foreach (DataControlFieldCell cell in row.Cells)
        {
            if (cell.ContainingField is BoundField)
                if (((BoundField)cell.ContainingField).DataField.Equals(columnName))
                    break;
            columnIndex++; // keep adding 1 while we don't have the correct name
        }
        return columnIndex;
    }

    protected bool ShowEditButton(GridViewRowEventArgs e)
    {
        if (RegistrationStatusType == CON.RegistrationStatusTypeId.Pending || RegistrationStatusType == CON.RegistrationStatusTypeId.ReturnToProvider)
        {
            return true;
        }

        return false;
    }


    protected bool ShowDeleteButton(GridViewRowEventArgs e)
    {
        var showDeleteButton = false;
        var index = GetColumnIndexByName(e.Row, "MODIFIED_STATUS_TYPE_ID");
        //var scrindex = GetColumnIndexByName(e.Row, "IS_SCREENED");
        var modifiedStatus = !string.IsNullOrEmpty(e.Row.Cells[index - 1].Text) ? Convert.ToInt32(e.Row.Cells[index - 1].Text) : 0;
        var isscr = DataBinder.Eval(e.Row.DataItem, "IS_SCREENED").ToString();
        if (isscr != "Y" && (RegistrationStatusType == CON.RegistrationStatusTypeId.Pending || RegistrationStatusType == CON.RegistrationStatusTypeId.ReturnToProvider))
        {
            return true;
        }

        return showDeleteButton;
    }

    protected void grdOwnerInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton imgEdit = (ImageButton)e.Row.FindControl("btnEdit");
            if (imgEdit != null)
            {
                if (
                 Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name)
                 || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator)
                 || Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, this.WorkflowPage.RegistrationId)
                 || (
                 Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)
                 && Helper.IsUserInSubRoles(Convert.ToInt32(this.WorkflowPage.RegistrationId.ToString()), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollmentAgentSubRole))
                 )
                {
                    imgEdit.Visible = true; // ShowEditButton(e);
                }
                else
                    imgEdit.Visible = false;
            }

            ImageButton img = (ImageButton)e.Row.FindControl("btnDelete");
            if (img != null)
            {
                //img.Visible = ShowDeleteButton(e);
                if (!ShowDeleteButton(e))
                {
                    if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name))
                    {
                        img.Visible = true;
                    }
                    else
                    {
                        img.Visible = false;
                    }
                    if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) ||
                                 Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
                    {
                        img.Visible = false;
                    }

                    return;
                }

                if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName)) img.Visible = false;
                else img.Attributes.Add("onclick", "javascript:return confirm('Ownership will be deleted. Are you sure?');");
            }
        }
    }

    protected void grdRealEstateOwnerInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ImageButton imgEdit = (ImageButton)e.Row.FindControl("btnEditRealEstate");
        if (imgEdit != null)
        {

            if (
                Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name)
                || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator)
                || Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, this.WorkflowPage.RegistrationId)
                || (
                Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)
                && Helper.IsUserInSubRoles(Convert.ToInt32(this.WorkflowPage.RegistrationId.ToString()), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollmentAgentSubRole))
                )
            {
                imgEdit.Visible = true;// ShowEditButton(e);
            }
            else
                imgEdit.Visible = false;
        }

        ImageButton img = (ImageButton)e.Row.FindControl("btnDeleteRealEstate");
        if (img != null)
        {
            if (!ShowDeleteButton(e))
            {
                img.Visible = false;
                return;
            }

            if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName)) img.Visible = false;
            else img.Attributes.Add("onclick", "javascript:return confirm('Real Estate Owner will be deleted. Are you sure?');");
        }
    }
    protected void grdAdditionalDisclosureInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ImageButton imgEdit = (ImageButton)e.Row.FindControl("btnEditAdditionalDisclosure");
        if (imgEdit != null)
        {
            imgEdit.Visible = true; // ShowEditButton(e);
        }

        ImageButton img = (ImageButton)e.Row.FindControl("btnDelete");
        if (img != null)
        {
            if (!ShowDeleteButton(e))
            {
                img.Visible = false;
                return;
            }

            if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName)) img.Visible = false;
            else img.Attributes.Add("onclick", "javascript:return confirm('Additional Disclosure Owner will be deleted. Are you sure?');");
        }
    }

    protected void grdAddiAddress_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        SetButtons(e.CommandName);

        SessionVarRetriever.IsNewOwner = true;
        switch (e.CommandName)
        {
            case "OwnerInfo":
                lblTitle.Text = "Owner Information";
                btnSave.ValidationGroup = "valOwnerInfo";
                SessionVarRetriever.IsRealEstateOwner = false;
                SessionVarRetriever.IsAdditionalDisclosure = false;
                ucOwnerInfo.AffiliationTypeVisible = true;
                ucOwnerInfo.PercentageOfOwnershipVisible = true;
                ucOwnerInfo.OwnerTitleVisible = true;
                ucOwnerInfo.LoadData(null);
                mltPopup.ActiveViewIndex = 0;
                cpeOwnInfo.Collapsed = true;
                mpe.Show();
                break;
            case "RealEstateOwnerInfo":
                lblTitle.Text = "Real Estate Owner Information";
                btnSave.ValidationGroup = "valOwnerInfo";
                SessionVarRetriever.IsRealEstateOwner = true;
                SessionVarRetriever.IsAdditionalDisclosure = false;
                ucRealEstateInfo.PercentageOfOwnershipVisible = true;
                ucRealEstateInfo.SSNVisible = true;
                ucRealEstateInfo.DOBVisible = false;
                ucRealEstateInfo.LoadData(null);
                mltPopup.ActiveViewIndex = 2;
                //cpeOwnInfo.Collapsed = false;
                cpeRealEstate.Collapsed = true;
                mpe.Show();
                break;
            case "AdditionalDisclosureInfo":
                lblTitle.Text = "Additional Disclosure";
                btnSave.ValidationGroup = "valOwnerInfo";
                SessionVarRetriever.IsAdditionalDisclosure = true;
                SessionVarRetriever.IsRealEstateOwner = false;
                ucAdditionalDisclosureInfo.PercentageOfOwnershipVisible = false;
                ucAdditionalDisclosureInfo.LoadData(null);
                mltPopup.ActiveViewIndex = 4;
                cpeAdditionalDisclosure.Collapsed = true;
                mpe.Show();
                break;
            case "OwnerIdentifyingInfo":
                lblTitle.Text = "Provider Identifying Information";
                btnSave.ValidationGroup = "valOwnerIdentifyingInfo";
                ucOwnerIdentifyingInfo.LoadData(null);
                mltPopup.ActiveViewIndex = 3;
                mpe.Show();
                break;
        }

    }

    protected void btnHistoryAddiAddress_Click(object sender, CommandEventArgs e)
    {

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            mpe.Show();
            return;
        }

        if (mltPopup.ActiveViewIndex == 0)
        {
            if (!ucOwnerInfo.ValidateData())
            {
                btnSave.Enabled = true;
                mpe.Show();
                return;
            }
            ucOwnerInfo.SaveData();
            InvalidateAgreements(this, new EventArgs());
            LoadOwnerInfo();
        }
        if (mltPopup.ActiveViewIndex == 2)
        {
            if (!ucRealEstateInfo.ValidateData())
            {
                btnSave.Enabled = true;
                mpe.Show();
                return;
            }
            ucRealEstateInfo.SaveData();
            InvalidateAgreements(this, new EventArgs());
            LoadRealEstateOwnerInfo();
        }

        if (mltPopup.ActiveViewIndex == 3)
        {
            if (!ucOwnerIdentifyingInfo.ValidateData())
            {
                btnSave.Enabled = true;
                mpe.Show();
                return;
            }
            ucOwnerIdentifyingInfo.SaveData();
            InvalidateAgreements(this, new EventArgs());
            LoadOwnerIdentifyingInfo();
        }
        if (mltPopup.ActiveViewIndex == 4)
        {
            if (!ucAdditionalDisclosureInfo.ValidateData())
            {
                btnSave.Enabled = true;
                mpe.Show();
                return;
            }
            ucAdditionalDisclosureInfo.SaveData();
            InvalidateAgreements(this, new EventArgs());
            LoadAdditionalDisclosureInfo();
        }
        btnSave.Enabled = true;
    }

    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        return false;
    }
    private void SetVisibility()
    {
        if (EntityTypeId == 0)
        {
            int diddReferralId = 0, entityTypeId = 0, providerTypeId = 0, specialtyTypeID = 0;
            string taxID = string.Empty;
            int taxIDTypeID = 0;
            Registration.SetEntityProviderTypesDIDD(this.WorkflowPage.RegistrationId, ref entityTypeId, ref providerTypeId, ref diddReferralId, ref specialtyTypeID, ref taxID, ref taxIDTypeID);
            EntityTypeId = entityTypeId;
            ProviderTypeId = providerTypeId;
            DIDDReferralId = diddReferralId;
            SpecialtyTypeID = specialtyTypeID;
        }

    }

    public override string ValidationGroup
    {
        get { return "valOwnerInfo"; }
    }

    public override string Title
    {
        get { return "Owner Information"; }
    }

    public override string IdText
    {
        get { return "ucOwnerInformation_" + this.WorkflowPage.RegistrationId; }
    }

    protected void grdHistory_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (_SortField.Equals(e.SortExpression))
        {
            _SortField = _SortField + " DESC";
        }
        else
        {
            _SortField = e.SortExpression;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_OwnerHistory", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdHistory.DataSource = ds.Tables[0];
            grdHistory.DataBind();
            mpeHistory.Show();
        }
    }

    protected void grdHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_OwnerHistory", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdHistory.DataSource = ds.Tables[0];
            grdHistory.DataBind();
            mpeHistory.Show();
        }
    }

    protected void grdHistory_Export(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        mpeHistory.Show();
    }
}