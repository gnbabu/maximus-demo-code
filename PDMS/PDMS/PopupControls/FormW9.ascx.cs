using MAXIMUS.Core.Libraries;
using Models.Data;
using Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using RadioButton = System.Web.UI.WebControls.RadioButton;

public partial class PopupControls_FormW9 : BaseSectionControl, IFormW9View
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

    #region Tax Classes
    DataTable _dtTaxClass;
    public TaxInfo Model { get; set; }

    public DataTable dtTaxClass
    {
	    get
	    {
		    if (_dtTaxClass == null)
		    {
			    DataSet ds = svc.SelectTaxEntityTypes();
                _dtTaxClass = Helper.HasRows(ds) ? ds.Tables[0] : null;
		    }

		    return _dtTaxClass;
	    }
    }

	private DataSet _dsTaxInfo { get; set; }
	private int _taxInfoIndex = -1;
	private TaxInfo _taxInfo = new TaxInfo();
	private bool _isEdit = false;
	private Dictionary<string, string> _parms = new Dictionary<string, string>();
    private const string sectionName = "W-9";
    private int taxinfoID = 0;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }
    }

    public string SaveButtonClientID {get; set; }


    private void GetTaxInfo()
    {
        if (_dsTaxInfo == null)
        {
            _dsTaxInfo = svc.SelectTaxInfo(this.WorkflowPage.RegistrationId);
            if (_dsTaxInfo != null && Helper.HasRows(_dsTaxInfo))
            {
                _taxInfoIndex = Convert.ToInt32(_dsTaxInfo.Tables[0].Rows[0].ItemArray[2]);
                if (hidTaxInfo.Text == string.Empty)
                    hidTaxInfo.Text = _taxInfoIndex.ToString();
            }
        }
    }


    protected override void OnLoad(EventArgs e)
    {
        //if (ddlState.Items.Count == 0)
        //{
        //    Helper.LoadDropDownListWithStates(ref ddlState, true);
        //    ddlState.SelectedValue = AppSettings.Get("StateCode");
        //}
        GetTaxInfo();
	    GetProviderInfo();
        LoadControlData();
		//LoadData(null);
		
		base.OnLoad(e);
        Page.Title="W9 Form";
    }

    private void GetProviderInfo()
    {
	    DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
	    if (ds != null && Helper.HasRows(ds))
	    {
		    var taxTypeId = Helper.GetInt("TAX_ID_TYPE_ID", ds.Tables[0].Rows[0]);
		    var taxId = Helper.GetString("TAX_ID", ds.Tables[0].Rows[0]).ToString();
		    if (taxTypeId == CON.TaxIDType.SSN)
		    {
			    divIndividual.Visible = true;
			    divOrganization.Visible = false;
			    txtSSN.Text = taxId;
			    lblOrgName.Text = "Individual Name:";

		    }
			else
		    {
			    divOrganization.Visible = true;
                divIndividual.Visible = false;
                txtEIN.Text = taxId;
                lblOrgName.Text = "Legal Business Name:";
		    }

		    txtOrgName.Text = Helper.GetString("NAME", ds.Tables[0].Rows[0]);
	    }
    }


    protected void rptTaxClassification_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RadioButton rbTaxIndex = (RadioButton)e.Item.FindControl("rbTaxClass");
            Label lbTaxID = (Label)e.Item.FindControl("lbTaxId");
            rbTaxIndex.Checked = Convert.ToInt32(lbTaxID.Text) == _taxInfoIndex;
        }
    }

    public override void LoadControlData()
    {
	    if (dtTaxClass != null & Helper.HasRows(dtTaxClass))
	    {
		    rptTaxClassification.DataSource = dtTaxClass;
			rptTaxClassification.DataBind();
	    }
        LoadData();
    }

    public override void LoadData(DataRow dr = null)
    {
	    var ds = svc.SelectTaxInfo(WorkflowPage.RegistrationId);
	    if (ds != null && Helper.HasRows(ds))
	    {
		    if (dr == null)
		    {
			    dr = ds.Tables[0].Rows[0];
		    }
	    }

	    hidIsEdit.Text = _isEdit.ToString();
	    _taxInfo.Load(dr);
        if(!String.IsNullOrEmpty(_taxInfo.INDICATEFORM) )
        {
            switch(_taxInfo.INDICATEFORM)
            {
                case "W9":
                    rbIndicate.Items[0].Selected = true;
                    break;
                case "Form 147":
                    rbIndicate.Items[1].Selected = true;
                    break;
            }
        }
        // ddlState.SelectedValue = _taxInfo.State;
        _isEdit = dr != null;
		if (dr != null)
			hidID.Text = Helper.GetInt("REG_TAX_INFO_ID", dr).ToString();
        taxinfoID = dr != null ? Helper.GetInt("REG_TAX_INFO_ID", dr) : 0;
        LoadPlaceHolder(taxinfoID);
		
        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }

    public void LoadPlaceHolder(int taxinfoID = 0, bool isEdit = true, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadW9.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, taxinfoID, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, taxinfoID, pageSection, isEdit);

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
                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                ucUploadSectionControl.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);

                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.SectionName = sectionName;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                PlaceholderUploadW9.Controls.Add(ucUploadSectionControl);
            }
            
        }

    }

    public override bool SaveData()
    {
	    if (!ValidateData())
		    return false;

	    if (Model == null)
			Model = new TaxInfo();

		_taxInfo.RegId = this.WorkflowPage.RegistrationId;
        //if (rbIndicate.SelectedValue == "0")
        if(rbIndicate.SelectedItem != null)
            _taxInfo.INDICATEFORM = rbIndicate.SelectedItem.Text;

	//	_taxInfo.State = ddlState.SelectedValue;
        Model.CopyPropertiesFrom(_taxInfo);
	    _parms = _taxInfo.CreateParameterList(_taxInfo);

        if (_taxInfo.RegTaxFormId > 0)
			Model.Update(Model, _parms);
		else
            _taxInfo.RegTaxFormId = Model.Insert(Model, _parms);

        foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadW9.Controls)
        {
            svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, _taxInfo.RegTaxFormId);
        }

        return true;
    }

    protected void UncheckAll()
    {
	    foreach (RepeaterItem item in this.rptTaxClassification.Items)
	    {
		    RadioButton rbTaxClass = (RadioButton) item.FindControl("rbTaxClass");
		    rbTaxClass.Checked = false;
	    }
    }

    protected void rbTaxClass_OnCheckedChanged(object sender, EventArgs e)
    {
        if (hidTaxInfo.Text == string.Empty)
        {
            hidTaxInfo.Text = _taxInfo.TaxFormId.ToString();
        }

        foreach (RepeaterItem item in this.rptTaxClassification.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                RadioButton rbTaxClass = (RadioButton)item.FindControl("rbTaxClass");
                Label lbTaxID = (Label)item.FindControl("lbTaxId");

                if (rbTaxClass.Checked && Convert.ToInt32(lbTaxID.Text) != Convert.ToInt32(hidTaxInfo.Text))
                {
                    hidTaxInfo.Text = lbTaxID.Text;
                    break;
                }
            }
        }
        UncheckAll();
        foreach (RepeaterItem item in this.rptTaxClassification.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbTaxID = (Label)item.FindControl("lbTaxId");
                if (Convert.ToInt32(lbTaxID.Text) == Convert.ToInt32(hidTaxInfo.Text))
                {
                    RadioButton rbTaxClass = (RadioButton)item.FindControl("rbTaxClass");
                    rbTaxClass.Checked = true;
                    _taxInfo.TaxFormId = Convert.ToInt32(hidTaxInfo.Text);
                }
            }
        }
    }

    public override bool ValidateData()
    {
        bool isValid = true;

     //   if (_taxInfo.TaxFormId == 0)
	    //{
		   // CustomValidator val = new CustomValidator();
		   // val.IsValid = false;
		   // val.ErrorMessage = "Please select the appropriate Tax Classification.";
		   // val.ValidationGroup = "vsFormW9";
		   // this.Page.Validators.Add(val);
     //       isValid = false;
	    //}

        //if (string.IsNullOrEmpty(ddlState.SelectedValue))
        //{
        //    CustomValidator val = new CustomValidator();
        //    val.IsValid = false;
        //    val.ErrorMessage = "Please select the State where this form is Registered.";
        //    val.ValidationGroup = "vsFormW9";
        //    this.Page.Validators.Add(val);
        //    return false;
        //}

        foreach (Control ctrl in PlaceholderUploadW9.Controls)
        {

            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            if (uploadControl.IsRequired)
            {
                isValid &= uploadControl.ValidateData("vsFormW9");
            }
        }

        return isValid;
    }


    public override string Title {
	    get { return "Form W9"; }
    }

    public override string IdText
    {
	    get { return "ucTaxInfo_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
	    get { return "vsFormW9"; }
    }

    //protected void ddlState_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    return;
    //}
}