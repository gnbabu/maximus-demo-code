using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
public partial class PopupControls_CDS : BaseSectionControl
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

    private const string sectionName = "StateCDSNumber";
    public DataTable dataTable;
    private void SetDt()
    {
        if (dataTable == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "STATE_CDS_NUMBER");
            dataTable = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable statesTable = null;
        if (!IsPostBack)
        {
            statesTable = ApplicationCache.SelectCDSRequireStates();
            if (statesTable != null)
            {
                Helper.LoadDropDown(ddlState, statesTable, "StateName", "StateId", true);
            }
        }

        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.StateCDSNumber)
        {
            statesTable = ApplicationCache.SelectCDSRequireStates();
            if (statesTable != null)
            {
                Helper.LoadDropDown(ddlState, statesTable, "StateName", "StateId", true);
            }

            if (!string.IsNullOrEmpty(hidID.Text))
                LoadPlaceHolder(int.Parse(hidID.Text));
            else
                LoadPlaceHolder();
        }
    }

    public override bool SaveData()
    {
        bool rtn = false;
        int cdsid = 0;
        bool isValid = true;
        if (!ValidateData())
        {
            return false;
        }
		//OHPNM-3209
        if (cdsDetail.Visible)//visible
        {
            foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadCDS.Controls)
            {
                //UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                isValid &= uploadDoc.ValidateData("valStateCDSNumber");
            }
            if (!isValid)
                return isValid;



            if (Page.IsValid)
            {
                try
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    if (!string.IsNullOrWhiteSpace(txtSTATE_CDS_Number.Text.ToString()))
                    {
                        parms.Add("STATE_CDS_Number", txtSTATE_CDS_Number.Text);
                    }
                    if (!string.IsNullOrWhiteSpace(ddlState.SelectedValue.ToString()))
                    {
                        parms.Add("State", ddlState.SelectedValue.ToString());
                    }
                    if (!string.IsNullOrWhiteSpace(txtDateIssued.Text.ToString()) && Convert.ToDateTime(txtDateIssued.Text) != DateTime.MinValue)
                    {
                        parms.Add("DateIssued", Convert.ToDateTime(txtDateIssued.Text).ToShortDateString());
                    }
                    else
                    {
                        parms.Add("DateIssued", null);
                    }
                    if (!string.IsNullOrWhiteSpace(txtExpirationDate.Text.ToString()) && Convert.ToDateTime(txtExpirationDate.Text) != DateTime.MinValue)
                    {
                        parms.Add("ExpirationDate", Convert.ToDateTime(txtExpirationDate.Text).ToShortDateString());
                    }
                    else
                    {
                        parms.Add("ExpirationDate", null);
                    }

                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

                    bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
                    if (isEdit)
                    {
                        parms.Add("REG_STATE_CDS_NUMBER_ID", hidID.Text);
                        svc.UpdateRegistrationDataTable("STATE_CDS_NUMBER", parms);
                        cdsid = int.Parse(hidID.Text);
                    }
                    else
                    {
                        cdsid = svc.InsertRegistrationDataTable("STATE_CDS_NUMBER", parms);
                    }

                    foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadCDS.Controls)
                    {
                        svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, cdsid);
                    }

                    return true;
                }
                catch (Exception ex) { throw ex; }
            }
        }
        else 
        {
            rtn = true;
        }
        return rtn;
    }

    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (cdsDetail.Visible)
        {
            if (!string.IsNullOrEmpty(txtSTATE_CDS_Number.Text) || !string.IsNullOrEmpty(ddlState.SelectedValue) || !string.IsNullOrEmpty(txtDateIssued.Text) || !string.IsNullOrEmpty(txtExpirationDate.Text))
            {
                isRequired = true;
            }
        }
        else
        {
            //OHPNM-3209 -- If "Add New" is not clicked, there are no fields on the screen to check if they are filled out.
            isRequired = true;
        }
        return isRequired;
    }

    public override bool ValidateData()
    {
        Page.Validate("valStateCDSNumber");
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valStateCDSNumber") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }
        return Page.IsValid;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "State CDS Number";
    }
    public override void LoadControlData()
    {
        LoadStateCDSNumber();
    }

    public override void LoadData(DataRow row)
    {
        int cdsID = 0;

        bool isEdit = row != null;

        cdsID = isEdit ? Helper.GetInt("REG_STATE_CDS_NUMBER_ID", row) : 0;

        LoadPlaceHolder(cdsID, isEdit, false, sectionName);

        if (isEdit)
        {

            txtSTATE_CDS_Number.Text = Helper.GetString("STATE_CDS_Number", row);
            ddlState.SelectedValue = Helper.GetString("State", row);
            txtDateIssued.Text = Helper.GetDate("DateIssued", row).ToString();
            txtExpirationDate.Text = Helper.GetDate("ExpirationDate", row).ToString();
        }
        else
        {
            InitFormFields();
        }

        hidID.Text = cdsID.ToString();
        hidIsEdit.Text = isEdit.ToString();
    }

    private void InitFormFields()
    {
        txtSTATE_CDS_Number.Text = string.Empty;
        ddlState.SelectedIndex = 0;
        txtDateIssued.Text = string.Empty;
        txtExpirationDate.Text = string.Empty;
        hidID.Text = string.Empty;
    }
    public void LoadPlaceHolder(int cdsId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadCDS.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, cdsId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, cdsId, pageSection, isEdit);

        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);
                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);
                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                ucUploadSectionControl.DestinationPath = @"C:\project\temp";
                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);
                ucUploadSectionControl.DocumentId = ds.Tables[i].Columns.Contains("DOCUMENT_ID") ? Helper.GetInt("DOCUMENT_ID", dr) : 0;
                ucUploadSectionControl.FileName = dr.Table.Columns.Contains("FILE_NAME") ? Helper.GetString("FILE_NAME", dr) : null;
                ucUploadSectionControl.RowId = cdsId;
                ucUploadSectionControl.SectionName = pageSection;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                PlaceholderUploadCDS.Controls.Add(ucUploadSectionControl);
            }
        }

    }

    private void LoadStateCDSNumber()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "STATE_CDS_NUMBER");
        DataTable dtStateCDSNumber = Helper.HasRows(ds) ? ds.Tables[0] : null;
        //this.DataList = dtStateCDSNumber;
        if (Helper.HasRows(ds)) grdCDS.DataSource = this.DataList = dtStateCDSNumber;
        else grdCDS.DataSource = this.DataList = null;
        grdCDS.DataBind();
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        cdsDetail.Visible = true;
        DataRow dr = this.DataList.Rows[index];

        if (Helper.HasRows(this.DataList))
            this.LoadData(dr);
        else
            this.LoadData(null);

    }

    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        cdsDetail.Visible = true;
        this.LoadData(null);
    }

    private void AddError(string errMsg, ref bool isGood, string ValidationGroup)
    {
        var val = new CustomValidator
        {
            IsValid = false,
            ErrorMessage = errMsg,
            ValidationGroup = ValidationGroup
        };
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public override string ValidationGroup
    {
        get { return "valStateCDSNumber"; }
    }

    public override string Title
    {
        get { return "State CDS Number"; }
    }

    public override string IdText
    {
        get { return "ucCDS_" + this.WorkflowPage.RegistrationId; }
    }

}