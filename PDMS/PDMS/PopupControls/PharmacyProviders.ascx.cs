using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_PharmacyProviders : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public string _SortField
    {
        get
        {
            return (string)ViewState["SortField"] ?? "DateOfAction"; // default sort 
        }
        set
        {
            ViewState["SortField"] = value;
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

    public int RegVisionProiverID
    {
        get
        {
            return ViewState["RegVisionProiverID"] == null ? 0 : Convert.ToInt32(ViewState["REG_VISION_PROVIDER_DETAILS_ID"]);
        }
        set
        {
            ViewState["RegVisionProiverID"] = value;
        }
    }

    public int RegPharmacyProviderID
    {
        get { return ViewState["RegPharmacyProviderID"] != null ? (int)ViewState["RegPharmacyProviderID"] : 0; }
        set { ViewState["RegPharmacyProviderID"] = value; }
    }

    #endregion

    #region Parent Page Events

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #endregion


    #region Public Methods
    public override void LoadData(DataRow row)
    {
        bool isEdit = false;

        if (row == null)
            isEdit = false;
        else
            isEdit = true;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PHARMACY_PROVIDER_INFO");

        if (Helper.HasRows(ds))
        {
            if (ds.Tables[0].Rows.Count >= 1)
            {
                DataRow datarow = ds.Tables[0].Rows[0];
                this.RegPharmacyProviderID = Convert.ToInt32(datarow["REG_PHARMACY_PROVIDER_ID"]);
                txtPharmacyName.Text = datarow["PHARMACY_NAME"].ToString();
                txtChiefPharmacist.Text = datarow["CHIEF_PHARMACIST_NAME"].ToString();
                txtOccupancyPermitNumber.Text = datarow["OCCUPANCY_PERMIT_NUMBER"].ToString();

                LoadPlaceHolder(Helper.GetInt("REG_PHARMACY_PROVIDER_ID", datarow), isEdit);
            }
        }
        else
            LoadPlaceHolder(0, false);

        hidIsEdit.Text = isEdit.ToString();

        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }

    public override bool SaveData()
    {
        int licenseId = 0;
        bool rtn = true;
        if (rblAddPharmacy.SelectedValue == "0")
        {
            return rtn;
        }
        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
        Page.Validate("valPharmacyProvider");


        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return false;
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

        if (isEdit)
            parms.Add("REG_PHARMACY_PROVIDER_ID", this.RegPharmacyProviderID.ToString());

        if (!string.IsNullOrWhiteSpace(txtPharmacyName.Text.ToString()))
        {
            parms.Add("PHARMACY_NAME", txtPharmacyName.Text);
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        if (!string.IsNullOrWhiteSpace(txtChiefPharmacist.Text.ToString()))
        {
            parms.Add("CHIEF_PHARMACIST_NAME", txtChiefPharmacist.Text);
        }

        if (!string.IsNullOrWhiteSpace(txtOccupancyPermitNumber.Text.ToString()))
        {
            parms.Add("OCCUPANCY_PERMIT_NUMBER", txtOccupancyPermitNumber.Text);
        }

        if (!isEdit)
            licenseId = svc.InsertRegistrationData(Convert.ToInt32(this.WorkflowPage.RegistrationId), "PHARMACY_PROVIDER_INFO", parms);
        else
        {
            svc.UpdateRegistrationDataTable("PHARMACY_PROVIDER_INFO", parms);
            licenseId = this.RegPharmacyProviderID;
        }

        foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderPharmacyProvider.Controls)
        {
            svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, licenseId);
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

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet dsHistory = new DataSet();
        dsHistory = psc.SelectRegistrationDataWithParams("usp_SelectREG_PHARMACIST_INFO_HISTORY", parms);

        if (Helper.HasRows(dsHistory))
        {
            dsHistory.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = dsHistory.Tables[0];
            grd.DataBind();
        }
    }
    public override void LoadControlData()
    {
        LoadPharmacyProviders();
    }
    private void LoadPharmacyProviders()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PHARMACY_PROVIDER_INFO");
        DataTable dtPharmacyProviders = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtPharmacyProviders;
        if (Helper.HasRows(dtPharmacyProviders))
        {
            rblAddPharmacy.SelectedValue = "1";
            divPharmacyProviders.Visible = true;

            this.LoadData(dtPharmacyProviders.Rows[0]);
        }
            
        else
        {
            rblAddPharmacy.SelectedValue = "0";
            divPharmacyProviders.Visible = false;
            this.LoadData(null);
        }
    }


    #endregion
    public override bool HasInputValue()
    {
        return true;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
    private const string sectionName = "Pharmacy";

    public void LoadPlaceHolder(int licensureId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderPharmacyProvider.Controls.Clear();
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

                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                PlaceholderPharmacyProvider.Controls.Add(ucUploadSectionControl);
            }
        }
    }
    
    public override string ValidationGroup
    {
        get { return "valPharmacyProviders"; }
    }

    public override string Title
    {
        get { return "Pharmacy Providers"; }
    }

    public override string IdText
    {
        get { return "ucPharmacyProviders_" + this.WorkflowPage.RegistrationId; }
    }
    protected void rblAddPharmacy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblAddPharmacy.SelectedValue == "1")
        {
            divPharmacyProviders.Visible = true;
        }
        else
        {
            divPharmacyProviders.Visible = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.CancelPopup();
    }

    private void CancelPopup()
    {
        this.mpeHistory.Hide();
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet dsHistory = new DataSet();
        dsHistory = psc.SelectRegistrationDataWithParams("usp_SelectREG_PHARMACIST_INFO_HISTORY", parms);

        if (Helper.HasRows(dsHistory))
        {
            dsHistory.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = dsHistory.Tables[0];
            grd.DataBind();
        }
        mpeHistory.Show();
    }

    protected void lnkHistoryExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_PHARMACIST_INFO_HISTORY", parms);
            if (Helper.HasRows(ds))
            {
                grdHistoryExport.DataSource = ds.Tables[0];
                grdHistoryExport.DataBind();
                grdHistoryExport.MasterTableView.ExportToExcel();
            }
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
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
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_PHARMACIST_INFO_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
            mpeHistory.Show();
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_PHARMACIST_INFO_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.DataSource = ds.Tables[0];
            grd.PageIndex = e.NewPageIndex;
            grd.DataBind();
            mpeHistory.Show();
        }
    }

}