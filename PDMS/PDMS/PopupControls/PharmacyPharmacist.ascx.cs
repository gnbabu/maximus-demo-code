using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_PharmacyPharmacist : BaseSectionControl
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

    public int RegID
    {
        get
        {
            return ViewState["RegID"] == null ? 0 : Convert.ToInt32(ViewState["RegID"]);
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }

    public int RegPharmacistID 
    {
        get { return ViewState["RegPharmacistID"] != null ? (int)ViewState["RegPharmacistID"] : 0; }
        set { ViewState["RegPharmacistID"] = value; }
    }

    #endregion

    #region Parent Page Events

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #region Public Methods
    public bool CanUserViewDelete()
    {

        return Registration.CanUserViewDelete(this.WorkflowPage.RegistrationId,this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
    }
    public override void LoadData(DataRow row)
    {
        bool isEdit = false;

        if (row == null)
            isEdit = false;
        else
            isEdit = true;

        int licensureId = isEdit ? Helper.GetInt("REG_PHARMACIST_ID", row) : 0;
        Helper.LoadDropDownListWithStates(ref ddlState);
        LoadPlaceHolder(licensureId, isEdit);

        bool isPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);


        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            txtPHARMACIST_NAME.Enabled =
                ddlState.Enabled =
            txtLICENSENUMBER.Enabled = true;
        }
        else
        {
            txtPHARMACIST_NAME.Enabled =
                   ddlState.Enabled =
               txtLICENSENUMBER.Enabled = false;
        }
       
        //ParentTable.Rows[0].Cells[2].Style["display"] = Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) ? "block" : "none";
        hidIsEdit.Text = isEdit.ToString();

        if (isEdit)
        {
            hidID.Text = row["REG_PHARMACIST_ID"].ToString();
            this.RegPharmacistID = Convert.ToInt32(row["REG_PHARMACIST_ID"]);
            txtPHARMACIST_NAME.Text = row["PHARMACIST_NAME"].ToString();
            txtLICENSENUMBER.Text = row["LICENSE_NUMBER"].ToString();
            string i = row["LICENSE_STATE"].ToString();
            ddlState.SelectedValue = i;
        }
        else
        {
            hidID.Text = string.Empty;
            //this.RegPharmacistID = Convert.ToInt32(row["REG_PHARMACIST_ID"]);
            txtPHARMACIST_NAME.Text = String.Empty;
            txtLICENSENUMBER.Text = String.Empty;
            ddlState.SelectedIndex = 0;
        }


        hidIsEdit.Text = isEdit.ToString();


    }
    private bool ValidatePharmacistInfo(string federalDEANumber, string reg_Pharmacist_info_id)
    {
        bool isGood = true;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("License_number", federalDEANumber);
        parms.Add("reg_Pharmacist_info_id", reg_Pharmacist_info_id);
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_PHARMACIST_INFOByLicenseIDRegID", parms);
        if (ds != null && Helper.HasRows(ds))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* License number already added.";
            val.ValidationGroup = "valPharmacist";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        return isGood;
    }
    public override bool HasInputValue()
    {
        return true;
    }
    public override bool SaveData()
    {
        int licenseId = 0;
        bool rtn = true;
        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
        bool isGood = true;

        Page.Validate("valPharmacist");

        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return false;
        }
        if (!string.IsNullOrEmpty(txtPHARMACIST_NAME.Text))
        {
            if (!isEdit)
            {
                isGood = ValidatePharmacistInfo(txtLICENSENUMBER.Text, "0");
            }
            else
            {
                isGood = ValidatePharmacistInfo(txtLICENSENUMBER.Text, this.RegPharmacistID.ToString());
            }
            if (!isGood)
                return false;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

            if (isEdit)
                parms.Add("REG_PHARMACIST_ID", this.RegPharmacistID.ToString());

            if (!string.IsNullOrWhiteSpace(txtPHARMACIST_NAME.Text.ToString()))
            {

                parms.Add("PHARMACIST_NAME", txtPHARMACIST_NAME.Text);
            }

            if (!string.IsNullOrWhiteSpace(txtLICENSENUMBER.Text.ToString()))
            {

                parms.Add("LICENSE_NUMBER", txtLICENSENUMBER.Text);
            }
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            if (!string.IsNullOrWhiteSpace(ddlState.SelectedValue.ToString()))
            {
                parms.Add("LICENSE_STATE", ddlState.SelectedValue);
            }

            if (!isEdit)
                licenseId = svc.InsertRegistrationData(Convert.ToInt32(this.WorkflowPage.RegistrationId), "PHARMACIST_INFO", parms);
            else
            {
                svc.UpdateRegistrationDataTable("PHARMACIST_INFO", parms);
                licenseId = this.RegPharmacistID;
            }


            //licenseId = Int32.Parse(txtLICENSENUMBER.Text);

            foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadPharmacist.Controls)
            {
                svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, licenseId);
            }
            //presenter.SaveRegVisionProviders(vProviders, false);
        }
        return rtn;
    }

    public override bool ValidateData()
    {
        return true;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadPharmacists();
    }


    private void LoadPharmacists()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PHARMACIST_INFO");
        DataTable dtPharmacists = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtPharmacists;
        rgPharmacist.DataSource = ds;

        rgPharmacist.DataBind();
    }


    protected void rgPharmacist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        DataRow dr;

        pharmacistDetail.Visible = true;
        dr = this.DataList.Rows[e.Item.ItemIndex];
        //lblTitle.Text = "Edit Pharmacist";
        //btnSave.ValidationGroup = "valPharmacist";        
        if (e.CommandName == "DeletePharmacistRow")
        {
            bool IsDeleted = DeletePharmacist(dr);
            if (IsDeleted) LoadPharmacists();
        }
        else
        {
            this.LoadData(dr);
        }
       
    }

    protected void btnSavePharmacist_Click(object sender, EventArgs e)
    {
        //ucPharmacyPharmacist.SaveData();
        //LoadData(Step);
    }
    #endregion

    private const string sectionName = "Pharmacist";

    public void LoadPlaceHolder(int licensureId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadPharmacist.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, licensureId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, licensureId, pageSection, isEdit);

        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                //IDWithFile.Add(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr));
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);

                ucUploadSectionControl.DestinationPath = @"C:\project\temp";

                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);

                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;
                ucUploadSectionControl.SectionName = sectionName;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                PlaceholderUploadPharmacist.Controls.Add(ucUploadSectionControl);
            }
        }

    }

    protected void cv1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (txtLICENSENUMBER.Text.Length > 10)
        {
            cv1.ErrorMessage = " License Number can't be more than 10 characters.";
            args.IsValid = false;
        }
    }

    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        pharmacistDetail.Visible = true;
        this.LoadData(null);
    }

    protected void rgPharmacyProviders_ItemCommand(object sender, GridCommandEventArgs e)
    {

        int index = Convert.ToInt32(e.CommandArgument);
        DataRow dr;

        dr = this.DataList.Rows[e.Item.ItemIndex];
        //this.Title = "Edit Pharmacy Provider";

        this.LoadData(dr);
        //mpe.Show();

        if (e.CommandName == RadGrid.EditCommandName)
        {
            //rgPharmacyProviders.MasterTableView.IsItemInserted = false;    //Close Insert Form When EditMode Is Open
            /*GridDataItem Item = (GridDataItem)e.Item;*/
            /*GridEditableItem editItem = (GridEditableItem)e.Item;
            TextBox txtSTATE_CDS_Number = (TextBox)editItem.FindControl("txtSTATE_CDS_Number");
            DropDownList ddlState = (DropDownList)editItem.FindControl("ddlState");
            TextBox txtDateIssued = (TextBox)editItem.FindControl("txtDateIssued");
            TextBox txtExpirationDate = (TextBox)editItem.FindControl("txtExpirationDate");
            txtSTATE_CDS_Number.Text = Item["STATE_CDS_Number_ID"].ToString();
            ddlState.SelectedValue = Item["State_ID"].ToString();
            txtDateIssued.Text = Item["DateIssued_ID"].ToString();
            txtExpirationDate.Text = Item["ExpirationDate_ID"].ToString();*/

        }
        if (e.CommandName == RadGrid.InitInsertCommandName)
        {
            //rgPharmacyProviders.MasterTableView.ClearEditItems();         //Close EditMode Form When Insert Mode Is Open
            /*e.Canceled = true;

            System.Collections.Specialized.ListDictionary newValues = new System.Collections.Specialized.ListDictionary();
            newValues["STATE_CDS_Number"] = " ";
            newValues["State"] = -1;
            newValues["DateIssued"] = " ";
            newValues["ExpirationDate"] = " ";


            e.Item.OwnerTableView.InsertItem(newValues);*/

        }
       
    }


    public override string ValidationGroup
    {
        get { return "valPharmacyProvider"; }
    }

    public override string Title
    {
        get { return "Pharmacy Providers"; }
    }

    public override string IdText
    {
        get { return "ucPharmacyPharmacist_" + this.WorkflowPage.RegistrationId; }
    }

    private bool DeletePharmacist(DataRow dr)
    {
        bool isValid = true;
        if (this.DataList.Rows.Count == 1 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId,CON.RegistrationPageName.Certification, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.PharmacyProviders)))
        //if only one record and required page then prevent deletion or else the page will not turn back to blue once it is green and will cause issues for page submission
        {
            AddError("* This is required section. You are required to have at least one pharmacist.", ref isValid, ValidationGroup);
            return isValid;
        }
        if (dr != null)
        {
            int REG_PHARMACIST_ID = Helper.GetInt("REG_PHARMACIST_ID", dr);
            int PHARMACIST_ID = Helper.GetInt("PHARMACIST_INFO_ID", dr);
            if (PHARMACIST_ID == 0)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.DeleteRegistrationData("PHARMACIST_INFO", "REG_PHARMACIST_ID", REG_PHARMACIST_ID);
            }
            else
            {
                AddError("* Pharmacist cannot be deleted.", ref isValid, ValidationGroup);
                return isValid;
            }
        }
        return isValid;
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